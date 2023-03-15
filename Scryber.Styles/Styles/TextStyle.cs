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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Scryber;
using Scryber.Drawing;

namespace Scryber.Styles
{
    [PDFParsableComponent("Text")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TextStyle : StyleItemBase
    {

        #region public PDFUnit FirstLineInset {get;set;} + RemoveFirstLineInset()

        [PDFAttribute("first-indent")]
        public Unit FirstLineInset
        {
            get
            {
                Unit f;
                if (this.TryGetValue(StyleKeys.TextFirstLineIndentKey, out f))
                    return f;
                else
                return Unit.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.TextFirstLineIndentKey, value);
            }
        }

        public void RemoveFirstLineInset()
        {
            this.RemoveValue(StyleKeys.TextFirstLineIndentKey);
        }

        #endregion

        #region public PDFUnit WordSpacing {get;set} + RemoveWordSpacing()

        [PDFAttribute("word-spacing")]
        public Unit WordSpacing
        {
            get
            {
                Unit u;
                if (this.TryGetValue(StyleKeys.TextWordSpacingKey, out u))
                    return u;
                else
                    return Unit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.TextWordSpacingKey, value);
            }
        }

        public void RemoveWordSpacing()
        {
            this.RemoveValue(StyleKeys.TextWordSpacingKey);
        }

        #endregion

        #region public PDFUnit CharacterSpacing {get;set} + RemoveCharacterSpacing()

