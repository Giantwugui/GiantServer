using Giant.Share;
using System;

namespace Giant.Net
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HttpHandlerAttribute : Attribute
    {
        public AppType AppType { get; private set; }
        public string Path { get; private set; }

        public HttpHandlerAttribute(AppType serverType, string path)
        {
            this.AppType = serverType;
            this.Path = path;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class GetAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PostAttribute : Attribute
    {
    }
}
