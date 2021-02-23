using System;

namespace JumpPointSearch
{
    public class SearchNode : IComparable<SearchNode>
    {
        public int Id { get; private set; } // 显然 是paddedId
        // start->cur->end 路径cost
        public int F { get; private set; }

        // start->cur 路径cost
        public int G { get; private set; }
        public SearchNode Parent { get; private set; }

        // 调试用临时数据
        //public GridMap Map;
        //public int UnPaddedId;
        //public int PaddedX;
        //public int PaddedY;
        //public int UnPaddedX;
        //public int UnPaddedY;
        //public int GeoX;
        //public int GeoY;

        public SearchNode(int id) 
        {
            Id = id;
            F = Constants.INF;
            G = Constants.INF;
            Parent = null;
        }


        // 调试临时代码
        //public void InitDebugInfo(GridMap map)
        //{
        //    Map = map;
        //    map.ToPadded_X_Y(Id, out PaddedX, out PaddedY);
        //    map.ToUnpadded_X_Y(Id, out UnPaddedX, out UnPaddedY);
        //    map.PaddedIdToGeo(Id, out GeoX, out GeoY);
        //    UnPaddedId = map.ToPaddedId(UnPaddedX, UnPaddedY);
        //}

        //public override string ToString()
        //{
        //    string str = string.Format($"id {Id} unpadded {UnPaddedX} {UnPaddedY} padded {PaddedX} {PaddedY} geo {GeoX} {GeoY}");
        //    return str;
        //}

        public void SetParent(SearchNode parent)
        {
            Parent = parent;
        }

        public void SetG(int g)
        {
            G = g;
        }

        public void SetF(int f)
        {
            F = f;
        }

        public int CompareTo(SearchNode other)
        {
            // f值越小 越靠后 方便List Remove操作
            if (F > other.F)
            {
                return -1;
            }
            if (F < other.F)
            {
                return 1;
            }
            
            // F值相同 G值越小 越优先 即越靠后 （H值相对不准）
            if(G < other.G)
            {
                return 1;
            }
            return -1;
        }

    }
}
