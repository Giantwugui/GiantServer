using Giant.Share;
using System;

namespace Giant.Net
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MessageHandlerAttribute : Attribute
    {
    }
}
