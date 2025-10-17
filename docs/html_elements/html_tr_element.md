---
layout: default
title: tr
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;tr&gt; : The Table Row Element

The `<tr>` element represents a row in a table. It contains table cells (`<td>` or `<th>`) and can be styled independently to control row appearance, colors, borders, and typography. Rows are kept together during page breaks and cannot be split across pages.

## Usage

The `<tr>` element creates a table row that:
- Contains table cells (`<td>` and/or `<th>`)
- Can be styled with background colors, borders, and typography
- Stays together on page breaks (never splits mid-row)
- Can repeat at the top of pages when in `<thead>`
- Supports data binding for dynamic content
- Applies styles to all contained cells

```html
<table>
    <tr style="background-color: #f0f0f0;">
        <td>Cell 1</td>
        <td>Cell 2</td>
        <td>Cell 3</td>
    </tr>
    <tr>
        <td>Cell 4</td>
        <td>Cell 5</td>
        <td>Cell 6</td>
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
| `title` | string | Sets the outline/bookmark title for the row. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the row. |

### Data Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-style-identifier` | string | Unique identifier for the row's style set. Used internally for style caching. |

### CSS Style Support

**Visual Styling**:
- `background-color`, `background-image`, `background-repeat`
- `background-opacity` - Transparency of background fill
- `color` - Text color (inherited by cells)

**Borders**:
- `border`, `border-width`, `border-color`, `border-style`
- `border-top`, `border-right`, `border-bottom`, `border-left`
- `border-sides` - Control which sides show borders
- `border-opacity` - Border transparency
- `border-dash` - Dashed or dotted borders
- `border-corner-radius` - Rounded corners

**Typography** (inherited by cells):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `font-bold`, `font-italic`

**Fill and Stroke**:
- `fill-color`, `fill-image`, `fill-repeat`, `fill-opacity`
- `stroke-color`, `stroke-width`, `stroke-dash`, `stroke-opacity`

### Note on Unsupported Styles

Table rows do **not** support:
- `margin` (margins do not apply to rows)
- `padding` (use cell padding instead)
- `position` (except `display: none` for hiding)
- `height` (row height is determined by cell content)
- `width` (row width is determined by table)
- `top`, `left`, `right`, `bottom` positioning

These styles are automatically removed if specified and will not affect the row.

---

## Row Types and Context

### Body Rows (in tbody)

Standard data rows in the table body:
```html
<tbody>
    <tr>
        <td>Standard row in body</td>
    </tr>
</tbody>
```

### Header Rows (in thead)

Header rows display in **bold** by default and **repeat at the top of each page**:
```html
<thead>
    <tr>
        <th>Column 1</th>
        <th>Column 2</th>
    </tr>
</thead>
```

### Footer Rows (in tfoot)

Footer rows appear at the bottom of the table:
```html
<tfoot>
    <tr>
        <td>Footer content</td>
    </tr>
</tfoot>
```

---

## Notes

### Default Behavior

Table rows have these characteristics:
1. **No Split**: Rows are kept together on page breaks (overflow-split: never)
2. **Auto Height**: Height is determined by cell content
3. **Full Width**: Spans the full width of the table
4. **Cell Inheritance**: Styles cascade to contained cells

### Page Breaking

Rows have special page break behavior:
- **Never Split**: A row will never be split across pages
- If a row doesn't fit on the current page, the entire row moves to the next page
- This ensures cell content stays together

```html
<!-- This entire row will stay together on one page -->
<tr>
    <td>This content</td>
    <td>will stay together</td>
</tr>
```

### Style Inheritance

Styles applied to rows cascade to cells:
```html
<tr style="color: blue; font-size: 12pt;">
    <td>This text is blue, 12pt</td>
    <td>This text is also blue, 12pt</td>
</tr>
```

Cell styles override row styles:
```html
<tr style="color: blue;">
    <td>Blue text</td>
    <td style="color: red;">Red text (overrides row)</td>
</tr>
```

### Alternating Row Colors

Create striped tables using nth-child CSS selectors:
```html
<style>
    tr:nth-child(odd) {
        background-color: #ffffff;
    }
    tr:nth-child(even) {
        background-color: #f0f0f0;
    }
</style>
```

---

## Examples

### Basic Row Styling

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr style="background-color: #336699; color: white;">
        <td style="padding: 8pt; border: 1pt solid black;">Header Cell 1</td>
        <td style="padding: 8pt; border: 1pt solid black;">Header Cell 2</td>
    </tr>
    <tr style="background-color: #f9f9f9;">
        <td style="padding: 8pt; border: 1pt solid black;">Data Cell 1</td>
        <td style="padding: 8pt; border: 1pt solid black;">Data Cell 2</td>
    </tr>
    <tr>
        <td style="padding: 8pt; border: 1pt solid black;">Data Cell 3</td>
        <td style="padding: 8pt; border: 1pt solid black;">Data Cell 4</td>
    </tr>
