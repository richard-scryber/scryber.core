---
layout: default
title: background-position-x
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# background-position-x : Background Horizontal Position Property

The `background-position-x` property sets the horizontal position of background images within an element in PDF documents. This property allows independent control of horizontal positioning without affecting vertical placement.

## Usage

```css
selector {
    background-position-x: value;
}
```

The background-position-x property controls the horizontal placement of a background image, allowing fine-tuned positioning along the x-axis independently from vertical positioning.

---

## Supported Values

### Keywords
- `left` - Align to left edge (equivalent to 0%)
- `center` - Center horizontally (equivalent to 50%)
- `right` - Align to right edge (equivalent to 100%)

### Percentage Values
- `0%` - Left edge
- `50%` - Center (default)
- `100%` - Right edge
- Any percentage: `25%`, `75%`, etc.
- Percentages align image and container at the same percentage point

### Length Values
- Offset from left edge: `20pt`, `30px`, `1in`, etc.
- Units: `pt` (points), `px` (pixels), `in` (inches), `cm` (centimeters), `mm` (millimeters)
- Positive values move right, negative values move left

### Edge-Offset Syntax
- `right 20pt` - 20pt from right edge
- `left 30pt` - 30pt from left edge

---

## Supported Elements

The `background-position-x` property can be applied to:
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
- Does not affect vertical positioning (use `background-position-y` for that)
- Can be used independently or combined with `background-position-y`
- More specific than `background-position` when only horizontal adjustment is needed
- Useful for responsive layouts where only horizontal positioning changes
- Percentages align the image percentage point with container percentage point
- Length values offset from the left edge by default
- Negative values move images outside the element boundaries
- Use with `background-repeat: no-repeat` for precise positioning

---

## Data Binding

The `background-position-x` property supports dynamic data binding, enabling horizontal positioning control based on data for flexible document layouts and responsive designs.

### Example 1: Dynamic horizontal alignment from user preferences

```html
<style>
    .letterhead {
        background-image: url('{{company.logoUrl}}');
        background-position-x: {{company.logoHorizontalPosition}};
        background-position-y: top;
        background-repeat: no-repeat;
        background-size: {{company.logoWidth}} {{company.logoHeight}};
        padding-top: {{company.logoHeight + 30}}pt;
        padding-left: 40pt;
        padding-right: 40pt;
    }
</style>
<body>
    <div class="letterhead">
        <p>Date: {{document.date}}</p>
        <p>Dear {{recipient.name}},</p>
        <p>{{document.content}}</p>
    </div>
</body>
```

Allows organizations to control horizontal logo placement (left, center, right, or specific offsets) through configuration while keeping vertical position fixed at the top. Perfect for accommodating different brand guidelines in multi-tenant systems.

### Example 2: Conditional horizontal positioning by document type

```html
<style>
    .document-header {
        background-image: url('{{branding.iconUrl}}');
        background-position-x: {{#if document.formal}}center{{else}}left 20pt{{/if}};
        background-position-y: center;
        background-repeat: no-repeat;
        background-size: 60pt 60pt;
        min-height: 100pt;
        padding: 20pt;
    }
</style>
<body>
    <div class="document-header">
        <h2>{{document.title}}</h2>
        <p>{{document.description}}</p>
    </div>
</body>
```

Automatically centers icons for formal documents while left-aligning them for informal ones. The vertical position remains consistent while horizontal placement adapts to document formality level.

### Example 3: Responsive horizontal spacing for multiple logos

```html
<style>
    .partner-header {
        background-image: url('{{primaryLogo.url}}'), url('{{partnerLogo.url}}');
        background-position-x: left {{margins.left}}pt, right {{margins.right}}pt;
        background-position-y: center, center;
        background-repeat: no-repeat, no-repeat;
        background-size: {{primaryLogo.width}}pt {{primaryLogo.height}}pt, {{partnerLogo.width}}pt {{partnerLogo.height}}pt;
        padding: 30pt;
        min-height: 100pt;
    }
</style>
<body>
    <div class="partner-header">
        <h1 style="text-align: center;">{{document.title}}</h1>
        <p style="text-align: center;">A Partnership Initiative</p>
    </div>
</body>
```

Creates partnership documents with two logos positioned with dynamic horizontal offsets from edges. Margins can be adjusted per document to accommodate different logo sizes while maintaining professional spacing.

