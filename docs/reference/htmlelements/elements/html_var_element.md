---
layout: default
title: var
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;var&gt; : The Variable Storage Element
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

The `<var>` element is used to store values in the document data stack for later retrieval. It can store calculated values, intermediate results, or any data that needs to be accessed across different parts of the document, including within repeating templates.

## Summary

The `<var>` element serves a dual purpose in Scryber:
1. **Semantic Markup**: Represents a variable or mathematical expression (rendered in italic by default)
2. **Data Storage**: Stores values in the document parameters using `data-id` and `data-value` attributes

When a `<var>` element includes both `data-id` and `data-value` attributes, the value is stored in `Document.Params` and can be retrieved anywhere in the document using `{{params.data-id-value}}`.

---

## Usage

```html
<!-- Store a value in the document data stack -->
<var data-id="totalAmount" data-value="{{model.total}}"></var>

<!-- Later retrieve the value anywhere in the document -->
<p>Total: {{params.totalAmount}}</p>

<!-- Within repeating templates -->
<div data-bind="{{model.items}}">
    <p>Item total: {{.price}}</p>
    <p>Grand total: {{params.totalAmount}}</p>
</div>
```

---

## Supported Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-id` | string | The key name to store the value under in the document parameters |
| `data-value` | expression | The value to store (supports data binding expressions) |
| `id` | string | Unique identifier for the element |
| `class` | string | CSS class name(s) for styling |
| `style` | string | Inline CSS styles |
| `title` | string | Title text (tooltip) |
| `hidden` | string | If set to "hidden", the element is not visible |

---

## Data Storage and Retrieval

### Storing Values

Values are stored during the data binding phase using the `data-id` and `data-value` attributes:

```html
<var data-id="myVariable" data-value="{{someExpression}}"></var>
```

### Retrieving Values

Stored values can be retrieved anywhere in the document using the `params` object:

```html
{{params.myVariable}}
```

### Scope

Values stored in `<var>` elements are available:
- Throughout the entire document after they are set
- Inside nested data binding contexts (repeating templates)
- Across different sections and pages
- In conditional expressions and calculations

---

## Notes

- The `<var>` element is hidden automatically if it has no text content
- By default, any visible content is rendered in italic font style (semantic HTML behavior)
- Values are stored during the data binding phase, so they must be set before they are referenced
- The `data-value` attribute supports full expression syntax including calculations
- Stored values persist for the lifetime of the document generation
- Multiple `<var>` elements can be used to store different values with unique `data-id` keys

---

## Examples

### Example 1: Basic Value Storage

```html
<var data-id="username" data-value="{{user.name}}"></var>
<p>Welcome back, {{params.username}}!</p>
```

### Example 2: Storing Calculated Values

```html
<var data-id="totalPrice" data-value="{{product.price * product.quantity}}"></var>
<var data-id="taxAmount" data-value="{{params.totalPrice * 0.08}}"></var>
<var data-id="grandTotal" data-value="{{params.totalPrice + params.taxAmount}}"></var>

<p>Subtotal: ${{params.totalPrice}}</p>
<p>Tax (8%): ${{params.taxAmount}}</p>
<p>Grand Total: ${{params.grandTotal}}</p>
```

### Example 3: Using in Repeating Templates

```html
<!-- Store the customer name at document level -->
<var data-id="customerName" data-value="{{invoice.customerName}}"></var>

<!-- Use it within a repeating template -->
<table>
    <thead>
        <tr>
            <th>Item</th>
            <th>Price</th>
            <th>Customer</th>
        </tr>
    </thead>
    <tbody>
        <tr data-bind="{{invoice.items}}">
            <td>{{.itemName}}</td>
            <td>{{.price}}</td>
            <td>{{params.customerName}}</td>
        </tr>
    </tbody>
</table>
```

### Example 4: Counter for Item Numbers

```html
<var data-id="itemCount" data-value="0"></var>

<div data-bind="{{model.products}}">
    <var data-id="itemCount" data-value="{{params.itemCount + 1}}"></var>
    <p>Item #{{params.itemCount}}: {{.name}}</p>
</div>
```

### Example 5: Storing Date Information

```html
<var data-id="reportDate" data-value="{{DateTime.Now}}"></var>
<var data-id="reportYear" data-value="{{DateTime.Now.Year}}"></var>

<h1>Annual Report {{params.reportYear}}</h1>
<p>Generated on: {{params.reportDate}}</p>
```

### Example 6: Page Total Calculations