</table>
```

### Striped Rows with CSS Classes

```html
<style>
    .table-striped tr {
        border-bottom: 1pt solid #ddd;
    }
    .table-striped tr:nth-child(odd) {
        background-color: #ffffff;
    }
    .table-striped tr:nth-child(even) {
        background-color: #f5f5f5;
    }
    .table-striped tr:hover {
        background-color: #e8f4f8;
    }
</style>

<table class="table-striped" style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #2c3e50; color: white; font-weight: bold;">
            <th style="padding: 10pt; text-align: left;">Name</th>
            <th style="padding: 10pt; text-align: left;">Department</th>
            <th style="padding: 10pt; text-align: left;">Role</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="padding: 8pt;">Alice Johnson</td>
            <td style="padding: 8pt;">Engineering</td>
            <td style="padding: 8pt;">Senior Developer</td>
        </tr>
        <tr>
            <td style="padding: 8pt;">Bob Smith</td>
            <td style="padding: 8pt;">Marketing</td>
            <td style="padding: 8pt;">Marketing Manager</td>
        </tr>
        <tr>
            <td style="padding: 8pt;">Carol White</td>
            <td style="padding: 8pt;">Sales</td>
            <td style="padding: 8pt;">Sales Representative</td>
        </tr>
    </tbody>
</table>
```

### Row with Border Styling

```html
<table style="width: 100%; border-collapse: separate; border-spacing: 2pt;">
    <tr style="border: 2pt solid #336699; border-radius: 5pt;
               background-color: #e8f4f8;">
        <td style="padding: 10pt;">This row has a border</td>
        <td style="padding: 10pt;">with rounded corners</td>
    </tr>
    <tr style="border-bottom: 3pt double #999999;">
        <td style="padding: 10pt;">This row has</td>
        <td style="padding: 10pt;">a double bottom border</td>
    </tr>
</table>
```

### Repeating Header Rows

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <!-- This row will repeat at the top of each page -->
        <tr style="background-color: #2c3e50; color: white;">
            <th style="padding: 8pt; border: 1pt solid white;">Product</th>
            <th style="padding: 8pt; border: 1pt solid white;">Description</th>
            <th style="padding: 8pt; border: 1pt solid white;">Price</th>
        </tr>
    </thead>
    <tbody>
        <!-- Many rows that span multiple pages -->
        <tr>
            <td style="padding: 8pt; border: 1pt solid #ddd;">Widget A</td>
            <td style="padding: 8pt; border: 1pt solid #ddd;">Standard widget</td>
            <td style="padding: 8pt; border: 1pt solid #ddd;">$10.00</td>
        </tr>
        <!-- More rows... -->
    </tbody>
</table>
```

### Data-Bound Dynamic Rows

