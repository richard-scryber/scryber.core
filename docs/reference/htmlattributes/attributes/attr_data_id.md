---
layout: default
title: data-id
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-id : The Reference Identifier Attribute

The `data-id` attribute provides a reference identifier for elements that may not have or need a standard `id` attribute. It's primarily used with variable (`<var>`) elements to create a secondary identification system for data referencing and cross-referencing within PDF documents.

## Usage

The `data-id` attribute is used to:
- Provide an alternative identification mechanism
- Create reference points for variables and data elements
- Enable cross-referencing without polluting the `id` namespace
- Support custom identification schemes
- Link related elements through custom IDs

```html
<!-- Variable with data-id for referencing -->
<var data-id="total-amount">$1,234.56</var>

<!-- Reference the variable elsewhere -->
<p>The total amount (<var data-id="total-amount-ref">$1,234.56</var>) is due on January 31.</p>
```

---

## Supported Elements

The `data-id` attribute is primarily supported by:

| Element | Description |
|---------|-------------|
| `<var>` | Variable element for representing values |

**Note**: While `data-id` can technically be added to any HTML element, it is specifically implemented and used with the `<var>` element in Scryber for variable identification and referencing purposes.

---

## Attribute Values

### Syntax

```html
<var data-id="identifier">Value</var>
```

### Value Type

| Type | Description | Example |
|------|-------------|---------|
| String | Any valid identifier string | `data-id="invoice-total"` |
| Alphanumeric | Letters, numbers, hyphens, underscores | `data-id="item_001"` |
| Namespaced | Dot or colon-separated identifiers | `data-id="order.total.amount"` |

### Naming Conventions

**Recommended patterns:**
- Use descriptive names: `data-id="quarterly-revenue"`
- Follow kebab-case: `data-id="total-amount"`
- Use namespacing: `data-id="invoice.line.total"`
- Be consistent across your document

**Avoid:**
- Spaces: `data-id="total amount"` (use hyphens or underscores)
- Special characters: `data-id="total@amount"`
- Starting with numbers: `data-id="123-total"`

---

## Binding Values

The `data-id` attribute supports both static and dynamic values:

### Static Data ID

```html
<var data-id="product-price">$99.99</var>
```

### Dynamic Data ID with Binding

```html
<!-- Model: { varId: "dynamic-total" } -->
<var data-id="{{model.varId}}">$500.00</var>
```

### Computed Data ID

```html
<!-- Model: { category: "electronics", itemId: 42 } -->
<var data-id="{{model.category}}-item-{{model.itemId}}">
    Product Name
</var>
```

---

## Notes

### Purpose and Use Cases

The `data-id` attribute serves several purposes:

1. **Secondary Identification**: Provides ID without interfering with standard `id` attribute
2. **Data Referencing**: Creates reference points for data values
3. **Custom Schemes**: Enables application-specific identification systems
4. **Variable Tracking**: Tracks variable values across document sections
5. **Cross-Referencing**: Links related data elements

### Difference from `id` Attribute

| Attribute | Purpose | Scope | Uniqueness |
|-----------|---------|-------|------------|
| `id` | Standard HTML identifier | Global, must be unique | Strictly enforced |
| `data-id` | Custom reference identifier | Application-specific | Not enforced by HTML |

**When to use `id`:**
- For CSS styling targets
- For JavaScript interaction (in web contexts)
- For anchor links and navigation
- When uniqueness is critical

**When to use `data-id`:**
- For application-level data referencing
- For secondary identification alongside `id`
- For variable value tracking
- When building custom referencing systems

### Variable Element Context

The `<var>` element with `data-id` is particularly useful for:

- **Financial Documents**: Tracking amounts, totals, subtotals
- **Reports**: Referencing data points and metrics
- **Invoices**: Identifying line items and calculations
- **Forms**: Tracking form field values
- **Dynamic Content**: Managing variable data in templates

### PDF Generation Behavior

In Scryber PDF generation:
- `data-id` values are **preserved** in the document structure
- No visual rendering difference (unlike `id`)
- Can be used for custom post-processing
- Helps maintain data relationships during generation

