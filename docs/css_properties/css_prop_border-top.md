---
layout: default
title: border-top
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-top : Top Border Shorthand Property

The `border-top` property is a shorthand that sets the width, style, and color of the top border of an element. This property allows precise control over individual border sides, enabling asymmetric designs and visual emphasis.

## Usage

```css
selector {
    border-top: width style color;
}
```

The border-top property combines border-top-width, border-top-style, and border-top-color into one declaration.

---

## Supported Values

### Border Width
- **Named values**: `thin`, `medium`, `thick`
- **Length values**: Any valid length unit (e.g., `1pt`, `2px`, `0.5mm`)

### Border Style (Required)
- `none` - No border
- `solid` - Solid line border
- `dashed` - Dashed line border
- `dotted` - Dotted line border
- `double` - Double line border

### Border Color
- **Named colors**: `red`, `blue`, `black`, etc.
- **Hexadecimal**: `#RRGGBB` or `#RGB`
- **RGB/RGBA**: `rgb(r, g, b)` or `rgba(r, g, b, a)`

---

## Supported Elements

The `border-top` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables and table cells (`<table>`, `<td>`, `<th>`)
- Images (`<img>`)
- All container elements

---

## Notes

- The border style must be specified for the border to be visible
- Values can be specified in any order
- Border width defaults to `medium` if not specified
- Border color defaults to the element's text color if not specified
- Top borders are commonly used for headers and section separators
- Can be combined with other directional border properties for asymmetric designs

---

## Data Binding

The `border-top` property supports dynamic values through data binding, allowing top borders to be customized based on document data at runtime.

### Example 1: Section headers with status colors

```html
<style>
    .section-header {
        padding-top: 15pt;
        font-size: 18pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="section-header" style="border-top: {{section.borderWidth}} solid {{section.statusColor}}">
        <h2>{{section.title}}</h2>
        <p>Status: {{section.status}}</p>
    </div>
</body>
```

Data context:
```json
{
    "section": {
        "title": "Project Overview",
        "status": "Active",
        "statusColor": "#16a34a",
        "borderWidth": "4pt"
    }
}
```

### Example 2: Dynamic table row separators

```html
<style>
    .table-row {
        padding-top: 10pt;
        margin-top: 10pt;
    }
</style>
<body>
    <div class="table-row" style="border-top: {{row.isSummary ? '3pt double #0f172a' : '1pt solid #e5e7eb'}}">
        <p>{{row.label}}: {{row.value}}</p>
    </div>
</body>
```

### Example 3: Priority-based top accents

```html
<style>
    .task-card {
        padding-top: 12pt;
        margin-bottom: 10pt;
        background-color: white;
    }
</style>
<body>
    <div class="task-card" style="border-top: 4pt solid {{priority.color}}">
        <h3>{{task.name}}</h3>
        <p>Priority: {{priority.level}}</p>
    </div>
</body>
```

---

## Examples

### Example 1: Simple top border

```html
<style>
    .top-border {
        border-top: 2pt solid #1f2937;
        padding-top: 12pt;
    }
</style>
<body>
    <div class="top-border">
        <p>Content with a solid top border</p>
    </div>
</body>
```

### Example 2: Section header with accent

```html
<style>
    .section-header {
        border-top: 4pt solid #2563eb;
        padding-top: 15pt;
        font-size: 20pt;
        font-weight: bold;
    }
</style>
<body>
    <h2 class="section-header">Chapter 1: Introduction</h2>
    <p>This section begins with a strong visual separator.</p>
</body>
```

### Example 3: Dashed separator

```html
<style>
    .dashed-separator {
        border-top: 2pt dashed #6b7280;
        padding-top: 15pt;
        margin-top: 15pt;
    }
</style>
<body>
    <p>First section content.</p>
    <div class="dashed-separator">
        <p>Second section with dashed top border separator.</p>
    </div>
</body>
```

### Example 4: Double line header

```html
<style>
    .formal-header {
        border-top: 4pt double #854d0e;
        padding-top: 20pt;
        text-align: center;
    }
    .header-title {
        font-size: 24pt;
        font-weight: bold;
        color: #92400e;
    }
</style>
<body>
    <div class="formal-header">
        <h1 class="header-title">Formal Document Title</h1>
    </div>
</body>
```

