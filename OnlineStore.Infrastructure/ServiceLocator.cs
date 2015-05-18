using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace OnlineStore.Infrastructure
{
    // 服务定位器的实现
    public class ServiceLocator : IServiceProvider
    {
        private readonly IUnityContainer _container;
        private static ServiceLocator _instance = new ServiceLocator();

        private ServiceLocator()
        {
            _container = new UnityContainer();
            _container.LoadConfiguration();
        }

        public static ServiceLocator Instance
        {
            get { return _instance; }
        }

        #region Public Methods

        public T GetService<T>()
        {
            return _container.Resolve<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
        }

        public T GetService<T>(object overridedArguments)
        {
            var overrides = GetParameterOverrides(overridedArguments);
            return _container.Resolve<T>(overrides.ToArray());
        }

        public object GetService(Type serviceType, object overridedArguments)
        {
            var overrides = GetParameterOverrides(overridedArguments);
            return _container.Resolve(serviceType, overrides.ToArray());
        }

        #endregion 

        #region Private Methods
        private IEnumerable<ParameterOverride> GetParameterOverrides(object overridedArguments)
        {
            var overrides = new List<ParameterOverride>();
            var argumentsType = overridedArguments.GetType();
            argumentsType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList()
                .ForEach(property =>
                {
                    var propertyValue = property.GetValue(overridedArguments, null);
                    var propertyName = property.Name;
                    overrides.Add(new ParameterOverride(propertyName, propertyValue));
                });
            return overrides;
        }
        #endregion 

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        #endregion 
    }
}
