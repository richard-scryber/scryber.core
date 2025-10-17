---
layout: default
title: data-value
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @data-value : The Computed Value Attribute

The `data-value` attribute enables computed or dynamic value assignment for elements, allowing you to bind expressions to element values, store intermediate calculations, or pass computed values to document parameters. This attribute is primarily used with `<var>` elements for creating document-level variables and computed values.

---

## Summary

The `data-value` attribute provides dynamic value computation and assignment, enabling:
- **Computed value storage** in document-level variables
- **Expression evaluation** for complex calculations
- **Intermediate value caching** for reuse throughout the document
- **Dynamic value assignment** from data binding expressions
- **Document parameter creation** for cross-reference usage

This attribute is essential for:
- Creating computed values for use throughout the document
- Storing intermediate calculations
- Building document-level constants from data
- Creating reusable value references
- Implementing calculated fields
- Dynamic parameter generation

**Primary Use Case**: The `data-value` attribute is most commonly used with `<var>` elements in combination with `data-id` to create document-level variables that can be referenced elsewhere.

---

## Usage

The `data-value` attribute is applied to `<var>` elements and `<num>` elements:

```html
<!-- Create document variable -->
<var data-id="taxRate" data-value="{{0.08}}"></var>

<!-- Use variable elsewhere -->
<div>Tax Amount: ${{model.subtotal * taxRate}}</div>
```

### Basic Syntax

```html
<!-- Store simple value -->
<var data-id="companyName" data-value="{{model.company}}"></var>

<!-- Store computed value -->
<var data-id="total" data-value="{{model.subtotal + model.tax}}"></var>

<!-- Store complex expression -->
<var data-id="discount" data-value="{{model.isPremium ? 0.15 : 0.05}}"></var>

<!-- Number element with data-value -->
<num data-value="{{model.quantity * model.unitPrice}}" data-format="C"></num>
```

---

## Supported Elements

The `data-value` attribute is supported on the following elements:

- `<var>` - Variable element for storing computed values
- `<num>` - Number element for formatted numeric display

**Note**: When used with `<var>`, the `data-id` attribute is typically required to make the value accessible as a document parameter.

---

## Binding Values

### Attribute Values

| Value Type | Description |
|------------|-------------|
| **Literal Value** | Direct string, number, or boolean value |
| **Binding Expression** | Expression that evaluates to a value: `{{expression}}` |
| **Property Access** | Reference to model property: `{{model.property}}` |
| **Calculation** | Mathematical expression: `{{value1 + value2}}` |
| **Conditional** | Ternary expression: `{{condition ? value1 : value2}}` |

### Type

**Type**: `object` (any type that can be converted to string)
**Default**: `null`

### Expression Support

The attribute accepts any valid binding expression:

```html
<!-- Simple value -->
<var data-id="name" data-value="{{model.userName}}"></var>

<!-- Mathematical calculation -->
<var data-id="taxAmount" data-value="{{model.subtotal * 0.08}}"></var>

<!-- String concatenation -->
<var data-id="fullName" data-value="{{model.firstName + ' ' + model.lastName}}"></var>

<!-- Conditional expression -->
<var data-id="status" data-value="{{model.isActive ? 'Active' : 'Inactive'}}"></var>

<!-- Complex calculation -->
<var data-id="total" data-value="{{(model.subtotal + model.shipping) * (1 + model.taxRate)}}"></var>
```

---

## Notes

### Variable Storage Mechanism

When `<var>` elements have both `data-id` and `data-value`:
1. Expression in `data-value` is evaluated during data binding
2. Result is stored in `document.Params[data-id]`
3. Value becomes available throughout the document
4. Can be referenced using `{{data-id}}` syntax

### Visibility Behavior

`<var>` elements with `data-id` and `data-value` but no content are automatically hidden:
- If the element has content, it displays normally
- If the element is empty, it becomes `Visible = false`
- This prevents empty variable declarations from affecting layout

### Variable Scope

Variables created with `data-value` have document-wide scope:
- Available in subsequent binding expressions
- Can be used in any element after the `<var>` declaration
- Not limited to parent/child relationships
- Persist throughout document generation

