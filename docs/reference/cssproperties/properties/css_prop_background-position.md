---
layout: default
title: background-position
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# background-position : Background Position Property

The `background-position` property sets the starting position of background images within an element in PDF documents. This property is essential for precisely positioning logos, watermarks, seals, and decorative elements.

## Usage

```css
selector {
    background-position: value;
}
```

The background-position property controls where a background image is placed within its container, using keywords, percentages, or length values.

---

## Supported Values

### Keywords
- Horizontal: `left`, `center`, `right`
- Vertical: `top`, `center`, `bottom`
- Combined: `center center` (default), `top left`, `bottom right`, etc.
- Single keyword: `center` (centers in both directions)

### Percentage Values
- Single value: `50%` - Horizontal position, vertical defaults to 50%
- Two values: `25% 75%` - Horizontal and vertical positions
- `0% 0%` - Top left corner
- `100% 100%` - Bottom right corner
- `50% 50%` - Center (default)

### Length Values
- Single value: `20pt` - Horizontal offset from left, vertical centers
- Two values: `30pt 40pt` - Horizontal and vertical offsets from top-left
- Units: `pt` (points), `px` (pixels), `in` (inches), `cm` (centimeters), `mm` (millimeters)

### Mixed Values
- Keyword with length: `right 20pt bottom 30pt`
- Keyword with percentage: `center 25%`
- Percentage with length: `50% 20pt`

### Four-Value Syntax
- `right 20pt bottom 30pt` - 20pt from right edge, 30pt from bottom edge
- `left 10pt top 15pt` - 10pt from left edge, 15pt from top edge

---

## Supported Elements

The `background-position` property can be applied to:
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
- Default value is `0% 0%` (top left), but appears centered if image repeats
- First value is horizontal position, second is vertical
- Single keyword centers in the perpendicular direction
- Percentages align image and container at the same percentage point
- Length values offset from top-left corner by default
- Use with `background-repeat: no-repeat` for precise positioning
- Four-value syntax allows offset from any edge
- Combine with `background-size` for optimal image scaling
- Negative values move images outside the element boundaries
- Multiple background images can have different positions

---

## Data Binding

The `background-position` property supports dynamic data binding, enabling precise control over image placement based on data for customized document layouts and personalized positioning.

### Example 1: Dynamic logo positioning from preferences

```html
<style>
    .invoice-header {
        background-image: url('{{company.logoUrl}}');
        background-position: {{company.logoPosition}};
        background-repeat: no-repeat;
        background-size: {{company.logoWidth}} {{company.logoHeight}};
        padding: 30pt;
        min-height: 120pt;
    }
</style>
<body>
    <div class="invoice-header">
        <h1 style="font-size: 32pt; color: {{company.primaryColor}};">INVOICE</h1>
        <p>Invoice #: {{invoice.number}}</p>
        <p>Date: {{invoice.date}}</p>
    </div>
</body>
```

Allows each organization to specify their preferred logo position (e.g., "top left", "right 20pt top 20pt", "center") through configuration, ensuring invoices match their exact branding guidelines stored in the database.

### Example 2: Conditional positioning based on document layout

```html
<style>
    .certificate-page {
        background-image: url('{{certificate.sealUrl}}');
        background-position: {{#if certificate.layout == 'classic'}}bottom right{{else if certificate.layout == 'modern'}}bottom center{{else}}center center{{/if}};
        background-repeat: no-repeat;
        background-size: {{certificate.sealSize}};
        padding: 60pt;
        min-height: 600pt;
        border: 3pt solid {{certificate.borderColor}};
    }
</style>
<body>
    <div class="certificate-page">
        <h1 style="text-align: center; font-size: 28pt;">{{certificate.title}}</h1>
        <p style="text-align: center; margin-top: 40pt;">Presented to</p>
        <p style="text-align: center; font-size: 24pt; font-weight: bold;">{{recipient.name}}</p>
    </div>
</body>
```

Automatically adjusts seal position based on certificate layout style - classic layouts place seals in the bottom right corner, modern layouts center them at the bottom, while alternative layouts center them completely.

### Example 3: Multi-element positioning with data arrays

```html
<style>
    .complex-header {
        background-image: url('{{branding.mainLogoUrl}}'), url('{{branding.accentImageUrl}}');
        background-position: {{branding.mainLogoPosition}}, {{branding.accentPosition}};
        background-repeat: no-repeat, no-repeat;
        background-size: {{branding.mainLogoSize}}, {{branding.accentSize}};
        padding: {{branding.headerPadding}};
        min-height: {{branding.headerHeight}};
    }
</style>
<body>
    <div class="complex-header">
        <h1>{{document.title}}</h1>
        <p>{{document.subtitle}}</p>
    </div>
</body>
```

