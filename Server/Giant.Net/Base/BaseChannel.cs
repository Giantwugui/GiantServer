using Giant.Core;
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
    public abstract class BaseChannel : Component
    {
        public ChannelType ChannelType { get; private set; }
        public bool IsConnected { get; protected set; }
        public IPEndPoint IPEndPoint { get; protected set; }
        public BaseNetService Service { get; private set; }

        public abstract MemoryStream Stream { get; }

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
            InstanceId = IdGenerator.NewId;
            ChannelType = type;
            Service = service;
        }

        public BaseChannel(long id, BaseNetService service, ChannelType type)
        {
            InstanceId = id;
            ChannelType = type;
            Service = service;
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

        public override void Dispose()
        {
            base.Dispose();
            Service.Remove(InstanceId);
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
