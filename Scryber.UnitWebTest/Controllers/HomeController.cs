using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Scryber.UnitWebTest.Models;
using Scryber.Components.Mvc;

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
        return this.PDF(path, true, "HelloOpenSans.pdf");
        
    }
}