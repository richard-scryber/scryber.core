---
layout: default
title: li
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;li&gt; : The List Item Element
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

The `<li>` element represents an item in a list. It must be contained within a parent list element: `<ul>` (unordered list), `<ol>` (ordered list), or `<menu>`. The `<li>` element can contain any type of content including text, images, tables, nested lists, and other block or inline elements. Each list item is automatically rendered with a marker (bullet, number, or custom label) inherited from its parent list.

## Usage

The `<li>` element creates a list item that:
- Displays with an automatic marker (bullet for `<ul>`, number for `<ol>`)
- Can contain any combination of inline and block content
- Supports nested lists for hierarchical structures
- Can override parent list styling for individual customization
- Supports custom labels to replace automatic markers
- Can be styled independently with CSS
- Works with data binding for dynamic content generation

```html
<ul>
    <li>Simple text item</li>
    <li>Item with <strong>bold</strong> text</li>
    <li>
        Item with nested content
        <p>Additional paragraph content</p>
    </li>
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
| `title` | string | Sets the outline/bookmark title for the list item in the PDF structure. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Custom Data Attributes (Scryber Extensions)

These attributes provide control over individual list items:

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-li-align` | alignment | Horizontal alignment of the marker/number: `left`, `center`, or `right`. Overrides parent list setting. |
| `data-li-inset` | unit | Width of the marker/number area for this item only. Overrides parent list setting. |
| `data-li-label` | string | Custom text label to use instead of automatic marker/number. |

### CSS Style Support

The `<li>` element supports extensive CSS styling through the `style` attribute or CSS classes:

**List Item-Specific Styles**:
- `-pdf-li-align`: Custom CSS property for marker alignment
- `-pdf-li-inset`: Custom CSS property for marker area width
- `-pdf-li-label`: Custom CSS property for custom label text

**Box Model**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`

**Layout & Positioning**:
- `display`: `block`, `inline-block`, `none`
- `break-before`, `break-after`, `break-inside` (control page/column breaks)
- `width`, `height`, `min-width`, `max-width` (with appropriate display mode)

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `color` (text color for item content)
- `opacity`

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `text-align`, `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`, `word-spacing`

---

## Notes

### Default Behavior

The `<li>` element has the following default behavior:

1. **Automatic Marker**: Inherits marker style from parent list (`<ul>` or `<ol>`)
2. **Block Display**: Content area displays as block-level by default
3. **Content Flow**: Content begins after the marker area plus alley (gap)
4. **Natural Breaking**: Content can split across pages/columns by default
5. **Marker Positioning**: Marker is positioned according to parent list settings

### Class Hierarchy

In the Scryber codebase:
- `HTMLListItem` extends `ListItem` extends `ContainerComponent` extends `VisualComponent`
- Implements standard HTML `<li>` behavior with PDF enhancements
- Inherits layout capabilities from base component classes
- Special layout handling via `LayoutEngineListItem2`

### Content Area Layout

The list item layout structure:

```
|<- marker area ->|<- alley ->|<- content area ->
|               • |    10pt   | Item text and content
```

- **Marker Area**: Width set by `data-li-inset` or parent list setting (default 30pt)
- **Alley**: Fixed 10pt gap between marker and content
- **Content Area**: Remaining space for item content

### Marker Inheritance

List item markers are determined by:
1. Parent list's `list-style-type` setting
2. Parent list's numbering configuration (for `<ol>`)
3. Individual item's `data-li-label` if specified (overrides all)

### Custom Labels

You can override automatic markers with custom labels:
- Use `data-li-label="★"` to display a star instead of bullet/number
- Labels are static text and don't auto-increment
- Useful for special items or custom marker symbols
- Can include Unicode characters and emoji

### Breaking Behavior

By default, list items can break across pages/columns:
- Use `break-inside: avoid` to keep item content together
- Long items will still break if they exceed available space
- Nested lists within items can break independently

---

## Examples

### Basic List Items

```html
<ul>
    <li>First item</li>
    <li>Second item</li>
    <li>Third item</li>
