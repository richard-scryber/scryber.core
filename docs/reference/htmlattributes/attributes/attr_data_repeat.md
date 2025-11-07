---
layout: default
title: data-repeat
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-repeat : The Table Header Repetition Attribute

The `data-repeat` attribute controls whether table header rows (`<thead>`) automatically repeat at the top of each page when a table spans multiple pages in a PDF document. This is a critical feature for multi-page tables, ensuring that column headers remain visible and providing context for data on every page.

---

## Summary

The `data-repeat` attribute enables or disables header repetition behavior for table headers, providing:
- **Automatic header repetition** across multi-page tables (enabled by default)
- **Improved readability** for long tables with consistent column identification
- **Professional formatting** for reports and data-heavy documents
- **Flexible control** over header display behavior

This attribute is essential for:
- Multi-page reports with large datasets
- Financial statements and accounting reports
- Inventory and product listings
- Transaction logs and audit trails
- Any table that may span multiple pages

**Default Behavior**: Table headers (`<thead>`) repeat on each page by default. Use `data-repeat="false"` to disable this behavior.

---

## Usage

The `data-repeat` attribute is applied to `<thead>` elements within tables:

```html
<table style="width: 100%;">
    <thead data-repeat="true">
        <tr>
            <th>Column 1</th>
            <th>Column 2</th>
        </tr>
    </thead>
    <tbody>
        <!-- Many rows of data -->
    </tbody>
</table>
```

### Basic Syntax

```html
<!-- Enable header repetition (default behavior) -->
<thead data-repeat="true">
    <tr><th>Header</th></tr>
</thead>

<!-- Disable header repetition -->
<thead data-repeat="false">
    <tr><th>Header</th></tr>
</thead>

<!-- Omit attribute (defaults to true) -->
<thead>
    <tr><th>Header</th></tr>
</thead>
```

---

## Supported Elements

The `data-repeat` attribute is **only** supported on the following element:

- `<thead>` - The table header section element

**Note**: This attribute has no effect on `<tbody>`, `<tfoot>`, or other table elements. It is specifically designed for header repetition control.

---

## Binding Values

### Attribute Values

| Value | Description |
|-------|-------------|
| `true` | Headers repeat on each page (default behavior) |
| `false` | Headers appear only once at the start of the table |

### Type

**Type**: `boolean`
**Default**: `true`

### Expression Support

The attribute accepts literal boolean values or binding expressions:

```html
<!-- Literal value -->
<thead data-repeat="true">

<!-- Bound value -->
<thead data-repeat="{{model.repeatHeaders}}">

<!-- Conditional based on table size -->
<thead data-repeat="{{count(model.items) > 20}}">
```

---

## Notes

### Default Behavior

By default, table headers (`<thead>`) automatically repeat at the top of each page when a table spans multiple pages. This provides:
- Consistent column identification across all pages
- Better readability for long tables
- Professional document formatting
- Improved usability for printed documents

### When to Disable Repetition

Consider setting `data-repeat="false"` when:
- Table is guaranteed to fit on a single page
- Headers are very tall and consume significant page space
- Custom page breaking is implemented
- Table structure changes mid-document
- Headers contain images or complex layouts that impact performance

### Performance Considerations

Header repetition has minimal performance impact:
- Headers are rendered once and referenced on subsequent pages
- Styling is cached for repeated headers
- Memory usage is negligible even for complex headers

For very complex headers (images, nested tables), consider:
- Simplifying header design
- Using text-only headers for large datasets
- Combining with `data-cache-styles` on data rows

### Multi-Row Headers

When `<thead>` contains multiple rows, all rows repeat together:

```html
<thead data-repeat="true">
    <tr>
        <th colspan="3">Main Header</th>
    </tr>
    <tr>
        <th>Column 1</th>
        <th>Column 2</th>
        <th>Column 3</th>
    </tr>
</thead>
```

Both header rows appear on each page.

### Page Breaking Behavior

When a table spans pages:
1. Current page fills with data rows
2. New page begins
3. If `data-repeat="true"`, header rows are rendered at top of new page
4. Data rows continue below the header
5. Process repeats for all pages

### Combining with Other Table Features

