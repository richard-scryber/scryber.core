---
layout: default
title: Multi-Column Layouts
nav_order: 4
parent: Layout & Positioning
parent_url: /learning/04-layout/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Multi-Column Layouts

Master column-based layouts using table-cell display for professional PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Create two-column layouts
- Build three or more column layouts
- Control column widths (fixed and fluid)
- Add gutters (spacing) between columns
- Understand why flexbox/grid don't work in PDF
- Use table-cell for reliable column layouts
- Create responsive column systems

---

## Why Not Flexbox or Grid?

Unlike modern web browsers, PDF rendering engines don't support:
- ❌ `display: flex` (flexbox)
- ❌ `display: grid` (CSS grid)
- ❌ Advanced layout features

**Instead, use:**
- ✅ `display: table` and `display: table-cell`
- ✅ `float: left` / `float: right`
- ✅ Percentage widths
- ✅ `calc()` for dynamic calculations

---

## Table-Cell Method (Recommended)

The most reliable method for column layouts in PDF.

### Basic Two-Column Layout

```css
.container {
    display: table;
    width: 100%;
}

.column {
    display: table-cell;
    width: 50%;  /* Each column takes 50% */
    padding: 15pt;
}
```

**HTML:**
```html
<div class="container">
    <div class="column">
        <p>Left column content</p>
    </div>
    <div class="column">
        <p>Right column content</p>
    </div>
</div>
```

### Three-Column Layout

```css
.container {
    display: table;
    width: 100%;
}

.column {
    display: table-cell;
    width: 33.33%;  /* Each column takes 1/3 */
    padding: 15pt;
}
```

### Four-Column Layout

```css
.container {
    display: table;
    width: 100%;
}

.column {
    display: table-cell;
    width: 25%;  /* Each column takes 1/4 */
    padding: 10pt;
}
```

---

## Column Width Control

### Equal Width Columns

```css
.container {
    display: table;
    width: 100%;
}

.column {
    display: table-cell;
    width: 50%;  /* Equal distribution */
}
```

### Unequal Width Columns

```css
.container {
    display: table;
    width: 100%;
}

.sidebar {
    display: table-cell;
    width: 200pt;  /* Fixed width */
}

.main-content {
    display: table-cell;
    /* Remaining width automatically */
}
```

### Percentage-Based Unequal Columns

```css
.container {
    display: table;
    width: 100%;
}

.sidebar {
    display: table-cell;
    width: 30%;  /* 30% of width */
}

.main-content {
    display: table-cell;
    width: 70%;  /* 70% of width */
}
```

---

## Adding Gutters (Spacing)

### Method 1: Padding

```css
.column {
    display: table-cell;
    width: 50%;
    padding: 0 15pt;  /* Horizontal padding */
}

/* Remove padding on outer edges */
.column:first-child {
    padding-left: 0;
}

.column:last-child {
    padding-right: 0;
}
```

### Method 2: Border Spacing

```css
.container {
    display: table;
    width: 100%;
    border-spacing: 20pt 0;  /* Horizontal spacing only */
}

.column {
    display: table-cell;
    width: 50%;
}
```

### Method 3: Calc() with Explicit Gaps

```css
.container {
    display: table;
    width: 100%;
}

/* Two columns with 20pt gap */
.column {
    display: table-cell;
    width: calc(50% - 10pt);  /* 50% minus half the gap */
}

.column:not(:last-child) {
    padding-right: 20pt;  /* Full gap */
}

/* Three columns with 20pt gaps */
.column-three {
    display: table-cell;
    width: calc(33.33% - 13.33pt);  /* 33.33% minus distributed gap */
}
```

---

## Vertical Alignment

Control how content aligns vertically within columns.

```css
.column {
    display: table-cell;
    vertical-align: top;     /* Align to top (recommended) */
}

.column-middle {
    display: table-cell;
    vertical-align: middle;  /* Center vertically */
}

.column-bottom {
    display: table-cell;
    vertical-align: bottom;  /* Align to bottom */
}
```

---

## Borders Between Columns

### Method 1: Border on Column

```css
.column {
    display: table-cell;
    border-right: 1pt solid #d1d5db;
    padding-right: 15pt;
}

.column:last-child {
    border-right: none;  /* Remove from last column */
}
```

### Method 2: Separator Element

```css
.column-divider {
    display: table-cell;
    width: 1pt;
    background-color: #d1d5db;
}
```

**HTML:**
```html
<div class="container">
    <div class="column">Left content</div>
    <div class="column-divider"></div>
    <div class="column">Right content</div>
</div>
```

---

## Practical Examples

