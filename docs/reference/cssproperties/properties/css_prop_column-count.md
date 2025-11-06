---
layout: default
title: column-count
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# column-count : Column Count Property

The `column-count` property specifies the number of columns an element should be divided into for multi-column layout. This is essential for creating newspaper-style layouts, magazine columns, brochures, and professional multi-column documents in PDF generation.

## Usage

```css
selector {
    column-count: value;
}
```

The column-count property divides content into the specified number of equal-width columns, automatically flowing content from one column to the next.

---

## Supported Values

### Integer Values
- `1` - Single column (default, normal flow)
- `2` - Two-column layout
- `3` - Three-column layout
- `4` - Four-column layout
- Any positive integer - Divides content into that many columns

### auto (default)
The number of columns is determined by other properties like `column-width`. When both column-count and column-width are auto, the element displays in single-column layout.

---

## Supported Elements

The `column-count` property can be applied to:
- Block containers (`<div>`, `<section>`, `<article>`)
- Content areas requiring multi-column flow
- Text-heavy sections (articles, reports, documentation)
- All block-level elements that contain flowable content

---

## Notes

- Column-count creates equal-width columns that span the element's width
- Content flows automatically from the bottom of one column to the top of the next
- Use with `column-gap` to control spacing between columns
- Columns are balanced by default - content distributes evenly across columns
- Headlines and other elements can span all columns using `column-span: all`
- Images and other content flow within columns naturally
- In PDF generation, columns adapt to page width constraints
- Column breaks can be controlled with page-break and break properties
- Best used for text-heavy content that benefits from narrower reading lines
- Multi-column layouts improve readability by reducing line length
- Particularly effective for newsletters, magazines, and brochures
- Column count applies to the direct content of the element

---

## Data Binding

The column-count property works seamlessly with data binding to create dynamic multi-column layouts that adapt based on document preferences, page sizes, and content requirements. This enables flexible magazine layouts, responsive newsletters, and configurable reports.

### Example 1: Dynamic column count based on page size

```html
<style>
    .article {
        column-gap: 20pt;
        padding: 30pt;
        text-align: justify;
        line-height: 1.7;
    }
    .article h2 {
        column-span: all;
        color: #1e3a8a;
        margin: 0 0 15pt 0;
    }
    .article p {
        margin: 0 0 12pt 0;
    }
</style>
<body>
    <!-- Adapt column count based on page width -->
    <div class="article" style="column-count: {{pageSize === 'A4' ? 2 : (pageSize === 'Letter' ? 2 : 3)}}">
        <h2>{{articleTitle}}</h2>
        <p>{{articleContent}}</p>
    </div>

    <!-- Conditional columns based on content length -->
    <div class="article" style="column-count: {{contentWords > 500 ? 3 : 2}}; column-gap: 25pt;">
        <h2>{{title}}</h2>
        <p>{{content}}</p>
    </div>
</body>
```

