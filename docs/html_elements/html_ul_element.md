---
layout: default
title: ul
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;ul&gt; : The Unordered List Element

The `<ul>` element represents an unordered list of items, typically rendered with bullet markers. It provides a semantic way to group related items where the order doesn't matter, such as features, options, or collections. In PDF output, unordered lists maintain their structure and support extensive customization through CSS.

## Usage

The `<ul>` element creates an unordered list that:
- Renders items with bullet markers (disc, circle, or custom markers)
- Contains one or more `<li>` (list item) elements
- Supports nested lists for hierarchical content
- Can be styled with CSS for custom markers and spacing
- Supports data binding for dynamic list generation
- Flows naturally across pages and columns when content overflows

```html
<ul>
    <li>First item</li>
    <li>Second item</li>
    <li>Third item</li>
</ul>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the list in the PDF structure. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Custom Data Attributes (Scryber Extensions)

These attributes provide advanced list control not available in standard HTML:

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-li-group` | string | Assigns the list to a numbering group for maintaining counts across multiple lists. |
| `data-li-concat` | boolean | When true, concatenates nested list numbers with parent list numbers (e.g., "1.a"). |
| `data-li-postfix` | string | Text to append after each list marker (e.g., "." for "• ."). |
| `data-li-prefix` | string | Text to prepend before each list marker (e.g., "# " for "# •"). |
| `data-li-inset` | unit | Width of the marker area from the left margin (default: 30pt). |
| `data-li-align` | alignment | Horizontal alignment of the marker: `left`, `center`, or `right` (default: right). |
| `data-li-style` | style | Override the numbering style (see list-style-type values). |

### CSS Style Support

The `<ul>` element supports extensive CSS styling through the `style` attribute or CSS classes:

**List-Specific Styles**:
- `list-style-type`: Marker style - `disc`, `circle`, `none`
- `list-style`: Shorthand for list styling
- `-pdf-li-prefix`: Custom CSS property for marker prefix
- `-pdf-li-postfix`: Custom CSS property for marker postfix
- `-pdf-li-inset`: Custom CSS property for marker area width
- `-pdf-li-align`: Custom CSS property for marker alignment
- `-pdf-li-concat`: Custom CSS property for concatenation (true/false/concatenate)
- `-pdf-li-group`: Custom CSS property for numbering group name

**Box Model**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`

**Layout & Positioning**:
- `display`: `block`, `none`
- `page-break-before`, `page-break-after`, `page-break-inside`
- `break-before`, `break-after`, `break-inside`
- `column-count`, `column-gap` (for multi-column layouts)

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `color` (inherited by list items and markers)
- `opacity`

**Typography** (inherited by list items):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `line-height`, `letter-spacing`

---

## Notes

### Default Behavior

The `<ul>` element has the following default behavior:

1. **Bullet Markers**: Uses disc (•) bullets by default
2. **Block Display**: Displays as a block-level element
3. **Indentation**: List items are indented with a 30pt inset for markers
4. **Right-Aligned Markers**: Markers align to the right of the marker area by default
5. **Natural Flow**: Content flows across pages and columns when space is limited

### Class Hierarchy

In the Scryber codebase:
- `HTMLListUnordered` extends `ListUnordered` extends `ListBase` extends `Panel`
- Implements standard HTML `<ul>` behavior
- Inherits layout and styling capabilities from base classes

### List Style Types

Supported `list-style-type` values for unordered lists:

- **disc**: Filled circle bullet (•) - default
- **circle**: Hollow circle bullet (○)
- **none**: No marker displayed

Note: Unlike HTML, `square` is not explicitly supported, but you can use custom markers with the `data-li-label` attribute on individual `<li>` elements.

### Marker Positioning

The marker area works as follows:
- A marker area with width specified by `data-li-inset` (default 30pt) is reserved on the left
- The marker aligns within this area according to `data-li-align` (default: right)
- A 10pt gap (alley) separates the marker from the content
- List item content begins after the marker area plus the alley

```
|<- marker area (inset) ->|<-alley->|<- content area ->
|                       • |   10pt  | List item text
```

### Nested Lists

Unordered lists can contain nested lists (either `<ul>` or `<ol>`):
- Nested lists inherit parent styling unless overridden
- Each nesting level can have different marker styles
- Use `data-li-concat` to create hierarchical numbering
- Indentation increases automatically for each nesting level

---

## Examples

### Basic Unordered List

```html
<ul>
    <li>Coffee</li>
    <li>Tea</li>
    <li>Milk</li>
