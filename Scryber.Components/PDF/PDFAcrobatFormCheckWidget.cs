using System;
using System.Collections.Generic;
using System.Linq;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.PDF.Graphics;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.PDF
{
    /// <summary>
    /// A checkbox or radio button widget. Unlike other field types (a single flat /AP stream),
    /// PDF checkboxes/radios need a nested appearance sub-dictionary keyed by state name -
    /// /AP /N << /Off ref /&lt;on-state-name&gt; ref >> - plus /AS naming the currently active one.
    /// The two appearance streams are small, hand-drawn shapes (a border box, optionally with an
    /// inner mark) rather than a full layout-engine pass, since the normal Normal/Over/Down
    /// xObject built by LayoutEngineInput is only used here for its geometry (size/location).
    /// </summary>
    public class PDFAcrobatFormCheckWidget : PDFAcrobatFormFieldWidget
    {
        protected Drawing.Point _location;
        protected Drawing.Size _size;
        protected Layout.PDFLayoutPage _page;
        protected Styles.Style _style;

        
        public bool IsChecked { get; set; }

        /// <summary>
        /// The name of the "on" appearance state, and the /V and /AS value when checked.
        /// Defaults to "on" (HTML's own default export value for an un-valued checkbox/radio) when not set.
        /// </summary>
        public string OnStateName { get; set; }

        public FormButtonFieldType ButtonType { get; set; }

        /// <summary>
        /// The radio group this widget belongs to, set by PDFAcrobatFormEntry.RegisterField at
        /// registration time (always available, well before any rendering) - used to trigger the
        /// group's own indirect object opening/closing from whichever kid happens to render
        /// first, so /Parent is resolvable on that kid's very own first (and only, per
        /// PDFAnnotationEntry's caching) render. Null for a plain checkbox.
        /// </summary>
        internal PDFAcrobatRadioGroupEntry Group { get; set; }

        public PDFAcrobatFormCheckWidget(string name, string value, string defaultValue, FormInputFieldType type, FormFieldOptions options)
            : base(name, value, defaultValue, type, options)
        {
        }

        protected override IEnumerable<PDFObjectRef> DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            var xObjectOn = this._states.ContainsKey(FormFieldAppearanceState.On) ? this._states[FormFieldAppearanceState.On] : null;
            var xObjectOff = this._states.ContainsKey(FormFieldAppearanceState.Off) ? this._states[FormFieldAppearanceState.Off] : null;
            
            
            
            if (null == xObjectOn || null == xObjectOff)
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new InvalidOperationException(
                        "The check-box does not have both On and Off Appearances. Cannot render");
                else
                {
                    context.TraceLog.Add(TraceLevel.Error, "Checkbox", "Both On and Off Appearances must be defined. The stated dictionary did not contain both.");
                }
                return null;
            }
            
            var parentRenderBounds = Rect.Empty;
            
            ComponentArrangement arrangement = null;
            var owner = xObjectOn.Owner as Component;
            while (null != owner)
            {
                arrangement = owner.GetFirstArrangement();
                if (null != arrangement)
                {
                    _style = arrangement.FullStyle;
                    break;
                }

                owner = owner.Parent;
            }
            

            string onName = string.IsNullOrEmpty(this.OnStateName) ?  "On" :  this.OnStateName;
            string currentState = this.IsChecked ? onName : "Off";

            this._location = context.Offset;
            var bounds = Rect.Empty;
            Rect? clipRect = null;
            
            //render the On state and record sizes.
            
            var prevXclude = xObjectOn.ChildContainer.ExcludeFromOutput;
            
            xObjectOn.ChildContainer.ExcludeFromOutput = false;
            var onRef = xObjectOn.OutputToPDF(context, writer);
            xObjectOn.ChildContainer.ExcludeFromOutput = prevXclude;

            if (null != onRef)
            {
                var sz = new Size(xObjectOn.Width, xObjectOn.Height);
                if (_size == Size.Empty)
                    _size = sz;
                else
                {
                    if(_size.Width < sz.Width)
                        _size.Width = sz.Width;
                    if(_size.Height < sz.Height)
                        _size.Height = sz.Height;
                }
                this._location = new Point(xObjectOn.Location.X, xObjectOn.Location.Y);
                
                

                if (xObjectOn.ClipRect.HasValue)
                {
                    this._location.X += xObjectOn.ClipRect.Value.X;
                    this._location.Y += xObjectOn.ClipRect.Value.Y;
                    this._size.Width = xObjectOn.ClipRect.Value.Width;
                    this._size.Height = xObjectOn.ClipRect.Value.Height;
                    bounds = parentRenderBounds;
                }
            }
            
            
            prevXclude = xObjectOn.ChildContainer.ExcludeFromOutput;
            
            xObjectOff.ChildContainer.ExcludeFromOutput = false;
            var offRef = xObjectOff.OutputToPDF(context, writer);
            xObjectOff.ChildContainer.ExcludeFromOutput = prevXclude;

            //Every radio (grouped or not - PDFAcrobatFormEntry.RegisterField always routes them
            //through a PDFAcrobatRadioGroupEntry) is a /Kids entry of that group's own node, which
            //declares /T, /FT, /Ff and /V once for the whole group - duplicating them here would be
            //actively wrong, not just redundant: a Kids-array member that declares its own /T
            //becomes a SEPARATE field in the field-name hierarchy ("groupname.kidname"), not a
            //plain annotation-kid of the group, which is exactly what broke mutual exclusivity when
            //this was first attempted - each radio ended up its own independent field.
            //
            ///Parent can't just be set by the group ahead of time - PDFAnnotationEntry.OutputToPDF
            //caches a widget's content on its FIRST call, and that call comes from the page's own
            ///Annots output (rendered while laying out pages), before PDFAcrobatRadioGroupEntry's
            //own OutputToPDF (part of the /AcroForm catalog entry, written only once every page is
            //known) ever runs. So whichever kid is asked to render first triggers the WHOLE group -
            //opens its indirect object, sets /Parent on every kid immediately, renders every other
            //kid nested inside it - using the writer's existing nested-indirect-object support (the
            //same pattern already used to collect and write a nested /Catalog reference), not any
            //"reserve a number, write content later" writer change. This kid then continues
            //rendering itself normally (below, still nested inside the still-open group), and
            //completes the group (writes its /Kids dictionary, closes it) once its own ref is known.
            bool isGroupedRadio = (this.FieldOptions & FormFieldOptions.Radio) == FormFieldOptions.Radio;
            bool triggeredGroup = false;

            if (isGroupedRadio && null != this.Group && !this.Group.IsOpen)
            {
                triggeredGroup = true;
                this.Group.BeginFromKid(context, writer, this);
            }

            PDFObjectRef root = writer.BeginObject();

            var font = this._style.CreateFont();
            var rsrc = ((Document)xObjectOn.Document).GetFontResource(font, true);
            string da = rsrc.Name.ToString() + " " + font.Size.ToPoints().Value.ToString() + " Tf";

            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Subtype", "Widget");

            if (isGroupedRadio)
            {
                if (null != this.Parent)
                    writer.WriteDictionaryObjectRefEntry("Parent", this.Parent);
            }
            else
            {
                writer.WriteDictionaryStringEntry("T", this.Name);
                writer.WriteDictionaryNameEntry("V", currentState);
                writer.WriteDictionaryNumberEntry("Ff", (int)this.FieldOptions);
                writer.WriteDictionaryNameEntry("FT", GetFieldTypeName(this.FieldType));
            }

            if (!string.IsNullOrEmpty(this.DefaultValue))
                writer.WriteDictionaryStringEntry("DV", this.DefaultValue);

            writer.WriteDictionaryStringEntry("DA", da);
            writer.WriteDictionaryNameEntry("AS", currentState);

            if (null != this._page && null != this._page.PageObjectRef)
                writer.WriteDictionaryObjectRefEntry("P", this._page.PageObjectRef);

            WriteAction(context, writer);

            //MK - appearance dictionary
            writer.BeginDictionaryEntry("MK");
            writer.BeginDictionary();
            if (this._style.IsValueDefined(Styles.StyleKeys.BorderColorKey))
                WriteInputColor(context, writer, "BC", this._style.Border.Color);
            if (this._style.IsValueDefined(Styles.StyleKeys.BgColorKey))
                WriteInputColor(context, writer, "BG", this._style.Background.Color);
            writer.EndDictionary();
            writer.EndDictionaryEntry();

            //AP - nested by state name, rather than the flat N/D/R of other field types
            writer.BeginDictionaryEntry("AP");
            writer.BeginDictionary();
            
            writer.BeginDictionaryEntry("N");
            writer.BeginDictionary();
            writer.WriteDictionaryObjectRefEntry("Off", offRef);
            writer.WriteDictionaryObjectRefEntry(onName, onRef);
            writer.EndDictionary();
            writer.EndDictionaryEntry();
            
            writer.EndDictionary();
            writer.EndDictionaryEntry();
            
            this._location.X += this.ContainerOffset.X;
            this._location.Y += this.ContainerOffset.Y;

            PDFReal left = context.Graphics.GetXPosition(_location.X);
            PDFReal top = context.Graphics.GetYPosition(_location.Y);
            PDFReal right = left + context.Graphics.GetXOffset(_size.Width);
            PDFReal bottom = top + context.Graphics.GetYOffset(_size.Height);

            writer.BeginDictionaryEntry("Rect");
            writer.WriteArrayRealEntries(true, left.Value, bottom.Value, right.Value, top.Value);
            writer.EndDictionaryEntry();

            writer.EndDictionary();
            writer.EndObject();

            if (triggeredGroup)
                //My own ref is now known - complete the group's /Kids array (adding it) and
                //close the group's still-open indirect object, nested around every kid rendered
                //above.
                this.Group.CompleteFromKid(root);

            //Only the widget's own object goes into /Fields or /Kids - offRef/onRef are
            //referenced solely via this widget's own /AP /N dictionary, the same way the base
            //class's flat /AP appearance objects are never included in its own returned refs.
            return new PDFObjectRef[] { root };
        }
        
    }
}
