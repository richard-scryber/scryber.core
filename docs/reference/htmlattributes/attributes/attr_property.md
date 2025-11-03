---
layout: default
title: property
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @property : The Metadata Property Attribute

The `property` attribute defines metadata properties in HTML documents using the Open Graph protocol or other RDF vocabularies. While primarily used for web pages, it can be included in PDF metadata for comprehensive document information preservation.

## Usage

The `property` attribute is used with `<meta>` elements to:
- Define Open Graph protocol metadata
- Specify RDF vocabulary properties
- Provide structured metadata for social media sharing
- Preserve rich document metadata in PDF generation
- Define custom metadata properties

```html
<head>
    <meta property="og:title" content="Annual Financial Report" />
    <meta property="og:type" content="document" />
    <meta property="og:description" content="Comprehensive annual financial report for 2024" />
</head>
```

---

## Supported Elements

The `property` attribute is supported by the following element:

| Element | Description |
|---------|-------------|
| `<meta>` | Metadata element for document properties |

---

## Attribute Values

### Syntax

```html
<meta property="property-name" content="property-value" />
```

### Common Property Values

#### Open Graph Protocol

| Property | Description | Example |
|----------|-------------|---------|
| `og:title` | Document title | `"Annual Report 2024"` |
| `og:type` | Content type | `"document"`, `"article"`, `"website"` |
| `og:description` | Document description | `"Financial summary for fiscal year 2024"` |
| `og:url` | Canonical URL | `"https://example.com/report"` |
| `og:image` | Representative image | `"https://example.com/cover.jpg"` |
| `og:site_name` | Site/organization name | `"Acme Corporation"` |
| `og:locale` | Document locale | `"en_US"`, `"es_ES"` |

#### Dublin Core Metadata

| Property | Description | Example |
|----------|-------------|---------|
| `dc:title` | Resource title | `"Technical Documentation"` |
| `dc:creator` | Content creator | `"John Smith"` |
| `dc:subject` | Topic or keywords | `"PDF, Documentation"` |
| `dc:description` | Resource description | `"Complete API reference"` |
| `dc:publisher` | Publisher name | `"Tech Publishing Inc."` |
| `dc:date` | Publication date | `"2024-01-15"` |
| `dc:language` | Content language | `"en"`, `"es"` |

#### Custom Properties

You can define custom properties for application-specific metadata:

```html
<meta property="app:version" content="2.1.0" />
<meta property="app:department" content="Finance" />
<meta property="app:confidentiality" content="Internal Use Only" />
```

---

## Binding Values

The `property` and `content` attributes support data binding:

### Static Metadata

```html
<meta property="og:title" content="Annual Report 2024" />
<meta property="og:type" content="document" />
```

### Dynamic Metadata with Data Binding

```html
<!-- Model: { reportTitle: "Q4 2024 Report", reportType: "quarterly" } -->
<meta property="og:title" content="{{model.reportTitle}}" />
<meta property="og:type" content="{{model.reportType}}" />
<meta property="og:date" content="{{model.generationDate}}" />
```

### Conditional Metadata

```html
<!-- Model: { isPublic: false, department: "Finance" } -->
<meta property="app:visibility"
      content="{{model.isPublic ? 'public' : 'internal'}}" />
<meta property="app:department" content="{{model.department}}" />
```

---

## Notes

### Open Graph Protocol

The Open Graph protocol was originally designed for social media sharing but provides a useful metadata framework for any document type. Key benefits:

- **Structured**: Well-defined vocabulary for common metadata
- **Extensible**: Supports custom properties
- **Widely recognized**: Understood by many systems
- **Comprehensive**: Covers titles, descriptions, images, and more

### Property vs Name

`<meta>` elements can use either `name` or `property` attributes:

**`name` attribute** (standard HTML metadata):
```html
<meta name="author" content="John Smith" />
<meta name="description" content="Document description" />
```

**`property` attribute** (RDF/Open Graph metadata):
```html
<meta property="og:title" content="Document Title" />
<meta property="dc:creator" content="John Smith" />
```

### PDF Generation Behavior

In Scryber PDF generation:

- `property` attributes are **informational** and preserved in document structure
- Standard metadata (`name="author"`, etc.) is embedded in PDF document properties
- Open Graph metadata is **not** embedded in PDF metadata dictionary
- Custom properties can be processed by custom code if needed
- Metadata helps document conversion workflows maintain information

### Use Cases

1. **Web-to-PDF Conversion**: Preserve Open Graph metadata from web pages
2. **Document Management**: Store custom classification metadata
3. **Workflow Integration**: Include application-specific properties
4. **Archive Systems**: Maintain rich metadata for long-term storage
5. **Search Enhancement**: Provide additional indexing information

