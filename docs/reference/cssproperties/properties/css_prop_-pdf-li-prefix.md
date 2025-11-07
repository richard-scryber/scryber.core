---
layout: default
title: -pdf-li-prefix
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# -pdf-li-prefix : List Item Prefix Property (Scryber Custom)

The `-pdf-li-prefix` property is a Scryber-specific extension that adds custom text before list item markers. This property enables advanced list formatting by allowing you to prepend text, symbols, or labels to list numbers or bullets, creating professional documentation with custom marker styles like "Item 1:", "Step A)", or "Section 2.1:".

## Usage

```css
selector {
    -pdf-li-prefix: "text";
}
```

The value is a string that will be displayed before the list marker.

---

## Supported Values

- **String values** - Any text enclosed in quotes
  - `"Item "` - Adds "Item " before the marker
  - `"Step "` - Adds "Step " before the marker
  - `"Section "` - Adds "Section " before the marker
  - `"("` - Adds opening parenthesis before the marker
  - `"["` - Adds opening bracket before the marker
- **Empty string** `""` - No prefix (default)

### Default Value

- **""** - No prefix text

---

## Supported Elements

The `-pdf-li-prefix` property can be applied to:
- Ordered lists (`<ol>`)
- Unordered lists (`<ul>`)
- Individual list items (`<li>`)

When applied to list containers, the prefix is inherited by all child list items.

---

## Notes

- This is a Scryber-specific property designed for advanced PDF list formatting
- The prefix text appears before the list marker (number, bullet, letter, etc.)
- Commonly used with `-pdf-li-postfix` to create complete custom marker formats
- Useful for creating labeled lists like "Step 1:", "Question 2:", "Item A)"
- Works with all list-style-type values
- The prefix is part of the marker display, not the content
- Can include spaces for proper formatting
- Multiple characters and symbols are supported
- Consider combining with `-pdf-li-align` for proper visual alignment
- Prefixes are inherited by nested lists unless overridden

---

## Data Binding

The `-pdf-li-prefix` property supports data binding, enabling dynamic prefix text based on document type, language, or organizational conventions. This powerful feature allows documents to automatically adapt their list marker labeling to different contexts and requirements.

### Example 1: Dynamic prefix based on list type

```html
<style>
    .dynamic-prefix-list {
        list-style-type: decimal;
        -pdf-li-prefix: "{{listConfig.prefix}}";
        -pdf-li-postfix: "{{listConfig.postfix}}";
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{listConfig.title}}</h2>
    <ol class="dynamic-prefix-list">
        <li>{{items[0]}}</li>
        <li>{{items[1]}}</li>
        <li>{{items[2]}}</li>
    </ol>
</body>
```

**Data context:**
```json
{
    "listConfig": {
        "title": "Project Tasks",
        "prefix": "Task ",
        "postfix": ": "
    },
    "items": [
        "Requirements gathering",
        "Design implementation",
        "Testing and deployment"
    ]
}
```

Different list types (tasks, steps, requirements, questions) can have appropriate prefixes applied automatically based on the document context.

### Example 2: Multilingual prefixes

```html
<style>
    .localized-prefix {
        list-style-type: decimal;
        -pdf-li-prefix: "{{locale.stepPrefix}}";
        -pdf-li-postfix: "{{locale.stepPostfix}}";
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{locale.title}}</h2>
    <ol class="localized-prefix">
        <li>{{locale.steps[0]}}</li>
        <li>{{locale.steps[1]}}</li>
        <li>{{locale.steps[2]}}</li>
    </ol>
</body>
```

**Data context (English):**
```json
{
    "locale": {
        "title": "Installation Steps",
        "stepPrefix": "Step ",
        "stepPostfix": ": ",
        "steps": [
            "Download the software",
            "Run the installer",
            "Complete configuration"
        ]
    }
}
```

**Data context (Spanish):**
```json
{
    "locale": {
        "title": "Pasos de Instalación",
        "stepPrefix": "Paso ",
        "stepPostfix": ": ",
        "steps": [
            "Descargar el software",
            "Ejecutar el instalador",
            "Completar la configuración"
        ]
    }
}
```

List prefixes automatically adapt to the document language, ensuring proper localization of marker labels.

### Example 3: Document identifier prefixes

```html
<style>
    .requirement-list {
        list-style-type: decimal;
        -pdf-li-prefix: "{{project.requirementPrefix}}";
        -pdf-li-postfix: ": ";
        font-size: 11pt;
    }
    .test-case-list {
        list-style-type: decimal;
        -pdf-li-prefix: "{{project.testCasePrefix}}";
        -pdf-li-postfix: ": ";
        font-size: 11pt;
    }
</style>
<body>
    <h2>{{project.name}} - Requirements</h2>
    <ol class="requirement-list">
        <li>{{requirements[0]}}</li>
        <li>{{requirements[1]}}</li>
    </ol>

    <h2>{{project.name}} - Test Cases</h2>
    <ol class="test-case-list">
        <li>{{testCases[0]}}</li>
        <li>{{testCases[1]}}</li>
    </ol>
</body>
```

