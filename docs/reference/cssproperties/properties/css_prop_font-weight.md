---
layout: default
title: font-weight
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# font-weight : The Font Weight Property

Summary

The `font-weight` property specifies the boldness or thickness of text characters. It accepts both numeric values (100-900) and keyword values, allowing precise control over text weight for creating visual hierarchy and emphasis in PDF documents.

## Usage

```css
/* Keyword values */
font-weight: normal;
font-weight: bold;
font-weight: lighter;
font-weight: bolder;

/* Numeric values */
font-weight: 100;
font-weight: 400;
font-weight: 700;
font-weight: 900;

/* Within style attribute */
<h1 style="font-weight: bold">Bold Heading</h1>
```

---

## Values

### Numeric Values

- **100** - Thin (Hairline)
- **200** - Extra Light (Ultra Light)
- **300** - Light
- **400** - Normal (Regular) - default weight
- **500** - Medium
- **600** - Semi Bold (Demi Bold)
- **700** - Bold
- **800** - Extra Bold (Ultra Bold)
- **900** - Black (Heavy)

### Keyword Values

- **thin** - 100 weight
- **extra-light** - 200 weight
- **lighter** - 300 weight
- **light** - 300 weight (alternate)
- **normal** - 400 weight (default)
- **regular** - 400 weight (alternate)
- **medium** - 500 weight
- **semi-bold** - 600 weight
- **bold** - 700 weight
- **extra-bold** - 800 weight
- **bolder** - 800 weight (alternate)
- **black** - 900 weight

### Expression Support

- **calc()** - Expression-based font weight
- **var()** - CSS variables: `var(--heading-weight)`

---

## Notes

- Normal text uses a weight of 400 (normal/regular)
- Bold text typically uses a weight of 700
- Not all fonts support all weight values; the renderer will use the closest available weight
- Numeric values must be multiples of 100 between 100 and 900
- Font-weight is inherited from parent elements
- Weight affects text rendering thickness and visual prominence
- Some fonts may only have two weights (normal and bold), in which case intermediate values will map to one of these
- Heavier weights are excellent for headings, emphasis, and creating visual hierarchy in PDFs

---

## Data Binding

CSS properties support dynamic values through Scryber's Handlebars-style data binding syntax using `{{expression}}` in inline styles. This enables font-weight to be determined dynamically based on model data, importance levels, or conditional logic.

### Binding Syntax

Data binding expressions are enclosed in double curly braces `{{}}` and can reference:
- Model properties: `{{model.propertyName}}`
- Nested data: `{{item.formatting.weight}}`
- Conditional expressions: `{{condition ? valueIfTrue : valueIfFalse}}`

### Data Binding Examples

```html
<!-- Dynamic weight from model data -->
<div style="font-weight: {{heading.weight}}">
    {{heading.text}}
</div>

<!-- Conditional bold for important items -->
<div style="font-weight: {{item.isImportant ? 'bold' : 'normal'}}">
    {{item.description}}
</div>

<!-- Emphasis based on status or priority -->
<body>
    <!-- Weight based on priority level -->
    <p style="font-weight: {{priority == 'high' ? '700' : priority == 'medium' ? '600' : '400'}}">
        {{taskDescription}}
    </p>

    <!-- Numeric weight from data -->
    <h1 style="font-weight: {{styles.headingWeight}}">
        {{pageTitle}}
    </h1>

    <!-- Conditional emphasis in tables -->
    <table>
        <tr style="font-weight: {{row.isTotal ? 'bold' : 'normal'}}">
            <td>{{row.label}}</td>
            <td>{{row.value}}</td>
        </tr>
    </table>

    <!-- Dynamic weight for alerts -->
    <div style="font-weight: {{alertLevel >= 3 ? 'bold' : 'normal'}}; color: {{alertColor}}">
        {{alertMessage}}
    </div>
</body>
```

**Note:** Bound font-weight values can be keywords ('normal', 'bold') or numeric values (100-900). Ensure bound data provides valid font-weight values to maintain consistent text rendering.

---

## Examples

### Example 1: Normal Weight

```html
<p style="font-weight: normal">
    This is regular text with normal weight (400)
</p>
```

### Example 2: Bold Weight

```html
<p style="font-weight: bold">
    This is bold text with weight 700
</p>
```

### Example 3: Numeric Weight

```html
<h1 style="font-weight: 700">
    Heading with numeric bold weight
</h1>
```

### Example 4: Light Weight

```html
<div style="font-weight: 300">
    Light-weight text for subtle emphasis
</div>
```

