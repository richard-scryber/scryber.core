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
using System.Xml;
using Scryber.Components;

namespace Scryber.Data
{
    public class PDFParsableTemplateGenerator : IPDFTemplate, IPDFTemplateGenerator
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

        public IDictionary<string,string> NamespacePrefixMappings { get; set; }

        public PDFParsableTemplateGenerator()
        {
            _initialised = false;
        }

        public PDFParsableTemplateGenerator(string xmlcontent, System.Xml.XmlNamespaceManager namespaces)
            : this()
        {
            this.XmlContent = xmlcontent;
            this.NamespacePrefixMappings = GetNamespacesFromManager(namespaces);

            //InitTemplate(xmlcontent, namespaces);
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

            //Append the actual content
            sb.Append(xmlcontent);

            //Close the parsabletemplate element
            sb.Append("</Content></");

            if (!string.IsNullOrEmpty(dataprefix))
            {
                sb.Append(dataprefix);
                sb.Append(":");
            }
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

        public IEnumerable<IPDFComponent> Instantiate(int index, IPDFComponent owner)
        {

            if (!_initialised)
                this.InitTemplate(this.XmlContent, this.NamespacePrefixMappings);

            if (null == owner)
                throw new ArgumentNullException("owner");

            using (System.IO.StringReader sr = new System.IO.StringReader(this._toparse))
            {
                //Get the closest remote component
                IPDFRemoteComponent remote = this.GetRemoteComponent(owner);
                IPDFDocument doc = owner.Document;
                IPDFComponent comp;
                if (doc is IPDFTemplateParser)
                {
                    comp = ((IPDFTemplateParser)doc).ParseTemplate(remote,sr);
                }
                else
                    throw RecordAndRaise.NullReference(Errors.ParentDocumentMustBeTemplateParser);

                if (!(comp is PDFTemplateInstance))
                    throw RecordAndRaise.InvalidCast(Errors.CannotConvertObjectToType, comp.GetType(), typeof(PDFTemplateInstance));

                PDFTemplateInstance template = (PDFTemplateInstance)comp;
                List<IPDFComponent> all = new List<IPDFComponent>(1);

                all.Add(template);
                return all;
            }
            
        }

        protected IPDFRemoteComponent GetRemoteComponent(IPDFComponent owner)
        {
            IPDFComponent comp = owner;
            while (null != comp)
            {
                if (comp is IPDFRemoteComponent && !String.IsNullOrEmpty(((IPDFRemoteComponent)comp).LoadedSource))
                {
                    return ((IPDFRemoteComponent)comp);
                }
                else
                    comp = comp.Parent;
            }
            
            IPDFDocument doc = owner.Document;
            if (doc is IPDFRemoteComponent)
                return ((IPDFRemoteComponent)doc);
            else
                return null;
        }

        
    }
}
