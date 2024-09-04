﻿/*  Copyright 2012 PerceiveIT Limited
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
    [PDFParsableComponent("Di")]
    public abstract class ListDefinitionItemBase : Panel
    {

        [PDFElement()]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }


        protected ListDefinitionItemBase(ObjectType type)
            : base(type)
        {
        }

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Size.FullWidth = true;
            style.Position.DisplayMode = Drawing.DisplayMode.Block;
            return style;
        }
    }

    [PDFParsableComponent("Dt")]
    public class ListDefinitionTerm : ListDefinitionItemBase
    {

        public ListDefinitionTerm() : this(ObjectTypes.DefinitionListTerm)
        {

        }

        protected ListDefinitionTerm(ObjectType type) : base(type)
        { }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Font.FontBold = true;
            return style;
        }
    }

    [PDFParsableComponent("Dd")]
    public class ListDefinitionItem : ListDefinitionItemBase
    {

        public ListDefinitionItem() : this(ObjectTypes.DefinitionListItem)
        {

        }

        protected ListDefinitionItem(ObjectType type) : base(type)
        { }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Margins.Left = 40;
            return style;
        }

        //protected override IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        //{
        //    return base.CreateLayoutEngine(parent, context, style);
        //}
    }

    public class DefinitionItemList : ComponentWrappingList<ListDefinitionItemBase>
    {

        #region .ctor(PDFComponentList)

        /// <summary>
        /// Creates the list of list items based on the provided component list. 
        /// </summary>
        /// <param name="inner"></param>
        public DefinitionItemList(ComponentList inner)
            : base(inner)
        {
        }

        #endregion
    }
}
