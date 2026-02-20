using System;
namespace Scryber
{
	/// <summary>
	/// A readonly class for specifying mime-types e.g (text/html)
	/// </summary>
	[PDFParsableValue()]
	public class MimeType : IEquatable<MimeType>
	{
		/// <summary>
		/// Retuns true if the mime type at least has a root type (e.g text)
		/// </summary>
		public bool IsValid
		{
			get { return !string.IsNullOrEmpty(this.Root); }
		}

        /// <summary>
        /// Gets the first type of the MimeType (e.g text for text/html or application for application/xhtml+xml)
        /// </summary>
        public string Root { get; private set; }

		/// <summary>
		/// Gets the content type of the MimeType (e.g. html for text/html or xhtml for application/xhtml+xml)
		/// </summary>
		public string Content { get; private set; }

        /// <summary>
        /// Gets the base type of the MimeType (e.g. xml for application/xhtml+xml)
        /// </summary>
        public string Base { get; private set; }

		/// <summary>
		/// Internal reference to the full type name (e.g text/html)
		/// </summary>
		private string _type;

		public MimeType(string type)
		{
			if (!string.IsNullOrEmpty(type))
			{
				if (type.IndexOf(" ") >= 0)
					throw new ArgumentException("Spaces are not supported in MimeTypes");


				if (type.IndexOf("/") > 0)
				{
					var both = type.Split('/');
					this.Root = both[0];
					this.Content = both[1];
					if (this.Content.IndexOf("+") > 0)
					{
						both = this.Content.Split('+');
						this.Content = both[0];
						this.Base = both[1];
					}
				}
				else
				{
					this.Root = type;
					this.Content = null;
				}
				this._type = type;
			}
			else
			{
				this.Root = null;
				this.Base = null;
				this.Content = null;
			}
		}

		

		public override string ToString()
		{
			return this.IsValid ? this._type : string.Empty;
		}

		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var mime = obj as MimeType;
			if (null == mime)
				return false;
			else
				return this.Equals(mime);
		}

		public virtual bool Equals(MimeType other)
		{
			if (null == this._type)
				return null == other._type;
			else if (null == other._type)
				return false;
			else
				return this._type.Equals(other._type, StringComparison.Ordinal);
		}

		//
		// operator overrides
		//

		public static implicit operator MimeType(string value)
		{
			return MimeType.Parse(value);
		}

        /// <summary>
        /// Parses a string into a mime type instance. Supported formats are 'root', 'root/content', 'root/base+content', 'root/[base+]content; charset=NAME; [variant=NAME]'
        /// </summary>
        /// <param name="value">The string to parse</param>
        /// <returns>A parsed MimeType instance or null if the string is empty</returns>
        /// <exception cref="ArgumentException">Thrown if the format is not in a supported </exception>
        public static MimeType Parse(string value)
		{
			MimeType parsed;
			if (string.IsNullOrEmpty(value))
				return null;
			else if (TryParse(value, out parsed))
				return parsed;
			else
				throw new ArgumentException("Could not convert '" + value + "' to a mime type value. Supported formats are 'root', 'root/content', 'root/base+content', 'root/[base+]content; charset=NAME; [variant=NAME]'");
		}


		public static bool TryParse(string value, out MimeType mime)
		{
			mime = null;
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			else if (value.IndexOf(';') > 0)
			{
				
				var parts = value.Split(';');

				var type = parts[0].Trim();

				if (parts.Length > 1)
				{
					var next = parts[1].Trim();
					string charset = null;
					string variant = null;

					if (next.StartsWith("charset="))
					{
						charset = next.Substring("charset=".Length);
					}
					else if (next.StartsWith("variant="))
					{
						variant = next.Substring("variant=".Length);
					}

					if (parts.Length > 2)
					{
						next = parts[2].Trim();

                        if (next.StartsWith("charset="))
                        {
                            charset = next.Substring("charset=".Length).Trim();
                        }
                        else if (next.StartsWith("variant="))
                        {
                            variant = next.Substring("variant=".Length).Trim();
                        }
                    }

					mime = new QualifiedMimeType(type, charset, variant);
				}
			}
			else
			{
				mime = new MimeType(value);
			}
			return mime.IsValid;
		}


		public static bool IsParsableType(MimeType type)
		{
			string known = type.ToString();
			switch (known)
			{
				case "text/html":
				case "text/xhtml":
				case "application/xhtml+xml":
				case "text/xml":
				case "image/svg+xml":
					return true;
				default:
					return false;
			}
		}

		//
		// Default Known Types
		//

		public static readonly MimeType Html = "text/html";

		public static readonly MimeType Xml = "text/xml";

		public static readonly MimeType xHtml = "application/xhtml+xml";

		public static readonly MimeType xHtmlSimple = "text/xhtml";

		public static readonly MimeType Text = "text/plain";

		public static readonly MimeType Svg = "image/svg+xml";

		public static readonly MimeType Pdf = "application/pdf";

		public static readonly MimeType Empty = new MimeType("");

		public static readonly MimeType JpegImage = "image/jpeg";

		public static readonly MimeType PngImage = "image/png";

		public static readonly MimeType GifImage = "image/gif";

		public static readonly MimeType TiffImage = "image/tiff";

		public static readonly MimeType SvgImage = "image/svg+xml";


	}

    /// <summary>
    /// Internal class that supports this use of charset and variant options within a mimetype.
    /// </summary>
    public class QualifiedMimeType : MimeType
    {

        public string Charset { get; private set; }

        public string Variant { get; private set; }

        public QualifiedMimeType(string type, string charset)
            : this(type, charset, null)
        {
        }

        public QualifiedMimeType(string type, string charset, string variant)
            : base(type)
        {
            this.Charset = charset;
            this.Variant = variant;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Charset) && string.IsNullOrEmpty(this.Variant))
            {
                return base.ToString();
            }
            else if (string.IsNullOrEmpty(this.Variant))
            {
                return base.ToString() + "; charset=" + this.Charset;
            }
            else if (string.IsNullOrEmpty(this.Charset))
            {
                return base.ToString() + "; variant=" + this.Variant;
            }
            else
            {
                return base.ToString() + "; charset=" + this.Charset + "; variant=" + this.Variant;
            }
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(MimeType other)
        {
            if (string.IsNullOrEmpty(this.Charset) && string.IsNullOrEmpty(this.Variant))
                return base.Equals(other); //we dont have anything extra to a mimetype

            else if (base.Equals(other))
            {
                if (other is QualifiedMimeType qmt)
                {
                    if (string.Equals(this.Variant, qmt.Variant, StringComparison.Ordinal) == false)
                        return false;

                    if (string.Equals(this.Charset, qmt.Charset, StringComparison.Ordinal) == false)
                        return false;

                    // both variant and charset are the same as well.
                    return true;
                }
                else
                    return false;

            }
            else
                return false;
        }
    }
}

