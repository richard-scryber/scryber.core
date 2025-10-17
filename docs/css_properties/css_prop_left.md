---
layout: default
title: left
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# left : Left Position Property

The `left` property specifies the horizontal offset from the left edge for positioned elements in PDF documents. It works in conjunction with the `position` property to precisely control element placement along the horizontal axis. This property is essential for creating sidebars, precise layouts, and horizontally positioned content.

## Usage

```css
selector {
    left: value;
}
```

The left property only affects elements with a `position` value of `relative`, `absolute`, or `fixed`. It has no effect on statically positioned elements.

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
- `0` - No offset from left edge
- `auto` - Browser calculates the position (default)
- Negative values: `-10pt`, `-20px` (moves element leftward)

---

## Supported Elements

The `left` property can be applied to:
- Block elements with position other than static (`<div>`, `<section>`, `<article>`)
- Positioned paragraphs (`<p>`)
- Positioned headings (`<h1>` through `<h6>`)
- Positioned images (`<img>`)
- Positioned spans (`<span>`)
- All positioned container elements

---

## Notes

- The `left` property only works with positioned elements (`position: relative`, `absolute`, or `fixed`)
- For `position: relative`, left moves the element from its normal position
- For `position: absolute`, left positions relative to the nearest positioned ancestor
- For `position: fixed`, left positions relative to the page
- Negative values move elements leftward from their reference point
- Percentage values are relative to the containing block's width
- When both `left` and `right` are specified, `left` takes precedence in left-to-right layouts
- Use `left` for fixed sidebars, margin notes, and horizontally positioned content

---

## Data Binding

The `left` property supports data binding, allowing dynamic horizontal positioning from the left edge based on your data model. This enables configurable layouts where element positions, margins, and alignments can be controlled through data.

### Example 1: Configurable sidebar offset

```html
<style>
    .sidebar {
        position: fixed;
        left: {{sidebar.offset}}pt;
        top: 0;
        width: {{sidebar.width}}pt;
        height: 100%;
        background-color: {{sidebar.backgroundColor}};
        padding: {{sidebar.padding}}pt;
        border-right: 1pt solid #d1d5db;
    }
    .main-content {
        margin-left: {{content.leftMargin}}pt;
        padding: 20pt;
    }
</style>
<body>
    <div class="sidebar">
        <h3>{{sidebar.title}}</h3>
        <ul>
            <li>{{nav.item1}}</li>
            <li>{{nav.item2}}</li>
            <li>{{nav.item3}}</li>
        </ul>
    </div>
    <div class="main-content">
        <h1>{{content.title}}</h1>
        <p>{{content.body}}</p>
    </div>
</body>
```

Data model:
```json
{
  "sidebar": {
    "offset": 0,
    "width": 150,
    "backgroundColor": "#f3f4f6",
    "padding": 20,
    "title": "Navigation"
  },
  "nav": {
    "item1": "Home",
    "item2": "About",
    "item3": "Contact"
  },
  "content": {
    "leftMargin": 180,
    "title": "Main Content",
    "body": "Document content with fixed left sidebar..."
  }
}
```

### Example 2: Dynamic badge positioning from left

```html
<style>
    .container {
        position: relative;
        height: 300pt;
        border: 1pt solid #d1d5db;
        padding: 20pt;
    }
    .badge {
        position: absolute;
        left: {{badge.leftPos}}pt;
        top: {{badge.topPos}}pt;
        background-color: {{badge.bgColor}};
        border: 2pt solid {{badge.borderColor}};
        padding: 8pt 12pt;
        border-radius: 5pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="container">
        <div class="badge">{{badge.text}}</div>
        <h2>{{product.title}}</h2>
        <p>{{product.description}}</p>
    </div>
</body>
```

Data model:
```json
{
  "badge": {
    "text": "NEW",
    "leftPos": 20,
    "topPos": 20,
    "bgColor": "#dbeafe",
    "borderColor": "#2563eb"
  },
  "product": {
    "title": "Product Feature",
    "description": "Feature description..."
  }
}
```

