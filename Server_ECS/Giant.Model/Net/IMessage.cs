namespace Giant.Model
{
    public interface IMessage
    {
    }

    public interface IRequest : IMessage
    {
        int RpcId { get; set; }
    }


    public interface IResponse : IMessage
    {
        int RpcId { get; set; }

        int Error { get; set; }

        string Message { get; set; }
    }

    public class ResponseMessage : IResponse
    {
        public int RpcId { get; set; }

        public int Error { get; set; }

        public string Message { get; set; }
    }
}
