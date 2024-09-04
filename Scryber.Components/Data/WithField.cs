﻿using System;
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
    public abstract class WithField : Panel
    {

        public WithField(ObjectType type)
            : base(type)
        {
        }
    }

    /// <summary>
    /// A with field that has any old content in it
    /// </summary>
    [PDFParsableComponent("ContentField")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_withContentField")]
    public class WithContentField : WithField
    {

        [PDFElement()]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get
            {
                return base.Contents;
            }
        }

        public WithContentField(): this((ObjectType)"WtCf")
        {

        }

        public WithContentField(ObjectType type) 
            : base(type)
        {
        }

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Size.FullWidth = true;
            style.Position.DisplayMode = Drawing.DisplayMode.Block;
            return style;
        }
    }
    
    /// <summary>
    /// Base class of fields that have a specific bound label or content
    /// </summary>
    public abstract class WithBoundField : WithField
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

        [PDFAttribute("label-class", Scryber.Styles.Style.PDFStylesNamespace)]
        [PDFDesignable("Label Class", Category = "Style Classes", Priority = 3, Type = "ClassName")]
        public string LabelClass { get; set; }

        [PDFAttribute("value-class", Scryber.Styles.Style.PDFStylesNamespace)]
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
        private Label _labelItem = null;

        public WithBoundField(ObjectType type): base(type)
        {

        }

        protected virtual void ResetContents()
        {
            this.Contents.Clear();
            this._contentsBuilt = false;
        }

        protected virtual void EnsureContents(ContextBase context)
        {
            if(!this._contentsBuilt)
            {
                this.BuildContents(context);
                this._contentsBuilt = true;
            }
        }

        protected virtual void BuildContents(ContextBase context)
        {
            if (this.LayoutType != FieldLayoutType.ValueOnly)
            { 
                Label label = new Label();
                label.Text = this.FieldLabel;
                if (!string.IsNullOrEmpty(this.LabelPostFix) && !string.IsNullOrEmpty(this.FieldLabel))
                    label.Text += this.LabelPostFix;

                label.StyleClass = this.LabelClass;

                this.Contents.Add(label);

                this._labelItem = label;

                if (this.LayoutType == FieldLayoutType.Above)
                {
                    label.DisplayMode = Drawing.DisplayMode.Block;

                }
                else if (this.LayoutType == FieldLayoutType.NextTo)
                {
                    this.DisplayMode = Drawing.DisplayMode.Block;
                    label.DisplayMode = Drawing.DisplayMode.Block;
                    this.ColumnCount = 2;
                    this.Contents.Add(new ColumnBreak());
                }
                else
                    label.DisplayMode = Drawing.DisplayMode.Inline;
            }

            VisualComponent field = this.DoBuildItemField(context);

            field.StyleClass = this.ValueClass;
            this.Contents.Add(field);
            if (this.LayoutType == FieldLayoutType.Inline)
                field.DisplayMode = Drawing.DisplayMode.Inline;
            else
                field.DisplayMode = Drawing.DisplayMode.Block;

        }

        public virtual void SetDataSourceBindingItem(DataItem item, DataContext context)
        {
            
        }


        /// <summary>
        /// Evaluates the XPath expression against the current data context - raising an exception if there is no current data on the stack
        /// </summary>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected object AssertGetDataItemValue(string path, DataBindEventArgs args)
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
        protected override void OnPreLayout(LayoutContext context)
        {
            this.EnsureContents(context);
            base.OnPreLayout(context);
        }


        public abstract VisualComponent DoBuildItemField(ContextBase context);

        protected bool IsEmptyValue(object value)
        {
            if (null == value || value.ToString() == string.Empty)
                return true;
            else
                return false;
        }
    }

    public class WithFieldCollection : ComponentWrappingList<WithField>
    {

        public WithFieldCollection(ComponentList inner)
            : base(inner)
        {
        }
    }
}
