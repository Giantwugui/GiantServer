using System;

namespace Giant.Framework
{
    public class PluginEntryAttribute : Attribute
    {
        public PluginEntryAttribute(string nodeName, bool isFrontNode)
        {
            mPluginName = nodeName;
            mIsFrontNode = isFrontNode;
        }

        /// <summary>
        /// 是否为前端节点
        /// </summary>
        public bool IsFrontNode
        {
            get { return mIsFrontNode; }
            set { mIsFrontNode = value; }
        }

        /// <summary>
        /// 节点名
        /// </summary>
        public string PluginName
        {
            get { return mPluginName; }
            set { mPluginName = value; }
        }

        private string mPluginName = "";

        private bool mIsFrontNode = false;
    }
}
