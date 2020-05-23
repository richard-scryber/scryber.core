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
using Scryber.Components;
using System.Xml;
using System.Xml.XPath;

namespace Scryber.Data
{
    /// <summary>
    /// Loads an XML file as a data source that can be cached and then enumerated over.
    /// </summary>
    [PDFParsableComponent("XMLDataSource")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datasource")]
    public class PDFXMLDataSource : PDFXPathDataSourceBase
    {

        #region public string SourcePath {get;set;}

        private string _source;

        /// <summary>
        /// The source data file for the xml.
        /// </summary>
        [PDFAttribute("source-path")]
        public string SourcePath
        {
            get { return _source; }
            set { _source = value; }
        }

        #endregion
        
        #region public System.Xml.XPath.XPathNavigator XPathNavData

        /// <summary>
        /// Local copy - It may be that we have multiple references and to this source and we don't have any caching set.
        /// So keep a local copy just for this purpose.
        /// </summary>
        private System.Xml.XPath.XPathNavigator _nav = null;

        /// <summary>
        /// XmlData has been deprecated to make the disinction between the XmlNodeData and XPathNavData 
        /// options in the datasource more evident.
        /// </summary>
        [Obsolete("Use XPathNavData instead. XmlData has been deprecated to make the disinction between the XmlNodeData and XPathNavData options in the datasource more evident. This existing property will be removed in a future version.",false)]
        [PDFAttribute("xml-data")]
        public System.Xml.XPath.XPathNavigator XmlData
        {
            get { return _nav; }
            set { _nav = value; }
        }

        /// <summary>
        /// Gets or sets the 
        /// </summary>
        [PDFAttribute("xpath-data")]
        public System.Xml.XPath.XPathNavigator XPathNavData
        {
            get { return _nav; }
            set { _nav = value; }
        }

        #endregion

        #region public System.Xml.XmlNode XmlNodeData

        private System.Xml.XmlNode _node;

        /// <summary>
        /// Gets or sets an XmlNode as the data to use within this source (XPath Data if set will be used as a priority)
        /// </summary>
        [PDFElement("Data")]
        [PDFAttribute("xml-node-data")]
        public System.Xml.XmlNode XmlNodeData
        {
            get { return _node; }
            set { _node = value; }
        }

        #endregion

        /// <summary>
        /// Override the base implementation to return true for the dataset schema
        /// </summary>
        public override bool SupportsDataSchema
        {
            get { return true; }
        }

        //
        // .ctors
        //

        #region public PDFXMLDataSource()

        /// <summary>
        /// Creates a new PDFXMLDatasource 
        /// </summary>
        public PDFXMLDataSource()
            : this(PDFObjectTypes.XmlData)
        {
        }

        #endregion

        #region protected PDFXMLDataSource(PDFObjectType type)

        protected PDFXMLDataSource(PDFObjectType type)
            : base(type)
        {
        }

        #endregion

        //
        // implementation
        //

        #region protected override XPathNavigator LoadSourceXPathData(PDFDataContext context)

        /// <summary>
        /// Overrides the abstract base method to return the source data.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <remarks>
        /// XPathNavData has the highest priority, if null then XmlNodeData will be checked, if this
        /// is also null then the SourcePath will be used. If this is also not set, then null will be
        /// retruned from this method.
        /// </remarks>
        protected override PDFXPathDataSourceBase.XPathDataCacheItem LoadSourceXPathData(PDFDataContext context)
        {
            System.Xml.XPath.XPathNavigator nav = null;
            System.Data.DataSet dataSet = null;

            if(this.HasCommands)
            {
                
                if (!this.HasData(out dataSet)) {

                    dataSet = this.CreateSet();
                    
                    foreach (var cmd in this.Commands)
                    {
                        cmd.EnsureDataLoaded(this, dataSet, context);
                    }
                }
                
                nav = this.ConvertDataSetToXPath(dataSet, dataSet.Tables[0].TableName, context);
            }
            if (null != this.XPathNavData)
            {
                nav = this.XPathNavData;
            }
            else if (null != this.XmlNodeData)
            {
                nav = this.XmlNodeData.CreateNavigator();
            }
            else if (!string.IsNullOrEmpty(this.SourcePath))
            {
                XmlDocument doc = this.LoadXMLDocument(this.SourcePath, context);
                nav = doc.CreateNavigator();
            }
            return new XPathDataCacheItem(dataSet, nav);
        }

        #endregion

        #region private bool GetCommandToExecute(string cmdName, out PDFProviderCommand found)

        /// <summary>
        /// Looks in this instances commands for a matching command -
        /// either the first command if name is not set, or the command with the matching name.
        /// </summary>
        /// <param name="cmdName"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        private bool GetCommandToExecute(string cmdName, out PDFXPathProviderCommandBase found)
        {
            if (string.IsNullOrEmpty(cmdName))
            {
                if (this.HasCommands)
                    found = this.Commands[0];
                else
                    found = null;

                return found != null;
            }
            else
            {
                if (this.HasCommands)
                    found = this.Commands[cmdName];
                else
                    found = null;

                if (null == found)
                    throw new NullReferenceException(string.Format(Errors.CommandWithNameCannotBeFound, cmdName, this.ID));

                return true;
            }
        }

        #endregion

        #region private System.Xml.XmlDocument LoadXMLDocument(string sourcepath)

        /// <summary>
        /// private implementation to load the data from the source.
        /// </summary>
        /// <param name="sourcepath"></param>
        /// <returns></returns>
        private System.Xml.XmlDocument LoadXMLDocument(string sourcepath, PDFDataContext context)
        {
            if (string.IsNullOrEmpty(sourcepath))
                throw new ArgumentNullException(string.Format(Errors.NoSourcePathDefinedOnXmlDataSource, this.ID));


            PDFDocument pdf = this.Document;
            string path = this.MapPath(sourcepath);

            System.Xml.XmlDocument doc = null;

            try
            {
                doc = new System.Xml.XmlDocument();
                doc.Load(path);
            }
            catch (Exception ex)
            {
                throw new PDFDataException(string.Format(Errors.CouldNotLoadTheDataFromTheSourcePath, "XML", sourcepath), ex);
            }
            return doc;
        }

        #endregion



        #region protected virtual XPathNavigator ConvertDataSetToXPath(DataSet ds, PDFDataContext context)

        /// <summary>
        /// Converts the provided dataset into an XPathNavigator instance.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual XPathNavigator ConvertDataSetToXPath(System.Data.DataSet ds, string rootTable, PDFDataContext context)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                //ds.Tables[rootTable].DefaultView.ToTable().WriteXml(ms);
                ds.WriteXml(ms);
                ms.Flush();
                ms.Position = 0;
                XPathDocument doc = new XPathDocument(ms);
                return doc.CreateNavigator();
            }
        }

        #endregion

       

    }
}
