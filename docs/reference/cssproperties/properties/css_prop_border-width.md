---
layout: default
title: border-width
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-width : Border Width Property

The `border-width` property sets the width of all four borders of an element. This property controls the thickness of borders and can be specified using named values or length measurements.

## Usage

```css
selector {
    border-width: value;
}
```

The border-width property accepts one to four values for setting widths of individual sides, or named thickness values.

---

## Supported Values

### Named Widths
- `thin` - A thin border (typically 1pt)
- `medium` - A medium border (typically 2pt) - default value
- `thick` - A thick border (typically 4pt)

### Length Values
Any valid length unit including:
- Points: `1pt`, `2pt`, `3pt`
- Pixels: `1px`, `2px`, `3px`
- Millimeters: `0.5mm`, `1mm`

### Multiple Values
- **One value**: Applies to all four sides
- **Two values**: First for top/bottom, second for left/right
- **Three values**: First for top, second for left/right, third for bottom
- **Four values**: Top, right, bottom, left (clockwise)

---

## Supported Elements

The `border-width` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables and table cells (`<table>`, `<td>`, `<th>`)
- Images (`<img>`)
- All container elements

---

## Notes

- Border width has no effect unless `border-style` is set to a value other than `none`
- The actual rendered size of named values (thin, medium, thick) may vary slightly
- Border width adds to the total size of the element unless `box-sizing: border-box` is used
- Zero width (`0`) effectively removes the border
- Border width cannot be negative
- For double borders, ensure the width is sufficient to show both lines (minimum 3pt recommended)
- The border is drawn outside the content area but inside the margin

---

## Data Binding

The `border-width` property supports dynamic values through data binding, allowing border thickness to be adjusted based on document data at runtime.

### Example 1: Conditional border emphasis

```html
<style>
    .task-item {
        border-style: solid;
        border-color: #2563eb;
        padding: 10pt;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="task-item" style="border-width: {{task.isHighlighted ? '3pt' : '1pt'}}">
        <p>Task: {{task.title}}</p>
        <p>Status: {{task.status}}</p>
    </div>
</body>
```

### Example 2: Priority-based border thickness

```html
<style>
    .document-card {
        border-style: solid;
        border-color: #1f2937;
        padding: 12pt;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="document-card" style="border-width: {{priority.borderWidth}}">
        <h3>{{document.title}}</h3>
        <p>Priority: {{priority.level}}</p>
    </div>
</body>
```

Data context:
```json
{
    "document": {
        "title": "Quarterly Report"
    },
    "priority": {
        "level": "High",
        "borderWidth": "4pt"
    }
}
```

### Example 3: Table rows with dynamic borders

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-row {
        border-style: solid;
        border-color: #cbd5e1;
    }
</style>
<body>
    <table class="data-table">
        <tr class="data-row" style="border-width: {{row.isSummary ? '3pt 0 3pt 0' : '1pt 0 1pt 0'}}">
            <td>{{row.label}}</td>
            <td>{{row.value}}</td>
        </tr>
    </table>
</body>
```

---

## Examples

### Example 1: Single width value for all sides

```html
<style>
    .uniform-border {
        border-width: 2pt;
        border-style: solid;
        border-color: black;
        padding: 10pt;
    }
</style>
<body>
    <div class="uniform-border">
        <p>All borders are 2pt thick</p>
    </div>
</body>
```

### Example 2: Named width values

```html
<style>
    .thin-border {
        border-width: thin;
        border-style: solid;
        border-color: #6b7280;
        padding: 8pt;
        margin-bottom: 10pt;
    }
    .medium-border {
        border-width: medium;
        border-style: solid;
        border-color: #6b7280;
        padding: 8pt;
        margin-bottom: 10pt;
    }
    .thick-border {
        border-width: thick;
        border-style: solid;
        border-color: #6b7280;
        padding: 8pt;
    }
</style>
<body>
    <div class="thin-border">Thin border</div>
    <div class="medium-border">Medium border</div>
    <div class="thick-border">Thick border</div>
</body>
```

### Example 3: Different widths for vertical and horizontal

```html
<style>
    .emphasis-box {
        border-width: 4pt 1pt;
        border-style: solid;
        border-color: #2563eb;
        padding: 15pt;
        background-color: #eff6ff;
    }
