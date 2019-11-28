namespace Server.App
{
    public class GateInfo
    {
        public int AppId { get; set; }
        public int SubId { get; set; }
        public string Address { get; set; }
        public int ClientCount { get; set; }

        public void Update(GateInfo info)
        {
            AppId = info.AppId;
            Address = info.Address;
            ClientCount = info.ClientCount;
        }
    }
}
