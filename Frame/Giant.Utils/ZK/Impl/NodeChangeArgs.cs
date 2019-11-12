using org.apache.zookeeper;

namespace Giant.Utils.ZK
{
    /// <summary>
    /// 节点变更参数。
    /// </summary>
    public abstract class NodeChangeArgs
    {
        /// <summary>
        /// 创建一个新的节点变更参数。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <param name="type">事件类型。</param>
        protected NodeChangeArgs(string path, Watcher.Event.EventType type)
        {
            Path = path;
            Type = type;
        }

        /// <summary>
        /// 变更类型。
        /// </summary>
        public Watcher.Event.EventType Type { get; private set; }

        /// <summary>
        /// 节点路径。
        /// </summary>
        public string Path { get; private set; }
    }

    /// <summary>
    /// 节点数据变更参数。
    /// </summary>
    public sealed class NodeDataChangeArgs : NodeChangeArgs
    {
        /// <summary>
        /// 创建一个新的节点数据变更参数。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <param name="type">事件类型。</param>
        /// <param name="currentData">最新的节点数据。</param>
        public NodeDataChangeArgs(string path, Watcher.Event.EventType type, byte[] currentData) : base(path, type)
        {
            CurrentData = currentData;
        }

        /// <summary>
        /// 当前节点数据（最新的）
        /// </summary>
        public byte[] CurrentData { get; private set; }
    }
}
