using Server.Frame;
using Giant.Log;
using Giant.Share;
using System;
using EpPathFinding;
using System.Threading;
using Giant.Data;

namespace Server.Map
{
    public class AppService : BaseAppService
    {
        public static AppService Instacne { get; } = new AppService();

        private readonly MapMananger mapMananger = new MapMananger();
        public MapMananger MapMananger => mapMananger;

        public override void Start(string[] args)
        {
            this.Init(args);

            Logger.Warn($"server start complete------------- appType {Framework.AppType} appId {Framework.AppId}");

            this.DoUpdate();
        }

        public override void Init(string[] args)
        {
            //框架的各种初始化工作
            base.Init(args);

            mapMananger.Init();
            this.NetProxyManager.Init(this, new BaseServerCreater());

            this.InitDone();
            ConsoleReader.Instance.Start(DoCmd);
        }

        private void DoUpdate()
        {
            while (true)
            {
                Thread.Sleep(1);

                this.Update(1 * 0.01f);
            }
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
            MapLibrary.Init();
            MapGridPosManager.Init();
        }

        public override void InitDone()
        {
            base.InitDone();
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
