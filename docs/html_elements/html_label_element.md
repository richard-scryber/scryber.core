---
layout: default
title: label
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;label&gt; : The Label Element

The `<label>` element provides descriptive text for form input fields. Labels improve form readability and organization by associating descriptive text with input controls. In Scryber PDFs, labels are rendered as inline text elements that can be styled and positioned alongside form fields to create professional, well-organized form layouts.

## Usage

The `<label>` element creates descriptive text that:
- Identifies and describes form input fields
- Displays as inline text by default (like `<span>`)
- Can be styled with CSS for visual emphasis
- Supports the `for` attribute to associate with specific inputs
- Works with data binding for dynamic content
- Can contain text, images, or other inline elements
- Helps create organized, professional form layouts

```html
<!-- Simple label with input -->
<label>Name:</label>
<input type="text" value="John Doe" />

<!-- Label with for attribute -->
<label for="email-field">Email Address:</label>
<input type="text" id="email-field" value="john@example.com" />

<!-- Label wrapping input -->
<label>
    <input type="checkbox" value="checked" />
    I agree to the terms
</label>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the label. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Label-Specific Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `for` | string | ID of the associated form control (input, select, etc.). Semantic association. |
| `data-content` | string | Sets the text content of the label (data binding friendly). |

### CSS Style Support

The `<label>` element supports extensive CSS styling:

**Display and Positioning**:
- `display`: `inline` (default), `block`, `inline-block`, `none`
- `position`: `static`, `relative`, `absolute`
- `float`: `left`, `right`, `none`
- `vertical-align`: `top`, `middle`, `bottom`, `baseline`

**Sizing**:
- `width`, `min-width`, `max-width`
- `height`, `min-height`, `max-height`

**Spacing**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding` (all variants)

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color` - text color
- `text-align`: `left`, `center`, `right`, `justify`
- `line-height`, `letter-spacing`
- `text-decoration`: `none`, `underline`, `line-through`
- `text-transform`: `uppercase`, `lowercase`, `capitalize`

**Visual Effects**:
- `background-color`, `background-image`
- `border`, `border-width`, `border-color`, `border-style`
- `border-radius` - rounded corners
- `opacity`

---

## Notes

### Default Behavior

Labels have these default characteristics:
- Display: `inline` (flows with text)
- Extends `Span` component (inline container)
- No default styling (inherits from parent)
- Acts like regular text unless styled

### The `for` Attribute

The `for` attribute creates a semantic association with an input field:

```html
<label for="username">Username:</label>
<input type="text" id="username" value="jdoe" />
```

**Note**: In static PDFs, the `for` attribute is primarily semantic. It doesn't create interactive behavior (like focus on click) since PDFs are static documents. However, it's still useful for:
- Code organization and readability
- Documentation of field relationships
- Future compatibility if interactive forms are added

### Label Patterns

**1. Label before input**:
```html
<label>Name:</label>
<input type="text" value="John Doe" />
```

**2. Label with block display**:
```html
<label style="display: block; font-weight: bold; margin-bottom: 5pt;">
    Email Address:
</label>
<input type="text" value="john@example.com" style="width: 100%;" />
```

**3. Label wrapping input**:
```html
<label>
    <input type="checkbox" value="checked" />
    <span style="margin-left: 5pt;">Subscribe to newsletter</span>
</label>
```

**4. Inline label with fixed width**:
```html
<label style="display: inline-block; width: 120pt; font-weight: bold;">
    Phone:
</label>
<input type="text" value="(555) 123-4567" style="width: 200pt;" />
```

### Data Content Attribute

The `data-content` attribute allows setting label text via data binding:

```html
<!-- Model: { fieldLabel: "Customer Name" } -->
<label data-content="{{model.fieldLabel}}" style="font-weight: bold;"></label>
<input type="text" value="{{model.customerName}}" />
```

This is particularly useful when label text needs to be dynamic or localized.

### Styling Best Practices

1. **Bold labels**: Use `font-weight: bold` for emphasis
2. **Fixed-width labels**: Use `display: inline-block; width: XXpt` for alignment
3. **Block labels**: Use `display: block` for labels above inputs
4. **Consistent spacing**: Use consistent margins/padding across forms
5. **Color coding**: Use colors to indicate required fields or categories

### Common Layout Patterns

**Horizontal layout** (label beside input):
```html
<label style="display: inline-block; width: 100pt;">Name:</label>
<input type="text" value="John Doe" style="width: 200pt;" />
```

**Vertical layout** (label above input):
```html
<label style="display: block; margin-bottom: 5pt;">Name:</label>
<input type="text" value="John Doe" style="width: 100%;" />
```

**Grid layout** (using table):
```html
<table>
    <tr>
        <td><label>Name:</label></td>
        <td><input type="text" value="John Doe" /></td>
    </tr>
