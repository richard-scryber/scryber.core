---
layout: default
title: media
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @media : The Media Query Attribute

The `media` attribute specifies media queries that determine when styles or resources should be applied. Used primarily with `<link>` and `<style>` elements, it enables conditional CSS loading based on output characteristics. In PDF generation context, it allows creating print-optimized styles and responsive layouts tailored for different page sizes.

## Usage

The `media` attribute defines media-specific conditions:
- Specifies when linked stylesheets or style blocks should apply
- Uses CSS media query syntax for conditional styling
- Common for `print` vs `screen` differentiation
- Supports page size, orientation, and dimension queries
- In PDF context, primarily used with `print` media type
- Supports data binding for dynamic media selection

```html
<!-- Print-specific stylesheet -->
<link rel="stylesheet" href="print.css" media="print" />

<!-- Screen-specific stylesheet -->
<link rel="stylesheet" href="screen.css" media="screen" />

<!-- All media types (default) -->
<link rel="stylesheet" href="styles.css" media="all" />

<!-- Orientation-specific -->
<link rel="stylesheet" href="landscape.css" media="print and (orientation: landscape)" />

<!-- Page size-specific -->
<style media="print">
    @page {
        size: A4;
        margin: 2cm;
    }
</style>

<!-- Dynamic media -->
<link rel="stylesheet" href="{{model.stylesheetUrl}}" media="{{model.mediaQuery}}" />
```

---

## Supported Elements

The `media` attribute is used with:

### Link Element
- `<link>` - External stylesheet loading with media conditions (primary use)

### Style Element
- `<style>` - Inline CSS with media conditions

---

## Binding Values

The `media` attribute supports data binding for dynamic media queries:

```html
<!-- Dynamic media query -->
<link rel="stylesheet" href="{{model.stylePath}}" media="{{model.targetMedia}}" />

<!-- Conditional media type -->
<link rel="stylesheet" href="styles.css"
      media="{{model.isPrint ? 'print' : 'screen'}}" />

<!-- Dynamic page size -->
<style media="{{model.mediaQuery}}">
    @page {
        size: {{model.pageSize}};
        margin: {{model.pageMargin}};
    }
</style>

<!-- Repeating stylesheets with different media -->
<template data-bind="{{model.stylesheets}}">
    <link rel="stylesheet" href="{{.url}}" media="{{.media}}" />
</template>

<!-- Configuration-based media -->
<link rel="stylesheet" href="layout.css"
      media="print and (orientation: {{model.orientation}})" />
```

**Data Model Example:**
```json
{
  "stylePath": "custom.css",
  "targetMedia": "print",
  "isPrint": true,
  "mediaQuery": "print and (orientation: portrait)",
  "pageSize": "A4",
  "pageMargin": "2cm",
  "stylesheets": [
    {
      "url": "base.css",
      "media": "all"
    },
    {
      "url": "print.css",
      "media": "print"
    }
  ],
  "orientation": "landscape"
}
```

---

## Notes

### Media Types

Standard media types for PDF generation:

```html
<!-- Print media (most common for PDF) -->
<link rel="stylesheet" href="print.css" media="print" />

<!-- Screen media (generally not applicable to PDF) -->
<link rel="stylesheet" href="screen.css" media="screen" />

<!-- All media types (default) -->
<link rel="stylesheet" href="common.css" media="all" />
<link rel="stylesheet" href="common.css" />  <!-- Same as media="all" -->
```

**PDF Context:**
- PDFs are considered `print` media
- `screen` media queries typically don't apply
- `all` applies to both print and screen
- Use `print` for PDF-specific styles

### Print Media Queries

Common print-specific media queries:

