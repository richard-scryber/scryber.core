---
layout: default
title: padding-inline
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# padding-inline : Inline Padding Shorthand Property

The `padding-inline` property is a logical shorthand for setting both `padding-inline-start` and `padding-inline-end` of an element in PDF documents. This property adapts to the writing direction, making it ideal for creating internationalized documents that work in both LTR and RTL languages.

## Usage

```css
selector {
    padding-inline: value;
}
```

The padding-inline property accepts 1-2 space-separated values with the following behavior:
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
- `0` - No inline padding

---

## Supported Elements

The `padding-inline` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Lists (`<ul>`, `<ol>`)
- All container elements

---

## Notes

- This is a logical shorthand property that adapts to text direction
- In LTR contexts, it controls left and right padding
- In RTL contexts, it controls right and left padding
- Provides semantic clarity for horizontal padding
- Useful for creating bidirectional layouts
- Padding inherits the element's background color
- Simplifies maintenance of internationalized documents
- Percentage values are relative to parent width

---

## Data Binding

The `padding-inline` property supports dynamic values through data binding, enabling you to create direction-aware, flexible horizontal internal spacing that adapts to layout requirements and internationalization needs.

### Example 1: Dynamic card padding based on content type

```html
<style>
    .card {
        padding-inline: {{card.type === 'featured' ? '30pt' : '20pt'}};
        padding-block: {{card.type === 'featured' ? '25pt' : '15pt'}};
        background-color: {{card.type === 'featured' ? '#dbeafe' : 'white'}};
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
</style>
<body>
    <div class="card">
        <h3>{{card.title}}</h3>
        <p>{{card.content}}</p>
    </div>
</body>
```

Data context:
```json
{
    "card": {
        "type": "featured",
        "title": "Featured Content",
        "content": "This card gets extra padding for emphasis"
    }
}
```

### Example 2: Responsive invoice layout with data-driven padding

```html
<style>
    .invoice {
        padding-inline: {{layout.pageInset}}pt;
        padding-block: 30pt;
    }
    .invoice-section {
        padding-inline: {{layout.sectionInset}}pt;
        padding-block: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-section">
            <h3>Bill To: {{customer.name}}</h3>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "layout": {
        "pageInset": 40,
        "sectionInset": 25
    },
    "customer": {
        "name": "Acme Corporation"
    }
}
```

### Example 3: Form fields with variable padding

```html
<style>
    .form-group {
        padding-inline: {{form.size === 'large' ? '25pt' : '20pt'}};
        padding-block: {{form.size === 'large' ? '20pt' : '15pt'}};
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .form-input {
        padding-inline: {{form.size === 'large' ? '15pt' : '12pt'}};
        padding-block: {{form.size === 'large' ? '12pt' : '10pt'}};
        border: 1pt solid #d1d5db;
        width: 100%;
    }
</style>
<body>
    <div class="form-group">
        <label>Email</label>
        <input class="form-input" type="email" />
    </div>
</body>
```

Data context:
```json
{
    "form": {
        "size": "large"
    }
}
```

---

## Examples

### Example 1: Uniform inline padding

```html
<style>
    .box {
        padding-inline: 20pt;
        padding-block: 15pt;
        background-color: #dbeafe;
        border: 2pt solid #3b82f6;
    }
</style>
<body>
    <div class="box">
        <p style="margin: 0;">Box with 20pt padding on both inline sides.</p>
    </div>
</body>
```

### Example 2: Asymmetric inline padding

```html
<style>
    .box {
        padding-inline: 30pt 15pt;
        padding-block: 15pt;
        background-color: #dcfce7;
        border-inline-start: 4pt solid #16a34a;
    }
</style>
<body>
    <div class="box">
        <p style="margin: 0;">30pt inline-start, 15pt inline-end padding.</p>
    </div>
</body>
```

### Example 3: Card with inline padding

```html
<style>
    .card {
        padding-inline: 25pt;
        padding-block: 20pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .card-header {
        margin: -20pt -25pt 15pt -25pt;
        padding-inline: 25pt;
        padding-block: 15pt;
        background-color: #f3f4f6;
        border-bottom: 1pt solid #e5e7eb;
    }
</style>
<body>
    <div class="card">
        <div class="card-header">
            <h3 style="margin: 0;">Card Title</h3>
        </div>
        <p style="margin: 0;">Card content with inline padding.</p>
    </div>
</body>
```

