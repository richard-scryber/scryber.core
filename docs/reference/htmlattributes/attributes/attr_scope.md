---
layout: default
title: scope
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @scope : The Table Header Scope Attribute

The `scope` attribute specifies whether a table header cell (`<th>`) applies to a row, column, or group of rows or columns. It defines the relationship between header cells and data cells, improving table structure, accessibility, and semantic meaning in PDF documents. Proper scope usage is essential for complex tables with multiple header levels.

## Usage

The `scope` attribute defines header relationships:
- Specifies which cells a header applies to
- Used exclusively with `<th>` (table header) elements
- Accepts values: `row`, `col`, `rowgroup`, `colgroup`
- Improves table accessibility and structure
- Essential for complex tables with multiple header levels
- Supports data binding for dynamic scope assignment

```html
<!-- Column header -->
<th scope="col">Product Name</th>

<!-- Row header -->
<th scope="row">Total Sales</th>

<!-- Column group header -->
<th scope="colgroup">Q1 Results</th>

<!-- Row group header -->
<th scope="rowgroup">Revenue</th>

<!-- Dynamic scope -->
<th scope="{{model.headerScope}}">{{model.headerText}}</th>
```

---

## Supported Elements

The `scope` attribute is used exclusively with:

### Table Header Element
- `<th>` - Table header cell (only element that uses scope)

---

## Binding Values

The `scope` attribute supports data binding for dynamic scope assignment:

```html
<!-- Dynamic scope value -->
<th scope="{{model.scopeType}}">{{model.headerLabel}}</th>

<!-- Conditional scope -->
<th scope="{{model.isColumnHeader ? 'col' : 'row'}}">Header</th>

<!-- Dynamic table headers -->
<template data-bind="{{model.columnHeaders}}">
    <th scope="col">{{.name}}</th>
</template>

<!-- Row headers with binding -->
<template data-bind="{{model.dataRows}}">
    <tr>
        <th scope="row">{{.category}}</th>
        <td>{{.value}}</td>
    </tr>
</template>

<!-- Complex table with dynamic scopes -->
<template data-bind="{{model.tableHeaders}}">
    <th scope="{{.scope}}" colspan="{{.colspan}}">{{.text}}</th>
</template>
```

**Data Model Example:**
```json
{
  "scopeType": "col",
  "headerLabel": "Product Name",
  "isColumnHeader": true,
  "columnHeaders": [
    { "name": "Product" },
    { "name": "Price" },
    { "name": "Quantity" }
  ],
  "dataRows": [
    { "category": "Electronics", "value": "$12,500" },
    { "category": "Furniture", "value": "$8,300" }
  ],
  "tableHeaders": [
    { "scope": "col", "colspan": 1, "text": "Item" },
    { "scope": "colgroup", "colspan": 3, "text": "Quarterly Sales" }
  ]
}
```

---

## Notes

### Scope Values

Four standard values for the `scope` attribute:

#### col (Column Header)

Headers that apply to columns:

```html
<table>
    <thead>
        <tr>
            <!-- Each header applies to its column -->
            <th scope="col">Product</th>
            <th scope="col">Price</th>
            <th scope="col">Quantity</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Widget A</td>
            <td>$99.99</td>
            <td>150</td>
        </tr>
    </tbody>
</table>
```

#### row (Row Header)

Headers that apply to rows:

```html
<table>
    <tbody>
        <tr>
            <!-- Header applies to this row -->
            <th scope="row">January</th>
            <td>$10,000</td>
            <td>$12,000</td>
            <td>$11,500</td>
        </tr>
        <tr>
            <th scope="row">February</th>
            <td>$11,000</td>
            <td>$13,000</td>
            <td>$12,500</td>
        </tr>
    </tbody>
</table>
```

#### colgroup (Column Group Header)

Headers that apply to a group of columns:

```html
<table>
    <thead>
        <tr>
            <th></th>
            <!-- Applies to the three columns below it -->
            <th scope="colgroup" colspan="3">Q1 2025</th>
            <th scope="colgroup" colspan="3">Q2 2025</th>
        </tr>
        <tr>
            <th scope="col">Category</th>
            <th scope="col">Jan</th>
            <th scope="col">Feb</th>
            <th scope="col">Mar</th>
            <th scope="col">Apr</th>
            <th scope="col">May</th>
            <th scope="col">Jun</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row">Sales</th>
            <td>$10K</td>
            <td>$11K</td>
            <td>$12K</td>
            <td>$13K</td>
            <td>$14K</td>
            <td>$15K</td>
        </tr>
    </tbody>
</table>
```

#### rowgroup (Row Group Header)