</table>
```

### Accessibility and Semantics

While PDFs are static and don't have interactive accessibility features, using proper `<label>` elements:
- Creates semantic document structure
- Improves code readability and maintainability
- Provides clear visual associations between labels and fields
- Follows HTML best practices

### Class Hierarchy

In the Scryber codebase:
- `HTMLLabel` extends `Span` (inline container)
- Supports `for` attribute for semantic association
- Supports `data-content` for dynamic text
- Uses `OnPreLayout` to process `data-content` attribute

---

## Examples

### Basic Labels with Inputs

```html
<!-- Simple inline label -->
<label>Name:</label>
<input type="text" value="John Doe" style="width: 200pt; margin-left: 5pt;" />

<br />

<!-- Label with styling -->
<label style="font-weight: bold; color: #336699;">Email:</label>
<input type="text" value="john@example.com" style="width: 250pt; margin-left: 5pt;" />

<br />

<!-- Label with for attribute -->
<label for="phone-input" style="font-weight: bold;">Phone:</label>
<input type="text" id="phone-input" value="(555) 123-4567"
       style="width: 150pt; margin-left: 5pt;" />
```

### Block Labels (Vertical Layout)

```html
<div style="margin-bottom: 15pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;
                  color: #333333;">
        Full Name:
    </label>
    <input type="text" value="Jane Smith" style="width: 100%; padding: 8pt;
           border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 15pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;
                  color: #333333;">
        Email Address:
    </label>
    <input type="text" value="jane.smith@example.com" style="width: 100%;
           padding: 8pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 15pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;
                  color: #333333;">
        Message:
    </label>
    <input type="text" value="Hello, I would like more information..."
           options="MultiLine"
           style="width: 100%; height: 80pt; padding: 8pt;
                  border: 1pt solid #cccccc;" />
</div>
```

### Fixed-Width Labels (Horizontal Alignment)

```html
<style>
    .form-row {
        margin-bottom: 12pt;
    }
    .form-label {
        display: inline-block;
        width: 130pt;
        font-weight: bold;
        color: #555555;
    }
    .form-input {
        width: 250pt;
        padding: 6pt;
        border: 1pt solid #cccccc;
    }
</style>

<div class="form-row">
    <label class="form-label">First Name:</label>
    <input type="text" value="John" class="form-input" />
</div>

<div class="form-row">
    <label class="form-label">Last Name:</label>
    <input type="text" value="Doe" class="form-input" />
</div>

<div class="form-row">
    <label class="form-label">Email:</label>
    <input type="text" value="john.doe@example.com" class="form-input" />
</div>

<div class="form-row">
    <label class="form-label">Phone:</label>
    <input type="text" value="(555) 123-4567" class="form-input" />
</div>

<div class="form-row">
    <label class="form-label">Address:</label>
    <input type="text" value="123 Main Street, City, ST 12345"
           style="width: 350pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>
```

### Labels with Checkboxes

```html
<!-- Checkbox with label after -->
<div style="margin-bottom: 10pt;">
    <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt;">I agree to the terms and conditions</label>
</div>

<!-- Checkbox with styled label -->
<div style="margin-bottom: 10pt;">
    <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt; color: #336699;">
        Subscribe to weekly newsletter
    </label>
</div>

<!-- Checkbox with bold label -->
<div style="margin-bottom: 10pt;">
    <input type="checkbox" value="" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt; font-weight: bold;">
        Send me promotional offers
    </label>
</div>

<!-- Label wrapping checkbox -->
<label style="display: block; margin-bottom: 10pt; cursor: pointer;">
    <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
    <span style="margin-left: 8pt;">
        I certify that all information provided is accurate
    </span>