### Example 4: Alert with inline padding

```html
<style>
    .alert {
        padding-inline: 15pt;
        padding-block: 12pt;
        background-color: #fef3c7;
        border-inline-start: 4pt solid #f59e0b;
        margin-bottom: 10pt;
    }
    .alert-icon {
        padding-inline: 50pt 15pt;
    }
</style>
<body>
    <div class="alert alert-icon">
        <strong>Warning:</strong> Important notification message.
    </div>
</body>
```

### Example 5: Form with inline padding

```html
<style>
    .form-group {
        padding-inline: 20pt;
        padding-block: 15pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .form-input {
        padding-inline: 12pt;
        padding-block: 10pt;
        border: 1pt solid #d1d5db;
        width: 100%;
    }
</style>
<body>
    <div class="form-group">
        <label style="display: block; margin-bottom: 5pt; font-weight: bold;">Email</label>
        <input class="form-input" type="email" />
    </div>
</body>
```

### Example 6: Invoice with inline padding

```html
<style>
    .invoice {
        padding-inline: 40pt;
        padding-block: 30pt;
    }
    .invoice-header {
        padding-inline: 30pt;
        padding-block: 25pt;
        background-color: #1e3a8a;
        color: white;
        text-align: center;
        margin-inline: -40pt;
        margin-block-start: -30pt;
        margin-block-end: 30pt;
    }
    .invoice-section {
        padding-inline: 25pt;
        padding-block: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1 style="margin: 0;">INVOICE</h1>
        </div>
        <div class="invoice-section">
            <h3 style="margin: 0 0 10pt 0;">Bill To</h3>
            <p style="margin: 0;">Acme Corporation</p>
        </div>
    </div>
</body>
```

