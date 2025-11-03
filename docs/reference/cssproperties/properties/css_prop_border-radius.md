---
layout: default
title: border-radius
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# border-radius : Border Radius Property

The `border-radius` property rounds the corners of an element's border. This property creates smooth, curved corners instead of sharp right angles, enhancing the visual design of PDF documents with modern, polished appearances.

## Usage

```css
selector {
    border-radius: value;
}
```

The border-radius property accepts one to four length values to define the curvature of element corners.

---

## Supported Values

### Length Values
Any valid length unit including:
- Points: `5pt`, `10pt`, `15pt`
- Pixels: `5px`, `10px`, `15px`
- Millimeters: `2mm`, `5mm`
- Percentages: `10%`, `50%` (relative to element dimensions)

### Multiple Values
- **One value**: Applies to all four corners
- **Two values**: First for top-left/bottom-right, second for top-right/bottom-left
- **Three values**: First for top-left, second for top-right/bottom-left, third for bottom-right
- **Four values**: Top-left, top-right, bottom-right, bottom-left (clockwise from top-left)

### Special Values
- `0` - No rounding (default, sharp corners)
- `50%` - Creates circular or pill shapes for equal-dimension elements

---

## Supported Elements

The `border-radius` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables and table cells (`<td>`, `<th>`)
- Images (`<img>`)
- Buttons and form elements
- All container elements

---

## Notes

- Border radius creates curved corners by defining the radius of the curve
- Works with or without visible borders
- Affects the element's background and content clipping
- Percentage values are relative to the element's dimensions
- Large radius values create more pronounced curves
- A 50% radius on square elements creates perfect circles
- Border radius can be combined with borders, backgrounds, and padding
- Overflow content may be clipped by rounded corners

---

## Data Binding

The `border-radius` property supports dynamic values through data binding, allowing corner rounding to be customized based on document data at runtime.

### Example 1: Dynamic badge styles

```html
<style>
    .badge {
        border: 1pt solid;
        padding: 4pt 12pt;
        font-size: 10pt;
        font-weight: bold;
        display: inline-block;
    }
</style>
<body>
    <span class="badge" style="border-radius: {{badge.radius}}; border-color: {{badge.color}}; background-color: {{badge.bgColor}}; color: white;">
        {{badge.label}}
    </span>
</body>
```

Data context:
```json
{
    "badge": {
        "label": "Premium",
        "radius": "12pt",
        "color": "#7c3aed",
        "bgColor": "#7c3aed"
    }
}
```

### Example 2: Card styles based on type

```html
<style>
    .info-card {
        border: 2pt solid #e5e7eb;
        padding: 15pt;
        margin-bottom: 10pt;
        background-color: white;
    }
</style>
<body>
    <div class="info-card" style="border-radius: {{card.borderRadius}}">
        <h3>{{card.title}}</h3>
        <p>{{card.content}}</p>
    </div>
</body>
```

Data context:
```json
{
    "card": {
        "title": "Product Feature",
        "content": "Advanced analytics and reporting",
        "borderRadius": "8pt"
    }
}
```

### Example 3: Conditional rounded corners

```html
<style>
    .alert-panel {
        border: 2pt solid;
        padding: 12pt;
        margin: 10pt 0;
    }
</style>
<body>
    <div class="alert-panel" style="border-radius: {{alert.isFeatured ? '10pt' : '4pt'}}; border-color: {{alert.color}}; background-color: {{alert.bgColor}}">
        <strong>{{alert.title}}</strong>
        <p>{{alert.message}}</p>
    </div>
</body>
```

---

## Examples

### Example 1: Uniform rounded corners

```html
<style>
    .rounded-box {
        border: 2pt solid #2563eb;
        border-radius: 8pt;
        padding: 15pt;
        background-color: #eff6ff;
    }
</style>
<body>
    <div class="rounded-box">
        <p>Box with 8pt rounded corners on all sides</p>
    </div>
</body>
```

### Example 2: Subtle rounding for cards

