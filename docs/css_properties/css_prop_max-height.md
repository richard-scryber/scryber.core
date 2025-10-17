---
layout: default
title: max-height
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# max-height : Maximum Height Property

The `max-height` property sets the maximum vertical dimension of an element in PDF documents. It ensures that an element will never be taller than the specified value, even if its content would require more space. This property is essential for controlling layout overflow, maintaining consistent page breaks, and creating fixed-height sections that adapt to varying content amounts.

## Usage

```css
selector {
    max-height: value;
}
```

The max-height property accepts a single value that defines the largest allowed height for the element. If the calculated or specified height would be larger, the max-height value is used instead.

---

## Supported Values

### Length Units
- Points: `200pt`, `400pt`, `600pt`
- Pixels: `200px`, `400px`, `600px`
- Inches: `3in`, `6in`, `8in`
- Centimeters: `10cm`, `15cm`, `20cm`
- Millimeters: `100mm`, `150mm`, `200mm`
- Ems: `20em`, `30em`, `40em`
- Percentage: `50%`, `80%`, `100%` (relative to parent height)

### Special Values
- `none` - No maximum height constraint (default)

---

## Supported Elements

The `max-height` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Images (`<img>`)
- Tables (`<table>`)
- Table cells (`<td>`, `<th>`)
- Form elements (`<textarea>`)
- All elements with `display: block`, `display: inline-block`, or `display: table`

---

## Notes

- `max-height` overrides `height` if height is larger than max-height
- When both `min-height` and `max-height` are specified, and min-height is larger, min-height wins
- Percentage values require the parent element to have an explicit height
- Essential for preventing content from extending beyond page boundaries in PDF documents
- Works with `overflow` property to control how content that exceeds max-height is handled
- Does not affect inline elements unless they are `display: inline-block`
- In PDF generation, max-height helps control page breaks and prevent awkward content splits
- Particularly useful for constraining image heights while maintaining aspect ratios
- Helps maintain consistent layouts when content length varies significantly
- Works in conjunction with `height` and `min-height` to create flexible yet constrained layouts
- Standard A4 page height is approximately 842pt (minus margins)

---

## Data Binding

The max-height property supports dynamic value binding through template expressions, allowing maximum height constraints to be set from data sources at runtime.

### Example 1: Images with configurable maximum heights

```html
<style>
    .image-container {
        padding: 20pt;
    }
    .responsive-image {
        width: auto;
        display: block;
        margin: 0 auto;
        border: 2pt solid #e5e7eb;
    }
</style>
<body>
    <div class="image-container">
        <img class="responsive-image"
             src="{{image.url}}"
             style="max-height: {{image.displaySize === 'large' ? '400pt' : '200pt'}}"
             alt="{{image.caption}}" />
    </div>
</body>
```

### Example 2: Content sections with data-driven height limits

```html
<style>
    .content-section {
        padding: 25pt;
        margin-bottom: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        overflow: hidden;
    }
    .section-title {
        margin: 0 0 15pt 0;
        font-size: 18pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="content-section" style="max-height: {{section.preview ? '250pt' : '500pt'}}">
        <h2 class="section-title">{{section.title}}</h2>
        <div>{{section.content}}</div>
    </div>
</body>
```

### Example 3: Cards with variable maximum heights

```html
<style>
    .card-grid {
        padding: 20pt;
    }
    .card {
        width: 280pt;
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
        overflow: hidden;
    }
    .card-title {
        margin: 0 0 10pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="card-grid">
        <div class="card" style="max-height: {{card.expanded ? '500pt' : '300pt'}}">
            <h3 class="card-title">{{card.title}}</h3>
            <p>{{card.description}}</p>
        </div>
    </div>
</body>
```

### Example 4: Dashboard panels with configurable height constraints

