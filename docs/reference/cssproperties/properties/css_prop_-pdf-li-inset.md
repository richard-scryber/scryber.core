---
layout: default
title: -pdf-li-inset
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# -pdf-li-inset : List Item Inset Property (Scryber Custom)

The `-pdf-li-inset` property is a Scryber-specific extension that controls the horizontal distance (inset) between the list marker and the list item content. This property allows precise control over the spacing between bullets/numbers and their associated text, enabling professional document layouts with optimal readability.

## Usage

```css
selector {
    -pdf-li-inset: value;
}
```

The value specifies the amount of space between the list marker and the content text.

---

## Supported Values

- **Length values** - Specified in points (pt), pixels (px), or other CSS units
  - `10pt` - 10 points of spacing
  - `15px` - 15 pixels of spacing
  - `5mm` - 5 millimeters of spacing
- **0** - No spacing between marker and content (not recommended)

### Default Value

The default inset varies based on the list style and font size, typically around 8-10pt.

---

## Supported Elements

The `-pdf-li-inset` property can be applied to:
- Ordered lists (`<ol>`)
- Unordered lists (`<ul>`)
- Individual list items (`<li>`)

When applied to list containers, the inset is inherited by all child list items.

---

## Notes

- This is a Scryber-specific property designed for advanced PDF list formatting
- The inset controls spacing between the marker and content, not the overall list indentation
- Proper inset values improve readability by providing adequate visual separation
- Too small values can make text appear cramped; too large values waste space
- The property works with all list-style-type values
- Can be combined with padding-left to control overall list positioning
- Different inset values can be applied to nested lists for visual hierarchy
- Inset affects only horizontal spacing, not vertical line spacing
- Consider font size when setting inset values for proportional layouts

---

## Data Binding

The `-pdf-li-inset` property supports data binding, enabling dynamic control of spacing between list markers and content based on document type, user preferences, or accessibility requirements. This allows documents to automatically adjust their layout for optimal readability across different contexts.

### Example 1: User-configurable spacing preferences

```html
<style>
    .custom-inset-list {
        list-style-type: decimal;
        -pdf-li-inset: {{userPrefs.insetSize}}pt;
        font-size: {{userPrefs.fontSize}}pt;
    }
</style>
<body>
    <h3>{{document.title}}</h3>
    <ol class="custom-inset-list">
        <li>{{items[0]}}</li>
        <li>{{items[1]}}</li>
        <li>{{items[2]}}</li>
    </ol>
</body>
```

**Data context:**
```json
{
    "document": {
        "title": "Requirements List"
    },
    "userPrefs": {
        "insetSize": 15,
        "fontSize": 12
    },
    "items": [
        "First requirement with custom spacing",
        "Second requirement with custom spacing",
        "Third requirement with custom spacing"
    ]
}
```

Users can configure inset spacing based on their readability preferences or accessibility needs. Larger insets improve readability for users with visual impairments.

### Example 2: Document density control

```html
<style>
    .density-list {
        list-style-type: disc;
        -pdf-li-inset: {{layout.density == 'compact' ? '5' : layout.density == 'normal' ? '10' : '15'}}pt;
        font-size: {{layout.fontSize}}pt;
        line-height: {{layout.lineHeight}};
    }
</style>
<body>
    <h2>{{content.heading}}</h2>
    <ul class="density-list">
        <li>{{content.items[0]}}</li>
        <li>{{content.items[1]}}</li>
        <li>{{content.items[2]}}</li>
    </ul>
</body>
```

**Data context:**
```json
{
    "layout": {
        "density": "normal",
        "fontSize": 11,
        "lineHeight": 1.6
    },
    "content": {
        "heading": "Feature Overview",
        "items": [
            "Fast performance",
            "Easy to use",
            "Well documented"
        ]
    }
}
```

Documents can adapt between compact (5pt inset), normal (10pt inset), or spacious (15pt inset) layouts based on space constraints or presentation requirements.

