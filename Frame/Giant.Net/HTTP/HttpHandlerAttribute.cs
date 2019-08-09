using Giant.Share;
using System;

namespace Giant.Net
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HttpHandlerAttribute : Attribute
    {
        public AppType AppType { get; private set; }

        public HttpHandlerAttribute(AppType serverType)
        {
            this.AppType = serverType;
        }
    }

    public abstract class IHttpAttribute : Attribute
    {
        public string Name { get; protected set; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class GetAttribute : IHttpAttribute
    {
        public GetAttribute(string name)
        {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class PostAttribute : IHttpAttribute
    {
        public PostAttribute(string name)
        {
            this.Name = name;
        }
    }
}