---

## Examples

### Example 1: Left-aligned logo

```html
<style>
    .left-logo {
        background-image: url('images/company-logo.png');
        background-position-x: left;
        background-position-y: center;
        background-repeat: no-repeat;
        min-height: 100pt;
        padding-left: 120pt;
        padding-top: 20pt;
    }
</style>
<body>
    <div class="left-logo">
        <h2>Document Header</h2>
        <p>With left-aligned logo</p>
    </div>
</body>
```

### Example 2: Right-aligned logo

```html
<style>
    .right-logo {
        background-image: url('images/logo.png');
        background-position-x: right;
        background-position-y: top;
        background-repeat: no-repeat;
        padding: 20pt;
        padding-right: 120pt;
    }
</style>
<body>
    <div class="right-logo">
        <h1>Invoice</h1>
        <p>INV-2025-250</p>
    </div>
</body>
```

### Example 3: Centered horizontally

```html
<style>
    .centered-horizontal {
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
    <div class="centered-horizontal">
        <h1>Confidential Document</h1>
        <p>Horizontally centered watermark.</p>
    </div>
</body>
```

### Example 4: Offset from left edge

```html
<style>
    .left-offset {
        background-image: url('images/small-icon.png');
        background-position-x: 20pt;
        background-position-y: 30pt;
        background-repeat: no-repeat;
        padding-left: 70pt;
        padding-top: 20pt;
    }
</style>
<body>
    <div class="left-offset">
        <h3>Section Title</h3>
        <p>With icon offset 20pt from left edge.</p>
    </div>
</body>
```

### Example 5: Offset from right edge

```html
<style>
    .right-offset {
        background-image: url('images/seal.png');
        background-position-x: right 30pt;
        background-position-y: bottom;
        background-repeat: no-repeat;
        background-size: 80pt 80pt;
        padding: 30pt;
        padding-bottom: 100pt;
        min-height: 400pt;
    }
</style>
<body>
    <div class="right-offset">
        <h2>Certificate</h2>
        <p>With seal 30pt from right edge.</p>
    </div>
</body>
```

### Example 6: Percentage-based horizontal positioning

```html
<style>
    .percent-horizontal {
        background-image: url('images/logo.png');
        background-position-x: 75%;
        background-position-y: top;
        background-repeat: no-repeat;
        padding: 30pt;
        min-height: 120pt;
    }
</style>
<body>
    <div class="percent-horizontal">
        <h1>Business Report</h1>
        <p>Logo positioned at 75% horizontally.</p>
    </div>
</body>
```

### Example 7: Multiple background images with different x positions

```html
<style>
    .multi-x-position {
        background-image: url('images/left-icon.png'), url('images/right-icon.png');
        background-position-x: left 20pt, right 20pt;
        background-position-y: top, top;
        background-repeat: no-repeat, no-repeat;
        background-size: 40pt 40pt, 40pt 40pt;
        padding: 60pt 80pt 20pt 80pt;
    }
</style>
<body>
    <div class="multi-x-position">
        <h2 style="text-align: center;">Decorated Header</h2>
        <p style="text-align: center;">Icons on both sides</p>
    </div>
</body>
```

### Example 8: Sidebar accent

```html
<style>
    .sidebar-accent {
        background-image: url('images/vertical-bar.png');
        background-position-x: left;
        background-position-y: center;
        background-repeat: no-repeat;
        background-size: 5pt 150pt;
        padding-left: 20pt;
        padding-top: 15pt;
        padding-bottom: 15pt;
        min-height: 200pt;
    }
</style>
<body>
    <div class="sidebar-accent">
        <h3>Important Notice</h3>
        <p>This section is marked with a left accent bar.</p>
    </div>
</body>
```

### Example 9: Invoice logo right-aligned with offset

```html
<style>
    .invoice-logo {
        background-image: url('images/company-logo.png');
        background-position-x: right 25pt;
        background-position-y: 25pt;
        background-repeat: no-repeat;
        background-size: 100pt 40pt;
        padding: 30pt;
        min-height: 100pt;
    }
</style>
<body>
    <div class="invoice-logo">
        <h1 style="font-size: 32pt; color: #1e3a8a;">INVOICE</h1>
        <p>Invoice #: INV-2025-300</p>
        <p>Date: January 15, 2025</p>
    </div>
</body>
```

