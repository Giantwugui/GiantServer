using Giant.Log;
using System;

namespace Giant.Net
{
    public class OutterNetworkService : NetworkService
    {
        public OutterNetworkService(NetworkType network, string address, Action<Session, bool> acceptCallback) : base(network, address)
        {
            base.OnConnect = acceptCallback;
        }
    }
}
