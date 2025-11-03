---
layout: default
title: margin-inline-start
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# margin-inline-start : Inline Start Margin Property

The `margin-inline-start` property sets the margin at the inline start edge of an element in PDF documents. This is a logical property that maps to either left or right margin depending on the writing direction. In left-to-right (LTR) languages it corresponds to `margin-left`, while in right-to-left (RTL) languages it corresponds to `margin-right`.

## Usage

```css
selector {
    margin-inline-start: value;
}
```

The margin-inline-start property accepts a single length value or percentage that defines the space at the inline start edge.

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
- `0` - No inline start margin
- `auto` - Automatic margin

---

## Supported Elements

The `margin-inline-start` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables (`<table>`)
- Lists (`<ul>`, `<ol>`)
- All container elements

---

## Notes

- This is a logical property that adapts to text direction
- In LTR contexts (English, Spanish, etc.), it behaves like `margin-left`
- In RTL contexts (Arabic, Hebrew, etc.), it behaves like `margin-right`
- Useful for creating internationalized documents that work in multiple languages
- Provides better semantic meaning than physical properties
- Simplifies maintenance of bidirectional layouts
- Does not collapse with adjacent margins (inline direction)
- Percentage values are relative to parent width

---

## Data Binding

The `margin-inline-start` property supports dynamic values through data binding, enabling you to create direction-aware, flexible spacing that adapts to writing direction and internationalization requirements.

### Example 1: Dynamic indentation based on content hierarchy

```html
<style>
    .content {
        padding: 30pt;
    }
    .level-1 {
        margin-inline-start: 0;
    }
    .level-2 {
        margin-inline-start: {{hierarchy.level2Indent}}pt;
        padding: 10pt;
        background-color: #f9fafb;
    }
    .level-3 {
        margin-inline-start: {{hierarchy.level3Indent}}pt;
        padding: 10pt;
        background-color: #f3f4f6;
    }
</style>
<body>
    <div class="content">
        <div class="level-1">
            <h3>Top Level Heading</h3>
        </div>
        <div class="level-2">
            <h4>Second Level Content</h4>
        </div>
        <div class="level-3">
            <h5>Third Level Details</h5>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "hierarchy": {
        "level2Indent": 25,
        "level3Indent": 50
    }
}
```

### Example 2: Localized invoice layout with direction-aware spacing

```html
<style>
    .invoice {
        padding: 40pt;
        direction: {{locale.textDirection}};
    }
    .invoice-section {
        margin-bottom: 25pt;
    }
    .section-heading {
        margin-inline-start: 0;
        font-weight: bold;
    }
    .line-item {
        margin-inline-start: {{layout.itemIndent}}pt;
        margin-bottom: 8pt;
        padding: 8pt;
        background-color: #f9fafb;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-section">
            <div class="section-heading">{{labels.services}}</div>
            <div class="line-item">
                <span>{{items[0].description}}</span>
                <span>{{items[0].amount}}</span>
            </div>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "locale": {
        "textDirection": "ltr"
    },
    "layout": {
        "itemIndent": 20
    },
    "labels": {
        "services": "Professional Services"
    },
    "items": [
        {
            "description": "Consulting",
            "amount": "$1,500.00"
        }
    ]
}
```

### Example 3: Comment thread with dynamic nesting

```html
<style>
    .comments {
        padding: 25pt;
    }
    .comment {
        margin-inline-start: {{comment.nestLevel * 30}}pt;
        margin-bottom: 15pt;
        padding: 12pt;
        background-color: #f9fafb;
        border-inline-start: 3pt solid {{comment.nestLevel === 0 ? '#d1d5db' : '#9ca3af'}};
    }
</style>
<body>
    <div class="comments">
        <div class="comment">
            <strong>{{comment.author}}</strong>
            <p>{{comment.text}}</p>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "comment": {
        "nestLevel": 1,
        "author": "Jane Doe",
        "text": "This is a reply to the main comment."
    }
}
```

---

## Examples

### Example 1: Basic inline start margin

```html
<style>
    .content {
        padding: 30pt;
    }
    .box {
        margin-inline-start: 25pt;
        padding: 15pt;
        background-color: #dbeafe;
    }
</style>
<body>
    <div class="content">
        <div class="box">
            <p>This box has a 25pt margin at the inline start.</p>
        </div>
    </div>
</body>
```

### Example 2: Indented paragraphs with inline start margin

```html
<style>
    .article {
        padding: 40pt;
    }
    .article h2 {
        margin-inline-start: 0;
        margin-bottom: 15pt;
    }
    .article p {
        margin-inline-start: 20pt;
        margin-bottom: 12pt;
        line-height: 1.6;
    }
    .article .first-para {
        margin-inline-start: 0;
    }
</style>
<body>
    <div class="article">
        <h2>Article Heading</h2>
        <p class="first-para">First paragraph without indentation.</p>
        <p>Subsequent paragraphs have inline start margin for indentation.</p>
        <p>This creates a consistent reading pattern.</p>
    </div>
</body>
```

