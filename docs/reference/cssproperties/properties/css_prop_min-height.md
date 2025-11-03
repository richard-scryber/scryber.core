---
layout: default
title: min-height
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# min-height : Minimum Height Property

The `min-height` property sets the minimum vertical dimension of an element in PDF documents. It ensures that an element will never be shorter than the specified value, even if its content would require less space. This property is crucial for maintaining consistent layouts and preventing elements from collapsing when content is minimal.

## Usage

```css
selector {
    min-height: value;
}
```

The min-height property accepts a single value that defines the smallest allowed height for the element. If the calculated or specified height would be smaller, the min-height value is used instead.

---

## Supported Values

### Length Units
- Points: `50pt`, `100pt`, `200pt`
- Pixels: `50px`, `100px`, `200px`
- Inches: `1in`, `2in`, `3in`
- Centimeters: `2cm`, `5cm`, `10cm`
- Millimeters: `20mm`, `50mm`, `100mm`
- Ems: `5em`, `10em`, `20em`
- Percentage: `25%`, `50%`, `75%` (relative to parent height)

### Special Values
- `0` - No minimum height constraint (default)
- `auto` - Calculated based on content (in some contexts)

---

## Supported Elements

The `min-height` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Images (`<img>`)
- Tables (`<table>`)
- Table cells (`<td>`, `<th>`)
- Table rows (`<tr>`)
- Form elements (`<textarea>`)
- All elements with `display: block`, `display: inline-block`, or `display: table`

---

## Notes

- `min-height` overrides `height` if height is smaller than min-height
- When both `min-height` and `max-height` are specified, and min-height is larger, min-height wins
- Percentage values require the parent element to have an explicit height
- Essential for creating consistent card layouts where content length varies
- Prevents layout collapse when elements have little or no content
- Particularly useful for table rows to maintain uniform row heights
- Does not affect inline elements unless they are `display: inline-block`
- In PDF generation, min-height helps maintain page structure and prevent awkward gaps
- Useful for headers, footers, and sections that need consistent vertical spacing
- Works in conjunction with `height` and `max-height` to create flexible yet constrained layouts
- Content that exceeds min-height will cause the element to expand naturally

---

## Data Binding

The min-height property supports dynamic value binding through template expressions, allowing minimum height constraints to be set from data sources at runtime.

### Example 1: Cards with variable minimum heights

```html
<style>
    .card-grid {
        padding: 20pt;
    }
    .card {
        width: 250pt;
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
    }
    .card-title {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="card-grid">
        <div class="card" style="min-height: {{card.minHeight}}pt">
            <h3 class="card-title">{{card.title}}</h3>
            <p>{{card.content}}</p>
        </div>
    </div>
</body>
```

### Example 2: Section headers with data-driven minimum heights

```html
<style>
    .section-header {
        padding: 20pt;
        background-color: #1e40af;
        color: white;
        display: flex;
        align-items: center;
        margin-bottom: 20pt;
    }
    .section-title {
        margin: 0;
        font-size: 24pt;
    }
</style>
<body>
    <div class="section-header" style="min-height: {{section.importance === 'high' ? '120pt' : '80pt'}}">
        <h1 class="section-title">{{section.title}}</h1>
    </div>
</body>
```

