---
layout: default
title: break-inside
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# break-inside : Break Inside Property

The `break-inside` property controls whether page and column breaks are allowed within an element when generating PDF documents. This is the modern CSS3 replacement for `page-break-inside`, offering more control including column break prevention for multi-column layouts. It's essential for keeping content blocks together and maintaining visual coherence.

## Usage

```css
selector {
    break-inside: value;
}
```

The break-inside property prevents or allows breaks from occurring within the bounds of an element, ensuring content integrity in your PDF documents and multi-column layouts.

---

## Supported Values

### auto (default)
Automatic breaking behavior. The browser/PDF generator may insert page or column breaks within the element if needed to fit content. This is the default value.

### avoid
Attempts to avoid any breaks (page or column) within the element. The PDF generator will try to keep the entire element together. If the element is too large to fit on one page or column, this directive may be overridden.

### avoid-page
Specifically attempts to avoid page breaks within the element, but allows column breaks in multi-column layouts. Useful when you want content to flow between columns but not split across pages.

### avoid-column
Specifically attempts to avoid column breaks within the element, but allows page breaks. Useful for keeping content together within a column while allowing natural page breaks.

---

## Supported Elements

The `break-inside` property can be applied to:
- Block-level elements (`<div>`, `<section>`, `<article>`)
- Tables (`<table>`)
- Lists (`<ul>`, `<ol>`)
- Paragraphs (`<p>`)
- Figures and images with captions
- Code blocks (`<pre>`)
- Block quotes (`<blockquote>`)
- Any container element

---

## Notes

- This is the modern CSS3 property that replaces `page-break-inside`
- The `avoid` value is a suggestion and may be overridden if content is too large
- `avoid-page` and `avoid-column` provide fine-grained control in multi-column layouts
- Particularly useful for tables, preventing rows from splitting across pages or columns
- Excellent for keeping figures with their captions together
- Helps prevent orphaned list items or code block fragments
- Should be used on container elements rather than inline elements
- Most effective with content that fits reasonably on a single page or column
- May increase blank space at page or column bottoms if content cannot fit
- Does not prevent breaks before or after the element, only within it

---

## Data Binding

The `break-inside` property supports data binding, allowing you to dynamically control content integrity and grouping based on data attributes, content type, or configuration. This is essential for maintaining visual coherence in data-driven documents with variable content.

### Example 1: Priority-based content grouping

```html
<style>
    .content-card {
        break-inside: {{card.priority === 'high' || card.keepTogether ? 'avoid' : 'auto'}};
        break-inside: {{card.allowColumnBreak ? 'avoid-page' : 'avoid'}};
        border: {{card.priority === 'high' ? '2pt' : '1pt'}} solid #e5e7eb;
        padding: {{card.padding || 15}}pt;
        margin: 20pt 0;
    }
    .card-title {
        font-size: {{card.titleSize || 16}}pt;
        font-weight: bold;
        color: {{card.priority === 'high' ? '#dc2626' : '#1e3a8a'}};
    }
</style>
<body>
    {{#each contentCards}}
    <div class="content-card">
        <h3 class="card-title">{{cardTitle}}</h3>
        <div>{{cardContent}}</div>
    </div>
    {{/each}}
</body>
```

### Example 2: Configurable table and figure integrity

```html
<style>
    .figure-container {
        break-inside: {{figureConfig.preventSplit ? 'avoid' : 'auto'}};
        break-inside: {{figureConfig.allowColumnBreak ? 'avoid-page' : 'avoid'}};
        text-align: center;
        margin: {{figureConfig.margin || 25}}pt 0;
    }
    .figure-image {
        width: {{figure.width || 100}}%;
        height: {{figure.height || 300}}pt;
        background-color: #e5e7eb;
        border: 2pt solid #9ca3af;
    }
    .figure-caption {
        margin-top: 10pt;
        font-size: 11pt;
        color: #4b5563;
        font-style: {{figureConfig.italicCaption ? 'italic' : 'normal'}};
    }
</style>
<body>
    {{#each figures}}
    <div class="figure-container">
        <div class="figure-image">[{{figureType}}]</div>
        <div class="figure-caption">
            <strong>Figure {{figureNumber}}:</strong> {{caption}}
        </div>
    </div>
    {{/each}}
</body>
```

### Example 3: Multi-column newsletter with smart content breaking

```html
<style>
    .newsletter {
        column-count: {{layout.columnCount || 2}};
        column-gap: {{layout.columnGap || 30}}pt;
        column-rule: {{layout.showRule ? '1pt solid #e5e7eb' : 'none'}};
    }
    .article-block {
        break-inside: {{article.avoidColumnBreak ? 'avoid-column' : 'auto'}};
        break-inside: {{article.avoidPageBreak ? 'avoid-page' : 'auto'}};
        margin-bottom: 25pt;
    }
    .article-title {
        font-size: {{article.titleSize || 16}}pt;
        font-weight: bold;
        color: {{article.featured ? '#dc2626' : '#1e3a8a'}};
        margin-bottom: 10pt;
    }
    .article-meta {
        font-size: 9pt;
        color: #6b7280;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="newsletter">
        {{#each articles}}
        <div class="article-block">
            <h2 class="article-title">{{articleTitle}}</h2>
            <div class="article-meta">By {{author}} | {{date}}</div>
            <p>{{articleContent}}</p>
        </div>
        {{/each}}
    </div>
</body>
```

---

## Examples

### Example 1: Keep data tables together