```html
<style>
    .card {
        border: 1pt solid #e5e7eb;
        border-radius: 4pt;
        padding: 15pt;
        background-color: white;
        margin-bottom: 12pt;
    }
    .card-title {
        font-size: 16pt;
        font-weight: bold;
        margin-bottom: 8pt;
    }
</style>
<body>
    <div class="card">
        <h3 class="card-title">Product Feature</h3>
        <p>Subtle rounded corners create a modern card appearance.</p>
    </div>
</body>
```

### Example 3: Pill-shaped button

```html
<style>
    .pill-button {
        border: 2pt solid #2563eb;
        border-radius: 25pt;
        background-color: #2563eb;
        color: white;
        padding: 10pt 25pt;
        text-align: center;
        font-weight: bold;
        display: inline-block;
    }
</style>
<body>
    <div class="pill-button">Click Here</div>
</body>
```

### Example 4: Circular avatar placeholder

```html
<style>
    .avatar {
        border: 3pt solid #6366f1;
        border-radius: 50%;
        width: 80pt;
        height: 80pt;
        background-color: #eef2ff;
        text-align: center;
        line-height: 80pt;
        font-size: 32pt;
        font-weight: bold;
        color: #6366f1;
    }
</style>
<body>
    <div class="avatar">JD</div>
</body>
```

### Example 5: Different radius per corner

```html
<style>
    .custom-corners {
        border: 2pt solid #8b5cf6;
        border-radius: 0 20pt 0 20pt;
        padding: 15pt;
        background-color: #faf5ff;
    }
</style>
<body>
    <div class="custom-corners">
        <p>Rounded top-right and bottom-left corners only</p>
    </div>
</body>
```

### Example 6: Alert boxes with rounded corners

```html
<style>
    .alert {
        border: 2pt solid;
        border-radius: 6pt;
        padding: 12pt;
        margin: 10pt 0;
    }
    .alert-success {
        border-color: #16a34a;
        background-color: #dcfce7;
        color: #166534;
    }
    .alert-warning {
        border-color: #f59e0b;
        background-color: #fef3c7;
        color: #92400e;
    }
    .alert-error {
        border-color: #dc2626;
        background-color: #fee2e2;
        color: #991b1b;
    }
</style>
<body>
    <div class="alert alert-success">
        <strong>Success:</strong> Changes saved successfully.
    </div>
    <div class="alert alert-warning">
        <strong>Warning:</strong> Please verify your information.
    </div>
    <div class="alert alert-error">
        <strong>Error:</strong> Unable to complete action.
    </div>
</body>
```

### Example 7: Badge with high radius

```html
<style>
    .badge {
        border: 1pt solid #3b82f6;
        border-radius: 12pt;
        background-color: #3b82f6;
        color: white;
        padding: 4pt 12pt;
        font-size: 10pt;
        font-weight: bold;
        display: inline-block;
    }
    .badge-success {
        border-color: #16a34a;
        background-color: #16a34a;
    }
    .badge-warning {
        border-color: #f59e0b;
        background-color: #f59e0b;
    }
</style>
<body>
    <p>
        Status: <span class="badge badge-success">Active</span>
        Priority: <span class="badge badge-warning">High</span>
        Type: <span class="badge">Standard</span>
    </p>
</body>
```

### Example 8: Table with rounded corners

```html
<style>
    .rounded-table {
        width: 100%;
        border: 2pt solid #374151;
        border-radius: 10pt;
        border-collapse: separate;
        border-spacing: 0;
        overflow: hidden;
    }
    .rounded-table th {
        background-color: #f3f4f6;
        padding: 10pt;
        border-bottom: 2pt solid #d1d5db;
        font-weight: bold;
    }
    .rounded-table td {
        padding: 8pt;
        border-bottom: 1pt solid #e5e7eb;
    }
    .rounded-table tr:last-child td {
        border-bottom: none;
    }
</style>
<body>
    <table class="rounded-table">
        <thead>
            <tr>
                <th>Product</th>
                <th>Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Widget A</td>
                <td>$29.99</td>
            </tr>
            <tr>
                <td>Widget B</td>
                <td>$39.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 9: Certificate with decorative corners

```html
<style>
    .certificate {
        border: 6pt double #854d0e;
        border-radius: 15pt;
        padding: 40pt;
        background-color: #fffbeb;
        text-align: center;
    }
    .cert-title {
        font-size: 28pt;
        font-weight: bold;
        color: #92400e;
        margin-bottom: 20pt;
    }
    .cert-seal {
        border: 3pt solid #b45309;
        border-radius: 50%;
        width: 60pt;
        height: 60pt;
        background-color: #fef3c7;
        margin: 20pt auto;
        line-height: 60pt;
        font-weight: bold;
        color: #92400e;
    }
