---
layout: default
title: embed
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;embed&gt; : The Embedded Content Element

Summary: The `<embed>` element includes the content of an external local or remote file directly into the PDF document. The external file is loaded, parsed, and rendered as part of the current document, enabling modular document composition and content reuse.

## Usage

The `<embed>` element loads and includes external HTML or XML content into your PDF document. The content is fetched during document generation and becomes part of the document structure. This allows you to create reusable content fragments and build complex documents from multiple sources.

```html
<embed src="./templates/header.html" />
```

Remote content is also supported:

```html
<embed src="https://example.com/templates/footer.html" />
```

---

## Supported Attributes

- **src** - **Required**. The path or URL to the external content file to load and include
- **id** - Unique identifier for the element
- **class** - CSS class name(s) for styling
- **style** - Inline CSS styles
- **title** - Sets the outline/bookmark title for the embedded content
- **hidden** - Controls visibility. Set to `"hidden"` to hide the element

Standard CSS properties are supported: `width`, `height`, `margin`, `padding`, `border`, `background-color`, `display`, `position`, `float`, `opacity`.

---

## Data Binding

The embed element fully supports data binding for dynamic content sources and conditional inclusion.

### Basic Data Binding

Bind the source path dynamically:

```html
<embed src="{{model.templatePath}}" />
```

### Conditional Visibility

Show or hide embedded content based on data:

```html
<embed src="./sections/disclaimer.html"
       hidden="{{if(model.includeDisclaimer, '', 'hidden')}}" />
```

### Dynamic Path Construction

Build paths dynamically from multiple data values:

```html
<embed src="templates/{{model.language}}/{{model.section}}.html" />
```

---

## Notes

- The embedded content is loaded and parsed at document generation time, not at viewing time
- Embedded content becomes part of the parent document's content tree
- External content can be HTML fragments or complete HTML documents
- Both local file paths and remote URLs (HTTP/HTTPS) are supported
- File paths are resolved relative to the current document or as absolute paths
- The `<embed>` element always inherits styles from the parent document
- For style isolation, use `<iframe>` instead with `data-passthrough="false"`
- If the external content fails to load in Strict mode, an error is thrown
- In Lax conformance mode, loading errors are logged but generation continues
- Embedded content can contain data binding expressions that are evaluated in the parent document's data context
- Multiple `<embed>` elements can reference the same external file
- Embedded content can itself contain additional `<embed>` elements (nesting is supported)
- The element is self-closing in HTML syntax: `<embed src="..." />`

---

## Examples

### Example 1: Simple Content Inclusion

Include a header template in your document:

```html
<!DOCTYPE html>
<html>
<head>
    <title>My Document</title>
</head>
<body>
    <embed src="./partials/header.html" />

    <div class="content">
        <h1>Main Content</h1>
        <p>This is the main document content.</p>
    </div>

    <embed src="./partials/footer.html" />
</body>
</html>
```

### Example 2: Dynamic Template Loading

Load templates based on data values:

```html
<embed src="{{model.headerTemplate}}" />

<div>
    <h1>{{model.title}}</h1>
    <p>{{model.description}}</p>
</div>

<embed src="{{model.footerTemplate}}" />
```

With data:
```json
{
    "model": {
        "headerTemplate": "./headers/corporate-header.html",
        "footerTemplate": "./footers/standard-footer.html",
        "title": "Annual Report",
        "description": "Financial results for 2025"
    }
}
```

### Example 3: Multi-Language Support

Load language-specific content:

```html
<embed src="i18n/{{model.language}}/header.html" />

<div class="content">
    <embed src="i18n/{{model.language}}/welcome-message.html" />
</div>

<embed src="i18n/{{model.language}}/footer.html" />
```

With data:
```json
{
    "model": {
        "language": "es"
    }
}
```

### Example 4: Conditional Content Inclusion

Include content only when conditions are met:

```html
<div class="document">
    <h1>{{model.reportTitle}}</h1>

    <embed src="./sections/summary.html" />

    <embed src="./sections/detailed-analysis.html"
           hidden="{{if(model.includeDetails, '', 'hidden')}}" />

    <embed src="./sections/appendix.html"
           hidden="{{if(model.includeAppendix, '', 'hidden')}}" />
</div>
```

With data:
```json
{
    "model": {
        "reportTitle": "Quarterly Report",
        "includeDetails": true,
        "includeAppendix": false
    }
}
```

### Example 5: Reusable Table Components

Include reusable table structures:

```html
<h2>Sales Data</h2>
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <embed src="./components/sales-table-header.html" />
    </thead>
    <tbody>
        {{#each salesData}}
        <tr>
            <td>{{this.region}}</td>
            <td>{{this.amount}}</td>
            <td>{{this.growth}}</td>
        </tr>
        {{/each}}
    </tbody>
    <tfoot>
        <embed src="./components/table-footer.html" />
    </tfoot>
</table>
```

### Example 6: Remote Content Loading

Load content from remote URLs:

