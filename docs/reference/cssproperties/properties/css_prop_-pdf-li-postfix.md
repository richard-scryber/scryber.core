---
layout: default
title: -pdf-li-postfix
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# -pdf-li-postfix : List Item Postfix Property (Scryber Custom)

The `-pdf-li-postfix` property is a Scryber-specific extension that adds custom text after list item markers. This property enables advanced list formatting by allowing you to append text, symbols, or punctuation to list numbers or bullets, creating professional documentation with custom marker styles like "1.", "A)", "1.1:", or "Item 5 -".

## Usage

```css
selector {
    -pdf-li-postfix: "text";
}
```

The value is a string that will be displayed after the list marker.

---

## Supported Values

- **String values** - Any text enclosed in quotes
  - `"."` - Adds period after the marker (common for numbered lists)
  - `":"` - Adds colon after the marker
  - `")"` - Adds closing parenthesis after the marker
  - `"]"` - Adds closing bracket after the marker
  - `" -"` - Adds space and dash after the marker
  - `". "` - Adds period and space after the marker
- **Empty string** `""` - No postfix (default)

### Default Value

- **""** - No postfix text

---

## Supported Elements

The `-pdf-li-postfix` property can be applied to:
- Ordered lists (`<ol>`)
- Unordered lists (`<ul>`)
- Individual list items (`<li>`)

When applied to list containers, the postfix is inherited by all child list items.

---

## Notes

- This is a Scryber-specific property designed for advanced PDF list formatting
- The postfix text appears after the list marker (number, bullet, letter, etc.)
- Commonly used with `-pdf-li-prefix` to create complete custom marker formats
- Essential for formatting like legal numbering (1., 1.1., 1.1.1.)
- Works with all list-style-type values
- The postfix is part of the marker display, not the content
- Including a space in the postfix ensures proper spacing from content
- Multiple characters and symbols are supported
- Postfix formatting affects visual appearance but not semantic structure
- Postfixes are inherited by nested lists unless overridden

---

## Data Binding

The `-pdf-li-postfix` property supports data binding, enabling dynamic postfix text based on document type, language conventions, or organizational standards. This allows documents to automatically adapt their list marker formatting to different contexts and requirements.

### Example 1: Configurable postfix punctuation

```html
<style>
    .formatted-list {
        list-style-type: decimal;
        -pdf-li-postfix: "{{format.numberPostfix}}";
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{document.title}}</h2>
    <ol class="formatted-list">
        <li>{{items[0]}}</li>
        <li>{{items[1]}}</li>
        <li>{{items[2]}}</li>
    </ol>
</body>
```

**Data context (with period):**
```json
{
    "document": {
        "title": "Legal Document"
    },
    "format": {
        "numberPostfix": ". "
    },
    "items": [
        "First provision",
        "Second provision",
        "Third provision"
    ]
}
```

**Data context (with colon):**
```json
{
    "document": {
        "title": "Instructions"
    },
    "format": {
        "numberPostfix": ": "
    },
    "items": [
        "First step",
        "Second step",
        "Third step"
    ]
}
```

Different document types can use appropriate postfix punctuation: periods for formal documents, colons for instructions, parentheses for informal lists.

### Example 2: Multilingual postfix conventions

```html
<style>
    .localized-list {
        list-style-type: decimal;
        -pdf-li-postfix: "{{locale.listPostfix}}";
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{locale.heading}}</h2>
    <ol class="localized-list">
        <li>{{locale.items[0]}}</li>
        <li>{{locale.items[1]}}</li>
        <li>{{locale.items[2]}}</li>
    </ol>
</body>
```

**Data context (English/US):**
```json
{
    "locale": {
        "heading": "Requirements",
        "listPostfix": ". ",
        "items": [
            "System shall be secure",
            "System shall be scalable",
            "System shall be maintainable"
        ]
    }
}
```

**Data context (French):**
```json
{
    "locale": {
        "heading": "Exigences",
        "listPostfix": " - ",
        "items": [
            "Le système doit être sécurisé",
            "Le système doit être évolutif",
            "Le système doit être maintenable"
        ]
    }
}
```

