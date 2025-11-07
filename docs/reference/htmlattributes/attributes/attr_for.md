---
layout: default
title: for
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @for : The Label Association Attribute

The `for` attribute creates a semantic association between a `<label>` element and a form input field. It references the `id` of the associated input, establishing a programmatic connection between the label and the field. While PDFs are static documents, the `for` attribute improves code organization, readability, and documents field relationships.

## Usage

The `for` attribute connects labels to form fields:
- References the `id` attribute of the associated input element
- Creates semantic relationship between label and field
- Improves code readability and organization
- Documents which label describes which field
- Useful for maintainability and future enhancements
- Works with all input types
- In static PDFs, primarily serves organizational purposes

```html
<!-- Basic label-input association -->
<label for="email-field">Email Address:</label>
<input type="text" id="email-field" name="email" value="john@example.com" />

<!-- Checkbox with label -->
<input type="checkbox" id="agree-terms" value="checked" />
<label for="agree-terms">I agree to the terms</label>

<!-- Radio button with label -->
<input type="radio" id="payment-card" name="payment" value="selected" />
<label for="payment-card">Credit Card</label>
```

---

## Supported Elements

The `for` attribute is supported by the following element:

| Element | Description |
|---------|-------------|
| `<label>` | Label element for form fields |

The `for` attribute references the `id` of:

| Element | Description |
|---------|-------------|
| `<input>` | Form input field (all types) |
| `<select>` | Dropdown selection (if supported) |
| `<textarea>` | Multi-line text area (if supported) |

---

## For Attribute Behavior

### Basic Association

The `for` attribute must match the `id` of the target input:

```html
<!-- Label with for="username" references input with id="username" -->
<label for="username">Username:</label>
<input type="text" id="username" name="username" value="jdoe" />
```

**Important**: The `for` value must exactly match the input's `id` value (case-sensitive).

### Multiple Labels for Same Field

Multiple labels can reference the same field:

```html
<!-- Main label -->
<label for="email-input" style="font-weight: bold;">Email:</label>
<input type="text" id="email-input" name="email" value="john@example.com" />

<!-- Additional descriptive label -->
<label for="email-input" style="font-size: 9pt; color: #666666;">
    (We'll never share your email)
</label>
```

**Note**: While multiple labels can reference one field, each field should have one primary label.

### Without For Attribute

Labels can also wrap inputs without using `for`:

```html
<!-- Label wrapping input (no for attribute needed) -->
<label>
    <input type="checkbox" value="checked" />
    <span style="margin-left: 5pt;">Subscribe to newsletter</span>
</label>

<!-- Equivalent with for attribute -->
<input type="checkbox" id="newsletter-cb" value="checked" />
<label for="newsletter-cb">Subscribe to newsletter</label>
```

**Best Practice**: Use `for` attribute when label and input are separate elements.

---

## Binding Values

The `for` attribute can be set statically or dynamically:

### Static For Value

```html
<label for="customer-name">Name:</label>
<input type="text" id="customer-name" name="name" value="John Doe" />
```

### Dynamic For with Data Binding

```html
<!-- Model: { fieldId: "email-field" } -->
<label for="{{model.fieldId}}">Email:</label>
<input type="text" id="{{model.fieldId}}" name="email" value="{{model.email}}" />
```

### Conditional For Value

```html
<!-- Model: { isPrimary: true } -->
<label for="{{model.isPrimary ? 'primary-email' : 'secondary-email'}}">
    Email:
</label>
<input type="text"
       id="{{model.isPrimary ? 'primary-email' : 'secondary-email'}}"
       name="email"
       value="{{model.email}}" />
```

### For in Repeating Templates

```html
<!-- Model: { fields: [{id: "field1", label: "Name", value: "John"}, ...] } -->
<template data-bind="{{model.fields}}">
    <div style="margin-bottom: 10pt;">
        <label for="{{.id}}">{{.label}}:</label>
        <input type="text" id="{{.id}}" name="{{.id}}" value="{{.value}}" />
    </div>
</template>
```

---

## Notes

### Static PDF Behavior

