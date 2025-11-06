---
layout: default
title: head
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;head&gt; : The Document Head Element
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
The `<head>` element contains metadata and document-level information that affects the entire PDF document. It is not rendered visually but provides essential information about document properties, styling, and configuration.

## Usage

The `<head>` element is a container for document metadata that:
- Sets PDF document properties (title, author, keywords, subject)
- Defines the document base path for resolving relative URLs
- Contains stylesheet references (`<link>` elements) and embedded styles (`<style>` elements)
- Configures PDF security and encryption settings through meta tags
- Provides document-level information without visual output
- Must be placed as the first child of the `<html>` element
- Acts as an invisible container (implements `IInvisibleContainer`)

```html
<!DOCTYPE html>
<html>
<head>
    <title>My PDF Document</title>
    <meta name="author" content="John Smith" />
    <meta name="description" content="A comprehensive guide" />
    <link rel="stylesheet" href="styles.css" />
    <style>
        body { font-family: Arial; }
    </style>
</head>
<body>
    <!-- Document content -->
</body>
</html>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. |

### Child Elements

The `<head>` element can contain:

| Element | Purpose | Required |
|---------|---------|----------|
| `<title>` | Sets the PDF document title | Recommended |
| `<base>` | Sets the base path for relative URLs | Optional |
| `<meta>` | Provides metadata (author, keywords, etc.) | Optional |
| `<link>` | Links external stylesheets | Optional |
| `<style>` | Embeds CSS styles | Optional |

---

## Notes

### Document Properties

The `<head>` element manages PDF document properties through its child elements:

1. **Title**: Set via `<title>` element or `title` attribute
2. **Author**: Set via `<meta name="author">`
3. **Subject**: Set via `<meta name="description">`
4. **Keywords**: Set via `<meta name="keywords">`
5. **Producer/Generator**: Set via `<meta name="generator">`

These properties appear in the PDF document's metadata and can be viewed in most PDF readers under Document Properties.

### Base Path Resolution

The `<base>` element sets the base URL for resolving relative paths:

```html
<head>
    <base href="https://example.com/documents/" />
</head>
```

All relative URLs in the document (images, stylesheets, links) are resolved relative to this base path.

### Meta Tag Processing

The `<head>` element processes specific meta tags during data binding:

**Standard Metadata**:
- `author`: Sets document author
- `description`: Sets document subject/description
- `keywords`: Sets document keywords
- `generator`: Sets document producer/generator

**PDF Security**:
- `print-restrictions`: Configures PDF permissions
- `print-encryption`: Sets encryption level (40bit or 128bit)

### Data Binding and Registration

The `<head>` element registers document information during the data binding phase:

1. Validates that the parent is a `Document` component
2. Updates document info properties
3. Processes all child `<meta>` elements
4. Applies security and encryption settings
5. Sets the document's loaded source from `<base>` if present

### Class Hierarchy

In the Scryber codebase:
- `HTMLHead` extends `ContainerComponent` implements `IInvisibleContainer`
- Contains a `ComponentList` for child elements
- Registers information with parent `Document` during data binding
- Supports `HTMLHeadBase` for base path configuration

### Invisible Container

The `<head>` element implements `IInvisibleContainer`, which means:
- It does not render any visual content
- It does not participate in layout
- It only affects document metadata and configuration
- Child elements are processed but not rendered (except styles)

---

## Examples

### Basic HTML Document Structure

```html
<!DOCTYPE html>
<html>
<head>
    <title>Annual Report 2025</title>
    <meta name="author" content="Finance Department" />
    <meta name="description" content="Annual financial report for fiscal year 2025" />
    <meta name="keywords" content="finance, annual report, 2025, revenue" />
</head>
<body>
    <h1>Annual Report 2025</h1>
    <p>This document contains the annual financial report...</p>
</body>
</html>
```

### Complete Document with All Metadata

```html
<!DOCTYPE html>
<html>
<head>
    <title>Corporate Policy Manual</title>
    <base href="https://company.com/documents/" />
    <meta name="author" content="HR Department" />
    <meta name="description" content="Employee policies and procedures" />
    <meta name="keywords" content="policy, procedures, HR, employment" />
    <meta name="generator" content="Scryber PDF Generator" />
    <meta charset="utf-8" />

    <link rel="stylesheet" href="corporate-styles.css" />

    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 20pt;
        }
        h1 {
            color: #336699;
            border-bottom: 2pt solid #336699;
        }
    </style>
</head>
<body>
    <h1>Corporate Policy Manual</h1>
    <!-- Document content -->