</label>
```

### Labels with Radio Buttons

```html
<div style="margin-bottom: 15pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 8pt;
                  color: #333333;">
        Select your preferred contact method:
    </label>

    <div style="margin-left: 10pt;">
        <div style="margin-bottom: 6pt;">
            <input type="radio" value="selected" name="contact" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Email</label>
        </div>

        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" name="contact" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Phone</label>
        </div>

        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" name="contact" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Mail</label>
        </div>

        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" name="contact" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Text Message</label>
        </div>
    </div>
</div>
```

### Styled Required Field Labels

```html
<style>
    .required-label {
        font-weight: bold;
        color: #333333;
    }
    .required-label::after {
        content: " *";
        color: #d32f2f;
        font-weight: bold;
    }
</style>

<div style="margin-bottom: 12pt;">
    <label class="required-label" style="display: block; margin-bottom: 5pt;">
        Full Name
    </label>
    <input type="text" value="John Doe" style="width: 100%; padding: 8pt;
           border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 12pt;">
    <label class="required-label" style="display: block; margin-bottom: 5pt;">
        Email Address
    </label>
    <input type="text" value="john@example.com" style="width: 100%; padding: 8pt;
           border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 12pt;">
    <label style="display: block; margin-bottom: 5pt; font-weight: bold; color: #333333;">
        Company Name
        <span style="color: #d32f2f;">*</span>
    </label>
    <input type="text" value="Acme Corporation" style="width: 100%; padding: 8pt;
           border: 1pt solid #cccccc;" />
</div>

<p style="margin-top: 15pt; font-size: 9pt; color: #666666;">
    <span style="color: #d32f2f; font-weight: bold;">*</span> Required fields
</p>
```

### Color-Coded Labels

```html
<style>
    .label-primary {
        display: inline-block;
        width: 140pt;
        font-weight: bold;
        color: #1976d2;
    }
    .label-success {
        display: inline-block;
        width: 140pt;
        font-weight: bold;
        color: #388e3c;
    }
    .label-warning {
        display: inline-block;
        width: 140pt;
        font-weight: bold;
        color: #f57c00;
    }
</style>

<div style="margin-bottom: 12pt;">
    <label class="label-primary">Account Number:</label>
    <input type="text" value="ACC-123456" style="width: 200pt; padding: 6pt;
           border: 1pt solid #1976d2;" />
</div>

<div style="margin-bottom: 12pt;">
    <label class="label-success">Verified Email:</label>
    <input type="text" value="verified@example.com" style="width: 250pt; padding: 6pt;
           border: 1pt solid #388e3c;" />
</div>

<div style="margin-bottom: 12pt;">
    <label class="label-warning">Pending Approval:</label>
    <input type="text" value="Under Review" style="width: 200pt; padding: 6pt;
           border: 1pt solid #f57c00; background-color: #fff3e0;" />
</div>
```

### Labels with Icons or Symbols

```html
<div style="margin-bottom: 12pt;">
    <label style="font-weight: bold;">
        ✉ Email Address:
    </label>
    <input type="text" value="contact@example.com" style="width: 250pt;
           margin-left: 10pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 12pt;">
    <label style="font-weight: bold;">
        📞 Phone Number:
    </label>
    <input type="text" value="(555) 123-4567" style="width: 200pt;
           margin-left: 10pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 12pt;">
    <label style="font-weight: bold;">
        🏢 Company:
    </label>
    <input type="text" value="Acme Corporation" style="width: 250pt;
           margin-left: 10pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>
```

### Labels with Background Colors

```html
<style>
    .badge-label {
        display: inline-block;
        background-color: #336699;
        color: white;
        padding: 6pt 12pt;
        border-radius: 3pt;
        font-weight: bold;
        font-size: 10pt;
        width: 120pt;
        text-align: center;
    }
</style>

<div style="margin-bottom: 15pt;">
    <label class="badge-label">USERNAME</label>
    <input type="text" value="jsmith2024" style="width: 200pt; margin-left: 10pt;
           padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 15pt;">
    <label class="badge-label">PASSWORD</label>
    <input type="password" value="********" style="width: 200pt; margin-left: 10pt;
           padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 15pt;">
    <label class="badge-label" style="background-color: #28a745;">VERIFIED</label>
    <input type="text" value="verified@email.com" style="width: 250pt; margin-left: 10pt;
           padding: 6pt; border: 1pt solid #28a745;" />
