---
layout: default
title: Text Formatting Elements
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# Text Formatting Elements

Specialized inline elements for text formatting including computer code, keyboard input, sample output, variables, highlighted text, small print, subscripts, superscripts, deletions, insertions, and underlines.

## Elements Overview

### Computer-Related Elements
- `<code>`: Inline code or computer text (monospace)
- `<kbd>`: Keyboard input (monospace)
- `<samp>`: Sample program output (monospace)
- `<var>`: Mathematical or programming variable (italic, supports data binding)

### Text Modification Elements
- `<mark>`: Highlighted/marked text (yellow background)
- `<small>`: Smaller text (75% size)
- `<sub>`: Subscript text (lowered and smaller)
- `<sup>`: Superscript text (raised and smaller)

### Edit/Change Indicators
- `<del>`: Deleted text (strikethrough)
- `<ins>`: Inserted text (underlined)
- `<u>`: Underlined text (presentational)
- `<s>`: Strikethrough text (presentational)

---

## Usage

### Code Elements

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

### Highlighting and Size

```html
<!-- Highlighted text -->
<p>The <mark>important term</mark> is highlighted.</p>

<!-- Small print -->
<p>Price: $99.99 <small>(taxes not included)</small></p>
```

### Subscript and Superscript

```html
<!-- Chemical formula -->
<p>Water is H<sub>2</sub>O.</p>

<!-- Mathematical expression -->
<p>E = mc<sup>2</sup></p>
```

### Edit Marks

```html
<!-- Deleted text -->
<p>The price was <del>$100</del> $80.</p>

<!-- Inserted text -->
<p>My favorite color is <ins>blue</ins>.</p>

<!-- Underlined -->
<p>This is <u>underlined text</u>.</p>

<!-- Strikethrough -->
<p>This is <s>no longer valid</s>.</p>
```

---

## Supported Attributes

### Standard Attributes (All Elements)

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the element. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element. |

### Element-Specific Attributes

**`<var>` Element**:
| Attribute | Type | Description |
|-----------|------|-------------|
| `data-id` | string | Identifier for storing variable value in document parameters. |
| `data-value` | object | Value to store (binding only - sets document parameter). |

**`<del>` Element**:
| Attribute | Type | Description |
|-----------|------|-------------|
| `cite` | string | URL or reference explaining the deletion. |
| `datetime` | string | Date/time when the text was deleted. |

**`<ins>` Element**:
| Attribute | Type | Description |
|-----------|------|-------------|
| `cite` | string | URL or reference explaining the insertion. |
| `datetime` | string | Date/time when the text was inserted. |

---

## Default Styling

### Computer Elements

**`<code>`, `<kbd>`, `<samp>`**:
- Font family: Monospace
- All other properties inherited

**`<var>`**:
- Font style: Italic
- All other properties inherited

### Visual Modifications

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

### Text Decorations

**`<del>`, `<s>`, `<strike>`**:
- Text decoration: Strikethrough

**`<ins>`, `<u>`**:
- Text decoration: Underline

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

### Code Examples

#### Inline Code

```html
<p>The <code>getElementById()</code> method returns an element.</p>

<p>Use <code>console.log()</code> to debug your code.</p>

<p>The function <code>calculateTotal(items)</code> returns a number.</p>
```

#### Styled Code

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

#### Code Blocks with Pre-formatted Text

```html
<div style="background-color: #f5f5f5; padding: 10pt; border: 1pt solid #ddd;
     border-radius: 4pt; font-family: monospace;">
    <code>function greet(name) {
    return "Hello, " + name;
}</code>
</div>
```

### Keyboard Input

#### Basic Keyboard Shortcuts

```html
<p>Press <kbd>Enter</kbd> to submit.</p>

<p>Use <kbd>Ctrl</kbd> + <kbd>C</kbd> to copy.</p>

<p>Press <kbd>Alt</kbd> + <kbd>F4</kbd> to close the window.</p>

<p>Type <kbd>Shift</kbd> + <kbd>Tab</kbd> to navigate backwards.</p>
```

#### Styled Keyboard Keys

```html
<style>
    .key {
        display: inline-block;
        padding: 3pt 6pt;
        border: 1pt solid #ccc;
        border-radius: 3pt;
        background-color: #fafafa;
        box-shadow: 0 2pt 0 #ddd;
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

#### Keyboard Navigation Instructions

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

#### Program Output

```html
<p>When you run the program, it displays: <samp>Operation completed successfully</samp></p>

<p>The server responded with: <samp>200 OK</samp></p>

<p>Console output: <samp>Error: File not found</samp></p>
```

#### Styled Sample Output

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

#### Command Line Examples

```html
<p>Run the command: <code>npm install</code></p>
<p>Output: <samp>added 247 packages in 12.3s</samp></p>

<p>Execute: <code>git status</code></p>
<p>Result: <samp>On branch main. Nothing to commit, working tree clean.</samp></p>
```

### Variables

#### Mathematical Variables

```html
<p>If <var>a</var> = 5 and <var>b</var> = 3, then <var>a</var> + <var>b</var> = 8.</p>

