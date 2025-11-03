---
layout: default
title: list-style
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# list-style : List Style Shorthand Property

The `list-style` property is a shorthand for setting all list-related properties in a single declaration. It combines `list-style-type`, `list-style-position`, and `list-style-image` properties to control the appearance and positioning of list item markers.

## Usage

```css
selector {
    list-style: value;
}
```

The list-style property can accept one, two, or three values in any order, representing the type, position, and/or image of the list marker.

---

## Supported Values

### Shorthand Syntax

```css
list-style: <list-style-type> || <list-style-position> || <list-style-image>;
```

### Common Values

- **list-style-type**: `disc`, `circle`, `square`, `decimal`, `lower-alpha`, `upper-alpha`, `lower-roman`, `upper-roman`, `none`
- **list-style-position**: `inside`, `outside` (default)
- **list-style-image**: `url()` or `none` (default)

### Examples of Combined Values

```css
list-style: disc;
list-style: square inside;
list-style: lower-alpha;
list-style: none;
list-style: circle outside;
```

---

## Supported Elements

The `list-style` property can be applied to:
- Ordered lists (`<ol>`)
- Unordered lists (`<ul>`)
- List items (`<li>`)

When applied to `<ul>` or `<ol>`, the style is inherited by all child `<li>` elements unless overridden.

---

## Notes

- The shorthand property sets all three list style properties simultaneously
- Order of values doesn't matter in the shorthand declaration
- Any omitted values are set to their default
- Individual properties can be used for more specific control
- The shorthand is convenient for simple list styling but may be less clear than individual properties
- In Scryber, additional custom properties (prefixed with `-pdf-li-`) provide extended list formatting capabilities for PDF documents

---

## Data Binding

The `list-style` property supports data binding, allowing you to dynamically control list appearance based on data values or application state. This is particularly useful for generating customized reports, multilingual documents, or documents with user-specific formatting preferences.

### Example 1: Dynamic list marker types

```html
<style>
    .dynamic-list {
        list-style: {{listFormat}};
        font-size: 12pt;
    }
</style>
<body>
    <ul class="dynamic-list">
        <li>First item</li>
        <li>Second item</li>
        <li>Third item</li>
    </ul>
</body>
```

**Data context:**
```json
{
    "listFormat": "disc"
}
```

This example allows switching between different list marker types (disc, circle, square, decimal, etc.) based on user preferences or document type.

### Example 2: Conditional formatting for document types

```html
<style>
    .contract-list {
        list-style: {{model.listStyle}};
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{model.documentType}} Items</h2>
    <ol class="contract-list">
        <li>First provision</li>
        <li>Second provision</li>
        <li>Third provision</li>
    </ol>
</body>
```

**Data context:**
```json
{
    "model": {
        "documentType": "Legal Contract",
        "listStyle": "upper-roman"
    }
}
```

Different document types can use different numbering schemes: decimal for technical specs, upper-roman for legal documents, lower-alpha for questionnaires, etc.

### Example 3: Multilingual list formatting

```html
<style>
    .localized-list {
        list-style: {{locale.preferredListStyle}};
        font-size: 12pt;
    }
</style>
<body>
    <h2>{{locale.title}}</h2>
    <ul class="localized-list">
        <li>{{locale.item1}}</li>
        <li>{{locale.item2}}</li>
        <li>{{locale.item3}}</li>
    </ul>
</body>
```

**Data context:**
```json
{
    "locale": {
        "title": "Requirements",
        "preferredListStyle": "circle",
        "item1": "First requirement",
        "item2": "Second requirement",
        "item3": "Third requirement"
    }
}
```

This approach allows different locales to have their preferred list marker styles while maintaining consistent document structure.

---

## Examples

### Example 1: Basic disc bullets

```html
<style>
    .basic-list {
        list-style: disc;
        font-size: 12pt;
    }
</style>
<body>
    <ul class="basic-list">
        <li>First item</li>
        <li>Second item</li>
        <li>Third item</li>
    </ul>
</body>
```

