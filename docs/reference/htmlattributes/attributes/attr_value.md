---
layout: default
title: value
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @value : The Value Attribute

The `value` attribute specifies the current value or content displayed in form input fields. In Scryber PDFs, the value represents the static data shown in the rendered document. This attribute is essential for populating form fields with data from your application or data binding expressions.

## Usage

The `value` attribute defines what content is displayed in input fields:
- Sets the text content for text-based inputs
- Controls checked/selected state for checkboxes and radio buttons
- Defines button text for button-type inputs
- Supports data binding for dynamic content
- Renders as static content in PDF output
- Can contain plain text, formatted text, or bound expressions
- Works with all input types

```html
<!-- Static text value -->
<input type="text" value="John Doe" />

<!-- Data-bound value -->
<input type="text" value="{{model.customerName}}" />

<!-- Checkbox value (non-empty = checked) -->
<input type="checkbox" value="checked" />

<!-- Button value (button text) -->
<input type="button" value="Click Me" />
```

---

## Supported Elements

The `value` attribute is supported by the following elements:

| Element | Description |
|---------|-------------|
| `<input>` | Form input field element - all types |

---

## Value Behavior by Input Type

### Text-Based Input Types

For text, password, email, url, tel, search, number, date, time, and similar types:

```html
<!-- Text input: displays the value as text -->
<input type="text" value="John Doe" />

<!-- Email input: displays email address -->
<input type="email" value="john@example.com" />

<!-- Number input: displays numeric value -->
<input type="number" value="42" />

<!-- Date input: displays formatted date -->
<input type="date" value="2024-01-15" />
```

**Behavior**: The value is displayed as text content within the input field.

### Checkbox Input Type

For checkboxes, the value controls the checked state:

```html
<!-- Checked checkbox: non-empty value -->
<input type="checkbox" value="checked" />
<input type="checkbox" value="1" />
<input type="checkbox" value="true" />
<input type="checkbox" value="yes" />

<!-- Unchecked checkbox: empty or omitted value -->
<input type="checkbox" value="" />
<input type="checkbox" />
```

**Behavior**:
- Non-empty value = checked (shows ✓ or ☑)
- Empty/null/omitted value = unchecked (shows ☐)
- The specific non-empty text doesn't matter, only that it's non-empty

### Radio Button Input Type

For radio buttons, the value controls the selected state:

```html
<!-- Selected radio button: non-empty value -->
<input type="radio" value="selected" name="option" />
<input type="radio" value="1" name="option" />
<input type="radio" value="true" name="option" />

<!-- Unselected radio button: empty or omitted value -->
<input type="radio" value="" name="option" />
<input type="radio" name="option" />
```

**Behavior**:
- Non-empty value = selected (shows ⦿ filled circle)
- Empty/null/omitted value = unselected (shows ○ empty circle)

### Button Input Types

For buttons, the value defines the button text:

```html
<!-- Button with text -->
<input type="button" value="Click Here" />
<input type="submit" value="Submit Form" />
<input type="reset" value="Reset" />
```

**Behavior**: The value is displayed as the button label text.

---

## Binding Values

The `value` attribute is commonly used with data binding to display dynamic content:

### Simple Value Binding

```html
<!-- Model: { name: "John Doe" } -->
<input type="text" value="{{model.name}}" />
```

### Nested Property Binding

```html
<!-- Model: { customer: { firstName: "John", lastName: "Doe" } } -->
<input type="text" value="{{model.customer.firstName}}" />
<input type="text" value="{{model.customer.lastName}}" />
```

### Expression Binding

```html
<!-- Model: { firstName: "John", lastName: "Doe" } -->
<input type="text" value="{{model.firstName + ' ' + model.lastName}}" />
```

### Conditional Value Binding

```html
<!-- Model: { isActive: true } -->
<input type="text" value="{{model.isActive ? 'Active' : 'Inactive'}}" />
```

### Checkbox State Binding

```html
<!-- Model: { agreeTerms: true, newsletter: false } -->
<input type="checkbox" value="{{model.agreeTerms ? 'checked' : ''}}" />
<input type="checkbox" value="{{model.newsletter ? 'checked' : ''}}" />
```

