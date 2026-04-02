using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Manages the rendering of the SVG Content as an image
/// </summary>
public class SVGPDFImageDataRenderer : IDisposable
{
    
    #region Public Properties
    
    /// <summary>
    /// Gets the canvas for this SVG Image data that is parsed from the loaded content
    /// </summary>
    public SVGCanvas Canvas { get; private set; }

    /// <summary>
    /// Gets the block to be rendered to the PDF output
    /// </summary>
    public PDFLayoutBlock LayoutBlock { get; private set; }
    
    public PDFObjectRef LayoutBlockRef { get; private set; }
    
    public PDFName LayoutRenderName { get; private set; }
    
    /// <summary>
    /// Gets the sizer that controls the final image output size
    /// </summary>
    public SVGImageDataSizer Sizer { get; private set; }
    
    /// <summary>
    /// Gets the current context
    /// </summary>
    public ContextBase Context { get; private set; }
    
    /// <summary>
    /// Gets the writer to write to
    /// </summary>
    public PDFWriter Writer { get; private set; }
    
    /// <summary>
    /// Gets the name of the image we are writing.
    /// </summary>
    public PDFName ImageName { get; private set; }

    /// <summary>
    /// Gets the filters that should be applied when writing to a stream.
    /// </summary>
    public IStreamFilter[] StreamFilters { get; private set; }
    
    /// <summary>
    /// Gets or sets the flag that identifies if the image object has been set up (needs setting up before rendering can take place).
    /// </summary>
    public bool IsSetUp { get; private set; }
    
    /// <summary>
    /// Gets or sets the flag that identifies if the image object has been released (no more rendering to take place)
    /// </summary>
    public bool IsReleased { get; private set; }
    
    
    /// <summary>
    /// Gets the XObject reference this renderer has output to
    /// </summary>
    public PDFObjectRef ImageRef { get; private set; }
    
    #endregion
    
    //
    // .ctor(s)
    //
    
    #region Constructor(SVGCanvas, PDFLayoutBlock, SVGImageDataSizer)
    
    /// <summary>
    /// Gets the sizer that controls the bounding boxes for the rendering.
    /// </summary>
    /// <param name="canvas">The SVG canvas we are going to be rendering</param>
    /// <param name="layoutBlock">The layout block that contains all the contents of the SVG Image laid out.</param>
    /// <param name="sizer">The sizer that defines the final output sizing.</param>
    /// <exception cref="ArgumentNullException">Thrown if any of the arguments are null</exception>
    public SVGPDFImageDataRenderer(SVGCanvas canvas, PDFLayoutBlock layoutBlock, SVGImageDataSizer sizer)
    {
        Canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
        LayoutBlock =  layoutBlock ?? throw new ArgumentNullException(nameof(layoutBlock));
        Sizer =  sizer ?? throw new ArgumentNullException(nameof(sizer));
    }
    
    #endregion

    //
    // implementation
    //

    #region SetUpImage + DoSetUpImage
    
    /// <summary>
    /// Initializes the writer and begins a new object stream to render the content into.
    /// </summary>
    /// <param name="imageName">The name of the image (resource name) that we are setting up. Required</param>
    /// <param name="filters">Any filters that should be applied to the object stream when rendering</param>
    /// <param name="writer">The writer we are going to output to. Required</param>
    /// <param name="context">A default context base to log to. Required</param>
    /// <returns>The object reference that has been set up to begin writing to.</returns>
    /// <exception cref="ArgumentNullException">Thrown if one of the required parameters are null (imageName, writer and context)</exception>
    /// <exception cref="InvalidOperationException">Thrown if the instance state is not correct - already set up, or already released.</exception>
    public PDFObjectRef SetUpImage(PDFName imageName, IStreamFilter[] filters, PDFWriter writer, ContextBase context)
    {
        if (imageName is null)
            throw new ArgumentNullException(nameof(imageName));
        if (writer is null)
            throw new ArgumentNullException(nameof(writer));
        if (context is null)
            throw new ArgumentNullException(nameof(context));
        
        
        if(this.IsSetUp)
            throw new InvalidOperationException("The renderer has already been initialized.");
        
        if(this.IsReleased)
            throw new InvalidOperationException("The renderer has already been released. Cannot set up again with the same instance.");
        
        this.ImageName = imageName;
        this.Writer = writer;
        this.Context = context;
        this.StreamFilters = filters;

        this.ImageRef = DoSetUpImage(filters);
        
        this.IsSetUp = (null != this.ImageRef);
        
        return this.ImageRef;

    }

