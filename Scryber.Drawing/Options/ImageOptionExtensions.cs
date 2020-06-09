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

namespace Scryber.Options
{

    public static class ImageOptionExtension
    {

        public static PDFImageFactoryList GetFactories(this ImagingOptions options)
        {
            var list = new PDFImageFactoryList(_configured.Length);
            for(var i = 0; i < _configured.Length; i++)
            {
                if (null != _configured[i])
                {
                    PDFImageFactory one = new PDFImageFactory(_options[i].Name, new System.Text.RegularExpressions.Regex(_options[i].Match), _configured[i]);
                    list.Add(one);
                }
            }
            return list;

        }


        private static IPDFImageDataFactory[] _configured;
        private static ImageDataFactoryOption[] _options;

        /// <summary>
        /// Static constructor that initializes the ImageDataFactoryOptions
        /// </summary>
        static ImageOptionExtension()
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            _options = config.ImagingOptions.Factories;
            List<IPDFImageDataFactory> all = new List<IPDFImageDataFactory>();

            if(null == _options)
            {
                _options = new ImageDataFactoryOption[] { };
            }

            if(null != _options && _options.Length > 0)
            {
                for(var i = 0; i < _options.Length; i++)
                {
                    var one = Utilities.TypeHelper.GetInstance<IPDFImageDataFactory>(_options[i].FactoryType, _options[i].FactoryAssembly, true);
                    all.Add(one);
                }
                
            }
            _configured = all.ToArray();

        }
    }

    public class PDFImageFactoryList : List<PDFImageFactory>
    {
        public PDFImageFactoryList() : this(4)
        {

        }

        public PDFImageFactoryList(int capacity)
            : base(capacity)
        {
        }


        public bool TryGetMatch(string path, out IPDFImageDataFactory factory)
        {
            foreach (PDFImageFactory item in this)
            {
                if (item.IsMatch(path, out factory))
                    return true;
            }

            factory = null;
            return false;

        }

        public bool IsRegistered(string name)
        {
            foreach (PDFImageFactory item in this)
            {
                if (item.Name == name)
                    return true;
            }
            return false;
        }

    }

    public class PDFImageFactory
    {
        public string Name { get; set; }

        public System.Text.RegularExpressions.Regex Matcher { get; private set; }

        public IPDFImageDataFactory Factory { get; private set; }

        public PDFImageFactory(string name, System.Text.RegularExpressions.Regex match, IPDFImageDataFactory factory)
        {
            this.Matcher = match;
            this.Factory = factory;
        }

        public bool IsMatch(string path, out IPDFImageDataFactory factory)
        {
            if (this.Matcher.IsMatch(path))
            {
                factory = this.Factory;
                return true;
            }
            else
            {
                factory = null;
                return false;
            }
        }
    }
}
