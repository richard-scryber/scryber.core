---
layout: default
title: Layout Best Practices
nav_order: 8
parent: Layout & Positioning
parent_url: /learning/04-layout/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Layout Best Practices

Master professional layout patterns, optimization techniques, and common gotchas for PDF generation.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Apply professional layout patterns
- Optimize layout performance
- Avoid common layout pitfalls
- Create maintainable layout systems
- Handle edge cases gracefully
- Build production-ready page layouts
- Troubleshoot layout issues effectively

---

## Core Layout Principles

### 1. Content-First Design

Start with content structure, then apply layout.

**❌ Layout-first thinking:**
```html
<div class="fancy-grid-system">
    <!-- Force content into rigid structure -->
</div>
```

**✅ Content-first approach:**
```html
<article>
    <h1>Natural heading</h1>
    <p>Content flows naturally</p>
    <!-- Apply layout as needed -->
</article>
```

### 2. Predictable Page Breaks

Control where content breaks across pages.

**❌ Uncontrolled breaks:**
```css
/* Headings orphaned, tables split awkwardly */
```

**✅ Controlled breaks:**
```css
h1, h2, h3 {
    page-break-after: avoid;  /* Keep with following content */
}

table, .card, .example {
    page-break-inside: avoid;  /* Keep together */
}

.chapter {
    page-break-before: always;  /* New page for chapters */
}
```

### 3. Consistent Spacing

Use a spacing scale throughout.

**❌ Random spacing:**
```css
.element1 { margin-bottom: 17pt; }
.element2 { margin-bottom: 23pt; }
.element3 { margin-bottom: 11pt; }
```

**✅ Consistent scale:**
```css
:root {
    --space-sm: 10pt;
    --space-md: 20pt;
    --space-lg: 30pt;
}

.element1 { margin-bottom: var(--space-sm); }
.element2 { margin-bottom: var(--space-md); }
.element3 { margin-bottom: var(--space-lg); }
```

---

## Professional Layout Patterns

### Pattern 1: Master Page Template

Create reusable page structure.

```css
/* ==============================================
   PAGE SETUP
   ============================================== */
@page {
    size: Letter;
    margin: 1.25in 1in 1in 1in;
}

* {
    box-sizing: border-box;
}

/* ==============================================
   TYPOGRAPHY HIERARCHY
   ============================================== */
body {
    font-family: Helvetica, sans-serif;
    font-size: 11pt;
    line-height: 1.6;
    color: #333;
    margin: 0;
}

h1 {
    font-size: 24pt;
    color: #1e40af;
    margin-top: 0;
    margin-bottom: 20pt;
    page-break-after: avoid;
}

h2 {
    font-size: 18pt;
    color: #2563eb;
    margin-top: 30pt;
    margin-bottom: 15pt;
    page-break-after: avoid;
}

p {
    margin-top: 0;
    margin-bottom: 12pt;
    orphans: 3;
    widows: 3;
}

/* ==============================================
   PAGE BREAKS
   ============================================== */
.section {
    page-break-before: always;
}

.no-break {
    page-break-inside: avoid;
}

/* ==============================================
   LAYOUT HELPERS
   ============================================== */
.container {
    width: 100%;
    max-width: 600pt;
    margin: 0 auto;
}

.two-column {
    display: table;
    width: 100%;
}

.column {
    display: table-cell;
    width: 50%;
    vertical-align: top;
    padding: 0 15pt;
}

.column:first-child {
    padding-left: 0;
}

.column:last-child {
    padding-right: 0;
}
```

### Pattern 2: Card-Based Layout

Reusable card components.

