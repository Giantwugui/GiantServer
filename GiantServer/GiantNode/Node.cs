using GiantCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiantNode
{
    public class Node
    {
        public void Return(Session session, string message)
        {
        }


        public static Node Single
        {
            get
            {
                if (mSingle == null)
                {
                    mSingle = new Node();
                }
                return mSingle;
            }
        }

        private static Node mSingle = null;
    }
}
