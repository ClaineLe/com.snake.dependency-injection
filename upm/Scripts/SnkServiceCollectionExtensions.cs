using System;

namespace SnkFramework.DependencyInjection
{
    public static class SnkServiceCollectionExtensions
    {
        public static void AddSingleton<TServiceInstance>(this ISnkServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(TServiceInstance));
        }
        
        public static void AddSingleton(this ISnkServiceCollection serviceCollection, Func<ISnkServiceProvider, object> implementationFactory)
        {
            serviceCollection.Add(SnkServiceDescriptor.Describe(implementationFactory));
        }
        
        public static void AddSingleton(this ISnkServiceCollection serviceCollection, object instance)
        {
            serviceCollection.Add(SnkServiceDescriptor.Describe(instance));
        }
        
        public static void AddSingleton(this ISnkServiceCollection serviceCollection, Type instanceType)
        {
            serviceCollection.Add(SnkServiceDescriptor.Describe(instanceType));
        }

        public static ISnkServiceProvider BuildServiceProvider(this ISnkServiceCollection serviceCollection, bool enableLogger)
        {
            return SnkServiceProvider.Build(serviceCollection, enableLogger);
        }

    }
}