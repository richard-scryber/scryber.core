---
layout: default
title: background-image
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# background-image : Background Image Property

The `background-image` property sets one or more background images for an element in PDF documents. This property is essential for adding logos, watermarks, letterheads, patterns, and decorative elements to enhance document appearance and branding.

## Usage

```css
selector {
    background-image: value;
}
```

The background-image property accepts URL values pointing to image files or the `none` keyword to remove background images.

---

## Supported Values

### URL Notation
- `url('path/to/image.jpg')` - Relative or absolute path to image file
- `url("path/to/image.png")` - Double quotes also supported
- `url(path/to/image.gif)` - Quotes optional for simple paths

### None Keyword
- `none` - No background image (default value)

### Supported Image Formats
- JPEG/JPG - Photographs and complex images
- PNG - Images with transparency
- GIF - Simple graphics and animations (static frame in PDF)
- BMP - Bitmap images
- SVG - Scalable vector graphics (support varies)

---

## Supported Elements

The `background-image` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Table rows (`<tr>`)
- Page headers and footers
- All container elements

---

## Notes

- Images are embedded directly into the PDF file
- Relative paths are resolved from the document location
- Absolute URLs can reference external resources (if supported)
- Background images are rendered behind text and foreground content
- Images do not inherit from parent elements
- By default, images repeat to fill the background area
- Use with `background-repeat: no-repeat` for single instance display
- Combine with `background-position` to control image placement
- Combine with `background-size` to control image scaling
- PNG transparency is fully supported for overlays
- Consider file size impact on final PDF document size
- High-resolution images may increase PDF file size significantly

---

## Data Binding

The `background-image` property supports dynamic data binding, enabling personalized PDF documents with data-driven images for logos, watermarks, photos, and branding elements.

### Example 1: Dynamic user profile photo

```html
<style>
    .employee-badge {
        background-image: url('{{employee.photoUrl}}');
        background-repeat: no-repeat;
        background-position: left center;
        background-size: 80pt 80pt;
        padding-left: 100pt;
        padding-top: 20pt;
        padding-bottom: 20pt;
        min-height: 100pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="employee-badge">
        <h2>{{employee.fullName}}</h2>
        <p>Employee ID: {{employee.id}}</p>
        <p>Department: {{employee.department}}</p>
    </div>
</body>
```

Perfect for creating employee badges, ID cards, or personnel reports where each document needs a different photo loaded from your database or user profile system.

### Example 2: Conditional watermarks based on document status

```html
<style>
    .document-content {
        background-image: url('{{#if document.isDraft}}images/draft-watermark.png{{else if document.isConfidential}}images/confidential-watermark.png{{else}}images/approved-watermark.png{{/if}}');
        background-repeat: no-repeat;
        background-position: center;
        background-size: 350pt 120pt;
        min-height: 700pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="document-content">
        <h1>{{document.title}}</h1>
        <p>Status: {{document.status}}</p>
        <p>{{document.content}}</p>
    </div>
</body>
```

Automatically applies the appropriate watermark based on document state - essential for document management systems where visual status indicators prevent misuse of drafts or confidential materials.

### Example 3: Product catalog with dynamic images

```html
<style>
    .product-card {
        background-image: url('{{product.imageUrl}}');
        background-repeat: no-repeat;
        background-position: top center;
        background-size: cover;
        min-height: 300pt;
        padding: 250pt 20pt 20pt 20pt;
        background-color: #f3f4f6;
    }
    .product-info {
        background-color: white;
        padding: 15pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    {{#each products}}
    <div class="product-card">
        <div class="product-info">
            <h3>{{name}}</h3>
            <p>SKU: {{sku}}</p>
            <p>Price: ${{price}}</p>
        </div>
    </div>
    {{/each}}
</body>
```

Generates product catalogs, price lists, or inventory reports where each item displays its own product image from your product database or CMS.

---

## Examples

### Example 1: Simple background image

