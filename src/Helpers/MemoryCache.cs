using System;
using System.Collections.Generic;

namespace AivaptDotNet.Helpers
{
    class MemoryCache
    {
        public readonly int Id;
        public readonly string Description;
        public List<CacheKeyValue> Cache;

        public MemoryCache(int id, string description)
        {
            Id = id;
            Description = description;
            //TODO: start cleaning process
        }

        public void AddKeyValue(CacheKeyValue keyValue)
        {
            Cache.Add(keyValue);
        }

        public void RemoveKeyValue(object key)
        {
            CacheKeyValue result = Cache.Find(x => x.Key == key);
            if(result != null)
            {
                Cache.Remove(result);
            }
        }

        public void UpdateKeyValue(object key, CacheKeyValue newValue)
        {
            CacheKeyValue result = Cache.Find(x => x.Key == key);
            if(result != null)
            {
                Cache.Remove(result);
                Cache.Add(newValue);
            }
        }

        public CacheKeyValue GetKeyValue(object key)
        {
            CacheKeyValue result = Cache.Find(x => x.Key == key);
            return result;
        }


    }

    class CacheKeyValue
    {
        public object Key;
        public object Value;
        public DateTime DestroyAt;
        public CacheKeyValue(object key, object value, DateTime destroyAt)
        {
            Key = key;
            Value = value;
            DestroyAt = destroyAt;
        }
    }
}