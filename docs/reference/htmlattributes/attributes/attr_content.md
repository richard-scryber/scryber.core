---
layout: default
title: content
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @content : The Metadata Content Value Attribute

The `content` attribute specifies the value for metadata properties in HTML `<meta>` elements. It works in conjunction with the `name`, `property`, or `http-equiv` attributes to define document metadata that can be embedded in PDF properties or used for documentation purposes.

## Usage

The `content` attribute is used with `<meta>` elements to:
- Provide values for document metadata properties
- Set PDF document information (author, subject, keywords)
- Define Open Graph protocol values
- Specify HTTP header equivalent values
- Supply custom metadata values

```html
<head>
    <meta name="author" content="John Smith" />
    <meta name="description" content="Annual financial report for 2024" />
    <meta name="keywords" content="finance, report, annual, 2024" />
    <meta property="og:title" content="Annual Report 2024" />
</head>
```

---

## Supported Elements

The `content` attribute is supported by the following element:

| Element | Description |
|---------|-------------|
| `<meta>` | Metadata element for document properties |

---

## Usage with Different Meta Types

### With `name` Attribute (Standard Metadata)

```html
<meta name="author" content="Jane Doe" />
<meta name="description" content="Product catalog 2024" />
<meta name="keywords" content="products, catalog, shopping" />
```

These values are **embedded in PDF document properties**:
- `author` → PDF Author field
- `description` → PDF Subject field
- `keywords` → PDF Keywords field

### With `property` Attribute (Open Graph/RDF)

```html
<meta property="og:title" content="Company Annual Report" />
<meta property="og:type" content="document" />
<meta property="og:description" content="Financial summary" />
```

These values are **informational** in PDFs (not embedded in standard PDF metadata).

### With `http-equiv` Attribute (HTTP Headers)

```html
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<meta http-equiv="Content-Language" content="en-US" />
```

