using System;
using System.Collections.Generic;
using System.Threading;

namespace StockManager.Core
{
    /// <summary>
    /// Server context lives for the entirety of the application.
    /// </summary>
    public class ServerContext : IDisposable
    {
        private static readonly Lazy<ServerContext> _instance = new Lazy<ServerContext>(() => new ServerContext());
        private SemaphoreSlim _lockThread = new SemaphoreSlim(1, 1);
        private bool _isDisposed;

        /// <summary>
        /// Gets an instance of the server context
        /// </summary>
        public static ServerContext instance => _instance.Value;

        private IDictionary<string, object> _contextData = new Dictionary<string, object>();

        /// <summary>
        /// Get context data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Unique key to identify the data</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return instance.GetData<T>(key);
        }

        /// <summary>
        /// Set context data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Unique key to identify the data</param>
        /// <param name="data"></param>
        public static void Set<T>(string key, T data)
        {
            instance.SetData(key, data);
        }

        /// <summary>
        /// Remove context data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Unique key to identify the data</param>
        public static void Remove<T>(string key)
        {
            instance.RemoveData<T>(key);
        }

        /// <summary>
        /// Get context data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Unique key to identify the data</param>
        /// <returns></returns>
        public T GetData<T>(string key)
        {
            _lockThread.Wait();
            try
            {
                string wrappedKey = WrapKey<T>(key);
                if (_contextData.ContainsKey(wrappedKey))
                {
                    return (T)_contextData[wrappedKey];
                }

                return default(T);
            }
            finally
            {
                _lockThread.Release();
            }
        }

        /// <summary>
        /// Set context data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Unique key to identify the data</param>
        /// <param name="data"></param>
        public void SetData<T>(string key, T data)
        {
            _lockThread.Wait();
            try
            {
                string wrappedKey = WrapKey<T>(key);
                if (_contextData.ContainsKey(wrappedKey))
                {
                    _contextData[wrappedKey] = data;
                }
                else
                {
                    _contextData.Add(wrappedKey, data);
                }
            }
            finally
            {
                _lockThread.Release();
            }
        }

        /// <summary>
        /// Removes a context data by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Unique key to identify the data</param>
        public void RemoveData<T>(string key)
        {
            _lockThread.Wait();
            try
            {
                string wrappedKey = WrapKey<T>(key);
                if(_contextData.ContainsKey(wrappedKey))
                {
                    _contextData.Remove(wrappedKey);
                }
            }
            finally
            {
                _lockThread.Release();
            }
        }


        private string WrapKey<T>(string key)
        {
            return $"{key}:{typeof(T).Name}";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed) return;

            if (isDisposing)
            {
                _lockThread.Wait();
                try
                {
                    _contextData.Clear();
                    _contextData = null;
                }
                finally
                {
                    _lockThread.Release();
                    _lockThread.Dispose();
                    _lockThread = null;
                }
            }

            _isDisposed = true;
        }
    }
}
