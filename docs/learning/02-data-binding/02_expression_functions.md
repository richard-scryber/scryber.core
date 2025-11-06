---
layout: default
title: Expression Functions
nav_order: 2
parent: Data Binding & Expressions
parent_url: /learning/02-data-binding/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Expression Functions

Master built-in functions for string manipulation, math operations, dates, and conditional logic.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Use string functions (concat, substring, upper, lower)
- Perform math operations (add, subtract, multiply, divide)
- Use the calc() function for calculations
- Work with date functions
- Apply conditional functions (if, choose)
- Combine functions in expressions

---

## Function Syntax

Functions are called within expression brackets:

{% raw %}
```html
{{function(arg1, arg2, ...)}}
```
{% endraw %}

---

## String Functions

### concat() - Concatenate Strings

{% raw %}
```html
<!-- Combine multiple strings -->
<p>{{concat(model.firstName, ' ', model.lastName)}}</p>

<!-- Result: John Doe -->

<!-- Combine with literals -->
<p>{{concat('Hello, ', model.name, '!')}}</p>

<!-- Result: Hello, John! -->
```
{% endraw %}

### upper() - Convert to Uppercase

{% raw %}
```html
<p>{{upper(model.name)}}</p>

<!-- Input: john doe -->
<!-- Result: JOHN DOE -->
```
{% endraw %}

### lower() - Convert to Lowercase

{% raw %}
```html
<p>{{lower(model.name)}}</p>

<!-- Input: JOHN DOE -->
<!-- Result: john doe -->
```
{% endraw %}

### substring() - Extract Part of String

{% raw %}
```html
<!-- substring(string, start, length) -->
<p>{{substring(model.description, 0, 50)}}</p>

<!-- Show first 50 characters -->

<!-- substring(string, start) -->
<p>{{substring(model.text, 10)}}</p>

<!-- From position 10 to end -->
```
{% endraw %}

### trim() - Remove Whitespace

{% raw %}
```html
<p>{{trim(model.text)}}</p>

<!-- Input: "  hello  " -->
<!-- Result: "hello" -->
```
{% endraw %}

### replace() - Replace Text

{% raw %}
```html
<!-- replace(string, oldValue, newValue) -->
<p>{{replace(model.text, 'old', 'new')}}</p>

<!-- Input: "This is old text" -->
<!-- Result: "This is new text" -->
```
{% endraw %}

---

## Math Functions

### add() - Addition

{% raw %}
```html
<p>{{add(model.price, model.tax)}}</p>

<!-- price: 100, tax: 8 -->
<!-- Result: 108 -->

<!-- Multiple values -->
<p>{{add(10, 20, 30, 40)}}</p>

<!-- Result: 100 -->
```
{% endraw %}

### subtract() - Subtraction

{% raw %}
```html
<p>{{subtract(model.total, model.discount)}}</p>

<!-- total: 150, discount: 20 -->
<!-- Result: 130 -->
```
{% endraw %}

### multiply() - Multiplication

{% raw %}
```html
<p>{{multiply(model.quantity, model.price)}}</p>

<!-- quantity: 5, price: 20 -->
<!-- Result: 100 -->
```
{% endraw %}

### divide() - Division

{% raw %}
```html
<p>{{divide(model.total, model.count)}}</p>

<!-- total: 100, count: 4 -->
<!-- Result: 25 -->
```
{% endraw %}

### calc() - Complex Calculations

The `calc()` function allows complex math expressions:

{% raw %}
```html
<!-- Basic arithmetic -->
<p>{{calc(model.price, '+', model.tax)}}</p>

<!-- With literals -->
<p>{{calc(model.quantity, '*', 10)}}</p>

<!-- Multiple operations (evaluated left to right) -->
<p>{{calc(model.price, '*', model.quantity, '+', model.tax)}}</p>

<!-- (price * quantity) + tax -->

<!-- Parentheses for order of operations -->
<p>{{calc('(', model.price, '+', model.tax, ')', '*', model.quantity)}}</p>

<!-- (price + tax) * quantity -->
```
{% endraw %}

