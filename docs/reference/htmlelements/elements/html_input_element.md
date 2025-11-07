---
layout: default
title: input
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;input&gt; : The Input Field Element
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---


## Summary

The `<input>` element creates form input fields in PDF documents. Since PDFs are static documents, inputs render as **visual representations** of form fields with their current values displayed. They can be styled, data-bound, and used to create form-like layouts for invoices, applications, surveys, and other documents that require formatted data display.

## Usage

The `<input>` element creates form fields that:
- Render as styled boxes displaying static values (not interactive in PDF)
- Support multiple input types: text, checkbox, radio, button, password, file
- Display current values from data binding or static content
- Can be styled with borders, padding, colors, and fonts
- Support form layouts with labels and fieldsets
- Show checkboxes and radio buttons as visual indicators (checked/unchecked)
- Enable creation of professional form templates
- Support multi-line text areas with appropriate options

```html
<!-- Basic text input with value -->
<input type="text" value="John Doe" />

<!-- Data-bound input field -->
<input type="text" value="{{model.customerName}}" />

<!-- Checkbox (checked state shown visually) -->
<input type="checkbox" value="checked" />
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and references. |
| `name` | string | Field name used for form identification and data binding. Auto-generated if not provided. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the input. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Input-Specific Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `type` | string | Input type: `text` (default), `checkbox`, `radio`, `button`, `password`, `file`. |
| `value` | string | The current/default value displayed in the field. Supports data binding. |
| `default-value` | string | Fallback value if no value is specified. |
| `options` | FormFieldOptions | Field options: `None`, `ReadOnly`, `Required`, `NoExport`, `MultiLine`, `Password`, `File`. |

### Type Attribute Values

| Type | Description | Visual Representation |
|------|-------------|----------------------|
| `text` | Single-line text input (default) | Text displayed in bordered box |
| `checkbox` | Checkbox input | Checked: shows checkmark symbol (✓ or ☑), Unchecked: empty box |
| `radio` | Radio button input | Selected: filled circle (⦿), Unselected: empty circle (○) |
| `button` | Button field | Text displayed as button |
| `password` | Password field | Text displayed (non-masked in PDF) |
| `file` | File upload field | File path or filename displayed |

### CSS Style Support

The `<input>` element supports extensive CSS styling:

**Sizing**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`
- Default: `width: 100%` (full width)

**Positioning**:
- `display`: `inline-block` (default), `block`, `inline`, `none`
- `position`: `static` (default), `relative`, `absolute`
- `float`: `left`, `right`, `none`
- `vertical-align`: `top`, `middle`, `bottom`, `baseline`

**Spacing**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding` (all variants) - default: `5pt`

**Visual Styling**:
- `border`, `border-width`, `border-color`, `border-style` - default: `1pt solid black`
- `border-radius` - rounded corners
- `background-color`, `background-image`
- `color` - text color
- `font-family`, `font-size`, `font-weight`, `font-style`

**Text Control**:
- `text-align`: `left`, `center`, `right`
- `line-height`, `letter-spacing`
- For single-line inputs: `word-wrap: nowrap` (default)
- For multi-line inputs: text wrapping enabled

---

## Notes

### Static Form Fields in PDF

Input fields in Scryber PDFs are **static visual representations**, not interactive form fields:
- Values are rendered as text content in styled boxes
- Users cannot edit the fields in the PDF viewer
- Perfect for completed forms, invoices, applications, and reports
- The visual appearance mimics HTML form fields

### Input Types and Rendering

**Text Input (type="text")**:
```html
<input type="text" value="John Doe" style="width: 200pt;" />
```
Renders as: `[John Doe____________________]` (text in bordered box)

**Checkbox (type="checkbox")**:
```html
<!-- Checked checkbox -->
<input type="checkbox" value="checked" />
<!-- Unchecked checkbox -->
<input type="checkbox" value="" />
```
Renders as: `☑` (checked) or `☐` (unchecked)

**Radio Button (type="radio")**:
```html
<!-- Selected radio -->
<input type="radio" value="selected" />
<!-- Unselected radio -->
<input type="radio" value="" />
```
Renders as: `⦿` (selected) or `○` (unselected)

**Multi-line Text (options="MultiLine")**:
```html
<input type="text" value="Line 1&#10;Line 2" options="MultiLine"
       style="height: 100pt;" />
