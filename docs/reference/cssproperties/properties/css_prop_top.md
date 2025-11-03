---
layout: default
title: top
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# top : Top Position Property

The `top` property specifies the vertical offset from the top edge for positioned elements in PDF documents. It works in conjunction with the `position` property to precisely control element placement. This property is essential for creating headers, fixed content, and precisely positioned elements.

## Usage

```css
selector {
    top: value;
}
```

The top property only affects elements with a `position` value of `relative`, `absolute`, or `fixed`. It has no effect on statically positioned elements.

---

## Supported Values

### Length Units
- Points: `10pt`, `20pt`, `50pt`
- Pixels: `10px`, `20px`, `50px`
- Inches: `0.5in`, `1in`, `2in`
- Centimeters: `2cm`, `5cm`
- Millimeters: `10mm`, `20mm`
- Ems: `1em`, `2em`, `3em`
- Percentage: `10%`, `50%`, `100%` (relative to containing block height)

### Special Values
- `0` - No offset from top edge
- `auto` - Browser calculates the position (default)
- Negative values: `-10pt`, `-20px` (moves element upward)

---

## Supported Elements

The `top` property can be applied to:
- Block elements with position other than static (`<div>`, `<section>`, `<article>`)
- Positioned paragraphs (`<p>`)
- Positioned headings (`<h1>` through `<h6>`)
- Positioned images (`<img>`)
- Positioned spans (`<span>`)
- All positioned container elements

---

## Notes

- The `top` property only works with positioned elements (`position: relative`, `absolute`, or `fixed`)
- For `position: relative`, top moves the element from its normal position
- For `position: absolute`, top positions relative to the nearest positioned ancestor
- For `position: fixed`, top positions relative to the page
- Negative values move elements upward from their reference point
- Percentage values are relative to the containing block's height
- When both `top` and `bottom` are specified, `top` takes precedence for most positioning contexts
- Use `top` for fixed headers, page numbers, and precisely positioned content

---

## Data Binding

The `top` property supports data binding, enabling dynamic vertical positioning based on your data model. This allows you to create flexible, configurable layouts where header offsets, element positions, and spacing can be controlled through data.

### Example 1: Dynamic header offset based on configuration

```html
<style>
    .page-header {
        position: fixed;
        top: {{header.offset}}pt;
        left: 0;
        right: 0;
        height: {{header.height}}pt;
        background-color: {{header.backgroundColor}};
        color: white;
        padding: {{header.padding}}pt 20pt;
    }
    .content {
        margin-top: {{content.topMargin}}pt;
    }
</style>
<body>
    <div class="page-header">
        <h2>{{header.title}}</h2>
    </div>
    <div class="content">
        <p>{{content.text}}</p>
    </div>
</body>
```

Data model:
```json
{
  "header": {
    "offset": 0,
    "height": 50,
    "padding": 12,
    "backgroundColor": "#1e3a8a",
    "title": "Annual Report 2025"
  },
  "content": {
    "topMargin": 70,
    "text": "Document content begins here..."
  }
}
```

### Example 2: Configurable stamp vertical positioning

```html
<style>
    .container {
        position: relative;
        height: 400pt;
        border: 1pt solid #d1d5db;
    }
    .stamp {
        position: absolute;
        top: {{stamp.topPosition}}pt;
        right: {{stamp.rightPosition}}pt;
        background-color: {{stamp.bgColor}};
        border: 2pt solid {{stamp.borderColor}};
        padding: 10pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="container">
        <div class="stamp">{{stamp.text}}</div>
        <p>{{document.content}}</p>
    </div>
</body>
```

Data model:
```json
{
  "stamp": {
    "text": "APPROVED",
    "topPosition": 20,
    "rightPosition": 20,
    "bgColor": "#dcfce7",
    "borderColor": "#16a34a"
  },
  "document": {
    "content": "Document content..."
  }
}
```

### Example 3: Data-driven watermark placement

```html
<style>
    .watermark {
        position: fixed;
        top: {{watermark.vertical}}%;
        left: {{watermark.horizontal}}%;
        transform: translate(-50%, -50%);
        font-size: {{watermark.size}}pt;
        color: rgba(0, 0, 0, {{watermark.opacity}});
        font-weight: bold;
        transform: rotate({{watermark.angle}}deg);
    }
</style>
<body>
    <div class="watermark">{{watermark.text}}</div>
    <div>
        <h1>{{document.title}}</h1>
        <p>{{document.body}}</p>
    </div>
</body>
```

