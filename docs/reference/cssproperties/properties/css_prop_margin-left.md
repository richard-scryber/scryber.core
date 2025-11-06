---
layout: default
title: margin-left
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# margin-left : Left Margin Property

The `margin-left` property sets the left margin of an element in PDF documents. The left margin creates space to the left of the element, separating it from adjacent elements or the page edge and controlling horizontal spacing in document layouts.

## Usage

```css
selector {
    margin-left: value;
}
```

The margin-left property accepts a single length value or percentage that defines the space to the left of the element.

---

## Supported Values

### Length Units
- Points: `10pt`, `15pt`, `20pt`
- Pixels: `10px`, `15px`, `20px`
- Inches: `0.5in`, `1in`
- Centimeters: `2cm`, `3cm`
- Millimeters: `10mm`, `20mm`
- Ems: `1em`, `1.5em`, `2em`
- Percentage: `5%`, `10%`, `15%` (relative to parent width)

### Special Values
- `0` - No left margin
- `auto` - Automatic margin (useful for layout alignment and centering)

---

## Supported Elements

The `margin-left` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables (`<table>`)
- Lists (`<ul>`, `<ol>`)
- List items (`<li>`)
- Images (`<img>`)
- All container elements

---

## Notes

- Left margins are transparent and do not have background colors
- Left margins do not collapse (unlike vertical margins)
- Negative left margins pull the element leftward or cause overlapping
- Percentage left margins are calculated relative to the parent element's width
- The `auto` value can be used with `margin-right: auto` to center block elements
- Left margins are particularly useful for indentation and list styling
- Left margins affect horizontal positioning and layout flow

---

## Data Binding

The `margin-left` property supports dynamic values through data binding, allowing you to create flexible left spacing for indentation, nested content hierarchy, and layout positioning based on data-driven requirements.

### Example 1: Dynamic indentation based on content level

```html
<style>
    .content {
        padding: 30pt;
    }
    .level-1 {
        margin-left: 0;
    }
    .level-2 {
        margin-left: {{indentation.level2}}pt;
        padding: 10pt;
        background-color: #f9fafb;
    }
    .level-3 {
        margin-left: {{indentation.level3}}pt;
        padding: 10pt;
        background-color: #f3f4f6;
    }
</style>
<body>
    <div class="content">
        <div class="level-1">
            <h3>Top Level</h3>
        </div>
        <div class="level-2">
            <h4>Second Level</h4>
        </div>
        <div class="level-3">
            <h5>Third Level</h5>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "indentation": {
        "level2": 20,
        "level3": 40
    }
}
```

### Example 2: Invoice line items with dynamic indentation

```html
<style>
    .invoice {
        padding: 40pt;
    }
    .section-title {
        font-weight: bold;
        font-size: 14pt;
        margin-bottom: 10pt;
    }
    .line-item {
        margin-left: {{layout.itemIndent}}pt;
        margin-bottom: 8pt;
        display: flex;
        justify-content: space-between;
    }
    .subtotal {
        margin-left: {{layout.itemIndent}}pt;
        margin-top: 10pt;
        padding-top: 10pt;
        border-top: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="invoice">
        <div class="section-title">Services Provided:</div>
        <div class="line-item">
            <span>Consulting Services</span>
            <span>{{items[0].amount}}</span>
        </div>
        <div class="subtotal">
            <strong>Subtotal: {{invoice.subtotal}}</strong>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "layout": {
        "itemIndent": 20
    },
    "items": [
        {
            "amount": "$1,500.00"
        }
    ],
    "invoice": {
        "subtotal": "$1,500.00"
    }
}
```

### Example 3: Chat message alignment based on sender

