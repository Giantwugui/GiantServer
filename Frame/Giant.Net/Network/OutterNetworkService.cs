namespace Giant.Net
{
    public class OutterNetworkService : NetworkService
    {
        public OutterNetworkService(NetworkType network) : base(network)
        {
        }

        public OutterNetworkService(NetworkType network, string address) : base(network, address)
        {
        }

    }
}