```html
<!-- Store page number or section -->
<var data-id="currentSection" data-value="Financial Summary"></var>

<!-- Calculate total -->
<var data-id="pageTotal" data-value="0"></var>
<div data-bind="{{model.transactions}}">
    <var data-id="pageTotal" data-value="{{params.pageTotal + .amount}}"></var>
    <p>{{.description}}: ${{.amount}}</p>
</div>

<p><strong>Section: {{params.currentSection}}</strong></p>
<p><strong>Total: ${{params.pageTotal}}</strong></p>
```

### Example 7: Conditional Logic with Stored Values

```html
<var data-id="hasErrors" data-value="{{model.errorCount > 0}}"></var>
<var data-id="statusColor" data-value="{{params.hasErrors ? 'red' : 'green'}}"></var>

<div style="color: {{params.statusColor}}">
    <p>Status: {{params.hasErrors ? 'Failed' : 'Success'}}</p>
</div>
```

### Example 8: Multi-page Document with Consistent Data

```html
<!-- Store company info at start -->
<var data-id="companyName" data-value="{{config.companyName}}"></var>
<var data-id="companyLogo" data-value="{{config.logoPath}}"></var>

<!-- Use on every page header -->
<header>
    <img src="{{params.companyLogo}}" alt="Logo" />
    <h1>{{params.companyName}}</h1>
</header>

<!-- Multiple pages can reference the same stored values -->
<section>
    <p>This report is provided by {{params.companyName}}</p>
</section>
```

### Example 9: Running Totals Across Sections

```html
<var data-id="runningTotal" data-value="0"></var>

<h2>Q1 Sales</h2>
<div data-bind="{{sales.q1}}">
    <var data-id="runningTotal" data-value="{{params.runningTotal + .amount}}"></var>
    <p>{{.month}}: ${{.amount}} (Running Total: ${{params.runningTotal}})</p>
</div>

<h2>Q2 Sales</h2>
<div data-bind="{{sales.q2}}">
    <var data-id="runningTotal" data-value="{{params.runningTotal + .amount}}"></var>
    <p>{{.month}}: ${{.amount}} (Running Total: ${{params.runningTotal}})</p>
</div>

<h2>Year-to-Date Total: ${{params.runningTotal}}</h2>
```

### Example 10: Storing Complex Objects

```html
<var data-id="selectedProduct" data-value="{{model.products[0]}}"></var>

<h2>Featured Product: {{params.selectedProduct.name}}</h2>
<p>Price: ${{params.selectedProduct.price}}</p>
<p>Description: {{params.selectedProduct.description}}</p>
```

### Example 11: Max/Min Value Tracking

```html
<var data-id="maxValue" data-value="0"></var>
<var data-id="minValue" data-value="999999"></var>

<div data-bind="{{model.measurements}}">
    <var data-id="maxValue" data-value="{{.value > params.maxValue ? .value : params.maxValue}}"></var>
    <var data-id="minValue" data-value="{{.value < params.minValue ? .value : params.minValue}}"></var>
    <p>Reading: {{.value}}</p>
</div>

<p>Maximum: {{params.maxValue}}</p>
<p>Minimum: {{params.minValue}}</p>
<p>Range: {{params.maxValue - params.minValue}}</p>
```

### Example 12: Invoice with Line Item Totals

```html
<var data-id="invoiceNumber" data-value="{{invoice.number}}"></var>
<var data-id="invoiceSubtotal" data-value="0"></var>

<h1>Invoice #{{params.invoiceNumber}}</h1>

<table>
    <thead>
        <tr>
            <th>Description</th>
            <th>Qty</th>
            <th>Price</th>
            <th>Total</th>
        </tr>
    </thead>
    <tbody>
        <tr data-bind="{{invoice.lineItems}}">
            <var data-id="lineTotal" data-value="{{.quantity * .unitPrice}}"></var>
            <var data-id="invoiceSubtotal" data-value="{{params.invoiceSubtotal + params.lineTotal}}"></var>
            <td>{{.description}}</td>
            <td>{{.quantity}}</td>
            <td>${{.unitPrice}}</td>
            <td>${{params.lineTotal}}</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="3">Subtotal:</td>
            <td>${{params.invoiceSubtotal}}</td>
        </tr>
        <tr>
            <td colspan="3">Tax (8%):</td>
            <td>${{params.invoiceSubtotal * 0.08}}</td>
        </tr>
        <tr>
            <td colspan="3"><strong>Total:</strong></td>
            <td><strong>${{params.invoiceSubtotal * 1.08}}</strong></td>
        </tr>
    </tfoot>
</table>
```

### Example 13: Nested Repeating Templates

