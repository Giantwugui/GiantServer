using Giant.Share;
using System;
using System.Net;
using System.Net.Sockets;

namespace Giant.Net
{
    public abstract class BaseService : IUpdate
    {
        public Socket Socket { get; protected set; }

        /// <summary>
        /// 用于对 将 Tcp，Udp 等其他通讯方式 回话统一封装成会话对象Session，提供无差别会话对象
        /// </summary>
        private Action<BaseProtocol> onAccept;
        public event Action<BaseProtocol> OnAccept
        {
            add { onAccept += value; }
            remove { onAccept -= value; }
        }

        public abstract BaseProtocol GetProtocol(uint id);

        public abstract BaseProtocol CreateProtocol(IPEndPoint endPoint);

        public abstract void Update();

        public abstract void Remove(uint id);

        public virtual void Accept(BaseProtocol protocol)
        {
            onAccept?.Invoke(protocol);
        }

    }
}
