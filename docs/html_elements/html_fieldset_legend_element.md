---
layout: default
title: fieldset and legend
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;fieldset&gt; and &lt;legend&gt; : Form Grouping Elements

The `<fieldset>` and `<legend>` elements are used together to create visually grouped sections within forms or form-like layouts. The `<fieldset>` element creates a bordered container, while the `<legend>` element provides a caption or title that appears embedded in the fieldset's border. These elements are essential for organizing complex forms into logical sections.

## Usage

The `<fieldset>` element creates a container that:
- Groups related form elements together
- Displays a border around its contents by default
- Can contain a `<legend>` as its first child for labeling
- Supports full CSS styling and customization
- Creates semantic structure for form sections
- Works with any HTML content, not just form elements

The `<legend>` element provides a caption that:
- Appears at the top of the fieldset border
- Acts as a title or label for the grouped content
- Is typically the first child of a `<fieldset>`
- Can be styled independently
- Has auto-width by default (doesn't span full width)

```html
<fieldset>
    <legend>Personal Information</legend>
    <label>Name:</label>
    <input type="text" value="John Doe" />
</fieldset>
```

---

## Supported Attributes

### Fieldset Attributes

#### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the fieldset. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

#### CSS Style Support for Fieldset

**Sizing**:
- `width`, `min-width`, `max-width`
- `height`, `min-height`, `max-height`
- Default: Auto-sized to content

**Positioning**:
- `display`: `block` (default), `inline-block`, `none`
- `position`: `static`, `relative`, `absolute`
- `float`: `left`, `right`, `none`

**Spacing**:
- `margin` (all variants) - Default: `2pt left/right`, `1em top`
- `padding` (all variants) - Default: `0.35em top`, `0.625em bottom`, `0.75em left/right`

**Visual Styling**:
- `border`, `border-width`, `border-color`, `border-style` - Default: `2pt solid`
- `border-radius` - rounded corners
- `background-color`, `background-image`
- `color` - text color (inherited by contents)

**Typography** (inherited by contents):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `line-height`, `text-align`

### Legend Attributes

#### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the legend. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the legend. |

#### CSS Style Support for Legend

**Sizing**:
- `width` - Default: Auto-width (not full width)
- `padding` (all variants)

**Positioning**:
- `display`: `block`, `inline-block`
- `text-align`: `left`, `center`, `right`

**Visual Styling**:
- `color`, `background-color`
- `border`, `padding`
- `font-family`, `font-size`, `font-weight`, `font-style`

---

## Notes

### Default Styling

**Fieldset defaults**:
- Margin: `2pt` left and right, `1em` top
- Padding: `0.35em` top, `0.625em` bottom, `0.75em` left/right
- Border: `2pt solid` (default color)
- Display: `block`

**Legend defaults**:
- Width: Auto (not full-width)
- Position: Embedded in top border
- Display: `block` (but auto-sized)

### Proper Structure

The `<legend>` should be the **first child** of the `<fieldset>`:

```html
<!-- Correct structure -->
<fieldset>
    <legend>Section Title</legend>
    <!-- Other content -->
</fieldset>

<!-- Incorrect - legend not first -->
<fieldset>
    <div>Some content</div>
    <legend>Section Title</legend>  <!-- Will still work but not semantically correct -->
</fieldset>
```

### Legend Positioning

The `<legend>` appears embedded in the top border of the fieldset. Its position can be influenced by:
- `text-align` on the fieldset (moves legend left/center/right)
- `padding` on the legend (adds space around the text)
- `margin` on the legend (adjusts position)

### Nesting Fieldsets

Fieldsets can be nested to create hierarchical form sections:

```html
<fieldset>
    <legend>Shipping Information</legend>

    <fieldset>
        <legend>Address</legend>
        <!-- Address fields -->
    </fieldset>

    <fieldset>
        <legend>Delivery Options</legend>
        <!-- Delivery fields -->
    </fieldset>
</fieldset>
```

### Styling Tips

1. **Remove border**: Set `border: none` for invisible grouping
2. **Custom borders**: Use any border style, width, and color
3. **Background colors**: Add backgrounds to distinguish sections
4. **Legend styling**: Style legends independently for emphasis
5. **Padding control**: Adjust padding for spacing within the fieldset

### Class Hierarchy

In the Scryber codebase:
- `HTMLFieldSet` extends `Div` (block container)
- `HTMLLegend` extends `Div` with auto-width enabled
- Both support full HTML attributes and CSS styling

---

## Examples

### Basic Fieldset with Legend

```html
<fieldset>
    <legend>Contact Information</legend>

    <div style="margin-bottom: 10pt;">
        <label style="display: inline-block; width: 100pt;">Name:</label>
        <input type="text" value="John Doe" style="width: 200pt;" />
    </div>

    <div style="margin-bottom: 10pt;">
        <label style="display: inline-block; width: 100pt;">Email:</label>
        <input type="text" value="john@example.com" style="width: 200pt;" />
    </div>

    <div style="margin-bottom: 10pt;">
        <label style="display: inline-block; width: 100pt;">Phone:</label>
        <input type="text" value="(555) 123-4567" style="width: 200pt;" />
    </div>
</fieldset>
```

### Styled Fieldset

```html
<fieldset style="border: 2pt solid #336699; border-radius: 5pt;
                 padding: 15pt; background-color: #f9f9f9;">
    <legend style="color: #336699; font-weight: bold; font-size: 14pt;
                   padding: 0 10pt;">
        Personal Details
    </legend>

    <div style="margin-bottom: 10pt;">
        <label style="font-weight: bold;">Full Name:</label>
        <input type="text" value="Jane Smith" style="width: 100%; margin-top: 5pt;
               padding: 8pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 10pt;">
        <label style="font-weight: bold;">Date of Birth:</label>
        <input type="text" value="03/15/1990" style="width: 150pt; margin-top: 5pt;
               padding: 8pt; border: 1pt solid #cccccc;" />
    </div>
</fieldset>
```

### Multiple Fieldsets in a Form

```html
<style>
    .form-fieldset {
        border: 2pt solid #cccccc;
        border-radius: 4pt;
        padding: 15pt;
        margin-bottom: 20pt;
        background-color: white;
    }
    .form-legend {
        font-weight: bold;
        font-size: 13pt;
        color: #2c5f8d;
        padding: 0 8pt;
    }
    .field-row {
        margin-bottom: 12pt;
    }
    .field-label {
        display: inline-block;
        width: 130pt;
        font-weight: bold;
        color: #555555;
    }
</style>

<fieldset class="form-fieldset">
    <legend class="form-legend">Account Information</legend>

    <div class="field-row">
        <label class="field-label">Username:</label>
        <input type="text" value="jsmith" style="width: 200pt; padding: 6pt;
               border: 1pt solid #cccccc;" />
    </div>

    <div class="field-row">
        <label class="field-label">Email:</label>
        <input type="text" value="jane.smith@example.com" style="width: 300pt;
               padding: 6pt; border: 1pt solid #cccccc;" />
    </div>

    <div class="field-row">
        <label class="field-label">Password:</label>
        <input type="password" value="********" style="width: 200pt; padding: 6pt;
               border: 1pt solid #cccccc;" />
    </div>
</fieldset>

<fieldset class="form-fieldset">
    <legend class="form-legend">Personal Information</legend>

    <div class="field-row">
        <label class="field-label">First Name:</label>
        <input type="text" value="Jane" style="width: 200pt; padding: 6pt;
               border: 1pt solid #cccccc;" />
    </div>

    <div class="field-row">
        <label class="field-label">Last Name:</label>
        <input type="text" value="Smith" style="width: 200pt; padding: 6pt;
               border: 1pt solid #cccccc;" />
    </div>

    <div class="field-row">
        <label class="field-label">Phone:</label>
        <input type="text" value="(555) 123-4567" style="width: 200pt; padding: 6pt;
               border: 1pt solid #cccccc;" />
    </div>
</fieldset>

<fieldset class="form-fieldset">
    <legend class="form-legend">Preferences</legend>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Receive email notifications</label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Subscribe to newsletter</label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" value="" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Enable two-factor authentication</label>
    </div>
</fieldset>
```

### Nested Fieldsets

```html
<fieldset style="border: 2pt solid #2c5f8d; padding: 20pt;">
    <legend style="font-weight: bold; font-size: 14pt; color: #2c5f8d;
                   padding: 0 10pt;">
        Shipping Information
    </legend>

    <fieldset style="border: 1pt solid #999999; padding: 12pt; margin-bottom: 15pt;">
        <legend style="font-weight: bold; color: #555555; padding: 0 8pt;">
            Recipient Details
        </legend>

        <div style="margin-bottom: 10pt;">
            <label style="display: inline-block; width: 120pt;">Full Name:</label>
            <input type="text" value="John Doe" style="width: 250pt; padding: 6pt;
                   border: 1pt solid #cccccc;" />
        </div>

        <div style="margin-bottom: 10pt;">
            <label style="display: inline-block; width: 120pt;">Phone:</label>
            <input type="text" value="(555) 987-6543" style="width: 200pt; padding: 6pt;
                   border: 1pt solid #cccccc;" />
        </div>
    </fieldset>

    <fieldset style="border: 1pt solid #999999; padding: 12pt; margin-bottom: 15pt;">
        <legend style="font-weight: bold; color: #555555; padding: 0 8pt;">
            Delivery Address
        </legend>

        <div style="margin-bottom: 10pt;">
            <label style="display: inline-block; width: 120pt;">Street Address:</label>
            <input type="text" value="123 Main Street" style="width: 300pt; padding: 6pt;
                   border: 1pt solid #cccccc;" />
        </div>

        <div style="margin-bottom: 10pt;">
            <label style="display: inline-block; width: 120pt;">City:</label>
            <input type="text" value="Springfield" style="width: 200pt; padding: 6pt;
                   border: 1pt solid #cccccc;" />
        </div>

        <div style="margin-bottom: 10pt;">
            <label style="display: inline-block; width: 120pt;">State:</label>
            <input type="text" value="IL" style="width: 80pt; padding: 6pt;
                   border: 1pt solid #cccccc; margin-right: 15pt;" />
            <label style="margin-right: 8pt;">ZIP:</label>
            <input type="text" value="62701" style="width: 100pt; padding: 6pt;
                   border: 1pt solid #cccccc;" />
        </div>
    </fieldset>

    <fieldset style="border: 1pt solid #999999; padding: 12pt;">
        <legend style="font-weight: bold; color: #555555; padding: 0 8pt;">
            Delivery Options
        </legend>

        <div style="margin-bottom: 8pt;">
            <input type="radio" value="selected" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Standard Shipping (5-7 days) - Free</label>
        </div>

        <div style="margin-bottom: 8pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Express Shipping (2-3 days) - $15.00</label>
        </div>

        <div style="margin-bottom: 8pt;">
            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">Overnight Shipping (1 day) - $30.00</label>
        </div>
    </fieldset>
</fieldset>
```

### Borderless Fieldset for Logical Grouping

```html
<fieldset style="border: none; padding: 0; margin: 0 0 20pt 0;">
    <legend style="font-size: 16pt; font-weight: bold; color: #336699;
                   padding: 0; margin-bottom: 15pt; border-bottom: 2pt solid #336699;
                   width: 100%;">
        Educational Background
    </legend>

    <div style="margin-bottom: 12pt;">
        <label style="display: inline-block; width: 150pt; font-weight: bold;">
            Highest Degree:
        </label>
        <input type="text" value="Master of Science" style="width: 250pt; padding: 6pt;
               border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: inline-block; width: 150pt; font-weight: bold;">
            Field of Study:
        </label>
        <input type="text" value="Computer Science" style="width: 250pt; padding: 6pt;
               border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: inline-block; width: 150pt; font-weight: bold;">
            Institution:
        </label>
        <input type="text" value="State University" style="width: 300pt; padding: 6pt;
               border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: inline-block; width: 150pt; font-weight: bold;">
            Graduation Year:
        </label>
        <input type="text" value="2015" style="width: 100pt; padding: 6pt;
               border: 1pt solid #cccccc;" />
    </div>
</fieldset>
```

### Fieldset with Background Color

```html
<fieldset style="border: 2pt solid #28a745; border-radius: 8pt;
                 padding: 20pt; background-color: #e8f5e9;">
    <legend style="background-color: #28a745; color: white; padding: 8pt 15pt;
                   border-radius: 4pt; font-weight: bold; font-size: 13pt;">
        Payment Information
    </legend>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Cardholder Name:
        </label>
        <input type="text" value="John Doe" style="width: 100%; padding: 8pt;
               border: 1pt solid #cccccc; background-color: white;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Card Number:
        </label>
        <input type="text" value="**** **** **** 1234" style="width: 100%; padding: 8pt;
               border: 1pt solid #cccccc; background-color: white;
               font-family: monospace;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <div style="display: inline-block; width: 48%; margin-right: 2%;">
            <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
                Expiration Date:
            </label>
            <input type="text" value="12/25" style="width: 100%; padding: 8pt;
                   border: 1pt solid #cccccc; background-color: white;" />
        </div>

        <div style="display: inline-block; width: 48%;">
            <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
                CVV:
            </label>
            <input type="text" value="***" style="width: 100%; padding: 8pt;
                   border: 1pt solid #cccccc; background-color: white;
                   font-family: monospace;" />
        </div>
    </div>
</fieldset>
```

### Fieldset with Radio Button Group

```html
<fieldset style="border: 2pt solid #666666; padding: 15pt;">
    <legend style="font-weight: bold; font-size: 12pt; padding: 0 8pt;">
        Subscription Plan
    </legend>

    <div style="margin-bottom: 12pt; padding: 10pt; border: 1pt solid #e0e0e0;
                background-color: #f9f9f9;">
        <div style="margin-bottom: 5pt;">
            <input type="radio" value="selected" name="plan" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt; font-weight: bold; font-size: 12pt;">
                Basic Plan - $9.99/month
            </label>
        </div>
        <p style="margin: 5pt 0 0 23pt; color: #666666; font-size: 10pt;">
            Access to basic features and email support
        </p>
    </div>

    <div style="margin-bottom: 12pt; padding: 10pt; border: 1pt solid #e0e0e0;
                background-color: #f9f9f9;">
        <div style="margin-bottom: 5pt;">
            <input type="radio" value="" name="plan" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt; font-weight: bold; font-size: 12pt;">
                Professional Plan - $29.99/month
            </label>
        </div>
        <p style="margin: 5pt 0 0 23pt; color: #666666; font-size: 10pt;">
            All features plus priority support and advanced analytics
        </p>
    </div>

    <div style="margin-bottom: 0; padding: 10pt; border: 1pt solid #e0e0e0;
                background-color: #f9f9f9;">
        <div style="margin-bottom: 5pt;">
            <input type="radio" value="" name="plan" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt; font-weight: bold; font-size: 12pt;">
                Enterprise Plan - $99.99/month
            </label>
        </div>
        <p style="margin: 5pt 0 0 23pt; color: #666666; font-size: 10pt;">
            Unlimited access, dedicated support, and custom integrations
        </p>
    </div>
</fieldset>
```

### Fieldset with Checkbox Group

```html
<fieldset style="border: 2pt solid #9c27b0; padding: 15pt; border-radius: 5pt;">
    <legend style="color: #9c27b0; font-weight: bold; font-size: 13pt; padding: 0 10pt;">
        Skills and Expertise
    </legend>

    <p style="margin-top: 0; margin-bottom: 12pt; color: #666666;">
        Select all that apply:
    </p>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">JavaScript / TypeScript</label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Python</label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" value="" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Java / C#</label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">SQL / Database Management</label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Cloud Services (AWS, Azure, GCP)</label>
    </div>

    <div style="margin-bottom: 8pt;">
        <input type="checkbox" value="" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">Mobile Development</label>
    </div>

    <div style="margin-bottom: 0;">
        <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt;">DevOps / CI/CD</label>
    </div>
</fieldset>
```

### Data-Bound Fieldset

```html
<!-- Model: {
    personalInfo: {
        firstName: "Alice",
        lastName: "Johnson",
        email: "alice@example.com",
        phone: "(555) 444-3333"
    }
} -->

<fieldset style="border: 2pt solid #336699; padding: 18pt; border-radius: 5pt;">
    <legend style="color: #336699; font-weight: bold; font-size: 14pt; padding: 0 10pt;">
        Contact Details
    </legend>

    <div style="margin-bottom: 12pt;">
        <label style="display: inline-block; width: 120pt; font-weight: bold;">
            First Name:
        </label>
        <input type="text" value="{{model.personalInfo.firstName}}"
               style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: inline-block; width: 120pt; font-weight: bold;">
            Last Name:
        </label>
        <input type="text" value="{{model.personalInfo.lastName}}"
               style="width: 250pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: inline-block; width: 120pt; font-weight: bold;">
            Email:
        </label>
        <input type="text" value="{{model.personalInfo.email}}"
               style="width: 300pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 0;">
        <label style="display: inline-block; width: 120pt; font-weight: bold;">
            Phone:
        </label>
        <input type="text" value="{{model.personalInfo.phone}}"
               style="width: 200pt; padding: 6pt; border: 1pt solid #cccccc;" />
    </div>
</fieldset>
```

### Medical Form with Multiple Fieldsets

```html
<style>
    .medical-fieldset {
        border: 2pt solid #2c5f8d;
        padding: 15pt;
        margin-bottom: 18pt;
        border-radius: 5pt;
        background-color: #f8f9fa;
    }
    .medical-legend {
        background-color: #2c5f8d;
        color: white;
        padding: 8pt 12pt;
        border-radius: 4pt;
        font-weight: bold;
        font-size: 12pt;
    }
    .med-field {
        margin-bottom: 12pt;
    }
    .med-label {
        display: inline-block;
        width: 160pt;
        font-weight: bold;
        color: #333333;
    }
</style>

<h1 style="text-align: center; color: #2c5f8d; margin-bottom: 25pt;">
    PATIENT MEDICAL HISTORY
</h1>

<fieldset class="medical-fieldset">
    <legend class="medical-legend">Patient Demographics</legend>

    <div class="med-field">
        <label class="med-label">Patient Name:</label>
        <input type="text" value="Robert Williams" style="width: 280pt; padding: 6pt;
               border: 1pt solid #999999; background-color: white;" />
    </div>

    <div class="med-field">
        <label class="med-label">Date of Birth:</label>
        <input type="text" value="06/12/1968" style="width: 120pt; padding: 6pt;
               border: 1pt solid #999999; background-color: white;" />
        <label style="margin-left: 15pt; margin-right: 8pt; font-weight: bold;">Age:</label>
        <input type="text" value="55" style="width: 60pt; padding: 6pt;
               border: 1pt solid #999999; background-color: white;" />
    </div>

    <div class="med-field">
        <label class="med-label">Gender:</label>
        <input type="radio" value="selected" style="width: 13pt; height: 13pt;" />
        <label style="margin-left: 5pt; margin-right: 15pt;">Male</label>
        <input type="radio" value="" style="width: 13pt; height: 13pt;" />
        <label style="margin-left: 5pt; margin-right: 15pt;">Female</label>
        <input type="radio" value="" style="width: 13pt; height: 13pt;" />
        <label style="margin-left: 5pt;">Other</label>
    </div>

    <div class="med-field">
        <label class="med-label">Blood Type:</label>
        <input type="text" value="O+" style="width: 80pt; padding: 6pt;
               border: 1pt solid #999999; background-color: white;" />
    </div>
</fieldset>

<fieldset class="medical-fieldset">
    <legend class="medical-legend">Current Medications</legend>

    <div class="med-field">
        <label style="display: block; font-weight: bold; margin-bottom: 6pt;">
            List all current medications:
        </label>
        <input type="text"
               value="Metformin 500mg - twice daily&#10;Lisinopril 10mg - once daily&#10;Atorvastatin 20mg - once daily at bedtime"
               options="MultiLine"
               style="width: 100%; height: 80pt; padding: 10pt;
                      border: 1pt solid #999999; background-color: white; font-size: 10pt;" />
    </div>
</fieldset>

<fieldset class="medical-fieldset">
    <legend class="medical-legend">Allergies</legend>

    <div style="margin-bottom: 10pt;">
        <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
            Known allergies (check all that apply):
        </label>

        <div style="margin-left: 10pt;">
            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">Penicillin</label>
            </div>

            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">Sulfa drugs</label>
            </div>

            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">Aspirin</label>
            </div>

            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">Latex</label>
            </div>

            <div style="margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">No known allergies</label>
            </div>
        </div>
    </div>

    <div class="med-field">
        <label style="display: block; font-weight: bold; margin-bottom: 6pt;">
            Additional allergy information:
        </label>
        <input type="text"
               value="Severe reaction to sulfa-based antibiotics (rash and difficulty breathing)"
               options="MultiLine"
               style="width: 100%; height: 60pt; padding: 10pt;
                      border: 1pt solid #999999; background-color: white; font-size: 10pt;" />
    </div>
</fieldset>

<fieldset class="medical-fieldset">
    <legend class="medical-legend">Medical Conditions</legend>

    <div style="margin-bottom: 10pt;">
        <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
            Current or past medical conditions:
        </label>

        <div style="margin-left: 10pt;">
            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">Type 2 Diabetes</label>
            </div>
            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">Hypertension</label>
            </div>

            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">Heart Disease</label>
            </div>
            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="checked" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">High Cholesterol</label>
            </div>

            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">Asthma</label>
            </div>
            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">COPD</label>
            </div>

            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">Arthritis</label>
            </div>
            <div style="display: inline-block; width: 48%; margin-bottom: 6pt;">
                <input type="checkbox" value="" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt;">Cancer</label>
            </div>
        </div>
    </div>
</fieldset>
```

### Application Form with Styled Fieldsets

```html
<style>
    .app-fieldset {
        border: 1pt solid #e0e0e0;
        border-left: 4pt solid #336699;
        padding: 18pt;
        margin-bottom: 20pt;
        background-color: white;
        box-shadow: 2pt 2pt 4pt rgba(0,0,0,0.1);
    }
    .app-legend {
        color: #336699;
        font-weight: bold;
        font-size: 13pt;
        padding: 0 10pt;
    }
</style>

<h1 style="color: #336699; border-bottom: 3pt solid #336699; padding-bottom: 10pt;">
    EMPLOYMENT APPLICATION
</h1>

<fieldset class="app-fieldset">
    <legend class="app-legend">Personal Information</legend>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Full Legal Name:
        </label>
        <input type="text" value="Michael Anderson" style="width: 100%; padding: 8pt;
               border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 12pt;">
        <div style="display: inline-block; width: 48%; margin-right: 2%;">
            <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
                Email Address:
            </label>
            <input type="text" value="m.anderson@email.com" style="width: 100%; padding: 8pt;
                   border: 1pt solid #cccccc;" />
        </div>

        <div style="display: inline-block; width: 48%;">
            <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
                Phone Number:
            </label>
            <input type="text" value="(555) 777-8888" style="width: 100%; padding: 8pt;
                   border: 1pt solid #cccccc;" />
        </div>
    </div>

    <div style="margin-bottom: 0;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Home Address:
        </label>
        <input type="text" value="789 Oak Avenue, Apartment 4B, Boston, MA 02101"
               style="width: 100%; padding: 8pt; border: 1pt solid #cccccc;" />
    </div>
</fieldset>

<fieldset class="app-fieldset">
    <legend class="app-legend">Position Applied For</legend>

    <div style="margin-bottom: 12pt;">
        <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
            Select position:
        </label>

        <div style="margin-left: 10pt;">
            <div style="margin-bottom: 6pt;">
                <input type="radio" value="selected" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Senior Software Engineer</label>
            </div>

            <div style="margin-bottom: 6pt;">
                <input type="radio" value="" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Technical Lead</label>
            </div>

            <div style="margin-bottom: 6pt;">
                <input type="radio" value="" style="width: 15pt; height: 15pt;" />
                <label style="margin-left: 8pt;">Engineering Manager</label>
            </div>
        </div>
    </div>

    <div style="margin-bottom: 12pt;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Desired Salary Range:
        </label>
        <input type="text" value="$120,000 - $150,000" style="width: 200pt; padding: 8pt;
               border: 1pt solid #cccccc;" />
    </div>

    <div style="margin-bottom: 0;">
        <label style="display: block; font-weight: bold; margin-bottom: 5pt;">
            Available Start Date:
        </label>
        <input type="text" value="February 1, 2024" style="width: 180pt; padding: 8pt;
               border: 1pt solid #cccccc;" />
    </div>
</fieldset>

<fieldset class="app-fieldset">
    <legend class="app-legend">Work Authorization</legend>

    <div style="margin-bottom: 12pt;">
        <label style="font-weight: bold; display: block; margin-bottom: 8pt;">
            Are you legally authorized to work in the United States?
        </label>

        <div style="margin-left: 10pt;">
            <input type="radio" value="selected" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt; margin-right: 20pt;">Yes</label>

            <input type="radio" value="" style="width: 15pt; height: 15pt;" />
            <label style="margin-left: 8pt;">No</label>
        </div>
    </div>

    <div style="margin-bottom: 0;">
        <input type="checkbox" value="checked" style="width: 15pt; height: 15pt;" />
        <label style="margin-left: 8pt; font-weight: bold;">
            I certify that all information provided in this application is true and complete.
        </label>
    </div>
</fieldset>
```

### Compact Inline Fieldsets

```html
<div style="width: 100%;">
    <div style="display: inline-block; width: 48%; margin-right: 2%;
                vertical-align: top;">
        <fieldset style="border: 2pt solid #336699; padding: 12pt; height: 180pt;">
            <legend style="font-weight: bold; color: #336699; padding: 0 8pt;">
                Billing Address
            </legend>

            <div style="margin-bottom: 10pt;">
                <label style="display: block; font-weight: bold; margin-bottom: 4pt;">
                    Name:
                </label>
                <input type="text" value="John Doe" style="width: 100%; padding: 6pt;
                       border: 1pt solid #cccccc;" />
            </div>

            <div style="margin-bottom: 10pt;">
                <label style="display: block; font-weight: bold; margin-bottom: 4pt;">
                    Street:
                </label>
                <input type="text" value="123 Main St" style="width: 100%; padding: 6pt;
                       border: 1pt solid #cccccc;" />
            </div>

            <div style="margin-bottom: 0;">
                <label style="display: block; font-weight: bold; margin-bottom: 4pt;">
                    City, State ZIP:
                </label>
                <input type="text" value="Boston, MA 02101" style="width: 100%; padding: 6pt;
                       border: 1pt solid #cccccc;" />
            </div>
        </fieldset>
    </div>

    <div style="display: inline-block; width: 48%; vertical-align: top;">
        <fieldset style="border: 2pt solid #336699; padding: 12pt; height: 180pt;">
            <legend style="font-weight: bold; color: #336699; padding: 0 8pt;">
                Shipping Address
            </legend>

            <div style="margin-bottom: 8pt;">
                <input type="checkbox" value="checked" style="width: 13pt; height: 13pt;" />
                <label style="margin-left: 6pt; font-style: italic;">
                    Same as billing address
                </label>
            </div>

            <div style="margin-bottom: 10pt;">
                <label style="display: block; font-weight: bold; margin-bottom: 4pt;">
                    Name:
                </label>
                <input type="text" value="John Doe" style="width: 100%; padding: 6pt;
                       border: 1pt solid #cccccc;" />
            </div>

            <div style="margin-bottom: 10pt;">
                <label style="display: block; font-weight: bold; margin-bottom: 4pt;">
                    Street:
                </label>
                <input type="text" value="123 Main St" style="width: 100%; padding: 6pt;
                       border: 1pt solid #cccccc;" />
            </div>

            <div style="margin-bottom: 0;">
                <label style="display: block; font-weight: bold; margin-bottom: 4pt;">
                    City, State ZIP:
                </label>
                <input type="text" value="Boston, MA 02101" style="width: 100%; padding: 6pt;
                       border: 1pt solid #cccccc;" />
            </div>
        </fieldset>
    </div>
</div>
```

---

## See Also

- [input](/reference/htmltags/input.html) - Input field element
- [label](/reference/htmltags/label.html) - Label element for form fields
- [form](/reference/htmltags/form.html) - Form container element
- [div](/reference/htmltags/div.html) - Generic block container
- [Data Binding](/reference/binding/) - Dynamic data binding and expressions
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
