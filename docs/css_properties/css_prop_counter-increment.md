---
layout: default
title: counter-increment
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# counter-increment : Counter Modification Property

The `counter-increment` property increases or decreases the value of one or more CSS counters. Counters are used with the `content` property and `counter()` or `counters()` functions to automatically number elements and create sequential content in documents.

## Usage

```css
selector {
    counter-increment: counter-name;
}
```

The counter-increment property accepts counter names and optional increment values. Multiple counters can be incremented in a single declaration.

---

## Supported Values

### Single Counter
- `counter-increment: chapter` - Increment "chapter" by 1 (default)
- `counter-increment: chapter 2` - Increment "chapter" by 2
- `counter-increment: chapter -1` - Decrement "chapter" by 1
- `counter-increment: chapter 0` - Don't change "chapter" value

### Multiple Counters
- `counter-increment: chapter section` - Increment both by 1
- `counter-increment: chapter 1 section 1` - Increment both by 1
- `counter-increment: chapter 2 section 1` - Increment "chapter" by 2, "section" by 1

### Special Keywords
- `none` - Do not increment any counters (default)

---

## Supported Elements

The `counter-increment` property applies to:
- All HTML elements
- All SVG elements
- Commonly used on elements that need numbering (headings, list items, figures, etc.)

---

## Notes

- Default value is `none` (no counters are incremented)
- Default increment value is 1 if not specified
- Increment occurs when the element is rendered
- Can increment by negative values to decrement
- Increment value of 0 effectively disables incrementing for that counter
- Counters must be initialized with `counter-reset` before incrementing
- Multiple elements can increment the same counter
- Increment happens after `counter-reset` on the same element
- Essential for automatic sequential numbering in documents
- Works with `content: counter(name)` to display counter values
- Can create complex hierarchical numbering systems
- Increment is cumulative across sibling elements
- Useful for lists, chapters, figures, tables, equations, and line numbers

---

## Data Binding

The `counter-increment` property automatically works with data-bound elements, incrementing counters for each generated item in template loops. This enables automatic sequential numbering of dynamic content.

### Example 1: Data-driven numbered list with automatic incrementing

```html
<style>
    .numbered-container { counter-reset: item; }
    .list-item {
        counter-increment: item;
        padding: 15px;
        margin: 10px 0;
        background: white;
        border: 1px solid #e5e7eb;
        border-radius: 8px;
    }
    .list-item::before {
        content: counter(item, decimal-leading-zero);
        font-size: 24px;
        font-weight: bold;
        color: #3b82f6;
        margin-right: 15px;
        display: inline-block;
        width: 40px;
    }
</style>
<body>
    <div class="numbered-container">
        <template data-bind="{{#each steps}}">
            <div class="list-item">{{instruction}}</div>
        </template>
    </div>
</body>
```

### Example 2: Dynamic FAQ with question numbering

```html
<style>
    .faq-section { counter-reset: question; }
    .faq-item {
        counter-increment: question;
        margin: 25px 0;
        padding: 20px;
        background: #f9fafb;
        border-radius: 8px;
    }
    .question-text::before {
        content: "Q" counter(question) ": ";
        font-weight: bold;
        color: #3b82f6;
        font-size: 18px;
    }
    .answer-text {
        margin-top: 10px;
        padding-left: 30px;
        color: #4b5563;
    }
</style>
<body>
    <div class="faq-section">
        <template data-bind="{{#each faqs}}">
            <div class="faq-item">
                <div class="question-text">{{question}}</div>
                <div class="answer-text">{{answer}}</div>
            </div>
        </template>
    </div>
</body>
```

### Example 3: Multi-category lists with separate counters

```html
<style>
    .content-area { counter-reset: figure table; }
    .figure-item {
        counter-increment: figure;
        margin: 15px 0;
        padding: 15px;
        border: 1px solid #d1d5db;
        border-radius: 6px;
    }
    .figure-item::before {
        content: "Figure " counter(figure) ": ";
        font-weight: bold;
        color: #1f2937;
    }
    .table-item {
        counter-increment: table;
        margin: 15px 0;
        padding: 15px;
        border: 1px solid #d1d5db;
        border-radius: 6px;
    }
    .table-item::before {
        content: "Table " counter(table) ": ";
        font-weight: bold;
        color: #1f2937;
    }
</style>
<body>
    <div class="content-area">
        <template data-bind="{{#each figures}}">
            <div class="figure-item">{{caption}}</div>
        </template>
        <template data-bind="{{#each tables}}">
            <div class="table-item">{{caption}}</div>
        </template>
    </div>
</body>
```

---

## Examples

### Example 1: Basic heading numbering

