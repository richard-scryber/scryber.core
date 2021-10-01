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
using System.Xml;
using System.Xml.XPath;
using Scryber.Components;

namespace Scryber.Data
{
    /// <summary>
    /// The XPath data source base is the root data source for the XPath data binding components (SQLXPathDataSource, XMLDataSource, OBjectXPathDataSource)
    /// </summary>
    public abstract class XPathDataSourceBase : DataSourceBase, IPDFContainerComponent
    {
        private const string LOG_CATEGORY = "XPath DataSource Base";

        //
        // properties
        //

        #region protected IXmlNamespaceResolver Resolver {get;set;}

        private IXmlNamespaceResolver _resolver;

        /// <summary>
        /// Gets or sets the namespace resolver for the datasource
        /// </summary>
        protected IXmlNamespaceResolver Resolver
        {
            get { return _resolver; }
            set { _resolver = value; }
        }

        #endregion

        #region public PDFProviderCommandList Commands {get;set;} + HasCommands {get;}

        private XPathProviderCommandList _cmds;

        /// <summary>
        /// Gets the list of commands in this DataSource
        /// </summary>
        [PDFElement("Commands")]
        [PDFArray(typeof(XPathProviderCommandBase))]
        public XPathProviderCommandList Commands
        {
            get
            {
                if (null == _cmds)
                    _cmds = new XPathProviderCommandList(this);
                return _cmds;
            }
        }

        public bool HasCommands
        {
            get { return null != _cmds && _cmds.Count > 0; }
        }

        private ComponentList _contents = null;

        ComponentList IPDFContainerComponent.Content
        {
            get {
                if (null == _contents)
                    _contents = new ComponentList(this, ObjectTypes.XmlData);
                return _contents;
            }
        }

        bool IPDFContainerComponent.HasContent
        {
            get { return null != _contents && _contents.Count > 0; }
        }

        #endregion

        #region public PDFXmlNamespaceCollection Namespaces {get;set;}

        private PDFXmlNamespaceCollection _nss;

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

        #region public PDFXmlTransformation Transformer {get;set;}

        private XmlTransformation _transform;

        /// <summary>
        /// Gets or set the optional XSLT transformation data for this source
        /// </summary>
        [PDFElement("Transform")]
        public XmlTransformation Transformer
        {
            get { return _transform; }
            set { _transform = value; }
        }

        #endregion

        #region public int CacheDuration {get;set;}

        private int _cachedur = -1;

        /// <summary>
        /// The total number of minutes to cache the data source
        /// </summary>
        [PDFAttribute("cache-duration")]
        public int CacheDuration
        {
            get { return this._cachedur; }
            set { this._cachedur = value; }
        }

        #endregion

        #region protected bool HasBeenBound {get;}

        private bool _hasbeenbound = false;

        /// <summary>
        /// Returns true if this XPathDataSource has been databound yet or not
        /// </summary>
        protected bool HasBeenBound
        {
            get { return this._hasbeenbound; }
        }

        #endregion

        #region protected XPathDataCacheItem LoadedData { get; set; }

        /// <summary>
        /// Gets or sets the data that has been loaded for this source
        /// </summary>
        protected XPathDataCacheItem LoadedData { get; set; }

        #endregion

        //
        // ctor
        //

        #region public PDFXPathDataSourceBase(PDFObjectType type)