</div>
```

### Data-Bound Labels

```html
<!-- Model: {
    fields: {
        nameLabel: "Customer Name",
        emailLabel: "Email Address",
        phoneLabel: "Phone Number"
    },
    customer: {
        name: "Alice Johnson",
        email: "alice@example.com",
        phone: "(555) 999-8888"
    }
} -->

<div style="margin-bottom: 12pt;">
    <label data-content="{{model.fields.nameLabel}}"
           style="display: inline-block; width: 150pt; font-weight: bold;"></label>
    <input type="text" value="{{model.customer.name}}"
           style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 12pt;">
    <label data-content="{{model.fields.emailLabel}}"
           style="display: inline-block; width: 150pt; font-weight: bold;"></label>
    <input type="text" value="{{model.customer.email}}"
           style="width: 300pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 12pt;">
    <label data-content="{{model.fields.phoneLabel}}"
           style="display: inline-block; width: 150pt; font-weight: bold;"></label>
    <input type="text" value="{{model.customer.phone}}"
           style="width: 200pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>
```

### Two-Column Form Layout

```html
<style>
    .two-col-form {
        width: 100%;
    }
    .form-col {
        display: inline-block;
        width: 48%;
        vertical-align: top;
        margin-right: 2%;
    }
    .field-group {
        margin-bottom: 15pt;
    }
    .field-label {
        display: block;
        font-weight: bold;
        margin-bottom: 5pt;
        color: #444444;
    }
    .field-input {
        width: 100%;
        padding: 8pt;
        border: 1pt solid #cccccc;
        border-radius: 3pt;
    }
</style>

<div class="two-col-form">
    <div class="form-col">
        <div class="field-group">
            <label class="field-label">First Name</label>
            <input type="text" value="John" class="field-input" />
        </div>

        <div class="field-group">
            <label class="field-label">Email</label>
            <input type="text" value="john@example.com" class="field-input" />
        </div>

        <div class="field-group">
            <label class="field-label">City</label>
            <input type="text" value="Chicago" class="field-input" />
        </div>
    </div>

    <div class="form-col">
        <div class="field-group">
            <label class="field-label">Last Name</label>
            <input type="text" value="Doe" class="field-input" />
        </div>

        <div class="field-group">
            <label class="field-label">Phone</label>
            <input type="text" value="(555) 123-4567" class="field-input" />
        </div>

        <div class="field-group">
            <label class="field-label">ZIP Code</label>
            <input type="text" value="60601" class="field-input" />
        </div>
    </div>
</div>
```

### Table-Based Form Layout

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="width: 30%; padding: 8pt; border: none;">
            <label style="font-weight: bold; color: #555555;">First Name:</label>
        </td>
        <td style="width: 70%; padding: 8pt; border: none;">
            <input type="text" value="John" style="width: 100%; padding: 6pt;
                   border: 1pt solid #cccccc;" />
        </td>
    </tr>
    <tr>
        <td style="padding: 8pt; border: none;">
            <label style="font-weight: bold; color: #555555;">Last Name:</label>
        </td>
        <td style="padding: 8pt; border: none;">
            <input type="text" value="Doe" style="width: 100%; padding: 6pt;
                   border: 1pt solid #cccccc;" />
        </td>
    </tr>
    <tr>
        <td style="padding: 8pt; border: none;">
            <label style="font-weight: bold; color: #555555;">Email Address:</label>
        </td>
        <td style="padding: 8pt; border: none;">
            <input type="text" value="john.doe@example.com" style="width: 100%;
                   padding: 6pt; border: 1pt solid #cccccc;" />
        </td>
    </tr>
    <tr>
        <td style="padding: 8pt; border: none;">
            <label style="font-weight: bold; color: #555555;">Phone Number:</label>
        </td>
        <td style="padding: 8pt; border: none;">
            <input type="text" value="(555) 123-4567" style="width: 100%;
                   padding: 6pt; border: 1pt solid #cccccc;" />
        </td>
    </tr>
    <tr>
        <td style="padding: 8pt; border: none; vertical-align: top;">
            <label style="font-weight: bold; color: #555555;">Comments:</label>
        </td>
        <td style="padding: 8pt; border: none;">
            <input type="text" value="Please contact me between 9am-5pm EST"
                   options="MultiLine"
                   style="width: 100%; height: 60pt; padding: 6pt;
                          border: 1pt solid #cccccc;" />
        </td>
    </tr>
</table>
```

