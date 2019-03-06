using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Model
{
    [ObjectSystem]
    public class OpcodeComponentSystem : AwakeSystem<OpcodeComponent>
    {
        public override void Awake(OpcodeComponent self)
        {
            self.Load();
        }
    }

    /// <summary>
    /// 所有的消息注册组件
    /// </summary>
    public class OpcodeComponent : Component
    {
        public void Load()
        {
        }
    }
}