</ul>
```

### List Items with Rich Content

```html
<ol>
    <li>
        <strong>Introduction</strong>
        <p>This paragraph provides details about the introduction section.</p>
    </li>
    <li>
        <strong>Main Content</strong>
        <p>The main content includes several important points.</p>
    </li>
</ol>
```

### List Item with Nested List

```html
<ul>
    <li>Parent Item
        <ul>
            <li>Child Item 1</li>
            <li>Child Item 2</li>
        </ul>
    </li>
    <li>Another Parent Item</li>
</ul>
```

### Custom Label for Specific Item

```html
<ul>
    <li>Regular bullet item</li>
    <li data-li-label="★">Special starred item</li>
    <li>Regular bullet item</li>
</ul>
```

### Multiple Custom Labels

```html
<ul style="list-style-type: none;">
    <li data-li-label="✓">Completed task</li>
    <li data-li-label="✗">Failed task</li>
    <li data-li-label="⧗">In progress task</li>
    <li data-li-label="○">Pending task</li>
</ul>
```

### Individual Item Styling

```html
<ol>
    <li style="background-color: #e8f5e9; padding: 10pt; margin-bottom: 5pt;">
        Highlighted item with green background
    </li>
    <li style="background-color: #fff3e0; padding: 10pt; margin-bottom: 5pt;">
        Highlighted item with orange background
    </li>
    <li>Regular item without special styling</li>
</ol>
```

### Custom Marker Alignment per Item

```html
<ol>
    <li>Normal right-aligned number</li>
    <li data-li-align="left">Left-aligned number</li>
    <li data-li-align="center">Center-aligned number</li>
    <li>Back to normal alignment</li>
</ol>
```

### Custom Inset per Item

```html
<ol>
    <li>Standard inset</li>
    <li data-li-inset="50pt">Wider inset for this item with longer number format</li>
    <li data-li-inset="80pt">Even wider inset for special formatting</li>
    <li>Back to standard inset</li>
</ol>
```

### Preventing Item Break

```html
<style>
    .keep-together {
        break-inside: avoid;
    }
</style>

<ul>
    <li>This item can break across pages normally</li>
    <li class="keep-together">
        This item will be kept together on the same page/column.
        Even if it has multiple lines of content, it won't split.
        The entire item moves as one unit.
    </li>
    <li>Another item that can break normally</li>
</ul>
```

### List Item with Image

```html
<ul>
    <li>
        <img src="images/icon1.png" style="width: 20pt; height: 20pt; vertical-align: middle;" />
        <span style="margin-left: 5pt;">Item with icon</span>
    </li>
    <li>
        <img src="images/icon2.png" style="width: 20pt; height: 20pt; vertical-align: middle;" />
        <span style="margin-left: 5pt;">Another item with icon</span>
    </li>
</ul>
```

### List Item with Table

```html
<ol>
    <li>
        Summary Data:
        <table style="width: 100%; margin-top: 5pt; border-collapse: collapse;">
            <tr>
                <td style="border: 1pt solid #ccc; padding: 5pt;">Metric</td>
                <td style="border: 1pt solid #ccc; padding: 5pt;">Value</td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ccc; padding: 5pt;">Total</td>
                <td style="border: 1pt solid #ccc; padding: 5pt;">1000</td>
            </tr>
        </table>
    </li>
</ol>
```

### Styled List Items with Borders

```html
<style>
    .bordered-items li {
        border: 1pt solid #ddd;
        border-radius: 3pt;
        padding: 10pt;
        margin-bottom: 8pt;
        background-color: #f9f9f9;
    }
    .bordered-items li:hover {
        background-color: #e3f2fd;
    }
</style>

<ul class="bordered-items">
    <li>First bordered item</li>
    <li>Second bordered item</li>
    <li>Third bordered item</li>
</ul>
```

### List Item with Multiple Paragraphs

```html
<ol>
    <li>
        <p style="margin: 0 0 5pt 0; font-weight: bold;">First Point</p>
        <p style="margin: 0 0 5pt 0;">This is the first paragraph explaining the point in detail.</p>
        <p style="margin: 0;">This is a second paragraph with additional information.</p>
    </li>
    <li>
        <p style="margin: 0 0 5pt 0; font-weight: bold;">Second Point</p>
        <p style="margin: 0;">Details about the second point.</p>
    </li>
