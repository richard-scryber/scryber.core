using System;
using Scryber.Components;
using Scryber.Modifications;
using System.Collections.Generic;

namespace Scryber.Html.Components;

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

        this.EnsureRemoteContentLoadedAndBound(root, dependants, context);
    }

    private void EnsureRemoteContentLoadedAndBound(FrameFileReference root, List<FrameFileReference> dependants, DataContext context)
    {
        root.EnsureContentLoadedAndBound(this, this.Document, context);

        foreach (var fref in dependants)
        {
            fref.EnsureContentLoadedAndBound(this, this.Document, context);
        }
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
                        fref = new FramePDFFileReference(path);
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
                        fref = new FrameTemplateFileReference(path);
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
                fref = new FrameContentTemplateReference(doc);
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