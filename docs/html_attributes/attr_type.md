---
layout: default
title: type
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @type : The Input Type Attribute

The `type` attribute specifies the type of input field to render in PDF documents. It controls both the visual appearance and the semantic meaning of form fields. Since Scryber PDFs are static documents, input types render as styled visual representations rather than interactive controls.

## Usage

The `type` attribute determines how input fields are displayed:
- Controls the visual rendering style (text box, checkbox, radio button, etc.)
- Defines the semantic meaning of the field
- Affects default styling and behavior
- Supports various input types for different data types
- Renders as static visual representation in PDF output
- Can be combined with CSS styling for custom appearances

```html
<!-- Text input (default) -->
<input type="text" value="John Doe" />

<!-- Checkbox input -->
<input type="checkbox" value="checked" />

<!-- Radio button input -->
<input type="radio" value="selected" />

<!-- Button input -->
<input type="button" value="Click Me" />
```

---

## Supported Elements

The `type` attribute is supported by the following elements:

| Element | Description |
|---------|-------------|
| `<input>` | Form input field element |

---

## Supported Type Values

### Text-Based Input Types

| Type | Description | Visual Representation |
|------|-------------|----------------------|
| `text` | Single-line text input (default) | Text displayed in bordered box |
| `password` | Password field | Text displayed (non-masked in PDF) |
| `email` | Email address field | Text displayed in bordered box |
| `url` | URL field | Text displayed in bordered box |
| `tel` | Telephone number field | Text displayed in bordered box |
| `search` | Search field | Text displayed in bordered box |

**Note**: Email, URL, tel, and search types render identically to text type in PDFs. The type attribute provides semantic meaning.

### Selection Input Types

| Type | Description | Visual Representation |
|------|-------------|----------------------|
| `checkbox` | Checkbox input | Checked: shows checkmark (✓), Unchecked: empty box (☐) |
| `radio` | Radio button input | Selected: filled circle (⦿), Unselected: empty circle (○) |

### Button Input Types

| Type | Description | Visual Representation |
|------|-------------|----------------------|
| `button` | Generic button | Text displayed as button |
| `submit` | Form submit button | Text displayed as button |
| `reset` | Form reset button | Text displayed as button |

**Note**: Button types are non-interactive in PDFs but can be styled to appear as buttons.

### File Input Types

| Type | Description | Visual Representation |
|------|-------------|----------------------|
| `file` | File upload field | File path or filename displayed |

### Number and Date Input Types

| Type | Description | Visual Representation |
|------|-------------|----------------------|
| `number` | Numeric input | Number displayed in bordered box |
| `range` | Range slider | Value displayed as text |
| `date` | Date picker | Date displayed as formatted text |
| `time` | Time picker | Time displayed as formatted text |
| `datetime-local` | Date and time picker | DateTime displayed as formatted text |
| `month` | Month picker | Month displayed as formatted text |
| `week` | Week picker | Week displayed as formatted text |

**Note**: Date and number types render as text in PDFs with formatting applied to the value.

---

## Binding Values

The `type` attribute can be set statically or dynamically using data binding:

### Static Type Declaration

```html
<input type="text" value="Static text input" />
<input type="checkbox" value="checked" />
<input type="radio" value="selected" />
```

### Dynamic Type with Data Binding

```html
<!-- Model: { inputType: "text", fieldValue: "Dynamic value" } -->
<input type="{{model.inputType}}" value="{{model.fieldValue}}" />
```

### Conditional Type Selection

```html
<!-- Model: { isCheckbox: true } -->
<input type="{{model.isCheckbox ? 'checkbox' : 'text'}}"
       value="{{model.value}}" />
```

### Value Binding for Checkboxes and Radio Buttons

For checkbox and radio button types, the `value` attribute controls the checked/selected state:

```html
<!-- Checkbox: non-empty value = checked -->
<input type="checkbox" value="checked" />
<input type="checkbox" value="" />

<!-- Radio: non-empty value = selected -->
<input type="radio" value="selected" name="option" />
<input type="radio" value="" name="option" />

<!-- Data-bound checkbox state -->
<!-- Model: { isAccepted: true } -->
<input type="checkbox" value="{{model.isAccepted ? 'checked' : ''}}" />
```

