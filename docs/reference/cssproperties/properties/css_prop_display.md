---
layout: default
title: display
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# display : Display Property

The `display` property specifies how an element is rendered in the document layout. It controls whether an element is treated as a block-level element, inline element, table element, or hidden entirely. This is fundamental for controlling document structure and layout in PDF generation.

## Usage

```css
selector {
    display: value;
}
```

The display property determines the element's box model and how it interacts with surrounding elements in the document flow.

---

## Supported Values

### block
The element generates a block-level box. Block elements start on a new line and take up the full width available. Default for elements like `<div>`, `<p>`, `<h1>`-`<h6>`, `<section>`, `<article>`.

### inline
The element generates an inline-level box. Inline elements flow with text content and only take up as much width as necessary. Width and height properties are ignored. Default for elements like `<span>`, `<a>`, `<strong>`, `<em>`.

### inline-block
The element generates a block box that flows inline. Combines characteristics of both inline and block - flows with text like inline but can have width, height, and vertical margins like block elements.

### none
The element is completely removed from the document flow and is not rendered. The element takes up no space in the layout.

### table
The element behaves like a `<table>` element. Creates a block-level table box.

### table-row
The element behaves like a `<tr>` (table row) element.

### table-cell
The element behaves like a `<td>` (table cell) element. Enables vertical alignment and equal-height columns without using actual table markup.

### list-item
The element generates a block box with an associated list-item marker (bullet or number), behaving like a `<li>` element.

---

## Supported Elements

The `display` property can be applied to:
- All HTML elements
- Block elements (`<div>`, `<section>`, `<article>`, `<p>`)
- Inline elements (`<span>`, `<a>`, `<strong>`)
- Table elements (`<table>`, `<tr>`, `<td>`)
- List elements (`<ul>`, `<ol>`, `<li>`)
- Form elements
- All container and content elements

---

## Notes

- The `display` property is one of the most fundamental CSS properties for layout control
- `display: none` removes the element completely from the layout (unlike `visibility: hidden` which reserves space)
- Block elements start on a new line and stretch to fill their container's width by default
- Inline elements flow within text and cannot have explicit width or height
- `inline-block` is ideal for creating horizontal layouts while maintaining box model properties
- Table display values allow table-like layouts without semantic table markup
- Table-cell elements align vertically and create equal-height columns automatically
- `list-item` display creates list markers (bullets/numbers) without using `<li>` elements
- Changing display can significantly affect how margins, padding, and positioning behave
- In PDF generation, display controls how elements flow and paginate

---

## Data Binding

The display property works seamlessly with data binding to create dynamic, conditional layouts that adapt based on data values. This enables powerful scenarios like conditional visibility, dynamic layout switching, and adaptive document structures.

### Example 1: Conditional section visibility

```html
<style>
    .section {
        padding: 20pt;
        margin-bottom: 15pt;
        border: 2pt solid #e5e7eb;
    }
</style>
<body>
    <!-- Conditionally show/hide sections based on data -->
    <div class="section" style="display: {{showSummary ? 'block' : 'none'}}">
        <h2>Executive Summary</h2>
        <p>This section only appears when showSummary is true.</p>
    </div>

    <div class="section" style="display: {{includeDetails ? 'block' : 'none'}}">
        <h2>Detailed Analysis</h2>
        <p>Detailed content shown based on includeDetails flag.</p>
    </div>

    <div class="section" style="display: {{showDebugInfo ? 'block' : 'none'}}">
        <h3>Debug Information</h3>
        <p>Development data: Version {{version}}, Generated: {{timestamp}}</p>
    </div>
</body>
```

### Example 2: Dynamic layout based on preferences

