using System;

namespace Giant.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MessageHandlerAttribute : ObjectAttribute
    {
    }
}