---

## Notes

### Static PDF Rendering

All input types in Scryber PDFs are **static visual representations**:
- Fields display current values but are not editable
- Checkboxes show checked/unchecked state visually
- Radio buttons show selected/unselected state visually
- Perfect for completed forms, invoices, and reports
- Users cannot interact with fields in PDF viewers

### Default Type Behavior

When no `type` attribute is specified, the default is `text`:

```html
<!-- These are equivalent -->
<input value="Default text" />
<input type="text" value="Default text" />
```

### Type-Specific Rendering

**Text Type (type="text")**:
- Renders in bordered box with padding
- Single line of text
- Default width: 100% of container
- Text can be styled with CSS

**Checkbox Type (type="checkbox")**:
- Renders as checkbox symbol
- Checked state: ✓ or ☑
- Unchecked state: ☐
- Size can be controlled with width/height
- Non-empty value = checked

**Radio Type (type="radio")**:
- Renders as radio button symbol
- Selected state: ⦿ (filled circle)
- Unselected state: ○ (empty circle)
- Size can be controlled with width/height
- Non-empty value = selected

**Button Types**:
- Render as text in button-styled box
- Can be styled with borders, padding, background colors
- Value attribute contains button text

**Password Type (type="password")**:
- Renders as regular text in PDF (not masked)
- Use with caution for sensitive data
- Consider masking value before rendering

**File Type (type="file")**:
- Displays filename or path
- Shows static reference to file
- Not interactive in PDF

### Multi-line Text Input

For multi-line text areas, use the `options="MultiLine"` attribute with type="text":

```html
<input type="text"
       value="Line 1&#10;Line 2&#10;Line 3"
       options="MultiLine"
       style="height: 80pt;" />
```

### Type and Styling

Different types can be styled using CSS:

```html
<style>
    input[type="text"] {
        border: 1pt solid #cccccc;
        padding: 8pt;
    }
    input[type="checkbox"] {
        width: 15pt;
        height: 15pt;
    }
    input[type="button"] {
        background-color: #336699;
        color: white;
        padding: 10pt 20pt;
        border-radius: 5pt;
    }
</style>
```

### Browser Compatibility

Scryber processes these input types during PDF generation. The rendered PDF displays static visual representations regardless of which PDF viewer is used.

---

## Examples

### Basic Text Input Types

```html
<!-- Standard text input -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Name:
    </label>
    <input type="text" value="John Doe" style="width: 250pt; padding: 6pt;
           border: 1pt solid #cccccc;" />
</div>

<!-- Password input (shows as text in PDF) -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Password:
    </label>
    <input type="password" value="********" style="width: 250pt; padding: 6pt;
           border: 1pt solid #cccccc; font-family: monospace;" />
</div>

<!-- Email input -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Email:
    </label>
    <input type="email" value="john.doe@example.com" style="width: 300pt;
           padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<!-- Phone input -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Phone:
    </label>
    <input type="tel" value="(555) 123-4567" style="width: 200pt; padding: 6pt;
           border: 1pt solid #cccccc;" />
</div>
```

### Checkbox Inputs

```html
<!-- Single checkbox (checked) -->
<div style="margin-bottom: 8pt;">
    <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt;">I agree to the terms and conditions</label>
</div>

<!-- Unchecked checkbox -->
<div style="margin-bottom: 8pt;">
    <input type="checkbox" value="" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt;">Subscribe to newsletter</label>
</div>

<!-- Styled checkbox -->
<div style="margin-bottom: 8pt;">
    <input type="checkbox" value="checked"
           style="width: 18pt; height: 18pt; border: 2pt solid #336699;" />
    <label style="margin-left: 8pt; font-weight: bold; color: #336699;">
        Premium member
    </label>
</div>

<!-- Multiple checkboxes -->
<div>
    <label style="display: block; font-weight: bold; margin-bottom: 8pt;">
        Select your interests:
    </label>
    <div style="margin-left: 15pt;">
        <div style="margin-bottom: 6pt;">
            <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Technology</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Sports</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="checkbox" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Music</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Travel</label>
        </div>
    </div>
</div>
```

