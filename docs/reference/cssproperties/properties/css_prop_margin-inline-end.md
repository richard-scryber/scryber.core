---
layout: default
title: margin-inline-end
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# margin-inline-end : Inline End Margin Property

The `margin-inline-end` property sets the margin at the inline end edge of an element in PDF documents. This is a logical property that maps to either right or left margin depending on the writing direction. In left-to-right (LTR) languages it corresponds to `margin-right`, while in right-to-left (RTL) languages it corresponds to `margin-left`.

## Usage

```css
selector {
    margin-inline-end: value;
}
```

The margin-inline-end property accepts a single length value or percentage that defines the space at the inline end edge.

---

## Supported Values

### Length Units
- Points: `10pt`, `15pt`, `20pt`
- Pixels: `10px`, `15px`, `20px`
- Inches: `0.5in`, `1in`
- Centimeters: `2cm`, `3cm`
- Millimeters: `10mm`, `20mm`
- Ems: `1em`, `1.5em`, `2em`
- Percentage: `5%`, `10%`, `15%` (relative to parent width)

### Special Values
- `0` - No inline end margin
- `auto` - Automatic margin

---

## Supported Elements

The `margin-inline-end` property can be applied to:
- Block elements (`<div>`, `<section>`, `<article>`)
- Inline-block elements
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Tables (`<table>`)
- Lists (`<ul>`, `<ol>`)
- Images (`<img>`)
- All container elements

---

## Notes

- This is a logical property that adapts to text direction
- In LTR contexts (English, Spanish, etc.), it behaves like `margin-right`
- In RTL contexts (Arabic, Hebrew, etc.), it behaves like `margin-left`
- Useful for creating internationalized documents that work in multiple languages
- Provides better semantic meaning than physical properties
- Simplifies maintenance of bidirectional layouts
- Does not collapse with adjacent margins (inline direction)
- Percentage values are relative to parent width

---

## Data Binding

The `margin-inline-end` property supports dynamic values through data binding, allowing you to create direction-aware, flexible end spacing for inline elements based on layout requirements and internationalization needs.

### Example 1: Dynamic badge spacing based on display mode

```html
<style>
    .badges {
        padding: 25pt;
    }
    .badge {
        display: inline-block;
        margin-inline-end: {{display.badgeSpacing}}pt;
        margin-bottom: 8pt;
        padding: {{display.compact ? '4pt 10pt' : '6pt 12pt'}};
        background-color: #3b82f6;
        color: white;
        border-radius: 3pt;
        font-size: {{display.compact ? '9pt' : '10pt'}};
    }
    .badge:last-child {
        margin-inline-end: 0;
    }
</style>
<body>
    <div class="badges">
        <span class="badge">New</span>
        <span class="badge">Featured</span>
        <span class="badge">Popular</span>
    </div>
</body>
```

Data context:
```json
{
    "display": {
        "compact": false,
        "badgeSpacing": 8
    }
}
```

### Example 2: Responsive grid layout with data-driven spacing

```html
<style>
    .product-grid {
        padding: 25pt;
    }
    .product-card {
        display: inline-block;
        width: {{grid.cardWidth}}pt;
        margin-inline-end: {{grid.spacing}}pt;
        margin-bottom: {{grid.spacing}}pt;
        padding: 15pt;
        border: 1pt solid #e5e7eb;
        vertical-align: top;
    }
    .product-card:nth-child({{grid.columns}}n) {
        margin-inline-end: 0;
    }
</style>
<body>
    <div class="product-grid">
        <div class="product-card">
            <div>{{products[0].name}}</div>
            <div>{{products[0].price}}</div>
        </div>
    </div>
</body>
```

Data context:
```json
{
    "grid": {
        "columns": 3,
        "cardWidth": 150,
        "spacing": 20
    },
    "products": [
        {
            "name": "Product A",
            "price": "$29.99"
        }
    ]
}
```

### Example 3: Bilingual button group with direction-aware spacing

```html
<style>
    .button-group {
        padding: 25pt;
        direction: {{locale.direction}};
    }
    .button {
        display: inline-block;
        margin-inline-end: {{spacing.buttonGap}}pt;
        padding: 10pt 20pt;
        background-color: #2563eb;
        color: white;
    }
    .button:last-child {
        margin-inline-end: 0;
    }
</style>
<body>
    <div class="button-group">
        <button class="button">{{buttons.primary}}</button>
        <button class="button">{{buttons.secondary}}</button>
    </div>
</body>
```

