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
    /// A collection of extension namespaces that the generator can understand.
    /// </summary>
    /// <remarks>Key is the inner elements XmlNamespace property. This is case insensitive</remarks>
    [Obsolete("Use the IScryberCOnfigurationSerive, from the ServiceProvider", true)]
    public class SchemaMappingCollection : ConfigurationElementCollection
    {

        public SchemaMappingCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }


        protected override ConfigurationElement CreateNewElement()
        {
            return new SchemaMappingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SchemaMappingElement)element).XmlNamespace;
        }


        /// <summary>
        /// Attempts to retrieve an extension element based on the xml namespace provided. Returns true if one was found.
        /// </summary>
        /// <param name="xmlnamesace"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        public bool TryGetMapping(string xmlnamesace, out SchemaMappingElement found)
        {
            if (this.Count == 0)
            {
                found = null;
                return false;
            }
            else
            {
                found = this.BaseGet(xmlnamesace) as SchemaMappingElement;
                return null != found;
            }
        }

        /// <summary>
        /// Initializes the default extension entries in the collection.
        /// </summary>
        public void InitDefaults()
        {
            //this.AddMapping(Const.ComponentXmlNamespace, Const.ComponentRuntimeNamespace, Const.ComponentRuntimeAssembly);
            //this.AddMapping(Const.StylesXmlNamespace, Const.StylesRuntimeNamespace, Const.StylesRuntimeAssembly);
            //this.AddMapping(Const.DataXmlNamespace, Const.DataRuntimeNamespace, Const.DataRuntimeAssembly);
        }

        protected override void Init()
        {
            this.InitDefaults();
            base.Init();
        }

        /// <summary>
        /// Adds a new extension to this collection (removing any existing entry first).
        /// </summary>
        /// <param name="xmlNamespace"></param>
        /// <param name="runtimeNamespace"></param>
        /// <param name="runtimeAssembly"></param>
        protected virtual void AddMapping(string xmlNamespace, string runtimeNamespace, string runtimeAssembly)
        {
            ConfigurationElement alreadyThere = this.BaseGet(xmlNamespace);
            if (null != alreadyThere)
                this.BaseRemove(xmlNamespace);

            SchemaMappingElement ele = new SchemaMappingElement();
            ele.XmlNamespace = xmlNamespace;
            ele.RuntimeNamespace = runtimeNamespace;
            ele.RuntimeAssembly = runtimeAssembly;
            this.BaseAdd(ele);
        }
    }
}
