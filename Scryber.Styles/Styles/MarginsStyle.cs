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
using Scryber.Drawing;
using System.ComponentModel;
using Scryber;

namespace Scryber.Styles
{
    [PDFParsableComponent("Margins")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams = "\"Margin\"")]
    public class MarginsStyle : StyleItemBase
    {

        #region public PDFUnit Top {get; set;}

        [PDFAttribute("top")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"margin-top\"")]
        [PDFDesignable("Margins Top", Category = "Margins", Type = "PDFUnit")]
        public PDFUnit Top
        {
            get
            {
                PDFUnit found;
                if (this.TryGetValue(StyleKeys.MarginsTopKey, out found))
                    return found;
                else if (this.TryGetValue(StyleKeys.MarginsAllKey, out found))
                    return found;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.MarginsTopKey, value);
            }
        }

        public void RemoveTop()
        {
            this.RemoveValue(StyleKeys.MarginsTopKey);
        }

        #endregion

        #region public PDFUnit Bottom {get;set;}

        [PDFAttribute("bottom")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"margin-bottom\"")]
        [PDFDesignable("Margins Bottom", Category = "Margins", Type = "PDFUnit")]
        public PDFUnit Bottom
        {
            get
            {
                PDFUnit b;
                if(this.TryGetValue(StyleKeys.MarginsBottomKey,out b))
                    return b;
                else if (this.TryGetValue(StyleKeys.MarginsAllKey, out b))
                    return b;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.MarginsBottomKey, value);
            }
        }

        public void RemoveBottom()
        {
            this.RemoveValue(StyleKeys.MarginsBottomKey);
        }

        #endregion

        #region public PDFUnit Left {get; set;}

        [PDFAttribute("left")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"margin-left\"")]
        [PDFDesignable("Margins Left", Category = "Margins", Type = "PDFUnit")]
        public PDFUnit Left
        {
            get
            {
                PDFUnit found;
                if (this.TryGetValue(StyleKeys.MarginsLeftKey, out found))
                    return found;
                else if (this.TryGetValue(StyleKeys.MarginsAllKey, out found))
                    return found;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.MarginsLeftKey, value);
            }
        }

        public void RemoveLeft()
        {
            this.RemoveValue(StyleKeys.MarginsLeftKey);
        }

        #endregion

        #region public PDFUnit Right {get;set;}

        [PDFAttribute("right")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"margin-right\"")]
        [PDFDesignable("Margins Right", Category = "Margins", Type = "PDFUnit")]
        public PDFUnit Right
        {
            get
            {
                PDFUnit r;
                if (this.TryGetValue(StyleKeys.MarginsRightKey, out r))
                    return r;
                else if (this.TryGetValue(StyleKeys.MarginsAllKey, out r))
                    return r;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.MarginsRightKey, value);
            }
        }

        public void RemoveRight()
        {
            this.RemoveValue(StyleKeys.MarginsRightKey);
        }

        #endregion

        #region public PDFUnit All {get;set;}

        [PDFAttribute("all")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"margin\"")]
        [PDFDesignable("Margins All", Category = "Margins", Type = "PDFUnit")]
        public PDFUnit All
        {
            get
            {
                PDFUnit b;
                if (this.TryGetValue(StyleKeys.MarginsAllKey, out b))
                    return b;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.MarginsAllKey, value);
            }
        }

        public void RemoveAll()
        {
            this.RemoveValue(StyleKeys.MarginsAllKey);
        }

        #endregion

        public MarginsStyle()
            : base(StyleKeys.MarginsItemKey)
        {
        }

        public bool TryGetThickness(out PDFThickness thickness)
        {
            return this.AssertOwner().TryGetThickness(this.IsInherited, StyleKeys.MarginsAllKey, StyleKeys.MarginsTopKey, StyleKeys.MarginsLeftKey, StyleKeys.MarginsBottomKey, StyleKeys.MarginsRightKey, out thickness);
        }

        public void SetThickness(PDFThickness thickness)
        {
            this.AssertOwner().SetThickness(this.IsInherited, thickness, StyleKeys.MarginsTopKey, StyleKeys.MarginsLeftKey, StyleKeys.MarginsBottomKey, StyleKeys.MarginsRightKey);
        }
    }
}