### Formatted Value Binding

```html
<!-- Model: { price: 1234.56, date: "2024-01-15" } -->
<input type="text" value="${{model.price.toFixed(2)}}" />
<input type="date" value="{{model.date}}" />
```

### Array Item Binding in Templates

```html
<!-- Model: { items: [{name: "Item 1", price: 10.00}, ...] } -->
<template data-bind="{{model.items}}">
    <div>
        <input type="text" value="{{.name}}" />
        <input type="text" value="${{.price}}" />
    </div>
</template>
```

---

## Notes

### Empty Values

An empty value attribute has different meanings for different input types:

```html
<!-- Text input: displays empty field -->
<input type="text" value="" />

<!-- Checkbox: unchecked state -->
<input type="checkbox" value="" />

<!-- Radio button: unselected state -->
<input type="radio" value="" />

<!-- Button: displays empty button (not recommended) -->
<input type="button" value="" />
```

### Multi-line Text Values

For multi-line text content, use line break encoding:

```html
<!-- Using HTML entity for line break -->
<input type="text" value="Line 1&#10;Line 2&#10;Line 3"
       options="MultiLine" style="height: 80pt;" />

<!-- Using \n in data binding -->
<!-- Model: { description: "Line 1\nLine 2\nLine 3" } -->
<input type="text" value="{{model.description}}"
       options="MultiLine" style="height: 80pt;" />
```

### Special Characters in Values

HTML entities can be used in values:

```html
<!-- Special characters -->
<input type="text" value="Price: $1,234.56 &amp; up" />
<input type="text" value="Email: info@company.com" />
<input type="text" value="Less than &lt; 100" />
<input type="text" value="Greater than &gt; 50" />
<input type="text" value="Quote: &quot;Hello&quot;" />
<input type="text" value="Apostrophe: &apos;Hi&apos;" />
```

### Default Value Fallback

If the `value` attribute is omitted, the field renders empty:

```html
<!-- No value specified: empty field -->
<input type="text" />

<!-- Same as -->
<input type="text" value="" />
```

The `default-value` attribute can provide a fallback:

```html
<!-- Uses default-value if value is empty -->
<input type="text" value="" default-value="N/A" />
```

### Value vs. Data Content

For text display (not form fields), use text content or `data-content`:

```html
<!-- Label with text content -->
<label>Name:</label>

<!-- Label with data-content attribute -->
<label data-content="{{model.labelText}}"></label>
```

The `value` attribute is specifically for input fields.

### Password Values

Password type inputs display the value as plain text in PDFs:

```html
<!-- Displays as plain text (not masked) -->
<input type="password" value="secret123" />
```

**Security Note**: PDFs are static documents. Password values are visible in the rendered PDF. Consider masking or omitting sensitive data:

```html
<!-- Masked password display -->
<input type="password" value="••••••••" />

<!-- Or use data binding to conditionally show -->
<!-- Model: { showPassword: false, password: "secret" } -->
<input type="password" value="{{model.showPassword ? model.password : '••••••••'}}" />
```

### Numeric Values

Number inputs can format numeric values:

```html
<!-- Integer value -->
<input type="number" value="42" />

<!-- Decimal value -->
<input type="number" value="123.45" />

<!-- With data binding and formatting -->
<!-- Model: { price: 1234.5678 } -->
<input type="number" value="{{model.price.toFixed(2)}}" />
```

### Date and Time Values

Date and time inputs use specific formats:

```html
<!-- Date format: YYYY-MM-DD -->
<input type="date" value="2024-01-15" />

<!-- Time format: HH:MM -->
<input type="time" value="14:30" />

<!-- DateTime format: YYYY-MM-DDTHH:MM -->
<input type="datetime-local" value="2024-01-15T14:30" />

<!-- Month format: YYYY-MM -->
<input type="month" value="2024-01" />

<!-- Week format: YYYY-W## -->
<input type="week" value="2024-W03" />
```

### URL and Email Values

