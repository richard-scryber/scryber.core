---
layout: default
title: opacity
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# opacity : Element Opacity Property

The `opacity` property controls the transparency level of an entire element, including all its children, backgrounds, borders, and text. This property is essential for creating fade effects, overlays, watermarks, and visual hierarchy in PDF documents.

## Usage

```css
selector {
    opacity: value;
}
```

The opacity property accepts a numeric value between 0.0 (fully transparent) and 1.0 (fully opaque).

---

## Supported Values

### Numeric Values
- `0.0` - Fully transparent (invisible)
- `0.5` - 50% transparent (semi-transparent)
- `1.0` - Fully opaque (default, fully visible)
- Any decimal value between 0.0 and 1.0

### Percentage Values
- `0%` - Fully transparent
- `50%` - 50% transparent
- `100%` - Fully opaque

---

## Supported Elements

The `opacity` property can be applied to all HTML and SVG elements:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Images (`<img>`)
- Tables and table elements
- SVG shapes and containers
- All other elements

---

## Notes

- Default opacity value is `1.0` (fully opaque)
- Opacity affects the entire element including background, borders, text, and all children
- Values outside the 0.0-1.0 range are clamped to the nearest valid value
- Unlike RGBA colors, opacity affects everything within the element uniformly
- Child elements cannot override parent opacity to become more opaque
- Opacity is multiplicative: if parent has 0.5 opacity and child has 0.5, effective child opacity is 0.25
- Useful for watermarks, disabled states, and visual layering
- PDF rendering maintains precise opacity across all viewers
- For color-specific transparency, use RGBA, `fill-opacity`, or `stroke-opacity` instead
- Opacity creates a new stacking context

---

## Data Binding

The `opacity` property can be dynamically set using data binding expressions, enabling element transparency to reflect states, priorities, or data-driven visibility levels.

### Example 1: Priority-based opacity in task lists

```html
<style>
    .task-item {
        background-color: #f3f4f6;
        padding: 12pt;
        margin: 8pt 0;
        border-left: 4pt solid #3b82f6;
    }
</style>
<body>
    {{#each tasks}}
    <div class="task-item" style="opacity: {{priority / 10}}">
        <h3>{{title}}</h3>
        <p>Priority: {{priority}}/10</p>
        <p>{{description}}</p>
    </div>
    {{/each}}
</body>
```

With model data:
```json
{
    "tasks": [
        { "title": "Critical Bug Fix", "priority": 10, "description": "Fix security vulnerability" },
        { "title": "Feature Enhancement", "priority": 7, "description": "Add user dashboard" },
        { "title": "Documentation Update", "priority": 4, "description": "Update API docs" }
    ]
}
```

### Example 2: Conditional visibility based on status

```html
<style>
    .status-row {
        padding: 10pt;
        border-bottom: 1pt solid #e5e7eb;
    }
</style>
<body>
    <table>
        {{#each records}}
        <tr class="status-row" style="opacity: {{isArchived ? 0.3 : isActive ? 1.0 : 0.6}}">
            <td>{{name}}</td>
            <td>{{status}}</td>
            <td>{{date}}</td>
        </tr>
        {{/each}}
    </table>
</body>
```

### Example 3: Age-based document opacity

```html
<style>
    .document-card {
        background-color: white;
        border: 1pt solid #d1d5db;
        padding: 15pt;
        margin: 10pt 0;
    }
</style>
<body>
    {{#each documents}}
    <div class="document-card" style="opacity: {{daysOld > 90 ? 0.4 : daysOld > 30 ? 0.7 : 1.0}}">
        <h3>{{title}}</h3>
        <p>Created: {{createdDate}}</p>
        <p>{{daysOld}} days old</p>
    </div>
    {{/each}}
</body>
```

---

## Examples

### Example 1: Basic opacity levels

```html
<style>
    .box {
        background-color: blue;
        padding: 20pt;
        margin: 10pt;
    }
    .opacity-full { opacity: 1.0; }
    .opacity-half { opacity: 0.5; }
    .opacity-quarter { opacity: 0.25; }
</style>
<body>
    <div class="box opacity-full">Fully opaque</div>
    <div class="box opacity-half">50% transparent</div>
    <div class="box opacity-quarter">75% transparent</div>
</body>
```

