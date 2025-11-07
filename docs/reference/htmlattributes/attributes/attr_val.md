---
layout: default
title: val
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @val : The Output Value Attribute

The `val` attribute specifies the value of an HTML `<output>` element. It provides an alternative to placing the output value as text content between the opening and closing tags.

## Usage

The `val` attribute is used to:
- Set the output value of an `<output>` element
- Provide structured value data separate from display content
- Enable data binding for output values
- Support programmatic value assignment

```html
<!-- Value as attribute -->
<output for="quantity price" val="$149.95" />

<!-- Value as content (alternative) -->
<output for="quantity price">$149.95</output>
```

---

## Supported Elements

The `val` attribute is supported by the following element:

| Element | Description |
|---------|-------------|
| `<output>` | Represents the result of a calculation or user action |

---

## Attribute Values

### Syntax

```html
<output val="value">Optional display content</output>
```

### Value Type

| Type | Description | Example |
|------|-------------|---------|
| String | Any text value | `val="Result"` |
| Number | Numeric value (as string) | `val="42.5"` |
| Formatted | Formatted display value | `val="$1,234.56"` |
| Expression | Data-bound expression | `val="{{model.total}}"` |

---

## Binding Values

The `val` attribute fully supports data binding:

### Static Value

```html
<output for="a b" val="30" />
```

### Dynamic Value with Data Binding

```html
<!-- Model: { totalAmount: 542.75 } -->
<output for="subtotal tax" val="{{model.totalAmount}}" />
```

### Calculated Value

```html
<!-- Model: { quantity: 5, price: 29.99 } -->
<output for="quantity price" val="{{model.quantity * model.price}}" />
```

### Formatted Value

```html
<!-- Model: { total: 1234.56 } -->
<output for="items" val="${{model.total.toFixed(2)}}" />
```

---

## Notes

### Value vs Content

You can specify the output value in two ways:

**Using `val` attribute**:
```html
<output val="Total: $100.00" />
```

**Using element content**:
```html
<output>Total: $100.00</output>
```

**Both together** (attribute takes precedence):
```html
<!-- The val attribute value is used, content is ignored -->
<output val="$100.00">This is ignored</output>
```

### When to Use val Attribute

Use the `val` attribute when:
- You need a self-closing output element
- The value comes from data binding
- You want to separate value from display markup
- You need programmatic value access

Use element content when:
- You need rich HTML content inside the output
- The display includes formatting or nested elements
- You prefer explicit visible content

### PDF Generation Behavior

In Scryber PDF generation:
- The `val` attribute renders as text in the PDF
- Output elements are non-interactive (static text)
- Both `val` attribute and element content render identically
- If both are present, `val` takes precedence

### Empty Values

An empty or missing `val` attribute results in an empty output:

```html
<!-- Empty output -->
<output val="" />

<!-- Also empty -->
<output></output>
```

---

## Examples

### Example 1: Basic Output Value

```html
<form id="calculator">
    <input type="number" id="num1" name="num1" value="10" />
    <input type="number" id="num2" name="num2" value="20" />
</form>

<output for="num1 num2" val="30" />
```

### Example 2: Formatted Currency Output

```html
<form id="orderForm">
    <input type="number" id="quantity" value="5" />
    <input type="number" id="price" value="29.99" />
</form>

<output for="quantity price" name="total" val="$149.95" />
```

### Example 3: Data-Bound Output

```html
<!-- Model: { subtotal: 500, taxRate: 0.08, total: 540 } -->

<form id="invoice">
    <input type="number" name="subtotal" value="{{model.subtotal}}" />
</form>

<div>
    <output for="subtotal" val="Subtotal: ${{model.subtotal}}" /><br/>
    <output for="subtotal" val="Tax: ${{model.subtotal * model.taxRate}}" /><br/>
    <output for="subtotal" val="Total: ${{model.total}}" />
</div>
```

### Example 4: Calculation Results

```html
<h2>Loan Calculator Results</h2>

<form id="loanCalc">
    <input type="number" id="principal" value="50000" />
    <input type="number" id="rate" value="5.5" />
    <input type="number" id="term" value="15" />
</form>

<table border="1" cellpadding="10" style="width: 100%;">
    <tr>
        <td>Monthly Payment:</td>
        <td><output for="principal rate term" val="$407.39" /></td>
    </tr>
    <tr>
        <td>Total Interest:</td>
        <td><output for="principal rate term" val="$23,330.20" /></td>
    </tr>
    <tr>
        <td>Total Amount:</td>
        <td><output for="principal rate term" val="$73,330.20" /></td>
    </tr>
</table>
```

