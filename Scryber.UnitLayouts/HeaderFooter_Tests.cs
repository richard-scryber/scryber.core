using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class HeaderFooter_Tests
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

        private class TemplateWithContent : ITemplate
        {
            public string InnerContent { get; set; }

            public Drawing.Color InnerBorder { get; set; }

            public Drawing.Unit InnerHeight { get; set; }

            public TemplateWithContent(string text, Drawing.Color borderColor, Drawing.Unit height)
            {
                this.InnerContent = text;
                this.InnerBorder = borderColor;
                this.InnerHeight = height;
            }

            public IEnumerable<IComponent> Instantiate(int index, IComponent owner)
            {
                var template = new Div() { Height = this.InnerHeight, BorderWidth = 1, BorderColor = this.InnerBorder, FontSize = 12, Padding = new Drawing.Thickness(5) };
                template.Contents.Add(new TextLiteral(this.InnerContent));

                return new IComponent[] { template };
            }
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionNoHeaderOrFooter()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);

            

            using (var ms = DocStreams.GetOutputStream("Section_NoHeaderOrFooter.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var firstPage = layout.AllPages[0] as PDFLayoutPage;
            Assert.IsNull(firstPage.HeaderBlock);
            Assert.IsNull(firstPage.FooterBlock);
            Assert.AreEqual(PageHeight, firstPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, firstPage.ContentBlock.TotalBounds.Width);

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionHeaderNoFooter()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            
            section.Header = new TemplateWithContent("Page Header", Drawing.StandardColors.Blue, 25);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);



            using (var ms = DocStreams.GetOutputStream("Section_HeaderNoFooter.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var firstPage = layout.AllPages[0] as PDFLayoutPage;
            Assert.IsNotNull(firstPage.HeaderBlock);
            Assert.IsNull(firstPage.FooterBlock);
            Assert.AreEqual(firstPage.HeaderBlock.Height, 25);
            Assert.AreEqual(PageHeight - 25, firstPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, firstPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, firstPage.ContentBlock.Columns[0].Contents.Count);

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionHeaderAndFooter()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            section.Header = new TemplateWithContent("Page Header", Drawing.StandardColors.Blue, 25);
            section.Footer = new TemplateWithContent("Page Footer", Drawing.StandardColors.Green, 30);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);



            using (var ms = DocStreams.GetOutputStream("Section_HeaderAndFooter.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var firstPage = layout.AllPages[0] as PDFLayoutPage;
            Assert.IsNotNull(firstPage.HeaderBlock);
            Assert.AreEqual(25, firstPage.HeaderBlock.Height);

            Assert.IsNotNull(firstPage.FooterBlock);
            Assert.AreEqual(30, firstPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 55, firstPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, firstPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, firstPage.ContentBlock.Columns[0].Contents.Count);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionTwoPagesWithHeaderAndFooter()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            section.Header = new TemplateWithContent("Page Header", Drawing.StandardColors.Blue, 25);
            section.Footer = new TemplateWithContent("Page Footer", Drawing.StandardColors.Green, 30);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);


            top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            top.Contents.Add(new TextLiteral("Sits on the second page"));
            section.Contents.Add(top);



            using (var ms = DocStreams.GetOutputStream("Section_TwoPagesHeaderAndFooter.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);

            var firstPage = layout.AllPages[0] as PDFLayoutPage;
            Assert.IsNotNull(firstPage.HeaderBlock);
            Assert.AreEqual(25, firstPage.HeaderBlock.Height);

            Assert.IsNotNull(firstPage.FooterBlock);
            Assert.AreEqual(30, firstPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 55, firstPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, firstPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, firstPage.ContentBlock.Columns[0].Contents.Count);

            var secondPage = layout.AllPages[1] as PDFLayoutPage;
            Assert.IsNotNull(secondPage.HeaderBlock);
            Assert.AreEqual(25, secondPage.HeaderBlock.Height);

            Assert.IsNotNull(secondPage.FooterBlock);
            Assert.AreEqual(30, secondPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 55, secondPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, secondPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, secondPage.ContentBlock.Columns[0].Contents.Count);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionTwoPagesWithHeaderAndFooterContinuation()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            section.Header = new TemplateWithContent("Page Header", Drawing.StandardColors.Blue, 25);
            section.Footer = new TemplateWithContent("Page Footer", Drawing.StandardColors.Green, 30);
            section.ContinuationHeader = new TemplateWithContent("Continuation Header", Drawing.StandardColors.Lime, 35);
            section.ContinuationFooter = new TemplateWithContent("Continuation Footer", Drawing.StandardColors.Fuchsia, 40);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);


            top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            top.Contents.Add(new TextLiteral("Sits on the second page"));
            section.Contents.Add(top);



            using (var ms = DocStreams.GetOutputStream("Section_TwoPagesHeaderAndFooterContinuation.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);

            var firstPage = layout.AllPages[0] as PDFLayoutPage;
            Assert.IsNotNull(firstPage.HeaderBlock);
            Assert.AreEqual(25, firstPage.HeaderBlock.Height);

            Assert.IsNotNull(firstPage.FooterBlock);
            Assert.AreEqual(30, firstPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 55, firstPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, firstPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, firstPage.ContentBlock.Columns[0].Contents.Count);

            var secondPage = layout.AllPages[1] as PDFLayoutPage;
            Assert.IsNotNull(secondPage.HeaderBlock);
            Assert.AreEqual(35, secondPage.HeaderBlock.Height);

            Assert.IsNotNull(secondPage.FooterBlock);
            Assert.AreEqual(40, secondPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 75, secondPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, secondPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, secondPage.ContentBlock.Columns[0].Contents.Count);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionThreePagesWithHeaderAndFooterContinuation()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            section.Header = new TemplateWithContent("Page Header", Drawing.StandardColors.Blue, 25);
            section.Footer = new TemplateWithContent("Page Footer", Drawing.StandardColors.Green, 30);
            section.ContinuationHeader = new TemplateWithContent("Continuation Header", Drawing.StandardColors.Lime, 35);
            section.ContinuationFooter = new TemplateWithContent("Continuation Footer", Drawing.StandardColors.Fuchsia, 40);

            Div div = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            div.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(div);


            div = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            div.Contents.Add(new TextLiteral("Sits on the second page"));
            section.Contents.Add(div);


            div = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            div.Contents.Add(new TextLiteral("Sits on the third page"));
            section.Contents.Add(div);



            using (var ms = DocStreams.GetOutputStream("Section_ThreePagesHeaderAndFooterContinuation.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(3, layout.AllPages.Count);

            var firstPage = layout.AllPages[0] as PDFLayoutPage;
            Assert.IsNotNull(firstPage.HeaderBlock);
            Assert.AreEqual(25, firstPage.HeaderBlock.Height);

            Assert.IsNotNull(firstPage.FooterBlock);
            Assert.AreEqual(30, firstPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 55, firstPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, firstPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, firstPage.ContentBlock.Columns[0].Contents.Count);

            var secondPage = layout.AllPages[1] as PDFLayoutPage;
            Assert.IsNotNull(secondPage.HeaderBlock);
            Assert.AreEqual(35, secondPage.HeaderBlock.Height);

            Assert.IsNotNull(secondPage.FooterBlock);
            Assert.AreEqual(40, secondPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 75, secondPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, secondPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, secondPage.ContentBlock.Columns[0].Contents.Count);

            var thirdPage = layout.AllPages[2] as PDFLayoutPage;
            Assert.IsNotNull(thirdPage.HeaderBlock);
            Assert.AreEqual(35, thirdPage.HeaderBlock.Height);

            Assert.IsNotNull(thirdPage.FooterBlock);
            Assert.AreEqual(40, thirdPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 75, thirdPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, thirdPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, thirdPage.ContentBlock.Columns[0].Contents.Count);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionFourPagesWithTwoSectionsHeaderAndFooterContinuation()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            section.Header = new TemplateWithContent("Page Header", Drawing.StandardColors.Blue, 25);
            section.Footer = new TemplateWithContent("Page Footer", Drawing.StandardColors.Green, 30);
            section.ContinuationHeader = new TemplateWithContent("Continuation Header", Drawing.StandardColors.Lime, 35);
            section.ContinuationFooter = new TemplateWithContent("Continuation Footer", Drawing.StandardColors.Fuchsia, 40);

            Div div = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            div.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(div);


            div = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            div.Contents.Add(new TextLiteral("Sits on the second page"));
            section.Contents.Add(div);


            section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            section.Header = new TemplateWithContent("Second Header", Drawing.StandardColors.Blue, 26);
            section.Footer = new TemplateWithContent("Second Footer", Drawing.StandardColors.Green, 31);
            section.ContinuationHeader = new TemplateWithContent("Second Continuation Header", Drawing.StandardColors.Lime, 36);
            section.ContinuationFooter = new TemplateWithContent("Second Continuation Footer", Drawing.StandardColors.Fuchsia, 41);

            div = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            div.Contents.Add(new TextLiteral("Sits on the second section, third page"));
            section.Contents.Add(div);

            div = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red, Padding = new Drawing.Thickness(5) };
            div.Contents.Add(new TextLiteral("Sits on the second section, fourth page"));
            section.Contents.Add(div);



            using (var ms = DocStreams.GetOutputStream("Section_FourPagesTwoSectionsHeaderAndFooterContinuation.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(4, layout.AllPages.Count);

            var firstPage = layout.AllPages[0] as PDFLayoutPage;
            Assert.IsNotNull(firstPage.HeaderBlock);
            Assert.AreEqual(25, firstPage.HeaderBlock.Height);

            Assert.IsNotNull(firstPage.FooterBlock);
            Assert.AreEqual(30, firstPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 55, firstPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, firstPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, firstPage.ContentBlock.Columns[0].Contents.Count);

            var secondPage = layout.AllPages[1] as PDFLayoutPage;
            Assert.IsNotNull(secondPage.HeaderBlock);
            Assert.AreEqual(35, secondPage.HeaderBlock.Height);

            Assert.IsNotNull(secondPage.FooterBlock);
            Assert.AreEqual(40, secondPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 75, secondPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, secondPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, secondPage.ContentBlock.Columns[0].Contents.Count);

            var thirdPage = layout.AllPages[2] as PDFLayoutPage;
            Assert.IsNotNull(thirdPage.HeaderBlock);
            Assert.AreEqual(26, thirdPage.HeaderBlock.Height);

            Assert.IsNotNull(thirdPage.FooterBlock);
            Assert.AreEqual(31, thirdPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 57, thirdPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, thirdPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, thirdPage.ContentBlock.Columns[0].Contents.Count);

            var fourthPage = layout.AllPages[3] as PDFLayoutPage;
            Assert.IsNotNull(fourthPage.HeaderBlock);
            Assert.AreEqual(36, fourthPage.HeaderBlock.Height);

            Assert.IsNotNull(fourthPage.FooterBlock);
            Assert.AreEqual(41, fourthPage.FooterBlock.Height);

            Assert.AreEqual(PageHeight - 77, fourthPage.ContentBlock.TotalBounds.Height);
            Assert.AreEqual(PageWidth, fourthPage.ContentBlock.TotalBounds.Width);
            Assert.AreEqual(1, fourthPage.ContentBlock.Columns[0].Contents.Count);
        }


    }
}
