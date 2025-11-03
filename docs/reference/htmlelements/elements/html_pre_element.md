---
layout: default
title: pre
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;pre&gt; : The Preformatted Text Element
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

The `<pre>` element represents preformatted text that should be displayed exactly as written in the source code. It is a block-level element designed for displaying content where whitespace, line breaks, and formatting are significant, such as code listings, ASCII art, or formatted data.

## Usage

The `<pre>` element creates a preformatted text container that:
- Preserves all whitespace, including spaces, tabs, and line breaks
- Uses a monospace font (Courier) by default for consistent character spacing
- Prevents text wrapping (text extends beyond boundaries if needed)
- Displays as a block-level element taking full width
- Has a default font size of 10pt
- Ideal for code snippets, command-line output, configuration files, and ASCII art
- Supports all CSS styling properties for fonts, colors, borders, and backgrounds
- Can generate PDF bookmarks/outlines when a `title` attribute is set

```html
<pre>
function greet(name) {
    console.log("Hello, " + name);
}
</pre>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title in the PDF document. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Data Binding Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-bind` | expression | Binds the element to a data context for use with templates. |
| `data-content` | expression | Dynamically sets the content of the pre element from bound data. |

### CSS Style Support

The `<pre>` element supports extensive CSS styling through the `style` attribute or CSS classes:

**Box Model**:
- `width`, `height`, `min-width`, `max-width`, `min-height`, `max-height`
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`
- `border-top`, `border-right`, `border-bottom`, `border-left`

**Positioning**:
- `position`: `static`, `relative`, `absolute`
- `display`: `block`, `inline`, `inline-block`, `none`

**Layout**:
- `overflow`: `visible`, `hidden`, `clip`
- `page-break-before`, `page-break-after`, `page-break-inside`

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `color` (text color)
- `opacity`

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `line-height`, `letter-spacing`
- `text-align` (though typically left-aligned for code)

---

## Notes

### Default Styling

The `<pre>` element has specialized default styling for displaying preformatted text:

**HTML pre (Scryber.Html.Components.HTMLPreformatted)**:
- **Full Width**: Takes 100% of parent container width
- **Display**: Block

**Base Preformatted (Scryber.Components.Preformatted)**:
- **Font Family**: Courier (monospace)
- **Font Size**: 10pt
- **Text Wrapping**: NoWrap (text does not wrap to next line)
- **Whitespace Preservation**: true (preserves spaces, tabs, and line breaks)
- **Position Mode**: Static
- **Display**: Block

These defaults ensure that code and formatted text appear exactly as written in the source.

### Whitespace Preservation

The key characteristic of the `<pre>` element is whitespace preservation:

1. **Spaces**: Multiple consecutive spaces are preserved (not collapsed to single space)
2. **Tabs**: Tab characters are preserved and rendered
3. **Line Breaks**: Line breaks in the source create line breaks in the output
4. **Indentation**: Indentation is maintained exactly as written

### No Text Wrapping

By default, text in `<pre>` elements does not wrap:
- Long lines extend beyond the container boundaries
- Use `overflow` CSS properties to control behavior with long lines
- Consider setting `white-space: pre-wrap` if wrapping is desired

### Monospace Font

The default Courier font ensures:
- All characters have the same width
- Proper alignment for columnar data
- Consistent spacing for code indentation
- Better readability for code snippets

### Escaping HTML

When displaying HTML code within `<pre>` elements, remember to escape special characters:
- Use `&lt;` for `<`
- Use `&gt;` for `>`
- Use `&amp;` for `&`

### Class Hierarchy

In the Scryber codebase:
- `HTMLPreformatted` extends `Preformatted` extends `Panel` extends `VisualComponent`
- The base `Preformatted` class handles whitespace preservation and monospace font
- The HTML version adds full-width sizing by default

---

## Examples

### Basic Code Block

```html
<pre>
function calculateTotal(items) {
    let total = 0;
    for (let item of items) {
        total += item.price;
    }
    return total;
}
</pre>
```

### Code Block with Styling

```html
<pre style="background-color: #f4f4f4; padding: 15pt;
            border: 1pt solid #ddd; border-radius: 5pt;
            font-size: 9pt; line-height: 1.4;">