---

## Examples

### Example 1: Basic Variable Identification

```html
<p>The current price is <var data-id="product-price">$99.99</var>.</p>

<p>With tax, the total is <var data-id="price-with-tax">$107.99</var>.</p>
```

### Example 2: Invoice Line Items

```html
<table>
    <tr>
        <td>Premium Widget</td>
        <td><var data-id="item-001-quantity">10</var></td>
        <td><var data-id="item-001-price">$25.00</var></td>
        <td><var data-id="item-001-total">$250.00</var></td>
    </tr>
    <tr>
        <td>Standard Gadget</td>
        <td><var data-id="item-002-quantity">5</var></td>
        <td><var data-id="item-002-price">$50.00</var></td>
        <td><var data-id="item-002-total">$250.00</var></td>
    </tr>
</table>
```

### Example 3: Financial Report with Data References

```html
<h2>Quarterly Financial Summary</h2>

<table border="1" cellpadding="10" style="width: 100%;">
    <tr>
        <td>Revenue</td>
        <td><var data-id="q4.revenue">$145,000</var></td>
    </tr>
    <tr>
        <td>Expenses</td>
        <td><var data-id="q4.expenses">$95,000</var></td>
    </tr>
    <tr style="font-weight: bold;">
        <td>Net Profit</td>
        <td><var data-id="q4.profit">$50,000</var></td>
    </tr>
</table>

<p>
    The Q4 profit of <var data-id="q4.profit.reference">$50,000</var>
    represents a 16% increase over Q3.
</p>
```

### Example 4: Data-Bound Variable IDs

```html
<!-- Model: { items: [{id: "001", name: "Widget", price: 25}, {id: "002", name: "Gadget", price: 50}] } -->

<h2>Product List</h2>

<template data-bind="{{model.items}}">
    <div>
        <strong>{{.name}}</strong>:
        <var data-id="product-{{.id}}-price">${{.price}}</var>
    </div>
</template>
```

### Example 5: Form Output Values

```html
<form id="calculator">
    <input type="number" id="num1" name="num1" value="10" />
    <input type="number" id="num2" name="num2" value="20" />
</form>

<p>
    Sum: <var data-id="calc.sum">30</var><br/>
    Product: <var data-id="calc.product">200</var><br/>
    Average: <var data-id="calc.average">15</var>
</p>
```

### Example 6: Multi-Level Data Identification

```html
<h1>Annual Report 2024</h1>

<h2>Q1 Results</h2>
<p>Revenue: <var data-id="report.2024.q1.revenue">$125,000</var></p>
<p>Expenses: <var data-id="report.2024.q1.expenses">$85,000</var></p>
<p>Profit: <var data-id="report.2024.q1.profit">$40,000</var></p>

<h2>Q2 Results</h2>
<p>Revenue: <var data-id="report.2024.q2.revenue">$145,000</var></p>
<p>Expenses: <var data-id="report.2024.q2.expenses">$95,000</var></p>
<p>Profit: <var data-id="report.2024.q2.profit">$50,000</var></p>

<h2>Annual Summary</h2>
<p>Total Revenue: <var data-id="report.2024.total.revenue">$563,000</var></p>
<p>Total Profit: <var data-id="report.2024.total.profit">$193,000</var></p>
```

### Example 7: Comparison Data Points

```html
<h2>Year-over-Year Comparison</h2>

<table border="1" cellpadding="10">
    <tr>
        <th>Metric</th>
        <th>2023</th>
        <th>2024</th>
        <th>Change</th>
    </tr>
    <tr>
        <td>Revenue</td>
        <td><var data-id="compare.2023.revenue">$500,000</var></td>
        <td><var data-id="compare.2024.revenue">$563,000</var></td>
        <td><var data-id="compare.revenue.change">+12.6%</var></td>
    </tr>
    <tr>
        <td>Profit</td>
        <td><var data-id="compare.2023.profit">$165,000</var></td>
        <td><var data-id="compare.2024.profit">$193,000</var></td>
        <td><var data-id="compare.profit.change">+17.0%</var></td>
    </tr>
</table>
```

