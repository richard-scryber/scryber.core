---
layout: default
title: letter-spacing
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# letter-spacing : Letter Spacing Property

The `letter-spacing` property controls the horizontal spacing between characters (letters) in text. This property is useful for adjusting text appearance, creating emphasis, and improving readability in PDF documents.

## Usage

```css
/* Length values */
letter-spacing: 0;
letter-spacing: 0.5pt;
letter-spacing: 1px;
letter-spacing: 0.05em;

/* Keyword value */
letter-spacing: normal;

/* Negative values */
letter-spacing: -0.5pt;
```

---

## Values

### Length Units

- **pt (points)** - Points: `1pt`, `2pt`, `0.5pt`
- **px (pixels)** - Pixels: `1px`, `2px`, `0.5px`
- **em** - Relative to font size: `0.1em`, `0.05em`
- **rem** - Relative to root font size: `0.1rem`
- **in, cm, mm** - Absolute units: `0.01in`, `0.1mm`

### Keyword Values

- **normal** - Default spacing (equivalent to 0)

### Negative Values

- Negative values are allowed and will decrease spacing between characters
- Use cautiously as excessive negative spacing can make text unreadable

### Default Value

- **normal** - No additional spacing (0)

---

## Notes

- Letter spacing adds or removes space between each character
- Positive values increase spacing, negative values decrease spacing
- The spacing is applied in addition to the default character spacing
- Very large positive values can make text difficult to read
- Small positive values (0.5pt-2pt) can improve readability of uppercase text
- Commonly used for headings, logos, and emphasis
- Relative units (em, rem) scale with font size
- Affects all characters including spaces
- Does not affect word boundaries (use word-spacing for that)

---

## Data Binding

The `letter-spacing` property can be dynamically controlled through data binding, allowing character spacing to be adjusted based on design preferences, accessibility requirements, or document types.

### Example 1: User Accessibility Preferences

```html
<p style="letter-spacing: {{model.accessibility.letterSpacing}}; font-size: 12pt">
    {{model.content}}
</p>

<!-- Data model example:
{
    "accessibility": {
        "letterSpacing": "1pt"  // User preference for improved readability
    },
    "content": "This text spacing improves readability for some users."
}
-->
```

### Example 2: Content Type-Based Spacing

```html
<h1 style="letter-spacing: {{model.heading.spacing}}; font-size: {{model.heading.size}}; text-transform: uppercase">
    {{model.heading.text}}
</h1>

<!-- Data model for logo/branding:
{
    "heading": {
        "text": "ANNUAL REPORT",
        "spacing": "3pt",
        "size": "24pt"
    }
}
-->

<!-- Data model for regular heading:
{
    "heading": {
        "text": "Section Overview",
        "spacing": "normal",
        "size": "18pt"
    }
}
-->
```

### Example 3: Dynamic Emphasis Levels

```html
<div style="letter-spacing: {{model.emphasis.spacing}}; font-size: 14pt; text-transform: uppercase">
    {{model.emphasis.text}}
</div>

<!-- Data model example:
{
    "emphasis": {
        "text": "IMPORTANT NOTICE",
        "spacing": "5pt"  // Higher spacing for maximum emphasis
    }
}
-->
```

---

## Examples

### Example 1: Spaced Out Heading

```html
<h1 style="letter-spacing: 3pt; font-size: 24pt; text-transform: uppercase">
    ANNUAL REPORT
</h1>
```

### Example 2: Normal Letter Spacing

```html
<p style="letter-spacing: normal; font-size: 12pt">
    This paragraph uses normal letter spacing for standard readability.
</p>
```

### Example 3: Tight Letter Spacing

```html
<p style="letter-spacing: -0.5pt; font-size: 11pt">
    This text has slightly tighter letter spacing to fit more content.
</p>
```

### Example 4: Logo Style Text

```html
<div style="letter-spacing: 5pt; font-size: 28pt; font-weight: bold; text-align: center">
    BRAND NAME
</div>
```

### Example 5: Subtitle with Moderate Spacing

```html
<h2 style="letter-spacing: 1.5pt; font-size: 16pt; color: #666; text-transform: uppercase">
    Chapter One: Introduction
</h2>
```

### Example 6: Certificate Header

```html
<div style="text-align: center; padding: 30pt">
    <h1 style="letter-spacing: 8pt; font-size: 32pt; font-weight: bold; color: #1e40af">
        CERTIFICATE
    </h1>
    <p style="letter-spacing: 3pt; font-size: 12pt; color: #666; text-transform: uppercase">
        Of Achievement
    </p>
</div>
```

### Example 7: Table Header with Spacing

```html
<table style="width: 100%; border-collapse: collapse">
    <thead>
        <tr style="background-color: #1e293b">
            <th style="letter-spacing: 1pt; color: white; padding: 10pt; text-transform: uppercase; font-size: 10pt">
                Product Name
            </th>
            <th style="letter-spacing: 1pt; color: white; padding: 10pt; text-transform: uppercase; font-size: 10pt">
                Quantity
            </th>
            <th style="letter-spacing: 1pt; color: white; padding: 10pt; text-transform: uppercase; font-size: 10pt">
                Price
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="padding: 8pt; border-bottom: 1pt solid #e5e7eb">Widget A</td>
            <td style="padding: 8pt; border-bottom: 1pt solid #e5e7eb; text-align: center">10</td>
            <td style="padding: 8pt; border-bottom: 1pt solid #e5e7eb; text-align: right">$29.99</td>
        </tr>
    </tbody>
</table>
```

