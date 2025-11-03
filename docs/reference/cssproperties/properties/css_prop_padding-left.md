---
layout: default
title: padding-left
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# padding-left : Left Padding Property

The `padding-left` property sets the left padding of an element in PDF documents. Left padding creates space inside the element to the left of the content, between the left border and the content, inheriting the element's background color.

## Usage

```css
selector {
    padding-left: value;
}
```

The padding-left property accepts a single length value or percentage that defines the space to the left of the content.

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
- `0` - No left padding

---

## Supported Elements

The `padding-left` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Lists (`<ul>`, `<ol>`)
- List items (`<li>`)
- All container elements

---

## Notes

- Left padding inherits the element's background color
- Left padding increases the overall width of the element
- Percentage left padding is calculated relative to parent element's width
- Left padding cannot be negative
- Left padding is inside the border, while margin is outside
- Background colors and images extend into the padding area
- Left padding is particularly useful for indentation and icon spacing
- Essential for creating proper horizontal spacing in layouts

---

## Data Binding

The `padding-left` property supports dynamic values through data binding, allowing you to create flexible left internal spacing for icons, indentation, or visual hierarchy based on content requirements.

### Example 1: Alert with icon padding

```html
<style>
    .alert {
        padding-top: 12pt;
        padding-bottom: 12pt;
        padding-left: {{alert.type === 'icon' ? '45pt' : '15pt'}};
        padding-right: 15pt;
        background-color: #dbeafe;
        border: 1pt solid #3b82f6;
        border-left: 4pt solid #3b82f6;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="alert">
        <strong>{{alert.title}}:</strong> {{alert.message}}
    </div>
</body>
```

Data context:
```json
{
    "alert": {
        "type": "icon",
        "title": "Info",
        "message": "Extra left padding reserved for icon placement"
    }
}
```

### Example 2: Nested content with hierarchy-based padding

```html
<style>
    .content {
        padding: 25pt;
    }
    .content-block {
        padding-top: 10pt;
        padding-bottom: 10pt;
        padding-left: {{block.level * 15}}pt;
        padding-right: 10pt;
        background-color: {{block.level === 0 ? '#f9fafb' : '#f3f4f6'}};
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="content">
        <div class="content-block">
            <p>{{block.text}}</p>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "block": {
        "level": 1,
        "text": "This block has left padding based on its hierarchy level"
    }
}
```

### Example 3: Quote block with data-driven accent padding

```html
<style>
    .quote-block {
        padding-top: 20pt;
        padding-bottom: 20pt;
        padding-left: {{quote.hasAccent ? '30pt' : '20pt'}};
        padding-right: 20pt;
        background-color: #f5f5f5;
        border-left: {{quote.hasAccent ? '5pt' : '0'}} solid #6366f1;
        font-style: italic;
        margin: 20pt 0;
    }
</style>
<body>
    <div class="quote-block">
        <p>{{quote.text}}</p>
        <p style="margin-top: 10pt; font-size: 10pt; color: #6b7280;">— {{quote.author}}</p>
    </div>
</body>
```

Data context:
```json
{
    "quote": {
        "hasAccent": true,
        "text": "Design is not just what it looks like. Design is how it works.",
        "author": "Steve Jobs"
    }
}
```

---

## Examples

### Example 1: Basic left padding

```html
<style>
    .box {
        padding-top: 15pt;
        padding-bottom: 15pt;
        padding-left: 30pt;
        padding-right: 15pt;
        background-color: #dbeafe;
        border: 2pt solid #3b82f6;
        border-left: 5pt solid #3b82f6;
    }
</style>
<body>
    <div class="box">
        <p style="margin: 0;">This box has extra left padding for emphasis.</p>
    </div>
</body>
```

### Example 2: Alert with icon space

```html
<style>
    .alert-with-icon {
        padding-top: 12pt;
        padding-bottom: 12pt;
        padding-left: 50pt;
        padding-right: 15pt;
        background-color: #dcfce7;
        border: 1pt solid #16a34a;
        border-left: 4pt solid #16a34a;
    }
</style>
<body>
    <div class="alert-with-icon">
        <strong>Success:</strong> Extra left padding reserved for icon placement.
    </div>
</body>
```

### Example 3: Indented quote block

