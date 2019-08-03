using Giant.Data;

namespace Server.App
{
    public partial class AppService
    {
        public void AppInitGlobal()
        {
        }

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

        private void AppInitRelation()
        {
        }

        private void AppInitAll()
        {
            AppInitMap();
        }

    }
}
