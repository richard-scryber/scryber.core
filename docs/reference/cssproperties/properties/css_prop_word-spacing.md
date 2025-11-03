---
layout: default
title: word-spacing
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# word-spacing : Word Spacing Property

The `word-spacing` property controls the horizontal spacing between words in text. This property adjusts the space added to the default word separation and can be used to improve readability or create specific typographic effects in PDF documents.

## Usage

```css
/* Length values */
word-spacing: 0;
word-spacing: 2pt;
word-spacing: 3px;
word-spacing: 0.2em;

/* Keyword value */
word-spacing: normal;

/* Negative values */
word-spacing: -1pt;
```

---

## Values

### Length Units

- **pt (points)** - Points: `1pt`, `2pt`, `5pt`
- **px (pixels)** - Pixels: `1px`, `2px`, `5px`
- **em** - Relative to font size: `0.2em`, `0.5em`
- **rem** - Relative to root font size: `0.2rem`
- **in, cm, mm** - Absolute units: `0.05in`, `1mm`

### Keyword Values

- **normal** - Default word spacing

### Negative Values

- Negative values are allowed and will decrease spacing between words
- Use cautiously as excessive negative spacing can make words run together

### Default Value

- **normal** - Default spacing between words

---

## Notes

- Word spacing adjusts the space between words, not between individual characters
- The spacing is added to (or subtracted from) the default space character width
- Positive values increase spacing, negative values decrease spacing
- Affects justified text alignment by providing additional space for justification
- Useful for improving readability in narrow columns
- Can be combined with letter-spacing for comprehensive text spacing control
- Relative units (em, rem) scale with font size
- Very large positive values can make text difficult to read
- Does not affect single-word lines or the last word in a line

---

## Data Binding

The `word-spacing` property supports data binding for dynamic control of word spacing based on readability preferences, document types, or accessibility requirements.

### Example 1: Accessibility-Enhanced Spacing

```html
<p style="word-spacing: {{model.accessibility.wordSpacing}}; font-size: 12pt; line-height: 1.6">
    {{model.content}}
</p>

<!-- Data model example:
{
    "accessibility": {
        "wordSpacing": "2pt"  // Enhanced spacing for improved readability
    },
    "content": "This paragraph has increased word spacing for better readability."
}
-->
```

### Example 2: Document Type-Based Spacing

```html
<p style="word-spacing: {{model.paragraph.wordSpacing}}; text-align: {{model.paragraph.alignment}}; font-size: 11pt">
    {{model.paragraph.text}}
</p>

<!-- Data model for justified text:
{
    "paragraph": {
        "text": "Justified paragraphs benefit from slight word spacing adjustments...",
        "wordSpacing": "1pt",
        "alignment": "justify"
    }
}
-->

<!-- Data model for normal text:
{
    "paragraph": {
        "text": "Regular paragraphs use standard word spacing...",
        "wordSpacing": "normal",
        "alignment": "left"
    }
}
-->
```

### Example 3: Emphasis Through Spacing

```html
<h2 style="word-spacing: {{model.heading.wordSpacing}}; letter-spacing: {{model.heading.letterSpacing}}; font-size: 20pt; text-transform: uppercase">
    {{model.heading.text}}
</h2>

<!-- Data model example:
{
    "heading": {
        "text": "IMPORTANT ANNOUNCEMENT",
        "wordSpacing": "8pt",
        "letterSpacing": "2pt"
    }
}
-->
```

---

## Examples

### Example 1: Increased Word Spacing

```html
<p style="word-spacing: 3pt; font-size: 12pt">
    This paragraph has increased spacing between words for improved readability.
</p>
```

### Example 2: Normal Word Spacing

```html
<p style="word-spacing: normal; font-size: 12pt">
    This paragraph uses the default word spacing that is standard for most text.
</p>
```

### Example 3: Tight Word Spacing

```html
<p style="word-spacing: -0.5pt; font-size: 11pt">
    This text has slightly tighter word spacing to fit more content.
</p>
```

### Example 4: Spaced Out Heading

