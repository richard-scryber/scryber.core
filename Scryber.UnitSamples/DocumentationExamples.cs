using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.Text;
using Scryber.Components;
using Scryber.Core.UnitTests;

namespace Scryber.UnitSamples;

using Scryber.Html.Components;
using Scryber.Styles;

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
    public void QuickStartHello2()
    {
        var path = GetTemplatePath("Documentation", "hello2.html");
        using var doc = Document.ParseDocument(path);
        
        //set the user to an instance (or JSON) value
        doc.Params["user"] = new { firstName = "John", lastName = "Smith", emailAddress = "j.smith@company.com",
                                   skills = new[] {"C Sharp", "javascript", "scrum master", "Team leader"},
                                   profilePicture = "" };

        //add some custom style information.
        doc.Params["brand"] = new { mainColor = "rgb(0,168,161)", lightColor = "silver"};
        
        using var ms = DocStreams.GetOutputStream("HelloWorld2.pdf");
        doc.SaveAsPDF(ms);

    }
    
    [TestMethod]
    public void QuickStartHelloComplex()
    {
        var path = GetTemplatePath("Documentation", "helloComplex.html");
        using var doc = Document.ParseDocument(path);

        
        doc.Params["user"] = new
        {
            firstName = "John", lastName = "Smith",
            sightImpared = false,
            reportsTo = new { firstName = "Sally", lastName = "Smith" },
            staff = new [] {
                new{ firstName = "Bill", lastName = "Jones", stillFriends = true,
                    emailAddress = "bill.jones200@nonexistant.com" },
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
            colorLight = "rgb(113, 129, 128)",
            logoUrl = "https://www.paperworkday.info/assets/PaperworkLogoOnly.png",
            strapLine = "<h2>paperwork</h2>" +
                        "<span>intelligent document creation and automation!</span>"
        };

        doc.AppendTraceLog = true;
        
        using var ms = DocStreams.GetOutputStream("HelloWorldComplex.pdf");
        doc.SaveAsPDF(ms);



    }

    [TestMethod]
    public void Configuration_WorkingInCode_1()
    {

        var doc = new Document();
        var page = new Page();
        var section = new Section();

        var title = new Label();
        title.Text = "Runtime Report";
        title.Style.Font.FontSize = 24;
        title.Style.Font.FontBold = true;
        title.Style.Margins.Bottom = 10;

        var body = new Div();
        body.Style.Padding.All = 12;
        body.Style.Border.Width = 1;
        body.Style.Border.Color = StandardColors.Gray;
        body.Contents.Add(new TextLiteral("Generated entirely in code."));

        section.Contents.Add(title);
        section.Contents.Add(body);
        page.Contents.Add(section);
        doc.Pages.Add(page);

        using var ms = DocStreams.GetOutputStream("Configuration_WorkingInCode_1.pdf");
        doc.SaveAsPDF(ms);

    }
    
}