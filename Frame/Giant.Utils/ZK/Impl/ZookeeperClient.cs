using org.apache.zookeeper;
using org.apache.zookeeper.data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Giant.Utils.ZK
{
    /// <summary>
    /// ZooKeeper客户端。
    /// </summary>
    public class ZookeeperClient : Watcher, IZookeeperClient
    {
        private bool isDisposed;
        private Event.KeeperState currentState;
        private ConnectionStateChangeHandler connectionStateChangeHandler;

        private readonly object zkEventLock = new object();
        private readonly AutoResetEvent stateChangedCondition = new AutoResetEvent(false);
        private readonly ConcurrentDictionary<string, NodeEntry> nodeEntries = new ConcurrentDictionary<string, NodeEntry>();


        /// <summary>
        /// 具体的ZooKeeper连接。
        /// </summary>
        public ZooKeeper ZooKeeper { get; private set; }

        /// <summary>
        /// 客户端选项。
        /// </summary>
        public ZookeeperClientOptions Options { get; }

        #region Constructor

        /// <summary>
        /// 创建一个新的ZooKeeper客户端。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString"/> 为空。</exception>
        public ZookeeperClient(string connectionString) : this(new ZookeeperClientOptions(connectionString))
        {
        }

        /// <summary>
        /// 创建一个新的ZooKeeper客户端。
        /// </summary>
        /// <param name="options">客户端选项。</param>
        public ZookeeperClient(ZookeeperClientOptions options)
        {
            Options = options;
            ZooKeeper = CreateZooKeeper();
        }

        #endregion Constructor

        #region Public Method

        /// <summary>
        /// 等待zk连接到具体的某一个状态。
        /// </summary>
        /// <param name="states">希望达到的状态。</param>
        /// <param name="timeout">最长等待时间。</param>
        /// <returns>如果成功则返回true，否则返回false。</returns>
        public bool WaitForKeeperState(Event.KeeperState states, TimeSpan timeout)
        {
            var stillWaiting = true;
            while (currentState != states)
            {
                if (!stillWaiting)
                {
                    return false;
                }

                stillWaiting = stateChangedCondition.WaitOne(timeout);
            }
            return true;
        }

        /// <summary>
        /// 重试直到zk连接上。
        /// </summary>
        /// <typeparam name="T">返回类型。</typeparam>
        /// <param name="callable">执行的zk操作。</param>
        /// <returns>执行结果。</returns>
        public async Task<T> RetryUntilConnected<T>(Func<Task<T>> callable)
        {
            var operationStartTime = DateTime.Now;
            while (true)
            {
                try
                {
                    return await callable();
                }
                catch (KeeperException.ConnectionLossException)
                {
                    await Task.Yield();
                    this.WaitForRetry();
                }
                catch (KeeperException.SessionExpiredException)
                {
                    await Task.Yield();
                    this.WaitForRetry();
                }
                if (DateTime.Now - operationStartTime > Options.OperatingTimeout)
                {
                    throw new TimeoutException($"Operation cannot be retried because of retry timeout ({Options.OperatingTimeout.TotalMilliseconds} milli seconds)");
                }
            }
        }

        /// <summary>
        /// 获取指定节点的数据。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <returns>节点数据。</returns>
        public async Task<byte[]> GetDataAsync(string path)
        {
            path = GetZooKeeperPath(path);
            var nodeEntry = GetOrAddNodeEntry(path);
            return await RetryUntilConnected(async () => await nodeEntry.GetDataAsync());
        }

        /// <summary>
        /// 获取指定节点下的所有子节点。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <returns>子节点集合。</returns>
        public async Task<IEnumerable<string>> GetChildrenAsync(string path)
        {
            path = GetZooKeeperPath(path);
            var nodeEntry = GetOrAddNodeEntry(path);
            return await RetryUntilConnected(async () => await nodeEntry.GetChildrenAsync());
        }

        /// <summary>
        /// 判断节点是否存在。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <returns>如果存在则返回true，否则返回false。</returns>
        public async Task<bool> ExistsAsync(string path)
        {
            path = GetZooKeeperPath(path);
            var nodeEntry = GetOrAddNodeEntry(path);
            return await RetryUntilConnected(async () => await nodeEntry.ExistsAsync());
        }

        /// <summary>
        /// 创建节点。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <param name="data">节点数据。</param>
        /// <param name="acls">权限。</param>
        /// <param name="createMode">创建模式。</param>
        /// <returns>节点路径。</returns>
        /// <remarks>
        /// 因为使用序列方式创建节点zk会修改节点name，所以需要返回真正的节点路径。
        /// </remarks>
        public async Task<string> CreateAsync(string path, byte[] data, List<ACL> acls, CreateMode createMode)
        {
            path = GetZooKeeperPath(path);

            var nodeEntry = GetOrAddNodeEntry(path);
            return await RetryUntilConnected(async () => await nodeEntry.CreateAsync(data, acls, createMode));
        }

        /// <summary>
        /// 设置节点数据。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <param name="data">节点数据。</param>
        /// <param name="version">版本号。</param>
        /// <returns>节点状态。</returns>
        public async Task<Stat> SetDataAsync(string path, byte[] data, int version = -1)
        {
            path = GetZooKeeperPath(path);

            var nodeEntry = GetOrAddNodeEntry(path);
            return await RetryUntilConnected(async () => await nodeEntry.SetDataAsync(data, version));
        }

        /// <summary>
        /// 删除节点。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <param name="version">版本号。</param>
        public async Task DeleteAsync(string path, int version = -1)
        {
            path = GetZooKeeperPath(path);

            var nodeEntry = GetOrAddNodeEntry(path);
            await RetryUntilConnected(async () =>
            {
                await nodeEntry.DeleteAsync(version);
                return 0;
            });
        }

        /// <summary>
        /// 订阅节点数据变更。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <param name="listener">监听者。</param>
        public async Task SubscribeDataChange(string path, NodeDataChangeHandler listener)
        {
            path = GetZooKeeperPath(path);

            var node = GetOrAddNodeEntry(path);
            await node.SubscribeDataChange(listener);
        }

        /// <summary>
        /// 取消订阅节点数据变更。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <param name="listener">监听者。</param>
        public void UnSubscribeDataChange(string path, NodeDataChangeHandler listener)
        {
            path = GetZooKeeperPath(path);

            var node = GetOrAddNodeEntry(path);
            node.UnSubscribeDataChange(listener);
        }

        /// <summary>
        /// 订阅连接状态变更。
        /// </summary>
        /// <param name="listener">监听者。</param>
        public void SubscribeStatusChange(ConnectionStateChangeHandler listener)
        {
            connectionStateChangeHandler += listener;
        }

        /// <summary>
        /// 取消订阅连接状态变更。
        /// </summary>
        /// <param name="listener">监听者。</param>
        public void UnSubscribeStatusChange(ConnectionStateChangeHandler listener)
        {
            connectionStateChangeHandler -= listener;
        }

        /// <summary>
        /// 订阅节点子节点变更。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <param name="listener">监听者。</param>
        public async Task<IEnumerable<string>> SubscribeChildrenChange(string path, NodeChildrenChangeHandler listener)
        {
            path = GetZooKeeperPath(path);

            var node = GetOrAddNodeEntry(path);
            return await node.SubscribeChildrenChange(listener);
        }

        /// <summary>
        /// 取消订阅节点子节点变更。
        /// </summary>
        /// <param name="path">节点路径。</param>
        /// <param name="listener">监听者。</param>
        public void UnSubscribeChildrenChange(string path, NodeChildrenChangeHandler listener)
        {
            path = GetZooKeeperPath(path);

            var node = GetOrAddNodeEntry(path);
            node.UnSubscribeChildrenChange(listener);
        }

        #endregion Public Method

        #region Overrides of Watcher

        /// <summary>Processes the specified event.</summary>
        /// <param name="watchedEvent">The event.</param>
        /// <returns></returns>
        public override async Task process(WatchedEvent watchedEvent)
        {
            if (isDisposed)
                return;

            var path = watchedEvent.getPath();
            if (path == null)
            {
                await OnConnectionStateChange(watchedEvent);
            }
            else
            {
                if (nodeEntries.TryGetValue(path, out NodeEntry nodeEntry))
                { 
                    await nodeEntry.OnChange(watchedEvent, false);
                }
            }
        }

        #endregion Overrides of Watcher

        #region Implementation of IDisposable

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            if (isDisposed)
                return;
            isDisposed = true;

            lock (zkEventLock)
            {
                Task.Run(async () =>
                {
                    await ZooKeeper.closeAsync().ConfigureAwait(false);
                }).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        #endregion Implementation of IDisposable

        #region Private Method

        private bool isFirstConnected = true;

        private async Task OnConnectionStateChange(WatchedEvent watchedEvent)
        {
            if (isDisposed)
                return;

            var state = watchedEvent.getState();
            SetCurrentState(state);

            if (state == Event.KeeperState.Expired)
            {
                await ReConnect();
            }
            else if (state == Event.KeeperState.SyncConnected)
            {
                if (isFirstConnected)
                {
                    isFirstConnected = false;
                }
                else
                {
                    foreach (var nodeEntry in nodeEntries)
                    {
                        await nodeEntry.Value.OnChange(watchedEvent, true);
                    }
                }
            }

            stateChangedCondition.Set();
            if (connectionStateChangeHandler == null)
                return;
            await connectionStateChangeHandler(this, new StateChangeArgs
            {
                State = state
            });
        }

        private NodeEntry GetOrAddNodeEntry(string path)
        {
            return nodeEntries.GetOrAdd(path, k => new NodeEntry(path, this));
        }

        private ZooKeeper CreateZooKeeper()
        {
            return new ZooKeeper(Options.ConnectionString, (int)Options.SessionTimeout.TotalMilliseconds, this, Options.SessionId, Options.SessionPasswd, Options.ReadOnly);
        }

        private async Task ReConnect()
        {
            if (!Monitor.TryEnter(zkEventLock, Options.ConnectionTimeout))
                return;
            try
            {
                await ZooKeeper?.closeAsync();
                ZooKeeper = CreateZooKeeper();
            }
            finally
            {
                Monitor.Exit(zkEventLock);
            }
        }

        private void SetCurrentState(Event.KeeperState state)
        {
            lock (this)
            {
                currentState = state;
            }
        }

        private string GetZooKeeperPath(string path)
        {
            var basePath = Options.BasePath ?? "/";

            if (!basePath.StartsWith("/"))
                basePath = basePath.Insert(0, "/");

            basePath = basePath.TrimEnd('/');

            if (!path.StartsWith("/"))
                path = path.Insert(0, "/");

            path = $"{basePath}{path.TrimEnd('/')}";
            return string.IsNullOrEmpty(path) ? "/" : path;
        }

        #endregion Private Method
    }
}