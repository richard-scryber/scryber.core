---
layout: default
title: border-right
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-right : Right Border Shorthand Property

The `border-right` property is a shorthand that sets the width, style, and color of the right border of an element. This property is useful for creating vertical separators, sidebar edges, and asymmetric designs.

## Usage

```css
selector {
    border-right: width style color;
}
```

The border-right property combines border-right-width, border-right-style, and border-right-color into one declaration.

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

The `border-right` property can be applied to:
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
- Right borders are commonly used for vertical separators and column divisions
- Particularly useful in layouts with sidebars or multi-column designs

---

## Data Binding

The `border-right` property supports dynamic values through data binding, allowing right borders to be customized based on document data at runtime.

### Example 1: Column dividers with conditional styling

```html
<style>
    .table-cell {
        display: table-cell;
        padding: 10pt 15pt;
        vertical-align: top;
    }
</style>
<body>
    <div class="table-cell" style="border-right: {{cell.isLast ? 'none' : '2pt solid #e5e7eb'}}">
        <p>{{cell.content}}</p>
    </div>
</body>
```

### Example 2: Dashboard metrics with dynamic separators

```html
<style>
    .metric-box {
        display: table-cell;
        padding: 15pt 20pt;
        text-align: center;
    }
</style>
<body>
    <div class="metric-box" style="border-right: {{metric.showDivider ? '3pt solid ' + metric.dividerColor : 'none'}}">
        <div style="font-size: 32pt; font-weight: bold;">{{metric.value}}</div>
        <div style="font-size: 11pt; color: #6b7280;">{{metric.label}}</div>
    </div>
</body>
```

Data context:
```json
{
    "metric": {
        "value": "$50K",
        "label": "Revenue",
        "showDivider": true,
        "dividerColor": "#e5e7eb"
    }
}
```

### Example 3: Sidebar navigation with active indicator

```html
<style>
    .nav-item {
        padding: 10pt 15pt;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="nav-item" style="border-right: {{item.isActive ? '4pt solid #2563eb' : '4pt solid transparent'}}; background-color: {{item.isActive ? '#eff6ff' : 'transparent'}}">
        <p>{{item.label}}</p>
    </div>
</body>
```

---

## Examples

### Example 1: Simple right border separator

```html
<style>
    .right-border {
        border-right: 2pt solid #6b7280;
        padding-right: 15pt;
        display: inline-block;
    }
</style>
<body>
    <div class="right-border">
        <p>Content with right border separator</p>
    </div>
    <div style="display: inline-block; padding-left: 15pt;">
        <p>Adjacent content</p>
    </div>
</body>
```

### Example 2: Sidebar with right edge

```html
<style>
    .layout {
        display: table;
        width: 100%;
    }
    .sidebar {
        display: table-cell;
        width: 180pt;
        border-right: 3pt solid #d1d5db;
        padding-right: 15pt;
        background-color: #f9fafb;
        vertical-align: top;
    }
    .content {
        display: table-cell;
        padding-left: 15pt;
        vertical-align: top;
    }
</style>
<body>
    <div class="layout">
        <div class="sidebar">
            <h3>Navigation</h3>
            <p>Menu items</p>
        </div>
        <div class="content">
            <h1>Main Content</h1>
            <p>Document content here.</p>
        </div>
    </div>
</body>
```

### Example 3: Table column separator

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        border-right: 1pt solid #e5e7eb;
        padding: 10pt;
    }
    .data-table th:last-child,
    .data-table td:last-child {
        border-right: none;
    }
    .data-table th {
        background-color: #f3f4f6;
        font-weight: bold;
    }
</style>
<body>
    <table class="data-table">
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
                <td>Widget B</td>
                <td>Software</td>
                <td>$49.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 4: Blockquote with right accent

```html
<style>
    .quote-right {
        border-right: 4pt solid #6366f1;
        padding-right: 20pt;
        margin-right: 20pt;
        font-style: italic;
        color: #4f46e5;
        text-align: right;
    }
</style>
<body>
    <div class="quote-right">
        <p>"Design is thinking made visual."</p>
        <p>— Saul Bass</p>
    </div>
</body>
```

### Example 5: Multi-column layout with dividers

```html
<style>
    .column-layout {
        display: table;
        width: 100%;
    }
    .column {
        display: table-cell;
        width: 33.33%;
        border-right: 2pt dashed #cbd5e1;
        padding: 0 15pt;
        vertical-align: top;
    }
    .column:last-child {
        border-right: none;
    }
    .column-title {
        font-weight: bold;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="column-layout">
        <div class="column">
            <div class="column-title">Column 1</div>
            <p>First column content</p>
        </div>
        <div class="column">
            <div class="column-title">Column 2</div>
            <p>Second column content</p>
        </div>
        <div class="column">
            <div class="column-title">Column 3</div>
            <p>Third column content</p>
        </div>
    </div>
</body>
```

### Example 6: Status indicators with dividers