</ul>
```

### Unordered List with Circle Markers

```html
<ul style="list-style-type: circle;">
    <li>Red</li>
    <li>Green</li>
    <li>Blue</li>
</ul>
```

### Unordered List with No Markers

```html
<ul style="list-style-type: none;">
    <li>Clean list without bullets</li>
    <li>Useful for custom styled lists</li>
    <li>Or navigation menus</li>
</ul>
```

### Styled Unordered List

```html
<style>
    .feature-list {
        background-color: #f5f5f5;
        padding: 20pt;
        border-left: 4pt solid #3498db;
        list-style-type: disc;
        color: #333;
        font-size: 12pt;
        line-height: 1.6;
    }
    .feature-list li {
        margin-bottom: 8pt;
    }
</style>

<ul class="feature-list">
    <li>Easy to use and implement</li>
    <li>Supports nested structures</li>
    <li>Fully customizable styling</li>
    <li>PDF-optimized rendering</li>
</ul>
```

### Nested Unordered Lists

```html
<ul>
    <li>Fruits
        <ul style="list-style-type: circle;">
            <li>Apple</li>
            <li>Banana</li>
            <li>Orange</li>
        </ul>
    </li>
    <li>Vegetables
        <ul style="list-style-type: circle;">
            <li>Carrot</li>
            <li>Broccoli</li>
            <li>Spinach</li>
        </ul>
    </li>
    <li>Grains</li>
</ul>
```

### Mixed Nested Lists (ul and ol)

```html
<ul>
    <li>Project Overview
        <ol>
            <li>Planning Phase</li>
            <li>Development Phase</li>
            <li>Testing Phase</li>
        </ol>
    </li>
    <li>Resources Required
        <ul style="list-style-type: circle;">
            <li>Team Members</li>
            <li>Budget</li>
            <li>Tools</li>
        </ul>
    </li>
</ul>
```

### Custom Marker Prefix and Postfix

```html
<!-- Using data attributes -->
<ul data-li-prefix="→ " data-li-postfix=" :">
    <li>First item</li>
    <li>Second item</li>
    <li>Third item</li>
</ul>

<!-- Using CSS properties -->
<style>
    .arrow-list {
        -pdf-li-prefix: '→ ';
        -pdf-li-postfix: ' :';
    }
</style>

<ul class="arrow-list">
    <li>Custom arrow markers</li>
    <li>With trailing colons</li>
</ul>
```

### Custom Marker Alignment and Inset

```html
<style>
    .wide-list {
        -pdf-li-inset: 50pt;
        -pdf-li-align: left;
    }
</style>

<ul class="wide-list">
    <li>List with wider marker area</li>
    <li>Markers aligned to the left</li>
    <li>Useful for complex nested structures</li>
</ul>
```

### Multi-Column List

```html
<style>
    .column-list {
        column-count: 2;
        column-gap: 20pt;
        column-rule: 1pt solid #ddd;
    }
</style>

<ul class="column-list">
    <li>Item 1</li>
    <li>Item 2</li>
    <li>Item 3</li>
    <li>Item 4</li>
    <li>Item 5</li>
    <li>Item 6</li>
    <li>Item 7</li>
    <li>Item 8</li>
</ul>
```

### List with Data Binding

```html
<!-- Template with model.items = ["Apple", "Banana", "Cherry"] -->
<ul>
    <li>Shopping List ({{count(model.items)}} items):</li>
    <template data-bind="{{model.items}}">
        <li>{{.}}</li>
    </template>
