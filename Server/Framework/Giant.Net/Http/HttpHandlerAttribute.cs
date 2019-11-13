using System;

namespace Giant.Net
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HttpHandlerAttribute : Attribute
    {
        public string Path { get; private set; }

        public HttpHandlerAttribute(string path)
        {
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