</ol>
```

### Conditional List Items

```html
<!-- Template with model.showOptional = true -->
<ul>
    <li>Always visible item</li>
    <li hidden="{{model.showOptional ? '' : 'hidden'}}">
        Conditionally visible item
    </li>
    <li>Another always visible item</li>
</ul>
```

### Data-Bound List Items

```html
<!-- Template with model.items = ["Apple", "Banana", "Cherry"] -->
<ul>
    <template data-bind="{{model.items}}">
        <li>{{.}}</li>
    </template>
</ul>

<!-- Output:
• Apple
• Banana
• Cherry
-->
```

### Complex Data Binding with Styling

```html
<!-- Template with model.tasks = [{name: "Task 1", done: true}, ...] -->
<style>
    .task-done {
        color: #888;
        text-decoration: line-through;
    }
    .task-pending {
        color: #000;
        font-weight: bold;
    }
</style>

<ul>
    <template data-bind="{{model.tasks}}">
        <li class="{{.done ? 'task-done' : 'task-pending'}}"
            data-li-label="{{.done ? '✓' : '○'}}">
            {{.name}}
        </li>
    </template>
</ul>
```

### Numbered Items with Status Indicators

```html
<ol>
    <li style="color: green;">
        <span data-li-label="✓">Completed step</span>
    </li>
    <li style="color: orange;">
        <span data-li-label="⧗">In progress step</span>
    </li>
    <li style="color: gray;">
        <span data-li-label="○">Pending step</span>
    </li>
</ol>
```

### List Item with Code Block

```html
<ol>
    <li>
        Install the package:
        <pre style="background-color: #f5f5f5; padding: 10pt; margin: 5pt 0; border: 1pt solid #ddd;">
npm install scryber-pdf
        </pre>
    </li>
    <li>
        Import the library:
        <pre style="background-color: #f5f5f5; padding: 10pt; margin: 5pt 0; border: 1pt solid #ddd;">
const scryber = require('scryber-pdf');
        </pre>
    </li>
</ol>
```

### List Item with Inline Styles

```html
<ul>
    <li style="font-size: 14pt; color: #2c3e50;">
        Large blue text item
    </li>
    <li style="font-size: 10pt; color: #95a5a6; font-style: italic;">
        Small gray italic text item
    </li>
    <li style="font-weight: bold; text-transform: uppercase;">
        Bold uppercase text item
    </li>
</ul>
```

### Alternating Row Colors

```html
<style>
    .striped li:nth-child(odd) {
        background-color: #f9f9f9;
    }
    .striped li:nth-child(even) {
        background-color: #ffffff;
    }
    .striped li {
        padding: 8pt;
        border-bottom: 1pt solid #eee;
    }
</style>

<ul class="striped">
    <li>First item - light background</li>
    <li>Second item - white background</li>
    <li>Third item - light background</li>
    <li>Fourth item - white background</li>
</ul>
```

### List Item with Floating Content

```html
<ul>
    <li>
        <img src="images/thumbnail.jpg"
             style="float: left; width: 40pt; height: 40pt; margin-right: 10pt;" />
        <strong>Item with floating image</strong><br />
        The image floats to the left and text wraps around it nicely.
        This creates a visually appealing layout for list items.
    </li>
</ul>
```

### Multi-Line List Items with Indentation

```html
<ol>
    <li>
        <div style="margin-bottom: 5pt;">
            <strong>First Major Point</strong>
        </div>
        <div style="margin-left: 20pt;">
            Supporting detail that is indented to show it's a sub-point
            of the major point above. This helps with visual hierarchy.
        </div>
    </li>
    <li>
        <div style="margin-bottom: 5pt;">
            <strong>Second Major Point</strong>
        </div>
        <div style="margin-left: 20pt;">
            More supporting details with proper indentation.
        </div>
    </li>