```html
<!-- Basic print stylesheet -->
<link rel="stylesheet" href="print.css" media="print" />

<!-- Portrait orientation -->
<link rel="stylesheet" href="portrait.css"
      media="print and (orientation: portrait)" />

<!-- Landscape orientation -->
<link rel="stylesheet" href="landscape.css"
      media="print and (orientation: landscape)" />

<!-- Minimum width -->
<link rel="stylesheet" href="wide.css"
      media="print and (min-width: 800pt)" />

<!-- Maximum width -->
<link rel="stylesheet" href="narrow.css"
      media="print and (max-width: 600pt)" />

<!-- Specific dimensions -->
<link rel="stylesheet" href="a4.css"
      media="print and (width: 210mm) and (height: 297mm)" />
```

### Page Size Media Features

Target specific page dimensions:

```html
<!-- A4 size (210mm × 297mm) -->
<style media="print and (width: 210mm) and (height: 297mm)">
    body {
        font-size: 11pt;
    }
</style>

<!-- Letter size (8.5in × 11in) -->
<style media="print and (width: 8.5in) and (height: 11in)">
    body {
        font-size: 11pt;
    }
</style>

<!-- Large format -->
<style media="print and (min-width: 11in)">
    body {
        font-size: 14pt;
    }
</style>
```

### Orientation Media Queries

Handle portrait vs landscape:

```html
<!-- Portrait-specific styles -->
<style media="print and (orientation: portrait)">
    .page-header {
        height: 100pt;
    }

    table {
        width: 100%;
        font-size: 10pt;
    }
</style>

<!-- Landscape-specific styles -->
<style media="print and (orientation: landscape)">
    .page-header {
        height: 80pt;
    }

    table {
        width: 100%;
        font-size: 12pt;
    }
}
</style>
```

### Multiple Media Queries

Combine multiple conditions:

```html
<!-- AND operator (both conditions must be true) -->
<link rel="stylesheet" href="styles.css"
      media="print and (orientation: landscape) and (min-width: 800pt)" />

<!-- Comma (OR operator - any condition can be true) -->
<link rel="stylesheet" href="styles.css"
      media="print, screen" />

<link rel="stylesheet" href="large.css"
      media="(min-width: 800pt), (orientation: landscape)" />

<!-- NOT operator -->
<link rel="stylesheet" href="not-small.css"
      media="not (max-width: 600pt)" />

<!-- Complex combination -->
<link rel="stylesheet" href="complex.css"
      media="print and (orientation: landscape) and (min-width: 800pt),
             print and (min-height: 1000pt)" />
```

### Resolution Media Queries

Target different print resolutions:

```html
<!-- High resolution printing -->
<style media="print and (min-resolution: 300dpi)">
    img {
        image-rendering: high-quality;
    }
</style>

<!-- Standard resolution -->
<style media="print and (max-resolution: 150dpi)">
    img {
        image-rendering: auto;
    }
</style>

<!-- Alternative resolution units -->
<style media="print and (min-resolution: 2dppx)">
    /* Styles for high-density output */
</style>
```

### Color vs Monochrome

Differentiate between color and monochrome output:

```html
<!-- Color printing -->
<style media="print and (color)">
    .highlight {
        background-color: yellow;
        color: black;
    }
</style>

<!-- Monochrome printing -->
<style media="print and (monochrome)">
    .highlight {
        background-color: transparent;
        border: 2pt solid black;
        font-weight: bold;
    }
</style>

<!-- Specific color depth -->
<style media="print and (min-color: 8)">
    /* 8-bit color or higher */
</style>
```

### Default Behavior

When no media attribute is specified:

```html
<!-- No media specified - applies to all media -->
<link rel="stylesheet" href="styles.css" />

<!-- Explicitly stating all media (same result) -->
<link rel="stylesheet" href="styles.css" media="all" />
```

### Media Attribute vs @media Rule

Two ways to specify media conditions:

```html
<!-- Method 1: media attribute on link -->
<link rel="stylesheet" href="print.css" media="print" />

<!-- Method 2: @media rule in CSS -->
<style>
    @media print {
        body {
            font-size: 12pt;
        }
    }
</style>

<!-- Method 3: Combined approach -->
<link rel="stylesheet" href="styles.css" media="print" />
<!-- styles.css can also contain @media rules -->
```

