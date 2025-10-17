---
layout: default
title: max-width
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# max-width : Maximum Width Property

The `max-width` property sets the maximum horizontal dimension of an element in PDF documents. It ensures that an element will never be wider than the specified value, even if its content or other constraints would make it larger. This property is essential for responsive layouts, readability optimization, and preventing content from extending beyond desired boundaries.

## Usage

```css
selector {
    max-width: value;
}
```

The max-width property accepts a single value that defines the largest allowed width for the element. If the calculated or specified width would be larger, the max-width value is used instead.

---

## Supported Values

### Length Units
- Points: `200pt`, `400pt`, `600pt`
- Pixels: `200px`, `400px`, `600px`
- Inches: `3in`, `6in`, `8in`
- Centimeters: `10cm`, `15cm`, `20cm`
- Millimeters: `100mm`, `150mm`, `200mm`
- Ems: `30em`, `40em`, `60em`
- Percentage: `50%`, `80%`, `100%` (relative to parent width)

### Special Values
- `none` - No maximum width constraint (default)

---

## Supported Elements

The `max-width` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Images (`<img>`)
- Tables (`<table>`)
- Table cells (`<td>`, `<th>`)
- Form inputs (`<input>`, `<textarea>`, `<select>`)
- All elements with `display: block`, `display: inline-block`, or `display: table`

---

## Notes

- `max-width` overrides `width` if width is larger than max-width
- When both `min-width` and `max-width` are specified, and min-width is larger, min-width wins
- Percentage values are calculated relative to the parent element's width
- Essential for creating readable text content by limiting line length
- Particularly useful for responsive layouts that adapt to different page sizes
- Prevents images and media from overflowing their containers
- Does not affect inline elements unless they are `display: inline-block`
- In PDF generation, max-width helps ensure content fits within page margins
- Commonly used with `margin: 0 auto` to center content with maximum width constraint
- Works in conjunction with `width` and `min-width` to create flexible yet constrained layouts
- Optimal text line length for readability is typically 50-75 characters (~500-600pt)

---

## Data Binding

The max-width property supports dynamic value binding through template expressions, allowing maximum width constraints to be set from data sources at runtime.

### Example 1: Content sections with configurable maximum widths

```html
<style>
    .content-wrapper {
        margin: 0 auto;
        padding: 30pt;
        background-color: white;
    }
    .content-wrapper h1 {
        margin: 0 0 20pt 0;
        font-size: 24pt;
    }
    .content-wrapper p {
        margin: 0 0 15pt 0;
        line-height: 1.6;
    }
</style>
<body>
    <div class="content-wrapper" style="max-width: {{layout.contentMaxWidth}}pt">
        <h1>{{article.title}}</h1>
        <p>{{article.content}}</p>
    </div>
</body>
```

### Example 2: Images with data-driven maximum widths

```html
<style>
    .image-container {
        padding: 20pt;
        text-align: center;
    }
    .responsive-image {
        height: auto;
        display: block;
        margin: 0 auto;
        border: 2pt solid #e5e7eb;
    }
</style>
<body>
    <div class="image-container">
        <img class="responsive-image"
             src="{{image.url}}"
             style="max-width: {{image.size === 'large' ? '700pt' : '400pt'}}"
             alt="{{image.caption}}" />
    </div>
</body>
```

### Example 3: Cards with conditional maximum widths

```html
<style>
    .card-grid {
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
        margin: 0 0 10pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="card-grid">
        <div class="card" style="max-width: {{card.featured ? '400pt' : '300pt'}}">
            <h3 class="card-title">{{card.title}}</h3>
            <p>{{card.description}}</p>
        </div>
    </div>
</body>
```

### Example 4: Table with configurable maximum width

```html
<style>
    .table-container {
        padding: 20pt;
    }
    .data-table {
        width: 100%;
        margin: 0 auto;
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
    <div class="table-container">
        <table class="data-table" style="max-width: {{config.tableMaxWidth}}pt">
            <thead>
                <tr>
                    <th>Product</th>
                    <th>Price</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>{{product.name}}</td>
                    <td>{{product.price}}</td>
                    <td>{{product.status}}</td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
```

---

## Examples

### Example 1: Centered content with maximum width

