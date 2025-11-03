---
layout: default
title: position
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# position : Position Property

The `position` property specifies the positioning method used for an element in PDF documents. It determines how an element is positioned on the page and how it interacts with the normal document flow. This is fundamental for creating complex layouts, overlays, headers, footers, and watermarks.

## Usage

```css
selector {
    position: value;
}
```

The position property accepts four distinct values that control how the element is positioned relative to its normal position, ancestors, or the page itself.

---

## Supported Values

### static (default)
The element is positioned according to the normal flow of the document. The `top`, `right`, `bottom`, and `left` properties have no effect. This is the default positioning behavior.

### relative
The element is positioned relative to its normal position in the document flow. Using `top`, `right`, `bottom`, and `left` offsets the element from where it would normally be, but the space it originally occupied is preserved.

### absolute
The element is removed from the normal document flow and positioned relative to its nearest positioned ancestor (an ancestor with `position` other than `static`). If no positioned ancestor exists, it positions relative to the page.

### fixed
The element is removed from the normal document flow and positioned relative to the page itself. Fixed elements appear in the same position on every page, making them ideal for headers, footers, page numbers, and watermarks.

---

## Supported Elements

The `position` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Images (`<img>`)
- Text spans (`<span>`)
- All container elements

---

## Notes

- Static positioning is the default and follows normal document flow
- Relative positioning preserves the original space in the flow
- Absolute positioning removes the element from flow and positions relative to the nearest positioned ancestor
- Fixed positioning is ideal for elements that should appear on every page
- Elements with `position: absolute` or `position: fixed` require `top`, `left`, `right`, or `bottom` properties to control placement
- Z-index can be used with positioned elements to control stacking order
- Fixed elements are perfect for page headers, footers, watermarks, and page numbers
- Positioned elements can overlap other content

---

## Data Binding

The `position` property supports data binding, allowing you to dynamically control positioning behavior based on your data model. This is particularly useful for conditional layouts, configurable document templates, and data-driven design requirements.

### Example 1: Dynamic watermark positioning based on document type

```html
<style>
    .watermark {
        position: {{document.watermark.positioning}};
        top: {{document.watermark.offsetTop}}pt;
        left: {{document.watermark.offsetLeft}}pt;
        font-size: {{document.watermark.fontSize}}pt;
        color: rgba(0, 0, 0, {{document.watermark.opacity}});
        font-weight: bold;
        transform: rotate(-45deg);
    }
</style>
<body>
    <div class="watermark">{{document.watermark.text}}</div>
    <div>
        <h1>{{document.title}}</h1>
        <p>{{document.content}}</p>
    </div>
</body>
```

Data model:
```json
{
  "document": {
    "title": "Confidential Report",
    "content": "This document contains sensitive information...",
    "watermark": {
      "text": "CONFIDENTIAL",
      "positioning": "fixed",
      "offsetTop": 300,
      "offsetLeft": 200,
      "fontSize": 72,
      "opacity": 0.15
    }
  }
}
```

### Example 2: Conditional header positioning based on layout preferences

```html
<style>
    .page-header {
        position: {{layout.headerPosition}};
        top: {{layout.headerOffset}}pt;
        left: 0;
        right: 0;
        height: {{layout.headerHeight}}pt;
        background-color: {{branding.primaryColor}};
        color: white;
        padding: 15pt 20pt;
    }
    .content {
        margin-top: {{layout.contentMarginTop}}pt;
    }
</style>
<body>
    <div class="page-header">
        <h1>{{company.name}} Report</h1>
    </div>
    <div class="content">
        <p>Document content starts here...</p>
    </div>
</body>
```

Data model:
```json
{
  "layout": {
    "headerPosition": "fixed",
    "headerOffset": 0,
    "headerHeight": 60,
    "contentMarginTop": 80
  },
  "company": {
    "name": "Acme Corporation"
  },
  "branding": {
    "primaryColor": "#1e3a8a"
  }
}
```

### Example 3: Dynamic approval stamp positioning

```html
<style>
    .document-container {
        position: relative;
        padding: 20pt;
        min-height: 400pt;
    }
    .status-stamp {
        position: {{stamp.position}};
        top: {{stamp.top}}pt;
        right: {{stamp.right}}pt;
        width: 120pt;
        height: 120pt;
        border: 4pt solid {{stamp.color}};
        border-radius: 10pt;
        color: {{stamp.color}};
        font-size: 24pt;
        font-weight: bold;
        text-align: center;
        padding: 35pt 10pt;
        transform: rotate({{stamp.rotation}}deg);
        opacity: {{stamp.opacity}};
    }
</style>
<body>
    <div class="document-container">
        <h1>{{document.title}}</h1>
        <p>{{document.content}}</p>
        <div class="status-stamp">{{stamp.text}}</div>
    </div>
</body>
```

