---
layout: default
title: stroke
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# stroke : SVG Stroke Color Property

The `stroke` property sets the outline color of SVG shapes and vector elements in PDF documents. This property is essential for creating borders around shapes, drawing lines, and defining the edges of vector graphics.

## Usage

```css
selector {
    stroke: value;
}
```

The stroke property accepts multiple value formats including named colors, hexadecimal notation, RGB/RGBA functions, and special keywords like `none`.

---

## Supported Values

### Named Colors
Standard CSS color names such as `red`, `blue`, `green`, `black`, `orange`, etc.

### Hexadecimal Colors
- Short form: `#RGB` (e.g., `#f00` for red)
- Long form: `#RRGGBB` (e.g., `#ff0000` for red)

### RGB/RGBA Functions
- RGB: `rgb(red, green, blue)` where values are 0-255
- RGBA: `rgba(red, green, blue, alpha)` where alpha is 0.0-1.0 for transparency

### Special Keywords
- `none` - No stroke (no outline)
- `transparent` - Same as `none`

---

## Supported Elements

The `stroke` property applies to SVG elements including:
- `<rect>` rectangles
- `<circle>` circles
- `<ellipse>` ellipses
- `<polygon>` polygons
- `<polyline>` polylines
- `<line>` lines
- `<path>` paths
- `<text>` SVG text elements
- All other SVG shape elements

---

## Notes

- The `stroke` property only affects SVG elements, not regular HTML elements
- Use `stroke: none` to remove outlines from shapes
- RGBA values enable transparent strokes that blend with underlying content
- Stroke colors are rendered precisely in PDF vector format
- Works in combination with `fill` for complete shape styling
- Use `stroke-width` to control the thickness of the stroke
- Use `stroke-opacity` for separate opacity control of the stroke
- Default stroke value is typically `none` (no stroke)
- Strokes are drawn centered on the shape's path
- Vector strokes maintain quality at any zoom level in PDF viewers
- For text outlines in regular HTML, use `text-stroke` (if supported) or SVG text elements

---

## Data Binding

The `stroke` property can be dynamically set using data binding expressions, enabling SVG stroke colors to change based on data categories, status conditions, or theme configurations.

### Example 1: Status-based outline colors

```html
<style>
    .status-box {
        fill: white;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="400" height="100">
        {{#each statusItems}}
        <rect class="status-box"
              style="stroke: {{statusColor}}"
              x="{{position}}"
              y="20"
              width="80"
              height="60"
              rx="5"/>
        {{/each}}
    </svg>
</body>
```

With model data:
```json
{
    "statusItems": [
        { "status": "Success", "statusColor": "#22c55e", "position": 20 },
        { "status": "Warning", "statusColor": "#f59e0b", "position": 120 },
        { "status": "Error", "statusColor": "#ef4444", "position": 220 }
    ]
}
```

### Example 2: Conditional network connection colors

```html
<style>
    .node {
        fill: white;
        stroke-width: 2;
    }
    .connection {
        stroke-width: 2;
    }
</style>
<body>
    <svg width="400" height="200">
        {{#each connections}}
        <line class="connection"
              style="stroke: {{isActive ? '#10b981' : '#9ca3af'}}"
              x1="{{x1}}"
              y1="{{y1}}"
              x2="{{x2}}"
              y2="{{y2}}"/>
        {{/each}}

        {{#each nodes}}
        <circle class="node"
                style="stroke: {{isOnline ? '#3b82f6' : '#6b7280'}}"
                cx="{{x}}"
                cy="{{y}}"
                r="20"/>
        {{/each}}
    </svg>
</body>
```

### Example 3: Category-based border colors from theme

```html
<style>
    .category-card {
        fill: white;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="350" height="250">
        {{#each categories}}
        <rect class="category-card"
              style="stroke: {{theme.colors[type]}}"
              x="{{xPos}}"
              y="{{yPos}}"
              width="100"
              height="100"
              rx="8"/>
        {{/each}}
    </svg>
</body>
```

