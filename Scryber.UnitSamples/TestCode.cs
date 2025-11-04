using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.Text;
using Scryber.Components;
using Scryber.Core.UnitTests;

namespace Scryber.UnitSamples;

using Scryber.Html.Components;

[TestClass]
public class TestCode : SampleBase
{
    [TestMethod]
    public void JustATest()
    {
        

        //documents can either contain a body **or** a frameset.
        //Not both.
        var doc = new HTMLDocument();
        doc.Body = new HTMLBody();
        doc.Body.Contents.Add("All the content goes here.");


    }

    [TestMethod]
    public void ProjectStatusReport()
    {

        var path = GetTemplatePath("ProjectStatusReport", "project-status-report.html", true);

        var doc = Document.ParseDocument(path);
        var datapath = GetTemplatePath("ProjectStatusReport", "project-status-sample.json");
        var dataContent = System.IO.File.ReadAllText(datapath);
        var data = System.Text.Json.JsonDocument.Parse(dataContent);
        using (var ms = DocStreams.GetOutputStream("ProjectStatusReport.pdf"))
        {
            doc.Params["model"] = data.RootElement;
            doc.SaveAsPDF(ms);
        }
    }
}