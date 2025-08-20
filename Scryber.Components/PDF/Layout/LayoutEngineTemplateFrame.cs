using System;
using System.Collections.Generic;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;



namespace Scryber.PDF.Layout;


public class LayoutEngineTemplateFrame : LayoutEngineFrame
{
    
    protected PDFFile GeneratedFile { get; set; }
    
    protected PDFFile ParentFile { get; set; }

    private PDFFramesetLayoutDocument LayoutDocument { get; set; }

    public LayoutEngineTemplateFrame(LayoutEngineFrameset parent, HTMLFrame forFrame, PDFFile parentFile, PDFLayoutContext context)
        : base(parent, forFrame, context)
    {
        this.ParentFile = parentFile;
        this.LayoutDocument = (PDFFramesetLayoutDocument)context.DocumentLayout;
    }

    public override void Layout(PDFLayoutContext context, Style fullstyle)
    {
        if (this.Frame.Visible)
        {
            if (null == this.GeneratedFile)
            {
                this.GeneratedFile = this.Frame.FileReference.GetOrCreateFile(context, this.ParentFile, this.Frame, this.LayoutDocument.DocumentComponent);
            }

            if (null == this.GeneratedFile)
            {
                throw new NullReferenceException("There is no generated file to get the pages from");
            }
        }

        this.AddFramePageReferences(context, fullstyle, this.GeneratedFile);

        base.Layout(context, fullstyle);
    }
}