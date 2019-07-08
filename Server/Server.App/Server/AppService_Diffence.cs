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
            MapGridPosManager.Init();

            MapMamanger = new MapMamanger();
            MapMamanger.Init();
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