### Example 7: Table with inline padding

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        padding-inline: 15pt;
        padding-block: 10pt;
        border: 1pt solid #d1d5db;
        text-align: start;
    }
    .data-table th {
        background-color: #1f2937;
        color: white;
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

### Example 8: Quote block with inline padding

```html
<style>
    .quote-block {
        padding-inline: 35pt 25pt;
        padding-block: 20pt;
        background-color: #f5f5f5;
        border-inline-start: 6pt solid #6366f1;
        font-style: italic;
        margin: 20pt 0;
    }
</style>
<body>
    <div class="quote-block">
        <p style="margin: 0;">"Be yourself; everyone else is already taken."</p>
        <p style="margin: 10pt 0 0 0; font-size: 11pt; color: #6b7280; text-align: end;">— Oscar Wilde</p>
    </div>
</body>
```

### Example 9: Product catalog with inline padding

```html
<style>
    .catalog {
        padding-inline: 30pt;
        padding-block: 25pt;
    }
    .product {
        padding-inline: 25pt;
        padding-block: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .product-name {
        margin: 0 0 10pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
    .product-price {
        margin: 0;
        padding-inline: 15pt;
        padding-block: 8pt;
        background-color: #dcfce7;
        color: #16a34a;
        font-size: 18pt;
        font-weight: bold;
        display: inline-block;
    }
</style>
<body>
    <div class="catalog">
        <div class="product">
            <h3 class="product-name">Premium Widget</h3>
            <div class="product-price">$99.99</div>
        </div>
    </div>
</body>
```

### Example 10: Newsletter with inline padding

```html
<style>
    .newsletter {
        padding-inline: 35pt;
        padding-block: 30pt;
    }
    .newsletter-header {
        padding-inline: 30pt;
        padding-block: 25pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
        margin-inline: -35pt;
        margin-block-start: -30pt;
        margin-block-end: 30pt;
    }
    .newsletter-section {
        padding-inline: 25pt;
        padding-block: 20pt;
        background-color: #f9fafb;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1 style="margin: 0;">Monthly Newsletter</h1>
        </div>
        <div class="newsletter-section">
            <h2 style="margin: 0 0 10pt 0;">Feature Story</h2>
            <p style="margin: 0;">Article content here.</p>
        </div>
    </div>
</body>
```

### Example 11: Receipt with inline padding

```html
<style>
    .receipt {
        width: 300pt;
        margin: 40pt auto;
        border: 2pt solid #000;
    }
    .receipt-header {
        padding-inline: 20pt;
        padding-block: 20pt;
        background-color: #f9fafb;
        text-align: center;
        border-bottom: 2pt solid #000;
    }
    .receipt-item {
        padding-inline: 25pt 20pt;
        padding-block: 10pt;
        border-bottom: 1pt dashed #d1d5db;
    }
</style>
<body>
    <div class="receipt">
        <div class="receipt-header">
            <h2 style="margin: 0;">Store Name</h2>
        </div>
        <div class="receipt-item">Item 1: $19.99</div>
        <div class="receipt-item">Item 2: $29.99</div>
    </div>
</body>
```

### Example 12: Report with inline padding

```html
<style>
    .report {
        padding-inline: 45pt;
        padding-block: 40pt;
    }
    .report-title {
        margin: 0 0 30pt 0;
        font-size: 24pt;
        text-align: center;
    }
    .report-section {
        padding-inline: 30pt;
        padding-block: 25pt;
        background-color: #f9fafb;
        border-inline-start: 5pt solid #2563eb;
        margin-bottom: 25pt;
    }
</style>
<body>
    <div class="report">
        <h1 class="report-title">Annual Report 2025</h1>
        <div class="report-section">
            <h2 style="margin: 0 0 15pt 0;">Executive Summary</h2>
            <p style="margin: 0;">Key findings and recommendations.</p>
        </div>
    </div>
</body>
```

### Example 13: Business card with inline padding

```html
<style>
    .business-card {
        width: 350pt;
        height: 200pt;
        padding-inline: 30pt;
        padding-block: 25pt;
        border: 3pt solid #1e3a8a;
    }
    .card-content {
        padding-inline: 20pt;
        padding-block: 15pt;
        background-color: #f9fafb;
    }
</style>
<body>
    <div class="business-card">
        <div class="card-content">
            <h2 style="margin: 0 0 8pt 0; color: #1e3a8a;">Robert Taylor</h2>
            <p style="margin: 0;">Chief Executive Officer</p>
        </div>
    </div>
</body>
```

### Example 14: Certificate with inline padding

```html
<style>
    .certificate {
        width: 500pt;
        margin: 50pt auto;
        border: 5pt double #1e3a8a;
    }
    .cert-content {
        padding-inline: 50pt;
        padding-block: 50pt;
        text-align: center;
    }
    .cert-title {
        margin: 0 0 30pt 0;
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
    }
</style>
<body>
    <div class="certificate">
        <div class="cert-content">
            <h1 class="cert-title">Certificate of Achievement</h1>
            <p style="margin: 0;">Awarded to Jennifer Wilson</p>
        </div>
    </div>
</body>
```

### Example 15: Section with inline padding

```html
<style>
    .content-section {
        padding-inline: 40pt 30pt;
        padding-block: 30pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .section-heading {
        margin: 0 0 20pt 0;
        padding-block-end: 10pt;
        border-block-end: 2pt solid #e5e7eb;
        font-size: 20pt;
    }
</style>
<body>
    <div class="content-section">
        <h2 class="section-heading">Section Title</h2>
        <p style="margin: 0;">Content with proper inline padding for optimal layout.</p>
    </div>
</body>
```

---

## See Also

- [padding-inline-start](/reference/cssproperties/css_prop_padding-inline-start) - Set inline start padding
- [padding-inline-end](/reference/cssproperties/css_prop_padding-inline-end) - Set inline end padding
- [padding](/reference/cssproperties/css_prop_padding) - Set all padding shorthand
- [padding-left](/reference/cssproperties/css_prop_padding-left) - Set left padding (physical property)
- [padding-right](/reference/cssproperties/css_prop_padding-right) - Set right padding (physical property)
- [margin-inline](/reference/cssproperties/css_prop_margin-inline) - Set inline margin shorthand
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
