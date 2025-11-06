---
layout: default
title: Formatting Output
nav_order: 7
parent: Data Binding & Expressions
parent_url: /learning/02-data-binding/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Formatting Output

Format dates, numbers, currencies, and text for professional, localized PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Format dates with custom patterns
- Format numbers with precision control
- Display currencies properly
- Format percentages
- Apply text transformations
- Use locale-specific formatting
- Create custom format patterns

---

## The format() Function

The `format()` function applies .NET format strings to values:

{% raw %}
```html
{{format(value, 'formatString')}}
```
{% endraw %}

---

## Date Formatting

### Common Date Formats

{% raw %}
```html
<!-- Short date -->
{{format(model.date, 'yyyy-MM-dd')}}
<!-- Output: 2025-01-15 -->

<!-- Long date -->
{{format(model.date, 'MMMM dd, yyyy')}}
<!-- Output: January 15, 2025 -->

<!-- US format -->
{{format(model.date, 'MM/dd/yyyy')}}
<!-- Output: 01/15/2025 -->

<!-- European format -->
{{format(model.date, 'dd/MM/yyyy')}}
<!-- Output: 15/01/2025 -->

<!-- Full date and time -->
{{format(model.date, 'yyyy-MM-dd HH:mm:ss')}}
<!-- Output: 2025-01-15 14:30:45 -->
```
{% endraw %}

### Date Format Specifiers

| Specifier | Description | Example |
|-----------|-------------|---------|
| `yyyy` | Year (4 digits) | 2025 |
| `yy` | Year (2 digits) | 25 |
| `MMMM` | Month (full name) | January |
| `MMM` | Month (abbreviated) | Jan |
| `MM` | Month (2 digits) | 01 |
| `M` | Month (1-2 digits) | 1 |
| `dd` | Day (2 digits) | 05 |
| `d` | Day (1-2 digits) | 5 |
| `dddd` | Day of week (full) | Monday |
| `ddd` | Day of week (abbr) | Mon |
| `HH` | Hour (24-hour) | 14 |
| `hh` | Hour (12-hour) | 02 |
| `mm` | Minutes | 30 |
| `ss` | Seconds | 45 |
| `tt` | AM/PM | PM |

### Custom Date Formats

{% raw %}
```html
<!-- Verbose format -->
<p>{{format(model.date, 'dddd, MMMM dd, yyyy')}}</p>
<!-- Output: Monday, January 15, 2025 -->

<!-- Time with AM/PM -->
<p>{{format(model.date, 'h:mm tt')}}</p>
<!-- Output: 2:30 PM -->

<!-- ISO 8601 format -->
<p>{{format(model.date, 'yyyy-MM-ddTHH:mm:ss')}}</p>
<!-- Output: 2025-01-15T14:30:45 -->

<!-- Custom separator -->
<p>{{format(model.date, 'MM-dd-yyyy')}}</p>
<!-- Output: 01-15-2025 -->
```
{% endraw %}

---

## Number Formatting

### Fixed Decimal Places

{% raw %}
```html
<!-- 2 decimal places -->
{{format(model.value, 'F2')}}
<!-- Input: 123.456 → Output: 123.46 -->

<!-- No decimal places -->
{{format(model.value, 'F0')}}
<!-- Input: 123.456 → Output: 123 -->

<!-- 4 decimal places -->
{{format(model.value, 'F4')}}
<!-- Input: 123.456 → Output: 123.4560 -->
```
{% endraw %}

### Number with Thousands Separator

{% raw %}
```html
<!-- With separator, no decimals -->
{{format(model.population, 'N0')}}
<!-- Input: 1234567 → Output: 1,234,567 -->

<!-- With separator, 2 decimals -->
{{format(model.amount, 'N2')}}
<!-- Input: 1234.5 → Output: 1,234.50 -->
```
{% endraw %}

### General Number Format

{% raw %}
```html
<!-- General format (removes trailing zeros) -->
{{format(model.value, 'G')}}
<!-- Input: 123.4500 → Output: 123.45 -->
```
{% endraw %}

---

## Currency Formatting

### Basic Currency

{% raw %}
```html
<!-- Default currency format -->
{{format(model.price, 'C')}}
<!-- Output: $1,234.56 (US locale) -->

<!-- Currency with no decimals -->
{{format(model.price, 'C0')}}
<!-- Output: $1,235 -->

<!-- Currency with specific decimals -->
{{format(model.price, 'C3')}}
<!-- Output: $1,234.560 -->
```
{% endraw %}

### Manual Currency Formatting

{% raw %}
```html
<!-- Custom currency symbol -->
<p>€{{format(model.price, 'F2')}}</p>
<!-- Output: €1,234.56 -->

<!-- Currency with text -->
<p>Price: ${{format(model.price, 'N2')}} USD</p>
<!-- Output: Price: $1,234.56 USD -->
```
{% endraw %}

