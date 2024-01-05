using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.PDF.Native;
using Scryber.PDF;
using Scryber.Styles;

namespace Scryber.Components
{
    /// <summary>
    /// Represents a single file attachment in a document
    /// </summary>
    [PDFParsableComponent("Attachment")]
    public class AttachmentAddition : Component, IPDFRenderComponent
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
        public string Source
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
        [PDFAttribute("data")]
        public PDFEmbeddedFileData Data
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
        public string Description
        {
            get;
            set;
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

        
        //
        // .ctor(s)
        //

        #region public PDFAttachment()

        public AttachmentAddition()
            : this(AttachmentType)
        {

        }

        #endregion

        #region protected PDFAttachment(ObjectType type)

        /// <summary>
        /// Protected constructor for inheriting types to call
        /// </summary>
        /// <param name="type"></param>
        protected AttachmentAddition(ObjectType type)
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
                PDFEmbeddedAttachment attach = this.GetAttachment(context, fullstyle);

                if (null != attach)
                {
                    //We are an addition - so register the attachment on the document catalog

                    object entry = context.DocumentLayout.RegisterCatalogEntry(context, PDFArtefactTypes.Names, attach);
                    set.SetArtefact(PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, entry);
                }
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

                if (artefacts.TryGetArtefact(PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, out obj))
                {
                    context.DocumentLayout.CloseArtefactEntry(PDFArtefactTypes.Names, obj);
                }
            }
        }

        #endregion

        #region protected virtual PDFEmbeddedAttachment GetAttachment(PDFLayoutContext context, Styles.PDFStyle fullstyle)

        /// <summary>
        /// Gets the PDFEmbeddedAttachment associated with this Component. If 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        /// <returns></returns>
        protected virtual PDFEmbeddedAttachment GetAttachment(PDFLayoutContext context, Styles.Style fullstyle)
        {
            if (null == this.Attachment)
            {
                if (context.ShouldLogVerbose)
                    context.TraceLog.Begin(TraceLevel.Verbose, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "Loading Attachment for component " + this.UniqueID);

                try
                {
                    using (context.PerformanceMonitor.Record(PerformanceMonitorType.Data_Load, "Attachment - " + this.UniqueID))
                    {
                        this.Attachment = LoadAttachment(context, fullstyle);
                    }
                }
                catch (Exception ex)
                {
                    if (context.Conformance == ParserConformanceMode.Strict)
                        throw new PDFMissingAttachmentException(string.Format("Could not load the attachment data for component " + this.UniqueID + ". See the inner exception for more details."), ex);
                    else
                        context.TraceLog.Add(TraceLevel.Error, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "Could not load the attachment data for component " + this.UniqueID, ex);

                    this.Attachment = null;
                }

                if (context.ShouldLogVerbose)
                {
                    if (null == this.Attachment)
                        context.TraceLog.End(TraceLevel.Verbose, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "No Attachment was loaded for component " + this.UniqueID);
                    else
                        context.TraceLog.End(TraceLevel.Verbose, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "Initialized and loaded the attachment " + this.Attachment.ToString());
                }
                else if (context.ShouldLogMessage)
                {
                    if (null == this.Attachment)
                        context.TraceLog.Add(TraceLevel.Message, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "No Attachment was loaded for component " + this.UniqueID);
                    else
                        context.TraceLog.Add(TraceLevel.Message, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "Initialized and loaded the attachment " + this.Attachment.ToString());
                }
            }
            return this.Attachment;
        }

        #endregion

        #region private PDFEmbeddedAttachment LoadAttachment(PDFLayoutContext context, Styles.PDFStyle fullstyle)

        private PDFEmbeddedAttachment LoadAttachment(PDFLayoutContext context, Styles.Style fullstyle)
        {
            PDFEmbeddedAttachment attach = null;

            if (null != this.Data)
            {
                if (context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Verbose, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "Creating the attachment instance from the assigned data on the component");

                //For the name use the source if specficied, otherwise use a unique increment id for this document
                string name = string.IsNullOrEmpty(this.Source) ? this.Document.GetIncrementID(PDFEmbeddedAttachment.EmbeddedFileObjectType) : this.Source;
                string desc = string.IsNullOrEmpty(this.Description) ? name : this.Description;

                attach = new PDFEmbeddedAttachment(this.Data.FullName,name,this.UniqueID, desc)
                {
                    FileData = this.Data,
                };
            }
            else if (string.IsNullOrEmpty(this.Source) == false)
            {
                string fullpath = this.MapPath(this.Source);
                ICacheProvider cache = this.Document.CacheProvider;

                if (context.ShouldLogVerbose)
                    context.TraceLog.Add(TraceLevel.Verbose, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "Creating the attachment instance from the source value at expanded path '" + fullpath + "'");

                bool isfile;
                string actualname;
                if (System.Uri.IsWellFormedUriString(fullpath, UriKind.Absolute))
                {
                    isfile = false;
                }
                else if (System.IO.Path.IsPathRooted(fullpath))
                {
                    isfile = true;
                }
                else
                    throw new ArgumentException("Cannot load an attachment with a relative path");

                object found;
                if (cache.TryRetrieveFromCache(PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, fullpath, out found))
                {
                    this.Data = (PDFEmbeddedFileData)found;

                    if (context.ShouldLogVerbose)
                        context.TraceLog.Add(TraceLevel.Verbose, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "Cache hit on previously loaded data from file - no need to retrieve from path again.");
                }
                else
                {
                    if (isfile)
                        this.Data = PDFEmbeddedFileData.LoadFileDataFromFile(context, fullpath);
                    else
                        this.Data = PDFEmbeddedFileData.LoadFileDataFromUri(context, fullpath);

                    DateTime expires = GetExpirationTime();

                    if (expires > DateTime.Now)
                    {
                        if (context.ShouldLogVerbose)
                            context.TraceLog.Add(TraceLevel.Verbose, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "Adding loaded data to the cache with an expiration time of " + expires.ToString("yyyy-MM-dd HH:mm:ss"));

                        cache.AddToCache(PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, fullpath, this.Data, expires);
                    }
                    else if (context.ShouldLogVerbose)
                        context.TraceLog.Add(TraceLevel.Verbose, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "Embedded attachment should not be cached for component " + this.UniqueID);
                }

                //extract the name of the file
                try
                {
                    if (isfile)
                        actualname = System.IO.Path.GetFileName(fullpath);
                    else
                    {
                        //Extract the file name from the full uri path
                        Uri uri = new Uri(fullpath);
                        actualname = uri.GetLeftPart(UriPartial.Path);
                        if (string.IsNullOrEmpty(actualname))
                            actualname = fullpath;
                        else if (actualname.IndexOf('.') > 0 && actualname.IndexOf('/') >= 0)
                            actualname = actualname.Substring(actualname.LastIndexOf('/') + 1);
                    }
                }
                catch (Exception ex)
                {
                    context.TraceLog.Add(TraceLevel.Error, PDFEmbeddedAttachment.EmbeddedFilesNamesCategory, "Could not extract the name of the file - replacing with component unique id", ex);
                    actualname = this.UniqueID;
                }

                //get the description
                string desc = this.Description;

                //build the attachment resource

                attach = new PDFEmbeddedAttachment(fullpath,actualname,this.UniqueID,desc)
                {
                    FileData = this.Data,
                };

            }
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
            else
                return DateTime.MinValue;

            //TODO: Support the caching

            //int cachedur = this.Document.RenderOptions.ImageCacheDurationMinutes;

            //if (cachedur == Scryber.Configuration.ImagingConfigurationSection.ImageCacheNeverExpires)
            //    expires = DateTime.MaxValue;
            //else
            //    expires = DateTime.Now.AddMinutes(cachedur);
            //return expires;
        }



        #endregion


        

        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            
            return null;
        }
        

    }


    /// <summary>
    /// A list of attachments
    /// </summary>
    public class PDFAttachmentList : ComponentWrappingList<AttachmentAddition>
    {

        public PDFAttachmentList(Document owner)
            : base(new ComponentList(owner, AttachmentAddition.AttachmentType))
        {

        }
    }

}
