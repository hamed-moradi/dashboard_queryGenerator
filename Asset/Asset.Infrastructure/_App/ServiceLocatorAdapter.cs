using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using SimpleInjector;

namespace Asset.Infrastructure._App
{
    public class ServiceLocatorAdapter
    {
        public static SimpleInjectorServiceLocatorAdapter Current;

        public ServiceLocatorAdapter(Container container)
        {
            Current = new SimpleInjectorServiceLocatorAdapter(container);
        }
    }

    public class SimpleInjectorServiceLocatorAdapter : IServiceLocator
    {
        private readonly Container _container;

        public SimpleInjectorServiceLocatorAdapter(Container container)
        {
            _container = container;
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return _container.GetAllInstances(typeof(TService)).Cast<TService>();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            IServiceProvider serviceProvider = _container;
            Type collectionType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var collection = (IEnumerable<object>) serviceProvider.GetService(collectionType);
            return collection ?? Enumerable.Empty<object>();
        }

        public TService GetInstance<TService>(string key)
        {
            return (TService) GetInstance(typeof(TService));
        }

        public TService GetInstance<TService>()
        {
            return (TService) _container.GetInstance(typeof(TService));
        }

        public object GetInstance(Type serviceType, string key)
        {
            return GetInstance(serviceType);
        }

        public object GetInstance(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }

        public object GetService(Type serviceType)
        {
            IServiceProvider serviceProvider = _container;
            return serviceProvider.GetService(serviceType);
        }
    }
}