---
layout: default
title: page-break-before
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# page-break-before : Page Break Before Property

The `page-break-before` property controls whether a page break should occur before an element when generating PDF documents. This property is essential for controlling document pagination, creating chapter breaks, and ensuring specific content starts on a new page. Note that this is a legacy CSS2 property; consider using the newer `break-before` property for more options.

## Usage

```css
selector {
    page-break-before: value;
}
```

The page-break-before property controls pagination by forcing or avoiding page breaks before specific elements in your PDF document.

---

## Supported Values

### auto (default)
Automatic page breaking behavior. The browser/PDF generator determines where page breaks occur based on content flow and available space. This is the default value.

### always
Forces a page break before the element. The element will always start on a new page, regardless of available space on the current page.

### avoid
Attempts to avoid a page break before the element. The PDF generator will try to keep the element on the same page as the preceding content, though this may not always be possible with very large content.

### left
Forces one or two page breaks before the element so that the element appears on a left-hand page. Useful for double-sided printing where chapters or sections should start on left pages.

### right
Forces one or two page breaks before the element so that the element appears on a right-hand page. Commonly used for double-sided documents where chapters should start on right-hand pages (common in books).

---

## Supported Elements

The `page-break-before` property can be applied to:
- Block-level elements (`<div>`, `<section>`, `<article>`)
- Headings (`<h1>` through `<h6>`)
- Paragraphs (`<p>`)
- Lists (`<ul>`, `<ol>`)
- Tables (`<table>`)
- Horizontal rules (`<hr>`)
- Any block container element

---

## Notes

- This is a CSS2 property; the newer `break-before` property provides more options
- The `auto` value is the default and allows natural page breaking
- Using `always` guarantees the element starts on a new page
- The `avoid` value is a suggestion and may be overridden if content is too large
- Left and right values are designed for double-sided (duplex) printing
- When using `left` or `right`, blank pages may be inserted to achieve the desired page position
- Page breaks are only applied during PDF generation, not in HTML preview
- Multiple consecutive elements with `page-break-before: always` will each start on their own page
- Margins and padding may affect the exact positioning after a page break

---

## Data Binding

The `page-break-before` property supports data binding, allowing you to dynamically control page breaks based on document data, configuration, or content type. This is particularly useful for reports, templates, and documents with variable structure.

### Example 1: Conditional page breaks for chapter starts

```html
<style>
    .section {
        page-break-before: {{item.isNewChapter ? 'always' : 'auto'}};
        margin-top: {{item.isNewChapter ? '0' : '20pt'}};
    }
    .section-title {
        font-size: {{item.isNewChapter ? '24pt' : '18pt'}};
        font-weight: bold;
    }
</style>
<body>
    {{#each sections}}
    <div class="section">
        <h2 class="section-title">{{title}}</h2>
        <p>{{content}}</p>
    </div>
    {{/each}}
</body>
```

### Example 2: Data-driven report sections

```html
<style>
    .report-section {
        page-break-before: {{config.separateSections ? 'always' : 'auto'}};
    }
    .section-header {
        background-color: {{config.headerColor}};
        color: white;
        padding: 15pt;
        font-size: 20pt;
        font-weight: bold;
    }
</style>
<body>
    {{#each reportSections}}
    <div class="report-section">
        <div class="section-header">{{sectionName}}</div>
        <div class="section-content">{{content}}</div>
    </div>
    {{/each}}
</body>
```

### Example 3: Multi-format document with conditional pagination

```html
<style>
    .document-part {
        page-break-before: {{documentType === 'formal' ? 'always' : 'auto'}};
    }
    .part-header {
        font-size: {{documentType === 'formal' ? '28pt' : '20pt'}};
        page-break-after: {{documentType === 'formal' ? 'avoid' : 'auto'}};
    }
</style>
<body>
    {{#each documentParts}}
    <div class="document-part">
        <h1 class="part-header">{{partTitle}}</h1>
        <p>{{partContent}}</p>
    </div>
    {{/each}}
</body>
```

---

## Examples

