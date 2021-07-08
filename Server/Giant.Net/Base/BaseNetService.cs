using System;
using System.Net;
using System.Net.Sockets;
using Giant.Core;

namespace Giant.Net
{
    public abstract class BaseNetService : Entity
    {
        protected Socket Socket { get; set; }

        /// <summary>
        /// 用于对 将 Tcp，Udp 等其他通讯方式 回话统一封装成会话对象Session，提供无差别会话对象
        /// </summary>
        private Action<BaseChannel> acceptCallback;
        public event Action<BaseChannel> AcceptCallback
        {
            add => acceptCallback += value;
            remove => acceptCallback -= value;
        }

        public abstract BaseChannel GetChannel(uint id);

        public abstract BaseChannel CreateChannel(string address);

        public abstract BaseChannel CreateChannel(IPEndPoint endPoint);

        public abstract void Update();

        public abstract void Remove(long id);

        protected virtual void Accept(BaseChannel channel)
        {
            acceptCallback?.Invoke(channel);
        }
    }
}
