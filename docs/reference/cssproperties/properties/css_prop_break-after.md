---
layout: default
title: break-after
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# break-after : Break After Property

The `break-after` property controls page and column breaks after an element when generating PDF documents. This is the modern CSS3 replacement for `page-break-after`, offering more control including column breaks for multi-column layouts. It ensures specific content ends before a break, creating clear document structure and proper section separation.

## Usage

```css
selector {
    break-after: value;
}
```

The break-after property provides precise control over pagination and column layout by forcing or avoiding breaks immediately following specific elements in your PDF document.

---

## Supported Values

### auto (default)
Automatic breaking behavior. The browser/PDF generator determines where page and column breaks occur based on content flow and available space. This is the default value.

### always
Forces a break (page or column, depending on context) after the element. Content following the element will always start on a new page or column.

### avoid
Attempts to avoid any break after the element. The PDF generator will try to keep the following content on the same page, though this may not always be possible with large content.

### page
Forces a page break after the element. Similar to `always` but explicitly specifies a page break (not a column break in multi-column layouts).

### left
Forces one or two page breaks after the element so that the next content appears on a left-hand page. Useful for double-sided printing.

### right
Forces one or two page breaks after the element so that the next content appears on a right-hand page. Commonly used for double-sided documents.

### column
Forces a column break after the element in multi-column layouts. The next content will start at the top of the next column. If already in the last column, it forces a page break.

---

## Supported Elements

The `break-after` property can be applied to:
- Block-level elements (`<div>`, `<section>`, `<article>`)
- Headings (`<h1>` through `<h6>`)
- Paragraphs (`<p>`)
- Lists (`<ul>`, `<ol>`)
- Tables (`<table>`)
- Images (`<img>`)
- Any block container element

---

## Notes

- This is the modern CSS3 property that replaces `page-break-after`
- The `auto` value is the default and allows natural breaking
- Using `always` or `page` guarantees content following the element starts on a new page
- The `column` value is specifically for multi-column layouts
- The `avoid` value is a suggestion and may be overridden if content is too large
- Left and right values are designed for double-sided (duplex) printing
- When using `left` or `right`, blank pages may be inserted to achieve the desired page position
- Commonly used after title pages, cover pages, and section summaries
- Can be combined with `break-before` and `break-inside` for complete control
- More flexible than the legacy `page-break-after` property

---

## Data Binding

The `break-after` property supports data binding, enabling dynamic control of page and column breaks following elements based on content type, configuration, or document structure. This provides powerful templating capabilities for complex document generation.

### Example 1: Conditional breaks after content blocks

```html
<style>
    .summary-block {
        break-after: {{block.isolate ? 'page' : 'auto'}};
        background-color: {{block.emphasize ? '#eff6ff' : 'transparent'}};
        padding: {{block.emphasize ? '20pt' : '0'}};
    }
    .block-header {
        font-size: {{block.headerSize || 20}}pt;
        font-weight: bold;
        margin-bottom: 15pt;
    }
</style>
<body>
    {{#each summaryBlocks}}
    <div class="summary-block">
        <h2 class="block-header">{{blockTitle}}</h2>
        <div>{{blockContent}}</div>
    </div>
    {{/each}}
</body>
```

### Example 2: Data-driven chart isolation

```html
<style>
    .chart-container {
        break-after: {{chartSettings.onePerPage ? 'page' : 'avoid'}};
        page-break-inside: avoid;
        padding: {{chartSettings.padding}}pt;
    }
    .chart-title {
        font-size: {{chartSettings.titleSize || 18}}pt;
        font-weight: bold;
        margin-bottom: 15pt;
        text-align: center;
    }
    .chart-notes {
        margin-top: 15pt;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    {{#each charts}}
    <div class="chart-container">
        <h2 class="chart-title">{{chartTitle}}</h2>
        <div class="chart-placeholder" style="height: {{chartHeight}}pt;">
            [Chart: {{chartType}}]
        </div>
        <div class="chart-notes">{{notes}}</div>
    </div>
    {{/each}}
</body>
```

### Example 3: Variable document sections with mixed breaks