### Example 1: Chapter breaks in a book

```html
<style>
    .chapter {
        page-break-before: always;
        font-size: 24pt;
        font-weight: bold;
        margin-top: 0;
    }
</style>
<body>
    <h1 class="chapter">Chapter 1: Introduction</h1>
    <p>Content of chapter 1...</p>

    <h1 class="chapter">Chapter 2: Getting Started</h1>
    <p>Content of chapter 2...</p>

    <h1 class="chapter">Chapter 3: Advanced Topics</h1>
    <p>Content of chapter 3...</p>
</body>
```

### Example 2: Section separator with automatic page breaks

```html
<style>
    .section {
        margin: 20pt 0;
    }
    .section-title {
        font-size: 18pt;
        font-weight: bold;
        color: #1e3a8a;
    }
</style>
<body>
    <div class="section">
        <h2 class="section-title">Introduction</h2>
        <p>This section flows naturally with automatic page breaking...</p>
    </div>

    <div class="section">
        <h2 class="section-title">Methodology</h2>
        <p>Content continues on the same page if space permits...</p>
    </div>
</body>
```

### Example 3: Major sections on new pages

```html
<style>
    .major-section {
        page-break-before: always;
    }
    .section-header {
        background-color: #1e3a8a;
        color: white;
        padding: 15pt;
        font-size: 20pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="major-section">
        <div class="section-header">Executive Summary</div>
        <p>Summary content starts on a fresh page...</p>
    </div>

    <div class="major-section">
        <div class="section-header">Financial Analysis</div>
        <p>Financial data starts on its own page...</p>
    </div>

    <div class="major-section">
        <div class="section-header">Recommendations</div>
        <p>Recommendations section on a new page...</p>
    </div>
</body>
```

### Example 4: Avoiding page breaks before important headings

```html
<style>
    .important-heading {
        page-break-before: avoid;
        page-break-after: avoid;
        font-size: 16pt;
        font-weight: bold;
        color: #dc2626;
        margin: 15pt 0 10pt 0;
    }
</style>
<body>
    <p>Previous paragraph content that flows naturally...</p>

    <h3 class="important-heading">Critical Information</h3>
    <p>This heading tries to stay with the preceding content to avoid orphaning.</p>
</body>
```

### Example 5: Right-hand page starts for book chapters

```html
<style>
    .book-chapter {
        page-break-before: right;
        padding-top: 50pt;
    }
    .chapter-number {
        font-size: 36pt;
        color: #6b7280;
        font-weight: 300;
    }
    .chapter-title {
        font-size: 28pt;
        font-weight: bold;
        margin-top: 10pt;
    }
</style>
<body>
    <div class="book-chapter">
        <div class="chapter-number">Chapter 1</div>
        <h1 class="chapter-title">The Beginning</h1>
        <p>Chapter content starts on right-hand page...</p>
    </div>

    <div class="book-chapter">
        <div class="chapter-number">Chapter 2</div>
        <h1 class="chapter-title">The Journey</h1>
        <p>Each chapter begins on a right-hand page...</p>
    </div>
</body>
```

### Example 6: Left-hand page starts for appendices

```html
<style>
    .appendix {
        page-break-before: left;
    }
    .appendix-header {
        font-size: 24pt;
        font-weight: bold;
        border-bottom: 2pt solid #1e3a8a;
        padding-bottom: 10pt;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="appendix">
        <h1 class="appendix-header">Appendix A: Technical Specifications</h1>
        <p>Technical details on left-hand page...</p>
    </div>

    <div class="appendix">
        <h1 class="appendix-header">Appendix B: References</h1>
        <p>References list on left-hand page...</p>
    </div>
</body>
```

### Example 7: Invoice sections on separate pages

