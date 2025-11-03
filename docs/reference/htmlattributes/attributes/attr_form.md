---
layout: default
title: form
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @form : The Associated Form Identifier Attribute

The `form` attribute associates an HTML output element with a form element by its ID. This attribute enables output elements to be linked to forms even when they are not nested within the form's DOM structure.

## Usage

The `form` attribute is used to:
- Associate an `<output>` element with a specific form
- Link output elements to forms outside the normal DOM hierarchy
- Enable form-related functionality for standalone output elements
- Reference the form that owns the output element

```html
<form id="calculator">
    <input type="number" id="num1" name="num1" value="10" />
    <input type="number" id="num2" name="num2" value="20" />
</form>

<!-- Output element associated with form via form attribute -->
<output for="num1 num2" form="calculator">30</output>
```

---

## Supported Elements

The `form` attribute is supported by the following element:

| Element | Description |
|---------|-------------|
| `<output>` | Represents the result of a calculation or user action |

**Note**: In standard HTML, the `form` attribute can also be used on input elements (`<input>`, `<textarea>`, `<select>`, `<button>`), but in Scryber PDF generation, it is primarily used with the `<output>` element.

---

## Attribute Values

### Syntax

```html
<output form="form-id">Output value</output>
```

### Value Type

| Type | Description | Example |
|------|-------------|---------|
| ID reference | The `id` of the associated form element | `form="myForm"` |

### Value Requirements

- Must reference a valid form element `id` in the document
- The form element must exist and have a matching `id` attribute
- If the form ID doesn't exist, the association is simply not made (no error)
- Can be dynamically bound to a model property

---

## Binding Values

The `form` attribute supports both static and dynamic values:

### Static Form Association

```html
<form id="contactForm">
    <input type="text" name="name" value="John Doe" />
    <input type="email" name="email" value="john@example.com" />
</form>

<output form="contactForm" for="name email">
    Contact: John Doe (john@example.com)
</output>
```

### Dynamic Form Association with Data Binding

```html
<!-- Model: { formId: "registrationForm" } -->
<form id="{{model.formId}}">
    <input type="text" name="username" value="{{model.username}}" />
</form>

<output form="{{model.formId}}" for="username">
    Username: {{model.username}}
</output>
```

### Conditional Form Association

```html
<!-- Model: { useForm: true, primaryFormId: "form1", secondaryFormId: "form2" } -->
<output form="{{model.useForm ? model.primaryFormId : model.secondaryFormId}}">
    Calculation result
</output>
```

---

## Notes

### Purpose in HTML

In web browsers, the `form` attribute:
- Allows form controls to be placed anywhere in the document
- Enables flexible form layouts
- Links output elements to forms for submission and validation
- Overrides default form association (parent form)

### Purpose in Scryber PDF

In Scryber PDF generation:
- The `form` attribute is primarily **informational and structural**
- It helps organize document semantics
- Forms are **not interactive** in static PDFs (no submission)
- The attribute maintains HTML compatibility when converting web forms to PDF
- Useful for documenting relationships between elements

### Form Element Relationship

The `form` attribute works in conjunction with:
- `for` attribute - Specifies which form controls the output is associated with
- `name` attribute - Names the output element
- `value` or `val` attribute - Provides the output value

### Default Behavior

- If `form` attribute is not specified, the output element is associated with its **ancestor form** (if any)
- If `form` attribute is specified, it overrides the ancestor form association
- If the referenced form ID doesn't exist, the output remains unassociated

### PDF Considerations

When generating PDFs:
- Forms are **rendered statically** (no interactive form fields by default)
- The `form` attribute helps maintain document structure
- Output values are rendered as text content
- Form association is preserved in the document semantic structure

### Use Cases in PDF Generation

1. **Documentation**: Preserve form structure when converting HTML to PDF
2. **Reports**: Show calculation results associated with input sections
3. **Invoices**: Display totals linked to item entry forms
4. **Receipts**: Show computed values related to input data
5. **Summary Sections**: Link output summaries to their data sources

---

## Examples

### Example 1: Basic Form Association

```html
<h2>Order Form</h2>

<form id="orderForm">
    <label for="quantity">Quantity:</label>
    <input type="number" id="quantity" name="quantity" value="5" />
    <br/>
    <label for="price">Unit Price:</label>
    <input type="number" id="price" name="price" value="29.99" />
</form>

<h3>Total</h3>
<output form="orderForm" for="quantity price" name="total">
    $149.95
</output>
```

### Example 2: Multiple Outputs for One Form