```html
<style>
    .image-box {
        background-image: url('images/pattern.png');
        padding: 20pt;
        min-height: 200pt;
    }
</style>
<body>
    <div class="image-box">
        <h2>Content Over Background</h2>
        <p>Text appears over the background image.</p>
    </div>
</body>
```

### Example 2: Company logo watermark

```html
<style>
    .watermarked-page {
        background-image: url('images/company-watermark.png');
        background-repeat: no-repeat;
        background-position: center;
        min-height: 700pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="watermarked-page">
        <h1>Confidential Report</h1>
        <p>This document contains proprietary information.</p>
    </div>
</body>
```

### Example 3: Letterhead logo

```html
<style>
    .letterhead {
        background-image: url('images/letterhead-header.png');
        background-repeat: no-repeat;
        background-position: top center;
        padding-top: 100pt;
        padding-left: 40pt;
        padding-right: 40pt;
    }
</style>
<body>
    <div class="letterhead">
        <p>Date: January 15, 2025</p>
        <p>Dear Customer,</p>
        <p>We are writing to inform you...</p>
    </div>
</body>
```

### Example 4: Certificate seal

```html
<style>
    .certificate {
        background-image: url('images/official-seal.png');
        background-repeat: no-repeat;
        background-position: bottom right;
        min-height: 500pt;
        padding: 40pt;
    }
    .cert-title {
        text-align: center;
        font-size: 28pt;
        font-weight: bold;
        color: #1e40af;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">Certificate of Excellence</h1>
        <p style="text-align: center; margin-top: 30pt;">Awarded to</p>
        <p style="text-align: center; font-size: 22pt; font-weight: bold;">Jennifer Williams</p>
    </div>
</body>
```

### Example 5: Background texture

```html
<style>
    .textured-section {
        background-image: url('images/paper-texture.jpg');
        background-repeat: repeat;
        padding: 25pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="textured-section">
        <h3>Important Notice</h3>
        <p>This section has a subtle paper texture background.</p>
    </div>
</body>
```

### Example 6: Invoice with corner logo

```html
<style>
    .invoice-header {
        background-image: url('images/company-logo.png');
        background-repeat: no-repeat;
        background-position: right 20pt top 20pt;
        padding: 30pt;
        min-height: 120pt;
    }
    .invoice-title {
        font-size: 32pt;
        font-weight: bold;
        color: #1e3a8a;
    }
</style>
<body>
    <div class="invoice-header">
        <h1 class="invoice-title">INVOICE</h1>
        <p>Invoice Number: INV-2025-001</p>
        <p>Date: January 15, 2025</p>
    </div>
</body>
```

### Example 7: Repeating pattern border

```html
<style>
    .decorative-box {
        background-image: url('images/border-pattern.png');
        background-repeat: repeat-x;
        background-position: bottom;
        padding: 20pt;
        padding-bottom: 40pt;
    }
</style>
<body>
    <div class="decorative-box">
        <h2>Elegant Section</h2>
        <p>Content with decorative border at the bottom.</p>
    </div>
</body>
```

### Example 8: Draft watermark diagonal

```html
<style>
    .draft-page {
        background-image: url('images/draft-watermark.png');
        background-repeat: no-repeat;
        background-position: center;
        background-size: 400pt 150pt;
        min-height: 792pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="draft-page">
        <h1>Project Proposal - DRAFT</h1>
        <p>This is a preliminary version of the proposal.</p>
    </div>
</body>
```

### Example 9: Report cover with background image

```html
<style>
    .report-cover {
        background-image: url('images/abstract-background.jpg');
        background-repeat: no-repeat;
        background-position: center;
        background-size: cover;
        min-height: 792pt;
        padding: 100pt 60pt;
        color: white;
    }
    .report-title {
        font-size: 42pt;
        font-weight: bold;
        text-shadow: 2pt 2pt 4pt rgba(0,0,0,0.5);
    }
</style>
<body>
    <div class="report-cover">
        <h1 class="report-title">Annual Report 2024</h1>
        <p style="font-size: 20pt;">Financial Performance and Strategic Overview</p>
    </div>
</body>
```

