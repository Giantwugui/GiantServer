using System.Collections.Generic;

namespace GiantNode
{
    public interface IPlugin
    {
        NodeEvents Events { get; }

        Dictionary<string, string> GetCommandSet();
    }
}
