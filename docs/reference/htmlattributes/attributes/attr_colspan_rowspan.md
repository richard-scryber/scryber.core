---
layout: default
title: colspan and rowspan
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @colspan and @rowspan : The Table Cell Spanning Attributes

The `colspan` and `rowspan` attributes enable table cells to span multiple columns or rows, creating complex table layouts with merged cells. These attributes are essential for creating structured data tables, reports, forms, and sophisticated grid layouts in PDF documents.

## Usage

The `colspan` and `rowspan` attributes control cell spanning:
- `colspan` - Number of columns a cell spans horizontally
- `rowspan` - Number of rows a cell spans vertically
- Default value is `1` (no spanning)
- Used exclusively with `<td>` and `<th>` table cells
- Enable complex table layouts and merged cells
- Support data binding for dynamic table structures
- Must account for spanned cells in row/column calculations

```html
<!-- Cell spanning 2 columns -->
<td colspan="2">Spans two columns</td>

<!-- Cell spanning 3 rows -->
<td rowspan="3">Spans three rows</td>

<!-- Cell spanning both -->
<td colspan="2" rowspan="2">Spans 2 columns and 2 rows</td>

<!-- Dynamic spanning -->
<td colspan="{{model.columnCount}}">Dynamic column span</td>
```

---

## Supported Elements

The `colspan` and `rowspan` attributes are used with:

### Table Cell Elements
- `<td>` - Table data cell (primary use)
- `<th>` - Table header cell

---

## Binding Values

The `colspan` and `rowspan` attributes support data binding:

```html
<!-- Dynamic colspan -->
<td colspan="{{model.spanWidth}}">Dynamic column span</td>

<!-- Dynamic rowspan -->
<td rowspan="{{model.spanHeight}}">Dynamic row span</td>

<!-- Calculated spanning -->
<td colspan="{{model.totalColumns - 1}}">Spans all but one column</td>

<!-- Conditional spanning -->
<td colspan="{{model.mergeColumns ? '3' : '1'}}">
    Conditionally merged cell
</td>

<!-- In repeating table structures -->
<template data-bind="{{model.tableData}}">
    <tr>
        <td colspan="{{.shouldMerge ? '2' : '1'}}">{{.content}}</td>
        <td hidden="{{.shouldMerge ? 'hidden' : ''}}">{{.extraContent}}</td>
    </tr>
</template>
```

**Data Model Example:**
```json
{
  "spanWidth": 3,
  "spanHeight": 2,
  "totalColumns": 4,
  "mergeColumns": true,
  "tableData": [
    {
      "shouldMerge": true,
      "content": "Merged cell content",
      "extraContent": "Hidden when merged"
    },
    {
      "shouldMerge": false,
      "content": "First cell",
      "extraContent": "Second cell"
    }
  ]
}
```

---

## Notes

### Colspan Basics

The `colspan` attribute specifies how many columns a cell spans:

```html
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <!-- Normal cells (colspan=1 by default) -->
        <td style="border: 1pt solid #ccc; padding: 8pt;">Cell 1</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Cell 2</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Cell 3</td>
    </tr>
    <tr>
        <!-- Cell spanning 2 columns -->
        <td colspan="2" style="border: 1pt solid #ccc; padding: 8pt;">
            Spans columns 1-2
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Cell 3</td>
    </tr>
    <tr>
        <!-- Cell spanning all 3 columns -->
        <td colspan="3" style="border: 1pt solid #ccc; padding: 8pt;">
            Spans all columns
        </td>
    </tr>
</table>
```

### Rowspan Basics

The `rowspan` attribute specifies how many rows a cell spans:

```html
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <!-- Cell spanning 2 rows -->
        <td rowspan="2" style="border: 1pt solid #ccc; padding: 8pt;">
            Spans rows 1-2
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Row 1, Col 2</td>
    </tr>
    <tr>
        <!-- Note: Only one cell in this row because first cell spans from above -->
        <td style="border: 1pt solid #ccc; padding: 8pt;">Row 2, Col 2</td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Row 3, Col 1</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Row 3, Col 2</td>
    </tr>
</table>
```

