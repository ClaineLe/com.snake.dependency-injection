using System;
using System.Reflection;

namespace SnkFramework.DependencyInjection
{
    public class SnkServiceDescriptor
    {
        public static SnkServiceDescriptor Describe(object serviceInstance)
            => new SnkServiceDescriptor(serviceInstance);
        public static SnkServiceDescriptor Describe(Type instanceType)
            => new SnkServiceDescriptor(instanceType);
        public static SnkServiceDescriptor Describe(Func<ISnkServiceProvider, object> instanceFactory)
            => new SnkServiceDescriptor(instanceFactory);

        private bool _enableLogger = true;
        private Type[] _serviceInterfaces;
        private PropertyInfo[] _properties;
        private Type[] _dependencies;
        
        public bool EnableLogger
        {
            get => this._enableLogger;
            set => this._enableLogger = value;
        }

        public object ServiceInstance { get; }

        public Type InstanceType { get; }

        public Func<ISnkServiceProvider, object> InstanceFactory { get; }

        private SnkServiceDescriptor(object serviceInstance)
        {
            this.ServiceInstance = serviceInstance;
            this.InstanceType = serviceInstance.GetType();
        }

        private SnkServiceDescriptor(Type instanceType)
        {
            this.InstanceType = instanceType;
        }
        
        private SnkServiceDescriptor(Func<ISnkServiceProvider, object> instanceFactory)
        {
            this.InstanceFactory = instanceFactory;
            var arguments = instanceFactory.GetType().GenericTypeArguments;
            this.InstanceType = arguments[arguments.Length - 1];
        }
    }
}