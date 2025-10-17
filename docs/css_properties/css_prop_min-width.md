---
layout: default
title: min-width
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# min-width : Minimum Width Property

The `min-width` property sets the minimum horizontal dimension of an element in PDF documents. It ensures that an element will never be narrower than the specified value, even if its content or other constraints would make it smaller. This property is essential for maintaining minimum readability and layout integrity.

## Usage

```css
selector {
    min-width: value;
}
```

The min-width property accepts a single value that defines the smallest allowed width for the element. If the calculated or specified width would be smaller, the min-width value is used instead.

---

## Supported Values

### Length Units
- Points: `50pt`, `100pt`, `200pt`
- Pixels: `50px`, `100px`, `200px`
- Inches: `1in`, `2in`, `3in`
- Centimeters: `2cm`, `5cm`, `10cm`
- Millimeters: `20mm`, `50mm`, `100mm`
- Ems: `5em`, `10em`, `20em`
- Percentage: `25%`, `50%`, `75%` (relative to parent width)

### Special Values
- `0` - No minimum width constraint (default)
- `auto` - Calculated based on content (in some contexts)

---

## Supported Elements

The `min-width` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Images (`<img>`)
- Tables (`<table>`)
- Table cells (`<td>`, `<th>`)
- Form inputs (`<input>`, `<select>`)
- All elements with `display: block`, `display: inline-block`, or `display: table`

---

## Notes

- `min-width` overrides `width` if width is smaller than min-width
- When both `min-width` and `max-width` are specified, and min-width is larger, min-width wins
- Percentage values are calculated relative to the parent element's width
- Essential for responsive layouts where elements need minimum dimensions for readability
- Particularly useful for form inputs to ensure adequate space for user input
- Helps prevent layout collapse when content is minimal or absent
- Does not affect inline elements unless they are `display: inline-block`
- In PDF generation, min-width ensures consistent sizing across different content scenarios
- Useful for maintaining column widths in tables when content varies
- Works in conjunction with `width` and `max-width` to create flexible yet constrained layouts

---

## Data Binding

The min-width property supports dynamic value binding through template expressions, allowing minimum width constraints to be set from data sources at runtime.

### Example 1: Cards with data-driven minimum widths

```html
<style>
    .card-container {
        padding: 20pt;
    }
    .card {
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
    }
    .card-title {
        font-size: 14pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="card-container">
        <div class="card" style="min-width: {{card.minWidth}}pt; max-width: {{card.maxWidth}}pt">
            <h3 class="card-title">{{card.title}}</h3>
            <p>{{card.content}}</p>
        </div>
    </div>
</body>
```

### Example 2: Buttons with conditional minimum widths

```html
<style>
    .button {
        padding: 10pt 20pt;
        background-color: #2563eb;
        color: white;
        border: none;
        text-align: center;
        margin: 5pt;
    }
</style>
<body>
    <button class="button" style="min-width: {{button.priority === 'primary' ? '150pt' : '100pt'}}">
        {{button.label}}
    </button>
</body>
```

### Example 3: Table columns with variable minimum widths

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
        background-color: #1f2937;
        color: white;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th style="min-width: {{columns.id.minWidth}}pt">ID</th>
                <th style="min-width: {{columns.name.minWidth}}pt">Name</th>
                <th style="min-width: {{columns.status.minWidth}}pt">Status</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>{{row.id}}</td>
                <td>{{row.name}}</td>
                <td>{{row.status}}</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 4: Form inputs with responsive minimum widths

```html
<style>
    .form-group {
        margin-bottom: 15pt;
    }
    .form-label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
    }
    .form-input {
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="form-group">
        <label class="form-label">{{field.label}}</label>
        <input class="form-input"
               type="text"
               style="min-width: {{field.minWidth}}pt; max-width: {{field.maxWidth}}pt" />
    </div>
</body>
```

---

## Examples

### Example 1: Minimum width for buttons

```html
<style>
    .button {
        min-width: 100pt;
        padding: 10pt 20pt;
        background-color: #2563eb;
        color: white;
        border: none;
        text-align: center;
        margin: 5pt;
    }
    .button-small {
        min-width: 60pt;
    }
    .button-large {
        min-width: 150pt;
    }
</style>
<body>
    <button class="button button-small">OK</button>
    <button class="button">Submit Form</button>
    <button class="button button-large">Continue to Checkout</button>
</body>
```

### Example 2: Form inputs with minimum width

