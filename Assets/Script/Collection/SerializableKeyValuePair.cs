using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yd.Collection
{
    [Serializable]
    public struct SKeyValuePair<TKey, TValue>
    {
        [FormerlySerializedAs("Key")] [SerializeField]
        private TKey key;

        [FormerlySerializedAs("Value")] [SerializeField]
        private TValue value;

        public SKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public TKey Key => key;
        public TValue Value => value;

        public static implicit operator SKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> item)
        {
            return new SKeyValuePair<TKey, TValue> { key = item.Key, value = item.Value };
        }

        public static implicit operator KeyValuePair<TKey, TValue>(SKeyValuePair<TKey, TValue> item)
        {
            return new KeyValuePair<TKey, TValue>(item.Key, item.value);
        }

        public void Deconstruct(out TKey outKey, out TValue outValue)
        {
            outKey = key;
            outValue = value;
        }

        public override string ToString()
        {
            return $"{key}, {value}";
        }
    }
}