```html
<h2 style="word-spacing: 8pt; letter-spacing: 2pt; font-size: 20pt; text-transform: uppercase">
    IMPORTANT ANNOUNCEMENT
</h2>
```

### Example 5: Justified Text with Word Spacing

```html
<p style="text-align: justify; word-spacing: 2pt; font-size: 11pt; line-height: 1.6">
    This paragraph uses justified alignment with additional word spacing.
    The extra spacing helps distribute text evenly across the line width
    while maintaining good readability throughout the document.
</p>
```

### Example 6: Quote with Emphasis

```html
<div style="text-align: center; padding: 30pt; background-color: #f9fafb">
    <p style="word-spacing: 5pt; font-size: 16pt; font-style: italic; line-height: 1.8; color: #374151">
        "The only way to do great work is to love what you do."
    </p>
    <p style="word-spacing: normal; font-size: 12pt; margin-top: 15pt; color: #6b7280">
        - Steve Jobs
    </p>
</div>
```

### Example 7: Table Caption

```html
<div style="margin-bottom: 10pt">
    <p style="word-spacing: 4pt; font-size: 11pt; text-transform: uppercase; letter-spacing: 1pt; color: #666">
        Table 1: Quarterly Revenue Breakdown
    </p>
</div>
<table style="width: 100%; border-collapse: collapse">
    <tr>
        <th style="padding: 8pt; border: 1pt solid #ccc; text-align: left">Quarter</th>
        <th style="padding: 8pt; border: 1pt solid #ccc; text-align: right">Revenue</th>
    </tr>
    <tr>
        <td style="padding: 6pt; border: 1pt solid #ccc">Q1</td>
        <td style="padding: 6pt; border: 1pt solid #ccc; text-align: right">$125,000</td>
    </tr>
</table>
```

### Example 8: Readable Body Text

```html
<div style="padding: 20pt; max-width: 500pt">
    <p style="word-spacing: 1pt; font-size: 11pt; line-height: 1.7; text-align: justify">
        When creating PDF documents, proper word spacing can significantly
        enhance readability. A slight increase in word spacing, such as
        1-2 points, often makes justified text more pleasant to read while
        maintaining professional appearance.
    </p>
</div>
```

### Example 9: Certificate Text

```html
<div style="text-align: center; border: 3pt double #1e40af; padding: 40pt">
    <h1 style="word-spacing: 6pt; letter-spacing: 4pt; font-size: 32pt; text-transform: uppercase; color: #1e40af">
        Certificate of Excellence
    </h1>
    <p style="word-spacing: 3pt; font-size: 14pt; margin-top: 30pt; line-height: 2">
        This is to certify that
    </p>
    <p style="word-spacing: normal; font-size: 20pt; font-weight: bold; margin-top: 10pt">
        Jane Doe
    </p>
    <p style="word-spacing: 3pt; font-size: 14pt; margin-top: 20pt; line-height: 2">
        has successfully completed the advanced course in
    </p>
    <p style="word-spacing: normal; font-size: 18pt; font-weight: bold; margin-top: 10pt">
        PDF Document Generation
    </p>
</div>
```

### Example 10: Product Description

```html
<div style="padding: 20pt; border: 1pt solid #e5e7eb">
    <h3 style="word-spacing: 2pt; font-size: 16pt; color: #1e293b">
        Premium Widget Pro
    </h3>
    <p style="word-spacing: 1.5pt; font-size: 11pt; line-height: 1.6; margin-top: 10pt; color: #374151">
        Our premium widget offers outstanding performance and reliability.
        Built with high-quality materials and backed by our comprehensive
        warranty, this product delivers exceptional value.
    </p>
    <p style="word-spacing: normal; font-size: 18pt; color: #16a34a; font-weight: bold; margin-top: 15pt">
        $299.99
    </p>
</div>
```

### Example 11: Newsletter Header