In Scryber PDFs, the `for` attribute is primarily semantic:
- Does **not** create interactive behavior (like clicking label to focus field)
- PDFs are static documents without interactive form controls
- Label and input are rendered separately in their positions
- The association is preserved in code but not in user interaction

### Benefits in Static PDFs

Despite being non-interactive, the `for` attribute provides value:

**Code Organization**:
```html
<!-- Clear relationship between label and field -->
<label for="customer-email">Customer Email:</label>
<input type="text" id="customer-email" name="email" value="john@example.com" />
```

**Documentation**:
- Makes code more readable and maintainable
- Clearly identifies which label belongs to which field
- Helps developers understand form structure

**Future Compatibility**:
- If interactive PDF forms are added in future
- Prepared for potential enhancements
- Follows HTML best practices

### ID and For Matching

The `for` value must exactly match the input's `id`:

```html
<!-- Correct: matching values -->
<label for="email-field">Email:</label>
<input type="text" id="email-field" name="email" />

<!-- Incorrect: mismatched values -->
<label for="email-field">Email:</label>
<input type="text" id="emailField" name="email" />

<!-- Incorrect: case mismatch -->
<label for="email-field">Email:</label>
<input type="text" id="Email-Field" name="email" />
```

### ID Uniqueness

Each `id` must be unique within the document:

```html
<!-- Correct: unique IDs -->
<label for="first-name">First:</label>
<input type="text" id="first-name" />

<label for="last-name">Last:</label>
<input type="text" id="last-name" />

<!-- Incorrect: duplicate IDs -->
<label for="name">First:</label>
<input type="text" id="name" />

<label for="name">Last:</label>
<input type="text" id="name" /> <!-- ERROR: duplicate ID -->
```

### For with Radio Buttons

Each radio button in a group can have its own label with `for`:

```html
<label style="font-weight: bold; display: block;">Payment Method:</label>

<input type="radio" id="payment-cc" name="payment" value="selected" />
<label for="payment-cc">Credit Card</label>

<input type="radio" id="payment-pp" name="payment" value="" />
<label for="payment-pp">PayPal</label>

<input type="radio" id="payment-bt" name="payment" value="" />
<label for="payment-bt">Bank Transfer</label>
```

**Note**: The `name` groups the radio buttons, while `id` and `for` associate individual labels.

### For with Checkboxes

Each checkbox typically has its own label:

```html
<input type="checkbox" id="email-notify" value="checked" />
<label for="email-notify">Email notifications</label>

<input type="checkbox" id="sms-notify" value="checked" />
<label for="sms-notify">SMS notifications</label>

<input type="checkbox" id="push-notify" value="" />
<label for="push-notify">Push notifications</label>
```

### Wrapping vs. For Attribute

Two approaches to associate labels with inputs:

**Approach 1: Using `for` attribute** (separate elements):
```html
<label for="field-1">Name:</label>
<input type="text" id="field-1" name="name" />
```

**Approach 2: Wrapping** (no `for` needed):
```html
<label>
    Name:
    <input type="text" name="name" />
</label>
```

**Best Practice**: Use `for` attribute when label and input need to be positioned separately.

### Label Positioning

Labels can be positioned before, after, or around inputs:

```html
<!-- Label before input (most common) -->
<label for="name-field">Name:</label>
<input type="text" id="name-field" name="name" />

<!-- Label after input (common for checkboxes) -->
<input type="checkbox" id="agree-cb" value="checked" />
<label for="agree-cb">I agree</label>

<!-- Label wrapping input -->
<label for="email-field">
    Email:
    <input type="text" id="email-field" name="email" />
</label>
```

---

## Examples

### Basic Label-Input Associations

```html
<!-- Text input with label -->
<div style="margin-bottom: 10pt;">
    <label for="full-name" style="display: inline-block; width: 120pt; font-weight: bold;">
        Full Name:
    </label>
    <input type="text" id="full-name" name="fullName" value="John Doe"
           style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<!-- Email input with label -->
<div style="margin-bottom: 10pt;">
    <label for="user-email" style="display: inline-block; width: 120pt; font-weight: bold;">
        Email:
    </label>
    <input type="email" id="user-email" name="email" value="john@example.com"
           style="width: 300pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<!-- Phone input with label -->
<div style="margin-bottom: 10pt;">
    <label for="user-phone" style="display: inline-block; width: 120pt; font-weight: bold;">
        Phone:
    </label>
    <input type="tel" id="user-phone" name="phone" value="(555) 123-4567"
           style="width: 200pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>

<!-- Date input with label -->
<div style="margin-bottom: 10pt;">
    <label for="birth-date" style="display: inline-block; width: 120pt; font-weight: bold;">
        Birth Date:
    </label>
    <input type="date" id="birth-date" name="birthDate" value="1985-03-15"
           style="width: 180pt; padding: 6pt; border: 1pt solid #cccccc;" />
</div>
```

