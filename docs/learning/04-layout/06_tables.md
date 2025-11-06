---
layout: default
title: Table Layouts
nav_order: 6
parent: Layout & Positioning
parent_url: /learning/04-layout/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Table Layouts

Master table-based layouts for structured data presentation and page layout in PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Create responsive table layouts
- Control table sizing (fixed vs auto)
- Style table elements (rows, cells, borders)
- Use tables for page layout (beyond data)
- Create alternating row styles
- Handle table page breaks
- Build complex nested table structures

---

## Tables for Data vs Layout

Tables serve two purposes in PDF documents:

**1. Data Tables** - Display tabular data
```html
<table>
    <thead>
        <tr>
            <th>Name</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Product A</td>
            <td>$99</td>
        </tr>
    </tbody>
</table>
```

**2. Layout Tables** - Structure page layout (using CSS, not `<table>`)
```css
.container {
    display: table;
}

.column {
    display: table-cell;
}
```

---

## Basic Table Structure

### HTML Structure

```html
<table>
    <thead>  <!-- Header rows -->
        <tr>
            <th>Column 1</th>
            <th>Column 2</th>
        </tr>
    </thead>
    <tbody>  <!-- Data rows -->
        <tr>
            <td>Data 1</td>
            <td>Data 2</td>
        </tr>
    </tbody>
    <tfoot>  <!-- Footer rows (optional) -->
        <tr>
            <td>Total</td>
            <td>$100</td>
        </tr>
    </tfoot>
</table>
```

### Basic Styling

```css
table {
    width: 100%;
    border-collapse: collapse;  /* Remove spacing between cells */
}

th, td {
    padding: 10pt;
    text-align: left;
    border: 1pt solid #d1d5db;
}

thead {
    background-color: #2563eb;
    color: white;
}
```

---

## Table Sizing

### table-layout: auto (Default)

Columns size based on content.

```css
table {
    table-layout: auto;  /* Default */
    width: 100%;
}
```

**Characteristics:**
- Flexible column widths
- Columns adjust to content
- Slower rendering for large tables
- Can create uneven columns

### table-layout: fixed

Columns size based on specified widths.

```css
table {
    table-layout: fixed;  /* Faster, predictable */
    width: 100%;
}

col:nth-child(1) { width: 30%; }
col:nth-child(2) { width: 40%; }
col:nth-child(3) { width: 30%; }
```

**Characteristics:**
- Fixed, predictable widths
- Faster rendering
- Content may overflow if too long
- Better for consistent layouts

---

## Column Width Control

### Using `<col>` Elements

```html
<table>
    <colgroup>
        <col style="width: 30%;" />
        <col style="width: 50%;" />
        <col style="width: 20%;" />
    </colgroup>
    <thead>
        <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Product A</td>
            <td>Description text</td>
            <td>$99</td>
        </tr>
    </tbody>
</table>
```

### Using CSS

```css
/* Specific columns */
table col:nth-child(1) { width: 200pt; }
table col:nth-child(2) { width: auto; }  /* Takes remaining */
table col:nth-child(3) { width: 100pt; }

/* Or via cells */
thead th:nth-child(1) { width: 200pt; }
thead th:nth-child(2) { width: auto; }
thead th:nth-child(3) { width: 100pt; }
```

---

## Border Styles

### Collapsed Borders (Recommended)

```css
table {
    border-collapse: collapse;  /* Single borders */
}

th, td {
    border: 1pt solid #d1d5db;
}
```

### Separated Borders

```css
table {
    border-collapse: separate;
    border-spacing: 5pt;  /* Space between cells */
}

th, td {
    border: 1pt solid #d1d5db;
}
```

### No Borders

```css
table {
    border-collapse: collapse;
}

th, td {
    border: none;
    border-bottom: 1pt solid #e5e7eb;  /* Only bottom borders */
}
```

---

## Alternating Row Colors

### Using CSS

```css
/* Even rows */
tbody tr:nth-child(even) {
    background-color: #f9fafb;
}

/* Odd rows */
tbody tr:nth-child(odd) {
    background-color: white;
}
```

### Zebra Striping

