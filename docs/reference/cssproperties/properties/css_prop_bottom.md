---
layout: default
title: bottom
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# bottom : Bottom Position Property

The `bottom` property specifies the vertical offset from the bottom edge for positioned elements in PDF documents. It works in conjunction with the `position` property to precisely control element placement from the bottom. This property is essential for creating footers, fixed bottom content, and precisely positioned elements anchored to the bottom.

## Usage

```css
selector {
    bottom: value;
}
```

The bottom property only affects elements with a `position` value of `relative`, `absolute`, or `fixed`. It has no effect on statically positioned elements.

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
- `0` - No offset from bottom edge
- `auto` - Browser calculates the position (default)
- Negative values: `-10pt`, `-20px` (moves element downward beyond edge)

---

## Supported Elements

The `bottom` property can be applied to:
- Block elements with position other than static (`<div>`, `<section>`, `<article>`)
- Positioned paragraphs (`<p>`)
- Positioned headings (`<h1>` through `<h6>`)
- Positioned images (`<img>`)
- Positioned spans (`<span>`)
- All positioned container elements

---

## Notes

- The `bottom` property only works with positioned elements (`position: relative`, `absolute`, or `fixed`)
- For `position: relative`, bottom moves the element from its normal position
- For `position: absolute`, bottom positions relative to the nearest positioned ancestor
- For `position: fixed`, bottom positions relative to the page
- Negative values move elements downward beyond their reference edge
- Percentage values are relative to the containing block's height
- When both `top` and `bottom` are specified, `top` takes precedence in most contexts
- Use `bottom` for fixed footers, page numbers, and bottom-anchored content

---

## Data Binding

The `bottom` property supports data binding, allowing dynamic vertical positioning from the bottom edge based on your data model. This enables configurable footers, signatures, and bottom-anchored content where positions can be controlled through data.

### Example 1: Configurable footer positioning

```html
<style>
    .page-footer {
        position: fixed;
        bottom: {{footer.offset}}pt;
        left: 0;
        right: 0;
        height: {{footer.height}}pt;
        background-color: {{footer.backgroundColor}};
        color: {{footer.textColor}};
        padding: {{footer.padding}}pt 20pt;
        text-align: center;
    }
    .content {
        margin-bottom: {{content.bottomMargin}}pt;
    }
</style>
<body>
    <div class="content">
        <h1>{{document.title}}</h1>
        <p>{{document.content}}</p>
    </div>
    <div class="page-footer">
        <p>{{footer.text}}</p>
    </div>
</body>
```

Data model:
```json
{
  "footer": {
    "offset": 0,
    "height": 50,
    "padding": 12,
    "backgroundColor": "#1e3a8a",
    "textColor": "white",
    "text": "© 2025 Company Name. All rights reserved."
  },
  "content": {
    "bottomMargin": 70
  },
  "document": {
    "title": "Document Title",
    "content": "Document content..."
  }
}
```

### Example 2: Dynamic signature block positioning

```html
<style>
    .letter {
        position: relative;
        min-height: 750pt;
        padding: 50pt;
    }
    .signature-block {
        position: absolute;
        bottom: {{signature.bottomOffset}}pt;
        left: {{signature.leftOffset}}pt;
    }
    .signature-line {
        border-bottom: 1pt solid black;
        width: {{signature.lineWidth}}pt;
        margin-bottom: 5pt;
    }
    .signature-name {
        font-weight: bold;
        font-size: {{signature.nameFontSize}}pt;
    }
    .signature-title {
        font-size: {{signature.titleFontSize}}pt;
        color: {{signature.titleColor}};
    }
</style>
<body>
    <div class="letter">
        <h2>{{letter.greeting}}</h2>
        <p>{{letter.content}}</p>
        <div class="signature-block">
            <div class="signature-line"></div>
            <div class="signature-name">{{signature.name}}</div>
            <div class="signature-title">{{signature.title}}</div>
        </div>
    </div>
</body>
```

Data model:
```json
{
  "letter": {
    "greeting": "Dear Client,",
    "content": "Letter content goes here..."
  },
  "signature": {
    "bottomOffset": 50,
    "leftOffset": 50,
    "lineWidth": 200,
    "name": "John Smith",
    "title": "Chief Executive Officer",
    "nameFontSize": 12,
    "titleFontSize": 10,
    "titleColor": "#6b7280"
  }
}
```

