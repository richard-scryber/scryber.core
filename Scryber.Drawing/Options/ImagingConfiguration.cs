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
using Scryber.Drawing;

namespace Scryber.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("The imaging configuration section is replaced by the IScryberConfigurationService", true)]
    public static class ImagingConfiguration
    {
        private static string ImagingSectionPath =  "scryber/imaging";

        [Obsolete("Use the configuration service", true)]
        public static ImagingConfigurationSection ImagingSection
        {
            get
            {
                object section = ConfigurationManager.GetSection(ImagingSectionPath);
                if (null == section)
                    section = new ImagingConfigurationSection();
                return (ImagingConfigurationSection)section;
            }
        }

        #region public static bool IsFactoryRegisterd(string path, out IPDFImageDataFactory factory)

        /// <summary>
        /// Searches the configured image factories and returns true if an appropriate image factory was found for the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="factory">If a match was found then this is set to the factory to use to create the image</param>
        /// <returns></returns>
        [Obsolete("Use the configuration service", true)]
        public static bool IsFactoryRegisterd(string path, out IPDFImageDataFactory factory)
        {
            object section = ConfigurationManager.GetSection(ImagingSectionPath);
            if (null == section)
            {
                factory = null;
                return false;
            }
            else if (section is ImagingConfigurationSection)
            {
                factory = ((ImagingConfigurationSection)section).GetRegisterdFactory(path);
                return null != factory;
            }
            else
            {
                string msg = String.Format(Errors.CannotCastObjectToType, section.GetType(), typeof(ImagingConfigurationSection));
                throw new ConfigurationErrorsException(msg);
            }
        }

        #endregion 

        [Obsolete("Use the configuration service", true)]
        public static bool AllowMissingImages()
        {
            return ImagingSection.AllowMissingImages;
        }
    }
}