```html
<style>
    .invoice-summary {
        page-break-before: always;
        padding: 20pt;
        border: 2pt solid #1e3a8a;
    }
    .invoice-details {
        page-break-before: always;
    }
    .section-title {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 15pt;
    }
</style>
<body>
    <div class="invoice-summary">
        <h2 class="section-title">Invoice Summary</h2>
        <p>Total Amount Due: $5,432.10</p>
    </div>

    <div class="invoice-details">
        <h2 class="section-title">Detailed Line Items</h2>
        <table>
            <tr><td>Item 1</td><td>$100.00</td></tr>
            <tr><td>Item 2</td><td>$250.00</td></tr>
        </table>
    </div>
</body>
```

### Example 8: Report with executive summary on first page

```html
<style>
    .executive-summary {
        background-color: #f0f9ff;
        padding: 30pt;
        border-left: 5pt solid #2563eb;
    }
    .main-report {
        page-break-before: always;
    }
    .summary-title {
        font-size: 22pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 15pt;
    }
</style>
<body>
    <div class="executive-summary">
        <h1 class="summary-title">Executive Summary</h1>
        <p>Key findings and recommendations...</p>
    </div>

    <div class="main-report">
        <h1>Detailed Analysis</h1>
        <p>Full report content begins on a new page...</p>
    </div>
</body>
```

### Example 9: Questionnaire sections

```html
<style>
    .question-section {
        page-break-before: always;
        padding: 20pt;
    }
    .section-number {
        display: inline-block;
        width: 40pt;
        height: 40pt;
        background-color: #2563eb;
        color: white;
        text-align: center;
        line-height: 40pt;
        border-radius: 50%;
        font-size: 18pt;
        font-weight: bold;
    }
    .section-instructions {
        margin: 15pt 0;
        font-style: italic;
        color: #6b7280;
    }
</style>
<body>
    <div class="question-section">
        <span class="section-number">1</span>
        <h2>Personal Information</h2>
        <p class="section-instructions">Please complete all fields in this section.</p>
    </div>

    <div class="question-section">
        <span class="section-number">2</span>
        <h2>Professional Background</h2>
        <p class="section-instructions">Provide details about your work experience.</p>
    </div>
</body>
```

### Example 10: Terms and conditions with each section on new page

```html
<style>
    .terms-section {
        page-break-before: always;
    }
    .terms-heading {
        font-size: 16pt;
        font-weight: bold;
        border-bottom: 2pt solid #374151;
        padding-bottom: 8pt;
        margin-bottom: 15pt;
    }
    .terms-content {
        text-align: justify;
        line-height: 1.6;
    }
</style>
<body>
    <div class="terms-section">
        <h2 class="terms-heading">1. Definitions</h2>
        <div class="terms-content">
            <p>In these terms and conditions...</p>
        </div>
    </div>

    <div class="terms-section">
        <h2 class="terms-heading">2. Scope of Services</h2>
        <div class="terms-content">
            <p>The services provided under this agreement...</p>
        </div>
    </div>
</body>
```

### Example 11: Multi-part form with page breaks

```html
<style>
    .form-part {
        page-break-before: always;
        border: 1pt solid #d1d5db;
        padding: 25pt;
        background-color: #f9fafb;
    }
    .part-header {
        background-color: #1e3a8a;
        color: white;
        padding: 12pt;
        margin: -25pt -25pt 20pt -25pt;
        font-size: 18pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="form-part">
        <div class="part-header">Part A: Applicant Information</div>
        <p>Name: _______________________</p>
        <p>Address: _____________________</p>
    </div>

    <div class="form-part">
        <div class="part-header">Part B: Employment History</div>
        <p>Current Employer: ____________</p>
        <p>Position: ____________________</p>
    </div>

    <div class="form-part">
        <div class="part-header">Part C: References</div>
        <p>Reference 1: _________________</p>
        <p>Reference 2: _________________</p>
    </div>
</body>
```

### Example 12: Product catalog with categories on separate pages

```html
<style>
    .product-category {
        page-break-before: always;
    }
    .category-banner {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        padding: 20pt;
        font-size: 24pt;
        font-weight: bold;
        margin-bottom: 20pt;
    }
    .product-grid {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 15pt;
    }
</style>
<body>
    <div class="product-category">
        <div class="category-banner">Electronics</div>
        <div class="product-grid">
            <div>Product 1</div>
            <div>Product 2</div>
        </div>
    </div>

    <div class="product-category">
        <div class="category-banner">Home & Garden</div>
        <div class="product-grid">
            <div>Product 3</div>
            <div>Product 4</div>
        </div>
    </div>
</body>
```

