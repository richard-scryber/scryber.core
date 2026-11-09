using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Scryber.Components;
using Scryber.UnitWebTest.Models;
using Scryber.Components.Mvc;
using Scryber.PDF.Resources;

namespace Scryber.UnitWebTest.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    
    public IActionResult Report()
    {
        var path = System.Environment.CurrentDirectory;
        path = System.IO.Path.Combine(path, "./Templates/HelloOpenSans.html");
        var doc = Document.ParseDocument(path);
        doc.LayoutComplete += (sender, args) =>
        {
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
        };
        return this.PDF(doc, true, "HelloOpenSans.pdf");
        
    }
}