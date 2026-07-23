using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("font")]
    public class HTMLFontSpan : Scryber.Components.Span
    {

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        
        [PDFAttribute("color")]
        public override Color FillColor
        {
            get => base.FillColor;
            set => base.FillColor = value;
        }

        [PDFAttribute("face")]
        public override FontSelector FontFamily
        {
            get => base.FontFamily;
            set => base.FontFamily = value;
        }

        [PDFAttribute("size")]
        public string LegacyFontSize
        {
            get
            {
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.FontSizeKey, out StyleValue<Unit> fontSize))
                    return GetFontSizeForCSS(fontSize.Value(this.Style));
                else
                    return string.Empty;
            }
            set
            { 
                var points = GetCSSSizeForFontSize(value);
                this.Style.SetValue(StyleKeys.FontSizeKey, new Unit(points, PageUnits.Points));
            }
        }

        /// <summary>
        /// Global Html hidden attribute used with xhtml as hidden='hidden'
        /// </summary>
        [PDFAttribute("hidden")]
        public string Hidden
        {
            get
            {
                if (this.Visible)
                    return string.Empty;
                else
                    return "hidden";
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value != "hidden")
                    this.Visible = true;
                else
                    this.Visible = false;
            }
        }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }


        public HTMLFontSpan()
            : this(HTMLObjectTypes.FontSpan)
        {
        }

        protected HTMLFontSpan(ObjectType type): base(type)
        { }

        private static readonly double[] _fontPointSizeMapping = { 12, 7.5, 10.0, 12, 13.5, 18, 24, 36 };

        private static double GetCSSSizeForFontSize(string value)
        {
            if (string.IsNullOrEmpty(value))
                return _fontPointSizeMapping[0];
            else if (value.StartsWith('-'))
            {
                if (int.TryParse(value.Substring(1), out var index))
                {
                    index = 3 -  index;
                    if(index <= 0)
                        index = 1;
                    else if(index >= _fontPointSizeMapping.Length)
                        index = _fontPointSizeMapping.Length - 1;
                    return _fontPointSizeMapping[index];
                }
                return _fontPointSizeMapping[0];
            }
            else if (value.StartsWith('+'))
            {
                if (int.TryParse(value.Substring(1), out var index))
                {
                    index = 3 + index;
                    if(index <= 0)
                        index = 1;
                    else if(index >= _fontPointSizeMapping.Length)
                        index = _fontPointSizeMapping.Length - 1;
                    return _fontPointSizeMapping[index];
                }
                return _fontPointSizeMapping[0];
            }
            else if (int.TryParse(value, out var index))
            {
                // Values beyond the legacy 1-7 scale are treated as an explicit point size rather than
                // being clamped into the mapping table - e.g. content pasted from Apple Pages emits
                // font size="30" meaning 30pt literally, not the legacy size 7 (36pt).
                if (index > 7)
                    return index;

                if(index <= 0)
                    index = 1;

                return _fontPointSizeMapping[index];
            }
            else
            {
                return _fontPointSizeMapping[0];
            }

        }

        private static string GetFontSizeForCSS(Unit fontSize)
        {
            var points = fontSize.IsRelative ? 16.0 : fontSize.PointsValue;
            if (points <= 7.6)
                return "1";
            else if(points <= 10.1)
                return "2";
            else if(points <= 12.1)
                return "3";
            else if(points <= 14.1)
                return "4";
            else if(points <= 18.1)
                return "5";
            else if (points <= 24.1)
                return "6";
            else
                return "7";
        }
    }

}
