using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Scryber.Text
{
	public class PDFXHTMLTextReaderUnPreserved : PDFTextReader
	{
		/// <summary>
		/// Gets the original text this reader was initialised with.
		/// </summary>
		public string Text { get; private set; }

		/// <summary>
		/// Returns true if this XHTML reader should preserve the returns, tabs and spaces within the string.
		/// </summary>
		public bool PreserveSpace { get; private set; }

		public override int Length
		{
			get {
				int count = 0;
				foreach (string s in this.Lines)
				{
					if (s != "\n")
						count += s.Length;
				}
				return count;
			}
		}



		public override bool EOF
		{
			get
			{
				return this.Index >= this.Lines.Length;
			}
		}

		/// <summary>
		/// The array of lines in this 
		/// </summary>
		private string[] Lines { get; set; }

		private int Index { get; set; }

		private PDFTextOp Current { get; set; }

		public override PDFTextOp Value
		{
			get
			{
				if (this.Current == null)
				{
					if (this.Index < 0)
						throw new ArgumentOutOfRangeException("Always call read on the text reader before accessing the first value");
					if (this.Index >= this.Lines.Length)
						throw new ArgumentOutOfRangeException("Cannt read past the end of the test string");
					string s = this.Lines[this.Index];
					if (s == "\n")
						this.Current = new PDFTextNewLineOp();
					else
						this.Current = new PDFTextDrawOp(s);
				}
				return this.Current;
			}
		}



		public PDFXHTMLTextReaderUnPreserved(string text)
			: base()
		{
			this.Text = text;
			this.PreserveSpace = false;

			this.SplitXHTML();
		}


		public override bool Read()
		{
			this.Index++;
			this.Current = null;

			return this.Index < this.Lines.Length;
		}


		protected override void ResetTextMarkers()
		{
			this.Current = null;
			this.Index = -1;
		}


		//
		// Splitting implementation
		//

		/// <summary>
		/// Although the Non-breaking space is considered as whitespace it is not to be split or contracted into a single space.
		/// </summary>
		private const char NBSPChar = (char)160;

		protected virtual void SplitXHTML()
		{

			var txt = this.Text;
			if(string.IsNullOrEmpty(txt))
			{
				this.Lines = new string[] { };
				return;
			}

			StringBuilder buffer = new StringBuilder(txt.Length);
			bool lastWasWhiteSpace = false;
			var index = 0;

			while(index < txt.Length)
			{
				var c = txt[index];
				if (char.IsWhiteSpace(c) && !(c == NBSPChar))
				{
					if (!lastWasWhiteSpace)
						buffer.Append(' ');
					lastWasWhiteSpace = true;
				}
				//else if (c == '&')
				//{
    //                int start = ++index;
				//	int end = 0;
				//	int max = Math.Max(start + 8, txt.Length);

				//	while(index < max)
				//	{
				//		c = txt[index];
				//		if(c == ';')
				//		{
				//			end = index;
				//			break;
				//		}
				//		index++;
				//	}
				//	if(end > start)
				//	{
				//		var escape = txt.Substring(start, end - start);

				//		if (escape[0] == '#' && int.TryParse(escape.Substring(1), out var result))
    //                    {
    //                        buffer.Append((char)result);
				//		}
				//		else if (HtmlEntities.TryGetValue(escape, out char toreplace))
				//			buffer.Append(toreplace);
				//		else
				//			buffer.Append("&" + escape + ";");
    //                }
				//	else if(end == 0)
				//	{
				//		//Just add ampersand
				//		buffer.Append('&');
				//		index = start - 1;
				//	}
					
				//	lastWasWhiteSpace = false;
				//}
				else
				{
					buffer.Append(txt[index]);
					lastWasWhiteSpace = false;
				}

				index++;

				
			}

            this.Lines = new string[] { buffer.ToString() };
            this.Index = -1;
            return;

            if (this.PreserveSpace == false) {

				if (char.IsWhiteSpace(txt, 0))
					txt = " " + txt.TrimStart();

				if (char.IsWhiteSpace(txt, txt.Length - 1))
					txt = txt.TrimEnd() + " ";
			}

			txt = this.ReplaceXHTMLChars(txt);

			if (this.PreserveSpace)
				this.Lines = this.SplitXHTMLPreservedLines(txt);
			else
				this.Lines = this.SplitXHTMLLines(txt);

			this.Index = -1;
		}


		private static IDictionary<string, char> HtmlEntities = Scryber.Html.HtmlEntities.DefaultKnownHTMLEntities;
		private static Regex matcher = new Regex("&(\\w{1,8});");

		protected virtual string ReplaceXHTMLChars(string text)
		{
			if (string.IsNullOrEmpty(this.Text)) {
				return string.Empty;
			}


			if (text.IndexOf('&') > -1)
			{
				var updated = matcher.Replace(text, (m) =>
				{
					var toswitch = m.Groups[1].Value;
					if (HtmlEntities.TryGetValue(toswitch, out char toreplace))
						return toreplace.ToString();
					else
						return m.Value;
				});

				return updated;

			}
			else
			{
				return text;
			}

		}

		private static char[] SplitChars = new char[] { '\n' };
		private static string[] EmptyString = new string[] { };

		protected virtual string[] SplitXHTMLPreservedLines(string text)
		{
			if (string.IsNullOrEmpty(text))
				return EmptyString;

			var lines = text.Split(SplitChars);

			if(lines.Length > 0)
			{
				List<string> act = new List<string>(lines.Length * 2);

				for(var i = 0; i < lines.Length; i++)
				{
					if (i > 0)
						act.Add("\n");

					act.Add(lines[i]);
				}
				lines = act.ToArray();
			}

			this.Index = -1;

			return lines;
		}

		protected virtual string[] SplitXHTMLLines(string text)
		{
			if (string.IsNullOrEmpty(text))
				return EmptyString;

            StringSplitOptions opts = StringSplitOptions.RemoveEmptyEntries;

            var lines = text.Split(new char[] { '\n' }, opts);
			//As we are not preserving space, then we 
			StringBuilder buffer = new StringBuilder();
            if (lines.Length > 1)
            {
                for (int i = 0; i < lines.Length; i++)
                {
					buffer.Append(lines[i].Trim());
					buffer.Append(" ");
                }

				lines = new string[] { buffer.ToString() };
            }
			else
			{
				buffer.Append(lines[0].Trim());
				buffer.Append(" ");
				lines = new string[] { buffer.ToString() };
			}

            this.Index = -1;

			return lines;
        }

		
    }
}