Headers that apply to a group of rows:

```html
<table>
    <tbody>
        <!-- Header for the revenue row group -->
        <tr>
            <th scope="rowgroup" rowspan="3">Revenue</th>
            <th scope="row">Product Sales</th>
            <td>$100,000</td>
        </tr>
        <tr>
            <th scope="row">Service Sales</th>
            <td>$50,000</td>
        </tr>
        <tr>
            <th scope="row">Total Revenue</th>
            <td>$150,000</td>
        </tr>

        <!-- Header for the expenses row group -->
        <tr>
            <th scope="rowgroup" rowspan="3">Expenses</th>
            <th scope="row">Salaries</th>
            <td>$60,000</td>
        </tr>
        <tr>
            <th scope="row">Operations</th>
            <td>$30,000</td>
        </tr>
        <tr>
            <th scope="row">Total Expenses</th>
            <td>$90,000</td>
        </tr>
    </tbody>
</table>
```

### When to Use Scope

Use `scope` for:
1. **Simple tables** - Use `col` for column headers, `row` for row headers
2. **Complex tables** - Use `colgroup` and `rowgroup` for multi-level headers
3. **Accessibility** - Screen readers use scope to understand table structure
4. **Semantic clarity** - Makes table relationships explicit

```html
<!-- Simple table - straightforward scopes -->
<table>
    <thead>
        <tr>
            <th scope="col">Name</th>
            <th scope="col">Age</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row">John</th>
            <td>30</td>
        </tr>
    </tbody>
</table>

<!-- Complex table - group scopes -->
<table>
    <thead>
        <tr>
            <th></th>
            <th scope="colgroup" colspan="2">2024</th>
            <th scope="colgroup" colspan="2">2025</th>
        </tr>
        <tr>
            <th scope="col">Category</th>
            <th scope="col">Q3</th>
            <th scope="col">Q4</th>
            <th scope="col">Q1</th>
            <th scope="col">Q2</th>
        </tr>
    </thead>
</table>
```

### Scope vs Headers Attribute

Two methods for defining header relationships:

```html
<!-- Method 1: Using scope (simpler, preferred) -->
<table>
    <thead>
        <tr>
            <th scope="col">Product</th>
            <th scope="col">Price</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row">Widget</th>
            <td>$99.99</td>
        </tr>
    </tbody>
</table>

<!-- Method 2: Using headers and id (for complex associations) -->
<table>
    <thead>
        <tr>
            <th id="product">Product</th>
            <th id="price">Price</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th id="widget">Widget</th>
            <td headers="product widget">$99.99</td>
        </tr>
    </tbody>
</table>
```

**Best Practice:** Use `scope` for most tables; use `headers` attribute only for very complex tables with ambiguous relationships.

### Default Behavior

When `scope` is omitted:
- Browsers may infer scope from context
- Explicit scope is better for clarity and accessibility
- PDF generation may benefit from explicit scoping

```html
<!-- Without scope (ambiguous) -->
<th>Product Name</th>

<!-- With scope (explicit) -->
<th scope="col">Product Name</th>
```

### Combining with Colspan/Rowspan

Use scope with spanning headers:

```html
<table>
    <thead>
        <tr>
            <!-- Column group spanning 3 columns -->
            <th scope="colgroup" colspan="3">Sales Data</th>
        </tr>
        <tr>
            <th scope="col">Product</th>
            <th scope="col">Units</th>
            <th scope="col">Revenue</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <!-- Row header -->
            <th scope="row">Widget A</th>
            <td>100</td>
            <td>$1,000</td>
        </tr>
    </tbody>
</table>
```

### Invalid Scope Values

Only four values are valid:

```html
<!-- Valid -->
<th scope="col">Valid</th>
<th scope="row">Valid</th>
<th scope="colgroup">Valid</th>
<th scope="rowgroup">Valid</th>

<!-- Invalid (will be ignored) -->
<th scope="column">Invalid</th>
<th scope="columns">Invalid</th>
<th scope="header">Invalid</th>
<th scope="all">Invalid</th>
```

### Case Sensitivity

Scope values are case-insensitive:

```html
<!-- All equivalent -->
<th scope="col">Column</th>
<th scope="COL">Column</th>
<th scope="Col">Column</th>
```

**Best Practice:** Use lowercase for consistency.

### Accessibility Benefits

Proper scope usage improves:
1. **Screen reader navigation** - Users can understand table structure
2. **Table comprehension** - Clear header-to-data relationships
3. **PDF accessibility** - Tagged PDFs with proper table structure
4. **Data extraction** - Automated tools can parse tables correctly

### PDF-Specific Considerations

