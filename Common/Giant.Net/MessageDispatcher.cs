using Giant.Message;
using System;
using System.Collections.Generic;

namespace Giant.Net
{
    public class MessageDispatcher
    {
        public readonly Dictionary<ushort, Type> messageTypes = new Dictionary<ushort, Type>();//所有的opcode对应的messagetype
        public readonly Dictionary<ushort, Action<Session, IMessage>> Handlers = new Dictionary<ushort, Action<Session, IMessage>>();

        public void RegisterHandler(ushort opcode, Action<Session, IMessage> response)
        {
            Handlers.Add(opcode, response);

            Type msgType = response.Method.GetParameters()[1].ParameterType;

            messageTypes.Add(opcode, msgType);
        }

        public void Dispatch(Session session, ushort id, IMessage message)
        {
            if (Handlers.TryGetValue(id, out var action))
            {
                action(session, message);
            }
        }

        public ushort GetOpcode(Type type)
        {
            foreach (var kv in messageTypes)
            {
                if (kv.Value == type)
                {
                    return kv.Key;
                }
            }

            return 0;
        }

        public Type GetMessageType(ushort opcode)
        {
            if (messageTypes.TryGetValue(opcode, out var type))
            {
            }
            else
            {
                throw new Exception($"Error GetOpcodeMessageType  Have not this opcode {opcode} dispatch message type");
            }

            return type;
        }
    }
}
