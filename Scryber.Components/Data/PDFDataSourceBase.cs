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
using Scryber.Components;

namespace Scryber.Data
{

    /// <summary>
    /// Abstract base class for all components that extract and return data
    /// </summary>
    public abstract class PDFDataSourceBase : PDFDataComponentBase, IPDFDataSource
    {


        #region public virtual bool SupportsDataSchema { get; }

        /// <summary>
        /// IPDFDataSource implementation that returns true if this 
        /// data source supports schema interrogation.
        /// </summary>
        /// <remarks>
        /// It is up to inheritors to provide their own implementation
        /// and override this property to return the correct value</remarks>
        public virtual bool SupportsDataSchema { get { return false; } }

        #endregion

        //
        // ctor
        //

        #region protected PDFDataSourceBase(PDFObjectType type)

        /// <summary>
        /// Protected constructor - PDFDataSourceBase is abstract and cannot be directly instaniated.
        /// </summary>
        /// <param name="type"></param>
        protected PDFDataSourceBase(PDFObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // IPDFDataSource Members
        //

        #region public object Select(string path, PDFDataContext context)

        /// <summary>
        /// Retrieves the data from this DataSource based on the path
        /// </summary>
        /// <param name="path">The path to source the data from - for an XML DataSource this is an XPath query</param>
        /// <param name="data">The data to extract the result from</param>
        /// <returns>The required data (could be null)</returns>
        public object Select(string path, PDFDataContext context)
        {
            if (path == null)
                path = string.Empty;
            object data;
            try
            {
                data = this.DoSelectData(path, context);
            }
            catch (Exception ex)
            {
                throw RecordAndRaise.Data(ex, Errors.CouldNotSelectData);
            }

            return data;
        }

        #endregion

        #region public object Select(string path, object withData, PDFDataContext context)

        /// <summary>
        /// Retrieves the data from this DataSource based on the path
        /// </summary>
        /// <param name="path">The path to source the data from - for an XML DataSource this is an XPath query</param>
        /// <param name="withData">The data to perforn the select operation on</param>
        /// <param name="context">The current data context</param>
        /// <returns>The required data (could be null)</returns>
        public object Select(string path, object withData, PDFDataContext context)
        {
            object data;
            if (path == null)
                path = string.Empty;

            try
            {
                data = this.DoSelectData(path, withData, context);
            }
            catch (Exception ex)
            {
                throw RecordAndRaise.Data(ex, Errors.CouldNotSelectData);
            }

            return data;
        }

        #endregion

        public bool EvaluateTestExpression(string expr, object withData, PDFDataContext context)
        {
            if (expr == null)
                expr = string.Empty;

            bool result;
            try
            {
                result = this.DoEvaluateTestExpression(expr, withData, context);
            }
            catch (Exception ex)
            {
                throw RecordAndRaise.Data(ex, Errors.CouldNotSelectData);
            }
            return result;
        }

        public object Evaluate(string expr, object withData, PDFDataContext context)
        {
            if (expr == null)
                expr = string.Empty;

            object result;
            try
            {
                result = this.DoEvaluateExpression(expr, withData, context);
            }
            catch (Exception ex)
            {
                throw RecordAndRaise.Data(ex, Errors.CouldNotSelectData);
            }
            return result;
        }

        #region public virtual PDFDataSchema GetDataSchema(string path, PDFDataContext context)

        /// <summary>
        /// Returns the PDFDataScema associated with this source. 
        /// NOTE: Check that this operation is supported via the SupportsDataSchema property before calling.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="context"></param>
        /// <returns>The data schema associated with the source</returns>
        /// <exception cref="System.NotSupportedException" >Thrown if this data source does not support schema extraction</exception>
        public virtual PDFDataSchema GetDataSchema(string path, PDFDataContext context)
        {
            return null;
        }

        #endregion

        /// <summary>
        /// Abstract method that all inheritors must override to 
        /// return the appropriate data based on the defined path
        /// </summary>
        /// <param name="path">The select path to the required data</param>
        /// <param name="context">The current data context</param>
        /// <param name="root">If true then this is a select of the top level data on the source</param>
        /// <returns></returns> 
        protected abstract object DoSelectData(string path, PDFDataContext context);


        /// <summary>
        /// Abstract method that all inheritors must override to
        /// return the appropriate data based on the defined path AGAINST the defined data
        /// </summary>
        /// <param name="path"></param>
        /// <param name="withData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract object DoSelectData(string path, object withData, PDFDataContext context);

        /// <summary>
        /// Abstract method that all inheritors must override to
        /// return the results of a test expression AGAINST the defined data
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="withdata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract bool DoEvaluateTestExpression(string expr, object withdata, PDFDataContext context);


        /// <summary>
        /// Abstract method that all inheritors must override to
        /// return the results of an expression and return the result.
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="withdata"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract object DoEvaluateExpression(string expr, object withdata, PDFDataContext context);
    }


}
