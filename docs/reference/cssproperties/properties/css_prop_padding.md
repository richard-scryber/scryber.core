---
layout: default
title: padding
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# padding : Padding Shorthand Property

The `padding` property is a shorthand for setting all four padding sides (top, right, bottom, left) of an element in PDF documents. Padding creates space inside the element's border, between the border and the content, and inherits the element's background color.

## Usage

```css
selector {
    padding: value;
}
```

The padding property accepts 1-4 space-separated values with the following behavior:
- **1 value**: Applies to all four sides
- **2 values**: First value for top and bottom, second for left and right
- **3 values**: First for top, second for left and right, third for bottom
- **4 values**: Top, right, bottom, left (clockwise from top)

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
- `0` - No padding

---

## Supported Elements

The `padding` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Lists (`<ul>`, `<ol>`)
- List items (`<li>`)
- All container elements

---

## Notes

- Padding inherits the element's background color
- Padding increases the overall size of the element
- Percentage padding is calculated relative to parent element's width (even for top/bottom)
- Padding cannot be negative
- Padding is inside the border, while margin is outside
- Background colors and images extend into padding area
- Padding provides internal spacing that's part of the clickable/interactive area
- Essential for creating visual separation between borders and content

---

## Data Binding

The `padding` property supports dynamic values through data binding, allowing you to create flexible internal spacing that adapts to content density, component size, or user preferences.

### Example 1: Dynamic card padding based on importance

```html
<style>
    .card {
        margin-bottom: 15pt;
        padding: {{card.priority === 'high' ? '25pt' : '15pt'}};
        background-color: {{card.priority === 'high' ? '#fef3c7' : 'white'}};
        border: {{card.priority === 'high' ? '2pt' : '1pt'}} solid #e5e7eb;
    }
</style>
<body>
    <div class="card">
        <h3>{{card.title}}</h3>
        <p>{{card.description}}</p>
    </div>
</body>
```

Data context:
```json
{
    "card": {
        "priority": "high",
        "title": "Important Notice",
        "description": "High priority cards get more padding for emphasis."
    }
}
```

### Example 2: Responsive padding based on content density

```html
<style>
    .invoice {
        padding: {{density === 'compact' ? '30pt' : '40pt'}};
        background-color: white;
    }
    .invoice-header {
        padding: {{density === 'compact' ? '15pt' : '25pt'}};
        background-color: #1e3a8a;
        color: white;
        margin-bottom: 30pt;
    }
    .invoice-section {
        padding: {{density === 'compact' ? '12pt' : '20pt'}};
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1>INVOICE</h1>
            <p>{{invoice.number}}</p>
        </div>
        <div class="invoice-section">
            <h3>Bill To</h3>
            <p>{{invoice.customer}}</p>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "density": "normal",
    "invoice": {
        "number": "INV-2025-001",
        "customer": "Acme Corporation"
    }
}
```

### Example 3: Variable padding for form fields

```html
<style>
    .form {
        padding: {{form.outerPadding}}pt;
        background-color: #f9fafb;
    }
    .form-group {
        padding: {{form.groupPadding}}pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .form-input {
        padding: {{form.inputPadding}}pt;
        border: 1pt solid #d1d5db;
        width: 100%;
    }
</style>
<body>
    <div class="form">
        <div class="form-group">
            <label>Full Name</label>
            <input class="form-input" type="text" />
        </div>
        <div class="form-group">
            <label>Email</label>
            <input class="form-input" type="email" />
        </div>
    </div>
</body>
```

Data context:
```json
{
    "form": {
        "outerPadding": 30,
        "groupPadding": 15,
        "inputPadding": 10
    }
}
```

---

## Examples

### Example 1: Uniform padding on all sides

```html
<style>
    .box {
        padding: 20pt;
        background-color: #dbeafe;
        border: 2pt solid #3b82f6;
    }
</style>
<body>
    <div class="box">
        <p>This box has 20pt padding on all sides.</p>
    </div>
</body>
```

### Example 2: Vertical and horizontal padding

```html
<style>
    .card {
        padding: 25pt 15pt;
        background-color: #f3f4f6;
        border: 1pt solid #d1d5db;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="card">
        <h3>Card Title</h3>
        <p>25pt top/bottom padding, 15pt left/right padding.</p>
    </div>
</body>
```

### Example 3: Three-value padding syntax

```html
<style>
    .notice {
        padding: 15pt 20pt 25pt;
        background-color: #fef3c7;
        border: 2pt solid #f59e0b;
    }
</style>
<body>
    <div class="notice">
        <p>15pt top, 20pt left/right, 25pt bottom padding.</p>
    </div>
</body>
```

### Example 4: Four-value padding syntax

```html
<style>
    .custom-box {
        padding: 10pt 15pt 20pt 25pt;
        background-color: #dcfce7;
        border: 2pt solid #16a34a;
    }
</style>
<body>
    <div class="custom-box">
        <p>Clockwise padding: 10pt top, 15pt right, 20pt bottom, 25pt left.</p>
    </div>
</body>
```

