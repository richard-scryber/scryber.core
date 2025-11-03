---
layout: default
title: border-bottom
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-bottom : Bottom Border Shorthand Property

The `border-bottom` property is a shorthand that sets the width, style, and color of the bottom border of an element. This property is commonly used for underlines, section separators, and footer accents.

## Usage

```css
selector {
    border-bottom: width style color;
}
```

The border-bottom property combines border-bottom-width, border-bottom-style, and border-bottom-color into one declaration.

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

The `border-bottom` property can be applied to:
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
- Bottom borders are excellent for creating underlines and separators
- Commonly used in headings, table rows, and footer elements

---

## Data Binding

The `border-bottom` property supports dynamic values through data binding, allowing bottom borders to be customized based on document data at runtime.

### Example 1: Table rows with conditional separators

```html
<style>
    .table-row {
        padding: 8pt 0;
    }
</style>
<body>
    <div class="table-row" style="border-bottom: {{row.isSummary ? '3pt double #0f172a' : '1pt solid #e5e7eb'}}">
        <p>{{row.label}}: {{row.value}}</p>
    </div>
</body>
```

### Example 2: Section headers with dynamic underlines

```html
<style>
    .section-header {
        padding-bottom: 10pt;
        margin-bottom: 15pt;
        font-size: 18pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="section-header" style="border-bottom: {{section.level === 1 ? '3pt' : '2pt'}} solid {{section.accentColor}}">
        {{section.title}}
    </div>
</body>
```

Data context:
```json
{
    "section": {
        "title": "Executive Summary",
        "level": 1,
        "accentColor": "#1e293b"
    }
}
```

### Example 3: Alert cards with severity indicators

```html
<style>
    .alert-card {
        padding: 15pt;
        margin: 10pt 0;
    }
</style>
<body>
    <div class="alert-card" style="border-bottom: 5pt solid {{alert.color}}; background-color: {{alert.bgColor}}">
        <strong>{{alert.title}}:</strong> {{alert.message}}
    </div>
</body>
```

---

## Examples

### Example 1: Simple heading underline

```html
<style>
    .underlined-heading {
        border-bottom: 2pt solid #2563eb;
        padding-bottom: 10pt;
        margin-bottom: 15pt;
    }
</style>
<body>
    <h2 class="underlined-heading">Section Title</h2>
    <p>Content following the underlined heading.</p>
</body>
```

