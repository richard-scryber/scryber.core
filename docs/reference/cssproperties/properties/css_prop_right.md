---
layout: default
title: right
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# right : Right Position Property

The `right` property specifies the horizontal offset from the right edge for positioned elements in PDF documents. It works in conjunction with the `position` property to precisely control element placement along the horizontal axis from the right side. This property is essential for creating right-aligned content, stamps, and precisely positioned elements.

## Usage

```css
selector {
    right: value;
}
```

The right property only affects elements with a `position` value of `relative`, `absolute`, or `fixed`. It has no effect on statically positioned elements.

---

## Supported Values

### Length Units
- Points: `10pt`, `20pt`, `50pt`
- Pixels: `10px`, `20px`, `50px`
- Inches: `0.5in`, `1in`, `2in`
- Centimeters: `2cm`, `5cm`
- Millimeters: `10mm`, `20mm`
- Ems: `1em`, `2em`, `3em`
- Percentage: `10%`, `50%`, `100%` (relative to containing block width)

### Special Values
- `0` - No offset from right edge
- `auto` - Browser calculates the position (default)
- Negative values: `-10pt`, `-20px` (moves element rightward beyond edge)

---

## Supported Elements

The `right` property can be applied to:
- Block elements with position other than static (`<div>`, `<section>`, `<article>`)
- Positioned paragraphs (`<p>`)
- Positioned headings (`<h1>` through `<h6>`)
- Positioned images (`<img>`)
- Positioned spans (`<span>`)
- All positioned container elements

---

## Notes

- The `right` property only works with positioned elements (`position: relative`, `absolute`, or `fixed`)
- For `position: relative`, right moves the element from its normal position
- For `position: absolute`, right positions relative to the nearest positioned ancestor
- For `position: fixed`, right positions relative to the page
- Negative values move elements rightward beyond their reference edge
- Percentage values are relative to the containing block's width
- When both `left` and `right` are specified, `left` takes precedence in left-to-right layouts
- Use `right` for right-aligned stamps, side notes, and corner content

---

## Data Binding

The `right` property supports data binding, enabling dynamic horizontal positioning from the right edge based on your data model. This is useful for configurable layouts where stamps, badges, annotations, and right-aligned elements can be positioned through data.

### Example 1: Dynamic stamp positioning from right

```html
<style>
    .document {
        position: relative;
        padding: 30pt;
        min-height: 500pt;
        border: 1pt solid #d1d5db;
    }
    .stamp {
        position: absolute;
        right: {{stamp.rightOffset}}pt;
        top: {{stamp.topOffset}}pt;
        width: {{stamp.size}}pt;
        height: {{stamp.size}}pt;
        border: 3pt solid {{stamp.borderColor}};
        border-radius: 50%;
        text-align: center;
        padding-top: {{stamp.paddingTop}}pt;
        font-weight: bold;
        color: {{stamp.textColor}};
        transform: rotate({{stamp.rotation}}deg);
        opacity: {{stamp.opacity}};
    }
</style>
<body>
    <div class="document">
        <h1>{{document.title}}</h1>
        <p>{{document.content}}</p>
        <div class="stamp">{{stamp.text}}</div>
    </div>
</body>
```

Data model:
```json
{
  "document": {
    "title": "Important Document",
    "content": "Document content..."
  },
  "stamp": {
    "text": "URGENT",
    "rightOffset": 30,
    "topOffset": 50,
    "size": 100,
    "paddingTop": 35,
    "borderColor": "#dc2626",
    "textColor": "#dc2626",
    "rotation": 15,
    "opacity": 0.7
  }
}
```

### Example 2: Configurable right-aligned page numbers and logos

```html
<style>
    .page-number {
        position: fixed;
        right: {{pageNum.rightMargin}}pt;
        top: {{pageNum.topMargin}}pt;
        font-size: {{pageNum.fontSize}}pt;
        color: {{pageNum.color}};
    }
    .company-logo {
        position: fixed;
        right: {{logo.rightOffset}}pt;
        top: {{logo.topOffset}}pt;
        width: {{logo.width}}pt;
        height: {{logo.height}}pt;
        background-color: {{logo.bgColor}};
        color: white;
        text-align: center;
        padding: 10pt;
        font-weight: bold;
        border-radius: 3pt;
    }
    .content {
        margin-right: {{content.rightMargin}}pt;
    }
</style>
<body>
    <div class="page-number">
        Page <span data-page-number="true"></span>
    </div>
    <div class="company-logo">{{logo.text}}</div>
    <div class="content">
        <h1>{{report.title}}</h1>
        <p>{{report.content}}</p>
    </div>
</body>
```

