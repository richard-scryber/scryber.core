---
layout: default
title: cellpadding and cellspacing
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @cellpadding and @cellspacing : Table Spacing Attributes

The `cellpadding` and `cellspacing` attributes control the spacing within and between table cells in PDF documents. These attributes provide precise control over table layout, affecting readability and visual presentation. They are legacy HTML attributes that remain useful for quick table spacing adjustments.

## Usage

These attributes control table cell spacing:
- **`cellpadding`**: Space between cell content and cell borders (inner spacing)
- **`cellspacing`**: Space between adjacent cells (outer spacing)
- Accept numeric values in points
- Applied to `<table>` elements
- Affect all cells in the table
- Can be overridden by CSS padding and border-spacing
- Useful for quick table layout adjustments

```html
<!-- Table with padding and spacing -->
<table cellpadding="10" cellspacing="5" style="border: 1pt solid black;">
    <tr>
        <td style="border: 1pt solid black;">Cell 1</td>
        <td style="border: 1pt solid black;">Cell 2</td>
    </tr>
</table>

<!-- Table with padding only -->
<table cellpadding="15" cellspacing="0" style="border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid black;">Cell 1</td>
        <td style="border: 1pt solid black;">Cell 2</td>
    </tr>
</table>
```

---

## Supported Elements

These attributes are supported by the following element:

| Element | Description |
|---------|-------------|
| `<table>` | Table element for structured data |

---

## Cellpadding Attribute

### Description

The `cellpadding` attribute sets the space between cell content and the cell border:
- Measured in points (pt) by default
- Applies to all four sides of the cell (top, right, bottom, left)
- Affects all cells in the table uniformly
- Increases readability by adding breathing room around content
- Similar to CSS `padding` property

### Syntax

```html
<table cellpadding="value">
```

Where `value` is a number representing points.

### Examples

```html
<!-- No padding -->
<table cellpadding="0">...</table>

<!-- Small padding (5pt) -->
<table cellpadding="5">...</table>

<!-- Medium padding (10pt) -->
<table cellpadding="10">...</table>

<!-- Large padding (20pt) -->
<table cellpadding="20">...</table>
```

### Visual Effect

```html
<!-- cellpadding="5" -->
┌─────────────────┐
│ ····Content···· │  (5pt padding on all sides)
└─────────────────┘

<!-- cellpadding="15" -->
┌─────────────────┐
│                 │
│    Content      │  (15pt padding on all sides)
│                 │
└─────────────────┘
```

---

## Cellspacing Attribute

### Description

The `cellspacing` attribute sets the space between adjacent cells:
- Measured in points (pt) by default
- Creates gaps between cell borders
- Affects all cells in the table uniformly
- When set to 0, cells touch each other
- Similar to CSS `border-spacing` property

### Syntax

```html
<table cellspacing="value">
```

Where `value` is a number representing points.

### Examples

```html
<!-- No spacing (cells touch) -->
<table cellspacing="0">...</table>

<!-- Small spacing (2pt) -->
<table cellspacing="2">...</table>

<!-- Medium spacing (5pt) -->
<table cellspacing="5">...</table>

<!-- Large spacing (10pt) -->
<table cellspacing="10">...</table>
```

### Visual Effect

```html
<!-- cellspacing="0" -->
┌───┬───┬───┐
│ A │ B │ C │  (No gaps between cells)
└───┴───┴───┘

<!-- cellspacing="5" -->
┌───┐ ┌───┐ ┌───┐
│ A │ │ B │ │ C │  (5pt gaps between cells)
└───┘ └───┘ └───┘
```

---

## Default Values

### Scryber Default Spacing

Tables in Scryber have these defaults:
- **cellpadding**: `2pt` (default padding)
- **cellspacing**: `2pt` (default spacing)
- **cell borders**: `1pt solid gray (#999999)`
- **cell margins**: `2pt` (default margin)

To remove default spacing:

```html
<table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
    <!-- Cells will have no padding or spacing -->
</table>
```

---

## CSS Equivalents

### Cellpadding to CSS

The `cellpadding` attribute can be replaced with CSS:

