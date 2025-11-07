---
layout: default
title: br
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;br&gt; : The Line Break Element
{: .no_toc }

---

<details open class='top-toc' markdown="block">
  <summary>
    On this page
  </summary>
  {: .text-delta }
- TOC
{: toc}
</details>

---

## Summary

The `<br/>` element creates a line break within text content, forcing subsequent content to start on a new line without creating a new paragraph or block-level container.

---

## Usage

The `<br/>` element:
- Creates a single line break in flowing text
- Does not add vertical spacing beyond the line height
- Works within inline and block-level containers
- Should be **closed** when used to respect the xhtml structure.
- Differs from `<p>` which creates a new paragraph with margins

```html
<p>
    First line of text<br/>
    Second line of text<br/>
    Third line of text
</p>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `class` | string | CSS class name(s) for styling. |
| `style` | string | Inline CSS styles. Generally not needed for line breaks. |
| `title` | string | Outline/bookmark title. Rarely used with line breaks. |
| `hidden` | string | Controls visibility. Set to "hidden" to prevent the line break. |

### CSS Style Support

While `<br/>` can technically accept styles, it's rarely styled. Common scenarios:

**Visibility**:
- `display: none` - Hide the break
- `visibility: hidden` - Reserve space but don't show

---

## Notes

### When to Use &lt;br&gt;

Use `<br/>` for:
- **Postal Addresses**: Breaking address lines
- **Poetry or Verse**: Preserving line structure
- **Line-Based Content**: Form labels, contact information
- **Within Paragraphs**: Manual line control inside a single paragraph

```html
<!-- Good use case: Address -->
<p>
    John Smith<br/>
    123 Main Street<br/>
    Anytown, ST 12345
</p>
```

### When NOT to Use &lt;br&gt;

Avoid `<br/>` for:
- **Spacing Between Sections**: Use `<p>` or `<div>` with margins instead
- **Creating Lists**: Use `<ul>` or `<ol>` elements
- **Layout**: Use CSS margins, padding, or positioning
- **Multiple Consecutive Breaks**: Use proper spacing with CSS

```html
<!-- Bad: Multiple breaks for spacing -->
<br/><br/><br/>

<!-- Good: Proper spacing with CSS -->
<div style="margin-top: 30pt;">Content</div>
```

### Line Break vs Paragraph Break

**Line Break (`<br/>`):**
- Creates a new line within the same paragraph
- No additional vertical spacing
- Maintains text flow and styling context
- Use for content that should be together

**Paragraph Break (`<p>`):**
- Creates a new block-level paragraph
- Adds default top/bottom margins (0.5em)
- Creates a new formatting context
- Use for separate thoughts or sections

```html
<!-- Line breaks within a paragraph -->
<p>
    This is one paragraph<br/>
    with multiple lines<br/>
    but it's all together.
</p>

<!-- Separate paragraphs -->
<p>This is the first paragraph.</p>
<p>This is the second paragraph.</p>
```

### Line Break Behavior

The `<br/>` element:
- Forces content to the next line at the current position
- Inherits line height from its parent container
- Does not break across pages (it's part of the line flow)
- Can be placed anywhere text content is valid

---

## Examples

### Basic Line Breaks

```html
<p>
    Line one<br/>
    Line two<br/>
    Line three
</p>
```

### Address Formatting

```html
<div style="font-size: 10pt;">
    <strong>Shipping Address:</strong><br/>
    Jane Doe<br/>
    456 Oak Avenue<br/>
    Suite 200<br/>
    Springfield, IL 62701<br/>
    United States
</div>
```

### Poetry or Verse

```html
<div style="font-style: italic; line-height: 1.6;">
    Roses are red,<br/>
    Violets are blue,<br/>
    You can make PDFs,<br/>
    Beautiful and true.
</div>
```

### Contact Information Block

```html
<div style="border: 1pt solid #ccc; padding: 10pt; background-color: #f9f9f9;">
    <strong>Contact Us</strong><br/>
    Phone: (555) 123-4567<br/>
    Email: info@example.com<br/>
    Hours: Monday-Friday 9am-5pm