```html
<div style="background-color: #1e293b; color: white; padding: 25pt">
    <h1 style="word-spacing: 5pt; letter-spacing: 3pt; font-size: 28pt; text-align: center; text-transform: uppercase">
        Monthly Tech Newsletter
    </h1>
    <p style="word-spacing: 2pt; font-size: 12pt; text-align: center; margin-top: 10pt; color: #cbd5e1">
        Volume 5 Issue 10 October 2025
    </p>
</div>
```

### Example 12: Legal Document

```html
<div style="font-size: 10pt; line-height: 1.6; padding: 30pt">
    <p style="word-spacing: 0.5pt; text-align: justify">
        <strong style="text-decoration: underline">TERMS AND CONDITIONS</strong>
    </p>
    <p style="word-spacing: 0.5pt; text-align: justify; margin-top: 15pt">
        This agreement is entered into as of the date of acceptance by both
        parties and shall remain in full force and effect until terminated
        in accordance with the provisions set forth herein.
    </p>
</div>
```

### Example 13: Report Executive Summary

```html
<div style="padding: 30pt">
    <h2 style="word-spacing: 3pt; font-size: 20pt; color: #1e40af; border-bottom: 3pt solid #1e40af; padding-bottom: 8pt">
        Executive Summary
    </h2>
    <p style="word-spacing: 1pt; font-size: 11pt; line-height: 1.7; text-align: justify; margin-top: 15pt">
        This report presents comprehensive findings from our annual market
        analysis. Key performance indicators show strong growth across all
        business segments, with particular success in digital transformation
        initiatives. Strategic recommendations for the upcoming fiscal year
        focus on sustainable expansion and operational efficiency.
    </p>
</div>
```

### Example 14: Menu Price List

```html
<div style="padding: 20pt; width: 400pt">
    <h3 style="word-spacing: 4pt; letter-spacing: 2pt; font-size: 18pt; text-transform: uppercase; color: #92400e; text-align: center">
        Beverages
    </h3>
    <div style="margin-top: 15pt">
        <div style="word-spacing: 2pt; font-size: 12pt; margin-bottom: 8pt; border-bottom: 1pt dotted #ccc; padding-bottom: 4pt">
            <span style="float: left">Fresh Brewed Coffee</span>
            <span style="float: right">$3.50</span>
        </div>
        <div style="word-spacing: 2pt; font-size: 12pt; margin-bottom: 8pt; border-bottom: 1pt dotted #ccc; padding-bottom: 4pt">
            <span style="float: left">Specialty Latte</span>
            <span style="float: right">$5.00</span>
        </div>
        <div style="word-spacing: 2pt; font-size: 12pt; margin-bottom: 8pt; border-bottom: 1pt dotted #ccc; padding-bottom: 4pt">
            <span style="float: left">Herbal Tea Selection</span>
            <span style="float: right">$2.75</span>
        </div>
    </div>
</div>
```

### Example 15: Academic Title Page

```html
<div style="text-align: center; padding: 80pt 50pt">
    <h1 style="word-spacing: 6pt; letter-spacing: 2pt; font-size: 24pt; font-weight: bold; line-height: 1.5; text-transform: uppercase">
        Advanced Techniques in<br/>
        PDF Document Generation<br/>
        Using Modern Technologies
    </h1>
    <p style="word-spacing: 3pt; font-size: 14pt; margin-top: 50pt">
        A Comprehensive Research Paper
    </p>
    <p style="word-spacing: normal; font-size: 12pt; margin-top: 30pt">
        Submitted by: John Smith<br/>
        Student ID: 12345<br/>
        Date: October 14, 2025
    </p>
    <p style="word-spacing: 2pt; font-size: 11pt; margin-top: 40pt; color: #666">
        Department of Computer Science<br/>
        University of Technology
    </p>
</div>
```

---

## See Also

- [letter-spacing](/reference/cssproperties/letter-spacing) - Space between characters
- [text-align](/reference/cssproperties/text-align) - Text alignment
- [line-height](/reference/cssproperties/line-height) - Line spacing
- [white-space](/reference/cssproperties/white-space) - Whitespace handling
- [font-size](/reference/cssproperties/font-size) - Text size
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