```html
<style>
    @page standard {
        size: A4 portrait;
        margin: 25mm;
    }
    @page wide {
        size: A4 landscape;
        margin: 20mm;
    }

    .document-section {
        break-after: {{section.breakType || 'auto'}};
        page: {{section.orientation === 'landscape' ? 'wide' : 'standard'}};
    }
    .section-header {
        background-color: {{section.color || '#1e3a8a'}};
        color: white;
        padding: 15pt;
        font-size: 20pt;
        margin-bottom: 20pt;
    }
</style>
<body>
    {{#each documentSections}}
    <div class="document-section">
        <div class="section-header">{{sectionTitle}}</div>
        <div class="section-body">{{sectionContent}}</div>
    </div>
    {{/each}}
</body>
```

---

## Examples

### Example 1: Cover page with break after

```html
<style>
    .cover-page {
        break-after: page;
        height: 100vh;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        text-align: center;
    }
    .cover-title {
        font-size: 52pt;
        font-weight: bold;
        margin-bottom: 20pt;
        text-shadow: 2pt 2pt 4pt rgba(0,0,0,0.3);
    }
    .cover-subtitle {
        font-size: 28pt;
        margin-bottom: 40pt;
    }
    .cover-date {
        font-size: 18pt;
        opacity: 0.9;
    }
</style>
<body>
    <div class="cover-page">
        <h1 class="cover-title">Strategic Business Plan</h1>
        <p class="cover-subtitle">Five-Year Growth Initiative</p>
        <p class="cover-date">March 2025</p>
    </div>

    <div>
        <h2>Table of Contents</h2>
        <p>Content begins on the next page...</p>
    </div>
</body>
```

### Example 2: Multi-column layout with column breaks

```html
<style>
    .magazine-layout {
        column-count: 2;
        column-gap: 30pt;
        column-rule: 1pt solid #e5e7eb;
    }
    .article-end {
        break-after: column;
        text-align: center;
        font-size: 24pt;
        color: #d1d5db;
        margin: 20pt 0;
    }
    .article-title {
        font-size: 18pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 15pt 0 10pt 0;
    }
</style>
<body>
    <div class="magazine-layout">
        <h2 class="article-title">Technology Trends</h2>
        <p>The technology landscape continues to evolve at a rapid pace.
        New innovations in artificial intelligence, cloud computing, and
        cybersecurity are reshaping how businesses operate.</p>
        <div class="article-end">◆◆◆</div>

        <h2 class="article-title">Market Analysis</h2>
        <p>Recent market data shows strong performance across multiple
        sectors. Consumer confidence remains high, and investment
        opportunities continue to emerge.</p>
        <div class="article-end">◆◆◆</div>

        <h2 class="article-title">Future Outlook</h2>
        <p>Looking ahead to the next quarter, analysts predict continued
        growth with some sector-specific challenges.</p>
    </div>
</body>
```

### Example 3: Title pages with right-hand breaks

```html
<style>
    .part-title {
        break-after: right;
        height: 100vh;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        background-color: #f9fafb;
    }
    .part-ornament {
        font-size: 96pt;
        color: #d1d5db;
        margin-bottom: 40pt;
    }
    .part-label {
        font-size: 18pt;
        text-transform: uppercase;
        letter-spacing: 4pt;
        color: #9ca3af;
        margin-bottom: 20pt;
    }
    .part-name {
        font-size: 48pt;
        font-weight: bold;
        color: #1f2937;
        font-family: serif;
    }
</style>
<body>
    <div class="part-title">
        <div class="part-ornament">✵</div>
        <div class="part-label">Part One</div>
        <div class="part-name">The Beginning</div>
    </div>

    <div>
        <h1>Chapter 1</h1>
        <p>Chapter content starts on right-hand page...</p>
    </div>

    <div class="part-title">
        <div class="part-ornament">✵</div>
        <div class="part-label">Part Two</div>
        <div class="part-name">The Journey</div>
    </div>

    <div>
        <h1>Chapter 5</h1>
        <p>Next section content...</p>
    </div>
</body>
```

### Example 4: Summary boxes with page breaks

