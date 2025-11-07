---
layout: default
title: style
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;style&gt; : The Embedded Style Element
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---


## Summary

The `<style>` element contains embedded CSS styles within the HTML document. It allows inline definition of CSS rules without requiring external stylesheet files, providing self-contained styling within a single document.

## Usage

The `<style>` element embeds CSS styles that:
- Contains CSS rules directly within the HTML document
- Applies styles to elements using CSS selectors
- Supports all standard CSS properties and selectors
- Can include multiple style rules and selectors
- Processes during document loading and data binding
- Must be placed within the `<head>` element
- Supports media queries for conditional styling
- Provides document-level styling without external files

```html
<head>
    <title>Styled Document</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20pt;
        }

        h1 {
            color: #336699;
            font-size: 24pt;
        }

        .highlight {
            background-color: yellow;
            padding: 5pt;
        }
    </style>
</head>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `type` | string | MIME type of the style content. Default and only supported value: `"text/css"`. |
| `media` | string | Media query for conditional inclusion (e.g., "print", "all"). |
| `hidden` | string | Controls visibility. Set to "hidden" to exclude the styles, or omit to include. |

### Media Attribute Values

The `media` attribute controls when styles are applied:

| Value | Description | Applied to PDF |
|-------|-------------|----------------|
| `print` | Print media type | ✓ Yes (PDFs are print media) |
| `all` | All media types | ✓ Yes |
| `screen` | Screen media type | No (PDFs are not screen) |
| Not specified | Defaults to all media | ✓ Yes |

---

## Notes

### CSS Processing

The `<style>` element processes CSS through the following steps:

1. **Parsing**: CSS content is parsed during loading or data binding
2. **Collection**: Parsed styles are added to a `StyleCollection`
3. **Group**: Styles are wrapped in a `StyleGroup` or `StyleRemoteGroup`
4. **Registration**: Style group is added to the document's `Styles` collection
5. **Application**: Styles are applied during layout using CSS selector matching

### CSS Selector Support

Scryber supports a comprehensive set of CSS selectors:

**Element Selectors**:
```css
body { }        /* Element name */
h1 { }          /* Heading element */
p { }           /* Paragraph element */
```

**Class Selectors**:
```css
.classname { }      /* Class selector */
div.highlight { }   /* Element with class */
```

**ID Selectors**:
```css
#elementid { }      /* ID selector */
```

**Descendant Selectors**:
```css
div p { }           /* Descendant */
body > div { }      /* Direct child */
```

**Attribute Selectors**:
```css
[attr] { }          /* Has attribute */
[attr="value"] { }  /* Attribute equals value */
```

**Pseudo-classes**:
```css
:first-child { }    /* First child */
:last-child { }     /* Last child */
:nth-child(n) { }   /* Nth child */
```

**Combinators**:
```css
div, p { }          /* Multiple selectors */
div + p { }         /* Adjacent sibling */
div ~ p { }         /* General sibling */
```

### Cascade and Specificity

CSS rules follow standard cascade and specificity rules:

1. **Inline styles** (highest specificity)
2. **ID selectors** (#id)
3. **Class selectors** (.class)
4. **Element selectors** (div)
5. **Browser defaults** (lowest specificity)

Later rules override earlier rules when specificity is equal.

### Media Query Support

The `media` attribute filters style inclusion:

```html
<!-- Included in PDF (print media) -->
<style media="print">
    body { margin: 72pt; }
</style>

<!-- Included in PDF (all media) -->
<style media="all">
    h1 { color: blue; }
</style>

<!-- NOT included in PDF (screen only) -->
<style media="screen">
    body { background: white; }
</style>
```

PDFs are considered **print media**, so styles with `media="print"` or `media="all"` are included.

### Type Attribute

The `type` attribute specifies the style language:

```html
<!-- Explicit CSS type (recommended) -->
<style type="text/css">
    body { font-family: Arial; }
