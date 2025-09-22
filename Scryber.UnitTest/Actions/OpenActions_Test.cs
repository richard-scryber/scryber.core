using System;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Actions;

[TestClass()]
public class OpenActions_Test
{
    public TestContext TextContext
        {
            get;
            set;
        }


        public OpenActions_Test()
        {
        }


        [TestMethod()]
        [TestCategory("Actions")]
        public void SetAction()
        {
            Assert.Inconclusive("Be nice to implement javascript actions within a document");
            
            // var src = "";
            //
            // using (var reader = new System.IO.StringReader(src))
            // {
            //     var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            //     doc.OpenActions.Add(new CheckSourceVersion(new Version("1.2"), "https://localhost:7055/"))
            //     
            //
            //     using (var stream = DocStreams.GetOutputStream("BindingController.pdf"))
            //     {
            //         doc.SaveAsPDF(stream);
            //     }
            //
            //     
            // }


            
        }
}