### Vertical Form Layout with For Attribute

```html
<style>
    .form-field {
        margin-bottom: 15pt;
    }
    .field-label {
        display: block;
        font-weight: bold;
        margin-bottom: 5pt;
        color: #333333;
    }
    .field-input {
        width: 100%;
        padding: 8pt;
        border: 1pt solid #cccccc;
        border-radius: 3pt;
    }
</style>

<div class="form-field">
    <label for="customer-name" class="field-label">Customer Name:</label>
    <input type="text" id="customer-name" name="customerName"
           value="Alice Johnson" class="field-input" />
</div>

<div class="form-field">
    <label for="company-name" class="field-label">Company Name:</label>
    <input type="text" id="company-name" name="companyName"
           value="Tech Solutions Inc." class="field-input" />
</div>

<div class="form-field">
    <label for="email-address" class="field-label">Email Address:</label>
    <input type="email" id="email-address" name="email"
           value="alice@techsolutions.com" class="field-input" />
</div>

<div class="form-field">
    <label for="phone-number" class="field-label">Phone Number:</label>
    <input type="tel" id="phone-number" name="phone"
           value="(555) 987-6543" class="field-input" style="width: 250pt;" />
</div>

<div class="form-field">
    <label for="comments-field" class="field-label">Comments:</label>
    <input type="text" id="comments-field" name="comments"
           value="Looking forward to working together on this project."
           options="MultiLine"
           style="width: 100%; height: 80pt; padding: 10pt;
                  border: 1pt solid #cccccc; font-size: 10pt;" />
</div>
```

### Checkbox Labels with For Attribute

```html
<!-- Checkboxes with labels after -->
<div style="padding: 15pt; border: 1pt solid #e0e0e0; background-color: #f9f9f9;">
    <label style="font-weight: bold; display: block; margin-bottom: 10pt;">
        Communication Preferences:
    </label>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" id="pref-email" name="emailNotifications" value="checked"
               style="width: 15pt; height: 15pt;" />
        <label for="pref-email" style="margin-left: 8pt;">
            Send me email notifications
        </label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" id="pref-sms" name="smsAlerts" value="checked"
               style="width: 15pt; height: 15pt;" />
        <label for="pref-sms" style="margin-left: 8pt;">
            Send me SMS alerts
        </label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" id="pref-push" name="pushNotifications" value=""
               style="width: 15pt; height: 15pt;" />
        <label for="pref-push" style="margin-left: 8pt;">
            Enable push notifications
        </label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" id="pref-newsletter" name="newsletter" value="checked"
               style="width: 15pt; height: 15pt;" />
        <label for="pref-newsletter" style="margin-left: 8pt;">
            Subscribe to weekly newsletter
        </label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" id="pref-marketing" name="marketing" value=""
               style="width: 15pt; height: 15pt;" />
        <label for="pref-marketing" style="margin-left: 8pt;">
            Receive promotional offers
        </label>
    </div>
</div>
```

### Radio Button Groups with For Attribute

