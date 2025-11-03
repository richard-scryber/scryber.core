---
layout: default
title: overflow-y
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# overflow-y : Vertical Overflow Property

The `overflow-y` property controls how content is handled when it exceeds the vertical boundaries of its containing element. This allows independent control of vertical overflow behavior, essential for fixed-height containers, content cards, and paginated layouts in PDF documents.

## Usage

```css
selector {
    overflow-y: value;
}
```

The overflow-y property specifically controls vertical overflow while leaving horizontal overflow behavior unaffected. This enables precise control over content clipping in the vertical direction.

---

## Supported Values

### visible (default)
Content is not clipped vertically and may extend beyond the element's top and bottom boundaries. Overflow content is visible and may overlap adjacent elements.

### hidden
Vertical overflow content is clipped and not visible. Content that extends beyond the element's height is cut off without any indication.

### scroll
Vertical overflow content is clipped, and scrolling mechanisms are provided (in interactive contexts). For PDF generation, this typically means content is clipped similar to hidden.

### auto
Vertical overflow content is clipped, and scrolling mechanisms are provided only when necessary. In PDF contexts, this typically behaves like hidden or allows content to flow to additional pages.

---

## Supported Elements

The `overflow-y` property can be applied to:
- Block elements with explicit height (`<div>`, `<section>`, `<article>`)
- Elements with defined height
- Container elements
- Text boxes and content areas
- Fixed-height cards and panels
- Any element where vertical content might exceed boundaries

---

## Notes

- The element must have an explicit height for overflow-y to take effect
- `overflow-y` affects only vertical content overflow
- Use with `overflow-x` for independent control of both axes
- Essential for creating consistent fixed-height content cards
- Hidden vertical overflow prevents content from extending beyond designated areas
- In PDF generation, vertical overflow affects pagination and content flow
- The shorthand `overflow` property sets both overflow-x and overflow-y simultaneously
- Vertical clipping occurs exactly at the boundary without fade effects
- Useful for preview sections that show only partial content
- Helps maintain consistent spacing and layout rhythm in documents

---

## Data Binding

The overflow-y property integrates with data binding to manage vertical content overflow dynamically based on content volume, container constraints, and layout preferences. This enables adaptive card heights, conditional content clipping, and responsive vertical layouts.

### Example 1: Dynamic card heights with overflow control

```html
<style>
    .content-card {
        width: 300pt;
        margin: 15pt;
        border: 2pt solid #e5e7eb;
        background-color: white;
        display: inline-block;
        vertical-align: top;
        padding: 15pt;
    }
</style>
<body>
    <!-- Apply fixed height and overflow-y based on card configuration -->
    <div class="content-card"
         style="height: {{card.fixedHeight ? card.height + 'pt' : 'auto'}};
                overflow-y: {{card.fixedHeight ? 'hidden' : 'visible'}}">
        <h3>{{card.title}}</h3>
        <p>{{card.content}}</p>
    </div>

    <!-- Conditional overflow for preview vs full view -->
    <div class="content-card"
         style="height: {{viewMode === 'preview' ? '200pt' : 'auto'}};
                overflow-y: {{viewMode === 'preview' ? 'hidden' : 'visible'}}">
        <h3>Article: {{articleTitle}}</h3>
        <div>{{articleBody}}</div>
    </div>
</body>
```

### Example 2: Adaptive overflow for dashboard panels

