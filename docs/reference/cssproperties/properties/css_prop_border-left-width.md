---
layout: default
title: border-left-width
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-left-width : Left Border Width Property

The `border-left-width` property sets the width of the left border of an element.

## Usage

```css
selector {
    border-left-width: value;
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

- Has no effect unless `border-left-style` is set
- Cannot be negative

---

## Data Binding

The `border-left-width` property supports dynamic values through data binding, allowing left border thickness to be adjusted based on document data at runtime.

### Example 1: Status indicator thickness

```html
<style>
    .alert-box {
        border-left-style: solid;
        padding: 15pt 15pt 15pt 20pt;
        margin: 10pt 0;
    }
</style>
<body>
    <div class="alert-box" style="border-left-width: {{alert.severity === 'critical' ? '8pt' : '5pt'}}; border-left-color: {{alert.color}}; background-color: {{alert.bgColor}}">
        <strong>{{alert.title}}:</strong> {{alert.message}}
    </div>
</body>
```

### Example 2: Priority-based accent bars

```html
<style>
    .task-card {
        border-left-style: solid;
        padding: 15pt 15pt 15pt 20pt;
        margin-bottom: 12pt;
        background-color: white;
    }
</style>
<body>
    <div class="task-card" style="border-left-width: {{task.priority === 'high' ? '6pt' : '3pt'}}; border-left-color: {{task.priorityColor}}">
        <h3>{{task.name}}</h3>
        <p>Priority: {{task.priority}}</p>
    </div>
</body>
```

### Example 3: Navigation active indicator

```html
<style>
    .nav-item {
        border-left-style: solid;
        border-left-color: transparent;
        padding: 10pt 10pt 10pt 15pt;
        margin-bottom: 5pt;
    }
</style>
<body>
    <div class="nav-item" style="border-left-width: {{item.isActive ? '5pt' : '0'}}; border-left-color: {{item.isActive ? '#2563eb' : 'transparent'}}; background-color: {{item.isActive ? '#eff6ff' : 'transparent'}}">
        {{item.label}}
    </div>
</body>
```

---

## Examples

### Example 1: Accent bar

```html
<style>
    .accent {
        border-left-width: 5pt;
        border-left-style: solid;
        border-left-color: #2563eb;
        padding-left: 15pt;
    }
</style>
<body>
    <div class="accent">Important content</div>
</body>
```

### Example 2: Blockquote

```html
<style>
    .blockquote {
        border-left-width: 5pt;
        border-left-style: solid;
        border-left-color: #6366f1;
        padding-left: 20pt;
    }
</style>
<body>
    <div class="blockquote">"Quote text"</div>
</body>
```

### Example 3: Alert indicator

```html
<style>
    .alert {
        border-left-width: 5pt;
        border-left-style: solid;
        padding: 15pt;
    }
    .alert-warning {
        border-left-color: #f59e0b;
    }
</style>
<body>
    <div class="alert alert-warning">Warning message</div>
</body>
```

### Example 4: Callout box

```html
<style>
    .callout {
        border-left-width: 6pt;
        border-left-style: solid;
        border-left-color: #16a34a;
        padding: 15pt 15pt 15pt 20pt;
    }
</style>
<body>
    <div class="callout">Pro tip content</div>
</body>
```

### Example 5: Navigation active

```html
<style>
    .nav-item {
        border-left-width: 4pt;
        border-left-style: solid;
        border-left-color: transparent;
        padding: 10pt 10pt 10pt 15pt;
    }
    .nav-item.active {
        border-left-color: #2563eb;
    }
</style>
<body>
    <div class="nav-item active">Dashboard</div>
</body>
```

### Example 6: Timeline

```html
<style>
    .timeline {
        border-left-width: 3pt;
        border-left-style: solid;
        border-left-color: #e5e7eb;
        padding-left: 25pt;
    }
</style>
<body>
    <div class="timeline">Event timeline</div>
</body>
```

### Example 7: Status card

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
</style>
<body>
    <div class="status-card status-complete">Task complete</div>
</body>
```

### Example 8: Code block

```html
<style>
    .code-block {
        border-left-width: 4pt;
        border-left-style: solid;
        border-left-color: #3b82f6;
        padding: 12pt 12pt 12pt 18pt;
    }
</style>
<body>
    <div class="code-block">function() {}</div>
</body>
```

### Example 9: Definition term

```html
<style>
    .definition {
        border-left-width: 3pt;
        border-left-style: solid;
        border-left-color: #8b5cf6;
        padding-left: 15pt;
    }
</style>
<body>
    <div class="definition">API: Application Programming Interface</div>
</body>
```

### Example 10: Invoice category

```html
<style>
    .invoice-item {
        border-left-width: 4pt;
        border-left-style: solid;
        border-left-color: #e5e7eb;
        padding: 10pt 10pt 10pt 15pt;
    }
    .invoice-item.priority {
        border-left-color: #dc2626;
    }
</style>
<body>
    <div class="invoice-item priority">Urgent service</div>
</body>
```

### Example 11: Form validation

```html
<style>
    .form-field {
        border: 1pt solid #d1d5db;
        border-left-width: 4pt;
        padding: 10pt;
    }
    .form-field.valid {
        border-left-color: #16a34a;
    }
</style>
<body>
    <div class="form-field valid">john@example.com</div>
</body>
```

### Example 12: Certificate

```html
<style>
    .certificate {
        border: 4pt double #854d0e;
        border-left-width: 8pt;
        padding: 40pt;
    }
</style>
<body>
    <div class="certificate">Certificate text</div>
</body>
```

### Example 13: Dashboard widget

```html
<style>
    .widget {
        border: 1pt solid #e5e7eb;
        border-left-width: 5pt;
        padding: 15pt;
    }
    .widget-sales {
        border-left-color: #2563eb;
    }
</style>
<body>
    <div class="widget widget-sales">$50K Revenue</div>
</body>
```

### Example 14: Pricing featured

```html
<style>
    .pricing-plan {
        border: 2pt solid #d1d5db;
        border-left-width: 8pt;
        padding: 25pt;
    }
    .pricing-plan.featured {
        border-left-color: #2563eb;
    }
</style>
<body>
    <div class="pricing-plan featured">Pro Plan</div>
</body>
```

### Example 15: Report section

```html
<style>
    .report-section {
        border-left-width: 5pt;
        border-left-style: solid;
        padding: 15pt 15pt 15pt 20pt;
    }
    .section-financial {
        border-left-color: #16a34a;
    }
</style>
<body>
    <div class="report-section section-financial">Financial data</div>
</body>
```

---

## See Also

- [border-left](/reference/cssproperties/css_prop_border-left) - Left border shorthand
- [border-width](/reference/cssproperties/css_prop_border-width) - All border widths
- [border-top-width](/reference/cssproperties/css_prop_border-top-width) - Top border width
- [border-right-width](/reference/cssproperties/css_prop_border-right-width) - Right border width
- [border-bottom-width](/reference/cssproperties/css_prop_border-bottom-width) - Bottom border width
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