```html
<!-- Payment method selection -->
<div style="margin-bottom: 20pt; padding: 15pt; border: 1pt solid #e0e0e0;">
    <label style="font-weight: bold; display: block; margin-bottom: 10pt;">
        Select Payment Method:
    </label>

    <div style="margin-bottom: 8pt;">
        <input type="radio" id="pay-credit" name="paymentMethod" value="selected"
               style="width: 15pt; height: 15pt;" />
        <label for="pay-credit" style="margin-left: 8pt; font-weight: bold; color: #336699;">
            Credit Card (Selected)
        </label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="radio" id="pay-debit" name="paymentMethod" value=""
               style="width: 15pt; height: 15pt;" />
        <label for="pay-debit" style="margin-left: 8pt;">
            Debit Card
        </label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="radio" id="pay-paypal" name="paymentMethod" value=""
               style="width: 15pt; height: 15pt;" />
        <label for="pay-paypal" style="margin-left: 8pt;">
            PayPal
        </label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="radio" id="pay-bank" name="paymentMethod" value=""
               style="width: 15pt; height: 15pt;" />
        <label for="pay-bank" style="margin-left: 8pt;">
            Bank Transfer
        </label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="radio" id="pay-cash" name="paymentMethod" value=""
               style="width: 15pt; height: 15pt;" />
        <label for="pay-cash" style="margin-left: 8pt;">
            Cash on Delivery
        </label>
    </div>
</div>

<!-- Shipping speed selection -->
<div style="margin-bottom: 20pt; padding: 15pt; border: 1pt solid #e0e0e0;">
    <label style="font-weight: bold; display: block; margin-bottom: 10pt;">
        Select Shipping Speed:
    </label>

    <div style="margin-bottom: 8pt;">
        <input type="radio" id="ship-standard" name="shippingSpeed" value=""
               style="width: 15pt; height: 15pt;" />
        <label for="ship-standard" style="margin-left: 8pt;">
            Standard (5-7 business days) - Free
        </label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="radio" id="ship-express" name="shippingSpeed" value="selected"
               style="width: 15pt; height: 15pt;" />
        <label for="ship-express" style="margin-left: 8pt; font-weight: bold; color: #336699;">
            Express (2-3 business days) - $9.99 (Selected)
        </label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="radio" id="ship-overnight" name="shippingSpeed" value=""
               style="width: 15pt; height: 15pt;" />
        <label for="ship-overnight" style="margin-left: 8pt;">
            Overnight (1 business day) - $24.99
        </label>
    </div>
</div>
```

### Registration Form with For Attributes

