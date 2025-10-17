---
layout: default
title: width
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# width : Width Property

The `width` property sets the horizontal dimension of an element in PDF documents. It defines how wide an element should be, affecting its layout and how it interacts with other elements in the document flow.

## Usage

```css
selector {
    width: value;
}
```

The width property accepts a single value that determines the element's width. By default, width applies to the content area only, but this can be modified with the `box-sizing` property.

---

## Supported Values

### Length Units
- Points: `100pt`, `200pt`, `400pt`
- Pixels: `100px`, `200px`, `400px`
- Inches: `2in`, `3in`, `6in`
- Centimeters: `5cm`, `10cm`, `15cm`
- Millimeters: `50mm`, `100mm`, `150mm`
- Ems: `10em`, `20em`, `30em`
- Percentage: `50%`, `75%`, `100%` (relative to parent width)

### Special Values
- `auto` - Browser/renderer calculates width automatically (default)

---

## Supported Elements

The `width` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Images (`<img>`)
- Tables (`<table>`)
- Table cells (`<td>`, `<th>`)
- Form inputs (`<input>`, `<textarea>`, `<select>`)
- Canvas elements
- All replaced and non-replaced block-level elements

---

## Notes

- By default, `width` applies to the content box only (padding and border are added outside)
- Use `box-sizing: border-box` to include padding and border in the width calculation
- Percentage widths are calculated relative to the parent element's content width
- The `auto` value allows the element to automatically size based on its content and constraints
- Width does not apply to inline elements (use `display: block` or `display: inline-block`)
- In PDF generation, fixed widths help ensure consistent layout across different rendering contexts
- Consider page width constraints when setting widths for PDF documents (standard A4 is ~595pt wide)
- Width interacts with `min-width` and `max-width` constraints - the final width will respect these boundaries
- Setting width on images maintains aspect ratio if height is not specified
- Tables can have their width distributed among columns using the `table-layout` property

---

## Data Binding

The width property supports dynamic value binding through template expressions, allowing widths to be set from data sources at runtime.

### Example 1: Chart with dynamic width

```html
<style>
    .chart-container {
        padding: 20pt;
    }
    .chart {
        margin-bottom: 20pt;
        padding: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .chart-bar {
        height: 30pt;
        background-color: #3b82f6;
        margin-bottom: 5pt;
    }
    .chart-label {
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="chart-container">
        <div class="chart">
            <div class="chart-bar" style="width: {{sales.q1}}pt"></div>
            <div class="chart-label">Q1: {{sales.q1}}pt</div>

            <div class="chart-bar" style="width: {{sales.q2}}pt"></div>
            <div class="chart-label">Q2: {{sales.q2}}pt</div>

            <div class="chart-bar" style="width: {{sales.q3}}pt"></div>
            <div class="chart-label">Q3: {{sales.q3}}pt</div>
        </div>
    </div>
</body>
```

### Example 2: Conditional image sizing

```html
<style>
    .product-image {
        height: auto;
        border: 2pt solid #e5e7eb;
        margin: 10pt;
    }
</style>
<body>
    <div>
        <img class="product-image"
             src="{{product.imageUrl}}"
             style="width: {{product.featured ? '400pt' : '200pt'}}"
             alt="{{product.name}}" />
    </div>
</body>
```

### Example 3: Progress bars with data-driven widths

```html
<style>
    .progress-container {
        width: 400pt;
        margin: 20pt;
    }
    .progress-item {
        margin-bottom: 15pt;
    }
    .progress-label {
        margin-bottom: 5pt;
        font-size: 11pt;
        font-weight: bold;
    }
    .progress-bar {
        width: 100%;
        height: 20pt;
        background-color: #e5e7eb;
        border-radius: 10pt;
        overflow: hidden;
    }
    .progress-fill {
        height: 100%;
        background-color: #16a34a;
    }
</style>
<body>
    <div class="progress-container">
        <div class="progress-item">
            <div class="progress-label">{{task.name}}: {{task.completion}}%</div>
            <div class="progress-bar">
                <div class="progress-fill" style="width: {{task.completion}}%"></div>
            </div>
        </div>
    </div>
</body>
```

### Example 4: Responsive table columns based on configuration

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        padding: 10pt;
        border: 1pt solid #d1d5db;
        text-align: left;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th style="width: {{config.idColumnWidth}}pt">ID</th>
                <th style="width: {{config.nameColumnWidth}}pt">Name</th>
                <th style="width: {{config.statusColumnWidth}}pt">Status</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>{{item.id}}</td>
                <td>{{item.name}}</td>
                <td>{{item.status}}</td>
            </tr>
        </tbody>
    </table>