### Professional Invoice Form

```html
<style>
    .invoice-section {
        margin-bottom: 20pt;
        padding: 15pt;
        border: 1pt solid #e0e0e0;
        background-color: #fafafa;
    }
    .invoice-header {
        font-weight: bold;
        font-size: 13pt;
        color: #336699;
        border-bottom: 2pt solid #336699;
        padding-bottom: 8pt;
        margin-bottom: 15pt;
    }
    .invoice-field {
        margin-bottom: 10pt;
    }
    .invoice-label {
        display: inline-block;
        width: 140pt;
        font-weight: bold;
        color: #555555;
    }
</style>

<div class="invoice-section">
    <div class="invoice-header">Bill To</div>

    <div class="invoice-field">
        <label class="invoice-label">Company Name:</label>
        <input type="text" value="Acme Corporation" style="width: 300pt; padding: 6pt;
               border: 1pt solid #cccccc; background-color: white;" />
    </div>

    <div class="invoice-field">
        <label class="invoice-label">Contact Person:</label>
        <input type="text" value="Jane Smith" style="width: 250pt; padding: 6pt;
               border: 1pt solid #cccccc; background-color: white;" />
    </div>

    <div class="invoice-field">
        <label class="invoice-label">Address:</label>
        <input type="text" value="456 Business Ave, Suite 200" style="width: 350pt;
               padding: 6pt; border: 1pt solid #cccccc; background-color: white;" />
    </div>

    <div class="invoice-field">
        <label class="invoice-label">City, State ZIP:</label>
        <input type="text" value="New York, NY 10001" style="width: 250pt; padding: 6pt;
               border: 1pt solid #cccccc; background-color: white;" />
    </div>

    <div class="invoice-field">
        <label class="invoice-label">Email:</label>
        <input type="text" value="billing@acme.com" style="width: 280pt; padding: 6pt;
               border: 1pt solid #cccccc; background-color: white;" />
    </div>
</div>

<div class="invoice-section">
    <div class="invoice-header">Invoice Details</div>

    <div class="invoice-field">
        <label class="invoice-label">Invoice Number:</label>
        <input type="text" value="INV-2024-0123" style="width: 180pt; padding: 6pt;
               border: 1pt solid #cccccc; background-color: white;
               font-family: monospace; font-weight: bold;" />
    </div>

    <div class="invoice-field">
        <label class="invoice-label">Invoice Date:</label>
        <input type="text" value="January 15, 2024" style="width: 180pt; padding: 6pt;
               border: 1pt solid #cccccc; background-color: white;" />
    </div>

    <div class="invoice-field">
        <label class="invoice-label">Due Date:</label>
        <input type="text" value="February 15, 2024" style="width: 180pt; padding: 6pt;
               border: 1pt solid #cccccc; background-color: white;
               color: #d32f2f; font-weight: bold;" />
    </div>

    <div class="invoice-field">
        <label class="invoice-label">Payment Terms:</label>
        <input type="text" value="Net 30" style="width: 120pt; padding: 6pt;
               border: 1pt solid #cccccc; background-color: white;" />
    </div>
</div>
```

### Application Form with Sections

