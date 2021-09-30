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
    
    public abstract class XsltArgumentBase : IBindableComponent
    {

        //
        // events to support databinding
        //

        #region public event PDFDataBindEventHandler DataBinding;

        public event PDFDataBindEventHandler DataBinding;

        protected virtual void OnDataBinding(PDFDataContext context)
        {
            if (null != this.DataBinding)
                this.DataBinding(this, new PDFDataBindEventArgs(context));
        }

        #endregion

        #region public event PDFDataBindEventHandler DataBound;

        public event PDFDataBindEventHandler DataBound;

        protected virtual void OnDataBound(PDFDataContext context)
        {
            if (null != this.DataBound)
                this.DataBound(this, new PDFDataBindEventArgs(context));
        }

        #endregion

        //
        // properties
        //

        #region public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of this argument
        /// </summary>
        [PDFAttribute("name")]
        public string Name { get; set; }

        #endregion

        #region public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the namespace uri of this argument
        /// </summary>
        [PDFAttribute("namespace")]
        public string Namespace { get; set; }

        #endregion

        #region public string FullName {get;}

        /// <summary>
        /// Gets the fully qualified name of the argument including the Namespace if set.
        /// </summary>
        public string FullName
        {
            get
            {
                return GetFullName(this);
            }
        }

        public static string GetFullName(XsltArgumentBase arg)
        {
            if (null == arg)
                throw new ArgumentNullException("arg");

            return GetFullName(arg.Namespace, arg.Name);
        }

        public static string GetFullName(string ns, string name)
        {
            string full;
            if (string.IsNullOrEmpty(ns) == false)
            {
                full = ns;
                full += "+";
                full += name;
            }
            else
                full = name;

            return full;
        }

        #endregion

        //
        // methods
        //

        /// <summary>
        /// Abstract method that all inheriting classes must implement in order to return the actual value of the Argument
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract object GetValue(PDFDataContext context);


        #region public void DataBind(PDFDataContext context) + DoDataBind(PDFDataContext context)

        /// <summary>
        /// Supports the databinding capabilites of the parameter by raising the events
        /// </summary>
        /// <param name="context"></param>
        public virtual void DataBind(PDFDataContext context)
        {
            this.OnDataBinding(context);
            this.DoDataBind(context);
            this.OnDataBound(context);

        }

        /// <summary>
        /// Inheritors can override this method to perform their own actions during databinding
        /// </summary>
        /// <param name="context"></param>
        protected virtual void DoDataBind(PDFDataContext context)
        {
        }

        #endregion
    }



    /// <summary>
    /// A list of defined xslt arguments
    /// </summary>
    public class XsltArgumentList : List<XsltArgumentBase>
    {
        #region public void DataBind(PDFDataContext context)

        /// <summary>
        /// Binds each of the XSLTArguments in this list
        /// </summary>
        /// <param name="context"></param>
        public void DataBind(PDFDataContext context)
        {
            if (this.Count > 0)
            {
                foreach (XsltArgumentBase arg in this)
                {
                    arg.DataBind(context);
                }
            }
        }

        #endregion
    }


    //
    // Concrete argument classes
    //

    #region public class PDFXsltArgument : PDFXsltArgumentBase

    /// <summary>
    /// An XsltArgument that has a string value
    /// </summary>
    [PDFParsableComponent("XsltArgument")]
    public class XsltArgument : XsltArgumentBase
    {
        [PDFAttribute("value")]
        [PDFElement("")]
        public string Value
        {
            get;
            set;
        }

        public override object GetValue(PDFDataContext context)
        {
            return Value;
        }
    }

    #endregion

    #region public class PDFXsltQueryStringArgument : PDFXsltArgumentBase

    /// <summary>
    /// An XsltArgument that takes it's value from the query string
    /// </summary>
    [PDFParsableComponent("XsltQSArgument")]
    public class PDFXsltQueryStringArgument : XsltArgumentBase
    {
        [PDFAttribute("default-value")]
        [PDFElement("")]
        public string DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// Optional query string parameter name. If not set then the base name of 
        /// this parameter will be used.
        /// </summary>
        [PDFAttribute("param-name")]
        public string QSParamName
        {
            get;
            set;
        }

        public override object GetValue(PDFDataContext context)
        {
            throw new NotSupportedException("Not Supported in .Net Core");
            //string name = this.QSParamName;
            //if (string.IsNullOrEmpty(name))
            //    name = this.Name;

            //System.Web.HttpContext webcontext = System.Web.HttpContext.Current;
            //if (null == webcontext)
            //    throw new PDFDataException(Errors.NoWebContextAvailable);

            //System.Collections.Specialized.NameValueCollection qs = webcontext.Request.QueryString;
            //string value = qs[name];
            //if (string.IsNullOrEmpty(value))
            //    value = this.DefaultValue;
            //return value;
        }
    }

    #endregion

    #region public class PDFXsltContextItemArgument : PDFXsltArgumentBase

    /// <summary>
    /// An XsltArgument that takes it's value from an Item value in the Items collection
    /// </summary>
    [PDFParsableComponent("XsltItemArgument")]
    public class PDFXsltContextItemArgument : XsltArgumentBase
    {
        [PDFAttribute("default-value")]
        [PDFElement("")]
        public string DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// Optional context item name. If not set then the base name of 
        /// this parameter will be used.
        /// </summary>
        [PDFAttribute("item-name")]
        public string ItemName
        {
            get;
            set;
        }

        public override object GetValue(PDFDataContext context)
        {
            string name = this.ItemName;
            if (string.IsNullOrEmpty(name))
                name = this.Name;

            
            object value = context.Items[name];

            if (null == value)
                value = this.DefaultValue;

            return value;
        }
    }

    #endregion
}
