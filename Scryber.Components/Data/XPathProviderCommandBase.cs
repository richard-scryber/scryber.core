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
using System.Xml.XPath;

namespace Scryber.Data
{
    public abstract class XPathProviderCommandBase : Scryber.Components.Component
    {

        
        //
        // ctor
        //
        

        public XPathProviderCommandBase(PDFObjectType type)
            : base(type)
        {
        }


        #region public virtual object GetNullValue()

        /// <summary>
        /// Returns the appropriate value for NULL on this provider.
        /// Inheritors can override to provide their own implementation.
        /// </summary>
        /// <returns></returns>
        public virtual object GetNullValue()
        {
            return null;
        }

        #endregion

        #region public void DataBind(PDFDataContext context) + DoDataBind(PDFDataContext context)

       
        
        /// <summary>
        /// Inheritors can override this method to perform their own actions during databinding
        /// </summary>
        /// <param name="context"></param>
        protected override void DoDataBind(PDFDataContext context, bool includeChildren)
        {
        }

        #endregion

        #region public void EnsureDataLoaded(PDFDataSourceBase source System.Data.DataSet dataSet, PDFDataContext context)

        /// <summary>
        /// Loads the data from this command and returns as an XPath Navigator
        /// </summary>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void EnsureDataLoaded(XPathDataSourceBase source, System.Data.DataSet dataSet, PDFDataContext context)
        {
            try
            {
                this.DoEnsureDataLoaded(source, dataSet, context);
            }
            catch(Exception ex)
            {
                if (context.Conformance == ParserConformanceMode.Lax)
                {
                    context.TraceLog.Add(TraceLevel.Error, "XPath Provider Command", ex.Message, ex);
                    context.TraceLog.Add(TraceLevel.Error, "XPath Provider Command", string.Format("Could not load the data for command '{0}' in source '{1}'. Returning null instead.", this.ID, source.ID), ex);
                }
                else
                    throw;
            }
            
        }

        #endregion

        /// <summary>
        /// Abstract method that inheritors must implement to load their own data
        /// </summary>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract void DoEnsureDataLoaded(XPathDataSourceBase source, System.Data.DataSet dataSet, PDFDataContext context);


    }


    public class XPathProviderCommandList : Scryber.Components.ComponentWrappingList<XPathProviderCommandBase>
    {

        #region public PDFProviderCommand this[string name]

        /// <summary>
        /// Gets any command with the specified name or returns null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public XPathProviderCommandBase this[string name]
        {
            get
            {
                if (this.Count > 0)
                {
                    foreach (XPathProviderCommandBase cmd in this)
                    {
                        if (string.Equals(cmd.ID, name))
                            return cmd;
                    }
                }
                return null;
            }
        }

        #endregion

        #region public PDFProviderCommand Default {get;}

        /// <summary>
        /// Gets the default command in this collection (where the command name is null or empty)
        /// </summary>
        public XPathProviderCommandBase Default
        {
            get
            {
                if (this.Count > 0)
                {
                    foreach (XPathProviderCommandBase cmd in this)
                    {
                        if (string.IsNullOrEmpty(cmd.ID))
                            return cmd;
                    }
                }
                return null;
            }
        }

        #endregion

        //
        // ctor
        //


        public XPathProviderCommandList(XPathDataSourceBase parentSource) : base(((IPDFContainerComponent)parentSource).Content)
        {
        }

        //
        // methods
        //


        public void DataBind(PDFDataContext context)
        {
            if(this.Count > 0)
            {
                foreach (XPathProviderCommandBase cmd in this)
                {
                    cmd.DataBind(context);
                }
            }
        }
        
    }
}
