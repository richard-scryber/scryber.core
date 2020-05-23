using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Generation
{
    /// <summary>
    /// Implements the PDF Parser Factory for the XML Reflection parser
    /// </summary>
    public class PDFXMLReflectionParserFactory : IPDFParserFactory
    {
        public PDFXMLReflectionParserFactory()
        { }

        public IPDFParser CreateParser(PDFGeneratorSettings settings)
        {
            return new PDFXMLParser(settings);
        }
    }
}
