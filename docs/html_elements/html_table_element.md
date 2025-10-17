---
layout: default
title: table
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;table&gt; : The Table Element

The `<table>` element is used to create structured tabular data displays in PDF documents. It supports complex layouts with headers, footers, row and column spans, borders, and full styling control. Tables automatically handle page breaks and can repeat headers across pages.

## Usage

The `<table>` element creates a structured grid that:
- Organizes content in rows and columns
- Supports semantic sections (thead, tbody, tfoot)
- Automatically calculates column widths or uses specified widths
- Handles page breaks and repeating headers
- Supports borders, padding, and full CSS styling
- Can be data-bound for dynamic content
- Maintains cell alignment and spacing

```html
<table style="width: 100%; border: 1pt solid black;">
    <thead>
        <tr>
            <th>Header 1</th>
            <th>Header 2</th>
            <th>Header 3</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Data 1</td>
            <td>Data 2</td>
            <td>Data 3</td>
        </tr>
    </tbody>
</table>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the table. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the table, or omit/empty to show. |

### Table Structure Elements

Tables should contain one or more of these structural elements:

| Element | Description |
|---------|-------------|
| `<thead>` | Table header section - repeats at top of each page by default |
| `<tbody>` | Table body section - contains main data rows |
| `<tfoot>` | Table footer section - appears at bottom of table |
| `<tr>` | Table row - contains cells |
| `<th>` | Table header cell - bold by default, used in thead |
| `<td>` | Table data cell - standard cell for data |

### CSS Style Support

**Table Layout**:
- `width`, `min-width`, `max-width` - Table width control
- `table-layout: auto | fixed` - Column width calculation method
- `border-collapse: collapse | separate` - Border rendering mode
- `border-spacing` - Space between cell borders (separate mode)

**Cell Spacing and Padding**:
- `padding` - Default padding for all cells
- `border` - Table and cell borders
- `margin` - Space around the table

**Borders**:
- `border`, `border-width`, `border-color`, `border-style`
- `border-top`, `border-right`, `border-bottom`, `border-left`
- Individual cell borders can be styled separately

**Visual Styling**:
- `background-color`, `background-image`
- `color` - Text color (inherited by cells)
- `font-family`, `font-size`, `font-weight` - Typography

**Layout Control**:
- `page-break-before`, `page-break-after` - Control page breaks around table
- `page-break-inside: avoid` - Keep entire table together if possible

---

## Table Sections

### thead - Table Header

The `<thead>` element contains header rows that:
- Display in **bold** by default
- **Repeat at the top of each page** when the table spans multiple pages
- Can be styled independently
- Usually contain `<th>` cells

```html
<thead style="background-color: #336699; color: white;">
    <tr>
        <th>Column 1</th>
        <th>Column 2</th>
    </tr>
</thead>
```

**Disabling Header Repetition**:
```html
<thead data-repeat="false">
    <!-- Header won't repeat on subsequent pages -->
</thead>
```

### tbody - Table Body

The `<tbody>` element contains the main data rows:
- Contains the primary table content
- Can have multiple tbody sections in one table
- Each tbody can be styled independently
- Supports data binding for dynamic rows

```html
<tbody>
    <tr>
        <td>Row 1, Cell 1</td>
        <td>Row 1, Cell 2</td>
    </tr>
    <tr>
        <td>Row 2, Cell 1</td>
        <td>Row 2, Cell 2</td>
    </tr>
</tbody>
```

### tfoot - Table Footer

The `<tfoot>` element contains footer rows:
- Appears at the bottom of the table
- Can contain summary data, totals, or notes
- Styled independently from header and body

```html
<tfoot style="font-weight: bold; border-top: 2pt solid black;">
    <tr>
        <td>Total:</td>
        <td>$1,234.56</td>
    </tr>
</tfoot>
```

---

## Notes

### Default Behavior

Tables have the following default characteristics:
1. **Auto Width**: Takes full width of container unless specified
2. **Auto Column Widths**: Columns sized to content unless specified
3. **Border Collapse**: Separate borders by default
4. **Cell Padding**: 2pt default padding
5. **Cell Margins**: 2pt default margin
6. **Cell Borders**: 1pt solid gray (#999999) default
7. **Header Repetition**: Headers repeat on each page by default

### Column Width Control

Column widths can be controlled in several ways:

**1. CSS on table**:
```html
<table style="width: 100%;">
    <!-- Columns auto-sized within 100% -->
</table>
```

**2. CSS on cells**:
```html
<th style="width: 30%;">Narrow</th>
<th style="width: 70%;">Wide</th>
```

**3. Fixed pixel/point widths**:
```html
<th style="width: 100pt;">Fixed Width</th>
<th style="width: 200pt;">Wider Fixed</th>
```

### Page Breaking

Tables automatically handle page breaks:
- Rows are kept together (won't split mid-row)
- Headers repeat on new pages by default
- Large cells may be clipped if content exceeds available space

Control page breaks:
```html
<!-- Keep entire table together -->
<table style="page-break-inside: avoid;">...</table>

