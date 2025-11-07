---
layout: default
title: Advanced Patterns
nav_order: 8
parent: Data Binding & Expressions
parent_url: /learning/02-data-binding/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Advanced Patterns

Master complex data binding scenarios, dynamic layouts, and reusable template patterns.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Build dynamic multi-section documents
- Create reusable template components
- Handle complex nested data structures
- Implement conditional layouts
- Use data-driven page breaks
- Build master-detail reports
- Create grouped and aggregated displays

---

## Pattern 1: Dynamic Multi-Section Documents

Create documents with variable numbers of sections based on data.

**C# Code:**
```csharp
doc.Params["model"] = new
{
    documentTitle = "Comprehensive Report",
    sections = new[]
    {
        new
        {
            sectionType = "summary",
            title = "Executive Summary",
            content = "Key findings and recommendations...",
            showChart = false
        },
        new
        {
            sectionType = "detailed",
            title = "Detailed Analysis",
            content = "In-depth analysis of the data...",
            showChart = true,
            chartData = new[] { 10, 20, 30, 40 }
        },
        new
        {
            sectionType = "table",
            title = "Financial Data",
            content = "",
            showChart = false,
            tableData = new[]
            {
                new { category = "Revenue", amount = 1000000 },
                new { category = "Expenses", amount = 750000 }
            }
        }
    }
};
```

**Template:**
{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>{{model.documentTitle}}</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }
        .section {
            page-break-before: always;
            margin-bottom: 30pt;
        }
        .section:first-child {
            page-break-before: auto;
        }
        h2 {
            color: #2563eb;
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 10pt;
        }
        table {
            width: 100%;
            border-collapse: collapse;
        }
        th {
            background-color: #2563eb;
            color: white;
            padding: 10pt;
        }
        td {
            padding: 8pt;
            border-bottom: 1pt solid #e5e7eb;
        }
    </style>
