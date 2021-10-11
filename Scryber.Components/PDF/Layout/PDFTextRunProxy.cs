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
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// The Text run proxy is a run of characters, but the characters are held in the proxy op, rather than in this run itself.
    /// </summary>
    public class PDFTextRunProxy : PDFTextRun
    {
        #region ivars

        private Size _measuredSize;
        private Scryber.Text.PDFTextProxyOp _proxy;

        #endregion

        #region public override PDFUnit Width {get;}

        /// <summary>
        /// Gets the width of this character run
        /// </summary>
        public override Unit Width
        {
            get { return this._measuredSize.Width; }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Gets the height of this character run
        /// </summary>
        public override Unit Height
        {
            get { return this._measuredSize.Height; }
        }

        #endregion

        #region public Scryber.Text.PDFTextProxyOp Proxy {get;}

        /// <summary>
        /// Gets the proxy that provides the characters in this run
        /// </summary>
        public Scryber.Text.PDFTextProxyOp Proxy
        {
            get { return _proxy; }
        }

        #endregion


        public PDFTextRunProxy(Size size, Scryber.Text.PDFTextProxyOp proxy, PDFLayoutLine line, IComponent owner)
            : base(line, owner)
        {
            this._measuredSize = size;
            this._proxy = proxy;
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Unit xoffset, Unit yoffset)
        {
            base.DoPushComponentLayout(context, pageIndex, xoffset, yoffset);
        }

        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            context.Graphics.FillText(this.Proxy.Text);
            return null;
        }
    }
}
