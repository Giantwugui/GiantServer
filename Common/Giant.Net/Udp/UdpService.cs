using Giant.Share;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{
    public class UdpChannelState
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
        private Dictionary<uint, UdpChannel> channels = new Dictionary<uint, UdpChannel>();
        public Dictionary<uint, UdpChannel> Channels { get { return channels; } }

        /// <summary>
        /// 所有等待连接的客户端
        /// </summary>
        private Dictionary<uint, UdpChannel> waitConnectClients = new Dictionary<uint, UdpChannel>();

        private EndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);


        public UdpService()
        {
            //指定端口为0则系统会随机绑定可用端口
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket.Bind(endPoint);
        }

        public UdpService(int port, Action<BaseChannel> onAcceptCallback)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket.Bind(endPoint);

            OnAccept += onAcceptCallback;
        }


        public override void Update()
        {
            Receive();

            foreach (var kv in channels)
            {
                kv.Value.Update();
            }
        }

        public override BaseChannel GetChannel(uint id)
        {
            if (channels.TryGetValue(id, out UdpChannel channel))
            {
                return channel;
            }

            return null;
        }

        public override void Remove(uint id)
        {
            if (channels.TryGetValue(id, out var channel))
            {
                channel.Dispose();

                channels.Remove(id);
            }
        }

        public override BaseChannel CreateChannel(string address)
        {
            IPEndPoint endPoint = NetworkHelper.ToIPEndPoint(address);

            return CreateChannel(endPoint);
        }

        /// <summary>
        /// 创建一个通讯对象
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public override BaseChannel CreateChannel(IPEndPoint endPoint)
        {
            uint udpId = IdGenerator.NewId;

            if (channels.TryGetValue(udpId, out UdpChannel channel))
            {
                channel.Dispose();

                channels.Remove(udpId);
            }

            channel = new UdpChannel((uint)udpId, Socket, endPoint, this);

            channels[udpId] = channel;
            waitConnectClients[udpId] = channel;

            return channel;
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

                    UdpChannel channel = null;
                    byte flag = tempBuffer[0];

                    switch (flag)
                    {
                        case UdpChannelState.SYN://请求建立连接
                            {
                                // UdpProtocolType + remoteUdp
                                if (msgLength != 5)
                                {
                                    break;
                                }

                                uint remoteUdp = BitConverter.ToUInt32(tempBuffer, 1);
                                if (waitConnectClients.TryGetValue(remoteUdp, out UdpChannel client))
                                {
                                    waitConnectClients.Remove(remoteUdp);
                                }

                                channel = new UdpChannel(IdGenerator.NewId, remoteUdp, this.Socket, (IPEndPoint)remoteIPEndPoint, this);

                                remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

                                channels[channel.Id] = channel;
                                waitConnectClients[channel.RemoteUdp] = channel;

                                this.Accept(channel);
                            }
                            break;
                        case UdpChannelState.ACK://建立连接
                            {
                                // UdpProtocolType + remoteUdp +localUdp 1+4+4
                                if (msgLength != 9)
                                {
                                    break;
                                }

                                uint remoteUdp = BitConverter.ToUInt32(tempBuffer, 1);
                                uint localUdp = BitConverter.ToUInt32(tempBuffer, 5);

                                channel = (UdpChannel)GetChannel(localUdp);

                                if (channel != null)
                                {
                                    channel.OnConnected(remoteUdp);
                                }
                            }
                            break;
                        case UdpChannelState.MSG://收到消息
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

                                channel = (UdpChannel)GetChannel(localUdp);
                                if (channel != null)
                                {
                                    if (channel.RemoteUdp == remoteUdp)
                                    {
                                        channel.OnReceive(tempBuffer, 9, msgLength - 9);
                                    }
                                    else
                                    {
                                        channel.Dispose();
                                        channels.Remove(channel.Id);
                                    }
                                }

                                waitConnectClients.Remove(channel.RemoteUdp);
                            }
                            break;
                        case UdpChannelState.FIN://断开连接
                            {
                                if (msgLength != 9)
                                {
                                    break;
                                }

                                uint remoteUdp = BitConverter.ToUInt32(tempBuffer, 1);
                                uint localUdp = BitConverter.ToUInt32(tempBuffer, 5);

                                channel = (UdpChannel)GetChannel(localUdp);

                                if (channel != null)
                                {
                                    if (channel.RemoteUdp == remoteUdp)
                                    {
                                        channel.Dispose();
                                        channels.Remove(channel.Id);
                                        waitConnectClients.Remove(channel.RemoteUdp);
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