        public XPathDataSourceBase(ObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // methods
        //

        #region protected override void DoDataBind(PDFDataContext context, bool includeChildren)

        /// <summary>
        /// Overrides the base implementation to databind the Transformer and Namespaces collections (and their inner elements)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="includeChildren"></param>
        protected override void DoDataBind(PDFDataContext context, bool includeChildren)
        {
            this._hasbeenbound = true;
            base.DoDataBind(context, includeChildren);
            if (includeChildren)
            {
                if (null != this.Commands)
                    this.Commands.DataBind(context);

                if (null != this.Transformer)
                    this.Transformer.DataBind(context);

                if (null != this.Namespaces)
                    this.Namespaces.DataBind(context);
            }
            
        }

        #endregion

        protected override bool DoEvaluateTestExpression(string expr, object withData, PDFDataContext context)
        {
            System.Xml.XPath.XPathNavigator nav;
            if (withData is System.Xml.XPath.XPathNavigator)
                nav = withData as System.Xml.XPath.XPathNavigator;
            else
                throw new NotSupportedException("The XmlDataSource can only perfrom select operations on XPathNavigator data.");
            
            object result;
            if (string.IsNullOrEmpty(expr))
                result = nav.Evaluate("*", context.NamespaceResolver);
            else
            {
                result = nav.Evaluate(expr, context.NamespaceResolver);
            }

            if (result is bool)
                return (bool)result;
            else if (result is System.Xml.XPath.XPathNodeIterator)
            {
                System.Xml.XPath.XPathNodeIterator itter = (System.Xml.XPath.XPathNodeIterator)result;
                return itter.Count > 0;
            }
            else
                return null != result;
        }

        protected override object DoEvaluateExpression(string expr, object withData, PDFDataContext context)
        {
            System.Xml.XPath.XPathNavigator nav;
            if (withData is System.Xml.XPath.XPathNavigator)
                nav = withData as System.Xml.XPath.XPathNavigator;
            else
                throw new NotSupportedException("The XmlDataSource can only perfrom select operations on XPathNavigator data.");

            object value = nav.Evaluate(expr, context.NamespaceResolver);
            if(value is XPathNodeIterator)
            {
                XPathNodeIterator itter = value as XPathNodeIterator;
                if (itter.MoveNext())
                    value = itter.Current.Value;
                else
                    value = null;
            }
            return value;
        }

        #region protected override object DoSelectData(string path, object withData, PDFDataContext context)

        protected override object DoSelectData(string path, object withData, PDFDataContext context)
        {
            System.Xml.XPath.XPathNavigator nav;
            if (withData is System.Xml.XPath.XPathNavigator)
                nav = withData as System.Xml.XPath.XPathNavigator;
            else
                throw new NotSupportedException("The XmlDataSource can only perfrom select operations on XPathNavigator data.");

            if (string.IsNullOrEmpty(path))
                return nav.Select("*", context.NamespaceResolver);
            else
            {
                return nav.Select(path, context.NamespaceResolver);
            }
        }

        #endregion

        #region protected override object DoSelectData(string path, PDFDataContext context)

        /// <summary>
        /// Overrides the base implementation to load the actual data at the path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="context"></param>
        /// <param name="root">If true then this is the top level selection of data and should use this sources assigned data. If false then it should use the current data context.</param>
        /// <returns></returns>
        protected override object DoSelectData(string path, PDFDataContext context)
        {
            
            //Get sthe source data from the implementation

            string key = this.GetCacheKey(context);

            XPathDataCacheItem item = null;

            
            if (null != this.LoadedData)
            {
                item = this.LoadedData;
            }
            else if (this.CacheDuration <= 0 || !this.TryGetCachedData(this.Type.ToString(), key, out item))
            {
                if (context.ShouldLogVerbose)
                    context.TraceLog.Begin(TraceLevel.Verbose, LOG_CATEGORY, "Directly loading the data for datasource component '" + this.ID + "' with cache key '" + key + "'");

                //Load the actual source data.
                item = LoadSourceXPathData(context);
                this.LoadedData = item;

                //Quick fall-out (don't cache it, as not a normal situation)
                if (null == item)
                {
                    context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "No Data was returned from the Datasource '" + this.ID + "'.");
                    return null;
                }

                //Transform as required
                XPathNavigator nav = item.Navigator;
                if (null != this.Transformer)
                    nav = this.Transformer.TransformData(item.Navigator, this.CacheDuration, this, context);

                if (this.CacheDuration > 0)
                {
                    this.AddToCache(this.Type.ToString(), key, item);

                    if (context.ShouldLogVerbose)
                        context.TraceLog.End(TraceLevel.Verbose, LOG_CATEGORY, "Completed loading the data for datasource component '" + this.ID + "' and added it to the cache with key '" + key + "'");
                    else if (context.ShouldLogMessage)
                        context.TraceLog.Add(TraceLevel.Message, LOG_CATEGORY, "Completed loading the data for datasource component '" + this.ID + "' and added it to the cache with key '" + key + "'");
                }
                else if (context.ShouldLogVerbose)
                    context.TraceLog.End(TraceLevel.Verbose, LOG_CATEGORY, "Completed loading the data for datasource component '" + this.ID + "'. Not cached");
                else if (context.ShouldLogMessage)
                    context.TraceLog.Add(TraceLevel.Message, LOG_CATEGORY, "Completed loading the data for datasource component '" + this.ID + "'. Not cached");
            }
            else //cache hit
            {
                this.LoadedData = item;
                if (context.ShouldLogMessage)
                    context.TraceLog.Add(TraceLevel.Message, LOG_CATEGORY, "Cache hit on datasource '" + this.ID + "' with cache key '" + key + "'");
            }

            //We have the data
            //Now we need a resolver for the prefixes and select statements.

            if (null == this.Resolver)
                this.Resolver = this.Document.CreateNamespaceResolver(this.Namespaces, item.Navigator.NameTable);

            //Push this onto the context
            context.NamespaceResolver = this.Resolver;

            XPathNodeIterator itter;

            //Finally return the select to return an itterator
            if (string.IsNullOrEmpty(path))
            {
                itter = item.Navigator.Select("*", context.NamespaceResolver);

                if (itter.Count == 0)
                    context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "ZERO items were retrieved from the Datasource '" + this.ID + "'");
            }
            else
            {
                itter = item.Navigator.Select(path, context.NamespaceResolver);

                if (itter.Count == 0)
                    context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "ZERO items were retrieved from the Datasource '" + this.ID + "' based on the select path '" + path + "'");
            }

