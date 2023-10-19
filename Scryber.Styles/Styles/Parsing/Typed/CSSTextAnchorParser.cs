using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
	/// <summary>
	/// Parses the text-anchor css value (start, middle or end)
	/// </summary>
	public class CSSTextAnchorParser : CSSEnumStyleParser<TextAnchor>
	{
		public CSSTextAnchorParser()
			: base(CSSStyleItems.TextAnchorType, StyleKeys.TextAnchorKey)
		{
		}



	}
}

