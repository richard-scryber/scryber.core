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
    public class ImagingConfigurationSection : ConfigurationSection
    {
        private const string AllowMissingImagesKey = "allow-missing-images";
        public const bool DefaultAllowMissingImages = false;

        /// <summary>
        /// If true then missing images (images that cannot be loaded) will be replaced with a placeholder image, rather than raising an exception.
        /// Default is false - cause an error.
        /// </summary>
        [ConfigurationProperty(AllowMissingImagesKey, IsRequired = false)]
        public bool AllowMissingImages
        {
            get
            {
                object value = this[AllowMissingImagesKey];
                bool parsed;
                if (null == value || !bool.TryParse(value.ToString(), out parsed))
                    return DefaultAllowMissingImages;
                else
                    return parsed;
            }
            set
            {
                this[AllowMissingImagesKey] = value;
            }
        }

        private const string ImageCacheDurationMinsKey = "cache-duration-mins";
        public const int DefaultCacheDurationMins = -1;

        /// <summary>
        /// The value for the duration if images should never expire, and be held permanently in cache.
        /// </summary>
        public const int ImageCacheNeverExpires = -1;

        [ConfigurationProperty(ImageCacheDurationMinsKey, IsRequired=false, DefaultValue=ImageCacheNeverExpires)]
        public int ImageCacheDurationMins
        {
            get
            {
                object value = this[ImageCacheDurationMinsKey];
                int parsed;
                if(null == value || !int.TryParse(value.ToString(), out parsed))
                    return DefaultCacheDurationMins;
                else
                    return parsed;
            }
            set
            {
                this[ImageCacheDurationMinsKey] = value;
            }
        }


        private const string FactoriesKey = "";

        [ConfigurationProperty(FactoriesKey,IsDefaultCollection=true)]
        [ConfigurationCollection(typeof(ImageFactoryElement),AddItemName="imageFactory",ClearItemsName="clear",RemoveItemName="remove")]
        public ImageDataFactoryCollection Factories
        {
            get { return this[FactoriesKey] as ImageDataFactoryCollection; }
            set { this[FactoriesKey] = value; }
        }


        public IPDFImageDataFactory GetRegisterdFactory(string path)
        {
            IPDFImageDataFactory found = null;
            ImageDataFactoryCollection facts = this.Factories;
            if (null != facts && facts.Count > 0)
            {
                foreach (ImageFactoryElement fact in facts)
                {
                    if (fact.IsMatch(path, out found))
                        break;
                    
                }
            }
            return found;
        }

        public ImagingConfigurationSection()
            : base()
        {
            
        }
    }
}
