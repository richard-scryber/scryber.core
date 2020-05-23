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
using System.Reflection;
using System.Xml;

namespace Scryber.Generation
{
    public class ParserAttributeDefinition : ParserPropertyDefinition
    {
        PDFXmlConverter _converter;
        bool _customparse;
        bool _iscodedom;

        public PDFXmlConverter Converter
        {
            get { return _converter; }
        }

        /// <summary>
        /// Returns true if this attributes type implements the IPDFParsableValue interface
        /// (can directly be parsed into an instance using the static Parse method)
        /// </summary>
        public override bool IsCustomParsable
        {
            get
            {
                return _customparse;
            }
        }

        /// <summary>
        /// Returns true if the type this attribute references implements the IPDFSimpleCodeDomValue - so can be generated via emit.
        /// </summary>
        public bool IsCodeDomGenerator
        {
            get
            {
                return _iscodedom;
            }
        }

        internal ParserAttributeDefinition(string name, string ns, PropertyInfo info, PDFXmlConverter convert, bool iscustomparsable)
            : base(name, ns, info, DeclaredParseType.Attribute)
        {
            this._converter = convert;
            this._customparse = iscustomparsable;
            this._iscodedom = Array.IndexOf<Type>(info.PropertyType.GetInterfaces(), typeof(IPDFSimpleExpressionValue)) > -1;
        }

        protected override object DoGetValue(XmlReader reader, PDFGeneratorSettings settings)
        {
            return this.Converter(reader, this.PropertyInfo.PropertyType, settings);
        }

        
    }
}
