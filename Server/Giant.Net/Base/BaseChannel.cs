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
        private Action<object> onErrorCallback;
        private Action<bool> onConnectCallback;
        private Action<MemoryStream> onReadCallback;

        public ChannelType ChannelType { get; private set; }
        public bool IsConnected { get; protected set; }
        public IPEndPoint IPEndPoint { get; protected set; }
        public BaseNetService Service { get; private set; }

        public abstract MemoryStream Stream { get; }

        protected BaseChannel(BaseNetService service, ChannelType type)
        {
            InstanceId = IdGenerator.NewId;
            ChannelType = type;
            Service = service;
        }

        protected BaseChannel(long id, BaseNetService service, ChannelType type)
        {
            InstanceId = id;
            ChannelType = type;
            Service = service;
        }

        public abstract void Update();

        public abstract void Connect();

        public abstract void Send(MemoryStream stream);

        public virtual void Start()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            Service.Remove(InstanceId);
        }

        public void RegistReadCallback(Action<MemoryStream> readCallback)
        {
            onReadCallback += readCallback;
        }

        public void RegistErrorCallback(Action<object> errorCallback)
        {
            onErrorCallback += errorCallback;
        }

        public void RegistConnectCallback(Action<bool> errorCallback)
        {
            onConnectCallback += errorCallback;
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
