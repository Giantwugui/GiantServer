using Giant.Share;
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
            this.Token = token;
            this.Account = account;
            this.EntryTime = TimeHelper.Now;
        }
    }

    public class ClientManager
    {
        private readonly int timeOut = 3;
        private readonly Dictionary<long, Client> waittingClient = new Dictionary<long, Client>();
        private readonly Dictionary<string, ClientEnter> enterList = new Dictionary<string, ClientEnter>();

        private readonly Dictionary<int, Client> clients = new Dictionary<int, Client>();
        private readonly Dictionary<long, Client> clientsIndexBySessionId = new Dictionary<long, Client>();


        public static ClientManager Instance { get; } = new ClientManager();

        public void WillEnter(ClientEnter client)
        {
            enterList[client.Account] = client;
        }

        public ClientEnter GetClientEntry(string account)
        {
            enterList.TryGetValue(account, out var entry);
            return entry;
        }

        public void Add(Client client)
        {
            clients.Add(client.Uid, client);
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

        public void Update()
        {
            RemoveTimeOutClient();
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
