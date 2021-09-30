using System;

namespace Scryber.Caching
{
    public class ScryberCachingServiceFactory : IScryberCachingServiceFactory
    {
        public ScryberCachingServiceFactory()
        {
        }

        public ICacheProvider GetProvider()
        {
            return new PDFStaticCacheProvider();
        }
    }
}