### Example 3: Configurable logo and margin notes

```html
<style>
    .document {
        position: relative;
        margin-left: {{document.leftMargin}}pt;
        padding: 30pt;
    }
    .margin-note {
        position: absolute;
        left: {{note.leftOffset}}pt;
        width: {{note.width}}pt;
        font-size: {{note.fontSize}}pt;
        color: {{note.color}};
        border-left: 2pt solid {{note.borderColor}};
        padding-left: 8pt;
    }
    .note-1 {
        top: {{note1.top}}pt;
    }
    .note-2 {
        top: {{note2.top}}pt;
    }
</style>
<body>
    <div class="document">
        <div class="margin-note note-1">{{note1.text}}</div>
        <h1>{{article.title}}</h1>
        <p>{{article.paragraph1}}</p>
        <div class="margin-note note-2">{{note2.text}}</div>
        <p>{{article.paragraph2}}</p>
    </div>
</body>
```

Data model:
```json
{
  "document": {
    "leftMargin": 120
  },
  "note": {
    "leftOffset": -100,
    "width": 80,
    "fontSize": 9,
    "color": "#dc2626",
    "borderColor": "#dc2626"
  },
  "note1": {
    "top": 50,
    "text": "Key Point"
  },
  "note2": {
    "top": 200,
    "text": "Important!"
  },
  "article": {
    "title": "Main Article",
    "paragraph1": "First paragraph with margin note...",
    "paragraph2": "Second paragraph with another note..."
  }
}
```

---

## Examples

### Example 1: Fixed sidebar on left

```html
<style>
    .sidebar {
        position: fixed;
        left: 0;
        top: 0;
        width: 150pt;
        height: 100%;
        background-color: #f3f4f6;
        padding: 20pt;
        border-right: 1pt solid #d1d5db;
    }
    .main-content {
        margin-left: 180pt;
        padding: 20pt;
    }
</style>
<body>
    <div class="sidebar">
        <h3>Navigation</h3>
        <ul>
            <li>Home</li>
            <li>About</li>
            <li>Contact</li>
        </ul>
    </div>
    <div class="main-content">
        <h1>Main Content</h1>
        <p>Document content with fixed left sidebar...</p>
    </div>
</body>
```

### Example 2: Absolute positioning from left

```html
<style>
    .container {
        position: relative;
        height: 300pt;
        border: 1pt solid #d1d5db;
        padding: 20pt;
    }
    .badge {
        position: absolute;
        left: 20pt;
        top: 20pt;
        background-color: #dbeafe;
        border: 2pt solid #2563eb;
        padding: 8pt 12pt;
        border-radius: 5pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="container">
        <div class="badge">NEW</div>
        <h2>Product Feature</h2>
        <p>Feature description...</p>
    </div>
</body>
```

### Example 3: Relative positioning with left offset

```html
<style>
    .text-line {
        padding: 10pt;
        background-color: #f9fafb;
    }
    .indented {
        position: relative;
        left: 20pt;
        color: #2563eb;
    }
</style>
<body>
    <div class="text-line">
        Normal text <span class="indented">indented text</span> more text.
    </div>
</body>
```

### Example 4: Fixed page number on left margin

```html
<style>
    .page-number-left {
        position: fixed;
        left: 10pt;
        top: 50%;
        transform: translateY(-50%);
        font-size: 10pt;
        color: #6b7280;
        writing-mode: vertical-lr;
    }
    .content {
        margin-left: 40pt;
    }
</style>
<body>
    <div class="page-number-left">
        Page <span data-page-number="true"></span>
    </div>
    <div class="content">
        <h1>Document Title</h1>
        <p>Main content...</p>
    </div>
</body>
```

### Example 5: Left-aligned watermark

