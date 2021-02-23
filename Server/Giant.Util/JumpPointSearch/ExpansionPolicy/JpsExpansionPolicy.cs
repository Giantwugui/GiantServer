using System;

namespace JumpPointSearch
{
    // Jps当前node 扩张找邻居 
    // 
    /// <summary>
    /// Jps当前node 扩张找邻居 的策略
    /// JpsExpansionPolicy 使用最基础的扩张策略 
    /// 没有对中间Jump Point(即没有Force Neighbor)做更激进的剪裁
    /// </summary>
    public class JpsExpansionPolicy : IJpsExpansionPolicy
    {
        private GridMap map;
        private GridMap dynamicMap;
        private BasicNodePool nodepool;
        private OnlineJumpPointLocator jpl;
        int which;
        int numNeighbours; // 需要进一步检测的邻居个数
        int[] neiNodeIds;
        int[] costs;

        public JpsExpansionPolicy(GridMap map)
        {
            this.map = map;
            nodepool = new BasicNodePool();
            jpl = new OnlineJumpPointLocator(map);
            neiNodeIds = new int[9];
            costs = new int[9];
        }

        private void Reset()
        {
            which = 0;
            numNeighbours = 0;
            for (int i = 0; i < 9; i++)
            {
                neiNodeIds[i] = Constants.INF;
                costs[i] = 0;
            }
        }

        // reset并清理nodePool
        public void Clear()
        {
            Reset();
            nodepool.Clear();
        }

        public int GeoToPaddedId(int geoX, int geoY)
        {
            return map.GeoToPaddedId(geoX, geoY);
        }

        public void PaddedIdToGeo(int paddedId, out int geoX, out int geoY)
        {
            map.PaddedIdToGeo(paddedId, out geoX, out geoY);
        }

        public SearchNode Generate(int nodeId)
        {
            return nodepool.Generate(nodeId);
        }

        public SearchNode TryFetchNextOpenNode()
        {
            return nodepool.TryFetchNextOpenNode();
        }

        public bool InOpenSet(int nodeId, out SearchNode searchNode)
        {
            return nodepool.InOpenSet(nodeId, out searchNode);
        }

        public bool InCloseSet(int nodeId)
        {
            return nodepool.InCloseSet(nodeId);
        }

        public void AddOpenNode(SearchNode node)
        {
            nodepool.AddOpenNode(node);
        }

        public void AddCloseNode(SearchNode node)
        {
            nodepool.AddCloseNode(node);
        }

        public bool OpenSetEmpty()
        {
            return nodepool.OpenSetEmpty();
        }

        public void First(out int nodeId, out int cost)
        {
            which = 0;
            cost = costs[which];
            nodeId = neiNodeIds[which];
        }

        public void Next(out int nodeId, out int cost)
        {
            if (which < numNeighbours)
            {
                which++;
            }
            cost = costs[which];
            nodeId = neiNodeIds[which];
        }

        public GridMap GetGridMap()
        {
            return map;
        }

        // 计算 n1 -> n2 方向
        private Direction ComputeDirection(SearchNode n1, SearchNode n2)
        {
            if (n1 == null) { return Direction.NONE; }

            int x, y, x2, y2;
            map.ToPadded_X_Y(n1.Id, out x, out y);
            map.ToPadded_X_Y(n2.Id, out x2, out y2);
            Direction dir = Direction.NONE;
            if (y2 == y)
            {
                if (x2 > x)
                    dir = Direction.EAST;
                else
                    dir = Direction.WEST;
            }
            else if (y2 < y)
            {
                if (x2 == x)
                    dir = Direction.NORTH;
                else if (x2 < x)
                    dir = Direction.NORTHWEST;
                else // x2 > x
                    dir = Direction.NORTHEAST;
            }
            else // y2 > y 
            {
                if (x2 == x)
                    dir = Direction.SOUTH;
                else if (x2 < x)
                    dir = Direction.SOUTHWEST;
                else // x2 > x
                    dir = Direction.SOUTHEAST;
            }
            return dir;
        }