```css
.card {
    border: 1pt solid #d1d5db;
    border-radius: 5pt;
    padding: 20pt;
    margin-bottom: 20pt;
    page-break-inside: avoid;
}

.card-header {
    border-bottom: 1pt solid #e5e7eb;
    margin: -20pt -20pt 15pt -20pt;
    padding: 15pt 20pt;
    background-color: #f9fafb;
}

.card-title {
    margin: 0;
    font-size: 16pt;
    color: #1e40af;
}

.card-body {
    /* Content area */
}

.card-footer {
    border-top: 1pt solid #e5e7eb;
    margin: 15pt -20pt -20pt -20pt;
    padding: 10pt 20pt;
    background-color: #f9fafb;
    font-size: 9pt;
    color: #666;
}
```

### Pattern 3: Sidebar Layout

Fixed sidebar with fluid content.

```css
.page-layout {
    display: table;
    width: 100%;
}

.sidebar {
    display: table-cell;
    width: 200pt;  /* Fixed width */
    background-color: #f9fafb;
    padding: 20pt;
    vertical-align: top;
    border-right: 1pt solid #d1d5db;
}

.main-content {
    display: table-cell;
    padding: 20pt;
    padding-left: 30pt;  /* Extra left padding */
    vertical-align: top;
}
```

### Pattern 4: Grid-Style Layout

Table-based grid system.

```css
/* Three-column grid */
.grid {
    display: table;
    width: 100%;
    border-spacing: 20pt 20pt;  /* Gaps */
}

.grid-row {
    display: table-row;
}

.grid-cell {
    display: table-cell;
    width: 33.33%;
    vertical-align: top;
}

/* Two-column grid */
.grid-cell-half {
    display: table-cell;
    width: 50%;
    vertical-align: top;
}

/* Four-column grid */
.grid-cell-quarter {
    display: table-cell;
    width: 25%;
    vertical-align: top;
}
```

---

## Performance Optimization

### 1. Minimize Complex Selectors

**❌ Slow, complex selectors:**
```css
body div.container div.content div.section div.box p.text span.highlight {
    color: red;  /* Very slow to match */
}
```

**✅ Simple, fast selectors:**
```css
.highlight {
    color: red;  /* Fast */
}
```

### 2. Use table-layout: fixed

**❌ Auto layout (slower):**
```css
table {
    table-layout: auto;  /* Content-based, slower */
}
```

**✅ Fixed layout (faster):**
```css
table {
    table-layout: fixed;  /* Width-based, faster */
    width: 100%;
}

col:nth-child(1) { width: 40%; }
col:nth-child(2) { width: 30%; }
col:nth-child(3) { width: 30%; }
```

### 3. Avoid Excessive Nesting

**❌ Deep nesting:**
```html
<div>
    <div>
        <div>
            <div>
                <div>
                    <p>Content</p>  <!-- 5 levels deep -->
                </div>
            </div>
        </div>
    </div>
</div>
```

**✅ Flatten structure:**
```html
<article class="content">
    <p>Content</p>  <!-- Flat, clear -->
</article>
```

### 4. Specify Dimensions

**❌ Unknown dimensions:**
```css
.box {
    /* No width/height - browser must calculate */
}
```

**✅ Explicit dimensions:**
```css
.box {
    width: 400pt;  /* Explicit - faster rendering */
    height: 200pt;
}
```

---

## Common Layout Pitfalls

### Pitfall 1: Forgetting box-sizing

**❌ Problem:**
```css
.box {
    width: 200pt;
    padding: 20pt;
    border: 2pt solid black;
    /* Total width: 244pt (not 200pt!) */
}
```

**✅ Solution:**
```css
* {
    box-sizing: border-box;  /* Include padding/border in width */
}

.box {
    width: 200pt;  /* Now truly 200pt total */
    padding: 20pt;
    border: 2pt solid black;
}
```

### Pitfall 2: Using Unsupported Layout Methods

**❌ Problem:**
```css
.container {
    display: flex;  /* Not supported in PDF! */
    display: grid;  /* Not supported in PDF! */
}
```

**✅ Solution:**
```css
.container {
    display: table;  /* Supported, reliable */
}

.column {
    display: table-cell;
}
```

### Pitfall 3: Not Controlling Page Breaks

