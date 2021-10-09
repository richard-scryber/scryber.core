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
using Scryber.PDF.Graphics;

namespace Scryber.Styles
{
    [PDFParsableComponent("Stroke")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class StrokeStyle : StyleItemBase
    {

        #region public LineStyle LineStyle {get;set;} + RemoveLineStyle()

        [PDFAttribute("style")]
        public LineType LineStyle
        {
            get
            {
                LineType val;
                if (this.TryGetValue(StyleKeys.StrokeStyleKey, out val))
                    return val;
                else if (this.IsDefined(StyleKeys.StrokeDashKey) && this.Dash != Dash.None)
                    return LineType.Dash;
                else if (this.IsDefined(StyleKeys.StrokeColorKey))
                    return LineType.Solid;
                else
                    return LineType.None;
            }
            set
            {
                this.SetValue(StyleKeys.StrokeStyleKey, value);
            }
        }

        public void RemoveLineStyle()
        {
            this.RemoveValue(StyleKeys.StrokeStyleKey);
        }

        #endregion

        #region public PDFColor Color {get;set;} + RemoveColor()

        [PDFAttribute("color")]
        public Color Color
        {
            get
            {
                Color col;
                if (this.TryGetValue(StyleKeys.StrokeColorKey, out col))
                    return col;
                else
                    return StandardColors.Transparent;
            }
            set { this.SetValue(StyleKeys.StrokeColorKey, value); }
        }

        public void RemoveColor()
        {
            this.RemoveValue(StyleKeys.StrokeColorKey);
        }

        #endregion

        #region public PDFUnit Width {get;set;} + RemoveWidth()

        [PDFAttribute("width")]
        public PDFUnit Width
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(StyleKeys.StrokeWidthKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.StrokeWidthKey, value);
            }
        }

        public void RemoveWidth()
        {
            this.RemoveValue(StyleKeys.StrokeWidthKey);
        }

        #endregion

        #region public PDFDash Dash {get;set;} + RemoveDash()

        [PDFAttribute("dash")]
        public Dash Dash
        {
            get
            {
                Dash dash;
                if (this.TryGetValue(StyleKeys.StrokeDashKey, out dash))
                    return dash;
                else
                    return Dash.None;
            }
            set
            {
                this.SetValue(StyleKeys.StrokeDashKey, value);
            }
        }

        public void RemoveDash()
        {
            this.RemoveValue(StyleKeys.StrokeDashKey);
        }

        #endregion

        #region public LineCaps LineCap {get;set;} + RemoveLineCap()

        [PDFAttribute("ending")]
        public LineCaps LineCap
        {
            get
            {
                LineCaps cap;
                if (this.TryGetValue(StyleKeys.StrokeEndingKey, out cap))
                    return (LineCaps)cap;
                else
                    return Const.DefaultLineCaps;
            }
            set
            {
                this.SetValue(StyleKeys.StrokeEndingKey, value);
            }
        }

        public void RemoveLineCap()
        {
            this.RemoveValue(StyleKeys.StrokeEndingKey);
        }

        #endregion

        #region public LineJoin LineJoin {get;set;} + RemoveLineJoin()

        [PDFAttribute("join")]
        public LineJoin LineJoin
        {
            get
            {
                LineJoin join;
                if (this.TryGetValue(StyleKeys.StrokeJoinKey, out join))
                    return (LineJoin)join;
                else
                    return Const.DefaultLineJoin;
            }
            set
            {
                this.SetValue(StyleKeys.StrokeJoinKey, value);
            }
        }

        public void RemoveLineJoin()
        {
            this.RemoveValue(StyleKeys.StrokeJoinKey);
        }

        #endregion

        #region public float Mitre {get;set;} + RemoveMitre()

        [PDFAttribute("mitre")]
        public float Mitre
        {
            get
            {
                float f;
                if (this.TryGetValue(StyleKeys.StrokeMitreKey, out f))
                    return f;
                else
                    return 0.0F;
            }
            set
            {
                this.SetValue(StyleKeys.StrokeMitreKey, value);
            }
        }

        public void RemoveMitre()
        {
            this.RemoveValue(StyleKeys.StrokeMitreKey);
        }

        #endregion

        #region public float Opacity {get; set} + RemoveOpacity()

        /// <summary>
        /// Gets or sets the opacity of the fill
        /// </summary>
        [PDFAttribute("opacity")]
        public double Opacity
        {
            get
            {
                double found;
                if (this.TryGetValue(StyleKeys.StrokeOpacityKey, out found))
                    return found;
                else
                    return 1.0; //Default of 100% opacity
            }
            set
            {
                this.SetValue(StyleKeys.StrokeOpacityKey, value);
            }
        }

        public void RemoveOpacity()
        {
            this.RemoveValue(StyleKeys.StrokeOpacityKey);
        }

        #endregion

        /// <summary>
        /// Creates a new Stroke style item
        /// </summary>
        public StrokeStyle()
            : base(StyleKeys.StrokeItemKey)
        {
        }


        public virtual PDFPen CreatePen()
        {
            return this.AssertOwner().DoCreateStrokePen();
        }

    }
}
