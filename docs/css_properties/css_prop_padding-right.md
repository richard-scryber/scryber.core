---
layout: default
title: padding-right
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# padding-right : Right Padding Property

The `padding-right` property sets the right padding of an element in PDF documents. Right padding creates space inside the element to the right of the content, between the right border and the content, inheriting the element's background color.

## Usage

```css
selector {
    padding-right: value;
}
```

The padding-right property accepts a single length value or percentage that defines the space to the right of the content.

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
- `0` - No right padding

---

## Supported Elements

The `padding-right` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Lists (`<ul>`, `<ol>`)
- All container elements

---

## Notes

- Right padding inherits the element's background color
- Right padding increases the overall width of the element
- Percentage right padding is calculated relative to parent element's width
- Right padding cannot be negative
- Right padding is inside the border, while margin is outside
- Background colors and images extend into the padding area
- Right padding is particularly useful for spacing text away from right borders
- Essential for creating proper horizontal spacing in layouts

---

## Data Binding

The `padding-right` property supports dynamic values through data binding, allowing you to create flexible right internal spacing for icons, badges, or content alignment based on layout requirements.

### Example 1: Alert boxes with icon space

```html
<style>
    .alert {
        padding-top: 12pt;
        padding-bottom: 12pt;
        padding-left: {{alert.hasIcon ? '45pt' : '15pt'}};
        padding-right: {{alert.hasIcon ? '15pt' : '15pt'}};
        background-color: #dbeafe;
        border: 1pt solid #3b82f6;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="alert">
        <strong>Info:</strong> {{alert.message}}
    </div>
</body>
```

Data context:
```json
{
    "alert": {
        "hasIcon": true,
        "message": "Extra left padding for icon placement"
    }
}
```

