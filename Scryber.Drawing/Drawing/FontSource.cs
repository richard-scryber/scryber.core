using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Scryber.Drawing
{

    /// <summary>
    /// Defines a specific location and type for a font, and allows a linked list of further sources.
    /// </summary>
    public class FontSource
    {
        private FontSource _next;

        private FontSourceType _type;
        private string _source;
        private FontSourceFormat _format;
        private bool _mapped = false;

        public FontSourceType Type
        {
            get { return _type; }
        }

        public string Source
        {
            get { return _source; }
        }

        public FontSourceFormat Format
        {
            get { return _format; }
        }

        public FontSource Next
        {
            get { return _next; }
            set { _next = value; }
        }

        /// <summary>
        /// Returns true, if this source path has been mapped (converted to a full path). Can only be set with the SetMappedSource() method.
        /// </summary>
        public bool Mapped
        {
            get { return _mapped; }
        }

        public FontSource()
        {
        }

        public FontSource(FontSourceType type, string source, FontSourceFormat format)
        {
            this._type = type;
            this._source = source;
            this._format = format;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            this.ToString(sb);
            return sb.ToString();
        }

        protected virtual void ToString(StringBuilder sb)
        {
            if (this.Type == FontSourceType.Local)
                sb.Append("local(");
            else
                sb.Append("url(");
            sb.Append(this.Source);
            sb.Append(") format(");
            sb.Append(this.Format.ToString().ToLower());
            sb.Append(")");
            if (null != this.Next)
            {
                sb.Append(", ");
                this.Next.ToString(sb);
            }
        }

        public void SetMappedSource(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            this._source = path;
            this._mapped = true;
        }

        public static bool TryParse(string value, out FontSource parsed)
        {
            parsed = null;
            if (null == value)
                return false;

            if (value.IndexOf(',') > 0)
            {
                var all = value.Split(',');
                FontSource curr = null;
                FontSource next;
                foreach (var one in all)
                {

                    if (!string.IsNullOrEmpty(one) && TryParseOneValue(one.Trim(), out next))
                    {
                        if (null == parsed)
                            parsed = next;

                        if (null != curr)
                            curr.Next = next;

                        curr = next;
                    }

                }

                return parsed != null;
            }
            else
                return TryParseOneValue(value, out parsed);
        }


        public static bool TryParseOneValue(string value, out FontSource parsed)
        {
            parsed = null;

            if(null == value)
                return false;

            // Initially get teh url or local options

            var open = value.IndexOf("(");
            if (open <= 0)
                return false;
            var close = value.IndexOf(')', open);
            if (close <= open)
                return false;

            var typeS = value.Substring(0, open);

            if (value.Contains("base64") && value.Contains("data:"))
                typeS = "base64";

            FontSourceType type;
            if (!Enum.TryParse<FontSourceType>(typeS, true, out type))
                return false;

            // Now extract the source value that was in the local or url

            var src = value.Substring(open + 1, close - (open + 1));

            src = src.Trim();

            if (src.StartsWith("'"))
            {
                if (!src.EndsWith("'"))
                    return false;

                src = src.Substring(1, src.Length - 2);
            }
            else if (src.StartsWith("\""))
            {
                if (!src.EndsWith("\""))
                    return false;

                src = src.Substring(1, src.Length - 2);
            }

            // Now parse the format(woff) etc.

            FontSourceFormat format = FontSourceFormat.Default;
            if (value.Length > (close + 1))
                value = value.Substring(close + 1).Trim();
            else
                value = string.Empty;

            if (value.Length > 0 && value.StartsWith("format("))
            {
                open = "format(".Length;
                close = value.IndexOf(')', open);
                if (close > open)
                {
                    var formatS = value.Substring(open, close - open).Trim();

                    if (formatS.StartsWith("'"))
                    {
                        if (!formatS.EndsWith("'"))
                            return false;

                        formatS = formatS.Substring(1, formatS.Length - 2);
                    }
                    else if (formatS.StartsWith("\""))
                    {
                        if (!formatS.EndsWith("\""))
                            return false;

                        formatS = formatS.Substring(1, formatS.Length - 2);
                    }
                    if (formatS.IndexOf('-') > 0)
                        formatS = formatS.Replace("-", "");

                    if (!Enum.TryParse<FontSourceFormat>(formatS, true, out format))
                        format = FontSourceFormat.Default; //be nice
                }
                else
                    return false; //deffo not right
            }


            parsed = new FontSource(type, src, format);

            return true;

        }
    }

    
}
