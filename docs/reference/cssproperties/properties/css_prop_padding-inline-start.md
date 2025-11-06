---
layout: default
title: padding-inline-start
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# padding-inline-start : Inline Start Padding Property

The `padding-inline-start` property sets the padding at the inline start edge of an element in PDF documents. This is a logical property that maps to either left or right padding depending on the writing direction. In left-to-right (LTR) languages it corresponds to `padding-left`, while in right-to-left (RTL) languages it corresponds to `padding-right`.

## Usage

```css
selector {
    padding-inline-start: value;
}
```

The padding-inline-start property accepts a single length value or percentage that defines the space at the inline start edge.

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
- `0` - No inline start padding

---

## Supported Elements

The `padding-inline-start` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Lists (`<ul>`, `<ol>`)
- All container elements

---

## Notes

- This is a logical property that adapts to text direction
- In LTR contexts (English, Spanish, etc.), it behaves like `padding-left`
- In RTL contexts (Arabic, Hebrew, etc.), it behaves like `padding-right`
- Useful for creating internationalized documents
- Provides better semantic meaning than physical properties
- Simplifies maintenance of bidirectional layouts
- Padding inherits the element's background color
- Percentage values are relative to parent width

---

## Data Binding

The `padding-inline-start` property supports dynamic values through data binding, enabling you to create direction-aware, flexible start internal spacing for icons, indentation, or visual hierarchy based on content requirements.

### Example 1: Alert with dynamic icon space

```html
<style>
    .alert {
        padding-inline-start: {{alert.hasIcon ? '50pt' : '15pt'}};
        padding-inline-end: 15pt;
        padding-block: 12pt;
        background-color: #dcfce7;
        border-inline-start: 4pt solid #16a34a;
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
        "hasIcon": true,
        "title": "Success",
        "message": "Space reserved for icon at inline start"
    }
}
```

### Example 2: Nested content with hierarchy-based padding

```html
<style>
    .content-block {
        padding-inline-start: {{block.level * 20}}pt;
        padding-inline-end: 15pt;
        padding-block: 15pt;
        background-color: {{block.level === 0 ? '#f9fafb' : '#f3f4f6'}};
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="content-block">
        <h4>{{block.title}}</h4>
        <p>{{block.text}}</p>
    </div>
</body>
```

Data context:
```json
{
    "block": {
        "level": 2,
        "title": "Subsection",
        "text": "Padding increases with hierarchy level"
    }
}
```

### Example 3: Localized invoice with direction-aware padding

```html
<style>
    .invoice-section {
        padding-inline-start: {{layout.sectionInset}}pt;
        padding-inline-end: 25pt;
        padding-block: 20pt;
        background-color: #f9fafb;
        border-inline-start: 5pt solid #1e3a8a;
        margin-bottom: 20pt;
        direction: {{locale.direction}};
    }
</style>
<body>
    <div class="invoice-section">
        <h3>{{section.title}}</h3>
        <p>{{section.content}}</p>
    </div>
</body>
```

Data context:
```json
{
    "layout": {
        "sectionInset": 30
    },
    "locale": {
        "direction": "ltr"
    },
    "section": {
        "title": "Payment Details",
        "content": "Account information"
    }
}
```

---

## Examples

### Example 1: Basic inline start padding

```html
<style>
    .box {
        padding-inline-start: 30pt;
        padding-inline-end: 15pt;
        padding-block: 15pt;
        background-color: #dbeafe;
        border-inline-start: 4pt solid #3b82f6;
    }
</style>
<body>
    <div class="box">
        <p style="margin: 0;">Box with inline start padding.</p>
    </div>
</body>
```

### Example 2: Alert with icon space

```html
<style>
    .alert {
        padding-inline-start: 50pt;
        padding-inline-end: 15pt;
        padding-block: 12pt;
        background-color: #dcfce7;
        border-inline-start: 4pt solid #16a34a;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="alert">
        <strong>Success:</strong> Space reserved for icon at inline start.
    </div>
</body>
```

### Example 3: Quote block with inline start padding

```html
<style>
    .quote-block {
        padding-inline-start: 40pt;
        padding-inline-end: 20pt;
        padding-block: 20pt;
        background-color: #f5f5f5;
        border-inline-start: 6pt solid #6366f1;
        font-style: italic;
        margin: 20pt 0;
    }
</style>
<body>
    <div class="quote-block">
        <p style="margin: 0;">"Excellence is not a destination; it is a continuous journey that never ends."</p>
    </div>
</body>
```

### Example 4: Navigation sidebar

```html
<style>
    .sidebar {
        width: 180pt;
        padding-inline-start: 25pt;
        padding-inline-end: 15pt;
        padding-block: 20pt;
        background-color: #f3f4f6;
        border-inline-start: 4pt solid #3b82f6;
    }
    .nav-item {
        padding: 10pt 15pt;
        margin-bottom: 5pt;
        background-color: white;
    }
</style>
<body>
    <div class="sidebar">
        <div class="nav-item">Dashboard</div>
        <div class="nav-item">Settings</div>
    </div>
</body>
```

### Example 5: Form input with inline start padding

```html
<style>
    .form-input {
        padding-inline-start: 15pt;
        padding-inline-end: 12pt;
        padding-block: 10pt;
        border: 1pt solid #d1d5db;
        width: 100%;
    }
    .input-with-icon {
        padding-inline-start: 40pt;
    }
</style>
<body>
    <input class="form-input input-with-icon" type="text" placeholder="Search..." />
</body>
```