### Expression Evaluation Order

Variables are evaluated in document order:
- Variables must be declared before use
- Cannot reference variables declared later in document
- Consider placement of variable declarations

### Formatting Numbers

When used with `<num>` elements, `data-value` provides the raw value:

```html
<num data-value="{{model.price}}" data-format="C"></num>
```

The `data-format` attribute controls display formatting while `data-value` provides the underlying value.

### Use Cases

**Document-Level Constants**:
```html
<var data-id="taxRate" data-value="{{0.0875}}"></var>
<var data-id="companyName" data-value="{{'Acme Corporation'}}"></var>
```

**Computed Totals**:
```html
<var data-id="grandTotal" data-value="{{model.subtotal + model.tax + model.shipping}}"></var>
```

**Conditional Values**:
```html
<var data-id="discountRate" data-value="{{model.isVIP ? 0.20 : 0.10}}"></var>
```

**Complex Calculations**:
```html
<var data-id="finalPrice"
     data-value="{{model.basePrice * (1 - model.discount) * (1 + model.tax)}}"></var>
```

---

## Examples

### 1. Simple Variable Storage

Store a computed value for later use:

```html
<var data-id="orderTotal" data-value="{{model.subtotal + model.tax}}"></var>

<div>
    <h2>Order Summary</h2>
    <p>Subtotal: ${{model.subtotal}}</p>
    <p>Tax: ${{model.tax}}</p>
    <p>Total: ${{orderTotal}}</p>
</div>
```

### 2. Tax Rate Constant

Create a reusable tax rate:

```html
<var data-id="taxRate" data-value="{{0.0825}}"></var>

<table style="width: 100%;">
    <template data-bind="{{model.items}}">
        <tr>
            <td>{{.name}}</td>
            <td>${{.price}}</td>
            <td>${{.price * taxRate}}</td>
            <td>${{.price * (1 + taxRate)}}</td>
        </tr>
    </template>
</table>
```

### 3. Discount Calculation

Compute and store discount amount:

```html
<var data-id="discountPercent" data-value="{{model.isPremium ? 0.15 : 0.05}}"></var>
<var data-id="discountAmount" data-value="{{model.subtotal * discountPercent}}"></var>

<div style="padding: 15pt; background-color: #d4edda;">
    <h3>Your Discount</h3>
    <p>Discount Rate: {{discountPercent * 100}}%</p>
    <p>Discount Amount: ${{discountAmount}}</p>
    <p>You Save: ${{discountAmount}}</p>
</div>
```

### 4. Full Name Computation

Combine first and last names:

```html
<var data-id="customerFullName"
     data-value="{{model.firstName + ' ' + model.lastName}}"></var>

<h1>Invoice for {{customerFullName}}</h1>

<div>
    <p>Dear {{customerFullName}},</p>
    <p>Thank you for your business.</p>
</div>
```

### 5. Formatted Number Display

Display computed value with formatting:

```html
<div>
    <p>Unit Price:
        <num data-value="{{model.basePrice}}" data-format="C"></num>
    </p>
    <p>Quantity: {{model.quantity}}</p>
    <p>Line Total:
        <num data-value="{{model.basePrice * model.quantity}}" data-format="C"></num>
    </p>
</div>
```

### 6. Conditional Status Text

Store conditional status string:

```html
<var data-id="statusText"
     data-value="{{model.isPaid ? 'PAID IN FULL' : 'PAYMENT PENDING'}}"></var>
<var data-id="statusColor"
     data-value="{{model.isPaid ? 'green' : 'red'}}"></var>

<div style="color: {{statusColor}}; font-weight: bold; font-size: 16pt;">
    {{statusText}}
</div>
```

### 7. Complex Financial Calculation

Multi-step calculation with intermediate values:

