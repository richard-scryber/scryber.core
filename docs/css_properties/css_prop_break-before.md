---
layout: default
title: break-before
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# break-before : Break Before Property

The `break-before` property controls page and column breaks before an element when generating PDF documents. This is the modern CSS3 replacement for `page-break-before`, offering more control including column breaks for multi-column layouts. It ensures specific content starts on a new page or column, creating clear document structure and organization.

## Usage

```css
selector {
    break-before: value;
}
```

The break-before property provides precise control over pagination and column layout by forcing or avoiding breaks before specific elements in your PDF document.

---

## Supported Values

### auto (default)
Automatic breaking behavior. The browser/PDF generator determines where page and column breaks occur based on content flow and available space. This is the default value.

### always
Forces a break (page or column, depending on context) before the element. The element will always start on a new page or column.

### avoid
Attempts to avoid any break before the element. The PDF generator will try to keep the element with preceding content, though this may not always be possible with large content.

### page
Forces a page break before the element. Similar to `always` but explicitly specifies a page break (not a column break in multi-column layouts).

### left
Forces one or two page breaks before the element so that the element appears on a left-hand page. Useful for double-sided printing where sections should start on left pages.

### right
Forces one or two page breaks before the element so that the element appears on a right-hand page. Commonly used for double-sided documents where chapters should start on right-hand pages (common in books).

### column
Forces a column break before the element in multi-column layouts. The element will start at the top of the next column. If already in the last column, it forces a page break.

---

## Supported Elements

The `break-before` property can be applied to:
- Block-level elements (`<div>`, `<section>`, `<article>`)
- Headings (`<h1>` through `<h6>`)
- Paragraphs (`<p>`)
- Lists (`<ul>`, `<ol>`)
- Tables (`<table>`)
- Horizontal rules (`<hr>`)
- Any block container element

---

## Notes

- This is the modern CSS3 property that replaces `page-break-before`
- The `auto` value is the default and allows natural breaking
- Using `always` or `page` guarantees the element starts on a new page
- The `column` value is specifically for multi-column layouts
- The `avoid` value is a suggestion and may be overridden if content is too large
- Left and right values are designed for double-sided (duplex) printing
- When using `left` or `right`, blank pages may be inserted to achieve the desired page position
- Breaks are only applied during PDF generation, not in HTML preview
- More flexible than the legacy `page-break-before` property
- Can be combined with `break-after` and `break-inside` for complete control

---

## Data Binding

The `break-before` property supports data binding, allowing you to create sophisticated, data-driven document layouts with dynamic page and column breaks. This enables flexible templates that adapt to different document structures and requirements.

### Example 1: Conditional chapter breaks

```html
<style>
    .content-section {
        break-before: {{section.level === 'chapter' ? 'page' : (section.level === 'major' ? 'column' : 'auto')}};
        padding-top: {{section.level === 'chapter' ? '30pt' : '15pt'}};
    }
    .section-header {
        font-size: {{section.level === 'chapter' ? '32pt' : '20pt'}};
        font-weight: bold;
        color: {{section.level === 'chapter' ? '#1e3a8a' : '#374151'}};
    }
</style>
<body>
    {{#each sections}}
    <div class="content-section">
        <h2 class="section-header">{{sectionTitle}}</h2>
        <p>{{sectionContent}}</p>
    </div>
    {{/each}}
</body>
```

### Example 2: Dynamic page orientation based on content

```html
<style>
    @page portrait-page {
        size: A4 portrait;
        margin: 25mm;
    }
    @page landscape-page {
        size: A4 landscape;
        margin: 20mm;
    }

    .data-section {
        break-before: {{section.requiresLandscape ? 'page' : 'auto'}};
        page: {{section.requiresLandscape ? 'landscape-page' : 'portrait-page'}};
    }
    .section-title {
        font-size: 24pt;
        font-weight: bold;
        margin-bottom: 20pt;
    }
</style>
<body>
    {{#each dataSections}}
    <div class="data-section">
        <h1 class="section-title">{{title}}</h1>
        {{#if requiresLandscape}}
        <table style="width: 100%;">
            <tr>
                {{#each columns}}
                <th>{{this}}</th>
                {{/each}}
            </tr>
        </table>
        {{else}}
        <p>{{narrative}}</p>
        {{/if}}
    </div>
    {{/each}}
</body>
```

