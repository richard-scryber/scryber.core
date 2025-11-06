---
layout: default
title: counter-reset
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# counter-reset : Counter Initialization Property

The `counter-reset` property creates or resets one or more CSS counters to a specified value. Counters are used with the `content` property and `counter()` or `counters()` functions to automatically number elements, create hierarchical numbering systems, and generate sequential content.

## Usage

```css
selector {
    counter-reset: counter-name;
}
```

The counter-reset property accepts counter names and optional initial values. Multiple counters can be reset in a single declaration.

---

## Supported Values

### Single Counter
- `counter-reset: chapter` - Create/reset counter "chapter" to 0
- `counter-reset: chapter 1` - Create/reset counter "chapter" to 1
- `counter-reset: chapter 5` - Create/reset counter "chapter" to 5

### Multiple Counters
- `counter-reset: chapter section` - Reset both "chapter" and "section" to 0
- `counter-reset: chapter 1 section 0` - Reset "chapter" to 1 and "section" to 0
- `counter-reset: fig 1 table 1 equation 1` - Reset multiple counters with initial values

### Special Keywords
- `none` - Do not create or reset any counters (default)

---

## Supported Elements

The `counter-reset` property applies to:
- All HTML elements
- All SVG elements
- Particularly useful on container elements like `<body>`, `<section>`, `<div>`, `<ol>`, etc.

---

## Notes

- Default value is `none` (no counters are reset)
- Counters are reset to 0 by default if no value is specified
- Resetting a counter creates it if it doesn't exist
- Counter scope is determined by the element hierarchy
- Child elements inherit counter values from parents
- Resetting a counter in a child element creates a new scope for that counter
- Essential for automatic numbering in documents, lists, figures, and tables
- Works with `counter-increment` to create sequential numbering
- Use with `content: counter(name)` to display counter values
- Counters cascade through the document tree
- Can create hierarchical numbering systems (1, 1.1, 1.2, 2, 2.1, etc.)
- Reset occurs before any `counter-increment` on the same element

---

## Data Binding

The `counter-reset` property works seamlessly with data binding to create dynamic numbering systems that respond to data-driven content. When combined with template loops, counters automatically number generated elements.

### Example 1: Dynamic section numbering with data binding

```html
<style>
    .document { counter-reset: section; }
    .section-heading {
        counter-increment: section;
        font-size: 20px;
        font-weight: bold;
        margin: 20px 0 10px;
    }
    .section-heading::before {
        content: "Section " counter(section) ": ";
        color: #3b82f6;
    }
</style>
<body>
    <div class="document">
        <template data-bind="{{#each sections}}">
            <h2 class="section-heading">{{title}}</h2>
            <p>{{content}}</p>
        </template>
    </div>
</body>
```

### Example 2: Multi-level outline with data-driven content

```html
<style>
    .outline { counter-reset: chapter; }
    .chapter {
        counter-reset: topic;
        counter-increment: chapter;
        margin: 20px 0;
    }
    .chapter-title::before {
        content: counter(chapter) ". ";
        font-weight: bold;
        color: #1f2937;
    }
    .topic {
        counter-increment: topic;
        margin-left: 30px;
        margin: 10px 0 10px 30px;
    }
    .topic::before {
        content: counter(chapter) "." counter(topic) " ";
        color: #6b7280;
    }
</style>
<body>
    <div class="outline">
        <template data-bind="{{#each chapters}}">
            <div class="chapter">
                <h2 class="chapter-title">{{chapterName}}</h2>
                <template data-bind="{{#each topics}}">
                    <div class="topic">{{topicName}}</div>
                </template>
            </div>
        </template>
    </div>
</body>
```

### Example 3: Data-driven task list with automatic numbering

```html
<style>
    .task-list { counter-reset: task; }
    .task-item {
        counter-increment: task;
        padding: 12px;
        margin: 8px 0;
        background: #f9fafb;
        border-left: 3px solid #3b82f6;
        border-radius: 4px;
    }
    .task-item::before {
        content: "Task " counter(task) ": ";
        font-weight: bold;
        color: #3b82f6;
        margin-right: 8px;
    }
</style>
<body>
    <div class="task-list">
        <template data-bind="{{#each tasks}}">
            <div class="task-item">{{description}}</div>
        </template>
    </div>
</body>
```

---

## Examples

### Example 1: Basic chapter numbering

```html
<style>
    body {
        counter-reset: chapter;
    }
    h1::before {
        counter-increment: chapter;
        content: "Chapter " counter(chapter) ": ";
    }
</style>
<body>
    <h1>Introduction</h1>
    <h1>Getting Started</h1>
    <h1>Advanced Topics</h1>
</body>
```

