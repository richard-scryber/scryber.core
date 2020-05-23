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
    /// A configuration element that declares a factory to be used for a particular prefix in a binding expression
    /// </summary>
    public class BindingFactoryElement : ConfigurationElement
    {

        #region public string BindingPrefix {get;set;}

        private const string BindingPrefixKey = "prefix";

        /// <summary>
        /// Gets or sets the string that will be used in binding expressions to identify that the
        /// factory this config element refers to should be used to generate binding expressions.
        /// </summary>
        [ConfigurationProperty(BindingPrefixKey, IsKey = true, IsRequired = true)]
        public string BindingPrefix
        {
            get { return this[BindingPrefixKey] as string; }
            set { this[BindingPrefixKey] = value; }
        }

        #endregion

        #region public string FactoryType {get;set;}

        private const string TypeKey = "factory-type";

        /// <summary>
        /// Gets or sets the name for the factory type that this configuration element refers to.
        /// </summary>
        [ConfigurationProperty(TypeKey, IsRequired = true)]
        public string FactoryType
        {
            get { return this[TypeKey] as string; }
            set { this[TypeKey] = value; }
        }

        #endregion

        #region protected virtual IPDFBindingExpressionFactory GetFactory()

        private IPDFBindingExpressionFactory _factory;

        /// <summary>
        /// Returns an instance of the IPDFBindingExpressionFactory that can return delegate event handlers for binding expressions.
        /// </summary>
        /// <returns></returns>
        public virtual IPDFBindingExpressionFactory GetFactory()
        {
            if (null == this._factory)
            {
                try
                {
                    Type t = Support.GetTypeFromName(this.FactoryType);
                    object instance = System.Activator.CreateInstance(t);
                    IPDFBindingExpressionFactory factory = (IPDFBindingExpressionFactory)instance;
                    _factory = factory;
                }
                catch (Exception ex)
                {
                    throw new System.Configuration.ConfigurationErrorsException(string.Format("Could not create an instace of the type '{0}' : {1}", this.FactoryType, ex.Message), ex);
                }
            }
            return _factory;
        }

        #endregion

    }

    /// <summary>
    /// A collection of configured Binding Factories that can be directly accessed, or use the 
    /// TryGetBindingElement to attempt retrieval without causing exception.
    /// </summary>
    public class BindingFactoryElementCollection : ConfigurationElementCollection
    {

        #region public BindingFactoryElementCollection()

        /// <summary>
        /// Creates a new BindingFactoryElementCollection
        /// </summary>
        public BindingFactoryElementCollection()
            : base()
        {
        }

        #endregion

        #region protected override object GetElementKey(ConfigurationElement element)

        /// <summary>
        /// Overrides the abstract base method to return the BindingPrefix of the BindingFactoryElement config element 
        /// </summary>
        /// <param name="element">Must be an instance of BindingFactoryElement</param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            BindingFactoryElement bind = (BindingFactoryElement)element;
            return bind.BindingPrefix;
        }

        #endregion

        #region protected override ConfigurationElement CreateNewElement()

        /// <summary>
        /// Overrides the abstract base method to return a new BindingFactoryElement
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new BindingFactoryElement();
        }

        #endregion

        #region public bool TryGetBindingElement(string prefix, out BindingFactoryElement element)

        /// <summary>
        /// Attepts to retrieve any configured BindingFactoryElement based on the provided prefix. Returns true if one is defined, otherwise false
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool TryGetBindingElement(string prefix, out BindingFactoryElement element)
        {
            if (this.Count > 0)
                element = this.BaseGet(prefix) as BindingFactoryElement;
            else
                element = null;

            return null != element;
        }

        #endregion

        #region protected override void Init() + InitDefaults()

        /// <summary>
        /// Initializes the default extension entries in the collection.
        /// </summary>
        protected virtual void InitDefaults()
        {
            
        }

        protected override void Init()
        {
            this.InitDefaults();
            base.Init();
        }

        #endregion
    }
}
