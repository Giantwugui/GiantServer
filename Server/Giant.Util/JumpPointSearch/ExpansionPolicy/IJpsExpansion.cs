namespace JumpPointSearch
{
    /// <summary>
    /// Jps当前node 扩张找邻居 通用接口
    /// 向上层通用的寻路算法屏蔽底层不同的优化策略（激进剪裁，预处理缓存等）
    /// </summary>
    public interface IJpsExpansionPolicy
    {
        void Expand(SearchNode current, int goalId);
        SearchNode Generate(int nodeId);
        void First(out int nodeId, out int cost);
        void Next(out int nodeId, out int cost);

        int GeoToPaddedId(int geoX, int geoY);

        void PaddedIdToGeo(int nodeId, out int geoX, out int geoY);

        SearchNode TryFetchNextOpenNode();

        bool InOpenSet(int nodeId, out SearchNode searchNode);

        bool InCloseSet(int nodeId);

        void AddOpenNode(SearchNode node);

        void AddCloseNode(SearchNode node);

        bool OpenSetEmpty();

        GridMap GetGridMap();

        void SetDynamicGrid(GridMap dynamicMap, GridMap rDynamicMap);

        void Clear();

        bool CheckDirectionWalkable(uint curTiles, Direction dir);

        bool JumpPointVisibleFromTo(int paddedIdStart, int paddedIdEnd);
    }
}
