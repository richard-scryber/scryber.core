---
layout: default
title: border-bottom-width
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-bottom-width : Bottom Border Width Property

The `border-bottom-width` property sets the width of the bottom border of an element.

## Usage

```css
selector {
    border-bottom-width: value;
}
```

---

## Supported Values

### Named Widths
- `thin`, `medium`, `thick`

### Length Values
Any valid length unit: `1pt`, `2px`, `0.5mm`, etc.

---

## Supported Elements

All elements that support borders.

---

## Notes

- Has no effect unless `border-bottom-style` is set
- Cannot be negative

---

## Data Binding

The `border-bottom-width` property supports dynamic values through data binding, allowing bottom border thickness to be adjusted based on document data at runtime.

### Example 1: Dynamic heading underlines

```html
<style>
    .heading {
        border-bottom-style: solid;
        border-bottom-color: #2563eb;
        padding-bottom: 10pt;
        margin-bottom: 15pt;
    }
</style>
<body>
    <h2 class="heading" style="border-bottom-width: {{heading.level === 1 ? '4pt' : '2pt'}}">
        {{heading.text}}
    </h2>
</body>
```

### Example 2: Table row emphasis

```html
<style>
    .table-row {
        border-bottom-style: solid;
        border-bottom-color: #e5e7eb;
        padding: 8pt 0;
    }
</style>
<body>
    <div class="table-row" style="border-bottom-width: {{row.isSummary ? '3pt' : '1pt'}}">
        <p>{{row.label}}: {{row.value}}</p>
    </div>
</body>
```

### Example 3: Navigation active state

```html
<style>
    .nav-item {
        border-bottom-style: solid;
        border-bottom-color: transparent;
        padding: 12pt 0;
    }
</style>
<body>
    <div class="nav-item" style="border-bottom-width: {{nav.isActive ? '3pt' : '1pt'}}; border-bottom-color: {{nav.isActive ? '#2563eb' : '#e5e7eb'}}">
        {{nav.label}}
    </div>
</body>
```

---

## Examples

### Example 1: Heading underline

```html
<style>
    .heading {
        border-bottom-width: 3pt;
        border-bottom-style: solid;
        border-bottom-color: #2563eb;
        padding-bottom: 10pt;
    }
</style>
<body>
    <h2 class="heading">Section Title</h2>
</body>
```

### Example 2: Table rows

```html
<style>
    .data-table td {
        border-bottom-width: 1pt;
        border-bottom-style: solid;
        border-bottom-color: #e5e7eb;
        padding: 8pt;
    }
</style>
<body>
    <table class="data-table">
        <tr><td>Data</td></tr>
    </table>
</body>
```

### Example 3: Form fields

```html
<style>
    .form-input {
        border-bottom-width: 2pt;
        border-bottom-style: solid;
        border-bottom-color: #9ca3af;
        padding: 5pt 0;
    }
</style>
<body>
    <div class="form-input">john@example.com</div>
</body>
```

### Example 4: Section separator

```html
<style>
    .section {
        border-bottom-width: 2pt;
        border-bottom-style: dashed;
        border-bottom-color: #cbd5e1;
        padding-bottom: 20pt;
    }
</style>
<body>
    <div class="section">Section content</div>
</body>
```

### Example 5: Quote accent

```html
<style>
    .quote {
        border-bottom-width: 4pt;
        border-bottom-style: solid;
        border-bottom-color: #6366f1;
        padding-bottom: 15pt;
    }
</style>
<body>
    <div class="quote">"Innovation is key"</div>
</body>
```

### Example 6: Navigation items

```html
<style>
    .nav-item {
        border-bottom-width: 1pt;
        border-bottom-style: solid;
        border-bottom-color: #e5e7eb;
        padding: 12pt 0;
    }
    .nav-item.active {
        border-bottom-width: 3pt;
        border-bottom-color: #2563eb;
    }
</style>
<body>
    <div class="nav-item active">Dashboard</div>
</body>
```

### Example 7: Card accent

```html
<style>
    .card {
        border: 1pt solid #e5e7eb;
        border-bottom-width: 4pt;
        border-bottom-color: #2563eb;
        padding: 15pt;
    }
</style>
<body>
    <div class="card">Featured card</div>
</body>
```

### Example 8: Invoice subtotal

```html
<style>
    .invoice-subtotal {
        border-bottom-width: 2pt;
        border-bottom-style: solid;
        border-bottom-color: #6b7280;
        padding: 10pt 0;
    }
</style>
<body>
    <div class="invoice-subtotal">Subtotal: $500</div>
</body>
```

### Example 9: Alert bottom

```html
<style>
    .alert {
        border-bottom-width: 5pt;
        border-bottom-style: solid;
        padding: 15pt;
    }
    .alert-success {
        border-bottom-color: #16a34a;
    }
</style>
<body>
    <div class="alert alert-success">Success message</div>
</body>
```

### Example 10: Pricing tiers

```html
<style>
    .pricing-tier {
        border: 2pt solid #d1d5db;
        border-bottom-width: 5pt;
        border-bottom-color: #6b7280;
        padding: 20pt;
    }
    .pricing-tier.featured {
        border-bottom-width: 8pt;
        border-bottom-color: #2563eb;
    }
</style>
<body>
    <div class="pricing-tier featured">Pro Plan</div>
</body>
```

### Example 11: Certificate signature

```html
<style>
    .cert-signature {
        border-bottom-width: 2pt;
        border-bottom-style: solid;
        border-bottom-color: #854d0e;
        padding-bottom: 5pt;
    }
</style>
<body>
    <div class="cert-signature">Signature line</div>
</body>
```

### Example 12: Dashboard footer

```html
<style>
    .panel-footer {
        border-bottom-width: 3pt;
        border-bottom-style: solid;
        border-bottom-color: #2563eb;
        padding: 10pt 15pt;
    }
</style>
<body>
    <div class="panel-footer">Updated: Oct 13</div>
</body>
```

### Example 13: Timeline entry

```html
<style>
    .timeline-entry {
        border-bottom-width: 2pt;
        border-bottom-style: solid;
        border-bottom-color: #e5e7eb;
        padding: 15pt 0;
    }
    .timeline-entry.milestone {
        border-bottom-width: 4pt;
        border-bottom-color: #2563eb;
    }
</style>
<body>
    <div class="timeline-entry milestone">Major event</div>
</body>
```

### Example 14: Profile section

```html
<style>
    .profile-section {
        border-bottom-width: 2pt;
        border-bottom-style: dashed;
        border-bottom-color: #cbd5e1;
        padding: 15pt 0;
    }
</style>
<body>
    <div class="profile-section">About section</div>
</body>
```

### Example 15: Report summary

```html
<style>
    .summary-footer {
        border-bottom-width: 4pt;
        border-bottom-style: double;
        border-bottom-color: #1e293b;
        padding: 15pt 0;
    }
</style>
<body>
    <div class="summary-footer">Net Income: $150K</div>
</body>
```

---

## See Also

- [border-bottom](/reference/cssproperties/css_prop_border-bottom) - Bottom border shorthand
- [border-width](/reference/cssproperties/css_prop_border-width) - All border widths
- [border-top-width](/reference/cssproperties/css_prop_border-top-width) - Top border width
- [border-right-width](/reference/cssproperties/css_prop_border-right-width) - Right border width
- [border-left-width](/reference/cssproperties/css_prop_border-left-width) - Left border width
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