Data model:
```json
{
  "pageNum": {
    "rightMargin": 20,
    "topMargin": 10,
    "fontSize": 10,
    "color": "#6b7280"
  },
  "logo": {
    "text": "LOGO",
    "rightOffset": 20,
    "topOffset": 20,
    "width": 80,
    "height": 40,
    "bgColor": "#1e3a8a"
  },
  "content": {
    "rightMargin": 120
  },
  "report": {
    "title": "Company Report",
    "content": "Report content..."
  }
}
```

### Example 3: Data-driven status badges on right side

```html
<style>
    .page {
        position: relative;
        padding: 30pt;
        min-height: 600pt;
    }
    .badge {
        position: absolute;
        right: {{badges.rightPosition}}pt;
        padding: 8pt 12pt;
        border-radius: 3pt;
        font-weight: bold;
        font-size: {{badges.fontSize}}pt;
    }
    .priority-badge {
        top: {{priority.top}}pt;
        background-color: {{priority.bgColor}};
        color: {{priority.textColor}};
        border: 2pt solid {{priority.borderColor}};
    }
    .status-badge {
        top: {{status.top}}pt;
        background-color: {{status.bgColor}};
        color: {{status.textColor}};
        border: 2pt solid {{status.borderColor}};
    }
</style>
<body>
    <div class="page">
        <div class="badge priority-badge">{{priority.text}}</div>
        <div class="badge status-badge">{{status.text}}</div>
        <h1>{{document.title}}</h1>
        <p>{{document.content}}</p>
    </div>
</body>
```

Data model:
```json
{
  "badges": {
    "rightPosition": 20,
    "fontSize": 10
  },
  "priority": {
    "text": "HIGH",
    "top": 20,
    "bgColor": "#fee2e2",
    "textColor": "#991b1b",
    "borderColor": "#dc2626"
  },
  "status": {
    "text": "ACTIVE",
    "top": 70,
    "bgColor": "#dcfce7",
    "textColor": "#166534",
    "borderColor": "#16a34a"
  },
  "document": {
    "title": "Project Document",
    "content": "Document content with multiple badges..."
  }
}
```

---

## Examples

### Example 1: Fixed page number on right

```html
<style>
    .page-number {
        position: fixed;
        right: 20pt;
        top: 10pt;
        font-size: 10pt;
        color: #6b7280;
    }
    .content {
        margin-top: 40pt;
    }
</style>
<body>
    <div class="page-number">
        Page <span data-page-number="true"></span>
    </div>
    <div class="content">
        <h1>Document Title</h1>
        <p>Main content...</p>
    </div>
</body>
```

### Example 2: Absolute positioned stamp on right

```html
<style>
    .document {
        position: relative;
        padding: 30pt;
        min-height: 500pt;
        border: 1pt solid #d1d5db;
    }
    .stamp {
        position: absolute;
        right: 30pt;
        top: 50pt;
        width: 100pt;
        height: 100pt;
        border: 3pt solid #dc2626;
        border-radius: 50%;
        text-align: center;
        padding-top: 35pt;
        font-weight: bold;
        color: #dc2626;
        transform: rotate(15deg);
        opacity: 0.7;
    }
</style>
<body>
    <div class="document">
        <h1>Important Document</h1>
        <p>Document content...</p>
        <div class="stamp">URGENT</div>
    </div>
</body>
```

### Example 3: Relative positioning with right offset

```html
<style>
    .text-container {
        padding: 10pt;
        background-color: #f9fafb;
        text-align: left;
    }
    .adjusted {
        position: relative;
        right: -10pt;
        color: #2563eb;
        font-weight: bold;
    }
</style>
<body>
    <div class="text-container">
        Normal text <span class="adjusted">shifted right</span> more text.
    </div>
</body>
```

### Example 4: Fixed logo on right

```html
<style>
    .company-logo {
        position: fixed;
        right: 20pt;
        top: 20pt;
        width: 80pt;
        height: 40pt;
        background-color: #1e3a8a;
        color: white;
        text-align: center;
        padding: 10pt;
        font-weight: bold;
        border-radius: 3pt;
    }
    .content {
        margin-right: 120pt;
    }
</style>
<body>
    <div class="company-logo">LOGO</div>
    <div class="content">
        <h1>Company Report</h1>
        <p>Report content...</p>
    </div>
</body>
```