### Example 1: Sidebar Layout

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Sidebar Layout</title>
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
           LAYOUT CONTAINER
           ============================================== */
        .page-container {
            display: table;
            width: 100%;
        }

        /* ==============================================
           SIDEBAR
           ============================================== */
        .sidebar {
            display: table-cell;
            width: 180pt;  /* Fixed width */
            background-color: #f9fafb;
            padding: 20pt;
            vertical-align: top;
            border-right: 1pt solid #d1d5db;
        }

        .sidebar h2 {
            font-size: 14pt;
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 15pt;
        }

        .nav-item {
            padding: 8pt;
            margin-bottom: 5pt;
            border-radius: 3pt;
            font-size: 10pt;
        }

        .nav-item.active {
            background-color: #dbeafe;
            color: #1e40af;
            font-weight: bold;
        }

        /* ==============================================
           MAIN CONTENT
           ============================================== */
        .main-content {
            display: table-cell;
            padding: 20pt;
            padding-left: 30pt;
            vertical-align: top;
        }

        .main-content h1 {
            margin-top: 0;
            color: #1e40af;
            font-size: 24pt;
            margin-bottom: 20pt;
        }

        .main-content h2 {
            font-size: 18pt;
            color: #2563eb;
            margin-top: 30pt;
            margin-bottom: 15pt;
        }

        .main-content p {
            margin-top: 0;
            margin-bottom: 12pt;
        }
    </style>
</head>
<body>
    <div class="page-container">
        <!-- Sidebar -->
        <div class="sidebar">
            <h2>Navigation</h2>
            <div class="nav-item active">Dashboard</div>
            <div class="nav-item">Reports</div>
            <div class="nav-item">Analytics</div>
            <div class="nav-item">Settings</div>
            <div class="nav-item">Help</div>

            <h2 style="margin-top: 30pt;">Quick Stats</h2>
            <div style="font-size: 9pt; color: #666;">
                <p style="margin-bottom: 8pt;"><strong>Total Sales:</strong><br/>$125,000</p>
                <p style="margin-bottom: 8pt;"><strong>New Customers:</strong><br/>45</p>
                <p style="margin-bottom: 0;"><strong>Active Projects:</strong><br/>12</p>
            </div>
        </div>

        <!-- Main Content -->
        <div class="main-content">
            <h1>Dashboard</h1>

            <h2>Overview</h2>
            <p>Welcome to your dashboard. Here you'll find an overview of your key metrics and recent activity.</p>

            <h2>Recent Activity</h2>
            <p>No recent activity to display at this time.</p>

            <h2>Performance Metrics</h2>
            <p>Your performance has been strong this quarter with consistent growth across all key indicators.</p>
        </div>
    </div>
</body>
</html>
```

### Example 2: Three-Column Layout

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Three-Column Layout</title>
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
            text-align: center;
            color: #1e40af;
            font-size: 28pt;
            margin-top: 0;
            margin-bottom: 40pt;
            padding-bottom: 20pt;
            border-bottom: 2pt solid #2563eb;
        }

        /* ==============================================
           THREE-COLUMN LAYOUT
           ============================================== */
        .three-column-container {
            display: table;
            width: 100%;
            border-spacing: 20pt 0;  /* 20pt gap between columns */
        }

        .column {
            display: table-cell;
            width: 33.33%;
            vertical-align: top;
        }

        /* ==============================================
           FEATURE CARDS
           ============================================== */
        .feature-card {
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
            padding: 20pt;
            background-color: white;
            height: 100%;
        }

        .feature-icon {
            width: 50pt;
            height: 50pt;
            background-color: #2563eb;
            border-radius: 50%;
            margin: 0 auto 15pt auto;
            display: table;
        }

        .feature-icon-text {
            display: table-cell;
            vertical-align: middle;
            text-align: center;
            color: white;
            font-size: 24pt;
            font-weight: bold;
        }

        .feature-title {
            text-align: center;
            font-size: 16pt;
            font-weight: bold;
            color: #1e40af;
            margin: 0 0 15pt 0;
        }

        .feature-description {
            text-align: center;
            font-size: 10pt;
            line-height: 1.6;
            color: #666;
            margin: 0;
        }
    </style>
</head>
<body>
    <h1>Our Services</h1>

    <div class="three-column-container">
        <!-- Column 1 -->
        <div class="column">
            <div class="feature-card">
                <div class="feature-icon">
                    <div class="feature-icon-text">1</div>
                </div>
                <h2 class="feature-title">Consulting</h2>
                <p class="feature-description">
                    Expert consulting services to help your business identify opportunities and overcome challenges with proven strategies.
                </p>
            </div>
        </div>

        <!-- Column 2 -->
        <div class="column">
            <div class="feature-card">
                <div class="feature-icon">
                    <div class="feature-icon-text">2</div>
                </div>
                <h2 class="feature-title">Development</h2>
                <p class="feature-description">
                    Custom software development tailored to your specific needs using modern technologies and best practices.
                </p>
            </div>
        </div>

        <!-- Column 3 -->
        <div class="column">
            <div class="feature-card">
                <div class="feature-icon">
                    <div class="feature-icon-text">3</div>
                </div>
                <h2 class="feature-title">Support</h2>
                <p class="feature-description">
                    Ongoing support and maintenance to ensure your systems run smoothly and efficiently at all times.
                </p>
            </div>
        </div>
    </div>

    <!-- Second Row -->
    <div class="three-column-container" style="margin-top: 20pt;">
        <div class="column">
            <div class="feature-card">
                <div class="feature-icon">
                    <div class="feature-icon-text">4</div>
                </div>
                <h2 class="feature-title">Training</h2>
                <p class="feature-description">
                    Comprehensive training programs to empower your team with the skills they need to succeed.
                </p>
            </div>
        </div>

        <div class="column">
            <div class="feature-card">
                <div class="feature-icon">
                    <div class="feature-icon-text">5</div>
                </div>
                <h2 class="feature-title">Strategy</h2>
                <p class="feature-description">
                    Strategic planning services to align your technology initiatives with business objectives.
                </p>
            </div>
        </div>

        <div class="column">
            <div class="feature-card">
                <div class="feature-icon">
                    <div class="feature-icon-text">6</div>
                </div>
                <h2 class="feature-title">Security</h2>
                <p class="feature-description">
                    Robust security solutions to protect your data and systems from evolving threats.
                </p>
            </div>
        </div>
    </div>
</body>
</html>
```

