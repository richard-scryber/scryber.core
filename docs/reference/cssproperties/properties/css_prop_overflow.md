---
layout: default
title: overflow
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# overflow : Overflow Property

The `overflow` property controls how content is handled when it exceeds the boundaries of its containing element. This is essential for managing text flow, constraining content areas, and creating scrollable regions in PDF documents.

## Usage

```css
selector {
    overflow: value;
}
```

The overflow property determines what happens when content is too large to fit within an element's content box. This property sets both horizontal and vertical overflow behavior simultaneously.

---

## Supported Values

### visible (default)
Content is not clipped and may render outside the element's box. Overflow content is visible and may overlap other elements.

### hidden
Overflow content is clipped and not visible. Content that extends beyond the element's boundaries is cut off without any indication.

### scroll
Overflow content is clipped, but scrolling mechanisms are provided (in interactive contexts). For PDF generation, this typically means content is clipped similar to hidden.

### auto
Overflow content is clipped, and scrolling mechanisms are provided only when necessary. In PDF contexts, this typically behaves like hidden or allows content to flow to additional pages.

---

## Supported Elements

The `overflow` property can be applied to:
- Block elements with explicit dimensions (`<div>`, `<section>`, `<article>`)
- Elements with defined width and height
- Container elements
- Text boxes and content areas
- Table cells (`<td>`, `<th>`)
- Any element where content might exceed boundaries

---

## Notes

- The element must have explicit dimensions (width/height) for overflow to take effect
- `overflow: visible` is the default and allows content to extend beyond boundaries
- `overflow: hidden` is useful for cropping content to exact dimensions
- Hidden overflow prevents content from overlapping adjacent elements
- In PDF generation, overflow handling affects pagination and content flow
- Use overflow to create fixed-size content areas with controlled dimensions
- Overflow applies to both the content and padding areas
- Child elements with positioning may still extend beyond overflow boundaries
- Text truncation and ellipsis are not automatic; hidden text is simply clipped
- Overflow behavior can differ between horizontal and vertical directions (use overflow-x and overflow-y)

---

## Data Binding

The overflow property integrates with data binding to create dynamic content areas that adapt their clipping behavior based on data conditions. This enables responsive content handling, conditional overflow management, and adaptive container sizing.

### Example 1: Dynamic overflow based on content length

```html
<style>
    .content-box {
        width: 400pt;
        height: 150pt;
        padding: 15pt;
        border: 2pt solid #2563eb;
        background-color: #dbeafe;
        margin-bottom: 20pt;
    }
</style>
<body>
    <!-- Apply overflow: hidden only when content exceeds threshold -->
    <div class="content-box" style="overflow: {{contentLength > 500 ? 'hidden' : 'visible'}}">
        <h3>{{title}}</h3>
        <p>{{content}}</p>
    </div>

    <!-- Conditional overflow for preview mode vs full view -->
    <div class="content-box" style="overflow: {{isPreviewMode ? 'hidden' : 'visible'}}; height: {{isPreviewMode ? '150pt' : 'auto'}}">
        <h3>Article Preview</h3>
        <p>{{articleContent}}</p>
    </div>
</body>
```

### Example 2: Adaptive overflow for different page sizes

```html
<style>
    .adaptive-container {
        width: 100%;
        padding: 20pt;
        border: 2pt solid #d1d5db;
    }
</style>
<body>
    <!-- Adjust overflow behavior based on page size -->
    <div class="adaptive-container"
         style="height: {{pageSize === 'A4' ? '250pt' : '300pt'}};
                overflow: {{allowOverflow ? 'visible' : 'hidden'}}">
        <h2>{{sectionTitle}}</h2>
        <p>{{sectionContent}}</p>
    </div>

    <!-- Conditional clipping for print vs screen -->
    <div style="width: 500pt;
                height: {{outputFormat === 'print' ? '200pt' : 'auto'}};
                overflow: {{outputFormat === 'print' ? 'hidden' : 'visible'}};
                padding: 15pt;
                border: 1pt solid #e5e7eb;">
        <h3>Content Area</h3>
        <p>{{dynamicContent}}</p>
    </div>
</body>
```

### Example 3: Conditional overflow for data-driven layouts

