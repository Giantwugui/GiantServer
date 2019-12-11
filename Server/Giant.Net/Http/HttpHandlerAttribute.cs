using System;
using Giant.Core;

namespace Giant.Net
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HttpHandlerAttribute : ObjectAttribute
    {
        public string Path { get; private set; }

        public HttpHandlerAttribute(string path)
        {
            Path = path;
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
