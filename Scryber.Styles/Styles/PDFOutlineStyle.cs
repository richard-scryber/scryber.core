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
    [PDFParsableComponent("Outline")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFOutlineStyle : PDFStyleItemBase
    {

        #region public bool IsOutlined {get;set;} + RemoveOutline()

        /// <summary>
        /// Default is true - so titles will be shown when they are set
        /// </summary>
        [PDFAttribute("is-outlined")]
        public bool IsOutlined
        {
            get
            {
                bool outline;
                if (this.TryGetValue(PDFStyleKeys.OutlineIsOutlinedKey, out outline))
                    return outline;
                else
                    return true;
            }
            set
            {
                this.SetValue(PDFStyleKeys.OutlineIsOutlinedKey, value);
            }
        }

        public void RemoveOutline()
        {
            this.RemoveValue(PDFStyleKeys.OutlineIsOutlinedKey);
        }

        #endregion

        #region public PDFColor Color {get;set;} + RemoveColor()

        [PDFAttribute("color")]
        public PDFColor Color
        {
            get
            {
                PDFColor outline;
                if (this.TryGetValue(PDFStyleKeys.OutlineColorKey, out outline))
                    return outline;
                else
                    return PDFColors.Transparent;
            }
            set
            {
                this.SetValue(PDFStyleKeys.OutlineColorKey, value);
            }
        }

        public void RemoveColor()
        {
            this.RemoveValue(PDFStyleKeys.OutlineColorKey);
        }

        #endregion

        #region public bool FontBold {get;set;} + RemoveFontBold()

        [PDFAttribute("bold")]
        public bool FontBold
        {
            get
            {
                bool bold;
                if (this.TryGetValue(PDFStyleKeys.OutlineBoldKey, out bold))
                    return bold;
                else
                    return false;
            }
            set
            {
                this.SetValue(PDFStyleKeys.OutlineBoldKey, value);
            }
        }

        public void RemoveFontBold()
        {
            this.RemoveValue(PDFStyleKeys.OutlineBoldKey);
        }

        #endregion

        #region public bool FontItalic {get;set;} + RemoveFontItalic()

        [PDFAttribute("italic")]
        public bool FontItalic
        {
            get
            {
                bool ital;
                if (this.TryGetValue(PDFStyleKeys.OutlineItalicKey, out ital))
                    return ital;
                else
                    return false;
            }
            set
            {
                this.SetValue(PDFStyleKeys.OutlineItalicKey, value);
            }
        }

        public void RemoveFontItalic()
        {
            this.RemoveValue(PDFStyleKeys.OutlineItalicKey);
        }

        #endregion

        #region public bool Open {get;set;} + RemoveOpen()

        [PDFAttribute("open")]
        public bool Open
        {
            get
            {
                bool open;
                if (this.TryGetValue(PDFStyleKeys.OutlineOpenKey, out open))
                    return open;
                else
                    return false;
            }
            set
            {
                this.SetValue(PDFStyleKeys.OutlineOpenKey, value);
            }
        }

        public void RemoveOpen()
        {
            this.RemoveValue(PDFStyleKeys.OutlineOpenKey);
        }

        #endregion

        //
        // .ctor
        //

        public PDFOutlineStyle()
            : base(PDFStyleKeys.OutlineItemKey)
        {
        }
    }
}
