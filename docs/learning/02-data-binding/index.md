---
layout: default
title: Data Binding & Expressions
nav_order: 2
parent: Learning Guides
parent_url: /learning/
has_children: true
has_toc: false
---

# Data Binding & Expressions

Transform static templates into dynamic, data-driven PDF documents with Scryber's powerful data binding system.

---

## Table of Contents

1. [Data Binding Basics](01_data_binding_basics.md) - Basic syntax, properties, passing data, data context
2. [Expression Functions](02_expression_functions.md) - String, math (calc), date, conditional functions
3. [Template Iteration](03_template_iteration.md) - Template element, {{#each}}, loops
4. [Conditional Rendering](04_conditional_rendering.md) - {{#if}}, {{#unless}}, inline conditionals
5. [Variables & Document Parameters](05_variables_params.md) - &lt;var&gt; element, Document.Params, calculated values
6. [Context & Scope](06_context_scope.md) - Data context, parent (..), root (@@root)
7. [Formatting Output](07_formatting_output.md) - Number, date, currency formatting
8. [Advanced Patterns](08_advanced_patterns.md) - Complex expressions, aggregation, best practices

---

## Overview

Data binding is what makes Scryber truly powerful. Instead of hard-coding content, you can create templates that pull data from your application, databases, APIs, or any other source. This series teaches you how to master dynamic content generation.

## What is Data Binding?

Data binding connects your template to your data. In Scryber, this means:

- **Dynamic Values** - Display data using `{{expression}}` syntax
- **Conditional Content** - Show or hide sections based on data
- **Loops & Iteration** - Repeat content for lists and arrays
- **Calculations** - Perform math and string operations
- **Formatting** - Format dates, numbers, and currency

## Quick Example

### Template
{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<body>
    <div>
        <h1>Invoice #{{invoice.number}}</h1>
        <p>Total: {{invoice.total}}</p>

        <table>
            {{#each invoice.items}}
            <tr>
                <td>{{this.name}}</td>
                <td>{{this.price}}</td>
            </tr>
            {{/each}}
        </table>
    </div>
</body>
</html>
```
{% endraw %}

### C# Code
```csharp
doc.Params["invoice"] = new {
    number = "INV-2025-001",
    total = "$1,250.00",
    items = new[] {
        new { name = "Service A", price = "$500.00" },
        new { name = "Service B", price = "$750.00" }
    }
};
```

### Result
A fully populated invoice with all items listed dynamically!

## What You'll Learn

This series covers everything you need to create sophisticated data-driven documents:

### 1. [Data Binding Basics](01_data_binding_basics.md)
- Basic `{{expression}}` syntax
- Binding to properties and nested objects
- Passing data from C#, JSON, and XML
- Understanding data context

### 2. [Expression Functions](02_expression_functions.md)
- String functions (concat, substring, upper, lower)
- Math functions (add, subtract, multiply, divide, **calc**)
- Date functions
- Conditional functions (if, choose)
- Comparison and logical operators

### 3. [Template Iteration](03_template_iteration.md)
- Using the `<template>` element
- Iterating with `{{#each}}`
- Working with `{{@index}}` and `{{@key}}`
- Nested loops and complex data structures

### 4. [Conditional Rendering](04_conditional_rendering.md)
- `{{#if}}` and `{{#unless}}` helpers
- Inline conditionals with `if()` function
- `{{else}}` clauses
- Conditional visibility and sections

### 5. [Variables & Document Parameters](05_variables_params.md)
- Using the `<var>` element to store values
- Document.Params for global access
- Variable scope and context
- Storing calculated values

### 6. [Context & Scope](06_context_scope.md)
- Understanding data context
- Parent context access with `..`
- Root context with `@@root`
- Context in nested structures

### 7. [Formatting Output](07_formatting_output.md)
- Number formatting
- Date and time formatting
- Currency formatting
- Custom format strings

### 8. [Advanced Patterns](08_advanced_patterns.md)
- Complex expressions
- Aggregation (sum, count, avg)
- Performance considerations
- Error handling
- Best practices

## Prerequisites

Before starting this series:

- **Complete [Getting Started](/learning/01-getting-started/)** - Basic Scryber knowledge required
- **Understand HTML** - Template structure
- **C# Basics** - For passing data to templates

## Key Concepts

### Expression Syntax

Scryber uses Handlebars-style syntax for data binding:

{% raw %}
```html
<!-- Simple value -->
<p>{{userName}}</p>

<!-- Nested property -->
<p>{{user.address.city}}</p>

<!-- Function call -->
<p>{{upper(userName)}}</p>

<!-- Conditional -->
<p>{{if(isActive, 'Active', 'Inactive')}}</p>
```
{% endraw %}

### Block Helpers

Block helpers control template flow:

{% raw %}
```html
<!-- Iteration -->
{{#each items}}
    <li>{{this.name}}</li>
{{/each}}

<!-- Conditional -->
{{#if showDetails}}
    <div>Details here</div>
{{/if}}
```
{% endraw %}

### Data Context

Every expression evaluates in a data context:

{% raw %}
```html
<!-- Root level: model.name -->
{{name}}

<!-- In each: current item -->
{{#each orders}}
    {{this.total}}  <!-- Current order's total -->
    {{..name}}      <!-- Parent context -->
{{/each}}
```
{% endraw %}

## Real-World Applications

Data binding enables powerful scenarios:

### Dynamic Invoices
{% raw %}
```html
<h1>Invoice #{{invoice.number}}</h1>
<p>Date: {{format(invoice.date, 'MMM dd, yyyy')}}</p>

{{#each invoice.items}}
<tr>
    <td>{{this.description}}</td>
    <td>{{format(this.amount, 'C2')}}</td>
</tr>
{{/each}}

<p><strong>Total: {{format(invoice.total, 'C2')}}</strong></p>
```
{% endraw %}

### Conditional Sections
{% raw %}
```html
{{#if customer.isPremium}}
<div class="premium-badge">Premium Customer</div>
{{/if}}

{{#if invoice.pastDue}}
<div class="warning">Payment Overdue</div>
{{/if}}
```
{% endraw %}

### Calculated Values
{% raw %}
```html
<var data-id="subtotal" data-value="{{calc(quantity, '*', price)}}" />
<var data-id="tax" data-value="{{calc(Document.Params.subtotal, '*', 0.08)}}" />
<var data-id="total" data-value="{{calc(Document.Params.subtotal, '+', Document.Params.tax)}}" />

<p>Subtotal: ${{Document.Params.subtotal}}</p>
<p>Tax (8%): ${{Document.Params.tax}}</p>
<p>Total: ${{Document.Params.total}}</p>
```
{% endraw %}

### Multi-Language Support
{% raw %}
```html
{{#if language == 'es'}}
    <h1>Hola, {{userName}}!</h1>
{{else}}
    <h1>Hello, {{userName}}!</h1>
{{/if}}
```
{% endraw %}

## Common Patterns

### Master-Detail Reports
{% raw %}
```html
{{#each departments}}
<h2>{{this.name}}</h2>
<table>
    {{#each this.employees}}
    <tr>
        <td>{{this.name}}</td>
        <td>{{this.title}}</td>
    </tr>
    {{/each}}
</table>
{{/each}}
```
{% endraw %}

### Running Totals
{% raw %}
```html
<var data-id="runningTotal" data-value="0" />

{{#each items}}
<tr>
    <td>{{this.name}}</td>
    <td>{{this.amount}}</td>
    <var data-id="runningTotal"
         data-value="{{calc(Document.Params.runningTotal, '+', this.amount)}}" />
    <td>{{Document.Params.runningTotal}}</td>
</tr>
{{/each}}
```
{% endraw %}

### Conditional Formatting
{% raw %}
```html
{{#each products}}
<tr style="color: {{if(this.inStock, 'black', 'red')}}">
    <td>{{this.name}}</td>
    <td>{{if(this.inStock, 'In Stock', 'Out of Stock')}}</td>
</tr>
{{/each}}
```
{% endraw %}

## Learning Path

**Recommended order for this series:**

1. **Start with Basics** - Articles 1-2 cover fundamental concepts
2. **Learn Iteration & Conditionals** - Articles 3-4 for dynamic content
3. **Master Variables** - Article 5 for storing and reusing values
4. **Understand Context** - Article 6 for complex data structures
5. **Add Formatting** - Article 7 for professional output
6. **Apply Advanced Patterns** - Article 8 for real-world scenarios

## Tips for Success

1. **Start Simple** - Begin with basic `{{property}}` expressions
2. **Test Incrementally** - Add complexity gradually
3. **Understand Context** - Know where you are in the data structure
4. **Use Variables** - Store calculated values for reuse
5. **Check Data First** - Ensure data is available before binding
6. **Handle Nulls** - Use `if()` to check for null/undefined values

## Next Steps

Ready to dive in? Start with [Data Binding Basics](01_data_binding_basics.md) to learn the fundamentals of dynamic content.

Already familiar with basics? Jump to:
- [Expression Functions](02_expression_functions.md) for calculations
- [Template Iteration](03_template_iteration.md) for loops
- [Variables & Document Parameters](05_variables_params.md) for value storage

---

**Related Series:**
- [Getting Started](/learning/01-getting-started/) - Prerequisites
- [Content Components](/learning/06-content/) - Dynamic tables and images
- [Practical Applications](/learning/08-practical/) - Real-world examples

---

**Begin your journey →** [Data Binding Basics](01_data_binding_basics.md)
