---
layout: default
title: white-space
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# white-space : White Space Property

The `white-space` property controls how whitespace (spaces, tabs, and line breaks) inside an element is handled. This property is essential for formatting preformatted text, code blocks, and controlling text wrapping behavior in PDF documents.

## Usage

```css
/* Keyword values */
white-space: normal;
white-space: nowrap;
white-space: pre;
white-space: pre-wrap;
white-space: pre-line;
```

---

## Values

### Whitespace Keywords

- **normal** - Sequences of whitespace collapse into single space; text wraps normally (default)
- **nowrap** - Sequences of whitespace collapse into single space; text does not wrap
- **pre** - Whitespace is preserved exactly as written; text does not wrap; behaves like `<pre>` element
- **pre-wrap** - Whitespace is preserved; text wraps at natural line breaks and as needed
- **pre-line** - Sequences of whitespace collapse except for line breaks; text wraps normally

### Default Value

- **normal** - Default whitespace handling

---

## Behavior Comparison

| Value | Collapse Whitespace | Preserve Line Breaks | Text Wrapping |
|-------|---------------------|---------------------|---------------|
| normal | Yes | No | Yes |
| nowrap | Yes | No | No |
| pre | No | Yes | No |
| pre-wrap | No | Yes | Yes |
| pre-line | Yes | Yes | Yes |

---

## Notes

- The `normal` value is standard for most text content
- The `pre` value is useful for displaying code or preformatted content
- The `nowrap` value prevents text from wrapping to the next line
- The `pre-wrap` value combines whitespace preservation with text wrapping
- The `pre-line` value is useful for poetry or addresses where line breaks matter
- Whitespace handling affects how spaces, tabs, and newlines in HTML are rendered
- Tab characters are typically rendered as multiple spaces when preserved
- The property affects text layout but not the actual HTML content

---

## Data Binding

The `white-space` property can be dynamically controlled through data binding, allowing whitespace handling to vary based on content type, format requirements, or display context.

### Example 1: Content Type-Based Whitespace Handling

```html
<div style="white-space: {{model.content.whitespaceMode}}; font-family: {{model.content.fontFamily}}; font-size: 10pt">
    {{model.content.text}}
</div>

<!-- Data model for code:
{
    "content": {
        "text": "function example() {\n    return true;\n}",
        "whitespaceMode": "pre",
        "fontFamily": "'Courier New', monospace"
    }
}
-->

<!-- Data model for normal text:
{
    "content": {
        "text": "This is regular text content...",
        "whitespaceMode": "normal",
        "fontFamily": "Arial, sans-serif"
    }
}
-->
```

### Example 2: Address Formatting with Data

```html
<div style="white-space: {{model.address.whitespaceMode}}; font-size: 11pt">
{{model.address.name}}
{{model.address.street}}
{{model.address.city}}, {{model.address.state}} {{model.address.zip}}
</div>

<!-- Data model example:
{
    "address": {
        "name": "John Smith",
        "street": "123 Main Street",
        "city": "New York",
        "state": "NY",
        "zip": "10001",
        "whitespaceMode": "pre-line"
    }
}
-->
```

### Example 3: Dynamic Table Cell Wrapping

```html
<table style="width: 100%; border-collapse: collapse">
    <tr>
        <td style="white-space: {{model.column.whitespaceMode}}; padding: 8pt; border: 1pt solid #ccc">
            {{model.column.text}}
        </td>
    </tr>
</table>

<!-- Data model for nowrap:
{
    "column": {
        "text": "SKU-2024-001-PREMIUM",
        "whitespaceMode": "nowrap"
    }
}
-->

<!-- Data model for normal wrap:
{
    "column": {
        "text": "This is a longer description that should wrap normally",
        "whitespaceMode": "normal"
    }
}
-->
```

---

## Examples

### Example 1: Normal Whitespace (Default)

```html
<p style="white-space: normal; font-size: 12pt">
    This text    has    multiple    spaces    that
    will collapse into single spaces. Line breaks
    in the source code are also ignored.
</p>
```

### Example 2: No Wrap Text

```html
<div style="width: 200pt; border: 1pt solid #ccc; padding: 10pt">
    <p style="white-space: nowrap; font-size: 11pt">
        This very long line of text will not wrap to the next line even though the container is narrow.
    </p>
</div>
```

### Example 3: Preformatted Code

```html
<pre style="white-space: pre; font-family: 'Courier New', monospace; font-size: 10pt; background-color: #f5f5f5; padding: 15pt; border: 1pt solid #ddd">
function example() {
    if (condition) {
        console.log("Hello World");
    }
    return true;
}
</pre>
```

### Example 4: Pre-Wrap for Code with Wrapping

```html
<div style="white-space: pre-wrap; font-family: 'Courier New', monospace; font-size: 10pt; background-color: #f5f5f5; padding: 15pt; border: 1pt solid #ddd; max-width: 300pt">
function veryLongFunctionNameThatMightNeedToWrap() {
    const longVariableName = "This is a very long string that will wrap to the next line when necessary";
    return longVariableName;
}
</div>
```

### Example 5: Pre-Line for Addresses

```html
<div style="white-space: pre-line; font-size: 11pt; line-height: 1.5">
John Smith
123 Main Street
Apartment 4B
New York, NY 10001
</div>
```

### Example 6: Table Cell with No Wrap

