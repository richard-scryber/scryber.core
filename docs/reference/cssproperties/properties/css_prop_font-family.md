---
layout: default
title: font-family
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# font-family : The Font Family Property

Summary

The `font-family` property specifies the typeface to be used for rendering text in PDF documents. It accepts a prioritized list of font family names, allowing for fallback options when the primary font is not available. Font names can be specified with or without quotes.

## Usage

```css
/* Single font family */
font-family: Arial;
font-family: 'Times New Roman';

/* Multiple font families with fallbacks */
font-family: Helvetica, Arial, sans-serif;
font-family: 'Georgia', 'Times New Roman', Times, serif;

/* Generic font families */
font-family: serif;
font-family: sans-serif;
font-family: monospace;
```

---

## Values

### Specific Font Family Names

- **Quoted names** - Font names with spaces must be quoted: `'Times New Roman'`, `"Helvetica Neue"`
- **Unquoted names** - Single-word font names can be unquoted: `Arial`, `Georgia`, `Verdana`
- **Multiple families** - Comma-separated list providing fallback options

### Generic Font Families

- **serif** - Fonts with serifs (e.g., Times, Georgia)
- **sans-serif** - Fonts without serifs (e.g., Arial, Helvetica)
- **monospace** - Fixed-width fonts (e.g., Courier, Consolas)
- **cursive** - Handwriting-style fonts
- **fantasy** - Decorative fonts

### Expression Support

- **calc()** - Can use expressions, but not within part of a font selector
- **var()** - Can use CSS variables, but not within part of a font selector

---

## Notes

- Font names are case-insensitive but convention uses proper case
- Trailing commas are automatically removed from font names
- Quotes (single or double) are automatically removed when processing
- The first available font in the list will be used for rendering
- Generic font families (serif, sans-serif, monospace) provide system-dependent fallbacks
- For PDF generation, ensure specified fonts are available on the system
- Expressions must encompass the entire font-family value, not individual font names

---

## Data Binding

CSS properties support dynamic values through Scryber's Handlebars-style data binding syntax using `{{expression}}` in inline styles. This enables font-family to be determined dynamically based on model data, user preferences, or document context.

### Binding Syntax

Data binding expressions are enclosed in double curly braces `{{}}` and can reference:
- Model properties: `{{model.propertyName}}`
- Nested data: `{{customer.preferences.fontFamily}}`
- Conditional expressions: `{{condition ? valueIfTrue : valueIfFalse}}`

### Data Binding Examples

```html
<!-- Dynamic font family from model -->
<div style="font-family: {{document.fontFamily}}">
    Content using dynamically selected font family
</div>

<!-- Conditional font based on document type -->
<div style="font-family: {{isReport ? 'Georgia, serif' : 'Arial, sans-serif'}}">
    Font family selected based on document type
</div>

<!-- User-specific font preferences -->
<body>
    <p style="font-family: {{user.preferredFont}}, Arial, sans-serif">
        Text using customer's preferred font with fallbacks
    </p>

    <!-- Different fonts for different sections -->
    <h1 style="font-family: {{branding.headingFont}}">
        {{reportTitle}}
    </h1>
    <div style="font-family: {{branding.bodyFont}}">
        {{reportContent}}
    </div>
</body>
```

**Note:** Ensure that font names from bound data match fonts available on the PDF generation system. Always include fallback fonts in the bound value or after the binding expression.

---

## Examples

### Example 1: Single Sans-Serif Font

```html
<p style="font-family: Arial">
    Simple paragraph using Arial font
</p>
```

### Example 2: Font with Spaces

```html
<div style="font-family: 'Times New Roman'">
    Text requiring quoted font name due to spaces
</div>
```

### Example 3: Font Fallback Chain

```html
<p style="font-family: Helvetica, Arial, sans-serif">
    Text with multiple fallback options for cross-platform compatibility
</p>
```

### Example 4: Serif Font Stack

```html
<article style="font-family: Georgia, 'Times New Roman', Times, serif">
    Article content with serif font preferences
</article>
```

### Example 5: Monospace for Code

```html
<code style="font-family: Courier, 'Courier New', monospace">
    var x = 10;
</code>
```

### Example 6: Generic Serif Font

```html
<div style="font-family: serif">
    Text using system's default serif font
</div>
```

### Example 7: Modern Sans-Serif Stack

```html
<body style="font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Helvetica, Arial, sans-serif">
    Body text with system font preferences
</body>
```

### Example 8: Invoice Header Font

```html
<h1 style="font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif">
    INVOICE
</h1>
```

### Example 9: Table Data Font

```html
<table style="font-family: Verdana, Geneva, sans-serif">
    <tr><td>Product</td><td>Price</td></tr>
    <tr><td>Widget</td><td>$10.00</td></tr>
</table>
```

### Example 10: Report Title Font

```html
<div style="font-family: 'Arial Black', Gadget, sans-serif">
    ANNUAL REPORT 2024
</div>
```

### Example 11: Footer Font

```html
<footer style="font-family: Tahoma, Geneva, sans-serif">
    Copyright 2024. All rights reserved.
</footer>
```

### Example 12: Emphasis Font

```html
<strong style="font-family: 'Impact', Charcoal, sans-serif">
    IMPORTANT NOTICE
</strong>
```

### Example 13: Form Label Font

```html
<label style="font-family: Calibri, Candara, sans-serif">
    Customer Name:
</label>
```

### Example 14: Quote Block Font

```html
<blockquote style="font-family: Garamond, 'Hoefler Text', serif">
    "Quality is not an act, it is a habit."
</blockquote>
```

### Example 15: PDF Document Body

```html
<div style="font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif">
    <h2 style="font-family: 'Roboto', 'Helvetica Neue', Arial, sans-serif">
        Section Title
    </h2>
    <p>
        This PDF document uses modern web-safe fonts with appropriate
        fallbacks to ensure consistent rendering across different systems
        while maintaining professional appearance.
    </p>
</div>
```

---

## See Also

- [font](/reference/cssproperties/font) - Shorthand font property
- [font-size](/reference/cssproperties/font-size) - Font size specification
- [font-style](/reference/cssproperties/font-style) - Italic and oblique styles
- [font-weight](/reference/cssproperties/font-weight) - Font weight values
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
