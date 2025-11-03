---
layout: default
title: column-width
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# column-width : Column Width Property

The `column-width` property specifies the ideal or minimum width for columns in a multi-column layout. Unlike `column-count` which sets an exact number of columns, `column-width` allows the browser to determine the optimal number of columns based on available space, making layouts more responsive and adaptable to different page sizes.

## Usage

```css
selector {
    column-width: value;
}
```

The column-width property suggests an ideal width for columns. The actual number of columns is calculated based on the container width and specified column width, with column-gap also considered in the calculation.

---

## Supported Values

### Length Units
- Points: `150pt`, `200pt`, `250pt`
- Pixels: `150px`, `200px`, `250px`
- Inches: `2in`, `2.5in`, `3in`
- Centimeters: `5cm`, `6cm`, `8cm`
- Millimeters: `50mm`, `60mm`, `80mm`
- Ems: `15em`, `20em`, `25em`

### auto (default)
The column width is determined by other properties such as `column-count`. When both are auto, single-column layout is used.

---

## Supported Elements

The `column-width` property can be applied to:
- Block containers (`<div>`, `<section>`, `<article>`)
- Content areas requiring flexible column layout
- Responsive multi-column designs
- All block-level elements that contain flowable content

---

## Notes

- Column-width specifies the ideal or minimum width for columns
- Actual column width may be wider to fill available space
- Cannot specify both column-width and column-count precisely - use one or the other
- When using column-width, the number of columns adapts to container size
- Provides more flexible, responsive column layouts than column-count
- Particularly useful for documents that might be viewed at different sizes
- Standard column widths for readability: 45-75 characters (approximately 250-450pt)
- Narrower columns (150-200pt) work well for brief content and lists
- Wider columns (300-400pt) suit detailed articles and reports
- In PDF generation, column-width helps create consistent layouts
- The browser/renderer will never create columns narrower than specified width
- If container is narrower than column-width + gaps, reverts to single column
- Combine with column-gap for complete control over multi-column appearance

---

## Data Binding

The column-width property integrates with data binding to create flexible, responsive multi-column layouts that automatically adapt to different page sizes and layout requirements. This enables truly responsive document designs that maintain optimal readability across various contexts.

### Example 1: Responsive column width based on page size

```html
<style>
    .article {
        column-gap: 25pt;
        padding: 30pt;
        text-align: justify;
        line-height: 1.7;
    }
    .article h2 {
        column-span: all;
        color: #1e3a8a;
        font-size: 24pt;
        margin: 0 0 15pt 0;
    }
    .article p {
        margin: 0 0 12pt 0;
    }
</style>
<body>
    <!-- Adaptive column width based on page dimensions -->
    <div class="article"
         style="column-width: {{pageSize === 'A4' ? '250pt' : (pageSize === 'Letter' ? '280pt' : '300pt')}}">
        <h2>{{articleTitle}}</h2>
        <p>{{articleContent}}</p>
    </div>

    <!-- Dynamic width for different document types -->
    <div class="article"
         style="column-width: {{documentType === 'magazine' ? '280pt' : (documentType === 'brochure' ? '200pt' : '250pt')}};
                column-gap: 22pt;">
        <h2>{{title}}</h2>
        <p>{{content}}</p>
    </div>
</body>
```

### Example 2: Configurable column layouts

```html
<style>
    .newsletter {
        padding: 40pt;
    }
    .newsletter-header {
        text-align: center;
        margin-bottom: 30pt;
        padding-bottom: 20pt;
        border-bottom: 3pt solid #1e3a8a;
    }
    .newsletter-content {
        text-align: justify;
        line-height: 1.6;
        font-size: 10pt;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>{{newsletterTitle}}</h1>
            <div>{{issueDate}}</div>
        </div>

        <!-- User-configurable column width -->
        <div class="newsletter-content"
             style="column-width: {{preferences.columnWidth || 220}}pt;
                    column-gap: {{preferences.columnGap || 20}}pt;">
            {{#each articles}}
            <h3 style="color: #1e3a8a; margin: 0 0 8pt 0;">{{this.title}}</h3>
            <p>{{this.content}}</p>
            {{/each}}
        </div>
    </div>
</body>
```

### Example 3: Adaptive layouts for multi-section documents

