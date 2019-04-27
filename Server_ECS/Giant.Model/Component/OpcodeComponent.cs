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

    [ObjectSystem]
    public class OpcodeComponentLoadSysten : LoadSystem<OpcodeComponent>
    {
        public override void Load(OpcodeComponent self)
        {
            self.Load();
        }
    }

    /// <summary>
    /// 所有的消息注册组件
    /// </summary>
    public class OpcodeComponent : Component
    {
        //所有opcode对应的component
        Dictionary<ushort, Type> components = new Dictionary<ushort, Type>();

        public void Load()
        {
            //从eventsystem 中加载

            List<Type> types = Game.EventSystem.GetTypes(typeof(MessageAttribute));

            foreach (Type type in types)
            {
                object[] obj = type.GetCustomAttributes(typeof(MessageAttribute), false);

                MessageAttribute attribute = obj[0] as MessageAttribute;

                components[attribute.Opcode] = type;
            }
        }

        public Type GetType(ushort opcode)
        {
            if (components.ContainsKey(opcode))
            {
                return components[opcode];
            }

            return null;
        }
    }
}
