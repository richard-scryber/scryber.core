using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.Styles;
using System.Collections;

namespace Scryber.Data
{
    [PDFRequiredFramework("0.8.4")]
    [PDFParsableComponent("DataGrid")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_datagrid")]
    public class DataGrid : Scryber.Components.DataTemplateComponent
    {
        //
        // properties
        //

        #region public PDFDataGridColumnCollection Columns {get;}

        private PDFDataGridColumnCollection _cells;

        /// <summary>
        /// Gets the collection of columns in this datagrid
        /// </summary>
        [PDFElement("Columns")]
        [PDFArray(typeof(DataGridColumn))]
        public PDFDataGridColumnCollection Columns
        {
            get
            {
                if (null == _cells)
                    _cells = new PDFDataGridColumnCollection(this);
                return _cells;
            }
        }

        #endregion

        #region protected BindingActionList BindingActions {get;}

        private List<BindingActionList> _bindingActions = new List<BindingActionList>();

        /// <summary>
        /// Gets the list of binding actions that must be executed after the components have been built
        /// </summary>
        internal List<BindingActionList> BindingActions
        {
            get
            {
                return _bindingActions;
            }
        }

        #endregion

        /// <summary>
        /// Holds the grid that has been built 
        /// </summary>
        private TableGrid _built = null;

        //
        // style class properties
        //


        #region public string HeaderCellClass { get; set; }

        [PDFAttribute("header-cell-class", Scryber.Styles.Style.PDFStylesNamespace)]
        [PDFJSConvertor("scryber.studio.design.convertors.styleClass_attr")]
        [PDFDesignable("Header Cell Class", Category = "Style Classes", Priority = 6, Type = "ClassName")]
        public string HeaderCellClass { get; set; }

        #endregion

        #region  public string CellClass { get; set; }

        [PDFAttribute("cell-class", Scryber.Styles.Style.PDFStylesNamespace)]
        [PDFJSConvertor("scryber.studio.design.convertors.styleClass_attr")]
        [PDFDesignable("Cell Class", Category = "Style Classes", Priority = 3, Type = "ClassName")]
        public string CellClass { get; set; }

        #endregion

        #region  public string AlternateCellClass { get; set; }

        [PDFAttribute("alternate-cell-class", Scryber.Styles.Style.PDFStylesNamespace)]
        [PDFJSConvertor("scryber.studio.design.convertors.styleClass_attr")]
        [PDFDesignable("Alternate Cell Class", Category = "Style Classes", Priority = 4, Type = "ClassName")]
        public string AlternateCellClass { get; set; }

        #endregion

        #region public string FooterCellClass { get; set; }

        [PDFAttribute("footer-cell-class", Scryber.Styles.Style.PDFStylesNamespace)]
        [PDFJSConvertor("scryber.studio.design.convertors.styleClass_attr")]
        [PDFDesignable("Footer Cell Class", Category = "Style Classes", Priority = 5, Type = "ClassName")]
        public string FooterCellClass { get; set; }

        #endregion


        //
        // style value properties
        //


        #region public DataAutoBindContent AutoBindSource {get;set;}

        private DataAutoBindContent _autobind = DataAutoBindContent.None;

        /// <summary>
        /// Gets or sets the autobind property that (if set) will make the grid generate columns of each of the available items in the source.
        /// </summary>
        [PDFAttribute("auto-bind")]
        [PDFDesignable("Auto Bind Content", Category = "Data", Priority = 4, Type = "Select")]
        public DataAutoBindContent AutoBindContent
        {
            get { return _autobind; }
            set { _autobind = value; }
        }

        #endregion

        #region public string ExcludedColumns {get;set;}

        private string[] _excludedColumns = null;
        private static readonly char[] _separator = new char[] { ',' };

        [PDFAttribute("exclude")]
        [PDFDesignable("Exclude Columns", Category = "Data", Priority = 5, Type = "String")]
        public string ExcludedColumns
        {
            get
            {
                if (null == _excludedColumns || _excludedColumns.Length == 0)
                    return string.Empty;
                else
                    return string.Join(",", _excludedColumns);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _excludedColumns = null;
                else
                    _excludedColumns = TrimAll(value.Split(_separator, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        private string[] TrimAll(string[] all)
        {
            for(var i = 0; i < all.Length; i++)
            {
                all[i] = all[i].Trim();
            }
            return all;
        }

        #endregion


        #region public IPDFTemplate EmptyTemplate {get;set;}

        private ITemplate _empty;

        /// <summary>
        /// Gets or sets the template to be used if this grid has no columns or rows
        /// </summary>
        [PDFElement("EmptyTemplate")]
        [PDFAttribute("empty-template")]
        [PDFTemplate()]
        [PDFDesignable("Empty Template", Ignore =true)]
        public ITemplate EmptyTemplate
        {
            get { return _empty; }
            set { _empty = value; }
        }

        #endregion

        #region public bool HasAggregations {get; private set;}

        /// <summary>
        /// Returns true if any of the columns have aggregations
        /// </summary>
        public bool HasAggregations
        {
            get;
            private set;
        }

        #endregion

        #region  public bool HasGroupings {get; private set;}

        /// <summary>
        /// Returns true if any of the columns are grouped
        /// </summary>
        public bool HasGroupings
        {
            get;
            private set;
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFDataGrid()

        /// <summary>
        /// 
        /// </summary>
        public DataGrid()
            : this((ObjectType)"DGrd")
        {
        }

        #endregion

        #region protected PDFDataGrid(PDFObjectType type)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        protected DataGrid(ObjectType type)
            : base(type)
        {
        }

        #endregion


        protected override void DoDataBind(PDFDataContext context, bool includeChildren)
        {
            this.BindStyles(context);

            object data = null;
            bool hasdata = false;
            System.Xml.IXmlNamespaceResolver resolver = context.NamespaceResolver;

            RemoveAnyExistingContent();

            IPDFDataSource source;
            if (this.HasAssignedDataSourceComponent(context, out source))
            {
                data = source.Select(this.SelectPath, context);
                if (null == data)
                    context.TraceLog.Add(TraceLevel.Warning, "DataGrid", string.Format("NULL data was returned for the path '{0}' on the PDFForEach component {1} with datasource {2}", this.SelectPath, this.ID, source.ID));
                hasdata = true;
            }
            else if (string.IsNullOrEmpty(this.SelectPath) == false)
            {
                source = context.DataStack.Source;
                data = source.Select(this.SelectPath, context.DataStack.Current, context);
                if (null == data)
                    context.TraceLog.Add(TraceLevel.Warning, "Data For Each", string.Format("NULL data was returned for the path '{0}' on the PDFForEach component {1}", this.SelectPath, this.ID));
                hasdata = true;
            }
            else
            {
                data = context.DataStack.Current;
                source = context.DataStack.Source;
            }

            if (hasdata)
                context.DataStack.Push(data, source);

            

            base.DoDataBind(context, includeChildren);

            if (hasdata)
            {
                context.DataStack.Pop();
            }

            context.NamespaceResolver = resolver;
        }

        /// <summary>
        /// Removes any existing bound contents created by this grid
        /// </summary>
        protected virtual void RemoveAnyExistingContent()
        {
            if (null != _built && _built.Parent != null)
            {
                ((IPDFContainerComponent)_built.Parent).Content.Remove(_built);
            }
        }

        protected virtual void BindStyles(PDFDataContext context)
        {
           
        }

        protected override ITemplate GetTemplateForBinding(PDFDataContext context, int index, int count)
        {
            throw new NotSupportedException("We don't support this on a data grid - it works differently");
        }


        protected override void DoBindDataIntoContainer(IPDFContainerComponent container, int containerposition, PDFDataContext context)
        {
            IPDFDataSource origSource = context.DataStack.Source;
            object origData = context.DataStack.Pop();

            
            object parentData = null;
            if (context.DataStack.HasData)
                parentData = context.DataStack.Current;

            Style applied = this.GetAppliedStyle();
            applied = applied.Flatten();
            
            IEnumerator enumerator = this.GetDataEnumerator(origData);
            if (this.AutoBindContent != DataAutoBindContent.None)
                this.AddAutoBindColumns(context);

            TableGrid grid = new TableGrid();
            container.Content.Add(grid);

            if (this.HasStyle && this.Style.HasValues)
                this.Style.MergeInto(grid.Style, Style.DirectStylePriority);
            
            if (!string.IsNullOrEmpty(this.StyleClass))
                grid.StyleClass = this.StyleClass;

            if (this.HasOutline)
                grid.Outline = this.Outline;

            this.ApplyCellStyles();
            int colindex = 0;
            int rowindex = -1;
            this._bindingActions = new List<BindingActionList>();

            if (this.ShouldBuildHeader())
            {
                TableHeaderRow header = new TableHeaderRow();
                grid.Rows.Add(header);

                BindingActionList headBind = new BindingActionList();
                this.BindingActions.Add(headBind);

                //Add the columns

                foreach (DataGridColumn column in this.Columns)
                {
                    

                    if (column.Visible == false)
                        continue;

                    Component comp = column.DoBuildHeaderCell(grid, header, rowindex, colindex, context);
                    if (null != comp && null != parentData)
                        headBind.Add(new BindingAction(parentData, origSource, comp));
                    colindex++;
                }

                rowindex++;
            }

            while (enumerator.MoveNext())
            {
                colindex = 0;
                TableRow row = new TableRow();
                grid.Rows.Add(row);

                BindingActionList rowBind = new BindingActionList();
                this.BindingActions.Add(rowBind);

                foreach (DataGridColumn column in this.Columns)
                {
                    if (column.Visible == false)
                        continue;

                    Component comp = column.DoBuildItemCell(grid, row, rowindex, colindex, context);
                    if (null != comp)
                        rowBind.Add(new BindingAction(enumerator.Current, origSource, comp));

                    colindex++;
                }

                rowindex++;
            }

            colindex = 0;

            if (this.ShouldBuildFooter())
            {
                TableFooterRow footer = new TableFooterRow();
                grid.Rows.Add(footer);

                BindingActionList footBind = new BindingActionList();
                this.BindingActions.Add(footBind);

                foreach (DataGridColumn column in this.Columns)
                {
                    if (column.Visible == false)
                        continue;

                    Component comp = column.DoBuildFooterCell(grid, footer, rowindex, colindex, context);
                    if (null != comp && null != parentData)
                        footBind.Add(new BindingAction(parentData, origSource, comp));
                    colindex++;
                }
            }

            
            context.DataStack.Push(origData, origSource);

            this.InitAndLoadRoot(grid, context);
            this.BindActionedComponents(this.BindingActions, context);

            bool allhidden = RemoveHiddenColumns(context);

            if (allhidden || grid.Rows.Count == 0)
            {
                grid.Visible = false;
                this.ShowEmptyTemplate(container, containerposition, context);
            }

            this.RemoveHeadersAndFooters(grid, context);

            _built = grid;
        }

        /// <summary>
        /// Pushes any cell styles of the grid onto the columns
        /// </summary>
        private void ApplyCellStyles()
        {
            if(!string.IsNullOrEmpty(this.CellClass))
            {
                foreach (var col in this.Columns)
                {
                    if (!string.IsNullOrEmpty(col.CellClass))
                        col.CellClass = this.CellClass + " " + col.CellClass;
                    else
                        col.CellClass = this.CellClass;
                }
            }

            if (!string.IsNullOrEmpty(this.AlternateCellClass))
            {
                foreach (var col in this.Columns)
                {
                    if (!string.IsNullOrEmpty(col.AlternatingCellClass))
                        col.AlternatingCellClass = this.AlternateCellClass + " " + col.AlternatingCellClass;
                    else
                        col.AlternatingCellClass = this.AlternateCellClass;
                }
            }

            if (!string.IsNullOrEmpty(this.HeaderCellClass))
            {
                foreach (var col in this.Columns)
                {
                    if (!string.IsNullOrEmpty(col.HeaderCellClass))
                        col.HeaderCellClass = this.HeaderCellClass + " " + col.HeaderCellClass;
                    else
                        col.HeaderCellClass = this.HeaderCellClass;
                }
            }

            if (!string.IsNullOrEmpty(this.FooterCellClass))
            {
                foreach (var col in this.Columns)
                {
                    if (!string.IsNullOrEmpty(col.FooterCellClass))
                        col.FooterCellClass = this.FooterCellClass + " " + col.FooterCellClass;
                    else
                        col.FooterCellClass = this.FooterCellClass;
                }
            }

        }

        /// <summary>
        /// Removes each of the columns in the built grid table if all the cells in them are not visible.
        /// Returns  true if ALL of the columns were removed.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool RemoveHiddenColumns(PDFDataContext context)
        {
            bool allhidden = true;
            //reverse order as we can remove items from the collection.
            for (int i = this.Columns.Count - 1; i >= 0; i--)
            {
                DataGridColumn column = this.Columns[i];

                if (column.RemoveIfColumnHidden(context))
                {
                }
                else
                    allhidden = false;
            }
            return allhidden;
        }

        private void RemoveHeadersAndFooters(TableGrid grid, PDFDataContext context)
        {
            bool hasHeaders = false;
            bool hasFooters = false;

            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].HasHeader)
                    hasHeaders = true;
                if (this.Columns[i].HasFooter)
                    hasFooters = true;

                if (hasHeaders && hasFooters)
                    break;
            }

            if (hasHeaders == false && grid.Rows.Count > 0)
                grid.Rows.RemoveAt(0);

            
        }

        private bool ShouldBuildFooter()
        {
            return false;
            //foreach (PDFDataGridColumn col in this.Columns)
            //{
            //    if (col.Visible)
            //        return true;
            //}
            //return false;
        }

        private bool ShouldBuildHeader()
        {
            foreach (DataGridColumn col in this.Columns)
            {
                if (col.Visible)
                    return true;
            }
            return false;
        }

        private bool ShowEmptyTemplate(IPDFContainerComponent container, int index, PDFDataContext context)
        {
            if (null == this.EmptyTemplate)
                return false;
            else
            {
                this.InstantiateAndAddWithTemplate(this.EmptyTemplate, 0, index, container, context);
                return true;
            }
        }

        private IEnumerator GetDataEnumerator(object data)
        {
            if (data is IEnumerable)
            {
                return ((IEnumerable)data).GetEnumerator();
            }
            else
            {
                List<object> all = new List<object>(1);
                all.Add(data);
                return all.GetEnumerator();
            }
        }

        protected virtual void AddAutoBindColumns(PDFDataContext context)
        {
            if (string.IsNullOrEmpty(this.DataSourceID))
                throw new InvalidOperationException("Can only auto bind the schema when the DataGrid has an explicit DataSourceID and the referencing source supports Schema derriving");

            IPDFDataSource found = base.FindDocumentComponentById(this.DataSourceID) as IPDFDataSource;
            if (null == found || found.SupportsDataSchema == false)
                throw new InvalidOperationException("Can only auto bind the schema when the DataGrid has an explicit DataSourceID and the referencing source supports Schema derriving");

            PDFDataSchema schema = found.GetDataSchema(this.SelectPath, context);

            if (null == schema || schema.Items == null || schema.Items.Count == 0)
                context.TraceLog.Add(TraceLevel.Warning, "PDFDataGrid", string.Format("Cannot autobind the columns as no schema items were returned for the path '{0}'", this.SelectPath));
            else
            {
                foreach (PDFDataItem item in schema.Items)
                {
                    if (ShouldIncludeAutoBoundItem(item))
                    {
                        DataGridColumn col = GetDataColumnForType(item.DataType);
                        if (null != col)
                        {
                            col.HeaderText = string.IsNullOrEmpty(item.Title) ? item.Name : item.Title;
                            col.SetDataSourceBindingItem(item, context);
                            if (context.TraceLog.ShouldLog(TraceLevel.Debug))
                                context.TraceLog.Add(TraceLevel.Debug, "PDFDataGrid", string.Format("The data column was automatically created for the schdema item '{0}' in data grid '{1}'", item, this));
                            this.Columns.Add(col);
                           
                        }
                    }
                }
            }
        }

        private bool ShouldIncludeAutoBoundItem(PDFDataItem item)
        {
            bool include = false;
            switch (this.AutoBindContent)
            {
                case DataAutoBindContent.None:
                    include = false;
                    break;
                //case DataAutoBindContent.Elements:
                //    include = (item.NodeType == System.Xml.XmlNodeType.Element);
                //    break;
                //case DataAutoBindContent.Attributes:
                //    include = (item.NodeType == System.Xml.XmlNodeType.Attribute);
                //    break;
                case DataAutoBindContent.All:
                    include = (item.NodeType == System.Xml.XmlNodeType.Element || item.NodeType == System.Xml.XmlNodeType.Attribute);
                    if(null != _excludedColumns && _excludedColumns.Length > 0 && Array.IndexOf(_excludedColumns, item.Name) > -1)
                            include = false;
                    
                    break;
                default:
                    break;
            }
            return include;
        }

        private DataGridColumn GetDataColumnForType(DataType type)
        {
            DataGridColumn col = null;
            switch (type)
            {
                case DataType.Boolean:
                case DataType.String:
                case DataType.Guid:
                case DataType.Text:
                case DataType.User:
                case DataType.UserGroup:
                case DataType.Lookup:
                case DataType.Unknown:
                    col = new DataGridTextColumn();
                    break;
                case DataType.Html:
                    col = new DataGridHtmlColumn();
                    break;
                case DataType.Integer:
                case DataType.Double:
                    col = new DataGridNumberColumn();
                    break;
                case DataType.DateTime:
                    col = new DataGridDateColumn();
                    break;
                case DataType.Image:
                    col = new DataGridImageDataColumn();
                    break;
                case DataType.Url:
                    col = new DataGridLinkColumn();
                    break;
                default:
                    break;
            }
            return col;
            
        }

        private void InitAndLoadRoot(Component built, PDFContextBase context)
        {
            if(context.TraceLog.ShouldLog(TraceLevel.Debug))
                context.TraceLog.Add(TraceLevel.Debug, "DataGrid", "Initializing and loading root component before binding");

            TraceLog log = context.TraceLog;
            PDFInitContext init = new PDFInitContext(context.Items, log, context.PerformanceMonitor, this.Document);
            PDFLoadContext load = new PDFLoadContext(context.Items, log, context.PerformanceMonitor, this.Document);

            built.Init(init);
            built.Load(load);
        }

        #region private void BindActionedComponents(BindingActionList actions)

        /// <summary>
        /// Takes all the binding actions (data and component pairs) 
        /// and then calls DataBind on each of them in turn using the current data context stack
        /// and the data in the binding action.
        /// </summary>
        /// <param name="actions"></param>
        private void BindActionedComponents(List<BindingActionList> actions, PDFDataContext context)
        {
            if(context.TraceLog.ShouldLog(TraceLevel.Debug))
                context.TraceLog.Begin(TraceLevel.Debug, "DataGrid", "Starting to bind " + actions.Count.ToString() + "components with their respective data items");
            int lastindex = context.CurrentIndex;
            int currIndex = 0;
            foreach (BindingActionList list in actions)
            {

                context.CurrentIndex = currIndex;

                foreach (BindingAction action in list)
                {
                    
                    context.DataStack.Push(action.Data, action.Source);
                    action.Component.DataBind(context);

                    this.OnItemDataBound(context, (IComponent)action.Component);

                    context.DataStack.Pop();
                    
                }

                currIndex++;
            }

            if (context.TraceLog.ShouldLog(TraceLevel.Debug))
                context.TraceLog.End(TraceLevel.Debug, "DataGrid", "Completed bound " + actions.Count.ToString() + "components with their respective data items");

            context.CurrentIndex = lastindex;
        }

        #endregion
    }
}
