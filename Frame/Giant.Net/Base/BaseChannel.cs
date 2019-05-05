using Giant.Share;
using System;
using System.IO;

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
        public long Id { get; private set; }

        public ChannelType ChannelType { get; private set; }

        public BaseService Service { get; private set; }

        public abstract MemoryStream Stream { get; }

        public bool IsConnected { get; protected set; }

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

        public BaseChannel(BaseService service, ChannelType type)
        {
            this.Id = IdGenerator.NewId;
            this.ChannelType = type;
            this.Service = service;
        }

        public BaseChannel(long id, BaseService service, ChannelType type)
        {
            this.ChannelType = type;
            this.Service = service;
        }

        public abstract void Update();

        public abstract void Connect();

        /// <summary>
        /// 转发消息
        /// </summary>
        public abstract void Send(MemoryStream memoryStream);
 

        public virtual void Start()
        {
        }

        public virtual void Dispose()
        {
            this.Service.Remove(this.Id);
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
