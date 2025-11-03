---
layout: default
title: padding-inline-end
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# padding-inline-end : Inline End Padding Property

The `padding-inline-end` property sets the padding at the inline end edge of an element in PDF documents. This is a logical property that maps to either right or left padding depending on the writing direction. In left-to-right (LTR) languages it corresponds to `padding-right`, while in right-to-left (RTL) languages it corresponds to `padding-left`.

## Usage

```css
selector {
    padding-inline-end: value;
}
```

The padding-inline-end property accepts a single length value or percentage that defines the space at the inline end edge.

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
- `0` - No inline end padding

---

## Supported Elements

The `padding-inline-end` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Lists (`<ul>`, `<ol>`)
- All container elements

---

## Notes

- This is a logical property that adapts to text direction
- In LTR contexts (English, Spanish, etc.), it behaves like `padding-right`
- In RTL contexts (Arabic, Hebrew, etc.), it behaves like `padding-left`
- Useful for creating internationalized documents
- Provides better semantic meaning than physical properties
- Simplifies maintenance of bidirectional layouts
- Padding inherits the element's background color
- Percentage values are relative to parent width

---

## Data Binding

The `padding-inline-end` property supports dynamic values through data binding, allowing you to create direction-aware, flexible end internal spacing for buttons, badges, or layout alignment based on content requirements.

### Example 1: Alert with dynamic action button space

```html
<style>
    .alert {
        padding-inline-start: 15pt;
        padding-inline-end: {{alert.hasAction ? '60pt' : '15pt'}};
        padding-block: 12pt;
        background-color: #fef3c7;
        border-inline-end: {{alert.hasAction ? '4pt' : '1pt'}} solid #f59e0b;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="alert">
        <strong>Warning:</strong> {{alert.message}}
    </div>
</body>
```

Data context:
```json
{
    "alert": {
        "hasAction": true,
        "message": "Space reserved for action button at inline end"
    }
}
```

### Example 2: Table cells with dynamic end padding

```html
<style>
    .data-table th,
    .data-table td {
        padding-inline-start: 12pt;
        padding-inline-end: {{table.spacious ? '25pt' : '15pt'}};
        padding-block: 10pt;
        border: 1pt solid #d1d5db;
    }
    .data-table .numeric {
        text-align: end;
        padding-inline-end: {{table.spacious ? '30pt' : '20pt'}};
    }
</style>
<body>
    <table class="data-table">
        <tr>
            <td>Product</td>
            <td class="numeric">$1,299.99</td>
        </tr>
    </table>
</body>
```

Data context:
```json
{
    "table": {
        "spacious": true
    }
}
```

### Example 3: Quote block with asymmetric padding

```html
<style>
    .quote-block {
        padding-inline-start: 30pt;
        padding-inline-end: {{quote.hasAttribution ? '40pt' : '30pt'}};
        padding-block: 20pt;
        background-color: #f5f5f5;
        border-inline-start: 6pt solid #6366f1;
        font-style: italic;
    }
    .quote-author {
        text-align: end;
        padding-inline-end: {{quote.hasAttribution ? '10pt' : '0'}};
        margin-top: 10pt;
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="quote-block">
        <p>{{quote.text}}</p>
        {{#if quote.hasAttribution}}
        <p class="quote-author">— {{quote.author}}</p>
        {{/if}}
    </div>
</body>
```

Data context:
```json
{
    "quote": {
        "hasAttribution": true,
        "text": "The only way to do great work is to love what you do.",
        "author": "Steve Jobs"
    }
}
```

---

## Examples

### Example 1: Basic inline end padding

```html
<style>
    .box {
        padding-inline-start: 15pt;
        padding-inline-end: 30pt;
        padding-block: 15pt;
        background-color: #dbeafe;
        border-inline-end: 4pt solid #3b82f6;
    }
</style>
<body>
    <div class="box">
        <p style="margin: 0;">Box with extra inline end padding.</p>
    </div>
</body>
```

### Example 2: Alert with action button space

```html
<style>
    .alert {
        padding-inline-start: 15pt;
        padding-inline-end: 50pt;
        padding-block: 12pt;
        background-color: #fef3c7;
        border-inline-end: 4pt solid #f59e0b;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="alert">
        <strong>Warning:</strong> Space reserved for action button at inline end.
    </div>
</body>
```

### Example 3: Quote block with asymmetric padding

```html
<style>
    .quote-block {
        padding-inline-start: 30pt;
        padding-inline-end: 40pt;
        padding-block: 20pt;
        background-color: #f5f5f5;
        border-inline-start: 6pt solid #6366f1;
        font-style: italic;
        margin: 20pt 0;
    }
    .quote-author {
        text-align: end;
        padding-inline-end: 10pt;
        margin-top: 10pt;
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="quote-block">
        <p style="margin: 0;">"The future belongs to those who believe in the beauty of their dreams."</p>
        <p class="quote-author">— Eleanor Roosevelt</p>
    </div>
</body>
```

