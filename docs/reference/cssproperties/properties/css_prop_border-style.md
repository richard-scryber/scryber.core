---
layout: default
title: border-style
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-style : Border Style Property

The `border-style` property sets the line style for all four borders of an element. This property is required for borders to be visible and provides various line patterns including solid, dashed, dotted, and double styles.

## Usage

```css
selector {
    border-style: value;
}
```

The border-style property accepts one to four values to set the style for individual sides independently.

---

## Supported Values

### Style Values
- `none` - No border (default)
- `solid` - Single solid line
- `dashed` - Dashed line border
- `dotted` - Dotted line border
- `double` - Double line border

### Multiple Values
- **One value**: Applies to all four sides
- **Two values**: First for top/bottom, second for left/right
- **Three values**: First for top, second for left/right, third for bottom
- **Four values**: Top, right, bottom, left (clockwise)

---

## Supported Elements

The `border-style` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables and table cells (`<table>`, `<td>`, `<th>`)
- Images (`<img>`)
- Lists and list items (`<ul>`, `<ol>`, `<li>`)
- All container elements

---

## Notes

- The border-style must be set to a value other than `none` for the border to be visible
- The `none` value is the default and effectively removes the border
- For double borders, ensure adequate border-width (minimum 3pt recommended)
- Dashed and dotted styles create repeating patterns along the border
- Border style affects the visual appearance but not the border width
- Different sides can have different styles using multiple values
- The spacing and pattern of dashed/dotted borders may vary based on border-width

---

## Data Binding

The `border-style` property supports dynamic values through data binding, allowing border patterns to be customized based on document data at runtime.

### Example 1: Status-based border styles

```html
<style>
    .notification {
        border-width: 2pt;
        border-color: #6b7280;
        padding: 12pt;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="notification" style="border-style: {{notification.style}}">
        <p>Type: {{notification.type}}</p>
        <p>Message: {{notification.message}}</p>
    </div>
</body>
```

Data context:
```json
{
    "notification": {
        "type": "Warning",
        "style": "dashed",
        "message": "Please verify your information"
    }
}
```

### Example 2: Conditional border patterns

```html
<style>
    .invoice-status {
        border-width: 2pt 2pt 2pt 6pt;
        border-color: #2563eb;
        padding: 10pt;
        margin-bottom: 10pt;
        background-color: #eff6ff;
    }
</style>
<body>
    <div class="invoice-status" style="border-style: {{invoice.isPaid ? 'solid' : 'dashed'}}">
        <h3>Invoice #{{invoice.number}}</h3>
        <p>Status: {{invoice.status}}</p>
    </div>
</body>
```

### Example 3: Alert severity styles

```html
<style>
    .alert-message {
        border-width: 2pt;
        padding: 12pt;
        margin: 10pt 0;
    }
</style>
<body>
    <div class="alert-message" style="border-style: {{alert.borderStyle}}; border-color: {{alert.color}}; background-color: {{alert.bgColor}}">
        <strong>{{alert.severity}}:</strong> {{alert.text}}
    </div>
</body>
```

Data context:
```json
{
    "alert": {
        "severity": "Critical",
        "borderStyle": "double",
        "color": "#dc2626",
        "bgColor": "#fee2e2",
        "text": "Immediate action required"
    }
}
```

---

## Examples

### Example 1: Solid border style

```html
<style>
    .solid-box {
        border-width: 2pt;
        border-style: solid;
        border-color: #1f2937;
        padding: 12pt;
    }
</style>
<body>
    <div class="solid-box">
        <p>Content with solid border on all sides</p>
    </div>
</body>
```

### Example 2: Dashed border for emphasis

```html
<style>
    .dashed-notice {
        border-width: 2pt;
        border-style: dashed;
        border-color: #f59e0b;
        background-color: #fffbeb;
        padding: 15pt;
    }
</style>
<body>
    <div class="dashed-notice">
        <strong>Notice:</strong> Dashed borders create a softer, less formal appearance.
    </div>
</body>
```

### Example 3: Dotted border for callouts

```html
<style>
    .dotted-tip {
        border-width: 2pt;
        border-style: dotted;
        border-color: #8b5cf6;
        padding: 12pt;
        background-color: #faf5ff;
    }
</style>
<body>
    <div class="dotted-tip">
        <strong>Tip:</strong> Dotted borders work well for tips and casual notes.
    </div>
</body>
```

### Example 4: Double border for certificates

