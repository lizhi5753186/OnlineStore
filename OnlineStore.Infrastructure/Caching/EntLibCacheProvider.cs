using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace OnlineStore.Infrastructure.Caching
{
    // 表示基于Microsoft Patterns & Practices - Enterprise Library Caching Application Block的缓存机制的实现
    // 该类简单理解为对Enterprise Library Caching中的CacheManager封装
    // 该缓存实现不支持分布式缓存，更多信息参考: 
    // http://stackoverflow.com/questions/7799664/enterpriselibrary-caching-in-load-balance 
    public class EntLibCacheProvider : ICacheProvider
    {
        // 获得CacheManager实例，该实例的注册通过cachingConfiguration进行注册进去的，具体看配置文件
        private readonly ICacheManager _cacheManager = CacheFactory.GetCacheManager();

        #region ICahceProvider

        public void Add(string key, string valueKey, object value)
        {
            Dictionary<string, object> dict = null;
            if (_cacheManager.Contains(key))
            {
                dict = (Dictionary<string, object>) _cacheManager[key];
                dict[valueKey] = value;
            }
            else
            {
                dict = new Dictionary<string, object> { { valueKey, value }};
            }

            _cacheManager.Add(key, dict);
        }

        public void Update(string key, string valueKey, object value)
        {
            Add(key, valueKey, value);
        }

        public object Get(string key, string valueKey)
        {
            if (!_cacheManager.Contains(key)) return null;
            var dict = (Dictionary<string, object>)_cacheManager[key];
            if (dict != null && dict.ContainsKey(valueKey))
                return dict[valueKey];
            else
                return null;
        }

        // 从缓存中移除对象
        public void Remove(string key)
        {
            _cacheManager.Remove(key);
        }

        // 判断指定的键值的缓存是否存在
        public bool Exists(string key)
        {
            return _cacheManager.Contains(key);
        }

        // 判断指定的键值和缓存键值的缓存是否存在
        public bool Exists(string key, string valueKey)
        {
            return _cacheManager.Contains(key) &&
               ((Dictionary<string, object>)_cacheManager[key]).ContainsKey(valueKey);
        }
        #endregion 
    }
}