```html
<style>
    .chat-container {
        padding: 25pt;
    }
    .message {
        margin-bottom: 15pt;
        padding: 10pt 15pt;
        max-width: 70%;
        border-radius: 8pt;
    }
    .message-sent {
        margin-left: {{message.sender === 'user' ? '30%' : '0'}};
        background-color: {{message.sender === 'user' ? '#3b82f6' : '#f3f4f6'}};
        color: {{message.sender === 'user' ? 'white' : '#1f2937'}};
    }
</style>
<body>
    <div class="chat-container">
        <div class="message message-sent">
            <p>{{message.text}}</p>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "message": {
        "sender": "user",
        "text": "Hello, I need help with this product."
    }
}
```

---

## Examples

### Example 1: Basic left margin for indentation

```html
<style>
    .document {
        padding: 40pt;
    }
    .indented {
        margin-left: 30pt;
        padding: 10pt;
        background-color: #f3f4f6;
    }
</style>
<body>
    <div class="document">
        <p>Regular paragraph with no indentation.</p>
        <div class="indented">
            <p>This content is indented with a 30pt left margin.</p>
        </div>
    </div>
</body>
```

### Example 2: Nested content with increasing indentation

```html
<style>
    .content {
        padding: 30pt;
    }
    .level-1 {
        margin-left: 0;
    }
    .level-2 {
        margin-left: 20pt;
        padding: 10pt;
        background-color: #f9fafb;
    }
    .level-3 {
        margin-left: 40pt;
        padding: 10pt;
        background-color: #f3f4f6;
    }
</style>
<body>
    <div class="content">
        <div class="level-1">
            <h3>Top Level</h3>
            <p>Content at top level.</p>
        </div>
        <div class="level-2">
            <h4>Second Level</h4>
            <p>Content indented one level.</p>
        </div>
        <div class="level-3">
            <h5>Third Level</h5>
            <p>Content indented two levels.</p>
        </div>
    </div>
</body>
```

### Example 3: Blockquote with left margin

```html
<style>
    .article {
        padding: 40pt;
    }
    .article p {
        margin-bottom: 12pt;
    }
    .blockquote {
        margin-left: 40pt;
        margin-right: 40pt;
        padding: 15pt 15pt 15pt 20pt;
        background-color: #f5f5f5;
        border-left: 4pt solid #6366f1;
        font-style: italic;
    }
    .quote-author {
        margin-top: 10pt;
        margin-left: 20pt;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="article">
        <p>Regular article text before the quote.</p>
        <div class="blockquote">
            <p>"Innovation is the ability to see change as an opportunity."</p>
            <p class="quote-author">— Anonymous</p>
        </div>
        <p>Article continues after the quote.</p>
    </div>
</body>
```

### Example 4: List with custom left margin

```html
<style>
    .list-container {
        padding: 30pt;
    }
    .custom-list {
        margin-left: 40pt;
        padding-left: 0;
        list-style-position: inside;
    }
    .custom-list li {
        margin-bottom: 8pt;
        padding-left: 10pt;
    }
    .nested-list {
        margin-left: 25pt;
        margin-top: 8pt;
    }
</style>
<body>
    <div class="list-container">
        <h3>Main Topics</h3>
        <ul class="custom-list">
            <li>First item</li>
            <li>Second item
                <ul class="nested-list">
                    <li>Nested item A</li>
                    <li>Nested item B</li>
                </ul>
            </li>
            <li>Third item</li>
        </ul>
    </div>
</body>
```

### Example 5: Invoice with indented line items

```html
<style>
    .invoice {
        padding: 40pt;
    }
    .invoice-header {
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e3a8a;
        color: white;
    }
    .invoice-section {
        margin-bottom: 20pt;
    }
    .section-title {
        font-weight: bold;
        font-size: 14pt;
        margin-bottom: 10pt;
    }
    .line-item {
        margin-left: 20pt;
        margin-bottom: 8pt;
        display: flex;
        justify-content: space-between;
    }
    .subtotal {
        margin-left: 20pt;
        margin-top: 10pt;
        padding-top: 10pt;
        border-top: 1pt solid #d1d5db;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1>INVOICE</h1>
            <p>INV-2025-001</p>
        </div>
        <div class="invoice-section">
            <div class="section-title">Services Provided:</div>
            <div class="line-item">
                <span>Consulting Services</span>
                <span>$1,500.00</span>
            </div>
            <div class="line-item">
                <span>Development Work</span>
                <span>$2,500.00</span>
            </div>
            <div class="subtotal">
                <strong>Subtotal: $4,000.00</strong>
            </div>
        </div>
    </div>
</body>
```

