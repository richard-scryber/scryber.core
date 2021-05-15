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
using System.Drawing;
using Scryber.Configuration;
using System.ComponentModel;
using Scryber.Resources;

namespace Scryber.Drawing
{
    /// <summary>
    /// Defines a font family, style attributes and size for outputting text onto a pdf document
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFFont : PDFGraphicsAdapter
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


        private PDFFontSelector _selector;

        public PDFFontSelector Selector
        {
            get { return this._selector; }
            set 
            { 
                this._selector = value;
                this.ClearResourceFont();
            }
        }

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
                this.Selector = PDFFontSelector.Parse(value);
                this.ClearResourceFont();
            }
        }

        #endregion

        #region public string FullName {get;}

        /// <summary>
        /// Gets the FullName for the font in the format 'FamilyName [,FontStyle]'.
        /// </summary>
        public string FullName
        {
            get
            {
                return GetFullName(this.FamilyName, this.FontStyle);
            }
        }

        #endregion

        #region public bool IsStandard {get;} + private static bool DerriveIsStandard(string value)

        

        /// <summary>
        /// Gets the flag to identify if the font is one of the standard PDF fonts.
        /// </summary>
        public bool IsStandard
        {
            get { return IsStandardFontFamily(this.FamilyName); }
        }

        private static Dictionary<string, bool> _stdFonts = new Dictionary<string, bool>()
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

        private PDFUnit _size;

        /// <summary>
        /// Gets or sets the current em size of the font.
        /// </summary>
        public PDFUnit Size
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


        #region public PFFontMetrics FontMetrics {get;}

        /// <summary>
        /// private ivar to cache the Font Metrics.
        /// </summary>
        private PDFFontMetrics _cachedmetrics = null;

        /// <summary>
        /// Gets the Metrics (ascent, descent etc.) associated with this this font.
        /// </summary>
        public PDFFontMetrics FontMetrics
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

        //
        // .ctor
        //

        #region .ctor(string family, PFUnit size, FontStyle style, bool isStandard) + 7 overloads

        /// <summary>
        /// Creates a new PDFFont based upon the existing font and the provided size
        /// </summary>
        /// <param name="basefont">The existing PDFFont</param>
        /// <param name="size">The new font size</param>
        public PDFFont(PDFFont basefont, PDFUnit size)
            : this(basefont.Selector, size, basefont.FontStyle)
        {
            this._familyName = basefont._familyName;
        }

        /// <summary>
        /// Creates a new PDFFont based upon the existing font and the provided FontStyle
        /// </summary>
        /// <param name="basefont">The existing PDFFont</param>
        /// <param name="style">The new FontStyle</param>
        public PDFFont(PDFFont basefont, FontStyle style)
            : this(basefont.Selector, basefont.Size, style)
        {
            this._familyName = basefont._familyName;
        }

        /// <summary>
        /// Creates a new PDFFont based upon the existing font and the provided size adn font style
        /// </summary>
        /// <param name="basefont">The existing PDFFont</param>
        /// <param name="size">the new font size</param>
        /// <param name="style">the new font style</param>
        public PDFFont(PDFFont basefont, PDFUnit size, FontStyle style)
            : this(basefont.Selector, size, style)
        {
            this._familyName = basefont._familyName;
        }


        /// <summary>
        /// Creates a new regular standard PDFFont with specified size
        /// </summary>
        /// <param name="font">One of the PDF Standard fonts that does not need to be included in the PDF file</param>
        /// <param name="size">The em size of the font</param>
        public PDFFont(StandardFont font, PDFUnit size)
            : this(font.ToString(), size, FontStyle.Regular)
        {
        }

        /// <summary>
        /// Creates a new standard PDFFont with the specified size and style
        /// </summary>
        /// <param name="font">One of the PDF Standard fonts that does not need to be included in the PDF file</param>
        /// <param name="size">The em size of the font</param>
        /// <param name="style">The new font style</param>
        public PDFFont(StandardFont font, PDFUnit size, FontStyle style)
            : this(font.ToString(), size, style)
        {
        }

        /// <summary>
        /// Creates a new regular PDFFont with specified family name and size
        /// </summary>
        /// <param name="family">The family name of the font (e.g. Arial, Helvetica, Courier)</param>
        /// <param name="size">The em size of the font</param>
        public PDFFont(string family, PDFUnit size)
            : this(family, size, FontStyle.Regular)
        {
        }

        /// <summary>
        /// Creates a new PDFFont with specified family name and size
        /// </summary>
        /// <param name="family">The family name of the font (e.g. Arial, Helvetica, Courier)</param>
        /// <param name="size">The em size of the font</param>
        /// <param name="style">The new font style</param>
        public PDFFont(string family, PDFUnit size, FontStyle style)
            : this(PDFFontSelector.Parse(family), size, style)
        {
        }

        

        /// <summary>
        /// Private constructor accepting the full set of parameters in order to construct a full PDFFont
        /// </summary>
        /// <param name="family">The fonts family name</param>
        /// <param name="size">The fonts unit size</param>
        /// <param name="style">The fonts style</param>
        /// <param name="isStd">Flag to identify if this is one of the PDF standard fonts.</param>
        public PDFFont(PDFFontSelector selector, PDFUnit size, FontStyle style)
        {
            this._selector = selector;
            this._size = size;
            this._style = style;
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
            return this.Equals(obj as PDFFont);
        }

        /// <summary>
        /// Returns true if the font matches this font
        /// </summary>
        /// <param name="font">The font to compare</param>
        /// <returns>Returns true if the font matches this instance</returns>
        public bool Equals(PDFFont font)
        {
            return Equals(this, font);
        }

        /// <summary>
        /// Comares the two font instances and returns true if they are equal
        /// </summary>
        /// <param name="one">The first to compare</param>
        /// <param name="two">The other instance to compare</param>
        /// <returns>True if the instances are comparably equal</returns>
        public static bool Equals(PDFFont one, PDFFont two)
        {
            if (null == one)
                return null == two;
            else if (null == two)
                return false;
            else
                return (one.FamilyName == two.FamilyName) && (one.FontStyle == two.FontStyle) && (one.Size == two.Size);
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
            this._familyName = null;
            this._cachedmetrics = null;
        }

        public virtual void SetResourceFont(string name, PDFFontDefinition definition)
        {
            this._familyName = name;
            this._cachedmetrics = definition.GetFontMetrics(this.Size);
            //var style = this.GetDrawingStyle();
            //var sys = PDFFontFactory.GetSystemFont(this._familyName, this.GetDrawingStyle(), (float)this.Size.PointsValue);
            //int line = sys.FontFamily.GetLineSpacing(style);
        }

        public System.Drawing.FontStyle GetDrawingStyle()
        {
            return GetDrawingStyle(this.FontStyle);
        }

        /// <summary>
        /// Converts the PDFX.FontStyle to a System.Drawing.FontStyle
        /// </summary>
        /// <param name="fontStyle">The PDFX.FontStyle</param>
        /// <returns>A comparable System.Drawing.FontStyle</returns>
        public static System.Drawing.FontStyle GetDrawingStyle(FontStyle fontStyle)
        {
            System.Drawing.FontStyle fs = System.Drawing.FontStyle.Regular;
            if ((fontStyle & FontStyle.Bold) > 0)
                fs |= System.Drawing.FontStyle.Bold;

            if ((fontStyle & FontStyle.Italic) > 0)
                fs |= System.Drawing.FontStyle.Italic;

            return fs;
        }

        #endregion

        #region protected PDFFontMetrics GetFontMetrics()

        /// <summary>
        /// Gets the associated font metrics for the current font
        /// </summary>
        /// <returns>The font metrics for this font</returns>
        protected PDFFontMetrics GetFontMetrics()
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

        public override bool SetUpGraphics(PDFGraphics graphics, PDFRect bounds)
        {
            graphics.SetCurrentFont(this);
            return true;
        }

        public override void ReleaseGraphics(PDFGraphics g, PDFRect bounds)
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
        public static string GetFullName(string font, FontStyle style)
        {
            bool bold = (style & FontStyle.Bold) > 0;
            bool ital = (style & FontStyle.Italic) > 0;
            return GetFullName(font, bold, ital);
        }

        #endregion



        #region public static string GetFullName(PDFFontSelector family, bool bold, bool italic)


        public static string GetFullName(string familyName, bool bold, bool italic)
        {
            string fn;
            if (string.IsNullOrEmpty(familyName))
                fn = "UNKNOWN_FONT";
            else
                fn = familyName;

            if (bold)
                fn += ",Bold";
            if (italic)
            {
                if (!bold)
                    fn += ",";
                else
                    fn += " ";
                fn += "Italic";
            }
            return fn;
        }

        #endregion


    }
}