### Invalid Media Queries

Handle invalid queries gracefully:

```html
<!-- Valid -->
<link rel="stylesheet" href="valid.css" media="print" />

<!-- Invalid media type (ignored) -->
<link rel="stylesheet" href="invalid.css" media="invalid-type" />

<!-- Malformed query (may be ignored) -->
<link rel="stylesheet" href="malformed.css" media="prin" />

<!-- Always test your media queries -->
```

### PDF-Specific Considerations

For PDF generation with Scryber:

1. **Use `print` media type** for PDF-specific styles
2. **Avoid `screen` media** as PDFs are print context
3. **Page size queries** are useful for multi-format PDFs
4. **Orientation queries** help with layout variations
5. **`@page` rules** should be in `print` media

```html
<!-- Recommended for PDF -->
<style media="print">
    @page {
        size: A4 portrait;
        margin: 2cm;
    }

    body {
        font-family: 'Times New Roman', serif;
        font-size: 11pt;
        line-height: 1.5;
    }

    .no-print {
        display: none;
    }
</style>
```

---

## Examples

### Basic Print Stylesheet

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Document with Print Styles</title>

    <!-- Common styles for all media -->
    <link rel="stylesheet" href="common.css" media="all" />

    <!-- Print-specific styles -->
    <link rel="stylesheet" href="print.css" media="print" />

    <!-- Screen-specific styles (not used in PDF) -->
    <link rel="stylesheet" href="screen.css" media="screen" />
</head>
<body>
    <h1>Document Title</h1>
    <p>This document has different styles for print and screen.</p>
</body>
</html>
```

### Inline Print Styles

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Print-Optimized Document</title>

    <style media="print">
        @page {
            size: A4;
            margin: 2.5cm;
        }

        body {
            font-family: 'Times New Roman', serif;
            font-size: 11pt;
            color: black;
        }

        h1 {
            font-size: 18pt;
            margin-bottom: 20pt;
            page-break-after: avoid;
        }

        h2 {
            font-size: 14pt;
            margin-top: 15pt;
            page-break-after: avoid;
        }

        .no-print {
            display: none;
        }

        a {
            color: black;
            text-decoration: none;
        }

        a[href]:after {
            content: " (" attr(href) ")";
            font-size: 9pt;
            color: #666;
        }
    </style>
</head>
<body>
    <h1>Professional Report</h1>

    <div class="no-print">
        <p>This section only appears on screen, not in print.</p>
    </div>

    <h2>Introduction</h2>
    <p>
        This document is optimized for printing.
        Visit our <a href="https://example.com">website</a> for more information.
    </p>
</body>
</html>
```

