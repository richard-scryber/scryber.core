---
layout: default
title: border-bottom-color
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-bottom-color : Bottom Border Color Property

The `border-bottom-color` property sets the color of the bottom border.

## Usage

```css
selector {
    border-bottom-color: value;
}
```

---

## Supported Values

### Named Colors
Standard CSS color names: `red`, `blue`, `green`, etc.

### Hexadecimal Colors
- Short form: `#RGB`
- Long form: `#RRGGBB`

### RGB/RGBA Functions
- `rgb(r, g, b)` - RGB values 0-255
- `rgba(r, g, b, a)` - Alpha 0.0-1.0

---

## Supported Elements

All elements that support borders.

---

## Notes

- Has no effect unless `border-bottom-style` is set
- Defaults to element's text color if not specified
- RGBA enables semi-transparent borders

---

## Data Binding

The `border-bottom-color` property supports dynamic values through data binding, allowing bottom border colors to be customized based on document data at runtime.

### Example 1: Heading underline colors

```html
<style>
    .heading {
        border-bottom-width: 3pt;
        border-bottom-style: solid;
        padding-bottom: 10pt;
        margin-bottom: 15pt;
    }
</style>
<body>
    <h2 class="heading" style="border-bottom-color: {{heading.accentColor}}">
        {{heading.text}}
    </h2>
</body>
```

Data context:
```json
{
    "heading": {
        "text": "Executive Summary",
        "accentColor": "#2563eb"
    }
}
```

### Example 2: Table row separators with status

```html
<style>
    .table-row {
        border-bottom-width: 2pt;
        border-bottom-style: solid;
        padding: 8pt 0;
    }
</style>
<body>
    <div class="table-row" style="border-bottom-color: {{row.isSummary ? '#0f172a' : '#e5e7eb'}}">
        <p>{{row.label}}: {{row.value}}</p>
    </div>
</body>
```

### Example 3: Card accent colors

```html
<style>
    .card {
        border: 1pt solid #e5e7eb;
        border-bottom-width: 4pt;
        border-bottom-style: solid;
        padding: 15pt;
        margin-bottom: 12pt;
    }
</style>
<body>
    <div class="card" style="border-bottom-color: {{card.categoryColor}}">
        <h3>{{card.title}}</h3>
        <p>{{card.content}}</p>
    </div>
</body>
```

---

## Examples

### Example 1: Named color

```html
<style>
    .box {
        border-bottom-width: 3pt;
        border-bottom-style: solid;
        border-bottom-color: blue;
        padding: 15pt;
    }
</style>
<body>
    <div class="box">Content with blue bottom border</div>
</body>
```

### Example 2: Hexadecimal color

```html
<style>
    .accent {
        border-bottom-width: 4pt;
        border-bottom-style: solid;
        border-bottom-color: #2563eb;
        padding: 15pt;
    }
</style>
<body>
    <div class="accent">Content with hex color bottom border</div>
</body>
```

### Example 3: RGB color

```html
<style>
    .custom {
        border-bottom-width: 2pt;
        border-bottom-style: solid;
        border-bottom-color: rgb(37, 99, 235);
        padding: 12pt;
    }
</style>
<body>
    <div class="custom">Content with RGB bottom border</div>
</body>
```

### Example 4: RGBA transparency

```html
<style>
    .transparent {
        border-bottom-width: 3pt;
        border-bottom-style: solid;
        border-bottom-color: rgba(37, 99, 235, 0.5);
        padding: 15pt;
    }
</style>
<body>
    <div class="transparent">Semi-transparent bottom border</div>
</body>
```

### Example 5: Alert colors

```html
<style>
    .alert-success {
        border-bottom-width: 5pt;
        border-bottom-style: solid;
        border-bottom-color: #16a34a;
        background-color: #dcfce7;
        padding: 15pt;
    }
</style>
<body>
    <div class="alert-success">Success message</div>
</body>
```

### Example 6: Warning color

```html
<style>
    .warning {
        border-bottom-width: 4pt;
        border-bottom-style: solid;
        border-bottom-color: #f59e0b;
        padding: 12pt;
    }
</style>
<body>
    <div class="warning">Warning content</div>
</body>
```