---

## Percentage Formatting

{% raw %}
```html
<!-- Basic percentage (multiplies by 100) -->
{{format(model.rate, 'P')}}
<!-- Input: 0.125 → Output: 12.50% -->

<!-- Percentage, no decimals -->
{{format(model.rate, 'P0')}}
<!-- Input: 0.125 → Output: 13% -->

<!-- Percentage with 1 decimal -->
{{format(model.rate, 'P1')}}
<!-- Input: 0.125 → Output: 12.5% -->
```
{% endraw %}

**Important:** The `P` format multiplies the value by 100. If your value is already a percentage (e.g., 12.5), don't use `P`:

{% raw %}
```html
<!-- Value is already percentage -->
<p>{{model.percentage}}%</p>
<!-- or -->
<p>{{format(model.percentage, 'F1')}}%</p>
```
{% endraw %}

---

## Text Formatting

### String Functions

{% raw %}
```html
<!-- Uppercase -->
{{upper(model.text)}}
<!-- Input: "hello world" → Output: "HELLO WORLD" -->

<!-- Lowercase -->
{{lower(model.text)}}
<!-- Input: "HELLO WORLD" → Output: "hello world" -->

<!-- Trim whitespace -->
{{trim(model.text)}}
<!-- Input: "  hello  " → Output: "hello" -->

<!-- Substring -->
{{substring(model.text, 0, 50)}}
<!-- First 50 characters -->
```
{% endraw %}

### Combining Formatting

{% raw %}
```html
<!-- Uppercase first letter -->
<p>{{concat(upper(substring(model.name, 0, 1)), substring(model.name, 1))}}</p>
<!-- Input: "john" → Output: "John" -->
```
{% endraw %}

---

## Practical Examples

### Example 1: Financial Report

**C# Code:**
```csharp
doc.Params["model"] = new
{
    companyName = "Tech Corp",
    reportDate = DateTime.Now,
    quarter = "Q4 2024",
    metrics = new
    {
        revenue = 5250000.00,
        expenses = 3890000.00,
        profit = 1360000.00,
        profitMargin = 0.2590,
        yearOverYearGrowth = 0.185,
        employeeCount = 450
    },
    regionalData = new[]
    {
        new { region = "North America", revenue = 2100000.00, growth = 0.15 },
        new { region = "Europe", revenue = 1800000.00, growth = 0.22 },
        new { region = "Asia Pacific", revenue = 1350000.00, growth = 0.19 }
    }
};
```

