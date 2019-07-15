using Giant.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.App
{
    public partial class AppService
    {
        public void AppInitGate()
        {
        }

        public void AppInitManager()
        {
        }

        private void AppInitMap()
        {
            MapLibrary.Init();
            MapGridPosManager.Init();

            MapMananger = new MapMananger();
            MapMananger.Init();
        }

        private void AppInitSocial()
        {
        }

        private void AppInitAll()
        {
            AppInitGate();
            AppInitManager();
            AppInitMap();
            AppInitSocial();
        }

    }
}
