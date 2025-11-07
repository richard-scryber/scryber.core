---
layout: default
title: Headers & Footers
nav_order: 7
parent: Layout & Positioning
parent_url: /learning/04-layout/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Headers & Footers

Master repeating page headers and footers for professional, consistent PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Create page headers that repeat on every page
- Add page footers with consistent information
- Include page numbers in headers/footers
- Add dynamic content (dates, titles)
- Style headers and footers appropriately
- Control header/footer on first/last pages
- Understand Scryber-specific header/footer implementation

---

## Header & Footer Concepts

Headers and footers are repeating elements that appear on every page (or specific pages) of your document:

**Headers:**
- Appear at the top of each page
- Within the top margin area
- Common content: Logo, document title, section name

**Footers:**
- Appear at the bottom of each page
- Within the bottom margin area
- Common content: Page numbers, date, copyright

---

## @page Margin Boxes (CSS Paged Media)

### Basic Structure

```css
@page {
    size: Letter;
    margin: 1in;

    /* Header area */
    @top-left { content: "Company Name"; }
    @top-center { content: "Document Title"; }
    @top-right { content: "Page " counter(page); }

    /* Footer area */
    @bottom-left { content: "© 2025 Company"; }
    @bottom-center { content: "Confidential"; }
    @bottom-right { content: counter(page) " of " counter(pages); }
}
```

**Note:** Support for @page margin boxes varies. Scryber uses specific HTML structure for headers/footers (see below).

---

## Scryber Headers & Footers

Scryber uses dedicated HTML elements within sections for headers and footers.

### Basic Structure

```html
<body>
    <!-- Header (repeats on every page) -->
    <header>
        <p>This appears at the top of every page</p>
    </header>

    <!-- Main content -->
    <div>
        <p>Page content goes here</p>
    </div>

    <!-- Footer (repeats on every page) -->
    <footer>
        <p>This appears at the bottom of every page</p>
    </footer>
</body>
```

### Styled Header Example

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page {
            size: Letter;
            margin: 1.25in 1in 1in 1in;  /* Extra top margin for header */
        }

        header {
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 10pt;
            margin-bottom: 20pt;
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
            vertical-align: middle;
            text-align: right;
        }

        footer {
            border-top: 1pt solid #d1d5db;
            padding-top: 10pt;
            margin-top: 20pt;
            font-size: 9pt;
            color: #666;
        }
    </style>
</head>
<body>
    <header>
        <div class="header-content">
            <div class="header-left">
                <strong>Company Name</strong>
            </div>
            <div class="header-right">
                Document Title
            </div>
        </div>
    </header>

    <!-- Content -->
    <div>
        <p>Your document content here...</p>
    </div>

    <footer>
        <p style="text-align: center; margin: 0;">
            © 2025 Company Name | Page <span class="page-number"></span>
        </p>
    </footer>
</body>
</html>
```

---

## Page Numbers

### Using CSS Counters (If Supported)

```css
@page {
    @bottom-right {
        content: "Page " counter(page) " of " counter(pages);
    }
}
```

### Using Scryber Page Numbering

Scryber provides page numbering through specific elements or data binding:

{% raw %}
```html
<footer>
    <p>Page {{@page}} of {{@pages}}</p>
</footer>
```
{% endraw %}

Or using dedicated page number elements (check Scryber documentation for exact syntax):

```html
<footer>
    <p>Page <span class="page-number"></span> of <span class="page-count"></span></p>
</footer>
```

---

## Dynamic Content in Headers/Footers

### Document Title

{% raw %}
```html
<header>
    <div class="header-content">
        <div class="header-left">
            <strong>{{model.documentTitle}}</strong>
        </div>
        <div class="header-right">
            {{model.date}}
        </div>
    </div>
</header>
```
{% endraw %}

### Date and Time

{% raw %}
```html
<footer>
    <p>Generated: {{format(model.generatedDate, 'MMMM dd, yyyy')}}</p>