Data model:
```json
{
  "document": {
    "title": "Contract Agreement",
    "content": "This agreement is made between..."
  },
  "stamp": {
    "text": "APPROVED",
    "position": "absolute",
    "top": 100,
    "right": 50,
    "color": "#16a34a",
    "rotation": -15,
    "opacity": 0.7
  }
}
```

---

## Examples

### Example 1: Fixed header on every page

```html
<style>
    .page-header {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        height: 60pt;
        background-color: #1e3a8a;
        color: white;
        padding: 15pt 20pt;
    }
    .content {
        margin-top: 80pt;
    }
</style>
<body>
    <div class="page-header">
        <h1>Company Report 2025</h1>
    </div>
    <div class="content">
        <p>Document content starts here...</p>
    </div>
</body>
```

### Example 2: Fixed footer with page numbers

```html
<style>
    .page-footer {
        position: fixed;
        bottom: 0;
        left: 0;
        right: 0;
        height: 40pt;
        background-color: #f3f4f6;
        padding: 12pt 20pt;
        text-align: center;
        border-top: 1pt solid #d1d5db;
    }
    .content {
        margin-bottom: 60pt;
    }
</style>
<body>
    <div class="content">
        <p>Your document content here...</p>
    </div>
    <div class="page-footer">
        <p>Page <span data-page-number="true"></span> of <span data-page-count="true"></span></p>
    </div>
</body>
```

### Example 3: Watermark using fixed positioning

```html
<style>
    .watermark {
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%) rotate(-45deg);
        font-size: 72pt;
        color: rgba(0, 0, 0, 0.1);
        font-weight: bold;
        z-index: -1;
    }
</style>
<body>
    <div class="watermark">CONFIDENTIAL</div>
    <div>
        <h1>Confidential Document</h1>
        <p>This document contains sensitive information...</p>
    </div>
</body>
```

### Example 4: Absolute positioning for stamps

```html
<style>
    .document-container {
        position: relative;
        padding: 20pt;
        min-height: 400pt;
    }
    .approved-stamp {
        position: absolute;
        top: 100pt;
        right: 50pt;
        width: 120pt;
        height: 120pt;
        border: 4pt solid #16a34a;
        border-radius: 10pt;
        color: #16a34a;
        font-size: 24pt;
        font-weight: bold;
        text-align: center;
        padding: 35pt 10pt;
        transform: rotate(-15deg);
        opacity: 0.7;
    }
</style>
<body>
    <div class="document-container">
        <h1>Contract Agreement</h1>
        <p>This agreement is made between...</p>
        <div class="approved-stamp">APPROVED</div>
    </div>
</body>
```

### Example 5: Relative positioning for adjustments

```html
<style>
    .signature-line {
        border-bottom: 1pt solid black;
        width: 200pt;
        display: inline-block;
    }
    .signature-date {
        position: relative;
        top: -5pt;
        margin-left: 20pt;
        font-size: 9pt;
        color: #6b7280;
    }
</style>
<body>
    <div>
        <span class="signature-line"></span>
        <span class="signature-date">Date: _____________</span>
    </div>
</body>
```

### Example 6: Fixed sidebar navigation

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
            <li>Section 1</li>
            <li>Section 2</li>
            <li>Section 3</li>
        </ul>
    </div>
    <div class="main-content">
        <h1>Main Content</h1>
        <p>Document content with fixed sidebar...</p>
    </div>
</body>
```

### Example 7: Absolute positioning for annotations

```html
<style>
    .page-container {
        position: relative;
        padding: 30pt;
        min-height: 600pt;
    }
    .annotation {
        position: absolute;
        background-color: #fef3c7;
        border: 1pt solid #f59e0b;
        padding: 8pt;
        font-size: 9pt;
        width: 150pt;
    }
    .annotation-1 {
        top: 120pt;
        right: 20pt;
    }
    .annotation-2 {
        top: 300pt;
        left: 20pt;
    }
</style>
<body>
    <div class="page-container">
        <h1>Technical Document</h1>
        <p>Main content of the document goes here...</p>
        <div class="annotation annotation-1">
            Note: This section requires review.
        </div>
        <div class="annotation annotation-2">
            Reference: See Appendix A
        </div>
    </div>
