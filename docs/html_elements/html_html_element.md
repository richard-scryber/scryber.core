---
layout: default
title: html
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;html&gt; : The Root HTML Document Element

The `<html>` element is the root element of an HTML document. It contains all other elements and defines the document structure with a head section for metadata and a body (or frameset) section for content.

## Usage

The `<html>` element serves as the document root that:
- Contains the `<head>` element for metadata, styles, and configuration
- Contains either a `<body>` element for content OR a `<frameset>` element for PDF merging
- Defines the document language with the `lang` attribute
- Applies default styling and user-agent styles to all child elements
- Manages CSS variable (`:root`) definitions from document parameters

```html
<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <title>My PDF Document</title>
        <style>
            body { font-family: Arial, sans-serif; }
        </style>
    </head>
    <body>
        <h1>Document Content</h1>
        <p>This is the main content of the PDF.</p>
    </body>
</html>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `lang` | string | Specifies the language of the document (e.g., "en", "es", "fr"). |
| `title` | string | Sets the outline/bookmark title for the document. |
| `class` | string | CSS class name(s) for styling the document. |
| `hidden` | string | Controls document visibility. Set to "hidden" to hide. |

### Document Structure

The `<html>` element must contain:
- **One `<head>` element** (optional but recommended) - Contains metadata
- **Either** a `<body>` element OR a `<frameset>` element (not both)

If both body and frameset are specified, an `InvalidOperationException` will be thrown.

---

## Notes

### Document Structure

The HTML element enforces proper document structure:
```
<html>
  ├─ <head>      (metadata, styles, configuration)
  └─ <body>      (content pages)
     OR
  └─ <frameset>  (PDF merging/modification)
```

### Body vs Frameset

**Use `<body>`** when:
- Creating new document content
- Generating PDFs from templates
- Standard document generation

**Use `<frameset>`** when:
- Merging existing PDF files
- Filling forms in existing PDFs
- Adding content to existing documents
- Combining multiple PDF sources

You **cannot** use both body and frameset in the same document.

### Root Styles (User Agent Stylesheet)

The HTML element automatically applies user-agent styles to elements, including:
- Quote marks for `<q>` elements (`:before` and `:after` pseudo-elements)
- Default typography and spacing rules
- CSS variable definitions from document parameters

### CSS Variables from Parameters

Document parameters starting with `--` are automatically added to the `:root` CSS context:

```csharp
doc.Params["--primary-color"] = "#336699";
doc.Params["--font-size"] = "12pt";
```

These become available as CSS variables:
```css
h1 { color: var(--primary-color); }
body { font-size: var(--font-size); }
```

### Language Attribute

The `lang` attribute specifies the document's language but does not affect text rendering in the current version. It's primarily for semantic correctness.

---

## Examples

### Basic HTML Document

```html
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>Simple PDF Document</title>
    </head>
    <body>
        <h1>Hello World</h1>
        <p>This is a basic PDF document.</p>
    </body>
</html>
```

### Document with External Stylesheet

```html
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>Styled Document</title>
        <link rel="stylesheet" href="styles/main.css" />
        <link rel="stylesheet" href="styles/print.css" />
    </head>
    <body>
        <h1>Professional Document</h1>
        <p>Content styled with external CSS.</p>
    </body>
</html>
```

### Document with Embedded Styles

```html
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>Invoice</title>
        <style>
            :root {
                --primary-color: #336699;
                --secondary-color: #999;
            }

            body {
                font-family: Arial, sans-serif;
                font-size: 11pt;
            }

            h1 {
                color: var(--primary-color);
                font-size: 24pt;
            }

            .invoice-table {
                width: 100%;
                border-collapse: collapse;
            }
        </style>
    </head>
    <body>
        <h1>Invoice #12345</h1>
        <table class="invoice-table">
            <tr>
                <td>Item</td>
                <td>Amount</td>
            </tr>
        </table>
    </body>
</html>
```

### Document with Metadata

```html
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>Corporate Report</title>
        <meta charset="utf-8" />
        <meta name="author" content="John Smith" />
        <meta name="subject" content="Q4 2024 Financial Report" />
        <meta name="keywords" content="finance, quarterly, report, 2024" />
    </head>
    <body>
        <h1>Q4 2024 Financial Report</h1>
        <p>Report content...</p>
    </body>
</html>
```

### Document with Frameset (PDF Merging)

```html
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>Filled Form</title>
    </head>
    <frameset>
        <frame src="forms/template.pdf" data-page-start="0" data-page-count="-1">
            <!-- Content to overlay on existing PDF -->
            <div style="position: absolute; top: 100pt; left: 50pt;">
                <p>{{model.customerName}}</p>
            </div>
        </frame>
    </frameset>
