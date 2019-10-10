using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFDemo.SimpleFrame.Infra.Cache
{
    public class CacheManagerInstance
    {
        private static CacheManager _cacheManager;

        private CacheManagerInstance()
        {

        }

        public static void Init(CacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public static CacheManager GetCacheManager()
        {
            return _cacheManager;
        }
    }
}