```html
<style>
    .status-bar {
        display: table;
        width: 100%;
    }
    .status-item {
        display: table-cell;
        width: 25%;
        border-right: 2pt solid #e5e7eb;
        padding: 15pt;
        text-align: center;
        vertical-align: top;
    }
    .status-item:last-child {
        border-right: none;
    }
    .status-value {
        font-size: 24pt;
        font-weight: bold;
        color: #2563eb;
    }
    .status-label {
        font-size: 11pt;
        color: #6b7280;
        margin-top: 5pt;
    }
</style>
<body>
    <div class="status-bar">
        <div class="status-item">
            <div class="status-value">1,234</div>
            <div class="status-label">Total Sales</div>
        </div>
        <div class="status-item">
            <div class="status-value">$50K</div>
            <div class="status-label">Revenue</div>
        </div>
        <div class="status-item">
            <div class="status-value">98%</div>
            <div class="status-label">Success Rate</div>
        </div>
        <div class="status-item">
            <div class="status-value">456</div>
            <div class="status-label">Active Users</div>
        </div>
    </div>
</body>
```

### Example 7: List items with separators

```html
<style>
    .inline-list {
        list-style: none;
        padding: 0;
    }
    .inline-list li {
        display: inline-block;
        border-right: 2pt solid #cbd5e1;
        padding: 0 15pt;
    }
    .inline-list li:last-child {
        border-right: none;
    }
</style>
<body>
    <ul class="inline-list">
        <li>Home</li>
        <li>About</li>
        <li>Services</li>
        <li>Contact</li>
    </ul>
</body>
```

### Example 8: Card grid with column dividers

```html
<style>
    .card-grid {
        display: table;
        width: 100%;
        border-collapse: separate;
    }
    .card {
        display: table-cell;
        width: 50%;
        border-right: 3pt solid #e5e7eb;
        padding: 20pt;
        vertical-align: top;
    }
    .card:last-child {
        border-right: none;
    }
    .card-title {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="card-grid">
        <div class="card">
            <div class="card-title">Feature One</div>
            <p>Description of the first feature with important details.</p>
        </div>
        <div class="card">
            <div class="card-title">Feature Two</div>
            <p>Description of the second feature with key benefits.</p>
        </div>
    </div>
</body>
```

### Example 9: Invoice item separator

```html
<style>
    .invoice-items {
        display: table;
        width: 100%;
    }
    .invoice-column {
        display: table-cell;
        border-right: 1pt solid #d1d5db;
        padding: 10pt 15pt;
        vertical-align: top;
    }
    .invoice-column:last-child {
        border-right: none;
    }
    .column-header {
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="invoice-items">
        <div class="invoice-column">
            <div class="column-header">Description</div>
            <p>Professional Services</p>
        </div>
        <div class="invoice-column">
            <div class="column-header">Quantity</div>
            <p>10 hours</p>
        </div>
        <div class="invoice-column">
            <div class="column-header">Rate</div>
            <p>$150/hr</p>
        </div>
        <div class="invoice-column">
            <div class="column-header">Total</div>
            <p>$1,500.00</p>
        </div>
    </div>
</body>
```

### Example 10: Profile card sections

```html
<style>
    .profile-card {
        border: 2pt solid #e5e7eb;
        padding: 20pt;
        display: table;
        width: 100%;
    }
    .profile-section {
        display: table-cell;
        border-right: 2pt dotted #cbd5e1;
        padding: 0 20pt;
        vertical-align: top;
    }
    .profile-section:last-child {
        border-right: none;
    }
    .section-title {
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="profile-card">
        <div class="profile-section">
            <div class="section-title">Contact</div>
            <p>john@example.com</p>
            <p>555-0123</p>
        </div>
        <div class="profile-section">
            <div class="section-title">Location</div>
            <p>San Francisco, CA</p>
            <p>United States</p>
        </div>
        <div class="profile-section">
            <div class="section-title">Role</div>
            <p>Senior Developer</p>
            <p>Engineering Team</p>
        </div>
    </div>
</body>
```

### Example 11: Pricing comparison with dividers

```html
<style>
    .pricing-compare {
        display: table;
        width: 100%;
        border: 2pt solid #d1d5db;
    }
    .pricing-option {
        display: table-cell;
        width: 50%;
        border-right: 2pt solid #d1d5db;
        padding: 25pt;
        text-align: center;
        vertical-align: top;
    }
    .pricing-option:last-child {
        border-right: none;
    }
    .pricing-option.featured {
        border-right-width: 3pt;
        border-right-color: #2563eb;
        background-color: #eff6ff;
    }
    .plan-name {
        font-size: 20pt;
        font-weight: bold;
        margin-bottom: 15pt;
    }
    .plan-price {
        font-size: 28pt;
        color: #2563eb;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="pricing-compare">
        <div class="pricing-option featured">
            <div class="plan-name">Standard</div>
            <div class="plan-price">$29.99</div>
            <p>Best for individuals</p>
        </div>
        <div class="pricing-option">
            <div class="plan-name">Premium</div>
            <div class="plan-price">$79.99</div>
            <p>Best for teams</p>
        </div>
    </div>
</body>
```

### Example 12: Certificate with right accent

