using System;
using UnityEngine;
using UnityEngine.Serialization;
using Yd.Extension;

namespace Yd.Collection
{
    [Serializable]
    public class SRange<T> where T : IComparable<T>
    {
        [FormerlySerializedAs("minInclusiveInclusive")] [FormerlySerializedAs("min")] [FormerlySerializedAs("start")]
        [SerializeField]
        protected T minInclusive;

        [FormerlySerializedAs("maxInclusiveInclusive")] [FormerlySerializedAs("max")] [FormerlySerializedAs("end")]
        [SerializeField]
        protected T maxInclusive;

        public SRange()
        {
            minInclusive = default;
            maxInclusive = default;
        }

        public SRange(T minInclusive, T maxInclusive)
        {
            Set(minInclusive, maxInclusive);
        }

        public T MinInclusive
        {
            get => minInclusive;
            set
            {
                minInclusive = value;
                if (minInclusive.CompareTo(maxInclusive) > 0)
                {
                    maxInclusive = minInclusive;
                }
            }
        }

        public T MaxInclusive
        {
            get => maxInclusive;
            set
            {
                maxInclusive = value;
                if (maxInclusive.CompareTo(minInclusive) < 0)
                {
                    minInclusive = maxInclusive;
                }
            }
        }

        public T Start
        {
            get => MinInclusive;
            set => MinInclusive = value;
        }


        public T End
        {
            get => MaxInclusive;
            set => MaxInclusive = value;
        }

        public void Set(T minInclusive, T maxInclusive)
        {
            MinInclusive = minInclusive;
            MaxInclusive = maxInclusive;
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

            MinInclusive.Clamp(minMin, minMax);
            MaxInclusive.Clamp(maxMin, maxMax);
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

            if (MinInclusive.CompareTo(minMin) < 0)
            {
                MinInclusive = minMin;
            }

            if (MaxInclusive.CompareTo(maxMax) > 0)
            {
                MaxInclusive = maxMax;
            }
        }

        public T Clamp(T v)
        {
            v = v.Clamp(MinInclusive, MaxInclusive);
            return v;
        }

        public bool IsInRange(T v)
        {
            if (MinInclusive.CompareTo(v) > 0)
            {
                return false;
            }

            if (MaxInclusive.CompareTo(v) < 0)
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

        public SRangeInt(int minInclusive, int maxInclusive) : base(minInclusive, maxInclusive)
        {
        }

        public static implicit operator Vector2Int(SRangeInt range)
        {
            return new Vector2Int(range.minInclusive, range.maxInclusive);
        }


        public int Random()
        {
            return UnityEngine.Random.Range(minInclusive, maxInclusive + 1);
        }
    }

    [Serializable]
    public class SRangeFloat : SRange<float>
    {
        public SRangeFloat()
        {
        }

        public SRangeFloat(float minInclusive, float maxInclusive) : base(minInclusive, maxInclusive)
        {
        }

        public static implicit operator Vector2(SRangeFloat range)
        {
            return new Vector2(range.minInclusive, range.maxInclusive);
        }

        public float Random()
        {
            return UnityEngine.Random.Range(minInclusive, maxInclusive);
        }

        public float Length()
        {
            return MaxInclusive - MinInclusive;
        }
    }
}