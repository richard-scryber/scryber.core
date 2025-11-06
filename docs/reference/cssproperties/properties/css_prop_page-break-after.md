---
layout: default
title: page-break-after
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# page-break-after : Page Break After Property

The `page-break-after` property controls whether a page break should occur after an element when generating PDF documents. This property is useful for ensuring specific content ends before a page break, separating sections, and controlling document flow. Note that this is a legacy CSS2 property; consider using the newer `break-after` property for more options.

## Usage

```css
selector {
    page-break-after: value;
}
```

The page-break-after property controls pagination by forcing or avoiding page breaks immediately following specific elements in your PDF document.

---

## Supported Values

### auto (default)
Automatic page breaking behavior. The browser/PDF generator determines where page breaks occur based on content flow and available space. This is the default value.

### always
Forces a page break immediately after the element. Content following the element will always start on a new page.

### avoid
Attempts to avoid a page break immediately after the element. The PDF generator will try to keep the following content on the same page, though this may not always be possible with large content.

### left
Forces one or two page breaks after the element so that the next content appears on a left-hand page. Useful for double-sided printing.

### right
Forces one or two page breaks after the element so that the next content appears on a right-hand page. Commonly used for double-sided documents.

---

## Supported Elements

The `page-break-after` property can be applied to:
- Block-level elements (`<div>`, `<section>`, `<article>`)
- Headings (`<h1>` through `<h6>`)
- Paragraphs (`<p>`)
- Lists (`<ul>`, `<ol>`)
- Tables (`<table>`)
- Images (`<img>`)
- Any block container element

---

## Notes

- This is a CSS2 property; the newer `break-after` property provides more options including column breaks
- The `auto` value is the default and allows natural page breaking
- Using `always` guarantees content following the element starts on a new page
- The `avoid` value is a suggestion and may be overridden if content is too large
- Left and right values are designed for double-sided (duplex) printing
- When using `left` or `right`, blank pages may be inserted to achieve the desired page position
- Commonly used after title pages, cover pages, and section summaries
- Can be combined with `page-break-before` for precise control
- Page breaks are only applied during PDF generation, not in HTML preview

---

## Data Binding

The `page-break-after` property supports data binding, enabling dynamic control of page breaks following elements based on data, configuration, or document structure. This is ideal for creating flexible templates that adapt to different content types and requirements.

### Example 1: Conditional page breaks after sections

```html
<style>
    .content-block {
        page-break-after: {{block.requiresNewPage ? 'always' : 'auto'}};
    }
    .block-title {
        font-size: 18pt;
        font-weight: bold;
        color: {{block.importance === 'high' ? '#dc2626' : '#1e3a8a'}};
    }
</style>
<body>
    {{#each contentBlocks}}
    <div class="content-block">
        <h2 class="block-title">{{blockTitle}}</h2>
        <div>{{blockContent}}</div>
    </div>
    {{/each}}
</body>
```

### Example 2: Dynamic summary pages

```html
<style>
    .summary-section {
        page-break-after: {{settings.summaryOnSeparatePage ? 'always' : 'auto'}};
        background-color: {{settings.highlightSummaries ? '#f0f9ff' : 'transparent'}};
        padding: {{settings.highlightSummaries ? '20pt' : '0'}};
    }
    .summary-title {
        font-size: 20pt;
        font-weight: bold;
        margin-bottom: 15pt;
    }
</style>
<body>
    {{#each sections}}
    <h1>{{sectionTitle}}</h1>
    <p>{{sectionContent}}</p>

    <div class="summary-section">
        <h2 class="summary-title">Summary</h2>
        <p>{{summaryText}}</p>
    </div>
    {{/each}}
</body>
```

### Example 3: Variable document structure with data-driven breaks

```html
<style>
    .data-table-container {
        page-break-after: {{tableConfig.isolateTables ? 'always' : 'auto'}};
    }
    .analysis-section {
        page-break-after: {{reportType === 'detailed' ? 'always' : 'avoid'}};
    }
</style>
<body>
    {{#each datasets}}
    <div class="data-table-container">
        <h2>{{datasetName}}</h2>
        <table>
            <tr><th>Metric</th><th>Value</th></tr>
            {{#each metrics}}
            <tr><td>{{name}}</td><td>{{value}}</td></tr>
            {{/each}}
        </table>
    </div>

    <div class="analysis-section">
        <h3>Analysis</h3>
        <p>{{analysis}}</p>
    </div>
    {{/each}}
</body>
```