<!-- Force page break before table -->
<table style="page-break-before: always;">...</table>
```

### Border Styling

**Collapsed Borders** (single line between cells):
```html
<table style="border-collapse: collapse; border: 1pt solid black;">
    <tr>
        <td style="border: 1pt solid black;">Cell</td>
    </tr>
</table>
```

**Separate Borders** (space between cells):
```html
<table style="border-collapse: separate; border-spacing: 5pt;">
    <tr>
        <td style="border: 1pt solid black;">Cell</td>
    </tr>
</table>
```

---

## Examples

### Basic Table with Header

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #e0e0e0;">Name</th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #e0e0e0;">Email</th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #e0e0e0;">Phone</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">John Doe</td>
            <td style="border: 1pt solid black; padding: 8pt;">john@example.com</td>
            <td style="border: 1pt solid black; padding: 8pt;">(555) 123-4567</td>
        </tr>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">Jane Smith</td>
            <td style="border: 1pt solid black; padding: 8pt;">jane@example.com</td>
            <td style="border: 1pt solid black; padding: 8pt;">(555) 987-6543</td>
        </tr>
    </tbody>
</table>
```

### Styled Table with CSS Classes

```html
<style>
    .data-table {
        width: 100%;
        border-collapse: collapse;
        font-size: 10pt;
    }
    .data-table th {
        background-color: #336699;
        color: white;
        padding: 10pt;
        text-align: left;
        border: 1pt solid #ffffff;
    }
    .data-table td {
        padding: 8pt;
        border: 1pt solid #dddddd;
    }
    .data-table tbody tr:nth-child(even) {
        background-color: #f9f9f9;
    }
</style>

<table class="data-table">
    <thead>
        <tr>
            <th>Product</th>
            <th>Quantity</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Widget A</td>
            <td>10</td>
            <td>$25.00</td>
        </tr>
        <tr>
            <td>Widget B</td>
            <td>5</td>
            <td>$50.00</td>
        </tr>
    </tbody>
</table>
```

### Striped Table (Alternating Row Colors)

```html
<style>
    .striped-table {
        width: 100%;
        border-collapse: collapse;
    }
    .striped-table th {
        background-color: #2c3e50;
        color: white;
        padding: 10pt;
        border: 1pt solid #34495e;
    }
    .striped-table td {
        padding: 8pt;
        border: 1pt solid #ecf0f1;
    }
    .striped-table tr:nth-child(odd) {
        background-color: #ffffff;
    }
    .striped-table tr:nth-child(even) {
        background-color: #ecf0f1;
    }
</style>

<table class="striped-table">
    <thead>
        <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Status</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>001</td>
            <td>Project Alpha</td>
            <td>Active</td>
        </tr>
        <tr>
            <td>002</td>
            <td>Project Beta</td>
            <td>Pending</td>
        </tr>
        <tr>
            <td>003</td>
            <td>Project Gamma</td>
            <td>Complete</td>
        </tr>
    </tbody>
</table>
```

### Table with Column Widths

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="width: 10%; border: 1pt solid black; padding: 8pt;">ID</th>
            <th style="width: 40%; border: 1pt solid black; padding: 8pt;">Description</th>
            <th style="width: 25%; border: 1pt solid black; padding: 8pt;">Category</th>
            <th style="width: 25%; border: 1pt solid black; padding: 8pt;">Date</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">1</td>
            <td style="border: 1pt solid black; padding: 8pt;">Lorem ipsum dolor sit amet</td>
            <td style="border: 1pt solid black; padding: 8pt;">Type A</td>
            <td style="border: 1pt solid black; padding: 8pt;">2024-01-15</td>
        </tr>
    </tbody>
</table>
```

### Table with colspan and rowspan

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th colspan="3" style="border: 1pt solid black; padding: 10pt;
                                    background-color: #34495e; color: white; text-align: center;">
                Quarterly Sales Report
            </th>
        </tr>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #95a5a6;">Quarter</th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #95a5a6;">Revenue</th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #95a5a6;">Growth</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">Q1 2024</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">$125,000</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">8%</td>
        </tr>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">Q2 2024</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">$145,000</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">16%</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt; font-weight: bold;">Total</td>
            <td colspan="2" style="border: 1pt solid black; padding: 8pt;
                                   text-align: right; font-weight: bold;">$270,000</td>
        </tr>
    </tfoot>
</table>
```

### Data-Bound Dynamic Table

