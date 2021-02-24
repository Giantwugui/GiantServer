using System;
using System.Collections.Generic;

namespace JumpPointSearch
{
    public class NativeMap
    {
        public readonly int MinGeoX;
        public readonly int MaxGeoX;
        public readonly int MinGeoY;
        public readonly int MaxGeoY;
        public readonly int Width; // 本质上是水平方向的坐标点的个数，不是绝对距离，两者差1
        public readonly int Height; // 本质上是垂直方向的坐标点的个数，不是绝对距离，两者差1

        public NativeMap(int minGeoX, int maxGeoX, int minGeoY, int maxGeoY)
        {
            MinGeoX = minGeoX;
            MaxGeoX = maxGeoX;
            MinGeoY = minGeoY;
            MaxGeoY = maxGeoY;

            Width = MaxGeoX - MinGeoX + 1;
            Height = MaxGeoY - MinGeoY + 1;
        }

        public NativeMap(NativeMap map)
        {
            MinGeoX = map.MinGeoX;
            MaxGeoX = map.MaxGeoX;
            MinGeoY = map.MinGeoY;
            MaxGeoY = map.MaxGeoY;

            Width = MaxGeoX - MinGeoX + 1;
            Height = MaxGeoY - MinGeoY + 1;
        }

        /// <summary>
        /// 根据unpadded 绝对坐标系 转为 前端坐标系的地理坐标
        /// </summary>
        /// <param name="unpaddedX">绝对坐标系的x坐标，即距离左上角向右水平距离</param>
        /// <param name="unpaddedY">绝对坐标系的y坐标，即距离左上角向下垂直距离</param>
        /// <param name="geoX"></param>
        /// <param name="geoY"></param>
        public void ToGeo_X_Y(int unpaddedX, int unpaddedY, out int geoX, out int geoY)
        {
            geoX = MinGeoX + unpaddedX;
            geoY = MaxGeoY - unpaddedY;
        }

        public void ToUnpadded_X_Y(int geoX, int geoY, out int unpaddedX, out int unpaddedY)
        {
            unpaddedX = geoX - MinGeoX;
            unpaddedY = MaxGeoY - geoY;
        }
    }

    /// <summary>
    /// 地理信息 
    /// 
    /// 注意 对于坐标变量
    /// 以geo开头的，如geoX，geoY，表示游戏实际地图的地理坐标 我们定义该坐标系为前端坐标系
    /// 普通的x，y，对应的坐标系非前端坐标系 为下图坐标系 即距离左上角起始点的width和height距离
    /// 这里 我们定义下图的坐标系为 绝对坐标系
    /// 
    /// （0，0）------------------------------->x轴正方形
    ///  |
    ///  |
    ///  |
    ///  |           (x,y)
    ///  |
    ///  |
    ///  |
    ///  |
    ///  |
    ///  |
    ///  |
    ///  ˇ
    ///  y轴正方向
    /// 
    /// </summary>

    public class GridMap
    {

        private const int paddedRowsBeforeFirstRow = 3;
        private const int paddedRowsAfterLastRow = 3;

        private NativeMap nativeMap; // 原始地图的信息

        private int dbWidth;
        private int dbHeight;
        private int dbSize;
        private int maxDbIndex;
        private byte[] db; // 地图经Padding和Width压缩后的信息保存在db中，每个bit表示一个坐标点, 0表示不可走，1表示可走

        private int paddedWidth;
        private int paddedHeight;
        private int paddingPerRow;

        /// <summary>
        /// 初始化GridMap
        /// </summary>
        /// <param name="walkableList">KeyValuePair Key=X Value=Y</param>
        public GridMap(int minX, int maxX, int minY, int maxY)
        {
            nativeMap = new NativeMap(minX, maxX, minY, maxY);
            InitDb();
        }

        public GridMap(GridMap map)
        {
            nativeMap = new NativeMap(map.nativeMap);
            InitDb();
        }

        public void SetAllWalkable()
        {
            for(int i=0; i<db.Length; i++)
            {
                db[i] = byte.MaxValue;
            }
        }

        public GridMap CreateRMap(GridMap map)
        {
            int mapHeight = map.NativeMapHeight();
            int mapWidth = map.NativeMapWidth();
            int rMapHeight = mapWidth;
            int rMapWidth = mapHeight;
            // rMap
            GridMap rMap = new GridMap(0, rMapWidth - 1, 0, rMapHeight - 1);
            return rMap;
        }