```html
<!-- Model: {
    employees: [
        {name: "Alice", dept: "Engineering", salary: 95000},
        {name: "Bob", dept: "Marketing", salary: 75000},
        {name: "Carol", dept: "Sales", salary: 85000}
    ]
} -->
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #34495e; color: white;">
            <th style="padding: 8pt; border: 1pt solid white;">Employee Name</th>
            <th style="padding: 8pt; border: 1pt solid white;">Department</th>
            <th style="padding: 8pt; border: 1pt solid white;">Salary</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.employees}}">
            <tr style="border-bottom: 1pt solid #ddd;">
                <td style="padding: 8pt;">{{.name}}</td>
                <td style="padding: 8pt;">{{.dept}}</td>
                <td style="padding: 8pt; text-align: right;">${{.salary}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### Conditional Row Styling

```html
<!-- Model: {
    transactions: [
        {id: 1, amount: 1500, status: "completed"},
        {id: 2, amount: 800, status: "pending"},
        {id: 3, amount: 2200, status: "completed"}
    ]
} -->
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #2c3e50; color: white;">
            <th style="padding: 8pt;">ID</th>
            <th style="padding: 8pt;">Amount</th>
            <th style="padding: 8pt;">Status</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.transactions}}">
            <tr style="background-color: {{.status == 'completed' ? '#d4edda' : '#fff3cd'}};
                       border-left: 4pt solid {{.status == 'completed' ? '#28a745' : '#ffc107'}};">
                <td style="padding: 8pt;">{{.id}}</td>
                <td style="padding: 8pt; text-align: right;">${{.amount}}</td>
                <td style="padding: 8pt; color: {{.status == 'completed' ? '#155724' : '#856404'}};">
                    {{.status}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### Row with Typography Styling

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr style="font-family: 'Arial'; font-size: 14pt; font-weight: bold;
               background-color: #34495e; color: white;">
        <td style="padding: 10pt;">Large Bold Header Row</td>
    </tr>
    <tr style="font-family: 'Georgia'; font-size: 10pt; font-style: italic;">
        <td style="padding: 8pt;">This row uses Georgia font in italic</td>
    </tr>
    <tr style="font-family: 'Courier New'; font-size: 9pt;">
        <td style="padding: 8pt;">Monospace font for code-like content</td>
    </tr>
</table>
```

### Highlighted Row

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="padding: 8pt; border: 1pt solid #ddd;">Normal row</td>
        <td style="padding: 8pt; border: 1pt solid #ddd;">Standard styling</td>
    </tr>
    <tr style="background-color: #fff3cd; border-left: 5pt solid #ffc107;">
        <td style="padding: 8pt; border: 1pt solid #ddd; font-weight: bold;">
            Highlighted row
        </td>
        <td style="padding: 8pt; border: 1pt solid #ddd;">
            This row stands out with background color and left border
        </td>
    </tr>
    <tr>
        <td style="padding: 8pt; border: 1pt solid #ddd;">Normal row</td>
        <td style="padding: 8pt; border: 1pt solid #ddd;">Standard styling</td>
    </tr>
</table>
```

### Row with Background Image

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr style="background-image: url('watermark.png');
               background-repeat: no-repeat;
               background-position: center;">
        <td style="padding: 20pt; font-size: 14pt; font-weight: bold;">
            Row with background image
        </td>
        <td style="padding: 20pt;">
            Content over the background
        </td>
    </tr>
</table>
```

### Grouped Rows with Subtotals

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #2c3e50; color: white;">
            <th style="padding: 8pt; border: 1pt solid white;">Item</th>
            <th style="padding: 8pt; border: 1pt solid white; text-align: right;">Amount</th>
        </tr>
    </thead>
    <tbody>
        <!-- Group 1 -->
        <tr style="background-color: #ecf0f1;">
            <td colspan="2" style="padding: 8pt; font-weight: bold; border: 1pt solid #ddd;">
                Category: Electronics
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; padding-left: 20pt; border: 1pt solid #ddd;">Laptop</td>
            <td style="padding: 8pt; text-align: right; border: 1pt solid #ddd;">$1,200</td>
        </tr>
        <tr>
            <td style="padding: 8pt; padding-left: 20pt; border: 1pt solid #ddd;">Monitor</td>
            <td style="padding: 8pt; text-align: right; border: 1pt solid #ddd;">$300</td>
        </tr>
        <tr style="background-color: #d5dbdb; font-weight: bold;">
            <td style="padding: 8pt; padding-left: 20pt; border: 1pt solid #ddd;">Subtotal</td>
            <td style="padding: 8pt; text-align: right; border: 1pt solid #ddd;">$1,500</td>
        </tr>

        <!-- Group 2 -->
        <tr style="background-color: #ecf0f1;">
            <td colspan="2" style="padding: 8pt; font-weight: bold; border: 1pt solid #ddd;">
                Category: Office Supplies
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt; padding-left: 20pt; border: 1pt solid #ddd;">Paper</td>
            <td style="padding: 8pt; text-align: right; border: 1pt solid #ddd;">$25</td>
        </tr>
        <tr style="background-color: #d5dbdb; font-weight: bold;">
            <td style="padding: 8pt; padding-left: 20pt; border: 1pt solid #ddd;">Subtotal</td>
            <td style="padding: 8pt; text-align: right; border: 1pt solid #ddd;">$25</td>
        </tr>
    </tbody>
    <tfoot>
        <tr style="background-color: #34495e; color: white; font-weight: bold; font-size: 12pt;">
            <td style="padding: 10pt; border: 1pt solid white;">Grand Total</td>
            <td style="padding: 10pt; text-align: right; border: 1pt solid white;">$1,525</td>
        </tr>
    </tfoot>
</table>
```

### Hidden Row

```html
<table style="width: 100%;">
    <tr>
        <td>Visible row</td>
    </tr>
    <tr hidden="hidden">
        <td>This row is hidden</td>
    </tr>
    <tr style="display: none;">
        <td>This row is also hidden</td>
    </tr>
    <tr>
        <td>Another visible row</td>
    </tr>
</table>
```

---

## See Also

- [table](/reference/htmltags/table.html) - Table element
- [td](/reference/htmltags/td.html) - Table cell elements (td and th)
- [thead, tbody, tfoot](/reference/htmltags/table.html#table-sections) - Table sections
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Dynamic data binding

---
