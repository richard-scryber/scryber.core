using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Tests border-collapse and border-spacing on tables
    /// </summary>
    [TestClass]
    public class TableBorderModel_Tests
    {
        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private void Layout(string css, string body, object model = null)
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>" + css + "</style></head><body style='margin:0; padding:0'>" + body + "</body></html>";
            _layout = null;
            using (var doc = Document.ParseDocument(new System.IO.StringReader(html), ParseSourceType.DynamicContent))
            {
                if (model != null)
                    doc.Params["model"] = model;
                using (var ms = new System.IO.MemoryStream())
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }
            Assert.IsNotNull(_layout);
        }

        private PDFLayoutBlock Find(string id, PDFLayoutBlock parent = null)
        {
            parent = parent ?? _layout.AllPages[0].ContentBlock;
            foreach (var column in parent.Columns)
            {
                foreach (var item in column.Contents)
                {
                    if (item is PDFLayoutBlock block)
                    {
                        if (block.Owner != null && block.Owner.ID == id)
                            return block;
                        var inner = Find(id, block);
                        if (null != inner)
                            return inner;
                    }
                }
            }
            return null;
        }

        private double Side(string id, Sides side)
        {
            var block = Find(id);
            Assert.IsNotNull(block, "Cell '" + id + "' was not found");
            var borders = block.FullStyle.CreateBorderPen();
            PDFPenBorders b = borders;
            Scryber.PDF.Graphics.PDFPen pen;
            switch (side)
            {
                case Sides.Left: pen = b.LeftPen; break;
                case Sides.Right: pen = b.RightPen; break;
                case Sides.Top: pen = b.TopPen; break;
                default: pen = b.BottomPen; break;
            }
            if (null == pen && (b.AllSides & side) == side)
                pen = b.AllPen;
            return (null == pen || pen.LineStyle == LineType.None) ? 0.0 : pen.Width.PointsValue;
        }

        private const string Grid = "<table id='t' class='t'><tr><td id='a'>A</td><td id='b'>B</td></tr><tr><td id='c'>C</td><td id='d'>D</td></tr></table>";
        private const string CellBorders = ".t { width: 300pt; } td { border: 2pt solid black; padding: 2pt; } ";

        [TestMethod]
        public void Separate_Default_EveryCellKeepsAllBorders()
        {
            Layout(CellBorders, Grid);
            foreach (var id in new[] { "a", "b", "c", "d" })
                foreach (var side in new[] { Sides.Left, Sides.Right, Sides.Top, Sides.Bottom })
                    Assert.AreEqual(2.0, Side(id, side), 0.01, id + " " + side);
        }

        [TestMethod]
        public void Collapse_SharedEdgesAreDrawnOnce()
        {
            Layout(CellBorders + ".t { border-collapse: collapse; }", Grid);

            //a: top-left, keeps its right and bottom (and its outer left and top)
            Assert.AreEqual(2.0, Side("a", Sides.Left), 0.01);
            Assert.AreEqual(2.0, Side("a", Sides.Top), 0.01);
            Assert.AreEqual(2.0, Side("a", Sides.Right), 0.01);
            Assert.AreEqual(2.0, Side("a", Sides.Bottom), 0.01);

            //b: its left is shared with a's right
            Assert.AreEqual(0.0, Side("b", Sides.Left), 0.01);
            Assert.AreEqual(2.0, Side("b", Sides.Right), 0.01);
            Assert.AreEqual(2.0, Side("b", Sides.Top), 0.01);
            Assert.AreEqual(2.0, Side("b", Sides.Bottom), 0.01);

            //c: its top is shared with a's bottom
            Assert.AreEqual(2.0, Side("c", Sides.Left), 0.01);
            Assert.AreEqual(0.0, Side("c", Sides.Top), 0.01);
            Assert.AreEqual(2.0, Side("c", Sides.Right), 0.01);
            Assert.AreEqual(2.0, Side("c", Sides.Bottom), 0.01);

            //d: left shared with c, top shared with b
            Assert.AreEqual(0.0, Side("d", Sides.Left), 0.01);
            Assert.AreEqual(0.0, Side("d", Sides.Top), 0.01);
            Assert.AreEqual(2.0, Side("d", Sides.Right), 0.01);
            Assert.AreEqual(2.0, Side("d", Sides.Bottom), 0.01);
        }

        [TestMethod]
        public void Collapse_WiderBorderWins()
        {
            Layout(CellBorders + ".t { border-collapse: collapse; } #b { border-left: 5pt solid red; }", Grid);
            //a's right (2) is narrower than b's left (5) so a's right is removed
            Assert.AreEqual(0.0, Side("a", Sides.Right), 0.01);
            Assert.AreEqual(5.0, Side("b", Sides.Left), 0.01);

            Layout(CellBorders + ".t { border-collapse: collapse; } #a { border-right: 5pt solid red; }", Grid);
            Assert.AreEqual(5.0, Side("a", Sides.Right), 0.01);
            Assert.AreEqual(0.0, Side("b", Sides.Left), 0.01);
        }

        [TestMethod]
        public void Collapse_OnlyWhereBothHaveBorders()
        {
            Layout(".t { width: 300pt; border-collapse: collapse; } #a { border: 2pt solid black; }", Grid);
            Assert.AreEqual(2.0, Side("a", Sides.Right), 0.01, "b has no border, so a's is kept");
            Assert.AreEqual(2.0, Side("a", Sides.Bottom), 0.01);
            Assert.AreEqual(0.0, Side("b", Sides.Left), 0.01);
        }

        [TestMethod]
        public void Collapse_TableBorder_SharedWithOuterCells()
        {
            Layout(CellBorders + ".t { border-collapse: collapse; border: 2pt solid blue; }", Grid);
            //the outer edges of the cells are the same as the table border, so the cell borders are removed
            Assert.AreEqual(0.0, Side("a", Sides.Left), 0.01);
            Assert.AreEqual(0.0, Side("a", Sides.Top), 0.01);
            Assert.AreEqual(0.0, Side("b", Sides.Right), 0.01);
            Assert.AreEqual(0.0, Side("d", Sides.Bottom), 0.01);
            //the inner edges remain
            Assert.AreEqual(2.0, Side("a", Sides.Right), 0.01);
            Assert.AreEqual(2.0, Side("a", Sides.Bottom), 0.01);
        }

        [TestMethod]
        public void Collapse_Colspan_AndRowspan()
        {
            var body = "<table class='t'><tr><td id='a' colspan='2'>A</td><td id='b' rowspan='2'>B</td></tr><tr><td id='c'>C</td><td id='d'>D</td></tr></table>";
            Layout(CellBorders + ".t { border-collapse: collapse; }", body);

            //a spans the first 2 columns, and is the left neighbour of b
            Assert.AreEqual(2.0, Side("a", Sides.Right), 0.01);
            Assert.AreEqual(0.0, Side("b", Sides.Left), 0.01);
            //c and d are under a (c's top shared with a's bottom)
            Assert.AreEqual(0.0, Side("c", Sides.Top), 0.01);
            //d is the left neighbour of b (rowspan) in row 2
            Assert.AreEqual(2.0, Side("d", Sides.Right), 0.01);
            //c is left of d
            Assert.AreEqual(0.0, Side("d", Sides.Left), 0.01);
        }

        [TestMethod]
        public void Collapse_RepeatedRowsFromTemplate_EachCellOnItsPosition()
        {
            var body = "<table class='t'>{{#each model.rows}}<tr><td id='c0_{{@index}}'>{{this.a}}</td><td id='c1_{{@index}}'>{{this.b}}</td></tr>{{/each}}</table>";
            Layout(CellBorders + ".t { border-collapse: collapse; }", body, new { rows = new[] { new { a = "1", b = "2" }, new { a = "3", b = "4" }, new { a = "5", b = "6" } } });

            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(2.0, Side("c0_" + i, Sides.Left), 0.01, "first column keeps its left, row " + i);
                Assert.AreEqual(0.0, Side("c1_" + i, Sides.Left), 0.01, "second column left is shared, row " + i);
                if (i == 0)
                    Assert.AreEqual(2.0, Side("c0_" + i, Sides.Top), 0.01);
                else
                    Assert.AreEqual(0.0, Side("c0_" + i, Sides.Top), 0.01, "top shared with the row above, row " + i);
            }
        }

        [TestMethod]
        public void Collapse_FromExpression()
        {
            var body = "<table id='t' class='t' style='border-collapse: {{model.mode}}'><tr><td id='a'>A</td><td id='b'>B</td></tr></table>";
            Layout(CellBorders, body, new { mode = "collapse" });
            Assert.AreEqual(0.0, Side("b", Sides.Left), 0.01);

            Layout(CellBorders, body, new { mode = "separate" });
            Assert.AreEqual(2.0, Side("b", Sides.Left), 0.01);
        }

        [TestMethod]
        public void Spacing_Single_AddsSpaceBetweenAndAtTheEdges()
        {
            Layout(CellBorders + ".t { border-spacing: 10pt; }", Grid);
            var a = Find("a").FullStyle.CreateMarginsThickness();
            var d = Find("d").FullStyle.CreateMarginsThickness();

            Assert.AreEqual(10.0, a.Left.PointsValue, 0.01);
            Assert.AreEqual(10.0, a.Top.PointsValue, 0.01);
            Assert.AreEqual(5.0, a.Right.PointsValue, 0.01);
            Assert.AreEqual(5.0, a.Bottom.PointsValue, 0.01);

            Assert.AreEqual(5.0, d.Left.PointsValue, 0.01);
            Assert.AreEqual(5.0, d.Top.PointsValue, 0.01);
            Assert.AreEqual(10.0, d.Right.PointsValue, 0.01);
            Assert.AreEqual(10.0, d.Bottom.PointsValue, 0.01);
        }

        [TestMethod]
        public void Spacing_TwoValues_HorizontalThenVertical()
        {
            Layout(CellBorders + ".t { border-spacing: 8pt 4pt; }", Grid);
            var b = Find("b").FullStyle.CreateMarginsThickness();
            Assert.AreEqual(4.0, b.Left.PointsValue, 0.01);
            Assert.AreEqual(8.0, b.Right.PointsValue, 0.01);
            Assert.AreEqual(4.0, b.Top.PointsValue, 0.01);
            Assert.AreEqual(2.0, b.Bottom.PointsValue, 0.01);
        }

        [TestMethod]
        public void Spacing_RelativeUnits_AreResolved()
        {
            Layout(CellBorders + ".t { border-spacing: 1em; }", "<div style='font-size: 10pt'>" + Grid + "</div>");
            var a = Find("a").FullStyle.CreateMarginsThickness();
            Assert.AreEqual(10.0, a.Left.PointsValue, 0.01);
            Assert.AreEqual(5.0, a.Right.PointsValue, 0.01);
        }

        [TestMethod]
        public void Spacing_IsIgnoredWhenCollapsed()
        {
            Layout(CellBorders + ".t { border-collapse: collapse; border-spacing: 10pt; }", Grid);
            var a = Find("a").FullStyle.CreateMarginsThickness();
            Assert.AreEqual(0.0, a.Left.PointsValue, 0.01);
            Assert.AreEqual(0.0, a.Top.PointsValue, 0.01);
        }

        [TestMethod]
        public void Spacing_NotSet_NoMargins()
        {
            Layout(CellBorders, Grid);
            var a = Find("a").FullStyle.CreateMarginsThickness();
            Assert.AreEqual(0.0, a.Left.PointsValue, 0.01);
        }

        [TestMethod]
        public void Spacing_TableWidthIsUnchanged()
        {
            Layout(CellBorders + ".t { border-spacing: 10pt; }", Grid);
            Assert.AreEqual(300.0, Find("t").TotalBounds.Width.PointsValue, 0.5);
        }
    }
}
