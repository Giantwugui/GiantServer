using Giant.Battle;
using Giant.Core;
using Giant.Util;
using Giant.Logger;
using System;
using System.Collections.Generic;

namespace Server.Zone
{
    public class MapSceneManangerComponent : InitUpdateSystem
    {
        private DepthMap<int, int, MapScene> mapSceneList = new DepthMap<int, int, MapScene>();

        public override void Init()
        {
            InitMapScene();
        }

        private void InitMapScene()
        {
            //记载当前AppType需要挂载的地图
            string fileName = $"{Scene.AppConfig.AppType}_{Scene.AppConfig.AppId}_{Scene.AppConfig.SubId}";
            Dictionary<int, DataModel> dataList = DataComponent.Instance.GetDatas(fileName);

            if (dataList == null) return;

            foreach (var kv in dataList)
            {
                int mapId = kv.Value.Id;
                //支线信息
                List<int> channel = kv.Value.GetString("Channel").ToIntList();
                foreach (var curChannel in channel)
                {
                    MapScene mapScene = ComponentFactory.Create<MapScene, int, int>(mapId, curChannel);
                    AddMap(mapScene);
                }
            }
        }

        public void AddMap(MapScene scene)
        {
            mapSceneList.Add(scene.MapId, scene.Channel, scene);
        }

        public MapScene GetMap(int mapId, int channel)
        {
            mapSceneList.TryGetValue(mapId, channel, out MapScene map);
            return map;
        }

        public override void Update(double dt)
        {
            foreach (var kv in mapSceneList)
            {
                foreach (var map in kv.Value)
                {
                    try
                    {
                        map.Value.Update(dt);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"update map Id {map.Value.MapModel.Id} type {map.Value.MapModel.MapType} error {ex}");
                    }
                }
            }
        }
    }
}