```
Renders as multi-line text area with text wrapping enabled.

### Data Binding

Inputs support dynamic value binding:

```html
<!-- Model: { name: "John Doe", email: "john@example.com" } -->
<input type="text" value="{{model.name}}" />
<input type="text" value="{{model.email}}" />

<!-- Conditional checkbox -->
<input type="checkbox" value="{{model.isActive ? 'checked' : ''}}" />
```

### Value Handling

The `value` attribute controls what is displayed:
- **Text inputs**: Display the value as text
- **Checkbox**: Non-empty value = checked (shows ✓), empty/null = unchecked
- **Radio**: Non-empty value = selected (shows ⦿), empty/null = unselected
- **Empty value**: Shows placeholder or empty field

### Field Options

The `options` attribute accepts these values (can be combined with bitwise OR):

| Option | Description |
|--------|-------------|
| `None` | Default behavior, no special options |
| `ReadOnly` | Indicates read-only field (visual only in PDF) |
| `Required` | Indicates required field (visual only in PDF) |
| `NoExport` | Field should not be exported (metadata only) |
| `MultiLine` | Enables text wrapping for multi-line input (4096) |
| `Password` | Marks field as password type (8192) |
| `File` | Marks field as file upload type (1048576) |

Example:
```html
<!-- Multi-line text input -->
<input type="text" value="Long text..." options="MultiLine"
       style="height: 80pt;" />
```

### Default Styling

Inputs have these default styles:
- Border: `1pt solid black`
- Padding: `5pt`
- Width: `100%` (full width of container)
- Display: `inline-block`
- Position: `static`
- Text wrapping: Disabled (unless MultiLine option set)
- Overflow: `clip`

### Class Hierarchy

In the Scryber codebase:
- `HTMLInput` extends `FormInputField` extends `Panel`
- Uses `LayoutEngineInput` for custom rendering
- Supports form field metadata and appearance states

---

## Examples

### Basic Text Inputs

```html
<!-- Simple text input -->
<input type="text" value="John Doe" />

<!-- Text input with specific width -->
<input type="text" value="jane@example.com" style="width: 250pt;" />

<!-- Text input with custom styling -->
<input type="text" value="(555) 123-4567"
       style="width: 150pt; padding: 8pt; border: 2pt solid #336699;
              border-radius: 4pt; font-size: 12pt;" />
```

### Text Inputs with Labels

```html
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 100pt; font-weight: bold;">
        Name:
    </label>
    <input type="text" value="John Doe" style="width: 300pt;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 100pt; font-weight: bold;">
        Email:
    </label>
    <input type="text" value="john@example.com" style="width: 300pt;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 100pt; font-weight: bold;">
        Phone:
    </label>
    <input type="text" value="(555) 123-4567" style="width: 300pt;" />
</div>
```

### Styled Form Inputs

```html
<style>
    .form-input {
        width: 100%;
        padding: 8pt;
        border: 1pt solid #cccccc;
        border-radius: 3pt;
        font-size: 11pt;
        background-color: #ffffff;
    }
    .form-input:focus {
        border-color: #336699;
    }
</style>

<input type="text" value="Styled Input" class="form-input" />
<input type="text" value="Another Field" class="form-input"
       style="margin-top: 5pt;" />
```

### Checkboxes

```html
<!-- Checked checkbox -->
<input type="checkbox" value="checked" />
<label style="margin-left: 5pt;">I agree to the terms</label>

<!-- Unchecked checkbox -->
<input type="checkbox" value="" />
<label style="margin-left: 5pt;">Subscribe to newsletter</label>

<!-- Styled checkbox with label -->
<div style="margin: 10pt 0;">
    <input type="checkbox" value="checked"
           style="width: 15pt; height: 15pt; margin-right: 5pt;" />
    <label style="vertical-align: middle;">
        I have read and accept the privacy policy
    </label>
