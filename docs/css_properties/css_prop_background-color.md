---
layout: default
title: background-color
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# background-color : Background Color Property

The `background-color` property sets the background color of an element in PDF documents. This property is essential for creating visual distinction, highlighting content areas, and establishing document aesthetics.

## Usage

```css
selector {
    background-color: value;
}
```

The background-color property accepts multiple value formats including named colors, hexadecimal notation, RGB/RGBA functions, and the `transparent` keyword.

---

## Supported Values

### Named Colors
Standard CSS color names such as `red`, `blue`, `green`, `white`, `lightgray`, etc.

### Hexadecimal Colors
- Short form: `#RGB` (e.g., `#f0f` for magenta)
- Long form: `#RRGGBB` (e.g., `#ff00ff` for magenta)

### RGB/RGBA Functions
- RGB: `rgb(red, green, blue)` where values are 0-255
- RGBA: `rgba(red, green, blue, alpha)` where alpha is 0.0-1.0 for transparency

### Transparent
- `transparent` - Makes the background fully transparent (equivalent to `rgba(0,0,0,0)`)

---

## Supported Elements

The `background-color` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Table rows (`<tr>`)
- Lists (`<ul>`, `<ol>`)
- List items (`<li>`)
- All container elements

---

## Notes

- Background colors fill the entire content area including padding but not margin
- RGBA values enable semi-transparent backgrounds that blend with underlying content
- The `transparent` keyword is useful for explicitly removing backgrounds
- Background colors do not inherit from parent elements
- Multiple overlapping elements with RGBA backgrounds create layered transparency effects
- Background colors are rendered behind text and other content
- PDF rendering maintains exact color fidelity across all viewers
- For printed documents, consider color contrast and readability

---

## Data Binding

The `background-color` property can be dynamically set using data binding expressions, enabling background colors to change based on model data, status conditions, or configuration settings.

### Example 1: Dynamic status backgrounds in reports

```html
<style>
    .status-badge {
        display: inline-block;
        padding: 6pt 12pt;
        border-radius: 4pt;
        color: white;
        font-weight: bold;
    }
</style>
<body>
    {{#each transactions}}
    <div class="status-badge" style="background-color: {{statusColor}}">
        {{status}}
    </div>
    {{/each}}
</body>
```

With model data:
```json
{
    "transactions": [
        { "status": "Approved", "statusColor": "#16a34a" },
        { "status": "Pending", "statusColor": "#f59e0b" },
        { "status": "Rejected", "statusColor": "#dc2626" }
    ]
}
```

### Example 2: Conditional highlighting based on thresholds

```html
<style>
    .data-cell {
        padding: 8pt;
        text-align: right;
    }
</style>
<body>
    <table>
        <thead>
            <tr>
                <th>Department</th>
                <th>Budget Usage</th>
            </tr>
        </thead>
        <tbody>
            {{#each departments}}
            <tr>
                <td>{{name}}</td>
                <td class="data-cell" style="background-color: {{usage > 90 ? '#fee2e2' : usage > 75 ? '#fef3c7' : '#dcfce7'}}">
                    {{usage}}%
                </td>
            </tr>
            {{/each}}
        </tbody>
    </table>
</body>
```

### Example 3: Themed sections with brand colors

```html
<style>
    .section {
        padding: 20pt;
        margin-bottom: 15pt;
    }
    .section-title {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="section" style="background-color: {{branding.primaryBackground}}">
        <h2 class="section-title" style="color: {{branding.primaryText}}">Executive Summary</h2>
        <p style="color: {{branding.primaryText}}">{{summary.executive}}</p>
    </div>

    <div class="section" style="background-color: {{branding.secondaryBackground}}">
        <h2 class="section-title" style="color: {{branding.secondaryText}}">Financial Overview</h2>
        <p style="color: {{branding.secondaryText}}">{{summary.financial}}</p>
    </div>
</body>
```

With configuration:
```json
{
    "branding": {
        "primaryBackground": "#dbeafe",
        "primaryText": "#1e40af",
        "secondaryBackground": "#dcfce7",
        "secondaryText": "#166534"
    }
}
```

---

## Examples

### Example 1: Basic background color

```html
<style>
    .highlight-box {
        background-color: lightyellow;
        padding: 10pt;
    }
</style>
<body>
    <div class="highlight-box">
        <p>This content has a light yellow background.</p>
    </div>
</body>
```

