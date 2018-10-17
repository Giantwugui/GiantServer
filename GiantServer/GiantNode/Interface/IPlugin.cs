using GiantCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantNode
{
    public interface IPlugin
    {
        NodeEvents Events { get; }

        Dictionary<string, string> GetCommandSet();
    }
}
