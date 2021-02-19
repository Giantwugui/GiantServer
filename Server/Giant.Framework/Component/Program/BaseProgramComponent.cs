using Giant.Core;
using Giant.Util;
using System;
using System.Threading;

namespace Giant.Framework
{
    public class BaseProgramComponent : InitSystem<string[]>
    {
        public override void Init(string[] args)
        {
            //公共基础服务
            ComponentFactory.CreateComponent<AppComponent, string[]>(args);

            //启动服务注册
            Scene.EventSystem.Handle(EventType.InitDone);

            DateTime dateTime = TimeHelper.Now;
            while (true)
            {
                OneThreadSynchronizationContext.Instance.Update();//异步回调处理

                double dt = (TimeHelper.Now - dateTime).TotalSeconds;
                dateTime = TimeHelper.Now;

                Scene.EventSystem.Update(dt);

                Thread.Sleep(1);
            }
        }
    }
}