```html
<style>
    .report-table {
        break-inside: avoid;
        width: 100%;
        border-collapse: collapse;
        margin: 25pt 0;
        background-color: white;
        box-shadow: 0 1pt 3pt rgba(0,0,0,0.1);
    }
    .report-table caption {
        font-size: 14pt;
        font-weight: bold;
        padding: 10pt;
        background-color: #1e3a8a;
        color: white;
        text-align: left;
    }
    .report-table th {
        background-color: #3b82f6;
        color: white;
        padding: 12pt;
        text-align: left;
        font-weight: bold;
    }
    .report-table td {
        padding: 10pt;
        border-bottom: 1pt solid #e5e7eb;
    }
    .report-table tr:nth-child(even) {
        background-color: #f9fafb;
    }
</style>
<body>
    <h2>Quarterly Financial Summary</h2>
    <p>The following table presents our financial performance for Q1 2025:</p>

    <table class="report-table">
        <caption>Revenue by Product Line - Q1 2025</caption>
        <thead>
            <tr>
                <th>Product</th>
                <th>Units Sold</th>
                <th>Revenue</th>
                <th>Growth</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Product A</td>
                <td>15,234</td>
                <td>$456,789</td>
                <td>+23%</td>
            </tr>
            <tr>
                <td>Product B</td>
                <td>8,912</td>
                <td>$267,360</td>
                <td>+15%</td>
            </tr>
            <tr>
                <td>Product C</td>
                <td>12,445</td>
                <td>$373,350</td>
                <td>+31%</td>
            </tr>
            <tr>
                <td>Product D</td>
                <td>6,789</td>
                <td>$203,670</td>
                <td>+8%</td>
            </tr>
        </tbody>
    </table>

    <p>Analysis of these results shows strong performance across all lines...</p>
</body>
```

### Example 2: Multi-column newsletter with column control

```html
<style>
    .newsletter-content {
        column-count: 2;
        column-gap: 30pt;
        column-rule: 2pt solid #e5e7eb;
    }
    .article-block {
        break-inside: avoid-column;
        margin-bottom: 25pt;
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
        font-style: italic;
    }
    .article-text {
        font-size: 11pt;
        line-height: 1.6;
        text-align: justify;
    }
    .pullquote {
        break-inside: avoid-column;
        background-color: #eff6ff;
        border-left: 4pt solid #3b82f6;
        padding: 15pt;
        margin: 15pt 0;
        font-style: italic;
        font-size: 12pt;
    }
</style>
<body>
    <div class="newsletter-content">
        <div class="article-block">
            <h2 class="article-title">Innovation in Healthcare</h2>
            <p class="article-meta">By Dr. Sarah Chen | March 15, 2025</p>
            <div class="article-text">
                <p>Recent advances in medical technology are revolutionizing
                patient care. New diagnostic tools powered by artificial
                intelligence can detect diseases earlier and more accurately.</p>
            </div>
        </div>

        <div class="pullquote">
            "The integration of AI in healthcare represents the most significant
            advancement in medical diagnostics in decades."
        </div>

        <div class="article-block">
            <h2 class="article-title">Market Trends Analysis</h2>
            <p class="article-meta">By Michael Rodriguez | March 15, 2025</p>
            <div class="article-text">
                <p>Consumer behavior continues to shift toward sustainable
                products, with 67% of shoppers prioritizing eco-friendly options.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 3: Keep figures with captions together

```html
<style>
    .figure-wrapper {
        break-inside: avoid;
        margin: 30pt 0;
        padding: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        border-radius: 8pt;
    }
    .figure-number {
        font-size: 12pt;
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 10pt;
    }
    .figure-image {
        width: 100%;
        height: 300pt;
        background-color: #e5e7eb;
        border: 2pt solid #d1d5db;
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 15pt 0;
        font-size: 14pt;
        color: #9ca3af;
    }
    .figure-caption {
        font-size: 11pt;
        color: #4b5563;
        line-height: 1.6;
        text-align: justify;
        margin-top: 10pt;
    }
    .caption-label {
        font-weight: bold;
    }
</style>
<body>
    <h2>Research Findings</h2>
    <p>Our analysis revealed several important patterns in the data.</p>

    <div class="figure-wrapper">
        <div class="figure-number">FIGURE 1</div>
        <div class="figure-image">[Graph: Annual Growth Trends]</div>
        <div class="figure-caption">
            <span class="caption-label">Figure 1:</span> Annual growth trends
            from 2020-2025 showing consistent upward trajectory with seasonal
            variations. Note the significant acceleration in Q4 2024.
        </div>
    </div>

    <p>As illustrated in Figure 1, the growth pattern demonstrates...</p>

    <div class="figure-wrapper">
        <div class="figure-number">FIGURE 2</div>
        <div class="figure-image">[Chart: Regional Distribution]</div>
        <div class="figure-caption">
            <span class="caption-label">Figure 2:</span> Geographic distribution
            of customer base across five major regions. North America and Europe
            represent 68% of total market share.
        </div>
    </div>
</body>
```

### Example 4: Keep code blocks intact

```html
<style>
    .code-example {
        break-inside: avoid;
        margin: 25pt 0;
    }
    .code-header {
        background-color: #374151;
        color: #f9fafb;
        padding: 10pt 15pt;
        border-radius: 5pt 5pt 0 0;
        font-family: monospace;
        font-size: 11pt;
    }
    .code-block {
        background-color: #1f2937;
        color: #f9fafb;
        padding: 20pt;
        border-radius: 0 0 5pt 5pt;
        font-family: 'Courier New', monospace;
        font-size: 10pt;
        line-height: 1.6;
        overflow-x: auto;
    }
    .code-comment {
        color: #9ca3af;
    }
    .code-keyword {
        color: #60a5fa;
    }
    .code-string {
        color: #34d399;
    }
    .code-function {
        color: #fbbf24;
    }