```css
tbody tr:nth-child(odd) {
    background-color: #f9fafb;
}

tbody tr:nth-child(even) {
    background-color: white;
}

/* Hover effect not applicable in PDF, but can style specific rows */
tbody tr.highlighted {
    background-color: #fef3c7;
}
```

---

## Table Page Breaks

### Keep Table Together

```css
table {
    page-break-inside: avoid;  /* Don't split table */
}
```

### Allow Breaks, Keep Rows Together

```css
table {
    page-break-inside: auto;  /* Can split */
}

tr {
    page-break-inside: avoid;  /* Don't split rows */
}
```

### Repeat Headers on Each Page

```css
thead {
    display: table-header-group;  /* Repeat on each page */
}

tfoot {
    display: table-footer-group;  /* Repeat on each page */
}
```

---

## Practical Examples

### Example 1: Data Table with Styling

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Sales Report</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            margin: 0;
        }

        h1 {
            color: #1e40af;
            font-size: 24pt;
            margin-top: 0;
            margin-bottom: 30pt;
            padding-bottom: 15pt;
            border-bottom: 2pt solid #2563eb;
        }

        /* ==============================================
           TABLE STYLING
           ============================================== */
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
            table-layout: fixed;
        }

        /* Column widths */
        col:nth-child(1) { width: 25%; }  /* Name */
        col:nth-child(2) { width: 20%; }  /* Region */
        col:nth-child(3) { width: 20%; }  /* Sales */
        col:nth-child(4) { width: 15%; }  /* Growth */
        col:nth-child(5) { width: 20%; }  /* Target */

        /* Header */
        thead {
            background-color: #2563eb;
            color: white;
        }

        th {
            padding: 12pt;
            text-align: left;
            font-weight: bold;
            border: none;
        }

        /* Body */
        td {
            padding: 10pt 12pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        /* Alternating rows */
        tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        /* Footer */
        tfoot {
            background-color: #eff6ff;
            font-weight: bold;
        }

        tfoot td {
            padding: 12pt;
            border-top: 2pt solid #2563eb;
            border-bottom: none;
        }

        /* ==============================================
           SPECIAL CELLS
           ============================================== */
        .number {
            text-align: right;
        }

        .positive {
            color: #059669;
        }

        .negative {
            color: #dc2626;
        }

        .center {
            text-align: center;
        }
    </style>
</head>
<body>
    <h1>Q4 2024 Sales Report</h1>

    <table>
        <colgroup>
            <col />
            <col />
            <col />
            <col />
            <col />
        </colgroup>
        <thead>
            <tr>
                <th>Salesperson</th>
                <th>Region</th>
                <th class="number">Sales</th>
                <th class="number">Growth</th>
                <th class="number">Target</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>John Smith</td>
                <td>Northeast</td>
                <td class="number">$450,000</td>
                <td class="number positive">+15%</td>
                <td class="number">$400,000</td>
            </tr>
            <tr>
                <td>Sarah Johnson</td>
                <td>Southeast</td>
                <td class="number">$380,000</td>
                <td class="number positive">+8%</td>
                <td class="number">$375,000</td>
            </tr>
            <tr>
                <td>Michael Brown</td>
                <td>Midwest</td>
                <td class="number">$290,000</td>
                <td class="number negative">-3%</td>
                <td class="number">$300,000</td>
            </tr>
            <tr>
                <td>Emily Davis</td>
                <td>West</td>
                <td class="number">$520,000</td>
                <td class="number positive">+22%</td>
                <td class="number">$425,000</td>
            </tr>
            <tr>
                <td>David Wilson</td>
                <td>Southwest</td>
                <td class="number">$340,000</td>
                <td class="number positive">+12%</td>
                <td class="number">$325,000</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="2">Total</td>
                <td class="number">$1,980,000</td>
                <td class="number positive">+10.8%</td>
                <td class="number">$1,825,000</td>
            </tr>
        </tfoot>
    </table>

    <p style="margin-top: 30pt; color: #666; font-size: 10pt;">
        <strong>Note:</strong> Growth percentages are compared to Q4 2023.
    </p>
</body>
</html>
```

### Example 2: Nested Table for Layout

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Invoice with Nested Tables</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            margin: 0;
        }

        /* ==============================================
           LAYOUT TABLE (using display: table)
           ============================================== */
        .header-layout {
            display: table;
            width: 100%;
            margin-bottom: 30pt;
        }

        .header-left {
            display: table-cell;
            width: 60%;
            vertical-align: top;
        }

        .header-right {
            display: table-cell;
            width: 40%;
            vertical-align: top;
            text-align: right;
        }

        .company-name {
            font-size: 24pt;
            font-weight: bold;
            color: #1e40af;
            margin-bottom: 10pt;
        }

        .invoice-number {
            font-size: 18pt;
            color: #2563eb;
            margin-bottom: 5pt;
        }

        /* ==============================================
           DATA TABLES
           ============================================== */
        table.data-table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }

        .data-table th {
            background-color: #2563eb;
            color: white;
            padding: 10pt;
            text-align: left;
            font-weight: bold;
        }

        .data-table td {
            padding: 8pt 10pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        .data-table tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        .data-table tfoot {
            background-color: #eff6ff;
            font-weight: bold;
        }

        .data-table tfoot td {
            padding: 10pt;
            border-top: 2pt solid #2563eb;
            border-bottom: none;
        }

        .text-right {
            text-align: right;
        }

        .text-center {
            text-align: center;
        }

        .total-row {
            font-size: 14pt;
            color: #1e40af;
        }
    </style>
</head>
<body>
    <!-- Header with layout table (CSS display: table) -->
    <div class="header-layout">
        <div class="header-left">
            <div class="company-name">Acme Corporation</div>
            <p style="margin: 0;">123 Business Street, Suite 100<br/>
            New York, NY 10001<br/>
            (555) 123-4567</p>
        </div>
        <div class="header-right">
            <div class="invoice-number">Invoice #2025-001</div>
            <p style="margin: 0;">Date: January 15, 2025<br/>
            Due: February 14, 2025</p>
        </div>
    </div>

    <!-- Bill To -->
    <div style="margin-bottom: 30pt;">
        <h2 style="color: #2563eb; font-size: 16pt; margin: 0 0 10pt 0;">Bill To:</h2>
        <p style="margin: 0;"><strong>XYZ Company</strong><br/>
        456 Commerce Ave<br/>
        Boston, MA 02101</p>
    </div>

    <!-- Line Items (data table) -->
    <table class="data-table">
        <thead>
            <tr>
                <th>Description</th>
                <th class="text-center">Qty</th>
                <th class="text-right">Rate</th>
                <th class="text-right">Amount</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Consulting Services</td>
                <td class="text-center">10</td>
                <td class="text-right">$150.00</td>
                <td class="text-right">$1,500.00</td>
            </tr>
            <tr>
                <td>Software Development</td>
                <td class="text-center">20</td>
                <td class="text-right">$200.00</td>
                <td class="text-right">$4,000.00</td>
            </tr>
            <tr>
                <td>Project Management</td>
                <td class="text-center">5</td>
                <td class="text-right">$175.00</td>
                <td class="text-right">$875.00</td>
            </tr>
            <tr>
                <td>Testing & QA</td>
                <td class="text-center">8</td>
                <td class="text-right">$125.00</td>
                <td class="text-right">$1,000.00</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3" class="text-right">Subtotal:</td>
                <td class="text-right">$7,375.00</td>
            </tr>
            <tr>
                <td colspan="3" class="text-right">Tax (8%):</td>
                <td class="text-right">$590.00</td>
            </tr>
            <tr class="total-row">
                <td colspan="3" class="text-right">Total:</td>
                <td class="text-right">$7,965.00</td>
            </tr>
        </tfoot>
    </table>

    <!-- Footer -->
    <div style="margin-top: 40pt; padding-top: 20pt; border-top: 1pt solid #d1d5db; font-size: 9pt; color: #666;">
        <p><strong>Payment Terms:</strong> Payment due within 30 days.</p>
        <p><strong>Note:</strong> Thank you for your business!</p>
    </div>
</body>
</html>
```

### Example 3: Complex Multi-Section Table

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Product Comparison Table</title>
    <style>
        @page {
            size: Letter landscape;  /* Wider page for comparison */
            margin: 0.75in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Helvetica, sans-serif;
            font-size: 10pt;
            margin: 0;
        }

        h1 {
            text-align: center;
            color: #1e40af;
            font-size: 24pt;
            margin: 0 0 30pt 0;
        }

        /* ==============================================
           COMPARISON TABLE
           ============================================== */
        table {
            width: 100%;
            border-collapse: collapse;
            table-layout: fixed;
        }

        /* Column widths */
        col:nth-child(1) { width: 30%; }  /* Feature */
        col:nth-child(2) { width: 17.5%; }  /* Basic */
        col:nth-child(3) { width: 17.5%; }  /* Standard */
        col:nth-child(4) { width: 17.5%; }  /* Pro */
        col:nth-child(5) { width: 17.5%; }  /* Enterprise */

        /* Header */
        thead th {
            padding: 15pt;
            text-align: center;
            font-weight: bold;
            border: 1pt solid #d1d5db;
        }

        thead th:first-child {
            background-color: #f9fafb;
            text-align: left;
        }

        thead th:nth-child(2) { background-color: #e0e7ff; color: #1e40af; }
        thead th:nth-child(3) { background-color: #dbeafe; color: #1e3a8a; }
        thead th:nth-child(4) { background-color: #bfdbfe; color: #1e40af; }
        thead th:nth-child(5) { background-color: #93c5fd; color: #1e3a8a; }

        /* Subheaders */
        .subheader {
            background-color: #1e40af;
            color: white;
            font-weight: bold;
            padding: 8pt 15pt;
        }

        .subheader td {
            border: 1pt solid #1e40af;
        }

        /* Body */
        td {
            padding: 10pt 15pt;
            border: 1pt solid #e5e7eb;
        }

        tbody tr td:first-child {
            background-color: #f9fafb;
            font-weight: 500;
        }

        tbody tr:nth-child(even) td:not(:first-child) {
            background-color: #fafafa;
        }

        /* Checkmarks and X marks */
        .check {
            text-align: center;
            color: #059669;
            font-weight: bold;
            font-size: 14pt;
        }

        .cross {
            text-align: center;
            color: #dc2626;
            font-weight: bold;
            font-size: 14pt;
        }

        .value {
            text-align: center;
            font-weight: bold;
            color: #2563eb;
        }
    </style>
</head>
<body>
    <h1>Product Plans Comparison</h1>

    <table>
        <colgroup>
            <col />
            <col />
            <col />
            <col />
            <col />
        </colgroup>
        <thead>
            <tr>
                <th>Feature</th>
                <th>Basic<br/><span style="font-weight: normal;">$29/mo</span></th>
                <th>Standard<br/><span style="font-weight: normal;">$79/mo</span></th>
                <th>Pro<br/><span style="font-weight: normal;">$149/mo</span></th>
                <th>Enterprise<br/><span style="font-weight: normal;">Custom</span></th>
            </tr>
        </thead>
        <tbody>
            <!-- Storage Section -->
            <tr class="subheader">
                <td colspan="5">Storage & Users</td>
            </tr>
            <tr>
                <td>Users</td>
                <td class="value">5</td>
                <td class="value">15</td>
                <td class="value">50</td>
                <td class="value">Unlimited</td>
            </tr>
            <tr>
                <td>Storage</td>
                <td class="value">10 GB</td>
                <td class="value">100 GB</td>
                <td class="value">500 GB</td>
                <td class="value">Unlimited</td>
            </tr>
            <tr>
                <td>File Upload Limit</td>
                <td class="value">50 MB</td>
                <td class="value">200 MB</td>
                <td class="value">2 GB</td>
                <td class="value">10 GB</td>
            </tr>

            <!-- Features Section -->
            <tr class="subheader">
                <td colspan="5">Core Features</td>
            </tr>
            <tr>
                <td>Document Management</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
            </tr>
            <tr>
                <td>Collaboration Tools</td>
                <td class="cross">✗</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
            </tr>
            <tr>
                <td>Advanced Analytics</td>
                <td class="cross">✗</td>
                <td class="cross">✗</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
            </tr>
            <tr>
                <td>API Access</td>
                <td class="cross">✗</td>
                <td class="cross">✗</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
            </tr>
            <tr>
                <td>Custom Integrations</td>
                <td class="cross">✗</td>
                <td class="cross">✗</td>
                <td class="cross">✗</td>
                <td class="check">✓</td>
            </tr>

            <!-- Support Section -->
            <tr class="subheader">
                <td colspan="5">Support</td>
            </tr>
            <tr>
                <td>Email Support</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
            </tr>
            <tr>
                <td>Priority Support</td>
                <td class="cross">✗</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
            </tr>
            <tr>
                <td>Dedicated Account Manager</td>
                <td class="cross">✗</td>
                <td class="cross">✗</td>
                <td class="cross">✗</td>
                <td class="check">✓</td>
            </tr>
            <tr>
                <td>24/7 Phone Support</td>
                <td class="cross">✗</td>
                <td class="cross">✗</td>
                <td class="check">✓</td>
                <td class="check">✓</td>
            </tr>
        </tbody>
    </table>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Sales Report

Create a sales report table with:
- Alternating row colors
- Right-aligned numbers
- Color-coded values (positive/negative)
- Footer with totals

### Exercise 2: Product Comparison

Create a comparison table with:
- Multiple products as columns
- Features as rows
- Checkmarks for included features
- Subheaders to group features

### Exercise 3: Invoice Layout

Create an invoice using:
- Layout tables (CSS display: table) for header
- Data table for line items
- Calculated totals in footer
- Professional styling

---

## Common Pitfalls

### ❌ Not Setting table-layout

```css
table {
    width: 100%;
    /* Missing table-layout - unpredictable column widths */
}
```

✅ **Solution:** Use table-layout: fixed

```css
table {
    width: 100%;
    table-layout: fixed;  /* Predictable widths */
}
```

### ❌ Forgetting border-collapse

```css
table {
    /* Missing border-collapse - double borders */
}

td {
    border: 1pt solid black;
}
```

✅ **Solution:** Set border-collapse

```css
table {
    border-collapse: collapse;  /* Single borders */
}
```

### ❌ Not Controlling Page Breaks

```css
table {
    /* Long table splits awkwardly */
}
```

✅ **Solution:** Control breaks

```css
table {
    page-break-inside: auto;  /* Can split */
}

tr {
    page-break-inside: avoid;  /* Don't split rows */
}

thead {
    display: table-header-group;  /* Repeat on each page */
}
```

### ❌ Inconsistent Column Widths

```css
/* No widths specified - inconsistent sizing */
th, td {
    padding: 10pt;
}
```

✅ **Solution:** Specify widths

```css
col:nth-child(1) { width: 40%; }
col:nth-child(2) { width: 30%; }
col:nth-child(3) { width: 30%; }
```

### ❌ Mixing Layout and Data Tables

```html
<!-- Confusing structure -->
<table>
    <tr>
        <td>
            <table><!-- Nested table --></table>
        </td>
    </tr>
</table>
```

✅ **Solution:** Use CSS display: table for layout

```css
.layout {
    display: table;
}

.column {
    display: table-cell;
}
```

---

## Table Layout Checklist

- [ ] table-layout specified (fixed or auto)
- [ ] border-collapse set to collapse
- [ ] Column widths defined (percentages or fixed)
- [ ] Page breaks controlled (avoid splitting rows)
- [ ] thead repeats on multiple pages if needed
- [ ] Alternating row colors if appropriate
- [ ] Consistent padding on all cells
- [ ] Proper use of thead, tbody, tfoot

---

## Best Practices

1. **Use table-layout: fixed** - Predictable, faster rendering
2. **Set border-collapse: collapse** - Clean single borders
3. **Define column widths** - Consistent layout across pages
4. **Control page breaks** - Don't split rows
5. **Use semantic HTML** - thead, tbody, tfoot for structure
6. **Repeat headers** - display: table-header-group for long tables
7. **Alternate row colors** - Improves readability
8. **Right-align numbers** - Standard convention for numeric data

---

## Next Steps

Now that you master table layouts:

1. **[Headers & Footers](07_headers_footers.md)** - Repeating page elements
2. **[Layout Best Practices](08_layout_best_practices.md)** - Professional patterns
3. **[Content Components](/learning/06-content/)** - Images, lists, and more

---

**Continue learning →** [Headers & Footers](07_headers_footers.md)
