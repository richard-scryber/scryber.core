---
layout: default
title: font-display
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# font-display : The Font Display Property

Summary

The `font-display` property controls how font files are displayed based on whether and when they are downloaded and ready to use. This property is currently recognized by Scryber.Core but not applied during PDF rendering. It is included for CSS compatibility with web standards.

## Usage

```css
/* Keyword values */
font-display: auto;
font-display: block;
font-display: swap;
font-display: fallback;
font-display: optional;

/* Typically used in @font-face rules */
@font-face {
    font-family: 'CustomFont';
    src: url('custom.woff2');
    font-display: swap;
}
```

---

## Values

### Keyword Values

- **auto** - Browser/system determines font display strategy (default)
- **block** - Block text rendering until font loads (short block period, infinite swap)
- **swap** - Show fallback immediately, swap to custom font when available
- **fallback** - Very short block period, short swap period
- **optional** - Very short block period, no swap period

---

## Notes

- This property is currently **not implemented** in Scryber.Core
- The CSS parser recognizes the property but skips to the next attribute without applying changes
- No behavioral effect will be seen when this property is applied in PDF generation
- In web contexts, `font-display` controls the Font Loading API behavior
- PDF generation typically loads all fonts before rendering, making this property less relevant
- Included for CSS compatibility when using shared stylesheets between web and PDF
- The property is primarily relevant for `@font-face` declarations in web development
- Future versions may implement font loading strategies if asynchronous font loading is added

---

## Data Binding

CSS properties support dynamic values through Scryber's Handlebars-style data binding syntax using `{{expression}}` in inline styles. While font-display is not currently implemented in Scryber.Core (as it's primarily a web-focused property), the data binding syntax is supported for shared web/PDF stylesheets.

### Binding Syntax

Data binding expressions are enclosed in double curly braces `{{}}` and can reference:
- Model properties: `{{model.propertyName}}`
- Nested data: `{{config.fontDisplay}}`
- Conditional expressions: `{{condition ? valueIfTrue : valueIfFalse}}`

### Data Binding Examples

```html
<!-- Dynamic font-display from configuration (no effect in PDF) -->
<style>
    @font-face {
        font-family: 'DynamicFont';
        src: url('{{fontConfig.url}}');
        font-display: {{fontConfig.displayMode}};
    }
</style>

<!-- Conditional display strategy (no effect in PDF) -->
<div style="font-display: {{isProduction ? 'swap' : 'block'}}">
    Configuration-aware font display
</div>

<!-- Shared web/PDF stylesheet with binding -->
<body>
    <style>
        /* Binding in @font-face for web use */
        @font-face {
            font-family: '{{branding.fontName}}';
            src: url('{{branding.fontUrl}}');
            font-display: {{fontLoadingStrategy}};
        }

        body {
            font-family: '{{branding.fontName}}', Arial, sans-serif;
        }
    </style>

    <p style="font-display: {{displayMode}}">
        Content with bound font-display property
    </p>

    <!-- Environment-specific configuration -->
    <div style="font-display: {{environment == 'web' ? 'optional' : 'auto'}}">
        {{content}}
    </div>
</body>
```

**Note:** Font-display is not applied during PDF rendering in Scryber.Core. These data binding examples are useful for shared stylesheets that serve both web and PDF contexts, where the property applies to web rendering only.

---

## Examples

### Example 1: Auto Display (No Effect)

```html
<style>
    @font-face {
        font-family: 'CustomFont';
        src: url('custom.ttf');
        font-display: auto;
    }
</style>
```

### Example 2: Swap Display (No Effect)

```html
<style>
    @font-face {
        font-family: 'WebFont';
        src: url('webfont.woff2');
        font-display: swap;
    }
    .custom-text {
        font-family: 'WebFont', Arial, sans-serif;
    }
</style>
<p class="custom-text">Text with swap font display</p>
```

### Example 3: Block Display (No Effect)

```html
<style>
    @font-face {
        font-family: 'DisplayFont';
        src: url('display.woff2');
        font-display: block;
    }
</style>
```

### Example 4: Fallback Display (No Effect)

```html
<style>
    @font-face {
        font-family: 'FallbackFont';
        src: url('fallback.woff2');
        font-display: fallback;
    }
</style>
```

### Example 5: Optional Display (No Effect)

```html
<style>
    @font-face {
        font-family: 'OptionalFont';
        src: url('optional.woff2');
        font-display: optional;
    }
</style>
```

### Example 6: Inline Style (No Effect)