### Example 13: Conference agenda with daily schedules

```html
<style>
    .conference-day {
        page-break-before: always;
    }
    .day-header {
        background-color: #fef3c7;
        border-left: 5pt solid #f59e0b;
        padding: 15pt;
        margin-bottom: 20pt;
    }
    .day-title {
        font-size: 20pt;
        font-weight: bold;
        color: #92400e;
    }
    .day-date {
        font-size: 14pt;
        color: #b45309;
    }
</style>
<body>
    <div class="conference-day">
        <div class="day-header">
            <div class="day-title">Day 1</div>
            <div class="day-date">Monday, March 15, 2025</div>
        </div>
        <p>9:00 AM - Opening Keynote</p>
        <p>10:30 AM - Workshop Session A</p>
    </div>

    <div class="conference-day">
        <div class="day-header">
            <div class="day-title">Day 2</div>
            <div class="day-date">Tuesday, March 16, 2025</div>
        </div>
        <p>9:00 AM - Technical Sessions</p>
        <p>2:00 PM - Panel Discussion</p>
    </div>
</body>
```

### Example 14: Legal document with articles on separate pages

```html
<style>
    .article {
        page-break-before: always;
        padding: 30pt;
    }
    .article-number {
        font-size: 48pt;
        color: #d1d5db;
        font-weight: 300;
        line-height: 1;
    }
    .article-title {
        font-size: 20pt;
        font-weight: bold;
        margin: 10pt 0 20pt 0;
        color: #1f2937;
    }
    .article-text {
        text-align: justify;
        line-height: 1.8;
        font-size: 11pt;
    }
</style>
<body>
    <div class="article">
        <div class="article-number">Article I</div>
        <h2 class="article-title">Scope and Purpose</h2>
        <div class="article-text">
            <p>This agreement establishes the terms...</p>
        </div>
    </div>

    <div class="article">
        <div class="article-number">Article II</div>
        <h2 class="article-title">Obligations of the Parties</h2>
        <div class="article-text">
            <p>Both parties agree to fulfill...</p>
        </div>
    </div>
</body>
```

### Example 15: Annual report with financial tables

```html
<style>
    .financial-section {
        page-break-before: always;
    }
    .section-header {
        background-color: #1e3a8a;
        color: white;
        padding: 15pt;
        font-size: 20pt;
        font-weight: bold;
        margin-bottom: 25pt;
    }
    .financial-table {
        width: 100%;
        border-collapse: collapse;
    }
    .financial-table th {
        background-color: #e5e7eb;
        padding: 10pt;
        text-align: left;
        font-weight: bold;
    }
    .financial-table td {
        padding: 8pt;
        border-bottom: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="financial-section">
        <div class="section-header">Balance Sheet</div>
        <table class="financial-table">
            <thead>
                <tr>
                    <th>Account</th>
                    <th>Amount</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Total Assets</td>
                    <td>$1,250,000</td>
                </tr>
            </tbody>
        </table>
    </div>

    <div class="financial-section">
        <div class="section-header">Income Statement</div>
        <table class="financial-table">
            <thead>
                <tr>
                    <th>Item</th>
                    <th>Amount</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Total Revenue</td>
                    <td>$850,000</td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
```

---

## See Also

- [page-break-after](/reference/cssproperties/css_prop_page-break-after) - Control page breaks after elements
- [page-break-inside](/reference/cssproperties/css_prop_page-break-inside) - Control page breaks within elements
- [break-before](/reference/cssproperties/css_prop_break-before) - Modern alternative with more options
- [break-after](/reference/cssproperties/css_prop_break-after) - Modern page and column break control
- [page](/reference/cssproperties/css_prop_page) - Specify named page for element
- [@page rule](/reference/css_atrules/css_atrule_page) - Define page properties and margins

---
