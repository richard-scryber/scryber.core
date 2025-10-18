---
layout: default
title: Conditional Rendering
nav_order: 4
parent: Data Binding & Expressions
parent_url: /learning/02-data-binding/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Conditional Rendering

Control what content appears in your PDFs based on data values and business logic.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Use {{#if}} blocks to conditionally show content
- Chain conditions with {{else}} and {{else if}}
- Apply comparison and logical operators
- Use show-if and hide-if attributes
- Build complex conditional logic
- Create dynamic sections based on data

---

## The {{#if}} Block

Show content only when a condition is true:

{% raw %}
```html
{{#if condition}}
    <!-- Content shown when true -->
{{/if}}
```
{% endraw %}

---

## Basic Conditionals

### Boolean Conditions

{% raw %}
```html
{{#if model.isPremium}}
    <div class="premium-badge">Premium Customer</div>
{{/if}}
```
{% endraw %}

```csharp
doc.Params["model"] = new { isPremium = true };
```

### Null/Empty Checks

{% raw %}
```html
{{#if model.notes}}
    <div class="notes">
        <h3>Notes</h3>
        <p>{{model.notes}}</p>
    </div>
{{/if}}
```
{% endraw %}

---

## {{else}} Clause

Provide alternative content when condition is false:

{% raw %}
```html
{{#if model.hasDiscount}}
    <p class="discount">Special discount applied!</p>
{{else}}
    <p>No discounts available.</p>
{{/if}}
```
{% endraw %}

---

## {{else if}} Chains

Test multiple conditions:

{% raw %}
```html
{{#if model.priority == 0}}
    <span class="badge low">Low Priority</span>
{{else if model.priority == 1}}
    <span class="badge medium">Medium Priority</span>
{{else if model.priority == 2}}
    <span class="badge high">High Priority</span>
{{else}}
    <span class="badge critical">Critical</span>
{{/if}}
```
{% endraw %}

---

## Comparison Operators

### Equality and Inequality

{% raw %}
```html
<!-- Equal to -->
{{#if model.status == 'active'}}
    <p>Status: Active</p>
{{/if}}

<!-- Not equal to -->
{{#if model.status != 'cancelled'}}
    <p>This order is valid.</p>
{{/if}}
```
{% endraw %}

### Numeric Comparisons

{% raw %}
```html
<!-- Greater than -->
{{#if model.age > 18}}
    <p>Adult</p>
{{/if}}

<!-- Greater than or equal -->
{{#if model.score >= 70}}
    <p class="pass">Passed</p>
{{/if}}

<!-- Less than -->
{{#if model.temperature < 32}}
    <p>Freezing conditions</p>
{{/if}}

<!-- Less than or equal -->
{{#if model.stock <= 10}}
    <p class="warning">Low stock!</p>
{{/if}}
```
{% endraw %}

---

## Logical Operators

### AND (&&)

Both conditions must be true:

{% raw %}
```html
{{#if model.age >= 18 && model.hasLicense}}
    <p>Eligible to drive</p>
{{else}}
    <p>Not eligible to drive</p>
{{/if}}
```
{% endraw %}

### OR (||)

At least one condition must be true:

{% raw %}
```html
{{#if model.isAdmin || model.isModerator}}
    <div class="admin-panel">
        <h2>Administration</h2>
        <!-- Admin controls -->
    </div>
{{/if}}
```
{% endraw %}

### NOT (!)

Negate a condition:

{% raw %}
```html
{{#if !model.isBlocked}}
    <div class="user-content">
        <!-- User can see this -->
    </div>
{{else}}
    <p>Your account has been blocked.</p>
{{/if}}
```
{% endraw %}

### Complex Logic

{% raw %}
```html
{{#if (model.isPremium || model.isVIP) && !model.isSuspended}}
    <div class="premium-features">
        <h2>Premium Features</h2>
        <p>Thank you for being a valued member!</p>
    </div>
{{/if}}
```
{% endraw %}

---

## Inline Conditionals with if()

Use the `if()` function for inline conditional values:

{% raw %}
```html
<!-- Simple inline conditional -->
<p class="{{if(model.isPremium, 'premium', 'standard')}}">
    Customer Type: {{if(model.isPremium, 'Premium', 'Standard')}}
</p>

<!-- Numeric conditional -->
<p style="color: {{if(model.balance >= 0, '#059669', '#dc2626')}};">
    Balance: ${{model.balance}}
</p>

<!-- Nested conditionals -->
<div class="status-{{if(model.status == 'active', 'active',
                     if(model.status == 'pending', 'pending', 'inactive'))}}">
    Status: {{model.status}}
</div>
```
{% endraw %}

---

## Conditional Sections

### Show Different Layouts

{% raw %}
```html
{{#if model.layoutType == 'summary'}}
    <div class="summary-layout">
        <h1>{{model.title}}</h1>
        <p>{{model.summary}}</p>
    </div>
{{else if model.layoutType == 'detailed'}}
    <div class="detailed-layout">
        <h1>{{model.title}}</h1>
        <p>{{model.summary}}</p>
        <div class="details">
            <p>{{model.fullContent}}</p>
        </div>
    </div>
{{else}}
    <div class="compact-layout">
        <h2>{{model.title}}</h2>
    </div>
{{/if}}
```
{% endraw %}

### Conditional Headers and Footers

{% raw %}
```html
{{#if model.includeHeader}}
    <div style="border-bottom: 2pt solid #2563eb; padding-bottom: 15pt; margin-bottom: 30pt;">
        <img src="{{model.logoUrl}}" style="height: 50pt;" />
        <h1>{{model.companyName}}</h1>
    </div>
{{/if}}

<!-- Main content -->
<div>{{model.content}}</div>

{{#if model.includeFooter}}
    <div style="border-top: 1pt solid #d1d5db; padding-top: 15pt; margin-top: 30pt;">
        <p style="text-align: center; font-size: 9pt; color: #666;">
            {{model.footerText}}
        </p>
    </div>
{{/if}}
```
{% endraw %}

---

## Combining Conditionals with Iteration

### Filter Items in Loops

{% raw %}
```html
<h2>Active Products</h2>
{{#each products}}
    {{#if this.isActive}}
        <div class="product">
            <h3>{{this.name}}</h3>
            <p>Price: ${{this.price}}</p>
        </div>
    {{/if}}
{{/each}}
```
{% endraw %}

### Show/Hide Sections Based on Data

{% raw %}
```html
{{#if model.orders.length > 0}}
    <h2>Recent Orders</h2>
    <table>
        <thead>
            <tr>
                <th>Order #</th>
                <th>Date</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>
            {{#each model.orders}}
            <tr>
                <td>{{this.orderNumber}}</td>
                <td>{{format(this.date, 'yyyy-MM-dd')}}</td>
                <td>${{format(this.total, 'F2')}}</td>
            </tr>
            {{/each}}
        </tbody>
    </table>
{{else}}
    <p>No orders found.</p>
{{/if}}
```
{% endraw %}

---

## Practical Examples

### Example 1: Dynamic Invoice with Payment Status

**C# Code:**
```csharp
doc.Params["model"] = new
{
    invoiceNumber = "INV-2025-001",
    customerName = "Acme Corporation",
    date = DateTime.Now,
    dueDate = DateTime.Now.AddDays(30),
    isPaid = false,
    isOverdue = false,
    items = new[]
    {
        new { description = "Consulting", hours = 10, rate = 150.00, total = 1500.00 },
        new { description = "Development", hours = 20, rate = 200.00, total = 4000.00 }
    },
    subtotal = 5500.00,
    discount = 0.0,
    hasDiscount = false,
    tax = 440.00,
    total = 5940.00,
    notes = "Payment due within 30 days."
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
        h1 {
            color: #1e40af;
        }
        .status-banner {
            padding: 15pt;
            margin-bottom: 20pt;
            border-radius: 5pt;
            font-weight: bold;
        }
        .paid {
            background-color: #d1fae5;
            color: #065f46;
            border: 2pt solid #059669;
        }
        .overdue {
            background-color: #fee2e2;
            color: #991b1b;
            border: 2pt solid #dc2626;
        }
        .pending {
            background-color: #fef3c7;
            color: #92400e;
            border: 2pt solid #f59e0b;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
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
        .discount-row {
            color: #059669;
            font-weight: bold;
        }
        .notes {
            background-color: #eff6ff;
            border-left: 4pt solid #2563eb;
            padding: 15pt;
            margin-top: 20pt;
        }
    </style>
</head>
<body>
    <h1>INVOICE #{{model.invoiceNumber}}</h1>

    <!-- Status Banner -->
    {{#if model.isPaid}}
        <div class="status-banner paid">
            ✓ PAID - Thank you for your payment!
        </div>
    {{else if model.isOverdue}}
        <div class="status-banner overdue">
            ⚠ OVERDUE - Payment is past due. Please remit immediately.
        </div>
    {{else}}
        <div class="status-banner pending">
            ⏳ PENDING - Payment due by {{format(model.dueDate, 'MMMM dd, yyyy')}}
        </div>
    {{/if}}

    <!-- Invoice Details -->
    <p><strong>Customer:</strong> {{model.customerName}}</p>
    <p><strong>Invoice Date:</strong> {{format(model.date, 'MMMM dd, yyyy')}}</p>
    <p><strong>Due Date:</strong> {{format(model.dueDate, 'MMMM dd, yyyy')}}</p>

    <!-- Line Items -->
    <table>
        <thead>
            <tr>
                <th>Description</th>
                <th>Hours</th>
                <th>Rate</th>
                <th style="text-align: right;">Total</th>
            </tr>
        </thead>
        <tbody>
            {{#each model.items}}
            <tr>
                <td>{{this.description}}</td>
                <td>{{this.hours}}</td>
                <td>${{format(this.rate, 'F2')}}</td>
                <td style="text-align: right;">${{format(this.total, 'F2')}}</td>
            </tr>
            {{/each}}
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3"><strong>Subtotal</strong></td>
                <td style="text-align: right;">${{format(model.subtotal, 'F2')}}</td>
            </tr>
            {{#if model.hasDiscount}}
            <tr class="discount-row">
                <td colspan="3">Discount</td>
                <td style="text-align: right;">-${{format(model.discount, 'F2')}}</td>
            </tr>
            {{/if}}
            <tr>
                <td colspan="3">Tax (8%)</td>
                <td style="text-align: right;">${{format(model.tax, 'F2')}}</td>
            </tr>
            <tr style="background-color: #eff6ff; font-weight: bold; font-size: 14pt;">
                <td colspan="3">TOTAL</td>
                <td style="text-align: right;">${{format(model.total, 'F2')}}</td>
            </tr>
        </tfoot>
    </table>

    <!-- Conditional Notes -->
    {{#if model.notes}}
        <div class="notes">
            <strong>Notes:</strong><br />
            {{model.notes}}
        </div>
    {{/if}}

    <!-- Payment Instructions (only if not paid) -->
    {{#if !model.isPaid}}
        <div style="margin-top: 30pt; padding: 15pt; background-color: #f9fafb; border: 1pt solid #d1d5db;">
            <h3>Payment Instructions</h3>
            <p>Please make payment to:</p>
            <p>
                <strong>Bank:</strong> First National Bank<br />
                <strong>Account:</strong> 123-456-7890<br />
                <strong>Reference:</strong> {{model.invoiceNumber}}
            </p>
        </div>
    {{/if}}
</body>
</html>
```
{% endraw %}

### Example 2: Personalized Report with User Levels

**C# Code:**
```csharp
doc.Params["model"] = new
{
    userName = "John Doe",
    userLevel = "premium", // or "standard", "admin"
    isAdmin = false,
    reportDate = DateTime.Now,
    metrics = new
    {
        totalSales = 125000,
        newCustomers = 45,
        conversionRate = 0.068
    },
    detailedAnalytics = new[]
    {
        new { category = "Electronics", revenue = 45000 },
        new { category = "Clothing", revenue = 35000 },
        new { category = "Home & Garden", revenue = 25000 },
        new { category = "Sports", revenue = 20000 }
    },
    alerts = new[]
    {
        new { type = "warning", message = "Sales down 5% from last month" },
        new { type = "info", message = "New products added to inventory" }
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
        .user-badge {
            display: inline-block;
            padding: 5pt 15pt;
            border-radius: 15pt;
            font-size: 10pt;
            font-weight: bold;
            margin-left: 10pt;
        }
        .badge-premium {
            background-color: #fef3c7;
            color: #92400e;
        }
        .badge-admin {
            background-color: #dbeafe;
            color: #1e40af;
        }
        .badge-standard {
            background-color: #f3f4f6;
            color: #4b5563;
        }
        .metric-box {
            display: inline-block;
            width: 30%;
            padding: 15pt;
            margin: 10pt 1%;
            border: 2pt solid #2563eb;
            border-radius: 5pt;
            text-align: center;
        }
        .metric-value {
            font-size: 24pt;
            font-weight: bold;
            color: #2563eb;
        }
        .alert {
            padding: 10pt;
            margin: 10pt 0;
            border-radius: 5pt;
        }
        .alert-warning {
            background-color: #fef3c7;
            border-left: 4pt solid #f59e0b;
        }
        .alert-info {
            background-color: #dbeafe;
            border-left: 4pt solid #2563eb;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 20pt 0;
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
    <h1>
        Sales Report for {{model.userName}}
        {{#if model.userLevel == 'premium'}}
            <span class="user-badge badge-premium">★ Premium</span>
        {{else if model.userLevel == 'admin'}}
            <span class="user-badge badge-admin">⚙ Admin</span>
        {{else}}
            <span class="user-badge badge-standard">Standard</span>
        {{/if}}
    </h1>

    <p style="color: #666; font-size: 10pt;">
        Generated: {{format(model.reportDate, 'MMMM dd, yyyy HH:mm')}}
    </p>

    <!-- Key Metrics (visible to all) -->
    <h2>Key Metrics</h2>
    <div>
        <div class="metric-box">
            <div class="metric-value">{{format(model.metrics.totalSales, 'C0')}}</div>
            <div>Total Sales</div>
        </div>
        <div class="metric-box">
            <div class="metric-value">{{model.metrics.newCustomers}}</div>
            <div>New Customers</div>
        </div>
        <div class="metric-box">
            <div class="metric-value">{{format(model.metrics.conversionRate, 'P1')}}</div>
            <div>Conversion Rate</div>
        </div>
    </div>

    <!-- Detailed Analytics (Premium and Admin only) -->
    {{#if model.userLevel == 'premium' || model.userLevel == 'admin'}}
        <h2>Detailed Category Analysis</h2>
        <table>
            <thead>
                <tr>
                    <th>Category</th>
                    <th style="text-align: right;">Revenue</th>
                    <th style="text-align: right;">% of Total</th>
                </tr>
            </thead>
            <tbody>
                {{#each model.detailedAnalytics}}
                <tr>
                    <td>{{this.category}}</td>
                    <td style="text-align: right;">{{format(this.revenue, 'C0')}}</td>
                    <td style="text-align: right;">
                        {{format(calc(this.revenue, '/', ../metrics.totalSales), 'P0')}}
                    </td>
                </tr>
                {{/each}}
            </tbody>
        </table>
    {{else}}
        <div style="padding: 20pt; background-color: #fef3c7; border: 2pt solid #f59e0b; border-radius: 5pt; margin: 20pt 0;">
            <strong>Upgrade to Premium</strong> to see detailed category analysis and more!
        </div>
    {{/if}}

    <!-- Alerts -->
    {{#if model.alerts.length > 0}}
        <h2>Alerts & Notifications</h2>
        {{#each model.alerts}}
            <div class="alert alert-{{this.type}}">
                {{#if this.type == 'warning'}}⚠{{else}}ℹ{{/if}}
                {{this.message}}
            </div>
        {{/each}}
    {{/if}}

    <!-- Admin Section -->
    {{#if model.isAdmin}}
        <div style="margin-top: 40pt; padding: 20pt; background-color: #eff6ff; border: 2pt solid #2563eb;">
            <h2>Admin Tools</h2>
            <p>Additional administrative controls and reports are available.</p>
            <ul>
                <li>User management</li>
                <li>System configuration</li>
                <li>Audit logs</li>
            </ul>
        </div>
    {{/if}}
</body>
</html>
```
{% endraw %}

---

## Try It Yourself

### Exercise 1: Grade Report

Create a template that:
- Shows student name and score
- Displays letter grade based on score (A: 90+, B: 80-89, C: 70-79, D: 60-69, F: <60)
- Shows "Pass" or "Fail" with appropriate styling
- Adds a "Honor Roll" badge if score >= 95

### Exercise 2: Order Status Page

Create a template that:
- Shows different content based on order status (pending, processing, shipped, delivered, cancelled)
- Displays tracking info only for shipped/delivered orders
- Shows estimated delivery only for pending/processing orders
- Adds return options only for delivered orders

### Exercise 3: Membership Card

Create a template that:
- Shows member name and ID
- Displays different tier badges (Bronze, Silver, Gold, Platinum)
- Lists benefits based on tier level
- Shows expiration warning if membership expires within 30 days

---

## Common Pitfalls

### ❌ Using Assignment Instead of Comparison

{% raw %}
```html
{{#if model.status = 'active'}}  <!-- Wrong: assignment -->
```
{% endraw %}

✅ **Solution:** Use `==` for comparison

{% raw %}
```html
{{#if model.status == 'active'}}  <!-- Correct -->
```
{% endraw %}

### ❌ Missing Parentheses in Complex Logic

{% raw %}
```html
{{#if model.a || model.b && model.c}}  <!-- Ambiguous -->
```
{% endraw %}

✅ **Solution:** Use parentheses

{% raw %}
```html
{{#if model.a || (model.b && model.c)}}  <!-- Clear -->
```
{% endraw %}

### ❌ Forgetting to Close {{/if}}

{% raw %}
```html
{{#if model.showSection}}
    <div>Content</div>
<!-- Missing {{/if}} -->
```
{% endraw %}

✅ **Solution:** Always close blocks

{% raw %}
```html
{{#if model.showSection}}
    <div>Content</div>
{{/if}}
```
{% endraw %}

---

## Next Steps

Now that you can control content visibility:

1. **[Variables & Params](05_variables_params.md)** - Store and reuse calculated values
2. **[Context & Scope](06_context_scope.md)** - Understand data access patterns
3. **[Formatting Output](07_formatting_output.md)** - Format dates, numbers, and text

---

**Continue learning →** [Variables & Params](05_variables_params.md)
