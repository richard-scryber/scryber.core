---
layout: default
title: font
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;font&gt; : The Font Element (Deprecated)
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---

## Summary
The `<font>` element is a deprecated HTML element for styling text appearance. While still supported for backward compatibility, **CSS styling is strongly recommended** instead.

## Usage

The `<font>` element was historically used to control text appearance through attributes:
- `color` - Sets text color
- `face` - Sets font family
- `size` - Sets font size

**⚠️ Deprecation Notice**: This element is deprecated in modern HTML. Use CSS properties (`color`, `font-family`, `font-size`) with `<span>` or other elements instead.

```html
<!-- Deprecated (but works) -->
<font color="red" face="Arial" size="14pt">Styled text</font>

<!-- Recommended alternative -->
<span style="color: red; font-family: Arial; font-size: 14pt;">Styled text</span>
```

---

## Supported Attributes

### Font Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `color` | color | Sets the text color. Accepts color names, hex values (#RRGGBB), or rgb() values. |
| `face` | string | Sets the font family name (e.g., "Arial", "Times New Roman", "Helvetica"). |
| `size` | unit | Sets the font size in points (e.g., "12pt", "14pt", "16pt"). |

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. |
| `style` | string | Inline CSS styles (preferred over font attributes). |
| `title` | string | Sets the outline/bookmark title. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide. |

### CSS Style Support

As an inline element, supports standard CSS properties:
- **Typography**: `font-family`, `font-size`, `font-weight`, `font-style`, `color`
- **Background**: `background-color`, `background-image`
- **Borders**: `border`, `padding`
- **Display**: `display`, `vertical-align`

---

## Notes

### Why is `<font>` Deprecated?

1. **Separation of Concerns**: HTML should define structure, CSS should define presentation
2. **Maintainability**: CSS classes and stylesheets are easier to maintain than inline font tags
3. **Flexibility**: CSS provides far more styling options than font attributes
4. **Standards Compliance**: Modern HTML standards discourage presentational markup

### Migration from `<font>` to CSS

| Old (font tag) | New (CSS equivalent) |
|----------------|---------------------|
| `<font color="red">` | `style="color: red;"` |
| `<font face="Arial">` | `style="font-family: Arial;"` |
| `<font size="14pt">` | `style="font-size: 14pt;"` |
| `<font color="blue" face="Helvetica" size="16pt">` | `style="color: blue; font-family: Helvetica; font-size: 16pt;"` |

### When to Use (Legacy Support)

The `<font>` element may be useful when:
- Maintaining legacy HTML templates
- Converting old HTML documents to PDF
- Working with generated HTML from legacy systems
- Quick prototyping (though CSS is still recommended)

### Default Behavior

The `<font>` element:
- Extends the `Span` component (inline element)
- Does not have default styling (neutral)
- Applies attributes directly to text properties
- Can be nested within other inline elements

---

## Examples

### Basic Font Styling (Deprecated)

```html
<p>This is <font color="red">red text</font> in a paragraph.</p>
<p>This is <font face="Courier">monospace text</font> in Courier.</p>
<p>This is <font size="18pt">larger text</font> at 18 points.</p>
```

### Combined Font Attributes

```html
<font color="#336699" face="Arial" size="16pt">
    Styled text with color, font, and size
</font>
```

### Migration Example: Before and After

**Before (deprecated `<font>`):**
```html
<h1><font color="#2c3e50" face="Arial" size="24pt">Document Title</font></h1>
<p>
    This is regular text with <font color="red" size="12pt">highlighted</font> sections
    and <font face="Courier" size="10pt">code snippets</font>.
</p>
```

**After (modern CSS):**
```html
<style>
    h1 {
        color: #2c3e50;
        font-family: Arial, sans-serif;
        font-size: 24pt;
    }

    .highlight {
        color: red;
        font-size: 12pt;
    }

    .code {
        font-family: Courier, monospace;
        font-size: 10pt;
    }
</style>

<h1>Document Title</h1>
<p>
    This is regular text with <span class="highlight">highlighted</span> sections
    and <span class="code">code snippets</span>.
</p>
```

---

## See Also

- [span](html_span_element.html) - Modern inline container (recommended)
- [strong](html_strong_element.html) - Bold text (semantic)
- [em](html_strong_em_element.html) - Italic text (semantic)
- [CSS Styles](/learning/styles/) - Complete CSS styling reference
- [Text Formatting](/learning/styles/fonts.html) - Modern text formatting elements

---
