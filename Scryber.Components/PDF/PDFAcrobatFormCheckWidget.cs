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

        public PDFAcrobatFormCheckWidget(string name, string value, string defaultValue, FormInputFieldType type, FormFieldOptions options)
            : base(name, value, defaultValue, type, options)
        {
        }

        protected override IEnumerable<PDFObjectRef> DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            var xObject = this._states.ContainsKey(FormFieldAppearanceState.Normal) ? this._states[FormFieldAppearanceState.Normal] : null;
            if (null == xObject)
                return null;

            var size = new Drawing.Size(xObject.Width, xObject.Height);
            var location = xObject.Location;
            string onName = string.IsNullOrEmpty(this.OnStateName) ? "on" : this.OnStateName;
            string currentState = this.IsChecked ? onName : "Off";

            var offRef = WriteMarkAppearance(context, writer, size, false);
            var onRef = WriteMarkAppearance(context, writer, size, true, onName);

            PDFObjectRef root = writer.BeginObject();

            var font = this._style.CreateFont();
            var rsrc = ((Document)xObject.Document).GetFontResource(font, true);
            string da = rsrc.Name.ToString() + " " + font.Size.ToPoints().Value.ToString() + " Tf";

            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Subtype", "Widget");
            writer.WriteDictionaryStringEntry("T", this.Name);
            writer.WriteDictionaryNameEntry("V", currentState);

            if (!string.IsNullOrEmpty(this.DefaultValue))
                writer.WriteDictionaryStringEntry("DV", this.DefaultValue);

            writer.WriteDictionaryNumberEntry("Ff", (int)this.FieldOptions);
            writer.WriteDictionaryStringEntry("DA", da);
            writer.WriteDictionaryNameEntry("FT", GetFieldTypeName(this.FieldType));
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

            PDFReal left = context.Graphics.GetXPosition(location.X);
            PDFReal top = context.Graphics.GetYPosition(location.Y);
            PDFReal right = left + context.Graphics.GetXOffset(size.Width);
            PDFReal bottom = top + context.Graphics.GetYOffset(size.Height);

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
                        g.FillRectangle(brush, insetX, insetY, markWidth, markHeight);
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
