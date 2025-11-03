---
layout: default
title: vertical-align
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# vertical-align : Vertical Alignment Property

The `vertical-align` property specifies the vertical alignment of inline and table-cell elements relative to their container or baseline. This property is essential for controlling the vertical positioning of text, images, and other inline content in PDF documents.

## Usage

```css
/* Keyword values */
vertical-align: baseline;
vertical-align: top;
vertical-align: middle;
vertical-align: bottom;
vertical-align: text-top;
vertical-align: text-bottom;
vertical-align: sub;
vertical-align: super;
```

---

## Values

### Alignment Keywords

- **baseline** - Aligns the baseline of the element with the baseline of the parent (default)
- **top** - Aligns the top of the element with the top of the tallest element on the line (for inline) or cell (for table cells)
- **middle** - Aligns the vertical midpoint of the element with the baseline plus half the x-height of the parent
- **bottom** - Aligns the bottom of the element with the bottom of the line box (for inline) or cell (for table cells)
- **text-top** - Aligns the top of the element with the top of the parent element's font
- **text-bottom** - Aligns the bottom of the element with the bottom of the parent element's font
- **sub** - Aligns the baseline of the element with the subscript baseline of the parent
- **super** - Aligns the baseline of the element with the superscript baseline of the parent

### Default Value

- **baseline** - Default vertical alignment

---

## Notes

- Applies to inline elements, inline-block elements, and table cells
- Does not apply to block-level elements
- The `sub` and `super` values are commonly used for mathematical formulas and footnote references
- In table cells, `top`, `middle`, and `bottom` control the vertical position of cell content
- The `middle` alignment is particularly useful for centering content vertically in table cells
- Vertical alignment affects the baseline positioning for text mixing different font sizes
- Images and other replaced elements use the bottom of the element as the baseline by default

---

## Data Binding

The `vertical-align` property can be dynamically controlled through data binding, allowing vertical positioning to adapt based on content type, table configurations, or formatting preferences.

### Example 1: Dynamic Table Cell Alignment

```html
<table style="width: 100%; border-collapse: collapse">
    <tr style="height: 60pt">
        <td style="vertical-align: {{model.table.headerAlign}}; border: 1pt solid #ccc; padding: 8pt">
            {{model.headers.column1}}
        </td>
        <td style="vertical-align: {{model.table.dataAlign}}; border: 1pt solid #ccc; padding: 8pt">
            {{model.data.value1}}
        </td>
    </tr>
</table>

<!-- Data model example:
{
    "table": {
        "headerAlign": "middle",
        "dataAlign": "top"
    },
    "headers": {"column1": "Product Name"},
    "data": {"value1": "Premium Widget"}
}
-->
```

### Example 2: Mathematical Expression with Dynamic Positioning

```html
<p style="font-size: 14pt">
    x<span style="vertical-align: {{model.notation.exponentAlign}}; font-size: 10pt">{{model.notation.exponent}}</span> +
    y<span style="vertical-align: {{model.notation.subscriptAlign}}; font-size: 10pt">{{model.notation.subscript}}</span>
</p>

<!-- Data model example:
{
    "notation": {
        "exponent": "2",
        "exponentAlign": "super",
        "subscript": "0",
        "subscriptAlign": "sub"
    }
}
-->
```

### Example 3: Content Type-Based Alignment

```html
<div style="font-size: 12pt">
    <span style="vertical-align: {{model.element.alignment}}">{{model.element.text}}</span>
</div>

<!-- Data model example for footnote:
{
    "element": {
        "text": "1",
        "alignment": "super"
    }
}
-->
```

---

## Examples

### Example 1: Subscript Text

```html
<p style="font-size: 12pt">
    Chemical formula: H<span style="vertical-align: sub; font-size: 9pt">2</span>O
</p>
```

### Example 2: Superscript Text

```html
<p style="font-size: 12pt">
    Einstein's equation: E=mc<span style="vertical-align: super; font-size: 9pt">2</span>
</p>
```

### Example 3: Table Cell Vertical Alignment

```html
<table style="width: 100%; border-collapse: collapse">
    <tr style="height: 60pt">
        <td style="vertical-align: top; border: 1pt solid #ccc; padding: 8pt">
            Top-aligned content
        </td>
        <td style="vertical-align: middle; border: 1pt solid #ccc; padding: 8pt">
            Middle-aligned content
        </td>
        <td style="vertical-align: bottom; border: 1pt solid #ccc; padding: 8pt">
            Bottom-aligned content
        </td>
    </tr>
</table>
```

### Example 4: Mixed Font Sizes with Baseline Alignment

```html
<p style="font-size: 12pt">
    Normal text
    <span style="font-size: 18pt; vertical-align: baseline">larger text</span>
    on the same baseline
</p>
```

### Example 5: Mathematical Expressions