Data context:
```json
{
    "locale": {
        "direction": "ltr"
    },
    "spacing": {
        "buttonGap": 10
    },
    "buttons": {
        "primary": "Save",
        "secondary": "Cancel"
    }
}
```

---

## Examples

### Example 1: Basic inline end margin

```html
<style>
    .box {
        display: inline-block;
        margin-inline-end: 20pt;
        padding: 15pt;
        background-color: #dbeafe;
    }
</style>
<body>
    <div class="box">Box 1</div>
    <div class="box">Box 2</div>
    <div class="box">Box 3</div>
</body>
```

### Example 2: Badge spacing with inline end margin

```html
<style>
    .badges {
        padding: 25pt;
    }
    .badge {
        display: inline-block;
        margin-inline-end: 8pt;
        margin-bottom: 8pt;
        padding: 6pt 12pt;
        background-color: #3b82f6;
        color: white;
        border-radius: 3pt;
        font-size: 10pt;
    }
    .badge:last-child {
        margin-inline-end: 0;
    }
</style>
<body>
    <div class="badges">
        <span class="badge">New</span>
        <span class="badge">Featured</span>
        <span class="badge">Popular</span>
        <span class="badge">Sale</span>
    </div>
</body>
```

### Example 3: Inline icons with text spacing

```html
<style>
    .icon-list {
        padding: 25pt;
    }
    .icon-item {
        margin-bottom: 15pt;
    }
    .icon {
        display: inline-block;
        margin-inline-end: 10pt;
        width: 20pt;
        height: 20pt;
        background-color: #3b82f6;
        border-radius: 50%;
        vertical-align: middle;
    }
    .icon-text {
        display: inline-block;
        vertical-align: middle;
    }
</style>
<body>
    <div class="icon-list">
        <div class="icon-item">
            <span class="icon"></span>
            <span class="icon-text">Feature One</span>
        </div>
        <div class="icon-item">
            <span class="icon"></span>
            <span class="icon-text">Feature Two</span>
        </div>
        <div class="icon-item">
            <span class="icon"></span>
            <span class="icon-text">Feature Three</span>
        </div>
    </div>
</body>
```

### Example 4: Product card grid with inline end margin

```html
<style>
    .product-grid {
        padding: 25pt;
    }
    .product-card {
        display: inline-block;
        width: 150pt;
        margin-inline-end: 20pt;
        margin-bottom: 20pt;
        padding: 15pt;
        border: 1pt solid #e5e7eb;
        background-color: white;
        vertical-align: top;
    }
    .product-card:nth-child(3n) {
        margin-inline-end: 0;
    }
    .product-name {
        margin-bottom: 8pt;
        font-weight: bold;
    }
    .product-price {
        color: #16a34a;
        font-size: 14pt;
        font-weight: bold;
    }
</style>
<body>
    <div class="product-grid">
        <div class="product-card">
            <div class="product-name">Product A</div>
            <div class="product-price">$29.99</div>
        </div>
        <div class="product-card">
            <div class="product-name">Product B</div>
            <div class="product-price">$39.99</div>
        </div>
        <div class="product-card">
            <div class="product-name">Product C</div>
            <div class="product-price">$49.99</div>
        </div>
    </div>
</body>
```

### Example 5: Button group with spacing

```html
<style>
    .button-group {
        padding: 25pt;
    }
    .button {
        display: inline-block;
        margin-inline-end: 10pt;
        padding: 10pt 20pt;
        background-color: #2563eb;
        color: white;
        border: none;
    }
    .button:last-child {
        margin-inline-end: 0;
    }
    .button-secondary {
        background-color: #6b7280;
    }
    .button-danger {
        background-color: #dc2626;
    }
</style>
<body>
    <div class="button-group">
        <button class="button">Save</button>
        <button class="button button-secondary">Cancel</button>
        <button class="button button-danger">Delete</button>
    </div>
</body>
```

### Example 6: Form inline fields with inline end margin