### Example 3: Configurable report structure with named pages

```html
<style>
    @page summary-page {
        size: A4 portrait;
        margin: 30mm;
    }
    @page detail-page {
        size: A4 portrait;
        margin: 20mm;
    }
    @page appendix-page {
        size: A4 landscape;
        margin: 15mm;
    }

    .report-part {
        break-before: {{part.newPage ? 'page' : 'auto'}};
        page: {{part.pageType}}-page;
    }
    .part-header {
        font-size: {{part.headerSize}}pt;
        color: {{part.headerColor}};
        margin-bottom: 20pt;
    }
</style>
<body>
    {{#each reportParts}}
    <div class="report-part">
        <h1 class="part-header">{{partTitle}}</h1>
        <div>{{partContent}}</div>
    </div>
    {{/each}}
</body>
```

---

## Examples

### Example 1: Chapter breaks with modern syntax

```html
<style>
    .chapter {
        break-before: page;
        padding-top: 30pt;
    }
    .chapter-number {
        font-size: 18pt;
        color: #6b7280;
        text-transform: uppercase;
        letter-spacing: 2pt;
        margin-bottom: 10pt;
    }
    .chapter-title {
        font-size: 32pt;
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 25pt;
    }
</style>
<body>
    <div class="chapter">
        <div class="chapter-number">Chapter One</div>
        <h1 class="chapter-title">Introduction to Modern Web Development</h1>
        <p>The landscape of web development has evolved dramatically...</p>
    </div>

    <div class="chapter">
        <div class="chapter-number">Chapter Two</div>
        <h1 class="chapter-title">Frontend Frameworks</h1>
        <p>Modern frontend development relies heavily on frameworks...</p>
    </div>
</body>
```

### Example 2: Multi-column layout with column breaks

```html
<style>
    .article-content {
        column-count: 2;
        column-gap: 30pt;
        column-rule: 1pt solid #d1d5db;
    }
    .section-heading {
        break-before: column;
        font-size: 16pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-top: 0;
        margin-bottom: 12pt;
    }
    .first-section {
        break-before: auto;
    }
</style>
<body>
    <div class="article-content">
        <h2 class="section-heading first-section">Introduction</h2>
        <p>This article explores the latest trends in technology and their
        impact on business operations. We examine multiple case studies
        and provide actionable insights.</p>

        <h2 class="section-heading">Market Analysis</h2>
        <p>The market has shown significant growth in recent quarters.
        Key indicators suggest continued expansion through next year.</p>

        <h2 class="section-heading">Future Outlook</h2>
        <p>Looking ahead, we anticipate several emerging trends that
        will shape the industry landscape.</p>
    </div>
</body>
```

### Example 3: Right-hand page starts for book sections

```html
<style>
    .book-section {
        break-before: right;
        padding: 50pt 40pt;
    }
    .section-ornament {
        text-align: center;
        font-size: 48pt;
        color: #d1d5db;
        margin-bottom: 30pt;
    }
    .section-label {
        text-align: center;
        font-size: 14pt;
        text-transform: uppercase;
        letter-spacing: 3pt;
        color: #9ca3af;
        margin-bottom: 15pt;
    }
    .section-title {
        text-align: center;
        font-size: 36pt;
        font-weight: bold;
        color: #1f2937;
        font-family: serif;
    }
</style>
<body>
    <div class="book-section">
        <div class="section-ornament">❦</div>
        <div class="section-label">Part One</div>
        <h1 class="section-title">The Foundation</h1>
    </div>

    <p>Chapter content begins here on the right-hand page...</p>

    <div class="book-section">
        <div class="section-ornament">❦</div>
        <div class="section-label">Part Two</div>
        <h1 class="section-title">The Journey</h1>
    </div>

    <p>Next section also starts on a right-hand page...</p>
</body>
```