```html
<style>
    .content-area {
        padding: 20pt;
    }
    .inline-layout span {
        margin-right: 15pt;
    }
</style>
<body>
    <!-- Switch between block and inline display based on user preference -->
    <div class="content-area">
        <h2>Product Features</h2>
        <div style="display: {{layout.style === 'compact' ? 'inline-block' : 'block'}}">
            <span>Feature 1</span>
            <span>Feature 2</span>
            <span>Feature 3</span>
        </div>
    </div>

    <!-- Conditionally use table layout -->
    <div style="display: {{preferences.useTableLayout ? 'table' : 'block'}}; width: 100%;">
        <div style="display: {{preferences.useTableLayout ? 'table-row' : 'block'}}">
            <div style="display: {{preferences.useTableLayout ? 'table-cell' : 'block'}}; padding: 10pt;">
                Column 1 content
            </div>
            <div style="display: {{preferences.useTableLayout ? 'table-cell' : 'block'}}; padding: 10pt;">
                Column 2 content
            </div>
        </div>
    </div>
</body>
```

### Example 3: Conditional rendering for different document types

```html
<style>
    .invoice-layout { background-color: #f9fafb; }
    .report-layout { background-color: #fffef7; }
</style>
<body>
    <!-- Show different layouts based on document type -->
    <div style="display: {{documentType === 'invoice' ? 'block' : 'none'}}" class="invoice-layout">
        <h1>Invoice #{{invoiceNumber}}</h1>
        <div style="display: {{isPaid ? 'none' : 'block'}}" class="payment-due">
            <strong>Payment Due: {{dueDate}}</strong>
        </div>
    </div>

    <div style="display: {{documentType === 'report' ? 'block' : 'none'}}" class="report-layout">
        <h1>{{reportTitle}}</h1>
        <div style="display: {{showConfidential ? 'block' : 'none'}}" class="confidential-notice">
            <p>CONFIDENTIAL - Internal Use Only</p>
        </div>
    </div>

    <!-- Conditional footer elements -->
    <div style="display: {{showDraftWatermark ? 'block' : 'none'}}" class="watermark">
        <p style="text-align: center; color: #dc2626; font-size: 36pt; opacity: 0.3;">DRAFT</p>
    </div>
</body>
```

---

## Examples

### Example 1: Block vs Inline elements

```html
<style>
    .block-element {
        display: block;
        background-color: #dbeafe;
        padding: 10pt;
        margin-bottom: 5pt;
        border: 1pt solid #2563eb;
    }
    .inline-element {
        display: inline;
        background-color: #dcfce7;
        padding: 5pt;
        border: 1pt solid #16a34a;
    }
</style>
<body>
    <div class="block-element">Block Element 1 - Takes full width</div>
    <div class="block-element">Block Element 2 - Starts on new line</div>
    <p>
        This paragraph contains <span class="inline-element">inline element 1</span>
        and <span class="inline-element">inline element 2</span> that flow with text.
    </p>
</body>
```

### Example 2: Converting spans to blocks

```html
<style>
    .label {
        display: block;
        font-weight: bold;
        margin-bottom: 5pt;
        color: #1e3a8a;
    }
    .value {
        display: block;
        margin-bottom: 15pt;
        padding: 8pt;
        background-color: #f3f4f6;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div style="width: 400pt; padding: 20pt;">
        <span class="label">Customer Name:</span>
        <span class="value">John Smith</span>

        <span class="label">Order Number:</span>
        <span class="value">ORD-2025-001</span>

        <span class="label">Total Amount:</span>
        <span class="value">$1,234.56</span>
    </div>
</body>
```

### Example 3: Inline-block for horizontal buttons

```html
<style>
    .button-container {
        text-align: center;
        padding: 20pt;
        background-color: #f9fafb;
    }
    .button {
        display: inline-block;
        width: 120pt;
        padding: 10pt;
        margin: 5pt;
        background-color: #2563eb;
        color: white;
        text-align: center;
        border-radius: 4pt;
        font-weight: bold;
    }
    .button-secondary {
        background-color: #6b7280;
    }
</style>
<body>
    <div class="button-container">
        <div class="button">Submit</div>
        <div class="button button-secondary">Cancel</div>
        <div class="button button-secondary">Reset</div>
    </div>
</body>
```

### Example 4: Hiding elements with display: none