</body>
```

---

## Examples

### Example 1: Fixed-width container

```html
<style>
    .container {
        width: 500pt;
        padding: 20pt;
        background-color: #f3f4f6;
        border: 2pt solid #d1d5db;
        margin: 0 auto;
    }
</style>
<body>
    <div class="container">
        <h2>Fixed Width Container</h2>
        <p>This container is exactly 500pt wide, plus padding and border.</p>
    </div>
</body>
```

### Example 2: Percentage-based responsive layout

```html
<style>
    .sidebar {
        width: 30%;
        float: left;
        padding: 15pt;
        background-color: #dbeafe;
    }
    .main-content {
        width: 70%;
        float: left;
        padding: 15pt;
        background-color: #f9fafb;
    }
</style>
<body>
    <div class="sidebar">
        <h3>Sidebar</h3>
        <p>30% width</p>
    </div>
    <div class="main-content">
        <h2>Main Content</h2>
        <p>70% width, adapts to parent container size.</p>
    </div>
</body>
```

### Example 3: Fixed-width image sizing

```html
<style>
    .thumbnail {
        width: 150pt;
        height: auto;
        border: 2pt solid #e5e7eb;
        margin: 10pt;
    }
    .large-image {
        width: 400pt;
        height: auto;
        display: block;
        margin: 20pt auto;
    }
</style>
<body>
    <img class="thumbnail" src="product1.jpg" alt="Product thumbnail" />
    <img class="large-image" src="hero-image.jpg" alt="Hero banner" />
