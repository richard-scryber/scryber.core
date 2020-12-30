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
    /// Represents and encapsulates a date value that can be rendered to a PDF
    /// </summary>
    [PDFParsableComponent("Date")]
    [PDFJSConvertor("scryber.studio.design.convertors.date")]
    public class Date : TextBase
    {
        //
        // properties
        //

        #region public DateTime Value { get; set; }

        /// <summary>
        /// The bindable date value. Default is the current date time
        /// </summary>
        [PDFAttribute("value")]
        [PDFDesignable("Value", Category = "General", Priority = 3, Type = "Date")]
        public DateTime Value { get; set; }

        #endregion

        #region public string DateFormat {get;set;}

        /// <summary>
        /// Gets or sets the bindable date format string for conversion from value to a string
        /// </summary>
        [PDFAttribute("date-format", Const.PDFStylesNamespace)]
        [PDFDesignable("Format", Category = "General", Priority = 4, Type = "DateFormat")]
        public string DateFormat
        {
            get
            {
                StyleValue<string> style;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.TextDateFormatKey, out style))
                    return style.Value;
                else
                    return string.Empty;
            }
            set
            {
                this.Style.Text.DateFormat = value;
            }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFDate()

        public Date()
            : this(PDFObjectTypes.DateComp)
        {
        }

        #endregion

        #region protected PDFDate(PDFObjectType type)

        protected Date(PDFObjectType type)
            : base(type)
        {
            this.Value = DateTime.Now;
        }

        #endregion

        //
        // methods
        //

        #region protected override Text.PDFTextReader CreateReader(PDFLayoutContext context, Styles.PDFStyle fullstyle)

        /// <summary>
        /// Overrides the base implementation to create a reader for the converted date value
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        /// <returns></returns>
        protected override Text.PDFTextReader CreateReader(PDFLayoutContext context, Styles.Style fullstyle)
        {
            string val;
            string format = fullstyle.GetValue(StyleKeys.TextDateFormatKey, string.Empty);

            if (string.IsNullOrEmpty(format))
                val = this.Value.ToString();
            else
                val = this.Value.ToString(format);

            return Scryber.Text.PDFTextReader.Create(val, TextFormat.Plain, false, context.TraceLog);
        }

        #endregion
    }
}