**Template:**
{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Financial Report</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }
        h1 {
            color: #1e40af;
        }
        .header-info {
            font-size: 10pt;
            color: #666;
            margin-bottom: 30pt;
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
        .metric-label {
            font-size: 10pt;
            color: #666;
            margin-top: 5pt;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 30pt 0;
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
        .positive {
            color: #059669;
        }
        .number-cell {
            text-align: right;
        }
    </style>
</head>
<body>
    <h1>{{model.companyName}} Financial Report</h1>

    <div class="header-info">
        <p><strong>Period:</strong> {{model.quarter}}</p>
        <p><strong>Report Generated:</strong> {{format(model.reportDate, 'MMMM dd, yyyy')}}</p>
        <p><strong>Generated at:</strong> {{format(model.reportDate, 'h:mm tt')}}</p>
    </div>

    <h2>Key Metrics</h2>

    <div>
        <div class="metric-box">
            <div class="metric-value">{{format(model.metrics.revenue, 'C0')}}</div>
            <div class="metric-label">Total Revenue</div>
        </div>
        <div class="metric-box">
            <div class="metric-value">{{format(model.metrics.profit, 'C0')}}</div>
            <div class="metric-label">Net Profit</div>
        </div>
        <div class="metric-box">
            <div class="metric-value">{{format(model.metrics.profitMargin, 'P1')}}</div>
            <div class="metric-label">Profit Margin</div>
        </div>
    </div>

    <div style="margin: 20pt 0;">
        <p>
            <strong>YoY Growth:</strong>
            <span class="positive">+{{format(model.metrics.yearOverYearGrowth, 'P1')}}</span>
        </p>
        <p>
            <strong>Employee Count:</strong>
            {{format(model.metrics.employeeCount, 'N0')}} employees
        </p>
    </div>

    <h2>Regional Performance</h2>

    <table>
        <thead>
            <tr>
                <th>Region</th>
                <th class="number-cell">Revenue</th>
                <th class="number-cell">% of Total</th>
                <th class="number-cell">Growth</th>
            </tr>
        </thead>
        <tbody>
            {{#each model.regionalData}}
            <tr>
                <td>{{this.region}}</td>
                <td class="number-cell">{{format(this.revenue, 'C0')}}</td>
                <td class="number-cell">
                    {{format(calc(this.revenue, '/', ../metrics.revenue), 'P1')}}
                </td>
                <td class="number-cell positive">+{{format(this.growth, 'P0')}}</td>
            </tr>
            {{/each}}
        </tbody>
        <tfoot>
            <tr style="background-color: #eff6ff; font-weight: bold;">
                <td>TOTAL</td>
                <td class="number-cell">{{format(model.metrics.revenue, 'C0')}}</td>
                <td class="number-cell">100%</td>
                <td class="number-cell"></td>
            </tr>
        </tfoot>
    </table>

    <div style="margin-top: 40pt; padding: 15pt; background-color: #f0fdf4; border-left: 4pt solid #059669;">
        <p style="margin: 0;">
            <strong>Summary:</strong>
            {{model.companyName}} generated {{format(model.metrics.revenue, 'C0')}} in revenue
            during {{model.quarter}}, with a profit margin of {{format(model.metrics.profitMargin, 'P1')}}.
            Year-over-year growth was {{format(model.metrics.yearOverYearGrowth, 'P1')}}.
        </p>
    </div>
</body>
</html>
```
{% endraw %}

### Example 2: Product Catalog with Pricing

**C# Code:**
```csharp
doc.Params["model"] = new
{
    catalogTitle = "Spring 2025 Product Catalog",
    publishDate = DateTime.Now,
    categories = new[]
    {
        new
        {
            name = "Electronics",
            products = new[]
            {
                new
                {
                    name = "Laptop Pro 15",
                    sku = "ELEC-LAP-001",
                    price = 1299.99,
                    originalPrice = 1499.99,
                    discount = 0.133,
                    inStock = true,
                    stockQuantity = 45,
                    releaseDate = new DateTime(2024, 9, 15)
                },
                new
                {
                    name = "Wireless Headphones",
                    sku = "ELEC-AUD-002",
                    price = 199.99,
                    originalPrice = 199.99,
                    discount = 0.0,
                    inStock = true,
                    stockQuantity = 120,
                    releaseDate = new DateTime(2024, 11, 1)
                }
            }
        },
        new
        {
            name = "Home & Garden",
            products = new[]
            {
                new
                {
                    name = "Smart Thermostat",
                    sku = "HOME-THM-001",
                    price = 149.99,
                    originalPrice = 179.99,
                    discount = 0.167,
                    inStock = true,
                    stockQuantity = 8,
                    releaseDate = new DateTime(2024, 8, 20)
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
    <title>Product Catalog</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }
        h1 {
            color: #1e40af;
            text-align: center;
        }
        .publish-date {
            text-align: center;
            color: #666;
            font-size: 10pt;
            margin-bottom: 40pt;
        }
        .category {
            margin-bottom: 40pt;
        }
        h2 {
            background-color: #2563eb;
            color: white;
            padding: 10pt;
        }
        .product {
            padding: 15pt;
            margin-bottom: 20pt;
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
            page-break-inside: avoid;
        }
        .product-name {
            font-size: 16pt;
            font-weight: bold;
            color: #1e40af;
        }
        .sku {
            font-size: 9pt;
            color: #666;
        }
        .price-section {
            margin: 10pt 0;
        }
        .current-price {
            font-size: 24pt;
            font-weight: bold;
            color: #059669;
        }
        .original-price {
            font-size: 14pt;
            color: #666;
            text-decoration: line-through;
        }
        .discount-badge {
            display: inline-block;
            background-color: #dc2626;
            color: white;
            padding: 5pt 10pt;
            border-radius: 3pt;
            font-weight: bold;
            margin-left: 10pt;
        }
        .stock-info {
            font-size: 10pt;
            margin-top: 10pt;
        }
        .in-stock {
            color: #059669;
        }
        .low-stock {
            color: #f59e0b;
        }
    </style>
</head>
<body>
    <h1>{{upper(model.catalogTitle)}}</h1>

    <div class="publish-date">
        Published: {{format(model.publishDate, 'MMMM dd, yyyy')}}
    </div>

    {{#each model.categories}}
        <div class="category">
            <h2>{{this.name}}</h2>

            {{#each this.products}}
                <div class="product">
                    <div class="product-name">{{this.name}}</div>
                    <div class="sku">SKU: {{this.sku}}</div>

                    <div class="price-section">
                        <span class="current-price">{{format(this.price, 'C2')}}</span>

                        {{#if this.discount > 0}}
                            <span class="original-price">{{format(this.originalPrice, 'C2')}}</span>
                            <span class="discount-badge">
                                SAVE {{format(this.discount, 'P0')}}
                            </span>
                        {{/if}}
                    </div>

                    <div class="stock-info">
                        {{#if this.inStock}}
                            {{#if this.stockQuantity <= 10}}
                                <span class="low-stock">
                                    ⚠ Low Stock: Only {{this.stockQuantity}} remaining
                                </span>
                            {{else}}
                                <span class="in-stock">
                                    ✓ In Stock ({{format(this.stockQuantity, 'N0')}} available)
                                </span>
                            {{/if}}
                        {{else}}
                            <span style="color: #dc2626;">✗ Out of Stock</span>
                        {{/if}}
                    </div>

                    <div style="font-size: 9pt; color: #666; margin-top: 10pt;">
                        Released: {{format(this.releaseDate, 'MMM yyyy')}}
                    </div>
                </div>
            {{/each}}
        </div>
    {{/each}}

    <div style="margin-top: 40pt; padding: 15pt; background-color: #f9fafb; text-align: center; font-size: 9pt;">
        <p>Prices shown in USD. Subject to change without notice.</p>
        <p>Catalog generated: {{format(model.publishDate, 'yyyy-MM-dd HH:mm:ss')}}</p>
    </div>
</body>
</html>
```
{% endraw %}

---

## Format String Reference

### Standard Numeric Formats

| Format | Description | Example Input | Example Output |
|--------|-------------|---------------|----------------|
| `C` or `C2` | Currency | 1234.56 | $1,234.56 |
| `C0` | Currency, no decimals | 1234.56 | $1,235 |
| `F2` | Fixed-point | 1234.56 | 1234.56 |
| `F0` | Fixed-point, no decimals | 1234.56 | 1235 |
| `N2` | Number with commas | 1234.56 | 1,234.56 |
| `N0` | Number, no decimals | 1234.56 | 1,235 |
| `P` or `P2` | Percentage | 0.1234 | 12.34% |
| `P0` | Percentage, no decimals | 0.1234 | 12% |
| `G` | General | 1234.5000 | 1234.5 |

### Standard Date Formats

| Format | Example Output |
|--------|----------------|
| `d` | 1/15/2025 |
| `D` | Monday, January 15, 2025 |
| `t` | 2:30 PM |
| `T` | 2:30:45 PM |
| `f` | Monday, January 15, 2025 2:30 PM |
| `F` | Monday, January 15, 2025 2:30:45 PM |
| `g` | 1/15/2025 2:30 PM |
| `G` | 1/15/2025 2:30:45 PM |
| `M` or `m` | January 15 |
| `Y` or `y` | January 2025 |

---

## Try It Yourself

### Exercise 1: Sales Report

Create a template that displays:
- Multiple sales amounts (use `C2` format)
- Sales percentages (use `P1` format)
- Sales volume numbers (use `N0` format)
- Report generation date (use long format)

### Exercise 2: Scientific Data

Create a template showing:
- Measurements with exact precision (F4)
- Large numbers with separators (N0)
- Percentages with 2 decimal places
- Timestamps with seconds

### Exercise 3: International Price List

Create a template that:
- Shows prices in different formats
- Displays dates in multiple formats (US, European, ISO)
- Formats large numbers appropriately
- Shows percentages for discounts

---

## Common Pitfalls

### ❌ Wrong Percentage Format

{% raw %}
```html
<!-- If value is already 12.5% (not 0.125) -->
{{format(model.percentage, 'P0')}}  <!-- Shows 1250%! -->
```
{% endraw %}

✅ **Solution:** Check if value needs multiplication

{% raw %}
```html
{{model.percentage}}%  <!-- If already percentage -->
{{format(model.decimal, 'P0')}}  <!-- If decimal (0.125) -->
```
{% endraw %}

### ❌ Inconsistent Decimal Places

{% raw %}
```html
<td>{{format(item1.price, 'F2')}}</td>  <!-- 123.45 -->
<td>{{format(item2.price, 'F3')}}</td>  <!-- 123.456 -->
```
{% endraw %}

✅ **Solution:** Use consistent formatting

{% raw %}
```html
<td>{{format(item1.price, 'F2')}}</td>  <!-- 123.45 -->
<td>{{format(item2.price, 'F2')}}</td>  <!-- 123.46 -->
```
{% endraw %}

### ❌ Missing Format on Numbers

{% raw %}
```html
<p>Price: ${{model.price}}</p>  <!-- Might show: $123.456789 -->
```
{% endraw %}

✅ **Solution:** Always format currency

{% raw %}
```html
<p>Price: {{format(model.price, 'C2')}}</p>  <!-- $123.46 -->
```
{% endraw %}

---

## Next Steps

Now that you can format output:

1. **[Advanced Patterns](08_advanced_patterns.md)** - Complex data binding scenarios
2. **[Common Mistakes](09_common_mistakes.md)** - Avoid common errors
3. **[Styling PDFs](/learning/03-styling/)** - Apply advanced styling

---

**Continue learning →** [Advanced Patterns](08_advanced_patterns.md)
