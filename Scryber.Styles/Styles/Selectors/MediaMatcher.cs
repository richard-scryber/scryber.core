using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Selectors
{
    [PDFParsableValue]
    public class MediaMatcher
    {
        public bool Not { get; set; }

        public string Type { get; set; }

        public string Features { get; set; }


        public virtual bool IsMatchedTo(OutputFormat format)
        {
            if (format == OutputFormat.PDF)
                return this.IsPrintMedia();
            else
                return false;
        }

        public bool IsPrintMedia()
        {
            bool print = false;

            if (string.IsNullOrEmpty(this.Type) && string.IsNullOrEmpty(this.Features))
                print = true;
            if (this.Type == "all" || this.Type.StartsWith("all "))
                print = true;
            else if (this.Type == "print" || this.Type.StartsWith("print "))
                print = true;
            else
                print = false;

            if (this.Not)
                print = !print;

            return print;
        }

        public static MediaMatcher Parse(string media)
        {
            MediaMatcher matcher = new MediaMatcher();
            if (string.IsNullOrEmpty(media))
                return matcher;

            media = media.ToLower().Trim();

            if (media.StartsWith("not "))
            {
                matcher.Not = true;
                media = media.Substring(3).Trim();
            }
            if (media.StartsWith("only "))
                media = media.Substring(4).Trim(); //Just ignore only

            if (media.StartsWith("("))
            {
                matcher.Type = string.Empty;
                matcher.Features = media;
            }
            else
            {
                var split = media.IndexOf(' ');

                if (split > 0)
                {

                    matcher.Type = media.Substring(0, split).Trim();
                    matcher.Features = media.Substring(split).Trim();
                }
                else
                    matcher.Type = media;
            }

            return matcher;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.Not)
                sb.Append("not ");

            if (!string.IsNullOrEmpty(this.Type))
            {
                sb.Append(this.Type);
                if (!string.IsNullOrEmpty(this.Features))
                {
                    sb.Append(" ");
                    sb.Append(this.Features);
                }
            }
            else if (!string.IsNullOrEmpty(this.Features))
            {
                sb.Append(this.Features);
            }

            return sb.ToString();
        }
    }
}