```html
<style>
    .form-inline {
        padding: 30pt;
    }
    .form-field {
        display: inline-block;
        margin-inline-end: 15pt;
        margin-bottom: 15pt;
        vertical-align: top;
    }
    .form-field:last-child {
        margin-inline-end: 0;
    }
    .form-field label {
        display: block;
        margin-bottom: 5pt;
        font-weight: bold;
        font-size: 11pt;
    }
    .form-field input {
        padding: 8pt;
        border: 1pt solid #d1d5db;
        width: 120pt;
    }
</style>
<body>
    <div class="form-inline">
        <div class="form-field">
            <label>First Name</label>
            <input type="text" />
        </div>
        <div class="form-field">
            <label>Last Name</label>
            <input type="text" />
        </div>
        <div class="form-field">
            <label>Age</label>
            <input type="text" />
        </div>
    </div>
</body>
```

### Example 7: Stats dashboard with inline end spacing

```html
<style>
    .dashboard {
        padding: 30pt;
    }
    .stat-card {
        display: inline-block;
        width: 140pt;
        margin-inline-end: 20pt;
        margin-bottom: 20pt;
        padding: 20pt;
        background-color: #f9fafb;
        border: 1pt solid #e5e7eb;
        text-align: center;
        vertical-align: top;
    }
    .stat-card:nth-child(3n) {
        margin-inline-end: 0;
    }
    .stat-value {
        margin-bottom: 8pt;
        font-size: 28pt;
        font-weight: bold;
        color: #1f2937;
    }
    .stat-label {
        font-size: 11pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="dashboard">
        <div class="stat-card">
            <div class="stat-value">1,234</div>
            <div class="stat-label">Total Users</div>
        </div>
        <div class="stat-card">
            <div class="stat-value">567</div>
            <div class="stat-label">Active Now</div>
        </div>
        <div class="stat-card">
            <div class="stat-value">89%</div>
            <div class="stat-label">Satisfaction</div>
        </div>
    </div>
</body>
```

### Example 8: Breadcrumb navigation

```html
<style>
    .breadcrumb {
        padding: 15pt 30pt;
        background-color: #f3f4f6;
    }
    .breadcrumb-item {
        display: inline-block;
        margin-inline-end: 5pt;
        font-size: 11pt;
    }
    .breadcrumb-separator {
        display: inline-block;
        margin-inline-end: 5pt;
        color: #6b7280;
    }
    .breadcrumb-item:last-child {
        font-weight: bold;
    }
</style>
<body>
    <div class="breadcrumb">
        <span class="breadcrumb-item">Home</span>
        <span class="breadcrumb-separator">/</span>
        <span class="breadcrumb-item">Products</span>
        <span class="breadcrumb-separator">/</span>
        <span class="breadcrumb-item">Electronics</span>
        <span class="breadcrumb-separator">/</span>
        <span class="breadcrumb-item">Laptops</span>
    </div>
</body>
```

### Example 9: Tag cloud with inline end margins

```html
<style>
    .tag-cloud {
        padding: 25pt;
    }
    .tag-cloud-title {
        margin-bottom: 20pt;
        font-size: 18pt;
        font-weight: bold;
    }
    .tag {
        display: inline-block;
        margin-inline-end: 8pt;
        margin-bottom: 8pt;
        padding: 5pt 12pt;
        background-color: #e0e7ff;
        color: #3730a3;
        border-radius: 4pt;
        font-size: 10pt;
    }
    .tag-large {
        font-size: 12pt;
        padding: 6pt 14pt;
    }
    .tag-small {
        font-size: 9pt;
        padding: 4pt 10pt;
    }
</style>
<body>
    <div class="tag-cloud">
        <h3 class="tag-cloud-title">Popular Topics</h3>
        <span class="tag tag-large">Technology</span>
        <span class="tag">Innovation</span>
        <span class="tag tag-small">Design</span>
        <span class="tag tag-large">Development</span>
        <span class="tag">Business</span>
        <span class="tag tag-small">Strategy</span>
        <span class="tag">Marketing</span>
    </div>
</body>
```

### Example 10: Invoice table with column spacing