### Example 2: Nested section numbering

```html
<style>
    body {
        counter-reset: section;
    }
    h2 {
        counter-reset: subsection;
    }
    h2::before {
        counter-increment: section;
        content: counter(section) ". ";
    }
    h3::before {
        counter-increment: subsection;
        content: counter(section) "." counter(subsection) " ";
    }
</style>
<body>
    <h2>First Section</h2>
    <h3>Subsection One</h3>
    <h3>Subsection Two</h3>
    <h2>Second Section</h2>
    <h3>Subsection One</h3>
</body>
```

### Example 3: Figure numbering starting at 1

```html
<style>
    body {
        counter-reset: figure 0;
    }
    figcaption::before {
        counter-increment: figure;
        content: "Figure " counter(figure) ": ";
        font-weight: bold;
    }
</style>
<body>
    <figure>
        <img src="chart.png" alt="Chart"/>
        <figcaption>Sales Data</figcaption>
    </figure>
    <figure>
        <img src="graph.png" alt="Graph"/>
        <figcaption>Growth Trends</figcaption>
    </figure>
</body>
```

### Example 4: Multiple independent counters

```html
<style>
    article {
        counter-reset: figure 0 table 0 equation 0;
    }
    .figure::before {
        counter-increment: figure;
        content: "Fig. " counter(figure) " - ";
    }
    .table::before {
        counter-increment: table;
        content: "Table " counter(table) " - ";
    }
    .equation::before {
        counter-increment: equation;
        content: "Eq. " counter(equation) " - ";
    }
</style>
<body>
    <article>
        <p class="figure">Revenue Chart</p>
        <p class="table">Quarterly Results</p>
        <p class="equation">E = mc²</p>
        <p class="figure">Growth Graph</p>
        <p class="table">Annual Summary</p>
    </article>
</body>
```

### Example 5: Custom list with reset

```html
<style>
    .custom-list {
        counter-reset: item;
        list-style: none;
    }
    .custom-list li::before {
        counter-increment: item;
        content: counter(item) ". ";
        font-weight: bold;
        color: #3b82f6;
    }
</style>
<body>
    <ul class="custom-list">
        <li>First item</li>
        <li>Second item</li>
        <li>Third item</li>
    </ul>
</body>
```

### Example 6: Resetting counters in nested sections

```html
<style>
    body {
        counter-reset: chapter;
    }
    .chapter {
        counter-reset: page 1;
    }
    .chapter::before {
        counter-increment: chapter;
        content: "Chapter " counter(chapter);
        display: block;
        font-size: 24px;
        font-weight: bold;
    }
    .page::before {
        counter-increment: page;
        content: "Page " counter(chapter) "-" counter(page);
    }
</style>
<body>
    <div class="chapter">
        <p class="page">Content...</p>
        <p class="page">Content...</p>
    </div>
    <div class="chapter">
        <p class="page">Content...</p>
        <p class="page">Content...</p>
    </div>
</body>
```

### Example 7: Table row numbering

```html
<style>
    table {
        counter-reset: row;
    }
    tbody tr::before {
        counter-increment: row;
        content: counter(row);
        display: table-cell;
        padding: 8px;
        font-weight: bold;
        background: #f3f4f6;
    }
</style>
<body>
    <table>
        <tbody>
            <tr>
                <td>Item A</td>
                <td>$100</td>
            </tr>
            <tr>
                <td>Item B</td>
                <td>$200</td>
            </tr>
            <tr>
                <td>Item C</td>
                <td>$150</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 8: Footnote numbering

```html
<style>
    article {
        counter-reset: footnote;
    }
    .footnote-ref {
        counter-increment: footnote;
    }
    .footnote-ref::after {
        content: "[" counter(footnote) "]";
        vertical-align: super;
        font-size: 0.75em;
        color: #3b82f6;
    }
</style>
<body>
    <article>
        <p>This statement requires citation<span class="footnote-ref"></span>.</p>
        <p>Another reference here<span class="footnote-ref"></span>.</p>
    </article>
</body>
```

### Example 9: Step-by-step instructions

```html
<style>
    .instructions {
        counter-reset: step;
    }
    .step::before {
        counter-increment: step;
        content: "Step " counter(step) ": ";
        font-weight: bold;
        color: #10b981;
        display: block;
        margin-bottom: 8px;
    }
</style>
<body>
    <div class="instructions">
        <div class="step">Open the application</div>
        <div class="step">Click on Settings</div>
        <div class="step">Configure your preferences</div>
        <div class="step">Save and exit</div>
    </div>
