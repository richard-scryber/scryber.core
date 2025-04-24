using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Logging;
using Scryber.PDF.Resources;

namespace Scryber.Components
{
    /// <summary>
    /// Represents a single file attachment in a document
    /// </summary>
    [PDFParsableComponent("IconAttachment")]
    public class IconAttachment : VisualComponent, IPDFRenderComponent, IPDFViewPortComponent
    {
        public static readonly ObjectType AttachmentType = (ObjectType)"Atch";
        public const string AttachmentAnotationEntryArtefact = "AttachmentAnnotationEntry";
        //
        // properties
        //

        #region public string Source { get;set;}

        private string _src;

        /// <summary>
        /// Gets or sets the source path to the attachment
        /// </summary>
        [PDFAttribute("src")]
        public virtual string Source
        {
            get { return _src; }
            set
            {
                if (value != _src)
                {
                    this.ResetAttachmentData();
                    _src = value;
                }
            }
        }

        #endregion

        #region public PDFEmbeddedFileData Data {get;set;}

        private PDFEmbeddedFileData _data;

        /// <summary>
        /// Gets or sets the embedded file data
        /// </summary>
        [PDFAttribute("file-data")]
        public virtual PDFEmbeddedFileData Data
        {
            get { return _data; }
            set
            {
                _data = value;
                this.ResetAttachmentData();
                _src = (null == _data) ? string.Empty : _data.FullName;
            }
        }

        #endregion

        #region public string Description {get;set;}

        /// <summary>
        /// Gets or sets the description for the attachment
        /// </summary>
        [PDFAttribute("desc")]
        public virtual string Description
        {
            get;
            set;
        }

        #endregion

        #region public AttachmentDisplayIcon DisplayIcon {get;set;}

        [PDFAttribute("icon")]
        public virtual AttachmentDisplayIcon DisplayIcon
        {
            get { return this.Style.GetValue(StyleKeys.AttachmentDisplayIconKey, AttachmentDisplayIcon.PushPin); }
            set { this.Style.SetValue(StyleKeys.AttachmentDisplayIconKey, value); }
        }

        #endregion

        #region public bool NeverCache {get;set;}

        /// <summary>
        /// If set to true then the attachment data file will always be loaded from it's source, and never cached.
        /// </summary>
        [PDFAttribute("never-cache")]
        public bool NeverCache
        {
            get;
            set;
        }

        #endregion

        #region protected PDFEmbeddedAttachment Attachment {get;set;}

        /// <summary>
        /// Gets or sets the actual attachment instance that can be added to the document
        /// </summary>
        protected PDFEmbeddedAttachment Attachment
        {
            get;
            set;
        }

        #endregion

        #region protected PDFAttachmentAnnotationEntry Annotation { get; set; }

        /// <summary>
        /// Gets or sets the annotation associated with this icon attachment.
        /// </summary>
        protected PDFAttachmentAnnotationEntry Annotation { get; set; }

        #endregion

        /// <summary>
        /// Gets or set the remote request for the attachment
        /// </summary>
        protected RemoteFileRequest Request { get; set; }
        
        //
        // .ctor(s)
        //

        #region public IconAttachment()

        public IconAttachment()
            : this(AttachmentType)
        {

        }

        #endregion

        #region protected IconAttachment(ObjectType type)

        /// <summary>
        /// Protected constructor for inheriting types to call
        /// </summary>
        /// <param name="type"></param>
        protected IconAttachment(ObjectType type)
            : base(type)
        {

        }

        #endregion

        //
        // methods
        //

        #region protected virtual void ResetAttachmentData()

        /// <summary>
        /// Clears any locally held information relating to the current attachment
        /// </summary>
        protected virtual void ResetAttachmentData()
        {
            this.Attachment = null;
        }

        #endregion


        protected override void DoDataBind(DataContext context, bool includeChildren)
        {
            base.DoDataBind(context, includeChildren);
            
            if (ShouldLoadAttachment(context))
            {
                this.Request = this.RegisterLoadAttachment(this.Source, context);
            }
        }

        protected override void DoLoad(LoadContext context)
        {
            base.DoLoad(context);
            if (ShouldLoadAttachment(context))
            {
                this.Request = this.RegisterLoadAttachment(this.Source, context);
            }
        }

        protected virtual bool ShouldLoadAttachment(ContextBase context)
        {
            if(null != this.Data)
                return false;
            if(null != this.Request)
                return false;
            if (string.IsNullOrEmpty(this.Source))
            {
                return false;
            }

            return true;
        }

