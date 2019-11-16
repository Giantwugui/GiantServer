using Giant.Log;
using Giant.Share;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{

    public class TcpService : BaseNetService
    {
        private readonly SocketAsyncEventArgs innerArgs = new SocketAsyncEventArgs();
        public readonly RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();

        /// <summary>
        /// 所有客户端连接信息
        /// </summary>
        private Dictionary<long, TcpChannel> channels = new Dictionary<long, TcpChannel>();
        public Dictionary<long, TcpChannel> Channels { get { return channels; } }


        public int PacketSizeLength { get; set; }

        public TcpService(int packetSizeLength)
        {
            PacketSizeLength = packetSizeLength;
        }

        public TcpService(int packetSizeLength, IPEndPoint endPoint, Action<BaseChannel> onAcceptCallback)
        {
            PacketSizeLength = packetSizeLength;

            OnAccept += onAcceptCallback;
            innerArgs.Completed += OnComplete;

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Socket.Bind(endPoint);
            Socket.Listen(1000);

            AcceptAsync();
        }


        public override void Update()
        {
        }

        public override BaseChannel GetChannel(uint id)
        {
            if (channels.TryGetValue(id, out TcpChannel channel))
            {
                return channel;
            }

            return null;
        }

        public override void Remove(long id)
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
            TcpChannel channel = new TcpChannel(Packet.PacketSizeLength2, endPoint, this);

            channels[channel.InstanceId] = channel;

            return channel;
        }

        private void AcceptAsync()
        {
            if (Socket == null)
            {
                return;
            }

            innerArgs.AcceptSocket = null;
            if (Socket.AcceptAsync(innerArgs))
            {
                return;
            }

            AcceptComplete(innerArgs);
        }

        private void OnComplete(object sender, SocketAsyncEventArgs eventArgs)
        {
            switch (eventArgs.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    OneThreadSynchronizationContext.Instance.Post(AcceptComplete, eventArgs);
                    //AcceptComplete(eventArgs);
                    break;
            }
        }

        private void AcceptComplete(object args)
        {
            SocketAsyncEventArgs eventArgs = (SocketAsyncEventArgs)args;

            if (eventArgs.SocketError != SocketError.Success)
            {
                Logger.Error(eventArgs.SocketError);
                AcceptAsync();
                return;
            }

            try
            {
                TcpChannel channel = new TcpChannel(Packet.PacketSizeLength2, eventArgs.AcceptSocket, this);

                channels[channel.InstanceId] = channel;

                Accept(channel);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                AcceptAsync();
            }
        }



    }
}