```html
<form id="calcForm">
    <input type="number" id="a" name="a" value="10" />
    <input type="number" id="b" name="b" value="20" />
    <input type="number" id="c" name="c" value="30" />
</form>

<div style="margin-top: 20pt;">
    <output form="calcForm" for="a b" name="sum">
        A + B = 30
    </output>
    <br/>
    <output form="calcForm" for="a b c" name="total">
        Total = 60
    </output>
    <br/>
    <output form="calcForm" for="a b c" name="average">
        Average = 20
    </output>
</div>
```

### Example 3: Output Outside Form Structure

```html
<!-- Form in header section -->
<div style="background-color: #f0f0f0; padding: 15pt; margin-bottom: 20pt;">
    <form id="filterForm">
        <label>Date Range:</label>
        <input type="date" name="startDate" value="2024-01-01" />
        <input type="date" name="endDate" value="2024-12-31" />
    </form>
</div>

<!-- Main content -->
<div style="margin: 20pt 0;">
    <h2>Report Summary</h2>
    <p>Based on the selected filters:</p>
</div>

<!-- Output in footer section, associated with header form -->
<div style="border-top: 1pt solid #ccc; padding-top: 10pt; margin-top: 40pt;">
    <output form="filterForm" for="startDate endDate">
        Report Period: January 1, 2024 - December 31, 2024
    </output>
</div>
```

### Example 4: Invoice with Calculation

```html
<h1>Invoice #12345</h1>

<form id="invoiceForm">
    <table border="1" cellpadding="10" style="width: 100%; margin: 20pt 0;">
        <tr>
            <th>Item</th>
            <th>Quantity</th>
            <th>Unit Price</th>
            <th>Line Total</th>
        </tr>
        <tr>
            <td>Premium Widget</td>
            <td><input type="number" id="qty1" name="qty1" value="10" /></td>
            <td>$25.00</td>
            <td>$250.00</td>
        </tr>
        <tr>
            <td>Standard Gadget</td>
            <td><input type="number" id="qty2" name="qty2" value="5" /></td>
            <td>$50.00</td>
            <td>$250.00</td>
        </tr>
    </table>
</form>

<div style="text-align: right; font-size: 14pt; font-weight: bold; margin-top: 20pt;">
    <output form="invoiceForm" for="qty1 qty2" name="subtotal">
        Subtotal: $500.00
    </output>
    <br/>
    <output form="invoiceForm" for="subtotal" name="tax">
        Tax (8%): $40.00
    </output>
    <br/>
    <output form="invoiceForm" for="subtotal tax" name="total">
        Total: $540.00
    </output>
</div>
```

### Example 5: Survey Results Summary

```html
<form id="surveyForm">
    <h2>Customer Satisfaction Survey</h2>

    <p>Rate our service (1-5):</p>
    <input type="number" id="rating" name="rating" value="4" min="1" max="5" />

    <p>Would you recommend us?</p>
    <input type="radio" name="recommend" value="yes" checked /> Yes
    <input type="radio" name="recommend" value="no" /> No

    <p>Comments:</p>
    <textarea name="comments" rows="4" style="width: 100%;">
        Great service overall!
    </textarea>
</form>

<div style="background-color: #e3f2fd; padding: 15pt; margin-top: 20pt;">
    <h3>Survey Summary</h3>
    <output form="surveyForm" for="rating" name="ratingDisplay">
        Rating: 4 out of 5 stars
    </output>
    <br/>
    <output form="surveyForm" for="recommend" name="recommendDisplay">
        Recommendation: Yes
    </output>
</div>
```

### Example 6: Multi-Page Form with Remote Output

```html
<!-- Page 1: Form inputs -->
<div style="page-break-after: always;">
    <h1>Application Form</h1>

    <form id="applicationForm">
        <h2>Personal Information</h2>
        <label>Full Name:</label>
        <input type="text" id="fullName" name="fullName" value="John Smith" />
        <br/><br/>

        <label>Email:</label>
        <input type="email" id="email" name="email" value="john@example.com" />
        <br/><br/>

        <label>Phone:</label>
        <input type="tel" id="phone" name="phone" value="(555) 123-4567" />
    </form>
</div>

<!-- Page 2: Output summary -->
<div>
    <h1>Application Summary</h1>

    <table border="1" cellpadding="10" style="width: 100%;">
        <tr>
            <td><strong>Applicant Name:</strong></td>
            <td>
                <output form="applicationForm" for="fullName">
                    John Smith
                </output>
            </td>
        </tr>
        <tr>
            <td><strong>Contact Email:</strong></td>
            <td>
                <output form="applicationForm" for="email">
                    john@example.com
                </output>
            </td>
        </tr>
        <tr>
            <td><strong>Contact Phone:</strong></td>
            <td>
                <output form="applicationForm" for="phone">
                    (555) 123-4567
                </output>
            </td>
        </tr>
    </table>
</div>
```