public class Calculator {
    public int Add(int a, int b) {
        return a + b;
    }
}
</pre>
```

### Code Block with Line Numbers Effect

```html
<pre style="background-color: #282c34; color: #abb2bf;
            padding: 15pt; border-left: 4pt solid #61afef;
            font-size: 9pt;">
 1  def fibonacci(n):
 2      if n &lt;= 1:
 3          return n
 4      else:
 5          return fibonacci(n-1) + fibonacci(n-2)
 6
 7  print(fibonacci(10))
</pre>
```

### Command Line Output

```html
<pre style="background-color: #000; color: #0f0;
            padding: 10pt; font-family: 'Courier New';
            font-size: 9pt;">
$ npm install scryber-pdf
npm notice created a lockfile as package-lock.json
npm WARN package-name@1.0.0 No description
+ scryber-pdf@1.2.3
added 15 packages in 4.829s
</pre>
```

### Configuration File

```html
<pre style="background-color: #fffbf0; padding: 15pt;
            border: 1pt solid #e6d5b8; font-size: 9pt;">
{
  "name": "my-application",
  "version": "1.0.0",
  "dependencies": {
    "express": "^4.18.0",
    "scryber": "^6.0.0"
  },
  "scripts": {
    "start": "node server.js"
  }
}
</pre>
```

### SQL Query

```html
<pre style="background-color: #f8f8f8; padding: 15pt;
            border-left: 3pt solid #ff6b6b; font-size: 9pt;">
SELECT
    customers.name,
    orders.order_date,
    SUM(order_items.quantity * order_items.price) AS total
FROM customers
INNER JOIN orders ON customers.id = orders.customer_id
INNER JOIN order_items ON orders.id = order_items.order_id
WHERE orders.order_date &gt;= '2024-01-01'
GROUP BY customers.name, orders.order_date
ORDER BY total DESC;
</pre>
```

### ASCII Art

```html
<pre style="text-align: center; font-size: 8pt; line-height: 1;">
    ___________
   /           \
  /   O     O   \
 |      ___      |
  \    \___/    /
   \___________/

   Have a nice day!
</pre>
```

### Code with Syntax Highlighting (Using Color)

```html
<pre style="background-color: #f5f5f5; padding: 15pt;
            border: 1pt solid #ccc; font-size: 9pt; line-height: 1.5;">
<span style="color: #0000ff; font-weight: bold;">public</span> <span style="color: #0000ff; font-weight: bold;">class</span> <span style="color: #2b91af;">Program</span>
{
    <span style="color: #0000ff; font-weight: bold;">static</span> <span style="color: #0000ff; font-weight: bold;">void</span> Main(<span style="color: #0000ff; font-weight: bold;">string</span>[] args)
    {
        <span style="color: #2b91af;">Console</span>.WriteLine(<span style="color: #a31515;">"Hello, World!"</span>);
    }
}
</pre>
```

### Tabular Data

```html
<pre style="font-size: 9pt; background-color: #f9f9f9; padding: 15pt;">
Name            Age    City            Status
---------------------------------------------------
John Doe        32     New York        Active
Jane Smith      28     Los Angeles     Active
Bob Johnson     45     Chicago         Inactive
Alice Williams  35     Houston         Active
</pre>
```

### Log File Output

```html
<pre style="background-color: #1e1e1e; color: #d4d4d4;
            padding: 15pt; font-size: 8pt; max-height: 150pt;
            overflow: hidden;">
[2025-10-13 10:15:23] INFO: Application started
[2025-10-13 10:15:24] INFO: Database connection established
[2025-10-13 10:15:25] INFO: Loading configuration...
[2025-10-13 10:15:26] INFO: Configuration loaded successfully
[2025-10-13 10:15:27] INFO: Starting web server on port 3000
[2025-10-13 10:15:28] INFO: Server is ready to accept connections
[2025-10-13 10:16:15] WARN: High memory usage detected (85%)
[2025-10-13 10:17:42] ERROR: Database query timeout after 30s
</pre>
```

### Email Header Format

```html
<pre style="background-color: #f0f0f0; padding: 10pt;
            border: 1pt solid #ccc; font-size: 9pt;">
From: sender@example.com
To: recipient@example.com
Date: Mon, 13 Oct 2025 10:30:00 -0500
Subject: Important Update
Message-ID: &lt;abc123@example.com&gt;
Content-Type: text/plain; charset=UTF-8
</pre>
```

### Mathematical Notation

```html
<pre style="font-family: 'Courier New'; font-size: 10pt;
            padding: 15pt; background-color: #fff9e6;
            border: 1pt solid #e6d5b8; text-align: center;">
    a² + b² = c²

    ∫₀^∞ e⁻ˣ² dx = √π/2

    Σ(i=1 to n) i = n(n+1)/2
