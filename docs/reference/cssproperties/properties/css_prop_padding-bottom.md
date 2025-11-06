---
layout: default
title: padding-bottom
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# padding-bottom : Bottom Padding Property

The `padding-bottom` property sets the bottom padding of an element in PDF documents. Bottom padding creates space inside the element below the content, between the bottom border and the content, inheriting the element's background color.

## Usage

```css
selector {
    padding-bottom: value;
}
```

The padding-bottom property accepts a single length value or percentage that defines the space below the content.

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
- `0` - No bottom padding

---

## Supported Elements

The `padding-bottom` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Lists (`<ul>`, `<ol>`)
- List items (`<li>`)
- All container elements

---

## Notes

- Bottom padding inherits the element's background color
- Bottom padding increases the overall height of the element
- Percentage bottom padding is calculated relative to parent element's width (not height)
- Bottom padding cannot be negative
- Bottom padding is inside the border, while margin is outside
- Background colors and images extend into the padding area
- Bottom padding creates vertical space that's part of the element's clickable area
- Essential for creating visual breathing room at the bottom of elements

---

## Data Binding

The `padding-bottom` property supports dynamic values through data binding, allowing you to create flexible bottom internal spacing that adapts to content type, footer styles, or layout density.

### Example 1: Section padding based on content type

```html
<style>
    .section {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .section-header {
        padding-top: 20pt;
        padding-bottom: {{section.hasSubtitle ? '20pt' : '15pt'}};
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: #dbeafe;
        border-bottom: 2pt solid #3b82f6;
    }
</style>
<body>
    <div class="section">
        <div class="section-header">
            <h2>{{section.title}}</h2>
            {{#if section.hasSubtitle}}
            <p>{{section.subtitle}}</p>
            {{/if}}
        </div>
    </div>
</body>
```

Data context:
```json
{
    "section": {
        "hasSubtitle": true,
        "title": "Section Title",
        "subtitle": "Additional context"
    }
}
```

### Example 2: Card body with variable bottom padding

```html
<style>
    .card {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .card-body {
        padding-top: 15pt;
        padding-bottom: {{card.hasFooter ? '10pt' : '15pt'}};
        padding-left: 20pt;
        padding-right: 20pt;
    }
    .card-footer {
        padding: 12pt 20pt;
        background-color: #f3f4f6;
        border-top: 1pt solid #e5e7eb;
    }
</style>
<body>
    <div class="card">
        <div class="card-body">
            <p>{{card.content}}</p>
        </div>
        {{#if card.hasFooter}}
        <div class="card-footer">
            <p>{{card.footerText}}</p>
        </div>
        {{/if}}
    </div>
</body>
```

Data context:
```json
{
    "card": {
        "hasFooter": true,
        "content": "Card content goes here",
        "footerText": "Card footer information"
    }
}
```

### Example 3: Newsletter sections with adaptive padding

```html
<style>
    .newsletter-section {
        padding-top: 25pt;
        padding-bottom: {{section.lastSection ? '25pt' : '20pt'}};
        padding-left: 25pt;
        padding-right: 25pt;
        background-color: white;
        margin-bottom: {{section.lastSection ? '0' : '20pt'}};
    }
</style>
<body>
    <div class="newsletter-section">
        <h2>{{section.title}}</h2>
        <p>{{section.content}}</p>
    </div>
</body>
```

Data context:
```json
{
    "section": {
        "lastSection": false,
        "title": "Feature Story",
        "content": "This month's highlight"
    }
}
```

---

## Examples

### Example 1: Basic bottom padding

```html
<style>
    .box {
        padding-top: 15pt;
        padding-bottom: 25pt;
        padding-left: 15pt;
        padding-right: 15pt;
        background-color: #dbeafe;
        border: 2pt solid #3b82f6;
    }
</style>
<body>
    <div class="box">
        <p style="margin: 0;">This box has extra bottom padding for spacing.</p>
    </div>
</body>
```

### Example 2: Card with bottom padding

```html
<style>
    .card {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .card-header {
        padding: 15pt 20pt;
        background-color: #f3f4f6;
        border-bottom: 1pt solid #e5e7eb;
    }
    .card-body {
        padding-top: 15pt;
        padding-bottom: 25pt;
        padding-left: 20pt;
        padding-right: 20pt;
    }
</style>
<body>
    <div class="card">
        <div class="card-header">
            <h3 style="margin: 0;">Card Title</h3>
        </div>
        <div class="card-body">
            <p style="margin: 0;">Card content with extra bottom padding.</p>
        </div>
    </div>
</body>
```

