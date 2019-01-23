using System;
using System.Net;

namespace Giant.Net
{
    public enum NetServiceType
    {
        TCP,
        UDP,
        HTTP
    }

    class NetService
    {
        public void Start(NetServiceType netServiceType, string address, int port)
        {
            try
            {
                IPEndPoint ipEndPoint = null;
                switch (netServiceType)
                {
                    case NetServiceType.TCP:
                        ipEndPoint = NetHelper.ToIPEndPoint(address, port);
                        mService = new TCPService(ipEndPoint, OnAccept);
                        break;
                    case NetServiceType.UDP:
                        ipEndPoint = NetHelper.ToIPEndPoint(address, port);
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("NetService StartError", ex);
            }
        }



        private void OnAccept(BChannel channel)
        {
        }


        private BaseService mService;
    }
}
