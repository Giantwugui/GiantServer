using System;
using System.Threading;
using System.Net.WebSockets;
using Giant.Log;
using System.IO;

namespace Giant.Net
{
    public class WebChannel : BaseChannel
    {
        private const ushort contentLength = ushort.MaxValue;//最大发送消息长度

        private readonly WebSocket webSocket;
        private readonly HttpListenerWebSocketContext socketContext;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly MemoryStream recvStream;
        private readonly MemoryStream memoryStream;

        public override MemoryStream Stream => this.memoryStream;

        public WebChannel(HttpListenerWebSocketContext socketContext, WebService service) : base(service, ChannelType.Accepter)
        {
            this.IsConnected = true;
            this.socketContext = socketContext;
            this.webSocket = socketContext.WebSocket;
            this.recvStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
        }

        public WebChannel(WebSocket webSocket, WebService service) : base(service, ChannelType.Connecter)
        {
            this.IsConnected = false;
            this.webSocket = webSocket;
            this.recvStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.memoryStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
        }

        public override void Start()
        {
            if (!this.IsConnected)
            {
                return;
            }

            StartRecv();
        }

        /// <summary>
        /// 建立连接
        /// </summary>
        public override void Connect()
        {
        }

        public override async void Send(MemoryStream memoryStream)
        {
            try
            {
                if (!IsConnected)
                {
                    return;
                }

                await webSocket.SendAsync(memoryStream.GetBuffer(), WebSocketMessageType.Text, true, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                this.OnError(ex);
            }
        }

        public async void ConnectAsync(string url)
        {
            try
            {
                await ((ClientWebSocket)webSocket).ConnectAsync(new Uri(url), cancellationTokenSource.Token);
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

        protected override void OnError(object error)
        {
            base.OnError(error);

            this.IsConnected = false;
        }

        private async void StartRecv()
        {
            if (!this.IsConnected)
            {
                return;
            }

            try
            {
                while (true)
                {
#if SERVER
                    ValueWebSocketReceiveResult receiveResult;
#else
                    WebSocketReceiveResult receiveResult;
#endif
                    int receiveCount = 0;
                    do
                    {
#if SERVER
                        receiveResult = await this.webSocket.ReceiveAsync(
                            new Memory<byte>(this.recvStream.GetBuffer(), receiveCount, this.recvStream.Capacity - receiveCount),
                            cancellationTokenSource.Token);
#else
                        receiveResult = await this.webSocket.ReceiveAsync(
                            new ArraySegment<byte>(this.recvStream.GetBuffer(), receiveCount, this.recvStream.Capacity - receiveCount),
                            cancellationTokenSource.Token);
#endif
                        if (!this.IsConnected)
                        {
                            return;
                        }

                        receiveCount += receiveResult.Count;
                    }
                    while (!receiveResult.EndOfMessage);

                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        this.OnError(ErrorCode.ERR_WebsocketPeerReset);
                        return;
                    }

                    if (receiveResult.Count > ushort.MaxValue)
                    {
                        await this.webSocket.CloseAsync(WebSocketCloseStatus.MessageTooBig, $"message too big: {receiveResult.Count}",cancellationTokenSource.Token);
                        this.OnError(ErrorCode.ERR_WebsocketMessageTooBig);
                        return;
                    }

                    this.recvStream.SetLength(receiveResult.Count);
                    this.OnRead(this.recvStream);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                this.OnError(ErrorCode.ERR_WebsocketRecvError);
            }
        }
    }
}