```html
<style>
    .form-group {
        margin-bottom: 15pt;
    }
    .form-label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
    }
    .form-input {
        min-width: 200pt;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .input-short {
        min-width: 80pt;
        max-width: 120pt;
    }
    .input-medium {
        min-width: 150pt;
    }
    .input-long {
        min-width: 300pt;
    }
</style>
<body>
    <div class="form-group">
        <label class="form-label">Postal Code</label>
        <input class="form-input input-short" type="text" />
    </div>
    <div class="form-group">
        <label class="form-label">City</label>
        <input class="form-input input-medium" type="text" />
    </div>
    <div class="form-group">
        <label class="form-label">Full Address</label>
        <input class="form-input input-long" type="text" />
    </div>
</body>
```

### Example 3: Table column minimum widths

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
        background-color: #1f2937;
        color: white;
    }
    .col-id {
        min-width: 50pt;
        width: 50pt;
    }
    .col-name {
        min-width: 150pt;
    }
    .col-status {
        min-width: 80pt;
    }
    .col-date {
        min-width: 100pt;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th class="col-id">ID</th>
                <th class="col-name">Name</th>
                <th class="col-status">Status</th>
                <th class="col-date">Date</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>1</td>
                <td>Product Alpha</td>
                <td>Active</td>
                <td>2025-01-15</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 4: Responsive cards with minimum width

```html
<style>
    .card-container {
        padding: 20pt;
    }
    .card {
        min-width: 200pt;
        max-width: 300pt;
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
    }
    .card-image {
        width: 100%;
        height: 120pt;
        background-color: #e5e7eb;
        margin-bottom: 12pt;
    }
    .card-title {
        margin: 0 0 10pt 0;
        font-size: 14pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="card-container">
        <div class="card">
            <div class="card-image"></div>
            <h3 class="card-title">Card A</h3>
            <p>Card content maintains minimum width.</p>
        </div>
        <div class="card">
            <div class="card-image"></div>
            <h3 class="card-title">Card B</h3>
            <p>Even with minimal content.</p>
        </div>
    </div>
</body>
```

### Example 5: Navigation menu items

```html
<style>
    .nav-menu {
        background-color: #1f2937;
        padding: 10pt;
    }
    .nav-item {
        display: inline-block;
        min-width: 80pt;
        padding: 10pt 15pt;
        margin: 0 5pt;
        background-color: #374151;
        color: white;
        text-align: center;
        text-decoration: none;
    }
    .nav-item-wide {
        min-width: 120pt;
    }
</style>
<body>
    <div class="nav-menu">
        <a class="nav-item" href="#">Home</a>
        <a class="nav-item" href="#">About</a>
        <a class="nav-item nav-item-wide" href="#">Products</a>
        <a class="nav-item nav-item-wide" href="#">Contact Us</a>
    </div>
</body>
```

### Example 6: Sidebar with minimum width

```html
<style>
    .layout-wrapper {
        padding: 20pt;
    }
    .sidebar {
        min-width: 180pt;
        width: 25%;
        float: left;
        padding: 15pt;
        background-color: #f3f4f6;
        border-right: 2pt solid #e5e7eb;
        box-sizing: border-box;
    }
    .content-area {
        margin-left: calc(25% + 10pt);
        padding: 15pt;
    }
    .sidebar h3 {
        margin: 0 0 15pt 0;
        font-size: 14pt;
    }
</style>
<body>
    <div class="layout-wrapper">
        <div class="sidebar">
            <h3>Navigation</h3>
            <ul>
                <li>Dashboard</li>
                <li>Reports</li>
                <li>Settings</li>
            </ul>
        </div>
        <div class="content-area">
            <h2>Main Content</h2>
            <p>Sidebar maintains minimum width even in responsive layouts.</p>
        </div>
    </div>
</body>
```

### Example 7: Badge and label elements

```html
<style>
    .content {
        padding: 20pt;
    }
    .badge {
        display: inline-block;
        min-width: 50pt;
        padding: 5pt 10pt;
        margin: 3pt;
        background-color: #dbeafe;
        color: #1e40af;
        border-radius: 3pt;
        text-align: center;
        font-size: 10pt;
    }
    .badge-success {
        background-color: #dcfce7;
        color: #16a34a;
    }
    .badge-warning {
        background-color: #fef3c7;
        color: #f59e0b;
    }
    .badge-error {
        background-color: #fee2e2;
        color: #dc2626;
    }
</style>
<body>
    <div class="content">
        <p>Order Status: <span class="badge badge-success">Paid</span></p>
        <p>Priority: <span class="badge badge-warning">High</span></p>
        <p>Tags: <span class="badge">New</span> <span class="badge">Featured</span></p>
    </div>
</body>
```

### Example 8: Price display with minimum width

```html
<style>
    .pricing-table {
        width: 100%;
        border-collapse: collapse;
        margin: 20pt 0;
    }
    .pricing-table th,
    .pricing-table td {
        padding: 12pt;
        border: 1pt solid #d1d5db;
    }
    .pricing-table th {
        background-color: #1f2937;
        color: white;
    }
    .price-cell {
        min-width: 100pt;
        text-align: right;
        font-weight: bold;
        color: #16a34a;
    }
    .quantity-cell {
        min-width: 60pt;
        text-align: center;
    }
</style>
<body>
    <table class="pricing-table">
        <thead>
            <tr>
                <th>Product</th>
                <th class="quantity-cell">Qty</th>
                <th class="price-cell">Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget A</td>
                <td class="quantity-cell">5</td>
                <td class="price-cell">$99.95</td>
            </tr>
            <tr>
                <td>Widget B</td>
                <td class="quantity-cell">12</td>
                <td class="price-cell">$1,245.00</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 9: Search bar with minimum width

```html
<style>
    .search-container {
        padding: 20pt;
        background-color: #f3f4f6;
    }
    .search-wrapper {
        display: flex;
        align-items: center;
    }
    .search-input {
        min-width: 250pt;
        flex: 1;
        padding: 10pt;
        border: 1pt solid #d1d5db;
        border-right: none;
    }
    .search-button {
        min-width: 80pt;
        padding: 10pt 20pt;
        background-color: #2563eb;
        color: white;
        border: 1pt solid #2563eb;
    }
</style>
<body>
    <div class="search-container">
        <div class="search-wrapper">
            <input class="search-input" type="text" placeholder="Search..." />
            <button class="search-button">Search</button>
        </div>
    </div>
</body>
```

### Example 10: Status indicators with consistent sizing

```html
<style>
    .status-list {
        padding: 20pt;
    }
    .status-item {
        margin-bottom: 10pt;
        display: flex;
        align-items: center;
    }
    .status-badge {
        min-width: 90pt;
        padding: 6pt 12pt;
        margin-right: 15pt;
        text-align: center;
        font-weight: bold;
        font-size: 10pt;
        border-radius: 3pt;
    }
    .status-active {
        background-color: #dcfce7;
        color: #16a34a;
    }
    .status-pending {
        background-color: #fef3c7;
        color: #f59e0b;
    }
    .status-inactive {
        background-color: #f3f4f6;
        color: #6b7280;
    }
</style>
<body>
    <div class="status-list">
        <div class="status-item">
            <span class="status-badge status-active">Active</span>
            <span>Service is running</span>
        </div>
        <div class="status-item">
            <span class="status-badge status-pending">Pending</span>
            <span>Awaiting approval</span>
        </div>
        <div class="status-item">
            <span class="status-badge status-inactive">Inactive</span>
            <span>Service is stopped</span>
        </div>
    </div>
</body>
```

### Example 11: Product SKU codes with minimum width

```html
<style>
    .product-list {
        padding: 20pt;
    }
    .product-item {
        margin-bottom: 15pt;
        padding: 15pt;
        border: 1pt solid #e5e7eb;
        background-color: white;
    }
    .product-sku {
        display: inline-block;
        min-width: 100pt;
        padding: 5pt 10pt;
        margin-right: 15pt;
        background-color: #f3f4f6;
        color: #1f2937;
        font-family: monospace;
        font-size: 11pt;
        border: 1pt solid #d1d5db;
    }
    .product-name {
        font-weight: bold;
        font-size: 14pt;
    }
</style>
<body>
    <div class="product-list">
        <div class="product-item">
            <span class="product-sku">WGT-001</span>
            <span class="product-name">Standard Widget</span>
        </div>
        <div class="product-item">
            <span class="product-sku">PRO-2025-A</span>
            <span class="product-name">Professional Widget Pro</span>
        </div>
    </div>
</body>
```

### Example 12: Metric cards with minimum dimensions

```html
<style>
    .metrics-grid {
        padding: 20pt;
    }
    .metric-card {
        min-width: 160pt;
        width: 23%;
        float: left;
        margin: 1%;
        padding: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        box-sizing: border-box;
    }
    .metric-label {
        font-size: 10pt;
        color: #6b7280;
        text-transform: uppercase;
        margin-bottom: 8pt;
    }
    .metric-value {
        font-size: 24pt;
        font-weight: bold;
        color: #1e40af;
    }
</style>
<body>
    <div class="metrics-grid">
        <div class="metric-card">
            <div class="metric-label">Revenue</div>
            <div class="metric-value">$52K</div>
        </div>
        <div class="metric-card">
            <div class="metric-label">Orders</div>
            <div class="metric-value">348</div>
        </div>
        <div class="metric-card">
            <div class="metric-label">Users</div>
            <div class="metric-value">1.2K</div>
        </div>
    </div>
</body>
```

### Example 13: Tag list with minimum tag width

```html
<style>
    .tag-container {
        padding: 20pt;
    }
    .tag {
        display: inline-block;
        min-width: 60pt;
        padding: 6pt 12pt;
        margin: 4pt;
        background-color: #e5e7eb;
        color: #1f2937;
        border-radius: 4pt;
        text-align: center;
        font-size: 10pt;
    }
    .tag-primary {
        background-color: #dbeafe;
        color: #1e40af;
    }
    .tag-secondary {
        background-color: #f3e8ff;
        color: #7e22ce;
    }
</style>
<body>
    <div class="tag-container">
        <h3>Article Tags:</h3>
        <span class="tag tag-primary">CSS</span>
        <span class="tag tag-primary">PDF</span>
        <span class="tag tag-secondary">Tutorial</span>
        <span class="tag">Layout</span>
        <span class="tag">Design</span>
    </div>
</body>
```

### Example 14: Login form with minimum widths

```html
<style>
    .login-container {
        min-width: 320pt;
        max-width: 400pt;
        margin: 40pt auto;
        padding: 30pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        box-sizing: border-box;
    }
    .login-title {
        margin: 0 0 25pt 0;
        text-align: center;
        font-size: 20pt;
    }
    .form-group {
        margin-bottom: 15pt;
    }
    .form-label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
        font-size: 11pt;
    }
    .form-input {
        min-width: 100%;
        padding: 10pt;
        border: 1pt solid #d1d5db;
        box-sizing: border-box;
    }
    .login-button {
        min-width: 100%;
        padding: 12pt;
        background-color: #2563eb;
        color: white;
        border: none;
        font-weight: bold;
        box-sizing: border-box;
    }
