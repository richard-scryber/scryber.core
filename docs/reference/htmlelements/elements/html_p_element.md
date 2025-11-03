---
layout: default
title: p
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;p&gt; : The Paragraph Element
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

The `<p>` element represents a paragraph of text. It is a block-level element that automatically adds vertical spacing before and after the content.

## Usage

The `<p>` element creates a paragraph that:
- Displays as a block-level element
- Has default top and bottom margins (0.5em each)
- Takes full width of its parent container
- Can contain inline elements and text
- Supports all CSS styling properties

```html
<p>This is a paragraph with default spacing.</p>
<p style="text-align: justify;">This paragraph is justified and will align text evenly on both left and right sides.</p>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the paragraph. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-content` | expression | Dynamically sets the content from bound data. |

### CSS Style Support

Supports all CSS properties including:

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color`, `line-height`, `letter-spacing`
- `text-align`, `text-indent`, `text-decoration`
- `hyphens` - Control word breaking with hyphens

**Spacing**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding` (all variants)

**Borders and Background**:
- `border` (all variants)
- `background-color`, `background-image`

**Layout**:
- `width`, `height`, `max-width`
- `page-break-before`, `page-break-after`, `page-break-inside`

---

## Notes

### Default Styling

The `<p>` element has default styling:
- **Top Margin**: 0.5em
- **Bottom Margin**: 0.5em
- **Display**: Block
- **Full Width**: Takes 100% of parent width

These defaults provide standard paragraph spacing. Override with CSS as needed.

### Text Flow

Paragraphs support:
- Automatic line wrapping
- Hyphenation (with `hyphens: auto`)
- Text justification
- First-line indenting

### Page Breaking

Control how paragraphs break across pages:
```html
<p style="page-break-inside: avoid;">This paragraph will not be split across pages.</p>
```

---

## Examples

### Basic Paragraphs

```html
<p>This is the first paragraph with default spacing.</p>
<p>This is the second paragraph, also with default spacing.</p>
```

### Styled Paragraph

```html
<p style="font-size: 12pt; color: #333; line-height: 1.5;
          text-align: justify; text-indent: 20pt;">
    This paragraph has custom font size, color, line height, justification,
    and first-line indentation for a professional appearance.
</p>
```

### With Background and Border

```html
<p style="background-color: #f0f0f0; border-left: 4pt solid #336699;
          padding: 10pt; margin: 15pt 0;">
    This paragraph has a gray background with a blue left border,
    creating a callout or note effect.
</p>
```

### Text Alignment

```html
<p style="text-align: left;">Left-aligned paragraph (default)</p>
<p style="text-align: center;">Center-aligned paragraph</p>
<p style="text-align: right;">Right-aligned paragraph</p>
<p style="text-align: justify;">Justified paragraph with text aligned to both margins</p>
```

### Data Binding

```html
<!-- With model = { description: "Product details here" } -->
<p class="description">{{model.description}}</p>

<!-- Dynamic styling -->
<p style="color: {{model.textColor}}; font-size: {{model.fontSize}}pt;">
    {{model.content}}
</p>
```

### First-Line Indent

```html
<style>
    .indented {
        text-indent: 30pt;
        text-align: justify;
        margin-bottom: 10pt;
    }
</style>

<p class="indented">
    This paragraph has a first-line indent of 30 points, creating a
    traditional book-style paragraph format.
</p>
<p class="indented">
    Each paragraph with this class will have the same formatting.
</p>
```

### With Hyphenation

```html
<p style="width: 200pt; text-align: justify; hyphens: auto;
          padding: 10pt; border: 1pt solid #ccc;">
    This paragraph has automatic hyphenation enabled for better
    justification in narrow columns with words-like-hyphenated-terms.
</p>
```

### No Break Across Pages

```html
<p style="page-break-inside: avoid; border: 1pt solid black; padding: 10pt;">
    This entire paragraph will be kept together on one page.
    If there isn't enough space, the entire paragraph moves to the next page.
</p>
```

### Drop Cap Effect

```html
<style>
    .drop-cap::first-letter {
        font-size: 36pt;
        font-weight: bold;
        float: left;
        margin-right: 5pt;
        line-height: 30pt;
    }
</style>

<p class="drop-cap">
    This paragraph starts with a large drop cap letter, creating
    an elegant beginning for a chapter or section.
</p>
```

### Multiple Paragraphs with Spacing

```html
<div style="column-count: 2; column-gap: 20pt;">
    <p>First paragraph in a two-column layout with standard spacing.</p>
    <p>Second paragraph continues in the columns.</p>
    <p>Third paragraph may flow to the second column.</p>
</div>
```

---

## See Also

- [div](/reference/htmltags/div.html) - Generic block container
- [span](/reference/htmltags/span.html) - Inline container
- [h1-h6](/reference/htmltags/heading.html) - Heading elements
- [blockquote](/reference/htmltags/blockquote.html) - Block quotation
- [pre](/reference/htmltags/pre.html) - Preformatted text

---