```html
<style>
    .dashboard-panel {
        width: 300pt;
        display: inline-block;
        vertical-align: top;
        margin: 10pt;
        padding: 15pt;
        background-color: white;
        border: 2pt solid #e5e7eb;
    }
</style>
<body>
    <!-- Dynamic overflow based on panel configuration -->
    <div class="dashboard-panel"
         style="height: {{panel.fixedHeight ? panel.height + 'pt' : 'auto'}};
                overflow: {{panel.fixedHeight ? 'hidden' : 'visible'}}">
        <h3>{{panel.title}}</h3>
        <div>{{panel.content}}</div>
    </div>

    <!-- Conditional overflow for different user roles -->
    <div style="width: 450pt;
                height: {{userRole === 'admin' ? 'auto' : '180pt'}};
                overflow: {{userRole === 'admin' ? 'visible' : 'hidden'}};
                padding: 20pt;
                background-color: #f9fafb;">
        <h2>System Information</h2>
        <p>{{systemDetails}}</p>
        <div style="display: {{userRole === 'admin' ? 'block' : 'none'}}">
            <h3>Advanced Details</h3>
            <p>{{advancedInfo}}</p>
        </div>
    </div>
</body>
```

---

## Examples

### Example 1: Basic overflow hidden

```html
<style>
    .container {
        width: 300pt;
        height: 100pt;
        overflow: hidden;
        border: 2pt solid #2563eb;
        padding: 10pt;
        background-color: #dbeafe;
    }
    .container p {
        margin: 0 0 10pt 0;
    }
</style>
<body>
    <div class="container">
        <p>This is a container with overflow: hidden.</p>
        <p>Any content that extends beyond the 100pt height will be clipped
        and not visible in the rendered output.</p>
        <p>This paragraph might be partially or completely hidden if there
        is too much content.</p>
        <p>Additional content continues here but may not be visible...</p>
    </div>
</body>
```

### Example 2: Overflow visible (default behavior)

```html
<style>
    .box-visible {
        width: 250pt;
        height: 80pt;
        overflow: visible;
        border: 2pt solid #16a34a;
        background-color: #dcfce7;
        padding: 10pt;
        margin-bottom: 50pt;
    }
    .box-visible p {
        margin: 0;
    }
</style>
<body>
    <h2>Overflow Visible Example</h2>
    <div class="box-visible">
        <p>This container has overflow: visible (the default).</p>
        <p>Content that exceeds the height will extend beyond the border
        and may overlap elements below.</p>
        <p>This demonstrates the default overflow behavior where content
        is not clipped.</p>
    </div>
    <p style="margin-top: 20pt;">This paragraph might be overlapped by the
    overflowing content above if there is insufficient space.</p>
</body>
```

### Example 3: Comparison of overflow values

```html
<style>
    .overflow-demo {
        display: inline-block;
        width: 200pt;
        height: 100pt;
        margin: 10pt;
        padding: 10pt;
        border: 2pt solid #1f2937;
        vertical-align: top;
    }
    .demo-visible {
        overflow: visible;
        background-color: #dbeafe;
    }
    .demo-hidden {
        overflow: hidden;
        background-color: #dcfce7;
    }
    .demo-label {
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 5pt;
        display: block;
    }
</style>
<body>
    <h2>Overflow Property Comparison</h2>
    <div class="overflow-demo demo-visible">
        <span class="demo-label">overflow: visible</span>
        <p style="margin: 5pt 0;">This content may extend beyond the box
        boundaries and overlap other content. The border shows the actual
        container size, but content is not restricted.</p>
    </div>
    <div class="overflow-demo demo-hidden">
        <span class="demo-label">overflow: hidden</span>
        <p style="margin: 5pt 0;">This content is clipped at the box
        boundaries. Any text or elements exceeding the height are cut off
        and not rendered in the final output.</p>
    </div>
</body>
```

### Example 4: Fixed-height content area

```html
<style>
    .article-preview {
        width: 400pt;
        height: 150pt;
        overflow: hidden;
        padding: 15pt;
        background-color: white;
        border: 1pt solid #d1d5db;
        margin-bottom: 20pt;
    }
    .article-preview h3 {
        margin: 0 0 10pt 0;
        color: #1e3a8a;
        font-size: 16pt;
    }
    .article-preview p {
        margin: 0 0 10pt 0;
        line-height: 1.6;
        text-align: justify;
    }
    .read-more {
        display: inline-block;
        padding: 8pt 15pt;
        background-color: #2563eb;
        color: white;
        font-weight: bold;
        margin-top: 10pt;
    }
</style>
<body>
    <div class="article-preview">
        <h3>Article Title: Understanding PDF Layout</h3>
        <p>This is a preview of a longer article. The content is constrained
        to a fixed height using overflow: hidden. Only the first portion of
        the article is visible, creating a preview effect.</p>
        <p>Additional paragraphs continue here, but they may be cut off if
        they exceed the 150pt height limit. This technique is useful for
        creating consistent card layouts or article previews.</p>
        <p>Even more content that might not be visible...</p>
        <div class="read-more">Read More</div>
    </div>
</body>
```