With theme configuration:
```json
{
    "theme": {
        "colors": {
            "primary": "#3b82f6",
            "secondary": "#10b981",
            "accent": "#f59e0b",
            "neutral": "#6b7280"
        }
    },
    "categories": [
        { "type": "primary", "xPos": 20, "yPos": 20 },
        { "type": "secondary", "xPos": 140, "yPos": 20 },
        { "type": "accent", "xPos": 20, "yPos": 130 }
    ]
}
```

---

## Examples

### Example 1: Basic rectangle with stroke

```html
<style>
    .outlined-rect {
        fill: lightblue;
        stroke: darkblue;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="200" height="100">
        <rect class="outlined-rect" x="10" y="10" width="180" height="80"/>
    </svg>
</body>
```

### Example 2: Circle with hex color stroke

```html
<style>
    .circle-outline {
        fill: none;
        stroke: #ff6600;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="150" height="150">
        <circle class="circle-outline" cx="75" cy="75" r="60"/>
    </svg>
</body>
```

### Example 3: Multiple strokes with different colors

```html
<style>
    .stroke-red { stroke: #ef4444; stroke-width: 2; fill: none; }
    .stroke-green { stroke: #10b981; stroke-width: 2; fill: none; }
    .stroke-blue { stroke: #3b82f6; stroke-width: 2; fill: none; }
</style>
<body>
    <svg width="300" height="100">
        <rect class="stroke-red" x="10" y="10" width="80" height="80"/>
        <circle class="stroke-green" cx="150" cy="50" r="40"/>
        <polygon class="stroke-blue" points="240,10 280,90 200,90"/>
    </svg>
</body>
```

### Example 4: Transparent stroke with RGBA

```html
<style>
    .semi-transparent-stroke {
        fill: yellow;
        stroke: rgba(255, 0, 0, 0.5);
        stroke-width: 4;
    }
</style>
<body>
    <svg width="150" height="150">
        <circle class="semi-transparent-stroke" cx="75" cy="75" r="60"/>
    </svg>
</body>
```

### Example 5: Lines with different stroke colors

```html
<style>
    .line-red { stroke: red; stroke-width: 2; }
    .line-green { stroke: green; stroke-width: 2; }
    .line-blue { stroke: blue; stroke-width: 2; }
</style>
<body>
    <svg width="300" height="150">
        <line class="line-red" x1="10" y1="30" x2="280" y2="30"/>
        <line class="line-green" x1="10" y1="70" x2="280" y2="70"/>
        <line class="line-blue" x1="10" y1="110" x2="280" y2="110"/>
    </svg>
</body>
```

### Example 6: Path with custom stroke

```html
<style>
    .custom-path {
        fill: none;
        stroke: #8b5cf6;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="300" height="200">
        <path class="custom-path" d="M 10 100 Q 150 10 290 100"/>
    </svg>
</body>
```

### Example 7: Border effect with fill and stroke

```html
<style>
    .bordered-shape {
        fill: #dbeafe;
        stroke: #1e40af;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="200" height="200">
        <rect class="bordered-shape" x="20" y="20" width="160" height="160" rx="10"/>
    </svg>
</body>
```

### Example 8: Icon with stroke

```html
<style>
    .icon-heart {
        fill: #fecaca;
        stroke: #dc2626;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="100" height="100" viewBox="0 0 24 24">
        <path class="icon-heart" d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z"/>
    </svg>
</body>
```

### Example 9: Chart with outlined bars

```html
<style>
    .bar-fill { fill: #60a5fa; }
    .bar-outline { stroke: #1e40af; stroke-width: 2; }
</style>
<body>
    <svg width="400" height="200">
        <rect class="bar-fill bar-outline" x="20" y="80" width="60" height="100"/>
        <rect class="bar-fill bar-outline" x="110" y="40" width="60" height="140"/>
        <rect class="bar-fill bar-outline" x="200" y="60" width="60" height="120"/>
        <rect class="bar-fill bar-outline" x="290" y="100" width="60" height="80"/>
    </svg>
</body>
```