```html
<style>
    .document {
        padding: 40pt;
    }
    .section {
        margin-bottom: 30pt;
    }
    .section-header {
        font-size: 22pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 15pt;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #e5e7eb;
    }
    .section-content {
        text-align: justify;
        line-height: 1.7;
        font-size: 10pt;
    }
</style>
<body>
    <div class="document">
        <h1 style="text-align: center; font-size: 32pt; color: #1e3a8a; margin-bottom: 30pt;">
            {{documentTitle}}
        </h1>

        <!-- Each section can have custom column width -->
        {{#each sections}}
        <div class="section">
            <h2 class="section-header">{{this.title}}</h2>
            <div class="section-content"
                 style="column-width: {{this.layout.columnWidth || config.defaultColumnWidth}}pt;
                        column-gap: {{this.layout.columnGap || config.defaultColumnGap}}pt;">
                <p>{{this.content}}</p>
            </div>
        </div>
        {{/each}}

        <!-- Responsive width based on content density -->
        <div class="section">
            <h2 class="section-header">Additional Information</h2>
            <div class="section-content"
                 style="column-width: {{contentDensity === 'high' ? '180pt' : (contentDensity === 'medium' ? '250pt' : '320pt')}};
                        column-gap: {{contentDensity === 'high' ? '15pt' : '25pt'}};">
                <p>{{additionalContent}}</p>
            </div>
        </div>
    </div>
</body>
```

---

## Examples

### Example 1: Basic responsive column width

```html
<style>
    .article {
        column-width: 250pt;
        column-gap: 20pt;
        padding: 30pt;
        text-align: justify;
        line-height: 1.7;
    }
    .article h2 {
        column-span: all;
        color: #1e3a8a;
        font-size: 24pt;
        margin: 0 0 15pt 0;
    }
    .article p {
        margin: 0 0 12pt 0;
    }
</style>
<body>
    <div class="article">
        <h2>Flexible Column Layouts</h2>
        <p>Using column-width instead of column-count creates flexible layouts
        that adapt to available space. The layout engine determines the optimal
        number of columns based on the 250pt target width.</p>
        <p>In a 600pt wide container, this might create 2 columns. In an 800pt
        container, you might see 3 columns. The layout adapts automatically
        while maintaining optimal column width for readability.</p>
        <p>This approach is particularly valuable for documents that might be
        rendered at different sizes or viewed on different devices.</p>
    </div>
</body>
```

### Example 2: Narrow columns for lists and directories

```html
<style>
    .directory {
        padding: 30pt;
    }
    .directory-header {
        text-align: center;
        background-color: #1e3a8a;
        color: white;
        padding: 25pt;
        margin-bottom: 25pt;
    }
    .directory-header h1 {
        margin: 0;
        font-size: 28pt;
    }
    .contact-list {
        column-width: 180pt;
        column-gap: 15pt;
    }
    .contact-entry {
        break-inside: avoid;
        margin-bottom: 12pt;
        padding: 10pt;
        background-color: #f9fafb;
        border-left: 3pt solid #2563eb;
    }
    .contact-name {
        font-weight: bold;
        color: #1e3a8a;
        font-size: 10pt;
        margin-bottom: 5pt;
    }
    .contact-info {
        font-size: 8pt;
        color: #6b7280;
        line-height: 1.4;
    }
</style>
<body>
    <div class="directory">
        <div class="directory-header">
            <h1>Quick Reference Directory</h1>
        </div>

        <div class="contact-list">
            <div class="contact-entry">
                <div class="contact-name">Anderson, Sarah</div>
                <div class="contact-info">
                    Sales Manager<br/>
                    Ext: 2101
                </div>
            </div>

            <div class="contact-entry">
                <div class="contact-name">Brown, Michael</div>
                <div class="contact-info">
                    IT Director<br/>
                    Ext: 2205
                </div>
            </div>

            <div class="contact-entry">
                <div class="contact-name">Chen, Lisa</div>
                <div class="contact-info">
                    Marketing Lead<br/>
                    Ext: 2308
                </div>
            </div>

            <div class="contact-entry">
                <div class="contact-name">Davis, Robert</div>
                <div class="contact-info">
                    Operations Manager<br/>
                    Ext: 2412
                </div>
            </div>

            <div class="contact-entry">
                <div class="contact-name">Evans, Jennifer</div>
                <div class="contact-info">
                    HR Manager<br/>
                    Ext: 2515
                </div>
            </div>

            <div class="contact-entry">
                <div class="contact-name">Foster, David</div>
                <div class="contact-info">
                    Finance Director<br/>
                    Ext: 2620
                </div>
            </div>

            <div class="contact-entry">
                <div class="contact-name">Garcia, Maria</div>
                <div class="contact-info">
                    Customer Success<br/>
                    Ext: 2724
                </div>
            </div>

            <div class="contact-entry">
                <div class="contact-name">Harris, James</div>
                <div class="contact-info">
                    Product Manager<br/>
                    Ext: 2828
                </div>
            </div>

            <div class="contact-entry">
                <div class="contact-name">Jackson, Emily</div>
                <div class="contact-info">
                    Design Lead<br/>
                    Ext: 2932
                </div>
            </div>

            <div class="contact-entry">
                <div class="contact-name">Kim, Daniel</div>
                <div class="contact-info">
                    Engineering Manager<br/>
                    Ext: 3036
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 3: Wide columns for detailed content

```html
<style>
    .magazine {
        width: 650pt;
        margin: 0 auto;
        padding: 45pt;
    }
    .magazine-header {
        text-align: center;
        margin-bottom: 30pt;
        padding-bottom: 20pt;
        border-bottom: 3pt solid #1e3a8a;
    }
    .magazine-title {
        font-size: 40pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 10pt 0;
    }
    .issue-info {
        font-size: 12pt;
        color: #6b7280;
    }
    .feature-article {
        margin-bottom: 35pt;
    }
    .article-headline {
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 15pt;
    }
    .article-body {
        column-width: 300pt;
        column-gap: 30pt;
        text-align: justify;
        line-height: 1.8;
        font-size: 11pt;
    }
    .article-body p {
        margin: 0 0 14pt 0;
    }
