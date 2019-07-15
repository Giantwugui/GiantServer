using Giant.Frame;
using Giant.Log;
using Giant.Share;
using System;
using EpPathFinding;

namespace Server.App
{
    public partial class AppService : BaseService
    {
        public static AppService Instacne { get; } = new AppService();

        public MapMananger MapMananger { get; private set; }

        private AppService() { }

        public override void Init(AppOption option)
        {
            //框架的各种初始化工作
            base.Init(option);

            this.InitAppDiffence();
            this.InitDone();

            ConsoleReader.Instance.Start(DoCmd);
        }

        public override void Update(float dt)
        {
            try
            {
                base.Update(dt);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public override void InitData()
        {
            base.InitData();
        }

        public override void InitAppDiffence()
        {
            base.InitAppDiffence();

            switch (this.AppType)
            {
                case AppType.Gate:
                    this.AppInitGate();
                    break;
                case AppType.Manager:
                        this.AppInitManager();
                    break;
                case AppType.Map:
                        this.AppInitMap();
                    break;
                case AppType.Relation:
                        this.AppInitSocial();
                    break;
                case AppType.AllServer:
                        this.AppInitAll();
                    break;
            }
        }

        public override void InitDone()
        {
            base.InitDone();
            this.AppState = AppState.Started;
        }

        private void DoCmd(string message)
        {
            string[] param = message.Split(' ');
            switch (param[0])
            {
                case "Test":
                    Benchmark.Test();
                    break;
                case "PathFind":
                    {
                        //string[] SEPos = param[1].Split('|');
                        //string[] start = SEPos[0].Split(':');
                        //string[] end = SEPos[1].Split(':');
                        //GridPos pos1 = new GridPos(int.Parse(start[0]), int.Parse(start[1]));
                        //GridPos pos2 = new GridPos(int.Parse(end[0]), int.Parse(end[1]));
                        GridPos pos1 = new GridPos(21, 16);
                        GridPos pos2 = new GridPos(41, 16);
                        Map map = MapMananger.GetMap(1);
                        if (map != null)
                        {
                            var path = map.PathFind(pos1, pos2);
                        }
                    }
                    break;
                default:
                    Logger.Info($"system call -> {message}");
                    break;
            }
        }
    }
}
