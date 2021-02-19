using Giant.Core;
using Giant.Framework;
using Giant.Net;
using Giant.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Gate
{
    public class ClientEnter
    {
        public string Account { get; private set; }
        public int Token { get; private set; }
        public DateTime EntryTime { get; private set; }

        public ClientEnter(string account, int token)
        {
            Token = token;
            Account = account;
            EntryTime = TimeHelper.Now;
        }
    }

    class ClientManagerComponent : InitSystem
    {
        private long timerId;
        private readonly int timeOut = 5;
        private readonly Dictionary<long, Client> waittingClient = new Dictionary<long, Client>();
        private readonly Dictionary<string, ClientEnter> enterList = new Dictionary<string, ClientEnter>();

        private readonly Dictionary<int, Client> clients = new Dictionary<int, Client>();
        private readonly Dictionary<long, Client> clientsIndexBySessionId = new Dictionary<long, Client>();

        public static ClientManagerComponent Instance { get; private set; }

        public override void Init()
        {
            Instance = this;
            timerId = TimerComponent.Instance.AddRepeatTimer(10 * 1000, RemoveTimeOutClient).InstanceId;
        }

        public void WillEnter(ClientEnter client)
        {
            enterList[client.Account] = client;
        }

        public ClientEnter GetClientEntry(string account)
        {
            enterList.TryGetValue(account, out var entry);
            return entry;
        }

        public void RemoveClientEntry(string account)
        {
            enterList.Remove(account);
        }

        public void Add(Client client)
        {
            clients[client.Uid] = client;
            clientsIndexBySessionId[client.Session.Id] = client;
        }

        public void Add2Watting(Client client)
        {
            waittingClient[client.Session.Id] = client;
        }

        public Client GetClient(int uid)
        {
            clients.TryGetValue(uid, out Client client);
            return client;
        }

        public Client GetClient(long sessionId)
        {
            clientsIndexBySessionId.TryGetValue(sessionId, out var client);
            return client;
        }

        public void Remove(long sessionId)
        {
            if (clientsIndexBySessionId.TryGetValue(sessionId, out Client client))
            {
                clients.Remove(client.Uid);
                clientsIndexBySessionId.Remove(sessionId);
            }
        }

        public int ClientCount()
        {
            return clients.Count + enterList.Count;
        }

        public void ClientOffline(Session session)
        {
            Client client = GetClient(session.Id);
            client?.Offline();
        }

        public override void Dispose()
        {
            base.Dispose();
            TimerComponent.Instance.Remove(timerId);
        }

        private void RemoveTimeOutClient()
        {
            DateTime time = TimeHelper.Now;

            // 过期没有连接到gate的
            var enterListTimeOut = enterList.Where(x => (time - x.Value.EntryTime).TotalMinutes > timeOut).Select(x => x.Key).ToList();
            enterListTimeOut.ForEach(x => enterList.Remove(x));

            //过期没有走login的
            var wattingList = waittingClient.Where(x => (time - x.Value.HeartBeatTime).TotalMinutes > timeOut).Select(x => x.Key).ToList();
            wattingList.ForEach(x => waittingClient.Remove(x));
        }
    }
}
