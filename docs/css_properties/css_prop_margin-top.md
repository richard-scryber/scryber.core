---
layout: default
title: margin-top
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# margin-top : Top Margin Property

The `margin-top` property sets the top margin of an element in PDF documents. The top margin creates space above the element, separating it from preceding elements or the page edge and controlling vertical spacing in document layouts.

## Usage

```css
selector {
    margin-top: value;
}
```

The margin-top property accepts a single length value or percentage that defines the space above the element.

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
- `0` - No top margin
- `auto` - Automatic margin (rarely used for top margin)

---

## Supported Elements

The `margin-top` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables (`<table>`)
- Lists (`<ul>`, `<ol>`)
- List items (`<li>`)
- Images (`<img>`)
- All container elements

---

## Notes

- Top margins are transparent and do not have background colors
- Top margins collapse with bottom margins of preceding elements (the larger margin wins)
- Negative top margins pull elements upward, potentially overlapping with preceding content
- Percentage top margins are calculated relative to the parent element's width (not height)
- Top margins push the element down from its natural position
- First child's top margin may collapse with parent's top margin
- Top margins are useful for creating vertical rhythm and spacing in documents

---

## Data Binding

The `margin-top` property supports dynamic values through data binding, allowing you to create flexible top spacing that adapts to document structure, content hierarchy, or layout preferences.

### Example 1: Dynamic heading spacing based on level

```html
<style>
    .document h1 {
        margin-top: 0;
        font-size: 24pt;
    }
    .document h2 {
        margin-top: {{spacing.h2Top}}pt;
        font-size: 18pt;
    }
    .document h3 {
        margin-top: {{spacing.h3Top}}pt;
        font-size: 14pt;
    }
</style>
<body>
    <div class="document">
        <h1>Document Title</h1>
        <p>Introduction text.</p>
        <h2>Main Section</h2>
        <p>Section content.</p>
        <h3>Subsection</h3>
        <p>Subsection content.</p>
    </div>
</body>
```

Data context:
```json
{
    "spacing": {
        "h2Top": 30,
        "h3Top": 20
    }
}
```

### Example 2: Invoice sections with dynamic top margins

```html
<style>
    .invoice {
        padding: 40pt;
    }
    .invoice-header {
        margin-top: 0;
    }
    .invoice-details {
        margin-top: {{layout.sectionGap}}pt;
    }
    .invoice-items {
        margin-top: {{layout.sectionGap}}pt;
    }
    .invoice-total {
        margin-top: {{layout.totalGap}}pt;
        padding-top: 15pt;
        border-top: 2pt solid #000;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1>INVOICE</h1>
        </div>
        <div class="invoice-details">
            <p><strong>Bill To:</strong> {{customer.name}}</p>
        </div>
        <div class="invoice-items">
            <table>
                <tr><td>Service</td><td>Amount</td></tr>
            </table>
        </div>
        <div class="invoice-total">
            <p>Total: {{invoice.total}}</p>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "layout": {
        "sectionGap": 30,
        "totalGap": 25
    },
    "customer": {
        "name": "Acme Corporation"
    },
    "invoice": {
        "total": "$1,500.00"
    }
}
```

### Example 3: Card spacing based on importance

```html
<style>
    .card-container {
        padding: 20pt;
    }
    .card {
        margin-top: {{item.priority === 'high' ? '20pt' : '15pt'}};
        padding: 15pt;
        background-color: {{item.priority === 'high' ? '#fef3c7' : 'white'}};
        border: 1pt solid #e5e7eb;
    }
    .card:first-child {
        margin-top: 0;
    }
</style>
<body>
    <div class="card-container">
        <div class="card" style="margin-top: {{items[0].priority === 'high' ? '20pt' : '15pt'}};">
            <h3>Card Title</h3>
            <p>Content with priority-based spacing.</p>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "items": [
        {
            "priority": "high"
        }
    ]
}
```

---

## Examples

### Example 1: Basic top margin

```html
<style>
    .section {
        margin-top: 25pt;
        padding: 15pt;
        background-color: #f3f4f6;
    }
</style>
<body>
    <div class="section">
        <h2>First Section</h2>
        <p>This section has a 25pt top margin.</p>
    </div>
    <div class="section">
        <h2>Second Section</h2>
        <p>This section also has a 25pt top margin.</p>
    </div>
</body>
```

