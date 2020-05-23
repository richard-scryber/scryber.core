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
using System.Configuration;

namespace Scryber.Configuration
{
    /// <summary>
    /// Configuration element for an extension. The extension maps an xml namespace to a runtime assembly and runtime namespace.
    /// This allows the XMLParser to learn about and understand a type dynamically
    /// </summary>
    public class SchemaMappingElement : ConfigurationElement
    {

        #region public string XmlNamespace {get;set;} : xml-namespace

        private const string XmlNamespaceKey = "xml-namespace";

        /// <summary>
        /// Gets or sets the Xml Namespace for this extension
        /// </summary>
        [ConfigurationProperty(XmlNamespaceKey, IsKey = true, IsRequired = true)]
        public string XmlNamespace
        {
            get { return this[XmlNamespaceKey] as string; }
            set { this[XmlNamespaceKey] = value; }
        }

        #endregion

        #region public string RuntimeNamespace {get;set;} : runtime-namespace

        private const string RuntimeNamespaceKey = "runtime-namespace";

        /// <summary>
        /// Gets or sets the Runtime Namespace for this extension
        /// </summary>
        [ConfigurationProperty(RuntimeNamespaceKey, IsKey = false, IsRequired = true)]
        public string RuntimeNamespace
        {
            get { return this[RuntimeNamespaceKey] as string; }
            set { this[RuntimeNamespaceKey] = value; }
        }

        #endregion

        #region public string RuntimeAssembly {get;set;} : runtime-assembly

        private const string RuntimeAssemblyKey = "runtime-assembly";

        /// <summary>
        /// Gets or sets the Runtime Assembly name for this extension
        /// </summary>
        [ConfigurationProperty(RuntimeAssemblyKey, IsKey = false, IsRequired = true)]
        public string RuntimeAssembly
        {
            get { return this[RuntimeAssemblyKey] as string; }
            set { this[RuntimeAssemblyKey] = value; }
        }

        #endregion
    }
}