### Example 5: Image cropping with overflow

```html
<style>
    .image-container {
        width: 200pt;
        height: 150pt;
        overflow: hidden;
        border: 2pt solid #d1d5db;
        margin: 20pt;
        display: inline-block;
    }
    .image-container img {
        width: 300pt;
        height: auto;
        margin-left: -50pt;
        margin-top: -20pt;
    }
    .caption {
        text-align: center;
        font-size: 10pt;
        color: #6b7280;
        margin-top: 5pt;
    }
</style>
<body>
    <h2>Cropped Image Display</h2>
    <div style="text-align: center;">
        <div style="display: inline-block;">
            <div class="image-container">
                <img src="large-photo.jpg" alt="Cropped view" />
            </div>
            <div class="caption">Cropped to 200x150pt</div>
        </div>
    </div>
</body>
```

### Example 6: Product card with constrained description

```html
<style>
    .product-card {
        width: 220pt;
        margin: 15pt;
        border: 2pt solid #e5e7eb;
        background-color: white;
        display: inline-block;
        vertical-align: top;
    }
    .product-image-area {
        width: 100%;
        height: 180pt;
        overflow: hidden;
        background-color: #f3f4f6;
    }
    .product-details {
        padding: 15pt;
    }
    .product-title {
        font-size: 14pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
    }
    .product-description {
        height: 60pt;
        overflow: hidden;
        font-size: 10pt;
        line-height: 1.5;
        color: #6b7280;
        margin-bottom: 10pt;
    }
    .product-price {
        font-size: 18pt;
        font-weight: bold;
        color: #16a34a;
    }
</style>
<body>
    <div class="product-card">
        <div class="product-image-area">
            <img src="product.jpg" style="width: 100%;" />
        </div>
        <div class="product-details">
            <div class="product-title">Premium Widget</div>
            <div class="product-description">
                This is a detailed product description that might be quite
                long. The overflow: hidden property ensures the description
                area maintains a consistent height across all product cards,
                even if descriptions vary in length. Additional details...
            </div>
            <div class="product-price">$149.99</div>
        </div>
    </div>
</body>
```

### Example 7: Dashboard panel with fixed dimensions

```html
<style>
    .dashboard-panel {
        width: 280pt;
        height: 200pt;
        overflow: hidden;
        background-color: white;
        border: 2pt solid #e5e7eb;
        padding: 20pt;
        margin: 10pt;
        display: inline-block;
        vertical-align: top;
    }
    .panel-header {
        font-size: 16pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 15pt;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #e5e7eb;
    }
    .panel-content {
        font-size: 11pt;
        line-height: 1.6;
    }
    .metric {
        margin-bottom: 10pt;
    }
    .metric-label {
        color: #6b7280;
        font-size: 10pt;
    }
    .metric-value {
        font-size: 24pt;
        font-weight: bold;
        color: #2563eb;
    }
</style>
<body>
    <h1>Sales Dashboard</h1>
    <div class="dashboard-panel">
        <div class="panel-header">Q4 Performance</div>
        <div class="panel-content">
            <div class="metric">
                <div class="metric-label">Total Revenue</div>
                <div class="metric-value">$487K</div>
            </div>
            <div class="metric">
                <div class="metric-label">Growth Rate</div>
                <div class="metric-value">+23%</div>
            </div>
            <div class="metric">
                <div class="metric-label">Customer Count</div>
                <div class="metric-value">1,842</div>
            </div>
        </div>
    </div>
</body>
```

### Example 8: Newsletter sidebar with overflow control

