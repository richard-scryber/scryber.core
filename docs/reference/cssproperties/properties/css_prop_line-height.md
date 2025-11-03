---
layout: default
title: line-height
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# line-height : The Line Height Property

Summary

The `line-height` property sets the vertical spacing between lines of text in PDF documents. It controls the leading (space between baselines of consecutive lines) and significantly affects readability. Line-height can be specified as an absolute unit, a unitless multiplier, or a relative unit (em units).

## Usage

```css
/* Unitless (multiplier of font-size) */
line-height: 1.5;
line-height: 2;

/* Absolute units */
line-height: 20pt;
line-height: 24px;
line-height: 1.5em;

/* Relative to font-size */
line-height: 150%;

/* Within style attribute */
<p style="line-height: 1.5">Text with comfortable line spacing</p>
```

---

## Values

### Unitless Numbers

- **Number** - Multiplier of the element's font-size: `1.5`, `2.0`
- Calculated as: `line-height = font-size × number`
- Example: `font-size: 12pt; line-height: 1.5` results in 18pt line height
- Converted to em units internally for processing

### Absolute Units

- **pt (points)** - Points: `20pt`, `24pt`
- **px (pixels)** - Pixels: `24px`, `30px`
- **in (inches)** - Inches: `0.25in`
- **cm (centimeters)** - Centimeters: `0.5cm`
- **mm (millimeters)** - Millimeters: `5mm`

### Relative Units

- **em** - Relative to element's font-size: `1.5em`, `2em`
- **rem** - Relative to root font-size: `1.5rem`
- **% (percentage)** - Percentage of font-size: `150%`, `200%`

### Expression Support

- **calc()** - Calculate line height: `calc(12pt + 4pt)`
- **var()** - Use CSS variables: `var(--line-spacing)`

---

## Notes

- Default line-height varies but is typically around 1.2 times the font-size
- Unitless values (numbers) are the recommended approach as they scale proportionally
- Unitless numbers are converted to em units internally (e.g., `1.5` becomes `1.5em`)
- Line-height affects the total vertical space occupied by text blocks
- Larger line-height values improve readability but increase document length
- Smaller line-height values create denser text but may reduce readability
- For body text, values between 1.4 and 1.8 are typically most readable
- Line-height is inherited by child elements
- In PDF generation, proper line-height is crucial for professional appearance
- The line-height property sets the `TextLeading` style internally

---

## Data Binding

CSS properties support dynamic values through Scryber's Handlebars-style data binding syntax using `{{expression}}` in inline styles. This enables line-height to be determined dynamically based on model data, readability preferences, or document requirements.

### Binding Syntax

Data binding expressions are enclosed in double curly braces `{{}}` and can reference:
- Model properties: `{{model.propertyName}}`
- Nested data: `{{document.spacing.lineHeight}}`
- Conditional expressions: `{{condition ? valueIfTrue : valueIfFalse}}`

### Data Binding Examples

```html
<!-- Dynamic line height from model -->
<p style="line-height: {{preferences.lineSpacing}}">
    Text with user-defined line spacing
</p>

<!-- Conditional spacing based on content type -->
<div style="line-height: {{contentType == 'legal' ? '2' : '1.5'}}">
    {{documentContent}}
</div>

<!-- Adaptive spacing for readability -->
<body>
    <!-- Line height based on font size -->
    <p style="font-size: {{fontSize}}pt; line-height: {{lineHeightMultiplier}}">
        Content with proportional line spacing
    </p>

    <!-- Different spacing for different sections -->
    <article style="line-height: {{styles.bodyLineHeight}}">
        <h1 style="line-height: {{styles.headingLineHeight}}">
            {{title}}
        </h1>
        <p>{{content}}</p>
    </article>

    <!-- Accessibility-aware line height -->
    <div style="line-height: {{accessibility.useGenerousSpacing ? '1.8' : '1.5'}}">
        {{accessibleContent}}
    </div>

    <!-- Document type-specific spacing -->
    <div style="line-height: {{docType == 'report' ? '1.6' : docType == 'invoice' ? '1.4' : '1.5'}}">
        Spacing optimized for document type
    </div>
</body>
```

**Note:** Bound line-height values can be unitless numbers (recommended), absolute units (e.g., '20pt'), or percentages (e.g., '150%'). Unitless values scale proportionally with font-size changes.

---

## Examples

### Example 1: Unitless Multiplier

```html
<p style="font-size: 12pt; line-height: 1.5">
    Text with 1.5x line height (18pt spacing between baselines)
</p>
```

### Example 2: Double Spacing

```html
<div style="font-size: 11pt; line-height: 2">
    Double-spaced text for better readability and note-taking space
</div>
```

### Example 3: Absolute Point Value

```html
<p style="font-size: 12pt; line-height: 20pt">
    Text with 20pt line height regardless of font size changes
</p>
```

### Example 4: Tight Line Height

```html
<div style="font-size: 14pt; line-height: 1.2">
    Compact text with minimal line spacing
</div>
```