### Example 5: Right-aligned date stamp

```html
<style>
    .header {
        position: relative;
        height: 60pt;
        background-color: #f3f4f6;
        padding: 15pt 20pt;
        margin-bottom: 20pt;
    }
    .date-stamp {
        position: absolute;
        right: 20pt;
        top: 50%;
        transform: translateY(-50%);
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="header">
        <h2>Monthly Report</h2>
        <div class="date-stamp">January 15, 2025</div>
    </div>
</body>
```

### Example 6: Document annotations on right

```html
<style>
    .article {
        position: relative;
        margin-right: 150pt;
        padding: 30pt;
    }
    .annotation {
        position: absolute;
        right: -140pt;
        width: 120pt;
        background-color: #fef3c7;
        border: 1pt solid #f59e0b;
        padding: 8pt;
        font-size: 9pt;
    }
    .note-1 {
        top: 80pt;
    }
    .note-2 {
        top: 250pt;
    }
</style>
<body>
    <div class="article">
        <h1>Research Paper</h1>
        <div class="annotation note-1">
            See reference [1]
        </div>
        <p>First section of the paper...</p>
        <div class="annotation note-2">
            Important finding
        </div>
        <p>Second section with annotations...</p>
    </div>
</body>
```

### Example 7: Right-positioned status badge

```html
<style>
    .invoice {
        position: relative;
        padding: 40pt;
        border: 2pt solid #d1d5db;
    }
    .status {
        position: absolute;
        right: 15pt;
        top: 15pt;
        background-color: #dcfce7;
        color: #166534;
        border: 2pt solid #16a34a;
        padding: 8pt 15pt;
        border-radius: 20pt;
        font-weight: bold;
        font-size: 10pt;
    }
</style>
<body>
    <div class="invoice">
        <div class="status">PAID</div>
        <h1>INVOICE #12345</h1>
        <p>Invoice date: January 15, 2025</p>
        <p>Amount: $1,250.00</p>
    </div>
</body>
```

### Example 8: Fixed corner ribbon on right

```html
<style>
    .ribbon {
        position: fixed;
        right: -30pt;
        top: 20pt;
        background-color: #dc2626;
        color: white;
        padding: 5pt 40pt;
        transform: rotate(45deg);
        font-weight: bold;
        font-size: 11pt;
        box-shadow: 0 2pt 4pt rgba(0,0,0,0.3);
    }
</style>
<body>
    <div class="ribbon">NEW</div>
    <div style="margin-top: 80pt;">
        <h1>Product Launch</h1>
        <p>Introducing our new product line...</p>
    </div>
</body>
```

### Example 9: Right margin chapter markers

```html
<style>
    .content {
        position: relative;
        margin-right: 100pt;
        padding: 30pt;
    }
    .chapter-marker {
        position: absolute;
        right: -90pt;
        width: 70pt;
        text-align: center;
        background-color: #1e3a8a;
        color: white;
        padding: 10pt;
        font-weight: bold;
        border-radius: 5pt;
    }
    .marker-1 {
        top: 0;
    }
    .marker-2 {
        top: 200pt;
    }
</style>
<body>
    <div class="content">
        <div class="chapter-marker marker-1">Ch 1</div>
        <h1>Chapter One</h1>
        <p>Chapter content begins...</p>
        <div class="chapter-marker marker-2">Ch 2</div>
        <h1>Chapter Two</h1>
        <p>Next chapter content...</p>
    </div>
</body>
```

### Example 10: Stacked elements aligned right

```html
<style>
    .stack {
        position: relative;
        height: 300pt;
        width: 100%;
    }
    .card {
        position: absolute;
        width: 250pt;
        height: 150pt;
        border: 1pt solid #d1d5db;
        background-color: white;
        padding: 15pt;
        box-shadow: 2pt 2pt 4pt rgba(0,0,0,0.1);
    }
    .card-1 {
        right: 0;
        top: 0;
        z-index: 3;
    }
    .card-2 {
        right: 15pt;
        top: 15pt;
        z-index: 2;
        background-color: #f9fafb;
    }
    .card-3 {
        right: 30pt;
        top: 30pt;
        z-index: 1;
        background-color: #f3f4f6;
    }
</style>
<body>
    <div class="stack">
        <div class="card card-3"></div>
        <div class="card card-2"></div>
        <div class="card card-1">
            <h3>Top Card</h3>
            <p>Right-aligned stack</p>
        </div>
    </div>
</body>
```

