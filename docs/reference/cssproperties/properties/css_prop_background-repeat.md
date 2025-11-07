---
layout: default
title: background-repeat
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# background-repeat : Background Repeat Property

The `background-repeat` property controls how background images are repeated (tiled) within an element in PDF documents. This property is essential for creating patterns, borders, and controlling single-instance image display.

## Usage

```css
selector {
    background-repeat: value;
}
```

The background-repeat property determines whether and how a background image repeats to fill the background area of an element.

---

## Supported Values

### Single Keywords
- `repeat` - Image repeats both horizontally and vertically (default)
- `repeat-x` - Image repeats only horizontally
- `repeat-y` - Image repeats only vertically
- `no-repeat` - Image displays once, no repetition
- `space` - Image repeats with equal spacing between instances to fill area
- `round` - Image repeats and scales to fit an integer number of times

### Two-Value Syntax
- First value: horizontal repetition behavior
- Second value: vertical repetition behavior
- Example: `repeat-x no-repeat` (same as `repeat-x`)
- Example: `no-repeat repeat` (repeat vertically only)

---

## Supported Elements

The `background-repeat` property can be applied to:
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
- Default value is `repeat` (tiles in both directions)
- Use `no-repeat` for logos, watermarks, and single-instance images
- Use `repeat-x` for horizontal borders and patterns
- Use `repeat-y` for vertical borders and sidebars
- The `space` value distributes images evenly with gaps
- The `round` value may stretch images to fit exact repetitions
- Combine with `background-position` to control starting position
- Does not affect `background-color`
- Particularly useful for texture patterns and decorative elements

---

## Data Binding

The `background-repeat` property supports dynamic data binding, allowing you to control image repetition patterns based on data for customized document layouts and branding.

### Example 1: Dynamic pattern selection

```html
<style>
    .branded-section {
        background-image: url('{{brand.patternUrl}}');
        background-repeat: {{brand.repeatStyle}};
        background-color: {{brand.backgroundColor}};
        padding: 25pt;
    }
</style>
<body>
    <div class="branded-section">
        <h2>{{brand.companyName}}</h2>
        <p>{{brand.tagline}}</p>
    </div>
</body>
```

Enables different organizations to use their own background patterns with custom repeat settings (repeat, repeat-x, repeat-y, no-repeat) - perfect for white-label document generation where each client has unique branding requirements.

### Example 2: Conditional repeating borders based on document type

```html
<style>
    .document-header {
        background-image: url('{{document.borderImageUrl}}');
        background-repeat: {{#if document.type == 'certificate'}}no-repeat{{else if document.type == 'letterhead'}}repeat-x{{else}}repeat{{/if}};
        background-position: {{#if document.type == 'certificate'}}center{{else if document.type == 'letterhead'}}top{{else}}top left{{/if}};
        padding: 30pt;
        min-height: {{document.headerHeight}};
    }
</style>
<body>
    <div class="document-header">
        <h1>{{document.title}}</h1>
    </div>
</body>
```

Automatically adjusts background repetition based on document type - certificates get single centered images, letterheads get horizontal borders, while general documents can have full repeating patterns.

### Example 3: Data-driven security patterns

```html
<style>
    .security-document {
        background-image: url('{{security.patternUrl}}');
        background-repeat: {{#if security.level >= 3}}repeat{{else}}no-repeat{{/if}};
        background-size: {{security.patternSize}};
        padding: 30pt;
    }
    .content-overlay {
        background-color: rgba(255, 255, 255, {{security.overlayOpacity}});
        padding: 20pt;
    }
</style>
<body>
    <div class="security-document">
        <div class="content-overlay">
            <h2>Security Level {{security.level}}: {{security.classification}}</h2>
            <p>{{document.content}}</p>
        </div>
    </div>
</body>
```

Creates security-enhanced documents where high-security items automatically get full repeating microprint patterns, while lower security levels may use simpler watermarks. Essential for compliance and document authentication.

---

## Examples

### Example 1: No repetition (single image)

```html
<style>
    .logo-header {
        background-image: url('images/company-logo.png');
        background-repeat: no-repeat;
        background-position: center;
        min-height: 100pt;
        padding: 20pt;
    }
</style>
<body>
    <div class="logo-header">
        <h1 style="text-align: center;">Company Report</h1>
    </div>
</body>
```

### Example 2: Repeating pattern

```html
<style>
    .patterned-background {
        background-image: url('images/subtle-pattern.png');
        background-repeat: repeat;
        padding: 25pt;
    }
</style>
<body>
    <div class="patterned-background">
        <h2>Textured Section</h2>
        <p>This section has a repeating pattern background.</p>
    </div>
</body>
```

### Example 3: Horizontal repeat for top border

```html
<style>
    .decorative-header {
        background-image: url('images/border-pattern.png');
        background-repeat: repeat-x;
        background-position: top;
        padding-top: 30pt;
        padding-left: 20pt;
        padding-right: 20pt;
    }
</style>
<body>
    <div class="decorative-header">
        <h1>Elegant Document Header</h1>
        <p>With decorative top border</p>
    </div>
</body>
```

### Example 4: Vertical repeat for sidebar

```html
<style>
    .sidebar {
        background-image: url('images/vertical-accent.png');
        background-repeat: repeat-y;
        background-position: left;
        padding-left: 30pt;
        min-height: 400pt;
    }
</style>
<body>
    <div class="sidebar">
        <h3>Navigation</h3>
        <p>Menu item 1</p>
        <p>Menu item 2</p>
        <p>Menu item 3</p>
    </div>
</body>
```

### Example 5: Certificate with single seal

