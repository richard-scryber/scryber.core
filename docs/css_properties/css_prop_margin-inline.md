---
layout: default
title: margin-inline
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# margin-inline : Inline Margin Shorthand Property

The `margin-inline` property is a logical shorthand for setting both `margin-inline-start` and `margin-inline-end` of an element in PDF documents. This property adapts to the writing direction, making it ideal for creating internationalized documents that work in both LTR and RTL languages.

## Usage

```css
selector {
    margin-inline: value;
}
```

The margin-inline property accepts 1-2 space-separated values with the following behavior:
- **1 value**: Applies to both inline-start and inline-end
- **2 values**: First value for inline-start, second for inline-end

---

## Supported Values

### Length Units
- Points: `10pt`, `15pt`, `20pt`
- Pixels: `10px`, `15px`, `20px`
- Inches: `0.5in`, `1in`
- Centimeters: `2cm`, `3cm`
- Millimeters: `10mm`, `20mm`
- Ems: `1em`, `1.5em`, `2em`
- Percentage: `5%`, `10%`, `15%` (relative to parent width)

### Special Values
- `0` - No inline margins
- `auto` - Automatic margins (useful for centering)

---

## Supported Elements

The `margin-inline` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables (`<table>`)
- Lists (`<ul>`, `<ol>`)
- Images (`<img>`)
- All container elements

---

## Notes

- This is a logical shorthand property that adapts to text direction
- In LTR contexts, it controls left and right margins
- In RTL contexts, it controls right and left margins
- Provides semantic clarity for horizontal spacing
- Useful for creating bidirectional layouts
- Does not collapse with adjacent margins
- The `auto` value with both sides can center block elements
- Simplifies maintenance of internationalized documents

---

## Data Binding

The `margin-inline` property supports dynamic values through data binding, enabling you to create direction-aware, flexible horizontal spacing that adapts to document layouts and internationalization requirements.

### Example 1: Dynamic document margins based on page size

```html
<style>
    .document {
        margin-inline: {{page.size === 'A4' ? '50pt' : '72pt'}};
        padding: 20pt;
    }
    .document h1 {
        margin-bottom: 20pt;
        font-size: 24pt;
    }
    .document .highlight {
        margin-inline: {{layout.highlightIndent}}pt;
        padding: 15pt;
        background-color: #fef3c7;
        border-inline-start: 4pt solid #f59e0b;
    }
</style>
<body>
    <div class="document">
        <h1>Document Title</h1>
        <p>Regular content with page-size-based margins.</p>
        <div class="highlight">
            <p>Highlighted section with additional indentation.</p>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "page": {
        "size": "A4"
    },
    "layout": {
        "highlightIndent": 30
    }
}
```

### Example 2: Responsive invoice layout with data-driven spacing

```html
<style>
    .invoice {
        margin-inline: {{layout.pageMargin}}pt;
        padding: 30pt;
    }
    .invoice-header {
        margin-inline: 0;
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e3a8a;
        color: white;
        text-align: center;
    }
    .invoice-section {
        margin-inline: {{layout.sectionIndent}}pt;
        margin-bottom: 25pt;
        padding: 15pt;
        background-color: #f9fafb;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1>INVOICE</h1>
            <p>{{invoice.number}}</p>
        </div>
        <div class="invoice-section">
            <strong>Bill To:</strong> {{invoice.customer}}
        </div>
    </div>
</body>
```

Data context:
```json
{
    "layout": {
        "pageMargin": 40,
        "sectionIndent": 20
    },
    "invoice": {
        "number": "INV-2025-001",
        "customer": "Acme Corporation"
    }
}
```

### Example 3: Centered card with adaptive margins

```html
<style>
    .page {
        padding: 30pt;
    }
    .centered-card {
        width: {{card.width}}pt;
        margin-inline: auto;
        margin-bottom: 20pt;
        padding: {{card.padding}}pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        text-align: center;
    }
    .card-content {
        margin-inline: {{card.contentIndent}}pt;
    }
</style>
<body>
    <div class="page">
        <div class="centered-card">
            <h2>{{card.title}}</h2>
            <div class="card-content">
                <p>{{card.description}}</p>
            </div>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "card": {
        "width": 400,
        "padding": 20,
        "contentIndent": 15,
        "title": "Welcome",
        "description": "This card uses data-driven margins for consistent layout."
    }
}
```

---

## Examples

### Example 1: Uniform inline margins

```html
<style>
    .container {
        padding: 30pt;
    }
    .box {
        margin-inline: 20pt;
        padding: 15pt;
        background-color: #dbeafe;
    }
</style>
<body>
    <div class="container">
        <div class="box">
            <p>This box has 20pt margins on both inline sides.</p>
        </div>
    </div>
</body>
```

