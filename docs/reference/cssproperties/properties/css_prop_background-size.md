---
layout: default
title: background-size
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# background-size : Background Size Property

The `background-size` property specifies the size of background images in PDF documents. This property is essential for controlling image scaling, ensuring proper fit for logos, watermarks, and full-page backgrounds.

## Usage

```css
selector {
    background-size: value;
}
```

The background-size property controls how background images are scaled within their container, from maintaining original dimensions to stretching across entire elements.

---

## Supported Values

### Keywords
- `auto` - Original image dimensions (default)
- `cover` - Scale image to cover entire area, maintaining aspect ratio (may crop)
- `contain` - Scale image to fit within area, maintaining aspect ratio (may show gaps)

### Length Values
- Single value: `100pt` - Sets width, height scales proportionally
- Two values: `200pt 150pt` - Sets explicit width and height
- Units: `pt` (points), `px` (pixels), `in` (inches), `cm` (centimeters), `mm` (millimeters)

### Percentage Values
- Single value: `50%` - Width as percentage of container, height scales proportionally
- Two values: `100% 50%` - Width and height as percentages of container
- Percentages relative to background positioning area

### Mixed Values
- Combine units: `auto 100pt` - Auto width, explicit height
- Combine percentage and length: `50% 80pt`

---

## Supported Elements

The `background-size` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Table cells (`<td>`, `<th>`)
- Table rows (`<tr>`)
- Page headers and footers
- All container elements with background images

---

## Notes

- Only affects elements with `background-image` set
- Default value is `auto` (original image size)
- `cover` ensures full coverage but may crop parts of the image
- `contain` ensures entire image is visible but may leave gaps
- Length values allow precise control over image dimensions
- Percentage values scale relative to the element's size
- Aspect ratio is maintained when using single values or keywords
- Two explicit values may distort the image if aspect ratio differs
- Large images scaled down improve PDF rendering performance
- Small images scaled up may appear pixelated
- Combine with `background-position` for optimal placement
- Use with `background-repeat: no-repeat` for single scaled images

---

## Data Binding

The `background-size` property supports dynamic data binding, enabling responsive and data-driven image scaling for logos, watermarks, and backgrounds that adapt to content or user preferences.

### Example 1: Dynamic logo sizing from company data

```html
<style>
    .company-letterhead {
        background-image: url('{{company.logoUrl}}');
        background-size: {{company.logoWidth}} {{company.logoHeight}};
        background-repeat: no-repeat;
        background-position: top center;
        padding-top: {{company.logoHeight + 20}}pt;
        padding-left: 40pt;
        padding-right: 40pt;
    }
</style>
<body>
    <div class="company-letterhead">
        <p>Date: {{letter.date}}</p>
        <p>Dear {{recipient.name}},</p>
        <p>{{letter.content}}</p>
    </div>
</body>
```

Perfect for multi-tenant systems where each organization has logos of different dimensions. The logo size and spacing automatically adjust based on database values, ensuring consistent professional appearance across all clients.

### Example 2: Conditional watermark sizing by page type

```html
<style>
    .document-page {
        background-image: url('{{watermark.imageUrl}}');
        background-size: {{#if page.type == 'cover'}}cover{{else if page.type == 'content'}}40%{{else}}250pt 80pt{{/if}};
        background-repeat: no-repeat;
        background-position: center;
        min-height: 792pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="document-page">
        <h1>{{page.title}}</h1>
        <p>{{page.content}}</p>
    </div>
</body>
```

Enables intelligent watermark sizing where cover pages use full-size backgrounds, content pages use proportional watermarks, and other pages use specific dimensions - all controlled by page type data.

### Example 3: User-configurable background images

```html
<style>
    .certificate {
        background-image: url('{{certificate.backgroundImageUrl}}');
        background-size: {{certificate.backgroundSize}};
        background-repeat: no-repeat;
        background-position: {{certificate.backgroundPosition}};
        min-height: 600pt;
        padding: 60pt;
        border: {{certificate.borderWidth}}pt solid {{certificate.borderColor}};
    }
    .certificate-content {
        text-align: center;
    }
</style>
<body>
    <div class="certificate">
        <div class="certificate-content">
            <h1 style="font-size: 28pt; color: {{certificate.titleColor}};">
                {{certificate.title}}
            </h1>
            <p style="margin-top: 30pt; font-size: 18pt;">Awarded to</p>
            <p style="font-size: 24pt; font-weight: bold;">{{recipient.name}}</p>
            <p style="margin-top: 20pt;">{{certificate.description}}</p>
        </div>
    </div>
</body>
```