```html
<!-- Using cellpadding attribute -->
<table cellpadding="10">
    <td>Content</td>
</table>

<!-- Equivalent CSS -->
<table>
    <td style="padding: 10pt;">Content</td>
</table>

<!-- Or apply to all cells with CSS -->
<style>
    table td {
        padding: 10pt;
    }
</style>
<table>
    <td>Content</td>
</table>
```

### Cellspacing to CSS

The `cellspacing` attribute can be replaced with CSS:

```html
<!-- Using cellspacing attribute -->
<table cellspacing="5">...</table>

<!-- Equivalent CSS -->
<table style="border-spacing: 5pt;">...</table>

<!-- For collapsed borders (no spacing) -->
<table style="border-collapse: collapse;">...</table>
```

---

## Combining with Border-Collapse

The `border-collapse` CSS property affects how cellspacing works:

### Separate Borders (default)

```html
<table cellpadding="8" cellspacing="5" style="border-collapse: separate;">
    <!-- cellspacing creates visible gaps between cells -->
</table>
```

### Collapsed Borders

```html
<table cellpadding="8" cellspacing="0" style="border-collapse: collapse;">
    <!-- cellspacing is ignored; borders merge together -->
</table>
```

**Note**: When `border-collapse: collapse` is used, the `cellspacing` attribute has no effect.

---

## Binding Values

Both attributes can be set statically or dynamically:

### Static Values

```html
<table cellpadding="10" cellspacing="5">
    <tr>
        <td>Cell content</td>
    </tr>
</table>
```

### Dynamic Values with Data Binding

```html
<!-- Model: { tablePadding: 12, tableSpacing: 4 } -->
<table cellpadding="{{model.tablePadding}}" cellspacing="{{model.tableSpacing}}">
    <tr>
        <td>Cell content</td>
    </tr>
</table>
```

### Conditional Values

```html
<!-- Model: { compact: true } -->
<table cellpadding="{{model.compact ? '5' : '12'}}"
       cellspacing="{{model.compact ? '0' : '5'}}">
    <tr>
        <td>Cell content</td>
    </tr>
</table>
```

---

## Notes

### When to Use Cellpadding

Use `cellpadding` when:
- You need uniform padding for all cells
- Quick prototyping or simple tables
- Consistent spacing across the entire table
- Working with legacy HTML templates

**Modern Alternative**: Use CSS `padding` on individual cells or cell groups for more control.

### When to Use Cellspacing

Use `cellspacing` when:
- You want visible gaps between cells
- Creating separated table layouts
- Adding space between bordered cells
- Working with legacy HTML templates

**Modern Alternative**: Use CSS `border-spacing` property or `border-collapse: separate`.

### Limitations

1. **No per-cell control**: Applies to all cells uniformly
2. **All sides equally**: Cannot set different values for top/right/bottom/left
3. **CSS is more flexible**: CSS padding and border-spacing offer more options
4. **Legacy attribute**: Part of older HTML specifications

### Performance Considerations

Both attributes are processed quickly and have no performance impact on PDF generation.

### Border Collapse Interaction

Understanding the interaction:

```html
<!-- Separate borders: spacing visible -->
<table cellspacing="10" style="border-collapse: separate;">
    <!-- 10pt gaps between cells -->
</table>

<!-- Collapsed borders: spacing ignored -->
<table cellspacing="10" style="border-collapse: collapse;">
    <!-- cellspacing has no effect; borders merge -->
</table>
```

### Mixing Attributes and CSS

You can combine these attributes with CSS for fine control:

```html
<table cellpadding="10" cellspacing="5" style="border: 2pt solid #336699;">
    <tr>
        <td style="padding-left: 20pt;">Left-padded cell</td>
        <td style="padding: 15pt;">Custom padding</td>
    </tr>
</table>
```

**Note**: CSS padding on individual cells will override cellpadding for those cells.

---

## Examples

### Basic Cellpadding Examples

