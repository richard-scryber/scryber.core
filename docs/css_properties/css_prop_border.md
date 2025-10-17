---
layout: default
title: border
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border : Border Shorthand Property

The `border` property is a shorthand that sets all border properties (width, style, and color) for all four sides of an element in a single declaration. This property is essential for creating visual boundaries, separating content sections, and enhancing the overall design of PDF documents.

## Usage

```css
selector {
    border: width style color;
}
```

The border property combines border-width, border-style, and border-color into one declaration. Values can be specified in any order.

---

## Supported Values

### Border Width
- **Named values**: `thin`, `medium`, `thick`
- **Length values**: Any valid length unit (e.g., `1pt`, `2px`, `0.5mm`)

### Border Style (Required)
- `none` - No border (default)
- `solid` - Solid line border
- `dashed` - Dashed line border
- `dotted` - Dotted line border
- `double` - Double line border

### Border Color
- **Named colors**: `red`, `blue`, `black`, etc.
- **Hexadecimal**: `#RRGGBB` or `#RGB`
- **RGB/RGBA**: `rgb(r, g, b)` or `rgba(r, g, b, a)`
- **Default**: Uses the element's text color if not specified

---

## Supported Elements

The `border` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables and table cells (`<table>`, `<td>`, `<th>`)
- Images (`<img>`)
- Lists and list items (`<ul>`, `<ol>`, `<li>`)
- All container elements

---

## Notes

- The border style must be specified for the border to be visible
- Border width defaults to `medium` if not specified
- Border color defaults to the element's text color if not specified
- Borders are drawn outside the content area but inside the margin
- The `border` shorthand applies to all four sides equally
- Use individual border properties (`border-top`, `border-right`, etc.) for different sides
- Double borders require sufficient width to display both lines
- Borders affect the total rendered size of elements unless `box-sizing: border-box` is used

---

## Data Binding

The `border` property supports dynamic values through data binding, allowing borders to be customized based on document data at runtime.

### Example 1: Invoice status borders

```html
<style>
    .invoice-card {
        padding: 15pt;
        margin-bottom: 10pt;
        background-color: white;
    }
</style>
<body>
    <div class="invoice-card" style="border: {{invoice.borderWidth}} solid {{invoice.statusColor}}">
        <h3>Invoice #{{invoice.number}}</h3>
        <p>Status: {{invoice.status}}</p>
        <p>Amount: {{invoice.total}}</p>
    </div>
</body>
```

Data context:
```json
{
    "invoice": {
        "number": "INV-2025-001",
        "status": "Paid",
        "statusColor": "#16a34a",
        "borderWidth": "2pt",
        "total": "$1,250.00"
    }
}
```

### Example 2: Conditional border emphasis

```html
<style>
    .order-item {
        padding: 12pt;
        margin-bottom: 8pt;
        background-color: #f9fafb;
    }
</style>
<body>
    <div class="order-item" style="border: {{isHighPriority ? '3pt' : '1pt'}} solid {{isHighPriority ? '#dc2626' : '#d1d5db'}}">
        <p>Order #{{orderNumber}}</p>
        <p>Priority: {{priority}}</p>
    </div>
</body>
```

### Example 3: Alert severity borders

```html
<style>
    .alert-box {
        padding: 12pt;
        margin: 10pt 0;
    }
</style>
<body>
    <div class="alert-box" style="border: 2pt {{alert.style}} {{alert.color}}; background-color: {{alert.bgColor}}">
        <strong>{{alert.title}}:</strong> {{alert.message}}
    </div>
</body>
```

Data context:
```json
{
    "alert": {
        "title": "Warning",
        "message": "Please review your settings",
        "style": "dashed",
        "color": "#f59e0b",
        "bgColor": "#fef3c7"
    }
}
```

---

## Examples

### Example 1: Simple solid border

```html
<style>
    .simple-box {
        border: 1pt solid black;
        padding: 10pt;
    }
</style>
<body>
    <div class="simple-box">
        <p>Content with a simple black border</p>
    </div>
</body>
```

### Example 2: Colored border with custom width

```html
<style>
    .colored-box {
        border: 2pt solid #2563eb;
        padding: 12pt;
        background-color: #eff6ff;
    }
</style>
<body>
    <div class="colored-box">
        <p>Blue bordered box with light blue background</p>
    </div>
</body>
```

### Example 3: Dashed border for emphasis

```html
<style>
    .dashed-notice {
        border: 2pt dashed #f59e0b;
        padding: 15pt;
        background-color: #fffbeb;
    }
</style>
<body>
    <div class="dashed-notice">
        <strong>Notice:</strong> This information requires your attention.
    </div>
</body>
```

### Example 4: Dotted border for callout

```html
<style>
    .dotted-callout {
        border: 1.5pt dotted #8b5cf6;
        padding: 10pt;
        font-style: italic;
    }
</style>
<body>
    <div class="dotted-callout">
        <p>Tip: Use dotted borders for subtle visual separation.</p>
    </div>
</body>
```

