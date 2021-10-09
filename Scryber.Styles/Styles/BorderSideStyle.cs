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
    public abstract class BorderSideStyle : StyleItemBase
    {
        //The instance style keys

        private StyleKey<LineType> _line;
        private StyleKey<PDFColor> _color;
        private StyleKey<PDFUnit> _width;
        private StyleKey<Dash> _dash;

        //Actual side we are
        private Sides _side;

        #region public LineStyle LineStyle {get;set;} + RemoveLineStyle()

        [PDFAttribute("style")]
        [PDFJSConvertor("scryber.studio.design.convertors.string_css", JSParams = "\"border-style\"")]
        public LineType LineStyle
        {
            get
            {
                LineType val;
                if (this.TryGetValue(_line,out val))
                    return val;
                else if (this.IsDefined(_dash) && this.Dash != Dash.None)
                    return LineType.Dash;
                else if (this.IsDefined(_color))
                    return LineType.Solid;
                else
                    return LineType.None;
            }
            set
            {
                this.SetValue(_line, value);
            }
        }

        public void RemoveLineStyle()
        {
            this.RemoveValue(_line);
        }

        #endregion

        #region public PDFColor Color {get;set;} + RemoveColor()

        [PDFAttribute("color")]
        [PDFJSConvertor("scryber.studio.design.convertors.color_css", JSParams = "\"border-color\"")]
        [PDFDesignable("Border Color", Category = "Border", Priority = 1, Type = "Color")]
        public PDFColor Color
        {
            get
            {
                PDFColor col;
                if (this.TryGetValue(_color, out col))
                    return col;
                else
                    return PDFColors.Transparent;
            }
            set { this.SetValue(_color, value); }
        }

        public void RemoveColor()
        {
            this.RemoveValue(_color);
        }

        #endregion

        #region public PDFUnit Width {get;set;} + RemoveWidth()

        [PDFAttribute("width")]
        [PDFJSConvertor("scryber.studio.design.convertors.unit_css", JSParams = "\"border-width\"")]
        [PDFDesignable("Border Width", Category = "Border", Priority = 1, Type = "PDFUnit")]
        public PDFUnit Width
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(_width, out f))
                    return f;
                else
                    return PDFUnit.Empty;
            }
            set
            {
                this.SetValue(_width, value);
            }
        }

        public void RemoveWidth()
        {
            this.RemoveValue(_width);
        }

        #endregion

        #region public PDFDash Dash {get;set;} + RemoveDash()

        [PDFAttribute("dash")]
        public Dash Dash
        {
            get
            {
                Dash dash;
                if (this.TryGetValue(_dash, out dash))
                    return dash;
                else
                    return Dash.None;
            }
            set
            {
                if (null == value)
                    this.RemoveDash();
                else
                    this.SetValue(_dash, value);
            }
        }

        public void RemoveDash()
        {
            this.RemoveValue(_dash);
        }

        #endregion



        //
        // .ctor
        //

        #region public PDFBorderStyle()


        public BorderSideStyle(Sides forSide, StyleKey forKey)
            : this(forSide, forKey, StyleKeys.BorderColorKey, StyleKeys.BorderWidthKey, StyleKeys.BorderStyleKey, StyleKeys.BorderDashKey)
        {

        }

        /// <summary>
        /// Creates a new Border style
        /// </summary>
        public BorderSideStyle(Sides forSide, StyleKey forKey, StyleKey<PDFColor> colorKey, StyleKey<PDFUnit> widthkey, StyleKey<LineType> lineStyleKey, StyleKey<Dash> dashkey): 
            base(forKey)
        {
            _side = forSide;
            _color = colorKey;
            _width = widthkey;
            _line = lineStyleKey;
            _dash = dashkey;
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
            return this.AssertOwner().DoCreateBorderSidePen(this._side, _color, _width, _line, _dash);
        }

        #endregion

        
    }

    [PDFParsableComponent("Border-Left")]
    public class BorderLeftStyle : BorderSideStyle
    {

        public BorderLeftStyle() : base(Sides.Left, StyleKeys.BorderItemLeftKey)
        {

        }
    }

    [PDFParsableComponent("Border-Right")]
    public class BorderRightStyle : BorderSideStyle
    {

        public BorderRightStyle() : base(Sides.Right, StyleKeys.BorderItemRightKey)
        {

        }
    }

    [PDFParsableComponent("Border-Top")]
    public class BorderTopStyle : BorderSideStyle
    {

        public BorderTopStyle() : base(Sides.Top, StyleKeys.BorderItemTopKey)
        {

        }
    }

    [PDFParsableComponent("Border-Bottom")]
    public class BorderBottomStyle : BorderSideStyle
    {

        public BorderBottomStyle() : base(Sides.Bottom, StyleKeys.BorderItemBottomKey)
        {
        }
    }
}