</body>
```

### Example 8: Fixed company logo on every page

```html
<style>
    .company-logo {
        position: fixed;
        top: 20pt;
        right: 20pt;
        width: 100pt;
        height: 40pt;
        background-color: #1e3a8a;
        color: white;
        text-align: center;
        padding: 10pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="company-logo">
        COMPANY
    </div>
    <div style="margin-top: 80pt;">
        <h1>Document Title</h1>
        <p>Your content here...</p>
    </div>
</body>
```

### Example 9: Absolute positioning for corner ribbons

```html
<style>
    .certificate {
        position: relative;
        width: 500pt;
        height: 350pt;
        border: 5pt solid #d4af37;
        padding: 40pt;
        background-color: #fffef8;
    }
    .ribbon {
        position: absolute;
        top: 20pt;
        right: -30pt;
        background-color: #dc2626;
        color: white;
        padding: 8pt 40pt;
        transform: rotate(45deg);
        font-weight: bold;
        box-shadow: 0 2pt 4pt rgba(0,0,0,0.2);
    }
</style>
<body>
    <div class="certificate">
        <h1>Certificate of Achievement</h1>
        <p>This certifies that...</p>
        <div class="ribbon">CERTIFIED</div>
    </div>
</body>
```

### Example 10: Fixed page border

```html
<style>
    .page-border {
        position: fixed;
        top: 10pt;
        left: 10pt;
        right: 10pt;
        bottom: 10pt;
        border: 2pt solid #1e3a8a;
        border-radius: 5pt;
        z-index: -1;
    }
    .content {
        padding: 40pt;
    }
</style>
<body>
    <div class="page-border"></div>
    <div class="content">
        <h1>Bordered Document</h1>
        <p>Content with a fixed border frame on every page...</p>
    </div>
</body>
```

### Example 11: Relative positioning for superscripts

```html
<style>
    .footnote-ref {
        position: relative;
        top: -6pt;
        font-size: 8pt;
        color: #2563eb;
    }
</style>
<body>
    <p>
        This statement requires citation<span class="footnote-ref">[1]</span>
        as does this one<span class="footnote-ref">[2]</span>.
    </p>
</body>
```

### Example 12: Absolute positioning for document badges

```html
<style>
    .report {
        position: relative;
        padding: 30pt;
        min-height: 500pt;
    }
    .badge {
        position: absolute;
        width: 80pt;
        height: 80pt;
        border-radius: 50%;
        text-align: center;
        padding-top: 25pt;
        font-weight: bold;
        font-size: 11pt;
    }
    .priority-badge {
        top: 20pt;
        left: 20pt;
        background-color: #fee2e2;
        color: #991b1b;
        border: 2pt solid #dc2626;
    }
</style>
<body>
    <div class="report">
        <div class="badge priority-badge">HIGH<br/>PRIORITY</div>
        <h1>Project Status Report</h1>
        <p>This report outlines the current status...</p>
    </div>
</body>
```

### Example 13: Fixed confidentiality footer

```html
<style>
    .confidential-footer {
        position: fixed;
        bottom: 5pt;
        left: 0;
        right: 0;
        text-align: center;
        font-size: 8pt;
        color: #991b1b;
        background-color: rgba(254, 226, 226, 0.5);
        padding: 5pt;
    }
</style>
<body>
    <div>
        <h1>Internal Memo</h1>
        <p>Confidential company information...</p>
    </div>
    <div class="confidential-footer">
        CONFIDENTIAL - For Internal Use Only
    </div>
</body>
```

### Example 14: Absolute positioning for layered cards

```html
<style>
    .card-stack {
        position: relative;
        width: 300pt;
        height: 200pt;
        margin: 50pt;
    }
    .card {
        position: absolute;
        width: 280pt;
        height: 180pt;
        background-color: white;
        border: 1pt solid #d1d5db;
        padding: 15pt;
        box-shadow: 2pt 2pt 5pt rgba(0,0,0,0.1);
    }
    .card-1 {
        top: 0;
        left: 0;
        z-index: 3;
    }
    .card-2 {
        top: 10pt;
        left: 10pt;
        z-index: 2;
        opacity: 0.7;
    }
    .card-3 {
        top: 20pt;
        left: 20pt;
        z-index: 1;
        opacity: 0.5;
    }
</style>
<body>
    <div class="card-stack">
        <div class="card card-3"></div>
        <div class="card card-2"></div>
        <div class="card card-1">
            <h3>Top Card</h3>
            <p>This is the front card in the stack.</p>
        </div>
    </div>
</body>
```

### Example 15: Fixed draft watermark diagonal

```html
<style>
    .draft-watermark {
        position: fixed;
        top: 50%;
        left: 0;
        right: 0;
        text-align: center;
        font-size: 96pt;
        font-weight: bold;
        color: rgba(220, 38, 38, 0.15);
        transform: translateY(-50%) rotate(-45deg);
        z-index: 1000;
        pointer-events: none;
    }
    .content {
        position: relative;
        z-index: 1;
    }
</style>
<body>
    <div class="draft-watermark">DRAFT</div>
    <div class="content">
        <h1>Document Title</h1>
        <p>This is a draft document...</p>
    </div>
</body>
```

---

## See Also

- [top](/reference/cssproperties/css_prop_top) - Set top offset for positioned elements
- [left](/reference/cssproperties/css_prop_left) - Set left offset for positioned elements
- [right](/reference/cssproperties/css_prop_right) - Set right offset for positioned elements
- [bottom](/reference/cssproperties/css_prop_bottom) - Set bottom offset for positioned elements
- [float](/reference/cssproperties/css_prop_float) - Float elements left or right
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
