using Giant.Core;
using Giant.Logger;
using Giant.Msg;
using System;

namespace Giant.Net
{
    public class OpcodeComponent : InitSystem, ILoadSystem
    {
        private readonly MultiMap<ushort, Type> opcodeTypes = new MultiMap<ushort, Type>();

        public static OpcodeComponent Instance { get; private set; }

        public override void Init()
        {
            Instance = this;
            Load();
        }

        public void Load()
        {
            opcodeTypes.Clear();
            opcodeTypes.AddRange(InnerOpcode.Opcode2Types);
            opcodeTypes.AddRange(OuterOpcode.Opcode2Types);
        }

        public ushort GetOpcode(Type type)
        {
            if (!opcodeTypes.TryGetKey(type, out ushort opcode))
            {
                Log.Error($"Error GetOpcodeMessageType  Have not this type {type.Name} dispatch message type");
            }

            return opcode;
        }

        public Type GetMessageType(ushort opcode)
        {
            if (!opcodeTypes.TryGetValue(opcode, out var type))
            {
                Log.Error($"Error GetOpcodeMessageType  Have not this opcode {opcode} dispatch message type");
            }

            return type;
        }
    }
}
