using Giant.Core;
using Giant.Log;
using Giant.Msg;
using Giant.Share;
using System;
using System.Reflection;

namespace Giant.Net
{
    public class MessageDispatcher
    {
        private readonly MultiMap<ushort, Type> opcodeTypes = new MultiMap<ushort, Type>();
        private readonly ListMap<ushort, IMHandler> Handlers = new ListMap<ushort, IMHandler>();

        public MessageDispatcher()
        {
            opcodeTypes.AddRange(InnerOpcode.Opcode2Types);
            opcodeTypes.AddRange(OuterOpcode.Opcode2Types);
        }

        public void Dispatch(Session session, ushort opcode, IMessage message)
        {
            if (Handlers.TryGetValue(opcode, out var handler))
            {
                handler.ForEach(x => x.Handle(session, message));
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
            if (!opcodeTypes.TryGetValue(opcode, out var type))
            {
                Logger.Error($"Error GetOpcodeMessageType  Have not this opcode {opcode} dispatch message type");
            }

            return type;
        }

        public void RegisterHandler(AppType appyType, Assembly assembly)
        {
            Type handlerType = typeof(MessageHandlerAttribute);
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (!(type.GetCustomAttribute(handlerType) is MessageHandlerAttribute attribute))
                {
                    continue;
                }
                if (Activator.CreateInstance(type) is IMHandler handler)
                {
                    this.RegisterHandler(handler);
                }
            }
        }

        public void RegisterHandler(object allServer, Assembly assembly)
        {
            throw new NotImplementedException();
        }

        private void RegisterHandler(IMHandler handler)
        {
            Type type = handler.GetMessageType();
            ushort opcode = GetOpcode(type);

            if (opcode == 0)
            {
                Logger.Warn($"Have no this handler's opcode, Handler {type.ToString()}, opcode {opcode}");
                return;
            }

            Handlers.Add(opcode, handler);
        }
    }
}