### Radio Button Inputs

```html
<!-- Radio button group -->
<div style="margin-bottom: 15pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 8pt;">
        Select your preferred contact method:
    </label>
    <div style="margin-left: 15pt;">
        <div style="margin-bottom: 6pt;">
            <input type="radio" value="selected" name="contact"
                   style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Email</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" name="contact"
                   style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Phone</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="radio" value="" name="contact"
                   style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Mail</label>
        </div>
    </div>
</div>

<!-- Styled radio buttons -->
<div style="padding: 15pt; border: 1pt solid #e0e0e0; background-color: #f9f9f9;">
    <label style="font-weight: bold; display: block; margin-bottom: 10pt;">
        Choose your plan:
    </label>
    <div style="margin-bottom: 8pt;">
        <input type="radio" value="" name="plan" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Basic - $9.99/month</label>
    </div>
    <div style="margin-bottom: 8pt;">
        <input type="radio" value="selected" name="plan"
               style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt; font-weight: bold; color: #336699;">
            Pro - $19.99/month (Selected)
        </label>
    </div>
    <div style="margin-bottom: 8pt;">
        <input type="radio" value="" name="plan" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Enterprise - $49.99/month</label>
    </div>
</div>
```

### Button Input Types

```html
<!-- Standard button -->
<input type="button" value="Click Here"
       style="padding: 10pt 20pt; border: 1pt solid #336699;
              background-color: #336699; color: white; font-weight: bold;
              border-radius: 5pt;" />

<!-- Submit button -->
<input type="submit" value="Submit Form"
       style="padding: 12pt 24pt; border: none;
              background-color: #28a745; color: white; font-weight: bold;
              border-radius: 3pt; margin-right: 10pt;" />

<!-- Reset button -->
<input type="reset" value="Reset"
       style="padding: 12pt 24pt; border: 1pt solid #cccccc;
              background-color: #f5f5f5; color: #333333; font-weight: bold;
              border-radius: 3pt;" />

<!-- Styled button group -->
<div style="text-align: center; padding: 20pt;">
    <input type="button" value="Save"
           style="padding: 10pt 25pt; border: none;
                  background-color: #336699; color: white; font-size: 11pt;
                  border-radius: 4pt; margin-right: 10pt;" />
    <input type="button" value="Cancel"
           style="padding: 10pt 25pt; border: 1pt solid #999999;
                  background-color: white; color: #333333; font-size: 11pt;
                  border-radius: 4pt; margin-right: 10pt;" />
    <input type="button" value="Delete"
           style="padding: 10pt 25pt; border: none;
                  background-color: #dc3545; color: white; font-size: 11pt;
                  border-radius: 4pt;" />
</div>
```

### Number and Date Input Types

```html
<!-- Number input -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Quantity:
    </label>
    <input type="number" value="5" style="width: 100pt; padding: 6pt;
           border: 1pt solid #cccccc; text-align: right;" />
</div>

<!-- Date input -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Birth Date:
    </label>
    <input type="date" value="1985-03-15" style="width: 150pt; padding: 6pt;
           border: 1pt solid #cccccc;" />
</div>

<!-- Time input -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Appointment:
    </label>
    <input type="time" value="14:30" style="width: 120pt; padding: 6pt;
           border: 1pt solid #cccccc;" />
</div>

<!-- DateTime input -->
<div style="margin-bottom: 10pt;">
    <label style="display: inline-block; width: 120pt; font-weight: bold;">
        Event Time:
    </label>
    <input type="datetime-local" value="2024-01-15T14:30"
           style="width: 200pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>
```

### File Input Type