The `data-repeat` attribute works seamlessly with:
- **Column widths**: Headers maintain consistent widths across pages
- **Styling**: Header styles are preserved on all pages
- **Borders**: Border rendering remains consistent
- **Background colors**: Background colors repeat correctly
- **Data binding**: Works with both static and dynamic tables

### Accessibility and Usability

Repeating headers improve document usability by:
- Eliminating need to reference earlier pages for column meaning
- Providing context at a glance
- Reducing errors in data interpretation
- Improving scanning and searching in printed documents

---

## Examples

### 1. Basic Repeating Header

Standard table with header repetition (default):

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead>
        <tr>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #f0f0f0;">
                Name
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #f0f0f0;">
                Email
            </th>
            <th style="border: 1pt solid black; padding: 8pt; background-color: #f0f0f0;">
                Phone
            </th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.contacts}}">
            <tr>
                <td style="border: 1pt solid black; padding: 8pt;">{{.name}}</td>
                <td style="border: 1pt solid black; padding: 8pt;">{{.email}}</td>
                <td style="border: 1pt solid black; padding: 8pt;">{{.phone}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 2. Disable Header Repetition

Single-page table without repetition:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="false">
        <tr>
            <th style="border: 1pt solid black; padding: 8pt;">
                Item
            </th>
            <th style="border: 1pt solid black; padding: 8pt;">
                Description
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">A</td>
            <td style="border: 1pt solid black; padding: 8pt;">First item</td>
        </tr>
        <tr>
            <td style="border: 1pt solid black; padding: 8pt;">B</td>
            <td style="border: 1pt solid black; padding: 8pt;">Second item</td>
        </tr>
    </tbody>
</table>
```

### 3. Styled Repeating Header

Professional styled header that repeats:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="true">
        <tr style="background-color: #336699; color: white;">
            <th style="padding: 10pt; text-align: left;">Product Code</th>
            <th style="padding: 10pt; text-align: left;">Product Name</th>
            <th style="padding: 10pt; text-align: right;">Quantity</th>
            <th style="padding: 10pt; text-align: right;">Unit Price</th>
            <th style="padding: 10pt; text-align: right;">Total</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.products}}">
            <tr>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">{{.code}}</td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">{{.name}}</td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right;">
                    {{.quantity}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right;">
                    ${{.unitPrice}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right;">
                    ${{.quantity * .unitPrice}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### 4. Multi-Row Header

Complex header with two rows, both repeat:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="true">
        <tr style="background-color: #2c3e50; color: white;">
            <th colspan="5" style="padding: 12pt; text-align: center; font-size: 14pt;">
                Quarterly Sales Report - Q1 2024
            </th>
        </tr>
        <tr style="background-color: #34495e; color: white;">
            <th style="padding: 8pt;">Region</th>
            <th style="padding: 8pt; text-align: right;">Jan</th>
            <th style="padding: 8pt; text-align: right;">Feb</th>
            <th style="padding: 8pt; text-align: right;">Mar</th>
            <th style="padding: 8pt; text-align: right;">Total</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.salesData}}">
            <tr>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">{{.region}}</td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right;">
                    ${{.january}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right;">
                    ${{.february}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right;">
                    ${{.march}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right; font-weight: bold;">
                    ${{.january + .february + .march}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### 5. Conditional Repetition Based on Data Size

Enable repetition only for large datasets:

```html
<!-- Model: { items: [...], repeatHeaders: items.length > 25 } -->
<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="{{model.repeatHeaders}}">
        <tr style="background-color: #f0f0f0;">
            <th style="padding: 8pt; border: 1pt solid black;">ID</th>
            <th style="padding: 8pt; border: 1pt solid black;">Name</th>
            <th style="padding: 8pt; border: 1pt solid black;">Status</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.items}}">
            <tr>
                <td style="padding: 8pt; border: 1pt solid #ddd;">{{.id}}</td>
                <td style="padding: 8pt; border: 1pt solid #ddd;">{{.name}}</td>
                <td style="padding: 8pt; border: 1pt solid #ddd;">{{.status}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 6. Invoice Line Items

Invoice table with repeating header:

```html
<h1>Invoice #{{model.invoiceNumber}}</h1>
<table style="width: 100%; border-collapse: collapse; margin-top: 20pt;">
    <thead data-repeat="true">
        <tr style="background-color: #e0e0e0; font-weight: bold;">
            <th style="padding: 10pt; border: 1pt solid black; text-align: left;">Description</th>
            <th style="padding: 10pt; border: 1pt solid black; text-align: right;">Qty</th>
            <th style="padding: 10pt; border: 1pt solid black; text-align: right;">Unit Price</th>
            <th style="padding: 10pt; border: 1pt solid black; text-align: right;">Total</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.lineItems}}">
            <tr>
                <td style="padding: 8pt; border: 1pt solid #ddd;">{{.description}}</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: right;">{{.quantity}}</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: right;">${{.unitPrice}}</td>
                <td style="padding: 8pt; border: 1pt solid #ddd; text-align: right; font-weight: bold;">
                    ${{.lineTotal}}
                </td>
            </tr>
        </template>
    </tbody>
    <tfoot>
        <tr style="font-weight: bold; background-color: #f9f9f9;">
            <td colspan="3" style="padding: 10pt; border: 1pt solid black; text-align: right;">
                Subtotal:
            </td>
            <td style="padding: 10pt; border: 1pt solid black; text-align: right;">
                ${{model.subtotal}}
            </td>
        </tr>
        <tr style="font-weight: bold; background-color: #f9f9f9;">
            <td colspan="3" style="padding: 10pt; border: 1pt solid black; text-align: right;">
                Tax ({{model.taxRate}}%):
            </td>
            <td style="padding: 10pt; border: 1pt solid black; text-align: right;">
                ${{model.taxAmount}}
            </td>
        </tr>
        <tr style="font-weight: bold; font-size: 12pt; background-color: #e0e0e0;">
            <td colspan="3" style="padding: 10pt; border: 1pt solid black; text-align: right;">
                Total:
            </td>
            <td style="padding: 10pt; border: 1pt solid black; text-align: right;">
                ${{model.total}}
            </td>
        </tr>
    </tfoot>
</table>
```

### 7. Transaction Log

Long transaction list with compact repeating header:

```html
<h1>Transaction Log</h1>
<p>Account: {{model.accountNumber}} | Date Range: {{model.startDate}} - {{model.endDate}}</p>

<table style="width: 100%; border-collapse: collapse; font-size: 9pt;">
    <thead data-repeat="true">
        <tr style="background-color: #336699; color: white;">
            <th style="padding: 6pt; border: 1pt solid black;">Date</th>
            <th style="padding: 6pt; border: 1pt solid black;">Transaction ID</th>
            <th style="padding: 6pt; border: 1pt solid black;">Description</th>
            <th style="padding: 6pt; border: 1pt solid black; text-align: right;">Debit</th>
            <th style="padding: 6pt; border: 1pt solid black; text-align: right;">Credit</th>
            <th style="padding: 6pt; border: 1pt solid black; text-align: right;">Balance</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.transactions}}">
            <tr>
                <td style="padding: 5pt; border-bottom: 1pt solid #ddd;">{{.date}}</td>
                <td style="padding: 5pt; border-bottom: 1pt solid #ddd;">{{.transactionId}}</td>
                <td style="padding: 5pt; border-bottom: 1pt solid #ddd;">{{.description}}</td>
                <td style="padding: 5pt; border-bottom: 1pt solid #ddd; text-align: right; color: red;">
                    {{.debit > 0 ? '$' + .debit : ''}}
                </td>
                <td style="padding: 5pt; border-bottom: 1pt solid #ddd; text-align: right; color: green;">
                    {{.credit > 0 ? '$' + .credit : ''}}
                </td>
                <td style="padding: 5pt; border-bottom: 1pt solid #ddd; text-align: right; font-weight: bold;">
                    ${{.balance}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### 8. Inventory Report with Grouped Headers

Grouped column headers that repeat:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="true">
        <tr style="background-color: #2c3e50; color: white;">
            <th rowspan="2" style="padding: 10pt; border: 1pt solid black; vertical-align: middle;">
                SKU
            </th>
            <th rowspan="2" style="padding: 10pt; border: 1pt solid black; vertical-align: middle;">
                Product
            </th>
            <th colspan="3" style="padding: 10pt; border: 1pt solid black; text-align: center;">
                Inventory
            </th>
        </tr>
        <tr style="background-color: #34495e; color: white;">
            <th style="padding: 8pt; border: 1pt solid black;">On Hand</th>
            <th style="padding: 8pt; border: 1pt solid black;">Committed</th>
            <th style="padding: 8pt; border: 1pt solid black;">Available</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.inventoryItems}}">
            <tr>
                <td style="padding: 6pt; border-bottom: 1pt solid #ddd;">{{.sku}}</td>
                <td style="padding: 6pt; border-bottom: 1pt solid #ddd;">{{.productName}}</td>
                <td style="padding: 6pt; border-bottom: 1pt solid #ddd; text-align: right;">
                    {{.onHand}}
                </td>
                <td style="padding: 6pt; border-bottom: 1pt solid #ddd; text-align: right;">
                    {{.committed}}
                </td>
                <td style="padding: 6pt; border-bottom: 1pt solid #ddd; text-align: right; font-weight: bold;">
                    {{.available}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### 9. Employee Directory

Multi-page directory with repeating styled header:

```html
<style>
    .directory-header {
        background: linear-gradient(to bottom, #336699, #2c5577);
        color: white;
        font-weight: bold;
    }
</style>

<h1>Employee Directory</h1>

<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="true" class="directory-header">
        <tr>
            <th style="padding: 10pt; text-align: left;">Name</th>
            <th style="padding: 10pt; text-align: left;">Department</th>
            <th style="padding: 10pt; text-align: left;">Title</th>
            <th style="padding: 10pt; text-align: left;">Email</th>
            <th style="padding: 10pt; text-align: left;">Phone</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.employees}}">
            <tr style="border-bottom: 1pt solid #ddd;">
                <td style="padding: 8pt;">{{.firstName}} {{.lastName}}</td>
                <td style="padding: 8pt;">{{.department}}</td>
                <td style="padding: 8pt;">{{.title}}</td>
                <td style="padding: 8pt;">{{.email}}</td>
                <td style="padding: 8pt;">{{.phone}}</td>
            </tr>
        </template>
    </tbody>