Allows administrators to configure certificate templates with custom background images and sizing through a UI, storing preferences in a database. Users can choose from "contain", "cover", specific dimensions, or percentages for complete control over certificate appearance.

---

## Examples

### Example 1: Cover entire area

```html
<style>
    .cover-background {
        background-image: url('images/hero-image.jpg');
        background-size: cover;
        background-repeat: no-repeat;
        background-position: center;
        min-height: 400pt;
        padding: 40pt;
        color: white;
    }
</style>
<body>
    <div class="cover-background">
        <h1 style="font-size: 36pt;">Annual Report 2024</h1>
        <p style="font-size: 18pt;">Financial Performance Overview</p>
    </div>
</body>
```

### Example 2: Contain within area

```html
<style>
    .contained-logo {
        background-image: url('images/company-logo.png');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: center;
        height: 150pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="contained-logo"></div>
</body>
```

### Example 3: Explicit dimensions

```html
<style>
    .sized-watermark {
        background-image: url('images/watermark.png');
        background-size: 300pt 100pt;
        background-repeat: no-repeat;
        background-position: center;
        min-height: 600pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="sized-watermark">
        <h1>Confidential Document</h1>
        <p>This document contains sensitive information.</p>
    </div>
</body>
```

### Example 4: Percentage sizing

```html
<style>
    .percentage-bg {
        background-image: url('images/seal.png');
        background-size: 50%;
        background-repeat: no-repeat;
        background-position: center;
        min-height: 400pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="percentage-bg">
        <h2 style="text-align: center;">Certificate of Achievement</h2>
        <p style="text-align: center;">Awarded to Jane Smith</p>
    </div>
</body>
```

### Example 5: Auto width with fixed height

```html
<style>
    .auto-width-logo {
        background-image: url('images/wide-logo.png');
        background-size: auto 60pt;
        background-repeat: no-repeat;
        background-position: left center;
        padding-left: 200pt;
        padding-top: 20pt;
        padding-bottom: 20pt;
    }
</style>
<body>
    <div class="auto-width-logo">
        <h1>Business Report</h1>
        <p>Q4 2024 Results</p>
    </div>
</body>
```

### Example 6: Full-page cover background

```html
<style>
    .full-page-cover {
        background-image: url('images/abstract-background.jpg');
        background-size: cover;
        background-repeat: no-repeat;
        background-position: center;
        min-height: 792pt;
        padding: 80pt 60pt;
        color: white;
    }
    .cover-title {
        font-size: 48pt;
        font-weight: bold;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="full-page-cover">
        <h1 class="cover-title">Strategic Plan 2025</h1>
        <p style="font-size: 20pt;">Vision, Mission, and Goals</p>
    </div>
</body>
```

### Example 7: Small repeating pattern with size control

```html
<style>
    .sized-pattern {
        background-image: url('images/small-pattern.png');
        background-size: 40pt 40pt;
        background-repeat: repeat;
        padding: 25pt;
    }
</style>
<body>
    <div class="sized-pattern">
        <h2>Patterned Background</h2>
        <p>Uniform pattern tiles at specific size.</p>
    </div>
</body>
```

### Example 8: Certificate seal positioned and sized

```html
<style>
    .certificate-seal {
        background-image: url('images/gold-seal.png');
        background-size: 120pt 120pt;
        background-repeat: no-repeat;
        background-position: bottom right 30pt;
        padding: 40pt;
        padding-bottom: 160pt;
        min-height: 500pt;
        border: 3pt solid #b45309;
    }
</style>
<body>
    <div class="certificate-seal">
        <h1 style="text-align: center; font-size: 28pt; color: #b45309;">
            Certificate of Excellence
        </h1>
        <p style="text-align: center; margin-top: 30pt; font-size: 18pt;">
            This certifies that
        </p>
        <p style="text-align: center; font-size: 24pt; font-weight: bold; color: #1e40af;">
            Robert Johnson
        </p>
    </div>
</body>
```