```html
<h2 style="color: #336699; border-bottom: 3pt solid #336699; padding-bottom: 10pt;">
    New Account Registration
</h2>

<fieldset style="border: 2pt solid #e0e0e0; padding: 15pt; margin-bottom: 20pt;">
    <legend style="font-weight: bold; font-size: 12pt; color: #336699; padding: 0 10pt;">
        Personal Information
    </legend>

    <div style="margin-bottom: 12pt;">
        <label for="reg-fullname" style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Full Name:
        </label>
        <input type="text" id="reg-fullname" name="fullName" value="Robert Martinez"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label for="reg-dob" style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Date of Birth:
        </label>
        <input type="date" id="reg-dob" name="birthDate" value="1988-07-20"
               style="width: 180pt; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 8pt;">
            Gender:
        </label>
        <div style="margin-left: 15pt;">
            <div style="margin-bottom: 6pt;">
                <input type="radio" id="gender-male" name="gender" value="selected"
                       style="width: 15pt; height: 15pt;" />
                <label for="gender-male" style="margin-left: 8pt;">Male</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="radio" id="gender-female" name="gender" value=""
                       style="width: 15pt; height: 15pt;" />
                <label for="gender-female" style="margin-left: 8pt;">Female</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="radio" id="gender-other" name="gender" value=""
                       style="width: 15pt; height: 15pt;" />
                <label for="gender-other" style="margin-left: 8pt;">Other</label>
            </div>
        </div>
    </div>
</fieldset>

<fieldset style="border: 2pt solid #e0e0e0; padding: 15pt; margin-bottom: 20pt;">
    <legend style="font-weight: bold; font-size: 12pt; color: #336699; padding: 0 10pt;">
        Contact Information
    </legend>

    <div style="margin-bottom: 12pt;">
        <label for="reg-email" style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Email Address:
        </label>
        <input type="email" id="reg-email" name="email" value="r.martinez@email.com"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label for="reg-phone" style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Phone Number:
        </label>
        <input type="tel" id="reg-phone" name="phone" value="(555) 456-7890"
               style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label for="reg-address" style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Street Address:
        </label>
        <input type="text" id="reg-address" name="address" value="456 Elm Street, Apt 12"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <div style="display: inline-block; width: 48%; margin-right: 2%;">
            <label for="reg-city" style="display: block; font-weight: bold; margin-bottom: 5pt;">
                City:
            </label>
            <input type="text" id="reg-city" name="city" value="Seattle"
                   style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
        </div>

        <div style="display: inline-block; width: 24%; margin-right: 2%;">
            <label for="reg-state" style="display: block; font-weight: bold; margin-bottom: 5pt;">
                State:
            </label>
            <input type="text" id="reg-state" name="state" value="WA"
                   style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;
                          text-align: center;" />
        </div>

        <div style="display: inline-block; width: 24%;">
            <label for="reg-zip" style="display: block; font-weight: bold; margin-bottom: 5pt;">
                ZIP:
            </label>
            <input type="text" id="reg-zip" name="zip" value="98101"
                   style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;
                          text-align: center;" />
        </div>
    </div>
</fieldset>

<fieldset style="border: 2pt solid #e0e0e0; padding: 15pt; margin-bottom: 20pt;">
    <legend style="font-weight: bold; font-size: 12pt; color: #336699; padding: 0 10pt;">
        Account Settings
    </legend>

    <div style="margin-bottom: 12pt;">
        <label for="reg-username" style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Username:
        </label>
        <input type="text" id="reg-username" name="username" value="rmartinez2024"
               style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;
                      font-family: monospace;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label for="reg-password" style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Password:
        </label>
        <input type="password" id="reg-password" name="password" value="••••••••••"
               style="width: 250pt; padding: 8pt; border: 1pt solid #cccccc;
                      font-family: monospace;" />
    </div>
</fieldset>

<fieldset style="border: 2pt solid #e0e0e0; padding: 15pt; margin-bottom: 20pt;">
    <legend style="font-weight: bold; font-size: 12pt; color: #336699; padding: 0 10pt;">
        Preferences
    </legend>

    <div style="margin-bottom: 15pt;">
        <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
            Newsletter Subscriptions:
        </label>
        <div style="margin-left: 15pt;">
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" id="pref-weekly" name="weeklyNewsletter" value="checked"
                       style="width: 15pt; height: 15pt;" />
                <label for="pref-weekly" style="margin-left: 8pt;">Weekly newsletter</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" id="pref-monthly" name="monthlyDigest" value="checked"
                       style="width: 15pt; height: 15pt;" />
                <label for="pref-monthly" style="margin-left: 8pt;">Monthly digest</label>
            </div>
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" id="pref-updates" name="productUpdates" value=""
                       style="width: 15pt; height: 15pt;" />
                <label for="pref-updates" style="margin-left: 8pt;">Product updates</label>
            </div>
        </div>
    </div>
</fieldset>

<div style="margin-bottom: 20pt;">
    <input type="checkbox" id="agree-terms" name="agreeTerms" value="checked"
           style="width: 15pt; height: 15pt;" />
    <label for="agree-terms" style="margin-left: 8pt; font-weight: bold;">
        I agree to the Terms of Service and Privacy Policy
    </label>
</div>
```

### Survey Form with For Attributes