```html
<style>
    .dashboard-panel {
        width: 320pt;
        background-color: white;
        border: 2pt solid #e5e7eb;
        padding: 20pt;
        margin: 10pt;
        display: inline-block;
        vertical-align: top;
    }
</style>
<body>
    <!-- Dynamic panel height based on dashboard layout -->
    <div class="dashboard-panel"
         style="height: {{dashboard.panelHeight}}pt;
                overflow-y: {{dashboard.clipContent ? 'hidden' : 'visible'}}">
        <h3>{{panel.title}}</h3>
        {{#each panel.metrics}}
        <div style="margin-bottom: 10pt;">
            <div style="color: #6b7280;">{{this.label}}</div>
            <div style="font-size: 20pt; font-weight: bold; color: #2563eb;">{{this.value}}</div>
        </div>
        {{/each}}
    </div>

    <!-- Conditional overflow based on user preferences -->
    <div class="dashboard-panel"
         style="height: {{preferences.compactView ? '250pt' : '400pt'}};
                overflow-y: {{preferences.compactView ? 'hidden' : 'visible'}}">
        <h3>Activity Feed</h3>
        {{#each activities}}
        <div style="padding: 10pt; border-bottom: 1pt solid #e5e7eb;">
            <strong>{{this.time}}</strong>: {{this.description}}
        </div>
        {{/each}}
    </div>
</body>
```

### Example 3: Responsive content lists with overflow

```html
<style>
    .content-list {
        border: 2pt solid #d1d5db;
        background-color: white;
        margin: 20pt;
    }
    .list-header {
        background-color: #1f2937;
        color: white;
        padding: 15pt;
        font-size: 16pt;
        font-weight: bold;
    }
    .list-content {
        padding: 15pt;
    }
</style>
<body>
    <!-- Dynamic list height based on display mode -->
    <div class="content-list">
        <div class="list-header">{{listTitle}}</div>
        <div class="list-content"
             style="height: {{displayMode === 'compact' ? '300pt' : (displayMode === 'full' ? 'auto' : '450pt')}};
                    overflow-y: {{displayMode === 'full' ? 'visible' : 'hidden'}}">
            {{#each items}}
            <div style="padding: 12pt; margin-bottom: 10pt; background-color: #f9fafb; border: 1pt solid #e5e7eb;">
                <strong>{{this.title}}</strong>
                <p style="margin: 5pt 0 0 0;">{{this.description}}</p>
            </div>
            {{/each}}
        </div>
    </div>

    <!-- Conditional overflow for different page sizes -->
    <div style="width: 500pt;
                height: {{pageSize === 'letter' ? '350pt' : '420pt'}};
                overflow-y: {{limitContentHeight ? 'hidden' : 'visible'}};
                border: 2pt solid #e5e7eb;
                padding: 20pt;">
        <h2>{{contentTitle}}</h2>
        <div>{{contentBody}}</div>
    </div>
</body>
```

---

## Examples

### Example 1: Fixed-height content box

```html
<style>
    .content-box {
        width: 400pt;
        height: 150pt;
        overflow-y: hidden;
        padding: 15pt;
        border: 2pt solid #2563eb;
        background-color: #dbeafe;
        margin-bottom: 20pt;
    }
    .content-box h3 {
        margin: 0 0 10pt 0;
        color: #1e40af;
    }
    .content-box p {
        margin: 0 0 10pt 0;
        line-height: 1.6;
    }
</style>
<body>
    <div class="content-box">
        <h3>Article Preview</h3>
        <p>This content box has a fixed height of 150pt with overflow-y: hidden.
        Any content that extends beyond this height will be clipped.</p>
        <p>Additional paragraphs may be partially or completely hidden if there
        is too much content to fit within the designated space.</p>
        <p>This paragraph might not be visible at all depending on the amount
        of content above it.</p>
        <p>More content continues here but is likely clipped...</p>
    </div>
    <p>Content below the box appears in its normal position without overlap.</p>
</body>
```

### Example 2: Comparison of overflow-y values