### Example 5: Table header row

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table thead {
        border-top: 3pt solid #1e293b;
    }
    .data-table th {
        border-top: 2pt solid #475569;
        background-color: #f1f5f9;
        padding: 10pt;
        font-weight: bold;
    }
    .data-table td {
        border-top: 1pt solid #e5e7eb;
        padding: 8pt;
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
                <td>Widget A</td>
                <td>$29.99</td>
            </tr>
            <tr>
                <td>Widget B</td>
                <td>$39.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 6: Alert with top accent

```html
<style>
    .alert {
        border-top: 4pt solid;
        padding: 15pt;
        margin: 10pt 0;
    }
    .alert-info {
        border-top-color: #2563eb;
        background-color: #eff6ff;
    }
    .alert-success {
        border-top-color: #16a34a;
        background-color: #dcfce7;
    }
    .alert-warning {
        border-top-color: #f59e0b;
        background-color: #fef3c7;
    }
</style>
<body>
    <div class="alert alert-info">
        <strong>Info:</strong> Top border provides visual emphasis.
    </div>
    <div class="alert alert-success">
        <strong>Success:</strong> Operation completed.
    </div>
    <div class="alert alert-warning">
        <strong>Warning:</strong> Please review.
    </div>
</body>
```

### Example 7: Invoice section separator

```html
<style>
    .invoice-section {
        border-top: 2pt solid #cbd5e1;
        padding-top: 15pt;
        margin-top: 15pt;
    }
    .invoice-total {
        border-top: 3pt double #0f172a;
        padding-top: 15pt;
        margin-top: 20pt;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="invoice-section">
        <p>Subtotal: $500.00</p>
        <p>Tax: $50.00</p>
    </div>
    <div class="invoice-total">
        <p>Total: $550.00</p>
    </div>
</body>
```

### Example 8: Quote block with top accent

```html
<style>
    .quote {
        border-top: 3pt solid #6366f1;
        padding-top: 15pt;
        margin-top: 20pt;
        font-style: italic;
        color: #4f46e5;
    }
    .quote-author {
        border-top: 1pt dotted #818cf8;
        padding-top: 10pt;
        margin-top: 10pt;
        text-align: right;
        font-size: 11pt;
        color: #6366f1;
    }
</style>
<body>
    <div class="quote">
        <p>"Innovation distinguishes between a leader and a follower."</p>
        <p class="quote-author">— Steve Jobs</p>
    </div>
</body>
```

### Example 9: Card with top color indicator

```html
<style>
    .card {
        border: 1pt solid #e5e7eb;
        padding: 15pt;
        margin-bottom: 12pt;
        background-color: white;
    }
    .card-priority-high {
        border-top: 4pt solid #dc2626;
    }
    .card-priority-medium {
        border-top: 4pt solid #f59e0b;
    }
    .card-priority-low {
        border-top: 4pt solid #16a34a;
    }
    .card-title {
        font-weight: bold;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="card card-priority-high">
        <div class="card-title">Critical Issue</div>
        <p>This requires immediate attention.</p>
    </div>
    <div class="card card-priority-medium">
        <div class="card-title">Important Task</div>
        <p>Address this soon.</p>
    </div>
    <div class="card card-priority-low">
        <div class="card-title">Minor Update</div>
        <p>Can be handled later.</p>
    </div>
</body>
```

### Example 10: Form section dividers

```html
<style>
    .form-section {
        border-top: 2pt solid #d1d5db;
        padding-top: 20pt;
        margin-top: 20pt;
    }
    .form-section:first-child {
        border-top: none;
        padding-top: 0;
        margin-top: 0;
    }
    .section-title {
        font-size: 16pt;
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 15pt;
    }
</style>
<body>
    <div class="form-section">
        <h3 class="section-title">Personal Information</h3>
        <p>Name: John Smith</p>
        <p>Email: john@example.com</p>
    </div>
    <div class="form-section">
        <h3 class="section-title">Address Details</h3>
        <p>Street: 123 Main St</p>
        <p>City: Springfield</p>
    </div>
</body>
```

### Example 11: Report header with brand line

```html
<style>
    .report-header {
        border-top: 6pt solid #1e3a8a;
        padding-top: 25pt;
        margin-bottom: 30pt;
    }
    .report-title {
        font-size: 32pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
    }
    .report-subtitle {
        font-size: 14pt;
        color: #3b82f6;
    }
</style>
<body>
    <div class="report-header">
        <h1 class="report-title">Annual Performance Report</h1>
        <p class="report-subtitle">Fiscal Year 2025</p>
    </div>
</body>
```

### Example 12: Certificate with decorative top

```html
<style>
    .certificate {
        border: 4pt double #854d0e;
        border-top: 8pt double #b45309;
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
        <h1 class="cert-title">Certificate of Completion</h1>
        <p>This certifies that Sarah Johnson</p>
        <p>has successfully completed the program</p>
    </div>
</body>
```

### Example 13: Timeline entries

```html
<style>
    .timeline-entry {
        border-top: 2pt solid #e5e7eb;
        padding: 15pt 0;
    }
    .timeline-entry.milestone {
        border-top: 4pt solid #2563eb;
        background-color: #eff6ff;
        padding: 15pt;
    }
    .entry-date {
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="timeline-entry">
        <div class="entry-date">January 2025</div>
        <p>Project initiated</p>
    </div>
    <div class="timeline-entry milestone">
        <div class="entry-date">March 2025</div>
        <p>Major milestone achieved</p>
    </div>
    <div class="timeline-entry">
        <div class="entry-date">June 2025</div>
        <p>Final review completed</p>
    </div>
</body>
```

### Example 14: Pricing tiers with top decoration

```html
<style>
    .pricing-grid {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 12pt;
    }
    .pricing-tier {
        display: table-cell;
        border: 2pt solid #d1d5db;
        border-top: 5pt solid #6b7280;
        padding: 20pt;
        text-align: center;
        vertical-align: top;
    }
    .pricing-tier.featured {
        border-top: 8pt solid #2563eb;
        background-color: #eff6ff;
    }
    .tier-name {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="pricing-grid">
        <div class="pricing-tier">
            <div class="tier-name">Basic</div>
            <p>$9.99/mo</p>
        </div>
        <div class="pricing-tier featured">
            <div class="tier-name">Pro</div>
            <p>$29.99/mo</p>
        </div>
        <div class="pricing-tier">
            <div class="tier-name">Enterprise</div>
            <p>$99.99/mo</p>
        </div>
    </div>
</body>
```

### Example 15: Data summary with category headers

```html
<style>
    .summary-section {
        border-top: 3pt solid #1e293b;
        padding-top: 15pt;
        margin-top: 20pt;
    }
    .summary-header {
        font-size: 16pt;
        font-weight: bold;
        color: #0f172a;
        margin-bottom: 12pt;
    }
    .summary-item {
        border-top: 1pt dotted #cbd5e1;
        padding-top: 8pt;
        margin-top: 8pt;
        display: table;
        width: 100%;
    }
    .item-label {
        display: table-cell;
        width: 70%;
    }
    .item-value {
        display: table-cell;
        text-align: right;
        font-weight: bold;
    }
</style>
<body>
    <div class="summary-section">
        <div class="summary-header">Financial Summary</div>
        <div class="summary-item">
            <div class="item-label">Total Revenue</div>
            <div class="item-value">$125,000</div>
        </div>
        <div class="summary-item">
            <div class="item-label">Total Expenses</div>
            <div class="item-value">$85,000</div>
        </div>
        <div class="summary-item">
            <div class="item-label">Net Profit</div>
            <div class="item-value">$40,000</div>
        </div>
    </div>
</body>
```

---

## See Also

- [border](/reference/cssproperties/css_prop_border) - Shorthand for all border properties
- [border-right](/reference/cssproperties/css_prop_border-right) - Set right border
- [border-bottom](/reference/cssproperties/css_prop_border-bottom) - Set bottom border
- [border-left](/reference/cssproperties/css_prop_border-left) - Set left border
- [border-top-width](/reference/cssproperties/css_prop_border-top-width) - Set top border width
- [border-top-style](/reference/cssproperties/css_prop_border-top-style) - Set top border style
- [border-top-color](/reference/cssproperties/css_prop_border-top-color) - Set top border color
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
