using org.apache.zookeeper;
using System.Collections.Generic;

namespace Giant.Utils.ZK
{
    /// <summary>
    /// 节点子节点变更参数。
    /// </summary>
    public sealed class NodeChildrenChangeArgs : NodeChangeArgs
    {
        /// <summary>
        /// 创建一个新的节点子节点变更参数。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <param name="type">事件类型。</param>
        /// <param name="currentChildrens">最新的子节点集合。</param>
        public NodeChildrenChangeArgs(string path, Watcher.Event.EventType type, IEnumerable<string> currentChildrens) : base(path, type)
        {
            CurrentChildrens = currentChildrens;
        }

        /// <summary>
        /// 当前节点的子节点数据（最新的）
        /// </summary>
        public IEnumerable<string> CurrentChildrens { get; private set; }
    }
}
