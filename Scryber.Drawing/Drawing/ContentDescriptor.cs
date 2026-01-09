using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Scryber.Drawing
{
    /// <summary>
    /// Base class for any 'content' style values including the supported types and values.
    /// Supports parsing and casting from a string
    /// </summary>
    /// <remarks>The content descriptor supports the CSS content: property values, and can be parsed from a string.
    ///
    /// It also suppors chaining as single linked list of descriptors using the Next property, and the append methods.
    /// </remarks>
    public abstract class ContentDescriptor
	{

		/// <summary>
		/// The type of this content descriptor
		/// </summary>
		public ContentDescriptorType Type { get; private set; }

		/// <summary>
		/// A next content descriptor, if there is one.
		/// </summary>
		public ContentDescriptor Next { get; set; }


		/// <summary>
		/// Protected constructor that takes the type
		/// </summary>
		/// <param name="type">Defines the type of this content descriptor</param>
		protected ContentDescriptor(ContentDescriptorType type)
		{
			this.Type = type;
			
		}

		/// <summary>
		/// Adds the content descriptor specified to the end of the linked list
		/// </summary>
		/// <param name="next"></param>
		public void Append(ContentDescriptor next)
		{
			if (null == this.Next)
				this.Next = next;
			else
				this.Next.Append(next);
		}


		/// <summary>
		/// Parses a SINGLE string part of the CSS content: property value into a ContentDescriptor of the known types.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <exception cref="NotSupportedException">Thrown if the value cannot be converted.</exception>
		public static ContentDescriptor Parse(string value)
		{
			if (string.IsNullOrEmpty(value))
				return null;

			value = value.Trim();
			if (string.IsNullOrEmpty(value))
				return null;

			if (value.StartsWith("url("))
			{
				return ContentImageDescriptor.Parse(value);
			}
			else if (value.StartsWith("linear-gradient("))
			{
				return ContentGradientDescriptor.Parse(value);
			}
			else if (value.StartsWith("radial-gradient("))
			{
				return ContentGradientDescriptor.Parse(value);
			}
			else if (value.StartsWith("repeating-linear-gradient("))
			{
				return ContentGradientDescriptor.Parse(value);
			}
			else if (value.StartsWith("repeating-radial-gradient("))
			{
				return ContentGradientDescriptor.Parse(value);
			}
			else if (value.StartsWith("counter("))
			{
				return ContentCounterDescriptor.Parse(value);
			}
			else if (value.StartsWith("counters("))
			{
				return ContentCountersDescriptor.Parse(value);
			}
			else if (value.StartsWith("attr("))
			{
				return ContentAttributeDescriptor.Parse(value);
			}
			else if (value == "open-quote")
			{
				return new ContentQuoteDescriptor(value, "“");
			}
			else if (value == "close-quote")
			{
				return new ContentQuoteDescriptor(value, "”");
			}
			else if (value.StartsWith("\""))
			{
				if (value.StartsWith("\"\\"))
				{
					var charcode = value.Substring(2, value.Length - 3);
					var charVal = char.ConvertFromUtf32(int.Parse(charcode, System.Globalization.NumberStyles.HexNumber));
					return new ContentTextDescriptor(charVal.ToString());
				}
				else
					return new ContentTextDescriptor(value.Substring(1, value.Length - 2));
			}
			else if (value.StartsWith("'"))
			{
				if (value.StartsWith("'\\"))
				{
					var charcode = value.Substring(2, value.Length - 3);
					var charVal = char.ConvertFromUtf32(int.Parse(charcode, System.Globalization.NumberStyles.HexNumber));
					return new ContentTextDescriptor(charVal.ToString());
				}
				else
					return new ContentTextDescriptor(value.Substring(1, value.Length - 2));
			}
			else
				return new ContentTextDescriptor(value);
        }


        public override string ToString()
        {
			return this.Type.ToString();
        }

        public static explicit operator ContentDescriptor(string value)
		{
			return ContentDescriptor.Parse(value);
		}
	}

	

    /// <summary>
    /// Content descriptor for textual content that can be applied with styles
    /// </summary>
    public class ContentTextDescriptor : ContentDescriptor
	{
		/// <summary>
		/// The textual content
		/// </summary>
		public string Text { get; set; }

		public ContentTextDescriptor(string val)
			: this(ContentDescriptorType.Text, val)
		{
		}

		public ContentTextDescriptor(ContentDescriptorType type, string val)
			: base(type)
		{
			this.Text = val;
		}

        public override string ToString()
        {
            var all = base.ToString() + " \"" + this.Text + "\"";
			if (null != this.Next)
				all += " -> " + this.Next.ToString();
			return all;
        }
    }


	/// <summary>
	/// Content descriptor for the open-quote and close-quote
	/// </summary>
	public class ContentQuoteDescriptor : ContentTextDescriptor
	{
		/// <summary>
		/// Gets the actual quote character(s) to inject as content
		/// </summary>
		public string Chars{
			get;
			private set;
		}

		/// <summary>
		/// Creates a new instance with the type and characters
		/// </summary>
		/// <param name="type"></param>
		/// <param name="chars"></param>
		public ContentQuoteDescriptor(string type, string chars)
			: base(ContentDescriptorType.Quote, type)
		{
			this.Chars = chars;
		}
	}

    /// <summary>
    /// Content Descriptor for the image type that can be applied with styles
    /// </summary>
    public class ContentImageDescriptor : ContentTextDescriptor
    {
		/// <summary>
		/// The actual image source (without any 'url(' or quotes prefixes)
		/// </summary>
        public string Source { get; set; }


        public ContentImageDescriptor(string source, string val) : base(ContentDescriptorType.Image, val)
        {
            this.Source = source;
        }


        internal static new ContentImageDescriptor Parse(string value)
        {
            if (!value.StartsWith("url("))
                throw new ArgumentOutOfRangeException("content image descriptors must be in the format 'url(...)");

            var index = value.IndexOf(")");

            if (index <= 4)
                throw new ArgumentOutOfRangeException("content image descriptors must be in the format 'url(...)");

            var url = value.Substring(4, index - 4).Trim();
            if (url.StartsWith("\""))
            {
                if (!url.EndsWith("\""))
                    throw new ArgumentException("The specified url '" + value + " starts with a double quote but does not end with one", "value");
                url = url.Substring(1, url.Length - 2);
            }
            else if (url.StartsWith("'"))
            {
                if (!url.EndsWith("'"))
                    throw new ArgumentException("The specified url '" + value + " starts with a single quote but does not end with one", "value");
                url = url.Substring(1, url.Length - 2);
            }

            return new ContentImageDescriptor(url, value);
        }

    }


    /// <summary>
    /// Content Descriptor for the Gradient Type that can be applied with styles
    /// </summary>
    public class ContentGradientDescriptor : ContentTextDescriptor
	{

		/// <summary>
		/// Gets the parsed gradient instance
		/// </summary>
		public GradientDescriptor Gradient
		{
			get;
			private set;
		}

		


        public ContentGradientDescriptor(GradientDescriptor gradient, string val)
			: base(ContentDescriptorType.Gradient, val)
		{
			Gradient = gradient ?? throw new ArgumentNullException("gradient");
		}

		/// <summary>
		/// Parses a gradient, making sure the gradient value is correct.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static new ContentGradientDescriptor Parse(string value)
		{
			GradientDescriptor gradient = GradientDescriptor.Parse(value);

			if (null == gradient)
				return null;
			else
				return new ContentGradientDescriptor(gradient, value);
		}
	}

    /// <summary>
    /// Content Descriptor for the counter Type that can be applied with styles
    /// </summary>
    public class ContentCounterDescriptor : ContentTextDescriptor
	{
		/// <summary>
		/// Gets the actual name of the counter
		/// </summary>
		public string CounterName
		{
			get;
			private set;
		}

        

        public ContentCounterDescriptor(string name, string val)
			: base(ContentDescriptorType.Counter, val)
		{
			this.CounterName = name;
		}


		public string GetContent(IComponent component)
		{
			var curr = component;
			string val = null;
			int counterValue;

            while (null != curr)
            {
				if (curr is ICountableComponent countable)
				{
					if(countable.HasCounters && countable.Counters.TryGetValue(this.CounterName, out counterValue))
					{
						val = counterValue.ToString();
						break;
					}
				}
                curr = curr.Parent;
            }

            return val;
        }

        internal static new ContentCounterDescriptor Parse(string value)
        {
            if (!value.StartsWith("counter("))
                throw new ArgumentOutOfRangeException("content counter descriptors must be in the format 'counter(name)'. Extensions or parameters are not supported");

            var index = value.IndexOf(")");

            if (index <= 8)
                throw new ArgumentOutOfRangeException("content counter descriptors must be in the format 'counter(name)'. Extensions or parameters are not supported");

            var name = value.Substring(8, index - 8).Trim();

			if(name.IndexOf(" ") >= 0 || name.IndexOf(",") > 0)
                throw new ArgumentOutOfRangeException("content counter descriptors must be in the format 'counter(name)'. Extensions or parameters are not supported");

            return new ContentCounterDescriptor(name, value);
        }
    }

	public class ContentCountersDescriptor : ContentTextDescriptor
	{
		public string CounterName
		{
			get;
			private set;
		}

		public string Separator
		{
			get;
			private set;
		}

		public ListNumberingGroupStyle Format
		{
			get;
			set;
		}

		public ContentCountersDescriptor(string name, string separator, string value)
			: base(ContentDescriptorType.Counters, value)
		{
			this.CounterName = name;
			this.Separator = separator;
			this.Format = ListNumberingGroupStyle.Decimals;
		}

        public string GetContent(IComponent component)
        {
            var curr = component;
            
            int counterValue;
			StringBuilder sb = new StringBuilder();

            while (null != curr)
            {
				//As a top to bottom ordering we use insert at the front
				//rather than building a stack to reverse the order.
                if (curr is ICountableComponent countable && countable.HasCounters && countable.Counters.TryGetValue(this.CounterName, out counterValue))
                {
					if (sb.Length > 0)
					{
						if (!string.IsNullOrEmpty(this.Separator))
							sb.Insert(0, this.Separator);

						sb.Insert(0, counterValue);
					}
					else
						sb.Append(counterValue);
                }

                curr = curr.Parent;
            }
			if (sb.Length > 0)
				return sb.ToString();
			else
				return null;
        }

        public static new ContentCountersDescriptor Parse(string value)
		{
            if (!value.StartsWith("counters("))
                throw new ArgumentOutOfRangeException("content counters descriptors must be in the format 'counters(name, separator [,optionalformat])'.");

            var index = value.IndexOf(")");

			string name = null, separator = null, format = null;
			
            if (index <= 9)
                throw new ArgumentOutOfRangeException("content counters descriptors must be in the format 'counters(name, separator [,optionalformat])'.");

            var inner = value.Substring(9, index - 9).Trim();

			if (inner.IndexOf(',') > 0)
			{
				var all = inner.Split(',');
				name = all[0].Trim();

				if (all.Length > 1)
				{
					separator = all[1].Trim();

					if (separator.StartsWith("'") || separator.StartsWith("\""))
						separator = separator.Substring(1, separator.Length - 2);	
				}
				if (all.Length > 2)
				{
					format = all[2].Trim();
				}
			}
			else
			{
				name = inner.Trim();
				separator = " ";
				format = null;
			}

			ContentCountersDescriptor desc = new ContentCountersDescriptor(name, separator, value);

			if (!string.IsNullOrEmpty(format))
			{
				switch (format)
				{
					case ("lower-alpha"):
					case ("lower-latin"):
						desc.Format = ListNumberingGroupStyle.LowercaseLetters;
						break;
                    case ("lower-roman"):
                        desc.Format = ListNumberingGroupStyle.LowercaseRoman;
                        break;
                    case ("upper-alpha"):
                    case ("upper-latin"):
                        desc.Format = ListNumberingGroupStyle.UppercaseLetters;
                        break;
                    case ("upper-roman"):
                        desc.Format = ListNumberingGroupStyle.UppercaseRoman;
                        break;
                    default:
						desc.Format = ListNumberingGroupStyle.Decimals;
						break;

				}
			}

			return desc;
        }
    }

    /// <summary>
    /// Content Descriptor for the Attribute Type that can be applied with styles
    /// </summary>
    public class ContentAttributeDescriptor : ContentDescriptor
	{
		public string Attribute { get; set; }

		public ContentAttributeDescriptor(string val)
			: base(ContentDescriptorType.Attribute)
		{
			this.Attribute = val;
		}
		
		/// <summary>
		/// Parses an attribute descriptor, making sure the attribute name is correct.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static new ContentAttributeDescriptor Parse(string value)
		{
			if(value.StartsWith("attr(") && value.EndsWith(")"))
				value = value.Substring(5, value.Length - 6);
			return new ContentAttributeDescriptor(value);
		}
	}

}

