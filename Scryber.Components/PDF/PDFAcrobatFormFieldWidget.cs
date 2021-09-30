using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Drawing;
using Scryber.PDF.Layout;

namespace Scryber.PDF
{
    public class PDFAcrobatFormFieldWidget : PDFAnnotationEntry
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string DefaultValue { get; set; }

        public FormFieldOptions FieldOptions { get; set; }

        public FormInputFieldType FieldType { get; set; }

        public IEnumerable<IResourceContainer> Resources
        {
            get { return this._states.Values.AsEnumerable<IResourceContainer>(); }
        }

        private Dictionary<FormFieldAppearanceState, Layout.PDFLayoutXObject> _states;

        private Drawing.PDFPoint _location;
        private Drawing.PDFSize _size;
        private Layout.PDFLayoutPage _page;
        private Styles.Style _style;

        public PDFAcrobatFormFieldWidget(string name, string value, string defaultValue, FormInputFieldType type, FormFieldOptions options)
        {
            this.Name = name;
            this.Value = value;
            this.FieldOptions = options;
            this.FieldType = type;
            this._states = new Dictionary<FormFieldAppearanceState, Layout.PDFLayoutXObject>();
            this.DefaultValue = defaultValue;
        }

        public void SetAppearance(FormFieldAppearanceState state, PDFLayoutXObject xObject, Layout.PDFLayoutPage page, Styles.Style style)
        {
            this._states[state] = xObject;
            if (state == FormFieldAppearanceState.Normal)
                this._style = style;
            this._page = page;
        }

        

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            //Get the default font and size required for the DA (default Appearance value)
            var xObject = this._states[FormFieldAppearanceState.Normal];
            if (null == xObject)
                return null;

            PDFObjectRef root = writer.BeginObject();

            var font = this._style.CreateFont();
            var rsrc = xObject.Document.GetResource(PDFResource.FontDefnResourceType, font.FullName, true) as PDFResource;
            string da = rsrc.Name.ToString() + " " + font.Size.ToPoints().Value.ToString() + " Tf";

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

            writer.WriteDictionaryNumberEntry("Ff", (int)this.FieldOptions + (int)this.FieldType);
            writer.WriteDictionaryStringEntry("DA", da);
            writer.WriteDictionaryNameEntry("FT", GetFieldTypeName(this.FieldType));
            if (null != this._page && null != this._page.PageObjectRef)
                writer.WriteDictionaryObjectRefEntry("P", this._page.PageObjectRef);

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
            writer.EndDictionary();
            writer.EndDictionaryEntry();

            if (this._states.Count > 0)
            {
                _location = context.Offset;

                Drawing.PDFRect bounds = Drawing.PDFRect.Empty;
                writer.BeginDictionaryEntry("AP");
                writer.BeginDictionary();
                foreach (var kvp in _states)
                {
                    xObject = kvp.Value;
                    FormFieldAppearanceState state = kvp.Key;

                    PDFObjectRef oref = xObject.OutputToPDF(context, writer);
                    
                    if (null != oref)
                    {
                        
                        PDFSize sz = new Drawing.PDFSize(xObject.Width, xObject.Height);
                        if (_size == PDFSize.Empty)
                            _size = sz;
                        else
                        {
                            if (_size.Width < sz.Width)
                                _size.Width = sz.Width;
                            if (_size.Height < sz.Height)
                                _size.Height = sz.Height;
                        }
                        var name = GetFieldStateName(kvp.Key);
                        writer.WriteDictionaryObjectRefEntry(name, oref);

                        //We should have all states starting at the same location no matter what.
                        this._location = xObject.Location; 
                    }
                }
                writer.EndDictionary();
                writer.EndDictionaryEntry();

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
            return root;
        }

        private void WriteInputColor(PDFRenderContext context, PDFWriter writer, string key, PDFColor color)
        {
            writer.BeginDictionaryEntry(key);
            if (color.ColorSpace == ColorSpace.RGB)
                writer.WriteArrayRealEntries(true, color.Red.Value, color.Green.Value, color.Blue.Value);
            else if (color.ColorSpace == ColorSpace.G)
                writer.WriteArrayRealEntries(true, color.Gray.Value);
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