### Example 6: Invoice section with inline start padding

```html
<style>
    .invoice-section {
        padding-inline-start: 30pt;
        padding-inline-end: 25pt;
        padding-block: 20pt;
        background-color: #f9fafb;
        border-inline-start: 5pt solid #1e3a8a;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="invoice-section">
        <h3 style="margin: 0 0 10pt 0;">Payment Details</h3>
        <p style="margin: 0;">Account information here.</p>
    </div>
</body>
```

### Example 7: Table cells with inline start padding

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        padding-inline-start: 20pt;
        padding-inline-end: 12pt;
        padding-block: 10pt;
        border: 1pt solid #d1d5db;
        text-align: start;
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
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget</td>
                <td>$29.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 8: Code block with inline start padding

```html
<style>
    .code-block {
        padding-inline-start: 25pt;
        padding-inline-end: 15pt;
        padding-block: 15pt;
        background-color: #1f2937;
        color: #f9fafb;
        font-family: monospace;
        font-size: 10pt;
        border-inline-start: 5pt solid #3b82f6;
    }
</style>
<body>
    <div class="code-block">
        const greeting = "Hello World";<br/>
        console.log(greeting);
    </div>
</body>
```

### Example 9: Product card with inline start padding

```html
<style>
    .product-card {
        padding-inline-start: 30pt;
        padding-inline-end: 20pt;
        padding-block: 20pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        border-inline-start: 4pt solid #16a34a;
        margin-bottom: 15pt;
    }
</style>
<body>
    <div class="product-card">
        <h3 style="margin: 0 0 10pt 0;">Premium Widget</h3>
        <p style="margin: 0; color: #16a34a; font-weight: bold;">$99.99</p>
    </div>
</body>
```

### Example 10: Newsletter article

```html
<style>
    .newsletter-article {
        padding-inline-start: 35pt;
        padding-inline-end: 25pt;
        padding-block: 20pt;
        background-color: #f9fafb;
        border-inline-start: 5pt solid #1e40af;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="newsletter-article">
        <h3 style="margin: 0 0 12pt 0;">Feature Story</h3>
        <p style="margin: 0;">Article content with inline start padding.</p>
    </div>
</body>
```

### Example 11: Receipt with indented items

```html
<style>
    .receipt {
        width: 300pt;
        margin: 40pt auto;
        border: 2pt solid #000;
    }
    .receipt-item {
        padding-inline-start: 25pt;
        padding-inline-end: 20pt;
        padding-block: 10pt;
        border-bottom: 1pt dashed #d1d5db;
    }
</style>
<body>
    <div class="receipt">
        <div class="receipt-item">Item 1: $19.99</div>
        <div class="receipt-item">Item 2: $29.99</div>
    </div>
</body>
```

### Example 12: Report section

```html
<style>
    .report-section {
        padding-inline-start: 30pt;
        padding-inline-end: 25pt;
        padding-block: 20pt;
        background-color: #f9fafb;
        border-inline-start: 6pt solid #2563eb;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="report-section">
        <h2 style="margin: 0 0 15pt 0; color: #2563eb;">Executive Summary</h2>
        <p style="margin: 0;">Key findings and recommendations.</p>
    </div>
</body>
```

### Example 13: Business card

```html
<style>
    .business-card {
        width: 350pt;
        height: 200pt;
        padding-inline-start: 35pt;
        padding-inline-end: 25pt;
        padding-block: 25pt;
        border: 3pt solid #1e3a8a;
        border-inline-start: 8pt solid #1e3a8a;
    }
</style>
<body>
    <div class="business-card">
        <h2 style="margin: 0 0 8pt 0; color: #1e3a8a;">Emily Davis</h2>
        <p style="margin: 0;">Product Manager</p>
    </div>
</body>
```

### Example 14: Certificate

```html
<style>
    .certificate {
        width: 500pt;
        margin: 50pt auto;
        border: 5pt double #1e3a8a;
    }
    .cert-content {
        padding-inline-start: 60pt;
        padding-inline-end: 40pt;
        padding-block: 50pt;
    }
</style>
<body>
    <div class="certificate">
        <div class="cert-content">
            <h1 style="margin: 0 0 30pt 0; color: #1e3a8a;">Certificate of Completion</h1>
            <p style="margin: 0;">Awarded to Jane Smith</p>
        </div>
    </div>
</body>
```

### Example 15: List with inline start padding

```html
<style>
    .custom-list {
        padding-inline-start: 40pt;
        padding-inline-end: 20pt;
        padding-block: 15pt;
        background-color: #f9fafb;
        border-inline-start: 3pt solid #3b82f6;
    }
    .custom-list li {
        margin-bottom: 8pt;
    }
</style>
<body>
    <ul class="custom-list">
        <li>First feature</li>
        <li>Second feature</li>
        <li>Third feature</li>
    </ul>
</body>
```

---

## See Also

- [padding-inline-end](/reference/cssproperties/css_prop_padding-inline-end) - Set inline end padding
- [padding-inline](/reference/cssproperties/css_prop_padding-inline) - Set both inline paddings shorthand
- [padding-left](/reference/cssproperties/css_prop_padding-left) - Set left padding (physical property)
- [padding-right](/reference/cssproperties/css_prop_padding-right) - Set right padding (physical property)
- [margin-inline-start](/reference/cssproperties/css_prop_margin-inline-start) - Set inline start margin
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