```html
<style>
    body {
        counter-reset: heading;
    }
    h2 {
        counter-increment: heading;
    }
    h2::before {
        content: counter(heading) ". ";
        color: #3b82f6;
    }
</style>
<body>
    <h2>Introduction</h2>
    <h2>Background</h2>
    <h2>Methodology</h2>
    <h2>Results</h2>
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
        counter-increment: section;
    }
    h2::before {
        content: counter(section) ". ";
    }
    h3 {
        counter-increment: subsection;
    }
    h3::before {
        content: counter(section) "." counter(subsection) " ";
    }
</style>
<body>
    <h2>First Section</h2>
    <h3>Subsection A</h3>
    <h3>Subsection B</h3>
    <h2>Second Section</h2>
    <h3>Subsection A</h3>
</body>
```

### Example 3: Figure numbering

```html
<style>
    body {
        counter-reset: figure;
    }
    figure {
        counter-increment: figure;
    }
    figcaption::before {
        content: "Figure " counter(figure) ": ";
        font-weight: bold;
        color: #1f2937;
    }
</style>
<body>
    <figure>
        <img src="chart1.png" alt="Sales Chart"/>
        <figcaption>Quarterly Sales Data</figcaption>
    </figure>
    <figure>
        <img src="chart2.png" alt="Growth Chart"/>
        <figcaption>Year-over-Year Growth</figcaption>
    </figure>
</body>
```

### Example 4: Custom list with increment by 2

```html
<style>
    .even-list {
        counter-reset: item 0;
        list-style: none;
    }
    .even-list li {
        counter-increment: item 2;
    }
    .even-list li::before {
        content: counter(item) ". ";
        font-weight: bold;
        color: #3b82f6;
    }
</style>
<body>
    <ul class="even-list">
        <li>First item (2)</li>
        <li>Second item (4)</li>
        <li>Third item (6)</li>
        <li>Fourth item (8)</li>
    </ul>
</body>
```

### Example 5: Countdown list with negative increment

```html
<style>
    .countdown {
        counter-reset: count 10;
        list-style: none;
    }
    .countdown li {
        counter-increment: count -1;
    }
    .countdown li::before {
        content: counter(count) ". ";
        font-weight: bold;
        color: #ef4444;
        font-size: 1.2em;
    }
</style>
<body>
    <ul class="countdown">
        <li>Tenth item</li>
        <li>Ninth item</li>
        <li>Eighth item</li>
        <li>Seventh item</li>
        <li>Sixth item</li>
    </ul>
</body>
```

### Example 6: Table and equation numbering

```html
<style>
    article {
        counter-reset: table equation;
    }
    .table-caption {
        counter-increment: table;
    }
    .table-caption::before {
        content: "Table " counter(table) ": ";
        font-weight: bold;
    }
    .equation {
        counter-increment: equation;
    }
    .equation::after {
        content: "(" counter(equation) ")";
        float: right;
        font-weight: bold;
    }
</style>
<body>
    <article>
        <p class="table-caption">Sales Results</p>
        <p class="equation">E = mc²</p>
        <p class="table-caption">Revenue Breakdown</p>
        <p class="equation">F = ma</p>
    </article>
</body>
```

### Example 7: Line numbering in code block

```html
<style>
    .code-block {
        counter-reset: line;
    }
    .code-line {
        counter-increment: line;
    }
    .code-line::before {
        content: counter(line, decimal-leading-zero) " ";
        color: #9ca3af;
        margin-right: 15px;
        text-align: right;
        display: inline-block;
        width: 30px;
    }
</style>
<body>
    <pre class="code-block">
<code class="code-line">function hello() {</code>
<code class="code-line">    console.log("Hello");</code>
<code class="code-line">}</code>
<code class="code-line">hello();</code>
    </pre>
</body>
```

### Example 8: Footnote references

```html
<style>
    article {
        counter-reset: footnote;
    }
    .footnote {
        counter-increment: footnote;
    }
    .footnote::after {
        content: "[" counter(footnote) "]";
        vertical-align: super;
        font-size: 0.75em;
        color: #3b82f6;
    }
</style>
<body>
    <article>
        <p>This statement<span class="footnote"></span> requires citation.</p>
        <p>Another fact<span class="footnote"></span> needs a reference.</p>
        <p>Final point<span class="footnote"></span> with source.</p>
    </article>
</body>
```

### Example 9: Multi-level outline numbering

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
        counter-reset: level3;
        margin-left: 30px;
    }
    .level2::before {
        content: counter(level1) "." counter(level2) " ";
    }
    .level3 {
        counter-increment: level3;
        margin-left: 60px;
    }
    .level3::before {
        content: counter(level1) "." counter(level2) "." counter(level3) " ";
        color: #6b7280;
    }
</style>
<body>
    <div class="outline">
        <div class="level1">Chapter One</div>
        <div class="level2">Section A</div>
        <div class="level3">Subsection i</div>
        <div class="level3">Subsection ii</div>
        <div class="level2">Section B</div>
        <div class="level1">Chapter Two</div>
        <div class="level2">Section A</div>
    </div>
