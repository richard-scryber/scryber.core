---
layout: default
title: template
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;template&gt; : The Template Element for Repeating Content
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---


## Summary

The `<template>` element is a powerful data-binding component that generates repeating content by iterating over collections. It enables dynamic content generation, data context scoping, and complex data transformations in PDF documents.

## Usage

The `<template>` element creates repeating content that:
- Iterates over arrays, lists, and enumerable collections
- Creates a new data context for each item
- Supports nested templates for hierarchical data
- Can filter and paginate data with start, step, and max count
- Works with XML, JSON, and .NET object data sources
- Supports conditional rendering through data binding
- Allows inline content definition via `data-content` attribute

```html
<template data-bind="{{model.items}}">
    <div class="item">
        <h3>{{.title}}</h3>
        <p>{{.description}}</p>
    </div>
</template>
```

---

## Supported Attributes

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | **Required**. Binds to a collection to iterate over. Each item becomes the data context for the template content. |
| `data-bind-start` | integer | Zero-based index of the first item to generate. Default is 0. |
| `data-bind-step` | integer | Step increment for iteration. Use 2 to show every other item, 3 for every third, etc. Default is 1. |
| `data-bind-max` | integer | Maximum number of items to generate. Useful for pagination or limiting output. Default is unlimited. |
| `data-content` | string | Inline HTML content to use as the template. Overrides child elements. |

### Styling and Performance Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-cache-styles` | boolean | When true, caches styles for better performance with large datasets. Default is false. |
| `data-style-identifier` | string | Unique identifier for style caching across the document. |

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `hidden` | string | Controls visibility. Set to "hidden" to prevent template rendering. |

---

## Data Context and Binding

### Current Item Reference

Within a template, use `{{.}}` to reference the current item, or `{{.propertyName}}` to access properties:

```html
<template data-bind="{{model.users}}">
    <div>
        <strong>{{.name}}</strong> - {{.email}}
    </div>
</template>
```

### Parent Context Access

Access parent context using path notation:

```html
<template data-bind="{{model.orders}}">
    <div>
        Order {{.orderNumber}} for customer {{model.customerName}}
    </div>
</template>
```

### Index and Count

The template automatically provides context about iteration:

```html
<template data-bind="{{model.items}}">
    <div>Item {{$index + 1}} of {{$count}}: {{.name}}</div>
</template>
```

---

## Notes

### Template Content

The template element itself doesn't render - only its content is generated for each iteration. The content can be:

1. **Child Elements**: Standard HTML elements as children
2. **Inline Content**: Using `data-content` attribute with HTML string
3. **Mixed Content**: Text and elements combined

### Data Types Supported

The `data-bind` attribute accepts:

- **.NET Collections**: `List<T>`, `Array`, `IEnumerable<T>`
- **JSON Arrays**: When using JSON data sources
- **XML NodeSets**: When using XPath expressions
- **Data Tables**: Rows from database results
- **Custom Enumerables**: Any type implementing `IEnumerable`

### Performance Optimization

For large datasets (100+ items):

1. Enable `data-cache-styles="true"` to reuse style calculations
2. Set `data-style-identifier` for consistent style caching
3. Use `data-bind-max` to limit items per page
4. Minimize complex expressions in binding

### Nested Templates

Templates can be nested to handle hierarchical data. Each nested template creates its own data context.

### Empty Collections

If the bound collection is empty or null, the template generates no output without error.

---

## Examples

### Basic Iteration

```html
<!-- Model: { products: [{name: "Widget", price: 19.99}, {name: "Gadget", price: 29.99}] } -->
<template data-bind="{{model.products}}">
    <div style="margin-bottom: 10pt;">
        <strong>{{.name}}</strong>: ${{.price}}
    </div>
</template>

<!-- Output: -->
<!-- Widget: $19.99 -->
<!-- Gadget: $29.99 -->
```