        public void InitWalkables(List<KeyValuePair<int, int>> walkableList)
        {
            foreach (var pos in walkableList)
            {
                int x, y;
                nativeMap.ToUnpadded_X_Y(pos.Key, pos.Value, out x, out y);
                SetLabel(ToPaddedId(x, y), true);
            }
        }

        public int NativeMapHeight()
        {
            return nativeMap.Height;
        }

        public int NativeMapWidth()
        {
            return nativeMap.Width;
        }

        public int PaddedMapWidth()
        {
            return paddedWidth;
        }

        public int PaddedMapHeight()
        {
            return paddedHeight;
        }
        public int ToPaddedId(int unpaddedX, int unpaddedY)
        {
            return ToPaddedId(unpaddedY * nativeMap.Width + unpaddedX);
        }

        // unpaddedId 从左上角起始点开始开始依次向右对应的id
        private int ToPaddedId(int unpaddedId)
        {
            return unpaddedId +
                // 越过填充点
                paddedRowsBeforeFirstRow * paddedWidth +
                // 越过补齐点
                (unpaddedId / nativeMap.Width) * paddingPerRow;
        }

        public int GeoToPaddedId(int geoX, int geoY)
        {
            int unpaddedX, unpaddedY;
            nativeMap.ToUnpadded_X_Y(geoX, geoY, out unpaddedX, out unpaddedY);
            return ToPaddedId(unpaddedX, unpaddedY);
        }

        public void PaddedIdToGeo(int paddedId, out int geoX, out int geoY)
        {
            int unpaddedX, unpaddedY;
            ToUnpadded_X_Y(paddedId, out unpaddedX, out unpaddedY);
            nativeMap.ToGeo_X_Y(unpaddedX, unpaddedY, out geoX, out geoY);
        }

        /// <summary>
        /// 根据padded id返回未经补齐前绝对坐标系下的x y
        /// 即返回未经补齐下该点距离左上角的width height
        /// 取得该x y后可以转换为客户端坐标系
        /// </summary>
        /// <param name="paddedId"></param>
        /// <param name="unpaddedX"></param>
        /// <param name="unpaddedY"></param>
        public void ToUnpadded_X_Y(int paddedId, out int unpaddedX, out int unpaddedY)
        {
            paddedId -= paddedRowsBeforeFirstRow * paddedWidth;
            unpaddedY = paddedId / paddedWidth;
            unpaddedX = paddedId % paddedWidth;
        }

        public void ToPadded_X_Y(int paddedId, out int paddedX, out int paddedY)
        {
            paddedY = paddedId / paddedWidth;
            paddedX = paddedId % paddedWidth;
        }

        public void ToGeo_X_Y(int paddedId, out int geoX, out int geoY)
        {
            int x, y;
            ToUnpadded_X_Y(paddedId, out x, out y);
            nativeMap.ToGeo_X_Y(x, y, out geoX, out geoY);
        }

        public void SetLabel(int paddedId, bool label)
        {
            int dbindex = paddedId >> Constants.LOG2_DBWORD_BITS; // 快速除以8 对应一维数组的下标
            if (dbindex > maxDbIndex) { return; }

            byte bitmask = (byte)(1 << (paddedId & Constants.DBWORD_BITS_MASK)); // 快速对8取余

            if (label)
            {
                db[dbindex] |= bitmask; // 可走 该位置位1
            }
            else
            {
                db[dbindex] &= (byte)~bitmask; // 不可走 该为置位0
            }
        }

        public void SetMultiLabel(int dbIndex, int startBit, int endBit, bool label)
        {
            if (dbIndex > maxDbIndex || startBit > endBit) { return; }
            if(startBit == 0 && endBit == 7)
            {
                if (label)
                {
                    db[dbIndex] = (byte)(db[dbIndex] & 255);
                }
                else
                {
                    db[dbIndex] = (byte)(db[dbIndex] & 0);
                }
            }
            else
            {
                byte bitmask = 0;
                for(int offset = startBit; offset <= endBit; offset++)
                {
                    bitmask |= (byte)(1 << offset);
                }
                if (label)
                {
                    db[dbIndex] |= bitmask; // 可走 该位置位1
                }
                else
                {
                    db[dbIndex] &= (byte)~bitmask; // 不可走 该为置位0
                }
            }

        }