### Example 3: Multi-level list with inline start margins

```html
<style>
    .list-container {
        padding: 30pt;
    }
    .list-level-1 {
        margin-inline-start: 0;
        margin-bottom: 8pt;
    }
    .list-level-2 {
        margin-inline-start: 25pt;
        margin-bottom: 8pt;
    }
    .list-level-3 {
        margin-inline-start: 50pt;
        margin-bottom: 8pt;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="list-container">
        <div class="list-level-1"><strong>1. Main Topic</strong></div>
        <div class="list-level-2">1.1 Subtopic A</div>
        <div class="list-level-3">1.1.1 Detail point</div>
        <div class="list-level-2">1.2 Subtopic B</div>
        <div class="list-level-1"><strong>2. Second Topic</strong></div>
        <div class="list-level-2">2.1 Subtopic C</div>
    </div>
</body>
```

### Example 4: Blockquote with inline start margin

```html
<style>
    .document {
        padding: 40pt;
    }
    .document p {
        margin-bottom: 12pt;
        line-height: 1.6;
    }
    .blockquote {
        margin-inline-start: 40pt;
        margin-inline-end: 40pt;
        margin-bottom: 20pt;
        padding: 15pt;
        background-color: #f5f5f5;
        border-inline-start: 4pt solid #6366f1;
        font-style: italic;
    }
</style>
<body>
    <div class="document">
        <p>Regular text before the quote.</p>
        <div class="blockquote">
            <p>"The best way to predict the future is to create it."</p>
        </div>
        <p>Text continues after the quote.</p>
    </div>
</body>
```

### Example 5: Form with inline start alignment

```html
<style>
    .form {
        padding: 30pt;
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
        margin-inline-start: 0;
        padding: 8pt;
        width: 100%;
        border: 1pt solid #d1d5db;
    }
    .form-help {
        margin-inline-start: 5pt;
        margin-top: 5pt;
        font-size: 9pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="form">
        <div class="form-group">
            <label class="form-label">Username</label>
            <input class="form-input" type="text" />
            <div class="form-help">Choose a unique username</div>
        </div>
        <div class="form-group">
            <label class="form-label">Email</label>
            <input class="form-input" type="email" />
        </div>
    </div>
</body>
```

### Example 6: Invoice with indented line items

```html
<style>
    .invoice {
        padding: 40pt;
    }
    .invoice-header {
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e3a8a;
        color: white;
    }
    .invoice-section {
        margin-bottom: 25pt;
    }
    .section-heading {
        margin-inline-start: 0;
        font-weight: bold;
        font-size: 14pt;
        margin-bottom: 10pt;
    }
    .line-item {
        margin-inline-start: 20pt;
        margin-bottom: 8pt;
        padding: 8pt;
        background-color: #f9fafb;
    }
    .item-description {
        font-weight: bold;
    }
    .item-price {
        margin-inline-start: 10pt;
        color: #16a34a;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1>INVOICE</h1>
        </div>
        <div class="invoice-section">
            <div class="section-heading">Professional Services</div>
            <div class="line-item">
                <span class="item-description">Consulting</span>
                <span class="item-price">$1,500.00</span>
            </div>
            <div class="line-item">
                <span class="item-description">Development</span>
                <span class="item-price">$2,500.00</span>
            </div>
        </div>
    </div>
</body>
```

### Example 7: Sidebar navigation with inline start margin

```html
<style>
    .sidebar {
        padding: 20pt;
        width: 200pt;
        background-color: #f3f4f6;
    }
    .nav-title {
        margin-inline-start: 0;
        margin-bottom: 15pt;
        font-size: 16pt;
        font-weight: bold;
    }
    .nav-item {
        margin-inline-start: 0;
        margin-bottom: 10pt;
        padding: 8pt 12pt;
        background-color: white;
        border-inline-start: 3pt solid transparent;
    }
    .nav-item.active {
        border-inline-start-color: #3b82f6;
        background-color: #dbeafe;
    }
    .nav-subitem {
        margin-inline-start: 20pt;
        margin-bottom: 8pt;
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="sidebar">
        <div class="nav-title">Menu</div>
        <div class="nav-item active">Dashboard</div>
        <div class="nav-item">Products</div>
        <div class="nav-subitem">Electronics</div>
        <div class="nav-subitem">Clothing</div>
        <div class="nav-item">Settings</div>
    </div>
</body>
```

### Example 8: Alert boxes with inline start margin