```html
<!-- No padding (tight cells) -->
<table cellpadding="0" style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid black;">Tight</td>
        <td style="border: 1pt solid black;">Content</td>
        <td style="border: 1pt solid black;">Here</td>
    </tr>
</table>

<!-- Small padding (5pt) -->
<table cellpadding="5" style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid black;">Small</td>
        <td style="border: 1pt solid black;">Padding</td>
        <td style="border: 1pt solid black;">Cells</td>
    </tr>
</table>

<!-- Medium padding (10pt) -->
<table cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid black;">Medium</td>
        <td style="border: 1pt solid black;">Padding</td>
        <td style="border: 1pt solid black;">Cells</td>
    </tr>
</table>

<!-- Large padding (20pt) -->
<table cellpadding="20" style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid black;">Large</td>
        <td style="border: 1pt solid black;">Padding</td>
        <td style="border: 1pt solid black;">Cells</td>
    </tr>
</table>
```

### Basic Cellspacing Examples

```html
<!-- No spacing (cells touch) -->
<table cellspacing="0" cellpadding="8" style="width: 100%;">
    <tr>
        <td style="border: 1pt solid black;">No</td>
        <td style="border: 1pt solid black;">Space</td>
        <td style="border: 1pt solid black;">Between</td>
    </tr>
</table>

<!-- Small spacing (2pt) -->
<table cellspacing="2" cellpadding="8" style="width: 100%;">
    <tr>
        <td style="border: 1pt solid black;">Small</td>
        <td style="border: 1pt solid black;">Gaps</td>
        <td style="border: 1pt solid black;">Between</td>
    </tr>
</table>

<!-- Medium spacing (5pt) -->
<table cellspacing="5" cellpadding="8" style="width: 100%;">
    <tr>
        <td style="border: 1pt solid black;">Medium</td>
        <td style="border: 1pt solid black;">Gaps</td>
        <td style="border: 1pt solid black;">Between</td>
    </tr>
</table>

<!-- Large spacing (10pt) -->
<table cellspacing="10" cellpadding="8" style="width: 100%;">
    <tr>
        <td style="border: 1pt solid black;">Large</td>
        <td style="border: 1pt solid black;">Gaps</td>
        <td style="border: 1pt solid black;">Between</td>
    </tr>
</table>
```

### Combined Cellpadding and Cellspacing

```html
<!-- Tight table (no padding or spacing) -->
<table cellpadding="0" cellspacing="0" style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; background-color: #e0e0e0;">ID</th>
            <th style="border: 1pt solid black; background-color: #e0e0e0;">Name</th>
            <th style="border: 1pt solid black; background-color: #e0e0e0;">Email</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black;">001</td>
            <td style="border: 1pt solid black;">John Doe</td>
            <td style="border: 1pt solid black;">john@example.com</td>
        </tr>
        <tr>
            <td style="border: 1pt solid black;">002</td>
            <td style="border: 1pt solid black;">Jane Smith</td>
            <td style="border: 1pt solid black;">jane@example.com</td>
        </tr>
    </tbody>
</table>

<!-- Comfortable table (padding but no spacing) -->
<table cellpadding="10" cellspacing="0" style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">ID</th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">Name</th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">Email</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black;">001</td>
            <td style="border: 1pt solid black;">John Doe</td>
            <td style="border: 1pt solid black;">john@example.com</td>
        </tr>
        <tr>
            <td style="border: 1pt solid black;">002</td>
            <td style="border: 1pt solid black;">Jane Smith</td>
            <td style="border: 1pt solid black;">jane@example.com</td>
        </tr>
    </tbody>
</table>

<!-- Spacious table (both padding and spacing) -->
<table cellpadding="12" cellspacing="5" style="width: 100%; border-collapse: separate;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">ID</th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">Name</th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">Email</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black; background-color: #f9f9f9;">001</td>
            <td style="border: 1pt solid black; background-color: #f9f9f9;">John Doe</td>
            <td style="border: 1pt solid black; background-color: #f9f9f9;">john@example.com</td>
        </tr>
        <tr>
            <td style="border: 1pt solid black;">002</td>
            <td style="border: 1pt solid black;">Jane Smith</td>
            <td style="border: 1pt solid black;">jane@example.com</td>
        </tr>
    </tbody>
</table>
```