</div>
```

### Radio Buttons

```html
<!-- Radio button group -->
<div style="margin-bottom: 5pt;">
    <input type="radio" value="selected" name="gender"
           style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 5pt; margin-right: 15pt;">Male</label>

    <input type="radio" value="" name="gender"
           style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 5pt; margin-right: 15pt;">Female</label>

    <input type="radio" value="" name="gender"
           style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 5pt;">Other</label>
</div>
```

### Multi-line Text Input (Text Area)

```html
<!-- Multi-line input -->
<input type="text" value="This is a longer text&#10;that spans multiple lines&#10;in the input field."
       options="MultiLine"
       style="width: 100%; height: 100pt; padding: 8pt;
              border: 1pt solid #cccccc;" />

<!-- Text area for comments -->
<label style="font-weight: bold; display: block; margin-bottom: 5pt;">
    Comments:
</label>
<input type="text"
       value="Please provide any additional information here..."
       options="MultiLine"
       style="width: 100%; height: 80pt; padding: 10pt;
              border: 1pt solid #999999; font-size: 10pt;" />
```

### Data-Bound Inputs

```html
<!-- Model: {
    firstName: "John",
    lastName: "Doe",
    email: "john.doe@example.com",
    phone: "(555) 123-4567",
    agreeTerms: true
} -->

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt;">First Name:</label>
    <input type="text" value="{{model.firstName}}" style="width: 200pt;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt;">Last Name:</label>
    <input type="text" value="{{model.lastName}}" style="width: 200pt;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt;">Email:</label>
    <input type="text" value="{{model.email}}" style="width: 300pt;" />
</div>

<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt;">Phone:</label>
    <input type="text" value="{{model.phone}}" style="width: 200pt;" />
</div>

<div style="margin-top: 15pt;">
    <input type="checkbox" value="{{model.agreeTerms ? 'checked' : ''}}" />
    <label style="margin-left: 5pt;">I agree to the terms and conditions</label>
</div>
```

### Complete Registration Form

```html
<style>
    .form-section {
        margin-bottom: 20pt;
        padding: 15pt;
        border: 1pt solid #e0e0e0;
        border-radius: 5pt;
    }
    .form-row {
        margin-bottom: 12pt;
    }
    .form-label {
        display: inline-block;
        width: 150pt;
        font-weight: bold;
        color: #333333;
    }
    .form-input {
        width: 300pt;
        padding: 8pt;
        border: 1pt solid #cccccc;
        border-radius: 3pt;
        font-size: 11pt;
    }
</style>

<div class="form-section">
    <h2 style="margin-top: 0; color: #336699;">Personal Information</h2>

    <div class="form-row">
        <label class="form-label">Full Name:</label>
        <input type="text" value="John Doe" class="form-input" />
    </div>

    <div class="form-row">
        <label class="form-label">Date of Birth:</label>
        <input type="text" value="01/15/1985" class="form-input" style="width: 150pt;" />
    </div>

    <div class="form-row">
        <label class="form-label">Email Address:</label>
        <input type="text" value="john.doe@example.com" class="form-input" />
    </div>

    <div class="form-row">
        <label class="form-label">Phone Number:</label>
        <input type="text" value="(555) 123-4567" class="form-input" style="width: 200pt;" />
    </div>
</div>

<div class="form-section">
    <h2 style="margin-top: 0; color: #336699;">Address</h2>

    <div class="form-row">
        <label class="form-label">Street Address:</label>
        <input type="text" value="123 Main Street" class="form-input" />
    </div>

    <div class="form-row">
        <label class="form-label">City:</label>
        <input type="text" value="Springfield" class="form-input" style="width: 200pt;" />
    </div>

    <div class="form-row">
        <label class="form-label">State:</label>
        <input type="text" value="IL" class="form-input" style="width: 80pt;" />
        <label style="margin-left: 20pt; margin-right: 10pt; font-weight: bold;">ZIP:</label>
        <input type="text" value="62701" class="form-input" style="width: 100pt;" />
    </div>
</div>
```

### Survey Form with Checkboxes

```html
<h2 style="color: #336699;">Customer Satisfaction Survey</h2>

