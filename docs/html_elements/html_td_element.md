---
layout: default
title: td and th
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;td&gt; and &lt;th&gt; : Table Cell Elements

The `<td>` (table data) and `<th>` (table header) elements represent individual cells within a table row. They contain the actual content of the table and support spanning multiple columns, custom styling, alignment, and all standard CSS properties.

## Usage

Table cell elements create individual cells that:
- Contain text, images, or other content
- Can span multiple columns with `colspan`
- Support vertical and horizontal alignment
- Have default padding and borders
- Can be styled independently
- Support data binding for dynamic content
- `<th>` displays in **bold** by default (header cells)
- `<td>` uses normal font weight (data cells)

```html
<table>
    <tr>
        <th>Header Cell</th>
        <td>Data Cell</td>
        <td colspan="2">Cell spanning 2 columns</td>
    </tr>
</table>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the cell. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the cell. |

### Table-Specific Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `colspan` | integer | Number of columns this cell should span (default: 1). |
| `scope` | string | For `<th>` only: Indicates if header is for row, col, rowgroup, or colgroup. No effect on output. |

### Data Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-style-identifier` | string | Unique identifier for the cell's style set. Used internally for style caching. |

### CSS Style Support

**Box Model**:
- `width`, `min-width`, `max-width` - Cell width (as percentage or fixed units)
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `margin` (limited support - use cell spacing on table instead)

**Borders**:
- `border`, `border-width`, `border-color`, `border-style`
- `border-top`, `border-right`, `border-bottom`, `border-left`
- `border-corner-radius` - Rounded corners

**Alignment**:
- `text-align`: `left`, `center`, `right`, `justify`
- `vertical-align`: `top`, `middle`, `bottom`

**Visual Styling**:
- `background-color`, `background-image`
- `background-position`, `background-size`, `background-repeat`
- `color` - Text color
- `opacity`

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `text-decoration`, `text-transform`
- `line-height`, `letter-spacing`, `word-spacing`

**Content Overflow**:
- `overflow`: `visible`, `hidden`, `clip`
- `word-wrap`, `word-break` - Control text wrapping
- `white-space` - Control whitespace handling

---

## Cell Types

### &lt;td&gt; - Table Data Cell

Standard cells for table data:
- Normal font weight
- Used in `<tbody>` for data rows
- Can contain any content type

```html
<td style="padding: 8pt;">Standard data cell</td>
```

### &lt;th&gt; - Table Header Cell

Header cells for column/row labels:
- **Bold** font weight by default
- Typically used in `<thead>`
- Can be used in rows or columns as headers
- Supports `scope` attribute (informational only)

```html
<th style="padding: 8pt;">Column Header</th>
<th scope="col">Column Header with Scope</th>
```

---

## Notes

### Default Styling

