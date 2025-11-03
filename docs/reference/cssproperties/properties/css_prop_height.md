---
layout: default
title: height
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# height : Height Property

The `height` property sets the vertical dimension of an element in PDF documents. It defines how tall an element should be, affecting its layout, content overflow behavior, and how it interacts with other elements in the document flow.

## Usage

```css
selector {
    height: value;
}
```

The height property accepts a single value that determines the element's height. By default, height applies to the content area only, but this can be modified with the `box-sizing` property.

---

## Supported Values

### Length Units
- Points: `100pt`, `200pt`, `400pt`
- Pixels: `100px`, `200px`, `400px`
- Inches: `2in`, `3in`, `6in`
- Centimeters: `5cm`, `10cm`, `15cm`
- Millimeters: `50mm`, `100mm`, `150mm`
- Ems: `10em`, `20em`, `30em`
- Percentage: `50%`, `75%`, `100%` (relative to parent height)

### Special Values
- `auto` - Browser/renderer calculates height automatically based on content (default)

---

## Supported Elements

The `height` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Images (`<img>`)
- Tables (`<table>`)
- Table cells (`<td>`, `<th>`)
- Table rows (`<tr>`)
- Form inputs (`<input>`, `<textarea>`, `<select>`)
- Canvas elements
- All replaced and non-replaced block-level elements

---

## Notes

- By default, `height` applies to the content box only (padding and border are added outside)
- Use `box-sizing: border-box` to include padding and border in the height calculation
- Percentage heights require the parent element to have an explicit height
- The `auto` value allows the element to automatically size based on its content
- Height does not apply to inline elements (use `display: block` or `display: inline-block`)
- In PDF generation, explicit heights help control page breaks and ensure predictable layouts
- Consider page height constraints for PDF documents (standard A4 is ~842pt tall)
- Height interacts with `min-height` and `max-height` constraints - the final height will respect these boundaries
- Setting height on images maintains aspect ratio if width is not specified
- If content exceeds the specified height, it may overflow or be clipped depending on `overflow` property
- For table cells, height acts as a minimum height - cells expand if content requires more space

---

## Data Binding

The height property supports dynamic value binding through template expressions, allowing heights to be set from data sources at runtime.

### Example 1: Chart bars with dynamic heights

```html
<style>
    .chart-container {
        padding: 20pt;
        display: flex;
        align-items: flex-end;
        height: 300pt;
    }
    .chart-bar {
        width: 60pt;
        margin: 0 10pt;
        background-color: #3b82f6;
        display: flex;
        flex-direction: column;
        justify-content: flex-end;
    }
    .bar-value {
        text-align: center;
        padding: 5pt;
        color: white;
        font-weight: bold;
    }
    .bar-label {
        text-align: center;
        margin-top: 10pt;
        font-size: 10pt;
    }
</style>
<body>
    <div class="chart-container">
        <div>
            <div class="chart-bar" style="height: {{data.jan * 2}}pt">
                <div class="bar-value">{{data.jan}}</div>
            </div>
            <div class="bar-label">Jan</div>
        </div>
        <div>
            <div class="chart-bar" style="height: {{data.feb * 2}}pt">
                <div class="bar-value">{{data.feb}}</div>
            </div>
            <div class="bar-label">Feb</div>
        </div>
    </div>
</body>
```

### Example 2: Conditional section heights

```html
<style>
    .content-section {
        width: 500pt;
        padding: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        overflow: hidden;
    }
</style>
<body>
    <div class="content-section" style="height: {{section.hasExtendedContent ? '400pt' : '200pt'}}">
        <h2>{{section.title}}</h2>
        <p>{{section.content}}</p>
    </div>
</body>
```

### Example 3: Image sizing based on data

```html
<style>
    .gallery {
        padding: 20pt;
    }
    .gallery-image {
        width: auto;
        border: 2pt solid #e5e7eb;
        margin: 10pt;
        display: block;
    }
</style>
<body>
    <div class="gallery">
        <img class="gallery-image"
             src="{{image.url}}"
             style="height: {{image.height}}pt"
             alt="{{image.caption}}" />
    </div>
</body>
```

### Example 4: Data-driven dashboard panels

```html
<style>
    .dashboard {
        padding: 20pt;
    }
    .metric-panel {
        width: 200pt;
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
        font-size: 28pt;
        font-weight: bold;
        color: #1e40af;
    }
</style>
<body>
    <div class="dashboard">
        <div class="metric-panel" style="height: {{panel.size === 'large' ? '180pt' : '120pt'}}">
            <div class="metric-label">{{panel.label}}</div>
            <div class="metric-value">{{panel.value}}</div>
        </div>
    </div>
</body>
```

