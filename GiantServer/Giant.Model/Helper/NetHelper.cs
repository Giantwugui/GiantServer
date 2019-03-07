using System.Net;

namespace Giant.Model
{
    public static class NetHelper
    {
        public static IPEndPoint ToIPEndPoint(string ip, int port)
        {
            return new IPEndPoint(IPAddress.Parse(ip), port);
        }
    }
}
