---
layout: default
title: margin-right
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# margin-right : Right Margin Property

The `margin-right` property sets the right margin of an element in PDF documents. The right margin creates space to the right of the element, separating it from adjacent elements or the page edge and controlling horizontal spacing in document layouts.

## Usage

```css
selector {
    margin-right: value;
}
```

The margin-right property accepts a single length value or percentage that defines the space to the right of the element.

---

## Supported Values

### Length Units
- Points: `10pt`, `15pt`, `20pt`
- Pixels: `10px`, `15px`, `20px`
- Inches: `0.5in`, `1in`
- Centimeters: `2cm`, `3cm`
- Millimeters: `10mm`, `20mm`
- Ems: `1em`, `1.5em`, `2em`
- Percentage: `5%`, `10%`, `15%` (relative to parent width)

### Special Values
- `0` - No right margin
- `auto` - Automatic margin (useful for layout alignment)

---

## Supported Elements

The `margin-right` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables (`<table>`)
- Lists (`<ul>`, `<ol>`)
- Images (`<img>`)
- Floated elements
- All container elements

---

## Notes

- Right margins are transparent and do not have background colors
- Right margins do not collapse (unlike vertical margins)
- Negative right margins pull adjacent elements closer or cause overlapping
- Percentage right margins are calculated relative to the parent element's width
- Right margins are particularly useful for spacing floated elements
- The `auto` value can be used with `margin-left: auto` to center block elements
- Right margins affect the flow of inline and floated content

---

## Data Binding

The `margin-right` property supports dynamic values through data binding, allowing you to create flexible right spacing for layouts, grids, and inline elements based on data-driven requirements.

### Example 1: Dynamic column spacing for responsive layouts

```html
<style>
    .column-container {
        padding: 20pt;
    }
    .column {
        float: left;
        width: {{layout.columnWidth}}%;
        margin-right: {{layout.columnGap}}%;
        padding: 15pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
    }
    .column:last-child {
        margin-right: 0;
    }
</style>
<body>
    <div class="column-container">
        <div class="column">
            <h3>Column 1</h3>
            <p>Dynamic column spacing.</p>
        </div>
        <div class="column">
            <h3>Column 2</h3>
            <p>Adapts to layout settings.</p>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "layout": {
        "columnWidth": 47,
        "columnGap": 6
    }
}
```

### Example 2: Badge spacing based on display mode

```html
<style>
    .badge {
        display: inline-block;
        margin-right: {{displayMode === 'compact' ? '4pt' : '10pt'}};
        padding: {{displayMode === 'compact' ? '3pt 8pt' : '5pt 12pt'}};
        background-color: #3b82f6;
        color: white;
        border-radius: 3pt;
        font-size: {{displayMode === 'compact' ? '9pt' : '10pt'}};
    }
    .badge:last-child {
        margin-right: 0;
    }
</style>
<body>
    <p>
        <span class="badge">New</span>
        <span class="badge">Featured</span>
        <span class="badge">Popular</span>
    </p>
</body>
```

Data context:
```json
{
    "displayMode": "normal"
}
```

### Example 3: Data-driven product grid spacing

```html
<style>
    .product-grid {
        padding: 25pt;
    }
    .product-card {
        float: left;
        width: {{grid.cardWidth}}pt;
        margin-right: {{grid.spacing}}pt;
        margin-bottom: {{grid.spacing}}pt;
        padding: 15pt;
        border: 1pt solid #e5e7eb;
        background-color: white;
    }
    .product-card:nth-child({{grid.columns}}n) {
        margin-right: 0;
    }
</style>
<body>
    <div class="product-grid" style="width: {{grid.totalWidth}}pt;">
        <div class="product-card">
            <div>Product A</div>
            <div>$29.99</div>
        </div>
        <div class="product-card">
            <div>Product B</div>
            <div>$39.99</div>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "grid": {
        "columns": 3,
        "cardWidth": 150,
        "spacing": 20,
        "totalWidth": 530
    }
}
```

---

## Examples

### Example 1: Basic right margin

