---
layout: default
title: data-style-identifier
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-style-identifier : The Style Identification Attribute

The `data-style-identifier` attribute provides a unique identifier for caching and optimizing style application across repeated elements. It enables Scryber to cache computed styles for elements that share the same styling, significantly improving performance when generating PDFs with many similar elements.

## Usage

The `data-style-identifier` attribute is used to:
- Enable style caching for repeated elements
- Optimize PDF generation performance
- Reduce memory usage for large tables and lists
- Improve rendering speed for data-bound templates
- Share style calculations across identical elements

```html
<!-- Without identifier - styles computed for each row -->
<template data-bind="{{model.items}}">
    <tr>
        <td style="padding: 8pt; border: 1pt solid #ccc;">{{.name}}</td>
    </tr>
</template>

<!-- With identifier - styles computed once and cached -->
<template data-bind="{{model.items}}" data-style-identifier="item-row">
    <tr>
        <td style="padding: 8pt; border: 1pt solid #ccc;">{{.name}}</td>
    </tr>
</template>
```

---

## Supported Elements

The `data-style-identifier` attribute is primarily supported by:

| Element | Description |
|---------|-------------|
| `<template>` | Template element for repeated content |
| `<tr>` | Table row element |
| `<td>` / `<th>` | Table cell elements |
| Other repeating elements | Any element that repeats with identical styling |

---

## Attribute Values

### Syntax

```html
<template data-bind="{{collection}}" data-style-identifier="unique-id">
    <!-- Template content -->
</template>
```

### Value Type

| Type | Description | Example |
|------|-------------|---------|
| String | Unique identifier for the style | `data-style-identifier="row-style"` |
| Descriptive | Meaningful name for the cached style | `data-style-identifier="product-item"` |
| Namespaced | Hierarchical identifier | `data-style-identifier="table.row.data"` |

### Naming Recommendations

**Good identifiers:**
- Descriptive: `"product-row"`, `"data-cell"`, `"list-item"`
- Unique per style group: Different identifiers for different styling
- Consistent: Use the same identifier for all elements with identical styling
- Simple: Short, readable names

**Avoid:**
- Non-unique IDs: Using the same ID for different styles
- Generic names: `"style1"`, `"temp"`, `"x"`
- Special characters: Stick to alphanumeric and hyphens/underscores

---

## Binding Values

The `data-style-identifier` attribute supports static and dynamic values:

### Static Identifier

```html
<template data-bind="{{model.products}}" data-style-identifier="product-row">
    <tr>
        <td>{{.name}}</td>
        <td>{{.price}}</td>
    </tr>
</template>
```

### Dynamic Identifier

```html
<!-- Model: { styleId: "custom-row-style" } -->
<template data-bind="{{model.items}}" data-style-identifier="{{model.styleId}}">
    <tr>
        <td>{{.value}}</td>
    </tr>
</template>
```

---

## Notes

### Performance Optimization

The `data-style-identifier` enables significant performance improvements:

**Without style identifier:**
- Each element's styles are computed independently
- Memory usage scales with element count
- Processing time increases linearly

**With style identifier:**
- Styles computed once and cached
- Memory usage dramatically reduced
- Processing time nearly constant regardless of element count

### When to Use

Use `data-style-identifier` when:

1. **Large Datasets**: Templates generating 100+ elements
2. **Complex Styling**: Elements with multiple CSS properties
3. **Identical Styling**: All generated elements have the same appearance
4. **Performance Critical**: PDF generation time is important
5. **Memory Constrained**: Large tables or lists causing memory issues

### Style Cache Behavior

How style caching works:

1. First element with the identifier is processed
2. Computed styles are cached with the identifier as key
3. Subsequent elements with the same identifier reuse cached styles
4. Only applies when styling is truly identical

### Cache Invalidation

The style cache is reset when:
- A new PDF document starts generation
- Different styling is applied (different CSS classes or inline styles)
- The identifier changes

### Combining with Other Optimizations

For maximum performance, combine with:

```html
<template data-bind="{{model.largeDataset}}"
          data-style-identifier="optimized-row"
          data-cache-styles="true">
    <tr>
        <td class="data-cell">{{.value}}</td>
    </tr>
</template>
```

---

## Examples

### Example 1: Basic Table Row Optimization

```html
<!-- Large table with 1000+ rows -->
<table border="1" cellpadding="8" style="width: 100%;">
    <thead>
        <tr>
            <th>Product</th>
            <th>Quantity</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.products}}"
                  data-style-identifier="product-row">
            <tr>
                <td style="padding: 8pt; border: 1pt solid #ddd;">{{.name}}</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: center;">{{.quantity}}</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: right;">{{.price}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### Example 2: List Items with Shared Styling

```html
<ul style="list-style: none; padding: 0;">
    <template data-bind="{{model.items}}"
              data-style-identifier="list-item-style">
        <li style="padding: 10pt; border-bottom: 1pt solid #eee; margin-bottom: 5pt;">
            <strong>{{.title}}</strong><br/>
            {{.description}}
        </li>
    </template>