```html
<style>
    .dashboard {
        padding: 20pt;
    }
    .metric-panel {
        width: 280pt;
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
        overflow: hidden;
    }
    .panel-header {
        margin-bottom: 15pt;
        font-size: 14pt;
        font-weight: bold;
    }
    .metric-value {
        font-size: 36pt;
        font-weight: bold;
        color: #1e40af;
    }
</style>
<body>
    <div class="dashboard">
        <div class="metric-panel" style="max-height: {{panel.compact ? '180pt' : '250pt'}}">
            <div class="panel-header">{{panel.title}}</div>
            <div class="metric-value">{{panel.value}}</div>
            <p>{{panel.description}}</p>
        </div>
    </div>
</body>
```

---

## Examples

### Example 1: Image with maximum height

```html
<style>
    .image-container {
        padding: 20pt;
    }
    .hero-image {
        max-height: 300pt;
        width: auto;
        display: block;
        margin: 0 auto;
    }
    .thumbnail {
        max-height: 100pt;
        width: auto;
        border: 2pt solid #e5e7eb;
        margin: 10pt;
    }
    .logo {
        max-height: 60pt;
        width: auto;
    }
</style>
<body>
    <div class="image-container">
        <img class="logo" src="company-logo.png" alt="Logo" />
        <img class="hero-image" src="banner.jpg" alt="Banner" />
        <img class="thumbnail" src="thumb1.jpg" alt="Thumbnail" />
    </div>
</body>
```

### Example 2: Content section with maximum height

```html
<style>
    .content-section {
        max-height: 400pt;
        padding: 25pt;
        margin-bottom: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        overflow: hidden;
    }
    .section-title {
        margin: 0 0 15pt 0;
        font-size: 18pt;
        font-weight: bold;
    }
    .section-content {
        line-height: 1.6;
    }
</style>
<body>
    <div class="content-section">
        <h2 class="section-title">Summary</h2>
        <div class="section-content">
            <p>Content is constrained to maximum 400pt height.</p>
            <p>Excess content will be hidden or scrollable depending on overflow setting.</p>
        </div>
    </div>
</body>
```

### Example 3: Card with maximum height

```html
<style>
    .card-grid {
        padding: 20pt;
    }
    .card {
        max-height: 350pt;
        width: 280pt;
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
        overflow: hidden;
    }
    .card-image {
        height: 150pt;
        width: 100%;
        background-color: #f3f4f6;
        margin-bottom: 15pt;
    }
    .card-title {
        margin: 0 0 10pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
    .card-description {
        color: #6b7280;
        line-height: 1.5;
    }
</style>
<body>
    <div class="card-grid">
        <div class="card">
            <div class="card-image"></div>
            <h3 class="card-title">Card Title</h3>
            <p class="card-description">Brief description.</p>
        </div>
        <div class="card">
            <div class="card-image"></div>
            <h3 class="card-title">Extended Card</h3>
            <p class="card-description">This card has much more content that would normally make it taller, but the max-height constraint ensures all cards remain the same height.</p>
        </div>
    </div>
</body>
```

### Example 4: Sidebar with maximum height

```html
<style>
    .layout-container {
        padding: 20pt;
    }
    .sidebar {
        max-height: 600pt;
        width: 200pt;
        float: left;
        padding: 20pt;
        background-color: #f3f4f6;
        border-right: 2pt solid #e5e7eb;
        overflow-y: auto;
        box-sizing: border-box;
    }
    .sidebar-section {
        margin-bottom: 20pt;
    }
    .sidebar-title {
        margin: 0 0 10pt 0;
        font-size: 14pt;
        font-weight: bold;
    }
    .content-area {
        margin-left: 240pt;
        padding: 20pt;
    }
</style>
<body>
    <div class="layout-container">
        <div class="sidebar">
            <div class="sidebar-section">
                <h3 class="sidebar-title">Navigation</h3>
                <ul>
                    <li>Dashboard</li>
                    <li>Reports</li>
                    <li>Settings</li>
                </ul>
            </div>
            <div class="sidebar-section">
                <h3 class="sidebar-title">Recent</h3>
                <p>Recent items list...</p>
            </div>
        </div>
        <div class="content-area">
            <h1>Main Content</h1>
            <p>Content area without height constraint.</p>
        </div>
    </div>
</body>
```

