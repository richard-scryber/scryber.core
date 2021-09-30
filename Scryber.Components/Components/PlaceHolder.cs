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
using Scryber.Components;
using Scryber.Styles;
using Scryber.Data;
using Scryber.PDF.Native;

namespace Scryber.Components
{
    [PDFParsableComponent("PlaceHolder")]
    public class PlaceHolder : ContainerComponent, IPDFInvisibleContainer
    {

        #region public string ParsableContents {get; set;}

        private bool _parsed = false;
        string _data;
        ITemplate _template;

        [PDFAttribute("contents")]
        [PDFElement()]
        public string ParsableContents
        {
            get { return _data; }
            set
            {
                if (_parsed)
                    throw new InvalidOperationException(Errors.CannotChangeTheContentsOfAPlaceHolderOnceParsed);

                _data = value;
                _parsed = false;

            }
        }

        #endregion

        [PDFAttribute("template")]
        public ITemplate Template
        {
            get { return this._template; }
            set
            {
                if(_parsed)
                    throw new InvalidOperationException(Errors.CannotChangeTheContentsOfAPlaceHolderOnceParsed);
                _template = value;
                _parsed = false;
            }
        }

        #region public PDFXmlNamespaceCollection Namespaces

        private PDFXmlNamespaceCollection _nss;

        /// <summary>
        /// Gets the list of namespaces that will be used in the parsable contents so they can be identified and used.
        /// </summary>
        [PDFArray(typeof(XmlNamespaceDeclaration))]
        [PDFElement("Namespaces")]
        public PDFXmlNamespaceCollection Namespaces
        {
            get
            {
                if (null == _nss)
                    _nss = new PDFXmlNamespaceCollection();
                return _nss;
            }

        }

        #endregion

        /// <summary>
        /// Gets the collection of components within this place holder
        /// </summary>
        public ComponentList Contents
        {
            get { return this.InnerContent; }
        }

        /// <summary>
        /// Returns true if this placeholder has parsed the contents to 
        /// </summary>
        public bool Parsed
        {
            get { return this._parsed; }
        }


        public PlaceHolder()
            : this(Scryber.PDFObjectTypes.PlaceHolder)
        {
        }

        protected PlaceHolder(ObjectType type)
            : base(type)
        {
        }

        protected override void DoLoad(PDFLoadContext context)
        {
            base.DoLoad(context);

            //this.EnsureContentsParsed(this.ParsableContents, context);
        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            base.OnDataBinding(context);

            //this.EnsureContentsParsed(this.ParsableContents, context);
        }


        protected override void OnDataBound(PDFDataContext context)
        {
            base.OnDataBound(context);

            //If we did actually parse the contents here, then we should honour the lifcycle and DataBind ourselves.
            //If the content was previously parsed, then this would be automatic as the components were already in the collection.
            bool parsed = this.EnsureContentsParsed(context);

            if (parsed)
                this.Contents.DataBind(context);
        }

        /// <summary>
        /// Removes any parsed (or otherwise) contents from this placeholder and 
        /// </summary>
        public void ResetContents()
        {
            this.Contents.Clear();
            this._parsed = false;

        }

        /// <summary>
        /// Makse sure that if we do have some data and it has not been parsed, them we should parse it.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        /// <returns>True if we did do the actual parsing when this method was called, otherwise false.</returns>
        protected virtual bool EnsureContentsParsed(PDFContextBase context)
        {
            if (!this._parsed)
            {
                IEnumerable<IComponent> all = DoParseContents(context);

                foreach (IComponent child in all)
                {
                    this.InnerContent.Add((Component)child);
                }

                //Do the init and load for these components

                PDFInitContext init = new PDFInitContext(context.Items, context.TraceLog, context.PerformanceMonitor, this.Document);
                PDFLoadContext load = new PDFLoadContext(context.Items, context.TraceLog, context.PerformanceMonitor, this.Document);
                foreach (IComponent child in all)
                {
                    child.Init(init);
                }
                foreach (IComponent child in all)
                {
                    if (child is Component)
                        ((Component)child).Load(load);
                }

                //We did do the parsing so let's return true.

                _parsed = true;
                return _parsed;
            }
            else
                return false;
        }

        protected virtual IEnumerable<IComponent> DoParseContents(PDFContextBase context)
        {
            System.Xml.XmlNamespaceManager mgr = GetNamespaceManager();
            ITemplate gen = null;

            if (!string.IsNullOrEmpty(this.ParsableContents))
                gen = new Data.ParsableTemplateGenerator(this.ParsableContents, mgr);
            else if (null != this.Template)
                gen = this.Template;

            if (null != gen)
            {
                IEnumerable<IComponent> all = gen.Instantiate(0, this);

                return all;
            }
            else
                return new IComponent[] { };
        }


        private System.Xml.XmlNamespaceManager GetNamespaceManager()
        {
            System.Xml.NameTable nt = new System.Xml.NameTable();
            System.Xml.XmlNamespaceManager mgr = new System.Xml.XmlNamespaceManager(nt);
            IRemoteComponent parsed = this.GetParsedParent();
            IDictionary<string, string> parsedNamespaces = null;

            //add the namespaces of the last parsed document so we can infer any declarations
            if (null != parsed)
            {
                parsedNamespaces = parsed.GetDeclaredNamespaces();
                if (null != parsedNamespaces)
                {
                    foreach (string prefix in parsedNamespaces.Keys)
                    {
                        mgr.AddNamespace(prefix, parsedNamespaces[prefix]);
                    }
                }
            }

            if (null != this._nss)
            {
                string found;
                foreach (XmlNamespaceDeclaration dec in _nss)
                {
                    //makes sure this overrides any existing namespace declaration
                    if (null != parsedNamespaces && parsedNamespaces.TryGetValue(dec.Prefix, out found))
                        mgr.RemoveNamespace(dec.Prefix, found);

                    mgr.AddNamespace(dec.Prefix, dec.NamespaceURI);
                }
            }
            return mgr;
        }
    }
}
