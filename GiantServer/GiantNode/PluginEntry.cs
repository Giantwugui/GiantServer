using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiantCore;

namespace GiantNode
{
    class PluginEntry : IPlugin
    {
        public PluginEntry()
        {
            mEvent = new GEvent();
            mEvent.onInit += OnInit;
            mEvent.onHandle += OnHandle;
            mEvent.onUpdate += OnUpdate;
        }

        public void OnHandle(Session session, byte[] message)
        {
            string strReceiveData = Encoding.Unicode.GetString(message);

            QK_JsonValue_Map tempMap = new QK_JsonValue_Map();
            tempMap.Parse(strReceiveData);

            //定义一种通讯用的Json格式
            //string cmdName = tempMap.StrValue("n");
            //string param1 = tempMap.StrValue("param1");
            //string param2 = tempMap.StrValue("param2");

            //NMDispatcher.Single.OnHandler(cmdName, param1, param2);

            if (!string.IsNullOrEmpty(strReceiveData))
            {
                //GiantServer.Single.LogOut(LogType.Warning, "OK. The content is:" + strReceiveData);

                //回传给客户端消息发送成功
                session.Return("Send Success!");
            }
        }

        public void OnInit()
        {
        }

        /// <summary>
        /// 心跳循环
        /// </summary>
        public void OnUpdate(float time)
        {
        }



        public static PluginEntry Single
        {
            get { return mSingle; }
        }

        #region 实现基类接口

        public GEvent Event
        {
            get { return mEvent; }
        }

        #endregion

        GEvent mEvent = null;
        static PluginEntry mSingle = null;
    }
}
