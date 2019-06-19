using Giant.Share;
using System;
using System.IO;
using System.Net;

namespace Giant.Net
{
    public enum ChannelType
    {
        Accepter = 1,
        Connecter = 2,
    }

    /// <summary>
    /// 通讯类型抽象类
    /// </summary>
    public abstract class BaseChannel: IDisposable
    {
        public long InstanceId { get; private set; }

        public ChannelType ChannelType { get; private set; }

        public IPEndPoint IPEndPoint { get; protected set; }

        public BaseNetService Service { get; private set; }

        public abstract MemoryStream Stream { get; }

        public bool IsConnected { get; protected set; }

        private Action<bool> onConnectCallback;
        public event Action<bool> OnConnectCallback
        {
            add { onConnectCallback += value; }
            remove { onConnectCallback -= value; }
        }

        private Action<object> onErrorCallback;
        public event Action<object> OnErrorCallback
        {
            add { onErrorCallback += value; }
            remove { onErrorCallback -= value; }
        }

        private Action<MemoryStream> onReadCallback;
        public event Action<MemoryStream> OnReadCallback
        {
            add { onReadCallback += value; }
            remove { onReadCallback -= value; }
        }

        public BaseChannel(BaseNetService service, ChannelType type)
        {
            this.InstanceId = IdGenerator.NewId;
            this.ChannelType = type;
            this.Service = service;
        }

        public BaseChannel(long id, BaseNetService service, ChannelType type)
        {
            this.InstanceId = id;
            this.ChannelType = type;
            this.Service = service;
        }

        public abstract void Update();

        public abstract void Connect();

        /// <summary>
        /// 转发消息
        /// </summary>
        public abstract void Send(MemoryStream stream);
 

        public virtual void Start()
        {
        }

        public virtual void Dispose()
        {
            this.Service.Remove(this.InstanceId);
        }

        protected void OnConnected(bool connect)
        {
            onConnectCallback?.Invoke(connect);
        }

        protected void OnRead(MemoryStream memoryStream)
        {
            onReadCallback?.Invoke(memoryStream);
        }

        protected virtual void OnError(object error)
        {
            onErrorCallback?.Invoke(error);
        }

    }
}
