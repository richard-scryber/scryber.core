---
layout: default
title: Sections & Page Breaks
nav_order: 3
parent: Layout & Positioning
parent_url: /learning/04-layout/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Sections & Page Breaks

Master page break control and document sectioning for professional multi-page PDF layouts.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Control page breaks with CSS properties
- Prevent awkward page breaks
- Force page breaks before/after elements
- Keep content together on pages
- Understand orphans and widows
- Create multi-section documents
- Use page-break properties strategically

---

## Why Page Breaks Matter

Unlike web pages that scroll continuously, PDFs have discrete pages. Controlling where content breaks is crucial for:
- **Professional appearance** - No headings orphaned at page bottom
- **Readability** - Related content stays together
- **Logical flow** - Sections start on new pages
- **Print quality** - Content breaks at natural points

---

## Page Break Properties

### page-break-before

Controls whether a page break occurs before an element.

```css
/* Values */
.element {
    page-break-before: auto;    /* Default: break if needed */
    page-break-before: always;  /* Always start new page */
    page-break-before: avoid;   /* Avoid breaking before */
    page-break-before: left;    /* Break to left page (even) */
    page-break-before: right;   /* Break to right page (odd) */
}
```

**Common use:**
```css
h1 {
    page-break-before: always;  /* Each h1 starts new page */
}

.chapter {
    page-break-before: always;  /* New page for chapters */
}
```

### page-break-after

Controls whether a page break occurs after an element.

```css
.element {
    page-break-after: auto;    /* Default */
    page-break-after: always;  /* Force page break after */
    page-break-after: avoid;   /* Avoid breaking after */
}
```

**Common use:**
```css
.section-end {
    page-break-after: always;  /* Break after section */
}

h2 {
    page-break-after: avoid;  /* Don't break right after heading */
}
```

### page-break-inside

Controls whether a page break can occur inside an element.

```css
.element {
    page-break-inside: auto;   /* Default: can break */
    page-break-inside: avoid;  /* Keep together on same page */
}
```

**Common use:**
```css
table {
    page-break-inside: avoid;  /* Don't split tables */
}

.card {
    page-break-inside: avoid;  /* Keep cards together */
}

.code-block {
    page-break-inside: avoid;  /* Don't split code */
}
```

---

## Practical Page Break Patterns

### Pattern 1: Chapter Breaks

```css
.chapter {
    page-break-before: always;  /* Start each chapter on new page */
}

.chapter h1 {
    margin-top: 0;  /* No extra top margin (already on new page) */
}
```

### Pattern 2: Keep Headings with Content

```css
h1, h2, h3, h4, h5, h6 {
    page-break-after: avoid;   /* Don't break right after heading */
    page-break-inside: avoid;  /* Don't break inside heading */
}
```

### Pattern 3: Keep Content Blocks Together

```css
.card, .section, .example {
    page-break-inside: avoid;  /* Keep entire block together */
}
```

### Pattern 4: Force Breaks Between Sections

```css
.section {
    page-break-after: always;  /* Each section on separate page */
}

.section:last-child {
    page-break-after: auto;  /* Don't break after last section */
}
```

---

## Orphans and Widows

Control minimum lines at page breaks.

### Orphans

Minimum lines at the *bottom* of a page before a break.

```css
p {
    orphans: 2;  /* At least 2 lines at bottom */
}

/* Default is usually 2 */
```

**Problem:** Single line at page bottom looks awkward
**Solution:** Set orphans: 2 or higher

### Widows

Minimum lines at the *top* of a page after a break.

```css
p {
    widows: 2;  /* At least 2 lines at top */
}

/* Default is usually 2 */
```

**Problem:** Single line at page top looks awkward
**Solution:** Set widows: 2 or higher

### Combined Control

```css
p, li {
    orphans: 3;  /* At least 3 lines at bottom */
    widows: 3;   /* At least 3 lines at top */
}
```

---

## Practical Examples

