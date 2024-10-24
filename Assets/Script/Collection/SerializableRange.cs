using System;
using UnityEngine;
using UnityEngine.Serialization;
using Yd.Extension;

namespace Yd.Collection
{
    [Serializable]
    public class SRange<T> where T : IComparable<T>
    {
        [FormerlySerializedAs("start")] [SerializeField] protected T min;

        [FormerlySerializedAs("end")] [SerializeField] protected T max;

        public SRange()
        {
            min = default;
            max = default;
        }

        public SRange(T min, T max)
        {
            this.min = min;
            this.max = max;
        }

        public T Min
        {
            get => min;
            set
            {
                min = value;
                if (min.CompareTo(max) > 0)
                {
                    min = max;
                }
            }
        }

        public T Max
        {
            get => max;
            set
            {
                max = value;
                if (max.CompareTo(min) < 0)
                {
                    max = min;
                }
            }
        }


        public static bool CheckRange(T min, T max)
        {
            return min.CompareTo(max) <= 0;
        }

        public void ClampRange(T minMin, T minMax, T maxMin, T maxMax)
        {
            #if UNITY_EDITOR
            if (!CheckRange(minMin, minMax))
            {
                Debug.LogWarning("minMin > minMax!");
                return;
            }
            if (!CheckRange(maxMin, maxMax))
            {
                Debug.LogWarning("maxMin > maxMax!");
                return;
            }
            #endif

            Min.Clamp(minMin, minMax);
            Max.Clamp(maxMin, maxMax);
        }

        public void ClampRange(T minMin, T maxMax)
        {
            #if UNITY_EDITOR
            if (!CheckRange(minMin, maxMax))
            {
                Debug.LogWarning("minMin > maxMax!");
                return;
            }
            #endif

            if (Min.CompareTo(minMin) < 0)
            {
                Min = minMin;
            }

            if (Max.CompareTo(maxMax) > 0)
            {
                Max = maxMax;
            }
        }

        public T Clamp(T v)
        {
            v = v.Clamp(Min, Max);
            return v;
        }

        public bool IsValid(T v)
        {
            if (Min.CompareTo(v) > 0)
            {
                return false;
            }

            if (Max.CompareTo(v) < 0)
            {
                return false;
            }

            return true;
        }
    }

    [Serializable]
    public class SRangeInt : SRange<int>
    {
        public SRangeInt()
        {
        }

        public SRangeInt(int min, int max) : base(min, max)
        {
        }

        public static implicit operator Vector2Int(SRangeInt range)
        {

            return new Vector2Int(range.min, range.max);
        }


        public int Random()
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
    }

    [Serializable]
    public class SRangeFloat : SRange<float>
    {

        public SRangeFloat(float min, float max) : base(min, max)
        {
        }

        public static implicit operator Vector2(SRangeFloat range)
        {
            return new Vector2(range.min, range.max);
        }

        public float Random()
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}