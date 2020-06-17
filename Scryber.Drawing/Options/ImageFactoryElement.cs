using System;
using System.Configuration;

namespace Scryber.Configuration
{

    [Obsolete("Use the IScryberCOnfigurationSerive, from the ServiceProvider", true)]
    public class ImageFactoryElement : ConfigurationElement
    {

        #region public string Extension {get;set;}

        private const string ExtensionKey = "match-path";

        /// <summary>
        /// Gets or sets the extension the image data factory handles
        /// </summary>
        [ConfigurationProperty(ExtensionKey, IsKey = true, IsRequired = true)]
        public string MatchPath
        {
            get { return this[ExtensionKey] as string; }
            set { this[ExtensionKey] = value; }
        }

        #endregion

        #region public string FactoryType {get;set;}

        private const string TypeKey = "factory-type";

        /// <summary>
        /// Gets or sets the name for the factory type that this configuration element refers to.
        /// </summary>
        [ConfigurationProperty(TypeKey, IsRequired = true)]
        public string FactoryType
        {
            get { return this[TypeKey] as string; }
            set { this[TypeKey] = value; }
        }

        #endregion

        private System.Text.RegularExpressions.Regex _expr;

        public System.Text.RegularExpressions.Regex Expression
        {
            get
            {
                if (null == _expr)
                    _expr = new System.Text.RegularExpressions.Regex(this.MatchPath);
                return _expr;
            }
        }

        private IPDFImageDataFactory _factory;

        public IPDFImageDataFactory Factory
        {
            get
            {
                if (null == _factory)
                    _factory = this.GetFactory();
                return _factory;
            }
        }

        public bool IsMatch(string path, out IPDFImageDataFactory factory)
        {
            if (null == this._expr)
                _expr = new System.Text.RegularExpressions.Regex(this.MatchPath);
            if (this._expr.IsMatch(path))
            {
                factory = this.GetFactory();
                return true;
            }
            else
            {
                factory = null;
                return false;
            }
        }

        protected virtual IPDFImageDataFactory GetFactory()
        {
            if (null == this._factory)
            {
                try
                {
                    Type t = Scryber.Utilities.TypeHelper.GetType(this.FactoryType);
                    object instance = System.Activator.CreateInstance(t);
                    IPDFImageDataFactory factory = (IPDFImageDataFactory)instance;
                    _factory = factory;
                }
                catch (Exception ex)
                {
                    throw new Scryber.PDFException(string.Format("Could not load the imaging factory '{0}'", this.FactoryType), ex);
                }
            }
            return _factory;
        }
    }

    [Obsolete("Use the IScryberCOnfigurationSerive, from the ServiceProvider", true)]
    public class ImageDataFactoryCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ImageFactoryElement();
        }

        protected override object GetElementKey(ConfigurationElement Component)
        {
            ImageFactoryElement factory = (ImageFactoryElement)Component;

            return factory.MatchPath;
        }

        public Options.PDFImageFactoryList GetList()
        {
            Options.PDFImageFactoryList list = new Options.PDFImageFactoryList(this.Count);
            foreach (ImageFactoryElement item in this)
            {
                list.Add(new Options.PDFImageFactory(item.MatchPath, item.Expression, item.Factory));
            }
            return list;
        }

    }
}