### Example 7: Shopping Cart with Totals

```html
<h1>Shopping Cart</h1>

<form id="cartForm">
    <table border="1" cellpadding="10" style="width: 100%;">
        <thead>
            <tr style="background-color: #336699; color: white;">
                <th>Product</th>
                <th>Price</th>
                <th>Quantity</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Laptop Computer</td>
                <td>$999.00</td>
                <td><input type="number" id="laptop_qty" name="laptop_qty" value="1" /></td>
                <td>$999.00</td>
            </tr>
            <tr>
                <td>Wireless Mouse</td>
                <td>$29.99</td>
                <td><input type="number" id="mouse_qty" name="mouse_qty" value="2" /></td>
                <td>$59.98</td>
            </tr>
            <tr>
                <td>USB Cable</td>
                <td>$9.99</td>
                <td><input type="number" id="cable_qty" name="cable_qty" value="3" /></td>
                <td>$29.97</td>
            </tr>
        </tbody>
    </table>
</form>

<div style="text-align: right; margin-top: 20pt; padding: 15pt; background-color: #f9f9f9;">
    <div style="font-size: 12pt;">
        <output form="cartForm" for="laptop_qty mouse_qty cable_qty" name="itemCount">
            Items in cart: 6
        </output>
    </div>
    <div style="font-size: 12pt; margin-top: 10pt;">
        <output form="cartForm" for="laptop_qty mouse_qty cable_qty" name="subtotal">
            Subtotal: $1,088.95
        </output>
    </div>
    <div style="font-size: 12pt; margin-top: 5pt;">
        <output form="cartForm" for="subtotal" name="shipping">
            Shipping: $15.00
        </output>
    </div>
    <div style="font-size: 14pt; font-weight: bold; margin-top: 10pt; color: #336699;">
        <output form="cartForm" for="subtotal shipping" name="grandTotal">
            Grand Total: $1,103.95
        </output>
    </div>
</div>
```

### Example 8: Data-Bound Form with Dynamic Output

```html
<!-- Model: {
    formId: "paymentForm",
    subtotal: 500.00,
    taxRate: 0.08,
    tax: 40.00,
    total: 540.00
} -->

<form id="{{model.formId}}">
    <h2>Payment Information</h2>
    <input type="text" name="cardNumber" value="**** **** **** 1234" />
    <input type="text" name="cardHolder" value="John Doe" />
</form>

<div style="margin-top: 20pt;">
    <h3>Payment Summary</h3>
    <output form="{{model.formId}}" name="subtotal">
        Subtotal: ${{model.subtotal}}
    </output>
    <br/>
    <output form="{{model.formId}}" name="tax">
        Tax ({{model.taxRate * 100}}%): ${{model.tax}}
    </output>
    <br/>
    <output form="{{model.formId}}" name="total" style="font-weight: bold; font-size: 14pt;">
        Total Due: ${{model.total}}
    </output>
</div>
```

### Example 9: Registration Form with Confirmation

```html
<h1>User Registration</h1>

<form id="registrationForm">
    <div style="margin-bottom: 15pt;">
        <label>Username:</label><br/>
        <input type="text" id="username" name="username" value="jsmith" />
    </div>

    <div style="margin-bottom: 15pt;">
        <label>Email Address:</label><br/>
        <input type="email" id="email" name="email" value="jsmith@example.com" />
    </div>

    <div style="margin-bottom: 15pt;">
        <label>Account Type:</label><br/>
        <input type="radio" name="accountType" value="free" checked /> Free
        <input type="radio" name="accountType" value="premium" /> Premium
    </div>
</form>

<div style="border: 2pt solid #336699; padding: 20pt; margin-top: 30pt; background-color: #e3f2fd;">
    <h2 style="color: #336699;">Registration Confirmation</h2>
    <p>Please verify your registration details:</p>

    <table style="width: 100%;">
        <tr>
            <td style="padding: 5pt;"><strong>Username:</strong></td>
            <td style="padding: 5pt;">
                <output form="registrationForm" for="username">jsmith</output>
            </td>
        </tr>
        <tr>
            <td style="padding: 5pt;"><strong>Email:</strong></td>
            <td style="padding: 5pt;">
                <output form="registrationForm" for="email">jsmith@example.com</output>
            </td>
        </tr>
        <tr>
            <td style="padding: 5pt;"><strong>Account Type:</strong></td>
            <td style="padding: 5pt;">
                <output form="registrationForm" for="accountType">Free Account</output>
            </td>
        </tr>
    </table>
</div>
```