```html
<style>
    .box {
        margin-right: 20pt;
        padding: 15pt;
        background-color: #dbeafe;
        display: inline-block;
    }
</style>
<body>
    <div class="box">Box 1</div>
    <div class="box">Box 2</div>
    <div class="box">Box 3</div>
</body>
```

### Example 2: Floated sidebar with right margin

```html
<style>
    .sidebar {
        float: left;
        width: 150pt;
        margin-right: 20pt;
        padding: 15pt;
        background-color: #f3f4f6;
    }
    .main-content {
        overflow: hidden;
    }
    .sidebar h3 {
        margin: 0 0 10pt 0;
    }
</style>
<body>
    <div class="sidebar">
        <h3>Navigation</h3>
        <p>Menu items</p>
    </div>
    <div class="main-content">
        <h1>Main Content</h1>
        <p>Content flows to the right of the sidebar with proper spacing.</p>
    </div>
</body>
```

### Example 3: Inline badges with right margin

```html
<style>
    .badge {
        display: inline-block;
        margin-right: 8pt;
        padding: 4pt 10pt;
        background-color: #3b82f6;
        color: white;
        border-radius: 3pt;
        font-size: 10pt;
    }
    .badge:last-child {
        margin-right: 0;
    }
</style>
<body>
    <p>
        <span class="badge">New</span>
        <span class="badge">Featured</span>
        <span class="badge">Popular</span>
    </p>
</body>
```

### Example 4: Column layout with right margin

```html
<style>
    .column-container {
        padding: 20pt;
    }
    .column {
        float: left;
        width: 30%;
        margin-right: 5%;
        padding: 15pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
    }
    .column:last-child {
        margin-right: 0;
    }
    .column h3 {
        margin: 0 0 10pt 0;
    }
</style>
<body>
    <div class="column-container">
        <div class="column">
            <h3>Column 1</h3>
            <p>First column content.</p>
        </div>
        <div class="column">
            <h3>Column 2</h3>
            <p>Second column content.</p>
        </div>
        <div class="column">
            <h3>Column 3</h3>
            <p>Third column content.</p>
        </div>
    </div>
</body>
```

### Example 5: Icon with text spacing

```html
<style>
    .icon-text-group {
        padding: 20pt;
    }
    .icon {
        display: inline-block;
        margin-right: 10pt;
        width: 20pt;
        height: 20pt;
        background-color: #3b82f6;
        border-radius: 50%;
        vertical-align: middle;
    }
    .text {
        display: inline-block;
        vertical-align: middle;
    }
</style>
<body>
    <div class="icon-text-group">
        <span class="icon"></span>
        <span class="text">Icon with text spacing</span>
    </div>
    <div class="icon-text-group">
        <span class="icon"></span>
        <span class="text">Another icon-text pair</span>
    </div>
</body>
```

### Example 6: Product card grid with right margin

```html
<style>
    .product-grid {
        padding: 25pt;
    }
    .product-card {
        float: left;
        width: 150pt;
        margin-right: 20pt;
        margin-bottom: 20pt;
        padding: 15pt;
        border: 1pt solid #e5e7eb;
        background-color: white;
    }
    .product-card:nth-child(3n) {
        margin-right: 0;
    }
    .product-name {
        margin: 0 0 8pt 0;
        font-weight: bold;
    }
    .product-price {
        margin: 0;
        color: #16a34a;
        font-size: 14pt;
    }
</style>
<body>
    <div class="product-grid">
        <div class="product-card">
            <div class="product-name">Product A</div>
            <div class="product-price">$29.99</div>
        </div>
        <div class="product-card">
            <div class="product-name">Product B</div>
            <div class="product-price">$39.99</div>
        </div>
        <div class="product-card">
            <div class="product-name">Product C</div>
            <div class="product-price">$49.99</div>
        </div>
    </div>
</body>
```

### Example 7: Button group with spacing

