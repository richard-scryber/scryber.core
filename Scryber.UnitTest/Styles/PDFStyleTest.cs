using Scryber.Styles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Drawing;
using Scryber.Components;
using Scryber.PDF.Graphics;

namespace Scryber.Core.UnitTests.Styles
{
    
    
    /// <summary>
    ///This is a test class for PDFStyleTest and is intended
    ///to contain all PDFStyleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFStyleTest
    {


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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for PDFStyle Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PDFStyleConstructorTest()
        {
            Style target = new Style();
            Assert.IsNotNull(target);
            Assert.AreEqual(ObjectTypes.Style, target.Type);
            
        }



        /// <summary>
        ///A test for CreatePostionOptions
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void CreatePostionOptionsTest()
        {
            Style target = new Style();

            //Default (empty) position options
            PDFPositionOptions actual = target.CreatePostionOptions();

            Assert.AreEqual(false, actual.FillWidth);
            Assert.AreEqual(HorizontalAlignment.Left, actual.HAlign);
            Assert.AreEqual(false, actual.Height.HasValue);
            Assert.AreEqual(false, actual.Width.HasValue);
            Assert.AreEqual(false, actual.X.HasValue);
            Assert.AreEqual(false, actual.Y.HasValue);
            Assert.AreEqual(PDFThickness.Empty(), actual.Margins);
            Assert.AreEqual(OverflowAction.NewPage, actual.OverflowAction);
            Assert.AreEqual(OverflowSplit.Any, actual.OverflowSplit);
            Assert.AreEqual(PDFThickness.Empty(), actual.Padding);
            Assert.AreEqual(PositionMode.Block, actual.PositionMode);
            Assert.AreEqual(VerticalAlignment.Top, actual.VAlign);
            Assert.AreEqual(Visibility.Visible, actual.Visibility);
            Assert.AreEqual(null, actual.Height);
            Assert.AreEqual(null, actual.Width);
            Assert.AreEqual(null, actual.X);
            Assert.AreEqual(null, actual.Y);

            Assert.AreEqual(false, actual.MinimumHeight.HasValue);
            Assert.AreEqual(false, actual.MinimumWidth.HasValue);
            Assert.AreEqual(false, actual.MaximumHeight.HasValue);
            Assert.AreEqual(false, actual.MaximumWidth.HasValue);

            Assert.AreEqual(null, actual.MinimumHeight);
            Assert.AreEqual(null, actual.MinimumWidth);
            Assert.AreEqual(null, actual.MaximumHeight);
            Assert.AreEqual(null, actual.MaximumWidth);

            target = new Style();
            target.Position.PositionMode = PositionMode.Inline;
            target.Size.FullWidth = true;
            target.Position.HAlign = HorizontalAlignment.Center;
            target.Position.VAlign = VerticalAlignment.Middle;
            target.Position.X = 20;
            target.Position.Y = 50;
            target.Size.Width = 100;
            //Don't define height
            target.Size.MaximumHeight = 90;
            target.Size.MinimumHeight = 50;
            target.Size.MaximumWidth = 200;
            target.Size.MinimumWidth = 150;

            target.Overflow.Action = OverflowAction.Truncate;
            target.Overflow.Split = OverflowSplit.Never;

            target.Margins.All = 10;
            target.Margins.Bottom = 20;

            target.Padding.All = 20;
            target.Padding.Right = 40;

            actual = target.CreatePostionOptions();

            Assert.AreEqual(false, actual.FillWidth); //false because a width has been set
            Assert.AreEqual(HorizontalAlignment.Center, actual.HAlign);
            Assert.AreEqual(VerticalAlignment.Middle, actual.VAlign);

            Assert.AreEqual(false, actual.Height.HasValue);
            Assert.AreEqual(true, actual.Width.HasValue);
            Assert.AreEqual(true, actual.X.HasValue);
            Assert.AreEqual(true, actual.Y.HasValue);
            Assert.AreEqual((PDFUnit)100, actual.Width);
            Assert.AreEqual((PDFUnit)20, actual.X);
            Assert.AreEqual((PDFUnit)50, actual.Y);
            Assert.AreEqual(null, actual.Height);

            Assert.AreEqual((PDFUnit)200, actual.MaximumWidth);
            Assert.AreEqual((PDFUnit)150, actual.MinimumWidth);
            Assert.AreEqual((PDFUnit)90, actual.MaximumHeight);
            Assert.AreEqual((PDFUnit)50, actual.MinimumHeight);

            Assert.AreEqual(OverflowAction.Truncate, actual.OverflowAction);
            Assert.AreEqual(OverflowSplit.Never, actual.OverflowSplit);

            Assert.AreEqual((PDFUnit)10, actual.Margins.Left);
            Assert.AreEqual((PDFUnit)20, actual.Margins.Bottom);

            Assert.AreEqual((PDFUnit)20, actual.Padding.Left);
            Assert.AreEqual((PDFUnit)40, actual.Padding.Right);

            Assert.AreEqual(PositionMode.Relative, actual.PositionMode); //mode is relative (because we have an xand a y)
            Assert.AreEqual(VerticalAlignment.Middle, actual.VAlign);
            Assert.AreEqual(Visibility.Visible, actual.Visibility);

           
        }

        /// <summary>
        ///A test for CreateTextOptions
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void CreateTextOptionsTest()
        {

            Style target = new Style();
            PDFTextRenderOptions actual = target.CreateTextOptions();

            Assert.IsNull(actual.Background);
            Assert.IsNull(actual.FillBrush);

            //Default value unit for first line inset and word spacing is Zero.
            Assert.IsTrue(actual.FirstLineInset.HasValue);
            Assert.AreEqual(PDFUnit.Zero, actual.FirstLineInset.Value);

            Assert.IsFalse(actual.WordSpacing.HasValue);
            Assert.IsNull(actual.Font, "Font is not null");
            Assert.IsNull(actual.Stroke);
            Assert.IsFalse(actual.Leading.HasValue);
            
            Assert.IsFalse(actual.WrapText.HasValue);

            //set the specific values

            target = new Style();
            
            target.Background.Color = StandardColors.Lime;
            target.Background.FillStyle = Scryber.Drawing.FillType.Solid;

            target.Fill.Color = StandardColors.Navy;
            target.Fill.Style = Scryber.Drawing.FillType.Solid;

            target.Stroke.Color = StandardColors.Purple;
            target.Stroke.Width = 2;
            target.Stroke.LineStyle = LineType.Solid;


            target.Font.FontSize = 36;
            target.Font.FontFamily = (FontSelector)"Bauhaus 92";
            
            
            target.Text.WrapText = Text.WordWrap.NoWrap;
            target.Text.WordSpacing = 1.0F;
            

            //validate against settings
            actual = target.CreateTextOptions();

            Assert.IsInstanceOfType(actual.Background, typeof(PDFSolidBrush));
            Assert.AreEqual(StandardColors.Lime, ((PDFSolidBrush)actual.Background).Color);

            Assert.IsInstanceOfType(actual.FillBrush, typeof(PDFSolidBrush));
            Assert.AreEqual(StandardColors.Navy, ((PDFSolidBrush)actual.FillBrush).Color);

            Assert.IsInstanceOfType(actual.Stroke, typeof(PDFSolidPen));
            Assert.AreEqual(StandardColors.Purple, ((PDFSolidPen)actual.Stroke).Color);
            Assert.AreEqual((PDFUnit)2, actual.Stroke.Width);

            Assert.IsInstanceOfType(actual.Font, typeof(PDFFont));
            Assert.AreEqual((FontSelector)"Bauhaus 92", actual.Font.Selector);
            Assert.AreEqual((PDFUnit)36, actual.Font.Size);

            
            Assert.IsTrue(actual.WordSpacing.HasValue);
            Assert.AreEqual((PDFUnit)1.0, actual.WordSpacing.Value);

            Assert.IsTrue(actual.WrapText.HasValue);
            Assert.AreEqual(Text.WordWrap.NoWrap, actual.WrapText.Value);


        }

        /// <summary>
        ///A test for Flatten
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void FlattenTest()
        {
            Style target = new Style();

            //Add a background color and style
            BackgroundStyle bg = new BackgroundStyle();
            target.Background.Color = StandardColors.Aqua;
            target.Background.FillStyle = Scryber.Drawing.FillType.Pattern;
            //target.Background.AddItem(bg);

            //Add a font
            Scryber.Styles.FontStyle fs = new Scryber.Styles.FontStyle();
            target.Font.FontBold = true;
            target.Font.FontFamily = (FontSelector)"Bauhaus 92";
            //target.AddItem(fs);

            //Flattening process will remove the duplicates from the bottom up.
            Style actual = target.Flatten();
            Assert.AreEqual(StandardColors.Aqua, actual.Background.Color);
            Assert.AreEqual(Scryber.Drawing.FillType.Pattern, actual.Background.FillStyle);
            Assert.AreEqual(true, actual.Font.FontBold);
            Assert.AreEqual((FontSelector)"Bauhaus 92", actual.Font.FontFamily);

            
        }

        /// <summary>
        ///A test for GetStyleItem
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void GetStyleItemTest()
        {
            Style target = new Style();

            //Add a background color and style
            //PDFBackgroundStyle bg = new PDFBackgroundStyle();
            target.Background.Color = StandardColors.Aqua;
            target.Background.FillStyle = Scryber.Drawing.FillType.Pattern;
            //target.AddItem(bg);

            //Add a font
            //PDFFontStyle fs = new PDFFontStyle();
            target.Font.FontBold = true;
            target.Font.FontFamily = (FontSelector)"Bauhaus 92";
            //target.AddItem(fs);

            

            
            //Flattenting should replace the duplicate backgrounds
            BackgroundStyle actual = target.GetOrCreateItem<BackgroundStyle>(StyleKeys.BgItemKey) as BackgroundStyle;
            Assert.IsNotNull(actual);

            //But we should not have lost any detail within the style for the background
            Assert.AreEqual(StandardColors.Aqua, actual.Color);
            Assert.AreEqual(Scryber.Drawing.FillType.Pattern, actual.FillStyle);

        }



        /// <summary>
        ///A test for MergeInherited
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void MergeInheritedTest()
        {
            Style target = new Style();

            //Add a background color and style - not inherited
            target.Background.Color = StandardColors.Aqua;
            target.Background.FillStyle = Scryber.Drawing.FillType.Pattern;
            
            //Add a font - inherited
            target.Font.FontBold = true;
            target.Font.FontFamily = (FontSelector)"Bauhaus 92";
            

            Label lbl = new Label();
            bool replace = false;

            Style merged = new Style();
            merged.Margins.All = 10;
            
            target.MergeInherited(merged, replace, 0);

            //Font is inherited
            bool expected = true;
            bool actual = merged.IsValueDefined(StyleKeys.FontWeightKey);
            Assert.AreEqual(expected, actual);

            //Background was defined on the target, but is not inherited
            expected = false;
            actual = merged.IsValueDefined(StyleKeys.BgColorKey);
            Assert.AreEqual(expected, actual);

            //Margins was defined on the actual style - should still be there.
            expected = true;
            actual = merged.IsValueDefined(StyleKeys.MarginsAllKey);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MergeInto
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void MergeIntoTest()
        {
            Style target = new Style();

            //Add a background color and style - not inherited
            target.Background.Color = StandardColors.Aqua;
            target.Background.FillStyle = Scryber.Drawing.FillType.Pattern;
            
            //Add a font - inherited
            target.Font.FontBold = true;
            target.Font.FontFamily = (FontSelector)"Bauhaus 92";
            

            Style merged = new Style();
            merged.Margins.All = 10;

            int priority = Style.DirectStylePriority;

            target.MergeInto(merged, priority);

            //Font is inherited
            bool expected = true;
            bool actual = merged.IsValueDefined(StyleKeys.FontFamilyKey);
            Assert.AreEqual(expected, actual,"Font style was not found");

            //Background was defined on the target, but is not inherited
            actual = merged.IsValueDefined(StyleKeys.BgColorKey);
            Assert.AreEqual(expected, actual, "Background was not found");

            //Margins was defined on the actual style - should still be there.
            expected = true;
            actual = merged.IsValueDefined(StyleKeys.MarginsAllKey);
            Assert.AreEqual(expected, actual, "Margins was not found");
            
        }

        /// <summary>
        ///A test for MergeInto
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void MergeIntoTest1()
        {
            Style target = new Style();

            //Add a background color and style - not inherited
            target.Background.Color = StandardColors.Aqua;
            target.Background.FillStyle = Scryber.Drawing.FillType.Pattern;
            
            //Add a font - inherited
            target.Font.FontBold = true;
            target.Font.FontFamily = (FontSelector)"Bauhaus 92";
            
            Style merged = new Style();
            merged.Margins.All = 10;
            

            Label lbl = new Label();
            ComponentState state = ComponentState.Normal;

            //in the base implementation these are ignored - so no difference
            target.MergeInto(merged, lbl, state);

            //Font is inherited
            bool expected = true;
            bool actual = merged.IsValueDefined(StyleKeys.FontFamilyKey);
            Assert.AreEqual(expected, actual, "Font style was not found");

            //Background was defined on the target
            expected = true;
            actual = merged.IsValueDefined(StyleKeys.BgColorKey);
            Assert.AreEqual(expected, actual, "Background was not found");

            //Margins was defined on the actual style - should still be there.
            expected = true;
            actual = merged.IsValueDefined(StyleKeys.MarginsAllKey);
            Assert.AreEqual(expected, actual, "Margins was not found");
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void RemoveTest()
        {
            Style target = new Style();

            //Add a background color and style - not inherited
            target.Background.Color = StandardColors.Aqua;
            target.Background.FillStyle = Scryber.Drawing.FillType.Pattern;
            
            //Add a font - inherited
            target.Font.FontBold = true;
            target.Font.FontFamily = (FontSelector)"Bauhaus 92";
            

            bool expected = true;
            bool actual;

            actual = target.RemoveItemStyleValues(StyleKeys.FontItemKey);

            Assert.AreEqual(expected, actual);
            Assert.IsFalse(target.IsValueDefined(StyleKeys.FontFamilyKey));
            Assert.IsFalse(target.IsValueDefined(StyleKeys.FontWeightKey));

            actual = target.RemoveItemStyleValues(StyleKeys.BgItemKey);
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(target.IsValueDefined(StyleKeys.BgColorKey));

            expected = false;
            actual = target.RemoveItemStyleValues(StyleKeys.FontItemKey);
            Assert.AreEqual(expected, actual);

            expected = false;
            actual = target.RemoveItemStyleValues(StyleKeys.BgItemKey);
            Assert.AreEqual(expected, actual);
        }




        /// <summary>
        ///A test for TryGetStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void TryGetStyleTest()
        {
            Style target = new Style();
            target.Background.Color = StandardColors.Aqua;
            
            target.Border.CornerRadius = 20;
            
            target.Stroke.Width = 4;

            target.Text.DateFormat = "D";


            
            //Text exists has data
            
            TextStyle text;
            bool found = target.TryGetItem<TextStyle>(StyleKeys.TextItemKey, out text);
            Assert.IsTrue(found);
            Assert.IsNotNull(text);
            Assert.AreEqual(text.Type, ObjectTypes.StyleText);

            

        }



        /// <summary>
        ///A test for Background
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void BackgroundTest()
        {
            Style target = new Style();

            BackgroundStyle actual;
            actual = target.Background;
            Assert.IsNotNull(actual);

            actual.Color = StandardColors.Lime;
            Assert.AreEqual(actual.Color, target.Background.Color);

            
        }

        /// <summary>
        ///A test for Border
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void BorderTest()
        {
            Style target = new Style();

            BorderStyle actual;
            actual = target.Border;
            Assert.IsNotNull(actual);

            actual.Color = StandardColors.Lime;
            Assert.AreEqual(actual.Color, target.Border.Color);

        }

        /// <summary>
        ///A test for Columns
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void ColumnsTest()
        {
            Style target = new Style();

            ColumnsStyle actual;
            actual = target.Columns;
            Assert.IsNotNull(actual);

            actual.ColumnCount = 5;
            Assert.AreEqual(actual.ColumnCount, target.Columns.ColumnCount);

        }

        /// <summary>
        ///A test for Fill
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void FillTest()
        {
            Style target = new Style();

            Scryber.Styles.FillStyle actual;
            actual = target.Fill;
            Assert.IsNotNull(actual);

            actual.Color = StandardColors.Lime;
            Assert.AreEqual(actual.Color, target.Fill.Color);
        }

        /// <summary>
        ///A test for Font
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void FontTest()
        {
            Style target = new Style();

            Scryber.Styles.FontStyle actual;
            actual = target.Font;
            Assert.IsNotNull(actual);

            actual.FontFamily = (FontSelector)"Bauhaus 92";
            Assert.AreEqual(actual.FontFamily, target.Font.FontFamily);

        }
        

        /// <summary>
        ///A test for Margins
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void MarginsTest()
        {
            Style target = new Style();

            MarginsStyle actual;
            actual = target.Margins;
            Assert.IsNotNull(actual);

            actual.All = (PDFUnit)10;
            Assert.AreEqual(actual.All, target.Margins.All);
        }

        /// <summary>
        ///A test for Outline
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void OutlineTest()
        {
            Style target = new Style();

            OutlineStyle actual;
            actual = target.Outline;
            Assert.IsNotNull(actual);

            actual.Color = StandardColors.Lime;
            Assert.AreEqual(actual.Color, target.Outline.Color);
        }

        /// <summary>
        ///A test for Overflow
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void OverflowTest()
        {
            Style target = new Style();

            OverflowStyle actual;
            actual = target.Overflow;
            Assert.IsNotNull(actual);

            actual.Action = OverflowAction.Truncate;
            Assert.AreEqual(actual.Action, target.Overflow.Action);
        }

        /// <summary>
        ///A test for Padding
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PaddingTest()
        {
            Style target = new Style();

            PaddingStyle actual;
            actual = target.Padding;
            Assert.IsNotNull(actual);

            actual.All = (PDFUnit)10;
            Assert.AreEqual(actual.All, target.Padding.All);

        }

        /// <summary>
        ///A test for PageStyle
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PageStyleTest()
        {
            Style target = new Style();

            PageStyle actual;
            actual = target.PageStyle;
            Assert.IsNotNull(actual);

            actual.Width = (PDFUnit)10;
            Assert.AreEqual(actual.Width, target.PageStyle.Width);

        }

        /// <summary>
        ///A test for Position
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void PositionTest()
        {
            Style target = new Style();

            PositionStyle actual;
            actual = target.Position;
            Assert.IsNotNull(actual);

            
        }

        /// <summary>
        ///A test for Position
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void SizeTest()
        {
            Style target = new Style();

            SizeStyle actual;
            actual = target.Size;
            Assert.IsNotNull(actual);

            actual.Width = (PDFUnit)10;
            actual.MinimumWidth = (PDFUnit)9;
            actual.MaximumWidth = (PDFUnit)11;
            actual.Height = (PDFUnit)20;
            actual.MinimumHeight = (PDFUnit)19;
            actual.MaximumHeight = (PDFUnit)21;

            Assert.AreEqual(actual.Width, target.Size.Width);
            Assert.AreEqual(actual.MinimumWidth, (PDFUnit)9);
            Assert.AreEqual(actual.MinimumHeight, (PDFUnit)19);
            Assert.AreEqual(actual.MaximumWidth, (PDFUnit)11);
            Assert.AreEqual(actual.MaximumHeight, (PDFUnit)21);
            Assert.AreEqual(actual.Height, (PDFUnit)20);
        }

        /// <summary>
        ///A test for Stroke
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void StrokeTest()
        {
            Style target = new Style();

            StrokeStyle actual;
            actual = target.Stroke;
            Assert.IsNotNull(actual);

            actual.Width = (PDFUnit)10;
            Assert.AreEqual(actual.Width, target.Stroke.Width);
        }

        /// <summary>
        ///A test for Table
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void TableTest()
        {
            Style target = new Style();

            TableStyle actual;
            actual = target.Table;
            Assert.IsNotNull(actual);

            actual.CellColumnSpan = 10;
            Assert.AreEqual(actual.CellColumnSpan, target.Table.CellColumnSpan);
        }

        /// <summary>
        ///A test for Text
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void TextTest()
        {
            Style target = new Style();

            TextStyle actual;
            actual = target.Text;
            Assert.IsNotNull(actual);

            actual.Leading = 10;
            Assert.AreEqual(actual.Leading, target.Text.Leading);
        }


        /// <summary>
        ///A test for HasItems
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void HasItemsTest()
        {
            Style target = new Style();
            bool actual;
            actual = target.HasValues;
            Assert.IsFalse(actual);

            target.Position.PositionMode = PositionMode.Absolute;
            actual = target.HasValues;
            Assert.IsTrue(actual);

            target.RemoveValue(StyleKeys.PositionModeKey);
            actual = target.HasValues;
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for ID
        ///</summary>
        [TestMethod()]
        [TestCategory("Styles")]
        public void IDTest()
        {
            Style target = new Style();
            string expected = "MyStyle";
            string actual;
            target.ID = expected;
            actual = target.ID;
            Assert.AreEqual(expected, actual);
            
        }

        
    }
}