        public void Expand(SearchNode current, int goalId)
        {
            Reset();

            // 计算当前的移动方向
            Direction dirCur = ComputeDirection(current.Parent, current);

            // 获取当前节点的九宫格邻居信息
            uint curTiles;
            int curId = current.Id;
            curTiles = map.GetNeighbours(curId);
            if(dynamicMap != null)
            {
                uint dynamicTiles = dynamicMap.GetNeighbours(curId);
                curTiles = curTiles & dynamicTiles;
            }

            // 根据邻居可达信息和当前移动方向，计算下一步要进行搜索的方向（nature neighbor和 forced neighbor）
            int succDirs = JumpPointSearch.ComputeSuccessors(dirCur, curTiles);
            for (int i = 0; i < 8; i++)
            {
                Direction d = (Direction)(1 << i);
                if ((succDirs & (int)d) > 0)
                {
                    int jumpCost;
                    int succId;
                    jpl.Jump(d, curId, goalId, out succId, out jumpCost);

                    if (succId != Constants.INF)
                    {
                        neiNodeIds[numNeighbours] = succId;
                        costs[numNeighbours] = jumpCost;
                        numNeighbours++;
                    }
                }
            }
        }

        /// <summary>
        /// 废弃
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        /// <param name="deltax1"></param>
        /// <param name="deltay1"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <param name="dir"></param>
        /// <param name="longDir"></param>
        /// <param name="paddedId"></param>
        /// <param name="endId"></param>
        /// <param name="nextPaddedId"></param>
        /// <param name="curDeltax1"></param>
        /// <param name="curDeltay1"></param>
        /// <returns></returns>
        public bool CheckNextPaddedIdForDirectPath(int deltaX, int deltaY, int deltax1, int deltay1, int endX, int endY, Direction[] dir,
            Direction longDir, int paddedId, out int nextPaddedId, out int curDeltax1, out int curDeltay1)
        {
            uint curTiles = map.GetNeighbours(paddedId);
            if(dynamicMap != null)
            {
                uint dynamicTiles = dynamicMap.GetNeighbours(paddedId);
                curTiles = curTiles & dynamicTiles;
            }

            int nextX_DirPaddedId = GetNeighbourPaddedId(paddedId, dir[1]);
            int nextY_DirPaddedId = GetNeighbourPaddedId(paddedId, dir[2]);

            #region 试探当前点和当前方向下个点是否在三条边上:与终点连线平行坐标轴或对角线


            int tempx, tempy;
            int tempDeltaX, tempDeltaY;
            int tempPaddedId;

            //////1.试探当前点
            if (deltax1 == 0)
            {
                map.ToUnpadded_X_Y(nextY_DirPaddedId, out tempx, out tempy);
                nextPaddedId = nextY_DirPaddedId;
                curDeltax1 = endX - tempx;
                curDeltay1 = endY - tempy;
                dir[0] = dir[2];
                return CheckDirectionWalkable(curTiles, dir[0]);
            }
            if (deltay1 == 0)
            {
                map.ToUnpadded_X_Y(nextX_DirPaddedId, out tempx, out tempy);
                nextPaddedId = nextX_DirPaddedId;
                curDeltax1 = endX - tempx;
                curDeltay1 = endY - tempy;
                dir[0] = dir[1];
                return CheckDirectionWalkable(curTiles, dir[0]);
            }
            ////////////2.试探当前方向下个点
            if (dir[0] == dir[1])
            {
                map.ToUnpadded_X_Y(nextX_DirPaddedId, out tempx, out tempy);
                tempPaddedId = nextX_DirPaddedId;
            }
            else
            {
                map.ToUnpadded_X_Y(nextY_DirPaddedId, out tempx, out tempy);
                tempPaddedId = nextY_DirPaddedId;
                //tempDir = dir[2];
            }
            tempDeltaX = endX - tempx;
            tempDeltaY = endY - tempy;

            bool nextPosEq = Math.Abs(deltaX) * Math.Abs(tempDeltaY) == Math.Abs(deltaY) * Math.Abs(tempDeltaX);
            if (nextPosEq || tempDeltaX == 0 || tempDeltaY == 0)//假如走到线上，三种方式
            {
                Direction last;
                if (tempDeltaX == 0)//到终点连线和x轴平行，下一次变方向
                {
                nextPaddedId = tempPaddedId;
                curDeltax1 = tempDeltaX;
                curDeltay1 = tempDeltaY;
                    last = dir[0];
                    dir[0] = dir[2];

            }
                else if (tempDeltaY == 0) //到终点连线和x轴平行，下一次变方向
                {
                    nextPaddedId = tempPaddedId;
                    curDeltax1 = tempDeltaX;
                    curDeltay1 = tempDeltaY;
                    last = dir[0];
                    dir[0] = dir[1];
                }
                else
                {
                    nextPaddedId = tempPaddedId;//到终点连线和起终点对角线重合，下一次走长边方向
                    curDeltax1 = tempDeltaX;
                    curDeltay1 = tempDeltaY;
                    last = dir[0];
                    dir[0] = longDir;
                }
                return CheckDirectionWalkable(curTiles, last);
            }
            #endregion
            else//斜着
            {
                int x1, y1, x2, y2;
                int tdx1, tdy1, tdx2, tdy2;
                map.ToUnpadded_X_Y(nextX_DirPaddedId, out x1, out y1);
                map.ToUnpadded_X_Y(nextY_DirPaddedId, out x2, out y2);
                tdx1 = endX - x1;
                tdy1 = endY - y1;
                tdx2 = endX - x2;
                tdy2 = endY - y2;

                if (tdx1 == 0 || tdy1 == 0 || tdx2 == 0 || tdy2 == 0)//////3.有一个为0 就走这个方向
                {
                    if (tdx1 == 0|| tdy1 == 0)
                    {
                        dir[0] = dir[1];
                        curDeltax1 = tdx1;
                        curDeltay1 = tdy1;
                        nextPaddedId = nextX_DirPaddedId;
                        return CheckDirectionWalkable(curTiles, dir[0]);
                    }
                    else
                    {
                        dir[0] = dir[2];
                        curDeltax1 = tdx2;
                        curDeltay1 = tdy2;
                        nextPaddedId = nextY_DirPaddedId;
                        return CheckDirectionWalkable(curTiles, dir[0]);
                    }
                }

                /////4.此时所有用于判断交线的参数都不为0
                bool temp1;//下个点是x方向时候是否交线
                temp1 = (Math.Abs(deltaX) * Math.Abs(deltay1) > Math.Abs(deltaY) * Math.Abs(deltax1))
                 != (Math.Abs(deltaX) * Math.Abs(tdy1) > Math.Abs(deltaY) * Math.Abs(tdx1));
                bool temp2;//下个点是y方向时候是否交线
                temp2 = (Math.Abs(deltaX) * Math.Abs(deltay1) > Math.Abs(deltaY) * Math.Abs(deltax1))
                 != (Math.Abs(deltaX) * Math.Abs(tdy2) > Math.Abs(deltaY) * Math.Abs(tdx2));

                if (temp1 && temp2)//到达45度差最后一点
                {
                    dir[0] = longDir;
                    curDeltax1 = tdx1;
                    curDeltay1 = tdy1;
                    nextPaddedId = nextX_DirPaddedId;
                }
                else if (!(temp1 ^ temp2))// 两边都不交线
                {
                    dir[0] = longDir;
                    if (longDir == dir[1])
                    {
                        curDeltax1 = tdx1;
                        curDeltay1 = tdy1;
                        nextPaddedId = nextX_DirPaddedId;
                    }
                    else
                    {
                        curDeltax1 = tdx2;
                        curDeltay1 = tdy2;
                        nextPaddedId = nextY_DirPaddedId;
                    }
                    return CheckDirectionWalkable(curTiles, dir[0]);
                }
                else//必有一个方向交线
                {

                    curDeltax1 = tdx1;
                    curDeltay1 = tdy1;
                    if (temp1)//往X轴方向相交了
                    {
                        nextPaddedId = nextX_DirPaddedId;
                        dir[0] = dir[1];
                    }
                    else//往Y轴方向相交了
                    {
                        dir[0] = dir[2];
                        curDeltax1 = tdx2;
                        curDeltay1 = tdy2;
                        nextPaddedId = nextY_DirPaddedId;
                    }
                }
            }
            //找到了点后
            return CheckDirectionWalkable(curTiles, dir[0]);
        }