```html
<style>
    .chapter-summary {
        break-after: page;
        background-color: #fef3c7;
        border: 3pt solid #f59e0b;
        border-radius: 10pt;
        padding: 25pt;
        margin: 30pt 0;
    }
    .summary-title {
        font-size: 20pt;
        font-weight: bold;
        color: #92400e;
        margin-bottom: 15pt;
        text-align: center;
    }
    .summary-points {
        list-style-type: none;
        padding: 0;
    }
    .summary-points li {
        padding: 10pt 0;
        padding-left: 30pt;
        position: relative;
        line-height: 1.6;
    }
    .summary-points li:before {
        content: "→";
        position: absolute;
        left: 0;
        color: #f59e0b;
        font-weight: bold;
        font-size: 16pt;
    }
</style>
<body>
    <h1>Chapter 3: Advanced Techniques</h1>
    <p>This chapter explored advanced methodologies and their practical applications...</p>

    <div class="chapter-summary">
        <div class="summary-title">Chapter Summary</div>
        <ul class="summary-points">
            <li>Advanced algorithms improve processing efficiency by 40%</li>
            <li>Implementation requires careful planning and testing</li>
            <li>Performance metrics show significant improvements</li>
            <li>Best practices ensure maintainable code</li>
        </ul>
    </div>

    <h1>Chapter 4: Real-World Applications</h1>
    <p>Next chapter begins on a fresh page...</p>
</body>
```

### Example 5: Data visualizations with breaks

```html
<style>
    .chart-page {
        break-after: page;
        padding: 30pt;
    }
    .chart-header {
        background-color: #1e3a8a;
        color: white;
        padding: 15pt;
        margin-bottom: 20pt;
        border-radius: 5pt;
    }
    .chart-title {
        font-size: 20pt;
        font-weight: bold;
    }
    .chart-subtitle {
        font-size: 12pt;
        opacity: 0.9;
        margin-top: 5pt;
    }
    .chart-container {
        width: 100%;
        height: 400pt;
        background-color: #f3f4f6;
        border: 2pt solid #d1d5db;
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 20pt 0;
    }
    .chart-notes {
        background-color: #eff6ff;
        padding: 15pt;
        border-left: 4pt solid #3b82f6;
        margin-top: 20pt;
    }
</style>
<body>
    <div class="chart-page">
        <div class="chart-header">
            <div class="chart-title">Revenue by Region - Q1 2025</div>
            <div class="chart-subtitle">All figures in millions USD</div>
        </div>
        <div class="chart-container">
            [Bar Chart: Regional Revenue]
        </div>
        <div class="chart-notes">
            <strong>Key Observations:</strong> North American region shows
            strongest growth at 23%, while European market demonstrates
            steady performance at 12% growth.
        </div>
    </div>

    <div class="chart-page">
        <div class="chart-header">
            <div class="chart-title">Customer Acquisition Trends</div>
            <div class="chart-subtitle">Monthly data for 2025</div>
        </div>
        <div class="chart-container">
            [Line Chart: Customer Growth]
        </div>
    </div>
</body>
```

### Example 6: Table of contents with break after

```html
<style>
    .toc-section {
        break-after: page;
        padding: 40pt;
    }
    .toc-header {
        text-align: center;
        margin-bottom: 40pt;
    }
    .toc-title {
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
    }
    .toc-subtitle {
        font-size: 14pt;
        color: #6b7280;
    }
    .toc-entry {
        display: flex;
        justify-content: space-between;
        padding: 12pt 0;
        border-bottom: 1pt dotted #d1d5db;
    }
    .toc-entry-title {
        font-size: 13pt;
    }
    .toc-entry-page {
        font-weight: bold;
        color: #2563eb;
    }
    .toc-section-divider {
        margin: 25pt 0;
        font-size: 16pt;
        font-weight: bold;
        color: #374151;
    }
</style>
<body>
    <div class="toc-section">
        <div class="toc-header">
            <h1 class="toc-title">Table of Contents</h1>
            <p class="toc-subtitle">Company Annual Report 2025</p>
        </div>

        <div class="toc-section-divider">Part I: Overview</div>
        <div class="toc-entry">
            <span class="toc-entry-title">Executive Summary</span>
            <span class="toc-entry-page">3</span>
        </div>
        <div class="toc-entry">
            <span class="toc-entry-title">Letter from the CEO</span>
            <span class="toc-entry-page">5</span>
        </div>

        <div class="toc-section-divider">Part II: Financial Performance</div>
        <div class="toc-entry">
            <span class="toc-entry-title">Financial Highlights</span>
            <span class="toc-entry-page">7</span>
        </div>
        <div class="toc-entry">
            <span class="toc-entry-title">Balance Sheet</span>
            <span class="toc-entry-page">12</span>
        </div>
    </div>

    <div>
        <h1>Executive Summary</h1>
        <p>Main content starts here...</p>
    </div>
</body>
```