### Example 5: Document layout with consistent padding

```html
<style>
    .document {
        padding: 50pt 40pt;
        background-color: white;
    }
    .document-header {
        padding: 20pt;
        background-color: #1e3a8a;
        color: white;
        margin-bottom: 20pt;
    }
    .document-section {
        padding: 15pt;
        background-color: #f9fafb;
        margin-bottom: 15pt;
    }
</style>
<body>
    <div class="document">
        <div class="document-header">
            <h1>Document Title</h1>
        </div>
        <div class="document-section">
            <h2>Section One</h2>
            <p>Section content with proper padding.</p>
        </div>
    </div>
</body>
```

### Example 6: Invoice with padding

```html
<style>
    .invoice {
        padding: 40pt;
        background-color: white;
    }
    .invoice-header {
        padding: 25pt;
        background-color: #1e3a8a;
        color: white;
        margin-bottom: 30pt;
    }
    .invoice-section {
        padding: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .line-item {
        padding: 10pt;
        border-bottom: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1>INVOICE</h1>
            <p>INV-2025-001</p>
        </div>
        <div class="invoice-section">
            <h3>Bill To</h3>
            <p>Acme Corporation</p>
        </div>
        <div class="invoice-section">
            <div class="line-item">Consulting Services - $1,500.00</div>
            <div class="line-item">Software License - $299.00</div>
        </div>
    </div>
</body>
```

### Example 7: Card grid with padding

```html
<style>
    .card-container {
        padding: 20pt;
    }
    .card {
        padding: 20pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .card-header {
        padding: 12pt;
        background-color: #f3f4f6;
        margin: -20pt -20pt 15pt -20pt;
        border-bottom: 1pt solid #e5e7eb;
    }
    .card-title {
        margin: 0;
        font-size: 14pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="card-container">
        <div class="card">
            <div class="card-header">
                <h3 class="card-title">Card Title</h3>
            </div>
            <p>Card content with proper padding for readability.</p>
        </div>
    </div>
</body>
```

### Example 8: Form with padding

```html
<style>
    .form {
        padding: 30pt;
        background-color: #f9fafb;
    }
    .form-group {
        padding: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .form-label {
        padding: 5pt 0;
        font-weight: bold;
        display: block;
    }
    .form-input {
        padding: 10pt;
        border: 1pt solid #d1d5db;
        width: 100%;
    }
    .form-button {
        padding: 12pt 30pt;
        background-color: #2563eb;
        color: white;
        border: none;
    }
</style>
<body>
    <div class="form">
        <div class="form-group">
            <label class="form-label">Full Name</label>
            <input class="form-input" type="text" />
        </div>
        <div class="form-group">
            <label class="form-label">Email</label>
            <input class="form-input" type="email" />
        </div>
        <button class="form-button">Submit</button>
    </div>
</body>
```

### Example 9: Table with padding

```html
<style>
    .report-table {
        width: 100%;
        border-collapse: separate;
        border-spacing: 0;
    }
    .report-table th {
        padding: 12pt 15pt;
        background-color: #1f2937;
        color: white;
        text-align: left;
    }
    .report-table td {
        padding: 10pt 15pt;
        border: 1pt solid #d1d5db;
        background-color: white;
    }
    .report-table tr:nth-child(even) td {
        background-color: #f9fafb;
    }
</style>
<body>
    <table class="report-table">
        <thead>
            <tr>
                <th>Product</th>
                <th>Quantity</th>
                <th>Price</th>
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
</body>
```

### Example 10: Alert boxes with padding

```html
<style>
    .alert {
        padding: 15pt 15pt 15pt 45pt;
        margin-bottom: 12pt;
        border-radius: 4pt;
        border-left: 4pt solid;
    }
    .alert-info {
        background-color: #dbeafe;
        border-left-color: #3b82f6;
    }
    .alert-success {
        background-color: #dcfce7;
        border-left-color: #16a34a;
    }
    .alert-warning {
        background-color: #fef3c7;
        border-left-color: #f59e0b;
    }
    .alert-error {
        background-color: #fee2e2;
        border-left-color: #dc2626;
    }
</style>
<body>
    <div class="alert alert-info">
        <strong>Info:</strong> System update scheduled.
    </div>
    <div class="alert alert-success">
        <strong>Success:</strong> Changes saved successfully.
    </div>
    <div class="alert alert-warning">
        <strong>Warning:</strong> Password expires soon.
    </div>
</body>
```

### Example 11: Quote block with padding

```html
<style>
    .content {
        padding: 40pt;
    }
    .quote-block {
        padding: 20pt 20pt 20pt 30pt;
        background-color: #f5f5f5;
        border-left: 5pt solid #6366f1;
        font-style: italic;
        margin: 20pt 0;
    }
    .quote-text {
        padding: 0;
        margin: 0 0 10pt 0;
        font-size: 14pt;
    }
    .quote-author {
        padding: 5pt 0 0 20pt;
        margin: 0;
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="content">
        <p>Regular content text.</p>
        <div class="quote-block">
            <p class="quote-text">"Design is not just what it looks like. Design is how it works."</p>
            <p class="quote-author">— Steve Jobs</p>
        </div>
        <p>Content continues.</p>
    </div>
</body>
```

