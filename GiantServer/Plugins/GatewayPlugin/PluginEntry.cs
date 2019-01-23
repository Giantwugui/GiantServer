using Giant.Core;
using Giant.Framework;
using Giant.Net;
using NetMQ;
using NetMQ.Sockets;
using System.Collections.Generic;
using System.Text;

namespace GatewayPlugin
{
    [PluginEntry("GatewayPlugin", true)]
    class PluginEntry : IPlugin
    {
        public PluginEntry()
        {
            mEvent = new NodeEvents();
            mEvent.OnInit += OnNodeInit;
            mEvent.OnCrash += OnCrash;
            mEvent.OnClosed += OnClosed;
            mEvent.OnHandle += OnHandle;
            mEvent.OnUpdate += OnNodeUpdate;
            mEvent.OnInsideHandle += OnNodeInsideHandle;
            mEvent.OnStartComplate += OnNodeStartComplate;
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
        }

        private void OnNodeStartComplate()
        {
            InnerMessage message = new InnerMessage(1, InnerMessageType.Inner, Encoding.UTF8.GetBytes("test"));

            mPushSocket.SendFrame(message.ToProtoString());
        }

        private void OnNodeInsideHandle(uint fromNode, byte[] message)
        {
            string Content = Encoding.UTF8.GetString(message);
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