### Example 8: Magazine Style Headline

```html
<div style="border-left: 4pt solid #dc2626; padding-left: 15pt; margin: 20pt 0">
    <h2 style="letter-spacing: 2pt; font-size: 22pt; text-transform: uppercase; color: #1e293b">
        Breaking News
    </h2>
    <p style="letter-spacing: 0.5pt; font-size: 14pt; color: #666; margin-top: 5pt">
        Major developments in the technology sector
    </p>
</div>
```

### Example 9: Business Card Layout

```html
<div style="border: 2pt solid #1e293b; padding: 30pt; width: 350pt">
    <h2 style="letter-spacing: 6pt; font-size: 20pt; text-transform: uppercase; color: #1e40af">
        John Smith
    </h2>
    <p style="letter-spacing: 2pt; font-size: 11pt; text-transform: uppercase; color: #666">
        Senior Developer
    </p>
    <p style="letter-spacing: normal; font-size: 10pt; margin-top: 15pt">
        Email: john.smith@example.com<br/>
        Phone: (555) 123-4567
    </p>
</div>
```

### Example 10: Quote with Emphasis

```html
<div style="text-align: center; padding: 40pt; background-color: #f9fafb">
    <p style="letter-spacing: 1pt; font-size: 18pt; font-style: italic; line-height: 1.6; color: #374151">
        "Design is not just what it looks like and feels like.<br/>
        Design is how it works."
    </p>
    <p style="letter-spacing: 2pt; font-size: 11pt; text-transform: uppercase; margin-top: 15pt; color: #6b7280">
        — Steve Jobs
    </p>
</div>
```

### Example 11: Price Tag with Spaced Numbers

```html
<div style="text-align: center; padding: 20pt; background-color: #fef2f2; border: 3pt solid #dc2626">
    <p style="letter-spacing: 2pt; font-size: 12pt; text-transform: uppercase; color: #991b1b">
        Limited Offer
    </p>
    <p style="letter-spacing: 3pt; font-size: 48pt; font-weight: bold; color: #dc2626; margin: 15pt 0">
        $99
    </p>
    <p style="letter-spacing: 1pt; font-size: 10pt; text-transform: uppercase">
        Regular Price: $149
    </p>
</div>
```

### Example 12: Document Section Divider

```html
<div style="text-align: center; padding: 20pt 0; border-top: 2pt solid #e5e7eb; border-bottom: 2pt solid #e5e7eb; margin: 30pt 0">
    <p style="letter-spacing: 5pt; font-size: 14pt; text-transform: uppercase; color: #374151">
        Section II
    </p>
</div>
```

### Example 13: Invoice Header

```html
<div style="padding: 20pt">
    <h1 style="letter-spacing: 4pt; font-size: 28pt; text-transform: uppercase; color: #1e293b">
        Invoice
    </h1>
    <div style="letter-spacing: 1pt; font-size: 11pt; color: #666; margin-top: 10pt">
        <p>NUMBER: INV-2024-001</p>
        <p>DATE: October 14, 2025</p>
    </div>
</div>
```

### Example 14: Menu Category Headers

```html
<div style="padding: 20pt">
    <h3 style="letter-spacing: 3pt; font-size: 16pt; text-transform: uppercase; color: #92400e; border-bottom: 2pt solid #92400e; padding-bottom: 5pt">
        Appetizers
    </h3>
    <p style="letter-spacing: normal; font-size: 11pt; margin-top: 10pt">
        Spring Rolls - $6.99<br/>
        Bruschetta - $7.99
    </p>

    <h3 style="letter-spacing: 3pt; font-size: 16pt; text-transform: uppercase; color: #92400e; border-bottom: 2pt solid #92400e; padding-bottom: 5pt; margin-top: 25pt">
        Main Courses
    </h3>
    <p style="letter-spacing: normal; font-size: 11pt; margin-top: 10pt">
        Grilled Salmon - $18.99<br/>
        Ribeye Steak - $24.99
    </p>
</div>
```

### Example 15: Report Cover Page

```html
<div style="text-align: center; padding: 100pt 50pt">
    <h1 style="letter-spacing: 8pt; font-size: 36pt; font-weight: bold; text-transform: uppercase; color: #1e40af; line-height: 1.4">
        Quarterly<br/>Financial<br/>Report
    </h1>
    <div style="margin-top: 50pt">
        <p style="letter-spacing: 3pt; font-size: 18pt; text-transform: uppercase; color: #64748b">
            Q4 2024
        </p>
        <p style="letter-spacing: 1pt; font-size: 12pt; color: #94a3b8; margin-top: 20pt">
            PREPARED BY FINANCE DEPARTMENT
        </p>
    </div>
</div>
```

---

## See Also

- [word-spacing](/reference/cssproperties/word-spacing) - Space between words
- [text-align](/reference/cssproperties/text-align) - Text alignment
- [font-size](/reference/cssproperties/font-size) - Text size
- [text-transform](/reference/cssproperties/text-transform) - Text case transformation
- [line-height](/reference/cssproperties/line-height) - Line spacing
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