These values are **informational** in PDFs (HTTP headers don't apply to PDF files).

---

## Binding Values

The `content` attribute fully supports data binding:

### Static Content

```html
<meta name="author" content="Corporate Team" />
<meta name="description" content="Annual Report" />
```

### Dynamic Content with Data Binding

```html
<!-- Model: { author: "Jane Smith", reportTitle: "Q4 Report", year: 2024 } -->
<meta name="author" content="{{model.author}}" />
<meta name="description" content="{{model.reportTitle}} for {{model.year}}" />
<meta name="keywords" content="report, {{model.year}}, financial" />
```

### Calculated Content

```html
<!-- Model: { firstName: "John", lastName: "Doe", date: "2024-01-15" } -->
<meta name="author" content="{{model.firstName}} {{model.lastName}}" />
<meta name="generator" content="Report Generator v2.0 - {{model.date}}" />
```

### Conditional Content

```html
<!-- Model: { isPublic: false } -->
<meta name="classification"
      content="{{model.isPublic ? 'Public' : 'Confidential'}}" />
```

---

## Notes

### PDF Metadata Embedding

When generating PDFs, Scryber embeds certain metadata into the PDF document properties:

| Meta Name | PDF Property | Description |
|-----------|--------------|-------------|
| `author` | Author | Document creator |
| `description` | Subject | Document subject/description |
| `keywords` | Keywords | Searchable keywords |
| `generator` | Producer | Generating application |

These appear in PDF readers under **File → Properties → Description**.

### Content Value Formats

The `content` attribute accepts various formats:

| Format | Example | Use Case |
|--------|---------|----------|
| Plain text | `"John Smith"` | Names, titles |
| Comma-separated | `"PDF, document, report"` | Keywords |
| Sentences | `"Annual financial report"` | Descriptions |
| URLs | `"https://example.com"` | Links |
| Dates | `"2024-01-15"` | Dates (ISO format recommended) |
| Numbers | `"1.0"` | Versions |

### Empty Content

An empty `content` attribute is valid but results in empty metadata:

```html
<!-- Empty author -->
<meta name="author" content="" />

<!-- Equivalent to omitting the meta tag -->
```

### Multiple Values

For properties that support multiple values (like keywords), use comma-separated values:

```html
<meta name="keywords" content="finance, accounting, report, 2024, annual" />
```

Or use multiple meta tags with the same property:

```html
<meta property="article:tag" content="Finance" />
<meta property="article:tag" content="Accounting" />
<meta property="article:tag" content="Report" />
```

---

## Examples

### Example 1: Basic Document Metadata

```html
<head>
    <title>Annual Report 2024</title>
    <meta charset="UTF-8" />
    <meta name="author" content="Finance Department" />
    <meta name="description" content="Annual financial report for fiscal year 2024" />
    <meta name="keywords" content="annual, report, finance, 2024" />
</head>
```

### Example 2: Comprehensive PDF Metadata

```html
<head>
    <title>Technical Documentation</title>
    <meta charset="UTF-8" />
    <meta name="author" content="Engineering Team" />
    <meta name="description" content="Complete API reference documentation for version 2.0" />
    <meta name="keywords" content="API, documentation, reference, technical, guide" />
    <meta name="generator" content="DocGen 1.5 - Scryber PDF" />
</head>
```

### Example 3: Data-Bound Metadata

```html
<!-- Model: {
    author: "Jane Doe",
    department: "Marketing",
    title: "Product Catalog",
    year: 2024
} -->

<head>
    <title>{{model.title}} {{model.year}}</title>
    <meta name="author" content="{{model.author}}" />
    <meta name="description" content="{{model.title}} created by {{model.department}}" />
    <meta name="keywords" content="catalog, products, {{model.year}}, {{model.department}}" />
    <meta name="generator" content="Catalog Generator - {{model.year}}" />
</head>
```

### Example 4: Open Graph Metadata

```html
<head>
    <title>Company Blog Post</title>

    <!-- Standard metadata -->
    <meta name="author" content="John Smith" />
    <meta name="description" content="Latest insights on PDF generation" />

    <!-- Open Graph metadata -->
    <meta property="og:title" content="Mastering PDF Generation with Scryber" />
    <meta property="og:type" content="article" />
    <meta property="og:description" content="Learn advanced techniques for PDF generation" />
    <meta property="og:image" content="https://example.com/article-image.jpg" />
    <meta property="og:url" content="https://example.com/blog/pdf-generation" />
</head>
```

### Example 5: Multi-Author Document

```html
<head>
    <title>Research Paper</title>
    <meta name="author" content="Dr. Jane Smith, Dr. John Doe, Dr. Sarah Johnson" />
    <meta name="description" content="Collaborative research on machine learning applications" />
    <meta name="keywords" content="machine learning, AI, research, collaboration" />
</head>
```

### Example 6: Detailed Keywords

```html
<head>
    <title>Product Specification</title>
    <meta name="author" content="Product Management" />
    <meta name="description" content="Detailed specifications for Model XYZ-2000" />
    <meta name="keywords" content="product, specification, XYZ-2000, technical, features, dimensions, compatibility, performance" />
</head>
```

### Example 7: Localized Content

```html
<head>
    <title>International Report</title>
    <meta charset="UTF-8" />
    <meta name="author" content="Global Team" />
    <meta name="description" content="International sales report - English version" />
    <meta name="keywords" content="international, sales, global, report" />
    <meta http-equiv="Content-Language" content="en-US" />
</head>
```

### Example 8: Version Information

```html
<head>
    <title>Software Documentation v2.1</title>
    <meta name="author" content="Documentation Team" />
    <meta name="description" content="User guide for Software v2.1" />
    <meta name="keywords" content="software, documentation, guide, v2.1" />
    <meta name="generator" content="DocBuilder v3.0" />
    <meta name="version" content="2.1.0" />
</head>
```

### Example 9: Corporate Document

```html
<head>
    <title>Quarterly Business Review</title>
    <meta charset="UTF-8" />
    <meta name="author" content="Executive Team - Acme Corporation" />
    <meta name="description" content="Q4 2024 business review including revenue, expenses, and strategic initiatives" />
    <meta name="keywords" content="quarterly, business review, Q4, 2024, strategy, performance" />
    <meta property="og:title" content="Acme Corp Q4 2024 Business Review" />
    <meta property="og:type" content="document" />
    <meta property="og:description" content="Executive summary of Q4 performance" />
</head>
```

### Example 10: Invoice Metadata

```html
<!-- Model: { invoiceNumber: "INV-2024-001", clientName: "ABC Corp", date: "2024-01-15" } -->

<head>
    <title>Invoice {{model.invoiceNumber}}</title>
    <meta name="author" content="Accounting Department" />
    <meta name="description" content="Invoice {{model.invoiceNumber}} for {{model.clientName}} - {{model.date}}" />
    <meta name="keywords" content="invoice, {{model.invoiceNumber}}, {{model.clientName}}, billing" />
    <meta property="invoice:number" content="{{model.invoiceNumber}}" />
    <meta property="invoice:client" content="{{model.clientName}}" />
    <meta property="invoice:date" content="{{model.date}}" />
</head>
```

### Example 11: Academic Paper

```html
<head>
    <title>Climate Change Research Paper</title>
    <meta charset="UTF-8" />
    <meta name="author" content="Dr. Sarah Johnson, Dr. Michael Chen" />
    <meta name="description" content="Research paper on climate change impacts in Arctic ecosystems" />
    <meta name="keywords" content="climate change, Arctic, ecosystems, environmental science, research" />
    <meta property="citation:title" content="Climate Change Impacts on Arctic Ecosystems" />
    <meta property="citation:author" content="Johnson, Sarah" />
    <meta property="citation:author" content="Chen, Michael" />
    <meta property="citation:publication_date" content="2024-01-10" />
</head>
```

### Example 12: Training Manual

```html
<head>
    <title>Employee Training Manual</title>
    <meta name="author" content="Human Resources Department" />
    <meta name="description" content="Comprehensive training manual for new employees covering company policies, procedures, and best practices" />
    <meta name="keywords" content="training, manual, employees, HR, onboarding, policies, procedures" />
    <meta name="generator" content="Training Documentation System v2.0" />
    <meta property="training:level" content="beginner" />
    <meta property="training:duration" content="40 hours" />
</head>
```

### Example 13: Legal Document

```html
<head>
    <title>Service Level Agreement</title>
    <meta name="author" content="Legal Department - Acme Corporation" />
    <meta name="description" content="Service Level Agreement between Acme Corporation and Client Services Ltd, effective January 1, 2024" />
    <meta name="keywords" content="SLA, service level agreement, contract, legal, terms" />
    <meta property="legal:contract-number" content="SLA-2024-045" />
    <meta property="legal:effective-date" content="2024-01-01" />
    <meta property="legal:parties" content="Acme Corporation, Client Services Ltd" />
</head>
```

### Example 14: Conditional Metadata

```html
<!-- Model: { isDraft: true, author: "John Doe", version: "0.9" } -->

<head>
    <title>{{model.isDraft ? '[DRAFT] ' : ''}}Project Proposal</title>
    <meta name="author" content="{{model.author}}" />
    <meta name="description" content="{{model.isDraft ? 'Draft version of project proposal - not final' : 'Final project proposal'}}" />
    <meta name="keywords" content="project, proposal, {{model.isDraft ? 'draft' : 'final'}}" />
    <meta name="version" content="{{model.version}}" />
    <meta name="status" content="{{model.isDraft ? 'draft' : 'final'}}" />
</head>
```

### Example 15: Rich Metadata Collection

```html
<head>
    <title>Comprehensive Document Example</title>
    <meta charset="UTF-8" />

    <!-- Standard HTML metadata -->
    <meta name="author" content="Corporate Documentation Team" />
    <meta name="description" content="Example document showcasing comprehensive metadata usage in PDF generation" />
    <meta name="keywords" content="metadata, PDF, documentation, example, comprehensive" />
    <meta name="generator" content="Scryber PDF Generator v2.0" />

    <!-- HTTP equivalents -->
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="Content-Language" content="en-US" />

    <!-- Open Graph metadata -->
    <meta property="og:title" content="Comprehensive Document Example" />
    <meta property="og:type" content="document" />
    <meta property="og:description" content="Showcase of metadata capabilities" />
    <meta property="og:locale" content="en_US" />

    <!-- Custom application metadata -->
    <meta property="app:document-id" content="DOC-2024-042" />
    <meta property="app:department" content="Documentation" />
    <meta property="app:classification" content="public" />
    <meta property="app:version" content="1.0.0" />
</head>
```

---

## See Also

- [meta element](/reference/htmltags/meta.html) - The meta HTML element
- [name attribute](/reference/htmlattributes/name.html) - Metadata property name
- [property attribute](/reference/htmlattributes/property.html) - Open Graph properties
- [http-equiv attribute](/reference/htmlattributes/http-equiv.html) - HTTP header equivalent
- [Document Properties](/reference/document/properties.html) - PDF document properties
- [Document Info](/reference/document/info.html) - PDF document information
- [Data Binding](/reference/binding/) - Dynamic data binding
- [Metadata](/reference/metadata/) - Complete metadata reference

---