Data model:
```json
{
  "watermark": {
    "text": "CONFIDENTIAL",
    "vertical": 50,
    "horizontal": 50,
    "size": 48,
    "opacity": 0.1,
    "angle": -45
  },
  "document": {
    "title": "Confidential Report",
    "body": "Sensitive information..."
  }
}
```

---

## Examples

### Example 1: Fixed header at top of page

```html
<style>
    .page-header {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        height: 50pt;
        background-color: #1e3a8a;
        color: white;
        padding: 12pt 20pt;
    }
    .content {
        margin-top: 70pt;
    }
</style>
<body>
    <div class="page-header">
        <h2>Annual Report 2025</h2>
    </div>
    <div class="content">
        <p>Document content begins here...</p>
    </div>
</body>
```

### Example 2: Absolute positioning from top

```html
<style>
    .container {
        position: relative;
        height: 400pt;
        border: 1pt solid #d1d5db;
    }
    .stamp {
        position: absolute;
        top: 20pt;
        right: 20pt;
        background-color: #dcfce7;
        border: 2pt solid #16a34a;
        padding: 10pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="container">
        <div class="stamp">APPROVED</div>
        <p>Document content...</p>
    </div>
</body>
```

### Example 3: Relative positioning with top offset

```html
<style>
    .baseline {
        border-bottom: 1pt solid #d1d5db;
        padding-bottom: 10pt;
    }
    .adjusted {
        position: relative;
        top: -5pt;
        color: #2563eb;
        font-weight: bold;
    }
</style>
<body>
    <div class="baseline">
        Normal text <span class="adjusted">raised text</span> more normal text.
    </div>
</body>
```

### Example 4: Fixed page number at top

```html
<style>
    .page-number {
        position: fixed;
        top: 10pt;
        right: 20pt;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="page-number">
        Page <span data-page-number="true"></span>
    </div>
    <div style="margin-top: 40pt;">
        <h1>Document Content</h1>
        <p>Main content here...</p>
    </div>
</body>
```

### Example 5: Top-aligned watermark

```html
<style>
    .watermark {
        position: fixed;
        top: 100pt;
        left: 50%;
        transform: translateX(-50%);
        font-size: 48pt;
        color: rgba(0, 0, 0, 0.1);
        font-weight: bold;
    }
</style>
<body>
    <div class="watermark">CONFIDENTIAL</div>
    <div>
        <h1>Confidential Report</h1>
        <p>Sensitive information...</p>
    </div>
</body>
```

### Example 6: Document title banner

```html
<style>
    .title-banner {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        height: 80pt;
        background: linear-gradient(to right, #1e3a8a, #3b82f6);
        color: white;
        padding: 20pt;
        text-align: center;
    }
    .content {
        margin-top: 100pt;
    }
</style>
<body>
    <div class="title-banner">
        <h1>Company Name</h1>
        <p>Document Title</p>
    </div>
    <div class="content">
        <p>Main document content...</p>
    </div>
</body>
```

### Example 7: Absolute annotation from top

```html
<style>
    .document {
        position: relative;
        padding: 30pt;
        min-height: 500pt;
    }
    .note {
        position: absolute;
        top: 50pt;
        left: -80pt;
        width: 60pt;
        font-size: 8pt;
        color: #dc2626;
        text-align: right;
    }
</style>
<body>
    <div class="document">
        <div class="note">Important!</div>
        <h1>Section Title</h1>
        <p>Main content with side note...</p>
    </div>
</body>
```

### Example 8: Top-positioned status badge

```html
<style>
    .invoice {
        position: relative;
        padding: 40pt;
        border: 1pt solid #d1d5db;
    }
    .status-badge {
        position: absolute;
        top: 15pt;
        right: 15pt;
        background-color: #dcfce7;
        color: #166534;
        border: 2pt solid #16a34a;
        padding: 5pt 15pt;
        border-radius: 15pt;
        font-weight: bold;
        font-size: 10pt;
    }
</style>
<body>
    <div class="invoice">
        <div class="status-badge">PAID</div>
        <h1>INVOICE #12345</h1>
        <p>Invoice details...</p>
    </div>
</body>
```

### Example 9: Fixed security classification

```html
<style>
    .classification {
        position: fixed;
        top: 5pt;
        left: 0;
        right: 0;
        background-color: #fee2e2;
        color: #991b1b;
        text-align: center;
        padding: 5pt;
        font-size: 9pt;
        font-weight: bold;
        border-bottom: 2pt solid #dc2626;
    }
    .content {
        margin-top: 35pt;
    }
</style>
<body>
    <div class="classification">
        CONFIDENTIAL - INTERNAL USE ONLY
    </div>
    <div class="content">
        <h1>Internal Document</h1>
        <p>Confidential content...</p>
    </div>
</body>
```