### Example 4: Avoiding breaks before important headings

```html
<style>
    .important-section {
        break-before: avoid;
        background-color: #fef3c7;
        border-left: 5pt solid #f59e0b;
        padding: 15pt;
        margin: 20pt 0;
    }
    .section-marker {
        display: inline-block;
        background-color: #f59e0b;
        color: white;
        padding: 5pt 10pt;
        border-radius: 3pt;
        font-size: 10pt;
        font-weight: bold;
        margin-bottom: 8pt;
    }
    .section-heading {
        font-size: 16pt;
        font-weight: bold;
        color: #92400e;
        margin: 8pt 0;
    }
</style>
<body>
    <p>Previous content that leads into an important section...</p>

    <div class="important-section">
        <div class="section-marker">IMPORTANT</div>
        <h3 class="section-heading">Critical Safety Information</h3>
        <p>This section tries to stay with preceding content to maintain context.</p>
    </div>

    <p>Additional details and explanations...</p>
</body>
```

### Example 5: Major report sections on new pages

```html
<style>
    .report-section {
        break-before: page;
    }
    .section-header {
        background: linear-gradient(135deg, #1e3a8a 0%, #3b82f6 100%);
        color: white;
        padding: 25pt;
        margin-bottom: 30pt;
        border-radius: 8pt;
    }
    .section-number {
        font-size: 48pt;
        font-weight: 300;
        opacity: 0.7;
        line-height: 1;
    }
    .section-name {
        font-size: 24pt;
        font-weight: bold;
        margin-top: 10pt;
    }
</style>
<body>
    <div class="report-section">
        <div class="section-header">
            <div class="section-number">01</div>
            <div class="section-name">Executive Summary</div>
        </div>
        <p>This report presents findings from our comprehensive analysis...</p>
    </div>

    <div class="report-section">
        <div class="section-header">
            <div class="section-number">02</div>
            <div class="section-name">Methodology</div>
        </div>
        <p>Our research approach incorporated multiple data sources...</p>
    </div>

    <div class="report-section">
        <div class="section-header">
            <div class="section-number">03</div>
            <div class="section-name">Results and Analysis</div>
        </div>
        <p>The data reveals several significant trends...</p>
    </div>
</body>
```

### Example 6: Newsletter with column breaks between articles

```html
<style>
    .newsletter {
        column-count: 2;
        column-gap: 25pt;
        column-rule: 2pt solid #e5e7eb;
    }
    .article {
        break-before: column;
        margin-bottom: 20pt;
    }
    .article:first-child {
        break-before: auto;
    }
    .article-title {
        font-size: 16pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 8pt;
    }
    .article-meta {
        font-size: 9pt;
        color: #6b7280;
        margin-bottom: 10pt;
    }
    .article-text {
        font-size: 10pt;
        line-height: 1.6;
        text-align: justify;
    }
</style>
<body>
    <div class="newsletter">
        <div class="article">
            <h2 class="article-title">Tech Industry Updates</h2>
            <div class="article-meta">By John Smith | March 15, 2025</div>
            <p class="article-text">The technology sector continues to show
            robust growth with several major announcements this quarter...</p>
        </div>

        <div class="article">
            <h2 class="article-title">Market Trends Analysis</h2>
            <div class="article-meta">By Sarah Johnson | March 15, 2025</div>
            <p class="article-text">Recent market data indicates shifting
            consumer preferences toward sustainable products...</p>
        </div>

        <div class="article">
            <h2 class="article-title">Investment Opportunities</h2>
            <div class="article-meta">By Michael Chen | March 15, 2025</div>
            <p class="article-text">Emerging markets present unique opportunities
            for diversified portfolios...</p>
        </div>
    </div>
</body>
```