### Data Table with Optimal Spacing

```html
<table cellpadding="10" cellspacing="0"
       style="width: 100%; border-collapse: collapse; font-size: 10pt;">
    <thead>
        <tr>
            <th style="border: 1pt solid #dddddd; padding: 12pt;
                       background-color: #336699; color: white; text-align: left;">
                Product Name
            </th>
            <th style="border: 1pt solid #dddddd; padding: 12pt;
                       background-color: #336699; color: white; text-align: center;">
                Quantity
            </th>
            <th style="border: 1pt solid #dddddd; padding: 12pt;
                       background-color: #336699; color: white; text-align: right;">
                Unit Price
            </th>
            <th style="border: 1pt solid #dddddd; padding: 12pt;
                       background-color: #336699; color: white; text-align: right;">
                Total
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #dddddd;">Premium Widget</td>
            <td style="border: 1pt solid #dddddd; text-align: center;">10</td>
            <td style="border: 1pt solid #dddddd; text-align: right;">$25.00</td>
            <td style="border: 1pt solid #dddddd; text-align: right; font-weight: bold;">$250.00</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #dddddd; background-color: #f9f9f9;">Standard Gadget</td>
            <td style="border: 1pt solid #dddddd; background-color: #f9f9f9; text-align: center;">5</td>
            <td style="border: 1pt solid #dddddd; background-color: #f9f9f9; text-align: right;">$50.00</td>
            <td style="border: 1pt solid #dddddd; background-color: #f9f9f9;
                       text-align: right; font-weight: bold;">$250.00</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #dddddd;">Deluxe Tool</td>
            <td style="border: 1pt solid #dddddd; text-align: center;">3</td>
            <td style="border: 1pt solid #dddddd; text-align: right;">$75.00</td>
            <td style="border: 1pt solid #dddddd; text-align: right; font-weight: bold;">$225.00</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="3" style="border: 2pt solid #336699; padding: 12pt;
                                   text-align: right; font-weight: bold; font-size: 12pt;">
                Grand Total:
            </td>
            <td style="border: 2pt solid #336699; padding: 12pt; text-align: right;
                       font-weight: bold; font-size: 12pt; background-color: #e3f2fd;">
                $725.00
            </td>
        </tr>
    </tfoot>
</table>
```

### Calendar-Style Table with Spacing

```html
<table cellpadding="15" cellspacing="3"
       style="width: 100%; border-collapse: separate; font-size: 10pt;">
    <thead>
        <tr>
            <th style="border: 1pt solid #cccccc; background-color: #336699;
                       color: white; text-align: center;">Sun</th>
            <th style="border: 1pt solid #cccccc; background-color: #336699;
                       color: white; text-align: center;">Mon</th>
            <th style="border: 1pt solid #cccccc; background-color: #336699;
                       color: white; text-align: center;">Tue</th>
            <th style="border: 1pt solid #cccccc; background-color: #336699;
                       color: white; text-align: center;">Wed</th>
            <th style="border: 1pt solid #cccccc; background-color: #336699;
                       color: white; text-align: center;">Thu</th>
            <th style="border: 1pt solid #cccccc; background-color: #336699;
                       color: white; text-align: center;">Fri</th>
            <th style="border: 1pt solid #cccccc; background-color: #336699;
                       color: white; text-align: center;">Sat</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #cccccc; text-align: center; color: #999999;">30</td>
            <td style="border: 1pt solid #cccccc; text-align: center; color: #999999;">31</td>
            <td style="border: 1pt solid #cccccc; text-align: center;
                       background-color: #fff3cd; font-weight: bold;">1</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">2</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">3</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">4</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">5</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #cccccc; text-align: center;">6</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">7</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">8</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">9</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">10</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">11</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">12</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #cccccc; text-align: center;">13</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">14</td>
            <td style="border: 1pt solid #cccccc; text-align: center;
                       background-color: #d4edda; font-weight: bold;">15</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">16</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">17</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">18</td>
            <td style="border: 1pt solid #cccccc; text-align: center;">19</td>
        </tr>
    </tbody>
</table>
```

