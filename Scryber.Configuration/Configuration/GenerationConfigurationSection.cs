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
    /// The configuration section for the PDF generation. Contains the Extensions collection that maps an xml namespace to a runtime assembly and namespace
    /// </summary>
    /// <remarks>
    /// The generation section in web.config allows the definition of extensions. Components that exist in a different namespace
    /// and are defined in a separate dll and namespace. By adding a mapping here you can enable the XML Parser to  know about and understand
    /// the component definition.
    ///   <example>
    ///      &lt;generation&gt;
    ///        &lt;clear /&gt;
    ///        &lt;register xml-namespace=&quot;http://schemas.other.scryber.co.uk&quot; runtime-namespace=&quot;[Other.Namespace]&quot; runtime-assembly=&quot;[Other Assembly Name]&quot; /&gt;
    ///      &lt;/generation&gt;
    ///   </example>
    /// </remarks>
    public class GenerationConfigurationSection : ConfigurationSection
    {

        #region  public SchemaMappingCollection Mappings

        /// <summary>
        /// Xml local name of the collection 
        /// </summary>
        private const string MappingCollectionKey = "schemaMappings";

        /// <summary>
        /// local cached value of the collection for fast retrieval
        /// </summary>
        private SchemaMappingCollection _col = null;


        /// <summary>
        /// Gets or sets the collection of defined extensions. 
        /// This collection is usually modified in the configuration file - register, remove and clear
        /// </summary>
        [ConfigurationCollection(typeof(SchemaMappingElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
            AddItemName = "register", RemoveItemName = "remove", ClearItemsName = "clear")]
        [ConfigurationProperty(MappingCollectionKey, IsDefaultCollection = false, IsRequired = false)]
        public SchemaMappingCollection Mappings
        {
            get
            {
                if(null == _col)
                    _col = this[MappingCollectionKey] as SchemaMappingCollection;
                if (null == _col)
                {
                    _col = new SchemaMappingCollection();
                    _col.InitDefaults();
                }
                return _col;
            }
            set
            {
                this[MappingCollectionKey] = value;
                _col = null;
            }
        }

        #endregion

        #region public BindingFactoryElementCollection ExpressionBinders

        private const string BindingCollectionKey = "expressionBinders";

        /// <summary>
        /// Gets the collection of configured BindingFactoryElements
        /// </summary>
        [ConfigurationCollection(typeof(BindingFactoryElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
            AddItemName = "register", RemoveItemName = "remove", ClearItemsName = "clear")]
        [ConfigurationProperty(BindingCollectionKey, IsDefaultCollection = true, IsRequired = false)]
        public BindingFactoryElementCollection ExpressionBinders
        {
            get
            {
                BindingFactoryElementCollection col = this[BindingCollectionKey] as BindingFactoryElementCollection;

                return col;
            }
            set
            {
                this[BindingCollectionKey] = value;
            }
        }

        #endregion

        


        private const string MissingReferenceKey = "missing-reference-action";

        /// <summary>
        /// Defines the action a parser should take if the reference to a file or stream is not found
        /// </summary>
        [ConfigurationProperty(MissingReferenceKey,IsRequired=false,IsKey=false)]
        public ParserReferenceMissingAction MissingReferenceAction
        {
            get
            {
                object value = this[MissingReferenceKey];
                if (null == value)
                    value = ParserReferenceMissingAction.RaiseException;
                return (ParserReferenceMissingAction)value;
            }
            set
            {
                this[MissingReferenceKey] = value;
            }
        }




        /// <summary>
        /// Static default section
        /// </summary>
        private static GenerationConfigurationSection _def = new GenerationConfigurationSection();

        /// <summary>
        /// Gets the default Generation section if none has been defined in the configuration section.
        /// </summary>
        public static GenerationConfigurationSection Default
        {
            get
            {
                return _def;
            }
        }
    }
}
