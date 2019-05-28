using Giant.Log;
using Giant.Msg;
using Giant.Share;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Giant.Net
{
    public abstract class MessageDispatcher
    {
        protected readonly MultiMap<ushort, Type> opcodeTypes = new MultiMap<ushort, Type>();
        protected readonly Dictionary<ushort, IMHandler> Handlers = new Dictionary<ushort, IMHandler>();

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
            if (!opcodeTypes.TryGetValue(opcode, out var type))
            {
                Logger.Error($"Error GetOpcodeMessageType  Have not this opcode {opcode} dispatch message type");
            }

            return type;
        }

        public void RegisterHandler(AppyType appyType, Assembly assembly)
        {
            Type handlerType = typeof(MessageHandlerAttribute);
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (!(type.GetCustomAttribute(handlerType) is MessageHandlerAttribute attribute))
                {
                    continue;
                }

                //只注册该App需要处理的消息处理
                if (!attribute.AppType.IsSame(appyType))
                {
                    continue;
                }

                IMHandler handler = Activator.CreateInstance(type) as IMHandler;

                this.RegisterHandler(handler);
            }
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
