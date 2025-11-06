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

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class BorderLayout_Tests
    {
        

        PDFLayoutDocument layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this.layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private PDFLayoutComponentRun GetBlockImageRunForPage(int pg, int column = 0, int contentIndex = 0, int runIndex = 0)
        {
            var lpg = layout.AllPages[pg];
            var l1 = lpg.ContentBlock.Columns[column].Contents[contentIndex] as PDFLayoutLine;
            var lrun = l1.Runs[runIndex] as Scryber.PDF.Layout.PDFLayoutComponentRun;
            return lrun;
        }

        private PDFLayoutComponentRun GetInlineImageRunForPage(int pg, int column = 0, int contentIndex = 0, int runIndex = 0)
        {
            var lpg = layout.AllPages[pg];
            var l1 = lpg.ContentBlock.Columns[column].Contents[contentIndex] as PDFLayoutLine;
            var lrun = l1.Runs[runIndex] as Scryber.PDF.Layout.PDFLayoutComponentRun;
            return lrun;
        }

        private void AssertAreApproxEqual(double one, double two, string message = null)
        {
            int precision = 5;
            one = Math.Round(one, precision);
            two = Math.Round(two, precision);
            Assert.AreEqual(one, two, message);
        }

        [TestMethod]
        public void FullBorder()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
</head>

<body style='padding:20px'>
  <div id='withBorders'  style='border: solid 1px blue; height: 200px; padding: 20px;'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));
            


            using (var ms = DocStreams.GetOutputStream("Borders_FullBorder.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);
            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual(Sides.Bottom | Sides.Top | Sides.Right | Sides.Left, borders.AllSides);
            Assert.IsNotNull(borders.AllPen);

            Assert.IsNull(borders.LeftPen);
            Assert.IsNull(borders.RightPen);
            Assert.IsNull(borders.TopPen);
            Assert.IsNull(borders.BottomPen);
        }


        [TestMethod]
        public void FullPlusBottomGreenBorder()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
</head>

<body style='padding:20px'>
  <div id='withBorders' style='border: solid 1px blue; height: 200px; padding: 20px; border-bottom: dashed 1px green'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));



            using (var ms = DocStreams.GetOutputStream("Borders_FullPlusBottomGreenBorder.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);


            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);
            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual(Sides.Top | Sides.Right | Sides.Left, borders.AllSides); //Not Bottom
            Assert.IsNotNull(borders.AllPen);
            Assert.IsNull(borders.LeftPen);
            Assert.IsNull(borders.RightPen);
            Assert.IsNull(borders.TopPen);

            Assert.IsNotNull(borders.BottomPen);

        }


        [TestMethod]
        public void FullNoBottomBorder()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
</head>

