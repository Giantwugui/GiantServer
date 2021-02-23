using Giant.Logger;
using System.Collections.Generic;
using UnityEngine;

namespace Giant.Battle
{
    public class RegionManager
    {
        public int REGION_SIZE = 7;
        // 地图中所有格子
        Region[] regionList;
        // 横向格子数
        int width;
        // 纵向格子数
        int height;
        // 起始点X坐标
        int baseX;
        // 起始点Y坐标
        int baseY;
        // 所属的map
        MapScene map;
        // 格子数
        int count;
        // 有PC存在的格子列表 后期用于优化怪物AI 往人堆里走
        Dictionary<int, Region> pcRegionList = new Dictionary<int, Region>();

        public bool Init(MapScene map, int width, int height, int baseX, int baseY)
        {
            this.map = map;
            this.baseX = baseX;
            this.baseY = baseY;
            //if (map.DungeonId == GameConfig.ArenaDungeonId 
            //    || map.DungeonId == GameConfig.ArenaForFunDungeonId
            //    || map.DungeonId == GameConfig.ArenaRunforDungeonId)
            //{
            //    // 如果是竞技场 
            //    //由于地图中FieldObject数量最多为10 格子放大一倍 看得更远更快 打得更准 增强体验感 不影响效率
            //    REGION_SIZE *= 2;
            //}
            this.width = (width + REGION_SIZE - 1) / REGION_SIZE;
            if (this.width < 0)
            {
                Log.Warn($"map {map.MapId} init region failed: width = {this.width}");
                return false;
            }

            this.height = (height + REGION_SIZE - 1) / REGION_SIZE;
            if (this.height < 0)
            {
                Log.Warn($"map {map.MapId} init region failed: height = {this.height}");
                return false;
            }

            count = this.width * this.height;
            regionList = new Region[count];
            for (int i = 0; i < count; i++)
            {
                regionList[i] = new Region();
            }
            //Log.Warn("width {0} height {1} count {2} minX {3} maxX {4} minY {5} maxY {6}", this.width, this.height, this.count, baseX, width + baseX, baseY, height + baseY);
            for (int i = 0; i < count; i++)
            {
                int x = this.baseX + (i % this.width) * REGION_SIZE;
                int y = this.baseY + (i / this.width) * REGION_SIZE;

                regionList[i].Init(i, x, y, REGION_SIZE, REGION_SIZE, map);

                regionList[i].LinkNeighbor(RegionDirection.REGION_NW, GetRegion(i, -1, 1));
                regionList[i].LinkNeighbor(RegionDirection.REGION_N, GetRegion(i, 0, 1));
                regionList[i].LinkNeighbor(RegionDirection.REGION_NE, GetRegion(i, 1, 1));

                regionList[i].LinkNeighbor(RegionDirection.REGION_W, GetRegion(i, -1, 0));
                regionList[i].LinkNeighbor(RegionDirection.REGION_E, GetRegion(i, 1, 0));

                regionList[i].LinkNeighbor(RegionDirection.REGION_SW, GetRegion(i, -1, -1));
                regionList[i].LinkNeighbor(RegionDirection.REGION_S, GetRegion(i, 0, -1));
                regionList[i].LinkNeighbor(RegionDirection.REGION_SE, GetRegion(i, 1, -1));
            }
            foreach (var item in regionList)
            {
                //Log.Warn("map {0} xBegin {1} yBegin {2} index {3} ", map.MapID, item.x, item.y, item.index);
                //item.PrintNeigbor();
            }
            return true;
        }

        public Region GetRegion(int index, int dx, int dy)
        {
            int y = index / width;
            int x = index % width;

            y += dy;
            x += dx;

            if (0 <= y && y < height)
            {
                if (0 <= x && x < width)
                {
                    return regionList[x + y * width];
                }
            }
            return null;
        }

        public Region GetRegion(Vector2 pos)
        {
            int x = (int)((pos.x - baseX) / REGION_SIZE);
            int y = (int)((pos.y - baseY) / REGION_SIZE);

            if (!(0 <= y && y < height))
            {
                return null;
            }
            if (!(0 <= x && x < width))
            {
                return null;
            }
            return regionList[y * width + x];
        }

        public int GetRegionIndex(Vector2 pos)
        {
            Region region = GetRegion(pos);

            if (null == region)
            {
                return -1;
            }

            return region.index;
        }
    }
}


