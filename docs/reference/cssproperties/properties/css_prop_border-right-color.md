---
layout: default
title: border-right-color
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-right-color : Right Border Color Property

The `border-right-color` property sets the color of the right border.

## Usage

```css
selector {
    border-right-color: value;
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

- Has no effect unless `border-right-style` is set
- Defaults to element's text color if not specified
- RGBA enables semi-transparent borders

---

## Data Binding

The `border-right-color` property supports dynamic values through data binding, allowing right border colors to be customized based on document data at runtime.

### Example 1: Column separators with status colors

```html
<style>
    .table-cell {
        display: table-cell;
        border-right-width: 2pt;
        border-right-style: solid;
        padding: 10pt 15pt;
    }
</style>
<body>
    <div class="table-cell" style="border-right-color: {{cell.isLast ? 'transparent' : '#e5e7eb'}}">
        {{cell.content}}
    </div>
</body>
```

### Example 2: Dashboard metric dividers

```html
<style>
    .metric-box {
        display: table-cell;
        border-right-width: 3pt;
        border-right-style: solid;
        padding: 15pt 20pt;
        text-align: center;
    }
</style>
<body>
    <div class="metric-box" style="border-right-color: {{metric.dividerColor}}">
        <div style="font-size: 32pt; font-weight: bold;">{{metric.value}}</div>
        <div style="font-size: 11pt; color: #6b7280;">{{metric.label}}</div>
    </div>
</body>
```

### Example 3: Sidebar navigation indicators

```html
<style>
    .nav-item {
        border-right-width: 4pt;
        border-right-style: solid;
        padding: 10pt 15pt;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="nav-item" style="border-right-color: {{item.isActive ? '#2563eb' : 'transparent'}}; background-color: {{item.isActive ? '#eff6ff' : 'transparent'}}">
        {{item.label}}
    </div>
</body>
```

---

## Examples

### Example 1: Named color

```html
<style>
    .box {
        border-right-width: 3pt;
        border-right-style: solid;
        border-right-color: blue;
        padding: 15pt;
    }
</style>
<body>
    <div class="box">Content with blue right border</div>
</body>
```

### Example 2: Hexadecimal color

```html
<style>
    .accent {
        border-right-width: 4pt;
        border-right-style: solid;
        border-right-color: #2563eb;
        padding: 15pt;
    }
</style>
<body>
    <div class="accent">Content with hex color right border</div>
</body>
```

### Example 3: RGB color

```html
<style>
    .custom {
        border-right-width: 2pt;
        border-right-style: solid;
        border-right-color: rgb(37, 99, 235);
        padding: 12pt;
    }
</style>
<body>
    <div class="custom">Content with RGB right border</div>
</body>
```

### Example 4: RGBA transparency

```html
<style>
    .transparent {
        border-right-width: 3pt;
        border-right-style: solid;
        border-right-color: rgba(37, 99, 235, 0.5);
        padding: 15pt;
    }
</style>
<body>
    <div class="transparent">Semi-transparent right border</div>
</body>
```

### Example 5: Alert colors

```html
<style>
    .alert-success {
        border-right-width: 5pt;
        border-right-style: solid;
        border-right-color: #16a34a;
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
        border-right-width: 4pt;
        border-right-style: solid;
        border-right-color: #f59e0b;
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
        border-right-width: 2pt;
        border-right-style: solid;
        border-right-color: #3b82f6;
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
        border-right-width: 4pt;
        border-right-color: #16a34a;
    }
    .form-field.invalid {
        border-right-width: 4pt;
        border-right-color: #dc2626;
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
        border-right-width: 4pt;
        border-right-style: solid;
        border-right-color: #6366f1;
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
        border-right-width: 4pt;
        border-right-style: solid;
        border-right-color: transparent;
        padding: 10pt;
    }
    .nav-item.active {
        border-right-color: #2563eb;
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
        border-right-width: 4pt;
        padding: 15pt;
    }
    .card-blue {
        border-right-color: #2563eb;
    }
    .card-green {
        border-right-color: #16a34a;
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
        border-right-width: 8pt;
        border-right-color: #b45309;
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
        border-right-width: 5pt;
        padding: 20pt;
    }
    .pricing-tier.basic {
        border-right-color: #6b7280;
    }
    .pricing-tier.pro {
        border-right-color: #2563eb;
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
        border-right-width: 6pt;
        border-right-style: solid;
        padding: 15pt;
    }
    .status-complete {
        border-right-color: #16a34a;
    }
    .status-pending {
        border-right-color: #f59e0b;
    }
    .status-error {
        border-right-color: #dc2626;
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
        border-right-width: 5pt;
        border-right-style: solid;
        padding: 15pt;
    }
    .widget-sales {
        border-right-color: #2563eb;
    }
    .widget-revenue {
        border-right-color: #16a34a;
    }
    .widget-users {
        border-right-color: #f59e0b;
    }
</style>
<body>
    <div class="widget widget-sales">Sales Widget</div>
</body>
```

---

## See Also

- [border-right](/reference/cssproperties/css_prop_border-right) - Right border shorthand
- [border-color](/reference/cssproperties/css_prop_border-color) - All border colors
- [border-right-width](/reference/cssproperties/css_prop_border-right-width) - Right border width
- [border-right-style](/reference/cssproperties/css_prop_border-right-style) - Right border style
- [color](/reference/cssproperties/css_prop_color) - Text color
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