```html
<style>
    .demo-container {
        display: inline-block;
        width: 250pt;
        height: 120pt;
        margin: 10pt;
        padding: 15pt;
        border: 2pt solid #1f2937;
        vertical-align: top;
    }
    .overflow-visible {
        overflow-y: visible;
        background-color: #dbeafe;
    }
    .overflow-hidden {
        overflow-y: hidden;
        background-color: #dcfce7;
    }
    .demo-label {
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 8pt;
        display: block;
    }
</style>
<body>
    <h2>Overflow-Y Property Comparison</h2>
    <div style="padding: 20pt;">
        <div class="demo-container overflow-visible">
            <span class="demo-label">overflow-y: visible</span>
            <p style="margin: 5pt 0;">This content may extend beyond the bottom
            boundary of the container. Multiple paragraphs of text will overflow
            and potentially overlap content below.</p>
            <p style="margin: 5pt 0;">Additional content continues here and may
            extend beyond the fixed height.</p>
        </div>

        <div class="demo-container overflow-hidden">
            <span class="demo-label">overflow-y: hidden</span>
            <p style="margin: 5pt 0;">This content is clipped at the bottom
            boundary. Any text that extends beyond the 120pt height is cut off
            and not visible in the output.</p>
            <p style="margin: 5pt 0;">This paragraph might be partially or
            completely hidden.</p>
        </div>
    </div>
</body>
```

### Example 3: Article preview cards

```html
<style>
    .article-grid {
        padding: 20pt;
    }
    .article-card {
        width: 300pt;
        margin: 15pt;
        border: 2pt solid #e5e7eb;
        background-color: white;
        display: inline-block;
        vertical-align: top;
    }
    .card-image {
        width: 100%;
        height: 150pt;
        background-color: #dbeafe;
    }
    .card-content {
        padding: 15pt;
    }
    .card-title {
        font-size: 16pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 10pt;
    }
    .card-excerpt {
        height: 80pt;
        overflow-y: hidden;
        font-size: 11pt;
        line-height: 1.6;
        color: #6b7280;
        margin-bottom: 10pt;
    }
    .read-more {
        display: inline-block;
        padding: 8pt 15pt;
        background-color: #2563eb;
        color: white;
        font-weight: bold;
        font-size: 10pt;
    }
</style>
<body>
    <div class="article-grid">
        <div class="article-card">
            <div class="card-image"></div>
            <div class="card-content">
                <div class="card-title">Understanding PDF Layout</div>
                <div class="card-excerpt">
                    Learn the fundamentals of PDF document generation and layout
                    control. This comprehensive guide covers everything from basic
                    concepts to advanced techniques for creating professional
                    documents. Discover how overflow properties help maintain
                    consistent card heights regardless of content length.
                </div>
                <div class="read-more">Read More</div>
            </div>
        </div>

        <div class="article-card">
            <div class="card-image"></div>
            <div class="card-content">
                <div class="card-title">Advanced Techniques</div>
                <div class="card-excerpt">
                    Short preview text.
                </div>
                <div class="read-more">Read More</div>
            </div>
        </div>
    </div>
</body>
```

### Example 4: Dashboard metrics panel

```html
<style>
    .dashboard {
        padding: 30pt;
        background-color: #f3f4f6;
    }
    .metrics-panel {
        width: 320pt;
        height: 250pt;
        overflow-y: hidden;
        background-color: white;
        border: 2pt solid #e5e7eb;
        padding: 20pt;
        margin: 10pt;
        display: inline-block;
        vertical-align: top;
    }
    .panel-title {
        font-size: 18pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 20pt;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #e5e7eb;
    }
    .metric-item {
        margin-bottom: 15pt;
        padding: 12pt;
        background-color: #f9fafb;
        border-left: 4pt solid #2563eb;
    }
    .metric-label {
        font-size: 10pt;
        color: #6b7280;
        text-transform: uppercase;
        margin-bottom: 5pt;
    }
    .metric-value {
        font-size: 24pt;
        font-weight: bold;
        color: #1f2937;
    }
</style>
<body>
    <div class="dashboard">
        <div class="metrics-panel">
            <div class="panel-title">Sales Metrics</div>
            <div class="metric-item">
                <div class="metric-label">Total Revenue</div>
                <div class="metric-value">$487,250</div>
            </div>
            <div class="metric-item">
                <div class="metric-label">Orders</div>
                <div class="metric-value">1,842</div>
            </div>
            <div class="metric-item">
                <div class="metric-label">Avg Order Value</div>
                <div class="metric-value">$264.50</div>
            </div>
            <div class="metric-item">
                <div class="metric-label">Growth Rate</div>
                <div class="metric-value">+23.5%</div>
            </div>
            <div class="metric-item">
                <div class="metric-label">Customer Satisfaction</div>
                <div class="metric-value">4.8/5.0</div>
            </div>
        </div>
    </div>
</body>
```

