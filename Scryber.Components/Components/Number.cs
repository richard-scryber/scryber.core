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
using Scryber.Styles;

namespace Scryber.Components
{
    /// <summary>
    /// Represents a numeric value and converts to a string in the content
    /// </summary>
    [PDFParsableComponent("Number")]
    public class Number : TextBase
    {
        //
        // properties
        //

        #region public double Value { get; set; }

        /// <summary>
        /// Gets or sets the bindable double value for this number
        /// </summary>
        [PDFAttribute("value")]
        public double Value { get; set; }

        #endregion

        #region public string NumberFormat {get;set;}

        /// <summary>
        /// Gets or sets the format to use to convert the number value to a string
        /// </summary>
        [PDFAttribute("number-format", Const.PDFStylesNamespace)]
        public virtual string NumberFormat
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.TextNumberFormatKey, string.Empty);
                else
                    return string.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.TextNumberFormatKey, value);
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the format provider for this number value
        /// </summary>
        public IFormatProvider FormatProvider { get; set; }

        //
        // .ctor
        //

        #region public PDFNumber()

        public Number()
            : this(ObjectTypes.NumberComp)
        {
        }

        #endregion

        #region protected PDFNumber(PDFObjectType type)

        protected Number(ObjectType type)
            : base(type)
        {
        }

        #endregion


        //
        // methods
        //

        #region protected override Text.PDFTextReader CreateReader(PDFLayoutContext context, Styles.PDFStyle fullstyle)

        /// <summary>
        /// overrides the default implementation to create a text reader for the Number in the correct format
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        /// <returns></returns>
        protected override Text.PDFTextReader CreateReader(PDFContextBase context, Styles.Style fullstyle)
        {
            string val;
            string format = fullstyle.GetValue(StyleKeys.TextNumberFormatKey, string.Empty);

            if (null == this.FormatProvider)
            {
                if (!string.IsNullOrEmpty(format))
                    val = this.Value.ToString(format);
                else
                    val = this.Value.ToString();
            }
            else
            {
                if (!string.IsNullOrEmpty(format))
                    val = this.Value.ToString(format,this.FormatProvider);
                else
                    val = this.Value.ToString(this.FormatProvider);
            }
            return Scryber.Text.PDFTextReader.Create(val, TextFormat.Plain, false, context.TraceLog);
        }

        #endregion
    }
}