### Example 10: Letterhead with left-positioned logo

```html
<style>
    .letterhead-left {
        background-image: url('images/letterhead-logo.png');
        background-position-x: 40pt;
        background-position-y: top;
        background-repeat: no-repeat;
        background-size: 120pt 50pt;
        padding-top: 70pt;
        padding-left: 40pt;
        padding-right: 40pt;
    }
</style>
<body>
    <div class="letterhead-left">
        <p>Date: January 15, 2025</p>
        <p>Dear Customer,</p>
        <p>We are writing to inform you...</p>
    </div>
</body>
```

### Example 11: Watermark offset to right

```html
<style>
    .watermark-right-offset {
        background-image: url('images/draft-watermark.png');
        background-position-x: 60%;
        background-position-y: center;
        background-repeat: no-repeat;
        background-size: 280pt 90pt;
        min-height: 700pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="watermark-right-offset">
        <h1>Project Proposal - DRAFT</h1>
        <p>This is a preliminary version.</p>
    </div>
</body>
```

### Example 12: Footer logo centered horizontally

```html
<style>
    .footer-centered {
        background-image: url('images/footer-logo.png');
        background-position-x: center;
        background-position-y: bottom;
        background-repeat: no-repeat;
        background-size: 100pt 35pt;
        padding: 20pt;
        padding-bottom: 50pt;
        border-top: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="footer-centered">
        <p style="text-align: center; font-size: 9pt;">
            © 2025 Company Name. All rights reserved.
        </p>
    </div>
</body>
```

### Example 13: Certificate seal positioned precisely

```html
<style>
    .certificate-seal {
        background-image: url('images/gold-seal.png');
        background-position-x: right 40pt;
        background-position-y: bottom 40pt;
        background-repeat: no-repeat;
        background-size: 100pt 100pt;
        padding: 40pt;
        min-height: 500pt;
        border: 3pt solid #b45309;
    }
</style>
<body>
    <div class="certificate-seal">
        <h1 style="text-align: center; font-size: 28pt; color: #b45309;">
            Certificate of Achievement
        </h1>
        <p style="text-align: center; margin-top: 30pt; font-size: 18pt;">
            Awarded to
        </p>
        <p style="text-align: center; font-size: 24pt; font-weight: bold; color: #1e40af;">
            Christopher Lee
        </p>
    </div>
</body>
```

### Example 14: Decorative element at specific percentage

```html
<style>
    .decorative-element {
        background-image: url('images/ornament.png');
        background-position-x: 25%;
        background-position-y: top;
        background-repeat: no-repeat;
        background-size: 60pt 60pt;
        padding-top: 80pt;
        padding-left: 30pt;
        padding-right: 30pt;
    }
</style>
<body>
    <div class="decorative-element">
        <h2>Elegant Section</h2>
        <p>With decorative element at 25% from left.</p>
    </div>
</body>
```

### Example 15: Adjusting only horizontal position

```html
<style>
    .default-position {
        background-image: url('images/pattern.png');
        background-position: center center;
        background-repeat: no-repeat;
        background-size: 200pt 150pt;
        min-height: 300pt;
        padding: 30pt;
        border: 1pt solid #d1d5db;
    }
    .adjusted-x-position {
        background-image: url('images/pattern.png');
        background-position-x: left 30pt;
        background-position-y: center;
        background-repeat: no-repeat;
        background-size: 200pt 150pt;
        min-height: 300pt;
        padding: 30pt;
        border: 1pt solid #d1d5db;
        margin-top: 20pt;
    }
</style>
<body>
    <div class="default-position">
        <h3>Default Centered Position</h3>
    </div>
    <div class="adjusted-x-position">
        <h3>Adjusted X Position Only</h3>
        <p>Vertical position remains centered.</p>
    </div>
</body>
```

---

## See Also

- [background](/reference/cssproperties/css_prop_background) - Shorthand for all background properties
- [background-position](/reference/cssproperties/css_prop_background-position) - Set both horizontal and vertical position
- [background-position-y](/reference/cssproperties/css_prop_background-position-y) - Set vertical position only
- [background-image](/reference/cssproperties/css_prop_background-image) - Set background image
- [background-repeat](/reference/cssproperties/css_prop_background-repeat) - Control image repetition
- [background-size](/reference/cssproperties/css_prop_background-size) - Control image sizing

---
