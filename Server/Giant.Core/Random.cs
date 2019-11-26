using System;

namespace Giant.Core
{
    public class RANDOM
    {
        readonly SRandom random;
        public RANDOM(int seed)
        {
            random = new SRandom(seed);
        }


        public int Rand(int min, int max)
        {
            return min + random.Next(max) % (max - min + 1);
        }
    }

    public class SRandom
    {
        private readonly int[] seedArray = new int[56];
        private int inext;
        private int inextp;

        public SRandom(int Seed)
        {
            inext = 0;
            inextp = 21;
            int num1 = 161803398 - (Seed == int.MinValue ? int.MaxValue : Math.Abs(Seed));
            seedArray[55] = num1;
            int num2 = 1;

            for (int index1 = 1; index1 < 55; ++index1)
            {
                int index2 = 21 * index1 % 55;
                seedArray[index2] = num2;
                num2 = num1 - num2;
                if (num2 < 0)
                    num2 += int.MaxValue;
                num1 = seedArray[index2];
            }
            for (int index1 = 1; index1 < 5; ++index1)
            {
                for (int index2 = 1; index2 < 56; ++index2)
                {
                    seedArray[index2] -= seedArray[1 + (index2 + 30) % 55];
                    if (seedArray[index2] < 0)
                        seedArray[index2] += int.MaxValue;
                }
            }

        }

        public virtual int Next(int maxValue)
        {
            if (maxValue >= 0)
                return (int)(Sample() * maxValue);
            else
                throw new ArgumentOutOfRangeException("maxValue < 0");
        }

        protected virtual double Sample()
        {
            return InternalSample() * 4.6566128752458E-10;
        }

        private int InternalSample()
        {
            int num1 = inext;
            int num2 = inextp;
            int index1;
            if ((index1 = num1 + 1) >= 56)
                index1 = 1;
            int index2;
            if ((index2 = num2 + 1) >= 56)
                index2 = 1;
            int num3 = seedArray[index1] - seedArray[index2];
            if (num3 == int.MaxValue)
                --num3;
            if (num3 < 0)
                num3 += int.MaxValue;
            seedArray[index1] = num3;
            inext = index1;
            inextp = index2;
            return num3;
        }
    }
}