</style>

<!-- Default type (CSS assumed) -->
<style>
    body { font-family: Arial; }
</style>
```

Only `type="text/css"` or no type attribute is supported. Other types are ignored.

### Performance Considerations

Embedded styles have performance characteristics:

**Advantages**:
- No external file loading required
- Faster processing (no network or file I/O)
- Self-contained documents
- Useful for dynamic style generation

**Disadvantages**:
- Cannot be shared across documents
- Increases document size
- Not cached separately
- Harder to maintain for large style sets

**Best Practices**:
- Use external stylesheets for shared styles
- Use `<style>` for document-specific overrides
- Keep embedded styles reasonably sized
- Consider combining small external files into embedded styles

### Class Hierarchy

In the Scryber codebase:
- `HTMLStyle` extends `Component`
- Contains `Contents` property for CSS text
- Maintains `StyleCollection` for parsed styles
- Uses `CSSStyleParser` to parse CSS content
- Creates `StyleGroup` or `StyleRemoteGroup` wrapper
- Adds to document's `Styles` collection during data binding

### Data Binding

The `<style>` element supports data binding:

```html
<style>
    body {
        margin: {{model.pageMargin}}pt;
        font-family: {{model.fontFamily}};
    }

    .header {
        background-color: {{model.brandColor}};
    }
</style>
```

Data binding expressions are evaluated before CSS parsing.

---

## Examples

### Basic Embedded Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Basic Styled Document</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 20pt;
        }

        h1 {
            color: #336699;
            font-size: 24pt;
            border-bottom: 2pt solid #336699;
            padding-bottom: 10pt;
        }

        p {
            margin: 10pt 0;
            text-align: justify;
        }
    </style>
</head>
<body>
    <h1>Welcome</h1>
    <p>This is a paragraph with embedded styles.</p>
</body>
</html>
```

### Complete Typography Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Typography Example</title>
    <style type="text/css">
        body {
            font-family: 'Georgia', serif;
            font-size: 12pt;
            line-height: 1.8;
            color: #333333;
        }

        h1, h2, h3, h4, h5, h6 {
            font-family: 'Helvetica', Arial, sans-serif;
            color: #2c3e50;
            margin-top: 20pt;
            margin-bottom: 10pt;
        }

        h1 { font-size: 28pt; }
        h2 { font-size: 22pt; }
        h3 { font-size: 18pt; }
        h4 { font-size: 16pt; }
        h5 { font-size: 14pt; }
        h6 { font-size: 12pt; }

        p {
            margin: 10pt 0;
        }

        strong {
            font-weight: bold;
            color: #000000;
        }

        em {
            font-style: italic;
        }

        code {
            font-family: 'Courier New', monospace;
            background-color: #f5f5f5;
            padding: 2pt 4pt;
            border: 1pt solid #e0e0e0;
        }
    </style>
</head>
<body>
    <h1>Typography Guide</h1>
    <p>This document demonstrates <strong>bold text</strong> and <em>italic text</em>.</p>
    <p>Inline code: <code>var x = 10;</code></p>
</body>
</html>
```

### Layout and Box Model

```html
<!DOCTYPE html>
<html>
<head>
    <title>Layout Example</title>
    <style>
        * {
            box-sizing: border-box;
        }

        body {
            margin: 0;
            padding: 0;
        }

        .container {
            width: 100%;
            padding: 20pt;
        }

        .header {
            background-color: #336699;
            color: white;
            padding: 20pt;
            text-align: center;
        }

        .content {
            padding: 30pt;
            border: 1pt solid #e0e0e0;
            margin: 20pt 0;
        }

        .sidebar {
            width: 30%;
            float: left;
            background-color: #f5f5f5;
            padding: 15pt;
            margin-right: 3%;
        }

        .main {
            width: 67%;
            float: left;
        }

        .footer {
            clear: both;
            background-color: #2c3e50;
            color: white;
            padding: 15pt;
            text-align: center;
            margin-top: 20pt;
        }
    </style>
