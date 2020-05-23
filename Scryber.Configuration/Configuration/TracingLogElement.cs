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
    public class TracingLogElement : ConfigurationElement
    {

        #region public string Name {get; set;} : @name

        private const string NameKey = "name";

        /// <summary>
        /// Gets or sets the file name of the Font file - 
        /// absolute path or relative to the executing assembly or web root.
        /// </summary>
        [ConfigurationProperty(NameKey, IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this[NameKey] as string; }
            set { this[NameKey] = value; }
        }

        #endregion

        //
        // factory attributes
        //

        #region public string FactoryTypeName {get; set;} : @factory-type

        private const string FactoryTypeKey = "factory-type";

        /// <summary>
        /// Gets or sets the name of the type that will create instances that support logging
        /// </summary>
        [ConfigurationProperty(FactoryTypeKey, IsKey = false, IsRequired = true)]
        public string FactoryTypeName
        {
            get { return this[FactoryTypeKey] as string; }
            set { this[FactoryTypeKey] = value; }
        }

        #endregion

        #region public bool Enabled {get; set;} : @enabled

        private const string EnabledKey = "enabled";

        /// <summary>
        /// Returns true if this factory is enabled (default is true)
        /// </summary>
        [ConfigurationProperty(EnabledKey, IsKey = false, IsRequired = false, DefaultValue=true)]
        public bool Enabled
        {
            get
            {
                object value = this[EnabledKey];
                if (null == value || !(value is bool))
                    return true;//default is true
                else
                    return (bool)value;
            }
            set { this[EnabledKey] = value; }
        }

        #endregion

        private IPDFTraceLogFactory _factory;
        private static object _lock = new object();

        public IPDFTraceLogFactory GetFactory()
        {
            if (null == _factory)
            {
                if(string.IsNullOrEmpty(this.FactoryTypeName))
                    throw new ConfigurationErrorsException("Required configuration value is not set - factory-type");

                GetFactoryFromTypeName();
            }
            return _factory;
        }

        private void GetFactoryFromTypeName()
        {
            lock (_lock)
            {
                IPDFTraceLogFactory factory;
                Type type = Support.GetTypeFromName(this.FactoryTypeName);
                object instance = Activator.CreateInstance(type);

                factory = (IPDFTraceLogFactory)instance;

                _factory = factory;
            }
        }
    }
}