---

## Examples

### Example 1: Cover page with page break after

```html
<style>
    .cover-page {
        page-break-after: always;
        height: 100vh;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
    }
    .cover-title {
        font-size: 48pt;
        font-weight: bold;
        margin-bottom: 20pt;
    }
    .cover-subtitle {
        font-size: 24pt;
    }
</style>
<body>
    <div class="cover-page">
        <h1 class="cover-title">Annual Report 2025</h1>
        <p class="cover-subtitle">Financial Performance & Strategic Overview</p>
    </div>

    <div>
        <h2>Table of Contents</h2>
        <p>Content begins on the next page...</p>
    </div>
</body>
```

### Example 2: Table of contents with page break after

```html
<style>
    .table-of-contents {
        page-break-after: always;
        padding: 30pt;
    }
    .toc-title {
        font-size: 28pt;
        font-weight: bold;
        border-bottom: 3pt solid #1e3a8a;
        padding-bottom: 15pt;
        margin-bottom: 25pt;
    }
    .toc-entry {
        margin: 10pt 0;
        font-size: 12pt;
    }
</style>
<body>
    <div class="table-of-contents">
        <h1 class="toc-title">Table of Contents</h1>
        <div class="toc-entry">1. Introduction ............... 3</div>
        <div class="toc-entry">2. Methodology ............... 7</div>
        <div class="toc-entry">3. Results .................. 12</div>
        <div class="toc-entry">4. Conclusion ............... 18</div>
    </div>

    <div>
        <h1>1. Introduction</h1>
        <p>The main content starts here on a fresh page...</p>
    </div>
</body>
```

### Example 3: Executive summary followed by main content

```html
<style>
    .executive-summary {
        page-break-after: always;
        background-color: #eff6ff;
        border: 2pt solid #2563eb;
        padding: 25pt;
        margin: 20pt;
    }
    .summary-heading {
        font-size: 22pt;
        font-weight: bold;
        color: #1e40af;
        margin-bottom: 15pt;
    }
    .summary-highlight {
        background-color: #dbeafe;
        padding: 12pt;
        margin: 10pt 0;
        border-left: 4pt solid #3b82f6;
    }
</style>
<body>
    <div class="executive-summary">
        <h1 class="summary-heading">Executive Summary</h1>
        <p>This report presents key findings from our annual performance review...</p>
        <div class="summary-highlight">
            <strong>Key Takeaway:</strong> Revenue increased by 23% year-over-year.
        </div>
    </div>

    <div>
        <h1>Detailed Analysis</h1>
        <p>The following sections provide in-depth analysis...</p>
    </div>
</body>
```

### Example 4: Section summaries with page breaks

```html
<style>
    .section-summary {
        page-break-after: always;
        background-color: #fef3c7;
        border-radius: 8pt;
        padding: 20pt;
        margin: 20pt 0;
    }
    .summary-title {
        font-size: 16pt;
        font-weight: bold;
        color: #92400e;
        margin-bottom: 10pt;
    }
    .key-points {
        list-style-type: disc;
        margin-left: 20pt;
    }
</style>
<body>
    <h1>Section 1: Market Analysis</h1>
    <p>Detailed market analysis content...</p>

    <div class="section-summary">
        <div class="summary-title">Section 1 Summary</div>
        <ul class="key-points">
            <li>Market growth rate: 15%</li>
            <li>Key competitors identified</li>
            <li>Emerging trends analyzed</li>
        </ul>
    </div>

    <h1>Section 2: Financial Performance</h1>
    <p>Next section begins on a new page...</p>
</body>
```

### Example 5: Charts and graphs with breaks after

```html
<style>
    .chart-container {
        page-break-after: always;
        padding: 20pt;
        text-align: center;
    }
    .chart-title {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 15pt;
    }
    .chart-placeholder {
        width: 400pt;
        height: 300pt;
        background-color: #f3f4f6;
        border: 2pt solid #9ca3af;
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 0 auto;
    }
    .chart-caption {
        margin-top: 10pt;
        font-size: 10pt;
        font-style: italic;
        color: #6b7280;
    }
</style>
<body>
    <div class="chart-container">
        <div class="chart-title">Figure 1: Revenue Trends 2020-2025</div>
        <div class="chart-placeholder">
            [Revenue Chart]
        </div>
        <div class="chart-caption">Data shows consistent growth across all quarters</div>
    </div>

    <div>
        <h2>Analysis of Revenue Trends</h2>
        <p>The chart above demonstrates...</p>
    </div>
</body>
```

