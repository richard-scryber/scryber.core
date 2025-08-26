using System;
using Scryber.Components;
using Scryber.Modifications;
using Scryber.PDF.Native;

using System.Collections.Generic;

namespace Scryber.Html.Components;

/// <summary>
/// A frameset of other content (referenced within a frame) that will be rendered into a single file.
/// </summary>
/// <remarks>The frameset can only reference zero or 1 existing PDF files (although that PDF file can be referenced multiple times). </remarks>
[PDFParsableComponent("frameset")]
public class HTMLFrameset : ContainerComponent
{
    
    private HTMLFrameList _frames;

    [PDFElement("")]
    [PDFArray(typeof(HTMLFrame))]
    public HTMLFrameList Frames
    {
        get
        {
            if (null == _frames)
                _frames = CreateFrameList();
            return _frames;
        }
        set
        {
            this._frames = value;
        }
    }

    

    /// <summary>
    /// Gets the root file or template reference, that will form the first document in the modification chain.
    /// </summary>
    public FrameFileReference RootReference
    {
        get;
        protected set;
    }

    /// <summary>
    /// Gets the list of further templates, that will be used in the modification chain.
    /// </summary>
    public List<FrameFileReference> DependantReferences
    {
        get;
        protected set;
    }

    /// <summary>
    /// Gets or sets the current PDF File being appened to and generated.
    /// </summary>
    public PDFFile CurrentFile
    {
        get;
        set;
    }
    
    public HTMLFrameset() : this(ObjectTypes.ModifyFrameSet)
    {}
    
    protected HTMLFrameset(ObjectType type)
        : base(type)
    {}
    
    
    protected virtual HTMLFrameList CreateFrameList()
    {
        return new HTMLFrameList(this.InnerContent);
    }


    protected override void OnDataBound(DataContext context)
    {
        base.OnDataBound(context);

        FrameFileReference root;
        List<FrameFileReference> dependants = ExtractRootAndDependantFrames(this.Frames, out root);

        if (null == root)
            throw new InvalidOperationException(
                "There was no root document found to begin creating the frameset from.");
        
        
        this.EnsureRemoteContent(root, dependants, context);

        this.RootReference = root;
        this.DependantReferences = dependants;
    }

    private void EnsureRemoteContent(FrameFileReference root, List<FrameFileReference> dependants, DataContext context)
    {
        root.EnsureContent(this, this.Document, this.CurrentFile, context);
        this.CurrentFile = root.FrameFile;
        
        foreach (var fref in dependants)
        {
            if (fref.EnsureContent(this, this.Document, this.CurrentFile, context))
                this.CurrentFile = fref.FrameFile;
        }

        //Store the current file in the top level document, so it is used for the layout.
        this.Document.PrependedFile = this.CurrentFile;
    }

    /// <summary>
    /// For all the frames within this set, extracts all the individual template
    /// references setting the root to the original pdf reference or the first template
    /// </summary>
    /// <param name="list">The list of frames within this set.</param>
    /// <param name="root">Set to the base pdf or the first template file</param>
    /// <returns>A list of all the references, not including the root</returns>
    /// <exception cref="InvalidOperationException">Thrown if there is more than one base pdf file.</exception>
    protected virtual List<FrameFileReference> ExtractRootAndDependantFrames(HTMLFrameList list,
        out FrameFileReference root)
    {
        root = null;
        List<FrameFileReference> refs = new List<FrameFileReference>();

        foreach (var frame in this.Frames)
        {
            
            if(frame.Visible == false)
                continue;
            
            FrameFileReference fref;
            FrameFileType fileType;
            
            if (!string.IsNullOrEmpty(frame.RemoteSource))
            {
                string path = frame.MapPath(frame.RemoteSource);

                MimeType type = frame.RemoteSourceMimeType;
                if (null == type)
                {
                    if (path.EndsWith(".pdf"))
                    {
                        type = MimeType.Pdf;
                    }
                    else
                    {
                        type = MimeType.Html;
                    }
                }

                if (type.Equals(MimeType.Pdf))
                {
                    if (null == root)
                    {
                        fref = new FramePDFFileReference(frame, path);
                        root = fref;
                    }
                    else if (root.FullPath == path)
                    {
                        fref = root;
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "There CANNOT be more than one pdf referenced file within a frameset.");
                    }

                    frame.FileReference = fref;
                }
                else
                {
                    var exist = refs.Find(one => {
                                                            if (one.FullPath.Equals(path)) return true;
                                                            else return false;
                                                        });
                    if (null == exist)
                    {
                        fref = new FrameTemplateFileReference(frame, path);
                        refs.Add(fref);
                    }
                    else
                    {
                        fref = exist;
                    }

                    frame.FileReference = fref;
                }
            }
            else if(null != frame.InnerHtml)
            {
                var doc = frame.InnerHtml;
                fref = new FrameContentTemplateReference(frame, doc);
                refs.Add(fref);

                frame.FileReference = fref;
            }
        }


        if (null == root)
        {
            root = refs[0];
            refs.RemoveAt(0);
        }

        return refs;
    }
}