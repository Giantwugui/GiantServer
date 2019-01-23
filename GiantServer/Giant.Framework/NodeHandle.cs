using Giant.Net;
using Giant.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Giant.Framework
{
    /// <summary>
    /// 节点句柄
    /// </summary>
    public class NodeHandle
    {
        public NodeHandle(string dllName, NodeRuntime runtime)
        {
            Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + dllName);

            Type[] typeList = assembly.GetTypes();

            List<Type> entryList = new List<Type>();

            foreach (var curr in typeList)
            {
                if (curr.GetInterface("IPlugin") != null)
                {
                    entryList.Add(curr);
                }
            }

            if (entryList.Count > 1)
            {
                throw new Exception(string.Format("Plugin {0} Can't Have More Then One Class Inherited interface IPlugin ", dllName));
            }

            PluginEntryAttribute pluginAttribute = entryList[0].GetCustomAttribute(typeof(PluginEntryAttribute)) as PluginEntryAttribute;
            if (pluginAttribute == null)
            {
                throw new Exception(string.Format("Plugin {0} Have Not PluginEntryAttribute", dllName));
            }

            if (!(Activator.CreateInstance(entryList[0]) is IPlugin plugin))
            {
                throw new Exception(string.Format("Plugin {0} CreateInstance Error !", dllName));
            }

            mRunTime = runtime;

            mNodeEvent = plugin.Events;

            mRunTime.IsFrontNode = pluginAttribute.IsFrontNode;

            mNodeName = pluginAttribute.PluginName;
        }

        /// <summary>
        /// 开始启动
        /// </summary>
        public void ToStart()
        {
            //初始化插件事件
            mNodeEvent.OnInit(mRunTime.Param);

            ThreadPool.QueueUserWorkItem(UpdateLoop, null);

            //内部通讯服务
            InnerNetService.Init(mRunTime);

            if (IsFrontNode)
            {
                //外部通讯服务
                OuterNetServer.Init(mRunTime);
            }

            //插件启动完成事件
            mNodeEvent.OnStartComplate();
        }

        /// <summary>
        /// 工作循环
        /// </summary>
        private void UpdateLoop(object param)
        {
            while (true)
            {
                DateTime now = DateTime.Now;

                float delay = (float)(now - mLastUpdateTime).TotalMilliseconds;
                
                Queue<Message> messages = MessageManager.PopList();

                while (messages.Count > 0)
                {
                    DistributeMessage(messages.Dequeue());
                }

                mLastUpdateTime = now;

                if (delay > 5)
                {
                    Log.LogOut(LogType.Warning, string.Format("节点{0} {1}s 没有响应了!", mNodeName, delay));
                }

                //插件跟新事件
                mNodeEvent.OnUpdate(delay);

                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 派发消息
        /// </summary>
        private void DistributeMessage(Message message)
        {
            switch (message.MessageType)
            {
                case MessageType.Client:
                    {
                        Session session = (Session)message.Param[0];

                        OuterMessage outerMessage = (OuterMessage)message.Param[1];

                        if (outerMessage.ToNode == mRunTime.NodeId)
                        {
                            mNodeEvent.Events_OnHandle(session, outerMessage.Content);
                        }
                        else
                        {
                             InnerMessage innerMessage = new InnerMessage(outerMessage.ToNode, InnerMessageType.Client, outerMessage.Content);

                            innerMessage.Add<Session>("Session", session);

                            InnerNetService.Transmit(outerMessage.ToNode, innerMessage.ToProtoString());
                        }
                    }
                    break;
                case MessageType.ClientOnline:
                    {
                        Session session = (Session)message.Param[0];

                        foreach (var currNode in mRunTime.Nodes)
                        {
                            if (currNode != mRunTime.NodeId)
                            {
                                InnerMessage innerMessage = new InnerMessage(currNode, InnerMessageType.ClientOnline, session.ToProtoBytes());

                                innerMessage.Add<Session>("Session", session);

                                InnerNetService.Transmit(currNode, innerMessage.ToProtoString());
                            }
                        }
                    }
                    break;
                case MessageType.ClientOffline:
                    {
                        Session session = (Session)message.Param[0];

                        foreach (var currNode in mRunTime.Nodes)
                        {
                            if (currNode != mRunTime.NodeId)
                            {
                                InnerMessage innerMessage = new InnerMessage(currNode, InnerMessageType.ClientOffline, session.ToProtoBytes());

                                innerMessage.Add<Session>("Session", session);

                                InnerNetService.Transmit(currNode, innerMessage.ToProtoString());
                            }
                        }
                    }
                    break;
                case MessageType.Inner:
                    {
                        OnInnerMessage((InnerMessage)message.Param[0]);
                    }
                    break;
            }
        }

        private void OnInnerMessage(InnerMessage innerMessage)
        {
            switch (innerMessage.MessageType)
            {
                case InnerMessageType.Inner:
                    {
                        mNodeEvent.Events_OnInsideHandle(NodeId, innerMessage.Content);
                    }
                    break;
                case InnerMessageType.Client:
                    {
                        mNodeEvent.Events_OnHandle(innerMessage.Get<Session>("Session"), innerMessage.Content);
                    }
                    break;
                case InnerMessageType.ClientOnline:
                    {
                        mNodeEvent.Events_OnClientOnline(innerMessage.Get<Session>("Session"));
                    }
                    break;
                case InnerMessageType.ClientOffline:
                    {
                        mNodeEvent.Events_OnClientOffline(innerMessage.Get<Session>("Session"));
                    }
                    break;
            }
        }

        public bool IsFrontNode
        {
            get { return mRunTime.IsFrontNode; }
        }

        public uint NodeId
        {
            get { return mRunTime.NodeId; }
        }

        /// <summary>
        /// 环境运行时
        /// </summary>
        private NodeRuntime mRunTime = null;

        /// <summary>
        /// 上次更新时间
        /// </summary>
        private DateTime mLastUpdateTime = DateTime.Now;

        /// <summary>
        /// 节点昵称
        /// </summary>
        private readonly string mNodeName = "";

        /// <summary>
        /// 节点事件
        /// </summary>
        private readonly NodeEvents mNodeEvent = null;
    }
}