```html
<style>
    .invoice {
        padding: 40pt;
    }
    .invoice-header {
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e3a8a;
        color: white;
    }
    .invoice-table {
        width: 100%;
        border-collapse: collapse;
    }
    .invoice-table th,
    .invoice-table td {
        padding: 10pt;
        border: 1pt solid #d1d5db;
        text-align: left;
    }
    .invoice-table th {
        background-color: #f3f4f6;
        font-weight: bold;
    }
    .col-description {
        width: 60%;
    }
    .col-quantity {
        width: 15%;
        text-align: center;
    }
    .col-price {
        width: 25%;
        text-align: right;
    }
</style>
<body>
    <div class="invoice">
        <div class="invoice-header">
            <h1>INVOICE #INV-2025-001</h1>
        </div>
        <table class="invoice-table">
            <thead>
                <tr>
                    <th class="col-description">Description</th>
                    <th class="col-quantity">Qty</th>
                    <th class="col-price">Price</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Consulting Services</td>
                    <td class="col-quantity">10</td>
                    <td class="col-price">$1,500.00</td>
                </tr>
                <tr>
                    <td>Software License</td>
                    <td class="col-quantity">1</td>
                    <td class="col-price">$299.00</td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
```

### Example 11: Social media profile cards

```html
<style>
    .profiles {
        padding: 25pt;
    }
    .profile-card {
        display: inline-block;
        width: 120pt;
        margin-inline-end: 15pt;
        margin-bottom: 15pt;
        padding: 15pt;
        border: 1pt solid #e5e7eb;
        text-align: center;
        vertical-align: top;
    }
    .profile-avatar {
        width: 60pt;
        height: 60pt;
        margin-inline-end: auto;
        margin-inline-start: auto;
        margin-bottom: 10pt;
        background-color: #d1d5db;
        border-radius: 50%;
    }
    .profile-name {
        margin-bottom: 5pt;
        font-weight: bold;
    }
    .profile-title {
        font-size: 9pt;
        color: #6b7280;
    }
</style>
<body>
    <div class="profiles">
        <div class="profile-card">
            <div class="profile-avatar"></div>
            <div class="profile-name">John Smith</div>
            <div class="profile-title">Developer</div>
        </div>
        <div class="profile-card">
            <div class="profile-avatar"></div>
            <div class="profile-name">Jane Doe</div>
            <div class="profile-title">Designer</div>
        </div>
        <div class="profile-card">
            <div class="profile-avatar"></div>
            <div class="profile-name">Bob Wilson</div>
            <div class="profile-title">Manager</div>
        </div>
    </div>
</body>
```

### Example 12: Newsletter with column layout

```html
<style>
    .newsletter {
        padding: 30pt;
    }
    .newsletter-header {
        margin-bottom: 30pt;
        padding: 20pt;
        background-color: #1e40af;
        color: white;
        text-align: center;
    }
    .column {
        display: inline-block;
        width: 48%;
        margin-inline-end: 4%;
        vertical-align: top;
    }
    .column:last-child {
        margin-inline-end: 0;
    }
    .column h2 {
        margin-bottom: 10pt;
        font-size: 16pt;
    }
    .column p {
        margin-bottom: 10pt;
        font-size: 11pt;
        line-height: 1.5;
    }
</style>
<body>
    <div class="newsletter">
        <div class="newsletter-header">
            <h1>Monthly Newsletter</h1>
        </div>
        <div class="column">
            <h2>Feature Article</h2>
            <p>Main content for the first column with inline end margin.</p>
        </div>
        <div class="column">
            <h2>Updates</h2>
            <p>Latest news and announcements in the second column.</p>
        </div>
    </div>
</body>
```

### Example 13: Pricing table comparison

```html
<style>
    .pricing {
        padding: 30pt;
    }
    .pricing-title {
        margin-bottom: 25pt;
        font-size: 22pt;
        font-weight: bold;
        text-align: center;
    }
    .pricing-plan {
        display: inline-block;
        width: 150pt;
        margin-inline-end: 20pt;
        padding: 20pt;
        border: 2pt solid #e5e7eb;
        text-align: center;
        vertical-align: top;
    }
    .pricing-plan:last-child {
        margin-inline-end: 0;
    }
    .plan-name {
        margin-bottom: 10pt;
        font-size: 16pt;
        font-weight: bold;
    }
    .plan-price {
        margin-bottom: 15pt;
        font-size: 24pt;
        color: #16a34a;
    }
    .plan-features {
        font-size: 10pt;
        text-align: left;
    }
</style>
<body>
    <div class="pricing">
        <h1 class="pricing-title">Pricing Plans</h1>
        <div class="pricing-plan">
            <div class="plan-name">Basic</div>
            <div class="plan-price">$9.99</div>
            <ul class="plan-features">
                <li>10 Projects</li>
                <li>Basic Support</li>
            </ul>
        </div>
        <div class="pricing-plan">
            <div class="plan-name">Pro</div>
            <div class="plan-price">$19.99</div>
            <ul class="plan-features">
                <li>50 Projects</li>
                <li>Priority Support</li>
            </ul>
        </div>
        <div class="pricing-plan">
            <div class="plan-name">Enterprise</div>
            <div class="plan-price">$49.99</div>
            <ul class="plan-features">
                <li>Unlimited Projects</li>
                <li>24/7 Support</li>
            </ul>
        </div>
    </div>
</body>
```

