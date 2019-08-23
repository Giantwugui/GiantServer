using System;
using System.Collections.Generic;
using System.Text;
using Giant.Net;
using Giant.Share;

namespace Server.Gate
{
    public class ClientEntry
    {
        public string Account { get; private set; }
        public int Token { get; private set; }
        public DateTime EntryTime { get; private set; }

        public ClientEntry(string account, int token)
        {
            this.Token = token;
            this.Account = account;
            this.EntryTime = TimeHelper.Now;
        }
    }

    public class ClientManager
    {
        private Dictionary<string, ClientEntry> waittingList = new Dictionary<string, ClientEntry>();

        private Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public static ClientManager Instance { get; } = new ClientManager();

        public void WillEnter(ClientEntry client)
        {
            ClientEntry entry = GetClientEntry(client.Account);
            entry?.Session.Dispose();

            waittingList[client.Account] = client;
        }

        public ClientEntry GetClientEntry(string account)
        {
            waittingList.TryGetValue(account, out var entry);
            return entry;
        }

        public void Add(Client client)
        {
            clients.Add(client.Uid, client);
        }

        public void Get(Session session)
        {
        }

        public void Remove(int uid)
        {
            clients.Remove(uid);
        }
    }
}