### Example 2: Heading spacing with top margin

```html
<style>
    .document h1 {
        margin-top: 0;
        font-size: 24pt;
    }
    .document h2 {
        margin-top: 30pt;
        font-size: 18pt;
    }
    .document h3 {
        margin-top: 20pt;
        font-size: 14pt;
    }
    .document p {
        margin-top: 0;
    }
</style>
<body>
    <div class="document">
        <h1>Document Title</h1>
        <p>Introduction paragraph.</p>
        <h2>First Section</h2>
        <p>Section content.</p>
        <h3>Subsection</h3>
        <p>Subsection content.</p>
    </div>
</body>
```

### Example 3: Invoice sections with top margin

```html
<style>
    .invoice {
        padding: 40pt;
    }
    .invoice-header {
        margin-top: 0;
        padding: 20pt;
        background-color: #1e3a8a;
        color: white;
    }
    .invoice-details {
        margin-top: 30pt;
    }
    .invoice-items {
        margin-top: 25pt;
    }
    .invoice-total {
        margin-top: 20pt;
        padding-top: 15pt;
        border-top: 2pt solid #000;
        font-weight: bold;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1>INVOICE</h1>
        </div>
        <div class="invoice-details">
            <p><strong>Bill To:</strong> Acme Corporation</p>
            <p><strong>Invoice #:</strong> INV-2025-001</p>
        </div>
        <div class="invoice-items">
            <table>
                <tr><td>Service</td><td>Amount</td></tr>
                <tr><td>Consulting</td><td>$1,500.00</td></tr>
            </table>
        </div>
        <div class="invoice-total">
            <p>Total: $1,500.00</p>
        </div>
    </div>
</body>
```

### Example 4: Card layout with top spacing

```html
<style>
    .card-container {
        padding: 20pt;
    }
    .card {
        margin-top: 0;
        padding: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .card + .card {
        margin-top: 15pt;
    }
    .card h3 {
        margin-top: 0;
    }
</style>
<body>
    <div class="card-container">
        <div class="card">
            <h3>Card One</h3>
            <p>First card content.</p>
        </div>
        <div class="card">
            <h3>Card Two</h3>
            <p>Second card content with top margin.</p>
        </div>
        <div class="card">
            <h3>Card Three</h3>
            <p>Third card content with top margin.</p>
        </div>
    </div>
</body>
```

### Example 5: Alert boxes with top margin

```html
<style>
    .alerts {
        padding: 25pt;
    }
    .alert {
        margin-top: 12pt;
        padding: 12pt;
        border-radius: 4pt;
    }
    .alert:first-child {
        margin-top: 0;
    }
    .alert-info {
        background-color: #dbeafe;
        border: 1pt solid #3b82f6;
    }
    .alert-success {
        background-color: #dcfce7;
        border: 1pt solid #16a34a;
    }
</style>
<body>
    <div class="alerts">
        <div class="alert alert-info">
            <strong>Info:</strong> System maintenance scheduled.
        </div>
        <div class="alert alert-success">
            <strong>Success:</strong> Changes saved successfully.
        </div>
    </div>
</body>
```

### Example 6: Form groups with top margin

```html
<style>
    .form {
        padding: 30pt;
    }
    .form-title {
        margin-top: 0;
        font-size: 20pt;
        font-weight: bold;
    }
    .form-group {
        margin-top: 15pt;
    }
    .form-group:first-of-type {
        margin-top: 20pt;
    }
    .form-label {
        margin-top: 0;
        font-weight: bold;
        display: block;
    }
    .form-button {
        margin-top: 25pt;
        padding: 10pt 20pt;
        background-color: #2563eb;
        color: white;
    }
</style>
<body>
    <div class="form">
        <h2 class="form-title">Registration Form</h2>
        <div class="form-group">
            <label class="form-label">Full Name</label>
            <input type="text" />
        </div>
        <div class="form-group">
            <label class="form-label">Email</label>
            <input type="email" />
        </div>
        <button class="form-button">Submit</button>
    </div>
</body>
```

### Example 7: Table with caption spacing