        // 获取该点是否可走
        public bool GetLabel(int paddedId)
        {
            byte bitmask = 1;
            bitmask <<= (paddedId & Constants.DBWORD_BITS_MASK);
            int dbindex = paddedId >> Constants.LOG2_DBWORD_BITS;
            if (dbindex > maxDbIndex) { return false; }
            return (db[dbindex] & bitmask) != 0;
        }

        private void InitDb()
        {
            // 以 49 * 49 的地图为例
            dbHeight = nativeMap.Height + paddedRowsBeforeFirstRow + paddedRowsAfterLastRow; // 49 + 6 = 55
            dbWidth = (nativeMap.Width >> Constants.LOG2_DBWORD_BITS) + 1; // 49/8 + 1 = 7, 宽度被压缩
            dbSize = dbWidth * dbHeight; // 需要db_size的长度保存地图
            maxDbIndex = dbSize - 1;

            paddedHeight = dbHeight;
            paddedWidth = dbWidth * Constants.DBWORD_BITS; // 7 * 8 = 56位 补齐由49补到56位
            paddingPerRow = paddedWidth - nativeMap.Width; // 56 - 49 = 7

            db = new byte[dbSize]; // 默认为0，全部不可走
        }

        /// <summary>
        /// 返回paddedId的9宫格可走信息，保存在返回的tiles[3]的低三位中
        /// tiles[0]上方，tiles[1]为paddedId所在行，tiles[2]为下方 tile[3]无意义，只为凑齐uint高8位
        /// 需要注意的是，由于paddedId小的放在低位，因此高低位方向与位置的相对东西方向相反
        /// 比如对于tiles[0] 0位表示NorthWest，1位表示North，2位表示NorthEast
        /// </summary>
        /// <param name="paddedId"></param>
        /// <returns></returns>
        public uint GetNeighbours(int paddedId)
        {
            byte[] tiles = new byte[4];

            int bitOffset = (paddedId & Constants.DBWORD_BITS_MASK);
            int dbindex = paddedId >> Constants.LOG2_DBWORD_BITS;

            int pos1 = dbindex - dbWidth;
            int pos2 = dbindex;
            int pos3 = dbindex + dbWidth;

            // 获取各自邻居信息，并将邻居信息右移到最低位
            tiles[0] = CalcDbRightShiftByte(pos1 - 1, bitOffset + 7);
            tiles[1] = CalcDbRightShiftByte(pos2 - 1, bitOffset + 7);
            tiles[2] = CalcDbRightShiftByte(pos3 - 1, bitOffset + 7);
            return BitConverter.ToUInt32(tiles, 0);
        }

        // 由于是LittleEndian 所以可以直接转换。对于BitEndian不可以
        private byte CalcDbRightShiftByte(int index, int offset)
        {
            uint val = BitConverter.ToUInt32(db, index);
            val = val >> offset;
            return (byte)val;
        }

        // 获取上一行 当前行 下一行的 数组中为向下方向(即地图方向的East方向)的32bit数据 用于快速定位JumpPoint
        // 当前位被偏移到32位中的最低位
        public uint[] GetNeighbours_32Bit(int paddedId)
        {
            uint[] tiles = new uint[3];
            int bitOffset = (paddedId & Constants.DBWORD_BITS_MASK);
            int dbindex = paddedId >> Constants.LOG2_DBWORD_BITS;

            int pos1 = dbindex - dbWidth;
            int pos2 = dbindex;
            int pos3 = dbindex + dbWidth;

            tiles[0] = CalcDbRightShiftUint(pos1, bitOffset);
            tiles[1] = CalcDbRightShiftUint(pos2, bitOffset);
            tiles[2] = CalcDbRightShiftUint(pos3, bitOffset);

            return tiles;
        }

        // 获取上一行 当前行 下一行的 数组中为向上方向(即地图方向的West方向)的32bit数据 用于快速定位JumpPoint
        // 当前位被偏移到32位中的最高位.
        public uint[]
        GetNeighbours_Upper32Bit(int paddedId)
        {
            uint[] tiles = new uint[3];
            int bitOffset = (paddedId & Constants.DBWORD_BITS_MASK);
            int dbindex = paddedId >> Constants.LOG2_DBWORD_BITS;

            // 先向上偏移32位 再右移bitOffset+1 得到的32位数 就是paddedId向西32位，paddedId在最高位
            dbindex -= 4;

            int pos1 = dbindex - dbWidth;
            int pos2 = dbindex;
            int pos3 = dbindex + dbWidth;

            tiles[0] = CalcDbRightShiftUint(pos1, bitOffset + 1);
            tiles[1] = CalcDbRightShiftUint(pos2, bitOffset + 1);
            tiles[2] = CalcDbRightShiftUint(pos3, bitOffset + 1);

            return tiles;
        }

