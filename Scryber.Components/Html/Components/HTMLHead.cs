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

                if (!string.IsNullOrEmpty(this.Title))
                    doc.Info.Title = this.Title;

                foreach (var item in this.Contents)
                {
                    if(item is HTMLMeta)
                    {
                        var meta = (HTMLMeta)item;
                        switch (meta.Name)
                        {
                            case ("author"):
                                doc.Info.Author = meta.Content;
                                break;
                            case ("description"):
                                doc.Info.Subject = meta.Content;
                                break;
                            case ("keywords"):
                                doc.Info.Keywords = meta.Content;
                                break;
                            case ("generator"):
                                doc.Info.Producer = meta.Content;
                                break;
                            default:
                                break;
                        }
                    }
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

        public override PDFStyle GetAppliedStyle(Component forComponent, PDFStyle baseStyle)
        {
            var applied = baseStyle;
            foreach (var item in this.Contents)
            {
                if(item is PDFHtmlStyleDefnGroup)
                {
                    applied = item.GetAppliedStyle(forComponent, applied);
                }
            }
            return applied;
        }
    }
}
