using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Generation
{
    /// <summary>
    /// Implements the Parser Factory for the XML Reflection parser
    /// </summary>
    public class PDFXMLReflectionParserFactory : IParserFactory
    {
        public PDFXMLReflectionParserFactory()
        { }

        public IComponentParser CreateParser(ParserSettings settings)
        {
            return new XMLParser(settings);
        }
    }
}