For PDF generation:
- Use explicit `scope` attributes for all headers
- Proper scope creates better table structure in tagged PDFs
- Helps with table extraction and data analysis
- Improves accessibility for PDF readers

---

## Examples

### Simple Table with Column Headers

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #f2f2f2;">
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Product</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Price</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Stock</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Widget A</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$99.99</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">150</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Widget B</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$149.99</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">75</td>
        </tr>
        <tr>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Widget C</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$199.99</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">200</td>
        </tr>
    </tbody>
</table>
```

### Table with Row Headers

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Month</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Sales</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Growth</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">January</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$10,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">+5%</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">February</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$12,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">+20%</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">March</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$15,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">+25%</td>
        </tr>
    </tbody>
</table>
```

### Table with Both Row and Column Headers

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th style="border: 1pt solid #ddd; padding: 8pt;"></th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Q1</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Q2</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Q3</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Q4</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">Revenue</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$50K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$60K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$70K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$80K</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">Expenses</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$30K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$35K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$40K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$45K</td>
        </tr>
        <tr style="background-color: #e8f4ea;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">Profit</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">$20K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">$25K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">$30K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">$35K</td>
        </tr>
    </tbody>
</table>
```

### Complex Table with Column Groups

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <!-- Top-level headers with colgroup scope -->
        <tr style="background-color: #336699; color: white;">
            <th style="border: 1pt solid #ddd; padding: 8pt;"></th>
            <th scope="colgroup" colspan="3" style="border: 1pt solid #ddd; padding: 8pt;">2024</th>
            <th scope="colgroup" colspan="3" style="border: 1pt solid #ddd; padding: 8pt;">2025</th>
        </tr>
        <!-- Sub-headers with col scope -->
        <tr style="background-color: #4a7ba7; color: white;">
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Category</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Q2</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Q3</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Q4</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Q1</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Q2</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Q3</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">Sales</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$100K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$110K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$120K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$130K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$140K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$150K</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">Costs</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$60K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$65K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$70K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$75K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$80K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$85K</td>
        </tr>
        <tr style="background-color: #d4edda;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">Profit</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">$40K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">$45K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">$50K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">$55K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">$60K</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">$65K</td>
        </tr>
    </tbody>
</table>
```

### Complex Table with Row Groups

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th colspan="2" style="border: 1pt solid #ddd; padding: 8pt;">Category</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Amount</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Percentage</th>
        </tr>
    </thead>
    <tbody>
        <!-- Revenue row group -->
        <tr style="background-color: #e8f4ea;">
            <th scope="rowgroup" rowspan="4" style="border: 1pt solid #ddd; padding: 8pt; vertical-align: top;">
                <strong>Revenue</strong>
            </th>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt;">Product Sales</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$150,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">60%</td>
        </tr>
        <tr style="background-color: #e8f4ea;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt;">Service Revenue</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$75,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">30%</td>
        </tr>
        <tr style="background-color: #e8f4ea;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt;">Other Income</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$25,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">10%</td>
        </tr>
        <tr style="background-color: #c3e6cb; font-weight: bold;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt;">Total Revenue</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$250,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">100%</td>
        </tr>

        <!-- Expenses row group -->
        <tr style="background-color: #fff3cd;">
            <th scope="rowgroup" rowspan="4" style="border: 1pt solid #ddd; padding: 8pt; vertical-align: top;">
                <strong>Expenses</strong>
            </th>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt;">Salaries</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$80,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">53%</td>
        </tr>
        <tr style="background-color: #fff3cd;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt;">Operations</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$40,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">27%</td>
        </tr>
        <tr style="background-color: #fff3cd;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt;">Marketing</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$30,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">20%</td>
        </tr>
        <tr style="background-color: #ffeaa7; font-weight: bold;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt;">Total Expenses</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">$150,000</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">100%</td>
        </tr>

        <!-- Net Income -->
        <tr style="background-color: #d1ecf1; font-weight: bold; font-size: 11pt;">
            <th colspan="2" scope="row" style="border: 2pt solid #336699; padding: 10pt;">
                <strong>Net Income</strong>
            </th>
            <td style="border: 2pt solid #336699; padding: 10pt;">$100,000</td>
            <td style="border: 2pt solid #336699; padding: 10pt;">40%</td>
        </tr>
    </tbody>
</table>
```

### Schedule Table

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Time</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Monday</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Tuesday</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Wednesday</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Thursday</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt;">Friday</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">9:00 AM</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Team Meeting</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Development</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Development</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Development</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Code Review</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">11:00 AM</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Development</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Client Call</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Development</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Testing</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Planning</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">2:00 PM</th>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Development</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Development</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Sprint Review</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Development</td>
            <td style="border: 1pt solid #ddd; padding: 8pt;">Retrospective</td>
        </tr>
    </tbody>
</table>
```

