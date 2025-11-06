---
layout: default
title: margin-bottom
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# margin-bottom : Bottom Margin Property

The `margin-bottom` property sets the bottom margin of an element in PDF documents. The bottom margin creates space below the element, separating it from following elements and controlling vertical spacing and document flow.

## Usage

```css
selector {
    margin-bottom: value;
}
```

The margin-bottom property accepts a single length value or percentage that defines the space below the element.

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
- `0` - No bottom margin
- `auto` - Automatic margin (rarely used for bottom margin)

---

## Supported Elements

The `margin-bottom` property can be applied to:
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

- Bottom margins are transparent and do not have background colors
- Bottom margins collapse with top margins of following elements (the larger margin wins)
- Negative bottom margins pull following elements upward, potentially causing overlaps
- Percentage bottom margins are calculated relative to the parent element's width (not height)
- Bottom margins are commonly used to create vertical spacing between elements
- Last child's bottom margin may collapse with parent's bottom margin
- Bottom margins are essential for establishing vertical rhythm in document layouts

---

## Data Binding

The `margin-bottom` property supports dynamic values through data binding, allowing you to create flexible bottom spacing that establishes consistent vertical rhythm based on content type, document density, or layout preferences.

### Example 1: Dynamic paragraph spacing for different document types

```html
<style>
    .article {
        padding: 40pt;
    }
    .article p {
        margin-bottom: {{typography.paragraphSpacing}}pt;
        line-height: {{typography.lineHeight}};
    }
    .article h2 {
        margin-bottom: {{typography.headingSpacing}}pt;
        font-size: 18pt;
    }
</style>
<body>
    <div class="article">
        <h2>Article Title</h2>
        <p>First paragraph with dynamic spacing.</p>
        <p>Second paragraph adapts to typography settings.</p>
    </div>
</body>
```

Data context:
```json
{
    "typography": {
        "paragraphSpacing": 12,
        "headingSpacing": 15,
        "lineHeight": 1.6
    }
}
```

### Example 2: Card spacing based on layout density

```html
<style>
    .card-stack {
        padding: 25pt;
    }
    .card {
        margin-bottom: {{layout.density === 'compact' ? '10pt' : '15pt'}};
        padding: {{layout.density === 'compact' ? '12pt' : '15pt'}};
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .card:last-child {
        margin-bottom: 0;
    }
</style>
<body>
    <div class="card-stack">
        <div class="card">
            <h3>Card One</h3>
            <p>Content with density-based spacing.</p>
        </div>
        <div class="card">
            <h3>Card Two</h3>
            <p>Automatically adjusts to layout preferences.</p>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "layout": {
        "density": "normal"
    }
}
```

### Example 3: Form field spacing based on validation

```html
<style>
    .form {
        padding: 30pt;
    }
    .form-group {
        margin-bottom: {{field.hasError ? '20pt' : '15pt'}};
    }
    .form-group.error {
        margin-bottom: 25pt;
    }
    .form-label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
    }
    .error-message {
        margin-top: 5pt;
        margin-bottom: 0;
        color: #dc2626;
        font-size: 10pt;
    }
</style>
<body>
    <div class="form">
        <div class="form-group {{field.hasError ? 'error' : ''}}">
            <label class="form-label">{{field.label}}</label>
            <input type="text" />
            {{#if field.hasError}}
            <p class="error-message">{{field.errorMessage}}</p>
            {{/if}}
        </div>
    </div>
</body>
```

Data context:
```json
{
    "field": {
        "label": "Email Address",
        "hasError": true,
        "errorMessage": "Please enter a valid email address"
    }
}
```

---

## Examples

### Example 1: Basic bottom margin

```html
<style>
    .section {
        margin-bottom: 20pt;
        padding: 15pt;
        background-color: #f3f4f6;
    }
</style>
<body>
    <div class="section">
        <h2>First Section</h2>
        <p>This section has a 20pt bottom margin.</p>
    </div>
    <div class="section">
        <h2>Second Section</h2>
        <p>Separated from the first by margin spacing.</p>
    </div>
</body>
```

### Example 2: Paragraph spacing with bottom margin

```html
<style>
    .article {
        padding: 40pt;
    }
    .article p {
        margin-bottom: 12pt;
        line-height: 1.6;
    }
    .article p:last-child {
        margin-bottom: 0;
    }
    .article h2 {
        margin-bottom: 15pt;
        font-size: 18pt;
    }
</style>
<body>
    <div class="article">
        <h2>Article Title</h2>
        <p>First paragraph with bottom margin for proper spacing.</p>
        <p>Second paragraph also has bottom margin.</p>
        <p>Last paragraph has no bottom margin.</p>
    </div>
</body>
```

