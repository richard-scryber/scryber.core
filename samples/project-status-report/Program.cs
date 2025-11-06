using System;
using System.IO;
using System.Text.Json;
using Scryber.Components;

namespace Scryber.Samples.ProjectStatusReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("==============================================");
                Console.WriteLine("  Scryber Project Status Report Generator");
                Console.WriteLine("==============================================");
                Console.WriteLine();

                // Determine paths
                string dataFile = args.Length > 0 ? args[0] : "data/project-status-sample.json";
                string outputFile = args.Length > 1 ? args[1] : "output/project-status-report.pdf";
                string templateFile = "templates/project-status-report.html";

                // Ensure output directory exists
                string? outputDir = System.IO.Path.GetDirectoryName(outputFile);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Validate files exist
                if (!File.Exists(dataFile))
                {
                    Console.WriteLine($"ERROR: Data file not found: {dataFile}");
                    Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");
                    return;
                }

                if (!File.Exists(templateFile))
                {
                    Console.WriteLine($"ERROR: Template file not found: {templateFile}");
                    Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");
                    return;
                }

                // Load JSON data
                Console.WriteLine($"Loading data from: {dataFile}");
                string jsonContent = File.ReadAllText(dataFile);
                var model = JsonSerializer.Deserialize<object>(jsonContent);

                if (model == null)
                {
                    Console.WriteLine("ERROR: Failed to deserialize JSON data");
                    return;
                }

                Console.WriteLine("✓ Data loaded successfully");
                Console.WriteLine();

                // Parse HTML template
                Console.WriteLine($"Parsing template: {templateFile}");
                using (var doc = Document.ParseDocument(templateFile))
                {
                    Console.WriteLine("✓ Template parsed successfully");
                    Console.WriteLine();

                    // Bind data to template
                    Console.WriteLine("Binding data to template...");
                    doc.Params["model"] = model;
                    Console.WriteLine("✓ Data bound successfully");
                    Console.WriteLine();

                    // Generate PDF
                    Console.WriteLine($"Generating PDF: {outputFile}");
                    using (var stream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                    {
                        doc.SaveAsPDF(stream);
                    }

                    Console.WriteLine("✓ PDF generated successfully");
                    Console.WriteLine();
                }

                // Get file info
                var fileInfo = new FileInfo(outputFile);
                Console.WriteLine("==============================================");
                Console.WriteLine("  Report Generated Successfully!");
                Console.WriteLine("==============================================");
                Console.WriteLine($"Output file: {System.IO.Path.GetFullPath(outputFile)}");
                Console.WriteLine($"File size: {FormatFileSize(fileInfo.Length)}");
                Console.WriteLine();
                Console.WriteLine("Open the PDF to view your project status report.");
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
                Console.WriteLine(ex.StackTrace);
                Environment.Exit(1);
            }
        }

        static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}