### Example 12: Product catalog with padding

```html
<style>
    .catalog {
        padding: 30pt;
    }
    .catalog-header {
        padding: 25pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
        margin-bottom: 30pt;
    }
    .product {
        padding: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .product-name {
        padding: 0 0 10pt 0;
        margin: 0;
        font-size: 16pt;
        font-weight: bold;
        border-bottom: 1pt solid #d1d5db;
    }
    .product-description {
        padding: 10pt 0;
        margin: 0;
        color: #6b7280;
    }
    .product-price {
        padding: 10pt 15pt;
        margin: 10pt 0 0 0;
        background-color: #dcfce7;
        color: #16a34a;
        font-size: 18pt;
        font-weight: bold;
        display: inline-block;
    }
</style>
<body>
    <div class="catalog">
        <div class="catalog-header">
            <h1>Product Catalog 2025</h1>
        </div>
        <div class="product">
            <h3 class="product-name">Premium Widget</h3>
            <p class="product-description">High-quality professional widget.</p>
            <div class="product-price">$99.99</div>
        </div>
    </div>
</body>
```

### Example 13: Receipt with padding

```html
<style>
    .receipt {
        width: 300pt;
        padding: 25pt;
        margin: 40pt auto;
        border: 3pt solid #000;
        background-color: white;
    }
    .receipt-header {
        padding: 15pt;
        background-color: #f9fafb;
        text-align: center;
        margin-bottom: 20pt;
        border-bottom: 2pt solid #000;
    }
    .receipt-item {
        padding: 8pt 0;
        border-bottom: 1pt dashed #d1d5db;
    }
    .receipt-total {
        padding: 15pt 0 0 0;
        margin-top: 15pt;
        border-top: 3pt double #000;
        font-weight: bold;
        font-size: 16pt;
    }
</style>
<body>
    <div class="receipt">
        <div class="receipt-header">
            <h2>Store Name</h2>
            <p>Receipt #12345</p>
        </div>
        <div class="receipt-item">Item 1: $19.99</div>
        <div class="receipt-item">Item 2: $29.99</div>
        <div class="receipt-item">Item 3: $39.99</div>
        <div class="receipt-total">Total: $89.97</div>
    </div>
</body>
```

### Example 14: Newsletter sections with padding

```html
<style>
    .newsletter {
        padding: 35pt;
        background-color: #f9fafb;
    }
    .newsletter-header {
        padding: 30pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
        margin-bottom: 30pt;
    }
    .newsletter-section {
        padding: 25pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 20pt;
    }
    .section-title {
        padding: 0 0 12pt 0;
        margin: 0 0 15pt 0;
        border-bottom: 2pt solid #e5e7eb;
        font-size: 18pt;
        font-weight: bold;
    }
    .section-content {
        padding: 0;
        line-height: 1.6;
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
            <div class="section-content">
                <p>Main article content with proper padding for readability.</p>
            </div>
        </div>
        <div class="newsletter-section">
            <h2 class="section-title">Company Updates</h2>
            <div class="section-content">
                <p>Latest news and announcements.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 15: Business card with padding

```html
<style>
    .business-card {
        width: 350pt;
        height: 200pt;
        padding: 25pt;
        margin: 40pt auto;
        border: 3pt solid #1e3a8a;
        background-color: white;
    }
    .card-header {
        padding: 0 0 15pt 0;
        margin-bottom: 15pt;
        border-bottom: 2pt solid #e5e7eb;
    }
    .card-name {
        padding: 0;
        margin: 0 0 5pt 0;
        font-size: 20pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .card-title {
        padding: 0;
        margin: 0;
        font-size: 12pt;
        color: #6b7280;
    }
    .card-contact {
        padding: 12pt;
        background-color: #f9fafb;
        font-size: 10pt;
        line-height: 1.5;
    }
</style>
<body>
    <div class="business-card">
        <div class="card-header">
            <h2 class="card-name">Jennifer Wilson</h2>
            <p class="card-title">Chief Marketing Officer</p>
        </div>
        <div class="card-contact">
            jennifer.wilson@example.com<br/>
            +1 (555) 246-8101<br/>
            LinkedIn: /in/jenniferwilson
        </div>
    </div>
</body>
```

---

## See Also

- [padding-top](/reference/cssproperties/css_prop_padding-top) - Set top padding
- [padding-right](/reference/cssproperties/css_prop_padding-right) - Set right padding
- [padding-bottom](/reference/cssproperties/css_prop_padding-bottom) - Set bottom padding
- [padding-left](/reference/cssproperties/css_prop_padding-left) - Set left padding
- [padding-inline](/reference/cssproperties/css_prop_padding-inline) - Set inline padding shorthand
- [margin](/reference/cssproperties/css_prop_margin) - Set margin shorthand property
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
