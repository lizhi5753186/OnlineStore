using OnlineStore.Infrastructure.Caching;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Infrastructure.InterceptionBehaviors
{
    // 缓存AOP的实现
    public class CachingBehavior : IInterceptionBehavior
    {
        private readonly ICacheProvider _cacheProvider;

        public CachingBehavior()
        {
            _cacheProvider = ServiceLocator.Instance.GetService<ICacheProvider>();
        }

        // 生成缓存值的键值
        private string GetValueKey(CacheAttribute cachingAttribute, IMethodInvocation input)
        {
            switch (cachingAttribute.Method)
            {
                // 如果是Remove，则不存在特定值键名，所有的以该方法名称相关的缓存都需要清除
                case CachingMethod.Remove:
                    return null;
                // 如果是Get或者Update，则需要产生一个针对特定参数值的键名
                case CachingMethod.Get:
                case CachingMethod.Update:
                    if (input.Arguments != null &&
                        input.Arguments.Count > 0)
                    {
                        var sb = new StringBuilder();
                        for (var i = 0; i < input.Arguments.Count; i++)
                        {
                            sb.Append(input.Arguments[i]);
                            if (i != input.Arguments.Count - 1)
                                sb.Append("_");
                        }

                        return sb.ToString();
                    }
                    else
                        return "NULL";
                default:
                    throw new InvalidOperationException("无效的缓存方式。");
            }
        }

        #region IInterceptionBehavior Members
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            // 获得被拦截的方法
            var method = input.MethodBase;
            var key = method.Name; // 获得拦截的方法名
            // 如果拦截的方法定义了Cache属性，说明需要对该方法的结果需要进行缓存
            if (!method.IsDefined(typeof (CacheAttribute), false)) 
                return getNext().Invoke(input, getNext);

            var cachingAttribute = (CacheAttribute)method.GetCustomAttributes(typeof (CacheAttribute), false)[0];
            var valueKey = GetValueKey(cachingAttribute, input);
            switch (cachingAttribute.Method)
            {
                case CachingMethod.Get:
                    try
                    {
                        // 如果缓存中存在该键值的缓存，则直接返回缓存中的结果退出
                        if (_cacheProvider.Exists(key, valueKey))
                        {
                            var value = _cacheProvider.Get(key, valueKey);
                            var arguments = new object[input.Arguments.Count];
                            input.Arguments.CopyTo(arguments, 0);
                            return new VirtualMethodReturn(input, value, arguments);
                        }
                        else // 否则先调用方法，再把返回结果进行缓存
                        {
                            var methodReturn = getNext().Invoke(input, getNext);
                            _cacheProvider.Add(key, valueKey, methodReturn.ReturnValue);
                            return methodReturn;
                        }
                    }
                    catch (Exception ex)
                    {
                        return new VirtualMethodReturn(input, ex);
                    }
                case CachingMethod.Update:
                    try
                    {
                        var methodReturn = getNext().Invoke(input, getNext);
                        if (_cacheProvider.Exists(key))
                        {
                            if (cachingAttribute.IsForce)
                            {
                                _cacheProvider.Remove(key);
                                _cacheProvider.Add(key, valueKey, methodReturn.ReturnValue);
                            }
                            else
                                _cacheProvider.Update(key, valueKey, methodReturn);
                        }
                        else
                            _cacheProvider.Add(key, valueKey, methodReturn.ReturnValue);

                        return methodReturn;
                    }
                    catch (Exception ex)
                    {
                        return new VirtualMethodReturn(input, ex);
                    }
                case CachingMethod.Remove:
                    try
                    {
                        var removeKeys = cachingAttribute.CorrespondingMethodNames;
                        foreach (var removeKey in removeKeys)
                        {
                            if (_cacheProvider.Exists(removeKey))
                                _cacheProvider.Remove(removeKey);
                        }

                        // 执行具体截获的方法
                        var methodReturn = getNext().Invoke(input, getNext);
                        return methodReturn;
                    }
                    catch (Exception ex)
                    {
                        return new VirtualMethodReturn(input, ex);
                    }
                default: break;
            }

            return getNext().Invoke(input, getNext);
        }

        public bool WillExecute
        {
            get { return true; }
        }
        #endregion 
    }
}