### Example 5: Double border for formal documents

```html
<style>
    .certificate-border {
        border: 4pt double #1e293b;
        padding: 30pt;
        text-align: center;
    }
    .cert-title {
        font-size: 24pt;
        font-weight: bold;
        margin-bottom: 15pt;
    }
</style>
<body>
    <div class="certificate-border">
        <h1 class="cert-title">Certificate of Achievement</h1>
        <p>Awarded to John Smith</p>
    </div>
</body>
```

### Example 6: Table with borders

```html
<style>
    .bordered-table {
        width: 100%;
        border: 2pt solid #374151;
        border-collapse: collapse;
    }
    .bordered-table th,
    .bordered-table td {
        border: 1pt solid #d1d5db;
        padding: 8pt;
    }
    .bordered-table th {
        background-color: #f3f4f6;
        font-weight: bold;
    }
</style>
<body>
    <table class="bordered-table">
        <thead>
            <tr>
                <th>Product</th>
                <th>Price</th>
                <th>Quantity</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget A</td>
                <td>$19.99</td>
                <td>5</td>
            </tr>
            <tr>
                <td>Widget B</td>
                <td>$29.99</td>
                <td>3</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 7: Alert box with semantic borders

```html
<style>
    .alert {
        padding: 12pt;
        margin: 10pt 0;
    }
    .alert-success {
        border: 2pt solid #16a34a;
        background-color: #dcfce7;
        color: #166534;
    }
    .alert-warning {
        border: 2pt solid #f59e0b;
        background-color: #fef3c7;
        color: #92400e;
    }
    .alert-error {
        border: 2pt solid #dc2626;
        background-color: #fee2e2;
        color: #991b1b;
    }
</style>
<body>
    <div class="alert alert-success">
        <strong>Success:</strong> Your changes have been saved.
    </div>
    <div class="alert alert-warning">
        <strong>Warning:</strong> Please review your input.
    </div>
    <div class="alert alert-error">
        <strong>Error:</strong> Unable to process request.
    </div>
</body>
```

### Example 8: Invoice header with border

```html
<style>
    .invoice-header {
        border: 3pt solid #1e3a8a;
        background-color: #dbeafe;
        padding: 20pt;
    }
    .invoice-title {
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0;
    }
    .invoice-info {
        font-size: 11pt;
        color: #3b82f6;
        margin-top: 5pt;
    }
</style>
<body>
    <div class="invoice-header">
        <h1 class="invoice-title">INVOICE</h1>
        <p class="invoice-info">Invoice #INV-2025-1234 | Date: October 13, 2025</p>
    </div>
</body>
```

### Example 9: Form field styling

```html
<style>
    .form-group {
        margin-bottom: 15pt;
    }
    .form-label {
        display: block;
        font-weight: bold;
        margin-bottom: 5pt;
    }
    .form-input {
        border: 1pt solid #9ca3af;
        padding: 8pt;
        width: 100%;
        background-color: white;
    }
    .form-input:focus {
        border: 2pt solid #2563eb;
    }
</style>
<body>
    <div class="form-group">
        <label class="form-label">Full Name</label>
        <div class="form-input">John Smith</div>
    </div>
    <div class="form-group">
        <label class="form-label">Email Address</label>
        <div class="form-input">john.smith@example.com</div>
    </div>
</body>
```

### Example 10: Card layout with borders

```html
<style>
    .card {
        border: 1pt solid #e5e7eb;
        background-color: white;
        padding: 15pt;
        margin-bottom: 12pt;
    }
    .card-title {
        font-size: 16pt;
        font-weight: bold;
        color: #1f2937;
        margin: 0 0 8pt 0;
    }
    .card-content {
        color: #6b7280;
        line-height: 1.5;
    }
</style>
<body>
    <div class="card">
        <h3 class="card-title">Product Features</h3>
        <p class="card-content">
            This product includes advanced features designed to improve
            productivity and streamline your workflow.
        </p>
    </div>
    <div class="card">
        <h3 class="card-title">Technical Specifications</h3>
        <p class="card-content">
            Built with modern technology and optimized for performance.
        </p>
    </div>
</body>
```

### Example 11: Quote block with border accent

```html
<style>
    .quote-box {
        border: 4pt solid #6366f1;
        border-left-width: 8pt;
        background-color: #f5f5f5;
        padding: 15pt 15pt 15pt 20pt;
        font-style: italic;
    }
    .quote-text {
        font-size: 14pt;
        color: #374151;
        margin: 0;
    }
    .quote-author {
        font-size: 11pt;
        color: #6b7280;
        margin-top: 10pt;
        text-align: right;
    }
