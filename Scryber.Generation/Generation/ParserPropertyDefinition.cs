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
    public abstract class ParserPropertyDefinition
    {
        #region ivars

        private PropertyInfo _desc;
        private Type _proptype;
        private string _name;
        private DeclaredParseType _parsetype;
        private string _ns;

        #endregion

        /// <summary>
        /// Gets the PropertyDescriptor for this definition
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get { return _desc; }
        }

        /// <summary>
        /// Gets the Type that this properties values are (must be)
        /// </summary>
        public Type ValueType
        {
            get { return _proptype; }
        }

        /// <summary>
        /// Gets the Parse type (attribute, element etc) this property is read from
        /// </summary>
        public DeclaredParseType ParseType
        {
            get { return _parsetype; }
        }

        /// <summary>
        /// Gets the name of the node to parse this definition from.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the namespace of the node to parse this definition from.
        /// </summary>
        public string NameSpace
        {
            get { return _ns; }
        }

        public virtual bool IsCustomParsable
        {
            get { return false; }
        }

        /// <summary>
        /// If true then this properties value (if set) will be assigned as the base path for loading content.
        /// </summary>
        public bool IsParserSourceValue { get; set; }

        public object GetValue(System.Xml.XmlReader reader, ParserSettings settings)
        {
            try
            {
                return this.DoGetValue(reader, settings);
            }
            catch(Exception ex)
            {
                throw new PDFParserException("Could not read the value " + this.Name + " from the Xml source", ex);
            }
        }

        protected virtual object DoGetValue(System.Xml.XmlReader reader, ParserSettings settings)
        {
            throw new NotSupportedException("GetValue is not supported on the root property type");
        }

        /// <summary>
        /// Creates a new PropertyDefinition
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ns"></param>
        /// <param name="pi"></param>
        /// <param name="parsetype"></param>
        protected ParserPropertyDefinition(string name, string ns, PropertyInfo pi, DeclaredParseType parsetype)
        {
            if (null == name)
                throw new ArgumentNullException("name");
            if (null == pi)
                throw new ArgumentNullException("desc");
            this._desc = pi;
            this._name = name;
            this._parsetype = parsetype;
            this._proptype = pi.PropertyType;
            this._ns = ns;
        }

        public override string ToString()
        {
            return "Property '" + this.Name + "' => " + this.PropertyInfo.DeclaringType + "." + this.PropertyInfo.Name; 
        }
    }


    public class ParserPropertyDefinitionCollection : System.Collections.ObjectModel.KeyedCollection<string,ParserPropertyDefinition>
    {
        private static List<string> _namespaces = new List<string>();

        /// <summary>
        /// Gets the property definition in the collection based on name and namespace
        /// </summary>
        /// <param name="name">The element name of the property</param>
        /// <param name="ns">The namespace of the property</param>
        /// <returns></returns>
        public ParserPropertyDefinition this[string name, string ns]
        {
            get
            {
                ParserPropertyDefinition defn;
                if (this.TryGetPropertyDefinition(name, ns, out defn))
                    return defn;
                else
                    return null;
            }
        }

        

        protected override string GetKeyForItem(ParserPropertyDefinition item)
        {
            return GetQualifiedName(item.Name, item.NameSpace);
        }

        private static string GetQualifiedName(string name, string ns)
        {
            
            if (!string.IsNullOrEmpty(ns))
            {
                ns = ns.ToLower();
                int index = _namespaces.IndexOf(ns);
                if (index < 0)
                {
                    index = _namespaces.Count;
                    _namespaces.Add(ns);
                }
                name = index.ToString() + ":" + name;
            }
                
            return name;
        }

        public bool TryGetPropertyDefinition(string name, string ns, out ParserPropertyDefinition defn)
        {
            if (this.Count == 0)
            {
                defn = null;
                return false;
            }
            else
            {
                //First attempt to get the property as a qualified name
                string qual = GetQualifiedName(name, ns);
                bool found = this.Dictionary.TryGetValue(qual, out defn);
                //If this does not exist then attempt to get the property
                //The has been defined without an explicit namespace (unqualified) on the type
                if (!found)
                    found = this.Dictionary.TryGetValue(name, out defn);
                return found;
            }
        }

        
    }
}
