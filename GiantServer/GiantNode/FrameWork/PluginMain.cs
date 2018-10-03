using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiantCore;
using System.Reflection;

namespace GiantNode
{
    class PluginMain
    {
        /// <summary>
        /// 初始化各个插件
        /// </summary>
        public static void InitPlugins()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.GetInterface("IPlugin") != null && !type.IsAbstract)
                {
                    IPlugin tempPlugin = Activator.CreateInstance(type) as IPlugin;
                    if (tempPlugin != null)
                    {
                        m_Plugins.Add(tempPlugin);
                    }
                }
            }
        }

        /// <summary>
        /// 循环心跳
        /// </summary>
        public static void OnUpdate(float time)
        {
            foreach (IPlugin curr in m_Plugins)
            {
                GEvent tempEvent = curr.Event;
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
                GEvent tempEvent = curr.Event;
                if (tempEvent == null) continue;

                tempEvent.Call_OnHandle(session, request);
            }
        }

        public static void OnInit()
        {
            foreach (IPlugin curr in m_Plugins)
            {
                GEvent tempEvent = curr.Event;
                if (tempEvent == null) continue;

                tempEvent.Call_Init();
            }
        }

        public static void OnStartComplate()
        {
            foreach (IPlugin curr in m_Plugins)
            {
                GEvent tempEvent = curr.Event;
                if (tempEvent == null) continue;

                tempEvent.Call_OnStartComplate();
            }
        }

        static List<IPlugin> m_Plugins = new List<IPlugin>();
    }
}
