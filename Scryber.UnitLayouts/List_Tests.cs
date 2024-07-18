using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Layout;
using Scryber.PDF;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class List_Tests
    {
        const string TestCategoryName = "Layout";
        const int LabelPadding = 2;

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
        public void SimpleOrderedList()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            section.Contents.Add(ol);

            

            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));
                
                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleList.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);

                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual((i + 1).ToString(), chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
            }

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleUnOrderedList()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ul = new ListUnordered();
            section.Contents.Add(ul);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ul.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("UnorderedList_SimpleList.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);


            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);

                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual("•", chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListExplicitDecimal()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.Decimals;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleListExplicitDecimal.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual((i + 1).ToString(), chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListAlphaUpper()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            string[] ItemValues = new[] { "A", "B", "C", "D", "E" };
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.UppercaseLetters;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleListAlphaUpper.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(ItemValues[i], chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
            }

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListAlphaLower()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            string[] ItemValues = new[] { "a", "b", "c", "d", "e" };
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.LowercaseLetters;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleListAlphaLower.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(ItemValues[i], chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
            }

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListRomanUpper()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            string[] ItemValues = new[] { "I", "II", "III", "IV", "V" };
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.UppercaseRoman;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleListRomanUpper.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(ItemValues[i], chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
            }

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListRomanLower()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            string[] ItemValues = new[] { "i", "ii", "iii", "iv", "v" };
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.LowercaseRoman;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleListRomanLower.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(ItemValues[i], chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListDecimalPostFix()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.Decimals;
            ol.NumberPostfix = ".";
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleListDecimalPostfix.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual((i + 1).ToString() + ".", chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListAlphaLowerPreAndPostFix()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            string[] ItemValues = new[] { "1a-", "1b-", "1c-", "1d-", "1e-" };
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.LowercaseLetters;
            ol.NumberPostfix = "-";
            ol.NumberPrefix = "1";
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleListAlphaLowerPreAndPostFix.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(ItemValues[i], chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListAlphaLowerWideInsetLeft()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            string[] ItemValues = new[] { "1a-", "1b-", "1c-", "1d-", "1e-" };
            const double DefaultNumberWidth = 100.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.LowercaseLetters;
            ol.NumberPostfix = "-";
            ol.NumberPrefix = "1";
            ol.NumberAlignment = HorizontalAlignment.Left;
            ol.NumberInset = DefaultNumberWidth;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleListAlphaLowerWideInsetLeft.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);

                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(ItemValues[i], chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(0.0, start.StartTextCursor.Width);
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListLongAlphaLowerNarrowInsetLeft()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            string[] ItemValues = new[] { "1...a-", "1...b-", "1...c-", "1...d-", "1...e-" };
            const double DefaultNumberWidth = 10.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.LowercaseLetters;
            ol.NumberPostfix = "-";
            ol.NumberPrefix = "1...";
            ol.NumberAlignment = HorizontalAlignment.Right;
            ol.NumberInset = DefaultNumberWidth;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleListLongAlphaLowerNarrowInsetLeft.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);

                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                //Even though we are narrow, we should not wrap, but we should clip (keep going).
                Assert.AreEqual(ItemValues[i], chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(0.0, start.StartTextCursor.Width);
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListNone()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.None;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleListNoNumbers.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(0, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                

                
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(3, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                //No Number to check
                //But still inset - as per html

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);

            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleLongOrderedListWithLeadingMultiPage()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 40;
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.TextLeading = 18;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_LongListWithLeading.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(2, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            var firstPageItemCount = olBlock.Columns[0].Contents.Count - 1;

            for (var i = 0; i < firstPageItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);

                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);
                Assert.AreEqual(numBlock, posRun.Region.Contents[0]);

                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual((i + 1).ToString(), chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding); 
            }

            var emptyLastBlock = olBlock.Columns[0].Contents[firstPageItemCount] as PDFLayoutBlock;
            Assert.AreEqual(0, emptyLastBlock.PositionedRegions.Count);

            var secondPageItemCount = ItemCount - firstPageItemCount;
            pg = layout.AllPages[1];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);
            Assert.AreEqual(secondPageItemCount, olBlock.Columns[0].Contents.Count);

            for (var i = 0; i < secondPageItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);


                Assert.AreEqual(4, itemLine.Runs.Count, "Runs did not match for item " + i);

                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + (i + firstPageItemCount), chars.Characters);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(numBlock, posRun.Region.Contents[0]);

                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual((i + firstPageItemCount + 1).ToString(), chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding); 
            }
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleLongOrderedListWithLeadingMultiColumn()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 40;
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.TextLeading = 18;
            section.ColumnCount = 2;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_LongListWithLeadingColumns.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);

            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            var firstColumnItemCount = olBlock.Columns[0].Contents.Count - 1;

            for (var i = 0; i < firstColumnItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);

                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width); 
                Assert.AreEqual("Item " + i, chars.Characters);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);
                Assert.AreEqual(numBlock, posRun.Region.Contents[0]);

                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual((i + 1).ToString(), chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding); 
            }

            var emptyLastBlock = olBlock.Columns[0].Contents[firstColumnItemCount] as PDFLayoutBlock;
            Assert.AreEqual(0, emptyLastBlock.PositionedRegions.Count);

            var secondColumnItemCount = ItemCount - firstColumnItemCount;
            
            olBlock = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);
            Assert.AreEqual(secondColumnItemCount, olBlock.Columns[0].Contents.Count);

            for (var i = 0; i < secondColumnItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);

                

                Assert.AreEqual(4, itemLine.Runs.Count, "Runs did not match for item " + i);

                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                //Start text cursor Width is based on the page
                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth + (PageWidth / 2.0) + (section.AlleyWidth / 2.0), start.StartTextCursor.Width);
                Assert.AreEqual("Item " + (i + firstColumnItemCount), chars.Characters);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual(numBlock, posRun.Region.Contents[0]);

                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual((i + firstColumnItemCount + 1).ToString(), chars.Characters);

                //Make sure we are right aligned by default - based on the page
                Assert.AreEqual(DefaultNumberWidth + (PageWidth / 2.0) + (section.AlleyWidth / 2.0), start.StartTextCursor.Width + chars.Width + LabelPadding, "Text start for second column in index " + i + " failed");
            }
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedOrderedListExplicitDecimal()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            int[] Nests = new int[] { 1, 3 };
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.Decimals;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

                if (Nests.Contains(i))
                {
                    var ol2 = new ListOrdered();
                    li.Contents.Add(ol2);

                    for (var j = 0; j < ItemCount; j++)
                    {
                        var li2 = new ListItem();
                        li2.Contents.Add("Inner Item " + i + "." + j);
                        ol2.Items.Add(li2);
                    }
                }

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_NestedListExplicitDecimal.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                if (Nests.Contains(i))
                {
                    //With nested we have the item text, the nested list, and then the positioned run line 
                    Assert.AreEqual(2, itemBlock.Columns[0].Contents.Count);
                    Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                    var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;

                    var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;
                    var nestedBlock = itemBlock.Columns[0].Contents[1] as PDFLayoutBlock;
                    

                    Assert.AreEqual(4, itemLine.Runs.Count);



                    var start = itemLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    var end = itemLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                    Assert.IsNotNull(posRun);


                    AssertOLBlockContent(nestedBlock, DefaultNumberWidth, DefaultGutterWidth, 2, new string[] { "1", "2", "3", "4", "5" }, "Inner Item " + i.ToString() + ".");

                    Assert.AreEqual(nestedBlock.Height + itemLine.Height, itemBlock.Height);
                }
                else
                {
                    Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                    Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                    var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                    var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    Assert.IsNotNull(numBlock);
                    Assert.IsNotNull(itemLine);

                    Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                    Assert.AreEqual(4, itemLine.Runs.Count);



                    var start = itemLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    var end = itemLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                    Assert.IsNotNull(posRun);

                    Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                    Assert.AreEqual("Item " + i, chars.Characters);


                    Assert.AreEqual(1, numBlock.Columns.Length);
                    Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                    var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.AreEqual(3, numLine.Runs.Count);

                    start = numLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    chars = numLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    end = numLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    Assert.AreEqual((i + 1).ToString(), chars.Characters);

                    //Make sure we are right aligned by default
                    Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
                }
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedUnorderedListExplicitBullet()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            int[] Nests = new int[] { 1, 3 };
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            //even though its an ordered list the style is bullet
            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.Bullet;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

                if (Nests.Contains(i))
                {
                    var ol2 = new ListOrdered();
                    ol2.NumberingStyle = ListNumberingGroupStyle.Bullet;
                    li.Contents.Add(ol2);

                    for (var j = 0; j < ItemCount; j++)
                    {
                        var li2 = new ListItem();
                        li2.Contents.Add("Inner Item " + i + "." + j);
                        ol2.Items.Add(li2);
                    }
                }

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_NestedListExplicitBullet.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            //var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            //Assert.IsNotNull(rowBlock);
            //Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            //Assert.AreEqual(CellHeight, rowBlock.Height);

            //Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                if (Nests.Contains(i))
                {
                    //With nested we have the item text, the nested list, and then the positioned run line 
                    Assert.AreEqual(2, itemBlock.Columns[0].Contents.Count);
                    Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                    var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;

                    var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;
                    var nestedBlock = itemBlock.Columns[0].Contents[1] as PDFLayoutBlock;


                    Assert.AreEqual(4, itemLine.Runs.Count);



                    var start = itemLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    var end = itemLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                    Assert.IsNotNull(posRun);


                    AssertOLBlockContent(nestedBlock, DefaultNumberWidth, DefaultGutterWidth, 2, new string[] { "•", "•", "•", "•", "•" }, "Inner Item " + i.ToString() + ".");

                    Assert.AreEqual(nestedBlock.Height + itemLine.Height, itemBlock.Height);
                }
                else
                {
                    Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                    Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                    var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                    var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    Assert.IsNotNull(numBlock);
                    Assert.IsNotNull(itemLine);

                    Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                    Assert.AreEqual(4, itemLine.Runs.Count);



                    var start = itemLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    var end = itemLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                    Assert.IsNotNull(posRun);

                    Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                    Assert.AreEqual("Item " + i, chars.Characters);


                    Assert.AreEqual(1, numBlock.Columns.Length);
                    Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                    var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.AreEqual(3, numLine.Runs.Count);

                    start = numLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    chars = numLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    end = numLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    Assert.AreEqual("•", chars.Characters);

                    //Make sure we are right aligned by default
                    Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
                }
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleDefinitionList()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;

            const double DefaultNumberWidth = 40.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.TextLeading = 15;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var dl = new ListDefinition();
            
            section.Contents.Add(dl);

            for (var i = 0; i < ItemCount; i++)
            {
                var dt = new ListDefinitionTerm();

                var dd = new ListDefinitionItem();
                
                dt.Contents.Add(new TextLiteral("Term " + i));
                dt.BorderColor = Drawing.StandardColors.Blue;
                dl.Items.Add(dt);

                dd.Contents.Add(new TextLiteral("Item " + i));
                dd.BorderColor = Drawing.StandardColors.Red;
                dl.Items.Add(dd);
                

            }

            using (var ms = DocStreams.GetOutputStream("DefinitionList_SimpleList.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var dlBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(dlBlock);

            Assert.AreEqual(1, dlBlock.Columns.Length);
            Assert.AreEqual(ItemCount * 2, dlBlock.Columns[0].Contents.Count);

            for (var i = 0; i < ItemCount; i++)
            {

                var termBlock = dlBlock.Columns[0].Contents[i * 2] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(termBlock);

                Assert.AreEqual(1, termBlock.Columns[0].Contents.Count);
                Assert.AreEqual(0, termBlock.PositionedRegions.Count);

                var termLine = termBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(termLine);

                Assert.AreEqual(3, termLine.Runs.Count);

                var start = termLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = termLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = termLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(0, start.StartTextCursor.Width);
                Assert.AreEqual("Term " + i, chars.Characters);

                var defnBlock = dlBlock.Columns[0].Contents[(i * 2) + 1] as PDFLayoutBlock;

                Assert.AreEqual(1, defnBlock.Columns.Length);
                Assert.AreEqual(1, defnBlock.Columns[0].Contents.Count);
                var defnLine = defnBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, defnLine.Runs.Count);

                start = defnLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = defnLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = defnLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual("Item " + i, chars.Characters);

                //Make sure we are right aligned by default
                Assert.AreEqual(DefaultNumberWidth, defnBlock.Position.Margins.Left);
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedOrderedListConcatenated()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            int[] Nests = new int[] { 1, 3 };
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.Decimals;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

                if (Nests.Contains(i))
                {
                    var ol2 = new ListOrdered();
                    ol2.NumberingStyle = ListNumberingGroupStyle.LowercaseRoman;
                    ol2.ConcatenateNumberWithParent = true;
                    ol2.NumberPrefix = "."; 
                    li.Contents.Add(ol2);

                    for (var j = 0; j < ItemCount; j++)
                    {
                        var li2 = new ListItem();
                        li2.Contents.Add("Inner Item " + i + "." + j);
                        ol2.Items.Add(li2);
                    }
                }

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_NestedListConcatenated.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                if (Nests.Contains(i))
                {
                    //With nested we have the item text, the nested list, and then the positioned run line 
                    Assert.AreEqual(2, itemBlock.Columns[0].Contents.Count);
                    Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                    var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                    var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    var nestedBlock = itemBlock.Columns[0].Contents[1] as PDFLayoutBlock;
                    

                    Assert.AreEqual(4, itemLine.Runs.Count);



                    var start = itemLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    var end = itemLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                    Assert.IsNotNull(posRun);

                    var prefix = (i + 1) + ".";
                    AssertOLBlockContent(nestedBlock, DefaultNumberWidth, DefaultGutterWidth, 2, new string[] { prefix + "i", prefix + "ii", prefix + "iii", prefix + "iv", prefix + "v" }, "Inner Item " + i.ToString() + ".");

                    Assert.AreEqual(nestedBlock.Height + itemLine.Height, itemBlock.Height);
                }
                else
                {
                    Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                    Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                    var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                    var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    Assert.IsNotNull(numBlock);
                    Assert.IsNotNull(itemLine);

                    Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                    Assert.AreEqual(4, itemLine.Runs.Count);



                    var start = itemLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    var end = itemLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                    Assert.IsNotNull(posRun);

                    Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                    Assert.AreEqual("Item " + i, chars.Characters);


                    Assert.AreEqual(1, numBlock.Columns.Length);
                    Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                    var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.AreEqual(3, numLine.Runs.Count);

                    start = numLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    chars = numLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    end = numLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    Assert.AreEqual((i + 1).ToString(), chars.Characters);

                    //Make sure we are right aligned by default
                    Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
                }
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void DeepNestedOrderedListConcatenated()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            int[] Nests = new int[] { 1 };
            const double ExplicitNumberWidth = 50.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.Decimals;
            ol.NumberInset = ExplicitNumberWidth;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

                if (i == 1)
                {
                    var ol2 = new ListOrdered();
                    ol2.NumberingStyle = ListNumberingGroupStyle.UppercaseLetters;
                    ol2.ConcatenateNumberWithParent = true;
                    ol2.NumberInset = ExplicitNumberWidth;
                    ol2.NumberPrefix = ".";
                    li.Contents.Add(ol2);

                    for (var j = 0; j < ItemCount; j++)
                    {
                        var li2 = new ListItem();
                        li2.Contents.Add("Inner Item " + i + "." + j);
                        ol2.Items.Add(li2);

                        if (j == 1)
                        {
                            var ol3 = new ListOrdered();
                            ol3.NumberingStyle = ListNumberingGroupStyle.LowercaseLetters;
                            ol3.ConcatenateNumberWithParent = true;
                            ol3.NumberInset = ExplicitNumberWidth;
                            ol3.NumberPrefix = ".";
                            li2.Contents.Add(ol3);

                            for (var k = 0; k < ItemCount; k++)
                            {
                                var li3 = new ListItem();
                                li3.Contents.Add("Inner Item " + i + "." + j + "." + k);
                                ol3.Items.Add(li3);

                                if (k == 1)
                                {
                                    var ol4 = new ListOrdered();
                                    ol4.NumberingStyle = ListNumberingGroupStyle.LowercaseRoman;
                                    ol4.ConcatenateNumberWithParent = true;
                                    ol4.NumberInset = ExplicitNumberWidth;
                                    ol4.NumberPrefix = "-";
                                    li3.Contents.Add(ol4);

                                    for (var l = 0; l < ItemCount; l++)
                                    {
                                        var li4 = new ListItem();
                                        li4.Contents.Add("Inner Item " + i + "." + j + "." + k + "-" + l);
                                        ol4.Items.Add(li4);
                                    }
                                }
                            }
                        }
                    }
                }

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_DeepNestedListConcatenated.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var ol1Block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ol1Block);

            Assert.AreEqual(1, ol1Block.Columns.Length);
            Assert.AreEqual(ItemCount, ol1Block.Columns[0].Contents.Count);

            var li1Block = ol1Block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(li1Block);

            Assert.AreEqual(2, li1Block.Columns[0].Contents.Count); //line with item and positioned region + inner block = 2
            Assert.AreEqual(1, li1Block.PositionedRegions.Count);


            var ol2Block = li1Block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(ol2Block);

            Assert.AreEqual(1, ol2Block.Columns.Length);
            Assert.AreEqual(ItemCount, ol2Block.Columns[0].Contents.Count);

            var li2Block = ol2Block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(li2Block);

            Assert.AreEqual(2, li2Block.Columns[0].Contents.Count);
            Assert.AreEqual(1, li2Block.PositionedRegions.Count);

            
            var ol3Block = li2Block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(ol3Block);

            Assert.AreEqual(1, ol3Block.Columns.Length);
            Assert.AreEqual(ItemCount, ol3Block.Columns[0].Contents.Count);

            var li3Block = ol3Block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(li3Block);

            Assert.AreEqual(2, li3Block.Columns[0].Contents.Count);
            Assert.AreEqual(1, li3Block.PositionedRegions.Count);

            var ol4Block = li3Block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(ol4Block);

            Assert.AreEqual(1, ol4Block.Columns.Length);
            Assert.AreEqual(ItemCount, ol4Block.Columns[0].Contents.Count);


            AssertOLBlockContent(ol4Block, ExplicitNumberWidth, DefaultGutterWidth, 4, new string[] { "2.B.b-i", "2.B.b-ii","2.B.b-iii","2.B.b-iv","2.B.b-v" }, "Inner Item 1.1.1-");
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedLongOrderedListExplicitDecimal()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 25;
            int[] Nests = new int[] { 20 };
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.Decimals;
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

                if (Nests.Contains(i))
                {
                    var ol2 = new ListOrdered();
                    li.Contents.Add(ol2);

                    for (var j = 0; j < 5; j++)
                    {
                        var li2 = new ListItem();
                        li2.Contents.Add("Inner Item " + i + "." + j);
                        ol2.Items.Add(li2);
                    }
                }

            }


            using (var ms = DocStreams.GetOutputStream("OrderedList_NestedLongListExplicitDecimal.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

           

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                if (Nests.Contains(i))
                {
                    //With nested we have the item text, the nested list, and then the positioned run line 
                    Assert.AreEqual(2, itemBlock.Columns[0].Contents.Count);
                    Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                    var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                    var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    var nestedBlock = itemBlock.Columns[0].Contents[1] as PDFLayoutBlock;
                    //var posLine = itemBlock.Columns[0].Contents[2] as PDFLayoutLine;

                    Assert.AreEqual(4, itemLine.Runs.Count);



                    var start = itemLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    var end = itemLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                    Assert.IsNotNull(posRun);


                    AssertOLBlockContent(nestedBlock, DefaultNumberWidth, DefaultGutterWidth, 2, new string[] { "1", "2", "3", "4", "5" }, "Inner Item " + i.ToString() + ".");

                    Assert.AreEqual(nestedBlock.Height + itemLine.Height, itemBlock.Height);
                }
                else
                {
                    Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                    Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                    var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                    var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    Assert.IsNotNull(numBlock);
                    Assert.IsNotNull(itemLine);

                    Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                    Assert.AreEqual(4, itemLine.Runs.Count);



                    var start = itemLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    var end = itemLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                    Assert.IsNotNull(posRun);

                    Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth, start.StartTextCursor.Width);
                    Assert.AreEqual("Item " + i, chars.Characters);


                    Assert.AreEqual(1, numBlock.Columns.Length);
                    Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                    var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.AreEqual(3, numLine.Runs.Count);

                    start = numLine.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);

                    chars = numLine.Runs[1] as PDFTextRunCharacter;
                    Assert.IsNotNull(chars);

                    end = numLine.Runs[2] as PDFTextRunEnd;
                    Assert.IsNotNull(end);

                    Assert.AreEqual((i + 1).ToString(), chars.Characters);

                    //Make sure we are right aligned by default
                    Assert.AreEqual(DefaultNumberWidth, start.StartTextCursor.Width + chars.Width + LabelPadding);
                }
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedListOverflowPage()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 30;
            const int NestedCount = 12;

            int[] Nests = new int[] { 25 };
            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.Margins = new Drawing.Thickness(10);
            section.BorderColor = Drawing.StandardColors.Aqua;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.Decimals;
            ol.FullWidth = true;
            ol.BorderColor = Drawing.StandardColors.Fuchsia;
            section.Contents.Add(ol);

            ListItem liOverflowingOwner = null;

            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));
                li.BorderColor = Drawing.StandardColors.Blue;
                li.Padding = new Drawing.Thickness(2);
                li.FullWidth = true;
                li.Margins = Thickness.Empty();
                ol.Items.Add(li);

                if (Nests.Contains(i))
                {
                    liOverflowingOwner = li;
                    var ol2 = new ListOrdered();
                    ol2.NumberingStyle = ListNumberingGroupStyle.Decimals;
                    ol2.BorderColor = Drawing.StandardColors.Olive;
                    ol2.FullWidth = true;
                    li.Contents.Add(ol2);

                    for (var j = 0; j < NestedCount; j++)
                    {
                        var li2 = new ListItem();
                        li2.Contents.Add("Inner Item " + i + "." + j);
                        li2.BorderColor = Drawing.StandardColors.Red;
                        li2.Padding = new Drawing.Thickness(2);
                        li2.FullWidth = true;
                        ol2.Items.Add(li2);
                    }
                }

            }

            using (var ms = DocStreams.GetOutputStream("OrderedList_NestedLongListOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //known layout offsets.
            const int Page1FirstItemCount = 26;
            const int Page1InnerItemCount = 4;

            Assert.AreEqual(2, layout.AllPages.Count);
            var pg1 = layout.AllPages[0];
            Assert.AreEqual(1, pg1.ContentBlock.Columns[0].Contents.Count);

            var ol1Block = pg1.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ol1Block);
            Assert.AreEqual(0, ol1Block.BlockRepeatIndex);
            Assert.AreEqual(1, ol1Block.Columns.Length);
            Assert.AreEqual(Page1FirstItemCount, ol1Block.Columns[0].Contents.Count);


            var li1Block = ol1Block.Columns[0].Contents[Page1FirstItemCount - 1] as PDFLayoutBlock;
            Assert.AreEqual(0, li1Block.BlockRepeatIndex);
            Assert.AreEqual(liOverflowingOwner, li1Block.Owner);

            Assert.AreEqual(1, li1Block.PositionedRegions.Count);
            Assert.AreEqual(2, li1Block.Columns[0].Contents.Count); //line + nested OL
            Assert.AreEqual(PageWidth - (2* 10), li1Block.Width); //Page - margins

            var ol2Block = li1Block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(ol2Block);
            Assert.AreEqual(PageWidth - DefaultGutterWidth - DefaultNumberWidth - (2 * (10 + 2)), ol2Block.Width);//Page - margins and li padding and inset
            Assert.AreEqual(Page1InnerItemCount, ol2Block.Columns[0].Contents.Count);

            var li2Block = ol2Block.Columns[0].Contents[Page1InnerItemCount - 1] as PDFLayoutBlock;
            Assert.AreEqual(1, li2Block.PositionedRegions.Count);
            Assert.AreEqual(1, li2Block.Columns[0].Contents.Count);

            Assert.AreEqual(PageWidth - DefaultGutterWidth - DefaultNumberWidth - (2 * (10 + 2)), li2Block.Width);

            var numBlock = li2Block.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
            var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(numLine);
            Assert.AreEqual(3, numLine.Runs.Count);
            var numChars = numLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(numChars);
            Assert.AreEqual(Page1InnerItemCount.ToString(), numChars.Characters);

            var labelLine = li2Block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(labelLine);
            Assert.AreEqual(PageWidth - ((DefaultGutterWidth + DefaultNumberWidth) * 2) - (2 * (10 + 2 + 2)), labelLine.FullWidth); //Page - 2 * (margins and li padding and inset)

            Assert.AreEqual(4, labelLine.Runs.Count); //3 for text + positioned run
            var labelChars = labelLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(labelChars);
            Assert.AreEqual("Inner Item " + (Page1FirstItemCount-1) + "." + (Page1InnerItemCount-1), labelChars.Characters);


            var pg2 = layout.AllPages[1];
            Assert.AreEqual(1, pg2.ContentBlock.Columns[0].Contents.Count);

            ol1Block = pg2.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ol1Block);
            Assert.AreEqual(1, ol1Block.BlockRepeatIndex);

            Assert.AreEqual(1, ol1Block.Columns.Length);

            const int Page2FirstItemCount = 1 + (ItemCount - Page1FirstItemCount); //remainder of inner + 4 outer items
            const int Page2InnerItemCount = NestedCount - Page1InnerItemCount;

            Assert.AreEqual(1, ol1Block.Columns.Length);
            Assert.AreEqual(Page2FirstItemCount, ol1Block.Columns[0].Contents.Count);

            li1Block = ol1Block.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, li1Block.BlockRepeatIndex);
            Assert.AreEqual(liOverflowingOwner, li1Block.Owner);
            Assert.AreEqual(PageWidth - (2 * 10), li1Block.Width); //Page - margins
            Assert.AreEqual(1, li1Block.Columns[0].Contents.Count);

            ol2Block = li1Block.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ol2Block);
            Assert.AreEqual(PageWidth - DefaultGutterWidth - DefaultNumberWidth - (2 * (10 + 2)), ol2Block.Width);//Page - margins and li padding and inset
            Assert.AreEqual(Page2InnerItemCount, ol2Block.Columns[0].Contents.Count);
            Assert.AreEqual(1, ol2Block.BlockRepeatIndex);

            li2Block = ol2Block.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, li2Block.PositionedRegions.Count);
            Assert.AreEqual(1, li2Block.Columns[0].Contents.Count);

            Assert.AreEqual(PageWidth - DefaultGutterWidth - DefaultNumberWidth - (2 * (10 + 2)), li2Block.Width);


            numBlock = li2Block.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
            numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(numLine);
            Assert.AreEqual(3, numLine.Runs.Count);
            numChars = numLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(numChars);
            Assert.AreEqual((Page1InnerItemCount + 1).ToString(), numChars.Characters);

            labelLine = li2Block.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(labelLine);
            Assert.AreEqual(PageWidth - ((DefaultGutterWidth + DefaultNumberWidth) * 2) - (2 * (10 + 2 + 2)), labelLine.FullWidth); //Page - 2 * (margins and li padding and inset)

            Assert.AreEqual(4, labelLine.Runs.Count); //3 for text + positioned run
            labelChars = labelLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(labelChars);

            Assert.AreEqual("Inner Item " + (Page1FirstItemCount-1) + "." + Page1InnerItemCount, labelChars.Characters);

            var lineHeight = labelLine.Height;
            var ol2Height = (Page2InnerItemCount * (lineHeight.PointsValue + 4)); //inc. padding

            var ol1Height = ol2Height  + ((Page2FirstItemCount-1) * (lineHeight.PointsValue + 4)) + 4; //one less outer item + items with padding + padding around the nested item

            Assert.AreEqual(ol2Height, ol2Block.Height, "For line height '" + lineHeight + "' didnt match " + Page2InnerItemCount +" items");
            Assert.AreEqual(ol1Height, ol1Block.Height, "For line height '" + lineHeight + "' didnt match " + Page2InnerItemCount + " inner items + " + Page2FirstItemCount + " outer items");
        }



        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOrderedListNumberingGroup()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            string[] Item1Values = new[] { "i", "ii", "iii", "iv", "v" };
            string[] Item2Values = new[] { "vi", "vii", "viii", "ix", "x" };

            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;

            doc.Pages.Add(section);


            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.LowercaseRoman;
            ol.NumberingGroup = "Roman";
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

            }

            section.Contents.Add(new PageBreak());

            ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.LowercaseRoman;
            ol.NumberingGroup = "Roman";
            section.Contents.Add(ol);

            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Grouped Item " + i));

                ol.Items.Add(li);

            }

            using (var ms = DocStreams.GetOutputStream("OrderedList_SimpleNumberGroup.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);

            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var ol1Block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ol1Block);

            AssertOLBlockContent(ol1Block, DefaultNumberWidth, DefaultGutterWidth, 1, Item1Values, "Item ");

            pg = layout.AllPages[1];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);

            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var ol2Block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ol2Block);

            AssertOLBlockContent(ol2Block, DefaultNumberWidth, DefaultGutterWidth, 1, Item2Values, "Grouped Item ");

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void NestedOrderedListNumberingGroup()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int ItemCount = 5;
            string[] Item1Values = new[] { "2.i", "2.ii", "2.iii", "2.iv", "2.v" };
            string[] Item2Values = new[] { "4.vi", "4.vii", "4.viii", "4.ix", "4.x" };

            const double DefaultNumberWidth = 30.0;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;

            doc.Pages.Add(section);


            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.Decimals;
            
            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));

                ol.Items.Add(li);

                if (i == 1 || i == 3)
                {
                    //Add the nested grouped lists
                    var ol2 = new ListOrdered();
                    ol2.NumberingStyle = ListNumberingGroupStyle.LowercaseRoman;
                    ol2.ConcatenateNumberWithParent = true;
                    ol2.NumberPrefix = ".";
                    ol2.NumberingGroup = "Roman";
                    li.Contents.Add(ol2);

                    for (var j = 0; j < ItemCount; j++)
                    {
                        var li2 = new ListItem();
                        li2.Contents.Add(new TextLiteral("Grouped Item " + i + "." + j));
                        ol2.Items.Add(li2);

                    }

                }


            }

            

            

            using (var ms = DocStreams.GetOutputStream("OrderedList_NestedNumberGroup.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);

            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olouterBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olouterBlock);
            Assert.AreEqual(ItemCount, olouterBlock.Columns[0].Contents.Count);

            var li1Block = olouterBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            var li3Block = olouterBlock.Columns[0].Contents[3] as PDFLayoutBlock;

            Assert.AreEqual(2, li1Block.Columns[0].Contents.Count);
            Assert.AreEqual(2, li3Block.Columns[0].Contents.Count);

            var olInnerBlock = li1Block.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(olInnerBlock);
            AssertOLBlockContent(olInnerBlock, DefaultNumberWidth, DefaultGutterWidth, 2, Item1Values, "Grouped Item 1.");

            olInnerBlock = li3Block.Columns[0].Contents[1] as PDFLayoutBlock;
            AssertOLBlockContent(olInnerBlock, DefaultNumberWidth, DefaultGutterWidth, 2, Item2Values, "Grouped Item 3.");

        }


        

        

        /// <summary>
        /// Checks the contents of a nested list
        /// </summary>
        /// <param name="olBlock"></param>
        /// <param name="numWidth"></param>
        /// <param name="gutterWidth"></param>
        /// <param name="depth"></param>
        /// <param name="itemNums"></param>
        /// <param name="itemsPrefix"></param>
        private void AssertOLBlockContent(PDFLayoutBlock olBlock, double numWidth, double gutterWidth, int depth, string[] itemNums, string itemsPrefix)
        {
            Assert.AreEqual(itemNums.Length, olBlock.Columns[0].Contents.Count);

            for (var i = 0; i < itemNums.Length; i++)
            {
                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(numWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                Assert.AreEqual(itemsPrefix + i.ToString(), chars.Characters);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);

                Assert.AreEqual((numWidth + gutterWidth) * depth, start.StartTextCursor.Width);

                Assert.AreEqual(numBlock, posRun.Region.Contents[0]);

                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);

                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                Assert.AreEqual(itemNums[i], chars.Characters);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);
            }
        }
    }
}
