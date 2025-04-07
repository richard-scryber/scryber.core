using System;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Expressive.Functions.Logical;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Svg.Components;

namespace Scryber.PDF;

/// <summary>
/// Supports the output of layout content as an XObject (outside of the main page drawing stream).
/// </summary>
/// <remarks>Handles the viewport sizing, any scaling, and outputting of resources and the explicit Do action back on the main page drawing stream</remarks>
public class PDFXObjectRenderer : IDisposable, IResourceContainer
{

    
    
    /// <summary>
    /// A reference to the PDFGraphics for the parent Stream
    /// </summary>
    public PDFGraphics OriginalGraphics { get; protected set; }
    
    /// <summary>
    /// Gets the original transformation matrix used before the xObject was set up
    /// </summary>
    public PDFTransformationMatrix OriginalMatrix { get; protected set; }
    
    /// <summary>
    /// Gets the oroginal offset of the context before set up
    /// </summary>
    public Point OriginalOffset { get; protected set; }
    
    /// <summary>
    /// Gets the original size of the context before set up
    /// </summary>
    public Size OriginalSize { get; protected set; }


    /// <summary>
    /// A reference to the PDFGraphics for the xObject itself.
    /// </summary>
    public PDFGraphics XObjectGraphics { get; protected set; }

    /// <summary>
    /// Gets the transformation matrix used to render the xObject in the correct position on the page
    /// </summary>
    public PDFTransformationMatrix xObjectMatrix { get; protected set; }

    /// <summary>
    /// Gets the offset for rendering the xObject
    /// </summary>
    public Point xObjectOffset { get; protected set; }
    
    /// <summary>
    /// Gets the size for rendering the xObject
    /// </summary>
    public Size xObjectSize { get; protected set; }

    
    /// <summary>
    /// Gets the owner of the xObject
    /// </summary>
    public IComponent Owner { get; protected set; }

    /// <summary>
    /// Gets the resources used in the xObject
    /// </summary>
    public PDFResourceList Resources { get; protected set; }

    
    public PDFLayoutXObjectResource xObjectResource { get; set; }
    
    
    /// <summary>
    /// Gets the objecg reference for the xObject stream.
    /// </summary>
    public PDFObjectRef RenderReference { get; protected set; }

    /// <summary>
    /// Gets the name of the Rendered XObject to be referred to in the parent stream.
    /// </summary>
    public PDFName OutputName { get; protected set; }

    /// <summary>
    /// Gets the bounding box for the rendered xObject
    /// </summary>
    public Rect RenderBoundingBox { get; protected set; }

    
    /// <summary>
    /// Gets the layout item this xObject is rendering
    /// </summary>
    public PDFLayoutItem Layout { get; protected set; }
    
    /// <summary>
    /// Gets the layout items position options
    /// </summary>
    public PDFPositionOptions Position { get; protected set; }
    
    /// <summary>
    /// Gets the rendering context for the output
    /// </summary>
    public PDFRenderContext Context { get; protected set; }
    
    public PDFWriter Writer { get; protected set; }
    
    /// <summary>
    /// Gets or sets the filters in the stream.
    /// </summary>
    protected IStreamFilter[] Filters { get; set; }

    public PDFXObjectRenderer(IComponent owner, PDFLayoutItem forLayout, PDFPositionOptions position, PDFRenderContext context, PDFWriter writer)
    {
        this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        this.Layout = forLayout ?? throw new ArgumentNullException(nameof(forLayout));
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
        this.Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        
        if (owner is IResourceContainer rsrcContainer)
            this.Resources = rsrcContainer.Resources;
        else
            this.Resources = new PDFResourceList(this, false);
        
        this.Position = position ?? throw new ArgumentNullException(nameof(position));
        this.OutputName = (PDFName)context.Document.GetIncrementID(owner.Type);
    }

    protected virtual void SaveContextState()
    {
        this.OriginalGraphics = this.Context.Graphics;
        this.OriginalOffset = this.Context.Offset;
        this.OriginalSize = this.Context.Space;
        this.OriginalMatrix = this.Context.RenderMatrix;
    }

    protected virtual void RestoreContextState()
    {
        this.Context.Graphics = this.OriginalGraphics;
        this.Context.Offset = this.OriginalOffset;
        this.Context.Space = this.OriginalSize;
        this.Context.RenderMatrix = this.OriginalMatrix;

        this.OriginalGraphics = null;
    }

