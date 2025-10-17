---
layout: default
title: fill
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# fill : SVG Fill Color Property

The `fill` property sets the interior color of SVG shapes and vector elements in PDF documents. This property is fundamental for styling vector graphics, icons, and custom shapes embedded in PDFs.

## Usage

```css
selector {
    fill: value;
}
```

The fill property accepts multiple value formats including named colors, hexadecimal notation, RGB/RGBA functions, and special keywords like `none`.

---

## Supported Values

### Named Colors
Standard CSS color names such as `red`, `blue`, `green`, `black`, `orange`, etc.

### Hexadecimal Colors
- Short form: `#RGB` (e.g., `#09f` for light blue)
- Long form: `#RRGGBB` (e.g., `#0099ff` for light blue)

### RGB/RGBA Functions
- RGB: `rgb(red, green, blue)` where values are 0-255
- RGBA: `rgba(red, green, blue, alpha)` where alpha is 0.0-1.0 for transparency

### Special Keywords
- `none` - No fill (transparent interior)
- `transparent` - Same as `none`

---

## Supported Elements

The `fill` property applies to SVG elements including:
- `<svg>` containers
- `<rect>` rectangles
- `<circle>` circles
- `<ellipse>` ellipses
- `<polygon>` polygons
- `<polyline>` polylines
- `<path>` paths
- `<text>` SVG text elements
- All other SVG shape elements

---

## Notes

- The `fill` property only affects SVG elements, not regular HTML elements
- Use `fill: none` to create outlined shapes without interior color
- RGBA fills enable transparent vector graphics
- Fill colors are rendered precisely in PDF vector format
- The fill property works in combination with `stroke` for complete shape styling
- Use `fill-opacity` for separate opacity control of the fill
- Vector fills maintain quality at any zoom level in PDF viewers
- Fill colors do not inherit by default in SVG contexts
- When `fill` is not specified, default fill is typically black

---

## Data Binding

The `fill` property can be dynamically set using data binding expressions, enabling SVG fill colors to change based on data values, categories, or configuration settings.

### Example 1: Data-driven chart colors

```html
<style>
    .chart-bar {
        stroke: white;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="400" height="200">
        {{#each chartData}}
        <rect class="chart-bar"
              style="fill: {{color}}"
              x="{{xPosition}}"
              y="{{yPosition}}"
              width="60"
              height="{{height}}"/>
        {{/each}}
    </svg>
</body>
```

With model data:
```json
{
    "chartData": [
        { "value": 80, "color": "#3b82f6", "xPosition": 20, "yPosition": 120, "height": 80 },
        { "value": 120, "color": "#10b981", "xPosition": 110, "yPosition": 80, "height": 120 },
        { "value": 65, "color": "#f59e0b", "xPosition": 200, "yPosition": 135, "height": 65 }
    ]
}
```

### Example 2: Conditional status indicators

```html
<style>
    .status-icon {
        stroke: white;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="400" height="50">
        {{#each statusList}}
        <circle class="status-icon"
                style="fill: {{isActive ? '#22c55e' : isWarning ? '#f59e0b' : '#ef4444'}}"
                cx="{{position}}"
                cy="25"
                r="15"/>
        {{/each}}
    </svg>
</body>
```

### Example 3: Category-based color coding

```html
<style>
    .category-shape {
        stroke: #374151;
        stroke-width: 1;
    }
</style>
<body>
    <svg width="300" height="250" viewBox="0 0 100 100">
        {{#each pieSegments}}
        <path class="category-shape"
              style="fill: {{categoryColor}}"
              d="{{pathData}}"/>
        {{/each}}
    </svg>
</body>
```

With configuration mapping:
```json
{
    "pieSegments": [
        { "category": "Sales", "categoryColor": "#3b82f6", "pathData": "M50,50 L50,10 A40,40 0 0,1 85,35 Z" },
        { "category": "Marketing", "categoryColor": "#10b981", "pathData": "M50,50 L85,35 A40,40 0 0,1 75,75 Z" },
        { "category": "Operations", "categoryColor": "#f59e0b", "pathData": "M50,50 L75,75 A40,40 0 0,1 15,65 Z" }
    ]
}
```

---

## Examples

### Example 1: Basic rectangle fill

