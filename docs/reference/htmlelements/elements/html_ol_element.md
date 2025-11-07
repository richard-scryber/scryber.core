---
layout: default
title: ol
parent: HTML Elements
parent_url: /reference/htmltags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;ol&gt; : The Ordered List Element
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

The `<ol>` element represents an ordered list of items, where the sequence matters. Items are automatically numbered using various numbering styles such as decimals (1, 2, 3), letters (a, b, c), or Roman numerals (i, ii, iii). In PDF output, ordered lists maintain precise numbering with support for nested hierarchies and custom formatting.

## Usage

The `<ol>` element creates an ordered list that:
- Automatically numbers items in sequence
- Supports multiple numbering styles (decimals, letters, Roman numerals)
- Contains one or more `<li>` (list item) elements
- Maintains numbering across nested lists with concatenation support
- Can be styled with CSS for custom number formatting
- Supports numbering groups for continuing counts across multiple lists
- Flows naturally across pages and columns when content overflows

```html
<ol>
    <li>First step</li>
    <li>Second step</li>
    <li>Third step</li>
</ol>
```

---

## Supported Attributes

### Standard HTML Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | Unique identifier for the element. Used for styling and internal references. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the element. |
| `title` | string | Sets the outline/bookmark title for the list in the PDF structure. |
| `hidden` | string | Controls visibility. Set to "hidden" to hide the element, or omit/empty to show. |

### Custom Data Attributes (Scryber Extensions)

These attributes provide advanced list control not available in standard HTML:

| Attribute | Type | Description |
|-----------|------|-------------|
| `data-li-group` | string | Assigns the list to a numbering group for maintaining counts across multiple lists. |
| `data-li-concat` | boolean | When true, concatenates nested list numbers with parent list numbers (e.g., "1.a"). Default is true for ordered lists. |
| `data-li-postfix` | string | Text to append after each number (default: "."). Example: ")" for "1)". |
| `data-li-prefix` | string | Text to prepend before each number (e.g., "# " for "# 1."). |
| `data-li-inset` | unit | Width of the number area from the left margin (default: 30pt). |
| `data-li-align` | alignment | Horizontal alignment of the number: `left`, `center`, or `right` (default: right). |
| `data-li-style` | style | Override the numbering style (see list-style-type values). |

### CSS Style Support

The `<ol>` element supports extensive CSS styling through the `style` attribute or CSS classes:

**List-Specific Styles**:
- `list-style-type`: Numbering style - `decimal`, `lower-alpha`, `upper-alpha`, `lower-roman`, `upper-roman`, `none`
- `list-style`: Shorthand for list styling
- `-pdf-li-prefix`: Custom CSS property for number prefix
- `-pdf-li-postfix`: Custom CSS property for number postfix (default: ".")
- `-pdf-li-inset`: Custom CSS property for number area width
- `-pdf-li-align`: Custom CSS property for number alignment
- `-pdf-li-concat`: Custom CSS property for concatenation (true/false/concatenate, default: true)
- `-pdf-li-group`: Custom CSS property for numbering group name

**Box Model**:
- `margin`, `margin-top`, `margin-right`, `margin-bottom`, `margin-left`
- `padding`, `padding-top`, `padding-right`, `padding-bottom`, `padding-left`
- `border`, `border-width`, `border-color`, `border-style`

**Layout & Positioning**:
- `display`: `block`, `none`
- `page-break-before`, `page-break-after`, `page-break-inside`
- `break-before`, `break-after`, `break-inside`
- `column-count`, `column-gap` (for multi-column layouts)

**Visual Styling**:
- `background`, `background-color`, `background-image`
- `color` (inherited by list items and numbers)
- `opacity`

**Typography** (inherited by list items):
- `font-family`, `font-size`, `font-weight`, `font-style`
- `line-height`, `letter-spacing`

---

## Notes

### Default Behavior

The `<ol>` element has the following default behavior:

1. **Decimal Numbers**: Uses decimal numbering (1, 2, 3...) by default
2. **Period Postfix**: Adds a period after each number by default (e.g., "1.")
3. **Concatenation Enabled**: Nested lists concatenate numbers by default (e.g., "1.1", "1.2")
4. **Block Display**: Displays as a block-level element
5. **Indentation**: List items are indented with a 30pt inset for numbers
6. **Right-Aligned Numbers**: Numbers align to the right of the number area by default
7. **Natural Flow**: Content flows across pages and columns when space is limited

### Class Hierarchy

