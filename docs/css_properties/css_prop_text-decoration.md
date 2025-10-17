---
layout: default
title: text-decoration
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# text-decoration : Text Decoration Property

The `text-decoration` property specifies decorative lines that appear on text. This is commonly used to underline links, strike through deleted text, or add emphasis to content in PDF documents.

## Usage

```css
/* Keyword values */
text-decoration: none;
text-decoration: underline;
text-decoration: overline;
text-decoration: line-through;
```

---

## Values

### Decoration Keywords

- **none** - No text decoration (default)
- **underline** - Draws a line beneath the text
- **overline** - Draws a line above the text
- **line-through** - Draws a line through the middle of the text (strikethrough)

### Default Value

- **none** - No decoration by default (except links which often default to underline)

---

## Notes

- Text decoration is drawn after the text is rendered
- Multiple decorations can be combined in some CSS implementations, but Scryber typically uses single values
- Underline is commonly used for hyperlinks
- Line-through is useful for showing deleted content, old prices, or corrections
- Overline is less commonly used but can be effective for special emphasis
- The decoration color typically matches the text color
- Text decoration does not affect the spacing or layout of text
- Decoration lines span the entire length of the text element

---

## Data Binding

The `text-decoration` property can be dynamically controlled using data binding, enabling decorations to be applied conditionally based on content state, link types, or document requirements.

### Example 1: Conditional Link Underline

```html
<a href="{{model.link.url}}"
   style="color: {{model.link.color}}; text-decoration: {{model.link.decoration}}">
    {{model.link.text}}
</a>

<!-- Data model example:
{
    "link": {
        "url": "https://example.com",
        "text": "Visit our website",
        "color": "blue",
        "decoration": "underline"  // or "none" for clean links
    }
}
-->
```

### Example 2: Status-Based Text Decoration

```html
<div>
    <p style="text-decoration: {{model.item.decoration}}; color: {{model.item.color}}">
        {{model.item.name}} - {{model.item.price}}
    </p>
</div>

<!-- Data model for deleted item:
{
    "item": {
        "name": "Old Product",
        "price": "$99.99",
        "decoration": "line-through",
        "color": "#999"
    }
}
-->

<!-- Data model for active item:
{
    "item": {
        "name": "Current Product",
        "price": "$79.99",
        "decoration": "none",
        "color": "#000"
    }
}
-->
```

### Example 3: Document Revision Tracking

```html
<p style="font-size: 11pt">
    The deadline is
    <span style="text-decoration: {{model.changes.oldValueDecoration}}; color: #999">
        {{model.changes.oldValue}}
    </span>
    {{model.changes.newValue}}
</p>

<!-- Data model example:
{
    "changes": {
        "oldValue": "March 15",
        "oldValueDecoration": "line-through",
        "newValue": "March 20"
    }
}
-->
```

---

## Examples

### Example 1: Underlined Link

```html
<a href="https://example.com" style="color: blue; text-decoration: underline">
    Visit our website
</a>
```

### Example 2: Remove Link Underline

```html
<a href="https://example.com" style="color: #2563eb; text-decoration: none">
    Clean link without underline
</a>
```

### Example 3: Strikethrough Price

```html
<p style="font-size: 14pt">
    <span style="text-decoration: line-through; color: #999">$99.99</span>
    <span style="color: #dc2626; font-weight: bold">$79.99</span>
</p>
```

### Example 4: Deleted Text

```html
<p style="font-size: 11pt">
    The meeting is scheduled for
    <span style="text-decoration: line-through">Tuesday</span>
    Wednesday at 2 PM.
</p>
```

### Example 5: Emphasized Header with Underline

```html
<h2 style="text-decoration: underline; font-size: 18pt; color: #333">
    Important Notice
</h2>
```

### Example 6: Overline for Special Emphasis

```html
<p style="font-size: 12pt">
    <span style="text-decoration: overline; font-weight: bold">
        Special Announcement
    </span>
</p>
```

### Example 7: Sale Price Display

```html
<div style="padding: 20pt; border: 2pt solid #dc2626">
    <h3 style="font-size: 20pt; color: #dc2626">
        Limited Time Offer!
    </h3>
    <p style="font-size: 16pt">
        Original Price:
        <span style="text-decoration: line-through; color: #666">$299.99</span>
    </p>
    <p style="font-size: 24pt; color: #dc2626; font-weight: bold">
        Now Only: $199.99
    </p>
</div>
```

### Example 8: Document Revisions

```html
<p style="font-size: 11pt; line-height: 1.6">
    The company will
    <span style="text-decoration: line-through; color: #999">implement</span>
    has implemented the new policy effective immediately.
    All employees
    <span style="text-decoration: line-through; color: #999">should</span>
    must attend the training session.
</p>
```

### Example 9: Navigation Menu

```html
<div style="background-color: #1e293b; padding: 15pt">
    <a href="#home" style="color: white; text-decoration: none; margin-right: 20pt">
        Home
    </a>
    <a href="#about" style="color: white; text-decoration: underline; margin-right: 20pt">
        About
    </a>
    <a href="#contact" style="color: white; text-decoration: none">
        Contact
    </a>
</div>
```

