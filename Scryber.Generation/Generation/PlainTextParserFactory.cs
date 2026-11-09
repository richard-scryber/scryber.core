using System;
namespace Scryber.Generation
{
	public class PlainTextParserFactory : IParserFactory
	{

		public MimeType[] SupportedTypes { get; }

		public PlainTextParserFactory()
		{
			SupportedTypes = new MimeType[] { MimeType.Text };
		}

		public IComponentParser CreateParser(MimeType type, ParserSettings settings)
		{
			return new PlainTextParser(settings);
		}
	}
}