</style>
<body>
    <h3>Implementation Example</h3>
    <p>The following code demonstrates the recommended approach:</p>

    <div class="code-example">
        <div class="code-header">example.js</div>
        <div class="code-block">
<span class="code-comment">// Initialize the application</span>
<span class="code-keyword">function</span> <span class="code-function">initializeApp</span>() {
    <span class="code-keyword">const</span> config = {
        apiKey: <span class="code-string">'your-api-key'</span>,
        endpoint: <span class="code-string">'https://api.example.com'</span>
    };

    <span class="code-comment">// Setup authentication</span>
    <span class="code-keyword">const</span> auth = <span class="code-keyword">new</span> <span class="code-function">AuthService</span>(config);
    auth.<span class="code-function">login</span>()
        .<span class="code-function">then</span>(user => {
            console.<span class="code-function">log</span>(<span class="code-string">'User logged in:'</span>, user);
        })
        .<span class="code-function">catch</span>(error => {
            console.<span class="code-function">error</span>(<span class="code-string">'Login failed:'</span>, error);
        });
}
        </div>
    </div>

    <p>This implementation ensures proper error handling and authentication...</p>
</body>
```

### Example 5: Keep callout boxes together

```html
<style>
    .callout {
        break-inside: avoid;
        padding: 20pt;
        margin: 25pt 0;
        border-radius: 8pt;
        box-shadow: 0 2pt 4pt rgba(0,0,0,0.1);
    }
    .callout-info {
        background-color: #eff6ff;
        border-left: 5pt solid #3b82f6;
    }
    .callout-warning {
        background-color: #fef3c7;
        border-left: 5pt solid #f59e0b;
    }
    .callout-success {
        background-color: #f0fdf4;
        border-left: 5pt solid #10b981;
    }
    .callout-danger {
        background-color: #fef2f2;
        border-left: 5pt solid #ef4444;
    }
    .callout-header {
        display: flex;
        align-items: center;
        gap: 10pt;
        margin-bottom: 10pt;
    }
    .callout-icon {
        width: 30pt;
        height: 30pt;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: bold;
        font-size: 16pt;
    }
    .callout-info .callout-icon {
        background-color: #3b82f6;
        color: white;
    }
    .callout-warning .callout-icon {
        background-color: #f59e0b;
        color: white;
    }
    .callout-title {
        font-size: 14pt;
        font-weight: bold;
    }
    .callout-info .callout-title {
        color: #1e40af;
    }
    .callout-warning .callout-title {
        color: #92400e;
    }
    .callout-content {
        line-height: 1.6;
    }
</style>
<body>
    <h2>Safety Guidelines</h2>

    <div class="callout callout-info">
        <div class="callout-header">
            <div class="callout-icon">i</div>
            <div class="callout-title">Information</div>
        </div>
        <div class="callout-content">
            Regular maintenance checks should be performed every 30 days to
            ensure optimal performance. Refer to the maintenance schedule in
            Appendix B for detailed procedures.
        </div>
    </div>

    <div class="callout callout-warning">
        <div class="callout-header">
            <div class="callout-icon">!</div>
            <div class="callout-title">Warning</div>
        </div>
        <div class="callout-content">
            Always disconnect power before performing any maintenance. Failure
            to follow this procedure may result in electric shock or equipment
            damage. Only qualified personnel should perform maintenance tasks.
        </div>
    </div>

    <p>Following these guidelines will ensure safe operation...</p>
</body>
```

### Example 6: Keep contact cards together

```html
<style>
    .contact-grid {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 20pt;
        margin: 25pt 0;
    }
    .contact-card {
        break-inside: avoid;
        border: 2pt solid #e5e7eb;
        border-radius: 10pt;
        padding: 20pt;
        background-color: #fafafa;
    }
    .contact-photo {
        width: 80pt;
        height: 80pt;
        border-radius: 50%;
        background-color: #d1d5db;
        margin: 0 auto 15pt auto;
    }
    .contact-name {
        text-align: center;
        font-size: 16pt;
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 5pt;
    }
    .contact-title {
        text-align: center;
        font-size: 12pt;
        color: #6b7280;
        margin-bottom: 15pt;
    }
    .contact-details {
        font-size: 10pt;
        line-height: 1.8;
        color: #374151;
    }
    .contact-label {
        font-weight: bold;
    }
</style>
<body>
    <h2>Team Directory</h2>

    <div class="contact-grid">
        <div class="contact-card">
            <div class="contact-photo"></div>
            <div class="contact-name">Emily Johnson</div>
            <div class="contact-title">Project Manager</div>
            <div class="contact-details">
                <div><span class="contact-label">Email:</span> emily.j@company.com</div>
                <div><span class="contact-label">Phone:</span> (555) 123-4567</div>
                <div><span class="contact-label">Ext:</span> 1234</div>
            </div>
        </div>

        <div class="contact-card">
            <div class="contact-photo"></div>
            <div class="contact-name">Michael Chen</div>
            <div class="contact-title">Technical Lead</div>
            <div class="contact-details">
                <div><span class="contact-label">Email:</span> michael.c@company.com</div>
                <div><span class="contact-label">Phone:</span> (555) 234-5678</div>
                <div><span class="contact-label">Ext:</span> 2345</div>
            </div>
        </div>

        <div class="contact-card">
            <div class="contact-photo"></div>
            <div class="contact-name">Sarah Williams</div>
            <div class="contact-title">UX Designer</div>
            <div class="contact-details">
                <div><span class="contact-label">Email:</span> sarah.w@company.com</div>
                <div><span class="contact-label">Phone:</span> (555) 345-6789</div>
                <div><span class="contact-label">Ext:</span> 3456</div>
            </div>
        </div>

        <div class="contact-card">
            <div class="contact-photo"></div>
            <div class="contact-name">David Martinez</div>
            <div class="contact-title">QA Engineer</div>
            <div class="contact-details">
                <div><span class="contact-label">Email:</span> david.m@company.com</div>
                <div><span class="contact-label">Phone:</span> (555) 456-7890</div>
                <div><span class="contact-label">Ext:</span> 4567</div>
            </div>
        </div>
    </div>