### Example 1: Multi-Chapter Document

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Multi-Chapter Book</title>
    <style>
        @page {
            size: 6in 9in;  /* Book size */
            margin: 0.75in;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: Georgia, serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 0;
        }

        /* ==============================================
           CHAPTER STYLING
           ============================================== */
        .chapter {
            page-break-before: always;  /* New page for each chapter */
        }

        .chapter h1 {
            font-size: 24pt;
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 40pt;
            padding-bottom: 20pt;
            border-bottom: 2pt solid #d1d5db;
            page-break-after: avoid;  /* Keep heading with content */
        }

        /* ==============================================
           HEADINGS
           ============================================== */
        h2 {
            font-size: 18pt;
            color: #2563eb;
            margin-top: 30pt;
            margin-bottom: 15pt;
            page-break-after: avoid;   /* Don't break after heading */
            page-break-inside: avoid;  /* Don't break inside heading */
        }

        h3 {
            font-size: 14pt;
            margin-top: 20pt;
            margin-bottom: 10pt;
            page-break-after: avoid;
            page-break-inside: avoid;
        }

        /* ==============================================
           CONTENT
           ============================================== */
        p {
            margin-top: 0;
            margin-bottom: 12pt;
            orphans: 3;  /* Min 3 lines at page bottom */
            widows: 3;   /* Min 3 lines at page top */
        }

        /* ==============================================
           SPECIAL BLOCKS
           ============================================== */
        .example {
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
            padding: 15pt;
            margin: 20pt 0;
            background-color: #f9fafb;
            page-break-inside: avoid;  /* Keep examples together */
        }

        .example-title {
            font-weight: bold;
            color: #2563eb;
            margin-top: 0;
            margin-bottom: 10pt;
        }
    </style>
</head>
<body>
    <!-- Chapter 1 -->
    <div class="chapter">
        <h1>Chapter 1: Introduction</h1>

        <p>This is the first chapter of our book. It provides an introduction to the main concepts we'll be exploring.</p>

        <h2>Background</h2>
        <p>Understanding the historical context is essential for grasping the full significance of our subject matter.</p>

        <div class="example">
            <p class="example-title">Example 1.1: Basic Concept</p>
            <p>This example demonstrates a fundamental principle. The entire example box will stay together on one page, never splitting across a page break.</p>
        </div>

        <h2>Key Concepts</h2>
        <p>The following concepts form the foundation of our discussion:</p>
        <ul>
            <li>First key concept with detailed explanation</li>
            <li>Second key concept that builds on the first</li>
            <li>Third key concept introducing new ideas</li>
        </ul>
    </div>

    <!-- Chapter 2 -->
    <div class="chapter">
        <h1>Chapter 2: Core Principles</h1>

        <p>In this chapter, we delve deeper into the core principles that govern our subject.</p>

        <h2>First Principle</h2>
        <p>The first principle establishes the groundwork for all subsequent ideas.</p>

        <div class="example">
            <p class="example-title">Example 2.1: Applying the Principle</p>
            <p>Here we see how the first principle manifests in practical scenarios.</p>
        </div>

        <h2>Second Principle</h2>
        <p>The second principle builds upon the first, creating a comprehensive framework.</p>
    </div>

    <!-- Chapter 3 -->
    <div class="chapter">
        <h1>Chapter 3: Advanced Topics</h1>

        <p>Having established the fundamentals, we now explore more advanced concepts.</p>

        <h2>Complex Interactions</h2>
        <p>When multiple principles interact, fascinating patterns emerge.</p>
    </div>