```html
<style>
    .alerts {
        padding: 25pt;
    }
    .alert {
        margin-inline-start: 15pt;
        margin-bottom: 12pt;
        padding: 12pt 12pt 12pt 40pt;
        border-inline-start: 4pt solid;
        border-radius: 4pt;
    }
    .alert-info {
        background-color: #dbeafe;
        border-inline-start-color: #3b82f6;
    }
    .alert-success {
        background-color: #dcfce7;
        border-inline-start-color: #16a34a;
    }
    .alert-warning {
        background-color: #fef3c7;
        border-inline-start-color: #f59e0b;
    }
</style>
<body>
    <div class="alerts">
        <div class="alert alert-info">
            <strong>Info:</strong> System maintenance scheduled.
        </div>
        <div class="alert alert-success">
            <strong>Success:</strong> Profile updated successfully.
        </div>
        <div class="alert alert-warning">
            <strong>Warning:</strong> Password expires soon.
        </div>
    </div>
</body>
```

### Example 9: Product specifications with inline start margin

```html
<style>
    .product {
        padding: 30pt;
    }
    .product-title {
        margin-inline-start: 0;
        margin-bottom: 20pt;
        font-size: 20pt;
        font-weight: bold;
    }
    .spec-group {
        margin-bottom: 20pt;
    }
    .spec-group-title {
        margin-inline-start: 0;
        margin-bottom: 10pt;
        font-size: 14pt;
        font-weight: bold;
        color: #1f2937;
    }
    .spec-item {
        margin-inline-start: 25pt;
        margin-bottom: 6pt;
        padding: 6pt;
        background-color: #f9fafb;
    }
    .spec-label {
        font-weight: bold;
        color: #6b7280;
    }
</style>
<body>
    <div class="product">
        <h1 class="product-title">Premium Widget</h1>
        <div class="spec-group">
            <h3 class="spec-group-title">Physical Properties</h3>
            <div class="spec-item">
                <span class="spec-label">Weight:</span> 2.5 lbs
            </div>
            <div class="spec-item">
                <span class="spec-label">Dimensions:</span> 10 x 8 x 2 inches
            </div>
        </div>
    </div>
</body>
```

### Example 10: Table of contents with inline start margin

```html
<style>
    .toc {
        padding: 30pt;
    }
    .toc-title {
        margin-inline-start: 0;
        margin-bottom: 25pt;
        font-size: 20pt;
        font-weight: bold;
        text-align: center;
    }
    .toc-chapter {
        margin-inline-start: 0;
        margin-bottom: 15pt;
        font-weight: bold;
        font-size: 14pt;
    }
    .toc-section {
        margin-inline-start: 20pt;
        margin-bottom: 8pt;
        font-size: 12pt;
    }
    .toc-subsection {
        margin-inline-start: 40pt;
        margin-bottom: 5pt;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="toc">
        <h1 class="toc-title">Table of Contents</h1>
        <div class="toc-chapter">Chapter 1: Introduction</div>
        <div class="toc-section">1.1 Background</div>
        <div class="toc-subsection">1.1.1 Historical Context</div>
        <div class="toc-section">1.2 Objectives</div>
        <div class="toc-chapter">Chapter 2: Methodology</div>
        <div class="toc-section">2.1 Research Design</div>
    </div>
</body>
```

### Example 11: Comment thread with inline start margins

```html
<style>
    .comments {
        padding: 25pt;
    }
    .comment {
        margin-inline-start: 0;
        margin-bottom: 15pt;
        padding: 12pt;
        background-color: #f9fafb;
        border-inline-start: 3pt solid #d1d5db;
    }
    .comment-reply {
        margin-inline-start: 30pt;
        margin-top: 10pt;
        padding: 10pt;
        background-color: #f3f4f6;
        border-inline-start: 3pt solid #9ca3af;
    }
    .comment-author {
        margin-bottom: 5pt;
        font-weight: bold;
        color: #1f2937;
    }
    .comment-text {
        margin-bottom: 0;
        color: #4b5563;
    }
</style>
<body>
    <div class="comments">
        <div class="comment">
            <div class="comment-author">John Smith</div>
            <p class="comment-text">Great article! Very informative.</p>
            <div class="comment-reply">
                <div class="comment-author">Jane Doe</div>
                <p class="comment-text">Thanks for the feedback!</p>
            </div>
        </div>
    </div>
</body>
```

### Example 12: Newsletter with article indentation