</body>
</html>
```

### Document with External Stylesheets

```html
<!DOCTYPE html>
<html>
<head>
    <title>Product Catalog</title>
    <meta name="author" content="Marketing Team" />

    <!-- External stylesheets -->
    <link rel="stylesheet" href="css/reset.css" />
    <link rel="stylesheet" href="css/layout.css" />
    <link rel="stylesheet" href="css/theme.css" />
    <link rel="stylesheet" href="css/print.css" media="print" />
</head>
<body>
    <!-- Catalog content -->
</body>
</html>
```

### Document with Embedded Styles

```html
<!DOCTYPE html>
<html>
<head>
    <title>Technical Specification</title>
    <meta name="author" content="Engineering" />
    <meta name="description" content="Product technical specifications" />

    <style>
        /* Global styles */
        * {
            box-sizing: border-box;
        }

        body {
            font-family: 'Helvetica', sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            margin: 30pt;
        }

        h1, h2, h3 {
            color: #2c3e50;
            margin-top: 20pt;
        }

        .spec-table {
            width: 100%;
            border-collapse: collapse;
        }

        .spec-table th {
            background-color: #34495e;
            color: white;
            padding: 8pt;
            text-align: left;
        }

        .spec-table td {
            border: 1pt solid #bdc3c7;
            padding: 6pt;
        }
    </style>
</head>
<body>
    <h1>Technical Specification</h1>
    <!-- Specification content -->
</body>
</html>
```

### Document with Base Path

```html
<!DOCTYPE html>
<html>
<head>
    <title>User Guide</title>
    <base href="https://cdn.example.com/docs/v2/" />
    <meta name="author" content="Documentation Team" />

    <!-- All relative URLs resolve from base path -->
    <link rel="stylesheet" href="styles/guide.css" />
    <!-- Resolves to: https://cdn.example.com/docs/v2/styles/guide.css -->
</head>
<body>
    <!-- Images also use base path -->
    <img src="images/logo.png" />
    <!-- Resolves to: https://cdn.example.com/docs/v2/images/logo.png -->
</body>
</html>
```

### Document with Security Settings

```html
<!DOCTYPE html>
<html>
<head>
    <title>Confidential Report</title>
    <meta name="author" content="Security Department" />
    <meta name="description" content="Confidential security audit report" />

    <!-- Restrict PDF permissions -->
    <meta name="print-restrictions" content="printing, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
<body>
    <h1>Confidential Report</h1>
    <p>This document is protected and printing/copying is restricted.</p>
</body>
</html>
```

### Document with Full Restrictions

```html
<!DOCTYPE html>
<html>
<head>
    <title>Restricted Document</title>
    <meta name="author" content="Legal Department" />

    <!-- Block all permissions except accessibility -->
    <meta name="print-restrictions" content="accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
<body>
    <h1>Restricted Document</h1>
    <p>This document cannot be printed, copied, or modified.</p>
</body>
</html>
```

### Document with No Restrictions

```html
<!DOCTYPE html>
<html>
<head>
    <title>Public Document</title>
    <meta name="author" content="Public Relations" />

    <!-- Allow all permissions -->
    <meta name="print-restrictions" content="none" />
</head>
<body>
    <h1>Public Document</h1>
    <p>This document is freely accessible with no restrictions.</p>
</body>
</html>
```

### Document with Multiple Security Permissions

```html
<!DOCTYPE html>
<html>
<head>
    <title>Protected Document</title>
    <meta name="author" content="Compliance" />

    <!-- Allow specific permissions -->
    <meta name="print-restrictions" content="printing, accessibility, annotations" />
    <meta name="print-encryption" content="128bit" />
</head>
<body>
    <h1>Protected Document</h1>
    <p>Printing and annotations allowed, but copying and modifications are restricted.</p>
</body>
</html>
```

### Multi-Language Document

```html
<!DOCTYPE html>
<html>
<head>
    <title>International User Guide</title>
    <meta charset="utf-8" />
    <meta name="author" content="Localization Team" />
    <meta name="description" content="User guide in multiple languages" />
    <meta name="keywords" content="guide, manual, international, multilingual" />

    <style>
        body {
            font-family: 'Noto Sans', Arial, sans-serif;
        }
    </style>
</head>
<body>
    <h1>User Guide</h1>
    <p>Content in multiple languages: English, Español, Français, 中文, 日本語</p>