### Example 6: Sidebar layout with left margin

```html
<style>
    .page {
        padding: 30pt;
    }
    .sidebar {
        float: left;
        width: 150pt;
        padding: 15pt;
        background-color: #f3f4f6;
    }
    .main-content {
        margin-left: 190pt;
        padding: 15pt;
    }
    .main-content h1 {
        margin-bottom: 15pt;
    }
    .main-content p {
        margin-bottom: 12pt;
    }
</style>
<body>
    <div class="page">
        <div class="sidebar">
            <h3>Navigation</h3>
            <ul>
                <li>Home</li>
                <li>About</li>
                <li>Services</li>
            </ul>
        </div>
        <div class="main-content">
            <h1>Main Content Area</h1>
            <p>Content flows with left margin to accommodate the sidebar.</p>
        </div>
    </div>
</body>
```

### Example 7: Form with label and input alignment

```html
<style>
    .form {
        padding: 30pt;
    }
    .form-row {
        margin-bottom: 15pt;
        overflow: hidden;
    }
    .form-label {
        float: left;
        width: 120pt;
        padding-top: 8pt;
        font-weight: bold;
    }
    .form-input-container {
        margin-left: 140pt;
    }
    .form-input {
        width: 100%;
        padding: 8pt;
        border: 1pt solid #d1d5db;
    }
    .form-help {
        margin-left: 140pt;
        margin-top: 5pt;
        font-size: 9pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="form">
        <div class="form-row">
            <label class="form-label">Full Name:</label>
            <div class="form-input-container">
                <input class="form-input" type="text" />
            </div>
        </div>
        <div class="form-row">
            <label class="form-label">Email:</label>
            <div class="form-input-container">
                <input class="form-input" type="email" />
            </div>
            <div class="form-help">We'll never share your email.</div>
        </div>
    </div>
</body>
```

### Example 8: Code block with left margin

```html
<style>
    .documentation {
        padding: 40pt;
    }
    .doc-section {
        margin-bottom: 25pt;
    }
    .doc-section h2 {
        margin-bottom: 10pt;
    }
    .code-block {
        margin-left: 20pt;
        margin-top: 10pt;
        padding: 15pt;
        background-color: #1f2937;
        color: #f9fafb;
        font-family: monospace;
        font-size: 10pt;
        border-left: 4pt solid #3b82f6;
    }
</style>
<body>
    <div class="documentation">
        <div class="doc-section">
            <h2>Example Usage</h2>
            <p>Here's how to use the function:</p>
            <div class="code-block">
                function example() {<br/>
                &nbsp;&nbsp;return "Hello World";<br/>
                }
            </div>
        </div>
    </div>
</body>
```

### Example 9: Table of contents with indentation

```html
<style>
    .toc-container {
        padding: 30pt;
    }
    .toc-title {
        margin-bottom: 20pt;
        font-size: 18pt;
        font-weight: bold;
    }
    .toc-item {
        margin-bottom: 8pt;
    }
    .toc-level-1 {
        margin-left: 0;
        font-weight: bold;
    }
    .toc-level-2 {
        margin-left: 20pt;
    }
    .toc-level-3 {
        margin-left: 40pt;
        font-size: 10pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="toc-container">
        <h2 class="toc-title">Table of Contents</h2>
        <div class="toc-item toc-level-1">1. Introduction</div>
        <div class="toc-item toc-level-2">1.1 Background</div>
        <div class="toc-item toc-level-2">1.2 Objectives</div>
        <div class="toc-item toc-level-1">2. Methodology</div>
        <div class="toc-item toc-level-2">2.1 Research Design</div>
        <div class="toc-item toc-level-3">2.1.1 Sample Selection</div>
        <div class="toc-item toc-level-3">2.1.2 Data Collection</div>
    </div>
</body>
```

