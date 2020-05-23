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
    public class PDFTextStyle : PDFStyleItemBase
    {

        #region public PDFUnit FirstLineInset {get;set;} + RemoveFirstLineInset()

        [PDFAttribute("first-indent")]
        public PDFUnit FirstLineInset
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(PDFStyleKeys.TextFirstLineIndentKey, out f))
                    return f;
                else
                return PDFUnit.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextFirstLineIndentKey, value);
            }
        }

        public void RemoveFirstLineInset()
        {
            this.RemoveValue(PDFStyleKeys.TextFirstLineIndentKey);
        }

        #endregion

        #region public PDFUnit WordSpacing {get;set} + RemoveWordSpacing()

        [PDFAttribute("word-spacing")]
        public PDFUnit WordSpacing
        {
            get
            {
                PDFUnit u;
                if (this.TryGetValue(PDFStyleKeys.TextWordSpacingKey, out u))
                    return u;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextWordSpacingKey, value);
            }
        }

        public void RemoveWordSpacing()
        {
            this.RemoveValue(PDFStyleKeys.TextWordSpacingKey);
        }

        #endregion

        #region public PDFUnit CharacterSpacing {get;set} + RemoveCharacterSpacing()

        [PDFAttribute("char-spacing")]
        public PDFUnit CharacterSpacing
        {
            get
            {
                PDFUnit u;
                if (this.TryGetValue(PDFStyleKeys.TextCharSpacingKey, out u))
                    return u;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextCharSpacingKey, value);
            }
        }

        public void RemoveCharacterSpacing()
        {
            this.RemoveValue(PDFStyleKeys.TextCharSpacingKey);
        }

        #endregion

        #region public double HorizontalScaling {get;set;} + RemoveHorizontalScaling()

        [PDFAttribute("h-scale")]
        public double HorizontalScaling
        {
            get
            {
                double s;
                if (this.TryGetValue(PDFStyleKeys.TextHorizontalScaling, out s))
                    return s;
                else
                    return 1.0;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextHorizontalScaling, value);
            }
        }

        public void RemoveHoziontalScaling()
        {
            this.RemoveValue(PDFStyleKeys.TextHorizontalScaling);
        }

        #endregion


        #region public double HorizontalScaling {get;set;} + RemoveHorizontalScaling()

        [PDFAttribute("text-direction")]
        public TextDirection TextDirection
        {
            get
            {
                TextDirection s;
                if (this.TryGetValue(PDFStyleKeys.TextDirectionKey, out s))
                    return s;
                else
                    return Scryber.TextDirection.LTR;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextDirectionKey, value);
            }
        }

        public void RemoveTextDirection()
        {
            this.RemoveValue(PDFStyleKeys.TextDirectionKey);
        }

        #endregion

        #region public WordWrap WrapText {get;set;} + RemoveWrapText()

        [PDFAttribute("wrap")]
        public Scryber.Text.WordWrap WrapText
        {
            get
            {
                Scryber.Text.WordWrap f;
                if (this.TryGetValue(PDFStyleKeys.TextWordWrapKey, out f))
                    return f;
                else
                    return Scryber.Text.WordWrap.Auto;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextWordWrapKey, value);
            }
        }

        public void RemoveWrapText()
        {
            this.RemoveValue(PDFStyleKeys.TextWordWrapKey);
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
                if (this.TryGetValue(PDFStyleKeys.TextDecorationKey, out decor))
                    return decor;
                else
                    return Scryber.Text.TextDecoration.None;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextDecorationKey, value);
            }
        }

        /// <summary>
        /// Clears any set value from this style
        /// </summary>
        public void RemoveDecoration()
        {
            this.RemoveValue(PDFStyleKeys.TextDecorationKey);
        }

        #endregion

        #region public PDFUnit Leading {get;set;} + RemoveLeadingText()

        [PDFAttribute("leading")]
        public PDFUnit Leading
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(PDFStyleKeys.TextLeadingKey, out f))
                    return f;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextLeadingKey, value);
            }
        }

        public void RemoveLeading()
        {
            this.RemoveValue(PDFStyleKeys.TextLeadingKey);
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
                if (this.TryGetValue(PDFStyleKeys.TextWhitespaceKey, out value))
                    return value;
                else
                    return false;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextWhitespaceKey, value);
            }
        }

        public void RemoveWhitespace()
        {
            this.RemoveValue(PDFStyleKeys.TextWhitespaceKey);
        }

        #endregion

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
                if (this.TryGetValue(PDFStyleKeys.TextDateFormatKey,out f))
                    return f;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextDateFormatKey, value);
            }
        }

        public void RemoveDateFormat()
        {
            this.RemoveValue(PDFStyleKeys.TextDateFormatKey);
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
                if (this.TryGetValue(PDFStyleKeys.TextNumberFormatKey, out f))
                    return f;
                else
                    return string.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.TextNumberFormatKey, value);
            }
        }

        public void RemoveNumberFormat()
        {
            this.RemoveValue(PDFStyleKeys.TextNumberFormatKey);
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFTextStyle()

        public PDFTextStyle()
            : base(PDFStyleKeys.TextItemKey)
        {
        }

        #endregion
    }
}
