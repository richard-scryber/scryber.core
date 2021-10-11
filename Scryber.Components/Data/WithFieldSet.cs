using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber.Data
{
    [PDFParsableComponent("FieldSet")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withFieldSet")]
    public class WithFieldSet : With
    {

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

        #region public string FieldClass { get; set; }

        /// <summary>
        /// Gets the standard class for the field as a whole
        /// </summary>
        [PDFAttribute("field-class", Scryber.Styles.Style.PDFStylesNamespace)]
        [PDFDesignable("Field Class", Category = "Style Classes", Priority = 2, Type = "ClassName")]
        public string FieldClass { get; set; }

        #endregion

        #region public string LabelClass { get; set; }

        /// <summary>
        /// Gets the standard class for all labels
        /// </summary>
        [PDFAttribute("label-class", Scryber.Styles.Style.PDFStylesNamespace)]
        [PDFDesignable("Label Class", Category = "Style Classes", Priority = 3, Type = "ClassName")]
        public string LabelClass { get; set; }

        #endregion

        #region public string ValueClass { get; set; }

        /// <summary>
        /// Gets or sets the class name for the value section of the field
        /// </summary>
        [PDFAttribute("value-class", Scryber.Styles.Style.PDFStylesNamespace)]
        [PDFDesignable("Value Class", Category = "Style Classes", Priority = 3, Type = "ClassName")]
        public string ValueClass { get; set; }

        #endregion

        #region public FieldLayoutType AutoLayoutType { get; set; }

        /// <summary>
        /// Gets or sets the layout type for any autobound fields
        /// </summary>
        [PDFAttribute("layout-type")]
        [PDFDesignable("Layout Type", Category = "General", Priority = 4, Type = "Select")]
        public FieldLayoutType AutoLayoutType { get; set; }

        #endregion

        #region public bool HideEmptyFields { get; set; }

        /// <summary>
        /// Gets or sets the flag that identifies if any fields that are empty should be hidden
        /// </summary>
        [PDFAttribute("hide-empty")]
        [PDFDesignable("Hide Empty Fields", Category = "General", Priority = 4, Type = "Boolean")]
        public bool HideEmptyFields { get; set; }

        #endregion


        #region public string LabelPostFix { get; set; }

        [PDFAttribute("label-postfix")]
        [PDFDesignable("Label Postfix", Category = "General", Priority = 4, Type = "String")]
        public string LabelPostFix { get; set; }

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
                    _excludedColumns = value.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        #endregion

        #region public PDFWithFieldCollection Fields {get;}

        private WithFieldCollection _fields;

        /// <summary>
        /// Gets the collection of fields in this set
        /// </summary>
        [PDFElement("Fields")]
        [PDFArray(typeof(WithField))]
        public WithFieldCollection Fields
        {
            get
            {
                if (null == this._fields)
                    this._fields = new WithFieldCollection(this.InnerContent);
                return _fields;
            }
        }

        #endregion

        #region protected BindingActionList BindingActions {get;}

        private BindingActionList _bindingActions = new BindingActionList();

        /// <summary>
        /// Gets the list of binding actions that must be executed after the components have been built
        /// </summary>
        internal BindingActionList BindingActions
        {
            get
            {
                return _bindingActions;
            }
        }

        #endregion


        

        public WithFieldSet() : this((ObjectType)"WtFs")
        {
        }

        protected WithFieldSet(ObjectType type) 
            : base(type)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="containerposition"></param>
        /// <param name="template"></param>
        /// <param name="context"></param>
        protected override void DoBindDataIntoContainer(IContainerComponent container, int containerposition, DataContext context)
        {
            int prevcount = context.CurrentIndex;
            DataStack stack = context.DataStack;

            object origData = stack.HasData ? stack.Current : null;
            IDataSource source = stack.HasData ? stack.Source : null;

            foreach(WithField fld in this.Fields)
            {
                this.BindingActions.Add(origData, source, fld);
            }

            if (this.AutoBindContent != DataAutoBindContent.None)
                this.AddAutoBindFields(origData, source, context);

            this.BindActionedComponents(this.BindingActions, context);
        }



        protected virtual void AddAutoBindFields(object data, IDataSource source, DataContext context)
        {
            if (string.IsNullOrEmpty(this.DataSourceID))
                throw new InvalidOperationException("Can only auto bind the schema when the With has an explicit DataSourceID and the referencing source supports Schema derriving");

            IDataSource found = base.FindDocumentComponentById(this.DataSourceID) as IDataSource;
            if (null == found || found.SupportsDataSchema == false)
                throw new InvalidOperationException("Can only auto bind the schema when the With has an explicit DataSourceID and the referencing source supports Schema derriving");

            DataSchema schema = found.GetDataSchema(this.SelectPath, context);

            if (null == schema || schema.Items == null || schema.Items.Count == 0)
                context.TraceLog.Add(TraceLevel.Warning, "PDFWithFieldSet", string.Format("Cannot autobind the columns as no schema items were returned for the path '{0}'", this.SelectPath));
            else
            {
                if (context.TraceLog.ShouldLog(TraceLevel.Debug))
                    context.TraceLog.Add(TraceLevel.Debug, "DataGrid", "Initializing and loading root component before binding");

                var log = context.TraceLog;
                InitContext init = new InitContext(context.Items, log, context.PerformanceMonitor, this.Document);
                LoadContext load = new LoadContext(context.Items, log, context.PerformanceMonitor, this.Document);


                foreach (DataItem item in schema.Items)
                {
                    if (ShouldIncludeAutoBoundItem(item))
                    {
                        WithBoundField field = GetFieldForType(item.DataType);
                        if (null != field)
                        {
                            field.StyleClass = this.FieldClass;

                            field.FieldLabel = string.IsNullOrEmpty(item.Title) ? item.Name : item.Title;
                            field.LabelClass = this.LabelClass;
                            field.ValueClass = this.ValueClass;
                            field.LabelPostFix = this.LabelPostFix;
                            field.LayoutType = this.AutoLayoutType;
                            field.DataType = item.DataType;
                            field.HideIfEmpty = this.HideEmptyFields;


                            field.SetDataSourceBindingItem(item, context);
                            if (context.TraceLog.ShouldLog(TraceLevel.Debug))
                                context.TraceLog.Add(TraceLevel.Debug, "PDFWithFieldSet", string.Format("The data field was automatically created for the schdema item '{0}' in set '{1}'", item, this));

                            this.Fields.Add(field);
                            field.Init(init);
                            field.Load(load);

                            this.BindingActions.Add(new BindingAction(data, source, field));
                        }
                    }
                }

                //added all the items
            }
        }

        private void BindActionedComponents(BindingActionList actions, DataContext context)
        {
            if (context.TraceLog.ShouldLog(TraceLevel.Debug))
                context.TraceLog.Begin(TraceLevel.Debug, "PDFWithFieldSet", "Starting to bind " + actions.Count.ToString() + "components with their respective data items");

            var lastIndex = context.CurrentIndex;
            var index = 0;

            foreach(BindingAction action in actions)
            {
                context.CurrentIndex = index;
                context.DataStack.Push(action.Data, action.Source);
                action.Component.DataBind(context);

                context.DataStack.Pop();
                index++;
            }

            if (context.TraceLog.ShouldLog(TraceLevel.Debug))
                context.TraceLog.End(TraceLevel.Debug, "PDFWithFieldSet", "Completed bound " + actions.Count.ToString() + "components with their respective data items");

        }

        protected bool ShouldIncludeAutoBoundItem(DataItem item)
        {
            switch (this.AutoBindContent)
            {
                case DataAutoBindContent.None:
                    return false;
                    
                //case DataAutoBindContent.Elements:
                //    return item.NodeType == System.Xml.XmlNodeType.Element;
                    
                //case DataAutoBindContent.Attributes:
                //    return item.NodeType == System.Xml.XmlNodeType.Attribute;

                case DataAutoBindContent.All:
                    if(null != this._excludedColumns)
                    {
                        if (Array.IndexOf(this._excludedColumns, item.Name) > -1)
                            return false;
                    }
                    return item.NodeType == System.Xml.XmlNodeType.Attribute || item.NodeType == System.Xml.XmlNodeType.Element;

                default:
                    return false;
            }
        }

        protected virtual WithBoundField GetFieldForType(Scryber.DataType type)
        {
            WithBoundField fld;
            switch (type)
            {
                case DataType.Integer:
                case DataType.Double:
                    fld = new WithNumberField();
                    break;

                case DataType.DateTime:
                    fld = new WithDateField();
                    break;

                case DataType.Boolean:
                    fld = new WithBooleanField();
                    break;

                case DataType.String:
                case DataType.Text:
                case DataType.Guid:
                case DataType.User:
                case DataType.UserGroup:
                case DataType.Lookup:
                case DataType.Unknown:
                    fld = new WithTextField();
                    break;

                case DataType.Html:
                    fld = new WithHtmlField();
                    break;

                case DataType.Image:
                    fld = new WithImageField();
                    break;

                case DataType.Array:
                case DataType.BinaryFile:
                    fld = null;
                    break;

                case DataType.Url:
                    fld = new WithUrlField();
                    break;

                default:
                    throw new NotSupportedException("The field type for the With component was not known");

            }
            return fld;
        }
    }
}