### Table Row Generation

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
        <template data-bind="{{model.records}}">
            <tr>
                <td>{{.id}}</td>
                <td>{{.name}}</td>
                <td>{{.status}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### Nested Templates for Hierarchical Data

```html
<!-- Model: { departments: [{name: "Sales", employees: [{name: "John"}, {name: "Jane"}]}] } -->
<template data-bind="{{model.departments}}">
    <div style="margin-bottom: 20pt; padding: 10pt; border: 1pt solid #ccc;">
        <h2>{{.name}} Department</h2>
        <ul>
            <template data-bind="{{.employees}}">
                <li>{{.name}}</li>
            </template>
        </ul>
    </div>
</template>

<!-- Output: -->
<!-- Sales Department -->
<!--   - John -->
<!--   - Jane -->
```

### Pagination with Start and Max

```html
<!-- Show items 10-19 (page 2 of 10 items per page) -->
<template data-bind="{{model.allItems}}"
          data-bind-start="10"
          data-bind-max="10">
    <div class="item">{{.title}}</div>
</template>
```

### Step Iteration (Every Other Item)

```html
<!-- Show only odd-indexed items -->
<template data-bind="{{model.items}}"
          data-bind-start="0"
          data-bind-step="2">
    <div>{{.name}}</div>
</template>

<!-- Show only even-indexed items -->
<template data-bind="{{model.items}}"
          data-bind-start="1"
          data-bind-step="2">
    <div>{{.name}}</div>
</template>
```

### Conditional Rendering with Expressions

```html
<template data-bind="{{model.items}}">
    <div hidden="{{.isHidden ? 'hidden' : ''}}">
        {{.content}}
    </div>
</template>
```

### Complex Nested Template with Totals

```html
<!-- Invoice with line items -->
<div class="invoice">
    <h1>Invoice #{{model.invoiceNumber}}</h1>
    <p>Customer: {{model.customerName}}</p>

    <table style="width: 100%; margin: 20pt 0;">
        <thead>
            <tr style="background-color: #f0f0f0;">
                <th>Description</th>
                <th style="text-align: right;">Qty</th>
                <th style="text-align: right;">Price</th>
                <th style="text-align: right;">Total</th>
            </tr>
        </thead>
        <tbody>
            <template data-bind="{{model.lineItems}}">
                <tr>
                    <td>{{.description}}</td>
                    <td style="text-align: right;">{{.quantity}}</td>
                    <td style="text-align: right;">${{.unitPrice}}</td>
                    <td style="text-align: right;">${{.total}}</td>
                </tr>
            </template>
        </tbody>
        <tfoot>
            <tr style="font-weight: bold; border-top: 2pt solid black;">
                <td colspan="3" style="text-align: right;">Total:</td>
                <td style="text-align: right;">${{model.totalAmount}}</td>
            </tr>
        </tfoot>
    </table>
</div>
```

### Multi-Level Nested Categories

```html
<!-- Model: categories with subcategories and products -->
<template data-bind="{{model.categories}}">
    <div style="margin-bottom: 30pt;">
        <h2 style="color: #336699; border-bottom: 2pt solid #336699;">
            {{.categoryName}}
        </h2>

        <template data-bind="{{.subcategories}}">
            <div style="margin: 15pt 0 15pt 20pt;">
                <h3 style="color: #666;">{{.name}}</h3>

                <template data-bind="{{.products}}">
                    <div style="margin: 5pt 0 5pt 20pt; padding: 5pt; border-left: 3pt solid #ccc;">
                        <strong>{{.productName}}</strong> - ${{.price}}
                        <br/>
                        <span style="font-size: 9pt; color: #666;">{{.description}}</span>
                    </div>
                </template>
            </div>
        </template>
    </div>
</template>
```

### Inline Content Template

```html
<!-- Using data-content for dynamic template generation -->
<template data-bind="{{model.widgets}}"
          data-content="<div style='padding: 10pt;'><strong>{{.title}}</strong><br/>{{.description}}</div>">
</template>
```

### Styled Cards with Alternating Colors

```html
<style>
    .card {
        padding: 15pt;
        margin-bottom: 10pt;
        border-radius: 5pt;
    }
    .card:nth-child(odd) {
        background-color: #f9f9f9;
    }
    .card:nth-child(even) {
        background-color: #e9e9e9;
    }
</style>

<template data-bind="{{model.articles}}">
    <div class="card">
        <h3 style="margin: 0 0 5pt 0;">{{.title}}</h3>
        <p style="margin: 0; color: #666;">{{.summary}}</p>
        <div style="margin-top: 5pt; font-size: 9pt; color: #999;">
            By {{.author}} on {{.date}}
        </div>
    </div>
</template>
```

### Directory Listing with File Types

```html
<template data-bind="{{model.files}}">
    <div style="padding: 8pt; border-bottom: 1pt solid #ddd;">
        <div style="display: inline-block; width: 40%;">
            <img src="{{.iconUrl}}" style="width: 16pt; height: 16pt; vertical-align: middle;"/>
            <span style="margin-left: 5pt;">{{.filename}}</span>
        </div>
        <div style="display: inline-block; width: 30%;">
            {{.fileType}}
        </div>
        <div style="display: inline-block; width: 30%; text-align: right;">
            {{.fileSize}}
        </div>
    </div>
</template>
```

### Chart Data Labels

```html
<!-- Generating labels for a bar chart -->
<div style="position: relative; height: 300pt;">
    <template data-bind="{{model.chartData}}">
        <div style="position: absolute;
                    left: {{$index * 50}}pt;
                    bottom: 0;
                    width: 40pt;
                    height: {{.value * 2}}pt;
                    background-color: #336699;">
        </div>
        <div style="position: absolute;
                    left: {{$index * 50}}pt;
                    bottom: -20pt;
                    width: 40pt;
                    text-align: center;
                    font-size: 8pt;">
            {{.label}}
        </div>
    </template>
</div>
```

### Timeline with Date Grouping

```html
<template data-bind="{{model.events}}">
    <div style="margin-bottom: 15pt; padding-left: 20pt; border-left: 2pt solid #336699;">
        <div style="font-weight: bold; color: #336699; margin-bottom: 5pt;">
            {{.date}}
        </div>
        <template data-bind="{{.items}}">
            <div style="margin-bottom: 8pt; padding-left: 15pt;">
                <div style="font-weight: bold;">{{.time}} - {{.title}}</div>
                <div style="color: #666; font-size: 9pt;">{{.description}}</div>
            </div>
        </template>
    </div>
</template>
```

### Performance-Optimized Large Dataset

```html
<!-- Efficient rendering of 1000+ items -->
<template data-bind="{{model.largeDataset}}"
          data-cache-styles="true"
          data-style-identifier="large-list-item">
    <div class="list-item">
        {{.name}} - {{.value}}
    </div>
</template>
```

### Conditional Sections

```html
<!-- Only render template if collection has items -->
<div style="border: 1pt solid #ccc; padding: 10pt;">
    <h3>Available Products</h3>
    <template data-bind="{{model.products}}">
        <div class="product">
            {{.name}} - ${{.price}}
        </div>
    </template>
    <div hidden="{{model.products.length > 0 ? 'hidden' : ''}}">
        <em>No products available.</em>
    </div>
</div>
```

### Master-Detail Report

```html
<template data-bind="{{model.customers}}">
    <div style="page-break-before: always; padding: 20pt;">
        <!-- Customer Header -->
        <div style="background-color: #336699; color: white; padding: 10pt; margin-bottom: 15pt;">
            <h1 style="margin: 0;">{{.companyName}}</h1>
            <div>Contact: {{.contactName}} | Phone: {{.phone}}</div>
        </div>

        <!-- Customer Orders -->
        <h2>Orders</h2>
        <table style="width: 100%; margin-bottom: 20pt;">
            <thead>
                <tr style="background-color: #f0f0f0;">
                    <th>Order Date</th>
                    <th>Order #</th>
                    <th style="text-align: right;">Amount</th>
                </tr>
            </thead>
            <tbody>
                <template data-bind="{{.orders}}">
                    <tr>
                        <td>{{.orderDate}}</td>
                        <td>{{.orderNumber}}</td>
                        <td style="text-align: right;">${{.amount}}</td>
                    </tr>
                </template>
            </tbody>
        </table>

        <!-- Order Details -->
        <h2>Order Details</h2>
        <template data-bind="{{.orders}}">
            <div style="margin-bottom: 20pt; padding: 10pt; border: 1pt solid #ddd;">
                <h3>Order {{.orderNumber}} - {{.orderDate}}</h3>
                <table style="width: 100%;">
                    <thead>
                        <tr>
                            <th>Product</th>
                            <th style="text-align: right;">Qty</th>
                            <th style="text-align: right;">Price</th>
                            <th style="text-align: right;">Total</th>
                        </tr>
                    </thead>
                    <tbody>
                        <template data-bind="{{.items}}">
                            <tr>
                                <td>{{.productName}}</td>
                                <td style="text-align: right;">{{.quantity}}</td>
                                <td style="text-align: right;">${{.unitPrice}}</td>
                                <td style="text-align: right;">${{.lineTotal}}</td>
                            </tr>
                        </template>
                    </tbody>
                    <tfoot>
                        <tr style="font-weight: bold;">
                            <td colspan="3" style="text-align: right;">Order Total:</td>
                            <td style="text-align: right;">${{.amount}}</td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </template>
    </div>
</template>
```

---

## See Also

- [Data Binding](/reference/binding/) - Complete guide to data binding expressions
- [ForEach Component](/reference/components/foreach.html) - Base ForEach component in Scryber namespace
- [Data Sources](/reference/datasources/) - XML, JSON, and database data sources
- [div](/reference/htmltags/div.html) - Generic container element
- [span](/reference/htmltags/span.html) - Inline container element
- [Expressions](/reference/expressions/) - Expression syntax and functions

---
