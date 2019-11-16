using Giant.Share;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Account
{
    public class ClientManager
    {
        private readonly int timeOut = 3;
        private readonly Dictionary<long, Client> clients = new Dictionary<long, Client>();


        public static ClientManager Instance { get; } = new ClientManager();

        public void Add(Client client)
        {
            clients.Add(client.Session.Id, client);
        }

        public Client GetClient(long sessionId)
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

        public void Update()
        {
            RemoveTimeOutClient();
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
