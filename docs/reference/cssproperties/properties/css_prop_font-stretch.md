---
layout: default
title: font-stretch
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# font-stretch : The Font Stretch Property

Summary

The `font-stretch` property selects a normal, condensed, or expanded face from a font. This property is currently recognized by Scryber.Core but not applied during PDF rendering. It is included for CSS compatibility and future enhancement.

## Usage

```css
/* Keyword values */
font-stretch: normal;
font-stretch: ultra-condensed;
font-stretch: extra-condensed;
font-stretch: condensed;
font-stretch: semi-condensed;
font-stretch: semi-expanded;
font-stretch: expanded;
font-stretch: extra-expanded;
font-stretch: ultra-expanded;

/* Percentage values (CSS3) */
font-stretch: 50%;
font-stretch: 100%;
font-stretch: 200%;
```

---

## Values

### Keyword Values

- **ultra-condensed** - Most condensed variant (50%)
- **extra-condensed** - Extra condensed variant (62.5%)
- **condensed** - Condensed variant (75%)
- **semi-condensed** - Slightly condensed variant (87.5%)
- **normal** - Normal width (100%) - default
- **semi-expanded** - Slightly expanded variant (112.5%)
- **expanded** - Expanded variant (125%)
- **extra-expanded** - Extra expanded variant (150%)
- **ultra-expanded** - Most expanded variant (200%)

### Percentage Values

- **50% - 200%** - Numeric representation of stretch values
- **100%** - Equivalent to normal

---

## Notes

- This property is currently **not implemented** in Scryber.Core
- The CSS parser recognizes the property but skips to the next attribute without applying changes
- No visual effect will be seen when this property is applied to PDF elements
- Included for CSS compatibility and potential future implementation
- Standard CSS font-stretch requires fonts to have multiple width variants
- Most common fonts do not include condensed or expanded variants
- Future versions may implement this feature if font width variants become more widely available

---

## Data Binding

CSS properties support dynamic values through Scryber's Handlebars-style data binding syntax using `{{expression}}` in inline styles. While font-stretch is not currently implemented in Scryber.Core, the data binding syntax is supported for future compatibility.

### Binding Syntax

Data binding expressions are enclosed in double curly braces `{{}}` and can reference:
- Model properties: `{{model.propertyName}}`
- Nested data: `{{style.fontStretch}}`
- Conditional expressions: `{{condition ? valueIfTrue : valueIfFalse}}`

### Data Binding Examples

```html
<!-- Dynamic stretch from model (no effect until implemented) -->
<div style="font-stretch: {{textStyle.stretch}}">
    Text with bound font-stretch value
</div>

<!-- Conditional stretch based on layout (no effect until implemented) -->
<div style="font-stretch: {{isNarrowLayout ? 'condensed' : 'normal'}}">
    {{content}}
</div>

<!-- Future-ready styling with data binding -->
<body>
    <!-- Stretch based on space constraints -->
    <p style="font-stretch: {{availableWidth < 300 ? 'condensed' : 'normal'}}">
        Content that might use condensed fonts in narrow spaces
    </p>

    <!-- User preference binding -->
    <div style="font-stretch: {{user.preferredStretch}}">
        {{userContent}}
    </div>

    <!-- Layout-aware stretch values -->
    <div style="font-stretch: {{layout.fontStretchValue}}">
        Prepared for future font-stretch implementation
    </div>
</body>
```

**Note:** Font-stretch is currently not applied during PDF rendering. These data binding examples demonstrate the syntax for future compatibility when the property is implemented. The binding expressions will be processed but have no visual effect.

---

## Examples

### Example 1: Normal Stretch (No Effect)

```html
<p style="font-stretch: normal">
    This text uses normal font stretch (currently not applied)
</p>
```

### Example 2: Condensed Stretch (No Effect)

```html
<div style="font-stretch: condensed">
    Condensed text (property recognized but not rendered)
</div>
```

### Example 3: Expanded Stretch (No Effect)

```html
<div style="font-stretch: expanded">
    Expanded text (property recognized but not rendered)
</div>
```

### Example 4: Ultra-Condensed (No Effect)

```html
<div style="font-stretch: ultra-condensed">
    Ultra-condensed text (currently has no visual effect)
</div>
```

### Example 5: Semi-Expanded (No Effect)

```html
<p style="font-stretch: semi-expanded">
    Semi-expanded text (not currently applied)
</p>
```

### Example 6: Percentage Value (No Effect)

```html
<div style="font-stretch: 75%">
    Text with 75% stretch value (not implemented)
</div>
```

### Example 7: Table Header (No Effect)

```html
<table>
    <thead style="font-stretch: condensed">
        <tr><th>Column 1</th><th>Column 2</th></tr>
    </thead>
</table>
```

### Example 8: Heading with Stretch (No Effect)

```html
<h1 style="font-stretch: expanded; font-weight: bold">
    Wide Heading (stretch not applied)
</h1>
```

### Example 9: Narrow Text Block (No Effect)

```html
<div style="font-stretch: ultra-condensed; width: 100pt">
    Narrow column text with ultra-condensed stretch
    (property has no current effect)
</div>
```

### Example 10: Combined Properties (Partial Effect)

```html
<div style="font-family: Arial; font-size: 12pt; font-stretch: condensed">
    Text with multiple font properties - family and size work,
    but stretch has no effect
</div>
```

### Example 11: Invoice Header (No Effect)

```html
<div style="font-size: 24pt; font-weight: bold; font-stretch: expanded">
    INVOICE
</div>
```

### Example 12: Certificate Text (No Effect)

```html
<div style="font-stretch: extra-expanded; text-align: center">
    CERTIFICATE OF ACHIEVEMENT
</div>
```

### Example 13: Condensed Table (No Effect)

```html
<table style="font-stretch: condensed; font-size: 9pt">
    <tr>
        <td>Compact data</td>
        <td>In condensed format</td>
    </tr>
</table>
```

### Example 14: Form Labels (No Effect)

```html
<label style="font-stretch: semi-condensed">
    Customer Name:
</label>
```

### Example 15: Future-Ready Styling

```html
<!-- This CSS is prepared for future implementation -->
<style>
    .condensed-heading {
        font-family: Arial, sans-serif;
        font-size: 16pt;
        font-weight: bold;
        font-stretch: condensed; /* Not currently applied */
    }

    .expanded-title {
        font-family: Helvetica, sans-serif;
        font-size: 24pt;
        font-weight: 900;
        font-stretch: expanded; /* Not currently applied */
    }
</style>

<div class="expanded-title">
    ANNUAL REPORT 2024
</div>
<div class="condensed-heading">
    Executive Summary
</div>
<p style="font-stretch: normal">
    Regular body text with normal stretch value.
    While font-stretch is not currently implemented,
    including it in stylesheets ensures forward compatibility
    when this feature is added in future versions.
</p>
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
