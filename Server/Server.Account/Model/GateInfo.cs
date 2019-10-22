using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Account
{
    public class GateInfo
    {
        public int AppId { get; set; }
        public int SubId { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public int ClientCount { get; set; }

        public void Update(GateInfo info)
        {
            this.IP = info.IP;
            this.Port = info.Port;
            this.ClientCount = info.ClientCount;
        }
    }
}