```html
<style>
    .content-wrapper {
        max-width: 600pt;
        margin: 0 auto;
        padding: 30pt;
        background-color: white;
    }
    .content-wrapper h1 {
        margin: 0 0 20pt 0;
        font-size: 24pt;
    }
    .content-wrapper p {
        margin: 0 0 15pt 0;
        line-height: 1.6;
    }
</style>
<body>
    <div class="content-wrapper">
        <h1>Article Title</h1>
        <p>Content is constrained to 600pt maximum width for optimal readability. This prevents lines from becoming too long on wide pages.</p>
        <p>Additional paragraphs maintain the same width constraint.</p>
    </div>
</body>
```

### Example 2: Responsive image sizing

```html
<style>
    .image-container {
        padding: 20pt;
    }
    .responsive-image {
        max-width: 100%;
        height: auto;
        display: block;
        margin: 0 auto;
    }
    .thumbnail {
        max-width: 150pt;
        height: auto;
        border: 2pt solid #e5e7eb;
        margin: 10pt;
    }
    .hero-image {
        max-width: 700pt;
        width: 100%;
        height: auto;
        display: block;
    }
</style>
<body>
    <div class="image-container">
        <img class="hero-image" src="banner.jpg" alt="Banner" />
        <img class="thumbnail" src="thumb1.jpg" alt="Thumbnail 1" />
        <img class="thumbnail" src="thumb2.jpg" alt="Thumbnail 2" />
    </div>
</body>
```

### Example 3: Form with maximum width

```html
<style>
    .form-container {
        max-width: 450pt;
        margin: 40pt auto;
        padding: 30pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        box-sizing: border-box;
    }
    .form-title {
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
    }
    .form-input {
        width: 100%;
        padding: 10pt;
        border: 1pt solid #d1d5db;
        box-sizing: border-box;
    }
</style>
<body>
    <div class="form-container">
        <h2 class="form-title">Contact Form</h2>
        <div class="form-group">
            <label class="form-label">Name</label>
            <input class="form-input" type="text" />
        </div>
        <div class="form-group">
            <label class="form-label">Email</label>
            <input class="form-input" type="email" />
        </div>
    </div>
</body>
```

### Example 4: Card grid with maximum card width

```html
<style>
    .card-grid {
        padding: 20pt;
        display: flex;
        flex-wrap: wrap;
        gap: 15pt;
    }
    .card {
        max-width: 300pt;
        flex: 1 1 250pt;
        padding: 20pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        box-sizing: border-box;
    }
    .card-image {
        width: 100%;
        height: 150pt;
        background-color: #f3f4f6;
        margin-bottom: 12pt;
    }
    .card-title {
        margin: 0 0 10pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="card-grid">
        <div class="card">
            <div class="card-image"></div>
            <h3 class="card-title">Card One</h3>
            <p>Card content with maximum width constraint.</p>
        </div>
        <div class="card">
            <div class="card-image"></div>
            <h3 class="card-title">Card Two</h3>
            <p>Cards won't exceed 300pt width.</p>
        </div>
    </div>
</body>
```

### Example 5: Table with maximum width

```html
<style>
    .table-container {
        padding: 20pt;
    }
    .data-table {
        max-width: 700pt;
        width: 100%;
        margin: 0 auto;
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
    <div class="table-container">
        <table class="data-table">
            <thead>
                <tr>
                    <th>Product</th>
                    <th>Price</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Widget A</td>
                    <td>$29.99</td>
                    <td>Available</td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
```

### Example 6: Alert box with maximum width

```html
<style>
    .alert-container {
        padding: 20pt;
    }
    .alert {
        max-width: 550pt;
        margin: 0 auto 15pt;
        padding: 15pt;
        border-radius: 4pt;
        border-left: 4pt solid;
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
    <div class="alert-container">
        <div class="alert alert-info">
            <strong>Info:</strong> System update completed successfully.
        </div>
        <div class="alert alert-success">
            <strong>Success:</strong> Your changes have been saved.
        </div>
        <div class="alert alert-warning">
            <strong>Warning:</strong> Password expires in 7 days.
        </div>
    </div>
</body>
```

### Example 7: Invoice with maximum width

```html
<style>
    .invoice {
        max-width: 600pt;
        margin: 40pt auto;
        padding: 30pt;
        border: 2pt solid #000;
        background-color: white;
        box-sizing: border-box;
    }
    .invoice-header {
        margin-bottom: 30pt;
        padding-bottom: 15pt;
        border-bottom: 2pt solid #e5e7eb;
    }
    .invoice-title {
        margin: 0;
        font-size: 28pt;
        color: #1e40af;
    }
    .line-items {
        width: 100%;
        margin: 20pt 0;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1 class="invoice-title">INVOICE</h1>
            <p>Invoice #INV-2025-001</p>
        </div>
        <table class="line-items">
            <tr>
                <td>Consulting Services</td>
                <td>$1,500.00</td>
            </tr>
        </table>
    </div>
</body>
```

