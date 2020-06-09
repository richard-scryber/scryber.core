using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Scryber.Configuration
{
    [Obsolete("Use the IScryberConfigurationSerive, from the ServiceProvider", true)]
    public class ParserFactoryElement : ConfigurationElement
    {

        #region public string Name {get; set;} : @name

        private const string NameKey = "name";

        /// <summary>
        /// Gets or sets the name of the parser so it can be referred to.
        /// </summary>
        [ConfigurationProperty(NameKey, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this[NameKey] as string; }
            set { this[NameKey] = value; }
        }

        #endregion


        #region public string FactoryTypeName {get; set;} : @factory-type

        private const string FactoryTypeKey = "factory-type";

        /// <summary>
        /// Gets or sets the name of the type that will create instances that support parsing
        /// </summary>
        [ConfigurationProperty(FactoryTypeKey, IsKey = false, IsRequired = true)]
        public string FactoryTypeName
        {
            get { return this[FactoryTypeKey] as string; }
            set { this[FactoryTypeKey] = value; }
        }

        #endregion



        private static object _lock = new object();
        private IPDFParserFactory _factory = null;

        public IPDFParserFactory GetFactory()
        {
            if (null == _factory)
            {
                if (string.IsNullOrEmpty(this.FactoryTypeName))
                    throw new ConfigurationErrorsException("Required configuration value is not set - factory-type");

                _factory = GetFactoryFromTypeName();
            }
            return _factory;
        }

        private IPDFParserFactory GetFactoryFromTypeName()
        {
            lock (_lock)
            {
                IPDFParserFactory factory;
                Type type = Support.GetTypeFromName(this.FactoryTypeName);
                object instance = Activator.CreateInstance(type);

                factory = (IPDFParserFactory)instance;

                return factory;
            }
        }
    }
}