```html
<!-- File input -->
<div style="margin-bottom: 10pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Upload Document:
    </label>
    <input type="file" value="resume.pdf"
           style="width: 350pt; padding: 8pt; border: 1pt solid #cccccc;
                  background-color: #f5f5f5;" />
</div>

<!-- Multiple file references -->
<div style="padding: 15pt; border: 1pt solid #e0e0e0;">
    <h3 style="margin-top: 0;">Attached Files</h3>
    <div style="margin-bottom: 8pt;">
        <label style="font-weight: bold;">Resume:</label>
        <input type="file" value="john_doe_resume.pdf"
               style="width: 300pt; padding: 6pt; margin-left: 10pt;
                      border: 1pt solid #cccccc; background-color: #fafafa;" />
    </div>
    <div style="margin-bottom: 8pt;">
        <label style="font-weight: bold;">Cover Letter:</label>
        <input type="file" value="cover_letter.docx"
               style="width: 300pt; padding: 6pt; margin-left: 10pt;
                      border: 1pt solid #cccccc; background-color: #fafafa;" />
    </div>
</div>
```

### Data-Bound Input Types

```html
<!-- Model: {
    fields: [
        {type: "text", label: "Name", value: "John Doe"},
        {type: "email", label: "Email", value: "john@example.com"},
        {type: "tel", label: "Phone", value: "(555) 123-4567"}
    ]
} -->

<template data-bind="{{model.fields}}">
    <div style="margin-bottom: 12pt;">
        <label style="display: inline-block; width: 120pt; font-weight: bold;">
            {{.label}}:
        </label>
        <input type="{{.type}}" value="{{.value}}"
               style="width: 300pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>
</template>
```

### Survey Form with Mixed Input Types

```html
<style>
    .survey-form {
        padding: 20pt;
        border: 2pt solid #336699;
    }
    .question-section {
        margin-bottom: 20pt;
        padding-bottom: 15pt;
        border-bottom: 1pt solid #e0e0e0;
    }
    .question-label {
        font-weight: bold;
        display: block;
        margin-bottom: 10pt;
        color: #333333;
    }
</style>

<div class="survey-form">
    <h2 style="color: #336699; margin-top: 0;">Customer Satisfaction Survey</h2>

    <!-- Text input -->
    <div class="question-section">
        <label class="question-label">1. Full Name:</label>
        <input type="text" value="Alice Johnson"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <!-- Email input -->
    <div class="question-section">
        <label class="question-label">2. Email Address:</label>
        <input type="email" value="alice.johnson@email.com"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <!-- Radio buttons -->
    <div class="question-section">
        <label class="question-label">3. How satisfied are you with our service?</label>
        <div style="margin-left: 15pt;">
            <div style="margin-bottom: 6pt;">
                <input type="radio" value="" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Very Satisfied</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="radio" value="selected" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Satisfied</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="radio" value="" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Neutral</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="radio" value="" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Dissatisfied</label>
            </div>
        </div>
    </div>

    <!-- Checkboxes -->
    <div class="question-section">
        <label class="question-label">4. Which features do you use? (Select all that apply)</label>
        <div style="margin-left: 15pt;">
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Online Dashboard</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Mobile App</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">API Integration</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Reporting Tools</label>
            </div>
        </div>
    </div>

    <!-- Number input -->
    <div class="question-section">
        <label class="question-label">5. How many years have you been a customer?</label>
        <input type="number" value="3"
               style="width: 100pt; padding: 8pt; border: 1pt solid #cccccc;
                      text-align: center; font-size: 14pt;" />
    </div>

    <!-- Date input -->
    <div class="question-section">
        <label class="question-label">6. Date of last purchase:</label>
        <input type="date" value="2024-01-10"
               style="width: 180pt; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <!-- Text area (multi-line) -->
    <div class="question-section" style="border-bottom: none;">
        <label class="question-label">7. Additional comments or suggestions:</label>
        <input type="text"
               value="The service has been excellent. I particularly appreciate the responsive customer support team and the intuitive user interface."
               options="MultiLine"
               style="width: 100%; height: 80pt; padding: 10pt;
                      border: 1pt solid #cccccc; font-size: 10pt;" />
    </div>

    <!-- Submit button -->
    <div style="text-align: center; margin-top: 20pt;">
        <input type="submit" value="Submit Survey"
               style="padding: 12pt 40pt; border: none;
                      background-color: #336699; color: white; font-weight: bold;
                      font-size: 12pt; border-radius: 5pt;" />
    </div>
</div>
```

### Registration Form with All Input Types

