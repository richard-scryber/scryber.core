---
layout: default
title: background-position-y
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# background-position-y : Background Vertical Position Property

The `background-position-y` property sets the vertical position of background images within an element in PDF documents. This property allows independent control of vertical positioning without affecting horizontal placement.

## Usage

```css
selector {
    background-position-y: value;
}
```

The background-position-y property controls the vertical placement of a background image, allowing fine-tuned positioning along the y-axis independently from horizontal positioning.

---

## Supported Values

### Keywords
- `top` - Align to top edge (equivalent to 0%)
- `center` - Center vertically (equivalent to 50%)
- `bottom` - Align to bottom edge (equivalent to 100%)

### Percentage Values
- `0%` - Top edge
- `50%` - Center (default)
- `100%` - Bottom edge
- Any percentage: `25%`, `75%`, etc.
- Percentages align image and container at the same percentage point

### Length Values
- Offset from top edge: `20pt`, `30px`, `1in`, etc.
- Units: `pt` (points), `px` (pixels), `in` (inches), `cm` (centimeters), `mm` (millimeters)
- Positive values move down, negative values move up

### Edge-Offset Syntax
- `bottom 20pt` - 20pt from bottom edge
- `top 30pt` - 30pt from top edge

---

## Supported Elements

The `background-position-y` property can be applied to:
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
- Default value is `50%` (center)
- Does not affect horizontal positioning (use `background-position-x` for that)
- Can be used independently or combined with `background-position-x`
- More specific than `background-position` when only vertical adjustment is needed
- Useful for responsive layouts where only vertical positioning changes
- Percentages align the image percentage point with container percentage point
- Length values offset from the top edge by default
- Negative values move images outside the element boundaries
- Use with `background-repeat: no-repeat` for precise positioning

---

## Data Binding

The `background-position-y` property supports dynamic data binding, enabling vertical positioning control based on data for flexible document layouts and adaptive designs.

### Example 1: Dynamic vertical positioning from layout preferences

```html
<style>
    .document-watermark {
        background-image: url('{{watermark.imageUrl}}');
        background-position-x: center;
        background-position-y: {{watermark.verticalPosition}};
        background-repeat: no-repeat;
        background-size: {{watermark.width}}pt {{watermark.height}}pt;
        min-height: 792pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="document-watermark">
        <h1>{{document.title}}</h1>
        <p>{{document.content}}</p>
    </div>
</body>
```

Enables administrators to control watermark vertical placement (top, center, bottom, or specific offsets) through configuration while keeping horizontal centering. Perfect for different document types requiring watermarks at various vertical positions.

### Example 2: Conditional vertical positioning by content length

```html
<style>
    .certificate-seal {
        background-image: url('{{certificate.sealUrl}}');
        background-position-x: center;
        background-position-y: {{#if certificate.hasLongText}}bottom 40pt{{else}}60%{{/if}};
        background-repeat: no-repeat;
        background-size: 100pt 100pt;
        padding: 60pt;
        min-height: {{certificate.minimumHeight}};
    }
</style>
<body>
    <div class="certificate-seal">
        <h1 style="text-align: center; font-size: 28pt;">{{certificate.title}}</h1>
        <p style="text-align: center; margin-top: 30pt;">{{certificate.awardText}}</p>
        <p style="text-align: center; font-size: 24pt; font-weight: bold;">{{recipient.name}}</p>
        {{#if certificate.hasLongText}}
        <p style="text-align: center; margin-top: 20pt;">{{certificate.additionalText}}</p>
        {{/if}}
    </div>
</body>
```

Intelligently adjusts seal vertical position based on content - certificates with extensive text push seals to the bottom with fixed offset, while shorter certificates position seals proportionally for better balance.

### Example 3: Multi-layer vertical positioning with data

```html
<style>
    .branded-document {
        background-image: url('{{branding.headerImageUrl}}'), url('{{branding.footerImageUrl}}');
        background-position-x: center, center;
        background-position-y: {{branding.headerOffset}}, {{branding.footerOffset}};
        background-repeat: repeat-x, repeat-x;
        background-size: auto {{branding.headerHeight}}pt, auto {{branding.footerHeight}}pt;
        padding-top: {{branding.headerOffset + branding.headerHeight + 20}}pt;
        padding-bottom: {{branding.footerHeight + 20}}pt;
        min-height: 792pt;
    }
</style>
<body>
    <div class="branded-document">
        <h1>{{document.title}}</h1>
        <p>{{document.content}}</p>
    </div>
</body>
```

Creates documents with separate header and footer decorative elements, each with configurable vertical positioning. Organizations can fine-tune the exact placement of both elements independently to match their brand guidelines and document requirements.

---

## Examples

### Example 1: Top-aligned logo

