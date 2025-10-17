---
layout: default
title: color
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# color : Text Color Property

The `color` property sets the foreground color of text content in PDF elements. This property is fundamental for controlling text appearance and establishing visual hierarchy in generated PDFs.

## Usage

```css
selector {
    color: value;
}
```

The color property accepts multiple value formats including named colors, hexadecimal notation, and RGB/RGBA functions.

---

## Supported Values

### Named Colors
Standard CSS color names such as `red`, `blue`, `green`, `black`, `white`, etc.

### Hexadecimal Colors
- Short form: `#RGB` (e.g., `#f00` for red)
- Long form: `#RRGGBB` (e.g., `#ff0000` for red)

### RGB/RGBA Functions
- RGB: `rgb(red, green, blue)` where values are 0-255
- RGBA: `rgba(red, green, blue, alpha)` where alpha is 0.0-1.0 for transparency

---

## Supported Elements

The `color` property can be applied to any text-containing element including:
- Paragraphs (`<p>`)
- Headings (`<h1>` through `<h6>`)
- Spans (`<span>`)
- Divs (`<div>`)
- List items (`<li>`)
- Table cells (`<td>`, `<th>`)
- Links (`<a>`)
- All other text elements

---

## Notes

- Colors are rendered accurately in PDF output with full color space support
- RGBA values provide transparency, with 0.0 being fully transparent and 1.0 fully opaque
- Hexadecimal colors are case-insensitive (`#FF0000` equals `#ff0000`)
- Short hex notation (`#RGB`) is expanded to long form (`#RRGGBB`)
- The color property inherits by default, so child elements will use parent colors unless overridden
- Transparent colors (RGBA with alpha < 1.0) blend with background colors/fills
- Named colors follow standard CSS3/X11 color specifications

---

## Data Binding

The `color` property can be dynamically set using data binding expressions, enabling colors to change based on model data, user input, or business logic conditions.

### Example 1: Status-based text colors in invoices

```html
<style>
    .status-text {
        font-weight: bold;
    }
</style>
<body>
    <div>
        <p>Order Status: <span class="status-text" style="color: {{order.statusColor}}">{{order.status}}</span></p>
        <p>Payment Status: <span class="status-text" style="color: {{payment.statusColor}}">{{payment.status}}</span></p>
    </div>
</body>
```

With model data:
```json
{
    "order": {
        "status": "Shipped",
        "statusColor": "#10b981"
    },
    "payment": {
        "status": "Paid",
        "statusColor": "#22c55e"
    }
}
```

### Example 2: Conditional colors based on values

```html
<style>
    .amount {
        font-size: 14pt;
        font-weight: bold;
    }
</style>
<body>
    <table>
        <thead>
            <tr>
                <th>Item</th>
                <th>Amount</th>
                <th>Status</th>
            </tr>
        </thead>
        <tbody>
            {{#each items}}
            <tr>
                <td>{{name}}</td>
                <td class="amount" style="color: {{amount > 1000 ? '#dc2626' : '#16a34a'}}">
                    ${{amount}}
                </td>
                <td style="color: {{isPaid ? '#10b981' : '#f59e0b'}}">
                    {{isPaid ? 'Paid' : 'Pending'}}
                </td>
            </tr>
            {{/each}}
        </tbody>
    </table>
</body>
```

### Example 3: Brand colors from configuration

```html
<style>
    .company-name {
        font-size: 24pt;
        font-weight: bold;
    }
    .tagline {
        font-size: 12pt;
        font-style: italic;
    }
</style>
<body>
    <div>
        <h1 class="company-name" style="color: {{theme.primaryColor}}">{{company.name}}</h1>
        <p class="tagline" style="color: {{theme.secondaryColor}}">{{company.tagline}}</p>
    </div>

    <div>
        <h2 style="color: {{theme.accentColor}}">Report Summary</h2>
        <p>Total Revenue: <span style="color: {{theme.successColor}}">${{revenue.total}}</span></p>
        <p>Outstanding: <span style="color: {{theme.warningColor}}">${{revenue.outstanding}}</span></p>
    </div>
</body>
```

With configuration data:
```json
{
    "theme": {
        "primaryColor": "#1e40af",
        "secondaryColor": "#64748b",
        "accentColor": "#3b82f6",
        "successColor": "#16a34a",
        "warningColor": "#f59e0b"
    }
}
```

---

## Examples

### Example 1: Basic text color with named color

```html
<style>
    .heading {
        color: darkblue;
        font-size: 18pt;
    }
</style>
<body>
    <h1 class="heading">Dark Blue Heading</h1>
    <p>This text remains the default color.</p>
</body>
```

### Example 2: Hexadecimal color notation

```html
<style>
    .brand-text {
        color: #2563eb;
        font-weight: bold;
    }
</style>
<body>
    <p class="brand-text">This text uses a specific brand blue color.</p>
</body>
```

### Example 3: Short hex notation

```html
<style>
    .red-text {
        color: #f00;
    }
    .green-text {
        color: #0f0;
    }
    .blue-text {
        color: #00f;
    }
</style>
<body>
    <p class="red-text">Red text</p>
    <p class="green-text">Green text</p>
    <p class="blue-text">Blue text</p>
</body>
```

### Example 4: RGB function

```html
<style>
    .custom-purple {
        color: rgb(128, 0, 128);
        font-size: 14pt;
    }
</style>
<body>
    <p class="custom-purple">Purple text using RGB values</p>
</body>
```

### Example 5: RGBA with transparency

```html
<style>
    .semi-transparent {
        color: rgba(0, 0, 0, 0.5);
        font-size: 12pt;
    }
</style>
<body>
    <p class="semi-transparent">This text is 50% transparent black</p>
</body>
```

