---
layout: default
title: fill-opacity
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# fill-opacity : SVG Fill Opacity Property

The `fill-opacity` property controls the transparency of SVG shape fills independently from the stroke. This property enables precise control over fill transparency while maintaining stroke opacity, essential for creating layered graphics and subtle visual effects in PDF documents.

## Usage

```css
selector {
    fill-opacity: value;
}
```

The fill-opacity property accepts a numeric value between 0.0 (fully transparent) and 1.0 (fully opaque).

---

## Supported Values

### Numeric Values
- `0.0` - Fully transparent (invisible fill)
- `0.5` - 50% transparent (semi-transparent)
- `1.0` - Fully opaque (default, solid fill)
- Any decimal value between 0.0 and 1.0

### Percentage Values
- `0%` - Fully transparent
- `50%` - 50% transparent
- `100%` - Fully opaque

---

## Supported Elements

The `fill-opacity` property applies to SVG elements including:
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

- Default fill-opacity value is `1.0` (fully opaque)
- Values outside the 0.0-1.0 range are clamped to the nearest valid value
- `fill-opacity` affects only the fill, not the stroke (use `stroke-opacity` for strokes)
- Can be combined with RGBA fill colors for additional transparency control
- When both RGBA alpha and fill-opacity are used, they multiply together
- Fill-opacity is independent of the overall `opacity` property
- Useful for creating overlapping transparent shapes
- PDF rendering maintains exact opacity values across viewers
- Semi-transparent fills blend with underlying content

---

## Data Binding

The `fill-opacity` property can be dynamically set using data binding expressions, enabling SVG fill transparency to reflect data values, confidence levels, or emphasis states.

### Example 1: Data value transparency in charts

```html
<style>
    .bar {
        fill: #3b82f6;
        stroke: #1e40af;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="400" height="200">
        {{#each dataPoints}}
        <rect class="bar"
              style="fill-opacity: {{value / 100}}"
              x="{{xPosition}}"
              y="{{yPosition}}"
              width="60"
              height="{{height}}"/>
        {{/each}}
    </svg>
</body>
```

With model data (opacity based on percentage values):
```json
{
    "dataPoints": [
        { "value": 30, "xPosition": 20, "yPosition": 140, "height": 60 },
        { "value": 70, "xPosition": 110, "yPosition": 60, "height": 140 },
        { "value": 50, "xPosition": 200, "yPosition": 100, "height": 100 },
        { "value": 90, "xPosition": 290, "yPosition": 20, "height": 180 }
    ]
}
```

### Example 2: Confidence levels in data visualization

```html
<style>
    .data-circle {
        fill: #10b981;
        stroke: #059669;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="400" height="150">
        {{#each measurements}}
        <circle class="data-circle"
                style="fill-opacity: {{confidence}}"
                cx="{{xPos}}"
                cy="75"
                r="25"/>
        {{/each}}
    </svg>
</body>
```

With model data:
```json
{
    "measurements": [
        { "value": 45, "confidence": 0.95, "xPos": 50 },
        { "value": 67, "confidence": 0.85, "xPos": 150 },
        { "value": 52, "confidence": 0.60, "xPos": 250 },
        { "value": 71, "confidence": 0.40, "xPos": 350 }
    ]
}
```

### Example 3: Conditional opacity based on status

```html
<style>
    .status-indicator {
        fill: #3b82f6;
        stroke: #1e40af;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="300" height="100">
        {{#each items}}
        <rect class="status-indicator"
              style="fill-opacity: {{isActive ? 1.0 : isPending ? 0.5 : 0.2}}"
              x="{{position}}"
              y="20"
              width="50"
              height="60"
              rx="5"/>
        {{/each}}
    </svg>
</body>
```

---

## Examples

### Example 1: Basic fill opacity

```html
<style>
    .opaque-rect {
        fill: blue;
        fill-opacity: 1.0;
    }
    .transparent-rect {
        fill: blue;
        fill-opacity: 0.3;
    }
</style>
<body>
    <svg width="300" height="150">
        <rect class="opaque-rect" x="20" y="20" width="100" height="100"/>
        <rect class="transparent-rect" x="150" y="20" width="100" height="100"/>
    </svg>
</body>
```

### Example 2: Overlapping transparent shapes