Different languages and regions have different conventions for list punctuation. Data binding ensures documents follow local formatting standards.

### Example 3: Legal document section numbering

```html
<style>
    .legal-sections {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: "{{jurisdiction.sectionPostfix}}";
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{contract.title}} - {{jurisdiction.name}}</h2>
    <ol class="legal-sections">
        <li>{{sections[0].title}}
            <ol class="legal-sections">
                <li>{{sections[0].provisions[0]}}</li>
                <li>{{sections[0].provisions[1]}}</li>
            </ol>
        </li>
        <li>{{sections[1].title}}</li>
    </ol>
</body>
```

**Data context (US jurisdiction):**
```json
{
    "contract": {
        "title": "Service Agreement"
    },
    "jurisdiction": {
        "name": "United States",
        "sectionPostfix": ". "
    },
    "sections": [
        {
            "title": "Terms of Service",
            "provisions": [
                "Service shall be provided as specified",
                "Payment terms are net 30 days"
            ]
        },
        {
            "title": "Liability"
        }
    ]
}
```

**Data context (UK jurisdiction):**
```json
{
    "contract": {
        "title": "Service Agreement"
    },
    "jurisdiction": {
        "name": "United Kingdom",
        "sectionPostfix": " "
    },
    "sections": [
        {
            "title": "Terms of Service",
            "provisions": [
                "Service shall be provided as specified",
                "Payment terms are net 30 days"
            ]
        },
        {
            "title": "Liability"
        }
    ]
}
```

Legal documents must conform to jurisdiction-specific formatting requirements. Data binding ensures automatic compliance with local legal documentation standards.

---

## Examples

### Example 1: Period after numbers (1. 2. 3.)

```html
<style>
    .period-postfix {
        list-style-type: decimal;
        -pdf-li-postfix: ". ";
        font-size: 12pt;
    }
</style>
<body>
    <ol class="period-postfix">
        <li>First item (displays as "1.")</li>
        <li>Second item (displays as "2.")</li>
        <li>Third item (displays as "3.")</li>
    </ol>
</body>
```

### Example 2: Colon after numbers (1: 2: 3:)

```html
<style>
    .colon-postfix {
        list-style-type: decimal;
        -pdf-li-postfix: ": ";
        font-size: 11pt;
    }
</style>
<body>
    <h3>Instructions:</h3>
    <ol class="colon-postfix">
        <li>Read the documentation carefully</li>
        <li>Follow the installation steps</li>
        <li>Test the implementation</li>
    </ol>
</body>
```

### Example 3: Parenthesized letters (A) B) C))

```html
<style>
    .paren-postfix {
        list-style-type: upper-alpha;
        -pdf-li-postfix: ") ";
        font-size: 11pt;
    }
</style>
<body>
    <h3>Multiple Choice:</h3>
    <ol class="paren-postfix">
        <li>Option A</li>
        <li>Option B</li>
        <li>Option C</li>
        <li>Option D</li>
    </ol>
</body>
```

### Example 4: Dash after numbers (1 - 2 - 3 -)

```html
<style>
    .dash-postfix {
        list-style-type: decimal;
        -pdf-li-postfix: " - ";
        font-size: 11pt;
    }
</style>
<body>
    <ol class="dash-postfix">
        <li>First requirement</li>
        <li>Second requirement</li>
        <li>Third requirement</li>
    </ol>
</body>
```

### Example 5: Legal document numbering (1. 1.1. 1.1.1.)

```html
<style>
    .legal-numbering {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
    }
</style>
<body>
    <h2>Contract Terms</h2>
    <ol class="legal-numbering">
        <li>General Provisions
            <ol class="legal-numbering">
                <li>Definitions
                    <ol class="legal-numbering">
                        <li>Term "Party"</li>
                        <li>Term "Agreement"</li>
                    </ol>
                </li>
                <li>Scope</li>
            </ol>
        </li>
        <li>Specific Terms</li>
    </ol>
</body>
```

### Example 6: Right bracket after letters (A] B] C])

