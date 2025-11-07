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
                        {{format(this.revenue / ../metrics.totalSales, 'P0')}}
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

### Example 3: Data-Driven SVG Bar Chart

Create a dynamic bar chart showing sales revenue by category with SVG.

**C# Code:**
```csharp
// Calculate sales data
var categories = new[]
{
    new { name = "Electronics", revenue = 125000m, target = 100000m },
    new { name = "Clothing", revenue = 98000m, target = 110000m },
    new { name = "Home & Garden", revenue = 87000m, target = 80000m },
    new { name = "Sports", revenue = 76000m, target = 85000m },
    new { name = "Books", revenue = 54000m, target = 60000m }
};

// Find max for scaling (needed for template calculations)
var maxRevenue = categories.Max(c => c.revenue);
var maxValue = Math.Max(maxRevenue, categories.Max(c => c.target));

doc.Params["model"] = new
{
    reportTitle = "Q4 Sales Performance by Category",
    reportDate = DateTime.Now,
    totalRevenue = categories.Sum(c => c.revenue),
    categories = categories.Select(c => new
    {
        name = c.name,
        revenue = c.revenue,
        target = c.target,
        metTarget = c.revenue >= c.target,
        percentOfTarget = (c.revenue / c.target) * 100
        // Heights will be calculated in the template
    }).ToArray(),
    chartHeight = 250,
    chartWidth = 550,
    maxValue = maxValue  // Used for scaling calculations in template
};
```

