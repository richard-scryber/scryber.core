---
layout: default
title: border-left-color
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-left-color : Left Border Color Property

The `border-left-color` property sets the color of the left border.

## Usage

```css
selector {
    border-left-color: value;
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

- Has no effect unless `border-left-style` is set
- Defaults to element's text color if not specified
- RGBA enables semi-transparent borders

---

## Data Binding

The `border-left-color` property supports dynamic values through data binding, allowing left border colors to be customized based on document data at runtime.

### Example 1: Alert severity colors

```html
<style>
    .alert-box {
        border-left-width: 5pt;
        border-left-style: solid;
        padding: 15pt;
        margin: 10pt 0;
    }
</style>
<body>
    <div class="alert-box" style="border-left-color: {{severity.color}}; background-color: {{severity.bgColor}}">
        <strong>{{severity.level}}:</strong> {{message}}
    </div>
</body>
```

Data context:
```json
{
    "severity": {
        "level": "Error",
        "color": "#dc2626",
        "bgColor": "#fee2e2"
    },
    "message": "Unable to process request"
}
```

### Example 2: Status-based accent bars

```html
<style>
    .status-card {
        border-left-width: 6pt;
        border-left-style: solid;
        padding: 15pt 15pt 15pt 20pt;
        margin-bottom: 12pt;
        background-color: white;
    }
</style>
<body>
    <div class="status-card" style="border-left-color: {{status.indicatorColor}}">
        <h3>{{task.name}}</h3>
        <p>Status: {{status.label}}</p>
    </div>
</body>
```

### Example 3: Category indicators

```html
<style>
    .item-card {
        border-left-width: 4pt;
        border-left-style: solid;
        padding: 12pt 12pt 12pt 18pt;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="item-card" style="border-left-color: {{category.color}}; background-color: {{category.bgColor}}">
        <h4>{{item.title}}</h4>
        <p>Category: {{category.name}}</p>
    </div>
</body>
```

---

## Examples

### Example 1: Named color

```html
<style>
    .box {
        border-left-width: 3pt;
        border-left-style: solid;
        border-left-color: blue;
        padding: 15pt;
    }
</style>
<body>
    <div class="box">Content with blue left border</div>
</body>
```

### Example 2: Hexadecimal color

```html
<style>
    .accent {
        border-left-width: 4pt;
        border-left-style: solid;
        border-left-color: #2563eb;
        padding: 15pt;
    }
</style>
<body>
    <div class="accent">Content with hex color left border</div>
</body>
```

### Example 3: RGB color

```html
<style>
    .custom {
        border-left-width: 2pt;
        border-left-style: solid;
        border-left-color: rgb(37, 99, 235);
        padding: 12pt;
    }
</style>
<body>
    <div class="custom">Content with RGB left border</div>
</body>
```

### Example 4: RGBA transparency

```html
<style>
    .transparent {
        border-left-width: 3pt;
        border-left-style: solid;
        border-left-color: rgba(37, 99, 235, 0.5);
        padding: 15pt;
    }
</style>
<body>
    <div class="transparent">Semi-transparent left border</div>
</body>
```

### Example 5: Alert colors

```html
<style>
    .alert-success {
        border-left-width: 5pt;
        border-left-style: solid;
        border-left-color: #16a34a;
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
        border-left-width: 4pt;
        border-left-style: solid;
        border-left-color: #f59e0b;
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
        border-left-width: 2pt;
        border-left-style: solid;
        border-left-color: #3b82f6;
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
        border-left-width: 4pt;
        border-left-color: #16a34a;
    }
    .form-field.invalid {
        border-left-width: 4pt;
        border-left-color: #dc2626;
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
        border-left-width: 4pt;
        border-left-style: solid;
        border-left-color: #6366f1;
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
        border-left-width: 4pt;
        border-left-style: solid;
        border-left-color: transparent;
        padding: 10pt;
    }
    .nav-item.active {
        border-left-color: #2563eb;
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
        border-left-width: 4pt;
        padding: 15pt;
    }
    .card-blue {
        border-left-color: #2563eb;
    }
    .card-green {
        border-left-color: #16a34a;
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
        border-left-width: 8pt;
        border-left-color: #b45309;
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
        border-left-width: 5pt;
        padding: 20pt;
    }
    .pricing-tier.basic {
        border-left-color: #6b7280;
    }
    .pricing-tier.pro {
        border-left-color: #2563eb;
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
        border-left-width: 6pt;
        border-left-style: solid;
        padding: 15pt;
    }
    .status-complete {
        border-left-color: #16a34a;
    }
    .status-pending {
        border-left-color: #f59e0b;
    }
    .status-error {
        border-left-color: #dc2626;
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
        border-left-width: 5pt;
        border-left-style: solid;
        padding: 15pt;
    }
    .widget-sales {
        border-left-color: #2563eb;
    }
    .widget-revenue {
        border-left-color: #16a34a;
    }
    .widget-users {
        border-left-color: #f59e0b;
    }
</style>
<body>
    <div class="widget widget-sales">Sales Widget</div>
</body>
```

---

## See Also

- [border-left](/reference/cssproperties/css_prop_border-left) - Left border shorthand
- [border-color](/reference/cssproperties/css_prop_border-color) - All border colors
- [border-left-width](/reference/cssproperties/css_prop_border-left-width) - Left border width
- [border-left-style](/reference/cssproperties/css_prop_border-left-style) - Left border style
- [color](/reference/cssproperties/css_prop_color) - Text color
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
