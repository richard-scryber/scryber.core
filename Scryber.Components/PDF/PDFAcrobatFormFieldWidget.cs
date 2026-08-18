using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Drawing;
using Scryber.PDF.Layout;
using Scryber.PDF.Graphics;
using Scryber.Components;

namespace Scryber.PDF
{
    public class PDFAcrobatFormFieldWidget : PDFAnnotationEntry, IPDFFormFieldNode
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string DefaultValue { get; set; }

        public FormFieldOptions FieldOptions { get; set; }

        public FormInputFieldType FieldType { get; set; }
        
        public int MaxLength { get; set; }
        

        /// <summary>
        /// The choices for a Choice (select/combo/list) field, written as the /Opt array.
        /// Null or empty for any other field type.
        /// </summary>
        public IEnumerable<Scryber.Components.FormFieldOption> Choices { get; set; }

        /// <summary>
        /// The action to fire on activation (e.g. a submit/reset action for a button-behaviour
        /// field). Null for fields with no click behaviour.
        /// </summary>
        public PDFAction Action { get; set; }

        public IEnumerable<IResourceContainer> Resources
        {
            get { return this._states.Values.AsEnumerable<IResourceContainer>(); }
        }

        protected Dictionary<FormFieldAppearanceState, Layout.PDFLayoutXObjectRun> _states;

        /// <summary>
        /// Per-state styles for Over/Down, populated only when a :hover/:active rule actually
        /// matched this field with its own values - drives a colour-only repaint of Normal's box
        /// at output time. Never populated for Normal itself (that's _style).
        /// </summary>
        protected Dictionary<FormFieldAppearanceState, Styles.Style> _stateStyles = new Dictionary<FormFieldAppearanceState, Styles.Style>();

        protected Drawing.Point _location;
        protected Drawing.Size _size;
        protected Layout.PDFLayoutPage _page;
        protected Styles.Style _style;

        public PDFAcrobatFormFieldWidget(string name, string value, string defaultValue, FormInputFieldType type, FormFieldOptions options)
        {
            this.Name = name;
            this.Value = value;
            this.FieldOptions = options;
            this.FieldType = type;
            this._states = new Dictionary<FormFieldAppearanceState, Layout.PDFLayoutXObjectRun>();
            this.DefaultValue = defaultValue;
        }

        /// <summary>
        /// stateStyle is the state-specific style (from Style.TryGetStyleState) when a :hover/:active
        /// rule matched this field, or null when none did - in which case this state's appearance
        /// just reuses the Normal xObject unchanged, exactly as before this existed.
        /// </summary>
        public void SetAppearance(FormFieldAppearanceState state, PDFLayoutXObjectRun xObject, Layout.PDFLayoutPage page, Styles.Style style, Styles.Style stateStyle = null)
        {
            this._states[state] = xObject;
            if (state == FormFieldAppearanceState.Normal)
                this._style = style;
            else if (null != stateStyle)
                this._stateStyles[state] = stateStyle;
            this._page = page;
        }

        

        protected override IEnumerable<PDFObjectRef> DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            //Get the default font and size required for the DA (default Appearance value)
            var xObject = this._states[FormFieldAppearanceState.Normal];
            if (null == xObject)
                return null;

            PDFObjectRef root = writer.BeginObject();

            var font = this._style.CreateFont();
            //GetResource(..., create:true) only works if this exact font was already registered
            //by something else (its create-fallback goes through the never-implemented
            //FontFactory.GetFontDefinition(string) overload) - GetFontResource is the real,
            //self-sufficient resolver normal text layout uses (see LayoutEngineText).
            var rsrc = ((Document)xObject.Document).GetFontResource(font, true);
            string da = rsrc.Name.ToString() + " " + font.Size.ToPoints().Value.ToString() + " Tf";

            //DA is what a reader falls back to whenever it regenerates a field's appearance itself
            //(e.g. after the user edits the value) - without a colour operator here, that
            //regenerated text always comes out default black regardless of the field's CSS colour.
            if (this._style.IsValueDefined(Styles.StyleKeys.FillColorKey))
            {
                var color = this._style.Fill.Color;
                if (color.ColorSpace == ColorSpace.RGB)
                    da += " " + color.Red.ToString() + " " + color.Green.ToString() + " " + color.Blue.ToString() + " rg";
                else if (color.ColorSpace == ColorSpace.G)
                    da += " " + color.Gray.ToString() + " g";
            }

            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Subtype", "Widget");
            writer.WriteDictionaryStringEntry("T", this.Name);
            
            if (!string.IsNullOrEmpty(this.Value))
            {
                writer.WriteDictionaryStringEntry("V", this.Value);
            }

