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

                context.TraceLog.Add(TraceLevel.Error, "Checkbox", "Both On and Off Appearances must be defined. The stated dictionary did not contain both.");
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
                    Style = arrangement.FullStyle;
                    break;
                }

                owner = owner.Parent;
            }
            

            string onName = string.IsNullOrEmpty(this.OnStateName) ?  "On" :  this.OnStateName;
            string currentState = this.IsChecked ? onName : "Off";

            this.Location = context.Offset;
            
            //render the On state and record sizes.
            
            var prevXclude = xObjectOn.ChildContainer.ExcludeFromOutput;
            
            xObjectOn.ChildContainer.ExcludeFromOutput = false;
            var onRef = xObjectOn.OutputToPDF(context, writer);
            xObjectOn.ChildContainer.ExcludeFromOutput = prevXclude;

            if (null != onRef)
            {
                var sz = new Size(xObjectOn.Width, xObjectOn.Height);
                if (Size == Size.Empty)
                    Size = sz;
                else
                {
                    if(Size.Width < sz.Width)
                        Size.Width = sz.Width;
                    if(Size.Height < sz.Height)
                        Size.Height = sz.Height;
                }
                this.Location = new Point(xObjectOn.Location.X, xObjectOn.Location.Y);
                
                

                if (xObjectOn.ClipRect.HasValue)
                {
                    this.Location.X += xObjectOn.ClipRect.Value.X;
                    this.Location.Y += xObjectOn.ClipRect.Value.Y;
                    this.Size.Width = xObjectOn.ClipRect.Value.Width;
                    this.Size.Height = xObjectOn.ClipRect.Value.Height;
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
            
            bool isGroupedRadio = (this.FieldOptions & FormFieldOptions.Radio) == FormFieldOptions.Radio;
            bool triggeredGroup = false;

            if (isGroupedRadio && null != this.Group && !this.Group.IsOpen)
            {
                triggeredGroup = true;
                this.Group.BeginFromKid(context, writer, this);
            }

            PDFObjectRef root = writer.BeginObject();

            try
            {
                var font = this.Style.CreateFont();
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

                if (null != this.Page && null != this.Page.PageObjectRef)
                    writer.WriteDictionaryObjectRefEntry("P", this.Page.PageObjectRef);

                WriteAction(context, writer);

                //MK - appearance dictionary
                writer.BeginDictionaryEntry("MK");
                writer.BeginDictionary();
                if (this.Style.IsValueDefined(Styles.StyleKeys.BorderColorKey))
                    WriteInputColor(context, writer, "BC", this.Style.Border.Color);
                if (this.Style.IsValueDefined(Styles.StyleKeys.BgColorKey))
                    WriteInputColor(context, writer, "BG", this.Style.Background.Color);
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

                this.Location.X += this.ContainerOffset.X;
                this.Location.Y += this.ContainerOffset.Y;

                PDFReal left = context.Graphics.GetXPosition(Location.X);
                PDFReal top = context.Graphics.GetYPosition(Location.Y);
                PDFReal right = left + context.Graphics.GetXOffset(Size.Width);
                PDFReal bottom = top + context.Graphics.GetYOffset(Size.Height);

                writer.BeginDictionaryEntry("Rect");
                writer.WriteArrayRealEntries(true, left.Value, bottom.Value, right.Value, top.Value);
                writer.EndDictionaryEntry();

                writer.EndDictionary();
            }
            finally
            {
                if (null != root)
                    writer.EndObject();
            }

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
