---
layout: default
title: margin
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# margin : Margin Shorthand Property

The `margin` property is a shorthand for setting all four margin sides (top, right, bottom, left) of an element in PDF documents. Margins create space outside the element's border, separating it from other elements and controlling document layout and white space.

## Usage

```css
selector {
    margin: value;
}
```

The margin property accepts 1-4 space-separated values with the following behavior:
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
- `0` - No margin
- `auto` - Automatic margin (useful for centering)

---

## Supported Elements

The `margin` property can be applied to:
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

- Margins are transparent and do not have background colors
- Vertical margins collapse between adjacent elements (the larger margin wins)
- Negative margins are supported and pull elements closer together
- Percentage margins are calculated relative to the parent element's width (even for top/bottom)
- Margins push elements away from their neighbors
- The `auto` value can be used for horizontal centering of block elements with defined width
- Margins do not affect the element's actual size, only its position and spacing

---

## Data Binding

The `margin` property supports dynamic values through data binding, allowing you to create flexible spacing that adapts to document context, user preferences, or data-driven requirements.

### Example 1: Dynamic invoice spacing based on document type

```html
<style>
    .invoice-container {
        margin: {{layout.margin}}pt;
    }
    .invoice-header {
        margin: 0 0 {{layout.sectionSpacing}}pt 0;
        background-color: #1e3a8a;
        color: white;
        padding: 20pt;
    }
</style>
<body>
    <div class="invoice-container">
        <div class="invoice-header">
            <h1>INVOICE</h1>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "layout": {
        "margin": 40,
        "sectionSpacing": 30
    }
}
```

### Example 2: Conditional spacing for compact vs. normal mode

```html
<style>
    .document {
        margin: {{preferences.compact ? '20pt' : '50pt'}};
    }
    .section {
        margin: 0 0 {{preferences.compact ? '10pt' : '20pt'}} 0;
        padding: 15pt;
        background-color: #f3f4f6;
    }
</style>
<body>
    <div class="document">
        <div class="section">
            <h2>Section Title</h2>
            <p>Content with dynamic spacing based on density preference.</p>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "preferences": {
        "compact": false
    }
}
```

### Example 3: Responsive margins based on page size

```html
<style>
    .report-container {
        margin: {{page.size === 'A4' ? '50pt 40pt' : '72pt 54pt'}};
    }
    .report-section {
        margin: 0 0 {{spacing.betweenSections}}pt 0;
    }
</style>
<body>
    <div class="report-container">
        <div class="report-section">
            <h2>Report Section</h2>
            <p>Margins adjust automatically based on page size.</p>
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
    "spacing": {
        "betweenSections": 25
    }
}
```

---

## Examples

### Example 1: Uniform margin on all sides

```html
<style>
    .card {
        margin: 20pt;
        padding: 15pt;
        background-color: #f3f4f6;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="card">
        <h3>Card with Uniform Margin</h3>
        <p>This card has 20pt margin on all sides.</p>
    </div>
</body>
```

### Example 2: Vertical and horizontal margins

```html
<style>
    .section {
        margin: 30pt 15pt;
        padding: 12pt;
        background-color: #dbeafe;
    }
</style>
<body>
    <div class="section">
        <h2>Section Title</h2>
        <p>30pt top/bottom margin, 15pt left/right margin.</p>
    </div>
</body>
```

### Example 3: Three-value margin syntax

```html
<style>
    .notice {
        margin: 10pt 20pt 30pt;
        padding: 10pt;
        background-color: #fef3c7;
        border: 1pt solid #f59e0b;
    }
</style>
<body>
    <div class="notice">
        <p>10pt top, 20pt left/right, 30pt bottom margin.</p>
    </div>
</body>
```

### Example 4: Four-value margin syntax

```html
<style>
    .custom-box {
        margin: 10pt 15pt 20pt 25pt;
        padding: 12pt;
        background-color: #dcfce7;
        border: 1pt solid #16a34a;
    }
</style>
<body>
    <div class="custom-box">
        <p>Clockwise margins: 10pt top, 15pt right, 20pt bottom, 25pt left.</p>
    </div>
</body>
```

### Example 5: Document layout with consistent spacing

```html
<style>
    .document {
        margin: 50pt 40pt;
    }
    .document h1 {
        margin: 0 0 20pt 0;
        font-size: 24pt;
    }
    .document p {
        margin: 0 0 12pt 0;
        line-height: 1.5;
    }
</style>
<body>
    <div class="document">
        <h1>Document Title</h1>
        <p>First paragraph with bottom margin for spacing.</p>
        <p>Second paragraph also with bottom margin.</p>
        <p>Consistent spacing throughout the document.</p>
    </div>
</body>
```