### Example 10: Product Comparison Table

```html
<table style="width: 100%; border-collapse: collapse">
    <tr>
        <th style="padding: 8pt; border: 1pt solid #ccc">Feature</th>
        <th style="padding: 8pt; border: 1pt solid #ccc">Basic</th>
        <th style="padding: 8pt; border: 1pt solid #ccc">Premium</th>
    </tr>
    <tr>
        <td style="padding: 6pt; border: 1pt solid #ccc">Storage</td>
        <td style="padding: 6pt; border: 1pt solid #ccc">10 GB</td>
        <td style="padding: 6pt; border: 1pt solid #ccc">
            <span style="text-decoration: line-through">10 GB</span> 100 GB
        </td>
    </tr>
    <tr>
        <td style="padding: 6pt; border: 1pt solid #ccc">Price</td>
        <td style="padding: 6pt; border: 1pt solid #ccc">$9.99/mo</td>
        <td style="padding: 6pt; border: 1pt solid #ccc">
            <span style="text-decoration: line-through; color: #666">$29.99</span>
            $19.99/mo
        </td>
    </tr>
</table>
```

### Example 11: Terms and Conditions

```html
<div style="font-size: 10pt; line-height: 1.5">
    <p>
        <span style="text-decoration: underline; font-weight: bold">
            Section 1: Payment Terms
        </span>
    </p>
    <p>
        Payment is due within 30 days of invoice date.
        <span style="text-decoration: line-through">45 days</span>
    </p>
</div>
```

### Example 12: Shopping Cart

```html
<div style="padding: 20pt; border: 1pt solid #e5e7eb">
    <h3 style="font-size: 16pt">Your Cart</h3>
    <div style="margin: 10pt 0; padding: 10pt; background-color: #fef2f2">
        <p style="font-size: 12pt">
            <span style="text-decoration: line-through; color: #991b1b">
                Premium Widget - $49.99
            </span>
        </p>
        <p style="font-size: 10pt; color: #dc2626">
            Item removed from cart
        </p>
    </div>
    <div style="margin: 10pt 0; padding: 10pt">
        <p style="font-size: 12pt">
            Standard Widget - $29.99
        </p>
    </div>
</div>
```

### Example 13: Email Signature

```html
<div style="font-size: 11pt; padding: 15pt; border-top: 2pt solid #2563eb">
    <p style="font-weight: bold">John Smith</p>
    <p>Senior Developer</p>
    <p>
        <a href="mailto:john@example.com" style="color: #2563eb; text-decoration: underline">
            john@example.com
        </a>
    </p>
    <p>
        <a href="https://example.com" style="color: #2563eb; text-decoration: none">
            www.example.com
        </a>
    </p>
</div>
```

### Example 14: Recipe Card

```html
<div style="padding: 20pt; background-color: #fffbeb">
    <h2 style="text-decoration: underline; color: #92400e">
        Chocolate Chip Cookies
    </h2>
    <p style="font-size: 11pt; margin-top: 15pt">
        Ingredients:
    </p>
    <ul style="font-size: 11pt">
        <li>2 cups flour</li>
        <li>
            <span style="text-decoration: line-through">1 cup</span> 3/4 cup sugar
        </li>
        <li>1 cup chocolate chips</li>
    </ul>
</div>
```

### Example 15: Invoice with Corrections

```html
<div style="padding: 30pt">
    <h1 style="font-size: 24pt">INVOICE</h1>
    <p style="font-size: 11pt; margin-top: 20pt">
        Invoice Number:
        <span style="text-decoration: line-through">INV-001</span>
        INV-001-REVISED
    </p>
    <table style="width: 100%; margin-top: 20pt; border-collapse: collapse">
        <tr>
            <th style="text-align: left; padding: 8pt; border-bottom: 2pt solid black">
                Description
            </th>
            <th style="text-align: right; padding: 8pt; border-bottom: 2pt solid black">
                Amount
            </th>
        </tr>
        <tr>
            <td style="padding: 6pt; border-bottom: 1pt solid #ddd">
                Consulting Services
            </td>
            <td style="text-align: right; padding: 6pt; border-bottom: 1pt solid #ddd">
                <span style="text-decoration: line-through; color: #666">$5,000.00</span>
                $4,500.00
            </td>
        </tr>
        <tr>
            <td style="padding: 6pt; font-weight: bold">
                Total Due
            </td>
            <td style="text-align: right; padding: 6pt; font-weight: bold">
                $4,500.00
            </td>
        </tr>
    </table>
</div>
```

---

## See Also

- [text-decoration-line](/reference/cssproperties/text-decoration-line) - Specific decoration line types
- [text-decoration-color](/reference/cssproperties/text-decoration-color) - Decoration color
- [text-decoration-style](/reference/cssproperties/text-decoration-style) - Decoration line style
- [color](/reference/cssproperties/color) - Text color
- [font-weight](/reference/cssproperties/font-weight) - Text weight
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