<div style="margin-bottom: 15pt;">
    <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
        Which products have you purchased? (Select all that apply)
    </label>

    <div style="margin-left: 10pt;">
        <div style="margin-bottom: 8pt;">
            <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Product A - Widget</label>
        </div>

        <div style="margin-bottom: 8pt;">
            <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Product B - Gadget</label>
        </div>

        <div style="margin-bottom: 8pt;">
            <input type="checkbox" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Product C - Tool</label>
        </div>

        <div style="margin-bottom: 8pt;">
            <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Product D - Device</label>
        </div>
    </div>
</div>

<div style="margin-bottom: 15pt;">
    <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
        How satisfied are you with our service?
    </label>

    <div style="margin-left: 10pt;">
        <div style="margin-bottom: 8pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Very Satisfied</label>
        </div>

        <div style="margin-bottom: 8pt;">
            <input type="radio" value="selected" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Satisfied</label>
        </div>

        <div style="margin-bottom: 8pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Neutral</label>
        </div>

        <div style="margin-bottom: 8pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Dissatisfied</label>
        </div>

        <div style="margin-bottom: 8pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Very Dissatisfied</label>
        </div>
    </div>
</div>

<div style="margin-bottom: 15pt;">
    <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
        Additional Comments:
    </label>
    <input type="text"
           value="The service was excellent and the staff was very helpful. I would definitely recommend this company to others."
           options="MultiLine"
           style="width: 100%; height: 80pt; padding: 10pt;
                  border: 1pt solid #cccccc; font-size: 10pt;" />
</div>
```

### Invoice Form with Styled Inputs

```html
<style>
    .invoice-header {
        background-color: #336699;
        color: white;
        padding: 15pt;
        margin-bottom: 20pt;
    }
    .invoice-row {
        margin-bottom: 10pt;
    }
    .invoice-label {
        display: inline-block;
        width: 120pt;
        font-weight: bold;
        color: #555555;
    }
    .invoice-input {
        width: 250pt;
        padding: 6pt;
        border: 1pt solid #cccccc;
        background-color: #f9f9f9;
        font-size: 11pt;
    }
</style>

<div class="invoice-header">
    <h1 style="margin: 0; font-size: 20pt;">INVOICE</h1>
</div>

<div style="margin-bottom: 25pt;">
    <h3 style="color: #336699; border-bottom: 2pt solid #336699; padding-bottom: 5pt;">
        Bill To
    </h3>

    <div class="invoice-row">
        <label class="invoice-label">Customer Name:</label>
        <input type="text" value="Acme Corporation" class="invoice-input" />
    </div>

    <div class="invoice-row">
        <label class="invoice-label">Address:</label>
        <input type="text" value="456 Business Ave" class="invoice-input" />
    </div>

    <div class="invoice-row">
        <label class="invoice-label">City, State ZIP:</label>
        <input type="text" value="Chicago, IL 60601" class="invoice-input" />
    </div>

    <div class="invoice-row">
        <label class="invoice-label">Email:</label>
        <input type="text" value="billing@acme.com" class="invoice-input" />
    </div>
</div>

<div style="margin-bottom: 25pt;">
    <h3 style="color: #336699; border-bottom: 2pt solid #336699; padding-bottom: 5pt;">
        Invoice Details
    </h3>

    <div class="invoice-row">
        <label class="invoice-label">Invoice Number:</label>
        <input type="text" value="INV-2024-001" class="invoice-input" style="width: 150pt;" />
    </div>

    <div class="invoice-row">
        <label class="invoice-label">Invoice Date:</label>
        <input type="text" value="January 15, 2024" class="invoice-input" style="width: 150pt;" />
    </div>

    <div class="invoice-row">
        <label class="invoice-label">Due Date:</label>
        <input type="text" value="February 15, 2024" class="invoice-input" style="width: 150pt;" />
    </div>

    <div class="invoice-row">
        <label class="invoice-label">Payment Terms:</label>
        <input type="text" value="Net 30" class="invoice-input" style="width: 150pt;" />
    </div>
</div>
```

### Application Form with Radio Buttons

```html
<h2 style="color: #336699; border-bottom: 2pt solid #336699; padding-bottom: 10pt;">
    Job Application Form
</h2>

<div style="margin-bottom: 15pt;">
    <label style="font-weight: bold;">Full Name:</label>
    <input type="text" value="Jane Smith" style="width: 100%; padding: 8pt;
           border: 1pt solid #cccccc; margin-top: 5pt;" />
