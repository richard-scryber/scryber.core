using System;
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
public class InvalidStatements : SampleBase
{
    [TestMethod]
    public void InvalidOutside()
    {
        var path = GetTemplatePath("Invalid", "InvalidOutside.html");
        bool error = false;
        bool parsed =  false;
        Exception exception = null;
        try
        {
            using var doc = Document.ParseDocument(path);
            parsed = true;
            
            doc.Params["model"] = new { ShowSection = true, ShowText = false };
            
            using var ms = DocStreams.GetOutputStream("InvalidTemplates_InvalidOutside.pdf");
            doc.SaveAsPDF(ms);
            
            
        }
        catch (Exception ex)
        {
            error = true;
            exception = ex;
        }
        
        Assert.IsTrue(error, "No error found");
        Assert.IsNotNull(exception, "No exception found");
        Assert.IsFalse(parsed, "Document should not have been parsed");

    }
    
    [TestMethod]
    public void InvalidInside()
    {
        var path = GetTemplatePath("Invalid", "InvalidInside.html");
        bool error = false;
        bool parsed =  false;
        Exception exception = null;
        try
        {
            using var doc = Document.ParseDocument(path);

            //Any errors will be raised up in strict mode.
            doc.ConformanceMode = ParserConformanceMode.Strict;
            doc.Params["model"] = new { ShowSection = true, ShowText = false };
            
            using var ms = DocStreams.GetOutputStream("InvalidTemplates_InvalidInside.pdf");
            doc.SaveAsPDF(ms);
            
            
        }
        catch (Exception ex)
        {
            error = true;
            exception = ex;
        }
        
        Assert.IsTrue(error, "No error found");
        Assert.IsNotNull(exception, "No exception found");
        Assert.IsFalse(parsed, "Document should not have been parsed");

    }
    
    [TestMethod]
    public void InvalidInside_Lax_StillThrows()
    {
        var path = GetTemplatePath("Invalid", "InvalidInside.html");
        bool error = false;
        bool parsed =  true;
        Exception exception = null;
        try
        {
            using var doc = Document.ParseDocument(path);

            //An expression with invalid syntax inside a template (here an #if) is parsed when it is bound.
            //Invalid syntax is invalid completely, so it is raised in lax mode as well as strict.
            doc.ConformanceMode = ParserConformanceMode.Lax;
            doc.AppendTraceLog = true;
            doc.Params["model"] = new { ShowSection = true, ShowText = false };
            
            using var ms = DocStreams.GetOutputStream("InvalidTemplates_InvalidInside_Lax.pdf");
            doc.SaveAsPDF(ms);
            
            
        }
        catch (Exception ex)
        {
            error = true;
            exception = ex;
        }
        
        Assert.IsTrue(error, "No error found");
        Assert.IsNotNull(exception, "No exception found");
        Assert.IsTrue(parsed, "Document should have been parsed, as the error is only found when binding the template");

    }
    
    [TestMethod]
    public void InvalidInside_NoParent()
    {
        var path = GetTemplatePath("Invalid", "InvalidInside.html");
        bool error = false;
        bool parsed =  true;
        Exception exception = null;
        try
        {
            using var doc = Document.ParseDocument(path);

            //Any errors will be consumed and processing continues.
            //But the logs will appear at the end of the output document.
            doc.AppendTraceLog = true;
            
            
            using var ms = DocStreams.GetOutputStream("InvalidTemplates_InvalidInside_NoParent.pdf");
            doc.SaveAsPDF(ms);
            
            
        }
        catch (Exception ex)
        {
            error = true;
            exception = ex;
        }
        
        Assert.IsFalse(error, "Error found");
        Assert.IsNull(exception, "Exception found");
        Assert.IsTrue(parsed, "Document should have been parsed");

    }


    
}