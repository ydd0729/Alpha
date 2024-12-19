using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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

    public static class RandomE
    {
        // private static readonly Random rand = new();

        /// <summary>
        ///     get a random point in a unit circle
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Vector2 RandomInCircle(float radius)
        {
            var xi1 = UnityEngine.Random.Range(0f, 1f);
            var xi2 = UnityEngine.Random.Range(0f, 1f);

            var phi = 2 * Mathf.PI * xi1;
            var r = radius * Mathf.Pow(xi2, 0.5f);

            var x = r * Mathf.Cos(phi);
            var y = r * Mathf.Sin(phi);

            return new Vector2(x, y);
        }

        public static Vector2 RandomInRing(float innerRadius, float radius)
        {
            var xi1 = UnityEngine.Random.Range(innerRadius / radius, 1f);
            var xi2 = UnityEngine.Random.Range(innerRadius / radius, 1f);

            var phi = 2 * Mathf.PI * xi1;
            var r = radius * Mathf.Pow(xi2, 0.5f);

            var x = r * Mathf.Cos(phi);
            var y = r * Mathf.Sin(phi);

            return new Vector2(x, y);
        }
    }
}