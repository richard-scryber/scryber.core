using System;
using System.IO;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Data;
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
        public void CheckHelperMatching()
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
        public void CheckHelperMatching2()
        {

        
        var str = @"{{#each collection}}
<!-- Content repeated for each item -->
{{this.property}}
{{/each}}";
            
            
            var expected =
                @"<hbar:each xmlns:hbar='Scryber.Handlebar.Components, Scryber.Components' data-bind='{{collection}}' >
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
        public void CheckHelperMatching3()
        {
            var str = @"{{#each items}}
                        <li>{{this}}</li>
                    {{/each}}";
            var expected =
                @"<hbar:each xmlns:hbar='Scryber.Handlebar.Components, Scryber.Components' data-bind='{{items}}' >
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
        public void CheckHelperMatching4()
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
<hbar:each xmlns:hbar='Scryber.Handlebar.Components, Scryber.Components' data-bind='{{products}}' >
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
        public void CheckHelperMatching5()
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
	<hbar:when data-test='{{model.priority == 0}}' >
    <span class=""badge low"">Low Priority</span>
</hbar:when>
	<hbar:when data-test='{{model.priority == 1}}' >
    <span class=""badge medium"">Medium Priority</span>
</hbar:when>
	<hbar:when data-test='{{model.priority == 2}}' >
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
        public void CheckHelperMatching6()
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
        public void CheckHelperMatching7()
        {

            var str = @"
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Sales Report</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }
        h1 {
            color: #1e40af;
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
            padding: 15pt;
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
            padding: 10pt;
            margin: 10pt 0;
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
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }
        th {
            background-color: #2563eb;
            color: white;
            padding: 10pt;
            text-align: left;
        }
        td {
            padding: 8pt;
            border-bottom: 1pt solid #e5e7eb;
        }
    </style>
</head>
<body>
    <h1>
        Sales Report for {{model.userName}}
        {{#if model.userLevel == 'premium'}}
            <span class=""user-badge badge-premium"">★ Premium</span>
        {{else if model.userLevel == 'admin'}}
            <span class=""user-badge badge-admin"">⚙ Admin</span>
        {{else}}
            <span class=""user-badge badge-standard"">Standard</span>
        {{/if}}
    </h1>

    <p style=""color: #666; font-size: 10pt;"">
        Generated: {{string(model.reportDate, 'MMMM dd, yyyy HH:mm')}}
    </p>

    <!-- Key Metrics (visible to all) -->
    <h2>Key Metrics</h2>
    <div>
        <div class=""metric-box"">
            <div class=""metric-value"">{{string(model.metrics.totalSales, 'C0')}}</div>
            <div>Total Sales</div>
        </div>
        <div class=""metric-box"">
            <div class=""metric-value"">{{model.metrics.newCustomers}}</div>
            <div>New Customers</div>
        </div>
        <div class=""metric-box"">
            <div class=""metric-value"">{{string(model.metrics.conversionRate, 'P1')}}</div>
            <div>Conversion Rate</div>
        </div>
    </div>

    <!-- Detailed Analytics (Premium and Admin only) -->
    {{#if model.userLevel == 'premium' || model.userLevel == 'admin'}}
        <h2>Detailed Category Analysis</h2>
        <table>
            <thead>
                <tr>
                    <th>Category</th>
                    <th style=""text-align: right;"">Revenue</th>
                    <th style=""text-align: right;"">% of Total</th>
                </tr>
            </thead>
            <tbody>
                {{#each model.detailedAnalytics}}
                <tr>
                    <td>{{this.category}}</td>
                    <td style=""text-align: right;"">{{string(this.revenue, 'C0')}}</td>
                    <td style=""text-align: right;"">
                        {{format(this.revenue / model.metrics.totalSales), 'P0')}}
                    </td>
                </tr>
                {{/each}}
            </tbody>
        </table>
    {{else}}
        <div style=""padding: 20pt; background-color: #fef3c7; border: 2pt solid #f59e0b; border-radius: 5pt; margin: 20pt 0;"">
            <strong>Upgrade to Premium</strong> to see detailed category analysis and more!
        </div>
    {{/if}}

    <!-- Alerts -->
    {{#if model.alerts.length > 0}}
        <h2>Alerts & Notifications</h2>
        {{#each model.alerts}}
            <div class=""alert alert-{{this.type}}"">
                {{#if this.type == 'warning'}}⚠{{else}}ℹ{{/if}}
                {{this.message}}
            </div>
        {{/each}}
    {{/if}}

    <!-- Admin Section -->
    {{#if model.isAdmin}}
        <div style=""margin-top: 40pt; padding: 20pt; background-color: #eff6ff; border: 2pt solid #2563eb;"">
            <h2>Admin Tools</h2>
            <p>Additional administrative controls and reports are available.</p>
            <ul>
                <li>User management</li>
                <li>System configuration</li>
                <li>Audit logs</li>
            </ul>
        </div>
    {{/if}}
</body>
</html>";
            
            //TODO: Add 'this' support, and ../ support
            //TODO: Add format function same as string(value, format)
            //TODO: Add functions for add(), subtract(), divide(), multiply()

        using (var reader = new System.IO.StringReader(str))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["model"] = new
                {
                    reportDate = DateTime.Now,
                    userName = "John Smith",
                    isAdmin = true,
                    userLevel = "premium",
                    alerts = new[]
                    {
                        new { type = "warning", message = "This is a warning message" },
                        new { type = "note", message = "This is just a note" },
                        new { type = "warning", message = "This is another warning" },
                        new { type = "verbose", message = "Just a debug message" }
                    },
                    detailedAnalytics = new []
                    {
                        new { category = "Online", revenue = 123.40, },
                        new { category = "In Store", revenue = 423.40 },
                        new { category = "Advertising", revenue = 0.40 }
                    },
                    metrics = new { totalSales = 547.2, newCustomers = 10, conversionRate = 0.21 }
                };

                using (var output = DocStreams.GetOutputStream("Handlebars_CheckHelperMatching7"))
                {
                    doc.SaveAsPDF(output);
                }

                doc.InitializeAndLoad(OutputFormat.PDF);
                doc.DataBind(OutputFormat.PDF);
                
                
            }
        }


    }
}