**❌ Problem:**
```html
<h2>Important Section</h2>
<!-- Page break happens here - heading orphaned! -->
<p>Content that starts on next page...</p>
```

**✅ Solution:**
```css
h2 {
    page-break-after: avoid;  /* Keep heading with content */
}
```

### Pitfall 4: Inconsistent Column Widths

**❌ Problem:**
```css
.col1 { width: 45%; }
.col2 { width: 50%; }  /* Total: 95% or 105%? Unclear! */
```

**✅ Solution:**
```css
.col1 { width: 40%; }
.col2 { width: 60%; }  /* Total: 100% */
/* Or */
.col1 { width: 200pt; }
.col2 { width: calc(100% - 200pt); }  /* Exactly fills */
```

### Pitfall 5: Absolute Positioning Without Context

**❌ Problem:**
```css
.child {
    position: absolute;
    top: 50pt;  /* Relative to page, not parent! */
}
```

**✅ Solution:**
```css
.parent {
    position: relative;  /* Create positioning context */
}

.child {
    position: absolute;
    top: 50pt;  /* Now relative to .parent */
}
```

---

## Responsive Layout Strategies

### Strategy 1: Percentage-Based Widths

```css
.fluid {
    width: 100%;  /* Adapts to container */
    max-width: 600pt;  /* But constrained */
}
```

### Strategy 2: Calc() for Dynamic Sizing

```css
.sidebar {
    width: 200pt;  /* Fixed */
}

.content {
    width: calc(100% - 200pt);  /* Remaining space */
}
```

### Strategy 3: Flexible Tables

```css
table {
    width: 100%;  /* Full width */
}

col:nth-child(1) { width: 30%; }  /* Proportional */
col:nth-child(2) { width: 50%; }
col:nth-child(3) { width: 20%; }
```

---

## Practical Example: Complete Professional Layout

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Professional Layout</title>
    <style>
        /* ==============================================
           PAGE SETUP
           ============================================== */
        @page {
            size: Letter;
            margin: 1.25in 1in 1in 1in;
        }

        * {
            box-sizing: border-box;
        }

        /* ==============================================
           BASE STYLES
           ============================================== */
        body {
            font-family: Helvetica, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            color: #333;
            margin: 0;
        }

        :root {
            --color-primary: #2563eb;
            --color-secondary: #3b82f6;
            --color-text: #333;
            --color-text-light: #666;
            --color-border: #d1d5db;
            --color-bg-light: #f9fafb;

            --space-xs: 5pt;
            --space-sm: 10pt;
            --space-md: 20pt;
            --space-lg: 30pt;
            --space-xl: 40pt;
        }

        /* ==============================================
           TYPOGRAPHY
           ============================================== */
        h1 {
            font-size: 24pt;
            color: #1e40af;
            margin-top: 0;
            margin-bottom: var(--space-md);
            page-break-after: avoid;
        }

        h2 {
            font-size: 18pt;
            color: var(--color-primary);
            margin-top: var(--space-lg);
            margin-bottom: var(--space-md);
            page-break-after: avoid;
        }

        h3 {
            font-size: 14pt;
            margin-top: var(--space-md);
            margin-bottom: var(--space-sm);
            page-break-after: avoid;
        }

        p {
            margin-top: 0;
            margin-bottom: var(--space-sm);
            orphans: 3;
            widows: 3;
        }

        /* ==============================================
           HEADER & FOOTER
           ============================================== */
        header {
            border-bottom: 2pt solid var(--color-primary);
            padding-bottom: 15pt;
            margin-bottom: var(--space-lg);
        }

        .header-content {
            display: table;
            width: 100%;
        }

        .header-left {
            display: table-cell;
            vertical-align: middle;
        }

        .header-right {
            display: table-cell;
            text-align: right;
            vertical-align: middle;
            color: var(--color-text-light);
            font-size: 10pt;
        }

        footer {
            border-top: 1pt solid var(--color-border);
            padding-top: 15pt;
            margin-top: var(--space-xl);
            font-size: 9pt;
            color: var(--color-text-light);
            text-align: center;
        }

        /* ==============================================
           LAYOUT COMPONENTS
           ============================================== */
        .section {
            margin-bottom: var(--space-xl);
        }

        .card {
            border: 1pt solid var(--color-border);
            border-radius: 5pt;
            padding: var(--space-md);
            margin-bottom: var(--space-md);
            page-break-inside: avoid;
        }

        .card-highlighted {
            border-left: 4pt solid var(--color-primary);
            background-color: var(--color-bg-light);
        }

        .two-column-layout {
            display: table;
            width: 100%;
            border-spacing: var(--space-md) 0;
        }

        .column {
            display: table-cell;
            width: 50%;
            vertical-align: top;
        }

        /* ==============================================
           UTILITIES
           ============================================== */
        .text-center { text-align: center; }
        .text-right { text-align: right; }
        .text-bold { font-weight: bold; }

        .mb-sm { margin-bottom: var(--space-sm); }
        .mb-md { margin-bottom: var(--space-md); }
        .mb-lg { margin-bottom: var(--space-lg); }

        /* ==============================================
           TABLES
           ============================================== */
        table {
            width: 100%;
            border-collapse: collapse;
            margin: var(--space-md) 0;
            table-layout: fixed;
        }

        thead {
            background-color: var(--color-primary);
            color: white;
        }

        th, td {
            padding: 10pt;
            text-align: left;
            border: 1pt solid var(--color-border);
        }

        tbody tr:nth-child(even) {
            background-color: var(--color-bg-light);
        }
    </style>