### Example 2: Asymmetric inline margins

```html
<style>
    .content {
        padding: 30pt;
    }
    .indented-box {
        margin-inline: 40pt 20pt;
        padding: 15pt;
        background-color: #dcfce7;
        border-inline-start: 4pt solid #16a34a;
    }
</style>
<body>
    <div class="content">
        <div class="indented-box">
            <p>40pt margin at inline-start, 20pt at inline-end.</p>
        </div>
    </div>
</body>
```

### Example 3: Centered content with auto margins

```html
<style>
    .page {
        padding: 30pt;
    }
    .centered-card {
        width: 400pt;
        margin-inline: auto;
        margin-bottom: 20pt;
        padding: 20pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        text-align: center;
    }
</style>
<body>
    <div class="page">
        <div class="centered-card">
            <h2>Centered Card</h2>
            <p>This card is horizontally centered using margin-inline: auto.</p>
        </div>
    </div>
</body>
```

### Example 4: Document layout with inline margins

```html
<style>
    .document {
        margin-inline: 50pt;
        padding: 20pt;
    }
    .document h1 {
        margin-bottom: 20pt;
        font-size: 24pt;
    }
    .document p {
        margin-bottom: 12pt;
        line-height: 1.6;
    }
    .document .highlight {
        margin-inline: 30pt;
        padding: 15pt;
        background-color: #fef3c7;
        border-inline-start: 4pt solid #f59e0b;
    }
</style>
<body>
    <div class="document">
        <h1>Document Title</h1>
        <p>Regular paragraph with document margins.</p>
        <div class="highlight">
            <p>Highlighted section with additional inline margins.</p>
        </div>
        <p>Continuation of the document.</p>
    </div>
</body>
```

### Example 5: Invoice with inline margins

```html
<style>
    .invoice {
        margin-inline: 40pt;
        padding: 30pt;
    }
    .invoice-header {
        margin-inline: 0;
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e3a8a;
        color: white;
        text-align: center;
    }
    .invoice-section {
        margin-inline: 20pt;
        margin-bottom: 25pt;
        padding: 15pt;
        background-color: #f9fafb;
    }
    .section-title {
        margin-bottom: 10pt;
        font-weight: bold;
        font-size: 14pt;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1>INVOICE</h1>
            <p>INV-2025-001</p>
        </div>
        <div class="invoice-section">
            <div class="section-title">Bill To</div>
            <p>Acme Corporation<br/>123 Business St.</p>
        </div>
        <div class="invoice-section">
            <div class="section-title">Invoice Details</div>
            <p>Date: January 15, 2025<br/>Due: February 15, 2025</p>
        </div>
    </div>
</body>
```

### Example 6: Card grid with inline spacing

```html
<style>
    .card-grid {
        margin-inline: 25pt;
        padding: 20pt;
    }
    .card {
        margin-inline: 10pt;
        margin-bottom: 15pt;
        padding: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .card h3 {
        margin-bottom: 10pt;
        font-size: 14pt;
        color: #1f2937;
    }
    .card p {
        margin-bottom: 0;
        color: #6b7280;
        font-size: 11pt;
    }
</style>
<body>
    <div class="card-grid">
        <div class="card">
            <h3>Feature One</h3>
            <p>Description of the first feature.</p>
        </div>
        <div class="card">
            <h3>Feature Two</h3>
            <p>Description of the second feature.</p>
        </div>
        <div class="card">
            <h3>Feature Three</h3>
            <p>Description of the third feature.</p>
        </div>
    </div>
</body>
```

### Example 7: Form layout with inline margins

```html
<style>
    .form {
        margin-inline: 30pt;
        padding: 25pt;
    }
    .form-title {
        margin-inline: 0;
        margin-bottom: 25pt;
        font-size: 20pt;
        font-weight: bold;
        text-align: center;
    }
    .form-group {
        margin-inline: 0;
        margin-bottom: 15pt;
    }
    .form-label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
    }
    .form-input {
        width: 100%;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .form-button {
        margin-inline: auto;
        display: block;
        padding: 10pt 30pt;
        background-color: #2563eb;
        color: white;
        border: none;
    }
</style>
<body>
    <div class="form">
        <h2 class="form-title">Registration Form</h2>
        <div class="form-group">
            <label class="form-label">Full Name</label>
            <input class="form-input" type="text" />
        </div>
        <div class="form-group">
            <label class="form-label">Email Address</label>
            <input class="form-input" type="email" />
        </div>
        <button class="form-button">Submit</button>
    </div>
</body>
```

