using System.Diagnostics;
using Scryber;
using Scryber.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Scryber.Options;
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

        var all = new List<FontRegistrationOption>(scryberConfig.FontOptions.Register ?? new FontRegistrationOption[] {});
        all.Add(fontRegistration);
        scryberConfig.FontOptions.Register = all.ToArray();
        
        var input = System.IO.Path.Combine(System.Environment.CurrentDirectory, "./Templates/HelloOpenSans.html");
        using var doc = Document.ParseDocument(input);
        
        doc.AddTraceLog(new TraceLogger(factory.CreateLogger<Document>(), TraceRecordLevel.Messages,  100,"Custom"));

        var output = System.IO.Path.Combine(System.Environment.CurrentDirectory, "output");
        if(!System.IO.Directory.Exists(output))
            System.IO.Directory.CreateDirectory(output);
        
        output = System.IO.Path.Combine(output, "./HelloOpenSans.pdf");

        using (var stream = new FileStream(output, FileMode.Create))
        {
            doc.SaveAsPDF(stream);
        }

        Console.WriteLine("Document created at path:");
        Console.WriteLine(output);
        Console.WriteLine();
        
        //Check the fonts are being included from the config file and the embedded resource.
        var resources = doc.SharedResources;
        Debug.Assert(resources.Count == 2);
        var one = resources[0] as PDFFontResource;
        var two = resources[1] as PDFFontResource;
        Debug.Assert(one != null);
        Debug.Assert(two != null);
        Debug.Assert(one.FontName == "Roboto Condensed Black,Bold Italic", "Font name should be Roboto Condensed Black,Bold Italic, not : " + one.FontName);
        Debug.Assert(two.FontName == "Open Sans Light", "Font name should be  Open Sans Light, not : " + two.FontName);
    }
}