### Example 5: Textarea with maximum height

```html
<style>
    .form-container {
        width: 450pt;
        padding: 25pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .form-group {
        margin-bottom: 15pt;
    }
    .form-label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
    }
    .form-textarea {
        max-height: 150pt;
        width: 100%;
        padding: 10pt;
        border: 1pt solid #d1d5db;
        resize: vertical;
        overflow-y: auto;
        box-sizing: border-box;
    }
    .form-textarea-short {
        max-height: 80pt;
    }
</style>
<body>
    <div class="form-container">
        <div class="form-group">
            <label class="form-label">Brief Comment</label>
            <textarea class="form-textarea form-textarea-short"></textarea>
        </div>
        <div class="form-group">
            <label class="form-label">Detailed Description</label>
            <textarea class="form-textarea"></textarea>
        </div>
    </div>
</body>
```

### Example 6: Table with maximum height

```html
<style>
    .table-wrapper {
        max-height: 400pt;
        overflow-y: auto;
        border: 1pt solid #d1d5db;
    }
    .data-table {
        width: 100%;
        border-collapse: collapse;
    }
    .data-table th,
    .data-table td {
        padding: 10pt;
        border: 1pt solid #d1d5db;
        text-align: left;
    }
    .data-table th {
        background-color: #1f2937;
        color: white;
        position: sticky;
        top: 0;
    }
</style>
<body>
    <div class="table-wrapper">
        <table class="data-table">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>001</td>
                    <td>Item One</td>
                    <td>Active</td>
                </tr>
                <tr>
                    <td>002</td>
                    <td>Item Two</td>
                    <td>Pending</td>
                </tr>
                <!-- More rows... -->
            </tbody>
        </table>
    </div>
</body>
```

### Example 7: Alert box with maximum height

```html
<style>
    .alerts-container {
        padding: 20pt;
    }
    .alert {
        max-height: 100pt;
        padding: 15pt;
        margin-bottom: 12pt;
        border-radius: 4pt;
        border-left: 4pt solid;
        overflow-y: auto;
    }
    .alert-info {
        background-color: #dbeafe;
        border-left-color: #3b82f6;
    }
    .alert-warning {
        background-color: #fef3c7;
        border-left-color: #f59e0b;
    }
    .alert-title {
        font-weight: bold;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="alerts-container">
        <div class="alert alert-info">
            <div class="alert-title">Information</div>
            <p>Brief message.</p>
        </div>
        <div class="alert alert-warning">
            <div class="alert-title">Warning</div>
            <p>This is a longer warning message that contains multiple sentences and detailed information about the warning condition. The max-height constraint ensures it doesn't take up too much space.</p>
        </div>
    </div>
</body>
```

### Example 8: Dashboard panels with maximum height

```html
<style>
    .dashboard {
        padding: 20pt;
    }
    .metric-panel {
        max-height: 200pt;
        width: 280pt;
        padding: 20pt;
        margin: 10pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        float: left;
        box-sizing: border-box;
        overflow: hidden;
    }
    .panel-header {
        margin-bottom: 15pt;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #e5e7eb;
        font-size: 14pt;
        font-weight: bold;
    }
    .metric-value {
        font-size: 36pt;
        font-weight: bold;
        color: #1e40af;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="dashboard">
        <div class="metric-panel">
            <div class="panel-header">Revenue</div>
            <div class="metric-value">$52K</div>
            <p>+12% from last month</p>
        </div>
        <div class="metric-panel">
            <div class="panel-header">Activity Log</div>
            <p>Recent activity entries...</p>
            <p>Multiple log entries that might exceed the maximum height...</p>
        </div>
    </div>
</body>
```

### Example 9: Modal dialog with maximum height