### Example 7: Assessment sections with breaks

```html
<style>
    .exam-section {
        break-after: page;
        padding: 30pt;
        border: 2pt solid #2563eb;
        border-radius: 8pt;
    }
    .section-banner {
        background-color: #2563eb;
        color: white;
        padding: 15pt;
        margin: -30pt -30pt 25pt -30pt;
        border-radius: 6pt 6pt 0 0;
    }
    .section-info {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .section-title {
        font-size: 18pt;
        font-weight: bold;
    }
    .section-points {
        font-size: 14pt;
    }
    .question-block {
        margin: 20pt 0;
        padding: 15pt;
        background-color: #f9fafb;
        border-left: 3pt solid #3b82f6;
    }
</style>
<body>
    <div class="exam-section">
        <div class="section-banner">
            <div class="section-info">
                <span class="section-title">Section A: Multiple Choice</span>
                <span class="section-points">40 Points</span>
            </div>
        </div>
        <div class="question-block">
            <strong>Question 1:</strong> Which of the following best describes...?
            <div style="margin-top: 10pt;">
                A) Option one<br/>
                B) Option two<br/>
                C) Option three<br/>
                D) Option four
            </div>
        </div>
        <div class="question-block">
            <strong>Question 2:</strong> The primary purpose of...
        </div>
    </div>

    <div class="exam-section">
        <div class="section-banner">
            <div class="section-info">
                <span class="section-title">Section B: Essay Questions</span>
                <span class="section-points">60 Points</span>
            </div>
        </div>
        <div class="question-block">
            <strong>Question 1:</strong> Discuss the impact of climate change
            on global economies. (30 points)
        </div>
    </div>
</body>
```

### Example 8: Product catalog with category breaks

```html
<style>
    .catalog-section {
        break-after: page;
    }
    .category-header {
        background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
        color: white;
        padding: 30pt;
        margin-bottom: 30pt;
        text-align: center;
        border-radius: 10pt;
    }
    .category-name {
        font-size: 36pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .category-description {
        font-size: 14pt;
        opacity: 0.95;
    }
    .product-list {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 20pt;
    }
    .product-item {
        border: 1pt solid #e5e7eb;
        padding: 15pt;
        border-radius: 8pt;
    }
    .product-name {
        font-weight: bold;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="catalog-section">
        <div class="category-header">
            <div class="category-name">Premium Electronics</div>
            <div class="category-description">
                High-performance devices for professionals
            </div>
        </div>
        <div class="product-list">
            <div class="product-item">
                <div class="product-name">Professional Laptop Pro</div>
                <p>16-inch display, 32GB RAM, 1TB SSD</p>
            </div>
            <div class="product-item">
                <div class="product-name">Wireless Headphones Elite</div>
                <p>Active noise cancellation, 30hr battery</p>
            </div>
        </div>
    </div>

    <div class="catalog-section">
        <div class="category-header">
            <div class="category-name">Smart Home Devices</div>
            <div class="category-description">
                Intelligent automation for modern living
            </div>
        </div>
        <div class="product-list">
            <div class="product-item">
                <div class="product-name">Smart Thermostat</div>
                <p>Energy-efficient climate control</p>
            </div>
        </div>
    </div>
</body>
```

### Example 9: Certificate pages with breaks