### Example 8: Table with inline margins

```html
<style>
    .report {
        margin-inline: 30pt;
        padding: 20pt;
    }
    .report-title {
        margin-inline: 0;
        margin-bottom: 20pt;
        font-size: 22pt;
        text-align: center;
    }
    .data-table {
        margin-inline: 10pt;
        margin-bottom: 20pt;
        width: calc(100% - 20pt);
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        padding: 10pt;
        border: 1pt solid #d1d5db;
    }
    .data-table th {
        background-color: #1f2937;
        color: white;
    }
</style>
<body>
    <div class="report">
        <h1 class="report-title">Sales Report</h1>
        <table class="data-table">
            <thead>
                <tr>
                    <th>Product</th>
                    <th>Quantity</th>
                    <th>Revenue</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Widget A</td>
                    <td>150</td>
                    <td>$15,000</td>
                </tr>
                <tr>
                    <td>Widget B</td>
                    <td>230</td>
                    <td>$23,000</td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
```

### Example 9: Alert boxes with inline margins

```html
<style>
    .alerts {
        margin-inline: 20pt;
        padding: 25pt;
    }
    .alert {
        margin-inline: 15pt;
        margin-bottom: 12pt;
        padding: 12pt;
        border-inline-start: 4pt solid;
        border-radius: 4pt;
    }
    .alert-info {
        background-color: #dbeafe;
        border-inline-start-color: #3b82f6;
    }
    .alert-success {
        background-color: #dcfce7;
        border-inline-start-color: #16a34a;
    }
    .alert-warning {
        background-color: #fef3c7;
        border-inline-start-color: #f59e0b;
    }
</style>
<body>
    <div class="alerts">
        <div class="alert alert-info">
            <strong>Info:</strong> System update in progress.
        </div>
        <div class="alert alert-success">
            <strong>Success:</strong> Operation completed successfully.
        </div>
        <div class="alert alert-warning">
            <strong>Warning:</strong> Action requires confirmation.
        </div>
    </div>
</body>
```

### Example 10: Quote block with inline margins

```html
<style>
    .article {
        margin-inline: 50pt;
        padding: 30pt;
    }
    .article p {
        margin-bottom: 12pt;
        line-height: 1.6;
    }
    .quote-block {
        margin-inline: 40pt;
        margin-bottom: 20pt;
        padding: 15pt;
        background-color: #f5f5f5;
        border-inline-start: 4pt solid #6366f1;
        font-style: italic;
    }
    .quote-author {
        margin-top: 10pt;
        margin-inline: 20pt;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="article">
        <p>Article text before the quote.</p>
        <div class="quote-block">
            <p>"Simplicity is the ultimate sophistication."</p>
            <p class="quote-author">— Leonardo da Vinci</p>
        </div>
        <p>Article continues after the quote.</p>
    </div>
</body>
```

### Example 11: Newsletter layout

```html
<style>
    .newsletter {
        margin-inline: 30pt;
        padding: 25pt;
    }
    .newsletter-header {
        margin-inline: 0;
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
    .newsletter-section {
        margin-inline: 15pt;
        margin-bottom: 25pt;
        padding: 15pt;
        background-color: #f9fafb;
    }
    .section-title {
        margin-inline: 0;
        margin-bottom: 10pt;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>Monthly Newsletter</h1>
            <p>January 2025</p>
        </div>
        <div class="newsletter-section">
            <h2 class="section-title">Feature Story</h2>
            <p>Main story content for this month.</p>
        </div>
        <div class="newsletter-section">
            <h2 class="section-title">Updates</h2>
            <p>Latest company news and announcements.</p>
        </div>
    </div>
</body>
```

### Example 12: Product catalog

```html
<style>
    .catalog {
        margin-inline: 25pt;
        padding: 20pt;
    }
    .catalog-title {
        margin-inline: 0;
        margin-bottom: 30pt;
        font-size: 24pt;
        text-align: center;
    }
    .product {
        margin-inline: 20pt;
        margin-bottom: 20pt;
        padding: 15pt;
        border: 1pt solid #e5e7eb;
        background-color: #f9fafb;
    }
    .product-name {
        margin-inline: 0;
        margin-bottom: 8pt;
        font-size: 14pt;
        font-weight: bold;
    }
    .product-description {
        margin-inline: 0;
        margin-bottom: 10pt;
        color: #6b7280;
    }
    .product-price {
        margin-inline: 0;
        font-size: 16pt;
        color: #16a34a;
        font-weight: bold;
    }
</style>
<body>
    <div class="catalog">
        <h1 class="catalog-title">Product Catalog 2025</h1>
        <div class="product">
            <div class="product-name">Premium Widget</div>
            <div class="product-description">High-quality widget for professional use.</div>
            <div class="product-price">$99.99</div>
        </div>
        <div class="product">
            <div class="product-name">Standard Widget</div>
            <div class="product-description">Reliable widget for everyday needs.</div>
            <div class="product-price">$49.99</div>
        </div>
    </div>
</body>
```