### Example 3: Table rows with conditional minimum heights

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
        vertical-align: top;
    }
    .data-table th {
        background-color: #1f2937;
        color: white;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th>Item</th>
                <th>Description</th>
            </tr>
        </thead>
        <tbody>
            <tr style="min-height: {{row.hasExtendedContent ? '80pt' : '50pt'}}">
                <td>{{row.item}}</td>
                <td>{{row.description}}</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 4: Dashboard panels with responsive minimum heights

```html
<style>
    .dashboard {
        padding: 20pt;
    }
    .metric-panel {
        width: 220pt;
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
    }
    .metric-label {
        font-size: 11pt;
        color: #6b7280;
        text-transform: uppercase;
    }
    .metric-value {
        font-size: 32pt;
        font-weight: bold;
        color: #1e40af;
    }
</style>
<body>
    <div class="dashboard">
        <div class="metric-panel" style="min-height: {{panel.type === 'detailed' ? '180pt' : '140pt'}}">
            <div class="metric-label">{{panel.label}}</div>
            <div class="metric-value">{{panel.value}}</div>
            <div>{{panel.details}}</div>
        </div>
    </div>
</body>
```

---

## Examples

### Example 1: Card layout with uniform minimum height

```html
<style>
    .card-grid {
        padding: 20pt;
    }
    .card {
        min-height: 200pt;
        width: 250pt;
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
    }
    .card-title {
        margin: 0 0 10pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
    .card-content {
        color: #6b7280;
    }
</style>
<body>
    <div class="card-grid">
        <div class="card">
            <h3 class="card-title">Card One</h3>
            <p class="card-content">Short content.</p>
        </div>
        <div class="card">
            <h3 class="card-title">Card Two</h3>
            <p class="card-content">This card has more content than the first card, but all cards maintain the same minimum height for visual consistency.</p>
        </div>
    </div>
</body>
```

### Example 2: Section headers with minimum height

```html
<style>
    .section-header {
        min-height: 80pt;
        padding: 20pt;
        background-color: #1e40af;
        color: white;
        display: flex;
        align-items: center;
        margin-bottom: 20pt;
    }
    .section-title {
        margin: 0;
        font-size: 24pt;
    }
</style>
<body>
    <div class="section-header">
        <h1 class="section-title">Introduction</h1>
    </div>
    <div class="section-header">
        <h1 class="section-title">Overview of Product Features and Benefits</h1>
    </div>
</body>
```

### Example 3: Table rows with minimum height

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
    .data-table th {
        min-height: 40pt;
        background-color: #1f2937;
        color: white;
    }
    .data-table tr {
        min-height: 50pt;
    }
    .data-table td {
        vertical-align: top;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th>Item</th>
                <th>Description</th>
                <th>Status</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>A</td>
                <td>Brief</td>
                <td>Active</td>
            </tr>
            <tr>
                <td>B</td>
                <td>This is a much longer description that spans multiple lines and contains detailed information</td>
                <td>Pending</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 4: Textarea with minimum height

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
    .form-textarea {
        min-height: 100pt;
        width: 100%;
        padding: 10pt;
        border: 1pt solid #d1d5db;
        resize: vertical;
        box-sizing: border-box;
    }
    .form-textarea-large {
        min-height: 150pt;
    }
</style>
<body>
    <div class="form-container">
        <div class="form-group">
            <label class="form-label">Comments</label>
            <textarea class="form-textarea"></textarea>
        </div>
        <div class="form-group">
            <label class="form-label">Detailed Description</label>
            <textarea class="form-textarea form-textarea-large"></textarea>
        </div>
    </div>
</body>
```

### Example 5: Dashboard panels with consistent height

```html
<style>
    .dashboard {
        padding: 20pt;
    }
    .metric-panel {
        min-height: 140pt;
        width: 220pt;
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
    }
    .metric-label {
        font-size: 11pt;
        color: #6b7280;
        text-transform: uppercase;
        margin-bottom: 10pt;
    }
    .metric-value {
        font-size: 32pt;
        font-weight: bold;
        color: #1e40af;
        margin-bottom: 10pt;
    }
    .metric-change {
        font-size: 11pt;
        color: #16a34a;
    }
</style>
<body>
    <div class="dashboard">
        <div class="metric-panel">
            <div class="metric-label">Revenue</div>
            <div class="metric-value">$48.5K</div>
            <div class="metric-change">+12.5% from last month</div>
        </div>
        <div class="metric-panel">
            <div class="metric-label">Orders</div>
            <div class="metric-value">342</div>
        </div>
    </div>
</body>
```

### Example 6: Content sections with minimum height

```html
<style>
    .content-section {
        min-height: 200pt;
        padding: 25pt;
        margin-bottom: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
    }
    .section-title {
        margin: 0 0 15pt 0;
        font-size: 18pt;
        font-weight: bold;
        color: #1f2937;
    }
    .section-body {
        line-height: 1.6;
        color: #4b5563;
    }
</style>
<body>
    <div class="content-section">
        <h2 class="section-title">Executive Summary</h2>
        <div class="section-body">
            <p>Brief overview of key points.</p>
        </div>
    </div>
    <div class="content-section">
        <h2 class="section-title">Detailed Analysis</h2>
        <div class="section-body">
            <p>This section contains extensive analysis and detailed information spanning multiple paragraphs and covering various aspects of the topic.</p>
            <p>Additional paragraphs provide further context and explanation.</p>
        </div>
    </div>
</body>
```

### Example 7: Product listing with uniform heights

```html
<style>
    .product-grid {
        padding: 20pt;
    }
    .product-item {
        min-height: 180pt;
        width: 280pt;
        padding: 15pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
    }
    .product-image {
        height: 100pt;
        width: 100%;
        background-color: #f3f4f6;
        margin-bottom: 10pt;
    }
    .product-name {
        margin: 0 0 8pt 0;
        font-size: 14pt;
        font-weight: bold;
    }
    .product-price {
        font-size: 16pt;
        color: #16a34a;
        font-weight: bold;
    }
</style>
<body>
    <div class="product-grid">
        <div class="product-item">
            <div class="product-image"></div>
            <h3 class="product-name">Widget</h3>
            <div class="product-price">$29.99</div>
        </div>
        <div class="product-item">
            <div class="product-image"></div>
            <h3 class="product-name">Professional Widget Pro</h3>
            <div class="product-price">$79.99</div>
        </div>
    </div>
</body>
```

### Example 8: Alert boxes with minimum height

```html
<style>
    .alerts-container {
        padding: 20pt;
    }
    .alert {
        min-height: 60pt;
        padding: 15pt;
        margin-bottom: 12pt;
        border-radius: 4pt;
        border-left: 4pt solid;
        display: flex;
        align-items: center;
    }
    .alert-info {
        background-color: #dbeafe;
        border-left-color: #3b82f6;
    }
    .alert-success {
        background-color: #dcfce7;
        border-left-color: #16a34a;
    }
    .alert-warning {
        background-color: #fef3c7;
        border-left-color: #f59e0b;
    }
</style>
<body>
    <div class="alerts-container">
        <div class="alert alert-info">
            <p><strong>Info:</strong> Update available.</p>
        </div>
        <div class="alert alert-success">
            <p><strong>Success:</strong> Your profile has been updated successfully and all changes have been saved to the database.</p>
        </div>
        <div class="alert alert-warning">
            <p><strong>Warning:</strong> Low storage.</p>
        </div>
    </div>
</body>
```

### Example 9: Sidebar navigation with minimum height

```html
<style>
    .layout-container {
        min-height: 600pt;
        display: flex;
    }
    .sidebar {
        min-height: 100%;
        width: 200pt;
        padding: 20pt;
        background-color: #1f2937;
        color: white;
    }
    .sidebar-header {
        margin-bottom: 25pt;
        font-size: 18pt;
        font-weight: bold;
    }
    .nav-item {
        min-height: 40pt;
        padding: 10pt;
        margin-bottom: 5pt;
        background-color: #374151;
        display: flex;
        align-items: center;
    }
    .content-main {
        flex: 1;
        padding: 20pt;
    }
</style>
<body>
    <div class="layout-container">
        <div class="sidebar">
            <div class="sidebar-header">Menu</div>
            <div class="nav-item">Dashboard</div>
            <div class="nav-item">Reports</div>
            <div class="nav-item">Settings</div>
        </div>
        <div class="content-main">
            <h1>Main Content</h1>
            <p>Content area with flexible height.</p>
        </div>
    </div>
</body>
```

### Example 10: Timeline events with minimum height

```html
<style>
    .timeline {
        width: 500pt;
        margin: 30pt auto;
    }
    .timeline-event {
        min-height: 100pt;
        margin-bottom: 20pt;
        padding: 15pt;
        padding-left: 80pt;
        position: relative;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        border-left: 4pt solid #3b82f6;
    }
    .event-date {
        position: absolute;
        left: 15pt;
        top: 15pt;
        width: 50pt;
        font-weight: bold;
        font-size: 11pt;
        color: #6b7280;
    }
    .event-title {
        margin: 0 0 8pt 0;
        font-size: 14pt;
        font-weight: bold;
    }
    .event-description {
        margin: 0;
        line-height: 1.5;
    }
</style>
<body>
    <div class="timeline">
        <div class="timeline-event">
            <div class="event-date">Jan 15</div>
            <h3 class="event-title">Project Kickoff</h3>
            <p class="event-description">Initial meeting.</p>
        </div>
        <div class="timeline-event">
            <div class="event-date">Feb 20</div>
            <h3 class="event-title">Development Phase</h3>
            <p class="event-description">Began implementation of core features including user authentication, data processing, and reporting capabilities.</p>
        </div>
    </div>
</body>
```

### Example 11: Testimonial cards with consistent height

```html
<style>
    .testimonials {
        padding: 30pt;
    }
    .testimonial-card {
        min-height: 200pt;
        width: 300pt;
        padding: 25pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
        position: relative;
    }
    .testimonial-quote {
        margin: 0 0 20pt 0;
        font-style: italic;
        color: #4b5563;
        line-height: 1.6;
    }
    .testimonial-author {
        position: absolute;
        bottom: 25pt;
        font-weight: bold;
        color: #1f2937;
    }
    .testimonial-role {
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="testimonials">
        <div class="testimonial-card">
            <p class="testimonial-quote">"Excellent product!"</p>
            <div class="testimonial-author">
                <div>John Smith</div>
                <div class="testimonial-role">CEO, Company A</div>
            </div>
        </div>
        <div class="testimonial-card">
            <p class="testimonial-quote">"This solution has transformed our workflow and significantly improved our team's productivity. Highly recommended!"</p>
            <div class="testimonial-author">
                <div>Jane Doe</div>
                <div class="testimonial-role">CTO, Company B</div>
            </div>
        </div>
    </div>
</body>
```

### Example 12: Invoice line items with minimum height

```html
<style>
    .invoice-table {
        width: 100%;
        border-collapse: collapse;
    }
    .invoice-table th,
    .invoice-table td {
        padding: 12pt;
        border: 1pt solid #d1d5db;
        text-align: left;
    }
    .invoice-table th {
        background-color: #1f2937;
        color: white;
    }
    .invoice-table tr {
        min-height: 60pt;
    }
    .description-cell {
        min-height: 60pt;
        vertical-align: top;
    }
    .amount-cell {
        text-align: right;
        font-weight: bold;
    }
</style>
<body>
    <table class="invoice-table">
        <thead>
            <tr>
                <th>Description</th>
                <th>Quantity</th>
                <th>Amount</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="description-cell">Consulting</td>
                <td>10</td>
                <td class="amount-cell">$1,500.00</td>
            </tr>
            <tr>
                <td class="description-cell">Software development services including design, implementation, testing, and deployment</td>
                <td>40</td>
                <td class="amount-cell">$6,000.00</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 13: Feature comparison boxes

```html
<style>
    .comparison-container {
        padding: 30pt;
    }
    .feature-box {
        min-height: 250pt;
        width: 220pt;
        padding: 25pt;
        margin: 10pt;
        background-color: white;
        border: 2pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
    }
    .feature-box-premium {
        border-color: #3b82f6;
        background-color: #eff6ff;
    }
    .feature-title {
        margin: 0 0 15pt 0;
        font-size: 18pt;
        font-weight: bold;
        color: #1f2937;
    }
    .feature-price {
        margin-bottom: 20pt;
        font-size: 28pt;
        font-weight: bold;
        color: #16a34a;
    }
    .feature-list {
        margin: 0;
        padding: 0 0 0 20pt;
        line-height: 1.8;
    }
</style>
<body>
    <div class="comparison-container">
        <div class="feature-box">
            <h3 class="feature-title">Basic</h3>
            <div class="feature-price">$29</div>
            <ul class="feature-list">
                <li>Feature 1</li>
                <li>Feature 2</li>
            </ul>
        </div>
        <div class="feature-box feature-box-premium">
            <h3 class="feature-title">Premium</h3>
            <div class="feature-price">$99</div>
            <ul class="feature-list">
                <li>All Basic features</li>
                <li>Advanced analytics</li>
                <li>Priority support</li>
                <li>Custom integrations</li>
            </ul>
        </div>
    </div>
</body>
```

### Example 14: Status panel with minimum height

```html
<style>
    .status-container {
        padding: 20pt;
    }
    .status-panel {
        min-height: 120pt;
        padding: 20pt;
        margin-bottom: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        border-left: 6pt solid #3b82f6;
        display: flex;
        align-items: center;
    }
    .status-icon {
        width: 60pt;
        height: 60pt;
        margin-right: 20pt;
        background-color: #dbeafe;
        border-radius: 30pt;
        flex-shrink: 0;
    }
    .status-content {
        flex: 1;
    }
    .status-title {
        margin: 0 0 8pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
    .status-description {
        margin: 0;
        color: #6b7280;
        line-height: 1.5;
    }
</style>
<body>
    <div class="status-container">
        <div class="status-panel">
            <div class="status-icon"></div>
            <div class="status-content">
                <h3 class="status-title">System Online</h3>
                <p class="status-description">All systems operating normally.</p>
            </div>
        </div>
        <div class="status-panel">
            <div class="status-icon"></div>
            <div class="status-content">
                <h3 class="status-title">Database Connected</h3>
                <p class="status-description">Successfully connected to primary database server with optimal performance metrics.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 15: Report sections with minimum heights

```html
<style>
    .report {
        width: 600pt;
        margin: 30pt auto;
    }
    .report-header {
        min-height: 100pt;
        padding: 25pt;
        background-color: #1e40af;
        color: white;
        display: flex;
        align-items: center;
        justify-content: center;
        text-align: center;
    }
    .report-section {
        min-height: 180pt;
        padding: 25pt;
        margin: 20pt 0;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
    }
    .section-header {
        margin: 0 0 15pt 0;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #e5e7eb;
        font-size: 18pt;
        font-weight: bold;
    }
    .section-content {
        line-height: 1.6;
    }
</style>
<body>
    <div class="report">
        <div class="report-header">
            <h1>Annual Report 2025</h1>
        </div>
        <div class="report-section">
            <h2 class="section-header">Overview</h2>
            <div class="section-content">
                <p>Summary of key achievements.</p>
            </div>
        </div>
        <div class="report-section">
            <h2 class="section-header">Financial Performance</h2>
            <div class="section-content">
                <p>Revenue increased by 25% year-over-year, reaching $15.2 million. Operating expenses were well-controlled at 18% of revenue. Net profit margin improved to 22%.</p>
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [height](/reference/cssproperties/css_prop_height) - Set element height
- [max-height](/reference/cssproperties/css_prop_max-height) - Set maximum height constraint
- [min-width](/reference/cssproperties/css_prop_min-width) - Set minimum width constraint
- [max-width](/reference/cssproperties/css_prop_max-width) - Set maximum width constraint
- [padding](/reference/cssproperties/css_prop_padding) - Set padding shorthand property
- [margin](/reference/cssproperties/css_prop_margin) - Set margin shorthand property
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