</body>
```

### Example 10: FAQ numbering

```html
<style>
    .faq {
        counter-reset: question;
    }
    .question::before {
        counter-increment: question;
        content: "Q" counter(question) ": ";
        font-weight: bold;
        color: #3b82f6;
    }
</style>
<body>
    <div class="faq">
        <p class="question">What is CSS?</p>
        <p>CSS stands for Cascading Style Sheets.</p>

        <p class="question">How do counters work?</p>
        <p>Counters are incremented and displayed using CSS properties.</p>

        <p class="question">Can I nest counters?</p>
        <p>Yes, counters can be nested for hierarchical numbering.</p>
    </div>
</body>
```

### Example 11: Multi-level outline

```html
<style>
    .outline {
        counter-reset: level1;
    }
    .level1 {
        counter-increment: level1;
        counter-reset: level2;
    }
    .level1::before {
        content: counter(level1) ". ";
        font-weight: bold;
    }
    .level2 {
        counter-increment: level2;
        margin-left: 30px;
    }
    .level2::before {
        content: counter(level1) "." counter(level2) " ";
    }
</style>
<body>
    <div class="outline">
        <div class="level1">Introduction</div>
        <div class="level2">Overview</div>
        <div class="level2">Objectives</div>
        <div class="level1">Main Content</div>
        <div class="level2">Section A</div>
        <div class="level2">Section B</div>
    </div>
</body>
```

### Example 12: Appendix numbering with letters

```html
<style>
    body {
        counter-reset: appendix;
    }
    .appendix::before {
        counter-increment: appendix;
        content: "Appendix " counter(appendix, upper-alpha) ": ";
        font-weight: bold;
    }
</style>
<body>
    <h2 class="appendix">Additional Resources</h2>
    <h2 class="appendix">Glossary</h2>
    <h2 class="appendix">References</h2>
</body>
```

### Example 13: Recipe steps with custom numbering

```html
<style>
    .recipe {
        counter-reset: step 0;
    }
    .recipe-step {
        counter-increment: step;
        padding-left: 60px;
        position: relative;
        margin-bottom: 20px;
    }
    .recipe-step::before {
        content: counter(step);
        position: absolute;
        left: 0;
        width: 40px;
        height: 40px;
        border-radius: 50%;
        background: #3b82f6;
        color: white;
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: bold;
    }
</style>
<body>
    <div class="recipe">
        <div class="recipe-step">Preheat oven to 350°F</div>
        <div class="recipe-step">Mix dry ingredients</div>
        <div class="recipe-step">Add wet ingredients</div>
        <div class="recipe-step">Bake for 30 minutes</div>
    </div>
</body>
```

### Example 14: Legal document numbering

```html
<style>
    .contract {
        counter-reset: clause;
    }
    .clause {
        counter-reset: subclause;
    }
    .clause::before {
        counter-increment: clause;
        content: counter(clause) ". ";
        font-weight: bold;
    }
    .subclause {
        margin-left: 30px;
    }
    .subclause::before {
        counter-increment: subclause;
        content: counter(clause) "." counter(subclause) " ";
    }
</style>
<body>
    <div class="contract">
        <p class="clause">Definitions</p>
        <p class="subclause">Term shall mean...</p>
        <p class="subclause">Party shall mean...</p>
        <p class="clause">Obligations</p>
        <p class="subclause">The first party agrees...</p>
        <p class="subclause">The second party agrees...</p>
    </div>
</body>
```

### Example 15: Course curriculum with module and lesson numbers

```html
<style>
    .curriculum {
        counter-reset: module;
    }
    .module {
        counter-reset: lesson;
    }
    .module-title::before {
        counter-increment: module;
        content: "Module " counter(module) ": ";
        font-weight: bold;
        color: #3b82f6;
    }
    .lesson::before {
        counter-increment: lesson;
        content: counter(module) "." counter(lesson) " ";
        color: #6b7280;
    }
</style>
<body>
    <div class="curriculum">
        <div class="module">
            <h2 class="module-title">Fundamentals</h2>
            <p class="lesson">Introduction to Programming</p>
            <p class="lesson">Variables and Data Types</p>
            <p class="lesson">Control Structures</p>
        </div>
        <div class="module">
            <h2 class="module-title">Advanced Topics</h2>
            <p class="lesson">Object-Oriented Programming</p>
            <p class="lesson">Design Patterns</p>
        </div>
    </div>
</body>
```

---

## See Also

- [counter-increment](/reference/cssproperties/css_prop_counter-increment) - Increment counter values
- [content](/reference/cssproperties/css_prop_content) - Generated content with counters
- [list-style-type](/reference/cssproperties/css_prop_list-style-type) - List marker style
- [display](/reference/cssproperties/css_prop_display) - Element display type
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