</style>
<body>
    <div class="magazine">
        <div class="magazine-header">
            <div class="magazine-title">DESIGN QUARTERLY</div>
            <div class="issue-info">October 2025 | Vol. 15, No. 4</div>
        </div>

        <div class="feature-article">
            <h1 class="article-headline">The Renaissance of Print Design</h1>
            <div class="article-body">
                <p>As digital tools become increasingly sophisticated, we're
                witnessing a renaissance in document design that combines the
                best of traditional print aesthetics with modern capabilities.</p>

                <p>The use of column-width rather than fixed column counts
                exemplifies this evolution. By specifying an ideal column width
                of 300pt, we ensure optimal readability while allowing the layout
                to adapt gracefully to different page configurations.</p>

                <p>This approach respects fundamental principles of typography—
                that line length significantly impacts reading comfort—while
                embracing the flexibility that modern document generation provides.
                The result is layouts that look professional across various
                contexts and sizes.</p>

                <p>Professional publishers have long understood that 45-75
                characters per line represents the sweet spot for sustained
                reading. By targeting a 300pt column width, we naturally achieve
                this optimal range, creating documents that are as comfortable
                to read as they are beautiful to look at.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 4: Product showcase with flexible columns

```html
<style>
    .product-showcase {
        padding: 35pt;
        background-color: #f9fafb;
    }
    .showcase-header {
        text-align: center;
        background-color: white;
        padding: 30pt;
        margin-bottom: 30pt;
        border: 3pt solid #e5e7eb;
    }
    .showcase-title {
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 10pt 0;
    }
    .showcase-subtitle {
        font-size: 16pt;
        color: #6b7280;
        margin: 0;
    }
    .product-grid {
        column-width: 200pt;
        column-gap: 20pt;
    }
    .product-card {
        break-inside: avoid;
        background-color: white;
        border: 2pt solid #e5e7eb;
        padding: 15pt;
        margin-bottom: 20pt;
    }
    .product-image {
        width: 100%;
        height: 140pt;
        background-color: #dbeafe;
        margin-bottom: 12pt;
    }
    .product-name {
        font-weight: bold;
        font-size: 12pt;
        color: #1e3a8a;
        margin-bottom: 8pt;
    }
    .product-description {
        font-size: 9pt;
        line-height: 1.5;
        color: #6b7280;
        margin-bottom: 10pt;
    }
    .product-price {
        font-size: 16pt;
        font-weight: bold;
        color: #16a34a;
    }
</style>
<body>
    <div class="product-showcase">
        <div class="showcase-header">
            <div class="showcase-title">Featured Products</div>
            <div class="showcase-subtitle">Autumn Collection 2025</div>
        </div>

        <div class="product-grid">
            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Alpine Series Widget</div>
                <div class="product-description">
                    Premium quality construction with advanced features for
                    demanding applications.
                </div>
                <div class="product-price">$149.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Coastal Collection Tool</div>
                <div class="product-description">
                    Durable design perfect for both professional and personal use.
                </div>
                <div class="product-price">$89.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Urban Essentials Kit</div>
                <div class="product-description">
                    Complete solution with everything you need in one package.
                </div>
                <div class="product-price">$199.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Professional Grade System</div>
                <div class="product-description">
                    Industry-leading performance meets intuitive operation.
                </div>
                <div class="product-price">$349.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Compact Traveler Model</div>
                <div class="product-description">
                    Lightweight and portable without compromising functionality.
                </div>
                <div class="product-price">$79.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Executive Edition Set</div>
                <div class="product-description">
                    Luxury materials and premium finish for discerning users.
                </div>
                <div class="product-price">$449.99</div>
            </div>
        </div>
    </div>
</body>
```