### Example 3: Data-driven approval stamp at bottom

```html
<style>
    .contract {
        position: relative;
        padding: 40pt;
        min-height: 600pt;
        border: 1pt solid #d1d5db;
    }
    .approval-stamp {
        position: absolute;
        bottom: {{stamp.bottomPosition}}pt;
        right: {{stamp.rightPosition}}pt;
        width: {{stamp.size}}pt;
        height: {{stamp.size}}pt;
        border: {{stamp.borderWidth}}pt solid {{stamp.color}};
        border-radius: 50%;
        text-align: center;
        padding-top: {{stamp.paddingTop}}pt;
        font-weight: bold;
        font-size: {{stamp.fontSize}}pt;
        color: {{stamp.color}};
        transform: rotate({{stamp.rotation}}deg);
        opacity: {{stamp.opacity}};
    }
</style>
<body>
    <div class="contract">
        <h1>{{contract.title}}</h1>
        <p>{{contract.content}}</p>
        <div class="approval-stamp">{{stamp.text}}</div>
    </div>
</body>
```

Data model:
```json
{
  "contract": {
    "title": "Contract Agreement",
    "content": "Terms and conditions..."
  },
  "stamp": {
    "text": "APPROVED",
    "bottomPosition": 40,
    "rightPosition": 50,
    "size": 120,
    "borderWidth": 4,
    "color": "#16a34a",
    "fontSize": 18,
    "paddingTop": 35,
    "rotation": -15,
    "opacity": 0.7
  }
}
```

---

## Examples

### Example 1: Fixed footer at bottom of page

```html
<style>
    .page-footer {
        position: fixed;
        bottom: 0;
        left: 0;
        right: 0;
        height: 50pt;
        background-color: #1e3a8a;
        color: white;
        padding: 12pt 20pt;
        text-align: center;
    }
    .content {
        margin-bottom: 70pt;
    }
</style>
<body>
    <div class="content">
        <h1>Document Title</h1>
        <p>Document content...</p>
    </div>
    <div class="page-footer">
        <p>© 2025 Company Name. All rights reserved.</p>
    </div>
</body>
```

### Example 2: Absolute positioning from bottom

```html
<style>
    .container {
        position: relative;
        height: 400pt;
        border: 1pt solid #d1d5db;
        padding: 20pt;
    }
    .signature {
        position: absolute;
        bottom: 20pt;
        right: 30pt;
        font-style: italic;
        color: #6b7280;
    }
</style>
<body>
    <div class="container">
        <h1>Letter Content</h1>
        <p>Letter text goes here...</p>
        <div class="signature">
            Sincerely,<br/>
            John Smith
        </div>
    </div>
</body>
```

### Example 3: Relative positioning with bottom offset

```html
<style>
    .text-container {
        padding: 15pt;
        background-color: #f9fafb;
        line-height: 2;
    }
    .subscript {
        position: relative;
        bottom: -3pt;
        font-size: 8pt;
        color: #2563eb;
    }
</style>
<body>
    <div class="text-container">
        H<span class="subscript">2</span>O is water.
        CO<span class="subscript">2</span> is carbon dioxide.
    </div>
</body>
```

### Example 4: Fixed page number at bottom

```html
<style>
    .page-number {
        position: fixed;
        bottom: 15pt;
        left: 0;
        right: 0;
        text-align: center;
        font-size: 10pt;
        color: #6b7280;
    }
    .content {
        margin-bottom: 40pt;
    }
</style>
<body>
    <div class="content">
        <h1>Document Title</h1>
        <p>Main content...</p>
    </div>
    <div class="page-number">
        Page <span data-page-number="true"></span> of <span data-page-count="true"></span>
    </div>
</body>
```

### Example 5: Bottom-aligned watermark

```html
<style>
    .watermark {
        position: fixed;
        bottom: 50pt;
        left: 50%;
        transform: translateX(-50%);
        font-size: 36pt;
        color: rgba(0, 0, 0, 0.08);
        font-weight: bold;
    }
</style>
<body>
    <div class="watermark">CONFIDENTIAL</div>
    <div>
        <h1>Confidential Document</h1>
        <p>Sensitive information...</p>
    </div>
</body>
```

### Example 6: Document footer with info

