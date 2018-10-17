using GiantNode;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GiantNodeBox
{
    class NodeHandle
    {
        public NodeHandle(string dllName)
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

            IPlugin plugin = Activator.CreateInstance(entryList[0]) as IPlugin;
            if (plugin == null)
            {
                throw new Exception(string.Format("Plugin {0} CreateInstance Error !", dllName));
            }

            mNodeEvent = plugin.Events;

            mIsFrontNode = pluginAttribute.IsFrontNode;

            mNodeName = pluginAttribute.PluginName;
        }




        private readonly string mNodeName = "";

        private readonly  bool mIsFrontNode = false;

        private readonly NodeEvents mNodeEvent = null;
    }
}
