using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SnkFramework.DependencyInjection
{
    public class SnkServiceProvider : ISnkServiceProvider
    {
        private readonly bool _enableLogger;
        
        private readonly Dictionary<Type, object> _dictionary = new Dictionary<Type, object>();
        protected SnkServiceProvider(bool enableLogger = false)
        {
            this._enableLogger = enableLogger;
        }

        public static ISnkServiceProvider Build(ISnkServiceCollection serviceCollection, bool enableLogger)
        {
            var serviceProvider = new SnkServiceProvider(enableLogger);
            var dictionary = serviceProvider.EnsureServiceInstance(serviceCollection);
            foreach (var kvp in dictionary)
            {
                serviceProvider._dictionary.Add(kvp.Key, kvp.Value);
            }
            serviceProvider._dictionary.Add(typeof(ISnkServiceProvider), serviceProvider);
            
            foreach (var kvp in dictionary)
                serviceProvider.Inject(kvp.Value);
            
            return serviceProvider;
        }

        private Dictionary<Type, object> EnsureServiceInstance(ISnkServiceCollection serviceCollection)
        {
            var dictionary = new Dictionary<Type, object>();
            foreach (var descriptor in serviceCollection)
            {
                if(descriptor == null)
                    throw new Exception("descriptor is null");

                object serviceInstance = null;
                if (descriptor.ServiceInstance != null)
                {
                    serviceInstance = descriptor.ServiceInstance;
                }
                else if(descriptor.InstanceFactory != null)
                {
                    serviceInstance = descriptor.InstanceFactory.Invoke(this);
                }
                else
                {
                    serviceInstance = Activator.CreateInstance(descriptor.InstanceType);
                }
                
                if(serviceInstance == null)
                {
                    throw new Exception("serviceInstance is null");
                }

                var interfaces = serviceInstance.GetType().GetInterfaces();
                if (interfaces.Length == 0)
                    throw new Exception($"对象没有找到可注册的接口。对象：{serviceInstance}");

                var injectableCount = 0;
                foreach (var @interface in interfaces)
                {
                    var attribute = @interface.GetCustomAttribute<SnkInjectAttribute>(true);
                    if (attribute != null)
                    {
                        dictionary.Add(@interface, serviceInstance);
                        injectableCount++;
                    }
                }

                if (injectableCount == 0)
                {
                    throw new Exception($"对象没有找到可注册的接口。对象：{serviceInstance}");
                }
            }

            return dictionary;
        }

        public void Inject(object target)
        {
            var properties = target.GetType()
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(prop => prop.CanWrite && prop.GetCustomAttribute<SnkInjectAttribute>() != null);
            Inject(target, properties);
        }
        
        public void Inject(object target, IEnumerable<PropertyInfo> properties)
        {
            foreach (var propertyInfo in properties)
            {
                var propertyValue = this.GetService(propertyInfo.PropertyType);
                if (propertyValue == null)
                {
                    throw new Exception($"没有找到参数. target:{target} service:{propertyInfo.PropertyType}");
                }
                propertyInfo.SetValue(target, propertyValue);
            }
        }

        public object GetService(Type serviceType)
        {
            if (this._dictionary.TryGetValue(serviceType, out var service) == false)
                return null;
            return service;
        }

        public T GetService<T>() where T : class
            => this.GetService(typeof(T)) as T;

        public IEnumerable<T> GetServices<T>() where T : class
        {
            var list = new List<T>();
            var interfaceType = typeof(T);
            foreach (var tuple in _dictionary)
            {
                if (interfaceType.IsInstanceOfType(tuple.Value))
                {
                    list.Add( tuple.Value as T);
                }
            }
            return list.Distinct();
        }

        public T CreateInstance<T>(bool inject = true, params object[] parameters) where T : class
            => CreateInstance(typeof(T), inject, parameters) as T;

        public object CreateInstance(Type instanceType, bool inject = true, params object[] parameters)
        {
            var instance = Activator.CreateInstance(instanceType, parameters);
            Inject(instance);
            return instance;
        }
    }
}