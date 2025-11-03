---
layout: default
title: column-gap
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# column-gap : Column Gap Property

The `column-gap` property specifies the space between columns in a multi-column layout. This property is essential for creating visually balanced and readable multi-column documents, controlling the whitespace that separates column content in newsletters, magazines, brochures, and professional reports.

## Usage

```css
selector {
    column-gap: value;
}
```

The column-gap property sets the width of the gap (gutter) between columns. Proper spacing improves readability and creates visual separation between column content.

---

## Supported Values

### Length Units
- Points: `10pt`, `20pt`, `30pt`
- Pixels: `10px`, `20px`, `30px`
- Inches: `0.25in`, `0.5in`, `1in`
- Centimeters: `1cm`, `2cm`, `3cm`
- Millimeters: `10mm`, `20mm`, `30mm`
- Ems: `1em`, `2em`, `3em`

### normal (default)
Uses the browser/renderer's default column gap, typically `1em` (approximately 16pt). This provides reasonable spacing for most layouts.

---

## Supported Elements

The `column-gap` property can be applied to:
- Block containers with column-count or column-width set
- Multi-column elements (`<div>`, `<section>`, `<article>`)
- Any element using multi-column layout
- Newsletter and magazine layouts
- Content areas with column flow

---

## Notes

- Column-gap only applies to elements with multi-column layout enabled
- Must be used with `column-count` or `column-width` to have effect
- Typical values range from 15pt to 40pt depending on column width
- Wider columns can accommodate larger gaps
- Narrower columns need smaller gaps to maintain proportions
- The gap appears between columns but not on outer edges
- In PDF generation, consistent gaps create professional appearance
- Consider page width when choosing gap size
- Too narrow gaps can make columns appear cluttered
- Too wide gaps waste space and can disrupt reading flow
- Standard newspaper gap is approximately 1 pica (12pt)
- Magazine layouts typically use 20pt-30pt gaps
- Gap width affects overall document texture and readability

---

## Data Binding

The column-gap property integrates with data binding to create adaptive spacing between columns based on layout preferences, page dimensions, and design requirements. This enables responsive magazine layouts, configurable newsletters, and flexible document designs.

### Example 1: Dynamic column gap based on page size

```html
<style>
    .article {
        column-count: 2;
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
    <!-- Responsive column gap based on page width -->
    <div class="article"
         style="column-gap: {{pageSize === 'A4' ? '20pt' : (pageSize === 'Letter' ? '25pt' : '30pt')}}">
        <h2>{{articleTitle}}</h2>
        <p>{{articleContent}}</p>
    </div>

    <!-- Adaptive gap for different output formats -->
    <div class="article"
         style="column-count: 3;
                column-gap: {{outputFormat === 'print' ? '18pt' : (outputFormat === 'screen' ? '25pt' : '22pt')}}">
        <h2>{{title}}</h2>
        <p>{{content}}</p>
    </div>
</body>
```