```html
<style>
    .certificate-page {
        break-after: page;
        height: 100vh;
        border: 15pt double #d4af37;
        padding: 60pt;
        background: linear-gradient(135deg, #fffef8 0%, #fefce8 100%);
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        text-align: center;
    }
    .cert-emblem {
        font-size: 72pt;
        color: #d4af37;
        margin-bottom: 30pt;
    }
    .cert-heading {
        font-size: 42pt;
        font-weight: bold;
        color: #1e3a8a;
        font-family: serif;
        margin-bottom: 30pt;
    }
    .cert-recipient-name {
        font-size: 32pt;
        font-style: italic;
        margin: 20pt 0;
        color: #1f2937;
    }
    .cert-text {
        font-size: 16pt;
        margin: 15pt 0;
        line-height: 1.6;
    }
</style>
<body>
    <div class="certificate-page">
        <div class="cert-emblem">★</div>
        <h1 class="cert-heading">Certificate of Excellence</h1>
        <p class="cert-text">This is to certify that</p>
        <p class="cert-recipient-name">Jennifer Williams</p>
        <p class="cert-text">has successfully completed the<br/>
        Advanced Project Management Course</p>
        <p class="cert-text">March 15, 2025</p>
    </div>

    <div class="certificate-page">
        <div class="cert-emblem">★</div>
        <h1 class="cert-heading">Certificate of Excellence</h1>
        <p class="cert-text">This is to certify that</p>
        <p class="cert-recipient-name">Robert Martinez</p>
        <p class="cert-text">has successfully completed the<br/>
        Advanced Project Management Course</p>
        <p class="cert-text">March 15, 2025</p>
    </div>
</body>
```

### Example 10: Glossary with alphabetical breaks

```html
<style>
    .glossary-letter-section {
        break-after: page;
        padding: 30pt;
    }
    .letter-header {
        text-align: center;
        margin-bottom: 40pt;
    }
    .letter-display {
        font-size: 96pt;
        font-weight: bold;
        color: #2563eb;
        line-height: 1;
    }
    .letter-underline {
        width: 100pt;
        height: 4pt;
        background-color: #2563eb;
        margin: 20pt auto;
    }
    .term-entry {
        margin: 20pt 0;
        padding-left: 20pt;
    }
    .term-word {
        font-size: 16pt;
        font-weight: bold;
        color: #1e40af;
        margin-bottom: 5pt;
    }
    .term-definition {
        line-height: 1.6;
        color: #374151;
    }
</style>
<body>
    <div class="glossary-letter-section">
        <div class="letter-header">
            <div class="letter-display">A</div>
            <div class="letter-underline"></div>
        </div>
        <div class="term-entry">
            <div class="term-word">Algorithm</div>
            <div class="term-definition">
                A step-by-step procedure or formula for solving a problem
                or accomplishing a task.
            </div>
        </div>
        <div class="term-entry">
            <div class="term-word">API (Application Programming Interface)</div>
            <div class="term-definition">
                A set of protocols and tools for building software applications
                that specify how components should interact.
            </div>
        </div>
    </div>

    <div class="glossary-letter-section">
        <div class="letter-header">
            <div class="letter-display">B</div>
            <div class="letter-underline"></div>
        </div>
        <div class="term-entry">
            <div class="term-word">Backend</div>
            <div class="term-definition">
                The server-side portion of an application that handles
                data processing and business logic.
            </div>
        </div>
    </div>
</body>
```

### Example 11: Form pages with breaks

```html
<style>
    .form-page {
        break-after: page;
        padding: 40pt;
    }
    .form-header {
        border-bottom: 3pt solid #1e3a8a;
        padding-bottom: 15pt;
        margin-bottom: 30pt;
    }
    .form-title {
        font-size: 24pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .form-instructions {
        font-size: 11pt;
        color: #6b7280;
        margin-top: 8pt;
    }
    .form-section {
        margin: 25pt 0;
    }
    .section-label {
        font-size: 16pt;
        font-weight: bold;
        color: #374151;
        margin-bottom: 15pt;
    }
    .form-field {
        margin: 15pt 0;
    }
    .field-label {
        font-weight: bold;
        margin-bottom: 5pt;
    }
    .field-line {
        border-bottom: 1pt solid #d1d5db;
        height: 30pt;
    }
</style>
<body>
    <div class="form-page">
        <div class="form-header">
            <h1 class="form-title">Employment Application - Page 1</h1>
            <p class="form-instructions">
                Please print clearly and complete all sections
            </p>
        </div>
        <div class="form-section">
            <div class="section-label">Personal Information</div>
            <div class="form-field">
                <div class="field-label">Full Name:</div>
                <div class="field-line"></div>
            </div>
            <div class="form-field">
                <div class="field-label">Address:</div>
                <div class="field-line"></div>
            </div>
            <div class="form-field">
                <div class="field-label">Phone Number:</div>
                <div class="field-line"></div>
            </div>
        </div>
    </div>

    <div class="form-page">
        <div class="form-header">
            <h1 class="form-title">Employment Application - Page 2</h1>
            <p class="form-instructions">
                Employment history and references
            </p>
        </div>
        <div class="form-section">
            <div class="section-label">Previous Employment</div>
            <div class="form-field">
                <div class="field-label">Company Name:</div>
                <div class="field-line"></div>
            </div>
        </div>
    </div>
</body>
```

