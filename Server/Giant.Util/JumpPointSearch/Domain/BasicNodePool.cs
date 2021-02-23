using System.Collections.Generic;

namespace JumpPointSearch
{
    // 基础版本的NodePool 不包含 分block块 和 池化技术 相关逻辑
    public class BasicNodePool
    {
        Dictionary<int, SearchNode> openDic;
        List<SearchNode> openList;
        HashSet<int> closeSet;

        public BasicNodePool()
        {
            openDic = new Dictionary<int, SearchNode>();
            openList = new List<SearchNode>(1024);
            closeSet = new HashSet<int>();
        }

        public SearchNode Generate(int nodeId)
        {
            return new SearchNode(nodeId);
        }

        public SearchNode TryFetchNextOpenNode()
        {
            SearchNode node = null;
            if(openList.Count > 0)
            {
                int index = openList.Count - 1;
                node = openList[index];
                openList.RemoveAt(index);
                openDic.Remove(node.Id);
            }
            return node;
        }

        public bool InOpenSet(int nodeId, out SearchNode searchNode)
        {
            return openDic.TryGetValue(nodeId, out searchNode);
        }

        public bool InCloseSet(int nodeId)
        {
            return closeSet.Contains(nodeId);
        }

        public void AddOpenNode(SearchNode node)
        {
            if (node == null) return;
            if(!openDic.ContainsKey(node.Id))
            {
                openDic.Add(node.Id, node);
                openList.Add(node);
                openList.Sort();
            }
        }

        public void AddCloseNode(SearchNode node)
        {
            if (node == null) return;
            closeSet.Add(node.Id);
        }

        public bool OpenSetEmpty()
        {
            return openList.Count == 0 ? true : false;
        }

        public void Clear()
        {
            openDic.Clear();
            openList.Clear();
            closeSet.Clear();
        }
    }
}
