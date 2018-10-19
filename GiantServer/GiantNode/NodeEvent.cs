using System;
using System.Collections.Generic;
using GiantCore;

namespace GiantNode
{
    public class NodeEvents
    {
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
        public Action<Session> Online;

        /// <summary>
        /// 下线事件
        /// </summary>
        public Action<Session> OnOffline;

        /// <summary>
        /// 连接事件
        /// </summary>
        public Action<bool> OnConnected;

        /// <summary>
        /// 消息接受时间
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