```html
<style>
    .bracket-postfix {
        list-style-type: upper-alpha;
        -pdf-li-postfix: "] ";
        font-size: 11pt;
    }
</style>
<body>
    <ol class="bracket-postfix">
        <li>First category</li>
        <li>Second category</li>
        <li>Third category</li>
    </ol>
</body>
```

### Example 7: Roman numerals with period (I. II. III.)

```html
<style>
    .roman-period {
        list-style-type: upper-roman;
        -pdf-li-postfix: ". ";
        font-size: 13pt;
        font-weight: bold;
    }
</style>
<body>
    <h2>Book Sections</h2>
    <ol class="roman-period">
        <li>Preface</li>
        <li>Introduction</li>
        <li>Main Content</li>
        <li>Conclusion</li>
        <li>Appendices</li>
    </ol>
</body>
```

### Example 8: Technical specification numbering

```html
<style>
    .spec-numbering {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: " ";
        font-size: 11pt;
    }
</style>
<body>
    <h2>System Requirements</h2>
    <ol class="spec-numbering">
        <li>Functional Requirements
            <ol class="spec-numbering">
                <li>User Authentication
                    <ol class="spec-numbering">
                        <li>Login functionality</li>
                        <li>Password recovery</li>
                    </ol>
                </li>
                <li>Data Management</li>
            </ol>
        </li>
        <li>Non-Functional Requirements</li>
    </ol>
</body>
```

### Example 9: Question and answer format

```html
<style>
    .qa-list {
        list-style-type: decimal;
        -pdf-li-prefix: "Q";
        -pdf-li-postfix: ": ";
        font-size: 12pt;
        font-weight: bold;
    }
    .qa-list li {
        margin-bottom: 15pt;
    }
    .answer {
        font-weight: normal;
        margin-top: 5pt;
    }
</style>
<body>
    <h2>FAQ</h2>
    <ol class="qa-list">
        <li>What is Scryber?
            <div class="answer">A: Scryber is a PDF generation library for .NET</div>
        </li>
        <li>How do I install it?
            <div class="answer">A: Use NuGet to install the Scryber package</div>
        </li>
        <li>Is it free?
            <div class="answer">A: Yes, Scryber is open source</div>
        </li>
    </ol>
</body>
```

### Example 10: Outline with hierarchical postfix

```html
<style>
    .outline-1 {
        list-style-type: upper-roman;
        -pdf-li-postfix: ". ";
        font-size: 13pt;
        font-weight: bold;
    }
    .outline-2 {
        list-style-type: upper-alpha;
        -pdf-li-postfix: ". ";
        font-size: 12pt;
        font-weight: 600;
    }
    .outline-3 {
        list-style-type: decimal;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
        font-weight: normal;
    }
</style>
<body>
    <h2>Research Paper Outline</h2>
    <ol class="outline-1">
        <li>Introduction
            <ol class="outline-2">
                <li>Background
                    <ol class="outline-3">
                        <li>Historical context</li>
                        <li>Current challenges</li>
                    </ol>
                </li>
                <li>Research objectives</li>
            </ol>
        </li>
        <li>Methodology</li>
    </ol>
</body>
```

### Example 11: Step-by-step guide with arrows

```html
<style>
    .step-guide {
        list-style-type: decimal;
        -pdf-li-prefix: "Step ";
        -pdf-li-postfix: " → ";
        font-size: 11pt;
    }
</style>
<body>
    <h2>Setup Guide</h2>
    <ol class="step-guide">
        <li>Create new project</li>
        <li>Install dependencies</li>
        <li>Configure settings</li>
        <li>Build application</li>
        <li>Deploy to production</li>
    </ol>
</body>
```

### Example 12: Table of contents with periods

