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
using Scryber;
using Scryber.Drawing;
using System.ComponentModel;


namespace Scryber.Styles
{
    [PDFParsableComponent("Font")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams = "\"Font\"")]
    public class FontStyle : StyleItemBase
    {

        #region public string FontFamily {get;set;} + RemoveFontFamily()
        
        [PDFAttribute("family")]
        [PDFJSConvertor("scryber.studio.design.convertors.string_css", JSParams = "\"font-family\"")]
        public PDFFontSelector FontFamily
        {
            get
            {
                PDFFontSelector found;
                if (this.TryGetValue(StyleKeys.FontFamilyKey, out found))
                    return found;
                else
                    return null;
            }
            set
            {
                this.SetValue<PDFFontSelector>(StyleKeys.FontFamilyKey, value);
            }
        }

        public void RemoveFontFamily()
        {
            this.RemoveValue(StyleKeys.FontFamilyKey);
        }

        #endregion

        #region public bool FontBold {get;set;} + RemoveFontBold()

        [PDFAttribute("bold")]
        [PDFJSConvertor("scryber.studio.design.convertors.bold_css", JSParams = "\"font-weight\"")]
        [PDFDesignable("Font Bold", Category = "Font", Priority = 2, Type = "Boolean")]
        public bool FontBold
        {
            get
            {
                bool found;
                if (this.TryGetValue(StyleKeys.FontBoldKey, out found))
                    return found;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.FontBoldKey, value);
            }
        }

        public void RemoveFontBold()
        {
            this.RemoveValue(StyleKeys.FontBoldKey);
        }

        #endregion

        #region public bool FontItalic{ get;set;} + RemoveFontItalic()
       
        [PDFAttribute("italic")]
        [PDFJSConvertor("scryber.studio.design.convertors.italic_css", JSParams = "\"font-style\"")]
        [PDFDesignable("Font Italic", Category = "Font", Priority = 2, Type = "Boolean")]
        public bool FontItalic
        {
            get
            {
                bool found;
                if (this.TryGetValue(StyleKeys.FontItalicKey, out found))
                    return found;
                else
                    return false;
            }
            set
            {
                this.SetValue(StyleKeys.FontItalicKey, value);
            }
        }

        public void RemoveFontItalic()
        {
            this.RemoveValue(StyleKeys.FontItalicKey);
        }

        #endregion

        #region public PDFUnit FontSize {get; set;} + RemoveFontSize()

        [PDFAttribute("size")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"font-size\"")]
        public PDFUnit FontSize
        {
            get
            {
                PDFUnit found;
                if (this.TryGetValue(StyleKeys.FontSizeKey, out found))
                    return found;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.FontSizeKey, value);
            }
        }

        public void RemoveFontSize()
        {
            this.RemoveValue(StyleKeys.FontSizeKey);
        }

        #endregion

        public FontStyle()
            : base(StyleKeys.FontItemKey)
        {
        }


        public PDFFont CreateFont()
        {
            return this.AssertOwner().DoCreateFont(true);
        }

    }
}