</footer>
```
{% endraw %}

---

## Practical Examples

### Example 1: Simple Header and Footer

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Document with Header and Footer</title>
    <style>
        @page {
            size: Letter;
            margin: 1.25in 1in 1in 1in;
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
           HEADER
           ============================================== */
        header {
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 15pt;
            margin-bottom: 30pt;
        }

        .company-name {
            font-size: 18pt;
            font-weight: bold;
            color: #1e40af;
        }

        /* ==============================================
           CONTENT
           ============================================== */
        h1 {
            color: #1e40af;
            margin-top: 0;
            margin-bottom: 20pt;
        }

        p {
            margin-bottom: 12pt;
        }

        /* ==============================================
           FOOTER
           ============================================== */
        footer {
            border-top: 1pt solid #d1d5db;
            padding-top: 15pt;
            margin-top: 40pt;
            font-size: 9pt;
            color: #666;
            text-align: center;
        }

        footer p {
            margin: 5pt 0;
        }
    </style>
</head>
<body>
    <!-- Header (repeats on every page) -->
    <header>
        <div class="company-name">Acme Corporation</div>
        <div style="font-size: 10pt; color: #666;">Annual Report 2024</div>
    </header>

    <!-- Content -->
    <h1>Executive Summary</h1>
    <p>This report provides an overview of our financial performance and strategic initiatives for the fiscal year 2024.</p>
    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>

    <!-- Add enough content to span multiple pages -->
    <h1>Financial Performance</h1>
    <p>Our revenue grew by 15% year-over-year, demonstrating strong market demand for our products and services.</p>
    <p>Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>

    <!-- Footer (repeats on every page) -->
    <footer>
        <p>© 2025 Acme Corporation | Confidential</p>
        <p>Page <span class="page-number"></span></p>
    </footer>
</body>
</html>
```

