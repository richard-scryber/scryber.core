using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.Text;
using Scryber.Components;
using Scryber.Core.UnitTests;

namespace Scryber.UnitSamples;

using Scryber.Html.Components;

[TestClass]
public class DocumentationExamples : SampleBase
{
    [TestMethod]
    public void QuickStartHello()
    {
        var path = GetTemplatePath("Documentation", "hello.html");
        using var doc = Document.ParseDocument(path);
        
        doc.Params["user"] = new
        {
            firstName = "John", lastName = "Smith", sightImpared = true
            
        };
        
        
        using var ms = DocStreams.GetOutputStream("HelloWorld.pdf");
        doc.SaveAsPDF(ms);



    }
    
    [TestMethod]
    public void QuickStartHelloComplex()
    {
        var path = GetTemplatePath("Documentation", "helloComplex.html");
        using var doc = Document.ParseDocument(path);
        
        doc.Params["user"] = new
        {
            firstName = "John", lastName = "Smith", sightImpared = true,
            friends = new [] {
                new
                {
                    firstName = "Bill", lastName = "Jones", stillFriends = true,
                    emailAddress = "bill.jones200@nonexistant.com"
                },
                new { firstName = "Sarah", lastName = "Jones", stillFriends = true,
                    emailAddress = "sarah.jones@nonexistant.com" },
                new { firstName = "James", lastName = "Long", stillFriends = true,
                    emailAddress = "james.longerthananyone@nonexistant.com" },
                new { firstName = "Sam", lastName = "Elsewhere", stillFriends = false,
                    emailAddress = "sam@notyou.co.uk" }
            }
            
        };
        
        doc.Params["brand"] = new
        {
            isBranded = true,
            color = "rgb(0, 168, 161)",
            logoUrl = "https://www.paperworkday.info/assets/PaperworkLogoOnly.png",
            strapLine = "<h2>paperwork</h2>" +
                        "<span>smart document creation and automation!</span>"
        };

        doc.Params["--brand-color"] = "rgb(0, 161, 116)";

        doc.AppendTraceLog = true;
        
        using var ms = DocStreams.GetOutputStream("HelloWorldComplex.pdf");
        doc.SaveAsPDF(ms);



    }
    
}