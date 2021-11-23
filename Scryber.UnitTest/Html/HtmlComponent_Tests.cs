using System;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib;
using Scryber.Components;
using Scryber.PDF.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.Html.Components;
using Scryber.PDF;


namespace Scryber.Core.UnitTests.Html
{
    [TestClass]
    public class HtmlComponent_Tests
    {
        
        #region public TestContext TestContext {get;set;}
        
        private TestContext _testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }
        
        #endregion

        #region Document Head and Body with Header, Footer and Content

        [TestMethod]
        public void ComponentDocuments_Test()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' class='top' title='Html Element' >
<head >
    <title>Test Document</title>
    <meta name='author' content='Scryber' />
</head>
<body class='bodyClass' style='padding:20pt;' title='Body Element' >
    <header>This is the header</header>
    <p>This is the page</p>
    <footer>This is the footer</footer>
</body>
</html>";
            
            using var sr = new System.IO.StringReader(html);
            using var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            using var stream = DocStreams.GetOutputStream("ComponentDocument.pdf");

            doc.SaveAsPDF(stream);
            
            Assert.IsNotNull(doc, "The document should not be null");
            Assert.AreEqual("top", doc.StyleClass, "The document should have the style class of 'top'");
            Assert.IsInstanceOfType(doc, typeof(HTMLDocument),"The document types did not match");
            Assert.AreEqual("Test Document", doc.Info.Title, "The document titles did not match");
            Assert.AreEqual("Scryber", doc.Info.Author, "The document authors did not match");
            Assert.AreEqual("Html Element", doc.Outline.Title,"The document outlines did not match");
            Assert.AreEqual(1, doc.Pages.Count,"The document page count did not match");

            var pg = doc.Pages[0] as Section;
            
            Assert.IsNotNull(pg, "The document section did not match");
            Assert.IsNotNull(pg.Header, "The document header did not match");
            Assert.IsNotNull(pg.Footer,"The document footer did not match");
            
            Assert.AreEqual("Body Element", pg.OutlineTitle, "The section outline did not match");
            Assert.AreEqual("bodyClass", pg.StyleClass, "The section style class did not match");
            Assert.AreEqual(20, pg.Style.Padding.All.PointsValue, "The section padding did not match");
            Assert.AreEqual(1, pg.Contents.Count,"Page content count did not match");

            var one = pg.Header.Instantiate(0, pg).ToArray();
            Assert.IsNotNull(one, "The section header was null");
            Assert.AreEqual(1, one.Length, "The section header content count was not 1");
            //Titles are not supported on title
            //Assert.AreEqual("Header Element", ((Component)one[0]).Outline.Title);
            
            one = pg.Footer.Instantiate(0, pg).ToArray();
            Assert.IsNotNull(one, "The section footer was null");
            Assert.AreEqual(1, one.Length, "The section footer content count was not 1");
            //Titles are not supported on templates
            //Assert.AreEqual("Footer Element", ((Component)one[0]).Outline.Title);
        }
        
        
        #endregion

        #region Headings 1 to 6 validation

        /// <summary>
        /// An image in the content with a full file path
        /// </summary>
        [TestMethod]
        public void ComponentHeadings_Test()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <h1 id='h1' title='Heading 1' >Heading 1 Title</h1>
    <h2 id='h2' title='Heading 2' >Heading 2 Title</h2>
    <h3 id='h3' title='Heading 3' >Heading 3 Title</h3>
    <h4 id='h4' title='Heading 4' >Heading 4 Title</h4>
    <h5 id='h5' title='Heading 5' >Heading 5 Title</h5>
    <h6 class='Head6' style='padding:20pt' id='h6' title='Heading 6' >Heading 6 Title</h6>
