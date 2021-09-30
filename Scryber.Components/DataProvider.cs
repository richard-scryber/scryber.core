using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber
{
    /// <summary>
    /// Contains a list of the data providers that allow components to retrieve data
    /// from external sources through an externally agreed contract that is not known to the document.
    /// </summary>
    public class PDFDataProviderList
    {
        private List<PDFDataProvider> _items;

        public PDFDataProviderList()
        {
            this._items = new List<PDFDataProvider>();
        }

        public bool TryGetProvider(string key, out IPDFDataProvider provider)
        {
            foreach (PDFDataProvider pro in this._items)
            {
                if(pro.Key == key)
                {
                    provider = pro.Instance;
                    return null != provider;
                }
            }
            provider = null;
            return false;
        }

        public bool TryGetDomainProvider(string key, string path, out IPDFDataProvider provider)
        {
            foreach (PDFDataProvider prov in this._items)
            {
                if ((string.IsNullOrEmpty(key) || key == "*") && prov.MatchesDomain(path))
                {
                    provider = prov.Instance;
                    return null != provider;
                }
                else if(prov.Key == key && prov.MatchesDomain(path))
                {
                    provider = prov.Instance;
                    return null != provider;
                }
            }

            provider = null;
            return false;
        }


        public void RegisterRequiredProvider(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            foreach (PDFDataProvider prov in this._items)
            {
                if(prov.Key == key)
                {
                    prov.Required = true;
                    return;
                }
            }

            PDFDataProvider toAdd = new PDFDataProvider(key, true);
            _items.Add(toAdd);

        }

        public void AddProvider(IPDFDataProvider provider)
        {
            if (null == provider)
                throw new ArgumentNullException("provider");

            foreach (PDFDataProvider prov in this._items)
            {
                if (prov.Key == provider.ProviderKey)
                {
                    prov.SetInstance(provider);
                    return;
                }
            }

            PDFDataProvider toAdd = new PDFDataProvider(provider);
            _items.Add(toAdd);
        }
    }

    public class PDFDataProvider
    {
        public string Key { get; private set; }

        public System.Text.RegularExpressions.Regex Domain { get; private set; }

        public bool Required { get; set; }

        public IPDFDataProvider Instance { get; private set; }

        public bool HasInstance
        {
            get { return this.Instance != null; }
        }

        public PDFDataProvider(string key, bool required)
        {
            this.Key = key;
        }

        public PDFDataProvider(IPDFDataProvider instance, bool required = false)
            : this(instance.ProviderKey, required)
        {
            this.Instance = instance;
            if (!string.IsNullOrEmpty(instance.DomainRegEx))
                this.Domain = new System.Text.RegularExpressions.Regex(instance.DomainRegEx);
        }

        public void SetInstance(IPDFDataProvider instance)
        {
            if (null == instance)
                throw new ArgumentNullException("instance");

            this.Instance = instance;

            if (!string.IsNullOrEmpty(instance.DomainRegEx))
                this.Domain = new System.Text.RegularExpressions.Regex(instance.DomainRegEx);
        }

        public void ClearInstance()
        {
            this.Instance = null;
        }

        public bool MatchesDomain(string path)
        {
            if (null != this.Domain)
                return this.Domain.IsMatch(path);
            else
                return false;
        }

    }
}
