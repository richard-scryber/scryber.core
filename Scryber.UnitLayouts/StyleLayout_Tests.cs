using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;
using Scryber.PDF.Graphics;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class StyleLayout_Tests
    {
        

        PDFLayoutDocument layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this.layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        [TestMethod]
        public void SingleCSSVariable()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
    <style>
        :root { --bg-color: red; }

        div.bg{ 
            background-color: var(--bg-color, black);
        }
    </style>
</head>

<body style='padding:20px'>
  <div class='bg'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));
            


            using (var ms = DocStreams.GetOutputStream("CSSVariables_StaticValue.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);
            var bgBrush = divBlock.FullStyle.CreateBackgroundBrush();

            Assert.IsNotNull(bgBrush);
            Assert.IsInstanceOfType(bgBrush, typeof(PDFSolidBrush));
            
            var solidBrush = (PDFSolidBrush)bgBrush;
            Assert.AreEqual(solidBrush.Color, StandardColors.Red);
        }

        
        [TestMethod]
        public void SingleCSSVariableUpdated()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
    <style>
        :root { --bg-color: red; }

        div{
            --bg-color: green;
        }

        div.bg{ 
            background-color: var(--bg-color, black);
        }
    </style>
</head>

<body style='padding:20px'>
  <div class='bg'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));
            


            using (var ms = DocStreams.GetOutputStream("CSSVariables_UpdatedValue.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);
            var bgBrush = divBlock.FullStyle.CreateBackgroundBrush();

            Assert.IsNotNull(bgBrush);
            Assert.IsInstanceOfType(bgBrush, typeof(PDFSolidBrush));
            
            //Should be updated to the new color defined for divs.
            var solidBrush = (PDFSolidBrush)bgBrush;
            Assert.AreEqual(solidBrush.Color, StandardColors.Green);
        }
        
        
        [TestMethod]
        public void SingleCSSVariableUpdatedBound()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
    <style>
        :root { 
            --bg-color: red;
        }

        div{
            --bg-color: var(brandColor);
        }

        div.bg{ 
            background-color: var(--bg-color, black);
        }
    </style>
</head>

<body style='padding:20px'>
  <div class='bg'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));
            
            doc.Params["brandcolor"] = "yellow";


            using (var ms = DocStreams.GetOutputStream("CSSVariables_BoundValue.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);
            var bgBrush = divBlock.FullStyle.CreateBackgroundBrush();

            Assert.IsNotNull(bgBrush);
            Assert.IsInstanceOfType(bgBrush, typeof(PDFSolidBrush));
            
            //Should be updated to the color bound to 'brandColor'.
            var solidBrush = (PDFSolidBrush)bgBrush;
            Assert.AreEqual(solidBrush.Color, StandardColors.Yellow);

            var bodyBlock = layout.AllPages[0].ContentBlock;
            Assert.IsNotNull(bodyBlock);
            
            // should not have a background
            bgBrush = bodyBlock.FullStyle.CreateBackgroundBrush();
            Assert.IsNull(bgBrush);
        }
    }
}
