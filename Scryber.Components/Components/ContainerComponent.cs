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
using Scryber.PDF;

namespace Scryber.Components
{
    public abstract class ContainerComponent : Component, IContainerComponent //, IPDFRenderComponent
    {
        
        //
        // public properties
        //

        #region protected PDFComponentList InnerContent {get; set;} + protected virtual PDFComponentList CreateList() + public bool HasContent {get;}

        private ComponentList _children = null;

        /// <summary>
        /// Interface implemented property to contain all the Page Components that this item contains.
        /// Inheritors can make this list publicly accessible.
        /// </summary>
        protected virtual ComponentList InnerContent
        {
            get
            {
                if (this._children == null)
                    this._children = this.CreateList();
                return this._children;
            }
            set
            {
                this._children = value;
            }
        }

        protected virtual ComponentList CreateList()
        {
            return new ComponentList(this, this.Type);
        }

        /// <summary>
        /// Interface implemented property that identifies whether this page Component contains other page Components.
        /// </summary>
        public bool HasContent
        {
            get { return this._children != null && this._children.Count > 0; }
        }

        #endregion

        #region PDFComponentList IPDFContainerComponent.Content {get;set;}

        /// <summary>
        /// Interface implementation that allows all content to be accessed via the IContainerComponent interface
        /// </summary>
        ComponentList IContainerComponent.Content
        {
            get { return this.InnerContent; }
        }

        #endregion

        
        //
        //.ctor
        //

        #region .ctor(PDFObjectType)

        /// <summary>
        /// Creates a new instance of the PDFContainerComponent
        /// </summary>
        /// <param name="type"></param>
        public ContainerComponent(ObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // overrides + supporting methods
        //

        #region protected override void DoInit() + protected virtual void DoInitChildren()

        /// <summary>
        /// Performs the base initialization and then calls DoInitChildren to ensure that each of its children are initialized.
        /// </summary>
        protected override void DoInit(InitContext context)
        {
            base.DoInit(context);
            this.DoInitChildren(context);
        }

        /// <summary>
        /// Initializes all children in the document
        /// </summary>
        protected virtual void DoInitChildren(InitContext context)
        {
            if (this.HasContent)
            {
                for (int i = 0; i < this.InnerContent.Count; i++)
                {
                    Component comp = this.InnerContent[i];
                    comp.Init(context);
                }
            }
        }

        #endregion


        #region protected override void DoLoad(PDFLoadContext context) + DoLoadChildren

        protected override void DoLoad(LoadContext context)
        {
            base.DoLoad(context);
            this.DoLoadChildren(context);
        }

        protected virtual void DoLoadChildren(LoadContext context)
        {
            if (this.HasContent)
            {
                for (int i = 0; i < this.InnerContent.Count; i++)
                {
                    Component comp = this.InnerContent[i];
                    comp.Load(context);
                }
            }
        }

        #endregion


        #region protected override void DoDataBind(includeChildren) + protected virtual void DoDataBindChildren()

        /// <summary>
        /// Overrides the default implementation to optionally databind children
        /// </summary>
        /// <param name="includeChildren">Flag to identify if children should be databound too</param>
        protected override void DoDataBind(DataContext context, bool includeChildren)
        {
            base.DoDataBind(context, includeChildren);

            if(includeChildren)
                this.DoDataBindChildren(context);
        }

        /// <summary>
        /// Databinds all the children in the container
        /// </summary>
        protected virtual void DoDataBindChildren(DataContext context)
        {
            if (this.HasContent)
            {
                this.InnerContent.DataBind(context);
            }
        }

        #endregion


        #region internal override void RegisterPreRender() + RegisterPostRender() + RegisterLayoutComplete()

        internal override void RegisterPreLayout(LayoutContext context)
        {
            base.RegisterPreLayout(context);

            if (this.HasContent)
            {
                for (int i = 0; i < this.InnerContent.Count; i++)
                {
                    Component comp = this.InnerContent[i];
                    comp.RegisterPreLayout(context);
                }
            }
        }

        /// <summary>
        /// Overrides base implementation to call the inner content methods
        /// </summary>
        internal override void RegisterPreRender(RenderContext context)
        {
            base.RegisterPreRender(context);
            if (this.HasContent)
            {
                for (int i = 0; i < this.InnerContent.Count; i++)
                {
                    Component comp = this.InnerContent[i];
                    comp.RegisterPreRender(context);
                }
                
            }
        }

        /// <summary>
        /// Overrides base implementation to call the inner content methods
        /// </summary>
        internal override void RegisterPostRender(RenderContext context)
        {
            base.RegisterPostRender(context);

            if (this.HasContent)
            {
                for (int i = 0; i < this.InnerContent.Count; i++)
                {
                    Component comp = this.InnerContent[i];
                    comp.RegisterPostRender(context);
                }
            }
        }

        /// <summary>
        /// Overrides base implementation to call the inner content methods
        /// </summary>
        internal override void RegisterLayoutComplete(LayoutContext context)
        {
            base.RegisterLayoutComplete(context);
            if (this.HasContent)
            {
                for (int i = 0; i < this.InnerContent.Count; i++)
                {
                    Component comp = this.InnerContent[i];
                    comp.RegisterLayoutComplete(context);
                }
            }
        }

        #endregion

        #region  public PDFComponent FindAComponent(string id) + 1 overload

        public override Component FindAComponentById(string id)
        {
            Component found;

            if (this.ID.Equals(id))
                return this;
            else if (this.FindAComponentById(this.InnerContent, id, out found))
                return found;
            else
                return null;
        }

        

        #endregion

        #region  public PDFComponent FindAComponentByName(string name) + 1 overload

        public override Component FindAComponentByName(string name)
        {
            Component found;
            if (string.Equals(this.Name, name))
                return this;
            else if(this.HasContent && this.FindAComponentByName(this.InnerContent, name, out found))
                return found;
            else
                return base.FindAComponentByName(name);
        }

        

        #endregion

        #region internal protected override void RegisterParent(PDFComponent parent) + UnregisterParent(PDFComponent parent)

        /// <summary>
        /// Overrides base implementation to unregister child components
        /// </summary>
        /// <param name="parent"></param>
        internal protected override void UnregisterParent(Component parent)
        {
            base.UnregisterParent(parent);
            if (this.HasContent)
            {
                foreach (Component comp in this.InnerContent)
                {
                    comp.UnregisterParent(this);
                }
            }
        }

        /// <summary>
        /// Overrides base implementation to push the change to child components
        /// </summary>
        /// <param name="parent"></param>
        protected internal override void RegisterParent(Component parent)
        {
            base.RegisterParent(parent);
            if (this.HasContent)
            {
                foreach (Component comp in this.InnerContent)
                {
                    comp.RegisterParent(this);
                }
            }
        }

        #endregion



        #region protected override void Dispose(bool disposing) + protected virtual void DisposeChildren(disposing)

        /// <summary>
        /// Overrides the default implementation to dispose of the children too
        /// </summary>
        /// <param name="disposing">flag to identify if this method has been called from the Dispose method or finalize</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
                this.DisposeChildren(disposing);

            base.Dispose(disposing);
        }

        protected virtual void DisposeChildren(bool disposing)
        {
            if (disposing && this.HasContent)
            {
                foreach (Component ele in this.InnerContent)
                {
                    ele.Dispose();
                }
            }
        }

        #endregion


    }
}
