/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{
    /// <summary>
    /// Defines a font family, style attributes and size for outputting text onto a pdf document
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Font : IPDFGraphicsAdapter
    {

        /// <summary>
        /// The default font family name
        /// </summary>
        public static readonly string DefaultFontFamily = "Helvetica";
        
        /// <summary>
        /// The default font em size.
        /// </summary>
        public static readonly double DefaultFontSize = 12.0;

        //
        // properties
        //

        #region public PDFFontSelector Selector {get; set;}

        private FontSelector _selector;

        public FontSelector Selector
        {
            get { return this._selector; }
            set 
            { 
                this._selector = value;
                this.ClearResourceFont();
            }
        }

        #endregion

        #region public string FamilyName {get; set;}

        private string _familyName;
        
        /// <summary>
        /// Gets or sets the FamilyName for this font (e.g. Times, Helvetica, Courier, Arial)
        /// </summary>
        public string FamilyName
        {
            get 
            {
                if (string.IsNullOrEmpty(this._familyName) == false)
                    return _familyName;

                //Ensure we have loaded the system font
                //This will also set the familyName
                if (null != this.Selector)
                    return this.Selector.FamilyName;
                else
                    return string.Empty;
            }
            set 
            {
                this.Selector = FontSelector.Parse(value);
                this.ClearResourceFont();
            }
        }

        #endregion

        #region public string FullName {get;}

        private string _fullName;

        /// <summary>
        /// Gets the FullName for the font in the format 'FamilyName [,FontStyle]'.
        /// </summary>
        public string FullName
        {
            get
            {
                if(null == _fullName)
                    _fullName = GetFullName(this.FamilyName, this.FontWeight, this.FontStyle);
                return _fullName;
            }
        }

        #endregion

        #region public bool IsStandard {get;} + private static bool DerriveIsStandard(string value)
        

        /// <summary>
        /// Gets the flag to identify if the font is one of the standard PDF fonts.
        /// </summary>
        public bool IsStandard
        {
            get { return IsStandardFontFamily(this.FamilyName) && IsStandardStyle(this.FontWeight, this.FontStyle); }
        }

        private static Dictionary<string, bool> _stdFonts = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)
        {
            {"Helvetica", true },
            {"Times", true },
            {"Symbol", true },
            {"Courier", true },
            {"Zapf Dingbats", true },
            {"Serif", true },
            {"Sans-Serif", true },
            {"Monospace", true }
        };

        private static bool IsStandardStyle(int weight, FontStyle style)
        {
            if(style == FontStyle.Regular || style == FontStyle.Italic)
            {
                return weight == FontWeights.Regular || weight == FontWeights.Bold;
            }
            //Anything else is not standard
            return false;
        }

        /// <summary>
        /// Returns the flag to identify if this is a Standard PDF font (does not need to be included in PDF document.
        /// </summary>
        /// <param name="family">The FamilyName of the font</param>
        /// <returns>Returns true if the family name is one of the defined standard values.</returns>
        public static bool IsStandardFontFamily(string family)
        {
            bool found;
            if (_stdFonts.TryGetValue(family, out found))
                return found;
            else
                return false;
        }

        #endregion

        #region public PDFUnit Size {get;set;}

        private Unit _size;

        /// <summary>
        /// Gets or sets the current em size of the font.
        /// </summary>
        public Unit Size
        {
            get { return _size; }
            set 
            {
                _size = value;
                this.ClearResourceFont();
            }
        }

        #endregion

        #region public FontStyle {get;set;}

        private FontStyle _style;

        /// <summary>
        /// Gets or sets the FontStyle for the font
        /// </summary>
        public FontStyle FontStyle
        {
            get { return _style; }
            set 
            {
                _style = value;
                this.ClearResourceFont();
            }
        }

        #endregion

        #region public int FontWeight {get;set;}

        private int _fontWeight = FontWeights.Regular;

        public int FontWeight
        {
            get {
                return _fontWeight;
            }
            set
            {
                _fontWeight = value;
                this.ClearResourceFont();
            }
        }

        #endregion

        #region public PFFontMetrics FontMetrics {get;}

        /// <summary>
        /// private ivar to cache the Font Metrics.
        /// </summary>
        private FontMetrics _cachedmetrics = null;

        /// <summary>
        /// Gets the Metrics (ascent, descent etc.) associated with this this font.
        /// </summary>
        public FontMetrics FontMetrics
        {
            get
            {
                return this._cachedmetrics;
            }
            set
            {
                this._cachedmetrics = value;
            }
        }

        #endregion

        private PDFFontResource _cachedResource;

        public PDFFontResource Resource
        {
            get { return _cachedResource; }
            private set { _cachedResource = value; }

        }
        
        //
        // .ctor
        //

        #region .ctor(string family, PFUnit size, FontStyle style, bool isStandard) + 7 overloads

        /// <summary>
        /// Creates a new PDFFont based upon the existing font and the provided size
        /// </summary>
        /// <param name="basefont">The existing PDFFont</param>
        /// <param name="size">The new font size</param>
        public Font(Font basefont, Unit size)
            : this(basefont.Selector, size, basefont.FontWeight, basefont.FontStyle)
        {
        }

        /// <summary>
        /// Creates a new PDFFont based upon the existing font and the provided FontStyle
        /// </summary>
        /// <param name="basefont">The existing PDFFont</param>
        /// <param name="style">The new FontStyle</param>
        public Font(Font basefont, int weight, FontStyle style)
            : this(basefont.Selector, basefont.Size, weight, style)
        {
        }

        /// <summary>
        /// Creates a new PDFFont based upon the existing font and the provided size adn font style
        /// </summary>
        /// <param name="basefont">The existing PDFFont</param>
        /// <param name="size">the new font size</param>
        /// <param name="style">the new font style</param>
        public Font(Font basefont, Unit size, int weight, FontStyle style)
            : this(basefont.Selector, size, weight, style)
        {
        }


        /// <summary>
        /// Creates a new regular standard PDFFont with specified size
        /// </summary>
        /// <param name="font">One of the PDF Standard fonts that does not need to be included in the PDF file</param>
        /// <param name="size">The em size of the font</param>
        public Font(StandardFont font, Unit size)
            : this(font.ToString(), size, FontWeights.Regular, FontStyle.Regular)
        {
        }

        /// <summary>
        /// Creates a new standard PDFFont with the specified size and style
        /// </summary>
        /// <param name="font">One of the PDF Standard fonts that does not need to be included in the PDF file</param>
        /// <param name="size">The em size of the font</param>
        /// <param name="style">The new font style</param>
        public Font(StandardFont font, Unit size, int weight, FontStyle style)
            : this(font.ToString(), size, weight, style)
        {
        }

        /// <summary>
        /// Creates a new regular PDFFont with specified family name and size
        /// </summary>
        /// <param name="family">The family name of the font (e.g. Arial, Helvetica, Courier)</param>
        /// <param name="size">The em size of the font</param>
        public Font(string family, Unit size)
            : this(family, size, FontWeights.Regular, FontStyle.Regular)
        {
        }

        /// <summary>
        /// Creates a new PDFFont with specified family name and size
        /// </summary>
        /// <param name="family">The family name of the font (e.g. Arial, Helvetica, Courier)</param>
        /// <param name="size">The em size of the font</param>
        /// <param name="style">The new font style</param>
        public Font(string family, Unit size, int weight, FontStyle style)
            : this(FontSelector.Parse(family), size, weight, style)
        {
        }

        

        /// <summary>
        /// Private constructor accepting the full set of parameters in order to construct a full PDFFont
        /// </summary>
        /// <param name="family">The fonts family name</param>
        /// <param name="size">The fonts unit size</param>
        /// <param name="style">The fonts style</param>
        /// <param name="isStd">Flag to identify if this is one of the PDF standard fonts.</param>
        public Font(FontSelector selector, Unit size, int weight, FontStyle style)
        {
            this._selector = selector ?? throw new ArgumentNullException("The font selector for a PDFFont cannot be null");
            this._size = size;
            this._style = style;
            this._fontWeight = weight;
            this._familyName = selector.FamilyName;
            this.ClearResourceFont();
        }

        #endregion


        //
        //methods
        //

        #region public bool Equals(object) + public bool Equals(PDFFont) + public static bool Equals(PDFFont, PFFont)

        /// <summary>
        /// Returns true if the object is a font and it matches this font
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>Returns true if the object is a font and it matches this instance</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Font);
        }

        /// <summary>
        /// Returns true if the font matches this font
        /// </summary>
        /// <param name="font">The font to compare</param>
        /// <returns>Returns true if the font matches this instance</returns>
        public bool Equals(Font font)
        {
            return Equals(this, font);
        }

        /// <summary>
        /// Comares the two font instances and returns true if they are equal
        /// </summary>
        /// <param name="one">The first to compare</param>
        /// <param name="two">The other instance to compare</param>
        /// <returns>True if the instances are comparably equal</returns>
        public static bool Equals(Font one, Font two)
        {
            if (null == one)
                return null == two;
            else if (null == two)
                return false;
            else
                return (one.FontStyle == two.FontStyle) && (one.FontWeight == two.FontWeight) && (one.Size == two.Size) && one.Selector.Equals(two.Selector);
        }

        #endregion

        #region public override int GetHashCode()

        public override int GetHashCode()
        {
            return (this.FullName + ":" + this.Size.PointsValue.ToString()).GetHashCode();
        }

        #endregion

        #region ClearResourceFont(), SetResourceFont()


        
        /// <summary>
        /// Clears any cached information about this fonts system equivalent.
        /// </summary>
        /// <remarks>Inheritors should override this method if they store system information, 
        /// and call this method if any properties would invalidate the cached font.</remarks>
        public virtual void ClearResourceFont()
        {
            this._fullName = null;
            this._familyName = null;
            this._cachedResource = null;
            this._cachedmetrics = null;
        }

        public virtual void SetResourceFont(string name, PDFFontResource resource)
        {
            this._familyName = name;
            this._cachedResource = resource;
            this._cachedmetrics = resource.Definition.GetFontMetrics(this.Size);
        }


        #endregion

        #region protected FontMetrics GetFontMetrics()

        /// <summary>
        /// Gets the associated font metrics for the current font
        /// </summary>
        /// <returns>The font metrics for this font</returns>
        protected FontMetrics GetFontMetrics()
        {
            return this._cachedmetrics;
            /*
            double factor = (double)f.SizeInPoints / ff.GetEmHeight(f.Style);
            double ascent = ff.GetCellAscent(f.Style) * factor;
            double descent = ff.GetCellDescent(f.Style) * factor;
            double lineheight = ff.GetLineSpacing(f.Style) * factor;
            return new PDFFontMetrics(f.SizeInPoints, ascent, descent, lineheight);
            */
        }

        #endregion

        public bool SetUpGraphics(PDFGraphics graphics, Rect bounds)
        {
            graphics.SetCurrentFont(this);
            return true;
        }

        public void ReleaseGraphics(PDFGraphics g, Rect bounds)
        {
            
        }


        //
        // static methods
        //

        #region public static string GetFullName(string family, FontStyle style)
        
        /// <summary>
        /// Gets a full name for a font based upon the family name and its font style
        /// </summary>
        /// <param name="family"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static string GetFullName(string familyName, int weight, FontStyle style)
        {
            string fn;
            if (string.IsNullOrEmpty(familyName))
                fn = "UNKNOWN_FONT";
            else
                fn = familyName;

            if (weight == FontWeights.Bold)
            {
                //Backwards compatibility for font names - Name, Bold Italic
                fn += ",Bold";

                if (style == FontStyle.Italic)
                {
                    fn += " Italic";
                }
                else if (style == FontStyle.Oblique)
                {
                    fn += " Oblique";
                }
            }
            else if (weight == FontWeights.Regular)
            {
                if (style == FontStyle.Italic)
                {
                    fn += ",Italic";
                }
                else if (style == FontStyle.Oblique)
                {
                    fn += ",Oblique";
                }
            }
            else if (style == FontStyle.Regular)
            {
                fn += "," + weight;
            }
            else
                fn += "," + weight + " " + style;

            return fn;
            
        }

        #endregion



        #region public static string GetFullName(PDFFontSelector family, bool bold, bool italic)


        public static string GetFullName(string familyName, bool bold, bool italic)
        {
            return GetFullName(familyName, bold ? FontWeights.Bold : FontWeights.Regular, italic ? FontStyle.Italic : FontStyle.Regular);
        }

        #endregion


        public override string ToString()
        {
            return this.FullName + " @" + this.Size.ToString();
        }
    }
}