**Supported operators:**
- `+` Addition
- `-` Subtraction
- `*` Multiplication
- `/` Division
- `(` `)` Parentheses for grouping

**Example:**

```csharp
doc.Params["model"] = new
{
    price = 100,
    quantity = 3,
    tax = 15
};
```

{% raw %}
```html
<!-- Total with tax -->
<p>Subtotal: {{calc(model.price, '*', model.quantity)}}</p>
<!-- Result: 300 -->

<p>Total: {{calc(model.price, '*', model.quantity, '+', model.tax)}}</p>
<!-- Result: 315 -->
```
{% endraw %}

---

## Conditional Functions

### if() - Inline Conditional

{% raw %}
```html
<!-- if(condition, trueValue, falseValue) -->
<p>{{if(model.isPremium, 'Premium Customer', 'Standard Customer')}}</p>

<!-- Numeric comparison -->
<p>{{if(model.age >= 18, 'Adult', 'Minor')}}</p>

<!-- String comparison -->
<p>{{if(model.status == 'active', 'Active', 'Inactive')}}</p>
```
{% endraw %}

### choose() - Multiple Conditions

{% raw %}
```html
<!-- choose(index, value1, value2, value3, ...) -->
<p>{{choose(model.priority, 'Low', 'Medium', 'High')}}</p>

<!-- If priority is 0: Low -->
<!-- If priority is 1: Medium -->
<!-- If priority is 2: High -->
```
{% endraw %}

---

## Comparison Operators

Use in conditional functions:

{% raw %}
```html
<!-- Equal -->
{{if(model.status == 'active', 'Yes', 'No')}}

<!-- Not equal -->
{{if(model.status != 'inactive', 'Active', 'Not Active')}}

<!-- Greater than -->
{{if(model.age > 18, 'Adult', 'Minor')}}

<!-- Greater than or equal -->
{{if(model.score >= 70, 'Pass', 'Fail')}}

<!-- Less than -->
{{if(model.temperature < 32, 'Freezing', 'Not Freezing')}}

<!-- Less than or equal -->
{{if(model.grade <= 59, 'Fail', 'Pass')}}
```
{% endraw %}

---

## Logical Operators

### AND

{% raw %}
```html
<!-- Both conditions must be true -->
{{if(model.age >= 18 && model.hasLicense, 'Can Drive', 'Cannot Drive')}}
```
{% endraw %}

### OR

{% raw %}
```html
<!-- At least one condition must be true -->
{{if(model.isAdmin || model.isModerator, 'Has Access', 'No Access')}}
```
{% endraw %}

### NOT

{% raw %}
```html
<!-- Negate condition -->
{{if(!model.isBlocked, 'Active', 'Blocked')}}
```
{% endraw %}

---

## Date Functions

### now() - Current Date/Time

{% raw %}
```html
<p>Generated: {{now()}}</p>

<!-- Result: 2025-01-15 10:30:00 -->
```
{% endraw %}

### format() - Format Date

{% raw %}
```html
<!-- format(date, formatString) -->
<p>{{format(model.orderDate, 'yyyy-MM-dd')}}</p>

<!-- Result: 2025-01-15 -->

<p>{{format(model.orderDate, 'MMMM dd, yyyy')}}</p>

<!-- Result: January 15, 2025 -->
```
{% endraw %}

**Common format strings:**
- `yyyy-MM-dd` - 2025-01-15
- `MM/dd/yyyy` - 01/15/2025
- `MMMM dd, yyyy` - January 15, 2025
- `dd-MMM-yy` - 15-Jan-25
- `HH:mm:ss` - 14:30:45

---

## Number Formatting

### format() - Format Numbers