```html
<style>
    .circle-red {
        fill: red;
        fill-opacity: 0.5;
    }
    .circle-blue {
        fill: blue;
        fill-opacity: 0.5;
    }
</style>
<body>
    <svg width="250" height="150">
        <circle class="circle-red" cx="90" cy="75" r="60"/>
        <circle class="circle-blue" cx="160" cy="75" r="60"/>
    </svg>
</body>
```

### Example 3: Gradient-like effect with varying opacity

```html
<style>
    .bar { fill: #3b82f6; }
    .opacity-10 { fill-opacity: 0.1; }
    .opacity-30 { fill-opacity: 0.3; }
    .opacity-50 { fill-opacity: 0.5; }
    .opacity-70 { fill-opacity: 0.7; }
    .opacity-100 { fill-opacity: 1.0; }
</style>
<body>
    <svg width="300" height="100">
        <rect class="bar opacity-10" x="0" y="0" width="60" height="100"/>
        <rect class="bar opacity-30" x="60" y="0" width="60" height="100"/>
        <rect class="bar opacity-50" x="120" y="0" width="60" height="100"/>
        <rect class="bar opacity-70" x="180" y="0" width="60" height="100"/>
        <rect class="bar opacity-100" x="240" y="0" width="60" height="100"/>
    </svg>
</body>
```

### Example 4: Fill opacity with stroke

```html
<style>
    .shape {
        fill: orange;
        fill-opacity: 0.4;
        stroke: darkorange;
        stroke-width: 3;
        stroke-opacity: 1.0;
    }
</style>
<body>
    <svg width="200" height="200">
        <circle class="shape" cx="100" cy="100" r="70"/>
    </svg>
</body>
```

### Example 5: Percentage-based opacity

```html
<style>
    .rect-25 { fill: green; fill-opacity: 25%; }
    .rect-50 { fill: green; fill-opacity: 50%; }
    .rect-75 { fill: green; fill-opacity: 75%; }
    .rect-100 { fill: green; fill-opacity: 100%; }
</style>
<body>
    <svg width="400" height="100">
        <rect class="rect-25" x="10" y="10" width="80" height="80"/>
        <rect class="rect-50" x="110" y="10" width="80" height="80"/>
        <rect class="rect-75" x="210" y="10" width="80" height="80"/>
        <rect class="rect-100" x="310" y="10" width="80" height="80"/>
    </svg>
</body>
```

### Example 6: Layered visualization

```html
<style>
    .base-layer { fill: #1e40af; fill-opacity: 1.0; }
    .middle-layer { fill: #3b82f6; fill-opacity: 0.7; }
    .top-layer { fill: #60a5fa; fill-opacity: 0.4; }
</style>
<body>
    <svg width="300" height="200">
        <rect class="base-layer" x="20" y="20" width="260" height="160"/>
        <rect class="middle-layer" x="40" y="40" width="220" height="120"/>
        <rect class="top-layer" x="60" y="60" width="180" height="80"/>
    </svg>
</body>
```

### Example 7: Transparent chart overlay

```html
<style>
    .chart-bar { fill: #3b82f6; }
    .chart-highlight { fill: yellow; fill-opacity: 0.3; }
</style>
<body>
    <svg width="400" height="200">
        <rect class="chart-bar" x="20" y="80" width="60" height="100"/>
        <rect class="chart-bar" x="110" y="40" width="60" height="140"/>
        <rect class="chart-bar" x="200" y="60" width="60" height="120"/>
        <rect class="chart-bar" x="290" y="100" width="60" height="80"/>
        <rect class="chart-highlight" x="110" y="40" width="60" height="140"/>
    </svg>
</body>
```

### Example 8: Watermark effect

```html
<style>
    .watermark {
        fill: gray;
        fill-opacity: 0.15;
        font-size: 48pt;
        font-family: Arial, sans-serif;
        font-weight: bold;
    }
</style>
<body>
    <svg width="400" height="200">
        <text class="watermark" x="50" y="120" transform="rotate(-30 200 100)">CONFIDENTIAL</text>
    </svg>
</body>
```

### Example 9: Icon states with opacity

```html
<style>
    .icon-inactive { fill: #6b7280; fill-opacity: 0.3; }
    .icon-active { fill: #3b82f6; fill-opacity: 1.0; }
    .icon-hover { fill: #60a5fa; fill-opacity: 0.7; }
</style>
<body>
    <svg width="300" height="60">
        <circle class="icon-inactive" cx="50" cy="30" r="20"/>
        <circle class="icon-active" cx="150" cy="30" r="20"/>
        <circle class="icon-hover" cx="250" cy="30" r="20"/>
    </svg>
    <p>Inactive - Active - Hover states</p>
</body>
```