    protected virtual PDFObjectRef DoSetUpImage(IStreamFilter[] filters)
    {
        var oref = this.Writer.BeginObject(this.ImageName.Value);
        return oref;
    }
    
    #endregion


    #region ReleaseImage + DoReleaseImage
    
    /// <summary>
    /// Releases the image rendering from the writer
    /// </summary>
    /// <param name="imageName">THe name of the image the renderer is releasing.</param>
    /// <returns>The original object reference for the image that was rendered</returns>
    /// <exception cref="ArgumentException">Thrown if the image name does not match the name originally provided to the image set up.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the renderer has not yet been set up, or it has already been released.</exception>
    public PDFObjectRef ReleaseImage(PDFName imageName)
    {
        if(!this.IsSetUp)
            throw new InvalidOperationException("The renderer has not been initialized.");
        
        if(imageName.Value != this.ImageName.Value)
            throw new ArgumentException(@"The given image name is not the expected image name from set up.", nameof(imageName));
        
        if(this.IsReleased)
            throw new InvalidOperationException("The renderer has already been released.");
        
        
        this.DoReleaseImage();
        this.IsReleased = true;
        
        return this.ImageRef;
    }

    protected virtual void DoReleaseImage()
    {
        this.Writer.EndObject();
    }
    
    #endregion


    public void OutputToPDF(PDFRenderContext context)
    {
        if(null == context)
            throw new ArgumentNullException(nameof(context));
        
        var prevOffset = context.Offset;
        
        if(!this.IsSetUp)
            throw new InvalidOperationException("The renderer has not been initialized.");
        if(this.IsReleased)
            throw new InvalidOperationException("The renderer has already been released.");
        
        bool startedStream = false;
        Rect origBounds = this.LayoutBlock.TotalBounds;
        
        try
        {
            context.Offset = Point.Empty;
            
            this.Writer.BeginStream(this.ImageRef, this.StreamFilters);
            startedStream = true;
            
            this.LayoutBlock.PagePosition = Point.Empty;
            var bounds = this.LayoutBlock.TotalBounds;
            bounds.Location = Point.Empty;
            this.LayoutBlock.TotalBounds = bounds;
            this.LayoutBlock.IsInlineContent = false;

            if (this.LayoutBlockRef == null)
            {
                this.DoOutputLayoutToPDF(this.LayoutBlock, context, this.Writer);
                this.DoOutputImageDraw(this.ImageName, this.LayoutRenderName, context, this.Writer);
            }
            var len = this.Writer.EndStream();
            startedStream = false;
            
            
            this.DoOutputImageXObjectDictionary(len, this.ImageName, this.LayoutRenderName, this.LayoutBlockRef, this.StreamFilters, context, this.Writer);

        }
        catch (Exception e)
        {
            throw;
        }
        finally
        {
            if(startedStream)
                this.Writer.EndStream();
            
            this.LayoutBlock.TotalBounds = origBounds;
            context.Offset = prevOffset;
        }
    }


    /// <summary>
    /// Will output the contents of the layoutBlock, and then return the shared resource name of the XObject that the layout rendered.
    /// </summary>
    /// <param name="layoutBlock">The block to render</param>
    /// <param name="context">The current render context</param>
    /// <param name="writer">The writer to output to.</param>
    /// <returns>The name of the XObject that was output.</returns>
    protected virtual PDFName DoOutputLayoutToPDF(PDFLayoutBlock layoutBlock, PDFRenderContext context, PDFWriter writer)
    {
        this.LayoutBlockRef = layoutBlock.OutputToPDF(context, writer);
        this.LayoutRenderName = AssertGetCanvasRenderName(this.Canvas, context);
        
        return this.LayoutRenderName;
    }

    /// <summary>
    /// Looks for the XObjectRenderer that is associated with the Canvas in the document shared resources, and returns it's name.
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown if the XObjectRenderer cannot be found.</exception>
    private PDFName AssertGetCanvasRenderName(SVGCanvas canvas, PDFRenderContext context)
    {
        var doc = (Document)context.Document;
        PDFName canvasName = null;

        foreach (var rsrc in doc.SharedResources)
        {
            if (rsrc is PDFLayoutXObjectResource xobj)
            {
                var renderer = xobj.Renderer;
                if (renderer != null && renderer.Owner == canvas)
                {
                    canvasName = renderer.OutputName;
                    break;
                }
            }
        }
        
        if(null == canvasName)
            throw new InvalidOperationException("No resource could be found for the LayoutXObject that matches the referenced image SVG canvas for " + canvas.ID);
        
        return canvasName;
    }