In the Scryber codebase:
- `HTMLListOrdered` extends `ListOrdered` extends `ListBase` extends `Panel`
- Implements standard HTML `<ol>` behavior with PDF enhancements
- Inherits layout and styling capabilities from base classes
- Overrides default styles to add period postfix and enable concatenation

### List Style Types

Supported `list-style-type` values for ordered lists:

- **decimal**: Arabic numerals (1, 2, 3, 4...) - default
- **lower-alpha** or **lower-latin**: Lowercase letters (a, b, c, d...)
- **upper-alpha** or **upper-latin**: Uppercase letters (A, B, C, D...)
- **lower-roman**: Lowercase Roman numerals (i, ii, iii, iv...)
- **upper-roman**: Uppercase Roman numerals (I, II, III, IV...)
- **none**: No numbering displayed

Any other value defaults to decimal numbering.

### Number Positioning

The number area works as follows:
- A number area with width specified by `data-li-inset` (default 30pt) is reserved on the left
- The number (with prefix/postfix) aligns within this area according to `data-li-align` (default: right)
- A 10pt gap (alley) separates the number from the content
- List item content begins after the number area plus the alley

```
|<- number area (inset) ->|<-alley->|<- content area ->
|                      1. |   10pt  | List item text
```

### Nested Lists and Concatenation

Ordered lists support hierarchical numbering:
- By default, nested `<ol>` elements concatenate their numbers with parent numbers
- Use `data-li-concat="false"` to restart numbering independently
- Different numbering styles can be used at each level
- Example: Parent "1." with child "a." produces "1. a."

### Numbering Groups

Numbering groups allow lists to share a counter:
- Assign the same group name to multiple lists using `data-li-group`
- All lists in a group continue the same sequence
- Useful for maintaining numbered sequences across sections
- Group names are case-sensitive

---

## Examples

### Basic Ordered List

```html
<ol>
    <li>First step</li>
    <li>Second step</li>
    <li>Third step</li>
</ol>
<!-- Output: 1. First step, 2. Second step, 3. Third step -->
```

### Lowercase Letters

```html
<ol style="list-style-type: lower-alpha;">
    <li>Option A</li>
    <li>Option B</li>
    <li>Option C</li>
</ol>
<!-- Output: a. Option A, b. Option B, c. Option C -->
```

### Uppercase Roman Numerals

```html
<ol style="list-style-type: upper-roman;">
    <li>Introduction</li>
    <li>Main Content</li>
    <li>Conclusion</li>
</ol>
<!-- Output: I. Introduction, II. Main Content, III. Conclusion -->
```

### Custom Postfix (Parentheses)

```html
<ol data-li-postfix=")">
    <li>Item one</li>
    <li>Item two</li>
    <li>Item three</li>
</ol>
<!-- Output: 1) Item one, 2) Item two, 3) Item three -->
```

### Custom Prefix and Postfix

```html
<style>
    .section-list {
        -pdf-li-prefix: 'Section ';
        -pdf-li-postfix: ':';
    }
</style>

<ol class="section-list">
    <li>Overview</li>
    <li>Details</li>
    <li>Summary</li>
</ol>
<!-- Output: Section 1: Overview, Section 2: Details, Section 3: Summary -->
```

### Nested Ordered Lists with Concatenation

```html
<ol>
    <li>Chapter One
        <ol style="list-style-type: lower-alpha;">
            <li>Section A</li>
            <li>Section B</li>
            <li>Section C</li>
        </ol>
    </li>
    <li>Chapter Two
        <ol style="list-style-type: lower-alpha;">
            <li>Section A</li>
            <li>Section B</li>
        </ol>
    </li>
</ol>
<!-- Output:
1. Chapter One
   1. a. Section A
   1. b. Section B
   1. c. Section C
2. Chapter Two
   2. a. Section A
   2. b. Section B
-->
```

### Nested Lists Without Concatenation

```html
<ol>
    <li>Parent Item
        <ol data-li-concat="false" style="list-style-type: lower-roman;">
            <li>Independent numbering</li>
            <li>Not concatenated</li>
        </ol>
    </li>
</ol>
<!-- Output:
1. Parent Item
   i. Independent numbering
   ii. Not concatenated
-->
```

### Mixed Nested Lists (ol and ul)

```html
<ol>
    <li>Preparation Steps
        <ul style="list-style-type: disc;">
            <li>Gather materials</li>
            <li>Review instructions</li>
            <li>Set up workspace</li>
        </ul>
    </li>
    <li>Implementation
        <ol style="list-style-type: lower-alpha;">
            <li>Start process</li>
            <li>Monitor progress</li>
            <li>Complete task</li>
        </ol>
    </li>
    <li>Review Results</li>
</ol>
```

