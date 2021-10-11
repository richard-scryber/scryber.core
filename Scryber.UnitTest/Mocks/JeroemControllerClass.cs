using System;
using System.Collections.Generic;
using Scryber.Components;
using Scryber;
using Scryber.Data;
using Scryber.Html.Components;

namespace Scryber.Core.UnitTests.Mocks
{
    public class GenericControllerClass
    {


        [PDFAction("init-para")]
        public void ParagraphInit(object sender, InitEventArgs args)
        {
            (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We are initialized", StyleClass = "block" });
            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Initialized the paragraph");
        }

        [PDFAction("load-para")]
        public void ParagraphLoad(object sender, LoadEventArgs args)
        {
            (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We have loaded", StyleClass = "block" });
            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Loaded the paragraph");
        }

        [PDFAction("bind-para")]
        public void ParagraphBinding(object sender, DataBindEventArgs args)
        {
            (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We are binding", StyleClass = "block" });
            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Binding the paragraph");
        }

        [PDFAction("bound-para")]
        public void ParagraphBound(object sender, DataBindEventArgs args)
        {
            (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We have bound", StyleClass = "block" });
            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Bound the paragraph");
        }


        [PDFAction("pre-layout-para")]
        public void ParagraphPreLayout(object sender, PDFLayoutEventArgs args)
        {
            (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We are laying out", StyleClass = "block" });
            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Laying-out the paragraph");
        }

        [PDFAction("post-layout-para")]
        public void ParagraphPostLayout(object sender, PDFLayoutEventArgs args)
        {
            //This label will not appear as we have finished using the components
            (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We have been laid out", StyleClass = "block" });

            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Laid-out the paragraph");
        }


        [PDFAction("pre-render-para")]
        public void ParagraphPreRender(object sender, PDFRenderEventArgs args)
        {
            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Rendering the paragraph");
        }

        [PDFAction("post-render-para")]
        public void ParagraphBinding(object sender, PDFRenderEventArgs args)
        {
            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Rendered the paragraph");
        }

        

        [PDFAction("template-item-bound")]
        public void TemplateItemBound(object sender, PDFTemplateItemDataBoundArgs args)
        {
            var template = sender as HTMLTemplate;

            //This is the wrapper component
            var component = args.Item as Component;

            var context = args.Context;

            var index = args.Context.CurrentIndex;

            //Add a class for every other item.
            if (index % 2 == 0)
                component.StyleClass = (component.StyleClass == null) ? "alternate" : (component.StyleClass + " alternate");

            index++;
        }

        [PDFAction("row-bound")]
        public void TemplateItemCellBound(object sender, DataBindEventArgs args)
        {
            //This is the wrapper component
            var row = sender as TableCell;

            var context = args.Context;

            var index = args.Context.CurrentIndex;

            //Add a class for every other item.

            if (index % 2 == 0)
                row.Style.Background.Color = (Scryber.Drawing.Color)"#DDD";// = (row.StyleClass == null) ? "alternate" : (row.StyleClass + " alternate");

        }

        
    }

    public class JeroemControllerClass
    {

        [PDFOutlet("MyDocument", Required = true)]
        public Document Document { get; set; }
        
        public JeroemControllerClass()
        {
        }

        [PDFAction()]
        public void Initialized(object sender, InitEventArgs args)
        {
            Document.Info.Title = "My Title";
        }

        

        //
        // Template Binding
        //


        [PDFAction(IsAction = false)]
        public void ImageBound(object sender, DataBindEventArgs args)
        {
            

            HTMLParagraph p = sender as HTMLParagraph;
            p.Contents.Add(new TextLiteral("This is my content"));
        }


    }
}
