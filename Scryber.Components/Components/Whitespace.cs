using System;
using Scryber.Styles;
using Scryber.Text;

namespace Scryber.Components
{
    [PDFParsableComponent("Whitespace")]
	public class Whitespace : Component, IPDFTextLiteral, ITextComponent
	{
        private string _text;

        /// <summary>
        /// The text value of this literal
        /// </summary>
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "General", Priority = 2, Type = "String")]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        private TextFormat _reader = TextFormat.XML;

        [PDFAttribute("format")]
        public TextFormat ReaderFormat
        {
            get { return _reader; }
            set { _reader = value; }
        }

        

        public Whitespace() : this(ObjectTypes.Whitespace)
		{
		}

		protected Whitespace(ObjectType type): base(type)
		{

		}

        public Whitespace(string text, TextFormat format): this()
        {
            this.Text = text;
            this.ReaderFormat = format;
        }


        public virtual PDFTextReader CreateReader(ContextBase context, Style fullstyle)
        {
            //We only return a reader if we are preserving whitespace.
            if (fullstyle.TryGetValue(StyleKeys.TextWhitespaceKey, out var found) && found.Value(fullstyle) == true)
                return PDFTextReader.Create(this.Text, this.ReaderFormat, true, context.TraceLog);
            else
                return null;
        }
	}
}