```html
<table style="width: 100%; border-collapse: collapse">
    <tr>
        <th style="white-space: nowrap; padding: 8pt; border: 1pt solid #ccc; background-color: #f0f0f0">
            Product Name
        </th>
        <th style="white-space: nowrap; padding: 8pt; border: 1pt solid #ccc; background-color: #f0f0f0">
            SKU Number
        </th>
        <th style="white-space: nowrap; padding: 8pt; border: 1pt solid #ccc; background-color: #f0f0f0">
            Unit Price
        </th>
    </tr>
    <tr>
        <td style="padding: 6pt; border: 1pt solid #ccc">Premium Widget</td>
        <td style="white-space: nowrap; padding: 6pt; border: 1pt solid #ccc">SKU-2024-001</td>
        <td style="white-space: nowrap; padding: 6pt; border: 1pt solid #ccc; text-align: right">$29.99</td>
    </tr>
</table>
```

### Example 7: Poetry with Pre-Line

```html
<div style="white-space: pre-line; font-style: italic; font-size: 12pt; padding: 20pt; background-color: #fefce8">
Two roads diverged in a yellow wood,
And sorry I could not travel both
And be one traveler, long I stood
And looked down one as far as I could
To where it bent in the undergrowth;

— Robert Frost
</div>
```

### Example 8: SQL Query Display

```html
<div style="white-space: pre; font-family: 'Courier New', monospace; font-size: 10pt; background-color: #1e293b; color: #e2e8f0; padding: 15pt; border-radius: 4pt">
SELECT
    customer_id,
    first_name,
    last_name,
    email
FROM customers
WHERE active = true
ORDER BY last_name;
</div>
```

### Example 9: Invoice Address Block

```html
<div style="font-size: 11pt">
    <p style="font-weight: bold; margin-bottom: 5pt">Bill To:</p>
    <div style="white-space: pre-line">
Acme Corporation
456 Business Ave
Suite 200
Chicago, IL 60601
    </div>
</div>
```

### Example 10: Ellipsis with No Wrap

```html
<div style="width: 200pt; border: 1pt solid #e5e7eb; padding: 10pt">
    <p style="white-space: nowrap; overflow: hidden; text-overflow: ellipsis; font-size: 11pt">
        This is a very long product name that will be truncated with an ellipsis
    </p>
</div>
```

### Example 11: Terminal Output Simulation

```html
<div style="white-space: pre; font-family: 'Courier New', monospace; font-size: 9pt; background-color: #000; color: #0f0; padding: 15pt">
$ npm install scryber-core
Installing packages...
✓ scryber-core@3.0.0
✓ dependencies installed
✓ Build completed successfully

Ready to use!
</div>
```

### Example 12: Configuration File Display

```html
<div style="white-space: pre; font-family: 'Courier New', monospace; font-size: 10pt; background-color: #fafaf9; padding: 15pt; border-left: 4pt solid #3b82f6">
{
  "version": "1.0.0",
  "settings": {
    "theme": "light",
    "fontSize": 12,
    "lineHeight": 1.5
  }
}
</div>
```

### Example 13: Mixed Whitespace Handling

```html
<div style="padding: 20pt">
    <h3 style="white-space: nowrap; font-size: 14pt; color: #1e293b">
        Document Properties
    </h3>
    <div style="white-space: pre-line; font-size: 11pt; margin-top: 10pt; line-height: 1.6">
Title: Annual Report 2024
Author: Finance Department
Date: October 14, 2025
Status: Final
    </div>
</div>
```

### Example 14: Email Template

```html
<div style="font-size: 11pt; padding: 20pt; background-color: #f9fafb">
    <p style="white-space: normal; margin-bottom: 15pt">
        Dear Valued Customer,
    </p>
    <p style="white-space: normal; margin-bottom: 15pt; line-height: 1.6">
        Thank you for your recent order. Your items have been shipped
        and should arrive within 3-5 business days.
    </p>
    <div style="white-space: pre-line; background-color: white; padding: 15pt; border: 1pt solid #e5e7eb; margin: 15pt 0">
Order Number: #2024-10-001
Ship Date: October 14, 2025
Tracking: 1Z999AA10123456784
    </div>
    <p style="white-space: normal">
        Best regards,<br/>
        Customer Service Team
    </p>
</div>
```

### Example 15: Technical Documentation

```html
<div style="padding: 30pt">
    <h2 style="white-space: normal; font-size: 18pt; color: #1e40af; margin-bottom: 15pt">
        API Reference
    </h2>

    <p style="white-space: normal; font-size: 11pt; margin-bottom: 15pt; line-height: 1.6">
        The following code demonstrates basic usage of the API endpoint
        for retrieving user information:
    </p>

    <div style="white-space: pre; font-family: 'Courier New', monospace; font-size: 10pt; background-color: #f8f9fa; padding: 15pt; border: 1pt solid #dee2e6; margin-bottom: 15pt">
GET /api/v1/users/{id}
Authorization: Bearer {token}

Response:
{
  "id": 12345,
  "name": "John Smith",
  "email": "john@example.com",
  "active": true
}
    </div>

    <p style="white-space: normal; font-size: 11pt; line-height: 1.6">
        Replace {id} with the user ID and {token} with your API token.
        The endpoint returns a JSON object containing user details.
    </p>
</div>
```

---

## See Also

- [word-spacing](/reference/cssproperties/word-spacing) - Space between words
- [letter-spacing](/reference/cssproperties/letter-spacing) - Space between characters
- [text-align](/reference/cssproperties/text-align) - Text alignment
- [overflow](/reference/cssproperties/overflow) - Content overflow handling
- [line-height](/reference/cssproperties/line-height) - Line spacing
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
