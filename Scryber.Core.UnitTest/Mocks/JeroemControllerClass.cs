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
        public void ParagraphInit(object sender, PDFInitEventArgs args)
        {
            (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We are initialized", StyleClass = "block" });
            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Initialized the paragraph");
        }

        [PDFAction("load-para")]
        public void ParagraphLoad(object sender, PDFLoadEventArgs args)
        {
            (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We have loaded", StyleClass = "block" });
            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Loaded the paragraph");
        }

        [PDFAction("bind-para")]
        public void ParagraphBinding(object sender, PDFDataBindEventArgs args)
        {
            (sender as HTMLParagraph).Contents.Add(new Label() { Text = "We are binding", StyleClass = "block" });
            args.Context.TraceLog.Add(TraceLevel.Message, "Custom Code", "Binding the paragraph");
        }

        [PDFAction("bound-para")]
        public void ParagraphBound(object sender, PDFDataBindEventArgs args)
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
    }

    public class JeroemControllerClass
    {

        [PDFOutlet("MyDocument", Required = true)]
        public Document Document { get; set; }
        
        public JeroemControllerClass()
        {
        }

        [PDFAction()]
        public void Initialized(object sender, PDFInitEventArgs args)
        {
            Document.Info.Title = "My Title";
        }

        

        //
        // Template Binding
        //


        [PDFAction(IsAction = false)]
        public void ImageBound(object sender, PDFDataBindEventArgs args)
        {
            

            HTMLParagraph p = sender as HTMLParagraph;
            p.Contents.Add(new TextLiteral("This is my content"));
        }


    }
}
