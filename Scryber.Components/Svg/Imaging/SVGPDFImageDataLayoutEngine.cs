using System;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Class that performs the actual layout of the content within a discreet SVGImage
/// </summary>
public class SVGPDFImageDataLayoutEngine
{

    public PDFLayoutBlock TryLayoutCanvas(SVGCanvas canvas, SVGImageDataSizer sizer, LayoutContext context, Style appliedstyle)
    {
        PDFLayoutBlock result = null;
        
        PDFLayoutContext pdfContext = context as PDFLayoutContext;

        if (pdfContext == null)
        {
            context.TraceLog.Add(TraceLevel.Error, "SVG Image", "The layout context was not a PDF layout context. Need the PDFContext to layout an SVG Canvas");
            return null;
        }

        var prevStyleStack = context.StyleStack;
        var prevItems = context.Items;
        var closeContainerAfter = false;
        var pg = pdfContext.DocumentLayout.CurrentPage;

        try
        {
            
            var open = pg.LastOpenBlock();

            if (open == null)
            {
                //Document layout is complete and done - special case for SVG images as backgrounds
                pg.ContentBlock.ReOpen();
                open = pg.ContentBlock;
                closeContainerAfter = true;
            }

            var baseStyle = SVGCanvas.GetDefaultBaseStyle();
            context.StyleStack = new StyleStack(baseStyle);
            context.Items = new ItemCollection(canvas);

            //We can now safely perform the sizing and layout.
            this.DoLayoutCanvas(open, canvas, sizer, context);
        }
        catch (Exception e)
        {
            if(context.Conformance == ParserConformanceMode.Strict)
                throw new PDFLayoutException("Could not layout the SVG Image contents. See the inner exception for details.", e);
            else
            {
                context.TraceLog.Add(TraceLevel.Error, "SVG Image", "The layout of the SVG Image failed : " + e.Message, e);
                result = null;
            }
        }
        finally
        {
            //restore the stack and the items
            pdfContext.StyleStack = prevStyleStack;
            pdfContext.Items = prevItems;
            
            //close any block we re-opened
            
            if(closeContainerAfter)
                pg.ContentBlock.Close();
            
        }
        
        return result;
    }

    protected virtual void DoLayoutCanvas(PDFLayoutBlock intoBlock, SVGCanvas canvas, SVGImageDataSizer sizer,
        LayoutContext context)
    {
        
    }
}