### Example 8: Scientific Data Values

```html
<h2>Experiment Results</h2>

<p>
    Temperature: <var data-id="exp.temp">25.3°C</var><br/>
    Pressure: <var data-id="exp.pressure">101.3 kPa</var><br/>
    Humidity: <var data-id="exp.humidity">65%</var><br/>
    pH Level: <var data-id="exp.ph">7.2</var>
</p>

<p>
    The measured temperature (<var data-id="exp.temp.ref">25.3°C</var>)
    was within the expected range.
</p>
```

### Example 9: Product Specifications

```html
<h2>Product Specifications: Model XYZ-2000</h2>

<table border="1" cellpadding="8" style="width: 100%;">
    <tr>
        <td>Dimensions (L×W×H)</td>
        <td>
            <var data-id="spec.length">50</var> ×
            <var data-id="spec.width">30</var> ×
            <var data-id="spec.height">20</var> cm
        </td>
    </tr>
    <tr>
        <td>Weight</td>
        <td><var data-id="spec.weight">2.5 kg</var></td>
    </tr>
    <tr>
        <td>Power Consumption</td>
        <td><var data-id="spec.power">120W</var></td>
    </tr>
    <tr>
        <td>Operating Temperature</td>
        <td>
            <var data-id="spec.temp.min">-10°C</var> to
            <var data-id="spec.temp.max">50°C</var>
        </td>
    </tr>
</table>
```

### Example 10: Survey Results

```html
<h2>Customer Satisfaction Survey Results</h2>

<p>Response Rate: <var data-id="survey.response-rate">87%</var></p>

<h3>Ratings (out of 5)</h3>
<ul>
    <li>Product Quality: <var data-id="survey.rating.quality">4.5</var></li>
    <li>Customer Service: <var data-id="survey.rating.service">4.7</var></li>
    <li>Value for Money: <var data-id="survey.rating.value">4.2</var></li>
    <li>Overall Satisfaction: <var data-id="survey.rating.overall">4.5</var></li>
</ul>

<p>
    Overall satisfaction score of <var data-id="survey.rating.overall.ref">4.5</var>
    represents a 0.3-point improvement over last quarter.
</p>
```

### Example 11: Budget Breakdown

```html
<h2>Department Budget Allocation</h2>

<table border="1" cellpadding="10" style="width: 100%;">
    <tr>
        <th>Department</th>
        <th>Budget</th>
        <th>% of Total</th>
    </tr>
    <tr>
        <td>Engineering</td>
        <td><var data-id="budget.engineering.amount">$500,000</var></td>
        <td><var data-id="budget.engineering.percent">40%</var></td>
    </tr>
    <tr>
        <td>Marketing</td>
        <td><var data-id="budget.marketing.amount">$300,000</var></td>
        <td><var data-id="budget.marketing.percent">24%</var></td>
    </tr>
    <tr>
        <td>Sales</td>
        <td><var data-id="budget.sales.amount">$250,000</var></td>
        <td><var data-id="budget.sales.percent">20%</var></td>
    </tr>
    <tr>
        <td>Operations</td>
        <td><var data-id="budget.operations.amount">$200,000</var></td>
        <td><var data-id="budget.operations.percent">16%</var></td>
    </tr>
    <tr style="font-weight: bold;">
        <td>Total</td>
        <td><var data-id="budget.total.amount">$1,250,000</var></td>
        <td><var data-id="budget.total.percent">100%</var></td>
    </tr>
</table>
```

### Example 12: Performance Metrics Dashboard

