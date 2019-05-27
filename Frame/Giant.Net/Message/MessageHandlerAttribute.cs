using Giant.Share;
using System;

namespace Giant.Net
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MessageHandlerAttribute : Attribute
    {
        public AppyType ServerType { get; private set; }

        public MessageHandlerAttribute(AppyType serverType)
        {
            this.ServerType = serverType;
        }
    }
}
