using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Styles.Parsing;

using Scryber.PDF.Layout;
using Scryber.PDF;

using System.Diagnostics;
using System.IO;
using Scryber.Data;
using Scryber.Text;
using Scryber.PDF.Resources;



namespace Scryber.Core.UnitTests.Parsing;

[TestClass]
public class ParseTemplates
{
    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
        get
        {
            return testContextInstance;
        }
        set
        {
            testContextInstance = value;
        }
    }
    
    //Check the parsing of template contents
    //To make sure that when the inner content is read it captures all the content, outside, and inside the template.

    /// <summary>
    /// The template is just a string literal wrapped by string literals.
    /// </summary>
    [TestCategory("Parsing")]
    [TestMethod]
    public void ParseSimpleTemplateInBody()
    {
        var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
<body class='testing' >
   Before the template
   <if data-test='{{model.outputTest == true}}' class='testing' >Inside the template</if>
   After the template
</body>
</html>";

        using var content = new StringReader(src);
        var doc = Document.ParseDocument(content, ParseSourceType.DynamicContent);
        
        Assert.IsNotNull(doc);
        Assert.AreEqual(1, doc.Pages.Count);
        
        var bdy = doc.Pages[0] as HTMLBody;
        Assert.IsNotNull(bdy);
        Assert.AreEqual("testing", bdy.StyleClass);
        
        Assert.AreEqual(3, bdy.Contents.Count);
        
        var text = bdy.Contents[0] as TextLiteral;
        Assert.IsNotNull(text);
        Assert.AreEqual("Before the template", text.Text.Trim());

        var iif = bdy.Contents[1] as HTMLIf;
        Assert.IsNotNull(iif);
        var template = iif.Template as ParsableTemplateGenerator;
        Assert.IsNotNull(template);
        Assert.AreEqual("Inside the template", template.XmlContent);
        
        text = bdy.Contents[2] as TextLiteral;
        Assert.IsNotNull(text);
        Assert.AreEqual("After the template", text.Text.Trim());

    }
    
    /// <summary>
    /// The template is just a string literal wrapped by string literals.
    /// </summary>
    [TestCategory("Parsing")]
    [TestMethod]
    public void ParseSimpleTemplateNestedInBody()
    {
        var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
<body class='testing' >
   <div class='inner'>
       Before the template
       <if data-test='{{model.outputTest == true}}' class='testing' >Inside the template</if>
       After the template
   </div>
</body>
</html>";

        using var content = new StringReader(src);
        var doc = Document.ParseDocument(content, ParseSourceType.DynamicContent);
        
        Assert.IsNotNull(doc);
        Assert.AreEqual(1, doc.Pages.Count);
        
        var bdy = doc.Pages[0] as HTMLBody;
        Assert.IsNotNull(bdy);
        Assert.AreEqual("testing", bdy.StyleClass);
        
        Assert.AreEqual(3, bdy.Contents.Count);

        var wspc = bdy.Contents[0] as Whitespace;
        Assert.IsNotNull(wspc);

        var div = bdy.Contents[1] as HTMLDiv;
        Assert.IsNotNull(div);
        
        wspc = bdy.Contents[2] as Whitespace;
        Assert.IsNotNull(wspc);
        
        //Check the inner div contents
        
        Assert.AreEqual(3, div.Contents.Count);
        
        var text = div.Contents[0] as TextLiteral;
        Assert.IsNotNull(text);
        Assert.AreEqual("Before the template", text.Text.Trim());

        var iif = div.Contents[1] as HTMLIf;
        Assert.IsNotNull(iif);
        var template = iif.Template as ParsableTemplateGenerator;
        Assert.IsNotNull(template);
        Assert.AreEqual("Inside the template", template.XmlContent);
        
        text = div.Contents[2] as TextLiteral;
        Assert.IsNotNull(text);
        Assert.AreEqual("After the template", text.Text.Trim());

    }
    
    /// <summary>
    /// The template is just a string literal wrapped by string literals.
    /// </summary>
    [TestCategory("Parsing")]
    [TestMethod]
    public void ParseSimpleTemplateUneven1NestedInBody()
    {
        var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
<body class='testing' >
   <div class='inner'>
       Before the template
       <if data-test='{{model.outputTest == true}}' class='testing' >Inside the template</if>
    </div>
    After the template
</body>
</html>";

        using var content = new StringReader(src);
        var doc = Document.ParseDocument(content, ParseSourceType.DynamicContent);
        
        Assert.IsNotNull(doc);
        Assert.AreEqual(1, doc.Pages.Count);
        
        var bdy = doc.Pages[0] as HTMLBody;
        Assert.IsNotNull(bdy);
        Assert.AreEqual("testing", bdy.StyleClass);
        
        Assert.AreEqual(3, bdy.Contents.Count);

        var wspc = bdy.Contents[0] as Whitespace;
        Assert.IsNotNull(wspc);

        var div = bdy.Contents[1] as HTMLDiv;
        Assert.IsNotNull(div);
        Assert.AreEqual("inner", div.StyleClass);
        
        //literal outside the div
        var text  = bdy.Contents[2] as TextLiteral;
        Assert.IsNotNull(text);
        Assert.AreEqual("After the template", text.Text.Trim());
        
        //Check the inner div contents
        
        Assert.AreEqual(3, div.Contents.Count);
        
        text = div.Contents[0] as TextLiteral;
        Assert.IsNotNull(text);
        Assert.AreEqual("Before the template", text.Text.Trim());

        var iif = div.Contents[1] as HTMLIf;
        Assert.IsNotNull(iif);
        var template = iif.Template as ParsableTemplateGenerator;
        Assert.IsNotNull(template);
        Assert.AreEqual("Inside the template", template.XmlContent);
        
        wspc = div.Contents[2] as Whitespace;
        Assert.IsNotNull(wspc);
        

    }
    
    /// <summary>
    /// The template is just a string literal wrapped by string literals.
    /// </summary>
    [TestCategory("Parsing")]
    [TestMethod]
    public void ParseSimpleTemplateUneven2NestedInBody()
    {
        var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
<body class='testing' >
    Before the template
    <div class='inner'>
       <if data-test='{{model.outputTest == true}}' class='testing' >Inside the template</if>
       After the template
    </div>
</body>
</html>";

        using var content = new StringReader(src);
        var doc = Document.ParseDocument(content, ParseSourceType.DynamicContent);
        
        Assert.IsNotNull(doc);
        Assert.AreEqual(1, doc.Pages.Count);
        
        var bdy = doc.Pages[0] as HTMLBody;
        Assert.IsNotNull(bdy);
        Assert.AreEqual("testing", bdy.StyleClass);
        
        Assert.AreEqual(3, bdy.Contents.Count);

        var text = bdy.Contents[0] as TextLiteral;
        Assert.IsNotNull(text);
        Assert.AreEqual("Before the template", text.Text.Trim());

        var div = bdy.Contents[1] as HTMLDiv;
        Assert.IsNotNull(div);
        Assert.AreEqual("inner", div.StyleClass);
        
        //literal outside the div
        var wspc  = bdy.Contents[2] as Whitespace;
        Assert.IsNotNull(wspc);
       
        
        //Check the inner div contents
        
        Assert.AreEqual(3, div.Contents.Count);

        wspc = div.Contents[0] as Whitespace;
        Assert.IsNotNull(wspc);
        
        var iif = div.Contents[1] as HTMLIf;
        Assert.IsNotNull(iif);
        var template = iif.Template as ParsableTemplateGenerator;
        Assert.IsNotNull(template);
        Assert.AreEqual("Inside the template", template.XmlContent);
       
                
        text = div.Contents[2] as TextLiteral;
        Assert.IsNotNull(text);
        Assert.AreEqual("After the template", text.Text.Trim());
        
    }
}