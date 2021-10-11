using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Components;
using Scryber.Styles;
using Scryber.PDF;
using Scryber.PDF.Secure;
using Scryber.Logging;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("head")]
    public class HTMLHead : ContainerComponent, IInvisibleContainer
    {
        private string _title;

        [PDFElement("title")]
        public string Title
        {
            get { return this._title; }
            set {

                this._title = value;

                if (null != this.Document)
                    this.Document.Info.Title = value;
            }
        }

        private HTMLHeadBase _base;

        [PDFElement("base")]
        public HTMLHeadBase BasePath
        {
            get { return _base; }
            set { _base = value;
                this.UpdateDocumentBase();
            }
        }

        [PDFArray(typeof(Component))]
        [PDFElement("")]
        public ComponentList Contents
        {
            get { return base.InnerContent; }
            set { base.InnerContent = value; }
        }

        protected internal override void RegisterParent(Component parent)
        {
            base.RegisterParent(parent);
            UpdateDocumentInfo(parent);
        }

        protected override void OnDataBound(DataContext context)
        {
            this.UpdateDocumentInfo(this.Parent);
            base.OnDataBound(context);
        }

        private void UpdateDocumentBase()
        {
            if(this.Parent is Document)
            {
                string path = string.Empty;
            }
        }

        private void UpdateDocumentInfo(Component parent)
        {
            if (parent is Document)
            {

                var doc = parent as Document;

                bool logVerbose = doc.TraceLog.ShouldLog(TraceLevel.Verbose);

                if (doc.TraceLog.ShouldLog(TraceLevel.Message))
                    doc.TraceLog.Add(TraceLevel.Message, "meta", "Updating the document information and restrictions");

                if (!string.IsNullOrEmpty(this.Title))
                {
                    if (logVerbose)
                        doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document title to " + this.Title);

                    doc.Info.Title = this.Title;
                }

                if(null != this.BasePath && !string.IsNullOrEmpty(this.BasePath.Href))
                {
                    if (logVerbose)
                        doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document LoadedSource to the base path " + this.BasePath.Href);
                    doc.LoadedSource = this.BasePath.Href;
                }

                foreach (var item in this.Contents)
                {
                    if(item is HTMLMeta)
                    {
                        var meta = (HTMLMeta)item;
                        switch (meta.Name)
                        {
                            case ("author"):
                                if (logVerbose)
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document author to " + meta.Content);

                                doc.Info.Author = meta.Content;
                                break;
                            case ("description"):
                                if (logVerbose)
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document description to " + meta.Content);

                                doc.Info.Subject = meta.Content;
                                break;
                            case ("keywords"):
                                if (logVerbose)
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document keywords to " + meta.Content);

                                doc.Info.Keywords = meta.Content;
                                break;
                            case ("generator"):
                                if (logVerbose)
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document generator to " + meta.Content);

                                doc.Info.Producer = meta.Content;
                                break;
                            case ("print-restrictions"):
                                if (logVerbose)
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document restrictions to " + meta.Content);

                                ParseRestrictions(meta.Content, doc.RenderOptions.Permissions, doc.TraceLog);
                                break;
                            case ("print-encryption"):
                                if (logVerbose)
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document restrictions to " + meta.Content);

                                ParseSecurityType(meta.Content, doc.RenderOptions.Permissions, doc.TraceLog);

                                break;
                            default:
                                if (logVerbose)
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Skipping unknown meta tag " + meta.Name);
                                break;
                        }
                    }
                }
            }
        }

        protected void ParseSecurityType(string content, PDFDocumentPermissions permissions, TraceLog log)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = content.ToLower();
                switch (content)
                {
                    case ("40bit"):
                        if (log.ShouldLog(TraceLevel.Message))
                            log.Add(TraceLevel.Message, "meta", "Set the document encryption to 40 bit standard (v1.2)");
                        permissions.Type = PDF.Secure.SecurityType.Standard40Bit;
                        break;
                    case ("128bit"):
                        if (log.ShouldLog(TraceLevel.Message))
                            log.Add(TraceLevel.Message, "meta", "Set the document encryption to 128 bit standard (v2.3)");
                        permissions.Type = PDF.Secure.SecurityType.Standard128Bit;
                        break;
                    default:
                        if (log.ShouldLog(TraceLevel.Warning))
                            log.Add(TraceLevel.Warning, "meta", "The document encryption " + content + " was not a recognised value, use 40bit or 128bit");

                        break;
                }
            }
        }

        private static readonly char[] _splits = new char[] { ' ', ',' };

        protected void ParseRestrictions(string content, PDFDocumentPermissions permissions, TraceLog log)
        {
            if (string.IsNullOrEmpty(content))
                return;
            content = content.Trim().ToLower();

            bool logVerbose = log.ShouldLog(TraceLevel.Verbose);
            if (content == "none")
            {
                if (logVerbose)
                    log.Add(TraceLevel.Verbose, "meta", "Cleared all restrictions from the document");
                return;
            }

            permissions.AllowAccessiblity = false;
            permissions.AllowAnnotations = false;
            permissions.AllowCopying = false;
            permissions.AllowDocumentAssembly = false;
            permissions.AllowFormFilling = false;
            permissions.AllowHighQualityPrinting = false;
            permissions.AllowModification = false;
            permissions.AllowPrinting = false;

            if (content == "all")
            {
                if (logVerbose)
                    log.Add(TraceLevel.Verbose, "meta", "Set restrictions to ALL for the document");

                return;
            }

            string[] parts = content.Split(_splits, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var part in parts)
            {
                switch (part)
                {
                    case ("allow-printing"):
                    case ("printing"):
                        permissions.AllowHighQualityPrinting = true;
                        permissions.AllowPrinting = true;
                        if (logVerbose)
                            log.Add(TraceLevel.Verbose, "meta", "Allowed printing for the document");
                        break;
                    case ("allow-accessibility"):
                    case ("accessibility"):
                        permissions.AllowAccessiblity = true;
                        if (logVerbose)
                            log.Add(TraceLevel.Verbose, "meta", "Allowed accessibility for the document");
                        break;
                    case ("allow-annotations"):
                    case ("annotations"):
                        permissions.AllowAnnotations = true;
                        if (logVerbose)
                            log.Add(TraceLevel.Verbose, "meta", "Allowed annotations for the document");
                        break;
                    case ("allow-copying"):
                    case ("copying"):
                        permissions.AllowCopying = true;
                        if (logVerbose)
                            log.Add(TraceLevel.Verbose, "meta", "Allowed copying for the document");
                        break;
                    case ("allow-modifications"):
                    case ("modifications"):
                        permissions.AllowModification = true;
                        permissions.AllowDocumentAssembly = true;
                        if (logVerbose)
                            log.Add(TraceLevel.Verbose, "meta", "Allowed modifications for the document");
                        break;
                    case ("allow-forms"):
                    case ("forms"):
                        permissions.AllowFormFilling = true;
                        if (logVerbose)
                            log.Add(TraceLevel.Verbose, "meta", "Allowed form filling for the document");
                        break;
                    default:
                        if (log.ShouldLog(TraceLevel.Warning))
                            log.Add(TraceLevel.Warning, "meta", "The restrictions part " + part + " was not recognised as a valid restriction");
                        break;
                }
            }

        }

        protected override void OnPreLayout(LayoutContext context)
        {
            base.OnPreLayout(context);
            this.UpdateDocumentInfo(this.Document);

        }

        public HTMLHead() : this(ObjectTypes.NoOp)
        {

        }

        public HTMLHead(ObjectType type): base(type)
        {

        }

        
    }


    /// <summary>
    /// Simple class to store the base for the document
    /// </summary>
    public class HTMLHeadBase
    {
        /// <summary>
        /// Gets or sets the base path for the document
        /// </summary>
        [PDFAttribute("href")]
        [PDFLoadedSource()]
        public string Href { get; set; }

        public HTMLHeadBase()
        {

        }
    }
}