### Example 3: Data Comparison Layout

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Data Comparison</title>
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
            text-align: center;
            color: #1e40af;
            font-size: 24pt;
            margin-top: 0;
            margin-bottom: 30pt;
        }

        /* ==============================================
           TWO-COLUMN COMPARISON
           ============================================== */
        .comparison-container {
            display: table;
            width: 100%;
            border-spacing: 30pt 0;
        }

        .comparison-column {
            display: table-cell;
            width: 50%;
            vertical-align: top;
        }

        .comparison-card {
            border: 2pt solid #d1d5db;
            border-radius: 8pt;
            overflow: hidden;
        }

        .comparison-header {
            background-color: #2563eb;
            color: white;
            padding: 20pt;
            text-align: center;
        }

        .comparison-title {
            font-size: 20pt;
            font-weight: bold;
            margin: 0 0 5pt 0;
        }

        .comparison-price {
            font-size: 32pt;
            font-weight: bold;
            margin: 10pt 0 5pt 0;
        }

        .comparison-period {
            font-size: 12pt;
            opacity: 0.9;
        }

        .comparison-body {
            padding: 25pt;
        }

        .feature-list {
            list-style: none;
            padding: 0;
            margin: 0;
        }

        .feature-item {
            padding: 10pt 0;
            border-bottom: 1pt solid #e5e7eb;
        }

        .feature-item:last-child {
            border-bottom: none;
        }

        .feature-item:before {
            content: '✓ ';
            color: #059669;
            font-weight: bold;
            margin-right: 10pt;
        }

        .feature-item.disabled {
            color: #999;
        }

        .feature-item.disabled:before {
            content: '✗ ';
            color: #dc2626;
        }

        .cta-button {
            display: block;
            background-color: #2563eb;
            color: white;
            text-align: center;
            padding: 12pt;
            border-radius: 5pt;
            font-weight: bold;
            margin-top: 20pt;
            text-decoration: none;
        }

        /* Highlight one option */
        .featured {
            border-color: #2563eb;
            border-width: 3pt;
        }

        .featured .comparison-header {
            background-color: #1e40af;
        }
    </style>