        /// <summary>
        /// 根据上一次计算出的内容，计算下一个探测点的内容和下一个点内容往下下个点的方向
        /// </summary>
        /// <param name="deltaX">起终点的X差</param>
        /// <param name="deltaY">起终点的Y差</param>
        /// <param name="deltax1">当前点差</param>
        /// <param name="deltay1">当前点差</param>
        /// <param name="endX">结束点坐标</param>
        /// <param name="endY">结束点坐标</param>
        /// <param name="dir">计算参考的三个方向</param>
        /// <param name="longDir">长边方向</param>
        /// <param name="paddedId">当前点id</param>
        /// <param name="endId">结束点id</param>
        /// <param name="nextPaddedId">下一点的id</param>
        /// <param name="nextDeltax1">下一点到终点的差</param>
        /// <param name="nextDeltay1">下一点到终点的差</param>
        /// <returns></returns>
        public bool CheckNextPaddedIdForDirectPath(int deltaX, int deltaY, int deltax1, int deltay1, int endX, int endY, Direction[] dir,
            Direction longDir, int paddedId,int endId, out int nextPaddedId, out int nextDeltax1, out int nextDeltay1)
        {
            uint curTiles = map.GetNeighbours(paddedId);
            if (dynamicMap != null)
            {
                uint dynamicTiles = dynamicMap.GetNeighbours(paddedId);
                curTiles &= dynamicTiles;
            }

            int x1, y1, x2, y2;
            int tdx1, tdy1, tdx2, tdy2;
            
            Direction curDir = dir[0];

            if (curDir == dir[1])
            {
                int nextX_DirPaddedId = GetNeighbourPaddedId(paddedId, dir[1]);
                map.ToUnpadded_X_Y(nextX_DirPaddedId, out x1, out y1);
                tdx1 = endX - x1;
                tdy1 = endY - y1;

                nextDeltax1 = tdx1;
                nextDeltay1 = tdy1;
                nextPaddedId = nextX_DirPaddedId;
            }
            else
            {
                int nextY_DirPaddedId = GetNeighbourPaddedId(paddedId, dir[2]);
                map.ToUnpadded_X_Y(nextY_DirPaddedId, out x2, out y2);
                tdx2 = endX - x2;
                tdy2 = endY - y2;

                nextDeltax1 = tdx2;
                nextDeltay1 = tdy2;
                nextPaddedId = nextY_DirPaddedId;
            }
            
            //假如本次穿越了，就给dir[0]换方向
            bool curPosEq = Math.Abs(deltaX) * Math.Abs(deltay1) == Math.Abs(deltaY) * Math.Abs(deltax1);
            if (deltaX==0 || deltaY==0)
            {

            }
            else if (curPosEq)
            {
                switchDir(dir);
            }
            else if(nextDeltax1 !=0 && nextDeltay1 !=0)
            {
                bool temp;//不再两边时是否交线
                temp = (Math.Abs(deltaX) * Math.Abs(deltay1) > Math.Abs(deltaY) * Math.Abs(deltax1))
                 != (Math.Abs(deltaX) * Math.Abs(nextDeltay1) > Math.Abs(deltaY) * Math.Abs(nextDeltax1));
                if (temp)
                {
                    switchDir(dir);
                }
            }

            if (nextDeltax1 == 0)// 矫正偏离，如已经触及边缘 矫正下
            {
                dir[0] = dir[2];
            }
            else if (nextDeltay1 == 0)
            {
                dir[0] = dir[1];
            }
            if(nextDeltax1 == 0 && nextDeltay1 == 0)
            {
                nextPaddedId = endId;
                return true;
            }

            return CheckDirectionWalkable(curTiles, curDir);
        }

