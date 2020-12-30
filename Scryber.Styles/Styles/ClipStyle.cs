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
    [PDFParsableComponent("Clipping")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ClipStyle : StyleItemBase
    {
        

        #region public PDFUnit Top {get; set;}

        [PDFAttribute("top")]
        public PDFUnit Top
        {
            get
            {
                PDFUnit found;
                if (this.TryGetValue(StyleKeys.ClipTopKey, out found))
                    return found;
                else if (this.TryGetValue(StyleKeys.ClipAllKey, out found))
                    return found;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.ClipTopKey, value);
            }
        }

        public void RemoveTop()
        {
            this.RemoveValue(StyleKeys.ClipTopKey);
        }

        #endregion

        #region public PDFUnit Bottom {get;set;}

        [PDFAttribute("bottom")]
        public PDFUnit Bottom
        {
            get
            {
                PDFUnit b;
                if (this.TryGetValue(StyleKeys.ClipBottomKey, out b))
                    return b;
                else if (this.TryGetValue(StyleKeys.ClipAllKey, out b))
                    return b;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.ClipBottomKey, value);
            }
        }

        public void RemoveBottom()
        {
            this.RemoveValue(StyleKeys.ClipBottomKey);
        }

        #endregion

        #region public PDFUnit Left {get; set;}

        [PDFAttribute("left")]
        public PDFUnit Left
        {
            get
            {
                PDFUnit found;
                if (this.TryGetValue(StyleKeys.ClipLeftKey, out found))
                    return found;
                else if (this.TryGetValue(StyleKeys.ClipAllKey, out found))
                    return found;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.ClipLeftKey, value);
            }
        }

        public void RemoveLeft()
        {
            this.RemoveValue(StyleKeys.ClipLeftKey);
        }

        #endregion

        #region public PDFUnit Right {get;set;}

        [PDFAttribute("right")]
        public PDFUnit Right
        {
            get
            {
                PDFUnit r;
                if (this.TryGetValue(StyleKeys.ClipRightKey, out r))
                    return r;
                else if (this.TryGetValue(StyleKeys.ClipAllKey, out r))
                    return r;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.ClipRightKey, value);
            }
        }

        public void RemoveRight()
        {
            this.RemoveValue(StyleKeys.ClipRightKey);
        }

        #endregion

        #region public PDFUnit All {get;set;}

        [PDFAttribute("all")]
        public PDFUnit All
        {
            get
            {
                PDFUnit b;
                if (this.TryGetValue(StyleKeys.ClipAllKey, out b))
                    return b;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.SetValue(StyleKeys.ClipAllKey, value);
            }
        }

        public void RemoveAll()
        {
            this.RemoveValue(StyleKeys.ClipAllKey);
        }

        #endregion


        public ClipStyle()
            : base(StyleKeys.ClipItemKey)
        {
        }


        public bool TryGetThickness(out PDFThickness thickness)
        {
            return this.AssertOwner().TryGetThickness(this.IsInherited, StyleKeys.ClipAllKey, StyleKeys.ClipTopKey, StyleKeys.ClipLeftKey, StyleKeys.ClipBottomKey, StyleKeys.ClipRightKey, out thickness);
        }

        public void SetThickness(PDFThickness thickness)
        {
            this.AssertOwner().SetThickness(this.IsInherited, thickness, StyleKeys.ClipTopKey, StyleKeys.ClipLeftKey, StyleKeys.ClipBottomKey, StyleKeys.ClipRightKey);
        }
    }
}