### Example 8: Modal dialog with maximum width

```html
<style>
    .modal-overlay {
        padding: 40pt;
        background-color: rgba(0, 0, 0, 0.5);
    }
    .modal {
        max-width: 500pt;
        margin: 0 auto;
        padding: 30pt;
        background-color: white;
        border-radius: 8pt;
        box-shadow: 0 4pt 6pt rgba(0, 0, 0, 0.1);
        box-sizing: border-box;
    }
    .modal-header {
        margin: 0 0 20pt 0;
        font-size: 20pt;
        font-weight: bold;
    }
    .modal-body {
        margin-bottom: 20pt;
        line-height: 1.6;
    }
    .modal-footer {
        text-align: right;
    }
    .modal-button {
        padding: 10pt 20pt;
        margin-left: 10pt;
        background-color: #2563eb;
        color: white;
        border: none;
    }
</style>
<body>
    <div class="modal-overlay">
        <div class="modal">
            <h2 class="modal-header">Confirm Action</h2>
            <div class="modal-body">
                <p>Are you sure you want to proceed with this action?</p>
            </div>
            <div class="modal-footer">
                <button class="modal-button">Cancel</button>
                <button class="modal-button">Confirm</button>
            </div>
        </div>
    </div>
</body>
```

### Example 9: Quote block with maximum width

```html
<style>
    .article-content {
        padding: 40pt;
    }
    .article-content p {
        max-width: 650pt;
        margin: 0 auto 15pt;
        line-height: 1.6;
    }
    .quote-block {
        max-width: 550pt;
        margin: 30pt auto;
        padding: 20pt 25pt;
        background-color: #f5f5f5;
        border-left: 5pt solid #6366f1;
        font-style: italic;
    }
    .quote-text {
        margin: 0 0 10pt 0;
        font-size: 14pt;
    }
    .quote-author {
        margin: 0;
        font-size: 11pt;
        color: #6b7280;
        text-align: right;
    }
</style>
<body>
    <div class="article-content">
        <p>Regular article text with constrained width for readability.</p>
        <div class="quote-block">
            <p class="quote-text">"Simplicity is the ultimate sophistication."</p>
            <p class="quote-author">— Leonardo da Vinci</p>
        </div>
        <p>Article continues with consistent width constraint.</p>
    </div>
</body>
```

### Example 10: Search results with maximum width

```html
<style>
    .search-results {
        max-width: 700pt;
        margin: 0 auto;
        padding: 20pt;
    }
    .search-header {
        margin-bottom: 20pt;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #e5e7eb;
    }
    .result-item {
        max-width: 100%;
        padding: 15pt;
        margin-bottom: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .result-title {
        margin: 0 0 8pt 0;
        font-size: 16pt;
        font-weight: bold;
        color: #2563eb;
    }
    .result-description {
        margin: 0;
        color: #6b7280;
        line-height: 1.5;
    }
</style>
<body>
    <div class="search-results">
        <div class="search-header">
            <h2>Search Results</h2>
            <p>Found 3 results</p>
        </div>
        <div class="result-item">
            <h3 class="result-title">Result One</h3>
            <p class="result-description">Description of the first search result.</p>
        </div>
        <div class="result-item">
            <h3 class="result-title">Result Two</h3>
            <p class="result-description">Description of the second search result.</p>
        </div>
    </div>
</body>
```

### Example 11: Newsletter with maximum width