</ul>

<!-- Output:
• Shopping List (3 items):
• Apple
• Banana
• Cherry
-->
```

### Complex Data Binding Example

```html
<!-- Template with model.features = [{name: "Feature A", status: "complete"}, ...] -->
<style>
    .status-complete { color: green; }
    .status-pending { color: orange; }
</style>

<ul>
    <template data-bind="{{model.features}}">
        <li class="status-{{.status}}">
            <strong>{{.name}}</strong>: {{.status}}
        </li>
    </template>
</ul>
```

### Preventing List Item Breaks

```html
<style>
    .keep-together > li {
        break-inside: avoid;
    }
</style>

<ul class="keep-together">
    <li>This item with long content will not split across pages or columns.
        It will move as a complete block to the next page if needed.</li>
    <li>Each list item stays together as a single unit.</li>
    <li>Useful for maintaining readability in multi-column layouts.</li>
</ul>
```

### Grouped Lists

```html
<style>
    .group-one {
        -pdf-li-group: 'main-list';
        list-style-type: disc;
    }
</style>

<h4>First Section</h4>
<ul class="group-one">
    <li>Item 1</li>
    <li>Item 2</li>
</ul>

<p>Some intervening content...</p>

<h4>Continued Section</h4>
<ul class="group-one">
    <li>Item 3 (continues numbering)</li>
    <li>Item 4</li>
</ul>
```

### List with Rich Content

```html
<ul>
    <li>
        <strong style="font-size: 14pt; color: #2c3e50;">Item with Heading</strong>
        <p style="margin: 5pt 0;">This list item contains multiple paragraphs
        and rich formatting options.</p>
    </li>
    <li>
        <img src="images/icon.png" style="width: 20pt; height: 20pt;" />
        <span style="vertical-align: middle;">Item with image</span>
    </li>
    <li>
        <table style="width: 100%; margin-top: 5pt;">
            <tr>
                <td>Name</td>
                <td>Value</td>
            </tr>
            <tr>
                <td>Size</td>
                <td>Large</td>
            </tr>
        </table>
    </li>
</ul>
```

### Deeply Nested Lists with Custom Styling

```html
<style>
    ul { list-style-type: disc; }
    ul ul { list-style-type: circle; }
    ul ul ul { list-style-type: none; -pdf-li-prefix: '- '; }
</style>

<ul>
    <li>Level 1 - Disc
        <ul>
            <li>Level 2 - Circle
                <ul>
                    <li>Level 3 - Dash</li>
                    <li>Level 3 - Dash</li>
                </ul>
            </li>
            <li>Level 2 - Circle</li>
        </ul>
    </li>
    <li>Level 1 - Disc</li>
</ul>
```

### Conditional List Items

```html
<!-- Template with model.showDetails = true -->
<ul>
    <li>Always visible item</li>
    <li hidden="{{model.showDetails ? '' : 'hidden'}}">
        Conditionally visible based on model data
    </li>
    <li>Another always visible item</li>
</ul>
```

### List with Background and Borders

```html
<style>
    .boxed-list {
        background-color: #fff3cd;
        border: 2pt solid #ffc107;
        border-radius: 5pt;
        padding: 15pt;
    }
    .boxed-list li {
        padding: 5pt;
        border-bottom: 1pt dashed #ccc;
    }
    .boxed-list li:last-child {
        border-bottom: none;
    }
</style>

<ul class="boxed-list">
    <li>Highlighted list item</li>
    <li>With borders between items</li>
    <li>Last item has no border</li>
</ul>
```

---

## See Also

- [ol](/reference/htmltags/ol.html) - Ordered list element
- [li](/reference/htmltags/li.html) - List item element
- [dl](/reference/htmltags/dl.html) - Definition list element
- [Lists Reference](/reference/components/lists.html) - Complete lists documentation
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and expressions

---