### Example 10: Chat-style message layout

```html
<style>
    .chat-container {
        padding: 25pt;
    }
    .message {
        margin-bottom: 15pt;
        padding: 10pt 15pt;
        max-width: 70%;
        border-radius: 8pt;
    }
    .message-sent {
        margin-left: 30%;
        background-color: #3b82f6;
        color: white;
    }
    .message-received {
        margin-left: 0;
        margin-right: 30%;
        background-color: #f3f4f6;
        color: #1f2937;
    }
    .message-time {
        margin-top: 5pt;
        font-size: 8pt;
        opacity: 0.7;
    }
</style>
<body>
    <div class="chat-container">
        <div class="message message-received">
            <p>Hello! How can I help you today?</p>
            <div class="message-time">10:30 AM</div>
        </div>
        <div class="message message-sent">
            <p>I need information about the product.</p>
            <div class="message-time">10:32 AM</div>
        </div>
    </div>
</body>
```

### Example 11: Certificate with centered content

```html
<style>
    .certificate {
        width: 500pt;
        margin-left: auto;
        margin-right: auto;
        margin-top: 50pt;
        padding: 40pt;
        border: 5pt double #1e3a8a;
        text-align: center;
    }
    .cert-title {
        margin-bottom: 20pt;
        font-size: 28pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .cert-body {
        margin-left: 40pt;
        margin-right: 40pt;
        margin-bottom: 30pt;
        font-size: 14pt;
    }
    .cert-signature {
        margin-top: 40pt;
        font-size: 12pt;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">Certificate of Completion</h1>
        <div class="cert-body">
            <p>This certifies that</p>
            <p style="font-size: 20pt; font-weight: bold;">Jane Smith</p>
            <p>has successfully completed the training program</p>
        </div>
        <div class="cert-signature">
            <p>Authorized Signature</p>
            <p>January 15, 2025</p>
        </div>
    </div>
</body>
```

### Example 12: Report with executive summary indent

```html
<style>
    .report {
        padding: 40pt;
    }
    .report-title {
        margin-bottom: 25pt;
        font-size: 24pt;
        text-align: center;
    }
    .executive-summary {
        margin-left: 30pt;
        margin-right: 30pt;
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #dbeafe;
        border-left: 4pt solid #2563eb;
    }
    .executive-summary h2 {
        margin-bottom: 12pt;
    }
    .report-section {
        margin-bottom: 25pt;
    }
    .report-section h2 {
        margin-bottom: 15pt;
        border-bottom: 1pt solid #d1d5db;
        padding-bottom: 8pt;
    }
</style>
<body>
    <div class="report">
        <h1 class="report-title">Quarterly Report</h1>
        <div class="executive-summary">
            <h2>Executive Summary</h2>
            <p>Key highlights and findings from Q1 2025.</p>
        </div>
        <div class="report-section">
            <h2>Financial Performance</h2>
            <p>Detailed analysis of quarterly results.</p>
        </div>
    </div>
</body>
```

### Example 13: Product specification with left margin

