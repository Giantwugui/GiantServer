using Giant.Logger;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;

namespace Giant.Net
{
    public class WebChannel : BaseChannel
    {
        private readonly WebSocket webSocket;
        private readonly HttpListenerWebSocketContext socketContext;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly MemoryStream recvStream;
        private readonly MemoryStream sendStream;

        public override MemoryStream Stream => sendStream;

        public WebChannel(HttpListenerWebSocketContext socketContext, WebService service) : base(service, ChannelType.Accepter)
        {
            IsConnected = true;
            this.socketContext = socketContext;
            webSocket = socketContext.WebSocket;
            recvStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            sendStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
        }

        public WebChannel(WebSocket webSocket, WebService service) : base(service, ChannelType.Connecter)
        {
            IsConnected = false;
            this.webSocket = webSocket;
            recvStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
            sendStream = service.MemoryStreamManager.GetStream("message", ushort.MaxValue);
        }

        public override void Start()
        {
            if (!IsConnected)
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
                OnError(ex);
            }
        }

        public async void ConnectAsync(string url)
        {
            try
            {
                await ((ClientWebSocket)webSocket).ConnectAsync(new Uri(url), cancellationTokenSource.Token);
                IsConnected = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public override void Update()
        {
        }

        public override void Dispose()
        {
            IsConnected = false;
            webSocket.Dispose();
        }

        protected override void OnError(object error)
        {
            base.OnError(error);

            IsConnected = false;
        }

        private async void StartRecv()
        {
            if (!IsConnected)
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
                        receiveResult = await webSocket.ReceiveAsync(
                            new Memory<byte>(recvStream.GetBuffer(), receiveCount, recvStream.Capacity - receiveCount),
                            cancellationTokenSource.Token);

                        if (!IsConnected)
                        {
                            return;
                        }

                        receiveCount += receiveResult.Count;
                    }
                    while (!receiveResult.EndOfMessage);

                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        OnError(ErrorCode.WebsocketPeerReset);
                        return;
                    }

                    if (receiveCount > ushort.MaxValue)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.MessageTooBig, $"message too big: {receiveCount}", cancellationTokenSource.Token);
                        OnError(ErrorCode.WebsocketMessageTooBig);
                        return;
                    }

                    recvStream.SetLength(receiveResult.Count);
                    OnRead(recvStream);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                OnError(ErrorCode.WebsocketRecvError);
            }
        }
    }
}
