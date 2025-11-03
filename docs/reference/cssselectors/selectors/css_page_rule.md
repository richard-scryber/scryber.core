---
layout: default
title: @page Rule
parent: CSS Selectors
parent_url: /reference/cssselectors/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @page At-Rule

The `@page` at-rule allows you to define styles specific to printed pages, including page margins, size, orientation, and other page-specific properties. This is particularly important in PDF generation with Scryber.

## Usage

```css
@page {
    property: value;
}
```

Applies styles to the page container itself, not the content within the page.

---

## Syntax Examples

```css
/* Basic page setup */
@page {
    size: A4;
    margin: 20mm;
}

/* Portrait orientation */
@page {
    size: A4 portrait;
}

/* Landscape orientation */
@page {
    size: A4 landscape;
}

/* Custom page size */
@page {
    size: 8.5in 11in;
    margin: 1in;
}

/* Different margins */
@page {
    margin-top: 30mm;
    margin-bottom: 20mm;
    margin-left: 25mm;
    margin-right: 25mm;
}
```

---

## Common Page Properties

- `size` - Page dimensions (A4, Letter, Legal, or custom dimensions)
- `margin` - Page margins (shorthand)
- `margin-top`, `margin-right`, `margin-bottom`, `margin-left` - Individual margins
- `orientation` - Page orientation (portrait or landscape)

---

## Standard Page Sizes

- `A4` - 210mm × 297mm
- `A3` - 297mm × 420mm
- `A5` - 148mm × 210mm
- `Letter` - 8.5in × 11in
- `Legal` - 8.5in × 14in
- `Tabloid` - 11in × 17in

---

## Notes

- The @page rule defines properties of the page box itself
- Page margins define the printable area within the page
- Size can be specified using standard names or custom dimensions
- Orientation can be portrait (default) or landscape
- Multiple @page rules can be defined for different sections
- Particularly important for PDF generation in Scryber

---

## Examples

### Example 1: Basic A4 page setup

```html
<style>
    @page {
        size: A4;
        margin: 25mm;
    }
</style>
<body>
    <h1>Document Title</h1>
    <p>Content with A4 page size and 25mm margins.</p>
</body>
```

### Example 2: Letter size with custom margins

```html
<style>
    @page {
        size: Letter;
        margin-top: 1in;
        margin-bottom: 1in;
        margin-left: 0.75in;
        margin-right: 0.75in;
    }
</style>
<body>
    <h1>US Letter Format</h1>
    <p>Document formatted for US Letter size paper.</p>
</body>
```

### Example 3: Landscape orientation

```html
<style>
    @page {
        size: A4 landscape;
        margin: 20mm 30mm;
    }
</style>
<body>
    <h1>Landscape Document</h1>
    <p>This page is in landscape orientation.</p>
</body>
```

### Example 4: Custom page size

```html
<style>
    @page {
        size: 8in 10in;
        margin: 0.5in;
    }
</style>
<body>
    <h1>Custom Size</h1>
    <p>Document with custom page dimensions.</p>
</body>
```

### Example 5: Minimal margins for full bleed

```html
<style>
    @page {
        size: A4;
        margin: 0;
    }

    body {
        margin: 0;
        padding: 0;
    }
</style>
<body>
    <div style="background-color: #0066cc; width: 100%; height: 100%;">
        <p style="color: white; padding: 20pt;">Full bleed background</p>
    </div>
</body>
```

### Example 6: Report format

```html
<style>
    @page {
        size: Letter portrait;
        margin-top: 0.75in;
        margin-bottom: 0.75in;
        margin-left: 1in;
        margin-right: 1in;
    }

    body {
        font-family: "Times New Roman", serif;
        font-size: 12pt;
    }
</style>
<body>
    <h1>Business Report</h1>
    <p>Professional document formatting.</p>
</body>
```

### Example 7: Brochure format

```html
<style>
    @page {
        size: A4 landscape;
        margin: 15mm;
    }

    body {
        column-count: 3;
        column-gap: 10mm;
    }
</style>
<body>
    <h1>Product Brochure</h1>
    <p>Three-column layout in landscape orientation.</p>
</body>
```

### Example 8: Legal document

```html
<style>
    @page {
        size: Legal;
        margin: 1in 1.25in;
    }

    body {
        font-family: "Courier New", monospace;
        font-size: 12pt;
        line-height: 2;
    }
</style>
<body>
    <h1>Legal Document</h1>
    <p>Double-spaced legal format.</p>
</body>
```

### Example 9: Invoice format

```html
<style>
    @page {
        size: A4;
        margin-top: 40mm;
        margin-bottom: 30mm;
        margin-left: 20mm;
        margin-right: 20mm;
    }
</style>
<body>
    <h1>Invoice #12345</h1>
    <p>Extra top margin for letterhead.</p>
</body>
```

### Example 10: Multi-page document with consistent formatting

```html
<style>
    @page {
        size: A4 portrait;
        margin: 25mm 20mm;
    }

    body {
        font-family: Arial, sans-serif;
        font-size: 11pt;
        line-height: 1.5;
    }

    h1 {
        font-size: 24pt;
        margin-bottom: 10mm;
    }

    h2 {
        font-size: 18pt;
        margin-top: 8mm;
        margin-bottom: 6mm;
    }
</style>
<body>
    <h1>Document Title</h1>
    <h2>Section 1</h2>
    <p>Content for section 1...</p>
    <h2>Section 2</h2>
    <p>Content for section 2...</p>
</body>
```

---

## See Also

- [@media Rule](/reference/cssselectors/css_media_rule)
- [Page Size Configuration](/guides/page_setup)
- [Element Selector](/reference/cssselectors/css_element_selector)
- [PDF Generation Pipeline](/architecture/generation_pipeline)

---
