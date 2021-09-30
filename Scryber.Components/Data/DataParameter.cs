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
    [PDFParsableComponent("Parameter")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_dataParameter")]
    public class DataParameter : IBindableComponent
    {

        #region ivars

        private string _name;
        private string _value;
        private System.Data.DbType _dbtype = System.Data.DbType.String;
        private int _size = -1;
        private string _default;

        #endregion

        //
        // events to support databinding
        //

        #region public event PDFDataBindEventHandler DataBinding;

        public event PDFDataBindEventHandler DataBinding;

        protected virtual void OnDataBinding(PDFDataBindEventArgs args)
        {
            if (null != this.DataBinding)
                this.DataBinding(this, args);
        }

        #endregion

        #region public event PDFDataBindEventHandler DataBound;

        public event PDFDataBindEventHandler DataBound;

        protected virtual void OnDataBound(PDFDataBindEventArgs args)
        {
            if (null != this.DataBound)
                this.DataBound(this, args);
        }

        #endregion


        //
        // properties
        //

        #region public string ParameterName {get;set;}

        /// <summary>
        /// Gets or sets the provider specific name of the parameter
        /// </summary>
        [PDFAttribute("name")]
        public string ParameterName
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region public string Value {get;set;}

        /// <summary>
        /// Gets or sets actual value of this parameter
        /// </summary>
        [PDFAttribute("value")]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

        #region public System.Data.DbType DataType

        /// <summary>
        /// Gets or sets the data type of this parameter
        /// </summary>
        [PDFAttribute("type")]
        public System.Data.DbType DataType
        {
            get { return _dbtype; }
            set { _dbtype = value; }
        }

        #endregion

        #region public int Size {get;set;}

        /// <summary>
        /// Gets or sets the parameter size for this parameter
        /// </summary>
        [PDFAttribute("size")]
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        #endregion

        #region public string DefaultValue {get;set;}

        /// <summary>
        /// Gets or sets the default value for this parameter
        /// </summary>
        [PDFAttribute("default-value")]
        public string DefaultValue
        {
            get { return _default; }
            set { _default = value; }
        }

        #endregion

        #region protected bool IsStringDataType {get;}

        /// <summary>
        /// Returns true if this parameter's data type is one of hte string types
        /// </summary>
        protected bool IsStringDataType
        {
            get
            {
                bool isstring;
                switch (this.DataType)
                {
                    case System.Data.DbType.AnsiString:
                    case System.Data.DbType.AnsiStringFixedLength:
                    case System.Data.DbType.String:
                    case System.Data.DbType.StringFixedLength:
                        isstring = true;
                        break;
                    default:
                        isstring = false;
                        break;
                }
                return isstring;
            }
        }

        #endregion


        //
        // methods
        //

        #region private string GetValueName()

        /// <summary>
        /// Gets the name that should be set on the paramter value collection
        /// </summary>
        /// <returns></returns>
        private string GetParameterName()
        {
            string name = this.ParameterName;
            return name;
        }

        #endregion

        #region public virtual object GetParameterValue(PDFXMLSQLDataProvider provider, PDFDataContext context)

        public virtual object GetParameterValue(IPDFDataSetProviderCommand command, PDFDataContext context)
        {
            string value = this.Value;
            
            if(string.IsNullOrEmpty(value))
            {
                value = this.DefaultValue;
            }

            if(String.IsNullOrEmpty(value))
            {
                //Special case where the data type is textual, and we are not null.
                if (this.IsStringDataType && null != value)
                    return String.Empty;
                else
                    return command.GetNullValue(this.DataType);
            }
            else
            {
                object parsed = TypeConverter.GetNativeValue(this.DataType, value);
                return parsed;
            }
            
        }

        #endregion

        #region public void DataBind(PDFDataContext context)

        /// <summary>
        /// Supports the databinding capabilites of the parameter by raising the events
        /// </summary>
        /// <param name="context"></param>
        public virtual void DataBind(PDFDataContext context)
        {
            if (null != this.DataBinding || null != this.DataBound)
            {
                PDFDataBindEventArgs args = new PDFDataBindEventArgs(context);
                this.OnDataBinding(args);

                this.OnDataBound(args);
            }

        }

        #endregion


    }

    

    public class PDFDataParameterList : List<DataParameter>
    {

        public void DataBind(PDFDataContext context)
        {
            if (this.Count > 0)
            {
                foreach (DataParameter param in this)
                {
                    param.DataBind(context);
                }
            }
        }
    }
}
