---
layout: default
title: -pdf-li-concat
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# -pdf-li-concat : List Item Concatenation Property (Scryber Custom)

The `-pdf-li-concat` property is a Scryber-specific extension that controls whether nested list item numbers are concatenated with their parent numbers to create hierarchical numbering schemes. This property is essential for creating professional documentation with compound numbering like 1.1, 1.2.3, or Section 2.3.1.

## Usage

```css
selector {
    -pdf-li-concat: value;
}
```

This property determines whether the current list level's number should be displayed concatenated with parent level numbers.

---

## Supported Values

- **true** - Concatenate with parent list numbers (e.g., 1.1, 1.2.1)
- **false** - Display only the current level number (default behavior)

### Default Value

- **false** - Each list level displays only its own number

---

## Supported Elements

The `-pdf-li-concat` property can be applied to:
- Ordered lists (`<ol>`)
- List items (`<li>`)

This property works in conjunction with `-pdf-li-group` to create hierarchical numbering systems.

---

## Notes

- This is a Scryber-specific property designed for advanced PDF list formatting
- Concatenation creates hierarchical numbering (1, 1.1, 1.1.1, etc.)
- The property must be used with `-pdf-li-group: true` to function correctly
- The separator between numbers is typically a period (.) by default
- Custom separators can be achieved using `-pdf-li-prefix` and `-pdf-li-postfix` properties
- This property is crucial for legal documents, technical specifications, and structured documentation
- Works with all list-style-type values (decimal, alpha, roman, etc.)
- The concatenation builds from the outermost to innermost list level
- Standard CSS list properties remain functional alongside this custom property

---

## Data Binding

The `-pdf-li-concat` property supports data binding, enabling dynamic control of hierarchical numbering based on document type, organizational standards, or user preferences. This allows documents to automatically adapt their numbering scheme to different contexts and requirements.

### Example 1: Dynamic hierarchical numbering

```html
<style>
    .document-structure {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: {{formatting.useHierarchical}};
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{document.title}}</h2>
    <ol class="document-structure">
        <li>{{chapters[0].title}}
            <ol class="document-structure">
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
    "document": {
        "title": "System Requirements Specification"
    },
    "formatting": {
        "useHierarchical": true
    },
    "chapters": [
        {
            "title": "Functional Requirements",
            "sections": ["User Management", "Data Processing"]
        }
    ]
}
```

When `useHierarchical` is true, produces 1, 1.1, 1.2; when false, produces 1, 1, 2. This allows organizations to switch between numbering styles for different document types.

### Example 2: Legal document numbering by jurisdiction

```html
<style>
    .legal-terms {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: {{jurisdiction.requiresHierarchicalNumbering}};
        -pdf-li-postfix: {{jurisdiction.numberPostfix}};
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{contract.title}} ({{jurisdiction.name}})</h2>
    <ol class="legal-terms">
        <li>{{clauses[0].title}}
            <ol class="legal-terms">
                <li>{{clauses[0].subclauses[0]}}</li>
                <li>{{clauses[0].subclauses[1]}}</li>
            </ol>
        </li>
    </ol>
</body>
```

**Data context:**
```json
{
    "contract": {
        "title": "Service Agreement"
    },
    "jurisdiction": {
        "name": "US Federal",
        "requiresHierarchicalNumbering": true,
        "numberPostfix": ". "
    },
    "clauses": [
        {
            "title": "General Provisions",
            "subclauses": ["Definitions", "Scope"]
        }
    ]
}
```

Different legal jurisdictions may require different numbering conventions. This approach ensures compliance with local legal document standards.

### Example 3: Standards-compliant technical documentation

```html
<style>
    .spec-section {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: {{standard.concatenateNumbers}};
        font-size: 11pt;
    }
</style>
<body>
    <h1>{{standard.name}} Compliance Document</h1>
    <ol class="spec-section">
        <li>{{requirements[0].category}}
            <ol class="spec-section">
                <li>{{requirements[0].items[0].text}}
                    <ol class="spec-section">
                        <li>{{requirements[0].items[0].details[0]}}</li>
                        <li>{{requirements[0].items[0].details[1]}}</li>
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
    "standard": {
        "name": "ISO 9001:2015",
        "concatenateNumbers": true
    },
    "requirements": [
        {
            "category": "Quality Management System",
            "items": [
                {
                    "text": "Documentation Requirements",
                    "details": [
                        "Quality manual must be maintained",
                        "Documented procedures required"
                    ]
                }
            ]
        }
    ]
}
```

Technical documentation standards (ISO, IEEE, ANSI) often mandate specific numbering formats. Data binding ensures automatic compliance with the selected standard.

---

## Examples

### Example 1: Basic hierarchical numbering

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
        <li>First item (1)
            <ol class="hierarchical">
                <li>Sub-item (1.1)</li>
                <li>Sub-item (1.2)</li>
            </ol>
        </li>
        <li>Second item (2)</li>
    </ol>