</head>
<body>
    <!-- Header -->
    <header>
        <div class="header-content">
            <div class="header-left">
                <strong style="font-size: 18pt; color: #1e40af;">Acme Corporation</strong>
            </div>
            <div class="header-right">
                Quarterly Report Q4 2024
            </div>
        </div>
    </header>

    <!-- Executive Summary -->
    <div class="section">
        <h1>Executive Summary</h1>

        <div class="card card-highlighted">
            <h3 style="margin-top: 0;">Key Highlights</h3>
            <ul style="margin-bottom: 0;">
                <li>Revenue increased 15% year-over-year</li>
                <li>Expanded to 3 new markets</li>
                <li>Customer satisfaction up 22%</li>
                <li>12 new products launched</li>
            </ul>
        </div>

        <p>This report provides a comprehensive overview of our business performance for Q4 2024.</p>
    </div>

    <!-- Financial Performance -->
    <div class="section">
        <h1>Financial Performance</h1>

        <h2>Revenue Breakdown</h2>

        <table>
            <thead>
                <tr>
                    <th>Category</th>
                    <th style="text-align: right;">Q4 2024</th>
                    <th style="text-align: right;">Q4 2023</th>
                    <th style="text-align: right;">Growth</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Products</td>
                    <td style="text-align: right;">$2,500,000</td>
                    <td style="text-align: right;">$2,100,000</td>
                    <td style="text-align: right; color: #059669;">+19%</td>
                </tr>
                <tr>
                    <td>Services</td>
                    <td style="text-align: right;">$1,200,000</td>
                    <td style="text-align: right;">$1,050,000</td>
                    <td style="text-align: right; color: #059669;">+14%</td>
                </tr>
                <tr>
                    <td>Subscriptions</td>
                    <td style="text-align: right;">$650,000</td>
                    <td style="text-align: right;">$550,000</td>
                    <td style="text-align: right; color: #059669;">+18%</td>
                </tr>
            </tbody>
        </table>

        <h2>Geographic Performance</h2>

        <div class="two-column-layout">
            <div class="column">
                <div class="card">
                    <h3 style="margin-top: 0; color: #2563eb;">North America</h3>
                    <p class="mb-sm"><strong>Revenue:</strong> $2.1M</p>
                    <p class="mb-sm"><strong>Growth:</strong> <span style="color: #059669;">+12%</span></p>
                    <p style="margin-bottom: 0;"><strong>Market Share:</strong> 35%</p>
                </div>
            </div>

            <div class="column">
                <div class="card">
                    <h3 style="margin-top: 0; color: #2563eb;">Europe</h3>
                    <p class="mb-sm"><strong>Revenue:</strong> $1.8M</p>
                    <p class="mb-sm"><strong>Growth:</strong> <span style="color: #059669;">+18%</span></p>
                    <p style="margin-bottom: 0;"><strong>Market Share:</strong> 28%</p>
                </div>
            </div>
        </div>
    </div>

    <!-- Footer -->
    <footer>
        <p>© 2025 Acme Corporation | Confidential | Page <span class="page-number"></span></p>
    </footer>
