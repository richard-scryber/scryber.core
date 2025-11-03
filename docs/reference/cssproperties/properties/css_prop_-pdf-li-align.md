---
layout: default
title: -pdf-li-align
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# -pdf-li-align : List Item Alignment Property (Scryber Custom)

The `-pdf-li-align` property is a Scryber-specific extension that controls the horizontal alignment of list item markers relative to the list content. This property provides precise control over how bullets, numbers, and custom markers are positioned, enabling professional document layouts with properly aligned list structures.

## Usage

```css
selector {
    -pdf-li-align: value;
}
```

This property determines how list markers are aligned relative to the list item content.

---

## Supported Values

- **left** - Align marker to the left (default)
- **right** - Align marker to the right of the marker space
- **center** - Center marker within the marker space

### Default Value

- **left** - Markers are left-aligned

---

## Supported Elements

The `-pdf-li-align` property can be applied to:
- Ordered lists (`<ol>`)
- Unordered lists (`<ul>`)
- Individual list items (`<li>`)

When applied to list containers, the alignment is inherited by all child list items.

---

## Notes

- This is a Scryber-specific property designed for advanced PDF list formatting
- The property affects the visual alignment of markers within their allocated space
- Right alignment is particularly useful for decimal numbers of varying lengths (1, 10, 100)
- Center alignment works well for single-character markers or symbols
- The alignment occurs within the marker space, not the entire list area
- This property complements standard CSS list properties
- Works with all list-style-type values (bullets, numbers, letters, roman numerals)
- Proper alignment improves readability, especially in lists with mixed marker widths
- Can be combined with `-pdf-li-inset` to fine-tune list layout

---

## Data Binding

The `-pdf-li-align` property supports data binding, allowing dynamic control of list marker alignment based on user preferences, document type, or organizational standards. This enables creating documents with consistent, configurable formatting that adapts to different presentation requirements.

### Example 1: User-configurable marker alignment

```html
<style>
    .aligned-list {
        list-style-type: decimal;
        -pdf-li-align: {{userPrefs.markerAlignment}};
        font-size: 11pt;
    }
</style>
<body>
    <h3>{{listTitle}}</h3>
    <ol class="aligned-list">
        <li>{{items[0]}}</li>
        <li>{{items[1]}}</li>
        <li>{{items[2]}}</li>
        <li>{{items[3]}}</li>
        <li>{{items[4]}}</li>
        <li>{{items[5]}}</li>
        <li>{{items[6]}}</li>
        <li>{{items[7]}}</li>
        <li>{{items[8]}}</li>
        <li>{{items[9]}}</li>
    </ol>
</body>
```

**Data context:**
```json
{
    "listTitle": "Project Tasks",
    "userPrefs": {
        "markerAlignment": "right"
    },
    "items": [
        "Item 1", "Item 2", "Item 3", "Item 4", "Item 5",
        "Item 6", "Item 7", "Item 8", "Item 9", "Item 10"
    ]
}
```

For lists with varying marker widths (1-10, 1-100), right alignment ensures visual consistency. Users can configure this based on their aesthetic preferences.

### Example 2: Document type-specific alignment

```html
<style>
    .doc-list {
        list-style-type: {{docStyle.numberingType}};
        -pdf-li-align: {{docStyle.alignment}};
        font-size: 12pt;
    }
</style>
<body>
    <h2>{{document.title}}</h2>
    <ol class="doc-list">
        <li>{{sections[0]}}</li>
        <li>{{sections[1]}}</li>
        <li>{{sections[2]}}</li>
    </ol>
</body>
```

**Data context:**
```json
{
    "document": {
        "title": "Annual Report 2024"
    },
    "docStyle": {
        "numberingType": "upper-roman",
        "alignment": "center"
    },
    "sections": [
        "Executive Summary",
        "Financial Performance",
        "Future Outlook"
    ]
}
```

Different document types can have predefined alignment preferences: center for formal reports, right for technical documents, left for informal lists.

### Example 3: Multilingual document formatting

```html
<style>
    .localized-list {
        list-style-type: {{locale.listType}};
        -pdf-li-align: {{locale.markerAlign}};
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{locale.heading}}</h2>
    <ul class="localized-list">
        <li>{{locale.items[0]}}</li>
        <li>{{locale.items[1]}}</li>
        <li>{{locale.items[2]}}</li>
    </ul>
</body>
```

**Data context:**
```json
{
    "locale": {
        "heading": "Key Features",
        "listType": "disc",
        "markerAlign": "left",
        "items": [
            "High performance",
            "Easy integration",
            "Comprehensive documentation"
        ]
    }
}
```

Different locales or cultures may have preferences for marker alignment. This approach ensures documents respect local formatting conventions while maintaining structure.

---

## Examples

### Example 1: Left-aligned markers (default)

```html
<style>
    .left-aligned {
        list-style-type: decimal;
        -pdf-li-align: left;
        font-size: 12pt;
    }
</style>
<body>
    <ol class="left-aligned">
        <li>First item</li>
        <li>Second item</li>
        <li>Third item</li>
    </ol>
</body>
```