### Example 5: Generous Line Height

```html
<article style="font-size: 11pt; line-height: 1.8">
    Article text with generous line spacing for improved readability
    in longer documents and reports.
</article>
```

### Example 6: Em-Based Line Height

```html
<p style="font-size: 12pt; line-height: 1.5em">
    Text with line height specified in em units (18pt)
</p>
```

### Example 7: Percentage Line Height

```html
<div style="font-size: 10pt; line-height: 150%">
    Text with percentage-based line height (15pt)
</div>
```

### Example 8: Invoice Line Items

```html
<table style="font-size: 10pt; line-height: 1.4">
    <tr>
        <td>Product Description</td>
        <td>Quantity</td>
        <td>Price</td>
    </tr>
    <tr>
        <td>Premium Widget Set with extended warranty</td>
        <td>5</td>
        <td>$125.00</td>
    </tr>
</table>
```

### Example 9: Report Paragraph Spacing

```html
<div style="font-size: 11pt; line-height: 1.6">
    <p>
        This report presents findings from our quarterly analysis.
        Comfortable line spacing ensures the content is easy to read
        and understand.
    </p>
    <p>
        Key metrics show improvement across all departments with
        particularly strong performance in sales and customer service.
    </p>
</div>
```

### Example 10: Heading with Custom Line Height

```html
<h1 style="font-size: 24pt; line-height: 1.3">
    Annual Report
    2024
</h1>
```

### Example 11: Compact Table Data

```html
<table style="font-size: 9pt; line-height: 1.1">
    <tr><td>Q1</td><td>$125,000</td><td>+15%</td></tr>
    <tr><td>Q2</td><td>$142,000</td><td>+8%</td></tr>
    <tr><td>Q3</td><td>$138,000</td><td>-2%</td></tr>
    <tr><td>Q4</td><td>$155,000</td><td>+12%</td></tr>
</table>
```

### Example 12: Certificate Text

```html
<div style="text-align: center">
    <div style="font-size: 28pt; line-height: 1.2">
        Certificate of Completion
    </div>
    <div style="font-size: 14pt; line-height: 2">
        This certifies that
    </div>
    <div style="font-size: 20pt; line-height: 1.5">
        Jane Smith
    </div>
</div>
```

### Example 13: Footer with Tight Spacing

```html
<footer style="font-size: 8pt; line-height: 1.3">
    <div>Company Name Inc. | 123 Business Street | City, State 12345</div>
    <div>Phone: (555) 123-4567 | Email: info@company.com</div>
    <div>© 2024 All Rights Reserved</div>
</footer>
```

### Example 14: Quote Block with Generous Spacing

```html
<blockquote style="font-size: 12pt; line-height: 1.8; font-style: italic">
    "Success is not final, failure is not fatal: it is the courage
    to continue that counts."
</blockquote>
```

### Example 15: Professional Document with Variable Line Heights

```html
<article style="font-family: Georgia, serif">
    <header>
        <h1 style="font-size: 22pt; line-height: 1.2">
            Executive Summary
        </h1>
        <div style="font-size: 10pt; line-height: 1.4; font-style: italic">
            Quarterly Business Review - Q4 2024
        </div>
    </header>

    <section style="font-size: 11pt; line-height: 1.6">
        <h2 style="font-size: 16pt; line-height: 1.3">
            Financial Performance
        </h2>
        <p>
            This quarter has demonstrated exceptional growth across all
            business units. Total revenue increased by 18% compared to
            the previous quarter, exceeding our projected targets.
        </p>
        <p>
            Key performance indicators show positive trends in customer
            acquisition, retention rates, and average transaction values.
            These metrics suggest sustainable growth patterns for the
            coming fiscal year.
        </p>
    </section>

    <section style="font-size: 10pt; line-height: 1.5">
        <h3 style="font-size: 13pt; line-height: 1.3">
            Detailed Metrics
        </h3>
        <table style="line-height: 1.4">
            <tr style="font-weight: bold">
                <td>Metric</td>
                <td>Value</td>
                <td>Change</td>
            </tr>
            <tr>
                <td>Revenue</td>
                <td>$2,450,000</td>
                <td>+18%</td>
            </tr>
            <tr>
                <td>New Customers</td>
                <td>1,247</td>
                <td>+23%</td>
            </tr>
        </table>
    </section>

    <footer style="font-size: 9pt; line-height: 1.3; border-top: 1pt solid #ccc; padding-top: 10pt">
        <div>Confidential - For Internal Use Only</div>
        <div>Prepared by Finance Department | January 2024</div>
    </footer>
</article>
```

---

## See Also

- [font](/reference/cssproperties/font) - Shorthand font property (includes line-height)
- [font-size](/reference/cssproperties/font-size) - Font size specification
- [font-family](/reference/cssproperties/font-family) - Font family specification
- [text-leading](/reference/cssproperties/text-leading) - Alternative name for line height
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
