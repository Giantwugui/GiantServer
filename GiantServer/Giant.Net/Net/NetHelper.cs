using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Giant.Net
{
    public class NetHelper
    {
        public static IPEndPoint ToIPEndPoint(string ip, int port)
        {
            return new IPEndPoint(IPAddress.Parse(ip), port);
        }
    }
}
