---
layout: default
title: background
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# background : Background Shorthand Property

The `background` property is a shorthand for setting multiple background-related properties in a single declaration. This property is essential for efficiently styling backgrounds in PDF documents, combining image, color, position, size, and repeat settings.

## Usage

```css
selector {
    background: [color] [image] [repeat] [position] / [size];
}
```

The background property can set `background-color`, `background-image`, `background-repeat`, `background-position`, and `background-size` in a single declaration. Values can be specified in any order, though size must come after position separated by a slash.

---

## Supported Values

### Color Values
- Named colors: `red`, `blue`, `white`, etc.
- Hexadecimal: `#RRGGBB` or `#RGB`
- RGB/RGBA: `rgb(r, g, b)` or `rgba(r, g, b, a)`
- `transparent` keyword

### Image Values
- `url('path/to/image.jpg')` - URL to an image file
- `none` - No background image (default)

### Repeat Values
- `repeat` - Repeat in both directions (default)
- `repeat-x` - Repeat horizontally only
- `repeat-y` - Repeat vertically only
- `no-repeat` - Display image once
- `space` - Repeat with spacing to fill area
- `round` - Repeat and scale to fit

### Position Values
- Keywords: `top`, `bottom`, `left`, `right`, `center`
- Percentages: `0%` to `100%`
- Length units: `10pt`, `20px`, etc.
- Combinations: `center top`, `right 10pt bottom 20pt`

### Size Values
- `auto` - Original image size (default)
- `cover` - Scale to cover entire area
- `contain` - Scale to fit within area
- Length values: `100pt 50pt`, `50% 25%`

---

## Supported Elements

The `background` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Table rows (`<tr>`)
- Page headers and footers
- All container elements

---

## Notes

- When using both position and size, separate them with a slash: `center / cover`
- Values can be specified in any order except size (which must follow position)
- Omitted values reset to their initial defaults
- Background images are rendered on top of background colors
- The shorthand is more efficient than setting individual properties
- For complex backgrounds, consider using individual properties for clarity
- Background images in PDFs are embedded into the document
- Supported image formats typically include JPG, PNG, and GIF

---

## Data Binding

Background properties support dynamic data binding, allowing you to create personalized and data-driven PDF documents with dynamic images, colors, and positioning.

### Example 1: Dynamic company logo from data

```html
<style>
    .company-header {
        background: url('{{company.logoUrl}}') no-repeat center / contain;
        min-height: 100pt;
        padding: 20pt;
    }
</style>
<body>
    <div class="company-header">
        <h1>{{company.name}} - Annual Report</h1>
    </div>
</body>
```

This example dynamically loads company logos based on data, perfect for multi-tenant systems or white-label documents where each client needs their own branding.

### Example 2: Conditional background colors by status

```html
<style>
    .status-section {
        background: {{#if status.isApproved}}#d1fae5{{else if status.isPending}}#fef3c7{{else}}#fee2e2{{/if}};
        padding: 20pt;
        border-left: 4pt solid {{#if status.isApproved}}#10b981{{else if status.isPending}}#f59e0b{{else}}#ef4444{{/if}};
    }
</style>
<body>
    <div class="status-section">
        <h2>Application Status: {{status.label}}</h2>
        <p>Your application is currently {{status.description}}.</p>
    </div>
</body>
```

Perfect for creating status-driven documents like approval letters, invoice status reports, or compliance certificates where visual indicators change based on state.

### Example 3: Data-driven watermarks and positioning

```html
<style>
    .document-page {
        background: url('{{document.watermarkUrl}}') no-repeat {{document.watermarkPosition}} / {{document.watermarkSize}};
        min-height: 792pt;
        padding: 40pt;
    }
    .brand-color {
        background: {{tenant.brandColor}};
        color: white;
        padding: 15pt;
    }
</style>
<body>
    <div class="document-page">
        <div class="brand-color">
            <h1>{{document.title}}</h1>
        </div>
        <p>Document content here...</p>
    </div>
</body>
```

Enables dynamic document branding where watermark images, positions, sizes, and brand colors are all controlled by data. Ideal for template systems serving multiple organizations or document types.

---

## Examples

### Example 1: Background color only

```html
<style>
    .colored-box {
        background: #e0f2fe;
        padding: 15pt;
    }
</style>
<body>
    <div class="colored-box">
        <h2>Simple Background Color</h2>
        <p>This element has a light blue background.</p>
    </div>
</body>
```

### Example 2: Background image with no-repeat

```html
<style>
    .logo-header {
        background: url('images/company-logo.png') no-repeat;
        padding: 80pt 20pt 20pt 20pt;
    }
</style>
<body>
    <div class="logo-header">
        <h1>Company Report 2025</h1>
    </div>
</body>
```

### Example 3: Centered background image

```html
<style>
    .certificate {
        background: url('images/seal.png') no-repeat center center;
        min-height: 500pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="certificate">
        <h1 style="text-align: center;">Certificate of Achievement</h1>
        <p style="text-align: center;">Awarded to John Smith</p>
    </div>
</body>
```

### Example 4: Background image with size (cover)

```html
<style>
    .branded-page {
        background: url('images/brand-pattern.jpg') no-repeat center / cover;
        min-height: 792pt;
        color: white;
        padding: 40pt;
    }
</style>
<body>
    <div class="branded-page">
        <h1>Annual Report</h1>
        <p>Executive Summary</p>
    </div>
</body>
```

### Example 5: Background image with color fallback

```html
<style>
    .section-header {
        background: #1e3a8a url('images/texture.png') repeat;
        color: white;
        padding: 20pt;
    }
</style>
<body>
    <div class="section-header">
        <h2>Financial Overview</h2>
        <p>Quarterly results and analysis</p>
    </div>
</body>
```

