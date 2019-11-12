using org.apache.zookeeper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giant.Utils.ZK
{
    /// <summary>
    /// 节点数据变更委托。
    /// </summary>
    /// <param name="client">ZooKeeper客户端。</param>
    /// <param name="args">节点数据变更参数。</param>
    public delegate Task NodeDataChangeHandler(IZookeeperClient client, NodeDataChangeArgs args);

    /// <summary>
    /// 节点子节点变更委托。
    /// </summary>
    /// <param name="client">ZooKeeper客户端。</param>
    /// <param name="args">节点子节点变更参数。</param>
    public delegate Task NodeChildrenChangeHandler(IZookeeperClient client, NodeChildrenChangeArgs args);

    /// <summary>
    /// 连接状态变更委托。
    /// </summary>
    /// <param name="client">ZooKeeper客户端。</param>
    /// <param name="args">连接状态变更参数。</param>
    public delegate Task ConnectionStateChangeHandler(IZookeeperClient client, StateChangeArgs args);
}