</body>
```

### Example 4: Table with fixed column widths

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        padding: 10pt;
        border: 1pt solid #d1d5db;
    }
    .col-id {
        width: 50pt;
    }
    .col-name {
        width: 200pt;
    }
    .col-description {
        width: auto;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th class="col-id">ID</th>
                <th class="col-name">Name</th>
                <th class="col-description">Description</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>001</td>
                <td>Product Alpha</td>
                <td>Detailed product description goes here</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 5: Form input field widths

```html
<style>
    .form-container {
        width: 450pt;
        padding: 25pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .form-group {
        margin-bottom: 15pt;
    }
    .form-label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
    }
    .input-full {
        width: 100%;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .input-half {
        width: 48%;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .input-short {
        width: 100pt;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="form-container">
        <div class="form-group">
            <label class="form-label">Full Name</label>
            <input class="input-full" type="text" />
        </div>
        <div class="form-group">
            <label class="form-label">Postal Code</label>
            <input class="input-short" type="text" />
        </div>
    </div>
</body>
```

### Example 6: Card layout with consistent widths

```html
<style>
    .card-grid {
        padding: 20pt;
    }
    .card {
        width: 250pt;
        padding: 20pt;
        margin-bottom: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        box-sizing: border-box;
    }
    .card-image {
        width: 100%;
        height: 150pt;
        background-color: #e5e7eb;
        margin-bottom: 12pt;
    }
    .card-title {
        margin: 0 0 10pt 0;
        font-size: 14pt;
        font-weight: bold;
    }
    .card-description {
        margin: 0;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="card-grid">
        <div class="card">
            <div class="card-image"></div>
            <h3 class="card-title">Feature One</h3>
            <p class="card-description">Description of feature one with consistent width.</p>
        </div>
        <div class="card">
            <div class="card-image"></div>
            <h3 class="card-title">Feature Two</h3>
            <p class="card-description">Description of feature two with consistent width.</p>
        </div>
    </div>
</body>
```

### Example 7: Constrained content area for readability

```html
<style>
    .article-container {
        width: 550pt;
        margin: 0 auto;
        padding: 40pt 30pt;
        background-color: white;
    }
    .article-container h1 {
        margin: 0 0 20pt 0;
        font-size: 24pt;
        line-height: 1.3;
    }
    .article-container p {
        margin: 0 0 15pt 0;
        line-height: 1.6;
        text-align: justify;
    }
</style>
<body>
    <div class="article-container">
        <h1>Article Title</h1>
        <p>Constraining content width to approximately 550pt creates optimal
        line length for readability in PDF documents. This width prevents
        lines from becoming too long, which can strain reader's eyes.</p>
        <p>Research suggests optimal line length is between 50-75 characters
        for comfortable reading.</p>
    </div>
</body>
```

### Example 8: Invoice layout with precise widths

```html
<style>
    .invoice {
        width: 550pt;
        margin: 40pt auto;
        padding: 30pt;
        border: 2pt solid #000;
    }
    .invoice-header {
        width: 100%;
        margin-bottom: 30pt;
    }
    .company-info {
        width: 60%;
        float: left;
    }
    .invoice-meta {
        width: 35%;
        float: right;
        text-align: right;
    }
    .line-items {
        width: 100%;
        clear: both;
        margin-top: 20pt;
    }
    .col-description {
        width: 60%;
    }
    .col-quantity {
        width: 15%;
        text-align: center;
    }
    .col-price {
        width: 25%;
        text-align: right;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <div class="company-info">
                <h2>Company Name</h2>
                <p>123 Business St.</p>
            </div>
            <div class="invoice-meta">
                <p><strong>Invoice #:</strong> INV-2025-001</p>
                <p><strong>Date:</strong> January 15, 2025</p>
            </div>
        </div>
        <table class="line-items">
            <tr>
                <th class="col-description">Description</th>
                <th class="col-quantity">Qty</th>
                <th class="col-price">Price</th>
            </tr>
        </table>
    </div>
</body>
```

### Example 9: Dashboard panels with fixed widths

```html
<style>
    .dashboard {
        padding: 20pt;
    }
    .metric-panel {
        width: 180pt;
        height: 120pt;
        padding: 15pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
    }
    .metric-value {
        font-size: 32pt;
        font-weight: bold;
        color: #1e40af;
        margin: 10pt 0;
    }
    .metric-label {
        font-size: 10pt;
        color: #6b7280;
        text-transform: uppercase;
    }
</style>
<body>
    <div class="dashboard">
        <div class="metric-panel">
            <div class="metric-label">Total Sales</div>
            <div class="metric-value">$48.5K</div>
        </div>
        <div class="metric-panel">
            <div class="metric-label">Orders</div>
            <div class="metric-value">342</div>
        </div>
        <div class="metric-panel">
            <div class="metric-label">Customers</div>
            <div class="metric-value">128</div>
        </div>
    </div>
</body>
```

### Example 10: Progress bars with width

```html
<style>
    .progress-container {
        width: 400pt;
        margin: 20pt;
    }
    .progress-item {
        margin-bottom: 20pt;
    }
    .progress-label {
        margin-bottom: 5pt;
        font-size: 11pt;
        font-weight: bold;
    }
    .progress-bar {
        width: 100%;
        height: 20pt;
        background-color: #e5e7eb;
        border-radius: 10pt;
        overflow: hidden;
    }
    .progress-fill {
        height: 100%;
        background-color: #16a34a;
    }
    .progress-75 {
        width: 75%;
    }
    .progress-50 {
        width: 50%;
    }
    .progress-90 {
        width: 90%;
    }
</style>
<body>
    <div class="progress-container">
        <div class="progress-item">
            <div class="progress-label">Project Completion: 75%</div>
            <div class="progress-bar">
                <div class="progress-fill progress-75"></div>
            </div>
        </div>
        <div class="progress-item">
            <div class="progress-label">Budget Used: 50%</div>
            <div class="progress-bar">
                <div class="progress-fill progress-50"></div>
            </div>
        </div>
    </div>
</body>
```

### Example 11: Sidebar navigation with fixed width

```html
<style>
    .layout-container {
        padding: 20pt;
    }
    .sidebar {
        width: 200pt;
        float: left;
        padding: 15pt;
        background-color: #f3f4f6;
        border-right: 1pt solid #e5e7eb;
    }
    .sidebar h3 {
        margin: 0 0 15pt 0;
        font-size: 14pt;
    }
    .sidebar ul {
        margin: 0;
        padding: 0;
        list-style: none;
    }
    .sidebar li {
        padding: 8pt;
        margin-bottom: 5pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .content-area {
        margin-left: 220pt;
        padding: 15pt;
    }
</style>
<body>
    <div class="layout-container">
        <div class="sidebar">
            <h3>Navigation</h3>
            <ul>
                <li>Dashboard</li>
                <li>Reports</li>
                <li>Settings</li>
            </ul>
        </div>
        <div class="content-area">
            <h1>Main Content</h1>
            <p>Content area adapts to remaining space.</p>
        </div>
    </div>
</body>
```

### Example 12: Product comparison table

```html
<style>
    .comparison-table {
        width: 100%;
        border-collapse: collapse;
        margin: 20pt 0;
    }
    .comparison-table th,
    .comparison-table td {
        padding: 12pt;
        border: 1pt solid #d1d5db;
        text-align: left;
    }
    .comparison-table th {
        background-color: #1f2937;
        color: white;
    }
    .col-feature {
        width: 40%;
        font-weight: bold;
    }
    .col-basic {
        width: 20%;
        text-align: center;
    }
    .col-pro {
        width: 20%;
        text-align: center;
        background-color: #dbeafe;
    }
    .col-enterprise {
        width: 20%;
        text-align: center;
    }
</style>
<body>
    <table class="comparison-table">
        <thead>
            <tr>
                <th class="col-feature">Feature</th>
                <th class="col-basic">Basic</th>
                <th class="col-pro">Pro</th>
                <th class="col-enterprise">Enterprise</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Storage</td>
                <td>10 GB</td>
                <td>100 GB</td>
                <td>Unlimited</td>
            </tr>
            <tr>
                <td>Users</td>
                <td>1</td>
                <td>5</td>
                <td>Unlimited</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 13: Certificate border design

```html
<style>
    .certificate {
        width: 700pt;
        height: 500pt;
        margin: 40pt auto;
        padding: 40pt;
        border: 10pt double #1e3a8a;
        background-color: #fffef7;
        text-align: center;
    }
    .certificate-inner {
        width: 100%;
        height: 100%;
        border: 2pt solid #1e3a8a;
        padding: 30pt;
        display: flex;
        flex-direction: column;
        justify-content: center;
    }
    .certificate-title {
        width: 100%;
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 30pt;
    }
    .certificate-recipient {
        width: 80%;
        margin: 0 auto 20pt;
        font-size: 24pt;
        border-bottom: 2pt solid #000;
        padding-bottom: 10pt;
    }
</style>
<body>
    <div class="certificate">
        <div class="certificate-inner">
            <div class="certificate-title">Certificate of Achievement</div>
            <div class="certificate-recipient">John Smith</div>
            <p>For outstanding performance</p>
        </div>
    </div>
</body>
```

### Example 14: Timeline visualization

```html
<style>
    .timeline {
        width: 500pt;
        margin: 30pt auto;
        position: relative;
    }
    .timeline-item {
        margin-bottom: 30pt;
        position: relative;
    }
    .timeline-date {
        width: 100pt;
        float: left;
        font-weight: bold;
        color: #6b7280;
    }
    .timeline-content {
        width: 380pt;
        float: right;
        padding: 15pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        border-left: 4pt solid #3b82f6;
    }
    .timeline-title {
        margin: 0 0 8pt 0;
        font-size: 14pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="timeline">
        <div class="timeline-item">
            <div class="timeline-date">Jan 2025</div>
            <div class="timeline-content">
                <div class="timeline-title">Project Launch</div>
                <p>Initial project kickoff and planning phase.</p>
            </div>
        </div>
        <div class="timeline-item">
            <div class="timeline-date">Mar 2025</div>
            <div class="timeline-content">
                <div class="timeline-title">Development Phase</div>
                <p>Core features implementation begins.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 15: Box-sizing comparison

```html
<style>
    .example-container {
        padding: 20pt;
    }
    .box-content {
        width: 300pt;
        padding: 20pt;
        border: 5pt solid #3b82f6;
        background-color: #dbeafe;
        margin-bottom: 20pt;
    }
    .box-border {
        width: 300pt;
        padding: 20pt;
        border: 5pt solid #16a34a;
        background-color: #dcfce7;
        box-sizing: border-box;
    }
    .box-label {
        font-size: 10pt;
        color: #6b7280;
        margin-top: 5pt;
    }
</style>
<body>
    <div class="example-container">
        <div class="box-content">
            <p><strong>Content Box (default)</strong></p>
            <p>Total width: 300pt + 40pt padding + 10pt border = 350pt</p>
        </div>
        <div class="box-label">box-sizing: content-box (default)</div>

        <div class="box-border">
            <p><strong>Border Box</strong></p>
            <p>Total width: 300pt (includes padding and border)</p>
        </div>
        <div class="box-label">box-sizing: border-box</div>
    </div>
</body>
```

---

## See Also

- [height](/reference/cssproperties/css_prop_height) - Set element height
- [min-width](/reference/cssproperties/css_prop_min-width) - Set minimum width constraint
- [max-width](/reference/cssproperties/css_prop_max-width) - Set maximum width constraint
- [padding](/reference/cssproperties/css_prop_padding) - Set padding shorthand property
- [border](/reference/cssproperties/css_prop_border) - Set border shorthand property
- [margin](/reference/cssproperties/css_prop_margin) - Set margin shorthand property
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
