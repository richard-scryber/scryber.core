---
layout: default
title: list-style-type
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# list-style-type : List Marker Type Property

The `list-style-type` property specifies the type of marker (bullet or number) to display for list items. This property provides various built-in marker styles for both ordered and unordered lists, essential for creating properly formatted lists in PDF documents.

## Usage

```css
selector {
    list-style-type: value;
}
```

The list-style-type property accepts keyword values that determine the appearance of list markers.

---

## Supported Values

### Unordered List Markers

- **disc** - Filled circle bullet (default for `<ul>`)
- **circle** - Hollow circle bullet
- **square** - Filled square bullet
- **none** - No marker

### Ordered List Markers

- **decimal** - Decimal numbers (1, 2, 3, ...) (default for `<ol>`)
- **lower-alpha** - Lowercase letters (a, b, c, ...)
- **upper-alpha** - Uppercase letters (A, B, C, ...)
- **lower-roman** - Lowercase Roman numerals (i, ii, iii, iv, ...)
- **upper-roman** - Uppercase Roman numerals (I, II, III, IV, ...)

### Default Values

- **disc** - Default for unordered lists (`<ul>`)
- **decimal** - Default for ordered lists (`<ol>`)

---

## Supported Elements

The `list-style-type` property can be applied to:
- Ordered lists (`<ol>`)
- Unordered lists (`<ul>`)
- Individual list items (`<li>`)

When applied to the list container, all child list items inherit the marker type unless specifically overridden.

---

## Notes

- The marker type is inherited by nested lists unless explicitly overridden
- Setting `list-style-type: none` removes markers entirely, useful for custom-styled lists
- For ordered lists, the numbering starts at 1 by default (can be changed with the `start` attribute)
- Roman numerals continue beyond the basic range (I-X): XI, XII, XIII, etc.
- Alphabetic markers cycle after Z: AA, AB, AC, etc.
- Scryber provides additional custom properties (prefixed with `-pdf-li-`) for advanced list marker customization
- The marker type affects only the visual appearance, not the semantic meaning of the list
- Different marker types can be combined in nested lists for visual hierarchy

---

## Data Binding

The `list-style-type` property supports data binding, enabling dynamic control of list marker types based on data values, user preferences, or document context. This powerful feature allows you to create flexible, customizable documents that adapt their formatting to different scenarios.

### Example 1: User-configurable list markers

```html
<style>
    .user-list {
        list-style-type: {{userPrefs.markerType}};
        font-size: 12pt;
    }
</style>
<body>
    <h3>{{userPrefs.listTitle}}</h3>
    <ol class="user-list">
        <li>First item</li>
        <li>Second item</li>
        <li>Third item</li>
    </ol>
</body>
```

**Data context:**
```json
{
    "userPrefs": {
        "listTitle": "Project Tasks",
        "markerType": "decimal"
    }
}
```

Users can select their preferred numbering format (decimal, alpha, roman) from application settings, which is then applied to all lists in generated documents.

### Example 2: Document type-specific formatting

```html
<style>
    .typed-list {
        list-style-type: {{doc.numberingStyle}};
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{doc.title}}</h2>
    <ol class="typed-list">
        <li>Section content one</li>
        <li>Section content two</li>
        <li>Section content three</li>
    </ol>
</body>
```

**Data context:**
```json
{
    "doc": {
        "title": "Legal Brief",
        "numberingStyle": "upper-roman"
    }
}
```

Different document types automatically get appropriate numbering: upper-roman for legal briefs, decimal for technical documents, lower-alpha for questionnaires, etc.

### Example 3: Multilingual list markers

```html
<style>
    .localized-bullets {
        list-style-type: {{lang.bulletStyle}};
        font-size: 12pt;
    }
</style>
<body>
    <h2>{{lang.heading}}</h2>
    <ul class="localized-bullets">
        <li>{{lang.feature1}}</li>
        <li>{{lang.feature2}}</li>
        <li>{{lang.feature3}}</li>
    </ul>
</body>
```

**Data context:**
```json
{
    "lang": {
        "heading": "Features",
        "bulletStyle": "disc",
        "feature1": "Easy to use",
        "feature2": "Powerful features",
        "feature3": "Great support"
    }
}
```

Different languages or regions can have culturally appropriate marker styles while maintaining consistent document structure.

---

## Examples

### Example 1: Disc bullets (default)

```html
<style>
    .disc-list {
        list-style-type: disc;
        font-size: 12pt;
    }
</style>
<body>
    <ul class="disc-list">
        <li>First item with disc bullet</li>
        <li>Second item with disc bullet</li>
        <li>Third item with disc bullet</li>
    </ul>
</body>
```