### Example 6: Color inheritance

```html
<style>
    .container {
        color: #333333;
    }
    .highlight {
        color: red;
    }
</style>
<body>
    <div class="container">
        <p>This paragraph inherits the dark gray color.</p>
        <p class="highlight">This paragraph is red.</p>
        <p>Back to dark gray.</p>
    </div>
</body>
```

### Example 7: Table cell colors

```html
<style>
    table {
        width: 100%;
    }
    .header-cell {
        color: white;
        background-color: #1e40af;
        padding: 8pt;
    }
    .data-cell {
        color: #374151;
        padding: 6pt;
    }
</style>
<body>
    <table>
        <thead>
            <tr>
                <th class="header-cell">Product</th>
                <th class="header-cell">Price</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="data-cell">Widget</td>
                <td class="data-cell">$19.99</td>
            </tr>
        </tbody>
    </table>
</body>
```

### Example 8: List item colors

```html
<style>
    .priority-high {
        color: #dc2626;
        font-weight: bold;
    }
    .priority-medium {
        color: #f59e0b;
    }
    .priority-low {
        color: #10b981;
    }
</style>
<body>
    <ul>
        <li class="priority-high">Critical Issue - Fix immediately</li>
        <li class="priority-medium">Important - Address soon</li>
        <li class="priority-low">Minor - Can wait</li>
    </ul>
</body>
```

### Example 9: Link colors

```html
<style>
    a {
        color: #2563eb;
        text-decoration: underline;
    }
    a:hover {
        color: #1e40af;
    }
    .external-link {
        color: #7c3aed;
    }
</style>
<body>
    <p>
        Visit our <a href="https://example.com">website</a> or
        check this <a href="https://external.com" class="external-link">external resource</a>.
    </p>
</body>
```

### Example 10: Color contrast for readability

```html
<style>
    .dark-bg {
        background-color: #1f2937;
        color: #f9fafb;
        padding: 12pt;
    }
    .light-bg {
        background-color: #f3f4f6;
        color: #111827;
        padding: 12pt;
    }
</style>
<body>
    <div class="dark-bg">
        <p>Light text on dark background for good contrast</p>
    </div>
    <div class="light-bg">
        <p>Dark text on light background for accessibility</p>
    </div>
</body>
```

### Example 11: Gradient-like effect with multiple colors

```html
<style>
    .title {
        color: #be123c;
        font-size: 24pt;
        font-weight: bold;
    }
    .subtitle {
        color: #e11d48;
        font-size: 16pt;
    }
    .body-text {
        color: #f43f5e;
        font-size: 12pt;
    }
</style>
<body>
    <h1 class="title">Main Title</h1>
    <h2 class="subtitle">Subtitle</h2>
    <p class="body-text">Body text with progressively lighter shades</p>
</body>
```

### Example 12: Status indicators

```html
<style>
    .status-success {
        color: #16a34a;
        font-weight: bold;
    }
    .status-warning {
        color: #ca8a04;
        font-weight: bold;
    }
    .status-error {
        color: #dc2626;
        font-weight: bold;
    }
</style>
<body>
    <p><span class="status-success">✓ Success:</span> Operation completed</p>
    <p><span class="status-warning">⚠ Warning:</span> Check configuration</p>
    <p><span class="status-error">✗ Error:</span> Failed to process</p>
</body>
```

### Example 13: Code block styling

```html
<style>
    .code-block {
        background-color: #f5f5f5;
        padding: 10pt;
        font-family: 'Courier New', monospace;
    }
    .keyword {
        color: #d73a49;
        font-weight: bold;
    }
    .string {
        color: #032f62;
    }
    .comment {
        color: #6a737d;
        font-style: italic;
    }
</style>
<body>
    <div class="code-block">
        <span class="keyword">function</span> example() {<br/>
        &nbsp;&nbsp;<span class="comment">// This is a comment</span><br/>
        &nbsp;&nbsp;<span class="keyword">return</span> <span class="string">"Hello World"</span>;<br/>
        }
    </div>
</body>
```

### Example 14: Invoice styling with color coding

```html
<style>
    .invoice-header {
        color: #1e293b;
        font-size: 18pt;
        font-weight: bold;
    }
    .invoice-label {
        color: #64748b;
        font-size: 10pt;
    }
    .invoice-value {
        color: #0f172a;
        font-size: 11pt;
        font-weight: bold;
    }
    .total-amount {
        color: #166534;
        font-size: 16pt;
        font-weight: bold;
    }
</style>
<body>
    <h1 class="invoice-header">Invoice #12345</h1>
    <p>
        <span class="invoice-label">Date:</span>
        <span class="invoice-value">2025-10-13</span>
    </p>
    <p>
        <span class="invoice-label">Total:</span>
        <span class="total-amount">$1,234.56</span>
    </p>
</body>
```

### Example 15: Report with semantic colors

```html
<style>
    .report-title {
        color: #0c4a6e;
        font-size: 20pt;
        font-weight: bold;
    }
    .metric-positive {
        color: #15803d;
    }
    .metric-negative {
        color: #b91c1c;
    }
    .metric-neutral {
        color: #6b7280;
    }
</style>
<body>
    <h1 class="report-title">Quarterly Performance Report</h1>
    <p>Revenue: <span class="metric-positive">+15.3% ↑</span></p>
    <p>Costs: <span class="metric-negative">+8.7% ↑</span></p>
    <p>Employees: <span class="metric-neutral">453 (unchanged)</span></p>
</body>
```

---

## See Also

- [background-color](/reference/cssproperties/css_prop_background-color) - Set background colors
- [opacity](/reference/cssproperties/css_prop_opacity) - Control overall element transparency
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property

---
