---
layout: default
title: data-cache-styles
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-cache-styles : The Style Caching Performance Attribute

The `data-cache-styles` attribute is a performance optimization feature that caches style calculations for template-generated content. When enabled, Scryber reuses style computations across repeated template instances, dramatically reducing processing time and memory usage for large datasets.

---

## Summary

The `data-cache-styles` attribute enables style calculation caching for template iterations, providing:
- **Significant performance improvements** for large collections (100+ items)
- **Reduced memory footprint** during PDF generation
- **Faster rendering times** by reusing computed styles
- **Consistent styling** across all template instances

This attribute is essential for:
- Processing large datasets (hundreds to thousands of items)
- High-performance batch PDF generation
- Reports with extensive repeated content
- Memory-constrained environments
- Time-sensitive document generation

**Performance Impact**: In testing with 1000+ items, enabling style caching can reduce generation time by 40-60% and memory usage by 30-50%.

---

## Usage

The `data-cache-styles` attribute is applied to `<template>` elements, typically in conjunction with `data-bind`:

```html
<template data-bind="{{model.items}}"
          data-cache-styles="true"
          data-style-identifier="unique-cache-id">
    <!-- Content with consistent styling -->
    <div class="item">{{.name}}</div>
</template>
```

### Basic Syntax

```html
<!-- Enable style caching -->
<template data-bind="{{collection}}"
          data-cache-styles="true">
    <!-- Template content -->
</template>

<!-- Enable with explicit identifier -->
<template data-bind="{{collection}}"
          data-cache-styles="true"
          data-style-identifier="my-cache-key">
    <!-- Template content -->
</template>

<!-- Disable style caching (default) -->
<template data-bind="{{collection}}"
          data-cache-styles="false">
    <!-- Template content -->
</template>
```

---

## Supported Elements

The `data-cache-styles` attribute is **only** supported on the following element:

- `<template>` - The template element for repeating content

**Note**: This attribute only affects template iterations. It has no effect on non-template elements.

---

## Binding Values

### Attribute Values

| Value | Description |
|-------|-------------|
| `true` | Enable style caching for this template |
| `false` | Disable style caching (default behavior) |

### Type

**Type**: `boolean`
**Default**: `false`

### Expression Support

The attribute accepts literal boolean values or binding expressions:

```html
<!-- Literal value -->
<template data-bind="{{items}}" data-cache-styles="true">

<!-- Bound value (enable for large datasets) -->
<template data-bind="{{items}}"
          data-cache-styles="{{count(items) > 100}}">

<!-- Bound from model property -->
<template data-bind="{{items}}"
          data-cache-styles="{{model.enableCaching}}">
```

---

## Notes

### How Style Caching Works

When `data-cache-styles="true"`:

1. **First Iteration**: Scryber computes all styles for template content (CSS parsing, inheritance, specificity resolution)
2. **Cache Storage**: Computed styles are stored with the cache identifier
3. **Subsequent Iterations**: Scryber retrieves cached styles instead of recomputing
4. **Memory Benefits**: Only one set of style objects is maintained, referenced by all instances

### Cache Identifier

The `data-style-identifier` attribute works with `data-cache-styles`:

```html
<template data-bind="{{items}}"
          data-cache-styles="true"
          data-style-identifier="item-cache">
```

- If not specified, Scryber generates an automatic identifier
- Explicit identifiers enable cache sharing across multiple templates
- Use unique identifiers for templates with different styling

### When to Enable Style Caching

**Enable style caching when**:
- Collection has 100+ items
- Template content has consistent styling across iterations
- Generating multiple similar documents
- Performance or memory is a concern
- Template uses complex CSS or class hierarchies

**Don't enable style caching when**:
- Collection has fewer than 50 items (minimal benefit)
- Styles vary significantly between items (e.g., dynamic colors based on data)
- Using inline styles with item-specific values
- Template content structure changes per item

### Style Variation and Caching

Style caching works best with **static styles**:

