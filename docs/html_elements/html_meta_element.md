---
layout: default
title: meta
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;meta&gt; : The Metadata Element

The `<meta>` element provides metadata about the PDF document. It sets document properties like author, description, keywords, and configures PDF-specific settings such as security permissions and encryption.

## Usage

The `<meta>` element provides document metadata that:
- Sets PDF document properties (author, subject, keywords, producer)
- Configures PDF security restrictions and permissions
- Sets encryption levels for document protection
- Does not render any visible content in the PDF
- Must be placed within the `<head>` element
- Uses `name` and `content` attributes for standard metadata
- Supports special `name` values for PDF-specific features

```html
<head>
    <title>My Document</title>
    <meta name="author" content="John Smith" />
    <meta name="description" content="A comprehensive guide to PDF generation" />
    <meta name="keywords" content="PDF, documentation, guide" />
</head>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `name` | string | The name of the metadata property. |
| `content` | string | The value of the metadata property. |
| `http-equiv` | string | HTTP header equivalent (limited support). |
| `charset` | string | Character encoding declaration (e.g., "utf-8"). |
| `hidden` | string | Controls visibility. Set to "hidden" to hide, or omit to show. |

### Metadata Name Values

The `name` attribute accepts the following values for PDF document properties:

| Name Value | Content | PDF Property | Description |
|------------|---------|--------------|-------------|
| `author` | string | Document Author | Sets the author of the PDF document |
| `description` | string | Document Subject | Sets the subject/description of the PDF |
| `keywords` | string | Document Keywords | Sets searchable keywords (comma-separated) |
| `generator` | string | Document Producer | Sets the producer/generator application name |

### PDF Security Name Values

Special `name` values for PDF security and encryption:

| Name Value | Content | Description |
|------------|---------|-------------|
| `print-restrictions` | permissions | Sets document permission restrictions |
| `print-encryption` | encryption level | Sets encryption security level |

### Print Restrictions Values

For `name="print-restrictions"`, the `content` attribute accepts:

| Content Value | Description |
|---------------|-------------|
| `none` | No restrictions - all permissions allowed (default) |
| `all` | All restrictions - block all permissions except accessibility |
| `printing` or `allow-printing` | Allow printing (high quality) |
| `accessibility` or `allow-accessibility` | Allow accessibility features |
| `annotations` or `allow-annotations` | Allow adding annotations/comments |
| `copying` or `allow-copying` | Allow text/graphics copying |
| `modifications` or `allow-modifications` | Allow document modifications |
| `forms` or `allow-forms` | Allow form field filling |

Multiple permissions can be space or comma-separated:
```html
<meta name="print-restrictions" content="printing, accessibility, copying" />
```

### Print Encryption Values

For `name="print-encryption"`, the `content` attribute accepts:

| Content Value | Security Type | Description |
|---------------|---------------|-------------|
| `40bit` | Standard 40-bit | PDF v1.2 standard 40-bit encryption (legacy) |
| `128bit` | Standard 128-bit | PDF v2.3 standard 128-bit encryption (recommended) |

---

## Notes

### Document Properties

The `<meta>` element sets standard PDF document properties that appear in the Document Properties dialog of PDF readers:

1. **Author** (`name="author"`): The person or organization that created the document
2. **Subject** (`name="description"`): A description of the document's content or purpose
3. **Keywords** (`name="keywords"`): Searchable keywords for document indexing
4. **Producer** (`name="generator"`): The application that generated the PDF

These properties are embedded in the PDF metadata dictionary and can be used for searching, cataloging, and document management.

### PDF Security and Restrictions

The `print-restrictions` meta tag controls what users can do with the PDF:

**Permission Model**:
- By default, all permissions are allowed (`content="none"`)
- Setting `content="all"` blocks everything except accessibility
- Specify individual permissions to allow only those actions
- Permissions are enforced by PDF readers that respect security settings

**Common Permission Combinations**:

1. **View Only** (no printing, copying, or modifications):
   ```html
   <meta name="print-restrictions" content="accessibility" />
   ```

2. **View and Print** (but no copying or editing):
   ```html
   <meta name="print-restrictions" content="printing, accessibility" />
   ```

3. **View, Print, and Copy** (but no editing):
   ```html
   <meta name="print-restrictions" content="printing, accessibility, copying" />
   ```

4. **Full Access** (no restrictions):
   ```html
   <meta name="print-restrictions" content="none" />
   ```

### Encryption Levels

PDF encryption protects document content and enforces restrictions:

**40-bit Encryption** (PDF 1.2):
- Legacy compatibility mode
- Lower security strength
- Compatible with older PDF readers
- Use only when compatibility is critical

**128-bit Encryption** (PDF 2.3):
- Modern standard encryption
- Strong security protection
- Recommended for all new documents
- Supported by all modern PDF readers

### Processing During Data Binding

Meta elements are processed by the parent `<head>` element during data binding:

1. The `<head>` element iterates through all child `<meta>` elements
2. Standard metadata (`author`, `description`, `keywords`, `generator`) updates document info
3. Security meta tags (`print-restrictions`, `print-encryption`) configure document permissions
4. Unknown meta tag names are logged and ignored

### Character Encoding

The `charset` attribute declares the character encoding:

```html
<meta charset="utf-8" />
```

While this is primarily informational in Scryber (encoding is handled automatically), it's good practice to include it for standards compliance.

### Class Hierarchy

In the Scryber codebase:
- `HTMLMeta` extends `Component`
- Simple data container with string properties
- Does not participate in layout or rendering
- Processed by parent `HTMLHead` component

---

## Examples

### Basic Document Metadata

```html
<head>
    <title>Annual Report 2025</title>
    <meta charset="utf-8" />
    <meta name="author" content="Finance Department" />
    <meta name="description" content="Annual financial report for fiscal year 2025" />
    <meta name="keywords" content="finance, annual report, 2025, revenue, expenses" />
