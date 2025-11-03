---
layout: default
title: data-bind-start, data-bind-step, data-bind-max
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-bind-start, @data-bind-step, @data-bind-max : Template Iteration Control Attributes

These three attributes provide precise control over template iteration behavior, enabling pagination, selective item rendering, and performance optimization for large datasets. They work in conjunction with the `data-bind` attribute on `<template>` elements to control which items from a collection are processed and rendered.

---

## Summary

The iteration control attributes extend the basic `data-bind` functionality by allowing you to:
- **Paginate** large datasets by specifying start index and maximum count
- **Filter** items by stepping through collections at intervals
- **Optimize performance** by limiting the number of items processed
- **Create alternating patterns** using step values
- **Implement batch processing** for reports and exports

These attributes are essential for:
- Paginated reports showing a subset of data
- Performance optimization with large collections (1000+ items)
- Creating alternating row patterns (odd/even)
- Batch processing documents
- Implementing "top N" item displays
- Creating multi-page reports with consistent item counts per page

---

## Usage

The iteration control attributes are applied to `<template>` elements alongside the `data-bind` attribute:

```html
<template data-bind="{{model.items}}"
          data-bind-start="10"
          data-bind-step="1"
          data-bind-max="20">
    <!-- Content repeated for items 10-29 -->
    <div>{{.name}}</div>
</template>
```

### Basic Syntax

```html
<!-- Start at index 0 (default), show 10 items -->
<template data-bind="{{collection}}"
          data-bind-max="10">
    <!-- Template content -->
</template>

<!-- Start at index 20, show 10 items (items 20-29) -->
<template data-bind="{{collection}}"
          data-bind-start="20"
          data-bind-max="10">
    <!-- Template content -->
</template>

<!-- Show every other item (step of 2) -->
<template data-bind="{{collection}}"
          data-bind-step="2">
    <!-- Template content -->
</template>

<!-- Complex: Start at 5, every 3rd item, max 15 items -->
<template data-bind="{{collection}}"
          data-bind-start="5"
          data-bind-step="3"
          data-bind-max="15">
    <!-- Template content -->
</template>
```

---

## Supported Elements

These attributes are **only** supported on the following element:

- `<template>` - The template element for repeating content

**Note**: These attributes must be used in conjunction with the `data-bind` attribute. They have no effect without a bound collection.

---

## Binding Values

### data-bind-start

**Type**: `integer`
**Default**: `0`
**Description**: Zero-based index of the first item to process from the collection.

```html
<!-- Start at first item (default) -->
<template data-bind="{{items}}" data-bind-start="0">

<!-- Start at 11th item (index 10) -->
<template data-bind="{{items}}" data-bind-start="10">

<!-- Start at 101st item (index 100) -->
<template data-bind="{{items}}" data-bind-start="100">
```

**Notes**:
- If `start` exceeds the collection size, no items are rendered
- Negative values are treated as 0
- Works with any enumerable data type

### data-bind-step

**Type**: `integer`
**Default**: `1`
**Description**: Increment value for iteration. Controls which items are selected from the collection.

```html
<!-- Process every item (default) -->
<template data-bind="{{items}}" data-bind-step="1">

<!-- Process every other item -->
<template data-bind="{{items}}" data-bind-step="2">

<!-- Process every third item -->
<template data-bind="{{items}}" data-bind-step="3">

<!-- Process every tenth item -->
<template data-bind="{{items}}" data-bind-step="10">
```

**Notes**:
- Step value of 1 processes all items sequentially
- Step value of 2 processes items at indices 0, 2, 4, 6, etc.
- Step value of 3 processes items at indices 0, 3, 6, 9, etc.
- Values less than 1 are treated as 1
- Combined with `start`, the first item is at `start`, then `start + step`, `start + (step * 2)`, etc.

### data-bind-max

**Type**: `integer`
**Default**: `int.MaxValue` (unlimited)
**Description**: Maximum number of items to process and render.

