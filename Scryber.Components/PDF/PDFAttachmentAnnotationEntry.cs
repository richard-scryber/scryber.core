using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.PDF.Native;
using Scryber.Drawing;
using Scryber.Styles;
using SixLabors.ImageSharp.Advanced;

namespace Scryber.PDF
{
    public class PDFAttachmentAnnotationEntry : PDFAnnotationEntry
    {

        public Components.IconAttachment Attachment { get; private set; }

        public Styles.Style AnnotationStyle { get; private set; }

        public PDFEmbeddedAttachment AttachmentFileSpec { get; private set; }

        public Components.Component LinkedFrom { get; private set; }

        public PDFAttachmentAnnotationEntry(Components.IconAttachment attach, Components.Component linkedFrom, PDFEmbeddedAttachment filespec, Styles.Style style)
        {
            if (null == attach)
                throw new ArgumentNullException("attach");

            this.Attachment = attach;
            this.AnnotationStyle = style;
            this.AttachmentFileSpec = filespec;
            this.LinkedFrom = linkedFrom;
        }

        protected override IEnumerable<PDFObjectRef> DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            int pgindex = context.PageIndex;

            PDFObjectRef annotRef = writer.BeginObject();
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Annot");
            writer.WriteDictionaryNameEntry("Subtype", "FileAttachment");
            if (!string.IsNullOrEmpty(this.AlternateText))
                writer.WriteDictionaryStringEntry("Contents", this.AlternateText);
            else if (!string.IsNullOrEmpty(this.Attachment.Description))
                writer.WriteDictionaryStringEntry("Contents", this.Attachment.Description);

            AttachmentDisplayIcon icon = this.AnnotationStyle.GetValue(StyleKeys.AttachmentDisplayIconKey, AttachmentDisplayIcon.None);
            var arrange = this.LinkedFrom?.GetFirstArrangement();
            
            Rect rect = Rect.Empty;

            if (null != arrange)
            {

                rect = this.GetIconBounds(arrange.RenderBounds, arrange.FullStyle);

                //Convert to PDF Values
                if (context.DrawingOrigin == DrawingOrigin.TopLeft)
                {
                    //PDFs have origin at bottom so need to convert.
                    PDFReal value = context.Graphics.GetXPosition(rect.X.RealValue);
                    rect.X = new Unit(value.Value, PageUnits.Points);
                    value = context.Graphics.GetYPosition(rect.Y.RealValue);
                    rect.Y = new Unit(value.Value, PageUnits.Points);
                    rect.Width = rect.X + rect.Width;
                    rect.Height = rect.Y - rect.Height;
                }
                else
                {
                    rect.Width = rect.X + rect.Width;
                    rect.Height = rect.Y + rect.Height;
                }



            }

            writer.BeginDictionaryEntry("Rect");
            writer.WriteArrayRealEntries(rect.X.Value, rect.Y.Value, rect.Width.Value, rect.Height.Value);
            writer.EndDictionaryEntry();
            

            string name = this.Attachment.UniqueID + "FileLink";
            writer.WriteDictionaryStringEntry("NM", name);

            PDFObjectRef fsRef = this.AttachmentFileSpec.OutputToPDF(context, writer);
            writer.WriteDictionaryObjectRefEntry("FS", fsRef);

            
            if (icon != AttachmentDisplayIcon.None)
                writer.WriteDictionaryNameEntry("Name", icon.ToString());
            else
            {
                var empty = writer.BeginObject();
                writer.BeginStream(empty);
                writer.WriteComment("Empty Appearance Stream for the None link");
                var len = writer.EndStream();
                writer.BeginDictionary();
                
                writer.WriteDictionaryNameEntry("Type", "XObject");
                
                writer.WriteDictionaryNameEntry("Subtype", "Form");
                
                writer.WriteDictionaryNumberEntry("FormType", 1);
                
                writer.BeginDictionaryEntry("BBox");
                writer.WriteArrayRealEntries(true, new [] { 0,0, rect.Width.PointsValue, rect.Height.PointsValue } );
                writer.EndDictionaryEntry();
                
                writer.BeginDictionaryEntry("Matrix");
                writer.WriteArrayRealEntries(true, new [] { 1,0, 0, 1, rect.X.PointsValue, rect.Y.PointsValue } );
                writer.EndDictionaryEntry();
                
                writer.WriteDictionaryNumberEntry("Length", len);
                
                writer.EndDictionary();
                writer.EndObject();
                writer.BeginDictionaryEntry("AP");
                writer.BeginDictionary();
                writer.WriteDictionaryObjectRefEntry("N", empty);
                writer.EndDictionary();
                writer.EndDictionaryEntry();
            }
            
            writer.EndDictionary();
            writer.EndObject();
            

            return new PDFObjectRef[] { annotRef };

        }

        protected virtual Rect GetIconBounds(Rect arrangeBounds, Style style)
        {
            var pos = style.CreatePostionOptions(false);
            if (pos.DisplayMode == DisplayMode.Inline)
            {
                if (style.TryGetValue(StyleKeys.PaddingInlineStart, out var startP))
                {
                    arrangeBounds.Width -= startP.Value(style);
                    arrangeBounds.X += startP.Value(style);
                }

                if (style.TryGetValue(StyleKeys.PaddingInlineEnd, out var endP))
                {
                    arrangeBounds.Width -= endP.Value(style);
                }
            }
            else if (pos.DisplayMode == DisplayMode.InlineBlock)
            {
                if (pos.Padding.IsEmpty == false)
                {
                    arrangeBounds.X += pos.Padding.Left;
                    arrangeBounds.Y += pos.Padding.Top;
                    arrangeBounds.Width -= pos.Padding.Right + pos.Padding.Left;
                    arrangeBounds.Height -= pos.Padding.Bottom + pos.Padding.Top;
                }

                if (pos.Margins.IsEmpty == false)
                {
                    
                }
            }
            else if (pos.DisplayMode == DisplayMode.Block)
            {
                
            }
            
            
            return arrangeBounds;
        }
    }
}
