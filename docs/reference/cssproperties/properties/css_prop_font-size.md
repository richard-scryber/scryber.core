---
layout: default
title: font-size
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# font-size : The Font Size Property

Summary

The `font-size` property sets the size of text in PDF documents. It accepts absolute length values (points, pixels, etc.) or predefined keyword values that map to specific point sizes. Font size is fundamental to text rendering and affects the visual hierarchy and readability of PDF content.

## Usage

```css
/* Absolute units */
font-size: 12pt;
font-size: 16px;
font-size: 1.2em;
font-size: 2rem;

/* Keyword values */
font-size: medium;
font-size: large;
font-size: x-small;
```

---

## Values

### Predefined Keywords

- **xx-small** - 6pt - Extra extra small text
- **x-small** - 8pt - Extra small text
- **small** - 10pt - Small text
- **medium** - 12pt - Medium/normal text (default)
- **large** - 16pt - Large text
- **x-large** - 24pt - Extra large text
- **xx-large** - 32pt - Extra extra large text

### Absolute Units

- **pt (points)** - Standard typographic unit (1pt = 1/72 inch): `12pt`, `14pt`
- **px (pixels)** - Pixel units: `16px`, `20px`
- **in (inches)** - Inches: `0.5in`
- **cm (centimeters)** - Centimeters: `1cm`
- **mm (millimeters)** - Millimeters: `10mm`

### Relative Units

- **em** - Relative to parent element's font size: `1.5em`, `0.8em`
- **rem** - Relative to root element's font size: `1.2rem`
- **% (percentage)** - Percentage of parent's font size: `120%`, `80%`

### Expression Support

- **calc()** - Calculate font size: `calc(12pt + 2pt)`
- **var()** - Use CSS variables: `var(--base-font-size)`

### Unsupported Values

- **larger** - Recognized but not applied
- **smaller** - Recognized but not applied

---

## Notes

- Points (pt) are the recommended unit for PDF generation as they map directly to print measurements
- The default font size is typically 12pt (medium)
- Relative units (em, rem, %) are calculated based on parent or root font sizes
- Keywords provide consistent sizing across documents
- Font size affects line height calculations when using relative line-height values
- Very small font sizes (below 6pt) may reduce readability in printed PDFs
- Expression binding is supported for dynamic font sizing

---

## Data Binding

CSS properties support dynamic values through Scryber's Handlebars-style data binding syntax using `{{expression}}` in inline styles. This enables font-size to be determined dynamically based on model data, user preferences, or document requirements.

### Binding Syntax

Data binding expressions are enclosed in double curly braces `{{}}` and can reference:
- Model properties: `{{model.propertyName}}`
- Nested data: `{{customer.preferences.fontSize}}`
- Conditional expressions: `{{condition ? valueIfTrue : valueIfFalse}}`

### Data Binding Examples

```html
<!-- Dynamic font size from model -->
<p style="font-size: {{customer.preferredFontSize}}pt">
    Text sized according to customer preference
</p>

<!-- Conditional sizing based on importance -->
<div style="font-size: {{isImportant ? '16pt' : '12pt'}}">
    {{message}}
</div>

<!-- Responsive sizing for different document sections -->
<body>
    <h1 style="font-size: {{styles.heading1Size}}pt">
        {{title}}
    </h1>
    <h2 style="font-size: {{styles.heading2Size}}pt">
        {{subtitle}}
    </h2>
    <p style="font-size: {{styles.bodySize}}pt">
        {{content}}
    </p>

    <!-- Size based on document type -->
    <div style="font-size: {{documentType == 'invoice' ? '10pt' : '11pt'}}">
        Standard body text with type-specific sizing
    </div>
</body>
```

**Note:** Bound font-size values should include units (e.g., 'pt', 'px'). Ensure the bound data provides valid CSS length values to avoid rendering issues.

---

## Examples

### Example 1: Standard Point Size

```html
<p style="font-size: 12pt">
    Standard body text at 12 points
</p>
```

### Example 2: Keyword Size

```html
<div style="font-size: medium">
    Medium-sized text (12pt)
</div>
```

### Example 3: Large Heading

```html
<h1 style="font-size: 24pt">
    Document Title
</h1>
```

### Example 4: Extra Large Keyword

```html
<div style="font-size: x-large">
    Extra large heading text (24pt)
</div>
```

### Example 5: Small Print

```html
<span style="font-size: xx-small">
    Fine print disclaimer text (6pt)
</span>
```

### Example 6: Pixel-Based Size

```html
<div style="font-size: 16px">
    Text sized in pixels
</div>
```

### Example 7: Relative Em Size

```html
<p style="font-size: 12pt">
    Parent text at 12pt
    <span style="font-size: 1.5em">larger nested text (18pt)</span>
</p>
```

### Example 8: Root Em Size

```html
<body style="font-size: 12pt">
    <div style="font-size: 1.5rem">
        Text at 1.5 times root size (18pt)
    </div>
</body>
```

### Example 9: Percentage Size

```html
<div style="font-size: 16pt">
    <span style="font-size: 75%">Smaller text (12pt)</span>
</div>
```

### Example 10: Invoice Header Sizes

```html
<div style="font-size: xx-large">INVOICE</div>
<div style="font-size: large">#INV-2024-001</div>
<div style="font-size: medium">Date: January 1, 2024</div>
```

### Example 11: Table Font Sizes

```html
<table>
    <thead style="font-size: 10pt">
        <tr><th>Item</th><th>Price</th></tr>
    </thead>
    <tbody style="font-size: 9pt">
        <tr><td>Widget</td><td>$10.00</td></tr>
    </tbody>
</table>
```

### Example 12: Hierarchical Sizes

```html
<h1 style="font-size: 20pt">Main Title</h1>
<h2 style="font-size: 16pt">Subtitle</h2>
<h3 style="font-size: 14pt">Section Heading</h3>
<p style="font-size: 11pt">Body text content</p>
```

### Example 13: Report Cover Page

```html
<div style="font-size: 32pt; font-weight: bold">
    ANNUAL REPORT
</div>
<div style="font-size: 24pt">
    Financial Year 2024
</div>
<div style="font-size: 14pt">
    Prepared by Finance Department
</div>
```

### Example 14: Form Labels and Values

```html
<div>
    <label style="font-size: small">Customer Name:</label>
    <span style="font-size: medium">John Smith</span>
</div>
```

### Example 15: Dynamic PDF Content

```html
<div style="font-size: 12pt">
    <h2 style="font-size: 1.5em">Section Title (18pt)</h2>
    <p>
        This document demonstrates various font sizes suitable for
        PDF generation. The base size is 12pt with headings scaled
        proportionally using em units.
    </p>
    <p style="font-size: 0.9em">
        Slightly smaller supplementary text (10.8pt) for additional
        information that should be readable but less prominent.
    </p>
    <footer style="font-size: x-small">
        Page footer with small text (8pt)
    </footer>
</div>
```

---

## See Also

- [font](/reference/cssproperties/font) - Shorthand font property
- [font-family](/reference/cssproperties/font-family) - Font family specification
- [line-height](/reference/cssproperties/line-height) - Line spacing
- [font-weight](/reference/cssproperties/font-weight) - Font weight values
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