</style>
<body>
    <div class="certificate">
        <h1 class="cert-title">Certificate of Excellence</h1>
        <p>Awarded to</p>
        <p style="font-size: 20pt; font-weight: bold; margin: 15pt 0;">David Martinez</p>
        <div class="cert-seal">2025</div>
    </div>
</body>
```

### Example 10: Form with rounded inputs

```html
<style>
    .form-group {
        margin-bottom: 15pt;
    }
    .form-label {
        display: block;
        font-weight: bold;
        margin-bottom: 5pt;
    }
    .form-input {
        border: 1pt solid #9ca3af;
        border-radius: 5pt;
        padding: 10pt;
        width: 100%;
        background-color: white;
    }
    .form-input.focus {
        border-color: #2563eb;
        border-width: 2pt;
    }
    .submit-button {
        border: none;
        border-radius: 6pt;
        background-color: #2563eb;
        color: white;
        padding: 10pt 20pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="form-group">
        <label class="form-label">Full Name</label>
        <div class="form-input">John Smith</div>
    </div>
    <div class="form-group">
        <label class="form-label">Email</label>
        <div class="form-input focus">john@example.com</div>
    </div>
    <div class="submit-button">Submit Form</div>
</body>
```

### Example 11: Callout with rounded accent

```html
<style>
    .callout {
        border: 2pt solid #6366f1;
        border-radius: 8pt;
        border-left-width: 6pt;
        background-color: #eef2ff;
        padding: 15pt 15pt 15pt 20pt;
    }
    .callout-title {
        font-size: 14pt;
        font-weight: bold;
        color: #4f46e5;
        margin-bottom: 8pt;
    }
    .callout-text {
        color: #6366f1;
    }
</style>
<body>
    <div class="callout">
        <div class="callout-title">Pro Tip</div>
        <p class="callout-text">
            Rounded corners soften the appearance and create a more approachable design.
        </p>
    </div>
</body>
```

### Example 12: Invoice with modern styling

```html
<style>
    .invoice-container {
        border: 2pt solid #1e293b;
        border-radius: 12pt;
        overflow: hidden;
    }
    .invoice-header {
        background-color: #1e3a8a;
        color: white;
        padding: 20pt;
        border-radius: 10pt 10pt 0 0;
    }
    .invoice-title {
        font-size: 28pt;
        font-weight: bold;
        margin: 0;
    }
    .invoice-body {
        padding: 20pt;
        background-color: white;
    }
    .invoice-item {
        border: 1pt solid #e5e7eb;
        border-radius: 4pt;
        padding: 10pt;
        margin-bottom: 8pt;
        background-color: #f9fafb;
    }
</style>
<body>
    <div class="invoice-container">
        <div class="invoice-header">
            <h1 class="invoice-title">INVOICE</h1>
            <p>Invoice #INV-2025-7890</p>
        </div>
        <div class="invoice-body">
            <div class="invoice-item">
                <p>Product A - $150.00</p>
            </div>
            <div class="invoice-item">
                <p>Product B - $225.00</p>
            </div>
        </div>
    </div>
</body>
```

### Example 13: Pricing cards with varying radii

```html
<style>
    .pricing-grid {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 12pt;
    }
    .pricing-card {
        display: table-cell;
        border: 2pt solid #d1d5db;
        border-radius: 10pt;
        padding: 20pt;
        text-align: center;
        vertical-align: top;
        background-color: white;
    }
    .pricing-card.featured {
        border-color: #2563eb;
        border-width: 3pt;
        border-radius: 15pt;
        background-color: #eff6ff;
    }
    .plan-name {
        font-size: 18pt;
        font-weight: bold;
        margin-bottom: 10pt;
    }
    .plan-price {
        border: 2pt solid #2563eb;
        border-radius: 25pt;
        background-color: #2563eb;
        color: white;
        padding: 10pt 20pt;
        font-size: 20pt;
        font-weight: bold;
        display: inline-block;
        margin: 15pt 0;
    }
</style>
<body>
    <div class="pricing-grid">
        <div class="pricing-card">
            <div class="plan-name">Basic</div>
            <div class="plan-price">$9.99</div>
            <p>Essential features</p>
        </div>
        <div class="pricing-card featured">
            <div class="plan-name">Pro</div>
            <div class="plan-price">$29.99</div>
            <p>All features</p>
        </div>
        <div class="pricing-card">
            <div class="plan-name">Enterprise</div>
            <div class="plan-price">$99.99</div>
            <p>Custom solutions</p>
        </div>
    </div>
</body>
```

### Example 14: Data dashboard with rounded panels

```html
<style>
    .dashboard {
        display: table;
        width: 100%;
        border-collapse: separate;
        border-spacing: 10pt;
    }
    .stat-panel {
        display: table-cell;
        border: 1pt solid #e5e7eb;
        border-radius: 8pt;
        padding: 15pt;
        background-color: white;
        text-align: center;
        vertical-align: top;
    }
    .stat-value {
        font-size: 32pt;
        font-weight: bold;
        color: #2563eb;
        margin: 10pt 0;
    }
    .stat-label {
        font-size: 11pt;
        color: #6b7280;
        text-transform: uppercase;
    }
    .stat-icon {
        border-radius: 50%;
        width: 40pt;
        height: 40pt;
        background-color: #eff6ff;
        margin: 0 auto 10pt;
        line-height: 40pt;
        font-weight: bold;
        color: #2563eb;
    }
</style>
<body>
    <div class="dashboard">
        <div class="stat-panel">
            <div class="stat-icon">$</div>
            <div class="stat-value">$12.5K</div>
            <div class="stat-label">Revenue</div>
        </div>
        <div class="stat-panel">
            <div class="stat-icon">#</div>
            <div class="stat-value">348</div>
            <div class="stat-label">Orders</div>
        </div>
        <div class="stat-panel">
            <div class="stat-icon">%</div>
            <div class="stat-value">94%</div>
            <div class="stat-label">Satisfaction</div>
        </div>
    </div>
</body>
```

### Example 15: Profile card with circular elements

```html
<style>
    .profile-card {
        border: 2pt solid #e5e7eb;
        border-radius: 12pt;
        padding: 25pt;
        background-color: white;
        text-align: center;
    }
    .profile-avatar {
        border: 4pt solid #2563eb;
        border-radius: 50%;
        width: 100pt;
        height: 100pt;
        background-color: #eff6ff;
        margin: 0 auto 15pt;
        line-height: 100pt;
        font-size: 40pt;
        font-weight: bold;
        color: #2563eb;
    }
    .profile-name {
        font-size: 20pt;
        font-weight: bold;
        margin-bottom: 5pt;
    }
    .profile-title {
        font-size: 12pt;
        color: #6b7280;
        margin-bottom: 15pt;
    }
    .profile-badge {
        border-radius: 15pt;
        background-color: #dcfce7;
        color: #166534;
        padding: 6pt 15pt;
        font-size: 10pt;
        font-weight: bold;
        display: inline-block;
        margin: 5pt;
    }
</style>
<body>
    <div class="profile-card">
        <div class="profile-avatar">AS</div>
        <div class="profile-name">Alexandra Smith</div>
        <div class="profile-title">Senior Designer</div>
        <div class="profile-badge">Verified</div>
        <div class="profile-badge">Pro Member</div>
    </div>
</body>
```

---

## See Also

- [border](/reference/cssproperties/css_prop_border) - Shorthand for all border properties
- [border-width](/reference/cssproperties/css_prop_border-width) - Set border width
- [border-style](/reference/cssproperties/css_prop_border-style) - Set border style
- [border-color](/reference/cssproperties/css_prop_border-color) - Set border color
- [style attribute](/reference/htmlattributes/attr_style) - Inline CSS styling

---