### Combining Colspan and Rowspan

Cells can span both columns and rows simultaneously:

```html
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <td colspan="2" rowspan="2"
            style="border: 1pt solid #ccc; padding: 8pt; text-align: center;">
            Spans 2x2 area
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Cell</td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Cell</td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Cell</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Cell</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Cell</td>
    </tr>
</table>
```

### Cell Count Calculations

When using colspan/rowspan, ensure each row has the correct logical cell count:

```html
<!-- 3-column table -->
<table width="100%">
    <tr>
        <!-- Row 1: 3 cells (1+1+1) -->
        <td>Cell 1</td>
        <td>Cell 2</td>
        <td>Cell 3</td>
    </tr>
    <tr>
        <!-- Row 2: Still 3 columns (2+1) -->
        <td colspan="2">Spans 2 columns</td>
        <td>Cell 3</td>
    </tr>
    <tr>
        <!-- Row 3: Still 3 columns (all merged) -->
        <td colspan="3">Spans all 3 columns</td>
    </tr>
</table>
```

**Common mistake:**
```html
<!-- WRONG: Row 2 has 4 columns instead of 3 -->
<tr>
    <td colspan="2">Spans 2</td>
    <td>Cell 3</td>
    <td>Cell 4</td>  <!-- Breaks layout! -->
</tr>
```

### Rowspan Cell Accounting

When a cell spans multiple rows, remember to account for it in subsequent rows:

```html
<table width="100%">
    <tr>
        <td rowspan="3">Spans 3 rows</td>
        <td>Row 1, Col 2</td>
        <td>Row 1, Col 3</td>
    </tr>
    <tr>
        <!-- First cell continues from above -->
        <td>Row 2, Col 2</td>
        <td>Row 2, Col 3</td>
    </tr>
    <tr>
        <!-- First cell still continues from above -->
        <td>Row 3, Col 2</td>
        <td>Row 3, Col 3</td>
    </tr>
</table>
```

### Header Cells with Spanning

Header cells (`<th>`) can also use colspan and rowspan:

```html
<table width="100%">
    <thead>
        <tr>
            <th rowspan="2">Product</th>
            <th colspan="3">Sales by Quarter</th>
        </tr>
        <tr>
            <!-- Product header continues from above -->
            <th>Q1</th>
            <th>Q2</th>
            <th>Q3</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Widget A</td>
            <td>$100</td>
            <td>$150</td>
            <td>$200</td>
        </tr>
    </tbody>
</table>
```

### Valid Values

- Must be positive integers (1, 2, 3, etc.)
- Default value is `1`
- Zero or negative values are invalid
- Should not exceed total table dimensions

```html
<!-- Valid -->
<td colspan="1">Normal cell</td>
<td colspan="2">Spans 2 columns</td>
<td colspan="5">Spans 5 columns</td>

<!-- Invalid -->
<td colspan="0">Invalid</td>
<td colspan="-1">Invalid</td>
```

### Table Layout Considerations

Spanned cells affect table layout and rendering:

- **Width distribution**: Colspan cells affect column width calculations
- **Alignment**: Consider alignment within spanned cells
- **Borders**: Border rendering in spanned cells
- **Background colors**: Backgrounds fill the entire spanned area

```html
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <td colspan="2"
            style="border: 1pt solid #ccc; padding: 8pt;
                   background-color: #e3f2fd; text-align: center;">
            Centered header spanning 2 columns
        </td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt; width: 50%;">
            Column 1
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt; width: 50%;">
            Column 2
        </td>
    </tr>
</table>
```

### Nested Tables vs Spanning

For very complex layouts, consider whether nested tables or spanning is more appropriate:

```html
<!-- Using spanning -->
<table>
    <tr>
        <td colspan="2" rowspan="2">Complex merged area</td>
        <td>Cell</td>
    </tr>
    <!-- ... complex spanning structure ... -->
</table>

<!-- Using nested tables (sometimes clearer) -->
<table>
    <tr>
        <td>
            <table>
                <!-- Nested table structure -->
            </table>
        </td>
        <td>Cell</td>
    </tr>
</table>
```