</table>
```

### 10. Product Catalog

Catalog with image-based header (repetition enabled):

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="true">
        <tr style="background-color: #f0f0f0;">
            <th style="padding: 10pt; width: 15%;">
                <img src="logo-small.png" style="height: 20pt;"/>
            </th>
            <th style="padding: 10pt; text-align: left;">Product Name</th>
            <th style="padding: 10pt; text-align: left;">Category</th>
            <th style="padding: 10pt; text-align: right;">Price</th>
            <th style="padding: 10pt; text-align: center;">Availability</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.products}}">
            <tr>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center;">
                    <img src="{{.thumbnailUrl}}" style="width: 40pt; height: 40pt;"/>
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">{{.productName}}</td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">{{.category}}</td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right; font-weight: bold;">
                    ${{.price}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center;">
                    {{.inStock ? 'In Stock' : 'Out of Stock'}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### 11. Audit Trail Report

Compliance report with repeating header:

```html
<h1>System Audit Trail</h1>
<p>Generated: {{model.reportDate}} | User: {{model.userName}}</p>

<table style="width: 100%; border-collapse: collapse; font-size: 8pt;">
    <thead data-repeat="true">
        <tr style="background-color: #dc3545; color: white;">
            <th style="padding: 6pt; border: 1pt solid black;">Timestamp</th>
            <th style="padding: 6pt; border: 1pt solid black;">User</th>
            <th style="padding: 6pt; border: 1pt solid black;">Action</th>
            <th style="padding: 6pt; border: 1pt solid black;">Resource</th>
            <th style="padding: 6pt; border: 1pt solid black;">Result</th>
            <th style="padding: 6pt; border: 1pt solid black;">IP Address</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.auditEntries}}">
            <tr>
                <td style="padding: 4pt; border-bottom: 1pt solid #ddd;">{{.timestamp}}</td>
                <td style="padding: 4pt; border-bottom: 1pt solid #ddd;">{{.user}}</td>
                <td style="padding: 4pt; border-bottom: 1pt solid #ddd;">{{.action}}</td>
                <td style="padding: 4pt; border-bottom: 1pt solid #ddd;">{{.resource}}</td>
                <td style="padding: 4pt; border-bottom: 1pt solid #ddd;
                           color: {{.result == 'Success' ? 'green' : 'red'}};">
                    {{.result}}
                </td>
                <td style="padding: 4pt; border-bottom: 1pt solid #ddd; font-family: monospace;">
                    {{.ipAddress}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### 12. Financial Statement

Financial data with multiple header levels:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="true">
        <tr style="background-color: #2c3e50; color: white;">
            <th colspan="4" style="padding: 15pt; text-align: center; font-size: 14pt;">
                Income Statement - Year Ended December 31, 2023
            </th>
        </tr>
        <tr style="background-color: #34495e; color: white;">
            <th style="padding: 8pt; width: 10%; text-align: center;">Account</th>
            <th style="padding: 8pt; width: 50%; text-align: left;">Description</th>
            <th style="padding: 8pt; width: 20%; text-align: right;">Current Year</th>
            <th style="padding: 8pt; width: 20%; text-align: right;">Prior Year</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.accounts}}">
            <tr>
                <td style="padding: 6pt; border-bottom: 1pt solid #ddd; text-align: center;">
                    {{.accountNumber}}
                </td>
                <td style="padding: 6pt; border-bottom: 1pt solid #ddd;">
                    {{.accountName}}
                </td>
                <td style="padding: 6pt; border-bottom: 1pt solid #ddd; text-align: right;">
                    ${{.currentYear}}
                </td>
                <td style="padding: 6pt; border-bottom: 1pt solid #ddd; text-align: right;">
                    ${{.priorYear}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### 13. Order Fulfillment Report

Warehouse picking list with repeating header:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="true">
        <tr style="background-color: #28a745; color: white;">
            <th style="padding: 10pt; text-align: center;">Pick</th>
            <th style="padding: 10pt; text-align: left;">Item Code</th>
            <th style="padding: 10pt; text-align: left;">Description</th>
            <th style="padding: 10pt; text-align: left;">Location</th>
            <th style="padding: 10pt; text-align: center;">Qty</th>
            <th style="padding: 10pt; text-align: center;">Picked</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.orderItems}}">
            <tr>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center;">
                    ☐
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; font-family: monospace;">
                    {{.itemCode}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">
                    {{.description}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; font-weight: bold;">
                    {{.binLocation}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center; font-size: 12pt;">
                    {{.quantity}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">
                    __________
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### 14. Event Schedule

Multi-day event schedule with repeating date headers:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="true">
        <tr style="background-color: #6c757d; color: white;">
            <th style="padding: 10pt; width: 15%;">Time</th>
            <th style="padding: 10pt; width: 35%;">Session</th>
            <th style="padding: 10pt; width: 25%;">Speaker</th>
            <th style="padding: 10pt; width: 25%;">Location</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.sessions}}">
            <tr>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; font-weight: bold;">
                    {{.startTime}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">
                    {{.title}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">
                    {{.speaker}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">
                    {{.location}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

### 15. Comparison Report

Side-by-side comparison with repeating headers:

```html
<table style="width: 100%; border-collapse: collapse;">
    <thead data-repeat="true">
        <tr style="background-color: #17a2b8; color: white;">
            <th style="padding: 10pt; text-align: left;">Feature</th>
            <th style="padding: 10pt; text-align: center;">Product A</th>
            <th style="padding: 10pt; text-align: center;">Product B</th>
            <th style="padding: 10pt; text-align: center;">Product C</th>
        </tr>
    </thead>
    <tbody>
        <template data-bind="{{model.features}}">
            <tr>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; font-weight: bold;">
                    {{.featureName}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center;">
                    {{.productA ? '✓' : '✗'}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center;">
                    {{.productB ? '✓' : '✗'}}
                </td>
                <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: center;">
                    {{.productC ? '✓' : '✗'}}
                </td>
            </tr>
        </template>
    </tbody>
</table>
```

---

## See Also

- [table element](/reference/htmltags/table.html) - The table HTML element
- [thead element](/reference/htmltags/thead.html) - Table header section
- [tbody element](/reference/htmltags/tbody.html) - Table body section
- [tfoot element](/reference/htmltags/tfoot.html) - Table footer section
- [Data Binding](/reference/binding/) - Complete data binding guide
- [template element](/reference/htmltags/template.html) - Template for repeating content

---
