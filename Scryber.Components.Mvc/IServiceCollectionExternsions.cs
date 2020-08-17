using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Scryber.Components.Mvc
{
    public static class IServiceCollectionExternsions
    {
        public static IServiceCollection AddScryberServices(this IServiceCollection collection, IConfiguration config)
        {

            collection.AddSingleton<IScryberConfigurationService>(new ScryberRootConfigurationService(config));
            var provider = collection.BuildServiceProvider();
            ServiceProvider.SetProvider(provider);
            return collection;
        }
    }



    
}