---

## Examples

### Example 1: Basic Open Graph Metadata

```html
<head>
    <title>Quarterly Sales Report</title>
    <meta property="og:title" content="Q4 2024 Sales Report" />
    <meta property="og:type" content="document" />
    <meta property="og:description" content="Comprehensive sales analysis for Q4 2024" />
</head>
```

### Example 2: Complete Open Graph Document Metadata

```html
<head>
    <title>Annual Report 2024</title>
    <meta charset="utf-8" />

    <!-- Standard HTML metadata -->
    <meta name="author" content="Finance Department" />
    <meta name="description" content="Annual financial report for fiscal year 2024" />

    <!-- Open Graph metadata -->
    <meta property="og:title" content="Acme Corp - Annual Report 2024" />
    <meta property="og:type" content="article" />
    <meta property="og:url" content="https://acme.com/reports/annual-2024" />
    <meta property="og:description" content="Comprehensive annual financial report including revenue, expenses, and growth analysis" />
    <meta property="og:site_name" content="Acme Corporation" />
    <meta property="og:locale" content="en_US" />
    <meta property="og:image" content="https://acme.com/images/report-cover-2024.jpg" />
</head>
```

### Example 3: Dublin Core Metadata

```html
<head>
    <title>Technical Documentation</title>

    <!-- Dublin Core metadata -->
    <meta property="dc:title" content="API Reference Documentation" />
    <meta property="dc:creator" content="Engineering Team" />
    <meta property="dc:subject" content="API, Documentation, Reference" />
    <meta property="dc:description" content="Complete API reference for version 2.0" />
    <meta property="dc:publisher" content="Tech Publishing Inc." />
    <meta property="dc:date" content="2024-01-15" />
    <meta property="dc:type" content="Text" />
    <meta property="dc:format" content="application/pdf" />
    <meta property="dc:language" content="en" />
</head>
```

### Example 4: Article with Author and Publication Info

```html
<head>
    <title>Research Paper: Machine Learning in Healthcare</title>

    <!-- Open Graph article metadata -->
    <meta property="og:title" content="Machine Learning Applications in Modern Healthcare" />
    <meta property="og:type" content="article" />
    <meta property="og:description" content="A comprehensive study on ML applications in healthcare" />

    <!-- Article-specific properties -->
    <meta property="article:published_time" content="2024-03-15T08:00:00Z" />
    <meta property="article:modified_time" content="2024-03-20T14:30:00Z" />
    <meta property="article:author" content="Dr. Jane Smith" />
    <meta property="article:section" content="Research" />
    <meta property="article:tag" content="Machine Learning" />
    <meta property="article:tag" content="Healthcare" />
    <meta property="article:tag" content="Artificial Intelligence" />
</head>
```

### Example 5: Custom Application Properties

```html
<head>
    <title>Internal Project Report</title>

    <!-- Standard metadata -->
    <meta name="author" content="Project Team Alpha" />

    <!-- Custom application properties -->
    <meta property="app:project-id" content="PRJ-2024-042" />
    <meta property="app:department" content="Research & Development" />
    <meta property="app:confidentiality" content="Internal Use Only" />
    <meta property="app:version" content="1.3.0" />
    <meta property="app:status" content="Final" />
    <meta property="app:reviewer" content="John Doe" />
    <meta property="app:approval-date" content="2024-01-20" />
</head>
```

### Example 6: Multi-Language Document

```html
<head>
    <title>International Product Catalog</title>

    <!-- Primary language metadata -->
    <meta property="og:title" content="Product Catalog 2024" />
    <meta property="og:locale" content="en_US" />
    <meta property="og:locale:alternate" content="es_ES" />
    <meta property="og:locale:alternate" content="fr_FR" />
    <meta property="og:locale:alternate" content="de_DE" />

    <!-- Language-specific descriptions -->
    <meta property="og:description" content="Complete product catalog for 2024" />
    <meta property="dc:language" content="en" />
</head>
```

### Example 7: Data-Bound Document Metadata

```html
<!-- Model: {
    title: "Q4 Financial Report",
    author: "Jane Doe",
    department: "Finance",
    confidential: true,
    version: "2.1",
    reportDate: "2024-12-31"
} -->

<head>
    <title>{{model.title}}</title>

    <meta name="author" content="{{model.author}}" />
    <meta name="description" content="Financial report for {{model.department}}" />

    <!-- Open Graph properties -->
    <meta property="og:title" content="{{model.title}}" />
    <meta property="og:type" content="document" />
    <meta property="og:description" content="{{model.department}} financial report" />

    <!-- Custom properties -->
    <meta property="app:version" content="{{model.version}}" />
    <meta property="app:department" content="{{model.department}}" />
    <meta property="app:confidential" content="{{model.confidential}}" />
    <meta property="app:report-date" content="{{model.reportDate}}" />
</head>
```

