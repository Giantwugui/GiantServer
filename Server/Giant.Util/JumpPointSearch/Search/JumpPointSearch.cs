using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpPointSearch
{
    public class JumpPointSearch
    {
        /// <summary>
        /// 根据当前方向和周围邻居的可达信息，返回需要后续执行Jump操作的方向
        /// </summary>
        /// <param name="d">由parent到当前node的方向</param>
        /// <param name="tiles">邻居9宫格的可达信息</param>
        /// <returns>需要检测的8个方向的按位或信息 返回的int只有低8位有意义</returns>
        public static int ComputeSuccessors(Direction d, uint tiles)
        {
            return ComputeForced(d, tiles) | ComputeNatural(d, tiles);
        }

        /// <summary>
        /// 返回当前node的强迫邻居 对角线切角情况下视为不可走
        /// </summary>
        /// <param name="d">由parent到当前node的方向</param>
        /// <param name="tiles">邻居9宫格的可达信息</param>
        /// <returns>需要检测的8个方向的按位或信息 返回的int只有低8位有意义</returns/
        private static int ComputeForced(Direction d, uint tiles)
        {
            // NB: to avoid branching statements, shift operations are
            // used below. The values of the shift constants correspond to 
            // direction values. 
           int  ret = 0;
            switch (d)
            {
                case Direction.NORTH:
                    // 右下 即SouthWest方不可走 则右方（West）右上 NorthWest为ForceNeighbor
                    if ((tiles & 65792) == 256) // 65792 = 0001 0000 0001 0000 0000; 256 = 0001 0000 0000
                    {
                        ret |= ((int)Direction.WEST | (int)Direction.NORTHWEST);
                    }
                    if ((tiles & 263168) == 1024) // 263168 = 0100 0000 0100 0000 0000; 1024 = 0100 0000 0000
                    {
                        ret |= ((int)Direction.EAST | (int)Direction.NORTHEAST);
                    }
                    break;
                case Direction.SOUTH:
                    if ((tiles & 257) == 256) // 257 = 0001 0000 0001; 256 = 0001 0000 0000
                    {
                        ret |= ((int)Direction.WEST | (int)Direction.SOUTHWEST);
                    }
                    if ((tiles & 1028) == 1024) // 1028 = 0100 0000 0100; 1024 = 0100 0000 0000
                    {
                        ret |= ((int)Direction.EAST | (int)Direction.SOUTHEAST);
                    }
                    break;
                case Direction.EAST:
                    if ((tiles & 3) == 2) // 3 = 0011; 2=0010
                    {
                        ret |= ((int)Direction.NORTH | (int)Direction.NORTHEAST);
                    }
                    if ((tiles & 196608) == 131072) // 196608 = 0011 0000 0000 0000 0000; 131072 = 0010 0000 0000 0000 0000
                    {
                        ret |= ((int)Direction.SOUTH | (int)Direction.SOUTHEAST);
                    }
                    break;
                case Direction.WEST:
                    if ((tiles & 6) == 2) // 6 = 0110; 2 = 0010
                    {
                        ret |= ((int)Direction.NORTH | (int)Direction.NORTHWEST);
                    }
                    if ((tiles & 393216) == 131072) // 393216 = 0110 0000 0000 0000 0000; 131072 = 0010 0000 0000 0000 0000
                    {
                        ret |= ((int)Direction.SOUTH | (int)Direction.SOUTHWEST);
                    }
                    break;
                default:
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 返回当前node的自然邻居 
        /// </summary>
        /// <param name="d">由parent到当前node的方向</param>
        /// <param name="tiles">邻居9宫格的可达信息</param>
        /// <returns>需要检测的8个方向的按位或信息 返回的int只有低8位有意义</returns>
        private static int ComputeNatural(Direction d, uint tiles)
        {
            // In the shift operations below the constant values
            // correspond to bit offsets for Direction.direction
            int ret = 0;
            int label = 0;
            switch (d)
            {
                case Direction.NORTH:
                    label = (tiles & 2) == 2 ? 1 : 0;
                    ret |= label << 0;
                    break;
                case Direction.SOUTH:
                    label = ((tiles & 131072) == 131072) ? 1 : 0;
                    ret |= label << 1;
                    break;
                case Direction.EAST:
                    label = ((tiles & 1024) == 1024) ? 1 : 0;
                    ret |= label << 2;
                    break;
                case Direction.WEST:
                    label = ((tiles & 256) == 256) ? 1 : 0;
                    ret |= label << 3;
                    break;
                case Direction.NORTHWEST:
                    label = ((tiles & 2) == 2) ? 1 : 0;
                    ret |= label << 0;

                    label = ((tiles & 256) == 256) ? 1 : 0;
                    ret |= label << 3;

                    label = ((tiles & 259) == 259) ? 1 : 0;
                    ret |= label << 5;
                    break;
                case Direction.NORTHEAST:
                    label = ((tiles & 2) == 2) ? 1 : 0;
                    ret |= label << 0;

                    label = ((tiles & 1024) == 1024) ? 1 : 0;
                    ret |= label << 2;

                    label = ((tiles & 1030) == 1030) ? 1 : 0;
                    ret |= label << 4;
                    break;
                case Direction.SOUTHWEST:
                    label = ((tiles & 131072) == 131072) ? 1 : 0;
                    ret |= label << 1;

                    label = ((tiles & 256) == 256) ? 1 : 0;
                    ret |= label << 3;

                    label = ((tiles & 196864) == 196864) ? 1 : 0;
                    ret |= label << 7;
                    break;
                case Direction.SOUTHEAST:
                    label = ((tiles & 131072) == 131072) ? 1 : 0;
                    ret |= label << 1;

                    label = ((tiles & 1024) == 1024) ? 1 : 0;
                    ret |= label << 2;

                    label = ((tiles & 394240) == 394240) ? 1 : 0;
                    ret |= label << 6;
                    break;
                default:
                    label = ((tiles & 2) == 2) ? 1 : 0;
                    ret |= label << 0;

                    label = ((tiles & 131072) == 131072) ? 1 : 0;
                    ret |= label << 1;

                    label = ((tiles & 1024) == 1024) ? 1 : 0;
                    ret |= label << 2;

                    label = ((tiles & 256) == 256) ? 1 : 0;
                    ret |= label << 3;

                    label = ((tiles & 259) == 259) ? 1 : 0;
                    ret |= label << 5;

                    label = ((tiles & 1030) == 1030) ? 1 : 0;
                    ret |= label << 4;

                    label = ((tiles & 196864) == 196864) ? 1 : 0;
                    ret |= label << 7;

                    label = ((tiles & 394240) == 394240) ? 1 : 0;
                    ret |= label << 6;
                    break;
            }
            return ret;
        }
    }
}