### Comparison Table

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th scope="col" style="border: 1pt solid #ddd; padding: 10pt;">Feature</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 10pt;">Basic Plan</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 10pt;">Pro Plan</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 10pt;">Enterprise Plan</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">Storage</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center;">10 GB</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center;">100 GB</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center;">Unlimited</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">Users</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center;">1</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center;">5</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center;">Unlimited</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">Support</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center;">Email</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center;">Email + Chat</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center;">24/7 Phone</td>
        </tr>
        <tr style="background-color: #e8f4ea;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">Price/Month</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center; font-weight: bold;">$9.99</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center; font-weight: bold;">$29.99</td>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: center; font-weight: bold;">$99.99</td>
        </tr>
    </tbody>
</table>
```

### Data-Bound Table

```html
<!-- Model: {
    headers: [
        { text: "Product", scope: "col" },
        { text: "Q1", scope: "col" },
        { text: "Q2", scope: "col" },
        { text: "Q3", scope: "col" },
        { text: "Q4", scope: "col" }
    ],
    rows: [
        { label: "Widget A", scope: "row", values: ["$10K", "$12K", "$15K", "$18K"] },
        { label: "Widget B", scope: "row", values: ["$8K", "$9K", "$11K", "$14K"] },
        { label: "Widget C", scope: "row", values: ["$5K", "$6K", "$7K", "$9K"] }
    ]
} -->

<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <template data-bind="{{model.headers}}">
                <th scope="{{.scope}}" style="border: 1pt solid #ddd; padding: 8pt;">{{.text}}</th>
            </template>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.rows}}">
            <tr>
                <th scope="{{.scope}}" style="border: 1pt solid #ddd; padding: 8pt; background-color: #f2f2f2;">
                    {{.label}}
                </th>
                <template data-bind="{{.values}}">
                    <td style="border: 1pt solid #ddd; padding: 8pt;">{{.}}</td>
                </template>
            </tr>
        </template>
    </tbody>
</table>
```

### Financial Report Table

```html
<table style="width: 100%; border-collapse: collapse; font-family: Arial, sans-serif;">
    <caption style="font-size: 14pt; font-weight: bold; padding: 10pt; text-align: left;">
        Income Statement - Year Ending December 31, 2025
    </caption>
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt; text-align: left;">Account</th>
            <th scope="col" style="border: 1pt solid #ddd; padding: 8pt; text-align: right;">Amount (USD)</th>
        </tr>
    </thead>
    <tbody>
        <tr style="background-color: #e8f4ea;">
            <th scope="row" colspan="2" style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">
                REVENUE
            </th>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; padding-left: 20pt;">Sales Revenue</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;">$500,000</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; padding-left: 20pt;">Service Revenue</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;">$150,000</td>
        </tr>
        <tr style="background-color: #d4edda; font-weight: bold;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt;">Total Revenue</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;">$650,000</td>
        </tr>

        <tr style="background-color: #fff3cd;">
            <th scope="row" colspan="2" style="border: 1pt solid #ddd; padding: 8pt; font-weight: bold;">
                EXPENSES
            </th>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; padding-left: 20pt;">Cost of Goods Sold</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;">$200,000</td>
        </tr>
        <tr>
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt; padding-left: 20pt;">Operating Expenses</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;">$250,000</td>
        </tr>
        <tr style="background-color: #ffeaa7; font-weight: bold;">
            <th scope="row" style="border: 1pt solid #ddd; padding: 8pt;">Total Expenses</th>
            <td style="border: 1pt solid #ddd; padding: 8pt; text-align: right;">$450,000</td>
        </tr>

        <tr style="background-color: #d1ecf1; font-weight: bold; font-size: 12pt;">
            <th scope="row" style="border: 2pt solid #336699; padding: 10pt;">NET INCOME</th>
            <td style="border: 2pt solid #336699; padding: 10pt; text-align: right;">$200,000</td>
        </tr>
    </tbody>
</table>
```

---

## See Also

- [th](/reference/htmltags/th.html) - Table header cell element
- [table](/reference/htmltags/table.html) - Table element
- [thead](/reference/htmltags/thead.html) - Table head element
- [tbody](/reference/htmltags/tbody.html) - Table body element
- [colspan](/reference/htmlattributes/colspan.html) - Column span attribute
- [rowspan](/reference/htmlattributes/rowspan.html) - Row span attribute
- [headers](/reference/htmlattributes/headers.html) - Headers attribute for complex associations

---
