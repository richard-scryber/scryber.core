---
layout: default
title: data-bind
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-bind : The Template Iteration Attribute

The `data-bind` attribute enables template-based iteration over collections in Scryber PDF documents. When applied to `<template>` elements, it dynamically generates repeating content for each item in an array, list, or any enumerable data source.

---

## Summary

The `data-bind` attribute is the primary mechanism for creating dynamic, data-driven repeating content in Scryber templates. It binds a template element to a collection and generates the template's content once for each item in that collection. Each iteration creates a new data context scoped to the current item, allowing you to reference item properties using the dot notation (`.propertyName`).

This attribute is essential for:
- Generating table rows from database results
- Creating lists of items from arrays
- Building master-detail reports
- Iterating over JSON or XML data
- Dynamically generating any repeating content structure

---

## Usage

The `data-bind` attribute is applied to `<template>` elements and accepts binding expressions that evaluate to enumerable collections:

```html
<template data-bind="{{model.items}}">
    <!-- Content repeated for each item -->
    <div>{{.name}}</div>
</template>
```

### Basic Syntax

```html
<!-- Bind to a collection -->
<template data-bind="{{collection}}">
    <!-- Template content -->
</template>

<!-- With nested property access -->
<template data-bind="{{model.orders.items}}">
    <!-- Template content -->
</template>

<!-- Using XPath-style expressions -->
<template data-bind="{@:Model.Data}">
    <!-- Template content -->
</template>
```

---

## Supported Elements

The `data-bind` attribute is **only** supported on the following element:

- `<template>` - The template element for repeating content

**Note**: While `data-bind` is technically part of the ForEach component infrastructure, it is specifically designed and intended for use with the `<template>` HTML element in Scryber templates.

---

## Binding Values

### Expression Syntax

The `data-bind` attribute accepts binding expressions in two formats:

**1. Modern Expression Syntax (Recommended)**
```html
<template data-bind="{{expression}}">
```
- Uses double curly braces: `{{...}}`
- Supports JavaScript-like expressions
- Allows method calls: `{{filter(model.items, .active)}}`
- Enables calculations: `{{model.items.slice(0, 10)}}`

**2. Legacy XPath Syntax**
```html
<template data-bind="{@:expression}">
```
- Uses `{@:...}` notation
- Simple property path access
- Compatible with older Scryber templates

### Supported Data Types

The expression must evaluate to one of the following enumerable types:

| Data Type | Example | Description |
|-----------|---------|-------------|
| `.NET Array` | `new[] { "a", "b", "c" }` | Standard C# arrays |
| `.NET List` | `List<Customer>` | Generic list collections |
| `IEnumerable<T>` | `IQueryable<Order>` | Any enumerable interface |
| `DataTable.Rows` | `dataTable.Rows` | Database query results |
| `JSON Array` | `[{...}, {...}]` | Arrays from JSON sources |
| `XML NodeList` | XPath results | XML document nodes |

### Data Context Scoping

Within a template, a new data context is created for each iteration:

```html
<template data-bind="{{model.products}}">
    <!-- Current item reference -->
    {{.}}              <!-- The entire current item -->
    {{.name}}          <!-- Property of current item -->
    {{.price * 1.2}}   <!-- Expression using current item -->

    <!-- Parent context still accessible -->
    {{model.companyName}}  <!-- Access parent model -->
    {{model.taxRate}}      <!-- Use parent data -->
</template>
```

### Special Context Variables

Within `data-bind` templates, special variables are available (syntax may vary):

| Variable | Description | Example |
|----------|-------------|---------|
| `.` | Current item | `{{.name}}` |
| `$index` or `index()` | Zero-based index | `{{$index + 1}}` |
| `$count` or `count()` | Total item count | `{{$count}}` |

---

## Notes

### Template Element Behavior

- The `<template>` element itself is **not rendered** in the output
- Only the template's **content** is generated for each item
- Multiple `<template>` elements can be nested for hierarchical data

### Empty Collections

- If the bound collection is `null` or empty, no content is generated
- No error is thrown for empty collections
- Use conditional rendering to show "no items" messages

### Performance Considerations

For large collections (100+ items):

1. **Enable style caching**: Add `data-cache-styles="true"`
2. **Use style identifiers**: Set `data-style-identifier="unique-id"`
3. **Limit items per page**: Use `data-bind-max` to paginate
4. **Simplify expressions**: Avoid complex calculations in binding expressions
5. **Pre-process data**: Filter and transform data before binding when possible