```html
<style>
    .newsletter-layout {
        padding: 30pt;
    }
    .sidebar {
        width: 180pt;
        float: left;
        margin-right: 20pt;
    }
    .sidebar-box {
        background-color: #fef3c7;
        border: 2pt solid #f59e0b;
        padding: 15pt;
        margin-bottom: 20pt;
        height: 120pt;
        overflow: hidden;
    }
    .sidebar-title {
        font-weight: bold;
        color: #92400e;
        margin-bottom: 10pt;
        font-size: 12pt;
    }
    .sidebar-content {
        font-size: 10pt;
        line-height: 1.5;
    }
    .main-content {
        margin-left: 220pt;
    }
</style>
<body>
    <div class="newsletter-layout">
        <div class="sidebar">
            <div class="sidebar-box">
                <div class="sidebar-title">Quick Tips</div>
                <div class="sidebar-content">
                    Here are some helpful tips for your next project. The fixed
                    height ensures consistent layout even with varying content
                    lengths. Additional tips may be clipped...
                </div>
            </div>
            <div class="sidebar-box">
                <div class="sidebar-title">Did You Know?</div>
                <div class="sidebar-content">
                    Interesting facts and information about PDF generation and
                    document layout techniques...
                </div>
            </div>
        </div>
        <div class="main-content">
            <h1>Main Article</h1>
            <p>The main content flows alongside the fixed-height sidebar boxes...</p>
        </div>
    </div>
</body>
```

### Example 9: Testimonial cards with consistent heights

```html
<style>
    .testimonial-grid {
        padding: 20pt;
        text-align: center;
    }
    .testimonial {
        width: 250pt;
        height: 180pt;
        display: inline-block;
        vertical-align: top;
        margin: 10pt;
        padding: 20pt;
        background-color: #f9fafb;
        border: 2pt solid #e5e7eb;
        border-radius: 8pt;
    }
    .testimonial-quote {
        height: 100pt;
        overflow: hidden;
        font-style: italic;
        font-size: 11pt;
        line-height: 1.6;
        color: #1f2937;
        margin-bottom: 15pt;
    }
    .testimonial-author {
        font-weight: bold;
        color: #2563eb;
        font-size: 12pt;
    }
    .testimonial-role {
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <h1 style="text-align: center;">Customer Testimonials</h1>
    <div class="testimonial-grid">
        <div class="testimonial">
            <div class="testimonial-quote">
                "This product has transformed our workflow completely. We've
                seen a 50% increase in productivity and our team couldn't be
                happier with the results. Highly recommended for any business!"
            </div>
            <div class="testimonial-author">Sarah Johnson</div>
            <div class="testimonial-role">CEO, TechCorp</div>
        </div>
        <div class="testimonial">
            <div class="testimonial-quote">
                "Outstanding quality and exceptional support. The best
                investment we've made this year!"
            </div>
            <div class="testimonial-author">Michael Chen</div>
            <div class="testimonial-role">Director, InnovateLab</div>
        </div>
    </div>
</body>
```

### Example 10: Brochure section with constrained areas

```html
<style>
    .brochure-section {
        width: 100%;
        padding: 30pt;
        background-color: #1e3a8a;
        color: white;
    }
    .feature-columns {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 20pt;
    }
    .feature-column {
        display: table-cell;
        vertical-align: top;
        background-color: rgba(255, 255, 255, 0.1);
        padding: 20pt;
        border-radius: 8pt;
    }
    .feature-title {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 15pt;
    }
    .feature-content {
        height: 120pt;
        overflow: hidden;
        font-size: 11pt;
        line-height: 1.6;
    }
    .feature-icon {
        width: 50pt;
        height: 50pt;
        background-color: #fbbf24;
        color: #1e3a8a;
        font-size: 24pt;
        font-weight: bold;
        text-align: center;
        line-height: 50pt;
        margin-bottom: 15pt;
        border-radius: 25pt;
    }
</style>
<body>
    <div class="brochure-section">
        <h1 style="text-align: center; margin-top: 0;">Our Services</h1>
        <div class="feature-columns">
            <div class="feature-column">
                <div class="feature-icon">1</div>
                <div class="feature-title">Consulting</div>
                <div class="feature-content">
                    Expert consulting services to help you achieve your business
                    goals. Our team brings decades of experience and proven
                    methodologies to every engagement. We work closely with you
                    to understand your unique challenges...
                </div>
            </div>
            <div class="feature-column">
                <div class="feature-icon">2</div>
                <div class="feature-title">Development</div>
                <div class="feature-content">
                    Custom development solutions tailored to your needs using
                    the latest technologies and best practices...
                </div>
            </div>
            <div class="feature-column">
                <div class="feature-icon">3</div>
                <div class="feature-title">Support</div>
                <div class="feature-content">
                    24/7 support services ensuring your systems run smoothly
                    and efficiently at all times...
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 11: Magazine article with sidebar callouts

```html
<style>
    .magazine-article {
        width: 600pt;
        margin: 0 auto;
        padding: 40pt;
    }
    .article-title {
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 20pt;
        border-bottom: 3pt solid #1e3a8a;
        padding-bottom: 10pt;
    }
    .callout-box {
        width: 220pt;
        height: 140pt;
        overflow: hidden;
        float: right;
        margin: 0 0 15pt 20pt;
        padding: 15pt;
        background-color: #dbeafe;
        border: 3pt solid #2563eb;
    }
    .callout-title {
        font-weight: bold;
        font-size: 14pt;
        color: #1e40af;
        margin-bottom: 10pt;
    }
    .callout-content {
        font-size: 10pt;
        line-height: 1.5;
    }
    .article-body {
        text-align: justify;
        line-height: 1.7;
    }
