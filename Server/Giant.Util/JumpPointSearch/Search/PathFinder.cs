using System;
using System.Collections.Generic;

namespace JumpPointSearch
{
    public class JpsPathFinder
    {
        IJpsExpansionPolicy expander;
        public JpsPathFinder(IJpsExpansionPolicy expander)
        {
            this.expander = expander;
        }

        /// <summary>
        /// 对外暴露的寻路接口 不可达则返回null 
        /// </summary>
        /// <param name="start">key为geoX value为geoY</param>
        /// <param name="goal">key为geoX value为geoY</param>
        /// <param name="useRayCast">是否使用三跳点间直线优化</param>
        /// <returns>返回路径</returns>
        public Stack<KeyValuePair<int, int>> FindPath(KeyValuePair<int, int> start, KeyValuePair<int, int> goal, bool useRayCast = true)
        {
            Clear();
            Stack<KeyValuePair<int, int>> path = null;

            int startId = expander.GeoToPaddedId(start.Key, start.Value);
            int goalId = expander.GeoToPaddedId(goal.Key, goal.Value);
            if (startId == goalId)
            {
                path = new Stack<KeyValuePair<int, int>>();
                path.Push(goal);
                path.Push(start);
                return path;
            }

            SearchNode goalNode = Search(startId, goalId);
            if (goalNode != null && goalNode.Id == goalId)
            {
                if (useRayCast)
                {
                    path = GetDirectPath(goalNode);
                }
                else
                {
                    path = new Stack<KeyValuePair<int, int>>();
                    for (SearchNode cur = goalNode; cur != null; cur = cur.Parent)
                    {
                        int geoX, geoY;
                        expander.PaddedIdToGeo(cur.Id, out geoX, out geoY);
                        path.Push(new KeyValuePair<int, int>(geoX, geoY));
                    }
                }

            }
            return path;
        }

        private Stack<KeyValuePair<int, int>> GetDirectPath(SearchNode node)
        {
            Stack<KeyValuePair<int, int>> path = new Stack<KeyValuePair<int, int>>();
            SearchNode cur = node;
            int geoX, geoY;
            expander.PaddedIdToGeo(cur.Id, out geoX, out geoY);
            path.Push(new KeyValuePair<int, int>(geoX, geoY));
            #region debug
            //SearchNode debugCur = node;
            //while (debugCur != null)
            //{
            //    Log.Debug($"old path paddedId {debugCur.Id}");
            //    debugCur = debugCur.Parent;
            //}
            #endregion

            while (cur.Parent != null)
            {
                if (cur.Parent.Parent != null)
                {
                    if (expander.JumpPointVisibleFromTo(cur.Parent.Parent.Id, cur.Id))
                    {
                        expander.PaddedIdToGeo(cur.Parent.Parent.Id, out geoX, out geoY);
                        path.Push(new KeyValuePair<int, int>(geoX, geoY));
                        cur = cur.Parent.Parent;
                    }
                    else
                    {
                        expander.PaddedIdToGeo(cur.Parent.Id, out geoX, out geoY);
                        path.Push(new KeyValuePair<int, int>(geoX, geoY));
                        cur = cur.Parent;
                    }
                }
                else
                {
                    expander.PaddedIdToGeo(cur.Parent.Id, out geoX, out geoY);
                    path.Push(new KeyValuePair<int, int>(geoX, geoY));
                    cur = cur.Parent;
                }
                //Log.Debug($"new  path paddedId {cur.Id}");
            }
            return path;
        }

        private SearchNode Search(int startId, int goalId)
        {
            SearchNode goal = null;
            SearchNode start = expander.Generate(startId);
            start.SetG(0);
            start.SetF(CalcH(startId, goalId, expander.GetGridMap()));
            expander.AddOpenNode(start);

            while (!expander.OpenSetEmpty())
            {
                SearchNode curNode = expander.TryFetchNextOpenNode();
                if (curNode == null)
                {
                    break;
                }
                // 找到
                if (curNode.Id == goalId)
                {
                    goal = curNode;
                    break;
                }
                expander.AddCloseNode(curNode);

                expander.Expand(curNode, goalId);
                int neiNodeId = Constants.INF;
                int costToNei = Constants.INF;
                // 依次curNode的周围跳点
                for (expander.First(out neiNodeId, out costToNei);
                        neiNodeId != Constants.INF; expander.Next(out neiNodeId, out costToNei))
                {
                    if (expander.InCloseSet(neiNodeId))
                    {
                        // 忽略已在CloseSet中的JumpPoint
                        continue;
                    }
                    SearchNode neiNode = null;
                    if (expander.InOpenSet(neiNodeId, out neiNode))
                    {
                        int gVal = curNode.G + costToNei;
                        // 经curNode -> neiNode的G值更小，neiNode更换父节点为curNode
                        if (gVal < neiNode.G)
                        {
                            neiNode.SetParent(curNode);
                            neiNode.SetG(gVal);
                        }
                    }
                    else
                    {
                        // 新的open node
                        neiNode = expander.Generate(neiNodeId);
                        int gVal = curNode.G + costToNei;
                        neiNode.SetG(gVal);
                        neiNode.SetF(gVal + CalcH(neiNodeId, goalId, expander.GetGridMap()));
                        neiNode.SetParent(curNode);
                        expander.AddOpenNode(neiNode);
                    }
                }
            }
            return goal;
        }

        private int CalcH(int x, int y, int x2, int y2)
        {
            int dx = Math.Abs(x - x2);
            int dy = Math.Abs(y - y2);
            if (dx < dy)
            {
                return dx * Constants.ROOT_TWO + (dy - dx) * Constants.ONE;
            }
            return dy * Constants.ROOT_TWO + (dx - dy) * Constants.ONE;
        }

        private int CalcH(int id, int id2, GridMap map)
        {
            int x, y, x2, y2;
            map.ToPadded_X_Y(id, out x, out y);
            map.ToPadded_X_Y(id2, out x2, out y2);
            return CalcH(x, y, x2, y2);
        }

        private void Clear()
        {
            expander.Clear();
        }

        public GridMap GetGridMap()
        {
            return expander.GetGridMap();
        }

        public void SetDynamicGrid(GridMap dynamicMap, GridMap rDynamicMap)
        {
            expander.SetDynamicGrid(dynamicMap, rDynamicMap);
        }
    }
}