### Iteration Control Attributes

The `data-bind` attribute works in conjunction with:

- `data-bind-start` - Start index for iteration (default: 0)
- `data-bind-step` - Step increment (default: 1)
- `data-bind-max` - Maximum items to process (default: unlimited)

See individual documentation for these attributes for details.

### Common Binding Patterns

**Nested Collections**
```html
<template data-bind="{{model.categories}}">
    <h2>{{.categoryName}}</h2>
    <template data-bind="{{.products}}">
        <div>{{.productName}}</div>
    </template>
</template>
```

**Accessing Parent Context**
```html
<template data-bind="{{model.orders}}">
    <div>
        Order {{.id}} for {{model.customerName}}
    </div>
</template>
```

**Filtered Collections**
```html
<template data-bind="{{filter(model.items, .active)}}">
    <div>{{.name}}</div>
</template>
```

---

## Examples

### 1. Simple List Iteration

Generate a simple list from an array:

```html
<!-- Model: { fruits: ["Apple", "Banana", "Orange", "Grape"] } -->
<ul>
    <template data-bind="{{model.fruits}}">
        <li>{{.}}</li>
    </template>
</ul>

<!-- Output: -->
<!-- • Apple -->
<!-- • Banana -->
<!-- • Orange -->
<!-- • Grape -->
```

### 2. Object Collection with Properties

Iterate over objects and access their properties:

```html
<!-- Model: { users: [{name: "John", email: "john@example.com"}, ...] } -->
<template data-bind="{{model.users}}">
    <div style="margin-bottom: 10pt;">
        <strong>{{.name}}</strong><br/>
        Email: {{.email}}
    </div>
</template>
```

### 3. Table Row Generation

Create table rows dynamically:

```html
<table style="width: 100%;">
    <thead>
        <tr>
            <th>#</th>
            <th>Product</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.products}}">
            <tr>
                <td>{{index() + 1}}</td>
                <td>{{.productName}}</td>
                <td>${{.price}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 4. Nested Template for Hierarchical Data

Handle multi-level data structures:

```html
<!-- Model: { departments: [{name: "Sales", employees: [...]}] } -->
<template data-bind="{{model.departments}}">
    <div style="page-break-inside: avoid; margin-bottom: 20pt;">
        <h2 style="background-color: #336699; color: white; padding: 10pt;">
            {{.name}} Department
        </h2>
        <table style="width: 100%;">
            <template data-bind="{{.employees}}">
                <tr>
                    <td>{{.name}}</td>
                    <td>{{.position}}</td>
                    <td>{{.email}}</td>
                </tr>
            </template>
        </table>
    </div>
</template>
```

### 5. Complex Expression Binding

Use expressions to transform data:

```html
<template data-bind="{{model.items}}">
    <div>
        <!-- Calculated values -->
        <strong>{{.name}}</strong><br/>
        Subtotal: ${{.price * .quantity}}<br/>
        Tax: ${{(.price * .quantity) * 0.08}}<br/>
        Total: ${{(.price * .quantity) * 1.08}}
    </div>
</template>
```

### 6. Accessing Parent Model in Nested Template

Reference parent context from within nested templates:

```html
<!-- Model: { companyName: "Acme Corp", orders: [...] } -->
<template data-bind="{{model.orders}}">
    <div>
        <h3>Order #{{.orderNumber}}</h3>
        <p>Company: {{model.companyName}}</p>

        <template data-bind="{{.items}}">
            <div>
                {{.product}} - {{.quantity}} units
                (Order: {{..orderNumber}})
            </div>
        </template>
    </div>
</template>
```

### 7. Conditional Item Display

Use expressions to conditionally render items:

```html
<template data-bind="{{model.products}}">
    <div hidden="{{.discontinued ? 'hidden' : ''}}">
        {{.name}} - ${{.price}}
    </div>
