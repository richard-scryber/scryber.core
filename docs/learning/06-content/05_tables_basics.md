---
layout: default
title: Tables - Basics
nav_order: 5
parent: Content Components
parent_url: /learning/06-content/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Tables - Basics

Master table structure, styling, borders, column widths, and cell alignment to create professional tabular data displays in PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Create well-structured HTML tables
- Style table borders and spacing
- Control column widths
- Apply cell padding and alignment
- Use table headers and footers
- Create alternating row colors
- Build professional data tables

---

## Table Structure

### Basic Table

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Basic Table</title>
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
    </style>
</head>
<body>
    <h1>Employee List</h1>

    <table>
        <thead>
            <tr>
                <th>Name</th>
                <th>Department</th>
                <th>Email</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>John Smith</td>
                <td>Engineering</td>
                <td>john@example.com</td>
            </tr>
            <tr>
                <td>Jane Doe</td>
                <td>Marketing</td>
                <td>jane@example.com</td>
            </tr>
        </tbody>
    </table>
</body>
</html>
```

### Table Parts

```html
<table>
    <!-- Table Header (repeats on each page) -->
    <thead>
        <tr>
            <th>Column 1</th>
            <th>Column 2</th>
        </tr>
    </thead>

    <!-- Table Body (main content) -->
    <tbody>
        <tr>
            <td>Data 1</td>
            <td>Data 2</td>
        </tr>
        <tr>
            <td>Data 3</td>
            <td>Data 4</td>
        </tr>
    </tbody>

    <!-- Table Footer (repeats on each page) -->
    <tfoot>
        <tr>
            <td>Total</td>
            <td>Sum</td>
        </tr>
    </tfoot>
</table>
```

---

## Border Styles

### border-collapse Property

```css
/* Collapsed borders (recommended) */
table.collapsed {
    border-collapse: collapse;  /* Single border between cells */
}

/* Separated borders */
table.separated {
    border-collapse: separate;  /* Space between cells */
    border-spacing: 5pt;
}
```

### Common Border Patterns

```css
/* ==============================================
   FULL BORDERS
   ============================================== */
.full-borders {
    border-collapse: collapse;
}

.full-borders th,
.full-borders td {
    border: 1pt solid #d1d5db;
    padding: 8pt;
}

/* ==============================================
   HORIZONTAL LINES ONLY
   ============================================== */
.horizontal-lines {
    border-collapse: collapse;
}

.horizontal-lines th,
.horizontal-lines td {
    border-bottom: 1pt solid #e5e7eb;
    padding: 8pt;
}

/* ==============================================
   OUTER BORDER ONLY
   ============================================== */
.outer-border {
    border: 2pt solid #2563eb;
    border-collapse: collapse;
}

.outer-border th,
.outer-border td {
    padding: 8pt;
}

/* ==============================================
   NO BORDERS
   ============================================== */
.no-borders {
    border-collapse: collapse;
}

.no-borders th,
.no-borders td {
    border: none;
    padding: 8pt;
}
```

---

## Column Widths

### Fixed Widths

```css
/* Specify width on th or td in first row */
table {
    width: 100%;
    table-layout: fixed;  /* Enforces specified widths */
}

th:nth-child(1) { width: 40%; }
th:nth-child(2) { width: 30%; }
th:nth-child(3) { width: 30%; }
```

### Using colgroup

```html
<table>
    <colgroup>
        <col style="width: 200pt;" />
        <col style="width: 150pt;" />
        <col style="width: 150pt;" />
    </colgroup>
    <thead>
        <tr>
            <th>Name</th>
            <th>Department</th>
            <th>Role</th>
        </tr>
    </thead>
    <tbody>
        <!-- table rows -->
    </tbody>
</table>
```

### Auto vs Fixed Layout

```css
/* Auto layout - adjusts to content */
table.auto {
    table-layout: auto;  /* Default, can be slow */
    width: 100%;
}

/* Fixed layout - uses specified widths */
table.fixed {
    table-layout: fixed;  /* Faster, predictable */
    width: 100%;
}
```

---

## Cell Padding and Spacing

### Padding

```css
/* Cell padding (inside spacing) */
th, td {
    padding: 10pt;  /* All sides */
}

/* Different padding per side */
th, td {
    padding-top: 8pt;
    padding-right: 12pt;
    padding-bottom: 8pt;
    padding-left: 12pt;
}

/* Shorthand */
th, td {
    padding: 8pt 12pt;  /* vertical horizontal */
}
```

### Border Spacing

```css
/* Only works with border-collapse: separate */
table {
    border-collapse: separate;
    border-spacing: 5pt;  /* Space between cells */
}

