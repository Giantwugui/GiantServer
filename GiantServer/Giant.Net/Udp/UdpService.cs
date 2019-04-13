using Giant.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{
    public class UdpProtocolType
    {
        public const byte SYN = 1;//请求建立连接
        public const byte ACK = 2;//数据传输,请求连接回应
        public const byte MSG = 3;//正常消息发送
        public const byte FIN = 4;//断开连接
    }

    public class UdpService : BaseService
    {
        private const ushort contentLength = ushort.MaxValue;//最大发送消息长度

        /// <summary>
        /// 所有UPD客户端连接信息
        /// </summary>
        private Dictionary<uint, UdpProtocol> protocols = new Dictionary<uint, UdpProtocol>();
        public Dictionary<uint, UdpProtocol> Protocols { get { return protocols; } }

        /// <summary>
        /// 所有等待连接的客户端
        /// </summary>
        private Dictionary<uint, UdpProtocol> waitConnectClients = new Dictionary<uint, UdpProtocol>();

        private EndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);


        public UdpService()
        {
            //指定端口为0则系统会随机绑定可用端口
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
            Socket.Bind(endPoint);
        }

        public UdpService(int port, Action<BaseProtocol> onAcceptCallback)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
            Socket.Bind(endPoint);

            OnAccept += onAcceptCallback;
        }


        public override void Update()
        {
            Receive();

            foreach (var kv in protocols)
            {
                kv.Value.Update();
            }
        }

        public override BaseProtocol GetProtocol(uint id)
        {
            if (protocols.TryGetValue(id, out UdpProtocol protocol))
            {
                return protocol;
            }

            return null;
        }

        public override void Remove(uint id)
        {
            if (protocols.TryGetValue(id, out var protocol))
            {
                protocol.Dispose();

                protocols.Remove(id);
            }
        }

        /// <summary>
        /// 创建一个通讯对象
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public override BaseProtocol CreateProtocol(IPEndPoint endPoint)
        {
            uint udpId = IdGenerator.NewId;

            if (protocols.TryGetValue(udpId, out UdpProtocol protocol))
            {
                protocol.Dispose();

                protocols.Remove(udpId);
            }

            protocol = new UdpProtocol((uint)udpId, Socket, endPoint, this);

            protocols[udpId] = protocol;
            waitConnectClients[udpId] = protocol;

            return protocol;
        }


        private void Receive()
        {
            if (Socket.Available > 0)
            {
                try
                {
                    byte[] tempBuffer = new byte[contentLength];

                    int msgLength = Socket.ReceiveFrom(tempBuffer, ref remoteIPEndPoint);


                    if (msgLength <= 0)
                    {
                        return;
                    }

                    UdpProtocol protocol = null;
                    byte flag = tempBuffer[0];

                    switch (flag)
                    {
                        case UdpProtocolType.SYN://请求建立连接
                            {
                                // UdpProtocolType + remoteUdp
                                if (msgLength != 5)
                                {
                                    break;
                                }

                                uint remoteUdp = BitConverter.ToUInt32(tempBuffer, 1);
                                if (waitConnectClients.TryGetValue(remoteUdp, out UdpProtocol client))
                                {
                                    waitConnectClients.Remove(remoteUdp);
                                }

                                protocol = new UdpProtocol(IdGenerator.NewId, remoteUdp, this.Socket, (IPEndPoint)remoteIPEndPoint, this);

                                remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

                                protocols[protocol.Id] = protocol;
                                waitConnectClients[protocol.RemoteUdp] = protocol;

                                this.Accept(protocol);
                            }
                            break;
                        case UdpProtocolType.ACK://建立连接
                            {
                                // UdpProtocolType + remoteUdp +localUdp 1+4+4
                                if (msgLength != 9)
                                {
                                    break;
                                }

                                uint remoteUdp = BitConverter.ToUInt32(tempBuffer, 1);
                                uint localUdp = BitConverter.ToUInt32(tempBuffer, 5);

                                protocol = (UdpProtocol)GetProtocol(localUdp);

                                if (protocol != null)
                                {
                                    protocol.OnConnected(remoteUdp);
                                }
                            }
                            break;
                        case UdpProtocolType.MSG://收到消息
                            {
                                //不是有效的消息
                                if (msgLength < 9)
                                {
                                    break;
                                }

                                //没有做消息长度验证功能。。。。
                                //以及包体拆分传输的情况。。。。

                                uint remoteUdp = BitConverter.ToUInt32(tempBuffer, 1);
                                uint localUdp = BitConverter.ToUInt32(tempBuffer, 5);

                                protocol = (UdpProtocol)GetProtocol(localUdp);
                                if (protocol != null)
                                {
                                    if (protocol.RemoteUdp == remoteUdp)
                                    {
                                        protocol.OnReceive(tempBuffer, 9, msgLength - 9);
                                    }
                                    else
                                    {
                                        protocol.Dispose();
                                        protocols.Remove(protocol.Id);
                                    }
                                }

                                waitConnectClients.Remove(protocol.RemoteUdp);
                            }
                            break;
                        case UdpProtocolType.FIN://断开连接
                            {
                                if (msgLength != 9)
                                {
                                    break;
                                }

                                uint remoteUdp = BitConverter.ToUInt32(tempBuffer, 1);
                                uint localUdp = BitConverter.ToUInt32(tempBuffer, 5);

                                protocol = (UdpProtocol)GetProtocol(localUdp);

                                if (protocol != null)
                                {
                                    if (protocol.RemoteUdp == remoteUdp)
                                    {
                                        protocol.Dispose();
                                        protocols.Remove(protocol.Id);
                                        waitConnectClients.Remove(protocol.RemoteUdp);
                                    }
                                }
                            }
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

    }
}
