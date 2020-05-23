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

        public PDFParsableTemplateGenerator()
        {
            _initialised = false;
        }

        public PDFParsableTemplateGenerator(string xmlcontent, System.Xml.XmlNamespaceManager namespaces)
            : this()
        {
            InitTemplate(xmlcontent, namespaces);
        }

        public void InitTemplate(string xmlcontent, System.Xml.XmlNamespaceManager namespaces)
        {
            if (null == namespaces)
                throw new ArgumentNullException("namespaces");

            if (null == xmlcontent)
                xmlcontent = string.Empty;

            string doc = WrapContentAsDocument(xmlcontent, namespaces);
            _toparse = doc;
            _initialised = true;
        }

        /// <summary>
        /// Takes the inner content for databinding and wraps a single element around it, then returns the full string
        /// </summary>
        /// <param name="xmlcontent"></param>
        /// <param name="namespaces"></param>
        /// <returns></returns>
        protected virtual string WrapContentAsDocument(string xmlcontent, System.Xml.XmlNamespaceManager namespaces)
        {
            string ns = Const.PDFDataNamespace;

            StringBuilder sb = new StringBuilder();
            string prefix = namespaces.LookupPrefix(ns);
            sb.Append("<");
            sb.Append("TemplateInstance ");

            IDictionary<string,string> all = namespaces.GetNamespacesInScope(XmlNamespaceScope.All);
            foreach (KeyValuePair<string,string> declared in all)
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
            sb.Append("xmlns='");
            sb.Append(ns);
            sb.Append("' ><Content>");

            //Append the actual content
            sb.Append(xmlcontent);

            //Close the parsabletemplate element
            sb.Append("</Content></");

            sb.Append("TemplateInstance");
            sb.Append(">");
            
            return sb.ToString();
        }

        public IEnumerable<IPDFComponent> Instantiate(int index, IPDFComponent owner)
        {
            
            if (!_initialised)
                throw new InvalidOperationException(Errors.TemplateHasNotBeenInitialised);

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