/* Different horizontal and vertical spacing */
table {
    border-collapse: separate;
    border-spacing: 10pt 5pt;  /* horizontal vertical */
}
```

---

## Cell Alignment

### Horizontal Alignment

```css
/* Left align (default for td) */
td {
    text-align: left;
}

/* Center align (common for th) */
th {
    text-align: center;
}

/* Right align (numbers) */
.number-column {
    text-align: right;
}
```

### Vertical Alignment

```css
/* Top align */
td {
    vertical-align: top;
}

/* Middle align (default) */
td {
    vertical-align: middle;
}

/* Bottom align */
td {
    vertical-align: bottom;
}
```

---

## Alternating Rows

### Striped Rows

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

### Hover Effects

```css
/* Note: Hover may not work in PDF */
tbody tr:hover {
    background-color: #eff6ff;
}
```

---

## Practical Examples

### Example 1: Financial Report Table

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Financial Report</title>
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
            font-size: 24pt;
            margin: 0 0 30pt 0;
            color: #1e40af;
        }

        /* ==============================================
           FINANCIAL TABLE
           ============================================== */
        .financial-table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }

        .financial-table thead th {
            background-color: #1e40af;
            color: white;
            padding: 12pt;
            text-align: left;
            font-weight: 600;
            border-bottom: 3pt solid #2563eb;
        }

        .financial-table tbody td {
            padding: 10pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        .financial-table tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        /* Right-align number columns */
        .financial-table .amount {
            text-align: right;
            font-family: 'Courier New', monospace;
            font-weight: 600;
        }

        /* Footer totals */
        .financial-table tfoot td {
            padding: 12pt;
            font-weight: 700;
            border-top: 2pt solid #2563eb;
            background-color: #eff6ff;
        }

        .financial-table tfoot .amount {
            color: #1e40af;
            font-size: 12pt;
        }

        /* Highlight negative values */
        .negative {
            color: #dc2626;
        }

        .positive {
            color: #10b981;
        }
    </style>
</head>
<body>
    <h1>Quarterly Financial Report</h1>

    <table class="financial-table">
        <thead>
            <tr>
                <th>Category</th>
                <th>Q1</th>
                <th>Q2</th>
                <th>Q3</th>
                <th>Q4</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Revenue</td>
                <td class="amount positive">$125,000</td>
                <td class="amount positive">$142,500</td>
                <td class="amount positive">$138,750</td>
                <td class="amount positive">$156,200</td>
                <td class="amount positive">$562,450</td>
            </tr>
            <tr>
                <td>Operating Expenses</td>
                <td class="amount negative">($45,000)</td>
                <td class="amount negative">($48,300)</td>
                <td class="amount negative">($47,100)</td>
                <td class="amount negative">($51,200)</td>
                <td class="amount negative">($191,600)</td>
            </tr>
            <tr>
                <td>Marketing</td>
                <td class="amount negative">($15,000)</td>
                <td class="amount negative">($18,500)</td>
                <td class="amount negative">($17,200)</td>
                <td class="amount negative">($22,000)</td>
                <td class="amount negative">($72,700)</td>
            </tr>
            <tr>
                <td>Research & Development</td>
                <td class="amount negative">($25,000)</td>
                <td class="amount negative">($28,000)</td>
                <td class="amount negative">($26,500)</td>
                <td class="amount negative">($30,000)</td>
                <td class="amount negative">($109,500)</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td>Net Income</td>
                <td class="amount">$40,000</td>
                <td class="amount">$47,700</td>
                <td class="amount">$47,950</td>
                <td class="amount">$53,000</td>
                <td class="amount">$188,650</td>
            </tr>
        </tfoot>
    </table>
</body>
</html>
```

### Example 2: Product Specification Table

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Product Specifications</title>
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
            font-size: 24pt;
            margin: 0 0 10pt 0;
            color: #1e40af;
        }

        .product-name {
            font-size: 16pt;
            color: #666;
            margin: 0 0 30pt 0;
        }

        /* ==============================================
           SPECIFICATION TABLE
           ============================================== */
        .spec-table {
            width: 100%;
            border-collapse: collapse;
            border: 2pt solid #2563eb;
        }

        .spec-table th {
            background-color: #2563eb;
            color: white;
            padding: 12pt;
            text-align: left;
            font-weight: 600;
        }

        .spec-table tbody tr {
            border-bottom: 1pt solid #e5e7eb;
        }

        .spec-table tbody tr:last-child {
            border-bottom: none;
        }

        .spec-table td {
            padding: 10pt;
        }

        /* First column (labels) */
        .spec-table td:first-child {
            font-weight: 600;
            color: #1e40af;
            width: 30%;
            background-color: #f9fafb;
        }

        /* Second column (values) */
        .spec-table td:last-child {
            width: 70%;
        }

        /* Highlight important specs */
        .highlight {
            background-color: #fef3c7;
            font-weight: 600;
        }
    </style>