### Example 5: News feed with limited items

```html
<style>
    .news-feed {
        width: 450pt;
        height: 400pt;
        overflow-y: hidden;
        border: 2pt solid #d1d5db;
        background-color: white;
        margin: 20pt;
    }
    .feed-header {
        background-color: #1f2937;
        color: white;
        padding: 15pt;
        font-size: 16pt;
        font-weight: bold;
    }
    .feed-content {
        padding: 15pt;
    }
    .news-item {
        padding: 15pt;
        margin-bottom: 10pt;
        border-bottom: 1pt solid #e5e7eb;
    }
    .news-time {
        font-size: 9pt;
        color: #6b7280;
        margin-bottom: 5pt;
    }
    .news-headline {
        font-size: 13pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 5pt;
    }
    .news-summary {
        font-size: 10pt;
        line-height: 1.5;
        color: #4b5563;
    }
</style>
<body>
    <div class="news-feed">
        <div class="feed-header">Latest Updates</div>
        <div class="feed-content">
            <div class="news-item">
                <div class="news-time">2 hours ago</div>
                <div class="news-headline">New Product Launch Announced</div>
                <div class="news-summary">
                    Company reveals innovative features for upcoming release.
                </div>
            </div>
            <div class="news-item">
                <div class="news-time">5 hours ago</div>
                <div class="news-headline">Q4 Results Exceed Expectations</div>
                <div class="news-summary">
                    Financial performance shows strong growth across all sectors.
                </div>
            </div>
            <div class="news-item">
                <div class="news-time">8 hours ago</div>
                <div class="news-headline">Partnership Agreement Signed</div>
                <div class="news-summary">
                    Strategic alliance to expand market presence in new regions.
                </div>
            </div>
            <div class="news-item">
                <div class="news-time">12 hours ago</div>
                <div class="news-headline">Industry Award Recognition</div>
                <div class="news-summary">
                    Company honored for innovation and excellence.
                </div>
            </div>
            <div class="news-item">
                <div class="news-time">1 day ago</div>
                <div class="news-headline">Additional Updates</div>
                <div class="news-summary">
                    This item might be partially or fully clipped...
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 6: Sidebar navigation with overflow

```html
<style>
    .layout-container {
        padding: 20pt;
    }
    .sidebar-nav {
        width: 200pt;
        height: 350pt;
        overflow-y: hidden;
        float: left;
        background-color: #f3f4f6;
        border: 2pt solid #d1d5db;
        margin-right: 20pt;
    }
    .nav-header {
        background-color: #1e3a8a;
        color: white;
        padding: 15pt;
        font-weight: bold;
        font-size: 14pt;
    }
    .nav-section {
        padding: 15pt;
    }
    .nav-category {
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 8pt;
        margin-top: 15pt;
        font-size: 12pt;
    }
    .nav-item {
        padding: 8pt 10pt;
        margin-bottom: 5pt;
        background-color: white;
        border: 1pt solid #d1d5db;
        font-size: 10pt;
    }
    .nav-item.active {
        background-color: #dbeafe;
        border-color: #2563eb;
        font-weight: bold;
    }
    .main-content {
        margin-left: 240pt;
    }