### Example 2: Square bullets with inside positioning

```html
<style>
    .square-list {
        list-style: square inside;
        font-size: 11pt;
        border: 1pt solid #ccc;
        padding: 10pt;
    }
</style>
<body>
    <ul class="square-list">
        <li>Item with square bullet positioned inside</li>
        <li>The bullet is part of the text flow</li>
        <li>Notice the alignment difference</li>
    </ul>
</body>
```

### Example 3: Numbered list with decimal style

```html
<style>
    .numbered-list {
        list-style: decimal;
        font-size: 12pt;
    }
</style>
<body>
    <ol class="numbered-list">
        <li>First step</li>
        <li>Second step</li>
        <li>Third step</li>
    </ol>
</body>
```

### Example 4: No markers

```html
<style>
    .no-markers {
        list-style: none;
        font-size: 11pt;
    }
</style>
<body>
    <ul class="no-markers">
        <li>Item without marker</li>
        <li>Another item without marker</li>
        <li>Clean list without bullets</li>
    </ul>
</body>
```

### Example 5: Lowercase alphabetic list

```html
<style>
    .alpha-list {
        list-style: lower-alpha;
        font-size: 12pt;
        padding-left: 30pt;
    }
</style>
<body>
    <ol class="alpha-list">
        <li>Option A</li>
        <li>Option B</li>
        <li>Option C</li>
        <li>Option D</li>
    </ol>
</body>
```

### Example 6: Uppercase Roman numerals

```html
<style>
    .roman-list {
        list-style: upper-roman;
        font-size: 13pt;
        font-weight: bold;
    }
</style>
<body>
    <ol class="roman-list">
        <li>Introduction</li>
        <li>Methodology</li>
        <li>Results</li>
        <li>Conclusion</li>
    </ol>
</body>
```

### Example 7: Circle bullets

```html
<style>
    .circle-list {
        list-style: circle;
        font-size: 11pt;
        line-height: 1.6;
    }
</style>
<body>
    <ul class="circle-list">
        <li>Circle bullet point</li>
        <li>Another circle bullet</li>
        <li>Final circle bullet</li>
    </ul>
</body>
```

### Example 8: Nested lists with different styles

```html
<style>
    .outer-list {
        list-style: disc;
        font-size: 12pt;
    }
    .inner-list {
        list-style: circle;
        font-size: 11pt;
        margin-top: 5pt;
    }
</style>
<body>
    <ul class="outer-list">
        <li>Main item 1
            <ul class="inner-list">
                <li>Sub-item 1.1</li>
                <li>Sub-item 1.2</li>
            </ul>
        </li>
        <li>Main item 2
            <ul class="inner-list">
                <li>Sub-item 2.1</li>
                <li>Sub-item 2.2</li>
            </ul>
        </li>
    </ul>
</body>
```

### Example 9: Task list without markers

```html
<style>
    .task-list {
        list-style: none;
        font-size: 11pt;
    }
    .task-list li::before {
        content: "☐ ";
        margin-right: 5pt;
    }
</style>
<body>
    <ul class="task-list">
        <li>Review documentation</li>
        <li>Test new features</li>
        <li>Update changelog</li>
    </ul>
</body>
```

### Example 10: Mixed list types in document

```html
<style>
    .requirements {
        list-style: decimal;
        font-size: 12pt;
    }
    .features {
        list-style: disc;
        font-size: 11pt;
        margin-left: 20pt;
    }
</style>
<body>
    <h2>Requirements</h2>
    <ol class="requirements">
        <li>System Requirements
            <ul class="features">
                <li>64-bit processor</li>
                <li>8GB RAM minimum</li>
            </ul>
        </li>
        <li>Software Requirements
            <ul class="features">
                <li>Latest OS version</li>
                <li>Updated drivers</li>
            </ul>
        </li>
    </ol>
</body>
```

### Example 11: Legal document structure

