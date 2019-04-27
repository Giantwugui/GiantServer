using System;
using System.Net.Sockets;

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
        public uint Id { get; private set; }

        public ChannelType ChannelType { get; private set; }

        public BaseService Service { get; private set; }

        public bool IsConnected { get; protected set; }

        private Action<object> onError;
        public event Action<object> OnError
        {
            add { onError += value; }
            remove { onError -= value; }
        }

        private Action<byte[]> onRead;
        public event Action<byte[]> OnRead
        {
            add { onRead += value; }
            remove { onRead -= value; }
        }

        public BaseChannel(BaseService service, ChannelType type)
        {
            this.Id = IdGenerator.NewId;
            this.ChannelType = type;
            this.Service = service;
        }

        public BaseChannel(uint id, BaseService service, ChannelType type)
        {
            this.ChannelType = type;
            this.Service = service;
        }

        public abstract void Update();

        public abstract void Connect();

        /// <summary>
        /// 转发消息
        /// </summary>
        /// <param name="message">消息体</param>
        public abstract void Write(byte[] message);
 

        public virtual void Start()
        {
        }

        public virtual void Dispose()
        {
            this.Service.Remove(this.Id);
        }


        protected void Read(byte[] message)
        {
            onRead?.Invoke(message);
        }

        protected virtual void Error(object error)
        {
            onError?.Invoke(error);
        }

    }
}
