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

    public class FontMappingCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new FontMapping();
        }

        protected override object GetElementKey(ConfigurationElement Component)
        {
            FontMapping lookup = (FontMapping)Component;
            bool bold = (lookup.FontStyle & System.Drawing.FontStyle.Bold) > 0;
            bool ital = (lookup.FontStyle & System.Drawing.FontStyle.Italic) > 0;
            return GetFullName(lookup.FamilyName, bold, ital);
        }

        private static string GetFullName(string family, bool bold, bool italic)
        {
            string fn;
            if (string.IsNullOrEmpty(family))
                fn = "UNKNOWN_FONT";
            else
                fn = family;

            if (bold)
                fn += ",Bold";
            if (italic)
            {
                if (!bold)
                    fn += ",";
                else
                    fn += " ";
                fn += "Italic";
            }
            return fn;
        }

        internal bool TryGetMappingName(string pdfName, out FontMapping mapping)
        {
            if (this.Count > 0)
            {
                mapping = this.BaseGet(pdfName) as FontMapping;
            }
            else
            {
                mapping = null;
            }
            
            return null != mapping;
        }

        
    }
}