### Example 10: Venn diagram

```html
<style>
    .venn-a {
        fill: red;
        fill-opacity: 0.4;
        stroke: darkred;
        stroke-width: 2;
    }
    .venn-b {
        fill: blue;
        fill-opacity: 0.4;
        stroke: darkblue;
        stroke-width: 2;
    }
    .venn-c {
        fill: green;
        fill-opacity: 0.4;
        stroke: darkgreen;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="300" height="250">
        <circle class="venn-a" cx="100" cy="100" r="70"/>
        <circle class="venn-b" cx="200" cy="100" r="70"/>
        <circle class="venn-c" cx="150" cy="170" r="70"/>
    </svg>
</body>
```

### Example 11: Progress bar with transparency

```html
<style>
    .progress-track {
        fill: #e5e7eb;
        fill-opacity: 0.5;
    }
    .progress-bar {
        fill: #10b981;
        fill-opacity: 0.8;
    }
</style>
<body>
    <svg width="300" height="40">
        <rect class="progress-track" x="0" y="0" width="300" height="40" rx="20"/>
        <rect class="progress-bar" x="0" y="0" width="210" height="40" rx="20"/>
    </svg>
    <p>70% Complete</p>
</body>
```

### Example 12: Heat map squares

```html
<style>
    .heat { fill: red; }
    .heat-low { fill-opacity: 0.2; }
    .heat-medium { fill-opacity: 0.5; }
    .heat-high { fill-opacity: 0.8; }
    .heat-max { fill-opacity: 1.0; }
</style>
<body>
    <svg width="200" height="50">
        <rect class="heat heat-low" x="0" y="0" width="40" height="40"/>
        <rect class="heat heat-medium" x="50" y="0" width="40" height="40"/>
        <rect class="heat heat-high" x="100" y="0" width="40" height="40"/>
        <rect class="heat heat-max" x="150" y="0" width="40" height="40"/>
    </svg>
</body>
```

### Example 13: Background pattern with opacity

```html
<style>
    .pattern-dot {
        fill: #3b82f6;
        fill-opacity: 0.1;
    }
</style>
<body>
    <svg width="300" height="200">
        <circle class="pattern-dot" cx="50" cy="50" r="20"/>
        <circle class="pattern-dot" cx="150" cy="50" r="20"/>
        <circle class="pattern-dot" cx="250" cy="50" r="20"/>
        <circle class="pattern-dot" cx="50" cy="150" r="20"/>
        <circle class="pattern-dot" cx="150" cy="150" r="20"/>
        <circle class="pattern-dot" cx="250" cy="150" r="20"/>
    </svg>
</body>
```

### Example 14: Shadow effect simulation

```html
<style>
    .shape-main {
        fill: #3b82f6;
        fill-opacity: 1.0;
    }
    .shape-shadow {
        fill: black;
        fill-opacity: 0.2;
    }
</style>
<body>
    <svg width="200" height="200">
        <rect class="shape-shadow" x="45" y="45" width="100" height="100"/>
        <rect class="shape-main" x="40" y="40" width="100" height="100"/>
    </svg>
</body>
```

### Example 15: Data visualization with confidence levels

```html
<style>
    .data-point { fill: #3b82f6; stroke: #1e40af; stroke-width: 2; }
    .confidence-high { fill-opacity: 1.0; }
    .confidence-medium { fill-opacity: 0.6; }
    .confidence-low { fill-opacity: 0.3; }
</style>
<body>
    <svg width="400" height="150">
        <circle class="data-point confidence-high" cx="50" cy="75" r="25"/>
        <circle class="data-point confidence-high" cx="150" cy="75" r="25"/>
        <circle class="data-point confidence-medium" cx="250" cy="75" r="25"/>
        <circle class="data-point confidence-low" cx="350" cy="75" r="25"/>
    </svg>
    <p>Data points with varying confidence levels</p>
</body>
```

---

## See Also

- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [opacity](/reference/cssproperties/css_prop_opacity) - Overall element transparency
- [stroke-opacity](/reference/cssproperties/css_prop_stroke-opacity) - SVG stroke transparency
- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property

---
