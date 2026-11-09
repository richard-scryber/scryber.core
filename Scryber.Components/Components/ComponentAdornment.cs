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

    /// <summary>
    /// Base class for the header and footer components in another component - e.g. Section or Main
    /// </summary>
    public class ComponentAdornment : LayoutTemplateComponent
    {
        public ComponentAdornment(ObjectType type)
            : base(type)
        {
        }

        protected override Styles.Style GetBaseStyle()
        {
            Styles.Style style = base.GetBaseStyle();
            style.Size.FullWidth = true;

            return style;
        }
    }


    /// <summary>
    /// The PDFPageHeader is a template container for any components that have repeating headers and footers
    /// It is a parsable PDF component, but will only act like a block in the content of a page
    /// </summary>
    [PDFParsableComponent("ComponentHeader")]
    public class ComponentHeader : ComponentAdornment
    {
        public ComponentHeader() : base(ObjectTypes.ComponentHeader) { }

    }


    /// <summary>
    /// The ComponentFooter is a template container for any components that have repeating headers and footers.
    /// It is a parsable component
    /// </summary>
    [PDFParsableComponent("ComponentFooter")]
    public class ComponentFooter : ComponentAdornment
    {
        public ComponentFooter() : base(ObjectTypes.ComponentFooter) { }
    }
}
