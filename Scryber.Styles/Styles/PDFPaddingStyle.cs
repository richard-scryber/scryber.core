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
    [PDFParsableComponent("Padding")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams = "\"Padding\"")]
    public class PDFPaddingStyle : PDFStyleItemBase
    {
        
        #region public PDFUnit Top {get; set;}

        [PDFAttribute("top")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"padding-top\"")]
        public PDFUnit Top
        {
            get
            {
                PDFUnit found;
                if (this.TryGetValue(PDFStyleKeys.PaddingTopKey, out found))
                    return found;
                else if (this.TryGetValue(PDFStyleKeys.PaddingAllKey, out found))
                    return found;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.PaddingTopKey, value);
            }
        }

        public void RemoveTop()
        {
            this.RemoveValue(PDFStyleKeys.PaddingTopKey);
        }

        #endregion

        #region public PDFUnit Bottom {get;set;}

        [PDFAttribute("bottom")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"padding-bottom\"")]
        public PDFUnit Bottom
        {
            get
            {
                PDFUnit b;
                if (this.TryGetValue(PDFStyleKeys.PaddingBottomKey, out b))
                    return b;
                else if (this.TryGetValue(PDFStyleKeys.PaddingAllKey, out b))
                    return b;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.PaddingBottomKey, value);
            }
        }

        public void RemoveBottom()
        {
            this.RemoveValue(PDFStyleKeys.PaddingBottomKey);
        }

        #endregion

        #region public PDFUnit Left {get; set;}

        [PDFAttribute("left")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"padding-left\"")]
        public PDFUnit Left
        {
            get
            {
                PDFUnit found;
                if (this.TryGetValue(PDFStyleKeys.PaddingLeftKey, out found))
                    return found;
                else if (this.TryGetValue(PDFStyleKeys.PaddingAllKey, out found))
                    return found;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.PaddingLeftKey, value);
            }
        }

        public void RemoveLeft()
        {
            this.RemoveValue(PDFStyleKeys.PaddingLeftKey);
        }

        #endregion

        #region public PDFUnit Right {get;set;}

        [PDFAttribute("right")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"padding-right\"")]
        public PDFUnit Right
        {
            get
            {
                PDFUnit r;
                if (this.TryGetValue(PDFStyleKeys.PaddingRightKey, out r))
                    return r;
                else if (this.TryGetValue(PDFStyleKeys.PaddingAllKey, out r))
                    return r;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.PaddingRightKey, value);
            }
        }

        public void RemoveRight()
        {
            this.RemoveValue(PDFStyleKeys.PaddingRightKey);
        }

        #endregion

        #region public PDFUnit All {get;set;}

        [PDFAttribute("all")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"padding\"")]
        public PDFUnit All
        {
            get
            {
                PDFUnit b;
                if (this.TryGetValue(PDFStyleKeys.PaddingAllKey, out b))
                    return b;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.PaddingAllKey, value);
            }
        }

        public void RemoveAll()
        {
            this.RemoveValue(PDFStyleKeys.PaddingAllKey);
        }

        #endregion


        public PDFPaddingStyle()
            : base(PDFStyleKeys.PaddingItemKey)
        {
        }


        public bool TryGetThickness(out PDFThickness thickness)
        {
            return this.AssertOwner().TryGetThickness(this.IsInherited, PDFStyleKeys.PaddingAllKey, PDFStyleKeys.PaddingTopKey, PDFStyleKeys.PaddingLeftKey, PDFStyleKeys.PaddingBottomKey, PDFStyleKeys.PaddingRightKey, out thickness);
        }

        public void SetThickness(PDFThickness thickness)
        {
            this.AssertOwner().SetThickness(this.IsInherited, thickness, PDFStyleKeys.PaddingTopKey, PDFStyleKeys.PaddingLeftKey, PDFStyleKeys.PaddingBottomKey, PDFStyleKeys.PaddingRightKey);
        }
    }
}