### Example 5: Newsletter with adaptive columns

```html
<style>
    .newsletter {
        padding: 40pt;
    }
    .newsletter-masthead {
        text-align: center;
        background-color: #1e3a8a;
        color: white;
        padding: 30pt;
        margin-bottom: 30pt;
    }
    .newsletter-name {
        font-size: 40pt;
        font-weight: bold;
        margin: 0 0 8pt 0;
    }
    .newsletter-tagline {
        font-size: 14pt;
        font-style: italic;
        margin: 0;
    }
    .newsletter-date {
        font-size: 11pt;
        margin: 10pt 0 0 0;
    }
    .newsletter-content {
        column-width: 220pt;
        column-gap: 22pt;
        text-align: justify;
        line-height: 1.6;
        font-size: 10pt;
    }
    .newsletter-content h3 {
        color: #1e3a8a;
        font-size: 14pt;
        margin: 0 0 10pt 0;
        break-after: avoid;
    }
    .newsletter-content p {
        margin: 0 0 12pt 0;
    }
    .news-item {
        break-inside: avoid;
        margin-bottom: 18pt;
        padding-bottom: 18pt;
        border-bottom: 1pt solid #e5e7eb;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-masthead">
            <div class="newsletter-name">TECH BRIEFING</div>
            <div class="newsletter-tagline">Your Weekly Technology Update</div>
            <div class="newsletter-date">October 14, 2025 | Issue #42</div>
        </div>

        <div class="newsletter-content">
            <div class="news-item">
                <h3>PDF Generation Advances</h3>
                <p>New capabilities in document generation are transforming how
                businesses create professional communications. Automated systems
                now rival traditional design tools in output quality while
                dramatically reducing production time.</p>
            </div>

            <div class="news-item">
                <h3>Layout Techniques</h3>
                <p>Modern CSS properties like column-width enable responsive
                multi-column layouts that adapt to different page sizes. This
                flexibility ensures consistent, professional appearance across
                various document formats and viewing contexts.</p>
            </div>

            <div class="news-item">
                <h3>Industry Adoption</h3>
                <p>Organizations across sectors are embracing automated document
                generation. Financial services, healthcare, and legal firms lead
                adoption, driven by needs for compliance, consistency, and scale.</p>
            </div>

            <div class="news-item">
                <h3>Best Practices</h3>
                <p>Experts recommend focusing on fundamentals: proper spacing,
                readable type sizes, and logical information hierarchy. Technical
                sophistication should enhance rather than overwhelm content.</p>
            </div>

            <div class="news-item">
                <h3>Future Outlook</h3>
                <p>The next generation of tools promises even greater capabilities.
                AI-assisted layout, dynamic personalization, and real-time
                collaboration will redefine document creation workflows.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 6: Academic journal format

```html
<style>
    .journal-article {
        width: 600pt;
        margin: 0 auto;
        padding: 50pt 40pt;
    }
    .article-title {
        text-align: center;
        font-size: 22pt;
        font-weight: bold;
        color: #1f2937;
        line-height: 1.3;
        margin-bottom: 18pt;
    }
    .author-info {
        text-align: center;
        margin-bottom: 12pt;
    }
    .authors {
        font-size: 12pt;
        color: #1f2937;
        margin-bottom: 8pt;
    }
    .affiliation {
        font-size: 10pt;
        color: #6b7280;
        font-style: italic;
    }
    .abstract-section {
        padding: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 30pt;
    }
    .abstract-title {
        font-weight: bold;
        font-size: 13pt;
        color: #1e3a8a;
        margin-bottom: 10pt;
    }
    .abstract-text {
        font-size: 10pt;
        line-height: 1.7;
        text-align: justify;
    }
    .article-content {
        column-width: 280pt;
        column-gap: 25pt;
        text-align: justify;
        line-height: 1.7;
        font-size: 10pt;
    }
    .article-content h2 {
        column-span: all;
        font-size: 16pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 25pt 0 12pt 0;
        border-bottom: 2pt solid #e5e7eb;
        padding-bottom: 6pt;
    }
    .article-content p {
        margin: 0 0 12pt 0;
    }