</body>
```

### Example 7: Keep process steps together

```html
<style>
    .procedure {
        margin: 30pt 0;
    }
    .step-container {
        break-inside: avoid;
        display: flex;
        gap: 20pt;
        margin: 20pt 0;
        padding: 20pt;
        background-color: #f0f9ff;
        border-radius: 8pt;
        border-left: 5pt solid #0ea5e9;
    }
    .step-number-box {
        flex-shrink: 0;
        width: 50pt;
        height: 50pt;
        background: linear-gradient(135deg, #0ea5e9 0%, #0284c7 100%);
        color: white;
        border-radius: 10pt;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 24pt;
        font-weight: bold;
        box-shadow: 0 2pt 4pt rgba(0,0,0,0.2);
    }
    .step-content {
        flex: 1;
    }
    .step-title {
        font-size: 16pt;
        font-weight: bold;
        color: #0c4a6e;
        margin-bottom: 8pt;
    }
    .step-description {
        line-height: 1.6;
        color: #0f172a;
        margin-bottom: 10pt;
    }
    .step-notes {
        font-size: 10pt;
        color: #475569;
        font-style: italic;
        background-color: #e0f2fe;
        padding: 8pt;
        border-radius: 4pt;
    }
</style>
<body>
    <h2>Installation Procedure</h2>

    <div class="procedure">
        <div class="step-container">
            <div class="step-number-box">1</div>
            <div class="step-content">
                <div class="step-title">Verify System Requirements</div>
                <div class="step-description">
                    Check that your system meets all minimum requirements including
                    operating system version, available disk space (minimum 10GB),
                    and RAM (minimum 8GB).
                </div>
                <div class="step-notes">
                    Note: Installation may fail if requirements are not met.
                </div>
            </div>
        </div>

        <div class="step-container">
            <div class="step-number-box">2</div>
            <div class="step-content">
                <div class="step-title">Download Installation Package</div>
                <div class="step-description">
                    Navigate to the official download page and select the appropriate
                    package for your operating system. Verify the checksum to ensure
                    file integrity.
                </div>
                <div class="step-notes">
                    Note: Download size is approximately 2.5GB.
                </div>
            </div>
        </div>

        <div class="step-container">
            <div class="step-number-box">3</div>
            <div class="step-content">
                <div class="step-title">Run Installation Wizard</div>
                <div class="step-description">
                    Double-click the installer and follow the on-screen instructions.
                    Choose the installation directory and select optional components
                    as needed.
                </div>
                <div class="step-notes">
                    Note: Administrator privileges are required.
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 8: Keep testimonial cards intact

```html
<style>
    .testimonial-card {
        break-inside: avoid;
        background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
        border-radius: 12pt;
        padding: 25pt;
        margin: 25pt 0;
        box-shadow: 0 4pt 6pt rgba(0,0,0,0.1);
        position: relative;
    }
    .quote-mark {
        position: absolute;
        top: 15pt;
        left: 15pt;
        font-size: 48pt;
        color: #f59e0b;
        opacity: 0.3;
        line-height: 1;
    }
    .testimonial-text {
        font-size: 13pt;
        line-height: 1.8;
        color: #78350f;
        font-style: italic;
        margin: 20pt 0;
        position: relative;
        z-index: 1;
    }
    .testimonial-footer {
        display: flex;
        align-items: center;
        gap: 15pt;
        margin-top: 20pt;
        padding-top: 15pt;
        border-top: 2pt solid #fbbf24;
    }
    .author-avatar {
        width: 50pt;
        height: 50pt;
        border-radius: 50%;
        background-color: #f59e0b;
        flex-shrink: 0;
    }
    .author-info {
        flex: 1;
    }
    .author-name {
        font-weight: bold;
        color: #92400e;
        font-size: 13pt;
    }
    .author-role {
        font-size: 11pt;
        color: #b45309;
        margin-top: 2pt;
    }
    .star-rating {
        color: #f59e0b;
        font-size: 14pt;
    }
</style>
<body>
    <h2>Client Testimonials</h2>

    <div class="testimonial-card">
        <div class="quote-mark">"</div>
        <div class="testimonial-text">
            "Working with this team transformed our business operations. Their
            expertise and dedication exceeded all expectations. The project was
            delivered on time, within budget, and the results speak for themselves.
            I highly recommend their services to any organization."
        </div>
        <div class="testimonial-footer">
            <div class="author-avatar"></div>
            <div class="author-info">
                <div class="author-name">Jennifer Thompson</div>
                <div class="author-role">CEO, Innovation Corp</div>
            </div>
            <div class="star-rating">★★★★★</div>
        </div>
    </div>

    <div class="testimonial-card">
        <div class="quote-mark">"</div>
        <div class="testimonial-text">
            "Exceptional quality and outstanding support throughout the entire
            process. The team was responsive, professional, and truly understood
            our needs. The solution they delivered has improved our efficiency
            by 40% and continues to drive value."
        </div>
        <div class="testimonial-footer">
            <div class="author-avatar"></div>
            <div class="author-info">
                <div class="author-name">Robert Garcia</div>
                <div class="author-role">Director of Operations, Tech Solutions Ltd</div>
            </div>
            <div class="star-rating">★★★★★</div>
        </div>
    </div>
</body>
```

### Example 9: Keep statistic cards together

```html
<style>
    .stats-grid {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 20pt;
        margin: 30pt 0;
    }
    .stat-card {
        break-inside: avoid;
        background-color: white;
        border: 2pt solid #e5e7eb;
        border-radius: 10pt;
        padding: 25pt;
        text-align: center;
        box-shadow: 0 2pt 4pt rgba(0,0,0,0.05);
    }
    .stat-icon {
        width: 60pt;
        height: 60pt;
        background-color: #dbeafe;
        border-radius: 50%;
        margin: 0 auto 15pt auto;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 28pt;
    }
    .stat-value {
        font-size: 36pt;
        font-weight: bold;
        color: #1e3a8a;
        margin: 10pt 0;
    }
    .stat-label {
        font-size: 12pt;
        color: #6b7280;
        text-transform: uppercase;
        letter-spacing: 1pt;
    }
    .stat-change {
        font-size: 11pt;
        margin-top: 8pt;
    }
    .stat-positive {
        color: #16a34a;
    }
    .stat-negative {
        color: #dc2626;
    }
</style>
<body>
    <h2>Key Performance Indicators - Q1 2025</h2>

    <div class="stats-grid">
        <div class="stat-card">
            <div class="stat-icon">👥</div>
            <div class="stat-value">15.2K</div>
            <div class="stat-label">Active Users</div>
            <div class="stat-change stat-positive">↑ 23% from last quarter</div>
        </div>

        <div class="stat-card">
            <div class="stat-icon">💰</div>
            <div class="stat-value">$2.4M</div>
            <div class="stat-label">Revenue</div>
            <div class="stat-change stat-positive">↑ 18% from last quarter</div>
        </div>

        <div class="stat-card">
            <div class="stat-icon">⭐</div>
            <div class="stat-value">4.8</div>
            <div class="stat-label">Customer Rating</div>
            <div class="stat-change stat-positive">↑ 0.3 from last quarter</div>
        </div>

        <div class="stat-card">
            <div class="stat-icon">📈</div>
            <div class="stat-value">342</div>
            <div class="stat-label">New Signups</div>
            <div class="stat-change stat-positive">↑ 45% from last quarter</div>
        </div>

        <div class="stat-card">
            <div class="stat-icon">⏱</div>
            <div class="stat-value">12m</div>
            <div class="stat-label">Avg Response Time</div>
            <div class="stat-change stat-positive">↓ 15% from last quarter</div>
        </div>

        <div class="stat-card">
            <div class="stat-icon">🎯</div>
            <div class="stat-value">94%</div>
            <div class="stat-label">Goal Achievement</div>
            <div class="stat-change stat-positive">↑ 8% from last quarter</div>
        </div>
    </div>
</body>
```

### Example 10: Keep pricing tiers intact

```html
<style>
    .pricing-container {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 20pt;
        margin: 30pt 0;
    }
    .pricing-card {
        break-inside: avoid;
        border: 2pt solid #e5e7eb;
        border-radius: 12pt;
        overflow: hidden;
        background-color: white;
        box-shadow: 0 4pt 6pt rgba(0,0,0,0.1);
    }
    .pricing-header {
        background: linear-gradient(135deg, #3b82f6 0%, #1e40af 100%);
        color: white;
        padding: 25pt;
        text-align: center;
    }
    .tier-name {
        font-size: 20pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .tier-price {
        font-size: 42pt;
        font-weight: bold;
        margin: 15pt 0;
    }
    .price-period {
        font-size: 14pt;
        opacity: 0.9;
    }
    .pricing-features {
        padding: 25pt;
    }
    .features-list {
        list-style: none;
        padding: 0;
        margin: 0;
    }
    .features-list li {
        padding: 10pt 0;
        padding-left: 30pt;
        position: relative;
        border-bottom: 1pt solid #f3f4f6;
    }
    .features-list li:before {
        content: "✓";
        position: absolute;
        left: 0;
        color: #10b981;
        font-weight: bold;
        font-size: 14pt;
    }
    .pricing-cta {
        padding: 0 25pt 25pt 25pt;
        text-align: center;
    }
    .cta-button {
        background-color: #3b82f6;
        color: white;
        padding: 12pt 30pt;
        border-radius: 6pt;
        font-weight: bold;
        display: inline-block;
    }
</style>
<body>
    <h2>Pricing Plans</h2>
    <p style="text-align: center; margin-bottom: 30pt;">
        Choose the plan that best fits your needs
    </p>

    <div class="pricing-container">
        <div class="pricing-card">
            <div class="pricing-header">
                <div class="tier-name">Starter</div>
                <div class="tier-price">$29<span class="price-period">/mo</span></div>
            </div>
            <div class="pricing-features">
                <ul class="features-list">
                    <li>Up to 5 users</li>
                    <li>10GB storage</li>
                    <li>Email support</li>
                    <li>Basic features</li>
                    <li>Mobile app access</li>
                </ul>
            </div>
            <div class="pricing-cta">
                <div class="cta-button">Get Started</div>
            </div>
        </div>

        <div class="pricing-card">
            <div class="pricing-header">
                <div class="tier-name">Professional</div>
                <div class="tier-price">$79<span class="price-period">/mo</span></div>
            </div>
            <div class="pricing-features">
                <ul class="features-list">
                    <li>Up to 20 users</li>
                    <li>100GB storage</li>
                    <li>Priority support</li>
                    <li>Advanced features</li>
                    <li>Mobile app access</li>
                    <li>API access</li>
                    <li>Custom integrations</li>
                </ul>
            </div>
            <div class="pricing-cta">
                <div class="cta-button">Get Started</div>
            </div>
        </div>

        <div class="pricing-card">
            <div class="pricing-header">
                <div class="tier-name">Enterprise</div>
                <div class="tier-price">$199<span class="price-period">/mo</span></div>
            </div>
            <div class="pricing-features">
                <ul class="features-list">
                    <li>Unlimited users</li>
                    <li>1TB storage</li>
                    <li>24/7 phone support</li>
                    <li>All features</li>
                    <li>Mobile app access</li>
                    <li>API access</li>
                    <li>Custom integrations</li>
                    <li>Dedicated manager</li>
                    <li>SLA guarantee</li>
                </ul>
            </div>
            <div class="pricing-cta">
                <div class="cta-button">Contact Sales</div>
            </div>
        </div>
    </div>
</body>
```

### Example 11: Keep FAQ items together (continued from previous examples)

```html
<style>
    .faq-container {
        margin: 30pt 0;
    }
    .faq-item {
        break-inside: avoid;
        margin: 20pt 0;
        background-color: #f9fafb;
        border-radius: 8pt;
        overflow: hidden;
    }
    .faq-question {
        background-color: #1e3a8a;
        color: white;
        padding: 15pt 20pt;
        font-size: 14pt;
        font-weight: bold;
        display: flex;
        align-items: center;
        gap: 12pt;
    }
    .faq-q-number {
        width: 30pt;
        height: 30pt;
        background-color: white;
        color: #1e3a8a;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-weight: bold;
        flex-shrink: 0;
    }
    .faq-answer {
        padding: 20pt;
        line-height: 1.8;
        color: #374151;
    }
</style>
<body>
    <h2>Frequently Asked Questions</h2>

    <div class="faq-container">
        <div class="faq-item">
            <div class="faq-question">
                <div class="faq-q-number">1</div>
                <span>What payment methods do you accept?</span>
            </div>
            <div class="faq-answer">
                We accept all major credit cards (Visa, MasterCard, American Express),
                PayPal, and bank transfers for enterprise customers. All transactions
                are processed securely through our encrypted payment gateway.
            </div>
        </div>

        <div class="faq-item">
            <div class="faq-question">
                <div class="faq-q-number">2</div>
                <span>Can I upgrade or downgrade my plan?</span>
            </div>
            <div class="faq-answer">
                Yes, you can change your plan at any time. Upgrades take effect
                immediately, and you'll be charged the prorated difference. Downgrades
                take effect at the start of your next billing cycle, and you'll receive
                a credit for the unused portion.
            </div>
        </div>

        <div class="faq-item">
            <div class="faq-question">
                <div class="faq-q-number">3</div>
                <span>Is there a free trial available?</span>
            </div>
            <div class="faq-answer">
                Yes! We offer a 14-day free trial with full access to all Professional
                plan features. No credit card is required to start your trial. You can
                upgrade to a paid plan at any time during or after the trial period.
            </div>
        </div>
    </div>
</body>
```

### Example 12: Keep agenda items together

```html
<style>
    .agenda-container {
        margin: 30pt 0;
    }
    .agenda-item {
        break-inside: avoid;
        display: flex;
        gap: 0;
        margin: 15pt 0;
        border: 1pt solid #e5e7eb;
        border-radius: 8pt;
        overflow: hidden;
        background-color: white;
    }
    .time-slot {
        flex: 0 0 120pt;
        background-color: #1e3a8a;
        color: white;
        padding: 20pt;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }
    .time-display {
        font-size: 20pt;
        font-weight: bold;
        margin-bottom: 5pt;
    }
    .time-duration {
        font-size: 10pt;
        opacity: 0.9;
    }
    .item-content {
        flex: 1;
        padding: 20pt;
    }
    .item-title {
        font-size: 16pt;
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 8pt;
    }
    .item-description {
        font-size: 11pt;
        color: #6b7280;
        line-height: 1.6;
        margin-bottom: 10pt;
    }
    .item-presenter {
        font-size: 10pt;
        color: #2563eb;
        font-weight: bold;
    }
</style>
<body>
    <h2>Conference Agenda - Day 1</h2>
    <p>March 20, 2025 | Main Auditorium</p>

    <div class="agenda-container">
        <div class="agenda-item">
            <div class="time-slot">
                <div class="time-display">9:00 AM</div>
                <div class="time-duration">60 minutes</div>
            </div>
            <div class="item-content">
                <div class="item-title">Opening Keynote: The Future of Technology</div>
                <div class="item-description">
                    An inspiring presentation exploring emerging technology trends
                    and their impact on business innovation. Discussion will cover
                    AI, blockchain, and quantum computing.
                </div>
                <div class="item-presenter">Presenter: Dr. Sarah Chen, CTO TechCorp</div>
            </div>
        </div>

        <div class="agenda-item">
            <div class="time-slot">
                <div class="time-display">10:15 AM</div>
                <div class="time-duration">45 minutes</div>
            </div>
            <div class="item-content">
                <div class="item-title">Workshop: Practical AI Implementation</div>
                <div class="item-description">
                    Hands-on workshop where participants will learn to implement
                    basic AI models in real-world scenarios. Bring your laptop
                    for interactive exercises.
                </div>
                <div class="item-presenter">Presenter: Prof. Michael Rodriguez</div>
            </div>
        </div>

        <div class="agenda-item">
            <div class="time-slot">
                <div class="time-display">11:15 AM</div>
                <div class="time-duration">30 minutes</div>
            </div>
            <div class="item-content">
                <div class="item-title">Panel Discussion: Cybersecurity Best Practices</div>
                <div class="item-description">
                    Industry experts discuss current cybersecurity challenges and
                    share strategies for protecting digital assets. Q&A session
                    included.
                </div>
                <div class="item-presenter">Panelists: Various security experts</div>
            </div>
        </div>
    </div>
</body>
```

### Example 13: Keep recipe cards together

```html
<style>
    .recipe-collection {
        margin: 30pt 0;
    }
    .recipe-card {
        break-inside: avoid;
        border: 3pt solid #16a34a;
        border-radius: 12pt;
        margin: 25pt 0;
        background-color: #f9fafb;
        overflow: hidden;
    }
    .recipe-header {
        background: linear-gradient(135deg, #16a34a 0%, #15803d 100%);
        color: white;
        padding: 20pt;
    }
    .recipe-name {
        font-size: 22pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .recipe-meta {
        display: flex;
        gap: 20pt;
        font-size: 11pt;
        opacity: 0.95;
    }
    .recipe-body {
        padding: 20pt;
    }
    .recipe-section {
        margin: 15pt 0;
    }
    .section-title {
        font-size: 14pt;
        font-weight: bold;
        color: #15803d;
        margin-bottom: 10pt;
        padding-bottom: 5pt;
        border-bottom: 2pt solid #86efac;
    }
    .ingredients-list {
        list-style-type: disc;
        margin-left: 20pt;
        line-height: 1.8;
    }
    .instructions-list {
        list-style-type: decimal;
        margin-left: 20pt;
        line-height: 1.8;
    }
</style>
<body>
    <h2>Featured Recipes</h2>

    <div class="recipe-collection">
        <div class="recipe-card">
            <div class="recipe-header">
                <div class="recipe-name">Mediterranean Quinoa Salad</div>
                <div class="recipe-meta">
                    <span>⏱ Prep: 15 min</span>
                    <span>👥 Serves: 4</span>
                    <span>🥗 Vegetarian</span>
                </div>
            </div>
            <div class="recipe-body">
                <div class="recipe-section">
                    <div class="section-title">Ingredients</div>
                    <ul class="ingredients-list">
                        <li>1 cup quinoa, cooked and cooled</li>
                        <li>1 cucumber, diced</li>
                        <li>2 tomatoes, chopped</li>
                        <li>1/2 red onion, finely diced</li>
                        <li>1/4 cup olives, sliced</li>
                        <li>1/4 cup feta cheese, crumbled</li>
                        <li>3 tbsp olive oil</li>
                        <li>2 tbsp lemon juice</li>
                    </ul>
                </div>
                <div class="recipe-section">
                    <div class="section-title">Instructions</div>
                    <ol class="instructions-list">
                        <li>Combine cooked quinoa with vegetables in a large bowl</li>
                        <li>Whisk together olive oil and lemon juice</li>
                        <li>Pour dressing over salad and toss to combine</li>
                        <li>Top with feta cheese and olives</li>
                        <li>Chill for 30 minutes before serving</li>
                    </ol>
                </div>
            </div>
        </div>

        <div class="recipe-card">
            <div class="recipe-header">
                <div class="recipe-name">Classic Beef Tacos</div>
                <div class="recipe-meta">
                    <span>⏱ Prep: 10 min</span>
                    <span>🔥 Cook: 15 min</span>
                    <span>👥 Serves: 6</span>
                </div>
            </div>
            <div class="recipe-body">
                <div class="recipe-section">
                    <div class="section-title">Ingredients</div>
                    <ul class="ingredients-list">
                        <li>1 lb ground beef</li>
                        <li>1 packet taco seasoning</li>
                        <li>12 taco shells</li>
                        <li>1 cup shredded lettuce</li>
                        <li>1 cup shredded cheese</li>
                        <li>1 tomato, diced</li>
                        <li>Sour cream and salsa for serving</li>
                    </ul>
                </div>
                <div class="recipe-section">
                    <div class="section-title">Instructions</div>
                    <ol class="instructions-list">
                        <li>Brown ground beef in a large skillet over medium heat</li>
                        <li>Add taco seasoning and water according to package directions</li>
                        <li>Simmer for 5 minutes until sauce thickens</li>
                        <li>Warm taco shells according to package directions</li>
                        <li>Fill shells with meat and desired toppings</li>
                    </ol>
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 14: Keep timeline entries together

```html
<style>
    .timeline-entry {
        break-inside: avoid;
        position: relative;
        padding-left: 60pt;
        margin: 25pt 0;
        padding-bottom: 25pt;
        border-left: 3pt solid #e5e7eb;
    }
    .timeline-entry:last-child {
        border-left-color: transparent;
    }
    .timeline-marker {
        position: absolute;
        left: -15pt;
        top: 0;
        width: 30pt;
        height: 30pt;
        background-color: #3b82f6;
        border: 4pt solid white;
        border-radius: 50%;
        box-shadow: 0 0 0 2pt #3b82f6;
    }
    .timeline-date {
        font-size: 11pt;
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 8pt;
        text-transform: uppercase;
        letter-spacing: 0.5pt;
    }
    .timeline-title {
        font-size: 16pt;
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 8pt;
    }
    .timeline-description {
        font-size: 12pt;
        line-height: 1.6;
        color: #4b5563;
    }
    .timeline-tags {
        display: flex;
        gap: 8pt;
        margin-top: 10pt;
        flex-wrap: wrap;
    }
    .tag {
        background-color: #dbeafe;
        color: #1e40af;
        padding: 4pt 10pt;
        border-radius: 12pt;
        font-size: 9pt;
    }
</style>
<body>
    <h2>Company Milestones</h2>

    <div class="timeline-entry">
        <div class="timeline-marker"></div>
        <div class="timeline-date">January 2020</div>
        <div class="timeline-title">Company Founded</div>
        <div class="timeline-description">
            Started with a vision to revolutionize the industry. Initial team
            of five passionate individuals working from a small office.
        </div>
        <div class="timeline-tags">
            <span class="tag">Startup</span>
            <span class="tag">Foundation</span>
        </div>
    </div>

    <div class="timeline-entry">
        <div class="timeline-marker"></div>
        <div class="timeline-date">June 2020</div>
        <div class="timeline-title">First Product Launch</div>
        <div class="timeline-description">
            Released our flagship product to market. Received overwhelmingly
            positive response with 1,000 customers in the first month.
        </div>
        <div class="timeline-tags">
            <span class="tag">Product</span>
            <span class="tag">Launch</span>
            <span class="tag">Growth</span>
        </div>
    </div>

    <div class="timeline-entry">
        <div class="timeline-marker"></div>
        <div class="timeline-date">March 2021</div>
        <div class="timeline-title">Series A Funding</div>
        <div class="timeline-description">
            Secured $5M in Series A funding from leading venture capital firms.
            Expanded team to 25 employees and opened second office location.
        </div>
        <div class="timeline-tags">
            <span class="tag">Funding</span>
            <span class="tag">Expansion</span>
        </div>
    </div>
</body>
```

### Example 15: Keep notification cards together

```html
<style>
    .notification-card {
        break-inside: avoid;
        display: flex;
        gap: 15pt;
        padding: 15pt;
        margin: 15pt 0;
        border-radius: 8pt;
        border-left: 4pt solid;
    }
    .notification-info {
        background-color: #eff6ff;
        border-left-color: #3b82f6;
    }
    .notification-warning {
        background-color: #fef3c7;
        border-left-color: #f59e0b;
    }
    .notification-success {
        background-color: #f0fdf4;
        border-left-color: #10b981;
    }
    .notification-error {
        background-color: #fef2f2;
        border-left-color: #ef4444;
    }
    .notification-icon {
        flex-shrink: 0;
        width: 40pt;
        height: 40pt;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 20pt;
        font-weight: bold;
    }
    .notification-info .notification-icon {
        background-color: #3b82f6;
        color: white;
    }
    .notification-warning .notification-icon {
        background-color: #f59e0b;
        color: white;
    }
    .notification-success .notification-icon {
        background-color: #10b981;
        color: white;
    }
    .notification-error .notification-icon {
        background-color: #ef4444;
        color: white;
    }
    .notification-content {
        flex: 1;
    }
    .notification-title {
        font-size: 13pt;
        font-weight: bold;
        margin-bottom: 5pt;
    }
    .notification-message {
        font-size: 11pt;
        line-height: 1.6;
    }
    .notification-timestamp {
        font-size: 9pt;
        color: #9ca3af;
        margin-top: 5pt;
    }
</style>
<body>
    <h2>System Notifications</h2>

    <div class="notification-card notification-success">
        <div class="notification-icon">✓</div>
        <div class="notification-content">
            <div class="notification-title">Backup Completed Successfully</div>
            <div class="notification-message">
                Your data has been successfully backed up to the cloud. All files
                are now securely stored and accessible from any device.
            </div>
            <div class="notification-timestamp">2 minutes ago</div>
        </div>
    </div>

    <div class="notification-card notification-warning">
        <div class="notification-icon">!</div>
        <div class="notification-content">
            <div class="notification-title">License Renewal Required</div>
            <div class="notification-message">
                Your software license expires in 15 days. Please renew to continue
                receiving updates and support. Click here to renew now.
            </div>
            <div class="notification-timestamp">1 hour ago</div>
        </div>
    </div>

    <div class="notification-card notification-info">
        <div class="notification-icon">i</div>
        <div class="notification-content">
            <div class="notification-title">New Feature Available</div>
            <div class="notification-message">
                We've added collaborative editing to your workspace. Teams can
                now work together in real-time on shared documents.
            </div>
            <div class="notification-timestamp">3 hours ago</div>
        </div>
    </div>

    <div class="notification-card notification-error">
        <div class="notification-icon">✕</div>
        <div class="notification-content">
            <div class="notification-title">Payment Failed</div>
            <div class="notification-message">
                Unable to process your payment. Please update your payment method
                or contact support for assistance.
            </div>
            <div class="notification-timestamp">5 hours ago</div>
        </div>
    </div>
</body>
```

---

## See Also

- [break-before](/reference/cssproperties/css_prop_break-before) - Control breaks before elements
- [break-after](/reference/cssproperties/css_prop_break-after) - Control breaks after elements
- [page-break-inside](/reference/cssproperties/css_prop_page-break-inside) - Legacy CSS2 property
- [page](/reference/cssproperties/css_prop_page) - Specify named page for element
- [@page rule](/reference/css_atrules/css_atrule_page) - Define page properties and margins

---
