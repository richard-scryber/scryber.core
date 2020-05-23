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
using System.Configuration;

namespace Scryber.Configuration
{
    public class FontMapping : System.Configuration.ConfigurationElement
    {

        #region public string FamilyName {get;set;} : family-name

        private const string FamilyNameKey = "family-name";

        /// <summary>
        /// Gets or sets the Font family name for this mapping
        /// </summary>
        [ConfigurationProperty(FamilyNameKey,IsKey=false,IsRequired=true)]
        public string FamilyName
        {
            get { return this[FamilyNameKey] as string; }
            set { this[FamilyNameKey] = value; }
        }

        #endregion

        #region public System.Drawing.FontStyle FontStyle {get;set;} : font-style

        private const string FontStyleKey = "font-style";

        /// <summary>
        /// Gets or sets the FontStyle for this mapping
        /// </summary>
        [ConfigurationProperty(FontStyleKey, IsKey = false, IsRequired = true)]
        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                object value = this[FontStyleKey];
                if (null == value)
                    return System.Drawing.FontStyle.Regular;
                else
                    return (System.Drawing.FontStyle)value;
            }
            set
            {
                this[FontStyleKey] = value;
            }
        }

        #endregion

        //
        // file attributes
        //

        #region public string FileName {get; set;} : font-file

        private const string FileNameKey = "font-file";

        /// <summary>
        /// Gets or sets the file name of the Font file - 
        /// absolute path or relative to the executing assembly or web root.
        /// </summary>
        [ConfigurationProperty(FileNameKey,IsKey=false, IsRequired=false)]
        public string FileName
        {
            get { return this[FileNameKey] as string; }
            set { this[FileNameKey] = value; }
        }

        #endregion

        //
        // resource attributes
        //

        #region public string ResourceName {get; set;} : rsrc-name

        private const string ResourceNameKey = "rsrc-name";

        /// <summary>
        /// Gets or sets the name of the font file in a resource
        /// </summary>
        [ConfigurationProperty(ResourceNameKey, IsKey = false, IsRequired = false)]
        public string ResourceName
        {
            get { return this[ResourceNameKey] as string; }
            set { this[ResourceNameKey] = value; }
        }

        #endregion

        #region public string ResourceBaseName {get; set;} : rsrc-base

        private const string ResourceBaseNameKey = "rsrc-base";

        /// <summary>
        /// Gets or sets the base name with optionally qualified asembly name for the resource manager
        /// </summary>
        [ConfigurationProperty(ResourceBaseNameKey, IsKey = false, IsRequired = false)]
        public string ResourceBaseName
        {
            get { return this[ResourceBaseNameKey] as string; }
            set { this[ResourceBaseNameKey] = value; }
        }

        #endregion

    }
}