</style>
<body>
    <div class="layout-container">
        <div class="sidebar-nav">
            <div class="nav-header">Navigation</div>
            <div class="nav-section">
                <div class="nav-category">Dashboard</div>
                <div class="nav-item active">Overview</div>
                <div class="nav-item">Analytics</div>
                <div class="nav-item">Reports</div>

                <div class="nav-category">Products</div>
                <div class="nav-item">All Products</div>
                <div class="nav-item">Categories</div>
                <div class="nav-item">Inventory</div>
                <div class="nav-item">Pricing</div>

                <div class="nav-category">Customers</div>
                <div class="nav-item">Customer List</div>
                <div class="nav-item">Orders</div>
                <div class="nav-item">Support</div>

                <div class="nav-category">Settings</div>
                <div class="nav-item">Account</div>
                <div class="nav-item">Preferences</div>
                <div class="nav-item">Security</div>
            </div>
        </div>
        <div class="main-content">
            <h1>Main Content Area</h1>
            <p>The navigation sidebar has fixed height with overflow-y: hidden.</p>
        </div>
    </div>
</body>
```

### Example 7: Event listing with constrained height

```html
<style>
    .events-container {
        width: 500pt;
        margin: 30pt auto;
        border: 3pt solid #1e3a8a;
        background-color: white;
    }
    .events-header {
        background-color: #1e3a8a;
        color: white;
        padding: 20pt;
        text-align: center;
    }
    .events-header h1 {
        margin: 0;
        font-size: 24pt;
    }
    .events-list {
        height: 350pt;
        overflow-y: hidden;
        padding: 20pt;
    }
    .event-item {
        margin-bottom: 20pt;
        padding: 15pt;
        background-color: #f9fafb;
        border-left: 5pt solid #2563eb;
    }
    .event-date {
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 8pt;
        font-size: 12pt;
    }
    .event-title {
        font-size: 14pt;
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 5pt;
    }
    .event-description {
        font-size: 10pt;
        line-height: 1.5;
        color: #6b7280;
    }
</style>
<body>
    <div class="events-container">
        <div class="events-header">
            <h1>Upcoming Events</h1>
            <p style="margin: 5pt 0 0 0;">October - December 2025</p>
        </div>
        <div class="events-list">
            <div class="event-item">
                <div class="event-date">October 20, 2025</div>
                <div class="event-title">Annual Conference</div>
                <div class="event-description">
                    Join us for our flagship annual conference featuring keynote
                    speakers and networking opportunities.
                </div>
            </div>
            <div class="event-item">
                <div class="event-date">November 5, 2025</div>
                <div class="event-title">Technical Workshop</div>
                <div class="event-description">
                    Hands-on workshop covering advanced PDF generation techniques.
                </div>
            </div>
            <div class="event-item">
                <div class="event-date">November 18, 2025</div>
                <div class="event-title">Webinar Series</div>
                <div class="event-description">
                    Monthly webinar covering best practices and new features.
                </div>
            </div>
            <div class="event-item">
                <div class="event-date">December 10, 2025</div>
                <div class="event-title">Year-End Celebration</div>
                <div class="event-description">
                    Celebrate achievements and preview upcoming plans.
                </div>
            </div>
            <div class="event-item">
                <div class="event-date">December 15, 2025</div>
                <div class="event-title">Planning Session</div>
                <div class="event-description">
                    This event might be clipped due to overflow constraints...
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 8: Comment section with limited display

