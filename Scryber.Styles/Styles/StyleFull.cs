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
using Scryber.Drawing;

namespace Scryber.Styles
{
    /// <summary>
    /// A full style is built and returned from the StyleStack, 
    /// and holds references to the position, font and text options - for quick access.
    /// </summary>
    public class StyleFull : Style
    {

        private Scryber.PDFPositionOptions _pos;
        private Scryber.PageSize _pgsize;
        private Scryber.PDFTextRenderOptions _text;
        private Scryber.PDFPageNumberOptions _pageNums;
        private Scryber.Drawing.PDFPenBorders _borders;


        public StyleFull()
            : base()
        {
            
        }

        protected override void DoClear()
        {
            this.ClearFullRefs();
            base.DoClear();
        }

        public void ClearFullRefs()
        {
            this._pos = null;
            this._pgsize = null;
            this._text = null;
            this._pageNums = null;
            this._borders = null;
        }

        protected override void BeginStyleChange()
        {
            this._pos = null;
            this._pgsize = null;
            this._text = null;
            this._pageNums = null;
            this._borders = null;

            base.BeginStyleChange();
        }

        internal protected override Scryber.PDFPositionOptions DoCreatePositionOptions()
        {
            if(null == _pos)
                _pos = base.DoCreatePositionOptions();

            return _pos;
        }

        internal protected override Scryber.PDFTextRenderOptions DoCreateTextOptions()
        {
            if(null == _text)
                _text = base.DoCreateTextOptions();


            return _text;
        }

        protected internal override PDFPenBorders DoCreatePenBorders()
        {
            if (null == _borders)
                _borders = base.DoCreatePenBorders();
            return _borders;
        }

        internal protected override Scryber.PageSize DoCreatePageSize()
        {
            if(null == _pgsize)
                _pgsize = base.DoCreatePageSize();

            return _pgsize;
        }

        internal protected override PDFPageNumberOptions DoCreatePageNumberOptions()
        {
            if(null == _pageNums)
                _pageNums = base.DoCreatePageNumberOptions();

            return _pageNums;
        }

        protected internal override Drawing.PDFThickness DoCreateMarginsThickness()
        {
            if (null != this._pos)
                return this._pos.Margins;
            else
                return base.DoCreateMarginsThickness();
        }

        protected internal override Drawing.PDFThickness DoCreatePaddingThickness()
        {
            if (null != this._pos)
                return this._pos.Padding;
            else
                return base.DoCreatePaddingThickness();
        }

        protected internal override Drawing.PDFThickness DoCreateClippingThickness()
        {
            if (null != this._pos)
                return this._pos.ClipInset;
            else
                return base.DoCreateClippingThickness();
        }
        
    }
}
