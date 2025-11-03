---
layout: default
title: mark, sub, sup
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;mark&gt;, &lt;small&gt;, &lt;sub&gt;, &lt;sup&gt;: Text Modification Elements
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

Specialized inline elements for text formatting including highlighted text, small print, subscripts and superscripts.

- `<mark>`: Highlighted/marked text (yellow background)
- `<small>`: Smaller text (75% size)
- `<sub>`: Subscript text (lowered and smaller)
- `<sup>`: Superscript text (raised and smaller)

---

## Usage


```html
<!-- Highlighted text -->
<p>The <mark>important term</mark> is highlighted.</p>

<!-- Small print -->
<p>Price: $99.99 <small>(taxes not included)</small></p>
```

```html
<!-- Chemical formula -->
<p>Water is H<sub>2</sub>O.</p>

<!-- Mathematical expression -->
<p>E = mc<sup>2</sup></p>
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

## Default Styling

**`<mark>`**:
- Background color: Yellow (mark color)
- Text color: Inherited

**`<small>`**:
- Font size: 75% of parent
- Baseline: Hanging

**`<sub>`**:
- Font size: 75% of parent
- Position: Relative, offset down by 0.5em
- Display: Inline-block

**`<sup>`**:
- Font size: 75% of parent
- Position: Relative, offset to baseline
- Display: Inline-block


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

## Examples

### Marked/Highlighted Text

#### Basic Highlighting

```html
<p>The <mark>important term</mark> is highlighted in yellow.</p>

<p>Please review the <mark>highlighted sections</mark> carefully.</p>

<p>Meeting time: <mark>3:00 PM</mark></p>
```

#### Custom Highlight Colors

```html
<p>
    This is <mark style="background-color: yellow;">yellow highlight</mark> and
    this is <mark style="background-color: lime;">green highlight</mark>.
</p>

<p>
    <mark style="background-color: #ffcccc; color: #800000;">
        Important: Action required
    </mark>
</p>

<p>
    Search results for "test":
    <mark style="background-color: #ffd700; padding: 2pt;">test</mark>
    appears 5 times.
</p>
```

#### Styled Highlights

```html
<style>
    .highlight-warning {
        background-color: #fff3cd;
        border-left: 3pt solid #ffc107;
        padding: 2pt 6pt;
    }
    .highlight-info {
        background-color: #d1ecf1;
        border-left: 3pt solid #17a2b8;
        padding: 2pt 6pt;
    }
</style>

<p>Status: <mark class="highlight-warning">Pending Review</mark></p>

<p>Note: <mark class="highlight-info">Additional information available</mark></p>
```

### Small Text

#### Fine Print

```html
<p>Price: $99.99 <small>(plus applicable taxes)</small></p>

<p>Terms and conditions apply. <small>See website for details.</small></p>

<p>Copyright 2025. <small>All rights reserved.</small></p>
```

#### Footnotes and Annotations

```html
<p>
    The study<small><sup>1</sup></small> shows significant improvements.
</p>
<p><small><sup>1</sup> Published in Nature, 2024</small></p>

<p>
    Available in stores nationwide<small>*</small>
</p>
<p><small>* Excluding Alaska and Hawaii</small></p>
```

#### Styled Small Text

```html
<p style="font-size: 14pt;">
    Product Name
    <small style="color: #666; display: block; margin-top: 5pt;">
        Model: XYZ-123
    </small>
</p>

<div>
    <h3 style="margin-bottom: 5pt;">Article Title</h3>
    <small style="color: #999;">Published on January 15, 2025 by Author Name</small>
</div>
```

### Subscript

#### Chemical Formulas

```html
<p>Water: H<sub>2</sub>O</p>

<p>Carbon dioxide: CO<sub>2</sub></p>

<p>Sulfuric acid: H<sub>2</sub>SO<sub>4</sub></p>

<p>Glucose: C<sub>6</sub>H<sub>12</sub>O<sub>6</sub></p>
```

#### Mathematical Notations

```html
<p>x<sub>1</sub>, x<sub>2</sub>, x<sub>3</sub>, ..., x<sub>n</sub></p>

<p>The sequence a<sub>n</sub> converges to L.</p>

<p>Base<sub>10</sub> logarithm</p>
```

#### Styled Subscript

```html
<p style="font-size: 14pt;">
    Chemical reaction: 2H<sub style="color: blue;">2</sub> +
    O<sub style="color: red;">2</sub> → 2H<sub style="color: blue;">2</sub>O
</p>
```

### Superscript

#### Mathematical Expressions

```html
<p>E = mc<sup>2</sup></p>

<p>2<sup>3</sup> = 8</p>

<p>x<sup>2</sup> + y<sup>2</sup> = r<sup>2</sup></p>

<p>10<sup>6</sup> = 1,000,000</p>
```

#### Ordinal Numbers

```html
<p>The 1<sup>st</sup> place winner</p>