### Example 2: Right-aligned decimal numbers

```html
<style>
    .right-aligned-numbers {
        list-style-type: decimal;
        -pdf-li-align: right;
        font-size: 11pt;
    }
</style>
<body>
    <ol class="right-aligned-numbers">
        <li>Item 1</li>
        <li>Item 2</li>
        <li>Item 3</li>
        <li>Item 4</li>
        <li>Item 5</li>
        <li>Item 6</li>
        <li>Item 7</li>
        <li>Item 8</li>
        <li>Item 9</li>
        <li>Item 10</li>
    </ol>
</body>
```

### Example 3: Center-aligned bullet markers

```html
<style>
    .centered-bullets {
        list-style-type: disc;
        -pdf-li-align: center;
        font-size: 11pt;
    }
</style>
<body>
    <ul class="centered-bullets">
        <li>Centered bullet point</li>
        <li>Another centered bullet</li>
        <li>Final centered bullet</li>
    </ul>
</body>
```

### Example 4: Right-aligned numbers for long lists

```html
<style>
    .long-list {
        list-style-type: decimal;
        -pdf-li-align: right;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
    }
</style>
<body>
    <h3>Task List (100+ items)</h3>
    <ol class="long-list">
        <li>First task</li>
        <li>Second task</li>
        <!-- ... -->
        <li>Ninety-ninth task</li>
        <li>One hundredth task</li>
    </ol>
</body>
```

### Example 5: Mixed alignment in nested lists

```html
<style>
    .outer-left {
        list-style-type: decimal;
        -pdf-li-align: left;
        font-size: 12pt;
    }
    .inner-right {
        list-style-type: lower-alpha;
        -pdf-li-align: right;
        font-size: 11pt;
    }
</style>
<body>
    <ol class="outer-left">
        <li>Main item with left alignment
            <ol class="inner-right">
                <li>Sub-item with right alignment</li>
                <li>Another sub-item</li>
            </ol>
        </li>
        <li>Second main item</li>
    </ol>
</body>
```

### Example 6: Table of contents with right-aligned numbers

```html
<style>
    .toc-list {
        list-style-type: decimal;
        -pdf-li-align: right;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
        padding-left: 30pt;
    }
    .toc-list li {
        padding: 6pt 0;
        border-bottom: 1pt dotted #ccc;
    }
</style>
<body>
    <h2>Table of Contents</h2>
    <ol class="toc-list">
        <li>Introduction</li>
        <li>Background</li>
        <li>Methodology</li>
        <li>Results</li>
        <li>Discussion</li>
        <li>Conclusion</li>
        <li>References</li>
        <li>Appendices</li>
        <li>Index</li>
        <li>Acknowledgments</li>
    </ol>
</body>
```

### Example 7: Center-aligned custom markers

```html
<style>
    .centered-custom {
        list-style-type: square;
        -pdf-li-align: center;
        font-size: 11pt;
        line-height: 1.8;
    }
</style>
<body>
    <ul class="centered-custom">
        <li>Centered square marker</li>
        <li>Visually balanced</li>
        <li>Professional appearance</li>
    </ul>
</body>
```

### Example 8: Procedure with right-aligned step numbers

```html
<style>
    .procedure-steps {
        list-style-type: decimal;
        -pdf-li-align: right;
        -pdf-li-prefix: "Step ";
        -pdf-li-postfix: ": ";
        font-size: 11pt;
    }
    .procedure-steps li {
        margin-bottom: 10pt;
    }
</style>
<body>
    <h2>Installation Procedure</h2>
    <ol class="procedure-steps">
        <li>Download installation package</li>
        <li>Extract files to destination</li>
        <li>Run setup wizard</li>
        <li>Configure settings</li>
        <li>Complete installation</li>
    </ol>
</body>
```

### Example 9: Legal document with hierarchical alignment

```html
<style>
    .legal-main {
        list-style-type: decimal;
        -pdf-li-align: right;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
        font-weight: bold;
    }
    .legal-sub {
        list-style-type: decimal;
        -pdf-li-align: right;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 10pt;
        font-weight: normal;
    }
</style>
<body>
    <h2>Contract Agreement</h2>
    <ol class="legal-main">
        <li>Terms and Conditions
            <ol class="legal-sub">
                <li>General provisions</li>
                <li>Specific terms</li>
                <li>Exceptions and limitations</li>
            </ol>
        </li>
        <li>Payment Terms</li>
    </ol>
</body>
```

### Example 10: FAQ with centered question numbers

