---
layout: default
title: border-top-style
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-top-style : Top Border Style Property

The `border-top-style` property sets the line style of the top border.

## Usage

```css
selector {
    border-top-style: value;
}
```

---

## Supported Values

- `none` - No border (default)
- `solid` - Solid line
- `dashed` - Dashed line
- `dotted` - Dotted line
- `double` - Double line

---

## Supported Elements

All elements that support borders.

---

## Notes

- Must be set for the border to be visible
- The `none` value removes the border

---

## Data Binding

The `border-top-style` property supports dynamic values through data binding, allowing top border patterns to be customized based on document data at runtime.

### Example 1: Status-based border styles

```html
<style>
    .section-header {
        border-top-width: 3pt;
        border-top-color: #2563eb;
        padding-top: 15pt;
    }
</style>
<body>
    <div class="section-header" style="border-top-style: {{section.isComplete ? 'solid' : 'dashed'}}">
        <h2>{{section.title}}</h2>
    </div>
</body>
```

### Example 2: Document formality indicators

```html
<style>
    .document-header {
        border-top-width: 4pt;
        border-top-color: #854d0e;
        padding-top: 20pt;
    }
</style>
<body>
    <div class="document-header" style="border-top-style: {{doc.isFormal ? 'double' : 'solid'}}">
        <h1>{{doc.title}}</h1>
    </div>
</body>
```

### Example 3: Table row separators

```html
<style>
    .table-row {
        border-top-width: 2pt;
        border-top-color: #e5e7eb;
        padding: 8pt 0;
    }
</style>
<body>
    <div class="table-row" style="border-top-style: {{row.isSummary ? 'double' : 'solid'}}">
        <p>{{row.label}}: {{row.value}}</p>
    </div>
</body>
```

---

## Examples

### Example 1: Solid top border

```html
<style>
    .header {
        border-top-width: 3pt;
        border-top-style: solid;
        border-top-color: #2563eb;
        padding-top: 15pt;
    }
</style>
<body>
    <h2 class="header">Section Title</h2>
</body>
```

### Example 2: Dashed separator

```html
<style>
    .section {
        border-top-width: 2pt;
        border-top-style: dashed;
        border-top-color: #cbd5e1;
        padding-top: 15pt;
    }
</style>
<body>
    <div class="section">Section content</div>
</body>
```

### Example 3: Dotted divider

```html
<style>
    .divider {
        border-top-width: 2pt;
        border-top-style: dotted;
        border-top-color: #6b7280;
        padding-top: 10pt;
    }
</style>
<body>
    <div class="divider">Content after divider</div>
</body>
```

### Example 4: Double line header

```html
<style>
    .formal-header {
        border-top-width: 4pt;
        border-top-style: double;
        border-top-color: #854d0e;
        padding-top: 20pt;
    }
</style>
<body>
    <div class="formal-header">Formal Document</div>
</body>
```

### Example 5: Table header

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
            <tr><th>Product</th></tr>
        </thead>
    </table>
</body>
```

### Example 6: Alert box

```html
<style>
    .alert {
        border-top-width: 4pt;
        border-top-style: solid;
        border-top-color: #f59e0b;
        padding: 15pt;
    }
</style>
<body>
    <div class="alert">Warning message</div>
</body>
```

### Example 7: Card accent

```html
<style>
    .card {
        border: 1pt solid #e5e7eb;
        border-top-width: 4pt;
        border-top-style: solid;
        border-top-color: #2563eb;
        padding: 15pt;
    }
</style>
<body>
    <div class="card">Card content</div>
</body>
```

### Example 8: Invoice section

```html
<style>
    .invoice-total {
        border-top-width: 3pt;
        border-top-style: double;
        border-top-color: #0f172a;
        padding-top: 15pt;
    }
</style>
<body>
    <div class="invoice-total">Total: $550.00</div>
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
    }
</style>
<body>
    <div class="quote">"Design quote"</div>
</body>
```

### Example 10: Navigation active

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
    }
</style>
<body>
    <div class="nav-item active">Dashboard</div>
</body>
```

### Example 11: Certificate

```html
<style>
    .certificate {
        border: 4pt double #854d0e;
        border-top-width: 8pt;
        border-top-style: double;
        padding: 40pt;
    }
</style>
<body>
    <div class="certificate">Certificate text</div>
</body>
```

### Example 12: Pricing tier

```html
<style>
    .pricing-tier {
        border: 2pt solid #d1d5db;
        border-top-width: 5pt;
        border-top-style: solid;
        border-top-color: #2563eb;
        padding: 20pt;
    }
</style>
<body>
    <div class="pricing-tier">Pro Plan</div>
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
        border-top-style: solid;
        border-top-color: #2563eb;
    }
</style>
<body>
    <div class="timeline-entry milestone">Major event</div>
</body>
```

### Example 14: Form section

```html
<style>
    .form-section {
        border-top-width: 2pt;
        border-top-style: dashed;
        border-top-color: #d1d5db;
        padding-top: 20pt;
    }
</style>
<body>
    <div class="form-section">Contact Info</div>
</body>
```

### Example 15: Report header

```html
<style>
    .report-header {
        border-top-width: 6pt;
        border-top-style: solid;
        border-top-color: #1e3a8a;
        padding-top: 25pt;
    }
</style>
<body>
    <div class="report-header">Annual Report</div>
</body>
```

---

## See Also

- [border-top](/reference/cssproperties/css_prop_border-top) - Top border shorthand
- [border-style](/reference/cssproperties/css_prop_border-style) - All border styles
- [border-right-style](/reference/cssproperties/css_prop_border-right-style) - Right border style
- [border-bottom-style](/reference/cssproperties/css_prop_border-bottom-style) - Bottom border style
- [border-left-style](/reference/cssproperties/css_prop_border-left-style) - Left border style
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