</body>
```

### Example 10: Step-by-step tutorial

```html
<style>
    .tutorial {
        counter-reset: step;
    }
    .step {
        counter-increment: step;
        margin-bottom: 20px;
        padding-left: 50px;
        position: relative;
    }
    .step::before {
        content: counter(step);
        position: absolute;
        left: 0;
        top: 0;
        width: 35px;
        height: 35px;
        border-radius: 50%;
        background: #10b981;
        color: white;
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: bold;
    }
</style>
<body>
    <div class="tutorial">
        <div class="step">Download the installer</div>
        <div class="step">Run the setup wizard</div>
        <div class="step">Configure settings</div>
        <div class="step">Complete installation</div>
    </div>
</body>
```

### Example 11: Quiz questions with automatic numbering

```html
<style>
    .quiz {
        counter-reset: question;
    }
    .question {
        counter-increment: question;
        margin-bottom: 25px;
    }
    .question::before {
        content: "Question " counter(question) ": ";
        font-weight: bold;
        color: #3b82f6;
        display: block;
        margin-bottom: 8px;
    }
</style>
<body>
    <div class="quiz">
        <div class="question">What is CSS?</div>
        <div class="question">What does HTML stand for?</div>
        <div class="question">Name three JavaScript frameworks.</div>
    </div>
</body>
```

### Example 12: Article with multiple counter types

```html
<style>
    article {
        counter-reset: heading figure table;
    }
    h2 {
        counter-increment: heading;
    }
    h2::before {
        content: counter(heading) ". ";
    }
    figure {
        counter-increment: figure;
    }
    figcaption::before {
        content: "Figure " counter(figure) ": ";
        font-weight: bold;
    }
    .table-title {
        counter-increment: table;
    }
    .table-title::before {
        content: "Table " counter(table) ": ";
        font-weight: bold;
    }
</style>
<body>
    <article>
        <h2>Introduction</h2>
        <figure>
            <img src="intro.png" alt="Introduction"/>
            <figcaption>Overview diagram</figcaption>
        </figure>
        <h2>Data Analysis</h2>
        <p class="table-title">Sales Data</p>
        <figure>
            <img src="chart.png" alt="Chart"/>
            <figcaption>Sales trend</figcaption>
        </figure>
    </article>
</body>
```

### Example 13: Agenda items with custom increment

```html
<style>
    .agenda {
        counter-reset: item 0;
    }
    .agenda-item {
        counter-increment: item;
        padding: 15px;
        border-left: 4px solid #3b82f6;
        margin-bottom: 10px;
    }
    .agenda-item::before {
        content: counter(item, decimal-leading-zero) ":00 - ";
        font-weight: bold;
        color: #3b82f6;
    }
</style>
<body>
    <div class="agenda">
        <div class="agenda-item">Opening Remarks</div>
        <div class="agenda-item">Keynote Presentation</div>
        <div class="agenda-item">Panel Discussion</div>
        <div class="agenda-item">Q&A Session</div>
    </div>
</body>
```

### Example 14: Legal clauses with Roman numerals

```html
<style>
    .contract {
        counter-reset: article;
    }
    .article {
        counter-increment: article;
        margin-bottom: 20px;
    }
    .article::before {
        content: "Article " counter(article, upper-roman) ": ";
        font-weight: bold;
        color: #1f2937;
    }
</style>
<body>
    <div class="contract">
        <div class="article">Definitions and Interpretation</div>
        <div class="article">Obligations of the Parties</div>
        <div class="article">Term and Termination</div>
        <div class="article">Miscellaneous Provisions</div>
    </div>
</body>
```

### Example 15: Chapter and page numbering

```html
<style>
    body {
        counter-reset: chapter;
    }
    .chapter {
        counter-increment: chapter;
        counter-reset: page;
        page-break-before: always;
    }
    .chapter::before {
        content: "Chapter " counter(chapter);
        display: block;
        font-size: 28px;
        font-weight: bold;
        margin-bottom: 20px;
    }
    .page {
        counter-increment: page;
        min-height: 500px;
        border-bottom: 2px solid #e5e7eb;
        padding: 20px;
        position: relative;
    }
    .page::after {
        content: counter(chapter) "-" counter(page);
        position: absolute;
        bottom: 10px;
        right: 20px;
        color: #6b7280;
    }
</style>
<body>
    <div class="chapter">
        <div class="page">Chapter content page 1...</div>
        <div class="page">Chapter content page 2...</div>
    </div>
    <div class="chapter">
        <div class="page">Chapter content page 1...</div>
    </div>
</body>
```

---

## See Also

- [counter-reset](/reference/cssproperties/css_prop_counter-reset) - Initialize counters
- [content](/reference/cssproperties/css_prop_content) - Generated content with counters
- [list-style-type](/reference/cssproperties/css_prop_list-style-type) - List marker style
- [display](/reference/cssproperties/css_prop_display) - Element display type
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