</html>
```

### Multi-Language Document

```html
<!DOCTYPE html>
<html lang="es">
    <head>
        <meta charset="utf-8" />
        <title>Documento en Español</title>
        <meta name="author" content="María García" />
    </head>
    <body>
        <h1>Bienvenido</h1>
        <p>Este es un documento PDF en español.</p>
    </body>
</html>
```

### Document with CSS Variables from Code

```csharp
// C# Code
var path = "templates/branded-document.html";
using (var doc = Document.ParseDocument(path))
{
    // Set CSS variables via parameters
    doc.Params["--brand-color"] = "#FF6600";
    doc.Params["--brand-font"] = "Helvetica";
    doc.Params["--logo-url"] = "url('images/logo.png')";

    // Set content parameters
    doc.Params["model"] = new {
        title = "Branded Report",
        content = "Document content here"
    };

    using (var stream = new FileStream("output.pdf", FileMode.Create))
    {
        doc.SaveAsPDF(stream);
    }
}
```

```html
<!-- HTML Template: branded-document.html -->
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>{{model.title}}</title>
        <style>
            body {
                font-family: var(--brand-font), sans-serif;
            }

            h1 {
                color: var(--brand-color);
            }

            .header {
                background-image: var(--logo-url);
                background-repeat: no-repeat;
                background-position: left center;
                padding-left: 60pt;
            }
        </style>
    </head>
    <body>
        <div class="header">
            <h1>{{model.title}}</h1>
        </div>
        <p>{{model.content}}</p>
    </body>
</html>
```

### Document with Security Settings

```html
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>Confidential Report</title>
        <meta name="author" content="Security Department" />
        <meta name="pdf-user-password" content="user123" />
        <meta name="pdf-owner-password" content="owner456" />
        <meta name="pdf-allow-print" content="false" />
        <meta name="pdf-allow-copy" content="false" />
        <meta name="pdf-allow-modify" content="false" />
    </head>
    <body>
        <h1>Confidential Information</h1>
        <p>This document is protected and encrypted.</p>
    </body>
</html>
```

### Complete Invoice Example

```html
<!DOCTYPE html>
<html lang="en">
    <head>
        <title>Invoice #{{model.invoiceNumber}}</title>
        <meta name="author" content="{{model.companyName}}" />
        <meta name="subject" content="Invoice for {{model.customerName}}" />
        <style>
            :root {
                --primary-color: #2c3e50;
                --accent-color: #3498db;
                --border-color: #bdc3c7;
            }

            body {
                font-family: Arial, sans-serif;
                font-size: 10pt;
                margin: 40pt;
            }

            .header {
                border-bottom: 2pt solid var(--primary-color);
                margin-bottom: 20pt;
                padding-bottom: 10pt;
            }

            .invoice-title {
                color: var(--primary-color);
                font-size: 28pt;
                font-weight: bold;
            }

            .invoice-table {
                width: 100%;
                border-collapse: collapse;
                margin-top: 20pt;
            }

            .invoice-table th {
                background-color: var(--primary-color);
                color: white;
                padding: 8pt;
                text-align: left;
            }

            .invoice-table td {
                border-bottom: 1pt solid var(--border-color);
                padding: 8pt;
            }

            .total-row {
                font-weight: bold;
                font-size: 12pt;
                background-color: #ecf0f1;
            }
        </style>
    </head>
    <body>
        <div class="header">
            <div class="invoice-title">INVOICE</div>
            <div>Invoice #: {{model.invoiceNumber}}</div>
            <div>Date: {{model.date}}</div>
        </div>

        <div style="margin-bottom: 20pt;">
            <strong>Bill To:</strong><br/>
            {{model.customerName}}<br/>
            {{model.customerAddress}}
        </div>

        <table class="invoice-table">
            <thead>
                <tr>
                    <th>Description</th>
                    <th>Quantity</th>
                    <th>Unit Price</th>
                    <th>Total</th>
                </tr>
            </thead>
            <tbody>
                <template data-bind="{{model.items}}">
                    <tr>
                        <td>{{.description}}</td>
                        <td>{{.quantity}}</td>
                        <td>${{.unitPrice}}</td>
                        <td>${{.total}}</td>
                    </tr>
                </template>
                <tr class="total-row">
                    <td colspan="3" style="text-align: right;">TOTAL:</td>
                    <td>${{model.totalAmount}}</td>
                </tr>
            </tbody>
        </table>
    </body>
</html>
```

---

## See Also

- [head](/reference/htmltags/head.html) - Document head for metadata
- [body](/reference/htmltags/body.html) - Document body for content
- [frameset](/reference/htmltags/frameset.html) - Frameset for PDF merging
- [meta](/reference/htmltags/meta.html) - Metadata elements
- [style](/reference/htmltags/style.html) - Embedded styles
- [link](/reference/htmltags/link.html) - External stylesheets

---
