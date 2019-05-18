using Giant.Msg;
using Giant.Log;
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

        public void RegisterHandler(IMHandler handler)
        {
            Handlers.Add(GetOpcode(handler.GetMessageType()), handler);
        }

        public void Dispatch(Session session, ushort opcode, IMessage message)
        {
            if (Handlers.TryGetValue(opcode, out IMHandler handler))
            {
                handler.Handle(session, message);
            }
            else
            {
                Logger.Error($"Can not find the handler mathord opcode {opcode} message type {message.GetType()}");
            }
        }

        public ushort GetOpcode(Type type)
        {
            if (!opcodeTypes.TryGetKey(type, out ushort opcode))
            {
                Logger.Error($"Error GetOpcodeMessageType  Have not this type {type.Name} dispatch message type");
            }

            return opcode;
        }

        public Type GetMessageType(ushort opcode)
        {
            if(!opcodeTypes.TryGetValue(opcode, out var type))
            {
                Logger.Error($"Error GetOpcodeMessageType  Have not this opcode {opcode} dispatch message type");
            }

            return type;
        }
    }
}
