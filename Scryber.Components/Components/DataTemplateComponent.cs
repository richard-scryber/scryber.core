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

namespace Scryber.Components
{
    /// <summary>
    /// Adds a DataSource and select path to the template Component so that it can add new sources
    /// to the context before binding.
    /// </summary>
    public abstract class DataTemplateComponent : BindingTemplateComponent
    {

        #region public string DataSourceID {get;set;}

        private string _srcid;

        /// <summary>
        /// Gets or sets the ID of a Data Source Component such as PDFXMLDataSource. 
        /// Cannot be set if there is an existing 'DataSource' value.
        /// </summary>
        [PDFAttribute("datasource-id")]
        [PDFDesignable("Data Source", Category = "Data", Priority = 2, Type = "SelectDataSource")]
        public string DataSourceID
        {
            get { return _srcid; }
            set
            {
                _srcid = value;
            }
        }

        #endregion

        #region public string SelectPath {get;set;}

        private string _sourcepath;
        /// <summary>
        /// Gets or sets any XPath expression to use to extract the results
        /// </summary>
        [PDFAttribute("select")]
        [PDFDesignable("Select", Priority = 2, Category = "Data", Type = "SelectDataPicker", JSOptions = "{\"context\":\"@datasource-id\",\"type\":\"Collection\"}")]
        public string SelectPath
        {
            get { return _sourcepath; }
            set { _sourcepath = value; }
        }


        #endregion


        public DataTemplateComponent(PDFObjectType type)
            : base(type)
        {
        }

        /// <summary>
        /// If this component has a specified datasourceId then the component will be loaded and set to the out parameter. The method will then return true.
        /// If this component does not have a datasourceId then method will return false.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="datasourceComponent"></param>
        /// <returns></returns>
        protected bool HasAssignedDataSourceComponent(PDFDataContext context, out IPDFDataSource datasourceComponent)
        {
            if (string.IsNullOrEmpty(this.DataSourceID) == false)
            {
                datasourceComponent = GetDataSourceComponent(this.DataSourceID, context);
                return null != datasourceComponent;
            }
            else
            {
                datasourceComponent = null;
                return false;
            }
        }

        /// <summary>
        /// Looks up and returns the IPDFDataSource based on the provided datasourceComponentID. Throws an expcetion if the datasourceComponentID is null or the component cannot be found
        /// </summary>
        /// <param name="datasourceComponentID">The ID of the IPDFDataSource</param>
        /// <param name="context">The current data context</param>
        /// <returns></returns>
        protected IPDFDataSource GetDataSourceComponent(string datasourceComponentID, PDFDataContext context)
        {
            IPDFDataSource datasourceComponent = null;

            if (string.IsNullOrEmpty(datasourceComponentID))
                throw new ArgumentNullException("datasourceComponentID");

            Component found = base.FindDocumentComponentById(datasourceComponentID);
            if (found == null)
                throw RecordAndRaise.ArgumentNull("DataSourceID", Errors.CouldNotFindControlWithID, datasourceComponentID);
            else if (!(found is IPDFDataSource))
                throw RecordAndRaise.Argument("DataSourceID", Errors.AssignedDataSourceIsNotIPDFDataSource, datasourceComponentID);
            else
                datasourceComponent = ((IPDFDataSource)found);


            return datasourceComponent;
        }


    }
}
