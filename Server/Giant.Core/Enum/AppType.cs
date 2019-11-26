namespace Giant.Core
{
    public enum AppType
    {
        /// <summary>
        /// 注册中心，负责服务的注册，以及通知其他服务注册到某一服务(当map上线 通知对应的gate连接到map)
        /// </summary>
        Global = 1,
        Gate = 1 << 1,
        Manager = 1 << 2,
        Zone = 1 << 3,
        Relation = 1 << 4,
        Account = 1 << 5,
        AllServer = Global | Gate | Manager | Zone | Relation | Account
    }
}