{% raw %}
```html
<!-- Currency -->
<p>{{format(model.amount, 'C')}}</p>

<!-- Result: $1,234.56 -->

<!-- Fixed decimal places -->
<p>{{format(model.price, 'F2')}}</p>

<!-- Result: 123.45 -->

<!-- Percentage -->
<p>{{format(model.rate, 'P')}}</p>

<!-- Result: 12.5% -->

<!-- With thousands separator -->
<p>{{format(model.population, 'N0')}}</p>

<!-- Result: 1,234,567 -->
```
{% endraw %}

---

## Practical Examples

### Example 1: Invoice Line Total

{% raw %}
```html
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
        {{#each model.items}}
        <tr>
            <td>{{this.description}}</td>
            <td>{{this.quantity}}</td>
            <td>{{format(this.price, 'C')}}</td>
            <td>{{format(calc(this.quantity, '*', this.price), 'C')}}</td>
        </tr>
        {{/each}}
    </tbody>
</table>
```
{% endraw %}

### Example 2: Customer Greeting

{% raw %}
```html
<h1>{{if(model.timeOfDay < 12, 'Good Morning',
        if(model.timeOfDay < 18, 'Good Afternoon', 'Good Evening'))}},
    {{model.customerName}}!</h1>
```
{% endraw %}

### Example 3: Status Badge

{% raw %}
```html
<div style="padding: 5pt; background-color: {{if(model.status == 'success', '#10b981',
                                                  if(model.status == 'warning', '#f59e0b',
                                                  if(model.status == 'error', '#ef4444', '#6b7280')))}};
             color: white;">
    {{upper(model.status)}}
</div>
```
{% endraw %}

### Example 4: Discount Calculation

{% raw %}
```html
<div class="pricing">
    <p>Original Price: {{format(model.originalPrice, 'C')}}</p>
    <p>Discount ({{model.discountPercent}}%):
       {{format(calc(model.originalPrice, '*', model.discountPercent, '/', 100), 'C')}}</p>
    <p style="font-weight: bold; font-size: 14pt;">
        Final Price: {{format(calc(model.originalPrice, '-',
                                   calc(model.originalPrice, '*', model.discountPercent, '/', 100)), 'C')}}
    </p>
</div>
```
{% endraw %}

### Example 5: Text Truncation with Ellipsis

{% raw %}
```html
<p>
    {{if(length(model.description) > 100,
         concat(substring(model.description, 0, 97), '...'),
         model.description)}}
</p>
```
{% endraw %}

---

## Complete Example: Dynamic Report

**C# Code:**
```csharp
var doc = Document.ParseDocument("report.html");

doc.Params["model"] = new
{
    reportTitle = "Q4 Sales Report",
    generatedDate = DateTime.Now,
    author = "sales department",
    sales = new[]
    {
        new { region = "North", revenue = 125000, target = 100000 },
        new { region = "South", revenue = 98000, target = 110000 },
        new { region = "East", revenue = 150000, target = 140000 },
        new { region = "West", revenue = 87000, target = 90000 }
    },
    totalRevenue = 460000,
    totalTarget = 440000
};

doc.SaveAsPDF("report.pdf");
```

**Template:**
{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Sales Report</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }
        h1 {
            color: #1e40af;
            text-transform: uppercase;
        }
        .header-info {
            font-size: 10pt;
            color: #666;
            margin-bottom: 30pt;
        }
        table {
            width: 100%;
            border-collapse: collapse;
        }
        th {
            background-color: #2563eb;
            color: white;
            padding: 10pt;
            text-align: left;
        }
        td {
            padding: 8pt;
            border-bottom: 1pt solid #e5e7eb;
        }
        .met-target {
            color: #059669;
            font-weight: bold;
        }
        .missed-target {
            color: #dc2626;
        }
    </style>
