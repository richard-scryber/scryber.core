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

    /// <summary>
    /// Target for a generated PDF's SubmitForm action (see PDFSubmitFormAction) - point a
    /// &lt;form action="..."&gt; at this URL (e.g. http://localhost:5188/Home/Submit) to see
    /// exactly what a reader actually sends when the form is filled in and submitted. The
    /// "ExportFormat" submit flag we set means readers should POST standard HTML form-encoded
    /// data (application/x-www-form-urlencoded), which Request.Form reads directly - but the raw
    /// body is also captured as a fallback in case a reader ignores that and sends FDF/XFDF/JSON
    /// instead, so nothing gets silently dropped either way.
    /// </summary>
    [AcceptVerbs("GET", "POST")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Submit()
    {
        var fields = new Dictionary<string, string>();
        string? rawBody = null;

        if (Request.HasFormContentType)
        {
            foreach (var kv in Request.Form)
                fields[kv.Key] = kv.Value.ToString();
        }
        else if (Request.Method == HttpMethods.Post)
        {
            using var reader = new StreamReader(Request.Body);
            rawBody = await reader.ReadToEndAsync();

            if (rawBody.TrimStart().StartsWith("%FDF"))
            {
                // Both Acrobat and Chrome's built-in PDFium viewer submit as FDF (Forms Data
                // Format) regardless of the SubmitForm action's ExportFormat flag - pull the
                // /T(name)/V(value) pairs out so they show up as real fields, not just raw text.
                // Value can be a parenthesised string (/V(Jane Doe)), a name (/V/Mr for a radio's
                // on-value), or absent entirely (an empty/unset field like a signature).
                var matches = System.Text.RegularExpressions.Regex.Matches(
                    rawBody,
                    @"<<\s*/T\(((?:[^()\\]|\\.)*)\)(?:\s*/V(?:\(((?:[^()\\]|\\.)*)\)|/([^\s>]+)))?\s*>>");
                foreach (System.Text.RegularExpressions.Match m in matches)
                {
                    var name = m.Groups[1].Value;
                    var value = m.Groups[2].Success ? m.Groups[2].Value
                        : m.Groups[3].Success ? m.Groups[3].Value
                        : string.Empty;
                    fields[name] = value;
                }
            }
        }

        foreach (var kv in Request.Query)
            fields["?" + kv.Key] = kv.Value.ToString();

        Console.WriteLine("=== Form submission received ===");
        Console.WriteLine($"Method: {Request.Method}  Content-Type: {Request.ContentType}");
        foreach (var kv in fields)
            Console.WriteLine($"  {kv.Key} = {kv.Value}");
        if (!string.IsNullOrEmpty(rawBody))
            Console.WriteLine($"  (raw body, not form-encoded): {rawBody}");
        Console.WriteLine("=================================");

        return Json(new
        {
            method = Request.Method,
            contentType = Request.ContentType,
            fields,
            rawBody
        });
    }
}