### Example 9: Letterhead logo with precise sizing

```html
<style>
    .letterhead {
        background-image: url('images/letterhead-logo.png');
        background-size: 180pt 50pt;
        background-repeat: no-repeat;
        background-position: top center;
        padding-top: 70pt;
        padding-left: 40pt;
        padding-right: 40pt;
    }
</style>
<body>
    <div class="letterhead">
        <p>Date: January 15, 2025</p>
        <p>Dear Valued Customer,</p>
        <p>We are pleased to announce our new product line...</p>
    </div>
</body>
```

### Example 10: Background texture with controlled size

```html
<style>
    .textured-section {
        background-image: url('images/paper-texture.jpg');
        background-size: 200pt 200pt;
        background-repeat: repeat;
        padding: 30pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="textured-section">
        <h3>Textured Background</h3>
        <p>Consistent texture tile size across the section.</p>
    </div>
</body>
```

### Example 11: Invoice corner logo

```html
<style>
    .invoice-header {
        background-image: url('images/company-logo.png');
        background-size: 100pt 40pt;
        background-repeat: no-repeat;
        background-position: right 20pt top 20pt;
        padding: 30pt;
        min-height: 100pt;
    }
</style>
<body>
    <div class="invoice-header">
        <h1 style="font-size: 32pt; color: #1e3a8a;">INVOICE</h1>
        <p>Invoice #: INV-2025-150</p>
    </div>
</body>
```

### Example 12: Responsive watermark

```html
<style>
    .watermark-responsive {
        background-image: url('images/draft-watermark.png');
        background-size: 60%;
        background-repeat: no-repeat;
        background-position: center;
        min-height: 700pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="watermark-responsive">
        <h1>Project Proposal - DRAFT</h1>
        <p>Preliminary version for internal review.</p>
    </div>
</body>
```

### Example 13: Two background images with different sizes

```html
<style>
    .multi-background {
        background-image: url('images/logo.png'), url('images/pattern.png');
        background-size: 150pt 60pt, 50pt 50pt;
        background-repeat: no-repeat, repeat;
        background-position: top center, top left;
        padding-top: 80pt;
        padding-left: 30pt;
        padding-right: 30pt;
    }
</style>
<body>
    <div class="multi-background">
        <h1>Professional Document</h1>
        <p>With logo and background pattern.</p>
    </div>
</body>
```

### Example 14: Portrait photo background

```html
<style>
    .photo-background {
        background-image: url('images/team-photo.jpg');
        background-size: contain;
        background-repeat: no-repeat;
        background-position: left center;
        background-color: #f3f4f6;
        padding-left: 320pt;
        padding-top: 30pt;
        padding-bottom: 30pt;
        min-height: 300pt;
    }
</style>
<body>
    <div class="photo-background">
        <h2>Team Overview</h2>
        <p>Our dedicated team of professionals works together to deliver excellence.</p>
    </div>
</body>
```

### Example 15: Scaled security pattern

```html
<style>
    .security-background {
        background-image: url('images/microprint-pattern.png');
        background-size: 30pt 30pt;
        background-repeat: repeat;
        padding: 30pt;
    }
    .secure-content {
        background-color: rgba(255, 255, 255, 0.95);
        padding: 20pt;
        border: 2pt solid #dc2626;
    }
</style>
<body>
    <div class="security-background">
        <div class="secure-content">
            <h2>Secure Document</h2>
            <p>Protected with security pattern background.</p>
            <p>Unauthorized reproduction prohibited.</p>
        </div>
    </div>
</body>
```

---

## See Also

- [background](/reference/cssproperties/css_prop_background) - Shorthand for all background properties
- [background-image](/reference/cssproperties/css_prop_background-image) - Set background image
- [background-repeat](/reference/cssproperties/css_prop_background-repeat) - Control image repetition
- [background-position](/reference/cssproperties/css_prop_background-position) - Set image position
- [background-color](/reference/cssproperties/css_prop_background-color) - Set background color

---
