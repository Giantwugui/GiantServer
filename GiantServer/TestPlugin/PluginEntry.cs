using GiantCore;
using GiantNode;
using System.Collections.Generic;
using System.Text;

namespace TestPlugin
{
    [PluginEntryAttribute("TestPlugin", false)]
    class PluginEntry : IPlugin
    {
        public PluginEntry()
        {
            mEvent = new NodeEvents();
            mEvent.OnNodeInit += OnInit;
            mEvent.OnNodeCrash += OnCrash;
            mEvent.OnNodeClosed += OnClosed;
            mEvent.OnHandle += OnHandle;
            mEvent.OnNodeUpdate += OnUpdate;
            mEvent.OnNodeInsideHandle += OnInsideHandle;
        }

        public void OnInit(Dictionary<string, string> param)
        {
        }

        public void OnClosed()
        {
        }

        public void OnCrash()
        {
        }

        public void OnHandle(Session session, byte[] message)
        {
            string strReceiveData = Encoding.Unicode.GetString(message);

            if (!string.IsNullOrEmpty(strReceiveData))
            {
                //回传给客户端消息发送成功
                session.Return("Send Success!");
            }
        }

        public void OnInsideHandle(uint fromNode, byte[] message)
        {
        }

        /// <summary>
        /// 心跳循环
        /// </summary>
        public void OnUpdate(float time)
        {
        }

        #region 实现基类接口

        public NodeEvents Events
        {
            get { return mEvent; }
        }

        public Dictionary<string, string> GetCommandSet()
        {
            return new Dictionary<string, string>();
        }

        #endregion

        NodeEvents mEvent = null;
    }
}