### Example 6: Invoice header with margins

```html
<style>
    .invoice-container {
        margin: 40pt;
    }
    .invoice-header {
        margin: 0 0 30pt 0;
        background-color: #1e3a8a;
        color: white;
        padding: 20pt;
    }
    .invoice-details {
        margin: 0 0 20pt 0;
    }
    .invoice-table {
        margin: 20pt 0;
        width: 100%;
    }
</style>
<body>
    <div class="invoice-container">
        <div class="invoice-header">
            <h1>INVOICE</h1>
            <p>Invoice #INV-2025-001</p>
        </div>
        <div class="invoice-details">
            <p><strong>Bill To:</strong> Acme Corporation</p>
            <p><strong>Date:</strong> January 15, 2025</p>
        </div>
        <table class="invoice-table">
            <tr><td>Item</td><td>Amount</td></tr>
        </table>
    </div>
</body>
```

### Example 7: Card grid with spacing

```html
<style>
    .card-container {
        margin: 20pt;
    }
    .card {
        margin: 0 0 15pt 0;
        padding: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .card h3 {
        margin: 0 0 10pt 0;
        color: #1f2937;
    }
    .card p {
        margin: 0;
        color: #6b7280;
    }
</style>
<body>
    <div class="card-container">
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

### Example 8: Form layout with margins

```html
<style>
    .form-container {
        margin: 30pt;
    }
    .form-group {
        margin: 0 0 15pt 0;
    }
    .form-label {
        margin: 0 0 5pt 0;
        font-weight: bold;
        display: block;
    }
    .form-input {
        margin: 0;
        padding: 8pt;
        border: 1pt solid #d1d5db;
        width: 100%;
    }
    .form-button {
        margin: 20pt 0 0 0;
        padding: 10pt 20pt;
        background-color: #2563eb;
        color: white;
    }
</style>
<body>
    <div class="form-container">
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

### Example 9: Table with margin spacing

```html
<style>
    .report-table {
        margin: 20pt auto;
        width: 90%;
        border-collapse: collapse;
    }
    .report-table th {
        margin: 0;
        padding: 10pt;
        background-color: #1f2937;
        color: white;
    }
    .report-table td {
        margin: 0;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .table-caption {
        margin: 0 0 10pt 0;
        font-weight: bold;
        font-size: 14pt;
    }
</style>
<body>
    <div class="table-caption">Monthly Sales Report</div>
    <table class="report-table">
        <thead>
            <tr>
                <th>Month</th>
                <th>Revenue</th>
                <th>Growth</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>January</td>
                <td>$125,000</td>
                <td>+12%</td>
            </tr>
            <tr>
                <td>February</td>
                <td>$138,000</td>
                <td>+10%</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 10: Sidebar layout with margins

```html
<style>
    .page-container {
        margin: 30pt;
    }
    .sidebar {
        margin: 0 20pt 0 0;
        padding: 15pt;
        background-color: #f3f4f6;
        width: 150pt;
        float: left;
    }
    .main-content {
        margin: 0 0 0 190pt;
    }
    .sidebar h3 {
        margin: 0 0 15pt 0;
    }
    .sidebar ul {
        margin: 0;
        padding: 0 0 0 15pt;
    }
</style>
<body>
    <div class="page-container">
        <div class="sidebar">
            <h3>Navigation</h3>
            <ul>
                <li>Home</li>
                <li>About</li>
                <li>Services</li>
            </ul>
        </div>
        <div class="main-content">
            <h1>Main Content Area</h1>
            <p>Content with proper margin spacing.</p>
        </div>
    </div>
</body>
```

### Example 11: Alert boxes with varied margins

```html
<style>
    .alerts-container {
        margin: 25pt;
    }
    .alert {
        margin: 0 0 12pt 0;
        padding: 12pt;
        border-radius: 4pt;
    }
    .alert-info {
        margin: 0 0 15pt 0;
        background-color: #dbeafe;
        border: 1pt solid #3b82f6;
    }
    .alert-success {
        margin: 0 0 15pt 0;
        background-color: #dcfce7;
        border: 1pt solid #16a34a;
    }
    .alert-warning {
        margin: 0 0 15pt 0;
        background-color: #fef3c7;
        border: 1pt solid #f59e0b;
    }
