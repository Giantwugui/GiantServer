using Giant.Msg;

namespace Giant.Net
{
    public class OutterNetworkService : NetworkService
    {
        public OutterNetworkService(NetworkType network, string address) : base(network, address)
        {
            this.MessageParser = new ProtoPacker();
            this.MessageDispatcher = new OutterMessageDispatcher();
        }

    }
}
