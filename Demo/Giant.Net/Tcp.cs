using System;
using System.Collections.Generic;
using System.Net;

namespace Giant.Net
{
    public class Tcp
    {
        public string IP { get; set; }

        public int Port { get; set; }

        public Action OnAccept;
        public Action<MemorySttream> OnReceived;
        public Action OnAccept;

        public Tcp()
        {
        }

        public Tcp(string ip, int port)
        {
            IP = ip;
            Port = port;
        }
    }
}
