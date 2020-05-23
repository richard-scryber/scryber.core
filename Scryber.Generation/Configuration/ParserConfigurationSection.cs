using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Scryber.Configuration
{
    public class ParserConfigurationSection : ConfigurationSection
    {
        public const string DefaultParserKey = "default-parser";

        [ConfigurationProperty(DefaultParserKey)]
        public string DefaultParser
        {
            get
            {
                return this[DefaultParserKey] as string;
            }
            set
            {
                this[DefaultParserKey] = value;
            }
        }

        public const string DefaultCultureKey = "default-culture";

        [ConfigurationProperty(DefaultCultureKey)]
        public string DefaultCulture
        {
            get
            {
                return this[DefaultCultureKey] as string;
            }
            set
            {
                this[DefaultCultureKey] = value;
            }
        }


        public const string FactoryCollectionKey = "";

        [ConfigurationCollection(typeof(ParserFactoryElement),
            CollectionType=ConfigurationElementCollectionType.AddRemoveClearMap)]
        [ConfigurationProperty(FactoryCollectionKey,IsDefaultCollection =true, IsRequired =false)]
        public ParserFactoryElementCollection Factories
        {
            get
            {
                ParserFactoryElementCollection col = this[FactoryCollectionKey] as ParserFactoryElementCollection;

                return col;
            }
            set
            {
                this[FactoryCollectionKey] = value;
            }
        }

        public IPDFParserFactory GetParser(string name)
        {
            ParserFactoryElement found = null;

            ParserFactoryElementCollection col = this.Factories;
            if (null != col)
            {
                foreach (ParserFactoryElement item in this.Factories)
                {
                    if (item.Name == name)
                    {
                        found = item;
                        break;
                    }
                }
            }

            if (null == found)
                return null;
            else
                return found.GetFactory();
        }
    }
}