### Example 7: Left-hand page starts for appendices

```html
<style>
    .appendix {
        break-before: left;
        padding: 40pt;
    }
    .appendix-banner {
        border-top: 5pt solid #7c3aed;
        border-bottom: 5pt solid #7c3aed;
        padding: 20pt 0;
        text-align: center;
        margin-bottom: 30pt;
    }
    .appendix-label {
        font-size: 16pt;
        color: #6b21a8;
        text-transform: uppercase;
        letter-spacing: 2pt;
        margin-bottom: 8pt;
    }
    .appendix-title {
        font-size: 28pt;
        font-weight: bold;
        color: #5b21b6;
    }
</style>
<body>
    <div class="appendix">
        <div class="appendix-banner">
            <div class="appendix-label">Appendix A</div>
            <div class="appendix-title">Technical Specifications</div>
        </div>
        <p>Detailed technical information appears on left-hand page...</p>
    </div>

    <div class="appendix">
        <div class="appendix-banner">
            <div class="appendix-label">Appendix B</div>
            <div class="appendix-title">Reference Materials</div>
        </div>
        <p>Additional reference materials on left-hand page...</p>
    </div>
</body>
```

### Example 8: Catalog categories with page breaks

```html
<style>
    .catalog-category {
        break-before: page;
    }
    .category-header {
        background: linear-gradient(135deg, #ec4899 0%, #8b5cf6 100%);
        color: white;
        padding: 30pt;
        margin-bottom: 25pt;
        text-align: center;
    }
    .category-icon {
        font-size: 64pt;
        margin-bottom: 15pt;
    }
    .category-name {
        font-size: 32pt;
        font-weight: bold;
    }
    .product-grid {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 20pt;
    }
    .product-card {
        border: 1pt solid #e5e7eb;
        border-radius: 8pt;
        padding: 15pt;
        text-align: center;
    }
</style>
<body>
    <div class="catalog-category">
        <div class="category-header">
            <div class="category-icon">💻</div>
            <div class="category-name">Electronics</div>
        </div>
        <div class="product-grid">
            <div class="product-card">Product 1</div>
            <div class="product-card">Product 2</div>
            <div class="product-card">Product 3</div>
        </div>
    </div>

    <div class="catalog-category">
        <div class="category-header">
            <div class="category-icon">🏠</div>
            <div class="category-name">Home & Garden</div>
        </div>
        <div class="product-grid">
            <div class="product-card">Product 4</div>
            <div class="product-card">Product 5</div>
            <div class="product-card">Product 6</div>
        </div>
    </div>
</body>
```

### Example 9: Training manual with module breaks

```html
<style>
    .training-module {
        break-before: page;
    }
    .module-cover {
        height: 200pt;
        background-color: #1e40af;
        color: white;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        margin-bottom: 30pt;
        border-radius: 10pt;
    }
    .module-number {
        font-size: 72pt;
        font-weight: bold;
        opacity: 0.5;
    }
    .module-title {
        font-size: 28pt;
        font-weight: bold;
        margin-top: 15pt;
    }
    .learning-objectives {
        background-color: #dbeafe;
        border-left: 4pt solid #3b82f6;
        padding: 20pt;
        margin: 25pt 0;
    }
    .objectives-heading {
        font-size: 16pt;
        font-weight: bold;
        color: #1e40af;
        margin-bottom: 12pt;
    }
</style>
<body>
    <div class="training-module">
        <div class="module-cover">
            <div class="module-number">1</div>
            <div class="module-title">Introduction to Safety Procedures</div>
        </div>
        <div class="learning-objectives">
            <div class="objectives-heading">Learning Objectives</div>
            <ul>
                <li>Identify common workplace hazards</li>
                <li>Apply proper safety protocols</li>
                <li>Respond to emergency situations</li>
            </ul>
        </div>
        <p>Module content begins here...</p>
    </div>

    <div class="training-module">
        <div class="module-cover">
            <div class="module-number">2</div>
            <div class="module-title">Equipment Operation</div>
        </div>
        <div class="learning-objectives">
            <div class="objectives-heading">Learning Objectives</div>
            <ul>
                <li>Operate equipment safely</li>
                <li>Perform routine maintenance</li>
                <li>Troubleshoot common issues</li>
            </ul>
        </div>
        <p>Equipment operation details...</p>
    </div>
</body>
```

