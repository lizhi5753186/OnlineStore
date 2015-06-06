using System.Collections.Generic;
using Microsoft.ApplicationServer.Caching;

namespace OnlineStore.Infrastructure.Caching
{
    // 分布式缓存，该类是对微软分布式缓存服务的封装
    // 在该案例中没用用到该缓存，但是提供在这里让大家明白微软的分布式缓存实现，并不是只有memcached和Redis
    // Redis参考：http://www.cnblogs.com/ceecy/p/3279407.html 和 http://blog.csdn.net/suifeng3051/article/details/23739295
    // 关于微软分布式缓存更多介绍参考：http://www.cnblogs.com/shanyou/archive/2010/06/29/AppFabricCaching.html 
    // 和http://www.cnblogs.com/mlj322/archive/2010/04/05/1704624.html
    public class AppfabricCacheProvider : ICacheProvider
    {
        private readonly DataCacheFactory _factory = new DataCacheFactory();
        private readonly DataCache _cache;

        public AppfabricCacheProvider()
        {
            this._cache = _factory.GetDefaultCache();
        }

        #region ICacheProvider Members
        public void Add(string key, string valueKey, object value)
        {
            // DataCache中不包含Contain方法，所有用Get方法来判断对应的key值是否在缓存中存在
            var val = (Dictionary<string, object>)_cache.Get(key);
            if (val == null)
            {
                val = new Dictionary<string, object> {{ valueKey, value}};
                _cache.Add(key, val);
            }
            else
            {
                if (!val.ContainsKey(valueKey))
                    val.Add(valueKey, value);
                else
                    val[valueKey] = value;

                _cache.Put(key, val);
            }
        }

        public void Update(string key, string valueKey, object value)
        {
            Add(key, valueKey, value);
        }

        public object Get(string key, string valueKey)
        {
            return Exists(key, valueKey) ? ((Dictionary<string, object>)_cache.Get(key))[valueKey] : null;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public bool Exists(string key)
        {
            return _cache.Get(key) != null;
        }

        public bool Exists(string key, string valueKey)
        {
            var val = _cache.Get(key);
            if (val == null)
                return false;
            return ((Dictionary<string, object>)val).ContainsKey(valueKey);
        }

        #endregion 
    }
}