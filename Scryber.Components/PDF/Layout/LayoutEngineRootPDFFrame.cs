using System.Collections.Generic;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.PDF.Layout;


/// <summary>
/// The frame layout engine for pages from a root pdf file
/// </summary>
public class LayoutEngineRootPDFFrame : LayoutEngineFrame
{
    protected PDFFile RootFile { get; set; }

    

    public LayoutEngineRootPDFFrame(LayoutEngineFrameset parent, HTMLFrame forFrame, PDFFile rootFile,
        PDFLayoutContext context)
        : base(parent, forFrame, context)
    {
        this.RootFile = rootFile;
        
    }

    public override void Layout(PDFLayoutContext context, Style fullStyle)
    {
        if (this.Frame.Visible)
        {
            this.AddFramePageReferences(context, fullStyle, this.RootFile);
        }
    }
    
}