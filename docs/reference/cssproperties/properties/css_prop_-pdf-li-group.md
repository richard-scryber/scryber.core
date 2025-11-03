---
layout: default
title: -pdf-li-group
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# -pdf-li-group : List Item Group Property (Scryber Custom)

The `-pdf-li-group` property is a Scryber-specific extension that controls how list item numbering is grouped across nested lists. This property enables advanced list numbering schemes commonly found in legal documents, technical specifications, and structured documentation where hierarchical numbering is required.

## Usage

```css
selector {
    -pdf-li-group: value;
}
```

This property determines whether nested list items continue the parent's numbering sequence or start a new independent numbering group.

---

## Supported Values

- **true** - Creates a new numbering group (default behavior)
- **false** - Continues parent numbering without creating a new group
- **[group-name]** - Creates or joins a named numbering group

### Default Value

- **true** - Each nested list starts its own numbering sequence

---

## Supported Elements

The `-pdf-li-group` property can be applied to:
- Ordered lists (`<ol>`)
- List items (`<li>`)

This property is particularly useful when applied to nested ordered lists to control hierarchical numbering.

---

## Notes

- This is a Scryber-specific property designed for advanced PDF list formatting
- The property is essential for creating legal-style numbering (1, 1.1, 1.1.1, etc.)
- When combined with `-pdf-li-concat`, it enables hierarchical numbering display
- Grouping affects how list numbers are tracked and incremented
- Named groups allow non-sequential lists to share the same numbering sequence
- The property does not affect unordered lists (bullets remain unchanged)
- Standard CSS list properties still apply alongside this custom property
- Understanding grouping is crucial for complex document structures with multiple nested levels

---

## Data Binding

The `-pdf-li-group` property supports data binding, allowing dynamic control of list numbering groups based on document type, user preferences, or organizational standards. This enables sophisticated numbering schemes that adapt to different contexts while maintaining consistent document structure.

### Example 1: Dynamic legal numbering scheme

```html
<style>
    .legal-sections {
        list-style-type: decimal;
        -pdf-li-group: {{doc.enableGrouping}};
        -pdf-li-concat: {{doc.enableGrouping}};
        -pdf-li-postfix: ". ";
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{doc.title}}</h2>
    <ol class="legal-sections">
        <li>{{sections[0].title}}
            <ol class="legal-sections">
                <li>{{sections[0].items[0]}}</li>
                <li>{{sections[0].items[1]}}</li>
            </ol>
        </li>
        <li>{{sections[1].title}}</li>
    </ol>
</body>
```

**Data context:**
```json
{
    "doc": {
        "title": "Terms of Service",
        "enableGrouping": true
    },
    "sections": [
        {
            "title": "General Provisions",
            "items": ["Definitions", "Scope of Agreement"]
        },
        {
            "title": "User Obligations"
        }
    ]
}
```

When `enableGrouping` is true, produces hierarchical numbering (1, 1.1, 1.2); when false, each list starts fresh (1, 1, 2).

### Example 2: Configurable document outline

```html
<style>
    .outline {
        list-style-type: decimal;
        -pdf-li-group: {{format.useHierarchicalNumbering}};
        -pdf-li-concat: {{format.useHierarchicalNumbering}};
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{docTitle}}</h2>
    <ol class="outline">
        <li>{{chapters[0].title}}
            <ol class="outline">
                <li>{{chapters[0].sections[0]}}</li>
                <li>{{chapters[0].sections[1]}}</li>
            </ol>
        </li>
    </ol>
</body>
```

**Data context:**
```json
{
    "docTitle": "Technical Specification",
    "format": {
        "useHierarchicalNumbering": true
    },
    "chapters": [
        {
            "title": "Introduction",
            "sections": ["Background", "Objectives"]
        }
    ]
}
```

Organizations can configure whether documents use hierarchical (1.1, 1.2) or independent (1, 2) numbering based on their documentation standards.

### Example 3: Named groups for complex documents

```html
<style>
    .requirement {
        list-style-type: decimal;
        -pdf-li-group: "{{groupName}}";
        -pdf-li-prefix: "REQ-";
        font-size: 11pt;
    }
</style>
<body>
    <h3>Functional Requirements</h3>
    <ol class="requirement" style="-pdf-li-group: '{{reqGroups.functional}}'">
        <li>User authentication required</li>
        <li>Data validation required</li>
    </ol>

    <h3>Non-Functional Requirements</h3>
    <ol class="requirement" style="-pdf-li-group: '{{reqGroups.nonFunctional}}'">
        <li>Response time under 200ms</li>
        <li>99.9% uptime required</li>
    </ol>
</body>
```

**Data context:**
```json
{
    "reqGroups": {
        "functional": "func-reqs",
        "nonFunctional": "non-func-reqs"
    }
}
```

Different requirement categories can maintain independent numbering sequences using named groups, essential for requirements traceability in large specifications.

---

## Examples

### Example 1: Basic independent numbering (default)

