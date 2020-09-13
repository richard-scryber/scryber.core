using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Native;
using Scryber.Styles.Selectors;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Styles
{
    [TestClass()]
    public class PDFStylePriorityMergeTest
    {

        #region public TestContext TestContext

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion


        [TestMethod]
        public void StylePrioritySingle_Test()
        {
            PDFStyleDefn defn;
            Document doc = new Document();
            

            defn = new PDFStyleDefn();
            defn.Match = "doc:Div.red"; //higher priority
            defn.Border.LineStyle = LineStyle.Solid;
            defn.Margins.Top = 30; 
            doc.Styles.Add(defn);

            defn = new PDFStyleDefn();
            defn.Match = ".red";
            defn.Border.Color = "#FF0000";
            defn.Border.Width = 3; //overriden by doc:Div.red
            doc.Styles.Add(defn);

            //Default div style (added after to ensure priority overrides order)
            defn = new PDFStyleDefn();
            defn.Match = "doc:Div";
            defn.Margins.Left = 10;
            defn.Margins.All = 20;
            defn.Fill.Color = "#00FF00";
            defn.Border.Color = "#0000FF"; //Overriden by .red
            defn.Border.Width = 1;
            defn.Border.LineStyle = LineStyle.Dash; //Overriden by doc:Div.red
            defn.Border.Dash = PDFDashes.Dot;
            doc.Styles.Add(defn);


            Page pg = new Page();
            doc.Pages.Add(pg);

            Div div = new Div();
            div.ElementName = "doc:Div";
            div.StyleClass = "red";
            div.BorderWidth = 10; //Highest priority
            pg.Contents.Add(div);

            var style = div.GetAppliedStyle();
            
            Assert.AreEqual(10, style.Border.Width.PointsValue, "Div border width should be 10");
            Assert.AreEqual(LineStyle.Solid, style.Border.LineStyle, "LineStyle should be solid from the doc:Div.red style");
            Assert.AreEqual((PDFColor)"#00FF00", style.Fill.Color, "Fill colour should be green from the doc:Div style");

            Assert.AreEqual(30, style.Margins.Top.PointsValue, "Top margin should be 30 from 'doc:Div.red'");
            Assert.AreEqual(10, style.Margins.Left.PointsValue, "Left margin should be 10 from 'doc:Div'");
            Assert.AreEqual(20, style.Margins.Bottom.PointsValue, "Bottom should be 20 from 'doc:Div' Margins.All");

        }

        [TestMethod]
        public void StylePriorityMultiple_Test()
        {

            PDFStyleDefn defn;
            Document doc = new Document();
            doc.ElementName = "doc:Document";

            defn = new PDFStyleDefn();
            defn.Match = "doc:Div.red"; //higher priority
            defn.Border.LineStyle = LineStyle.Solid;
            defn.Margins.Top = 30;
            doc.Styles.Add(defn);

            defn = new PDFStyleDefn();
            defn.Match = ".red";
            defn.Border.Color = "#FF0000";
            defn.Border.Width = 3; //overriden by doc:Div.red
            doc.Styles.Add(defn);

            //Default div style (added after to ensure priority overrides order)
            defn = new PDFStyleDefn();
            defn.Match = "doc:Div";
            defn.Margins.Left = 10;
            defn.Margins.All = 20;
            defn.Fill.Color = "#00FF00";
            defn.Border.Color = "#0000FF"; //Overriden by .red
            defn.Border.Width = 1;
            defn.Border.LineStyle = LineStyle.Dash; //Overriden by doc:Div.red
            defn.Border.Dash = PDFDashes.Dot;
            doc.Styles.Add(defn);


            Page pg = new Page();
            pg.ElementName = "doc:Page";
            pg.StyleClass = "green";
            doc.Pages.Add(pg);

            Div div = new Div();
            div.ElementName = "doc:Div";
            div.StyleClass = "red";
            div.BorderWidth = 10; //Highest priority
            pg.Contents.Add(div);

            var style = div.GetAppliedStyle();

            Assert.AreEqual(10, style.Border.Width.PointsValue, "Div border width should be 10");
            Assert.AreEqual(LineStyle.Solid, style.Border.LineStyle, "LineStyle should be solid from the doc:Div.red style");
            Assert.AreEqual((PDFColor)"#00FF00", style.Fill.Color, "Fill colour should be green from the doc:Div style");

            Assert.AreEqual(30, style.Margins.Top.PointsValue, "Top margin should be 30 from 'doc:Div.red'");
            Assert.AreEqual(10, style.Margins.Left.PointsValue, "Left margin should be 10 from 'doc:Div'");
            Assert.AreEqual(20, style.Margins.Bottom.PointsValue, "Bottom should be 20 from 'doc:Div' Margins.All");

        }

        [TestMethod]
        public void StylePriorityCompound_Test()
        {
            PDFStyleDefn defn;
            Document doc = new Document();
            doc.ElementName = "doc:Document";

            defn = new PDFStyleDefn();
            defn.Match = "doc:Div.red"; //higher priority
            defn.Border.Color = "#00FFFF";
            defn.Margins.Top = 20;
            defn.Margins.Left = 20;
            doc.Styles.Add(defn);

            defn = new PDFStyleDefn();
            defn.Match = "doc:Page.green .red"; //highest style priority
            defn.Border.Color = "#FF0000"; 
            defn.Border.Width = 3; 
            defn.Margins.Left = 30;
            doc.Styles.Add(defn);

            defn = new PDFStyleDefn();
            defn.Match = "doc:Page.blue .red"; //should be ignored
            defn.Border.Color = "#FFFFFF";
            defn.Border.Width = 10;
            defn.Margins.Left = 10;
            doc.Styles.Add(defn);

            defn = new PDFStyleDefn(); //Base applied style
            defn.Match = "doc:Div";
            defn.Margins.Left = 10;
            defn.Margins.All = 20;
            defn.Fill.Color = "#00FF00";
            defn.Border.Color = "#0000FF"; //Overriden by both
            
            doc.Styles.Add(defn);


            Page pg = new Page();
            pg.ElementName = "doc:Page";
            pg.StyleClass = "green";
            doc.Pages.Add(pg);

            Div div = new Div();
            div.ElementName = "doc:Div";
            div.StyleClass = "red";
            div.BorderWidth = 5; //Highest priority
            pg.Contents.Add(div);

            var style = div.GetAppliedStyle();

            Assert.AreEqual(5, style.Border.Width.PointsValue, "Div border width should be 5");
            Assert.AreEqual((PDFColor)"#FF0000", style.Border.Color, "Fill colour should be green from the doc:Div style");

            Assert.AreEqual(20, style.Margins.Top.PointsValue, "Top margin should be 30 from 'doc:Div.red'");
            Assert.AreEqual(30, style.Margins.Left.PointsValue, "Left margin should be 10 from 'doc:Div'");
            Assert.AreEqual(20, style.Margins.Bottom.PointsValue, "Bottom should be 20 from 'doc:Div' Margins.All");


        }


    }
}
