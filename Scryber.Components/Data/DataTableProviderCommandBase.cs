using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml.XPath;
using System.Xml;

namespace Scryber.Data
{
    public abstract class DataTableProviderCommandBase : XPathProviderCommandBase, IDataSetProviderCommand
    {

        //
        // properites
        //

        #region public string ElementName {get;set;}

        private string _elename;

        /// <summary>
        /// Gets or sets the element name. The name that will be used when generating the XML for each item returned from this command
        /// </summary>
        [PDFAttribute("element-name")]
        [PDFDesignable("Element Name", Category = "Data",Priority = 3, Type ="String", Validate = "/^[a-z][a-z0-9]+$/i")]
        public string ElementName
        {
            get { return _elename; }
            set { _elename = value; }
        }

        #endregion

        #region public string ElementNamespaceUri { get; set; }

        /// <summary>
        /// Gets or sets the namespace uri of the root element
        /// </summary>
        [PDFAttribute("element-namespace")]
        [PDFDesignable("Element Namespace", Category = "Data", Priority = 3, Type = "String", Validate = "/^(http|https):\\/\\/[^ \"]+$/")]
        public string ElementNamespaceUri { get; set; }

        #endregion

        #region public string AsAttributes
        /// <summary>
        /// Gets or sets the comma separated list names of the returned data fields that should be encoded as 
        /// attributes rather than inner elements. Default is none (blank), 
        /// the special value * will make all fileds be output as attributes.
        /// </summary>
        [PDFAttribute("attributes")]
        [PDFDesignable(Ignore = true)]
        public string AsAttributes
        {
            get;
            set;
        }

        #endregion

        #region public PDFDataParameterList Parameters {get;} + HasParameters {get;}

        private PDFDataParameterList _params;

        /// <summary>
        /// Gets the list of parameters in this command
        /// </summary>
        [PDFElement("Parameters")]
        [PDFArray(typeof(DataParameter))]
        public PDFDataParameterList Parameters
        {
            get
            {
                if (null == _params)
                    _params = new PDFDataParameterList();
                return _params;
            }
        }

        public bool HasParameters
        {
            get { return null != _params && _params.Count > 0; }
        }

        #endregion

        #region public PDFDataRelationList Relations {get;} + HasRelations{get;}

        private PDFDataRelationList _rels;

        /// <summary>
        /// Gets the relations in this command
        /// </summary>
        [PDFArray(typeof(DataRelation))]
        [PDFElement("Relations")]
        public PDFDataRelationList Relations
        {
            get
            {
                if (null == _rels)
                    _rels = new PDFDataRelationList();
                return _rels;
            }
        }

        /// <summary>
        /// Returns true if this command has related commands
        /// </summary>
        public bool HasRelations
        {
            get { return null != _rels && _rels.Count > 0; }
        }

        #endregion

        


        //
        // temp data storage methods
        //

        #region public PDFDataSchema DataSchema {get; private set;}

        /// <summary>
        /// Gets the Data Schema associated with the last loaded data.
        /// </summary>
        public DataSchema DataSchema
        {
            get;
            private set;
        }

        #endregion

        #region public DataSet DataSet {get; private set;}

        /// <summary>
        /// Gets the DataSet from the last loaded data
        /// </summary>
        public DataSet DataSet
        {
            get;
            private set;
        }

        #endregion


        //
        // .ctor
        //

        #region public PDFDataTableProviderCommandBase(PDFObjectType type) : base(type)

        public DataTableProviderCommandBase(ObjectType type) : base(type)
        {

        }

        #endregion

        //
        // abstract overrides
        //

        #region protected override System.Xml.XPath.XPathNavigator DoLoadXPathData(PDFXPathDataSourceBase source, PDFDataContext context)

        /// <summary>
        /// Main method that loads all the data (including any related commands) into a dataset and returns an XPathNavigable instance of the data representation
        /// </summary>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override void DoEnsureDataLoaded(XPathDataSourceBase source, DataSet ds, DataContext context)
        {
            
            //We are always top level here so top element namespace should be the same as inner element namespace
            if (!string.IsNullOrEmpty(this.ElementNamespaceUri))
                ds.Namespace = this.ElementNamespaceUri;

            
            this.FillData(ds, source, null, context);

            if (this.HasRelations)
            {
                foreach (DataRelation rel in this.Relations)
                {
                    FillRelatedData(rel, ds, source, context);
                }
            }

            this.DataSet = ds;
            this.DataSchema = DoPopulateDataSchema(ds, context);

        }

        #endregion