```html
<style>
    .certificate {
        border-width: 6pt;
        border-style: double;
        border-color: #854d0e;
        padding: 40pt;
        background-color: #fffbeb;
        text-align: center;
    }
    .cert-title {
        font-size: 28pt;
        font-weight: bold;
        color: #92400e;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">Certificate of Achievement</h1>
        <p>This certificate is presented to</p>
        <p style="font-size: 20pt; font-weight: bold; margin: 15pt 0;">Michael Chen</p>
    </div>
</body>
```

### Example 5: Mixed border styles

```html
<style>
    .mixed-borders {
        border-width: 3pt;
        border-style: solid dashed;
        border-color: #2563eb;
        padding: 12pt;
    }
</style>
<body>
    <div class="mixed-borders">
        <p>Solid top and bottom borders, dashed left and right borders</p>
    </div>
</body>
```

### Example 6: Different style per side

```html
<style>
    .custom-card {
        border-width: 2pt 2pt 4pt 2pt;
        border-style: solid solid double solid;
        border-color: #6366f1;
        padding: 15pt;
        background-color: #f5f3ff;
    }
</style>
<body>
    <div class="custom-card">
        <h3>Special Card</h3>
        <p>Double border on bottom for emphasis, solid on other sides.</p>
    </div>
</body>
```

### Example 7: Table with varied border styles

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
        border-style: dashed;
        border-color: #cbd5e1;
        padding: 8pt;
    }
</style>
<body>
    <table class="styled-table">
        <thead>
            <tr>
                <th>Item</th>
                <th>Status</th>
                <th>Date</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Project A</td>
                <td>Complete</td>
                <td>2025-10-01</td>
            </tr>
            <tr>
                <td>Project B</td>
                <td>In Progress</td>
                <td>2025-10-15</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 8: Alert boxes with style variations

```html
<style>
    .alert {
        padding: 12pt;
        margin: 10pt 0;
        border-width: 1pt 1pt 1pt 4pt;
    }
    .alert-info {
        border-style: solid;
        border-color: #2563eb;
        background-color: #eff6ff;
    }
    .alert-warning {
        border-style: dashed;
        border-color: #f59e0b;
        background-color: #fef3c7;
    }
    .alert-danger {
        border-style: double;
        border-color: #dc2626;
        background-color: #fee2e2;
        border-width: 1pt 1pt 1pt 6pt;
    }
</style>
<body>
    <div class="alert alert-info">
        <strong>Info:</strong> Solid border for informational messages.
    </div>
    <div class="alert alert-warning">
        <strong>Warning:</strong> Dashed border for warnings.
    </div>
    <div class="alert alert-danger">
        <strong>Danger:</strong> Double border for critical alerts.
    </div>
</body>
```

### Example 9: Form elements with dotted focus

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
        border-style: dotted;
        border-color: #2563eb;
    }
</style>
<body>
    <div class="form-group">
        <label class="form-label">Full Name</label>
        <div class="form-input">John Smith</div>
    </div>
    <div class="form-group">
        <label class="form-label">Email Address</label>
        <div class="form-input focus">john.smith@example.com</div>
    </div>