```html
<style>
    .watermark {
        position: fixed;
        left: 50pt;
        top: 50%;
        transform: translateY(-50%) rotate(-90deg);
        font-size: 48pt;
        color: rgba(0, 0, 0, 0.08);
        font-weight: bold;
    }
</style>
<body>
    <div class="watermark">DRAFT</div>
    <div>
        <h1>Draft Document</h1>
        <p>Content under review...</p>
    </div>
</body>
```

### Example 6: Document margin notes

```html
<style>
    .document {
        position: relative;
        margin-left: 120pt;
        padding: 30pt;
    }
    .margin-note {
        position: absolute;
        left: -100pt;
        width: 80pt;
        font-size: 9pt;
        color: #dc2626;
        border-left: 2pt solid #dc2626;
        padding-left: 8pt;
    }
    .note-1 {
        top: 50pt;
    }
    .note-2 {
        top: 200pt;
    }
</style>
<body>
    <div class="document">
        <div class="margin-note note-1">Key Point</div>
        <h1>Main Article</h1>
        <p>First paragraph with margin note...</p>
        <div class="margin-note note-2">Important!</div>
        <p>Second paragraph with another note...</p>
    </div>
</body>
```

### Example 7: Left-positioned logo

```html
<style>
    .header {
        position: relative;
        height: 80pt;
        background-color: #1e3a8a;
        margin-bottom: 20pt;
    }
    .logo {
        position: absolute;
        left: 30pt;
        top: 50%;
        transform: translateY(-50%);
        color: white;
        font-size: 20pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="header">
        <div class="logo">COMPANY</div>
    </div>
    <div>
        <h1>Document Title</h1>
        <p>Content here...</p>
    </div>
</body>
```

### Example 8: Left-aligned stamp

```html
<style>
    .contract {
        position: relative;
        padding: 40pt;
        border: 1pt solid #d1d5db;
        min-height: 500pt;
    }
    .stamp {
        position: absolute;
        left: 40pt;
        bottom: 40pt;
        width: 120pt;
        height: 120pt;
        border: 4pt solid #16a34a;
        border-radius: 50%;
        text-align: center;
        padding-top: 40pt;
        font-weight: bold;
        font-size: 16pt;
        color: #16a34a;
        transform: rotate(-15deg);
        opacity: 0.7;
    }
</style>
<body>
    <div class="contract">
        <h1>Contract Agreement</h1>
        <p>This agreement is made...</p>
        <div class="stamp">APPROVED</div>
    </div>
</body>
```

### Example 9: Fixed status indicator

```html
<style>
    .status-bar {
        position: fixed;
        left: 0;
        top: 0;
        width: 5pt;
        height: 100%;
        background-color: #16a34a;
    }
    .content {
        margin-left: 20pt;
    }
</style>
<body>
    <div class="status-bar"></div>
    <div class="content">
        <h1>Approved Document</h1>
        <p>This document has been approved...</p>
    </div>
</body>
```

### Example 10: Stacked cards with left offsets

```html
<style>
    .card-container {
        position: relative;
        height: 250pt;
        width: 400pt;
        margin: 50pt;
    }
    .card {
        position: absolute;
        width: 300pt;
        height: 200pt;
        border: 1pt solid #d1d5db;
        background-color: white;
        padding: 15pt;
        box-shadow: 2pt 2pt 4pt rgba(0,0,0,0.1);
    }
    .card-1 {
        left: 0;
        top: 0;
        z-index: 3;
    }
    .card-2 {
        left: 20pt;
        top: 10pt;
        z-index: 2;
        background-color: #f3f4f6;
    }
    .card-3 {
        left: 40pt;
        top: 20pt;
        z-index: 1;
        background-color: #e5e7eb;
    }
</style>
<body>
    <div class="card-container">
        <div class="card card-3"></div>
        <div class="card card-2"></div>
        <div class="card card-1">
            <h3>Card Title</h3>
            <p>Front card content</p>
        </div>
    </div>
</body>
```

### Example 11: Negative left offset for margin content

