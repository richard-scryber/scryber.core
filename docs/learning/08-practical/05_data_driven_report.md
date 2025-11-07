---
layout: default
title: Data-Driven Report
nav_order: 5
parent: Practical Applications
parent_url: /learning/08-practical/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Data-Driven Report

Generate dynamic PDF reports from databases, APIs, and external data sources with real-time data binding and calculations.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Load data from databases
- Fetch data from REST APIs
- Transform data for PDF generation
- Create dynamic charts and graphs
- Implement conditional sections
- Calculate aggregates and KPIs
- Handle large datasets efficiently

---

## Complete Data-Driven Report System

### HTML Template

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

        body {
            font-family: 'Helvetica', 'Arial', sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            color: #333;
        }

        /* Header */
        .report-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 15pt;
            margin-bottom: 30pt;
        }

        .report-header h1 {
            font-size: 20pt;
            color: #2563eb;
            margin: 0;
        }

        .report-meta {
            text-align: right;
            font-size: 10pt;
            color: #666;
        }

        /* KPI Dashboard */
        .kpi-dashboard {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 15pt;
            margin-bottom: 30pt;
        }

        .kpi-card {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 15pt;
            border-radius: 8pt;
            text-align: center;
        }

        .kpi-value {
            font-size: 24pt;
            font-weight: bold;
            margin: 5pt 0;
        }

        .kpi-label {
            font-size: 9pt;
            text-transform: uppercase;
            opacity: 0.9;
        }

        .kpi-change {
            font-size: 10pt;
            margin-top: 5pt;
        }

        .kpi-change.positive::before {
            content: '▲ ';
        }

        .kpi-change.negative::before {
            content: '▼ ';
        }

        /* Data tables */
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

        .data-table td.center {
            text-align: center;
        }

        .data-table tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        .data-table tfoot {
            background-color: #eff6ff;
            font-weight: bold;
        }

        /* Status badges */
        .status-badge {
            display: inline-block;
            padding: 3pt 8pt;
            border-radius: 3pt;
            font-size: 9pt;
            font-weight: bold;
        }

        .status-active {
            background-color: #d1fae5;
            color: #065f46;
        }

        .status-pending {
            background-color: #fef3c7;
            color: #92400e;
        }

        .status-completed {
            background-color: #dbeafe;
            color: #1e40af;
        }

        .status-cancelled {
            background-color: #fee2e2;
            color: #991b1b;
        }

        /* Charts */
        .chart-section {
            margin: 30pt 0;
            page-break-inside: avoid;
        }

        .chart-title {
            font-size: 14pt;
            font-weight: bold;
            color: #2563eb;
            margin-bottom: 15pt;
        }

        .chart-container {
            text-align: center;
        }

        .chart-image {
            max-width: 100%;
            height: auto;
        }

        /* Conditional sections */
        .alert {
            background-color: #fef2f2;
            border-left: 4pt solid #dc2626;
            padding: 15pt;
            margin: 20pt 0;
        }

        .warning {
            background-color: #fffbeb;
            border-left: 4pt solid #f59e0b;
            padding: 15pt;
            margin: 20pt 0;
        }

        .info {
            background-color: #eff6ff;
            border-left: 4pt solid #2563eb;
            padding: 15pt;
            margin: 20pt 0;
        }

        /* Section headers */
        h2 {
            font-size: 16pt;
            color: #2563eb;
            border-bottom: 1pt solid #2563eb;
            padding-bottom: 8pt;
            margin-top: 30pt;
            margin-bottom: 15pt;
        }

        h3 {
            font-size: 13pt;
            color: #1e40af;
            margin-top: 20pt;
            margin-bottom: 10pt;
        }

        /* Footer */
        .report-footer {
            position: absolute;
            bottom: 40pt;
            left: 72pt;
            right: 72pt;
            display: flex;
            justify-content: space-between;
            font-size: 9pt;
            color: #666;
            border-top: 1pt solid #e5e7eb;
            padding-top: 10pt;
        }
    </style>
