using Giant.Msg;
using Giant.Net;

namespace Robot
{
    public static class MessageEx
    {
        public static bool IsSuccess(this IResponse response)
        {
            return response.Error == ErrorCode.Success;
        }
    }
}
