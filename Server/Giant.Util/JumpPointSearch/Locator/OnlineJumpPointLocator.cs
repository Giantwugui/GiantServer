using CommonUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpPointSearch
{
    /// <summary>
    /// 在GridMap中定位JumpPoint
    /// 在线计算跳点信息（即无预处理缓存数据，支持在线动态占位）
    /// </summary>
    public class OnlineJumpPointLocator
    {
        private GridMap map;
        private GridMap rMap; // map经顺时针旋转90度

        private GridMap dynamicMap;
        private GridMap rDynamicMap;

        public OnlineJumpPointLocator(GridMap map)
        {
            this.map = map;
            rMap = CreateRmap();
        }

        /// <summary>
        /// 创建顺时针旋转90的地图，对于在map向North, South搜索时，等价于在rmap的East, West搜索，复用算法
        /// 由于在rmap行走只是在相对坐标系下行走，需要最后换算为map下的unpadded id
        /// 因此rMap下对应的前端坐标系（geoMinX geoMaxX geoMinY geoMaxY）无意义，只要保证宽度和长度正确即可
        /// </summary>
        private GridMap CreateRmap()
        {
            int mapHeight = map.NativeMapHeight();
            int mapWidth = map.NativeMapWidth();
            int rMapHeight = mapWidth;
            int rMapWidth = mapHeight;
            // rMap
            GridMap rMap = new GridMap(0, rMapWidth -1, 0, rMapHeight -1);

            // 由unpadded map信息依次初始化
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    bool label = map.GetLabel(map.ToPaddedId(x, y));
                    int rx = ((rMapWidth - 1) - y);
                    int ry = x;
                    int rpaddedId = rMap.ToPaddedId(rx, ry);
                    rMap.SetLabel(rpaddedId, label);
                }
            }
            return rMap;
        }

        /// <summary>
        /// 向某个方向寻找jump point JPS算法的核心函数
        /// </summary>
        /// <param name="d">方向</param>
        /// <param name="curNodeId">当前node id</param>
        /// <param name="goalId">终点 id</param>
        /// <param name="jumpNodeId">找到的跳点</param>
        /// <param name="jumpcost">到该跳点的cost</param>
        public void Jump(Direction d, int curNodeId, int goalId, out int jumpNodeId, out int jumpCost)
        {
            switch (d)
            {
                case Direction.NORTH:
                    JumpNorth(curNodeId, goalId, out jumpNodeId, out jumpCost);
                    break;
                case Direction.SOUTH:
                    JumpSouth(curNodeId, goalId, out jumpNodeId, out jumpCost);
                    break;
                case Direction.EAST:
                    JumpEast(curNodeId, goalId, out jumpNodeId, out jumpCost);
                    break;
                case Direction.WEST:
                    JumpWest(curNodeId, goalId, out jumpNodeId, out jumpCost);
                    break;
                case Direction.NORTHEAST:
                    JumpNorthEast(curNodeId, goalId, out jumpNodeId, out jumpCost);
                    break;
                case Direction.NORTHWEST:
                    JumpNorthWest(curNodeId, goalId, out jumpNodeId, out jumpCost);
                    break;
                case Direction.SOUTHEAST:
                    JumpSouthEast(curNodeId, goalId, out jumpNodeId, out jumpCost);
                    break;
                case Direction.SOUTHWEST:
                    JumpSouthWest(curNodeId, goalId, out jumpNodeId, out jumpCost);
                    break;
                default:
                    jumpNodeId = Constants.INF;
                    jumpCost = Constants.INF;
                    break;
            }
        }

        /// <summary>
        /// 将map下的paddedId转为rMap下的paddedId
        /// </summary>
        /// <param name="paddedId"></param>
        /// <returns></returns>
        private int MapToRMapPaddedId(int paddedId)
        {
            if (paddedId == Constants.INF) { return paddedId; }

            int x, y;
            int rx, ry;
            map.ToUnpadded_X_Y(paddedId, out x, out y);
            ry = x;
            rx = map.NativeMapHeight() - y - 1;
            return rMap.ToPaddedId(rx, ry);
        }

        private int RMapToMapPaddedId(int rMapPaddedId)
        {
            if (rMapPaddedId == Constants.INF) { return rMapPaddedId; }

            int x, y;
            int rx, ry;
            rMap.ToUnpadded_X_Y(rMapPaddedId, out rx, out ry);
            x = ry;
            y = rMap.NativeMapWidth() - rx - 1;
            return map.ToPaddedId(x, y);
        }

        private void JumpNorth(int nodeId, int goalId, out int jumpNodeId, out int jumpCost)
        {
            nodeId = MapToRMapPaddedId(nodeId);
            goalId = MapToRMapPaddedId(goalId);
            __JumpNorth(nodeId, goalId, out jumpNodeId, out jumpCost, rMap);
            jumpNodeId = RMapToMapPaddedId(jumpNodeId);
        }

        private void __JumpNorth(int nodeId, int goalId, out int jumpNodeId, out int jumpCost, GridMap myMap)
        {
            // 等价于rMap中向East搜寻JumpPoint
            __JumpEast(nodeId, goalId, out jumpNodeId, out jumpCost, rMap, rDynamicMap);
        }

        private void JumpSouth(int nodeId, int goalId, out int  jumpNodeId, out int jumpCost)
        {
            nodeId = MapToRMapPaddedId(nodeId);
            goalId = MapToRMapPaddedId(goalId);
            __JumpSouth(nodeId, goalId, out jumpNodeId, out jumpCost, rMap);
            jumpNodeId = RMapToMapPaddedId(jumpNodeId);
        }

        private void __JumpSouth(int nodeId, int goalId, out int  jumpNodeId, out int  jumpCost, GridMap myMap)
        {
            // 等价于rMap中向West搜索JumpPoint
            __JumpWest(nodeId, goalId, out jumpNodeId, out jumpCost, rMap, rDynamicMap);
        }

        private void JumpEast(int nodeId, int goalId, out int jumpNodeId, out int jumpCost)
        {
            __JumpEast(nodeId, goalId, out jumpNodeId, out jumpCost, map, dynamicMap);
        }

        private void __JumpEast(int nodeId, int goalId, out int jumpNodeId, out int jumpCost, GridMap myMap, GridMap myDynamicMap)
        {
            jumpNodeId = nodeId;

            uint[] neis;
            bool deadend = false;

            while (true)
            {
                // 读取上一行 当前行 下一行向East方向32个node信息（包含当前node在最低位）
                // nei[0]对应上一行 nei[1]当前行 nei[2]对应下一行
                neis = myMap.GetNeighbours_32Bit(jumpNodeId);
                if(myDynamicMap != null)
                {
                    uint[] dynamicNeis = myDynamicMap.GetNeighbours_32Bit(jumpNodeId);
                    neis[0] = neis[0] & dynamicNeis[0];
                    neis[1] = neis[1] & dynamicNeis[1];
                    neis[2] = neis[2] & dynamicNeis[2];
                }

                // 根据上下行 计算JumpPoint
                uint forcedBits = (~neis[0] << 1) & neis[0];
                forcedBits |= (~neis[2] << 1) & neis[2];

                // 根据当前行 计算不可达点 则不能继续再按照当前方向（East）继续搜索
                uint deadendBits = ~neis[1];

                // 当前方向不可达或者有跳点 则需要停止
                uint stopBits = (forcedBits | deadendBits);
                if (stopBits > 0)
                {
                    uint stopPos = JPSExt.ffs(stopBits) - 1; // returns idx+1
                    jumpNodeId += (int)stopPos;
                    // 判断是当前行障碍在前 还是跳点在前
                    deadend = ((int)deadendBits & (1 << (int)stopPos)) > 0;
                    break;
                }

                // 没有满足的stop点 则按照当前方向继续搜索 
                // 注意 如果最后一个点的上下行由障碍 但是当前的neighbors信息不足以判断
                // 因此需要向下跳31个点，最后一个点需要重新计算
                jumpNodeId += 31;
            }

            int numSteps = jumpNodeId - nodeId;
            int goalDist = goalId - nodeId;
            if (goalDist >= 0 &&  numSteps > goalDist)
            {
                jumpNodeId = goalId;
                jumpCost = goalDist * Constants.ONE;
                return;
            }

            if (deadend)
            {
                // 从障碍点回退到上一个可达点
                if(numSteps > 0)
                {
                    numSteps--;
                }
                jumpNodeId = Constants.INF;
            }
            jumpCost = numSteps * Constants.ONE;
        }


        private void JumpWest(int nodeId, int goalId, out int jumpNodeId, out int jumpCost)
        {
            __JumpWest(nodeId, goalId, out jumpNodeId, out jumpCost, map, dynamicMap);
        }

        // 与jump east逻辑一样 方向反过来
        private void __JumpWest(int nodeId, int goalId, out int jumpNodeId, out int jumpCost, GridMap myMap, GridMap myDynamicMap)
        {
            bool deadend = false;
            uint[] neis;

            jumpNodeId = nodeId;
            while (true)
            {
                neis = myMap.GetNeighbours_Upper32Bit(jumpNodeId);
                if (myDynamicMap != null)
                {
                    uint[] dynamicNeis = myDynamicMap.GetNeighbours_Upper32Bit(jumpNodeId);
                    neis[0] = neis[0] & dynamicNeis[0];
                    neis[1] = neis[1] & dynamicNeis[1];
                    neis[2] = neis[2] & dynamicNeis[2];
                }
                uint forcedBits = (~neis[0] >> 1) & neis[0];
                forcedBits |= (~neis[2] >> 1) & neis[2];
                uint deadendBits = ~neis[1];

                uint stopBits = (forcedBits | deadendBits);
                if (stopBits > 0)
                {
                    uint stopPos = JPSExt.clz(stopBits);
                    jumpNodeId -= (int)stopPos;
                    deadend = (deadendBits & (0x80000000 >> (int)stopPos)) > 0;
                    break;
                }
                jumpNodeId -= 31;
            }

            int numSteps = nodeId - jumpNodeId;
            int goalDist = nodeId - goalId;
            if (goalDist >= 0 && numSteps > goalDist)
            {
                jumpNodeId = goalId;
                jumpCost = goalDist * Constants.ONE;
                return;
            }

            if (deadend)
            {
                if(numSteps > 0)
                {
                    numSteps--;
                }
                jumpNodeId = Constants.INF;
            }
            jumpCost = numSteps * Constants.ONE;
        }

        private void JumpNorthEast(int nodeId, int goalId, out int jumpNodeId, out int jumpCost)
        {
            uint numSteps = 0;

            int nextId = nodeId;
            int mapWidth = map.PaddedMapWidth();

            // 判断邻居障碍信息，不可走直接返回
            uint neis = map.GetNeighbours(nextId);
            if(dynamicMap != null)
            {
                uint dynamicNeis = dynamicMap.GetNeighbours(nextId);
                neis = dynamicNeis & neis;
            }
            if ((neis & 1542) != 1542) { jumpNodeId = Constants.INF; jumpCost = 0; return; }

            // North搜寻准备
            int rNextId = MapToRMapPaddedId(nextId);
            int rGoalId = MapToRMapPaddedId(goalId);
            int rMapWidth = rMap.PaddedMapWidth();
            while (true)
            {
                numSteps++;
                // NorthEast对角线 下一个JumpPoint East方向是上一行+1
                nextId = nextId - mapWidth + 1;
                rNextId = rNextId + rMapWidth + 1;

                int jpId1, jpId2;
                int cost1, cost2;

                __JumpNorth(rNextId, rGoalId, out jpId1, out cost1, rMap);
                if (jpId1 != Constants.INF) { break; }

                __JumpEast(nextId, goalId, out jpId2, out cost2, map, dynamicMap);
                if (jpId2 != Constants.INF) { break; }

                // 某个方向cost为0 说明有障碍 一步也走不动 对角线方向无法继续移动
                if (cost1 == 0 || cost2 == 0) { nextId = Constants.INF; break; }
            }
            jumpNodeId = nextId;
            jumpCost = (int)(numSteps * Constants.ROOT_TWO);
        }

        private void JumpNorthWest(int nodeId, int goalId, out int jumpNodeId, out int jumpCost)
        {
            int numSteps = 0;

            int nextId = nodeId;
            int mapWidth = map.PaddedMapWidth();

            uint neis =  map.GetNeighbours(nextId);
            if (dynamicMap != null)
            {
                uint dynamicNeis = dynamicMap.GetNeighbours(nextId);
                neis = dynamicNeis & neis;
            }
            if ((neis & 771) != 771) { jumpNodeId = Constants.INF; jumpCost = 0; return; }

            int rnextId = MapToRMapPaddedId(nextId);
            int rgoalId = MapToRMapPaddedId(goalId);
            int rMapWidth = rMap.PaddedMapWidth();
            while (true)
            {
                numSteps++;
                nextId = nextId - mapWidth - 1;
                rnextId = rnextId - (rMapWidth - 1);

                int jpId1, jpId2;
                int cost1, cost2;
                __JumpNorth(rnextId, rgoalId, out jpId1, out cost1, rMap);
                if (jpId1 != Constants.INF) { break; }
                __JumpWest(nextId, goalId, out jpId2, out cost2, map, dynamicMap);
                if (jpId2 != Constants.INF) { break; }

                if (cost1 == 0 || cost2 == 0) { nextId = Constants.INF; break; }
            }
            jumpNodeId = nextId;
            jumpCost = numSteps * Constants.ROOT_TWO;
        }

        private void JumpSouthEast(int nodeId, int goalId, out int jumpNodeId, out int jumpCost)
        {
            int numSteps = 0;

            int nextId = nodeId;
            int mapWidth = map.PaddedMapWidth();

            uint neis = map.GetNeighbours(nextId);
            if (dynamicMap != null)
            {
                uint dynamicNeis = dynamicMap.GetNeighbours(nextId);
                neis = dynamicNeis & neis;
            }
            if ((neis & 394752) != 394752) { jumpNodeId = Constants.INF; jumpCost = 0; return; }

            int rNextId = MapToRMapPaddedId(nextId);
            int rGoalId = MapToRMapPaddedId(goalId);
            int rMapWidth = rMap.PaddedMapWidth();
            while (true)
            {
                numSteps++;
                nextId = nextId + mapWidth + 1;
                rNextId = rNextId + rMapWidth - 1;

                int jpId1, jpId2;
                int cost1, cost2;
                __JumpSouth(rNextId, rGoalId, out jpId1, out cost1, rMap);
                if (jpId1 != Constants.INF) { break; }
                __JumpEast(nextId, goalId, out jpId2, out cost2, map, dynamicMap);
                if (jpId2 != Constants.INF) { break; }

                if (cost1 == 0 || cost2 == 0) { nextId = Constants.INF; break; }
            }
            jumpNodeId = nextId;
            jumpCost = numSteps * Constants.ROOT_TWO;
        }

        private void JumpSouthWest(int nodeId, int goalId, out int jumpNodeId, out int jumpCost)
        {
            int numSteps = 0;

            uint neis;
            int nextId = nodeId;
            int mapWidth = map.PaddedMapWidth();

            neis = map.GetNeighbours(nextId);
            if (dynamicMap != null)
            {
                uint dynamicNeis = dynamicMap.GetNeighbours(nextId);
                neis = dynamicNeis & neis;
            }
            if ((neis & 197376) != 197376) { jumpNodeId = Constants.INF; jumpCost = 0; return; }

            int rNextId = MapToRMapPaddedId(nextId);
            int rGoalId = MapToRMapPaddedId(goalId);
            int rMapWidth = rMap.PaddedMapWidth();
            while (true)
            {
                numSteps++;
                nextId = nextId + mapWidth - 1;
                rNextId = rNextId - (rMapWidth + 1);

                int jpId1, jpId2;
                int cost1, cost2;
                __JumpSouth(rNextId, rGoalId, out jpId1, out cost1, rMap);
                if (jpId1 != Constants.INF) { break; }
                __JumpWest(nextId, goalId, out jpId2, out cost2, map, dynamicMap);
                if (jpId2 != Constants.INF) { break; }

                if (cost1 == 0 || cost2 == 0) { nextId = Constants.INF; break; }
            }
            jumpNodeId = nextId;
            jumpCost = numSteps * Constants.ROOT_TWO;
        }

        public void SetDynamicGrid(GridMap dynamicMap, GridMap rDynamicMap)
        {
            this.dynamicMap = dynamicMap;
            this.rDynamicMap = rDynamicMap;
        }
    }
}