### Example 3: Heading hierarchy with bottom margins

```html
<style>
    .document {
        padding: 40pt 50pt;
    }
    .document h1 {
        margin-bottom: 25pt;
        font-size: 24pt;
    }
    .document h2 {
        margin-bottom: 20pt;
        font-size: 18pt;
    }
    .document h3 {
        margin-bottom: 15pt;
        font-size: 14pt;
    }
    .document p {
        margin-bottom: 12pt;
    }
</style>
<body>
    <div class="document">
        <h1>Main Title</h1>
        <p>Introduction text.</p>
        <h2>Section Heading</h2>
        <p>Section content.</p>
        <h3>Subsection</h3>
        <p>Subsection content.</p>
    </div>
</body>
```

### Example 4: Card stack with bottom margins

```html
<style>
    .card-stack {
        padding: 25pt;
    }
    .card {
        margin-bottom: 15pt;
        padding: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        border-radius: 4pt;
    }
    .card:last-child {
        margin-bottom: 0;
    }
    .card h3 {
        margin-bottom: 10pt;
        font-size: 14pt;
        color: #1f2937;
    }
    .card p {
        margin-bottom: 0;
        color: #6b7280;
    }
</style>
<body>
    <div class="card-stack">
        <div class="card">
            <h3>Card One</h3>
            <p>First card content.</p>
        </div>
        <div class="card">
            <h3>Card Two</h3>
            <p>Second card content.</p>
        </div>
        <div class="card">
            <h3>Card Three</h3>
            <p>Third card content.</p>
        </div>
    </div>
</body>
```

### Example 5: Invoice layout with bottom margins

```html
<style>
    .invoice {
        padding: 40pt;
    }
    .invoice-header {
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e3a8a;
        color: white;
    }
    .invoice-details {
        margin-bottom: 25pt;
    }
    .invoice-details p {
        margin-bottom: 5pt;
    }
    .invoice-table {
        margin-bottom: 20pt;
        width: 100%;
        border-collapse: collapse;
    }
    .invoice-notes {
        margin-bottom: 0;
        padding: 15pt;
        background-color: #f9fafb;
        font-size: 10pt;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1>INVOICE</h1>
            <p>INV-2025-001</p>
        </div>
        <div class="invoice-details">
            <p><strong>Bill To:</strong> Acme Corporation</p>
            <p><strong>Date:</strong> January 15, 2025</p>
        </div>
        <table class="invoice-table">
            <tr>
                <th>Description</th>
                <th>Amount</th>
            </tr>
            <tr>
                <td>Services</td>
                <td>$1,500.00</td>
            </tr>
        </table>
        <div class="invoice-notes">
            Payment terms: Net 30 days
        </div>
    </div>
</body>
```

### Example 6: Form with field spacing

```html
<style>
    .form {
        padding: 30pt;
    }
    .form-title {
        margin-bottom: 25pt;
        font-size: 20pt;
        font-weight: bold;
    }
    .form-group {
        margin-bottom: 15pt;
    }
    .form-group label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
    }
    .form-group input,
    .form-group textarea {
        width: 100%;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .form-button {
        margin-bottom: 0;
        padding: 10pt 20pt;
        background-color: #2563eb;
        color: white;
    }
</style>
<body>
    <div class="form">
        <h2 class="form-title">Contact Form</h2>
        <div class="form-group">
            <label>Name</label>
            <input type="text" />
        </div>
        <div class="form-group">
            <label>Email</label>
            <input type="email" />
        </div>
        <div class="form-group">
            <label>Message</label>
            <textarea rows="4"></textarea>
        </div>
        <button class="form-button">Send Message</button>
    </div>
</body>
```

### Example 7: List with item spacing

```html
<style>
    .list-container {
        padding: 30pt;
    }
    .list-title {
        margin-bottom: 20pt;
        font-size: 18pt;
        font-weight: bold;
    }
    .styled-list {
        margin-bottom: 0;
        padding-left: 20pt;
    }
    .styled-list li {
        margin-bottom: 10pt;
        line-height: 1.5;
    }
    .styled-list li:last-child {
        margin-bottom: 0;
    }
</style>
<body>
    <div class="list-container">
        <h2 class="list-title">Key Features</h2>
        <ul class="styled-list">
            <li>High performance and reliability</li>
            <li>Easy to use interface</li>
            <li>Comprehensive documentation</li>
            <li>24/7 customer support</li>
        </ul>
    </div>
</body>
```

### Example 8: Alert stack with bottom margins