### Example 8: Document with Image Metadata

```html
<head>
    <title>Product Brochure</title>

    <meta property="og:title" content="Premium Product Line 2024" />
    <meta property="og:type" content="product" />
    <meta property="og:description" content="Our latest premium product offerings" />

    <!-- Image metadata -->
    <meta property="og:image" content="https://example.com/brochure-cover.jpg" />
    <meta property="og:image:url" content="https://example.com/brochure-cover.jpg" />
    <meta property="og:image:secure_url" content="https://example.com/brochure-cover.jpg" />
    <meta property="og:image:type" content="image/jpeg" />
    <meta property="og:image:width" content="1200" />
    <meta property="og:image:height" content="630" />
    <meta property="og:image:alt" content="Premium Product Line Cover Image" />
</head>
```

### Example 9: Book or Publication Metadata

```html
<head>
    <title>The Complete Guide to PDF Generation</title>

    <!-- Open Graph book metadata -->
    <meta property="og:title" content="The Complete Guide to PDF Generation" />
    <meta property="og:type" content="book" />
    <meta property="og:url" content="https://example.com/books/pdf-guide" />
    <meta property="og:description" content="A comprehensive guide to generating PDFs programmatically" />
    <meta property="og:image" content="https://example.com/book-cover.jpg" />

    <!-- Book-specific properties -->
    <meta property="book:author" content="John Smith" />
    <meta property="book:isbn" content="978-0-123456-78-9" />
    <meta property="book:release_date" content="2024-01-15" />
    <meta property="book:tag" content="Programming" />
    <meta property="book:tag" content="PDF" />
    <meta property="book:tag" content="Documentation" />
</head>
```

### Example 10: Corporate Report with Full Metadata

```html
<head>
    <title>Acme Corporation - Sustainability Report 2024</title>

    <!-- Standard HTML metadata -->
    <meta charset="utf-8" />
    <meta name="author" content="Corporate Sustainability Team" />
    <meta name="description" content="Annual sustainability and corporate responsibility report" />
    <meta name="keywords" content="sustainability, environment, corporate responsibility, ESG" />

    <!-- Open Graph metadata -->
    <meta property="og:title" content="Acme Corp Sustainability Report 2024" />
    <meta property="og:type" content="article" />
    <meta property="og:description" content="Our commitment to environmental sustainability and social responsibility" />
    <meta property="og:site_name" content="Acme Corporation" />
    <meta property="og:locale" content="en_US" />

    <!-- Dublin Core metadata -->
    <meta property="dc:title" content="Sustainability Report 2024" />
    <meta property="dc:creator" content="Acme Corporation" />
    <meta property="dc:date" content="2024-02-01" />
    <meta property="dc:type" content="Report" />
    <meta property="dc:subject" content="Sustainability, Environment, Corporate Responsibility" />

    <!-- Custom corporate properties -->
    <meta property="acme:report-type" content="sustainability" />
    <meta property="acme:fiscal-year" content="2024" />
    <meta property="acme:department" content="Corporate Affairs" />
    <meta property="acme:document-class" content="public" />
</head>
```

### Example 11: Invoice with Metadata

```html
<head>
    <title>Invoice #INV-2024-001</title>

    <!-- Standard metadata -->
    <meta name="author" content="Accounting Department" />
    <meta name="description" content="Invoice for professional services" />

    <!-- Document properties -->
    <meta property="og:title" content="Invoice #INV-2024-001" />
    <meta property="og:type" content="document" />

    <!-- Business document properties -->
    <meta property="invoice:number" content="INV-2024-001" />
    <meta property="invoice:date" content="2024-01-15" />
    <meta property="invoice:due-date" content="2024-02-14" />
    <meta property="invoice:customer" content="Client Corporation" />
    <meta property="invoice:amount" content="7500.00" />
    <meta property="invoice:currency" content="USD" />
    <meta property="invoice:status" content="unpaid" />
</head>
```

### Example 12: Scientific Paper Metadata