### Example 3: Hierarchical inset for nested lists

```html
<style>
    .level-1 {
        list-style-type: decimal;
        -pdf-li-inset: {{spacing.level1}}pt;
        font-size: 12pt;
    }
    .level-2 {
        list-style-type: lower-alpha;
        -pdf-li-inset: {{spacing.level2}}pt;
        font-size: 11pt;
    }
    .level-3 {
        list-style-type: lower-roman;
        -pdf-li-inset: {{spacing.level3}}pt;
        font-size: 10pt;
    }
</style>
<body>
    <h2>{{document.title}}</h2>
    <ol class="level-1">
        <li>{{sections[0].title}}
            <ol class="level-2">
                <li>{{sections[0].subsections[0].title}}
                    <ol class="level-3">
                        <li>{{sections[0].subsections[0].items[0]}}</li>
                        <li>{{sections[0].subsections[0].items[1]}}</li>
                    </ol>
                </li>
            </ol>
        </li>
    </ol>
</body>
```

**Data context:**
```json
{
    "document": {
        "title": "Technical Specification"
    },
    "spacing": {
        "level1": 16,
        "level2": 12,
        "level3": 8
    },
    "sections": [
        {
            "title": "Requirements",
            "subsections": [
                {
                    "title": "Functional Requirements",
                    "items": ["User authentication", "Data validation"]
                }
            ]
        }
    ]
}
```

Progressive inset reduction for nested levels creates visual hierarchy. Organizations can define standard spacing schemes that automatically apply to all documents.

---

## Examples

### Example 1: Default inset spacing

```html
<style>
    .default-inset {
        list-style-type: disc;
        font-size: 12pt;
    }
</style>
<body>
    <ul class="default-inset">
        <li>Item with default spacing</li>
        <li>Another item</li>
        <li>Final item</li>
    </ul>
</body>
```

### Example 2: Increased inset for better readability

```html
<style>
    .large-inset {
        list-style-type: decimal;
        -pdf-li-inset: 15pt;
        font-size: 12pt;
    }
</style>
<body>
    <ol class="large-inset">
        <li>First item with more spacing</li>
        <li>Second item with comfortable gap</li>
        <li>Third item with improved readability</li>
    </ol>
</body>
```

### Example 3: Compact list with reduced inset

```html
<style>
    .compact-list {
        list-style-type: disc;
        -pdf-li-inset: 5pt;
        font-size: 10pt;
        line-height: 1.4;
    }
</style>
<body>
    <h3>Quick Reference</h3>
    <ul class="compact-list">
        <li>Compact item one</li>
        <li>Compact item two</li>
        <li>Compact item three</li>
        <li>Compact item four</li>
    </ul>
</body>
```

### Example 4: Nested lists with varying insets

```html
<style>
    .outer-list {
        list-style-type: decimal;
        -pdf-li-inset: 12pt;
        font-size: 12pt;
    }
    .inner-list {
        list-style-type: lower-alpha;
        -pdf-li-inset: 8pt;
        font-size: 11pt;
    }
</style>
<body>
    <ol class="outer-list">
        <li>Main item with 12pt inset
            <ol class="inner-list">
                <li>Sub-item with 8pt inset</li>
                <li>Another sub-item</li>
            </ol>
        </li>
        <li>Second main item</li>
    </ol>
</body>
```

### Example 5: Professional document with consistent spacing

```html
<style>
    .document-list {
        list-style-type: decimal;
        -pdf-li-inset: 10pt;
        font-size: 11pt;
        line-height: 1.6;
    }
</style>
<body>
    <h2>Project Requirements</h2>
    <ol class="document-list">
        <li>The system shall provide user authentication</li>
        <li>The system shall support multiple user roles</li>
        <li>The system shall maintain audit logs</li>
        <li>The system shall comply with security standards</li>
    </ol>
</body>
```

### Example 6: Technical specification with precise spacing

