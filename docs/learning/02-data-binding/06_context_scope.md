---
layout: default
title: Context & Scope
nav_order: 6
parent: Data Binding & Expressions
parent_url: /learning/02-data-binding/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Context & Scope

Master data access patterns in nested templates, loops, and conditional blocks.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Understand the current context (`this`)
- Access parent contexts with `../`
- Navigate to root context
- Manage context in loops and conditionals
- Use context effectively in complex templates
- Avoid common scope-related errors

---

## Understanding Context

In Scryber templates, **context** refers to the current data object being accessed. The context changes as you enter loops and nested structures.

### Root Context

At the document level, the context is the parameters you set:

```csharp
doc.Params["title"] = "My Document";
doc.Params["author"] = "John Doe";
```

{% raw %}
```html
<!-- Root context -->
<h1>{{title}}</h1>
<p>By {{author}}</p>
```
{% endraw %}

---

## The `this` Keyword

`this` refers to the current context object.

### In Root Context

{% raw %}
```html
<!-- At root level, these are equivalent -->
<p>{{title}}</p>
<p>{{this.title}}</p>
```
{% endraw %}

### In Loops

{% raw %}
```html
{{#each products}}
    <!-- this refers to current product -->
    <p>{{this.name}} - ${{this.price}}</p>
{{/each}}
```
{% endraw %}

### Simple Values

When iterating over simple arrays, `this` is the value itself:

{% raw %}
```html
{{#each colors}}
    <div style="background-color: {{this}};">{{this}}</div>
{{/each}}
```
{% endraw %}

```csharp
doc.Params["colors"] = new[] { "#ff0000", "#00ff00", "#0000ff" };
```

---

## Parent Context (`../`)

Access the parent scope from within a nested context.

### One Level Up

{% raw %}
```html
{{#each orders}}
    <div class="order">
        <p>Order #{{this.orderNumber}}</p>
        <p>Customer: {{../customerName}}</p>  <!-- Parent context -->

        {{#each this.items}}
            <div>{{this.name}} - Qty: {{this.quantity}}</div>
        {{/each}}
    </div>
{{/each}}
```
{% endraw %}

```csharp
doc.Params["customerName"] = "Acme Corp";
doc.Params["orders"] = new[]
{
    new
    {
        orderNumber = "001",
        items = new[]
        {
            new { name = "Widget A", quantity = 5 },
            new { name = "Widget B", quantity = 3 }
        }
    }
};
```

### Multiple Levels Up

{% raw %}
```html
{{#each departments}}
    <h2>{{this.name}}</h2>

    {{#each this.teams}}
        <h3>{{this.teamName}}</h3>

        {{#each this.members}}
            <p>
                {{this.memberName}} in {{../teamName}} ({{../../name}})
            </p>
        {{/each}}
    {{/each}}
{{/each}}
```
{% endraw %}

- `this.memberName` - Current member
- `../teamName` - Parent team
- `../../name` - Grandparent department

---

## Root Context Access

Access root-level parameters from any nesting level.

### Using Root Path

While `../` accesses parent, you can navigate to root by using the parameter name directly when it doesn't conflict:

{% raw %}
```html
{{#each model.orders}}
    {{#each this.items}}
        <!-- Access root-level parameter -->
        <p>Company: {{companyName}}</p>
        <p>Item: {{this.name}}</p>
    {{/each}}
{{/each}}
```
{% endraw %}

```csharp
doc.Params["companyName"] = "Tech Corp";
doc.Params["model"] = new
{
    orders = new[]
    {
        new { items = new[] { new { name = "Widget" } } }
    }
};
```

---

## Context in Loops

### Simple Arrays

{% raw %}
```html
{{#each items}}
    <li>{{this}}</li>  <!-- this is the string value -->
{{/each}}
```
{% endraw %}

```csharp
doc.Params["items"] = new[] { "Apple", "Banana", "Orange" };
```

### Arrays of Objects

{% raw %}
```html
{{#each products}}
    <div>
        <h3>{{this.name}}</h3>
        <p>{{this.description}}</p>
        <p>${{this.price}}</p>
    </div>
{{/each}}
```
{% endraw %}

### Nested Arrays

{% raw %}
```html
{{#each categories}}
    <h2>{{this.categoryName}}</h2>

    {{#each this.products}}
        <div class="product">
            <h3>{{this.name}}</h3>
            <p>Category: {{../categoryName}}</p>  <!-- Parent context -->
            <p>Price: ${{this.price}}</p>
        </div>
    {{/each}}
{{/each}}
```
{% endraw %}

---

## Context in Conditionals

Context remains the same inside `{{#if}}` blocks:

{% raw %}
```html
{{#each products}}
    <div class="product">
        <h3>{{this.name}}</h3>

        {{#if this.onSale}}
            <span class="sale-badge">SALE!</span>
            <p>Was: ${{this.originalPrice}}</p>
            <p>Now: ${{this.salePrice}}</p>
        {{else}}
            <p>Price: ${{this.price}}</p>
        {{/if}}
    </div>
{{/each}}
```
{% endraw %}

---

## Practical Examples