</head>
<body>
    <h1>{{model.documentTitle}}</h1>

    {{#each model.sections}}
        <div class="section">
            <h2>{{this.title}}</h2>

            {{#if this.sectionType == 'summary'}}
                <div class="summary">
                    <p>{{this.content}}</p>
                </div>
            {{else if this.sectionType == 'detailed'}}
                <div class="detailed">
                    <p>{{this.content}}</p>
                    {{#if this.showChart}}
                        <div class="chart-placeholder">
                            [Chart would be rendered here]
                        </div>
                    {{/if}}
                </div>
            {{else if this.sectionType == 'table'}}
                <table>
                    <thead>
                        <tr>
                            <th>Category</th>
                            <th style="text-align: right;">Amount</th>
                        </tr>
                    </thead>
                    <tbody>
                        {{#each this.tableData}}
                        <tr>
                            <td>{{this.category}}</td>
                            <td style="text-align: right;">{{format(this.amount, 'C0')}}</td>
                        </tr>
                        {{/each}}
                    </tbody>
                </table>
            {{/if}}
        </div>
    {{/each}}
</body>
</html>
```
{% endraw %}

---

## Pattern 2: Master-Detail Reports

Display hierarchical data with master records and their associated details.

**C# Code:**
```csharp
doc.Params["model"] = new
{
    reportTitle = "Customer Orders Report",
    reportDate = DateTime.Now,
    customers = new[]
    {
        new
        {
            customerId = "CUST-001",
            customerName = "Acme Corporation",
            email = "contact@acme.com",
            phone = "555-1234",
            totalOrders = 3,
            totalSpent = 15000.00,
            orders = new[]
            {
                new
                {
                    orderNumber = "ORD-001",
                    orderDate = new DateTime(2025, 1, 10),
                    status = "Delivered",
                    items = new[]
                    {
                        new { product = "Widget A", quantity = 10, price = 100.00, total = 1000.00 },
                        new { product = "Widget B", quantity = 5, price = 200.00, total = 1000.00 }
                    },
                    orderTotal = 2000.00
                },
                new
                {
                    orderNumber = "ORD-002",
                    orderDate = new DateTime(2025, 1, 15),
                    status = "Processing",
                    items = new[]
                    {
                        new { product = "Widget C", quantity = 20, price = 150.00, total = 3000.00 }
                    },
                    orderTotal = 3000.00
                }
            }
        },
        new
        {
            customerId = "CUST-002",
            customerName = "Global Industries",
            email = "orders@global.com",
            phone = "555-5678",
            totalOrders = 1,
            totalSpent = 5000.00,
            orders = new[]
            {
                new
                {
                    orderNumber = "ORD-003",
                    orderDate = new DateTime(2025, 1, 12),
                    status = "Shipped",
                    items = new[]
                    {
                        new { product = "Widget D", quantity = 25, price = 200.00, total = 5000.00 }
                    },
                    orderTotal = 5000.00
                }
            }
        }
    }
};
```

**Template:**
{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Customer Orders Report</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }
        h1 {
            color: #1e40af;
        }
        .customer-section {
            page-break-inside: avoid;
            margin-bottom: 40pt;
            border: 2pt solid #d1d5db;
            border-radius: 5pt;
            padding: 20pt;
        }
        .customer-header {
            background-color: #eff6ff;
            margin: -20pt -20pt 20pt -20pt;
            padding: 15pt 20pt;
            border-bottom: 2pt solid #2563eb;
        }
        .customer-name {
            font-size: 18pt;
            font-weight: bold;
            color: #1e40af;
        }
        .customer-id {
            font-size: 10pt;
            color: #666;
        }
        .order {
            margin: 20pt 0;
            padding: 15pt;
            background-color: #f9fafb;
            border-left: 4pt solid #3b82f6;
        }
        .order-header {
            display: table;
            width: 100%;
            margin-bottom: 10pt;
        }
        .order-number {
            display: table-cell;
            font-weight: bold;
            font-size: 14pt;
        }
        .order-status {
            display: table-cell;
            text-align: right;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 10pt 0;
        }
        th {
            background-color: #2563eb;
            color: white;
            padding: 8pt;
            text-align: left;
            font-size: 10pt;
        }
        td {
            padding: 6pt 8pt;
            border-bottom: 1pt solid #e5e7eb;
            font-size: 10pt;
        }
        .summary-box {
            background-color: #dbeafe;
            padding: 10pt;
            margin-top: 10pt;
            border-radius: 3pt;
        }
    </style>
</head>
<body>
    <h1>{{model.reportTitle}}</h1>
    <p style="color: #666;">Generated: {{format(model.reportDate, 'MMMM dd, yyyy HH:mm')}}</p>

    {{#each model.customers}}
        <div class="customer-section">
            <!-- Customer Header -->
            <div class="customer-header">
                <div class="customer-name">{{this.customerName}}</div>
                <div class="customer-id">{{this.customerId}}</div>
                <div style="font-size: 10pt; margin-top: 5pt;">
                    {{this.email}} | {{this.phone}}
                </div>
            </div>

            <!-- Customer Summary -->
            <div class="summary-box">
                <strong>Customer Summary:</strong>
                {{this.totalOrders}} orders | Total Spent: {{format(this.totalSpent, 'C0')}}
            </div>

            <!-- Orders -->
            <h3>Orders</h3>
            {{#each this.orders}}
                <div class="order">
                    <div class="order-header">
                        <div class="order-number">{{this.orderNumber}}</div>
                        <div class="order-status">
                            Status:
                            <span style="color: {{if(this.status == 'Delivered', '#059669',
                                                     if(this.status == 'Shipped', '#f59e0b',
                                                     '#2563eb'))}};">
                                {{this.status}}
                            </span>
                        </div>
                    </div>

                    <p style="font-size: 10pt; color: #666;">
                        Order Date: {{format(this.orderDate, 'MMM dd, yyyy')}}
                    </p>

                    <!-- Order Items -->
                    <table>
                        <thead>
                            <tr>
                                <th>Product</th>
                                <th style="text-align: center;">Qty</th>
                                <th style="text-align: right;">Price</th>
                                <th style="text-align: right;">Total</th>
                            </tr>
                        </thead>
                        <tbody>
                            {{#each this.items}}
                            <tr>
                                <td>{{this.product}}</td>
                                <td style="text-align: center;">{{this.quantity}}</td>
                                <td style="text-align: right;">{{format(this.price, 'C2')}}</td>
                                <td style="text-align: right;">{{format(this.total, 'C2')}}</td>
                            </tr>
                            {{/each}}
                        </tbody>
                        <tfoot>
                            <tr style="background-color: white; font-weight: bold;">
                                <td colspan="3" style="text-align: right;">Order Total:</td>
                                <td style="text-align: right;">{{format(this.orderTotal, 'C2')}}</td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            {{/each}}
        </div>
    {{/each}}
</body>
</html>
```
{% endraw %}

---

## Pattern 3: Grouped Data with Subtotals

Group data by category and calculate subtotals.

**C# Code:**
```csharp
var salesData = new[]
{
    new { region = "North", salesperson = "Alice", amount = 50000 },
    new { region = "North", salesperson = "Bob", amount = 45000 },
    new { region = "South", salesperson = "Charlie", amount = 60000 },
    new { region = "South", salesperson = "Diana", amount = 55000 },
    new { region = "East", salesperson = "Eve", amount = 70000 },
    new { region = "East", salesperson = "Frank", amount = 65000 }
};

// Group by region
var grouped = salesData
    .GroupBy(s => s.region)
    .Select(g => new
    {
        region = g.Key,
        sales = g.Select(s => new { salesperson = s.salesperson, amount = s.amount }).ToArray(),
        subtotal = g.Sum(s => s.amount),
        salespeople = g.Count()
    })
    .ToArray();

doc.Params["model"] = new
{
    title = "Sales by Region",
    reportDate = DateTime.Now,
    regions = grouped,
    grandTotal = salesData.Sum(s => s.amount)
};
```

**Template:**
{% raw %}
```html
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
        .region {
            margin-bottom: 30pt;
        }
        .region-header {
            background-color: #2563eb;
            color: white;
            padding: 10pt;
            font-size: 16pt;
            font-weight: bold;
        }
        table {
            width: 100%;
            border-collapse: collapse;
        }
        td {
            padding: 8pt;
            border-bottom: 1pt solid #e5e7eb;
        }
        .subtotal-row {
            background-color: #eff6ff;
            font-weight: bold;
        }
        .grandtotal-row {
            background-color: #1e40af;
            color: white;
            font-size: 16pt;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <h1>{{model.title}}</h1>
    <p>Report Date: {{format(model.reportDate, 'MMMM dd, yyyy')}}</p>

    {{#each model.regions}}
        <div class="region">
            <div class="region-header">
                {{this.region}} Region ({{this.salespeople}} salespeople)
            </div>

            <table>
                {{#each this.sales}}
                <tr>
                    <td>{{this.salesperson}}</td>
                    <td style="text-align: right;">{{format(this.amount, 'C0')}}</td>
                </tr>
                {{/each}}
                <tr class="subtotal-row">
                    <td>{{../region}} Subtotal</td>
                    <td style="text-align: right;">{{format(this.subtotal, 'C0')}}</td>
                </tr>
            </table>
        </div>
    {{/each}}

    <!-- Grand Total -->
    <table>
        <tr class="grandtotal-row">
            <td>GRAND TOTAL</td>
            <td style="text-align: right;">{{format(model.grandTotal, 'C0')}}</td>
        </tr>
    </table>
</body>
</html>
```
{% endraw %}

---

## Pattern 4: Conditional Page Breaks

Control page breaks based on data.

{% raw %}
```html
{{#each model.items}}
    <div class="item" style="{{if(@index > 0 && calc(@index, '%', 5) == 0, 'page-break-before: always;', '')}}">
        <!-- Content -->
        <h3>{{this.title}}</h3>
        <p>{{this.content}}</p>
    </div>
{{/each}}
```
{% endraw %}

Or with explicit conditions:

{% raw %}
```html
{{#each model.sections}}
    {{#if this.startNewPage}}
        <div style="page-break-before: always;"></div>
    {{/if}}

    <div class="section">
        <h2>{{this.title}}</h2>
        <p>{{this.content}}</p>
    </div>
{{/each}}
```
{% endraw %}

---

## Pattern 5: Dynamic Column Layouts

Create responsive multi-column layouts based on data count.

**C# Code:**
```csharp
doc.Params["model"] = new
{
    products = productList,
    columnCount = productList.Length <= 3 ? 1 : (productList.Length <= 6 ? 2 : 3)
};
```

**Template:**
{% raw %}
```html
<div style="display: table; width: 100%;">
    {{#each model.products}}
        <div style="display: table-cell; width: {{calc(100, '/', ../columnCount)}}%; padding: 10pt; vertical-align: top;">
            <div style="border: 1pt solid #d1d5db; padding: 15pt;">
                <h3>{{this.name}}</h3>
                <p>{{format(this.price, 'C2')}}</p>
            </div>
        </div>

        {{#if(calc(add(@index, 1), '%', ../columnCount))}}
            <!-- Continue on same row -->
        {{else}}
            </div><div style="display: table; width: 100%;">
        {{/if}}
    {{/each}}
</div>
```
{% endraw %}

---

## Pattern 6: Reusable Template Components

Create reusable styling configurations.

**C# Code:**
```csharp
// Shared styling configuration
var brandStyle = new
{
    colors = new
    {
        primary = "#2563eb",
        secondary = "#3b82f6",
        success = "#059669",
        warning = "#f59e0b",
        danger = "#dc2626"
    },
    fonts = new
    {
        heading = "24pt",
        subheading = "18pt",
        body = "11pt"
    }
};

// Apply to multiple documents
doc.Params["brand"] = brandStyle;
doc.Params["data"] = actualData;
```

**Template:**
{% raw %}
```html
<style>
    h1 {
        font-size: {{brand.fonts.heading}};
        color: {{brand.colors.primary}};
    }
    h2 {
        font-size: {{brand.fonts.subheading}};
        color: {{brand.colors.secondary}};
    }
    .success {
        color: {{brand.colors.success}};
    }
    .warning {
        color: {{brand.colors.warning}};
    }
</style>
```
{% endraw %}

---

## Pattern 7: Data-Driven Conditional Styles

Apply styles based on data values.

{% raw %}
```html
{{#each model.items}}
    <div style="padding: 10pt;
                background-color: {{if(this.priority == 'high', '#fee2e2',
                                      if(this.priority == 'medium', '#fef3c7',
                                      '#f3f4f6'))}};
                border-left: 4pt solid {{if(this.priority == 'high', '#dc2626',
                                           if(this.priority == 'medium', '#f59e0b',
                                           '#9ca3af'))}};">
        <h3>{{this.title}}</h3>
        <p>Priority: {{upper(this.priority)}}</p>
    </div>
{{/each}}
```
{% endraw %}

---

## Pattern 8: Pagination with Index

Display items with page-aware numbering.

{% raw %}
```html
<table>
    <thead>
        <tr>
            <th>#</th>
            <th>Name</th>
            <th>Details</th>
        </tr>
    </thead>
    <tbody>
        {{#each model.items}}
        <tr style="{{if(calc(@index, '%', 20) == 0 && @index > 0, 'page-break-before: always;', '')}}">
            <td>{{add(@index, 1)}}</td>
            <td>{{this.name}}</td>
            <td>{{this.details}}</td>
        </tr>
        {{/each}}
    </tbody>
</table>
```
{% endraw %}

---

## Try It Yourself

### Exercise 1: Multi-Level Grouping

Create a report that groups data by:
- Country → Region → City → Store
- Calculate subtotals at each level
- Show grand total at the end

### Exercise 2: Dynamic Dashboard

Create a dashboard that:
- Shows different widgets based on user role
- Displays different chart types based on data type
- Adjusts layout based on widget count
- Includes conditional page breaks

### Exercise 3: Complex Invoice

Create an invoice with:
- Multiple shipping addresses (if applicable)
- Line items with conditional discounts
- Volume-based pricing tiers
- Dynamic payment terms based on customer type
- Conditional late fees display

---

## Common Pitfalls

### ❌ Deeply Nested Context Issues

{% raw %}
```html
{{#each level1}}
    {{#each this.level2}}
        {{#each this.level3}}
            {{#each this.level4}}
                <!-- Accessing parent data becomes error-prone -->
                <p>{{../../../level1Property}}</p>
            {{/each}}
        {{/each}}
    {{/each}}
{{/each}}
```
{% endraw %}

✅ **Solution:** Restructure data or use root parameters

```csharp
// Option 1: Flatten data structure
// Option 2: Pass needed values at each level
doc.Params["companyName"] = "Tech Corp";  // Accessible from anywhere
```

### ❌ Complex Inline Conditionals

{% raw %}
```html
{{if(model.a && model.b || model.c && !model.d, if(model.e > 10, 'value1', 'value2'), 'value3')}}
```
{% endraw %}

✅ **Solution:** Calculate in C#

```csharp
var displayValue = DetermineDisplayValue(model);
doc.Params["displayValue"] = displayValue;
```

### ❌ Repeating Calculations

{% raw %}
```html
{{#each items}}
    <p>Total: {{format(calc(this.price, '*', this.quantity, '+', calc(this.price, '*', this.quantity, '*', 0.08)), 'C2')}}</p>
{{/each}}
```
{% endraw %}

✅ **Solution:** Pre-calculate in C#

```csharp
var items = rawItems.Select(item => new
{
    item.price,
    item.quantity,
    subtotal = item.price * item.quantity,
    tax = item.price * item.quantity * 0.08,
    total = (item.price * item.quantity) * 1.08
}).ToArray();
```

---

## Next Steps

Now that you've mastered advanced patterns:

1. **[Common Mistakes](09_common_mistakes.md)** - Avoid common errors
2. **[Styling PDFs](/learning/03-styling/)** - Advanced styling techniques
3. **[Practical Applications](/learning/08-practical/)** - Real-world examples

---

**Continue learning →** [Common Mistakes](09_common_mistakes.md)