</style>
<body>
    <div class="emphasis-box">
        <p>Thick top and bottom borders (4pt), thin left and right borders (1pt)</p>
    </div>
</body>
```

### Example 4: Three values for asymmetric design

```html
<style>
    .custom-card {
        border-width: 1pt 2pt 3pt;
        border-style: solid;
        border-color: #8b5cf6;
        padding: 12pt;
        background-color: #faf5ff;
    }
</style>
<body>
    <div class="custom-card">
        <p>Top: 1pt, Left/Right: 2pt, Bottom: 3pt</p>
    </div>
</body>
```

### Example 5: Individual width for each side

```html
<style>
    .progressive-border {
        border-width: 1pt 2pt 3pt 4pt;
        border-style: solid;
        border-color: #f59e0b;
        padding: 12pt;
    }
</style>
<body>
    <div class="progressive-border">
        <p>Each side has a different width: Top 1pt, Right 2pt, Bottom 3pt, Left 4pt</p>
    </div>
</body>
```

### Example 6: Table with varying border widths

```html
<style>
    .styled-table {
        width: 100%;
        border-width: 3pt;
        border-style: solid;
        border-color: #1e293b;
        border-collapse: collapse;
    }
    .styled-table th {
        border-width: 0 0 2pt 0;
        border-style: solid;
        border-color: #475569;
        padding: 10pt;
        background-color: #f1f5f9;
        font-weight: bold;
    }
    .styled-table td {
        border-width: 0 0 1pt 0;
        border-style: solid;
        border-color: #cbd5e1;
        padding: 8pt;
    }
</style>
<body>
    <table class="styled-table">
        <thead>
            <tr>
                <th>Product</th>
                <th>Price</th>
                <th>Stock</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget A</td>
                <td>$29.99</td>
                <td>150</td>
            </tr>
            <tr>
                <td>Widget B</td>
                <td>$39.99</td>
                <td>75</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 7: Certificate with thick decorative border

```html
<style>
    .certificate {
        border-width: 8pt;
        border-style: double;
        border-color: #854d0e;
        padding: 40pt;
        text-align: center;
        background-color: #fffbeb;
    }
    .cert-title {
        font-size: 28pt;
        font-weight: bold;
        color: #92400e;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">Certificate of Completion</h1>
        <p>Awarded to Sarah Johnson</p>
    </div>
</body>
```

### Example 8: Alert boxes with accent borders

```html
<style>
    .alert {
        border-width: 1pt 1pt 1pt 4pt;
        border-style: solid;
        padding: 12pt;
        margin: 10pt 0;
    }
    .alert-info {
        border-color: #2563eb;
        background-color: #eff6ff;
    }
    .alert-success {
        border-color: #16a34a;
        background-color: #dcfce7;
    }
    .alert-warning {
        border-color: #f59e0b;
        background-color: #fef3c7;
    }
</style>
<body>
    <div class="alert alert-info">
        <strong>Info:</strong> Thick left border emphasizes the alert type.
    </div>
    <div class="alert alert-success">
        <strong>Success:</strong> Operation completed successfully.
    </div>
    <div class="alert alert-warning">
        <strong>Warning:</strong> Please review your settings.
    </div>
</body>
```