</body>
</html>
```

---

## Layout Troubleshooting Guide

### Issue: Content Overflowing

**Symptoms:**
- Content cut off
- Text disappearing

**Checklist:**
1. ✅ Check container width (100% or specified)
2. ✅ Verify box-sizing: border-box is set
3. ✅ Check for fixed widths that are too large
4. ✅ Ensure calc() calculations are correct
5. ✅ Verify padding/margin aren't causing overflow

### Issue: Uneven Columns

**Symptoms:**
- Columns different widths
- Layout looks unbalanced

**Checklist:**
1. ✅ Verify widths add up to 100%
2. ✅ Check for extra padding/margins
3. ✅ Ensure display: table-cell on all columns
4. ✅ Verify vertical-align: top is set
5. ✅ Check border-spacing if using

### Issue: Page Breaks in Wrong Places

**Symptoms:**
- Headings separated from content
- Tables split awkwardly

**Checklist:**
1. ✅ Add page-break-after: avoid to headings
2. ✅ Add page-break-inside: avoid to cards/tables
3. ✅ Set orphans and widows to 3+
4. ✅ Test with real content length
5. ✅ Verify page margins are adequate

---

## Production Checklist

### Layout Checklist

- [ ] box-sizing: border-box applied globally
- [ ] Page margins defined (@page rule)
- [ ] Typography hierarchy established
- [ ] Consistent spacing scale used
- [ ] Page breaks controlled strategically
- [ ] Headers/footers implemented
- [ ] All layouts use supported methods (table-cell, not flexbox/grid)
- [ ] Tested with multi-page content
- [ ] Column widths total 100%
- [ ] No content overflow

### Performance Checklist

- [ ] Simple, efficient selectors (max 3 levels)
- [ ] table-layout: fixed for tables
- [ ] Dimensions specified where possible
- [ ] Minimal nesting depth
- [ ] Reusable component classes
- [ ] CSS organized logically
- [ ] No complex calculations in render path

### Quality Checklist

- [ ] Tested with real content
- [ ] Tested with edge cases (empty, very long)
- [ ] Print preview checked
- [ ] Page breaks verified
- [ ] All pages have headers/footers
- [ ] Typography is readable
- [ ] Spacing is consistent
- [ ] Layout is maintainable

---

## Best Practices Summary

1. **Use box-sizing: border-box** - Always, globally
2. **Control page breaks** - Headings, cards, tables
3. **Consistent spacing scale** - 10pt, 20pt, 30pt, etc.
4. **Simple selectors** - Maximum 3 levels deep
5. **table-cell for columns** - Not flexbox or grid
6. **Specify dimensions** - When known, for performance
7. **Test with real content** - Edge cases and page breaks
8. **Organize CSS logically** - Base, layout, components, utilities
9. **Comment complex layouts** - Explain why, not just what
10. **Validate frequently** - Generate PDFs early and often

---

## Next Steps

You've completed the Layout & Positioning series! Continue your journey:

1. **[Typography & Fonts](/learning/05-typography/)** - Advanced text styling
2. **[Content Components](/learning/06-content/)** - Images, lists, and more
3. **[Practical Applications](/learning/08-practical/)** - Real-world documents

---

**Continue learning →** [Typography & Fonts](/learning/05-typography/)**