</body>
</html>
```

### Example 2: Report with Controlled Breaks

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Business Report</title>
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
            line-height: 1.6;
            margin: 0;
        }

        /* ==============================================
           COVER PAGE
           ============================================== */
        .cover {
            text-align: center;
            padding-top: 200pt;
            page-break-after: always;  /* Cover on separate page */
        }

        .cover h1 {
            font-size: 36pt;
            color: #1e40af;
            margin-bottom: 20pt;
        }

        .cover .subtitle {
            font-size: 18pt;
            color: #666;
            margin-bottom: 60pt;
        }

        /* ==============================================
           SECTIONS
           ============================================== */
        .section {
            margin-bottom: 40pt;
        }

        .section-title {
            font-size: 24pt;
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 20pt;
            padding-bottom: 10pt;
            border-bottom: 2pt solid #2563eb;
            page-break-after: avoid;  /* Keep title with content */
        }

        h2 {
            font-size: 18pt;
            color: #2563eb;
            margin-top: 25pt;
            margin-bottom: 15pt;
            page-break-after: avoid;
            page-break-inside: avoid;
        }

        /* ==============================================
           CONTENT BLOCKS
           ============================================== */
        .data-block {
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
            padding: 20pt;
            margin: 20pt 0;
            page-break-inside: avoid;  /* Don't split data blocks */
        }

        .data-block h3 {
            margin-top: 0;
            color: #2563eb;
        }

        /* ==============================================
           TABLES
           ============================================== */
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
            page-break-inside: avoid;  /* Keep tables together */
        }

        thead {
            background-color: #2563eb;
            color: white;
        }

        th, td {
            padding: 10pt;
            text-align: left;
            border: 1pt solid #d1d5db;
        }

        tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        /* ==============================================
           FOOTER
           ============================================== */
        .report-footer {
            margin-top: 60pt;
            padding-top: 20pt;
            border-top: 2pt solid #d1d5db;
            page-break-before: avoid;  /* Keep with last content */
        }
    </style>
</head>
<body>
    <!-- Cover Page -->
    <div class="cover">
        <h1>Annual Business Report</h1>
        <p class="subtitle">Fiscal Year 2024</p>
        <p>Prepared by: Finance Department<br/>
        Date: January 15, 2025</p>
    </div>

    <!-- Executive Summary -->
    <div class="section">
        <h1 class="section-title">Executive Summary</h1>
        <p>This report provides a comprehensive overview of our financial performance and strategic initiatives for fiscal year 2024.</p>

        <div class="data-block">
            <h3>Key Highlights</h3>
            <ul>
                <li>Revenue increased by 15% year-over-year</li>
                <li>Expanded to 3 new international markets</li>
                <li>Launched 12 new products</li>
                <li>Customer satisfaction improved by 22%</li>
            </ul>
        </div>
    </div>

    <!-- Financial Performance -->
    <div class="section">
        <h1 class="section-title">Financial Performance</h1>

        <h2>Revenue Analysis</h2>
        <p>Total revenue for fiscal year 2024 reached $12.5 million, representing significant growth over the previous year.</p>

        <table>
            <thead>
                <tr>
                    <th>Quarter</th>
                    <th>Revenue</th>
                    <th>Expenses</th>
                    <th>Profit</th>
                    <th>Growth</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Q1 2024</td>
                    <td>$2,800,000</td>
                    <td>$1,900,000</td>
                    <td>$900,000</td>
                    <td>12%</td>
                </tr>
                <tr>
                    <td>Q2 2024</td>
                    <td>$3,100,000</td>
                    <td>$2,050,000</td>
                    <td>$1,050,000</td>
                    <td>15%</td>
                </tr>
                <tr>
                    <td>Q3 2024</td>
                    <td>$3,250,000</td>
                    <td>$2,150,000</td>
                    <td>$1,100,000</td>
                    <td>18%</td>
                </tr>
                <tr>
                    <td>Q4 2024</td>
                    <td>$3,350,000</td>
                    <td>$2,200,000</td>
                    <td>$1,150,000</td>
                    <td>20%</td>
                </tr>
            </tbody>
        </table>

        <h2>Expense Management</h2>
        <p>Operating expenses were carefully controlled throughout the year, maintaining healthy profit margins.</p>

        <div class="data-block">
            <h3>Expense Breakdown</h3>
            <ul>
                <li>Personnel: 45%</li>
                <li>Operations: 30%</li>
                <li>Marketing: 15%</li>
                <li>Research & Development: 10%</li>
            </ul>
        </div>
    </div>

    <!-- Conclusion -->
    <div class="section">
        <h1 class="section-title">Conclusion</h1>
        <p>Fiscal year 2024 was a year of significant achievement and growth. We enter 2025 with strong momentum and clear strategic direction.</p>
    </div>

    <!-- Footer -->
    <div class="report-footer">
        <p style="text-align: center; color: #666; font-size: 9pt;">
            This report is confidential and intended for internal use only.<br/>
            © 2025 Acme Corporation. All rights reserved.
        </p>
    </div>
</body>
</html>
```