```html
<style>
    .newsletter {
        max-width: 600pt;
        margin: 0 auto;
        background-color: white;
    }
    .newsletter-header {
        padding: 30pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
    .newsletter-content {
        padding: 30pt;
    }
    .newsletter-section {
        max-width: 100%;
        margin-bottom: 25pt;
    }
    .section-title {
        margin: 0 0 12pt 0;
        font-size: 18pt;
        font-weight: bold;
    }
    .section-text {
        margin: 0;
        line-height: 1.6;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>Monthly Newsletter</h1>
            <p>January 2025</p>
        </div>
        <div class="newsletter-content">
            <div class="newsletter-section">
                <h2 class="section-title">Feature Story</h2>
                <p class="section-text">Newsletter content maintains optimal width for email and PDF viewing.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 12: Product details with maximum width

```html
<style>
    .product-page {
        max-width: 800pt;
        margin: 30pt auto;
        padding: 30pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .product-header {
        max-width: 100%;
        margin-bottom: 25pt;
        padding-bottom: 15pt;
        border-bottom: 2pt solid #e5e7eb;
    }
    .product-title {
        margin: 0 0 10pt 0;
        font-size: 28pt;
        font-weight: bold;
    }
    .product-image {
        max-width: 400pt;
        width: 100%;
        height: auto;
        margin: 0 auto 20pt;
        display: block;
    }
    .product-description {
        max-width: 650pt;
        margin: 0 auto;
        line-height: 1.6;
    }
</style>
<body>
    <div class="product-page">
        <div class="product-header">
            <h1 class="product-title">Premium Widget</h1>
            <p>SKU: WGT-PRO-001</p>
        </div>
        <img class="product-image" src="product.jpg" alt="Product" />
        <div class="product-description">
            <p>Detailed product description with optimal width for reading.</p>
        </div>
    </div>
</body>
```

### Example 13: Dashboard with maximum width sections

```html
<style>
    .dashboard {
        max-width: 900pt;
        margin: 0 auto;
        padding: 20pt;
    }
    .dashboard-header {
        max-width: 100%;
        margin-bottom: 25pt;
    }
    .metrics-row {
        display: flex;
        gap: 15pt;
        margin-bottom: 20pt;
    }
    .metric-card {
        flex: 1;
        max-width: 280pt;
        padding: 20pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .metric-value {
        font-size: 32pt;
        font-weight: bold;
        color: #1e40af;
    }
</style>
<body>
    <div class="dashboard">
        <div class="dashboard-header">
            <h1>Dashboard</h1>
        </div>
        <div class="metrics-row">
            <div class="metric-card">
                <div class="metric-value">$52K</div>
                <div>Revenue</div>
            </div>
            <div class="metric-card">
                <div class="metric-value">342</div>
                <div>Orders</div>
            </div>
        </div>
    </div>
</body>
```

### Example 14: Certificate with maximum width

```html
<style>
    .certificate-container {
        padding: 40pt;
    }
    .certificate {
        max-width: 700pt;
        margin: 0 auto;
        padding: 40pt;
        border: 10pt double #1e3a8a;
        background-color: #fffef7;
        text-align: center;
        box-sizing: border-box;
    }
    .certificate-inner {
        padding: 30pt;
        border: 2pt solid #1e3a8a;
    }
    .certificate-title {
        margin: 0 0 30pt 0;
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .recipient-name {
        max-width: 500pt;
        margin: 0 auto 20pt;
        font-size: 24pt;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #000;
    }
</style>
<body>
    <div class="certificate-container">
        <div class="certificate">
            <div class="certificate-inner">
                <h1 class="certificate-title">Certificate of Completion</h1>
                <div class="recipient-name">Jane Smith</div>
                <p>Has successfully completed the training program</p>
            </div>
        </div>
    </div>
</body>
```

### Example 15: Pricing table with maximum width

```html
<style>
    .pricing-section {
        max-width: 900pt;
        margin: 40pt auto;
        padding: 30pt;
    }
    .pricing-header {
        max-width: 650pt;
        margin: 0 auto 30pt;
        text-align: center;
    }
    .pricing-cards {
        display: flex;
        gap: 20pt;
        justify-content: center;
    }
    .pricing-card {
        max-width: 280pt;
        flex: 1;
        padding: 25pt;
        background-color: white;
        border: 2pt solid #e5e7eb;
        text-align: center;
    }
    .price-amount {
        font-size: 36pt;
        font-weight: bold;
        color: #16a34a;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="pricing-section">
        <div class="pricing-header">
            <h1>Choose Your Plan</h1>
            <p>Select the perfect plan for your needs</p>
        </div>
        <div class="pricing-cards">
            <div class="pricing-card">
                <h3>Basic</h3>
                <div class="price-amount">$29</div>
                <p>Perfect for individuals</p>
            </div>
            <div class="pricing-card">
                <h3>Pro</h3>
                <div class="price-amount">$99</div>
                <p>Best for teams</p>
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [width](/reference/cssproperties/css_prop_width) - Set element width
- [min-width](/reference/cssproperties/css_prop_min-width) - Set minimum width constraint
- [max-height](/reference/cssproperties/css_prop_max-height) - Set maximum height constraint
- [min-height](/reference/cssproperties/css_prop_min-height) - Set minimum height constraint
- [padding](/reference/cssproperties/css_prop_padding) - Set padding shorthand property
- [margin](/reference/cssproperties/css_prop_margin) - Set margin shorthand property
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