### Example 6: Question groups in assessment

```html
<style>
    .question-group {
        page-break-after: always;
        padding: 25pt;
        border: 2pt solid #d1d5db;
        background-color: #fafafa;
    }
    .group-header {
        background-color: #1e3a8a;
        color: white;
        padding: 12pt;
        margin: -25pt -25pt 20pt -25pt;
        font-size: 16pt;
        font-weight: bold;
    }
    .question {
        margin: 15pt 0;
        padding-left: 15pt;
    }
</style>
<body>
    <div class="question-group">
        <div class="group-header">Questions 1-5: Multiple Choice</div>
        <div class="question">
            <strong>1.</strong> What is the capital of France?<br/>
            A) London &nbsp; B) Paris &nbsp; C) Berlin &nbsp; D) Madrid
        </div>
        <div class="question">
            <strong>2.</strong> Which planet is closest to the Sun?<br/>
            A) Venus &nbsp; B) Earth &nbsp; C) Mercury &nbsp; D) Mars
        </div>
    </div>

    <div class="question-group">
        <div class="group-header">Questions 6-10: True or False</div>
        <div class="question">
            <strong>6.</strong> The Earth is flat. (True/False)
        </div>
    </div>
</body>
```

### Example 7: Title page for book chapters

```html
<style>
    .chapter-title-page {
        page-break-after: right;
        height: 100vh;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        background-color: #f9fafb;
    }
    .chapter-ornament {
        font-size: 72pt;
        color: #d1d5db;
        margin-bottom: 30pt;
    }
    .chapter-label {
        font-size: 16pt;
        text-transform: uppercase;
        letter-spacing: 3pt;
        color: #6b7280;
        margin-bottom: 15pt;
    }
    .chapter-name {
        font-size: 36pt;
        font-weight: bold;
        color: #1f2937;
    }
</style>
<body>
    <div class="chapter-title-page">
        <div class="chapter-ornament">✦</div>
        <div class="chapter-label">Chapter One</div>
        <div class="chapter-name">The Beginning</div>
    </div>

    <div>
        <p>The chapter content begins on the right-hand page...</p>
    </div>
</body>
```

### Example 8: Full-page images with breaks

```html
<style>
    .full-page-image {
        page-break-after: always;
        width: 100%;
        text-align: center;
        padding: 30pt;
    }
    .image-title {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 20pt;
    }
    .image-placeholder {
        width: 90%;
        height: 500pt;
        background-color: #e5e7eb;
        margin: 0 auto;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 24pt;
        color: #9ca3af;
    }
    .image-description {
        margin-top: 15pt;
        font-size: 11pt;
        color: #4b5563;
        text-align: justify;
    }
</style>
<body>
    <div class="full-page-image">
        <div class="image-title">Architectural Rendering</div>
        <div class="image-placeholder">[Building Image]</div>
        <div class="image-description">
            The proposed building features modern design elements...
        </div>
    </div>

    <div>
        <h2>Design Specifications</h2>
        <p>Details follow on the next page...</p>
    </div>
</body>
```

### Example 9: Certificate with blank back page

```html
<style>
    .certificate {
        page-break-after: always;
        width: 100%;
        height: 100vh;
        border: 10pt double #d4af37;
        padding: 50pt;
        background-color: #fffef8;
        text-align: center;
    }
    .cert-title {
        font-size: 42pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 50pt 0 30pt 0;
        font-family: serif;
    }
    .cert-recipient {
        font-size: 28pt;
        margin: 30pt 0;
        font-style: italic;
    }
    .cert-description {
        font-size: 16pt;
        margin: 20pt 0;
    }
</style>
<body>
    <div class="certificate">
        <div class="cert-title">Certificate of Achievement</div>
        <p class="cert-description">This is to certify that</p>
        <p class="cert-recipient">John Smith</p>
        <p class="cert-description">has successfully completed the Professional Development Course</p>
    </div>

    <div>
        <h1>Continuing Education Credits</h1>
        <p>Additional information follows...</p>
    </div>
</body>
```

### Example 10: Glossary entries with page breaks

