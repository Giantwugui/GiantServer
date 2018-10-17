using GiantNode.Interface;
using System.Collections.Generic;

namespace GiantNode
{
    /// <summary>
    /// 节点运行时
    /// </summary>
    public class NodeRuntime : IRunTime
    {
        public NodeRuntime(uint groupId, uint nodeId, string nodeName, uint[] nodes, Dictionary<string, string> param)
        {
            mNodeId = nodeId;
            mGroupId = groupId;
            mNodeName = nodeName;
            mNodes = nodes;
            mParam = param;
        }

        public string GetParam(string key)
        {
            if (mParam.ContainsKey(key))
                return mParam[key];

            return "";
        }

        /// <summary>
        /// 当前节点id
        /// </summary>
        public uint NodeId
        {
            get { return mNodeId; }
        }

        /// <summary>
        /// 组id
        /// </summary>
        public uint GroupId
        {
            get { return mGroupId; }
        }

        public string NodeName
        {
            get { return mNodeName; }
        }

        /// <summary>
        /// 所有节点信息
        /// </summary>
        public uint[] Nodes
        {
            get { return mNodes; }
        }

        /// <summary>
        /// 是否为前端节点
        /// </summary>
        public bool IsFrontNode
        {
            get { return mIsFrontNode; }
            set { mIsFrontNode = value; }
        }

        public Dictionary<string, string> Param
        {
            get { return mParam; }
        }

        /// <summary>
        /// 当前节点id
        /// </summary>
        private uint mNodeId = 0;

        /// <summary>
        /// 组id
        /// </summary>
        private uint mGroupId = 0;

        /// <summary>
        /// 是否为前端节点
        /// </summary>
        private bool mIsFrontNode = false;

        /// <summary>
        /// 当前节点名
        /// </summary>
        private string mNodeName;

        /// <summary>
        /// 所有节点信息
        /// </summary>
        private uint[] mNodes;

        /// <summary>
        /// 参数
        /// </summary>
        private Dictionary<string, string> mParam = new Dictionary<string, string>();
    }
}
