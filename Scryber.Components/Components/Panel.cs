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

// Uncomment this pre-processor directive to paint the column sizes
//#define SHOW_COLUMN_DIMENSIONS

using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF.Layout;
using Scryber.PDF;

namespace Scryber.Components
{
    /// <summary>
    /// Base class of all visual component containers
    /// </summary>
    [PDFParsableComponent("Panel")]
    public class Panel : VisualComponent, IPDFViewPortComponent
    {
        

        //
        // properties
        //

        #region public PDFComponentList Contents {get;}

        /// <summary>
        /// Gets the content collection of page Components in this panel
        /// </summary>
        public virtual ComponentList Contents
        {
            get
            {
                return this.InnerContent;
            }
        }

        #endregion

        #region public OverflowAction OverflowAction {get;set;}
        
        /// <summary>
        /// Gets or sets the overflow action for this panel
        /// </summary>
        [PDFAttribute("overflow-action", Styles.Style.PDFStylesNamespace)]
        [PDFDesignable(Ignore = true)]
        public OverflowAction OverflowAction
        {
            get
            {
                if (this.HasStyle)
                {
                    return this.Style.GetValue(StyleKeys.OverflowActionKey, OverflowAction.None);
                }
                else
                    return Drawing.OverflowAction.None;
            }
            set
            {
                this.Style.SetValue(StyleKeys.OverflowActionKey, value);
            }
        }

        #endregion

        #region public PDFThickness ClippingInset {get;set;}

        /// <summary>
        /// Gets or sets the inset of the clipping rectangle in relation to the panel contents
        /// </summary>
        [PDFAttribute("clipping-inset", Styles.Style.PDFStylesNamespace)]
        [PDFDesignable(Ignore = true)]
        public Thickness ClippingInset
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.CreateClippingThickness();
                else
                    return Thickness.Empty();
            }
            set
            {
                this.Style.Clipping.SetThickness(value);
            }
        }

        #endregion

       
        //
        // .ctors
        //

        public Panel()
            : this(ObjectTypes.Panel)
        {
        }

        public Panel(ObjectType type)
            : base(type)
        {

        }
        
        /// <summary>
        /// Overrides the base style to set the position mode as a block
        /// </summary>
        /// <returns></returns>
        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Position.PositionMode = Drawing.PositionMode.Static;
            style.Position.DisplayMode = DisplayMode.Block;
            return style;
        }
        

        #region IPDFViewPortComponent Members

        IPDFLayoutEngine IPDFViewPortComponent.GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return this.CreateLayoutEngine(parent, context, style);
        }

        protected virtual IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return new LayoutEnginePanel(this, parent);
        }

        #endregion

        protected override void SetArrangement(ComponentArrangement arrange)
        {
            base.SetArrangement(arrange);
        }

        
    }


    
}