### Example 10: Magazine layout with article breaks

```html
<style>
    .magazine-article {
        break-before: page;
        padding: 40pt;
    }
    .article-category {
        display: inline-block;
        background-color: #dc2626;
        color: white;
        padding: 6pt 12pt;
        font-size: 10pt;
        font-weight: bold;
        text-transform: uppercase;
        letter-spacing: 1pt;
        margin-bottom: 15pt;
    }
    .article-headline {
        font-size: 38pt;
        font-weight: bold;
        line-height: 1.2;
        color: #1f2937;
        margin-bottom: 15pt;
    }
    .article-deck {
        font-size: 16pt;
        color: #6b7280;
        margin-bottom: 20pt;
        font-style: italic;
    }
    .article-byline {
        font-size: 11pt;
        color: #9ca3af;
        margin-bottom: 25pt;
        padding-bottom: 15pt;
        border-bottom: 1pt solid #e5e7eb;
    }
</style>
<body>
    <div class="magazine-article">
        <div class="article-category">Technology</div>
        <h1 class="article-headline">The Future of Artificial Intelligence</h1>
        <p class="article-deck">
            How AI is transforming industries and reshaping our daily lives
            in ways we never imagined possible.
        </p>
        <div class="article-byline">
            By Alexandra Rodriguez | Photography by James Kim
        </div>
        <p>Artificial intelligence has evolved from science fiction...</p>
    </div>

    <div class="magazine-article">
        <div class="article-category">Business</div>
        <h1 class="article-headline">Sustainable Business Practices</h1>
        <p class="article-deck">
            Leading companies are proving that profitability and environmental
            responsibility can go hand in hand.
        </p>
        <div class="article-byline">
            By Marcus Thompson | Research by Data Insights Team
        </div>
        <p>The shift toward sustainability is no longer optional...</p>
    </div>
</body>
```

### Example 11: Financial report with section breaks

```html
<style>
    .financial-section {
        break-before: page;
    }
    .section-divider {
        height: 100pt;
        background: linear-gradient(180deg, transparent 0%, #1e3a8a 100%);
        margin-bottom: 30pt;
    }
    .section-title-block {
        background-color: #1e3a8a;
        color: white;
        padding: 20pt;
        margin-bottom: 25pt;
    }
    .section-title {
        font-size: 28pt;
        font-weight: bold;
    }
    .section-subtitle {
        font-size: 14pt;
        opacity: 0.9;
        margin-top: 8pt;
    }
</style>
<body>
    <div class="financial-section">
        <div class="section-divider"></div>
        <div class="section-title-block">
            <div class="section-title">Balance Sheet</div>
            <div class="section-subtitle">As of December 31, 2025</div>
        </div>
        <p>Assets, liabilities, and equity details...</p>
    </div>

    <div class="financial-section">
        <div class="section-divider"></div>
        <div class="section-title-block">
            <div class="section-title">Income Statement</div>
            <div class="section-subtitle">Year Ended December 31, 2025</div>
        </div>
        <p>Revenue and expense analysis...</p>
    </div>

    <div class="financial-section">
        <div class="section-divider"></div>
        <div class="section-title-block">
            <div class="section-title">Cash Flow Statement</div>
            <div class="section-subtitle">Year Ended December 31, 2025</div>
        </div>
        <p>Cash flow activities and analysis...</p>
    </div>
</body>
```

### Example 12: Event program with session breaks