URL and email inputs validate format (for semantic purposes):

```html
<!-- URL value -->
<input type="url" value="https://www.example.com" />

<!-- Email value -->
<input type="email" value="user@example.com" />
```

**Note**: In PDFs, these render as text. Format validation occurs during PDF generation, not in the viewer.

### Value Persistence

Values in Scryber PDFs are static and don't persist user changes:
- PDFs display the values at generation time
- Values cannot be edited in PDF viewers (static documents)
- Perfect for reports, invoices, completed forms, and records

---

## Examples

### Basic Text Input Values

```html
<!-- Simple text value -->
<label style="display: inline-block; width: 100pt; font-weight: bold;">Name:</label>
<input type="text" value="John Doe" style="width: 250pt; padding: 6pt;
       border: 1pt solid #cccccc;" />

<!-- Empty value -->
<label style="display: inline-block; width: 100pt; font-weight: bold;">Middle Name:</label>
<input type="text" value="" style="width: 250pt; padding: 6pt;
       border: 1pt solid #cccccc; background-color: #f9f9f9;" />

<!-- Long text value -->
<label style="display: block; font-weight: bold; margin-bottom: 5pt;">Address:</label>
<input type="text" value="1234 Main Street, Apartment 567, Building C"
       style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
```

### Data-Bound Values

```html
<!-- Model: {
    customer: {
        firstName: "Alice",
        lastName: "Johnson",
        email: "alice.johnson@email.com",
        phone: "(555) 123-4567",
        memberSince: "2020-03-15"
    }
} -->

<h3 style="color: #336699;">Customer Information</h3>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 130pt; font-weight: bold;">
        First Name:
    </label>
    <input type="text" value="{{model.customer.firstName}}"
           style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 130pt; font-weight: bold;">
        Last Name:
    </label>
    <input type="text" value="{{model.customer.lastName}}"
           style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 130pt; font-weight: bold;">
        Email:
    </label>
    <input type="email" value="{{model.customer.email}}"
           style="width: 300pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 130pt; font-weight: bold;">
        Phone:
    </label>
    <input type="tel" value="{{model.customer.phone}}"
           style="width: 200pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 130pt; font-weight: bold;">
        Member Since:
    </label>
    <input type="date" value="{{model.customer.memberSince}}"
           style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>
```

### Checkbox Values

```html
<!-- Checked checkboxes -->
<div style="margin-bottom: 8pt;">
    <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt;">Email notifications</label>
</div>

<div style="margin-bottom: 8pt;">
    <input type="checkbox" value="1" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt;">SMS alerts</label>
</div>

<div style="margin-bottom: 8pt;">
    <input type="checkbox" value="yes" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt;">Newsletter subscription</label>
</div>

<!-- Unchecked checkboxes -->
<div style="margin-bottom: 8pt;">
    <input type="checkbox" value="" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt;">Marketing emails</label>
</div>

<div style="margin-bottom: 8pt;">
    <input type="checkbox" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt;">Third-party offers</label>
</div>
```

### Radio Button Values

```html
<label style="font-weight: bold; display: block; margin-bottom: 8pt;">
    Select your plan:
</label>

<div style="margin-left: 15pt;">
    <div style="margin-bottom: 6pt;">
        <input type="radio" value="" name="plan" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Basic ($9.99/month)</label>
    </div>

    <div style="margin-bottom: 6pt;">
        <input type="radio" value="selected" name="plan" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt; font-weight: bold; color: #336699;">
            Standard ($19.99/month) - Selected
        </label>
    </div>

    <div style="margin-bottom: 6pt;">
        <input type="radio" value="" name="plan" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Premium ($39.99/month)</label>
    </div>

    <div style="margin-bottom: 6pt;">
        <input type="radio" value="" name="plan" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Enterprise (Custom pricing)</label>
    </div>
</div>
```

### Button Values