```html
<!-- Model: { items: [{name: "Item 1", qty: 5, price: 10.00}, ...] } -->
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt;">Product Name</th>
            <th style="border: 1pt solid black; padding: 8pt;">Quantity</th>
            <th style="border: 1pt solid black; padding: 8pt;">Unit Price</th>
            <th style="border: 1pt solid black; padding: 8pt;">Total</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.items}}">
            <tr>
                <td style="border: 1pt solid black; padding: 8pt;">{{.name}}</td>
                <td style="border: 1pt solid black; padding: 8pt; text-align: right;">{{.qty}}</td>
                <td style="border: 1pt solid black; padding: 8pt; text-align: right;">${{.price}}</td>
                <td style="border: 1pt solid black; padding: 8pt; text-align: right;">
                    ${{.qty * .price}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### Complex Table with Multiple Sections

```html
<table style="width: 100%; border-collapse: collapse; font-size: 10pt;">
    <thead>
        <tr>
            <th colspan="4" style="border: 1pt solid black; padding: 12pt;
                                    background-color: #2c3e50; color: white;
                                    text-align: center; font-size: 14pt;">
                Employee Performance Summary
            </th>
        </tr>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #34495e; color: white;">
                Employee
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #34495e; color: white;">
                Department
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #34495e; color: white;">
                Score
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #34495e; color: white;">
                Rating
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">Alice Johnson</td>
            <td style="border: 1pt solid black; padding: 8pt;">Engineering</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: center;">95</td>
            <td style="border: 1pt solid black; padding: 8pt;
                       background-color: #2ecc71; color: white; text-align: center;">Excellent</td>
        </tr>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">Bob Smith</td>
            <td style="border: 1pt solid black; padding: 8pt;">Marketing</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: center;">88</td>
            <td style="border: 1pt solid black; padding: 8pt;
                       background-color: #3498db; color: white; text-align: center;">Good</td>
        </tr>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">Carol White</td>
            <td style="border: 1pt solid black; padding: 8pt;">Sales</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: center;">92</td>
            <td style="border: 1pt solid black; padding: 8pt;
                       background-color: #2ecc71; color: white; text-align: center;">Excellent</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="2" style="border: 1pt solid black; padding: 8pt;
                                   font-weight: bold; text-align: right;">
                Average Score:
            </td>
            <td colspan="2" style="border: 1pt solid black; padding: 8pt;
                                   font-weight: bold; text-align: center;">
                91.7
            </td>
        </tr>
    </tfoot>
</table>
```

### Responsive Table with Variable Widths

```html
<table style="width: 100%; border-collapse: collapse; min-width: 500pt;">
    <thead>
        <tr>
            <th style="width: 20%; border: 1pt solid black; padding: 8pt;">Code</th>
            <th style="width: 50%; border: 1pt solid black; padding: 8pt;">Description</th>
            <th style="width: 15%; border: 1pt solid black; padding: 8pt;">Qty</th>
            <th style="width: 15%; border: 1pt solid black; padding: 8pt;">Price</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt; font-family: monospace;">SKU-001</td>
            <td style="border: 1pt solid black; padding: 8pt;">
                Premium Widget with Extended Warranty and Support
            </td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">12</td>
            <td style="border: 1pt solid black; padding: 8pt; text-align: right;">$99.99</td>
        </tr>
    </tbody>
</table>
```

### Table with Conditional Styling

```html
<!-- Model: {
    transactions: [
        {date: "2024-01-15", description: "Sale", amount: 1500, type: "credit"},
        {date: "2024-01-16", description: "Purchase", amount: 800, type: "debit"}
    ]
} -->
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt;">Date</th>
            <th style="border: 1pt solid black; padding: 8pt;">Description</th>
            <th style="border: 1pt solid black; padding: 8pt;">Amount</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.transactions}}">
            <tr>
                <td style="border: 1pt solid black; padding: 8pt;">{{.date}}</td>
                <td style="border: 1pt solid black; padding: 8pt;">{{.description}}</td>
                <td style="border: 1pt solid black; padding: 8pt; text-align: right;
                           color: {{.type == 'credit' ? 'green' : 'red'}};">
                    {{.type == 'credit' ? '+' : '-'}}${{.amount}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### Borderless Table for Layout

```html
<table style="width: 100%; border: none; border-collapse: collapse;">
    <tr>
        <td style="width: 30%; padding: 10pt; vertical-align: top; border: none;">
            <img src="logo.png" style="width: 100pt;" />
        </td>
        <td style="width: 70%; padding: 10pt; vertical-align: top; border: none;">
            <h2 style="margin: 0;">Company Name</h2>
            <p style="margin: 5pt 0;">123 Business Street</p>
            <p style="margin: 5pt 0;">City, State 12345</p>
            <p style="margin: 5pt 0;">Phone: (555) 123-4567</p>
        </td>
    </tr>
</table>
```

---

## See Also

- [tr](/reference/htmltags/tr.html) - Table row element
- [td](/reference/htmltags/td.html) - Table cell elements (td and th)
- [div](/reference/htmltags/div.html) - Generic block container
- [Data Binding](/reference/binding/) - Dynamic data binding
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Template Element](/reference/htmltags/template.html) - Template for repeating content

---
