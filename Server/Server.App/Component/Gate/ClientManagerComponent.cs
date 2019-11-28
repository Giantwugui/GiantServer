using Giant.Core;
using Giant.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.App
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

    class ClientManagerComponent : Component, IUpdateSystem, IInitSystem
    {
        private readonly int timeOut = 3;
        private readonly Dictionary<long, Client> waittingClient = new Dictionary<long, Client>();
        private readonly Dictionary<string, ClientEnter> enterList = new Dictionary<string, ClientEnter>();

        private readonly Dictionary<int, Client> clients = new Dictionary<int, Client>();
        private readonly Dictionary<long, Client> clientsIndexBySessionId = new Dictionary<long, Client>();

        public static ClientManagerComponent Instance { get; private set; }

        public void Init()
        {
            Instance = this;
        }

        public void Update(double dt)
        {
            RemoveTimeOutClient();
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
            clients.Add(client.Uid, client);
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