**Data context:**
```json
{
    "project": {
        "name": "Project Alpha",
        "requirementPrefix": "ALPHA-REQ-",
        "testCasePrefix": "ALPHA-TC-"
    },
    "requirements": [
        "System shall authenticate users",
        "System shall validate input data"
    ],
    "testCases": [
        "Verify login with valid credentials",
        "Verify input validation error messages"
    ]
}
```

Project-specific identifier prefixes ensure traceability across requirements, test cases, and other document artifacts. Each project can have its own naming convention.

---

## Examples

### Example 1: Basic "Item" prefix

```html
<style>
    .item-prefix {
        list-style-type: decimal;
        -pdf-li-prefix: "Item ";
        font-size: 12pt;
    }
</style>
<body>
    <ol class="item-prefix">
        <li>First item (displays as "Item 1")</li>
        <li>Second item (displays as "Item 2")</li>
        <li>Third item (displays as "Item 3")</li>
    </ol>
</body>
```

### Example 2: Step-by-step procedure

```html
<style>
    .step-list {
        list-style-type: decimal;
        -pdf-li-prefix: "Step ";
        -pdf-li-postfix: ": ";
        font-size: 11pt;
    }
    .step-list li {
        margin-bottom: 10pt;
    }
</style>
<body>
    <h2>Installation Steps</h2>
    <ol class="step-list">
        <li>Download the installation package</li>
        <li>Extract files to destination folder</li>
        <li>Run the setup wizard</li>
        <li>Configure initial settings</li>
        <li>Complete the installation</li>
    </ol>
</body>
```

### Example 3: Question numbering

```html
<style>
    .question-list {
        list-style-type: decimal;
        -pdf-li-prefix: "Q";
        -pdf-li-postfix: ": ";
        font-size: 12pt;
        font-weight: bold;
    }
    .question-list li {
        margin-bottom: 15pt;
    }
</style>
<body>
    <h2>Exam Questions</h2>
    <ol class="question-list">
        <li>What is the capital of France?</li>
        <li>Who wrote "Romeo and Juliet"?</li>
        <li>What is the chemical symbol for gold?</li>
    </ol>
</body>
```

### Example 4: Section labeling

```html
<style>
    .section-list {
        list-style-type: decimal;
        -pdf-li-prefix: "Section ";
        -pdf-li-postfix: ": ";
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 12pt;
        font-weight: bold;
    }
</style>
<body>
    <h2>Document Sections</h2>
    <ol class="section-list">
        <li>Introduction
            <ol class="section-list">
                <li>Background</li>
                <li>Objectives</li>
            </ol>
        </li>
        <li>Main Content</li>
    </ol>
</body>
```

### Example 5: Parenthesized numbers

```html
<style>
    .parenthesized {
        list-style-type: decimal;
        -pdf-li-prefix: "(";
        -pdf-li-postfix: ") ";
        font-size: 11pt;
    }
</style>
<body>
    <ol class="parenthesized">
        <li>First item (displays as "(1)")</li>
        <li>Second item (displays as "(2)")</li>
        <li>Third item (displays as "(3)")</li>
    </ol>
</body>
```

### Example 6: Alphabetic with prefix

```html
<style>
    .option-list {
        list-style-type: upper-alpha;
        -pdf-li-prefix: "Option ";
        -pdf-li-postfix: ": ";
        font-size: 11pt;
    }
</style>
<body>
    <h3>Choose your preferred method:</h3>
    <ol class="option-list">
        <li>Email notification</li>
        <li>SMS notification</li>
        <li>Push notification</li>
        <li>No notification</li>
    </ol>
</body>
```

### Example 7: Task numbering

```html
<style>
    .task-list {
        list-style-type: decimal;
        -pdf-li-prefix: "Task #";
        -pdf-li-postfix: " - ";
        font-size: 11pt;
    }
</style>
<body>
    <h2>Project Tasks</h2>
    <ol class="task-list">
        <li>Requirements gathering</li>
        <li>Design mockups</li>
        <li>Implementation</li>
        <li>Testing</li>
        <li>Deployment</li>
    </ol>
</body>
```

### Example 8: Chapter numbering

```html
<style>
    .chapter-list {
        list-style-type: upper-roman;
        -pdf-li-prefix: "Chapter ";
        -pdf-li-postfix: " - ";
        font-size: 14pt;
        font-weight: bold;
    }
    .chapter-list li {
        margin-bottom: 15pt;
    }
</style>
<body>
    <h1>Book Contents</h1>
    <ol class="chapter-list">
        <li>The Beginning</li>
        <li>The Journey</li>
        <li>The Challenge</li>
        <li>The Resolution</li>
        <li>The End</li>
    </ol>
</body>
```