    protected virtual void RegisterSelfAsResource()
    {
        var rsrc = new PDFLayoutXObjectResource(Scryber.PDF.Resources.PDFResource.XObjectResourceType,
            this.Layout.ToString(), this);
        
        this.Layout.GetLayoutPage().PageOwner.Register(rsrc);
        
        this.Document.EnsureResource(rsrc.ResourceType, rsrc.ResourceKey, rsrc);
        this.xObjectResource = rsrc;
    }

    public void SetupGraphics(PDFPositionOptions positionOptions, Rect totalBounds)
    {
        if (null != this.XObjectGraphics)
            throw new InvalidOperationException(
                "This xObject has already been set up to render, it cannot be called twice");

        //Store the current state to be restored below
        this.SaveContextState();
        this.RegisterSelfAsResource();
        
        this.RenderReference = this.Writer.BeginObject();
        this.Filters = this.Context.Compression == OutputCompressionType.FlateDecode
            ? this.Layout.GetLayoutPage().PageCompressionFilters
            : null;
        this.Writer.BeginStream(this.RenderReference, this.Filters);
        
        
        this.XObjectGraphics = PDFGraphics.Create(this.Writer, false, this, DrawingOrigin.TopLeft, totalBounds.Size,
            this.Context);
        
        this.xObjectSize = totalBounds.Size;
        this.xObjectOffset = totalBounds.Location;
        
        var matrix = this.Context.RenderMatrix;
        
        if (null == matrix)
            matrix = PDFTransformationMatrix.Identity();
        else
            matrix = matrix.Clone();

        matrix = this.CalculateViewPortMatrix(matrix, this.Position, totalBounds.Location, totalBounds.Size);
        
        this.xObjectMatrix = matrix;
       

        this.Context.Offset = Point.Empty;
        this.Context.Space = this.xObjectSize;
        this.Context.Graphics = this.XObjectGraphics;
        this.Context.RenderMatrix = matrix;
    }


    public void ReleaseGraphics()
    {
        if (null == this.XObjectGraphics)
        {
            throw new InvalidOperationException("This xObject has not be set up, or has already been disposed.");
        }

        var len = Writer.EndStream();
        
        Writer.BeginDictionary();
        this.WriteXObjectDictionaryContent(this.Writer, len, this.Filters);
        Writer.EndDictionary();
        Writer.EndObject();
        
        //set everything back to before the renderd xObject
        this.RestoreContextState();
        
        //finally write the Do command for the rendering
        this.WriteXObjectRenderDo();

        
    }
    
    protected virtual PDFTransformationMatrix CalculateViewPortMatrix(PDFTransformationMatrix original,
        PDFPositionOptions positionOptions, Point location, Size totalSize)
    {
        if (!positionOptions.ViewPort.HasValue)
            return original;
        else
        {
            var aspect = positionOptions.ViewPortRatio;
            
            var srcWidth = positionOptions.ViewPort.Value.Width;
            var srcHeight = positionOptions.ViewPort.Value.Height;
            var destWidth = positionOptions.Width ?? srcWidth;
            var destHeight = positionOptions.Height ?? srcHeight;
            
            var newMatrix = original.Clone();
            var dest = new Size(destWidth, destHeight);
            
            if (aspect.Align == AspectRatioAlign.None)
            {
                ViewPortAspectRatio.ApplyMaxNonUniformScaling(newMatrix, new Size(destWidth, destHeight),
                    positionOptions.ViewPort.Value);
            }
            else if (aspect.Meet == AspectRatioMeet.Meet)
            {
                ViewPortAspectRatio.ApplyUniformScaling(newMatrix, new Size(destWidth, destHeight),
                    positionOptions.ViewPort.Value,
                    aspect.Align);
            }
            else //stretch
            {
                ViewPortAspectRatio.ApplyUniformStretching(newMatrix, new Size(destWidth, destHeight),
                    positionOptions.ViewPort.Value,
                    aspect.Align);
            }
            
            return newMatrix;
        }
    }

    protected Rect GetBoundingBox()
    {
        return new Rect(Point.Empty, this.xObjectSize);
    }