```html
<style>
    .certificate {
        background-image: url('images/official-seal.png');
        background-repeat: no-repeat;
        background-position: bottom right;
        padding: 40pt;
        padding-bottom: 100pt;
        min-height: 500pt;
    }
    .cert-title {
        text-align: center;
        font-size: 28pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">Certificate of Completion</h1>
        <p style="text-align: center; margin-top: 40pt;">This certifies that</p>
        <p style="text-align: center; font-size: 22pt; font-weight: bold;">David Martinez</p>
        <p style="text-align: center;">has successfully completed the program</p>
    </div>
</body>
```

### Example 6: Watermark (no repeat)

```html
<style>
    .watermarked-page {
        background-image: url('images/confidential-watermark.png');
        background-repeat: no-repeat;
        background-position: center;
        background-size: 350pt 100pt;
        min-height: 700pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="watermarked-page">
        <h1>Confidential Document</h1>
        <p>This document contains sensitive information.</p>
        <p>Please handle with appropriate security measures.</p>
    </div>
</body>
```

### Example 7: Bottom border with repeat-x

```html
<style>
    .footer-border {
        background-image: url('images/decorative-line.png');
        background-repeat: repeat-x;
        background-position: bottom;
        padding: 20pt;
        padding-bottom: 35pt;
    }
</style>
<body>
    <div class="footer-border">
        <p>Document content that ends with a decorative border.</p>
        <p>© 2025 Company Name</p>
    </div>
</body>
```

### Example 8: Tiled texture background

```html
<style>
    .paper-texture {
        background-image: url('images/paper-tile.jpg');
        background-repeat: repeat;
        padding: 30pt;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="paper-texture">
        <h2>Document with Paper Texture</h2>
        <p>The background creates a paper-like appearance.</p>
    </div>
</body>
```

### Example 9: Left accent bar with repeat-y

```html
<style>
    .accent-section {
        background-image: url('images/left-accent.png');
        background-repeat: repeat-y;
        background-position: left center;
        padding-left: 25pt;
        min-height: 200pt;
    }
    .section-content {
        border-left: 3pt solid #3b82f6;
        padding-left: 15pt;
    }
</style>
<body>
    <div class="accent-section">
        <div class="section-content">
            <h3>Important Information</h3>
            <p>This section is highlighted with a vertical accent.</p>
        </div>
    </div>
</body>
```

### Example 10: Invoice letterhead (no repeat)

```html
<style>
    .invoice-letterhead {
        background-image: url('images/invoice-header.png');
        background-repeat: no-repeat;
        background-position: top center;
        padding-top: 120pt;
        padding-left: 30pt;
        padding-right: 30pt;
    }
</style>
<body>
    <div class="invoice-letterhead">
        <h1 style="font-size: 28pt;">INVOICE</h1>
        <p>Invoice #: INV-2025-100</p>
        <p>Date: January 15, 2025</p>
    </div>
</body>
```

### Example 11: Repeating dot pattern

```html
<style>
    .dotted-background {
        background-image: url('images/dots-pattern.png');
        background-repeat: repeat;
        background-color: #f9fafb;
        padding: 25pt;
    }
</style>
<body>
    <div class="dotted-background">
        <h3>Section with Dot Pattern</h3>
        <p>Subtle repeating dots create visual interest.</p>
    </div>
</body>
```

### Example 12: Security pattern (repeat)

```html
<style>
    .secure-document {
        background-image: url('images/security-microprint.png');
        background-repeat: repeat;
        padding: 30pt;
    }
    .content-box {
        background-color: rgba(255, 255, 255, 0.95);
        padding: 20pt;
    }
</style>
<body>
    <div class="secure-document">
        <div class="content-box">
            <h2>Secure Document</h2>
            <p>Background security pattern prevents unauthorized copying.</p>
        </div>
    </div>
</body>
```

### Example 13: Top and bottom borders

```html
<style>
    .bordered-section {
        background-image: url('images/top-border.png'), url('images/bottom-border.png');
        background-repeat: repeat-x, repeat-x;
        background-position: top, bottom;
        padding: 40pt 20pt;
        min-height: 300pt;
    }
</style>
<body>
    <div class="bordered-section">
        <h2>Framed Content</h2>
        <p>This section has decorative borders at top and bottom.</p>
    </div>
</body>
```

### Example 14: Corner decoration (no repeat)

```html
<style>
    .corner-decorated {
        background-image: url('images/corner-ornament.png');
        background-repeat: no-repeat;
        background-position: top left;
        padding: 40pt;
        padding-top: 60pt;
        padding-left: 60pt;
    }
</style>
<body>
    <div class="corner-decorated">
        <h2>Elegant Section</h2>
        <p>Decorative corner ornament adds sophistication.</p>
    </div>
</body>
```

### Example 15: Repeated stamp pattern

```html
<style>
    .stamped-background {
        background-image: url('images/approved-stamp.png');
        background-repeat: repeat;
        background-size: 100pt 100pt;
        padding: 30pt;
        opacity: 0.95;
    }
    .foreground-content {
        background-color: rgba(255, 255, 255, 0.9);
        padding: 20pt;
    }
</style>
<body>
    <div class="stamped-background">
        <div class="foreground-content">
            <h2>Approved Document</h2>
            <p>Multiple approval stamps in background.</p>
        </div>
    </div>
</body>
```

---

## See Also

- [background](/reference/cssproperties/css_prop_background) - Shorthand for all background properties
- [background-image](/reference/cssproperties/css_prop_background-image) - Set background image
- [background-position](/reference/cssproperties/css_prop_background-position) - Set image position
- [background-size](/reference/cssproperties/css_prop_background-size) - Control image sizing
- [background-color](/reference/cssproperties/css_prop_background-color) - Set background color

---