### Three-Level Nested Hierarchy

```html
<style>
    .level1 {
        list-style-type: decimal;
        -pdf-li-postfix: '.';
    }
    .level2 {
        list-style-type: lower-alpha;
        -pdf-li-prefix: ' ';
        -pdf-li-postfix: '.';
    }
    .level3 {
        list-style-type: lower-roman;
        -pdf-li-prefix: ' ';
        -pdf-li-postfix: '.';
    }
</style>

<ol class="level1">
    <li>First Level
        <ol class="level2">
            <li>Second Level
                <ol class="level3">
                    <li>Third Level Item</li>
                    <li>Third Level Item</li>
                </ol>
            </li>
            <li>Second Level</li>
        </ol>
    </li>
    <li>First Level</li>
</ol>
<!-- Output shows: 1. → 1. a. → 1. a. i. -->
```

### Grouped Lists (Continued Numbering)

```html
<style>
    .continued {
        -pdf-li-group: 'main-sequence';
    }
</style>

<h3>Part One</h3>
<ol class="continued">
    <li>First item</li>
    <li>Second item</li>
</ol>

<p>Some intervening content...</p>

<h3>Part Two</h3>
<ol class="continued">
    <li>Third item (continues from 2)</li>
    <li>Fourth item</li>
</ol>

<!-- Output:
Part One: 1. First item, 2. Second item
Part Two: 3. Third item, 4. Fourth item
-->
```

### Custom Number Alignment and Width

```html
<style>
    .wide-numbers {
        -pdf-li-inset: 60pt;
        -pdf-li-align: left;
        -pdf-li-prefix: 'Step ';
        -pdf-li-postfix: ' - ';
    }
</style>

<ol class="wide-numbers">
    <li>Initialize the system</li>
    <li>Configure settings</li>
    <li>Run diagnostics</li>
</ol>
```

### Multi-Column Ordered List

```html
<style>
    .column-list {
        column-count: 2;
        column-gap: 30pt;
        column-rule: 1pt solid #ddd;
    }
</style>

<ol class="column-list">
    <li>Item 1</li>
    <li>Item 2</li>
    <li>Item 3</li>
    <li>Item 4</li>
    <li>Item 5</li>
    <li>Item 6</li>
    <li>Item 7</li>
    <li>Item 8</li>
</ol>
```

### Data Binding with Dynamic Lists

```html
<!-- Template with model.steps = ["Initialize", "Configure", "Execute"] -->
<ol>
    <li>Procedure Overview ({{count(model.steps)}} steps):</li>
    <template data-bind="{{model.steps}}">
        <li>{{.}}</li>
    </template>
</ol>

<!-- Output:
1. Procedure Overview (3 steps):
2. Initialize
3. Configure
4. Execute
-->
```

### Complex Data Binding Example

```html
<!-- Template with model.tasks = [{title: "Task A", priority: "high"}, ...] -->
<style>
    .priority-high { color: #e74c3c; font-weight: bold; }
    .priority-medium { color: #f39c12; }
    .priority-low { color: #95a5a6; }
</style>

<ol>
    <template data-bind="{{model.tasks}}">
        <li class="priority-{{.priority}}">
            {{.title}} (Priority: {{.priority}})
        </li>
    </template>
</ol>
```

### Styled Ordered List

```html
<style>
    .procedure-list {
        background-color: #e8f5e9;
        border-left: 4pt solid #4caf50;
        padding: 15pt;
        list-style-type: decimal;
        font-size: 12pt;
        line-height: 1.8;
    }
    .procedure-list li {
        margin-bottom: 10pt;
        padding-left: 5pt;
    }
</style>

<ol class="procedure-list">
    <li>Prepare all required materials and tools</li>
    <li>Review safety guidelines and precautions</li>
    <li>Follow the step-by-step instructions carefully</li>
    <li>Verify completion and perform quality checks</li>
</ol>
```

### Preventing List Item Breaks

```html
<style>
    .keep-together > li {
        break-inside: avoid;
    }
</style>

<ol class="keep-together">
    <li>This numbered item with long content will not split across
        pages or columns. It will move as a complete block to the
        next page if needed to maintain readability.</li>
    <li>Each list item stays together as a single unit.</li>
    <li>Particularly useful for instructions or procedures.</li>
</ol>
```

### List with Rich Content

