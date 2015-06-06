using System;

namespace OnlineStore.Infrastructure.Caching
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CacheAttribute : Attribute
    {
        public CachingMethod Method { get; set; }

        public bool IsForce { get; set; }
        
        // 缓存相关的方法名称，该参数仅在Remove的方式用到
        public string[] CorrespondingMethodNames { get; set; }

        public CacheAttribute(CachingMethod method)
        {
            this.Method = method;
        }

        public CacheAttribute(CachingMethod method, params string[] correspondingMethodNames)
            : this(method)
        {
            this.CorrespondingMethodNames = correspondingMethodNames;
        }
    }
}