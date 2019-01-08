using ProtoBuf;

namespace Giant.Net
{
    /// <summary>
    /// 外部消息
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class OuterMessage
    {
        public OuterMessage()
        { }

        /// <summary>
        /// 发往节点id
        /// </summary>
        public uint ToNode { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public byte[] Content { get; set; }
    }
}