</body>
</html>";

            using var sr = new StringReader(html);
            using var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            using var stream = DocStreams.GetOutputStream("ComponentHeadings.pdf");

            Assert.AreEqual(1, doc.Pages.Count);
            var pg = doc.Pages[0];
            Assert.IsInstanceOfType(pg, typeof(Section));
            var section = (Section) pg;
            Assert.AreEqual(6, section.Contents.Count);

            var h1 = section.Contents[0] as Head1;
            var h2 = section.Contents[1] as Head2;
            var h3 = section.Contents[2] as Head3;
            var h4 = section.Contents[3] as Head4;
            var h5 = section.Contents[4] as Head5;
            var h6 = section.Contents[5] as Head6;

            ValidateHeading(h1, typeof(HTMLHead1), "h1", "Heading 1", "Heading 1 Title");
            ValidateHeading(h2, typeof(HTMLHead2), "h2", "Heading 2", "Heading 2 Title");
            ValidateHeading(h3, typeof(HTMLHead3), "h3", "Heading 3", "Heading 3 Title");
            ValidateHeading(h4, typeof(HTMLHead4), "h4", "Heading 4", "Heading 4 Title");
            ValidateHeading(h5, typeof(HTMLHead5), "h5", "Heading 5", "Heading 5 Title");
            ValidateHeading(h6, typeof(HTMLHead6), "h6", "Heading 6", "Heading 6 Title");

            Assert.IsNotNull(h6);
            Assert.AreEqual("Head6", h6.StyleClass, "The heading 6 should have the style class of 'Head6'");
            Assert.AreEqual(20.0, h6.Style.Padding.All.PointsValue,"The padding should be 20pt all around for h6");
            doc.SaveAsPDF(stream);
 
        }

        private void ValidateHeading(HeadingBase head, Type type, string id, string outline, string literalContent)
        {
            Assert.IsNotNull(head, "The heading for " + id + " was null");
            Assert.IsInstanceOfType(head, type,"The heading " + id + " was not of type " + type.FullName);
            Assert.AreEqual(id, head.ID,"Expected the ID of the heading to be " + id);
            Assert.AreEqual(outline, head.OutlineTitle, "Expected the outline for " + id + " to be " + outline);
            Assert.AreEqual(1, head.Contents.Count, "Expected 1 item in the heading contents for " + id);
            var literal = head.Contents[0] as TextLiteral;
            Assert.IsNotNull(literal, "The literal content could not be found, or was not of the correct type");
            Assert.AreEqual(literalContent, literal.Text, "Expected the literal content of " + id + " to be " + literalContent);
        }
        
        #endregion

        #region Lists and list item validation
        
        [TestMethod]
        public void ComponentLists_Test()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <ul id='ulId' class='Unordered' title='Unordered List' data-li-group='Unordered' 
        data-li-concat='true' data-li-prefix='#' data-li-postfix='.' data-li-inset='30pt' 
        data-li-align='center' data-li-style='Bullet' >
        <li id='ulOne' class='liClass1' >First Unordered Item</li>
        <li id='ulTwo' class='liClass2' hidden='hidden' >Second Unordered Item</li>
    </ul>

    <ol id='olId' class='Ordered' hidden='hidden' title='Ordered List' data-li-group='Ordered' 
        data-li-concat='false' data-li-prefix='-' data-li-postfix='--' data-li-inset='40pt' 
        data-li-align='Right' data-li-style='UppercaseRoman' >
        <li id='olOne' class='oliClass1' >First Ordered Item</li>
        <li id='olTwo' class='oliClass2' hidden='hidden'
            style='padding:20pt'
            data-li-alignment='Right' data-li-inset='60pt' data-li-label='OLLabel' title='OL2 Title' >Second Ordered Item</li>
    </ol>
