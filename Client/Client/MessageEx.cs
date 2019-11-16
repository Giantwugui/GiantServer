using Giant.Msg;
using Giant.Net;

namespace Client
{
    public static class MessageEx
    {
        public static bool IsSuccess(this IResponse response)
        {
            return response.Error == ErrorCode.Success;
        }
    }
}
