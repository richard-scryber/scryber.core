---
layout: default
title: style
parent: HTML Attributes
parent_url: /reference/htmlattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @style : The Style Attribute

The `style` attribute applies inline CSS styles directly to an HTML element. While inline styles provide quick, element-specific formatting, Scryber offers comprehensive CSS styling support through `<style>` blocks and external stylesheets for more maintainable designs.

## Usage

The `style` attribute applies CSS styles directly to an element:
- Provides immediate, element-specific styling
- Takes precedence over class and element styles (highest CSS specificity)
- Useful for unique, one-off styling or dynamic values
- Supports data binding for dynamic style values
- For maintainable, reusable styles, use CSS classes instead
- Refer to the **[CSS Styles documentation](/reference/styles/)** for comprehensive styling capabilities

```html
<!-- Basic inline style -->
<p style="color: red; font-size: 14pt;">Styled paragraph</p>

<!-- Multiple style properties -->
<div style="background-color: #f0f0f0; padding: 15pt; border: 1pt solid #ccc;">
    Content with multiple styles
</div>

<!-- Dynamic style with data binding -->
<span style="color: {{model.textColor}}; font-weight: bold;">
    Dynamic styled text
</span>
```

---

## Supported Elements

The `style` attribute is supported on **all HTML elements** in Scryber, including:

### Block Elements
- `<div>`, `<section>`, `<article>`, `<aside>`, `<main>`, `<nav>`
- `<header>`, `<footer>`, `<address>`
- `<h1>` through `<h6>`, `<p>`, `<blockquote>`, `<pre>`
- `<ul>`, `<ol>`, `<li>`, `<dl>`, `<dt>`, `<dd>`
- `<table>`, `<thead>`, `<tbody>`, `<tfoot>`, `<tr>`, `<td>`, `<th>`
- `<fieldset>`, `<legend>`, `<figure>`, `<figcaption>`

### Inline Elements
- `<a>`, `<span>`, `<strong>`, `<em>`, `<b>`, `<i>`, `<u>`
- `<code>`, `<kbd>`, `<samp>`, `<var>`, `<mark>`, `<small>`

### Media Elements
- `<img>`, `<iframe>`, `<canvas>`

---

## Binding Values

The `style` attribute supports data binding for dynamic style values:

```html
<!-- Single dynamic property -->
<div style="color: {{model.primaryColor}};">
    Dynamically colored text
</div>

<!-- Multiple dynamic properties -->
<p style="font-size: {{model.fontSize}}pt; color: {{model.textColor}};">
    Custom sized and colored text
</p>

<!-- Computed values -->
<div style="width: {{model.width}}pt; height: {{model.height}}pt;
            background-color: {{model.bgColor}};">
    Dynamic dimensions and color
</div>

<!-- Conditional styling -->
<span style="font-weight: {{model.isImportant ? 'bold' : 'normal'}};
             color: {{model.isError ? 'red' : 'black'}};">
    Conditional style
</span>

<!-- In repeated content -->
<template data-bind="{{model.items}}">
    <div style="padding: 10pt; background-color: {{.color}};
                border-left: 4pt solid {{.borderColor}};">
        <h3 style="color: {{.textColor}};">{{.title}}</h3>
    </div>
</template>
```

**Data Model Example:**
```json
{
  "primaryColor": "#336699",
  "fontSize": 16,
  "textColor": "#333333",
  "width": 200,
  "height": 100,
  "bgColor": "#f0f0f0",
  "isImportant": true,
  "isError": false,
  "items": [
    {
      "title": "Item 1",
      "color": "#e3f2fd",
      "borderColor": "#2196f3",
      "textColor": "#1976d2"
    }
  ]
}
```

---

## Notes

### Comprehensive CSS Support

Scryber provides extensive CSS styling capabilities beyond inline styles. For detailed information about all supported CSS properties and advanced styling features, please refer to the **[CSS Styles Documentation](/reference/styles/)**.

The CSS Styles section covers:
- **Typography**: fonts, sizes, weights, text decoration, spacing
- **Colors**: text colors, backgrounds, gradients, opacity
- **Box Model**: margins, padding, borders, dimensions
- **Layout**: positioning, display, flexbox patterns
- **Visual Effects**: shadows, borders, border-radius
- **Lists**: list styles, markers, positioning
- **Tables**: cell spacing, borders, alignment
- **Page Layout**: page breaks, widows, orphans
- **And much more...**

