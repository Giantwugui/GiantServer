using GiantNode;
using System.Collections.Generic;

namespace GiantNodeBox
{
    class PluginManager
    {
        public static void StartPlugins()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("FrontPort", "8099");

            List<uint> allNodes = new List<uint>(1);

            NodeHandle nodeHandle = new NodeHandle("TestPlugin.dll", new NodeRuntime(1, 1, "TestPlugin", allNodes.ToArray(), param));

            nodeHandle.ToStart();

            m_Plugins.Add(nodeHandle);
        }


        static List<NodeHandle> m_Plugins = new List<NodeHandle>();
    }
}