### Example 4: Table cells with inline end padding

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        padding-inline-start: 12pt;
        padding-inline-end: 20pt;
        padding-block: 10pt;
        border: 1pt solid #d1d5db;
        text-align: start;
    }
    .data-table th {
        background-color: #1f2937;
        color: white;
    }
    .data-table .numeric {
        text-align: end;
        padding-inline-end: 25pt;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th>Product</th>
                <th class="numeric">Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget</td>
                <td class="numeric">$1,299.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 5: Form label alignment

```html
<style>
    .form-row {
        margin-bottom: 15pt;
        display: flex;
    }
    .form-label {
        width: 120pt;
        padding-block: 8pt;
        padding-inline-end: 20pt;
        font-weight: bold;
        text-align: end;
    }
    .form-input {
        flex: 1;
        padding: 8pt 12pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="form-row">
        <label class="form-label">Full Name:</label>
        <input class="form-input" type="text" />
    </div>
</body>
```

### Example 6: Sidebar with inline end padding

```html
<style>
    .sidebar {
        width: 150pt;
        padding-inline-start: 15pt;
        padding-inline-end: 20pt;
        padding-block: 20pt;
        background-color: #f3f4f6;
        border-inline-end: 2pt solid #d1d5db;
    }
</style>
<body>
    <div class="sidebar">
        <h3 style="margin: 0 0 15pt 0;">Quick Links</h3>
        <p>Navigation items</p>
    </div>
</body>
```

### Example 7: Product card with inline end padding

```html
<style>
    .product-card {
        padding-inline-start: 20pt;
        padding-inline-end: 25pt;
        padding-block: 20pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .product-price {
        text-align: end;
        padding-inline-end: 5pt;
        color: #16a34a;
        font-weight: bold;
        font-size: 18pt;
    }
</style>
<body>
    <div class="product-card">
        <h3 style="margin: 0 0 10pt 0;">Premium Widget</h3>
        <p class="product-price" style="margin: 0;">$99.99</p>
    </div>
</body>
```

### Example 8: Invoice line items

```html
<style>
    .invoice-line {
        padding-inline-start: 15pt;
        padding-inline-end: 30pt;
        padding-block: 10pt;
        border-bottom: 1pt solid #d1d5db;
        display: flex;
        justify-content: space-between;
    }
    .item-price {
        padding-inline-end: 10pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="invoice-line">
        <span>Consulting Services</span>
        <span class="item-price">$1,500.00</span>
    </div>
</body>
```

### Example 9: Newsletter column

```html
<style>
    .newsletter-column {
        width: 48%;
        padding-inline-start: 15pt;
        padding-inline-end: 20pt;
        padding-block: 15pt;
        background-color: #f9fafb;
        display: inline-block;
        vertical-align: top;
    }
</style>
<body>
    <div class="newsletter-column">
        <h3 style="margin: 0 0 10pt 0;">Column Title</h3>
        <p style="margin: 0;">Content with inline end padding.</p>
    </div>
</body>
```

### Example 10: Badge with asymmetric padding

```html
<style>
    .badge {
        display: inline-block;
        padding-inline-start: 10pt;
        padding-inline-end: 15pt;
        padding-block: 6pt;
        background-color: #e0e7ff;
        color: #3730a3;
        border-radius: 3pt;
        font-size: 10pt;
    }
</style>
<body>
    <span class="badge">Featured</span>
</body>
```

### Example 11: Business card section

```html
<style>
    .business-card {
        width: 350pt;
        height: 200pt;
        padding: 20pt;
        border: 2pt solid #1e3a8a;
    }
    .card-section {
        padding-inline-end: 20pt;
        display: inline-block;
        width: 60%;
        vertical-align: top;
    }
</style>
<body>
    <div class="business-card">
        <div class="card-section">
            <h2 style="margin: 0;">John Doe</h2>
            <p style="margin: 5pt 0 0 0;">Manager</p>
        </div>
    </div>
</body>
```

### Example 12: Receipt aligned columns

```html
<style>
    .receipt-item {
        padding-inline-start: 10pt;
        padding-inline-end: 25pt;
        padding-block: 8pt;
        border-bottom: 1pt dashed #d1d5db;
        display: flex;
        justify-content: space-between;
    }
    .item-price {
        padding-inline-end: 10pt;
    }
</style>
<body>
    <div class="receipt-item">
        <span>Widget A</span>
        <span class="item-price">$29.99</span>
    </div>
</body>
```

### Example 13: Report section

```html
<style>
    .report-section {
        padding-inline-start: 25pt;
        padding-inline-end: 30pt;
        padding-block: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="report-section">
        <h2 style="margin: 0 0 15pt 0;">Financial Summary</h2>
        <p style="margin: 0;">Content with proper inline end padding.</p>
    </div>
</body>
```

### Example 14: Breadcrumb navigation

```html
<style>
    .breadcrumb {
        padding-inline: 20pt;
        padding-block: 12pt;
        background-color: #f3f4f6;
    }
    .breadcrumb-item {
        display: inline-block;
        padding-inline-end: 15pt;
        font-size: 11pt;
    }
</style>
<body>
    <div class="breadcrumb">
        <span class="breadcrumb-item">Home /</span>
        <span class="breadcrumb-item">Products /</span>
        <span class="breadcrumb-item">Item</span>
    </div>
</body>
```

### Example 15: Certificate

```html
<style>
    .certificate {
        width: 500pt;
        margin: 50pt auto;
        border: 5pt double #1e3a8a;
    }
    .cert-content {
        padding-inline-start: 40pt;
        padding-inline-end: 60pt;
        padding-block: 50pt;
    }
</style>
<body>
    <div class="certificate">
        <div class="cert-content">
            <h1 style="margin: 0 0 30pt 0; color: #1e3a8a;">Certificate of Excellence</h1>
            <p style="margin: 0;">Awarded to Michael Brown</p>
        </div>
    </div>
</body>
```

---

## See Also

- [padding-inline-start](/reference/cssproperties/css_prop_padding-inline-start) - Set inline start padding
- [padding-inline](/reference/cssproperties/css_prop_padding-inline) - Set both inline paddings shorthand
- [padding-right](/reference/cssproperties/css_prop_padding-right) - Set right padding (physical property)
- [padding-left](/reference/cssproperties/css_prop_padding-left) - Set left padding (physical property)
- [margin-inline-end](/reference/cssproperties/css_prop_margin-inline-end) - Set inline end margin
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
