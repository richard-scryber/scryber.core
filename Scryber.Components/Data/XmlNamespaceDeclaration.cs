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

namespace Scryber.Data
{
    [PDFParsableComponent("Xmlns")]
    public class XmlNamespaceDeclaration : IPDFBindableComponent
    {

        //
        // events
        //

        #region public event PDFDataBindEventHandler DataBinding;

        /// <summary>
        /// Event that is raised when this this namespace declaration is databinding
        /// </summary>
        public event PDFDataBindEventHandler DataBinding;

        /// <summary>
        /// Raises the DataBinding event if something is registered to it
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnDataBinding(PDFDataContext context)
        {
            if (null != this.DataBinding)
                this.DataBinding(this, new PDFDataBindEventArgs(context));
        }

        #endregion

        #region public event PDFDataBindEventHandler DataBound;

        /// <summary>
        /// Event that is raised when this this namespace declaration has been databound
        /// </summary>
        public event PDFDataBindEventHandler DataBound;

        /// <summary>
        /// Raises the DataBound event if something is registered to it
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnDataBound(PDFDataContext context)
        {
            if (null != this.DataBound)
                this.DataBound(this, new PDFDataBindEventArgs(context));
        }

        #endregion

        //
        // properties
        //

        #region public string Prefix {get;set;}

        private string _prefix;
        
        [PDFAttribute("prefix")]
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        #endregion

        #region public string NamespaceURI {get;set;}

        private string _nameSpace;

        [PDFAttribute("namespace")]
        public string NamespaceURI
        {
            get { return _nameSpace; }
            set { _nameSpace = value; }
        }

        #endregion

        //
        // ctor
        //

        #region public PDFXmlNamespaceDeclaration()

        /// <summary>
        /// Creates a new instance of the PDFXMLNamespaceDeclaration
        /// </summary>
        public XmlNamespaceDeclaration()
        {
        }

        #endregion

        //
        // methods
        //

        #region public void DataBind(PDFDataContext context)

        /// <summary>
        /// Implements the databinding capability of the Namespace declaration
        /// </summary>
        /// <param name="context"></param>
        public void DataBind(PDFDataContext context)
        {
            this.OnDataBinding(context);
            this.DoDataBind(context);
            this.OnDataBound(context);
        }

        #endregion

        #region protected virtual void DoDataBind(PDFDataContext context)

        /// <summary>
        /// Inheritors can override this method to perform further actions during databinding
        /// </summary>
        /// <param name="context"></param>
        protected virtual void DoDataBind(PDFDataContext context)
        {
        }

        #endregion
    }


    /// <summary>
    /// A collection of PDFXmlNamespaceDeclarations
    /// </summary>
    public class PDFXmlNamespaceCollection : List<XmlNamespaceDeclaration>
    {


        /// <summary>
        /// Databinds all the NamespaceDeclarations in this collection
        /// </summary>
        /// <param name="context"></param>
        public void DataBind(PDFDataContext context)
        {
            if (this.Count > 0)
            {
                foreach (XmlNamespaceDeclaration dec in this)
                {
                    dec.DataBind(context);
                }
            }
        }
    }
}
