using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF;
using Scryber.PDF.Layout;

namespace Scryber.Components
{
    [PDFParsableComponent("Input")]
    public class FormInputField : Panel, IPDFFormField
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
        
        [PDFAttribute("maxlength")]
        public int MaxLength
        {
            get;
            set;
        }

        public FormInputFieldType FieldType { get; set; }

        [PDFAttribute("options")]
        public FormFieldOptions Options { get; set; }

        /// <summary>
        /// Distinguishes the Button-family field types (checkbox/radio/pushbutton) that all
        /// share FieldType = Button (PDF's /FT /Btn covers all three) - set alongside FieldType
        /// whenever the "type" attribute maps to one of them.
        /// </summary>
        public FormButtonFieldType ButtonType { get; set; }

        /// <summary>
        /// Whether a checkbox/radio field is checked. Ignored for other field types.
        /// </summary>
        [PDFAttribute("checked")]
        public HtmlBoolean Checked { get; set; }

        /// <summary>
        /// HTML's "required" boolean attribute - maps onto the PDF /Ff Required bit (readers like
        /// Acrobat/Chrome highlight required fields, e.g. in red, using this flag rather than anything
        /// visual we bake into the appearance stream).
        /// </summary>
        [PDFAttribute("required")]
        public HtmlBoolean Required { get; set; }

        /// <summary>
        /// None by default - set to Submit/Reset for a submit/reset-behaviour button or input,
        /// which builds and attaches a PDFSubmitFormAction/PDFResetFormAction to this field's
        /// widget using the parent Form's Action/Method.
        /// </summary>
        public FormSubmitBehavior SubmitBehavior { get; set; }

        /// <summary>
        /// Binds the raw "type" attribute string. Accepts both Scryber's native FormInputFieldType
        /// member names (e.g. "Signature") and real HTML input type keywords (e.g. "password", "submit"),
        /// which don't map 1:1 onto FormInputFieldType and so can't be bound directly via enum parsing.
        /// </summary>
        [PDFAttribute("type")]
        public string FieldTypeName
        {
            get { return this.FieldType.ToString(); }
            set { this.SetFieldTypeFromAttributeValue(value); }
        }

        private void SetFieldTypeFromAttributeValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            switch (value.ToLowerInvariant())
            {
                case "password":
                    this.FieldType = FormInputFieldType.Text;
                    this.Options |= FormFieldOptions.Password;
                    break;
                case "file":
                    this.FieldType = FormInputFieldType.Text;
                    this.Options |= FormFieldOptions.File;
                    break;
                case "submit":
                    this.FieldType = FormInputFieldType.Button;
                    this.SubmitBehavior = FormSubmitBehavior.Submit;
                    break;
                case "reset":
                    this.FieldType = FormInputFieldType.Button;
                    this.SubmitBehavior = FormSubmitBehavior.Reset;
                    break;
                case "button":
                    //An explicit type="button" has no default click behaviour (unlike a bare
                    //<button>, which defaults to submit) - clears any default set by a subclass
                    //constructor (e.g. HTMLButton) before this attribute was parsed.
                    this.FieldType = FormInputFieldType.Button;
                    this.SubmitBehavior = FormSubmitBehavior.None;
                    break;
                case "checkbox":
                    this.FieldType = FormInputFieldType.Button;
                    this.ButtonType = FormButtonFieldType.CheckBox;
                    break;
                case "radio":
                    this.FieldType = FormInputFieldType.Button;
                    this.ButtonType = FormButtonFieldType.Radio;
                    this.Options |= FormFieldOptions.Radio;
                    break;
                default:
                    if (Enum.TryParse<FormInputFieldType>(value, true, out var parsed))
                        this.FieldType = parsed;
                    break;
            }
        }

        public PDFAcrobatFormFieldWidget Widget { get; private set; }

        /// <summary>
        /// Some HTML field elements (textarea, button) carry their initial Value as literal
        /// inner text rather than a value= attribute. The generic content parser attaches that
        /// text as a plain child (subclasses must re-declare Contents with [PDFElement] for this
        /// to happen at all, since FormInputField itself doesn't - &lt;input&gt; has no inner
        /// text in HTML), so once data binding completes it's harvested here into Value - the
        /// single source of truth this class renders and exports from - rather than being left
        /// to render again as a second, untracked child.
        /// </summary>
        protected void HarvestInnerTextAsValueIfEmpty()
        {
            if (string.IsNullOrEmpty(this.Value))
            {
                var text = this.Contents.OfType<TextLiteral>().FirstOrDefault();
                if (null != text)
                {
                    var content = text.Text;
                    this.Contents.Remove(text);
                    this.Value = string.IsNullOrEmpty(content) ? content : content.Trim();
                }
            }
        }

        public FormInputField() : this(ObjectTypes.FormInputField)
        { }

        protected FormInputField(ObjectType type) : base(type)
        {

        }


        /// <summary>
        /// Returns the nearest ancestor Form, or null if this field is not contained within one -
        /// fields with no Form ancestor are valid and fall back to direct root registration.
        /// </summary>
        protected Form GetParentForm(ContextBase context)
        {
            var parent = this.Parent;
            while (null != parent && !(parent is Form))
            {
                parent = parent.Parent;
            }
            return parent as Form;
        }


        protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, Style fullstyle)
        {
            PDFAcrobatFormFieldWidget entry = GetFieldEntry(context);
            this.Widget = entry;

            var form = this.GetParentForm(context);
            if (null != form)
                form.RegisterField(this, context);
            else
                context.DocumentLayout.RegisterCatalogEntry(context, PDFArtefactTypes.AcrobatForms, entry);

            base.DoRegisterArtefacts(context, set, fullstyle);

        }

        private PDFAcrobatFormFieldWidget _fieldEntry;

        /// <summary>
        /// Builds (or returns the already-built) widget entry for this field. Cached because this can be
        /// invoked twice in one render pass - directly here, and again via IPDFFormField.GetFieldEntry when
        /// a parent Form registers this field into its own /Kids - and both call sites must share one instance
        /// so the appearance streams set on this.Widget during layout end up on the object that's actually output.
        /// </summary>
        protected virtual PDFAcrobatFormFieldWidget GetFieldEntry(ContextBase context)
        {
            if (null != this._fieldEntry)
                return this._fieldEntry;

            if (string.IsNullOrEmpty(this.Name))
            {
                if (string.IsNullOrEmpty(this.ID))
                    this.ID = this.GetIncrementID(this.Type);

                this.Name = this.UniqueID;

            }

            if (this.Required)
                this.Options |= FormFieldOptions.Required;

            if (this.ButtonType == FormButtonFieldType.CheckBox || this.ButtonType == FormButtonFieldType.Radio)
            {
                var checkWidget = new PDFAcrobatFormCheckWidget(this.Name, this.Value, this.DefaultValue, this.FieldType, this.Options);
                checkWidget.IsChecked = this.Checked;
                checkWidget.ButtonType = this.ButtonType;
                if (!string.IsNullOrEmpty(this.Value))
                    checkWidget.OnStateName = this.Value;
                this._fieldEntry = checkWidget;
            }
            else
            {
                this._fieldEntry = new PDFAcrobatFormFieldWidget(this.Name, this.Value, this.DefaultValue, this.FieldType, this.Options);
                this._fieldEntry.MaxLength = this.MaxLength;
            }

            this._fieldEntry.Action = this.BuildSubmitAction(context);

            return this._fieldEntry;
        }

        /// <summary>
        /// Builds the submit/reset action for this field, if it has submit behaviour. A reset
        /// action needs no target, but a submit action does nothing without a Form ancestor
        /// that has an action= URL to submit to - it's not an error, just nothing to attach.
        /// </summary>
        protected virtual PDF.PDFAction BuildSubmitAction(ContextBase context)
        {
            if (this.SubmitBehavior == FormSubmitBehavior.Reset)
                return new PDF.PDFResetFormAction(this);

            if (this.SubmitBehavior == FormSubmitBehavior.Submit)
            {
                var form = this.GetParentForm(context);
                if (null != form && !string.IsNullOrEmpty(form.Action))
                    return new PDF.PDFSubmitFormAction(this, form.Action, form.Method);
            }

            return null;
        }

        object IPDFFormField.GetFieldEntry(ContextBase context)
        {
            return this.GetFieldEntry(context);
        }


        protected override IPDFLayoutEngine CreateLayoutEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style style)
        {
            return new PDF.Layout.LayoutEngineInput(this, parent);
        }

        protected override Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Position.PositionMode = Drawing.PositionMode.Static;
            style.Position.DisplayMode = DisplayMode.InlineBlock;
            style.Border.Width = 1;
            style.Border.LineStyle = Drawing.LineType.Solid;
            style.Border.Color = Drawing.StandardColors.Black;
            style.Background.Color = StandardColors.White;
            style.Position.VAlign = VerticalAlignment.Bottom;
            style.Fill.Color = StandardColors.Black;
            
            style.Padding.All = 5;
            style.Margins.All = 5;
            if ((this.Options & FormFieldOptions.MultiLine) != FormFieldOptions.MultiLine)
                style.Text.WrapText = Text.WordWrap.NoWrap;
            
            style.Overflow.Action = Drawing.OverflowAction.Clip;
           
            return style;
        }

    }
}
