using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.Core.UnitTests.Generation
{
    //[TestClass()]
    public class ProcessDocument_Tests
    {

        //[TestMethod()]
        //[TestCategory("Hello World")]
        public void ProcessHelloWorld()
        {
            using (Document doc = Document.ParseDocument("./HelloWorld.pdfx"))
            {
                doc.ProcessDocument("./HelloWorld.pdf", System.IO.FileMode.Create);
            }
        }

        public void ProcessHellowWorldCode()
        {
            using (Document doc = this.GenerateHelloWorld())
            {

            }
        }

        public Document GenerateHelloWorld()
        {
            Document doc = new Document();

            Page pg = new Page();
            doc.Pages.Add(pg);
            pg.Style.Margins.All = 10;

            Label lbl = new Label();
            lbl.Text = "Hello World";
            pg.Contents.Add(lbl);

            return doc;
        }
    }
}
