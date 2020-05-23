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
    [PDFParsableComponent("Stroke")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFStrokeStyle : PDFStyleItemBase
    {

        #region public LineStyle LineStyle {get;set;} + RemoveLineStyle()

        [PDFAttribute("style")]
        public LineStyle LineStyle
        {
            get
            {
                LineStyle val;
                if (this.TryGetValue(PDFStyleKeys.StrokeStyleKey, out val))
                    return val;
                else if (this.IsDefined(PDFStyleKeys.StrokeDashKey) && this.Dash != PDFDash.None)
                    return LineStyle.Dash;
                else if (this.IsDefined(PDFStyleKeys.StrokeColorKey))
                    return LineStyle.Solid;
                else
                    return LineStyle.None;
            }
            set
            {
                this.SetValue(PDFStyleKeys.StrokeStyleKey, value);
            }
        }

        public void RemoveLineStyle()
        {
            this.RemoveValue(PDFStyleKeys.StrokeStyleKey);
        }

        #endregion

        #region public PDFColor Color {get;set;} + RemoveColor()

        [PDFAttribute("color")]
        public PDFColor Color
        {
            get
            {
                PDFColor col;
                if (this.TryGetValue(PDFStyleKeys.StrokeColorKey, out col))
                    return col;
                else
                    return PDFColors.Transparent;
            }
            set { this.SetValue(PDFStyleKeys.StrokeColorKey, value); }
        }

        public void RemoveColor()
        {
            this.RemoveValue(PDFStyleKeys.StrokeColorKey);
        }

        #endregion

        #region public PDFUnit Width {get;set;} + RemoveWidth()

        [PDFAttribute("width")]
        public PDFUnit Width
        {
            get
            {
                PDFUnit f;
                if (this.TryGetValue(PDFStyleKeys.StrokeWidthKey, out f))
                    return f;
                else
                    return PDFUnit.Empty;
            }
            set
            {
                this.SetValue(PDFStyleKeys.StrokeWidthKey, value);
            }
        }

        public void RemoveWidth()
        {
            this.RemoveValue(PDFStyleKeys.StrokeWidthKey);
        }

        #endregion

        #region public PDFDash Dash {get;set;} + RemoveDash()

        [PDFAttribute("dash")]
        public PDFDash Dash
        {
            get
            {
                PDFDash dash;
                if (this.TryGetValue(PDFStyleKeys.StrokeDashKey, out dash))
                    return dash;
                else
                    return PDFDash.None;
            }
            set
            {
                this.SetValue(PDFStyleKeys.StrokeDashKey, value);
            }
        }

        public void RemoveDash()
        {
            this.RemoveValue(PDFStyleKeys.StrokeDashKey);
        }

        #endregion

        #region public LineCaps LineCap {get;set;} + RemoveLineCap()

        [PDFAttribute("ending")]
        public LineCaps LineCap
        {
            get
            {
                LineCaps cap;
                if (this.TryGetValue(PDFStyleKeys.StrokeEndingKey, out cap))
                    return (LineCaps)cap;
                else
                    return Const.DefaultLineCaps;
            }
            set
            {
                this.SetValue(PDFStyleKeys.StrokeEndingKey, value);
            }
        }

        public void RemoveLineCap()
        {
            this.RemoveValue(PDFStyleKeys.StrokeEndingKey);
        }

        #endregion

        #region public LineJoin LineJoin {get;set;} + RemoveLineJoin()

        [PDFAttribute("join")]
        public LineJoin LineJoin
        {
            get
            {
                LineJoin join;
                if (this.TryGetValue(PDFStyleKeys.StrokeJoinKey, out join))
                    return (LineJoin)join;
                else
                    return Const.DefaultLineJoin;
            }
            set
            {
                this.SetValue(PDFStyleKeys.StrokeJoinKey, value);
            }
        }

        public void RemoveLineJoin()
        {
            this.RemoveValue(PDFStyleKeys.StrokeJoinKey);
        }

        #endregion

        #region public float Mitre {get;set;} + RemoveMitre()

        [PDFAttribute("mitre")]
        public float Mitre
        {
            get
            {
                float f;
                if (this.TryGetValue(PDFStyleKeys.StrokeMitreKey, out f))
                    return f;
                else
                    return 0.0F;
            }
            set
            {
                this.SetValue(PDFStyleKeys.StrokeMitreKey, value);
            }
        }

        public void RemoveMitre()
        {
            this.RemoveValue(PDFStyleKeys.StrokeMitreKey);
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
                if (this.TryGetValue(PDFStyleKeys.StrokeOpacityKey, out found))
                    return found;
                else
                    return 1.0; //Default of 100% opacity
            }
            set
            {
                this.SetValue(PDFStyleKeys.StrokeOpacityKey, value);
            }
        }

        public void RemoveOpacity()
        {
            this.RemoveValue(PDFStyleKeys.StrokeOpacityKey);
        }

        #endregion

        /// <summary>
        /// Creates a new Stroke style item
        /// </summary>
        public PDFStrokeStyle()
            : base(PDFStyleKeys.StrokeItemKey)
        {
        }


        public virtual PDFPen CreatePen()
        {
            return this.AssertOwner().DoCreateStrokePen();
        }

    }
}
