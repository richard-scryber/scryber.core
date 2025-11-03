---
layout: default
title: font-style
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# font-style : The Font Style Property

Summary

The `font-style` property specifies the stylistic variation of a font, primarily controlling whether text is displayed in normal, italic, or oblique style. This property is essential for emphasizing text or creating visual distinction in PDF documents.

## Usage

```css
/* Keyword values */
font-style: normal;
font-style: italic;
font-style: oblique;

/* Within style attribute */
<p style="font-style: italic">Emphasized text</p>
```

---

## Values

### Supported Values

- **normal** - Regular upright text (default)
- **italic** - Cursive italic version of the font (true italic)
- **oblique** - Slanted version of the normal font (simulated italic)

### Expression Support

- **calc()** - Expressions with font-style conversions
- **var()** - CSS variables: `var(--text-style)`

---

## Notes

- `normal` is the default font style for regular text
- `italic` uses the font's designed italic variant when available
- `oblique` creates a slanted version, which may be simulated if the font lacks a true oblique variant
- Many fonts treat italic and oblique similarly in PDF rendering
- Font-style is inherited from parent elements
- Some fonts may not have italic variants; in such cases, the renderer may simulate the style
- Commonly used for emphasis, citations, foreign words, and technical terms

---

## Data Binding

CSS properties support dynamic values through Scryber's Handlebars-style data binding syntax using `{{expression}}` in inline styles. This enables font-style to be determined dynamically based on model data, content type, or conditional logic.

### Binding Syntax

Data binding expressions are enclosed in double curly braces `{{}}` and can reference:
- Model properties: `{{model.propertyName}}`
- Nested data: `{{content.formatting.style}}`
- Conditional expressions: `{{condition ? valueIfTrue : valueIfFalse}}`

### Data Binding Examples

```html
<!-- Dynamic style from model data -->
<p style="font-style: {{text.styleValue}}">
    Text with dynamically applied font style
</p>

<!-- Conditional italic for emphasis -->
<span style="font-style: {{item.isEmphasized ? 'italic' : 'normal'}}">
    {{item.text}}
</span>

<!-- Context-aware styling -->
<body>
    <!-- Italicize foreign language terms -->
    <p>
        The concept of <span style="font-style: {{isForeignTerm ? 'italic' : 'normal'}}">
            {{term}}
        </span> is central to understanding.
    </p>

    <!-- Style based on content type -->
    <div style="font-style: {{contentType == 'quote' ? 'italic' : 'normal'}}">
        {{contentText}}
    </div>

    <!-- User-configurable emphasis -->
    <div style="font-style: {{preferences.useItalicForNotes ? 'italic' : 'normal'}}">
        Note: {{noteContent}}
    </div>
</body>
```

**Note:** Bound font-style values should be valid CSS keywords: 'normal', 'italic', or 'oblique'. Invalid values may result in the default 'normal' style being applied.

---

## Examples

### Example 1: Normal Font Style

```html
<p style="font-style: normal">
    This text uses normal (upright) font style
</p>
```

### Example 2: Italic Text

```html
<p style="font-style: italic">
    This text is displayed in italic style
</p>
```

### Example 3: Oblique Text

```html
<p style="font-style: oblique">
    This text uses oblique (slanted) style
</p>
```

### Example 4: Emphasized Quote

```html
<blockquote style="font-style: italic">
    "The only way to do great work is to love what you do."
</blockquote>
```

### Example 5: Foreign Language

```html
<p>
    The French phrase <span style="font-style: italic">je ne sais quoi</span>
    means "I don't know what"
</p>
```

### Example 6: Book Title

```html
<p>
    She read <span style="font-style: italic">To Kill a Mockingbird</span>
    last summer
</p>
```

### Example 7: Technical Terms

```html
<p style="font-style: normal">
    The variable <code style="font-style: italic">x</code> represents
    the unknown value
</p>
```

### Example 8: Invoice Note

```html
<div style="font-size: 10pt">
    <div>Total Amount: $1,250.00</div>
    <div style="font-style: italic">Payment due within 30 days</div>
</div>
```

### Example 9: Report Caption

```html
<figure>
    <img src="chart.png" />
    <figcaption style="font-style: italic; font-size: small">
        Figure 1: Sales Growth 2020-2024
    </figcaption>
</figure>
```

### Example 10: Legal Disclaimer

```html
<footer style="font-style: italic; font-size: 9pt">
    This document is confidential and intended for authorized recipients only.
</footer>
```

### Example 11: Definition Term

```html
<dl>
    <dt style="font-weight: bold">API</dt>
    <dd style="font-style: italic">
        Application Programming Interface - a set of protocols for building software
    </dd>
</dl>
```

### Example 12: Citation

```html
<p>
    According to Smith (2024), "innovation drives success"
    <span style="font-style: italic">(p. 42)</span>
</p>
```

### Example 13: Mixing Styles

```html
<p style="font-style: normal">
    Normal text with <span style="font-style: italic">italic emphasis</span>
    and back to normal
</p>
```

### Example 14: Certificate Text

```html
<div style="text-align: center">
    <div style="font-size: 20pt; font-weight: bold">
        Certificate of Achievement
    </div>
    <div style="font-size: 14pt; font-style: italic; margin: 20pt 0">
        Awarded to
    </div>
    <div style="font-size: 18pt; font-weight: bold">
        Jane Doe
    </div>
</div>
```

### Example 15: Professional Document

```html
<article style="font-family: Georgia, serif; font-size: 11pt">
    <h1 style="font-style: normal; font-size: 18pt">
        Executive Summary
    </h1>
    <p>
        This report presents findings from our annual review.
        <span style="font-style: italic">Key metrics</span> show
        improvement across all departments.
    </p>
    <p style="font-style: italic; border-left: 3pt solid #ccc; padding-left: 12pt">
        Note: All financial figures have been audited and verified
        by independent accountants.
    </p>
    <p>
        The term <span style="font-style: italic">synergy</span> has
        become increasingly relevant to our operational strategy.
    </p>
</article>
```

---

## See Also

- [font](/reference/cssproperties/font) - Shorthand font property
- [font-family](/reference/cssproperties/font-family) - Font family specification
- [font-weight](/reference/cssproperties/font-weight) - Font weight values
- [font-size](/reference/cssproperties/font-size) - Font size specification
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
