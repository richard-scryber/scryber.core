---
layout: default
title: text-decoration-line
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# text-decoration-line : Text Decoration Line Property

The `text-decoration-line` property specifies the type of decoration line to apply to text. This property is part of the text-decoration shorthand and provides granular control over text decoration effects in PDF documents.

## Usage

```css
/* Keyword values */
text-decoration-line: none;
text-decoration-line: underline;
text-decoration-line: overline;
text-decoration-line: line-through;
```

---

## Values

### Decoration Line Keywords

- **none** - No text decoration line (default)
- **underline** - Draws a line beneath the text
- **overline** - Draws a line above the text
- **line-through** - Draws a line through the middle of the text (strikethrough)

### Default Value

- **none** - No decoration line by default

---

## Notes

- This property provides the same functionality as `text-decoration` but with more explicit naming
- In CSS3, `text-decoration-line` is part of the expanded text-decoration model
- The decoration line color typically matches the text color unless overridden
- Multiple decoration lines can be specified in some implementations (e.g., `underline overline`)
- The line is drawn after text rendering and doesn't affect text layout or spacing
- Useful for semantic styling where decoration intent should be explicit
- Functionally equivalent to the `text-decoration` property in Scryber

---

## Data Binding

The `text-decoration-line` property supports data binding for dynamic decoration control based on document state, content types, or styling preferences.

### Example 1: Dynamic Link Styling

```html
<a href="{{model.link.url}}"
   style="color: {{model.link.color}}; text-decoration-line: {{model.link.decorationLine}}">
    {{model.link.text}}
</a>

<!-- Data model example:
{
    "link": {
        "url": "https://example.com",
        "text": "External Link",
        "color": "#2563eb",
        "decorationLine": "underline"  // or "none"
    }
}
-->
```

### Example 2: Product Status Indication

```html
<div>
    <span style="text-decoration-line: {{model.product.decorationLine}}; color: {{model.product.textColor}}">
        {{model.product.name}}
    </span>
</div>

<!-- Data model for discontinued product:
{
    "product": {
        "name": "Legacy Widget v1.0",
        "decorationLine": "line-through",
        "textColor": "#666"
    }
}
-->

<!-- Data model for active product:
{
    "product": {
        "name": "Modern Widget v2.0",
        "decorationLine": "none",
        "textColor": "#000"
    }
}
-->
```

### Example 3: Emphasis Style from Settings

```html
<h2 style="text-decoration-line: {{model.heading.decorationLine}}; font-size: 18pt; color: #1e40af">
    {{model.heading.text}}
</h2>

<!-- Data model example:
{
    "heading": {
        "text": "Section Title",
        "decorationLine": "underline"  // or "overline" or "none"
    }
}
-->
```

---

## Examples

### Example 1: Underlined Heading

```html
<h2 style="text-decoration-line: underline; font-size: 18pt; color: #1e40af">
    Section Title
</h2>
```

### Example 2: Strikethrough for Corrections

```html
<p style="font-size: 12pt">
    The project deadline is
    <span style="text-decoration-line: line-through">March 15</span>
    March 20.
</p>
```

### Example 3: No Decoration on Links

```html
<a href="https://example.com" style="color: #2563eb; text-decoration-line: none">
    Clean link without underline
</a>
```

### Example 4: Overline for Header Emphasis

```html
<div style="text-decoration-line: overline; font-size: 16pt; font-weight: bold; padding-bottom: 5pt">
    CONFIDENTIAL DOCUMENT
</div>
```

### Example 5: Price Comparison

```html
<div style="padding: 15pt; border: 2pt solid #dc2626; background-color: #fef2f2">
    <p style="font-size: 14pt">
        Regular Price:
        <span style="text-decoration-line: line-through; color: #991b1b">
            $149.99
        </span>
    </p>
    <p style="font-size: 20pt; color: #dc2626; font-weight: bold">
        Sale Price: $99.99
    </p>
</div>
```

### Example 6: Table with Deprecated Items

```html
<table style="width: 100%; border-collapse: collapse">
    <tr>
        <th style="text-align: left; padding: 8pt; border-bottom: 2pt solid black">
            Product
        </th>
        <th style="text-align: left; padding: 8pt; border-bottom: 2pt solid black">
            Status
        </th>
    </tr>
    <tr>
        <td style="padding: 6pt; border-bottom: 1pt solid #ddd">
            <span style="text-decoration-line: line-through; color: #666">
                Legacy Widget v1.0
            </span>
        </td>
        <td style="padding: 6pt; border-bottom: 1pt solid #ddd; color: #dc2626">
            Discontinued
        </td>
    </tr>
    <tr>
        <td style="padding: 6pt; border-bottom: 1pt solid #ddd">
            Modern Widget v2.0
        </td>
        <td style="padding: 6pt; border-bottom: 1pt solid #ddd; color: #16a34a">
            Active
        </td>
    </tr>
</table>
```

