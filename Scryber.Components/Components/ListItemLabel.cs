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

namespace Scryber.Components
{
    [PDFParsableComponent("Li-Label")]
    public class ListItemLabel : TextLiteral
    {

        public ListItemLabel(string text)
            : base(text)
        {
            this.ElementName = "lbl";
        }

        public ListItemLabel() : this(null)
        {
        }

        protected override Styles.Style GetBaseStyle()
        {
            Styles.Style style = base.GetBaseStyle();
            return style;
        }
    }

    [PDFParsableComponent("Li-Bullet")]
    public class PDFListBulletItemLabel : ListItemLabel
    {
        public PDFListBulletItemLabel(string text)
            : base(text)
        {
            this.ElementName = "lbl";
        }

        public PDFListBulletItemLabel()
            : this(null)
        {
        }

        //protected override Styles.PDFStyle GetBaseStyle()
        //{
        //    Styles.PDFStyle style = base.GetBaseStyle();
        //    style.Font.FontFamily = Const.DefaultBulletFont;

        //    return style;
        //}
    }

    [PDFParsableComponent("Di-Label")]
    public class PDFListDefinitionItemLabel : ListItemLabel
    {

        public PDFListDefinitionItemLabel(string text)
            : base(text)
        {
            this.ElementName = "lbl";
        }

        public PDFListDefinitionItemLabel()
            : this(null)
        {
        }

        protected override Styles.Style GetBaseStyle()
        {
            Styles.Style style = base.GetBaseStyle();
            style.Font.FontBold = true;

            return style;
        }
    }
}
