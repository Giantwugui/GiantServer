using System;
using System.Collections.Generic;
using EpPathFinding;
using JumpPointSearch;
using UnityEngine;

namespace Giant.Battle
{
    public partial class MapScene
    {
        private BaseGrid grid;
        private BaseGrid gridBig;
        private JumpPointParam jumpPointParam;
        private JumpPointParam jumpPointParamBig;
        protected GridMap dynamicMap;
        protected GridMap rDynamicMap;
        protected bool UseDynamicGrid { get; private set; }

        public int MinX => MapModel.MinX;
        public int MinY => MapModel.MinY;
        public int MaxX => MapModel.MaxX;
        public int MaxY => MapModel.MaxY;
        public Vector2 BeginPosition => MapModel.BeginPosition;
        public bool HighPrecision => MapModel.HighPrecision;

        public void InitDynamicGrid()
        {
            UseDynamicGrid = false;
        }

        public void EnableDynamicGrid(bool enable)
        {
            UseDynamicGrid = enable;
        }

        public void SetFieldObjectObstract(Unit unit, bool obstract)
        {
            if (dynamicMap == null || rDynamicMap == null || unit == null)
            {
                return;
            }
            int geoX = (int)Math.Round(unit.Position.x);
            int geoY = (int)Math.Round(unit.Position.y);
            int r = (int)(unit.Radius);

            int dynamicPaddedId = dynamicMap.GeoToPaddedId(geoX, geoY);
            dynamicMap.SetDynamicWalkable(dynamicPaddedId, r, !obstract);

            int rDynamicPaddedId = dynamicMap.MapToRMapPaddedId(dynamicPaddedId, rDynamicMap);
            rDynamicMap.SetDynamicWalkable(rDynamicPaddedId, r, !obstract);
        }

        public virtual void UpdateDynamicGrid()
        {
            return;
        }

        public bool IsWalkableAt(int x, int y, bool useBig = false)
        {
            if (useBig)
            {
                if (gridBig == null) return false;

                return gridBig.IsWalkableAt(x, y);
            }

            if (grid == null) return false;

            return grid.IsWalkableAt(x, y);
        }

        public bool CheckPath(Vector2 from, Vector2 to, bool useBig = false)
        {
            GridPos start = VectorToGridPos(from);
            GridPos end = VectorToGridPos(to);

            JumpPointParam tempPointParam = useBig ? jumpPointParamBig : jumpPointParam;
            if (tempPointParam == null) return false;
            tempPointParam.Reset(start, end);
            List<GridPos> resultPath = JumpPointFinder.FindPath(tempPointParam);
            // not found path
            if (resultPath == null || resultPath.Count == 0)
            {
                return false;
            }
            return true;
        }

        public Vector2[] GetPath(Vector2 from, Vector2 to, bool useBig = false)
        {
            GridPos start = VectorToGridPos(from);
            GridPos end = VectorToGridPos(to);
         
            JumpPointParam tempPointParam = useBig ? jumpPointParamBig : jumpPointParam;
            if (tempPointParam == null) return null;

            tempPointParam.Reset(start, end);
            List<GridPos> path = JumpPointFinder.FindPath(tempPointParam);
            if (path == null || path.Count == 0)
            {
                return new Vector2[] { from, from };
            }

            List<int> removeList = CheckDirect(0, path);
            removeList.Reverse();
            removeList.ForEach(x => path.RemoveAt(x));

            Vector2[] result;
            if (path.Count == 1)
            {
                result = new Vector2[] { from, to };
            }
            else
            {
                result = path.ConvertAll(x => new Vector2(x.x, x.y)).ToArray();
                result[0] = from;
                result[^1] = to;
            }
            return result;
        }

        public Vector2[] GetPath_New(Vector2 from, Vector2 to, bool useDynamic)
        {
            int startX = (int)Math.Round(from.x);
            int startY = (int)Math.Round(from.y);
            int endX = (int)Math.Round(to.x);
            int endY = (int)Math.Round(to.y);

            if (useDynamic)
            {
                MapModel.JpsPathFinder.SetDynamicGrid(dynamicMap, rDynamicMap);
                //dynamicMap.PrintUnwalkable();
            }
            else
            {
                MapModel.JpsPathFinder.SetDynamicGrid(null, null);
            }

            Stack<KeyValuePair<int, int>> path = FindPath(MapModel.JpsPathFinder, startX, startY, endX, endY);
            // 重置动态格挡信息
            MapModel.JpsPathFinder.SetDynamicGrid(null, null);

            if (path == null)
            {
                // 小网格未找到，使用大网格不考虑动态阻挡
                MapModel.JpsPathFinderBig.SetDynamicGrid(null, null);
                path = FindPath(MapModel.JpsPathFinderBig, startX, startY, endX, endY);

                if (path == null) return null;
            }

            KeyValuePair<int, int>[] arr = path.ToArray();
            Vector2[] result = new Vector2[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                result[i] = new Vector2(arr[i].Key, arr[i].Value);
            }

            return result;
        }

        private Stack<KeyValuePair<int, int>> FindPath(JpsPathFinder pathFinder, int startX, int startY, int endX, int endY)
        {
            return pathFinder.FindPath(new KeyValuePair<int, int>(startX, startY), new KeyValuePair<int, int>(endX, endY), true);
        }

        private GridPos VectorToGridPos(Vector2 vector)
        {
            return new GridPos((int)Math.Round(vector.x), (int)Math.Round(vector.y));
        }

        private List<int> CheckDirect(int startIndex, List<GridPos> path)
        {
            int nextStartIndex = startIndex;
            List<int> remoeIndexList = new List<int>();
            
            for (int i = startIndex + 2; i < path.Count; i++)
            {
                nextStartIndex = i - 1;
                if (IsDirect(path[startIndex], path[i]))
                {
                    remoeIndexList.Add(i - 1);
                }
                else
                {
                    break;
                }
            }

            if (nextStartIndex + 2 < path.Count)
            {
                remoeIndexList.AddRange(CheckDirect(nextStartIndex, path));
            }

            return remoeIndexList;
        }

        private bool IsDirect(GridPos start, GridPos end, bool useBig = false)
        {
            int minx = 0, miny = 0, maxx = 0, maxy = 0;
            if (start.x < end.x)
            {
                minx = start.x;
                maxx = end.x;
            }
            else
            {
                minx = end.x;
                maxx = start.x;
            }

            if (start.y < end.y)
            {
                miny = start.y;
                maxy = end.y;
            }
            else
            {
                miny = end.y;
                maxy = start.y;
            }

            GridPos line = new GridPos(end.x - start.x, end.y - start.y);
            for (int x = minx; x< maxx; x++)
            {
                for (int y = miny; y< maxy; y++)
                {
                    if (useBig)
                    {
                        if (!IsIntersectionGrid(line, x - start.x, y - start.y) && gridBig.IsWalkableAt(x, y)) return false;
                    }
                    else
                    {
                        if (!IsIntersectionGrid(line, x - start.x, y - start.y) && grid.IsWalkableAt(x, y)) return false;
                    }
                }
            }
            return true;
        }

        private bool IsIntersectionGrid(GridPos line, int x, int y)
        {
            float a = line.y, b = -line.x;
            float result = (a * x + b * y) * (a * x + b * y) / (a * a + b * b);
            if (result < 0.5f) return true;
            return false;
        }
    }
}