</head>
```

### Document with Generator Information

```html
<head>
    <title>Sales Report</title>
    <meta name="author" content="Sales Team" />
    <meta name="description" content="Quarterly sales performance report" />
    <meta name="generator" content="SalesReportGenerator v3.0 using Scryber" />
    <meta name="keywords" content="sales, Q3, report, performance" />
</head>
```

### Protected Document with Printing Allowed

```html
<head>
    <title>Confidential Report</title>
    <meta name="author" content="Executive Team" />
    <meta name="description" content="Confidential strategic planning document" />

    <!-- Allow printing and accessibility only -->
    <meta name="print-restrictions" content="printing, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### View-Only Document (Maximum Restrictions)

```html
<head>
    <title>Restricted Document</title>
    <meta name="author" content="Legal Department" />
    <meta name="description" content="Legally protected document - view only" />

    <!-- Block all actions except accessibility -->
    <meta name="print-restrictions" content="accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Document with Full Permissions

```html
<head>
    <title>Public Document</title>
    <meta name="author" content="Public Relations" />
    <meta name="description" content="Public information document" />

    <!-- No restrictions - all permissions allowed -->
    <meta name="print-restrictions" content="none" />
</head>
```

### Document with Multiple Permissions

```html
<head>
    <title>User Manual</title>
    <meta name="author" content="Documentation Team" />
    <meta name="description" content="Product user manual and guide" />
    <meta name="keywords" content="manual, guide, instructions, help" />

    <!-- Allow printing, copying, and accessibility -->
    <meta name="print-restrictions" content="printing, copying, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Form Document with Fill Permissions

```html
<head>
    <title>Application Form</title>
    <meta name="author" content="HR Department" />
    <meta name="description" content="Employee application form" />

    <!-- Allow form filling, printing, and accessibility -->
    <meta name="print-restrictions" content="forms, printing, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Document Allowing Annotations

```html
<head>
    <title>Review Document</title>
    <meta name="author" content="Review Committee" />
    <meta name="description" content="Document for review with comments" />

    <!-- Allow annotations, printing, and accessibility -->
    <meta name="print-restrictions" content="annotations, printing, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Legacy Compatibility Document

```html
<head>
    <title>Legacy Document</title>
    <meta name="author" content="Archive Department" />
    <meta name="description" content="Document for legacy PDF readers" />

    <!-- Use 40-bit encryption for older readers -->
    <meta name="print-restrictions" content="printing, accessibility" />
    <meta name="print-encryption" content="40bit" />
</head>
```

### Complete Security Configuration

```html
<head>
    <title>Secure Document</title>
    <meta charset="utf-8" />
    <meta name="author" content="Security Department" />
    <meta name="description" content="Secure document with controlled access" />
    <meta name="keywords" content="secure, confidential, restricted" />
    <meta name="generator" content="SecureDoc Generator v1.0" />

    <!-- Allow printing and accessibility, block modifications -->
    <meta name="print-restrictions" content="printing, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Academic Paper Metadata

```html
<head>
    <title>Research Paper: PDF Generation Techniques</title>
    <meta charset="utf-8" />
    <meta name="author" content="Dr. Jane Smith, Dr. John Doe" />
    <meta name="description" content="A comprehensive study of modern PDF generation techniques and performance optimization strategies" />
    <meta name="keywords" content="PDF, generation, research, performance, optimization, document processing" />
    <meta name="generator" content="Academic Paper Generator" />

    <!-- Allow copying for citations, but not modifications -->
    <meta name="print-restrictions" content="printing, copying, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Corporate Template Metadata

```html
<head>
    <title>{{model.documentTitle}}</title>
    <meta charset="utf-8" />
    <meta name="author" content="{{model.author}}" />
    <meta name="description" content="{{model.description}}" />
    <meta name="keywords" content="{{model.keywords}}" />
    <meta name="generator" content="Corporate Template System v2.0" />

    <!-- Dynamic security based on model -->
    <meta name="print-restrictions" content="{{model.permissions}}" />
    <meta name="print-encryption" content="{{model.encryptionLevel}}" />
</head>
```