### Example 10: Stacked elements with top offsets

```html
<style>
    .stack-container {
        position: relative;
        height: 300pt;
        width: 250pt;
        margin: 30pt;
    }
    .layer {
        position: absolute;
        width: 200pt;
        height: 150pt;
        border: 1pt solid #d1d5db;
        background-color: white;
        padding: 10pt;
    }
    .layer-1 {
        top: 0;
        left: 0;
        z-index: 3;
    }
    .layer-2 {
        top: 15pt;
        left: 15pt;
        z-index: 2;
        background-color: #f3f4f6;
    }
    .layer-3 {
        top: 30pt;
        left: 30pt;
        z-index: 1;
        background-color: #e5e7eb;
    }
</style>
<body>
    <div class="stack-container">
        <div class="layer layer-3"></div>
        <div class="layer layer-2"></div>
        <div class="layer layer-1">
            <h3>Front Layer</h3>
            <p>Top layer content</p>
        </div>
    </div>
</body>
```

### Example 11: Negative top offset for overlap

```html
<style>
    .header-block {
        background-color: #1e3a8a;
        color: white;
        padding: 30pt;
        height: 100pt;
    }
    .content-card {
        position: relative;
        top: -30pt;
        background-color: white;
        border: 1pt solid #d1d5db;
        padding: 20pt;
        margin: 0 40pt;
        box-shadow: 0 4pt 6pt rgba(0,0,0,0.1);
    }
</style>
<body>
    <div class="header-block">
        <h1>Welcome</h1>
    </div>
    <div class="content-card">
        <h2>Card Title</h2>
        <p>This card overlaps the header above.</p>
    </div>
</body>
```

### Example 12: Percentage-based top positioning

```html
<style>
    .container {
        position: relative;
        height: 400pt;
        border: 2pt solid #1e3a8a;
        background-color: #f9fafb;
    }
    .centered-box {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        background-color: white;
        border: 1pt solid #d1d5db;
        padding: 20pt;
        text-align: center;
    }
</style>
<body>
    <div class="container">
        <div class="centered-box">
            <h2>Centered Content</h2>
            <p>Positioned at 50% from top</p>
        </div>
    </div>
</body>
```

### Example 13: Fixed corner ribbon

```html
<style>
    .corner-ribbon {
        position: fixed;
        top: 15pt;
        right: -35pt;
        background-color: #dc2626;
        color: white;
        padding: 5pt 40pt;
        transform: rotate(45deg);
        font-weight: bold;
        font-size: 11pt;
    }
</style>
<body>
    <div class="corner-ribbon">NEW</div>
    <div style="margin-top: 60pt;">
        <h1>Product Catalog</h1>
        <p>Browse our latest products...</p>
    </div>
</body>
```

### Example 14: Top-aligned alert banner

```html
<style>
    .alert-banner {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        background-color: #fef3c7;
        border-bottom: 2pt solid #f59e0b;
        padding: 10pt 20pt;
    }
    .alert-icon {
        color: #f59e0b;
        font-weight: bold;
        margin-right: 8pt;
    }
    .main-content {
        margin-top: 50pt;
    }
</style>
<body>
    <div class="alert-banner">
        <span class="alert-icon">⚠</span>
        <span>Important: Please review the updated terms.</span>
    </div>
    <div class="main-content">
        <h1>Document Content</h1>
        <p>Main text here...</p>
    </div>
</body>
```

### Example 15: Multiple top-positioned elements

```html
<style>
    .page {
        position: relative;
        min-height: 600pt;
        padding: 20pt;
    }
    .header-logo {
        position: absolute;
        top: 10pt;
        left: 20pt;
        font-size: 18pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .header-date {
        position: absolute;
        top: 10pt;
        right: 20pt;
        font-size: 10pt;
        color: #6b7280;
    }
    .title {
        position: absolute;
        top: 60pt;
        left: 20pt;
        right: 20pt;
    }
    .content {
        margin-top: 120pt;
    }
</style>
<body>
    <div class="page">
        <div class="header-logo">COMPANY</div>
        <div class="header-date">January 15, 2025</div>
        <div class="title">
            <h1>Document Title</h1>
        </div>
        <div class="content">
            <p>Main document content...</p>
        </div>
    </div>
</body>
```

---

## See Also

- [position](/reference/cssproperties/css_prop_position) - Set positioning method
- [left](/reference/cssproperties/css_prop_left) - Set left offset for positioned elements
- [right](/reference/cssproperties/css_prop_right) - Set right offset for positioned elements
- [bottom](/reference/cssproperties/css_prop_bottom) - Set bottom offset for positioned elements
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