<p>On the 21<sup>st</sup> of December</p>

<p>3<sup>rd</sup> quarter results</p>
```

#### Footnote References

```html
<p>
    According to recent studies<sup>1</sup>, the method is effective.
    Further research<sup>2</sup> confirms these findings.
</p>
<hr />
<p><sup>1</sup> Smith et al., 2024</p>
<p><sup>2</sup> Johnson, 2025</p>
```

#### Styled Superscript

```html
<p>
    Area = πr<sup style="color: red;">2</sup>
</p>

<p>
    Price: $99.99<sup style="font-size: 60%;">*</sup>
</p>
<p><sup style="color: #666;">*</sup> <small>Limited time offer</small></p>
```

### Combined Formatting

#### Complex Mathematical Expression

```html
<p style="font-size: 14pt; text-align: center;">
    <var>a</var><sub>0</sub> + <var>a</var><sub>1</sub><var>x</var> +
    <var>a</var><sub>2</sub><var>x</var><sup>2</sup> + ... +
    <var>a</var><sub>n</sub><var>x</var><sup>n</sup>
</p>
```

#### Technical Documentation

```html
<div style="border: 1pt solid #ddd; padding: 15pt; background-color: #f9f9f9;">
    <p><strong>Function:</strong> <code>calculateInterest(principal, rate, time)</code></p>
    <p><strong>Parameters:</strong></p>
    <ul>
        <li><var>principal</var> - The initial amount (number)</li>
        <li><var>rate</var> - Annual interest rate (number)</li>
        <li><var>time</var> - Time period in years (number)</li>
    </ul>
    <p><strong>Returns:</strong> <samp>number</samp></p>
    <p><strong>Example:</strong></p>
    <p><code>calculateInterest(1000, 0.05, 2)</code></p>
    <p><strong>Output:</strong> <samp>100</samp></p>
</div>
```

#### Product Specifications

```html
<div style="border: 1pt solid #ccc; padding: 15pt;">
    <h3><strong>Product XYZ-2000</strong></h3>
    <p><small>Model: XYZ-2000-B</small></p>
    <p><strong>Price:</strong> <del>$299.99</del>
       <mark style="background-color: #90EE90;">$199.99</mark></p>
    <p><strong>Features:</strong></p>
    <ul>
        <li>Speed: <ins>Up to 1000 MB/s</ins></li>
        <li>Storage: 512 GB</li>
        <li>Weight: 1.5<small>kg</small></li>
    </ul>
    <p><small>* Free shipping on orders over $50</small></p>
</div>
```

#### Scientific Article

```html
<article>
    <h2>Chemical Analysis</h2>
    <p>
        The reaction between H<sub>2</sub>SO<sub>4</sub> and
        NaOH produces Na<sub>2</sub>SO<sub>4</sub> and H<sub>2</sub>O.
    </p>
    <p>
        The concentration was measured as 2.5 × 10<sup>-3</sup> mol/L.
    </p>
    <p>
        <mark>Note:</mark> Temperature must be maintained at
        25<sup>°</sup>C ± 2<sup>°</sup>C.
    </p>
    <p style="margin-top: 15pt;">
        <small>
            <sup>1</sup> Data collected over 3-month period<br/>
            <sup>2</sup> Standard deviation: ±0.05
        </small>
    </p>
</article>
```

#### Code Documentation Example

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

#### Revision History

```html
<table style="width: 100%; border-collapse: collapse;">
    <tr style="background-color: #f0f0f0;">
        <th style="border: 1pt solid #ccc; padding: 5pt;">Version</th>
        <th style="border: 1pt solid #ccc; padding: 5pt;">Changes</th>
        <th style="border: 1pt solid #ccc; padding: 5pt;">Date</th>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 5pt;">1.0</td>
        <td style="border: 1pt solid #ccc; padding: 5pt;">Initial release</td>
        <td style="border: 1pt solid #ccc; padding: 5pt;"><small>2025-01-01</small></td>
    </tr>
    <tr>
        <td style="border: 1pt solid #ccc; padding: 5pt;">1.1</td>
        <td style="border: 1pt solid #ccc; padding: 5pt;">
            <ins>Added new features</ins>, <del>Removed deprecated API</del>
        </td>
        <td style="border: 1pt solid #ccc; padding: 5pt;"><small>2025-01-15</small></td>
    </tr>
</table>
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

- [code, kbd, samp](html_code_kbd_samp_elements.html) - Computer related text elements
- [del, ins, u, s](html_del_ins_u_s_elements.html) - Text edit / change elements
- [strong, b, em, i](html_strong_em_elements.html) - Bold and italic text elements
- [span](html_span_elements.html) - Generic inline container
- [CSS Styles](/learning/styles/) - Complete CSS styling reference
- [Data Binding](/learning/binding/) - Data binding and expressions

---
