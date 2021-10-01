using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;
using Scryber.PDF;

namespace Scryber.Components
{
    [PDFParsableComponent("Input")]
    public class FormInputField : Panel
    {
        private TextLiteral _innerContent;

        [PDFAttribute("value")]
        public string Value
        {
            get
            {
                if (null == this._innerContent)
                    return string.Empty;
                else
                    return this._innerContent.Text;
            }
            set {
                if (string.IsNullOrEmpty(value))
                {
                    if (null != this._innerContent)
                    {
                        this.Contents.Remove(this._innerContent);
                        this._innerContent = null;
                    }
                }
                else
                {
                    if (null == this._innerContent)
                    {
                        this._innerContent = new TextLiteral();
                        this.Contents.Add(this._innerContent);
                    }
                    this._innerContent.Text = value;
                }
            }
        }

        [PDFAttribute("default-value")]
        public string DefaultValue { get; set; }

        [PDFAttribute("type")]
        public FormInputFieldType FieldType { get; set; }

        [PDFAttribute("options")]
        public FormFieldOptions Options { get; set; }

        public PDFAcrobatFormFieldWidget Widget { get; private set; }

        public FormInputField() : this(ObjectTypes.FormInputField)
        { }

        protected FormInputField(ObjectType type) : base(type)
        {

        }


        protected Form GetParentForm(PDFContextBase context)
        {
            var parent = this.Parent;
            while (null != parent && !(parent is Form))
            {
                parent = parent.Parent;
            }

            if (null == parent)
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new PDFLayoutException("The input field '" + this.ID + " is not contained within a Form");
                else
                    context.TraceLog.Add(TraceLevel.Error, "Form Fields", "The input field '" + this.ID + " is not contained within a Form");

                return null;
            }
            else
                return (Form)parent;
        }


        protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, Style fullstyle)
        {
            //var form = this.GetParentForm(context);
            PDFAcrobatFormFieldWidget entry = GetFieldEntry(context);
            context.DocumentLayout.RegisterCatalogEntry(context, PDFArtefactTypes.AcrobatForms, entry);
            this.Widget = entry;

            base.DoRegisterArtefacts(context, set, fullstyle);
            
        }

        

        protected virtual PDFAcrobatFormFieldWidget GetFieldEntry(PDFContextBase context)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                if (string.IsNullOrEmpty(this.ID))
                    this.ID = this.GetIncrementID(this.Type);

                this.Name = this.UniqueID;
                
            }
            PDFAcrobatFormFieldWidget entry = new PDFAcrobatFormFieldWidget(this.Name, this.Value, this.DefaultValue, this.FieldType, this.Options);

            return entry;
        }


        protected override IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return new PDF.Layout.LayoutEngineInput(this, parent);
        }

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();

            style.Position.PositionMode = Drawing.PositionMode.Block;
            style.Border.Width = 1;
            style.Border.LineStyle = Drawing.LineType.Solid;
            style.Border.Color = Drawing.PDFColors.Black;
            style.Padding.All = 5;
            style.Size.FullWidth = true;

            if ((this.Options & FormFieldOptions.MultiLine) != FormFieldOptions.MultiLine)
                style.Text.WrapText = Text.WordWrap.NoWrap;
            
            style.Overflow.Action = Drawing.OverflowAction.Clip;
           
            return style;
        }

    }
}