</template>
```

### 8. Invoice Line Items with Totals

Generate invoice sections with calculations:

```html
<h1>Invoice #{{model.invoiceNumber}}</h1>
<table style="width: 100%; margin: 20pt 0;">
    <thead>
        <tr style="background-color: #f0f0f0; font-weight: bold;">
            <th style="text-align: left;">Description</th>
            <th style="text-align: right;">Qty</th>
            <th style="text-align: right;">Unit Price</th>
            <th style="text-align: right;">Total</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.lineItems}}">
            <tr>
                <td>{{.description}}</td>
                <td style="text-align: right;">{{.quantity}}</td>
                <td style="text-align: right;">${{.unitPrice}}</td>
                <td style="text-align: right;">${{.lineTotal}}</td>
            </tr>
        </template>
    </tbody>
    <tfoot>
        <tr style="font-weight: bold; border-top: 2pt solid black;">
            <td colspan="3" style="text-align: right;">Subtotal:</td>
            <td style="text-align: right;">${{model.subtotal}}</td>
        </tr>
        <tr style="font-weight: bold;">
            <td colspan="3" style="text-align: right;">Tax ({{model.taxRate}}%):</td>
            <td style="text-align: right;">${{model.taxAmount}}</td>
        </tr>
        <tr style="font-weight: bold; font-size: 14pt;">
            <td colspan="3" style="text-align: right;">Total:</td>
            <td style="text-align: right;">${{model.total}}</td>
        </tr>
    </tfoot>
</table>
```

### 9. Multi-Page Customer Report

Generate one page per customer with order details:

```html
<template data-bind="{{model.customers}}">
    <div style="page-break-before: always;">
        <!-- Customer header -->
        <div style="background-color: #336699; color: white; padding: 15pt; margin-bottom: 20pt;">
            <h1 style="margin: 0;">{{.companyName}}</h1>
            <div style="margin-top: 5pt;">
                Contact: {{.contactName}} | Phone: {{.phone}} | Email: {{.email}}
            </div>
        </div>

        <!-- Customer summary -->
        <div style="margin-bottom: 20pt; padding: 10pt; background-color: #f0f0f0;">
            <strong>Account Summary</strong><br/>
            Total Orders: {{.totalOrders}}<br/>
            Total Revenue: ${{.totalRevenue}}<br/>
            Average Order: ${{.averageOrder}}
        </div>

        <!-- Order history -->
        <h2>Order History</h2>
        <table style="width: 100%;">
            <thead>
                <tr style="background-color: #e0e0e0;">
                    <th>Date</th>
                    <th>Order #</th>
                    <th style="text-align: right;">Amount</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                <template data-bind="{{.orders}}">
                    <tr>
                        <td>{{.orderDate}}</td>
                        <td>{{.orderNumber}}</td>
                        <td style="text-align: right;">${{.amount}}</td>
                        <td>{{.status}}</td>
                    </tr>
                </template>
            </tbody>
        </table>
    </div>
</template>
```

### 10. Catalog with Categories and Products

Three-level nested template structure:

```html
<template data-bind="{{model.categories}}">
    <div style="margin-bottom: 40pt;">
        <h1 style="color: #336699; border-bottom: 3pt solid #336699; padding-bottom: 5pt;">
            {{.categoryName}}
        </h1>
        <p style="font-style: italic; color: #666;">{{.description}}</p>

        <template data-bind="{{.subcategories}}">
            <div style="margin: 20pt 0 20pt 20pt;">
                <h2 style="color: #666; border-bottom: 1pt solid #ccc;">
                    {{.name}}
                </h2>

                <div style="column-count: 2; column-gap: 20pt;">
                    <template data-bind="{{.products}}">
                        <div style="break-inside: avoid; margin-bottom: 15pt; padding: 10pt; border: 1pt solid #ddd;">
                            <h3 style="margin: 0 0 5pt 0; color: #336699;">
                                {{.productName}}
                            </h3>
                            <div style="color: #666; font-size: 9pt; margin-bottom: 5pt;">
                                SKU: {{.sku}}
                            </div>
                            <p style="margin: 5pt 0;">{{.description}}</p>
                            <div style="font-weight: bold; font-size: 12pt; color: #336699;">
                                ${{.price}}
                            </div>
                        </div>
                    </template>
                </div>
            </div>
        </template>
    </div>