</style>
<body>
    <div class="login-container">
        <h2 class="login-title">Login</h2>
        <div class="form-group">
            <label class="form-label">Username</label>
            <input class="form-input" type="text" />
        </div>
        <div class="form-group">
            <label class="form-label">Password</label>
            <input class="form-input" type="password" />
        </div>
        <button class="login-button">Sign In</button>
    </div>
</body>
```

### Example 15: Breadcrumb navigation with minimum widths

```html
<style>
    .breadcrumb-nav {
        padding: 15pt 20pt;
        background-color: #f9fafb;
        border-bottom: 1pt solid #e5e7eb;
    }
    .breadcrumb {
        display: flex;
        align-items: center;
    }
    .breadcrumb-item {
        min-width: 60pt;
        padding: 5pt 10pt;
        margin-right: 5pt;
        text-align: center;
        background-color: white;
        border: 1pt solid #e5e7eb;
        text-decoration: none;
        color: #1f2937;
    }
    .breadcrumb-separator {
        margin: 0 5pt;
        color: #6b7280;
    }
    .breadcrumb-item-active {
        background-color: #dbeafe;
        color: #1e40af;
        font-weight: bold;
    }
</style>
<body>
    <div class="breadcrumb-nav">
        <div class="breadcrumb">
            <a class="breadcrumb-item" href="#">Home</a>
            <span class="breadcrumb-separator">›</span>
            <a class="breadcrumb-item" href="#">Products</a>
            <span class="breadcrumb-separator">›</span>
            <span class="breadcrumb-item breadcrumb-item-active">Widget A</span>
        </div>
    </div>
</body>
```

---

## See Also

- [width](/reference/cssproperties/css_prop_width) - Set element width
- [max-width](/reference/cssproperties/css_prop_max-width) - Set maximum width constraint
- [min-height](/reference/cssproperties/css_prop_min-height) - Set minimum height constraint
- [max-height](/reference/cssproperties/css_prop_max-height) - Set maximum height constraint
- [padding](/reference/cssproperties/css_prop_padding) - Set padding shorthand property
- [margin](/reference/cssproperties/css_prop_margin) - Set margin shorthand property
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