<p>Solve for <var>x</var>: <var>x</var><sup>2</sup> + 5<var>x</var> + 6 = 0</p>

<p>The formula is <var>y</var> = <var>m</var><var>x</var> + <var>c</var></p>
```

#### Programming Variables

```html
<p>The variable <var>userName</var> stores the user's name.</p>

<p>Set <var>count</var> to 0 and increment it in the loop.</p>

<p>The function returns the value of <var>result</var>.</p>
```

#### Styled Variables

```html
<style>
    .var-math {
        font-style: italic;
        color: #2E86AB;
        font-family: serif;
    }
</style>

<p>Calculate <var class="var-math">π</var> × <var class="var-math">r</var><sup>2</sup></p>

<p style="font-size: 12pt;">
    <var style="font-size: 14pt; color: #d64545;">velocity</var> =
    <var style="color: #4caf50;">distance</var> ÷
    <var style="color: #2196f3;">time</var>
</p>
```

#### Variable with Data Binding

```html
<!-- With model = { username: "admin", userId: "12345" } -->
<var data-id="currentUser" data-value="{{model.username}}">{{model.username}}</var>

<p>User ID: <var data-id="uid" data-value="{{model.userId}}">{{model.userId}}</var></p>
```

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

### Deleted Text

#### Price Changes

```html
<p>Was: <del>$100.00</del> Now: <strong style="color: red;">$79.99</strong></p>

<p>Original price: <del style="color: #999;">$149.99</del>
   Sale price: $99.99</p>
```

#### Document Revisions

```html
<p>The meeting is scheduled for <del>Tuesday</del> <ins>Wednesday</ins> at 2 PM.</p>

<p>Contact: <del>john@oldcompany.com</del> <ins>john@newcompany.com</ins></p>

<p>Status: <del>Pending</del> <ins>Approved</ins></p>
```

#### With Citation

```html
<p>
    The event will be held <del cite="management-memo-2025" datetime="2025-01-10">
    in the main hall</del> <ins>in the conference center</ins>.
</p>
```

#### Styled Deletions

```html
<style>
    .deleted {
        text-decoration: line-through;
        color: #999;
        background-color: #ffe6e6;
    }
</style>

<p>Price: <del class="deleted">$250</del> <strong>$180</strong></p>

<p>
    <del style="text-decoration: line-through; color: red;">
        Discontinued model
    </del>
</p>
```

### Inserted Text

#### Added Content

```html
<p>My favorite colors are blue, green, and <ins>purple</ins>.</p>

<p>Available in sizes: S, M, L, <ins>XL</ins></p>

<p>Features: Fast, Secure, <ins>and Reliable</ins></p>
```

#### Document Edits

```html
<p>Please <ins>carefully</ins> review the attached documents.</p>

<p>The deadline is <ins>Friday, January 20</ins>.</p>

<p>Send the report to <ins>manager@company.com</ins>.</p>
```

#### With Citation

```html
<p>
    <ins cite="update-notice" datetime="2025-01-15">
        New payment methods now accepted.
    </ins>
</p>
```

#### Styled Insertions

```html
<style>
    .inserted {
        text-decoration: underline;
        background-color: #e6ffe6;
        color: #006600;
    }
</style>

<p>New feature: <ins class="inserted">Dark mode support</ins></p>

<p>
    <ins style="background-color: #d4edda; border-left: 3pt solid #28a745;
    padding: 2pt 6pt;">
        Now available in your region
    </ins>
</p>
```

### Underlined Text

#### Basic Underline

```html
<p>This is <u>underlined text</u>.</p>

<p>Important: <u>Do not remove this label</u>.</p>

<p>The <u>underlined term</u> is defined in the glossary.</p>
```

#### Styled Underlines

```html
<p>
    This has a <u style="text-decoration-color: red;">red underline</u>.
</p>

<p>
    Special term: <u style="text-decoration: underline wavy;
    text-decoration-color: blue;">wavy underline</u>
</p>

<p>
    <u style="text-decoration-thickness: 2pt; text-decoration-color: green;">
        Thick green underline
    </u>
</p>
```

#### Chinese/Japanese Proper Names

```html
<!-- Traditional use of <u> for proper nouns in Chinese text -->
<p>The author <u>李白</u> was a famous poet.</p>
```

### Strikethrough Text

#### Completed Tasks

```html
<ul>
    <li><s>Write documentation</s></li>
    <li><s>Review code</s></li>
    <li>Deploy to production</li>
</ul>
```

#### No Longer Valid

```html
<p>Office hours: <s>9 AM - 5 PM</s> Now 24/7</p>

<p>Old website: <s>www.oldsite.com</s></p>

<p><s>Out of stock</s> Back in stock!</p>
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

- [strong](/reference/htmltags/strong.html) - Bold text (semantic)
- [em](/reference/htmltags/em.html) - Italic text (semantic)
- [b](/reference/htmltags/b.html) - Bold text (presentational)
- [i](/reference/htmltags/i.html) - Italic text (presentational)
- [span](/reference/htmltags/span.html) - Generic inline container
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and expressions

---