### Example 5: Multiple Outputs with Different Values

```html
<form id="stats">
    <input type="number" id="score1" value="85" />
    <input type="number" id="score2" value="92" />
    <input type="number" id="score3" value="78" />
</form>

<h3>Statistics</h3>
<p>
    Sum: <output for="score1 score2 score3" val="255" /><br/>
    Average: <output for="score1 score2 score3" val="85.0" /><br/>
    Highest: <output for="score1 score2 score3" val="92" /><br/>
    Lowest: <output for="score1 score2 score3" val="78" />
</p>
```

### Example 6: Rich Content vs Simple Value

```html
<!-- Simple value with val attribute -->
<output for="total" val="$1,234.56" />

<!-- Rich content without val attribute -->
<output for="total">
    <strong style="color: #336699; font-size: 14pt;">$1,234.56</strong>
</output>

<!-- Styled output container with val -->
<output for="total" val="$1,234.56"
        style="display: block; padding: 10pt; background-color: #e3f2fd;
               font-size: 14pt; font-weight: bold; color: #336699;" />
```

### Example 7: Shopping Cart Total

```html
<form id="cart">
    <table border="1" cellpadding="8" style="width: 100%;">
        <tr>
            <td>Product A</td>
            <td><input type="number" id="qtyA" value="2" /></td>
            <td>$50.00</td>
            <td><output val="$100.00" /></td>
        </tr>
        <tr>
            <td>Product B</td>
            <td><input type="number" id="qtyB" value="1" /></td>
            <td>$75.00</td>
            <td><output val="$75.00" /></td>
        </tr>
        <tr style="font-weight: bold;">
            <td colspan="3" style="text-align: right;">Total:</td>
            <td><output for="qtyA qtyB" val="$175.00" /></td>
        </tr>
    </table>
</form>
```

### Example 8: Dynamic Value Calculation

```html
<!-- Model: { items: [
    {name: "Widget", qty: 10, price: 25},
    {name: "Gadget", qty: 5, price: 50}
]} -->

<h2>Order Summary</h2>

<template data-bind="{{model.items}}">
    <div style="margin-bottom: 10pt;">
        {{.name}}: <output val="{{.qty}} × ${{.price}} = ${{.qty * .price}}" />
    </div>
</template>

<div style="font-weight: bold; margin-top: 15pt; padding-top: 10pt; border-top: 2pt solid black;">
    Grand Total: <output val="${{sum(model.items, .qty * .price)}}" />
</div>
```

### Example 9: Report with Calculated Percentages

```html
<h2>Sales Performance Report</h2>

<form id="salesData">
    <input type="number" id="target" value="100000" />
    <input type="number" id="actual" value="125000" />
</form>

<table border="1" cellpadding="10" style="width: 100%;">
    <tr>
        <td>Sales Target:</td>
        <td><output for="target" val="$100,000" /></td>
    </tr>
    <tr>
        <td>Actual Sales:</td>
        <td><output for="actual" val="$125,000" /></td>
    </tr>
    <tr>
        <td>Difference:</td>
        <td><output for="target actual" val="$25,000" /></td>
    </tr>
    <tr style="background-color: #d4edda;">
        <td><strong>Performance:</strong></td>
        <td><output for="target actual" val="125% of target" /></td>
    </tr>
</table>
```

### Example 10: Invoice with Line Items

```html
<!-- Model: { lineItems: [
    {desc: "Consulting", qty: 40, rate: 150, total: 6000},
    {desc: "Development", qty: 80, rate: 120, total: 9600}
], subtotal: 15600, tax: 1248, grandTotal: 16848} -->

<h1>Invoice</h1>

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
        <template data-bind="{{model.lineItems}}">
            <tr>
                <td>{{.desc}}</td>
                <td>{{.qty}}</td>
                <td>${{.rate}}</td>
                <td><output val="${{.total}}" /></td>
            </tr>
        </template>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="3" style="text-align: right;"><strong>Subtotal:</strong></td>
            <td><output val="${{model.subtotal}}" /></td>
        </tr>
        <tr>
            <td colspan="3" style="text-align: right;"><strong>Tax (8%):</strong></td>
            <td><output val="${{model.tax}}" /></td>
        </tr>
        <tr style="background-color: #e3f2fd; font-weight: bold; font-size: 12pt;">
            <td colspan="3" style="text-align: right;">TOTAL:</td>
            <td><output val="${{model.grandTotal}}" /></td>
        </tr>
    </tfoot>
</table>
```

