using Giant.Core;
using Giant.EnumUtil;
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

    public class MsgDispatchComponent : InitSystem<MapScene>
    {
        private Dictionary<TriggerMessageType, MsgSubscriber> subscribers = new Dictionary<TriggerMessageType, MsgSubscriber>();

        public MapScene Map { get; private set; }

        public override void Init(MapScene map)
        {
            Map = map;
        }

        public void SubscribeMessage(TriggerMessageType type, Action<object> action)
        {
            MsgSubscriber subscriber = GetSubscriber(type);
            if (subscriber == null)
            {
                subscriber = ComponentFactory.Create<MsgSubscriber, TriggerMessageType>(type);
                subscribers[type] = subscriber;
            }
            subscriber.Subscribes += action;
        }

        public void DisSubscribeMessage(TriggerMessageType type, Action<object> action)
        {
            MsgSubscriber subscriber = GetSubscriber(type);
            if (subscriber != null)
            {
                subscriber.Subscribes -= action;
            }
        }

        public void DispatchMessage(TriggerMessageType type, object param = null)
        { 
            GetSubscriber(type)?.Dispatch(param);
        }

        private MsgSubscriber GetSubscriber(TriggerMessageType type)
        {
            subscribers.TryGetValue(type, out var action);
            return action;
        }
    }
}