### Example 2: User-configurable spacing preferences

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
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>{{newsletterTitle}}</h1>
            <div>{{publishDate}}</div>
        </div>

        <!-- Column spacing based on user preferences -->
        <div class="newsletter-content"
             style="column-count: {{preferences.columns}};
                    column-gap: {{preferences.spacing === 'tight' ? '15pt' :
                                  (preferences.spacing === 'normal' ? '22pt' :
                                  (preferences.spacing === 'loose' ? '30pt' : '22pt'))}}">
            {{#each articles}}
            <h3>{{this.title}}</h3>
            <p>{{this.content}}</p>
            {{/each}}
        </div>
    </div>
</body>
```

### Example 3: Adaptive gap for magazine layouts

```html
<style>
    .magazine-page {
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
    .feature-article {
        text-align: justify;
        line-height: 1.8;
        font-size: 11pt;
    }
    .feature-article h1 {
        font-size: 28pt;
        color: #1e3a8a;
        margin-bottom: 15pt;
    }
</style>
<body>
    <div class="magazine-page">
        <div class="magazine-header">
            <h1>{{magazineTitle}}</h1>
            <div>{{issueInfo}}</div>
        </div>

        <!-- Dynamic gap based on layout configuration -->
        <div class="feature-article"
             style="column-count: {{layout.columns}};
                    column-gap: {{
                        layout.style === 'premium' ? '35pt' :
                        (layout.style === 'standard' ? '25pt' :
                        (layout.style === 'compact' ? '18pt' : '25pt'))
                    }}">
            <h1>{{articleTitle}}</h1>
            <p>{{articleBody}}</p>
        </div>

        <!-- Section-specific column gaps -->
        {{#each sections}}
        <div style="column-count: 2;
                    column-gap: {{this.customGap || config.defaultGap}}pt;
                    margin-top: 30pt;">
            <h2 style="column-span: all; font-size: 20pt; color: #1e3a8a; margin-bottom: 15pt;">
                {{this.sectionTitle}}
            </h2>
            <p>{{this.sectionContent}}</p>
        </div>
        {{/each}}
    </div>
</body>
```

---

## Examples

### Example 1: Basic column gap in two-column layout

```html
<style>
    .article {
        column-count: 2;
        column-gap: 25pt;
        padding: 30pt;
        text-align: justify;
        line-height: 1.7;
    }
    .article h2 {
        column-span: all;
        color: #1e3a8a;
        margin: 0 0 15pt 0;
        font-size: 24pt;
    }
    .article p {
        margin: 0 0 12pt 0;
    }
</style>
<body>
    <div class="article">
        <h2>Understanding Column Spacing</h2>
        <p>Column gap is crucial for creating readable multi-column layouts.
        The 25pt gap between these columns provides clear visual separation
        without wasting valuable page space.</p>
        <p>Professional documents require careful attention to spacing. Too
        little gap makes columns run together, while too much gap fragments
        the reading experience and reduces content density.</p>
        <p>This example demonstrates optimal spacing for a two-column article
        layout, balancing readability with efficient space utilization.</p>
    </div>
</body>
```

### Example 2: Comparing different gap sizes

```html
<style>
    .demo-section {
        margin-bottom: 30pt;
        padding: 20pt;
        border: 2pt solid #e5e7eb;
        background-color: white;
    }
    .gap-label {
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
        font-size: 14pt;
    }
    .gap-small {
        column-count: 2;
        column-gap: 10pt;
        text-align: justify;
        font-size: 10pt;
        line-height: 1.5;
    }
    .gap-medium {
        column-count: 2;
        column-gap: 25pt;
        text-align: justify;
        font-size: 10pt;
        line-height: 1.5;
    }
    .gap-large {
        column-count: 2;
        column-gap: 40pt;
        text-align: justify;
        font-size: 10pt;
        line-height: 1.5;
    }
</style>
<body>
    <div class="demo-section">
        <div class="gap-label">Small Gap (10pt)</div>
        <div class="gap-small">
            <p style="margin: 0;">This layout uses a 10pt column gap. The narrow
            spacing creates a compact feel but may make columns appear crowded.
            Best used when space is limited or for dense reference materials.</p>
        </div>
    </div>

    <div class="demo-section">
        <div class="gap-label">Medium Gap (25pt)</div>
        <div class="gap-medium">
            <p style="margin: 0;">This layout uses a 25pt column gap. The moderate
            spacing provides clear separation while maintaining good content density.
            Ideal for most newsletter and magazine layouts.</p>
        </div>
    </div>

    <div class="demo-section">
        <div class="gap-label">Large Gap (40pt)</div>
        <div class="gap-large">
            <p style="margin: 0;">This layout uses a 40pt column gap. The wide
            spacing creates an open, airy feel perfect for premium publications
            and marketing materials where space is abundant.</p>
        </div>
    </div>
</body>
```

### Example 3: Newsletter with three columns and spacing

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
    .newsletter-title {
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 10pt 0;
    }
    .newsletter-date {
        font-size: 12pt;
        color: #6b7280;
    }
    .newsletter-content {
        column-count: 3;
        column-gap: 20pt;
        text-align: justify;
        line-height: 1.6;
        font-size: 10pt;
    }
    .newsletter-content h3 {
        color: #1e3a8a;
        font-size: 13pt;
        margin: 0 0 8pt 0;
    }
    .newsletter-content p {
        margin: 0 0 10pt 0;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <div class="newsletter-title">MONTHLY DIGEST</div>
            <div class="newsletter-date">October 2025 | Volume 5, Issue 10</div>
        </div>

        <div class="newsletter-content">
            <h3>Technology Trends</h3>
            <p>The landscape of document generation continues to evolve with
            new capabilities emerging regularly. Organizations are discovering
            the power of automated PDF creation for business communications.</p>

            <h3>Industry Insights</h3>
            <p>Market research indicates strong demand for professional document
            solutions. Companies invest in tools that enable consistent, branded
            output across all business documents.</p>

            <h3>Best Practices</h3>
            <p>Successful implementations share common characteristics: attention
            to layout details, proper spacing, and thoughtful typography. The 20pt
            column gap in this newsletter provides optimal separation.</p>

            <h3>Success Stories</h3>
            <p>Leading organizations report significant time savings and improved
            document quality after implementing automated generation systems with
            professional multi-column layouts.</p>

            <h3>Looking Ahead</h3>
            <p>Future developments promise even more sophisticated layout options,
            making it easier to create publication-quality documents from any
            application or data source.</p>
        </div>
    </div>
</body>
```

### Example 4: Magazine article with optimal spacing

```html
<style>
    .magazine-article {
        width: 650pt;
        margin: 0 auto;
        padding: 50pt;
        background-color: white;
    }
    .article-header {
        margin-bottom: 30pt;
    }
    .article-headline {
        font-size: 42pt;
        font-weight: bold;
        color: #1e3a8a;
        line-height: 1.1;
        margin: 0 0 15pt 0;
    }
    .article-subhead {
        font-size: 16pt;
        color: #6b7280;
        line-height: 1.4;
        margin-bottom: 10pt;
    }
    .article-byline {
        font-size: 11pt;
        color: #9ca3af;
        font-style: italic;
    }
    .article-body {
        column-count: 2;
        column-gap: 30pt;
        text-align: justify;
        line-height: 1.8;
        font-size: 11pt;
    }
    .article-body p {
        margin: 0 0 14pt 0;
    }
    .article-body p:first-child:first-letter {
        font-size: 48pt;
        font-weight: bold;
        float: left;
        line-height: 40pt;
        margin: 0 8pt 0 0;
        color: #1e3a8a;
    }
</style>
<body>
    <div class="magazine-article">
        <div class="article-header">
            <h1 class="article-headline">The Art of Document Design</h1>
            <div class="article-subhead">
                How proper spacing and layout transform ordinary documents into
                professional publications
            </div>
            <div class="article-byline">By Alexandra Chen | Photography by Michael Torres</div>
        </div>

        <div class="article-body">
            <p>Document design is both science and art. The technical aspects—
            typography, spacing, alignment—combine with aesthetic judgment to
            create layouts that are simultaneously functional and beautiful.</p>

            <p>Consider the humble column gap. This space between text columns
            serves multiple purposes: it provides visual breathing room, prevents
            the eye from jumping between columns, and creates rhythm across the
            page. In this article, a 30pt gap gives the layout an open, premium
            feel appropriate for magazine-style content.</p>

            <p>Professional designers understand that every element contributes
            to the reader's experience. Column spacing, line height, font size—
            these seemingly minor details combine to either facilitate or hinder
            comprehension. Getting them right requires both knowledge and practice.</p>

            <p>The rise of automated document generation has democratized access
            to professional layouts. Tools that once required specialized design
            software are now available to developers and content creators, enabling
            anyone to produce publication-quality documents.</p>
        </div>
    </div>
</body>
```

### Example 5: Brochure with balanced column spacing

```html
<style>
    .brochure {
        padding: 35pt;
    }
    .brochure-banner {
        background-color: #1e3a8a;
        color: white;
        text-align: center;
        padding: 30pt;
        margin-bottom: 35pt;
    }
    .brochure-banner h1 {
        margin: 0 0 10pt 0;
        font-size: 36pt;
    }
    .brochure-tagline {
        font-size: 16pt;
        margin: 0;
    }
    .intro-text {
        text-align: center;
        font-size: 14pt;
        line-height: 1.8;
        margin-bottom: 30pt;
        color: #1f2937;
    }
    .features {
        column-count: 3;
        column-gap: 25pt;
    }
    .feature-box {
        break-inside: avoid;
        margin-bottom: 20pt;
        padding: 20pt;
        background-color: #f9fafb;
        border-top: 4pt solid #2563eb;
    }
    .feature-icon {
        width: 50pt;
        height: 50pt;
        background-color: #2563eb;
        color: white;
        border-radius: 25pt;
        text-align: center;
        line-height: 50pt;
        font-size: 24pt;
        font-weight: bold;
        margin-bottom: 12pt;
    }
    .feature-title {
        font-weight: bold;
        color: #1e3a8a;
        font-size: 14pt;
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
        <div class="brochure-banner">
            <h1>Professional Services</h1>
            <div class="brochure-tagline">Excellence in Every Detail</div>
        </div>

        <div class="intro-text">
            <p style="margin: 0;">We deliver comprehensive solutions tailored to your
            unique business needs with unmatched expertise and dedication.</p>
        </div>

        <div class="features">
            <div class="feature-box">
                <div class="feature-icon">1</div>
                <div class="feature-title">Strategic Consulting</div>
                <div class="feature-text">
                    Expert guidance to help you navigate complex challenges and
                    achieve your business objectives with proven methodologies.
                </div>
            </div>

            <div class="feature-box">
                <div class="feature-icon">2</div>
                <div class="feature-title">Custom Development</div>
                <div class="feature-text">
                    Tailored solutions built to your exact specifications using
                    cutting-edge technologies and industry best practices.
                </div>
            </div>

            <div class="feature-box">
                <div class="feature-icon">3</div>
                <div class="feature-title">24/7 Support</div>
                <div class="feature-text">
                    Round-the-clock assistance ensuring your systems run smoothly
                    with minimal downtime and maximum efficiency.
                </div>
            </div>

            <div class="feature-box">
                <div class="feature-icon">4</div>
                <div class="feature-title">Training Programs</div>
                <div class="feature-text">
                    Comprehensive training to empower your team with the skills
                    and knowledge needed for success.
                </div>
            </div>

            <div class="feature-box">
                <div class="feature-icon">5</div>
                <div class="feature-title">Cloud Integration</div>
                <div class="feature-text">
                    Seamless integration with cloud platforms ensuring scalability,
                    reliability, and accessibility.
                </div>
            </div>

            <div class="feature-box">
                <div class="feature-icon">6</div>
                <div class="feature-title">Security Audits</div>
                <div class="feature-text">
                    Thorough security assessments to protect your valuable data
                    and systems from emerging threats.
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 6: Product catalog with tight spacing

```html
<style>
    .catalog {
        padding: 30pt;
        background-color: #f9fafb;
    }
    .catalog-header {
        text-align: center;
        background-color: white;
        padding: 25pt;
        margin-bottom: 25pt;
        border: 3pt solid #e5e7eb;
    }
    .catalog-title {
        font-size: 32pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 8pt 0;
    }
    .catalog-subtitle {
        font-size: 14pt;
        color: #6b7280;
        margin: 0;
    }
    .product-grid {
        column-count: 4;
        column-gap: 15pt;
    }
    .product-card {
        break-inside: avoid;
        background-color: white;
        border: 2pt solid #e5e7eb;
        padding: 12pt;
        margin-bottom: 15pt;
    }
    .product-image {
        width: 100%;
        height: 90pt;
        background-color: #f3f4f6;
        margin-bottom: 10pt;
    }
    .product-name {
        font-weight: bold;
        font-size: 10pt;
        color: #1f2937;
        margin-bottom: 5pt;
    }
    .product-sku {
        font-size: 8pt;
        color: #9ca3af;
        margin-bottom: 8pt;
    }
    .product-price {
        font-size: 13pt;
        font-weight: bold;
        color: #16a34a;
    }
</style>
<body>
    <div class="catalog">
        <div class="catalog-header">
            <div class="catalog-title">Product Catalog</div>
            <div class="catalog-subtitle">Q4 2025 Collection</div>
        </div>

        <div class="product-grid">
            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Premium Widget</div>
                <div class="product-sku">SKU: PW-2025-001</div>
                <div class="product-price">$29.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Deluxe Gadget</div>
                <div class="product-sku">SKU: DG-2025-002</div>
                <div class="product-price">$39.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Pro Tool</div>
                <div class="product-sku">SKU: PT-2025-003</div>
                <div class="product-price">$49.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Elite Device</div>
                <div class="product-sku">SKU: ED-2025-004</div>
                <div class="product-price">$59.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Master System</div>
                <div class="product-sku">SKU: MS-2025-005</div>
                <div class="product-price">$69.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Advanced Kit</div>
                <div class="product-sku">SKU: AK-2025-006</div>
                <div class="product-price">$79.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Professional Pack</div>
                <div class="product-sku">SKU: PP-2025-007</div>
                <div class="product-price">$89.99</div>
            </div>

            <div class="product-card">
                <div class="product-image"></div>
                <div class="product-name">Ultimate Bundle</div>
                <div class="product-sku">SKU: UB-2025-008</div>
                <div class="product-price">$99.99</div>
            </div>
        </div>
    </div>
</body>
```

### Example 7: Restaurant menu with generous spacing

```html
<style>
    .menu {
        width: 700pt;
        margin: 0 auto;
        padding: 45pt;
        background-color: #fffef7;
        border: 6pt solid #1e3a8a;
    }
    .menu-header {
        text-align: center;
        margin-bottom: 35pt;
    }
    .restaurant-name {
        font-size: 40pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 10pt 0;
    }
    .restaurant-tagline {
        font-size: 16pt;
        color: #6b7280;
        font-style: italic;
        margin: 0;
    }
    .menu-section {
        margin-bottom: 35pt;
    }
    .section-header {
        font-size: 22pt;
        font-weight: bold;
        color: #1e3a8a;
        text-align: center;
        margin-bottom: 20pt;
        padding-bottom: 12pt;
        border-bottom: 2pt solid #1e3a8a;
    }
    .menu-items {
        column-count: 2;
        column-gap: 35pt;
    }
    .menu-item {
        break-inside: avoid;
        margin-bottom: 18pt;
    }
    .item-header {
        display: table;
        width: 100%;
        margin-bottom: 5pt;
    }
    .item-name {
        display: table-cell;
        font-weight: bold;
        font-size: 12pt;
        color: #1f2937;
    }
    .item-price {
        display: table-cell;
        text-align: right;
        font-weight: bold;
        font-size: 12pt;
        color: #16a34a;
    }
    .item-description {
        font-size: 10pt;
        color: #6b7280;
        line-height: 1.5;
        font-style: italic;
    }
</style>
<body>
    <div class="menu">
        <div class="menu-header">
            <div class="restaurant-name">Bistro Elegante</div>
            <div class="restaurant-tagline">Contemporary French Cuisine</div>
        </div>

        <div class="menu-section">
            <div class="section-header">Starters</div>
            <div class="menu-items">
                <div class="menu-item">
                    <div class="item-header">
                        <div class="item-name">French Onion Soup</div>
                        <div class="item-price">$14</div>
                    </div>
                    <div class="item-description">
                        Classic soup with caramelized onions and Gruyere crostini
                    </div>
                </div>

                <div class="menu-item">
                    <div class="item-header">
                        <div class="item-name">Escargots de Bourgogne</div>
                        <div class="item-price">$18</div>
                    </div>
                    <div class="item-description">
                        Burgundy snails with garlic herb butter and toast points
                    </div>
                </div>

                <div class="menu-item">
                    <div class="item-header">
                        <div class="item-name">Salade Niçoise</div>
                        <div class="item-price">$16</div>
                    </div>
                    <div class="item-description">
                        Fresh tuna, green beans, olives, and anchovy vinaigrette
                    </div>
                </div>

                <div class="menu-item">
                    <div class="item-header">
                        <div class="item-name">Foie Gras Terrine</div>
                        <div class="item-price">$24</div>
                    </div>
                    <div class="item-description">
                        House-made terrine with fig compote and brioche toast
                    </div>
                </div>
            </div>
        </div>

        <div class="menu-section">
            <div class="section-header">Main Courses</div>
            <div class="menu-items">
                <div class="menu-item">
                    <div class="item-header">
                        <div class="item-name">Coq au Vin</div>
                        <div class="item-price">$32</div>
                    </div>
                    <div class="item-description">
                        Braised chicken in red wine with mushrooms and pearl onions
                    </div>
                </div>

                <div class="menu-item">
                    <div class="item-header">
                        <div class="item-name">Boeuf Bourguignon</div>
                        <div class="item-price">$38</div>
                    </div>
                    <div class="item-description">
                        Slow-cooked beef in Burgundy wine with root vegetables
                    </div>
                </div>

                <div class="menu-item">
                    <div class="item-header">
                        <div class="item-name">Dover Sole Meunière</div>
                        <div class="item-price">$42</div>
                    </div>
                    <div class="item-description">
                        Whole Dover sole with brown butter and lemon
                    </div>
                </div>

                <div class="menu-item">
                    <div class="item-header">
                        <div class="item-name">Rack of Lamb</div>
                        <div class="item-price">$44</div>
                    </div>
                    <div class="item-description">
                        Herb-crusted lamb with ratatouille and rosemary jus
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 8: Report with professional column spacing

```html
<style>
    .report {
        width: 600pt;
        margin: 0 auto;
        padding: 40pt;
    }
    .report-header {
        margin-bottom: 30pt;
        padding-bottom: 20pt;
        border-bottom: 3pt solid #1e3a8a;
    }
    .report-title {
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 0 0 10pt 0;
    }
    .report-subtitle {
        font-size: 14pt;
        color: #6b7280;
        margin: 0;
    }
    .executive-summary {
        background-color: #f3f4f6;
        padding: 20pt;
        margin-bottom: 30pt;
        border-left: 5pt solid #2563eb;
    }
    .summary-title {
        font-weight: bold;
        font-size: 16pt;
        color: #1e3a8a;
        margin: 0 0 12pt 0;
    }
    .summary-text {
        font-size: 11pt;
        line-height: 1.7;
        color: #1f2937;
        margin: 0;
    }
    .report-body {
        column-count: 2;
        column-gap: 28pt;
        text-align: justify;
        line-height: 1.7;
        font-size: 10pt;
    }
    .report-body h2 {
        column-span: all;
        font-size: 18pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 25pt 0 15pt 0;
        border-bottom: 2pt solid #e5e7eb;
        padding-bottom: 8pt;
    }
    .report-body p {
        margin: 0 0 12pt 0;
    }
</style>
<body>
    <div class="report">
        <div class="report-header">
            <div class="report-title">Annual Performance Report</div>
            <div class="report-subtitle">Fiscal Year 2025 | Q1-Q4 Analysis</div>
        </div>

        <div class="executive-summary">
            <div class="summary-title">Executive Summary</div>
            <p class="summary-text">
                This report presents comprehensive analysis of organizational
                performance across all key metrics for fiscal year 2025. Results
                indicate strong growth with revenue increasing 24% year-over-year
                while maintaining operational efficiency and expanding market share.
            </p>
        </div>

        <div class="report-body">
            <h2>Financial Performance</h2>
            <p>Revenue growth exceeded projections by 8%, driven primarily by
            expansion into new markets and successful product launches. Gross
            margins improved from 42% to 46%, reflecting operational efficiencies
            and favorable market conditions.</p>
            <p>Operating expenses remained well-controlled at 28% of revenue,
            down from 31% in the prior year. This improvement demonstrates
            effective cost management while maintaining investment in growth
            initiatives and research and development.</p>

            <h2>Market Analysis</h2>
            <p>Market conditions throughout the fiscal year remained generally
            favorable despite periodic volatility. Competitive landscape analysis
            shows strengthening position relative to key competitors across all
            primary market segments.</p>
            <p>Customer acquisition costs decreased by 15% while customer lifetime
            value increased by 23%, indicating improved marketing effectiveness
            and product-market fit. The 28pt column gap in this report provides
            optimal reading experience for detailed analysis.</p>
        </div>
    </div>
</body>
```

### Example 9: Directory with varied spacing

```html
<style>
    .directory {
        padding: 35pt;
    }
    .directory-banner {
        background-color: #1f2937;
        color: white;
        text-align: center;
        padding: 30pt;
        margin-bottom: 30pt;
    }
    .directory-banner h1 {
        margin: 0 0 10pt 0;
        font-size: 32pt;
    }
    .directory-banner p {
        margin: 0;
        font-size: 14pt;
    }
    .alphabetical-section {
        margin-bottom: 25pt;
    }
    .letter-header {
        font-size: 24pt;
        font-weight: bold;
        color: #1e3a8a;
        background-color: #f3f4f6;
        padding: 10pt 15pt;
        margin-bottom: 15pt;
        border-left: 5pt solid #2563eb;
    }
    .listings {
        column-count: 3;
        column-gap: 22pt;
    }
    .listing {
        break-inside: avoid;
        margin-bottom: 15pt;
        padding: 12pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
    }
    .listing-name {
        font-weight: bold;
        color: #1e3a8a;
        font-size: 11pt;
        margin-bottom: 5pt;
    }
    .listing-title {
        font-size: 9pt;
        color: #6b7280;
        margin-bottom: 3pt;
    }
    .listing-contact {
        font-size: 8pt;
        color: #9ca3af;
    }
</style>
<body>
    <div class="directory">
        <div class="directory-banner">
            <h1>Employee Directory</h1>
            <p>Complete Staff Listing 2025</p>
        </div>

        <div class="alphabetical-section">
            <div class="letter-header">A</div>
            <div class="listings">
                <div class="listing">
                    <div class="listing-name">Anderson, Sarah</div>
                    <div class="listing-title">Sales Manager</div>
                    <div class="listing-contact">Ext 2101 | sanderson@company.com</div>
                </div>

                <div class="listing">
                    <div class="listing-name">Andrews, Michael</div>
                    <div class="listing-title">Marketing Director</div>
                    <div class="listing-contact">Ext 2102 | mandrews@company.com</div>
                </div>

                <div class="listing">
                    <div class="listing-name">Armstrong, Jennifer</div>
                    <div class="listing-title">HR Specialist</div>
                    <div class="listing-contact">Ext 2103 | jarmstrong@company.com</div>
                </div>
            </div>
        </div>

        <div class="alphabetical-section">
            <div class="letter-header">B</div>
            <div class="listings">
                <div class="listing">
                    <div class="listing-name">Baker, Robert</div>
                    <div class="listing-title">IT Manager</div>
                    <div class="listing-contact">Ext 2201 | rbaker@company.com</div>
                </div>

                <div class="listing">
                    <div class="listing-name">Bennett, Lisa</div>
                    <div class="listing-title">Financial Analyst</div>
                    <div class="listing-contact">Ext 2202 | lbennett@company.com</div>
                </div>

                <div class="listing">
                    <div class="listing-name">Brooks, David</div>
                    <div class="listing-title">Operations Lead</div>
                    <div class="listing-contact">Ext 2203 | dbrooks@company.com</div>
                </div>

                <div class="listing">
                    <div class="listing-name">Brown, Emma</div>
                    <div class="listing-title">Customer Success</div>
                    <div class="listing-contact">Ext 2204 | ebrown@company.com</div>
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 10: Magazine feature with premium spacing

```html
<style>
    .magazine-feature {
        width: 700pt;
        margin: 0 auto;
        padding: 50pt;
        background-color: white;
    }
    .feature-headline {
        font-size: 48pt;
        font-weight: bold;
        color: #1e3a8a;
        line-height: 1.1;
        margin: 0 0 20pt 0;
    }
    .feature-deck {
        font-size: 18pt;
        color: #6b7280;
        line-height: 1.5;
        margin-bottom: 15pt;
    }
    .feature-byline {
        font-size: 12pt;
        color: #9ca3af;
        font-style: italic;
        margin-bottom: 35pt;
        padding-bottom: 20pt;
        border-bottom: 1pt solid #e5e7eb;
    }
    .lead-paragraph {
        font-size: 15pt;
        line-height: 1.9;
        color: #1f2937;
        margin-bottom: 30pt;
        text-align: justify;
    }
    .feature-body {
        column-count: 2;
        column-gap: 35pt;
        text-align: justify;
        line-height: 1.8;
        font-size: 11pt;
    }
    .feature-body p {
        margin: 0 0 15pt 0;
    }
    .pullquote {
        column-span: all;
        text-align: center;
        font-size: 24pt;
        font-weight: bold;
        color: #1e3a8a;
        font-style: italic;
        margin: 25pt 0;
        padding: 20pt 0;
        border-top: 3pt solid #e5e7eb;
        border-bottom: 3pt solid #e5e7eb;
    }
</style>
<body>
    <div class="magazine-feature">
        <h1 class="feature-headline">Design Principles for the Digital Age</h1>
        <div class="feature-deck">
            How traditional publishing principles inform modern document generation
            and create timeless, professional layouts
        </div>
        <div class="feature-byline">
            By Victoria Sterling | Illustrations by Marcus Kim | October 2025
        </div>

        <div class="lead-paragraph">
            The principles that have guided print design for centuries remain
            remarkably relevant in our digital age. Understanding these foundational
            concepts—spacing, proportion, hierarchy—enables creators to produce
            documents that are both beautiful and functional.
        </div>

        <div class="feature-body">
            <p>Column spacing, or the "gutter" in traditional typography, plays
            a crucial role in readability and visual appeal. The 35pt gap in this
            feature creates a premium, open feel appropriate for long-form content
            where reading comfort is paramount.</p>

            <p>Professional designers have long understood that whitespace is not
            wasted space—it's an active design element that guides the eye and
            creates rhythm. Generous gaps between columns prevent visual confusion
            and allow each column to breathe independently.</p>

            <div class="pullquote">
                "Whitespace is not wasted space—it's an active design element"
            </div>

            <p>Modern document generation tools bring these professional capabilities
            to everyone. What once required specialized knowledge and expensive
            software is now accessible to developers and content creators through
            straightforward CSS properties and automated generation systems.</p>

            <p>The future of document design lies in combining traditional principles
            with digital flexibility, creating outputs that honor print's legacy
            while embracing digital possibilities. This synthesis produces documents
            that work beautifully in any medium.</p>
        </div>
    </div>
</body>
```

---

## See Also

- [column-count](/reference/cssproperties/css_prop_column-count) - Set number of columns
- [column-width](/reference/cssproperties/css_prop_column-width) - Set ideal column width
- [column-span](/reference/cssproperties/css_prop_column-span) - Make elements span columns
- [margin](/reference/cssproperties/css_prop_margin) - Set margin spacing
- [padding](/reference/cssproperties/css_prop_padding) - Set padding spacing
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