```html
<!-- GOOD: Static styles, perfect for caching -->
<template data-bind="{{items}}" data-cache-styles="true">
    <div class="item-card">
        <h3>{{.name}}</h3>
        <p>{{.description}}</p>
    </div>
</template>

<!-- ACCEPTABLE: Dynamic content, static structure -->
<template data-bind="{{items}}" data-cache-styles="true">
    <div class="item" style="padding: 10pt;">
        {{.dynamicContent}}
    </div>
</template>

<!-- NOT IDEAL: Dynamic inline styles -->
<template data-bind="{{items}}" data-cache-styles="false">
    <div style="color: {{.color}}; background: {{.bgColor}};">
        {{.content}}
    </div>
</template>
```

### Performance Guidelines

**Small Collections (< 50 items)**:
- Caching overhead may exceed benefits
- Generally not recommended

**Medium Collections (50-500 items)**:
- Moderate performance improvement (20-30%)
- Recommended if generation time is critical

**Large Collections (500-1000 items)**:
- Significant performance improvement (40-50%)
- Highly recommended

**Very Large Collections (1000+ items)**:
- Dramatic performance improvement (50-60%+)
- Essential for acceptable performance
- Should be combined with pagination (`data-bind-max`)

### Memory Considerations

Style caching reduces memory usage by:
- Storing one copy of style objects instead of N copies
- Reducing garbage collection pressure
- Enabling larger batch processing

### Combining with Other Optimizations

For maximum performance with large datasets:

```html
<style>
    /* Use CSS classes instead of inline styles */
    .data-row { padding: 5pt; border-bottom: 1pt solid #ddd; }
</style>

<template data-bind="{{model.largeDataset}}"
          data-cache-styles="true"
          data-style-identifier="large-data"
          data-bind-max="100">
    <div class="data-row">
        {{.name}} - {{.value}}
    </div>
</template>
```

Optimizations combined:
1. CSS classes (reusable styles)
2. Style caching (avoid recomputation)
3. Pagination (limit items per generation)

---

## Examples

### 1. Basic Style Caching

Enable caching for a simple list:

```html
<template data-bind="{{model.items}}"
          data-cache-styles="true">
    <div style="padding: 10pt; border-bottom: 1pt solid #ddd;">
        {{.name}}
    </div>
</template>
```

### 2. Large Table with Caching

Optimize a large data table:

```html
<table style="width: 100%;">
    <thead>
        <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Status</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.records}}"
                  data-cache-styles="true"
                  data-style-identifier="table-rows">
            <tr>
                <td>{{.id}}</td>
                <td>{{.name}}</td>
                <td>{{.status}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 3. Product Catalog with CSS Classes

Use CSS classes with caching for consistency:

```html
<style>
    .product-card {
        padding: 15pt;
        margin-bottom: 10pt;
        border: 1pt solid #ddd;
        border-radius: 5pt;
    }
    .product-name {
        font-weight: bold;
        font-size: 12pt;
        color: #336699;
    }
    .product-price {
        color: #009900;
        font-size: 14pt;
    }
</style>

<template data-bind="{{model.products}}"
          data-cache-styles="true"
          data-style-identifier="product-list">
    <div class="product-card">
        <div class="product-name">{{.name}}</div>
        <p>{{.description}}</p>
        <div class="product-price">${{.price}}</div>
    </div>
</template>
```

### 4. Conditional Caching Based on Dataset Size

Enable caching only for large datasets:

```html
<!-- Model: { items: [...], enableCache: items.length > 100 } -->
<template data-bind="{{model.items}}"
          data-cache-styles="{{count(model.items) > 100}}">
    <div style="padding: 8pt;">
        {{.name}}
    </div>
</template>
```

### 5. Multi-Template with Shared Cache

Share cache identifier across related templates:

```html
<style>
    .list-item {
        padding: 10pt;
        border-bottom: 1pt solid #eee;
    }
</style>

<!-- First template -->
<h2>Active Items</h2>
<template data-bind="{{model.activeItems}}"
          data-cache-styles="true"
          data-style-identifier="item-style">
    <div class="list-item">{{.name}} - Active</div>
