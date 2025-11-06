---
layout: default
title: strong, em, b, i
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;strong&gt;, &lt;em&gt;, &lt;b&gt;, &lt;i&gt; : Bold and Italic Text Elements
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

The bold and italic text elements provide both semantic and presentational text emphasis in PDF documents. `<strong>` and `<em>` are semantic elements indicating importance and emphasis, while `<b>` and `<i>` are presentational elements for visual styling without semantic meaning.

---

## Usage

These inline elements format text with bold or italic styles:

**Semantic Elements (Recommended)**:
- `<strong>`: Indicates strong importance, seriousness, or urgency (renders as bold)
- `<em>`: Indicates emphasis or stress (renders as italic)

**Presentational Elements**:
- `<b>`: Bold text without semantic meaning (stylistic offset)
- `<i>`: Italic text without semantic meaning (stylistic offset)

All four elements:
- Flow inline with surrounding text
- Can be nested within each other for combined effects
- Support full CSS styling
- Can contain other inline elements
- Support data binding

```html
<!-- Semantic (preferred) -->
<p>This is <strong>very important</strong> information.</p>
<p>You <em>must</em> read this carefully.</p>

<!-- Presentational -->
<p>The <b>title</b> appears in bold.</p>
<p>The term <i>lorem ipsum</i> is Latin.</p>
```

---

## Supported Attributes

### Standard HTML Attributes

All four elements support the same attributes:

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the element. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### CSS Style Support

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color`, `text-decoration`, `text-transform`
- `letter-spacing`, `word-spacing`, `line-height`

**Background and Borders**:
- `background-color`, `background-image`
- `border`, `border-radius`
- `padding`, `margin`

**Visual Effects**:
- `opacity`, `text-shadow`
- `transform` (rotation, scaling)

**Display**:
- `display`: `inline` (default), `inline-block`, `block`, `none`
- `vertical-align`

---

## Notes

### Semantic vs Presentational

**Use Semantic Elements When**:
- Content has meaningful emphasis or importance
- Screen readers should convey emphasis
- The meaning would change without the formatting
- Building accessible documents

**Use Presentational Elements When**:
- Styling is purely visual (names, terms, keywords)
- No semantic meaning is intended
- Following design conventions without semantic weight

### Default Styling

**`<strong>` and `<b>`**:
- Font weight: Bold
- Inherits all other styles from parent
- No other default styling

**`<em>` and `<i>`**:
- Font style: Italic
- Inherits all other styles from parent
- No other default styling

### Nesting and Combinations

Elements can be nested to combine effects:

```html
<!-- Bold and italic -->
<strong><em>Very important emphasis</em></strong>
<b><i>Bold italic text</i></b>

<!-- With other formatting -->
<strong><u>Bold underlined</u></strong>
```

---

## Class Hierarchy

In the Scryber codebase:
- `HTMLBoldSpan`/`HTMLStrong` extends `BoldSpan` extends `SpanBase` extends `Panel`
- `HTMLItalicSpan`/`HTMLEmphasis` extends `ItalicSpan` extends `SpanBase` extends `Panel`
- Both inherit inline display behavior from `SpanBase`
- Both support nested content through `Contents` collection

---

## Examples

### Basic Bold Text

```html
<!-- Semantic bold -->
<p>This is <strong>important information</strong> you should know.</p>
<p><strong>Warning:</strong> Do not proceed without authorization.</p>

<!-- Presentational bold -->
<p>The <b>product name</b> is displayed in bold.</p>
<p><b>Chapter 1:</b> Getting Started</p>
```

### Basic Italic Text

```html
<!-- Semantic emphasis -->
<p>You <em>must</em> complete this form.</p>
<p>I <em>really</em> appreciate your help.</p>

<!-- Presentational italic -->
<p>The book <i>Moby Dick</i> is a classic.</p>
<p>The term <i>carpe diem</i> means "seize the day".</p>
```

### Combined Bold and Italic

```html
<!-- Using nested tags -->
<p>This is <strong><em>extremely important</em></strong> text.</p>
<p>The term <b><i>status quo</i></b> appears in bold italic.</p>

<!-- Using CSS -->
<p>This is <span style="font-weight: bold; font-style: italic;">bold italic</span>.</p>
```

### With Color and Styling

```html
<!-- Red bold text -->
<p>Error: <strong style="color: red;">Invalid input detected</strong></p>

<!-- Green bold success message -->
<p><strong style="color: green; font-size: 12pt;">Success!</strong></p>

<!-- Styled emphasis -->
<p>Please <em style="color: blue; text-decoration: underline;">click here</em> to continue.</p>
```

### With CSS Classes

```html
<style>
    .important {
        color: red;
        font-weight: bold;
    }
    .highlight {
        background-color: yellow;
        padding: 2pt;
    }
    .term {
        font-style: italic;
        color: #333;
    }
    .strong-large {
        font-weight: bold;
        font-size: 14pt;
        color: #003366;
    }
