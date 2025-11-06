---
layout: default
title: Practical Applications
nav_order: 8
parent: Learning Guides
parent_url: /learning/
has_children: true
has_toc: false
---

# Practical Applications

Learn through complete, real-world examples - from invoices to reports, certificates to catalogs.

---

## Table of Contents

1. [Invoice Template](01_invoice_template.md) - Professional invoices with calculations
2. [Business Letter](02_business_letter.md) - Letterhead and multi-page correspondence
3. [Report Template](03_report_template.md) - Multi-section reports with charts
4. [Certificate Template](04_certificate_template.md) - Awards and certificates
5. [Data-Driven Report](05_data_driven_report.md) - Dynamic content from APIs/databases
6. [Catalog & Brochure](06_catalog_brochure.md) - Product catalogs with images
7. [Form Template](07_form_template.md) - Print-and-fill forms
8. [Multi-Language & Branded Documents](08_multi_language_branded.md) - Enterprise templates

---

## Overview

The best way to learn is by example. This series presents complete, production-ready PDF templates that you can adapt for your own projects. Each article walks through a complete document type with full code, styling, and data binding.

## What You'll Build

This series includes complete examples for common document types:

- **Invoices** - Professional billing documents with calculations
- **Business Letters** - Letterhead and multi-page correspondence
- **Reports** - Multi-section reports with charts and tables
- **Certificates** - Awards and achievement certificates
- **Data-Driven Reports** - Dynamic content from databases/APIs
- **Catalogs** - Product listings with images and descriptions
- **Forms** - Print-and-fill forms with instructions
- **Multi-Language Documents** - Localized, branded templates

## Learning Approach

Each article provides:

- **Complete Working Code** - Copy and adapt for your needs
- **Step-by-Step Breakdown** - Understand how it works
- **Styling Explained** - Professional visual design
- **Data Binding Examples** - Real-world data structures
- **Common Variations** - Adapt to your requirements
- **Tips & Tricks** - Professional touches

## Quick Preview

### Invoice Example Structure

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        /* Professional invoice styling */
    </style>