### Example 2: Advanced Header with Logo

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Professional Document</title>
    <style>
        @page {
            size: Letter;
            margin: 1.5in 1in 1in 1in;
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
           HEADER
           ============================================== */
        header {
            position: relative;
            height: 70pt;
            border-bottom: 3pt solid #2563eb;
            padding-bottom: 15pt;
            margin-bottom: 30pt;
        }

        .logo {
            position: absolute;
            top: 0;
            left: 0;
            width: 60pt;
            height: 60pt;
            background-color: #2563eb;
            border-radius: 5pt;
            color: white;
            font-size: 30pt;
            font-weight: bold;
            display: table;
        }

        .logo-text {
            display: table-cell;
            vertical-align: middle;
            text-align: center;
        }

        .header-info {
            position: absolute;
            top: 5pt;
            left: 75pt;
        }

        .company-name {
            font-size: 20pt;
            font-weight: bold;
            color: #1e40af;
            margin: 0 0 5pt 0;
        }

        .document-title {
            font-size: 12pt;
            color: #666;
            margin: 0;
        }

        .header-meta {
            position: absolute;
            top: 5pt;
            right: 0;
            text-align: right;
            font-size: 9pt;
            color: #666;
        }

        .header-meta p {
            margin: 0 0 3pt 0;
        }

        /* ==============================================
           CONTENT
           ============================================== */
        h1 {
            color: #1e40af;
            font-size: 24pt;
            margin-top: 0;
            margin-bottom: 20pt;
        }

        h2 {
            color: #2563eb;
            font-size: 18pt;
            margin-top: 30pt;
            margin-bottom: 15pt;
        }

        p {
            margin-bottom: 12pt;
        }

        /* ==============================================
           FOOTER
           ============================================== */
        footer {
            display: table;
            width: 100%;
            border-top: 1pt solid #d1d5db;
            padding-top: 15pt;
            margin-top: 40pt;
            font-size: 9pt;
            color: #666;
        }

        .footer-left {
            display: table-cell;
            width: 50%;
            vertical-align: middle;
        }

        .footer-right {
            display: table-cell;
            width: 50%;
            text-align: right;
            vertical-align: middle;
        }
    </style>
</head>
<body>
    <!-- Header -->
    <header>
        <div class="logo">
            <div class="logo-text">A</div>
        </div>

        <div class="header-info">
            <div class="company-name">Acme Corporation</div>
            <div class="document-title">Quarterly Business Review</div>
        </div>

        <div class="header-meta">
            <p><strong>Q4 2024</strong></p>
            <p>January 15, 2025</p>
            <p>Confidential</p>
        </div>
    </header>

    <!-- Content -->
    <h1>Executive Summary</h1>
    <p>This quarterly review provides an analysis of our business performance and strategic initiatives.</p>

    <h2>Key Highlights</h2>
    <ul>
        <li>Revenue growth of 18% year-over-year</li>
        <li>Expanded operations to two new markets</li>
        <li>Launched five new product lines</li>
        <li>Customer satisfaction increased by 25%</li>
    </ul>

    <h2>Financial Performance</h2>
    <p>Our financial results demonstrate strong growth across all key metrics.</p>
    <p>Total revenue reached $3.5M in Q4, exceeding our target by 12%.</p>

    <!-- Footer -->
    <footer>
        <div class="footer-left">
            <p>© 2025 Acme Corporation. All rights reserved.</p>
        </div>
        <div class="footer-right">
            <p>Page <span class="page-number"></span> of <span class="page-count"></span></p>
        </div>
    </footer>
</body>
</html>
```

### Example 3: Multi-Section with Different Headers

```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Multi-Section Document</title>
    <style>
        @page {
            size: Letter;
            margin: 1.25in 1in 1in 1in;
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
           DEFAULT HEADER
           ============================================== */
        header {
            display: table;
            width: 100%;
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 15pt;
            margin-bottom: 30pt;
            font-size: 10pt;
        }

        .header-left {
            display: table-cell;
            width: 70%;
            vertical-align: middle;
        }

        .header-right {
            display: table-cell;
            width: 30%;
            text-align: right;
            vertical-align: middle;
            color: #666;
        }

        .section-name {
            font-weight: bold;
            color: #1e40af;
            font-size: 12pt;
        }

        /* ==============================================
           CONTENT
           ============================================== */
        .section {
            page-break-before: always;
        }

        .section:first-child {
            page-break-before: auto;
        }

        h1 {
            color: #1e40af;
            font-size: 24pt;
            margin-top: 0;
            margin-bottom: 20pt;
        }

        h2 {
            color: #2563eb;
            font-size: 18pt;
            margin-top: 25pt;
            margin-bottom: 15pt;
        }

        p {
            margin-bottom: 12pt;
        }

        /* ==============================================
           FOOTER
           ============================================== */
        footer {
            border-top: 1pt solid #d1d5db;
            padding-top: 15pt;
            margin-top: 40pt;
            font-size: 9pt;
            color: #666;
            text-align: center;
        }
    </style>
</head>
<body>
    <!-- Section 1: Introduction -->
    <div class="section">
        <header>
            <div class="header-left">
                <div style="color: #666;">Annual Report 2024</div>
                <div class="section-name">Introduction</div>
            </div>
            <div class="header-right">
                Acme Corporation
            </div>
        </header>

        <h1>Introduction</h1>
        <p>Welcome to our 2024 annual report. This document provides a comprehensive overview of our performance.</p>

        <h2>About This Report</h2>
        <p>This report covers the fiscal year ending December 31, 2024.</p>

        <footer>
            <p>© 2025 Acme Corporation | Page <span class="page-number"></span></p>
        </footer>
    </div>

    <!-- Section 2: Financial Performance -->
    <div class="section">
        <header>
            <div class="header-left">
                <div style="color: #666;">Annual Report 2024</div>
                <div class="section-name">Financial Performance</div>
            </div>
            <div class="header-right">
                Acme Corporation
            </div>
        </header>

        <h1>Financial Performance</h1>
        <p>Our financial results demonstrate strong growth and profitability.</p>

        <h2>Revenue Analysis</h2>
        <p>Total revenue increased by 15% to reach $12.5 million.</p>

        <footer>
            <p>© 2025 Acme Corporation | Page <span class="page-number"></span></p>
        </footer>
    </div>

    <!-- Section 3: Conclusion -->
    <div class="section">
        <header>
            <div class="header-left">
                <div style="color: #666;">Annual Report 2024</div>
                <div class="section-name">Conclusion</div>
            </div>
            <div class="header-right">
                Acme Corporation
            </div>
        </header>

        <h1>Conclusion</h1>
        <p>We enter 2025 with strong momentum and clear strategic direction.</p>

        <footer>
            <p>© 2025 Acme Corporation | Page <span class="page-number"></span></p>
        </footer>
    </div>
</body>
</html>
```

---

## Try It Yourself

### Exercise 1: Basic Header/Footer

Create a document with:
- Simple header with company name
- Simple footer with page number
- Test with multi-page content

### Exercise 2: Professional Layout

Create a document with:
- Header with logo, title, and date
- Footer with copyright and page numbers
- Content that spans multiple pages

### Exercise 3: Section-Based Headers

Create a document with:
- Different headers for different sections
- Section name in header
- Consistent footer across all sections

---

## Common Pitfalls

### ❌ Forgetting Extra Margin for Headers

```css
@page {
    margin: 1in;  /* Not enough space for header */
}

header {
    /* Header overlaps content */
}
```

✅ **Solution:** Increase top margin

```css
@page {
    margin: 1.5in 1in 1in 1in;  /* Extra top margin */
}
```

### ❌ Not Testing Multi-Page

```html
<!-- Short content doesn't show header/footer repeating -->
<p>Short content</p>
```

✅ **Solution:** Test with enough content

```html
<!-- Add enough content to span multiple pages -->
<p>Content that spans...</p>
<!-- Repeat many times -->
```

### ❌ Complex Positioned Headers

```css
header {
    position: absolute;  /* May not repeat correctly */
}
```

✅ **Solution:** Use normal flow

```css
header {
    /* Normal flow - will repeat on each page */
    border-bottom: 2pt solid #2563eb;
    padding-bottom: 15pt;
}
```

### ❌ Inconsistent Header/Footer Styling

```html
<!-- Different styles on each page -->
<header style="border-bottom: 1pt solid black;">...</header>
<!-- Later -->
<header style="border-bottom: 2pt solid blue;">...</header>
```

✅ **Solution:** Use consistent CSS

```css
header {
    border-bottom: 2pt solid #2563eb;
}
```

### ❌ Forgetting Page Number Elements

```html
<footer>
    <p>Page  of </p>  <!-- Empty! -->
</footer>
```

✅ **Solution:** Include proper page number elements

```html
<footer>
    <p>Page <span class="page-number"></span> of <span class="page-count"></span></p>
</footer>
```

---

## Header & Footer Checklist

- [ ] Adequate page margins (extra for header/footer)
- [ ] Header defined with consistent styling
- [ ] Footer defined with consistent styling
- [ ] Page numbers included (if appropriate)
- [ ] Dynamic content properly bound (dates, titles)
- [ ] Tested with multi-page content
- [ ] Borders/dividers between header/footer and content
- [ ] Font sizes appropriate (typically smaller than body)

---

## Best Practices

1. **Increase page margins** - Provide space for headers/footers
2. **Keep headers simple** - Company name, document title, section
3. **Always include page numbers** - Essential for multi-page documents
4. **Use borders to separate** - Visual distinction from content
5. **Smaller font sizes** - 9-10pt for footers, 10-11pt for headers
6. **Test with real content** - Ensure proper repetition
7. **Consistent styling** - Same header/footer across all pages
8. **Consider printing** - Headers/footers respect print margins

---

## Next Steps

Now that you master headers and footers:

1. **[Layout Best Practices](08_layout_best_practices.md)** - Professional layout patterns
2. **[Typography & Fonts](/learning/05-typography/)** - Advanced text styling
3. **[Practical Applications](/learning/08-practical/)** - Real-world documents

---

**Continue learning →** [Layout Best Practices](08_layout_best_practices.md)