### Example 5: Black Weight

```html
<div style="font-weight: 900">
    Extra heavy text for maximum impact
</div>
```

### Example 6: Semi-Bold for Subheadings

```html
<h2 style="font-weight: 600">
    Semi-bold subheading text
</h2>
```

### Example 7: Invoice Header

```html
<div style="font-size: 24pt; font-weight: bold">
    INVOICE
</div>
<div style="font-size: 16pt; font-weight: 600">
    #INV-2024-001
</div>
<div style="font-size: 12pt; font-weight: normal">
    Date: January 1, 2024
</div>
```

### Example 8: Emphasis in Paragraph

```html
<p style="font-weight: normal">
    Please note: <span style="font-weight: bold">Payment is due within 30 days</span>
    of the invoice date
</p>
```

### Example 9: Table Headers

```html
<table>
    <thead>
        <tr style="font-weight: bold">
            <th>Product</th>
            <th>Quantity</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody style="font-weight: normal">
        <tr>
            <td>Widget</td>
            <td>10</td>
            <td>$100.00</td>
        </tr>
    </tbody>
</table>
```

### Example 10: Weight Hierarchy

```html
<article>
    <h1 style="font-weight: 900; font-size: 24pt">Main Title</h1>
    <h2 style="font-weight: 700; font-size: 18pt">Section Heading</h2>
    <h3 style="font-weight: 600; font-size: 14pt">Subsection</h3>
    <p style="font-weight: 400; font-size: 11pt">Body text content</p>
</article>
```

### Example 11: Light Weight Caption

```html
<figure>
    <img src="chart.png" />
    <figcaption style="font-weight: 300; font-style: italic">
        Figure 1: Revenue Growth
    </figcaption>
</figure>
```

### Example 12: Report Cover Page

```html
<div style="text-align: center">
    <div style="font-size: 32pt; font-weight: black">
        ANNUAL REPORT
    </div>
    <div style="font-size: 24pt; font-weight: 600">
        2024
    </div>
    <div style="font-size: 14pt; font-weight: normal">
        Financial Summary and Analysis
    </div>
</div>
```

### Example 13: Form Labels and Values

```html
<div>
    <span style="font-weight: semi-bold">Customer Name:</span>
    <span style="font-weight: normal">John Smith</span>
</div>
<div>
    <span style="font-weight: semi-bold">Account Number:</span>
    <span style="font-weight: normal">ACC-12345</span>
</div>
```

### Example 14: Alert Box

```html
<div style="border: 2pt solid red; padding: 10pt">
    <div style="font-weight: bold; font-size: 14pt">
        IMPORTANT NOTICE
    </div>
    <div style="font-weight: normal; font-size: 11pt">
        Your account requires immediate attention. Please contact
        customer service at your earliest convenience.
    </div>
</div>
```

### Example 15: Professional Document with Weight Variations

```html
<article style="font-family: Arial, sans-serif">
    <header>
        <h1 style="font-size: 20pt; font-weight: 900">
            Executive Summary
        </h1>
        <div style="font-size: 11pt; font-weight: 300; font-style: italic">
            Prepared by Finance Department - Q4 2024
        </div>
    </header>

    <section>
        <h2 style="font-size: 16pt; font-weight: 700">
            Financial Overview
        </h2>
        <p style="font-size: 11pt; font-weight: normal">
            This report provides a comprehensive analysis of our financial
            performance. <span style="font-weight: 600">Key findings</span>
            indicate strong growth across all business units.
        </p>

        <div style="background: #f0f0f0; padding: 10pt; margin: 10pt 0">
            <div style="font-weight: bold">Total Revenue</div>
            <div style="font-size: 18pt; font-weight: 900">$2,450,000</div>
        </div>

        <p style="font-size: 9pt; font-weight: 300">
            Note: All figures are in USD and have been audited by
            independent accountants.
        </p>
    </section>

    <footer style="border-top: 1pt solid #000; padding-top: 10pt">
        <div style="font-size: 10pt; font-weight: 600">
            Confidential Document
        </div>
        <div style="font-size: 9pt; font-weight: normal">
            Page 1 of 15
        </div>
    </footer>
</article>
```

---

## See Also

- [font](/reference/cssproperties/font) - Shorthand font property
- [font-family](/reference/cssproperties/font-family) - Font family specification
- [font-style](/reference/cssproperties/font-style) - Italic and oblique styles
- [font-size](/reference/cssproperties/font-size) - Font size specification
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