### Example 10: Multiple background elements

```html
<style>
    .complex-header {
        background-image: url('images/header-logo.png');
        background-repeat: no-repeat;
        background-position: left center;
        padding-left: 120pt;
        padding-top: 20pt;
        padding-bottom: 20pt;
        border-bottom: 2pt solid #1e40af;
    }
</style>
<body>
    <div class="complex-header">
        <h1>Business Quarterly Review</h1>
        <p>Q4 2024 Results and Analysis</p>
    </div>
</body>
```

### Example 11: Transparent overlay

```html
<style>
    .overlay-section {
        background-image: url('images/transparent-overlay.png');
        background-repeat: no-repeat;
        background-position: top right;
        padding: 30pt;
    }
</style>
<body>
    <div class="overlay-section">
        <h2>Section with Transparent Overlay</h2>
        <p>PNG transparency allows elegant overlays.</p>
    </div>
</body>
```

### Example 12: Footer with branding

```html
<style>
    .branded-footer {
        background-image: url('images/footer-logo.png');
        background-repeat: no-repeat;
        background-position: right center;
        padding: 15pt;
        padding-right: 100pt;
        background-color: #f3f4f6;
        border-top: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="branded-footer">
        <p style="font-size: 9pt;">© 2025 Company Name. All rights reserved.</p>
        <p style="font-size: 9pt;">123 Business Avenue, Suite 100 | phone: (555) 123-4567</p>
    </div>
</body>
```

### Example 13: Security background pattern

```html
<style>
    .secure-document {
        background-image: url('images/security-pattern.png');
        background-repeat: repeat;
        padding: 30pt;
    }
    .security-notice {
        background-color: rgba(255, 255, 255, 0.95);
        padding: 15pt;
        border: 2pt solid #dc2626;
    }
</style>
<body>
    <div class="secure-document">
        <div class="security-notice">
            <h2>Confidential Information</h2>
            <p>This document contains sensitive data.</p>
        </div>
    </div>
</body>
```

### Example 14: Badge or emblem positioning

```html
<style>
    .award-certificate {
        background-image: url('images/gold-medal.png');
        background-repeat: no-repeat;
        background-position: top center;
        padding-top: 120pt;
        padding-left: 40pt;
        padding-right: 40pt;
        text-align: center;
    }
</style>
<body>
    <div class="award-certificate">
        <h1 style="font-size: 28pt; color: #b45309;">Certificate of Achievement</h1>
        <p style="font-size: 18pt; margin-top: 20pt;">Presented to</p>
        <p style="font-size: 24pt; font-weight: bold; color: #1e40af;">Michael Chen</p>
        <p style="margin-top: 20pt;">For outstanding performance in 2024</p>
    </div>
</body>
```

### Example 15: Removing background image

```html
<style>
    .default-background {
        background-image: url('images/pattern.png');
        padding: 20pt;
    }
    .no-background {
        background-image: none;
        background-color: white;
        padding: 20pt;
        margin-top: 10pt;
    }
</style>
<body>
    <div class="default-background">
        <p>This section has a background image.</p>
    </div>
    <div class="no-background">
        <p>This section explicitly removes the background image.</p>
    </div>
</body>
```

---

## See Also

- [background](/reference/cssproperties/css_prop_background) - Shorthand for all background properties
- [background-color](/reference/cssproperties/css_prop_background-color) - Set background color
- [background-repeat](/reference/cssproperties/css_prop_background-repeat) - Control image repetition
- [background-position](/reference/cssproperties/css_prop_background-position) - Set image position
- [background-size](/reference/cssproperties/css_prop_background-size) - Control image sizing
- [opacity](/reference/cssproperties/css_prop_opacity) - Control overall element transparency

---