```html
<h2 style="color: #336699; border-bottom: 3pt solid #336699; padding-bottom: 10pt;">
    Employment Application
</h2>

<fieldset style="border: 2pt solid #e0e0e0; padding: 20pt; margin-bottom: 20pt;">
    <legend style="font-weight: bold; font-size: 13pt; color: #336699; padding: 0 10pt;">
        Personal Information
    </legend>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Full Legal Name:
        </label>
        <input type="text" value="Michael James Anderson" style="width: 100%;
               padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <div style="display: inline-block; width: 48%; margin-right: 2%;">
            <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
                Date of Birth:
            </label>
            <input type="text" value="03/22/1985" style="width: 100%; padding: 8pt;
                   border: 1pt solid #cccccc;" />
        </div>

        <div style="display: inline-block; width: 48%;">
            <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
                Social Security Number:
            </label>
            <input type="text" value="***-**-1234" style="width: 100%; padding: 8pt;
                   border: 1pt solid #cccccc; font-family: monospace;" />
        </div>
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Home Address:
        </label>
        <input type="text" value="789 Residential Lane, Apt 4B" style="width: 100%;
               padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <div style="display: inline-block; width: 48%; margin-right: 2%;">
            <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
                Email Address:
            </label>
            <input type="text" value="m.anderson@email.com" style="width: 100%;
                   padding: 8pt; border: 1pt solid #cccccc;" />
        </div>

        <div style="display: inline-block; width: 48%;">
            <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
                Phone Number:
            </label>
            <input type="text" value="(555) 777-8888" style="width: 100%;
                   padding: 8pt; border: 1pt solid #cccccc;" />
        </div>
    </div>
</fieldset>

<fieldset style="border: 2pt solid #e0e0e0; padding: 20pt; margin-bottom: 20pt;">
    <legend style="font-weight: bold; font-size: 13pt; color: #336699; padding: 0 10pt;">
        Employment History
    </legend>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Current/Most Recent Employer:
        </label>
        <input type="text" value="Tech Solutions Inc." style="width: 100%;
               padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Job Title:
        </label>
        <input type="text" value="Senior Software Engineer" style="width: 100%;
               padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <div style="display: inline-block; width: 48%; margin-right: 2%;">
            <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
                Start Date:
            </label>
            <input type="text" value="January 2018" style="width: 100%; padding: 8pt;
                   border: 1pt solid #cccccc;" />
        </div>

        <div style="display: inline-block; width: 48%;">
            <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
                End Date:
            </label>
            <input type="text" value="Present" style="width: 100%; padding: 8pt;
                   border: 1pt solid #cccccc;" />
        </div>
    </div>

    <div style="margin-bottom: 0;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Responsibilities and Achievements:
        </label>
        <input type="text"
               value="Led development team of 5 engineers. Architected microservices platform that improved system performance by 40%. Mentored junior developers and conducted code reviews."
               options="MultiLine"
               style="width: 100%; height: 80pt; padding: 8pt;
                      border: 1pt solid #cccccc; font-size: 10pt;" />
    </div>
</fieldset>
```

### Dynamic Form with Repeating Fields

```html
<!-- Model: {
    formFields: [
        {label: "First Name", value: "John", required: true},
        {label: "Last Name", value: "Doe", required: true},
        {label: "Email", value: "john@example.com", required: true},
        {label: "Company", value: "Acme Corp", required: false}
    ]
} -->

<h3 style="color: #336699; margin-bottom: 15pt;">Contact Information</h3>

<template data-bind="{{model.formFields}}">
    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            {{.label}}{{.required ? ' *' : ''}}
        </label>
        <input type="text" value="{{.value}}"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>
</template>

<p style="margin-top: 15pt; font-size: 9pt; color: #666666;">
    <span style="color: #d32f2f; font-weight: bold;">*</span> Required fields
</p>
```

### Compact Inline Form

```html
<div style="padding: 15pt; border: 1pt solid #e0e0e0; background-color: #f9f9f9;">
    <h3 style="margin-top: 0; color: #336699;">Quick Contact</h3>

    <div style="margin-bottom: 10pt;">
        <label style="display: inline-block; width: 80pt; font-weight: bold;">Name:</label>
        <input type="text" value="Jane Doe" style="width: 200pt; padding: 5pt;
               border: 1pt solid #cccccc; margin-right: 15pt;" />

        <label style="display: inline-block; width: 80pt; font-weight: bold;">Email:</label>
        <input type="text" value="jane@example.com" style="width: 220pt; padding: 5pt;
               border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 0;">
        <label style="display: inline-block; width: 80pt; font-weight: bold;">Message:</label>
        <input type="text" value="I'm interested in your services"
               style="width: 468pt; padding: 5pt; border: 1pt solid #cccccc;" />
    </div>
</div>
```

---

## See Also

- [input](/reference/htmltags/input.html) - Input field element
- [fieldset](/reference/htmltags/fieldset.html) - Fieldset and legend for grouping form elements
- [form](/reference/htmltags/form.html) - Form container element
- [span](/reference/htmltags/span.html) - Generic inline container
- [div](/reference/htmltags/div.html) - Generic block container
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
