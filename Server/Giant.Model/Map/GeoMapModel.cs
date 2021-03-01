using EpPathFinding;
using Giant.Core;
using JumpPointSearch;
using System;
using System.Collections.Generic;
using System.IO;

namespace Giant.Model
{
    public class GeoMapModel
    {
        public readonly string FileName;
        public readonly int MinX_Small;
        public readonly int MaxX_Small;
        public readonly int MinY_Small;
        public readonly int MaxY_Small;

        public readonly int MinX_Big;
        public readonly int MaxX_Big;
        public readonly int MinY_Big;
        public readonly int MaxY_Big;

        public readonly DynamicGrid OldGridSmall;
        public readonly DynamicGrid OldGridBig;
        public readonly JumpPointParam OldJpsFinderSmall;
        public readonly JumpPointParam OldJpsFinderBig;

        public readonly JpsPathFinder NewJpsFinderSmall;
        public readonly JpsPathFinder NewJpsFinderBig;

        public GeoMapModel(string fileName)
        {
            FileName = fileName;

            string directory = $"{Environment.CurrentDirectory}/Grid";
            string filePath = Path.Combine(directory, fileName) + ".bytes";

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);

            //chk version
            double version = binaryReader.ReadDouble();

            if (version == 0.1)
            {
                List<GridPos> grids = new List<GridPos>();

                MinX_Small = int.MaxValue;
                MinY_Small = int.MaxValue;
                MaxX_Small = int.MinValue;
                MaxY_Small = int.MinValue;

                List<KeyValuePair<int, int>> walkableList = new List<KeyValuePair<int, int>>();

                int length = binaryReader.ReadInt32();

                for (int i = 0; i < length; i++)
                {
                    GridPos grid = new GridPos(binaryReader.ReadInt32(), binaryReader.ReadInt32());
                    grids.Add(grid);
                    if (MinX_Small > grid.x)
                    {
                        MinX_Small = grid.x;
                    }
                    if (MaxX_Small < grid.x)
                    {
                        MaxX_Small = grid.x;
                    }
                    if (MinY_Small > grid.y)
                    {
                        MinY_Small = grid.y;
                    }
                    if (MaxY_Small < grid.y)
                    {
                        MaxY_Small = grid.y;
                    }
                    walkableList.Add(new KeyValuePair<int, int>(grid.x, grid.y));
                }

                // 校正边界值
                MinX_Small--;
                MaxX_Small++;
                MinY_Small--;
                MaxY_Small++;

                OldGridSmall = new DynamicGrid(grids);
                OldJpsFinderSmall = new JumpPointParam(OldGridSmall, EndNodeUnWalkableTreatment.DISALLOW);

                //新版jps
                GridMap map = new GridMap(MinX_Small, MaxX_Small, MinY_Small, MaxY_Small);
                map.InitWalkables(walkableList);
                JpsExpansionPolicy expander = new JpsExpansionPolicy(map);
                NewJpsFinderSmall = new JpsPathFinder(expander);
            }

            binaryReader.Close();
            fileStream.Close();

            // 读取Big
            filePath = string.Format("{0}Big.bytes", Path.Combine(directory, fileName));

            fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            binaryReader = new BinaryReader(fileStream);

            //chk version
            version = binaryReader.ReadDouble();

            if (version == 0.1)
            {
                List<GridPos> grids = new List<GridPos>();
                List<KeyValuePair<int, int>> walkableList = new List<KeyValuePair<int, int>>();
                int length = binaryReader.ReadInt32();

                for (int i = 0; i < length; i++)
                {
                    GridPos grid = new GridPos(binaryReader.ReadInt32(), binaryReader.ReadInt32());
                    grids.Add(grid);
                    if (MinX_Big > grid.x)
                    {
                        MinX_Big = grid.x;
                    }
                    if (MaxX_Big < grid.x)
                    {
                        MaxX_Big = grid.x;
                    }
                    if (MinY_Big > grid.y)
                    {
                        MinY_Big = grid.y;
                    }
                    if (MaxY_Big < grid.y)
                    {
                        MaxY_Big = grid.y;
                    }

                    walkableList.Add(new KeyValuePair<int, int>(grid.x, grid.y));
                }

                // 校正边界值
                MinX_Big--;
                MaxX_Big++;
                MinY_Big--;
                MaxY_Big++;

                OldGridBig = new DynamicGrid(grids);
                OldJpsFinderBig = new JumpPointParam(OldGridBig, EndNodeUnWalkableTreatment.DISALLOW);

                // 新版jps
                GridMap map = new GridMap(MinX_Big, MaxX_Big, MinY_Big, MaxY_Big);
                map.InitWalkables(walkableList);

                JpsExpansionPolicy expander = new JpsExpansionPolicy(map);
                NewJpsFinderBig = new JpsPathFinder(expander);
            }
            binaryReader.Close();
            fileStream.Close();
        }

        public int Id => throw new NotImplementedException();

        public bool GetWalkbleAt(int x, int y)
        {
            GridMap map = NewJpsFinderBig.GetGridMap();
            return map.GetLabel(map.GeoToPaddedId(x, y));
        }
    }
}