</body>
</html>
```

### Document with Mixed Styling

```html
<!DOCTYPE html>
<html>
<head>
    <title>Styled Report</title>
    <meta name="author" content="Design Team" />

    <!-- External base styles -->
    <link rel="stylesheet" href="base.css" />

    <!-- Embedded custom styles override external -->
    <style>
        /* Override base styles for this document */
        body {
            background-color: #f9f9f9;
            margin: 25pt;
        }

        .custom-header {
            background: linear-gradient(to right, #667eea, #764ba2);
            color: white;
            padding: 20pt;
            margin: -25pt -25pt 20pt -25pt;
        }
    </style>
</head>
<body>
    <div class="custom-header">
        <h1>Styled Report</h1>
    </div>
    <!-- Report content -->
</body>
</html>
```

### Document with Generator Information

```html
<!DOCTYPE html>
<html>
<head>
    <title>Generated Report</title>
    <meta name="author" content="Automated System" />
    <meta name="description" content="Automatically generated sales report" />
    <meta name="generator" content="SalesReportGenerator v2.5 using Scryber" />
    <meta name="keywords" content="sales, report, automated, Q4 2025" />
</head>
<body>
    <h1>Q4 2025 Sales Report</h1>
    <p>Generated: October 13, 2025</p>
    <!-- Report content -->
</body>
</html>
```

### Corporate Template Structure

```html
<!DOCTYPE html>
<html>
<head>
    <title>{{model.documentTitle}}</title>
    <base href="https://corporate.example.com/templates/" />

    <meta name="author" content="{{model.department}}" />
    <meta name="description" content="{{model.description}}" />
    <meta name="keywords" content="{{model.keywords}}" />

    <!-- Corporate stylesheets -->
    <link rel="stylesheet" href="styles/corporate-reset.css" />
    <link rel="stylesheet" href="styles/corporate-layout.css" />
    <link rel="stylesheet" href="styles/corporate-brand.css" />

    <style>
        /* Document-specific overrides */
        body {
            margin: {{model.pageMargin}}pt;
        }

        .header {
            background-color: {{model.brandColor}};
        }
    </style>
</head>
<body>
    <!-- Template content -->
</body>
</html>
```

### Academic Paper Template

```html
<!DOCTYPE html>
<html>
<head>
    <title>{{model.paperTitle}}</title>

    <meta name="author" content="{{model.authors}}" />
    <meta name="description" content="{{model.abstract}}" />
    <meta name="keywords" content="{{model.keywords}}" />

    <style>
        /* Academic paper formatting */
        body {
            font-family: 'Times New Roman', serif;
            font-size: 12pt;
            line-height: 2;
            margin: 72pt;
            text-align: justify;
        }

        h1 {
            font-size: 16pt;
            text-align: center;
            margin: 24pt 0;
        }

        h2 {
            font-size: 14pt;
            margin: 18pt 0 12pt 0;
        }

        .author-info {
            text-align: center;
            font-size: 12pt;
            margin: 12pt 0;
        }

        .abstract {
            margin: 24pt 36pt;
            font-size: 11pt;
        }

        .references {
            font-size: 11pt;
        }
    </style>
</head>
<body>
    <!-- Academic paper content -->
</body>
</html>
```

### Invoice Template

```html
<!DOCTYPE html>
<html>
<head>
    <title>Invoice {{model.invoiceNumber}}</title>

    <meta name="author" content="{{model.companyName}}" />
    <meta name="description" content="Invoice for {{model.clientName}}" />
    <meta name="keywords" content="invoice, billing, {{model.invoiceNumber}}" />

    <style>
        body {
            font-family: Arial, sans-serif;
            font-size: 10pt;
            margin: 30pt;
        }

        .invoice-header {
            border-bottom: 2pt solid #333;
            padding-bottom: 10pt;
            margin-bottom: 20pt;
        }

        .invoice-header h1 {
            margin: 0;
            font-size: 24pt;
        }

        .invoice-table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
        }

        .invoice-table th {
            background-color: #333;
            color: white;
            padding: 8pt;
            text-align: left;
        }

        .invoice-table td {
            border-bottom: 1pt solid #ddd;
            padding: 8pt;
        }

        .total-row {
            font-weight: bold;
            font-size: 12pt;
            background-color: #f5f5f5;
        }
    </style>
</head>
<body>
    <!-- Invoice content -->
</body>
</html>
```

---

## See Also

- [meta](/reference/htmltags/meta.html) - Meta element for document metadata
- [title](/reference/htmltags/title.html) - Title element for document title
- [link](/reference/htmltags/link.html) - Link element for external stylesheets
- [style](/reference/htmltags/style.html) - Style element for embedded CSS
- [body](/reference/htmltags/body.html) - Body element for document content
- [html](/reference/htmltags/html.html) - Root HTML element
- [Document Component](/reference/components/document.html) - Base document component
- [Document Properties](/reference/document/properties.html) - PDF document properties
- [Security Settings](/reference/security/) - PDF security and encryption
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