```html
<style>
    .document-footer {
        position: fixed;
        bottom: 0;
        left: 0;
        right: 0;
        background-color: #f3f4f6;
        border-top: 2pt solid #d1d5db;
        padding: 10pt 20pt;
        font-size: 9pt;
    }
    .footer-left {
        float: left;
        color: #6b7280;
    }
    .footer-right {
        float: right;
        color: #6b7280;
    }
    .content {
        margin-bottom: 60pt;
    }
</style>
<body>
    <div class="content">
        <h1>Annual Report</h1>
        <p>Report content...</p>
    </div>
    <div class="document-footer">
        <div class="footer-left">Document ID: REP-2025-001</div>
        <div class="footer-right">Generated: January 15, 2025</div>
    </div>
</body>
```

### Example 7: Absolute positioned stamp at bottom

```html
<style>
    .contract {
        position: relative;
        padding: 40pt;
        min-height: 600pt;
        border: 1pt solid #d1d5db;
    }
    .approval-stamp {
        position: absolute;
        bottom: 40pt;
        right: 50pt;
        width: 120pt;
        height: 120pt;
        border: 4pt solid #16a34a;
        border-radius: 50%;
        text-align: center;
        padding-top: 35pt;
        font-weight: bold;
        font-size: 18pt;
        color: #16a34a;
        transform: rotate(-15deg);
        opacity: 0.7;
    }
</style>
<body>
    <div class="contract">
        <h1>Contract Agreement</h1>
        <p>Terms and conditions...</p>
        <div class="approval-stamp">APPROVED</div>
    </div>
</body>
```

### Example 8: Bottom status bar

```html
<style>
    .status-bar {
        position: fixed;
        bottom: 0;
        left: 0;
        right: 0;
        height: 30pt;
        background-color: #dcfce7;
        border-top: 2pt solid #16a34a;
        padding: 8pt 15pt;
        font-size: 10pt;
        color: #166534;
    }
    .status-icon {
        font-weight: bold;
        margin-right: 5pt;
    }
    .content {
        margin-bottom: 50pt;
    }
</style>
<body>
    <div class="content">
        <h1>Document Status</h1>
        <p>Content here...</p>
    </div>
    <div class="status-bar">
        <span class="status-icon">✓</span>
        <span>Document approved and published</span>
    </div>
</body>
```

### Example 9: Bottom-aligned footnotes

```html
<style>
    .page-content {
        position: relative;
        min-height: 700pt;
        padding: 30pt;
    }
    .footnotes {
        position: absolute;
        bottom: 20pt;
        left: 30pt;
        right: 30pt;
        border-top: 1pt solid #d1d5db;
        padding-top: 10pt;
        font-size: 9pt;
        color: #6b7280;
    }
    .footnote-item {
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="page-content">
        <h1>Research Paper</h1>
        <p>Content with references<sup>1</sup> and citations<sup>2</sup>.</p>
        <div class="footnotes">
            <div class="footnote-item"><sup>1</sup> Source reference one</div>
            <div class="footnote-item"><sup>2</sup> Source reference two</div>
        </div>
    </div>
</body>
```

### Example 10: Stacked cards from bottom

```html
<style>
    .card-stack {
        position: relative;
        height: 350pt;
        width: 300pt;
        margin: 30pt;
    }
    .card {
        position: absolute;
        width: 280pt;
        height: 180pt;
        border: 1pt solid #d1d5db;
        background-color: white;
        padding: 15pt;
        box-shadow: 2pt 2pt 4pt rgba(0,0,0,0.1);
    }
    .card-1 {
        bottom: 0;
        left: 0;
        z-index: 3;
    }
    .card-2 {
        bottom: 15pt;
        left: 10pt;
        z-index: 2;
        background-color: #f9fafb;
    }
    .card-3 {
        bottom: 30pt;
        left: 20pt;
        z-index: 1;
        background-color: #f3f4f6;
    }
</style>
<body>
    <div class="card-stack">
        <div class="card card-3"></div>
        <div class="card card-2"></div>
        <div class="card card-1">
            <h3>Bottom Card</h3>
            <p>This card is anchored to the bottom</p>
        </div>
    </div>
</body>
```

### Example 11: Negative bottom offset for overlap

