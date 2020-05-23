using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Scryber.Configuration
{
    public static class ParserConfiguration
    {
        public const string SectionPath = ScryberConfiguration.ScryberConfigGroupKey + "/parsers";


        private static ParserConfigurationSection _section = null;

        public static ParserConfigurationSection Section
        {
            get
            {
                if(null == _section)
                    _section = (ParserConfigurationSection)ConfigurationManager.GetSection(SectionPath);
                if (null == _section)
                    _section = new ParserConfigurationSection();

                return _section;
            }
        }

        private static System.Globalization.CultureInfo _defaultCulture = null;

        public static System.Globalization.CultureInfo GetDefaultCulture()
        {
            if (null == _defaultCulture)
            {
                if (!string.IsNullOrEmpty(Section.DefaultCulture))
                {
                    _defaultCulture = System.Globalization.CultureInfo.GetCultureInfo(Section.DefaultCulture);
                }
            }
            return _defaultCulture;
        }

        public static IPDFParser GetParser(Scryber.Generation.PDFGeneratorSettings settings)
        {
            string name = Section.DefaultParser;
            IPDFParser parser = GetParser(name, settings);
            if (!string.IsNullOrEmpty(name) && null == parser)
                throw new System.Configuration.ConfigurationErrorsException("The default parser '" + name + "' is not parser defined in the configuration file. Either remove the explicit default-parser value, or include a parser with that name");

            return parser;
        }

        public static IPDFParser GetParser(string name, Scryber.Generation.PDFGeneratorSettings settings)
        {
            IPDFParserFactory factory = Section.GetParser(name);
            if (null != factory)
                return factory.CreateParser(settings);
            else
            {
                return null;
            }
        }
    }
}
