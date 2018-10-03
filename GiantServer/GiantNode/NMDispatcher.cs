using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantNode
{
    class NMDispatcher
    {
        public void ReHandlerListener(string cmdName, Action<string, string>listener)
        {
            mHandlerListener.Add(cmdName, listener);
        }

        public void OnHandler(string cmdName, string param1, string param2)
        {
            if (mHandlerListener.ContainsKey(cmdName)) mHandlerListener[cmdName](param1, param2);
        }

        void InitEvent()
        {
            NM_Test.BiandEvent();
        }

        /// <summary>
        /// 单例对象
        /// </summary>
        public static NMDispatcher Single
        {
            get
            {
                if (mSingle == null)
                {
                    mSingle = new NMDispatcher();
                    mSingle.InitEvent();
                }
                return mSingle;
            }
        }

        static NMDispatcher mSingle;
        Dictionary<string, Action<string, string>> mHandlerListener = new Dictionary<string, Action<string, string>>();
    }
}
