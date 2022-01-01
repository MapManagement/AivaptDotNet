using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AivaptDotNet.Helpers
{
    public class MemoryCache
    {
        public readonly int Id;
        public readonly string Description;
        public readonly int Cycle;
        public List<CacheKeyValue> Cache;
        private Task ClearTask;
        private CancellationTokenSource CTokenSource;

        public MemoryCache(int id, string description, int cycle) //cycle in seconds
        {
            Id = id;
            Description = description;
            Cycle = cycle;
            Cache = new List<CacheKeyValue>();

            CTokenSource = new CancellationTokenSource();
            var cToken = CTokenSource.Token; 

            ClearTask = new Task(ClearProcess, cToken);
            ClearTask.Start();
        }

        public void AddKeyValue(CacheKeyValue keyValue)
        {
            Cache.Add(keyValue);
        }

        public void RemoveKeyValue(string key)
        {
            CacheKeyValue result = Cache.Find(x => x.Key == key);
            if(result != null)
            {
                Cache.Remove(result);
            }
        }

        public void UpdateKeyValue(string key, CacheKeyValue newValue)
        {
            CacheKeyValue result = Cache.Find(x => x.Key == key);
            if(result != null)
            {
                Cache.Remove(result);
                Cache.Add(newValue);
            }
        }

        public CacheKeyValue GetKeyValue(string key)
        {
            CacheKeyValue result = Cache.Find(x => x.Key == key); //compares strings
            return result;
        }

        public void StopClearProcess()
        {
            CTokenSource.Cancel();
        }

        private void ClearProcess()
        {
            while(!CTokenSource.Token.IsCancellationRequested)
            {
                Thread.Sleep(Cycle * 1000);
                foreach(var keyValue in Cache)
                {
                    if(DateTime.Now >= keyValue.DestroyAt)
                    {
                        //TODO: remove event
                        Cache.Remove(keyValue); 
                    }
                }
            }
            CTokenSource.Dispose();
        }
    }

    public class CacheKeyValue
    {
        public string Key;
        public object Value;
        public DateTime DestroyAt;
        public CacheKeyValue(string key, object value, DateTime destroyAt)
        {
            Key = key;
            Value = value;
            DestroyAt = destroyAt;
        }
    }
}