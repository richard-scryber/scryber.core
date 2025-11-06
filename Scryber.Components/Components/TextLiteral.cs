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
using Scryber.Text;
using Scryber.PDF;

namespace Scryber.Components
{
    [PDFParsableComponent("Text")]
    [PDFJSConvertor("scryber.studio.design.convertors.text")]
    public class TextLiteral : Component, IPDFTextLiteral, ITextComponent
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

        private TextFormat _reader = TextFormat.XML;

        [PDFAttribute("format")]
        public TextFormat ReaderFormat
        {
            get { return _reader; }
            set { _reader = value; }
        }

        public TextLiteral()
            : this(null, ObjectTypes.Text)
        {

        }

        public TextLiteral(string text)
            : this(text, TextFormat.Plain)
        {
        }

        public TextLiteral(string text, TextFormat format) 
            : this(text, ObjectTypes.Text)
        {
            this.ReaderFormat = format;
        }

        protected TextLiteral(string text, ObjectType type): base(type)
        {
            this.Text = text;
        }


        public virtual PDFTextReader CreateReader(ContextBase context, Style fullstyle)
        {
            if (string.IsNullOrEmpty(this.Text))
                return null;

            TextFormat format = this.ReaderFormat;
            bool preserveWhitespace = fullstyle.GetValue(StyleKeys.TextWhitespaceKey, false);
            if (preserveWhitespace)
                context.TraceLog.Add("PreFormatted","Creating a reader with preserved white space");

            return PDFTextReader.Create(this.Text, format, preserveWhitespace, context.TraceLog);
        }


        protected override void SetArrangement(ComponentArrangement arrange, PDFRenderContext context)
        {
            base.SetArrangement(arrange, context);
        }
        
    }

}
