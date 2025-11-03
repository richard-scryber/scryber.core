---
layout: default
title: border-right-width
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-right-width : Right Border Width Property

The `border-right-width` property sets the width of the right border of an element.

## Usage

```css
selector {
    border-right-width: value;
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

- Has no effect unless `border-right-style` is set
- Cannot be negative

---

## Data Binding

The `border-right-width` property supports dynamic values through data binding, allowing right border thickness to be adjusted based on document data at runtime.

### Example 1: Column dividers with conditional width

```html
<style>
    .table-cell {
        display: table-cell;
        border-right-style: solid;
        border-right-color: #e5e7eb;
        padding: 10pt 15pt;
    }
</style>
<body>
    <div class="table-cell" style="border-right-width: {{cell.isLastColumn ? '0' : '2pt'}}">
        {{cell.content}}
    </div>
</body>
```

### Example 2: Dashboard separators

```html
<style>
    .metric-panel {
        display: table-cell;
        border-right-style: solid;
        border-right-color: white;
        padding: 15pt 20pt;
        text-align: center;
    }
</style>
<body>
    <div class="metric-panel" style="border-right-width: {{metric.showDivider ? '3pt' : '0'}}">
        <div style="font-size: 32pt;">{{metric.value}}</div>
        <div style="font-size: 11pt;">{{metric.label}}</div>
    </div>
</body>
```

### Example 3: Featured pricing tiers

```html
<style>
    .pricing-option {
        display: table-cell;
        border-right-style: solid;
        border-right-color: #d1d5db;
        padding: 25pt;
    }
</style>
<body>
    <div class="pricing-option" style="border-right-width: {{plan.isFeatured ? '4pt' : '2pt'}}; border-right-color: {{plan.isFeatured ? '#2563eb' : '#d1d5db'}}">
        <h3>{{plan.name}}</h3>
        <p>{{plan.price}}</p>
    </div>
</body>
```

---

## Examples

### Example 1: Sidebar divider

```html
<style>
    .sidebar {
        border-right-width: 3pt;
        border-right-style: solid;
        border-right-color: #d1d5db;
        padding-right: 15pt;
    }
</style>
<body>
    <div class="sidebar">Sidebar content</div>
</body>
```

### Example 2: Column separator

```html
<style>
    .column {
        border-right-width: 2pt;
        border-right-style: dashed;
        border-right-color: #cbd5e1;
        padding: 0 15pt;
    }
</style>
<body>
    <div class="column">Column 1</div>
</body>
```

### Example 3: Table cells

```html
<style>
    .data-table td {
        border-right-width: 1pt;
        border-right-style: solid;
        border-right-color: #e5e7eb;
        padding: 10pt;
    }
</style>
<body>
    <table class="data-table">
        <tr><td>Data</td><td>More data</td></tr>
    </table>
</body>
```

### Example 4: Status indicator

```html
<style>
    .status-bar div {
        border-right-width: 2pt;
        border-right-style: solid;
        border-right-color: #e5e7eb;
        padding: 15pt;
    }
</style>
<body>
    <div class="status-bar">
        <div>Sales: 100</div>
    </div>
</body>
```

### Example 5: Inline separators

```html
<style>
    .inline-item {
        border-right-width: 2pt;
        border-right-style: solid;
        border-right-color: #cbd5e1;
        padding: 0 15pt;
        display: inline-block;
    }
</style>
<body>
    <div class="inline-item">Home</div>
    <div class="inline-item">About</div>
</body>
```

### Example 6: Card grid

```html
<style>
    .card {
        border-right-width: 3pt;
        border-right-style: solid;
        border-right-color: #e5e7eb;
        padding: 20pt;
    }
</style>
<body>
    <div class="card">Card content</div>
</body>
```

### Example 7: Invoice columns

```html
<style>
    .invoice-column {
        border-right-width: 1pt;
        border-right-style: solid;
        border-right-color: #d1d5db;
        padding: 10pt 15pt;
    }
</style>
<body>
    <div class="invoice-column">Description</div>
</body>
```

### Example 8: Profile sections

```html
<style>
    .profile-section {
        border-right-width: 2pt;
        border-right-style: dotted;
        border-right-color: #cbd5e1;
        padding: 0 20pt;
    }
</style>
<body>
    <div class="profile-section">Contact info</div>
</body>
```

### Example 9: Pricing comparison

```html
<style>
    .pricing-option {
        border-right-width: 2pt;
        border-right-style: solid;
        border-right-color: #d1d5db;
        padding: 25pt;
    }
    .pricing-option.featured {
        border-right-width: 3pt;
        border-right-color: #2563eb;
    }
</style>
<body>
    <div class="pricing-option featured">Pro Plan</div>
</body>
```

### Example 10: Dashboard metrics

```html
<style>
    .metric-box {
        border-right-width: 3pt;
        border-right-style: solid;
        border-right-color: white;
        padding: 15pt 20pt;
    }
</style>
<body>
    <div class="metric-box">12.5K Users</div>
</body>
```

### Example 11: Certificate layout

```html
<style>
    .cert-main {
        border-right-width: 3pt;
        border-right-style: solid;
        border-right-color: #b45309;
        padding-right: 25pt;
    }
</style>
<body>
    <div class="cert-main">Certificate text</div>
</body>
```

### Example 12: Form columns

```html
<style>
    .form-column {
        border-right-width: 2pt;
        border-right-style: solid;
        border-right-color: #e5e7eb;
        padding: 0 20pt;
    }
</style>
<body>
    <div class="form-column">Personal Details</div>
</body>
```

### Example 13: Navigation breadcrumb

```html
<style>
    .breadcrumb-item {
        border-right-width: 2pt;
        border-right-style: solid;
        border-right-color: #cbd5e1;
        padding: 0 10pt;
        display: inline-block;
    }
</style>
<body>
    <div class="breadcrumb-item">Home</div>
    <div class="breadcrumb-item">Products</div>
</body>
```

### Example 14: Data widget

```html
<style>
    .widget {
        border-right-width: 5pt;
        border-right-style: solid;
        padding: 15pt;
    }
    .widget-sales {
        border-right-color: #2563eb;
    }
</style>
<body>
    <div class="widget widget-sales">$50K Revenue</div>
</body>
```

### Example 15: Timeline marker

```html
<style>
    .timeline-item {
        border-right-width: 6pt;
        border-right-style: solid;
        border-right-color: transparent;
        padding: 15pt 20pt 15pt 0;
    }
    .timeline-item.milestone {
        border-right-color: #2563eb;
    }
</style>
<body>
    <div class="timeline-item milestone">Major milestone</div>
</body>
```

---

## See Also

- [border-right](/reference/cssproperties/css_prop_border-right) - Right border shorthand
- [border-width](/reference/cssproperties/css_prop_border-width) - All border widths
- [border-top-width](/reference/cssproperties/css_prop_border-top-width) - Top border width
- [border-bottom-width](/reference/cssproperties/css_prop_border-bottom-width) - Bottom border width
- [border-left-width](/reference/cssproperties/css_prop_border-left-width) - Left border width
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