### Comparison Table with Different Spacing

```html
<h3 style="color: #336699;">Spacing Comparison</h3>

<!-- Tight spacing -->
<h4>Tight (padding=5, spacing=0)</h4>
<table cellpadding="5" cellspacing="0"
       style="width: 100%; border-collapse: collapse; margin-bottom: 20pt;">
    <tr>
        <td style="border: 1pt solid black; background-color: #e0e0e0;">Tight cell</td>
        <td style="border: 1pt solid black;">Content here</td>
        <td style="border: 1pt solid black;">More content</td>
    </tr>
</table>

<!-- Medium spacing -->
<h4>Medium (padding=10, spacing=0)</h4>
<table cellpadding="10" cellspacing="0"
       style="width: 100%; border-collapse: collapse; margin-bottom: 20pt;">
    <tr>
        <td style="border: 1pt solid black; background-color: #e0e0e0;">Medium cell</td>
        <td style="border: 1pt solid black;">Content here</td>
        <td style="border: 1pt solid black;">More content</td>
    </tr>
</table>

<!-- Spacious -->
<h4>Spacious (padding=15, spacing=5)</h4>
<table cellpadding="15" cellspacing="5"
       style="width: 100%; border-collapse: separate; margin-bottom: 20pt;">
    <tr>
        <td style="border: 1pt solid black; background-color: #e0e0e0;">Spacious cell</td>
        <td style="border: 1pt solid black;">Content here</td>
        <td style="border: 1pt solid black;">More content</td>
    </tr>
</table>
```

### Invoice Table with Professional Spacing

```html
<table cellpadding="10" cellspacing="0"
       style="width: 100%; border-collapse: collapse; font-size: 11pt;">
    <thead>
        <tr>
            <th style="border-bottom: 2pt solid #336699; padding: 12pt;
                       text-align: left; color: #336699; font-size: 12pt;">
                Description
            </th>
            <th style="border-bottom: 2pt solid #336699; padding: 12pt;
                       text-align: center; color: #336699; font-size: 12pt;">
                Qty
            </th>
            <th style="border-bottom: 2pt solid #336699; padding: 12pt;
                       text-align: right; color: #336699; font-size: 12pt;">
                Rate
            </th>
            <th style="border-bottom: 2pt solid #336699; padding: 12pt;
                       text-align: right; color: #336699; font-size: 12pt;">
                Amount
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt;">
                Web Development Services
            </td>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt; text-align: center;">
                40 hrs
            </td>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt;
                       text-align: right; font-family: monospace;">
                $150.00
            </td>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt;
                       text-align: right; font-weight: bold; font-family: monospace;">
                $6,000.00
            </td>
        </tr>
        <tr>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt;">
                Design Consultation
            </td>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt; text-align: center;">
                10 hrs
            </td>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt;
                       text-align: right; font-family: monospace;">
                $120.00
            </td>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt;
                       text-align: right; font-weight: bold; font-family: monospace;">
                $1,200.00
            </td>
        </tr>
        <tr>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt;">
                Server Setup and Configuration
            </td>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt; text-align: center;">
                5 hrs
            </td>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt;
                       text-align: right; font-family: monospace;">
                $100.00
            </td>
            <td style="border-bottom: 1pt solid #e0e0e0; padding: 10pt;
                       text-align: right; font-weight: bold; font-family: monospace;">
                $500.00
            </td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="3" style="padding: 12pt; text-align: right;
                                   font-size: 13pt; font-weight: bold;">
                Subtotal:
            </td>
            <td style="padding: 12pt; text-align: right; font-size: 13pt;
                       font-weight: bold; font-family: monospace;">
                $7,700.00
            </td>
        </tr>
        <tr>
            <td colspan="3" style="padding: 12pt; text-align: right; font-size: 13pt;">
                Tax (8%):
            </td>
            <td style="padding: 12pt; text-align: right; font-size: 13pt;
                       font-family: monospace;">
                $616.00
            </td>
        </tr>
        <tr>
            <td colspan="3" style="border-top: 2pt solid #336699; padding: 12pt;
                                   text-align: right; font-size: 14pt; font-weight: bold;
                                   color: #336699;">
                TOTAL:
            </td>
            <td style="border-top: 2pt solid #336699; padding: 12pt; text-align: right;
                       font-size: 14pt; font-weight: bold; font-family: monospace;
                       background-color: #e3f2fd; color: #336699;">
                $8,316.00
            </td>
        </tr>
    </tfoot>
</table>
```