```html
<h2 style="color: #336699; border-bottom: 3pt solid #336699; padding-bottom: 10pt;">
    User Registration Form
</h2>

<!-- Personal Information -->
<fieldset style="border: 2pt solid #e0e0e0; padding: 15pt; margin-bottom: 20pt;">
    <legend style="font-weight: bold; font-size: 12pt; color: #336699; padding: 0 10pt;">
        Personal Information
    </legend>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Full Name:
        </label>
        <input type="text" value="Jennifer Martinez"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Email Address:
        </label>
        <input type="email" value="jennifer.martinez@email.com"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Phone Number:
        </label>
        <input type="tel" value="(555) 987-6543"
               style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Date of Birth:
        </label>
        <input type="date" value="1990-07-22"
               style="width: 180pt; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Website (Optional):
        </label>
        <input type="url" value="https://www.example.com"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>
</fieldset>

<!-- Account Settings -->
<fieldset style="border: 2pt solid #e0e0e0; padding: 15pt; margin-bottom: 20pt;">
    <legend style="font-weight: bold; font-size: 12pt; color: #336699; padding: 0 10pt;">
        Account Settings
    </legend>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Username:
        </label>
        <input type="text" value="jmartinez2024"
               style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;
                      font-family: monospace;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Password:
        </label>
        <input type="password" value="••••••••••"
               style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;
                      font-family: monospace;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 8pt;">
            Account Type:
        </label>
        <div style="margin-left: 15pt;">
            <div style="margin-bottom: 6pt;">
                <input type="radio" value="" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Personal</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="radio" value="selected" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Business</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="radio" value="" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Enterprise</label>
            </div>
        </div>
    </div>
</fieldset>

<!-- Preferences -->
<fieldset style="border: 2pt solid #e0e0e0; padding: 15pt; margin-bottom: 20pt;">
    <legend style="font-weight: bold; font-size: 12pt; color: #336699; padding: 0 10pt;">
        Preferences
    </legend>

    <div style="margin-bottom: 15pt;">
        <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
            Communication Preferences:
        </label>
        <div style="margin-left: 15pt;">
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Email newsletters</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Product updates</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Marketing offers</label>
            </div>
        </div>
    </div>

    <div style="margin-bottom: 0;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            About Me (Optional):
        </label>
        <input type="text"
               value="I'm a business consultant interested in productivity tools and project management solutions."
               options="MultiLine"
               style="width: 100%; height: 70pt; padding: 10pt;
                      border: 1pt solid #cccccc; font-size: 10pt;" />
    </div>
</fieldset>

<!-- Agreement -->
<div style="margin-bottom: 20pt;">
    <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt; font-weight: bold;">
        I agree to the Terms of Service and Privacy Policy
    </label>
</div>

<!-- Submit button -->
<div style="text-align: center;">
    <input type="submit" value="Create Account"
           style="padding: 14pt 50pt; border: none;
                  background-color: #28a745; color: white; font-weight: bold;
                  font-size: 13pt; border-radius: 5pt;" />
</div>
```

### Conditional Type Rendering with Data Binding

```html
<!-- Model: {
    showPassword: false,
    useCheckbox: true,
    credentials: {
        username: "admin",
        password: "secret123"
    },
    accepted: true
} -->

<div style="margin-bottom: 12pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Username:
    </label>
    <input type="text" value="{{model.credentials.username}}"
           style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 12pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
        Password:
    </label>
    <input type="{{model.showPassword ? 'text' : 'password'}}"
           value="{{model.credentials.password}}"
           style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;
                  font-family: monospace;" />
</div>

<div style="margin-top: 15pt;">
    <input type="{{model.useCheckbox ? 'checkbox' : 'radio'}}"
           value="{{model.accepted ? 'checked' : ''}}"
           style="width: 15pt; height: 15pt;" />
    <label style="margin-left: 8pt;">
        I agree to the terms
    </label>
</div>
```

---

## See Also

- [input](/reference/htmltags/input.html) - Input field element
- [value](/reference/htmlattributes/value.html) - Value attribute for form fields
- [name](/reference/htmlattributes/name.html) - Name attribute for form fields
- [label](/reference/htmltags/label.html) - Label element for form fields
- [fieldset](/reference/htmltags/fieldset.html) - Fieldset and legend for grouping
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