### Invoice with Restricted Copying

```html
<head>
    <title>Invoice #{{model.invoiceNumber}}</title>
    <meta name="author" content="{{model.companyName}}" />
    <meta name="description" content="Invoice for {{model.clientName}} - {{model.invoiceDate}}" />
    <meta name="keywords" content="invoice, billing, {{model.invoiceNumber}}" />

    <!-- Allow printing but prevent copying -->
    <meta name="print-restrictions" content="printing, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Contract Document

```html
<head>
    <title>Service Agreement Contract</title>
    <meta name="author" content="Legal Department - Acme Corporation" />
    <meta name="description" content="Service agreement between Acme Corporation and Client" />
    <meta name="keywords" content="contract, agreement, legal, terms, service" />

    <!-- Allow printing and form filling (for signatures) -->
    <meta name="print-restrictions" content="printing, forms, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Newsletter Metadata

```html
<head>
    <title>Monthly Newsletter - October 2025</title>
    <meta charset="utf-8" />
    <meta name="author" content="Marketing Department" />
    <meta name="description" content="Company newsletter featuring updates, news, and announcements for October 2025" />
    <meta name="keywords" content="newsletter, october, 2025, news, updates, company" />
    <meta name="generator" content="Newsletter Generator v4.2" />

    <!-- Public document - no restrictions -->
    <meta name="print-restrictions" content="none" />
</head>
```

### Technical Documentation

```html
<head>
    <title>API Reference Documentation</title>
    <meta charset="utf-8" />
    <meta name="author" content="Engineering Team" />
    <meta name="description" content="Complete API reference documentation for Scryber PDF Library v2.0" />
    <meta name="keywords" content="API, documentation, reference, Scryber, PDF, programming" />
    <meta name="generator" content="DocGen v1.5" />

    <!-- Allow copying for code examples -->
    <meta name="print-restrictions" content="printing, copying, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Multi-Author Document

```html
<head>
    <title>Collaborative Research Report</title>
    <meta charset="utf-8" />
    <meta name="author" content="Dr. Alice Johnson, Prof. Bob Chen, Dr. Carol Williams, Dr. David Brown" />
    <meta name="description" content="Collaborative research findings on machine learning applications in document processing" />
    <meta name="keywords" content="research, collaboration, machine learning, document processing, AI" />

    <!-- Allow annotations for peer review -->
    <meta name="print-restrictions" content="printing, copying, annotations, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Compliance Document

```html
<head>
    <title>GDPR Compliance Report 2025</title>
    <meta charset="utf-8" />
    <meta name="author" content="Compliance Officer - Data Protection Team" />
    <meta name="description" content="Annual GDPR compliance report and audit results for fiscal year 2025" />
    <meta name="keywords" content="GDPR, compliance, data protection, privacy, audit, 2025" />
    <meta name="generator" content="Compliance Reporting System v3.1" />

    <!-- Strict restrictions for compliance documents -->
    <meta name="print-restrictions" content="accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Educational Material

```html
<head>
    <title>Introduction to Programming - Chapter 5</title>
    <meta charset="utf-8" />
    <meta name="author" content="Prof. Sarah Martinez, Computer Science Department" />
    <meta name="description" content="Educational material covering loops, functions, and data structures in Python programming" />
    <meta name="keywords" content="education, programming, python, tutorial, computer science" />

    <!-- Allow copying for students but prevent modifications -->
    <meta name="print-restrictions" content="printing, copying, accessibility" />
    <meta name="print-encryption" content="128bit" />
</head>
```

### Product Specification Sheet

```html
<head>
    <title>Product Specification: Model XYZ-2000</title>
    <meta charset="utf-8" />
    <meta name="author" content="Product Management Team" />
    <meta name="description" content="Detailed technical specifications for XYZ-2000 model including dimensions, features, and compatibility" />
    <meta name="keywords" content="product, specification, XYZ-2000, technical, features" />

    <!-- Public spec sheet - all permissions -->
    <meta name="print-restrictions" content="none" />
</head>
```

---

## See Also

- [head](/reference/htmltags/head.html) - Head element (container for meta tags)
- [title](/reference/htmltags/title.html) - Title element for document title
- [link](/reference/htmltags/link.html) - Link element for external stylesheets
- [Document Component](/reference/components/document.html) - Base document component
- [Document Properties](/reference/document/properties.html) - PDF document properties
- [Document Info](/reference/document/info.html) - Document information API
- [Security Settings](/reference/security/) - PDF security and encryption
- [Permissions](/reference/security/permissions.html) - PDF permission types
- [Encryption](/reference/security/encryption.html) - PDF encryption levels

---