### Example 2: Circle bullets

```html
<style>
    .circle-list {
        list-style-type: circle;
        font-size: 12pt;
    }
</style>
<body>
    <ul class="circle-list">
        <li>Item with hollow circle</li>
        <li>Another hollow circle item</li>
        <li>Final hollow circle item</li>
    </ul>
</body>
```

### Example 3: Square bullets

```html
<style>
    .square-list {
        list-style-type: square;
        font-size: 11pt;
        line-height: 1.6;
    }
</style>
<body>
    <ul class="square-list">
        <li>Square bullet point</li>
        <li>Another square bullet</li>
        <li>Final square bullet</li>
    </ul>
</body>
```

### Example 4: Decimal numbering

```html
<style>
    .decimal-list {
        list-style-type: decimal;
        font-size: 12pt;
    }
</style>
<body>
    <ol class="decimal-list">
        <li>First numbered item</li>
        <li>Second numbered item</li>
        <li>Third numbered item</li>
        <li>Fourth numbered item</li>
        <li>Fifth numbered item</li>
    </ol>
</body>
```

### Example 5: Lowercase alphabetic

```html
<style>
    .lower-alpha-list {
        list-style-type: lower-alpha;
        font-size: 12pt;
    }
</style>
<body>
    <ol class="lower-alpha-list">
        <li>Option a</li>
        <li>Option b</li>
        <li>Option c</li>
        <li>Option d</li>
    </ol>
</body>
```

### Example 6: Uppercase alphabetic

```html
<style>
    .upper-alpha-list {
        list-style-type: upper-alpha;
        font-size: 12pt;
        font-weight: bold;
    }
</style>
<body>
    <ol class="upper-alpha-list">
        <li>Section A</li>
        <li>Section B</li>
        <li>Section C</li>
    </ol>
</body>
```

### Example 7: Lowercase Roman numerals

```html
<style>
    .lower-roman-list {
        list-style-type: lower-roman;
        font-size: 11pt;
    }
</style>
<body>
    <ol class="lower-roman-list">
        <li>Introduction</li>
        <li>Background</li>
        <li>Methodology</li>
        <li>Results</li>
        <li>Discussion</li>
    </ol>
</body>
```

### Example 8: Uppercase Roman numerals

```html
<style>
    .upper-roman-list {
        list-style-type: upper-roman;
        font-size: 13pt;
        font-weight: bold;
    }
</style>
<body>
    <ol class="upper-roman-list">
        <li>Chapter I: The Beginning</li>
        <li>Chapter II: The Journey</li>
        <li>Chapter III: The Revelation</li>
        <li>Chapter IV: The End</li>
    </ol>
</body>
```

### Example 9: No markers

```html
<style>
    .no-marker-list {
        list-style-type: none;
        font-size: 11pt;
        padding-left: 10pt;
    }
</style>
<body>
    <ul class="no-marker-list">
        <li>Item without marker</li>
        <li>Another item without marker</li>
        <li>Clean minimal list</li>
    </ul>
</body>
```

### Example 10: Nested lists with different marker types

```html
<style>
    .outer {
        list-style-type: decimal;
        font-size: 12pt;
    }
    .middle {
        list-style-type: lower-alpha;
        font-size: 11pt;
        margin-top: 5pt;
    }
    .inner {
        list-style-type: lower-roman;
        font-size: 10pt;
        margin-top: 5pt;
    }
</style>
<body>
    <ol class="outer">
        <li>First Level
            <ol class="middle">
                <li>Second Level
                    <ol class="inner">
                        <li>Third Level</li>
                        <li>Third Level</li>
                    </ol>
                </li>
                <li>Second Level</li>
            </ol>
        </li>
        <li>First Level</li>
    </ol>
</body>
```

### Example 11: Mixed list types for document structure

```html
<style>
    .main-sections {
        list-style-type: upper-roman;
        font-size: 14pt;
        font-weight: bold;
    }
    .subsections {
        list-style-type: upper-alpha;
        font-size: 12pt;
        font-weight: normal;
        margin-top: 8pt;
    }
    .items {
        list-style-type: decimal;
        font-size: 11pt;
        margin-top: 5pt;
    }
</style>
<body>
    <ol class="main-sections">
        <li>Introduction
            <ol class="subsections">
                <li>Background
                    <ol class="items">
                        <li>Historical context</li>
                        <li>Current state</li>
                    </ol>
                </li>
                <li>Objectives</li>
            </ol>
        </li>
        <li>Main Content</li>
    </ol>
</body>
```

