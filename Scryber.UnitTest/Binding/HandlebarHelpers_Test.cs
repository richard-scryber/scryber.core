using System;
using System.IO;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Data;
using Scryber.Styles;
using System.Linq;
using System.Linq.Expressions;
using Scryber.Drawing;
using Scryber.Handlebar.Components;
using Scryber.Html.Components;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.PDF.Resources;

namespace Scryber.Core.UnitTests.Binding
{
    /// <summary>
    /// Testing the inclusion of {#each } {#if } etc. within the content file.
    /// </summary>
    [TestClass()]
    public class HandlebarHelpers_Test
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
        
        const string ns = "Scryber.Handlebar.Components, Scryber.Components";
        const string prefix = "hbar";

        [TestMethod]
        public void CheckHelperMatching1_regex()
        {

            var str = @"{{#using people}}
{{name}}
{{#each}}";

            var expected =
                @"<hbar:using xmlns:hbar='Scryber.Handlebar.Components, Scryber.Components' data-bind='{{people}}' >
{{name}}
<hbar:each xmlns:hbar='Scryber.Handlebar.Components, Scryber.Components' data-bind='{{.}}' >";


            var splitter = new Scryber.Generation.HBarHelperSplitter(ns, prefix);

            var result = splitter.ReplaceAll(str);

            Assert.AreEqual(expected, result);

        }
        
        [TestMethod()]
        public void CheckHelperMatching2_each_this()
        {

        
        var str = @"{{#each collection}}
<!-- Content repeated for each item -->
{{this.property}}
{{/each}}";
            
            
            var expected =
                @"<hbar:each xmlns:hbar='Scryber.Handlebar.Components, Scryber.Components' data-bind=""{{collection}}"" >
<!-- Content repeated for each item -->
{{this.property}}
</hbar:each>";

            var splitter = new Scryber.Generation.HBarHelperSplitter(ns, prefix);

            var result = splitter.ReplaceAll(str);

            Assert.AreEqual(expected, result);

            // Check that it parses as a fragment
            using (var sr = new StringReader(str))
            {
                var comp = Document.Parse(sr, ParseSourceType.DynamicContent);
                
                Assert.IsInstanceOfType(comp, typeof(EachHelper));
                var each = (EachHelper)comp;
                
                Assert.IsInstanceOfType(each.Template, typeof(ParsableTemplateGenerator));
                var template = (ParsableTemplateGenerator)each.Template;

                expected = @"
<!-- Content repeated for each item -->
{{this.property}}
";
                Assert.AreEqual(expected, template.XmlContent);
                
            }

        }
        
        
        [TestMethod]
        public void CheckHelperMatching3_each()
        {
            var str = @"{{#each items}}
                        <li>{{this}}</li>
                    {{/each}}";
            var expected =
                @"<hbar:each xmlns:hbar='Scryber.Handlebar.Components, Scryber.Components' data-bind=""{{items}}"" >
                        <li>{{this}}</li>
                    </hbar:each>";

            var splitter = new Scryber.Generation.HBarHelperSplitter(ns, prefix);

            var result = splitter.ReplaceAll(str);

            Assert.AreEqual(expected, result);
            
            // Check that it parses as a fragment
            using (var sr = new StringReader(result))
            {
                var comp = Document.Parse(sr, ParseSourceType.DynamicContent);
                
                Assert.IsInstanceOfType(comp, typeof(EachHelper));
                var each = (EachHelper)comp;
                
                //check the template content
                Assert.IsInstanceOfType(each.Template, typeof(ParsableTemplateGenerator));
                var template = (ParsableTemplateGenerator)each.Template;

                expected = @"
                        <li>{{this}}</li>
                    ";
                Assert.AreEqual(expected, template.XmlContent);
                
            }

        }
        
        [TestMethod]
        public void CheckHelperMatching4_each()
        {
            var str = @"
{{#each products}}
<tr>
            <td>{{add(@index, 1)}}</td>  <!-- Convert to 1-based -->
            <td>{{this.name}}</td>
            <td>${{this.price}}</td>
</tr>
{{/each}}
";

            var expected = @"
<hbar:each xmlns:hbar='Scryber.Handlebar.Components, Scryber.Components' data-bind=""{{products}}"" >
<tr>
            <td>{{add(@index, 1)}}</td>  <!-- Convert to 1-based -->
            <td>{{this.name}}</td>
            <td>${{this.price}}</td>
</tr>
</hbar:each>
";

            var splitter = new Scryber.Generation.HBarHelperSplitter(ns, prefix);

            var result = splitter.ReplaceAll(str);

            Assert.AreEqual(expected, result);
            
            // Check that it parses as a fragment
            using (var sr = new StringReader(result))
            {
                var comp = Document.Parse(sr, ParseSourceType.DynamicContent);
                
                Assert.IsInstanceOfType(comp, typeof(EachHelper));
                var each = (EachHelper)comp;
                
                //check the template content
                Assert.IsInstanceOfType(each.Template, typeof(ParsableTemplateGenerator));
                var template = (ParsableTemplateGenerator)each.Template;

                expected = @"
<tr>
            <td>{{add(@index, 1)}}</td>  <!-- Convert to 1-based -->
            <td>{{this.name}}</td>
            <td>${{this.price}}</td>
</tr>
";
                Assert.AreEqual(expected, template.XmlContent);
                
            }


        }
        
        [TestMethod]
        public void CheckHelperMatching5_If()
        {
            var str = @"		  
{{#if model.priority == 0}}
    <span class=""badge low"">Low Priority</span>
{{else if model.priority == 1}}
    <span class=""badge medium"">Medium Priority</span>
{{else if model.priority == 2}}
    <span class=""badge high"">High Priority</span>
{{else}}
    <span class=""badge critical"">Critical</span>
{{/if}}
";
            var expected =
                @"		  
<hbar:choose xmlns:hbar='Scryber.Handlebar.Components, Scryber.Components' >
	<hbar:when data-test=""{{model.priority == 0}}"" >
    <span class=""badge low"">Low Priority</span>
</hbar:when>
	<hbar:when data-test=""{{model.priority == 1}}"" >
    <span class=""badge medium"">Medium Priority</span>
</hbar:when>
	<hbar:when data-test=""{{model.priority == 2}}"" >
    <span class=""badge high"">High Priority</span>
</hbar:when>
	<hbar:otherwise>
    <span class=""badge critical"">Critical</span>
</hbar:otherwise>
</hbar:choose>
";

            var splitter = new Scryber.Generation.HBarHelperSplitter(ns, prefix);

            var result = splitter.ReplaceAll(str);

            Assert.AreEqual(expected, result);
            
            
            // Check that it parses as a fragment
            using (var sr = new StringReader(result))
            {
                var comp = Document.Parse(sr, ParseSourceType.DynamicContent);
                
                Assert.IsInstanceOfType(comp, typeof(ChooseHelper));
                var choose = (ChooseHelper)comp;
                
                Assert.AreEqual(3, choose.Whens.Count);
                Assert.IsNotNull(choose.Otherwise);

                var when = choose.Whens[0];
                Assert.IsNotNull(when);
                Assert.IsNotNull(when.Template);
                //check the template content
                Assert.IsInstanceOfType(when.Template, typeof(ParsableTemplateGenerator));
                var template = (ParsableTemplateGenerator)when.Template;

                expected = @"
    <span class=""badge low"">Low Priority</span>
";
                Assert.AreEqual(expected, template.XmlContent);
                
                when = choose.Whens[1];
                Assert.IsNotNull(when);
                Assert.IsNotNull(when.Template);
                //check the template content
                Assert.IsInstanceOfType(when.Template, typeof(ParsableTemplateGenerator));
                template = (ParsableTemplateGenerator)when.Template;

                expected = @"
    <span class=""badge medium"">Medium Priority</span>
";
                Assert.AreEqual(expected, template.XmlContent);
                
                when = choose.Whens[2];
                Assert.IsNotNull(when);
                Assert.IsNotNull(when.Template);
                //check the template content
                Assert.IsInstanceOfType(when.Template, typeof(ParsableTemplateGenerator));
                template = (ParsableTemplateGenerator)when.Template;

                expected = @"
    <span class=""badge high"">High Priority</span>
";
                Assert.AreEqual(expected, template.XmlContent);

                var other = choose.Otherwise;
                Assert.IsNotNull(other);
                Assert.IsNotNull(other.Template);
                //check the template content
                Assert.IsInstanceOfType(other.Template, typeof(ParsableTemplateGenerator));
                template = (ParsableTemplateGenerator)other.Template;

                expected = @"
    <span class=""badge critical"">Critical</span>
";
                Assert.AreEqual(expected, template.XmlContent);

            }

        }

        [TestMethod]
        public void CheckHelperMatching6_If_Render()
        {
            var str = @"
<div xmlns='http://www.w3.org/1999/xhtml' >
{{#if model.status != 'cancelled'}}
    <p id='is_valid'>This order is valid.</p>
{{/if}}
<!-- Greater than -->
{{#if model.age > 18}}
    <p id='is_adult'>Adult</p>
{{/if}}

<!-- Greater than or equal -->
{{#if model.score >= 70}}
    <p id='did_pass' class=""pass"">Passed</p>
{{/if}}

<!-- Less than -->
{{#if model.temperature < 32}}
    <p id='is_freezing'>Freezing conditions</p>
{{/if}}

<!-- Less than or equal -->
{{#if model.stock <= 10}}
    <p id='stock_is_low' class=""warning"">Low stock!</p>
{{/if}}

{{#if model.age >= 18 && model.hasLicense}}
    <p id='can_drive'>Eligible to drive</p>
{{else}}
    <p id='cant_drive'>Not eligible to drive</p>
{{/if}}

{{#if model.isAdmin || model.isModerator}}
    <div id='has_admin' class=""admin-panel"">
        <h2>Administration</h2>
        <!-- Admin controls -->
    </div>
{{/if}}

{{#if model.layoutType == 'summary'}}
    <div id='summary' class=""summary-layout"">
        <h1>{{model.title}}</h1>
        <p>{{model.summary}}</p>
    </div>
{{else if model.layoutType == 'detailed'}}
    <div id='detailed' class=""detailed-layout"">
        <h1>{{model.title}}</h1>
        <p>{{model.summary}}</p>
        <div class=""details"">
            <p>{{model.fullContent}}</p>
        </div>
    </div>
{{else}}
    <div id='compact' class=""compact-layout"">
        <h2 >{{model.title}}</h2>
    </div>
{{/if}}
<p>After the fact</p>
</div>
";


            // Check that it parses as a fragment
            using (var sr = new StringReader(str))
            {
                var comp = Document.Parse(sr, ParseSourceType.DynamicContent);
                var div = comp as HTMLDiv;
                Assert.IsNotNull(div);

                HTMLDocument doc = new HTMLDocument();
                doc.Body = new HTMLBody();
                doc.Body.Contents.Add(div);

                doc.Params["model"] = new
                {
                    status = "delivered",
                    score = 90,
                    temperature = 40,
                    stock = 4,
                    age = 16,
                    hasLicense = true,
                    isModerator = true,
                    layoutType = "compact"
                };

                doc.InitializeAndLoad(OutputFormat.PDF);
                doc.DataBind(OutputFormat.PDF);

                using (var stream = DocStreams.GetOutputStream("Handlebars_CheckHelperMatching6.pdf"))
                {
                    doc.RenderToPDF(stream);
                    
                    var created = doc.FindAComponentById("is_valid");
                    Assert.IsNotNull(created); //exists

                    created = doc.FindAComponentById("is_adult");
                    Assert.IsNull(created); //does not exist

                    created = doc.FindAComponentById("did_pass");
                    Assert.IsNotNull(created); //exists

                    created = doc.FindAComponentById("is_freezing");
                    Assert.IsNull(created); //does not exist
                    
                    created = doc.FindAComponentById("stock_is_low");
                    Assert.IsNotNull(created); //exists
                    
                    created = doc.FindAComponentById("can_drive");
                    Assert.IsNull(created); //does not exist
                    
                    created = doc.FindAComponentById("cant_drive");
                    Assert.IsNotNull(created); //exists
                    
                    created = doc.FindAComponentById("has_admin");
                    Assert.IsNotNull(created); //exists
                    
                    created = doc.FindAComponentById("detailed");
                    Assert.IsNull(created); //does not exist
                    
                    created = doc.FindAComponentById("summary");
                    Assert.IsNull(created); //does not exist
                    
                    created = doc.FindAComponentById("compact");
                    Assert.IsNotNull(created); //exists

                }
                
                
            }

            
            using (var sr = new StringReader(str))
            {
                var comp = Document.Parse(sr, ParseSourceType.DynamicContent);
                var div2 = comp as HTMLDiv;
                
                var doc2 = new HTMLDocument();
                doc2.Body = new HTMLBody();

                doc2.Body.Contents.Add(div2);

                doc2.Params["model"] = new
                {
                    status = "cancelled",
                    score = 20,
                    temperature = 30,
                    stock = 40,
                    age = 19,
                    hasLicense = true,
                    isAdmin= false,
                    layoutType = "detailed"
                };
                
                doc2.InitializeAndLoad(OutputFormat.PDF);
                doc2.DataBind(OutputFormat.PDF);

                using (var stream = DocStreams.GetOutputStream("Handlebars_CheckHelperMatching6_v2.pdf"))
                {
                    doc2.RenderToPDF(stream);
                    
                    var created = doc2.FindAComponentById("is_valid");
                    Assert.IsNull(created); //exists

                    created = doc2.FindAComponentById("is_adult");
                    Assert.IsNotNull(created); //does not exist

                    created = doc2.FindAComponentById("did_pass");
                    Assert.IsNull(created); //exists

                    created = doc2.FindAComponentById("is_freezing");
                    Assert.IsNotNull(created); //does not exist
                    
                    created = doc2.FindAComponentById("stock_is_low");
                    Assert.IsNull(created); //exists
                    
                    created = doc2.FindAComponentById("can_drive");
                    Assert.IsNotNull(created); //exists
                    
                    created = doc2.FindAComponentById("cant_drive");
                    Assert.IsNull(created); //exists
                    
                    created = doc2.FindAComponentById("has_admin");
                    Assert.IsNull(created); //does not exist
                    
                    created = doc2.FindAComponentById("detailed");
                    Assert.IsNotNull(created); //exists
                    
                    created = doc2.FindAComponentById("summary");
                    Assert.IsNull(created); //does not exist
                    
                    created = doc2.FindAComponentById("compact");
                    Assert.IsNull(created); //does not exist
                }
                
            }

        }
        
        [TestMethod]
        public void CheckHelperMatching7_With()
        {
            var str = @"
<div xmlns='http://www.w3.org/1999/xhtml' >
    {{#with model.user }}
        <h4>Inside With Block</h4>
        <p id='inside_user_age'>age: {{age ?? 'NOT FOUND'}}</p>
        <p id='inside_user_license'>this.hasLicense: {{this.hasLicense ?? 'NOT FOUND'}}</p>
        <p id='inside_user_moderator'>.isModerator: {{.isModerator ?? 'NOT FOUND'}}</p>
        <p id='inside_user_status'>status: {{status ?? 'NOT FOUND'}}</p>
        <p id='inside_user_model_status'>model.status: {{model.status ?? 'NOT FOUND'}}</p>
        <p id='inside_user_dot_status'>.status: {{.status ?? 'NOT FOUND'}}</p>
        <p id='inside_user_this_status'>this.status: {{this.status ?? 'NOT FOUND'}}</p>
        <p id='inside_parent_status'>../status: {{../status ?? 'NOT FOUND'}}</p>
        {{#if .age >= 17 && .hasLicense}}
            <p id='can_drive'>Eligible to drive</p>
        {{else}}
            <p id='cant_drive'>NOT eligible to drive</p>
        {{/if}}
        
        {{#if .age >= 18}}
            <p id='can_drink' >Age : {{age}}, so eligible to drink</p>
        {{/if}}

    {{/with}}  

    <h4>Outside With Block</h4>
    <p id='outside_user_age'>age: {{age ?? 'NOT FOUND'}}</p>
    <p id='outside_user_license'>this.hasLicense :{{this.hasLicense ?? 'NOT FOUND'}}</p>
    <p id='outside_user_moderator'>.isModerator :{{.isModerator ?? 'NOT FOUND'}}</p>
    <p id='outside_user_status'>status :{{status ?? 'NOT FOUND'}}</p>
    <p id='outside_user_model_status'>model.status: {{model.status ?? 'NOT FOUND'}}</p>
    <p id='outside_user_dot_status'>.status :{{.status ?? 'NOT FOUND'}}</p>
    <p id='outside_user_this_status'>this.status :{{this.status ?? 'NOT FOUND'}}</p>
   
    <h4>Inside Empty With Block</h4>
    {{#with model.notfound}}
        <p id='does_not_exist'>This should not be used</p>
        {{else}}
        <p id='use_instead'>This should be used instead</p>
    {{/with}}

    <h4>With Nested</h4>
    {{#with model}}
        {{#with user }}
        <p id='parent_status'>../status: {{../status ?? 'NOT FOUND'}}</p>
        {{/with}}
    {{/with}}

    <h4>With As Option</h4>
    {{#with model.user as | u | }}
        <p id='as_u_age'>age: {{u.age ?? 'NOT FOUND'}}</p>
    {{/with}}
        {{log ""All Done""}}
</div>
";


            // Check that it parses as a fragment
            using (var sr = new StringReader(str))
            {
                var comp = Document.Parse(sr, ParseSourceType.DynamicContent);
                var div = comp as HTMLDiv;
                Assert.IsNotNull(div);

                HTMLDocument doc = new HTMLDocument();
                doc.Body = new HTMLBody();
                doc.Body.Contents.Add(div);
                
                var style = new StyleDefn("p");
                style.Margins.Bottom = 4;
                style.Margins.Top = 4;
                style.Padding.Left = 10;
                doc.Styles.Add(style);

                style = new StyleDefn("div");
                style.Margins.Left = 10;
                doc.Styles.Add(style);
                
                
                
                doc.Params["model"] = new
                {
                    status = "delivered",
                    score = 90,
                    temperature = 40,
                    stock = 4,
                    user = new {    
                        age = 20,
                        hasLicense = false,
                        isModerator = true,
                },
                    layoutType = "compact"
                };

                doc.InitializeAndLoad(OutputFormat.PDF);
                doc.DataBind(OutputFormat.PDF);

                using (var stream = DocStreams.GetOutputStream("CheckHelperMatching7_With.pdf"))
                {
                    doc.AppendTraceLog = true;
                    doc.RenderToPDF(stream);
                    
                    /*
                       <p id='inside_user_age'>age: {{age ?? 'NOT FOUND'}}</p>
                       <p id='inside_user_license'>this.hasLicense: {{this.hasLicense ?? 'NOT FOUND'}}</p>
                       <p id='inside_user_moderator'>.isModerator: {{.isModerator ?? 'NOT FOUND'}}</p>
                       <p id='inside_user_status'>status: {{status ?? 'NOT FOUND'}}</p>
                       <p id='inside_user_model_status'>model.status: {{model.status ?? 'NOT FOUND'}}</p>
                       <p id='inside_user_dot_status'>.status: {{.status ?? 'NOT FOUND'}}</p>
                       <p id='inside_user_this_status'>this.status: {{this.status ?? 'NOT FOUND'}}</p>
                     */
                    
                    var created = doc.FindAComponentById("inside_user_age") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("20", ((TextLiteral)created.Contents[1]).Text);

                    created = doc.FindAComponentById("inside_user_license") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("False", ((TextLiteral)created.Contents[1]).Text);

                    created = doc.FindAComponentById("inside_user_moderator") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("True", ((TextLiteral)created.Contents[1]).Text);
                    
                    created = doc.FindAComponentById("inside_user_status") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("NOT FOUND", ((TextLiteral)created.Contents[1]).Text);
                    
                    created = doc.FindAComponentById("inside_user_model_status") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("delivered", ((TextLiteral)created.Contents[1]).Text);
                    
                    created = doc.FindAComponentById("inside_user_dot_status") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("NOT FOUND", ((TextLiteral)created.Contents[1]).Text);
                    
                    created = doc.FindAComponentById("inside_user_this_status") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("NOT FOUND", ((TextLiteral)created.Contents[1]).Text);
                    
                    
                    /*
                        {{#if .age >= 17 && .hasLicense}}
                           <p id='can_drive'>Eligible to drive</p>
                       {{else}}
                           <p id='cant_drive'>NOT eligible to drive</p>
                       {{/if}}
                       
                       {{#if .age >= 18}}
                           <p id='can_drink' >Age : {{age}}, so eligible to drink</p>
                       {{/if}}
                    */
                    
                    created = doc.FindAComponentById("can_drive") as HTMLParagraph;
                    Assert.IsNull(created);
                    
                    created = doc.FindAComponentById("cant_drive") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    
                    created = doc.FindAComponentById("can_drink") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    
                    /*
                       <p id='outside_user_age'>age: {{age ?? 'NOT FOUND'}}</p>
                       <p id='outside_user_license'>this.hasLicense :{{this.hasLicense ?? 'NOT FOUND'}}</p>
                       <p id='outside_user_moderator'>.isModerator :{{.isModerator ?? 'NOT FOUND'}}</p>
                       <p id='outside_user_status'>status :{{status ?? 'NOT FOUND'}}</p>
                       <p id='outside_user_model_status'>model.status: {{model.status ?? 'NOT FOUND'}}</p>
                       <p id='outside_user_dot_status'>.status :{{.status ?? 'NOT FOUND'}}</p>
                       <p id='outside_user_this_status'>this.status :{{this.status ?? 'NOT FOUND'}}</p>
                     */

                    created = doc.FindAComponentById("outside_user_age") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("NOT FOUND", ((TextLiteral)created.Contents[1]).Text);

                    created = doc.FindAComponentById("outside_user_license") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("NOT FOUND", ((TextLiteral)created.Contents[1]).Text);

                    created = doc.FindAComponentById("outside_user_moderator") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("NOT FOUND", ((TextLiteral)created.Contents[1]).Text);
                    
                    created = doc.FindAComponentById("outside_user_status") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("NOT FOUND", ((TextLiteral)created.Contents[1]).Text);
                    
                    created = doc.FindAComponentById("outside_user_model_status") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("delivered", ((TextLiteral)created.Contents[1]).Text);
                    
                    created = doc.FindAComponentById("outside_user_dot_status") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("NOT FOUND", ((TextLiteral)created.Contents[1]).Text);
                    
                    created = doc.FindAComponentById("outside_user_this_status") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("NOT FOUND", ((TextLiteral)created.Contents[1]).Text);
                    
                    
                    /*
                       {{#with model.notfound}}
                           <p id='does_not_exist'>This should not be used</p>
                           {{else}}
                           <p id='use_instead'>This should be used instead</p>
                       {{/with}}
                     */
                    
                    created = doc.FindAComponentById("does_not_exist") as HTMLParagraph;
                    Assert.IsNull(created);
                    
                    created = doc.FindAComponentById("use_instead") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("This should be used instead", ((TextLiteral)created.Contents[0]).Text);
                    
                    
                    /*
                       {{#with model}}
                           {{#with user }}
                           <p id='parent_status'>../status: {{../status ?? 'NOT FOUND'}}</p>
                           {{/with}}
                       {{/with}}
                     */
                    
                    created = doc.FindAComponentById("parent_status") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("delivered", ((TextLiteral)created.Contents[1]).Text);
                    
                    /*
                     {{#with model.user as | u | }}
                         <p id='as_u_age'>age: {{u.age ?? 'NOT FOUND'}}</p>
                     {{/with}}
                     */
                    
                    created = doc.FindAComponentById("as_u_age") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("20", ((TextLiteral)created.Contents[1]).Text);
                }
                
                
            }

        }
        
        [TestMethod]
        public void CheckHelperMatching8_Log()
        {
            var str = @"
<div xmlns='http://www.w3.org/1999/xhtml' >
    Before the log entry
    {{log ""From the log"" }}
    {{#with model.user }}
        {{log ""Debug Level"" level=""debug""}}
        {{log ""Ignored parameter"" unknown=""something""}}
        {{log ""Has Licence : "" hasLicense "" Is Moderator : "" isModerator }}
        {{log ""Has Licence : "" hasLicense "" Is Moderator : "" isModerator level=""warn"" category=""output"" }}
        {{log ""Has Licence : "" hasLicense "" Is Moderator : "" if(isModerator,""moderator"", concat(""not "", ""moderator"")) level=""warn"" }}
        <h4>Inside With Block</h4>
        <p id='inside_user_age'>age: {{age ?? 'NOT FOUND'}}</p>
        <p id='inside_user_license'>this.hasLicense: {{this.hasLicense ?? 'NOT FOUND'}}</p>
        <p id='inside_user_moderator'>.isModerator: {{.isModerator ?? 'NOT FOUND'}}</p>
        <p id='inside_user_status'>status: {{status ?? 'NOT FOUND'}}</p>
        <p id='inside_user_model_status'>model.status: {{model.status ?? 'NOT FOUND'}}</p>
        <p id='inside_user_dot_status'>.status: {{.status ?? 'NOT FOUND'}}</p>
        <p id='inside_user_this_status'>this.status: {{this.status ?? 'NOT FOUND'}}</p>
        <p id='inside_parent_status'>../status: {{../status ?? 'NOT FOUND'}}</p>
        {{#if .age >= 17 && .hasLicense}}
            {{log ""Is Eligible to drive"" }}
            <p id='can_drive'>Eligible to drive</p>
        {{else}}
            {{log ""Not Eligible to drive"" }}
            <p id='cant_drive'>NOT eligible to drive</p>
        {{/if}}
        
        {{#if .age >= 18}}
            <p id='can_drink' >Age : {{age}}, so eligible to drink</p>
        {{/if}}

    {{/with}}  
</div>
";


            // Check that it parses as a fragment
            using (var sr = new StringReader(str))
            {
                var comp = Document.Parse(sr, ParseSourceType.DynamicContent);
                var div = comp as HTMLDiv;
                Assert.IsNotNull(div);

                HTMLDocument doc = new HTMLDocument();
                doc.Body = new HTMLBody();
                doc.Body.Contents.Add(div);
                
                doc.Params["model"] = new
                {
                    status = "delivered",
                    score = 90,
                    temperature = 40,
                    stock = 4,
                    user = new {    
                        age = 20,
                        hasLicense = false,
                        isModerator = true,
                },
                    layoutType = "compact"
                };

                doc.InitializeAndLoad(OutputFormat.PDF);
                

                using (var stream = DocStreams.GetOutputStream("CheckHelperMatching8_Log.pdf"))
                {
                    doc.AppendTraceLog = true;
                    doc.ConformanceMode = ParserConformanceMode.Strict;
                    
                    doc.DataBind(OutputFormat.PDF);
                    doc.RenderToPDF(stream);
                    
                    
                    
                    var created = doc.FindAComponentById("inside_user_age") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    Assert.AreEqual("20", ((TextLiteral)created.Contents[1]).Text);

                    
                    
                    
                    
                    created = doc.FindAComponentById("can_drive") as HTMLParagraph;
                    Assert.IsNull(created);
                    
                    created = doc.FindAComponentById("cant_drive") as HTMLParagraph;
                    Assert.IsNotNull(created);
                    
                    created = doc.FindAComponentById("can_drink") as HTMLParagraph;
                    Assert.IsNotNull(created);
                }
                
                
            }

        }
        
        [TestMethod]
        public void CheckHelperMatching20()
        {
            var style = @"body {
            font-family: Helvetica, sans-serif;
            margin: 20pt;
            font-size: 12pt;
        }
        h1 {
            color: #1e40af;
        }
        h2 {
            font-size: 16pt;
            margin-bottom: 5pt;
}
        .user-badge {
            display: inline-block;
            padding: 5pt 15pt;
            border-radius: 15pt;
            font-size: 10pt;
            font-weight: bold;
            margin-left: 10pt;
        }
        .badge-premium {
            background-color: #fef3c7;
            color: #92400e;
        }
        .badge-admin {
            background-color: #dbeafe;
            color: #1e40af;
        }
        .badge-standard {
            background-color: #f3f4f6;
            color: #4b5563;
        }
        .metric-box {
            display: inline-block;
            width: 30%;
            padding: 10pt;
            margin: 10pt 1%;
            border: 2pt solid #2563eb;
            border-radius: 5pt;
            text-align: center;
        }
        .metric-value {
            font-size: 24pt;
            font-weight: bold;
            color: #2563eb;
        }
        .alert {
            padding: 5pt;
            margin: 5pt 0;
            border-radius: 5pt;
        }
        .alert-warning {
            background-color: #fef3c7;
            border-left: 4pt solid #f59e0b;
        }
        .alert-info {
            background-color: #dbeafe;
            border-left: 4pt solid #2563eb;
        }
        .alert > i {
            display: inline-block;
            vertical-align : bottom;
            border: solid 1pt #f59e0b;
            color: #f59e0b;
            font-size: 10pt;
            font-style: normal;
            padding: 2pt 5pt;
            border-radius : 8pt;
            font-weight: 600;
        }
        .alert.alert-info > i {
            padding: 2pt 7pt;
            border-color: #2563eb;
            color: #2563eb;
        }
        table {
            width: 100%;
            margin: 20pt 0;
        }
        th {
            background-color: #2563eb;
            color: white;
            padding: 5pt;
            text-align: left;
            margin: 0pt
        }
        td {
            padding: 4pt;
            border: none;
            border-bottom: 1pt solid #e5e7eb;
            margin:0pt
        }";

            var str = $@"
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Sales Bar Chart</title>
    <style>
        body {{
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }}
        h1 {{
            color: #1e40af;
            margin-bottom: 5pt;
        }}
        .subtitle {{
            color: #666;
            font-size: 10pt;
            margin-bottom: 30pt;
        }}
        .chart-container {{
            margin: 30pt 0;
            text-align: center;
        }}
        .summary-box {{
            display: inline-block;
            width: 30%;
            padding: 15pt;
            margin: 10pt 1%;
            background-color: #eff6ff;
            border: 2pt solid #2563eb;
            border-radius: 5pt;
            text-align: center;
        }}
        .summary-value {{
            font-size: 20pt;
            font-weight: bold;
            color: #2563eb;
        }}
        .legend {{
            margin: 20pt 0;
            font-size: 9pt;
        }}
        .legend-item {{
            display: inline-block;
            margin-right: 20pt;
        }}
        .legend-color {{
            display: inline-block;
            width: 15pt;
            height: 15pt;
            margin-right: 5pt;
            vertical-align: middle;
        }}
        table {{
            width: 100%;
            border-collapse: collapse;
            margin-top: 30pt;
        }}
        th {{
            background-color: #2563eb;
            color: white;
            padding: 10pt;
            text-align: left;
        }}
        td {{
            padding: 8pt;
            border-bottom: 1pt solid #e5e7eb;
        }}
        .status-met {{
            color: #059669;
            font-weight: bold;
        }}
        .status-missed {{
            color: #dc2626;
            font-weight: bold;
        }}
    </style>
</head>
<body>
    <h1>{{{{model.reportTitle}}}}</h1>
    <p class=""subtitle"">Generated: {{{{format(model.reportDate, 'MMMM dd, yyyy')}}}}</p>

    <!-- Summary Metrics -->
    <div style=""margin-bottom: 30pt;"">
        <div class=""summary-box"">
            <div class=""summary-value"">{{{{format(model.totalRevenue, 'C0')}}}}</div>
            <div>Total Revenue</div>
        </div>
        <div class=""summary-box"">
            <div class=""summary-value"">{{{{model.categories.length}}}}</div>
            <div>Categories</div>
        </div>
        <div class=""summary-box"">
            <div class=""summary-value"">
            </div>
            <div>Targets Met</div>
        </div>
    </div>

    <!-- SVG Bar Chart -->
    <div class=""chart-container"">
        <svg width=""{{{{model.chartWidth}}}}pt"" height=""{{{{model.chartHeight}}}}pt""
             xmlns=""http://www.w3.org/2000/svg"">

            <!-- Chart Title -->
            <text x=""{{{{model.chartWidth / 2}}}}"" y=""20""
                  text-anchor=""middle""
                  font-size=""12""
                  font-weight=""600""
                  fill=""#1e40af"">
                Sales Revenue by Category
            </text>

            <!-- Y-axis labels -->
            <text x=""35"" y=""50"" text-anchor=""end"" font-size=""9"" fill=""#666"">
                ${{{{format(model.maxValue, '#,##0')}}}}
            </text>
            <text x=""35"" y=""150"" text-anchor=""end"" font-size=""9"" fill=""#666"">
                ${{{{format(model.maxValue / 2, '#,##0')}}}}
            </text>
            <text x=""35"" y=""245"" text-anchor=""end"" font-size=""9"" fill=""#666"">
                $0
            </text>

            <!-- Y-axis line -->
            <line x1=""40"" y1=""40"" x2=""40"" y2=""240""
                  stroke=""#d1d5db"" stroke-width=""2"" />

            <!-- X-axis line -->
            <line x1=""40"" y1=""240"" x2=""540"" y2=""240""
                  stroke=""#d1d5db"" stroke-width=""2"" />

            <!-- Grid lines -->
            <line x1=""40"" y1=""140"" x2=""540"" y2=""140""
                  stroke=""#e5e7eb"" stroke-width=""1"" stroke-dasharray=""5,5"" />
            <line x1=""40"" y1=""40"" x2=""540"" y2=""40""
                  stroke=""#e5e7eb"" stroke-width=""1"" stroke-dasharray=""5,5"" />

            
            <!-- Bars and Labels -->
            {{{{#each model.categories}}}}
                <!-- Calculate bar heights using template variables (0-200pt scale) -->
                <var data-id=""barHeight"" data-value=""{{{{this.revenue / model.maxValue * 200}}}}"" />
                <var data-id=""targetLineHeight"" data-value=""{{{{this.target / model.maxValue * 200}}}}"" />
                <var data-id=""barIndex"" data-value=""{{{{@index}}}}"" />

                <!-- Calculate bar position (5 categories, 100pt width each) -->
                <g>
                    <!-- Bar background -->
                    <rect x=""{{{{50 + @index * 100}}}}""
                          y=""40""
                          width=""70""
                          height=""200""
                          fill=""#f9fafb""
                          stroke=""#e5e7eb"" />


                    <!-- Revenue bar -->
                    <rect x=""{{{{50 + (barIndex * 100)}}}}""
                          y=""{{{{240 - barHeight}}}}""
                          width=""70""
                          height=""{{{{barHeight}}}}""
                          fill=""{{{{if(this.metTarget, '#10b981', '#ef4444')}}}}""
                           />

                    <!-- Target line -->
                    <line x1=""{{{{50 + (@index * 100)}}}}""
                          y1=""{{{{240 - targetLineHeight}}}}""
                          x2=""{{{{120 + index() * 100}}}}""
                          y2=""{{{{240 - targetLineHeight}}}}""
                          stroke=""#2563eb""
                          stroke-width=""2"" />

                    <!-- Revenue value on top of bar (only show if bar is tall enough) -->
                    {{{{#if barHeight > 30}}}}
                    <text x=""{{{{85 + (barIndex * 100)}}}}""
                          y=""{{{{250 - barHeight}}}}""
                          text-anchor=""middle""
                          font-size=""8""
                          font-weight=""600""
                          fill=""white"">
                        {{{{format(this.revenue, 'C0')}}}}
                    </text>
                    {{{{/if}}}}

                    <!-- Category name -->
                    <text x=""{{{{85 + @index * 100}}}}""
                          y=""248""
                          text-anchor=""middle""
                          font-size=""9""
                          fill=""#374151"">
                        {{{{this.name}}}}
                    </text>

                    <!-- Performance indicator -->
                    {{{{#if this.metTarget}}}}
                        <text x=""{{{{85 + @index * 100}}}}""
                              y=""268""
                              text-anchor=""middle""
                              font-size=""8""
                              fill=""#059669""
                              font-weight=""600"">
                            ✓ {{{{format(this.percentOfTarget, '0')}}}}%
                        </text>
                    {{{{else}}}}
                        <text x=""{{{{85 + @index * 100}}}}""
                              y=""268""
                              text-anchor=""middle""
                              font-size=""8""
                              fill=""#dc2626""
                              font-weight=""600"">
                            {{{{format(this.percentOfTarget, '0')}}}}%
                        </text>
                    {{{{/if}}}}
                </g>
            {{{{/each}}}}

            <!-- Legend -->
            <g transform=""translate(40, 300)"">
                <rect x=""0"" y=""0"" width=""15"" height=""15"" fill=""#10b981"" opacity=""0.8"" />
                <text x=""20"" y=""12"" font-size=""9"" fill=""#374151"">Revenue (Met Target)</text>

                <rect x=""150"" y=""0"" width=""15"" height=""15"" fill=""#ef4444"" opacity=""0.8"" />
                <text x=""170"" y=""12"" font-size=""9"" fill=""#374151"">Revenue (Missed Target)</text>

                <line x1=""300"" y1=""7.5"" x2=""315"" y2=""7.5""
                      stroke=""#2563eb"" stroke-width=""2"" />
                <text x=""320"" y=""12"" font-size=""9"" fill=""#374151"">Target</text>
            </g>
        </svg>
    </div>

    <!-- Detailed Data Table -->
    <h2 style='page-break-before: always'>Detailed Breakdown</h2>
    <table>
        <thead>
            <tr>
                <th>Category</th>
                <th style=""text-align: right;"">Revenue</th>
                <th style=""text-align: right;"">Target</th>
                <th style=""text-align: right;"">% of Target</th>
                <th style=""text-align: center;"">Status</th>
            </tr>
        </thead>
        <tbody>
            {{{{#each model.categories}}}}
            <tr>
                <td><strong>{{{{this.name}}}}</strong></td>
                <td style=""text-align: right;"">{{{{format(this.revenue, 'C0')}}}}</td>
                <td style=""text-align: right;"">{{{{format(this.target, 'C0')}}}}</td>
                <td style=""text-align: right;"">{{{{format(this.percentOfTarget, '0.0')}}}}%</td>
                <td style=""text-align: center;"">
                    {{{{#if this.metTarget}}}}
                        <span class=""status-met"">✓ Met</span>
                    {{{{else}}}}
                        <span class=""status-missed"">✗ Missed</span>
                    {{{{/if}}}}
                </td>
            </tr>
            {{{{/each}}}}
        </tbody>
        <tfoot>
            <tr style=""background-color: #eff6ff; font-weight: bold;"">
                <td>TOTAL</td>
                <td style=""text-align: right;"">{{{{format(model.totalRevenue, 'C0')}}}}</td>
                <td colspan=""3""></td>
            </tr>
        </tfoot>
    </table>

    <!-- Conditional Insights -->
    <h2>Key Insights</h2>
    {{{{#each model.categories}}}}
        {{{{#if this.metTarget}}}}
            {{{{#if this.percentOfTarget >= 120}}}}
                <div style=""padding: 10pt; margin: 10pt 0; background-color: #d1fae5; border-left: 4pt solid #059669;"">
                    <strong>{{{{this.name}}}}</strong> exceeded expectations by {{{{format(this.percentOfTarget - 100, '0')}}}}%!
                    Outstanding performance.
                </div>
            {{{{/if}}}}
        {{{{else}}}}
            {{{{#if this.percentOfTarget < 80}}}}
                <div style=""padding: 10pt; margin: 10pt 0; background-color: #fee2e2; border-left: 4pt solid #dc2626;"">
                    <strong>{{{{this.name}}}}</strong> significantly underperformed at {{{{format(this.percentOfTarget, '0')}}}}% of target.
                    Immediate attention required.
                </div>
            {{{{else}}}}
                <div style=""padding: 10pt; margin: 10pt 0; background-color: #fef3c7; border-left: 4pt solid #f59e0b;"">
                    <strong>{{{{this.name}}}}</strong> missed target by {{{{format(100 - this.percentOfTarget, '0')}}}}%.
                    Monitor closely next quarter.
                </div>
            {{{{/if}}}}
        {{{{/if}}}}
    {{{{/each}}}}
</body>
</html>

";
            
            //Added 'this' support, and ../ support
            //TODO: Add format function same as string(value, format)
            //TODO: Add functions for add(), subtract(), divide(), multiply()

        using (var reader = new System.IO.StringReader(str))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.ConformanceMode = ParserConformanceMode.Strict;
                var categories = new[]
                {
                    new { name = "Electronics", revenue = 125000m, target = 100000m },
                    new { name = "Clothing", revenue = 98000m, target = 110000m },
                    new { name = "Home & Garden", revenue = 87000m, target = 80000m },
                    new { name = "Sports", revenue = 76000m, target = 85000m },
                    new { name = "Books", revenue = 54000m, target = 60000m }
                };

// Find max for scaling (needed for template calculations)
                var maxRevenue = categories.Max(c => c.revenue);
                var maxValue = Math.Max(maxRevenue, categories.Max(c => c.target));
                
                doc.Params["model"] = new
                {
                    reportTitle = "Q4 Sales Performance by Category",
                    reportDate = DateTime.Now,
                    totalRevenue = categories.Sum(c => c.revenue),
                    categories = categories.Select(c => new
                    {
                        name = c.name,
                        revenue = c.revenue,
                        target = c.target,
                        metTarget = c.revenue >= c.target,
                        percentOfTarget = (c.revenue / c.target) * 100
                        // Heights will be calculated in the template
                    }).ToArray(),
                    chartHeight = 320,
                    chartWidth = 550,
                    maxValue = maxValue  // Used for scaling calculations in template
                };


                using (var output = DocStreams.GetOutputStream("Handlebars_CheckHelperMatching7.pdf"))
                {
                    doc.AppendTraceLog = false;
                    doc.SaveAsPDF(output);
                }
                
                
            }
        }


    }
}