</head>
<body>
    <h1>{{upper(model.reportTitle)}}</h1>

    <div class="header-info">
        <p>Generated: {{format(model.generatedDate, 'MMMM dd, yyyy HH:mm')}}</p>
        <p>Author: {{concat(upper(substring(model.author, 0, 1)), substring(model.author, 1))}}</p>
    </div>

    <table>
        <thead>
            <tr>
                <th>Region</th>
                <th>Revenue</th>
                <th>Target</th>
                <th>Performance</th>
                <th>Status</th>
            </tr>
        </thead>
        <tbody>
            {{#each model.sales}}
            <tr>
                <td>{{this.region}}</td>
                <td>{{format(this.revenue, 'C0')}}</td>
                <td>{{format(this.target, 'C0')}}</td>
                <td>{{format(calc(this.revenue, '/', this.target), 'P0')}}</td>
                <td class="{{if(this.revenue >= this.target, 'met-target', 'missed-target')}}">
                    {{if(this.revenue >= this.target, '✓ Met', '✗ Missed')}}
                </td>
            </tr>
            {{/each}}
        </tbody>
        <tfoot>
            <tr style="background-color: #eff6ff; font-weight: bold;">
                <td>TOTAL</td>
                <td>{{format(model.totalRevenue, 'C0')}}</td>
                <td>{{format(model.totalTarget, 'C0')}}</td>
                <td>{{format(calc(model.totalRevenue, '/', model.totalTarget), 'P0')}}</td>
                <td class="{{if(model.totalRevenue >= model.totalTarget, 'met-target', 'missed-target')}}">
                    {{if(model.totalRevenue >= model.totalTarget, '✓ Above Target', '✗ Below Target')}}
                </td>
            </tr>
        </tfoot>
    </table>

    <div style="margin-top: 30pt; padding: 15pt; background-color: #fef3c7; border-left: 4pt solid #f59e0b;">
        <strong>Summary:</strong>
        Total revenue of {{format(model.totalRevenue, 'C0')}} is
        {{if(model.totalRevenue >= model.totalTarget,
             concat(format(calc(calc(model.totalRevenue, '-', model.totalTarget), '/', model.totalTarget), 'P0'), ' above target'),
             concat(format(calc(calc(model.totalTarget, '-', model.totalRevenue), '/', model.totalTarget), 'P0'), ' below target'))}}.
    </div>
</body>
</html>
```
{% endraw %}

---

## Try It Yourself

### Exercise 1: Price Calculator

Create a template that:
- Shows original price
- Calculates discount amount
- Shows final price
- Uses color coding (green if discounted, black if not)

### Exercise 2: Grade Reporter

Create a template that:
- Shows student name
- Shows numeric score
- Calculates letter grade using if() functions
- Shows "Pass" or "Fail"

### Exercise 3: Date Formatter

Create a template that displays the same date in:
- Short format (MM/dd/yyyy)
- Long format (MMMM dd, yyyy)
- ISO format (yyyy-MM-dd)
- Custom format

---

## Common Pitfalls

### ❌ Missing Quotes in Strings

{% raw %}
```html
{{concat(Hello, model.name)}}  <!-- Error: Hello is not defined -->
```
{% endraw %}

✅ **Solution:** Quote string literals

{% raw %}
```html
{{concat('Hello, ', model.name)}}
```
{% endraw %}

### ❌ Wrong calc() Syntax

{% raw %}
```html
{{calc(model.price + model.tax)}}  <!-- Error: operators as strings -->
```
{% endraw %}

✅ **Solution:** Operators must be quoted strings

{% raw %}
```html
{{calc(model.price, '+', model.tax)}}
```
{% endraw %}

### ❌ Incorrect Parentheses in calc()

{% raw %}
```html
{{calc((model.a + model.b), '*', model.c)}}  <!-- Error -->
```
{% endraw %}

✅ **Solution:** Parentheses must be separate arguments

{% raw %}
```html
{{calc('(', model.a, '+', model.b, ')', '*', model.c)}}
```
{% endraw %}

---

## Next Steps

Now that you can use expression functions:

1. **[Template Iteration](03_template_iteration.md)** - Loop through data
2. **[Conditional Rendering](04_conditional_rendering.md)** - Show/hide content
3. **[Variables & Params](05_variables_params.md)** - Store calculated values

---

**Continue learning →** [Template Iteration](03_template_iteration.md)