```html
<ol>
    <li>
        <strong style="font-size: 14pt; color: #2c3e50;">Introduction</strong>
        <p style="margin: 5pt 0 10pt 0;">This section provides an overview
        of the topic with detailed explanations and examples.</p>
    </li>
    <li>
        <strong style="font-size: 14pt; color: #2c3e50;">Methodology</strong>
        <table style="width: 100%; margin-top: 5pt;">
            <tr>
                <td style="border: 1pt solid #ccc; padding: 5pt;">Step</td>
                <td style="border: 1pt solid #ccc; padding: 5pt;">Description</td>
            </tr>
            <tr>
                <td style="border: 1pt solid #ccc; padding: 5pt;">1</td>
                <td style="border: 1pt solid #ccc; padding: 5pt;">Data Collection</td>
            </tr>
        </table>
    </li>
    <li>
        <strong style="font-size: 14pt; color: #2c3e50;">Results</strong>
        <ul style="margin-top: 5pt;">
            <li>Finding A</li>
            <li>Finding B</li>
            <li>Finding C</li>
        </ul>
    </li>
</ol>
```

### Legal Document Style Numbering

```html
<style>
    .legal-list {
        -pdf-li-postfix: '.';
        -pdf-li-concat: concatenate;
    }
    .legal-list ol {
        -pdf-li-postfix: '.';
        list-style-type: lower-alpha;
    }
    .legal-list ol ol {
        list-style-type: lower-roman;
    }
</style>

<ol class="legal-list">
    <li>Terms and Conditions
        <ol>
            <li>General Provisions
                <ol>
                    <li>This agreement shall be binding</li>
                    <li>All parties must comply</li>
                </ol>
            </li>
            <li>Specific Terms</li>
        </ol>
    </li>
    <li>Liability Limitations</li>
</ol>
<!-- Output: 1., 1. a., 1. a. i., 1. a. ii., 1. b., 2. -->
```

### Numbered Checklist

```html
<style>
    .checklist li {
        margin-bottom: 12pt;
        padding: 8pt;
        background-color: #f8f9fa;
        border-left: 3pt solid #6c757d;
    }
    .checklist li::before {
        content: '☐ ';
        margin-right: 5pt;
    }
</style>

<ol class="checklist">
    <li>Review project requirements</li>
    <li>Allocate resources and assign tasks</li>
    <li>Set milestones and deadlines</li>
    <li>Monitor progress regularly</li>
    <li>Complete final review and sign-off</li>
</ol>
```

### Instructions with Nested Sub-steps

```html
<ol>
    <li>Installation
        <ol style="list-style-type: lower-alpha;">
            <li>Download the installer</li>
            <li>Run the installation wizard
                <ol style="list-style-type: lower-roman;">
                    <li>Accept license agreement</li>
                    <li>Choose installation directory</li>
                    <li>Select components to install</li>
                    <li>Complete installation</li>
                </ol>
            </li>
            <li>Restart your system</li>
        </ol>
    </li>
    <li>Configuration
        <ol style="list-style-type: lower-alpha;">
            <li>Open settings panel</li>
            <li>Configure preferences</li>
            <li>Save configuration</li>
        </ol>
    </li>
</ol>
```

### Outline-Style Document Structure

```html
<style>
    .outline {
        font-size: 12pt;
        line-height: 1.6;
    }
    .outline > li {
        margin-bottom: 15pt;
        font-weight: bold;
    }
    .outline ol {
        font-weight: normal;
        margin-top: 5pt;
    }
</style>

<ol class="outline">
    <li>Executive Summary
        <ol style="list-style-type: lower-alpha;">
            <li>Project objectives</li>
            <li>Key findings</li>
            <li>Recommendations</li>
        </ol>
    </li>
    <li>Background
        <ol style="list-style-type: lower-alpha;">
            <li>Historical context</li>
            <li>Current situation</li>
        </ol>
    </li>
    <li>Detailed Analysis
        <ol style="list-style-type: lower-alpha;">
            <li>Methodology</li>
            <li>Data collection</li>
            <li>Results interpretation</li>
        </ol>
    </li>
</ol>
```

---

## See Also

- [ul](/reference/htmltags/ul.html) - Unordered list element
- [li](/reference/htmltags/li.html) - List item element
- [dl](/reference/htmltags/dl.html) - Definition list element
- [Lists Reference](/reference/components/lists.html) - Complete lists documentation
- [CSS Styles](/reference/styles/) - Complete CSS styling reference
- [Data Binding](/reference/binding/) - Data binding and expressions

---