### Example 3: Product Catalog with Cards

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Product Catalog</title>
    <style>
        @page {
            size: Letter;
            margin: 0.75in;
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
            font-size: 28pt;
            color: #1e40af;
            text-align: center;
            margin-top: 0;
            margin-bottom: 40pt;
            padding-bottom: 20pt;
            border-bottom: 3pt solid #2563eb;
        }

        .category {
            margin-bottom: 40pt;
        }

        .category-title {
            font-size: 20pt;
            color: #2563eb;
            margin-top: 0;
            margin-bottom: 20pt;
            page-break-after: avoid;  /* Keep with products */
        }

        /* ==============================================
           PRODUCT CARDS
           ============================================== */
        .product-card {
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
            padding: 15pt;
            margin-bottom: 20pt;
            page-break-inside: avoid;  /* Keep entire card together */
        }

        .product-header {
            border-bottom: 1pt solid #e5e7eb;
            padding-bottom: 10pt;
            margin-bottom: 10pt;
        }

        .product-name {
            font-size: 16pt;
            font-weight: bold;
            color: #1e40af;
            margin: 0 0 5pt 0;
        }

        .product-sku {
            font-size: 9pt;
            color: #666;
        }

        .product-body {
            margin-bottom: 10pt;
        }

        .product-price {
            font-size: 18pt;
            font-weight: bold;
            color: #2563eb;
            margin-top: 10pt;
        }

        .product-features {
            margin: 10pt 0;
            padding-left: 20pt;
        }

        .product-features li {
            margin-bottom: 5pt;
        }
    </style>
</head>
<body>
    <h1>2025 Product Catalog</h1>

    <!-- Category 1 -->
    <div class="category">
        <h2 class="category-title">Electronics</h2>

        <div class="product-card">
            <div class="product-header">
                <div class="product-name">Premium Wireless Headphones</div>
                <div class="product-sku">SKU: ELEC-HEAD-001</div>
            </div>
            <div class="product-body">
                <p>High-quality wireless headphones with active noise cancellation and premium sound quality.</p>
                <ul class="product-features">
                    <li>40-hour battery life</li>
                    <li>Active noise cancellation</li>
                    <li>Bluetooth 5.0</li>
                    <li>Comfortable over-ear design</li>
                </ul>
                <div class="product-price">$299.99</div>
            </div>
        </div>

        <div class="product-card">
            <div class="product-header">
                <div class="product-name">Smart Watch Pro</div>
                <div class="product-sku">SKU: ELEC-WATCH-001</div>
            </div>
            <div class="product-body">
                <p>Advanced smartwatch with health tracking and smartphone integration.</p>
                <ul class="product-features">
                    <li>Heart rate monitoring</li>
                    <li>GPS tracking</li>
                    <li>Water resistant</li>
                    <li>7-day battery life</li>
                </ul>
                <div class="product-price">$399.99</div>
            </div>
        </div>

        <div class="product-card">
            <div class="product-header">
                <div class="product-name">Portable Bluetooth Speaker</div>
                <div class="product-sku">SKU: ELEC-SPEAK-001</div>
            </div>
            <div class="product-body">
                <p>Compact waterproof speaker with powerful 360-degree sound.</p>
                <ul class="product-features">
                    <li>12-hour battery life</li>
                    <li>Waterproof (IPX7)</li>
                    <li>360-degree sound</li>
                    <li>Compact and portable</li>
                </ul>
                <div class="product-price">$149.99</div>
            </div>
        </div>
    </div>

    <!-- Category 2 -->
    <div class="category">
        <h2 class="category-title">Home & Office</h2>

        <div class="product-card">
            <div class="product-header">
                <div class="product-name">Ergonomic Office Chair</div>
                <div class="product-sku">SKU: HOME-CHAIR-001</div>
            </div>
            <div class="product-body">
                <p>Premium ergonomic chair designed for all-day comfort and productivity.</p>
                <ul class="product-features">
                    <li>Lumbar support</li>
                    <li>Adjustable height and tilt</li>
                    <li>Breathable mesh back</li>
                    <li>5-year warranty</li>
                </ul>
                <div class="product-price">$449.99</div>
            </div>
        </div>

        <div class="product-card">
            <div class="product-header">
                <div class="product-name">Standing Desk Converter</div>
                <div class="product-sku">SKU: HOME-DESK-001</div>
            </div>
            <div class="product-body">
                <p>Transform any desk into a standing desk with easy height adjustment.</p>
                <ul class="product-features">
                    <li>Gas spring lift mechanism</li>
                    <li>Spacious work surface</li>
                    <li>Keyboard tray included</li>
                    <li>Supports dual monitors</li>
                </ul>
                <div class="product-price">$329.99</div>
            </div>
        </div>
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Chapter Document