### Orientation-Specific Styles

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Orientation-Aware Document</title>

    <!-- Portrait styles -->
    <style media="print and (orientation: portrait)">
        @page {
            size: A4 portrait;
            margin: 2cm;
        }

        .main-content {
            column-count: 1;
        }

        table {
            font-size: 9pt;
        }
    </style>

    <!-- Landscape styles -->
    <style media="print and (orientation: landscape)">
        @page {
            size: A4 landscape;
            margin: 1.5cm;
        }

        .main-content {
            column-count: 2;
            column-gap: 20pt;
        }

        table {
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <div class="main-content">
        <h1>Flexible Layout</h1>
        <p>This layout adapts to page orientation.</p>

        <table style="width: 100%; border-collapse: collapse;">
            <thead>
                <tr>
                    <th style="border: 1pt solid #000; padding: 5pt;">Column 1</th>
                    <th style="border: 1pt solid #000; padding: 5pt;">Column 2</th>
                    <th style="border: 1pt solid #000; padding: 5pt;">Column 3</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style="border: 1pt solid #000; padding: 5pt;">Data 1</td>
                    <td style="border: 1pt solid #000; padding: 5pt;">Data 2</td>
                    <td style="border: 1pt solid #000; padding: 5pt;">Data 3</td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
</html>
```

### Page Size-Specific Styles

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Multi-Format Document</title>

    <!-- A4 size styles -->
    <style media="print and (width: 210mm)">
        @page {
            size: A4;
            margin: 2cm;
        }

        body {
            font-size: 11pt;
        }

        .content-wrapper {
            width: 170mm;
        }
    </style>

    <!-- Letter size styles -->
    <style media="print and (width: 8.5in)">
        @page {
            size: letter;
            margin: 1in;
        }

        body {
            font-size: 12pt;
        }

        .content-wrapper {
            width: 6.5in;
        }
    </style>

    <!-- Legal size styles -->
    <style media="print and (width: 8.5in) and (height: 14in)">
        @page {
            size: legal;
            margin: 1in;
        }

        body {
            font-size: 11pt;
            line-height: 1.6;
        }
    </style>
</head>
<body>
    <div class="content-wrapper">
        <h1>Adaptive Document</h1>
        <p>This document adapts its layout based on page size.</p>
    </div>
</body>
</html>
```

### Color vs Monochrome

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Color-Aware Document</title>

    <!-- Color printing styles -->
    <style media="print and (color)">
        .warning {
            background-color: #fff3cd;
            border-left: 4pt solid #ffc107;
            padding: 10pt;
            color: #856404;
        }

        .success {
            background-color: #d4edda;
            border-left: 4pt solid #28a745;
            padding: 10pt;
            color: #155724;
        }

        .error {
            background-color: #f8d7da;
            border-left: 4pt solid #dc3545;
            padding: 10pt;
            color: #721c24;
        }

        h1 {
            color: #336699;
        }
    </style>

    <!-- Monochrome printing styles -->
    <style media="print and (monochrome)">
        .warning, .success, .error {
            background-color: transparent;
            border: 2pt solid black;
            padding: 10pt;
            font-weight: bold;
        }

        .warning:before {
            content: "⚠ WARNING: ";
        }

        .success:before {
            content: "✓ SUCCESS: ";
        }

        .error:before {
            content: "✗ ERROR: ";
        }

        h1 {
            color: black;
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <h1>Status Report</h1>

    <div class="success">
        All systems operating normally.
    </div>

    <div class="warning">
        Minor performance degradation detected.
    </div>

    <div class="error">
        Critical failure in backup system.
    </div>
</body>
</html>
```

### Wide vs Narrow Layouts

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Responsive Print Layout</title>

    <!-- Narrow pages -->
    <style media="print and (max-width: 600pt)">
        body {
            font-size: 10pt;
        }

        .sidebar {
            width: 100%;
            float: none;
            margin-bottom: 15pt;
        }

        .main-content {
            width: 100%;
            margin-left: 0;
        }

        .two-column {
            column-count: 1;
        }
    </style>

    <!-- Wide pages -->
    <style media="print and (min-width: 600pt)">
        body {
            font-size: 11pt;
        }

        .sidebar {
            width: 200pt;
            float: left;
            margin-right: 20pt;
        }

        .main-content {
            margin-left: 220pt;
        }

        .two-column {
            column-count: 2;
            column-gap: 20pt;
        }
    </style>
</head>
<body>
    <div class="sidebar">
        <h3>Navigation</h3>
        <ul>
            <li><a href="#section1">Section 1</a></li>
            <li><a href="#section2">Section 2</a></li>
        </ul>
    </div>

    <div class="main-content">
        <h1>Main Content</h1>
        <div class="two-column">
            <p>This content will display in two columns on wide pages
            and one column on narrow pages.</p>
        </div>
    </div>
</body>
</html>
```

### Multiple Conditional Stylesheets

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Multi-Conditional Document</title>

    <!-- Base styles for all -->
    <link rel="stylesheet" href="base.css" media="all" />

    <!-- Print-only base -->
    <link rel="stylesheet" href="print-base.css" media="print" />

    <!-- Print portrait -->
    <link rel="stylesheet" href="portrait.css"
          media="print and (orientation: portrait)" />

    <!-- Print landscape -->
    <link rel="stylesheet" href="landscape.css"
          media="print and (orientation: landscape)" />

    <!-- A4 specific -->
    <link rel="stylesheet" href="a4.css"
          media="print and (width: 210mm)" />

    <!-- Letter specific -->
    <link rel="stylesheet" href="letter.css"
          media="print and (width: 8.5in)" />

    <!-- High resolution -->
    <link rel="stylesheet" href="high-res.css"
          media="print and (min-resolution: 300dpi)" />
</head>
<body>
    <h1>Comprehensive Media Queries</h1>
    <p>This document uses multiple conditional stylesheets.</p>
</body>
</html>
```

### Data-Bound Media Queries

```html
<!-- Model: {
    pageSize: "A4",
    orientation: "portrait",
    stylesheets: [
        { url: "base.css", media: "all" },
        { url: "print.css", media: "print" },
        { url: "portrait.css", media: "print and (orientation: portrait)" }
    ]
} -->

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Dynamic Media Queries</title>

    <!-- Dynamic stylesheets -->
    <template data-bind="{{model.stylesheets}}">
        <link rel="stylesheet" href="{{.url}}" media="{{.media}}" />
    </template>

    <!-- Dynamic page setup -->
    <style media="print">
        @page {
            size: {{model.pageSize}} {{model.orientation}};
            margin: 2cm;
        }
    </style>
</head>
<body>
    <h1>Document with Dynamic Media Configuration</h1>
    <p>Page size: {{model.pageSize}}</p>
    <p>Orientation: {{model.orientation}}</p>
</body>
</html>
```

### Report with Print Optimization

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Annual Report 2025</title>

    <style media="print">
        @page {
            size: A4 portrait;
            margin: 2.5cm;

            @top-center {
                content: "Annual Report 2025";
                font-size: 9pt;
                color: #666;
            }

            @bottom-right {
                content: "Page " counter(page) " of " counter(pages);
                font-size: 9pt;
            }
        }

        body {
            font-family: Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
        }

        h1 {
            font-size: 24pt;
            margin-bottom: 20pt;
            page-break-after: avoid;
        }

        h2 {
            font-size: 16pt;
            margin-top: 20pt;
            page-break-after: avoid;
            border-bottom: 2pt solid #336699;
            padding-bottom: 5pt;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            page-break-inside: avoid;
        }

        th {
            background-color: #f2f2f2;
            padding: 8pt;
            text-align: left;
            border: 1pt solid #ddd;
        }

        td {
            padding: 8pt;
            border: 1pt solid #ddd;
        }

        .chart {
            page-break-inside: avoid;
        }

        .page-break {
            page-break-after: always;
        }
    </style>
</head>
<body>
    <h1>Annual Report 2025</h1>

    <h2>Executive Summary</h2>
    <p>Financial performance exceeded expectations...</p>

    <div class="page-break"></div>

    <h2>Financial Results</h2>
    <table>
        <thead>
            <tr>
                <th>Quarter</th>
                <th>Revenue</th>
                <th>Growth</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Q1</td>
                <td>$1.2M</td>
                <td>15%</td>
            </tr>
            <tr>
                <td>Q2</td>
                <td>$1.4M</td>
                <td>18%</td>
            </tr>
        </tbody>
    </table>
</body>
</html>
```

### Invoice with Print Styles

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Invoice #12345</title>

    <style media="print">
        @page {
            size: A4;
            margin: 1.5cm;
        }

        body {
            font-family: Arial, sans-serif;
            font-size: 10pt;
        }

        .invoice-header {
            display: flex;
            justify-content: space-between;
            margin-bottom: 30pt;
            border-bottom: 2pt solid #000;
            padding-bottom: 15pt;
        }

        .invoice-details {
            margin-bottom: 20pt;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20pt;
        }

        th {
            background-color: #333;
            color: white;
            padding: 10pt;
            text-align: left;
        }

        td {
            padding: 8pt;
            border-bottom: 1pt solid #ddd;
        }

        .total-row {
            font-weight: bold;
            font-size: 12pt;
        }

        .payment-terms {
            margin-top: 30pt;
            padding: 15pt;
            background-color: #f8f9fa;
        }
    </style>
</head>
<body>
    <div class="invoice-header">
        <div>
            <h1 style="margin: 0;">INVOICE</h1>
            <p><strong>Invoice #:</strong> 12345</p>
            <p><strong>Date:</strong> January 15, 2025</p>
        </div>
        <div style="text-align: right;">
            <p><strong>Company Name</strong></p>
            <p>123 Business St</p>
            <p>City, State 12345</p>
        </div>
    </div>

    <div class="invoice-details">
        <h3>Bill To:</h3>
        <p>Client Name<br/>
        456 Client Ave<br/>
        City, State 67890</p>
    </div>

    <table>
        <thead>
            <tr>
                <th>Description</th>
                <th>Quantity</th>
                <th>Unit Price</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Service A</td>
                <td>10</td>
                <td>$100.00</td>
                <td>$1,000.00</td>
            </tr>
            <tr>
                <td>Service B</td>
                <td>5</td>
                <td>$150.00</td>
                <td>$750.00</td>
            </tr>
            <tr class="total-row">
                <td colspan="3" style="text-align: right;">TOTAL:</td>
                <td>$1,750.00</td>
            </tr>
        </tbody>
    </table>

    <div class="payment-terms">
        <p><strong>Payment Terms:</strong> Net 30 days</p>
        <p><strong>Due Date:</strong> February 14, 2025</p>
    </div>
</body>
</html>
```

### Certificate with Print Optimization

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title>Certificate of Completion</title>

    <style media="print and (orientation: landscape)">
        @page {
            size: A4 landscape;
            margin: 0;
        }

        body {
            margin: 0;
            padding: 0;
            display: flex;
            align-items: center;
            justify-content: center;
            height: 210mm;
        }

        .certificate {
            width: 250mm;
            padding: 40pt;
            border: 10pt double #336699;
            text-align: center;
            background-color: #fafafa;
        }

        h1 {
            font-size: 36pt;
            color: #336699;
            margin-bottom: 20pt;
            font-family: 'Georgia', serif;
        }

        .recipient {
            font-size: 28pt;
            color: #000;
            margin: 30pt 0;
            font-weight: bold;
        }

        .details {
            font-size: 14pt;
            margin: 20pt 0;
        }

        .signature {
            margin-top: 40pt;
            display: flex;
            justify-content: space-around;
        }

        .signature-line {
            border-top: 2pt solid #000;
            width: 150pt;
            padding-top: 5pt;
        }
    </style>
</head>
<body>
    <div class="certificate">
        <h1>Certificate of Completion</h1>

        <p class="details">This is to certify that</p>

        <p class="recipient">John Doe</p>

        <p class="details">
            has successfully completed the course<br/>
            <strong>Advanced PDF Generation</strong><br/>
            on January 15, 2025
        </p>

        <div class="signature">
            <div>
                <div class="signature-line">Instructor</div>
            </div>
            <div>
                <div class="signature-line">Director</div>
            </div>
        </div>
    </div>
</body>
</html>
```

---

## See Also

- [link](/reference/htmltags/link.html) - Link element for stylesheets
- [style](/reference/htmltags/style.html) - Inline style element
- [rel](/reference/htmlattributes/rel.html) - Relationship attribute for link element
- [@media](/reference/css/media.html) - CSS media queries
- [@page](/reference/css/page.html) - Page rules for print
- [CSS Styles](/reference/styles/) - Complete CSS reference

---