```html
<!-- Store intermediate calculations -->
<var data-id="itemTotal" data-value="{{model.subtotal}}"></var>
<var data-id="taxAmount" data-value="{{itemTotal * 0.0825}}"></var>
<var data-id="shippingCost" data-value="{{itemTotal > 100 ? 0 : 9.99}}"></var>
<var data-id="grandTotal" data-value="{{itemTotal + taxAmount + shippingCost}}"></var>

<table style="width: 100%; font-size: 12pt;">
    <tr>
        <td style="text-align: right; padding: 5pt;">Subtotal:</td>
        <td style="text-align: right; padding: 5pt; width: 100pt;">${{itemTotal}}</td>
    </tr>
    <tr>
        <td style="text-align: right; padding: 5pt;">Tax (8.25%):</td>
        <td style="text-align: right; padding: 5pt;">${{taxAmount}}</td>
    </tr>
    <tr>
        <td style="text-align: right; padding: 5pt;">Shipping:</td>
        <td style="text-align: right; padding: 5pt;">
            ${{shippingCost}}
            <if data-test="{{shippingCost == 0}}">
                <span style="color: green; font-size: 8pt;"> (FREE!)</span>
            </if>
        </td>
    </tr>
    <tr style="border-top: 2pt solid black; font-weight: bold; font-size: 14pt;">
        <td style="text-align: right; padding: 10pt 5pt;">Grand Total:</td>
        <td style="text-align: right; padding: 10pt 5pt;">${{grandTotal}}</td>
    </tr>
</table>
```

### 8. Date Formatting

Store formatted date string:

```html
<var data-id="invoiceDate" data-value="{{model.date}}"></var>
<var data-id="dueDate" data-value="{{model.dueDate}}"></var>

<div>
    <p><strong>Invoice Date:</strong> {{invoiceDate}}</p>
    <p><strong>Payment Due:</strong> {{dueDate}}</p>
</div>
```

### 9. Percentage Calculations

Calculate and display percentages:

```html
<var data-id="completionPercent"
     data-value="{{(model.completed / model.total) * 100}}"></var>

<div style="padding: 15pt; border: 1pt solid #336699;">
    <h3>Progress Report</h3>
    <p>Completed: {{model.completed}} of {{model.total}} items</p>
    <p>Completion Rate:
        <num data-value="{{completionPercent}}" data-format="#0.0"></num>%
    </p>
    <div style="width: 100%; background-color: #ddd; height: 20pt;">
        <div style="width: {{completionPercent}}%; background-color: #336699; height: 20pt;"></div>
    </div>
</div>
```

### 10. Conditional Pricing

Store pricing based on customer type:

```html
<var data-id="basePrice" data-value="{{model.listPrice}}"></var>
<var data-id="discount"
     data-value="{{model.customerType == 'wholesale' ? 0.30 : model.customerType == 'retail' ? 0.10 : 0}}"></var>
<var data-id="finalPrice" data-value="{{basePrice * (1 - discount)}}"></var>

<div>
    <p>List Price: ${{basePrice}}</p>
    <if data-test="{{discount > 0}}">
        <p>Your Discount: {{discount * 100}}%</p>
    </if>
    <p style="font-size: 16pt; font-weight: bold;">
        Your Price: ${{finalPrice}}
    </p>
</div>
```

### 11. Company Information Variables

Store company details as variables:

```html
<var data-id="companyName" data-value="{{'Acme Corporation'}}"></var>
<var data-id="companyAddress" data-value="{{'123 Business St, Suite 100'}}"></var>
<var data-id="companyCityState" data-value="{{'New York, NY 10001'}}"></var>
<var data-id="companyPhone" data-value="{{'(555) 123-4567'}}"></var>

<!-- Use throughout document -->
<div style="text-align: center; margin-bottom: 20pt;">
    <h1>{{companyName}}</h1>
    <p>{{companyAddress}}</p>
    <p>{{companyCityState}}</p>
    <p>Phone: {{companyPhone}}</p>
</div>
```

### 12. Aggregate Statistics

Calculate summary statistics:

```html
<!-- Hidden variables for calculations -->
<var data-id="totalOrders" data-value="{{count(model.orders)}}"></var>
<var data-id="totalRevenue" data-value="{{sum(model.orders, .amount)}}"></var>
<var data-id="avgOrderValue" data-value="{{totalRevenue / totalOrders}}"></var>

<div style="background-color: #e7f3ff; padding: 20pt; margin: 20pt 0;">
    <h2 style="margin: 0 0 15pt 0;">Summary Statistics</h2>
    <table style="width: 100%; font-size: 12pt;">
        <tr>
            <td>Total Orders:</td>
            <td style="text-align: right; font-weight: bold;">{{totalOrders}}</td>
        </tr>
        <tr>
            <td>Total Revenue:</td>
            <td style="text-align: right; font-weight: bold;">
                <num data-value="{{totalRevenue}}" data-format="C"></num>
            </td>
        </tr>
        <tr>
            <td>Average Order Value:</td>
            <td style="text-align: right; font-weight: bold;">
                <num data-value="{{avgOrderValue}}" data-format="C"></num>
            </td>
        </tr>
    </table>
</div>
```

### 13. Shipping Cost Calculation

Complex shipping logic:

```html
<var data-id="weight" data-value="{{model.totalWeight}}"></var>
<var data-id="baseShipping" data-value="{{5.99}}"></var>
<var data-id="weightCharge" data-value="{{weight > 5 ? (weight - 5) * 0.99 : 0}}"></var>
<var data-id="totalShipping" data-value="{{baseShipping + weightCharge}}"></var>
<var data-id="freeShipping" data-value="{{model.subtotal > 50}}"></var>
<var data-id="finalShipping" data-value="{{freeShipping ? 0 : totalShipping}}"></var>

<div>
    <p>Package Weight: {{weight}} lbs</p>
    <if data-test="{{freeShipping}}">
        <p style="color: green; font-weight: bold;">
            FREE SHIPPING! (Orders over $50)
        </p>
    </if>
    <if data-test="{{!freeShipping}}">
        <p>Shipping Cost: ${{finalShipping}}</p>
    </if>
</div>
```

### 14. Dynamic Page Title

Create page-specific titles:

```html
<var data-id="pageTitle"
     data-value="{{model.documentType + ' - ' + model.referenceNumber}}"></var>
<var data-id="pageSubtitle"
     data-value="{{model.customerName + ' (' + model.date + ')'}}"></var>

<div style="background-color: #336699; color: white; padding: 20pt;">
    <h1 style="margin: 0;">{{pageTitle}}</h1>
    <div style="margin-top: 5pt; font-size: 11pt;">{{pageSubtitle}}</div>
</div>
```

### 15. Conditional Message Text

Generate dynamic messages:

```html
<var data-id="daysUntilDue" data-value="{{model.daysUntilDue}}"></var>
<var data-id="urgencyMessage"
     data-value="{{daysUntilDue < 0 ? 'OVERDUE' : daysUntilDue == 0 ? 'DUE TODAY' : daysUntilDue < 7 ? 'DUE SOON' : 'ON TRACK'}}"></var>
<var data-id="urgencyColor"
     data-value="{{daysUntilDue < 0 ? 'red' : daysUntilDue < 7 ? 'orange' : 'green'}}"></var>

<div style="padding: 15pt; border: 2pt solid {{urgencyColor}}; background-color: rgba(255,255,255,0.9);">
    <div style="color: {{urgencyColor}}; font-weight: bold; font-size: 14pt;">
        {{urgencyMessage}}
    </div>
    <if data-test="{{daysUntilDue >= 0}}">
        <p>Payment due in {{daysUntilDue}} days</p>
    </if>
    <if data-test="{{daysUntilDue < 0}}">
        <p>Payment is {{-daysUntilDue}} days overdue</p>
    </if>
</div>
```

### 16. Multi-Currency Support

Calculate currency conversions:

```html
<var data-id="usdAmount" data-value="{{model.amount}}"></var>
<var data-id="exchangeRate" data-value="{{model.exchangeRate}}"></var>
<var data-id="localAmount" data-value="{{usdAmount * exchangeRate}}"></var>

<table style="width: 100%;">
    <tr>
        <td>Amount (USD):</td>
        <td style="text-align: right;">
            <num data-value="{{usdAmount}}" data-format="C"></num>
        </td>
    </tr>
    <tr>
        <td>Exchange Rate:</td>
        <td style="text-align: right;">{{exchangeRate}}</td>
    </tr>
    <tr style="font-weight: bold; border-top: 1pt solid black;">
        <td>Amount ({{model.localCurrency}}):</td>
        <td style="text-align: right;">
            {{model.currencySymbol}}<num data-value="{{localAmount}}" data-format="#,##0.00"></num>
        </td>
    </tr>
</table>
```

