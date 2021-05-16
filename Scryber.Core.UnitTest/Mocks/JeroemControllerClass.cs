using System;
using System.Collections.Generic;
using Scryber.Components;
using Scryber;
using Scryber.Data;
using Scryber.Html.Components;

namespace Scryber.Core.UnitTests.Mocks
{
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


        [PDFAction()]
        public void ImageBound(object sender, PDFDataBindEventArgs args)
        {
            var img = sender as HTMLImage;
            img.Source = "/MaterialIcons/" + img.Source + ".png";
        }


    }
}
