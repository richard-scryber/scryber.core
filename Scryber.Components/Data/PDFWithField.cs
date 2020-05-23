using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Data
{
    /// <summary>
    /// Base class of the fields in a with 
    /// </summary>
    public abstract class PDFWithField : PDFPanel
    {

        public PDFWithField(PDFObjectType type)
            : base(type)
        {
        }
    }

    /// <summary>
    /// A with field that has any old content in it
    /// </summary>
    [PDFParsableComponent("ContentField")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withContentField")]
    public class PDFWithContentField : PDFWithField
    {

        [PDFElement()]
        [PDFArray(typeof(PDFComponent))]
        public override PDFComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        public PDFWithContentField(): this((PDFObjectType)"WtCf")
        {

        }

        public PDFWithContentField(PDFObjectType type) 
            : base(type)
        {
        }

        protected override PDFStyle GetBaseStyle()
        {
            PDFStyle style = base.GetBaseStyle();
            style.Size.FullWidth = true;
            style.Position.PositionMode = Drawing.PositionMode.Block;
            return style;
        }
    }
    
    /// <summary>
    /// Base class of fields that have a specific bound label or content
    /// </summary>
    public abstract class PDFWithBoundField : PDFWithField
    {

        private FieldLayoutType _layoutType = FieldLayoutType.NextTo;

        [PDFAttribute("layout-type")]
        [PDFDesignable("Field Layout Type", Category = "General", Priority = 4, Type = "String")]
        public FieldLayoutType LayoutType
        {
            get { return _layoutType; }
            set
            {
                if (value != _layoutType)
                {
                    _layoutType = value;
                    this.ResetContents();
                }
            }
        }

        
        [PDFAttribute("label")]
        [PDFDesignable("Label Text", Category = "General", Priority = 5, Type = "String")]
        public string FieldLabel { get; set; }

        [PDFAttribute("label-class", Scryber.Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Label Class", Category = "Style Classes", Priority = 3, Type = "ClassName")]
        public string LabelClass { get; set; }

        [PDFAttribute("value-class", Scryber.Styles.PDFStyle.PDFStylesNamespace)]
        [PDFDesignable("Value Class", Category = "Style Classes", Priority = 4, Type = "ClassName")]
        public string ValueClass { get; set; }

        #region public string LabelPostFix { get; set; }

        [PDFAttribute("label-postfix")]
        [PDFDesignable("Label Postfix", Category = "General", Priority = 6, Type = "String")]
        public string LabelPostFix { get; set; }

        #endregion

        #region public bool HideIfEmpty { get; set; }

        [PDFAttribute("hide-empty")]
        [PDFDesignable("Hide if Empty", Category = "General", Priority = 6, Type = "Boolean")]
        public bool HideIfEmpty { get; set; }

        #endregion

        #region public DataType DataType { get; set; }

        /// <summary>
        /// Gets or sets the data type for the field
        /// </summary>
        [PDFAttribute("data-type")]
        [PDFDesignable("Data Type", Ignore = true)]
        public DataType DataType { get; set; }

        #endregion


        private bool _contentsBuilt = false;
        private PDFLabel _labelItem = null;

        public PDFWithBoundField(PDFObjectType type): base(type)
        {

        }

        protected virtual void ResetContents()
        {
            this.Contents.Clear();
            this._contentsBuilt = false;
        }

        protected virtual void EnsureContents(PDFContextBase context)
        {
            if(!this._contentsBuilt)
            {
                this.BuildContents(context);
                this._contentsBuilt = true;
            }
        }

        protected virtual void BuildContents(PDFContextBase context)
        {
            if (this.LayoutType != FieldLayoutType.ValueOnly)
            { 
                PDFLabel label = new PDFLabel();
                label.Text = this.FieldLabel;
                if (!string.IsNullOrEmpty(this.LabelPostFix) && !string.IsNullOrEmpty(this.FieldLabel))
                    label.Text += this.LabelPostFix;

                label.StyleClass = this.LabelClass;

                this.Contents.Add(label);

                this._labelItem = label;

                if (this.LayoutType == FieldLayoutType.Above)
                {
                    label.PositionMode = Drawing.PositionMode.Block;

                }
                else if (this.LayoutType == FieldLayoutType.NextTo)
                {
                    this.PositionMode = Drawing.PositionMode.Block;
                    label.PositionMode = Drawing.PositionMode.Block;
                    this.ColumnCount = 2;
                    this.Contents.Add(new PDFColumnBreak());
                }
                else
                    label.PositionMode = Drawing.PositionMode.Inline;
            }

            PDFVisualComponent field = this.DoBuildItemField(context);

            field.StyleClass = this.ValueClass;
            this.Contents.Add(field);
            if (this.LayoutType == FieldLayoutType.Inline)
                field.PositionMode = Drawing.PositionMode.Inline;
            else
                field.PositionMode = Drawing.PositionMode.Block;

        }

        public virtual void SetDataSourceBindingItem(PDFDataItem item, PDFDataContext context)
        {
            
        }


        /// <summary>
        /// Evaluates the XPath expression against the current data context - raising an exception if there is no current data on the stack
        /// </summary>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected object AssertGetDataItemValue(string path, PDFDataBindEventArgs args)
        {
            if (args.Context.DataStack.HasData == false)
                throw new PDFDataException(Errors.CouldNotBindDataComponent);

            object value = null;
            try
            {

                value = args.Context.DataStack.Source.Evaluate(path, args.Context.DataStack.Current, args.Context);
            }
            catch (Exception ex)
            {
                throw new PDFDataException(Errors.CouldNotBindDataComponent, ex);
            }
            if (IsEmptyValue(value) && this.HideIfEmpty)
                this.Visible = false;

            return value;
        }

        /// <summary>
        /// Here we make sure that the contents of the field ase created.
        /// </summary>
        /// <param name="context"></param>
        protected override void OnPreLayout(PDFLayoutContext context)
        {
            this.EnsureContents(context);
            base.OnPreLayout(context);
        }


        public abstract PDFVisualComponent DoBuildItemField(PDFContextBase context);

        protected bool IsEmptyValue(object value)
        {
            if (null == value || value.ToString() == string.Empty)
                return true;
            else
                return false;
        }
    }

    public class PDFWithFieldCollection : PDFComponentWrappingList<PDFWithField>
    {

        public PDFWithFieldCollection(PDFComponentList inner)
            : base(inner)
        {
        }
    }
}