Table cells have these default styles:
- **Padding**: 2pt on all sides
- **Margin**: 2pt on all sides
- **Border**: 1pt solid gray (#999999)
- **Vertical Alignment**: Middle
- **Display Mode**: Table cell
- **Overflow**: Clip (content clipped if too large)

Header cells (`<th>`) additionally have:
- **Font Weight**: Bold

### Column Spanning

Use `colspan` to make a cell span multiple columns:
```html
<tr>
    <td>Column 1</td>
    <td colspan="2">Spans columns 2 and 3</td>
</tr>
```

**Important**: Total colspan in a row should equal the table's column count.

### Row Spanning

**Note**: While HTML supports `rowspan`, the Scryber implementation does not expose this attribute in the HTML components. Use `colspan` for horizontal spanning.

### Cell Width Control

Cell widths can be specified in several ways:

**Percentage widths**:
```html
<td style="width: 30%;">30% of table width</td>
<td style="width: 70%;">70% of table width</td>
```

**Fixed widths**:
```html
<td style="width: 100pt;">Fixed 100 points</td>
<td style="width: 200pt;">Fixed 200 points</td>
```

**Auto widths**: Omit width to let cells size to content.

### Vertical Alignment

Control vertical positioning of content within cells:
```html
<td style="vertical-align: top;">Top aligned</td>
<td style="vertical-align: middle;">Middle aligned (default)</td>
<td style="vertical-align: bottom;">Bottom aligned</td>
```

### Content Overflow

Cells have `overflow: clip` by default, meaning content that doesn't fit will be clipped:
```html
<!-- Content will be clipped if too large -->
<td style="width: 100pt; overflow: clip;">
    Very long content that exceeds cell width will be clipped
</td>

<!-- Allow content to be visible -->
<td style="width: 100pt; overflow: visible;">
    Content may overflow cell boundaries
</td>
```

### Text Wrapping

Control how text wraps within cells:
```html
<td style="word-wrap: break-word;">
    LongWordsThatDontFitWillBeBreken
</td>

<td style="white-space: nowrap;">
    This text won't wrap to next line
</td>
```

---

## Examples

### Basic Table Cells

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #e0e0e0;">
                Name
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #e0e0e0;">
                Email
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">John Doe</td>
            <td style="border: 1pt solid black; padding: 8pt;">john@example.com</td>
        </tr>
    </tbody>
</table>
```

### Cells with Different Alignments

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid black; padding: 8pt; text-align: left;">
            Left aligned text
        </td>
        <td style="border: 1pt solid black; padding: 8pt; text-align: center;">
            Center aligned text
        </td>
        <td style="border: 1pt solid black; padding: 8pt; text-align: right;">
            Right aligned text
        </td>
    </tr>
    <tr>
        <td style="border: 1pt solid black; padding: 20pt; vertical-align: top;">
            Top aligned
        </td>
        <td style="border: 1pt solid black; padding: 20pt; vertical-align: middle;">
            Middle aligned (default)
        </td>
        <td style="border: 1pt solid black; padding: 20pt; vertical-align: bottom;">
            Bottom aligned
        </td>
    </tr>
</table>
```

### Cells with Column Spanning

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th colspan="3" style="border: 1pt solid black; padding: 10pt;
                                    background-color: #34495e; color: white;
                                    text-align: center; font-size: 14pt;">
                Sales Report - Q1 2024
            </th>
        </tr>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #95a5a6;">
                Month
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #95a5a6;">
                Revenue
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #95a5a6;">
                Growth
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">January</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">$42,000</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">5%</td>
        </tr>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">February</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">$45,000</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">7%</td>
        </tr>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">March</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">$48,000</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">7%</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt; font-weight: bold;">
                Total
            </td>
            <td colspan="2" style="border: 1pt solid black; padding: 8pt;
                                   text-align: right; font-weight: bold;">
                $135,000
            </td>
        </tr>
    </tfoot>
</table>
```

### Styled Cells with Colors

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="padding: 10pt; background-color: #2c3e50; color: white; border: 1pt solid white;">
                Status
            </th>
            <th style="padding: 10pt; background-color: #2c3e50; color: white; border: 1pt solid white;">
                Count
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="padding: 8pt; background-color: #2ecc71; color: white;
                       border: 1pt solid #27ae60; font-weight: bold;">
                Completed
            </td>
            <td style="padding: 8pt; background-color: #2ecc71; color: white;
                       border: 1pt solid #27ae60; text-align: center;">
                45
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; background-color: #f39c12; color: white;
                       border: 1pt solid #e67e22; font-weight: bold;">
                In Progress
            </td>
            <td style="padding: 8pt; background-color: #f39c12; color: white;
                       border: 1pt solid #e67e22; text-align: center;">
                23
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; background-color: #e74c3c; color: white;
                       border: 1pt solid #c0392b; font-weight: bold;">
                Pending
            </td>
            <td style="padding: 8pt; background-color: #e74c3c; color: white;
                       border: 1pt solid #c0392b; text-align: center;">
                12
            </td>
        </tr>
    </tbody>
</table>
```

### Cells with Different Widths

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="width: 10%; border: 1pt solid black; padding: 8pt;">ID</th>
            <th style="width: 40%; border: 1pt solid black; padding: 8pt;">Description</th>
            <th style="width: 20%; border: 1pt solid black; padding: 8pt;">Category</th>
            <th style="width: 15%; border: 1pt solid black; padding: 8pt;">Date</th>
            <th style="width: 15%; border: 1pt solid black; padding: 8pt;">Amount</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt; text-align: center;">001</td>
            <td style="border: 1pt solid black; padding: 8pt;">
                Premium Widget with Extended Warranty
            </td>
            <td style="border: 1pt solid black; padding: 8pt;">Electronics</td>
            <td style="border: 1pt solid black; padding: 8pt;">2024-01-15</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">$299.99</td>
        </tr>
    </tbody>
</table>
```

### Data-Bound Table Cells

```html
<!-- Model: {
    products: [
        {id: "P001", name: "Laptop", price: 1299.99, stock: 15},
        {id: "P002", name: "Mouse", price: 24.99, stock: 150},
        {id: "P003", name: "Keyboard", price: 79.99, stock: 75}
    ]
} -->
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #34495e; color: white;">
                Product ID
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #34495e; color: white;">
                Product Name
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #34495e; color: white;">
                Price
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #34495e; color: white;">
                Stock
            </th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.products}}">
            <tr>
                <td style="border: 1pt solid black; padding: 8pt; font-family: monospace;">
                    {{.id}}
                </td>
                <td style="border: 1pt solid black; padding: 8pt;">
                    {{.name}}
                </td>
                <td style="border: 1pt solid black; padding: 8pt; text-align: right;">
                    ${{.price}}
                </td>
                <td style="border: 1pt solid black; padding: 8pt; text-align: center;
                           color: {{.stock < 50 ? 'red' : 'green'}}; font-weight: bold;">
                    {{.stock}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### Cells with Custom Padding and Borders

```html
<table style="width: 100%; border-collapse: separate; border-spacing: 3pt;">
    <tr>
        <td style="border: 3pt solid #3498db; padding: 15pt;
                   background-color: #ebf5fb; border-radius: 5pt;">
            Cell with thick blue border and generous padding
        </td>
        <td style="border-left: 5pt solid #e74c3c; padding: 10pt 10pt 10pt 15pt;
                   background-color: #fadbd8;">
            Cell with thick left border accent
        </td>
    </tr>
</table>
```

### Cells with Images and Mixed Content

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid #ddd; padding: 10pt; width: 25%; text-align: center;">
            <img src="product.jpg" style="width: 80pt; height: 80pt;" />
        </td>
        <td style="border: 1pt solid #ddd; padding: 10pt; width: 75%;">
            <h3 style="margin: 0 0 5pt 0; color: #2c3e50;">Premium Product</h3>
            <p style="margin: 0 0 5pt 0; color: #7f8c8d; font-size: 9pt;">
                SKU: PROD-12345
            </p>
            <p style="margin: 0; font-size: 10pt; line-height: 1.5;">
                High-quality product with excellent features and outstanding
                customer reviews. Perfect for professional use.
            </p>
        </td>
    </tr>
</table>
```

### Numeric Data Cells with Formatting

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt; text-align: left;">Metric</th>
            <th style="border: 1pt solid black; padding: 8pt; text-align: right;">Q1</th>
            <th style="border: 1pt solid black; padding: 8pt; text-align: right;">Q2</th>
            <th style="border: 1pt solid black; padding: 8pt; text-align: right;">Q3</th>
            <th style="border: 1pt solid black; padding: 8pt; text-align: right;">Q4</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Revenue</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;
                       font-family: monospace;">$125,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;
                       font-family: monospace;">$145,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;
                       font-family: monospace;">$138,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;
                       font-family: monospace;">$167,000</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Growth %</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;
                       color: #27ae60;">+8.5%</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;
                       color: #27ae60;">+16.0%</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;
                       color: #e74c3c;">-4.8%</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;
                       color: #27ae60;">+21.0%</td>
        </tr>
    </tbody>
</table>
```

### Header Cells with Scope

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th scope="col" style="border: 1pt solid black; padding: 8pt;">Product</th>
            <th scope="col" style="border: 1pt solid black; padding: 8pt;">Price</th>
            <th scope="col" style="border: 1pt solid black; padding: 8pt;">Stock</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row" style="border: 1pt solid black; padding: 8pt; text-align: left;">
                Widget A
            </th>
            <td style="border: 1pt solid black; padding: 8pt;">$25.00</td>
            <td style="border: 1pt solid black; padding: 8pt;">100</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid black; padding: 8pt; text-align: left;">
                Widget B
            </th>
            <td style="border: 1pt solid black; padding: 8pt;">$45.00</td>
            <td style="border: 1pt solid black; padding: 8pt;">50</td>
        </tr>
    </tbody>
</table>
```

### Cells with Conditional Formatting

```html
<!-- Model: {
    scores: [
        {student: "Alice", math: 95, english: 88, science: 92},
        {student: "Bob", math: 72, english: 85, science: 78},
        {student: "Carol", math: 88, english: 91, science: 89}
    ]
} -->
<style>
    .grade-a { background-color: #d4edda; color: #155724; font-weight: bold; }
    .grade-b { background-color: #d1ecf1; color: #0c5460; }
    .grade-c { background-color: #fff3cd; color: #856404; }
</style>

<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt;">Student</th>
            <th style="border: 1pt solid black; padding: 8pt;">Math</th>
            <th style="border: 1pt solid black; padding: 8pt;">English</th>
            <th style="border: 1pt solid black; padding: 8pt;">Science</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.scores}}">
            <tr>
                <td style="border: 1pt solid black; padding: 8pt;">{{.student}}</td>
                <td style="border: 1pt solid black; padding: 8pt; text-align: center;"
                    class="{{.math >= 90 ? 'grade-a' : (.math >= 80 ? 'grade-b' : 'grade-c')}}">
                    {{.math}}
                </td>
                <td style="border: 1pt solid black; padding: 8pt; text-align: center;"
                    class="{{.english >= 90 ? 'grade-a' : (.english >= 80 ? 'grade-b' : 'grade-c')}}">
                    {{.english}}
                </td>
                <td style="border: 1pt solid black; padding: 8pt; text-align: center;"
                    class="{{.science >= 90 ? 'grade-a' : (.science >= 80 ? 'grade-b' : 'grade-c')}}">
                    {{.science}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### Cells with Text Wrapping Control

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid black; padding: 8pt; width: 150pt;
                   word-wrap: break-word;">
            ThisIsAVeryLongWordThatNeedsToBeWrappedWithinTheCellBoundaries
        </td>
        <td style="border: 1pt solid black; padding: 8pt; width: 150pt;
                   white-space: nowrap; overflow: hidden;">
            This text will not wrap and may be clipped
        </td>
    </tr>
</table>
```

### Complex Cell Layout

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid #ddd; padding: 0; width: 40%;">
            <div style="background-color: #3498db; color: white; padding: 10pt;">
                <h3 style="margin: 0;">Section Title</h3>
            </div>
            <div style="padding: 10pt;">
                <p style="margin: 5pt 0;">Content area with multiple elements</p>
                <ul style="margin: 5pt 0; padding-left: 15pt;">
                    <li>Feature one</li>
                    <li>Feature two</li>
                    <li>Feature three</li>
                </ul>
            </div>
        </td>
        <td style="border: 1pt solid #ddd; padding: 15pt; width: 60%;
                   vertical-align: top; background-color: #ecf0f1;">
            <p style="margin: 0 0 10pt 0; font-size: 11pt; line-height: 1.6;">
                This cell contains detailed description text with proper
                formatting and spacing. The content is top-aligned within
                the cell.
            </p>
            <p style="margin: 0; font-size: 9pt; color: #7f8c8d; font-style: italic;">
                Additional notes or metadata can be included here.
            </p>
        </td>
    </tr>
</table>
```

---

## See Also

- [table](/reference/htmltags/table.html) - Table element
- [tr](/reference/htmltags/tr.html) - Table row element
- [thead, tbody, tfoot](/reference/htmltags/table.html#table-sections) - Table sections
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Dynamic data binding
- [Template Element](/reference/htmltags/template.html) - Template for repeating content

---
