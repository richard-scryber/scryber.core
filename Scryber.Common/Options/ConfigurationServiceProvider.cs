using System;
using Microsoft.Extensions.Configuration;

namespace Scryber.Options
{
    public class ConfigurationServiceProvider : IServiceProvider
    {
        private readonly IConfiguration _config;
        
        public ConfigurationServiceProvider(IConfiguration config)
        {
            _config = config;
        }
        
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IConfiguration))
                return this._config;
            else
                return null;
        }
    }
    
}