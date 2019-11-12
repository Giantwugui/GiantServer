using org.apache.zookeeper;

namespace Giant.Utils.ZK
{
    /// <summary>
    /// 连接状态变更事件参数。
    /// </summary>
    public class StateChangeArgs
    {
        /// <summary>
        /// 连接状态。
        /// </summary>
        public Watcher.Event.KeeperState State { get; set; }
    }
}
