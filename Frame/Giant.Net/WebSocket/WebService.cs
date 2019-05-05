using Giant.Log;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;

namespace Giant.Net
{
    public class WebService : BaseService
    {
        private HttpListener httpListener;
        public readonly RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();

        /// <summary>
        /// 所有客户端连接信息
        /// </summary>
        private Dictionary<long, WebChannel> channels = new Dictionary<long, WebChannel>();
        public Dictionary<long, WebChannel> Channels { get { return channels; } }

        public WebService()
        {
        }

        public WebService(List<string> prefixes, Action<BaseChannel> onAcceptCallback)
        {
            this.OnAccept += onAcceptCallback;

            httpListener = new HttpListener();
            prefixes.ForEach(prefixe => httpListener.Prefixes.Add(prefixe));

            AcceptAsync();
        }

        public async void AcceptAsync()
        {
            try
            {
                httpListener.Start();

                HttpListenerContext context = await httpListener.GetContextAsync();

                HttpListenerWebSocketContext socketContext = await context.AcceptWebSocketAsync(null);

                WebChannel channel = new WebChannel(socketContext, this);

                this.Accept(channel);

                channels[channel.Id] = channel;

                AcceptAsync();
            }
            catch (HttpListenerException e)
            {
                if (e.ErrorCode == 5)
                {
                    throw new Exception($"CMD管理员中输入: netsh http add urlacl url=http://*:8080/ user=Everyone", e);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public override BaseChannel GetChannel(uint id)
        {
            if (channels.TryGetValue(id, out var channel))
            {
                return channel;
            }

            return null;
        }

        public override BaseChannel CreateChannel(string address)
        {
            ClientWebSocket webSocket = new ClientWebSocket();

            WebChannel channel = new WebChannel(webSocket, this);
            channel.ConnectAsync(address);

            return channel;
        }

        public override BaseChannel CreateChannel(IPEndPoint endPoint)
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
        }

        public override void Remove(long id)
        {
            if (channels.TryGetValue(id, out WebChannel channel))
            {
                channel.Dispose();
                channels.Remove(id);
            }
        }
    }
}