### Example 13: Receipt layout

```html
<style>
    .receipt {
        width: 300pt;
        margin-inline: auto;
        margin-top: 40pt;
        padding: 20pt;
        border: 2pt solid #000;
    }
    .receipt-header {
        margin-inline: 0;
        margin-bottom: 20pt;
        text-align: center;
        border-bottom: 1pt solid #000;
        padding-bottom: 10pt;
    }
    .receipt-items {
        margin-inline: 0;
        margin-bottom: 15pt;
    }
    .receipt-item {
        margin-inline: 10pt;
        margin-bottom: 8pt;
        display: flex;
        justify-content: space-between;
    }
    .receipt-total {
        margin-inline: 10pt;
        padding-top: 10pt;
        border-top: 2pt solid #000;
        font-weight: bold;
        font-size: 14pt;
    }
</style>
<body>
    <div class="receipt">
        <div class="receipt-header">
            <h2>Store Name</h2>
            <p>Receipt #12345</p>
        </div>
        <div class="receipt-items">
            <div class="receipt-item">
                <span>Item 1</span>
                <span>$19.99</span>
            </div>
            <div class="receipt-item">
                <span>Item 2</span>
                <span>$29.99</span>
            </div>
        </div>
        <div class="receipt-total">
            <div class="receipt-item">
                <span>Total</span>
                <span>$49.98</span>
            </div>
        </div>
    </div>
</body>
```

### Example 14: Report with executive summary

```html
<style>
    .report {
        margin-inline: 40pt;
        padding: 30pt;
    }
    .report-title {
        margin-inline: 0;
        margin-bottom: 30pt;
        font-size: 24pt;
        text-align: center;
    }
    .executive-summary {
        margin-inline: 30pt;
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #dbeafe;
        border-inline: 4pt solid #2563eb;
    }
    .executive-summary h2 {
        margin-inline: 0;
        margin-bottom: 12pt;
        font-size: 18pt;
    }
    .report-section {
        margin-inline: 15pt;
        margin-bottom: 25pt;
    }
    .section-heading {
        margin-inline: 0;
        margin-bottom: 10pt;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="report">
        <h1 class="report-title">Annual Report 2025</h1>
        <div class="executive-summary">
            <h2>Executive Summary</h2>
            <p>Key findings and highlights from the year.</p>
        </div>
        <div class="report-section">
            <h3 class="section-heading">Financial Performance</h3>
            <p>Detailed financial analysis and metrics.</p>
        </div>
    </div>
</body>
```

### Example 15: Certificate with inline margins

```html
<style>
    .certificate {
        width: 500pt;
        margin-inline: auto;
        margin-top: 50pt;
        padding: 40pt;
        border: 5pt double #1e3a8a;
        text-align: center;
    }
    .cert-title {
        margin-inline: 0;
        margin-bottom: 20pt;
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .cert-body {
        margin-inline: 40pt;
        margin-bottom: 30pt;
        font-size: 14pt;
    }
    .cert-recipient {
        margin-inline: 0;
        margin-bottom: 20pt;
        font-size: 22pt;
        font-weight: bold;
    }
    .cert-signature {
        margin-inline: 0;
        margin-top: 40pt;
        font-size: 12pt;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">Certificate of Excellence</h1>
        <div class="cert-body">
            <p>This certifies that</p>
            <p class="cert-recipient">Michael Brown</p>
            <p>has successfully completed the Advanced Professional Program</p>
        </div>
        <div class="cert-signature">
            <p>Authorized Signature</p>
            <p>January 15, 2025</p>
        </div>
    </div>
</body>
```

---

## See Also

- [margin-inline-start](/reference/cssproperties/css_prop_margin-inline-start) - Set inline start margin
- [margin-inline-end](/reference/cssproperties/css_prop_margin-inline-end) - Set inline end margin
- [margin](/reference/cssproperties/css_prop_margin) - Set all margins shorthand
- [margin-left](/reference/cssproperties/css_prop_margin-left) - Set left margin (physical property)
- [margin-right](/reference/cssproperties/css_prop_margin-right) - Set right margin (physical property)
- [padding-inline](/reference/cssproperties/css_prop_padding-inline) - Set inline padding shorthand
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