```html
<style>
    .independent-list {
        list-style-type: decimal;
        -pdf-li-group: true;
        font-size: 12pt;
    }
</style>
<body>
    <ol class="independent-list">
        <li>Item 1
            <ol class="independent-list">
                <li>Sub-item 1 (starts at 1)</li>
                <li>Sub-item 2</li>
            </ol>
        </li>
        <li>Item 2</li>
    </ol>
</body>
```

### Example 2: Hierarchical numbering with grouping

```html
<style>
    .hierarchical {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
</style>
<body>
    <ol class="hierarchical">
        <li>First Level (1)
            <ol class="hierarchical">
                <li>Second Level (1.1)</li>
                <li>Second Level (1.2)
                    <ol class="hierarchical">
                        <li>Third Level (1.2.1)</li>
                        <li>Third Level (1.2.2)</li>
                    </ol>
                </li>
            </ol>
        </li>
        <li>First Level (2)</li>
    </ol>
</body>
```

### Example 3: Legal document numbering

```html
<style>
    .legal-section {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ".";
        font-size: 11pt;
        font-weight: bold;
    }
    .legal-subsection {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: " ";
        font-size: 10pt;
        font-weight: normal;
    }
</style>
<body>
    <h2>Contract Terms</h2>
    <ol class="legal-section">
        <li>Definitions
            <ol class="legal-subsection">
                <li>The term "Party" refers to...</li>
                <li>The term "Agreement" means...</li>
            </ol>
        </li>
        <li>Terms of Service
            <ol class="legal-subsection">
                <li>Service provision details</li>
                <li>Payment terms</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 4: Named group for continued numbering

```html
<style>
    .group-a {
        list-style-type: decimal;
        -pdf-li-group: "main-sequence";
        font-size: 11pt;
    }
</style>
<body>
    <h3>Part 1</h3>
    <ol class="group-a">
        <li>Item 1</li>
        <li>Item 2</li>
    </ol>

    <p>Some intervening content...</p>

    <h3>Part 2 (continues numbering)</h3>
    <ol class="group-a">
        <li>Item 3 (continues from previous list)</li>
        <li>Item 4</li>
    </ol>
</body>
```

### Example 5: Technical specification numbering

```html
<style>
    .spec-main {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ".";
        font-size: 12pt;
        font-weight: bold;
    }
    .spec-sub {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: " ";
        font-size: 11pt;
        font-weight: normal;
    }
</style>
<body>
    <h2>System Requirements</h2>
    <ol class="spec-main">
        <li>Hardware Requirements
            <ol class="spec-sub">
                <li>Minimum 8GB RAM</li>
                <li>64-bit processor</li>
                <li>100GB storage</li>
            </ol>
        </li>
        <li>Software Requirements
            <ol class="spec-sub">
                <li>Operating System: Windows 10+</li>
                <li>Framework: .NET 6.0+</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 6: Multi-level procedure with grouping

```html
<style>
    .procedure {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
    .procedure li {
        margin-bottom: 10pt;
    }
</style>
<body>
    <h2>Installation Procedure</h2>
    <ol class="procedure">
        <li>Preparation
            <ol class="procedure">
                <li>Download software</li>
                <li>Verify checksum</li>
                <li>Backup existing data</li>
            </ol>
        </li>
        <li>Installation
            <ol class="procedure">
                <li>Run installer</li>
                <li>Configure settings
                    <ol class="procedure">
                        <li>Set installation path</li>
                        <li>Choose components</li>
                        <li>Review options</li>
                    </ol>
                </li>
                <li>Complete installation</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 7: Policy document structure

```html
<style>
    .policy {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-prefix: "Section ";
        -pdf-li-postfix: ": ";
        font-size: 12pt;
        font-weight: bold;
    }
    .policy-item {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 10pt;
        font-weight: normal;
    }
</style>
<body>
    <h1>Company Policy Manual</h1>
    <ol class="policy">
        <li>Employee Conduct
            <ol class="policy-item">
                <li>Professional behavior standards</li>
                <li>Dress code requirements</li>
                <li>Communication guidelines</li>
            </ol>
        </li>
        <li>Leave Policies
            <ol class="policy-item">
                <li>Annual leave entitlement</li>
                <li>Sick leave procedures</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 8: Standard Operating Procedures (SOP)

```html
<style>
    .sop-section {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ".0 ";
        font-size: 13pt;
        font-weight: bold;
        color: #1e40af;
    }
    .sop-step {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
        font-weight: normal;
        color: #000;
    }
</style>
<body>
    <h2>SOP-001: Quality Control</h2>
    <ol class="sop-section">
        <li>Initial Inspection
            <ol class="sop-step">
                <li>Visual examination</li>
                <li>Measurements recording</li>
                <li>Defect documentation</li>
            </ol>
        </li>
        <li>Testing Procedures
            <ol class="sop-step">
                <li>Equipment setup</li>
                <li>Test execution</li>
                <li>Results analysis</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 9: Project plan with phases

```html
<style>
    .phase {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-prefix: "Phase ";
        -pdf-li-postfix: ": ";
        font-size: 14pt;
        font-weight: bold;
    }
    .task {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-prefix: "Task ";
        -pdf-li-postfix: " - ";
        font-size: 11pt;
        font-weight: normal;
    }