```html
<style>
    .comments-section {
        width: 450pt;
        margin: 20pt;
        border: 2pt solid #e5e7eb;
        background-color: white;
    }
    .comments-header {
        padding: 15pt;
        background-color: #f3f4f6;
        border-bottom: 2pt solid #e5e7eb;
        font-size: 16pt;
        font-weight: bold;
        color: #1f2937;
    }
    .comments-list {
        height: 300pt;
        overflow-y: hidden;
        padding: 15pt;
    }
    .comment {
        margin-bottom: 15pt;
        padding: 12pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        border-radius: 6pt;
    }
    .comment-author {
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 5pt;
        font-size: 11pt;
    }
    .comment-time {
        font-size: 9pt;
        color: #9ca3af;
        margin-bottom: 8pt;
    }
    .comment-text {
        font-size: 10pt;
        line-height: 1.6;
        color: #4b5563;
    }
</style>
<body>
    <div class="comments-section">
        <div class="comments-header">Comments (Showing Latest)</div>
        <div class="comments-list">
            <div class="comment">
                <div class="comment-author">Sarah Johnson</div>
                <div class="comment-time">2 hours ago</div>
                <div class="comment-text">
                    This is an excellent example of overflow control. The fixed
                    height ensures consistent layout across different pages.
                </div>
            </div>
            <div class="comment">
                <div class="comment-author">Michael Chen</div>
                <div class="comment-time">4 hours ago</div>
                <div class="comment-text">
                    I appreciate the detailed documentation. Very helpful for
                    understanding PDF layout techniques.
                </div>
            </div>
            <div class="comment">
                <div class="comment-author">Emma Davis</div>
                <div class="comment-time">6 hours ago</div>
                <div class="comment-text">
                    Great work on this feature! The examples really help clarify
                    how to use overflow-y effectively.
                </div>
            </div>
            <div class="comment">
                <div class="comment-author">James Wilson</div>
                <div class="comment-time">8 hours ago</div>
                <div class="comment-text">
                    The comparison examples are particularly useful for seeing
                    the differences between values.
                </div>
            </div>
            <div class="comment">
                <div class="comment-author">Lisa Anderson</div>
                <div class="comment-time">12 hours ago</div>
                <div class="comment-text">
                    This comment might be partially or fully hidden due to the
                    overflow constraints applied to the container.
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 9: Product features list with overflow

```html
<style>
    .product-showcase {
        width: 600pt;
        margin: 30pt auto;
        border: 3pt solid #e5e7eb;
        background-color: white;
    }
    .product-header {
        background-color: #1e3a8a;
        color: white;
        padding: 25pt;
        text-align: center;
    }
    .product-title {
        font-size: 28pt;
        font-weight: bold;
        margin: 0 0 10pt 0;
    }
    .product-tagline {
        font-size: 14pt;
        margin: 0;
    }
    .features-container {
        display: table;
        width: 100%;
    }
    .features-column {
        display: table-cell;
        width: 50%;
        vertical-align: top;
    }
    .features-list {
        height: 280pt;
        overflow-y: hidden;
        padding: 20pt;
    }
    .feature {
        margin-bottom: 15pt;
        padding-left: 25pt;
        position: relative;
    }
    .feature:before {
        content: "✓";
        position: absolute;
        left: 0;
        color: #16a34a;
        font-weight: bold;
        font-size: 14pt;
    }
    .feature-title {
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 3pt;
    }
    .feature-desc {
        font-size: 10pt;
        color: #6b7280;
        line-height: 1.4;
    }
</style>
<body>
    <div class="product-showcase">
        <div class="product-header">
            <div class="product-title">Premium Package</div>
            <div class="product-tagline">Everything you need and more</div>
        </div>
        <div class="features-container">
            <div class="features-column">
                <div class="features-list">
                    <div class="feature">
                        <div class="feature-title">Advanced PDF Generation</div>
                        <div class="feature-desc">Create professional documents</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">Custom Templates</div>
                        <div class="feature-desc">Fully customizable layouts</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">Batch Processing</div>
                        <div class="feature-desc">Generate multiple documents</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">Cloud Storage</div>
                        <div class="feature-desc">Secure document storage</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">API Access</div>
                        <div class="feature-desc">Integrate with your systems</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">Analytics Dashboard</div>
                        <div class="feature-desc">Track usage and performance</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">24/7 Support</div>
                        <div class="feature-desc">Always available to help</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">Priority Updates</div>
                        <div class="feature-desc">Get features first (may be clipped)</div>
                    </div>
                </div>
            </div>
            <div class="features-column">
                <div class="features-list">
                    <div class="feature">
                        <div class="feature-title">Team Collaboration</div>
                        <div class="feature-desc">Work together seamlessly</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">Version Control</div>
                        <div class="feature-desc">Track document changes</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">White Label Options</div>
                        <div class="feature-desc">Brand as your own</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">Encryption</div>
                        <div class="feature-desc">Enterprise-grade security</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">Compliance Tools</div>
                        <div class="feature-desc">Meet industry standards</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">Workflow Automation</div>
                        <div class="feature-desc">Streamline processes</div>
                    </div>
                    <div class="feature">
                        <div class="feature-title">Multi-language Support</div>
                        <div class="feature-desc">Global compatibility</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
