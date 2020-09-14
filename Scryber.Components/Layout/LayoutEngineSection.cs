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
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.Layout
{
    internal class LayoutEngineSection : LayoutEnginePage
    {
        Section _section;
        private int _startpageindex;
        

        protected Section Section
        {
            get { return _section; }
        }

        internal LayoutEngineSection(Section section, IPDFLayoutEngine parent)
            : base(section, parent)
        {
            _section = section;
        }

        protected override void DoLayoutComponent()
        {
            this._startpageindex = this.Context.DocumentLayout.CurrentPageIndex + 1; //we don't have a page yet
            base.DoLayoutComponent();
        }

        protected override IPDFTemplate GetCurrentFooterTemplate(int pageIndex)
        {
            if (pageIndex == _startpageindex)
                return base.GetCurrentFooterTemplate(pageIndex);

            else if (Section.ContinuationFooter == null)
                return base.GetCurrentFooterTemplate(pageIndex);
            else
                return Section.ContinuationFooter;
        }

        protected override IPDFTemplate GetCurrentHeaderTemplate(int pageIndex)
        {
            if (pageIndex == _startpageindex)
                return base.GetCurrentHeaderTemplate(pageIndex);
            else if (Section.ContinuationHeader == null)
                return base.GetCurrentHeaderTemplate(pageIndex);
            else
                return Section.ContinuationHeader;
        }
       
    }
}