        protected override void DoDataBind(DataContext context, bool includeChildren)
        {
            base.DoDataBind(context, includeChildren);
            if (includeChildren)
            {
                this.DataBindChildren(context);
            }

        }

        protected virtual void DataBindChildren(DataContext context)
        {
            if(this.HasParameters)
            {
                foreach (var param in this.Parameters)
                {
                    param.DataBind(context);
                }
            }
            if(this.HasRelations)
            {
                foreach (var rel in this.Relations)
                {
                    rel.DataBind(context);
                }
            }
        }

        //
        // IPDFDataSetProvider implementation
        //

        #region public abstract void FillData(DataSet dataset, PDFXPathDataSourceBase source, IPDFDataSetProviderCommand parent, PDFDataContext context);

        /// <summary>
        /// Abstract base method that inheritors must override to populate the main data in this provider
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="source"></param>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        public abstract void FillData(DataSet dataset, XPathDataSourceBase source, IDataSetProviderCommand parent, DataContext context);

        #endregion

        #region public virtual object GetNullValue(DbType type)

        /// <summary>
        /// Returns the default null value for the specified DbType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>Default implementation returns DBNull.Value for all types, inheritors can override as required.</remarks>
        public virtual object GetNullValue(DbType type)
        {
            return DBNull.Value;
        }

        #endregion

        #region protected virtual string GetDataTableName(DataSet ds)

        /// <summary>
        /// returns the name that should be given to a data table that has been loaded.
        /// </summary>
        /// <param name="index">The index of the table being loaded. The first table will be 1.</param>
        /// <returns></returns>
        public virtual string GetDataTableName(DataSet ds)
        {
            if (!string.IsNullOrEmpty(this.ElementName))
                return this.ElementName;
            if (!string.IsNullOrEmpty(this.ID))
                return this.ID;
            else
            {
                this.ID = "Command" + ds.Tables.Count + 1;
                return this.ID;
            }
        }

        #endregion

        //
        // support methods
        //

        #region protected virtual PDFDataSchema DoPopulateDataSchema(DataSet dataset, PDFDataContext context)

        protected virtual DataSchema DoPopulateDataSchema(DataSet dataset, DataContext context)
        {
            return DataSetSchemaGenerator.CreateSchemaFromSet(dataset, context);
        }

        #endregion

        #region protected virtual void ApplyAttributes(DataSet ds, string attributenames, PDFDataContext context)

        /// <summary>
        /// Sets the column mapping for this commands data table columns to attributes
        /// based on the comma separated list of names (or the catch all *)
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="attributenames"></param>
        /// <param name="context"></param>
        protected virtual void ApplyAttributes(DataSet ds, string attributenames, DataContext context)
        {
            DataTable dt = ds.Tables[this.GetDataTableName(ds)];
            if (attributenames == "*")
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    dc.ColumnMapping = MappingType.Attribute;
                }
            }
            else
            {
                string[] all = attributenames.Split(',');
                foreach (string name in all)
                {
                    DataColumn dc;
                    if (dt.Columns.Contains(name.Trim()))
                    {
                        dc = dt.Columns[name.Trim()];
                        dc.ColumnMapping = MappingType.Attribute;
                    }
                    else
                        context.TraceLog.Add(TraceLevel.Warning, "Data Table provider command", "The data column '" + name + "could not be found, so cannot be set as a mapping type of Attribute");
                }
            }
        }

        #endregion

        #region protected virtual void FillRelatedData(PDFSqlRelation rel, DataSet ds, PDFXPathDataSourceBase source, PDFDataContext context)

        /// <summary>
        /// Fills the data into the dataset from the releated command, and then adds the relation constraint to the data set.
        /// </summary>
        /// <param name="rel"></param>
        /// <param name="ds"></param>
        /// <param name="source"></param>
        /// <param name="context"></param>
        protected virtual void FillRelatedData(DataRelation rel, DataSet ds, XPathDataSourceBase source, DataContext context)
        {
            string cmd = rel.ChildCommand;
            XPathProviderCommandBase provider = source.Commands[cmd];
            if (null == provider)
                throw new NullReferenceException(string.Format(Errors.CommandWithNameCannotBeFound, cmd, source.ID));
            if (!(provider is IDataSetProviderCommand))
                throw new InvalidCastException(string.Format(Errors.CommandForRelatedDataMustMatchType, typeof(SqlProviderCommand), typeof(SqlProviderCommand)));

            IDataSetProviderCommand dsProvider = (IDataSetProviderCommand)provider;
            dsProvider.FillData(ds, source, this, context);

            rel.AddRelation(this, dsProvider, ds, context);

        }

        #endregion



    }

}
