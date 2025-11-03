---
layout: default
title: border-top-color
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-top-color : Top Border Color Property

The `border-top-color` property sets the color of the top border.

## Usage

```css
selector {
    border-top-color: value;
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

- Has no effect unless `border-top-style` is set
- Defaults to element's text color if not specified
- RGBA enables semi-transparent borders

---

## Data Binding

The `border-top-color` property supports dynamic values through data binding, allowing top border colors to be customized based on document data at runtime.

### Example 1: Status-based colors

```html
<style>
    .section-header {
        border-top-width: 4pt;
        border-top-style: solid;
        padding-top: 15pt;
        font-size: 18pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="section-header" style="border-top-color: {{section.statusColor}}">
        <h2>{{section.title}}</h2>
        <p>Status: {{section.status}}</p>
    </div>
</body>
```

Data context:
```json
{
    "section": {
        "title": "Project Overview",
        "status": "Active",
        "statusColor": "#16a34a"
    }
}
```

### Example 2: Priority-based colors

```html
<style>
    .task-header {
        border-top-width: 3pt;
        border-top-style: solid;
        padding-top: 12pt;
        margin-bottom: 10pt;
    }
</style>
<body>
    <div class="task-header" style="border-top-color: {{priority.color}}">
        <h3>{{task.name}}</h3>
        <p>Priority: {{priority.level}}</p>
    </div>
</body>
```

### Example 3: Conditional alert colors

```html
<style>
    .alert-header {
        border-top-width: 5pt;
        border-top-style: solid;
        padding: 15pt;
        margin: 10pt 0;
    }
</style>
<body>
    <div class="alert-header" style="border-top-color: {{alert.severity === 'error' ? '#dc2626' : (alert.severity === 'warning' ? '#f59e0b' : '#2563eb')}}; background-color: {{alert.bgColor}}">
        <strong>{{alert.title}}:</strong> {{alert.message}}
    </div>
</body>
```

---

## Examples

### Example 1: Named color

```html
<style>
    .box {
        border-top-width: 3pt;
        border-top-style: solid;
        border-top-color: blue;
        padding: 15pt;
    }
</style>
<body>
    <div class="box">Content with blue top border</div>
</body>
```

### Example 2: Hexadecimal color

```html
<style>
    .accent {
        border-top-width: 4pt;
        border-top-style: solid;
        border-top-color: #2563eb;
        padding: 15pt;
    }
</style>
<body>
    <div class="accent">Content with hex color top border</div>
</body>
```

### Example 3: RGB color

```html
<style>
    .custom {
        border-top-width: 2pt;
        border-top-style: solid;
        border-top-color: rgb(37, 99, 235);
        padding: 12pt;
    }
</style>
<body>
    <div class="custom">Content with RGB top border</div>
</body>
```

### Example 4: RGBA transparency

```html
<style>
    .transparent {
        border-top-width: 3pt;
        border-top-style: solid;
        border-top-color: rgba(37, 99, 235, 0.5);
        padding: 15pt;
    }
</style>
<body>
    <div class="transparent">Semi-transparent top border</div>
</body>
```

### Example 5: Alert colors

```html
<style>
    .alert-success {
        border-top-width: 5pt;
        border-top-style: solid;
        border-top-color: #16a34a;
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
        border-top-width: 4pt;
        border-top-style: solid;
        border-top-color: #f59e0b;
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
        border-top-width: 2pt;
        border-top-style: solid;
        border-top-color: #3b82f6;
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
        border-top-width: 4pt;
        border-top-color: #16a34a;
    }
    .form-field.invalid {
        border-top-width: 4pt;
        border-top-color: #dc2626;
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
        border-top-width: 4pt;
        border-top-style: solid;
        border-top-color: #6366f1;
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
        border-top-width: 4pt;
        border-top-style: solid;
        border-top-color: transparent;
        padding: 10pt;
    }
    .nav-item.active {
        border-top-color: #2563eb;
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
        border-top-width: 4pt;
        padding: 15pt;
    }
    .card-blue {
        border-top-color: #2563eb;
    }
    .card-green {
        border-top-color: #16a34a;
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
        border-top-width: 8pt;
        border-top-color: #b45309;
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
        border-top-width: 5pt;
        padding: 20pt;
    }
    .pricing-tier.basic {
        border-top-color: #6b7280;
    }
    .pricing-tier.pro {
        border-top-color: #2563eb;
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
        border-top-width: 6pt;
        border-top-style: solid;
        padding: 15pt;
    }
    .status-complete {
        border-top-color: #16a34a;
    }
    .status-pending {
        border-top-color: #f59e0b;
    }
    .status-error {
        border-top-color: #dc2626;
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
        border-top-width: 5pt;
        border-top-style: solid;
        padding: 15pt;
    }
    .widget-sales {
        border-top-color: #2563eb;
    }
    .widget-revenue {
        border-top-color: #16a34a;
    }
    .widget-users {
        border-top-color: #f59e0b;
    }
</style>
<body>
    <div class="widget widget-sales">Sales Widget</div>
</body>
```

---

## See Also

- [border-top](/reference/cssproperties/css_prop_border-top) - Top border shorthand
- [border-color](/reference/cssproperties/css_prop_border-color) - All border colors
- [border-top-width](/reference/cssproperties/css_prop_border-top-width) - Top border width
- [border-top-style](/reference/cssproperties/css_prop_border-top-style) - Top border style
- [color](/reference/cssproperties/css_prop_color) - Text color
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
