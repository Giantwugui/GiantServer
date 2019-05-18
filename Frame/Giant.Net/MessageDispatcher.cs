using Giant.Msg;
using System;
using System.Collections.Generic;

namespace Giant.Net
{
    public class MessageDispatcher
    {
        public readonly MultiMap<ushort, Type> opcodeTypes = new MultiMap<ushort, Type>();
        public readonly Dictionary<ushort, IMHandler> Handlers = new Dictionary<ushort, IMHandler>();

        public MessageDispatcher()
        {
            opcodeTypes.AddRange(InnerOpcode.Opcode2Types);
            opcodeTypes.AddRange(OuterOpcode.Opcode2Types);
        }

        public void RegisterHandler(ushort opcode, IMHandler handler)
        {
            Handlers.Add(opcode, handler);
        }

        public void RegisterHandler(IMHandler handler)
        {
            Handlers.Add(GetOpcode(handler.GetMessageType()), handler);
        }

        public void Dispatch(Session session, ushort id, IMessage message)
        {
            if (Handlers.TryGetValue(id, out IMHandler handler))
            {
                handler.Handle(session, message);
            }
        }

        public ushort GetOpcode(Type type)
        {
            if (!opcodeTypes.TryGetKey(type, out ushort opcode))
            {
                throw new Exception($"Error GetOpcodeMessageType  Have not this type {type.Name} dispatch message type");
            }

            return opcode;
        }

        public Type GetMessageType(ushort opcode)
        {
            if(!opcodeTypes.TryGetValue(opcode, out var type))
            {
                throw new Exception($"Error GetOpcodeMessageType  Have not this opcode {opcode} dispatch message type");
            }

            return type;
        }
    }
}
