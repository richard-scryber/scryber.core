using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("head")]
    public class HTMLHead : ContainerComponent, IPDFInvisibleContainer
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

        private void UpdateDocumentInfo(Component parent)
        {
            if (parent is Document)
            {
                var doc = parent as Document;

                if (doc.TraceLog.ShouldLog(TraceLevel.Message))
                    doc.TraceLog.Add(TraceLevel.Message, "meta", "Updating the document information and restrictions");

                if (!string.IsNullOrEmpty(this.Title))
                {
                    if (doc.TraceLog.ShouldLog(TraceLevel.Verbose))
                        doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document title to " + this.Title);

                    doc.Info.Title = this.Title;
                }

                foreach (var item in this.Contents)
                {
                    if(item is HTMLMeta)
                    {
                        var meta = (HTMLMeta)item;
                        switch (meta.Name)
                        {
                            case ("author"):
                                if (doc.TraceLog.ShouldLog(TraceLevel.Verbose))
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document author to " + meta.Content);

                                doc.Info.Author = meta.Content;
                                break;
                            case ("description"):
                                if (doc.TraceLog.ShouldLog(TraceLevel.Verbose))
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document description to " + meta.Content);

                                doc.Info.Subject = meta.Content;
                                break;
                            case ("keywords"):
                                if (doc.TraceLog.ShouldLog(TraceLevel.Verbose))
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document keywords to " + meta.Content);

                                doc.Info.Keywords = meta.Content;
                                break;
                            case ("generator"):
                                if (doc.TraceLog.ShouldLog(TraceLevel.Verbose))
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document generator to " + meta.Content);

                                doc.Info.Producer = meta.Content;
                                break;
                            case ("restrictions"):
                                if (doc.TraceLog.ShouldLog(TraceLevel.Verbose))
                                    doc.TraceLog.Add(TraceLevel.Verbose, "meta", "Updating the document restrictions to " + meta.Content);

                                ParseRestrictions(meta.Content, doc.Permissions);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private static readonly char[] _splits = new char[] { ' ', ',' };

        protected void ParseRestrictions(string content, Secure.DocumentPermissions permissions)
        {
            if (string.IsNullOrEmpty(content))
                return;
            content = content.Trim().ToLower();

            if (content == "none")
                return;

            permissions.AllowAccessiblity = false;
            permissions.AllowAnnotations = false;
            permissions.AllowCopying = false;
            permissions.AllowDocumentAssembly = false;
            permissions.AllowFormFilling = false;
            permissions.AllowHighQualityPrinting = false;
            permissions.AllowModification = false;
            permissions.AllowPrinting = false;

            if (content == "all")
                return;

            string[] parts = content.Split(_splits, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var part in parts)
            {
                switch (part)
                {
                    case ("allow-printing"):
                    case ("printing"):
                        permissions.AllowHighQualityPrinting = true;
                        permissions.AllowPrinting = true;
                        break;
                    case ("allow-accessibility"):
                    case ("accessibility"):
                        permissions.AllowAccessiblity = true;
                        break;
                    case ("allow-annotations"):
                    case ("annotations"):
                        permissions.AllowAnnotations = true;
                        break;
                    case ("allow-copying"):
                    case ("copying"):
                        permissions.AllowCopying = true;
                        break;
                    case ("allow-modifications"):
                    case ("modifications"):
                        permissions.AllowModification = true;
                        permissions.AllowDocumentAssembly = true;
                        break;
                    case ("allow-forms"):
                    case ("forms"):
                        permissions.AllowFormFilling = true;
                        break;
                    default:
                        break;
                }
            }

        }

        protected override void OnPreLayout(PDFLayoutContext context)
        {
            base.OnPreLayout(context);
            this.UpdateDocumentInfo(this.Document);

        }

        public HTMLHead() : this(PDFObjectTypes.NoOp)
        {

        }

        public HTMLHead(PDFObjectType type): base(type)
        {

        }

        
    }
}
