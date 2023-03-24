using System;
using System.Collections.Generic;

namespace Scryber.Options
{
	public class ParserFactoryDictionary : Dictionary<MimeType, IParserFactory>
	{
		public ParserFactoryDictionary()
		{
		}
	}
}