```html
<style>
    .button-group {
        padding: 20pt;
    }
    .button {
        display: inline-block;
        margin-right: 10pt;
        padding: 10pt 20pt;
        background-color: #2563eb;
        color: white;
        border: none;
        cursor: pointer;
    }
    .button-secondary {
        background-color: #6b7280;
    }
    .button:last-child {
        margin-right: 0;
    }
</style>
<body>
    <div class="button-group">
        <button class="button">Primary Action</button>
        <button class="button button-secondary">Secondary</button>
        <button class="button button-secondary">Cancel</button>
    </div>
</body>
```

### Example 8: Invoice line items with right margin

```html
<style>
    .invoice {
        padding: 40pt;
    }
    .line-item {
        margin-bottom: 10pt;
        overflow: hidden;
    }
    .item-description {
        float: left;
        width: 60%;
        margin-right: 5%;
    }
    .item-quantity {
        float: left;
        width: 15%;
        margin-right: 5%;
        text-align: center;
    }
    .item-price {
        float: left;
        width: 15%;
        text-align: right;
    }
</style>
<body>
    <div class="invoice">
        <div class="line-item">
            <div class="item-description">Consulting Services</div>
            <div class="item-quantity">10 hrs</div>
            <div class="item-price">$1,500.00</div>
        </div>
        <div class="line-item">
            <div class="item-description">Software License</div>
            <div class="item-quantity">1</div>
            <div class="item-price">$299.00</div>
        </div>
    </div>
</body>
```

### Example 9: Tag list with right margin

```html
<style>
    .tag-list {
        padding: 20pt;
    }
    .tag {
        display: inline-block;
        margin-right: 6pt;
        margin-bottom: 6pt;
        padding: 5pt 12pt;
        background-color: #e0e7ff;
        color: #3730a3;
        border-radius: 4pt;
        font-size: 10pt;
    }
</style>
<body>
    <div class="tag-list">
        <span class="tag">Technology</span>
        <span class="tag">Innovation</span>
        <span class="tag">Design</span>
        <span class="tag">Development</span>
        <span class="tag">Business</span>
        <span class="tag">Strategy</span>
    </div>
</body>
```

### Example 10: Form inline fields with right margin

```html
<style>
    .form-inline {
        padding: 30pt;
    }
    .form-field {
        display: inline-block;
        margin-right: 15pt;
        vertical-align: top;
    }
    .form-field:last-child {
        margin-right: 0;
    }
    .form-field label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
        font-size: 11pt;
    }
    .form-field input {
        padding: 8pt;
        border: 1pt solid #d1d5db;
        width: 150pt;
    }
</style>
<body>
    <div class="form-inline">
        <div class="form-field">
            <label>First Name</label>
            <input type="text" />
        </div>
        <div class="form-field">
            <label>Last Name</label>
            <input type="text" />
        </div>
        <div class="form-field">
            <label>Age</label>
            <input type="text" />
        </div>
    </div>
</body>
```

### Example 11: Stats dashboard with right margin

```html
<style>
    .dashboard {
        padding: 30pt;
    }
    .stat-card {
        float: left;
        width: 140pt;
        margin-right: 20pt;
        margin-bottom: 20pt;
        padding: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        text-align: center;
    }
    .stat-card:nth-child(3n) {
        margin-right: 0;
    }
    .stat-value {
        margin: 0 0 8pt 0;
        font-size: 28pt;
        font-weight: bold;
        color: #1f2937;
    }
    .stat-label {
        margin: 0;
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="dashboard">
        <div class="stat-card">
            <div class="stat-value">1,234</div>
            <div class="stat-label">Total Users</div>
        </div>
        <div class="stat-card">
            <div class="stat-value">567</div>
            <div class="stat-label">Active Now</div>
        </div>
        <div class="stat-card">
            <div class="stat-value">89%</div>
            <div class="stat-label">Satisfaction</div>
        </div>
    </div>
</body>
```

### Example 12: Newsletter layout with image float