        // 由于是LittleEndian 所以可以直接转换。对于BigEndian不可以
        private uint CalcDbRightShiftUint(int index, int offset)
        {
            ulong val = BitConverter.ToUInt64(db, index);
            val = val >> offset;
            return (uint)val;
        }

        /// <summary>
        /// 以geoX geoY为中心，边长为2r的正方形设置为障碍或非障碍
        /// </summary>
       
        public void SetDynamicWalkable_Old(int centerPaddedId, int r, bool walkable)
        {
            int startPaddedId = centerPaddedId - r - r * paddedWidth;
            int endPaddedId = centerPaddedId + r + r * paddedWidth;

            int height = 2 * r + 1;
            int width = 2 * r + 1;

            if (!((startPaddedId > 0) && (startPaddedId <= endPaddedId)))
            {
                return;
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int paddedId = startPaddedId + x + y * paddedWidth;
                    SetLabel(paddedId, walkable);
                }
            }
        }

        public void SetDynamicWalkable(int centerPaddedId, int r, bool walkable)
        {
            int startPaddedId = centerPaddedId - r - r * paddedWidth;
            int endPaddedId = centerPaddedId + r + r * paddedWidth;

            int height = 2 * r + 1;
            int width = 2 * r + 1;

            if (!((startPaddedId > 0) && (startPaddedId <= endPaddedId)))
            {
                return;
            }

            // 该行的startPaddedId 和EndPaddedId
            int lineStartId = 0;
            int lineEndId = 0; ;
            for (int y = 0; y < height; y++)
            {
                lineStartId = startPaddedId + y * paddedWidth;
                lineEndId = lineStartId + width - 1;

                // 计算该行的起始点对应 db的index和偏移量
                int dbStartIndex = lineStartId >> Constants.LOG2_DBWORD_BITS;
                int startBitOffset = lineStartId & Constants.DBWORD_BITS_MASK;

                // 计算该行的结束点对应 db的index和偏移量
                int dbEndIndex = lineEndId >> Constants.LOG2_DBWORD_BITS;
                int EndBitOffset = lineEndId & Constants.DBWORD_BITS_MASK;

                if (dbStartIndex > dbEndIndex)
                {
                    return;
                }

                // 如果中间行，则直接用Byte设置是否可走，节省位操作运算
                for (int line = dbStartIndex + 1; line < dbEndIndex; line++)
                {
                    SetMultiLabel(line, 0, 7, walkable);
                }

                // 处理首行和尾行
                // 只占一行的话
                if (dbStartIndex == dbEndIndex)
                {
                    SetMultiLabel(dbStartIndex, startBitOffset, EndBitOffset, walkable);
                }
                else
                {
                    SetMultiLabel(dbStartIndex, startBitOffset, 7, walkable);
                    SetMultiLabel(dbEndIndex, 0, EndBitOffset, walkable);
                }
            }
        }

        public int MapToRMapPaddedId(int paddedId, GridMap rMap)
        {
            if (rMap == null || paddedId == Constants.INF) { return paddedId; }

            int x, y;
            int rx, ry;
            ToUnpadded_X_Y(paddedId, out x, out y);
            ry = x;
            rx = NativeMapHeight() - y - 1;
            return rMap.ToPaddedId(rx, ry);
        }


        public void PrintUnwalkable()
        {
            for(int y=nativeMap.MinGeoY; y < nativeMap.MaxGeoY; y++)
            {
                bool print = false;
                string str = string.Empty;
                for(int x = nativeMap.MinGeoX; x < nativeMap.MaxGeoX; x++)
                {
                    int unpaddedX, unpaddedY;
                    nativeMap.ToUnpadded_X_Y(x, y, out unpaddedX, out unpaddedY);
                    int id = ToPaddedId(unpaddedX, unpaddedY);
                    bool label = GetLabel(id);
                    if(!label)
                    {
                        print = true;
                        str += string.Format($" x_y {x} {y} ");
                    }
                }
                if(print)
                {
                    //Logger.Log.Warn(str);
                }
            }
        }
    }


}