</head>
<body>
    <h1>Choose Your Plan</h1>

    <div class="comparison-container">
        <!-- Basic Plan -->
        <div class="comparison-column">
            <div class="comparison-card">
                <div class="comparison-header">
                    <div class="comparison-title">Basic</div>
                    <div class="comparison-price">$29</div>
                    <div class="comparison-period">per month</div>
                </div>
                <div class="comparison-body">
                    <ul class="feature-list">
                        <li class="feature-item">Up to 10 users</li>
                        <li class="feature-item">5 GB storage</li>
                        <li class="feature-item">Email support</li>
                        <li class="feature-item">Basic analytics</li>
                        <li class="feature-item disabled">Advanced features</li>
                        <li class="feature-item disabled">Priority support</li>
                        <li class="feature-item disabled">Custom branding</li>
                    </ul>
                    <a href="#" class="cta-button">Get Started</a>
                </div>
            </div>
        </div>

        <!-- Pro Plan -->
        <div class="comparison-column">
            <div class="comparison-card featured">
                <div class="comparison-header">
                    <div class="comparison-title">Professional</div>
                    <div class="comparison-price">$99</div>
                    <div class="comparison-period">per month</div>
                </div>
                <div class="comparison-body">
                    <ul class="feature-list">
                        <li class="feature-item">Unlimited users</li>
                        <li class="feature-item">50 GB storage</li>
                        <li class="feature-item">Priority email & chat support</li>
                        <li class="feature-item">Advanced analytics</li>
                        <li class="feature-item">All advanced features</li>
                        <li class="feature-item">24/7 priority support</li>
                        <li class="feature-item">Custom branding</li>
                    </ul>
                    <a href="#" class="cta-button">Get Started</a>
                </div>
            </div>
        </div>
    </div>

    <p style="text-align: center; margin-top: 30pt; color: #666; font-size: 10pt;">
        All plans include a 30-day money-back guarantee. No credit card required for trial.
    </p>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Sidebar Layout

Create a document with:
- Fixed-width sidebar (180-200pt)
- Fluid main content area
- Navigation links in sidebar
- Content in main area

### Exercise 2: Three-Column Feature Grid

Create a features page with:
- Three equal-width columns
- Icons or numbers in each column
- Consistent spacing between columns
- Multiple rows of features

### Exercise 3: Before/After Comparison

Create a comparison layout:
- Two equal columns
- "Before" content in left column
- "After" content in right column
- Visual separation between columns

---

## Common Pitfalls

### ❌ Trying to Use Flexbox

```css
.container {
    display: flex;  /* Not supported in PDF! */
    gap: 20pt;
}
```

✅ **Solution:** Use table-cell

```css
.container {
    display: table;
    width: 100%;
    border-spacing: 20pt 0;
}

.column {
    display: table-cell;
}
```

### ❌ Forgetting Vertical Alignment

```css
.column {
    display: table-cell;
    /* Missing vertical-align */
}
```

✅ **Solution:** Set vertical-align

```css
.column {
    display: table-cell;
    vertical-align: top;  /* Recommended */
}
```

### ❌ Column Widths Don't Add Up to 100%

```css
.column1 { width: 40%; }
.column2 { width: 70%; }  /* Total: 110%! */
```

✅ **Solution:** Ensure total is 100%

```css
.column1 { width: 40%; }
.column2 { width: 60%; }  /* Total: 100% */
```

### ❌ Not Accounting for Padding/Border

```css
* {
    box-sizing: content-box;  /* Default */
}

.column {
    width: 50%;
    padding: 20pt;  /* Adds to width! */
}
```

✅ **Solution:** Use border-box

```css
* {
    box-sizing: border-box;
}

.column {
    width: 50%;  /* Includes padding */
    padding: 20pt;
}
```

### ❌ Inconsistent Column Heights

```css
.column {
    display: table-cell;
    background-color: #f9fafb;
    /* Columns with different content have different heights */
}
```

✅ **Solution:** This is expected behavior with table-cell. If you need equal heights, use background on the container or plan content carefully.

---

## Column Layout Checklist

- [ ] Using display: table and table-cell (not flexbox/grid)
- [ ] vertical-align: top set on columns
- [ ] Column widths add up to 100%
- [ ] box-sizing: border-box applied
- [ ] Appropriate gutter spacing (padding or border-spacing)
- [ ] Content breaks properly if columns are tall
- [ ] Borders/dividers between columns if needed
- [ ] Responsive width strategy (fixed vs percentage)

---

## Best Practices

1. **Use table-cell** - Most reliable for PDF column layouts
2. **Set vertical-align: top** - Prevents unexpected alignment
3. **Apply border-box** - Makes width calculations predictable
4. **Plan for content length** - Columns grow with content
5. **Use consistent gutters** - Typically 15-30pt between columns
6. **Test with real content** - Varying content lengths affect layout
7. **Consider page breaks** - Tall columns will break across pages
8. **Keep it simple** - Avoid deeply nested column structures

---

## Next Steps

Now that you master column layouts:

1. **[Positioning](05_positioning.md)** - Absolute and relative positioning
2. **[Tables](06_tables.md)** - Advanced table layouts
3. **[Headers & Footers](07_headers_footers.md)** - Repeating page elements

---

**Continue learning →** [Positioning](05_positioning.md)