</head>
<body>
    <h1>Product Specifications</h1>
    <p class="product-name">Model X-2000 Professional</p>

    <table class="spec-table">
        <thead>
            <tr>
                <th>Specification</th>
                <th>Details</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Processor</td>
                <td>Intel Core i7-12700K, 3.6 GHz (5.0 GHz Turbo)</td>
            </tr>
            <tr>
                <td>Memory</td>
                <td>32 GB DDR5 RAM</td>
            </tr>
            <tr>
                <td>Storage</td>
                <td>1 TB NVMe SSD + 2 TB HDD</td>
            </tr>
            <tr>
                <td>Graphics</td>
                <td>NVIDIA GeForce RTX 4070, 12 GB GDDR6X</td>
            </tr>
            <tr>
                <td>Display</td>
                <td>27" 4K UHD (3840×2160), 144Hz, IPS Panel</td>
            </tr>
            <tr class="highlight">
                <td>Warranty</td>
                <td>3 Years Premium Support</td>
            </tr>
            <tr>
                <td>Dimensions</td>
                <td>18.5" × 8.2" × 16.3" (W × D × H)</td>
            </tr>
            <tr>
                <td>Weight</td>
                <td>22.5 lbs (10.2 kg)</td>
            </tr>
            <tr>
                <td>Power Supply</td>
                <td>750W 80+ Gold Certified</td>
            </tr>
            <tr>
                <td>Operating System</td>
                <td>Windows 11 Pro 64-bit</td>
            </tr>
        </tbody>
    </table>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Price List

Create a pricing table with:
- Product names, descriptions, and prices
- Alternating row colors
- Right-aligned prices
- Total row at bottom with distinct styling

### Exercise 2: Comparison Table

Build a feature comparison table with:
- 3-4 products as columns
- Features as rows
- Checkmarks (✓) or X marks (✗) for availability
- Highlight differences

### Exercise 3: Schedule Table

Design a weekly schedule table with:
- Days as columns
- Time slots as rows
- Colored cells for different activities
- Clear borders and spacing

---

## Common Pitfalls

### ❌ Not Setting Table Width

```css
table {
    /* Width undefined, may overflow */
    border-collapse: collapse;
}
```

✅ **Solution:**

```css
table {
    width: 100%;
    border-collapse: collapse;
}
```

### ❌ Forgetting border-collapse

```css
table {
    width: 100%;
    /* Missing border-collapse */
}

th, td {
    border: 1pt solid black;
}
```

✅ **Solution:**

```css
table {
    width: 100%;
    border-collapse: collapse;  /* Single borders */
}

th, td {
    border: 1pt solid black;
}
```

### ❌ Inconsistent Cell Padding

```css
th {
    padding: 15pt;
}

td {
    padding: 5pt;  /* Different from th */
}
```

✅ **Solution:**

```css
th, td {
    padding: 10pt;  /* Consistent */
}
```

### ❌ Not Right-Aligning Numbers

```css
/* Numbers left-aligned, hard to compare */
td {
    text-align: left;
}
```

✅ **Solution:**

```css
.number-column {
    text-align: right;
}
```

---

## Table Basics Checklist

- [ ] Table width specified (usually 100%)
- [ ] border-collapse set to collapse
- [ ] thead, tbody, tfoot used appropriately
- [ ] Consistent cell padding
- [ ] Numbers right-aligned
- [ ] Column widths defined
- [ ] Border styles consistent
- [ ] Headers styled distinctly

---

## Best Practices

1. **Use border-collapse: collapse** - Single borders between cells
2. **Set Table Width** - Usually 100% of container
3. **Consistent Padding** - 8-12pt for readability
4. **Right-Align Numbers** - Easier to compare
5. **Define Column Widths** - Use percentages or fixed widths
6. **Style Headers Distinctly** - Different background color
7. **Alternating Rows** - Improves readability
8. **Test with Data** - Varying content lengths

---

## Next Steps

1. **[Tables - Advanced](06_tables_advanced.md)** - Dynamic data and calculations
2. **[Attachments & Embedded Content](07_attachments_embedded.md)** - File attachments
3. **[Content Best Practices](08_content_best_practices.md)** - Optimization

---

**Continue learning →** [Tables - Advanced](06_tables_advanced.md)