            if(!string.IsNullOrEmpty(this.DefaultValue))
            {
                writer.WriteDictionaryStringEntry("DV", this.DefaultValue);
            }

            writer.WriteDictionaryNumberEntry("Ff", (int)this.FieldOptions);
            if (this.MaxLength > 0)
                writer.WriteDictionaryNumberEntry("MaxLen", this.MaxLength);
            writer.WriteDictionaryStringEntry("DA", da);
            writer.WriteDictionaryNameEntry("FT", GetFieldTypeName(this.FieldType));
            if (null != this._page && null != this._page.PageObjectRef)
                writer.WriteDictionaryObjectRefEntry("P", this._page.PageObjectRef);

            WriteAction(context, writer);

            if (null != this.Choices && this.Choices.Any())
            {
                writer.BeginDictionaryEntry("Opt");
                writer.BeginArray();
                foreach (var choice in this.Choices)
                {
                    writer.BeginArrayEntry();
                    writer.BeginArray();
                    writer.BeginArrayEntry();
                    writer.WriteStringLiteral(choice.Value ?? string.Empty);
                    writer.EndArrayEntry();
                    writer.BeginArrayEntry();
                    writer.WriteStringLiteral(choice.Label ?? string.Empty);
                    writer.EndArrayEntry();
                    writer.EndArray();
                    writer.EndArrayEntry();
                }
                writer.EndArray();
                writer.EndDictionaryEntry();
            }

            //MK - appearance dictionary
            writer.BeginDictionaryEntry("MK");
            writer.BeginDictionary();

            if (this._style.IsValueDefined(Styles.StyleKeys.BorderColorKey))
            {
                WriteInputColor(context, writer, "BC", this._style.Border.Color);
            }
            if (this._style.IsValueDefined(Styles.StyleKeys.BgColorKey))
            {
                WriteInputColor(context, writer, "BG", this._style.Background.Color);
            }

            //A pushbutton's visible label comes from /MK /CA (its "caption"), not from /V or
            //whatever text happens to be baked into our own /AP - readers that regenerate a
            //pushbutton's appearance (which NeedAppearances invites, and buttons especially
            //tend to get fully self-rendered) look here for what to draw, and draw a blank
            //button without it.
            if ((this.FieldOptions & FormFieldOptions.Pushbutton) == FormFieldOptions.Pushbutton
                && !string.IsNullOrEmpty(this.Value))
            {
                writer.WriteDictionaryStringEntry("CA", this.Value);
            }

            writer.EndDictionary();
            writer.EndDictionaryEntry();

            //Only buttons self-render an /AP for now - everything else relies on the reader
            //regenerating its own appearance from /DA + /MK via /NeedAppearances. xObject.OutputToPDF
            //still runs below for every field regardless (its side effects - Location/Width/Height -
            //are what /Rect is computed from), it just isn't wired into a real /AP dictionary unless
            //this is a pushbutton, so non-button fields end up with a harmless unreferenced XObject.
            bool isPushbutton = (this.FieldOptions & FormFieldOptions.Pushbutton) == FormFieldOptions.Pushbutton;

