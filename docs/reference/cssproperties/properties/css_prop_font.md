---
layout: default
title: font
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# font : The Shorthand Font Property

Summary

The `font` property is a shorthand CSS property that sets multiple font-related properties in a single declaration. It can set font-style, font-variant, font-weight, font-size, line-height, and font-family. Alternatively, it can set the font to a system font using predefined keywords.

## Usage

```css
/* System font keywords */
font: caption;
font: icon;
font: menu;
font: message-box;
font: small-caption;
font: status-bar;

/* Individual values */
font: [font-style] [font-variant] [font-weight] font-size [/line-height] font-family;

/* Examples */
font: italic bold 16pt 'Helvetica', sans-serif;
font: 12pt/1.5 'Times New Roman', serif;
font: normal 14pt Arial;
```

---

## Values

### System Font Keywords

- **caption** - Helvetica 12pt Bold - Font used for captioned controls
- **icon** - Helvetica 8pt Bold - Font used for icon labels
- **menu** - Times 10pt Regular - Font used in menus
- **message-box** - Times 10pt Bold - Font used in dialog boxes
- **small-caption** - Helvetica 8pt Italic - Font used for labeling small controls
- **status-bar** - Courier 10pt Bold - Font used in window status bars

### Individual Values

- **font-style** - Optional: `normal`, `italic`, `oblique`
- **font-variant** - Optional: `normal`, `small-caps` (recognized but not applied)
- **font-weight** - Optional: `normal`, `bold`, `100`-`900`, etc.
- **font-size** - Required: Absolute size (e.g., `12pt`, `16px`) or keyword (e.g., `medium`, `large`)
- **line-height** - Optional: Number (relative to font-size) or absolute unit. Follows font-size with `/` separator
- **font-family** - Required: One or more font family names, comma-separated

---

## Notes

- The `font` property is a shorthand that resets all font-related properties
- When using individual values, `font-size` and `font-family` are required
- The order of optional properties (`font-style`, `font-variant`, `font-weight`) matters and must appear before `font-size`
- Line-height must immediately follow font-size with a forward slash separator (e.g., `12pt/1.5`)
- Expressions (calc(), var()) are not currently supported in the compound font property
- System font keywords provide quick access to predefined font configurations for PDF generation

---

## Data Binding

CSS properties support dynamic values through Scryber's Handlebars-style data binding syntax using `{{expression}}` in inline styles. This enables font properties to be set based on model data, calculations, or conditional logic at PDF generation time.

### Binding Syntax

Data binding expressions are enclosed in double curly braces `{{}}` and can reference:
- Model properties: `{{model.propertyName}}`
- Nested data: `{{customer.preferences.fontConfig}}`
- Conditional expressions: `{{condition ? valueIfTrue : valueIfFalse}}`

### Data Binding Examples

```html
<!-- Dynamic font from model data -->
<div style="font: {{fontSize}}pt {{fontFamily}}">
    Text with font size and family from model
</div>

<!-- Conditional font based on document type -->
<h1 style="font: {{isInvoice ? 'bold 18pt Helvetica' : 'normal 16pt Arial'}}">
    {{documentTitle}}
</h1>

<!-- User preference-based font configuration -->
<body>
    <p style="font: {{user.preferredStyle}} {{user.preferredWeight}} {{user.preferredSize}}pt {{user.preferredFont}}">
        Content styled according to user preferences
    </p>
</body>
```

**Note:** The shorthand `font` property with data binding works best when the entire value is bound. Individual property values within the compound font declaration cannot be individually bound using `{{}}` syntax.

---

## Examples

### Example 1: Using System Font Keywords

```html
<div style="font: caption">
    This text uses the caption system font (Helvetica 12pt Bold)
</div>
```

### Example 2: Simple Font Declaration

```html
<p style="font: 14pt Arial">
    Standard paragraph with Arial 14pt font
</p>
```

### Example 3: Font with Style and Weight

```html
<div style="font: italic bold 16pt 'Times New Roman'">
    Italic and bold Times New Roman text
</div>
```

### Example 4: Font with Line Height

```html
<p style="font: 12pt/1.5 Georgia">
    Text with 12pt font size and 1.5x line height (18pt spacing)
</p>
```

### Example 5: Font with Absolute Line Height

```html
<div style="font: 14pt/20pt Helvetica">
    Text with 14pt font and 20pt absolute line height
</div>
```

### Example 6: Font Family Fallbacks

```html
<p style="font: 12pt 'Helvetica Neue', Helvetica, Arial, sans-serif">
    Text with multiple fallback fonts
</p>
```

### Example 7: Menu Style Font

```html
<div style="font: menu">
    Navigation item using menu system font
</div>
```

### Example 8: Message Box Font

```html
<div style="font: message-box">
    Dialog content using message-box system font
</div>
```

### Example 9: Complete Font Specification

```html
<h1 style="font: italic normal bold 24pt/28pt 'Garamond', serif">
    Fully specified heading with all font properties
</h1>
```

### Example 10: Status Bar Text

```html
<div style="font: status-bar">
    Footer text using status-bar monospace font
</div>
```

### Example 11: Small Caption for Labels

```html
<label style="font: small-caption">
    Form field label using small-caption font
</label>
```

### Example 12: Icon Label Font

```html
<span style="font: icon">
    Small icon label text
</span>
```

### Example 13: Normal Weight with Oblique Style

```html
<p style="font: oblique 400 13pt Verdana">
    Oblique text with numeric weight specification
</p>
```

### Example 14: Font in Invoice Header

```html
<div style="font: bold 18pt 'Helvetica', sans-serif">
    INVOICE #12345
</div>
```

### Example 15: Font in PDF Report Body

```html
<div style="font: normal 11pt/16pt 'Arial', sans-serif">
    Standard body text for PDF reports with comfortable line spacing
    that ensures good readability in generated documents.
</div>
```

---

## See Also

- [font-family](/reference/cssproperties/font-family) - Font family specification
- [font-size](/reference/cssproperties/font-size) - Font size values
- [font-style](/reference/cssproperties/font-style) - Italic and oblique styles
- [font-weight](/reference/cssproperties/font-weight) - Font weight specification
- [line-height](/reference/cssproperties/line-height) - Line height and leading
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