Create a multi-chapter document with:
- Cover page (separate)
- 3 chapters, each starting on a new page
- Headings that don't break from following content
- Examples/callouts that stay together

### Exercise 2: Table Reports

Create a report with multiple tables:
- Tables that don't split across pages
- Headings that stay with tables
- Page breaks between major sections

### Exercise 3: Product Cards

Create a catalog with:
- Multiple product cards
- Cards that never split across pages
- Category headings that stay with products
- Proper spacing between elements

---

## Common Pitfalls

### ❌ Breaking Headings from Content

```css
h2 {
    margin-bottom: 15pt;
    /* Missing page-break-after: avoid */
}
```

✅ **Solution:** Prevent breaks after headings

```css
h2 {
    margin-bottom: 15pt;
    page-break-after: avoid;  /* Keep with following content */
}
```

### ❌ Allowing Tables to Split

```html
<table>
    <!-- Long table that splits across pages -->
</table>
```

✅ **Solution:** Keep tables together

```css
table {
    page-break-inside: avoid;  /* Don't split table */
}
```

### ❌ Forgetting to Control Orphans/Widows

```css
p {
    /* No orphan/widow control */
}
```

✅ **Solution:** Set minimum lines

```css
p {
    orphans: 3;  /* Min 3 lines at bottom */
    widows: 3;   /* Min 3 lines at top */
}
```

### ❌ Not Removing Last Page Break

```css
.section {
    page-break-after: always;  /* All sections, including last */
}
```

✅ **Solution:** Remove break from last element

```css
.section {
    page-break-after: always;
}

.section:last-child {
    page-break-after: auto;  /* Don't break after last */
}
```

### ❌ Overusing page-break-inside: avoid

```css
* {
    page-break-inside: avoid;  /* Everything stays together - pages overflow! */
}
```

✅ **Solution:** Use selectively

```css
.card, table, .example {
    page-break-inside: avoid;  /* Only specific elements */
}
```

---

## Page Break Checklist

- [ ] Headings don't break from following content (page-break-after: avoid)
- [ ] Important blocks stay together (page-break-inside: avoid)
- [ ] Chapters/sections start on new pages (page-break-before: always)
- [ ] Orphans and widows controlled (orphans: 3, widows: 3)
- [ ] Last elements don't force blank pages (:last-child handling)
- [ ] Tables and code blocks stay together
- [ ] Cover pages have page-break-after: always
- [ ] Content flows naturally between controlled breaks

---

## Best Practices

1. **Prevent heading orphans** - Use `page-break-after: avoid` on all headings
2. **Keep related content together** - Use `page-break-inside: avoid` for cards, tables, examples
3. **Force strategic breaks** - Use `page-break-before: always` for chapters/major sections
4. **Control line breaks** - Set `orphans` and `widows` to 3 or higher
5. **Remove unnecessary breaks** - Use `:last-child` to prevent blank pages
6. **Test with real content** - Page breaks depend on content length
7. **Balance control and flexibility** - Don't overuse `avoid`, allow natural flow

---

## Next Steps

Now that you control page breaks:

1. **[Multi-Column Layouts](04_multi_column.md)** - Create column-based designs
2. **[Positioning](05_positioning.md)** - Absolute and relative positioning
3. **[Headers & Footers](07_headers_footers.md)** - Repeating page elements

---

**Continue learning →** [Multi-Column Layouts](04_multi_column.md)
