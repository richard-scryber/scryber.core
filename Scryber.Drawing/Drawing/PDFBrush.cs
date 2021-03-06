﻿/*  Copyright 2012 PerceiveIT Limited
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
using Scryber.Native;
using Scryber.Resources;
using System.ComponentModel;

namespace Scryber.Drawing
{
    public abstract class PDFBrush : PDFGraphicsAdapter
    {
        public abstract FillType FillStyle {get;}

        public PDFBrush UnderBrush { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFSolidBrush : PDFBrush
    {
        public override FillType FillStyle
        {
            get { return FillType.Solid; }
        }

        private PDFColor _col;

        public PDFColor Color
        {
            get { return _col; }
            set { _col = value; }
        }

        private PDFReal _op;

        public PDFReal Opacity
        {
            get { return _op; }
            set { _op = value; }
        }

        public PDFSolidBrush()
            : this(PDFColors.Transparent)
        {
        }

        public PDFSolidBrush(PDFColor color)
            : this(color, 1)
        {
        }

        public PDFSolidBrush(PDFColor color, double opacity)
            : this(color, (PDFReal)opacity)
        {
        }

        public PDFSolidBrush(PDFColor color, PDFReal opacity)
        {
            this._col = color;
            this._op = opacity;
        }

        public override bool SetUpGraphics(PDFGraphics g, PDFRect bounds)
        {
            g.SetFillOpacity(this.Opacity);
            if (this.Color != null && this.Color.IsEmpty == false)
            {
                g.SetFillColor(this.Color);
                return true;
            }
            else
                return false;
        }

        public override void ReleaseGraphics(PDFGraphics g, PDFRect bounds)
        {
            
        }


        public static PDFSolidBrush Create(PDFColor color)
        {
            return new PDFSolidBrush(color);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFNoBrush : PDFBrush
    {
        public override FillType FillStyle
        {
            get { return FillType.None; }
        }

        public override bool SetUpGraphics(PDFGraphics g, PDFRect bounds)
        {
            return false;
        }

        public override void ReleaseGraphics(PDFGraphics g, PDFRect bounds)
        {
        }

        public PDFNoBrush()
            : base()
        {
        }
    }

    
}
