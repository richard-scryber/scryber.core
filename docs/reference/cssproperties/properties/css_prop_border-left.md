---
layout: default
title: border-left
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-left : Left Border Shorthand Property

The `border-left` property is a shorthand that sets the width, style, and color of the left border of an element. This property is particularly useful for creating accent bars, callout boxes, and timeline markers.

## Usage

```css
selector {
    border-left: width style color;
}
```

The border-left property combines border-left-width, border-left-style, and border-left-color into one declaration.

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

The `border-left` property can be applied to:
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
- Left borders are excellent for creating visual accents and callout indicators
- Commonly used in blockquotes, alerts, and sidebar navigation

---

## Data Binding

The `border-left` property supports dynamic values through data binding, allowing left borders to be customized based on document data at runtime.

### Example 1: Alert boxes with severity colors

```html
<style>
    .alert-box {
        padding: 15pt;
        margin: 10pt 0;
    }
</style>
<body>
    <div class="alert-box" style="border-left: 5pt solid {{severity.color}}; background-color: {{severity.bgColor}}">
        <strong>{{severity.level}}:</strong> {{message}}
    </div>
</body>
```

Data context:
```json
{
    "severity": {
        "level": "Warning",
        "color": "#f59e0b",
        "bgColor": "#fef3c7"
    },
    "message": "Please review your input"
}
```

### Example 2: Status cards with dynamic indicators

```html
<style>
    .status-card {
        padding: 15pt 15pt 15pt 20pt;
        margin-bottom: 12pt;
        background-color: white;
    }
</style>
<body>
    <div class="status-card" style="border-left: 6pt solid {{status.indicatorColor}}">
        <h3>{{task.name}}</h3>
        <p>Status: {{status.label}}</p>
    </div>
</body>
```

Data context:
```json
{
    "task": {
        "name": "Complete Documentation"
    },
    "status": {
        "label": "In Progress",
        "indicatorColor": "#f59e0b"
    }
}
```

### Example 3: Callout boxes with conditional accents

```html
<style>
    .callout {
        padding: 15pt 15pt 15pt 20pt;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="callout" style="border-left: {{callout.isImportant ? '8pt' : '4pt'}} solid {{callout.accentColor}}; background-color: {{callout.bgColor}}">
        <strong>{{callout.title}}</strong>
        <p>{{callout.content}}</p>
    </div>
</body>
```

---

## Examples

### Example 1: Simple left accent

```html
<style>
    .accent-box {
        border-left: 4pt solid #2563eb;
        padding-left: 15pt;
    }
</style>
<body>
    <div class="accent-box">
        <p>Content with left accent border</p>
    </div>
</body>
```

### Example 2: Blockquote style

```html
<style>
    .blockquote {
        border-left: 5pt solid #6366f1;
        padding-left: 20pt;
        margin: 15pt 0;
        font-style: italic;
        color: #4f46e5;
    }
</style>
<body>
    <div class="blockquote">
        <p>"The best time to plant a tree was 20 years ago. The second best time is now."</p>
        <p style="text-align: right; font-size: 11pt;">— Chinese Proverb</p>
    </div>
</body>
```

### Example 3: Alert boxes with left indicator

```html
<style>
    .alert {
        border-left: 5pt solid;
        padding: 15pt;
        margin: 10pt 0;
    }
    .alert-info {
        border-left-color: #2563eb;
        background-color: #eff6ff;
        color: #1e40af;
    }
    .alert-success {
        border-left-color: #16a34a;
        background-color: #dcfce7;
        color: #166534;
    }
    .alert-warning {
        border-left-color: #f59e0b;
        background-color: #fef3c7;
        color: #92400e;
    }
    .alert-error {
        border-left-color: #dc2626;
        background-color: #fee2e2;
        color: #991b1b;
    }
</style>
<body>
    <div class="alert alert-info">
        <strong>Info:</strong> Here's some information.
    </div>
    <div class="alert alert-success">
        <strong>Success:</strong> Operation completed.
    </div>
    <div class="alert alert-warning">
        <strong>Warning:</strong> Please be careful.
    </div>
    <div class="alert alert-error">
        <strong>Error:</strong> Something went wrong.
    </div>
</body>
```

### Example 4: Callout box

```html
<style>
    .callout {
        border-left: 6pt solid #16a34a;
        background-color: #f0fdf4;
        padding: 15pt 15pt 15pt 20pt;
        margin: 15pt 0;
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
        <p>Use left borders to draw attention to important information.</p>
    </div>
</body>
```

### Example 5: Sidebar navigation