```html
<!-- Button with different text values -->
<div style="text-align: center; padding: 20pt;">
    <input type="button" value="Save Changes"
           style="padding: 10pt 25pt; border: none;
                  background-color: #28a745; color: white; font-weight: bold;
                  border-radius: 5pt; margin-right: 10pt;" />

    <input type="button" value="Cancel"
           style="padding: 10pt 25pt; border: 1pt solid #999999;
                  background-color: white; color: #333333; font-weight: bold;
                  border-radius: 5pt; margin-right: 10pt;" />

    <input type="submit" value="Submit Form"
           style="padding: 10pt 25pt; border: none;
                  background-color: #336699; color: white; font-weight: bold;
                  border-radius: 5pt;" />
</div>
```

### Multi-line Text Values

```html
<!-- Multi-line text with line breaks -->
<label style="display: block; font-weight: bold; margin-bottom: 5pt;">
    Project Description:
</label>
<input type="text"
       value="Project Alpha is a comprehensive initiative to modernize our infrastructure.&#10;&#10;Key objectives include:&#10;- Cloud migration&#10;- Performance optimization&#10;- Security enhancements&#10;&#10;Expected completion: Q2 2024"
       options="MultiLine"
       style="width: 100%; height: 120pt; padding: 10pt;
              border: 1pt solid #cccccc; font-size: 10pt;" />

<!-- Another multi-line example -->
<label style="display: block; font-weight: bold; margin-bottom: 5pt; margin-top: 15pt;">
    Comments:
</label>
<input type="text"
       value="The customer was very satisfied with the service.&#10;Follow-up scheduled for next month.&#10;&#10;Additional notes: Priority customer, eligible for discount."
       options="MultiLine"
       style="width: 100%; height: 100pt; padding: 10pt;
              border: 1pt solid #cccccc; font-size: 10pt;" />
```

### Formatted Numeric Values

```html
<!-- Integer values -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Quantity:
    </label>
    <input type="number" value="250"
           style="width: 100pt; padding: 6pt; border: 1pt solid #cccccc;
                  text-align: right; font-weight: bold;" />
</div>

<!-- Decimal values -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Unit Price:
    </label>
    <input type="number" value="19.99"
           style="width: 120pt; padding: 6pt; border: 1pt solid #cccccc;
                  text-align: right; font-family: monospace;" />
</div>

<!-- Currency values -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Total Amount:
    </label>
    <input type="text" value="$4,997.50"
           style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;
                  text-align: right; font-weight: bold; font-size: 12pt;
                  background-color: #ffffcc;" />
</div>

<!-- Percentage values -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Discount:
    </label>
    <input type="text" value="15%"
           style="width: 80pt; padding: 6pt; border: 1pt solid #cccccc;
                  text-align: center; color: #d32f2f; font-weight: bold;" />
</div>
```

### Date and Time Values

```html
<!-- Date values -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 130pt; font-weight: bold;">
        Order Date:
    </label>
    <input type="date" value="2024-01-15"
           style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 130pt; font-weight: bold;">
        Delivery Date:
    </label>
    <input type="date" value="2024-01-22"
           style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<!-- Time values -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 130pt; font-weight: bold;">
        Appointment Time:
    </label>
    <input type="time" value="14:30"
           style="width: 120pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<!-- DateTime values -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 130pt; font-weight: bold;">
        Event Start:
    </label>
    <input type="datetime-local" value="2024-01-20T09:00"
           style="width: 200pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<!-- Month and week values -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 130pt; font-weight: bold;">
        Billing Period:
    </label>
    <input type="month" value="2024-01"
           style="width: 120pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>
```

### Special Characters in Values

```html
<!-- HTML entities -->
<div style="margin-bottom: 10pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Company Name:
    </label>
    <input type="text" value="Smith &amp; Johnson Inc."
           style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Product Code:
    </label>
    <input type="text" value="PROD-&lt;2024&gt;-001"
           style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;
                  font-family: monospace;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Quoted Text:
    </label>
    <input type="text" value="The CEO said &quot;Innovation is key&quot; in the meeting"
           style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
</div>
```

### Conditional Values with Data Binding