```html
<style>
    .newsletter {
        padding: 30pt;
    }
    .article {
        margin-bottom: 25pt;
        overflow: hidden;
    }
    .article-image {
        float: left;
        width: 120pt;
        height: 80pt;
        margin-right: 15pt;
        background-color: #d1d5db;
    }
    .article-content h3 {
        margin: 0 0 8pt 0;
        font-size: 14pt;
    }
    .article-content p {
        margin: 0;
        font-size: 11pt;
        line-height: 1.5;
    }
</style>
<body>
    <div class="newsletter">
        <div class="article">
            <div class="article-image"></div>
            <div class="article-content">
                <h3>Article Title</h3>
                <p>Article summary text flows to the right of the image with proper margin spacing.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 13: Breadcrumb navigation with right margin

```html
<style>
    .breadcrumb {
        padding: 15pt 30pt;
        background-color: #f3f4f6;
    }
    .breadcrumb-item {
        display: inline-block;
        margin-right: 8pt;
        font-size: 11pt;
    }
    .breadcrumb-item:after {
        content: " /";
        margin-left: 8pt;
        color: #6b7280;
    }
    .breadcrumb-item:last-child:after {
        content: "";
    }
    .breadcrumb-item:last-child {
        margin-right: 0;
        font-weight: bold;
    }
</style>
<body>
    <div class="breadcrumb">
        <span class="breadcrumb-item">Home</span>
        <span class="breadcrumb-item">Products</span>
        <span class="breadcrumb-item">Category</span>
        <span class="breadcrumb-item">Item</span>
    </div>
</body>
```

### Example 14: Table with column spacing

```html
<style>
    .data-table {
        margin: 30pt;
        border-collapse: separate;
        border-spacing: 0;
    }
    .data-table th,
    .data-table td {
        padding: 10pt;
        border: 1pt solid #d1d5db;
    }
    .data-table th {
        background-color: #1f2937;
        color: white;
    }
    .data-table td:not(:last-child) {
        margin-right: 0;
    }
    .numeric-column {
        text-align: right;
        padding-right: 15pt;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th>Product</th>
                <th class="numeric-column">Quantity</th>
                <th class="numeric-column">Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget A</td>
                <td class="numeric-column">10</td>
                <td class="numeric-column">$99.99</td>
            </tr>
            <tr>
                <td>Widget B</td>
                <td class="numeric-column">5</td>
                <td class="numeric-column">$149.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 15: Business card with right margin sections

```html
<style>
    .business-card {
        width: 350pt;
        height: 200pt;
        margin: 30pt auto;
        padding: 20pt;
        border: 2pt solid #1e3a8a;
        overflow: hidden;
    }
    .card-left {
        float: left;
        width: 60%;
        margin-right: 5%;
    }
    .card-right {
        float: left;
        width: 35%;
    }
    .card-name {
        margin: 0 0 5pt 0;
        font-size: 18pt;
        font-weight: bold;
    }
    .card-title {
        margin: 0 0 15pt 0;
        font-size: 12pt;
        color: #6b7280;
    }
    .card-company {
        margin: 0 0 8pt 0;
        font-size: 14pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .card-contact {
        margin: 0;
        font-size: 10pt;
    }
</style>
<body>
    <div class="business-card">
        <div class="card-left">
            <div class="card-name">Jane Smith</div>
            <div class="card-title">Senior Account Manager</div>
            <div class="card-contact">
                jane.smith@example.com<br/>
                +1 (555) 123-4567
            </div>
        </div>
        <div class="card-right">
            <div class="card-company">ACME Corp</div>
            <div class="card-contact">
                123 Business St.<br/>
                New York, NY 10001
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [margin](/reference/cssproperties/css_prop_margin) - Set all margins shorthand
- [margin-left](/reference/cssproperties/css_prop_margin-left) - Set left margin
- [margin-top](/reference/cssproperties/css_prop_margin-top) - Set top margin
- [margin-bottom](/reference/cssproperties/css_prop_margin-bottom) - Set bottom margin
- [padding-right](/reference/cssproperties/css_prop_padding-right) - Set right padding
- [margin-inline-end](/reference/cssproperties/css_prop_margin-inline-end) - Set inline end margin
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