```html
<style>
    .glossary-section {
        page-break-after: always;
    }
    .glossary-letter {
        font-size: 48pt;
        font-weight: bold;
        color: #2563eb;
        border-bottom: 3pt solid #2563eb;
        padding-bottom: 10pt;
        margin-bottom: 25pt;
    }
    .glossary-term {
        font-weight: bold;
        font-size: 14pt;
        margin-top: 15pt;
        color: #1e40af;
    }
    .glossary-definition {
        margin: 5pt 0 10pt 20pt;
        line-height: 1.6;
    }
</style>
<body>
    <div class="glossary-section">
        <div class="glossary-letter">A</div>
        <div class="glossary-term">Algorithm</div>
        <div class="glossary-definition">A step-by-step procedure for solving a problem...</div>
        <div class="glossary-term">API</div>
        <div class="glossary-definition">Application Programming Interface...</div>
    </div>

    <div class="glossary-section">
        <div class="glossary-letter">B</div>
        <div class="glossary-term">Backend</div>
        <div class="glossary-definition">The server-side portion of an application...</div>
    </div>
</body>
```

### Example 11: Data tables with analysis after each

```html
<style>
    .data-section {
        page-break-after: always;
    }
    .section-title {
        font-size: 20pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 15pt;
    }
    .data-table {
        width: 100%;
        border-collapse: collapse;
        margin: 20pt 0;
    }
    .data-table th {
        background-color: #1e40af;
        color: white;
        padding: 10pt;
        text-align: left;
    }
    .data-table td {
        padding: 8pt;
        border-bottom: 1pt solid #e5e7eb;
    }
    .analysis-box {
        background-color: #f0f9ff;
        border-left: 4pt solid #3b82f6;
        padding: 15pt;
        margin-top: 20pt;
    }
</style>
<body>
    <div class="data-section">
        <h2 class="section-title">Q1 2025 Sales Data</h2>
        <table class="data-table">
            <thead>
                <tr>
                    <th>Region</th>
                    <th>Sales</th>
                    <th>Growth</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>North</td>
                    <td>$125,000</td>
                    <td>+15%</td>
                </tr>
                <tr>
                    <td>South</td>
                    <td>$98,000</td>
                    <td>+8%</td>
                </tr>
            </tbody>
        </table>
        <div class="analysis-box">
            <strong>Analysis:</strong> Northern region shows strongest growth...
        </div>
    </div>

    <div class="data-section">
        <h2 class="section-title">Q2 2025 Sales Data</h2>
        <p>Next quarter data on a fresh page...</p>
    </div>
</body>
```

### Example 12: Newsletter articles separated by page breaks

```html
<style>
    .newsletter-article {
        page-break-after: always;
        padding: 30pt;
    }
    .article-header {
        border-bottom: 3pt solid #dc2626;
        padding-bottom: 15pt;
        margin-bottom: 20pt;
    }
    .article-category {
        color: #dc2626;
        font-size: 11pt;
        font-weight: bold;
        text-transform: uppercase;
        letter-spacing: 1pt;
    }
    .article-headline {
        font-size: 24pt;
        font-weight: bold;
        margin-top: 8pt;
    }
    .article-byline {
        font-size: 10pt;
        color: #6b7280;
        margin-top: 8pt;
    }
</style>
<body>
    <div class="newsletter-article">
        <div class="article-header">
            <div class="article-category">Technology</div>
            <h1 class="article-headline">Artificial Intelligence Trends in 2025</h1>
            <div class="article-byline">By Sarah Johnson | March 15, 2025</div>
        </div>
        <p>The landscape of artificial intelligence continues to evolve...</p>
    </div>

    <div class="newsletter-article">
        <div class="article-header">
            <div class="article-category">Business</div>
            <h1 class="article-headline">Market Outlook for Q2</h1>
            <div class="article-byline">By Michael Chen | March 15, 2025</div>
        </div>
        <p>Economic indicators suggest strong performance...</p>
    </div>
</body>
```

### Example 13: Recipe cards with page breaks