```html
<!-- Model: {
    status: "active",
    balance: 1500.00,
    isPremium: true,
    lastLogin: "2024-01-15T10:30:00"
} -->

<!-- Status display -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Status:
    </label>
    <input type="text"
           value="{{model.status === 'active' ? 'Active' : 'Inactive'}}"
           style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;
                  background-color: {{model.status === 'active' ? '#d4edda' : '#f8d7da'}};
                  color: {{model.status === 'active' ? '#155724' : '#721c24'}};
                  font-weight: bold; text-align: center;" />
</div>

<!-- Balance display with formatting -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Balance:
    </label>
    <input type="text"
           value="${{model.balance.toFixed(2)}}"
           style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;
                  text-align: right; font-family: monospace;
                  color: {{model.balance >= 0 ? '#155724' : '#721c24'}};" />
</div>

<!-- Membership tier -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Membership:
    </label>
    <input type="text"
           value="{{model.isPremium ? 'Premium Member' : 'Standard Member'}}"
           style="width: 200pt; padding: 6pt; border: 1pt solid #cccccc;
                  background-color: {{model.isPremium ? '#fff3cd' : '#ffffff'}};
                  font-weight: {{model.isPremium ? 'bold' : 'normal'}};" />
</div>

<!-- Premium badge checkbox -->
<div style="margin-bottom: 10pt;">
    <input type="checkbox"
           value="{{model.isPremium ? 'checked' : ''}}"
           style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt; font-weight: bold; color: #f39c12;">
        Premium Features Enabled
    </label>
</div>
```

### Dynamic Form with Repeating Values

```html
<!-- Model: {
    contacts: [
        {name: "John Doe", email: "john@example.com", phone: "(555) 111-1111", preferred: true},
        {name: "Jane Smith", email: "jane@example.com", phone: "(555) 222-2222", preferred: false},
        {name: "Bob Johnson", email: "bob@example.com", phone: "(555) 333-3333", preferred: true}
    ]
} -->

<h3 style="color: #336699;">Contact List</h3>

<template data-bind="{{model.contacts}}">
    <div style="margin-bottom: 15pt; padding: 12pt; border: 1pt solid #e0e0e0;
                background-color: #f9f9f9;">
        <div style="margin-bottom: 8pt;">
            <label style="display: inline-block; width: 100pt; font-weight: bold;">
                Name:
            </label>
            <input type="text" value="{{.name}}"
                   style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>

        <div style="margin-bottom: 8pt;">
            <label style="display: inline-block; width: 100pt; font-weight: bold;">
                Email:
            </label>
            <input type="email" value="{{.email}}"
                   style="width: 280pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>

        <div style="margin-bottom: 8pt;">
            <label style="display: inline-block; width: 100pt; font-weight: bold;">
                Phone:
            </label>
            <input type="tel" value="{{.phone}}"
                   style="width: 180pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>

        <div>
            <input type="checkbox" value="{{.preferred ? 'checked' : ''}}"
                   style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Preferred contact</label>
        </div>
    </div>
</template>
```

### Invoice Form with Calculated Values

