---
layout: default
title: Invoice Template
nav_order: 1
parent: Practical Applications
parent_url: /learning/08-practical/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# Invoice Template

Build professional, calculation-ready invoice PDFs with dynamic line items, automatic totals, and customizable branding.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Create professional invoice templates
- Implement line items with data binding
- Calculate subtotals, tax, and totals automatically
- Add company branding and styling
- Handle payment terms and notes
- Generate invoices from C# code

---

## Complete Invoice Template

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>{{invoice.number}}</title>
    <style>
        body {
            font-family: 'Helvetica', 'Arial', sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            color: #333;
            margin: 40pt;
        }

        /* Header styling */
        .invoice-header {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            border-bottom: 2pt solid #2563eb;
            padding-bottom: 15pt;
            margin-bottom: 30pt;
        }

        .company-logo {
            height: 50pt;
        }

        .invoice-title {
            text-align: right;
        }

        .invoice-title h1 {
            font-size: 28pt;
            color: #2563eb;
            margin: 0;
            font-weight: bold;
        }

        .invoice-title p {
            margin: 5pt 0;
            font-size: 12pt;
            color: #666;
        }

        /* Company and customer info */
        .info-section {
            display: flex;
            justify-content: space-between;
            margin-bottom: 30pt;
        }

        .company-info, .customer-info {
            width: 45%;
        }

        .company-info h3, .customer-info h3 {
            font-size: 12pt;
            color: #2563eb;
            margin-bottom: 8pt;
            font-weight: 600;
        }

        .company-info p, .customer-info p {
            margin: 4pt 0;
            font-size: 10pt;
            line-height: 1.4;
        }

        /* Invoice details */
        .invoice-details {
            background-color: #f9fafb;
            padding: 15pt;
            margin-bottom: 30pt;
            border: 1pt solid #e5e7eb;
            border-radius: 4pt;
        }

        .invoice-details table {
            width: 100%;
            border: none;
        }

        .invoice-details td {
            padding: 4pt 8pt;
            border: none;
        }

        .invoice-details td:first-child {
            font-weight: 600;
            color: #555;
            width: 150pt;
        }

        /* Line items table */
        .line-items {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20pt;
        }

        .line-items thead {
            background-color: #2563eb;
            color: white;
        }

        .line-items th {
            padding: 12pt;
            text-align: left;
            font-weight: bold;
            font-size: 10pt;
            text-transform: uppercase;
        }

        .line-items th.right {
            text-align: right;
        }

        .line-items td {
            padding: 10pt 12pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        .line-items td.right {
            text-align: right;
        }

        .line-items tbody tr:nth-child(even) {
            background-color: #f9fafb;
        }

        .line-items tbody tr:hover {
            background-color: #eff6ff;
        }

        /* Totals section */
        .totals-section {
            width: 300pt;
            margin-left: auto;
            margin-bottom: 30pt;
        }

        .totals-section table {
            width: 100%;
            border-collapse: collapse;
        }

        .totals-section td {
            padding: 8pt 12pt;
            border-bottom: 1pt solid #e5e7eb;
        }

        .totals-section td:first-child {
            font-weight: 600;
        }

        .totals-section td:last-child {
            text-align: right;
        }

        .total-row {
            background-color: #2563eb;
            color: white;
            font-size: 13pt;
            font-weight: bold;
        }

        .total-row td {
            border-bottom: none;
            padding: 12pt;
        }

        /* Payment terms and notes */
        .payment-terms {
            background-color: #fffbeb;
            border-left: 4pt solid #f59e0b;
            padding: 15pt;
            margin-bottom: 20pt;
        }

        .payment-terms h3 {
            margin: 0 0 8pt 0;
            font-size: 12pt;
            color: #d97706;
        }

        .payment-terms p {
            margin: 4pt 0;
            font-size: 10pt;
            line-height: 1.5;
        }

        /* Footer */
        .invoice-footer {
            text-align: center;
            margin-top: 40pt;
            padding-top: 15pt;
            border-top: 1pt solid #e5e7eb;
            font-size: 9pt;
            color: #666;
        }

        /* Notes section */
        .notes {
            margin-bottom: 20pt;
        }

        .notes h3 {
            font-size: 12pt;
            color: #555;
            margin-bottom: 8pt;
        }

        .notes p {
            font-size: 10pt;
            line-height: 1.5;
            color: #666;
        }
    </style>
</head>
<body>
    <!-- Header -->
    <div class="invoice-header">
        <div>
            <img src="{{company.logo}}" class="company-logo" alt="Company Logo" />
        </div>
        <div class="invoice-title">
            <h1>INVOICE</h1>
            <p>{{invoice.number}}</p>
        </div>
    </div>

    <!-- Company and Customer Info -->
    <div class="info-section">
        <div class="company-info">
            <h3>From</h3>
            <p><strong>{{company.name}}</strong></p>
            <p>{{company.address}}</p>
            <p>{{company.city}}, {{company.state}} {{company.zip}}</p>
            <p>Phone: {{company.phone}}</p>
            <p>Email: {{company.email}}</p>
        </div>

        <div class="customer-info">
            <h3>Bill To</h3>
            <p><strong>{{customer.name}}</strong></p>
            {{#if customer.company}}
            <p>{{customer.company}}</p>
            {{/if}}
            <p>{{customer.address}}</p>
            <p>{{customer.city}}, {{customer.state}} {{customer.zip}}</p>
            {{#if customer.email}}
            <p>Email: {{customer.email}}</p>
            {{/if}}
        </div>
    </div>

    <!-- Invoice Details -->
    <div class="invoice-details">
        <table>
            <tr>
                <td>Invoice Number:</td>
                <td>{{invoice.number}}</td>
                <td>Invoice Date:</td>
                <td>{{invoice.date}}</td>
            </tr>
            <tr>
                <td>Purchase Order:</td>
                <td>{{invoice.poNumber}}</td>
                <td>Due Date:</td>
                <td>{{invoice.dueDate}}</td>
            </tr>
        </table>
    </div>

    <!-- Line Items -->
    <table class="line-items">
        <thead>
            <tr>
                <th>Description</th>
                <th class="right">Quantity</th>
                <th class="right">Unit Price</th>
                <th class="right">Amount</th>
            </tr>
        </thead>
        <tbody>
            {{#each invoice.items}}
            <tr>
                <td>
                    <strong>{{this.description}}</strong>
                    {{#if this.details}}
                    <br /><span style="font-size: 9pt; color: #666;">{{this.details}}</span>
                    {{/if}}
                </td>
                <td class="right">{{this.quantity}}</td>
                <td class="right">${{format(this.price, '0.00')}}</td>
                <td class="right">${{format(calc(this.quantity, '*', this.price), '0.00')}}</td>
            </tr>
            {{/each}}
        </tbody>
    </table>

    <!-- Totals -->
    <div class="totals-section">
        <table>
            <tr>
                <td>Subtotal:</td>
                <td>${{format(invoice.subtotal, '0.00')}}</td>
            </tr>
            {{#if invoice.discount}}
            <tr>
                <td>Discount ({{invoice.discountPercent}}%):</td>
                <td>-${{format(invoice.discount, '0.00')}}</td>
            </tr>
            {{/if}}
            <tr>
                <td>Tax ({{format(calc(invoice.taxRate, '*', 100), '0.0')}}%):</td>
                <td>${{format(invoice.tax, '0.00')}}</td>
            </tr>
            {{#if invoice.shipping}}
            <tr>
                <td>Shipping:</td>
                <td>${{format(invoice.shipping, '0.00')}}</td>
            </tr>
            {{/if}}
            <tr class="total-row">
                <td>Total Due:</td>
                <td>${{format(invoice.total, '0.00')}}</td>
            </tr>
        </table>
    </div>

    <!-- Payment Terms -->
    <div class="payment-terms">
        <h3>Payment Terms</h3>
        <p>{{invoice.paymentTerms}}</p>
        {{#if invoice.paymentInstructions}}
        <p>{{invoice.paymentInstructions}}</p>
        {{/if}}
    </div>

    <!-- Notes -->
    {{#if invoice.notes}}
    <div class="notes">
        <h3>Notes</h3>
        <p>{{invoice.notes}}</p>
    </div>
    {{/if}}

    <!-- Footer -->
    <div class="invoice-footer">
        <p>{{company.name}} | {{company.phone}} | {{company.email}}</p>
        <p>{{company.website}}</p>
    </div>
</body>
</html>
```
{% endraw %}

---

## C# Invoice Generator

```csharp
using Scryber.Components;
using Scryber.PDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class InvoiceGenerator
{
    private readonly string _templatePath;

    public InvoiceGenerator(string templatePath)
    {
        _templatePath = templatePath;
    }

    public void GenerateInvoice(InvoiceData invoiceData, Stream output)
    {
        // Calculate totals
        CalculateTotals(invoiceData);

        // Load template
        var doc = Document.ParseDocument(_templatePath);

        // Set metadata
        doc.Info.Title = $"Invoice {invoiceData.Invoice.Number}";
        doc.Info.Author = invoiceData.Company.Name;
        doc.Info.Subject = $"Invoice for {invoiceData.Customer.Name}";
        doc.Info.CreationDate = DateTime.Now;

        // Bind data
        doc.Params["company"] = invoiceData.Company;
        doc.Params["customer"] = invoiceData.Customer;
        doc.Params["invoice"] = invoiceData.Invoice;

        // Generate PDF
        doc.SaveAsPDF(output);
    }

    private void CalculateTotals(InvoiceData invoiceData)
    {
        var invoice = invoiceData.Invoice;

        // Calculate subtotal
        invoice.Subtotal = invoice.Items.Sum(item => item.Quantity * item.Price);

        // Apply discount if any
        if (invoice.DiscountPercent > 0)
        {
            invoice.Discount = invoice.Subtotal * (invoice.DiscountPercent / 100);
        }

        // Calculate taxable amount
        var taxableAmount = invoice.Subtotal - invoice.Discount;

        // Calculate tax
        invoice.Tax = taxableAmount * invoice.TaxRate;

        // Calculate total
        invoice.Total = taxableAmount + invoice.Tax + invoice.Shipping;
    }
}

// Data models
public class InvoiceData
{
    public CompanyInfo Company { get; set; }
    public CustomerInfo Customer { get; set; }
    public Invoice Invoice { get; set; }
}

public class CompanyInfo
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
}

public class CustomerInfo
{
    public string Name { get; set; }
    public string Company { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Email { get; set; }
}

public class Invoice
{
    public string Number { get; set; }
    public string Date { get; set; }
    public string DueDate { get; set; }
    public string PoNumber { get; set; }
    public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    public decimal Subtotal { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal Discount { get; set; }
    public decimal TaxRate { get; set; }
    public decimal Tax { get; set; }
    public decimal Shipping { get; set; }
    public decimal Total { get; set; }
    public string PaymentTerms { get; set; }
    public string PaymentInstructions { get; set; }
    public string Notes { get; set; }
}

public class InvoiceItem
{
    public string Description { get; set; }
    public string Details { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```

---

## Usage Example

```csharp
// Create invoice data
var invoiceData = new InvoiceData
{
    Company = new CompanyInfo
    {
        Name = "Acme Corporation",
        Logo = "./images/acme-logo.png",
        Address = "123 Main Street",
        City = "New York",
        State = "NY",
        Zip = "10001",
        Phone = "(555) 123-4567",
        Email = "billing@acme.com",
        Website = "www.acme.com"
    },
    Customer = new CustomerInfo
    {
        Name = "John Smith",
        Company = "Smith Industries",
        Address = "456 Oak Avenue",
        City = "Boston",
        State = "MA",
        Zip = "02101",
        Email = "john@smithind.com"
    },
    Invoice = new Invoice
    {
        Number = "INV-2025-001",
        Date = "October 17, 2025",
        DueDate = "November 17, 2025",
        PoNumber = "PO-12345",
        Items = new List<InvoiceItem>
        {
            new InvoiceItem
            {
                Description = "Professional Services",
                Details = "Consulting services for Q4 2025",
                Quantity = 40,
                Price = 150.00m
            },
            new InvoiceItem
            {
                Description = "Software License",
                Details = "Annual subscription",
                Quantity = 1,
                Price = 2500.00m
            },
            new InvoiceItem
            {
                Description = "Training Session",
                Details = "On-site team training",
                Quantity = 2,
                Price = 500.00m
            }
        },
        DiscountPercent = 0,
        TaxRate = 0.08m,
        Shipping = 0,
        PaymentTerms = "Net 30 days. Payment due within 30 days of invoice date.",
        PaymentInstructions = "Please make checks payable to Acme Corporation or pay via bank transfer to account #12345678.",
        Notes = "Thank you for your business!"
    }
};

// Generate invoice
var generator = new InvoiceGenerator("invoice-template.html");

using (var output = new FileStream("invoice-2025-001.pdf", FileMode.Create))
{
    generator.GenerateInvoice(invoiceData, output);
    Console.WriteLine($"Invoice generated: {output.Name}");
    Console.WriteLine($"Total amount: ${invoiceData.Invoice.Total:0.00}");
}
```

---

## Step-by-Step Breakdown

### 1. Header Section

{% raw %}
```html
<div class="invoice-header">
    <div>
        <img src="{{company.logo}}" class="company-logo" />
    </div>
    <div class="invoice-title">
        <h1>INVOICE</h1>
        <p>{{invoice.number}}</p>
    </div>
</div>
```
{% endraw %}

**Features:**
- Company logo on left
- Invoice title and number on right
- Flexbox layout for alignment
- Bottom border for separation

### 2. Company and Customer Information

{% raw %}
```html
<div class="info-section">
    <div class="company-info">
        <h3>From</h3>
        <p><strong>{{company.name}}</strong></p>
        <p>{{company.address}}</p>
    </div>

    <div class="customer-info">
        <h3>Bill To</h3>
        <p><strong>{{customer.name}}</strong></p>
        {{#if customer.company}}
        <p>{{customer.company}}</p>
        {{/if}}
    </div>
</div>
```
{% endraw %}

**Features:**
- Side-by-side layout
- Conditional customer company name
- Clear visual hierarchy

### 3. Line Items Table

{% raw %}
```html
<table class="line-items">
    <thead>
        <tr>
            <th>Description</th>
            <th class="right">Quantity</th>
            <th class="right">Unit Price</th>
            <th class="right">Amount</th>
        </tr>
    </thead>
    <tbody>
        {{#each invoice.items}}
        <tr>
            <td>
                <strong>{{this.description}}</strong>
                {{#if this.details}}
                <br /><span style="font-size: 9pt;">{{this.details}}</span>
                {{/if}}
            </td>
            <td class="right">{{this.quantity}}</td>
            <td class="right">${{format(this.price, '0.00')}}</td>
            <td class="right">${{format(calc(this.quantity, '*', this.price), '0.00')}}</td>
        </tr>
        {{/each}}
    </tbody>
</table>
```
{% endraw %}

**Features:**
- Dynamic rows with `{{#each}}`
- Automatic calculation: quantity × price
- Optional item details
- Professional styling with alternating rows

### 4. Totals Calculation

{% raw %}
```html
<div class="totals-section">
    <table>
        <tr>
            <td>Subtotal:</td>
            <td>${{format(invoice.subtotal, '0.00')}}</td>
        </tr>
        <tr>
            <td>Tax ({{format(calc(invoice.taxRate, '*', 100), '0.0')}}%):</td>
            <td>${{format(invoice.tax, '0.00')}}</td>
        </tr>
        <tr class="total-row">
            <td>Total Due:</td>
            <td>${{format(invoice.total, '0.00')}}</td>
        </tr>
    </table>
</div>
```
{% endraw %}

**Features:**
- Subtotal, tax, and total
- Optional discount and shipping
- Highlighted total row
- Right-aligned for readability

---

## Common Variations

### Variation 1: Simple Invoice (Minimal)

Remove optional sections for a cleaner look:

```html
<!-- Remove discount section -->
<!-- Remove shipping section -->
<!-- Remove notes section -->
```

### Variation 2: Detailed Invoice (Itemized)

Add more item details:

{% raw %}
```html
<tr>
    <td>
        <strong>{{this.description}}</strong>
        <br /><span>SKU: {{this.sku}}</span>
        <br /><span>{{this.details}}</span>
    </td>
    <td class="right">{{this.quantity}} {{this.unit}}</td>
    <td class="right">${{format(this.price, '0.00')}}</td>
    <td class="right">{{this.discountPercent}}%</td>
    <td class="right">${{format(this.lineTotal, '0.00')}}</td>
</tr>
```
{% endraw %}

### Variation 3: Service Invoice (Hourly)

For hourly billing:

{% raw %}
```html
<table class="line-items">
    <thead>
        <tr>
            <th>Service Description</th>
            <th class="right">Hours</th>
            <th class="right">Rate</th>
            <th class="right">Amount</th>
        </tr>
    </thead>
    <tbody>
        {{#each invoice.services}}
        <tr>
            <td>
                <strong>{{this.description}}</strong>
                <br /><span>Date: {{this.date}}</span>
            </td>
            <td class="right">{{format(this.hours, '0.00')}}</td>
            <td class="right">${{{format(this.rate, '0.00')}}/hr</td>
            <td class="right">${{format(calc(this.hours, '*', this.rate), '0.00')}}</td>
        </tr>
        {{/each}}
    </tbody>
</table>
```
{% endraw %}

---

## Tips & Tricks

### Professional Touches

1. **Logo Optimization:**
```html
<img src="{{company.logo}}"
     class="company-logo"
     alt="{{company.name}}"
     style="max-height: 50pt; max-width: 200pt;" />
```

2. **Status Badge:**
{% raw %}
```html
{{#if invoice.isPaid}}
<div style="position: absolute; top: 100pt; right: 50pt;
            transform: rotate(-15deg); border: 4pt solid #10b981;
            color: #10b981; font-size: 24pt; font-weight: bold;
            padding: 10pt 20pt;">
    PAID
</div>
{{/if}}
```
{% endraw %}

3. **QR Code for Payment:**
```html
<div style="text-align: center; margin-top: 20pt;">
    <img src="{{invoice.paymentQrCode}}"
         style="width: 100pt; height: 100pt;" />
    <p style="font-size: 8pt;">Scan to pay</p>
</div>
```

4. **Terms and Conditions:**
```html
<div style="font-size: 8pt; color: #666; margin-top: 30pt;
            border-top: 1pt solid #e5e7eb; padding-top: 10pt;">
    <p><strong>Terms & Conditions:</strong></p>
    <p>{{company.termsAndConditions}}</p>
</div>
```

---

## Try It Yourself

### Exercise 1: Customize the Design

Modify the invoice template:
- Change the color scheme (replace #2563eb)
- Add your company logo
- Adjust fonts and spacing
- Add custom footer information

### Exercise 2: Add Calculations

Enhance the template with:
- Line item discounts
- Multiple tax rates
- Currency conversion
- Running totals

### Exercise 3: Create Variations

Build specialized invoices for:
- Recurring subscriptions
- Project milestones
- Retail sales
- International customers (multi-currency)

---

## Common Pitfalls

### ❌ Hardcoded Calculations

```html
<!-- Hardcoded total - won't update -->
<td>Total: $1,000.00</td>
```

✅ **Solution:**

{% raw %}
```html
<!-- Dynamic calculation -->
<td>Total: ${{format(invoice.total, '0.00')}}</td>
```
{% endraw %}

### ❌ Missing Null Checks

{% raw %}
```html
<!-- Will break if customer.company is null -->
<p>{{customer.company}}</p>
```
{% endraw %}

✅ **Solution:**

{% raw %}
```html
<!-- Conditional rendering -->
{{#if customer.company}}
<p>{{customer.company}}</p>
{{/if}}
```
{% endraw %}

### ❌ Poor Number Formatting

{% raw %}
```html
<!-- No currency formatting -->
<td>${{invoice.total}}</td>
<!-- Output: $1234.5 -->
```
{% endraw %}

✅ **Solution:**

{% raw %}
```html
<!-- Proper formatting -->
<td>${{format(invoice.total, '0.00')}}</td>
<!-- Output: $1,234.50 -->
```
{% endraw %}

---

## Customization Checklist

- [ ] Replace logo with your company logo
- [ ] Update color scheme to match brand
- [ ] Customize payment terms
- [ ] Add/remove optional sections
- [ ] Test with real customer data
- [ ] Verify calculations are correct
- [ ] Check all conditional sections
- [ ] Test with various item counts (1, 10, 50+ items)

---

## Best Practices

1. **Always Calculate in C#** - Don't rely on template calculations for financial data
2. **Format Currency Consistently** - Use format() for all monetary values
3. **Validate Data** - Check for null/empty values before generation
4. **Test Edge Cases** - Single item, many items, zero amounts
5. **Use Semantic HTML** - Proper table structure for accessibility
6. **Include Metadata** - Set proper PDF title and author
7. **Version Your Templates** - Track template changes
8. **Keep Styling External** - Use <style> tag, not inline styles everywhere

---

## Next Steps

1. **[Business Letter](02_business_letter.md)** - Multi-page correspondence
2. **[Report Template](03_report_template.md)** - Complex reports with sections
3. **[Data-Driven Report](05_data_driven_report.md)** - Database integration

---

**Continue learning →** [Business Letter](02_business_letter.md)