Enables sophisticated multi-layered backgrounds where both main logos and accent graphics have data-driven positioning. Perfect for complex branded documents requiring precise placement of multiple visual elements based on brand guidelines.

---

## Examples

### Example 1: Centered background

```html
<style>
    .centered-logo {
        background-image: url('images/company-logo.png');
        background-position: center;
        background-repeat: no-repeat;
        min-height: 200pt;
        padding: 20pt;
    }
</style>
<body>
    <div class="centered-logo">
        <h2 style="text-align: center; margin-top: 120pt;">Company Report</h2>
    </div>
</body>
```

### Example 2: Top left corner

```html
<style>
    .top-left-logo {
        background-image: url('images/logo.png');
        background-position: top left;
        background-repeat: no-repeat;
        padding: 80pt 20pt 20pt 80pt;
    }
</style>
<body>
    <div class="top-left-logo">
        <h1>Document Title</h1>
        <p>Content begins here...</p>
    </div>
</body>
```

### Example 3: Bottom right seal

```html
<style>
    .certificate {
        background-image: url('images/official-seal.png');
        background-position: bottom right;
        background-repeat: no-repeat;
        min-height: 500pt;
        padding: 40pt;
    }
    .cert-title {
        text-align: center;
        font-size: 28pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">Certificate of Achievement</h1>
        <p style="text-align: center; margin-top: 30pt;">Awarded to</p>
        <p style="text-align: center; font-size: 22pt; font-weight: bold;">Emily Davis</p>
    </div>
</body>
```

### Example 4: Precise offset from top-left

```html
<style>
    .positioned-logo {
        background-image: url('images/small-logo.png');
        background-position: 20pt 30pt;
        background-repeat: no-repeat;
        padding: 80pt 20pt 20pt 100pt;
    }
</style>
<body>
    <div class="positioned-logo">
        <h2>Business Letter</h2>
        <p>Date: January 15, 2025</p>
    </div>
</body>
```

### Example 5: Right-aligned with offset

```html
<style>
    .right-offset-logo {
        background-image: url('images/company-logo.png');
        background-position: right 20pt top 20pt;
        background-repeat: no-repeat;
        padding: 30pt;
        min-height: 100pt;
    }
</style>
<body>
    <div class="right-offset-logo">
        <h1 style="font-size: 32pt; color: #1e3a8a;">INVOICE</h1>
        <p>Invoice Number: INV-2025-200</p>
    </div>
</body>
```

### Example 6: Percentage-based positioning

```html
<style>
    .percent-position {
        background-image: url('images/watermark.png');
        background-position: 75% 25%;
        background-repeat: no-repeat;
        background-size: 200pt 80pt;
        min-height: 600pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="percent-position">
        <h1>Document Title</h1>
        <p>Content with positioned watermark.</p>
    </div>
</body>
```

### Example 7: Multiple positions for multiple backgrounds

```html
<style>
    .multi-bg-position {
        background-image: url('images/logo.png'), url('images/pattern.png');
        background-position: top center, center center;
        background-repeat: no-repeat, repeat;
        background-size: 150pt 60pt, 50pt 50pt;
        padding-top: 80pt;
        padding-left: 30pt;
        padding-right: 30pt;
    }
</style>
<body>
    <div class="multi-bg-position">
        <h1>Professional Document</h1>
        <p>With positioned logo and background pattern.</p>
    </div>
</body>
```

### Example 8: Watermark in center

```html
<style>
    .centered-watermark {
        background-image: url('images/confidential-watermark.png');
        background-position: center center;
        background-repeat: no-repeat;
        background-size: 350pt 120pt;
        min-height: 700pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="centered-watermark">
        <h1>Confidential Report</h1>
        <p>This document contains sensitive information.</p>
    </div>
</body>
```

### Example 9: Footer logo bottom center

```html
<style>
    .footer-logo {
        background-image: url('images/footer-logo.png');
        background-position: bottom center;
        background-repeat: no-repeat;
        background-size: 120pt 40pt;
        padding-bottom: 60pt;
        padding-top: 20pt;
        padding-left: 20pt;
        padding-right: 20pt;
        border-top: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="footer-logo">
        <p style="text-align: center; font-size: 9pt;">
            © 2025 Company Name. All rights reserved.
        </p>
        <p style="text-align: center; font-size: 9pt;">
            123 Business Street | contact@company.com
        </p>
    </div>
</body>
```

