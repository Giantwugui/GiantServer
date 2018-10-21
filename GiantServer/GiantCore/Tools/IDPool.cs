using System.Collections.Generic;

namespace GiantCore
{
    class IDPool
    {
        public static uint GetID()
        {
            lock (mIDPool)
            {
                uint id = 0;
                while (mIDPool.Contains(id++))
                {
                }
                return id;
            }
        }

        public static void FreeID(uint id)
        {
            lock (mIDPool)
            {
                if (mIDPool.Contains(id))
                {
                    mIDPool.Remove(id);
                }
            }
        }

        private static List<uint> mIDPool = new List<uint>();
    }
}