</head>
<body>
    <!-- Header -->
    <div class="report-header">
        <div>
            <h1>{{report.title}}</h1>
            <p style="margin: 5pt 0 0 0; color: #666;">{{report.subtitle}}</p>
        </div>
        <div class="report-meta">
            <p>Generated: {{report.generatedDate}}</p>
            <p>Period: {{report.period}}</p>
            <p>Report ID: {{report.id}}</p>
        </div>
    </div>

    <!-- KPI Dashboard -->
    <div class="kpi-dashboard">
        {{#each kpis}}
        <div class="kpi-card">
            <div class="kpi-label">{{this.label}}</div>
            <div class="kpi-value">{{this.value}}</div>
            <div class="kpi-change {{this.changeClass}}">{{this.change}}</div>
        </div>
        {{/each}}
    </div>

    <!-- Alerts (conditional) -->
    {{#if report.hasAlerts}}
    <div class="alert">
        <h3 style="margin-top: 0; color: #dc2626;">⚠ Alerts Requiring Attention</h3>
        <ul style="margin: 10pt 0;">
            {{#each report.alerts}}
            <li>{{this}}</li>
            {{/each}}
        </ul>
    </div>
    {{/if}}

    <!-- Sales by Region -->
    <h2>Sales Performance by Region</h2>

    <table class="data-table">
        <thead>
            <tr>
                <th>Region</th>
                <th class="right">Sales</th>
                <th class="right">Target</th>
                <th class="right">Achievement</th>
                <th class="right">Growth</th>
                <th class="center">Status</th>
            </tr>
        </thead>
        <tbody>
            {{#each salesByRegion}}
            <tr>
                <td><strong>{{this.region}}</strong></td>
                <td class="right">${{format(this.sales, '#,##0')}}</td>
                <td class="right">${{format(this.target, '#,##0')}}</td>
                <td class="right">{{format(this.achievement, '0.0')}}%</td>
                <td class="right">{{this.growth}}%</td>
                <td class="center">
                    <span class="status-badge status-{{this.statusClass}}">
                        {{this.status}}
                    </span>
                </td>
            </tr>
            {{/each}}
        </tbody>
        <tfoot>
            <tr>
                <td>Total</td>
                <td class="right">${{format(totals.sales, '#,##0')}}</td>
                <td class="right">${{format(totals.target, '#,##0')}}</td>
                <td class="right">{{format(totals.achievement, '0.0')}}%</td>
                <td class="right">{{totals.growth}}%</td>
                <td class="center">-</td>
            </tr>
        </tfoot>
    </table>

    <!-- Chart -->
    {{#if charts.salesTrend}}
    <div class="chart-section">
        <div class="chart-title">Sales Trend (Last 12 Months)</div>
        <div class="chart-container">
            <img src="{{charts.salesTrend}}" class="chart-image" />
        </div>
    </div>
    {{/if}}

    <!-- Top Products -->
    <h2>Top 10 Products</h2>

    <table class="data-table">
        <thead>
            <tr>
                <th>Rank</th>
                <th>Product</th>
                <th>Category</th>
                <th class="right">Units Sold</th>
                <th class="right">Revenue</th>
                <th class="right">Margin</th>
            </tr>
        </thead>
        <tbody>
            {{#each topProducts}}
            <tr>
                <td class="center"><strong>{{this.rank}}</strong></td>
                <td>{{this.name}}</td>
                <td>{{this.category}}</td>
                <td class="right">{{format(this.unitsSold, '#,##0')}}</td>
                <td class="right">${{format(this.revenue, '#,##0')}}</td>
                <td class="right">{{format(this.margin, '0.0')}}%</td>
            </tr>
            {{/each}}
        </tbody>
    </table>

    <!-- Customer Segments -->
    <h2>Customer Segment Analysis</h2>

    {{#each customerSegments}}
    <h3>{{this.segment}}</h3>
    <p>{{this.description}}</p>

    <div style="display: grid; grid-template-columns: repeat(3, 1fr); gap: 15pt; margin: 15pt 0;">
        <div style="background: #f9fafb; padding: 15pt; border-left: 3pt solid #2563eb;">
            <div style="font-size: 9pt; color: #666; text-transform: uppercase;">Customers</div>
            <div style="font-size: 18pt; font-weight: bold; color: #2563eb;">{{this.customerCount}}</div>
        </div>
        <div style="background: #f9fafb; padding: 15pt; border-left: 3pt solid #2563eb;">
            <div style="font-size: 9pt; color: #666; text-transform: uppercase;">Avg Order Value</div>
            <div style="font-size: 18pt; font-weight: bold; color: #2563eb;">${{format(this.avgOrderValue, '#,##0')}}</div>
        </div>
        <div style="background: #f9fafb; padding: 15pt; border-left: 3pt solid #2563eb;">
            <div style="font-size: 9pt; color: #666; text-transform: uppercase;">Lifetime Value</div>
            <div style="font-size: 18pt; font-weight: bold; color: #2563eb;">${{format(this.lifetimeValue, '#,##0')}}</div>
        </div>
    </div>
    {{/each}}

    <!-- Recommendations (conditional) -->
    {{#if recommendations}}
    <h2>Strategic Recommendations</h2>

    {{#each recommendations}}
    <div class="info">
        <h4 style="margin-top: 0; color: #2563eb;">{{this.title}}</h4>
        <p style="margin: 10pt 0;">{{this.description}}</p>
        <p style="margin: 5pt 0; font-size: 10pt; color: #666;">
            <strong>Expected Impact:</strong> {{this.impact}}
        </p>
    </div>
    {{/each}}
    {{/if}}

    <!-- Footer -->
    <div class="report-footer">
        <span>{{company.name}} | Confidential</span>
        <span>Page <page-number /> of <page-count /></span>
        <span>Generated: {{report.generatedDate}}</span>
    </div>
</body>
</html>
```
{% endraw %}

---

## C# Data-Driven Report Generator

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class DataDrivenReportGenerator
{
    private readonly string _templatePath;
    private readonly string _connectionString;
    private readonly HttpClient _httpClient;

    public DataDrivenReportGenerator(
        string templatePath,
        string connectionString = null)
    {
        _templatePath = templatePath;
        _connectionString = connectionString;
        _httpClient = new HttpClient();
    }

    public async Task<Stream> GenerateReportAsync(ReportConfiguration config)
    {
        // Collect data from multiple sources
        var reportData = new ReportData
        {
            Company = config.Company,
            Report = new Report
            {
                Title = config.Title,
                Subtitle = config.Subtitle,
                Period = config.Period,
                GeneratedDate = DateTime.Now.ToString("MMMM dd, yyyy HH:mm"),
                Id = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()
            }
        };

        // Load data from database
        if (!string.IsNullOrEmpty(_connectionString))
        {
            await LoadDatabaseDataAsync(reportData, config);
        }

        // Load data from API
        if (!string.IsNullOrEmpty(config.ApiEndpoint))
        {
            await LoadApiDataAsync(reportData, config);
        }

        // Calculate aggregates and KPIs
        CalculateKPIs(reportData);
        CalculateTotals(reportData);
        GenerateRecommendations(reportData);

        // Generate charts (if needed)
        if (config.GenerateCharts)
        {
            reportData.Charts = await GenerateChartsAsync(reportData);
        }

        // Generate PDF
        return GeneratePDF(reportData);
    }

    private async Task LoadDatabaseDataAsync(
        ReportData reportData,
        ReportConfiguration config)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Load sales by region
            reportData.SalesByRegion = await LoadSalesByRegionAsync(
                connection,
                config.StartDate,
                config.EndDate);

            // Load top products
            reportData.TopProducts = await LoadTopProductsAsync(
                connection,
                config.StartDate,
                config.EndDate,
                limit: 10);

            // Load customer segments
            reportData.CustomerSegments = await LoadCustomerSegmentsAsync(
                connection,
                config.StartDate,
                config.EndDate);
        }
    }

    private async Task<List<RegionalSales>> LoadSalesByRegionAsync(
        SqlConnection connection,
        DateTime startDate,
        DateTime endDate)
    {
        var query = @"
            SELECT
                r.RegionName as Region,
                SUM(o.TotalAmount) as Sales,
                r.Target,
                COUNT(DISTINCT o.CustomerId) as CustomerCount
            FROM Orders o
            JOIN Regions r ON o.RegionId = r.RegionId
            WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
            GROUP BY r.RegionName, r.Target
            ORDER BY Sales DESC";

        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

            var results = new List<RegionalSales>();

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var sales = reader.GetDecimal(1);
                    var target = reader.GetDecimal(2);
                    var achievement = (sales / target) * 100;

                    results.Add(new RegionalSales
                    {
                        Region = reader.GetString(0),
                        Sales = sales,
                        Target = target,
                        Achievement = achievement,
                        Growth = CalculateGrowth(sales, target),
                        Status = achievement >= 100 ? "Exceeded" :
                                achievement >= 90 ? "On Track" :
                                achievement >= 75 ? "At Risk" : "Behind",
                        StatusClass = achievement >= 100 ? "completed" :
                                     achievement >= 90 ? "active" :
                                     achievement >= 75 ? "pending" : "cancelled"
                    });
                }
            }

            return results;
        }
    }

    private async Task<List<ProductPerformance>> LoadTopProductsAsync(
        SqlConnection connection,
        DateTime startDate,
        DateTime endDate,
        int limit)
    {
        var query = @"
            SELECT TOP (@Limit)
                p.ProductName as Name,
                p.Category,
                SUM(oi.Quantity) as UnitsSold,
                SUM(oi.Quantity * oi.UnitPrice) as Revenue,
                AVG((oi.UnitPrice - oi.Cost) / oi.UnitPrice * 100) as Margin
            FROM OrderItems oi
            JOIN Products p ON oi.ProductId = p.ProductId
            JOIN Orders o ON oi.OrderId = o.OrderId
            WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
            GROUP BY p.ProductName, p.Category
            ORDER BY Revenue DESC";

        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Limit", limit);
            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

            var results = new List<ProductPerformance>();
            int rank = 1;

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    results.Add(new ProductPerformance
                    {
                        Rank = rank++,
                        Name = reader.GetString(0),
                        Category = reader.GetString(1),
                        UnitsSold = reader.GetInt32(2),
                        Revenue = reader.GetDecimal(3),
                        Margin = reader.GetDecimal(4)
                    });
                }
            }

            return results;
        }
    }

    private async Task<List<CustomerSegment>> LoadCustomerSegmentsAsync(
        SqlConnection connection,
        DateTime startDate,
        DateTime endDate)
    {
        var query = @"
            SELECT
                cs.SegmentName as Segment,
                cs.Description,
                COUNT(DISTINCT c.CustomerId) as CustomerCount,
                AVG(o.TotalAmount) as AvgOrderValue,
                SUM(o.TotalAmount) / COUNT(DISTINCT c.CustomerId) as LifetimeValue
            FROM Customers c
            JOIN CustomerSegments cs ON c.SegmentId = cs.SegmentId
            LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
            WHERE o.OrderDate BETWEEN @StartDate AND @EndDate
            GROUP BY cs.SegmentName, cs.Description
            ORDER BY LifetimeValue DESC";

        using (var command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

            var results = new List<CustomerSegment>();

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    results.Add(new CustomerSegment
                    {
                        Segment = reader.GetString(0),
                        Description = reader.GetString(1),
                        CustomerCount = reader.GetInt32(2),
                        AvgOrderValue = reader.GetDecimal(3),
                        LifetimeValue = reader.GetDecimal(4)
                    });
                }
            }

            return results;
        }
    }

    private async Task LoadApiDataAsync(
        ReportData reportData,
        ReportConfiguration config)
    {
        try
        {
            var response = await _httpClient.GetAsync(config.ApiEndpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var apiData = JsonSerializer.Deserialize<ApiDataResponse>(json);

            // Merge API data with report data
            if (apiData != null)
            {
                // Example: Add external metrics
                reportData.Report.Alerts = apiData.Alerts;
                reportData.Report.HasAlerts = apiData.Alerts?.Any() == true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Failed to load API data: {ex.Message}");
        }
    }

    private void CalculateKPIs(ReportData reportData)
    {
        var totalSales = reportData.SalesByRegion?.Sum(r => r.Sales) ?? 0;
        var totalTarget = reportData.SalesByRegion?.Sum(r => r.Target) ?? 0;
        var achievement = totalTarget > 0 ? (totalSales / totalTarget * 100) : 0;

        reportData.Kpis = new List<Kpi>
        {
            new Kpi
            {
                Label = "Total Revenue",
                Value = $"${totalSales:N0}",
                Change = "↑ 23.5%",
                ChangeClass = "positive"
            },
            new Kpi
            {
                Label = "Target Achievement",
                Value = $"{achievement:F1}%",
                Change = achievement >= 100 ? "↑ Target Met" : "↓ Below Target",
                ChangeClass = achievement >= 100 ? "positive" : "negative"
            },
            new Kpi
            {
                Label = "Total Orders",
                Value = reportData.TopProducts?.Sum(p => p.UnitsSold).ToString("N0") ?? "0",
                Change = "↑ 18.2%",
                ChangeClass = "positive"
            },
            new Kpi
            {
                Label = "Avg Margin",
                Value = $"{reportData.TopProducts?.Average(p => p.Margin):F1}%",
                Change = "↑ 2.3%",
                ChangeClass = "positive"
            }
        };
    }

    private void CalculateTotals(ReportData reportData)
    {
        if (reportData.SalesByRegion?.Any() == true)
        {
            reportData.Totals = new Totals
            {
                Sales = reportData.SalesByRegion.Sum(r => r.Sales),
                Target = reportData.SalesByRegion.Sum(r => r.Target),
                Achievement = 0,
                Growth = reportData.SalesByRegion.Average(r => r.Growth)
            };

            reportData.Totals.Achievement =
                (reportData.Totals.Sales / reportData.Totals.Target) * 100;
        }
    }

    private void GenerateRecommendations(ReportData reportData)
    {
        var recommendations = new List<Recommendation>();

        // Analyze performance and generate recommendations
        if (reportData.Totals?.Achievement < 90)
        {
            recommendations.Add(new Recommendation
            {
                Title = "Focus on Underperforming Regions",
                Description = "Northeast and West regions are below target. Consider increasing marketing spend and sales support in these areas.",
                Impact = "Potential 15% increase in Q4 revenue"
            });
        }

        var topMarginProduct = reportData.TopProducts?
            .OrderByDescending(p => p.Margin)
            .FirstOrDefault();

        if (topMarginProduct != null)
        {
            recommendations.Add(new Recommendation
            {
                Title = $"Promote High-Margin Products",
                Description = $"'{topMarginProduct.Name}' has the highest margin at {topMarginProduct.Margin:F1}%. Increase promotional efforts for similar products.",
                Impact = "Potential 8% improvement in overall margins"
            });
        }

        reportData.Recommendations = recommendations;
    }

    private async Task<Charts> GenerateChartsAsync(ReportData reportData)
    {
        // In a real implementation, you would:
        // 1. Use a charting library (e.g., ScottPlot, OxyPlot)
        // 2. Generate chart images
        // 3. Save to temp files
        // 4. Return file paths

        return new Charts
        {
            SalesTrend = "./charts/sales-trend.png",
            RegionalBreakdown = "./charts/regional-breakdown.png"
        };
    }

    private Stream GeneratePDF(ReportData reportData)
    {
        var doc = Document.ParseDocument(_templatePath);

        // Set metadata
        doc.Info.Title = reportData.Report.Title;
        doc.Info.Author = reportData.Company.Name;
        doc.Info.CreationDate = DateTime.Now;

        // Bind data
        doc.Params["company"] = reportData.Company;
        doc.Params["report"] = reportData.Report;
        doc.Params["kpis"] = reportData.Kpis;
        doc.Params["salesByRegion"] = reportData.SalesByRegion;
        doc.Params["topProducts"] = reportData.TopProducts;
        doc.Params["customerSegments"] = reportData.CustomerSegments;
        doc.Params["totals"] = reportData.Totals;
        doc.Params["recommendations"] = reportData.Recommendations;
        doc.Params["charts"] = reportData.Charts;

        // Configure for performance
        doc.RenderOptions.Compression = PDFCompressionType.FlateDecode;
        doc.RenderOptions.FontSubsetting = true;

        // Generate
        var output = new MemoryStream();
        doc.SaveAsPDF(output);
        output.Position = 0;

        return output;
    }

    private decimal CalculateGrowth(decimal current, decimal previous)
    {
        if (previous == 0) return 0;
        return ((current - previous) / previous) * 100;
    }
}

// Data models (continued in next section)
```

---

## Data Models

```csharp
public class ReportConfiguration
{
    public CompanyInfo Company { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ApiEndpoint { get; set; }
    public bool GenerateCharts { get; set; } = true;
}

public class ReportData
{
    public CompanyInfo Company { get; set; }
    public Report Report { get; set; }
    public List<Kpi> Kpis { get; set; } = new List<Kpi>();
    public List<RegionalSales> SalesByRegion { get; set; } = new List<RegionalSales>();
    public List<ProductPerformance> TopProducts { get; set; } = new List<ProductPerformance>();
    public List<CustomerSegment> CustomerSegments { get; set; } = new List<CustomerSegment>();
    public Totals Totals { get; set; }
    public List<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
    public Charts Charts { get; set; }
}

public class CompanyInfo
{
    public string Name { get; set; }
}

public class Report
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Period { get; set; }
    public string GeneratedDate { get; set; }
    public string Id { get; set; }
    public bool HasAlerts { get; set; }
    public List<string> Alerts { get; set; }
}

public class Kpi
{
    public string Label { get; set; }
    public string Value { get; set; }
    public string Change { get; set; }
    public string ChangeClass { get; set; }
}

public class RegionalSales
{
    public string Region { get; set; }
    public decimal Sales { get; set; }
    public decimal Target { get; set; }
    public decimal Achievement { get; set; }
    public decimal Growth { get; set; }
    public string Status { get; set; }
    public string StatusClass { get; set; }
}

public class ProductPerformance
{
    public int Rank { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public int UnitsSold { get; set; }
    public decimal Revenue { get; set; }
    public decimal Margin { get; set; }
}

public class CustomerSegment
{
    public string Segment { get; set; }
    public string Description { get; set; }
    public int CustomerCount { get; set; }
    public decimal AvgOrderValue { get; set; }
    public decimal LifetimeValue { get; set; }
}

public class Totals
{
    public decimal Sales { get; set; }
    public decimal Target { get; set; }
    public decimal Achievement { get; set; }
    public decimal Growth { get; set; }
}

public class Recommendation
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Impact { get; set; }
}

public class Charts
{
    public string SalesTrend { get; set; }
    public string RegionalBreakdown { get; set; }
}

public class ApiDataResponse
{
    public List<string> Alerts { get; set; }
}
```

---

## Usage Example

```csharp
// Configure report
var config = new ReportConfiguration
{
    Company = new CompanyInfo { Name = "Acme Corporation" },
    Title = "Q4 2025 Sales Performance Report",
    Subtitle = "Comprehensive Analysis and Insights",
    Period = "October - December 2025",
    StartDate = new DateTime(2025, 10, 1),
    EndDate = new DateTime(2025, 12, 31),
    ApiEndpoint = "https://api.acme.com/alerts",
    GenerateCharts = true
};

// Generate report
var generator = new DataDrivenReportGenerator(
    "data-driven-report-template.html",
    "Server=localhost;Database=SalesDB;Integrated Security=true;"
);

using (var reportStream = await generator.GenerateReportAsync(config))
{
    using (var fileStream = new FileStream("q4-sales-report.pdf", FileMode.Create))
    {
        await reportStream.CopyToAsync(fileStream);
        Console.WriteLine("Data-driven report generated successfully!");
    }
}
```

---

## Try It Yourself

### Exercise 1: Custom Data Source

Connect to your own database:
- Write SQL queries for your data
- Transform data for PDF format
- Calculate relevant KPIs
- Test with real data

### Exercise 2: API Integration

Integrate with REST APIs:
- Fetch data from external services
- Handle API errors gracefully
- Cache API responses
- Combine multiple data sources

### Exercise 3: Advanced Analytics

Add analytical features:
- Trend analysis
- Forecasting
- Anomaly detection
- Comparative analysis

---

## Best Practices

1. **Async Operations** - Use async/await for database and API calls
2. **Error Handling** - Handle connection failures gracefully
3. **Data Validation** - Validate data before PDF generation
4. **Performance** - Use efficient queries and caching
5. **Security** - Use parameterized queries, never concatenate SQL
6. **Scalability** - Process large datasets in batches
7. **Monitoring** - Log query performance and errors
8. **Testing** - Test with various data volumes

---

## Next Steps

1. **[Catalog & Brochure](06_catalog_brochure.md)** - Product catalogs
2. **[Form Template](07_form_template.md)** - Interactive forms
3. **[Multi-Language & Branded Documents](08_multi_language_branded.md)** - Enterprise templates

---

**Continue learning →** [Catalog & Brochure](06_catalog_brochure.md)
