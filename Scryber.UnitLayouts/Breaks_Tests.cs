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
    public class Breaks_Tests
    {
        const string TestCategoryName = "Layout";

        PDFLayoutDocument layout;

        /// <summary>
        /// Event handler that sets the layout instance variable, after the layout has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimplePageBreak()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);


            section.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(new PageBreak());
            section.Contents.Add(new TextLiteral("Sits on the second page"));


            using (var ms = DocStreams.GetOutputStream("Breaks_SimplePageBreak.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(25, first.Height);
            Assert.AreEqual(3, first.Runs.Count);
            var chars = first.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the first page", chars.Characters);

            content = layout.AllPages[1].ContentBlock;

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(second);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(3, second.Runs.Count);
            chars = second.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the second page", chars.Characters);



        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedPageBreak()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);


            var div1 = new Div() { Margins = new Thickness(10), BorderColor = Drawing.StandardColors.Blue };
            section.Contents.Add(div1);


            var div2 = new Div() { Margins = new Thickness(20), BorderColor = Drawing.StandardColors.Aqua };
            div1.Contents.Add(div2);

            div2.Contents.Add(new TextLiteral("Sits on the first page"));
            div2.Contents.Add(new PageBreak());
            div2.Contents.Add(new TextLiteral("Sits on the second page"));


            using (var ms = DocStreams.GetOutputStream("Breaks_NestedPageBreak.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var outer = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(outer);
            Assert.AreEqual(25 + (2 * 10) + (2 * 20), outer.Height);
            var inner = outer.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(inner);
            Assert.AreEqual(25 + (2 * 20), inner.Height);

            var first = inner.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(3, first.Runs.Count);
            var chars = first.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the first page", chars.Characters);

            content = layout.AllPages[1].ContentBlock;

            outer = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(outer);
            Assert.AreEqual(25 + (2 * 10) + (2 * 20), outer.Height);
            inner = outer.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(inner);
            Assert.AreEqual(25 + (2 * 20), inner.Height);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = inner.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(second);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(3, second.Runs.Count);
            chars = second.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the second page", chars.Characters);



        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleColumnBreak()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.ColumnCount = 2;
            doc.Pages.Add(section);


            section.Contents.Add(new TextLiteral("Sits on the first column"));
            section.Contents.Add(new ColumnBreak());
            section.Contents.Add(new TextLiteral("Sits on the second column"));


            using (var ms = DocStreams.GetOutputStream("Breaks_SimpleColumnBreak.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns.Length);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(25, first.Height);
            Assert.AreEqual(3, first.Runs.Count);
            var chars = first.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the first column", chars.Characters);

            content = layout.AllPages[0].ContentBlock;

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[1].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(second);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(3, second.Runs.Count);
            chars = second.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the second column", chars.Characters);



        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedColumnBreak()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.ColumnCount = 2;
            doc.Pages.Add(section);


            var div1 = new Div() { Margins = new Thickness(10), BorderColor = Drawing.StandardColors.Blue };
            section.Contents.Add(div1);


            var div2 = new Div() { Margins = new Thickness(20), BorderColor = Drawing.StandardColors.Aqua };
            div1.Contents.Add(div2);

            div2.Contents.Add(new TextLiteral("Sits on the 1st column"));
            div2.Contents.Add(new ColumnBreak());
            div2.Contents.Add(new TextLiteral("Sits on the 2nd column"));


            using (var ms = DocStreams.GetOutputStream("Breaks_NestedColumnBreak.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns.Length);

            var outer = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(outer);
            Assert.AreEqual(25 + (2 * 10) + (2 * 20), outer.Height);
            Assert.AreEqual((content.Width - 10) / 2.0, outer.Width);
            var inner = outer.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(inner);
            Assert.AreEqual(25 + (2 * 20), inner.Height);
            Assert.AreEqual(((content.Width - 10) / 2.0) - 20, inner.Width);

            var first = inner.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(3, first.Runs.Count);
            var chars = first.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the 1st column", chars.Characters);



            outer = content.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(outer);
            Assert.AreEqual(25 + (2 * 10) + (2 * 20), outer.Height);
            Assert.AreEqual((content.Width - 10) / 2.0, outer.Width);

            inner = outer.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(inner);
            Assert.AreEqual(25 + (2 * 20), inner.Height);
            Assert.AreEqual(((content.Width - 10) / 2.0) - 20, inner.Width);

            //Check the block after to make sure it is on the second column
            var second = inner.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(second);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(3, second.Runs.Count);
            chars = second.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the 2nd column", chars.Characters);



        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedColumnBreakNewPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.ColumnCount = 2;
            doc.Pages.Add(section);


            var div1 = new Div() { Margins = new Thickness(10), BorderColor = Drawing.StandardColors.Blue };
            section.Contents.Add(div1);


            var div2 = new Div() { Margins = new Thickness(20), BorderColor = Drawing.StandardColors.Aqua };
            div1.Contents.Add(div2);

            div2.Contents.Add(new TextLiteral("Sits on the 1st column"));
            div2.Contents.Add(new ColumnBreak());
            div2.Contents.Add(new TextLiteral("Sits on the 2nd column"));
            div2.Contents.Add(new ColumnBreak());
            div2.Contents.Add(new TextLiteral("Sits on the 2nd page"));


            using (var ms = DocStreams.GetOutputStream("Breaks_NestedColumnBreakNewPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns.Length);

            var outer = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(outer);
            Assert.AreEqual(25 + (2 * 10) + (2 * 20), outer.Height);
            Assert.AreEqual((content.Width - 10) / 2.0, outer.Width);

            var inner = outer.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(inner);
            Assert.AreEqual(25 + (2 * 20), inner.Height);
            Assert.AreEqual(((content.Width - 10) / 2.0) - 20, inner.Width);

            var first = inner.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(3, first.Runs.Count);
            var chars = first.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the 1st column", chars.Characters);



            outer = content.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(outer);
            Assert.AreEqual(25 + (2 * 10) + (2 * 20), outer.Height);
            Assert.AreEqual((content.Width - 10) / 2.0, outer.Width);

            inner = outer.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(inner);
            Assert.AreEqual(25 + (2 * 20), inner.Height);
            Assert.AreEqual(((content.Width - 10) / 2.0) - 20, inner.Width);

            //Check the block after to make sure it is on the second column
            var second = inner.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(second);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(3, second.Runs.Count);
            chars = second.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the 2nd column", chars.Characters);

            //Check the block after to make sure it has been pushed to a new 2 column page

            content = layout.AllPages[1].ContentBlock;

            Assert.AreEqual(2, content.Columns.Length);

            outer = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(outer);
            Assert.AreEqual(25 + (2 * 10) + (2 * 20), outer.Height);
            Assert.AreEqual((content.Width - 10) / 2.0, outer.Width);

            inner = outer.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(inner);
            Assert.AreEqual(25 + (2 * 20), inner.Height);
            Assert.AreEqual(((content.Width - 10) / 2.0) - 20, inner.Width);

            var third = inner.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(third);
            Assert.AreEqual(25, third.Height);
            Assert.AreEqual(3, third.Runs.Count);
            chars = third.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the 2nd page", chars.Characters);

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedColumnBreakInColumnBreak()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.ColumnCount = 2;
            doc.Pages.Add(section);


            var div1 = new Div() { Margins = new Thickness(10), BorderColor = Drawing.StandardColors.Blue };
            section.Contents.Add(div1);


            var div2 = new Div() { Margins = new Thickness(20), BorderColor = Drawing.StandardColors.Aqua };
            div2.ColumnCount = 2;
            div1.Contents.Add(div2);

            div2.Contents.Add(new TextLiteral("1.1.1"));
            div2.Contents.Add(new ColumnBreak());
            div2.Contents.Add(new TextLiteral("1.1.2"));
            div2.Contents.Add(new ColumnBreak());
            div2.Contents.Add(new TextLiteral("1.2.1"));
            div2.Contents.Add(new ColumnBreak());
            div2.Contents.Add(new TextLiteral("1.2.2"));
            div2.Contents.Add(new ColumnBreak());
            div2.Contents.Add(new TextLiteral("2.1.1"));
            div2.Contents.Add(new ColumnBreak());
            div2.Contents.Add(new TextLiteral("2.1.2"));
            div2.Contents.Add(new ColumnBreak());
            div2.Contents.Add(new TextLiteral("2.2.1"));
            div2.Contents.Add(new ColumnBreak());
            div2.Contents.Add(new TextLiteral("2.2.2"));


            using (var ms = DocStreams.GetOutputStream("Breaks_NestedColumnInColumnBreak.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);

            for (var pg = 1; pg <= 2; pg++)
            {
                var content = layout.AllPages[pg - 1].ContentBlock;
                Assert.AreEqual(2, content.Columns.Length);

                for (var col = 1; col <= 2; col++)
                {
                    var outer = content.Columns[col - 1].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(outer);
                    Assert.AreEqual(25 + (2 * 10) + (2 * 20), outer.Height); //line height + 2 margins
                    Assert.AreEqual((content.Width - 10) / 2.0, outer.Width); //page width - 1 margin

                    var inner = outer.Columns[0].Contents[0] as PDFLayoutBlock;

                    Assert.IsNotNull(inner);
                    Assert.AreEqual(25 + (2 * 20), inner.Height); //line height + 1 margin
                    Assert.AreEqual(((content.Width - 10) / 2.0) - 20, inner.Width); //page width minus alley / 2 - outer margins

                    for (var sub = 1; sub <= 2; sub++)
                    {
                        var first = inner.Columns[sub - 1].Contents[0] as PDFLayoutLine;
                        Assert.IsNotNull(first);
                        Assert.AreEqual(3, first.Runs.Count);
                        var chars = first.Runs[1] as PDFTextRunCharacter;
                        var expected = pg + "." + col + "." + sub ;
                        Assert.AreEqual(expected, chars.Characters);
                    }

                }


            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleLineBreak()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            doc.Pages.Add(section);


            section.Contents.Add(new TextLiteral("Sits on the first line"));
            section.Contents.Add(new LineBreak());
            section.Contents.Add(new TextLiteral("Sits on the second line"));


            using (var ms = DocStreams.GetOutputStream("Breaks_SimpleLineBreak.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(24, first.Height);
            Assert.AreEqual(3, first.Runs.Count);
            var chars = first.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the first line", chars.Characters);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(second);
            Assert.AreEqual(24, second.Height);
            Assert.AreEqual(3, second.Runs.Count);
            chars = second.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the second line", chars.Characters);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleDoubleLineBreak()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            doc.Pages.Add(section);


            section.Contents.Add(new TextLiteral("Sits on the first line"));
            section.Contents.Add(new LineBreak());
            section.Contents.Add(new LineBreak());
            section.Contents.Add(new TextLiteral("Sits on the third line"));


            using (var ms = DocStreams.GetOutputStream("Breaks_SimpleDoubleLineBreak.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(3, content.Columns[0].Contents.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(24, first.Height);
            Assert.AreEqual(3, first.Runs.Count);
            var chars = first.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the first line", chars.Characters);

            var space = content.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.AreEqual(1, space.Runs.Count);
            Assert.AreEqual(24, space.Height);
            Assert.IsInstanceOfType(space.Runs[0], typeof(PDFTextRunSpacer));

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(second);
            Assert.AreEqual(24, second.Height);
            Assert.AreEqual(3, second.Runs.Count);
            chars = second.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the third line", chars.Characters);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void MultipleLineBreakExplicitLeading()
        {
            int BreakCount = 19;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 30;
            doc.Pages.Add(section);


            section.Contents.Add(new TextLiteral("Sits on the first line"));

            for (var i = 0; i < BreakCount; i++)
            {
                section.Contents.Add(new LineBreak());
            }
            section.Contents.Add(new TextLiteral("Sits on the " + (BreakCount + 1) + "th line"));


            using (var ms = DocStreams.GetOutputStream("Breaks_MultipleLineBreakExplicitLeading.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(BreakCount + 1, content.Columns[0].Contents.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(30, first.Height);
            Assert.AreEqual(3, first.Runs.Count);
            var chars = first.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the first line", chars.Characters);

            for (int i = 1; i < BreakCount; i++)
            {
                var space = content.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.AreEqual(1, space.Runs.Count);
                Assert.AreEqual(30, space.Height);
                Assert.IsInstanceOfType(space.Runs[0], typeof(PDFTextRunSpacer));
            }
            //Check the block after to make sure it is ignoring the positioned region.
            var last = content.Columns[0].Contents[BreakCount] as PDFLayoutLine;
            Assert.IsNotNull(last);
            Assert.AreEqual(30, last.Height);
            Assert.AreEqual(3, last.Runs.Count);
            chars = last.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the " + (BreakCount + 1) + "th line", chars.Characters);
        }



        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void MultipleLineBreakVariousSizes()
        {
            int BreakCount = 19;
            double[] LineHeights = new double[] { 24,
                2 * 1.2, 2 * 1.2,
                24.0, 24.0, 24.0,
                10.0 * 1.2, 10.0 * 1.2,
                24.0, 24.0, 24.0,
                18.0 * 1.2, 18.0 * 1.2,
                24.0, 24.0, 24.0,
                26.0 * 1.2, 26.0 * 1.2,
                24.0, 24.0, 24.0,
                34.0 * 1.2, 34.0 * 1.2,
                24.0, 24.0};

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            doc.Pages.Add(section);

            int lineIndex = 0;
            Span span = new Span();
            span.Contents.Add(new TextLiteral((lineIndex++) + ". Sits on the 1st line in 20pts"));
            span.Contents.Add(new LineBreak());
            section.Contents.Add(span);
           
            for (var i = 1; i < BreakCount; i++)
            {
                if (i % 4 == 1)
                {
                    span = new Span();
                    span.FontSize = i * 2;
                    span.Contents.Add(new TextLiteral((lineIndex++) + ".Text in " + (i * 2) + "pts on a new line"));
                    span.Contents.Add(new LineBreak()); //should be the same as the font size
                    span.Contents.Add(new TextLiteral((lineIndex) + ".After the break at " + (i * 2) + "pts"));
                    section.Contents.Add(span);
                }
                section.Contents.Add(new LineBreak()); // should either be same as the font size, or the outer size.
                lineIndex++;
            }
            section.Contents.Add(new TextLiteral("Sits on the " + (BreakCount + 1) + "th line in 20pts"));


            using (var ms = DocStreams.GetOutputStream("Breaks_MultipleLineBreakMultipleSizes.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock.Columns[0];

            Assert.AreEqual(LineHeights.Length, content.Contents.Count);
 
            for (int i = 0; i < LineHeights.Length; i++)
            {
                var line = content.Contents[i] as PDFLayoutLine;
                Assert.AreEqual(LineHeights[i], line.Height, "Line Height for '" + i + "' did not match");
            }
            
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedLineBreakMultiColumn()
        {
            int BreakCount = 19;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 30;
            doc.Pages.Add(section);

            var div = new Div() { ColumnCount = 2, MaximumHeight = 500, BorderColor = Drawing.StandardColors.Aqua, Margins = new Thickness(5) };
            section.Contents.Add(div);

            var div2 = new Div() { BorderColor = Drawing.StandardColors.Blue, Margins = new Thickness(5) };
            div.Contents.Add(div2);

            div2.Contents.Add(new TextLiteral("Sits on the first line"));

            for (var i = 0; i < BreakCount; i++)
            {
                div2.Contents.Add(new LineBreak());
            }
            div2.Contents.Add(new TextLiteral("Sits on the " + (BreakCount + 1) + "th line"));


            using (var ms = DocStreams.GetOutputStream("Breaks_NestedLineBreakMultiColumn.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var outer = content.Columns[0].Contents[0] as PDFLayoutBlock;
            
            Assert.AreEqual(2, outer.Columns.Length);
            Assert.AreEqual(1, outer.Columns[0].Contents.Count);
            Assert.IsTrue(500.0 >= outer.Height.PointsValue);

            var inner = outer.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsTrue(500.0 >= inner.Height.PointsValue);
            var col1count = (int)Math.Floor(500.0 / 30.0);
            Assert.AreEqual(col1count, inner.Columns[0].Contents.Count);

            
            var first = inner.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(30, first.Height);
            Assert.AreEqual(3, first.Runs.Count);
            var chars = first.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the first line", chars.Characters);

            for (int i = 1; i < col1count; i++)
            {
                var space = inner.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.AreEqual(1, space.Runs.Count);
                Assert.AreEqual(30, space.Height);
                Assert.IsInstanceOfType(space.Runs[0], typeof(PDFTextRunSpacer));
            }

            //Assert.Inconclusive();

            var remainder = BreakCount - col1count;

            Assert.AreEqual(1, outer.Columns[1].Contents.Count);
            inner = outer.Columns[1].Contents[0] as PDFLayoutBlock;

            first = inner.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(30, first.Height);
            Assert.AreEqual(1, first.Runs.Count);
            
            for (var i = 1; i < remainder; i++)
            {
                var space = inner.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.AreEqual(1, space.Runs.Count);
                Assert.AreEqual(30, space.Height);
                Assert.IsInstanceOfType(space.Runs[0], typeof(PDFTextRunSpacer));
            }

            //Check the block after to make sure it is ignoring the positioned region.
            var last = inner.Columns[0].Contents[remainder] as PDFLayoutLine;
            Assert.IsNotNull(last);
            Assert.AreEqual(30, last.Height);
            Assert.AreEqual(3, last.Runs.Count);
            chars = last.Runs[1] as PDFTextRunCharacter;
            Assert.AreEqual("Sits on the " + (BreakCount + 1) + "th line", chars.Characters);
        }
    }
}
