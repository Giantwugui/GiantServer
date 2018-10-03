using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantNode
{
    class NM_Test
    {
        public static void BiandEvent()
        {
            NMDispatcher.Single.ReHandlerListener("Debug", OnTest);
        }

        public static void OnTest(string param1, string param2)
        {
        }

    }
}