### Inline Style Syntax

Inline styles use standard CSS property-value pairs separated by semicolons:

```html
<element style="property1: value1; property2: value2; property3: value3;">
```

**Important syntax rules:**
- Use colons (`:`) between property and value
- Use semicolons (`;`) to separate multiple properties
- Units must be specified for dimensions (`pt`, `px`, `%`, `em`, etc.)
- Colors can be names, hex codes, RGB, or RGBA
- Properties are case-insensitive but lowercase is conventional

```html
<!-- Correct syntax -->
<p style="font-size: 12pt; color: #336699; margin: 10pt;">Text</p>

<!-- Also correct (case-insensitive) -->
<p style="FONT-SIZE: 12pt; COLOR: red;">Text</p>

<!-- Incorrect: missing semicolon, no units -->
<p style="font-size: 12pt color: red margin: 10">Text</p>
```

### CSS Units

Scryber supports various CSS units:

| Unit | Description | Example |
|------|-------------|---------|
| `pt` | Points (recommended for PDFs) | `font-size: 12pt;` |
| `px` | Pixels | `width: 200px;` |
| `%` | Percentage of parent | `width: 50%;` |
| `em` | Relative to font size | `margin: 1.5em;` |
| `in` | Inches | `width: 8.5in;` |
| `cm` | Centimeters | `height: 29.7cm;` |
| `mm` | Millimeters | `border: 2mm solid;` |

```html
<div style="width: 100%; padding: 15pt; font-size: 12pt;">
    <p style="margin: 1em; line-height: 1.5;">Content</p>
    <img src="logo.png" style="width: 2in; height: 1in;" />
</div>
```

### Inline Styles vs CSS Classes

**Use Inline Styles For:**
- Unique, one-off styling
- Dynamic values from data binding
- Quick prototyping
- Overriding class styles when needed

**Use CSS Classes For:**
- Reusable style patterns
- Maintaining consistency
- Easier maintenance and updates
- Better readability
- Separation of style and content

```html
<!-- Good use of inline style: unique value -->
<div style="background-image: url('{{model.userPhoto}}');">

<!-- Better with class: reusable pattern -->
<style>
    .highlight-box {
        background-color: yellow;
        padding: 10pt;
        border: 2pt solid orange;
    }
</style>

<div class="highlight-box">Reusable style</div>
<div class="highlight-box">Another use</div>
```

### CSS Specificity

Inline styles have the highest CSS specificity and will override styles from classes and element selectors:

```html
<style>
    div { color: black; }           /* Specificity: 1 */
    .text { color: blue; }          /* Specificity: 10 */
    #special { color: green; }      /* Specificity: 100 */
</style>

<div>Black text</div>
<div class="text">Blue text</div>
<div class="text" id="special">Green text</div>
<div class="text" id="special" style="color: red;">Red text (inline wins)</div>
```

### Common CSS Properties

Here are some commonly used CSS properties in inline styles:

**Typography:**
```html
<p style="font-family: Arial, sans-serif;
         font-size: 14pt;
         font-weight: bold;
         font-style: italic;
         color: #333;
         text-align: center;
         line-height: 1.6;
         letter-spacing: 0.5pt;">
```

**Spacing:**
```html
<div style="margin: 20pt;
            padding: 15pt;
            margin-top: 10pt;
            padding-left: 20pt;">
```

**Borders:**
```html
<div style="border: 1pt solid #ccc;
            border-radius: 5pt;
            border-left: 4pt solid blue;">
```

**Backgrounds:**
```html
<div style="background-color: #f0f0f0;
            background: linear-gradient(to bottom, white, #e0e0e0);">
```

**Dimensions:**
```html
<div style="width: 200pt;
            height: 100pt;
            min-width: 150pt;
            max-width: 300pt;">
```

### Color Values

Scryber supports multiple color formats:

```html
<!-- Named colors -->
<span style="color: red;">Red text</span>
<span style="color: blue;">Blue text</span>

<!-- Hexadecimal colors -->
<span style="color: #336699;">Hex color</span>
<span style="color: #fff;">Short hex</span>

<!-- RGB colors -->
<span style="color: rgb(51, 102, 153);">RGB color</span>

<!-- RGBA colors (with alpha/opacity) -->
<span style="color: rgba(51, 102, 153, 0.5);">Semi-transparent</span>
```

### Multiple Properties

Apply multiple CSS properties by separating them with semicolons:

```html
<div style="
    width: 100%;
    padding: 20pt;
    margin-bottom: 15pt;
    background-color: #f8f9fa;
    border: 1pt solid #dee2e6;
    border-radius: 8pt;
    font-family: Arial, sans-serif;
    font-size: 12pt;
    color: #333;
    line-height: 1.6;
">
    Fully styled content
</div>
```

---

## Examples

### Basic Typography Styling

```html
<p style="font-family: 'Times New Roman', serif; font-size: 14pt; color: #333;">
    Serif paragraph text
</p>

<p style="font-family: Arial, sans-serif; font-size: 12pt; font-weight: bold; color: #336699;">
    Bold sans-serif text
</p>

<p style="font-style: italic; text-decoration: underline; color: #666;">
    Italic underlined text
</p>
```

### Box Styling

```html
<div style="
    width: 300pt;
    padding: 20pt;
    margin: 15pt;
    background-color: #f0f0f0;
    border: 2pt solid #336699;
    border-radius: 10pt;
">
    Box with padding, margin, border, and rounded corners
</div>
```

### Text Alignment and Spacing

```html
<p style="text-align: left; margin-bottom: 10pt;">Left-aligned text</p>
<p style="text-align: center; margin-bottom: 10pt;">Center-aligned text</p>
<p style="text-align: right; margin-bottom: 10pt;">Right-aligned text</p>
<p style="text-align: justify; line-height: 1.8;">
    Justified text with increased line height. This paragraph will be
    justified across the full width with even spacing between words.
</p>
```

### Color Styling

```html
<!-- Text colors -->
<span style="color: red;">Red text</span>
<span style="color: #336699;">Blue text (hex)</span>
<span style="color: rgb(51, 102, 153);">RGB color</span>

<!-- Background colors -->
<div style="background-color: yellow; padding: 5pt;">Yellow background</div>
<div style="background-color: #e3f2fd; padding: 10pt; margin-top: 5pt;">
    Light blue background
</div>

<!-- Gradients -->
<div style="background: linear-gradient(to right, #ffffff, #336699);
            padding: 20pt; color: white;">
    Gradient background
</div>
```

### Borders and Outlines

```html
<div style="border: 1pt solid black; padding: 10pt; margin-bottom: 10pt;">
    Simple solid border
</div>

<div style="border: 2pt dashed #336699; padding: 10pt; margin-bottom: 10pt;">
    Dashed border
</div>

<div style="border-left: 4pt solid red; padding-left: 10pt; margin-bottom: 10pt;">
    Left border accent
</div>

<div style="border: 1pt solid #ccc; border-radius: 15pt; padding: 15pt;">
    Rounded corners
</div>
```

### Layout with Dimensions

```html
<div style="width: 400pt; height: 200pt;
            background-color: #f0f0f0;
            padding: 20pt;
            margin: 0 auto;">
    Fixed width and height container
</div>

<div style="width: 100%; max-width: 500pt;
            padding: 15pt;
            background-color: #e0e0e0;">
    Full width with maximum constraint
</div>
```

### Data Binding with Styles

```html
<!-- Model: { theme: { primary: "#336699", fontSize: 14 } } -->

<div style="color: {{model.theme.primary}};
            font-size: {{model.theme.fontSize}}pt;
            padding: 15pt;
            border: 2pt solid {{model.theme.primary}};">
    Content with dynamic theme colors
</div>

<!-- Model: { status: "error", message: "Error occurred" } -->
<div style="
    padding: 10pt;
    background-color: {{model.status == 'error' ? '#f8d7da' : '#d4edda'}};
    color: {{model.status == 'error' ? '#721c24' : '#155724'}};
    border: 1pt solid {{model.status == 'error' ? '#f5c6cb' : '#c3e6cb'}};
">
    {{model.message}}
</div>
```