</style>

<p>This is <strong class="important">critical information</strong>.</p>
<p>The <b class="highlight">highlighted term</b> is key.</p>
<p>The Latin phrase <i class="term">et cetera</i> means "and so forth".</p>
<p><strong class="strong-large">Chapter Title</strong></p>
```

### Headers and Subheaders

```html
<h2><strong>Section 1: Introduction</strong></h2>
<p><b>Overview:</b> This section covers the basics.</p>

<h3><em>Important Considerations</em></h3>
<p><i>Note:</i> Please review carefully.</p>
```

### Lists with Emphasis

```html
<ul>
    <li><strong>Bold item</strong>: Description text here.</li>
    <li><strong>Another bold item</strong>: More details.</li>
    <li><em>Emphasized item</em>: Special note.</li>
</ul>

<ol>
    <li><b>Step 1:</b> Open the application.</li>
    <li><b>Step 2:</b> Enter your credentials.</li>
    <li><b>Step 3:</b> Click <strong>Submit</strong>.</li>
</ol>
```

### Data Binding

```html
<!-- With model = { userName: "John Smith", status: "Active", itemName: "Product A" } -->
<p>Welcome <strong>{{model.userName}}</strong>!</p>
<p>Status: <strong style="color: green;">{{model.status}}</strong></p>
<p>Item: <i>{{model.itemName}}</i></p>

<!-- Conditional styling -->
<p>Alert: <strong style="color: {{model.isError ? 'red' : 'green'}};">
    {{model.message}}
</strong></p>
```

### Tables with Bold Headers

```html
<table>
    <thead>
        <tr>
            <th><strong>Name</strong></th>
            <th><strong>Department</strong></th>
            <th><strong>Status</strong></th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>John Doe</td>
            <td><i>Engineering</i></td>
            <td><strong style="color: green;">Active</strong></td>
        </tr>
    </tbody>
</table>
```

### Nested Emphasis Levels

```html
<p>
    <strong>
        Important:
        <span style="font-size: 11pt;">
            This is critical information with
            <em>special emphasis</em> on certain points.
        </span>
    </strong>
</p>

<p>
    The book <b><i>War and Peace</i></b> by
    <i>Leo Tolstoy</i> is <strong>highly recommended</strong>.
</p>
```

### Warning and Error Messages

```html
<div style="border: 2pt solid red; padding: 10pt; background-color: #ffeeee;">
    <strong style="color: red; font-size: 12pt;">ERROR:</strong>
    <span>The operation failed. Please <strong>contact support</strong>.</span>
</div>

<div style="border: 2pt solid orange; padding: 10pt; background-color: #fff8dc;">
    <strong style="color: orange;">Warning:</strong>
    <span>This action <em>cannot be undone</em>.</span>
</div>

<div style="border: 2pt solid green; padding: 10pt; background-color: #eeffee;">
    <strong style="color: green;">Success:</strong>
    <span>Your changes have been saved.</span>
</div>
```

### Labels and Definitions

```html
<p><b>Name:</b> John Smith</p>
<p><b>Email:</b> john.smith@example.com</p>
<p><b>Department:</b> <i>Engineering</i></p>

<dl>
    <dt><strong>API</strong></dt>
    <dd><i>Application Programming Interface</i></dd>

    <dt><strong>PDF</strong></dt>
    <dd><i>Portable Document Format</i></dd>
</dl>
```

### Quotes and Citations

```html
<blockquote>
    <p><em>
        "The only way to do great work is to love what you do."
    </em></p>
    <p style="text-align: right;">- <strong>Steve Jobs</strong></p>
</blockquote>

<p>
    As noted in the book <i>Code Complete</i> by
    <strong>Steve McConnell</strong>, <em>good code is its own best documentation</em>.
</p>
```

### Document Metadata

```html
<div style="margin-bottom: 20pt;">
    <p><b>Document Title:</b> Annual Report 2024</p>
    <p><b>Author:</b> <i>Finance Department</i></p>
    <p><b>Date:</b> January 15, 2025</p>
    <p><b>Status:</b> <strong style="color: green;">Approved</strong></p>
</div>
```

### Inline Buttons/Labels

```html
<p>
    Click the <strong style="background-color: #336699; color: white;
    padding: 2pt 6pt; border-radius: 3pt;">Submit</strong> button to continue.
</p>

<p>
    Press <b style="border: 1pt solid #999; padding: 2pt 6pt;
    background-color: #f5f5f5; font-family: monospace;">Enter</b> to save.
</p>
```

### Forms and Instructions

```html
<div>
    <p><strong>Instructions:</strong></p>
    <ol>
        <li>Enter your <b>full name</b> in the field below.</li>
        <li>Provide a <em>valid email address</em>.</li>
        <li>Choose a <strong>strong password</strong> (minimum 8 characters).</li>
        <li>Read and accept the <i>Terms of Service</i>.</li>
    </ol>
    <p><strong style="color: red;">Note:</strong> All fields marked with
       <strong style="color: red;">*</strong> are <em>required</em>.</p>