```html
<style>
    .top-logo {
        background-image: url('images/company-logo.png');
        background-position-x: center;
        background-position-y: top;
        background-repeat: no-repeat;
        min-height: 150pt;
        padding-top: 80pt;
        padding-left: 20pt;
        padding-right: 20pt;
    }
</style>
<body>
    <div class="top-logo">
        <h2>Company Report</h2>
        <p>Annual financial summary</p>
    </div>
</body>
```

### Example 2: Bottom-aligned seal

```html
<style>
    .bottom-seal {
        background-image: url('images/official-seal.png');
        background-position-x: center;
        background-position-y: bottom;
        background-repeat: no-repeat;
        background-size: 100pt 100pt;
        padding: 40pt;
        padding-bottom: 120pt;
        min-height: 500pt;
    }
</style>
<body>
    <div class="bottom-seal">
        <h1 style="text-align: center; font-size: 28pt;">Certificate</h1>
        <p style="text-align: center; margin-top: 30pt;">Awarded to Jane Smith</p>
    </div>
</body>
```

### Example 3: Centered vertically

```html
<style>
    .centered-vertical {
        background-image: url('images/watermark.png');
        background-position-x: center;
        background-position-y: center;
        background-repeat: no-repeat;
        background-size: 300pt 100pt;
        min-height: 600pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="centered-vertical">
        <h1>Confidential Document</h1>
        <p>Vertically centered watermark.</p>
    </div>
</body>
```

### Example 4: Offset from top edge

```html
<style>
    .top-offset {
        background-image: url('images/header-graphic.png');
        background-position-x: center;
        background-position-y: 30pt;
        background-repeat: no-repeat;
        background-size: 200pt 60pt;
        padding-top: 110pt;
        padding-left: 30pt;
        padding-right: 30pt;
    }
</style>
<body>
    <div class="top-offset">
        <h1>Document Title</h1>
        <p>With header graphic 30pt from top.</p>
    </div>
</body>
```

### Example 5: Offset from bottom edge

```html
<style>
    .bottom-offset {
        background-image: url('images/footer-logo.png');
        background-position-x: center;
        background-position-y: bottom 25pt;
        background-repeat: no-repeat;
        background-size: 120pt 40pt;
        padding: 30pt;
        padding-bottom: 80pt;
        min-height: 400pt;
    }
</style>
<body>
    <div class="bottom-offset">
        <h2>Report Section</h2>
        <p>Footer logo 25pt from bottom edge.</p>
    </div>
</body>
```

### Example 6: Percentage-based vertical positioning

```html
<style>
    .percent-vertical {
        background-image: url('images/accent-graphic.png');
        background-position-x: left;
        background-position-y: 25%;
        background-repeat: no-repeat;
        background-size: 50pt 100pt;
        padding-left: 70pt;
        padding-top: 30pt;
        padding-bottom: 30pt;
        min-height: 400pt;
    }
</style>
<body>
    <div class="percent-vertical">
        <h2>Important Section</h2>
        <p>Accent graphic positioned at 25% vertically.</p>
    </div>
</body>
```

### Example 7: Multiple background images with different y positions

```html
<style>
    .multi-y-position {
        background-image: url('images/top-banner.png'), url('images/bottom-banner.png');
        background-position-x: center, center;
        background-position-y: top, bottom;
        background-repeat: repeat-x, repeat-x;
        background-size: auto 20pt, auto 20pt;
        padding-top: 35pt;
        padding-bottom: 35pt;
        padding-left: 30pt;
        padding-right: 30pt;
        min-height: 300pt;
    }
</style>
<body>
    <div class="multi-y-position">
        <h2 style="text-align: center;">Framed Content</h2>
        <p style="text-align: center;">With decorative top and bottom borders</p>
    </div>
</body>
```

### Example 8: Watermark at specific vertical position

```html
<style>
    .watermark-position {
        background-image: url('images/draft-watermark.png');
        background-position-x: center;
        background-position-y: 35%;
        background-repeat: no-repeat;
        background-size: 350pt 120pt;
        min-height: 700pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="watermark-position">
        <h1>Project Proposal - DRAFT</h1>
        <p>This is a preliminary version of the proposal.</p>
    </div>
</body>
```

### Example 9: Certificate ribbon at top

```html
<style>
    .certificate-ribbon {
        background-image: url('images/ribbon-graphic.png');
        background-position-x: center;
        background-position-y: top;
        background-repeat: no-repeat;
        background-size: 80pt 120pt;
        padding-top: 140pt;
        padding-left: 40pt;
        padding-right: 40pt;
        min-height: 500pt;
        border: 3pt solid #b45309;
    }
</style>
<body>
    <div class="certificate-ribbon">
        <h1 style="text-align: center; font-size: 28pt; color: #b45309;">
            Certificate of Excellence
        </h1>
        <p style="text-align: center; margin-top: 30pt; font-size: 18pt;">
            Presented to
        </p>
        <p style="text-align: center; font-size: 24pt; font-weight: bold; color: #1e40af;">
            Michael Anderson
        </p>
    </div>
</body>
```