### Complex Styling

```html
<div style="
    width: 500pt;
    margin: 20pt auto;
    padding: 30pt;
    background: linear-gradient(to bottom, #ffffff, #f8f9fa);
    border: 1pt solid #dee2e6;
    border-radius: 10pt;
    box-shadow: 0 2pt 10pt rgba(0,0,0,0.1);
">
    <h2 style="
        margin: 0 0 15pt 0;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #336699;
        color: #336699;
        font-size: 20pt;
        font-weight: bold;
    ">
        Complex Styled Section
    </h2>
    <p style="
        line-height: 1.6;
        color: #333;
        margin-bottom: 15pt;
    ">
        This section demonstrates complex inline styling with multiple
        properties for layout, typography, and visual effects.
    </p>
</div>
```

### Table Cell Styling

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr>
        <th style="
            background-color: #336699;
            color: white;
            padding: 10pt;
            text-align: left;
            border: 1pt solid #2c5a8a;
        ">
            Header 1
        </th>
        <th style="
            background-color: #336699;
            color: white;
            padding: 10pt;
            text-align: left;
            border: 1pt solid #2c5a8a;
        ">
            Header 2
        </th>
    </tr>
    <tr>
        <td style="padding: 10pt; border: 1pt solid #dee2e6;">Cell 1</td>
        <td style="padding: 10pt; border: 1pt solid #dee2e6;">Cell 2</td>
    </tr>
</table>
```

### Link Styling

```html
<a href="https://example.com"
   style="
       color: #336699;
       text-decoration: none;
       font-weight: bold;
       border-bottom: 2pt solid #336699;
       padding-bottom: 2pt;
   ">
    Styled link
</a>

<a href="#section1"
   style="
       display: inline-block;
       padding: 10pt 20pt;
       background-color: #336699;
       color: white;
       text-decoration: none;
       border-radius: 5pt;
       margin: 5pt;
   ">
    Button-style link
</a>
```

### Image Styling

```html
<img src="photo.jpg"
     style="
         width: 200pt;
         height: 150pt;
         border: 3pt solid #336699;
         border-radius: 10pt;
         padding: 5pt;
         background-color: white;
     " />

<img src="logo.png"
     style="
         display: block;
         margin: 20pt auto;
         width: 150pt;
         height: auto;
     " />
```

### Responsive Width Patterns

```html
<!-- Percentage-based widths -->
<div style="width: 100%; background-color: #f0f0f0; padding: 10pt;">
    Full width container
</div>

<div style="width: 50%; background-color: #e0e0e0; padding: 10pt; margin-top: 10pt;">
    Half width container
</div>

<!-- Fixed width with centering -->
<div style="width: 400pt; margin: 20pt auto; background-color: #d0d0d0; padding: 10pt;">
    Centered fixed-width container
</div>
```

### List Styling

```html
<ul style="
    list-style-type: square;
    padding-left: 20pt;
    line-height: 2;
    color: #333;
">
    <li style="margin-bottom: 5pt;">First item</li>
    <li style="margin-bottom: 5pt;">Second item</li>
    <li style="margin-bottom: 5pt;">Third item</li>
</ul>

<ol style="
    padding-left: 25pt;
    font-weight: bold;
    color: #336699;
">
    <li style="margin-bottom: 8pt;">
        <span style="font-weight: normal; color: #333;">First numbered item</span>
    </li>
    <li style="margin-bottom: 8pt;">
        <span style="font-weight: normal; color: #333;">Second numbered item</span>
    </li>
</ol>
```

### Card Component with Inline Styles

```html
<div style="
    border: 1pt solid #dee2e6;
    border-radius: 8pt;
    padding: 0;
    margin-bottom: 20pt;
    overflow: hidden;
    background-color: white;
">
    <div style="
        background-color: #336699;
        color: white;
        padding: 15pt;
        font-size: 18pt;
        font-weight: bold;
    ">
        Card Header
    </div>
    <div style="padding: 15pt;">
        <p style="margin: 0 0 10pt 0; line-height: 1.6;">
            Card body content goes here. This demonstrates a card
            component built entirely with inline styles.
        </p>
    </div>
    <div style="
        background-color: #f8f9fa;
        padding: 10pt 15pt;
        font-size: 10pt;
        color: #6c757d;
        border-top: 1pt solid #dee2e6;
    ">
        Card footer
    </div>
