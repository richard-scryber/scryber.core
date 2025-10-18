---
layout: default
title: Tables - Advanced
nav_order: 6
parent: Content Components
parent_url: /learning/06-content/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Tables - Advanced

Master advanced table techniques including data binding, calculated columns, cell spanning, repeating headers, and page breaks for complex, data-driven tables.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Generate table rows dynamically with data binding
- Create calculated columns with formulas
- Use colspan and rowspan for complex layouts
- Control table page breaks
- Repeat headers on each page
- Build running totals and aggregations
- Create professional data-driven tables

---

## Dynamic Table Rows

### Basic Data Binding

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Dynamic Table</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            margin: 0;
        }

        table {
            width: 100%;
            border-collapse: collapse;
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

        tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }
    </style>
</head>
<body>
    <h1>Employee Directory</h1>

    <table>
        <thead>
            <tr>
                <th>Name</th>
                <th>Department</th>
                <th>Email</th>
                <th>Phone</th>
            </tr>
        </thead>
        <tbody>
            {{#each employees}}
            <tr>
                <td>{{this.name}}</td>
                <td>{{this.department}}</td>
                <td>{{this.email}}</td>
                <td>{{this.phone}}</td>
            </tr>
            {{/each}}
        </tbody>
    </table>
</body>
</html>
```
{% endraw %}

### Accessing Index

{% raw %}
```html
<tbody>
    {{#each items}}
    <tr>
        <td>{{calc(@index, '+', 1)}}</td>  <!-- Row number -->
        <td>{{this.name}}</td>
        <td>{{this.value}}</td>
    </tr>
    {{/each}}
</tbody>
```
{% endraw %}

---

## Calculated Columns

### Simple Calculations

{% raw %}
```html
<table>
    <thead>
        <tr>
            <th>Item</th>
            <th>Quantity</th>
            <th>Price</th>
            <th>Total</th>
        </tr>
    </thead>
    <tbody>
        {{#each items}}
        <tr>
            <td>{{this.name}}</td>
            <td>{{this.quantity}}</td>
            <td>${{this.price}}</td>
            <td>${{calc(this.quantity, '*', this.price)}}</td>
        </tr>
        {{/each}}
    </tbody>
</table>
```
{% endraw %}

### Complex Calculations

{% raw %}
```html
<tbody>
    {{#each orders}}
    <tr>
        <td>{{this.productName}}</td>
        <td>{{this.quantity}}</td>
        <td>${{this.unitPrice}}</td>
        <!-- Subtotal -->
        <td>${{calc(this.quantity, '*', this.unitPrice)}}</td>
        <!-- Tax (8%) -->
        <td>${{calc(this.quantity, '*', this.unitPrice, '*', 0.08)}}</td>
        <!-- Total with tax -->
        <td>${{calc(this.quantity, '*', this.unitPrice, '*', 1.08)}}</td>
    </tr>
    {{/each}}
</tbody>
```
{% endraw %}

---

## Running Totals

### Using Variables for Accumulation

{% raw %}
```html
<!-- Initialize running total -->
<var data-id="runningTotal" data-value="0" />

<table>
    <thead>
        <tr>
            <th>Date</th>
            <th>Transaction</th>
            <th>Amount</th>
            <th>Balance</th>
        </tr>
    </thead>
    <tbody>
        {{#each transactions}}
        <tr>
            <td>{{this.date}}</td>
            <td>{{this.description}}</td>
            <td>${{this.amount}}</td>
            <!-- Update running total -->
            <var data-id="runningTotal"
                 data-value="{{calc(Document.Params.runningTotal, '+', this.amount)}}" />
            <!-- Display current total -->
            <td>${{Document.Params.runningTotal}}</td>
        </tr>
        {{/each}}
    </tbody>
</table>
```
{% endraw %}

---

## Cell Spanning

### Column Span (colspan)

```html
<table>
    <thead>
        <tr>
            <th colspan="3">Q1 Results</th>
        </tr>
        <tr>
            <th>Product</th>
            <th>Units</th>
            <th>Revenue</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Product A</td>
            <td>1,200</td>
            <td>$48,000</td>
        </tr>
        <tr>
            <td>Product B</td>
            <td>850</td>
            <td>$34,000</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td>Total</td>
            <td>2,050</td>
            <td>$82,000</td>
        </tr>
    </tfoot>
</table>
```

### Row Span (rowspan)

```html
<table>
    <tbody>
        <tr>
            <td rowspan="3">Engineering</td>
            <td>John Smith</td>
            <td>Senior Developer</td>
        </tr>
        <tr>
            <!-- Engineering cell spans into this row -->
            <td>Jane Doe</td>
            <td>Developer</td>
        </tr>
        <tr>
            <!-- Engineering cell spans into this row -->
            <td>Bob Johnson</td>
            <td>Junior Developer</td>
        </tr>
        <tr>
            <td rowspan="2">Marketing</td>
            <td>Alice Brown</td>
            <td>Manager</td>
        </tr>
        <tr>
            <td>Charlie Davis</td>
            <td>Coordinator</td>
        </tr>
    </tbody>
</table>
```

---

## Page Breaks in Tables

### Controlling Page Breaks

```css
/* Avoid breaking table */
table {
    page-break-inside: avoid;
}

/* Allow breaks in tbody */
tbody {
    page-break-inside: auto;
}

/* Avoid breaking rows */
tr {
    page-break-inside: avoid;
}

/* Keep header with first row */
thead {
    display: table-header-group;  /* Repeats on each page */
}

/* Keep footer with last row */
tfoot {
    display: table-footer-group;  /* Repeats on each page */
}
```

### New Page Before Table

```css
.new-page-table {
    page-break-before: always;
}
```

```html
<table class="new-page-table">
    <!-- Table content -->
</table>
```

---

## Repeating Headers and Footers

### Repeating on Each Page

```css
thead {
    display: table-header-group;
}

tfoot {
    display: table-footer-group;
}
```

```html
<table>
    <!-- This header repeats on each page -->
    <thead>
        <tr>
            <th>Column 1</th>
            <th>Column 2</th>
            <th>Column 3</th>
        </tr>
    </thead>

    <tbody>
        <!-- Many rows that span multiple pages -->
        <tr><td>...</td><td>...</td><td>...</td></tr>
        <!-- ... hundreds of rows ... -->
    </tbody>

    <!-- This footer repeats on each page -->
    <tfoot>
        <tr>
            <td colspan="3">Continued on next page...</td>
        </tr>
    </tfoot>
</table>
```

---

## Practical Examples

### Example 1: Invoice with Calculations

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Invoice</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 11pt;
            margin: 0;
        }

        h1 {
            font-size: 28pt;
            margin: 0 0 30pt 0;
            color: #1e40af;
        }

        .invoice-info {
            margin-bottom: 30pt;
        }

        .invoice-info p {
            margin: 5pt 0;
        }

        /* ==============================================
           INVOICE TABLE
           ============================================== */
        .invoice-table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }

        .invoice-table thead th {
            background-color: #1e40af;
            color: white;
            padding: 12pt;
            text-align: left;
            font-weight: 600;
        }

        .invoice-table thead th.right {
            text-align: right;
        }

        .invoice-table tbody td {
            padding: 10pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        .invoice-table tbody td.right {
            text-align: right;
            font-family: 'Courier New', monospace;
        }

        .invoice-table tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        /* ==============================================
           TOTALS SECTION
           ============================================== */
        .totals {
            width: 50%;
            margin-left: auto;
            margin-top: 20pt;
        }

        .totals table {
            width: 100%;
            border-collapse: collapse;
        }

        .totals td {
            padding: 8pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        .totals td:first-child {
            font-weight: 600;
            text-align: right;
        }

        .totals td:last-child {
            text-align: right;
            font-family: 'Courier New', monospace;
        }

        .totals .grand-total {
            font-size: 14pt;
            font-weight: 700;
            background-color: #eff6ff;
            border-top: 2pt solid #2563eb;
            border-bottom: 2pt solid #2563eb;
            color: #1e40af;
        }
    </style>
</head>
<body>
    <h1>Invoice #{{invoice.number}}</h1>

    <div class="invoice-info">
        <p><strong>Date:</strong> {{invoice.date}}</p>
        <p><strong>Customer:</strong> {{customer.name}}</p>
        <p><strong>Address:</strong> {{customer.address}}</p>
    </div>

    <!-- Initialize subtotal variable -->
    <var data-id="subtotal" data-value="0" />

    <!-- Line items table -->
    <table class="invoice-table">
        <thead>
            <tr>
                <th>#</th>
                <th>Description</th>
                <th class="right">Qty</th>
                <th class="right">Unit Price</th>
                <th class="right">Amount</th>
            </tr>
        </thead>
        <tbody>
            {{#each invoice.items}}
            <tr>
                <td>{{calc(@index, '+', 1)}}</td>
                <td>{{this.description}}</td>
                <td class="right">{{this.quantity}}</td>
                <td class="right">${{this.unitPrice}}</td>
                <td class="right">${{calc(this.quantity, '*', this.unitPrice)}}</td>
            </tr>
            <!-- Accumulate subtotal -->
            <var data-id="subtotal"
                 data-value="{{calc(Document.Params.subtotal, '+', this.quantity, '*', this.unitPrice)}}" />
            {{/each}}
        </tbody>
    </table>

    <!-- Totals section -->
    <div class="totals">
        <table>
            <tr>
                <td>Subtotal:</td>
                <td>${{Document.Params.subtotal}}</td>
            </tr>
            <tr>
                <td>Tax (8%):</td>
                <td>${{calc(Document.Params.subtotal, '*', 0.08)}}</td>
            </tr>
            <tr>
                <td>Shipping:</td>
                <td>$15.00</td>
            </tr>
            <tr class="grand-total">
                <td>Total:</td>
                <td>${{calc(Document.Params.subtotal, '*', 1.08, '+', 15)}}</td>
            </tr>
        </table>
    </div>
</body>
</html>
```
{% endraw %}

### Example 2: Multi-Page Report with Repeating Headers

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Sales Report</title>
    <style>
        @page {
            size: Letter;
            margin: 0.75in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            font-size: 10pt;
            margin: 0;
        }

        h1 {
            font-size: 20pt;
            margin: 0 0 20pt 0;
            color: #1e40af;
        }

        /* ==============================================
           LARGE DATA TABLE
           ============================================== */
        .data-table {
            width: 100%;
            border-collapse: collapse;
        }

        /* Header repeats on each page */
        .data-table thead {
            display: table-header-group;
        }

        .data-table thead th {
            background-color: #2563eb;
            color: white;
            padding: 8pt;
            text-align: left;
            font-weight: 600;
            font-size: 9pt;
        }

        .data-table thead th.right {
            text-align: right;
        }

        .data-table tbody td {
            padding: 6pt;
            border-bottom: 0.5pt solid #e5e7eb;
            font-size: 9pt;
        }

        .data-table tbody td.right {
            text-align: right;
            font-family: 'Courier New', monospace;
        }

        .data-table tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        /* Prevent row breaks */
        .data-table tbody tr {
            page-break-inside: avoid;
        }

        /* Footer repeats on each page */
        .data-table tfoot {
            display: table-footer-group;
        }

        .data-table tfoot td {
            padding: 8pt;
            font-weight: 700;
            background-color: #eff6ff;
            border-top: 2pt solid #2563eb;
        }
    </style>
</head>
<body>
    <h1>Annual Sales Report - {{year}}</h1>

    <table class="data-table">
        <!-- This header will repeat on every page -->
        <thead>
            <tr>
                <th>Order ID</th>
                <th>Date</th>
                <th>Customer</th>
                <th>Product</th>
                <th class="right">Quantity</th>
                <th class="right">Unit Price</th>
                <th class="right">Total</th>
            </tr>
        </thead>

        <tbody>
            {{#each orders}}
            <tr>
                <td>{{this.orderId}}</td>
                <td>{{this.date}}</td>
                <td>{{this.customerName}}</td>
                <td>{{this.productName}}</td>
                <td class="right">{{this.quantity}}</td>
                <td class="right">${{this.unitPrice}}</td>
                <td class="right">${{calc(this.quantity, '*', this.unitPrice)}}</td>
            </tr>
            {{/each}}
        </tbody>

        <!-- This footer will repeat on every page -->
        <tfoot>
            <tr>
                <td colspan="6">Grand Total:</td>
                <td class="right">${{totals.grandTotal}}</td>
            </tr>
        </tfoot>
    </table>
</body>
</html>
```
{% endraw %}

---

## Try It Yourself

### Exercise 1: Order Summary

Create an order summary table with:
- Dynamic rows from data
- Quantity × Price calculations
- Running subtotal column
- Tax and total at bottom

### Exercise 2: Grouped Data Table

Build a table with:
- Row spanning for group headers
- Multiple items per group
- Subtotals per group
- Grand total at end

### Exercise 3: Multi-Page Inventory Report

Design an inventory report with:
- 100+ items (test pagination)
- Repeating headers on each page
- Calculated "Total Value" column (Qty × Price)
- Footer showing page totals

---

## Common Pitfalls

### ❌ Not Using table-header-group

```css
/* Headers won't repeat on pages */
thead {
    display: block;  /* Wrong! */
}
```

✅ **Solution:**

```css
thead {
    display: table-header-group;  /* Repeats on pages */
}
```

### ❌ Breaking Rows Across Pages

```css
/* Rows may split awkwardly */
tr {
    page-break-inside: auto;
}
```

✅ **Solution:**

```css
tr {
    page-break-inside: avoid;  /* Keep rows together */
}
```

### ❌ Incorrect Calculation Syntax

{% raw %}
```html
<!-- Wrong: Missing operators -->
<td>{{calc(this.a this.b)}}</td>

<!-- Wrong: Too many nested calcs -->
<td>{{calc(calc(calc(this.a, '+', this.b), '*', 2), '-', 10)}}</td>
```
{% endraw %}

✅ **Solution:**

{% raw %}
```html
<!-- Correct: All operators specified -->
<td>{{calc(this.a, '+', this.b)}}</td>

<!-- Better: Break into steps with variables -->
<var data-id="sum" data-value="{{calc(this.a, '+', this.b)}}" />
<var data-id="doubled" data-value="{{calc(Document.Params.sum, '*', 2)}}" />
<td>{{calc(Document.Params.doubled, '-', 10)}}</td>
```
{% endraw %}

---

## Advanced Tables Checklist

- [ ] Data binding working correctly
- [ ] Calculations tested with various values
- [ ] Running totals accumulate properly
- [ ] Page breaks controlled (thead repeats)
- [ ] Rows don't break across pages
- [ ] Column spanning correct
- [ ] Row spanning correct
- [ ] Large datasets tested (pagination)

---

## Best Practices

1. **Test with Real Data** - Varying dataset sizes
2. **Use table-header-group** - For repeating headers
3. **Avoid Row Breaks** - page-break-inside: avoid on tr
4. **Validate Calculations** - Test with known values
5. **Initialize Variables** - Set to 0 before loops
6. **Clear Spanning** - Ensure colspan/rowspan makes sense
7. **Performance** - Limit calculations in large tables
8. **Debugging** - Test with small datasets first

---

## Next Steps

1. **[Attachments & Embedded Content](07_attachments_embedded.md)** - File embedding
2. **[Content Best Practices](08_content_best_practices.md)** - Optimization
3. **[Practical Applications](/learning/08-practical/)** - Real-world examples

---

**Continue learning →** [Attachments & Embedded Content](07_attachments_embedded.md)
