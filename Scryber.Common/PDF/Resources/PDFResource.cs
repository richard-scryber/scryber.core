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
using Scryber.PDF.Native;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// An abstract base class for all resource objects that are rendered to a PDFDocumnet, but are 
    /// </summary>
    public abstract class PDFResource : TypedObject, IPDFResource, IDisposable, IEquatable<PDFResource>
    {

        #region public static string XObjectResourceType {get;}

        /// <summary>
        /// Gets the name of the XObject (including image) resource type.
        /// </summary>
        public static string XObjectResourceType
        {
            get { return "XObject"; }
        }

        #endregion

        #region public static string FontDefnResourceType {get;}
        
        /// <summary>
        /// Gets the name of the 'Font' resource type
        /// </summary>
        public static string FontDefnResourceType
        {
            get { return "Font"; }
        }

        #endregion

        #region public static string GSStateResourceType

        /// <summary>
        /// Gets the type of an external graphics state resource
        /// </summary>
        public static string GSStateResourceType
        {
            get { return "ExtGState"; }
        }

        #endregion

        #region public static string ProcSetResourceType {get;}

        /// <summary>
        /// Returns the key for the ProcSet resource type.
        /// </summary>
        public static string ProcSetResourceType
        {
            get { return "ProcSet"; }
        }

        #endregion

        #region public static string PatternResourceType

        /// <summary>
        /// Returns the key for pattern resource types
        /// </summary>
        public static string PatternResourceType
        {
            get { return "Pattern"; }
        }

        #endregion

        //
        // properties
        //

        #region public abstract string ResourceType { get; }

        /// <summary>
        /// Gets the name of the resource type associated with this instance.
        /// Inheritors nust override this to provide their own implementation
        /// </summary>
        /// <remarks>The normal types associated with resources are declared as static properties in this class.</remarks>
        public abstract string ResourceType { get; }

        #endregion

        #region public abstract string ResourceKey { get; }

        /// <summary>
        /// Gets the key associated with this resource instance. 
        /// Inheritors must override thos to provide their own implementation
        /// </summary>
        public abstract string ResourceKey { get; }

        #endregion

        //
        // .ctor
        //

        protected PDFResource(ObjectType type)
            : base(type)
        {
        }




        #region public PDFName Name {get;set;}

        private PDFName _name;

        public PDFName Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region public bool Registered {get;}

        private bool _registered;

        public bool Registered
        {
            get { return _registered; }
        }

        #endregion

        #region public IPDFResourceContainer Container {get;set;}

        private IResourceContainer _cont;

        public virtual IResourceContainer Container
        {
            get { return _cont; }
            set { _cont = value; }
        }

        #endregion

        #region protected PDFObjectRef RenderReference {get;set;}

        private PDFObjectRef _oref = null;

        protected PDFObjectRef RenderReference
        {
            get { return _oref; }
            set { _oref = value; }
        }

        #endregion

        /// <summary>
        /// Inheritors must override this method to perform the actual render output.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected abstract PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer);


        #region public PDFObjectRef EnsureRendered(PDFContextBase context, PDFWriter writer)

        /// <summary>
        /// renders this instance to the output writer if the resource has been registered. 
        /// Resources are only rendered once, so if this resource has already been rendered it simply returns the previous reference
        /// </summary>
        /// <param name="context">The context associated with the rendering</param>
        /// <param name="writer">The writer to output any instructions to</param>
        /// <returns>A PDFObject reference for this resource</returns>
        /// <remarks>In order to ensure that this resource is output, then call the RegisterUse with a resource collection.</remarks>
        public PDFObjectRef EnsureRendered(ContextBase context, PDFWriter writer)
        {

            if (this.Registered)
            {
                if (null == this.RenderReference)
                {
                    this.RenderReference = this.DoRenderToPDF(context, writer);
                }
                return this.RenderReference;
            }
            else
                return null;
        }

        #endregion

        #region public virtual bool Equals(string resourcetype, string key)

        /// <summary>
        /// Returns true if this instances resource type and name are (Exactly) the same as the passed type and name
        /// </summary>
        /// <param name="resourcetype"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Equals(string resourcetype, string key)
        {
            return String.Equals(this.ResourceType, resourcetype) && String.Equals(this.Name, key);
        }

        #endregion

        #region public void RegisterUse(PDFResourceList resourcelist, IPDFComponent Component)

        /// <summary>
        /// Ensures that this resource is registered in the resource list.
        /// </summary>
        /// <param name="resourcelist"></param>
        /// <param name="Component"></param>
        public virtual void RegisterUse(PDFResourceList resourcelist, IComponent Component)
        {
            if (resourcelist != null)
            {
                resourcelist.EnsureInList(this);
                this.Container = resourcelist.Container;
                this._registered = true;
            }
        }

        #endregion

        #region IDisposable Implementation
        
        /// <summary>
        /// Inheritors should override this method to dispose of any unmanaged resources
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            
        }

        /// <summary>
        /// Releases any unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        ~PDFResource()
        {
            this.Dispose(false);
        }

        #endregion

        #region IEquatable<PDFResource>

        /// <summary>
        /// Returns true if this resource and the passed resource are equal (have a reference to the same resource).
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool Equals(PDFResource resource)
        {
            return this.Equals(resource.ResourceType, resource.ResourceKey);
        }

        #endregion

        #region object overrides

        /// <summary>
        /// overrides the base implementation and returns true 
        /// if this object is considered equal to the passed object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            else
                return this.Equals((PDFResource)obj);
        }

        /// <summary>
        /// Overrides the default implemenation to return a hash code of this instances string representation
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Returns a string representation of this resource
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ResourceType + "::" + this.ResourceKey;
        }

        #endregion
    }
}