</div>
```

### Alert Messages with Styles

```html
<!-- Success alert -->
<div style="
    padding: 15pt;
    margin-bottom: 15pt;
    background-color: #d4edda;
    border: 1pt solid #c3e6cb;
    border-left: 4pt solid #28a745;
    border-radius: 5pt;
    color: #155724;
">
    <strong style="font-weight: bold;">Success!</strong>
    Your operation completed successfully.
</div>

<!-- Error alert -->
<div style="
    padding: 15pt;
    margin-bottom: 15pt;
    background-color: #f8d7da;
    border: 1pt solid #f5c6cb;
    border-left: 4pt solid #dc3545;
    border-radius: 5pt;
    color: #721c24;
">
    <strong style="font-weight: bold;">Error!</strong>
    An error occurred. Please try again.
</div>

<!-- Warning alert -->
<div style="
    padding: 15pt;
    background-color: #fff3cd;
    border: 1pt solid #ffeeba;
    border-left: 4pt solid #ffc107;
    border-radius: 5pt;
    color: #856404;
">
    <strong style="font-weight: bold;">Warning!</strong>
    Please review this information carefully.
</div>
```

### Dynamic Status Indicators

```html
<!-- Model: { items: [{name: "Task 1", status: "complete", priority: "high"}] } -->

<template data-bind="{{model.items}}">
    <div style="
        padding: 15pt;
        margin-bottom: 10pt;
        border-left: 4pt solid {{.status == 'complete' ? '#28a745' : '#ffc107'}};
        background-color: {{.priority == 'high' ? '#ffebee' : '#f8f9fa'}};
    ">
        <h4 style="margin: 0 0 5pt 0; color: {{.status == 'complete' ? '#28a745' : '#333'}};">
            {{.name}}
        </h4>
        <p style="margin: 0; font-size: 10pt; color: #666;">
            Status: {{.status}} | Priority: {{.priority}}
        </p>
    </div>
</template>
```

### Blockquote Styling

```html
<blockquote style="
    margin: 20pt 0;
    padding: 15pt 20pt;
    border-left: 5pt solid #336699;
    background-color: #f8f9fa;
    font-style: italic;
    color: #555;
">
    <p style="margin: 0 0 10pt 0; font-size: 14pt; line-height: 1.6;">
        This is a styled blockquote with custom colors, borders,
        and typography to make it stand out from regular text.
    </p>
    <footer style="
        text-align: right;
        font-size: 11pt;
        color: #666;
        font-style: normal;
    ">
        — Author Name
    </footer>
</blockquote>
```

### Footer with Full Styling

```html
<footer style="
    margin-top: 40pt;
    padding: 20pt;
    background: linear-gradient(to bottom, #f8f9fa, #e9ecef);
    border-top: 3pt solid #336699;
    text-align: center;
">
    <div style="
        max-width: 600pt;
        margin: 0 auto;
    ">
        <p style="
            margin: 0 0 10pt 0;
            font-size: 14pt;
            font-weight: bold;
            color: #336699;
        ">
            Company Name
        </p>
        <p style="
            margin: 0 0 5pt 0;
            font-size: 10pt;
            color: #666;
            line-height: 1.5;
        ">
            123 Business Street, City, State 12345
        </p>
        <p style="
            margin: 0;
            font-size: 9pt;
            color: #999;
        ">
            © 2025 Company Name. All rights reserved.
        </p>
    </div>
</footer>
```

---

## See Also

- [class](/reference/htmlattributes/class.html) - CSS class attribute for reusable styles
- [id](/reference/htmlattributes/id.html) - Unique identifier attribute
- [CSS Styles](/reference/styles/) - **Comprehensive CSS styling documentation**
- [style element](/reference/htmltags/style.html) - Defining CSS stylesheets
- [Data Binding](/reference/binding/) - Dynamic data binding in attributes
- [div](/reference/htmltags/div.html) - Generic container element
- [span](/reference/htmltags/span.html) - Generic inline element

---