</style>
<body>
    <div class="journal-article">
        <h1 class="article-title">
            Optimal Column Width in Automated Document Generation:
            An Empirical Study
        </h1>

        <div class="author-info">
            <div class="authors">
                Dr. Elizabeth Martinez, PhD and Prof. James Richardson, DSc
            </div>
            <div class="affiliation">
                Institute for Document Technology Research, University of Design Sciences
            </div>
        </div>

        <div class="abstract-section">
            <div class="abstract-title">Abstract</div>
            <div class="abstract-text">
                This study examines the impact of column width on readability and
                comprehension in automatically generated PDF documents. Through
                controlled experiments with 500 participants, we demonstrate that
                column widths between 250-300 points optimize reading speed and
                retention. Our findings support the use of flexible column-width
                properties over fixed column counts in responsive document layouts.
            </div>
        </div>

        <div class="article-content">
            <h2>1. Introduction</h2>
            <p>The advent of sophisticated document generation systems has renewed
            interest in classical typography principles. While digital tools offer
            unprecedented flexibility, fundamental questions about optimal layout
            parameters remain relevant.</p>
            <p>This research addresses a specific aspect of multi-column layout:
            the relationship between column width and reading efficacy. Our work
            builds upon established typography research while accounting for the
            unique characteristics of generated PDF documents.</p>

            <h2>2. Methodology</h2>
            <p>We conducted controlled reading comprehension experiments using
            documents with varying column widths. Participants read identical
            content formatted with column widths ranging from 150pt to 400pt,
            with column gaps held constant at 20pt.</p>
            <p>Reading speed, comprehension accuracy, and subjective preference
            were measured for each configuration. Statistical analysis revealed
            significant correlations between column width and all measured outcomes.</p>

            <h2>3. Results</h2>
            <p>The data strongly support an optimal range of 250-300pt for column
            width in standard business documents. Reading speeds peaked at 280pt,
            while comprehension remained consistently high across the 250-330pt
            range. Subjective preference aligned closely with objective metrics.</p>
        </div>
    </div>
