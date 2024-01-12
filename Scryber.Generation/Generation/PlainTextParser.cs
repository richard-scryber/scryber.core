using System;
using System.IO;
using System.Xml;

namespace Scryber.Generation
{
	public class PlainTextParser : IComponentParser
	{
        public ParserSettings Settings { get; private set; }

		public PlainTextParser(ParserSettings settings)
		{
            this.Settings = settings;
		}

        public object RootComponent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IComponent Parse(string source, Stream stream, ParseSourceType type)
        {
            return Parse(source, new StreamReader(stream), type);
        }

        public IComponent Parse(string source, TextReader reader, ParseSourceType type)
        {
            var content = reader.ReadToEnd();
            var literal = Activator.CreateInstance(this.Settings.TextLiteralType) as IPDFTextLiteral;

            literal.Text = content;
            literal.ReaderFormat = TextFormat.Plain;

            //TODO: Split up the content and add binding support

            return literal;
            
        }

        public IComponent Parse(string source, XmlReader reader, ParseSourceType type)
        {
            throw new NotImplementedException("The palin text parser cannot understand xml");
        }
    }
}