### Example 1: Invoice with Customer and Line Items

**C# Code:**
```csharp
doc.Params["company"] = new
{
    name = "Tech Corp",
    address = "123 Main St",
    phone = "555-1234"
};

doc.Params["invoice"] = new
{
    number = "INV-2025-001",
    date = DateTime.Now,
    customer = new
    {
        name = "Acme Corporation",
        address = "456 Oak Ave",
        contact = "John Smith"
    },
    items = new[]
    {
        new { description = "Web Development", hours = 40, rate = 150.00 },
        new { description = "Consulting", hours = 10, rate = 200.00 },
        new { description = "Support", hours = 5, rate = 100.00 }
    },
    subtotal = 8500.00,
    tax = 680.00,
    total = 9180.00
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
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }
        .header {
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 15pt;
            margin-bottom: 30pt;
        }
        .company-info {
            float: right;
            text-align: right;
        }
        .customer-info {
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
    </style>
</head>
<body>
    <!-- Header with company info (root context) -->
    <div class="header">
        <div class="company-info">
            <strong>{{company.name}}</strong><br />
            {{company.address}}<br />
            {{company.phone}}
        </div>
        <h1>INVOICE</h1>
    </div>

    <!-- Invoice details -->
    <p><strong>Invoice #:</strong> {{invoice.number}}</p>
    <p><strong>Date:</strong> {{format(invoice.date, 'MMMM dd, yyyy')}}</p>

    <!-- Customer info (nested context) -->
    <div class="customer-info">
        <h2>Bill To:</h2>
        <p>
            <strong>{{invoice.customer.name}}</strong><br />
            {{invoice.customer.address}}<br />
            Contact: {{invoice.customer.contact}}
        </p>
    </div>

    <!-- Line items table -->
    <table>
        <thead>
            <tr>
                <th>Description</th>
                <th>Hours</th>
                <th>Rate</th>
                <th style="text-align: right;">Amount</th>
            </tr>
        </thead>
        <tbody>
            {{#each invoice.items}}
            <tr>
                <td>{{this.description}}</td>
                <td>{{this.hours}}</td>
                <td>${{format(this.rate, 'F2')}}</td>
                <td style="text-align: right;">
                    ${{format(calc(this.hours, '*', this.rate), 'F2')}}
                </td>
            </tr>
            {{/each}}
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3"><strong>Subtotal</strong></td>
                <td style="text-align: right;">${{format(invoice.subtotal, 'F2')}}</td>
            </tr>
            <tr>
                <td colspan="3">Tax (8%)</td>
                <td style="text-align: right;">${{format(invoice.tax, 'F2')}}</td>
            </tr>
            <tr style="background-color: #eff6ff; font-weight: bold; font-size: 14pt;">
                <td colspan="3">TOTAL</td>
                <td style="text-align: right;">${{format(invoice.total, 'F2')}}</td>
            </tr>
        </tfoot>
    </table>

    <!-- Footer with company info (root context) -->
    <div style="margin-top: 50pt; text-align: center; font-size: 9pt; color: #666;">
        <p>Thank you for your business!</p>
        <p>{{company.name}} | {{company.phone}}</p>
    </div>
</body>
</html>
```
{% endraw %}

### Example 2: Multi-Level Organization Chart

