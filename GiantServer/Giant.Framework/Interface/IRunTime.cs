using System.Collections.Generic;

namespace Giant.Framework
{
    public interface IRunTime
    {
        uint GroupId { get; }

        uint NodeId { get; }

        string NodeName { get; }

        uint[] Nodes { get; }

        bool IsFrontNode { get; }

        Dictionary<string, string> Param { get; }

        string GetParam(string key);
    }
}
