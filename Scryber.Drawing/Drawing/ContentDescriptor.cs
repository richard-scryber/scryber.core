using System;
using Newtonsoft.Json.Linq;

namespace Scryber.Drawing
{
	/// <summary>
	/// Base class for any 'content' style values including the supported types and values.
	/// Supports parsing and casting from a string
	/// </summary>
	public abstract class ContentDescriptor
	{

		public ContentDescriptorType Type { get; private set; }

		public virtual string Value { get; private set; }

		public ContentDescriptor Next { get; set; }

		protected ContentDescriptor(ContentDescriptorType type, string value)
		{
			this.Type = type;
			this.Value = value;
		}

		public void Append(ContentDescriptor next)
		{
			if (null == this.Next)
				this.Next = next;
			else
				this.Next.Append(next);
		}


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
			else if (value.StartsWith("attr("))
			{
				return null;
			}
			else if (value == "open-quote")
			{
				return new ContentQuoteDescriptor(value, "“");
			}
			else if(value == "close-quote")
			{
				return new ContentQuoteDescriptor(value, "”");
			}
			else if(value.StartsWith("\""))
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
				throw new NotSupportedException("The content value " + value + " is not supported, or cannot be parsed");
        }


        public override string ToString()
        {
			return this.Value;
        }

        public static explicit operator ContentDescriptor(string value)
		{
			return ContentDescriptor.Parse(value);
		}
	}

	/// <summary>
	/// Content Descriptor for the image type that can be applied with styles
	/// </summary>
	public class ContentImageDescriptor : ContentDescriptor
	{
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

			if(index <= 4)
                throw new ArgumentOutOfRangeException("content image descriptors must be in the format 'url(...)");

            var url = value.Substring(4, index-4).Trim();
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
    /// Content descriptor for textual content that can be applied with styles
    /// </summary>
    public class ContentTextDescriptor : ContentDescriptor
	{

		public ContentTextDescriptor(string val) : this(ContentDescriptorType.Text, val)
		{

		}

		public ContentTextDescriptor(ContentDescriptorType type, string val)
			: base(type, val)
		{ }
	}


	public class ContentQuoteDescriptor : ContentTextDescriptor
	{

		public string Chars{
			get;
			private set;
		}

		public ContentQuoteDescriptor(string type, string chars)
			: base(ContentDescriptorType.Quote, type)
		{
			this.Chars = chars;
		}
	}


    /// <summary>
    /// Content Descriptor for the Gradient Type that can be applied with styles
    /// </summary>
    public class ContentGradientDescriptor : ContentDescriptor
	{

		public GradientDescriptor Gradient
		{
			get;
			private set;
		}


        public ContentGradientDescriptor(GradientDescriptor gradient, string val): base(ContentDescriptorType.Gradient, val)
		{
			Gradient = gradient ?? throw new ArgumentNullException("gradient");
		}

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
    public class ContentCounterDescriptor : ContentDescriptor
	{
		public string CounterName
		{
			get;
			private set;
		}

        

        public ContentCounterDescriptor(string name, string val) : base(ContentDescriptorType.Counter, val)
		{
			this.CounterName = name;
		}

        internal static new ContentCounterDescriptor Parse(string value)
        {
            if (!value.StartsWith("counter("))
                throw new ArgumentOutOfRangeException("content counter descriptors must be in the format 'counter(name)'. Extensions or parameters are not supported");

            var index = value.IndexOf(")");

            if (index <= 4)
                throw new ArgumentOutOfRangeException("content counter descriptors must be in the format 'counter(name)'. Extensions or parameters are not supported");

            var name = value.Substring(8, index - 8).Trim();

			if(name.IndexOf(" ") >= 0 || name.IndexOf(",") > 0)
                throw new ArgumentOutOfRangeException("content counter descriptors must be in the format 'counter(name)'. Extensions or parameters are not supported");

            return new ContentCounterDescriptor(name, value);
        }
    }

    /// <summary>
    /// Content Descriptor for the Attribute Type that can be applied with styles
    /// </summary>
    public class ContentAttributeDescriptor : ContentDescriptor
	{

		public ContentAttributeDescriptor(string val) : base(ContentDescriptorType.Attribute, val)
		{ }
	}

}

