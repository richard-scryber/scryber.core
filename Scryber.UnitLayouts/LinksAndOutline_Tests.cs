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
    public class LinksAndOutline_Tests
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
        public void SimpleActionLinks()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            var last = new Link();
            last.Contents.Add(new TextLiteral("Last Page"));
            last.Action = LinkAction.LastPage;

            section.Contents.Add(last);
            section.Contents.Add(new PageBreak());

            var next = new Link();
            next.Contents.Add(new TextLiteral("Next Page"));
            next.Action = LinkAction.NextPage;

            section.Contents.Add(next);
            section.Contents.Add(new PageBreak());


            var prev = new Link();
            prev.Contents.Add(new TextLiteral("Previous Page"));
            prev.Action = LinkAction.PrevPage;

            section.Contents.Add(prev);
            section.Contents.Add(new PageBreak());

            var first = new Link();
            first.Contents.Add(new TextLiteral("First Page"));
            first.Action = LinkAction.FirstPage;

            section.Contents.Add(first);

            using (var ms = DocStreams.GetOutputStream("Links_SimpleActionLinks.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(4, layout.AllPages.Count);

            CheckPageLinkAction(layout.AllPages[0], last, LinkAction.LastPage);
            CheckPageLinkAction(layout.AllPages[1], next, LinkAction.NextPage);
            CheckPageLinkAction(layout.AllPages[2], prev, LinkAction.PrevPage);
            CheckPageLinkAction(layout.AllPages[3], first, LinkAction.FirstPage);

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleDestinationLinks()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            var uri = new Link();
            uri.Contents.Add(new TextLiteral("Google"));
            uri.Action = LinkAction.Uri;
            
            uri.File = "https://www.google.com";

            section.Contents.Add(uri);
            section.Contents.Add(new PageBreak());

            var comp = new Link();
            comp.Contents.Add(new TextLiteral("To Component"));
            comp.Action = LinkAction.Destination;
            comp.Destination = "#DestinationComponent";
            comp.DestinationFit = OutlineFit.BoundingBox;

            section.Contents.Add(comp);
            section.Contents.Add(new PageBreak());

            var dest = new Span();
            dest.Contents.Add(new TextLiteral("Linked to from previous page"));
            dest.ID = "DestinationComponent";
            section.Contents.Add(dest);

            using (var ms = DocStreams.GetOutputStream("Links_SimpleDestinationLinks.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(3, layout.AllPages.Count);

            CheckPageLinkAction(layout.AllPages[0], uri, LinkAction.Uri, "https://www.google.com");
            var destAction = CheckPageLinkAction(layout.AllPages[1], comp, LinkAction.Destination, comp.Destination) as PDFDestinationAction;

            Assert.AreEqual(destAction.Destination.Component, dest);
            Assert.AreEqual(destAction.Destination.Fit, OutlineFit.BoundingBox);

            var arrange = dest.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            Assert.AreEqual(2, arrange.PageIndex);


        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleOutlineLinks()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OutlineTitle = "Top Section";
            doc.Pages.Add(section);

            var div1 = new Div();
            div1.Contents.Add(new TextLiteral("On the first page"));
            div1.OutlineTitle = "Page 1";

            section.Contents.Add(div1);

            section.Contents.Add(new PageBreak());

            var div2 = new Div();
            div2.Contents.Add(new TextLiteral("On the second page"));
            div2.OutlineTitle = "Page 2";

            section.Contents.Add(div2);

            section.Contents.Add(new PageBreak());

            var div3 = new Div();
            div3.Contents.Add(new TextLiteral("On the third page"));
            div3.OutlineTitle = "Page 3";
            div3.Outline.OutlineOpen = true;

            section.Contents.Add(div3);

            div3.Contents.Add(new LineBreak());

            var inner = new Span();
            inner.Contents.Add(new TextLiteral("Inner item"));
            inner.OutlineTitle = "Inner";
            div3.Contents.Add(inner);

            using (var ms = DocStreams.GetOutputStream("Links_SimpleOutlineLinks.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(3, layout.AllPages.Count);

            IArtefactCollection coll;
            Assert.IsTrue(layout.Artefacts.TryGetCollection(PDFArtefactTypes.Outlines, out coll));

            Assert.IsNotNull(coll);
            Assert.IsInstanceOfType(coll, typeof(PDFOutlineStack));

            var stack = coll as PDFOutlineStack;
            Assert.AreEqual(1, stack.Roots.Count);
            var top = stack.Roots[0];

            Assert.AreEqual("Top Section", top.Outline.Title);
            Assert.IsFalse(top.Outline.OutlineOpen);
            Assert.AreEqual(section.UniqueID, top.Outline.DestinationName);
            Assert.IsTrue(top.HasInnerItems);
            Assert.AreEqual(3, top.InnerItems.Count);

            var one = top.InnerItems[0];
            var two = top.InnerItems[1];
            var three = top.InnerItems[2];

            Assert.AreEqual("Page 1", one.Outline.Title);
            Assert.IsFalse(one.Outline.OutlineOpen);
            Assert.AreEqual(div1.UniqueID, one.Outline.DestinationName);
            Assert.IsFalse(one.HasInnerItems);

            Assert.AreEqual("Page 2", two.Outline.Title);
            Assert.IsFalse(two.Outline.OutlineOpen);
            Assert.AreEqual(div2.UniqueID, two.Outline.DestinationName);
            Assert.IsFalse(one.HasInnerItems);

            Assert.AreEqual("Page 3", three.Outline.Title);
            Assert.IsTrue(three.Outline.OutlineOpen);
            Assert.AreEqual(div3.UniqueID, three.Outline.DestinationName);
            Assert.IsTrue(three.HasInnerItems);

            var inn = three.InnerItems[0];

            Assert.AreEqual("Inner", inn.Outline.Title);
            Assert.IsFalse(inn.Outline.OutlineOpen);
            Assert.AreEqual(inner.UniqueID, inn.Outline.DestinationName);
            Assert.IsFalse(inn.HasInnerItems);

            //The catalog names tree contains the destinations for each of the outline items
            //based on the name or unique id of the component as a list
            Assert.IsTrue(layout.Artefacts.TryGetCollection(PDFArtefactTypes.Names, out coll));

            var names = coll as PDFCategorisedNameDictionary;
            Assert.IsNotNull(names);

            PDFCategorisedNameTree dests;
            Assert.IsTrue(names.TryGetTree("Dests", out dests));

            var entry = dests[div1.UniqueID] as PDFDestination;
            Assert.IsNotNull(entry);

            Assert.AreEqual(div1, entry.Component);
            Assert.AreEqual(div1.UniqueID, entry.FullName);

            entry = dests[div2.UniqueID] as PDFDestination;
            Assert.IsNotNull(entry);

            Assert.AreEqual(div2, entry.Component);
            Assert.AreEqual(div2.UniqueID, entry.FullName);


            entry = dests[div3.UniqueID] as PDFDestination;
            Assert.IsNotNull(entry);

            Assert.AreEqual(div3, entry.Component);
            Assert.AreEqual(div3.UniqueID, entry.FullName);

            entry = dests[inner.UniqueID] as PDFDestination;
            Assert.IsNotNull(entry);

            Assert.AreEqual(inner, entry.Component);
            Assert.AreEqual(inner.UniqueID, entry.FullName);


        }

        private static PDFAction CheckPageLinkAction(PDFLayoutPage pg, Link link, LinkAction action, string destination = null)
        {
            Assert.AreEqual(1, pg.Artefacts.Count);
            IArtefactCollection annots;

            var line = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //line = inlineBegin, textBegin, chars, textEnd, inlineEnd

            var chars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            Assert.IsTrue(pg.Artefacts.TryGetCollection("Annots", out annots));
            Assert.IsInstanceOfType(annots, typeof(PDFAnnotationCollection));

            var col = annots as PDFAnnotationCollection;
            Assert.AreEqual(1, col.Count);

            var linkEntry = col[0] as PDFAnnotationLinkEntry;
            Assert.IsNotNull(linkEntry);

            Assert.AreEqual(action, linkEntry.Action.ActionType);
            Assert.ReferenceEquals(link, linkEntry.Component);

            var arrange = linkEntry.Component.GetFirstArrangement();
            Assert.IsNotNull(arrange);

            Assert.AreEqual(pg.PageIndex, arrange.PageIndex);
            Assert.AreEqual(0, arrange.RenderBounds.X);
            Assert.AreEqual(2.5, arrange.RenderBounds.Y);
            Assert.AreEqual(20, arrange.RenderBounds.Height); //font height
            Assert.AreEqual(chars.Width, arrange.RenderBounds.Width);

            if (!string.IsNullOrEmpty(destination))
            {
                if (action == LinkAction.Uri)
                {
                    var uri = linkEntry.Action as PDFUriDestinationAction;
                    Assert.IsNotNull(uri);
                    Assert.AreEqual(uri.Url, destination);
                }
                else if (action == LinkAction.Destination)
                {
                    var dest = linkEntry.Action as PDFDestinationAction;

                    //Check that the component we are linking to is there and has a matching name or id
                    Assert.IsNotNull(dest.Destination.Component);

                    if (destination.StartsWith("#"))
                    {
                        Assert.AreEqual(dest.Destination.Component.ID, destination.Substring(1));
                    }
                    else
                    {
                        Assert.AreEqual(dest.Destination.Component.Name, destination);
                    }
                }
            }

            return linkEntry.Action;
        }




    }
}
