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
using System.Text;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using System.ComponentModel;
using Scryber.Drawing;

namespace Scryber.PDF.Graphics
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class PDFPen : IPDFGraphicsAdapter
    {
        public abstract LineType LineStyle { get;}

        #region PDFPenFlags

        private int _flags = 0;
        
        [Flags()]
        protected internal enum SetValues : int
        {
            Width = 1,
            Mitre = 2,
            Caps = 4,
            Join = 8,
            Color = 16,
            Dash = 32,
            Reserved1 = 64,
            Reserved2 = 128,
            Reserved3 = 256
        }

        protected internal void SetValue(SetValues value)
        {
            _flags = _flags | (int)value;
        }

        protected internal bool IsSet(SetValues value)
        {
            return (_flags & (int)value) > 0;
        }

        protected internal void ClearValue(SetValues value)
        {
            _flags = _flags & ~(int)value;
        }

        protected internal void ClearAll()
        {
            _flags = 0;
        }

        #endregion

        private Unit _w;

        public Unit Width
        {
            get { return _w; }
            set { _w = value; this.SetValue(SetValues.Width); }
        }
        
        private float _mitre;

        public float MitreLimit
        {
            get { return _mitre; }
            set { _mitre = value; this.SetValue(SetValues.Mitre); }
        }

        private LineCaps _caps;
        
        public LineCaps LineCaps
        {
            get { return _caps; }
            set { _caps = value; this.SetValue(SetValues.Caps); }
        }

        private LineJoin _join;

        public LineJoin LineJoin
        {
            get { return _join; }
            set { _join = value; this.SetValue(SetValues.Join); }
        }

        private PDFReal _op = (PDFReal)(-1.0);

        public PDFReal Opacity
        {
            get { return _op; }
            set { _op = value; }
        }

        public virtual void Reset()
        {
            this.ClearAll();
        }

        public virtual bool SetUpGraphics(PDFGraphics graphics, Rect bounds)
        {
            if (this.IsSet(SetValues.Caps))
                graphics.RenderLineCap(this.LineCaps);
            if (this.IsSet(SetValues.Join))
                graphics.RenderLineJoin(this.LineJoin);
            if (this.IsSet(SetValues.Mitre))
                graphics.RenderLineMitre(this.MitreLimit);
            if (this.IsSet(SetValues.Width))
                graphics.RenderLineWidth(this.Width);
            if (this.Opacity.Value > 0.0)
                graphics.SetStrokeOpacity(this.Opacity);

            return true;
        }

        public virtual void ReleaseGraphics(PDFGraphics g, Rect bounds)
        {
        }

        public static PDFPen Create(Color color, Unit width)
        {
            return new PDFSolidPen(color, width);
        }

        public static PDFPen Create(PDFBrush brush, Unit width)
        {
            if (brush.FillStyle == FillType.Solid)
            {
                PDFSolidBrush solid = (PDFSolidBrush)brush;
                PDFPen pen = Create(solid.Color, width);
                pen.Opacity = solid.Opacity;
                return pen;
            }
            else
                return new PDFNoPen();
        }
        
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFNoPen : PDFPen
    {
        public override LineType LineStyle
        {
            get { return LineType.None; }
        }

        public PDFNoPen()
            : base()
        {
        }

        public override bool SetUpGraphics(PDFGraphics g, Rect bounds)
        {
            return false;
        }

        public override void ReleaseGraphics(PDFGraphics g, Rect bounds)
        {
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFSolidPen : PDFPen
    {
        public override LineType LineStyle
        {
            get { return LineType.Solid; }
        }

        private Color _col;

        public Color Color
        {
            get { return _col; }
            set 
            {
                _col = value;
                if (value.IsEmpty)
                    this.ClearValue(SetValues.Color);
                else
                    this.SetValue(SetValues.Color); ;
            }
        }

        public PDFSolidPen()
            : base()
        {
        }

        public PDFSolidPen(Color color, Unit width)
            : this()
        {
            this.Width = width;
            this.Color = color;
        }

        public override bool SetUpGraphics(PDFGraphics graphics, Rect bounds)
        {
            bool result = base.SetUpGraphics(graphics, bounds);
            if (this.IsSet(SetValues.Color))
                graphics.SetStrokeColor(Color);
            return result;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFDashPen : PDFSolidPen
    {
        public override LineType LineStyle
        {
            get
            {
                return LineType.Dash;
            }
        }

        private Dash _dash;

        public Dash Dash
        {
            get { return _dash; }
            set 
            {
                _dash = value;
                if (value == null)
                    this.ClearValue(SetValues.Dash);
                else
                    this.SetValue(SetValues.Dash);
            }
        }

        public PDFDashPen(Dash dash)
        {
            this.Dash = dash;
        }

        public PDFDashPen(Dash dash, Color color, Unit width)
            : base(color, width)
        {
            this.Dash = dash;
        }

        public override bool SetUpGraphics(PDFGraphics graphics, Rect bounds)
        {
            bool result = base.SetUpGraphics(graphics, bounds);
            if (this.IsSet(SetValues.Dash))
                graphics.RenderLineDash(this.Dash);
            return result;
        }

    }
}
