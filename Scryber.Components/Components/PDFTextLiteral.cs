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
using System.Drawing;
using Scryber.Styles;
using Scryber.Native;
using Scryber.Text;

namespace Scryber.Components
{
    [PDFParsableComponent("Text")]
    [PDFJSConvertor("scryber.studio.design.convertors.text")]
    public class PDFTextLiteral : PDFComponent, IPDFTextLiteral, IPDFTextComponent
    {

        private string _text;
        
        /// <summary>
        /// The text value of this literal
        /// </summary>
        [PDFAttribute("value")]
        [PDFElement()]
        [PDFDesignable("Value", Category = "General", Priority = 2, Type = "String")]
        public string Text
        {
            get
            {
                return _text;
            }
            set 
            {
                _text = value;
            }
        }

        private TextFormat _reader = TextFormat.Plain;

        [PDFAttribute("format")]
        public TextFormat ReaderFormat
        {
            get { return _reader; }
            set { _reader = value; }
        }

        public PDFTextLiteral()
            : this(null, PDFObjectTypes.Text)
        {

        }

        public PDFTextLiteral(string text)
            : this(text, PDFObjectTypes.Text)
        {
        }

        public PDFTextLiteral(string text, TextFormat format) 
            : this(text, PDFObjectTypes.Text)
        {
            this.ReaderFormat = format;
        }

        protected PDFTextLiteral(string text, PDFObjectType type): base(type)
        {
            this.Text = text;
        }


        public virtual PDFTextReader CreateReader(PDFLayoutContext context, PDFStyle fullstyle)
        {
            TextFormat format = this.ReaderFormat;
            bool preserveWhitespace = fullstyle.GetValue(PDFStyleKeys.TextWhitespaceKey, false);

            return PDFTextReader.Create(this.Text, format, preserveWhitespace, context.TraceLog);
        }


        protected override void SetArrangement(PDFComponentArrangement arrange)
        {
            base.SetArrangement(arrange);
        }
        
    }

}