</template>
```

### 11. Data Grid with Alternating Row Colors

Apply styling based on position:

```html
<style>
    .data-row { padding: 8pt; border-bottom: 1pt solid #ddd; }
    .data-row:nth-child(even) { background-color: #f9f9f9; }
    .data-row:nth-child(odd) { background-color: #ffffff; }
</style>

<template data-bind="{{model.records}}">
    <div class="data-row">
        <div style="display: inline-block; width: 30%;">{{.id}}</div>
        <div style="display: inline-block; width: 40%;">{{.name}}</div>
        <div style="display: inline-block; width: 30%;">{{.status}}</div>
    </div>
</template>
```

### 12. Timeline with Event Grouping

Group events by date with nested iterations:

```html
<template data-bind="{{model.timeline}}">
    <div style="margin-bottom: 25pt; padding-left: 25pt; border-left: 3pt solid #336699;">
        <h2 style="color: #336699; margin: 0 0 10pt 0;">
            {{.date}}
        </h2>

        <template data-bind="{{.events}}">
            <div style="margin-bottom: 12pt; padding: 10pt; background-color: #f9f9f9;">
                <div style="font-weight: bold; color: #336699;">
                    {{.time}} - {{.title}}
                </div>
                <div style="margin-top: 5pt; color: #666;">
                    {{.description}}
                </div>
                <div style="margin-top: 5pt; font-size: 9pt; color: #999;">
                    Location: {{.location}} | Duration: {{.duration}}
                </div>
            </div>
        </template>
    </div>
</template>
```

### 13. Product Comparison Table

Generate comparison columns dynamically:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr style="background-color: #336699; color: white;">
            <th style="text-align: left; padding: 10pt;">Feature</th>
            <template data-bind="{{model.products}}">
                <th style="padding: 10pt;">{{.name}}</th>
            </template>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.features}}">
            <tr style="border-bottom: 1pt solid #ddd;">
                <td style="padding: 8pt; font-weight: bold;">{{.featureName}}</td>
                <template data-bind="{{model.products}}">
                    <td style="padding: 8pt; text-align: center;">
                        {{.features[..featureName] ? '✓' : '✗'}}
                    </td>
                </template>
            </tr>
        </template>
    </tbody>
</table>
```

### 14. File Directory Listing with Icons

Display files with type-specific icons:

```html
<h2>Directory Contents</h2>
<template data-bind="{{model.files}}">
    <div style="padding: 10pt; border-bottom: 1pt solid #eee;">
        <div style="display: inline-block; width: 50%; vertical-align: middle;">
            <img src="{{.iconPath}}" style="width: 16pt; height: 16pt; vertical-align: middle;"/>
            <span style="margin-left: 8pt;">{{.filename}}</span>
        </div>
        <div style="display: inline-block; width: 20%; vertical-align: middle; text-align: center;">
            {{.fileType}}
        </div>
        <div style="display: inline-block; width: 15%; vertical-align: middle; text-align: center;">
            {{.fileSize}}
        </div>
        <div style="display: inline-block; width: 15%; vertical-align: middle; text-align: right;">
            {{.modifiedDate}}
        </div>
    </div>
</template>
```

### 15. Performance-Optimized Large Dataset

Efficient rendering with style caching:

```html
<!-- Efficiently render 1000+ items -->
<style>
    .large-list-item {
        padding: 5pt;
        border-bottom: 1pt solid #eee;
        font-size: 10pt;
    }
</style>

<template data-bind="{{model.largeDataset}}"
          data-cache-styles="true"
          data-style-identifier="large-list-cache">
    <div class="large-list-item">
        <strong>{{.id}}</strong> - {{.name}} | {{.value}}
    </div>
</template>
```

### 16. Conditional Empty State

Show message when collection is empty:

```html
<div style="border: 1pt solid #ccc; padding: 15pt;">
    <h3>Products</h3>

    <template data-bind="{{model.products}}">
        <div class="product-item">
            {{.name}} - ${{.price}}
        </div>
    </template>

    <!-- Show this when no products -->
    <div hidden="{{count(model.products) > 0 ? 'hidden' : ''}}"
         style="color: #999; font-style: italic; text-align: center; padding: 20pt;">
        No products available at this time.
    </div>
</div>
```

---

## See Also

- [template element](/reference/htmltags/template.html) - The template HTML element
- [data-bind-start attribute](/reference/htmlattributes/data-bind-start.html) - Control iteration start index
- [data-bind-step attribute](/reference/htmlattributes/data-bind-step.html) - Control iteration step
- [data-bind-max attribute](/reference/htmlattributes/data-bind-max.html) - Limit iteration count
- [data-content attribute](/reference/htmlattributes/data-content.html) - Inline template content
- [Data Binding](/reference/binding/) - Complete data binding guide
- [Expressions](/reference/expressions/) - Expression syntax reference
- [ForEach Component](/reference/components/foreach.html) - Base component class

---