### Example 10: Invoice stamp at bottom

```html
<style>
    .invoice-stamp {
        background-image: url('images/paid-stamp.png');
        background-position-x: right 30pt;
        background-position-y: bottom 30pt;
        background-repeat: no-repeat;
        background-size: 100pt 100pt;
        padding: 30pt;
        padding-bottom: 150pt;
        min-height: 500pt;
    }
</style>
<body>
    <div class="invoice-stamp">
        <h1 style="font-size: 32pt; color: #1e3a8a;">INVOICE</h1>
        <p>Invoice #: INV-2025-350</p>
        <p>Date: January 15, 2025</p>
        <p>Amount Due: $1,250.00</p>
    </div>
</body>
```

### Example 11: Letterhead logo at precise vertical position

```html
<style>
    .letterhead-precise {
        background-image: url('images/letterhead-logo.png');
        background-position-x: center;
        background-position-y: 40pt;
        background-repeat: no-repeat;
        background-size: 150pt 55pt;
        padding-top: 115pt;
        padding-left: 40pt;
        padding-right: 40pt;
    }
</style>
<body>
    <div class="letterhead-precise">
        <p>Date: January 15, 2025</p>
        <p>Dear Customer,</p>
        <p>We are pleased to present...</p>
    </div>
</body>
```

### Example 12: Decorative element at middle-top

```html
<style>
    .decorative-top {
        background-image: url('images/ornament.png');
        background-position-x: center;
        background-position-y: 20%;
        background-repeat: no-repeat;
        background-size: 80pt 60pt;
        padding: 50pt 30pt;
        min-height: 400pt;
    }
</style>
<body>
    <div class="decorative-top">
        <h2 style="text-align: center; margin-top: 80pt;">Elegant Document</h2>
        <p style="text-align: center;">With decorative element near top</p>
    </div>
</body>
```

### Example 13: Report section with accent bar

```html
<style>
    .accent-bar {
        background-image: url('images/horizontal-accent.png');
        background-position-x: left;
        background-position-y: center;
        background-repeat: repeat-x;
        background-size: auto 3pt;
        padding-top: 30pt;
        padding-bottom: 30pt;
        padding-left: 20pt;
        padding-right: 20pt;
        min-height: 200pt;
    }
</style>
<body>
    <div class="accent-bar">
        <h3>Key Findings</h3>
        <p>This section is divided by a centered horizontal accent.</p>
    </div>
</body>
```

### Example 14: Footer badge precisely positioned

```html
<style>
    .footer-badge {
        background-image: url('images/quality-badge.png');
        background-position-x: right 40pt;
        background-position-y: bottom 15pt;
        background-repeat: no-repeat;
        background-size: 60pt 60pt;
        padding: 20pt;
        padding-right: 120pt;
        padding-bottom: 80pt;
        border-top: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="footer-badge">
        <p style="font-size: 9pt;">© 2025 Company Name. All rights reserved.</p>
        <p style="font-size: 9pt;">ISO 9001 Certified Quality Management</p>
    </div>
</body>
```

### Example 15: Adjusting only vertical position

```html
<style>
    .default-center {
        background-image: url('images/logo.png');
        background-position: center center;
        background-repeat: no-repeat;
        background-size: 150pt 60pt;
        min-height: 300pt;
        padding: 30pt;
        border: 1pt solid #d1d5db;
    }
    .adjusted-y-position {
        background-image: url('images/logo.png');
        background-position-x: center;
        background-position-y: top 40pt;
        background-repeat: no-repeat;
        background-size: 150pt 60pt;
        min-height: 300pt;
        padding: 30pt;
        border: 1pt solid #d1d5db;
        margin-top: 20pt;
    }
</style>
<body>
    <div class="default-center">
        <h3 style="text-align: center; margin-top: 120pt;">Default Centered</h3>
    </div>
    <div class="adjusted-y-position">
        <h3 style="text-align: center; margin-top: 120pt;">Adjusted Y Position</h3>
        <p style="text-align: center;">Horizontal position remains centered</p>
    </div>
</body>
```

---

## See Also

- [background](/reference/cssproperties/css_prop_background) - Shorthand for all background properties
- [background-position](/reference/cssproperties/css_prop_background-position) - Set both horizontal and vertical position
- [background-position-x](/reference/cssproperties/css_prop_background-position-x) - Set horizontal position only
- [background-image](/reference/cssproperties/css_prop_background-image) - Set background image
- [background-repeat](/reference/cssproperties/css_prop_background-repeat) - Control image repetition
- [background-size](/reference/cssproperties/css_prop_background-size) - Control image sizing

---