</head>
<body>
    <div class="header">
        <h1>Document Title</h1>
    </div>
    <div class="container">
        <div class="sidebar">Sidebar content</div>
        <div class="main">Main content</div>
    </div>
    <div class="footer">Footer content</div>
</body>
</html>
```

### Table Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Table Styling</title>
    <style>
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }

        thead {
            background-color: #336699;
            color: white;
        }

        thead th {
            padding: 10pt;
            text-align: left;
            font-weight: bold;
        }

        tbody tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        tbody tr:hover {
            background-color: #e8f4f8;
        }

        tbody td {
            padding: 8pt;
            border-bottom: 1pt solid #e0e0e0;
        }

        tfoot {
            background-color: #f5f5f5;
            font-weight: bold;
        }

        tfoot td {
            padding: 10pt;
            border-top: 2pt solid #336699;
        }
    </style>
</head>
<body>
    <table>
        <thead>
            <tr>
                <th>Product</th>
                <th>Quantity</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Item 1</td>
                <td>10</td>
                <td>$100</td>
            </tr>
            <tr>
                <td>Item 2</td>
                <td>5</td>
                <td>$75</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="2">Total</td>
                <td>$175</td>
            </tr>
        </tfoot>
    </table>
</body>
</html>
```

### Utility Classes

```html
<!DOCTYPE html>
<html>
<head>
    <title>Utility Classes</title>
    <style>
        /* Text alignment */
        .text-left { text-align: left; }
        .text-center { text-align: center; }
        .text-right { text-align: right; }
        .text-justify { text-align: justify; }

        /* Spacing utilities */
        .mt-10 { margin-top: 10pt; }
        .mt-20 { margin-top: 20pt; }
        .mb-10 { margin-bottom: 10pt; }
        .mb-20 { margin-bottom: 20pt; }
        .p-10 { padding: 10pt; }
        .p-20 { padding: 20pt; }

        /* Color utilities */
        .text-primary { color: #336699; }
        .text-success { color: #28a745; }
        .text-warning { color: #ffc107; }
        .text-danger { color: #dc3545; }

        .bg-primary { background-color: #336699; color: white; }
        .bg-light { background-color: #f8f9fa; }
        .bg-dark { background-color: #343a40; color: white; }

        /* Border utilities */
        .border { border: 1pt solid #dee2e6; }
        .border-top { border-top: 1pt solid #dee2e6; }
        .border-bottom { border-bottom: 1pt solid #dee2e6; }
        .rounded { border-radius: 4pt; }

        /* Display utilities */
        .hidden { display: none; }
        .block { display: block; }
        .inline { display: inline; }
        .inline-block { display: inline-block; }
    </style>
</head>
<body>
    <div class="bg-primary p-20 text-center mb-20">
        <h1>Utility Classes</h1>
    </div>

    <p class="text-danger mb-10">Error message</p>
    <p class="text-success">Success message</p>

    <div class="border rounded p-10 mt-20">
        <p>Bordered and rounded content</p>
    </div>
</body>
</html>
```

### Print-Specific Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Print-Optimized Document</title>

    <!-- Always applied -->
    <style>
        body {
            font-family: Arial, sans-serif;
            font-size: 11pt;
        }
    </style>

    <!-- PDF/Print specific -->
    <style media="print">
        body {
            margin: 72pt;
            color: #000000;
        }

        h1 {
            page-break-before: always;
            page-break-after: avoid;
        }

        table {
            page-break-inside: avoid;
        }

        .no-print {
            display: none;
        }

        a {
            color: #000000;
            text-decoration: underline;
        }

        a[href]:after {
            content: " (" attr(href) ")";
            font-size: 9pt;
            color: #666666;
        }
    </style>
