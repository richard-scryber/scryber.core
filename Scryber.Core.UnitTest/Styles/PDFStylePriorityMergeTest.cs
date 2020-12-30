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
            StyleDefn defn;
            Document doc = new Document();
            

            defn = new StyleDefn();
            defn.Match = "doc:Div.red"; //higher priority
            defn.Border.LineStyle = LineType.Solid;
            defn.Margins.Top = 30; 
            doc.Styles.Add(defn);

            defn = new StyleDefn();
            defn.Match = ".red";
            defn.Border.Color = "#FF0000";
            defn.Border.Width = 3; //overriden by doc:Div.red
            doc.Styles.Add(defn);

            //Default div style (added after to ensure priority overrides order)
            defn = new StyleDefn();
            defn.Match = "doc:Div";
            defn.Margins.Left = 10;
            defn.Margins.All = 20;
            defn.Fill.Color = "#00FF00";
            defn.Border.Color = "#0000FF"; //Overriden by .red
            defn.Border.Width = 1;
            defn.Border.LineStyle = LineType.Dash; //Overriden by doc:Div.red
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
            Assert.AreEqual(LineType.Solid, style.Border.LineStyle, "LineStyle should be solid from the doc:Div.red style");
            Assert.AreEqual((PDFColor)"#00FF00", style.Fill.Color, "Fill colour should be green from the doc:Div style");

            Assert.AreEqual(30, style.Margins.Top.PointsValue, "Top margin should be 30 from 'doc:Div.red'");
            Assert.AreEqual(10, style.Margins.Left.PointsValue, "Left margin should be 10 from 'doc:Div'");
            Assert.AreEqual(20, style.Margins.Bottom.PointsValue, "Bottom should be 20 from 'doc:Div' Margins.All");

        }

        [TestMethod]
        public void StylePriorityMultiple_Test()
        {

            StyleDefn defn;
            Document doc = new Document();
            doc.ElementName = "doc:Document";

            defn = new StyleDefn();
            defn.Match = "doc:Div.red"; //higher priority
            defn.Border.LineStyle = LineType.Solid;
            defn.Margins.Top = 30;
            doc.Styles.Add(defn);

            defn = new StyleDefn();
            defn.Match = ".red";
            defn.Border.Color = "#FF0000";
            defn.Border.Width = 3; //overriden by doc:Div.red
            doc.Styles.Add(defn);

            //Default div style (added after to ensure priority overrides order)
            defn = new StyleDefn();
            defn.Match = "doc:Div";
            defn.Margins.Left = 10;
            defn.Margins.All = 20;
            defn.Fill.Color = "#00FF00";
            defn.Border.Color = "#0000FF"; //Overriden by .red
            defn.Border.Width = 1;
            defn.Border.LineStyle = LineType.Dash; //Overriden by doc:Div.red
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
            Assert.AreEqual(LineType.Solid, style.Border.LineStyle, "LineStyle should be solid from the doc:Div.red style");
            Assert.AreEqual((PDFColor)"#00FF00", style.Fill.Color, "Fill colour should be green from the doc:Div style");

            Assert.AreEqual(30, style.Margins.Top.PointsValue, "Top margin should be 30 from 'doc:Div.red'");
            Assert.AreEqual(10, style.Margins.Left.PointsValue, "Left margin should be 10 from 'doc:Div'");
            Assert.AreEqual(20, style.Margins.Bottom.PointsValue, "Bottom should be 20 from 'doc:Div' Margins.All");

        }

        [TestMethod]
        public void StylePriorityCompound_Test()
        {
            StyleDefn defn;
            Document doc = new Document();
            doc.ElementName = "doc:Document";

            defn = new StyleDefn();
            defn.Match = "doc:Div.red"; //higher priority
            defn.Border.Color = "#00FFFF";
            defn.Margins.Top = 20;
            defn.Margins.Left = 20;
            doc.Styles.Add(defn);

            defn = new StyleDefn();
            defn.Match = "doc:Page.green .red"; //highest style priority
            defn.Border.Color = "#FF0000"; 
            defn.Border.Width = 3; 
            defn.Margins.Left = 30;
            doc.Styles.Add(defn);

            defn = new StyleDefn();
            defn.Match = "doc:Page.blue .red"; //should be ignored
            defn.Border.Color = "#FFFFFF";
            defn.Border.Width = 10;
            defn.Margins.Left = 10;
            doc.Styles.Add(defn);

            defn = new StyleDefn(); //Base applied style
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


        [TestMethod]
        public void StylePriorityNested_Test()
        {
            //Checks that the nested values are used in a style stack
            //for inherited styles

            var src = @"<?xml version='1.0' encoding='UTF-8' ?>
            <doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                          xmlns:style='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'>
              <Pages>
                <doc:Section id='pg' style:padding='20pt' style:fill-color='#880000' style:bg-color='#FF0000' >
                  <Content>
                    <doc:Div id='first' style:fill-color='#008800' style:bg-color='#00FF00' >
                      This should be green
                    </doc:Div>
                    <doc:Div id='second' style:fill-color='#000088' style:bg-color='#0000FF'  >
                      This should be blue
                    </doc:Div>
                    This should be red
                  </Content>
                </doc:Section>
              </Pages>
            </doc:Document>";

            using (var ms = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(ms, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("StylePriorityNested.pdf"))
                    doc.SaveAsPDF(stream);

                var pg = doc.FindAComponentById("pg");
                var first = doc.FindAComponentById("first");
                var second = doc.FindAComponentById("second");

                var style = pg.GetFirstArrangement().FullStyle;
                Assert.AreEqual((PDFColor)"#880000", style.Fill.Color, "Page fill color incorrect");
                Assert.AreEqual((PDFColor)"#FF0000", style.Background.Color, "Page bg color incorrect");

                style = first.GetFirstArrangement().FullStyle;
                Assert.AreEqual((PDFColor)"#008800", style.Fill.Color, "First Div fill color incorrect");
                Assert.AreEqual((PDFColor)"#00FF00", style.Background.Color, "First div bg color incorrect");

                style = second.GetFirstArrangement().FullStyle;
                Assert.AreEqual((PDFColor)"#000088", style.Fill.Color, "Second Div fill color incorrect");
                Assert.AreEqual((PDFColor)"#0000FF", style.Background.Color, "Second Div bg color incorrect");

            }
        }


        [TestMethod]
        public void StylePriorityNestedClasses_Test()
        {
            //Checks that the nested values are used in a style stack
            //for inherited styles

            var src = @"<?xml version='1.0' encoding='UTF-8' ?>
            <doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                          xmlns:style='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'>
              <Styles>
                <style:Style match='doc:Section.red' >
                    <style:Background color='#FF0000' />
                    <style:Fill color='#880000' />
                </style:Style>
                <style:Style match='.green' >
                    <style:Background color='#00FF00' />
                    <style:Fill color='#008800' />
                </style:Style>
                <style:Style match='.blue' >
                    <style:Background color='#0000FF' />
                    <style:Fill color='#000088' />
                </style:Style>
              </Styles>
              <Pages>
                <doc:Section id='pg' style:class='red' >
                  <Content>
                    <doc:Div id='first' style:class='green' >
                      This should be green
                      <doc:Div id='second' style:class='blue'  >
                        This should be blue
                      </doc:Div>
                      <doc:Div id='third' >
                        This should also be green
                      </doc:Div>
                    </doc:Div>
                    This should be red
                  </Content>
                </doc:Section>
              </Pages>
            </doc:Document>";

            using (var ms = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(ms, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("StylePriorityNestedClasses.pdf"))
                    doc.SaveAsPDF(stream);

                var pg = doc.FindAComponentById("pg");
                var first = doc.FindAComponentById("first");
                var second = doc.FindAComponentById("second");
                var third = doc.FindAComponentById("third");

                var style = pg.GetFirstArrangement().FullStyle;
                Assert.AreEqual((PDFColor)"#880000", style.Fill.Color, "Page fill color incorrect");
                Assert.AreEqual((PDFColor)"#FF0000", style.Background.Color, "Page bg color incorrect");

                style = first.GetFirstArrangement().FullStyle;
                Assert.AreEqual((PDFColor)"#008800", style.Fill.Color, "First Div fill color incorrect");
                Assert.AreEqual((PDFColor)"#00FF00", style.Background.Color, "First div bg color incorrect");

                style = second.GetFirstArrangement().FullStyle;
                Assert.AreEqual((PDFColor)"#000088", style.Fill.Color, "Second Div fill color incorrect");
                Assert.AreEqual((PDFColor)"#0000FF", style.Background.Color, "Second Div bg color incorrect");

                style = third.GetFirstArrangement().FullStyle;
                Assert.AreEqual((PDFColor)"#008800", style.Fill.Color, "Third Div fill color incorrect");
                Assert.IsFalse(style.IsValueDefined(StyleKeys.BgColorKey), "Third div bg color should not be present");

            }
        }
    }
}
