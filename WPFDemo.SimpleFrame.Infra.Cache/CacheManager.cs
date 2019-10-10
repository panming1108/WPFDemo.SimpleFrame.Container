using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Cache
{
    public class CacheManager
    {
        private static readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        public T TryGet<T>(string key)
        {
            return (T)_cache.GetOrAdd(key, default(T));
        }

        public T TrySet<T>(string key, T value)
        {
            return (T)_cache.AddOrUpdate(key, value, (tkey, oldvalue) => value);
        }
    }
}
