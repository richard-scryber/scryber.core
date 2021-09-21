using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Html.Components;

namespace Scryber.UnitSamples
{
    [TestClass]
    public class ListSamples : SampleBase
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

        [TestMethod()]
        public void SimpleList()
        {
            var path = GetTemplatePath("Lists", "ListsSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Lists", "ListsSimple.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        [TestMethod()]
        public void OverflowingList()
        {
            var path = GetTemplatePath("Lists", "ListsOverflow.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Lists", "ListsOverflow.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void DefinitionList()
        {
            var path = GetTemplatePath("Lists", "ListsDefinition.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Lists", "ListsDefinition.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        [TestMethod()]
        public void NestedList()
        {
            var path = GetTemplatePath("Lists", "ListsNested.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Lists", "ListsNested.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        [TestMethod()]
        public void PrePostFixList()
        {
            var path = GetTemplatePath("Lists", "ListsPrePostFix.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Lists", "ListsPrePostFix.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        [TestMethod()]
        public void ConcatenatedList()
        {
            var path = GetTemplatePath("Lists", "ListsNestedConcatenated.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Lists", "ListsNestedConcatenated.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        [TestMethod()]
        public void NumberGroupedList()
        {
            var path = GetTemplatePath("Lists", "ListsGrouped.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Lists", "ListsGrouped.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void NumberInsetAndAlignList()
        {
            var path = GetTemplatePath("Lists", "ListsInsetAndAlign.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Lists", "ListsInsetAndAlign.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }




        [TestMethod()]
        public void CodedList()
        {
            var path = GetTemplatePath("Lists", "ListsCoded.html");

            using (var doc = Document.ParseDocument(path))
            {
                
                if (doc.TryFindAComponentById("TopDiv", out Div top))
                {
                    ListOrdered ol = new ListOrdered() {
                        NumberingStyle = ListNumberingGroupStyle.LowercaseLetters,
                        BorderColor = PDFColors.Red, BorderWidth = 1 };
                    for(var i = 1; i < 10; i ++)
                    {
                        ListItem li = new ListItem();
                        li.Contents.Add(new TextLiteral("Item #" + i));

                        //Setting the item number alignment to left individually
                        if (i == 5)
                            li.NumberAlignment = HorizontalAlignment.Left;

                        ol.Items.Add(li);
                    }
                    top.Contents.Add(ol);
                }

                if (doc.TryFindAComponentById("SecondDiv", out Div second))
                {
                    ListDefinition dl = new ListDefinition();
                    
                    for (var i = 1; i < 10; i++)
                    {
                        ListDefinitionTerm term = new ListDefinitionTerm();
                        term.Contents.Add(new TextLiteral("Term " + i));
                        dl.Items.Add(term);

                        ListDefinitionItem def = new ListDefinitionItem();
                        def.Contents.Add(new TextLiteral("Definition for term " + i));

                        //Setting the item number inset to 100 individually
                        if (i == 5)
                            def.Style.Margins.Left = 100;
                        
                        dl.Items.Add(def);

                    }
                    second.Contents.Add(dl);
                }

                using (var stream = GetOutputStream("Lists", "ListsCoded.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }
    }
}