</div>
```

### Scientific and Technical Text

```html
<p>
    The formula for the area of a circle is <i>A</i> = π<i>r</i><sup>2</sup>,
    where <i>r</i> is the radius.
</p>

<p>
    The <strong>Pythagorean theorem</strong> states that
    <i>a</i><sup>2</sup> + <i>b</i><sup>2</sup> = <i>c</i><sup>2</sup>.
</p>

<p>
    <b>Important:</b> The value of <i>x</i> must be
    <strong>greater than zero</strong>.
</p>
```

### Navigation and Menu Items

```html
<div style="padding: 10pt; background-color: #f0f0f0;">
    <strong>Menu:</strong>
    <a href="#home"><strong>Home</strong></a> |
    <a href="#products"><strong>Products</strong></a> |
    <a href="#services"><strong>Services</strong></a> |
    <a href="#contact"><strong>Contact</strong></a>
</div>
```

### Product Listings

```html
<div style="border: 1pt solid #ccc; padding: 15pt; margin: 10pt 0;">
    <h3><strong>Premium Widget</strong></h3>
    <p><b>SKU:</b> <i>WDG-001</i></p>
    <p><b>Price:</b> <strong style="color: green; font-size: 14pt;">$99.99</strong></p>
    <p><b>Status:</b> <strong style="color: green;">In Stock</strong></p>
    <p><em>Free shipping on orders over $50</em></p>
</div>
```

### Status Indicators

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr>
        <td style="padding: 5pt; border: 1pt solid #ccc;">Task 1</td>
        <td style="padding: 5pt; border: 1pt solid #ccc;">
            <strong style="color: green;">Completed</strong>
        </td>
    </tr>
    <tr>
        <td style="padding: 5pt; border: 1pt solid #ccc;">Task 2</td>
        <td style="padding: 5pt; border: 1pt solid #ccc;">
            <strong style="color: orange;">In Progress</strong>
        </td>
    </tr>
    <tr>
        <td style="padding: 5pt; border: 1pt solid #ccc;">Task 3</td>
        <td style="padding: 5pt; border: 1pt solid #ccc;">
            <em style="color: #999;">Pending</em>
        </td>
    </tr>
</table>
```

### With Background Colors

```html
<p>
    The <strong style="background-color: yellow; padding: 2pt;">highlighted term</strong>
    is important.
</p>

<p>
    <strong style="background-color: #336699; color: white; padding: 5pt 10pt;
    border-radius: 4pt;">
        Blue Badge
    </strong>
</p>

<p>
    Status: <em style="background-color: #90EE90; padding: 3pt 8pt;
    border-radius: 3pt;">Active</em>
</p>
```

### Document Headers

```html
<div style="text-align: center; margin-bottom: 30pt;">
    <h1><strong>ANNUAL REPORT</strong></h1>
    <p><i>Fiscal Year 2024</i></p>
    <p><strong>Company Name, Inc.</strong></p>
</div>
```

### Repeating Bold Items with Data Binding

```html
<!-- With model.items = [{name: "Item 1", value: "100"}, {name: "Item 2", value: "200"}] -->
<template data-bind="{{model.items}}">
    <p><strong>{{.name}}:</strong> ${{.value}}</p>
</template>
```

### Combined with Links

```html
<p>
    For more information, visit our
    <a href="https://example.com"><strong>website</strong></a> or
    <a href="mailto:info@example.com"><em>contact us</em></a>.
</p>

<p>
    <a href="#section1" style="text-decoration: none;">
        <strong style="color: #336699;">Section 1: Introduction</strong>
    </a>
</p>
```

### Keyboard Instructions

```html
<p>
    Press <b>Ctrl</b> + <b>C</b> to copy and
    <b>Ctrl</b> + <b>V</b> to paste.
</p>

<p>
    Use the <strong>Tab</strong> key to navigate and
    <strong>Enter</strong> to submit.
</p>
```

### Mixed Semantic and Presentational

```html
<article>
    <h2><strong>Getting Started Guide</strong></h2>
    <p>
        <b>Introduction:</b> This guide will help you get started
        <em>quickly and easily</em>.
    </p>
    <p>
        <strong>Important:</strong> Make sure you have <i>administrator privileges</i>
        before proceeding.
    </p>
</article>
```

---

## See Also

- [span](/reference/htmltags/span.html) - Generic inline container
- [u](/reference/htmltags/u.html) - Underlined text
- [mark](/reference/htmltags/mark.html) - Highlighted/marked text
- [code](/reference/htmltags/code.html) - Code/monospace text
- [small](/reference/htmltags/small.html) - Smaller text
- [del](/reference/htmltags/del.html) - Deleted/strikethrough text
- [ins](/reference/htmltags/ins.html) - Inserted/underlined text
- [CSS Styles](/reference/styles/) - Complete CSS styling reference

---