### Example 2: Hexadecimal background

```html
<style>
    .info-panel {
        background-color: #e0f2fe;
        padding: 12pt;
        border: 1pt solid #0369a1;
    }
</style>
<body>
    <div class="info-panel">
        <p>Information panel with light blue background</p>
    </div>
</body>
```

### Example 3: Contrasting backgrounds

```html
<style>
    .dark-section {
        background-color: #1f2937;
        color: white;
        padding: 15pt;
    }
    .light-section {
        background-color: #f9fafb;
        color: #111827;
        padding: 15pt;
    }
</style>
<body>
    <div class="dark-section">
        <h2>Dark Theme Section</h2>
        <p>White text on dark background</p>
    </div>
    <div class="light-section">
        <h2>Light Theme Section</h2>
        <p>Dark text on light background</p>
    </div>
</body>
```

### Example 4: RGBA transparency

```html
<style>
    .overlay {
        background-color: rgba(0, 0, 255, 0.2);
        padding: 10pt;
    }
</style>
<body>
    <div class="overlay">
        <p>This has a semi-transparent blue background</p>
    </div>
</body>
```

### Example 5: Table row alternating colors

```html
<style>
    table {
        width: 100%;
        border-collapse: collapse;
    }
    tr:nth-child(even) {
        background-color: #f3f4f6;
    }
    tr:nth-child(odd) {
        background-color: white;
    }
    td {
        padding: 8pt;
    }
</style>
<body>
    <table>
        <tr><td>Row 1</td><td>Data 1</td></tr>
        <tr><td>Row 2</td><td>Data 2</td></tr>
        <tr><td>Row 3</td><td>Data 3</td></tr>
        <tr><td>Row 4</td><td>Data 4</td></tr>
    </table>
</body>
```

### Example 6: Colored table headers

```html
<style>
    .table-header {
        background-color: #1e40af;
        color: white;
        padding: 10pt;
        font-weight: bold;
    }
    .table-cell {
        background-color: white;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <table>
        <thead>
            <tr>
                <th class="table-header">Product</th>
                <th class="table-header">Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="table-cell">Widget</td>
                <td class="table-cell">$29.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 7: Alert boxes with semantic colors

```html
<style>
    .alert {
        padding: 12pt;
        margin: 8pt 0;
        border-radius: 4pt;
    }
    .alert-success {
        background-color: #dcfce7;
        color: #166534;
        border: 1pt solid #16a34a;
    }
    .alert-warning {
        background-color: #fef3c7;
        color: #92400e;
        border: 1pt solid #f59e0b;
    }
    .alert-error {
        background-color: #fee2e2;
        color: #991b1b;
        border: 1pt solid #dc2626;
    }
</style>
<body>
    <div class="alert alert-success">
        <strong>Success!</strong> Your changes have been saved.
    </div>
    <div class="alert alert-warning">
        <strong>Warning!</strong> Please review your input.
    </div>
    <div class="alert alert-error">
        <strong>Error!</strong> Something went wrong.
    </div>
</body>
```

### Example 8: Sidebar with background

```html
<style>
    .container {
        display: table;
        width: 100%;
    }
    .sidebar {
        display: table-cell;
        width: 150pt;
        background-color: #f3f4f6;
        padding: 12pt;
    }
    .content {
        display: table-cell;
        background-color: white;
        padding: 12pt;
    }
</style>
<body>
    <div class="container">
        <div class="sidebar">
            <h3>Navigation</h3>
            <p>Menu items here</p>
        </div>
        <div class="content">
            <h1>Main Content</h1>
            <p>Document content here</p>
        </div>
    </div>
</body>
```

### Example 9: Highlighted text with background

```html
<style>
    .highlight-yellow {
        background-color: #fef08a;
        padding: 2pt 4pt;
    }
    .highlight-green {
        background-color: #bbf7d0;
        padding: 2pt 4pt;
    }
    .highlight-blue {
        background-color: #bfdbfe;
        padding: 2pt 4pt;
    }
</style>
<body>
    <p>
        This is normal text with
        <span class="highlight-yellow">yellow highlighted</span>,
        <span class="highlight-green">green highlighted</span>, and
        <span class="highlight-blue">blue highlighted</span> sections.
    </p>
