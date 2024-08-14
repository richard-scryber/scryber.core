using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.PDF.Native;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.PDF
{
    public class PDFAttachmentAnnotationEntry : PDFAnnotationEntry
    {

        public Components.IconAttachment Attachment { get; private set; }

        public Styles.Style AnnotationStyle { get; private set; }

        public PDFEmbeddedAttachment AttachmentFileSpec { get; private set; }

        public Rect IconContentBounds { get; set; }

        public Rect IconBorderBounds { get; set; }

        public PDFAttachmentAnnotationEntry(Components.IconAttachment attach, PDFEmbeddedAttachment filespec, Styles.Style style)
        {
            if (null == attach)
                throw new ArgumentNullException("attach");

            this.Attachment = attach;
            this.AnnotationStyle = style;
            this.AttachmentFileSpec = filespec;
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

            Rect rect = this.IconContentBounds;
            Rect containerBounds = this.Attachment.GetFirstArrangement().RenderBounds; //This is the absolute bounds of the border rectangle to render in.
            containerBounds.X += (IconContentBounds.X - IconBorderBounds.X); //Add the x offset of the content in the border
            containerBounds.Y += (IconContentBounds.X - IconBorderBounds.X); //Add the x offset of the content in the border
            //containerBounds.Width = rect.Width;
            //containerBounds.Height = rect.Height;
            rect = containerBounds;

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

            writer.BeginDictionaryEntry("Rect");
            writer.WriteArrayRealEntries(rect.X.Value, rect.Y.Value, rect.Width.Value, rect.Height.Value);
            writer.EndDictionaryEntry();


            string name = this.Attachment.UniqueID + "FileLink";
            writer.WriteDictionaryStringEntry("NM", name);

            PDFObjectRef fsRef = this.AttachmentFileSpec.OutputToPDF(context, writer);
            writer.WriteDictionaryObjectRefEntry("FS", fsRef);

            AttachmentDisplayIcon icon = this.AnnotationStyle.GetValue(StyleKeys.AttachmentDisplayIconKey, AttachmentDisplayIcon.None);
            if (icon != AttachmentDisplayIcon.None)
                writer.WriteDictionaryNameEntry("Name", icon.ToString());

            writer.EndDictionary();
            writer.EndObject();

            return new PDFObjectRef[] { annotRef };

        }
    }
}