        protected virtual RemoteFileRequest RegisterLoadAttachment(string path, ContextBase context)
        {
            
            var config = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            var cache = TimeSpan.FromMinutes(config.ImagingOptions.ImageCacheDuration);
            
            
            this.Attachment = this.GetAttachment(context);
            
            
            RemoteRequestCallback callback = new RemoteRequestCallback(this.RemoteAttachmentCallback);
            
            var request = this.Document.RegisterRemoteFileRequest(PDFResource.AttachmentFileSpecType, this.Attachment.FullFilePath, cache, callback,this, null);
            
            return request;
        }

        private bool RemoteAttachmentCallback(IComponent owner, IRemoteRequest request, System.IO.Stream data)
        {
            if (request.IsSuccessful && request.Result != null && request.Result is PDFEmbeddedFileData cached)
            {
                this.Attachment.FileData = cached;
                this.Data = this.Attachment.FileData;
                return true;
            }
            
            if (null == data || data.CanRead == false)
            {
                request.CompleteRequest(null, false, new Exception("No Stream data to read"));
                return false;
            }
            
            //Create the attachment from the stream.
            
            var bin = new byte[data.Length];
            var len = data.Read(bin, 0, bin.Length);
            
            if (len < bin.Length)
            {
                throw new Exception("Invalid data length");
            }
            var file = new PDFEmbeddedFileData();
            file.FileData = bin;
            file.FullName = request.FilePath;
            file.Filters = null;
            file.DataLength = bin.Length;
            this._data = file;

            if(null != this.Attachment)
                this.Attachment.FileData = file;
 
            request.CompleteRequest(this.Data, true, null);
            
            return true;
            
        }


        #region protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, Styles.PDFStyle fullstyle)

        /// <summary>
        /// Overrides the base implmentation to register any artefacts with the document - in this case the attachment if defined
        /// </summary>
        /// <param name="context"></param>
        /// <param name="set"></param>
        /// <param name="fullstyle"></param>
        protected override void DoRegisterArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet set, Styles.Style fullstyle)
        {
            if (this.Visible)
            {
                PDFEmbeddedAttachment attach = this.GetAttachment(context);
                var icon = fullstyle.GetValue(StyleKeys.AttachmentDisplayIconKey, AttachmentDisplayIcon.PushPin);
                
                if (null != attach && icon != AttachmentDisplayIcon.None)
                {

                    this.Annotation = new PDFAttachmentAnnotationEntry(this, this, attach, fullstyle);
                    
                    if(!string.IsNullOrEmpty(this.OutlineTitle))
                        this.Annotation.AlternateText = this.OutlineTitle;
                    
                    Scryber.PDF.Layout.PDFLayoutPage pg = context.DocumentLayout.CurrentPage;
                    
                    object annotEntry = pg.RegisterPageEntry(context, PDFArtefactTypes.Annotations, this.Annotation);
                    set.SetArtefact(AttachmentAnotationEntryArtefact, annotEntry);

                    context.DocumentLayout.RegisterCatalogEntry(context, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory,  attach);
                }
                else if(null == attach)
                {
                    context.TraceLog.Add(TraceLevel.Warning, "Attachment", "No attachment was able to loaded for " + this.ID);
                }
            }
            else
            {
                context.TraceLog.Add(TraceLevel.Verbose, "Attachment", "The component " + this.ID + " is set to hidden so no attachment is being added to the document." + this.ID);
            }
            base.DoRegisterArtefacts(context, set, fullstyle);
        }

        #endregion

        #region protected override void DoCloseLayoutArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet artefacts, Styles.PDFStyle fullstyle)

        /// <summary>
        /// Overrides the base implementation to unregister any artefacts so the attachment can be closed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="artefacts"></param>
        /// <param name="fullstyle"></param>
        protected override void DoCloseLayoutArtefacts(PDFLayoutContext context, PDFArtefactRegistrationSet artefacts, Styles.Style fullstyle)
        {
            base.DoCloseLayoutArtefacts(context, artefacts, fullstyle);

            if (this.Visible)
            {
                object obj;

                if (artefacts.TryGetArtefact(AttachmentAnotationEntryArtefact, out obj))
                {
                    Scryber.PDF.Layout.PDFLayoutPage pg = context.DocumentLayout.CurrentPage;
                    pg.CloseArtefactEntry(PDFArtefactTypes.Annotations, obj);
                }
            }
        }