</style>
<body>
    <div class="alerts-container">
        <div class="alert alert-info">
            <strong>Info:</strong> System update available.
        </div>
        <div class="alert alert-success">
            <strong>Success:</strong> Profile updated successfully.
        </div>
        <div class="alert alert-warning">
            <strong>Warning:</strong> Password expires in 7 days.
        </div>
    </div>
</body>
```

### Example 12: Quote block with asymmetric margins

```html
<style>
    .article {
        margin: 40pt 50pt;
    }
    .article p {
        margin: 0 0 12pt 0;
        line-height: 1.6;
    }
    .quote-block {
        margin: 20pt 40pt 20pt 20pt;
        padding: 15pt 15pt 15pt 20pt;
        background-color: #f5f5f5;
        border-left: 4pt solid #6366f1;
        font-style: italic;
    }
    .quote-author {
        margin: 8pt 0 0 0;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="article">
        <p>Regular article text with standard margins.</p>
        <div class="quote-block">
            <p>"Design is not just what it looks like and feels like. Design is how it works."</p>
            <p class="quote-author">— Steve Jobs</p>
        </div>
        <p>Article continues after the quote.</p>
    </div>
</body>
```

### Example 13: Multi-column layout with margins

```html
<style>
    .newsletter {
        margin: 30pt;
    }
    .newsletter-header {
        margin: 0 0 25pt 0;
        padding: 15pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
    .column {
        margin: 0 15pt 0 0;
        width: 48%;
        float: left;
    }
    .column:last-child {
        margin: 0;
    }
    .column h2 {
        margin: 0 0 10pt 0;
    }
    .column p {
        margin: 0 0 10pt 0;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>Monthly Newsletter</h1>
        </div>
        <div class="column">
            <h2>Column One</h2>
            <p>First column content with proper margin spacing.</p>
        </div>
        <div class="column">
            <h2>Column Two</h2>
            <p>Second column content with proper margin spacing.</p>
        </div>
    </div>
</body>
```

### Example 14: Receipt layout with margins

```html
<style>
    .receipt {
        margin: 40pt auto;
        width: 300pt;
        padding: 20pt;
        border: 2pt solid #000;
    }
    .receipt-header {
        margin: 0 0 20pt 0;
        text-align: center;
        border-bottom: 1pt solid #000;
        padding-bottom: 10pt;
    }
    .receipt-item {
        margin: 0 0 8pt 0;
        display: flex;
        justify-content: space-between;
    }
    .receipt-total {
        margin: 15pt 0 0 0;
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
        <div class="receipt-item">
            <span>Item 1</span>
            <span>$19.99</span>
        </div>
        <div class="receipt-item">
            <span>Item 2</span>
            <span>$29.99</span>
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

### Example 15: Product catalog with margins

```html
<style>
    .catalog {
        margin: 25pt;
    }
    .catalog-title {
        margin: 0 0 30pt 0;
        font-size: 20pt;
        text-align: center;
    }
    .product {
        margin: 0 0 20pt 0;
        padding: 15pt;
        border: 1pt solid #e5e7eb;
        background-color: #f9fafb;
    }
    .product-name {
        margin: 0 0 8pt 0;
        font-size: 14pt;
        font-weight: bold;
    }
    .product-description {
        margin: 0 0 10pt 0;
        color: #6b7280;
    }
    .product-price {
        margin: 0;
        font-size: 16pt;
        color: #16a34a;
        font-weight: bold;
    }
</style>
<body>
    <div class="catalog">
        <h1 class="catalog-title">Product Catalog</h1>
        <div class="product">
            <div class="product-name">Premium Widget</div>
            <div class="product-description">High-quality widget for all your needs.</div>
            <div class="product-price">$99.99</div>
        </div>
        <div class="product">
            <div class="product-name">Standard Widget</div>
            <div class="product-description">Reliable widget at an affordable price.</div>
            <div class="product-price">$49.99</div>
        </div>
    </div>
</body>
```

---

## See Also

- [margin-top](/reference/cssproperties/css_prop_margin-top) - Set top margin
- [margin-right](/reference/cssproperties/css_prop_margin-right) - Set right margin
- [margin-bottom](/reference/cssproperties/css_prop_margin-bottom) - Set bottom margin
- [margin-left](/reference/cssproperties/css_prop_margin-left) - Set left margin
- [margin-inline](/reference/cssproperties/css_prop_margin-inline) - Set inline margins (start and end)
- [padding](/reference/cssproperties/css_prop_padding) - Set padding shorthand property
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