### Example 12: Legal document numbering

```html
<style>
    .legal-main {
        list-style-type: decimal;
        font-size: 12pt;
        font-weight: bold;
    }
    .legal-sub {
        list-style-type: lower-alpha;
        font-size: 11pt;
        font-weight: normal;
        margin-top: 6pt;
    }
    .legal-detail {
        list-style-type: lower-roman;
        font-size: 10pt;
        margin-top: 4pt;
    }
</style>
<body>
    <h2>Terms of Service</h2>
    <ol class="legal-main">
        <li>General Provisions
            <ol class="legal-sub">
                <li>Definitions
                    <ol class="legal-detail">
                        <li>User definition</li>
                        <li>Service definition</li>
                    </ol>
                </li>
                <li>Scope of agreement</li>
            </ol>
        </li>
        <li>User Obligations</li>
    </ol>
</body>
```

### Example 13: Multiple choice exam

```html
<style>
    .question-list {
        list-style-type: decimal;
        font-size: 12pt;
        font-weight: bold;
    }
    .choice-list {
        list-style-type: upper-alpha;
        font-size: 11pt;
        font-weight: normal;
        margin-top: 8pt;
        margin-bottom: 15pt;
    }
</style>
<body>
    <h2>Exam Questions</h2>
    <ol class="question-list">
        <li>What is the capital of France?
            <ol class="choice-list">
                <li>London</li>
                <li>Paris</li>
                <li>Berlin</li>
                <li>Madrid</li>
            </ol>
        </li>
        <li>Which programming language is used for PDF generation?
            <ol class="choice-list">
                <li>Python</li>
                <li>C#</li>
                <li>JavaScript</li>
                <li>All of the above</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 14: Outline format with varied markers

```html
<style>
    .outline-1 {
        list-style-type: upper-roman;
        font-size: 13pt;
        font-weight: bold;
    }
    .outline-2 {
        list-style-type: upper-alpha;
        font-size: 12pt;
        font-weight: 600;
        margin-top: 6pt;
    }
    .outline-3 {
        list-style-type: decimal;
        font-size: 11pt;
        font-weight: normal;
        margin-top: 4pt;
    }
    .outline-4 {
        list-style-type: lower-alpha;
        font-size: 10pt;
        margin-top: 3pt;
    }
</style>
<body>
    <h2>Document Outline</h2>
    <ol class="outline-1">
        <li>Main Topic
            <ol class="outline-2">
                <li>Subtopic
                    <ol class="outline-3">
                        <li>Point
                            <ol class="outline-4">
                                <li>Detail</li>
                                <li>Detail</li>
                            </ol>
                        </li>
                    </ol>
                </li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 15: Feature list with custom styling

```html
<style>
    .feature-category {
        list-style-type: disc;
        font-size: 14pt;
        font-weight: bold;
        color: #2563eb;
    }
    .feature-list {
        list-style-type: circle;
        font-size: 11pt;
        font-weight: normal;
        color: #000;
        margin-top: 6pt;
    }
</style>
<body>
    <h2>Product Features</h2>
    <ul class="feature-category">
        <li>Performance
            <ul class="feature-list">
                <li>Fast rendering engine</li>
                <li>Optimized memory usage</li>
                <li>Efficient processing</li>
            </ul>
        </li>
        <li>Compatibility
            <ul class="feature-list">
                <li>Cross-platform support</li>
                <li>Multiple format support</li>
                <li>Standards compliant</li>
            </ul>
        </li>
    </ul>
</body>
```

---

## See Also

- [list-style](/reference/cssproperties/css_prop_list-style) - List style shorthand
- [-pdf-li-group](/reference/cssproperties/css_prop_-pdf-li-group) - Scryber custom: List number grouping
- [-pdf-li-concat](/reference/cssproperties/css_prop_-pdf-li-concat) - Scryber custom: List number concatenation
- [-pdf-li-align](/reference/cssproperties/css_prop_-pdf-li-align) - Scryber custom: List marker alignment
- [-pdf-li-inset](/reference/cssproperties/css_prop_-pdf-li-inset) - Scryber custom: List content inset
- [-pdf-li-prefix](/reference/cssproperties/css_prop_-pdf-li-prefix) - Scryber custom: List marker prefix
- [-pdf-li-postfix](/reference/cssproperties/css_prop_-pdf-li-postfix) - Scryber custom: List marker postfix
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
