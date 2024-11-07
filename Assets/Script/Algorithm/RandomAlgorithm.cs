using System;
using System.Collections.Generic;

namespace Yd.Algorithm
{
    public class RandomInt
    {
        private readonly int minDuplicateGap;
        private readonly Random random = new();
        private readonly List<int> range = new();
        private int swapIndex;
        private int usedCount;

        public RandomInt(int min, int max, int minDuplicateGap)
        {
            for (var i = min; i <= max; i++)
            {
                range.Add(i);
            }

            if (minDuplicateGap > range.Count)
            {
                throw new ArgumentException("argument out of range.");
            }

            usedCount = 0;
            this.minDuplicateGap = minDuplicateGap;
        }

        public int Next()
        {
            var index = random.Next(0, range.Count - usedCount);
            var e = range[index];

            if (usedCount < minDuplicateGap)
            {
                usedCount += 1;
            }

            if (usedCount != 0)
            {
                swapIndex = (swapIndex + 1) % usedCount;
                range.Swap(index, range.Count - 1 - swapIndex);
            }

            return e;
        }
    }
}