```html
<style>
    .tech-spec {
        list-style-type: decimal;
        -pdf-li-inset: 14pt;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
</style>
<body>
    <h2>Technical Specifications</h2>
    <ol class="tech-spec">
        <li>Hardware Requirements
            <ol class="tech-spec">
                <li>Minimum 8GB RAM</li>
                <li>64-bit processor</li>
                <li>100GB available storage</li>
            </ol>
        </li>
        <li>Software Requirements
            <ol class="tech-spec">
                <li>Operating System: Windows 10 or later</li>
                <li>Framework: .NET 6.0 or higher</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 7: Bullet list with generous spacing

```html
<style>
    .spacious-bullets {
        list-style-type: disc;
        -pdf-li-inset: 20pt;
        font-size: 13pt;
        line-height: 1.8;
    }
</style>
<body>
    <h2>Key Benefits</h2>
    <ul class="spacious-bullets">
        <li>Easy to use and implement</li>
        <li>Comprehensive documentation</li>
        <li>Active community support</li>
        <li>Regular updates and improvements</li>
    </ul>
</body>
```

### Example 8: Dense information list

```html
<style>
    .dense-list {
        list-style-type: square;
        -pdf-li-inset: 6pt;
        font-size: 9pt;
        line-height: 1.3;
    }
</style>
<body>
    <h4>Quick Facts</h4>
    <ul class="dense-list">
        <li>Founded: 2020</li>
        <li>Location: Global</li>
        <li>Employees: 50+</li>
        <li>Products: 10+</li>
        <li>Customers: 1000+</li>
    </ul>
</body>
```

### Example 9: Legal document with standard inset

```html
<style>
    .legal-list {
        list-style-type: decimal;
        -pdf-li-inset: 12pt;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
        text-align: justify;
    }
    .legal-list li {
        margin-bottom: 10pt;
    }
</style>
<body>
    <h2>Terms of Service</h2>
    <ol class="legal-list">
        <li>General Provisions
            <ol class="legal-list">
                <li>These terms govern your use of the service</li>
                <li>By using the service, you agree to these terms</li>
                <li>We may modify these terms at any time</li>
            </ol>
        </li>
        <li>User Responsibilities</li>
    </ol>
</body>
```

### Example 10: Multi-level procedure with progressive insets

```html
<style>
    .level-1 {
        list-style-type: decimal;
        -pdf-li-inset: 15pt;
        font-size: 12pt;
        font-weight: bold;
    }
    .level-2 {
        list-style-type: lower-alpha;
        -pdf-li-inset: 12pt;
        font-size: 11pt;
        font-weight: 600;
    }
    .level-3 {
        list-style-type: lower-roman;
        -pdf-li-inset: 8pt;
        font-size: 10pt;
        font-weight: normal;
    }
</style>
<body>
    <h2>Installation Guide</h2>
    <ol class="level-1">
        <li>Preparation
            <ol class="level-2">
                <li>System Check
                    <ol class="level-3">
                        <li>Verify operating system</li>
                        <li>Check available disk space</li>
                        <li>Ensure admin privileges</li>
                    </ol>
                </li>
                <li>Download software</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 11: Feature comparison with aligned insets

```html
<style>
    .feature-list {
        list-style-type: circle;
        -pdf-li-inset: 10pt;
        font-size: 11pt;
    }
</style>
<body>
    <table style="width: 100%; border-collapse: collapse">
        <tr>
            <th style="text-align: left; padding: 8pt; border-bottom: 1pt solid #000">
                Basic Plan
            </th>
            <th style="text-align: left; padding: 8pt; border-bottom: 1pt solid #000">
                Premium Plan
            </th>
        </tr>
        <tr>
            <td style="vertical-align: top; padding: 8pt">
                <ul class="feature-list">
                    <li>5 users</li>
                    <li>10GB storage</li>
                    <li>Email support</li>
                </ul>
            </td>
            <td style="vertical-align: top; padding: 8pt">
                <ul class="feature-list">
                    <li>Unlimited users</li>
                    <li>1TB storage</li>
                    <li>24/7 phone support</li>
                </ul>
            </td>
        </tr>
    </table>
</body>
```