</body>
</html>";

            using var sr = new System.IO.StringReader(html);
            using var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            using var stream = DocStreams.GetOutputStream("ComponentLists.pdf");

            Assert.AreEqual(1, doc.Pages.Count);
            var pg = doc.Pages[0];
            Assert.IsInstanceOfType(pg, typeof(Section));
            var section = (Section) pg;
            Assert.AreEqual(2, section.Contents.Count);

            var ul = section.Contents[0] as ListUnordered;
            var ol = section.Contents[1] as ListOrdered;

            Assert.IsNotNull(ul);
            ValidateList(ul, typeof(HTMLListUnordered), "ulId", "Unordered", false, "Unordered List", "Unordered"
                , true, "#", ".", 30.0, HorizontalAlignment.Center, ListNumberingGroupStyle.Bullet);
            
            Assert.AreEqual(2, ul.Items.Count);
            Assert.AreEqual(2, ul.Contents.Count);

            var l1 = ul.Items[0];
            var l2 = ul.Items[1];
            
            AssertListItem(l1, typeof(HTMLListItem), "ulOne", "liClass1",
                true, "First Unordered Item");
            AssertListItem(l2, typeof(HTMLListItem),"ulTwo","liClass2", 
                false, "Second Unordered Item");
            
            
            
            
            Assert.IsNotNull(ol);
            ValidateList(ol, typeof(HTMLListOrdered), "olId", "Ordered", true, "Ordered List", "Ordered"
                , false, "-", "--", 40.0, HorizontalAlignment.Right, ListNumberingGroupStyle.UppercaseRoman);

             l1 = ol.Items[0];
             l2 = ol.Items[1];
            
            AssertListItem(l1, typeof(HTMLListItem), "olOne", "oliClass1",
                true, "First Ordered Item");
            AssertListItem(l2, typeof(HTMLListItem),"olTwo","oliClass2", 
                false, "Second Ordered Item");
            
            // Secondary properties
            // data-li-alignment='Right' data-li-inset='60pt' data-li-label='OLLabel' title='OL2 Title'
            Assert.AreEqual(HorizontalAlignment.Right, l2.NumberAlignment, "ListItem alignment did not match for " + l2.ID);
            Assert.AreEqual(60.0, l2.NumberInset.PointsValue, "ListItem inset did not match for " + l2.ID);
            Assert.AreEqual("OLLabel", l2.ItemLabelText, "ListItem label did not match for " + l2.ID);
            Assert.AreEqual("OL2 Title", l2.OutlineTitle, "ListItem outline did not match for " + l2.ID);
            Assert.AreEqual(20.0, l2.Style.Padding.All, "ListItem padding did not match for " + l2.ID);
            
            doc.SaveAsPDF(stream);
        }

        private static void AssertListItem(ListItem li, Type type, string id, string className, bool visible, string content)
        {
            Assert.IsInstanceOfType(li, type, "ListItem types did not match for " + id);
            Assert.AreEqual(id, li.ID, "ListItem ids did not match for " + id);
            Assert.AreEqual(className, li.StyleClass, "ListItem classes did not match for " + id);
            Assert.AreEqual(visible, li.Visible, "ListItem visibility did not match for " + id);

            Assert.AreEqual(li.Contents.Count, 1, "ListItem inner count did not match for " + id);
            var liLiteral = li.Contents[0] as TextLiteral;
            Assert.IsNotNull(liLiteral, "ListItem types did not match for " + id);
            Assert.AreEqual(content, liLiteral.Text, "ListItem literal content did not match for " + id);
            
        }

        private void ValidateList(ListBase list, Type type, string id, string className, bool hidden,
            string title, string groupName, bool concatenated, string prefix,
            string postfix, double inset, HorizontalAlignment align, ListNumberingGroupStyle numberStyle)
        {
            Assert.IsInstanceOfType(list, type, "List types did not match for " + id);
            Assert.AreEqual(id, list.ID,"List IDs did not match for " + id);
            Assert.AreEqual(className, list.StyleClass,"List classes did not match for " + id);
            Assert.AreEqual(hidden, !list.Visible,"List visible did not match for " + id);
            Assert.AreEqual(title, list.OutlineTitle,"List outlines did not match for " + id);
            Assert.AreEqual(groupName, list.NumberingGroup,"List number groups did not match for " + id);
            Assert.AreEqual(concatenated, list.ConcatenateNumberWithParent,"List concatenation did not match for " + id);
            Assert.AreEqual(prefix, list.NumberPrefix,"List prefixes did not match for " + id);
            Assert.AreEqual(postfix, list.NumberPostfix,"List postfixes did not match for " + id);
            Assert.AreEqual(inset, list.NumberInset.PointsValue, "List insets did not match for " + id);
            Assert.AreEqual(align, list.NumberAlignment,"List alignments did not match for " + id);
            Assert.AreEqual(numberStyle, list.NumberingStyle,"List number styles did not match for " + id );
        }
        
        #endregion
        
        //TODO: List definition tests

        #region Span and sub classes - b, i, u, strong, em, strike, code
        
        /// <summary>
        /// Tests each of the spans - span, b, i, u, strong, em, strike, code
        /// </summary>
        [TestMethod]
        public void ComponentSpans_Test()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <span id='Outer' class='spanClass' style='padding:20pt' hidden='' title='Outer Span' >
        <b class='spanStrong' id='bSpan' style='color:red;' hidden='hidden' title='Bold Span' >This is strong</b>
        <i class='spanEmphasis' id='iSpan' style='color:red;' hidden='hidden' title='Italic Span' >This is emphasis</i>
        <u class='spanUnder' id='uSpan' style='color:green;' hidden='hidden' title='Underlined Span' >This is underlined</u>
        <strong class='spanStrong2' id='strongSpan' style='color:blue;' hidden='hidden' title='Strong Span' >This is strong as well</strong>
        <em class='spanEmphasis2' id='emSpan' style='color:gray;' hidden='hidden' title='Emphasis Span' >This is emphasis as well</em>
        <strike class='spanStrike' id='strikeSpan' style='color:yellow;' hidden='hidden' title='Strike Span' >This is strike through</strike>
        <code class='spanCode' id='codeSpan' style='color:maroon;' hidden='hidden' title='Code Span' >This is Code</code>
        <font face='Times' size='30' color='green' >This is the legacy font element</font>
    </span>
