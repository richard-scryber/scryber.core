using System;
namespace Scryber.Caching
{
	public class PDFNoCachingProvider : ICacheProvider
	{
		public PDFNoCachingProvider()
		{
		}

        public void AddToCache(string type, string key, object data)
        {
            
        }

        public void AddToCache(string type, string key, object data, TimeSpan duration)
        {
            
        }

        public void AddToCache(string type, string key, object data, DateTime expires)
        {
            
        }

        public bool TryRetrieveFromCache(string type, string key, out object data)
        {
            data = null;
            return false;
        }
    }
}

