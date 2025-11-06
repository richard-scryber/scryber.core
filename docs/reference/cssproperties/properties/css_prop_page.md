---
layout: default
title: page
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# page : Page Context Property

The `page` property specifies which named page context an element should use when generating PDF documents. This allows different parts of a document to use different page configurations (size, orientation, margins, headers, footers) by referencing named `@page` rules. It's essential for creating complex documents with varying page layouts.

## Usage

```css
selector {
    page: page-name;
}
```

The page property assigns elements to specific named page contexts defined by corresponding `@page page-name` rules. This enables different sections of a document to have distinct page properties.

---

## Supported Values

### auto (default)
Uses the default page context. Elements will use the unnamed `@page` rule or browser defaults.

### Custom Identifier
Any valid CSS identifier can be used as a page name. The element will use the page context defined by the corresponding `@page page-name` rule.

Examples:
- `page: cover;` - Uses `@page cover { ... }`
- `page: content;` - Uses `@page content { ... }`
- `page: landscape-section;` - Uses `@page landscape-section { ... }`
- `page: chapter-start;` - Uses `@page chapter-start { ... }`

---

## Supported Elements

The `page` property can be applied to:
- Block-level elements (`<div>`, `<section>`, `<article>`)
- Headings (`<h1>` through `<h6>`)
- Any element that can start on a new page
- Container elements that encompass page-specific content

---

## Notes

- The `page` property must reference a named `@page` rule to have effect
- Elements with different `page` values will typically start on new pages
- Named pages can have different sizes, orientations, margins, and other properties
- The `page` property is inherited, so child elements use the parent's page context
- Setting a different `page` value usually triggers an implicit page break
- Named pages are ideal for documents with mixed page layouts (portrait/landscape)
- You can define multiple named page contexts in a single document
- The `page` property works in conjunction with page break properties
- Named pages enable sophisticated document layouts with varying specifications
- First, left, and right pseudo-classes can be used with named pages: `@page name:first`

---

## Data Binding

The `page` property supports data binding, enabling dynamic assignment of elements to different named page contexts based on data, configuration, or document structure. This provides maximum flexibility for creating complex documents with varying page layouts.

### Example 1: Data-driven page context selection

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
    @page compact-page {
        size: A5 portrait;
        margin: 15mm;
    }

    .content-section {
        page: {{section.pageContext || 'portrait-page'}};
    }
    .section-header {
        font-size: {{section.headerSize || 24}}pt;
        font-weight: bold;
        color: {{section.headerColor || '#1e3a8a'}};
        margin-bottom: 20pt;
    }