</head>
<body>
    <h1>Print-Optimized Report</h1>
    <p>Visit our <a href="https://example.com">website</a> for more information.</p>
    <div class="no-print">This won't appear in PDF</div>
</body>
</html>
```

### Multi-Column Layout

```html
<!DOCTYPE html>
<html>
<head>
    <title>Multi-Column Document</title>
    <style>
        body {
            margin: 30pt;
            font-family: Georgia, serif;
            font-size: 11pt;
        }

        .two-column {
            column-count: 2;
            column-gap: 20pt;
            column-rule: 1pt solid #cccccc;
            text-align: justify;
        }

        .three-column {
            column-count: 3;
            column-gap: 15pt;
            column-rule: 1pt dotted #999999;
        }

        .no-break {
            page-break-inside: avoid;
            column-break-inside: avoid;
        }
    </style>
</head>
<body>
    <h1>Multi-Column Layout</h1>

    <div class="two-column">
        <p>This content flows across two columns with a rule between them...</p>
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit...</p>
    </div>

    <div class="three-column">
        <div class="no-break">
            <h3>Section 1</h3>
            <p>Content that stays together...</p>
        </div>
    </div>
</body>
</html>
```

### Data-Bound Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>{{model.documentTitle}}</title>
    <style>
        body {
            font-family: {{model.fontFamily}};
            font-size: {{model.fontSize}}pt;
            margin: {{model.pageMargin}}pt;
            color: {{model.textColor}};
        }

        .header {
            background-color: {{model.brandColor}};
            color: {{model.headerTextColor}};
            padding: {{model.headerPadding}}pt;
        }

        h1 {
            font-size: {{model.headingSize}}pt;
            color: {{model.brandColor}};
        }

        .highlight {
            background-color: {{model.highlightColor}};
            padding: 5pt;
        }
    </style>
</head>
<body>
    <div class="header">
        <h1>{{model.documentTitle}}</h1>
    </div>
    <p class="highlight">Important information</p>
</body>
</html>
```

### Form Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Form Styling</title>
    <style>
        .form-group {
            margin-bottom: 15pt;
        }

        .form-label {
            display: block;
            font-weight: bold;
            margin-bottom: 5pt;
            color: #333333;
        }

        .form-control {
            width: 100%;
            padding: 8pt;
            border: 1pt solid #cccccc;
            border-radius: 4pt;
            font-size: 11pt;
        }

        .form-control:focus {
            border-color: #336699;
            outline: 2pt solid #e8f4f8;
        }

        .btn {
            padding: 10pt 20pt;
            border: none;
            border-radius: 4pt;
            font-size: 11pt;
            cursor: pointer;
        }

        .btn-primary {
            background-color: #336699;
            color: white;
        }

        .btn-secondary {
            background-color: #6c757d;
            color: white;
        }

        .form-help {
            font-size: 9pt;
            color: #666666;
            margin-top: 3pt;
        }
    </style>
</head>
<body>
    <div class="form-group">
        <label class="form-label">Name:</label>
        <input type="text" class="form-control" />
        <div class="form-help">Enter your full name</div>
    </div>

    <button class="btn btn-primary">Submit</button>
</body>
</html>
```

### Card Component Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Card Components</title>
    <style>
        .card {
            border: 1pt solid #dee2e6;
            border-radius: 8pt;
            margin-bottom: 20pt;
            overflow: hidden;
        }

        .card-header {
            background-color: #f8f9fa;
            padding: 15pt;
            border-bottom: 1pt solid #dee2e6;
            font-weight: bold;
        }

        .card-body {
            padding: 20pt;
        }

        .card-footer {
            background-color: #f8f9fa;
            padding: 10pt 15pt;
            border-top: 1pt solid #dee2e6;
            font-size: 9pt;
            color: #666666;
        }

        .card-title {
            margin: 0 0 10pt 0;
            font-size: 16pt;
            color: #336699;
        }

        .card-text {
            margin: 0;
            line-height: 1.6;
        }
    </style>
</head>
<body>
    <div class="card">
        <div class="card-header">Featured</div>
        <div class="card-body">
            <h3 class="card-title">Card Title</h3>
            <p class="card-text">Card content goes here...</p>
        </div>
        <div class="card-footer">Last updated 3 mins ago</div>
    </div>
</body>
</html>
```