    protected virtual void WriteXObjectDictionaryContent(PDFWriter writer, long streamLength, IStreamFilter[] filters)
    {
        writer.WriteDictionaryNameEntry("Type", "XObject");
        writer.WriteDictionaryNameEntry("Subtype", "Form");

        if (null != this.xObjectMatrix)
        {
            writer.BeginDictionaryEntry("Matrix");
            writer.WriteArrayRealEntries(this.xObjectMatrix.Components, "#0.0000");
            writer.EndDictionaryEntry();
        }

        var bbox = this.GetBoundingBox();
        this.xObjectResource.BoundingBox = bbox;
            
        writer.BeginDictionaryEntry("BBox");
        writer.BeginArrayS();
        writer.WriteReal(bbox.X.PointsValue);
        writer.WriteRealS(bbox.Y.PointsValue);
        writer.WriteRealS(bbox.Width.PointsValue);
        writer.WriteRealS(bbox.Height.PointsValue);

        writer.EndArray();
        writer.EndDictionaryEntry();

        this.RenderBoundingBox = bbox;
            
        if (null != this.Resources)
        {
            var res = this.Resources.WriteResourceList(this.Context, writer);
            writer.WriteDictionaryObjectRefEntry("Resources", res);
        }
        else if (this.Owner is IResourceContainer rsrc)
        {
            
             var res = rsrc.Resources.WriteResourceList(this.Context, writer);
             writer.WriteDictionaryObjectRefEntry("Resources", res);
        }

        if (null != filters && filters.Length > 0)
        {
            writer.BeginDictionaryEntry("Length");
            writer.WriteNumberS(streamLength);
            writer.EndDictionaryEntry();
            writer.BeginDictionaryEntry("Filter");
            writer.BeginArray();

            foreach (IStreamFilter filter in filters)
            {
                writer.BeginArrayEntry();
                writer.WriteName(filter.FilterName);
                writer.EndArrayEntry();
            }
            writer.EndArray();
            writer.EndDictionaryEntry();
        }
        else
        {
            writer.BeginDictionaryEntry("Length");
            writer.WriteNumberS(streamLength);
            writer.EndDictionaryEntry();
        }
    }

    protected virtual void WriteXObjectRenderDo()
    {
        var moveMatrix = new PDFTransformationMatrix();
        var x = this.OriginalOffset.X.RealValue;
        var y = (this.OriginalOffset.Y + this.OriginalSize.Height).RealValue;
        x = this.Context.Graphics.GetXPosition(x);
        y =  this.Context.Graphics.GetYPosition(y);
        moveMatrix.SetTranslation(x.Value, y.Value);
        
        this.Context.Graphics.SetTransformationMatrix(moveMatrix, true, true);
        this.Context.Graphics.PaintXObject(this.OutputName);
    }
    
    
    //
    // IResourceContainer implementation
    //

    public IDocument Document
    {
        get
        {
            return this.Context.Document;
        }
    }
    
    public string Register(ISharedResource rsrc)
    {
        return this.Register((PDFResource)rsrc).Value;
    }

    public PDFName Register(PDFResource resource)
    {
        if (null == resource.Name || string.IsNullOrEmpty(resource.Name.Value))
        {
            string name = this.Document.GetIncrementID(resource.Type);
            resource.Name = (PDFName)name;
        }

        if (null == this.Resources)
            this.Resources = new PDFResourceList(this, false);
        
        resource.RegisterUse(this.Resources, this.Owner);
        return resource.Name;
    }

    public string MapPath(string source)
    {
        IResourceContainer parentRegister = this.GetParentResourceRegister();
        if (null == parentRegister)
            return source;
        else
            return parentRegister.MapPath(source);
    }

    private IResourceContainer _parentCache = null;

    protected virtual IResourceContainer GetParentResourceRegister()
    {
        if (null == _parentCache)
        {
            PDFLayoutItem parent = this.Layout.Parent;

            while (null != parent && !(parent is IResourceContainer))
            {
                parent = parent.Parent;
            }

            _parentCache = parent as IResourceContainer;
        }

        return _parentCache;
    }
    

    //
    // IDisposable implementation
    //

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (null != this.XObjectGraphics)
            {
                this.XObjectGraphics.Dispose();
            }

            if (null != this.OriginalGraphics)
            {
                this.RestoreContextState();
            }
        }
    }

    public void Dispose()
    {
        this.Dispose(true);
    }

    ~PDFXObjectRenderer()
    {
        this.Dispose(false);
    }

    
}