### Accessibility Considerations

When using spanning cells:
- Use clear header associations
- Ensure logical reading order
- Consider screen reader interpretation
- Use `<th>` appropriately for headers

```html
<table>
    <thead>
        <tr>
            <th colspan="2">Product Information</th>
            <th colspan="2">Pricing</th>
        </tr>
        <tr>
            <th>Name</th>
            <th>Category</th>
            <th>Regular</th>
            <th>Sale</th>
        </tr>
    </thead>
    <tbody>
        <!-- Data rows -->
    </tbody>
</table>
```

---

## Examples

### Basic Colspan Example

```html
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <th style="border: 1pt solid #ccc; padding: 8pt;">Name</th>
        <th style="border: 1pt solid #ccc; padding: 8pt;">Age</th>
        <th style="border: 1pt solid #ccc; padding: 8pt;">City</th>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">John</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">30</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">New York</td>
    </tr>
    <tr>
        <td colspan="3" style="border: 1pt solid #ccc; padding: 8pt;
                                background-color: #f0f0f0; text-align: center;">
            Total: 1 person
        </td>
    </tr>
</table>
```

### Basic Rowspan Example

```html
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <th style="border: 1pt solid #ccc; padding: 8pt;">Category</th>
        <th style="border: 1pt solid #ccc; padding: 8pt;">Item</th>
        <th style="border: 1pt solid #ccc; padding: 8pt;">Price</th>
    </tr>
    <tr>
        <td rowspan="3" style="border: 1pt solid #ccc; padding: 8pt;
                                background-color: #e3f2fd; font-weight: bold;">
            Electronics
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Laptop</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">$1,200</td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Mouse</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">$25</td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">Keyboard</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">$80</td>
    </tr>
</table>
```

### Header Row with Colspan

```html
<table width="100%" style="border-collapse: collapse;">
    <thead>
        <tr>
            <th colspan="4"
                style="border: 1pt solid #336699; padding: 15pt;
                       background-color: #336699; color: white;
                       text-align: center; font-size: 16pt;">
                Quarterly Sales Report
            </th>
        </tr>
        <tr>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #e3f2fd;">Product</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #e3f2fd;">Q1</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #e3f2fd;">Q2</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #e3f2fd;">Q3</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Widget A</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$50,000</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$65,000</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$72,000</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Widget B</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$40,000</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$45,000</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$48,000</td>
        </tr>
    </tbody>
</table>
```

### Complex Multi-Level Headers

```html
<table width="100%" style="border-collapse: collapse;">
    <thead>
        <tr>
            <th rowspan="2" style="border: 1pt solid #ccc; padding: 8pt;
                                   background-color: #336699; color: white;">
                Product
            </th>
            <th colspan="3" style="border: 1pt solid #ccc; padding: 8pt;
                                   background-color: #336699; color: white;">
                2023
            </th>
            <th colspan="3" style="border: 1pt solid #ccc; padding: 8pt;
                                   background-color: #336699; color: white;">
                2024
            </th>
        </tr>
        <tr>
            <!-- Product header continues from above -->
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #66aacc; color: white;">Q1</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #66aacc; color: white;">Q2</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #66aacc; color: white;">Q3</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #66aacc; color: white;">Q1</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #66aacc; color: white;">Q2</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #66aacc; color: white;">Q3</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Widget A</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$10K</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$12K</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$15K</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$18K</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$20K</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$22K</td>
        </tr>
    </tbody>
</table>
```

### Product Comparison Table

```html
<table width="100%" style="border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Feature</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Basic</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Pro</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Enterprise</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td colspan="4"
                style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #e3f2fd; font-weight: bold;">
                Core Features
            </td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Storage</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">10 GB</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">100 GB</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Unlimited</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Users</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">1</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">5</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Unlimited</td>
        </tr>
        <tr>
            <td colspan="4"
                style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #e3f2fd; font-weight: bold;">
                Advanced Features
            </td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">API Access</td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: center;">✗</td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: center;">✓</td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: center;">✓</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">24/7 Support</td>
            <td colspan="2" style="border: 1pt solid #ccc; padding: 8pt;
                                   text-align: center;">Email only</td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: center;">
                Phone & Email
            </td>
        </tr>
    </tbody>
</table>
```

