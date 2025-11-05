using System;
using System.IO;
using System.Text.Json;
using Scryber.Components;

namespace WwfTigersReport
{
    /// <summary>
    /// Sample C# code demonstrating how to populate the WWF Tigers Report template
    /// This loads data from a JSON file and passes it to the Scryber template
    /// Based on the 2024 WWF Tigers Alive Annual Report
    /// </summary>
    public class TigersReportGenerator
    {
        public static void GenerateReport()
        {
            GenerateReport("wwf_tigers_report_data.json", "wwf_tigers_report_template.html", "WWF_Tigers_Report_2024.pdf");
        }

        public static void GenerateReport(string jsonDataPath, string templatePath, string outputPdfPath)
        {
            // Read and deserialize the JSON data file
            string jsonContent = File.ReadAllText(jsonDataPath);
            var model = JsonSerializer.Deserialize<object>(jsonContent);

            // Load the template
            using (var doc = Document.ParseDocument(templatePath))
            {
                // Pass the JSON data to the template - Scryber handles the binding automatically
                doc.Params["model"] = model;

                // Generate the PDF (SaveAsPDF handles all processing)
                using (var stream = new FileStream(outputPdfPath, FileMode.Create))
                {
                    doc.SaveAsPDF(stream);
                }

                Console.WriteLine($"Report generated successfully: {outputPdfPath}");
            }
        }
    }
}