```html
<style>
    .conference-session {
        break-before: page;
        padding: 30pt;
    }
    .session-header {
        border-left: 10pt solid #10b981;
        padding-left: 20pt;
        margin-bottom: 30pt;
    }
    .session-time {
        font-size: 16pt;
        font-weight: bold;
        color: #059669;
    }
    .session-title {
        font-size: 26pt;
        font-weight: bold;
        color: #1f2937;
        margin: 10pt 0;
    }
    .session-speaker {
        font-size: 14pt;
        color: #6b7280;
    }
    .session-description {
        line-height: 1.8;
        margin: 20pt 0;
        text-align: justify;
    }
    .speaker-bio {
        background-color: #f0fdf4;
        padding: 15pt;
        border-radius: 5pt;
        margin-top: 20pt;
    }
</style>
<body>
    <div class="conference-session">
        <div class="session-header">
            <div class="session-time">9:00 AM - 10:30 AM</div>
            <h2 class="session-title">Opening Keynote: The Future of Innovation</h2>
            <div class="session-speaker">Dr. Emily Chen, Chief Innovation Officer</div>
        </div>
        <div class="session-description">
            <p>Join us for an inspiring keynote that explores emerging trends...</p>
        </div>
        <div class="speaker-bio">
            <strong>About the Speaker:</strong> Dr. Emily Chen has over 20 years
            of experience in technology innovation...
        </div>
    </div>

    <div class="conference-session">
        <div class="session-header">
            <div class="session-time">11:00 AM - 12:30 PM</div>
            <h2 class="session-title">Workshop: Practical AI Implementation</h2>
            <div class="session-speaker">Prof. David Martinez, AI Research Lab</div>
        </div>
        <div class="session-description">
            <p>A hands-on workshop covering real-world AI applications...</p>
        </div>
        <div class="speaker-bio">
            <strong>About the Speaker:</strong> Prof. David Martinez leads the
            AI Research Lab at Tech University...
        </div>
    </div>
</body>
```

### Example 13: Recipe book with recipe breaks

```html
<style>
    .recipe {
        break-before: page;
        padding: 40pt;
    }
    .recipe-hero {
        width: 100%;
        height: 250pt;
        background-color: #f3f4f6;
        border-radius: 10pt;
        display: flex;
        align-items: center;
        justify-content: center;
        margin-bottom: 30pt;
        font-size: 24pt;
        color: #9ca3af;
    }
    .recipe-name {
        font-size: 32pt;
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 10pt;
    }
    .recipe-info {
        display: flex;
        gap: 25pt;
        margin-bottom: 30pt;
        font-size: 12pt;
        color: #6b7280;
    }
    .info-item {
        display: flex;
        align-items: center;
        gap: 8pt;
    }
</style>
<body>
    <div class="recipe">
        <div class="recipe-hero">[Dish Photo]</div>
        <h1 class="recipe-name">Grilled Salmon with Lemon Butter</h1>
        <div class="recipe-info">
            <div class="info-item">⏱ Prep: 15 min</div>
            <div class="info-item">🔥 Cook: 12 min</div>
            <div class="info-item">👥 Serves: 4</div>
            <div class="info-item">📊 Difficulty: Easy</div>
        </div>
        <h3>Ingredients</h3>
        <ul>
            <li>4 salmon fillets (6 oz each)</li>
            <li>4 tablespoons butter</li>
            <li>2 lemons, juiced</li>
        </ul>
    </div>

    <div class="recipe">
        <div class="recipe-hero">[Dish Photo]</div>
        <h1 class="recipe-name">Vegetarian Lasagna</h1>
        <div class="recipe-info">
            <div class="info-item">⏱ Prep: 30 min</div>
            <div class="info-item">🔥 Cook: 45 min</div>
            <div class="info-item">👥 Serves: 8</div>
            <div class="info-item">📊 Difficulty: Medium</div>
        </div>
        <h3>Ingredients</h3>
        <ul>
            <li>12 lasagna noodles</li>
            <li>2 cups ricotta cheese</li>
        </ul>
    </div>
</body>
```