```html
<style>
    .product-spec {
        padding: 30pt;
    }
    .product-name {
        margin-bottom: 20pt;
        font-size: 20pt;
        font-weight: bold;
    }
    .spec-category {
        margin-bottom: 15pt;
    }
    .spec-category h3 {
        margin-bottom: 8pt;
        font-size: 14pt;
        color: #1f2937;
    }
    .spec-item {
        margin-left: 25pt;
        margin-bottom: 5pt;
        display: flex;
    }
    .spec-label {
        width: 120pt;
        font-weight: bold;
        color: #6b7280;
    }
    .spec-value {
        flex: 1;
    }
</style>
<body>
    <div class="product-spec">
        <h1 class="product-name">Premium Widget Pro</h1>
        <div class="spec-category">
            <h3>Physical Specifications</h3>
            <div class="spec-item">
                <span class="spec-label">Dimensions:</span>
                <span class="spec-value">10 x 8 x 2 inches</span>
            </div>
            <div class="spec-item">
                <span class="spec-label">Weight:</span>
                <span class="spec-value">2.5 lbs</span>
            </div>
        </div>
        <div class="spec-category">
            <h3>Technical Specifications</h3>
            <div class="spec-item">
                <span class="spec-label">Power:</span>
                <span class="spec-value">110-240V AC</span>
            </div>
            <div class="spec-item">
                <span class="spec-label">Warranty:</span>
                <span class="spec-value">2 Years</span>
            </div>
        </div>
    </div>
</body>
```

### Example 14: Newsletter with article indentation

```html
<style>
    .newsletter {
        padding: 30pt;
    }
    .newsletter-header {
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
    .article-section {
        margin-bottom: 25pt;
    }
    .article-title {
        margin-bottom: 10pt;
        font-size: 16pt;
        font-weight: bold;
    }
    .article-intro {
        margin-bottom: 10pt;
    }
    .article-body {
        margin-left: 20pt;
        padding-left: 15pt;
        border-left: 2pt solid #e5e7eb;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>Monthly Newsletter</h1>
        </div>
        <div class="article-section">
            <h2 class="article-title">Feature Story</h2>
            <p class="article-intro">Introduction to the main story.</p>
            <div class="article-body">
                <p>Detailed content with left margin indentation for visual hierarchy and improved readability.</p>
            </div>
        </div>
    </div>
</body>
```

### Example 15: Timeline layout with left margin

```html
<style>
    .timeline {
        padding: 30pt 30pt 30pt 50pt;
    }
    .timeline-title {
        margin-bottom: 25pt;
        font-size: 20pt;
        font-weight: bold;
    }
    .timeline-item {
        margin-left: 30pt;
        margin-bottom: 20pt;
        padding-left: 20pt;
        border-left: 3pt solid #3b82f6;
        position: relative;
    }
    .timeline-date {
        margin-bottom: 5pt;
        font-weight: bold;
        color: #3b82f6;
        font-size: 12pt;
    }
    .timeline-title-item {
        margin-bottom: 5pt;
        font-weight: bold;
        font-size: 14pt;
    }
    .timeline-description {
        margin-bottom: 0;
        color: #6b7280;
        font-size: 11pt;
    }
</style>
<body>
    <div class="timeline">
        <h1 class="timeline-title">Company Milestones</h1>
        <div class="timeline-item">
            <div class="timeline-date">January 2025</div>
            <div class="timeline-title-item">Product Launch</div>
            <p class="timeline-description">Successfully launched version 2.0 of our flagship product.</p>
        </div>
        <div class="timeline-item">
            <div class="timeline-date">March 2025</div>
            <div class="timeline-title-item">Market Expansion</div>
            <p class="timeline-description">Expanded operations to three new international markets.</p>
        </div>
        <div class="timeline-item">
            <div class="timeline-date">June 2025</div>
            <div class="timeline-title-item">Award Recognition</div>
            <p class="timeline-description">Received industry excellence award for innovation.</p>
        </div>
    </div>
</body>
```

---

## See Also

- [margin](/reference/cssproperties/css_prop_margin) - Set all margins shorthand
- [margin-right](/reference/cssproperties/css_prop_margin-right) - Set right margin
- [margin-top](/reference/cssproperties/css_prop_margin-top) - Set top margin
- [margin-bottom](/reference/cssproperties/css_prop_margin-bottom) - Set bottom margin
- [padding-left](/reference/cssproperties/css_prop_padding-left) - Set left padding
- [margin-inline-start](/reference/cssproperties/css_prop_margin-inline-start) - Set inline start margin
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