```html
<style>
    .content-section {
        background-color: white;
        padding: 30pt;
        margin-bottom: 50pt;
    }
    .call-to-action {
        position: relative;
        bottom: -30pt;
        background-color: #2563eb;
        color: white;
        padding: 20pt;
        margin: 0 40pt;
        text-align: center;
        border-radius: 5pt;
        box-shadow: 0 4pt 6pt rgba(0,0,0,0.1);
    }
</style>
<body>
    <div class="content-section">
        <h1>Special Offer</h1>
        <p>Limited time offer details...</p>
        <div class="call-to-action">
            <h2>Act Now!</h2>
            <p>Get 20% off today</p>
        </div>
    </div>
</body>
```

### Example 12: Percentage-based bottom positioning

```html
<style>
    .container {
        position: relative;
        height: 500pt;
        border: 2pt solid #1e3a8a;
        background-color: #f9fafb;
    }
    .bottom-banner {
        position: absolute;
        bottom: 10%;
        left: 10%;
        right: 10%;
        background-color: white;
        border: 1pt solid #d1d5db;
        padding: 20pt;
        text-align: center;
    }
</style>
<body>
    <div class="container">
        <div class="bottom-banner">
            <h2>Bottom Banner</h2>
            <p>Positioned at 10% from bottom</p>
        </div>
    </div>
</body>
```

### Example 13: Fixed confidentiality notice

```html
<style>
    .confidential-notice {
        position: fixed;
        bottom: 3pt;
        left: 0;
        right: 0;
        background-color: rgba(254, 226, 226, 0.8);
        color: #991b1b;
        text-align: center;
        padding: 4pt;
        font-size: 8pt;
        font-weight: bold;
        border-top: 1pt solid #dc2626;
    }
</style>
<body>
    <div>
        <h1>Internal Memo</h1>
        <p>Confidential company information...</p>
    </div>
    <div class="confidential-notice">
        CONFIDENTIAL - NOT FOR DISTRIBUTION
    </div>
</body>
```

### Example 14: Bottom-aligned signature block

```html
<style>
    .letter {
        position: relative;
        min-height: 750pt;
        padding: 50pt;
    }
    .signature-block {
        position: absolute;
        bottom: 50pt;
        left: 50pt;
    }
    .signature-line {
        border-bottom: 1pt solid black;
        width: 200pt;
        margin-bottom: 5pt;
    }
    .signature-name {
        font-weight: bold;
    }
    .signature-title {
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="letter">
        <h2>Dear Client,</h2>
        <p>Letter content goes here...</p>
        <div class="signature-block">
            <div class="signature-line"></div>
            <div class="signature-name">John Smith</div>
            <div class="signature-title">Chief Executive Officer</div>
        </div>
    </div>
</body>
```

### Example 15: Multiple bottom-positioned elements

```html
<style>
    .certificate {
        position: relative;
        height: 550pt;
        width: 700pt;
        border: 5pt solid #d4af37;
        padding: 40pt;
        background-color: #fffef8;
    }
    .cert-seal {
        position: absolute;
        bottom: 30pt;
        left: 50pt;
        width: 80pt;
        height: 80pt;
        border: 3pt solid #d4af37;
        border-radius: 50%;
        text-align: center;
        padding-top: 25pt;
        font-weight: bold;
        font-size: 10pt;
        color: #d4af37;
    }
    .cert-signature {
        position: absolute;
        bottom: 30pt;
        right: 50pt;
        text-align: right;
    }
    .signature-line {
        border-bottom: 1pt solid black;
        width: 150pt;
        margin-bottom: 5pt;
    }
    .cert-date {
        position: absolute;
        bottom: 30pt;
        left: 50%;
        transform: translateX(-50%);
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="certificate">
        <h1 style="text-align: center;">Certificate of Achievement</h1>
        <p style="text-align: center; margin-top: 50pt;">
            This certifies that John Doe has successfully completed...
        </p>
        <div class="cert-seal">OFFICIAL<br/>SEAL</div>
        <div class="cert-date">Issued: January 15, 2025</div>
        <div class="cert-signature">
            <div class="signature-line"></div>
            <div>Director's Signature</div>
        </div>
    </div>
</body>
```

---

## See Also

- [position](/reference/cssproperties/css_prop_position) - Set positioning method
- [top](/reference/cssproperties/css_prop_top) - Set top offset for positioned elements
- [left](/reference/cssproperties/css_prop_left) - Set left offset for positioned elements
- [right](/reference/cssproperties/css_prop_right) - Set right offset for positioned elements
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
