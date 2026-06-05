using System.Diagnostics;
using Scryber;
using Scryber.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Scryber.Options;
using Scryber.PDF.Layout;
using Scryber.PDF.Resources;

namespace Scryber.UnitConsoleTest;

class Program
{
    static void Main(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        using ILoggerFactory factory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });
        
        Scryber.ServiceProvider.Init(config);
        
        var scryberConfig = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();

        var fontRegistration = new FontRegistrationOption()
        {
            Family = "OpenSans-light",
            Style = "Regular",
            Weight = 300,
            //File = "./Fonts/OpenSans-light.ttf",
            Resource = "Scryber.UnitConsoleTest.Fonts.OpenSans-Light.ttf, Scryber.UnitConsoleTest"
        };

        var all = new List<FontRegistrationOption>(scryberConfig.FontOptions.Register ?? new List<FontRegistrationOption>());
        all.Add(fontRegistration);
        scryberConfig.FontOptions.Register = all;
        
        var input = System.IO.Path.Combine(System.Environment.CurrentDirectory, "./Templates/HelloOpenSans.html");
        using var doc = Document.ParseDocument(input);
        
        doc.AddTraceLog(new TraceLogger(factory.CreateLogger<Document>(), TraceRecordLevel.Messages,  100,"Custom"));
        
        doc.Params["model"] = new
        {
            Name = "Hello World",
        };
        
        var output = System.IO.Path.Combine(System.Environment.CurrentDirectory, "output");
        if(!System.IO.Directory.Exists(output))
            System.IO.Directory.CreateDirectory(output);
        
        output = System.IO.Path.Combine(output, "./HelloOpenSans.pdf");

        PDFLayoutDocument? layout = null;
        
        using (var stream = new FileStream(output, FileMode.Create))
        {
            doc.LayoutComplete += (sender, eventArgs) =>
            {
                layout = eventArgs.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
            };
            doc.AppendTraceLog = true;
            doc.SaveAsPDF(stream);
        }

        Console.WriteLine();
        Console.WriteLine("Document created at path:");
        Console.WriteLine(output);
        Console.WriteLine();

        if(null == layout)
            throw new Exception("Layout not found");
        
        CheckDocumentFonts(doc, layout);
        CheckDocumentFunctions(doc, layout);
    }

    private static void CheckDocumentFonts(Document doc, PDFLayoutDocument layout)
    {
        try
        {
            //Check the fonts are being included from the config file and the embedded resource.
            var resources = doc.SharedResources;
            Debug.Assert(resources.Count == 2);
            var one = resources[0] as PDFFontResource;
            var two = resources[1] as PDFFontResource;
            Debug.Assert(one != null);
            Debug.Assert(two != null);
            Debug.Assert(one.FontName == "Roboto Condensed Black,Bold Italic",
                "Font name should be Roboto Condensed Black,Bold Italic, not : " + one.FontName);
            Debug.Assert(two.FontName == "Open Sans Light",
                "Font name should be  Open Sans Light, not : " + two.FontName);
            
            Console.WriteLine("Font check completed");
        }
        catch (Exception e)
        {
            Console.WriteLine("Font check failed: " + e.Message);
            Console.WriteLine(e);
            throw;
        }
    }

    private static void CheckDocumentFunctions(Document doc, PDFLayoutDocument layout)
    {
        try
        {
            //Checks that the 'custom' function has been called and has returned the correct value.
            var p = layout.AllPages[0].ContentBlock.Columns[0].Contents[3] as PDFLayoutBlock;
            Debug.Assert(p != null);
            var line = p.Columns[0].Contents[0] as PDFLayoutLine;
            Debug.Assert(line != null);
            Debug.Assert(line.Runs.Count == 6, "There should be 8 runs on the line not : " + line.Runs.Count);
            var chars = line.Runs[1] as PDFTextRunCharacter;
            Debug.Assert(chars != null);
            Debug.Assert(chars.Characters == "Calling a custom function: ");
            chars = line.Runs[4] as PDFTextRunCharacter;
            Debug.Assert(chars != null);
            Debug.Assert(chars.Characters == "Value is Hello World");
            
            Console.WriteLine("Function check completed");
        }
        catch (Exception e)
        {
            Console.WriteLine("Function check failed: " + e.Message);
            Console.WriteLine(e);
            throw;
        }
    }
}