### Example 10: Financial Calculator

```html
<h1>Loan Calculator</h1>

<form id="loanForm">
    <table border="0" cellpadding="10" style="width: 100%;">
        <tr>
            <td><label for="principal">Loan Amount:</label></td>
            <td><input type="number" id="principal" name="principal" value="50000" /></td>
        </tr>
        <tr>
            <td><label for="rate">Interest Rate (%):</label></td>
            <td><input type="number" id="rate" name="rate" value="5.5" step="0.1" /></td>
        </tr>
        <tr>
            <td><label for="term">Term (years):</label></td>
            <td><input type="number" id="term" name="term" value="15" /></td>
        </tr>
    </table>
</form>

<div style="background-color: #f0f0f0; padding: 20pt; margin-top: 20pt;">
    <h2>Calculation Results</h2>

    <table style="width: 100%; font-size: 12pt;">
        <tr>
            <td style="padding: 8pt;"><strong>Monthly Payment:</strong></td>
            <td style="padding: 8pt; text-align: right;">
                <output form="loanForm" for="principal rate term" name="monthly">
                    $407.39
                </output>
            </td>
        </tr>
        <tr>
            <td style="padding: 8pt;"><strong>Total Interest:</strong></td>
            <td style="padding: 8pt; text-align: right;">
                <output form="loanForm" for="principal rate term" name="totalInterest">
                    $23,330.20
                </output>
            </td>
        </tr>
        <tr style="background-color: #336699; color: white; font-weight: bold;">
            <td style="padding: 8pt;"><strong>Total Amount:</strong></td>
            <td style="padding: 8pt; text-align: right;">
                <output form="loanForm" for="principal totalInterest" name="totalAmount">
                    $73,330.20
                </output>
            </td>
        </tr>
    </table>
</div>
```

### Example 11: Nested Forms with Multiple Outputs

```html
<!-- Primary form -->
<form id="primaryForm">
    <h2>Primary Information</h2>
    <input type="text" id="field1" name="field1" value="Value A" />
    <input type="text" id="field2" name="field2" value="Value B" />
</form>

<!-- Secondary form -->
<form id="secondaryForm">
    <h2>Secondary Information</h2>
    <input type="text" id="field3" name="field3" value="Value C" />
    <input type="text" id="field4" name="field4" value="Value D" />
</form>

<!-- Outputs associated with different forms -->
<div style="margin-top: 30pt;">
    <h2>Summary</h2>

    <h3>Primary Summary</h3>
    <output form="primaryForm" for="field1 field2">
        Combined: Value A, Value B
    </output>

    <h3>Secondary Summary</h3>
    <output form="secondaryForm" for="field3 field4">
        Combined: Value C, Value D
    </output>

    <h3>Overall Summary</h3>
    <p>
        Primary: <output form="primaryForm" for="field1">Value A</output><br/>
        Secondary: <output form="secondaryForm" for="field3">Value C</output>
    </p>
</div>
```

### Example 12: Conditional Form Association

```html
<!-- Model: { isPremium: true } -->

<form id="premiumForm">
    <h2>Premium Plan Details</h2>
    <input type="text" name="features" value="All features included" />
</form>

<form id="basicForm">
    <h2>Basic Plan Details</h2>
    <input type="text" name="features" value="Limited features" />
</form>

<div style="margin-top: 20pt;">
    <h2>Selected Plan</h2>
    <output form="{{model.isPremium ? 'premiumForm' : 'basicForm'}}" for="features">
        {{model.isPremium ? 'All features included' : 'Limited features'}}
    </output>
</div>
```

### Example 13: Report with Form Filters

```html
<div style="page-break-after: always;">
    <h1>Sales Report - Configuration</h1>

    <form id="reportFilters">
        <h2>Filter Options</h2>

        <label>Region:</label>
        <input type="text" id="region" name="region" value="North America" />
        <br/><br/>

        <label>Quarter:</label>
        <input type="text" id="quarter" name="quarter" value="Q4 2024" />
        <br/><br/>

        <label>Product Category:</label>
        <input type="text" id="category" name="category" value="Electronics" />
    </form>
</div>

<div>
    <h1>Sales Report - Results</h1>

    <div style="background-color: #e3f2fd; padding: 15pt; margin-bottom: 20pt;">
        <h3>Applied Filters</h3>
        <p>
            Region: <output form="reportFilters" for="region">North America</output><br/>
            Quarter: <output form="reportFilters" for="quarter">Q4 2024</output><br/>
            Category: <output form="reportFilters" for="category">Electronics</output>
        </p>
    </div>

    <h2>Sales Data</h2>
    <p>[Report data tables would appear here]</p>

    <div style="margin-top: 40pt; text-align: center; font-size: 10pt; color: #666;">
        <output form="reportFilters" for="region quarter category">
            Report generated for North America, Q4 2024, Electronics category
        </output>
    </div>
</div>
```