### 17. Inventory Status Calculation

Compute inventory metrics:

```html
<var data-id="onHand" data-value="{{model.quantityOnHand}}"></var>
<var data-id="committed" data-value="{{model.quantityCommitted}}"></var>
<var data-id="available" data-value="{{onHand - committed}}"></var>
<var data-id="reorderPoint" data-value="{{model.reorderPoint}}"></var>
<var data-id="needsReorder" data-value="{{available <= reorderPoint}}"></var>

<div style="padding: 15pt; border: 1pt solid #ddd;">
    <h3>{{model.productName}}</h3>
    <table style="width: 100%;">
        <tr>
            <td>On Hand:</td>
            <td style="text-align: right;">{{onHand}}</td>
        </tr>
        <tr>
            <td>Committed:</td>
            <td style="text-align: right;">{{committed}}</td>
        </tr>
        <tr style="font-weight: bold;">
            <td>Available:</td>
            <td style="text-align: right;">{{available}}</td>
        </tr>
    </table>

    <if data-test="{{needsReorder}}">
        <div style="margin-top: 10pt; padding: 10pt; background-color: #fff3cd; border: 1pt solid #ffc107;">
            <strong>⚠ Reorder Required</strong><br/>
            Available quantity ({{available}}) is at or below reorder point ({{reorderPoint}})
        </div>
    </if>
</div>
```

### 18. Loan Payment Calculator

Calculate loan payment details:

```html
<var data-id="principal" data-value="{{model.loanAmount}}"></var>
<var data-id="annualRate" data-value="{{model.interestRate}}"></var>
<var data-id="monthlyRate" data-value="{{annualRate / 12}}"></var>
<var data-id="months" data-value="{{model.loanTermYears * 12}}"></var>
<var data-id="monthlyPayment"
     data-value="{{principal * (monthlyRate * (1 + monthlyRate)^months) / ((1 + monthlyRate)^months - 1)}}"></var>
<var data-id="totalPayments" data-value="{{monthlyPayment * months}}"></var>
<var data-id="totalInterest" data-value="{{totalPayments - principal}}"></var>

<div style="padding: 20pt; border: 2pt solid #336699;">
    <h2 style="color: #336699;">Loan Details</h2>
    <table style="width: 100%; font-size: 11pt;">
        <tr>
            <td>Loan Amount:</td>
            <td style="text-align: right;">
                <num data-value="{{principal}}" data-format="C"></num>
            </td>
        </tr>
        <tr>
            <td>Interest Rate:</td>
            <td style="text-align: right;">{{annualRate * 100}}%</td>
        </tr>
        <tr>
            <td>Term:</td>
            <td style="text-align: right;">{{model.loanTermYears}} years</td>
        </tr>
        <tr style="border-top: 1pt solid black; font-weight: bold; font-size: 12pt;">
            <td>Monthly Payment:</td>
            <td style="text-align: right;">
                <num data-value="{{monthlyPayment}}" data-format="C"></num>
            </td>
        </tr>
        <tr>
            <td>Total Payments:</td>
            <td style="text-align: right;">
                <num data-value="{{totalPayments}}" data-format="C"></num>
            </td>
        </tr>
        <tr>
            <td>Total Interest:</td>
            <td style="text-align: right; color: red;">
                <num data-value="{{totalInterest}}" data-format="C"></num>
            </td>
        </tr>
    </table>
</div>
```

### 19. Grade Calculation

Compute grade statistics:

```html
<var data-id="totalPoints" data-value="{{sum(model.assignments, .pointsEarned)}}"></var>
<var data-id="possiblePoints" data-value="{{sum(model.assignments, .pointsPossible)}}"></var>
<var data-id="percentage" data-value="{{(totalPoints / possiblePoints) * 100}}"></var>
<var data-id="letterGrade"
     data-value="{{percentage >= 90 ? 'A' : percentage >= 80 ? 'B' : percentage >= 70 ? 'C' : percentage >= 60 ? 'D' : 'F'}}"></var>

<div style="padding: 20pt; border: 2pt solid #336699;">
    <h2>Grade Report for {{model.studentName}}</h2>

    <table style="width: 100%; margin: 15pt 0;">
        <thead>
            <tr style="background-color: #f0f0f0;">
                <th style="padding: 8pt; text-align: left;">Assignment</th>
                <th style="padding: 8pt; text-align: right;">Points Earned</th>
                <th style="padding: 8pt; text-align: right;">Points Possible</th>
            </tr>
        </thead>
        <tbody>
            <template data-bind="{{model.assignments}}">
                <tr>
                    <td style="padding: 6pt; border-bottom: 1pt solid #ddd;">{{.name}}</td>
                    <td style="padding: 6pt; border-bottom: 1pt solid #ddd; text-align: right;">
                        {{.pointsEarned}}
                    </td>
                    <td style="padding: 6pt; border-bottom: 1pt solid #ddd; text-align: right;">
                        {{.pointsPossible}}
                    </td>
                </tr>
            </template>
        </tbody>
        <tfoot>
            <tr style="font-weight: bold; border-top: 2pt solid black;">
                <td style="padding: 8pt;">Total:</td>
                <td style="padding: 8pt; text-align: right;">{{totalPoints}}</td>
                <td style="padding: 8pt; text-align: right;">{{possiblePoints}}</td>
            </tr>
        </tfoot>
    </table>

    <div style="padding: 15pt; background-color: #e7f3ff; text-align: center;">
        <div style="font-size: 14pt; margin-bottom: 5pt;">
            Final Grade: <num data-value="{{percentage}}" data-format="#0.0"></num>%
        </div>
        <div style="font-size: 24pt; font-weight: bold; color: #336699;">
            {{letterGrade}}
        </div>
    </div>
</div>
```

### 20. Comprehensive Invoice with All Calculations

Production-ready invoice with complete calculations:

