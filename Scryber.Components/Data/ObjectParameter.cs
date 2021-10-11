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
    [PDFParsableComponent("ObjectParameter")]
    public class ObjectParameter : IBindableComponent
    {
        //
        // events to support databinding
        //

        #region public event PDFDataBindEventHandler DataBinding;

        public event DataBindEventHandler DataBinding;

        protected virtual void OnDataBinding(DataBindEventArgs args)
        {
            if (null != this.DataBinding)
                this.DataBinding(this, args);
        }

        #endregion

        #region public event PDFDataBindEventHandler DataBound;

        public event DataBindEventHandler DataBound;

        protected virtual void OnDataBound(DataBindEventArgs args)
        {
            if (null != this.DataBound)
                this.DataBound(this, args);
        }

        #endregion

        //
        // properties
        //

        #region public string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the name of the parameter
        /// </summary>
        [PDFAttribute("name")]
        public string ParameterName { get; set; }

        #endregion

        #region public string Value { get; set; }

        /// <summary>
        /// Gets or sets the string representation of the value
        /// </summary>
        [PDFAttribute("value")]
        public string Value { get; set; }

        #endregion

        #region public TypeCode Type { get; set; }

        /// <summary>
        /// Gets or sets the native type code of the parameter
        /// </summary>
        [PDFAttribute("type")]
        public TypeCode Type { get; set; }

        #endregion

        #region public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the default value of the parameter
        /// </summary>
        [PDFAttribute("default-value")]
        public string DefaultValue { get; set; }

        #endregion

        //
        // ctor
        //

        public ObjectParameter()
        { }


        //
        // methods
        //

        #region public void DataBind(PDFDataContext context)

        /// <summary>
        /// Supports the databinding capabilites of the parameter by raising the events
        /// </summary>
        /// <param name="context"></param>
        public virtual void DataBind(DataContext context)
        {
            if (null != this.DataBinding || null != this.DataBound)
            {
                DataBindEventArgs args = new DataBindEventArgs(context);
                this.OnDataBinding(args);

                this.OnDataBound(args);
            }

        }

        #endregion

        #region public object GetNativeValue(PDFObjectProviderCommand command, System.Type paramtype)

        /// <summary>
        /// Converts the string value of this parameter into it's native value.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="paramtype"></param>
        /// <returns></returns>
        public object GetNativeValue(ObjectProviderCommand command, System.Type paramtype)
        {
            string value = this.Value;
            if (string.IsNullOrEmpty(value))
            {
                //special case where the value has been set to string.Empty (would normally be null)
                //and we should not ignore it.
                if (paramtype == typeof(string) && value == string.Empty)
                    return value;

                value = this.DefaultValue;
                if (paramtype == typeof(string) && value == string.Empty)
                    return value;

                else if (string.IsNullOrEmpty(value))
                {
                    if (paramtype.IsValueType)
                        throw new NullReferenceException(string.Format(Errors.NoDefaultValueSetOnEmptyValueTypeParameter, this.ParameterName, paramtype));

                    return null;
                }
            }

            object native = TypeConverter.GetNativeValue(paramtype, value);
            return native;

        }

        #endregion
    }


    /// <summary>
    /// A list of parameters
    /// </summary>
    public class PDFObjectParameterList : List<ObjectParameter>
    {

        public ObjectParameter this[string name]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    ObjectParameter p = this[i];
                    if (p.ParameterName == name)
                        return p;
                }
                return null;
            }
        }
        public void DataBind(DataContext context)
        {
            if (this.Count > 0)
            {
                foreach (ObjectParameter param in this)
                {
                    param.DataBind(context);
                }
            }
        }
    }
}