</div>

<div style="margin-bottom: 15pt;">
    <label style="font-weight: bold;">Email Address:</label>
    <input type="text" value="jane.smith@email.com" style="width: 100%; padding: 8pt;
           border: 1pt solid #cccccc; margin-top: 5pt;" />
</div>

<div style="margin-bottom: 15pt;">
    <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
        Position Applied For:
    </label>

    <div style="margin-left: 10pt;">
        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Software Engineer</label>
        </div>

        <div style="margin-bottom: 6pt;">
            <input type="radio" value="selected" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Senior Developer</label>
        </div>

        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Team Lead</label>
        </div>

        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Project Manager</label>
        </div>
    </div>
</div>

<div style="margin-bottom: 15pt;">
    <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
        Years of Experience:
    </label>

    <div style="margin-left: 10pt;">
        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">0-2 years</label>
        </div>

        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">3-5 years</label>
        </div>

        <div style="margin-bottom: 6pt;">
            <input type="radio" value="selected" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">6-10 years</label>
        </div>

        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">10+ years</label>
        </div>
    </div>
</div>

<div style="margin-bottom: 15pt;">
    <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
        Cover Letter:
    </label>
    <input type="text"
           value="I am excited to apply for this position. With over 8 years of experience in software development, I have developed strong skills in multiple programming languages and frameworks. I am passionate about creating efficient, scalable solutions and would love the opportunity to contribute to your team."
           options="MultiLine"
           style="width: 100%; height: 100pt; padding: 10pt;
                  border: 1pt solid #cccccc; font-size: 10pt;" />
</div>

<div style="margin-top: 20pt;">
    <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt;">
        I certify that all information provided is accurate and complete.
    </label>
</div>
```

### Horizontal Form Layout

```html
<style>
    .compact-form {
        border: 1pt solid #cccccc;
        padding: 15pt;
        background-color: #f9f9f9;
    }
    .form-field {
        display: inline-block;
        margin-right: 15pt;
        margin-bottom: 10pt;
        vertical-align: top;
    }
    .field-label {
        display: block;
        font-weight: bold;
        font-size: 9pt;
        color: #666666;
        margin-bottom: 4pt;
    }
    .field-input {
        padding: 6pt;
        border: 1pt solid #cccccc;
        font-size: 10pt;
        background-color: white;
    }
</style>

<div class="compact-form">
    <div class="form-field">
        <label class="field-label">First Name</label>
        <input type="text" value="John" class="field-input" style="width: 120pt;" />
    </div>

    <div class="form-field">
        <label class="field-label">Last Name</label>
        <input type="text" value="Doe" class="field-input" style="width: 120pt;" />
    </div>

    <div class="form-field">
        <label class="field-label">Date</label>
        <input type="text" value="01/15/2024" class="field-input" style="width: 100pt;" />
    </div>

    <div style="clear: both;"></div>

    <div class="form-field">
        <label class="field-label">Email</label>
        <input type="text" value="john.doe@example.com" class="field-input" style="width: 260pt;" />
    </div>

    <div class="form-field">
        <label class="field-label">Phone</label>
        <input type="text" value="(555) 123-4567" class="field-input" style="width: 120pt;" />
    </div>
</div>
```

### Two-Column Form Layout

```html
<style>
    .two-column-form {
        width: 100%;
    }
    .form-column {
        display: inline-block;
        width: 48%;
        vertical-align: top;
        margin-right: 2%;
    }
    .form-group {
        margin-bottom: 15pt;
    }
    .group-label {
        display: block;
        font-weight: bold;
        margin-bottom: 5pt;
        color: #333333;
    }
    .group-input {
        width: 100%;
        padding: 8pt;
        border: 1pt solid #cccccc;
        border-radius: 3pt;
    }
</style>