```html
<style>
    .toc {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
        padding-left: 0;
    }
    .toc li {
        padding: 5pt 0;
    }
</style>
<body>
    <h2>Table of Contents</h2>
    <ol class="toc">
        <li>Introduction
            <ol class="toc">
                <li>Purpose</li>
                <li>Scope</li>
                <li>Definitions</li>
            </ol>
        </li>
        <li>Main Content
            <ol class="toc">
                <li>Overview</li>
                <li>Details
                    <ol class="toc">
                        <li>Technical aspects</li>
                        <li>Implementation</li>
                    </ol>
                </li>
            </ol>
        </li>
        <li>Conclusion</li>
    </ol>
</body>
```

### Example 13: Clause numbering for legal documents

```html
<style>
    .clause {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-prefix: "Clause ";
        -pdf-li-postfix: ": ";
        font-size: 11pt;
        font-weight: bold;
    }
    .subclause {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: " ";
        font-size: 10pt;
        font-weight: normal;
    }
</style>
<body>
    <h2>Service Agreement</h2>
    <ol class="clause">
        <li>Service Delivery
            <ol class="subclause">
                <li>The provider shall deliver services as specified</li>
                <li>Services will be provided during business hours</li>
            </ol>
        </li>
        <li>Payment Terms
            <ol class="subclause">
                <li>Payment is due within 30 days</li>
                <li>Late payments incur interest</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 14: Procedure with custom formatting

```html
<style>
    .procedure {
        list-style-type: decimal;
        -pdf-li-postfix: ") ";
        font-size: 11pt;
        line-height: 1.6;
    }
    .procedure li {
        margin-bottom: 8pt;
    }
</style>
<body>
    <h2>Safety Procedure</h2>
    <ol class="procedure">
        <li>Ensure all equipment is powered off</li>
        <li>Wear appropriate protective gear</li>
        <li>Follow lockout/tagout procedures</li>
        <li>Verify safe conditions before proceeding</li>
        <li>Complete safety checklist</li>
    </ol>
</body>
```

### Example 15: Academic paper sections

```html
<style>
    .academic {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
    }
    .academic li {
        margin-bottom: 6pt;
    }
</style>
<body>
    <h1>Research Paper Structure</h1>
    <ol class="academic">
        <li>Abstract</li>
        <li>Introduction
            <ol class="academic">
                <li>Background and motivation</li>
                <li>Problem statement</li>
                <li>Research questions</li>
                <li>Contribution</li>
            </ol>
        </li>
        <li>Related Work
            <ol class="academic">
                <li>Previous approaches</li>
                <li>Current state of the art</li>
                <li>Gaps in existing research</li>
            </ol>
        </li>
        <li>Methodology
            <ol class="academic">
                <li>Research design</li>
                <li>Data collection
                    <ol class="academic">
                        <li>Sampling strategy</li>
                        <li>Data sources</li>
                        <li>Collection instruments</li>
                    </ol>
                </li>
                <li>Analysis methods</li>
            </ol>
        </li>
        <li>Results
            <ol class="academic">
                <li>Descriptive statistics</li>
                <li>Hypothesis testing</li>
                <li>Key findings</li>
            </ol>
        </li>
        <li>Discussion
            <ol class="academic">
                <li>Interpretation of results</li>
                <li>Implications</li>
                <li>Limitations</li>
            </ol>
        </li>
        <li>Conclusion
            <ol class="academic">
                <li>Summary of findings</li>
                <li>Future research directions</li>
            </ol>
        </li>
        <li>References</li>
    </ol>
</body>
```

---

## See Also

- [list-style](/reference/cssproperties/css_prop_list-style) - List style shorthand
- [list-style-type](/reference/cssproperties/css_prop_list-style-type) - Type of list marker
- [-pdf-li-group](/reference/cssproperties/css_prop_-pdf-li-group) - Scryber custom: List number grouping
- [-pdf-li-concat](/reference/cssproperties/css_prop_-pdf-li-concat) - Scryber custom: List number concatenation
- [-pdf-li-align](/reference/cssproperties/css_prop_-pdf-li-align) - Scryber custom: List marker alignment
- [-pdf-li-inset](/reference/cssproperties/css_prop_-pdf-li-inset) - Scryber custom: List content inset
- [-pdf-li-prefix](/reference/cssproperties/css_prop_-pdf-li-prefix) - Scryber custom: List marker prefix
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
