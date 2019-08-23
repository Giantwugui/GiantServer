using System;
using System.Collections.Generic;
using System.Text;
using Giant.Net;
using Giant.Share;

namespace Server.Gate
{
    public class ClientManager
    {
        private Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public static ClientManager Instance { get; } = new ClientManager();

        public void Add(Client client)
        {
            clients.Add(client.Uid, client);
        }

        public void Remove(int uid)
        {
            clients.Remove(uid);
        }
    }
}
