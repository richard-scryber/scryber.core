---
layout: default
title: text-align
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# text-align : Text Alignment Property

The `text-align` property specifies the horizontal alignment of text and inline content within block-level elements. This is essential for controlling the layout and visual presentation of text in PDF documents.

## Usage

```css
/* Keyword values */
text-align: left;
text-align: right;
text-align: center;
text-align: justify;
```

---

## Values

### Alignment Keywords

- **left** - Aligns text to the left edge of the containing block (default for LTR text)
- **right** - Aligns text to the right edge of the containing block
- **center** - Centers text horizontally within the containing block
- **justify** - Stretches lines so that each line has equal width, with text aligned to both left and right margins (except the last line)

### Default Value

- **left** - Default alignment for left-to-right text

---

## Notes

- Text alignment affects all inline content including text, images, and inline elements
- Justified text distributes space between words to create flush left and right edges
- The last line of justified text is typically left-aligned
- Center alignment is commonly used for headings, titles, and emphasized content
- Right alignment is useful for numeric columns, dates, and RTL text
- Text alignment is inherited by child elements
- Block-level elements like `<div>` and `<p>` are affected, but the property doesn't apply to the blocks themselves

---

## Data Binding

The `text-align` property can be dynamically set using data binding expressions, enabling alignment to be controlled by user preferences, content types, or document settings.

### Example 1: User Preference-Based Alignment

```html
<div style="text-align: {{model.userPreferences.textAlignment}}; font-size: 12pt">
    This paragraph's alignment is determined by user preferences.
    Users can choose 'left', 'center', 'right', or 'justify'.
</div>
```

### Example 2: Content Type Conditional Alignment

```html
<h1 style="text-align: {{model.document.titleAlignment}}; font-size: 24pt">
    {{model.document.title}}
</h1>

<p style="text-align: {{model.document.bodyAlignment}}; font-size: 11pt">
    {{model.document.content}}
</p>

<!-- Data model example:
{
    "document": {
        "title": "Annual Report 2024",
        "titleAlignment": "center",
        "content": "Report content goes here...",
        "bodyAlignment": "justify"
    }
}
-->
```

### Example 3: Multilingual Document with RTL Support

```html
<div style="text-align: {{model.language.alignment}}; direction: {{model.language.direction}}; font-size: 12pt">
    {{model.content}}
</div>

<!-- Data model example:
{
    "language": {
        "code": "ar",
        "direction": "rtl",
        "alignment": "right"
    },
    "content": "محتوى النص هنا"
}
-->
```

---

## Examples

### Example 1: Centered Heading

```html
<h1 style="text-align: center; font-size: 24pt">
    Annual Report 2024
</h1>
```

### Example 2: Left-Aligned Body Text

```html
<p style="text-align: left; font-size: 12pt">
    This paragraph is aligned to the left edge of its container,
    which is the default behavior for most text content.
</p>
```

### Example 3: Right-Aligned Date

```html
<div style="text-align: right; font-size: 10pt; color: #666">
    Date: October 14, 2025
</div>
```

### Example 4: Justified Paragraph

```html
<p style="text-align: justify; font-size: 11pt; line-height: 1.5">
    This paragraph uses justified alignment, which stretches the text
    to fill the entire width of the container. Words are spaced evenly
    to create clean left and right edges, providing a professional
    appearance commonly used in books and formal documents.
</p>
```

### Example 5: Centered Title Page

```html
<div style="text-align: center; margin-top: 100pt">
    <h1 style="font-size: 32pt; font-weight: bold">
        QUARTERLY FINANCIAL REPORT
    </h1>
    <h2 style="font-size: 18pt; margin-top: 20pt">
        Q4 2024
    </h2>
    <p style="font-size: 12pt; margin-top: 40pt">
        Prepared by Finance Department
    </p>
</div>
```

### Example 6: Table Cell Alignment

```html
<table style="width: 100%; border-collapse: collapse">
    <thead>
        <tr>
            <th style="text-align: left; padding: 8pt; border-bottom: 1pt solid black">
                Product
            </th>
            <th style="text-align: right; padding: 8pt; border-bottom: 1pt solid black">
                Price
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td style="text-align: left; padding: 6pt">Widget A</td>
            <td style="text-align: right; padding: 6pt">$29.99</td>
        </tr>
        <tr>
            <td style="text-align: left; padding: 6pt">Widget B</td>
            <td style="text-align: right; padding: 6pt">$39.99</td>
        </tr>
    </tbody>
</table>
```

### Example 7: Invoice Header

```html
<div style="text-align: right; font-size: 10pt">
    <div>Invoice #: INV-2024-1234</div>
    <div>Date: October 14, 2025</div>
    <div>Due Date: November 14, 2025</div>
</div>
```

### Example 8: Mixed Alignment Document

