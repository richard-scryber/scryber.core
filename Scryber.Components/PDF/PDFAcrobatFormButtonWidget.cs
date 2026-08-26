using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.PDF.Native;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.PDF
{
    public class PDFAcrobatFormButtonWidget : PDFAcrobatFormFieldWidget
    {

        public PDFAcrobatFormButtonWidget(string name, string value, string defaultValue, FormInputFieldType type, FormFieldOptions options) :
            base(name, value, defaultValue, type, options)
        {
        }

        protected override IEnumerable<PDFObjectRef> DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            var xObjectNormal = this._states.ContainsKey(FormFieldAppearanceState.Normal) 
                ? this._states[FormFieldAppearanceState.Normal] : null;
            var xObjectOver = this._states.ContainsKey(FormFieldAppearanceState.Over)
                ? this._states[FormFieldAppearanceState.Over] : xObjectNormal;
            var xObjectDown = this._states.ContainsKey(FormFieldAppearanceState.Down)
                ? this._states[FormFieldAppearanceState.Down] : xObjectNormal;

            if (null == xObjectNormal)
            {
                if (context.Conformance == ParserConformanceMode.Strict)
                    throw new InvalidOperationException(
                        "The button does not have a normal appearance. Cannot render");

                context.TraceLog.Add(TraceLevel.Error, "Button", "The button does not have a normal appearance defined. The stated dictionary must at least have a normal appearance.");
                return null;
            }
            
            var parentRenderBounds = Rect.Empty;
            ComponentArrangement arrangement = null;
            
            var owner = xObjectNormal.Owner as Component;
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
            
            this.Location = new Point(context.Offset.X + xObjectNormal.Location.X, context.Offset.Y + xObjectNormal.Location.Y);
            
            var sz = new Size(xObjectNormal.Width, xObjectNormal.Height);
            if(this.Size == Size.Empty)
                this.Size = sz;
            else
            {
                if(this.Size.Width < sz.Width)
                    this.Size.Width = sz.Width;
                if(this.Size.Height < sz.Height)
                    this.Size.Height = sz.Height;
            }
            
            

            if (xObjectNormal.ClipRect.HasValue)
            {
                this.Location.X += xObjectNormal.ClipRect.Value.X;
                this.Location.Y += xObjectNormal.ClipRect.Value.Y;
                this.Size.Width = xObjectNormal.ClipRect.Value.Width;
                this.Size.Height = xObjectNormal.ClipRect.Value.Height;
            }
            
            
            //render the normal state
            var prevExclude = xObjectNormal.ChildContainer.ExcludeFromOutput;
            xObjectNormal.ChildContainer.ExcludeFromOutput = false;
            var normalRef = xObjectNormal.OutputToPDF(context, writer);
            xObjectNormal.ChildContainer.ExcludeFromOutput = prevExclude;

            if (null == normalRef)
            {
                if(context.Conformance == ParserConformanceMode.Strict)
                    throw new InvalidOperationException("The button did not render a normal appearance xObject. Cannot continue");
                
                context.TraceLog.Add(TraceLevel.Error, "Button", "The button did not render a normal state xObject, cannot continue");
                return null;
            }

            PDFObjectRef root = writer.BeginObject();

            try
            {
                var font = this.Style.CreateFont();
                var rsrc = ((Document)xObjectNormal.Document).GetFontResource(font, true);
                string da = rsrc.Name.ToString() + " " + font.Size.ToPoints().Value.ToString() + " Tf";
                
                writer.BeginDictionary();
                
                //Consolidate - standard entries
                writer.WriteDictionaryNameEntry("Subtype", "Widget");
                writer.WriteDictionaryStringEntry("T", this.Name);
                writer.WriteDictionaryNameEntry("FT", GetFieldTypeName(this.FieldType));
                writer.WriteDictionaryNumberEntry("Ff", (int)this.FieldOptions);
                if (null != this.Page && null != this.Page.PageObjectRef)
                    writer.WriteDictionaryObjectRefEntry("P", this.Page.PageObjectRef);

                writer.WriteDictionaryNameEntry("V", this.Value);
                writer.WriteDictionaryStringEntry("DA", da);

                if (!string.IsNullOrEmpty(this.Value))
                    writer.WriteDictionaryStringEntry("DV", this.Value);
                
                this.WriteAction(context, writer);
                
                //BS - border style. Explicitly zero-width, all style in the AP's
                writer.BeginDictionaryEntry("BS");
                writer.BeginDictionary();
                writer.WriteDictionaryNumberEntry("W", 0);
                writer.EndDictionary();
                writer.EndDictionaryEntry();

                //MK - appearance dictionary
                writer.BeginDictionaryEntry("MK");
                writer.BeginDictionary();
                
                if (this.Style.IsValueDefined(Styles.StyleKeys.BgColorKey))
                {
                    WriteInputColor(context, writer, "BG", this.Style.Background.Color);
                }

                if (!string.IsNullOrEmpty(this.Value))
                    writer.WriteDictionaryStringEntry("CA", this.Value);

                writer.EndDictionary();
                writer.EndDictionaryEntry();
                
                //Appearance
                this.WriteAppearances(context, writer, xObjectNormal, xObjectOver, xObjectDown);
                
                //rect bounds
                this.WriteButtonRect(context, writer, xObjectNormal, xObjectOver, xObjectDown);
                
                writer.EndDictionary();
                
            }
            finally
            {
                if (null != root)
                    writer.EndObject();
            }
            
            return new PDFObjectRef[] {root};
        }

        protected virtual void WriteAppearances(PDFRenderContext context, PDFWriter writer, 
            PDFLayoutXObjectRun normal, PDFLayoutXObjectRun over, PDFLayoutXObjectRun down)
        {
            PDFObjectRef normalOref = null;
            PDFObjectRef downOref = null;
            PDFObjectRef overOref = null;
            
            writer.BeginDictionaryEntry("AP");
            writer.BeginDictionary();
            
            try
            {
                normalOref = normal.OutputToPDF(context, writer);
                
                if(null == normalOref)
                    throw new InvalidOperationException("The normal state does not have a normal appearance object reference.");

                if (null != down)
                {
                    downOref = down.OutputToPDF(context, writer);
                    
                    if(null == downOref)
                        throw new InvalidOperationException("The down state has a layout, but does not have an appearance object reference.");

                }
                else
                {
                    downOref = normalOref;
                }

                if (null != over)
                {
                    overOref = over.OutputToPDF(context, writer);
                    
                    if(null == overOref)
                        throw new InvalidOperationException("The over state has a layout, but does not have a appearance object reference.");

                }
                else
                {
                    overOref = normalOref;
                }

                var name = GetFieldStateName(FormFieldAppearanceState.Normal);
                writer.WriteDictionaryObjectRefEntry(name, normalOref);
                
                name = GetFieldStateName(FormFieldAppearanceState.Over);
                writer.WriteDictionaryObjectRefEntry(name, overOref);
                
                name = GetFieldStateName(FormFieldAppearanceState.Down);
                writer.WriteDictionaryObjectRefEntry(name, downOref);
            }
            finally
            {
                writer.EndDictionary();
                writer.EndDictionaryEntry();
            }

            
        }


        protected virtual void WriteButtonRect(PDFRenderContext context, PDFWriter writer, 
            PDFLayoutXObjectRun normal, PDFLayoutXObjectRun over, PDFLayoutXObjectRun down)
        {
            this.Location.X += this.ContainerOffset.X;
            this.Location.Y += this.ContainerOffset.Y;
            
            PDFReal left = context.Graphics.GetXPosition(Location.X);
            PDFReal top = context.Graphics.GetYPosition(Location.Y);
            PDFReal right = left + context.Graphics.GetXOffset(Size.Width);
            PDFReal bottom = top + context.Graphics.GetYOffset(Size.Height);
            
            writer.BeginDictionaryEntry("Rect");
            writer.WriteArrayRealEntries(true, left.Value, bottom.Value, right.Value, top.Value);
            writer.EndDictionaryEntry();
        }
    }
}