### Badge and Label Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Badges and Labels</title>
    <style>
        .badge {
            display: inline-block;
            padding: 3pt 8pt;
            font-size: 9pt;
            font-weight: bold;
            border-radius: 10pt;
            text-align: center;
        }

        .badge-primary { background-color: #336699; color: white; }
        .badge-success { background-color: #28a745; color: white; }
        .badge-warning { background-color: #ffc107; color: #333; }
        .badge-danger { background-color: #dc3545; color: white; }
        .badge-info { background-color: #17a2b8; color: white; }

        .label {
            display: inline-block;
            padding: 5pt 10pt;
            background-color: #e9ecef;
            border: 1pt solid #dee2e6;
            border-radius: 4pt;
            font-size: 10pt;
        }

        .tag {
            display: inline-block;
            padding: 4pt 8pt;
            margin-right: 5pt;
            background-color: #f8f9fa;
            border: 1pt solid #dee2e6;
            border-radius: 3pt;
            font-size: 9pt;
        }
    </style>
</head>
<body>
    <h1>Status: <span class="badge badge-success">Active</span></h1>
    <p>Priority: <span class="badge badge-warning">High</span></p>
    <p>Tags: <span class="tag">PDF</span> <span class="tag">Document</span> <span class="tag">Report</span></p>
</body>
</html>
```

### Alert and Message Boxes

```html
<!DOCTYPE html>
<html>
<head>
    <title>Alert Boxes</title>
    <style>
        .alert {
            padding: 12pt 15pt;
            margin-bottom: 15pt;
            border: 1pt solid transparent;
            border-radius: 4pt;
        }

        .alert-info {
            background-color: #d1ecf1;
            border-color: #bee5eb;
            color: #0c5460;
        }

        .alert-success {
            background-color: #d4edda;
            border-color: #c3e6cb;
            color: #155724;
        }

        .alert-warning {
            background-color: #fff3cd;
            border-color: #ffeaa7;
            color: #856404;
        }

        .alert-danger {
            background-color: #f8d7da;
            border-color: #f5c6cb;
            color: #721c24;
        }

        .alert-heading {
            font-weight: bold;
            margin-bottom: 5pt;
        }
    </style>
</head>
<body>
    <div class="alert alert-info">
        <div class="alert-heading">Information</div>
        This is an informational message.
    </div>

    <div class="alert alert-success">
        <div class="alert-heading">Success!</div>
        Operation completed successfully.
    </div>

    <div class="alert alert-warning">
        <div class="alert-heading">Warning</div>
        Please review this information carefully.
    </div>

    <div class="alert alert-danger">
        <div class="alert-heading">Error</div>
        An error occurred during processing.
    </div>
</body>
</html>
```

### Invoice Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Invoice Styling</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            font-size: 10pt;
            margin: 30pt;
        }

        .invoice-header {
            display: flex;
            justify-content: space-between;
            margin-bottom: 30pt;
            padding-bottom: 15pt;
            border-bottom: 2pt solid #333333;
        }

        .company-info {
            font-size: 12pt;
        }

        .company-name {
            font-size: 20pt;
            font-weight: bold;
            color: #336699;
        }

        .invoice-title {
            font-size: 28pt;
            font-weight: bold;
            color: #333333;
        }

        .invoice-table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }

        .invoice-table thead {
            background-color: #333333;
            color: white;
        }

        .invoice-table th {
            padding: 10pt;
            text-align: left;
        }

        .invoice-table td {
            padding: 8pt;
            border-bottom: 1pt solid #e0e0e0;
        }

        .invoice-total {
            text-align: right;
            font-size: 14pt;
            font-weight: bold;
            margin-top: 20pt;
            padding-top: 10pt;
            border-top: 2pt solid #333333;
        }

        .payment-terms {
            margin-top: 30pt;
            padding: 15pt;
            background-color: #f5f5f5;
            border-left: 4pt solid #336699;
        }
    </style>
</head>
<body>
    <div class="invoice-header">
        <div class="company-info">
            <div class="company-name">ACME Corporation</div>
            <div>123 Business Street</div>
            <div>City, State 12345</div>
        </div>
        <div class="invoice-title">INVOICE</div>
    </div>

    <table class="invoice-table">
        <thead>
            <tr>
                <th>Description</th>
                <th>Quantity</th>
                <th>Price</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Product 1</td>
                <td>10</td>
                <td>$50.00</td>
                <td>$500.00</td>
            </tr>
        </tbody>
    </table>

    <div class="invoice-total">
        Total: $500.00
    </div>

    <div class="payment-terms">
        <strong>Payment Terms:</strong> Net 30 days
    </div>
</body>
</html>
```

### Resume/CV Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Resume</title>
    <style>
        body {
            font-family: 'Calibri', Arial, sans-serif;
            font-size: 11pt;
            margin: 40pt;
            line-height: 1.5;
        }

        .header {
            text-align: center;
            margin-bottom: 30pt;
            padding-bottom: 15pt;
            border-bottom: 3pt solid #336699;
        }

        .name {
            font-size: 28pt;
            font-weight: bold;
            color: #2c3e50;
            margin-bottom: 5pt;
        }

        .contact-info {
            font-size: 10pt;
            color: #666666;
        }

        .section {
            margin-bottom: 25pt;
        }

        .section-title {
            font-size: 16pt;
            font-weight: bold;
            color: #336699;
            border-bottom: 2pt solid #e0e0e0;
            padding-bottom: 5pt;
            margin-bottom: 15pt;
        }

        .experience-item {
            margin-bottom: 15pt;
        }

        .job-title {
            font-weight: bold;
            font-size: 12pt;
        }

        .company {
            font-style: italic;
            color: #666666;
        }

        .date-range {
            float: right;
            color: #999999;
            font-size: 10pt;
        }

        .skills {
            display: flex;
            flex-wrap: wrap;
        }

        .skill-item {
            background-color: #e8f4f8;
            padding: 5pt 10pt;
            margin: 3pt;
            border-radius: 3pt;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <div class="header">
        <div class="name">John Doe</div>
        <div class="contact-info">
            john.doe@example.com | (555) 123-4567 | linkedin.com/in/johndoe
        </div>
    </div>

    <div class="section">
        <div class="section-title">Experience</div>
        <div class="experience-item">
            <div class="date-range">2020 - Present</div>
            <div class="job-title">Senior Developer</div>
            <div class="company">Tech Company Inc.</div>
            <p>Responsibilities and achievements...</p>
        </div>
    </div>

    <div class="section">
        <div class="section-title">Skills</div>
        <div class="skills">
            <span class="skill-item">C#</span>
            <span class="skill-item">JavaScript</span>
            <span class="skill-item">SQL</span>
            <span class="skill-item">PDF Generation</span>
        </div>
    </div>
</body>
</html>
```

---

## See Also

- [link](/reference/htmltags/link.html) - Link element for external stylesheets
- [head](/reference/htmltags/head.html) - Head element (container for style elements)
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [CSS Selectors](/reference/styles/selectors.html) - CSS selector syntax
- [CSS Properties](/reference/styles/properties.html) - Supported CSS properties
- [Style Collection](/reference/styles/collection.html) - Document style collection
- [CSS Parser](/reference/styles/parser.html) - CSS parsing details
- [Media Queries](/reference/styles/media.html) - Media query support

---
