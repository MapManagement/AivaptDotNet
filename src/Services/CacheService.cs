using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;



namespace AivaptDotNet.Services
{
    public class CacheService
    {
        #region Fields

        private int _cycle;
        public List<CacheKeyValue> Cache;
        private Task ClearTask;
        private CancellationTokenSource CTokenSource;

        #endregion

        #region Public Methods

        public void Initialize(int cycle)
        {
            _cycle = cycle;
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

        #endregion

        #region Private Methods

        private void ClearProcess()
        {
            while(!CTokenSource.Token.IsCancellationRequested)
            {
                Thread.Sleep(_cycle * 1000);
                var tempCache = new List<CacheKeyValue> (Cache);
                foreach(var keyValue in tempCache)
                {
                    if(DateTime.Now >= keyValue.DestroyAt)
                    {
                        keyValue.ClearAction();
                        Cache.Remove(keyValue); 
                    }
                }
                tempCache = null;
            }
            CTokenSource.Dispose();
        }

        #endregion
    }

    public class CacheKeyValue
    {
        #region Fields

        public string Key;
        public object Value;
        public DateTime DestroyAt;
        public Action ClearAction;

        #endregion

        #region Cosntructors
    
        public CacheKeyValue(string key, object value, DateTime destroyAt)
        {
            Key = key;
            Value = value;
            DestroyAt = destroyAt;
        }

        public CacheKeyValue(string key, object value, DateTime destroyAt, Action clearAction)
        {
            Key = key;
            Value = value;
            DestroyAt = destroyAt;
            ClearAction = clearAction;
        }

        #endregion
    }
}