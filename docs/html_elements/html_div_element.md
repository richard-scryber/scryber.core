---
layout: default
title: div
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;div&gt; : The Generic Div Element

The `<div>` element is a generic block-level container used to group content and apply styling. It is one of the most commonly used elements in HTML templates for creating document structure and layout in PDF output.

## Usage

The `<div>` element creates a block-level container that:
- Takes full width of its parent container by default
- Stacks vertically with other block elements
- Can contain any type of content (text, images, other divs, etc.)
- Supports all CSS styling properties for positioning, sizing, colors, borders, and backgrounds
- Can be used with data binding and expressions

```html
<div class="section">
    <div style="font-size: 16pt; color: #333;">
        This is a simple div with inline styling
    </div>
    <div class="highlight">
        This div uses a CSS class for styling
    </div>
</div>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title when the element is used as a document section. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |
| `data-content` | expression | Dynamically sets the content of the div from bound data. |

### CSS Style Support

The `<div>` element supports extensive CSS styling through the `style` attribute or CSS classes:

**Box Model**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`
- `border-top`, `border-right`, `border-bottom`, `border-left`

**Positioning**:
- `position`: `static`, `relative`, `absolute`
- `display`: `block`, `inline`, `inline-block`, `none`
- `float`: `left`, `right`, `none`
- `clear`: `both`, `left`, `right`, `none`
- `top`, `left`, `right`, `bottom` (for positioned elements)

**Layout**:
- `overflow`: `visible`, `hidden`, `clip`
- `column-count`, `column-width`, `column-gap` (multi-column layout)
- `page-break-before`, `page-break-after`, `page-break-inside`

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `background-position`, `background-size`, `background-repeat`
- `color` (text color)
- `opacity`
- `transform` (rotation, scaling, translation)

**Typography** (inherited by child text):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `text-align`, `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`, `word-spacing`

---

## Notes

### Default Behavior

The `<div>` element has the following default behavior:

1. **Block Display**: Displays as a block-level element (stacks vertically)
2. **Full Width**: Takes 100% of the parent container's width by default
3. **Static Position**: Uses normal document flow positioning
4. **No Overflow**: Content overflows to the next page when space is exhausted

### Class Hierarchy

In the Scryber codebase:
- `HTMLDiv` extends `Div` extends `Panel` extends `VisualComponent`
- The `HTMLDiv` class is specifically for HTML namespace elements
- Inherits all properties and behaviors from the base `Panel` class

### Layout Engine

The `<div>` element uses the `LayoutEnginePanel` layout engine, which:
- Supports child content layout in block or inline flow
- Handles page breaks and content overflow
- Manages multi-column layouts when configured
- Calculates box model dimensions (content, padding, border, margin)

### Page Breaking

Content within a `<div>` will automatically flow to the next page when:
- The content height exceeds available space
- A `page-break-before` or `page-break-after` style is applied
- The `overflow-action` is set to continue

Control page breaking behavior:
```html
<div style="page-break-before: always;">Starts on new page</div>
<div style="page-break-inside: avoid;">Keeps together</div>
```

### Multi-Column Layout

Divs can be configured for multi-column layout:
```html
<div style="column-count: 2; column-gap: 20pt;">
    Content flows across two columns
</div>
```

---

## Examples

### Basic Div with Styling

```html
<div style="background-color: #f0f0f0; padding: 20pt; border: 1pt solid #ccc;">
    <h2>Section Title</h2>
    <p>This div creates a styled container with gray background and border.</p>
</div>
```

### Nested Divs for Layout

```html
<div style="width: 100%; border: 2pt solid #333;">
    <div style="background-color: #336699; color: white; padding: 10pt;">
        Header Section
    </div>
    <div style="padding: 15pt;">
        <div style="width: 50%; float: left;">
            Left column content
        </div>
        <div style="width: 50%; float: right;">
            Right column content
        </div>
    </div>
</div>
```

### Positioned Div

```html
<div style="position: relative; height: 200pt; border: 1pt solid black;">
    <div style="position: absolute; top: 50pt; left: 50pt;
                background-color: yellow; padding: 10pt;">
        Absolutely positioned content
    </div>
</div>
```

### Div with Data Binding

```html
<!-- In template with model = { name: "John", age: 30 } -->
<div class="user-card">
    <div style="font-weight: bold;">Name: {{model.name}}</div>
    <div>Age: {{model.age}}</div>
</div>

<!-- Output: -->
<!-- Name: John -->
<!-- Age: 30 -->
```

### Repeating Divs with Template

```html
<!-- In template with model.items = [{name: "Item 1"}, {name: "Item 2"}] -->
<div class="container">
    <template data-bind="{{model.items}}">
        <div class="item" style="margin-bottom: 10pt; padding: 5pt; border: 1pt solid #ddd;">
            {{.name}}
        </div>
    </template>
</div>

<!-- Output: -->
<!-- Two divs, each with item name and styling -->
```

### Multi-Column Div

```html
<div style="column-count: 3; column-gap: 15pt; column-rule: 1pt solid #ccc;">
    Lorem ipsum dolor sit amet, consectetur adipiscing elit.
    This text will flow across three columns with a rule between them.
    Each column will be equal width with 15pt gap between.
</div>
```

### Div with Background Image

```html
<div style="background-image: url('images/background.jpg');
            background-size: cover;
            background-position: center;
            min-height: 200pt;
            padding: 20pt;">
    <div style="background-color: rgba(255,255,255,0.8); padding: 10pt;">
        Content over background image
    </div>
</div>
```

### Hidden Div

```html
<!-- Using hidden attribute -->
<div hidden="hidden">
    This content will not appear in the PDF
</div>

<!-- Using CSS display -->
<div style="display: none;">
    This content is also hidden
</div>

<!-- Conditionally shown based on expression -->
<div hidden="{{model.hideSection ? 'hidden' : ''}}">
    Shown only when model.hideSection is false
</div>
```

### Transform Example

```html
<div style="transform: rotate(45deg);
            width: 100pt;
            height: 100pt;
            background-color: #ff6347;
            margin: 50pt;">
    Rotated div
</div>
```

### Page Break Control

```html
<div style="page-break-after: always;">
    This content ends with a page break
</div>

<div style="page-break-before: always;">
    This content starts on a new page
</div>

<div style="page-break-inside: avoid; border: 1pt solid black; padding: 10pt;">
    This entire div will be kept together on one page if possible
</div>
```

---

## See Also

- [span](/reference/htmltags/span.html) - Inline container element
- [p](/reference/htmltags/p.html) - Paragraph element (block container with margins)
- [section](/reference/htmltags/section.html) - Semantic section element
- [Panel Component](/reference/components/panel.html) - Base panel component in Scryber namespace
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and expressions
- [Layout](/reference/layout/) - Layout and positioning guide

---