### Example 3: Section footer with bottom padding

```html
<style>
    .section {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .section-content {
        padding: 15pt 20pt;
    }
    .section-footer {
        padding-top: 12pt;
        padding-bottom: 20pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: #f3f4f6;
        border-top: 1pt solid #e5e7eb;
    }
</style>
<body>
    <div class="section">
        <div class="section-content">
            <p>Main content area.</p>
        </div>
        <div class="section-footer">
            <p style="margin: 0;">Footer with extra bottom padding.</p>
        </div>
    </div>
</body>
```

### Example 4: Table cells with bottom padding

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th {
        padding-top: 12pt;
        padding-bottom: 15pt;
        padding-left: 12pt;
        padding-right: 12pt;
        background-color: #1f2937;
        color: white;
        text-align: left;
    }
    .data-table td {
        padding-top: 10pt;
        padding-bottom: 12pt;
        padding-left: 12pt;
        padding-right: 12pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th>Product</th>
                <th>Status</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget</td>
                <td>In Stock</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 5: Alert with bottom padding

```html
<style>
    .alert {
        padding-top: 12pt;
        padding-bottom: 18pt;
        padding-left: 15pt;
        padding-right: 15pt;
        background-color: #fef3c7;
        border: 1pt solid #f59e0b;
        border-bottom: 3pt solid #f59e0b;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="alert">
        <p style="margin: 0 0 8pt 0;"><strong>Warning:</strong> Action required.</p>
        <p style="margin: 0;">Extra bottom padding for visual emphasis.</p>
    </div>
</body>
```

### Example 6: Quote block with bottom padding

```html
<style>
    .quote-block {
        padding-top: 20pt;
        padding-bottom: 30pt;
        padding-left: 30pt;
        padding-right: 20pt;
        background-color: #f5f5f5;
        border-left: 5pt solid #6366f1;
        font-style: italic;
        margin: 20pt 0;
    }
    .quote-text {
        margin: 0 0 15pt 0;
        font-size: 14pt;
    }
    .quote-author {
        margin: 0;
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="quote-block">
        <p class="quote-text">"The journey of a thousand miles begins with one step."</p>
        <p class="quote-author">— Lao Tzu</p>
    </div>
</body>
```

### Example 7: Form button with bottom padding

```html
<style>
    .form-group {
        padding: 15pt 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .form-actions {
        padding-top: 15pt;
        padding-bottom: 25pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: white;
        border-top: 2pt solid #e5e7eb;
    }
    .button {
        padding: 10pt 20pt;
        background-color: #2563eb;
        color: white;
        border: none;
    }
</style>
<body>
    <div class="form-group">
        <label>Email</label>
        <input type="email" />
    </div>
    <div class="form-actions">
        <button class="button">Submit</button>
    </div>
</body>
```

### Example 8: Invoice section with bottom padding

```html
<style>
    .invoice-section {
        padding-top: 20pt;
        padding-bottom: 30pt;
        padding-left: 25pt;
        padding-right: 25pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .section-title {
        margin: 0 0 15pt 0;
        font-size: 14pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="invoice-section">
        <h3 class="section-title">Payment Details</h3>
        <p style="margin: 0;">Payment information with extra bottom padding.</p>
    </div>
</body>
```

### Example 9: Product description with bottom padding

```html
<style>
    .product {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .product-header {
        padding: 15pt 20pt;
        background-color: #f3f4f6;
        border-bottom: 1pt solid #e5e7eb;
    }
    .product-description {
        padding-top: 15pt;
        padding-bottom: 25pt;
        padding-left: 20pt;
        padding-right: 20pt;
    }
    .product-footer {
        padding: 12pt 20pt;
        background-color: #dcfce7;
        border-top: 1pt solid #e5e7eb;
    }
</style>
<body>
    <div class="product">
        <div class="product-header">
            <h3 style="margin: 0;">Premium Widget</h3>
        </div>
        <div class="product-description">
            <p style="margin: 0;">Detailed product description.</p>
        </div>
        <div class="product-footer">
            <p style="margin: 0; color: #16a34a; font-weight: bold;">$99.99</p>
        </div>
    </div>
</body>
```

### Example 10: Newsletter header with bottom padding

```html
<style>
    .newsletter-header {
        padding-top: 30pt;
        padding-bottom: 35pt;
        padding-left: 25pt;
        padding-right: 25pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
    .newsletter-title {
        margin: 0 0 12pt 0;
        font-size: 24pt;
    }
    .newsletter-subtitle {
        margin: 0;
        font-size: 14pt;
    }
</style>
<body>
    <div class="newsletter-header">
        <h1 class="newsletter-title">Monthly Newsletter</h1>
        <p class="newsletter-subtitle">January 2025</p>
    </div>
</body>
```

### Example 11: Receipt footer with bottom padding

```html
<style>
    .receipt {
        width: 300pt;
        margin: 40pt auto;
        border: 2pt solid #000;
    }
    .receipt-items {
        padding: 15pt 20pt;
    }
    .receipt-footer {
        padding-top: 15pt;
        padding-bottom: 25pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: #f9fafb;
        border-top: 2pt solid #000;
        text-align: center;
    }
</style>
<body>
    <div class="receipt">
        <div class="receipt-items">
            <p>Items here</p>
        </div>
        <div class="receipt-footer">
            <p style="margin: 0; font-weight: bold;">Thank you for your business!</p>
        </div>
    </div>
</body>
```

### Example 12: Report conclusion with bottom padding

```html
<style>
    .report-section {
        padding: 20pt 30pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .conclusion {
        padding-top: 25pt;
        padding-bottom: 40pt;
        padding-left: 30pt;
        padding-right: 30pt;
        background-color: #dbeafe;
        border-top: 3pt solid #2563eb;
    }
</style>
<body>
    <div class="report-section">
        <p>Regular content.</p>
    </div>
    <div class="conclusion">
        <h2 style="margin: 0 0 15pt 0;">Conclusion</h2>
        <p style="margin: 0;">Summary with extra bottom padding.</p>
    </div>
</body>
```

### Example 13: Badge with bottom padding

```html
<style>
    .badge-pill {
        display: inline-block;
        padding-top: 5pt;
        padding-bottom: 8pt;
        padding-left: 12pt;
        padding-right: 12pt;
        background-color: #e0e7ff;
        color: #3730a3;
        border-radius: 20pt;
        font-size: 10pt;
        margin-right: 8pt;
    }
</style>
<body>
    <span class="badge-pill">Featured</span>
    <span class="badge-pill">New</span>
</body>
```

### Example 14: Certificate body with bottom padding

```html
<style>
    .certificate {
        width: 500pt;
        margin: 50pt auto;
        border: 5pt double #1e3a8a;
    }
    .cert-body {
        padding-top: 40pt;
        padding-bottom: 50pt;
        padding-left: 40pt;
        padding-right: 40pt;
        text-align: center;
    }
    .cert-signature {
        padding-top: 15pt;
        padding-bottom: 30pt;
        padding-left: 40pt;
        padding-right: 40pt;
        background-color: #f9fafb;
        border-top: 2pt solid #1e3a8a;
        text-align: center;
    }
</style>
<body>
    <div class="certificate">
        <div class="cert-body">
            <h1 style="margin: 0 0 20pt 0;">Certificate</h1>
            <p style="margin: 0;">Awarded to Jane Doe</p>
        </div>
        <div class="cert-signature">
            <p style="margin: 0;">Authorized Signature</p>
        </div>
    </div>
</body>
```

### Example 15: Section with visual separation

```html
<style>
    .content-section {
        padding-top: 20pt;
        padding-bottom: 35pt;
        padding-left: 25pt;
        padding-right: 25pt;
        background-color: white;
        border-bottom: 3pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .section-heading {
        margin: 0 0 15pt 0;
        padding-bottom: 10pt;
        border-bottom: 1pt solid #d1d5db;
        font-size: 18pt;
    }
</style>
<body>
    <div class="content-section">
        <h2 class="section-heading">Section Title</h2>
        <p style="margin: 0;">Content with generous bottom padding.</p>
    </div>
</body>
```

---

## See Also

- [padding](/reference/cssproperties/css_prop_padding) - Set all padding shorthand
- [padding-top](/reference/cssproperties/css_prop_padding-top) - Set top padding
- [padding-left](/reference/cssproperties/css_prop_padding-left) - Set left padding
- [padding-right](/reference/cssproperties/css_prop_padding-right) - Set right padding
- [margin-bottom](/reference/cssproperties/css_prop_margin-bottom) - Set bottom margin
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