```html
<!-- Model: {
    invoice: {
        number: "INV-2024-001",
        date: "2024-01-15",
        customer: "Acme Corporation",
        subtotal: 1250.00,
        taxRate: 0.08,
        tax: 100.00,
        total: 1350.00
    }
} -->

<div style="padding: 20pt; border: 2pt solid #336699;">
    <h2 style="color: #336699; margin-top: 0;">INVOICE</h2>

    <div style="margin-bottom: 12pt;">
        <label style="display: inline-block; width: 130pt; font-weight: bold;">
            Invoice Number:
        </label>
        <input type="text" value="{{model.invoice.number}}"
               style="width: 180pt; padding: 6pt; border: 1pt solid #cccccc;
                      font-family: monospace; font-weight: bold;
                      background-color: #ffffcc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: inline-block; width: 130pt; font-weight: bold;">
            Date:
        </label>
        <input type="date" value="{{model.invoice.date}}"
               style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 20pt;">
        <label style="display: inline-block; width: 130pt; font-weight: bold;">
            Customer:
        </label>
        <input type="text" value="{{model.invoice.customer}}"
               style="width: 350pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>

    <hr style="border: 1pt solid #e0e0e0; margin: 20pt 0;" />

    <div style="margin-bottom: 10pt;">
        <label style="display: inline-block; width: 130pt; font-weight: bold;">
            Subtotal:
        </label>
        <input type="text" value="${{model.invoice.subtotal.toFixed(2)}}"
               style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;
                      text-align: right; font-family: monospace;" />
    </div>

    <div style="margin-bottom: 10pt;">
        <label style="display: inline-block; width: 130pt; font-weight: bold;">
            Tax ({{model.invoice.taxRate * 100}}%):
        </label>
        <input type="text" value="${{model.invoice.tax.toFixed(2)}}"
               style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;
                      text-align: right; font-family: monospace;" />
    </div>

    <div style="margin-bottom: 10pt;">
        <label style="display: inline-block; width: 130pt; font-weight: bold;
                      font-size: 13pt;">
            TOTAL:
        </label>
        <input type="text" value="${{model.invoice.total.toFixed(2)}}"
               style="width: 150pt; padding: 8pt; border: 2pt solid #336699;
                      text-align: right; font-family: monospace; font-size: 14pt;
                      font-weight: bold; background-color: #e3f2fd;" />
    </div>
</div>
```

### Password Field Values

```html
<!-- Plain text password (visible) -->
<div style="margin-bottom: 12pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Password (Visible):
    </label>
    <input type="password" value="MySecretP@ssw0rd"
           style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;
                  font-family: monospace;" />
    <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #d32f2f;">
        Warning: Password displayed as entered in PDF
    </p>
</div>

<!-- Masked password (recommended) -->
<div style="margin-bottom: 12pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Password (Masked):
    </label>
    <input type="password" value="••••••••••••"
           style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;
                  font-family: monospace;" />
</div>

<!-- Asterisk masked -->
<div style="margin-bottom: 12pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Password (Asterisks):
    </label>
    <input type="password" value="************"
           style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;
                  font-family: monospace; letter-spacing: 3pt;" />
</div>
```

### File Upload Values

```html
<!-- Single file -->
<div style="margin-bottom: 15pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Resume:
    </label>
    <input type="file" value="john_doe_resume.pdf"
           style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;
                  background-color: #f5f5f5; font-family: monospace;" />
</div>

<!-- Multiple file references -->
<div style="margin-bottom: 15pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Supporting Documents:
    </label>
    <div style="margin-left: 15pt;">
        <div style="margin-bottom: 6pt;">
            <input type="file" value="document_1.pdf"
                   style="width: 350pt; padding: 6pt; border: 1pt solid #cccccc;
                          background-color: #fafafa; font-family: monospace;
                          font-size: 9pt;" />
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="file" value="document_2.docx"
                   style="width: 350pt; padding: 6pt; border: 1pt solid #cccccc;
                          background-color: #fafafa; font-family: monospace;
                          font-size: 9pt;" />
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="file" value="spreadsheet.xlsx"
                   style="width: 350pt; padding: 6pt; border: 1pt solid #cccccc;
                          background-color: #fafafa; font-family: monospace;
                          font-size: 9pt;" />
        </div>
    </div>
</div>
```

### Empty vs. Default Values

```html
<!-- Empty value (no default) -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Optional Field:
    </label>
    <input type="text" value=""
           style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;
                  background-color: #f9f9f9;" />
</div>

<!-- With default-value fallback -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Country:
    </label>
    <input type="text" value="" default-value="United States"
           style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<!-- Placeholder-style empty field -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Middle Initial:
    </label>
    <input type="text" value=""
           style="width: 50pt; padding: 6pt; border: 1pt solid #cccccc;
                  background-color: #fafafa; text-align: center;" />
</div>
```

---

## See Also

- [input](/reference/htmltags/input.html) - Input field element
- [type](/reference/htmlattributes/type.html) - Input type attribute
- [name](/reference/htmlattributes/name.html) - Name attribute for form fields
- [label](/reference/htmltags/label.html) - Label element for form fields
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Template Element](/reference/htmltags/template.html) - Template for repeating content

---