</style>
<body>
    <h2>Project Timeline</h2>
    <ol class="phase">
        <li>Planning
            <ol class="task">
                <li>Requirements gathering</li>
                <li>Resource allocation</li>
                <li>Timeline creation</li>
            </ol>
        </li>
        <li>Development
            <ol class="task">
                <li>Design implementation</li>
                <li>Code review</li>
                <li>Unit testing</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 10: Regulatory compliance checklist

```html
<style>
    .regulation {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ".";
        font-size: 11pt;
    }
</style>
<body>
    <h2>Compliance Checklist</h2>
    <ol class="regulation">
        <li>Data Protection
            <ol class="regulation">
                <li>Data encryption implemented</li>
                <li>Access controls configured</li>
                <li>Audit logs enabled
                    <ol class="regulation">
                        <li>User actions tracked</li>
                        <li>System events logged</li>
                        <li>Reports generated monthly</li>
                    </ol>
                </li>
            </ol>
        </li>
        <li>Security Measures</li>
    </ol>
</body>
```

### Example 11: Research paper outline

```html
<style>
    .paper-section {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 12pt;
        font-weight: bold;
    }
    .paper-subsection {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
        font-weight: 600;
    }
    .paper-point {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 10pt;
        font-weight: normal;
    }
</style>
<body>
    <h2>Research Paper Structure</h2>
    <ol class="paper-section">
        <li>Introduction
            <ol class="paper-subsection">
                <li>Background
                    <ol class="paper-point">
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

### Example 12: Training manual chapters

```html
<style>
    .chapter {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-prefix: "Chapter ";
        -pdf-li-postfix: ": ";
        font-size: 14pt;
        font-weight: bold;
        margin-top: 15pt;
    }
    .section {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
        font-weight: normal;
    }
</style>
<body>
    <h1>Training Manual</h1>
    <ol class="chapter">
        <li>Getting Started
            <ol class="section">
                <li>Installation</li>
                <li>Configuration</li>
                <li>First steps</li>
            </ol>
        </li>
        <li>Advanced Features
            <ol class="section">
                <li>Custom styling</li>
                <li>Integration options</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 13: API documentation structure

```html
<style>
    .api-category {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 12pt;
        font-weight: bold;
        color: #059669;
    }
    .api-method {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 10pt;
        font-weight: normal;
        color: #000;
    }
</style>
<body>
    <h2>API Reference</h2>
    <ol class="api-category">
        <li>Authentication Methods
            <ol class="api-method">
                <li>POST /api/login</li>
                <li>POST /api/logout</li>
                <li>GET /api/verify</li>
            </ol>
        </li>
        <li>User Management
            <ol class="api-method">
                <li>GET /api/users</li>
                <li>POST /api/users</li>
                <li>PUT /api/users/:id</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 14: Bill of Materials (BOM)

```html
<style>
    .bom-assembly {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ".";
        font-size: 11pt;
        font-weight: bold;
    }
    .bom-part {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 10pt;
        font-weight: normal;
    }
</style>
<body>
    <h2>Bill of Materials</h2>
    <ol class="bom-assembly">
        <li>Main Assembly
            <ol class="bom-part">
                <li>Housing - Part #12345</li>
                <li>Circuit Board - Part #12346
                    <ol class="bom-part">
                        <li>Resistor 10kΩ - Part #12347</li>
                        <li>Capacitor 100µF - Part #12348</li>
                    </ol>
                </li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 15: Change log documentation

```html
<style>
    .version {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-prefix: "v";
        -pdf-li-postfix: " - ";
        font-size: 12pt;
        font-weight: bold;
    }
    .change-type {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 10pt;
        font-weight: normal;
    }
</style>
<body>
    <h2>Version History</h2>
    <ol class="version">
        <li>Major Release
            <ol class="change-type">
                <li>Added new list properties</li>
                <li>Improved rendering performance</li>
                <li>Fixed memory leak issue</li>
            </ol>
        </li>
        <li>Minor Update
            <ol class="change-type">
                <li>Updated documentation</li>
                <li>Bug fixes</li>
            </ol>
        </li>
    </ol>
</body>
```

---

## See Also

- [list-style](/reference/cssproperties/css_prop_list-style) - List style shorthand
- [list-style-type](/reference/cssproperties/css_prop_list-style-type) - Type of list marker
- [-pdf-li-concat](/reference/cssproperties/css_prop_-pdf-li-concat) - Scryber custom: List number concatenation
- [-pdf-li-align](/reference/cssproperties/css_prop_-pdf-li-align) - Scryber custom: List marker alignment
- [-pdf-li-inset](/reference/cssproperties/css_prop_-pdf-li-inset) - Scryber custom: List content inset
- [-pdf-li-prefix](/reference/cssproperties/css_prop_-pdf-li-prefix) - Scryber custom: List marker prefix
- [-pdf-li-postfix](/reference/cssproperties/css_prop_-pdf-li-postfix) - Scryber custom: List marker postfix
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