```html
<style>
    .newsletter {
        padding: 30pt;
    }
    .newsletter-header {
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
    .article {
        margin-bottom: 25pt;
    }
    .article-title {
        margin-inline-start: 0;
        margin-bottom: 10pt;
        font-size: 16pt;
        font-weight: bold;
    }
    .article-summary {
        margin-inline-start: 20pt;
        padding-inline-start: 15pt;
        border-inline-start: 2pt solid #e5e7eb;
        line-height: 1.6;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>Company Newsletter</h1>
        </div>
        <div class="article">
            <h2 class="article-title">New Product Launch</h2>
            <div class="article-summary">
                <p>We're excited to announce the launch of our latest product line.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 13: Receipt with indented totals

```html
<style>
    .receipt {
        margin: 40pt auto;
        width: 300pt;
        padding: 20pt;
        border: 2pt solid #000;
    }
    .receipt-header {
        margin-bottom: 20pt;
        text-align: center;
        border-bottom: 1pt solid #000;
        padding-bottom: 10pt;
    }
    .receipt-item {
        margin-inline-start: 0;
        margin-bottom: 8pt;
        display: flex;
        justify-content: space-between;
    }
    .receipt-subtotal {
        margin-inline-start: 20pt;
        margin-top: 15pt;
        padding-top: 10pt;
        border-top: 1pt solid #000;
    }
    .receipt-total {
        margin-inline-start: 20pt;
        margin-top: 10pt;
        padding-top: 10pt;
        border-top: 2pt solid #000;
        font-weight: bold;
        font-size: 14pt;
    }
</style>
<body>
    <div class="receipt">
        <div class="receipt-header">
            <h2>Store Name</h2>
            <p>Receipt #12345</p>
        </div>
        <div class="receipt-item">
            <span>Item 1</span>
            <span>$19.99</span>
        </div>
        <div class="receipt-item">
            <span>Item 2</span>
            <span>$29.99</span>
        </div>
        <div class="receipt-subtotal">
            <div class="receipt-item">
                <span>Subtotal</span>
                <span>$49.98</span>
            </div>
        </div>
        <div class="receipt-total">
            <div class="receipt-item">
                <span>Total</span>
                <span>$49.98</span>
            </div>
        </div>
    </div>
</body>
```

### Example 14: Report sections with inline start indentation

```html
<style>
    .report {
        padding: 40pt;
    }
    .report-title {
        margin-inline-start: 0;
        margin-bottom: 30pt;
        font-size: 24pt;
        text-align: center;
    }
    .report-section {
        margin-bottom: 30pt;
    }
    .section-title {
        margin-inline-start: 0;
        margin-bottom: 15pt;
        font-size: 18pt;
        font-weight: bold;
        border-bottom: 2pt solid #d1d5db;
        padding-bottom: 5pt;
    }
    .section-content {
        margin-inline-start: 15pt;
        line-height: 1.6;
    }
    .subsection {
        margin-inline-start: 30pt;
        margin-top: 15pt;
        padding: 10pt;
        background-color: #f9fafb;
    }
</style>
<body>
    <div class="report">
        <h1 class="report-title">Annual Report 2025</h1>
        <div class="report-section">
            <h2 class="section-title">Executive Summary</h2>
            <div class="section-content">
                <p>Overview of the year's achievements.</p>
                <div class="subsection">
                    <strong>Key Metrics:</strong> Revenue growth of 25%
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 15: Code documentation with inline start margins

```html
<style>
    .documentation {
        padding: 35pt;
    }
    .doc-title {
        margin-inline-start: 0;
        margin-bottom: 20pt;
        font-size: 22pt;
        font-weight: bold;
    }
    .doc-section {
        margin-bottom: 25pt;
    }
    .section-heading {
        margin-inline-start: 0;
        margin-bottom: 10pt;
        font-size: 16pt;
        font-weight: bold;
    }
    .code-example {
        margin-inline-start: 20pt;
        padding: 15pt;
        background-color: #1f2937;
        color: #f9fafb;
        font-family: monospace;
        font-size: 10pt;
        border-inline-start: 4pt solid #3b82f6;
    }
    .code-description {
        margin-inline-start: 20pt;
        margin-top: 10pt;
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="documentation">
        <h1 class="doc-title">API Documentation</h1>
        <div class="doc-section">
            <h2 class="section-heading">Usage Example</h2>
            <div class="code-example">
                api.getData({ id: 123 })
            </div>
            <div class="code-description">
                Retrieves data for the specified ID
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [margin-inline-end](/reference/cssproperties/css_prop_margin-inline-end) - Set inline end margin
- [margin-inline](/reference/cssproperties/css_prop_margin-inline) - Set both inline margins shorthand
- [margin-left](/reference/cssproperties/css_prop_margin-left) - Set left margin (physical property)
- [margin-right](/reference/cssproperties/css_prop_margin-right) - Set right margin (physical property)
- [padding-inline-start](/reference/cssproperties/css_prop_padding-inline-start) - Set inline start padding
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