</div>
```

### Form-Style Layout

```html
<div>
    <strong>Name:</strong> John Smith<br/>
    <strong>Employee ID:</strong> E12345<br/>
    <strong>Department:</strong> Engineering<br/>
    <strong>Location:</strong> Building A, Floor 3
</div>
```

### Invoice Header

```html
<div style="text-align: right; font-size: 9pt;">
    Acme Corporation<br/>
    123 Business Blvd<br/>
    Commerce City, CA 90210<br/>
    Tel: (555) 987-6543<br/>
    www.acmecorp.com
</div>
```

### Signature Block

```html
<div style="margin-top: 40pt;">
    Sincerely,<br/>
    <br/>
    <br/>
    _______________________<br/>
    John Doe<br/>
    Chief Executive Officer<br/>
    Acme Corporation
</div>
```

### Multi-Column Contact List

```html
<div style="column-count: 2; column-gap: 20pt;">
    <div style="margin-bottom: 15pt;">
        <strong>Alice Johnson</strong><br/>
        Sales Manager<br/>
        alice@example.com<br/>
        (555) 123-0001
    </div>
    <div style="margin-bottom: 15pt;break-before: always">
        <strong>Bob Wilson</strong><br/>
        Technical Lead<br/>
        bob@example.com<br/>
        (555) 123-0002
    </div>
    <div style="margin-bottom: 15pt; break-before: always;">
        <strong>Carol Martinez</strong><br/>
        Marketing Director<br/>
        carol@example.com<br/>
        (555) 123-0003
    </div>
</div>
```

### Song Lyrics with Stanzas

```html
<div style="font-family: 'Courier New'; line-height: 1.8; font-size: 10pt;">
    <p>
        First line of first verse<br/>
        Second line of first verse<br/>
        Third line of first verse<br/>
        Fourth line of first verse
    </p>

    <p>
        First line of second verse<br/>
        Second line of second verse<br/>
        Third line of second verse<br/>
        Fourth line of second verse
    </p>
</div>
```

### Table Cell with Multi-Line Content

```html
<table style="width: 100%;">
    <tr>
        <td style="width: 30%;">Customer:</td>
        <td>
            ABC Company<br/>
            Attention: Jane Smith<br/>
            Purchase Order: PO-12345
        </td>
    </tr>
    <tr>
        <td>Delivery Address:</td>
        <td>
            789 Industrial Way<br/>
            Warehouse B<br/>
            Logistics City, TX 75001
        </td>
    </tr>
</table>
```

### Legal Document Footer

```html
<div style="font-size: 8pt; text-align: center; border-top: 1pt solid #ccc; padding-top: 10pt;">
    Legal Disclaimer: This document contains confidential information.<br/>
    © 2025 Acme Corporation. All rights reserved.<br/>
    Document ID: DOC-2025-001 | Generated: January 15, 2025
</div>
```

### Certificate Text

```html
<div style="text-align: center; padding: 40pt;">
    <h1>Certificate of Completion</h1>
    <p style="font-size: 14pt; margin: 30pt 0;">
        This certifies that<br/>
        <br/>
        <strong style="font-size: 18pt;">John Doe</strong><br/>
        <br/>
        has successfully completed the course<br/>
        <br/>
        <em>Advanced PDF Generation</em><br/>
        <br/>
        on January 15, 2025
    </p>
</div>
```

### Data Sheet Specifications

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt; width: 40%;">
            <strong>Model Number</strong>
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">
            XYZ-2000<br/>
            <span style="font-size: 8pt; color: #666;">(Discontinued models: XYZ-1000, XYZ-1500)</span>
        </td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 8pt;">
            <strong>Specifications</strong>
        </td>
        <td style="border: 1pt solid #ccc; padding: 8pt;">
            Width: 120mm<br/>
            Height: 80mm<br/>
            Depth: 40mm<br/>
            Weight: 2.5kg
        </td>
    </tr>
</table>
```

---

## See Also

- [p](html_p_element.html) - Paragraph element with margins
- [div](html_div_element.html) - Block-level container
- [hr](html_hr_element.html) - Horizontal rule divider
- [span](html_span_element.html) - Inline container
- [pre](html_pre_element.html) - Preformatted text with preserved whitespace

---
