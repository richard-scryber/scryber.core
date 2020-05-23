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
    public class ImageFactoryElement : ConfigurationElement
    {

        #region public string Extension {get;set;}

        private const string ExtensionKey = "match-path";

        /// <summary>
        /// Gets or sets the extension the image data factory handles
        /// </summary>
        [ConfigurationProperty(ExtensionKey, IsKey=true, IsRequired=true)]
        public string MatchPath
        {
            get { return this[ExtensionKey] as string; }
            set { this[ExtensionKey] = value; }
        }

        #endregion

        #region public string FactoryType {get;set;}

        private const string TypeKey = "factory-type";

        /// <summary>
        /// Gets or sets the name for the factory type that this configuration element refers to.
        /// </summary>
        [ConfigurationProperty(TypeKey,IsRequired=true)]
        public string FactoryType
        {
            get { return this[TypeKey] as string; }
            set { this[TypeKey] = value; }
        }

        #endregion

        private System.Text.RegularExpressions.Regex _expr;

        public System.Text.RegularExpressions.Regex Expression
        {
            get
            {
                if (null == _expr)
                    _expr = new System.Text.RegularExpressions.Regex(this.MatchPath);
                return _expr;
            }
        }

        private IPDFImageDataFactory _factory;

        public IPDFImageDataFactory Factory
        {
            get
            {
                if (null == _factory)
                    _factory = this.GetFactory();
                return _factory;
            }
        }

        public bool IsMatch(string path, out IPDFImageDataFactory factory)
        {
            if (null == this._expr)
                _expr = new System.Text.RegularExpressions.Regex(this.MatchPath);
            if (this._expr.IsMatch(path))
            {
                factory = this.GetFactory();
                return true;
            }
            else
            {
                factory = null;
                return false;
            }
        }

        protected virtual IPDFImageDataFactory GetFactory()
        {
            if (null == this._factory)
            {
                try
                {
                    Type t = Support.GetTypeFromName(this.FactoryType);
                    object instance = System.Activator.CreateInstance(t);
                    IPDFImageDataFactory factory = (IPDFImageDataFactory)instance;
                    _factory = factory;
                }
                catch (Exception ex)
                {
                    throw new Scryber.PDFException(string.Format(Errors.CouldNotLoadImagingFactory, this.FactoryType),ex);
                }
            }
            return _factory;
        }
    }

    public class ImageDataFactoryCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ImageFactoryElement();
        }

        protected override object GetElementKey(ConfigurationElement Component)
        {
            ImageFactoryElement factory = (ImageFactoryElement)Component;

            return factory.MatchPath;
        }

        public PDFImageFactoryList GetList()
        {
            PDFImageFactoryList list = new PDFImageFactoryList(this.Count);
            foreach (ImageFactoryElement item in this)
            {
                list.Add(new PDFImageFactory(item.MatchPath, item.Expression, item.Factory));
            }
            return list;
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
            if(this.Matcher.IsMatch(path))
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