### Example 2: Table row separators

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th {
        border-bottom: 3pt solid #1e293b;
        padding: 10pt;
        background-color: #f1f5f9;
        font-weight: bold;
    }
    .data-table td {
        border-bottom: 1pt solid #e5e7eb;
        padding: 8pt;
    }
    .data-table tr:last-child td {
        border-bottom: 2pt solid #374151;
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

### Example 3: Form field underlines

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
        border-bottom: 2pt solid #9ca3af;
        padding: 5pt 0;
        width: 100%;
    }
    .form-input.focus {
        border-bottom: 3pt solid #2563eb;
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

### Example 4: Section separator

```html
<style>
    .section {
        border-bottom: 2pt dashed #cbd5e1;
        padding-bottom: 20pt;
        margin-bottom: 20pt;
    }
    .section:last-child {
        border-bottom: none;
    }
</style>
<body>
    <div class="section">
        <h3>First Section</h3>
        <p>Content of the first section.</p>
    </div>
    <div class="section">
        <h3>Second Section</h3>
        <p>Content of the second section.</p>
    </div>
    <div class="section">
        <h3>Final Section</h3>
        <p>Content of the final section.</p>
    </div>
</body>
```

### Example 5: Quote with bottom accent

```html
<style>
    .quote-box {
        border-bottom: 4pt solid #6366f1;
        padding-bottom: 15pt;
        margin-bottom: 20pt;
        font-style: italic;
        color: #4f46e5;
    }
</style>
<body>
    <div class="quote-box">
        <p>"Simplicity is the ultimate sophistication."</p>
        <p style="text-align: right; font-size: 11pt;">— Leonardo da Vinci</p>
    </div>
</body>
```

### Example 6: Navigation menu with bottom borders

```html
<style>
    .nav-menu {
        list-style: none;
        padding: 0;
    }
    .nav-menu li {
        border-bottom: 1pt solid #e5e7eb;
        padding: 12pt 0;
    }
    .nav-menu li:last-child {
        border-bottom: none;
    }
    .nav-menu li.active {
        border-bottom: 3pt solid #2563eb;
        color: #2563eb;
        font-weight: bold;
    }
</style>
<body>
    <ul class="nav-menu">
        <li class="active">Dashboard</li>
        <li>Reports</li>
        <li>Analytics</li>
        <li>Settings</li>
    </ul>
</body>
```

### Example 7: Card with bottom accent

```html
<style>
    .card {
        border: 1pt solid #e5e7eb;
        border-bottom: 4pt solid #2563eb;
        padding: 15pt;
        margin-bottom: 12pt;
        background-color: white;
    }
    .card-title {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="card">
        <div class="card-title">Featured Item</div>
        <p>This card has a thick blue bottom border for emphasis.</p>
    </div>
</body>
```

### Example 8: Invoice subtotal and total

```html
<style>
    .invoice-items {
        margin-bottom: 20pt;
    }
    .invoice-item {
        border-bottom: 1pt dotted #cbd5e1;
        padding: 8pt 0;
        display: table;
        width: 100%;
    }
    .item-description {
        display: table-cell;
        width: 70%;
    }
    .item-amount {
        display: table-cell;
        text-align: right;
    }
    .invoice-subtotal {
        border-bottom: 2pt solid #6b7280;
        padding: 10pt 0;
        display: table;
        width: 100%;
        font-weight: bold;
    }
    .invoice-total {
        border-bottom: 4pt double #0f172a;
        padding: 12pt 0;
        display: table;
        width: 100%;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="invoice-items">
        <div class="invoice-item">
            <div class="item-description">Product A</div>
            <div class="item-amount">$150.00</div>
        </div>
        <div class="invoice-item">
            <div class="item-description">Product B</div>
            <div class="item-amount">$225.00</div>
        </div>
    </div>
    <div class="invoice-subtotal">
        <div class="item-description">Subtotal</div>
        <div class="item-amount">$375.00</div>
    </div>
    <div class="invoice-total">
        <div class="item-description">Total</div>
        <div class="item-amount">$375.00</div>
    </div>
</body>
```

### Example 9: Alert with bottom decoration

```html
<style>
    .alert {
        padding: 15pt;
        margin: 10pt 0;
        border-bottom: 5pt solid;
    }
    .alert-success {
        border-bottom-color: #16a34a;
        background-color: #dcfce7;
        color: #166534;
    }
    .alert-warning {
        border-bottom-color: #f59e0b;
        background-color: #fef3c7;
        color: #92400e;
    }
    .alert-error {
        border-bottom-color: #dc2626;
        background-color: #fee2e2;
        color: #991b1b;
    }
</style>
<body>
    <div class="alert alert-success">
        <strong>Success:</strong> Changes saved.
    </div>
    <div class="alert alert-warning">
        <strong>Warning:</strong> Review required.
    </div>
    <div class="alert alert-error">
        <strong>Error:</strong> Action failed.
    </div>
</body>
```

### Example 10: Pricing tiers with bottom emphasis

```html
<style>
    .pricing-container {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 12pt;
    }
    .pricing-tier {
        display: table-cell;
        border: 2pt solid #d1d5db;
        border-bottom: 5pt solid #6b7280;
        padding: 20pt;
        text-align: center;
        vertical-align: top;
    }
    .pricing-tier.featured {
        border-bottom: 8pt solid #2563eb;
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
            <div class="tier-name">Basic</div>
            <div class="tier-price">$9.99</div>
        </div>
        <div class="pricing-tier featured">
            <div class="tier-name">Pro</div>
            <div class="tier-price">$29.99</div>
        </div>
        <div class="pricing-tier">
            <div class="tier-name">Enterprise</div>
            <div class="tier-price">$99.99</div>
        </div>
    </div>
</body>
```

### Example 11: Certificate footer

```html
<style>
    .certificate {
        border: 4pt double #854d0e;
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
    .cert-signature {
        border-bottom: 2pt solid #854d0e;
        display: inline-block;
        padding-bottom: 5pt;
        margin-top: 30pt;
        min-width: 150pt;
    }
    .cert-label {
        font-size: 10pt;
        color: #78350f;
        margin-top: 5pt;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">Certificate of Excellence</h1>
        <p>Presented to Robert Johnson</p>
        <div class="cert-signature">Director's Signature</div>
        <p class="cert-label">Program Director</p>
    </div>
</body>
```

### Example 12: Dashboard panel footers

```html
<style>
    .dashboard-panel {
        border: 1pt solid #e5e7eb;
        background-color: white;
        margin-bottom: 15pt;
    }
    .panel-header {
        padding: 15pt;
        font-weight: bold;
        background-color: #f9fafb;
    }
    .panel-content {
        padding: 15pt;
    }
    .panel-footer {
        border-bottom: 3pt solid #2563eb;
        padding: 10pt 15pt;
        background-color: #f9fafb;
        text-align: right;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="dashboard-panel">
        <div class="panel-header">Sales Overview</div>
        <div class="panel-content">
            <p>Total Sales: $125,000</p>
            <p>Growth: +15%</p>
        </div>
        <div class="panel-footer">Last updated: Oct 13, 2025</div>
    </div>
</body>
```

### Example 13: Timeline entries

```html
<style>
    .timeline-entry {
        border-bottom: 2pt solid #e5e7eb;
        padding: 15pt 0;
    }
    .timeline-entry:last-child {
        border-bottom: none;
    }
    .timeline-entry.milestone {
        border-bottom: 4pt solid #2563eb;
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
        <p>Project kickoff</p>
    </div>
    <div class="timeline-entry milestone">
        <div class="entry-date">March 2025</div>
        <p>Major milestone achieved</p>
    </div>
    <div class="timeline-entry">
        <div class="entry-date">June 2025</div>
        <p>Project completion</p>
    </div>
</body>
```

### Example 14: Profile sections

```html
<style>
    .profile-section {
        border-bottom: 2pt dashed #cbd5e1;
        padding: 15pt 0;
        margin: 10pt 0;
    }
    .profile-section:last-child {
        border-bottom: none;
    }
    .section-title {
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 10pt;
    }
    .section-content {
        color: #6b7280;
        line-height: 1.6;
    }
</style>
<body>
    <div class="profile-section">
        <div class="section-title">About</div>
        <div class="section-content">
            Senior software engineer with 10 years of experience.
        </div>
    </div>
    <div class="profile-section">
        <div class="section-title">Skills</div>
        <div class="section-content">
            JavaScript, Python, React, Node.js
        </div>
    </div>
    <div class="profile-section">
        <div class="section-title">Education</div>
        <div class="section-content">
            BS Computer Science, Stanford University
        </div>
    </div>
</body>
```

### Example 15: Report sections with varied separators

```html
<style>
    .report-section {
        padding: 20pt 0;
    }
    .section-header {
        border-bottom: 3pt solid #1e293b;
        padding-bottom: 10pt;
        margin-bottom: 15pt;
        font-size: 18pt;
        font-weight: bold;
        color: #0f172a;
    }
    .subsection {
        border-bottom: 1pt solid #e5e7eb;
        padding: 10pt 0;
        margin: 10pt 0;
    }
    .subsection:last-child {
        border-bottom: none;
    }
    .summary-footer {
        border-bottom: 4pt double #1e293b;
        padding: 15pt 0;
        margin-top: 20pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="report-section">
        <div class="section-header">Executive Summary</div>
        <div class="subsection">
            <p><strong>Revenue:</strong> $500,000</p>
        </div>
        <div class="subsection">
            <p><strong>Expenses:</strong> $350,000</p>
        </div>
        <div class="summary-footer">
            <p><strong>Net Income:</strong> $150,000</p>
        </div>
    </div>
</body>
```

---

## See Also

- [border](/reference/cssproperties/css_prop_border) - Shorthand for all border properties
- [border-top](/reference/cssproperties/css_prop_border-top) - Set top border
- [border-right](/reference/cssproperties/css_prop_border-right) - Set right border
- [border-left](/reference/cssproperties/css_prop_border-left) - Set left border
- [border-bottom-width](/reference/cssproperties/css_prop_border-bottom-width) - Set bottom border width
- [border-bottom-style](/reference/cssproperties/css_prop_border-bottom-style) - Set bottom border style
- [border-bottom-color](/reference/cssproperties/css_prop_border-bottom-color) - Set bottom border color
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
