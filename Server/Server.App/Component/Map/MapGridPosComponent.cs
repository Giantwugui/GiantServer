using EpPathFinding;
using Giant.Core;
using Giant.Logger;
using System;
using System.Collections.Generic;
using System.IO;

namespace Server.App
{
    public class MapGridPosComponent : Component, IUpdateSystem, ILoadSystem
    {
        private Dictionary<string, DynamicGrid> mapGridList = new Dictionary<string, DynamicGrid>();

        public static MapGridPosComponent Instance { get; private set; }

        public void Init()
        {
            Instance = this;
            Load();
        }

        public void Load()
        {
            mapGridList.Clear();
            string mapBinaryPath = $"{Environment.CurrentDirectory}/MapData";
            var files = Directory.GetFiles(mapBinaryPath, "*.byte", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                LoadMapData(file);
            }
        }

        public DynamicGrid GetGrid(string name)
        {
            mapGridList.TryGetValue(name, out var grid);
            return grid;
        }

        private void LoadMapData(string path)
        {
            if (!File.Exists(path))
            {
                Log.Error($"map data have not exist please check : {path}");
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
