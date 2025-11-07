---
layout: default
title: border-top-width
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-top-width : Top Border Width Property

The `border-top-width` property sets the width of the top border of an element. Use this property when you need precise control over individual border widths.

## Usage

```css
selector {
    border-top-width: value;
}
```

---

## Supported Values

### Named Widths
- `thin` - A thin border (typically 1pt)
- `medium` - A medium border (typically 2pt) - default
- `thick` - A thick border (typically 4pt)

### Length Values
Any valid length unit: `1pt`, `2px`, `0.5mm`, etc.

---

## Supported Elements

All elements that support borders: block elements, paragraphs, headings, tables, table cells, images, and containers.

---

## Notes

- Has no effect unless `border-top-style` is set to a value other than `none`
- Cannot be negative
- Adds to the total size of the element unless `box-sizing: border-box` is used

---

## Data Binding

The `border-top-width` property supports dynamic values through data binding, allowing top border thickness to be adjusted based on document data at runtime.

### Example 1: Priority-based header emphasis

```html
<style>
    .section-header {
        border-top-style: solid;
        border-top-color: #2563eb;
        padding-top: 15pt;
        font-size: 18pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="section-header" style="border-top-width: {{section.priority === 'high' ? '6pt' : '3pt'}}">
        {{section.title}}
    </div>
</body>
```

### Example 2: Dynamic table row separators

```html
<style>
    .table-row {
        border-top-style: solid;
        border-top-color: #e5e7eb;
        padding: 8pt 0;
    }
</style>
<body>
    <div class="table-row" style="border-top-width: {{row.isTotal ? '3pt' : '1pt'}}">
        <p>{{row.label}}: {{row.value}}</p>
    </div>
</body>
```

### Example 3: Conditional section dividers

```html
<style>
    .content-section {
        border-top-style: solid;
        border-top-color: #cbd5e1;
        padding-top: 15pt;
        margin-top: 15pt;
    }
</style>
<body>
    <div class="content-section" style="border-top-width: {{section.borderWidth}}">
        <h3>{{section.heading}}</h3>
        <p>{{section.content}}</p>
    </div>
</body>
```

---

## Examples

### Example 1: Emphasized header

```html
<style>
    .header {
        border-top-width: 4pt;
        border-top-style: solid;
        border-top-color: #2563eb;
        padding-top: 15pt;
    }
</style>
<body>
    <h2 class="header">Section Title</h2>
</body>
```

### Example 2: Varying widths

```html
<style>
    .thin-top {
        border-top-width: thin;
        border-top-style: solid;
        border-top-color: #6b7280;
        padding: 10pt;
        margin-bottom: 10pt;
    }
    .medium-top {
        border-top-width: medium;
        border-top-style: solid;
        border-top-color: #6b7280;
        padding: 10pt;
        margin-bottom: 10pt;
    }
    .thick-top {
        border-top-width: thick;
        border-top-style: solid;
        border-top-color: #6b7280;
        padding: 10pt;
    }
</style>
<body>
    <div class="thin-top">Thin top border</div>
    <div class="medium-top">Medium top border</div>
    <div class="thick-top">Thick top border</div>
</body>
```

### Example 3: Table headers

```html
<style>
    .data-table th {
        border-top-width: 3pt;
        border-top-style: solid;
        border-top-color: #1e293b;
        padding: 10pt;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr>
                <th>Product</th>
                <th>Price</th>
            </tr>
        </thead>
    </table>
</body>
```

### Example 4: Alert emphasis

```html
<style>
    .alert {
        border-top-width: 5pt;
        border-top-style: solid;
        padding: 15pt;
    }
    .alert-warning {
        border-top-color: #f59e0b;
        background-color: #fef3c7;
    }
</style>
<body>
    <div class="alert alert-warning">
        <strong>Warning:</strong> Important message.
    </div>
</body>
```

### Example 5: Section divider

```html
<style>
    .section {
        border-top-width: 2pt;
        border-top-style: dashed;
        border-top-color: #cbd5e1;
        padding-top: 15pt;
        margin-top: 15pt;
    }
</style>
<body>
    <div class="section">
        <h3>New Section</h3>
        <p>Section content.</p>
    </div>
</body>
```

### Example 6: Card with accent