```html
<h2 style="color: #336699;">Employee Satisfaction Survey</h2>

<div style="margin-bottom: 20pt;">
    <label for="survey-name" style="display: block; font-weight: bold; margin-bottom: 8pt;">
        1. Your Name:
    </label>
    <input type="text" id="survey-name" name="respondentName" value="Jennifer Lee"
           style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 20pt;">
    <label for="survey-dept" style="display: block; font-weight: bold; margin-bottom: 8pt;">
        2. Department:
    </label>
    <input type="text" id="survey-dept" name="department" value="Engineering"
           style="width: 300pt; padding: 8pt; border: 1pt solid #cccccc;" />
</div>

<div style="margin-bottom: 20pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 8pt;">
        3. How satisfied are you with your current role?
    </label>
    <div style="margin-left: 15pt;">
        <div style="margin-bottom: 6pt;">
            <input type="radio" id="role-very-satisfied" name="roleSatisfaction" value=""
                   style="width: 15pt; height: 15pt;" />
            <label for="role-very-satisfied" style="margin-left: 8pt;">Very Satisfied</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="radio" id="role-satisfied" name="roleSatisfaction" value="selected"
                   style="width: 15pt; height: 15pt;" />
            <label for="role-satisfied" style="margin-left: 8pt;">Satisfied</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="radio" id="role-neutral" name="roleSatisfaction" value=""
                   style="width: 15pt; height: 15pt;" />
            <label for="role-neutral" style="margin-left: 8pt;">Neutral</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="radio" id="role-dissatisfied" name="roleSatisfaction" value=""
                   style="width: 15pt; height: 15pt;" />
            <label for="role-dissatisfied" style="margin-left: 8pt;">Dissatisfied</label>
        </div>
    </div>
</div>

<div style="margin-bottom: 20pt;">
    <label style="display: block; font-weight: bold; margin-bottom: 8pt;">
        4. Which benefits do you value most? (Select all that apply)
    </label>
    <div style="margin-left: 15pt;">
        <div style="margin-bottom: 6pt;">
            <input type="checkbox" id="benefit-health" name="healthInsurance" value="checked"
                   style="width: 15pt; height: 15pt;" />
            <label for="benefit-health" style="margin-left: 8pt;">Health Insurance</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="checkbox" id="benefit-401k" name="retirement401k" value="checked"
                   style="width: 15pt; height: 15pt;" />
            <label for="benefit-401k" style="margin-left: 8pt;">401(k) Retirement Plan</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="checkbox" id="benefit-pto" name="paidTimeOff" value="checked"
                   style="width: 15pt; height: 15pt;" />
            <label for="benefit-pto" style="margin-left: 8pt;">Paid Time Off</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="checkbox" id="benefit-remote" name="remoteWork" value=""
                   style="width: 15pt; height: 15pt;" />
            <label for="benefit-remote" style="margin-left: 8pt;">Remote Work Options</label>
        </div>
        <div style="margin-bottom: 6pt;">
            <input type="checkbox" id="benefit-training" name="profDevelopment" value="checked"
                   style="width: 15pt; height: 15pt;" />
            <label for="benefit-training" style="margin-left: 8pt;">
                Professional Development Training
            </label>
        </div>
    </div>
</div>

<div style="margin-bottom: 20pt;">
    <label for="survey-suggestions" style="display: block; font-weight: bold; margin-bottom: 8pt;">
        5. What suggestions do you have for improving the workplace?
    </label>
    <input type="text" id="survey-suggestions" name="suggestions"
           value="More flexible working hours would be beneficial. Also, additional team-building activities would help improve collaboration."
           options="MultiLine"
           style="width: 100%; height: 100pt; padding: 10pt;
                  border: 1pt solid #cccccc; font-size: 10pt;" />
</div>
```

### Invoice Form with For Attributes