### Example 14: Receipt with item spacing

```html
<style>
    .receipt {
        margin: 40pt auto;
        width: 320pt;
        padding: 20pt;
        border: 2pt solid #000;
    }
    .receipt-header {
        margin-bottom: 20pt;
        text-align: center;
        border-bottom: 1pt solid #000;
        padding-bottom: 10pt;
    }
    .receipt-item {
        margin-bottom: 8pt;
    }
    .item-name {
        display: inline-block;
        width: 60%;
        margin-inline-end: 5%;
    }
    .item-qty {
        display: inline-block;
        width: 15%;
        margin-inline-end: 5%;
        text-align: center;
    }
    .item-price {
        display: inline-block;
        width: 15%;
        text-align: right;
    }
    .receipt-total {
        margin-top: 15pt;
        padding-top: 10pt;
        border-top: 2pt solid #000;
        font-weight: bold;
        font-size: 14pt;
    }
</style>
<body>
    <div class="receipt">
        <div class="receipt-header">
            <h2>Store Name</h2>
            <p>Receipt #12345</p>
        </div>
        <div class="receipt-item">
            <span class="item-name">Widget A</span>
            <span class="item-qty">2</span>
            <span class="item-price">$39.98</span>
        </div>
        <div class="receipt-item">
            <span class="item-name">Widget B</span>
            <span class="item-qty">1</span>
            <span class="item-price">$29.99</span>
        </div>
        <div class="receipt-total">
            <span class="item-name">Total</span>
            <span class="item-qty"></span>
            <span class="item-price">$69.97</span>
        </div>
    </div>
</body>
```

### Example 15: Business card layout

```html
<style>
    .business-card {
        width: 350pt;
        height: 200pt;
        margin: 30pt auto;
        padding: 20pt;
        border: 2pt solid #1e3a8a;
    }
    .card-left {
        display: inline-block;
        width: 60%;
        margin-inline-end: 5%;
        vertical-align: top;
    }
    .card-right {
        display: inline-block;
        width: 35%;
        vertical-align: top;
        text-align: right;
    }
    .card-name {
        margin-bottom: 5pt;
        font-size: 18pt;
        font-weight: bold;
    }
    .card-title {
        margin-bottom: 15pt;
        font-size: 12pt;
        color: #6b7280;
    }
    .card-company {
        margin-bottom: 8pt;
        font-size: 14pt;
        font-weight: bold;
        color: #1e3a8a;
    }
    .card-contact {
        font-size: 10pt;
        line-height: 1.4;
    }
</style>
<body>
    <div class="business-card">
        <div class="card-left">
            <div class="card-name">Sarah Johnson</div>
            <div class="card-title">Marketing Director</div>
            <div class="card-contact">
                sarah.johnson@example.com<br/>
                +1 (555) 987-6543
            </div>
        </div>
        <div class="card-right">
            <div class="card-company">ACME Corp</div>
            <div class="card-contact">
                456 Business Ave<br/>
                Suite 200<br/>
                New York, NY 10001
            </div>
        </div>
    </div>
</body>
```

---

## See Also

- [margin-inline-start](/reference/cssproperties/css_prop_margin-inline-start) - Set inline start margin
- [margin-inline](/reference/cssproperties/css_prop_margin-inline) - Set both inline margins shorthand
- [margin-right](/reference/cssproperties/css_prop_margin-right) - Set right margin (physical property)
- [margin-left](/reference/cssproperties/css_prop_margin-left) - Set left margin (physical property)
- [padding-inline-end](/reference/cssproperties/css_prop_padding-inline-end) - Set inline end padding
- [style](/reference/htmlattributes/attr_style) - Inline style attribute

---