</ul>
```

### Example 3: Invoice Line Items

```html
<!-- Model: { lineItems: [...] } (potentially 100+ items) -->

<table border="1" cellpadding="10" style="width: 100%;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th>Description</th>
            <th>Qty</th>
            <th>Rate</th>
            <th>Amount</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.lineItems}}"
                  data-style-identifier="invoice-line">
            <tr>
                <td style="padding: 8pt; border: 1pt solid #ddd;">{{.description}}</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: center;">{{.quantity}}</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: right;">${{.rate}}</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: right; font-weight: bold;">${{.total}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### Example 4: Data Grid with Complex Styling

```html
<style>
    .data-row {
        padding: 8pt;
        border-bottom: 1pt solid #e0e0e0;
        font-size: 10pt;
    }
    .data-row:nth-child(even) {
        background-color: #f9f9f9;
    }
</style>

<template data-bind="{{model.records}}"
          data-style-identifier="data-grid-row">
    <div class="data-row">
        <span style="display: inline-block; width: 20%;">{{.id}}</span>
        <span style="display: inline-block; width: 40%;">{{.name}}</span>
        <span style="display: inline-block; width: 40%;">{{.email}}</span>
    </div>
</template>
```

### Example 5: Product Catalog Items

```html
<template data-bind="{{model.products}}"
          data-style-identifier="catalog-product">
    <div style="page-break-inside: avoid; margin-bottom: 20pt; padding: 15pt;
                border: 1pt solid #ddd; border-radius: 5pt;">
        <h3 style="margin: 0 0 10pt 0; color: #336699;">{{.name}}</h3>
        <p style="margin: 5pt 0; color: #666;">SKU: {{.sku}}</p>
        <p style="margin: 10pt 0;">{{.description}}</p>
        <div style="font-size: 18pt; font-weight: bold; color: #336699;">
            ${{.price}}
        </div>
    </div>
</template>
```

### Example 6: Multiple Identifier Groups

```html
<!-- Different identifiers for different row types -->
<table border="1" cellpadding="8" style="width: 100%;">
    <tbody>
        <!-- Header rows -->
        <template data-bind="{{model.categories}}"
                  data-style-identifier="category-header">
            <tr style="background-color: #336699; color: white; font-weight: bold;">
                <td colspan="3">{{.name}}</td>
            </tr>
        </template>

        <!-- Data rows -->
        <template data-bind="{{model.items}}"
                  data-style-identifier="data-row">
            <tr>
                <td>{{.name}}</td>
                <td>{{.quantity}}</td>
                <td>{{.price}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### Example 7: Nested Templates with Identifiers

```html
<template data-bind="{{model.departments}}"
          data-style-identifier="department-section">
    <div style="margin-bottom: 30pt;">
        <h2 style="color: #336699; border-bottom: 2pt solid #336699;">
            {{.name}}
        </h2>

        <template data-bind="{{.employees}}"
                  data-style-identifier="employee-row">
            <div style="padding: 8pt; border-bottom: 1pt solid #eee;">
                <strong>{{.name}}</strong> - {{.position}}
            </div>
        </template>
    </div>
</template>
```

### Example 8: Transaction Log

```html
<!-- Model: { transactions: [...] } (potentially thousands) -->

<h2>Transaction History</h2>

<template data-bind="{{model.transactions}}"
          data-style-identifier="transaction-entry">
    <div style="padding: 10pt; border-bottom: 1pt solid #ddd; font-size: 10pt;">
        <div style="display: flex; justify-content: space-between;">
            <span><strong>{{.date}}</strong> - {{.description}}</span>
            <span style="font-weight: bold; {{.amount >= 0 ? 'color: green;' : 'color: red;'}}">
                ${{.amount}}
            </span>
        </div>
    </div>
</template>
```

### Example 9: Report with Alternating Row Colors

```html
<table border="1" cellpadding="10" style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th>Date</th>
            <th>Activity</th>
            <th>Status</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.activities}}"
                  data-style-identifier="activity-row">
            <tr style="border: 1pt solid #ddd;">
                <td style="padding: 8pt;">{{.date}}</td>
                <td style="padding: 8pt;">{{.activity}}</td>
                <td style="padding: 8pt; text-align: center;">
                    <span style="padding: 4pt 8pt; border-radius: 3pt;
                                 background-color: {{.status === 'completed' ? '#d4edda' : '#fff3cd'}};">
                        {{.status}}
                    </span>
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### Example 10: Performance-Critical Large Dataset

