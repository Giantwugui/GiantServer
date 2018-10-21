using GiantCore;
using GiantNode;
using NetMQ;
using NetMQ.Sockets;
using System.Collections.Generic;
using System.Text;

namespace TestPlugin
{
    [PluginEntryAttribute("TestPlugin", true)]
    class PluginEntry : IPlugin
    {
        public PluginEntry()
        {
            mEvent = new NodeEvents();
            mEvent.OnNodeInit += OnNodeInit;
            mEvent.OnNodeCrash += OnCrash;
            mEvent.OnNodeClosed += OnClosed;
            mEvent.OnNodeUpdate += OnNodeUpdate;
            mEvent.OnNodeInsideHandle += OnNodeInsideHandle;
            mEvent.OnNodeStartComplate += OnNodeStartComplate;
        }

        public void OnNodeInit(Dictionary<string, string> param)
        {
            mPushSocket = new PushSocket("ipc://NodeServer_1_1");
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

        /// <summary>
        /// 心跳循环
        /// </summary>
        private void OnNodeUpdate(float time)
        {
            InnerMessage message = new InnerMessage()
            {
                MessageType = MessageType.InnerMessage,
                ToNode = 1,
                Content = Encoding.UTF8.GetBytes("test")
            };

            mPushSocket.SendFrame(message.ToJson());
        }

        private void OnNodeStartComplate()
        {

        }

        private void OnNodeInsideHandle(uint fromNode, byte[] message)
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

        PushSocket mPushSocket = null;

        NodeEvents mEvent = null;
    }
}
