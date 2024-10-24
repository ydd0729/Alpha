namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            return GetValueOrAdd(dictionary, key, new TValue());
        }

        public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            dictionary.Add(key, newValue);

            return dictionary[key];
        }
    }
}