```html
<!-- Optimized for 10,000+ records -->
<style>
    .record {
        padding: 5pt;
        border-bottom: 1pt solid #f0f0f0;
        font-size: 9pt;
    }
</style>

<template data-bind="{{model.largeDataset}}"
          data-style-identifier="large-dataset-row"
          data-cache-styles="true">
    <div class="record">
        <span>{{.id}}</span> |
        <span>{{.value}}</span> |
        <span>{{.timestamp}}</span>
    </div>
</template>
```

### Example 11: Customer List

```html
<h2>Customer Directory</h2>

<template data-bind="{{model.customers}}"
          data-style-identifier="customer-card">
    <div style="padding: 15pt; margin-bottom: 10pt;
                border: 1pt solid #ddd; background-color: #f9f9f9;">
        <h3 style="margin: 0 0 5pt 0; color: #336699;">{{.companyName}}</h3>
        <p style="margin: 3pt 0;">
            <strong>Contact:</strong> {{.contactName}}<br/>
            <strong>Email:</strong> {{.email}}<br/>
            <strong>Phone:</strong> {{.phone}}
        </p>
    </div>
</template>
```

### Example 12: Dynamic Identifier Based on Data

```html
<!-- Model: { items: [...], useCompactStyle: true } -->

<template data-bind="{{model.items}}"
          data-style-identifier="{{model.useCompactStyle ? 'compact-row' : 'expanded-row'}}">
    <div style="{{model.useCompactStyle ? 'padding: 5pt;' : 'padding: 15pt;'}}
                border-bottom: 1pt solid #eee;">
        {{.content}}
    </div>
</template>
```

### Example 13: Financial Statement Rows

```html
<table border="1" cellpadding="10" style="width: 100%;">
    <tbody>
        <!-- Account entries (potentially hundreds) -->
        <template data-bind="{{model.accounts}}"
                  data-style-identifier="account-entry">
            <tr>
                <td style="padding: 8pt;">{{.accountNumber}}</td>
                <td style="padding: 8pt;">{{.accountName}}</td>
                <td style="padding: 8pt; text-align: right; font-family: monospace;">
                    ${{.balance}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### Example 14: Email Campaign Report

```html
<h2>Campaign Performance Metrics</h2>

<template data-bind="{{model.campaigns}}"
          data-style-identifier="campaign-metric">
    <div style="padding: 15pt; margin-bottom: 15pt;
                border-left: 4pt solid #336699; background-color: #f0f0f0;">
        <h3 style="margin: 0 0 10pt 0;">{{.campaignName}}</h3>
        <table style="width: 100%; font-size: 10pt;">
            <tr>
                <td>Sent:</td>
                <td style="text-align: right; font-weight: bold;">{{.sent}}</td>
            </tr>
            <tr>
                <td>Opened:</td>
                <td style="text-align: right; font-weight: bold;">{{.opened}} ({{.openRate}}%)</td>
            </tr>
            <tr>
                <td>Clicked:</td>
                <td style="text-align: right; font-weight: bold;">{{.clicked}} ({{.clickRate}}%)</td>
            </tr>
        </table>
    </div>
</template>
```

### Example 15: Complete Optimized Report

```html
<!-- Model: { sections: [{title: "Section", items: [...]}] } -->

<style>
    .section-header {
        padding: 12pt;
        background-color: #336699;
        color: white;
        font-weight: bold;
        margin-bottom: 10pt;
    }

    .item-row {
        padding: 8pt;
        border-bottom: 1pt solid #e0e0e0;
        font-size: 10pt;
    }
</style>

<template data-bind="{{model.sections}}"
          data-style-identifier="report-section">
    <div style="margin-bottom: 30pt;">
        <div class="section-header">
            {{.title}}
        </div>

        <!-- Nested template with its own identifier -->
        <template data-bind="{{.items}}"
                  data-style-identifier="section-item">
            <div class="item-row">
                <span style="display: inline-block; width: 30%;">{{.code}}</span>
                <span style="display: inline-block; width: 50%;">{{.description}}</span>
                <span style="display: inline-block; width: 20%; text-align: right;">{{.value}}</span>
            </div>
        </template>
    </div>
</template>
```

---

## See Also

- [template element](/reference/htmltags/template.html) - Template element for iteration
- [data-bind attribute](/reference/htmlattributes/data-bind.html) - Data binding for templates
- [Performance Optimization](/reference/performance/) - PDF generation optimization
- [Style Caching](/reference/performance/style-caching.html) - Style caching details
- [Data Binding](/reference/binding/) - Complete data binding reference
- [Large Datasets](/reference/performance/large-datasets.html) - Handling large datasets

---