</body>
```

### Example 10: Invoice header with branding

```html
<style>
    .invoice-header {
        background-color: #1e3a8a;
        color: white;
        padding: 20pt;
    }
    .invoice-title {
        font-size: 24pt;
        font-weight: bold;
        margin: 0;
    }
    .invoice-subtitle {
        font-size: 12pt;
        margin: 4pt 0 0 0;
    }
</style>
<body>
    <div class="invoice-header">
        <h1 class="invoice-title">INVOICE</h1>
        <p class="invoice-subtitle">Invoice #INV-2025-001</p>
    </div>
</body>
```

### Example 11: Layered backgrounds with transparency

```html
<style>
    .base-layer {
        background-color: #3b82f6;
        padding: 20pt;
    }
    .middle-layer {
        background-color: rgba(255, 255, 255, 0.5);
        padding: 15pt;
    }
    .top-layer {
        background-color: rgba(0, 0, 0, 0.1);
        padding: 10pt;
    }
</style>
<body>
    <div class="base-layer">
        <div class="middle-layer">
            <div class="top-layer">
                <p>Multiple semi-transparent layers create depth</p>
            </div>
        </div>
    </div>
</body>
```

### Example 12: Quote block styling

```html
<style>
    .quote-block {
        background-color: #f5f5f5;
        border-left: 4pt solid #6366f1;
        padding: 12pt 12pt 12pt 16pt;
        font-style: italic;
    }
    .quote-author {
        background-color: transparent;
        color: #6b7280;
        font-size: 10pt;
        margin-top: 8pt;
    }
</style>
<body>
    <div class="quote-block">
        <p>"The best way to predict the future is to invent it."</p>
        <p class="quote-author">— Alan Kay</p>
    </div>
</body>
```

### Example 13: Data grid with colored cells

```html
<style>
    .data-grid {
        width: 100%;
        border-collapse: collapse;
    }
    .cell-positive {
        background-color: #d1fae5;
        color: #065f46;
        padding: 6pt;
    }
    .cell-negative {
        background-color: #fee2e2;
        color: #991b1b;
        padding: 6pt;
    }
    .cell-neutral {
        background-color: #f3f4f6;
        color: #374151;
        padding: 6pt;
    }
</style>
<body>
    <table class="data-grid">
        <tr>
            <td class="cell-positive">+12.5%</td>
            <td class="cell-negative">-3.2%</td>
            <td class="cell-neutral">0.0%</td>
        </tr>
    </table>
</body>
```

### Example 14: Report sections with color coding

```html
<style>
    .section {
        padding: 15pt;
        margin-bottom: 10pt;
    }
    .executive-summary {
        background-color: #dbeafe;
        border-left: 4pt solid #2563eb;
    }
    .financial-data {
        background-color: #dcfce7;
        border-left: 4pt solid #16a34a;
    }
    .recommendations {
        background-color: #fef3c7;
        border-left: 4pt solid #f59e0b;
    }
</style>
<body>
    <div class="section executive-summary">
        <h2>Executive Summary</h2>
        <p>Overview of key findings...</p>
    </div>
    <div class="section financial-data">
        <h2>Financial Data</h2>
        <p>Detailed financial information...</p>
    </div>
    <div class="section recommendations">
        <h2>Recommendations</h2>
        <p>Strategic recommendations...</p>
    </div>
</body>
```

### Example 15: Business card layout

```html
<style>
    .business-card {
        width: 250pt;
        height: 150pt;
        background-color: #0f172a;
        color: white;
        padding: 15pt;
    }
    .card-name {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 5pt;
    }
    .card-title {
        font-size: 11pt;
        color: #94a3b8;
        margin-bottom: 10pt;
    }
    .card-contact {
        font-size: 9pt;
        background-color: rgba(255, 255, 255, 0.1);
        padding: 6pt;
    }
</style>
<body>
    <div class="business-card">
        <div class="card-name">John Smith</div>
        <div class="card-title">Senior Developer</div>
        <div class="card-contact">
            john.smith@example.com<br/>
            +1 (555) 123-4567
        </div>
    </div>
</body>
```

---

## See Also

- [color](/reference/cssproperties/css_prop_color) - Set text color
- [opacity](/reference/cssproperties/css_prop_opacity) - Control overall element transparency
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [border](/reference/htmlattributes/attr_border) - Border styling

---
