using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Web;

namespace Giant.Net
{
    public class HttpService : BaseService
    {
        private HttpListener httpListener;

        /// <summary>
        /// 所有客户端连接信息
        /// </summary>
        private Dictionary<long, HttpChannel> channels = new Dictionary<long, HttpChannel>();
        public Dictionary<long, HttpChannel> Channels { get { return channels; } }

        public HttpService()
        {
        }

        public HttpService(List<string> prefixes, Action<BaseChannel> onAcceptCallback)
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

                HttpChannel channel = new HttpChannel(socketContext, this);

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
            catch (Exception e)
            {
                Console.WriteLine(e);
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

            HttpChannel channel = new HttpChannel(webSocket, this);
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
            if (channels.TryGetValue(id, out HttpChannel channel))
            {
                channel.Dispose();
                channels.Remove(id);
            }
        }
    }
}
