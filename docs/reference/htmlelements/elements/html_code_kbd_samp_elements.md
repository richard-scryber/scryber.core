---
layout: default
title: code, kbd, samp
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;code&gt;, &lt;kbd&gt;, &lt;samp&gt; : Computer Related Formatting Elements
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

Specialized inline elements for text formatting computer code, keyboard input and sample output

- `<code>`: Inline code or computer text (monospace)
- `<kbd>`: Keyboard input (monospace)
- `<samp>`: Sample program output (monospace)


---

## Usage

```html
<!-- Inline code -->
<p>Use the <code>print()</code> function to output text.</p>

<!-- Keyboard input -->
<p>Press <kbd>Ctrl</kbd> + <kbd>C</kbd> to copy.</p>

<!-- Sample output -->
<p>The program outputs: <samp>Hello, World!</samp></p>

<!-- Variable -->
<p>Calculate the value of <var>x</var> when <var>y</var> = 10.</p>
```

---

## Supported Attributes


| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the element. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element. |


---


## CSS Style Support

All elements support standard CSS properties:

**Typography**:
- `font-family`, `font-size`, `font-weight`, `font-style`
- `color`, `text-decoration`, `text-transform`
- `letter-spacing`, `word-spacing`

**Background and Borders**:
- `background-color`, `background-image`
- `border`, `border-radius`, `padding`

**Visual Effects**:
- `opacity`, `text-shadow`, `transform`

**Display**:
- `display`: `inline` (default), `inline-block`, `block`, `none`
- `vertical-align`

---

## Class Hierarchy

```c#
Scryber.Html.Components.HTMLCodeSpan, Scryber.Components
Scryber.Html.Components.HTMLKeyboardSpan, Scryber.Components
Scryber.Html.Components.HTMLSampleSpan, Scryber.Components
```

In the library codebase:
- `HTMLCodeSpan`, `HTMLKeyboardSpan` and `HTMLSampleSpan` all extend `HTMLSpan` that extends `SpanBase` extends `VisualComponent`
- Inherits inline display behavior from `SpanBase`
- Supports nested content through `Contents` collection

---

## Notes

## Default Styling

The elements are by default output in monospace font

- Font family: Monospace
- All other properties inherited

---


## Examples


### Inline Code

```html
<p>The <code>getElementById()</code> method returns an element.</p>

<p>Use <code>console.log()</code> to debug your code.</p>

<p>The function <code>calculateTotal(items)</code> returns a number.</p>
```

### Styled Code

```html
<style>
    .code-highlight {
        background-color: #f5f5f5;
        padding: 2pt 4pt;
        border: 1pt solid #ddd;
        border-radius: 3pt;
        color: #c7254e;
    }
</style>

<p>Call the <code class="code-highlight">render()</code> method.</p>

<p>The <code style="background-color: #272822; color: #f8f8f2;
   padding: 3pt 6pt; border-radius: 3pt;">async/await</code> syntax simplifies promises.</p>
```

### Code Blocks with Pre-formatted Text

```html
<div style="background-color: #f5f5f5; padding: 10pt; border: 1pt solid #ddd;
     border-radius: 4pt; font-family: monospace;">
    <code>function greet(name) {
    return "Hello, " + name;
}</code>
</div>
```

### Basic Keyboard Shortcuts

```html
<p>Press <kbd>Enter</kbd> to submit.</p>

<p>Use <kbd>Ctrl</kbd> + <kbd>C</kbd> to copy.</p>

<p>Press <kbd>Alt</kbd> + <kbd>F4</kbd> to close the window.</p>

<p>Type <kbd>Shift</kbd> + <kbd>Tab</kbd> to navigate backwards.</p>
```

### Styled Keyboard Keys

```html
<style>
    .key {
        display: inline-block;
        padding: 3pt 6pt;
        border: 1pt solid #ccc;
        border-radius: 3pt;
        background-color: #fafafa;
        font-family: monospace;
        font-size: 10pt;
    }
</style>

<p>Press <kbd class="key">Ctrl</kbd> + <kbd class="key">S</kbd> to save.</p>

<p>
    <kbd style="background-color: #4CAF50; color: white; padding: 4pt 8pt;
    border-radius: 4pt; font-weight: bold;">Enter</kbd> to continue
</p>
```

### Keyboard Navigation Instructions

```html
<div style="border: 1pt solid #ddd; padding: 15pt; background-color: #f9f9f9;">
    <p><strong>Keyboard Shortcuts:</strong></p>
    <ul>
        <li><kbd>F1</kbd> - Help</li>
        <li><kbd>Ctrl</kbd> + <kbd>N</kbd> - New Document</li>
        <li><kbd>Ctrl</kbd> + <kbd>O</kbd> - Open File</li>
        <li><kbd>Ctrl</kbd> + <kbd>S</kbd> - Save</li>
        <li><kbd>Ctrl</kbd> + <kbd>Z</kbd> - Undo</li>
    </ul>
</div>
```

### Sample Output

```html
<p>When you run the program, it displays: <samp>Operation completed successfully</samp></p>

<p>The server responded with: <samp>200 OK</samp></p>

<p>Console output: <samp>Error: File not found</samp></p>
```

### Styled Sample Output

```html
<style>
    .output {
        background-color: #000;
        color: #0f0;
        padding: 2pt 6pt;
        font-family: monospace;
    }
    .error-output {
        background-color: #fff5f5;
        color: #d00;
        padding: 2pt 6pt;
        font-family: monospace;
        border-left: 3pt solid #d00;
    }
</style>

<p>Terminal output: <samp class="output">Build successful</samp></p>

<p>Error message: <samp class="error-output">Connection timeout</samp></p>
```

### Command Line Examples

```html
<p>Run the command: <code>npm install</code></p>
<p>Output: <samp>added 247 packages in 12.3s</samp></p>

<p>Execute: <code>git status</code></p>
<p>Result: <samp>On branch main. Nothing to commit, working tree clean.</samp></p>
```


### Code Documentation Example

```html
<div style="font-family: 'Courier New', monospace; font-size: 10pt;">
    <p><code>function <var>calculateArea</var>(radius) {</code></p>
    <p style="margin-left: 20pt;">
        <code>return Math.PI * radius<sup>2</sup>;</code>
    </p>
    <p><code>}</code></p>
    <p style="margin-top: 10pt;">
        <small>// Usage example</small>
    </p>
    <p><code>const area = <var>calculateArea</var>(5);</code></p>
    <p><code>console.log(area); <samp>// Output: 78.53981633974483</samp></code></p>
</div>
```


#### Keyboard + Code Example

```html
<div style="border: 1pt solid #ddd; padding: 15pt; background-color: #fafafa;">
    <p><strong>Quick Reference:</strong></p>
    <p>To open developer tools, press <kbd>F12</kbd> or <kbd>Ctrl</kbd> +
       <kbd>Shift</kbd> + <kbd>I</kbd></p>
    <p>In the console, type: <code>document.getElementById('myElement')</code></p>
    <p>Expected output: <samp>&lt;div id="myElement"&gt;...&lt;/div&gt;</samp></p>
</div>
```

---

## See Also

- [del, ins, u, s](html_del_ins_u_s_elements.html) - Text change formatting elements
- [mark, sub, sup](html_mark_sub_sup_elements.html) - Text modification elements
- [strong, b, em, i](html_strong_em_elements.html) - Bold and italic text elements
- [span](html_span_elements.html) - Generic inline container
- [CSS Styles](/learning/styles/) - Complete CSS styling reference
- [Data Binding](/learning/binding/) - Data binding and expressions

---