### Example 7: Document Annotations

```html
<p style="font-size: 11pt; line-height: 1.6">
    The quarterly report shows
    <span style="text-decoration-line: line-through; color: #999">
        significant losses
    </span>
    moderate growth in all sectors.
</p>
```

### Example 8: Navigation with Active State

```html
<div style="padding: 12pt; background-color: #f3f4f6">
    <a href="#section1" style="color: #374151; text-decoration-line: none; margin-right: 15pt">
        Section 1
    </a>
    <a href="#section2" style="color: #1e40af; text-decoration-line: underline; margin-right: 15pt">
        Section 2 (Current)
    </a>
    <a href="#section3" style="color: #374151; text-decoration-line: none">
        Section 3
    </a>
</div>
```

### Example 9: Special Offer Banner

```html
<div style="text-align: center; background-color: #fef3c7; padding: 20pt">
    <h2 style="text-decoration-line: overline underline; font-size: 22pt; color: #92400e">
        SPECIAL PROMOTION
    </h2>
    <p style="font-size: 14pt; margin-top: 10pt">
        <span style="text-decoration-line: line-through">$299</span>
        <span style="font-weight: bold; color: #dc2626">NOW $199!</span>
    </p>
</div>
```

### Example 10: Contract Revisions

```html
<div style="font-size: 10pt; font-family: 'Times New Roman', serif">
    <p>
        <strong style="text-decoration-line: underline">Article 3.2</strong>
    </p>
    <p style="margin-top: 10pt; line-height: 1.5">
        The contractor shall deliver
        <span style="text-decoration-line: line-through; color: #666">
            within 30 business days
        </span>
        within 45 business days from the contract signing date.
    </p>
</div>
```

### Example 11: Menu with Sold Out Items

```html
<div style="padding: 20pt; border: 1pt solid #e5e7eb">
    <h3 style="font-size: 16pt; text-decoration-line: underline; margin-bottom: 15pt">
        Today's Specials
    </h3>
    <p style="font-size: 12pt; margin-bottom: 8pt">
        <span style="text-decoration-line: line-through; color: #9ca3af">
            Grilled Salmon - $24.99
        </span>
        <span style="color: #dc2626; font-size: 10pt"> (Sold Out)</span>
    </p>
    <p style="font-size: 12pt; margin-bottom: 8pt">
        Beef Wellington - $32.99
    </p>
    <p style="font-size: 12pt">
        Vegetarian Pasta - $18.99
    </p>
</div>
```

### Example 12: Academic Citation

```html
<div style="font-size: 11pt">
    <p style="text-decoration-line: underline; font-weight: bold">
        References
    </p>
    <p style="margin-top: 10pt; text-indent: -20pt; padding-left: 20pt">
        Smith, J. (2024). <span style="text-decoration-line: underline">
        Advanced PDF Generation Techniques</span>. Tech Publishers.
    </p>
</div>
```

### Example 13: Change Log

```html
<div style="font-size: 10pt; font-family: monospace; background-color: #f5f5f5; padding: 15pt">
    <p style="color: #16a34a">+ Added new feature for export</p>
    <p style="color: #dc2626; text-decoration-line: line-through">
        - Removed deprecated API endpoint
    </p>
    <p style="color: #f59e0b">~ Updated dependencies</p>
</div>
```

### Example 14: Terms and Conditions Updates

```html
<div style="font-size: 9pt; padding: 20pt; border: 1pt solid #d1d5db">
    <p style="text-decoration-line: underline; font-weight: bold; margin-bottom: 10pt">
        Updated Terms of Service - January 2025
    </p>
    <p style="line-height: 1.5">
        Users must
        <span style="text-decoration-line: line-through; color: #666">
            renew annually
        </span>
        agree to automatic renewal unless cancelled 30 days prior to
        the renewal date.
    </p>
</div>
```

### Example 15: Shopping List with Completed Items

```html
<div style="padding: 20pt">
    <h3 style="text-decoration-line: underline; font-size: 16pt">
        Shopping List
    </h3>
    <ul style="font-size: 12pt; margin-top: 15pt">
        <li style="text-decoration-line: line-through; color: #6b7280">
            Milk (2 gallons)
        </li>
        <li style="text-decoration-line: line-through; color: #6b7280">
            Bread
        </li>
        <li>Eggs (1 dozen)</li>
        <li>Butter</li>
        <li style="text-decoration-line: line-through; color: #6b7280">
            Coffee
        </li>
    </ul>
</div>
```

---

## See Also

- [text-decoration](/reference/cssproperties/text-decoration) - Shorthand text decoration property
- [text-decoration-color](/reference/cssproperties/text-decoration-color) - Decoration line color
- [text-decoration-style](/reference/cssproperties/text-decoration-style) - Decoration line style
- [color](/reference/cssproperties/color) - Text color
- [border-bottom](/reference/cssproperties/border-bottom) - Alternative underline effect
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