<div class="two-column-form">
    <div class="form-column">
        <div class="form-group">
            <label class="group-label">First Name</label>
            <input type="text" value="John" class="group-input" />
        </div>

        <div class="form-group">
            <label class="group-label">Email</label>
            <input type="text" value="john@example.com" class="group-input" />
        </div>

        <div class="form-group">
            <label class="group-label">City</label>
            <input type="text" value="Chicago" class="group-input" />
        </div>
    </div>

    <div class="form-column">
        <div class="form-group">
            <label class="group-label">Last Name</label>
            <input type="text" value="Doe" class="group-input" />
        </div>

        <div class="form-group">
            <label class="group-label">Phone</label>
            <input type="text" value="(555) 123-4567" class="group-input" />
        </div>

        <div class="form-group">
            <label class="group-label">ZIP Code</label>
            <input type="text" value="60601" class="group-input" />
        </div>
    </div>
</div>
```

### Password and File Input Fields

```html
<div style="margin-bottom: 15pt;">
    <label style="font-weight: bold; display: block; margin-bottom: 5pt;">
        Password:
    </label>
    <input type="password" value="secretpass123" options="Password"
           style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;
                  font-family: monospace;" />
    <p style="margin: 5pt 0 0 0; font-size: 9pt; color: #666666;">
        Note: Password displayed as entered (PDFs are static documents)
    </p>
</div>

<div style="margin-bottom: 15pt;">
    <label style="font-weight: bold; display: block; margin-bottom: 5pt;">
        Upload Document:
    </label>
    <input type="file" value="resume.pdf" options="File"
           style="width: 300pt; padding: 8pt; border: 1pt solid #cccccc;
                  background-color: #f5f5f5;" />
</div>
```

### Dynamic Form with Data Binding

```html
<!-- Model: {
    user: {
        firstName: "Alice",
        lastName: "Johnson",
        email: "alice@example.com",
        preferences: {
            newsletter: true,
            notifications: true,
            marketing: false
        }
    }
} -->

<h2 style="color: #336699;">User Preferences</h2>

<div style="margin-bottom: 20pt; padding: 15pt; border: 1pt solid #e0e0e0;">
    <h3 style="margin-top: 0;">Personal Information</h3>

    <div style="margin-bottom: 10pt;">
        <label style="display: inline-block; width: 130pt; font-weight: bold;">
            First Name:
        </label>
        <input type="text" value="{{model.user.firstName}}"
               style="width: 200pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 10pt;">
        <label style="display: inline-block; width: 130pt; font-weight: bold;">
            Last Name:
        </label>
        <input type="text" value="{{model.user.lastName}}"
               style="width: 200pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 10pt;">
        <label style="display: inline-block; width: 130pt; font-weight: bold;">
            Email:
        </label>
        <input type="text" value="{{model.user.email}}"
               style="width: 300pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>
</div>

<div style="padding: 15pt; border: 1pt solid #e0e0e0;">
    <h3 style="margin-top: 0;">Communication Preferences</h3>

    <div style="margin-bottom: 10pt;">
        <input type="checkbox"
               value="{{model.user.preferences.newsletter ? 'checked' : ''}}"
               style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Send me newsletter updates</label>
    </div>

    <div style="margin-bottom: 10pt;">
        <input type="checkbox"
               value="{{model.user.preferences.notifications ? 'checked' : ''}}"
               style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Enable push notifications</label>
    </div>

    <div style="margin-bottom: 10pt;">
        <input type="checkbox"
               value="{{model.user.preferences.marketing ? 'checked' : ''}}"
               style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Receive marketing emails</label>
    </div>
</div>
```

### Form Template with Repeating Fields

```html
<!-- Model: {
    contacts: [
        {name: "John Doe", email: "john@example.com", phone: "(555) 111-1111"},
        {name: "Jane Smith", email: "jane@example.com", phone: "(555) 222-2222"}
    ]
} -->

<h2 style="color: #336699;">Contact List</h2>

<template data-bind="{{model.contacts}}">
    <div style="margin-bottom: 15pt; padding: 12pt;
                border: 1pt solid #e0e0e0; background-color: #f9f9f9;">
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
            <input type="text" value="{{.email}}"
                   style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>

        <div style="margin-bottom: 8pt;">
            <label style="display: inline-block; width: 100pt; font-weight: bold;">
                Phone:
            </label>
            <input type="text" value="{{.phone}}"
                   style="width: 200pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>
    </div>
