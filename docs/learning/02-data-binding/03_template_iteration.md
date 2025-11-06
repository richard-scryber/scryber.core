---
layout: default
title: Template Iteration
nav_order: 3
parent: Data Binding & Expressions
parent_url: /learning/02-data-binding/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Template Iteration

Loop through collections and arrays to generate dynamic, repeating content in your PDFs.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Use the `{{#each}}` helper to iterate over collections
- Access current item properties in loops
- Use `{{@index}}` and `{{@key}}` for position tracking
- Create nested loops for complex data
- Handle empty collections gracefully
- Build dynamic tables, lists, and repeating sections

---

## The {{#each}} Helper

The `{{#each}}` helper iterates over arrays and collections:

{% raw %}
```html
{{#each collection}}
    <!-- Content repeated for each item -->
    {{this.property}}
{{/each}}
```
{% endraw %}

---

## Basic Iteration

### Simple Array

**C# Code:**
```csharp
doc.Params["items"] = new[] { "Apple", "Banana", "Orange" };
```

**Template:**
{% raw %}
```html
<ul>
    {{#each items}}
        <li>{{this}}</li>
    {{/each}}
</ul>
```
{% endraw %}

**Output:**
```html
<ul>
    <li>Apple</li>
    <li>Banana</li>
    <li>Orange</li>
</ul>
```

### Array of Objects

**C# Code:**
```csharp
doc.Params["products"] = new[]
{
    new { name = "Widget A", price = 10.00 },
    new { name = "Widget B", price = 15.00 },
    new { name = "Widget C", price = 20.00 }
};
```

**Template:**
{% raw %}
```html
<ul>
    {{#each products}}
        <li>{{this.name}} - ${{this.price}}</li>
    {{/each}}
</ul>
```
{% endraw %}

**Output:**
```html
<ul>
    <li>Widget A - $10.00</li>
    <li>Widget B - $15.00</li>
    <li>Widget C - $20.00</li>
</ul>
```

---

## Using {{@index}}

Access the current iteration index (0-based):

{% raw %}
```html
<table>
    <thead>
        <tr>
            <th>#</th>
            <th>Product</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        {{#each products}}
        <tr>
            <td>{{add(@index, 1)}}</td>  <!-- Convert to 1-based -->
            <td>{{this.name}}</td>
            <td>${{this.price}}</td>
        </tr>
        {{/each}}
    </tbody>
</table>
```
{% endraw %}

---

## Using {{@key}}

When iterating over dictionaries or objects with named properties:

**C# Code:**
```csharp
doc.Params["stats"] = new Dictionary<string, int>
{
    { "Users", 1250 },
    { "Orders", 3480 },
    { "Revenue", 125000 }
};
```

**Template:**
{% raw %}
```html
<dl>
    {{#each stats}}
        <dt>{{@key}}</dt>
        <dd>{{this}}</dd>
    {{/each}}
</dl>
```
{% endraw %}

**Output:**
```html
<dl>
    <dt>Users</dt>
    <dd>1250</dd>
    <dt>Orders</dt>
    <dd>3480</dd>
    <dt>Revenue</dt>
    <dd>125000</dd>
</dl>
```

---

## Nested Loops

### Two-Level Nesting

**C# Code:**
```csharp
doc.Params["departments"] = new[]
{
    new
    {
        name = "Sales",
        employees = new[]
        {
            new { name = "John Doe", title = "Sales Manager" },
            new { name = "Jane Smith", title = "Sales Rep" }
        }
    },
    new
    {
        name = "Engineering",
        employees = new[]
        {
            new { name = "Bob Johnson", title = "Lead Developer" },
            new { name = "Alice Brown", title = "Developer" }
        }
    }
};
```

**Template:**
{% raw %}
```html
{{#each departments}}
    <h2>{{this.name}}</h2>
    <ul>
        {{#each this.employees}}
            <li><strong>{{this.name}}</strong> - {{this.title}}</li>
        {{/each}}
    </ul>
{{/each}}
```
{% endraw %}

### Three-Level Nesting

