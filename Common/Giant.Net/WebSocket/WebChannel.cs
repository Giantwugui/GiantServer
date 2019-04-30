using System;
using System.Threading;
using System.Net.WebSockets;
using Giant.Log;

namespace Giant.Net
{
    public class WebChannel : BaseChannel
    {
        private const ushort contentLength = ushort.MaxValue;//最大发送消息长度

        private WebSocket webSocket;
        private readonly HttpListenerWebSocketContext socketContext;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        private readonly byte[] reveiveBuffer = new byte[contentLength];
        

        public WebChannel(HttpListenerWebSocketContext socketContext, WebService service) : base(service, ChannelType.Accepter)
        {
            this.webSocket = socketContext.WebSocket;
            this.socketContext = socketContext;
            this.IsConnected = true;
        }

        public WebChannel(WebSocket webSocket, WebService service) : base(service, ChannelType.Connecter)
        {
            this.IsConnected = false;
            this.webSocket = webSocket;
        }

        public override void Start()
        {
            if (!this.IsConnected)
            {
                return;
            }

            ReceiveAsync();
        }

        /// <summary>
        /// 建立连接
        /// </summary>
        public override void Connect()
        {
        }

        public override async void Send(byte[] message)
        {
            try
            {
                if (!IsConnected)
                {
                    return;
                }

                await webSocket.SendAsync(message, WebSocketMessageType.Text, true, tokenSource.Token);
            }
            catch (Exception ex)
            {
                this.Error(ex);
            }
        }

        public async void ConnectAsync(string url)
        {
            try
            {
                await ((ClientWebSocket)webSocket).ConnectAsync(new Uri(url), tokenSource.Token);
                this.IsConnected = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public override void Update()
        {
        }

        public override void Dispose()
        {
            this.IsConnected = false;
            webSocket.Dispose();
        }

        protected override void Error(object error)
        {
            base.Error(error);

            this.IsConnected = false;
        }

        private async void ReceiveAsync()
        {
            try
            {
                //持续接收需要 保留偏移量以便于做数据拼接，这里只是处理了每次数据一次传输完成的情况
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(reveiveBuffer, tokenSource.Token);

                //接收过程中有报错
                if (result.EndOfMessage)
                {
                    byte[] content = new byte[result.Count];

                    Array.Copy(reveiveBuffer, 0, content, 0, result.Count);

                    this.Read(content);
                }

                this.ReceiveAsync();
            }
            catch (Exception ex)
            {
                this.Error(webSocket.State);
                Logger.Error(ex);
            }
        }
    }
}
