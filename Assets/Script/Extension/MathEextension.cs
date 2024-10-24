using System;

namespace Yd.Extension
{
    public static class MathE
    {
        public static T Clamp<T>(this T v, T min, T max) where T : IComparable<T>
        {
            if (v.CompareTo(min) < 0)
            {
                v = min;
            }

            if (v.CompareTo(max) > 0)
            {
                v = max;
            }

            return v;
        }
    }
}