```html
<style>
    .nav-menu {
        list-style: none;
        padding: 0;
    }
    .nav-item {
        border-left: 4pt solid transparent;
        padding: 10pt 10pt 10pt 15pt;
        margin-bottom: 5pt;
    }
    .nav-item.active {
        border-left-color: #2563eb;
        background-color: #eff6ff;
        color: #2563eb;
        font-weight: bold;
    }
    .nav-item:hover {
        border-left-color: #cbd5e1;
        background-color: #f9fafb;
    }
</style>
<body>
    <ul class="nav-menu">
        <li class="nav-item active">Dashboard</li>
        <li class="nav-item">Projects</li>
        <li class="nav-item">Tasks</li>
        <li class="nav-item">Settings</li>
    </ul>
</body>
```

### Example 6: Timeline with left markers

```html
<style>
    .timeline {
        border-left: 3pt solid #e5e7eb;
        padding-left: 25pt;
        margin-left: 20pt;
    }
    .timeline-item {
        padding: 15pt 0;
        margin-bottom: 15pt;
        position: relative;
    }
    .timeline-item::before {
        content: '';
        width: 12pt;
        height: 12pt;
        background-color: #2563eb;
        border: 3pt solid white;
        border-radius: 50%;
        position: absolute;
        left: -31pt;
    }
    .timeline-date {
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="timeline">
        <div class="timeline-item">
            <div class="timeline-date">January 15, 2025</div>
            <p>Project initiated</p>
        </div>
        <div class="timeline-item">
            <div class="timeline-date">March 1, 2025</div>
            <p>First milestone completed</p>
        </div>
        <div class="timeline-item">
            <div class="timeline-date">May 20, 2025</div>
            <p>Final delivery</p>
        </div>
    </div>
</body>
```

### Example 7: Status cards with color indicators

```html
<style>
    .status-card {
        border-left: 6pt solid;
        padding: 15pt;
        margin-bottom: 12pt;
        background-color: white;
    }
    .status-complete {
        border-left-color: #16a34a;
        background-color: #f0fdf4;
    }
    .status-progress {
        border-left-color: #f59e0b;
        background-color: #fffbeb;
    }
    .status-pending {
        border-left-color: #6b7280;
        background-color: #f9fafb;
    }
    .card-title {
        font-weight: bold;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="status-card status-complete">
        <div class="card-title">Task A</div>
        <p>Status: Complete</p>
    </div>
    <div class="status-card status-progress">
        <div class="card-title">Task B</div>
        <p>Status: In Progress</p>
    </div>
    <div class="status-card status-pending">
        <div class="card-title">Task C</div>
        <p>Status: Pending</p>
    </div>
</body>
```

### Example 8: Code block with accent

```html
<style>
    .code-block {
        border-left: 4pt solid #3b82f6;
        background-color: #f5f5f5;
        padding: 12pt 12pt 12pt 18pt;
        font-family: 'Courier New', monospace;
        font-size: 10pt;
    }
</style>
<body>
    <div class="code-block">
        <p>function example() {</p>
        <p>&nbsp;&nbsp;return "Hello World";</p>
        <p>}</p>
    </div>
</body>
```

### Example 9: Definition list style

```html
<style>
    .definition {
        border-left: 3pt solid #8b5cf6;
        padding-left: 15pt;
        margin: 15pt 0;
    }
    .term {
        font-weight: bold;
        color: #7c3aed;
        margin-bottom: 5pt;
    }
    .description {
        color: #6b7280;
        line-height: 1.6;
    }
</style>
<body>
    <div class="definition">
        <div class="term">API (Application Programming Interface)</div>
        <div class="description">
            A set of protocols and tools for building software applications.
        </div>
    </div>
    <div class="definition">
        <div class="term">REST (Representational State Transfer)</div>
        <div class="description">
            An architectural style for designing networked applications.
        </div>
    </div>
</body>
```

### Example 10: Invoice line items with category markers

```html
<style>
    .invoice-item {
        border-left: 4pt solid #e5e7eb;
        padding: 10pt 10pt 10pt 15pt;
        margin-bottom: 8pt;
        display: table;
        width: 100%;
    }
    .invoice-item.priority {
        border-left-color: #dc2626;
        background-color: #fef2f2;
    }
    .item-description {
        display: table-cell;
        width: 70%;
    }
    .item-amount {
        display: table-cell;
        text-align: right;
        font-weight: bold;
    }
</style>
<body>
    <div class="invoice-item">
        <div class="item-description">Standard Service</div>
        <div class="item-amount">$150.00</div>
    </div>
    <div class="invoice-item priority">
        <div class="item-description">Urgent Service (Priority)</div>
        <div class="item-amount">$300.00</div>
    </div>
    <div class="invoice-item">
        <div class="item-description">Additional Support</div>
        <div class="item-amount">$75.00</div>
    </div>
</body>
```

### Example 11: Form validation indicators

