using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Scryber.Generation
{
    /// <summary>
    /// Adds specific functionality to replace the {#each ...}, {#if ...} handlebar helpers with the known xml handle bar components.
    /// </summary>
    public class XHtmlHandleHelperReader : XmlTextReader
    {
        
        public const string HandlebarNamespace = "Scryber.Handlebar.Components, Scryber.Components";
        public const string HandlebarPrefix = "hbar:";
        private static HBarHelperMapping[] KnownHelpers = new HBarHelperMapping[]
        {
            new HBarEquals()
        };
        


        public XHtmlHandleHelperReader(System.IO.Stream stream) : this(GetSanitizedStream(stream))
        {
        }

        public XHtmlHandleHelperReader(System.IO.TextReader reader) : base(GetSanitizedReader(reader))
        {
        }

        private static TextReader GetSanitizedStream(Stream stream)
        {
            var str = ExtractString(stream);
            str = SanitizeString(str);
            return new StringReader(str);
        }

        private static TextReader GetSanitizedReader(TextReader reader)
        {
            var str = ExtractString(reader);
            str = SanitizeString(str);
            return new StringReader(str);
        }


        private static string ExtractString(Stream stream)
        {
            return ExtractString(new StreamReader(stream));
        }

        private static string ExtractString(TextReader reader)
        {
            var all = reader.ReadToEnd();
            return all;
        }

        private static string SanitizeString(string input)
        {
            var helper = new HBarHelperSplitter(HandlebarNamespace, HandlebarPrefix, KnownHelpers);
            var output = helper.ReplaceAll(input);
            return output;
        }

        
    }
}