### Example 11: Negative right offset for overflow

```html
<style>
    .container {
        position: relative;
        width: 400pt;
        height: 200pt;
        border: 1pt solid #d1d5db;
        overflow: hidden;
        margin-right: 50pt;
    }
    .callout {
        position: absolute;
        right: -40pt;
        top: 50%;
        transform: translateY(-50%);
        background-color: #dc2626;
        color: white;
        padding: 10pt 50pt 10pt 10pt;
        border-radius: 5pt 0 0 5pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="container">
        <h2>Content Box</h2>
        <p>Main content here...</p>
        <div class="callout">SALE</div>
    </div>
</body>
```

### Example 12: Percentage-based right positioning

```html
<style>
    .banner {
        position: relative;
        height: 120pt;
        background-color: #1e3a8a;
        color: white;
    }
    .banner-text {
        position: absolute;
        right: 10%;
        top: 50%;
        transform: translateY(-50%);
        text-align: right;
    }
</style>
<body>
    <div class="banner">
        <div class="banner-text">
            <h1>Welcome</h1>
            <p>Right-aligned content at 10% from edge</p>
        </div>
    </div>
</body>
```

### Example 13: Fixed side panel on right

```html
<style>
    .side-panel {
        position: fixed;
        right: 0;
        top: 0;
        width: 120pt;
        height: 100%;
        background-color: #f3f4f6;
        padding: 15pt;
        border-left: 1pt solid #d1d5db;
    }
    .main-content {
        margin-right: 140pt;
        padding: 20pt;
    }
</style>
<body>
    <div class="side-panel">
        <h4>Quick Links</h4>
        <ul style="font-size: 9pt;">
            <li>Link 1</li>
            <li>Link 2</li>
            <li>Link 3</li>
        </ul>
    </div>
    <div class="main-content">
        <h1>Main Document</h1>
        <p>Content with fixed right panel...</p>
    </div>
</body>
```

### Example 14: Right-aligned price tags

```html
<style>
    .product-list {
        position: relative;
        padding: 20pt;
    }
    .product-item {
        position: relative;
        padding: 12pt;
        margin-bottom: 10pt;
        border: 1pt solid #d1d5db;
        min-height: 40pt;
    }
    .price-tag {
        position: absolute;
        right: 15pt;
        top: 50%;
        transform: translateY(-50%);
        background-color: #16a34a;
        color: white;
        padding: 5pt 12pt;
        border-radius: 15pt;
        font-weight: bold;
        font-size: 12pt;
    }
</style>
<body>
    <div class="product-list">
        <div class="product-item">
            <h3>Product One</h3>
            <p>Product description</p>
            <div class="price-tag">$29.99</div>
        </div>
        <div class="product-item">
            <h3>Product Two</h3>
            <p>Product description</p>
            <div class="price-tag">$39.99</div>
        </div>
    </div>
</body>
```

### Example 15: Multiple right-positioned elements

```html
<style>
    .page {
        position: relative;
        padding: 30pt;
        min-height: 600pt;
    }
    .badge {
        position: absolute;
        padding: 8pt 12pt;
        border-radius: 3pt;
        font-weight: bold;
        font-size: 10pt;
    }
    .priority-high {
        right: 20pt;
        top: 20pt;
        background-color: #fee2e2;
        color: #991b1b;
        border: 2pt solid #dc2626;
    }
    .status-active {
        right: 20pt;
        top: 70pt;
        background-color: #dcfce7;
        color: #166534;
        border: 2pt solid #16a34a;
    }
    .category-tag {
        right: 20pt;
        top: 120pt;
        background-color: #dbeafe;
        color: #1e40af;
        border: 2pt solid #2563eb;
    }
</style>
<body>
    <div class="page">
        <div class="badge priority-high">HIGH</div>
        <div class="badge status-active">ACTIVE</div>
        <div class="badge category-tag">TECH</div>
        <h1>Project Document</h1>
        <p>Document content with multiple badges...</p>
    </div>
</body>
```

---

## See Also

- [position](/reference/cssproperties/css_prop_position) - Set positioning method
- [top](/reference/cssproperties/css_prop_top) - Set top offset for positioned elements
- [left](/reference/cssproperties/css_prop_left) - Set left offset for positioned elements
- [bottom](/reference/cssproperties/css_prop_bottom) - Set bottom offset for positioned elements
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