```html
<style>
    .quote-indented {
        padding-top: 20pt;
        padding-bottom: 20pt;
        padding-left: 40pt;
        padding-right: 20pt;
        background-color: #f5f5f5;
        border-left: 6pt solid #6366f1;
        font-style: italic;
        margin: 20pt 0;
    }
</style>
<body>
    <div class="quote-indented">
        <p style="margin: 0;">"The secret of getting ahead is getting started."</p>
        <p style="margin: 10pt 0 0 0; font-size: 11pt; color: #6b7280;">— Mark Twain</p>
    </div>
</body>
```

### Example 4: List with custom left padding

```html
<style>
    .custom-list {
        padding-left: 40pt;
        padding-top: 10pt;
        padding-bottom: 10pt;
        background-color: #f9fafb;
        border-left: 3pt solid #3b82f6;
    }
    .custom-list li {
        margin-bottom: 8pt;
    }
</style>
<body>
    <ul class="custom-list">
        <li>First item with custom left padding</li>
        <li>Second item</li>
        <li>Third item</li>
    </ul>
</body>
```

### Example 5: Form label with left padding

```html
<style>
    .form-group {
        padding: 15pt 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
    }
    .form-input {
        padding-top: 8pt;
        padding-bottom: 8pt;
        padding-left: 12pt;
        padding-right: 12pt;
        border: 1pt solid #d1d5db;
        width: 100%;
    }
</style>
<body>
    <div class="form-group">
        <label style="display: block; margin-bottom: 5pt; font-weight: bold;">Username</label>
        <input class="form-input" type="text" placeholder="Enter username" />
    </div>
</body>
```

### Example 6: Sidebar navigation with left padding

```html
<style>
    .sidebar-nav {
        width: 180pt;
        padding-top: 20pt;
        padding-bottom: 20pt;
        padding-left: 25pt;
        padding-right: 15pt;
        background-color: #f3f4f6;
        border-left: 4pt solid #3b82f6;
    }
    .nav-item {
        padding: 8pt 12pt 8pt 15pt;
        margin-bottom: 5pt;
        background-color: white;
        border-left: 3pt solid transparent;
    }
    .nav-item.active {
        border-left-color: #3b82f6;
        background-color: #dbeafe;
    }
</style>
<body>
    <div class="sidebar-nav">
        <div class="nav-item active">Dashboard</div>
        <div class="nav-item">Settings</div>
        <div class="nav-item">Reports</div>
    </div>
</body>
```

### Example 7: Invoice line items with left padding

```html
<style>
    .invoice-items {
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .line-item {
        padding-top: 12pt;
        padding-bottom: 12pt;
        padding-left: 30pt;
        padding-right: 20pt;
        border-bottom: 1pt solid #e5e7eb;
    }
    .line-item:last-child {
        border-bottom: none;
    }
</style>
<body>
    <div class="invoice-items">
        <div class="line-item">Consulting Services - $1,500.00</div>
        <div class="line-item">Software License - $299.00</div>
        <div class="line-item">Support Package - $199.00</div>
    </div>
</body>
```

### Example 8: Code block with left padding

```html
<style>
    .code-block {
        padding-top: 15pt;
        padding-bottom: 15pt;
        padding-left: 25pt;
        padding-right: 15pt;
        background-color: #1f2937;
        color: #f9fafb;
        font-family: monospace;
        font-size: 10pt;
        border-left: 5pt solid #3b82f6;
    }
</style>
<body>
    <div class="code-block">
        function example() {<br/>
        &nbsp;&nbsp;return "Hello World";<br/>
        }
    </div>
</body>
```

### Example 9: Table cells with left padding

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        padding-top: 10pt;
        padding-bottom: 10pt;
        padding-left: 20pt;
        padding-right: 12pt;
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
                <th>Product</th>
                <th>Quantity</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget A</td>
                <td>150</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 10: Newsletter article with left padding

```html
<style>
    .newsletter-article {
        padding-top: 20pt;
        padding-bottom: 20pt;
        padding-left: 35pt;
        padding-right: 25pt;
        background-color: #f9fafb;
        border-left: 5pt solid #1e40af;
        margin-bottom: 20pt;
    }
    .article-title {
        margin: 0 0 12pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="newsletter-article">
        <h3 class="article-title">Feature Article</h3>
        <p style="margin: 0;">Article content with generous left padding.</p>
    </div>
</body>
```

### Example 11: Product card with left padding