```html
<head>
    <title>Climate Change Impacts on Arctic Ecosystems</title>

    <!-- Standard metadata -->
    <meta name="author" content="Dr. Sarah Johnson, Dr. Michael Chen" />
    <meta name="description" content="Research on climate change effects in Arctic regions" />
    <meta name="keywords" content="climate change, Arctic, ecosystems, research" />

    <!-- Open Graph metadata -->
    <meta property="og:title" content="Climate Change Impacts on Arctic Ecosystems" />
    <meta property="og:type" content="article" />
    <meta property="og:description" content="Peer-reviewed research on Arctic ecosystem changes" />

    <!-- Article metadata -->
    <meta property="article:published_time" content="2024-01-10T00:00:00Z" />
    <meta property="article:author" content="Dr. Sarah Johnson" />
    <meta property="article:author" content="Dr. Michael Chen" />
    <meta property="article:section" content="Environmental Science" />
    <meta property="article:tag" content="Climate Science" />
    <meta property="article:tag" content="Ecology" />

    <!-- Academic properties -->
    <meta property="citation:title" content="Climate Change Impacts on Arctic Ecosystems" />
    <meta property="citation:author" content="Johnson, Sarah" />
    <meta property="citation:author" content="Chen, Michael" />
    <meta property="citation:publication_date" content="2024-01-10" />
    <meta property="citation:journal_title" content="Journal of Arctic Research" />
    <meta property="citation:volume" content="42" />
    <meta property="citation:issue" content="1" />
    <meta property="citation:firstpage" content="15" />
    <meta property="citation:lastpage" content="34" />
    <meta property="citation:doi" content="10.1234/jar.2024.001" />
</head>
```

### Example 13: Event Documentation

```html
<head>
    <title>Annual Conference 2024 - Proceedings</title>

    <!-- Open Graph event metadata -->
    <meta property="og:title" content="Tech Summit 2024 - Conference Proceedings" />
    <meta property="og:type" content="article" />
    <meta property="og:description" content="Documentation of presentations and discussions from Tech Summit 2024" />

    <!-- Event-specific properties -->
    <meta property="event:name" content="Tech Summit 2024" />
    <meta property="event:start_date" content="2024-03-15" />
    <meta property="event:end_date" content="2024-03-17" />
    <meta property="event:location" content="San Francisco Convention Center" />
    <meta property="event:organizer" content="Tech Events Inc." />
    <meta property="event:attendance" content="5000" />

    <!-- Document properties -->
    <meta property="document:type" content="proceedings" />
    <meta property="document:version" content="final" />
    <meta property="document:page-count" content="320" />
</head>
```

### Example 14: Training Manual with Metadata

```html
<head>
    <title>Employee Onboarding Guide</title>

    <!-- Standard metadata -->
    <meta name="author" content="Human Resources" />
    <meta name="description" content="Complete guide for new employee onboarding" />

    <!-- Open Graph metadata -->
    <meta property="og:title" content="Employee Onboarding Guide - 2024 Edition" />
    <meta property="og:type" content="article" />
    <meta property="og:description" content="Essential information for new hires" />

    <!-- Training document properties -->
    <meta property="training:title" content="Employee Onboarding" />
    <meta property="training:level" content="beginner" />
    <meta property="training:duration" content="40 hours" />
    <meta property="training:department" content="Human Resources" />
    <meta property="training:version" content="2024.1" />
    <meta property="training:last-updated" content="2024-01-05" />
    <meta property="training:mandatory" content="true" />
</head>
```

### Example 15: Legal Document with Classification

```html
<head>
    <title>Service Level Agreement</title>

    <!-- Standard metadata -->
    <meta name="author" content="Legal Department" />
    <meta name="description" content="Service Level Agreement between Company and Client" />

    <!-- Open Graph metadata -->
    <meta property="og:title" content="Service Level Agreement - Contract #SLA-2024-045" />
    <meta property="og:type" content="article" />

    <!-- Legal document properties -->
    <meta property="legal:document-type" content="Service Level Agreement" />
    <meta property="legal:contract-number" content="SLA-2024-045" />
    <meta property="legal:effective-date" content="2024-01-01" />
    <meta property="legal:expiration-date" content="2025-12-31" />
    <meta property="legal:party-a" content="Acme Corporation" />
    <meta property="legal:party-b" content="Client Services Ltd" />
    <meta property="legal:jurisdiction" content="California, USA" />
    <meta property="legal:classification" content="confidential" />
    <meta property="legal:status" content="active" />
</head>
```

---

## See Also

- [meta element](/reference/htmltags/meta.html) - The meta HTML element
- [content attribute](/reference/htmlattributes/content.html) - Meta content value
- [name attribute](/reference/htmlattributes/name.html) - Standard metadata name
- [http-equiv attribute](/reference/htmlattributes/http-equiv.html) - HTTP header equivalent
- [Open Graph Protocol](https://ogp.me/) - Open Graph specification
- [Dublin Core](https://dublincore.org/) - Dublin Core metadata initiative
- [Document Properties](/reference/document/properties.html) - PDF document properties
- [Data Binding](/reference/binding/) - Dynamic data binding

---