```html
<style>
    .report-container {
        padding: 30pt;
    }
    .report-title {
        margin-top: 0;
        font-size: 22pt;
    }
    .report-table {
        margin-top: 20pt;
        width: 100%;
        border-collapse: collapse;
    }
    .report-table th {
        padding: 10pt;
        background-color: #1f2937;
        color: white;
    }
    .report-table td {
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .table-notes {
        margin-top: 15pt;
        font-size: 9pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="report-container">
        <h1 class="report-title">Quarterly Report</h1>
        <table class="report-table">
            <thead>
                <tr><th>Quarter</th><th>Revenue</th></tr>
            </thead>
            <tbody>
                <tr><td>Q1</td><td>$250,000</td></tr>
                <tr><td>Q2</td><td>$280,000</td></tr>
            </tbody>
        </table>
        <p class="table-notes">* All figures in USD</p>
    </div>
</body>
```

### Example 8: Article layout with paragraph spacing

```html
<style>
    .article {
        padding: 40pt 50pt;
    }
    .article-title {
        margin-top: 0;
        font-size: 24pt;
        font-weight: bold;
    }
    .article-meta {
        margin-top: 8pt;
        font-size: 10pt;
        color: #6b7280;
    }
    .article-content {
        margin-top: 25pt;
    }
    .article-content p {
        margin-top: 0;
        margin-bottom: 12pt;
        line-height: 1.6;
    }
    .article-content h2 {
        margin-top: 30pt;
        font-size: 18pt;
    }
</style>
<body>
    <div class="article">
        <h1 class="article-title">Article Headline</h1>
        <div class="article-meta">Published on January 15, 2025</div>
        <div class="article-content">
            <p>First paragraph of the article.</p>
            <h2>Section Heading</h2>
            <p>Content under the section heading.</p>
        </div>
    </div>
</body>
```

### Example 9: Sidebar layout with top margin

```html
<style>
    .page {
        padding: 30pt;
    }
    .sidebar {
        width: 150pt;
        float: left;
        margin-right: 20pt;
    }
    .sidebar-title {
        margin-top: 0;
        font-size: 14pt;
        font-weight: bold;
    }
    .sidebar-section {
        margin-top: 20pt;
        padding: 10pt;
        background-color: #f3f4f6;
    }
    .main-content {
        margin-left: 190pt;
    }
    .main-content h1 {
        margin-top: 0;
    }
</style>
<body>
    <div class="page">
        <div class="sidebar">
            <h3 class="sidebar-title">Quick Links</h3>
            <div class="sidebar-section">
                <p>Link 1</p>
                <p>Link 2</p>
            </div>
            <div class="sidebar-section">
                <p>Link 3</p>
                <p>Link 4</p>
            </div>
        </div>
        <div class="main-content">
            <h1>Main Content</h1>
            <p>Content area with proper spacing.</p>
        </div>
    </div>
</body>
```

### Example 10: Quote block with top margin

```html
<style>
    .content {
        padding: 40pt;
    }
    .content p {
        margin-top: 0;
        margin-bottom: 12pt;
    }
    .quote-block {
        margin-top: 25pt;
        margin-bottom: 25pt;
        padding: 15pt 15pt 15pt 20pt;
        background-color: #f5f5f5;
        border-left: 4pt solid #6366f1;
        font-style: italic;
    }
    .quote-text {
        margin-top: 0;
    }
    .quote-author {
        margin-top: 10pt;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="content">
        <p>Regular paragraph text before the quote.</p>
        <div class="quote-block">
            <p class="quote-text">"Innovation distinguishes between a leader and a follower."</p>
            <p class="quote-author">— Steve Jobs</p>
        </div>
        <p>Text continues after the quote block.</p>
    </div>
</body>
```

### Example 11: Product listing with top margin

```html
<style>
    .product-list {
        padding: 25pt;
    }
    .product-list-title {
        margin-top: 0;
        font-size: 20pt;
        text-align: center;
    }
    .product-item {
        margin-top: 20pt;
        padding: 15pt;
        border: 1pt solid #e5e7eb;
    }
    .product-item:first-of-type {
        margin-top: 30pt;
    }
    .product-name {
        margin-top: 0;
        font-size: 14pt;
        font-weight: bold;
    }
    .product-price {
        margin-top: 8pt;
        font-size: 16pt;
        color: #16a34a;
    }
</style>
<body>
    <div class="product-list">
        <h1 class="product-list-title">Featured Products</h1>
        <div class="product-item">
            <h3 class="product-name">Product A</h3>
            <p class="product-price">$99.99</p>
        </div>
        <div class="product-item">
            <h3 class="product-name">Product B</h3>
            <p class="product-price">$149.99</p>
        </div>
    </div>
</body>
```

