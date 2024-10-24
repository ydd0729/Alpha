using System;
using System.Collections.Generic;

namespace Yd.Algorithm
{
    public static class Algorithm
    {
        // Knuth-Durstenfeld
        public static void Shuffle<T>(this IList<T> list)
        {
            Random random = new();

            for (var i = list.Count - 1; i >= 0; --i)
            {
                list.Swap(i, random.Next(0, i));
            }
        }

        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            (list[index1], list[index2]) = (list[index2], list[index1]);
        }
    }
}