**C# Code:**
```csharp
doc.Params["organization"] = new
{
    companyName = "Tech Corp",
    ceo = "Jane Smith",
    divisions = new[]
    {
        new
        {
            divisionName = "Engineering",
            head = "Bob Johnson",
            departments = new[]
            {
                new
                {
                    deptName = "Frontend",
                    manager = "Alice Brown",
                    employees = new[]
                    {
                        new { name = "Charlie Davis", role = "Senior Developer" },
                        new { name = "Diana Evans", role = "Developer" }
                    }
                },
                new
                {
                    deptName = "Backend",
                    manager = "Eve Foster",
                    employees = new[]
                    {
                        new { name = "Frank Green", role = "Senior Developer" },
                        new { name = "Grace Hill", role = "Developer" }
                    }
                }
            }
        },
        new
        {
            divisionName = "Sales",
            head = "Henry Irving",
            departments = new[]
            {
                new
                {
                    deptName = "Enterprise",
                    manager = "Iris Jones",
                    employees = new[]
                    {
                        new { name = "Jack King", role = "Account Executive" },
                        new { name = "Karen Lee", role = "Sales Rep" }
                    }
                }
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
    <title>Organization Chart</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }
        h1 {
            color: #1e40af;
            text-align: center;
        }
        .ceo {
            text-align: center;
            padding: 15pt;
            background-color: #dbeafe;
            border: 2pt solid #2563eb;
            margin: 20pt 0;
            font-weight: bold;
        }
        .division {
            margin: 30pt 0;
            padding: 20pt;
            border: 2pt solid #d1d5db;
            page-break-inside: avoid;
        }
        .division-header {
            background-color: #2563eb;
            color: white;
            padding: 10pt;
            margin: -20pt -20pt 20pt -20pt;
        }
        .department {
            margin: 20pt 0 20pt 20pt;
            padding: 15pt;
            border-left: 3pt solid #3b82f6;
            background-color: #f9fafb;
        }
        .employee {
            margin: 8pt 0 8pt 20pt;
            padding: 8pt;
            background-color: white;
            border-left: 2pt solid #93c5fd;
        }
    </style>
</head>
<body>
    <!-- Root context -->
    <h1>{{organization.companyName}} - Organization Chart</h1>

    <div class="ceo">
        CEO: {{organization.ceo}}
    </div>

    <!-- Division level (1st nesting) -->
    {{#each organization.divisions}}
        <div class="division">
            <div class="division-header">
                <h2 style="margin: 0; color: white;">
                    {{this.divisionName}} Division
                </h2>
                <p style="margin: 5pt 0 0 0;">
                    Division Head: {{this.head}}
                </p>
            </div>

            <!-- Department level (2nd nesting) -->
            {{#each this.departments}}
                <div class="department">
                    <h3 style="margin-top: 0;">{{this.deptName}} Department</h3>
                    <p><strong>Manager:</strong> {{this.manager}}</p>
                    <p style="font-size: 9pt; color: #666;">
                        Reports to: {{../head}} ({{../divisionName}})
                    </p>

                    <h4>Team Members:</h4>

                    <!-- Employee level (3rd nesting) -->
                    {{#each this.employees}}
                        <div class="employee">
                            <strong>{{this.name}}</strong> - {{this.role}}<br />
                            <span style="font-size: 9pt; color: #666;">
                                {{../../deptName}} → {{../../../divisionName}} →
                                {{../../../../organization.companyName}}
                            </span>
                        </div>
                    {{/each}}
                </div>
            {{/each}}
        </div>
    {{/each}}

    <!-- Footer with company name (root context) -->
    <div style="margin-top: 40pt; text-align: center; font-size: 9pt; color: #666;">
        <p>{{organization.companyName}} Confidential</p>
    </div>
</body>
</html>
```
{% endraw %}

---

## Context Navigation Tips

### Accessing Different Levels

| Context | How to Access | Example |
|---------|--------------|---------|
| Current | `{{this.property}}` | `{{this.name}}` |
| Parent | `{{../property}}` | `{{../categoryName}}` |
| Grandparent | `{{../../property}}` | `{{../../divisionName}}` |
| Root parameter | `{{paramName}}` | `{{companyName}}` |

### When to Use Each

**Use `this`** when:
- Inside a loop accessing current item
- Clarifying which context you mean
- Working with simple array values

**Use `../`** when:
- Need parent loop's data
- Accessing data from enclosing context
- Building hierarchical displays

**Use root parameters** when:
- Need global configuration
- Accessing company/brand info
- Using shared constants

---

## Try It Yourself

### Exercise 1: Nested Product Categories

Create a template with:
- Root: Store name and tagline
- Level 1: Categories (Electronics, Clothing, etc.)
- Level 2: Subcategories (Laptops, Phones, etc.)
- Level 3: Products
- Show full path for each product (Store → Category → Subcategory → Product)

### Exercise 2: Project Hierarchy

Create a template showing:
- Root: Company name
- Level 1: Projects
- Level 2: Phases
- Level 3: Tasks
- Show which project and phase each task belongs to

### Exercise 3: Sales Territory Report

Create a template with:
- Root: Company and report date
- Level 1: Regions
- Level 2: Territories
- Level 3: Sales reps
- Level 4: Individual sales
- Calculate totals at each level using parent context

---

## Common Pitfalls

### ❌ Wrong Number of `../`

{% raw %}
```html
{{#each departments}}
    {{#each this.employees}}
        <p>{{../companyName}}</p>  <!-- Too few ../ -->
    {{/each}}
{{/each}}
```
{% endraw %}

✅ **Solution:** Count nesting levels

{% raw %}
```html
{{#each departments}}
    {{#each this.employees}}
        <p>{{../../companyName}}</p>  <!-- Correct -->
    {{/each}}
{{/each}}
```
{% endraw %}

### ❌ Forgetting `this` in Loops

{% raw %}
```html
{{#each products}}
    <p>{{name}}</p>  <!-- May not work -->
{{/each}}
```
{% endraw %}

✅ **Solution:** Use `this`

{% raw %}
```html
{{#each products}}
    <p>{{this.name}}</p>
{{/each}}
```
{% endraw %}

### ❌ Conflicting Names

```csharp
doc.Params["name"] = "Company Name";
doc.Params["products"] = new[]
{
    new { name = "Product A" }  // Conflicts with root "name"
};
```

✅ **Solution:** Use clear naming

```csharp
doc.Params["companyName"] = "Company Name";
doc.Params["products"] = new[]
{
    new { productName = "Product A" }
};
```

---

## Next Steps

Now that you master context and scope:

1. **[Formatting Output](07_formatting_output.md)** - Format dates, numbers, and text
2. **[Advanced Patterns](08_advanced_patterns.md)** - Complex data binding scenarios
3. **[Practical Applications](/learning/08-practical/)** - Real-world examples

---

**Continue learning →** [Formatting Output](07_formatting_output.md)