        #endregion

        #region protected virtual PDFEmbeddedAttachment GetAttachment(PDFLayoutContext context, Styles.PDFStyle fullstyle)

        /// <summary>
        /// Gets the PDFEmbeddedAttachment associated with this Component. If 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public virtual PDFEmbeddedAttachment GetAttachment(ContextBase context)
        {
            if (null == this.Attachment)
            {
                PDFResource found = null;
                var path = this.Source;

                if (!string.IsNullOrEmpty(path))
                {
                    path = this.MapPath(path);

                    found = this.Document.GetResource(PDFResource.AttachmentFileSpecType, this, path, false);
                }

                if (null != found && found is PDFEmbeddedAttachment attachment)
                {
                    this.Attachment = attachment;
                }
                else
                {
                    var log = context.TraceLog;

                    if (log.ShouldLog(TraceLevel.Verbose))
                        log.Begin(TraceLevel.Verbose, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory,
                            "Getting Attachment for component " + this.UniqueID);

                    this.Attachment = CreateAttachment(log, path);
                    if (this.Attachment != null)
                        this.Document.SharedResources.Add(this.Attachment);
                }
            }
            return this.Attachment;
        }

        #endregion

        #region private PDFEmbeddedAttachment LoadAttachment(PDFLayoutContext context, Styles.PDFStyle fullstyle)

        private PDFEmbeddedAttachment CreateAttachment(TraceLog log, string fullpath)
        {

            PDFEmbeddedAttachment attach = null;

            if (null == this.Data)
                return null;

            if (log.ShouldLog(TraceLevel.Verbose))
                log.Add(TraceLevel.Verbose, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory,
                    "Creating the attachment instance from the assigned data on the component");

            //For the name use the source if specficied, otherwise use a unique increment id for this document
            string name = string.IsNullOrEmpty(this.Source)
                ? this.Document.GetIncrementID(PDFEmbeddedAttachment.EmbeddedFileObjectType)
                : this.Source;
            
            string desc = string.IsNullOrEmpty(this.Description) ? name : this.Description;

            attach = new PDFEmbeddedAttachment(fullpath, name, this.UniqueID, desc)
            {
                FileData = this.Data,
                Name = new PDFName(this.Document.GetIncrementID(PDFEmbeddedAttachment.EmbeddedFileObjectType))
            };


            return attach;
        }

        #endregion

        #region private DateTime GetExpirationTime()

        /// <summary>
        /// Gets the expiration date time for any attachment in this component based on the Never Cache property and then the ImageCacheDuration setting on the document.
        /// </summary>
        /// <returns></returns>
        private DateTime GetExpirationTime()
        {
            DateTime expires;
            
            if (this.NeverCache)
                return DateTime.MinValue;

            
            int cachedur = this.Document.RenderOptions.ImageCacheDurationMinutes;
            expires = DateTime.Now.AddMinutes(cachedur);
            
            return expires;
        }



        #endregion

        private const double IconWidthFactor = 0.75;

        /// <summary>
        /// Returns the required size to be made available within the layout for the attachment icon to render into.
        /// </summary>
        /// <param name="available">The current available size</param>
        /// <param name="context">The current layout context</param>
        /// <param name="appliedstyle">The style applied to the component</param>
        /// <returns>The required size of the component content (excluding any padding or margins)</returns>
        public Size GetRequiredSizeForLayout(Size available, PDFLayoutContext context, Style appliedstyle)
        {
            PDFPositionOptions pos = appliedstyle.CreatePostionOptions(context.PositionDepth > 0);
            PDFTextRenderOptions opts = appliedstyle.CreateTextOptions();

            Unit h;
            Unit w;

            if (pos.Height.HasValue)
                h = pos.Height.Value;

            else
                h = opts.GetAscent(); //set the height as the height of the A


            if (pos.Width.HasValue)
                w = pos.Width.Value;

            else
                w = h * IconWidthFactor;

            return new Size(w, h);
        }

        



        /// <summary>
        /// We don't need to do anything here as the registered AttachmentAnnotationEntry will render the content.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            
            return null;
        }

        /// <summary>
        /// Implements the ViewPort to return a layout engine
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        /// <returns></returns>
        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style fullstyle)
        {
            return new Scryber.PDF.Layout.LayoutEngineAttachment(this, parent);
        }

        
        

    }


    

}
