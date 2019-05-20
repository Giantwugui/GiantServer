using System;

namespace Giant.Net
{
    public enum ServerType
    {
        Client = 0,
        Gate = 1,
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MessageHandlerAttribute : Attribute
    {
        public ServerType ServerType { get; private set; }

        public MessageHandlerAttribute(ServerType serverType)
        {
            this.ServerType = serverType;
        }
    }
}