### Example 12: Timeline events with breaks

```html
<style>
    .timeline-period {
        break-after: page;
        padding: 40pt;
    }
    .period-header {
        background-color: #7c3aed;
        color: white;
        padding: 20pt;
        margin-bottom: 30pt;
        text-align: center;
        border-radius: 8pt;
    }
    .period-years {
        font-size: 32pt;
        font-weight: bold;
    }
    .period-title {
        font-size: 18pt;
        margin-top: 10pt;
    }
    .event {
        display: flex;
        gap: 20pt;
        margin: 25pt 0;
        padding: 15pt;
        background-color: #faf5ff;
        border-left: 4pt solid #a855f7;
    }
    .event-year {
        flex: 0 0 80pt;
        font-size: 20pt;
        font-weight: bold;
        color: #7c3aed;
    }
    .event-details {
        flex: 1;
    }
    .event-title {
        font-size: 14pt;
        font-weight: bold;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="timeline-period">
        <div class="period-header">
            <div class="period-years">2000 - 2010</div>
            <div class="period-title">The Foundation Years</div>
        </div>
        <div class="event">
            <div class="event-year">2001</div>
            <div class="event-details">
                <div class="event-title">Company Founded</div>
                <p>Started with a small team of five in a garage office</p>
            </div>
        </div>
        <div class="event">
            <div class="event-year">2005</div>
            <div class="event-details">
                <div class="event-title">First Major Contract</div>
                <p>Secured partnership with Fortune 500 company</p>
            </div>
        </div>
    </div>

    <div class="timeline-period">
        <div class="period-header">
            <div class="period-years">2011 - 2020</div>
            <div class="period-title">Expansion Era</div>
        </div>
        <div class="event">
            <div class="event-year">2012</div>
            <div class="event-details">
                <div class="event-title">International Expansion</div>
                <p>Opened offices in Europe and Asia</p>
            </div>
        </div>
    </div>
</body>
```

### Example 13: Poetry or verses with breaks

```html
<style>
    .poem {
        break-after: page;
        padding: 60pt;
        text-align: center;
    }
    .poem-title {
        font-size: 28pt;
        font-weight: bold;
        margin-bottom: 10pt;
        color: #1f2937;
        font-family: serif;
    }
    .poem-author {
        font-size: 14pt;
        font-style: italic;
        color: #6b7280;
        margin-bottom: 40pt;
    }
    .stanza {
        margin: 30pt auto;
        max-width: 400pt;
        line-height: 2;
        font-size: 13pt;
        font-family: serif;
    }
    .stanza-line {
        margin: 8pt 0;
    }
</style>
<body>
    <div class="poem">
        <h1 class="poem-title">The Road Not Taken</h1>
        <p class="poem-author">Robert Frost</p>
        <div class="stanza">
            <div class="stanza-line">Two roads diverged in a yellow wood,</div>
            <div class="stanza-line">And sorry I could not travel both</div>
            <div class="stanza-line">And be one traveler, long I stood</div>
            <div class="stanza-line">And looked down one as far as I could</div>
            <div class="stanza-line">To where it bent in the undergrowth;</div>
        </div>
    </div>

    <div class="poem">
        <h1 class="poem-title">Stopping by Woods</h1>
        <p class="poem-author">Robert Frost</p>
        <div class="stanza">
            <div class="stanza-line">Whose woods these are I think I know.</div>
            <div class="stanza-line">His house is in the village though;</div>
        </div>
    </div>
</body>
```

### Example 14: Meeting agendas with breaks