### Example 9: Form with varying border weights

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
        border-width: 0 0 2pt 0;
        border-style: solid;
        border-color: #9ca3af;
        padding: 5pt 0;
        width: 100%;
    }
    .form-input.focus {
        border-width: 0 0 3pt 0;
        border-color: #2563eb;
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
</body>
```

### Example 10: Card with subtle bottom accent

```html
<style>
    .card {
        border-width: 1pt 1pt 4pt 1pt;
        border-style: solid;
        border-color: #e5e7eb #e5e7eb #2563eb #e5e7eb;
        background-color: white;
        padding: 15pt;
        margin-bottom: 12pt;
    }
    .card-title {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="card">
        <h3 class="card-title">Featured Item</h3>
        <p>This card has a thick blue bottom border for emphasis.</p>
    </div>
</body>
```

### Example 11: Invoice with progressive borders

```html
<style>
    .invoice-section {
        border-width: 2pt 0 1pt 0;
        border-style: solid;
        border-color: #1e293b;
        padding: 10pt 0;
        margin: 10pt 0;
    }
    .invoice-total {
        border-width: 3pt 0 3pt 0;
        border-style: double;
        border-color: #0f172a;
        padding: 12pt 0;
        font-size: 14pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="invoice-section">
        <p>Subtotal: $500.00</p>
    </div>
    <div class="invoice-section">
        <p>Tax (10%): $50.00</p>
    </div>
    <div class="invoice-total">
        <p>Total Amount: $550.00</p>
    </div>
</body>
```

### Example 12: Callout box with thick accent

```html
<style>
    .callout {
        border-width: 0 0 0 6pt;
        border-style: solid;
        border-color: #dc2626;
        background-color: #fef2f2;
        padding: 15pt 15pt 15pt 20pt;
    }
    .callout-title {
        font-size: 14pt;
        font-weight: bold;
        color: #991b1b;
        margin-bottom: 8pt;
    }
    .callout-text {
        color: #7f1d1d;
    }
</style>
<body>
    <div class="callout">
        <div class="callout-title">Important Notice</div>
        <p class="callout-text">
            This information requires immediate attention. Please review carefully.
        </p>
    </div>
</body>
```

### Example 13: Data table with emphasis rows

```html
<style>
    .data-table {
        width: 100%;
        border-width: 2pt;
        border-style: solid;
        border-color: #374151;
        border-collapse: collapse;
    }
    .data-table th {
        border-width: 0 0 3pt 0;
        border-style: solid;
        border-color: #1f2937;
        padding: 10pt;
        background-color: #f3f4f6;
    }
    .data-table td {
        border-width: 0 0 1pt 0;
        border-style: solid;
        border-color: #e5e7eb;
        padding: 8pt;
    }
    .data-table .total-row td {
        border-width: 2pt 0 2pt 0;
        border-color: #374151;
        font-weight: bold;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th>Description</th>
                <th>Amount</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Product Sales</td>
                <td>$1,500.00</td>
            </tr>
            <tr>
                <td>Services</td>
                <td>$750.00</td>
            </tr>
            <tr class="total-row">
                <td>Total Revenue</td>
                <td>$2,250.00</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 14: Sidebar with emphasis border

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
        border-color: #d1d5db;
        padding: 15pt;
        background-color: #f9fafb;
        vertical-align: top;
    }
    .content {
        display: table-cell;
        padding: 15pt;
        vertical-align: top;
    }
    .sidebar-heading {
        border-width: 0 0 2pt 0;
        border-style: solid;
        border-color: #6b7280;
        padding-bottom: 8pt;
        margin-bottom: 12pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="layout">
        <div class="sidebar">
            <div class="sidebar-heading">Quick Links</div>
            <p>Dashboard</p>
            <p>Reports</p>
            <p>Settings</p>
        </div>
        <div class="content">
            <h1>Main Content Area</h1>
            <p>Document content goes here.</p>
        </div>
    </div>
</body>
```

### Example 15: Pricing tiers with border distinction

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
        border-color: #d1d5db;
        padding: 20pt;
        text-align: center;
        vertical-align: top;
    }
    .pricing-tier.featured {
        border-width: 4pt;
        border-color: #2563eb;
        background-color: #eff6ff;
    }
    .tier-name {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .tier-price {
        font-size: 24pt;
        color: #2563eb;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="pricing-container">
        <div class="pricing-tier">
            <div class="tier-name">Starter</div>
            <div class="tier-price">$9/mo</div>
            <p>Basic features</p>
        </div>
        <div class="pricing-tier featured">
            <div class="tier-name">Pro</div>
            <div class="tier-price">$29/mo</div>
            <p>All features included</p>
        </div>
        <div class="pricing-tier">
            <div class="tier-name">Enterprise</div>
            <div class="tier-price">$99/mo</div>
            <p>Custom solutions</p>
        </div>
    </div>
</body>
```

---

## See Also

- [border](/reference/cssproperties/css_prop_border) - Shorthand for all border properties
- [border-style](/reference/cssproperties/css_prop_border-style) - Set border style
- [border-color](/reference/cssproperties/css_prop_border-color) - Set border color
- [border-top-width](/reference/cssproperties/css_prop_border-top-width) - Set top border width
- [border-right-width](/reference/cssproperties/css_prop_border-right-width) - Set right border width
- [border-bottom-width](/reference/cssproperties/css_prop_border-bottom-width) - Set bottom border width
- [border-left-width](/reference/cssproperties/css_prop_border-left-width) - Set left border width
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
