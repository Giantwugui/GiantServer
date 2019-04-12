namespace Giant.Net
{
    public interface IMessage
    {
        ushort Id { get; set; }

        byte[] MsgContent { get; set; }
    }

}
