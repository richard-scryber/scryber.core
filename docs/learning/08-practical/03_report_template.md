---
layout: default
title: Report Template
nav_order: 3
parent: Practical Applications
parent_url: /learning/08-practical/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Report Template

Build comprehensive multi-section reports with cover pages, table of contents, charts, tables, and professional page numbering.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Create professional report cover pages
- Generate table of contents
- Structure multi-section reports
- Add charts and visualizations
- Implement page headers and footers
- Number pages professionally
- Include executive summaries

---

## Complete Report Template

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>{{report.title}}</title>
    <style>
        @page {
            size: letter;
            margin: 72pt 72pt 100pt 72pt;
        }

        @page cover {
            margin: 0;
        }

        @page toc {
            margin: 72pt;
        }

        body {
            font-family: 'Helvetica', 'Arial', sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            color: #333;
        }

        /* Cover Page */
        .cover-page {
            page: cover;
            height: 100%;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            text-align: center;
            page-break-after: always;
        }

        .cover-logo {
            height: 80pt;
            margin-bottom: 40pt;
        }

        .cover-title {
            font-size: 36pt;
            font-weight: bold;
            margin: 0 0 20pt 0;
            line-height: 1.2;
        }

        .cover-subtitle {
            font-size: 18pt;
            margin: 0 0 40pt 0;
            opacity: 0.9;
        }

        .cover-meta {
            font-size: 14pt;
            margin: 10pt 0;
        }

        .cover-footer {
            position: absolute;
            bottom: 40pt;
            left: 0;
            right: 0;
            text-align: center;
            font-size: 12pt;
            opacity: 0.8;
        }

        /* Table of Contents */
        .toc {
            page: toc;
            page-break-after: always;
        }

        .toc h1 {
            font-size: 24pt;
            color: #2563eb;
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 15pt;
            margin-bottom: 30pt;
        }

        .toc-entry {
            display: flex;
            justify-content: space-between;
            padding: 8pt 0;
            border-bottom: 1pt dotted #ddd;
            margin-bottom: 5pt;
        }

        .toc-entry:hover {
            background-color: #f3f4f6;
        }

        .toc-title {
            flex: 1;
        }

        .toc-page {
            margin-left: 10pt;
            color: #666;
        }

        .toc-section-1 {
            font-weight: bold;
            font-size: 12pt;
            color: #2563eb;
        }

        .toc-section-2 {
            margin-left: 20pt;
            font-size: 11pt;
        }

        .toc-section-3 {
            margin-left: 40pt;
            font-size: 10pt;
            color: #666;
        }

        /* Executive Summary */
        .executive-summary {
            background-color: #eff6ff;
            border-left: 4pt solid #2563eb;
            padding: 20pt;
            margin: 30pt 0;
            page-break-inside: avoid;
        }

        .executive-summary h2 {
            margin-top: 0;
            color: #2563eb;
        }

        /* Sections */
        h1 {
            font-size: 20pt;
            color: #2563eb;
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 10pt;
            margin-top: 30pt;
            margin-bottom: 20pt;
            page-break-before: always;
        }

        h1:first-of-type {
            page-break-before: auto;
        }

        h2 {
            font-size: 16pt;
            color: #1e40af;
            margin-top: 25pt;
            margin-bottom: 15pt;
        }

        h3 {
            font-size: 13pt;
            color: #1e3a8a;
            margin-top: 20pt;
            margin-bottom: 10pt;
        }

        /* Paragraphs */
        p {
            margin-bottom: 12pt;
            text-align: justify;
        }

        /* Tables */
        .data-table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
            font-size: 10pt;
        }

        .data-table thead {
            background-color: #2563eb;
            color: white;
        }

        .data-table th {
            padding: 10pt;
            text-align: left;
            font-weight: bold;
        }

        .data-table th.right {
            text-align: right;
        }

        .data-table td {
            padding: 8pt 10pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        .data-table td.right {
            text-align: right;
        }

        .data-table tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        .data-table tfoot {
            background-color: #eff6ff;
            font-weight: bold;
        }

        /* Charts */
        .chart-container {
            text-align: center;
            margin: 30pt 0;
            page-break-inside: avoid;
        }

        .chart-title {
            font-weight: bold;
            font-size: 12pt;
            margin-bottom: 15pt;
            color: #2563eb;
        }

        .chart-image {
            max-width: 100%;
            height: auto;
        }

        .chart-caption {
            font-size: 9pt;
            color: #666;
            margin-top: 10pt;
            font-style: italic;
        }

        /* Key Metrics */
        .metrics-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 20pt;
            margin: 30pt 0;
        }

        .metric-card {
            background: #f9fafb;
            border: 2pt solid #e5e7eb;
            border-radius: 8pt;
            padding: 20pt;
            text-align: center;
        }

        .metric-value {
            font-size: 28pt;
            font-weight: bold;
            color: #2563eb;
            margin: 10pt 0;
        }

        .metric-label {
            font-size: 10pt;
            color: #666;
            text-transform: uppercase;
        }

        .metric-change {
            font-size: 11pt;
            margin-top: 8pt;
        }

        .metric-change.positive {
            color: #10b981;
        }

        .metric-change.negative {
            color: #ef4444;
        }

        /* Callout Boxes */
        .callout {
            background-color: #fff7ed;
            border-left: 4pt solid #f59e0b;
            padding: 15pt;
            margin: 20pt 0;
            page-break-inside: avoid;
        }

        .callout h4 {
            margin-top: 0;
            color: #d97706;
        }

        /* Page Headers & Footers */
        .report-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding-bottom: 10pt;
            border-bottom: 1pt solid #e5e7eb;
            margin-bottom: 20pt;
        }

        .report-footer {
            position: absolute;
            bottom: 40pt;
            left: 72pt;
            right: 72pt;
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding-top: 10pt;
            border-top: 1pt solid #e5e7eb;
            font-size: 9pt;
            color: #666;
        }

        /* Appendix */
        .appendix {
            page-break-before: always;
        }

        .appendix h1 {
            color: #666;
        }
    </style>