```html
<style>
    .card {
        border: 1pt solid #e5e7eb;
        border-top-width: 4pt;
        border-top-color: #2563eb;
        padding: 15pt;
    }
</style>
<body>
    <div class="card">
        <h3>Featured Card</h3>
        <p>Card content with thick top border.</p>
    </div>
</body>
```

### Example 7: Invoice total

```html
<style>
    .invoice-total {
        border-top-width: 3pt;
        border-top-style: double;
        border-top-color: #0f172a;
        padding-top: 15pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="invoice-total">
        <p>Total: $550.00</p>
    </div>
</body>
```

### Example 8: Form section

```html
<style>
    .form-section {
        border-top-width: 2pt;
        border-top-style: solid;
        border-top-color: #d1d5db;
        padding-top: 20pt;
        margin-top: 20pt;
    }
</style>
<body>
    <div class="form-section">
        <h3>Contact Information</h3>
    </div>
</body>
```

### Example 9: Quote separator

```html
<style>
    .quote {
        border-top-width: 3pt;
        border-top-style: solid;
        border-top-color: #6366f1;
        padding-top: 15pt;
        margin-top: 20pt;
        font-style: italic;
    }
</style>
<body>
    <div class="quote">
        <p>"Creativity is intelligence having fun."</p>
    </div>
</body>
```

### Example 10: Dashboard panels

```html
<style>
    .panel {
        border: 1pt solid #e5e7eb;
        background-color: white;
    }
    .panel-footer {
        border-top-width: 2pt;
        border-top-style: solid;
        border-top-color: #2563eb;
        padding: 10pt;
    }
</style>
<body>
    <div class="panel">
        <div style="padding: 15pt;">Panel content</div>
        <div class="panel-footer">Footer info</div>
    </div>
</body>
```

### Example 11: Certificate decoration

```html
<style>
    .certificate {
        border: 4pt double #854d0e;
        border-top-width: 8pt;
        padding: 40pt;
        text-align: center;
    }
</style>
<body>
    <div class="certificate">
        <h1>Certificate</h1>
    </div>
</body>
```

### Example 12: Pricing tiers

```html
<style>
    .pricing-tier {
        border: 2pt solid #d1d5db;
        border-top-width: 5pt;
        padding: 20pt;
        text-align: center;
    }
    .pricing-tier.featured {
        border-top-width: 8pt;
        border-top-color: #2563eb;
    }
</style>
<body>
    <div class="pricing-tier featured">
        <h3>Pro Plan</h3>
        <p>$29.99/mo</p>
    </div>
</body>
```

### Example 13: Timeline milestone

```html
<style>
    .timeline-entry {
        border-top-width: 2pt;
        border-top-style: solid;
        border-top-color: #e5e7eb;
        padding: 15pt 0;
    }
    .timeline-entry.milestone {
        border-top-width: 4pt;
        border-top-color: #2563eb;
    }
</style>
<body>
    <div class="timeline-entry milestone">
        <p><strong>March 2025:</strong> Major milestone</p>
    </div>
</body>
```

### Example 14: Report header

```html
<style>
    .report-header {
        border-top-width: 6pt;
        border-top-style: solid;
        border-top-color: #1e3a8a;
        padding-top: 25pt;
    }
    .report-title {
        font-size: 28pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="report-header">
        <h1 class="report-title">Annual Report 2025</h1>
    </div>
</body>
```

### Example 15: Navigation active state

```html
<style>
    .nav-item {
        border-top-width: 0;
        border-top-style: solid;
        padding: 10pt;
    }
    .nav-item.active {
        border-top-width: 4pt;
        border-top-color: #2563eb;
        background-color: #eff6ff;
    }
</style>
<body>
    <div class="nav-item active">Dashboard</div>
    <div class="nav-item">Settings</div>
</body>
```

---

## See Also

- [border-top](/reference/cssproperties/css_prop_border-top) - Top border shorthand
- [border-width](/reference/cssproperties/css_prop_border-width) - All border widths
- [border-right-width](/reference/cssproperties/css_prop_border-right-width) - Right border width
- [border-bottom-width](/reference/cssproperties/css_prop_border-bottom-width) - Bottom border width
- [border-left-width](/reference/cssproperties/css_prop_border-left-width) - Left border width
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