```html
<style>
    .blue-rect {
        fill: blue;
    }
</style>
<body>
    <svg width="200" height="100">
        <rect class="blue-rect" x="10" y="10" width="180" height="80"/>
    </svg>
</body>
```

### Example 2: Circle with hex color

```html
<style>
    .orange-circle {
        fill: #ff6600;
    }
</style>
<body>
    <svg width="150" height="150">
        <circle class="orange-circle" cx="75" cy="75" r="60"/>
    </svg>
</body>
```

### Example 3: No fill with stroke only

```html
<style>
    .outlined-rect {
        fill: none;
        stroke: #2563eb;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="200" height="100">
        <rect class="outlined-rect" x="10" y="10" width="180" height="80"/>
    </svg>
</body>
```

### Example 4: Transparent fill with RGBA

```html
<style>
    .semi-transparent {
        fill: rgba(255, 0, 0, 0.3);
        stroke: red;
        stroke-width: 1;
    }
</style>
<body>
    <svg width="150" height="150">
        <circle class="semi-transparent" cx="75" cy="75" r="60"/>
    </svg>
</body>
```

### Example 5: Multiple shapes with different fills

```html
<style>
    .shape-red { fill: #ef4444; }
    .shape-green { fill: #10b981; }
    .shape-blue { fill: #3b82f6; }
</style>
<body>
    <svg width="300" height="100">
        <rect class="shape-red" x="10" y="10" width="80" height="80"/>
        <circle class="shape-green" cx="150" cy="50" r="40"/>
        <polygon class="shape-blue" points="240,10 280,90 200,90"/>
    </svg>
</body>
```

### Example 6: Icon with fill color

```html
<style>
    .icon-star {
        fill: #fbbf24;
        stroke: #f59e0b;
        stroke-width: 1;
    }
</style>
<body>
    <svg width="100" height="100" viewBox="0 0 24 24">
        <path class="icon-star" d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z"/>
    </svg>
</body>
```

### Example 7: Chart bars with colored fills

```html
<style>
    .bar-1 { fill: #3b82f6; }
    .bar-2 { fill: #10b981; }
    .bar-3 { fill: #f59e0b; }
    .bar-4 { fill: #ef4444; }
</style>
<body>
    <svg width="400" height="200">
        <rect class="bar-1" x="20" y="80" width="60" height="100"/>
        <rect class="bar-2" x="110" y="40" width="60" height="140"/>
        <rect class="bar-3" x="200" y="60" width="60" height="120"/>
        <rect class="bar-4" x="290" y="100" width="60" height="80"/>
    </svg>
</body>
```

### Example 8: Pie chart segments

```html
<style>
    .segment-1 { fill: #3b82f6; }
    .segment-2 { fill: #10b981; }
    .segment-3 { fill: #f59e0b; }
    .segment-4 { fill: #ef4444; }
    .pie-slice { stroke: white; stroke-width: 2; }
</style>
<body>
    <svg width="200" height="200" viewBox="0 0 100 100">
        <circle class="segment-1 pie-slice" cx="50" cy="50" r="40" stroke-dasharray="62.8 188.4"/>
        <circle class="segment-2 pie-slice" cx="50" cy="50" r="40" stroke-dasharray="31.4 219.8" stroke-dashoffset="-62.8"/>
        <circle class="segment-3 pie-slice" cx="50" cy="50" r="40" stroke-dasharray="47.1 204.1" stroke-dashoffset="-94.2"/>
        <circle class="segment-4 pie-slice" cx="50" cy="50" r="40" stroke-dasharray="47.1 204.1" stroke-dashoffset="-141.3"/>
    </svg>
</body>
```

### Example 9: Logo with brand colors

```html
<style>
    .logo-primary { fill: #1e40af; }
    .logo-secondary { fill: #3b82f6; }
    .logo-accent { fill: #60a5fa; }
</style>
<body>
    <svg width="150" height="150" viewBox="0 0 100 100">
        <circle class="logo-primary" cx="30" cy="50" r="20"/>
        <circle class="logo-secondary" cx="50" cy="50" r="20"/>
        <circle class="logo-accent" cx="70" cy="50" r="20"/>
    </svg>
</body>
```

### Example 10: Status indicators