### Example 10: Top right corner with margin

```html
<style>
    .corner-logo {
        background-image: url('images/stamp.png');
        background-position: right 15pt top 15pt;
        background-repeat: no-repeat;
        background-size: 80pt 80pt;
        padding: 30pt;
        min-height: 150pt;
    }
</style>
<body>
    <div class="corner-logo">
        <h2>Approved Document</h2>
        <p>This document has been reviewed and approved.</p>
    </div>
</body>
```

### Example 11: Left sidebar decoration

```html
<style>
    .left-decoration {
        background-image: url('images/vertical-accent.png');
        background-position: left center;
        background-repeat: no-repeat;
        background-size: 10pt 200pt;
        padding-left: 30pt;
        padding-top: 20pt;
        padding-bottom: 20pt;
        min-height: 250pt;
    }
</style>
<body>
    <div class="left-decoration">
        <h3>Important Section</h3>
        <p>This section is marked with a vertical accent on the left.</p>
    </div>
</body>
```

### Example 12: Certificate with multiple positioned elements

```html
<style>
    .certificate-complex {
        background-image: url('images/seal.png'), url('images/ribbon.png');
        background-position: bottom left 40pt, top right 30pt;
        background-repeat: no-repeat, no-repeat;
        background-size: 100pt 100pt, 60pt 120pt;
        padding: 60pt;
        min-height: 500pt;
        border: 3pt solid #b45309;
    }
</style>
<body>
    <div class="certificate-complex">
        <h1 style="text-align: center; font-size: 28pt; color: #b45309;">
            Certificate of Excellence
        </h1>
        <p style="text-align: center; margin-top: 40pt; font-size: 18pt;">
            This certifies that
        </p>
        <p style="text-align: center; font-size: 24pt; font-weight: bold; color: #1e40af;">
            Alexandra Martinez
        </p>
    </div>
</body>
```

### Example 13: Offset from bottom edge

```html
<style>
    .bottom-offset-logo {
        background-image: url('images/logo-watermark.png');
        background-position: center bottom 50pt;
        background-repeat: no-repeat;
        background-size: 200pt 60pt;
        min-height: 600pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="bottom-offset-logo">
        <h1>Annual Report</h1>
        <p>Executive summary and financial highlights...</p>
    </div>
</body>
```

### Example 14: Repeating border at specific position

```html
<style>
    .border-decoration {
        background-image: url('images/decorative-border.png');
        background-position: bottom left;
        background-repeat: repeat-x;
        background-size: auto 20pt;
        padding-bottom: 35pt;
        padding-top: 20pt;
        padding-left: 20pt;
        padding-right: 20pt;
    }
</style>
<body>
    <div class="border-decoration">
        <h2>Section Title</h2>
        <p>Content with decorative bottom border.</p>
    </div>
</body>
```

### Example 15: Complex letterhead layout

```html
<style>
    .letterhead-complex {
        background-image:
            url('images/logo-main.png'),
            url('images/contact-icon.png'),
            url('images/footer-line.png');
        background-position:
            top left 40pt,
            top right 40pt,
            bottom left;
        background-repeat:
            no-repeat,
            no-repeat,
            repeat-x;
        background-size:
            120pt 50pt,
            30pt 30pt,
            auto 2pt;
        padding: 80pt 40pt 25pt 40pt;
        min-height: 700pt;
    }
</style>
<body>
    <div class="letterhead-complex">
        <p>Date: January 15, 2025</p>
        <p>Dear Valued Customer,</p>
        <p>We are pleased to present our quarterly update...</p>
    </div>
</body>
```

---

## See Also

- [background](/reference/cssproperties/css_prop_background) - Shorthand for all background properties
- [background-image](/reference/cssproperties/css_prop_background-image) - Set background image
- [background-repeat](/reference/cssproperties/css_prop_background-repeat) - Control image repetition
- [background-size](/reference/cssproperties/css_prop_background-size) - Control image sizing
- [background-position-x](/reference/cssproperties/css_prop_background-position-x) - Set horizontal position only
- [background-position-y](/reference/cssproperties/css_prop_background-position-y) - Set vertical position only
- [background-color](/reference/cssproperties/css_prop_background-color) - Set background color

---