```html
<style>
    .faq {
        list-style-type: decimal;
        -pdf-li-align: center;
        -pdf-li-prefix: "Q";
        -pdf-li-postfix: ": ";
        font-size: 12pt;
        font-weight: bold;
    }
    .faq li {
        margin-bottom: 15pt;
    }
    .answer {
        font-weight: normal;
        margin-top: 5pt;
        font-size: 11pt;
    }
</style>
<body>
    <h2>Frequently Asked Questions</h2>
    <ol class="faq">
        <li>What is Scryber?
            <div class="answer">Scryber is a PDF generation library for .NET applications.</div>
        </li>
        <li>How do I install it?
            <div class="answer">Use NuGet package manager to add Scryber to your project.</div>
        </li>
        <li>Is it open source?
            <div class="answer">Yes, Scryber is available under an open source license.</div>
        </li>
    </ol>
</body>
```

### Example 11: Feature checklist with aligned markers

```html
<style>
    .checklist {
        list-style-type: square;
        -pdf-li-align: center;
        font-size: 11pt;
    }
    .checklist li {
        padding: 4pt 0;
    }
</style>
<body>
    <h3>Project Checklist</h3>
    <ul class="checklist">
        <li>Requirements documented</li>
        <li>Design approved</li>
        <li>Development completed</li>
        <li>Testing finished</li>
        <li>Documentation updated</li>
        <li>Deployment successful</li>
    </ul>
</body>
```

### Example 12: Numbered paragraphs with right alignment

```html
<style>
    .numbered-paragraphs {
        list-style-type: decimal;
        -pdf-li-align: right;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
        line-height: 1.6;
        text-align: justify;
    }
    .numbered-paragraphs li {
        margin-bottom: 12pt;
    }
</style>
<body>
    <h2>Research Findings</h2>
    <ol class="numbered-paragraphs">
        <li>The initial hypothesis was confirmed through extensive testing and analysis of the collected data from multiple sources.</li>
        <li>Secondary findings revealed unexpected correlations between variables that warrant further investigation.</li>
        <li>Statistical significance was established with a confidence level exceeding 95 percent.</li>
    </ol>
</body>
```

### Example 13: Roman numerals with varied alignment

```html
<style>
    .roman-right {
        list-style-type: upper-roman;
        -pdf-li-align: right;
        font-size: 12pt;
        font-weight: bold;
    }
</style>
<body>
    <h2>Document Sections</h2>
    <ol class="roman-right">
        <li>Introduction</li>
        <li>Literature Review</li>
        <li>Methodology</li>
        <li>Results and Analysis</li>
        <li>Discussion</li>
        <li>Conclusions and Recommendations</li>
    </ol>
</body>
```

### Example 14: Multi-column list with center alignment

```html
<style>
    .centered-list {
        list-style-type: disc;
        -pdf-li-align: center;
        font-size: 10pt;
    }
</style>
<body>
    <table style="width: 100%">
        <tr>
            <td style="width: 50%; vertical-align: top">
                <h4>Features Set A</h4>
                <ul class="centered-list">
                    <li>Feature One</li>
                    <li>Feature Two</li>
                    <li>Feature Three</li>
                </ul>
            </td>
            <td style="width: 50%; vertical-align: top">
                <h4>Features Set B</h4>
                <ul class="centered-list">
                    <li>Feature Four</li>
                    <li>Feature Five</li>
                    <li>Feature Six</li>
                </ul>
            </td>
        </tr>
    </table>
</body>
```

### Example 15: Report with aligned section numbering

```html
<style>
    .report-section {
        list-style-type: decimal;
        -pdf-li-align: right;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: " ";
        font-size: 12pt;
        font-weight: bold;
    }
    .report-subsection {
        list-style-type: decimal;
        -pdf-li-align: right;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
        font-weight: normal;
    }
</style>
<body>
    <h1>Annual Report 2024</h1>
    <ol class="report-section">
        <li>Executive Summary
            <ol class="report-subsection">
                <li>Overview of achievements</li>
                <li>Key financial highlights</li>
                <li>Strategic initiatives</li>
            </ol>
        </li>
        <li>Financial Performance
            <ol class="report-subsection">
                <li>Revenue analysis</li>
                <li>Cost management</li>
                <li>Profitability trends</li>
            </ol>
        </li>
        <li>Future Outlook</li>
    </ol>
</body>
```

---

## See Also

- [list-style](/reference/cssproperties/css_prop_list-style) - List style shorthand
- [list-style-type](/reference/cssproperties/css_prop_list-style-type) - Type of list marker
- [-pdf-li-group](/reference/cssproperties/css_prop_-pdf-li-group) - Scryber custom: List number grouping
- [-pdf-li-concat](/reference/cssproperties/css_prop_-pdf-li-concat) - Scryber custom: List number concatenation
- [-pdf-li-inset](/reference/cssproperties/css_prop_-pdf-li-inset) - Scryber custom: List content inset
- [-pdf-li-prefix](/reference/cssproperties/css_prop_-pdf-li-prefix) - Scryber custom: List marker prefix
- [-pdf-li-postfix](/reference/cssproperties/css_prop_-pdf-li-postfix) - Scryber custom: List marker postfix
- [text-align](/reference/cssproperties/css_prop_text-align) - Text alignment property
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
