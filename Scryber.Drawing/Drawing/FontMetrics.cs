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
using System.ComponentModel;

namespace Scryber.Drawing
{
    /// <summary>
    /// General metrics on the a font for a specific size
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FontMetrics
    {
        private double _emh;
        /// <summary>
        /// Gets the em height for a font
        /// </summary>
        public double EmHeight
        {
            get { return _emh; }
        }
               
        private double _desc;

        /// <summary>
        /// Gets the size of the descender for a font (from the base line to the bottom of the 'j')
        /// </summary>
        public double Descent
        {
            get { return _desc; }
        }

        private double _asc;
        /// <summary>
        /// Gets the size of the ascender (base line to the top of the 'h')
        /// </summary>
        public double Ascent
        {
            get { return _asc; }
        }

        private double _lineHeight;
        /// <summary>
        /// Gets the total height of the line (ascender + descender + line spacing) or (the distance between two baselines)
        /// </summary>
        public double LineHeight
        {
            get { return _lineHeight; }
        }

        private double _exh;
        /// <summary>
        /// Gets the height of a lower case x in the font size
        /// </summary>
        public double ExHeight
        {
            get { return _exh; }
        }

        private double _zero;

        /// <summary>
        /// Gets the width of the zero character in the font size
        /// </summary>
        public double ZeroWidth
        {
            get { return _zero; }
        }

        /// <summary>
        /// Gets the amount of white space that should be between two lines of text (the leading)
        /// </summary>
        public double LineSpacing
        {
            get { return this.LineHeight - (this.Ascent + this.Descent); }
        }


        public FontMetrics(double emheight, double ascent, double descent, double lineheight, double exheight, double zeroWidth)
        {
            this._emh = emheight;
            this._desc = descent;
            this._asc = ascent;
            this._lineHeight = lineheight;
            this._exh = exheight;
            this._zero = zeroWidth;
        }

    }
}
