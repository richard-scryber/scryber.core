using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.PDF.Native;

namespace Scryber.PDF.Layout
{
    public class PDFLayoutMarkedContentBegin : PDFLayoutRun
    {
        public PDFMarkedContentType ContentType { get; private set; }

        public override PDFUnit Height { get { return PDFUnit.Zero; } }

        public override PDFUnit Width { get { return PDFUnit.Zero; } }

        public PDFLayoutMarkedContentBegin(PDFLayoutLine line, IComponent owner, PDFMarkedContentType type)
            : base(line, owner)
        {
            this.ContentType = type;
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)
        {
            
        }

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            PDFName name = GetNameForContentType(this.ContentType);
            context.Graphics.BeginMarkedContent(name);
            return base.DoOutputToPDF(context, writer);
        }

        public static readonly PDFName TextMarkedContent = new PDFName("Tx");

        public static PDFName GetNameForContentType(PDFMarkedContentType contentType)
        {
            switch (contentType)
            {
                case PDFMarkedContentType.Text:
                    return TextMarkedContent;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contentType));
            }
        }
    }

    public class PDFLayoutMarkedContentEnd : PDFLayoutRun
    {
        public override PDFUnit Height { get { return PDFUnit.Zero; } }

        public override PDFUnit Width { get { return PDFUnit.Zero; } }

        public PDFLayoutMarkedContentBegin Begin { get; private set; }

        public PDFLayoutMarkedContentEnd(PDFLayoutMarkedContentBegin start)
            : base(start.Line, start.Owner)
        {
            this.Begin = start;
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)
        {

        }

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            PDFName name = PDFLayoutMarkedContentBegin.GetNameForContentType(this.Begin.ContentType);
            context.Graphics.EndMarkedContent(name);
            return base.DoOutputToPDF(context, writer);
        }
    }

    public enum PDFMarkedContentType
    {
        Text
    }
}
