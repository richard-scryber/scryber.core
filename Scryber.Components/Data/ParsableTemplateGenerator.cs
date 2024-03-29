﻿/*  Copyright 2012 PerceiveIT Limited
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
using System.Xml;
using Scryber.Components;

namespace Scryber.Data
{
    public class ParsableTemplateGenerator : ITemplate, IPDFTemplateGenerator, IPDFDataTemplateGenerator
    {
        
        private string _toparse;
        private bool _initialised;

        /// <summary>
        /// Gets or sets the XML content for this template
        /// </summary>
        public string XmlContent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the element that was parsed.
        /// </summary>
        public string ElementName { get; set; }

        /// <summary>
        /// Gets or sets the prefix stem for the DataStyleIdentifier
        /// </summary>
        public string DataStyleStem { get; set; }

        /// <summary>
        /// Gets or Sets the flag that indicates if the template should be generated with a style identifier.
        /// Default is true, as this improves speed and caches the full style - in cases where there is an issue
        /// </summary>
        public bool UseDataStyleIdentifier { get; set; }


        [PDFAttribute("class")]
        public string StyleClass { get; set; }

        [PDFAttribute("style")]
        [PDFElement("Style")]
        public Scryber.Styles.Style Style { get; set; }


        public bool IsBlock { get; set; }


        public IDictionary<string,string> NamespacePrefixMappings { get; set; }

        public ParsableTemplateGenerator()
        {
            _initialised = false;
        }

        public ParsableTemplateGenerator(string xmlcontent, System.Xml.XmlNamespaceManager namespaces)
            : this()
        {
            this.XmlContent = xmlcontent;
            this.NamespacePrefixMappings = GetNamespacesFromManager(namespaces);

            //InitTemplate(xmlcontent, namespaces);
        }

        public ParsableTemplateGenerator(string xmlContent, IDictionary<string, string> namespaceMappings)
        {
            this.XmlContent = xmlContent;
            this.NamespacePrefixMappings = namespaceMappings;
        }

        public void InitTemplate(string xmlcontent, System.Xml.XmlNamespaceManager namespaces)
        {
            if (null == namespaces)
                throw new ArgumentNullException("namespaces");

            if (null == xmlcontent)
                xmlcontent = string.Empty;

            this.XmlContent = xmlcontent;
            this.NamespacePrefixMappings = GetNamespacesFromManager(namespaces);

            this.InitTemplate(this.XmlContent, this.NamespacePrefixMappings);

            
        }

        protected virtual IDictionary<string,string> GetNamespacesFromManager(XmlNamespaceManager manager)
        {
            return manager.GetNamespacesInScope(XmlNamespaceScope.All);
        }

        public void InitTemplate(string xmlcontent, IDictionary<string,string> prefixNamespaces)
        {
            if (null == prefixNamespaces)
                prefixNamespaces = new Dictionary<string, string>();

            string doc = WrapContentAsDocument(xmlcontent, prefixNamespaces);

            _toparse = doc;
            _initialised = true;
        }

        /// <summary>
        /// Takes the inner content for databinding and wraps a single element around it, then returns the full string
        /// </summary>
        /// <param name="xmlcontent"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        protected virtual string WrapContentAsDocument(string xmlcontent, IDictionary<string, string> prefixNamespaces)
        {
            string ns = Const.PDFDataNamespace;

            StringBuilder sb = new StringBuilder();
            string dataprefix = GetOrAddNamespace(ns, "data", prefixNamespaces);
            
            sb.Append("<");
            if(!string.IsNullOrEmpty(dataprefix))
            {
                sb.Append(dataprefix);
                sb.Append(":");
            }

            if (this.IsBlock)
                sb.Append("TemplateBlockInstance ");
            else
                sb.Append("TemplateInstance ");

            
            foreach (KeyValuePair<string,string> declared in prefixNamespaces)
            {
                sb.Append("xmlns");
                if (!string.IsNullOrEmpty(declared.Key))
                {
                    sb.Append(":");
                    sb.Append(declared.Key);
                }
                sb.Append("='");
                sb.Append(declared.Value);
                sb.Append("' ");
            }

            //Declare the ParsableTemplate namespace as the DataNamespace
            //sb.Append("xmlns='");
            //sb.Append(ns);
            sb.Append("><Content>");

            if(xmlcontent.StartsWith("<?xml"))
            {
                int substr = xmlcontent.IndexOf("?>");
                xmlcontent = xmlcontent.Substring(substr + 2);
            }
            //Append the actual content
            sb.Append(xmlcontent);

            //Close the parsabletemplate element
            sb.Append("</Content></");

            if (!string.IsNullOrEmpty(dataprefix))
            {
                sb.Append(dataprefix);
                sb.Append(":");
            }

            if (this.IsBlock)
                sb.Append("TemplateBlockInstance");
            else
                sb.Append("TemplateInstance");

            sb.Append(">");
            
            return sb.ToString();
        }

        /// <summary>
        /// Looks for an existing namespace declaration (that could be mapped in the options), and
        /// returns the prefix. If there is no exsiting, then it's added with the defined prefix and returned.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="prefix"></param>
        /// <param name="declared"></param>
        /// <returns>The prefix for the namepsace</returns>
        private string GetOrAddNamespace(string ns, string prefix, IDictionary<string,string> declared)
        {
            var service = ServiceProvider.GetService<Scryber.IScryberConfigurationService>();
            string xml = service.ParsingOptions.GetXmlNamespaceForAssemblyNamespace(ns);

            if(string.IsNullOrEmpty(xml))
            {
                //There is no mapping for the namespace, so use the original
                xml = ns;
            }

            foreach (var kvp in declared)
            {
                if(kvp.Value.Equals(xml))
                {
                    //We have found the declaration, so don't need to add,
                    //just return the key.
                    return kvp.Key;
                }
            }

            //not found so add and return
            declared.Add(prefix, xml);
            return prefix;
        }


        //
        // IPDFTemplate Instantiate method.
        //

        public IEnumerable<IComponent> Instantiate(int index, IComponent owner)
        {

            if (!_initialised)
                this.InitTemplate(this.XmlContent, this.NamespacePrefixMappings);

            if (null == owner)
                throw new ArgumentNullException("owner");

            using (System.IO.StringReader sr = new System.IO.StringReader(this._toparse))
            {
                //Get the closest remote component
                IRemoteComponent remote = this.GetRemoteComponent(owner);
                IDocument doc = owner.Document;
                IComponent comp;
                if (doc is ITemplateParser)
                {
                    comp = ((ITemplateParser)doc).ParseTemplate(remote,sr);
                }
                else
                    throw RecordAndRaise.NullReference(Errors.ParentDocumentMustBeTemplateParser);

                if (this.IsBlock)
                {
                    if (!(comp is TemplateBlockInstance))
                        throw RecordAndRaise.InvalidCast(Errors.CannotConvertObjectToType, comp.GetType(), typeof(TemplateBlockInstance));

                    TemplateBlockInstance template = (TemplateBlockInstance)comp;

                    if (null != this.Style && (template is IStyledComponent))
                        this.Style.MergeInto((template as IStyledComponent).Style);

                    template.StyleClass = this.StyleClass;
                    template.ElementName = this.ElementName;
                }
                else
                {
                    if (!(comp is TemplateInstance))
                        throw RecordAndRaise.InvalidCast(Errors.CannotConvertObjectToType, comp.GetType(), typeof(TemplateInstance));

                    TemplateInstance template = (TemplateInstance)comp;

                    if (null != this.Style && (template is IStyledComponent))
                        this.Style.MergeInto((template as IStyledComponent).Style);

                    template.StyleClass = this.StyleClass;
                    template.ElementName = this.ElementName;
                }

                if(comp is Component && owner is Component && this.UseDataStyleIdentifier)
                {
                    var stem = this.DataStyleStem;
                    if (string.IsNullOrEmpty(stem))
                        stem = ((Component)owner).UniqueID;

                    DataStyleIdentifierVisitor visitor = new DataStyleIdentifierVisitor(stem, 1);
                    visitor.PushToComponents(comp as Component);
                }

                List<IComponent> all = new List<IComponent>(1);

                all.Add(comp);
                return all;
            }
            
        }

        protected IRemoteComponent GetRemoteComponent(IComponent owner)
        {
            IComponent comp = owner;
            while (null != comp)
            {
                if (comp is IRemoteComponent && !String.IsNullOrEmpty(((IRemoteComponent)comp).LoadedSource))
                {
                    return ((IRemoteComponent)comp);
                }
                else
                    comp = comp.Parent;
            }
            
            IDocument doc = owner.Document;
            if (doc is IRemoteComponent)
                return ((IRemoteComponent)doc);
            else
                return null;
        }


        private class DataStyleIdentifierVisitor
        {

            private string _stem { get; set; }
            private int _nextIndex;
            private StringBuilder _buffer;

            public DataStyleIdentifierVisitor(string stem, int nextIndex)
            {
                if (string.IsNullOrEmpty(stem))
                    throw new ArgumentNullException("stem");

                this._stem = stem;
                this._nextIndex = nextIndex;
                this._buffer = new StringBuilder(this._stem.Length + 3);
            }

            public void PushToComponents(Component component)
            {
                var next = this.NextIdentifier();
                if (component is IDataStyledComponent)
                    (component as IDataStyledComponent).DataStyleIdentifier = next;

                if(component is IPDFDataTemplateGenerator)
                {
                    var dt = component as IPDFDataTemplateGenerator;
                    if (string.IsNullOrEmpty(dt.DataStyleStem))
                        dt.DataStyleStem = next;
                }

                if((component is IContainerComponent) && (component as IContainerComponent).HasContent)
                {
                    var container = (IContainerComponent)component;

                    foreach (var comp in container.Content)
                    {
                        if (comp is Component)
                            this.PushToComponents(comp as Component);
                    }
                }

            }

            protected virtual string NextIdentifier()
            {
                this._buffer.Clear();
                this._buffer.Append(this._stem);
                this._buffer.Append("_");
                this._buffer.Append(this._nextIndex);
                this._nextIndex++;

                return this._buffer.ToString();
            }
        }
        
    }
}
