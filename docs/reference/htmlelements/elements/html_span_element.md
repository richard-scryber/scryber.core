---
layout: default
title: span
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;span&gt; : The Generic Inline Container
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

The `<span>` element is a generic inline container used to group inline content and apply styling without breaking the flow of text. It is one of the most commonly used elements for inline text formatting and styling.

## Usage

The `<span>` element creates an inline container that:
- Flows with surrounding text without creating line breaks
- Can wrap to multiple lines
- Can contain text and other inline elements
- Supports all CSS styling properties
- Can be used with data binding and expressions

```html
<p>This is <span style="color: red; font-weight: bold;">important text</span> in a paragraph.</p>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the span content. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-content` | expression | Dynamically sets the text content from bound data. |

### CSS Style Support

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color` (text color)
- `text-decoration`, `text-transform`
- `letter-spacing`, `word-spacing`

**Background and Borders**:
- `background-color`, `background-image`
- `border`, `border-radius`
- `padding`

**Display**:
- `display`: `inline` (default), `inline-block`, `block`, `none`
- `vertical-align`

**Transform**:
- `transform` (rotation, scaling, translation)

---

## Notes

### Default Behavior

The `<span>` element has the following default behavior:

1. **Inline Display**: Flows with text without line breaks
2. **No Width/Height**: Sized by content unless `display: inline-block` or `display: block`
3. **Wrappable**: Wraps to next line when needed
4. **Inherits Styles**: Inherits text properties from parent

### Common Use Cases

- Applying color or emphasis to specific words
- Data binding for dynamic text content
- Wrapping text for CSS class application
- Inline icons or symbols

---

## Examples

### Basic Text Styling

```html
<p>
    The price is <span style="color: green; font-weight: bold;">$99.99</span>
    but you can save <span style="color: red;">20%</span> today!
</p>
```

### With CSS Classes

```html
<style>
    .highlight { background-color: yellow; padding: 2pt; }
    .price { color: #006600; font-size: 14pt; font-weight: bold; }
</style>

<p>Special offer: <span class="highlight">Only <span class="price">$49.99</span></span></p>
```

### Data Binding

```html
<!-- With model = { userName: "John", userRole: "Admin" } -->
<p>Welcome <span style="font-weight: bold;">{{model.userName}}</span>!</p>
<p>Your role: <span class="role">{{model.userRole}}</span></p>
```

### Inline-Block for Sizing

```html
<span style="display: inline-block; width: 100pt; background-color: #ddd;
             text-align: center; padding: 5pt;">
    Sized Box
</span>
```

### Nested Spans

```html
<p>
    <span style="font-size: 16pt;">
        This is larger text with
        <span style="color: blue;">blue</span> and
        <span style="color: red; font-weight: bold;">bold red</span> words.
    </span>
</p>
```

### Dynamic Content

```html
<p>Status: <span data-content="{{model.status}}"
               style="color: {{model.statusColor}}"></span></p>
```

---

## See Also

- [div](/reference/htmltags/div.html) - Block-level container element
- [strong](/reference/htmltags/strong.html) - Bold text (semantic)
- [em](/reference/htmltags/em.html) - Italic text (semantic)
- [b](/reference/htmltags/b.html) - Bold text (presentational)
- [i](/reference/htmltags/i.html) - Italic text (presentational)

---