### Example 2: User-configurable newsletter layout

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
        line-height: 1.7;
        font-size: 11pt;
    }
    .newsletter-content h3 {
        color: #1e3a8a;
        margin: 0 0 10pt 0;
    }
    .newsletter-content p {
        margin: 0 0 10pt 0;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>{{newsletterTitle}}</h1>
            <p>{{publishDate}}</p>
        </div>

        <!-- Dynamic column layout based on user preferences -->
        <div class="newsletter-content"
             style="column-count: {{layout.columnCount}};
                    column-gap: {{layout.columnGap}}pt;">
            {{#each articles}}
            <h3>{{this.title}}</h3>
            <p>{{this.content}}</p>
            {{/each}}
        </div>
    </div>
</body>
```

### Example 3: Responsive multi-column documents

```html
<style>
    .document {
        width: 100%;
        padding: 35pt;
    }
    .document-title {
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
        text-align: center;
        margin-bottom: 25pt;
    }
    .document-content {
        text-align: justify;
        line-height: 1.8;
        font-size: 11pt;
    }
</style>
<body>
    <div class="document">
        <h1 class="document-title">{{documentTitle}}</h1>

        <!-- Column count based on document type and format -->
        <div class="document-content"
             style="column-count: {{
                     documentType === 'brochure' ? 3 :
                     (documentType === 'magazine' ? 2 :
                     (documentType === 'report' ? (format === 'compact' ? 2 : 1) : 1))
                 }};
                    column-gap: {{
                     documentType === 'brochure' ? '20pt' :
                     (documentType === 'magazine' ? '30pt' : '25pt')
                 }};">
            <p>{{documentBody}}</p>
        </div>

        <!-- Adaptive columns for different sections -->
        {{#each sections}}
        <div style="column-count: {{this.preferredColumns || config.defaultColumns}};
                    column-gap: 22pt;
                    margin-top: 25pt;">
            <h2 style="column-span: all; color: #1e3a8a; margin-bottom: 15pt;">{{this.title}}</h2>
            <p>{{this.content}}</p>
        </div>
        {{/each}}
    </div>
</body>
```

---

## Examples

### Example 1: Basic two-column layout

```html
<style>
    .two-column {
        column-count: 2;
        column-gap: 20pt;
        padding: 30pt;
        text-align: justify;
        line-height: 1.6;
    }
    .two-column h2 {
        margin: 0 0 15pt 0;
        color: #1e3a8a;
    }
    .two-column p {
        margin: 0 0 12pt 0;
    }
</style>
<body>
    <div class="two-column">
        <h2>Article Title</h2>
        <p>This content is displayed in a two-column layout, similar to
        newspaper articles. The text flows naturally from the bottom of the
        first column to the top of the second column.</p>
        <p>Multi-column layouts are ideal for text-heavy documents where
        narrower columns improve readability. Research shows that line lengths
        of 45-75 characters are optimal for comfortable reading.</p>
        <p>Additional paragraphs continue to flow through the columns,
        creating a professional, magazine-style appearance that is perfect
        for reports, newsletters, and documentation.</p>
    </div>
</body>
```

### Example 2: Three-column newsletter

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
    .newsletter-header h1 {
        margin: 0 0 10pt 0;
        font-size: 32pt;
        color: #1e3a8a;
    }
    .newsletter-tagline {
        font-size: 14pt;
        color: #6b7280;
        font-style: italic;
    }
    .newsletter-content {
        column-count: 3;
        column-gap: 25pt;
        text-align: justify;
        line-height: 1.7;
        font-size: 11pt;
    }
    .newsletter-content h3 {
        margin: 0 0 10pt 0;
        color: #1e3a8a;
        font-size: 14pt;
    }
    .newsletter-content p {
        margin: 0 0 10pt 0;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>TECH INSIGHTS</h1>
            <div class="newsletter-tagline">Your Monthly Technology Newsletter</div>
        </div>
        <div class="newsletter-content">
            <h3>Industry Updates</h3>
            <p>The technology sector continues to evolve rapidly with new
            innovations emerging daily. Companies are investing heavily in
            document automation and PDF generation technologies.</p>
            <p>Market analysis shows strong growth in enterprise document
            solutions, with organizations seeking efficient ways to create
            and manage professional documents at scale.</p>

            <h3>Product Spotlight</h3>
            <p>This month we highlight advanced layout features including
            multi-column support for creating newspaper and magazine style
            documents. These capabilities enable professional publishing
            directly from your applications.</p>

            <h3>Tips and Tricks</h3>
            <p>Use column-count to create balanced, readable layouts. Combine
            with column-gap for proper spacing and column-span to create
            headers that stretch across all columns.</p>
            <p>Remember that shorter line lengths improve readability,
            making multi-column layouts ideal for text-heavy content.</p>
        </div>
    </div>
</body>
```

### Example 3: Magazine article layout

```html
<style>
    .magazine-page {
        width: 600pt;
        margin: 0 auto;
        padding: 40pt;
        background-color: white;
    }
    .article-header {
        margin-bottom: 30pt;
    }
    .article-title {
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 10pt 0;
        line-height: 1.2;
    }
    .article-byline {
        font-size: 12pt;
        color: #6b7280;
        font-style: italic;
        margin-bottom: 15pt;
    }
    .lead-text {
        font-size: 14pt;
        line-height: 1.8;
        color: #1f2937;
        margin-bottom: 25pt;
        font-weight: 500;
    }
    .article-body {
        column-count: 2;
        column-gap: 30pt;
        text-align: justify;
        line-height: 1.7;
        font-size: 11pt;
    }
    .article-body p {
        margin: 0 0 12pt 0;
    }
</style>
<body>
    <div class="magazine-page">
        <div class="article-header">
            <h1 class="article-title">The Future of Document Design</h1>
            <div class="article-byline">By Alexandra Thompson | October 2025</div>
            <div class="lead-text">
                Exploring how multi-column layouts transform the way we create
                and consume professional documents in the digital age.
            </div>
        </div>
        <div class="article-body">
            <p>Multi-column layouts have been a staple of print media for
            centuries, from newspapers to magazines. Now, with advanced PDF
            generation capabilities, we can bring these sophisticated layouts
            to digitally-generated documents.</p>
            <p>The key to effective multi-column design lies in understanding
            how readers process information. Studies show that line lengths
            between 45-75 characters optimize reading comprehension and speed.</p>
            <p>By dividing content into columns, we naturally constrain line
            length, making text more approachable and easier to scan. This is
            particularly valuable for lengthy documents where reader engagement
            is crucial.</p>
            <p>Modern document generation tools make it simple to apply these
            principles, enabling anyone to create professional, publication-
            quality layouts without specialized design software.</p>
        </div>
    </div>
</body>
```

### Example 4: Brochure with mixed layouts

```html
<style>
    .brochure {
        padding: 35pt;
    }
    .brochure-header {
        text-align: center;
        background-color: #1e3a8a;
        color: white;
        padding: 30pt;
        margin-bottom: 30pt;
    }
    .brochure-header h1 {
        margin: 0 0 10pt 0;
        font-size: 32pt;
    }
    .intro-section {
        margin-bottom: 30pt;
        font-size: 13pt;
        line-height: 1.8;
        text-align: center;
        color: #1f2937;
    }
    .features-section {
        column-count: 3;
        column-gap: 20pt;
        margin-bottom: 30pt;
    }
    .feature-box {
        break-inside: avoid;
        margin-bottom: 15pt;
        padding: 15pt;
        background-color: #f3f4f6;
        border-left: 4pt solid #2563eb;
    }
    .feature-title {
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 8pt;
        font-size: 12pt;
    }
    .feature-description {
        font-size: 10pt;
        line-height: 1.5;
        color: #6b7280;
    }
</style>
<body>
    <div class="brochure">
        <div class="brochure-header">
            <h1>Premium Services</h1>
            <p style="margin: 0;">Excellence in Every Detail</p>
        </div>

        <div class="intro-section">
            <p style="margin: 0;">Discover our comprehensive range of services
            designed to meet your business needs with precision and professionalism.</p>
        </div>

        <div class="features-section">
            <div class="feature-box">
                <div class="feature-title">Consulting Services</div>
                <div class="feature-description">
                    Expert guidance to help you achieve your strategic objectives
                    with proven methodologies.
                </div>
            </div>

            <div class="feature-box">
                <div class="feature-title">Custom Development</div>
                <div class="feature-description">
                    Tailored solutions built to your exact specifications using
                    cutting-edge technologies.
                </div>
            </div>

            <div class="feature-box">
                <div class="feature-title">24/7 Support</div>
                <div class="feature-description">
                    Round-the-clock assistance ensuring your systems run smoothly
                    without interruption.
                </div>
            </div>

            <div class="feature-box">
                <div class="feature-title">Training Programs</div>
                <div class="feature-description">
                    Comprehensive training to empower your team with essential
                    skills and knowledge.
                </div>
            </div>

            <div class="feature-box">
                <div class="feature-title">Cloud Integration</div>
                <div class="feature-description">
                    Seamless integration with cloud platforms for scalability
                    and reliability.
                </div>
            </div>

            <div class="feature-box">
                <div class="feature-title">Security Audits</div>
                <div class="feature-description">
                    Thorough security assessments to protect your valuable
                    data and systems.
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 5: Academic paper format

```html
<style>
    .paper {
        width: 550pt;
        margin: 0 auto;
        padding: 50pt 40pt;
    }
    .paper-title {
        text-align: center;
        font-size: 20pt;
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 15pt;
    }
    .paper-authors {
        text-align: center;
        font-size: 12pt;
        color: #6b7280;
        margin-bottom: 10pt;
    }
    .paper-affiliation {
        text-align: center;
        font-size: 10pt;
        color: #9ca3af;
        margin-bottom: 30pt;
    }
    .abstract {
        padding: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        margin-bottom: 30pt;
    }
    .abstract-title {
        font-weight: bold;
        font-size: 12pt;
        margin-bottom: 10pt;
        color: #1e3a8a;
    }
    .abstract-text {
        font-size: 10pt;
        line-height: 1.6;
        text-align: justify;
    }
    .paper-content {
        column-count: 2;
        column-gap: 25pt;
        text-align: justify;
        line-height: 1.7;
        font-size: 10pt;
    }
    .paper-content h2 {
        column-span: all;
        font-size: 14pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 20pt 0 12pt 0;
        border-bottom: 2pt solid #e5e7eb;
        padding-bottom: 5pt;
    }
    .paper-content p {
        margin: 0 0 10pt 0;
    }
</style>
<body>
    <div class="paper">
        <div class="paper-title">
            Multi-Column Layout Optimization in PDF Generation
        </div>
        <div class="paper-authors">
            Jane Smith, PhD and John Doe, MSc
        </div>
        <div class="paper-affiliation">
            Department of Computer Science, University of Technology
        </div>

        <div class="abstract">
            <div class="abstract-title">Abstract</div>
            <div class="abstract-text">
                This paper examines the application of multi-column layouts in
                automated PDF generation systems. We demonstrate how column-based
                designs improve readability and document structure while maintaining
                professional appearance standards. Our findings suggest significant
                benefits in reader comprehension and engagement.
            </div>
        </div>

        <div class="paper-content">
            <h2>1. Introduction</h2>
            <p>Document layout significantly impacts how readers process and
            retain information. Traditional single-column formats, while simple,
            often result in line lengths that exceed optimal reading ranges.</p>
            <p>Multi-column layouts address this challenge by constraining line
            length while maintaining efficient use of page space. This approach
            has been validated through decades of print media experience.</p>

            <h2>2. Methodology</h2>
            <p>We conducted extensive testing across various document types,
            measuring readability metrics and user satisfaction. Our test group
            included 500 participants from diverse backgrounds.</p>
            <p>Documents were generated using automated PDF generation tools
            with varying column configurations. Participants rated each layout
            on multiple criteria including readability and visual appeal.</p>

            <h2>3. Results</h2>
            <p>The data clearly demonstrates preference for two-column layouts
            in documents exceeding 500 words. Three-column layouts performed
            well for brief, scan-able content like newsletters.</p>
        </div>
    </div>
</body>
```

### Example 6: Product catalog with four columns

```html
<style>
    .catalog {
        padding: 30pt;
    }
    .catalog-header {
        text-align: center;
        margin-bottom: 30pt;
    }
    .catalog-header h1 {
        font-size: 28pt;
        color: #1e3a8a;
        margin: 0 0 10pt 0;
    }
    .catalog-subtitle {
        font-size: 14pt;
        color: #6b7280;
    }
    .catalog-grid {
        column-count: 4;
        column-gap: 15pt;
    }
    .product-item {
        break-inside: avoid;
        margin-bottom: 15pt;
        padding: 12pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .product-image {
        width: 100%;
        height: 80pt;
        background-color: #f3f4f6;
        margin-bottom: 8pt;
    }
    .product-name {
        font-weight: bold;
        font-size: 10pt;
        color: #1f2937;
        margin-bottom: 5pt;
    }
    .product-code {
        font-size: 8pt;
        color: #9ca3af;
        margin-bottom: 5pt;
    }
    .product-price {
        font-size: 12pt;
        font-weight: bold;
        color: #16a34a;
    }
</style>
<body>
    <div class="catalog">
        <div class="catalog-header">
            <h1>Product Catalog</h1>
            <div class="catalog-subtitle">Fall 2025 Collection</div>
        </div>

        <div class="catalog-grid">
            <div class="product-item">
                <div class="product-image"></div>
                <div class="product-name">Widget Pro</div>
                <div class="product-code">SKU: WP-001</div>
                <div class="product-price">$29.99</div>
            </div>

            <div class="product-item">
                <div class="product-image"></div>
                <div class="product-name">Gadget Plus</div>
                <div class="product-code">SKU: GP-002</div>
                <div class="product-price">$39.99</div>
            </div>

            <div class="product-item">
                <div class="product-image"></div>
                <div class="product-name">Tool Master</div>
                <div class="product-code">SKU: TM-003</div>
                <div class="product-price">$49.99</div>
            </div>

            <div class="product-item">
                <div class="product-image"></div>
                <div class="product-name">Device Elite</div>
                <div class="product-code">SKU: DE-004</div>
                <div class="product-price">$59.99</div>
            </div>

            <div class="product-item">
                <div class="product-image"></div>
                <div class="product-name">System Advanced</div>
                <div class="product-code">SKU: SA-005</div>
                <div class="product-price">$69.99</div>
            </div>

            <div class="product-item">
                <div class="product-image"></div>
                <div class="product-name">Premium Kit</div>
                <div class="product-code">SKU: PK-006</div>
                <div class="product-price">$79.99</div>
            </div>

            <div class="product-item">
                <div class="product-image"></div>
                <div class="product-name">Deluxe Set</div>
                <div class="product-code">SKU: DS-007</div>
                <div class="product-price">$89.99</div>
            </div>

            <div class="product-item">
                <div class="product-image"></div>
                <div class="product-name">Professional Pack</div>
                <div class="product-code">SKU: PP-008</div>
                <div class="product-price">$99.99</div>
            </div>
        </div>
    </div>
</body>
```

### Example 7: FAQ document with two columns

```html
<style>
    .faq-document {
        width: 600pt;
        margin: 0 auto;
        padding: 40pt;
    }
    .faq-title {
        text-align: center;
        font-size: 32pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 30pt;
    }
    .faq-content {
        column-count: 2;
        column-gap: 30pt;
    }
    .faq-item {
        break-inside: avoid;
        margin-bottom: 20pt;
    }
    .faq-question {
        font-weight: bold;
        color: #1e3a8a;
        font-size: 12pt;
        margin-bottom: 8pt;
    }
    .faq-answer {
        font-size: 10pt;
        line-height: 1.6;
        color: #4b5563;
        text-align: justify;
    }
</style>
<body>
    <div class="faq-document">
        <h1 class="faq-title">Frequently Asked Questions</h1>

        <div class="faq-content">
            <div class="faq-item">
                <div class="faq-question">Q: How do I create multi-column layouts?</div>
                <div class="faq-answer">
                    Use the column-count property to specify the number of columns.
                    Content will automatically flow from one column to the next.
                </div>
            </div>

            <div class="faq-item">
                <div class="faq-question">Q: What is the ideal column count?</div>
                <div class="faq-answer">
                    For most documents, 2-3 columns work best. Use 2 for detailed
                    articles, 3 for newsletters, and 4+ for catalogs or lists.
                </div>
            </div>

            <div class="faq-item">
                <div class="faq-question">Q: How do I control spacing between columns?</div>
                <div class="faq-answer">
                    Use the column-gap property to set the space between columns.
                    Typical values range from 15pt to 30pt depending on layout.
                </div>
            </div>

            <div class="faq-item">
                <div class="faq-question">Q: Can elements span multiple columns?</div>
                <div class="faq-answer">
                    Yes, use column-span: all to make an element span across all
                    columns. This is useful for headings and section dividers.
                </div>
            </div>

            <div class="faq-item">
                <div class="faq-question">Q: Do columns work in PDF generation?</div>
                <div class="faq-answer">
                    Absolutely! Modern PDF generation libraries fully support
                    multi-column layouts with automatic content flow and balancing.
                </div>
            </div>

            <div class="faq-item">
                <div class="faq-question">Q: How are columns balanced?</div>
                <div class="faq-answer">
                    By default, content distributes evenly across columns, ensuring
                    balanced column heights when possible.
                </div>
            </div>

            <div class="faq-item">
                <div class="faq-question">Q: What about images in columns?</div>
                <div class="faq-answer">
                    Images flow within columns naturally. Use break-inside: avoid
                    to prevent images from splitting across columns.
                </div>
            </div>

            <div class="faq-item">
                <div class="faq-question">Q: Can I change column count mid-document?</div>
                <div class="faq-answer">
                    Yes, different sections can have different column counts by
                    applying the property to specific containers.
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 8: Restaurant menu with three columns

```html
<style>
    .menu {
        width: 650pt;
        margin: 0 auto;
        padding: 40pt;
        background-color: #fffef7;
        border: 5pt solid #1e3a8a;
    }
    .menu-header {
        text-align: center;
        margin-bottom: 30pt;
    }
    .menu-title {
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 10pt 0;
    }
    .menu-subtitle {
        font-size: 14pt;
        color: #6b7280;
        font-style: italic;
    }
    .menu-section {
        margin-bottom: 30pt;
    }
    .section-title {
        font-size: 20pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 15pt;
        text-align: center;
        border-bottom: 2pt solid #1e3a8a;
        padding-bottom: 10pt;
    }
    .menu-items {
        column-count: 3;
        column-gap: 20pt;
    }
    .menu-item {
        break-inside: avoid;
        margin-bottom: 15pt;
    }
    .item-name {
        font-weight: bold;
        color: #1f2937;
        font-size: 11pt;
        margin-bottom: 3pt;
    }
    .item-description {
        font-size: 9pt;
        color: #6b7280;
        line-height: 1.4;
        margin-bottom: 3pt;
    }
    .item-price {
        font-weight: bold;
        color: #16a34a;
        font-size: 10pt;
    }
</style>
<body>
    <div class="menu">
        <div class="menu-header">
            <div class="menu-title">La Cuisine</div>
            <div class="menu-subtitle">Fine Dining Experience</div>
        </div>

        <div class="menu-section">
            <div class="section-title">Appetizers</div>
            <div class="menu-items">
                <div class="menu-item">
                    <div class="item-name">French Onion Soup</div>
                    <div class="item-description">Classic soup with Gruyere</div>
                    <div class="item-price">$12.00</div>
                </div>

                <div class="menu-item">
                    <div class="item-name">Caesar Salad</div>
                    <div class="item-description">Romaine, parmesan, croutons</div>
                    <div class="item-price">$10.00</div>
                </div>

                <div class="menu-item">
                    <div class="item-name">Bruschetta</div>
                    <div class="item-description">Tomatoes, basil, olive oil</div>
                    <div class="item-price">$9.00</div>
                </div>

                <div class="menu-item">
                    <div class="item-name">Stuffed Mushrooms</div>
                    <div class="item-description">Herb cheese filling</div>
                    <div class="item-price">$11.00</div>
                </div>

                <div class="menu-item">
                    <div class="item-name">Calamari Fritti</div>
                    <div class="item-description">Crispy fried squid</div>
                    <div class="item-price">$14.00</div>
                </div>

                <div class="menu-item">
                    <div class="item-name">Caprese Salad</div>
                    <div class="item-description">Tomato, mozzarella, basil</div>
                    <div class="item-price">$13.00</div>
                </div>
            </div>
        </div>

        <div class="menu-section">
            <div class="section-title">Main Courses</div>
            <div class="menu-items">
                <div class="menu-item">
                    <div class="item-name">Grilled Salmon</div>
                    <div class="item-description">Lemon butter sauce</div>
                    <div class="item-price">$28.00</div>
                </div>

                <div class="menu-item">
                    <div class="item-name">Ribeye Steak</div>
                    <div class="item-description">12oz aged beef</div>
                    <div class="item-price">$38.00</div>
                </div>

                <div class="menu-item">
                    <div class="item-name">Chicken Piccata</div>
                    <div class="item-description">Lemon caper sauce</div>
                    <div class="item-price">$24.00</div>
                </div>

                <div class="menu-item">
                    <div class="item-name">Pasta Primavera</div>
                    <div class="item-description">Fresh vegetables</div>
                    <div class="item-price">$22.00</div>
                </div>

                <div class="menu-item">
                    <div class="item-name">Lamb Chops</div>
                    <div class="item-description">Rosemary garlic</div>
                    <div class="item-price">$36.00</div>
                </div>

                <div class="menu-item">
                    <div class="item-name">Lobster Tail</div>
                    <div class="item-description">Drawn butter</div>
                    <div class="item-price">$42.00</div>
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 9: Directory listing with four columns

```html
<style>
    .directory {
        padding: 35pt;
    }
    .directory-header {
        text-align: center;
        background-color: #1f2937;
        color: white;
        padding: 25pt;
        margin-bottom: 30pt;
    }
    .directory-header h1 {
        margin: 0;
        font-size: 28pt;
    }
    .directory-list {
        column-count: 4;
        column-gap: 15pt;
    }
    .directory-entry {
        break-inside: avoid;
        margin-bottom: 12pt;
        padding: 10pt;
        background-color: #f9fafb;
        border-left: 3pt solid #2563eb;
    }
    .entry-name {
        font-weight: bold;
        color: #1e3a8a;
        font-size: 10pt;
        margin-bottom: 5pt;
    }
    .entry-details {
        font-size: 8pt;
        color: #6b7280;
        line-height: 1.4;
    }
</style>
<body>
    <div class="directory">
        <div class="directory-header">
            <h1>Company Directory</h1>
        </div>

        <div class="directory-list">
            <div class="directory-entry">
                <div class="entry-name">Anderson, Sarah</div>
                <div class="entry-details">
                    Sales Manager<br/>
                    Ext: 2101<br/>
                    sanderson@company.com
                </div>
            </div>

            <div class="directory-entry">
                <div class="entry-name">Brown, Michael</div>
                <div class="entry-details">
                    IT Director<br/>
                    Ext: 2205<br/>
                    mbrown@company.com
                </div>
            </div>

            <div class="directory-entry">
                <div class="entry-name">Chen, Lisa</div>
                <div class="entry-details">
                    Marketing Lead<br/>
                    Ext: 2308<br/>
                    lchen@company.com
                </div>
            </div>

            <div class="directory-entry">
                <div class="entry-name">Davis, Robert</div>
                <div class="entry-details">
                    Operations<br/>
                    Ext: 2412<br/>
                    rdavis@company.com
                </div>
            </div>

            <div class="directory-entry">
                <div class="entry-name">Evans, Jennifer</div>
                <div class="entry-details">
                    HR Manager<br/>
                    Ext: 2515<br/>
                    jevans@company.com
                </div>
            </div>

            <div class="directory-entry">
                <div class="entry-name">Foster, David</div>
                <div class="entry-details">
                    Finance Director<br/>
                    Ext: 2620<br/>
                    dfoster@company.com
                </div>
            </div>

            <div class="directory-entry">
                <div class="entry-name">Garcia, Maria</div>
                <div class="entry-details">
                    Customer Success<br/>
                    Ext: 2724<br/>
                    mgarcia@company.com
                </div>
            </div>

            <div class="directory-entry">
                <div class="entry-name">Harris, James</div>
                <div class="entry-details">
                    Product Manager<br/>
                    Ext: 2828<br/>
                    jharris@company.com
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 10: Terms and conditions with two columns

```html
<style>
    .terms-document {
        width: 600pt;
        margin: 0 auto;
        padding: 40pt;
    }
    .terms-header {
        text-align: center;
        margin-bottom: 30pt;
    }
    .terms-title {
        font-size: 24pt;
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 10pt;
    }
    .terms-date {
        font-size: 11pt;
        color: #6b7280;
    }
    .terms-content {
        column-count: 2;
        column-gap: 25pt;
        text-align: justify;
        line-height: 1.6;
        font-size: 9pt;
    }
    .terms-content h2 {
        column-span: all;
        font-size: 14pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 20pt 0 10pt 0;
        border-bottom: 2pt solid #e5e7eb;
        padding-bottom: 5pt;
    }
    .terms-content p {
        margin: 0 0 8pt 0;
    }
    .terms-content ol {
        margin: 0 0 10pt 0;
        padding-left: 15pt;
    }
</style>
<body>
    <div class="terms-document">
        <div class="terms-header">
            <div class="terms-title">Terms and Conditions</div>
            <div class="terms-date">Effective Date: October 14, 2025</div>
        </div>

        <div class="terms-content">
            <h2>1. Acceptance of Terms</h2>
            <p>By accessing and using this service, you accept and agree to be
            bound by the terms and provisions of this agreement. If you do not
            agree to these terms, please do not use this service.</p>

            <h2>2. Use License</h2>
            <p>Permission is granted to temporarily use the service for personal,
            non-commercial transitory viewing only. This is the grant of a license,
            not a transfer of title, and under this license you may not:</p>
            <ol>
                <li>Modify or copy the materials</li>
                <li>Use the materials for commercial purpose</li>
                <li>Attempt to decompile or reverse engineer any software</li>
                <li>Remove any copyright or proprietary notations</li>
            </ol>

            <h2>3. Disclaimer</h2>
            <p>The materials on this service are provided on an 'as is' basis.
            We make no warranties, expressed or implied, and hereby disclaim and
            negate all other warranties including, without limitation, implied
            warranties or conditions of merchantability, fitness for a particular
            purpose, or non-infringement of intellectual property.</p>

            <h2>4. Limitations</h2>
            <p>In no event shall the company or its suppliers be liable for any
            damages including, without limitation, damages for loss of data or
            profit, or due to business interruption arising out of the use or
            inability to use the service.</p>
        </div>
    </div>
</body>
```

---

## See Also

- [column-gap](/reference/cssproperties/css_prop_column-gap) - Set spacing between columns
- [column-width](/reference/cssproperties/css_prop_column-width) - Set ideal column width
- [column-span](/reference/cssproperties/css_prop_column-span) - Make elements span columns
- [width](/reference/cssproperties/css_prop_width) - Set element width
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
