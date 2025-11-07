using System.Collections.Generic;
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
    
    
    [TestMethod]
    public void MonthlySalesReport()
    {

        var path = GetTemplatePath("MonthlySalesReport", "monthly-sales-report.html", true);

        var doc = Document.ParseDocument(path);
        var datapath = GetTemplatePath("MonthlySalesReport", "monthly-sales-sample.json");
        var dataContent = System.IO.File.ReadAllText(datapath);
        var data = System.Text.Json.JsonDocument.Parse(dataContent);
        using (var ms = DocStreams.GetOutputStream("MonthlySalesReport.pdf"))
        {
            doc.Params["model"] = data.RootElement;
            doc.SaveAsPDF(ms);
        }
    }
    
    
    [TestMethod]
    public void ExpressionsCribSheet()
    {
        var preamble = @"<div xmlns='http://www.w3.org/1999/xhtml' class='content' >
    <h2 class='sub-title' >
      <img src='/images/paperworklogoonly.png' style='width: 28pt; height: 25pt;' />
      <span>About</span></h2>
    <p contenteditable='true' style='margin-top: 20pt;'>A <span class='pwk-name' >paperwork</span> template is designed to be dynamic, separating the data from the structure and the style.<br/>
       Databinding is the mechanism used to include that data content into the document layout as part of the generation process.
    </p>
    <p>To enable this in <span class='pwk-name' >paperwork</span>, expressions are used to perform multiple calculations and evaluations on the current data 'model'.
    </p>
    <h4 class='group-title' style='margin-top: 20pt; width: calc(100% - 40pt);' title='Relevant Concepts'>Relevant Concepts</h4>
    <p>In order to get the best out of this document, you should be familiar with the following technologies:
    </p>
    <ul>
      <li>HTML (including XHTML and namespaces).</li>
      <li>Cascading styles sheets.</li>
      <li>JSON Object notation.</li>
    </ul>
</div>";
        
        var path = GetTemplatePath("ExpressionsCribSheet", "expressions.html", true);

        var doc = Document.ParseDocument(path);
        var datapath = GetTemplatePath("ExpressionsCribSheet", "expressions.json", true);
        var dataContent = System.IO.File.ReadAllText(datapath);
        var data = System.Text.Json.JsonDocument.Parse(dataContent);
        var _layouts = new Dictionary<string, string>();
        _layouts.Add("preamble", preamble);
        
        doc.Params["func"] = data.RootElement;
        doc.Params["_layouts"] = _layouts;
        
        
        
        datapath = GetTemplatePath("ExpressionsCribSheet", "model.json", true);
        dataContent = System.IO.File.ReadAllText(datapath);
        data = System.Text.Json.JsonDocument.Parse(dataContent);
        doc.Params["model"] = data.RootElement;
        
        using (var ms = DocStreams.GetOutputStream("ExpressionsCribSheet.pdf"))
        {
            doc.SaveAsPDF(ms);
        }
    }
    
    
    [TestMethod]
    public void Expressions_JustIndex_CribSheet()
    {
        
        var path = GetTemplatePath("ExpressionsCribSheet", "expressions_justindex.html", true);

        var doc = Document.ParseDocument(path);
        var datapath = GetTemplatePath("ExpressionsCribSheet", "expressions.json", true);
        var dataContent = System.IO.File.ReadAllText(datapath);
        var data = System.Text.Json.JsonDocument.Parse(dataContent);
        
        doc.Params["func"] = data.RootElement;
        
        
        
        datapath = GetTemplatePath("ExpressionsCribSheet", "model.json", true);
        dataContent = System.IO.File.ReadAllText(datapath);
        data = System.Text.Json.JsonDocument.Parse(dataContent);
        doc.Params["model"] = data.RootElement;
        
        using (var ms = DocStreams.GetOutputStream("Expressions_JustIndexCribSheet.pdf"))
        {
            doc.SaveAsPDF(ms);
        }
    }
}