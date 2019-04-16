using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{

    public class TcpService : BaseService
    {
        private SocketAsyncEventArgs innerArgs = new SocketAsyncEventArgs();

        /// <summary>
        /// 所有客户端连接信息
        /// </summary>
        private Dictionary<uint, TcpProtocol> protocols = new Dictionary<uint, TcpProtocol>();
        public Dictionary<uint, TcpProtocol> Protocols { get { return protocols; } }

        public TcpService()
        {
        }

        public TcpService(IPEndPoint endPoint, Action<BaseProtocol> onAcceptCallback)
        {
            this.OnAccept += onAcceptCallback;
            innerArgs.Completed += OnComplete;

            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            this.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.Socket.Bind(endPoint);
            this.Socket.Listen(200);

            this.AcceptAsync();
        }


        public override void Update()
        {
            foreach (var kv in protocols)
            {
                kv.Value.Update();
            }
        }

        public override BaseProtocol GetProtocol(uint id)
        {
            if (protocols.TryGetValue(id, out TcpProtocol protocol))
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
            TcpProtocol protocol = new TcpProtocol(endPoint, this);

            protocols[protocol.Id] = protocol;

            return protocol;
        }

        private void AcceptAsync()
        {
            try
            {
                if (Socket == null)
                {
                    return;
                }

                innerArgs.AcceptSocket = null;
                if (this.Socket.AcceptAsync(innerArgs))
                {
                    return;
                }

                AcceptComplete(innerArgs);
            }
            catch(Exception ex)
            {
            }
        }

        private void OnComplete(object sender, SocketAsyncEventArgs eventArgs)
        {
            switch (eventArgs.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    AcceptComplete(eventArgs);
                    break;
            }
        }

        private void AcceptComplete(SocketAsyncEventArgs eventArgs)
        {
            if (eventArgs.LastOperation == SocketAsyncOperation.Accept && eventArgs.SocketError == SocketError.Success)
            {
                TcpProtocol protocol = new TcpProtocol(eventArgs.AcceptSocket, this);

                protocols[protocol.Id] = protocol;

                this.Accept(protocol);
            }

            AcceptAsync();
        }



    }
}