</template>

<!-- Second template with same cache -->
<h2>Inactive Items</h2>
<template data-bind="{{model.inactiveItems}}"
          data-cache-styles="true"
          data-style-identifier="item-style">
    <div class="list-item">{{.name}} - Inactive</div>
</template>
```

### 6. Invoice Line Items

Optimize repeating invoice lines:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #f0f0f0;">
            <th style="text-align: left;">Description</th>
            <th style="text-align: right;">Qty</th>
            <th style="text-align: right;">Unit Price</th>
            <th style="text-align: right;">Total</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.lineItems}}"
                  data-cache-styles="true"
                  data-style-identifier="invoice-lines">
            <tr>
                <td style="padding: 5pt;">{{.description}}</td>
                <td style="text-align: right; padding: 5pt;">{{.quantity}}</td>
                <td style="text-align: right; padding: 5pt;">${{.unitPrice}}</td>
                <td style="text-align: right; padding: 5pt; font-weight: bold;">${{.total}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 7. Paginated Large Dataset

Combine caching with pagination:

```html
<h1>Records 1-100 of {{count(model.allRecords)}}</h1>
<template data-bind="{{model.allRecords}}"
          data-bind-start="0"
          data-bind-max="100"
          data-cache-styles="true"
          data-style-identifier="record-page-1">
    <div style="padding: 5pt; border-bottom: 1pt solid #ddd; font-size: 9pt;">
        <strong>{{.id}}</strong> | {{.name}} | {{.value}}
    </div>
</template>
```

### 8. Nested Templates with Selective Caching

Cache outer template only:

```html
<!-- Cache outer template (many categories) -->
<template data-bind="{{model.categories}}"
          data-cache-styles="true"
          data-style-identifier="category-headers">
    <div style="margin-bottom: 20pt;">
        <h2 style="color: #336699; border-bottom: 2pt solid #336699;">
            {{.categoryName}}
        </h2>

        <!-- Don't cache inner template (few items per category) -->
        <template data-bind="{{.products}}"
                  data-cache-styles="false">
            <div style="padding: 8pt; margin-left: 15pt;">
                {{.name}} - ${{.price}}
            </div>
        </template>
    </div>
</template>
```

### 9. Employee Directory

Large employee list with caching:

```html
<style>
    .employee-card {
        padding: 12pt;
        margin-bottom: 8pt;
        border: 1pt solid #ddd;
        background-color: #f9f9f9;
    }
    .employee-name {
        font-weight: bold;
        font-size: 11pt;
    }
    .employee-details {
        font-size: 9pt;
        color: #666;
        margin-top: 3pt;
    }
</style>

<template data-bind="{{model.employees}}"
          data-cache-styles="true"
          data-style-identifier="employee-directory">
    <div class="employee-card">
        <div class="employee-name">{{.firstName}} {{.lastName}}</div>
        <div class="employee-details">
            {{.department}} | {{.title}}<br/>
            Email: {{.email}} | Phone: {{.phone}}
        </div>
    </div>
