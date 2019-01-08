using Giant.Net;
using System;
using System.Collections.Generic;

namespace GiantNode
{
    public class NodeEvents
    {
        public void Events_OnClosed()
        {
            if (OnClosed != null)
            {
                OnClosed();
            }
        }

        public void Events_OnCrash()
        {
            if (OnCrash != null)
            {
                OnCrash();
            }
        }
        public void Events_OnStartComplate()
        {
            if (OnStartComplate != null)
            {
                OnStartComplate();
            }
        }
        public void Events_OnUpdate(float delay)
        {
            if (OnUpdate != null)
            {
                OnUpdate(delay);
            }
        }

        public void Events_OnClientOnline(Session session)
        {
            if (OnClientOnline != null)
            {
                OnClientOnline(session);
            }
        }

        public void Events_OnClientOffline(Session session)
        {
            if (OnClientOffline != null)
            {
                OnClientOffline(session);
            }
        }

        public void Events_OnConnected(bool isConnected)
        {
            if (OnConnected != null)
            {
                OnConnected(isConnected);
            }
        }

        public void Events_OnReceiveMessage(byte[] message)
        {
            if (OnReceiveMessage != null)
            {
                OnReceiveMessage(message);
            }
        }

        public void Events_OnHandle(Session session, byte[] message)
        {
            OnHandle?.Invoke(session, message);
        }

        public void Events_OnInsideHandle(uint fromNode, byte[] message)
        {
            OnInsideHandle?.Invoke(fromNode, message);
        }

        public void Events_OnInit(Dictionary<string, string> param)
        {
            if (OnInit != null)
            {
                OnInit(param);
            }
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        public Action OnClosed;

        /// <summary>
        /// 崩溃事件
        /// </summary>
        public Action OnCrash;

        /// <summary>
        /// 启动完成事件
        /// </summary>
        public Action OnStartComplate;

        /// <summary>
        /// 节点更新事件
        /// </summary>
        public Action<float> OnUpdate;

        /// <summary>
        /// 上线事件
        /// </summary>
        public Action<Session> OnClientOnline;

        /// <summary>
        /// 下线事件
        /// </summary>
        public Action<Session> OnClientOffline;

        /// <summary>
        /// 连接事件
        /// </summary>
        public Action<bool> OnConnected;

        /// <summary>
        /// 消息接受事件
        /// </summary>
        public Action<byte[]> OnReceiveMessage;

        /// <summary>
        /// 消息派发事件
        /// </summary>
        public Action<Session, byte[]> OnHandle;

        /// <summary>
        /// 节点内部请求事件
        /// </summary>
        public Action<uint, byte[]> OnInsideHandle;

        /// <summary>
        /// 节点初始化事件
        /// </summary>
        public Action<Dictionary<string, string>> OnInit;

    }
}
