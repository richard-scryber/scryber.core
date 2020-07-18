using System;
using System.Collections.Generic;

namespace Scryber
{
    public static class ServiceProvider
    {

        private static IServiceProvider _services;
        private static bool _initialized = false;


        public static T GetService<T>() where T : class
        {
            if (null == _services)
                throw new NullReferenceException("The services provider has not been set");
            return _services.GetService(typeof(T)) as T;
        }

        public static void SetProvider(IServiceProvider provider)
        {
            if (null == provider)
                throw new ArgumentNullException(nameof(provider));

            if (null != _services)
                provider = new FallBackServiceProvider(provider, _services);
            _services = provider;
        }

        public static void Init()
        {
            IServiceProvider first = new FirstServices();
            IServiceProvider prev = null;

            if (null != _services)
                prev = _services;

            _services = first;

            if (null != prev)
                _services = new FallBackServiceProvider(prev, _services);

            _initialized = true;
        }

        public static void EnsureInitialized()
        {
            if (!_initialized)
            {
                Init();
            }
        }

        static ServiceProvider()
        {
            EnsureInitialized();
        }

        //
        // Inner Classes
        //



        /// <summary>
        /// Forms a linked list of service providers. Aks the top to get a service, and if not fulfilled, asks the bottom
        /// </summary>
        private class FallBackServiceProvider : IServiceProvider
        {
            private IServiceProvider _top, _bottom;

            public FallBackServiceProvider(IServiceProvider top, IServiceProvider bottom)
            {
                if (null == top)
                    throw new ArgumentNullException(nameof(top));
                if (null == bottom)
                    throw new ArgumentNullException(nameof(bottom));

                this._top = top;
                this._bottom = bottom;
            }

            public object GetService(Type t)
            {
                object val = _top.GetService(t);
                if (null == val)
                    val = _bottom.GetService(t);

                return val;
            }
        }

        public class DictionaryServiceProvider : IServiceProvider
        {
            protected IDictionary<Type, object> _initialized;


            public DictionaryServiceProvider(IDictionary<Type, object> services)
            {
                _initialized = services;
            }

            public object GetService(Type t)
            {
                object found;
                if (this._initialized.TryGetValue(t, out found))
                    return found;
                else
                    return null;
            }

        }

        /// <summary>
        /// Base level services - these will be initialized by default whenever accessing the service provider
        /// </summary>
        private class FirstServices : DictionaryServiceProvider
        {
            public FirstServices() : base(InitFirst())
            {
                
            }

            private static IDictionary<Type,object> InitFirst()
            {
                var initialized = new Dictionary<Type, object>();

                initialized.Add(typeof(IPDFPathMappingService), new Utilities.LocalFilePathMappingService());
                initialized.Add(typeof(IScryberConfigurationService), new Utilities.ScryberDefaultConfigurationService());
                initialized.Add(typeof(IScryberCachingServiceFactory), new Caching.ScryberCachingServiceFactory());

                return initialized;
            }
        }


          
    }
}