</head>
<body>
    <!-- Company header with logo -->
    <header>
        <img src="{{company.logo}}" />
        <h1>Invoice</h1>
    </header>

    <!-- Invoice details -->
    <div class="invoice-info">
        <p>Invoice #: {{invoice.number}}</p>
        <p>Date: {{invoice.date}}</p>
    </div>

    <!-- Customer details -->
    <div class="customer-info">
        <h3>Bill To:</h3>
        <p>{{customer.name}}</p>
        <p>{{customer.address}}</p>
    </div>

    <!-- Line items table -->
    <table>
        <thead>
            <tr>
                <th>Description</th>
                <th>Quantity</th>
                <th>Price</th>
                <th>Amount</th>
            </tr>
        </thead>
        <tbody>
            {{#each invoice.items}}
            <tr>
                <td>{{this.description}}</td>
                <td>{{this.quantity}}</td>
                <td>${{this.price}}</td>
                <td>${{calc(this.quantity, '*', this.price)}}</td>
            </tr>
            {{/each}}
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3">Subtotal</td>
                <td>${{invoice.subtotal}}</td>
            </tr>
            <tr>
                <td colspan="3">Tax ({{invoice.taxRate}}%)</td>
                <td>${{invoice.tax}}</td>
            </tr>
            <tr class="total-row">
                <td colspan="3">Total</td>
                <td>${{invoice.total}}</td>
            </tr>
        </tfoot>
    </table>

    <!-- Payment terms -->
    <footer>
        <p>{{invoice.paymentTerms}}</p>
    </footer>
</body>
</html>
```
{% endraw %}

## What You'll Learn

This series includes 9 complete practical examples:

### 1. [Invoice Template](01_invoice_template.md)
Complete professional invoice with:
- Company header and branding
- Line items with data binding
- Automatic calculations (subtotals, tax, total)
- Payment terms and footer
- Multiple layout variations

### 2. [Business Letter](02_business_letter.md)
Professional correspondence with:
- Letterhead design
- Address blocks
- Letter body formatting
- Signature blocks
- Multi-page handling
- Date and reference numbers

### 3. [Report Template](03_report_template.md)
Multi-section business report with:
- Cover page with branding
- Table of contents
- Sections and chapters
- Charts and graphs (SVG)
- Executive summary
- Page numbering and headers

### 4. [Certificate Template](04_certificate_template.md)
Achievement certificates with:
- Decorative borders and backgrounds
- Dynamic names and dates
- Signatures and seals
- Landscape orientation
- Elegant typography

### 5. [Data-Driven Report](05_data_driven_report.md)
Dynamic reports from databases/APIs:
- Loading data from external sources
- Dynamic tables with calculations
- Conditional sections
- Charts and visualizations
- Summary statistics and KPIs
- Export options

### 6. [Catalog & Brochure](06_catalog_brochure.md)
Product catalogs with:
- Product listings with images
- Grid layouts
- Product descriptions and pricing
- Category pages
- Multi-column design
- Professional layout

### 7. [Form Template](07_form_template.md)
Print-and-fill forms:
- Form structure and layout
- Form fields and labels
- Checkboxes and instructions
- Professional form design
- Validation instructions

### 8. [Multi-Language & Branded Documents](08_multi_language_branded.md)
Enterprise document templates:
- Multi-language document patterns
- Localization and i18n
- Brand guidelines implementation
- Color schemes and typography
- Reusable branded components
- Template management
- Automation workflows

## Prerequisites

Before starting this series:

- **Complete [Getting Started](/learning/01-getting-started/)** - Basic Scryber knowledge
- **Review [Data Binding](/learning/02-data-binding/)** - Dynamic content
- **Review [Styling](/learning/03-styling/)** - CSS fundamentals

Helpful but not required:
- [Layout & Positioning](/learning/04-layout/) - Page structure
- [Typography & Fonts](/learning/05-typography/) - Font usage
- [Content Components](/learning/06-content/) - Images, tables, SVG

## How to Use This Series

### For Beginners

1. **Start with Invoice** - Simplest complete example
2. **Try Business Letter** - Learn multi-page documents
3. **Build a Report** - Combine multiple concepts
4. **Explore Other Examples** - Find what matches your needs

### For Experienced Developers

1. **Jump to Your Use Case** - Find the closest match
2. **Copy and Adapt** - Use as a starting template
3. **Review Techniques** - Learn specific patterns
4. **Combine Examples** - Mix and match components

### As a Reference

- **Look Up Patterns** - How to implement specific features
- **Check Data Structures** - Example data binding
- **Review Styling** - Professional design patterns
- **Copy Code Snippets** - Reusable components

## Common Patterns Across Examples

### Professional Headers

{% raw %}
```html
<header style="border-bottom: 2pt solid #2563eb; padding-bottom: 15pt;">
    <div style="display: flex; justify-content: space-between;">
        <img src="{{company.logo}}" style="height: 50pt;" />
        <div style="text-align: right;">
            <h1 style="margin: 0; color: #2563eb;">{{documentType}}</h1>
            <p style="margin: 5pt 0;">{{documentNumber}}</p>
        </div>
    </div>
</header>
```
{% endraw %}

### Styled Tables

```css
table {
    width: 100%;
    border-collapse: collapse;
    margin: 20pt 0;
}

thead {
    background-color: #2563eb;
    color: white;
}

th {
    padding: 10pt;
    text-align: left;
    font-weight: bold;
}

td {
    padding: 8pt;
    border-bottom: 1pt solid #e5e7eb;
}

tbody tr:nth-child(even) {
    background-color: #f9fafb;
}

tfoot {
    background-color: #eff6ff;
    font-weight: bold;
}
```

### Dynamic Calculations

{% raw %}
```html
<!-- Store subtotal -->
<var data-id="subtotal" data-value="0" />

<!-- Calculate line items -->
{{#each items}}
<tr>
    <td>{{this.description}}</td>
    <td>${{this.amount}}</td>
    <var data-id="subtotal"
         data-value="{{calc(Document.Params.subtotal, '+', this.amount)}}" />
</tr>
{{/each}}

<!-- Calculate tax and total -->
<var data-id="tax"
     data-value="{{calc(Document.Params.subtotal, '*', taxRate)}}" />
<var data-id="total"
     data-value="{{calc(Document.Params.subtotal, '+', Document.Params.tax)}}" />

<!-- Display totals -->
<tr>
    <td>Tax ({{taxRate}}%)</td>
    <td>${{Document.Params.tax}}</td>
</tr>
<tr>
    <td>Total</td>
    <td>${{Document.Params.total}}</td>
</tr>
```
{% endraw %}

### Conditional Content

{% raw %}
```html
{{#if customer.isPremium}}
<div class="premium-badge">
    <p>Valued Premium Customer</p>
</div>
{{/if}}

{{#if invoice.pastDue}}
<div class="warning-box">
    <p>Payment is past due. Please remit immediately.</p>
</div>
{{/if}}
```
{% endraw %}

### Page Numbers

```html
<footer style="text-align: center; margin-top: 30pt;">
    <p style="font-size: 9pt; color: #666;">
        Page <page-number /> of <page-count /> | {{company.name}} | {{company.phone}}
    </p>
</footer>
```

## Data Structure Examples

### Invoice Data

```json
{
    "company": {
        "name": "Acme Corporation",
        "logo": "./images/acme-logo.png",
        "address": "123 Main St, City, ST 12345",
        "phone": "(555) 123-4567",
        "email": "billing@acme.com"
    },
    "invoice": {
        "number": "INV-2025-001",
        "date": "2025-10-17",
        "dueDate": "2025-11-17",
        "subtotal": 1000.00,
        "taxRate": 0.08,
        "tax": 80.00,
        "total": 1080.00,
        "items": [
            {
                "description": "Professional Services",
                "quantity": 10,
                "price": 100.00
            }
        ],
        "paymentTerms": "Net 30 days"
    },
    "customer": {
        "name": "John Doe",
        "company": "Doe Industries",
        "address": "456 Oak Ave, Town, ST 67890",
        "isPremium": true
    }
}
```

### Report Data

```json
{
    "report": {
        "title": "Q4 Sales Report",
        "date": "2025-10-17",
        "author": "Sales Team",
        "sections": [
            {
                "title": "Executive Summary",
                "content": "..."
            },
            {
                "title": "Detailed Analysis",
                "content": "..."
            }
        ],
        "salesData": [
            {"region": "North", "sales": 125000, "growth": 12.5},
            {"region": "South", "sales": 98000, "growth": 8.2}
        ]
    }
}
```

## Styling Tips

### Professional Color Schemes

```css
/* Corporate Blue */
:root {
    --primary: #2563eb;
    --secondary: #1e40af;
    --accent: #3b82f6;
    --light: #eff6ff;
    --dark: #1e3a8a;
}

/* Modern Green */
:root {
    --primary: #10b981;
    --secondary: #059669;
    --accent: #34d399;
    --light: #d1fae5;
    --dark: #065f46;
}
```

### Typography Hierarchy

```css
h1 {
    font-size: 24pt;
    font-weight: bold;
    color: var(--primary);
    margin-bottom: 20pt;
}

h2 {
    font-size: 18pt;
    font-weight: bold;
    color: var(--secondary);
    margin: 15pt 0 10pt 0;
}

h3 {
    font-size: 14pt;
    font-weight: 600;
    color: var(--dark);
    margin: 10pt 0 8pt 0;
}

body {
    font-size: 11pt;
    line-height: 1.6;
    color: #333;
}
```

## Next Steps

Ready to build real documents? Start with the example that best matches your needs:

- **New to Scryber?** → Start with [Invoice Template](01_invoice_template.md)
- **Need correspondence?** → [Business Letter](02_business_letter.md)
- **Building reports?** → [Report Template](03_report_template.md)
- **Dynamic content?** → [Data-Driven Report](05_data_driven_report.md)
- **Product listings?** → [Catalog & Brochure](06_catalog_brochure.md)

## Tips for Adapting Examples

1. **Start with Complete Example** - Get it working first
2. **Modify Data Structure** - Adapt to your data model
3. **Adjust Styling** - Match your brand
4. **Add/Remove Sections** - Customize to your needs
5. **Test with Real Data** - Use actual data early
6. **Iterate and Refine** - Improve based on feedback

## Additional Resources

- **[HTML Element Reference](/reference/htmltags/)** - All supported elements
- **[CSS Property Reference](/reference/css/)** - All supported properties
- **[Data Binding Reference](/reference/data-binding/)** - Expression syntax
- **[Code Examples Repository](/examples/)** - More examples

---

**Related Series:**
- [Data Binding](/learning/02-data-binding/) - Dynamic content
- [Styling & Appearance](/learning/03-styling/) - Design fundamentals
- [Content Components](/learning/06-content/) - Tables, images, SVG

---

**Start building →** [Invoice Template](01_invoice_template.md)