### Example 11: Grade Calculator

```html
<form id="grades">
    <h2>Course Grades</h2>
    <label>Homework: <input type="number" id="homework" value="85" /> (20%)</label><br/>
    <label>Midterm: <input type="number" id="midterm" value="78" /> (30%)</label><br/>
    <label>Final: <input type="number" id="final" value="92" /> (50%)</label>
</form>

<div style="margin-top: 20pt; padding: 15pt; background-color: #f0f0f0;">
    <h3>Final Grade Calculation</h3>
    <p>
        Homework contribution: <output for="homework" val="17.0 points" /><br/>
        Midterm contribution: <output for="midterm" val="23.4 points" /><br/>
        Final contribution: <output for="final" val="46.0 points" />
    </p>
    <p style="font-size: 14pt; font-weight: bold; color: #336699;">
        Final Grade: <output for="homework midterm final" val="86.4 (B)" />
    </p>
</div>
```

### Example 12: BMI Calculator

```html
<form id="bmiCalc">
    <label>Weight (lbs): <input type="number" id="weight" value="180" /></label><br/>
    <label>Height (inches): <input type="number" id="height" value="70" /></label>
</form>

<div style="margin-top: 20pt;">
    <h3>Results</h3>
    <table border="0" cellpadding="8">
        <tr>
            <td>Body Mass Index (BMI):</td>
            <td><output for="weight height" val="25.8" /></td>
        </tr>
        <tr>
            <td>Category:</td>
            <td><output for="weight height" val="Overweight" /></td>
        </tr>
        <tr>
            <td>Healthy Range:</td>
            <td><output val="18.5 - 24.9" /></td>
        </tr>
    </table>
</div>
```

### Example 13: Time Duration Calculator

```html
<form id="timeCalc">
    <label>Start: <input type="time" id="start" value="09:00" /></label><br/>
    <label>End: <input type="time" id="end" value="17:30" /></label>
</form>

<div style="margin-top: 15pt;">
    <h3>Duration</h3>
    <p>
        Total hours: <output for="start end" val="8.5 hours" /><br/>
        Total minutes: <output for="start end" val="510 minutes" /><br/>
        Work day: <output for="start end" val="Full day (8+ hours)" />
    </p>
</div>
```

### Example 14: Temperature Converter

```html
<form id="tempConvert">
    <label>Celsius: <input type="number" id="celsius" value="25" /></label>
</form>

<div style="margin-top: 15pt;">
    <h3>Conversions</h3>
    <table border="1" cellpadding="10">
        <tr>
            <td>Fahrenheit:</td>
            <td><output for="celsius" val="77.0°F" /></td>
        </tr>
        <tr>
            <td>Kelvin:</td>
            <td><output for="celsius" val="298.15 K" /></td>
        </tr>
    </table>
</div>
```

### Example 15: Conditional Output Values

```html
<!-- Model: { score: 95, passed: true } -->

<form id="examResults">
    <input type="number" id="score" name="score" value="{{model.score}}" />
</form>

<div style="padding: 20pt; {{model.passed ? 'background-color: #d4edda;' : 'background-color: #f8d7da;'}}">
    <h2>Exam Results</h2>

    <p>
        Your Score: <output for="score" val="{{model.score}}/100" /><br/>
        Status: <output for="score" val="{{model.passed ? 'PASSED' : 'FAILED'}}" /><br/>
        Grade: <output for="score" val="{{model.score >= 90 ? 'A' : model.score >= 80 ? 'B' : model.score >= 70 ? 'C' : model.score >= 60 ? 'D' : 'F'}}" />
    </p>
</div>
```

---

## See Also

- [output element](/reference/htmltags/output.html) - The output HTML element
- [form attribute](/reference/htmlattributes/form.html) - Associates output with form
- [for attribute](/reference/htmlattributes/for.html) - Associates output with form controls
- [value attribute](/reference/htmlattributes/value.html) - Input value attribute
- [name attribute](/reference/htmlattributes/name.html) - Names form elements
- [Data Binding](/reference/binding/) - Dynamic data binding
- [Expressions](/reference/expressions/) - Expression syntax
- [HTML Forms](/reference/forms/) - Complete form reference

---