### Example 7: Table styling

```html
<style>
    .data-table th {
        border-bottom-width: 2pt;
        border-bottom-style: solid;
        border-bottom-color: #3b82f6;
        padding: 10pt;
    }
</style>
<body>
    <table class="data-table">
        <thead>
            <tr><th>Header</th></tr>
        </thead>
    </table>
</body>
```

### Example 8: Form validation

```html
<style>
    .form-field {
        border: 1pt solid #d1d5db;
        padding: 10pt;
    }
    .form-field.valid {
        border-bottom-width: 4pt;
        border-bottom-color: #16a34a;
    }
    .form-field.invalid {
        border-bottom-width: 4pt;
        border-bottom-color: #dc2626;
    }
</style>
<body>
    <div class="form-field valid">Valid input</div>
</body>
```

### Example 9: Quote accent

```html
<style>
    .quote {
        border-bottom-width: 4pt;
        border-bottom-style: solid;
        border-bottom-color: #6366f1;
        padding: 15pt;
        font-style: italic;
    }
</style>
<body>
    <div class="quote">"Inspiring quote"</div>
</body>
```

### Example 10: Navigation active

```html
<style>
    .nav-item {
        border-bottom-width: 4pt;
        border-bottom-style: solid;
        border-bottom-color: transparent;
        padding: 10pt;
    }
    .nav-item.active {
        border-bottom-color: #2563eb;
    }
</style>
<body>
    <div class="nav-item active">Dashboard</div>
</body>
```

### Example 11: Card categories

```html
<style>
    .card {
        border: 1pt solid #e5e7eb;
        border-bottom-width: 4pt;
        padding: 15pt;
    }
    .card-blue {
        border-bottom-color: #2563eb;
    }
    .card-green {
        border-bottom-color: #16a34a;
    }
</style>
<body>
    <div class="card card-blue">Blue category</div>
</body>
```

### Example 12: Certificate

```html
<style>
    .certificate {
        border: 4pt double #854d0e;
        border-bottom-width: 8pt;
        border-bottom-color: #b45309;
        padding: 40pt;
    }
</style>
<body>
    <div class="certificate">Certificate text</div>
</body>
```

### Example 13: Pricing tiers

```html
<style>
    .pricing-tier {
        border: 2pt solid #d1d5db;
        border-bottom-width: 5pt;
        padding: 20pt;
    }
    .pricing-tier.basic {
        border-bottom-color: #6b7280;
    }
    .pricing-tier.pro {
        border-bottom-color: #2563eb;
    }
</style>
<body>
    <div class="pricing-tier pro">Pro Plan</div>
</body>
```

### Example 14: Status indicators

```html
<style>
    .status-card {
        border-bottom-width: 6pt;
        border-bottom-style: solid;
        padding: 15pt;
    }
    .status-complete {
        border-bottom-color: #16a34a;
    }
    .status-pending {
        border-bottom-color: #f59e0b;
    }
    .status-error {
        border-bottom-color: #dc2626;
    }
</style>
<body>
    <div class="status-card status-complete">Complete</div>
</body>
```

### Example 15: Dashboard widgets

```html
<style>
    .widget {
        border: 1pt solid #e5e7eb;
        border-bottom-width: 5pt;
        border-bottom-style: solid;
        padding: 15pt;
    }
    .widget-sales {
        border-bottom-color: #2563eb;
    }
    .widget-revenue {
        border-bottom-color: #16a34a;
    }
    .widget-users {
        border-bottom-color: #f59e0b;
    }
</style>
<body>
    <div class="widget widget-sales">Sales Widget</div>
</body>
```

---

## See Also

- [border-bottom](/reference/cssproperties/css_prop_border-bottom) - Bottom border shorthand
- [border-color](/reference/cssproperties/css_prop_border-color) - All border colors
- [border-bottom-width](/reference/cssproperties/css_prop_border-bottom-width) - Bottom border width
- [border-bottom-style](/reference/cssproperties/css_prop_border-bottom-style) - Bottom border style
- [color](/reference/cssproperties/css_prop_color) - Text color
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