</ol>
```

### List Item with Definition-Style Content

```html
<ul style="list-style-type: none;">
    <li style="margin-bottom: 15pt;">
        <strong style="font-size: 13pt; color: #2c3e50;">Term One</strong><br />
        <span style="margin-left: 20pt; display: block; margin-top: 3pt;">
            Definition or explanation of the first term with detailed information.
        </span>
    </li>
    <li style="margin-bottom: 15pt;">
        <strong style="font-size: 13pt; color: #2c3e50;">Term Two</strong><br />
        <span style="margin-left: 20pt; display: block; margin-top: 3pt;">
            Definition or explanation of the second term.
        </span>
    </li>
</ul>
```

### Interactive-Style Checklist

```html
<style>
    .checklist {
        list-style-type: none;
    }
    .checklist li {
        padding: 10pt;
        margin-bottom: 5pt;
        border: 1pt solid #ddd;
        border-radius: 3pt;
        background-color: #fff;
    }
    .checklist li[data-li-label="✓"] {
        background-color: #d4edda;
        border-color: #c3e6cb;
    }
</style>

<ul class="checklist">
    <li data-li-label="✓">Completed task with checkmark</li>
    <li data-li-label="○">Incomplete task</li>
    <li data-li-label="✓">Another completed task</li>
    <li data-li-label="○">Pending task</li>
</ul>
```

### Nested Content with Varying Insets

```html
<ol>
    <li>Level 1 content
        <ol style="list-style-type: lower-alpha;">
            <li>Level 2 content</li>
            <li data-li-inset="50pt">Level 2 with wider inset
                <ol style="list-style-type: lower-roman;">
                    <li>Level 3 content</li>
                    <li data-li-inset="70pt">Level 3 with custom inset</li>
                </ol>
            </li>
        </ol>
    </li>
</ol>
```

### List Item with Gradient Background

```html
<style>
    .gradient-items li {
        background: linear-gradient(to right, #667eea 0%, #764ba2 100%);
        color: white;
        padding: 12pt;
        margin-bottom: 8pt;
        border-radius: 4pt;
    }
</style>

<ul class="gradient-items">
    <li>Item with gradient background</li>
    <li>Another gradient item</li>
    <li>Third gradient item</li>
</ul>
```

### Priority-Based List Items

```html
<style>
    .priority-high {
        border-left: 4pt solid #e74c3c;
        padding-left: 10pt;
        font-weight: bold;
        color: #c0392b;
    }
    .priority-medium {
        border-left: 4pt solid #f39c12;
        padding-left: 10pt;
        color: #d68910;
    }
    .priority-low {
        border-left: 4pt solid #95a5a6;
        padding-left: 10pt;
        color: #7f8c8d;
    }
</style>

<ul>
    <li class="priority-high">High priority item - urgent action required</li>
    <li class="priority-medium">Medium priority item - important but not urgent</li>
    <li class="priority-low">Low priority item - nice to have</li>
</ul>
```

### List Item with Quotes

```html
<ol>
    <li>
        <strong>First principle:</strong>
        <blockquote style="margin: 10pt 0; padding: 10pt;
                           border-left: 3pt solid #ccc;
                           background-color: #f9f9f9;
                           font-style: italic;">
            "Quality is not an act, it is a habit." - Aristotle
        </blockquote>
    </li>
    <li>
        <strong>Second principle:</strong>
        <blockquote style="margin: 10pt 0; padding: 10pt;
                           border-left: 3pt solid #ccc;
                           background-color: #f9f9f9;
                           font-style: italic;">
            "Simplicity is the ultimate sophistication." - Leonardo da Vinci
        </blockquote>
    </li>
</ol>
```

---

## See Also

- [ul](/reference/htmltags/ul.html) - Unordered list element
- [ol](/reference/htmltags/ol.html) - Ordered list element
- [dl](/reference/htmltags/dl.html) - Definition list element
- [dt](/reference/htmltags/dt.html) - Definition term element
- [dd](/reference/htmltags/dd.html) - Definition description element
- [Lists Reference](/reference/components/lists.html) - Complete lists documentation
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and expressions

---
