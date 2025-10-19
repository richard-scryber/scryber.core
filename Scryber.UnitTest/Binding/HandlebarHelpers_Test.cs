using System;
using System.IO;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Data;
using Scryber.Drawing;
using Scryber.Handlebar.Components;
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
    <p>This order is valid.</p>
{{/if}}
<!-- Greater than -->
{{#if model.age > 18}}
    <p>Adult</p>
{{/if}}

<!-- Greater than or equal -->
{{#if model.score >= 70}}
    <p class=""pass"">Passed</p>
{{/if}}

<!-- Less than -->
{{#if model.temperature < 32}}
    <p>Freezing conditions</p>
{{/if}}

<!-- Less than or equal -->
{{#if model.stock <= 10}}
    <p class=""warning"">Low stock!</p>
{{/if}}

{{#if model.age >= 18 && model.hasLicense}}
    <p>Eligible to drive</p>
{{else}}
    <p>Not eligible to drive</p>
{{/if}}
</div>
";


            // Check that it parses as a fragment
            using (var sr = new StringReader(str))
            {
                var comp = Document.Parse(sr, ParseSourceType.DynamicContent);
            }

        }
        
        public void CheckHelperMatching7()
        {

            var str = "";


/*

{{#if model.isAdmin || model.isModerator}}
    <div class=""admin-panel"">
        <h2>Administration</h2>
        <!-- Admin controls -->
    </div>
{{/if}}

{{#if model.layoutType == 'summary'}}
    <div class=""summary-layout"">
        <h1>{{model.title}}</h1>
        <p>{{model.summary}}</p>
    </div>
{{else if model.layoutType == 'detailed'}}
    <div class=""detailed-layout"">
        <h1>{{model.title}}</h1>
        <p>{{model.summary}}</p>
        <div class=""details"">
            <p>{{model.fullContent}}</p>
        </div>
    </div>
{{else}}
    <div class=""compact-layout"">
        <h2>{{model.title}}</h2>
    </div>
{{/if}}

{{#if model.orders.length > 0}}
    <h2>Recent Orders</h2>
    <table>
        <thead>
            <tr>
                <th>Order #</th>
                <th>Date</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>
            {{#each model.orders}}
            <tr>
                <td>{{this.orderNumber}}</td>
                <td>{{format(this.date, 'yyyy-MM-dd')}}</td>
                <td>${{format(this.total, 'F2')}}</td>
            </tr>
            {{/each}}
        </tbody>
    </table>
{{else}}
    <p>No orders found.</p>
{{/if}}

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
        Generated: {{format(model.reportDate, 'MMMM dd, yyyy HH:mm')}}
    </p>

    <!-- Key Metrics (visible to all) -->
    <h2>Key Metrics</h2>
    <div>
        <div class=""metric-box"">
            <div class=""metric-value"">{{format(model.metrics.totalSales, 'C0')}}</div>
            <div>Total Sales</div>
        </div>
        <div class=""metric-box"">
            <div class=""metric-value"">{{model.metrics.newCustomers}}</div>
            <div>New Customers</div>
        </div>
        <div class=""metric-box"">
            <div class=""metric-value"">{{format(model.metrics.conversionRate, 'P1')}}</div>
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
                    <td style=""text-align: right;"">{{format(this.revenue, 'C0')}}</td>
                    <td style=""text-align: right;"">
                        {{format(calc(this.revenue, '/', ../metrics.totalSales), 'P0')}}
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
</html>"; */

        using (var reader = new System.IO.StringReader(str))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/Toroid24.jpg", this.TestContext);

                var imgReader = Scryber.Imaging.ImageReader.Create();
                ImageData data;
                
                using (var fs = new System.IO.FileStream(path, FileMode.Open))
                {
                    data = imgReader.ReadStream(path, fs, false);
                }
                
                
                doc.Params["MyImage"] = data;

                var img = doc.FindAComponentById("LoadedImage") as Image;
                Assert.IsNotNull(img);
                Assert.IsNull(img.Data);

                doc.InitializeAndLoad(OutputFormat.PDF);
                doc.DataBind(OutputFormat.PDF);
                
                //Makes sure the image data is bound to the element
                Assert.IsNotNull(img.Data);
                Assert.AreEqual(data, img.Data);

            }
        }


    }
}