</style>
<body>
    <div class="magazine-article">
        <div class="article-title">The Evolution of Document Design</div>
        <div class="callout-box">
            <div class="callout-title">Key Insight</div>
            <div class="callout-content">
                Understanding overflow properties is crucial for creating
                professional, polished documents with consistent layouts.
                Fixed-height elements ensure visual harmony across pages.
                Additional insights that might be clipped...
            </div>
        </div>
        <div class="article-body">
            <p>Modern document design requires careful attention to layout and
            content flow. The overflow property provides essential control over
            how content behaves within constrained spaces.</p>
            <p>Professional designers use overflow to create consistent,
            predictable layouts that maintain visual integrity across different
            content lengths and document types.</p>
        </div>
    </div>
</body>
```

### Example 12: Report summary boxes

```html
<style>
    .report-summary {
        padding: 30pt;
    }
    .summary-grid {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 15pt;
    }
    .summary-row {
        display: table-row;
    }
    .summary-box {
        display: table-cell;
        padding: 20pt;
        background-color: #f3f4f6;
        border-left: 5pt solid #2563eb;
    }
    .summary-title {
        font-size: 14pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 12pt;
    }
    .summary-content {
        height: 100pt;
        overflow: hidden;
        font-size: 10pt;
        line-height: 1.6;
        color: #4b5563;
    }
    .summary-footer {
        margin-top: 10pt;
        font-size: 9pt;
        color: #6b7280;
        font-style: italic;
    }
</style>
<body>
    <div class="report-summary">
        <h1>Executive Summary</h1>
        <div class="summary-grid">
            <div class="summary-row">
                <div class="summary-box">
                    <div class="summary-title">Financial Overview</div>
                    <div class="summary-content">
                        Revenue increased by 24% year-over-year, reaching $12.5M
                        in Q4. Operating expenses remained stable while gross
                        margins improved significantly. Key drivers included
                        expanded market share and improved operational efficiency.
                        Detailed breakdown continues...
                    </div>
                    <div class="summary-footer">See page 12 for details</div>
                </div>
                <div class="summary-box">
                    <div class="summary-title">Market Analysis</div>
                    <div class="summary-content">
                        Market conditions remained favorable throughout the
                        quarter. Competitive landscape analysis shows strong
                        positioning relative to industry peers...
                    </div>
                    <div class="summary-footer">See page 18 for details</div>
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 13: Event schedule with time-constrained entries

