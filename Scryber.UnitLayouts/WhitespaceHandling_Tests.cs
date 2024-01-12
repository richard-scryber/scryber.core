﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.Drawing;
using System.IO;
using Scryber.PDF.Resources;

namespace Scryber.UnitLayouts
{
	[TestClass]
	public class WhitespaceHandling_Tests
	{

        public const string WhitespaceSamplePath = "../../../Content/WhitespaceLayouts/";

        public static string LoadLayoutSample(string fileName)
        {
            var path = System.IO.Path.Combine(WhitespaceSamplePath, fileName);
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                throw new System.IO.FileNotFoundException("The whitespace sample '" + fileName + "' could not be found at path '" + path + "'");

            var text = System.IO.File.ReadAllText(path);
            return text;
        }

		public WhitespaceHandling_Tests()
		{
		}

        PDFLayoutDocument layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this.layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        

        private string WhitespaceSrc1 = "1_WhitespaceSimpleSpans.html";

        [TestMethod()]
        public void Whitespace_1_SimpleSpans()
        {
            var src = LoadLayoutSample(WhitespaceSrc1);

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("Whitespace_1_SimpleSpans.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    
                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);

                    var wrap = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrap);

                    var span = wrap.Contents[1] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    var lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("This is text.", lit.Text);

                    //Back at the outer div
                    lit = wrap.Contents[2] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(" And this is after with a white space prepended.\n    ", lit.Text);

                    //check the contents of the layout

                    Assert.IsNotNull(this.layout);

                    var lpg = this.layout.AllPages[0];
                    var body = lpg.ContentBlock;
                    Assert.AreEqual(1, body.Columns.Length);
                    Assert.AreEqual(1, body.Columns[0].Contents.Count);

                    var lwrap = body.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);

                    Assert.AreEqual(1, lwrap.Columns.Length);
                    Assert.AreEqual(1, lwrap.Columns[0].Contents.Count);
                    var line = lwrap.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.IsNotNull(line);

                    Assert.AreEqual(8, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("This is text.", (line.Runs[2] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[3], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[4], typeof(PDFLayoutInlineEnd));
                    Assert.IsInstanceOfType(line.Runs[5], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[6], typeof(PDFTextRunCharacter));
                    Assert.AreEqual(" And this is after with a white space prepended. ", (line.Runs[6] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[7], typeof(PDFTextRunEnd));

                }
            }
        }

        private string WhitespaceSrc2 = "2_ExtendedWhitespaceAndEscapes.html";

        [TestMethod()]
        public void Whitespace_2_SpansWithExtendedWhitespaceAndEscapes()
        {
            var src = LoadLayoutSample(WhitespaceSrc2);

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("Whitespace_2_SpansWithEscapes.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);

                    var wrap = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrap);

                    Assert.IsInstanceOfType(wrap.Contents[0], typeof(Whitespace));

                    var span = wrap.Contents[1] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    var lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("This    is    text.", lit.Text);

                    //space in between
                    Assert.IsInstanceOfType(wrap.Contents[2], typeof(Whitespace));


                    //Second span
                    span = wrap.Contents[3] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("& this is after\n        with a \"white\" spáce prepended.", lit.Text);

                    //check the contents of the layout

                    Assert.IsNotNull(this.layout);

                    var lpg = this.layout.AllPages[0];
                    var body = lpg.ContentBlock;
                    Assert.AreEqual(1, body.Columns.Length);
                    Assert.AreEqual(1, body.Columns[0].Contents.Count);

                    var lwrap = body.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);

                    Assert.AreEqual(1, lwrap.Columns.Length);
                    Assert.AreEqual(1, lwrap.Columns[0].Contents.Count);
                    var line = lwrap.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.IsNotNull(line);

