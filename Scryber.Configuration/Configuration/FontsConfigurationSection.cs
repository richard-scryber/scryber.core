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
    public class FontsConfigurationSection : System.Configuration.ConfigurationSection
    {

        private const string DefaultDirectoryKey = "default-directory";

        /// <summary>
        /// Gets or sets the default directory to search for fonts identified in the stream.
        /// </summary>
        [ConfigurationProperty(DefaultDirectoryKey, IsRequired = false)]
        public string DefaultDirectory
        {
            get
            {
                string val = this[DefaultDirectoryKey] as string;
                if (null == val)
                    return string.Empty;
                else
                    return val;
            }
            set { this[DefaultDirectoryKey] = value; }
        }


        private const string FontSubstitutionKey = "font-substitution";

        /// <summary>
        /// Flag to identifiy if fonts should be substituted 
        /// if a matching font could not be found on the system
        /// </summary>
        [ConfigurationProperty(FontSubstitutionKey, IsRequired = false, DefaultValue = ScryberConfiguration.DefaultUseFontSubstitution)]
        public bool UseFontSubstitution
        {
            get
            {
                object value = this[FontSubstitutionKey];
                if (null == value || !(value is bool))
                    return ScryberConfiguration.DefaultUseFontSubstitution;
                else
                    return (bool)value;
            }
            set
            {
                this[FontSubstitutionKey] = value;
            }
        }

        private const string UseSystemFontsKey = "use-system-fonts";

        /// <summary>
        /// Flag to identifiy if fonts should be substituted 
        /// if a matching font could not be found on the system
        /// </summary>
        [ConfigurationProperty(UseSystemFontsKey, IsRequired = false, DefaultValue = ScryberConfiguration.DefaultUseSystemFonts)]
        public bool UseSystemFonts
        {
            get
            {
                object value = this[UseSystemFontsKey];
                if (null == value || !(value is bool))
                    return ScryberConfiguration.DefaultUseSystemFonts;
                else
                    return (bool)value;
            }
            set
            {
                this[UseSystemFontsKey] = value;
            }
        }

        private const string DefaultFontNameKey = "default-font";

        /// <summary>
        /// Gets or sets the default font to use for documents if not explicitly defined
        /// </summary>
        [ConfigurationProperty(DefaultFontNameKey,IsRequired=false)]
        public string DefaultFontName
        {
            get
            {
                object value = this[DefaultFontNameKey];
                if (null == value || value.ToString().Length == 0)
                    return ScryberConfiguration.DefaultFontName;
                else
                    return value.ToString();
            }
            set { this[DefaultFontNameKey] = value; }
        }

        private const string FontNameCollectionKey = "";

        [ConfigurationCollection(typeof(FontMapping), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
            AddItemName="add", RemoveItemName="remove", ClearItemsName="clear")]
        [ConfigurationProperty(FontNameCollectionKey, IsDefaultCollection = true, IsRequired = false)]
        public FontMappingCollection FontNames
        {
            get
            {
                FontMappingCollection col = this[FontNameCollectionKey] as FontMappingCollection;
                
                return col;
            }
            set
            {
                this[FontNameCollectionKey] = value;
            }
        }

        public bool TryGetMappingName(string pdfName, out FontMapping mapping)
        {
            FontMappingCollection col = this.FontNames;
            if (col == null || col.Count == 0)
            {
                mapping = null;
                return false;
            }
            else
                return col.TryGetMappingName(pdfName, out mapping);
        }
    }
}
