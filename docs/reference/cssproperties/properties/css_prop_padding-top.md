---
layout: default
title: padding-top
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# padding-top : Top Padding Property

The `padding-top` property sets the top padding of an element in PDF documents. Top padding creates space inside the element above the content, between the top border and the content, inheriting the element's background color.

## Usage

```css
selector {
    padding-top: value;
}
```

The padding-top property accepts a single length value or percentage that defines the space above the content.

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
- `0` - No top padding

---

## Supported Elements

The `padding-top` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Lists (`<ul>`, `<ol>`)
- List items (`<li>`)
- All container elements

---

## Notes

- Top padding inherits the element's background color
- Top padding increases the overall height of the element
- Percentage top padding is calculated relative to parent element's width (not height)
- Top padding cannot be negative
- Top padding is inside the border, while margin is outside
- Background colors and images extend into the padding area
- Top padding creates vertical space that's part of the element's clickable area
- Essential for creating visual breathing room at the top of elements

---

## Data Binding

The `padding-top` property supports dynamic values through data binding, allowing you to create flexible top internal spacing that adapts to header sizes, content importance, or layout requirements.

### Example 1: Dynamic header padding based on header type

```html
<style>
    .page-header {
        padding-top: {{header.size === 'large' ? '40pt' : '30pt'}};
        padding-bottom: 15pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: #1e3a8a;
        color: white;
    }
</style>
<body>
    <div class="page-header">
        <h1>{{header.title}}</h1>
        <p>{{header.subtitle}}</p>
    </div>
</body>
```

Data context:
```json
{
    "header": {
        "size": "large",
        "title": "Welcome",
        "subtitle": "Large headers get extra top padding for prominence"
    }
}
```

### Example 2: Card headers with variable top padding

```html
<style>
    .card {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .card-header {
        padding-top: {{card.featured ? '25pt' : '15pt'}};
        padding-bottom: 15pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: {{card.featured ? '#dbeafe' : '#f3f4f6'}};
        border-bottom: 1pt solid #e5e7eb;
    }
</style>
<body>
    <div class="card">
        <div class="card-header">
            <h3>{{card.title}}</h3>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "card": {
        "featured": true,
        "title": "Featured Content"
    }
}
```

### Example 3: Report cover with data-driven padding

```html
<style>
    .report-cover {
        padding-top: {{design.coverTopPadding}}pt;
        padding-bottom: 50pt;
        padding-left: 40pt;
        padding-right: 40pt;
        background-color: #1e3a8a;
        color: white;
        text-align: center;
    }
</style>
<body>
    <div class="report-cover">
        <h1>{{report.title}}</h1>
        <p>{{report.year}}</p>
    </div>
</body>
```

Data context:
```json
{
    "design": {
        "coverTopPadding": 100
    },
    "report": {
        "title": "Annual Report",
        "year": "2025"
    }
}
```

---

## Examples

### Example 1: Basic top padding

```html
<style>
    .box {
        padding-top: 20pt;
        padding-bottom: 10pt;
        padding-left: 15pt;
        padding-right: 15pt;
        background-color: #dbeafe;
        border: 2pt solid #3b82f6;
    }
</style>
<body>
    <div class="box">
        <p>This box has more padding at the top than other sides.</p>
    </div>
</body>
```

### Example 2: Header with top padding

```html
<style>
    .page-header {
        padding-top: 30pt;
        padding-bottom: 15pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: #1e3a8a;
        color: white;
    }
    .page-header h1 {
        margin: 0;
    }
</style>
<body>
    <div class="page-header">
        <h1>Page Title</h1>
        <p>Subtitle with extra top padding</p>
    </div>
</body>
```

### Example 3: Card with top padding

```html
<style>
    .card {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .card-header {
        padding-top: 20pt;
        padding-bottom: 15pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: #f3f4f6;
        border-bottom: 1pt solid #e5e7eb;
    }
    .card-body {
        padding-top: 15pt;
        padding-bottom: 15pt;
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
            <p>Card content area.</p>
        </div>
    </div>
</body>
```

### Example 4: Alert with icon space

```html
<style>
    .alert {
        padding-top: 12pt;
        padding-bottom: 12pt;
        padding-left: 45pt;
        padding-right: 15pt;
        background-color: #dbeafe;
        border: 1pt solid #3b82f6;
        border-left: 4pt solid #3b82f6;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="alert">
        <strong>Info:</strong> Extra left padding for icon placement.
    </div>
</body>
```

### Example 5: Invoice header with top padding

```html
<style>
    .invoice-header {
        padding-top: 35pt;
        padding-bottom: 20pt;
        padding-left: 30pt;
        padding-right: 30pt;
        background-color: #1e3a8a;
        color: white;
        text-align: center;
    }
    .invoice-header h1 {
        margin: 0 0 10pt 0;
        font-size: 28pt;
    }
</style>
<body>
    <div class="invoice-header">
        <h1>INVOICE</h1>
        <p style="margin: 0;">INV-2025-001</p>
    </div>
</body>
```

### Example 6: Form group with top padding

```html
<style>
    .form-group {
        padding-top: 15pt;
        padding-bottom: 15pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .form-label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="form-group">
        <label class="form-label">Full Name</label>
        <input type="text" />
    </div>
</body>
```