</template>
```

### 10. Transaction Log

High-volume transaction listing:

```html
<h1>Transaction Log ({{count(model.transactions)}} transactions)</h1>
<table style="width: 100%; font-size: 9pt;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th style="padding: 5pt;">Date</th>
            <th style="padding: 5pt;">ID</th>
            <th style="padding: 5pt;">Type</th>
            <th style="padding: 5pt; text-align: right;">Amount</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.transactions}}"
                  data-cache-styles="true"
                  data-style-identifier="transaction-rows">
            <tr>
                <td style="padding: 3pt;">{{.date}}</td>
                <td style="padding: 3pt;">{{.transactionId}}</td>
                <td style="padding: 3pt;">{{.type}}</td>
                <td style="padding: 3pt; text-align: right;">${{.amount}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 11. Event Schedule

Large event listing with time-based grouping:

```html
<style>
    .event-item {
        padding: 10pt;
        margin-bottom: 8pt;
        border-left: 4pt solid #336699;
        background-color: #f5f5f5;
    }
    .event-time {
        font-weight: bold;
        color: #336699;
    }
</style>

<template data-bind="{{model.events}}"
          data-cache-styles="true"
          data-style-identifier="event-schedule">
    <div class="event-item">
        <div class="event-time">{{.time}}</div>
        <div>{{.title}}</div>
        <div style="font-size: 9pt; color: #666;">{{.location}}</div>
    </div>
</template>
```

### 12. Inventory Report with Alternating Rows

Use CSS pseudo-classes with caching:

```html
<style>
    .inventory-row {
        padding: 8pt;
        border-bottom: 1pt solid #ddd;
    }
    .inventory-row:nth-child(even) {
        background-color: #f9f9f9;
    }
</style>

<table style="width: 100%;">
    <template data-bind="{{model.inventoryItems}}"
              data-cache-styles="true"
              data-style-identifier="inventory-report">
        <tr class="inventory-row">
            <td>{{.sku}}</td>
            <td>{{.productName}}</td>
            <td style="text-align: right;">{{.quantity}}</td>
            <td style="text-align: right;">${{.value}}</td>
        </tr>
    </template>
</table>
```

### 13. Multi-Page Report with Consistent Styling

Cache styles across multiple report pages:

```html
<style>
    .report-section {
        padding: 15pt;
        margin-bottom: 10pt;
        border: 1pt solid #ccc;
    }
</style>

<!-- Page 1 -->
<div style="page-break-after: always;">
    <h1>Section A</h1>
    <template data-bind="{{model.sectionA}}"
              data-cache-styles="true"
              data-style-identifier="report-sections">
        <div class="report-section">
            <h3>{{.title}}</h3>
            <p>{{.content}}</p>
        </div>
    </template>
</div>

<!-- Page 2 -->
<div style="page-break-after: always;">
    <h1>Section B</h1>
    <template data-bind="{{model.sectionB}}"
              data-cache-styles="true"
              data-style-identifier="report-sections">
        <div class="report-section">
            <h3>{{.title}}</h3>
            <p>{{.content}}</p>
        </div>
    </template>
</div>
```

### 14. Customer Mailing Labels

High-volume label generation:

```html
<style>
    .mailing-label {
        width: 180pt;
        height: 90pt;
        padding: 10pt;
        border: 1pt solid #000;
        margin-bottom: 5pt;
        display: inline-block;
    }
</style>

<template data-bind="{{model.customers}}"
          data-cache-styles="true"
          data-style-identifier="mailing-labels">
    <div class="mailing-label">
        <div style="font-weight: bold;">{{.name}}</div>
        <div>{{.address1}}</div>
        <div>{{.address2}}</div>
        <div>{{.city}}, {{.state}} {{.zip}}</div>
    </div>
</template>
```

### 15. Performance Comparison Report

Demonstrate caching benefits with metrics:

```html
<!-- Without caching (baseline) -->
<div style="page-break-after: always;">
    <h1>Test 1: Without Style Caching</h1>
    <p>Generation started: {{model.startTime}}</p>
    <template data-bind="{{model.testData}}"
              data-cache-styles="false">
        <div style="padding: 5pt; border-bottom: 1pt solid #ddd;">
            {{.id}} - {{.value}}
        </div>
    </template>
    <p>Generation ended: {{model.endTime}}</p>
    <p>Items processed: {{count(model.testData)}}</p>
</div>

<!-- With caching (optimized) -->
<div>
    <h1>Test 2: With Style Caching</h1>
    <p>Generation started: {{model.startTime}}</p>
    <template data-bind="{{model.testData}}"
              data-cache-styles="true"
              data-style-identifier="performance-test">
        <div style="padding: 5pt; border-bottom: 1pt solid #ddd;">
            {{.id}} - {{.value}}
        </div>
    </template>
    <p>Generation ended: {{model.endTime}}</p>
    <p>Items processed: {{count(model.testData)}}</p>
</div>
```

### 16. Sales Report with Summary Cards

Cache consistent card layout:

```html
<style>
    .sales-card {
        padding: 12pt;
        margin-bottom: 10pt;
        border: 2pt solid #336699;
        background-color: #f0f8ff;
    }
    .sales-amount {
        font-size: 16pt;
        font-weight: bold;
        color: #336699;
    }
</style>

<template data-bind="{{model.salesData}}"
          data-cache-styles="true"
          data-style-identifier="sales-cards">
    <div class="sales-card">
        <div style="font-weight: bold;">{{.region}}</div>
        <div class="sales-amount">${{.totalSales}}</div>
        <div style="font-size: 9pt; color: #666;">
            {{.transactionCount}} transactions
        </div>
    </div>
</template>
```

### 17. Log File Viewer

Thousands of log entries with caching:

```html
<style>
    .log-entry {
        font-family: 'Courier New', monospace;
        font-size: 8pt;
        padding: 2pt 5pt;
        border-bottom: 1pt solid #eee;
    }
</style>

<h1>Application Log</h1>
<p>Total entries: {{count(model.logEntries)}}</p>
<template data-bind="{{model.logEntries}}"
          data-bind-max="1000"
          data-cache-styles="true"
          data-style-identifier="log-entries">
    <div class="log-entry">
        [{{.timestamp}}] {{.level}} - {{.message}}
    </div>
</template>
```

### 18. Product Comparison Grid

Large comparison table with caching:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th style="padding: 8pt;">Feature</th>
            <th style="padding: 8pt;">Value</th>
            <th style="padding: 8pt;">Comparison</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.features}}"
                  data-cache-styles="true"
                  data-style-identifier="feature-rows">
            <tr>
                <td style="padding: 6pt; font-weight: bold;">{{.featureName}}</td>
                <td style="padding: 6pt;">{{.value}}</td>
                <td style="padding: 6pt; text-align: center;">{{.rating}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 19. Batch Processing Status Report

Monitor large batch operations:

```html
<h1>Batch Processing Report</h1>
<p>Processed: {{model.processedCount}} of {{count(model.batchItems)}}</p>

<style>
    .batch-item {
        padding: 6pt;
        border-bottom: 1pt solid #ddd;
        font-size: 9pt;
    }
    .status-success { color: green; }
    .status-error { color: red; }
    .status-pending { color: orange; }
</style>

<template data-bind="{{model.batchItems}}"
          data-cache-styles="true"
          data-style-identifier="batch-items">
    <div class="batch-item">
        <strong>{{.itemId}}</strong> - {{.description}}
        <span class="status-{{.status}}"> [{{.status}}]</span>
    </div>
</template>
```

### 20. Complex Nested Structure with Selective Caching

Optimize only high-volume sections:

```html
<style>
    .section-header {
        background-color: #336699;
        color: white;
        padding: 10pt;
        margin-bottom: 10pt;
    }
    .subsection-item {
        padding: 5pt;
        margin-left: 15pt;
        border-left: 3pt solid #ccc;
    }
</style>

<!-- Don't cache outer (few departments) -->
<template data-bind="{{model.departments}}"
          data-cache-styles="false">
    <div style="margin-bottom: 30pt;">
        <div class="section-header">
            {{.departmentName}}
        </div>

        <!-- Cache inner (many employees per department) -->
        <template data-bind="{{.employees}}"
                  data-cache-styles="true"
                  data-style-identifier="employee-items">
            <div class="subsection-item">
                {{.name}} - {{.position}}
            </div>
        </template>
    </div>
</template>
```

---

## See Also

- [data-bind attribute](/reference/htmlattributes/data-bind.html) - Primary template iteration attribute
- [data-style-identifier attribute](/reference/htmlattributes/data-style-identifier.html) - Cache identifier
- [data-bind-max attribute](/reference/htmlattributes/data-bind-max.html) - Pagination for performance
- [template element](/reference/htmltags/template.html) - The template HTML element
- [Performance Guide](/reference/performance/) - Complete performance optimization guide
- [ForEach Component](/reference/components/foreach.html) - Base component class

---