```html
<style>
    .meeting-agenda {
        break-after: page;
        padding: 35pt;
    }
    .agenda-header {
        background-color: #10b981;
        color: white;
        padding: 20pt;
        margin-bottom: 25pt;
        border-radius: 5pt;
    }
    .meeting-title {
        font-size: 24pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .meeting-info {
        font-size: 12pt;
        opacity: 0.95;
    }
    .agenda-item {
        display: flex;
        gap: 15pt;
        margin: 20pt 0;
        padding: 15pt;
        background-color: #f0fdf4;
        border-left: 4pt solid #10b981;
    }
    .item-time {
        flex: 0 0 100pt;
        font-weight: bold;
        color: #065f46;
    }
    .item-details {
        flex: 1;
    }
    .item-title {
        font-size: 14pt;
        font-weight: bold;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="meeting-agenda">
        <div class="agenda-header">
            <div class="meeting-title">Q1 Strategy Meeting</div>
            <div class="meeting-info">
                March 20, 2025 | 9:00 AM - 12:00 PM | Conference Room A
            </div>
        </div>
        <div class="agenda-item">
            <div class="item-time">9:00 AM</div>
            <div class="item-details">
                <div class="item-title">Opening Remarks</div>
                <p>CEO welcome and quarterly overview</p>
            </div>
        </div>
        <div class="agenda-item">
            <div class="item-time">9:30 AM</div>
            <div class="item-details">
                <div class="item-title">Financial Review</div>
                <p>Q4 results and Q1 projections</p>
            </div>
        </div>
    </div>

    <div class="meeting-agenda">
        <div class="agenda-header">
            <div class="meeting-title">Product Launch Planning</div>
            <div class="meeting-info">
                March 25, 2025 | 2:00 PM - 4:00 PM | Conference Room B
            </div>
        </div>
        <div class="agenda-item">
            <div class="item-time">2:00 PM</div>
            <div class="item-details">
                <div class="item-title">Product Overview</div>
                <p>Features and market positioning</p>
            </div>
        </div>
    </div>
</body>
```

### Example 15: Workshop materials with exercise breaks

```html
<style>
    .workshop-exercise {
        break-after: page;
        padding: 40pt;
    }
    .exercise-header {
        display: flex;
        align-items: center;
        gap: 20pt;
        margin-bottom: 30pt;
        padding-bottom: 20pt;
        border-bottom: 3pt solid #f59e0b;
    }
    .exercise-number {
        width: 80pt;
        height: 80pt;
        background-color: #f59e0b;
        color: white;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 32pt;
        font-weight: bold;
    }
    .exercise-title {
        flex: 1;
    }
    .exercise-name {
        font-size: 24pt;
        font-weight: bold;
        color: #92400e;
    }
    .exercise-duration {
        font-size: 12pt;
        color: #78350f;
        margin-top: 5pt;
    }
    .instructions {
        background-color: #fffbeb;
        padding: 20pt;
        border-radius: 5pt;
        margin: 20pt 0;
    }
</style>
<body>
    <div class="workshop-exercise">
        <div class="exercise-header">
            <div class="exercise-number">1</div>
            <div class="exercise-title">
                <div class="exercise-name">Team Building Activity</div>
                <div class="exercise-duration">Duration: 30 minutes</div>
            </div>
        </div>
        <div class="instructions">
            <h3>Instructions:</h3>
            <ol>
                <li>Form groups of 4-5 people</li>
                <li>Each group selects a team leader</li>
                <li>Complete the challenge worksheet together</li>
                <li>Present your findings to the larger group</li>
            </ol>
        </div>
        <h3>Discussion Questions:</h3>
        <p>1. What strategies did your team use?</p>
        <p>2. How did you resolve disagreements?</p>
    </div>

    <div class="workshop-exercise">
        <div class="exercise-header">
            <div class="exercise-number">2</div>
            <div class="exercise-title">
                <div class="exercise-name">Communication Skills</div>
                <div class="exercise-duration">Duration: 45 minutes</div>
            </div>
        </div>
        <div class="instructions">
            <h3>Instructions:</h3>
            <ol>
                <li>Partner with someone you don't know well</li>
                <li>Take turns describing a complex process</li>
            </ol>
        </div>
    </div>
</body>
```

---

## See Also

- [break-before](/reference/cssproperties/css_prop_break-before) - Control breaks before elements
- [break-inside](/reference/cssproperties/css_prop_break-inside) - Control breaks within elements
- [page-break-after](/reference/cssproperties/css_prop_page-break-after) - Legacy CSS2 property
- [page](/reference/cssproperties/css_prop_page) - Specify named page for element
- [@page rule](/reference/css_atrules/css_atrule_page) - Define page properties and margins

---