### Example 12: Newsletter sections with top margin

```html
<style>
    .newsletter {
        padding: 30pt;
    }
    .newsletter-header {
        margin-top: 0;
        padding: 15pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
    .newsletter-section {
        margin-top: 25pt;
    }
    .newsletter-section h2 {
        margin-top: 0;
        font-size: 16pt;
        color: #1f2937;
    }
    .newsletter-section p {
        margin-top: 8pt;
        line-height: 1.5;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>Monthly Newsletter</h1>
        </div>
        <div class="newsletter-section">
            <h2>Feature Story</h2>
            <p>Main story content goes here.</p>
        </div>
        <div class="newsletter-section">
            <h2>Updates</h2>
            <p>Latest updates and announcements.</p>
        </div>
    </div>
</body>
```

### Example 13: Receipt layout with top margin

```html
<style>
    .receipt {
        margin: 40pt auto;
        width: 300pt;
        padding: 20pt;
        border: 2pt solid #000;
    }
    .receipt-header {
        margin-top: 0;
        text-align: center;
    }
    .receipt-date {
        margin-top: 5pt;
        font-size: 9pt;
        text-align: center;
    }
    .receipt-items {
        margin-top: 20pt;
    }
    .receipt-item {
        margin-top: 8pt;
    }
    .receipt-item:first-child {
        margin-top: 0;
    }
    .receipt-total {
        margin-top: 15pt;
        padding-top: 10pt;
        border-top: 2pt solid #000;
        font-weight: bold;
    }
</style>
<body>
    <div class="receipt">
        <h2 class="receipt-header">Store Name</h2>
        <p class="receipt-date">January 15, 2025</p>
        <div class="receipt-items">
            <div class="receipt-item">Item 1 - $19.99</div>
            <div class="receipt-item">Item 2 - $29.99</div>
        </div>
        <div class="receipt-total">Total: $49.98</div>
    </div>
</body>
```

### Example 14: Report with executive summary

```html
<style>
    .report {
        padding: 40pt;
    }
    .report-title {
        margin-top: 0;
        font-size: 24pt;
        text-align: center;
    }
    .executive-summary {
        margin-top: 30pt;
        padding: 20pt;
        background-color: #dbeafe;
        border-left: 4pt solid #2563eb;
    }
    .executive-summary h2 {
        margin-top: 0;
        font-size: 18pt;
    }
    .report-section {
        margin-top: 35pt;
    }
    .report-section h2 {
        margin-top: 0;
        font-size: 16pt;
        border-bottom: 1pt solid #d1d5db;
        padding-bottom: 5pt;
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
            <h2>Financial Performance</h2>
            <p>Detailed financial analysis.</p>
        </div>
    </div>
</body>
```

### Example 15: Certificate with top spacing

```html
<style>
    .certificate {
        margin: 50pt auto;
        width: 500pt;
        padding: 40pt;
        border: 5pt double #1e3a8a;
        text-align: center;
    }
    .certificate-title {
        margin-top: 0;
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .certificate-subtitle {
        margin-top: 10pt;
        font-size: 14pt;
        color: #6b7280;
    }
    .certificate-recipient {
        margin-top: 40pt;
        font-size: 20pt;
        font-style: italic;
    }
    .certificate-description {
        margin-top: 25pt;
        font-size: 12pt;
        line-height: 1.6;
    }
    .certificate-signature {
        margin-top: 50pt;
        font-size: 11pt;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="certificate-title">Certificate of Achievement</h1>
        <p class="certificate-subtitle">This certifies that</p>
        <p class="certificate-recipient">John Smith</p>
        <p class="certificate-description">
            Has successfully completed the Advanced Training Program
        </p>
        <p class="certificate-signature">
            Authorized Signature<br/>
            January 15, 2025
        </p>
    </div>
</body>
```

---

## See Also

- [margin](/reference/cssproperties/css_prop_margin) - Set all margins shorthand
- [margin-bottom](/reference/cssproperties/css_prop_margin-bottom) - Set bottom margin
- [margin-left](/reference/cssproperties/css_prop_margin-left) - Set left margin
- [margin-right](/reference/cssproperties/css_prop_margin-right) - Set right margin
- [padding-top](/reference/cssproperties/css_prop_padding-top) - Set top padding
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