        [PDFAttribute("char-spacing")]
        public Unit CharacterSpacing
        {
            get
            {
                Unit u;
                if (this.TryGetValue(StyleKeys.TextCharSpacingKey, out u))
                    return u;
                else
                    return Unit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.TextCharSpacingKey, value);
            }
        }

        public void RemoveCharacterSpacing()
        {
            this.RemoveValue(StyleKeys.TextCharSpacingKey);
        }

        #endregion

        #region public double HorizontalScaling {get;set;} + RemoveHorizontalScaling()

        [PDFAttribute("h-scale")]
        public double HorizontalScaling
        {
            get
            {
                double s;
                if (this.TryGetValue(StyleKeys.TextHorizontalScaling, out s))
                    return s;
                else
                    return 1.0;
            }
            set
            {
                this.SetValue(StyleKeys.TextHorizontalScaling, value);
            }
        }

        public void RemoveHoziontalScaling()
        {
            this.RemoveValue(StyleKeys.TextHorizontalScaling);
        }

        #endregion


        #region public double HorizontalScaling {get;set;} + RemoveHorizontalScaling()

        [PDFAttribute("text-direction")]
        public TextDirection TextDirection
        {
            get
            {
                TextDirection s;
                if (this.TryGetValue(StyleKeys.TextDirectionKey, out s))
                    return s;
                else
                    return Scryber.TextDirection.LTR;
            }
            set
            {
                this.SetValue(StyleKeys.TextDirectionKey, value);
            }
        }

        public void RemoveTextDirection()
        {
            this.RemoveValue(StyleKeys.TextDirectionKey);
        }

        #endregion

        #region public WordWrap WrapText {get;set;} + RemoveWrapText()

        [PDFAttribute("wrap")]
        public Scryber.Text.WordWrap WrapText
        {
            get
            {
                Scryber.Text.WordWrap f;
                if (this.TryGetValue(StyleKeys.TextWordWrapKey, out f))
                    return f;
                else
                    return Scryber.Text.WordWrap.Auto;
            }
            set
            {
                this.SetValue(StyleKeys.TextWordWrapKey, value);
            }
        }

        public void RemoveWrapText()
        {
            this.RemoveValue(StyleKeys.TextWordWrapKey);
        }

        #endregion

        #region public WordHyphenation Hyphenation {get;set;} + RemoveHypenation()

        public Scryber.Text.WordHyphenation Hyphenation
        {
            get
            {
                Scryber.Text.WordHyphenation h;
                if (this.TryGetValue(StyleKeys.TextWordHyphenation, out h))
                    return h;
                else
                    return Scryber.Text.WordHyphenation.None;
            }
            set
            {
                this.SetValue(StyleKeys.TextWordHyphenation, value);
            }
        }

        public void RemoveHypenation()
        {
            this.RemoveValue(StyleKeys.TextWordHyphenation);
        }

        #endregion

        #region public TextDecoration Decoration {get;set;} + RemoveDecoration()

        /// <summary>
        /// Gets or sets the text decoration of the rendered characters.
        /// </summary>
        [PDFAttribute("decoration")]
        public Scryber.Text.TextDecoration Decoration
        {
            get
            {
                Scryber.Text.TextDecoration decor;
                if (this.TryGetValue(StyleKeys.TextDecorationKey, out decor))
                    return decor;
                else
                    return Scryber.Text.TextDecoration.None;
            }
            set
            {
                this.SetValue(StyleKeys.TextDecorationKey, value);
            }
        }

        /// <summary>
        /// Clears any set value from this style
        /// </summary>
        public void RemoveDecoration()
        {
            this.RemoveValue(StyleKeys.TextDecorationKey);
        }

        #endregion

        #region public PDFUnit Leading {get;set;} + RemoveLeadingText()

        [PDFAttribute("leading")]
        public Unit Leading
        {
            get
            {
                Unit f;
                if (this.TryGetValue(StyleKeys.TextLeadingKey, out f))
                    return f;
                else
                    return Unit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.TextLeadingKey, value);
            }
        }

        public void RemoveLeading()
        {
            this.RemoveValue(StyleKeys.TextLeadingKey);
        }

        #endregion

        #region public bool PreserveWhitespace {get;set;} + RemoveWhiteSpace()

        /// <summary>
        /// If true then all whitespace within the text component will be preserved.
        /// </summary>
        [PDFAttribute("whitespace-significant")]
        public bool PreserveWhitespace
        {
            get
            {
                bool value;
                if (this.TryGetValue(StyleKeys.TextWhitespaceKey, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.TextWhitespaceKey, value);
            }
        }

        public void RemoveWhitespace()
        {
            this.RemoveValue(StyleKeys.TextWhitespaceKey);
        }

        #endregion

        /// <summary>
        /// If false then the measurement of any font and text rendering will be from the top left.
        /// This is the default option, but if the text should be rendered from the baseline of the font
        /// characters, set it to true.
        /// </summary>
        public bool PositionFromBaseline
        {
            get
            {
                bool value;
                if (this.TryGetValue(StyleKeys.TextPositionFromBaseline, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.TextPositionFromBaseline, value);
            }
        }

        #region public string DateFormat {get;set;} + RemoveDateText()

        /// <summary>
        /// Gets or sets the format string which stipulates how to convert date values to a string value and display
        /// </summary>
        [PDFAttribute("date-format")]
        public string DateFormat
        {
            get
            {
                string f;
                if (this.TryGetValue(StyleKeys.TextDateFormatKey,out f))
                    return f;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.TextDateFormatKey, value);
            }
        }

        public void RemoveDateFormat()
        {
            this.RemoveValue(StyleKeys.TextDateFormatKey);
        }

        #endregion

        #region public string NumberFormat {get;set;} + RemoveNumberText()

        /// <summary>
        /// Gets or sets the format string which stipulates how to convert numeric values to a string value and display
        /// </summary>
        [PDFAttribute("number-format")]
        public string NumberFormat
        {
            get
            {
                string f;
                if (this.TryGetValue(StyleKeys.TextNumberFormatKey, out f))
                    return f;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.TextNumberFormatKey, value);
            }
        }

        public void RemoveNumberFormat()
        {
            this.RemoveValue(StyleKeys.TextNumberFormatKey);
        }

        #endregion

        #region

        [PDFAttribute("content")]
        public ContentDescriptor Content
        {
            get
            {
                ContentDescriptor desc = null;
                if (this.TryGetValue(StyleKeys.ContentTextKey, out desc))
                    return desc;
                else
                    return null;
            }
            set
            {
                this.SetValue(StyleKeys.ContentTextKey, value);
            }
        }

        public void RemoveContent()
        {
            this.RemoveValue(StyleKeys.ContentTextKey);
        }


        #endregion

        //
        // .ctor
        //

        #region public PDFTextStyle()

        public TextStyle()
            : base(StyleKeys.TextItemKey)
        {
        }

        #endregion


        
    }
}
