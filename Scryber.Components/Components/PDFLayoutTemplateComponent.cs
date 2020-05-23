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
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Components
{

    /// <summary>
    /// Abstract base class for all components that can instantiate and contian components generated from an IPDFTempate 
    /// at the time of layout (rather than at binding).
    /// </summary>
    public abstract class PDFLayoutTemplateComponent : PDFVisualComponent, IPDFViewPortComponent
    {

        public PDFLayoutTemplateComponent(PDFObjectType type) : base(type)
        {
        }

        private int _generationIndex = 0;

        protected int GeneratedCount
        {
            get { return _generationIndex; }
            set { _generationIndex = value; }
        }

        public void InstantiateTemplate(IPDFTemplate template, PDFLayoutContext context, PDFRect available, int pageindex)
        {
            if (null == template)
                throw new ArgumentNullException("template");
            if (null == context)
                throw new ArgumentNullException("context");

            List<IPDFComponent> generated = new List<IPDFComponent>(template.Instantiate(GeneratedCount, this));

            if (generated.Count == 0)
                return;

            PDFInitContext init = new PDFInitContext(context.Items, context.TraceLog, context.PerformanceMonitor);
            PDFLoadContext load = new PDFLoadContext(context.Items, context.TraceLog, context.PerformanceMonitor);
            PDFDataContext data = new PDFDataContext(context.Items, context.TraceLog, context.PerformanceMonitor);


            IPDFContainerComponent container = this;
            IPDFComponentList components = container.Content as IPDFComponentList;

            for (int index = 0; index < generated.Count; index++)
            {
                IPDFComponent comp = generated[index];
                components.Insert(index, comp);
                comp.Init(init);
            }

            foreach (IPDFComponent comp in generated)
            {
                comp.Load(load);
            }
            foreach (IPDFComponent comp in generated)
            {
                if (comp is IPDFBindableComponent)
                    (comp as IPDFBindableComponent).DataBind(data);
            }
            this.GeneratedCount++;
        }
        


        #region IPDFViewPortComponent Members

        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, PDFStyle style)
        {
            return new Layout.LayoutEnginePanel(this, parent);
        }

        #endregion
    }

    
    
}