### Report Table with Alternating Row Spacing

```html
<table cellpadding="12" cellspacing="0"
       style="width: 100%; border-collapse: collapse; font-size: 10pt;">
    <thead>
        <tr>
            <th style="border: 1pt solid #dddddd; background-color: #2c3e50;
                       color: white; padding: 14pt; text-align: left;">
                Employee Name
            </th>
            <th style="border: 1pt solid #dddddd; background-color: #2c3e50;
                       color: white; padding: 14pt; text-align: center;">
                Department
            </th>
            <th style="border: 1pt solid #dddddd; background-color: #2c3e50;
                       color: white; padding: 14pt; text-align: center;">
                Performance Score
            </th>
            <th style="border: 1pt solid #dddddd; background-color: #2c3e50;
                       color: white; padding: 14pt; text-align: center;">
                Rating
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #dddddd; background-color: #ffffff;">
                Alice Johnson
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #ffffff; text-align: center;">
                Engineering
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #ffffff;
                       text-align: center; font-weight: bold;">
                95
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #2ecc71;
                       color: white; text-align: center; font-weight: bold;">
                Excellent
            </td>
        </tr>
        <tr>
            <td style="border: 1pt solid #dddddd; background-color: #ecf0f1;">
                Bob Smith
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #ecf0f1; text-align: center;">
                Marketing
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #ecf0f1;
                       text-align: center; font-weight: bold;">
                88
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #3498db;
                       color: white; text-align: center; font-weight: bold;">
                Good
            </td>
        </tr>
        <tr>
            <td style="border: 1pt solid #dddddd; background-color: #ffffff;">
                Carol White
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #ffffff; text-align: center;">
                Sales
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #ffffff;
                       text-align: center; font-weight: bold;">
                92
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #2ecc71;
                       color: white; text-align: center; font-weight: bold;">
                Excellent
            </td>
        </tr>
        <tr>
            <td style="border: 1pt solid #dddddd; background-color: #ecf0f1;">
                David Brown
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #ecf0f1; text-align: center;">
                Operations
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #ecf0f1;
                       text-align: center; font-weight: bold;">
                85
            </td>
            <td style="border: 1pt solid #dddddd; background-color: #3498db;
                       color: white; text-align: center; font-weight: bold;">
                Good
            </td>
        </tr>
    </tbody>
</table>
```

### Data-Bound Table with Dynamic Spacing

```html
<!-- Model: {
    tablePadding: 10,
    tableSpacing: 0,
    products: [
        {name: "Widget A", stock: 150, price: 25.00},
        {name: "Gadget B", stock: 75, price: 50.00},
        {name: "Tool C", stock: 200, price: 15.00}
    ]
} -->

<table cellpadding="{{model.tablePadding}}"
       cellspacing="{{model.tableSpacing}}"
       style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Product Name
            </th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                In Stock
            </th>
            <th style="border: 1pt solid black; background-color: #336699; color: white;">
                Price
            </th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.products}}">
            <tr>
                <td style="border: 1pt solid black;">{{.name}}</td>
                <td style="border: 1pt solid black; text-align: center;">{{.stock}}</td>
                <td style="border: 1pt solid black; text-align: right;">${{.price}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

---

## See Also

- [table](/reference/htmltags/table.html) - Table element
- [td](/reference/htmltags/td.html) - Table cell elements (td and th)
- [tr](/reference/htmltags/tr.html) - Table row element
- [border](/reference/htmlattributes/border.html) - Border attribute
- [align](/reference/htmlattributes/align.html) - Alignment attributes
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Dynamic data binding

---
