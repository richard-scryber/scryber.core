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
    public class PaddingStyle : StyleItemBase
    {
        
        #region public PDFUnit Top {get; set;}

        [PDFAttribute("top")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"padding-top\"")]
        public Unit Top
        {
            get
            {
                Unit found;
                if (this.TryGetValue(StyleKeys.PaddingTopKey, out found))
                    return found;
                else if (this.TryGetValue(StyleKeys.PaddingAllKey, out found))
                    return found;
                else
                    return Unit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.PaddingTopKey, value);
            }
        }

        public void RemoveTop()
        {
            this.RemoveValue(StyleKeys.PaddingTopKey);
        }

        #endregion

        #region public PDFUnit Bottom {get;set;}

        [PDFAttribute("bottom")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"padding-bottom\"")]
        public Unit Bottom
        {
            get
            {
                Unit b;
                if (this.TryGetValue(StyleKeys.PaddingBottomKey, out b))
                    return b;
                else if (this.TryGetValue(StyleKeys.PaddingAllKey, out b))
                    return b;
                else
                    return Unit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.PaddingBottomKey, value);
            }
        }

        public void RemoveBottom()
        {
            this.RemoveValue(StyleKeys.PaddingBottomKey);
        }

        #endregion

        #region public PDFUnit Left {get; set;}

        [PDFAttribute("left")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"padding-left\"")]
        public Unit Left
        {
            get
            {
                Unit found;
                if (this.TryGetValue(StyleKeys.PaddingLeftKey, out found))
                    return found;
                else if (this.TryGetValue(StyleKeys.PaddingAllKey, out found))
                    return found;
                else
                    return Unit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.PaddingLeftKey, value);
            }
        }

        public void RemoveLeft()
        {
            this.RemoveValue(StyleKeys.PaddingLeftKey);
        }

        #endregion

        #region public PDFUnit Right {get;set;}

        [PDFAttribute("right")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"padding-right\"")]
        public Unit Right
        {
            get
            {
                Unit r;
                if (this.TryGetValue(StyleKeys.PaddingRightKey, out r))
                    return r;
                else if (this.TryGetValue(StyleKeys.PaddingAllKey, out r))
                    return r;
                else
                    return Unit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.PaddingRightKey, value);
            }
        }

        public void RemoveRight()
        {
            this.RemoveValue(StyleKeys.PaddingRightKey);
        }

        #endregion

        #region public PDFUnit All {get;set;}

        [PDFAttribute("all")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"padding\"")]
        public Unit All
        {
            get
            {
                Unit b;
                if (this.TryGetValue(StyleKeys.PaddingAllKey, out b))
                    return b;
                else
                    return Unit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.PaddingAllKey, value);
            }
        }

        public void RemoveAll()
        {
            this.RemoveValue(StyleKeys.PaddingAllKey);
        }

        #endregion


        public PaddingStyle()
            : base(StyleKeys.PaddingItemKey)
        {
        }


        public bool TryGetThickness(out Thickness thickness)
        {
            return this.AssertOwner().TryGetThickness(this.IsInherited, StyleKeys.PaddingAllKey, StyleKeys.PaddingTopKey, StyleKeys.PaddingLeftKey, StyleKeys.PaddingBottomKey, StyleKeys.PaddingRightKey, out thickness);
        }

        public void SetThickness(Thickness thickness)
        {
            this.AssertOwner().SetThickness(this.IsInherited, thickness, StyleKeys.PaddingTopKey, StyleKeys.PaddingLeftKey, StyleKeys.PaddingBottomKey, StyleKeys.PaddingRightKey);
        }
    }
}