```html
<style>
    .status-success { fill: #22c55e; }
    .status-warning { fill: #f59e0b; }
    .status-error { fill: #ef4444; }
    .status-icon { stroke: white; stroke-width: 2; }
</style>
<body>
    <svg width="300" height="50">
        <circle class="status-success status-icon" cx="30" cy="25" r="15"/>
        <circle class="status-warning status-icon" cx="150" cy="25" r="15"/>
        <circle class="status-error status-icon" cx="270" cy="25" r="15"/>
    </svg>
</body>
```

### Example 11: Overlapping shapes with transparency

```html
<style>
    .overlay-red { fill: rgba(239, 68, 68, 0.5); }
    .overlay-blue { fill: rgba(59, 130, 246, 0.5); }
    .overlay-green { fill: rgba(34, 197, 94, 0.5); }
</style>
<body>
    <svg width="250" height="250">
        <circle class="overlay-red" cx="100" cy="100" r="60"/>
        <circle class="overlay-blue" cx="140" cy="120" r="60"/>
        <circle class="overlay-green" cx="120" cy="80" r="60"/>
    </svg>
</body>
```

### Example 12: Traffic light

```html
<style>
    .light-housing { fill: #374151; stroke: #1f2937; stroke-width: 2; }
    .light-red { fill: #dc2626; }
    .light-yellow { fill: #fbbf24; }
    .light-green { fill: #16a34a; }
    .light-off { fill: #4b5563; }
</style>
<body>
    <svg width="100" height="250">
        <rect class="light-housing" x="20" y="10" width="60" height="230" rx="10"/>
        <circle class="light-red" cx="50" cy="50" r="20"/>
        <circle class="light-off" cx="50" cy="125" r="20"/>
        <circle class="light-off" cx="50" cy="200" r="20"/>
    </svg>
</body>
```

### Example 13: Progress indicator

```html
<style>
    .progress-bg { fill: #e5e7eb; }
    .progress-fill { fill: #3b82f6; }
</style>
<body>
    <svg width="300" height="30">
        <rect class="progress-bg" x="0" y="0" width="300" height="30" rx="15"/>
        <rect class="progress-fill" x="0" y="0" width="210" height="30" rx="15"/>
    </svg>
    <p>70% Complete</p>
</body>
```

### Example 14: Weather icons

```html
<style>
    .sun { fill: #fbbf24; stroke: #f59e0b; stroke-width: 2; }
    .cloud { fill: #9ca3af; }
    .rain { fill: #3b82f6; }
</style>
<body>
    <svg width="300" height="100">
        <circle class="sun" cx="50" cy="50" r="25"/>
        <ellipse class="cloud" cx="150" cy="50" rx="30" ry="20"/>
        <circle class="cloud" cx="135" cy="45" rx="20" ry="20"/>
        <circle class="cloud" cx="165" cy="45" rx="20" ry="20"/>
        <rect class="rain" x="240" y="60" width="4" height="20"/>
        <rect class="rain" x="250" y="60" width="4" height="20"/>
        <rect class="rain" x="260" y="60" width="4" height="20"/>
    </svg>
</body>
```

### Example 15: Gradient-like effect with multiple shades

```html
<style>
    .shade-1 { fill: #1e3a8a; }
    .shade-2 { fill: #1e40af; }
    .shade-3 { fill: #2563eb; }
    .shade-4 { fill: #3b82f6; }
    .shade-5 { fill: #60a5fa; }
</style>
<body>
    <svg width="300" height="100">
        <rect class="shade-1" x="0" y="0" width="60" height="100"/>
        <rect class="shade-2" x="60" y="0" width="60" height="100"/>
        <rect class="shade-3" x="120" y="0" width="60" height="100"/>
        <rect class="shade-4" x="180" y="0" width="60" height="100"/>
        <rect class="shade-5" x="240" y="0" width="60" height="100"/>
    </svg>
</body>
```

---

## See Also

- [fill-opacity](/reference/cssproperties/css_prop_fill-opacity) - Control fill transparency separately
- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [stroke-width](/reference/cssproperties/css_prop_stroke-width) - Control stroke thickness
- [color](/reference/cssproperties/css_prop_color) - Text color property
- [opacity](/reference/cssproperties/css_prop_opacity) - Overall element transparency

---
