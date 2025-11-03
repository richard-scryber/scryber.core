---
layout: default
title: border-color
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-color : Border Color Property

The `border-color` property sets the color of all four borders of an element. This property controls the visual appearance of borders and can be specified using various color formats including named colors, hexadecimal notation, and RGB/RGBA functions.

## Usage

```css
selector {
    border-color: value;
}
```

The border-color property accepts one to four color values to set colors for individual sides independently.

---

## Supported Values

### Named Colors
Standard CSS color names such as `red`, `blue`, `green`, `black`, `gray`, etc.

### Hexadecimal Colors
- Short form: `#RGB` (e.g., `#f00` for red)
- Long form: `#RRGGBB` (e.g., `#ff0000` for red)

### RGB/RGBA Functions
- RGB: `rgb(red, green, blue)` where values are 0-255
- RGBA: `rgba(red, green, blue, alpha)` where alpha is 0.0-1.0 for transparency

### Multiple Values
- **One value**: Applies to all four sides
- **Two values**: First for top/bottom, second for left/right
- **Three values**: First for top, second for left/right, third for bottom
- **Four values**: Top, right, bottom, left (clockwise)

---

## Supported Elements

The `border-color` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables and table cells (`<table>`, `<td>`, `<th>`)
- Images (`<img>`)
- Lists and list items (`<ul>`, `<ol>`, `<li>`)
- All container elements

---

## Notes

- Border color has no effect unless `border-style` is set to a value other than `none`
- If not specified, border-color defaults to the element's text color
- RGBA colors allow semi-transparent borders
- Different sides can have different colors using multiple values
- Hexadecimal colors are case-insensitive
- Border colors are rendered with full PDF color fidelity
- Transparent borders can be created using `rgba(0,0,0,0)` or the `transparent` keyword

---

## Data Binding

The `border-color` property supports dynamic values through data binding, allowing border colors to be customized based on document data at runtime.

### Example 1: Status-based border colors

```html
<style>
    .order-card {
        border-width: 2pt;
        border-style: solid;
        padding: 15pt;
        margin-bottom: 10pt;
        background-color: white;
    }
</style>
<body>
    <div class="order-card" style="border-color: {{order.statusColor}}">
        <h3>Order #{{order.number}}</h3>
        <p>Status: {{order.status}}</p>
        <p>Total: {{order.total}}</p>
    </div>
</body>
```

Data context:
```json
{
    "order": {
        "number": "ORD-2025-456",
        "status": "Shipped",
        "statusColor": "#2563eb",
        "total": "$345.00"
    }
}
```

### Example 2: Alert severity colors

```html
<style>
    .alert-box {
        border-width: 1pt 1pt 1pt 4pt;
        border-style: solid;
        padding: 12pt;
        margin: 10pt 0;
    }
</style>
<body>
    <div class="alert-box" style="border-color: {{severity.color}}; background-color: {{severity.bgColor}}">
        <strong>{{severity.level}}:</strong> {{message}}
    </div>
</body>
```

Data context:
```json
{
    "severity": {
        "level": "Error",
        "color": "#dc2626",
        "bgColor": "#fee2e2"
    },
    "message": "Unable to process your request"
}
```

### Example 3: Table rows with conditional colors

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-row {
        border-width: 1pt;
        border-style: solid;
        padding: 8pt;
    }
</style>
<body>
    <table class="data-table">
        <tr class="data-row" style="border-color: {{row.isOverdue ? '#dc2626' : '#d1d5db'}}; background-color: {{row.isOverdue ? '#fee2e2' : 'white'}}">
            <td>{{row.taskName}}</td>
            <td>{{row.dueDate}}</td>
        </tr>
    </table>
</body>
```

---

## Examples

### Example 1: Single color for all borders

```html
<style>
    .simple-box {
        border-width: 2pt;
        border-style: solid;
        border-color: #2563eb;
        padding: 12pt;
    }
</style>
<body>
    <div class="simple-box">
        <p>Blue border on all sides</p>
    </div>
