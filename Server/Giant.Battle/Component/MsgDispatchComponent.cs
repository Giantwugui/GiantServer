using Giant.Core;
using System;
using System.Collections.Generic;

namespace Giant.Battle
{
    public class MsgSubscriber : Entity, IInitSystem
    {
        public event Action<object> Subscribes;

        public void Dispatch(object param)
        {
            Subscribes?.Invoke(param);
        }
    }

    public class MsgDispatchComponent : Component
    {
        private Dictionary<MessageType, MsgSubscriber> subscribers = new Dictionary<MessageType, MsgSubscriber>();

        public void SubscribeMessage(MessageType type, Action<object> action)
        {
            MsgSubscriber subscriber = GetSubscriber(type);
            if (subscriber == null)
            {
                subscriber = ComponentFactory.CreateComponent<MsgSubscriber, MessageType>(type);
                subscribers[type] = subscriber;
            }
            subscriber.Subscribes += action;
        }

        public void DisSubscribeMessage(MessageType type, Action<object> action)
        {
            MsgSubscriber subscriber = GetSubscriber(type);
            if (subscriber != null)
            {
                subscriber.Subscribes -= action;
            }
        }

        public void DispatchMessage(MessageType type, object param)
        { 
            GetSubscriber(type)?.Dispatch(param);
        }

        private MsgSubscriber GetSubscriber(MessageType type)
        {
            subscribers.TryGetValue(type, out var action);
            return action;
        }
    }
}