    /// <summary>
    /// Outputs the instructions in the Image XObject to draw the actual SVG Convas contents that have previously been rendered and associated with the layoutName.
    /// Also outputs any required transformations so the Canvas XObject will appear correctly.
    /// </summary>
    /// <param name="imageName"></param>
    /// <param name="layoutName"></param>
    /// <param name="context"></param>
    /// <param name="writer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected virtual void DoOutputImageDraw(PDFName imageName, PDFName layoutName,
        PDFRenderContext context, PDFWriter writer)
    {
        if(null == layoutName)
            throw new ArgumentNullException(nameof(layoutName));
        
        var matrix = this.Sizer.GetCanvasToImageMatrix(context);
        if (null != matrix && !matrix.IsIdentity)
        {
            var components = matrix.Components;
            for (var i = 0; i < components.Length; i++)
            {
                writer.WriteRealS(components[i]);
            }
        
            writer.WriteOpCodeS(PDFOpCode.GraphTransformMatrix);
        
            for (var i = 0; i < components.Length; i++)
            {
                writer.WriteRealS(components[i]);
            }
        
            writer.WriteOpCodeS(PDFOpCode.TxtTransformMatrix);
        }
        
        writer.WriteNameS(layoutName.Value);
        writer.WriteOpCodeS(PDFOpCode.XobjPaint);
    }


    /// <summary>
    /// Outputs the associated XObject content dictionary to the current object (that has the instructions to render the SVGCanvas layout contents.
    /// </summary>
    /// <param name="streamLength">The length of the xObject stream for the image (that renders the matrix and xObject paint for the canvas.</param>
    /// <param name="layoutName">The name of this XObject</param>
    /// <param name="layoutRenderName">The name of the associated layout XObject</param>
    /// <param name="layoutRenderRef">Any filters to apply to a stream</param>
    /// <param name="context">The current render context</param>
    /// <param name="writer">The current pdf writer to write the dictionary to.</param>
    protected virtual void DoOutputImageXObjectDictionary(long streamLength, PDFName layoutName, PDFName layoutRenderName, PDFObjectRef layoutRenderRef, IStreamFilter[] filters, PDFRenderContext context, PDFWriter writer)
    {
        writer.BeginDictionary();
        writer.WriteDictionaryNameEntry("Type", "XObject");
        writer.WriteDictionaryNameEntry("Subtype", "Form");
        
        //Write the resources (TODO: Add the Procset for convention)
        writer.BeginDictionaryEntry("Resources");

            writer.BeginDictionary();
            writer.BeginDictionaryEntry("XObject");

            writer.BeginDictionary();
            writer.WriteDictionaryObjectRefEntry(layoutRenderName.Value, layoutRenderRef);

            writer.EndDictionary();
            writer.EndDictionaryEntry();

            writer.EndDictionary(); //Resource
            
        writer.EndDictionaryEntry();
        
        var bbox = this.Sizer.GetImageToCanvasBBox(context);
        writer.BeginDictionaryEntry("BBox");
        writer.BeginArray();
        writer.WriteRealS(bbox.X.PointsValue);
        writer.WriteRealS(bbox.Y.PointsValue);
        writer.WriteRealS(bbox.Width.PointsValue);
        writer.WriteRealS(bbox.Height.PointsValue);
        writer.EndArray();
        writer.EndDictionaryEntry();

        if (null != filters && filters.Length > 0)
        {
            writer.BeginDictionaryEntry("Filter");
            writer.BeginArray();

            foreach (var filter in filters)
            {
                writer.BeginArrayEntry();
                writer.WriteName(filter.FilterName);
                writer.EndArrayEntry();
            }
            
            writer.EndArray();
            writer.EndDictionaryEntry();
        }
        
        writer.BeginDictionaryEntry("Length");
        writer.WriteNumberS(streamLength);
        writer.EndDictionaryEntry();
        
        
        writer.EndDictionary();
        
    }
    
    
    #region IDisposable Support

    public void Dispose()
    {
        this.Dispose(true);
    }

    ~SVGPDFImageDataRenderer()
    {
        this.Dispose(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this.IsSetUp)
            {
                if (!this.IsReleased)
                {
                    this.Writer.EndObject();
                }
            }
        }
    }
    
    #endregion
    
}