</body>
```

### Example 2: Three-level document structure

```html
<style>
    .document-structure {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
</style>
<body>
    <ol class="document-structure">
        <li>Chapter One (1)
            <ol class="document-structure">
                <li>Section (1.1)
                    <ol class="document-structure">
                        <li>Subsection (1.1.1)</li>
                        <li>Subsection (1.1.2)</li>
                    </ol>
                </li>
                <li>Section (1.2)</li>
            </ol>
        </li>
        <li>Chapter Two (2)</li>
    </ol>
</body>
```

### Example 3: Legal document numbering with periods

```html
<style>
    .legal-doc {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
    }
</style>
<body>
    <h2>Terms and Conditions</h2>
    <ol class="legal-doc">
        <li>General Provisions
            <ol class="legal-doc">
                <li>Definitions and Interpretation</li>
                <li>Scope of Agreement</li>
            </ol>
        </li>
        <li>Specific Terms
            <ol class="legal-doc">
                <li>Payment Terms</li>
                <li>Delivery Terms</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 4: Technical specification numbering

```html
<style>
    .tech-spec {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
    .spec-title {
        font-weight: bold;
        color: #1e40af;
    }
</style>
<body>
    <h2>System Requirements Specification</h2>
    <ol class="tech-spec">
        <li><span class="spec-title">Functional Requirements</span>
            <ol class="tech-spec">
                <li>User Authentication
                    <ol class="tech-spec">
                        <li>Login functionality</li>
                        <li>Password recovery</li>
                        <li>Two-factor authentication</li>
                    </ol>
                </li>
                <li>Data Management</li>
            </ol>
        </li>
        <li><span class="spec-title">Non-Functional Requirements</span></li>
    </ol>
</body>
```

### Example 5: Mixed numbering with concatenation

```html
<style>
    .mixed-level-1 {
        list-style-type: upper-alpha;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 12pt;
        font-weight: bold;
    }
    .mixed-level-2 {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
        font-weight: normal;
    }
    .mixed-level-3 {
        list-style-type: lower-alpha;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 10pt;
    }
</style>
<body>
    <ol class="mixed-level-1">
        <li>Section A
            <ol class="mixed-level-2">
                <li>Requirement A.1
                    <ol class="mixed-level-3">
                        <li>Detail A.1.a</li>
                        <li>Detail A.1.b</li>
                    </ol>
                </li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 6: Standard Operating Procedure (SOP)

```html
<style>
    .sop {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: " ";
        font-size: 11pt;
    }
</style>
<body>
    <h2>SOP-2024-001: Quality Control Process</h2>
    <ol class="sop">
        <li>Initial Inspection
            <ol class="sop">
                <li>Visual examination of product</li>
                <li>Dimensional measurements
                    <ol class="sop">
                        <li>Length verification</li>
                        <li>Width verification</li>
                        <li>Height verification</li>
                    </ol>
                </li>
                <li>Surface quality check</li>
            </ol>
        </li>
        <li>Testing Procedures
            <ol class="sop">
                <li>Equipment preparation</li>
                <li>Test execution</li>
                <li>Results documentation</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 7: Project Work Breakdown Structure (WBS)

```html
<style>
    .wbs {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
    .wbs li {
        margin-bottom: 8pt;
    }
</style>
<body>
    <h2>Project Work Breakdown Structure</h2>
    <ol class="wbs">
        <li>Project Initiation
            <ol class="wbs">
                <li>Define project scope</li>
                <li>Identify stakeholders</li>
                <li>Create project charter</li>
            </ol>
        </li>
        <li>Planning Phase
            <ol class="wbs">
                <li>Requirements analysis
                    <ol class="wbs">
                        <li>Gather requirements</li>
                        <li>Document specifications</li>
                        <li>Validate with stakeholders</li>
                    </ol>
                </li>
                <li>Resource planning</li>
            </ol>
        </li>
        <li>Execution Phase</li>
    </ol>
</body>
```

### Example 8: ISO standard document structure

```html
<style>
    .iso-standard {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
        line-height: 1.6;
    }
</style>
<body>
    <h2>ISO 9001:2015 Quality Management</h2>
    <ol class="iso-standard">
        <li>Scope
            <ol class="iso-standard">
                <li>General</li>
                <li>Application</li>
            </ol>
        </li>
        <li>Normative References</li>
        <li>Terms and Definitions
            <ol class="iso-standard">
                <li>Terms relating to organization</li>
                <li>Terms relating to quality</li>
                <li>Terms relating to process</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 9: Multi-level table of contents

```html
<style>
    .toc {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: " ";
        font-size: 11pt;
        padding-left: 0;
    }
    .toc li {
        padding: 4pt 0;
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
                <li>Chapter Overview</li>
                <li>Detailed Analysis
                    <ol class="toc">
                        <li>Methodology</li>
                        <li>Results</li>
                    </ol>
                </li>
            </ol>
        </li>
        <li>Conclusion</li>
    </ol>
</body>
```

### Example 10: RFC document structure

```html
<style>
    .rfc-section {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: ". ";
        font-size: 11pt;
    }
</style>
<body>
    <h1>RFC 9999: Example Protocol Specification</h1>
    <ol class="rfc-section">
        <li>Introduction
            <ol class="rfc-section">
                <li>Requirements Language</li>
                <li>Terminology</li>
            </ol>
        </li>
        <li>Protocol Overview
            <ol class="rfc-section">
                <li>Basic Operation</li>
                <li>Message Format
                    <ol class="rfc-section">
                        <li>Header Structure</li>
                        <li>Payload Format</li>
                    </ol>
                </li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 11: Course syllabus outline

```html
<style>
    .syllabus {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
    .week-title {
        font-weight: bold;
        color: #059669;
    }
</style>
<body>
    <h2>Course Syllabus - PDF Generation 101</h2>
    <ol class="syllabus">
        <li><span class="week-title">Week 1: Introduction</span>
            <ol class="syllabus">
                <li>What is PDF?</li>
                <li>PDF standards overview</li>
                <li>Tools and libraries</li>
            </ol>
        </li>
        <li><span class="week-title">Week 2: Basic Document Creation</span>
            <ol class="syllabus">
                <li>Document structure</li>
                <li>Text formatting
                    <ol class="syllabus">
                        <li>Fonts and typography</li>
                        <li>Colors and styles</li>
                    </ol>
                </li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 12: Audit checklist with hierarchical numbering

```html
<style>
    .audit {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
</style>
<body>
    <h2>Annual Audit Checklist</h2>
    <ol class="audit">
        <li>Financial Records
            <ol class="audit">
                <li>Revenue Documentation
                    <ol class="audit">
                        <li>Sales invoices verified</li>
                        <li>Payment records complete</li>
                        <li>Reconciliation performed</li>
                    </ol>
                </li>
                <li>Expense Documentation</li>
            </ol>
        </li>
        <li>Compliance Review
            <ol class="audit">
                <li>Regulatory requirements met</li>
                <li>Policy adherence confirmed</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 13: Software architecture document

```html
<style>
    .architecture {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
</style>
<body>
    <h2>System Architecture Document</h2>
    <ol class="architecture">
        <li>Architecture Overview
            <ol class="architecture">
                <li>System Context</li>
                <li>Design Principles</li>
            </ol>
        </li>
        <li>Component Design
            <ol class="architecture">
                <li>Presentation Layer
                    <ol class="architecture">
                        <li>User Interface Components</li>
                        <li>View Models</li>
                    </ol>
                </li>
                <li>Business Logic Layer
                    <ol class="architecture">
                        <li>Service Layer</li>
                        <li>Domain Models</li>
                    </ol>
                </li>
                <li>Data Access Layer</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 14: Risk assessment matrix

```html
<style>
    .risk {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        -pdf-li-postfix: " ";
        font-size: 11pt;
    }
    .high-risk {
        color: #dc2626;
    }
</style>
<body>
    <h2>Project Risk Assessment</h2>
    <ol class="risk">
        <li>Technical Risks
            <ol class="risk">
                <li class="high-risk">Technology obsolescence</li>
                <li>Integration challenges
                    <ol class="risk">
                        <li>API compatibility issues</li>
                        <li>Data format mismatches</li>
                    </ol>
                </li>
            </ol>
        </li>
        <li>Business Risks
            <ol class="risk">
                <li>Market changes</li>
                <li>Resource availability</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 15: User manual with hierarchical sections

```html
<style>
    .manual {
        list-style-type: decimal;
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
        line-height: 1.6;
    }
    .manual-section {
        font-weight: bold;
        margin-top: 10pt;
    }
</style>
<body>
    <h1>User Manual - PDF Generation Software</h1>
    <ol class="manual">
        <li class="manual-section">Getting Started
            <ol class="manual">
                <li>Installation
                    <ol class="manual">
                        <li>System requirements</li>
                        <li>Installation steps</li>
                        <li>Initial configuration</li>
                    </ol>
                </li>
                <li>Quick Start Guide</li>
            </ol>
        </li>
        <li class="manual-section">Advanced Features
            <ol class="manual">
                <li>Custom Styling
                    <ol class="manual">
                        <li>CSS properties</li>
                        <li>List formatting</li>
                    </ol>
                </li>
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
- [-pdf-li-align](/reference/cssproperties/css_prop_-pdf-li-align) - Scryber custom: List marker alignment
- [-pdf-li-inset](/reference/cssproperties/css_prop_-pdf-li-inset) - Scryber custom: List content inset
- [-pdf-li-prefix](/reference/cssproperties/css_prop_-pdf-li-prefix) - Scryber custom: List marker prefix
- [-pdf-li-postfix](/reference/cssproperties/css_prop_-pdf-li-postfix) - Scryber custom: List marker postfix
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