### Example 2: Watermark text

```html
<style>
    .watermark {
        position: absolute;
        top: 200pt;
        left: 100pt;
        font-size: 72pt;
        font-weight: bold;
        color: gray;
        opacity: 0.1;
        transform: rotate(-45deg);
    }
    .content {
        position: relative;
        z-index: 10;
    }
</style>
<body>
    <div class="watermark">DRAFT</div>
    <div class="content">
        <h1>Document Title</h1>
        <p>This is the main content of the document.</p>
    </div>
</body>
```

### Example 3: Image transparency

```html
<style>
    .logo-faded {
        opacity: 0.4;
        width: 150pt;
    }
</style>
<body>
    <img src="logo.png" class="logo-faded" alt="Faded logo"/>
    <p>Content with semi-transparent logo background</p>
</body>
```

### Example 4: Disabled state

```html
<style>
    .button {
        background-color: #3b82f6;
        color: white;
        padding: 10pt 20pt;
        text-align: center;
    }
    .button-disabled {
        opacity: 0.4;
    }
</style>
<body>
    <div class="button">Active Button</div>
    <div class="button button-disabled">Disabled Button</div>
</body>
```

### Example 5: Overlay effect

```html
<style>
    .overlay-container {
        position: relative;
    }
    .overlay {
        background-color: black;
        opacity: 0.6;
        padding: 20pt;
    }
    .overlay-text {
        color: white;
    }
</style>
<body>
    <div class="overlay-container">
        <div class="overlay">
            <p class="overlay-text">This text appears on a dark overlay</p>
        </div>
    </div>
</body>
```

### Example 6: Percentage-based opacity

```html
<style>
    .item { background-color: green; padding: 15pt; margin: 5pt; }
    .opacity-25 { opacity: 25%; }
    .opacity-50 { opacity: 50%; }
    .opacity-75 { opacity: 75%; }
    .opacity-100 { opacity: 100%; }
</style>
<body>
    <div class="item opacity-25">25% Opacity</div>
    <div class="item opacity-50">50% Opacity</div>
    <div class="item opacity-75">75% Opacity</div>
    <div class="item opacity-100">100% Opacity</div>
</body>
```

### Example 7: Fade effect for sections

```html
<style>
    .section {
        padding: 20pt;
        background-color: #f3f4f6;
        margin-bottom: 10pt;
    }
    .section-faded {
        opacity: 0.3;
    }
</style>
<body>
    <div class="section">
        <h2>Current Section</h2>
        <p>This section is fully visible.</p>
    </div>
    <div class="section section-faded">
        <h2>Previous Section</h2>
        <p>This section is faded to show it's less relevant.</p>
    </div>
</body>
```

### Example 8: Table row emphasis

```html
<style>
    table { width: 100%; border-collapse: collapse; }
    td { padding: 8pt; border: 1pt solid #d1d5db; }
    .row-deemphasize { opacity: 0.4; }
</style>
<body>
    <table>
        <tr>
            <td>Active Row</td>
            <td>$100.00</td>
        </tr>
        <tr class="row-deemphasize">
            <td>Inactive Row</td>
            <td>$50.00</td>
        </tr>
        <tr>
            <td>Active Row</td>
            <td>$75.00</td>
        </tr>
    </table>
</body>
```

### Example 9: Chart with opacity hierarchy

```html
<style>
    .chart-primary { opacity: 1.0; background-color: #3b82f6; }
    .chart-secondary { opacity: 0.7; background-color: #60a5fa; }
    .chart-tertiary { opacity: 0.4; background-color: #93c5fd; }
    .bar { padding: 10pt; margin: 5pt; }
</style>
<body>
    <div class="bar chart-primary">Primary Data (100%)</div>
    <div class="bar chart-secondary">Secondary Data (70%)</div>
    <div class="bar chart-tertiary">Tertiary Data (40%)</div>
</body>
```