### Example 6: Watermark background

```html
<style>
    .watermark-page {
        background: url('images/watermark.png') no-repeat center / 300pt;
        min-height: 700pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="watermark-page">
        <h1>Confidential Document</h1>
        <p>This document contains sensitive information.</p>
    </div>
</body>
```

### Example 7: Letterhead with top logo

```html
<style>
    .letterhead {
        background: white url('images/letterhead-logo.png') no-repeat top center / 200pt 60pt;
        padding-top: 80pt;
        padding-left: 40pt;
        padding-right: 40pt;
    }
</style>
<body>
    <div class="letterhead">
        <p>Date: January 15, 2025</p>
        <p>Dear Valued Customer,</p>
        <p>We are pleased to announce...</p>
    </div>
</body>
```

### Example 8: Background pattern with repeat

```html
<style>
    .patterned-section {
        background: #f9fafb url('images/dot-pattern.png') repeat;
        padding: 25pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="patterned-section">
        <h3>Important Notice</h3>
        <p>Please review the following information carefully.</p>
    </div>
</body>
```

### Example 9: Invoice with corner logo

```html
<style>
    .invoice {
        background: white url('images/company-logo-small.png') no-repeat right 20pt top 20pt;
        padding: 30pt;
    }
    .invoice-title {
        font-size: 24pt;
        font-weight: bold;
        color: #1e40af;
    }
</style>
<body>
    <div class="invoice">
        <h1 class="invoice-title">INVOICE</h1>
        <p>Invoice #: INV-2025-001</p>
        <p>Date: January 15, 2025</p>
    </div>
</body>
```

### Example 10: Certificate with border image

```html
<style>
    .certificate-border {
        background: white url('images/certificate-border.png') no-repeat center / contain;
        min-height: 600pt;
        padding: 60pt;
    }
    .cert-text {
        text-align: center;
        font-size: 18pt;
    }
</style>
<body>
    <div class="certificate-border">
        <h1 class="cert-text">Certificate of Completion</h1>
        <p class="cert-text">This certifies that</p>
        <p class="cert-text" style="font-size: 24pt; font-weight: bold;">Sarah Johnson</p>
        <p class="cert-text">has successfully completed the training program</p>
    </div>
</body>
```

### Example 11: Report cover page

```html
<style>
    .cover-page {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%) url('images/grid-overlay.png') repeat;
        min-height: 792pt;
        color: white;
        padding: 100pt 60pt;
    }
    .cover-title {
        font-size: 36pt;
        font-weight: bold;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="cover-page">
        <h1 class="cover-title">Market Analysis Report</h1>
        <p style="font-size: 18pt;">Q4 2024 Results</p>
    </div>
</body>
```

### Example 12: Sidebar with background texture

```html
<style>
    .document-layout {
        display: table;
        width: 100%;
    }
    .sidebar {
        display: table-cell;
        width: 150pt;
        background: #f3f4f6 url('images/paper-texture.png') repeat;
        padding: 15pt;
    }
    .main-content {
        display: table-cell;
        background: white;
        padding: 20pt;
    }
</style>
<body>
    <div class="document-layout">
        <div class="sidebar">
            <h3>Table of Contents</h3>
            <p>1. Introduction</p>
            <p>2. Methodology</p>
            <p>3. Results</p>
        </div>
        <div class="main-content">
            <h1>Research Findings</h1>
            <p>Main document content...</p>
        </div>
    </div>
</body>
```

### Example 13: Branded footer

```html
<style>
    .document-footer {
        background: #1f2937 url('images/footer-logo.png') no-repeat right 20pt center / 80pt 30pt;
        color: white;
        padding: 15pt 120pt 15pt 20pt;
        font-size: 9pt;
    }
</style>
<body>
    <div class="document-footer">
        <p>Company Name | 123 Business Street | contact@company.com</p>
        <p>© 2025 All Rights Reserved</p>
    </div>
</body>
```

### Example 14: Draft watermark diagonal

```html
<style>
    .draft-document {
        background: white url('images/draft-diagonal.png') no-repeat center / 400pt 100pt;
        min-height: 792pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="draft-document">
        <h1>Project Proposal</h1>
        <p>This is a draft version of the proposal...</p>
    </div>
</body>
```

### Example 15: Multi-element background composition

```html
<style>
    .complex-layout {
        background: #ffffff;
        padding: 30pt;
    }
    .header-section {
        background: url('images/header-bg.jpg') no-repeat top center / cover;
        min-height: 150pt;
        padding: 30pt;
        color: white;
    }
    .content-section {
        background: white url('images/subtle-pattern.png') repeat;
        padding: 25pt;
        margin-top: 15pt;
    }
    .footer-section {
        background: #1e3a8a url('images/footer-pattern.png') repeat-x bottom;
        color: white;
        padding: 20pt;
        margin-top: 15pt;
    }
</style>
<body>
    <div class="complex-layout">
        <div class="header-section">
            <h1>Professional Document</h1>
        </div>
        <div class="content-section">
            <h2>Main Content</h2>
            <p>Document body text...</p>
        </div>
        <div class="footer-section">
            <p>Footer information</p>
        </div>
    </div>
</body>
```

---

## See Also

- [background-color](/reference/cssproperties/css_prop_background-color) - Set background color
- [background-image](/reference/cssproperties/css_prop_background-image) - Set background image
- [background-repeat](/reference/cssproperties/css_prop_background-repeat) - Control image repetition
- [background-position](/reference/cssproperties/css_prop_background-position) - Set image position
- [background-size](/reference/cssproperties/css_prop_background-size) - Control image sizing
- [opacity](/reference/cssproperties/css_prop_opacity) - Control overall element transparency

---