            if (this._states.Count > 0)
            {
                _location = context.Offset;

                Drawing.Rect bounds = Drawing.Rect.Empty;
                if (isPushbutton)
                {
                    writer.BeginDictionaryEntry("AP");
                    writer.BeginDictionary();
                }
                foreach (var kvp in _states)
                {
                    xObject = kvp.Value;
                    FormFieldAppearanceState state = kvp.Key;

                    PDFObjectRef oref;
                    if (this._stateStyles.TryGetValue(state, out var stateStyle))
                        oref = WriteRepaintedAppearance(context, writer, xObject, stateStyle, state);
                    else
                        oref =  xObject.OutputToPDF(context, writer);

                    if (null != oref)
                    {
                        
                        Size sz = new Drawing.Size(xObject.Width, xObject.Height);
                        if (_size == Size.Empty)
                            _size = sz;
                        else
                        {
                            if (_size.Width < sz.Width)
                                _size.Width = sz.Width;
                            if (_size.Height < sz.Height)
                                _size.Height = sz.Height;
                        }
                        if (isPushbutton)
                        {
                            var name = GetFieldStateName(kvp.Key);
                            writer.WriteDictionaryObjectRefEntry(name, oref);
                        }

                        //We should have all states starting at the same location no matter what.
                        this._location = new Point(xObject.Location.X, xObject.Location.Y);

                        var pos = this._style.CreatePostionOptions(false);
                        if (!pos.Margins.IsEmpty && pos.DisplayMode != DisplayMode.Inline)
                        {
                            _location.X += pos.Margins.Left;
                            _location.Y += pos.Margins.Top;
                            _size.Width -= pos.Margins.Right + pos.Margins.Left;
                            _size.Height -= pos.Margins.Top + pos.Margins.Bottom;
                        }
                    }
                }
                if (isPushbutton)
                {
                    writer.EndDictionary();
                    writer.EndDictionaryEntry();
                }

                PDFReal left = context.Graphics.GetXPosition(_location.X);
                PDFReal top = context.Graphics.GetYPosition(_location.Y);
                PDFReal right = left + context.Graphics.GetXOffset(_size.Width);
                PDFReal bottom = top + context.Graphics.GetYOffset(_size.Height);

                writer.BeginDictionaryEntry("Rect");
                writer.WriteArrayRealEntries(true, left.Value, bottom.Value, right.Value, top.Value);
                writer.EndDictionaryEntry();
            }
            writer.EndDictionary();
            writer.EndObject();
            //context.Offset = new PDFPoint(context.Offset.X, context.Offset.Y + _size.Height);
            return new PDFObjectRef[] { root };
        }

        /// <summary>
        /// A colour-only repaint of Normal's exact box (same geometry, hand-drawn independently
        /// of the layout engine like the checkbox/radio appearances) using a :hover/:active
        /// state's background/border colours, falling back to Normal's own colour for whichever
        /// of the two the state doesn't override. Text content isn't reproduced here - a full
        /// second layout pass per state was explicitly out of scope for this - so Normal remains
        /// the only appearance state that shows the field's value text.
        /// </summary>
        private PDFObjectRef WriteRepaintedAppearance(PDFRenderContext context, PDFWriter writer, PDFLayoutXObjectRun normalXObject, Styles.Style stateStyle, FormFieldAppearanceState state)
        {
            var size = new Drawing.Size(normalXObject.Width, normalXObject.Height);
            var origGraphics = context.Graphics;
            
            var oref = writer.BeginObject();
            writer.BeginStream(oref);

            using (var g = PDFGraphics.Create(writer, false, this._page, DrawingOrigin.TopLeft, size, context))
            {
                context.Graphics = g;
                var rect = new Rect(0, 0, size.Width, size.Height);
                var borders = stateStyle.CreateBorderPen();
                var bg = stateStyle.CreateBackgroundBrush();
                
                PDFLayoutItem.OutputBackground(bg, borders, context, rect);
                
                PDFLayoutItem.OutputBorder(bg, borders, context, rect);
                
                
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

            context.Graphics = origGraphics;

            return oref;
        }

        /// <summary>
        /// Writes /A inline, mirroring PDFAnnotationLinkEntry's exact pattern - the action's own
        /// OutputToPDF writes its dictionary directly (it returns null rather than an indirect ref).
        /// </summary>
        protected void WriteAction(PDFRenderContext context, PDFWriter writer)
        {
            if (null != this.Action)
            {
                writer.BeginDictionaryEntry("A");
                var actionref = this.Action.OutputToPDF(context, writer);
                if (null != actionref)
                    writer.WriteObjectRefS(actionref);
                writer.EndDictionaryEntry();
            }
        }

        protected void WriteInputColor(PDFRenderContext context, PDFWriter writer, string key, Color color)
        {
            writer.BeginDictionaryEntry(key);

            if (color.ColorSpace == ColorSpace.RGB)
                writer.WriteArrayRealEntries(true, color.Red, color.Green, color.Blue);
            else if (color.ColorSpace == ColorSpace.G)
                writer.WriteArrayRealEntries(true, color.Gray);
            else
            {
                writer.BeginArray();
                writer.EndArray();
                context.TraceLog.Add(TraceLevel.Warning, "Output", "The color space " + color.ColorSpace.ToString() + " is not supported in input backgrounds");
            }
            writer.EndDictionaryEntry();
        }

        protected static string GetFieldStateName(FormFieldAppearanceState state)
        {
            switch (state)
            {
                case FormFieldAppearanceState.Normal:
                    return "N";
                case FormFieldAppearanceState.Over:
                    return "R";
                case FormFieldAppearanceState.Down:
                    return "D";
                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }
        }

        protected static string GetFieldTypeName(FormInputFieldType type)
        {
            switch (type)
            {
                case FormInputFieldType.Text:
                    return "Tx";
                case FormInputFieldType.Button:
                    return "Btn";
                case FormInputFieldType.Choice:
                    return "Ch";
                case FormInputFieldType.Signature:
                    return "Sig";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }

    

    public class PDFAcrobatFormFieldEntryList : List<PDFAcrobatFormFieldWidget>
    {

    }
}
