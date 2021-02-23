using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpPointSearch
{
    public class Constants
    {
        public const int DBWORD_BITS = 8; // 1 * 8 = 8
        public const int DBWORD_BITS_MASK = (DBWORD_BITS - 1); //7=0b111 快速对8取余 
        public const int LOG2_DBWORD_BITS = 3; //log2(warthog::DBWORD_BITS); 快速除以8

        // search and sort constants
        public const double DBL_ONE = 1.0f;
        public const double DBL_TWO = 2.0f;
        public const double DBL_ROOT_TWO = 1.4142f;
        public const double DBL_ONE_OVER_TWO = 0.5;
        public const double DBL_ONE_OVER_ROOT_TWO = 1.0 / DBL_ROOT_TWO;//0.707106781f;
        public const double DBL_ROOT_TWO_OVER_FOUR = DBL_ROOT_TWO * 0.25;
        public const int ONE = 100;
        public const int TWO = 200;
        public const int ROOT_TWO = (int)(DBL_ROOT_TWO * ONE);
        public const int ONE_OVER_TWO = (int)(DBL_ONE_OVER_TWO * ONE);
        public const int ONE_OVER_ROOT_TWO = (int)(DBL_ONE_OVER_ROOT_TWO * ONE);
        public const int ROOT_TWO_OVER_FOUR = (int)(DBL_ROOT_TWO * ONE);
        public const int INF = int.MaxValue;
    }

    // 8个方向，快速位操作
    public enum Direction
    {
        NONE = 0,
        NORTH = 1,
        SOUTH = 2,
        EAST = 4,
        WEST = 8,
        NORTHEAST = 16,
        NORTHWEST = 32,
        SOUTHEAST = 64,
        SOUTHWEST = 128
    }

    public enum Quadrant
    {
        FIRST = 1,
        SECOND = 2,
        THIRD = 3,
        FOURTH = 4
    }
}
