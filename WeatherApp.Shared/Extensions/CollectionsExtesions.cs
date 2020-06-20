using System.Linq;
using System.Text;

namespace System.Collections.Generic.Extensions
{
    public static class CollectionsExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                dictionary.Remove(key);

            dictionary.Add(key, value);
        }
    }
}