### Example 14: Portfolio projects with page breaks

```html
<style>
    .portfolio-project {
        break-before: page;
        padding: 40pt;
    }
    .project-header {
        margin-bottom: 30pt;
    }
    .project-title {
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
    }
    .project-client {
        font-size: 16pt;
        color: #6b7280;
        margin-bottom: 15pt;
    }
    .project-tags {
        display: flex;
        gap: 10pt;
        flex-wrap: wrap;
    }
    .tag {
        background-color: #dbeafe;
        color: #1e40af;
        padding: 5pt 12pt;
        border-radius: 15pt;
        font-size: 10pt;
    }
    .project-image {
        width: 100%;
        height: 350pt;
        background-color: #e5e7eb;
        border-radius: 8pt;
        margin: 20pt 0;
        display: flex;
        align-items: center;
        justify-content: center;
    }
</style>
<body>
    <div class="portfolio-project">
        <div class="project-header">
            <h1 class="project-title">E-Commerce Platform Redesign</h1>
            <div class="project-client">Client: TechMart Retail</div>
            <div class="project-tags">
                <span class="tag">UI/UX Design</span>
                <span class="tag">Web Development</span>
                <span class="tag">React</span>
            </div>
        </div>
        <div class="project-image">[Project Screenshot]</div>
        <p>Complete redesign of the e-commerce platform to improve
        user experience and increase conversion rates...</p>
    </div>

    <div class="portfolio-project">
        <div class="project-header">
            <h1 class="project-title">Mobile Banking App</h1>
            <div class="project-client">Client: First National Bank</div>
            <div class="project-tags">
                <span class="tag">Mobile App</span>
                <span class="tag">iOS/Android</span>
                <span class="tag">Flutter</span>
            </div>
        </div>
        <div class="project-image">[App Screenshot]</div>
        <p>Development of a secure mobile banking application with
        intuitive interface and advanced features...</p>
    </div>
</body>
```

### Example 15: Research paper with explicit page breaks

```html
<style>
    .paper-section {
        break-before: page;
    }
    .section-marker {
        width: 60pt;
        height: 60pt;
        background-color: #7c3aed;
        color: white;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 24pt;
        font-weight: bold;
        margin: 0 auto 20pt auto;
    }
    .section-heading {
        text-align: center;
        font-size: 28pt;
        font-weight: bold;
        color: #5b21b6;
        margin-bottom: 30pt;
    }
    .section-content {
        line-height: 2;
        text-align: justify;
        columns: 1;
    }
</style>
<body>
    <div class="paper-section">
        <div class="section-marker">1</div>
        <h2 class="section-heading">Abstract</h2>
        <div class="section-content">
            <p>This research investigates the impact of machine learning
            algorithms on predictive analytics in healthcare settings...</p>
        </div>
    </div>

    <div class="paper-section">
        <div class="section-marker">2</div>
        <h2 class="section-heading">Introduction</h2>
        <div class="section-content">
            <p>The application of artificial intelligence in healthcare
            has grown exponentially in recent years...</p>
        </div>
    </div>

    <div class="paper-section">
        <div class="section-marker">3</div>
        <h2 class="section-heading">Methodology</h2>
        <div class="section-content">
            <p>Our research methodology combined quantitative and qualitative
            approaches to ensure comprehensive analysis...</p>
        </div>
    </div>
</body>
```

---

## See Also

- [break-after](/reference/cssproperties/css_prop_break-after) - Control breaks after elements
- [break-inside](/reference/cssproperties/css_prop_break-inside) - Control breaks within elements
- [page-break-before](/reference/cssproperties/css_prop_page-break-before) - Legacy CSS2 property
- [page](/reference/cssproperties/css_prop_page) - Specify named page for element
- [@page rule](/reference/css_atrules/css_atrule_page) - Define page properties and margins

---
