using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Verifies the CSS !important flag is honoured by the layout engine, not just style
    /// resolution — a lower-specificity !important width should win over a higher-specificity
    /// non-important width in the actual rendered geometry.
    /// </summary>
    [TestClass()]
    public class CSSImportantLayout_Tests
    {
        private const string TestCategory = "Inject-Layouts";

        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Important_LowSpecificityWidth_WinsOverHighSpecificityWidth()
        {
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        .box { width: 100pt !important; }
        body div#target.box { width: 300pt; }
    </style>
</head>
<body>
    <div id='target' class='box' style='height: 40pt; background-color: lime;'></div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            using (var ms = DocStreams.GetOutputStream("CSSImportantLayout_LowSpecificityWidthWins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var targetBlock = pageRegion.Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(targetBlock, "target layout block should not be null");
            Assert.AreEqual(100.0, targetBlock.TotalBounds.Width.PointsValue, 1.0,
                "The !important width (100pt) should win over the higher specificity, non-important width (300pt)");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void NoImportant_HighSpecificityWidth_StillWins_Regression()
        {
            //Regression check: without !important anywhere, higher specificity should win as before.
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        .box { width: 100pt; }
        body div#target.box { width: 300pt; }
    </style>
</head>
<body>
    <div id='target' class='box' style='height: 40pt; background-color: lime;'></div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            using (var ms = DocStreams.GetOutputStream("CSSImportantLayout_RegressionNoImportant.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var targetBlock = pageRegion.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(300.0, targetBlock.TotalBounds.Width.PointsValue, 1.0,
                "Without !important, the higher specificity width (300pt) should win as before");
        }
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void NoImportant_InlineWidth_StillWins_Regression()
        {
            //Regression check: without !important anywhere, higher specificity should win as before.
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        .box { width: 100pt; }
        body div#target.box { width: 300pt; }
    </style>
</head>
<body>
    <div id='target' class='box' style='height: 40pt; width:200pt; background-color: lime;'></div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            using (var ms = DocStreams.GetOutputStream("CSSImportantLayout_RegressionNoImportant.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var targetBlock = pageRegion.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(200.0, targetBlock.TotalBounds.Width.PointsValue, 1.0,
                "Without !important, the higher specificity width (300pt) should win as before");
        }
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void NoImportant_InlineWidth_ImportantWins_Regression()
        {
            //Regression check: without !important anywhere, higher specificity should win as before.
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        .box { width: 100pt !important; }
        body div#target.box { width: 300pt; }
    </style>
</head>
<body>
    <div id='target' class='box' style='height: 40pt; width:200pt; background-color: lime;'></div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            using (var ms = DocStreams.GetOutputStream("CSSImportantLayout_RegressionNoImportant.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var targetBlock = pageRegion.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(100.0, targetBlock.TotalBounds.Width.PointsValue, 1.0,
                "Without !important, the higher specificity width (300pt) should win as before");
        }
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void NoImportant_ImportantInlineWidth_ImportantInlineWins_Regression()
        {
            //Regression check: without !important anywhere, higher specificity should win as before.
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <style>
        .box { width: 100pt !important; }
        body div#target.box { width: 300pt; }
    </style>
</head>
<body>
    <div id='target' class='box' style='height: 40pt; width:200pt !important; background-color: lime;'></div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            using (var ms = DocStreams.GetOutputStream("CSSImportantLayout_RegressionNoImportant.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var targetBlock = pageRegion.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(200.0, targetBlock.TotalBounds.Width.PointsValue, 1.0,
                "Without !important, the higher specificity width (300pt) should win as before");
        }
    }
}