### Example 2: Table cells with dynamic padding

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        padding-top: 10pt;
        padding-bottom: 10pt;
        padding-left: 12pt;
        padding-right: {{table.denseMode ? '8pt' : '12pt'}};
        border: 1pt solid #d1d5db;
    }
    .data-table th {
        background-color: #1f2937;
        color: white;
        text-align: left;
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

Data context:
```json
{
    "table": {
        "denseMode": false
    }
}
```

### Example 3: Button padding based on size

```html
<style>
    .button {
        padding-top: {{button.size === 'large' ? '12pt' : '8pt'}};
        padding-bottom: {{button.size === 'large' ? '12pt' : '8pt'}};
        padding-left: {{button.size === 'large' ? '25pt' : '15pt'}};
        padding-right: {{button.size === 'large' ? '25pt' : '15pt'}};
        background-color: #2563eb;
        color: white;
        border: none;
    }
</style>
<body>
    <button class="button">{{button.label}}</button>
</body>
```

Data context:
```json
{
    "button": {
        "size": "large",
        "label": "Submit"
    }
}
```

---

## Examples

### Example 1: Basic right padding

```html
<style>
    .box {
        padding-top: 15pt;
        padding-bottom: 15pt;
        padding-left: 15pt;
        padding-right: 25pt;
        background-color: #dbeafe;
        border: 2pt solid #3b82f6;
    }
</style>
<body>
    <div class="box">
        <p>This box has more padding on the right side.</p>
    </div>
</body>
```

### Example 2: Text with right padding for alignment

```html
<style>
    .aligned-text {
        padding-right: 20pt;
        background-color: #f3f4f6;
        text-align: right;
        border-right: 3pt solid #3b82f6;
    }
</style>
<body>
    <div class="aligned-text">
        <p>Right-aligned text with right padding.</p>
    </div>
</body>
```

### Example 3: Table cells with right padding

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        padding-top: 10pt;
        padding-bottom: 10pt;
        padding-left: 12pt;
        padding-right: 20pt;
        border: 1pt solid #d1d5db;
    }
    .data-table th {
        background-color: #1f2937;
        color: white;
        text-align: left;
    }
    .data-table .numeric {
        text-align: right;
        padding-right: 25pt;
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

### Example 4: Alert with icon space on right

```html
<style>
    .alert-right {
        padding-top: 12pt;
        padding-bottom: 12pt;
        padding-left: 15pt;
        padding-right: 45pt;
        background-color: #dcfce7;
        border: 1pt solid #16a34a;
        border-right: 4pt solid #16a34a;
    }
</style>
<body>
    <div class="alert-right">
        <strong>Success:</strong> Extra right padding for icon or action button.
    </div>
</body>
```

### Example 5: Sidebar with right padding

```html
<style>
    .sidebar {
        width: 150pt;
        float: left;
        padding-top: 20pt;
        padding-bottom: 20pt;
        padding-left: 15pt;
        padding-right: 20pt;
        background-color: #f3f4f6;
        border-right: 2pt solid #d1d5db;
    }
</style>
<body>
    <div class="sidebar">
        <h3 style="margin: 0 0 15pt 0;">Navigation</h3>
        <p>Menu items</p>
    </div>
</body>
```

### Example 6: Form label with right padding

```html
<style>
    .form-row {
        margin-bottom: 15pt;
        overflow: hidden;
    }
    .form-label {
        float: left;
        width: 120pt;
        padding-top: 8pt;
        padding-right: 20pt;
        font-weight: bold;
        text-align: right;
    }
    .form-input {
        float: left;
        padding: 8pt;
        border: 1pt solid #d1d5db;
        width: 200pt;
    }
</style>
<body>
    <div class="form-row">
        <label class="form-label">Full Name:</label>
        <input class="form-input" type="text" />
    </div>
</body>
```

### Example 7: Quote block with right padding

```html
<style>
    .quote-block {
        padding-top: 20pt;
        padding-bottom: 20pt;
        padding-left: 30pt;
        padding-right: 40pt;
        background-color: #f5f5f5;
        border-left: 5pt solid #6366f1;
        font-style: italic;
        margin: 20pt 0;
    }
    .quote-author {
        text-align: right;
        padding-right: 10pt;
        margin-top: 10pt;
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="quote-block">
        <p>"Quality is not an act, it is a habit."</p>
        <p class="quote-author">— Aristotle</p>
    </div>
</body>
```

### Example 8: Product card with right padding

```html
<style>
    .product-card {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
        overflow: hidden;
    }
    .product-image {
        float: left;
        width: 100pt;
        height: 100pt;
        background-color: #d1d5db;
    }
    .product-info {
        float: left;
        padding-top: 15pt;
        padding-bottom: 15pt;
        padding-left: 20pt;
        padding-right: 25pt;
    }
</style>
<body>
    <div class="product-card">
        <div class="product-image"></div>
        <div class="product-info">
            <h3 style="margin: 0 0 8pt 0;">Product Name</h3>
            <p style="margin: 0; color: #16a34a; font-weight: bold;">$99.99</p>
        </div>
    </div>
</body>
```

### Example 9: Invoice line items with right padding

```html
<style>
    .invoice-line {
        padding-top: 10pt;
        padding-bottom: 10pt;
        padding-left: 15pt;
        padding-right: 30pt;
        border-bottom: 1pt solid #d1d5db;
        display: flex;
        justify-content: space-between;
    }
    .item-description {
        flex: 1;
    }
    .item-price {
        padding-right: 10pt;
        font-weight: bold;
        text-align: right;
    }
</style>
<body>
    <div class="invoice-line">
        <span class="item-description">Consulting Services</span>
        <span class="item-price">$1,500.00</span>
    </div>
</body>
```

### Example 10: Newsletter column with right padding

```html
<style>
    .newsletter-column {
        float: left;
        width: 48%;
        padding-top: 15pt;
        padding-bottom: 15pt;
        padding-left: 15pt;
        padding-right: 20pt;
        background-color: #f9fafb;
        margin-right: 2%;
    }
</style>
<body>
    <div class="newsletter-column">
        <h3 style="margin: 0 0 10pt 0;">Column Title</h3>
        <p>Column content with extra right padding.</p>
    </div>
</body>
```

### Example 11: Badge with asymmetric padding

```html
<style>
    .badge-custom {
        display: inline-block;
        padding-top: 6pt;
        padding-bottom: 6pt;
        padding-left: 10pt;
        padding-right: 15pt;
        background-color: #e0e7ff;
        color: #3730a3;
        border-radius: 3pt;
        font-size: 10pt;
    }
</style>
<body>
    <span class="badge-custom">Premium</span>
</body>
```

### Example 12: Business card section with right padding

```html
<style>
    .business-card {
        width: 350pt;
        height: 200pt;
        padding: 20pt;
        border: 2pt solid #1e3a8a;
        overflow: hidden;
    }
    .card-left {
        float: left;
        width: 60%;
        padding-right: 20pt;
    }
    .card-right {
        float: left;
        width: 40%;
    }
</style>
<body>
    <div class="business-card">
        <div class="card-left">
            <h2 style="margin: 0 0 5pt 0;">John Doe</h2>
            <p style="margin: 0;">Senior Manager</p>
        </div>
        <div class="card-right">
            <p>Contact info</p>
        </div>
    </div>
</body>
```

### Example 13: Report section with right padding

```html
<style>
    .report-section {
        padding-top: 20pt;
        padding-bottom: 20pt;
        padding-left: 25pt;
        padding-right: 30pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .section-title {
        margin: 0 0 15pt 0;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #d1d5db;
    }
</style>
<body>
    <div class="report-section">
        <h2 class="section-title">Financial Summary</h2>
        <p>Section content with proper right padding.</p>
    </div>
</body>
```

### Example 14: Receipt with aligned columns

```html
<style>
    .receipt-item {
        padding-top: 8pt;
        padding-bottom: 8pt;
        padding-left: 10pt;
        padding-right: 25pt;
        border-bottom: 1pt dashed #d1d5db;
    }
    .item-name {
        display: inline-block;
        width: 60%;
    }
    .item-price {
        display: inline-block;
        width: 35%;
        text-align: right;
        padding-right: 10pt;
    }
</style>
<body>
    <div class="receipt-item">
        <span class="item-name">Widget A</span>
        <span class="item-price">$29.99</span>
    </div>
</body>
```

### Example 15: Breadcrumb with right padding

```html
<style>
    .breadcrumb {
        padding-top: 12pt;
        padding-bottom: 12pt;
        padding-left: 20pt;
        padding-right: 20pt;
        background-color: #f3f4f6;
    }
    .breadcrumb-item {
        display: inline-block;
        padding-right: 15pt;
        font-size: 11pt;
    }
    .breadcrumb-item:after {
        content: " /";
        margin-left: 8pt;
        color: #6b7280;
    }
    .breadcrumb-item:last-child:after {
        content: "";
    }
</style>
<body>
    <div class="breadcrumb">
        <span class="breadcrumb-item">Home</span>
        <span class="breadcrumb-item">Products</span>
        <span class="breadcrumb-item">Item</span>
    </div>
</body>
```

---

## See Also

- [padding](/reference/cssproperties/css_prop_padding) - Set all padding shorthand
- [padding-left](/reference/cssproperties/css_prop_padding-left) - Set left padding
- [padding-top](/reference/cssproperties/css_prop_padding-top) - Set top padding
- [padding-bottom](/reference/cssproperties/css_prop_padding-bottom) - Set bottom padding
- [margin-right](/reference/cssproperties/css_prop_margin-right) - Set right margin
- [padding-inline-end](/reference/cssproperties/css_prop_padding-inline-end) - Set inline end padding
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