```html
<style>
    .section-list {
        list-style: upper-alpha;
        font-size: 11pt;
        font-weight: bold;
    }
    .subsection-list {
        list-style: decimal;
        font-size: 10pt;
        font-weight: normal;
        margin-top: 5pt;
    }
</style>
<body>
    <ol class="section-list">
        <li>Terms and Conditions
            <ol class="subsection-list">
                <li>General provisions</li>
                <li>Specific terms</li>
            </ol>
        </li>
        <li>Privacy Policy
            <ol class="subsection-list">
                <li>Data collection</li>
                <li>Data usage</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 12: Navigation menu without markers

```html
<style>
    .nav-menu {
        list-style: none;
        font-size: 12pt;
        padding: 0;
    }
    .nav-menu li {
        padding: 8pt;
        border-bottom: 1pt solid #eee;
    }
</style>
<body>
    <ul class="nav-menu">
        <li>Home</li>
        <li>Products</li>
        <li>Services</li>
        <li>Contact</li>
    </ul>
</body>
```

### Example 13: Procedure steps with lowercase Roman numerals

```html
<style>
    .procedure {
        list-style: lower-roman;
        font-size: 11pt;
        line-height: 1.8;
    }
</style>
<body>
    <h3>Installation Procedure</h3>
    <ol class="procedure">
        <li>Download the installation package</li>
        <li>Extract files to destination folder</li>
        <li>Run setup wizard</li>
        <li>Configure initial settings</li>
        <li>Complete installation</li>
    </ol>
</body>
```

### Example 14: FAQ with custom numbering

```html
<style>
    .faq-list {
        list-style: decimal;
        font-size: 12pt;
    }
    .faq-list li {
        margin-bottom: 15pt;
        font-weight: bold;
    }
    .answer {
        font-weight: normal;
        margin-top: 5pt;
        color: #555;
    }
</style>
<body>
    <h2>Frequently Asked Questions</h2>
    <ol class="faq-list">
        <li>What is Scryber?
            <div class="answer">Scryber is a PDF generation library for .NET</div>
        </li>
        <li>How do I install it?
            <div class="answer">Use NuGet package manager to install</div>
        </li>
        <li>Is it open source?
            <div class="answer">Yes, Scryber is open source</div>
        </li>
    </ol>
</body>
```

### Example 15: Table of contents style

```html
<style>
    .toc {
        list-style: none;
        font-size: 11pt;
        padding-left: 0;
    }
    .toc li {
        padding: 6pt 0;
        border-bottom: 1pt dotted #ccc;
    }
    .chapter-num {
        font-weight: bold;
        margin-right: 10pt;
    }
</style>
<body>
    <h2>Table of Contents</h2>
    <ul class="toc">
        <li><span class="chapter-num">1.</span> Introduction</li>
        <li><span class="chapter-num">2.</span> Getting Started</li>
        <li><span class="chapter-num">3.</span> Advanced Topics</li>
        <li><span class="chapter-num">4.</span> Reference</li>
    </ul>
</body>
```

---

## See Also

- [list-style-type](/reference/cssproperties/css_prop_list-style-type) - Type of list marker
- [-pdf-li-group](/reference/cssproperties/css_prop_-pdf-li-group) - Scryber custom: List number grouping
- [-pdf-li-concat](/reference/cssproperties/css_prop_-pdf-li-concat) - Scryber custom: List number concatenation
- [-pdf-li-align](/reference/cssproperties/css_prop_-pdf-li-align) - Scryber custom: List marker alignment
- [-pdf-li-inset](/reference/cssproperties/css_prop_-pdf-li-inset) - Scryber custom: List content inset
- [-pdf-li-prefix](/reference/cssproperties/css_prop_-pdf-li-prefix) - Scryber custom: List marker prefix
- [-pdf-li-postfix](/reference/cssproperties/css_prop_-pdf-li-postfix) - Scryber custom: List marker postfix
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