</head>
<body>
    <!-- Cover Page -->
    <div class="cover-page">
        <img src="{{company.logo}}" class="cover-logo" alt="Company Logo" />
        <h1 class="cover-title">{{report.title}}</h1>
        <p class="cover-subtitle">{{report.subtitle}}</p>
        <div class="cover-meta">
            <p>{{report.period}}</p>
            <p>Prepared by: {{report.author}}</p>
            <p>{{report.date}}</p>
        </div>
        <div class="cover-footer">
            <p>{{company.name}} | {{company.confidentiality}}</p>
        </div>
    </div>

    <!-- Table of Contents -->
    <div class="toc">
        <h1>Table of Contents</h1>

        {{#each report.sections}}
        <div class="toc-entry toc-section-{{this.level}}">
            <span class="toc-title">{{this.number}} {{this.title}}</span>
            <span class="toc-page">{{this.page}}</span>
        </div>
        {{/each}}
    </div>

    <!-- Executive Summary -->
    <div class="executive-summary">
        <h2>Executive Summary</h2>
        {{#each report.executiveSummary}}
        <p>{{this}}</p>
        {{/each}}
    </div>

    <!-- Key Metrics Overview -->
    <h2>Key Performance Indicators</h2>

    <div class="metrics-grid">
        {{#each report.kpis}}
        <div class="metric-card">
            <div class="metric-label">{{this.label}}</div>
            <div class="metric-value">{{this.value}}</div>
            <div class="metric-change {{this.changeClass}}">
                {{this.change}}
            </div>
        </div>
        {{/each}}
    </div>

    <!-- Main Content Sections -->
    {{#each report.contentSections}}
    <h1>{{this.title}}</h1>

    {{#each this.subsections}}
    <h2>{{this.title}}</h2>

    {{#each this.paragraphs}}
    <p>{{this}}</p>
    {{/each}}

    <!-- Tables -->
    {{#if this.table}}
    <table class="data-table">
        <thead>
            <tr>
                {{#each this.table.headers}}
                <th class="{{this.align}}">{{this.label}}</th>
                {{/each}}
            </tr>
        </thead>
        <tbody>
            {{#each this.table.rows}}
            <tr>
                {{#each this.cells}}
                <td class="{{this.align}}">{{this.value}}</td>
                {{/each}}
            </tr>
            {{/each}}
        </tbody>
        {{#if this.table.totals}}
        <tfoot>
            <tr>
                {{#each this.table.totals}}
                <td class="{{this.align}}">{{this.value}}</td>
                {{/each}}
            </tr>
        </tfoot>
        {{/if}}
    </table>
    {{/if}}

    <!-- Charts -->
    {{#if this.chart}}
    <div class="chart-container">
        <div class="chart-title">{{this.chart.title}}</div>
        <img src="{{this.chart.image}}" class="chart-image" alt="{{this.chart.title}}" />
        <p class="chart-caption">Figure {{this.chart.number}}: {{this.chart.caption}}</p>
    </div>
    {{/if}}

    <!-- Callouts -->
    {{#if this.callout}}
    <div class="callout">
        <h4>{{this.callout.title}}</h4>
        <p>{{this.callout.text}}</p>
    </div>
    {{/if}}

    {{/each}}
    {{/each}}

    <!-- Appendix -->
    {{#if report.appendices}}
    <div class="appendix">
        <h1>Appendices</h1>

        {{#each report.appendices}}
        <h2>Appendix {{this.letter}}: {{this.title}}</h2>
        {{#each this.content}}
        <p>{{this}}</p>
        {{/each}}
        {{/each}}
    </div>
    {{/if}}

    <!-- Report Footer (on all pages except cover) -->
    <div class="report-footer">
        <span>{{company.name}} | {{report.title}}</span>
        <span>Page <page-number /> of <page-count /></span>
        <span>{{report.confidentiality}}</span>
    </div>
</body>
</html>
```
{% endraw %}

---

## C# Report Generator

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ReportGenerator
{
    private readonly string _templatePath;

    public ReportGenerator(string templatePath)
    {
        _templatePath = templatePath;
    }

    public void GenerateReport(ReportData reportData, Stream output)
    {
        // Load template
        var doc = Document.ParseDocument(_templatePath);

        // Set metadata
        doc.Info.Title = reportData.Report.Title;
        doc.Info.Author = reportData.Report.Author;
        doc.Info.Subject = reportData.Report.Subtitle;
        doc.Info.Keywords = string.Join(", ", reportData.Report.Tags ?? new List<string>());
        doc.Info.CreationDate = DateTime.Now;

        // Bind data
        doc.Params["company"] = reportData.Company;
        doc.Params["report"] = reportData.Report;

        // Configure for reports
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
        doc.RenderOptions.FontSubsetting = true;

        // Generate PDF
        doc.SaveAsPDF(output);
    }
}

// Data models
public class ReportData
{
    public CompanyInfo Company { get; set; }
    public Report Report { get; set; }
}

public class CompanyInfo
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public string Confidentiality { get; set; }
}

public class Report
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Period { get; set; }
    public string Author { get; set; }
    public string Date { get; set; }
    public string Confidentiality { get; set; }
    public List<string> Tags { get; set; }

    // Table of Contents
    public List<TocEntry> Sections { get; set; } = new List<TocEntry>();

    // Executive Summary
    public List<string> ExecutiveSummary { get; set; } = new List<string>();

    // KPIs
    public List<Kpi> Kpis { get; set; } = new List<Kpi>();

    // Main Content
    public List<ContentSection> ContentSections { get; set; } = new List<ContentSection>();

    // Appendices
    public List<Appendix> Appendices { get; set; } = new List<Appendix>();
}

public class TocEntry
{
    public int Level { get; set; }
    public string Number { get; set; }
    public string Title { get; set; }
    public int Page { get; set; }
}

public class Kpi
{
    public string Label { get; set; }
    public string Value { get; set; }
    public string Change { get; set; }
    public string ChangeClass { get; set; }  // "positive" or "negative"
}

public class ContentSection
{
    public string Title { get; set; }
    public List<Subsection> Subsections { get; set; } = new List<Subsection>();
}

public class Subsection
{
    public string Title { get; set; }
    public List<string> Paragraphs { get; set; } = new List<string>();
    public DataTable Table { get; set; }
    public Chart Chart { get; set; }
    public Callout Callout { get; set; }
}

public class DataTable
{
    public List<TableHeader> Headers { get; set; }
    public List<TableRow> Rows { get; set; }
    public List<TableCell> Totals { get; set; }
}

public class TableHeader
{
    public string Label { get; set; }
    public string Align { get; set; }  // "left", "right", "center"
}

public class TableRow
{
    public List<TableCell> Cells { get; set; }
}

public class TableCell
{
    public string Value { get; set; }
    public string Align { get; set; }
}

public class Chart
{
    public string Title { get; set; }
    public string Image { get; set; }
    public string Number { get; set; }
    public string Caption { get; set; }
}

public class Callout
{
    public string Title { get; set; }
    public string Text { get; set; }
}

public class Appendix
{
    public string Letter { get; set; }
    public string Title { get; set; }
    public List<string> Content { get; set; }
}
```

---

## Usage Example

```csharp
// Create report data
var reportData = new ReportData
{
    Company = new CompanyInfo
    {
        Name = "Acme Corporation",
        Logo = "./images/acme-logo.png",
        Confidentiality = "Confidential - Internal Use Only"
    },
    Report = new Report
    {
        Title = "Q4 2025 Sales Performance Report",
        Subtitle = "Comprehensive Analysis and Strategic Recommendations",
        Period = "October - December 2025",
        Author = "Sales Analytics Team",
        Date = "January 15, 2026",
        Confidentiality = "Confidential",
        Tags = new List<string> { "Sales", "Q4", "2025", "Analytics" },

        // Executive Summary
        ExecutiveSummary = new List<string>
        {
            "Q4 2025 demonstrated exceptional sales performance with total revenue reaching $12.5M, representing a 23% increase over Q4 2024. This growth was driven primarily by strong performance in the Northeast region and successful launch of three new product lines.",

            "Key achievements include exceeding annual targets by 15%, expanding our customer base by 1,200 new accounts, and improving average deal size by 18%. The sales team's focus on strategic accounts yielded significant results with enterprise sales growing 45% year-over-year.",

            "Looking ahead to 2026, we recommend continued investment in the Northeast region, expansion of our partner channel, and acceleration of digital transformation initiatives to sustain this momentum."
        },

        // KPIs
        Kpis = new List<Kpi>
        {
            new Kpi
            {
                Label = "Total Revenue",
                Value = "$12.5M",
                Change = "↑ 23% YoY",
                ChangeClass = "positive"
            },
            new Kpi
            {
                Label = "New Customers",
                Value = "1,200",
                Change = "↑ 35% YoY",
                ChangeClass = "positive"
            },
            new Kpi
            {
                Label = "Average Deal Size",
                Value = "$48K",
                Change = "↑ 18% YoY",
                ChangeClass = "positive"
            }
        },

        // Main Content
        ContentSections = new List<ContentSection>
        {
            new ContentSection
            {
                Title = "Sales Performance Overview",
                Subsections = new List<Subsection>
                {
                    new Subsection
                    {
                        Title = "Revenue by Region",
                        Paragraphs = new List<string>
                        {
                            "Regional performance analysis reveals strong growth across all territories, with the Northeast region leading at 35% growth. The West region showed steady improvement throughout the quarter, while the South region maintained consistent performance."
                        },
                        Table = new DataTable
                        {
                            Headers = new List<TableHeader>
                            {
                                new TableHeader { Label = "Region", Align = "left" },
                                new TableHeader { Label = "Q4 2025", Align = "right" },
                                new TableHeader { Label = "Q4 2024", Align = "right" },
                                new TableHeader { Label = "Growth", Align = "right" }
                            },
                            Rows = new List<TableRow>
                            {
                                new TableRow
                                {
                                    Cells = new List<TableCell>
                                    {
                                        new TableCell { Value = "Northeast", Align = "left" },
                                        new TableCell { Value = "$4.2M", Align = "right" },
                                        new TableCell { Value = "$3.1M", Align = "right" },
                                        new TableCell { Value = "35%", Align = "right" }
                                    }
                                },
                                new TableRow
                                {
                                    Cells = new List<TableCell>
                                    {
                                        new TableCell { Value = "West", Align = "left" },
                                        new TableCell { Value = "$3.8M", Align = "right" },
                                        new TableCell { Value = "$3.2M", Align = "right" },
                                        new TableCell { Value = "19%", Align = "right" }
                                    }
                                },
                                new TableRow
                                {
                                    Cells = new List<TableCell>
                                    {
                                        new TableCell { Value = "South", Align = "left" },
                                        new TableCell { Value = "$2.8M", Align = "right" },
                                        new TableCell { Value = "$2.6M", Align = "right" },
                                        new TableCell { Value = "8%", Align = "right" }
                                    }
                                },
                                new TableRow
                                {
                                    Cells = new List<TableCell>
                                    {
                                        new TableCell { Value = "Midwest", Align = "left" },
                                        new TableCell { Value = "$1.7M", Align = "right" },
                                        new TableCell { Value = "$1.3M", Align = "right" },
                                        new TableCell { Value = "31%", Align = "right" }
                                    }
                                }
                            },
                            Totals = new List<TableCell>
                            {
                                new TableCell { Value = "Total", Align = "left" },
                                new TableCell { Value = "$12.5M", Align = "right" },
                                new TableCell { Value = "$10.2M", Align = "right" },
                                new TableCell { Value = "23%", Align = "right" }
                            }
                        }
                    }
                }
            }
        },

        // Table of Contents
        Sections = new List<TocEntry>
        {
            new TocEntry { Level = 1, Number = "1", Title = "Sales Performance Overview", Page = 3 },
            new TocEntry { Level = 2, Number = "1.1", Title = "Revenue by Region", Page = 3 },
            new TocEntry { Level = 2, Number = "1.2", Title = "Product Performance", Page = 5 },
            new TocEntry { Level = 1, Number = "2", Title = "Market Analysis", Page = 7 },
            new TocEntry { Level = 1, Number = "3", Title = "Strategic Recommendations", Page = 9 },
            new TocEntry { Level = 1, Number = "Appendix A", Title = "Detailed Methodology", Page = 11 }
        }
    }
};

// Generate report
var generator = new ReportGenerator("report-template.html");

using (var output = new FileStream("q4-2025-sales-report.pdf", FileMode.Create))
{
    generator.GenerateReport(reportData, output);
    Console.WriteLine($"Report generated: {output.Name}");
}
```

---

## Try It Yourself

### Exercise 1: Custom Report Design

Create a report for your organization:
- Design a branded cover page
- Add your company's KPIs
- Include relevant charts and tables
- Test with real data

### Exercise 2: Dynamic Charts

Integrate chart generation:
- Use charting library (e.g., Chart.js, D3.js)
- Generate chart images
- Embed in report template
- Add interactive legends

### Exercise 3: Automated Reports

Build an automated system:
- Pull data from database
- Generate charts automatically
- Create report on schedule
- Email to stakeholders

---

## Best Practices

1. **Consistent Branding** - Use company colors and fonts throughout
2. **Clear Hierarchy** - Use heading levels appropriately
3. **Page Breaks** - Start major sections on new pages
4. **Data Visualization** - Use charts for complex data
5. **Executive Summary** - Always include for busy readers
6. **Page Numbers** - Include on all pages except cover
7. **Table of Contents** - Essential for reports over 10 pages
8. **Appendices** - Move detailed data to appendices

---

## Next Steps

1. **[Certificate Template](04_certificate_template.md)** - Awards and certificates
2. **[Data-Driven Report](05_data_driven_report.md)** - Dynamic database reports
3. **[Catalog & Brochure](06_catalog_brochure.md)** - Product catalogs

---

**Continue learning →** [Certificate Template](04_certificate_template.md)