### Example 14: Multi-Step Process with Form

```html
<h1>Order Process</h1>

<!-- Step 1: Order Form -->
<form id="orderProcess">
    <h2>Step 1: Order Details</h2>
    <input type="text" id="orderNum" name="orderNum" value="ORD-2024-001" />
    <input type="text" id="customer" name="customer" value="Acme Corporation" />
    <input type="number" id="amount" name="amount" value="1500.00" />
</form>

<!-- Step 2: Review (uses output elements) -->
<div style="margin-top: 30pt;">
    <h2>Step 2: Review Order</h2>
    <table border="1" cellpadding="10" style="width: 100%;">
        <tr>
            <td><strong>Order Number:</strong></td>
            <td><output form="orderProcess" for="orderNum">ORD-2024-001</output></td>
        </tr>
        <tr>
            <td><strong>Customer:</strong></td>
            <td><output form="orderProcess" for="customer">Acme Corporation</output></td>
        </tr>
        <tr>
            <td><strong>Amount:</strong></td>
            <td><output form="orderProcess" for="amount">$1,500.00</output></td>
        </tr>
    </table>
</div>

<!-- Step 3: Confirmation -->
<div style="margin-top: 30pt; background-color: #d4edda; padding: 20pt;">
    <h2>Step 3: Confirmation</h2>
    <output form="orderProcess" for="orderNum customer amount">
        Order ORD-2024-001 for Acme Corporation ($1,500.00) has been processed.
    </output>
</div>
```

### Example 15: Complex Report with Multiple Forms

```html
<h1>Financial Analysis Report</h1>

<!-- Revenue Form -->
<form id="revenueForm">
    <h2>Revenue Data</h2>
    <input type="number" id="q1rev" name="q1rev" value="125000" />
    <input type="number" id="q2rev" name="q2rev" value="145000" />
    <input type="number" id="q3rev" name="q3rev" value="138000" />
    <input type="number" id="q4rev" name="q4rev" value="155000" />
</form>

<!-- Expenses Form -->
<form id="expenseForm">
    <h2>Expense Data</h2>
    <input type="number" id="q1exp" name="q1exp" value="85000" />
    <input type="number" id="q2exp" name="q2exp" value="95000" />
    <input type="number" id="q3exp" name="q3exp" value="92000" />
    <input type="number" id="q4exp" name="q4exp" value="98000" />
</form>

<!-- Analysis Results -->
<div style="margin-top: 40pt;">
    <h2>Annual Summary</h2>

    <table border="1" cellpadding="10" style="width: 100%;">
        <tr style="background-color: #336699; color: white;">
            <th>Category</th>
            <th>Total</th>
        </tr>
        <tr>
            <td><strong>Total Revenue</strong></td>
            <td>
                <output form="revenueForm" for="q1rev q2rev q3rev q4rev" style="font-family: monospace;">
                    $563,000
                </output>
            </td>
        </tr>
        <tr>
            <td><strong>Total Expenses</strong></td>
            <td>
                <output form="expenseForm" for="q1exp q2exp q3exp q4exp" style="font-family: monospace;">
                    $370,000
                </output>
            </td>
        </tr>
        <tr style="background-color: #e3f2fd; font-weight: bold;">
            <td><strong>Net Profit</strong></td>
            <td>
                <output for="q1rev q2rev q3rev q4rev q1exp q2exp q3exp q4exp"
                        style="font-family: monospace; color: #336699;">
                    $193,000
                </output>
            </td>
        </tr>
    </table>
</div>
```

---

## See Also

- [output element](/reference/htmltags/output.html) - The output HTML element
- [form element](/reference/htmltags/form.html) - The form HTML element
- [for attribute](/reference/htmlattributes/for.html) - Associates output with form controls
- [val attribute](/reference/htmlattributes/val.html) - Output value attribute
- [name attribute](/reference/htmlattributes/name.html) - Names form elements
- [id attribute](/reference/htmlattributes/id.html) - Unique identifier
- [Data Binding](/reference/binding/) - Dynamic data binding
- [HTML Forms](/reference/forms/) - Complete form reference

---