</body>
```

### Example 10: Invoice sections with style separation

```html
<style>
    .invoice-section {
        padding: 12pt 0;
        margin: 10pt 0;
    }
    .section-header {
        border-width: 0 0 2pt 0;
        border-style: solid;
        border-color: #1e293b;
        padding-bottom: 8pt;
        margin-bottom: 10pt;
        font-size: 14pt;
        font-weight: bold;
    }
    .section-divider {
        border-width: 0 0 1pt 0;
        border-style: dashed;
        border-color: #cbd5e1;
        padding-bottom: 8pt;
    }
    .section-total {
        border-width: 3pt 0 3pt 0;
        border-style: double;
        border-color: #0f172a;
        padding: 12pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="invoice-section">
        <div class="section-header">Items</div>
        <p class="section-divider">Product A: $50.00</p>
        <p class="section-divider">Product B: $75.00</p>
        <p class="section-total">Total: $125.00</p>
    </div>
</body>
```

### Example 11: Callout box with dashed accent

```html
<style>
    .callout {
        border-width: 0 0 0 5pt;
        border-style: dashed;
        border-color: #16a34a;
        background-color: #f0fdf4;
        padding: 15pt 15pt 15pt 20pt;
    }
    .callout-title {
        font-size: 14pt;
        font-weight: bold;
        color: #166534;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="callout">
        <div class="callout-title">Pro Tip</div>
        <p>Use dashed borders for less formal, friendly content.</p>
    </div>
</body>
```

### Example 12: Card layout with solid borders

```html
<style>
    .card-container {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 10pt;
    }
    .card {
        display: table-cell;
        border-width: 1pt;
        border-style: solid;
        border-color: #e5e7eb;
        padding: 15pt;
        vertical-align: top;
        background-color: white;
    }
    .card-title {
        border-width: 0 0 2pt 0;
        border-style: solid;
        border-color: #2563eb;
        padding-bottom: 8pt;
        margin-bottom: 10pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="card-container">
        <div class="card">
            <div class="card-title">Feature 1</div>
            <p>Description of feature one.</p>
        </div>
        <div class="card">
            <div class="card-title">Feature 2</div>
            <p>Description of feature two.</p>
        </div>
    </div>
</body>
```

### Example 13: Data table with dotted separators

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
        border-width: 0 0 2pt 0;
        border-style: solid;
        border-color: #1f2937;
        padding: 10pt;
        background-color: #f9fafb;
        font-weight: bold;
    }
    .data-table td {
        border-width: 0 0 1pt 0;
        border-style: dotted;
        border-color: #d1d5db;
        padding: 8pt;
    }
    .data-table .summary-row td {
        border-width: 2pt 0 0 0;
        border-style: solid;
        border-color: #374151;
        font-weight: bold;
        padding-top: 12pt;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th>Category</th>
                <th>Amount</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Revenue</td>
                <td>$10,000</td>
            </tr>
            <tr>
                <td>Expenses</td>
                <td>$3,500</td>
            </tr>
            <tr class="summary-row">
                <td>Net Income</td>
                <td>$6,500</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 14: Quote block with double border

```html
<style>
    .quote-block {
        border-width: 4pt 0 4pt 0;
        border-style: double;
        border-color: #6366f1;
        padding: 20pt 0;
        margin: 20pt 0;
        text-align: center;
        font-style: italic;
    }
    .quote-text {
        font-size: 16pt;
        color: #4f46e5;
        line-height: 1.6;
    }
    .quote-author {
        font-size: 12pt;
        color: #818cf8;
        margin-top: 10pt;
    }
</style>
<body>
    <div class="quote-block">
        <p class="quote-text">
            "The only way to do great work is to love what you do."
        </p>
        <p class="quote-author">— Steve Jobs</p>
    </div>
</body>
```

### Example 15: Pricing table with style hierarchy

```html
<style>
    .pricing-grid {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 12pt;
    }
    .pricing-plan {
        display: table-cell;
        border-width: 2pt;
        border-style: dashed;
        border-color: #d1d5db;
        padding: 20pt;
        text-align: center;
        vertical-align: top;
        background-color: white;
    }
    .pricing-plan.featured {
        border-width: 3pt;
        border-style: solid;
        border-color: #2563eb;
        background-color: #eff6ff;
    }
    .plan-header {
        border-width: 0 0 2pt 0;
        border-style: solid;
        border-color: #2563eb;
        padding-bottom: 10pt;
        margin-bottom: 15pt;
    }
    .plan-name {
        font-size: 18pt;
        font-weight: bold;
    }
    .plan-price {
        font-size: 28pt;
        color: #2563eb;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="pricing-grid">
        <div class="pricing-plan">
            <div class="plan-header">
                <div class="plan-name">Basic</div>
            </div>
            <div class="plan-price">$9.99</div>
            <p>Essential features</p>
        </div>
        <div class="pricing-plan featured">
            <div class="plan-header">
                <div class="plan-name">Professional</div>
            </div>
            <div class="plan-price">$29.99</div>
            <p>All features included</p>
        </div>
        <div class="pricing-plan">
            <div class="plan-header">
                <div class="plan-name">Enterprise</div>
            </div>
            <div class="plan-price">$99.99</div>
            <p>Custom solutions</p>
        </div>
    </div>
</body>
```

---

## See Also

- [border](/reference/cssproperties/css_prop_border) - Shorthand for all border properties
- [border-width](/reference/cssproperties/css_prop_border-width) - Set border width
- [border-color](/reference/cssproperties/css_prop_border-color) - Set border color
- [border-top-style](/reference/cssproperties/css_prop_border-top-style) - Set top border style
- [border-right-style](/reference/cssproperties/css_prop_border-right-style) - Set right border style
- [border-bottom-style](/reference/cssproperties/css_prop_border-bottom-style) - Set bottom border style
- [border-left-style](/reference/cssproperties/css_prop_border-left-style) - Set left border style
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