**Template:**
{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Sales Bar Chart</title>
    <style>
        body {
            font-family: Helvetica, sans-serif;
            margin: 40pt;
        }
        h1 {
            color: #1e40af;
            margin-bottom: 5pt;
        }
        .subtitle {
            color: #666;
            font-size: 10pt;
            margin-bottom: 30pt;
        }
        .chart-container {
            margin: 30pt 0;
            text-align: center;
        }
        .summary-box {
            display: inline-block;
            width: 30%;
            padding: 15pt;
            margin: 10pt 1%;
            background-color: #eff6ff;
            border: 2pt solid #2563eb;
            border-radius: 5pt;
            text-align: center;
        }
        .summary-value {
            font-size: 20pt;
            font-weight: bold;
            color: #2563eb;
        }
        .legend {
            margin: 20pt 0;
            font-size: 9pt;
        }
        .legend-item {
            display: inline-block;
            margin-right: 20pt;
        }
        .legend-color {
            display: inline-block;
            width: 15pt;
            height: 15pt;
            margin-right: 5pt;
            vertical-align: middle;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 30pt;
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
        .status-met {
            color: #059669;
            font-weight: bold;
        }
        .status-missed {
            color: #dc2626;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <h1>{{model.reportTitle}}</h1>
    <p class="subtitle">Generated: {{format(model.reportDate, 'MMMM dd, yyyy')}}</p>

    <!-- Summary Metrics -->
    <div style="margin-bottom: 30pt;">
        <div class="summary-box">
            <div class="summary-value">{{format(model.totalRevenue, 'C0')}}</div>
            <div>Total Revenue</div>
        </div>
        <div class="summary-box">
            <div class="summary-value">{{model.categories.length}}</div>
            <div>Categories</div>
        </div>
        <div class="summary-box">
            <div class="summary-value">
                {{#each model.categories}}
                    {{#if this.metTarget}}1{{/if}}
                {{/each}}/{{model.categories.length}}
            </div>
            <div>Targets Met</div>
        </div>
    </div>

    <!-- SVG Bar Chart -->
    <div class="chart-container">
        <svg width="{{model.chartWidth}}pt" height="{{model.chartHeight}}pt"
             xmlns="http://www.w3.org/2000/svg">

            <!-- Chart Title -->
            <text x="{{model.chartWidth / 2}}" y="20"
                  text-anchor="middle"
                  font-size="12"
                  font-weight="bold"
                  fill="#1e40af">
                Sales Revenue by Category
            </text>

            <!-- Y-axis labels -->
            <text x="35" y="50" text-anchor="end" font-size="9" fill="#666">
                ${{format(model.maxValue, '#,##0')}}
            </text>
            <text x="35" y="150" text-anchor="end" font-size="9" fill="#666">
                ${{format(model.maxValue / 2, '#,##0')}}
            </text>
            <text x="35" y="245" text-anchor="end" font-size="9" fill="#666">
                $0
            </text>

            <!-- Y-axis line -->
            <line x1="40" y1="40" x2="40" y2="240"
                  stroke="#d1d5db" stroke-width="2" />

            <!-- X-axis line -->
            <line x1="40" y1="240" x2="540" y2="240"
                  stroke="#d1d5db" stroke-width="2" />

            <!-- Grid lines -->
            <line x1="40" y1="140" x2="540" y2="140"
                  stroke="#e5e7eb" stroke-width="1" stroke-dasharray="5,5" />
            <line x1="40" y1="40" x2="540" y2="40"
                  stroke="#e5e7eb" stroke-width="1" stroke-dasharray="5,5" />

            <!-- Bars and Labels -->
            {{#each model.categories}}
                <!-- Calculate bar heights using template variables (0-200pt scale) -->
                <var data-id="barHeight" data-value="{{this.revenue / ../maxValue * 200}}" />
                <var data-id="targetLineHeight" data-value="{{this.target / ../maxValue * 200}}" />

                <!-- Calculate bar position (5 categories, 100pt width each) -->
                <g>
                    <!-- Bar background -->
                    <rect x="{{50 + @index * 100}}"
                          y="40"
                          width="70"
                          height="200"
                          fill="#f9fafb"
                          stroke="#e5e7eb" />

                    <!-- Revenue bar -->
                    <rect x="{{50 + @index * 100}}"
                          y="{{240 - barHeight}}"
                          width="70"
                          height="{{barHeight}}"
                          fill="{{if(this.metTarget, '#10b981', '#ef4444')}}"
                          opacity="0.8" />

                    <!-- Target line -->
                    <line x1="{{50 + @index * 100}}"
                          y1="{{240 - targetLineHeight}}"
                          x2="{{120 + @index * 100}}"
                          y2="{{240 - targetLineHeight}}"
                          stroke="#2563eb"
                          stroke-width="2"
                          stroke-dasharray="3,3" />

                    <!-- Revenue value on top of bar (only show if bar is tall enough) -->
                    {{#if barHeight > 30}}
                    <text x="{{85 + @index * 100}}"
                          y="{{235 - barHeight}}"
                          text-anchor="middle"
                          font-size="8"
                          font-weight="bold"
                          fill="white">
                        {{format(this.revenue, 'C0')}}
                    </text>
                    {{/if}}

                    <!-- Category name -->
                    <text x="{{85 + @index * 100}}"
                          y="255"
                          text-anchor="middle"
                          font-size="9"
                          fill="#374151">
                        {{this.name}}
                    </text>

                    <!-- Performance indicator -->
                    {{#if this.metTarget}}
                        <text x="{{85 + @index * 100}}"
                              y="268"
                              text-anchor="middle"
                              font-size="8"
                              fill="#059669"
                              font-weight="bold">
                            ✓ {{format(this.percentOfTarget, '0')}}%
                        </text>
                    {{else}}
                        <text x="{{85 + @index * 100}}"
                              y="268"
                              text-anchor="middle"
                              font-size="8"
                              fill="#dc2626"
                              font-weight="bold">
                            {{format(this.percentOfTarget, '0')}}%
                        </text>
                    {{/if}}
                </g>
            {{/each}}

            <!-- Legend -->
            <g transform="translate(40, 290)">
                <rect x="0" y="0" width="15" height="15" fill="#10b981" opacity="0.8" />
                <text x="20" y="12" font-size="9" fill="#374151">Revenue (Met Target)</text>

                <rect x="150" y="0" width="15" height="15" fill="#ef4444" opacity="0.8" />
                <text x="170" y="12" font-size="9" fill="#374151">Revenue (Missed Target)</text>

                <line x1="300" y1="7.5" x2="315" y2="7.5"
                      stroke="#2563eb" stroke-width="2" stroke-dasharray="3,3" />
                <text x="320" y="12" font-size="9" fill="#374151">Target</text>
            </g>
        </svg>
    </div>

    <!-- Detailed Data Table -->
    <h2>Detailed Breakdown</h2>
    <table>
        <thead>
            <tr>
                <th>Category</th>
                <th style="text-align: right;">Revenue</th>
                <th style="text-align: right;">Target</th>
                <th style="text-align: right;">% of Target</th>
                <th style="text-align: center;">Status</th>
            </tr>
        </thead>
        <tbody>
            {{#each model.categories}}
            <tr>
                <td><strong>{{this.name}}</strong></td>
                <td style="text-align: right;">{{format(this.revenue, 'C0')}}</td>
                <td style="text-align: right;">{{format(this.target, 'C0')}}</td>
                <td style="text-align: right;">{{format(this.percentOfTarget, '0.0')}}%</td>
                <td style="text-align: center;">
                    {{#if this.metTarget}}
                        <span class="status-met">✓ Met</span>
                    {{else}}
                        <span class="status-missed">✗ Missed</span>
                    {{/if}}
                </td>
            </tr>
            {{/each}}
        </tbody>
        <tfoot>
            <tr style="background-color: #eff6ff; font-weight: bold;">
                <td>TOTAL</td>
                <td style="text-align: right;">{{format(model.totalRevenue, 'C0')}}</td>
                <td colspan="3"></td>
            </tr>
        </tfoot>
    </table>

    <!-- Conditional Insights -->
    <h2>Key Insights</h2>
    {{#each model.categories}}
        {{#if this.metTarget}}
            {{#if this.percentOfTarget >= 120}}
                <div style="padding: 10pt; margin: 10pt 0; background-color: #d1fae5; border-left: 4pt solid #059669;">
                    <strong>{{this.name}}</strong> exceeded expectations by {{format(this.percentOfTarget - 100, '0')}}%!
                    Outstanding performance.
                </div>
            {{/if}}
        {{else}}
            {{#if this.percentOfTarget < 80}}
                <div style="padding: 10pt; margin: 10pt 0; background-color: #fee2e2; border-left: 4pt solid #dc2626;">
                    <strong>{{this.name}}</strong> significantly underperformed at {{format(this.percentOfTarget, '0')}}% of target.
                    Immediate attention required.
                </div>
            {{else}}
                <div style="padding: 10pt; margin: 10pt 0; background-color: #fef3c7; border-left: 4pt solid #f59e0b;">
                    <strong>{{this.name}}</strong> missed target by {{format(100 - this.percentOfTarget, '0')}}%.
                    Monitor closely next quarter.
                </div>
            {{/if}}
        {{/if}}
    {{/each}}
</body>
</html>
```
{% endraw %}

**Key Features:**

1. **Dynamic Bar Heights** - Bars scale based on actual revenue data, calculated in template
2. **Conditional Coloring** - Green bars for met targets, red for missed
3. **Target Lines** - Dashed lines show target values for comparison
4. **Data Labels** - Revenue amounts appear on bars (if space allows)
5. **Performance Indicators** - Checkmarks and percentages below each bar
6. **Detailed Table** - Complete data breakdown below the chart
7. **Conditional Insights** - Different messages based on performance levels
8. **Template Variables** - Uses `<var>` to store calculated heights for reuse

**How It Works:**

- **Template Calculations**: Each category's bar height is calculated in the template using `<var>` elements:
  ```
  <var data-id="barHeight" data-value="{{this.revenue / ../maxValue * 200}}" />
  ```
- **Variable Storage**: Calculated values are stored and accessed via `Document.Params.barHeight`
- **Conditional Coloring**: `{{#if}}` blocks determine bar colors: `{{if(this.metTarget, '#10b981', '#ef4444')}}`
- **Mathematical Expressions**: Standard math notation positions bars: `{{50 + @index * 100}}`
- **Conditional Labels**: Labels only appear when bars are tall enough: `{{#if Document.Params.barHeight > 30}}`
- **Nested Conditionals**: Insights section uses multiple conditional levels for performance warnings

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
