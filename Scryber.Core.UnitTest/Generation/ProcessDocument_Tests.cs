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
            using (PDFDocument doc = PDFDocument.ParseDocument("./HelloWorld.pdfx"))
            {
                doc.ProcessDocument("./HelloWorld.pdf", System.IO.FileMode.Create);
            }
        }

        public void ProcessHellowWorldCode()
        {
            using (PDFDocument doc = this.GenerateHelloWorld())
            {

            }
        }

        public PDFDocument GenerateHelloWorld()
        {
            PDFDocument doc = new PDFDocument();

            PDFPage pg = new PDFPage();
            doc.Pages.Add(pg);
            pg.Style.Margins.All = 10;

            PDFLabel lbl = new PDFLabel();
            lbl.Text = "Hello World";
            pg.Contents.Add(lbl);

            return doc;
        }
    }
}
