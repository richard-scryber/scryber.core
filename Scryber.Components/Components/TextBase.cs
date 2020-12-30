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
using Scryber.Styles;
using Scryber.Resources;
using Scryber.Text;
using Scryber.Drawing;
using Scryber.Native;

namespace Scryber.Components
{
    public abstract class TextBase : VisualComponent
                                        , IPDFTextComponent
                                        , IPDFViewPortComponent
    {

        

        #region protected virtual string BaseText {get;set;}

        private string _text;

        protected virtual string BaseText
        {
            get { return _text; }
            set { _text = value;}
        }

        #endregion

        #region protected .ctor(type)

        protected TextBase(PDFObjectType type)
            : base(type)
        {
        }

        #endregion

        #region protected virtual PDFTextReader CreateReader() + Interface implementation

        protected virtual PDFTextReader CreateReader(PDFLayoutContext context, Style fullstyle)
        {
            TextFormat format = TextFormat.Plain;
            bool preserveWhitespace = fullstyle.GetValue(StyleKeys.TextWhitespaceKey, false);


            if (this is IPDFTextLiteral)
                format = ((IPDFTextLiteral)this).ReaderFormat;
            
            return PDFTextReader.Create(this.BaseText, format, preserveWhitespace, context.TraceLog);
        }

        PDFTextReader IPDFTextComponent.CreateReader(PDFLayoutContext context, Style fullstyle)
        {
            return this.CreateReader(context, fullstyle);
        }

        #endregion

        #region protected override PDFStyle GetBaseStyle()

        /// <summary>
        /// Overrides the base implementation to apply an inline style to the text base. 
        /// Inheritors can override to apply their own styles too.
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            Style def = base.GetBaseStyle();
            def.Position.PositionMode = Scryber.Drawing.PositionMode.Inline;

            return def;
        }

        #endregion

        /// <summary>
        /// Custom implementaton of the base text class.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        /// <returns></returns>
        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style fullstyle)
        {
            return new Layout.LayoutEngineText(this, parent);
        }
    }
}
