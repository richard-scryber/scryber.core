using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;

namespace Scryber.UnitSamples
{
    [TestClass]
    public class OverviewSamples : SampleBase
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
        public void SimpleParsing()
        {
            var path = GetTemplatePath("Overview", "SimpleParsing.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Overview", "SimpleParsing.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod]
        public void XLinqParsing()
        {

            XNamespace ns = "http://www.w3.org/1999/xhtml";

            var html = new XElement(ns + "html",
                new XElement(ns + "head",
                    new XElement(ns + "title",
                        new XText("Hello World"))
                    ),
                new XElement(ns + "body",
                    new XElement(ns + "div",
                        new XAttribute("style", "padding:10px"),
                        new XText("Hello World."))
                    )
                );

            using (var reader = html.CreateReader())
            {
                //passing an empty string to the path as we don't have images or other references to load
                using (var doc = Document.ParseDocument(reader, string.Empty, ParseSourceType.DynamicContent))
                {
                    using (var stream = GetOutputStream("Overview", "XLinqParsing.pdf"))
                    {
                        doc.SaveAsPDF(stream);
                    }
                }
            }
        }


        [TestMethod]
        public void StringParsing()
        {
            var title = "Hello World";
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>" + title + @"</title>
                    </head>
                    <body>
                        <div style='padding: 10px' >" + title + @".</div>
                    </body>
                </html>";

            using (var reader = new StringReader(src))
            {
                using (var doc = Document.ParseDocument(reader, string.Empty, ParseSourceType.DynamicContent))
                {
                    using (var stream = GetOutputStream("Overview", "StringParsing.pdf"))
                    {
                        doc.SaveAsPDF(stream);
                    }
                }
            }
        }


        protected Document GetHelloWorld()
        {
            var doc = new Document();
            doc.Info.Title = "Hello World";

            var page = new Page();
            doc.Pages.Add(page);

            var div = new Div() { Padding = new PDFThickness(10) };
            page.Contents.Add(div);

            div.Contents.Add(new TextLiteral("Hello World"));

            return doc;
        }


        [TestMethod]
        public void DocumentInCode()
        {

            using (var doc = GetHelloWorld())
            {
                using (var stream = GetOutputStream("Overview", "CodedDocument.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod()]
        public void EmbedContent()
        {
            var path = GetTemplatePath("Overview", "EmbeddedContent.html");

            using (var doc = Document.ParseDocument(path))
            {
                //Embedded content is loaded at parse time
                var embedded = doc.FindAComponentById("MyTsAndCs") as Div;
                Assert.IsNotNull(embedded);
                
                using (var stream = GetOutputStream("Overview", "EmbeddedContent.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        public void SimpleBinding()
        {
            var path = GetTemplatePath("Overview", "SimpleBinding.html");

            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["title"] = "Hello World";

                //Before databinding - value is not set
                Assert.IsNull(doc.Info.Title);

                using (var stream = GetOutputStream("Overview", "SimpleBinding.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                //After databinding
                Assert.AreEqual("Hello World", doc.Info.Title);
            }
        }

        public class User
        {
            public string Salutation { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

        [TestMethod]
        public void ComplexBinding()
        {
            var path = GetTemplatePath("Overview", "ComplexBinding.html");

            using (var doc = Document.ParseDocument(path))
            {
                var user = new User() { Salutation = "Mr", FirstName = "Richard", LastName = "Smith" };

                doc.Params["model"] = new
                {
                    user = user
                };
                doc.Params["theme"] = new
                {
                    color = "#FF0000",
                    space = "10pt",
                    align = "center"
                };

                using (var stream = GetOutputStream("Overview", "ComplexBinding.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }


        public class Order
        {

            public int ID { get; set; }

            public string CurrencyFormat { get; set; }

            public double TaxRate { get; set; }

            public double Total { get; set; }

            public List<OrderItem> Items { get; set; }
        }


        public class OrderItem
        {

            public string ItemNo { get; set; }

            public string ItemName { get; set; }

            public double Quantity { get; set; }

            public double ItemPrice { get; set; }

        }


        public class OrderMockService
        {

            public Order GetOrder(int id)
            {
                var order = new Order() { ID = id, CurrencyFormat = "£##0.00", TaxRate = 0.2 };
                order.Items = new List<OrderItem>(){
                    new OrderItem() { ItemNo = "O 12", ItemName = "Widget", Quantity = 2, ItemPrice = 12.5 },
                    new OrderItem() { ItemNo = "O 17", ItemName = "Sprogget", Quantity = 4, ItemPrice = 1.5 },
                    new OrderItem() { ItemNo = "I 13", ItemName = "M10 bolts with a counter clockwise thread on the inner content and a star nut top, tamper proof and locking ring included.", Quantity = 8, ItemPrice = 1.0 }
                };
                order.Total = (2.0 * 12.5) + (4.0 * 1.5) + (8 * 1.0);

                return order;
            }

        }


        [TestMethod]
        public void LoopBinding()
        {
            var path = GetTemplatePath("Overview", "LoopBinding.html");

            using (var doc = Document.ParseDocument(path))
            {
                var service = new OrderMockService();
                var user = new User() { Salutation = "Mr", FirstName = "Richard", LastName = "Smith" };
                var order = service.GetOrder(1);

                doc.Params["model"] = new
                {
                    user = user,
                    order = order
                };

                doc.Params["theme"] = new
                {
                    color = "#FF0000",
                    space = "10pt",
                    align = "center"
                };

                

                using (var stream = GetOutputStream("Overview", "LoopBinding.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }


        public class OrderWithTerms : Order
        {
            public int PaymentTerms { get; set; }

        }

        public class OrderMockService2
        {

            public Order GetOrder(int id)
            {
                var order = new OrderWithTerms() { ID = id, CurrencyFormat = "£##0.00", TaxRate = 0.2 };
                order.Items = new List<OrderItem>(){
                    new OrderItem() { ItemNo = "O 12", ItemName = "Widget", Quantity = 2, ItemPrice = 12.5 },
                    new OrderItem() { ItemNo = "O 17", ItemName = "Sprogget", Quantity = 4, ItemPrice = 1.5 },
                    new OrderItem() { ItemNo = "I 13", ItemName = "M10 bolts with a counter clockwise thread on the inner content and a star nut top, tamper proof and locking ring included.", Quantity = 8, ItemPrice = 1.0 }
                };
                order.Total = (2.0 * 12.5) + (4.0 * 1.5) + (8 * 1.0);
                order.PaymentTerms = 0;
                return order;
            }

        }

        [TestMethod]
        public void ChoicesBinding()
        {
            var path = GetTemplatePath("Overview", "ChoicesBinding.html");

            using (var doc = Document.ParseDocument(path))
            {
                //Use mock service 2
                var service = new OrderMockService2();

                var user = new User() { Salutation = "Mr", FirstName = "Richard", LastName = "Smith" };
                var order = service.GetOrder(1);

                doc.Params["model"] = new
                {
                    user = user,
                    order = order
                };

                doc.Params["theme"] = new
                {
                    color = "#FF0000",
                    space = "10pt",
                    align = "center"
                };



                using (var stream = GetOutputStream("Overview", "ChoicesBinding.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod]
        public void AggregationAndCalcBinding()
        {
            var path = GetTemplatePath("Overview", "AggregationAndCalcBinding.html");

            using (var doc = Document.ParseDocument(path))
            {
                //Use mock service 2
                var service = new OrderMockService2();

                var user = new User() { Salutation = "Mr", FirstName = "Richard", LastName = "Smith" };
                var order = service.GetOrder(1);

                doc.Params["model"] = new
                {
                    user = user,
                    order = order
                };

                doc.Params["theme"] = new
                {
                    color = "#FF0000",
                    space = "10pt",
                    align = "center"
                };

                using (var stream = GetOutputStream("Overview", "AggregationAndCalcBinding.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }


        [TestMethod]
        public void ComponentStyles()
        {
            var path = GetTemplatePath("Overview", "StylingComponents.html");

            using (var doc = Document.ParseDocument(path))
            {
                //Use mock service 2
                var service = new OrderMockService2();

                var user = new User() { Salutation = "Mr", FirstName = "Richard", LastName = "Smith" };
                var order = service.GetOrder(1);
                

                doc.Params["model"] = new
                {
                    user = user,
                    order = order
                };

                using (var stream = GetOutputStream("Overview", "StylingComponents.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }


        [TestMethod]
        public void StylesWithCSSLink()
        {
            
            var path = GetTemplatePath("Overview", "StylingWithCSSLink.html");

            using (var doc = Document.ParseDocument(path))
            {
                //Use mock service 2
                var service = new OrderMockService2();

                var user = new User() { Salutation = "Mr", FirstName = "Richard", LastName = "Smith" };
                var order = service.GetOrder(1);


                doc.Params["model"] = new
                {
                    user = user,
                    order = order
                };

                using (var stream = GetOutputStream("Overview", "StylingWithCSSLink.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

    }
}
