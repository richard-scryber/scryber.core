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

namespace Scryber.Generation
{
    /// <summary>
    /// Simple element whose content (text) is a simple type
    /// </summary>
    public class ParserSimpleElementDefinition : ParserPropertyDefinition
    {

        private PDFXmlConverter _convert;
        private bool _customparse;

        internal PDFXmlConverter Converter
        {
            get { return _convert; }
        }

        public override bool IsCustomParsable
        {
            get
            {
                return _customparse;
            }
        }

        public ParserSimpleElementDefinition(string name, string ns, PropertyInfo pi, PDFXmlConverter convert, bool iscustomparsable)
            : base(name, ns, pi, DeclaredParseType.SimpleElement)
        {
            this._convert = convert;
            this._customparse = iscustomparsable;
        }


        protected override object DoGetValue(System.Xml.XmlReader reader, PDFGeneratorSettings settings)
        {
            return Converter(reader, this.PropertyInfo.PropertyType, settings);
        }
    }
}