### Schedule/Timetable

```html
<table width="100%" style="border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #336699; color: white;">Time</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #336699; color: white;">Monday</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #336699; color: white;">Tuesday</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;
                       background-color: #336699; color: white;">Wednesday</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">9:00 AM</td>
            <td rowspan="2" style="border: 1pt solid #ccc; padding: 8pt;
                                   background-color: #e3f2fd;">
                Morning Meeting<br/>(9:00-10:30)
            </td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Email Review</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Team Standup</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">10:00 AM</td>
            <!-- Morning Meeting continues -->
            <td style="border: 1pt solid #ccc; padding: 8pt;">Development</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Development</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">11:00 AM</td>
            <td colspan="3" style="border: 1pt solid #ccc; padding: 8pt;
                                   background-color: #fff3cd; text-align: center;">
                Company All-Hands Meeting
            </td>
        </tr>
    </tbody>
</table>
```

### Grouped Data Table

```html
<table width="100%" style="border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Region</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">City</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Sales</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td rowspan="3" style="border: 1pt solid #ccc; padding: 8pt;
                                   background-color: #e8f5e9; font-weight: bold;">
                North
            </td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">New York</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$500K</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Boston</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$350K</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Chicago</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$420K</td>
        </tr>
        <tr>
            <td rowspan="2" style="border: 1pt solid #ccc; padding: 8pt;
                                   background-color: #fff3e0; font-weight: bold;">
                South
            </td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Miami</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$380K</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Atlanta</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">$290K</td>
        </tr>
        <tr>
            <td colspan="2" style="border: 1pt solid #ccc; padding: 8pt;
                                   font-weight: bold; text-align: right;">
                Total:
            </td>
            <td style="border: 1pt solid #ccc; padding: 8pt; font-weight: bold;">
                $1,940K
            </td>
        </tr>
    </tbody>
</table>
```

### Form Layout with Spanning

```html
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <td colspan="2"
            style="border: 1pt solid #ccc; padding: 15pt;
                   background-color: #336699; color: white;
                   font-size: 16pt; font-weight: bold;">
            Registration Form
        </td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt; width: 30%;
                   font-weight: bold;">
            Full Name:
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">
            [Input field]
        </td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt; font-weight: bold;">
            Email:
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">
            [Input field]
        </td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt; font-weight: bold;
                   vertical-align: top;">
            Address:
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">
            [Textarea field]
        </td>
    </tr>
    <tr>
        <td colspan="2" style="border: 1pt solid #ccc; padding: 10pt;
                               text-align: center; background-color: #f0f0f0;">
            <button style="padding: 8pt 20pt;">Submit</button>
        </td>
    </tr>
</table>
```

### Invoice Table

```html
<table width="100%" style="border-collapse: collapse;">
    <thead>
        <tr>
            <th colspan="4"
                style="border: 1pt solid #ccc; padding: 15pt;
                       background-color: #336699; color: white; text-align: left;">
                <h2 style="margin: 0;">INVOICE #12345</h2>
            </th>
        </tr>
        <tr>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Item</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Quantity</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Price</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Total</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Premium Widget</td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: center;">2</td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: right;">$150.00</td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: right;">$300.00</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">Standard Widget</td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: center;">5</td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: right;">$75.00</td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: right;">$375.00</td>
        </tr>
        <tr>
            <td colspan="3"
                style="border: 1pt solid #ccc; padding: 8pt; text-align: right;
                       font-weight: bold;">
                Subtotal:
            </td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: right;">
                $675.00
            </td>
        </tr>
        <tr>
            <td colspan="3"
                style="border: 1pt solid #ccc; padding: 8pt; text-align: right;
                       font-weight: bold;">
                Tax (10%):
            </td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: right;">
                $67.50
            </td>
        </tr>
        <tr>
            <td colspan="3"
                style="border: 1pt solid #ccc; padding: 8pt; text-align: right;
                       font-weight: bold; background-color: #f0f0f0;">
                Total:
            </td>
            <td style="border: 1pt solid #ccc; padding: 8pt; text-align: right;
                       font-weight: bold; font-size: 14pt; background-color: #f0f0f0;">
                $742.50
            </td>
        </tr>
    </tbody>
</table>
```