### Example 12: Task list with custom inset

```html
<style>
    .task-list {
        list-style-type: decimal;
        -pdf-li-inset: 18pt;
        -pdf-li-prefix: "Task ";
        -pdf-li-postfix: ": ";
        font-size: 11pt;
    }
    .task-list li {
        margin-bottom: 12pt;
    }
</style>
<body>
    <h2>Project Tasks</h2>
    <ol class="task-list">
        <li>Complete requirements analysis and document all functional specifications</li>
        <li>Design system architecture and create technical documentation</li>
        <li>Implement core features and write unit tests</li>
        <li>Conduct integration testing and fix identified issues</li>
        <li>Deploy to production environment and monitor performance</li>
    </ol>
</body>
```

### Example 13: Outline with hierarchical spacing

```html
<style>
    .outline {
        list-style-type: upper-roman;
        -pdf-li-inset: 16pt;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 12pt;
    }
    .outline-sub {
        list-style-type: upper-alpha;
        -pdf-li-inset: 12pt;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
    .outline-detail {
        list-style-type: decimal;
        -pdf-li-inset: 8pt;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 10pt;
    }
</style>
<body>
    <h2>Document Outline</h2>
    <ol class="outline">
        <li>Introduction
            <ol class="outline-sub">
                <li>Background
                    <ol class="outline-detail">
                        <li>Historical context</li>
                        <li>Current situation</li>
                    </ol>
                </li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 14: Checklist with minimal inset

```html
<style>
    .checklist {
        list-style-type: none;
        -pdf-li-inset: 0pt;
        font-size: 11pt;
        padding-left: 0;
    }
    .checklist li:before {
        content: "☐ ";
        margin-right: 8pt;
        font-size: 14pt;
    }
</style>
<body>
    <h3>Pre-flight Checklist</h3>
    <ul class="checklist">
        <li>Documentation reviewed</li>
        <li>Code peer-reviewed</li>
        <li>Tests passing</li>
        <li>Build successful</li>
        <li>Deployment approved</li>
    </ul>
</body>
```

### Example 15: Report with proportional spacing

```html
<style>
    .report-main {
        list-style-type: decimal;
        -pdf-li-inset: 14pt;
        -pdf-li-postfix: ". ";
        font-size: 12pt;
        font-weight: bold;
    }
    .report-sub {
        list-style-type: lower-alpha;
        -pdf-li-inset: 11pt;
        font-size: 11pt;
        font-weight: normal;
        margin-top: 6pt;
    }
    .report-detail {
        list-style-type: lower-roman;
        -pdf-li-inset: 8pt;
        font-size: 10pt;
        margin-top: 4pt;
    }
</style>
<body>
    <h1>Quarterly Business Review</h1>
    <ol class="report-main">
        <li>Financial Performance
            <ol class="report-sub">
                <li>Revenue Analysis
                    <ol class="report-detail">
                        <li>Product sales</li>
                        <li>Service revenue</li>
                        <li>Licensing fees</li>
                    </ol>
                </li>
                <li>Cost Management
                    <ol class="report-detail">
                        <li>Operational expenses</li>
                        <li>Capital expenditures</li>
                    </ol>
                </li>
            </ol>
        </li>
        <li>Market Analysis
            <ol class="report-sub">
                <li>Market share trends</li>
                <li>Competitive landscape</li>
            </ol>
        </li>
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
- [-pdf-li-prefix](/reference/cssproperties/css_prop_-pdf-li-prefix) - Scryber custom: List marker prefix
- [-pdf-li-postfix](/reference/cssproperties/css_prop_-pdf-li-postfix) - Scryber custom: List marker postfix
- [padding-left](/reference/cssproperties/css_prop_padding-left) - Left padding property
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
