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
        private Dictionary<uint, TcpChannel> channels = new Dictionary<uint, TcpChannel>();
        public Dictionary<uint, TcpChannel> Channels { get { return channels; } }

        public TcpService()
        {
        }

        public TcpService(IPEndPoint endPoint, Action<BaseChannel> onAcceptCallback)
        {
            this.OnAccept += onAcceptCallback;
            innerArgs.Completed += OnComplete;

            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.Socket.Bind(endPoint);
            this.Socket.Listen(200);

            this.AcceptAsync();
        }


        public override void Update()
        {
            foreach (var kv in channels)
            {
                kv.Value.Update();
            }
        }

        public override BaseChannel GetChannel(uint id)
        {
            if (channels.TryGetValue(id, out TcpChannel channel))
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

        /// <summary>
        /// 创建一个通讯对象
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public override BaseChannel CreateChannel(IPEndPoint endPoint)
        {
            TcpChannel channel = new TcpChannel(endPoint, this);

            channels[channel.Id] = channel;

            return channel;
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
                TcpChannel channel = new TcpChannel(eventArgs.AcceptSocket, this);

                channels[channel.Id] = channel;

                this.Accept(channel);
            }

            AcceptAsync();
        }



    }
}
