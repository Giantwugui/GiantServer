using System.Collections.Generic;

namespace Giant.Framework
{
    public interface IPlugin
    {
        NodeEvents Events { get; }

        Dictionary<string, string> GetCommandSet();
    }
}