```html
<!-- Process all items (default) -->
<template data-bind="{{items}}">

<!-- Process maximum of 10 items -->
<template data-bind="{{items}}" data-bind-max="10">

<!-- Process maximum of 50 items -->
<template data-bind="{{items}}" data-bind-max="50">

<!-- Process maximum of 100 items -->
<template data-bind="{{items}}" data-bind-max="100">
```

**Notes**:
- Limits the total number of items processed, not the index
- Combined with `start`, processes up to `max` items starting from `start`
- Combined with `step`, processes up to `max` items at `step` intervals
- If collection has fewer items than `max`, all available items are processed
- Values less than 1 result in no items being processed

### Combining Attributes

The three attributes work together to provide flexible iteration control:

```html
<!-- Formula: Process items from collection[start] up to max items, incrementing by step -->
<template data-bind="{{items}}"
          data-bind-start="START_INDEX"
          data-bind-step="STEP_VALUE"
          data-bind-max="MAX_COUNT">
```

**Processing Logic**:
1. Start at index `data-bind-start` (default: 0)
2. Process the item at current index
3. Increment index by `data-bind-step` (default: 1)
4. Repeat until `data-bind-max` items are processed or collection ends
5. Skip any indices that exceed collection bounds

---

## Notes

### Pagination Calculations

For pagination, calculate the start index based on page number and items per page:

```
start_index = (page_number - 1) * items_per_page
max_count = items_per_page
```

Example for page 3 with 20 items per page:
```
start_index = (3 - 1) * 20 = 40
max_count = 20
// Processes items 40-59
```

### Performance Considerations

1. **Large Collections**: Always use `data-bind-max` to limit rendering for collections over 100 items
2. **Style Caching**: Combine with `data-cache-styles="true"` for better performance
3. **Memory Usage**: Processing fewer items reduces memory footprint during PDF generation
4. **Rendering Time**: Each item requires layout calculation; limiting items improves generation speed

### Index Behavior

- Indices are zero-based (first item is index 0)
- Invalid indices (negative, too large) are handled gracefully
- Out-of-bounds indices are skipped without error
- The `index()` function within the template reflects the iteration count, not the collection index

### Common Patterns

**Odd Items Only**:
```html
<template data-bind="{{items}}" data-bind-start="0" data-bind-step="2">
```

**Even Items Only**:
```html
<template data-bind="{{items}}" data-bind-start="1" data-bind-step="2">
```

**Top N Items**:
```html
<template data-bind="{{items}}" data-bind-max="10">
```

**Page 2 of 20 Items Per Page**:
```html
<template data-bind="{{items}}" data-bind-start="20" data-bind-max="20">
```

**Every 5th Item**:
```html
<template data-bind="{{items}}" data-bind-step="5">
```

---

## Examples

### 1. Basic Pagination (First Page)

Show the first 10 items from a collection:

```html
<!-- Model: { products: [...100 products...] } -->
<h2>Products - Page 1</h2>
<table style="width: 100%;">
    <thead>
        <tr>
            <th>Product</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.products}}"
                  data-bind-start="0"
                  data-bind-max="10">
            <tr>
                <td>{{.name}}</td>
                <td>${{.price}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 2. Pagination (Second Page)

Show items 11-20 (indices 10-19):

```html
<h2>Products - Page 2</h2>
<table style="width: 100%;">
    <thead>
        <tr>
            <th>Product</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.products}}"
                  data-bind-start="10"
                  data-bind-max="10">
            <tr>
                <td>{{.name}}</td>
                <td>${{.price}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 3. Top 5 Items

Display only the first 5 items:

```html
<h2>Top 5 Bestsellers</h2>
<div style="column-count: 1;">
    <template data-bind="{{model.bestsellers}}"
              data-bind-max="5">
        <div style="padding: 10pt; margin-bottom: 10pt; border: 1pt solid #ddd;">
            <h3 style="margin: 0;">{{index() + 1}}. {{.name}}</h3>
            <p style="margin: 5pt 0 0 0;">{{.description}}</p>
            <div style="font-weight: bold; color: #336699;">
                ${{.price}}
            </div>
        </div>
    </template>
</div>
```

### 4. Alternating Row Pattern (Odd Rows)

Show only odd-indexed items (1st, 3rd, 5th, etc.):

```html
<h2>Odd Items</h2>
<template data-bind="{{model.items}}"
          data-bind-start="0"
          data-bind-step="2">
    <div style="background-color: #f0f0f0; padding: 10pt; margin-bottom: 5pt;">
        Item {{.name}}
    </div>
</template>
```

### 5. Alternating Row Pattern (Even Rows)

Show only even-indexed items (2nd, 4th, 6th, etc.):

```html
<h2>Even Items</h2>
<template data-bind="{{model.items}}"
          data-bind-start="1"
          data-bind-step="2">
    <div style="background-color: #e0e0e0; padding: 10pt; margin-bottom: 5pt;">
        Item {{.name}}
    </div>
</template>
```

### 6. Multi-Column Layout (Two Columns)

Split items into two columns using odd/even pattern:

```html
<div style="width: 100%;">
    <!-- Left Column (Odd Items) -->
    <div style="display: inline-block; width: 48%; vertical-align: top; margin-right: 2%;">
        <h3>Column A</h3>
        <template data-bind="{{model.items}}"
                  data-bind-start="0"
                  data-bind-step="2">
            <div style="padding: 8pt; border: 1pt solid #ddd; margin-bottom: 5pt;">
                {{.name}}
            </div>
        </template>
    </div>

    <!-- Right Column (Even Items) -->
    <div style="display: inline-block; width: 48%; vertical-align: top;">
        <h3>Column B</h3>
        <template data-bind="{{model.items}}"
                  data-bind-start="1"
                  data-bind-step="2">
            <div style="padding: 8pt; border: 1pt solid #ddd; margin-bottom: 5pt;">
                {{.name}}
            </div>
        </template>
    </div>
</div>
```

### 7. Every Third Item

Show every third item from a collection:

```html
<h2>Every Third Product</h2>
<template data-bind="{{model.products}}"
          data-bind-step="3">
    <div style="margin-bottom: 15pt;">
        <strong>{{.name}}</strong> - ${{.price}}
    </div>
</template>
```

### 8. Batch Processing Report (Batch 1)

Process items in batches of 50:

```html
<div style="page-break-after: always;">
    <h1>Batch 1 - Items 1-50</h1>
    <table style="width: 100%;">
        <template data-bind="{{model.allItems}}"
                  data-bind-start="0"
                  data-bind-max="50">
            <tr>
                <td>{{.id}}</td>
                <td>{{.name}}</td>
                <td>{{.status}}</td>
            </tr>
        </template>
    </table>
</div>
```

### 9. Batch Processing Report (Batch 2)

Second batch (items 51-100):

```html
<div style="page-break-after: always;">
    <h1>Batch 2 - Items 51-100</h1>
    <table style="width: 100%;">
        <template data-bind="{{model.allItems}}"
                  data-bind-start="50"
                  data-bind-max="50">
            <tr>
                <td>{{.id}}</td>
                <td>{{.name}}</td>
                <td>{{.status}}</td>
            </tr>
        </template>
    </table>
</div>
```

### 10. Sampling Every 10th Item

Create a sample report showing every 10th item:

```html
<h2>Sample Data (Every 10th Record)</h2>
<template data-bind="{{model.largeDataset}}"
          data-bind-step="10">
    <div style="padding: 10pt; border-bottom: 1pt solid #ddd;">
        <strong>{{.recordNumber}}</strong>: {{.data}}
    </div>
</template>
```

### 11. Dynamic Pagination with Binding

Use bound values to control pagination:

```html
<!-- Model: { items: [...], pageSize: 20, currentPage: 3 } -->
<!-- Calculate: start = (currentPage - 1) * pageSize = 40 -->
<h2>Page {{model.currentPage}}</h2>
<template data-bind="{{model.items}}"
          data-bind-start="{{(model.currentPage - 1) * model.pageSize}}"
          data-bind-max="{{model.pageSize}}">
    <div>{{.name}}</div>
</template>
```

### 12. Top N with Performance Optimization

Show top 50 items with style caching:

```html
<h2>Top 50 Revenue Generating Products</h2>
<template data-bind="{{model.sortedProducts}}"
          data-bind-max="50"
          data-cache-styles="true"
          data-style-identifier="top-products">
    <div style="padding: 10pt; border-bottom: 1pt solid #ccc;">
        <div style="font-weight: bold;">{{.name}}</div>
        <div style="color: #666;">Revenue: ${{.revenue}}</div>
    </div>
</template>
```

### 13. Quarter Sampling (Every 4th Item)

Show every 4th item for quarterly sampling:

```html
<h2>Quarterly Sample Data</h2>
<table style="width: 100%;">
    <thead>
        <tr>
            <th>Period</th>
            <th>Value</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.monthlyData}}"
                  data-bind-step="4">
            <tr>
                <td>{{.period}}</td>
                <td>{{.value}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 14. Limited Preview with Message

Show first 10 items with an indicator for more:

```html
<h2>Order Preview (First 10 Orders)</h2>
<template data-bind="{{model.orders}}"
          data-bind-max="10">
    <div style="padding: 8pt; border-bottom: 1pt solid #ddd;">
        Order #{{.orderNumber}} - {{.customerName}}
    </div>
</template>
<div style="margin-top: 10pt; padding: 10pt; background-color: #f0f0f0; text-align: center;"
     hidden="{{count(model.orders) <= 10 ? 'hidden' : ''}}">
    ... and {{count(model.orders) - 10}} more orders
</div>
```

### 15. Complex Multi-Page Report

Generate a multi-page report with consistent item counts per page:

```html
<!-- Page 1: Items 0-24 -->
<div style="page-break-after: always;">
    <div style="background-color: #336699; color: white; padding: 10pt;">
        <h1 style="margin: 0;">Inventory Report - Page 1</h1>
    </div>
    <table style="width: 100%; margin-top: 10pt;">
        <thead>
            <tr style="background-color: #f0f0f0;">
                <th>SKU</th>
                <th>Product</th>
                <th>Qty</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            <template data-bind="{{model.inventory}}"
                      data-bind-start="0"
                      data-bind-max="25">
                <tr>
                    <td>{{.sku}}</td>
                    <td>{{.productName}}</td>
                    <td style="text-align: right;">{{.quantity}}</td>
                    <td style="text-align: right;">${{.value}}</td>
                </tr>
            </template>
        </tbody>
    </table>
    <div style="text-align: right; margin-top: 10pt; font-size: 9pt; color: #666;">
        Page 1 - Items 1-25
    </div>
</div>

<!-- Page 2: Items 25-49 -->
<div style="page-break-after: always;">
    <div style="background-color: #336699; color: white; padding: 10pt;">
        <h1 style="margin: 0;">Inventory Report - Page 2</h1>
    </div>
    <table style="width: 100%; margin-top: 10pt;">
        <thead>
            <tr style="background-color: #f0f0f0;">
                <th>SKU</th>
                <th>Product</th>
                <th>Qty</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            <template data-bind="{{model.inventory}}"
                      data-bind-start="25"
                      data-bind-max="25">
                <tr>
                    <td>{{.sku}}</td>
                    <td>{{.productName}}</td>
                    <td style="text-align: right;">{{.quantity}}</td>
                    <td style="text-align: right;">${{.value}}</td>
                </tr>
            </template>
        </tbody>
    </table>
    <div style="text-align: right; margin-top: 10pt; font-size: 9pt; color: #666;">
        Page 2 - Items 26-50
    </div>
</div>
```

### 16. Three-Column Layout

Distribute items across three columns using step patterns:

```html
<div style="width: 100%;">
    <!-- Column 1: Items 0, 3, 6, 9... -->
    <div style="display: inline-block; width: 32%; vertical-align: top; margin-right: 1%;">
        <h3>Column 1</h3>
        <template data-bind="{{model.items}}"
                  data-bind-start="0"
                  data-bind-step="3">
            <div style="padding: 5pt; margin-bottom: 5pt; border: 1pt solid #ddd;">
                {{.name}}
            </div>
        </template>
    </div>

    <!-- Column 2: Items 1, 4, 7, 10... -->
    <div style="display: inline-block; width: 32%; vertical-align: top; margin-right: 1%;">
        <h3>Column 2</h3>
        <template data-bind="{{model.items}}"
                  data-bind-start="1"
                  data-bind-step="3">
            <div style="padding: 5pt; margin-bottom: 5pt; border: 1pt solid #ddd;">
                {{.name}}
            </div>
        </template>
    </div>

    <!-- Column 3: Items 2, 5, 8, 11... -->
    <div style="display: inline-block; width: 32%; vertical-align: top;">
        <h3>Column 3</h3>
        <template data-bind="{{model.items}}"
                  data-bind-start="2"
                  data-bind-step="3">
            <div style="padding: 5pt; margin-bottom: 5pt; border: 1pt solid #ddd;">
                {{.name}}
            </div>
        </template>
    </div>
</div>
```

### 17. Skip First N Items

Skip the first 5 items and show the rest:

```html
<h2>Items (Excluding First 5)</h2>
<template data-bind="{{model.items}}"
          data-bind-start="5">
    <div style="padding: 8pt; border-bottom: 1pt solid #eee;">
        {{.name}}
    </div>
</template>
```

### 18. Percentage Sampling (10% Sample)

Show approximately 10% of items (every 10th):

```html
<h2>10% Data Sample</h2>
<p style="color: #666; font-style: italic;">
    Showing approximately 10% of total records (every 10th item)
</p>
<template data-bind="{{model.allRecords}}"
          data-bind-step="10">
    <div style="padding: 10pt; background-color: #f9f9f9; margin-bottom: 5pt;">
        Record ID: {{.id}} - {{.data}}
    </div>
</template>
```

### 19. Combined with Nested Templates

Paginate outer collection and limit inner collection:

```html
<!-- Show first 5 categories, with top 3 products each -->
<template data-bind="{{model.categories}}"
          data-bind-max="5">
    <div style="margin-bottom: 20pt;">
        <h2 style="color: #336699;">{{.categoryName}}</h2>

        <template data-bind="{{.products}}"
                  data-bind-max="3">
            <div style="padding: 8pt; margin-left: 15pt; border-left: 3pt solid #336699;">
                <strong>{{.name}}</strong> - ${{.price}}
            </div>
        </template>
    </div>
</template>
```

### 20. Performance-Optimized Large Dataset

Handle a very large dataset with pagination and caching:

```html
<!-- Process 1000 items in chunks of 100 -->
<style>
    .data-row {
        padding: 5pt;
        border-bottom: 1pt solid #eee;
        font-size: 9pt;
    }
</style>

<h1>Large Dataset Report (Items 1-100)</h1>
<template data-bind="{{model.massiveDataset}}"
          data-bind-start="0"
          data-bind-max="100"
          data-cache-styles="true"
          data-style-identifier="large-dataset-chunk1">
    <div class="data-row">
        <span style="font-weight: bold;">{{.id}}</span> |
        {{.name}} |
        {{.value}} |
        {{.status}}
    </div>
</template>
```

---

## See Also

- [data-bind attribute](/reference/htmlattributes/data-bind.html) - Primary template iteration attribute
- [template element](/reference/htmltags/template.html) - The template HTML element
- [data-cache-styles attribute](/reference/htmlattributes/data-cache-styles.html) - Performance optimization
- [ForEach Component](/reference/components/foreach.html) - Base component class
- [Data Binding](/reference/binding/) - Complete data binding guide
- [Expressions](/reference/expressions/) - Expression syntax reference

---