### Example 10: Confidential document overlay

```html
<style>
    .confidential-banner {
        background-color: red;
        color: white;
        opacity: 0.15;
        padding: 30pt;
        text-align: center;
        font-size: 36pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="confidential-banner">CONFIDENTIAL</div>
    <h1>Sensitive Information</h1>
    <p>Document content goes here...</p>
</body>
```

### Example 11: Progressive disclosure

```html
<style>
    .step {
        background-color: #e5e7eb;
        padding: 15pt;
        margin: 10pt 0;
    }
    .step-complete { opacity: 0.5; }
    .step-current { opacity: 1.0; }
    .step-future { opacity: 0.3; }
</style>
<body>
    <div class="step step-complete">
        <h3>Step 1: Complete</h3>
        <p>This step is finished.</p>
    </div>
    <div class="step step-current">
        <h3>Step 2: In Progress</h3>
        <p>Currently working on this step.</p>
    </div>
    <div class="step step-future">
        <h3>Step 3: Upcoming</h3>
        <p>This step hasn't started yet.</p>
    </div>
</body>
```

### Example 12: Background image with text overlay

```html
<style>
    .hero {
        position: relative;
        height: 200pt;
    }
    .hero-image {
        width: 100%;
        opacity: 0.3;
    }
    .hero-text {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        font-size: 24pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="hero">
        <img src="background.jpg" class="hero-image" alt="Background"/>
        <div class="hero-text">Welcome to Our Company</div>
    </div>
</body>
```

### Example 13: Status indicators with opacity

```html
<style>
    .status-badge {
        display: inline-block;
        padding: 6pt 12pt;
        margin: 5pt;
        background-color: #3b82f6;
        color: white;
        border-radius: 4pt;
    }
    .status-archived { opacity: 0.3; }
    .status-inactive { opacity: 0.5; }
    .status-active { opacity: 1.0; }
</style>
<body>
    <span class="status-badge status-archived">Archived</span>
    <span class="status-badge status-inactive">Inactive</span>
    <span class="status-badge status-active">Active</span>
</body>
```

### Example 14: Comparison view

```html
<style>
    .comparison {
        display: table;
        width: 100%;
    }
    .version {
        display: table-cell;
        width: 50%;
        padding: 15pt;
    }
    .old-version {
        opacity: 0.4;
        background-color: #fee2e2;
    }
    .new-version {
        opacity: 1.0;
        background-color: #dcfce7;
    }
</style>
<body>
    <div class="comparison">
        <div class="version old-version">
            <h3>Old Version</h3>
            <p>Previous content...</p>
        </div>
        <div class="version new-version">
            <h3>New Version</h3>
            <p>Updated content...</p>
        </div>
    </div>
</body>
```

### Example 15: Timeline with emphasis

```html
<style>
    .timeline-item {
        padding: 15pt;
        margin: 10pt 0;
        background-color: #f3f4f6;
        border-left: 4pt solid #3b82f6;
    }
    .timeline-past { opacity: 0.4; }
    .timeline-present { opacity: 1.0; border-color: #10b981; }
    .timeline-future { opacity: 0.6; }
</style>
<body>
    <div class="timeline-item timeline-past">
        <h3>Q1 2024 - Completed</h3>
        <p>Project initialization phase</p>
    </div>
    <div class="timeline-item timeline-present">
        <h3>Q2 2024 - Current</h3>
        <p>Development phase</p>
    </div>
    <div class="timeline-item timeline-future">
        <h3>Q3 2024 - Planned</h3>
        <p>Testing and deployment phase</p>
    </div>
</body>
```

---

## See Also

- [fill-opacity](/reference/cssproperties/css_prop_fill-opacity) - SVG fill transparency
- [stroke-opacity](/reference/cssproperties/css_prop_stroke-opacity) - SVG stroke transparency
- [color](/reference/cssproperties/css_prop_color) - Text color (supports RGBA)
- [background-color](/reference/cssproperties/css_prop_background-color) - Background color (supports RGBA)

---
