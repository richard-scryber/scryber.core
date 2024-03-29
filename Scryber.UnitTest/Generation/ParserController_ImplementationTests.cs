﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Generation;
using Scryber.Components;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.Core.UnitTests.Generation
{

    public class SimpleDocument_Controller
    {
        [PDFOutlet("outerdoc")]
        public Document Document;

        [PDFOutlet("titlepage")]
        public Page TitlePage { get; set; }

        [PDFOutlet("mylabel")]
        public Label MyLabel;

        [PDFOutlet("notfound")]
        public Label NotFound;

        //
        // event handlers
        //

        public List<string> Invoked = new List<string>(); //keeps a list of all the invoked events on the controller

        [PDFAction("handlepageinit")]
        public void HandlePageInit(object sender, InitEventArgs args)
        {
            Assert.IsNotNull(args.Context);
            Invoked.Add("init");
        }

        [PDFAction("handlepageload")]
        public void HandlePageLoad(object sender, LoadEventArgs args)
        {
            Assert.IsNotNull(args.Context);
            Invoked.Add("load");
        }

        [PDFAction("handlepagebinding")]
        public void HandlePageBinding(object sender, DataBindEventArgs args)
        {
            Assert.IsNotNull(args.Context);
            Invoked.Add("binding");
        }

        [PDFAction("handlepagebound")]
        public void HandlePageBound(object sender, DataBindEventArgs args)
        {
            Assert.IsNotNull(args.Context);
            Invoked.Add("bound");
        }

        [PDFAction("handleprelayout")]
        public void HandlePagePreLayout(object sender, LayoutEventArgs args)
        {
            Assert.IsNotNull(args.Context);
            Invoked.Add("pre-layout");
        }

        [PDFAction("handlepostlayout")]
        public void HandlePagePostLayout(object sender, LayoutEventArgs args)
        {
            Assert.IsNotNull(args.Context);
            Invoked.Add("post-layout");
        }

        [PDFAction("handleprerender")]
        public void HandlePagePreRender(object sender, RenderEventArgs args)
        {
            Assert.IsNotNull(args.Context);
            Invoked.Add("pre-render");
        }

        [PDFAction("handlepostrender")]
        public void HandlePagePostRender(object sender, RenderEventArgs args)
        {
            Assert.IsNotNull(args.Context);
            Invoked.Add("post-render");
        }




    }


    [TestClass()]
    public class ParserController_ImplementationTests
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

        [TestMethod()]
        [TestCategory("ParserControllerDefinition")]
        public void ControllerInstance_Outlets()
        {
            string documentxml = @"<?xml version='1.0' encoding='utf-8' ?>
                                <?scryber parser-mode='Strict' parser-log='false' append-log='false' log-level='Warnings' 
                                          controller='Scryber.Core.UnitTests.Generation.SimpleDocument_Controller, Scryber.UnitTests' ?>
                                <doc:Document xmlns:doc='Scryber.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                                              id='outerdoc' auto-bind='true'>
                                  <Pages>
                                    <doc:Page id='titlepage' >
                                      <Content>
                                        <doc:Label id='mylabel' />
                                      </Content>
                                    </doc:Page>

                                  </Pages>
                                </doc:Document>";

            Document parsed;
            using (System.IO.StringReader sr = new System.IO.StringReader(documentxml))
            {
                parsed = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            }

            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Controller);
            Assert.AreEqual(typeof(SimpleDocument_Controller), parsed.Controller.GetType());
            TestContext.WriteLine("Controller has been set correctly");

            SimpleDocument_Controller controller = (SimpleDocument_Controller)parsed.Controller;

            //check the Document field
            Assert.IsNotNull(controller.Document);
            Assert.AreEqual(controller.Document, parsed);
            Assert.AreEqual("outerdoc", controller.Document.ID);

            TestContext.WriteLine("outerdoc has been set to the document");

            //check the TitlePage property
            Assert.IsNotNull(controller.TitlePage);
            Assert.AreEqual(parsed.Pages[0], controller.TitlePage);
            Assert.AreEqual("titlepage", controller.TitlePage.ID);

            TestContext.WriteLine("titlepage has been set to teh page");

            //Check the label property
            Assert.IsNotNull(controller.MyLabel);
            Assert.AreEqual("mylabel", controller.MyLabel.ID);

            TestContext.WriteLine("mylabel has been set to the label");

            //Check the NotFound
            Assert.IsNull(controller.NotFound);

            TestContext.WriteLine("NotFound is still null");
        }

        [TestMethod()]
        [TestCategory("ParserControllerDefinition")]
        public void ControllerInstance_Actions()
        {
            string documentxml = @"<?xml version='1.0' encoding='utf-8' ?>
                                <?scryber parser-mode='Strict' parser-log='false' append-log='false' log-level='Warnings' 
                                          controller='Scryber.Core.UnitTests.Generation.SimpleDocument_Controller, Scryber.UnitTests' ?>
                                <doc:Document xmlns:doc='Scryber.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                                              id='outerdoc' auto-bind='true'>
                                  <Pages>

                                    <doc:Page id='titlepage' on-init='handlepageinit' on-loaded='handlepageload'
                                                             on-databinding='handlepagebinding' on-databound='handlepagebound'
                                                             on-prelayout='handleprelayout' on-postlayout='handlepostlayout'
                                                             on-prerender='handleprerender' on-postrender='handlepostrender' >
                                      <Content>
                                        <doc:Label id='mylabel' />
                                      </Content>
                                    </doc:Page>

                                  </Pages>
                                </doc:Document>";

            Document parsed;
            using (System.IO.StringReader sr = new System.IO.StringReader(documentxml))
            {
                parsed = Document.ParseDocument(sr, ParseSourceType.Other);
            }

            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Controller);
            Assert.AreEqual(typeof(SimpleDocument_Controller), parsed.Controller.GetType());
            TestContext.WriteLine("Controller has been set correctly");

            using (var ms = DocStreams.GetOutputStream("ControllerInstanceActions.pdf"))
            {
                parsed.SaveAsPDF(ms);
            }

            TestContext.WriteLine("Document has been processed");

            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Controller);

            SimpleDocument_Controller controller = (SimpleDocument_Controller)parsed.Controller;
            List<string> invoked = controller.Invoked;

            Assert.AreEqual(8, invoked.Count);
            TestContext.WriteLine("Controller has 8 items in the invoked list. 1 per event.");

            Assert.IsTrue(invoked.Contains("init"));
            Assert.IsTrue(invoked.Contains("load"));
            Assert.IsTrue(invoked.Contains("binding"));
            Assert.IsTrue(invoked.Contains("bound"));
            Assert.IsTrue(invoked.Contains("pre-layout"));
            Assert.IsTrue(invoked.Contains("post-layout"));
            Assert.IsTrue(invoked.Contains("pre-render"));
            Assert.IsTrue(invoked.Contains("post-render"));

            TestContext.WriteLine("All events have been processed and added to the invoked collection");


        }
    }
}