```html
<style>
    .alerts {
        padding: 25pt;
    }
    .alert {
        margin-bottom: 12pt;
        padding: 12pt;
        border-radius: 4pt;
    }
    .alert:last-child {
        margin-bottom: 0;
    }
    .alert-info {
        background-color: #dbeafe;
        border: 1pt solid #3b82f6;
    }
    .alert-success {
        background-color: #dcfce7;
        border: 1pt solid #16a34a;
    }
    .alert-warning {
        background-color: #fef3c7;
        border: 1pt solid #f59e0b;
    }
    .alert p {
        margin-bottom: 0;
    }
</style>
<body>
    <div class="alerts">
        <div class="alert alert-info">
            <p><strong>Info:</strong> System update scheduled for tonight.</p>
        </div>
        <div class="alert alert-success">
            <p><strong>Success:</strong> Your profile has been updated.</p>
        </div>
        <div class="alert alert-warning">
            <p><strong>Warning:</strong> Your trial period ends in 7 days.</p>
        </div>
    </div>
</body>
```

### Example 9: Table with caption spacing

```html
<style>
    .report {
        padding: 30pt;
    }
    .report-title {
        margin-bottom: 10pt;
        font-size: 22pt;
        font-weight: bold;
    }
    .report-subtitle {
        margin-bottom: 25pt;
        font-size: 12pt;
        color: #6b7280;
    }
    .data-table {
        margin-bottom: 15pt;
        width: 100%;
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
    .table-footer {
        margin-bottom: 0;
        font-size: 9pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="report">
        <h1 class="report-title">Sales Report</h1>
        <p class="report-subtitle">Q4 2024 Performance</p>
        <table class="data-table">
            <thead>
                <tr>
                    <th>Month</th>
                    <th>Revenue</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>October</td>
                    <td>$125,000</td>
                </tr>
                <tr>
                    <td>November</td>
                    <td>$138,000</td>
                </tr>
            </tbody>
        </table>
        <p class="table-footer">* All figures in USD</p>
    </div>
</body>
```

### Example 10: Quote block with bottom margin

```html
<style>
    .content {
        padding: 40pt;
    }
    .content p {
        margin-bottom: 12pt;
        line-height: 1.6;
    }
    .quote-block {
        margin-bottom: 25pt;
        padding: 15pt 15pt 15pt 20pt;
        background-color: #f5f5f5;
        border-left: 4pt solid #6366f1;
        font-style: italic;
    }
    .quote-block p {
        margin-bottom: 0;
    }
    .quote-block .quote-author {
        margin-top: 10pt;
        margin-bottom: 0;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="content">
        <p>Introduction paragraph before the quote.</p>
        <div class="quote-block">
            <p>"The only way to do great work is to love what you do."</p>
            <p class="quote-author">— Steve Jobs</p>
        </div>
        <p>Content continues after the quote with proper spacing.</p>
    </div>
</body>
```

### Example 11: Product catalog with bottom spacing

