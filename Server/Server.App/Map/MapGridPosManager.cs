using EpPathFinding;
using Giant.Log;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server.App
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
            mapGridList.Add(fileStream.Name, grid);
            fileStream.Close();
        }

    }
}