```html
<style>
    .product-card {
        background-color: white;
        border: 1pt solid #e5e7eb;
        margin-bottom: 15pt;
        overflow: hidden;
    }
    .product-info {
        padding-top: 20pt;
        padding-bottom: 20pt;
        padding-left: 30pt;
        padding-right: 20pt;
    }
    .product-name {
        margin: 0 0 8pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
    .product-price {
        margin: 0;
        padding-left: 5pt;
        color: #16a34a;
        font-size: 18pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="product-card">
        <div class="product-info">
            <h3 class="product-name">Premium Widget</h3>
            <p class="product-price">$99.99</p>
        </div>
    </div>
</body>
```

### Example 12: Receipt with indented items

```html
<style>
    .receipt {
        width: 300pt;
        margin: 40pt auto;
        border: 2pt solid #000;
    }
    .receipt-header {
        padding: 20pt;
        text-align: center;
        background-color: #f9fafb;
        border-bottom: 2pt solid #000;
    }
    .receipt-item {
        padding-top: 10pt;
        padding-bottom: 10pt;
        padding-left: 25pt;
        padding-right: 20pt;
        border-bottom: 1pt dashed #d1d5db;
    }
</style>
<body>
    <div class="receipt">
        <div class="receipt-header">
            <h2 style="margin: 0;">Store Name</h2>
        </div>
        <div class="receipt-item">Item 1: $19.99</div>
        <div class="receipt-item">Item 2: $29.99</div>
    </div>
</body>
```

### Example 13: Report section with left border and padding

```html
<style>
    .report-section {
        padding-top: 20pt;
        padding-bottom: 20pt;
        padding-left: 30pt;
        padding-right: 25pt;
        background-color: #f9fafb;
        border-left: 6pt solid #2563eb;
        margin-bottom: 20pt;
    }
    .section-title {
        margin: 0 0 15pt 0;
        font-size: 18pt;
        font-weight: bold;
        color: #2563eb;
    }
</style>
<body>
    <div class="report-section">
        <h2 class="section-title">Executive Summary</h2>
        <p style="margin: 0;">Section content with prominent left padding and border.</p>
    </div>
</body>
```

### Example 14: Business card with left padding

```html
<style>
    .business-card {
        width: 350pt;
        height: 200pt;
        padding-top: 25pt;
        padding-bottom: 25pt;
        padding-left: 35pt;
        padding-right: 25pt;
        border: 3pt solid #1e3a8a;
        border-left: 8pt solid #1e3a8a;
    }
    .card-name {
        margin: 0 0 8pt 0;
        font-size: 20pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .card-title {
        margin: 0 0 20pt 0;
        font-size: 12pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="business-card">
        <h2 class="card-name">Michael Chen</h2>
        <p class="card-title">Chief Technology Officer</p>
        <p style="margin: 0; font-size: 10pt;">michael.chen@example.com</p>
    </div>
</body>
```

### Example 15: Certificate with left padding

```html
<style>
    .certificate {
        width: 500pt;
        margin: 50pt auto;
        border: 5pt double #1e3a8a;
    }
    .cert-content {
        padding-top: 50pt;
        padding-bottom: 50pt;
        padding-left: 60pt;
        padding-right: 40pt;
    }
    .cert-title {
        margin: 0 0 30pt 0;
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .cert-recipient {
        margin: 20pt 0;
        padding-left: 20pt;
        font-size: 22pt;
        font-weight: bold;
        border-left: 4pt solid #3b82f6;
    }
</style>
<body>
    <div class="certificate">
        <div class="cert-content">
            <h1 class="cert-title">Certificate of Achievement</h1>
            <p style="margin: 0;">This certifies that</p>
            <p class="cert-recipient">Sarah Johnson</p>
            <p style="margin: 0;">Has successfully completed the program</p>
        </div>
    </div>
</body>
```

---

## See Also

- [padding](/reference/cssproperties/css_prop_padding) - Set all padding shorthand
- [padding-right](/reference/cssproperties/css_prop_padding-right) - Set right padding
- [padding-top](/reference/cssproperties/css_prop_padding-top) - Set top padding
- [padding-bottom](/reference/cssproperties/css_prop_padding-bottom) - Set bottom padding
- [margin-left](/reference/cssproperties/css_prop_margin-left) - Set left margin
- [padding-inline-start](/reference/cssproperties/css_prop_padding-inline-start) - Set inline start padding
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
