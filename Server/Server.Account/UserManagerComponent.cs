using Giant.Core;
using Giant.Framework;
using Giant.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Account
{
    public class UserManagerComponent : InitSystem
    {
        private long timerId;
        private readonly int timeOut = 3;
        private readonly Dictionary<long, User> clients = new Dictionary<long, User>();

        public static UserManagerComponent Instance { get; private set; }

        public override void Init()
        {
            Instance = this;
            timerId = TimerComponent.Instance.AddRepeatTimer(10 * 1000, RemoveTimeOutClient).InstanceId;
        }

        public void Add(User client)
        {
            clients.Add(client.Session.Id, client);
        }

        public User GetClient(long sessionId)
        {
            clients.TryGetValue(sessionId, out var client);
            return client;
        }

        public void Remove(long sessionId)
        {
            clients.Remove(sessionId);
        }

        public int ClientCount()
        {
            return clients.Count;
        }

        public override void Dispose()
        {
            base.Dispose();
            TimerComponent.Instance.Remove(timerId);
        }

        private void RemoveTimeOutClient()
        {
            DateTime time = TimeHelper.Now;

            //过期没有走login的
            var wattingList = clients.Where(x => (time - x.Value.HeartBeatTime).TotalMinutes > timeOut).Select(x => x.Key).ToList();
            wattingList.ForEach(x => clients.Remove(x));
        }
    }
}
