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
            

            string onName = "On";
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
                    bounds = parentRenderBounds;
                }
            }
            
            
            prevXclude = xObjectOn.ChildContainer.ExcludeFromOutput;
            
            xObjectOff.ChildContainer.ExcludeFromOutput = false;
            var offRef = xObjectOff.OutputToPDF(context, writer);
            xObjectOff.ChildContainer.ExcludeFromOutput = prevXclude;

            PDFObjectRef root = writer.BeginObject();

            var font = this._style.CreateFont();
            var rsrc = ((Document)xObjectOn.Document).GetFontResource(font, true);
            string da = rsrc.Name.ToString() + " " + font.Size.ToPoints().Value.ToString() + " Tf";

            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Subtype", "Widget");

            if (null != this.Parent)
            {
                //Grouped under a PDFAcrobatRadioGroupEntry, which already declares /T, /FT, /Ff
                //and /V once for the whole group - duplicating them here is unnecessary and, for
                ///V specifically, wrong (the group's /V is the one shared selected value; this
                //kid's own current state is /AS alone).
                //
                //Currently dormant in practice: PDFAnnotationEntry.OutputToPDF caches this widget's
                //content on its FIRST call, and that first call comes from the page's own /Annots
                //output (rendered while laying out pages) - which happens before
                //PDFAcrobatRadioGroupEntry.OutputToPDF (part of the /AcroForm catalog entry,
                //written only once every page is known) ever gets a chance to set Parent. So this
                //widget's real /T/FT/Ff/V still get written directly below, every time, until the
                //group's object number can be reserved earlier than that (a writer-level change -
                //see PDFWriter.InitializeIndirectObject/XRefTable.Append - deliberately deferred,
                //not worth the risk for what's currently just a Chrome display quirk).
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
            
            writer.BeginDictionaryEntry("D");
            writer.BeginDictionary();
            writer.WriteDictionaryObjectRefEntry("Off", offRef);
            writer.WriteDictionaryObjectRefEntry(onName, onRef);
            writer.EndDictionary();
            writer.EndDictionaryEntry();
            
            writer.BeginDictionaryEntry("R");
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

            //Only the widget's own object goes into /Fields or /Kids - offRef/onRef are
            //referenced solely via this widget's own /AP /N dictionary, the same way the base
            //class's flat /AP appearance objects are never included in its own returned refs.
            return new PDFObjectRef[] { root };
        }

        /// <summary>
        /// Hand-draws one appearance state (a bordered box, and for the "on" state an inner mark)
        /// as its own small Form XObject - independent of the layout engine, since this content
        /// never goes through a normal layout pass.
        /// </summary>
        private PDFObjectRef WriteMarkAppearance(PDFRenderContext context, PDFWriter writer, Drawing.Size size, bool isOn, string onName = null)
        {
            var oref = writer.BeginObject();
            writer.BeginStream(oref);

            var markColor = this._style.IsValueDefined(Styles.StyleKeys.BorderColorKey) ? this._style.Border.Color : StandardColors.Black;
            var borderWidth = this._style.Border.Width;

            using (var g = PDFGraphics.Create(writer, false, this._page, DrawingOrigin.TopLeft, size, context))
            {
                var pen = PDFPen.Create(markColor, borderWidth);
                g.DrawRectangle(pen, 0, 0, size.Width, size.Height);

                if (isOn)
                {
                    var brush = new PDFSolidBrush(markColor);
                    var insetX = size.Width * 0.25;
                    var insetY = size.Height * 0.25;
                    var markWidth = size.Width - (insetX * 2);
                    var markHeight = size.Height - (insetY * 2);

                    if (this.ButtonType == FormButtonFieldType.Radio)
                        g.FillElipse(brush, insetX, insetY, markWidth, markHeight);
                    else
                    {
                        var path = new GraphicsPath();
                        path.BeginPath();
                        path.MoveTo(new Point(insetX, size.Height / 2.0));
                        path.LineTo(new Point(size.Width / 3.0, markHeight));
                        path.LineTo(new Point(markWidth, insetY));
                        path.ClosePath(false);
                        g.DrawPath(pen, Point.Empty, path);
                        
                    }
                }
            }

            var len = writer.EndStream();
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "XObject");
            writer.WriteDictionaryNameEntry("Subtype", "Form");
            writer.BeginDictionaryEntry("BBox");
            writer.WriteArrayRealEntries(true, 0f, 0f, (float)size.Width.PointsValue, (float)size.Height.PointsValue);
            writer.EndDictionaryEntry();
            writer.BeginDictionaryEntry("Length");
            writer.WriteNumberS(len);
            writer.EndDictionaryEntry();
            writer.EndDictionary();
            writer.EndObject();

            return oref;
        }
    }
}
