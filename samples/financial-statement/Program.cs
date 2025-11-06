using System;
using System.IO;
using System.Text.Json;
using Scryber.Components;

namespace Scryber.Samples.FinancialStatement
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("==============================================");
                Console.WriteLine("  Scryber Financial Statement Generator");
                Console.WriteLine("==============================================");
                Console.WriteLine();

                // Parse arguments
                string dataFile = args.Length > 0 ? args[0] : "data/financial-data.json";
                string outputFile = args.Length > 1 ? args[1] : "output/financial-statement.pdf";
                string templateFile = "templates/financial-statement.html";

                // Create output directory if needed
                string? outputDir = System.IO.Path.GetDirectoryName(outputFile);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Validate input files exist
                if (!File.Exists(dataFile))
                {
                    Console.WriteLine($"ERROR: Data file not found: {dataFile}");
                    Console.WriteLine();
                    return;
                }

                if (!File.Exists(templateFile))
                {
                    Console.WriteLine($"ERROR: Template file not found: {templateFile}");
                    Console.WriteLine();
                    return;
                }

                // Load JSON data
                Console.WriteLine($"Loading data from: {dataFile}");
                string jsonContent = File.ReadAllText(dataFile);
                var model = JsonSerializer.Deserialize<object>(jsonContent);
                Console.WriteLine("✓ Data loaded successfully");
                Console.WriteLine();

                // Parse template and generate PDF
                Console.WriteLine($"Parsing template: {templateFile}");
                using (var doc = Document.ParseDocument(templateFile))
                {
                    Console.WriteLine("✓ Template parsed successfully");
                    Console.WriteLine();

                    Console.WriteLine("Binding data to template...");
                    doc.Params["model"] = model;
                    Console.WriteLine("✓ Data bound successfully");
                    Console.WriteLine();

                    Console.WriteLine($"Generating PDF: {outputFile}");
                    using (var stream = new FileStream(outputFile, FileMode.Create))
                    {
                        doc.SaveAsPDF(stream);
                    }
                    Console.WriteLine("✓ PDF generated successfully");
                    Console.WriteLine();
                }

                // Show success message with file info
                var fileInfo = new FileInfo(outputFile);
                Console.WriteLine("==============================================");
                Console.WriteLine("  Financial Statement Generated Successfully!");
                Console.WriteLine("==============================================");
                Console.WriteLine($"Output file: {System.IO.Path.GetFullPath(outputFile)}");
                Console.WriteLine($"File size: {fileInfo.Length / 1024.0:F2} KB");
                Console.WriteLine();
                Console.WriteLine("Open the PDF to view your financial statement.");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("==============================================");
                Console.WriteLine("  ERROR");
                Console.WriteLine("==============================================");
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine("Stack trace:");
                Console.WriteLine(ex.ToString());
                Console.WriteLine();
                Environment.ExitCode = 1;
            }
        }
    }
}
