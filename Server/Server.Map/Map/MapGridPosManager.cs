using EpPathFinding;
using Giant.Log;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server.Map
{
    public class MapGridPosManager
    {
        private static Dictionary<string, DynamicGrid> mapGridList = new Dictionary<string, DynamicGrid>();

        public static void Init()
        {
            mapGridList.Clear();
            string mapBinaryPath = $"{Environment.CurrentDirectory}/MapData";
            var files = Directory.GetFiles(mapBinaryPath, "*.byte", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                LoadMapData(file);
            }
        }

        public static DynamicGrid GetGrid(string name)
        {
            mapGridList.TryGetValue(name, out var grid);
            return grid;
        }

        private static void LoadMapData(string path)
        {
            if (!File.Exists(path))
            {
                Logger.Error($"map data have not exist please check : {path}");
                return;
            }

            DynamicGrid grid = new DynamicGrid();
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                int length = reader.ReadInt32();
                for (int i = 0; i < length; ++i)
                {
                    grid.SetWalkableAt(new GridPos(reader.ReadInt32(), reader.ReadInt32()), reader.ReadBoolean());
                }
            }

            string[] name = fileStream.Name.Split('\\');
            mapGridList.Add(name[name.Length - 1], grid);
            fileStream.Close();
        }

    }
}