{% raw %}
```html
{{#each companies}}
    <h1>{{this.companyName}}</h1>
    {{#each this.departments}}
        <h2>{{this.deptName}}</h2>
        <ul>
            {{#each this.employees}}
                <li>{{this.name}} ({{this.role}})</li>
            {{/each}}
        </ul>
    {{/each}}
{{/each}}
```
{% endraw %}

---

## Context Access in Loops

### Parent Context

Access parent scope data inside loops:

{% raw %}
```html
{{#each orders}}
    <p>Order {{this.orderNumber}} for customer {{../customerName}}</p>
    {{#each this.items}}
        <p>  Item: {{this.name}} - Qty: {{this.quantity}}</p>
    {{/each}}
{{/each}}
```
{% endraw %}

The `../` syntax accesses the parent context.

### Root Context

Access the root data from any nested level:

{% raw %}
```html
{{#each departments}}
    {{#each this.employees}}
        <p>{{this.name}} works for {{../../companyName}}</p>
    {{/each}}
{{/each}}
```
{% endraw %}

---

## Empty Collections

### Using {{else}}

Handle empty collections gracefully:

{% raw %}
```html
{{#each products}}
    <div class="product">
        <h3>{{this.name}}</h3>
        <p>Price: ${{this.price}}</p>
    </div>
{{else}}
    <p>No products available.</p>
{{/each}}
```
{% endraw %}

---

## Dynamic Tables

### Invoice Line Items

**C# Code:**
```csharp
doc.Params["model"] = new
{
    invoiceNumber = "INV-2025-001",
    items = new[]
    {
        new { description = "Widget A", quantity = 5, price = 10.00, total = 50.00 },
        new { description = "Widget B", quantity = 3, price = 15.00, total = 45.00 },
        new { description = "Widget C", quantity = 2, price = 20.00, total = 40.00 }
    },
    subtotal = 135.00,
    tax = 10.80,
    total = 145.80
};
```

