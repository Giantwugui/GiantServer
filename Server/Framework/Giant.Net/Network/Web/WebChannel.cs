using System;
using System.Threading;
using System.Net.WebSockets;
using Giant.Log;
using System.IO;

namespace Giant.Net
{
    public class WebChannel : BaseChannel
    {
        private readonly WebSocket webSocket;
        private readonly HttpListenerWebSocketContext socketContext;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly MemoryStream recvStream;
        private readonly MemoryStream sendStream;

        public override MemoryStream Stream => this.sendStream;

        public WebChannel(HttpListenerWebSocketContext socketContext, WebService service) : base(service, ChannelType.Accepter)
        {
            this.IsConnected = true;
            this.socketContext = socketContext;
            this.webSocket = socketContext.WebSocket;
            this.recvStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.sendStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
        }

        public WebChannel(WebSocket webSocket, WebService service) : base(service, ChannelType.Connecter)
        {
            this.IsConnected = false;
            this.webSocket = webSocket;
            this.recvStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.sendStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
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

        public override async void Send(MemoryStream stream)
        {
            try
            {
                if (!IsConnected)
                {
                    return;
                }

                await webSocket.SendAsync(stream.GetBuffer(), WebSocketMessageType.Text, true, cancellationTokenSource.Token);
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
                    int receiveCount = 0;
                    ValueWebSocketReceiveResult receiveResult;

                    do
                    {
                        receiveResult = await this.webSocket.ReceiveAsync(
                            new Memory<byte>(this.recvStream.GetBuffer(), receiveCount, this.recvStream.Capacity - receiveCount),
                            cancellationTokenSource.Token);

                        if (!this.IsConnected)
                        {
                            return;
                        }

                        receiveCount += receiveResult.Count;
                    }
                    while (!receiveResult.EndOfMessage);

                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        this.OnError(ErrorCode.WebsocketPeerReset);
                        return;
                    }

                    if (receiveCount > ushort.MaxValue)
                    {
                        await this.webSocket.CloseAsync(WebSocketCloseStatus.MessageTooBig, $"message too big: {receiveCount}",cancellationTokenSource.Token);
                        this.OnError(ErrorCode.WebsocketMessageTooBig);
                        return;
                    }

                    this.recvStream.SetLength(receiveResult.Count);
                    this.OnRead(this.recvStream);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                this.OnError(ErrorCode.WebsocketRecvError);
            }
        }
    }
}
