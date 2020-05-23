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
using Scryber.Styles;

namespace Scryber.Components
{

    public class PDFSpanBase : PDFPanel
    {
        public PDFSpanBase()
            : this(PDFObjectTypes.Span)
        {
        }

        protected PDFSpanBase(PDFObjectType type)
            : base(type)
        {
        }

        

        protected override PDFStyle GetBaseStyle()
        {
            Styles.PDFStyle inline = base.GetBaseStyle();
            inline.Position.PositionMode = Drawing.PositionMode.Inline;
            return inline;
        }

    }

    [PDFParsableComponent("Span")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_span")]
    public class PDFSpan : PDFSpanBase
    {
        [PDFElement()]
        [PDFArray(typeof(PDFComponent))]
        public override PDFComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        public PDFSpan()
            : base()
        {
        }


        protected override PDFStyle GetBaseStyle()
        {
            return base.GetBaseStyle();
        }

    }

    [PDFParsableComponent("B")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_bold")]
    public class PDFBoldSpan : PDFSpanBase
    {
        [PDFElement()]
        [PDFArray(typeof(PDFComponent))]
        public override PDFComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        public PDFBoldSpan()
            : base()
        {
        }


        protected override PDFStyle GetBaseStyle()
        {
            Styles.PDFStyle bold = base.GetBaseStyle();
            bold.Font.FontBold = true;
            return bold;
        }

    }

    [PDFParsableComponent("I")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_italic")]
    public class PDFItalicSpan : PDFSpanBase
    {
        [PDFElement()]
        [PDFArray(typeof(PDFComponent))]
        public override PDFComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        public PDFItalicSpan()
            : base()
        {
        }

        protected override PDFStyle GetBaseStyle()
        {
            Styles.PDFStyle bold = base.GetBaseStyle();
            bold.Font.FontItalic = true;
            return bold;
        }
    }

    [PDFParsableComponent("U")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_underline")]
    public class PDFUnderlinedSpan : PDFSpanBase
    {
        [PDFElement()]
        [PDFArray(typeof(PDFComponent))]
        public override PDFComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        public PDFUnderlinedSpan()
            : base()
        {
        }

        protected override PDFStyle GetBaseStyle()
        {
            Styles.PDFStyle under = base.GetBaseStyle();
            under.Text.Decoration |= Text.TextDecoration.Underline;
            return under;
        }
    }

}
