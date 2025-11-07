using System;
using System.IO;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.UnitSamples
{
    public class TigersReport : SampleBase
    {

        #region public TestContext TestContext

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #endregion
        
        public void CreateTigersReport()
        {
            // Read and deserialize the JSON data file
            string path = GetTemplatePath("Tigers", "wwf_tigers_report_data.json");
            var jsonContent = File.ReadAllText(path);
            var model = JsonSerializer.Deserialize<object>(jsonContent);


            path = GetTemplatePath("Tigers", "wwf_tigers_report_template.html");

            using (var doc = Document.ParseDocument(path))
            {
                // Load the template
                doc.Params["model"] = model;

                // Generate the PDF (SaveAsPDF handles all processing)
                var outputPdfPath = "wwwf_tigers_report_template.pdf";
                using (var stream = GetOutputStream("Samples", "outputPdfPath.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }
    }

}