```html
<style>
    .visible {
        background-color: #dcfce7;
        padding: 10pt;
        margin-bottom: 5pt;
        border: 1pt solid #16a34a;
    }
    .hidden {
        display: none;
    }
    .section {
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="section">
        <h2>Visible Elements</h2>
        <div class="visible">This element is visible</div>
        <div class="hidden">This element is hidden and takes no space</div>
        <div class="visible">This element appears right after the first</div>
    </div>
    <div class="section">
        <h2>Conditional Content</h2>
        <div class="visible">Standard content always shown</div>
        <div class="hidden">Debug information (hidden in production)</div>
        <div class="hidden">Draft watermark (hidden in final version)</div>
    </div>
</body>
```

### Example 5: Table layout without table elements

```html
<style>
    .table {
        display: table;
        width: 100%;
        border-collapse: collapse;
    }
    .table-row {
        display: table-row;
    }
    .table-cell {
        display: table-cell;
        padding: 10pt;
        border: 1pt solid #d1d5db;
    }
    .header-cell {
        display: table-cell;
        padding: 10pt;
        background-color: #1f2937;
        color: white;
        font-weight: bold;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="table">
        <div class="table-row">
            <div class="header-cell">Product</div>
            <div class="header-cell">Quantity</div>
            <div class="header-cell">Price</div>
        </div>
        <div class="table-row">
            <div class="table-cell">Widget A</div>
            <div class="table-cell">25</div>
            <div class="table-cell">$12.99</div>
        </div>
        <div class="table-row">
            <div class="table-cell">Widget B</div>
            <div class="table-cell">10</div>
            <div class="table-cell">$24.99</div>
        </div>
    </div>
</body>
```

### Example 6: Equal-height columns with table-cell

```html
<style>
    .column-container {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 15pt;
    }
    .column {
        display: table-cell;
        padding: 20pt;
        background-color: #f3f4f6;
        border: 2pt solid #d1d5db;
        vertical-align: top;
        width: 33.33%;
    }
    .column h3 {
        margin: 0 0 10pt 0;
        color: #1e3a8a;
    }
    .column p {
        margin: 0;
        line-height: 1.6;
    }
</style>
<body>
    <div class="column-container">
        <div class="column">
            <h3>Feature One</h3>
            <p>Short description.</p>
        </div>
        <div class="column">
            <h3>Feature Two</h3>
            <p>This is a much longer description that spans multiple lines.
            The columns will automatically have equal height.</p>
        </div>
        <div class="column">
            <h3>Feature Three</h3>
            <p>Medium length description here.</p>
        </div>
    </div>
</body>
```

### Example 7: Custom list with list-item display

```html
<style>
    .custom-list {
        padding-left: 0;
        margin: 20pt;
    }
    .list-item {
        display: list-item;
        list-style-type: disc;
        margin-left: 20pt;
        padding: 5pt 0;
        color: #1e3a8a;
    }
    .numbered-item {
        display: list-item;
        list-style-type: decimal;
        margin-left: 20pt;
        padding: 5pt 0;
    }
</style>
<body>
    <h2>Custom Bulleted List</h2>
    <div class="custom-list">
        <div class="list-item">First bullet point</div>
        <div class="list-item">Second bullet point</div>
        <div class="list-item">Third bullet point</div>
    </div>

    <h2>Custom Numbered List</h2>
    <div class="custom-list">
        <div class="numbered-item">First numbered item</div>
        <div class="numbered-item">Second numbered item</div>
        <div class="numbered-item">Third numbered item</div>
    </div>
</body>
```

### Example 8: Product grid with inline-block

```html
<style>
    .product-grid {
        text-align: center;
        padding: 20pt;
        background-color: #f9fafb;
    }
    .product-card {
        display: inline-block;
        width: 150pt;
        margin: 10pt;
        padding: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        vertical-align: top;
        text-align: left;
    }
    .product-image {
        width: 100%;
        height: 100pt;
        background-color: #dbeafe;
        margin-bottom: 10pt;
    }
    .product-name {
        font-weight: bold;
        margin-bottom: 5pt;
        color: #1e3a8a;
    }
    .product-price {
        color: #16a34a;
        font-size: 14pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="product-grid">
        <div class="product-card">
            <div class="product-image"></div>
            <div class="product-name">Product Alpha</div>
            <div class="product-price">$29.99</div>
        </div>
        <div class="product-card">
            <div class="product-image"></div>
            <div class="product-name">Product Beta</div>
            <div class="product-price">$39.99</div>
        </div>
        <div class="product-card">
            <div class="product-image"></div>
            <div class="product-name">Product Gamma</div>
            <div class="product-price">$49.99</div>
        </div>
    </div>
</body>
```

