using System;

namespace Giant.Utils.Test
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ActivedTestAttribute : Attribute
    {
        public ActivedTestAttribute(string identity)
        {
            this.Identity = identity;
        }
        public string Identity { get; private set; }
    }
}