```

### Example 10: Timeline with vertical constraints

```html
<style>
    .timeline-container {
        width: 550pt;
        height: 450pt;
        overflow-y: hidden;
        margin: 30pt auto;
        padding: 25pt;
        border: 2pt solid #d1d5db;
        background-color: white;
    }
    .timeline-header {
        font-size: 24pt;
        font-weight: bold;
        color: #1e3a8a;
        margin-bottom: 25pt;
        text-align: center;
        padding-bottom: 15pt;
        border-bottom: 3pt solid #1e3a8a;
    }
    .timeline-item {
        margin-bottom: 25pt;
        padding-left: 40pt;
        position: relative;
    }
    .timeline-marker {
        position: absolute;
        left: 0;
        width: 20pt;
        height: 20pt;
        background-color: #2563eb;
        border-radius: 10pt;
        border: 3pt solid white;
        box-shadow: 0 0 0 2pt #2563eb;
    }
    .timeline-date {
        font-weight: bold;
        color: #2563eb;
        margin-bottom: 5pt;
        font-size: 12pt;
    }
    .timeline-title {
        font-size: 16pt;
        font-weight: bold;
        color: #1f2937;
        margin-bottom: 8pt;
    }
    .timeline-description {
        font-size: 11pt;
        line-height: 1.6;
        color: #6b7280;
    }
</style>
<body>
    <div class="timeline-container">
        <div class="timeline-header">Company Milestones</div>
        <div class="timeline-item">
            <div class="timeline-marker"></div>
            <div class="timeline-date">January 2025</div>
            <div class="timeline-title">Company Founded</div>
            <div class="timeline-description">
                Established with a vision to revolutionize document generation
                technology and provide innovative solutions.
            </div>
        </div>
        <div class="timeline-item">
            <div class="timeline-marker"></div>
            <div class="timeline-date">March 2025</div>
            <div class="timeline-title">First Product Launch</div>
            <div class="timeline-description">
                Released initial version with core PDF generation capabilities
                and basic layout features.
            </div>
        </div>
        <div class="timeline-item">
            <div class="timeline-marker"></div>
            <div class="timeline-date">June 2025</div>
            <div class="timeline-title">Major Update Released</div>
            <div class="timeline-description">
                Added advanced layout controls including overflow management
                and column support.
            </div>
        </div>
        <div class="timeline-item">
            <div class="timeline-marker"></div>
            <div class="timeline-date">August 2025</div>
            <div class="timeline-title">1000 Customers Milestone</div>
            <div class="timeline-description">
                Reached significant customer base demonstrating market fit.
            </div>
        </div>
        <div class="timeline-item">
            <div class="timeline-marker"></div>
            <div class="timeline-date">October 2025</div>
            <div class="timeline-title">Enterprise Features Added</div>
            <div class="timeline-description">
                This milestone might be partially hidden due to vertical
                overflow constraints in the timeline container.
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [overflow](/reference/cssproperties/css_prop_overflow) - Control both horizontal and vertical overflow
- [overflow-x](/reference/cssproperties/css_prop_overflow-x) - Control horizontal overflow behavior
- [height](/reference/cssproperties/css_prop_height) - Set element height
- [max-height](/reference/cssproperties/css_prop_max-height) - Set maximum height constraint
- [min-height](/reference/cssproperties/css_prop_min-height) - Set minimum height constraint
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