---

## Examples

### Example 1: Fixed-height container

```html
<style>
    .container {
        height: 300pt;
        width: 500pt;
        padding: 20pt;
        background-color: #f3f4f6;
        border: 2pt solid #d1d5db;
        overflow: hidden;
    }
</style>
<body>
    <div class="container">
        <h2>Fixed Height Container</h2>
        <p>This container is exactly 300pt tall, plus padding and border.</p>
        <p>If content exceeds this height, it will be clipped.</p>
    </div>
</body>
```

### Example 2: Full-height sidebar layout

```html
<style>
    .page-wrapper {
        height: 700pt;
    }
    .sidebar {
        height: 100%;
        width: 200pt;
        float: left;
        padding: 15pt;
        background-color: #dbeafe;
        border-right: 2pt solid #3b82f6;
    }
    .main-content {
        height: 100%;
        margin-left: 220pt;
        padding: 15pt;
        background-color: #f9fafb;
    }
</style>
<body>
    <div class="page-wrapper">
        <div class="sidebar">
            <h3>Sidebar</h3>
            <p>Full height sidebar</p>
        </div>
        <div class="main-content">
            <h2>Main Content</h2>
            <p>Content area matching sidebar height.</p>
        </div>
    </div>
</body>
```

### Example 3: Fixed-height image sizing

```html
<style>
    .thumbnail {
        height: 150pt;
        width: auto;
        border: 2pt solid #e5e7eb;
        margin: 10pt;
    }
    .banner {
        height: 200pt;
        width: 100%;
        object-fit: cover;
        display: block;
    }
    .logo {
        height: 60pt;
        width: auto;
    }
</style>
<body>
    <img class="logo" src="company-logo.png" alt="Company Logo" />
    <img class="banner" src="hero-banner.jpg" alt="Hero Banner" />
    <img class="thumbnail" src="product.jpg" alt="Product" />
</body>
```