```html
<!-- Company Information -->
<var data-id="companyName" data-value="{{'Acme Corporation'}}"></var>
<var data-id="companyAddress" data-value="{{'123 Business Street'}}"></var>

<!-- Invoice Calculations -->
<var data-id="itemsSubtotal" data-value="{{sum(model.lineItems, .lineTotal)}}"></var>
<var data-id="discountPercent" data-value="{{model.customerType == 'VIP' ? 0.10 : 0}}"></var>
<var data-id="discountAmount" data-value="{{itemsSubtotal * discountPercent}}"></var>
<var data-id="subtotalAfterDiscount" data-value="{{itemsSubtotal - discountAmount}}"></var>
<var data-id="taxRate" data-value="{{0.0825}}"></var>
<var data-id="taxAmount" data-value="{{subtotalAfterDiscount * taxRate}}"></var>
<var data-id="shippingCost" data-value="{{subtotalAfterDiscount > 100 ? 0 : 12.99}}"></var>
<var data-id="grandTotal" data-value="{{subtotalAfterDiscount + taxAmount + shippingCost}}"></var>

<!-- Status Variables -->
<var data-id="isPaid" data-value="{{model.paymentStatus == 'paid'}}"></var>
<var data-id="statusColor" data-value="{{isPaid ? 'green' : 'red'}}"></var>
<var data-id="statusText" data-value="{{isPaid ? 'PAID' : 'UNPAID'}}"></var>

<!-- Document -->
<div style="padding: 30pt;">
    <!-- Header -->
    <table style="width: 100%; margin-bottom: 30pt;">
        <tr>
            <td style="width: 50%;">
                <h1 style="margin: 0; color: #336699;">{{companyName}}</h1>
                <div>{{companyAddress}}</div>
            </td>
            <td style="width: 50%; text-align: right;">
                <h1 style="margin: 0;">INVOICE</h1>
                <div><strong>#{{model.invoiceNumber}}</strong></div>
                <div>Date: {{model.invoiceDate}}</div>
            </td>
        </tr>
    </table>

    <!-- Bill To -->
    <div style="margin-bottom: 30pt;">
        <strong>Bill To:</strong><br/>
        {{model.customerName}}<br/>
        {{model.customerAddress}}
    </div>

    <!-- Line Items -->
    <table style="width: 100%; border-collapse: collapse;">
        <thead>
            <tr style="background-color: #336699; color: white;">
                <th style="padding: 10pt; text-align: left;">Description</th>
                <th style="padding: 10pt; text-align: right;">Qty</th>
                <th style="padding: 10pt; text-align: right;">Unit Price</th>
                <th style="padding: 10pt; text-align: right;">Total</th>
            </tr>
        </thead>
        <tbody>
            <template data-bind="{{model.lineItems}}">
                <tr>
                    <td style="padding: 8pt; border-bottom: 1pt solid #ddd;">{{.description}}</td>
                    <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right;">
                        {{.quantity}}
                    </td>
                    <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right;">
                        <num data-value="{{.unitPrice}}" data-format="C"></num>
                    </td>
                    <td style="padding: 8pt; border-bottom: 1pt solid #ddd; text-align: right; font-weight: bold;">
                        <num data-value="{{.lineTotal}}" data-format="C"></num>
                    </td>
                </tr>
            </template>
        </tbody>
    </table>

    <!-- Totals -->
    <div style="width: 50%; margin-left: 50%; margin-top: 20pt;">
        <table style="width: 100%;">
            <tr>
                <td style="padding: 5pt; text-align: right;">Subtotal:</td>
                <td style="padding: 5pt; text-align: right; width: 120pt;">
                    <num data-value="{{itemsSubtotal}}" data-format="C"></num>
                </td>
            </tr>
            <if data-test="{{discountAmount > 0}}">
                <tr>
                    <td style="padding: 5pt; text-align: right;">
                        Discount ({{discountPercent * 100}}%):
                    </td>
                    <td style="padding: 5pt; text-align: right; color: green;">
                        -<num data-value="{{discountAmount}}" data-format="C"></num>
                    </td>
                </tr>
            </if>
            <tr>
                <td style="padding: 5pt; text-align: right;">Tax ({{taxRate * 100}}%):</td>
                <td style="padding: 5pt; text-align: right;">
                    <num data-value="{{taxAmount}}" data-format="C"></num>
                </td>
            </tr>
            <tr>
                <td style="padding: 5pt; text-align: right;">
                    Shipping:
                    <if data-test="{{shippingCost == 0}}">
                        <span style="color: green; font-size: 8pt;"> (FREE!)</span>
                    </if>
                </td>
                <td style="padding: 5pt; text-align: right;">
                    <num data-value="{{shippingCost}}" data-format="C"></num>
                </td>
            </tr>
            <tr style="border-top: 2pt solid black; font-weight: bold; font-size: 14pt;">
                <td style="padding: 10pt 5pt; text-align: right;">Total:</td>
                <td style="padding: 10pt 5pt; text-align: right;">
                    <num data-value="{{grandTotal}}" data-format="C"></num>
                </td>
            </tr>
        </table>
    </div>

    <!-- Payment Status -->
    <div style="margin-top: 30pt; padding: 15pt; border: 2pt solid {{statusColor}}; text-align: center;">
        <div style="color: {{statusColor}}; font-weight: bold; font-size: 18pt;">
            {{statusText}}
        </div>
        <if data-test="{{isPaid}}">
            <div style="margin-top: 5pt;">
                Payment received: {{model.paymentDate}}
            </div>
        </if>
        <if data-test="{{!isPaid}}">
            <div style="margin-top: 5pt;">
                Payment due: {{model.dueDate}}
            </div>
        </if>
    </div>
</div>
```

---

## See Also

- [var element](/reference/htmltags/var.html) - Variable element for computed values
- [num element](/reference/htmltags/num.html) - Number element for formatted display
- [data-id attribute](/reference/htmlattributes/data-id.html) - Variable identifier
- [Data Binding](/reference/binding/) - Complete data binding guide
- [Expressions](/reference/expressions/) - Expression syntax reference
- [Document Parameters](/reference/parameters/) - Document-level parameters

---
