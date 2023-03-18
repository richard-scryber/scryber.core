using System;
using System.IO;
using System.Xml;
using Scryber.Generation;

namespace Scryber.Html.Parsing
{

    public class HTMLFragmentParserFactory : IParserFactory
    {
        public MimeType[] SupportedTypes { get; }

        public HTMLFragmentParserFactory()
        {
            this.SupportedTypes = new MimeType[] { MimeType.Html };
        }

        public IComponentParser CreateParser(MimeType forType, ParserSettings settings)
        {
            if (!forType.IsValid || !forType.Equals(MimeType.Html))
                throw new ArgumentOutOfRangeException("The html fragment parser factroy can only create parsers for the '" + MimeType.Html.ToString() + "' type of content ");

            return new HTMLFragmentParser(settings);
        }
    }

    /// <summary>
    /// Implements the parsing of non-formal html content
    /// </summary>
	public class HTMLFragmentParser : IComponentParser
	{
        public object RootComponent { get; set; }

        public ParserSettings Settings{
            get;
            private set;
        }

        public HTMLFragmentParser(ParserSettings settings)
		{
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
		}

        public IComponent Parse(string source, Stream stream, ParseSourceType type)
        {
            using (var sr = new StreamReader(stream))
                return Parse(source, sr, type);
        }

        public IComponent Parse(string source, TextReader reader, ParseSourceType type)
        {
            throw new NotImplementedException();

        }

        public IComponent Parse(string source, XmlReader reader, ParseSourceType type)
        {
            throw new NotImplementedException();
        }
    }
}