### Example 7: Table header with top padding

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th {
        padding-top: 15pt;
        padding-bottom: 12pt;
        padding-left: 12pt;
        padding-right: 12pt;
        background-color: #1f2937;
        color: white;
        text-align: left;
    }
    .data-table td {
        padding-top: 10pt;
        padding-bottom: 10pt;
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
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget</td>
                <td>$29.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 8: Section with header padding

```html
<style>
    .section {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .section-header {
        padding-top: 20pt;
        padding-bottom: 15pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: #dbeafe;
        border-bottom: 2pt solid #3b82f6;
    }
    .section-content {
        padding: 15pt 20pt;
    }
</style>
<body>
    <div class="section">
        <div class="section-header">
            <h2 style="margin: 0;">Section Title</h2>
        </div>
        <div class="section-content">
            <p>Section content area.</p>
        </div>
    </div>
</body>
```

### Example 9: Quote block with top padding

```html
<style>
    .quote-block {
        padding-top: 25pt;
        padding-bottom: 20pt;
        padding-left: 30pt;
        padding-right: 20pt;
        background-color: #f5f5f5;
        border-left: 5pt solid #6366f1;
        font-style: italic;
        margin: 20pt 0;
    }
</style>
<body>
    <div class="quote-block">
        <p>"Success is not final, failure is not fatal."</p>
    </div>
</body>
```

### Example 10: Product card with top padding

```html
<style>
    .product-card {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .product-image {
        padding-top: 100pt;
        background-color: #d1d5db;
    }
    .product-info {
        padding-top: 15pt;
        padding-bottom: 15pt;
        padding-left: 15pt;
        padding-right: 15pt;
    }
</style>
<body>
    <div class="product-card">
        <div class="product-image"></div>
        <div class="product-info">
            <h3 style="margin: 0 0 8pt 0;">Product Name</h3>
            <p style="margin: 0;">$99.99</p>
        </div>
    </div>
</body>
```

### Example 11: Newsletter header with top padding

```html
<style>
    .newsletter-header {
        padding-top: 40pt;
        padding-bottom: 25pt;
        padding-left: 30pt;
        padding-right: 30pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
</style>
<body>
    <div class="newsletter-header">
        <h1 style="margin: 0 0 10pt 0;">Monthly Newsletter</h1>
        <p style="margin: 0;">January 2025</p>
    </div>
</body>
```

### Example 12: Receipt header with top padding

```html
<style>
    .receipt {
        width: 300pt;
        margin: 40pt auto;
        border: 2pt solid #000;
    }
    .receipt-header {
        padding-top: 25pt;
        padding-bottom: 20pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: #f9fafb;
        text-align: center;
        border-bottom: 2pt solid #000;
    }
</style>
<body>
    <div class="receipt">
        <div class="receipt-header">
            <h2 style="margin: 0 0 8pt 0;">Store Name</h2>
            <p style="margin: 0;">Receipt #12345</p>
        </div>
    </div>
</body>
```

### Example 13: Report title with top padding

```html
<style>
    .report-cover {
        padding-top: 100pt;
        padding-bottom: 50pt;
        padding-left: 40pt;
        padding-right: 40pt;
        background-color: #1e3a8a;
        color: white;
        text-align: center;
    }
    .report-title {
        margin: 0 0 20pt 0;
        font-size: 32pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="report-cover">
        <h1 class="report-title">Annual Report</h1>
        <p style="margin: 0; font-size: 18pt;">2025</p>
    </div>
</body>
```

### Example 14: Badge with top padding

```html
<style>
    .badge {
        display: inline-block;
        padding-top: 6pt;
        padding-bottom: 6pt;
        padding-left: 12pt;
        padding-right: 12pt;
        background-color: #3b82f6;
        color: white;
        border-radius: 3pt;
        font-size: 10pt;
        margin-right: 8pt;
    }
</style>
<body>
    <span class="badge">New</span>
    <span class="badge">Featured</span>
    <span class="badge">Popular</span>
</body>
```

### Example 15: Certificate with top padding

```html
<style>
    .certificate {
        width: 500pt;
        margin: 50pt auto;
        border: 5pt double #1e3a8a;
    }
    .cert-header {
        padding-top: 50pt;
        padding-bottom: 30pt;
        padding-left: 40pt;
        padding-right: 40pt;
        background-color: #f9fafb;
        text-align: center;
        border-bottom: 2pt solid #1e3a8a;
    }
    .cert-body {
        padding: 40pt;
        text-align: center;
    }
</style>
<body>
    <div class="certificate">
        <div class="cert-header">
            <h1 style="margin: 0; color: #1e3a8a;">Certificate of Excellence</h1>
        </div>
        <div class="cert-body">
            <p>Awarded to</p>
            <h2 style="margin: 15pt 0;">Jane Smith</h2>
        </div>
    </div>
</body>
```

---

## See Also

- [padding](/reference/cssproperties/css_prop_padding) - Set all padding shorthand
- [padding-bottom](/reference/cssproperties/css_prop_padding-bottom) - Set bottom padding
- [padding-left](/reference/cssproperties/css_prop_padding-left) - Set left padding
- [padding-right](/reference/cssproperties/css_prop_padding-right) - Set right padding
- [margin-top](/reference/cssproperties/css_prop_margin-top) - Set top margin
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