### Example 9: Navigation menu with inline-block

```html
<style>
    .nav-menu {
        background-color: #1f2937;
        padding: 0;
        margin: 0;
        text-align: center;
    }
    .nav-item {
        display: inline-block;
        padding: 15pt 25pt;
        color: white;
        font-weight: bold;
        border-right: 1pt solid #374151;
    }
    .nav-item:last-child {
        border-right: none;
    }
    .nav-item.active {
        background-color: #2563eb;
    }
</style>
<body>
    <div class="nav-menu">
        <div class="nav-item active">Home</div>
        <div class="nav-item">Products</div>
        <div class="nav-item">Services</div>
        <div class="nav-item">About</div>
        <div class="nav-item">Contact</div>
    </div>
</body>
```

### Example 10: Complex form layout with mixed display types

```html
<style>
    .form-container {
        width: 500pt;
        padding: 25pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
    }
    .form-row {
        display: block;
        margin-bottom: 15pt;
    }
    .form-label {
        display: block;
        font-weight: bold;
        margin-bottom: 5pt;
        color: #1f2937;
    }
    .form-input {
        display: block;
        width: 100%;
        padding: 8pt;
        border: 1pt solid #d1d5db;
        background-color: white;
    }
    .form-inline-group {
        display: block;
        margin-bottom: 15pt;
    }
    .form-inline-label {
        display: inline-block;
        width: 150pt;
        font-weight: bold;
        color: #1f2937;
    }
    .form-inline-input {
        display: inline-block;
        width: 300pt;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .checkbox-group {
        display: block;
        margin-top: 15pt;
    }
    .checkbox-item {
        display: inline-block;
        margin-right: 20pt;
    }
</style>
<body>
    <div class="form-container">
        <h2 style="margin-top: 0;">Registration Form</h2>

        <div class="form-row">
            <span class="form-label">Full Name:</span>
            <span class="form-input">&nbsp;</span>
        </div>

        <div class="form-inline-group">
            <span class="form-inline-label">Email:</span>
            <span class="form-inline-input">&nbsp;</span>
        </div>

        <div class="form-inline-group">
            <span class="form-inline-label">Phone:</span>
            <span class="form-inline-input">&nbsp;</span>
        </div>

        <div class="checkbox-group">
            <span class="form-label">Preferences:</span>
            <div class="checkbox-item">[ ] Newsletter</div>
            <div class="checkbox-item">[ ] Updates</div>
            <div class="checkbox-item">[ ] Promotions</div>
        </div>
    </div>
</body>
```

### Example 11: Magazine layout with mixed display

```html
<style>
    .magazine-page {
        padding: 30pt;
    }
    .headline {
        display: block;
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
        border-bottom: 3pt solid #1e3a8a;
        padding-bottom: 10pt;
    }
    .byline {
        display: block;
        font-style: italic;
        color: #6b7280;
        margin-bottom: 20pt;
    }
    .lead-image {
        display: block;
        width: 100%;
        height: 200pt;
        background-color: #e5e7eb;
        margin-bottom: 15pt;
    }
    .pull-quote {
        display: inline-block;
        width: 200pt;
        padding: 15pt;
        margin: 10pt 0 10pt 20pt;
        background-color: #fef3c7;
        border-left: 4pt solid #f59e0b;
        float: right;
        font-size: 14pt;
        font-style: italic;
    }
    .article-text {
        text-align: justify;
        line-height: 1.6;
    }
</style>
<body>
    <div class="magazine-page">
        <span class="headline">The Future of PDF Generation</span>
        <span class="byline">By Jane Developer | October 14, 2025</span>
        <div class="lead-image"></div>

        <div class="article-text">
            <div class="pull-quote">
                "Display properties are fundamental to modern document layout."
            </div>
            <p>The display property provides powerful control over how elements
            appear in your PDF documents. From simple block and inline layouts
            to complex table structures, understanding display is essential.</p>

            <p>Modern PDF generation requires flexible layout techniques that
            adapt to various content types and document structures...</p>
        </div>
    </div>
</body>
```

