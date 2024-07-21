using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Shared.Collections
{
    [Serializable]
    public class SerializableIDictionary<TDictionary, TKey, TValue> :
        ISerializationCallbackReceiver,
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>,
        // IDictionary,
        IDeserializationCallback,
        ISerializable
        where
        TDictionary :
        class,
        IDictionary<TKey, TValue>,
        IReadOnlyDictionary<TKey, TValue>,
        // IDictionary,
        IDeserializationCallback,
        ISerializable,
        new()
    {
        [SerializeField] protected List<SerializableKeyValuePair<TKey, TValue>> list;
        [SerializeField] protected string warningText;
        protected TDictionary Dictionary = new();

        protected virtual void GenerateWarningText()
        {
        }

        #region ISerializationCallbackReceiver

        public virtual void OnBeforeSerialize()
        {
            GenerateWarningText();
        }

        public virtual void OnAfterDeserialize()
        {
        }

        #endregion

        #region IDictionary<,> & IReadOnlyDictionary<,>

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((ICollection)Dictionary).Count;
        }

        public bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            // ReSharper disable once RedundantCast
            get => ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).IsReadOnly;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Dictionary.Add(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TKey key, TValue value)
        {
            Dictionary.Add(key, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(TKey key)
        {
            return Dictionary.Remove(key);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Dictionary.Remove(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            // ReSharper disable once RedundantCast
            ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Clear();
        }

        public TValue this[TKey key]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((IDictionary<TKey, TValue>)Dictionary)[key];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => ((IDictionary<TKey, TValue>)Dictionary)[key] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Dictionary.Contains(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(TKey key)
        {
            return ((IReadOnlyDictionary<TKey, TValue>)Dictionary).ContainsKey(key);
        }

        // bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key)
        // {
        //     return ((IReadOnlyDictionary<TKey, TValue>)Dictionary).ContainsKey(key);
        // }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, out TValue value)
        {
            return ((IDictionary<TKey, TValue>)Dictionary).TryGetValue(key, out value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            // ReSharper disable once RedundantCast
            // ReSharper disable once NotDisposedResourceIsReturned
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)Dictionary).GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            // ReSharper disable once NotDisposedResourceIsReturned
            return ((IEnumerable)Dictionary).GetEnumerator();
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((IDictionary<TKey, TValue>)Dictionary).Keys;
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((IDictionary<TKey, TValue>)Dictionary).Values;
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyDictionary<TKey, TValue>)Dictionary).Keys;
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyDictionary<TKey, TValue>)Dictionary).Values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Dictionary.CopyTo(array, arrayIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnDeserialization(object sender)
        {
            Dictionary.OnDeserialization(sender);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Dictionary.GetObjectData(info, context);
        }

        #endregion

        #region IDictionary

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public void Add(object key, object value)
        // {
        //     Dictionary.Add(key, value);
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public void Remove(object key)
        // {
        //     Dictionary.Remove(key);
        // }
        //
        // void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        // {
        //     ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Clear();
        // }
        //
        // public object this[object key]
        // {
        //     [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //     get => Dictionary[key];
        //
        //     [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //     set => Dictionary[key] = value;
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public bool Contains(object key)
        // {
        //     return Dictionary.Contains(key);
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        // {
        //     return ((IDictionary<TKey, TValue>)Dictionary).TryGetValue(key, out value);
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // IDictionaryEnumerator IDictionary.GetEnumerator()
        // {
        //     return ((IDictionary)Dictionary).GetEnumerator();
        // }
        //
        // ICollection IDictionary.Keys
        // {
        //     [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //     get => ((IDictionary)Dictionary).Keys;
        // }
        //
        // ICollection IDictionary.Values
        // {
        //     [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //     get => ((IDictionary)Dictionary).Values;
        // }
        //
        // public bool IsSynchronized
        // {
        //     [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //     get => Dictionary.IsSynchronized;
        // }
        //
        // public bool IsFixedSize
        // {
        //     [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //     get => Dictionary.IsFixedSize;
        // }
        //
        // public object SyncRoot
        // {
        //     [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //     get => Dictionary.SyncRoot;
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public void CopyTo(Array array, int index)
        // {
        //     Dictionary.CopyTo(array, index);
        // }

        #endregion
    }

    [Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        public TKey Key;

        public TValue Value;

        public static implicit operator SerializableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> item)
        {
            return new SerializableKeyValuePair<TKey, TValue> { Key = item.Key, Value = item.Value };
        }

        public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> item)
        {
            return new KeyValuePair<TKey, TValue>(item.Key, item.Value);
        }
    }

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : SerializableIDictionary<Dictionary<TKey, TValue>, TKey, TValue>
    {
        public override void OnAfterDeserialize()
        {
            Clear();

            foreach (var item in list)
            {
                Dictionary.TryAdd(item.Key, item.Value);
            }
        }

        protected override void GenerateWarningText()
        {
            if (list == null)
            {
                return;
            }
            
            Dictionary<TKey, List<int>> keyIndex = new();
            StringBuilder warningTextBuilder = new();


            for (var i = 0; i < list.Count; i++)
            {
                if (keyIndex.ContainsKey(list[i].Key))
                {
                    keyIndex[list[i].Key].Add(i);
                }
                else
                {
                    keyIndex.Add(list[i].Key, new() { i });
                }
            }

            var hasDuplicates = false;
            foreach (var item in keyIndex.Values.Where(item => item.Count > 1))
            {
                hasDuplicates = true;

                warningTextBuilder.Append("Duplicate keys at ");

                for (int i = 0; i < item.Count; i++)
                {
                    warningTextBuilder.Append($" {item[i]}").Append(i == item.Count - 1 ? ".\n" : ",");
                }
            }

            if (hasDuplicates)
            {
                warningTextBuilder.Append($"\nNo duplicate but the first will be deserialized to dictionary.");
            }

            warningText = warningTextBuilder.ToString();
        }
    }
}