using System;

namespace Scryber.Caching
{
    public class ScryberCachingServiceFactory : IScryberCachingServiceFactory
    {
        public ScryberCachingServiceFactory()
        {
        }

        public IPDFCacheProvider GetProvider()
        {
            return new PDFStaticCacheProvider();
        }
    }
}
