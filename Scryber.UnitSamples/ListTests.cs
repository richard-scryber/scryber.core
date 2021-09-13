using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.UnitSamples
{
    [TestClass]
    public class ListTests : TestBase
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
        public void CodedList()
        {
            var path = GetTemplatePath("Lists", "ListsCoded.html");

            using (var doc = Document.ParseDocument(path))
            {
                
                if (doc.TryFindAComponentById("TopDiv", out Div top))
                {
                    ListOrdered ol = new ListOrdered() { NumberingStyle = ListNumberingGroupStyle.LowercaseLetters };
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
                    dl.NumberAlignment = HorizontalAlignment.Left;
                    dl.NumberInset = 50;

                    for (var i = 1; i < 10; i++)
                    {
                        ListItem li = new ListItem() { ItemLabelText = "Item #" + i };
                        li.Contents.Add(new TextLiteral("Definition for item " + i));

                        //Setting the item number inset to 50 individually
                        if (i == 5)
                            li.NumberInset = 100;
                        
                        dl.Items.Add(li);

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