```html
<style>
    .content-block {
        position: relative;
        margin-left: 80pt;
        padding: 20pt;
        border-left: 3pt solid #2563eb;
    }
    .section-number {
        position: absolute;
        left: -60pt;
        top: 0;
        font-size: 24pt;
        font-weight: bold;
        color: #2563eb;
    }
</style>
<body>
    <div class="content-block">
        <div class="section-number">01</div>
        <h2>Section Title</h2>
        <p>Section content with number in left margin...</p>
    </div>
</body>
```

### Example 12: Percentage-based left positioning

```html
<style>
    .banner {
        position: relative;
        height: 150pt;
        background-color: #f3f4f6;
    }
    .banner-content {
        position: absolute;
        left: 25%;
        top: 50%;
        transform: translateY(-50%);
        width: 50%;
        text-align: center;
    }
</style>
<body>
    <div class="banner">
        <div class="banner-content">
            <h1>Centered Banner</h1>
            <p>Positioned at 25% from left</p>
        </div>
    </div>
</body>
```

### Example 13: Fixed chapter marker

```html
<style>
    .chapter-marker {
        position: fixed;
        left: 0;
        top: 100pt;
        background-color: #1e3a8a;
        color: white;
        padding: 10pt 15pt 10pt 10pt;
        border-radius: 0 5pt 5pt 0;
        font-weight: bold;
    }
    .content {
        margin-left: 20pt;
    }
</style>
<body>
    <div class="chapter-marker">Ch. 1</div>
    <div class="content">
        <h1>Chapter One: Introduction</h1>
        <p>Chapter content begins here...</p>
    </div>
</body>
```

### Example 14: Left-aligned floating box

```html
<style>
    .article {
        position: relative;
        padding: 30pt;
    }
    .info-box {
        position: absolute;
        left: 0;
        top: 100pt;
        width: 150pt;
        background-color: #dbeafe;
        border: 1pt solid #2563eb;
        padding: 12pt;
        font-size: 10pt;
    }
    .article-text {
        margin-left: 170pt;
    }
</style>
<body>
    <div class="article">
        <div class="info-box">
            <h4>Did You Know?</h4>
            <p>Interesting fact about the topic...</p>
        </div>
        <div class="article-text">
            <h1>Article Title</h1>
            <p>Main article text flows to the right of the info box...</p>
        </div>
    </div>
</body>
```

### Example 15: Multiple left-positioned elements

```html
<style>
    .report {
        position: relative;
        padding: 40pt;
        padding-left: 100pt;
        min-height: 600pt;
    }
    .marker {
        position: absolute;
        width: 60pt;
        text-align: center;
        font-weight: bold;
        padding: 5pt;
        border-radius: 3pt;
    }
    .marker-priority {
        left: 20pt;
        top: 80pt;
        background-color: #fee2e2;
        color: #991b1b;
    }
    .marker-status {
        left: 20pt;
        top: 200pt;
        background-color: #dcfce7;
        color: #166534;
    }
    .marker-review {
        left: 20pt;
        top: 320pt;
        background-color: #fef3c7;
        color: #92400e;
    }
</style>
<body>
    <div class="report">
        <div class="marker marker-priority">HIGH</div>
        <div class="marker marker-status">DONE</div>
        <div class="marker marker-review">REVIEW</div>
        <h1>Project Status Report</h1>
        <h2>Section 1</h2>
        <p>First section content...</p>
        <h2>Section 2</h2>
        <p>Second section content...</p>
        <h2>Section 3</h2>
        <p>Third section content...</p>
    </div>
</body>
```

---

## See Also

- [position](/reference/cssproperties/css_prop_position) - Set positioning method
- [top](/reference/cssproperties/css_prop_top) - Set top offset for positioned elements
- [right](/reference/cssproperties/css_prop_right) - Set right offset for positioned elements
- [bottom](/reference/cssproperties/css_prop_bottom) - Set bottom offset for positioned elements
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
