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
using Scryber.PDF;

namespace Scryber.Components
{

    /// <summary>
    /// Abstract base class for all components that can instantiate and contian components generated from an IPDFTempate 
    /// at the time of layout (rather than at binding).
    /// </summary>
    public abstract class LayoutTemplateComponent : VisualComponent, IPDFViewPortComponent
    {

        public LayoutTemplateComponent(ObjectType type) : base(type)
        {
        }

        private int _generationIndex = 0;

        protected int GeneratedCount
        {
            get { return _generationIndex; }
            set { _generationIndex = value; }
        }

        public void InstantiateTemplate(ITemplate template, PDFLayoutContext context, Rect available, int pageindex)
        {
            if (null == template)
                throw new ArgumentNullException("template");
            if (null == context)
                throw new ArgumentNullException("context");

            List<IComponent> generated = new List<IComponent>(template.Instantiate(GeneratedCount, this));

            if (generated.Count == 0)
                return;

            InitContext init = new InitContext(context.Items, context.TraceLog, context.PerformanceMonitor, this.Document)
            {
                Compression = context.Compression,
                OutputFormat = context.OutputFormat,
                Conformance = context.Conformance
            };

            LoadContext load = new LoadContext(context.Items, context.TraceLog, context.PerformanceMonitor, this.Document)
            {
                Compression = context.Compression,
                OutputFormat = context.OutputFormat,
                Conformance = context.Conformance
            };

            DataContext data = new DataContext(context.Items, context.TraceLog, context.PerformanceMonitor, this.Document)
            {
                Compression = context.Compression,
                OutputFormat = context.OutputFormat,
                Conformance = context.Conformance
            };


            IContainerComponent container = this;
            IComponentList components = container.Content as IComponentList;

            for (int index = 0; index < generated.Count; index++)
            {
                IComponent comp = generated[index];
                components.Insert(index, comp);
                comp.Init(init);
            }

            foreach (IComponent comp in generated)
            {
                comp.Load(load);
            }
            foreach (IComponent comp in generated)
            {
                if (comp is IBindableComponent)
                    (comp as IBindableComponent).DataBind(data);
            }
            this.GeneratedCount++;
        }
        


        #region IPDFViewPortComponent Members

        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return new PDF.Layout.LayoutEnginePanel(this, parent);
        }

        #endregion
    }

    
    
}