```html
<div class="document">
    <!-- Load company header from CDN -->
    <embed src="https://cdn.company.com/templates/header.html" />

    <div class="content">
        <p>Document content here...</p>
    </div>

    <!-- Load disclaimer from remote server -->
    <embed src="https://legal.company.com/disclaimers/standard.html" />
</div>
```

### Example 7: Regional Content Variations

Include region-specific content:

```html
<div class="invoice">
    <embed src="headers/{{model.country}}/invoice-header.html" />

    <div class="invoice-body">
        <p>Invoice #{{model.invoiceNumber}}</p>
        <p>Total: {{model.total}}</p>
    </div>

    <embed src="footers/{{model.country}}/{{model.state}}/legal-footer.html" />
</div>
```

With data:
```json
{
    "model": {
        "country": "US",
        "state": "CA",
        "invoiceNumber": "INV-2025-001",
        "total": "$1,250.00"
    }
}
```

### Example 8: Modular Document Assembly

Build complex documents from multiple parts:

```html
<!DOCTYPE html>
<html>
<head>
    <title>{{model.documentTitle}}</title>
</head>
<body>
    <!-- Cover page -->
    <div style="page-break-after: always;">
        <embed src="sections/cover.html" />
    </div>

    <!-- Table of contents -->
    <div style="page-break-after: always;">
        <embed src="sections/toc.html" />
    </div>

    <!-- Chapters -->
    <div style="page-break-after: always;">
        <embed src="chapters/chapter1.html" />
    </div>

    <div style="page-break-after: always;">
        <embed src="chapters/chapter2.html" />
    </div>

    <div>
        <embed src="chapters/chapter3.html" />
    </div>
</body>
</html>
```

### Example 9: API-Driven Content

Load content from API endpoints:

```html
<div class="report">
    <embed src="{{model.apiUrl}}/reports/{{model.reportId}}/header.html" />

    <div class="report-body">
        <embed src="{{model.apiUrl}}/reports/{{model.reportId}}/content.html" />
    </div>

    <embed src="{{model.apiUrl}}/reports/{{model.reportId}}/footer.html" />
</div>
```

With data:
```json
{
    "model": {
        "apiUrl": "https://api.example.com",
        "reportId": "12345"
    }
}
```

### Example 10: Styled Embedded Content

Apply styling to the embedded content container:

```html
<style>
    .embedded-section {
        margin: 20pt 0;
        padding: 15pt;
        border: 1pt solid #ddd;
        background-color: #f9f9f9;
    }
</style>

<div class="embedded-section">
    <embed src="./sections/important-notice.html" />
</div>
```

### Example 11: Template Versioning

Load different template versions:

```html
<embed src="templates/v{{model.templateVersion}}/header.html" />

<div class="content">
    <h1>{{model.title}}</h1>
    <embed src="templates/v{{model.templateVersion}}/body.html" />
</div>

<embed src="templates/v{{model.templateVersion}}/footer.html" />
```

With data:
```json
{
    "model": {
        "templateVersion": "2",
        "title": "Updated Document Format"
    }
}
```

### Example 12: Signature Blocks

Include signature blocks from shared templates:

```html
<div class="letter">
    <p>Dear {{model.recipientName}},</p>

    <p>{{model.messageBody}}</p>

    <p>Sincerely,</p>

    <embed src="signatures/{{model.signerDepartment}}.html" />
</div>
```

With data:
```json
{
    "model": {
        "recipientName": "John Smith",
        "messageBody": "We are pleased to inform you...",
        "signerDepartment": "sales"
    }
}
```

### Example 13: Form Sections

Build forms from reusable sections:

```html
<div class="application-form">
    <h1>Employment Application</h1>

    <embed src="form-sections/personal-info.html" />

    <embed src="form-sections/employment-history.html" />

    <embed src="form-sections/education.html" />

    <embed src="form-sections/references.html" />

    <embed src="form-sections/legal-declarations.html" />
</div>
```

### Example 14: Customer-Specific Content

Include customer-specific templates:

```html
<div class="contract">
    <embed src="contracts/{{model.customerType}}/header.html" />

    <div class="contract-body">
        <h2>Terms and Conditions</h2>
        <embed src="contracts/{{model.customerType}}/terms.html" />
    </div>

    <embed src="contracts/{{model.customerType}}/signature-page.html" />
</div>
```

With data:
```json
{
    "model": {
        "customerType": "enterprise"
    }
}
```

### Example 15: Dynamic Section Lists

Include sections from a data-driven list:

```html
<div class="report">
    <h1>{{model.reportTitle}}</h1>

    {{#each model.sections}}
    <div class="section" style="page-break-after: {{this.pageBreak}};">
        <embed src="report-sections/{{this.filename}}" />
    </div>
    {{/each}}
</div>
```

With data:
```json
{
    "model": {
        "reportTitle": "Annual Report",
        "sections": [
            {"filename": "executive-summary.html", "pageBreak": "always"},
            {"filename": "financial-overview.html", "pageBreak": "always"},
            {"filename": "operations.html", "pageBreak": "always"},
            {"filename": "outlook.html", "pageBreak": "auto"}
        ]
    }
}
```