        public void switchDir(Direction[] dir)
        {
            if (dir[0] == dir[1])
            {
                dir[0] = dir[2];
            }
            else
            {
                dir[0] = dir[1];
            }
        }

        public bool CheckDirectionWalkable(uint curTiles, Direction dir)
        {
            switch (dir)
            {
                case Direction.EAST:
                    if ((curTiles & 1024) == 1024)//1024=0000 0000 0100 0000 0000
                    {
                        return true;
                    }
                    return false;
                case Direction.NORTHEAST:
                    if ((curTiles & 4) == 4)//4=0000 0000 0000 0000 0100
                    {
                        return true;
                    }
                    return false;
                case Direction.NORTH:
                    if ((curTiles & 2) == 2)//2=0000 0000 0000 0000 0010
                    {
                        return true;
                    }
                    return false;
                case Direction.NORTHWEST:
                    if ((curTiles & 1) == 1)//1=0000 0000 0000 0000 0001
                    {
                        return true;
                    }
                    return false;
                case Direction.WEST:
                    if ((curTiles & 256) == 256)//256=0000 0000 0001 0000 0000
                    {
                        return true;
                    }
                    return false;
                case Direction.SOUTHWEST:
                    if ((curTiles & 65536) == 65536)//65536=0001 0000 0000 0000 0000
                    {
                        return true;
                    }
                    return false;
                case Direction.SOUTH:
                    if ((curTiles & 131072) == 131072)//131072=0010 0000 0000 0000 0000
                    {
                        return true;
                    }
                    return false;
                case Direction.SOUTHEAST:
                    if ((curTiles & 262144) == 262144)//262144=0100 0000 0000 0000 0000
                    {
                        return true;
                    }
                    return false;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// 考虑优化45度和直线之间的中间跳点，默认从跳点1经跳点2到跳点3的连线不与坐标轴平行
        /// </summary>
        /// <param name="paddedIdStart"></param>
        /// <param name="paddedIdEnd"></param>
        /// <returns></returns>
        public bool JumpPointVisibleFromTo(int paddedIdStart, int paddedIdEnd)
        {
            int sx, sy, ex, ey;
            map.ToUnpadded_X_Y(paddedIdStart, out sx, out sy);
            map.ToUnpadded_X_Y(paddedIdEnd, out ex, out ey);

            int deltaX = ex - sx;
            int deltaY = ey - sy;

            Direction realxDir;
            Direction realyDir;
            if (deltaX >= 0 && deltaY >= 0)
            {
                realxDir = Direction.EAST;
                realyDir = Direction.SOUTH;
            }
            else if (deltaX < 0 && deltaY < 0)
            {
                realxDir = Direction.WEST;
                realyDir = Direction.NORTH;
            }
            else if (deltaX < 0)
            {
                realxDir = Direction.WEST;
                realyDir = Direction.SOUTH;
            }
            else
            {
                realxDir = Direction.EAST;
                realyDir = Direction.NORTH;
            }

            //Console.WriteLine($"from start {sx} {sy} to end {ex} {ey} dlt {deltaX} {deltaY} dir {realxDir} dir {realyDir}");

            Direction lastRealDir;
            if (Math.Abs(deltaX) > Math.Abs(deltaY))
            {
                lastRealDir = realxDir;
            }
            else
            {
                lastRealDir = realyDir;
            }

            Direction longDir = lastRealDir;


            int nextPaddedId = paddedIdStart;
            Direction[] directions = new Direction[3];
            directions[0] = lastRealDir;//下一次计算参考方向
            directions[1] = realxDir;
            directions[2] = realyDir;

            int curId;
            uint curTiles = map.GetNeighbours(paddedIdStart);
            if (dynamicMap != null)
            {
                uint dynamicTiles = dynamicMap.GetNeighbours(paddedIdStart);
                curTiles = curTiles & dynamicTiles;
            }

            if (CheckDirectionWalkable(curTiles, lastRealDir))
            {
                curId = paddedIdStart;
                nextPaddedId = GetNeighbourPaddedId(paddedIdStart, lastRealDir);
            }
            else
            {
                return false;
            }
            int curDeltax1, curDeltay1;
            map.ToUnpadded_X_Y(curId, out curDeltax1, out curDeltay1);
            curDeltax1 = deltaX;
            curDeltay1 = deltaY;
            while (nextPaddedId != paddedIdEnd)
            {
                //int debugX, debugY, debugEndX, debugEndY, geoX, geoY, geoEX, geoEY;
                //map.ToPadded_X_Y(curId, out debugX, out debugY);
                //map.ToGeo_X_Y(curId, out geoX, out geoY);
                //map.ToGeo_X_Y(paddedIdEnd, out geoEX, out geoEY);
                //debugEndX = ex;
                //debugEndY = ey;
                //Console.WriteLine($@"pad {debugX} {debugY} geo {geoX} {geoY} End {debugEndX} {debugEndY} EndGeo {geoEX} {geoEY} curDelta {curDeltax1} {curDeltay1} refDir {directions[0]} {directions[1]} {directions[2]}");

                if (!CheckNextPaddedIdForDirectPath(deltaX, deltaY, curDeltax1, curDeltay1, ex, ey, directions,
                    longDir, curId, paddedIdEnd, out nextPaddedId, out curDeltax1, out curDeltay1))
                {
                    //int debugSX, debugSY;
                    //map.ToGeo_X_Y(paddedIdStart, out debugSX, out debugSY);
                    //Console.WriteLine($"from {debugSX} {debugSY} to {geoEX} {geoEY} notVisible");
                    return false;
                }
                curId = nextPaddedId;
            }
            return true;
        }

        public int GetNeighbourPaddedId(int paddedId, Direction dir)
        {
            int x, y;
            map.ToUnpadded_X_Y(paddedId, out x, out y);
            int x1 = x, y1 = y;
            switch (dir)
            {
                case Direction.EAST:
                    x1 += 1;
                    return map.ToPaddedId(x1, y1);
                case Direction.NORTHEAST:
                    x1 += 1;
                    y1 -= 1;
                    return map.ToPaddedId(x1, y1);
                case Direction.NORTH:
                    y1 -= 1;
                    return map.ToPaddedId(x1, y1);
                case Direction.NORTHWEST:
                    x1 -= 1;
                    y1 -= 1;
                    return map.ToPaddedId(x1, y1);
                case Direction.WEST:
                    x1 -= 1;
                    return map.ToPaddedId(x1, y1);
                case Direction.SOUTHWEST:
                    x1 -= 1;
                    y1 += 1;
                    return map.ToPaddedId(x1, y1);
                case Direction.SOUTH:
                    y1 += 1;
                    return map.ToPaddedId(x1, y1);
                case Direction.SOUTHEAST:
                    x1 += 1;
                    y1 += 1;
                    return map.ToPaddedId(x1, y1);
                default:
                    break;
            }
            return paddedId;
        }

        /// <summary>
        /// 假装每次使用第一象限，然后获取假装的方向的真实方向
        /// </summary>
        /// <param name="direct">想要探索的方向</param>
        /// <param name="quad">所处的象限</param>
        /// <returns>第一象限的方向转为其他象限后的方向</returns>
        public Direction GetFirstQuadrantDir2OtherQuad(Direction direct, Quadrant quad)
        {
            switch (quad)
            {
                case Quadrant.FIRST:
                    break;
                case Quadrant.SECOND:
                    if (direct == Direction.EAST)
                    {
                        return Direction.WEST;
                    }
                    else if (direct == Direction.WEST)
                    {
                        return Direction.EAST;
                    }
                    break;
                case Quadrant.THIRD:
                    if (direct == Direction.EAST)
                    {
                        return Direction.WEST;
                    }
                    else if (direct == Direction.WEST)
                    {
                        return Direction.EAST;
                    }
                    else if (direct == Direction.SOUTH)
                    {
                        return Direction.NORTH;
                    }
                    else if (direct == Direction.NORTH)
                    {
                        return Direction.SOUTH;
                    }
                    break;
                case Quadrant.FOURTH:
                    if (direct == Direction.SOUTH)
                    {
                        return Direction.NORTH;
                    }
                    else if (direct == Direction.NORTH)
                    {
                        return Direction.SOUTH;
                    }
                    break;
                default:
                    break;
            }
            return direct;
        }

        public void SetDynamicGrid(GridMap dynamicMap, GridMap rDynamicMap)
        {
            this.dynamicMap = dynamicMap;
            jpl.SetDynamicGrid(dynamicMap, rDynamicMap);
        }
    }
}
