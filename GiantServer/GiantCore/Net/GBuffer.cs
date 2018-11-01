using System;
using System.Collections.Generic;

namespace GiantCore
{
    public class GBuffer
    {
        public GBuffer()
        { 
        }

        public GBuffer(byte[] message)
        {
            mContent.AddRange(message);
        }

        public void Flush(int size)
        {
            if (size > 0)
            {
                byte[] tempBuffer = new byte[size];

                Array.Copy(TempBuffer, tempBuffer, size);

                mContent.AddRange(tempBuffer);
            }
        }

        public List<byte[]> PopList()
        {
            List<byte[]> tempList = new List<byte[]>();

            if (mContent.Count <= 0) return null;

            tempList.Add(mContent.ToArray());

            return tempList;
        }

        //public byte[] Pop()
        //{
        //    if (mContent.Count <= 0) return null;
        //}


        public List<byte> Content
        {
            get { return mContent; }
        }

        public int SendSize
        {
            get { return mContent.Count; }
        }

        public byte[] TempBuffer = new byte[4096];

        List<byte> mContent = new List<byte>();
    }
}