### Data Matrix with Merged Cells

```html
<table width="100%" style="border-collapse: collapse;">
    <tr>
        <td colspan="2" rowspan="2"
            style="border: 1pt solid #ccc; padding: 8pt; background-color: #e0e0e0;">
        </td>
        <td colspan="3" style="border: 1pt solid #ccc; padding: 8pt;
                                background-color: #e3f2fd; text-align: center;
                                font-weight: bold;">
            Product Type
        </td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt; text-align: center;">A</td>
        <td style="border: 1pt solid #ccc; padding: 8pt; text-align: center;">B</td>
        <td style="border: 1pt solid #ccc; padding: 8pt; text-align: center;">C</td>
    </tr>
    <tr>
        <td rowspan="3" style="border: 1pt solid #ccc; padding: 8pt;
                                background-color: #fff3e0; font-weight: bold;
                                writing-mode: vertical-rl; text-align: center;">
            Region
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">North</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">120</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">95</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">78</td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">South</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">105</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">110</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">92</td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">West</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">88</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">102</td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">85</td>
    </tr>
</table>
```

### Dynamic Spanning with Data Binding

```html
<!-- Model: {
    report: {
        title: "Sales Report",
        columns: 4,
        sections: [
            { name: "Q1", items: [{...}, {...}] },
            { name: "Q2", items: [{...}] }
        ]
    }
} -->

<table width="100%">
    <tr>
        <th colspan="{{model.report.columns}}">
            {{model.report.title}}
        </th>
    </tr>
    <template data-bind="{{model.report.sections}}">
        <tr>
            <td colspan="{{model.report.columns}}"
                style="background-color: #e3f2fd; font-weight: bold; padding: 10pt;">
                {{.name}}
            </td>
        </tr>
        <template data-bind="{{.items}}">
            <tr>
                <td>{{.product}}</td>
                <td>{{.quantity}}</td>
                <td>{{.price}}</td>
                <td>{{.total}}</td>
            </tr>
        </template>
    </template>
</table>
```

### Calendar Grid

```html
<table width="100%" style="border-collapse: collapse;">
    <thead>
        <tr>
            <th colspan="7"
                style="border: 1pt solid #ccc; padding: 15pt;
                       background-color: #336699; color: white;">
                January 2025
            </th>
        </tr>
        <tr>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Sun</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Mon</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Tue</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Wed</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Thu</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Fri</th>
            <th style="border: 1pt solid #ccc; padding: 8pt;">Sat</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td colspan="2" style="border: 1pt solid #ccc; padding: 8pt;
                                   background-color: #f0f0f0;"></td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">1</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">2</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">3</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">4</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">5</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ccc; padding: 8pt;">6</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">7</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">8</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">9</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">10</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">11</td>
            <td style="border: 1pt solid #ccc; padding: 8pt;">12</td>
        </tr>
        <!-- Additional weeks... -->
    </tbody>
</table>
```

---

## See Also

- [table](/reference/htmltags/table.html) - Table element
- [tr](/reference/htmltags/tr.html) - Table row element
- [td](/reference/htmltags/td.html) - Table data cell element
- [th](/reference/htmltags/th.html) - Table header cell element
- [width](/reference/htmlattributes/width.html) - Width attribute
- [height](/reference/htmlattributes/height.html) - Height attribute
- [style](/reference/htmlattributes/style.html) - Inline CSS styling
- [Tables](/reference/tables/) - Complete table documentation
- [Data Binding](/reference/binding/) - Dynamic attribute values

---