### Example 16: Conditional Disclaimers

Include disclaimers based on document type:

```html
<div class="document">
    <h1>{{model.documentType}} Document</h1>

    <div class="content">
        <p>{{model.content}}</p>
    </div>

    {{#if model.requiresLegalDisclaimer}}
    <div style="margin-top: 30pt; font-size: 8pt;">
        <embed src="disclaimers/legal.html" />
    </div>
    {{/if}}

    {{#if model.requiresFinancialDisclaimer}}
    <div style="margin-top: 10pt; font-size: 8pt;">
        <embed src="disclaimers/financial.html" />
    </div>
    {{/if}}
</div>
```

With data:
```json
{
    "model": {
        "documentType": "Investment",
        "content": "Investment opportunity details...",
        "requiresLegalDisclaimer": true,
        "requiresFinancialDisclaimer": true
    }
}
```

### Example 17: Nested Embeds with Parameters

Store parameters and use in embedded content:

```html
<var data-id="companyName" data-value="{{model.company}}" />
<var data-id="reportYear" data-value="{{model.year}}" />

<div class="annual-report">
    <embed src="./sections/cover.html" />
    <!-- cover.html can access {{Document.Params.companyName}} and {{Document.Params.reportYear}} -->

    <embed src="./sections/letter-to-shareholders.html" />

    <embed src="./sections/financial-highlights.html" />
</div>
```

With data:
```json
{
    "model": {
        "company": "Acme Corporation",
        "year": "2025"
    }
}
```

### Example 18: Email Templates

Build email-style documents from components:

```html
<div class="email-document">
    <!-- Email header -->
    <embed src="email/header.html" />

    <!-- Email body -->
    <div style="margin: 20pt; font-size: 11pt;">
        <p>Dear {{model.recipientName}},</p>

        <embed src="email/body-templates/{{model.messageType}}.html" />

        <p>Best regards,</p>
        <embed src="email/signatures/{{model.sender}}.html" />
    </div>

    <!-- Email footer -->
    <embed src="email/footer.html" />
</div>
```

With data:
```json
{
    "model": {
        "recipientName": "Jane Doe",
        "messageType": "welcome",
        "sender": "customer-support"
    }
}
```

### Example 19: Product Catalogs

Assemble product pages from templates:

```html
<div class="catalog">
    <embed src="catalog/cover.html" />

    {{#each model.productCategories}}
    <div style="page-break-before: always;">
        <h1>{{this.categoryName}}</h1>
        <embed src="catalog/categories/{{this.templateFile}}" />
    </div>
    {{/each}}

    <embed src="catalog/order-form.html" />
</div>
```

With data:
```json
{
    "model": {
        "productCategories": [
            {"categoryName": "Electronics", "templateFile": "electronics.html"},
            {"categoryName": "Accessories", "templateFile": "accessories.html"},
            {"categoryName": "Software", "templateFile": "software.html"}
        ]
    }
}
```

### Example 20: Context-Aware Content

Include content based on multiple conditions:

```html
<var data-id="documentType" data-value="{{model.type}}" />
<var data-id="securityLevel" data-value="{{model.security}}" />

<div class="secure-document">
    <!-- Header varies by security level -->
    <embed src="headers/{{model.security}}-security-header.html" />

    <!-- Main content -->
    <div class="content">
        <h1>{{model.title}}</h1>
        <embed src="content/{{model.type}}/main.html" />
    </div>

    <!-- Conditional watermark -->
    {{#if model.isConfidential}}
    <div style="position: absolute; top: 50%; left: 50%; transform: rotate(-45deg);
                opacity: 0.1; font-size: 72pt; color: red;">
        <embed src="watermarks/confidential.html" />
    </div>
    {{/if}}

    <!-- Footer varies by document type and security -->
    <embed src="footers/{{model.type}}-{{model.security}}-footer.html" />

    <!-- Audit trail for classified documents -->
    {{#if model.requiresAudit}}
    <div style="page-break-before: always; font-size: 8pt;">
        <embed src="audit/audit-trail.html" />
    </div>
    {{/if}}
</div>
```

With data:
```json
{
    "model": {
        "type": "financial",
        "security": "high",
        "title": "Q4 Financial Projections",
        "isConfidential": true,
        "requiresAudit": true
    }
}
```

---

## See Also

- [&lt;iframe&gt; - Inline Frame](/reference/htmltags/iframe.html) for embedded content with style isolation
- [&lt;object&gt; - File Attachments](/reference/htmltags/object.html) for attaching files to the PDF
- [&lt;template&gt; - Template Element](/reference/htmltags/template.html) for data-driven repetition
- [&lt;var&gt; - Variables](/reference/htmltags/var.html) for storing document parameters
- [Data Binding](/reference/data-binding.html) for dynamic content
- [Document Parameters](/reference/document-params.html) for sharing data across included content
- [Remote Resources](/reference/resources.html) for loading remote content

---