```html
<style>
    .schedule {
        width: 550pt;
        margin: 20pt auto;
    }
    .schedule-entry {
        margin-bottom: 15pt;
        border: 2pt solid #e5e7eb;
        background-color: white;
        overflow: hidden;
    }
    .entry-time {
        width: 120pt;
        float: left;
        padding: 15pt;
        background-color: #1e3a8a;
        color: white;
        font-weight: bold;
        font-size: 14pt;
        text-align: center;
    }
    .entry-details {
        margin-left: 120pt;
        padding: 15pt;
        height: 80pt;
        overflow: hidden;
    }
    .entry-title {
        font-size: 14pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 8pt;
    }
    .entry-description {
        font-size: 10pt;
        line-height: 1.5;
        color: #6b7280;
    }
</style>
<body>
    <h1 style="text-align: center;">Conference Schedule</h1>
    <div class="schedule">
        <div class="schedule-entry">
            <div class="entry-time">9:00 AM</div>
            <div class="entry-details">
                <div class="entry-title">Opening Keynote</div>
                <div class="entry-description">
                    Welcome address and conference overview. Discussion of key
                    themes and objectives for the day. Introductions to featured
                    speakers and special guests. Additional information that may
                    be clipped due to space constraints...
                </div>
            </div>
        </div>
        <div class="schedule-entry">
            <div class="entry-time">10:30 AM</div>
            <div class="entry-details">
                <div class="entry-title">Technical Workshop</div>
                <div class="entry-description">
                    Hands-on session covering advanced PDF generation techniques
                    and best practices for document layout...
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 14: Price comparison table with fixed cells

```html
<style>
    .pricing-table {
        width: 100%;
        border-collapse: collapse;
        margin: 30pt 0;
    }
    .pricing-table th {
        padding: 15pt;
        background-color: #1f2937;
        color: white;
        font-size: 14pt;
        border: 1pt solid #374151;
    }
    .pricing-table td {
        padding: 12pt;
        border: 1pt solid #d1d5db;
        text-align: center;
    }
    .feature-cell {
        text-align: left;
        font-weight: bold;
        color: #1e3a8a;
    }
    .features-list {
        height: 100pt;
        overflow: hidden;
        text-align: left;
        font-size: 10pt;
        line-height: 1.8;
    }
    .price-highlight {
        font-size: 24pt;
        font-weight: bold;
        color: #16a34a;
    }
</style>
<body>
    <h1 style="text-align: center;">Pricing Plans</h1>
    <table class="pricing-table">
        <thead>
            <tr>
                <th style="width: 40%;">Features</th>
                <th style="width: 20%;">Basic</th>
                <th style="width: 20%;">Pro</th>
                <th style="width: 20%;">Enterprise</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="feature-cell">Monthly Price</td>
                <td><div class="price-highlight">$29</div></td>
                <td><div class="price-highlight">$79</div></td>
                <td><div class="price-highlight">$199</div></td>
            </tr>
            <tr>
                <td class="feature-cell">
                    <div class="features-list">
                        • PDF Generation<br/>
                        • Basic Templates<br/>
                        • Email Support<br/>
                        • 100 Documents/month<br/>
                        • Standard Processing<br/>
                        • Basic Analytics<br/>
                        • Single User<br/>
                        • More features...<br/>
                        • Additional items...
                    </div>
                </td>
                <td>✓</td>
                <td>✓✓</td>
                <td>✓✓✓</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 15: Certificate with controlled content areas

```html
<style>
    .certificate {
        width: 700pt;
        height: 500pt;
        margin: 40pt auto;
        padding: 40pt;
        border: 10pt double #1e3a8a;
        background-color: #fffef7;
        position: relative;
    }
    .cert-border {
        width: 100%;
        height: 100%;
        border: 2pt solid #1e3a8a;
        padding: 30pt;
        overflow: hidden;
    }
    .cert-title {
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        text-align: center;
        margin-bottom: 30pt;
    }
    .cert-content {
        height: 200pt;
        overflow: hidden;
        text-align: center;
        padding: 20pt;
    }
    .recipient-name {
        font-size: 28pt;
        font-weight: bold;
        color: #1f2937;
        margin: 20pt 0;
        border-bottom: 2pt solid #1f2937;
        display: inline-block;
        padding-bottom: 5pt;
    }
    .cert-text {
        font-size: 14pt;
        line-height: 1.8;
        color: #374151;
    }
    .cert-footer {
        text-align: center;
        margin-top: 40pt;
        font-size: 11pt;
    }
</style>
<body>
    <div class="certificate">
        <div class="cert-border">
            <div class="cert-title">Certificate of Excellence</div>
            <div class="cert-content">
                <div class="cert-text">This certifies that</div>
                <div class="recipient-name">Jennifer Williams</div>
                <div class="cert-text">
                    has successfully completed the Advanced PDF Generation
                    Course with distinction, demonstrating exceptional skill
                    and understanding of document layout principles, overflow
                    management, and professional design techniques.
                </div>
            </div>
            <div class="cert-footer">
                Awarded on October 14, 2025
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [overflow-x](/reference/cssproperties/css_prop_overflow-x) - Control horizontal overflow behavior
- [overflow-y](/reference/cssproperties/css_prop_overflow-y) - Control vertical overflow behavior
- [width](/reference/cssproperties/css_prop_width) - Set element width
- [height](/reference/cssproperties/css_prop_height) - Set element height
- [max-height](/reference/cssproperties/css_prop_max-height) - Set maximum height constraint
- [max-width](/reference/cssproperties/css_prop_max-width) - Set maximum width constraint
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