```html
<style>
    .recipe-card {
        page-break-after: always;
        border: 2pt solid #16a34a;
        border-radius: 10pt;
        padding: 25pt;
        background-color: #f9fafb;
    }
    .recipe-title {
        font-size: 26pt;
        font-weight: bold;
        color: #15803d;
        margin-bottom: 15pt;
    }
    .recipe-meta {
        display: flex;
        gap: 20pt;
        margin-bottom: 20pt;
        font-size: 11pt;
        color: #6b7280;
    }
    .ingredients-section {
        background-color: #f0fdf4;
        padding: 15pt;
        border-radius: 5pt;
        margin-bottom: 15pt;
    }
    .section-heading {
        font-size: 16pt;
        font-weight: bold;
        color: #15803d;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="recipe-card">
        <h1 class="recipe-title">Chocolate Chip Cookies</h1>
        <div class="recipe-meta">
            <span>Prep: 15 min</span>
            <span>Cook: 12 min</span>
            <span>Servings: 24</span>
        </div>
        <div class="ingredients-section">
            <div class="section-heading">Ingredients</div>
            <ul>
                <li>2 cups all-purpose flour</li>
                <li>1 cup butter</li>
                <li>1 cup chocolate chips</li>
            </ul>
        </div>
        <div class="section-heading">Instructions</div>
        <ol>
            <li>Preheat oven to 350°F...</li>
        </ol>
    </div>

    <div class="recipe-card">
        <h1 class="recipe-title">Vegetable Soup</h1>
        <p>Next recipe on a fresh page...</p>
    </div>
</body>
```

### Example 14: Team member profiles

```html
<style>
    .team-profile {
        page-break-after: always;
        display: flex;
        gap: 25pt;
        padding: 30pt;
    }
    .profile-photo {
        width: 150pt;
        height: 150pt;
        background-color: #d1d5db;
        border-radius: 50%;
        flex-shrink: 0;
    }
    .profile-info {
        flex: 1;
    }
    .profile-name {
        font-size: 24pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 5pt;
    }
    .profile-title {
        font-size: 14pt;
        color: #6b7280;
        margin-bottom: 15pt;
    }
    .profile-bio {
        line-height: 1.6;
        text-align: justify;
    }
</style>
<body>
    <div class="team-profile">
        <div class="profile-photo"></div>
        <div class="profile-info">
            <h2 class="profile-name">Jane Doe</h2>
            <div class="profile-title">Chief Executive Officer</div>
            <p class="profile-bio">
                Jane brings 20 years of experience in technology leadership...
            </p>
        </div>
    </div>

    <div class="team-profile">
        <div class="profile-photo"></div>
        <div class="profile-info">
            <h2 class="profile-name">John Smith</h2>
            <div class="profile-title">Chief Technology Officer</div>
            <p class="profile-bio">
                John is a seasoned technologist with expertise in cloud architecture...
            </p>
        </div>
    </div>
</body>
```

### Example 15: Product specification sheets

```html
<style>
    .product-spec {
        page-break-after: always;
    }
    .spec-header {
        background: linear-gradient(90deg, #1e3a8a 0%, #3b82f6 100%);
        color: white;
        padding: 20pt;
        margin-bottom: 25pt;
    }
    .product-name {
        font-size: 28pt;
        font-weight: bold;
        margin-bottom: 8pt;
    }
    .product-code {
        font-size: 12pt;
        opacity: 0.9;
    }
    .spec-grid {
        display: grid;
        grid-template-columns: 150pt 1fr;
        gap: 10pt;
        margin: 15pt 0;
    }
    .spec-label {
        font-weight: bold;
        color: #374151;
    }
    .spec-value {
        color: #6b7280;
    }
</style>
<body>
    <div class="product-spec">
        <div class="spec-header">
            <div class="product-name">Professional Laptop Pro</div>
            <div class="product-code">SKU: LAP-PRO-2025-001</div>
        </div>
        <div class="spec-grid">
            <div class="spec-label">Processor:</div>
            <div class="spec-value">Intel Core i7-12700H</div>
            <div class="spec-label">RAM:</div>
            <div class="spec-value">32GB DDR5</div>
            <div class="spec-label">Storage:</div>
            <div class="spec-value">1TB NVMe SSD</div>
        </div>
    </div>

    <div class="product-spec">
        <div class="spec-header">
            <div class="product-name">Business Desktop Elite</div>
            <div class="product-code">SKU: DSK-ELT-2025-002</div>
        </div>
        <p>Specifications for the next product...</p>
    </div>
</body>
```

---

## See Also

- [page-break-before](/reference/cssproperties/css_prop_page-break-before) - Control page breaks before elements
- [page-break-inside](/reference/cssproperties/css_prop_page-break-inside) - Control page breaks within elements
- [break-after](/reference/cssproperties/css_prop_break-after) - Modern alternative with more options
- [break-before](/reference/cssproperties/css_prop_break-before) - Modern page and column break control
- [page](/reference/cssproperties/css_prop_page) - Specify named page for element
- [@page rule](/reference/css_atrules/css_atrule_page) - Define page properties and margins

---