### Example 12: Dashboard with table-cell layout

```html
<style>
    .dashboard {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 15pt;
        padding: 20pt;
    }
    .dashboard-row {
        display: table-row;
    }
    .metric-card {
        display: table-cell;
        padding: 20pt;
        background-color: white;
        border: 2pt solid #e5e7eb;
        text-align: center;
        vertical-align: middle;
    }
    .metric-value {
        display: block;
        font-size: 36pt;
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 10pt;
    }
    .metric-label {
        display: block;
        font-size: 12pt;
        color: #6b7280;
        text-transform: uppercase;
    }
</style>
<body>
    <div class="dashboard">
        <div class="dashboard-row">
            <div class="metric-card">
                <span class="metric-value">1,234</span>
                <span class="metric-label">Total Sales</span>
            </div>
            <div class="metric-card">
                <span class="metric-value">$45.6K</span>
                <span class="metric-label">Revenue</span>
            </div>
            <div class="metric-card">
                <span class="metric-value">892</span>
                <span class="metric-label">Customers</span>
            </div>
        </div>
    </div>
</body>
```

### Example 13: Invoice with selective hiding

```html
<style>
    .invoice {
        width: 550pt;
        padding: 30pt;
        margin: 20pt auto;
        border: 2pt solid #1f2937;
    }
    .invoice-header {
        display: block;
        margin-bottom: 30pt;
        padding-bottom: 15pt;
        border-bottom: 2pt solid #1f2937;
    }
    .company-name {
        display: block;
        font-size: 24pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .draft-watermark {
        display: none; /* Hidden in final version */
        color: #dc2626;
        font-size: 48pt;
        text-align: center;
        opacity: 0.3;
    }
    .debug-info {
        display: none; /* Hidden in production */
        background-color: #fef3c7;
        padding: 10pt;
        font-size: 8pt;
        font-family: monospace;
    }
    .line-item {
        display: table;
        width: 100%;
        margin-bottom: 5pt;
    }
    .line-col {
        display: table-cell;
        padding: 5pt;
    }
</style>
<body>
    <div class="invoice">
        <div class="draft-watermark">DRAFT</div>
        <div class="debug-info">Version: 1.0 | Generated: 2025-10-14</div>

        <div class="invoice-header">
            <span class="company-name">ACME Corporation</span>
        </div>

        <h2>Invoice #INV-2025-001</h2>

        <div class="line-item">
            <div class="line-col" style="width: 60%;">Professional Services</div>
            <div class="line-col" style="width: 20%;">10 hrs</div>
            <div class="line-col" style="width: 20%; text-align: right;">$1,500.00</div>
        </div>
    </div>
</body>
```

### Example 14: Brochure layout with strategic display

