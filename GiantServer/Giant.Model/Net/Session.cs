using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Giant.Model
{
    //所有的通讯消息都使用 protobuf 格式

    public class SessionAwakeSystem : AwakeSystem<Session, TChannel>
    {
        public override void Awake(Session self, TChannel a)
        {
            self.Awake(a);
        }
    }


    /// <summary>
    /// 会话：用户处理客户端，服务端的消息
    /// </summary>
    public class Session : Entity
    {
        /// <summary>
        /// 所有的请求消息
        /// </summary>
        private Dictionary<int, Action<IResponse>> requestCallBack = new Dictionary<int, Action<IResponse>>();

        private TChannel Channel { get; set; }

        private int RpcId { get; set; }

        private NetComponent Network
        {
            get { return GetParent<NetComponent>(); }
        }


        public void Awake(TChannel channel)
        {
            this.Channel = channel;

            this.Channel.OnErrorCallBack += (aChannel, error) => 
            {
                Network.Remove(this.Id);
            };

            Channel.OnReceiveCallBack += OnRevceived;
        }


        public void Start()
        {
            this.Channel.Start();
        }

        private void OnRevceived(BChannel channel, byte[] message)
        {
            
        }

        public void Send(IRequest request)
        {
            Send(0x01, request);
        }

        public async Task<IResponse> Call(IRequest request)
        {
            int rpcId = ++RpcId;

            TaskCompletionSource<IResponse> tcs = new TaskCompletionSource<IResponse>();

            requestCallBack[rpcId] = (response) =>
            {
                tcs.SetResult(response);
            };

            request.RpcId = rpcId;

            Send(0x00, request);

            return await tcs.Task;
        }


        private void Send(int flag, IRequest request)
        {
        }

    }
}