</pre>
```

### Code with Title and Caption

```html
<div style="margin: 15pt 0;">
    <div style="font-weight: bold; margin-bottom: 5pt; font-size: 11pt;">
        Listing 1: Fibonacci Implementation
    </div>
    <pre style="background-color: #f4f4f4; padding: 15pt;
                border-left: 4pt solid #336699; font-size: 9pt;">
def fibonacci(n):
    if n &lt;= 1:
        return n
    return fibonacci(n-1) + fibonacci(n-2)
    </pre>
    <div style="font-style: italic; font-size: 9pt; color: #666;
                margin-top: 5pt;">
        A recursive implementation of the Fibonacci sequence
    </div>
</div>
```

### Data Binding with Code

```html
<!-- With model.codeSnippets = [
    { title: "Example 1", code: "function hello() {\n  return 'world';\n}", language: "javascript" },
    { title: "Example 2", code: "def greet():\n  return 'hello'", language: "python" }
] -->

<template data-bind="{{model.codeSnippets}}">
    <div style="margin-bottom: 20pt;">
        <h3>{{.title}} ({{.language}})</h3>
        <pre style="background-color: #f5f5f5; padding: 15pt;
                    border: 1pt solid #ddd; font-size: 9pt;">{{.code}}</pre>
    </div>
</template>
```

### XML/HTML Code Display

```html
<pre style="background-color: #f8f8f8; padding: 15pt;
            border: 1pt solid #e0e0e0; font-size: 9pt; line-height: 1.4;">
&lt;?xml version="1.0" encoding="UTF-8"?&gt;
&lt;document&gt;
    &lt;header&gt;
        &lt;title&gt;Sample Document&lt;/title&gt;
        &lt;author&gt;John Doe&lt;/author&gt;
    &lt;/header&gt;
    &lt;body&gt;
        &lt;paragraph&gt;Content goes here&lt;/paragraph&gt;
    &lt;/body&gt;
&lt;/document&gt;
</pre>
```

### Diff/Patch Format

```html
<pre style="background-color: #f0f0f0; padding: 15pt;
            font-size: 9pt; line-height: 1.3;">
<span style="color: #666;">--- original.js    2025-10-12 10:00:00</span>
<span style="color: #666;">+++ modified.js    2025-10-13 14:30:00</span>
<span style="color: #00a;">@@ -1,4 +1,5 @@</span>
 function calculate(x, y) {
<span style="background-color: #fdd; color: #d00;">-    return x + y;</span>
<span style="background-color: #dfd; color: #090;">+    // Added validation</span>
<span style="background-color: #dfd; color: #090;">+    return (x || 0) + (y || 0);</span>
 }
</pre>
```

### Code Block in Multi-Column Layout

```html
<div style="column-count: 2; column-gap: 20pt;">
    <p>
        The following code demonstrates the implementation:
    </p>

    <pre style="break-inside: avoid; background-color: #f5f5f5;
                padding: 10pt; font-size: 8pt; margin: 10pt 0;">
function add(a, b) {
    return a + b;
}
    </pre>

    <p>
        This function accepts two parameters and returns their sum.
    </p>
</div>
```

### CSV/TSV Data Format

```html
<pre style="background-color: #ffffff; padding: 15pt;
            border: 1pt solid #ccc; font-size: 9pt;">
ID,Name,Email,Status,Date
001,John Doe,john@example.com,Active,2025-10-01
002,Jane Smith,jane@example.com,Active,2025-10-05
003,Bob Johnson,bob@example.com,Inactive,2025-09-28
004,Alice Williams,alice@example.com,Active,2025-10-10
</pre>
```

---

## See Also

- [code](/reference/htmltags/code.html) - Inline code element for short code fragments
- [samp](/reference/htmltags/samp.html) - Sample output element
- [kbd](/reference/htmltags/kbd.html) - Keyboard input element
- [var](/reference/htmltags/var.html) - Variable element for mathematical or programming variables
- [div](/reference/htmltags/div.html) - Generic block container
- [Panel Component](/reference/components/panel.html) - Base panel component in Scryber namespace

---
