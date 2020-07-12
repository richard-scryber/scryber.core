using System;
using System.Collections.Generic;
using Scryber.Components;
using Scryber;
using Scryber.Data;

namespace Scryber.Core.UnitTests.Mocks
{
    public class MockControllerClass
    {

        public List<String> Results = new List<string>();

        [PDFOutlet]
        public PDFHead1 Title { get; set; }

        [PDFOutlet("MyDocument", Required = true)]
        public PDFDocument Document { get; set; }
        
        public MockControllerClass()
        { 
            Results.Add("Controller Initialized");
        }

        //
        // Document lifecycle
        //

        [PDFAction()]
        public void DocumentInitialized(object sender, PDFInitEventArgs args)
        {
            Results.Add("Controller Document Initialized");
        }

        [PDFAction()]
        public void DocumentLoaded(object sender, PDFLoadEventArgs args)
        {
            this.Title.Text = "Set In the Load Event";
            Results.Add("Controller Document Loaded");
        }

        [PDFAction()]
        public void DocumentBinding(object sender, PDFDataBindEventArgs args)
        {
            Results.Add("Controller Document DataBinding");
        }

        [PDFAction()]
        public void DocumentDataBound(object sender, PDFDataBindEventArgs args)
        {
            Results.Add("Controller Document Databound");
        }

        [PDFAction()]
        public void DocumentPreLayout(object sender, PDFLayoutEventArgs args)
        {
            Results.Add("Controller Document Laying out");
        }

        [PDFAction()]
        public void DocumentPostLayout(object sender, PDFLayoutEventArgs args)
        {
            Results.Add("Controller Document Laid out");
        }

        [PDFAction()]
        public void DocumentPreRender(object sender, PDFRenderEventArgs args)
        {
            Results.Add("Controller Document Rendering");
        }

        [PDFAction()]
        public void DocumentPostRender(object sender, PDFRenderEventArgs args)
        {
            Results.Add("Controller Document Rendered");
        }

        //
        // Heading Binding
        //

        [PDFAction()]
        public void HeaderBinding(object sender, PDFDataBindEventArgs args)
        {
            if (null == this.Title)
                throw new NullReferenceException("Title heading was not set");
            this.Title.Text = "Set from the controller";

            Results.Add("Controller Header Databound");
        }

        //
        // ForEach Binding
        //

        private string[] Entries = new string[] { "First", "Second", "Third" };

        [PDFAction()]
        public void ForEachBinding(object sender, PDFDataBindEventArgs args)
        {
            var forEach = sender as PDFForEach;
            forEach.Value = Entries;

            Results.Add("Controller ForEach Binding");
        }

        [PDFAction()]
        public void ForEachItemBinding(object sender, PDFTemplateItemDataBoundArgs args)
        {
            var forEach = sender as PDFForEach;
            var index = args.Context.CurrentIndex;

            Results.Add("Controller ForEach Item Bound " + index);
        }

        [PDFAction()]
        public void ForEachListItemBound(object sender, PDFDataBindEventArgs args)
        {
            var li = sender as PDFListItem;

            var str = (string)args.Context.DataStack.Current;
            str += " - Index " + args.Context.CurrentIndex;

            li.Contents.Add(new PDFTextLiteral(str));

            Results.Add("Controller ForEach Label " + args.Context.CurrentIndex + " Databound");
        }

        public IPDFComponent PartialView(PDFDocument doc, PDFInitContext context)
        {
            var str = "";

            using (var reader = new System.IO.StringReader(str))
            {
                var content = PDFDocument.Parse(reader, ParseSourceType.DynamicContent);
                return content;
            }
        }


    }
}