</body>
```

### Example 2: Named colors

```html
<style>
    .colored-boxes div {
        border-width: 2pt;
        border-style: solid;
        padding: 10pt;
        margin-bottom: 10pt;
    }
    .red-border {
        border-color: red;
        background-color: #fee2e2;
    }
    .green-border {
        border-color: green;
        background-color: #dcfce7;
    }
    .blue-border {
        border-color: blue;
        background-color: #dbeafe;
    }
</style>
<body>
    <div class="colored-boxes">
        <div class="red-border">Red border</div>
        <div class="green-border">Green border</div>
        <div class="blue-border">Blue border</div>
    </div>
</body>
```

### Example 3: Different colors for vertical and horizontal

```html
<style>
    .two-tone-box {
        border-width: 3pt;
        border-style: solid;
        border-color: #2563eb #16a34a;
        padding: 15pt;
    }
</style>
<body>
    <div class="two-tone-box">
        <p>Blue top and bottom, green left and right</p>
    </div>
</body>
```

### Example 4: Three color values

```html
<style>
    .gradient-border {
        border-width: 2pt;
        border-style: solid;
        border-color: #dc2626 #f59e0b #16a34a;
        padding: 12pt;
    }
</style>
<body>
    <div class="gradient-border">
        <p>Red top, orange left/right, green bottom</p>
    </div>
</body>
```

### Example 5: Individual color per side

```html
<style>
    .rainbow-box {
        border-width: 4pt;
        border-style: solid;
        border-color: #ef4444 #f59e0b #10b981 #3b82f6;
        padding: 15pt;
        background-color: #f9fafb;
    }
</style>
<body>
    <div class="rainbow-box">
        <p>Each side has a different color: red top, orange right, green bottom, blue left</p>
    </div>
</body>
```

### Example 6: Alert boxes with semantic colors

```html
<style>
    .alert {
        border-width: 1pt 1pt 1pt 4pt;
        border-style: solid;
        padding: 12pt;
        margin: 10pt 0;
    }
    .alert-success {
        border-color: #16a34a;
        background-color: #dcfce7;
        color: #166534;
    }
    .alert-warning {
        border-color: #f59e0b;
        background-color: #fef3c7;
        color: #92400e;
    }
    .alert-error {
        border-color: #dc2626;
        background-color: #fee2e2;
        color: #991b1b;
    }
    .alert-info {
        border-color: #2563eb;
        background-color: #eff6ff;
        color: #1e40af;
    }
</style>
<body>
    <div class="alert alert-success">
        <strong>Success:</strong> Operation completed successfully.
    </div>
    <div class="alert alert-warning">
        <strong>Warning:</strong> Please review your input.
    </div>
    <div class="alert alert-error">
        <strong>Error:</strong> Unable to process request.
    </div>
    <div class="alert alert-info">
        <strong>Info:</strong> Additional information available.
    </div>
</body>
```

### Example 7: Table with colored borders

```html
<style>
    .colored-table {
        width: 100%;
        border-width: 3pt;
        border-style: solid;
        border-color: #1e293b;
        border-collapse: collapse;
    }
    .colored-table th {
        border-width: 0 0 2pt 0;
        border-style: solid;
        border-color: #3b82f6;
        padding: 10pt;
        background-color: #eff6ff;
        font-weight: bold;
        color: #1e40af;
    }
    .colored-table td {
        border-width: 0 0 1pt 0;
        border-style: solid;
        border-color: #cbd5e1;
        padding: 8pt;
    }
</style>
<body>
    <table class="colored-table">
        <thead>
            <tr>
                <th>Product</th>
                <th>Category</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget A</td>
                <td>Hardware</td>
                <td>$29.99</td>
            </tr>
            <tr>
                <td>Service B</td>
                <td>Software</td>
                <td>$49.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 8: Form fields with focus colors

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
        border-width: 1pt;
        border-style: solid;
        border-color: #9ca3af;
        padding: 8pt;
        width: 100%;
        background-color: white;
    }
    .form-input.focus {
        border-width: 2pt;
        border-color: #2563eb;
    }
    .form-input.error {
        border-width: 2pt;
        border-color: #dc2626;
        background-color: #fef2f2;
    }