### Example 9: Article sections

```html
<style>
    .article-section {
        list-style-type: decimal;
        -pdf-li-prefix: "Article ";
        -pdf-li-postfix: ". ";
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 11pt;
    }
</style>
<body>
    <h2>Company Bylaws</h2>
    <ol class="article-section">
        <li>General Provisions
            <ol class="article-section">
                <li>Name and purpose</li>
                <li>Registered office</li>
            </ol>
        </li>
        <li>Membership
            <ol class="article-section">
                <li>Eligibility</li>
                <li>Rights and duties</li>
            </ol>
        </li>
    </ol>
</body>
```

### Example 10: Figure references

```html
<style>
    .figure-list {
        list-style-type: decimal;
        -pdf-li-prefix: "Figure ";
        -pdf-li-postfix: ": ";
        font-size: 10pt;
        font-style: italic;
    }
</style>
<body>
    <h3>List of Figures</h3>
    <ol class="figure-list">
        <li>System architecture diagram</li>
        <li>Database schema</li>
        <li>User interface mockup</li>
        <li>Deployment workflow</li>
    </ol>
</body>
```

### Example 11: Requirement identifiers

```html
<style>
    .requirement {
        list-style-type: decimal;
        -pdf-li-prefix: "REQ-";
        -pdf-li-postfix: ": ";
        font-size: 11pt;
    }
    .requirement li {
        margin-bottom: 8pt;
    }
</style>
<body>
    <h2>Functional Requirements</h2>
    <ol class="requirement">
        <li>The system shall authenticate users</li>
        <li>The system shall log all transactions</li>
        <li>The system shall generate reports</li>
        <li>The system shall support data export</li>
    </ol>
</body>
```

### Example 12: Exercise numbering

```html
<style>
    .exercise {
        list-style-type: decimal;
        -pdf-li-prefix: "Exercise ";
        -pdf-li-postfix: " - ";
        font-size: 12pt;
        font-weight: bold;
    }
    .exercise li {
        margin-bottom: 20pt;
    }
</style>
<body>
    <h2>Practice Exercises</h2>
    <ol class="exercise">
        <li>Calculate the area of a circle with radius 5cm</li>
        <li>Convert 100 degrees Fahrenheit to Celsius</li>
        <li>Solve the equation: 2x + 5 = 15</li>
    </ol>
</body>
```

### Example 13: Test case identifiers

```html
<style>
    .test-case {
        list-style-type: decimal;
        -pdf-li-prefix: "TC-";
        -pdf-li-postfix: ": ";
        font-size: 10pt;
        font-family: 'Courier New', monospace;
    }
</style>
<body>
    <h3>Test Cases</h3>
    <ol class="test-case">
        <li>Verify user login with valid credentials</li>
        <li>Verify user login with invalid password</li>
        <li>Verify password reset functionality</li>
        <li>Verify session timeout behavior</li>
    </ol>
</body>
```

### Example 14: Appendix labeling

```html
<style>
    .appendix {
        list-style-type: upper-alpha;
        -pdf-li-prefix: "Appendix ";
        -pdf-li-postfix: " - ";
        font-size: 13pt;
        font-weight: bold;
    }
</style>
<body>
    <h2>Appendices</h2>
    <ol class="appendix">
        <li>Technical Specifications</li>
        <li>Glossary of Terms</li>
        <li>Bibliography</li>
        <li>Index</li>
    </ol>
</body>
```

### Example 15: Phase identification in project plan

```html
<style>
    .phase {
        list-style-type: decimal;
        -pdf-li-prefix: "Phase ";
        -pdf-li-postfix: ": ";
        -pdf-li-group: true;
        -pdf-li-concat: true;
        font-size: 12pt;
        font-weight: bold;
        color: #1e40af;
    }
    .milestone {
        list-style-type: decimal;
        -pdf-li-prefix: "Milestone ";
        -pdf-li-postfix: " - ";
        font-size: 11pt;
        font-weight: normal;
        color: #000;
    }
</style>
<body>
    <h2>Project Timeline</h2>
    <ol class="phase">
        <li>Planning
            <ol class="milestone">
                <li>Requirements completed</li>
                <li>Budget approved</li>
                <li>Team assembled</li>
            </ol>
        </li>
        <li>Development
            <ol class="milestone">
                <li>Alpha release</li>
                <li>Beta release</li>
                <li>Feature complete</li>
            </ol>
        </li>
        <li>Deployment
            <ol class="milestone">
                <li>Staging deployment</li>
                <li>Production deployment</li>
                <li>Go-live completed</li>
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
- [-pdf-li-inset](/reference/cssproperties/css_prop_-pdf-li-inset) - Scryber custom: List content inset
- [-pdf-li-postfix](/reference/cssproperties/css_prop_-pdf-li-postfix) - Scryber custom: List marker postfix
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
