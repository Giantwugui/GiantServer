using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Share
{
    public class RANDOM
    {
        readonly SRandom m_render;
        public RANDOM(int seed)
        {
            m_render = new SRandom(seed);
        }


        public int Rand(int min, int max)
        {
            return min + m_render.Next(max) % (max - min + 1);
        }
    }

    public class SRandom
    {
        private readonly int[] seedArray = new int[56];
        private int inext;
        private int inextp;

        public SRandom(int Seed)
        {
            this.inext = 0;
            this.inextp = 21;
            int num1 = 161803398 - (Seed == int.MinValue ? int.MaxValue : System.Math.Abs(Seed));
            this.seedArray[55] = num1;
            int num2 = 1;

            for (int index1 = 1; index1 < 55; ++index1)
            {
                int index2 = 21 * index1 % 55;
                this.seedArray[index2] = num2;
                num2 = num1 - num2;
                if (num2 < 0)
                    num2 += int.MaxValue;
                num1 = this.seedArray[index2];
            }
            for (int index1 = 1; index1 < 5; ++index1)
            {
                for (int index2 = 1; index2 < 56; ++index2)
                {
                    this.seedArray[index2] -= this.seedArray[1 + (index2 + 30) % 55];
                    if (this.seedArray[index2] < 0)
                        this.seedArray[index2] += int.MaxValue;
                }
            }

        }

        public virtual int Next(int maxValue)
        {
            if (maxValue >= 0)
                return (int)(this.Sample() * (double)maxValue);
            else
                throw new ArgumentOutOfRangeException("maxValue < 0");
        }

        protected virtual double Sample()
        {
            return (double)this.InternalSample() * 4.6566128752458E-10;
        }

        private int InternalSample()
        {
            int num1 = this.inext;
            int num2 = this.inextp;
            int index1;
            if ((index1 = num1 + 1) >= 56)
                index1 = 1;
            int index2;
            if ((index2 = num2 + 1) >= 56)
                index2 = 1;
            int num3 = this.seedArray[index1] - this.seedArray[index2];
            if (num3 == int.MaxValue)
                --num3;
            if (num3 < 0)
                num3 += int.MaxValue;
            this.seedArray[index1] = num3;
            this.inext = index1;
            this.inextp = index2;
            return num3;
        }
    }
}