</body>
</html>";
            
            using var sr = new System.IO.StringReader(html);
            using var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            using var stream = DocStreams.GetOutputStream("ComponentSpans.pdf");

            Assert.AreEqual(1, doc.Pages.Count);
            var pg = doc.Pages[0];
            Assert.IsInstanceOfType(pg, typeof(Section));
            var section = (Section) pg;
            Assert.AreEqual(1, section.Contents.Count);

            var outer = section.Contents[0] as SpanBase;
            Assert.IsNotNull(outer);
            ValidateSpan(outer, typeof(HTMLSpan), "Outer", "spanClass", 20, null, true,
                "Outer Span", null);
            
            Assert.AreEqual(8, outer.Contents.Count);
            
            ValidateSpan(outer.Contents[0] as SpanBase, typeof(HTMLBoldSpan),"bSpan",
                "spanStrong", null, StandardColors.Red, false,
                "Bold Span", "This is strong");
            
            ValidateSpan(outer.Contents[1] as SpanBase, typeof(HTMLItalicSpan),"iSpan",
                "spanEmphasis", null, StandardColors.Red, false,
                "Italic Span", "This is emphasis");
            ValidateSpan(outer.Contents[2] as SpanBase, typeof(HTMLUnderlinedSpan),"uSpan",
                "spanUnder", null, StandardColors.Green, false,
                "Underlined Span", "This is underlined");
            ValidateSpan(outer.Contents[3] as SpanBase, typeof(HTMLStrong),"strongSpan",
                "spanStrong2", null, StandardColors.Blue, false,
                "Strong Span", "This is strong as well");
            
            ValidateSpan(outer.Contents[4] as SpanBase, typeof(HTMLEmphasis),"emSpan",
                "spanEmphasis2", null, StandardColors.Gray, false,
                "Emphasis Span", "This is emphasis as well");
            
            ValidateSpan(outer.Contents[5] as SpanBase, typeof(HTMLStrikeSpan),"strikeSpan",
                "spanStrike", null, StandardColors.Yellow, false,
                "Strike Span", "This is strike through");
            
            ValidateSpan(outer.Contents[6] as SpanBase, typeof(HTMLCodeSpan),"codeSpan",
                "spanCode", null, StandardColors.Maroon, false,
                "Code Span", "This is Code");

            //Special case for the font element
            var font = outer.Contents[7] as HTMLFontSpan;
            Assert.IsNotNull(font);
            Assert.AreEqual("Times", font.FontFamily.ToString(),"The font family did not match");
            Assert.AreEqual(30, font.FontSize.PointsValue, "The font size did not match");
            Assert.AreEqual(StandardColors.Green, font.FillColor, "The font color did not match");
            doc.SaveAsPDF(stream);
            
            
        }

        private void ValidateSpan(SpanBase span, Type type, string id, string className, double? padding, Color? color,
            bool visible, string title, string content)
        {
            Assert.IsNotNull(span);
            Assert.IsInstanceOfType(span, type, "The span type did not match for " + id);
            Assert.AreEqual(id, span.ID,"The span id did not match " + id);
            Assert.AreEqual(className, span.StyleClass,"The span id did not match " + id);
            if (padding.HasValue)
                Assert.AreEqual(padding.Value, span.Style.Padding.All, "The span padding did not match " + id);
            if(color.HasValue)
                Assert.AreEqual(color.Value, span.Style.Fill.Color, "The span color did not match " + id);
            
            if(!string.IsNullOrEmpty(title))
                Assert.AreEqual(title, span.OutlineTitle, "The span outline did not match for " + id);

            if (null != content)
            {
                Assert.AreEqual(1, span.Contents.Count, "The span contents count did not match for " + id);
                Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral), "The span contents type did not match for " + id);
                var lit = (TextLiteral) span.Contents[0];
                Assert.AreEqual(content, lit.Text, "The span contents text did not match for " + id);
            }
        }
        
        #endregion
        
        #region Div blocks and sub classes - article, section, blockQuote, main, nav, p, pre, fieldSet, legend

        [TestMethod]
        public void ComponentBlocks_Test()
        {
            //div, article, section, blockQuote, main, nav, p, pre, fieldSet, legend
            var html = @"<?scryber parser-mode='strict' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <div id='OuterDiv' class='divClass' style='padding:60pt' hidden='' title='Outer Div'>
        <article id='Article' class='articleClass' style='padding:10pt' title='Article Title' >
            <header>
                This is the article header
            </header>
            This is the article content
            <footer>
                This is the article footer
            </footer>
        </article>
        <section id='Section' class='sectionClass' style='padding:20pt' title='Section Title' >
            <header>
                This is the section header
            </header>
            This is the section content
            <footer>
                This is the section footer
            </footer>
        </section>
        <blockquote id='quote' class='quoteClass' style='padding:30pt' hidden='hidden' title='A Block Quote'>
            This is the block quote content
        </blockquote>
        <main id='main' class='mainClass' style='padding:40pt' hidden='' title='A Main Element'>
            This is the main content
        </main>
        <nav id='navigation' class='navClass' style='padding:35pt' hidden='hidden' title='A Nav Element'>
            This is the nav content
        </nav>
        <p id='para' class='paraClass' style='padding:25pt' hidden='' title='A Para Element'>
            This is the paragraph content
        </p>
        <pre id='preformatted' class='preClass' style='padding:15pt' hidden='hidden' title='A Preformatted Element' >
            This is the pre-formatted content
        </pre>
        <fieldset id='field' class='fieldClass' style='padding:5pt' hidden='' title='A Fieldset Element' >
            <legend id='legend' class='legendClass' style='padding:8pt' hidden='hidden' title='fieldset legend' >This is the legend</legend>
            This is the field set content.
        </fieldset>

    </div>
</body>
</html>";
            
            using var sr = new System.IO.StringReader(html);
            using var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            using var stream = DocStreams.GetOutputStream("ComponentBlocks.pdf");
            doc.SaveAsPDF(stream);
            
            Assert.AreEqual(1, doc.Pages.Count);
            var pg = doc.Pages[0];
            Assert.IsInstanceOfType(pg, typeof(Section));
            
            Assert.AreEqual(1, ((Section)pg).Contents.Count);

            //div id='OuterDiv' class='divClass' style='padding:60pt' hidden='' title='Outer Div'
            var outer = ((Section)pg).Contents[0] as Panel;
            
            var componentCount = 8;
            Assert.IsNotNull(outer, "outer was null");
            AssertBlock(outer, typeof(HTMLDiv), "OuterDiv", "divClass", 60.0, true, "Outer Div");
            Assert.AreEqual(componentCount, outer.Contents.Count);

            
            
            //article id='Article' class='articleClass' style='padding:10pt' title='Article Title' 
            var article = outer.Contents[0] as HTMLArticle;
            Assert.IsNotNull(article,"The first element was not an article");
            AssertBlock(article, typeof(HTMLArticle), "Article", "articleClass", 10.0, true, "Article Title");
            AssertLiteralContent(article, 1, "This is the article content");
            
            // article header
            var head = article.Contents[0] as HTMLComponentHeader;
            AssertLiteralContent(head, 0, "This is the article header");
            
            // article footer
            var foot = article.Contents[2] as HTMLComponentFooter;
            AssertLiteralContent(foot, 0, "This is the article footer");
            
            
            
            //section id='Section' class='sectionClass' style='padding:20pt' title='Section Title'
            var section = outer.Contents[1] as HTMLSection;
            Assert.IsNotNull(section,"The first element was not a section");
            AssertBlock(section, typeof(HTMLSection), "Section", "sectionClass", 20.0, true, "Section Title");
            AssertLiteralContent(section, 1, "This is the section content");
            
            //section header
            head = section.Contents[0] as HTMLComponentHeader;
            AssertLiteralContent(head, 0, "This is the section header"); 
            
            //section footer
            foot = section.Contents[2] as HTMLComponentFooter;
            AssertLiteralContent(foot, 0, "This is the section footer");
            
            
            //block quote id='quote' class='quoteClass' style='padding:30pt' hidden='hidden' title='A Block Quote'
            var quote = outer.Contents[2] as HTMLBlockQuote;
            Assert.IsNotNull(quote, "The second element was not a block quote " + outer.Contents[2].GetType());
            AssertBlock(quote, typeof(HTMLBlockQuote), "quote", "quoteClass", 30.0, false, "A Block Quote");
            AssertLiteralContent(quote, 0, "This is the block quote content");
            
            
            //main id='main' class='mainClass' style='padding:40pt' hidden='' title='A Main Element'
            var main = outer.Contents[3] as HTMLMain;
            Assert.IsNotNull(main, "The third element was not a main element " + outer.Contents[3].GetType());
            AssertBlock(main, typeof(HTMLMain), "main", "mainClass", 40.0, true, "A Main Element");
            AssertLiteralContent(main, 0, "This is the main content");
            
            
            //nav id='navigation' class='navClass' style='padding:35pt' hidden='hidden' title='A Nav Element'
            var nav = outer.Contents[4] as HTMLNav;
            Assert.IsNotNull(nav, "The fourth element was not a nav element " + outer.Contents[4].GetType());
            AssertBlock(nav, typeof(HTMLNav), "navigation", "navClass", 35.0, false, "A Nav Element");
            AssertLiteralContent(nav, 0, "This is the nav content");
            
            
            //p id='para' class='paraClass' style='padding:25pt' hidden='' title='A Para Element'
            var p = outer.Contents[5] as HTMLParagraph;
            Assert.IsNotNull(p, "The fifth element was not a paragraph element " + outer.Contents[5].GetType());
            AssertBlock(p, typeof(HTMLParagraph), "para", "paraClass", 25.0, true, "A Para Element");
            AssertLiteralContent(p, 0, "This is the paragraph content");
            
            //pre id='preformatted' class='preClass' style='padding:15pt' hidden='hidden' title='A Preformatted Element'
            var pre = outer.Contents[6] as HTMLPreformatted;
            Assert.IsNotNull(pre, "The sixth element was not a pre element " + outer.Contents[6].GetType());
            AssertBlock(pre, typeof(HTMLPreformatted), "preformatted", "preClass", 15.0, true, "A Preformatted Element");
            AssertLiteralContent(pre, 0, "This is the pre-formatted content");
            
            //fieldset id='field' class='fieldClass' style='padding:5pt' hidden='' title='A Fieldset Element'
            var fldSet = outer.Contents[7] as HTMLFieldSet;
            Assert.IsNotNull(fldSet, "The seventh element was not a fieldset element " + outer.Contents[7].GetType());
            AssertBlock(fldSet, typeof(HTMLFieldSet), "field", "fieldClass", 5.0, true, "A Fieldset Element");
            //content is second element
            AssertLiteralContent(fldSet, 1, "This is the field set content.");

            //inner legend id='legend' class='legendClass' style='padding:8pt' hidden='hidden' title='fieldset legend' 
            var legend = fldSet.Contents[0] as HTMLLegend;
            Assert.IsNotNull(legend);
            AssertBlock(legend, typeof(HTMLLegend),"legend", "legendClass", 8.0, false, "fieldset legend");
            AssertLiteralContent(legend, 0, "This is the legend");
        }

        private void AssertLiteralContent(IContainerComponent comp, int index, string literalText)
        {
            Assert.IsNotNull(comp,"The container was null");
            Assert.IsTrue(comp.HasContent, "The Container " + comp.ID + " does not have any content");
            Assert.IsTrue(comp.Content.Count > index,"The literal text was not at the specified index in " + comp.ID);
            var content = comp.Content[index] as TextLiteral;
            Assert.IsNotNull(content,"The literal was not at the specified index for " + comp.ID);
            Assert.AreEqual(literalText, content.Text.Trim(), "The literal text did not match in " + comp.ID);
        }
        
        private void AssertBlock(Panel panel, Type type, string id, string className, double? padding, bool visible, string title)
        {
            Assert.IsNotNull(panel, "The panel was null for " + id);
            Assert.IsInstanceOfType(panel, type, "The types did not match for " + id);
            Assert.AreEqual(panel.ID, id, "The ids did not match for " + id);
            Assert.AreEqual(panel.StyleClass, className, "The class names did not match for " + id);
            if (padding.HasValue)
                Assert.AreEqual(padding.Value, panel.Style.Padding.All, "The padding did not match for " + id);
            Assert.AreEqual(title, panel.OutlineTitle, "The outline titles did not match for " + id);
        }
        

        #endregion
        
        #region Tables, Rows, Cells and thead, tbody, tfoot
        
        

        [TestMethod]
        public void ComponentTables_Test()
        {
            //table, tr, td, thead, tfoot
            var html = @"<?scryber parser-mode='strict' ?>
    <html xmlns='http://www.w3.org/1999/xhtml' >
        <body style='padding:20pt;' >
            <!-- Simple Table -->
            <table id='simpleTable' class='tblClass' style='padding:5pt' hidden='' title='Simple Table' >
                <tr id='simpleRow' class='trClass' style='padding:5pt' hidden='hidden' title='row' >
                    <td id='simpleCell' class='tdClass' style='padding:5pt' hidden='hidden' title='cell'>
                        In the cell content
                    </td>
                </tr>
                <tr id='simpleRow2' class='trClass' style='padding:10pt' hidden='' title='row' >
                    <td id='simpleCell' class='tdClass' style='padding:5pt' hidden='' title='cell'>
                        In the row 2 cell content
                    </td>
                </tr>
            </table>
            
            <!-- Table with complex content -->
            <table id='complexTable' class='tblComplex' style='padding:10pt;' hidden='' title='Complex Table' >
                <thead id='complexTHead' class='theadComplex' title='Header'>
                    <tr id='complexThRow' class='thRowComplex' title='Header Row 1'>
                        <th id='complexTh1' class='thComplex' title='Header Cell 1' scope='col' >Header 1</th>
                        <th id='complexTh2' class='thComplex' title='Header Cell 2' scope='col' >Header 2</th>
                        <th id='complexTh3' class='thComplex' title='Header Cell 3' scope='col' >Header 3</th>
                    </tr>
                </thead>
                <tbody>
                    <tr id='complexTBRow' class='trRowComplex' title='Body Row 1'>
                        <td id='complexTd1' class='tdComplex' title='Cell 1' >Cell 1</td>
                        <td id='complexTd2' class='tdComplex' title='Cell 2' >Cell 2</td>
                        <td id='complexTd3' class='tdComplex' title='Cell 3' >Cell 3</td>
                    </tr>
                    <tr id='complexTBRow2' class='trRowComplex' title='Body Row 2'>
                        <td id='complexTd4' class='thComplex' title='Cell 1' >Cell 1</td>
                        <td id='complexTd5' colspan='2' class='thComplex' title='Cell 2' >Cell Spanned 2</td>
                    </tr>
                </tbody>
                <tr>
                    <th id='complexTh1' class='tdComplex' title='Single Cell 1' scope='row' >Row Header</th>
                    <td id='complexTh2' class='tdComplex' title='Single Cell 2' >Cell 2</td>
                    <td id='complexTh3' class='tdComplex' title='Single Cell 3' >Cell 3</td>
                </tr>
                <tfoot>
                    <tr id='complexTfootRow' class='tfRowComplex' title='Footer Row 1'>
                        <td id='complexTf1' class='tfootComplex' title='Footer Cell 1' >Footer 1</td>
                        <td id='complexTf2' class='tfootComplex' title='Footer Cell 2' >Footer 2</td>
                        <td id='complexTf3' class='tfootComplex' title='Footer Cell 3' >Footer 3</td>
                    </tr>
                </tfoot>
            </table>

        </body>
    </html>";
            
            using var sr = new System.IO.StringReader(html);
            using var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            using var stream = DocStreams.GetOutputStream("ComponentTables.pdf");
            //doc.SaveAsPDF(stream);
            
            Assert.AreEqual(1, doc.Pages.Count);
            var pg = doc.Pages[0];
            Assert.IsInstanceOfType(pg, typeof(Section));
            
            Assert.AreEqual(2, ((Section)pg).Contents.Count);
            var section = (Section) pg;
            
            //Simple Table
            
            //table id='simpleTable' class='tblClass' style='padding:5pt' hidden='' title='Simple Table' 
            var table = (TableGrid) section.Contents[0];
            AssertTable(table, typeof(HTMLTableGrid), "simpleTable", "tblClass", 2, 5.0, true,"Simple Table");
            
            //tr id='simpleRow' class='trClass' style='padding:5pt' hidden='hidden' title='row'
            var row = table.Rows[0];
            AssertRowContent(row, typeof(HTMLTableRow), "simpleRow", "trClass", 1, 5.0, false, "row");
            
            //td id='simpleCell' class='tdClass' style='padding:5pt' hidden='hidden' title='cell'
            var cell = row.Cells[0];
            AssertCellContent(cell, typeof(HTMLTableCell), "simpleCell", "tdClass", 1, 5.0, false, "cell", "In the cell content");
            
            //Complex Table
            table = (TableGrid) section.Contents[1];
            //table id='complexTable' class='tblComplex' style='padding:10pt;' hidden='' title='Complex Table'
            AssertTable(table, typeof(HTMLTableGrid), "complexTable","tblComplex", 5, 10.0, true, "Complex Table");
            
            //Complex Table thead contents
            
            //tr id='complexThRow' class='thRowComplex' title='Header Row 1'
            row = table.Rows[0];
            AssertRowContent(row, typeof(HTMLTableRow), "complexThRow", "thRowComplex", 3, null, true, "Header Row 1");
            Assert.IsInstanceOfType(row.Parent, typeof(HTMLTableHead), "The row's parent was not thead");
            
            //th id='complexTh1' class='thComplex' title='Header Cell 1' scope='col' >Header 1</th
            cell = row.Cells[0];
            AssertCellContent(cell, typeof(HTMLTableHeaderCell), "complexTh1", "thComplex", 1,null, true,"Header Cell 1", "Header 1" );
            
            //th id='complexTh2' class='thComplex' title='Header Cell 2' scope='col' >Header 2
            cell = row.Cells[1];
            AssertCellContent(cell, typeof(HTMLTableHeaderCell), "complexTh2", "thComplex", 1,null, true,"Header Cell 2", "Header 2" );
            Assert.AreEqual("col", ((HTMLTableHeaderCell) cell).HeaderScope, "The header cell scope was not captured");
            
            
            //Complex Table tbody contents
            
            //tr id='complexTBRow' class='trRowComplex' title='Body Row 1'
            row = table.Rows[1];
            AssertRowContent(row, typeof(HTMLTableRow), "complexTBRow", "trRowComplex", 3, null, true, "Body Row 1");
            Assert.IsInstanceOfType(row.Parent, typeof(HTMLTableBody), "The body row's parent was not tbody");

            cell = row.Cells[2]; //get the third one.
            //<td id='complexTd3' class='tdComplex' title='Cell 3' >Cell 3</td>
            AssertCellContent(cell, typeof(HTMLTableCell), "complexTd3", "tdComplex", 1,null, true,"Cell 3", "Cell 3");
            
            //tr id='complexTBRow2' class='trRowComplex' title='Body Row 2' + Spanned cell
            row = table.Rows[2];
            AssertRowContent(row, typeof(HTMLTableRow), "complexTBRow2", "trRowComplex", 2, null, true, "Body Row 2");
            Assert.IsInstanceOfType(row.Parent, typeof(HTMLTableBody), "The second body row's parent was not tbody");
            
            //td id='complexTd5' colspan='2' class='thComplex' title='Cell 2' >Cell Spanned 2
            cell = row.Cells[1];
            AssertCellContent(cell, typeof(HTMLTableCell),"complexTd5","thComplex", 2, null, true, "Cell 2", "Cell Spanned 2");
            
            
            //outside of tbody, with auto assigned id
            //tr
            row = table.Rows[3];
            AssertRowContent(row, typeof(HTMLTableRow), "trow1", null, 3, null, true, string.Empty);
            Assert.IsInstanceOfType(row.Parent, typeof(HTMLTableGrid), "The second body row's parent was not table");

            //th id='complexTh1' class='tdComplex' title='Single Cell 1' scope='row' >Row Header</th
            cell = row.Cells[0];
            AssertCellContent(cell,typeof(HTMLTableHeaderCell), "complexTh1", "tdComplex", 1, null, true, "Single Cell 1", "Row Header");
            Assert.AreEqual("row", ((HTMLTableHeaderCell)cell).HeaderScope, "Header scopes did not match");
            
            
            //tfoot
            //tr id='complexTfootRow' class='tfRowComplex' title='Footer Row 1'
            row = table.Rows[4];
            AssertRowContent(row, typeof(HTMLTableRow), "complexTfootRow", "tfRowComplex", 3, null, true, "Footer Row 1");
            Assert.IsInstanceOfType(row.Parent, typeof(HTMLTableFooter), "The footer row's parent was not tfoot");
            
            //td id='complexTf1' class='tfootComplex' title='Footer Cell 1' >Footer 1
            cell = row.Cells[0];
            AssertCellContent(cell, typeof(HTMLTableCell), "complexTf1", "tfootComplex", 1, null, true, "Footer Cell 1", "Footer 1");

            doc.SaveAsPDF(stream);
        }

        private void AssertTable(TableGrid table, Type type, string id, string className, int rowCount, double? padding,
            bool visible, string title)
        {
            Assert.IsNotNull(table,"The table was null");
            Assert.IsInstanceOfType(table, type, "The table was not the correct type :" + table.GetType().FullName);
            Assert.AreEqual(id, table.ID, "The table ID's do not match");
            Assert.AreEqual(className, table.StyleClass, "The table style classes do not match for " + table.ID);
            Assert.IsTrue(table.HasContent, "The table " + table.ID + " does not have any content");
            Assert.AreEqual(rowCount, table.Rows.Count,"The table '" + id + " row count was not " + rowCount.ToString());
            Assert.AreEqual(title, table.OutlineTitle, "The table '" + table.ID + "' titles did not match");
            Assert.AreEqual(visible, table.Visible, "The table " + table.ID + " visibility does not match");
            if(padding.HasValue)
                Assert.AreEqual(padding.Value, table.Style.Padding.All.Value, "The table padding did not match");
        }

        private void AssertRowContent(TableRow row, Type type, string id, string className, int cellCount, double? padding, bool visible, string title)
        {
            Assert.IsNotNull(row,"The row was null");
            Assert.IsInstanceOfType(row, type, "The row was not the correct type :" + row.GetType().FullName);
            Assert.AreEqual(id, row.ID, "The row ID's do not match");
            Assert.AreEqual(className, row.StyleClass, "The row style classes do not match for " + row.ID);
            Assert.IsTrue(row.HasContent, "The row " + row.ID + " does not have any content");
            Assert.AreEqual(cellCount, row.Cells.Count,"The row '" + id + " cell count was not " + cellCount.ToString());
            Assert.AreEqual(title, row.OutlineTitle, "The row '" + row.ID + "' titles dod not match");
            Assert.AreEqual(visible, row.Visible, "The row " + row.ID + " visibility does nt match");
            if(padding.HasValue)
                Assert.AreEqual(padding.Value, row.Style.Padding.All.Value, "The row padding did not match");
            
            
        }
        
        private void AssertCellContent(TableCell cell, Type type, string id, string className, int columnSpan, double? padding, bool visible, string title, string literalText)
        {
            Assert.IsNotNull(cell,"The cell was null");
            Assert.IsInstanceOfType(cell, type, "The cell was not the correct type :" + cell.GetType().FullName);
            Assert.AreEqual(id, cell.ID, "The cell ID's do not match");
            Assert.AreEqual(className, cell.StyleClass, "The cell style classes do not match for " + cell.ID);
            Assert.IsTrue(cell.HasContent, "The cell " + cell.ID + " does not have any content");
            Assert.AreEqual(columnSpan, cell.Style.Table.CellColumnSpan,"The cell '" + id + " column span was not " + columnSpan.ToString());
            Assert.AreEqual(title, cell.OutlineTitle, "The cell '" + cell.ID + "' titles dod not match");
            Assert.AreEqual(visible, cell.Visible, "The cell " + cell.ID + " visibility does nt match");
            if(padding.HasValue)
                Assert.AreEqual(padding.Value, cell.Style.Padding.All.Value, "The cell padding did not match");
            
            var content = cell.Contents[0] as TextLiteral;
            Assert.IsNotNull(content,"The literal was not at the specified index for " + cell.ID);
            Assert.AreEqual(literalText, content.Text.Trim(), "The literal text did not match in " + cell.ID);
        }
        
        #endregion
        
        #region template, iframe, embed and link
        
        
        
        #endregion

        #region label num, page, time, if

        

        #endregion
        
        
    }
}