                    Assert.AreEqual(13, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("This is text.", (line.Runs[2] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[3], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[4], typeof(PDFLayoutInlineEnd));

                    Assert.IsInstanceOfType(line.Runs[5], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[6], typeof(PDFTextRunCharacter));
                    Assert.AreEqual(" ", (line.Runs[6] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[7], typeof(PDFTextRunEnd));

                    Assert.IsInstanceOfType(line.Runs[8], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(line.Runs[9], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[10], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("& this is after with a \"white\" spáce prepended.", (line.Runs[10] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[11], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[12], typeof(PDFLayoutInlineEnd));
                }
            }
        }

        private string WhitespaceSrc3 = "3_WhitespaceWithNesting.html";

        [TestMethod()]
        public void Whitespace_3_NestedSpansWithWhitespace()
        {
            var src = LoadLayoutSample(WhitespaceSrc3);

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("Whitespace_3_NestedSpans.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);

                    var wrap = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrap);

                    
                    

                    var lit = wrap.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("\n            Hello\n            ", lit.Text);

                    
                    var span = wrap.Contents[1] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(" World!", lit.Text);

                    //space at the end
                    Assert.IsInstanceOfType(wrap.Contents[2], typeof(Whitespace));


                    //check the contents of the layout

                    Assert.IsNotNull(this.layout);

                    var lpg = this.layout.AllPages[0];
                    var body = lpg.ContentBlock;
                    Assert.AreEqual(1, body.Columns.Length);
                    Assert.AreEqual(1, body.Columns[0].Contents.Count);

                    var lwrap = body.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);

                    Assert.AreEqual(1, lwrap.Columns.Length);
                    Assert.AreEqual(1, lwrap.Columns[0].Contents.Count);
                    var line = lwrap.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.IsNotNull(line);

                    Assert.AreEqual(8, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("Hello ", (line.Runs[1] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[3], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(line.Runs[4], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[5], typeof(PDFTextRunCharacter));
                    Assert.AreEqual(" World!", (line.Runs[5] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[6], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[7], typeof(PDFLayoutInlineEnd));
                }
            }
        }

        private string WhitespaceSrc4 = "4_WhitespaceNoExtra.html";

        [TestMethod()]
        public void Whitespace_4_NothingInbetweenWithWhitespace()
        {
            var src = LoadLayoutSample(WhitespaceSrc4);

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("Whitespace_4_NoExtra.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);

                    var wrap = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrap);




                    var lit = wrap.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("\n        This is text.", lit.Text);


                    var span = wrap.Contents[1] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("And this is after WITHOUT a white space!", lit.Text);

                    //space at the end
                    Assert.IsInstanceOfType(wrap.Contents[2], typeof(Whitespace));


                    //check the contents of the layout

                    Assert.IsNotNull(this.layout);

                    var lpg = this.layout.AllPages[0];
                    var body = lpg.ContentBlock;
                    Assert.AreEqual(1, body.Columns.Length);
                    Assert.AreEqual(1, body.Columns[0].Contents.Count);

                    var lwrap = body.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);

                    Assert.AreEqual(1, lwrap.Columns.Length);
                    Assert.AreEqual(1, lwrap.Columns[0].Contents.Count);
                    var line = lwrap.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.IsNotNull(line);

                    Assert.AreEqual(8, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("This is text.", (line.Runs[1] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[3], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(line.Runs[4], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[5], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("And this is after WITHOUT a white space!", (line.Runs[5] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[6], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[7], typeof(PDFLayoutInlineEnd));
                }
            }
        }

        private string WhitespaceSrc5 = "5_WhitespaceNoExtraSpan.html";

        [TestMethod()]
        public void Whitespace_5_NothingInbetweenWithWhitespaceSpanned()
        {
            var src = LoadLayoutSample(WhitespaceSrc5);

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("Whitespace_5_NoExtraSpanned.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);

                    var wrap = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrap);

                    Assert.IsInstanceOfType(wrap.Contents[0], typeof(Whitespace));

                    var span = wrap.Contents[1] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    var lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("This is text.", lit.Text);


                    span = wrap.Contents[2] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("And this is after WITHOUT a white space!", lit.Text);

                    //space at the end
                    Assert.IsInstanceOfType(wrap.Contents[3], typeof(Whitespace));


                    //check the contents of the layout

                    Assert.IsNotNull(this.layout);

                    var lpg = this.layout.AllPages[0];
                    var body = lpg.ContentBlock;
                    Assert.AreEqual(1, body.Columns.Length);
                    Assert.AreEqual(1, body.Columns[0].Contents.Count);

                    var lwrap = body.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);

                    Assert.AreEqual(1, lwrap.Columns.Length);
                    Assert.AreEqual(1, lwrap.Columns[0].Contents.Count);
                    var line = lwrap.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.IsNotNull(line);

                    Assert.AreEqual(10, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("This is text.", (line.Runs[2] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[3], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[4], typeof(PDFLayoutInlineEnd));
                    Assert.IsInstanceOfType(line.Runs[5], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(line.Runs[6], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[7], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("And this is after WITHOUT a white space!", (line.Runs[7] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[8], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[9], typeof(PDFLayoutInlineEnd));
                }
            }
        }


        private string WhitespaceSrc6 = "6_WhitespaceNoExtraSpanAndBold.html";

        [TestMethod()]
        public void Whitespace_6_NothingInbetweenWithWhitespaceSpannedBold()
        {
            var src = LoadLayoutSample(WhitespaceSrc6);

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("Whitespace_6_NoExtraSpannedBold.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);

                    var wrap = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrap);

                    Assert.IsInstanceOfType(wrap.Contents[0], typeof(Whitespace));

                    var span = wrap.Contents[1] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    var lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("This is text.", lit.Text);


                    span = wrap.Contents[2] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(3, span.Contents.Count);

                    lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("And this is after\n            ", lit.Text);

                    var b = span.Contents[1] as BoldSpan;
                    Assert.IsNotNull(b);
                    Assert.AreEqual(1, b.Contents.Count);
                    lit = b.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("WITHOUT", lit.Text);

                    lit = span.Contents[2] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("\n        a white space!", lit.Text);

                    //space at the end
                    Assert.IsInstanceOfType(wrap.Contents[3], typeof(Whitespace));


                    //check the contents of the layout

                    Assert.IsNotNull(this.layout);

                    var lpg = this.layout.AllPages[0];
                    var body = lpg.ContentBlock;
                    Assert.AreEqual(1, body.Columns.Length);
                    Assert.AreEqual(1, body.Columns[0].Contents.Count);

                    var lwrap = body.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);

                    Assert.AreEqual(1, lwrap.Columns.Length);
                    Assert.AreEqual(1, lwrap.Columns[0].Contents.Count);
                    var line = lwrap.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.IsNotNull(line);

                    Assert.AreEqual(18, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("This is text.", (line.Runs[2] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[3], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[4], typeof(PDFLayoutInlineEnd));
                    Assert.IsInstanceOfType(line.Runs[5], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(line.Runs[6], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[7], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("And this is after ", (line.Runs[7] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[8], typeof(PDFTextRunEnd));
                    //bold
                    Assert.IsInstanceOfType(line.Runs[9], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(line.Runs[10], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[11], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("WITHOUT", (line.Runs[11] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[12], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[13], typeof(PDFLayoutInlineEnd));
                    Assert.IsInstanceOfType(line.Runs[14], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(line.Runs[15], typeof(PDFTextRunCharacter));
                    Assert.AreEqual(" a white space!", (line.Runs[15] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(line.Runs[16], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(line.Runs[17], typeof(PDFLayoutInlineEnd));
                }
            }
        }


        //Non breaking spaces

        private string WhitespaceSrc7 = "7_WhitespaceNonBreakingSpace.html";

        [TestMethod("Whitespace_7_NonBreakingSpaceLiteral")]
        public void Whitespace_7_NBSPLiteral()
        {
            var src = LoadLayoutSample(WhitespaceSrc7);

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("Whitespace_7_Nonbreakingspace.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);


                    //var wrap = pg.FindAComponentById("wrapper_placebo") as Div;
                    //Assert.IsNotNull(wrap);
                    //Assert.AreEqual(1, wrap.Contents.Count);
                    //Assert.IsInstanceOfType(wrap.Contents[0], typeof(TextLiteral));



                    //var lit = wrap.Contents[0] as TextLiteral;
                    //Assert.IsNotNull(lit);
                    //Assert.AreEqual("\n        This is a long line of text that should wrap on multiple lines for testing the non-breaking space within a line of text.\n    ", lit.Text);
                    //var index = lit.Text.IndexOf('-');
                    //Assert.IsTrue(index > 0);


                    //wrap = pg.FindAComponentById("wrapper") as Div;
                    //Assert.IsNotNull(wrap);
                    //Assert.AreEqual(1, wrap.Contents.Count);
                    //Assert.IsInstanceOfType(wrap.Contents[0], typeof(TextLiteral));

                    //lit = wrap.Contents[0] as TextLiteral;
                    //Assert.IsNotNull(lit);
                    //Assert.AreEqual("\n        This is a long line of text that should wrap on multiple lines for testing the non-breaking space within a line" + (char)160 + "of text.\n    ", lit.Text);


                    //wrap = pg.FindAComponentById("wrapper_column") as Div;
                    //Assert.IsNotNull(wrap);
                    //Assert.AreEqual(1, wrap.Contents.Count);
                    //Assert.IsInstanceOfType(wrap.Contents[0], typeof(TextLiteral));

                    //lit = wrap.Contents[0] as TextLiteral;
                    //Assert.IsNotNull(lit);
                    //Assert.AreEqual("\n            This is a long line of text that should wrap" + (char)160 + "on" + (char)160 + "multiple lines for testing" + (char)160 + "the" + (char)160 + "non-breaking" + (char)160 + "space" + (char)160 + "within a" + (char)160 + "line" + (char)160 + "of" + (char)160 + "text.\n        ", lit.Text);


                    var wrap = pg.FindAComponentById("wrapper_span") as Div;
                    Assert.IsNotNull(wrap);
                    Assert.AreEqual(3 * 2 + 1, wrap.Contents.Count);
                    Assert.IsInstanceOfType(wrap.Contents[0], typeof(Whitespace));

                    Assert.IsInstanceOfType(wrap.Contents[0], typeof(Whitespace));

                    var span = wrap.Contents[1] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    var lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("This is a long line of text that should wrap" + (char)160 + "on" + (char)160 + "multiple", lit.Text);

                    Assert.IsInstanceOfType(wrap.Contents[2], typeof(Whitespace));

                    var b = wrap.Contents[3] as BoldSpan;
                    Assert.IsNotNull(b);
                    Assert.AreEqual(1, b.Contents.Count);
                    lit = b.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("lines for testing" + (char)160 + "the" + (char)160 + "non-breaking" + (char)160 + "space" + (char)160 + "within", lit.Text);

                    Assert.IsInstanceOfType(wrap.Contents[4], typeof(Whitespace));

                    span = wrap.Contents[5] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    lit = span.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("a" + (char)160 + "line" + (char)160 + "of" + (char)160 + "text.", lit.Text);

                    //space at the end
                    Assert.IsInstanceOfType(wrap.Contents[6], typeof(Whitespace));


                    Assert.IsNotNull(this.layout);
                    var lpg = this.layout.AllPages[0];

                    var lcb = lpg.ContentBlock;
                    var lcols = lcb.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lcols);
                    Assert.AreEqual("columns", lcols.Owner.ID);
                    Assert.AreEqual(2, lcols.Columns.Length);

                    //Add other content after uncommenting

                    var lcolReg1 = lcols.Columns[0];
                    Assert.AreEqual(1, lcolReg1.Contents.Count);
                    var lwrapspan = lcolReg1.Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrapspan);

                    Assert.AreEqual("wrapper_span", lwrapspan.Owner.ID);
                    Assert.AreEqual(3, lwrapspan.Columns[0].Contents.Count);

                    var lline = lwrapspan.Columns[0].Contents[0] as PDFLayoutLine;
                    

                    Assert.IsNotNull(lline);
                    Assert.AreEqual(4, lline.Runs.Count);
                    Assert.IsInstanceOfType(lline.Runs[0], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(lline.Runs[1], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(lline.Runs[2], typeof(PDFTextRunCharacter));
                    Assert.IsInstanceOfType(lline.Runs[3], typeof(PDFTextRunNewLine));

                    lline = lwrapspan.Columns[0].Contents[1] as PDFLayoutLine;
                    Assert.IsNotNull(lline);

                    Assert.AreEqual(11, lline.Runs.Count);
                    Assert.IsInstanceOfType(lline.Runs[0], typeof(PDFTextRunSpacer));
                    Assert.IsInstanceOfType(lline.Runs[1], typeof(PDFTextRunCharacter));
                    Assert.IsInstanceOfType(lline.Runs[2], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(lline.Runs[3], typeof(PDFLayoutInlineEnd));

                    //Added whitespace between 2 inline runs
                    Assert.IsInstanceOfType(lline.Runs[4], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(lline.Runs[5], typeof(PDFTextRunCharacter));
                    Assert.AreEqual(" ", (lline.Runs[5] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(lline.Runs[6], typeof(PDFTextRunEnd));

                    Assert.IsInstanceOfType(lline.Runs[7], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(lline.Runs[8], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(lline.Runs[9], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("lines for testing" + (char)160 + "the" + (char)160 + "non-", (lline.Runs[9] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(lline.Runs[10], typeof(PDFTextRunNewLine));

                    lline = lwrapspan.Columns[0].Contents[2] as PDFLayoutLine;
                    Assert.IsNotNull(lline);
                    Assert.AreEqual(12, lline.Runs.Count);

                    Assert.IsInstanceOfType(lline.Runs[0], typeof(PDFTextRunSpacer));
                    Assert.IsInstanceOfType(lline.Runs[1], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("breaking" + (char)160 + "space" + (char)160 + "within", (lline.Runs[1] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(lline.Runs[2], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(lline.Runs[3], typeof(PDFLayoutInlineEnd));

                    //Added whitespace between 2 inline runs
                    Assert.IsInstanceOfType(lline.Runs[4], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(lline.Runs[5], typeof(PDFTextRunCharacter));
                    Assert.AreEqual(" ", (lline.Runs[5] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(lline.Runs[6], typeof(PDFTextRunEnd));

                    Assert.IsInstanceOfType(lline.Runs[7], typeof(PDFLayoutInlineBegin));
                    Assert.IsInstanceOfType(lline.Runs[8], typeof(PDFTextRunBegin));
                    Assert.IsInstanceOfType(lline.Runs[9], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("a" + (char)160 + "line" + (char)160 + "of" + (char)160 + "text.", (lline.Runs[9] as PDFTextRunCharacter).Characters);
                    Assert.IsInstanceOfType(lline.Runs[10], typeof(PDFTextRunEnd));
                    Assert.IsInstanceOfType(lline.Runs[11], typeof(PDFLayoutInlineEnd));
                }
            }

            
        }


        




    }
}