```html
<div style="padding: 20pt">
    <h1 style="text-align: center; font-size: 20pt">
        Document Title
    </h1>
    <p style="text-align: justify; font-size: 11pt">
        The body content uses justified alignment for a professional
        appearance. This creates even margins on both sides of the text.
    </p>
    <p style="text-align: right; font-size: 9pt; font-style: italic">
        - Author Name
    </p>
</div>
```

### Example 9: Form Layout

```html
<div style="width: 400pt">
    <div style="margin-bottom: 10pt">
        <label style="display: inline-block; width: 100pt; text-align: right">
            Name:
        </label>
        <span style="margin-left: 10pt">John Smith</span>
    </div>
    <div style="margin-bottom: 10pt">
        <label style="display: inline-block; width: 100pt; text-align: right">
            Email:
        </label>
        <span style="margin-left: 10pt">john@example.com</span>
    </div>
</div>
```

### Example 10: Price List with Right-Aligned Prices

```html
<div style="width: 300pt">
    <div style="border-bottom: 1pt solid #ccc; padding: 6pt">
        <span style="float: left">Coffee</span>
        <span style="float: right; text-align: right">$3.50</span>
    </div>
    <div style="border-bottom: 1pt solid #ccc; padding: 6pt">
        <span style="float: left">Tea</span>
        <span style="float: right; text-align: right">$2.50</span>
    </div>
    <div style="padding: 6pt; font-weight: bold">
        <span style="float: left">Total</span>
        <span style="float: right; text-align: right">$6.00</span>
    </div>
</div>
```

### Example 11: Centered Quote Block

```html
<div style="text-align: center; padding: 30pt; font-style: italic">
    <p style="font-size: 14pt; line-height: 1.6">
        "Success is not final, failure is not fatal:<br/>
        it is the courage to continue that counts."
    </p>
    <p style="font-size: 11pt; margin-top: 10pt">
        - Winston Churchill
    </p>
</div>
```

### Example 12: Newsletter Header

```html
<div style="background-color: #f0f0f0; padding: 20pt">
    <h1 style="text-align: center; font-size: 28pt; color: #333">
        Monthly Newsletter
    </h1>
    <p style="text-align: center; font-size: 12pt; color: #666">
        Volume 12, Issue 10 - October 2025
    </p>
</div>
```

### Example 13: Two-Column Layout with Different Alignments

```html
<table style="width: 100%">
    <tr>
        <td style="width: 50%; vertical-align: top; padding-right: 10pt">
            <h3 style="text-align: left">Left Column</h3>
            <p style="text-align: left; font-size: 10pt">
                Content aligned to the left for easy reading.
            </p>
        </td>
        <td style="width: 50%; vertical-align: top; padding-left: 10pt">
            <h3 style="text-align: right">Right Column</h3>
            <p style="text-align: right; font-size: 10pt">
                Content aligned to the right for emphasis.
            </p>
        </td>
    </tr>
</table>
```

### Example 14: Certificate Design

```html
<div style="border: 3pt double #333; padding: 40pt; text-align: center">
    <h1 style="font-size: 32pt; font-weight: bold; color: #1a5490">
        CERTIFICATE OF ACHIEVEMENT
    </h1>
    <p style="font-size: 14pt; margin-top: 30pt">
        This certifies that
    </p>
    <p style="font-size: 20pt; font-weight: bold; margin-top: 10pt">
        John Doe
    </p>
    <p style="font-size: 14pt; margin-top: 20pt">
        has successfully completed
    </p>
    <p style="font-size: 16pt; font-weight: bold; margin-top: 10pt">
        Advanced PDF Generation Course
    </p>
    <p style="font-size: 11pt; margin-top: 30pt">
        October 14, 2025
    </p>
</div>
```

### Example 15: Report with Justified Body and Centered Headings

```html
<div style="padding: 30pt">
    <h1 style="text-align: center; font-size: 24pt; margin-bottom: 20pt">
        Executive Summary
    </h1>
    <p style="text-align: justify; font-size: 11pt; line-height: 1.6; margin-bottom: 12pt">
        This report presents a comprehensive analysis of market trends
        and performance metrics for the fiscal year 2024. The data shows
        significant growth across all major product categories, with
        particularly strong performance in the digital services sector.
    </p>
    <h2 style="text-align: left; font-size: 16pt; margin-top: 20pt; margin-bottom: 10pt">
        Key Findings
    </h2>
    <p style="text-align: justify; font-size: 11pt; line-height: 1.6">
        Revenue increased by 23% year-over-year, while operating costs
        remained stable. Customer satisfaction scores improved across
        all surveyed demographics, indicating successful implementation
        of our quality improvement initiatives.
    </p>
</div>
```

---

## See Also

- [vertical-align](/reference/cssproperties/vertical-align) - Vertical alignment of inline elements
- [text-indent](/reference/cssproperties/text-indent) - First-line indentation
- [direction](/reference/cssproperties/direction) - Text directionality
- [word-spacing](/reference/cssproperties/word-spacing) - Space between words
- [letter-spacing](/reference/cssproperties/letter-spacing) - Space between characters
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
