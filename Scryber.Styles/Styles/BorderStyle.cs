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
    [PDFParsableComponent("Border")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [PDFJSConvertor("scryber.studio.design.convertors.styleItem", JSParams = "\"Border\"")]
    public class BorderStyle : StyleItemBase
    {


        #region public PDFUnit CornerRadius {get;set;} + RemoveCornerRadius()

        [PDFAttribute("corner-radius")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"corner-radius\"")]
        [PDFDesignable("Border Radius", Category = "Border", Priority = 2, Type = "PDFUnit")]
        public Unit CornerRadius
        {
            get
            {
                Unit rad;
                if (this.TryGetValue(StyleKeys.BorderCornerRadiusKey, out rad))
                    return rad;
                else
                    return Unit.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.BorderCornerRadiusKey, value);
            }
        }

        public void RemoveCornerRadius()
        {
            this.RemoveValue(StyleKeys.BorderCornerRadiusKey);
        }

        #endregion

        #region public Sides Sides {get; set;} + RemoveSides()

        [PDFAttribute("sides")]
        [PDFJSConvertor("scryber.studio.design.convertors.bordersides_css", JSParams = "\"border\"")]
        [PDFDesignable("Sides", Ignore = false, Category = "Border", Priority = 3, Type = "FlagsSelect")]
        public Sides Sides
        {
            get
            {
                Sides side;
                if (this.TryGetValue(StyleKeys.BorderSidesKey, out side))
                    return side;
                else
                    return (Sides.Left | Sides.Right | Sides.Top | Sides.Bottom);
            }
            set
            {
                this.SetValue(StyleKeys.BorderSidesKey, value);
            }
        }

        public void RemoveSides()
        {
            this.RemoveValue(StyleKeys.BorderSidesKey);
        }

        #endregion

        #region public LineStyle LineStyle {get;set;} + RemoveLineStyle()

        [PDFAttribute("style")]
        [PDFJSConvertor("scryber.studio.design.convertors.string_css", JSParams = "\"border-style\"")]
        public LineType LineStyle
        {
            get
            {
                LineType val;
                if (this.TryGetValue(StyleKeys.BorderStyleKey,out val))
                    return val;
                else if (this.IsDefined(StyleKeys.BorderDashKey) && this.Dash != Dash.None)
                    return LineType.Dash;
                else if (this.IsDefined(StyleKeys.BorderColorKey))
                    return LineType.Solid;
                else
                    return LineType.None;
            }
            set
            {
                this.SetValue(StyleKeys.BorderStyleKey, value);
            }
        }

        public void RemoveLineStyle()
        {
            this.RemoveValue(StyleKeys.BorderStyleKey);
        }

        #endregion

        #region public PDFColor Color {get;set;} + RemoveColor()

        [PDFAttribute("color")]
        [PDFJSConvertor("scryber.studio.design.convertors.color_css", JSParams = "\"border-color\"")]
        [PDFDesignable("Border Color", Category = "Border", Priority = 1, Type = "Color")]
        public Color Color
        {
            get
            {
                Color col;
                if (this.TryGetValue(StyleKeys.BorderColorKey, out col))
                    return col;
                else
                    return StandardColors.Transparent;
            }
            set { this.SetValue(StyleKeys.BorderColorKey, value); }
        }

        public void RemoveColor()
        {
            this.RemoveValue(StyleKeys.BorderColorKey);
        }

        #endregion

        #region public PDFUnit Width {get;set;} + RemoveWidth()

        [PDFAttribute("width")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"border-width\"")]
        [PDFDesignable("Border Width", Category = "Border", Priority = 1, Type = "PDFUnit")]
        public Unit Width
        {
            get
            {
                Unit f;
                if (this.TryGetValue(StyleKeys.BorderWidthKey, out f))
                    return f;
                else
                    return Unit.Empty;
            }
            set
            {
                this.SetValue(StyleKeys.BorderWidthKey, value);
            }
        }

        public void RemoveWidth()
        {
            this.RemoveValue(StyleKeys.BorderWidthKey);
        }

        #endregion

        #region public PDFDash Dash {get;set;} + RemoveDash()

        [PDFAttribute("dash")]
        public Dash Dash
        {
            get
            {
                Dash dash;
                if (this.TryGetValue(StyleKeys.BorderDashKey, out dash))
                    return dash;
                else
                    return Dash.None;
            }
            set
            {
                if (null == value)
                    this.RemoveDash();
                else
                    this.SetValue(StyleKeys.BorderDashKey, value);
            }
        }

        public void RemoveDash()
        {
            this.RemoveValue(StyleKeys.BorderDashKey);
        }

        #endregion

        #region public LineCaps LineCap {get;set;} + RemoveLineCap()

        [PDFAttribute("ending")]
        public LineCaps LineCap
        {
            get
            {
                LineCaps cap;
                if (this.TryGetValue(StyleKeys.BorderEndingKey, out cap))
                    return (LineCaps)cap;
                else
                    return Const.DefaultLineCaps;
            }
            set
            {
                this.SetValue(StyleKeys.BorderEndingKey, value);
            }
        }

        public void RemoveLineCap()
        {
            this.RemoveValue(StyleKeys.BorderEndingKey);
        }

        #endregion

        #region public LineJoin LineJoin {get;set;} + RemoveLineJoin()

        [PDFAttribute("join")]
        public LineJoin LineJoin
        {
            get
            {
                LineJoin join;
                if (this.TryGetValue(StyleKeys.BorderJoinKey, out join))
                    return (LineJoin)join;
                else
                    return Const.DefaultLineJoin;
            }
            set
            {
                this.SetValue(StyleKeys.BorderJoinKey, value);
            }
        }

        public void RemoveLineJoin()
        {
            this.RemoveValue(StyleKeys.BorderJoinKey);
        }

        #endregion

        #region public float Mitre {get;set;} + RemoveMitre()

        [PDFAttribute("mitre")]
        public float Mitre
        {
            get
            {
                float f;
                if (this.TryGetValue(StyleKeys.BorderMitreKey, out f))
                    return f;
                else
                    return 0.0F;
            }
            set
            {
                this.SetValue(StyleKeys.BorderMitreKey, value);
            }
        }

        public void RemoveMitre()
        {
            this.RemoveValue(StyleKeys.BorderMitreKey);
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
                if (this.TryGetValue(StyleKeys.BorderOpacityKey, out found))
                    return found;
                else
                    return 1.0; //Default of 100% opacity
            }
            set
            {
                this.SetValue(StyleKeys.BorderOpacityKey, value);
            }
        }

        public void RemoveOpacity()
        {
            this.RemoveValue(StyleKeys.BorderOpacityKey);
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFBorderStyle()

        /// <summary>
        /// Creates a new Border style
        /// </summary>
        public BorderStyle(): 
            base(StyleKeys.BorderItemKey)
        {

        }

        #endregion

        //
        // public methods
        //

        #region public virtual PDFPen CreatePen()

        /// <summary>
        /// Creates a new pen that matches this styles values.
        /// </summary>
        /// <returns></returns>
        public virtual PDFPen CreatePen()
        {
            StyleValue<Color> c;
            StyleValue<LineType> t;
            StyleValue<Dash> d;
            StyleValue<Unit> w;

            return this.AssertOwner().DoCreateBorderPen(out c, out w, out t, out d);
        }

        #endregion

        
    }
}
