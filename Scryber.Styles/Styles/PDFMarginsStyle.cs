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
    public class PDFMarginsStyle : PDFStyleItemBase
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
                if (this.TryGetValue(PDFStyleKeys.MarginsTopKey, out found))
                    return found;
                else if (this.TryGetValue(PDFStyleKeys.MarginsAllKey, out found))
                    return found;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.MarginsTopKey, value);
            }
        }

        public void RemoveTop()
        {
            this.RemoveValue(PDFStyleKeys.MarginsTopKey);
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
                if(this.TryGetValue(PDFStyleKeys.MarginsBottomKey,out b))
                    return b;
                else if (this.TryGetValue(PDFStyleKeys.MarginsAllKey, out b))
                    return b;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.MarginsBottomKey, value);
            }
        }

        public void RemoveBottom()
        {
            this.RemoveValue(PDFStyleKeys.MarginsBottomKey);
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
                if (this.TryGetValue(PDFStyleKeys.MarginsLeftKey, out found))
                    return found;
                else if (this.TryGetValue(PDFStyleKeys.MarginsAllKey, out found))
                    return found;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.MarginsLeftKey, value);
            }
        }

        public void RemoveLeft()
        {
            this.RemoveValue(PDFStyleKeys.MarginsLeftKey);
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
                if (this.TryGetValue(PDFStyleKeys.MarginsRightKey, out r))
                    return r;
                else if (this.TryGetValue(PDFStyleKeys.MarginsAllKey, out r))
                    return r;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.MarginsRightKey, value);
            }
        }

        public void RemoveRight()
        {
            this.RemoveValue(PDFStyleKeys.MarginsRightKey);
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
                if (this.TryGetValue(PDFStyleKeys.MarginsAllKey, out b))
                    return b;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(PDFStyleKeys.MarginsAllKey, value);
            }
        }

        public void RemoveAll()
        {
            this.RemoveValue(PDFStyleKeys.MarginsAllKey);
        }

        #endregion

        public PDFMarginsStyle()
            : base(PDFStyleKeys.MarginsItemKey)
        {
        }

        public bool TryGetThickness(out PDFThickness thickness)
        {
            return this.AssertOwner().TryGetThickness(this.IsInherited, PDFStyleKeys.MarginsAllKey, PDFStyleKeys.MarginsTopKey, PDFStyleKeys.MarginsLeftKey, PDFStyleKeys.MarginsBottomKey, PDFStyleKeys.MarginsRightKey, out thickness);
        }

        public void SetThickness(PDFThickness thickness)
        {
            this.AssertOwner().SetThickness(this.IsInherited, thickness, PDFStyleKeys.MarginsTopKey, PDFStyleKeys.MarginsLeftKey, PDFStyleKeys.MarginsBottomKey, PDFStyleKeys.MarginsRightKey);
        }
    }
}