</style>
<body>
    <div class="quote-box">
        <p class="quote-text">
            "Design is not just what it looks like and feels like.
            Design is how it works."
        </p>
        <p class="quote-author">— Steve Jobs</p>
    </div>
</body>
```

### Example 12: Data table with thick borders

```html
<style>
    .data-table {
        width: 100%;
        border: 3pt solid #0f172a;
        border-collapse: collapse;
    }
    .data-table th {
        border: 2pt solid #1e293b;
        background-color: #334155;
        color: white;
        padding: 10pt;
        font-weight: bold;
    }
    .data-table td {
        border: 1pt solid #cbd5e1;
        padding: 8pt;
    }
    .total-row {
        border: 2pt solid #0f172a;
        background-color: #f1f5f9;
        font-weight: bold;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th>Item</th>
                <th>Quantity</th>
                <th>Price</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Product A</td>
                <td>2</td>
                <td>$50.00</td>
                <td>$100.00</td>
            </tr>
            <tr>
                <td>Product B</td>
                <td>1</td>
                <td>$75.00</td>
                <td>$75.00</td>
            </tr>
            <tr class="total-row">
                <td colspan="3">Grand Total</td>
                <td>$175.00</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 13: Certificate with decorative border

```html
<style>
    .certificate {
        border: 6pt double #854d0e;
        padding: 40pt;
        background-color: #fffbeb;
        text-align: center;
    }
    .cert-heading {
        font-size: 32pt;
        font-weight: bold;
        color: #854d0e;
        margin-bottom: 20pt;
    }
    .cert-body {
        font-size: 14pt;
        line-height: 1.8;
        color: #78350f;
    }
    .cert-name {
        font-size: 20pt;
        font-weight: bold;
        color: #92400e;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-heading">CERTIFICATE OF EXCELLENCE</h1>
        <div class="cert-body">
            <p>This certifies that</p>
            <p class="cert-name">Jane Doe</p>
            <p>has successfully completed the Advanced PDF Design course</p>
            <p>on this thirteenth day of October, 2025</p>
        </div>
    </div>
</body>
```

### Example 14: Sidebar with border separation

```html
<style>
    .layout {
        display: table;
        width: 100%;
    }
    .sidebar {
        display: table-cell;
        width: 180pt;
        border: 2pt solid #e5e7eb;
        border-right-width: 3pt;
        background-color: #f9fafb;
        padding: 15pt;
        vertical-align: top;
    }
    .main-content {
        display: table-cell;
        padding: 15pt 20pt;
        vertical-align: top;
    }
    .nav-item {
        border: 1pt solid transparent;
        padding: 8pt;
        margin-bottom: 5pt;
    }
    .nav-item.active {
        border: 1pt solid #2563eb;
        background-color: #eff6ff;
        color: #2563eb;
        font-weight: bold;
    }
</style>
<body>
    <div class="layout">
        <div class="sidebar">
            <h3>Navigation</h3>
            <div class="nav-item active">Dashboard</div>
            <div class="nav-item">Reports</div>
            <div class="nav-item">Settings</div>
        </div>
        <div class="main-content">
            <h1>Dashboard</h1>
            <p>Welcome to your dashboard. View your statistics and recent activity here.</p>
        </div>
    </div>
</body>
```

### Example 15: Pricing table with borders

```html
<style>
    .pricing-table {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 10pt;
    }
    .pricing-card {
        display: table-cell;
        border: 2pt solid #d1d5db;
        padding: 20pt;
        text-align: center;
        vertical-align: top;
    }
    .pricing-card.featured {
        border: 3pt solid #2563eb;
        background-color: #eff6ff;
    }
    .plan-name {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .plan-price {
        font-size: 24pt;
        color: #2563eb;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="pricing-table">
        <div class="pricing-card">
            <div class="plan-name">Basic</div>
            <div class="plan-price">$9.99</div>
            <p>Essential features for small projects</p>
        </div>
        <div class="pricing-card featured">
            <div class="plan-name">Professional</div>
            <div class="plan-price">$29.99</div>
            <p>Advanced features for growing businesses</p>
        </div>
        <div class="pricing-card">
            <div class="plan-name">Enterprise</div>
            <div class="plan-price">$99.99</div>
            <p>Full suite for large organizations</p>
        </div>
    </div>
</body>
```

---

## See Also

- [border-width](/reference/cssproperties/css_prop_border-width) - Set border width
- [border-style](/reference/cssproperties/css_prop_border-style) - Set border style
- [border-color](/reference/cssproperties/css_prop_border-color) - Set border color
- [border-radius](/reference/cssproperties/css_prop_border-radius) - Set rounded corners
- [border-top](/reference/cssproperties/css_prop_border-top) - Set top border
- [border-right](/reference/cssproperties/css_prop_border-right) - Set right border
- [border-bottom](/reference/cssproperties/css_prop_border-bottom) - Set bottom border
- [border-left](/reference/cssproperties/css_prop_border-left) - Set left border
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