```html
<h2>System Performance Metrics</h2>

<div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20pt;">
    <div style="padding: 15pt; background-color: #f0f0f0;">
        <h3>Response Time</h3>
        <p style="font-size: 24pt; font-weight: bold; color: #336699;">
            <var data-id="metrics.response-time">145ms</var>
        </p>
    </div>

    <div style="padding: 15pt; background-color: #f0f0f0;">
        <h3>Throughput</h3>
        <p style="font-size: 24pt; font-weight: bold; color: #336699;">
            <var data-id="metrics.throughput">1,250 req/s</var>
        </p>
    </div>

    <div style="padding: 15pt; background-color: #f0f0f0;">
        <h3>Error Rate</h3>
        <p style="font-size: 24pt; font-weight: bold; color: #336699;">
            <var data-id="metrics.error-rate">0.05%</var>
        </p>
    </div>

    <div style="padding: 15pt; background-color: #f0f0f0;">
        <h3>Uptime</h3>
        <p style="font-size: 24pt; font-weight: bold; color: #336699;">
            <var data-id="metrics.uptime">99.99%</var>
        </p>
    </div>
</div>
```

### Example 13: Medical Test Results

```html
<h2>Laboratory Test Results</h2>
<p><strong>Patient ID:</strong> <var data-id="patient.id">P-12345</var></p>
<p><strong>Test Date:</strong> <var data-id="test.date">2024-01-15</var></p>

<table border="1" cellpadding="10" style="width: 100%;">
    <tr>
        <th>Test</th>
        <th>Result</th>
        <th>Normal Range</th>
    </tr>
    <tr>
        <td>Glucose</td>
        <td><var data-id="test.glucose">95 mg/dL</var></td>
        <td>70-100 mg/dL</td>
    </tr>
    <tr>
        <td>Cholesterol</td>
        <td><var data-id="test.cholesterol">185 mg/dL</var></td>
        <td>&lt;200 mg/dL</td>
    </tr>
    <tr>
        <td>Blood Pressure</td>
        <td>
            <var data-id="test.bp.systolic">120</var>/
            <var data-id="test.bp.diastolic">80</var> mmHg
        </td>
        <td>120/80 mmHg</td>
    </tr>
</table>
```

### Example 14: Dynamic Variable References

```html
<!-- Model: {
    vars: [
        {id: "revenue", label: "Revenue", value: 145000},
        {id: "expenses", label: "Expenses", value: 95000},
        {id: "profit", label: "Net Profit", value: 50000}
    ]
} -->

<h2>Financial Metrics</h2>

<template data-bind="{{model.vars}}">
    <div style="margin-bottom: 10pt;">
        <strong>{{.label}}:</strong>
        <var data-id="metric.{{.id}}">${{.value}}</var>
    </div>
</template>
```

### Example 15: Complex Hierarchical Data

```html
<h1>Company Performance Report</h1>

<!-- North America Region -->
<h2>North America</h2>
<h3>Q4 2024</h3>
<p>
    Sales: <var data-id="region.na.q4.sales">$2,500,000</var><br/>
    Growth: <var data-id="region.na.q4.growth">+15%</var>
</p>

<!-- Europe Region -->
<h2>Europe</h2>
<h3>Q4 2024</h3>
<p>
    Sales: <var data-id="region.eu.q4.sales">$1,800,000</var><br/>
    Growth: <var data-id="region.eu.q4.growth">+12%</var>
</p>

<!-- Asia Pacific Region -->
<h2>Asia Pacific</h2>
<h3>Q4 2024</h3>
<p>
    Sales: <var data-id="region.ap.q4.sales">$1,200,000</var><br/>
    Growth: <var data-id="region.ap.q4.growth">+22%</var>
</p>

<!-- Global Summary -->
<h2>Global Total</h2>
<p>
    Total Sales: <var data-id="global.q4.sales">$5,500,000</var><br/>
    Average Growth: <var data-id="global.q4.growth">+16%</var>
</p>
```

---

## See Also

- [var element](/reference/htmltags/var.html) - Variable element
- [id attribute](/reference/htmlattributes/id.html) - Standard identifier attribute
- [data-* attributes](/reference/htmlattributes/data-attributes.html) - Custom data attributes
- [Data Binding](/reference/binding/) - Dynamic data binding
- [Variables](/reference/variables/) - Variable handling in Scryber
- [Cross-References](/reference/cross-references/) - Document cross-referencing

---