```html
<style>
    .certificate-layout {
        display: table;
        width: 100%;
        border: 4pt double #854d0e;
        padding: 30pt;
        background-color: #fffbeb;
    }
    .cert-main {
        display: table-cell;
        width: 70%;
        border-right: 3pt solid #b45309;
        padding-right: 25pt;
        vertical-align: middle;
    }
    .cert-seal {
        display: table-cell;
        text-align: center;
        padding-left: 25pt;
        vertical-align: middle;
    }
    .cert-title {
        font-size: 24pt;
        font-weight: bold;
        color: #92400e;
    }
</style>
<body>
    <div class="certificate-layout">
        <div class="cert-main">
            <h1 class="cert-title">Certificate of Achievement</h1>
            <p>Presented to Michelle Wong</p>
        </div>
        <div class="cert-seal">
            <p style="font-size: 40pt; font-weight: bold;">★</p>
            <p style="font-weight: bold;">2025</p>
        </div>
    </div>
</body>
```

### Example 13: Dashboard metrics with separators

```html
<style>
    .metrics-dashboard {
        display: table;
        width: 100%;
        background-color: #f9fafb;
        padding: 20pt;
    }
    .metric-box {
        display: table-cell;
        border-right: 3pt solid white;
        padding: 15pt 20pt;
        text-align: center;
        vertical-align: top;
    }
    .metric-box:last-child {
        border-right: none;
    }
    .metric-number {
        font-size: 32pt;
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 5pt;
    }
    .metric-label {
        font-size: 11pt;
        color: #6b7280;
        text-transform: uppercase;
    }
    .metric-change {
        font-size: 10pt;
        color: #16a34a;
        margin-top: 5pt;
    }
</style>
<body>
    <div class="metrics-dashboard">
        <div class="metric-box">
            <div class="metric-number">12.5K</div>
            <div class="metric-label">Users</div>
            <div class="metric-change">+15%</div>
        </div>
        <div class="metric-box">
            <div class="metric-number">$48K</div>
            <div class="metric-label">Revenue</div>
            <div class="metric-change">+22%</div>
        </div>
        <div class="metric-box">
            <div class="metric-number">98.5%</div>
            <div class="metric-label">Uptime</div>
            <div class="metric-change">+0.3%</div>
        </div>
    </div>
</body>
```

### Example 14: Form with section dividers

```html
<style>
    .form-layout {
        display: table;
        width: 100%;
    }
    .form-column {
        display: table-cell;
        width: 50%;
        border-right: 2pt solid #e5e7eb;
        padding: 0 20pt;
        vertical-align: top;
    }
    .form-column:last-child {
        border-right: none;
    }
    .form-section-title {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 15pt;
        color: #1f2937;
    }
    .form-field {
        margin-bottom: 12pt;
    }
    .field-label {
        font-weight: bold;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="form-layout">
        <div class="form-column">
            <div class="form-section-title">Personal Details</div>
            <div class="form-field">
                <div class="field-label">Name:</div>
                <div>John Smith</div>
            </div>
            <div class="form-field">
                <div class="field-label">Email:</div>
                <div>john@example.com</div>
            </div>
        </div>
        <div class="form-column">
            <div class="form-section-title">Address Information</div>
            <div class="form-field">
                <div class="field-label">Street:</div>
                <div>123 Main Street</div>
            </div>
            <div class="form-field">
                <div class="field-label">City:</div>
                <div>San Francisco</div>
            </div>
        </div>
    </div>
</body>
```

### Example 15: Timeline with milestone markers

```html
<style>
    .timeline {
        border-left: 3pt solid #e5e7eb;
        padding-left: 25pt;
    }
    .timeline-item {
        border-right: 6pt solid transparent;
        padding: 15pt 20pt 15pt 0;
        margin-bottom: 20pt;
    }
    .timeline-item.milestone {
        border-right-color: #2563eb;
        background-color: #eff6ff;
    }
    .timeline-date {
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 5pt;
    }
    .timeline-content {
        color: #374151;
    }
</style>
<body>
    <div class="timeline">
        <div class="timeline-item">
            <div class="timeline-date">January 15, 2025</div>
            <div class="timeline-content">Project kickoff meeting</div>
        </div>
        <div class="timeline-item milestone">
            <div class="timeline-date">March 1, 2025</div>
            <div class="timeline-content">Phase 1 completion - Major milestone</div>
        </div>
        <div class="timeline-item">
            <div class="timeline-date">April 20, 2025</div>
            <div class="timeline-content">Final testing phase</div>
        </div>
    </div>
</body>
```

---

## See Also

- [border](/reference/cssproperties/css_prop_border) - Shorthand for all border properties
- [border-top](/reference/cssproperties/css_prop_border-top) - Set top border
- [border-bottom](/reference/cssproperties/css_prop_border-bottom) - Set bottom border
- [border-left](/reference/cssproperties/css_prop_border-left) - Set left border
- [border-right-width](/reference/cssproperties/css_prop_border-right-width) - Set right border width
- [border-right-style](/reference/cssproperties/css_prop_border-right-style) - Set right border style
- [border-right-color](/reference/cssproperties/css_prop_border-right-color) - Set right border color
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
