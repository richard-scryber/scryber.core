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
    [PDFParsableComponent("Label")]
    public class Label: SpanBase
    {

        private string _text;

        [PDFAttribute("text")]
        [PDFElement("")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        

        public Label()
            : this(PDFObjectTypes.Label)
        {
        }

        protected Label(ObjectType type)
            : base(type)
        {
        }

        protected override void OnPreLayout(PDFLayoutContext context)
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                this.Contents.Clear();
                this.Contents.Add(new TextLiteral(this.Text));
            }
            base.OnPreLayout(context);
        }
    }
}