```html
<div style="padding: 20pt; border: 2pt solid #336699;">
    <h2 style="color: #336699; margin-top: 0;">INVOICE</h2>

    <fieldset style="border: 1pt solid #e0e0e0; padding: 15pt; margin-bottom: 15pt;">
        <legend style="font-weight: bold; color: #336699; padding: 0 10pt;">
            Bill To
        </legend>

        <div style="margin-bottom: 10pt;">
            <label for="inv-customer" style="display: inline-block; width: 140pt; font-weight: bold;">
                Customer Name:
            </label>
            <input type="text" id="inv-customer" name="customerName" value="Global Tech Corp"
                   style="width: 300pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>

        <div style="margin-bottom: 10pt;">
            <label for="inv-address" style="display: inline-block; width: 140pt; font-weight: bold;">
                Address:
            </label>
            <input type="text" id="inv-address" name="address" value="123 Business Blvd"
                   style="width: 350pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>

        <div style="margin-bottom: 10pt;">
            <label for="inv-city-state" style="display: inline-block; width: 140pt; font-weight: bold;">
                City, State ZIP:
            </label>
            <input type="text" id="inv-city-state" name="cityStateZip"
                   value="San Francisco, CA 94102"
                   style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>

        <div style="margin-bottom: 10pt;">
            <label for="inv-email" style="display: inline-block; width: 140pt; font-weight: bold;">
                Email:
            </label>
            <input type="email" id="inv-email" name="email" value="billing@globaltech.com"
                   style="width: 280pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>
    </fieldset>

    <fieldset style="border: 1pt solid #e0e0e0; padding: 15pt; margin-bottom: 15pt;">
        <legend style="font-weight: bold; color: #336699; padding: 0 10pt;">
            Invoice Details
        </legend>

        <div style="margin-bottom: 10pt;">
            <label for="inv-number" style="display: inline-block; width: 140pt; font-weight: bold;">
                Invoice Number:
            </label>
            <input type="text" id="inv-number" name="invoiceNumber" value="INV-2024-0456"
                   style="width: 180pt; padding: 6pt; border: 1pt solid #cccccc;
                          font-family: monospace; font-weight: bold;
                          background-color: #ffffcc;" />
        </div>

        <div style="margin-bottom: 10pt;">
            <label for="inv-date" style="display: inline-block; width: 140pt; font-weight: bold;">
                Invoice Date:
            </label>
            <input type="date" id="inv-date" name="invoiceDate" value="2024-01-20"
                   style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>

        <div style="margin-bottom: 10pt;">
            <label for="inv-due" style="display: inline-block; width: 140pt; font-weight: bold;">
                Due Date:
            </label>
            <input type="date" id="inv-due" name="dueDate" value="2024-02-20"
                   style="width: 150pt; padding: 6pt; border: 1pt solid #cccccc;
                          color: #d32f2f; font-weight: bold;" />
        </div>

        <div style="margin-bottom: 10pt;">
            <label for="inv-terms" style="display: inline-block; width: 140pt; font-weight: bold;">
                Payment Terms:
            </label>
            <input type="text" id="inv-terms" name="paymentTerms" value="Net 30"
                   style="width: 120pt; padding: 6pt; border: 1pt solid #cccccc;" />
        </div>
    </fieldset>
</div>
```

### Data-Bound Form with Dynamic For Values

```html
<!-- Model: {
    formFields: [
        {id: "field-001", label: "Company Name", value: "Acme Corp", type: "text"},
        {id: "field-002", label: "Industry", value: "Technology", type: "text"},
        {id: "field-003", label: "Founded", value: "2010", type: "number"},
        {id: "field-004", label: "Website", value: "https://acme.com", type: "url"}
    ]
} -->

<h3 style="color: #336699;">Company Information</h3>

<template data-bind="{{model.formFields}}">
    <div style="margin-bottom: 12pt;">
        <label for="{{.id}}" style="display: block; font-weight: bold; margin-bottom: 5pt;">
            {{.label}}:
        </label>
        <input type="{{.type}}" id="{{.id}}" name="{{.id}}" value="{{.value}}"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>
</template>
```

### Inline Labels with For Attribute

```html
<div style="padding: 15pt; border: 1pt solid #e0e0e0; background-color: #f9f9f9;">
    <h3 style="margin-top: 0; color: #336699;">Quick Contact Form</h3>

    <div style="margin-bottom: 10pt;">
        <label for="quick-name" style="display: inline-block; width: 80pt; font-weight: bold;">
            Name:
        </label>
        <input type="text" id="quick-name" name="name" value="Sarah Chen"
               style="width: 220pt; padding: 6pt; border: 1pt solid #cccccc;
                      margin-right: 15pt;" />

        <label for="quick-email" style="display: inline-block; width: 60pt; font-weight: bold;">
            Email:
        </label>
        <input type="email" id="quick-email" name="email" value="sarah.chen@email.com"
               style="width: 240pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 0;">
        <label for="quick-message" style="display: inline-block; width: 80pt; font-weight: bold;
                      vertical-align: top;">
            Message:
        </label>
        <input type="text" id="quick-message" name="message"
               value="I'm interested in learning more about your services."
               style="width: 550pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>
</div>
```

---

## See Also

- [label](/reference/htmltags/label.html) - Label element for form fields
- [input](/reference/htmltags/input.html) - Input field element
- [name](/reference/htmlattributes/name.html) - Name attribute for form fields
- [type](/reference/htmlattributes/type.html) - Input type attribute
- [value](/reference/htmlattributes/value.html) - Value attribute for form fields
- [fieldset](/reference/htmltags/fieldset.html) - Fieldset and legend for grouping
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