**Template:**
{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Invoice</title>
    <style>
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
        .total-row {
            background-color: #eff6ff;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <h1>Invoice #{{model.invoiceNumber}}</h1>

    <table>
        <thead>
            <tr>
                <th>#</th>
                <th>Description</th>
                <th>Qty</th>
                <th>Price</th>
                <th style="text-align: right;">Total</th>
            </tr>
        </thead>
        <tbody>
            {{#each model.items}}
            <tr>
                <td>{{add(@index, 1)}}</td>
                <td>{{this.description}}</td>
                <td>{{this.quantity}}</td>
                <td>${{format(this.price, 'F2')}}</td>
                <td style="text-align: right;">${{format(this.total, 'F2')}}</td>
            </tr>
            {{/each}}
        </tbody>
        <tfoot>
            <tr>
                <td colspan="4">Subtotal</td>
                <td style="text-align: right;">${{format(model.subtotal, 'F2')}}</td>
            </tr>
            <tr>
                <td colspan="4">Tax (8%)</td>
                <td style="text-align: right;">${{format(model.tax, 'F2')}}</td>
            </tr>
            <tr class="total-row">
                <td colspan="4">TOTAL</td>
                <td style="text-align: right;">${{format(model.total, 'F2')}}</td>
            </tr>
        </tfoot>
    </table>
</body>
</html>
```
{% endraw %}

---

## Dynamic Lists

### Product Catalog

**C# Code:**
```csharp
doc.Params["catalog"] = new
{
    categoryName = "Electronics",
    products = new[]
    {
        new
        {
            name = "Laptop",
            price = 999.99,
            features = new[] { "16GB RAM", "512GB SSD", "Full HD Display" }
        },
        new
        {
            name = "Smartphone",
            price = 699.99,
            features = new[] { "5G Enabled", "128GB Storage", "Triple Camera" }
        }
    }
};
```

**Template:**
{% raw %}
```html
<h1>{{catalog.categoryName}}</h1>

{{#each catalog.products}}
    <div class="product">
        <h2>{{this.name}}</h2>
        <p class="price">${{format(this.price, 'F2')}}</p>

        <h3>Features:</h3>
        <ul>
            {{#each this.features}}
                <li>{{this}}</li>
            {{/each}}
        </ul>
    </div>
{{/each}}
```
{% endraw %}

---

## Conditional Iteration

### Filtering with {{#if}}

{% raw %}
```html
<h2>Premium Products</h2>
{{#each products}}
    {{#if this.isPremium}}
        <div class="premium-product">
            <h3>{{this.name}}</h3>
            <p>Price: ${{this.price}}</p>
        </div>
    {{/if}}
{{/each}}
```
{% endraw %}

### Alternating Styles

{% raw %}
```html
{{#each items}}
    <div class="{{if(calc(@index, '%', 2), 'odd-row', 'even-row')}}">
        {{this.name}}
    </div>
{{/each}}
```
{% endraw %}

---

## Practical Examples

### Example 1: Employee Directory

**C# Code:**
```csharp
doc.Params["model"] = new
{
    companyName = "Tech Corp",
    employees = new[]
    {
        new { name = "Alice Johnson", department = "Engineering", email = "alice@techcorp.com" },
        new { name = "Bob Smith", department = "Sales", email = "bob@techcorp.com" },
        new { name = "Carol Williams", department = "Engineering", email = "carol@techcorp.com" },
        new { name = "David Brown", department = "HR", email = "david@techcorp.com" }
    }
};
```

**Template:**
{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Employee Directory</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }
        h1 {
            color: #1e40af;
            border-bottom: 2pt solid #1e40af;
            padding-bottom: 10pt;
        }
        .employee {
            padding: 15pt;
            margin-bottom: 15pt;
            border: 1pt solid #d1d5db;
            border-radius: 5pt;
        }
        .employee:nth-child(even) {
            background-color: #f9fafb;
        }
        .name {
            font-size: 14pt;
            font-weight: bold;
            color: #2563eb;
        }
        .department {
            color: #666;
            font-style: italic;
        }
    </style>
</head>
<body>
    <h1>{{model.companyName}} - Employee Directory</h1>

    {{#each model.employees}}
        <div class="employee">
            <div class="name">{{add(@index, 1)}}. {{this.name}}</div>
            <div class="department">{{this.department}}</div>
            <div class="email">{{this.email}}</div>
        </div>
    {{/each}}

    <p style="margin-top: 30pt; color: #666; font-size: 10pt;">
        Total Employees: {{model.employees.length}}
    </p>
</body>
</html>
```
{% endraw %}

### Example 2: Sales Report with Nested Data

**C# Code:**
```csharp
doc.Params["model"] = new
{
    reportTitle = "Q4 2024 Sales Report",
    generatedDate = DateTime.Now,
    regions = new[]
    {
        new
        {
            regionName = "North America",
            totalSales = 1250000,
            salespeople = new[]
            {
                new { name = "John Doe", sales = 450000 },
                new { name = "Jane Smith", sales = 400000 },
                new { name = "Bob Wilson", sales = 400000 }
            }
        },
        new
        {
            regionName = "Europe",
            totalSales = 980000,
            salespeople = new[]
            {
                new { name = "Alice Brown", sales = 520000 },
                new { name = "Charlie Davis", sales = 460000 }
            }
        }
    }
};
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
        }
        .header-info {
            font-size: 10pt;
            color: #666;
            margin-bottom: 30pt;
        }
        .region {
            margin-bottom: 40pt;
            page-break-inside: avoid;
        }
        h2 {
            background-color: #2563eb;
            color: white;
            padding: 10pt;
            margin-bottom: 15pt;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 10pt;
        }
        th {
            background-color: #eff6ff;
            padding: 8pt;
            text-align: left;
            border-bottom: 2pt solid #2563eb;
        }
        td {
            padding: 8pt;
            border-bottom: 1pt solid #e5e7eb;
        }
        .total {
            font-weight: bold;
            font-size: 14pt;
            color: #059669;
        }
    </style>
</head>
<body>
    <h1>{{model.reportTitle}}</h1>
    <div class="header-info">
        Generated: {{format(model.generatedDate, 'MMMM dd, yyyy HH:mm')}}
    </div>

    {{#each model.regions}}
        <div class="region">
            <h2>{{this.regionName}}</h2>

            <table>
                <thead>
                    <tr>
                        <th>Salesperson</th>
                        <th style="text-align: right;">Sales</th>
                        <th style="text-align: right;">% of Region</th>
                    </tr>
                </thead>
                <tbody>
                    {{#each this.salespeople}}
                    <tr>
                        <td>{{this.name}}</td>
                        <td style="text-align: right;">{{format(this.sales, 'C0')}}</td>
                        <td style="text-align: right;">
                            {{format(calc(this.sales, '/', ../totalSales), 'P0')}}
                        </td>
                    </tr>
                    {{/each}}
                </tbody>
            </table>

            <div class="total">
                Region Total: {{format(this.totalSales, 'C0')}}
            </div>
        </div>
    {{/each}}
</body>
</html>
```
{% endraw %}

### Example 3: Product Grid Layout

{% raw %}
```html
<div style="display: table; width: 100%;">
    {{#each products}}
        <div style="display: table-cell; width: 33%; padding: 10pt; vertical-align: top;">
            <div style="border: 1pt solid #d1d5db; padding: 15pt;">
                <img src="{{this.imageUrl}}" style="width: 100%; height: auto;" />
                <h3>{{this.name}}</h3>
                <p class="price">${{format(this.price, 'F2')}}</p>
                <p>{{this.description}}</p>
            </div>
        </div>

        {{#if(calc(add(@index, 1), '%', 3))}}
            <!-- Continue on same row -->
        {{else}}
            </div><div style="display: table; width: 100%;">
        {{/if}}
    {{/each}}
</div>
```
{% endraw %}

---

## Try It Yourself

### Exercise 1: Student Roster

Create a template that displays:
- Course name and instructor
- List of students with:
  - Student number (1, 2, 3...)
  - Name
  - Email
  - Grade
- Total student count

### Exercise 2: Multi-Level Menu

Create a restaurant menu with:
- Multiple categories (Appetizers, Entrees, Desserts)
- Each category has multiple items
- Each item has name, description, and price
- Add alternating background colors

### Exercise 3: Nested Organization Chart

Create an org chart showing:
- Company name
- Multiple departments
- Each department has employees
- Each employee has name, title, and email
- Calculate and show total employees per department

---

## Common Pitfalls

### ❌ Forgetting {{this}}

{% raw %}
```html
{{#each products}}
    <p>{{name}}</p>  <!-- Won't work -->
{{/each}}
```
{% endraw %}

✅ **Solution:** Use `{{this.property}}`

{% raw %}
```html
{{#each products}}
    <p>{{this.name}}</p>
{{/each}}
```
{% endraw %}

### ❌ Wrong Context in Nested Loops

{% raw %}
```html
{{#each departments}}
    {{#each employees}}
        <p>{{deptName}}</p>  <!-- Wrong scope -->
    {{/each}}
{{/each}}
```
{% endraw %}

✅ **Solution:** Use parent context

{% raw %}
```html
{{#each departments}}
    {{#each this.employees}}
        <p>{{../deptName}}</p>
    {{/each}}
{{/each}}
```
{% endraw %}

### ❌ Not Handling Empty Collections

{% raw %}
```html
{{#each items}}
    <div>{{this.name}}</div>
{{/each}}
<!-- Shows nothing if empty -->
```
{% endraw %}

✅ **Solution:** Add {{else}}

{% raw %}
```html
{{#each items}}
    <div>{{this.name}}</div>
{{else}}
    <p>No items found.</p>
{{/each}}
```
{% endraw %}

---

## Next Steps

Now that you can iterate through data:

1. **[Conditional Rendering](04_conditional_rendering.md)** - Show/hide content dynamically
2. **[Variables & Params](05_variables_params.md)** - Store and reuse values
3. **[Context & Scope](06_context_scope.md)** - Master data access patterns

---

**Continue learning →** [Conditional Rendering](04_conditional_rendering.md)
