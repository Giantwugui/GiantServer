using Giant.Core;
using Giant.Logger;
using Giant.Msg;
using Giant.Util;
using System;
using System.Collections.Generic;

namespace Giant.Net
{
    public class MessageDispatcherComponent : InitSystem
    {
        private readonly ListMap<ushort, IMHandler> Handlers = new ListMap<ushort, IMHandler>();

        public override void Init()
        {
            Load();
        }

        public void Dispatch(Session session, ushort opcode, IMessage message)
        {
            if (Handlers.TryGetValue(opcode, out var handler))
            {
                handler.ForEach(x => x.Handle(session, message));
            }
            else
            {
                Log.Error($"Can not find the handler mathord opcode {opcode} message type {message.GetType()}");
            }
        }

        private void Load()
        {
            List<Type> handler = Scene.EventSystem.GetTypes(typeof(MessageHandlerAttribute));

            handler?.ForEach(type =>
            {
                if (Activator.CreateInstance(type) is IMHandler handler)
                {
                    RegisterHandler(handler);
                }
            });
        }

        private void RegisterHandler(IMHandler handler)
        {
            Type type = handler.GetMessageType();
            ushort opcode = OpcodeComponent.Instance.GetOpcode(type);

            if (opcode == 0)
            {
                Log.Warn($"Have no this handler's opcode, Handler {type.ToString()}, opcode {opcode}");
                return;
            }

            Handlers.Add(opcode, handler);
        }
    }
}
