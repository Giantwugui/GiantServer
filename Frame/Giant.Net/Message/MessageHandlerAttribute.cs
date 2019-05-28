using Giant.Share;
using System;

namespace Giant.Net
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MessageHandlerAttribute : Attribute
    {
        public AppType AppType { get; private set; }

        public MessageHandlerAttribute(AppType serverType)
        {
            this.AppType = serverType;
        }
    }
}
