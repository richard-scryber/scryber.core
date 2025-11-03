---
layout: default
title: fieldset and legend
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;fieldset&gt; and &lt;legend&gt; : Form Grouping Elements
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
The `<fieldset>` and `<legend>` elements are used together to create visually grouped sections within forms or form-like layouts. The `<fieldset>` element creates a bordered container, while the `<legend>` element provides a caption or title that appears embedded in the fieldset's border. These elements are essential for organizing complex forms into logical sections.

---

## Usage

The `<fieldset>` element creates a container that:
- Groups related form elements together
- Displays a border around its contents by default
- Can contain a `<legend>` as its first child for labeling
- Supports full CSS styling and customization
- Creates semantic structure for form sections
- Works with any HTML content, not just form elements

The `<legend>` element provides a caption that:
- Appears at the top of the fieldset border
- Acts as a title or label for the grouped content
- Is typically the first child of a `<fieldset>`
- Can be styled independently
- Has auto-width by default (doesn't span full width)

```html
<fieldset>
    <legend>Personal Information</legend>
    <label>Name:</label>
    <input type="text" value="John Doe" />
</fieldset>
```

---

## Fieldset Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the fieldset. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### CSS Style Support for Fieldset

**Sizing**:
- `width`, `min-width`, `max-width`
- `height`, `min-height`, `max-height`
- Default: Auto-sized to content

**Positioning**:
- `display`: `block` (default), `inline-block`, `none`
- `position`: `static`, `relative`, `absolute`
- `float`: `left`, `right`, `none`

**Spacing**:
- `margin` (all variants) - Default: `2pt left/right`, `1em top`
- `padding` (all variants) - Default: `0.35em top`, `0.625em bottom`, `0.75em left/right`

**Visual Styling**:
- `border`, `border-width`, `border-color`, `border-style` - Default: `2pt solid`
- `border-radius` - rounded corners
- `background-color`, `background-image`
- `color` - text color (inherited by contents)

**Typography** (inherited by contents):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `line-height`, `text-align`

## Legend Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the legend. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the legend. |

### CSS Style Support for Legend

**Sizing**:
- `width` - Default: Auto-width (not full width)
- `padding` (all variants)

**Positioning**:
- `display`: `block`, `inline-block`
- `text-align`: `left`, `center`, `right`

**Visual Styling**:
- `color`, `background-color`
- `border`, `padding`
- `font-family`, `font-size`, `font-weight`, `font-style`

---

## Notes

### Default Styling

**Fieldset defaults**:
- Margin: `2pt` left and right, `1em` top
- Padding: `0.35em` top, `0.625em` bottom, `0.75em` left/right
- Border: `2pt solid` (default color)
- Display: `block`

**Legend defaults**:
- Width: Auto (not full-width)
- Position: Embedded in top border
- Display: `block` (but auto-sized)

### Proper Structure

The `<legend>` should be the **first child** of the `<fieldset>`:

```html
<!-- Correct structure -->
<fieldset>
    <legend>Section Title</legend>
    <!-- Other content -->
</fieldset>

<!-- Incorrect - legend not first -->
<fieldset>
    <div>Some content</div>
    <legend>Section Title</legend>  <!-- Will still work but not semantically correct -->
</fieldset>
```

### Legend Positioning

The `<legend>` appears embedded in the top border of the fieldset. Its position can be influenced by:
- `text-align` on the fieldset (moves legend left/center/right)
- `padding` on the legend (adds space around the text)
- `margin` on the legend (adjusts position)

### Nesting Fieldsets

Fieldsets can be nested to create hierarchical form sections:

```html
<fieldset>
    <legend>Shipping Information</legend>

    <fieldset>
        <legend>Address</legend>
        <!-- Address fields -->
    </fieldset>

    <fieldset>
        <legend>Delivery Options</legend>
        <!-- Delivery fields -->
    </fieldset>
</fieldset>
```

### Styling Tips

1. **Remove border**: Set `border: none` for invisible grouping
2. **Custom borders**: Use any border style, width, and color
3. **Background colors**: Add backgrounds to distinguish sections
4. **Legend styling**: Style legends independently for emphasis
5. **Padding control**: Adjust padding for spacing within the fieldset

---

## Class Hierarchy

In the library codebase:
- `HTMLFieldSet` extends `Div` (block container)
- `HTMLLegend` extends `Div` with auto-width enabled
- Both support full HTML attributes and CSS styling

---

## Examples

*Examples will follow with forms implementation within the library*

---

## See Also

- [input](/reference/htmltags/input.html) - Input field element
- [label](/reference/htmltags/label.html) - Label element for form fields
- [form](/reference/htmltags/form.html) - Form container element
- [div](/reference/htmltags/div.html) - Generic block container
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