```html
<style>
    .brochure {
        padding: 30pt;
    }
    .header-banner {
        display: block;
        background-color: #1e3a8a;
        color: white;
        padding: 20pt;
        text-align: center;
        margin-bottom: 30pt;
    }
    .banner-title {
        display: block;
        font-size: 32pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .banner-subtitle {
        display: block;
        font-size: 16pt;
    }
    .feature-grid {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 20pt;
    }
    .feature-row {
        display: table-row;
    }
    .feature-box {
        display: table-cell;
        padding: 20pt;
        background-color: #f3f4f6;
        border: 2pt solid #d1d5db;
        vertical-align: top;
    }
    .feature-icon {
        display: inline-block;
        width: 60pt;
        height: 60pt;
        background-color: #2563eb;
        color: white;
        text-align: center;
        line-height: 60pt;
        font-size: 24pt;
        font-weight: bold;
        margin-bottom: 15pt;
    }
    .feature-title {
        display: block;
        font-size: 16pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
    }
    .feature-description {
        display: block;
        line-height: 1.6;
    }
</style>
<body>
    <div class="brochure">
        <div class="header-banner">
            <span class="banner-title">Premium Services</span>
            <span class="banner-subtitle">Excellence in Every Detail</span>
        </div>

        <div class="feature-grid">
            <div class="feature-row">
                <div class="feature-box">
                    <div class="feature-icon">1</div>
                    <span class="feature-title">Quality</span>
                    <span class="feature-description">
                        Top-tier quality in every aspect of our service delivery.
                    </span>
                </div>
                <div class="feature-box">
                    <div class="feature-icon">2</div>
                    <span class="feature-title">Speed</span>
                    <span class="feature-description">
                        Fast turnaround times without compromising excellence.
                    </span>
                </div>
                <div class="feature-box">
                    <div class="feature-icon">3</div>
                    <span class="feature-title">Support</span>
                    <span class="feature-description">
                        24/7 customer support ready to assist you anytime.
                    </span>
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 15: Advanced newsletter layout

```html
<style>
    .newsletter {
        width: 600pt;
        margin: 0 auto;
        padding: 20pt;
    }
    .masthead {
        display: table;
        width: 100%;
        margin-bottom: 20pt;
        border-bottom: 3pt solid #1e3a8a;
        padding-bottom: 15pt;
    }
    .masthead-logo {
        display: table-cell;
        width: 30%;
        vertical-align: middle;
        font-size: 24pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .masthead-info {
        display: table-cell;
        width: 70%;
        text-align: right;
        vertical-align: middle;
        color: #6b7280;
    }
    .article-grid {
        display: block;
        margin-bottom: 30pt;
    }
    .featured-article {
        display: block;
        margin-bottom: 20pt;
        padding: 20pt;
        background-color: #dbeafe;
        border-left: 5pt solid #2563eb;
    }
    .article-headline {
        display: block;
        font-size: 20pt;
        font-weight: bold;
        color: #1e40af;
        margin-bottom: 10pt;
    }
    .article-excerpt {
        display: block;
        line-height: 1.6;
    }
    .secondary-articles {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 15pt;
    }
    .secondary-article {
        display: table-cell;
        padding: 15pt;
        background-color: #f3f4f6;
        border: 1pt solid #d1d5db;
        vertical-align: top;
    }
    .secondary-title {
        display: block;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 8pt;
    }
    .read-more {
        display: inline-block;
        margin-top: 10pt;
        padding: 8pt 15pt;
        background-color: #2563eb;
        color: white;
        font-weight: bold;
        text-align: center;
    }
</style>
<body>
    <div class="newsletter">
        <div class="masthead">
            <div class="masthead-logo">TECH NEWS</div>
            <div class="masthead-info">
                October 2025 | Volume 12 | Issue 10
            </div>
        </div>

        <div class="article-grid">
            <div class="featured-article">
                <span class="article-headline">Breaking: New PDF Technology Announced</span>
                <span class="article-excerpt">
                    Revolutionary advances in document generation bring new
                    possibilities for designers and developers...
                </span>
                <div class="read-more">Read More</div>
            </div>

            <div class="secondary-articles">
                <div class="secondary-article">
                    <span class="secondary-title">Industry Update</span>
                    <p style="margin: 0; font-size: 10pt;">
                        Latest trends in PDF generation and document automation.
                    </p>
                </div>
                <div class="secondary-article">
                    <span class="secondary-title">Tutorial Corner</span>
                    <p style="margin: 0; font-size: 10pt;">
                        Learn advanced layout techniques for professional documents.
                    </p>
                </div>
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [float](/reference/cssproperties/css_prop_float) - Float elements left or right
- [position](/reference/cssproperties/css_prop_position) - Set positioning method
- [width](/reference/cssproperties/css_prop_width) - Set element width
- [height](/reference/cssproperties/css_prop_height) - Set element height
- [overflow](/reference/cssproperties/css_prop_overflow) - Control overflow behavior
- [vertical-align](/reference/cssproperties/css_prop_vertical-align) - Set vertical alignment
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