### Example 10: Dashed stroke pattern

```html
<style>
    .dashed-line {
        stroke: #374151;
        stroke-width: 2;
        stroke-dasharray: 5,5;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="dashed-line" x1="10" y1="50" x2="290" y2="50"/>
    </svg>
</body>
```

### Example 11: Traffic signs

```html
<style>
    .sign-stop {
        fill: #dc2626;
        stroke: white;
        stroke-width: 3;
    }
    .sign-yield {
        fill: #fef3c7;
        stroke: #dc2626;
        stroke-width: 3;
    }
    .sign-go {
        fill: #10b981;
        stroke: white;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="300" height="100">
        <polygon class="sign-stop" points="30,15 60,15 75,30 75,60 60,75 30,75 15,60 15,30"/>
        <polygon class="sign-yield" points="120,15 180,75 140,75"/>
        <circle class="sign-go" cx="250" cy="50" r="30"/>
    </svg>
</body>
```

### Example 12: Network diagram nodes

```html
<style>
    .node-active {
        fill: white;
        stroke: #10b981;
        stroke-width: 3;
    }
    .node-inactive {
        fill: white;
        stroke: #9ca3af;
        stroke-width: 2;
    }
    .connection {
        stroke: #d1d5db;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="300" height="150">
        <line class="connection" x1="75" y1="75" x2="225" y2="75"/>
        <circle class="node-active" cx="75" cy="75" r="30"/>
        <circle class="node-inactive" cx="225" cy="75" r="30"/>
    </svg>
</body>
```

### Example 13: Callout boxes

```html
<style>
    .callout-info {
        fill: #dbeafe;
        stroke: #3b82f6;
        stroke-width: 2;
    }
    .callout-warning {
        fill: #fef3c7;
        stroke: #f59e0b;
        stroke-width: 2;
    }
    .callout-error {
        fill: #fee2e2;
        stroke: #ef4444;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="300" height="200">
        <rect class="callout-info" x="10" y="10" width="280" height="50" rx="5"/>
        <rect class="callout-warning" x="10" y="75" width="280" height="50" rx="5"/>
        <rect class="callout-error" x="10" y="140" width="280" height="50" rx="5"/>
    </svg>
</body>
```

### Example 14: Timeline visualization

```html
<style>
    .timeline-line {
        stroke: #9ca3af;
        stroke-width: 2;
    }
    .timeline-milestone {
        fill: white;
        stroke: #3b82f6;
        stroke-width: 3;
    }
    .timeline-complete {
        fill: #3b82f6;
        stroke: #1e40af;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="400" height="100">
        <line class="timeline-line" x1="50" y1="50" x2="350" y2="50"/>
        <circle class="timeline-complete" cx="50" cy="50" r="12"/>
        <circle class="timeline-complete" cx="150" cy="50" r="12"/>
        <circle class="timeline-milestone" cx="250" cy="50" r="12"/>
        <circle class="timeline-milestone" cx="350" cy="50" r="12"/>
    </svg>
</body>
```

### Example 15: Architectural diagram

```html
<style>
    .wall {
        fill: none;
        stroke: black;
        stroke-width: 4;
    }
    .door {
        fill: none;
        stroke: #8b4513;
        stroke-width: 2;
    }
    .window {
        fill: lightblue;
        stroke: #4b5563;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="300" height="250">
        <!-- Room outline -->
        <rect class="wall" x="20" y="20" width="260" height="210"/>
        <!-- Door -->
        <rect class="door" x="120" y="225" width="60" height="5"/>
        <!-- Windows -->
        <rect class="window" x="50" y="20" width="40" height="5"/>
        <rect class="window" x="210" y="20" width="40" height="5"/>
    </svg>
</body>
```

---

## See Also

- [stroke-width](/reference/cssproperties/css_prop_stroke-width) - Control stroke thickness
- [stroke-opacity](/reference/cssproperties/css_prop_stroke-opacity) - SVG stroke transparency
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [color](/reference/cssproperties/css_prop_color) - Text color property

---