```html
<p style="font-size: 14pt">
    x<span style="vertical-align: super; font-size: 10pt">n</span> +
    y<span style="vertical-align: sub; font-size: 10pt">0</span> =
    z<span style="vertical-align: super; font-size: 10pt">2</span>
</p>
```

### Example 6: Footnote References

```html
<p style="font-size: 11pt">
    This statement requires citation<span style="vertical-align: super; font-size: 8pt; color: blue">1</span>
    and further explanation<span style="vertical-align: super; font-size: 8pt; color: blue">2</span>.
</p>
```

### Example 7: Invoice Table with Aligned Content

```html
<table style="width: 100%; border-collapse: collapse">
    <thead>
        <tr style="background-color: #f0f0f0">
            <th style="vertical-align: middle; padding: 10pt; border: 1pt solid #ccc">
                Item
            </th>
            <th style="vertical-align: middle; padding: 10pt; border: 1pt solid #ccc">
                Quantity
            </th>
            <th style="vertical-align: middle; padding: 10pt; border: 1pt solid #ccc">
                Price
            </th>
        </tr>
    </thead>
    <tbody>
        <tr style="height: 40pt">
            <td style="vertical-align: middle; padding: 8pt; border: 1pt solid #ccc">
                Product Name
            </td>
            <td style="vertical-align: middle; text-align: center; padding: 8pt; border: 1pt solid #ccc">
                5
            </td>
            <td style="vertical-align: middle; text-align: right; padding: 8pt; border: 1pt solid #ccc">
                $49.99
            </td>
        </tr>
    </tbody>
</table>
```

### Example 8: Image and Text Alignment

```html
<p style="font-size: 12pt">
    <img src="icon.png" style="width: 16pt; height: 16pt; vertical-align: middle"/>
    Icon aligned with text middle
</p>
```

### Example 9: Chemical Formulas

```html
<div style="font-size: 13pt">
    <p>
        Glucose: C<span style="vertical-align: sub; font-size: 10pt">6</span>H<span style="vertical-align: sub; font-size: 10pt">12</span>O<span style="vertical-align: sub; font-size: 10pt">6</span>
    </p>
    <p>
        Sulfate ion: SO<span style="vertical-align: sub; font-size: 10pt">4</span><span style="vertical-align: super; font-size: 10pt">2-</span>
    </p>
</div>
```

### Example 10: Data Table with Multi-line Cells

```html
<table style="width: 100%; border-collapse: collapse">
    <tr>
        <td style="vertical-align: top; width: 30%; padding: 10pt; border: 1pt solid #ddd">
            <strong>Description:</strong>
        </td>
        <td style="vertical-align: top; padding: 10pt; border: 1pt solid #ddd">
            This is a detailed description that spans multiple lines.
            The content is aligned to the top of the cell for better
            readability when cells have different heights.
        </td>
    </tr>
</table>
```

### Example 11: Price Tag with Superscript Cents

```html
<div style="font-size: 32pt; font-weight: bold">
    $29<span style="vertical-align: super; font-size: 16pt">.99</span>
</div>
```

### Example 12: Scientific Notation

```html
<p style="font-size: 12pt">
    Avogadro's number: 6.022 × 10<span style="vertical-align: super; font-size: 9pt">23</span> mol<span style="vertical-align: super; font-size: 9pt">-1</span>
</p>
```

### Example 13: Resume Table Layout

```html
<table style="width: 100%; border-collapse: collapse">
    <tr>
        <td style="vertical-align: top; width: 100pt; padding-right: 20pt">
            <strong>2020-2024</strong>
        </td>
        <td style="vertical-align: top">
            <strong>Senior Developer</strong><br/>
            Acme Corporation<br/>
            Led development of multiple projects and mentored junior developers.
        </td>
    </tr>
</table>
```

### Example 14: Ordinal Numbers

```html
<p style="font-size: 12pt">
    She finished 1<span style="vertical-align: super; font-size: 8pt">st</span> place,
    he finished 2<span style="vertical-align: super; font-size: 8pt">nd</span> place,
    and they finished 3<span style="vertical-align: super; font-size: 8pt">rd</span> place.
</p>
```

### Example 15: Complex Mathematical Formula

```html
<div style="font-size: 14pt; padding: 20pt">
    <p>
        The quadratic formula:
    </p>
    <p style="font-size: 16pt; text-align: center; margin: 20pt 0">
        x =
        <span style="display: inline-block; vertical-align: middle">
            -b ± √(b<span style="vertical-align: super; font-size: 12pt">2</span> - 4ac)
        </span>
        <span style="display: inline-block; vertical-align: middle; border-top: 1pt solid black; padding-top: 2pt">
            2a
        </span>
    </p>
</div>
```

---

## See Also

- [text-align](/reference/cssproperties/text-align) - Horizontal text alignment
- [line-height](/reference/cssproperties/line-height) - Line spacing
- [font-size](/reference/cssproperties/font-size) - Text size
- [padding](/reference/cssproperties/padding) - Element padding
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