```html
<div style="font-display: swap; font-family: Arial">
    Text with font-display in inline style (not applied)
</div>
```

### Example 7: Multiple Font Faces (No Effect)

```html
<style>
    @font-face {
        font-family: 'HeadingFont';
        src: url('heading-regular.woff2');
        font-weight: 400;
        font-display: swap;
    }
    @font-face {
        font-family: 'HeadingFont';
        src: url('heading-bold.woff2');
        font-weight: 700;
        font-display: swap;
    }
</style>
```

### Example 8: Web-PDF Shared Stylesheet (No Effect in PDF)

```html
<style>
    /* Shared stylesheet for web and PDF */
    @font-face {
        font-family: 'BrandFont';
        src: url('brand-font.woff2') format('woff2'),
             url('brand-font.ttf') format('truetype');
        font-display: swap; /* Applies to web, ignored in PDF */
    }

    body {
        font-family: 'BrandFont', Arial, sans-serif;
    }
</style>
```

### Example 9: Performance Optimization (Web Only)

```html
<style>
    @font-face {
        font-family: 'FastFont';
        src: url('fast-font.woff2');
        font-display: optional; /* Prioritize performance over custom font */
    }
</style>
```

### Example 10: Critical Content Font (No Effect)

```html
<style>
    @font-face {
        font-family: 'CriticalFont';
        src: url('critical.woff2');
        font-display: block; /* Wait for font to load */
    }
    .critical-heading {
        font-family: 'CriticalFont', serif;
    }
</style>
<h1 class="critical-heading">Important Heading</h1>
```

### Example 11: Invoice Branding (No Effect)

```html
<style>
    @font-face {
        font-family: 'InvoiceFont';
        src: url('invoice-font.woff2');
        font-display: swap;
    }
    .invoice-header {
        font-family: 'InvoiceFont', 'Helvetica', sans-serif;
        font-size: 24pt;
    }
</style>
<div class="invoice-header">INVOICE</div>
```

### Example 12: Report Custom Typography (No Effect)

```html
<style>
    @font-face {
        font-family: 'ReportSerif';
        src: url('report-serif.woff2');
        font-display: fallback;
    }
    .report-body {
        font-family: 'ReportSerif', Georgia, serif;
    }
</style>
```

### Example 13: Logo Font (No Effect)

```html
<style>
    @font-face {
        font-family: 'LogoFont';
        src: url('logo.woff2');
        font-display: block; /* Ensure logo font always displays */
    }
    .company-logo-text {
        font-family: 'LogoFont', sans-serif;
    }
</style>
<div class="company-logo-text">ACME Corp</div>
```

### Example 14: Decorative Text (No Effect)

```html
<style>
    @font-face {
        font-family: 'DecorativeFont';
        src: url('decorative.woff2');
        font-display: optional; /* Not critical, skip if slow */
    }
    .decorative {
        font-family: 'DecorativeFont', cursive;
    }
</style>
```

### Example 15: Complete Document with Font Display

```html
<!-- CSS prepared for web use, with PDF compatibility -->
<style>
    @font-face {
        font-family: 'DocumentFont';
        src: url('document-regular.woff2') format('woff2'),
             url('document-regular.ttf') format('truetype');
        font-weight: 400;
        font-style: normal;
        font-display: swap;
    }

    @font-face {
        font-family: 'DocumentFont';
        src: url('document-bold.woff2') format('woff2'),
             url('document-bold.ttf') format('truetype');
        font-weight: 700;
        font-style: normal;
        font-display: swap;
    }

    body {
        font-family: 'DocumentFont', Arial, sans-serif;
        font-size: 11pt;
    }

    h1, h2 {
        font-weight: 700;
    }
</style>

<body>
    <h1>Professional Document</h1>
    <p>
        This document uses custom web fonts with font-display: swap
        for optimal web performance. In PDF generation via Scryber.Core,
        the font-display property is recognized but not applied, as
        fonts are typically loaded synchronously before rendering.
    </p>
    <p>
        The fallback font stack ensures that content remains readable
        whether custom fonts load successfully or not.
    </p>
</body>
```

---

## See Also

- [font](/reference/cssproperties/font) - Shorthand font property
- [font-family](/reference/cssproperties/font-family) - Font family specification
- [font-style](/reference/cssproperties/font-style) - Italic and oblique styles
- [font-weight](/reference/cssproperties/font-weight) - Font weight values
- [font-size](/reference/cssproperties/font-size) - Font size specification
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