</style>
<body>
    {{#each sections}}
    <div class="content-section">
        <h1 class="section-header">{{sectionTitle}}</h1>
        <p>{{sectionContent}}</p>
    </div>
    {{/each}}
</body>
```

### Example 2: Document type-based page assignments

```html
<style>
    @page cover-style {
        size: {{documentConfig.coverSize || 'A4'}} portrait;
        margin: 0;
    }
    @page content-style {
        size: {{documentConfig.contentSize || 'A4'}} portrait;
        margin: {{documentConfig.contentMargin || '25mm'}};
    }
    @page appendix-style {
        size: {{documentConfig.appendixSize || 'A4'}} landscape;
        margin: {{documentConfig.appendixMargin || '20mm'}};
    }

    .cover {
        page: cover-style;
    }
    .main-content {
        page: content-style;
    }
    .appendix {
        page: appendix-style;
    }
</style>
<body>
    <div class="cover">
        <h1>{{documentTitle}}</h1>
        <p>{{documentSubtitle}}</p>
    </div>

    {{#each chapters}}
    <div class="main-content">
        <h2>{{chapterTitle}}</h2>
        <p>{{chapterContent}}</p>
    </div>
    {{/each}}

    {{#if hasAppendix}}
    <div class="appendix">
        <h2>Appendix</h2>
        <p>{{appendixContent}}</p>
    </div>
    {{/if}}
</body>
```

### Example 3: Named page sequences with conditional formatting

```html
<style>
    @page summary-page {
        size: {{pageSettings.size || 'A4'}} portrait;
        margin: {{pageSettings.summaryMargin || '30mm'}};
    }
    @page detail-page {
        size: {{pageSettings.size || 'A4'}} portrait;
        margin: {{pageSettings.detailMargin || '25mm'}};
    }
    @page data-page {
        size: {{pageSettings.size || 'A4'}} landscape;
        margin: {{pageSettings.dataMargin || '15mm'}};
    }

    .report-part {
        page: {{part.pageType}}-page;
    }
    .part-header {
        background-color: {{part.headerBg || '#1e3a8a'}};
        color: white;
        padding: 15pt;
        font-size: {{part.headerSize || 20}}pt;
        margin-bottom: 20pt;
    }
</style>
<body>
    {{#each reportParts}}
    <div class="report-part">
        <div class="part-header">{{partTitle}}</div>
        <div class="part-content">
            {{#if part.pageType === 'data'}}
            <table>
                <tr>
                    {{#each part.columns}}
                    <th>{{this}}</th>
                    {{/each}}
                </tr>
                {{#each part.rows}}
                <tr>
                    {{#each this}}
                    <td>{{this}}</td>
                    {{/each}}
                </tr>
                {{/each}}
            </table>
            {{else}}
            <p>{{part.textContent}}</p>
            {{/if}}
        </div>
    </div>
    {{/each}}
</body>
```

---

## Examples

### Example 1: Cover page with different formatting

```html
<style>
    @page cover {
        size: A4 landscape;
        margin: 0;
    }
    @page content {
        size: A4 portrait;
        margin: 25mm;
    }

    .cover-section {
        page: cover;
        width: 100%;
        height: 100vh;
        background: linear-gradient(135deg, #1e3a8a 0%, #3b82f6 100%);
        color: white;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        text-align: center;
    }
    .main-content {
        page: content;
    }
    .cover-title {
        font-size: 52pt;
        font-weight: bold;
        margin-bottom: 20pt;
    }
    .cover-subtitle {
        font-size: 28pt;
    }
</style>
<body>
    <div class="cover-section">
        <h1 class="cover-title">Annual Report 2025</h1>
        <p class="cover-subtitle">Strategic Growth & Innovation</p>
    </div>

    <div class="main-content">
        <h2>Executive Summary</h2>
        <p>The main content uses standard portrait pages with margins...</p>
    </div>
</body>
```

### Example 2: Mixed portrait and landscape sections

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

    .text-section {
        page: portrait-page;
    }
    .chart-section {
        page: landscape-page;
    }
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th {
        background-color: #1e3a8a;
        color: white;
        padding: 10pt;
    }
    .data-table td {
        border: 1pt solid #d1d5db;
        padding: 8pt;
    }
</style>
<body>
    <div class="text-section">
        <h1>Project Overview</h1>
        <p>This section contains narrative content in portrait orientation...</p>
    </div>

    <div class="chart-section">
        <h2>Quarterly Performance Data</h2>
        <table class="data-table">
            <thead>
                <tr>
                    <th>Quarter</th>
                    <th>Region A</th>
                    <th>Region B</th>
                    <th>Region C</th>
                    <th>Region D</th>
                    <th>Total</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Q1 2025</td>
                    <td>$125,000</td>
                    <td>$108,000</td>
                    <td>$142,000</td>
                    <td>$135,000</td>
                    <td>$510,000</td>
                </tr>
            </tbody>
        </table>
    </div>

    <div class="text-section">
        <h2>Analysis</h2>
        <p>Analysis returns to portrait orientation...</p>
    </div>
</body>
```

### Example 3: Chapter pages with special formatting

```html
<style>
    @page chapter-start {
        size: A4 portrait;
        margin: 50mm 25mm 25mm 25mm;
    }
    @page chapter-body {
        size: A4 portrait;
        margin: 25mm;
    }

    .chapter-opening {
        page: chapter-start;
    }
    .chapter-content {
        page: chapter-body;
    }
    .chapter-number {
        font-size: 72pt;
        color: #d1d5db;
        font-weight: 300;
        text-align: center;
        margin-bottom: 20pt;
    }
    .chapter-title {
        font-size: 36pt;
        font-weight: bold;
        text-align: center;
        color: #1e3a8a;
    }
</style>
<body>
    <div class="chapter-opening">
        <div class="chapter-number">1</div>
        <h1 class="chapter-title">Introduction to Modern Development</h1>
    </div>

    <div class="chapter-content">
        <p>The chapter content continues with standard formatting...</p>
    </div>

    <div class="chapter-opening">
        <div class="chapter-number">2</div>
        <h1 class="chapter-title">Advanced Techniques</h1>
    </div>

    <div class="chapter-content">
        <p>Next chapter content...</p>
    </div>
</body>
```

### Example 4: Legal document with different page types

```html
<style>
    @page cover-page {
        size: Letter portrait;
        margin: 1in;
    }
    @page toc-page {
        size: Letter portrait;
        margin: 0.75in 1in;
    }
    @page body-page {
        size: Legal portrait;
        margin: 1in;
    }
    @page appendix-page {
        size: Letter landscape;
        margin: 0.5in;
    }

    .document-cover {
        page: cover-page;
        text-align: center;
    }
    .table-of-contents {
        page: toc-page;
    }
    .main-body {
        page: body-page;
    }
    .appendix {
        page: appendix-page;
    }
</style>
<body>
    <div class="document-cover">
        <h1>Legal Services Agreement</h1>
        <p>Contract #2025-LSA-001</p>
    </div>

    <div class="table-of-contents">
        <h2>Table of Contents</h2>
        <p>1. Definitions ............. 3</p>
        <p>2. Services ............... 5</p>
    </div>

    <div class="main-body">
        <h2>Article I: Definitions</h2>
        <p>Legal content on Legal-sized pages...</p>
    </div>

    <div class="appendix">
        <h2>Appendix A: Fee Schedule</h2>
        <table>
            <tr><th>Service</th><th>Rate</th></tr>
        </table>
    </div>
</body>
```

### Example 5: Training manual with module-specific pages

```html
<style>
    @page intro-page {
        size: A4 portrait;
        margin: 30mm;
        background-color: #f9fafb;
    }
    @page module-page {
        size: A4 portrait;
        margin: 20mm;
    }
    @page exercise-page {
        size: A4 portrait;
        margin: 15mm;
        background-color: #fef3c7;
    }

    .introduction {
        page: intro-page;
    }
    .module-content {
        page: module-page;
    }
    .practice-exercise {
        page: exercise-page;
    }
    .exercise-banner {
        background-color: #f59e0b;
        color: white;
        padding: 15pt;
        margin-bottom: 20pt;
        font-size: 18pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="introduction">
        <h1>Course Introduction</h1>
        <p>Welcome to the professional development course...</p>
    </div>

    <div class="module-content">
        <h2>Module 1: Core Concepts</h2>
        <p>This module covers fundamental principles...</p>
    </div>

    <div class="practice-exercise">
        <div class="exercise-banner">Practice Exercise 1</div>
        <p>Complete the following tasks to reinforce learning...</p>
    </div>
</body>
```

### Example 6: Marketing brochure with varied layouts

```html
<style>
    @page front-cover {
        size: A4 portrait;
        margin: 0;
    }
    @page feature-page {
        size: A4 landscape;
        margin: 15mm;
    }
    @page pricing-page {
        size: A4 portrait;
        margin: 20mm;
    }
    @page back-cover {
        size: A4 portrait;
        margin: 0;
    }

    .front {
        page: front-cover;
        height: 100vh;
        background: linear-gradient(135deg, #ec4899 0%, #8b5cf6 100%);
        color: white;
        display: flex;
        justify-content: center;
        align-items: center;
    }
    .features {
        page: feature-page;
    }
    .pricing {
        page: pricing-page;
    }
    .back {
        page: back-cover;
        height: 100vh;
        background-color: #1f2937;
        color: white;
    }
</style>
<body>
    <div class="front">
        <h1 style="font-size: 48pt;">Premium Services</h1>
    </div>

    <div class="features">
        <h2>Feature Comparison</h2>
        <p>Wide format for detailed comparison tables...</p>
    </div>

    <div class="pricing">
        <h2>Pricing Plans</h2>
        <p>Portrait format for pricing tiers...</p>
    </div>

    <div class="back">
        <p>Contact information and company details...</p>
    </div>
</body>
```

### Example 7: Academic thesis with specialized sections

```html
<style>
    @page title-page {
        size: A4 portrait;
        margin: 50mm 30mm;
    }
    @page abstract-page {
        size: A4 portrait;
        margin: 35mm 30mm;
    }
    @page main-text {
        size: A4 portrait;
        margin: 25mm;
    }
    @page bibliography-page {
        size: A4 portrait;
        margin: 20mm 25mm;
    }

    .title {
        page: title-page;
        text-align: center;
    }
    .abstract {
        page: abstract-page;
    }
    .chapters {
        page: main-text;
    }
    .references {
        page: bibliography-page;
    }
    .thesis-title {
        font-size: 20pt;
        font-weight: bold;
        margin: 40pt 0;
    }
</style>
<body>
    <div class="title">
        <h1 class="thesis-title">
            Advanced Machine Learning Techniques<br/>
            for Predictive Analytics
        </h1>
        <p>by John Smith</p>
        <p>March 2025</p>
    </div>

    <div class="abstract">
        <h2>Abstract</h2>
        <p>This research investigates...</p>
    </div>

    <div class="chapters">
        <h2>Chapter 1: Introduction</h2>
        <p>Main content with standard formatting...</p>
    </div>

    <div class="references">
        <h2>References</h2>
        <p>[1] Smith, J. (2024). Machine Learning Fundamentals...</p>
    </div>
</body>
```

### Example 8: Restaurant menu with different page styles

```html
<style>
    @page menu-cover {
        size: 210mm 297mm;
        margin: 0;
    }
    @page menu-main {
        size: 210mm 297mm;
        margin: 20mm;
    }
    @page wine-list {
        size: 148mm 210mm;
        margin: 15mm;
    }

    .cover {
        page: menu-cover;
        height: 100vh;
        background-color: #1f2937;
        color: #f59e0b;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }
    .menu-items {
        page: menu-main;
    }
    .wines {
        page: wine-list;
    }
    .restaurant-name {
        font-size: 48pt;
        font-weight: bold;
        font-family: serif;
    }
    .menu-section {
        margin: 30pt 0;
    }
    .section-title {
        font-size: 18pt;
        font-weight: bold;
        border-bottom: 2pt solid #1e3a8a;
        padding-bottom: 8pt;
        margin-bottom: 15pt;
    }
</style>
<body>
    <div class="cover">
        <div class="restaurant-name">La Cuisine</div>
        <p style="font-size: 16pt;">Fine Dining Experience</p>
    </div>

    <div class="menu-items">
        <div class="menu-section">
            <h2 class="section-title">Appetizers</h2>
            <p>French Onion Soup ............ $12</p>
            <p>Caesar Salad ................. $10</p>
        </div>

        <div class="menu-section">
            <h2 class="section-title">Main Courses</h2>
            <p>Grilled Salmon ............... $28</p>
            <p>Beef Tenderloin .............. $35</p>
        </div>
    </div>

    <div class="wines">
        <h2>Wine Selection</h2>
        <p>Red Wines</p>
        <p>Cabernet Sauvignon ........... $45</p>
        <p>Merlot ....................... $38</p>
    </div>
</body>
```

### Example 9: Technical manual with specifications

```html
<style>
    @page manual-standard {
        size: A4 portrait;
        margin: 25mm;
    }
    @page specification-sheet {
        size: A4 landscape;
        margin: 15mm;
    }
    @page safety-page {
        size: A4 portrait;
        margin: 20mm;
        border: 5pt solid #dc2626;
    }

    .standard-content {
        page: manual-standard;
    }
    .specifications {
        page: specification-sheet;
    }
    .safety-info {
        page: safety-page;
    }
    .safety-header {
        background-color: #fef2f2;
        border: 2pt solid #dc2626;
        padding: 15pt;
        margin-bottom: 20pt;
    }
    .safety-title {
        color: #dc2626;
        font-size: 20pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="standard-content">
        <h1>Product Manual</h1>
        <p>Thank you for purchasing our product...</p>
    </div>

    <div class="specifications">
        <h2>Technical Specifications</h2>
        <table>
            <tr>
                <th>Parameter</th>
                <th>Value</th>
                <th>Range</th>
                <th>Tolerance</th>
            </tr>
            <tr>
                <td>Voltage</td>
                <td>120V</td>
                <td>110-130V</td>
                <td>±5%</td>
            </tr>
        </table>
    </div>

    <div class="safety-info">
        <div class="safety-header">
            <div class="safety-title">⚠ SAFETY WARNINGS</div>
        </div>
        <p>Read all safety instructions before operating this equipment...</p>
    </div>
</body>
```

### Example 10: Event program with varied sections

```html
<style>
    @page program-cover {
        size: A5 portrait;
        margin: 0;
    }
    @page schedule-page {
        size: A5 landscape;
        margin: 10mm;
    }
    @page speaker-bio {
        size: A5 portrait;
        margin: 15mm;
    }

    .program-front {
        page: program-cover;
        height: 100%;
        background: linear-gradient(135deg, #7c3aed 0%, #a855f7 100%);
        color: white;
        padding: 20mm;
        display: flex;
        flex-direction: column;
        justify-content: center;
    }
    .schedule {
        page: schedule-page;
    }
    .bios {
        page: speaker-bio;
    }
    .event-title {
        font-size: 28pt;
        font-weight: bold;
        margin-bottom: 15pt;
    }
</style>
<body>
    <div class="program-front">
        <h1 class="event-title">Innovation Summit 2025</h1>
        <p style="font-size: 16pt;">March 20-22, 2025</p>
    </div>

    <div class="schedule">
        <h2>Daily Schedule</h2>
        <p>9:00 AM - Opening Keynote</p>
        <p>10:30 AM - Breakout Sessions</p>
        <p>12:00 PM - Networking Lunch</p>
    </div>

    <div class="bios">
        <h3>Speaker: Dr. Sarah Chen</h3>
        <p>Dr. Chen is a renowned expert in artificial intelligence...</p>
    </div>
</body>
```

### Example 11: Real estate listing with property pages

```html
<style>
    @page listing-overview {
        size: Letter portrait;
        margin: 20mm;
    }
    @page photo-spread {
        size: Letter landscape;
        margin: 10mm;
    }
    @page floor-plans {
        size: 11in 17in;
        margin: 15mm;
    }

    .overview {
        page: listing-overview;
    }
    .photos {
        page: photo-spread;
    }
    .plans {
        page: floor-plans;
    }
    .property-title {
        font-size: 24pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 15pt;
    }
    .photo-grid {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 10pt;
    }
    .photo-placeholder {
        height: 150pt;
        background-color: #e5e7eb;
        border: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="overview">
        <h1 class="property-title">123 Main Street</h1>
        <p>Beautiful 4-bedroom, 3-bathroom home in desirable neighborhood...</p>
        <p>Price: $650,000</p>
    </div>

    <div class="photos">
        <h2>Property Photos</h2>
        <div class="photo-grid">
            <div class="photo-placeholder">[Living Room]</div>
            <div class="photo-placeholder">[Kitchen]</div>
            <div class="photo-placeholder">[Master Bedroom]</div>
            <div class="photo-placeholder">[Backyard]</div>
        </div>
    </div>

    <div class="plans">
        <h2>Floor Plans</h2>
        <p>Detailed architectural drawings...</p>
    </div>
</body>
```

### Example 12: Portfolio with project showcases

```html
<style>
    @page portfolio-intro {
        size: A4 portrait;
        margin: 30mm;
    }
    @page project-showcase {
        size: A4 landscape;
        margin: 15mm;
    }
    @page contact-page {
        size: A4 portrait;
        margin: 25mm;
    }

    .intro {
        page: portfolio-intro;
        text-align: center;
    }
    .project {
        page: project-showcase;
    }
    .contact {
        page: contact-page;
    }
    .designer-name {
        font-size: 36pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .project-title {
        font-size: 28pt;
        color: #1e3a8a;
        margin-bottom: 20pt;
    }
</style>
<body>
    <div class="intro">
        <h1 class="designer-name">Alex Johnson</h1>
        <p style="font-size: 16pt;">Creative Designer & Art Director</p>
    </div>

    <div class="project">
        <h2 class="project-title">Brand Identity - TechStart</h2>
        <p>Complete brand redesign for emerging technology startup...</p>
    </div>

    <div class="project">
        <h2 class="project-title">Website Redesign - HealthCare Pro</h2>
        <p>Modernizing healthcare platform interface...</p>
    </div>

    <div class="contact">
        <h2>Get In Touch</h2>
        <p>Email: alex@design.com</p>
        <p>Phone: (555) 123-4567</p>
    </div>
</body>
```

### Example 13: Conference proceedings with author pages

```html
<style>
    @page proceedings-title {
        size: A4 portrait;
        margin: 40mm 30mm;
    }
    @page paper-abstract {
        size: A4 portrait;
        margin: 25mm;
    }
    @page full-paper {
        size: A4 portrait;
        margin: 20mm;
        column-count: 2;
        column-gap: 10mm;
    }

    .title-page {
        page: proceedings-title;
        text-align: center;
    }
    .abstract {
        page: paper-abstract;
    }
    .paper-content {
        page: full-paper;
    }
    .proceedings-title {
        font-size: 24pt;
        font-weight: bold;
        margin: 30pt 0;
    }
</style>
<body>
    <div class="title-page">
        <h1 class="proceedings-title">
            International Conference on<br/>
            Artificial Intelligence 2025
        </h1>
        <p>Proceedings</p>
    </div>

    <div class="abstract">
        <h2>Paper 1: Deep Learning for Image Recognition</h2>
        <p><strong>Authors:</strong> Smith, J., Chen, L., Rodriguez, M.</p>
        <p><strong>Abstract:</strong> This paper presents a novel approach...</p>
    </div>

    <div class="paper-content">
        <h3>Introduction</h3>
        <p>The field of computer vision has advanced significantly...</p>
        <h3>Methodology</h3>
        <p>Our approach utilizes convolutional neural networks...</p>
    </div>
</body>
```

### Example 14: Wedding program with varied sections

```html
<style>
    @page wedding-cover {
        size: 140mm 200mm;
        margin: 0;
    }
    @page ceremony-order {
        size: 140mm 200mm;
        margin: 15mm;
    }
    @page reception-info {
        size: 140mm 200mm;
        margin: 15mm;
    }

    .cover {
        page: wedding-cover;
        height: 100%;
        background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        text-align: center;
        padding: 20mm;
    }
    .ceremony {
        page: ceremony-order;
    }
    .reception {
        page: reception-info;
    }
    .couple-names {
        font-size: 32pt;
        font-weight: bold;
        color: #92400e;
        font-family: serif;
        font-style: italic;
    }
</style>
<body>
    <div class="cover">
        <div class="couple-names">Sarah & Michael</div>
        <p style="font-size: 16pt; margin-top: 20pt;">
            April 15, 2025
        </p>
    </div>

    <div class="ceremony">
        <h2>Order of Ceremony</h2>
        <p>Processional</p>
        <p>Welcome & Opening Prayer</p>
        <p>Exchange of Vows</p>
        <p>Ring Exchange</p>
        <p>Pronouncement</p>
        <p>Recessional</p>
    </div>

    <div class="reception">
        <h2>Reception</h2>
        <p>Grand Ballroom<br/>
        The Riverside Hotel<br/>
        6:00 PM - Midnight</p>
        <p>Dinner will be served at 7:00 PM</p>
    </div>
</body>
```

### Example 15: Magazine with article-specific layouts

```html
<style>
    @page magazine-cover {
        size: Letter portrait;
        margin: 0;
    }
    @page toc-page {
        size: Letter portrait;
        margin: 15mm;
    }
    @page feature-article {
        size: Letter portrait;
        margin: 12mm;
        column-count: 3;
        column-gap: 8mm;
    }
    @page full-spread {
        size: 11in 17in landscape;
        margin: 10mm;
    }

    .cover {
        page: magazine-cover;
        height: 100vh;
        background-color: #dc2626;
        color: white;
    }
    .contents {
        page: toc-page;
    }
    .article {
        page: feature-article;
    }
    .spread {
        page: full-spread;
    }
    .magazine-title {
        font-size: 64pt;
        font-weight: bold;
        text-align: center;
        padding-top: 50mm;
    }
</style>
<body>
    <div class="cover">
        <h1 class="magazine-title">TECH TODAY</h1>
        <p style="text-align: center; font-size: 20pt;">March 2025 Issue</p>
    </div>

    <div class="contents">
        <h2>In This Issue</h2>
        <p>Feature: AI Revolution .............. 12</p>
        <p>Interview: Tech Leaders ............. 24</p>
    </div>

    <div class="article">
        <h2>The AI Revolution</h2>
        <p>Artificial intelligence is transforming every industry...</p>
    </div>

    <div class="spread">
        <h2>Infographic: Technology Timeline</h2>
        <p>Full spread visualization...</p>
    </div>
</body>
```

---

## See Also

- [@page rule](/reference/css_atrules/css_atrule_page) - Define page properties and margins
- [size](/reference/cssproperties/css_prop_size) - Set page dimensions in @page rule
- [break-before](/reference/cssproperties/css_prop_break-before) - Control page breaks before elements
- [break-after](/reference/cssproperties/css_prop_break-after) - Control page breaks after elements
- [margin (in @page)](/reference/css_atrules/css_atrule_page#margins) - Set page margins

---