```html
<var data-id="reportTitle" data-value="{{report.title}}"></var>

<div data-bind="{{report.departments}}">
    <var data-id="deptName" data-value="{{.name}}"></var>
    <h2>{{params.deptName}} - {{params.reportTitle}}</h2>

    <div data-bind="{{.employees}}">
        <p>Employee: {{.name}} (Department: {{params.deptName}})</p>
        <p>Report: {{params.reportTitle}}</p>
    </div>
</div>
```

### Example 14: Storing Configuration Values

```html
<!-- Store configuration at document start -->
<var data-id="currency" data-value="USD"></var>
<var data-id="currencySymbol" data-value="$"></var>
<var data-id="locale" data-value="en-US"></var>
<var data-id="dateFormat" data-value="MM/dd/yyyy"></var>

<!-- Use throughout document -->
<p>Amount: {{params.currencySymbol}}100.00 {{params.currency}}</p>
<p>Locale: {{params.locale}}</p>
```

### Example 15: Visible Content with Storage

```html
<!-- Store value AND display it -->
<p>
    Tax Rate: <var data-id="taxRate" data-value="{{config.taxRate}}">{{config.taxRate}}%</var>
</p>

<!-- Later use the stored value in calculations -->
<p>Tax Amount: ${{params.orderTotal * params.taxRate / 100}}</p>
```

### Example 16: Accumulating Statistics

```html
<var data-id="totalOrders" data-value="0"></var>
<var data-id="completedOrders" data-value="0"></var>
<var data-id="cancelledOrders" data-value="0"></var>

<div data-bind="{{orders}}">
    <var data-id="totalOrders" data-value="{{params.totalOrders + 1}}"></var>
    <var data-id="completedOrders" data-value="{{.status == 'completed' ? params.completedOrders + 1 : params.completedOrders}}"></var>
    <var data-id="cancelledOrders" data-value="{{.status == 'cancelled' ? params.cancelledOrders + 1 : params.cancelledOrders}}"></var>
    <p>Order {{.id}}: {{.status}}</p>
</div>

<h2>Order Statistics</h2>
<p>Total Orders: {{params.totalOrders}}</p>
<p>Completed: {{params.completedOrders}}</p>
<p>Cancelled: {{params.cancelledOrders}}</p>
<p>Success Rate: {{params.completedOrders / params.totalOrders * 100}}%</p>
```

### Example 17: Section Headers with Context

```html
<div data-bind="{{report.sections}}">
    <var data-id="sectionTitle" data-value="{{.title}}"></var>
    <var data-id="sectionIndex" data-value="{{$index + 1}}"></var>

    <h2>Section {{params.sectionIndex}}: {{params.sectionTitle}}</h2>

    <div data-bind="{{.items}}">
        <p>Item {{$index + 1}} in {{params.sectionTitle}}</p>
    </div>
</div>
```

### Example 18: Average Calculations

```html
<var data-id="sum" data-value="0"></var>
<var data-id="count" data-value="0"></var>

<div data-bind="{{model.scores}}">
    <var data-id="sum" data-value="{{params.sum + .score}}"></var>
    <var data-id="count" data-value="{{params.count + 1}}"></var>
    <p>Score: {{.score}}</p>
</div>

<p><strong>Average Score: {{params.sum / params.count}}</strong></p>
```

### Example 19: Conditional Display Based on Stored Values

```html
<var data-id="itemCount" data-value="{{model.items.length}}"></var>
<var data-id="hasItems" data-value="{{params.itemCount > 0}}"></var>

<div data-if="{{params.hasItems}}">
    <p>Showing {{params.itemCount}} items</p>
    <div data-bind="{{model.items}}">
        <p>{{.name}}</p>
    </div>
</div>

<div data-if="{{!params.hasItems}}">
    <p>No items to display</p>
</div>
```

### Example 20: Multi-level Totals

```html
<var data-id="grandTotal" data-value="0"></var>

<div data-bind="{{categories}}">
    <var data-id="categoryTotal" data-value="0"></var>
    <h2>{{.categoryName}}</h2>

    <div data-bind="{{.items}}">
        <var data-id="categoryTotal" data-value="{{params.categoryTotal + .amount}}"></var>
        <var data-id="grandTotal" data-value="{{params.grandTotal + .amount}}"></var>
        <p>{{.name}}: ${{.amount}}</p>
    </div>

    <p><strong>Category Total: ${{params.categoryTotal}}</strong></p>
</div>

<h2>Grand Total: ${{params.grandTotal}}</h2>
```

---

## See Also

- [Data Binding](/reference/databinding/)
- [Expressions](/reference/expressions/)
- [Repeating Templates](/reference/databinding/repeating)
- [Document Parameters](/reference/document/parameters)
- [span Element](/reference/htmlelements/html_span_element)
- [@data-bind attribute](/reference/htmlattributes/attr_data_bind)

---