</template>
```

### Professional Medical Form

```html
<style>
    .medical-form {
        font-size: 10pt;
    }
    .section-header {
        background-color: #2c5f8d;
        color: white;
        padding: 10pt;
        margin: 15pt 0 10pt 0;
        font-weight: bold;
    }
    .form-field-row {
        margin-bottom: 10pt;
    }
    .field-label-wide {
        display: inline-block;
        width: 180pt;
        font-weight: bold;
    }
    .medical-input {
        padding: 6pt;
        border: 1pt solid #999999;
        background-color: white;
    }
</style>

<div class="medical-form">
    <h1 style="text-align: center; color: #2c5f8d;">PATIENT INTAKE FORM</h1>

    <div class="section-header">Patient Information</div>

    <div class="form-field-row">
        <label class="field-label-wide">Patient Name:</label>
        <input type="text" value="Robert Johnson" class="medical-input" style="width: 280pt;" />
    </div>

    <div class="form-field-row">
        <label class="field-label-wide">Date of Birth:</label>
        <input type="text" value="03/22/1975" class="medical-input" style="width: 120pt;" />
        <label style="margin-left: 20pt; margin-right: 10pt; font-weight: bold;">Age:</label>
        <input type="text" value="48" class="medical-input" style="width: 60pt;" />
    </div>

    <div class="form-field-row">
        <label class="field-label-wide">Emergency Contact:</label>
        <input type="text" value="Mary Johnson" class="medical-input" style="width: 200pt;" />
        <label style="margin-left: 10pt; margin-right: 5pt; font-weight: bold;">Phone:</label>
        <input type="text" value="(555) 999-8888" class="medical-input" style="width: 120pt;" />
    </div>

    <div class="section-header">Medical History</div>

    <div style="margin-bottom: 10pt;">
        <label style="font-weight: bold; display: block; margin-bottom: 6pt;">
            Do you have any of the following conditions? (Check all that apply)
        </label>

        <div style="margin-left: 15pt;">
            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 12pt; height: 12pt;" />
                <label style="margin-left: 6pt;">Diabetes</label>
            </div>
            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 12pt; height: 12pt;" />
                <label style="margin-left: 6pt;">High Blood Pressure</label>
            </div>

            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 12pt; height: 12pt;" />
                <label style="margin-left: 6pt;">Heart Disease</label>
            </div>
            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 12pt; height: 12pt;" />
                <label style="margin-left: 6pt;">Asthma</label>
            </div>

            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 12pt; height: 12pt;" />
                <label style="margin-left: 6pt;">Allergies</label>
            </div>
            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 12pt; height: 12pt;" />
                <label style="margin-left: 6pt;">None</label>
            </div>
        </div>
    </div>

    <div style="margin-bottom: 10pt;">
        <label style="font-weight: bold; display: block; margin-bottom: 5pt;">
            Current Medications:
        </label>
        <input type="text"
               value="Lisinopril 10mg daily&#10;Aspirin 81mg daily"
               options="MultiLine"
               style="width: 100%; height: 60pt; padding: 8pt;
                      border: 1pt solid #999999; font-size: 10pt;" />
    </div>

    <div style="margin-bottom: 10pt;">
        <label style="font-weight: bold; display: block; margin-bottom: 5pt;">
            Reason for Visit:
        </label>
        <input type="text"
               value="Annual physical examination and blood pressure follow-up"
               options="MultiLine"
               style="width: 100%; height: 60pt; padding: 8pt;
                      border: 1pt solid #999999; font-size: 10pt;" />
    </div>

    <div style="margin-top: 20pt; padding-top: 15pt; border-top: 1pt solid #cccccc;">
        <input type="checkbox" value="checked" style="width: 12pt; height: 12pt;" />
        <label style="margin-left: 8pt; font-weight: bold;">
            I certify that the above information is accurate to the best of my knowledge.
        </label>
    </div>
</div>
```

---

## See Also

- [label](/reference/htmltags/label.html) - Label element for form fields
- [fieldset](/reference/htmltags/fieldset.html) - Fieldset and legend for grouping form elements
- [form](/reference/htmltags/form.html) - Form container element
- [div](/reference/htmltags/div.html) - Generic block container
- [span](/reference/htmltags/span.html) - Inline container for styling
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