            return itter;
        }

        #endregion

        #region public virtual PDFDataSchema GetDataSchema(string path, PDFDataContext context)

        /// <summary>
        /// Returns the PDFDataScema associated with this source. 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="context"></param>
        /// <returns>The data schema associated with the source</returns>
        /// <exception cref="System.NotSupportedException" >Thrown if this data source does not support schema extraction</exception>
        public override PDFDataSchema GetDataSchema(string path, PDFDataContext context)
        {
            System.Data.DataSet ds;
            if (this.HasData(out ds))
            {
                PDFDataSchema schema = DataSetSchemaGenerator.CreateSchemaFromSet(ds, context);
                if (!string.IsNullOrEmpty(path))
                {
                    if (path == schema.RootPath)
                        return schema;
                    else if (!path.StartsWith(schema.RootPath))
                        return null;
                    else
                    {
                        foreach (var one in schema.Items)
                        {
                            if (one.FullPath == path)
                            {
                                PDFDataSchema child = new PDFDataSchema(one.FullPath, one.Children);
                                return child;
                            }
                        }
                        return null;
                    }
                }
                else
                    return schema;
            }
            else
                return base.GetDataSchema(path, context);
        }

        #endregion

        
        protected bool HasData(out System.Data.DataSet ds)
        {
            ds = this.LoadedData == null ? null : this.LoadedData.DataSet;
            return null != ds;
        }

        protected System.Data.DataSet CreateSet()
        {
            if (null == this.LoadedData)
            {
                this.LoadedData = new XPathDataCacheItem(new System.Data.DataSet(this.ID), null);
                return this.LoadedData.DataSet;
            }
            else
                return this.LoadedData.DataSet;
        }

        #region protected virtual string GetCacheKey(PDFDataContext context)
        
        /// <summary>
        /// Returns a string that uniquely identifies this instances loaded data
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual string GetCacheKey(PDFDataContext context)
        {
            return this.UniqueID;
        }

        #endregion

        protected class XPathDataCacheItem
        {
            public System.Data.DataSet DataSet { get; private set; }
            public XPathNavigator Navigator { get; set; }

            public XPathDataCacheItem(System.Data.DataSet ds, XPathNavigator navigator)
            {
                this.DataSet = ds;
                this.Navigator = navigator;
            }
        }

        #region protected virtual bool TryGetCachedData(string type, string key, out XPathDataCacheItem item)

        /// <summary>
        /// Attempts to retrieve any stored data from the document cache.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="nav"></param>
        /// <returns></returns>
        protected virtual bool TryGetCachedData(string type, string key, out XPathDataCacheItem item)
        {
            object value;
            Document doc = this.Document;
            
            if(null == doc)
                throw new NullReferenceException("This data source is not part of the document heirarchy and cannot use the caching capabilities available");
            
            ICacheProvider cacheProv = doc.CacheProvider;
            if (null != cacheProv)
            {
                if (cacheProv.TryRetrieveFromCache(type, key, out value))
                {
                    if (!(value is XPathDataCacheItem))
                        throw new InvalidCastException("The data returned from the cache for this XPath datasource '" + this.ID + "' was not an XPathNavigator.");
                    
                    item = (XPathDataCacheItem)value;
                    return true;
                }
            }

            item = null;
            return false;
        }

        #endregion

        #region protected virtual void AddToCache(string type, string key, XPathDataCacheItem item)

        /// <summary>
        /// Adds the XPath document to the documents cache provider.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="nav"></param>
        protected virtual void AddToCache(string type, string key, XPathDataCacheItem item)
        {
            Document doc = this.Document;
            
            if(null == doc)
                throw new NullReferenceException("This data source is not part of the document heirarchy and cannot use the caching capabilities available");
            
            ICacheProvider cacheProv = doc.CacheProvider;
            if (null != cacheProv)
            {
                DateTime expires = DateTime.Now.AddMinutes(this.CacheDuration);
                cacheProv.AddToCache(type, key, item, expires);
            }
        }

        #endregion

        
        /// <summary>
        /// Method that all inheriting classes must override to actually load the source data.
        /// </summary>
        /// <param name="context">The current data context</param>
        /// <returns>An XPath navigator that represents the current source</returns>
        protected abstract XPathDataCacheItem LoadSourceXPathData(PDFDataContext context);
    }
}