```html
<style>
    .modal-overlay {
        padding: 40pt;
    }
    .modal {
        max-height: 500pt;
        max-width: 550pt;
        margin: 0 auto;
        background-color: white;
        border-radius: 8pt;
        overflow: hidden;
        box-sizing: border-box;
    }
    .modal-header {
        padding: 20pt;
        background-color: #1e40af;
        color: white;
        font-size: 20pt;
        font-weight: bold;
    }
    .modal-body {
        max-height: 350pt;
        padding: 25pt;
        overflow-y: auto;
        line-height: 1.6;
    }
    .modal-footer {
        padding: 15pt 20pt;
        border-top: 1pt solid #e5e7eb;
        text-align: right;
    }
</style>
<body>
    <div class="modal-overlay">
        <div class="modal">
            <div class="modal-header">Terms and Conditions</div>
            <div class="modal-body">
                <p>Lengthy terms and conditions text...</p>
                <p>Multiple paragraphs of legal text...</p>
            </div>
            <div class="modal-footer">
                <button>Accept</button>
            </div>
        </div>
    </div>
</body>
```

### Example 10: Product listing with maximum height

```html
<style>
    .product-list {
        padding: 20pt;
    }
    .product-item {
        max-height: 180pt;
        padding: 15pt;
        margin-bottom: 15pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        overflow: hidden;
        display: flex;
    }
    .product-image {
        width: 120pt;
        height: 120pt;
        flex-shrink: 0;
        background-color: #f3f4f6;
        margin-right: 15pt;
    }
    .product-details {
        flex: 1;
    }
    .product-name {
        margin: 0 0 8pt 0;
        font-size: 16pt;
        font-weight: bold;
    }
    .product-description {
        margin: 0;
        color: #6b7280;
        line-height: 1.5;
    }
</style>
<body>
    <div class="product-list">
        <div class="product-item">
            <div class="product-image"></div>
            <div class="product-details">
                <h3 class="product-name">Widget A</h3>
                <p class="product-description">Brief description.</p>
            </div>
        </div>
        <div class="product-item">
            <div class="product-image"></div>
            <div class="product-details">
                <h3 class="product-name">Widget B</h3>
                <p class="product-description">This is a much longer product description that includes extensive details about features, specifications, and benefits. The max-height ensures consistent item sizing.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 11: Comment section with maximum height

```html
<style>
    .comments-section {
        max-height: 400pt;
        padding: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        overflow-y: auto;
    }
    .comment {
        padding: 15pt;
        margin-bottom: 12pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
    }
    .comment-author {
        margin-bottom: 8pt;
        font-weight: bold;
        color: #1f2937;
    }
    .comment-text {
        margin: 0;
        line-height: 1.5;
        color: #4b5563;
    }
    .comment-date {
        margin-top: 8pt;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="comments-section">
        <div class="comment">
            <div class="comment-author">John Smith</div>
            <p class="comment-text">Great article! Very informative.</p>
            <div class="comment-date">2 hours ago</div>
        </div>
        <div class="comment">
            <div class="comment-author">Jane Doe</div>
            <p class="comment-text">Thanks for sharing this detailed analysis.</p>
            <div class="comment-date">1 day ago</div>
        </div>
    </div>
</body>
```

### Example 12: News feed with maximum height

```html
<style>
    .news-feed {
        max-height: 500pt;
        width: 350pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        overflow-y: auto;
    }
    .feed-header {
        padding: 15pt;
        background-color: #1e40af;
        color: white;
        font-size: 16pt;
        font-weight: bold;
        position: sticky;
        top: 0;
    }
    .feed-item {
        padding: 15pt;
        border-bottom: 1pt solid #e5e7eb;
    }
    .feed-title {
        margin: 0 0 8pt 0;
        font-size: 14pt;
        font-weight: bold;
    }
    .feed-excerpt {
        margin: 0;
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="news-feed">
        <div class="feed-header">Latest News</div>
        <div class="feed-item">
            <h3 class="feed-title">News Article One</h3>
            <p class="feed-excerpt">Brief excerpt of the news article...</p>
        </div>
        <div class="feed-item">
            <h3 class="feed-title">News Article Two</h3>
            <p class="feed-excerpt">Another news excerpt...</p>
        </div>
    </div>
</body>
```

### Example 13: Banner with maximum height

```html
<style>
    .page-banner {
        max-height: 250pt;
        width: 100%;
        background-color: #1e3a8a;
        color: white;
        padding: 40pt;
        box-sizing: border-box;
        overflow: hidden;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    .banner-content {
        text-align: center;
    }
    .banner-title {
        margin: 0 0 15pt 0;
        font-size: 36pt;
        font-weight: bold;
    }
    .banner-subtitle {
        margin: 0;
        font-size: 18pt;
    }
</style>
<body>
    <div class="page-banner">
        <div class="banner-content">
            <h1 class="banner-title">Welcome</h1>
            <p class="banner-subtitle">Discover our services</p>
        </div>
    </div>
</body>
```

### Example 14: Activity timeline with maximum height

```html
<style>
    .activity-timeline {
        max-height: 450pt;
        width: 400pt;
        margin: 20pt auto;
        padding: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        overflow-y: auto;
    }
    .timeline-item {
        padding: 15pt;
        margin-bottom: 12pt;
        padding-left: 40pt;
        position: relative;
        background-color: white;
        border: 1pt solid #e5e7eb;
        border-left: 3pt solid #3b82f6;
    }
    .timeline-marker {
        position: absolute;
        left: 10pt;
        top: 15pt;
        width: 12pt;
        height: 12pt;
        background-color: #3b82f6;
        border-radius: 6pt;
    }
    .timeline-title {
        margin: 0 0 5pt 0;
        font-weight: bold;
    }
    .timeline-time {
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="activity-timeline">
        <div class="timeline-item">
            <div class="timeline-marker"></div>
            <div class="timeline-title">Order Placed</div>
            <div class="timeline-time">10:30 AM</div>
        </div>
        <div class="timeline-item">
            <div class="timeline-marker"></div>
            <div class="timeline-title">Payment Processed</div>
            <div class="timeline-time">10:32 AM</div>
        </div>
        <div class="timeline-item">
            <div class="timeline-marker"></div>
            <div class="timeline-title">Order Shipped</div>
            <div class="timeline-time">2:15 PM</div>
        </div>
    </div>
</body>
```

### Example 15: Report section with maximum height

```html
<style>
    .report-container {
        width: 600pt;
        margin: 30pt auto;
    }
    .report-section {
        max-height: 350pt;
        margin-bottom: 20pt;
        padding: 25pt;
        background-color: white;
        border: 1pt solid #e5e7eb;
        overflow-y: auto;
    }
    .section-header {
        margin: 0 0 15pt 0;
        padding-bottom: 10pt;
        border-bottom: 2pt solid #e5e7eb;
        font-size: 20pt;
        font-weight: bold;
        position: sticky;
        top: 0;
        background-color: white;
    }
    .section-content {
        line-height: 1.6;
    }
    .data-summary {
        padding: 15pt;
        margin: 15pt 0;
        background-color: #f9fafb;
        border-left: 4pt solid #3b82f6;
    }
</style>
<body>
    <div class="report-container">
        <div class="report-section">
            <h2 class="section-header">Executive Summary</h2>
            <div class="section-content">
                <p>Detailed report content with maximum height constraint...</p>
                <div class="data-summary">
                    <strong>Key Metrics:</strong>
                    <ul>
                        <li>Revenue: $1.2M</li>
                        <li>Growth: 25%</li>
                    </ul>
                </div>
                <p>Additional analysis and findings...</p>
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [height](/reference/cssproperties/css_prop_height) - Set element height
- [min-height](/reference/cssproperties/css_prop_min-height) - Set minimum height constraint
- [max-width](/reference/cssproperties/css_prop_max-width) - Set maximum width constraint
- [min-width](/reference/cssproperties/css_prop_min-width) - Set minimum width constraint
- [width](/reference/cssproperties/css_prop_width) - Set element width
- [padding](/reference/cssproperties/css_prop_padding) - Set padding shorthand property
- [margin](/reference/cssproperties/css_prop_margin) - Set margin shorthand property
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
