using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using GiantNode;
using GiantCore;

namespace GiantNodeBox
{
    class PluginManager
    {
        /// <summary>
        /// 初始化各个插件
        /// </summary>
        public static void StartInitPlugins()
        {
            NodeHandle nodeHandle = new NodeHandle("TestPlugin.dll");

            m_Plugins.Add(nodeHandle);
        }

        /// <summary>
        /// 循环心跳
        /// </summary>
        public static void OnUpdate(float time)
        {
            foreach (IPlugin curr in m_Plugins)
            {
                NodeEvents tempEvent = curr.Events;
                if (tempEvent == null) continue;

                tempEvent.Call_OnUpdate(time);
            }
        }

        /// <summary>
        /// 全局消息处理
        /// </summary>
        public static void OnHandle(Session session, byte[] request)
        {
            foreach (IPlugin curr in m_Plugins)
            {
                NodeEvents tempEvent = curr.Events;
                if (tempEvent == null) continue;

                tempEvent.Call_OnHandle(session, request);
            }
        }

        public static void OnInit()
        {
            foreach (IPlugin curr in m_Plugins)
            {
                NodeEvents tempEvent = curr.Events;
                if (tempEvent == null) continue;

                tempEvent.Call_Init();
            }
        }

        public static void OnStartComplate()
        {
            foreach (IPlugin curr in m_Plugins)
            {
                NodeEvents tempEvent = curr.Events;
                if (tempEvent == null) continue;

                tempEvent.Call_OnStartComplate();
            }
        }

        static List<NodeHandle> m_Plugins = new List<NodeHandle>();
    }
}