### Example 4: Table row heights

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
    .data-table th {
        height: 40pt;
        background-color: #1f2937;
        color: white;
    }
    .data-table tr {
        height: 35pt;
    }
    .tall-row {
        height: 60pt;
        vertical-align: top;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th>Product</th>
                <th>Description</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget A</td>
                <td>Standard widget</td>
                <td>$19.99</td>
            </tr>
            <tr class="tall-row">
                <td>Widget B</td>
                <td>Premium widget with extended features and capabilities</td>
                <td>$49.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 5: Form textarea height

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
    .form-input {
        width: 100%;
        height: 35pt;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .form-textarea {
        width: 100%;
        height: 120pt;
        padding: 8pt;
        border: 1pt solid #d1d5db;
        resize: none;
    }
</style>
<body>
    <div class="form-container">
        <div class="form-group">
            <label class="form-label">Subject</label>
            <input class="form-input" type="text" />
        </div>
        <div class="form-group">
            <label class="form-label">Message</label>
            <textarea class="form-textarea"></textarea>
        </div>
    </div>
</body>
```

### Example 6: Card with fixed height

```html
<style>
    .card-container {
        padding: 20pt;
    }
    .card {
        height: 280pt;
        width: 250pt;
        padding: 20pt;
        margin-bottom: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        box-sizing: border-box;
        overflow: hidden;
    }
    .card-image {
        height: 150pt;
        width: 100%;
        background-color: #e5e7eb;
        margin-bottom: 12pt;
    }
    .card-title {
        height: 40pt;
        margin: 0 0 10pt 0;
        font-size: 14pt;
        font-weight: bold;
        overflow: hidden;
    }
    .card-description {
        height: 60pt;
        margin: 0;
        font-size: 10pt;
        color: #6b7280;
        overflow: hidden;
    }
</style>
<body>
    <div class="card-container">
        <div class="card">
            <div class="card-image"></div>
            <h3 class="card-title">Product Name</h3>
            <p class="card-description">Product description with fixed height ensures uniform card layout.</p>
        </div>
    </div>
</body>
```

### Example 7: Header and footer with fixed heights

```html
<style>
    .page-container {
        height: 800pt;
        width: 600pt;
        display: flex;
        flex-direction: column;
    }
    .page-header {
        height: 80pt;
        background-color: #1e3a8a;
        color: white;
        padding: 15pt 20pt;
        box-sizing: border-box;
    }
    .page-content {
        flex: 1;
        padding: 20pt;
        background-color: white;
        overflow: auto;
    }
    .page-footer {
        height: 60pt;
        background-color: #f3f4f6;
        padding: 15pt 20pt;
        box-sizing: border-box;
        border-top: 1pt solid #e5e7eb;
    }
</style>
<body>
    <div class="page-container">
        <div class="page-header">
            <h1>Document Header</h1>
        </div>
        <div class="page-content">
            <p>Main content area with flexible height.</p>
        </div>
        <div class="page-footer">
            <p>Page 1 of 1</p>
        </div>
    </div>
</body>
```

### Example 8: Dashboard metrics with uniform heights

```html
<style>
    .dashboard {
        padding: 20pt;
    }
    .metric-panel {
        height: 150pt;
        width: 200pt;
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
    }
    .metric-icon {
        height: 50pt;
        width: 50pt;
        background-color: #dbeafe;
        border-radius: 25pt;
        margin-bottom: 15pt;
    }
    .metric-value {
        height: 40pt;
        font-size: 28pt;
        font-weight: bold;
        color: #1e40af;
        line-height: 40pt;
    }
    .metric-label {
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="dashboard">
        <div class="metric-panel">
            <div class="metric-icon"></div>
            <div class="metric-value">$52K</div>
            <div class="metric-label">Total Revenue</div>
        </div>
        <div class="metric-panel">
            <div class="metric-icon"></div>
            <div class="metric-value">1,234</div>
            <div class="metric-label">Total Orders</div>
        </div>
    </div>
</body>
```

### Example 9: Progress tracker with heights

```html
<style>
    .progress-tracker {
        width: 400pt;
        margin: 20pt;
    }
    .progress-step {
        height: 80pt;
        margin-bottom: 15pt;
        position: relative;
        padding-left: 60pt;
    }
    .step-indicator {
        height: 40pt;
        width: 40pt;
        border-radius: 20pt;
        background-color: #16a34a;
        color: white;
        text-align: center;
        line-height: 40pt;
        font-weight: bold;
        position: absolute;
        left: 0;
        top: 0;
    }
    .step-content {
        height: 100%;
        padding: 8pt 15pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
    }
    .step-title {
        font-weight: bold;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="progress-tracker">
        <div class="progress-step">
            <div class="step-indicator">1</div>
            <div class="step-content">
                <div class="step-title">Order Placed</div>
                <div>Your order has been received</div>
            </div>
        </div>
        <div class="progress-step">
            <div class="step-indicator">2</div>
            <div class="step-content">
                <div class="step-title">Processing</div>
                <div>Order is being prepared</div>
            </div>
        </div>
    </div>
</body>
```

### Example 10: Pricing table with uniform heights

```html
<style>
    .pricing-container {
        padding: 30pt;
    }
    .pricing-card {
        height: 450pt;
        width: 200pt;
        float: left;
        margin: 0 10pt;
        border: 2pt solid #e5e7eb;
        background-color: white;
        box-sizing: border-box;
    }
    .pricing-header {
        height: 100pt;
        padding: 20pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
    .pricing-price {
        height: 80pt;
        padding: 15pt;
        text-align: center;
        font-size: 32pt;
        font-weight: bold;
        border-bottom: 2pt solid #e5e7eb;
    }
    .pricing-features {
        height: 200pt;
        padding: 20pt;
        overflow: hidden;
    }
    .pricing-button {
        height: 70pt;
        padding: 15pt;
        text-align: center;
    }
</style>
<body>
    <div class="pricing-container">
        <div class="pricing-card">
            <div class="pricing-header">
                <h3>Basic Plan</h3>
            </div>
            <div class="pricing-price">$29</div>
            <div class="pricing-features">
                <p>Feature 1</p>
                <p>Feature 2</p>
                <p>Feature 3</p>
            </div>
            <div class="pricing-button">
                <button>Choose Plan</button>
            </div>
        </div>
    </div>
</body>
```

### Example 11: Banner with fixed height

```html
<style>
    .hero-banner {
        height: 250pt;
        width: 100%;
        background-color: #1e3a8a;
        color: white;
        padding: 40pt;
        box-sizing: border-box;
        position: relative;
    }
    .hero-content {
        height: 100%;
        display: flex;
        flex-direction: column;
        justify-content: center;
    }
    .hero-title {
        font-size: 36pt;
        font-weight: bold;
        margin-bottom: 15pt;
    }
    .hero-subtitle {
        font-size: 18pt;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="hero-banner">
        <div class="hero-content">
            <div class="hero-title">Welcome to Our Service</div>
            <div class="hero-subtitle">Transforming businesses since 2025</div>
        </div>
    </div>
</body>
```

### Example 12: Certificate with fixed height

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
        box-sizing: border-box;
    }
    .certificate-inner {
        width: 100%;
        height: 100%;
        border: 2pt solid #1e3a8a;
        padding: 30pt;
        display: flex;
        flex-direction: column;
        justify-content: center;
        box-sizing: border-box;
    }
    .certificate-title {
        height: 80pt;
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        line-height: 80pt;
    }
    .certificate-body {
        height: 200pt;
        display: flex;
        flex-direction: column;
        justify-content: center;
    }
</style>
<body>
    <div class="certificate">
        <div class="certificate-inner">
            <div class="certificate-title">Certificate of Excellence</div>
            <div class="certificate-body">
                <h2>Awarded to Jane Doe</h2>
                <p>For outstanding achievement</p>
            </div>
        </div>
    </div>
</body>
```

### Example 13: Agenda timeline with row heights

```html
<style>
    .agenda {
        width: 500pt;
        margin: 30pt auto;
    }
    .agenda-item {
        height: 80pt;
        margin-bottom: 10pt;
        border: 1pt solid #e5e7eb;
        background-color: white;
        box-sizing: border-box;
    }
    .agenda-time {
        height: 100%;
        width: 100pt;
        float: left;
        padding: 15pt;
        background-color: #1e40af;
        color: white;
        font-weight: bold;
        font-size: 14pt;
        text-align: center;
        box-sizing: border-box;
    }
    .agenda-content {
        height: 100%;
        margin-left: 100pt;
        padding: 15pt;
        box-sizing: border-box;
    }
    .agenda-title {
        font-weight: bold;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="agenda">
        <div class="agenda-item">
            <div class="agenda-time">9:00 AM</div>
            <div class="agenda-content">
                <div class="agenda-title">Opening Keynote</div>
                <div>Welcome and introduction</div>
            </div>
        </div>
        <div class="agenda-item">
            <div class="agenda-time">10:30 AM</div>
            <div class="agenda-content">
                <div class="agenda-title">Technical Session</div>
                <div>Deep dive into new features</div>
            </div>
        </div>
    </div>
</body>
```

### Example 14: Separator dividers with height

```html
<style>
    .content-section {
        padding: 20pt;
    }
    .divider-thin {
        height: 1pt;
        width: 100%;
        background-color: #e5e7eb;
        margin: 20pt 0;
    }
    .divider-medium {
        height: 3pt;
        width: 100%;
        background-color: #d1d5db;
        margin: 25pt 0;
    }
    .divider-thick {
        height: 10pt;
        width: 100%;
        background-color: #1e40af;
        margin: 30pt 0;
    }
    .spacer {
        height: 40pt;
    }
</style>
<body>
    <div class="content-section">
        <h2>Section One</h2>
        <p>Content for the first section.</p>
        <div class="divider-thin"></div>

        <h2>Section Two</h2>
        <p>Content for the second section.</p>
        <div class="divider-medium"></div>

        <h2>Section Three</h2>
        <p>Content for the third section.</p>
        <div class="divider-thick"></div>

        <div class="spacer"></div>
        <h2>Section Four</h2>
    </div>
</body>
```

### Example 15: Box-sizing with height

```html
<style>
    .example-container {
        padding: 20pt;
    }
    .box-content {
        height: 200pt;
        width: 300pt;
        padding: 20pt;
        border: 5pt solid #3b82f6;
        background-color: #dbeafe;
        margin-bottom: 30pt;
    }
    .box-border {
        height: 200pt;
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
            <p>Total height: 200pt + 40pt padding + 10pt border = 250pt</p>
        </div>
        <div class="box-label">box-sizing: content-box (default)</div>

        <div class="box-border">
            <p><strong>Border Box</strong></p>
            <p>Total height: 200pt (includes padding and border)</p>
        </div>
        <div class="box-label">box-sizing: border-box</div>
    </div>
</body>
```

---

## See Also

- [width](/reference/cssproperties/css_prop_width) - Set element width
- [min-height](/reference/cssproperties/css_prop_min-height) - Set minimum height constraint
- [max-height](/reference/cssproperties/css_prop_max-height) - Set maximum height constraint
- [padding](/reference/cssproperties/css_prop_padding) - Set padding shorthand property
- [border](/reference/cssproperties/css_prop_border) - Set border shorthand property
- [margin](/reference/cssproperties/css_prop_margin) - Set margin shorthand property
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