```html
<style>
    .form-group {
        margin-bottom: 15pt;
    }
    .form-field {
        border: 1pt solid #d1d5db;
        border-left: 4pt solid #d1d5db;
        padding: 10pt;
    }
    .form-field.valid {
        border-left-color: #16a34a;
        background-color: #f0fdf4;
    }
    .form-field.invalid {
        border-left-color: #dc2626;
        background-color: #fef2f2;
    }
    .field-label {
        font-weight: bold;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="form-group">
        <div class="field-label">Email Address</div>
        <div class="form-field valid">john@example.com</div>
    </div>
    <div class="form-group">
        <div class="field-label">Phone Number</div>
        <div class="form-field invalid">Invalid format</div>
    </div>
</body>
```

### Example 12: Certificate with decorative left

```html
<style>
    .certificate {
        border: 4pt double #854d0e;
        border-left: 8pt double #b45309;
        padding: 40pt;
        background-color: #fffbeb;
    }
    .cert-title {
        font-size: 28pt;
        font-weight: bold;
        color: #92400e;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">Certificate of Achievement</h1>
        <p>This certificate is awarded to</p>
        <p style="font-size: 20pt; font-weight: bold; margin: 15pt 0;">Patricia Lee</p>
        <p>for outstanding performance</p>
    </div>
</body>
```

### Example 13: Dashboard widget with category colors

```html
<style>
    .dashboard-grid {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 12pt;
    }
    .widget {
        display: table-cell;
        border: 1pt solid #e5e7eb;
        border-left: 5pt solid;
        padding: 15pt;
        vertical-align: top;
    }
    .widget-sales {
        border-left-color: #2563eb;
    }
    .widget-revenue {
        border-left-color: #16a34a;
    }
    .widget-users {
        border-left-color: #f59e0b;
    }
    .widget-value {
        font-size: 24pt;
        font-weight: bold;
        margin-bottom: 5pt;
    }
    .widget-label {
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="dashboard-grid">
        <div class="widget widget-sales">
            <div class="widget-value">1,234</div>
            <div class="widget-label">Sales</div>
        </div>
        <div class="widget widget-revenue">
            <div class="widget-value">$52K</div>
            <div class="widget-label">Revenue</div>
        </div>
        <div class="widget widget-users">
            <div class="widget-value">890</div>
            <div class="widget-label">Users</div>
        </div>
    </div>
</body>
```

### Example 14: Pricing with feature highlights

```html
<style>
    .pricing-plan {
        border: 2pt solid #d1d5db;
        padding: 25pt;
        margin-bottom: 15pt;
    }
    .pricing-plan.featured {
        border-left: 8pt solid #2563eb;
        background-color: #eff6ff;
    }
    .plan-name {
        font-size: 20pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .plan-price {
        font-size: 32pt;
        color: #2563eb;
        margin: 15pt 0;
    }
    .feature-list {
        list-style: none;
        padding: 0;
    }
    .feature-list li {
        border-left: 2pt solid #10b981;
        padding-left: 10pt;
        margin: 8pt 0;
    }
</style>
<body>
    <div class="pricing-plan featured">
        <div class="plan-name">Professional</div>
        <div class="plan-price">$29.99</div>
        <ul class="feature-list">
            <li>Unlimited projects</li>
            <li>Priority support</li>
            <li>Advanced analytics</li>
        </ul>
    </div>
</body>
```

### Example 15: Report sections with category markers

```html
<style>
    .report-section {
        border-left: 5pt solid;
        padding: 15pt 15pt 15pt 20pt;
        margin: 15pt 0;
    }
    .section-financial {
        border-left-color: #16a34a;
        background-color: #f0fdf4;
    }
    .section-operational {
        border-left-color: #2563eb;
        background-color: #eff6ff;
    }
    .section-strategic {
        border-left-color: #7c3aed;
        background-color: #f5f3ff;
    }
    .section-title {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="report-section section-financial">
        <div class="section-title">Financial Performance</div>
        <p>Revenue increased by 15% year-over-year.</p>
    </div>
    <div class="report-section section-operational">
        <div class="section-title">Operational Efficiency</div>
        <p>Reduced costs by 10% through process improvements.</p>
    </div>
    <div class="report-section section-strategic">
        <div class="section-title">Strategic Initiatives</div>
        <p>Launched three new products in key markets.</p>
    </div>
</body>
```

---

## See Also

- [border](/reference/cssproperties/css_prop_border) - Shorthand for all border properties
- [border-top](/reference/cssproperties/css_prop_border-top) - Set top border
- [border-right](/reference/cssproperties/css_prop_border-right) - Set right border
- [border-bottom](/reference/cssproperties/css_prop_border-bottom) - Set bottom border
- [border-left-width](/reference/cssproperties/css_prop_border-left-width) - Set left border width
- [border-left-style](/reference/cssproperties/css_prop_border-left-style) - Set left border style
- [border-left-color](/reference/cssproperties/css_prop_border-left-color) - Set left border color
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
