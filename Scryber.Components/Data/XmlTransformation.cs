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
using System.Xml.Xsl;

namespace Scryber.Data
{
    public class XmlTransformation : IBindableComponent
    {

        private const string XSLTCacheType = "Scryber.PDF.XSLT";
        private const bool XSLTUseDebug = false;


        //
        // events to support databinding
        //

        #region public event PDFDataBindEventHandler DataBinding;

        public event DataBindEventHandler DataBinding;

        protected virtual void OnDataBinding(DataBindEventArgs args)
        {
            if (null != this.DataBinding)
                this.DataBinding(this, args);
        }

        #endregion

        #region public event PDFDataBindEventHandler DataBound;

        public event DataBindEventHandler DataBound;

        protected virtual void OnDataBound(DataBindEventArgs args)
        {
            if (null != this.DataBound)
                this.DataBound(this, args);
        }

        #endregion

        //
        // properties
        //

        #region public string XSLTPath {get;set;}

        /// <summary>
        /// Gets or sets the path to the XSLT with which to transform the data
        /// </summary>
        [PDFAttribute("path")]
        public string XSLTPath
        {
            get;
            set;
        }

        #endregion

        #region public System.Xml.Xsl.XslCompiledTransform Transformer {get;set;}

        private XslCompiledTransform _transformer;

        /// <summary>
        /// Gets or sets the XSL compiled transformer for this 
        /// </summary>
        public XslCompiledTransform Transformer
        {
            get { return _transformer; }
            set { _transformer = value; }
        }

        #endregion

        #region public PDFXsltArgumentList Arguments {get;set;} + bool HasArguments {get;}

        private XsltArgumentList _args;

        /// <summary>
        /// Gets or sets the list of arguments for this transformer
        /// </summary>
        [PDFArray(typeof(XsltArgumentBase))]
        [PDFElement("")]
        public XsltArgumentList Arguments
        {
            get
            {
                if (null == _args)
                    _args = new XsltArgumentList();
                return _args;
            }
            set
            {
                _args = value;
            }
        }

        public bool HasArguments
        {
            get { return null != _args && _args.Count > 0; }
        }

        #endregion

        //
        // .ctor
        //


        public System.Xml.XPath.XPathNavigator TransformData(System.Xml.XPath.XPathNavigator nav, int cacheduration, IDataSource source, DataContext context)
        {
            //Check we have something to use for a transformation.
            if (string.IsNullOrEmpty(this.XSLTPath) && null == this.Transformer)
                return nav;

            System.Xml.XmlWriter xmlWriter = null;
            System.IO.MemoryStream memoryStream = null;
            System.IO.StreamWriter streamWriter = null;
            System.Xml.XmlDocument result = null;
            
            System.Xml.XmlDocument output = new System.Xml.XmlDocument();
            try
            {
                System.Xml.Xsl.XslCompiledTransform trans = this.DoGetTransformer(cacheduration, source, context);
                System.Xml.Xsl.XsltArgumentList args = this.DoGetArguments(context);
                memoryStream = new System.IO.MemoryStream();
                streamWriter = new System.IO.StreamWriter(memoryStream, Encoding.UTF8);

                System.Xml.XmlWriterSettings writerSettings = CreateWriterSettings();
                xmlWriter = System.Xml.XmlWriter.Create(streamWriter, writerSettings);

                trans.Transform(nav, args, xmlWriter);
                xmlWriter.Flush();
                streamWriter.Flush();

                result = new System.Xml.XmlDocument();
                memoryStream.Position = 0;
                result.Load(memoryStream);

            }
            catch (Exception ex)
            {
                throw new PDFDataException(Errors.CouldNotTransformInputData, ex);
            }
            finally
            {
                if (null != xmlWriter)
                    xmlWriter.Close();
                if (null != streamWriter)
                    streamWriter.Dispose();
                if (null != memoryStream)
                    memoryStream.Dispose();
            }

            return result.CreateNavigator();
        }

        protected virtual System.Xml.Xsl.XslCompiledTransform DoGetTransformer(int cacheduration, IDataSource source, DataContext context)
        {

            if (null == _transformer)
            {
                string path = this.XSLTPath;
                if (string.IsNullOrEmpty(path))
                    throw new NullReferenceException(string.Format(Errors.XSLTPathOrTransformerNotSetOnInstance, source.ID));

                path = source.MapPath(path);
                ICacheProvider cache = ((Scryber.Components.Document)source.Document).CacheProvider;
                System.Xml.Xsl.XslCompiledTransform transformer;

                object found;

                if (cacheduration > 0 && cache.TryRetrieveFromCache(XSLTCacheType, path, out found))
                    transformer = (System.Xml.Xsl.XslCompiledTransform)found;
                else
                {
                    
                    transformer = new System.Xml.Xsl.XslCompiledTransform(XSLTUseDebug);
                    try
                    {
                        transformer.Load(path);
                    }
                    catch (Exception ex)
                    {
                        throw new PDFDataException(string.Format(Errors.XSLTCouldNotBeLoadedFromPath, path), ex);
                    }

                    if (cacheduration > 0)
                        cache.AddToCache(XSLTCacheType, path, transformer, new TimeSpan(0, cacheduration, 0));
                }
                
                _transformer = transformer;
            }

            return _transformer;
        }


        protected virtual System.Xml.Xsl.XsltArgumentList DoGetArguments(DataContext context)
        {
            System.Xml.Xsl.XsltArgumentList args = new System.Xml.Xsl.XsltArgumentList();
            if (this.HasArguments)
            {
                foreach (XsltArgumentBase arg in this.Arguments)
                {
                    string name = arg.Name;
                    string ns = null == arg.Namespace ? string.Empty : arg.Namespace;
                    object value = arg.GetValue(context);
                    args.AddParam(name, ns, value);
                }
            }
            return args;
        }



        protected virtual System.Xml.XmlWriterSettings CreateWriterSettings()
        {
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.ConformanceLevel = System.Xml.ConformanceLevel.Auto;
            settings.Indent = true;
            settings.NewLineHandling = System.Xml.NewLineHandling.Entitize;
            return settings;
        }


        #region public void DataBind(PDFDataContext context)

        /// <summary>
        /// Supports the databinding capabilites of the parameter by raising the events
        /// </summary>
        /// <param name="context"></param>
        public virtual void DataBind(DataContext context)
        {
            if (null != this.DataBinding || null != this.DataBound)
            {
                DataBindEventArgs args = new DataBindEventArgs(context);
                this.OnDataBinding(args);

                this.OnDataBound(args);
            }
            if (this.HasArguments)
            {
                this.Arguments.DataBind(context);
            }
        }

        #endregion
    }
}