<body style='padding:20px'>
  <div id='withBorders' style='border: solid 1px blue; height: 200px; padding: 20px; border-bottom: none'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));



            using (var ms = DocStreams.GetOutputStream("Borders_FullNoBottomBorder.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);


            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            ((Scryber.Styles.StyleFull)divBlock.FullStyle).ClearFullRefs();

            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual(Sides.Top | Sides.Right | Sides.Left, borders.AllSides); //Not Bottom
            Assert.IsNotNull(borders.AllPen);

            Assert.IsNull(borders.LeftPen);
            Assert.IsNull(borders.RightPen);
            Assert.IsNull(borders.TopPen);

            Assert.IsNotNull(borders.BottomPen);
            Assert.IsInstanceOfType(borders.BottomPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

        }


        [TestMethod]
        public void FullNoLeftBottomBorder()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
</head>

<body style='padding:20px'>
  <div id='withBorders' style='border: solid 1px blue; height: 200px; padding: 20px; border-bottom: none; border-left: none;'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));



            using (var ms = DocStreams.GetOutputStream("Borders_FullNoLeftBottomBorder.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);


            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            ((Scryber.Styles.StyleFull)divBlock.FullStyle).ClearFullRefs();

            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual(Sides.Top | Sides.Right, borders.AllSides); //Not Bottom or Left
            Assert.IsNotNull(borders.AllPen);

            Assert.IsNotNull(borders.LeftPen);
            Assert.IsInstanceOfType(borders.LeftPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

            Assert.IsNull(borders.RightPen);
            Assert.IsNull(borders.TopPen);

            Assert.IsNotNull(borders.BottomPen);
            Assert.IsInstanceOfType(borders.BottomPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

        }


        [TestMethod]
        public void FullNoLeftRightBottomBorder()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
</head>

<body style='padding:20px'>
  <div id='withBorders' style='border: solid 1px blue; height: 200px; padding: 20px; border-bottom: none; border-left: none; border-right:none'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));



            using (var ms = DocStreams.GetOutputStream("Borders_FullNoLeftRightBottomBorder.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);


            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            ((Scryber.Styles.StyleFull)divBlock.FullStyle).ClearFullRefs();

            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual(Sides.Top, borders.AllSides); //Not Bottom, Left or Right
            Assert.IsNotNull(borders.AllPen);

            Assert.IsNotNull(borders.LeftPen);
            Assert.IsInstanceOfType(borders.LeftPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

            Assert.IsNotNull(borders.RightPen);
            Assert.IsInstanceOfType(borders.RightPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

            Assert.IsNull(borders.TopPen);

            Assert.IsNotNull(borders.BottomPen);
            Assert.IsInstanceOfType(borders.BottomPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

        }


        [TestMethod]
        public void FullNoLeftRightBottomOrTopBorder()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
</head>

<body style='padding:20px'>
  <div id='withBorders' style='border: solid 1px blue; height: 200px; padding: 20px; border-bottom: none; border-left: none; border-right:none; border-top: none'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));



            using (var ms = DocStreams.GetOutputStream("Borders_FullNoLeftRightBottomOrTopBorder.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);


            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            ((Scryber.Styles.StyleFull)divBlock.FullStyle).ClearFullRefs();

            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual((Sides)0, borders.AllSides); //Not Bottom, Left or Right
            Assert.IsNotNull(borders.AllPen);

            Assert.IsNotNull(borders.LeftPen);
            Assert.IsInstanceOfType(borders.LeftPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

            Assert.IsNotNull(borders.RightPen);
            Assert.IsInstanceOfType(borders.RightPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

            Assert.IsNotNull(borders.TopPen);
            Assert.IsInstanceOfType(borders.TopPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

            Assert.IsNotNull(borders.BottomPen);
            Assert.IsInstanceOfType(borders.BottomPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

        }


        [TestMethod]
        public void FullNoLeftBottomBorderFromStyle1()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
    <style>
        #withBorders{
            border: solid 1px blue;
        }
    </style>
</head>

<body style='padding:20px'>
  <div id='withBorders' style='height: 200px; padding: 20px; border-bottom: none; border-left: none;'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));



            using (var ms = DocStreams.GetOutputStream("Borders_FullNoLeftBottomBorderFromStyle1.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);


            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            ((Scryber.Styles.StyleFull)divBlock.FullStyle).ClearFullRefs();

            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual(Sides.Top | Sides.Right, borders.AllSides); //Not Bottom or Left
            Assert.IsNotNull(borders.AllPen);

            Assert.IsNotNull(borders.LeftPen);
            Assert.IsInstanceOfType(borders.LeftPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

            Assert.IsNull(borders.RightPen);
            Assert.IsNull(borders.TopPen);

            Assert.IsNotNull(borders.BottomPen);
            Assert.IsInstanceOfType(borders.BottomPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

        }


        [TestMethod]
        public void FullNoLeftBottomBorderFromStyle2()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
    <style>
        #withBorders{
            border: solid 1px blue;
        }

        div{
            border-bottom: none;
            border-left: none;
        }
    </style>
</head>

<body style='padding:20px'>
  <div id='withBorders' style='height: 200px; padding: 20px;'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));



            using (var ms = DocStreams.GetOutputStream("Borders_FullNoLeftBottomBorderFromStyle2.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);


            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            ((Scryber.Styles.StyleFull)divBlock.FullStyle).ClearFullRefs();

            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual(Sides.Top | Sides.Right, borders.AllSides); //Not Bottom or Left
            Assert.IsNotNull(borders.AllPen);
            Assert.IsInstanceOfType(borders.AllPen, typeof(Scryber.PDF.Graphics.PDFSolidPen));
            var solidAll = borders.AllPen as PDF.Graphics.PDFSolidPen;

            Assert.IsNotNull(borders.LeftPen);
            Assert.IsInstanceOfType(borders.LeftPen, typeof(Scryber.PDF.Graphics.PDFSolidPen)); //style 1 has a higher priority than style 2
            var solidLeft = borders.LeftPen as PDF.Graphics.PDFSolidPen;

            Assert.AreEqual(solidAll.Color, solidLeft.Color);
            Assert.AreEqual(solidAll.Width, solidLeft.Width);

            Assert.IsNull(borders.RightPen);
            Assert.IsNull(borders.TopPen);

            Assert.IsNotNull(borders.BottomPen);
            Assert.IsInstanceOfType(borders.BottomPen, typeof(Scryber.PDF.Graphics.PDFSolidPen));

            var solidBottom = borders.BottomPen as PDF.Graphics.PDFSolidPen;

            Assert.AreEqual(solidAll.Color, solidBottom.Color);
            Assert.AreEqual(solidAll.Width, solidBottom.Width);

        }

        [TestMethod]
        public void FullNoLeftBottomBorderFromStyle3()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
    <style>
        div{
            border: solid 1px blue;
        }

        #withBorders{
            border-bottom: none;
            border-left: none;
        }
    </style>
</head>

<body style='padding:20px'>
  <div id='withBorders' style='height: 200px; padding: 20px;'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));



            using (var ms = DocStreams.GetOutputStream("Borders_FullNoLeftBottomBorderFromStyle3.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);


            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            ((Scryber.Styles.StyleFull)divBlock.FullStyle).ClearFullRefs();

            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual(Sides.Top | Sides.Right, borders.AllSides); //Not Bottom or Left
            Assert.IsNotNull(borders.AllPen);
            Assert.IsInstanceOfType(borders.AllPen, typeof(Scryber.PDF.Graphics.PDFSolidPen));
            
            Assert.IsNotNull(borders.LeftPen);
            Assert.IsInstanceOfType(borders.LeftPen, typeof(Scryber.PDF.Graphics.PDFNoPen)); //style 2 has a higher priority than style 1

            Assert.IsNull(borders.RightPen);
            Assert.IsNull(borders.TopPen);

            Assert.IsNotNull(borders.BottomPen);
            Assert.IsInstanceOfType(borders.BottomPen, typeof(Scryber.PDF.Graphics.PDFNoPen));


        }


        [TestMethod]
        public void FullNoLeftBottomBorderFromStyle4()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
    <style>
        div{
            border: solid 1px blue;
        }

        #withBorders{
            border-bottom-color: green;
            border-bottom-width: 2pt;
            border-left-color: red;
            border-left-width: 4pt;
        }
    </style>
</head>

<body style='padding:20px'>
  <div id='withBorders' style='height: 200px; padding: 20px;'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));



            using (var ms = DocStreams.GetOutputStream("Borders_FullNoLeftBottomBorderFromStyle4.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);


            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            ((Scryber.Styles.StyleFull)divBlock.FullStyle).ClearFullRefs();

            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual(Sides.Top | Sides.Right, borders.AllSides); //Not Bottom or Left
            Assert.IsNotNull(borders.AllPen);
            Assert.IsInstanceOfType(borders.AllPen, typeof(Scryber.PDF.Graphics.PDFSolidPen));
            var penAll = borders.AllPen as PDF.Graphics.PDFSolidPen;

            Assert.IsNotNull(borders.LeftPen);
            Assert.IsInstanceOfType(borders.LeftPen, typeof(Scryber.PDF.Graphics.PDFSolidPen)); //style 2 has a higher priority than style 1
            var penLeft = borders.LeftPen as PDF.Graphics.PDFSolidPen;
            Assert.AreEqual(penAll.LineStyle, penLeft.LineStyle);
            Assert.AreEqual(penLeft.Width, (Unit)4);
            Assert.AreEqual(penLeft.Color, StandardColors.Red);

            Assert.IsNull(borders.RightPen);
            Assert.IsNull(borders.TopPen);

            Assert.IsNotNull(borders.BottomPen);
            Assert.IsInstanceOfType(borders.BottomPen, typeof(Scryber.PDF.Graphics.PDFSolidPen));
            var penBottom = borders.BottomPen as PDF.Graphics.PDFSolidPen;
            Assert.AreEqual(penAll.LineStyle, penBottom.LineStyle);
            Assert.AreEqual(penBottom.Width, (Unit)2);
            Assert.AreEqual(penBottom.Color, StandardColors.Green);

        }


        [TestMethod]
        public void FullNoLeftBottomBorderFromStyle5()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <title>All Borders</title>
    <style>
        div{
            border: none 1pt blue;
        }

        #withBorders{
            border-bottom-style: dashed;

            border-left-style: dotted;
            border-left-color: red;
            border-left-width: 4pt;
        }
    </style>
</head>

<body style='padding:20px'>
  <div id='withBorders' style='height: 200px; padding: 20px;'>Inside the div</div>
</body>

</html>";


            var doc = Document.ParseDocument(new System.IO.StringReader(html));



            using (var ms = DocStreams.GetOutputStream("Borders_FullNoLeftBottomBorderFromStyle5.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);


            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            ((Scryber.Styles.StyleFull)divBlock.FullStyle).ClearFullRefs();

            var borders = divBlock.FullStyle.CreateBorderPen();

            Assert.AreEqual(Sides.Top | Sides.Right, borders.AllSides); //Not Bottom or Left
            Assert.IsNotNull(borders.AllPen);
            Assert.IsInstanceOfType(borders.AllPen, typeof(Scryber.PDF.Graphics.PDFNoPen));

            Assert.IsNotNull(borders.LeftPen);
            Assert.IsInstanceOfType(borders.LeftPen, typeof(Scryber.PDF.Graphics.PDFDashPen)); //style 2 has a higher priority than style 1

            var penLeft = borders.LeftPen as PDF.Graphics.PDFDashPen;
            Assert.AreEqual(LineType.Dash, penLeft.LineStyle);
            Assert.AreEqual(penLeft.Width, (Unit)4);
            Assert.AreEqual(penLeft.Color, StandardColors.Red);

            Assert.IsNull(borders.RightPen);
            Assert.IsNull(borders.TopPen);

            Assert.IsNotNull(borders.BottomPen);
            Assert.IsInstanceOfType(borders.BottomPen, typeof(Scryber.PDF.Graphics.PDFDashPen));
            var penBottom = borders.BottomPen as PDF.Graphics.PDFDashPen;
            Assert.AreEqual(LineType.Dash, penBottom.LineStyle);
            Assert.AreEqual(penBottom.Width, (Unit)1);
            Assert.AreEqual(penBottom.Color, StandardColors.Blue);

        }


    }
}