</body>
```

### Example 7: Brochure with mixed column widths

```html
<style>
    .brochure {
        padding: 40pt;
    }
    .brochure-hero {
        text-align: center;
        background: linear-gradient(135deg, #1e3a8a 0%, #2563eb 100%);
        color: white;
        padding: 40pt;
        margin-bottom: 35pt;
    }
    .hero-title {
        font-size: 44pt;
        font-weight: bold;
        margin: 0 0 12pt 0;
    }
    .hero-subtitle {
        font-size: 18pt;
        margin: 0;
    }
    .intro-section {
        margin-bottom: 30pt;
        text-align: center;
    }
    .intro-text {
        font-size: 15pt;
        line-height: 1.8;
        color: #1f2937;
        margin: 0 auto;
        max-width: 500pt;
    }
    .features-section {
        column-width: 250pt;
        column-gap: 28pt;
        margin-bottom: 30pt;
    }
    .feature-block {
        break-inside: avoid;
        margin-bottom: 22pt;
        padding: 18pt;
        background-color: #f9fafb;
        border-left: 5pt solid #2563eb;
    }
    .feature-number {
        display: inline-block;
        width: 35pt;
        height: 35pt;
        background-color: #2563eb;
        color: white;
        border-radius: 18pt;
        text-align: center;
        line-height: 35pt;
        font-weight: bold;
        font-size: 16pt;
        margin-bottom: 10pt;
    }
    .feature-heading {
        font-weight: bold;
        font-size: 14pt;
        color: #1e3a8a;
        margin-bottom: 8pt;
    }
    .feature-text {
        font-size: 10pt;
        line-height: 1.6;
        color: #6b7280;
    }
</style>
<body>
    <div class="brochure">
        <div class="brochure-hero">
            <div class="hero-title">Transform Your Business</div>
            <div class="hero-subtitle">Innovative Solutions for Modern Challenges</div>
        </div>

        <div class="intro-section">
            <p class="intro-text">
                We partner with forward-thinking organizations to deliver exceptional
                results through cutting-edge technology and proven methodologies.
            </p>
        </div>

        <div class="features-section">
            <div class="feature-block">
                <div class="feature-number">1</div>
                <div class="feature-heading">Strategic Planning</div>
                <div class="feature-text">
                    Comprehensive analysis and strategic roadmap development to guide
                    your organization toward its goals with clarity and confidence.
                </div>
            </div>

            <div class="feature-block">
                <div class="feature-number">2</div>
                <div class="feature-heading">Implementation Excellence</div>
                <div class="feature-text">
                    Expert execution of complex projects with attention to detail,
                    risk management, and quality assurance at every stage.
                </div>
            </div>

            <div class="feature-block">
                <div class="feature-number">3</div>
                <div class="feature-heading">Continuous Improvement</div>
                <div class="feature-text">
                    Ongoing optimization and refinement to ensure sustained success
                    and adaptation to changing business requirements.
                </div>
            </div>

            <div class="feature-block">
                <div class="feature-number">4</div>
                <div class="feature-heading">Team Enablement</div>
                <div class="feature-text">
                    Comprehensive training and knowledge transfer to empower your
                    team with skills needed for long-term independence.
                </div>
            </div>

            <div class="feature-block">
                <div class="feature-number">5</div>
                <div class="feature-heading">Measurable Results</div>
                <div class="feature-text">
                    Clear metrics and regular reporting to demonstrate value and
                    track progress against defined success criteria.
                </div>
            </div>

            <div class="feature-block">
                <div class="feature-number">6</div>
                <div class="feature-heading">Partnership Approach</div>
                <div class="feature-text">
                    Collaborative relationships built on trust, transparency, and
                    mutual commitment to achieving exceptional outcomes.
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 8: FAQ document with optimal width

```html
<style>
    .faq {
        width: 650pt;
        margin: 0 auto;
        padding: 40pt;
    }
    .faq-header {
        text-align: center;
        margin-bottom: 35pt;
    }
    .faq-title {
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 12pt 0;
    }
    .faq-intro {
        font-size: 13pt;
        color: #6b7280;
        margin: 0;
    }
    .faq-content {
        column-width: 300pt;
        column-gap: 30pt;
    }
    .faq-category {
        column-span: all;
        font-size: 20pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 25pt 0 15pt 0;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #e5e7eb;
    }
    .faq-item {
        break-inside: avoid;
        margin-bottom: 20pt;
    }
    .faq-question {
        font-weight: bold;
        color: #2563eb;
        font-size: 12pt;
        margin-bottom: 8pt;
    }
    .faq-answer {
        font-size: 10pt;
        line-height: 1.7;
        color: #4b5563;
        text-align: justify;
    }
</style>
<body>
    <div class="faq">
        <div class="faq-header">
            <h1 class="faq-title">Frequently Asked Questions</h1>
            <p class="faq-intro">Find answers to common questions about our services</p>
        </div>

        <div class="faq-content">
            <h2 class="faq-category">Getting Started</h2>

            <div class="faq-item">
                <div class="faq-question">Q: How do I begin using the service?</div>
                <div class="faq-answer">
                    Simply create an account on our website and follow the guided
                    setup process. Our intuitive interface walks you through each
                    step, and help is available if needed.
                </div>
            </div>

            <div class="faq-item">
                <div class="faq-question">Q: What are the system requirements?</div>
                <div class="faq-answer">
                    Our service works with all modern web browsers and requires
                    no special software installation. A stable internet connection
                    is recommended for optimal performance.
                </div>
            </div>

            <h2 class="faq-category">Features and Capabilities</h2>

            <div class="faq-item">
                <div class="faq-question">Q: Can I customize document layouts?</div>
                <div class="faq-answer">
                    Yes, our platform offers extensive customization options including
                    column layouts, spacing, typography, and branding elements. The
                    column-width property enables responsive multi-column designs.
                </div>
            </div>

            <div class="faq-item">
                <div class="faq-question">Q: What file formats are supported?</div>
                <div class="faq-answer">
                    We support PDF, HTML, and various image formats. PDF output
                    maintains precise formatting and is ideal for professional
                    documents requiring consistent appearance across platforms.
                </div>
            </div>

            <h2 class="faq-category">Billing and Plans</h2>

            <div class="faq-item">
                <div class="faq-question">Q: What pricing plans are available?</div>
                <div class="faq-answer">
                    We offer flexible plans for individuals, teams, and enterprises.
                    All plans include core features with higher tiers providing
                    advanced capabilities and priority support.
                </div>
            </div>

            <div class="faq-item">
                <div class="faq-question">Q: Can I change my plan later?</div>
                <div class="faq-answer">
                    Absolutely. You can upgrade or downgrade your plan at any time.
                    Changes take effect immediately, and billing adjusts proportionally
                    for the current period.
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 9: Event program with flexible layout

```html
<style>
    .program {
        padding: 38pt;
    }
    .program-cover {
        text-align: center;
        background-color: #1e3a8a;
        color: white;
        padding: 45pt;
        margin-bottom: 35pt;
    }
    .event-name {
        font-size: 42pt;
        font-weight: bold;
        margin: 0 0 15pt 0;
        line-height: 1.1;
    }
    .event-details {
        font-size: 16pt;
        margin: 0;
    }
    .schedule-content {
        column-width: 260pt;
        column-gap: 25pt;
    }
    .time-block {
        break-inside: avoid;
        margin-bottom: 25pt;
    }
    .time-header {
        font-size: 16pt;
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 12pt;
        padding-bottom: 8pt;
        border-bottom: 2pt solid #dbeafe;
    }
    .session {
        break-inside: avoid;
        margin-bottom: 15pt;
        padding-left: 15pt;
        border-left: 3pt solid #e5e7eb;
    }
    .session-title {
        font-weight: bold;
        font-size: 11pt;
        color: #1e3a8a;
        margin-bottom: 5pt;
    }
    .session-speaker {
        font-size: 9pt;
        color: #6b7280;
        font-style: italic;
        margin-bottom: 5pt;
    }
    .session-location {
        font-size: 9pt;
        color: #9ca3af;
    }
</style>
<body>
    <div class="program">
        <div class="program-cover">
            <div class="event-name">Annual Technology Conference 2025</div>
            <div class="event-details">
                October 25-27 | Convention Center | City, State
            </div>
        </div>

        <div class="schedule-content">
            <div class="time-block">
                <div class="time-header">Friday, October 25 | Morning</div>

                <div class="session">
                    <div class="session-title">Opening Keynote: The Future of Technology</div>
                    <div class="session-speaker">Dr. Sarah Johnson, CEO TechCorp</div>
                    <div class="session-location">Main Auditorium | 9:00 AM - 10:30 AM</div>
                </div>

                <div class="session">
                    <div class="session-title">Workshop: Advanced PDF Generation</div>
                    <div class="session-speaker">Michael Chen, Lead Developer</div>
                    <div class="session-location">Room 201 | 11:00 AM - 12:30 PM</div>
                </div>
            </div>

            <div class="time-block">
                <div class="time-header">Friday, October 25 | Afternoon</div>

                <div class="session">
                    <div class="session-title">Panel: Document Automation Trends</div>
                    <div class="session-speaker">Industry Experts Panel</div>
                    <div class="session-location">Main Auditorium | 2:00 PM - 3:30 PM</div>
                </div>

                <div class="session">
                    <div class="session-title">Technical Deep Dive: Layout Systems</div>
                    <div class="session-speaker">Alexandra Martinez, Architect</div>
                    <div class="session-location">Room 105 | 4:00 PM - 5:30 PM</div>
                </div>
            </div>

            <div class="time-block">
                <div class="time-header">Saturday, October 26 | Morning</div>

                <div class="session">
                    <div class="session-title">Case Study: Enterprise Implementation</div>
                    <div class="session-speaker">Robert Williams, CTO GlobalBank</div>
                    <div class="session-location">Main Auditorium | 9:00 AM - 10:30 AM</div>
                </div>

                <div class="session">
                    <div class="session-title">Hands-on Lab: Multi-Column Layouts</div>
                    <div class="session-speaker">Jennifer Lee, UX Designer</div>
                    <div class="session-location">Lab 3A | 11:00 AM - 1:00 PM</div>
                </div>
            </div>

            <div class="time-block">
                <div class="time-header">Saturday, October 26 | Afternoon</div>

                <div class="session">
                    <div class="session-title">Best Practices in Document Design</div>
                    <div class="session-speaker">David Foster, Design Director</div>
                    <div class="session-location">Room 210 | 2:00 PM - 3:30 PM</div>
                </div>

                <div class="session">
                    <div class="session-title">Closing Keynote: Innovation Ahead</div>
                    <div class="session-speaker">Lisa Anderson, VP Product</div>
                    <div class="session-location">Main Auditorium | 4:00 PM - 5:00 PM</div>
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 10: Report with adaptive column structure

```html
<style>
    .report {
        width: 620pt;
        margin: 0 auto;
        padding: 45pt;
    }
    .report-cover {
        text-align: center;
        margin-bottom: 40pt;
        padding-bottom: 30pt;
        border-bottom: 4pt solid #1e3a8a;
    }
    .report-title {
        font-size: 32pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 15pt 0;
        line-height: 1.2;
    }
    .report-subtitle {
        font-size: 16pt;
        color: #6b7280;
        margin: 0 0 15pt 0;
    }
    .report-meta {
        font-size: 12pt;
        color: #9ca3af;
    }
    .executive-summary {
        background-color: #f9fafb;
        padding: 25pt;
        margin-bottom: 35pt;
        border-left: 6pt solid #2563eb;
    }
    .summary-heading {
        font-size: 18pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 15pt 0;
    }
    .summary-text {
        font-size: 12pt;
        line-height: 1.8;
        color: #1f2937;
        text-align: justify;
    }
    .report-content {
        column-width: 285pt;
        column-gap: 30pt;
        text-align: justify;
        line-height: 1.7;
        font-size: 10pt;
    }
    .report-content h2 {
        column-span: all;
        font-size: 20pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 30pt 0 15pt 0;
        border-bottom: 3pt solid #e5e7eb;
        padding-bottom: 10pt;
    }
    .report-content p {
        margin: 0 0 12pt 0;
    }
    .report-content .highlight {
        background-color: #fef3c7;
        padding: 2pt 5pt;
        border-radius: 2pt;
    }
</style>
<body>
    <div class="report">
        <div class="report-cover">
            <h1 class="report-title">Strategic Technology Assessment</h1>
            <div class="report-subtitle">
                Comprehensive Analysis and Recommendations for Digital Transformation
            </div>
            <div class="report-meta">
                Prepared for Executive Leadership | October 2025
            </div>
        </div>

        <div class="executive-summary">
            <div class="summary-heading">Executive Summary</div>
            <p class="summary-text">
                This assessment examines current technology infrastructure and
                recommends strategic initiatives for modernization. Key findings
                indicate significant opportunities for improvement in document
                automation, with potential cost savings of 35% and quality improvements
                measured at 42% through systematic implementation of advanced
                generation technologies.
            </p>
        </div>

        <div class="report-content">
            <h2>Current State Analysis</h2>
            <p>Our comprehensive review reveals a mixed technology landscape with
            both strengths and areas requiring attention. Legacy systems continue
            to provide value but increasingly constrain operational efficiency and
            innovation capability.</p>
            <p>Document generation processes rely heavily on manual intervention,
            resulting in inconsistent output quality and substantial time investment.
            The 285pt <span class="highlight">column-width setting</span> in this
            report ensures optimal readability while adapting to different page
            configurations.</p>

            <h2>Strategic Recommendations</h2>
            <p>We recommend phased implementation of modern document automation
            systems, beginning with high-volume, standardized outputs where benefits
            will materialize quickly. This approach minimizes risk while demonstrating
            clear value to stakeholders.</p>
            <p>Investment in automated generation capabilities will yield significant
            returns through reduced production time, improved consistency, and
            enhanced ability to personalize communications at scale. Total cost
            of ownership analysis projects positive ROI within 18 months.</p>

            <h2>Implementation Roadmap</h2>
            <p>Phase one focuses on infrastructure modernization and tool selection.
            Phase two implements pilot projects in controlled environments. Phase
            three scales successful patterns across the organization while maintaining
            quality standards and user satisfaction.</p>
        </div>
    </div>
</body>
```

---

## See Also

- [column-count](/reference/cssproperties/css_prop_column-count) - Set exact number of columns
- [column-gap](/reference/cssproperties/css_prop_column-gap) - Set spacing between columns
- [column-span](/reference/cssproperties/css_prop_column-span) - Make elements span columns
- [width](/reference/cssproperties/css_prop_width) - Set element width
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