```html
<style>
    .catalog {
        padding: 25pt;
    }
    .catalog-header {
        margin-bottom: 30pt;
        text-align: center;
    }
    .catalog-header h1 {
        margin-bottom: 8pt;
        font-size: 24pt;
    }
    .catalog-header p {
        margin-bottom: 0;
        color: #6b7280;
    }
    .product {
        margin-bottom: 20pt;
        padding: 15pt;
        border: 1pt solid #e5e7eb;
        background-color: #f9fafb;
    }
    .product:last-child {
        margin-bottom: 0;
    }
    .product-name {
        margin-bottom: 8pt;
        font-size: 14pt;
        font-weight: bold;
    }
    .product-description {
        margin-bottom: 10pt;
        color: #6b7280;
    }
    .product-price {
        margin-bottom: 0;
        font-size: 16pt;
        color: #16a34a;
        font-weight: bold;
    }
</style>
<body>
    <div class="catalog">
        <div class="catalog-header">
            <h1>2025 Product Catalog</h1>
            <p>Premium selection of products</p>
        </div>
        <div class="product">
            <div class="product-name">Premium Widget</div>
            <div class="product-description">Top-quality widget for professional use.</div>
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

### Example 12: Newsletter sections

```html
<style>
    .newsletter {
        padding: 30pt;
    }
    .newsletter-header {
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
    .newsletter-header h1 {
        margin-bottom: 5pt;
    }
    .newsletter-header p {
        margin-bottom: 0;
    }
    .newsletter-section {
        margin-bottom: 25pt;
    }
    .newsletter-section:last-child {
        margin-bottom: 0;
    }
    .newsletter-section h2 {
        margin-bottom: 12pt;
        font-size: 16pt;
        color: #1f2937;
    }
    .newsletter-section p {
        margin-bottom: 10pt;
        line-height: 1.5;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>Monthly Newsletter</h1>
            <p>January 2025 Edition</p>
        </div>
        <div class="newsletter-section">
            <h2>Feature Story</h2>
            <p>This month's top story and highlights.</p>
        </div>
        <div class="newsletter-section">
            <h2>Company Updates</h2>
            <p>Latest news and announcements from the team.</p>
        </div>
    </div>
</body>
```

### Example 13: Receipt with item spacing

```html
<style>
    .receipt {
        margin: 40pt auto;
        width: 300pt;
        padding: 20pt;
        border: 2pt solid #000;
    }
    .receipt-header {
        margin-bottom: 20pt;
        padding-bottom: 15pt;
        border-bottom: 1pt solid #000;
        text-align: center;
    }
    .receipt-header h2 {
        margin-bottom: 5pt;
    }
    .receipt-header p {
        margin-bottom: 0;
        font-size: 9pt;
    }
    .receipt-item {
        margin-bottom: 8pt;
        display: flex;
        justify-content: space-between;
    }
    .receipt-items {
        margin-bottom: 15pt;
    }
    .receipt-total {
        margin-bottom: 0;
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
            <p>123 Main Street</p>
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

### Example 14: Report with section spacing

```html
<style>
    .report {
        padding: 40pt;
    }
    .report-title {
        margin-bottom: 10pt;
        font-size: 24pt;
        text-align: center;
    }
    .report-date {
        margin-bottom: 35pt;
        text-align: center;
        color: #6b7280;
    }
    .executive-summary {
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #dbeafe;
        border-left: 4pt solid #2563eb;
    }
    .executive-summary h2 {
        margin-bottom: 12pt;
    }
    .executive-summary p {
        margin-bottom: 0;
    }
    .report-section {
        margin-bottom: 30pt;
    }
    .report-section:last-child {
        margin-bottom: 0;
    }
    .report-section h2 {
        margin-bottom: 15pt;
        font-size: 18pt;
        border-bottom: 1pt solid #d1d5db;
        padding-bottom: 8pt;
    }
</style>
<body>
    <div class="report">
        <h1 class="report-title">Annual Report 2025</h1>
        <p class="report-date">Published: January 15, 2025</p>
        <div class="executive-summary">
            <h2>Executive Summary</h2>
            <p>Overview of the year's achievements and key metrics.</p>
        </div>
        <div class="report-section">
            <h2>Financial Performance</h2>
            <p>Detailed analysis of financial results.</p>
        </div>
        <div class="report-section">
            <h2>Future Outlook</h2>
            <p>Strategic plans for the coming year.</p>
        </div>
    </div>
</body>
```

### Example 15: Badge collection with bottom margins

```html
<style>
    .profile {
        padding: 30pt;
    }
    .profile-header {
        margin-bottom: 25pt;
        padding-bottom: 15pt;
        border-bottom: 2pt solid #e5e7eb;
    }
    .profile-name {
        margin-bottom: 5pt;
        font-size: 20pt;
        font-weight: bold;
    }
    .profile-title {
        margin-bottom: 0;
        color: #6b7280;
    }
    .badges-section {
        margin-bottom: 20pt;
    }
    .badges-title {
        margin-bottom: 15pt;
        font-size: 14pt;
        font-weight: bold;
    }
    .badge {
        display: inline-block;
        margin-right: 8pt;
        margin-bottom: 8pt;
        padding: 6pt 12pt;
        background-color: #e0e7ff;
        color: #3730a3;
        border-radius: 4pt;
        font-size: 10pt;
    }
</style>
<body>
    <div class="profile">
        <div class="profile-header">
            <h1 class="profile-name">John Smith</h1>
            <p class="profile-title">Senior Developer</p>
        </div>
        <div class="badges-section">
            <h3 class="badges-title">Skills & Certifications</h3>
            <span class="badge">JavaScript</span>
            <span class="badge">Python</span>
            <span class="badge">AWS Certified</span>
            <span class="badge">Scrum Master</span>
        </div>
    </div>
</body>
```

---

## See Also

- [margin](/reference/cssproperties/css_prop_margin) - Set all margins shorthand
- [margin-top](/reference/cssproperties/css_prop_margin-top) - Set top margin
- [margin-left](/reference/cssproperties/css_prop_margin-left) - Set left margin
- [margin-right](/reference/cssproperties/css_prop_margin-right) - Set right margin
- [padding-bottom](/reference/cssproperties/css_prop_padding-bottom) - Set bottom padding
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