</style>
<body>
    <div class="form-group">
        <label class="form-label">Username</label>
        <div class="form-input">johndoe</div>
    </div>
    <div class="form-group">
        <label class="form-label">Email</label>
        <div class="form-input focus">john@example.com</div>
    </div>
    <div class="form-group">
        <label class="form-label">Password</label>
        <div class="form-input error">••••••••</div>
    </div>
</body>
```

### Example 9: Certificate with gold border

```html
<style>
    .certificate {
        border-width: 6pt;
        border-style: double;
        border-color: #b45309;
        padding: 40pt;
        background-color: #fffbeb;
        text-align: center;
    }
    .cert-title {
        font-size: 28pt;
        font-weight: bold;
        color: #92400e;
        margin-bottom: 20pt;
    }
    .cert-name {
        font-size: 20pt;
        border-width: 0 0 2pt 0;
        border-style: solid;
        border-color: #d97706;
        padding-bottom: 10pt;
        display: inline-block;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">CERTIFICATE OF COMPLETION</h1>
        <p>This is to certify that</p>
        <div class="cert-name">Emily Rodriguez</div>
        <p>has successfully completed the program</p>
    </div>
</body>
```

### Example 10: Card layout with brand colors

```html
<style>
    .card-grid {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 12pt;
    }
    .card {
        display: table-cell;
        border-width: 2pt;
        border-style: solid;
        padding: 15pt;
        vertical-align: top;
    }
    .card-blue {
        border-color: #2563eb;
        background-color: #eff6ff;
    }
    .card-green {
        border-color: #16a34a;
        background-color: #dcfce7;
    }
    .card-purple {
        border-color: #7c3aed;
        background-color: #f5f3ff;
    }
    .card-title {
        font-weight: bold;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="card-grid">
        <div class="card card-blue">
            <div class="card-title">Analytics</div>
            <p>Track your metrics</p>
        </div>
        <div class="card card-green">
            <div class="card-title">Reports</div>
            <p>Generate insights</p>
        </div>
        <div class="card card-purple">
            <div class="card-title">Settings</div>
            <p>Configure options</p>
        </div>
    </div>
</body>
```

### Example 11: Invoice with accent borders

```html
<style>
    .invoice-header {
        border-width: 0 0 3pt 0;
        border-style: solid;
        border-color: #1e3a8a;
        padding-bottom: 15pt;
        margin-bottom: 20pt;
    }
    .invoice-title {
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .invoice-section {
        border-width: 1pt 0;
        border-style: solid;
        border-color: #cbd5e1;
        padding: 12pt 0;
        margin: 10pt 0;
    }
    .invoice-total {
        border-width: 3pt 0 3pt 0;
        border-style: double;
        border-color: #0f172a;
        padding: 15pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="invoice-header">
        <h1 class="invoice-title">INVOICE</h1>
        <p>Invoice #INV-2025-5678</p>
    </div>
    <div class="invoice-section">
        <p>Product A: $150.00</p>
        <p>Product B: $225.00</p>
    </div>
    <div class="invoice-total">
        <p>Total Amount Due: $375.00</p>
    </div>
</body>
```

### Example 12: Callout with RGBA transparency

```html
<style>
    .callout {
        border-width: 0 0 0 5pt;
        border-style: solid;
        border-color: rgba(37, 99, 235, 0.7);
        background-color: rgba(239, 246, 255, 0.5);
        padding: 15pt 15pt 15pt 20pt;
    }
    .callout-title {
        font-size: 14pt;
        font-weight: bold;
        color: #1e40af;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="callout">
        <div class="callout-title">Important Note</div>
        <p>This callout uses semi-transparent border and background colors.</p>
    </div>
</body>
```

### Example 13: Data grid with status colors

```html
<style>
    .status-grid {
        width: 100%;
        border-width: 2pt;
        border-style: solid;
        border-color: #374151;
        border-collapse: collapse;
    }
    .status-grid th {
        border-width: 1pt;
        border-style: solid;
        border-color: #6b7280;
        padding: 8pt;
        background-color: #f9fafb;
        font-weight: bold;
    }
    .status-grid td {
        border-width: 1pt;
        border-style: solid;
        padding: 8pt;
    }
    .status-complete {
        border-color: #16a34a;
        background-color: #dcfce7;
    }
    .status-pending {
        border-color: #f59e0b;
        background-color: #fef3c7;
    }
    .status-error {
        border-color: #dc2626;
        background-color: #fee2e2;
    }
</style>
<body>
    <table class="status-grid">
        <thead>
            <tr>
                <th>Task</th>
                <th>Status</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Task A</td>
                <td class="status-complete">Complete</td>
            </tr>
            <tr>
                <td>Task B</td>
                <td class="status-pending">Pending</td>
            </tr>
            <tr>
                <td>Task C</td>
                <td class="status-error">Error</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 14: Sidebar with colored accent

```html
<style>
    .layout {
        display: table;
        width: 100%;
    }
    .sidebar {
        display: table-cell;
        width: 180pt;
        border-width: 0 3pt 0 0;
        border-style: solid;
        border-color: #6366f1;
        padding: 15pt;
        background-color: #f5f3ff;
        vertical-align: top;
    }
    .content {
        display: table-cell;
        padding: 15pt;
        vertical-align: top;
    }
    .nav-item {
        border-width: 0 0 0 3pt;
        border-style: solid;
        border-color: transparent;
        padding: 8pt 8pt 8pt 12pt;
        margin-bottom: 5pt;
    }
    .nav-item.active {
        border-color: #6366f1;
        background-color: #eef2ff;
        font-weight: bold;
    }
</style>
<body>
    <div class="layout">
        <div class="sidebar">
            <h3>Menu</h3>
            <div class="nav-item active">Dashboard</div>
            <div class="nav-item">Analytics</div>
            <div class="nav-item">Reports</div>
        </div>
        <div class="content">
            <h1>Dashboard</h1>
            <p>Welcome to your dashboard.</p>
        </div>
    </div>
</body>
```

### Example 15: Pricing plans with tier colors

```html
<style>
    .pricing-container {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 10pt;
    }
    .pricing-tier {
        display: table-cell;
        border-width: 2pt;
        border-style: solid;
        padding: 20pt;
        text-align: center;
        vertical-align: top;
        background-color: white;
    }
    .tier-basic {
        border-color: #6b7280;
    }
    .tier-pro {
        border-color: #2563eb;
        background-color: #eff6ff;
        border-width: 3pt;
    }
    .tier-enterprise {
        border-color: #7c3aed;
    }
    .tier-name {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .tier-price {
        font-size: 24pt;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="pricing-container">
        <div class="pricing-tier tier-basic">
            <div class="tier-name">Basic</div>
            <div class="tier-price">$9.99</div>
            <p>Essential features</p>
        </div>
        <div class="pricing-tier tier-pro">
            <div class="tier-name">Professional</div>
            <div class="tier-price">$29.99</div>
            <p>All features</p>
        </div>
        <div class="pricing-tier tier-enterprise">
            <div class="tier-name">Enterprise</div>
            <div class="tier-price">$99.99</div>
            <p>Custom solutions</p>
        </div>
    </div>
</body>
```

---

## See Also

- [border](/reference/cssproperties/css_prop_border) - Shorthand for all border properties
- [border-width](/reference/cssproperties/css_prop_border-width) - Set border width
- [border-style](/reference/cssproperties/css_prop_border-style) - Set border style
- [border-top-color](/reference/cssproperties/css_prop_border-top-color) - Set top border color
- [border-right-color](/reference/cssproperties/css_prop_border-right-color) - Set right border color
- [border-bottom-color](/reference/cssproperties/css_prop_border-bottom-color) - Set bottom border color
- [border-left-color](/reference/cssproperties/css_prop_border-left-color) - Set left border color
- [color](/reference/cssproperties/css_prop_color) - Set text color
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
