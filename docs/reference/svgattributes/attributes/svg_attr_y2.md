---
layout: default
title: y2
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @y2 : The Linear Gradient End Y Coordinate Attribute

The `y2` attribute defines the Y-coordinate of the ending point for a linear gradient vector. Together with `x2`, it establishes the destination point where the gradient ends, completing the gradient vector that determines the direction, angle, and extent of the color transition.

## Usage

The `y2` attribute is used to:
- Define the vertical ending position of a linear gradient
- Control gradient angle and direction when combined with x1/y1/x2
- Support both relative (percentage) and absolute (unit-based) coordinates
- Enable vertical, diagonal, and custom-angle gradients
- Work with both objectBoundingBox (default) and userSpaceOnUse coordinate systems
- Create data-driven gradient effects through data binding
- Position gradients precisely for charts and data visualizations

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <linearGradient id="verticalGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </linearGradient>
    </defs>
    <rect x="10" y="10" width="380" height="280" fill="url(#verticalGrad)" />
</svg>
```

---

## Supported Values

| Value Type | Format | Description | Example |
|------------|--------|-------------|---------|
| Percentage | `0%` to `100%` | Relative to shape bounding box (objectBoundingBox mode) | `y2="100%"` |
| Decimal | `0` to `1.0` | Relative coordinate (0=top, 1=bottom) | `y2="1.0"` |
| Length Units | `pt`, `px`, `mm`, `cm`, `in` | Absolute coordinates (userSpaceOnUse mode) | `y2="300pt"` |
| Default | `0%` | Used when attribute is omitted (horizontal gradient) | - |

### Common Patterns

```html
<!-- No vertical change (horizontal gradient) - DEFAULT -->
<linearGradient id="g1" y2="0%">

<!-- Full height (vertical gradient) -->
<linearGradient id="g2" y2="100%">

<!-- Half height -->
<linearGradient id="g3" y2="50%">

<!-- Absolute positioning -->
<linearGradient id="g4" y2="300pt" gradientUnits="userSpaceOnUse">
```

---

## Supported Elements

The `y2` attribute is supported on:

- **[&lt;linearGradient&gt;](/reference/svgtags/linearGradient.html)** - Defines linear gradient end Y position

Note: This attribute is NOT used with `<radialGradient>` elements.

---

## Data Binding

### Dynamic Vertical Gradient Extent

Bind the y2 coordinate to data for dynamic gradient range:

```html
<!-- Model: { endY: 80 } -->
<svg width="400" height="400">
    <defs>
        <linearGradient id="dynamicVertical"
                        x1="0%" y1="0%"
                        x2="0%" y2="{{model.endY}}%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </linearGradient>
    </defs>
    <rect width="400" height="400" fill="url(#dynamicVertical)" />
</svg>
```

### Chart Height-Based Gradients

Position gradient endpoint based on chart dimensions:

```html
<!-- Model: { chartHeight: 300, dataMaxHeight: 250 } -->
<svg width="500" height="{{model.chartHeight}}">
    <defs>
        <linearGradient id="chartGrad"
                        x1="0%" y1="0%"
                        x2="0%" y2="{{(model.dataMaxHeight / model.chartHeight) * 100}}%">
            <stop offset="0%" stop-color="#3498db" stop-opacity="0.8" />
            <stop offset="100%" stop-color="#3498db" stop-opacity="0.1" />
        </linearGradient>
    </defs>
    <rect width="500" height="{{model.chartHeight}}" fill="url(#chartGrad)" />
</svg>
```

### Data-Driven Diagonal Gradients

Calculate gradient angle from data values:

```html
<!-- Model: { angle: 45 } -->
<svg width="400" height="300">
    <defs>
        <linearGradient id="angleGrad"
                        x1="{{50 - Math.cos(model.angle * Math.PI / 180) * 50}}%"
                        y1="{{50 - Math.sin(model.angle * Math.PI / 180) * 50}}%"
                        x2="{{50 + Math.cos(model.angle * Math.PI / 180) * 50}}%"
                        y2="{{50 + Math.sin(model.angle * Math.PI / 180) * 50}}%">
            <stop offset="0%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#angleGrad)" />
</svg>
```

### Conditional Vertical Extent

Change gradient extent based on data conditions:

```html
<!-- Model: { fillLevel: 65, maxLevel: 100 } -->
<svg width="200" height="400">
    <defs>
        <linearGradient id="fillGrad"
                        x1="0%"
                        y1="{{100 - (model.fillLevel / model.maxLevel * 100)}}%"
                        x2="0%"
                        y2="100%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>

    <rect x="50" y="0" width="100" height="400" rx="50" fill="#ecf0f1" />
    <rect x="50"
          y="{{400 - (model.fillLevel / model.maxLevel * 400)}}"
          width="100"
          height="{{model.fillLevel / model.maxLevel * 400}}"
          rx="50"
          fill="url(#fillGrad)" />
</svg>
```

### Temperature Range Visualization

Create vertical gradients representing value ranges:

```html
<!-- Model: { minTemp: -20, maxTemp: 40, warmThreshold: 20 } -->
<svg width="150" height="400">
    <defs>
        <linearGradient id="tempRange"
                        x1="0%" y1="0%"
                        x2="0%"
                        y2="{{((model.warmThreshold - model.minTemp) / (model.maxTemp - model.minTemp)) * 100}}%">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>

    <rect x="50" y="20" width="50" height="360" rx="25" fill="url(#tempRange)" />
</svg>
```

---

## Notes

### Gradient Vector Direction

The gradient vector runs from start point (x1, y1) to end point (x2, y2):
- The y2 coordinate determines the vertical extent
- Color transitions perpendicular to the vector
- Vector length affects gradient behavior with spreadMethod

### Coordinate Systems

The `y2` attribute behaves differently based on `gradientUnits`:

1. **objectBoundingBox (default)**: Relative to filled shape
   - Values 0-1 or 0%-100%
   - `0%` = top edge, `100%` = bottom edge
   - Gradient automatically scales with shape size
   - Most common for responsive designs

2. **userSpaceOnUse**: Absolute document coordinates
   - Values in pt, px, mm, cm, in
   - Fixed position in document space
   - Consistent across multiple shapes

### Common Gradient Direction Patterns

```html
<!-- Horizontal (no Y change) - DEFAULT -->
<linearGradient x1="0%" y1="0%" x2="100%" y2="0%">

<!-- Vertical top-to-bottom -->
<linearGradient x1="0%" y1="0%" x2="0%" y2="100%">

<!-- Vertical bottom-to-top -->
<linearGradient x1="0%" y1="100%" x2="0%" y2="0%">

<!-- Diagonal top-left to bottom-right -->
<linearGradient x1="0%" y1="0%" x2="100%" y2="100%">

<!-- Diagonal top-right to bottom-left -->
<linearGradient x1="100%" y1="0%" x2="0%" y2="100%">

<!-- Diagonal bottom-left to top-right -->
<linearGradient x1="0%" y1="100%" x2="100%" y2="0%">
```

### Default Behavior

- Default value: `0%` (same as y1, creating horizontal gradient)
- Default gradient: horizontal left-to-right
- Must be used with `<linearGradient>` only

### Use Cases for y2

Common scenarios where y2 is essential:
- Vertical bar charts with gradient fills
- Progress indicators and level gauges
- Sky/ground gradients in illustrations
- Card and panel backgrounds
- Vertical data visualizations
- Temperature and depth indicators

---

## Examples

### Basic Vertical Gradient (Top to Bottom)

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="vertical" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#vertical)" />
</svg>
```

### Vertical Gradient (Bottom to Top)

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="bottomTop" x1="0%" y1="100%" x2="0%" y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#bottomTop)" />
</svg>
```

### Partial Height Gradient

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="partial" x1="0%" y1="0%" x2="0%" y2="50%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#partial)" />
</svg>
```

### Diagonal Gradient (Top-Left to Bottom-Right)

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="diag1" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#diag1)" />
</svg>
```

### Diagonal Gradient (Bottom-Left to Top-Right)

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="diag2" x1="0%" y1="100%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#f39c12" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#diag2)" />
</svg>
```

### Sky Background Gradient

```html
<svg width="600" height="400">
    <defs>
        <linearGradient id="sky" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#1e3c72" />
            <stop offset="50%" stop-color="#2a5298" />
            <stop offset="100%" stop-color="#7e8ba3" />
        </linearGradient>
    </defs>

    <rect width="600" height="400" fill="url(#sky)" />
</svg>
```

### Vertical Bar Chart with Gradient

```html
<svg width="500" height="400">
    <defs>
        <linearGradient id="barGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#1a5490" />
        </linearGradient>
    </defs>

    <!-- Y-axis line -->
    <line x1="50" y1="50" x2="50" y2="370" stroke="#333" stroke-width="2" />
    <line x1="50" y1="370" x2="470" y2="370" stroke="#333" stroke-width="2" />

    <!-- Bars -->
    <rect x="80" y="150" width="60" height="220" fill="url(#barGrad)" />
    <rect x="160" y="100" width="60" height="270" fill="url(#barGrad)" />
    <rect x="240" y="180" width="60" height="190" fill="url(#barGrad)" />
    <rect x="320" y="130" width="60" height="240" fill="url(#barGrad)" />
    <rect x="400" y="160" width="60" height="210" fill="url(#barGrad)" />
</svg>
```

### Progress Bar with Vertical Fill

```html
<svg width="100" height="400">
    <defs>
        <linearGradient id="progress" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>

    <!-- Background -->
    <rect x="20" y="20" width="60" height="360" rx="30" fill="#ecf0f1" />
    <!-- Progress (75%) -->
    <rect x="20" y="110" width="60" height="270" rx="30" fill="url(#progress)" />

    <text x="50" y="400" text-anchor="middle" font-size="14" font-weight="bold">
        75%
    </text>
</svg>
```

### Temperature Gauge with Gradient

```html
<svg width="150" height="400">
    <defs>
        <linearGradient id="temp" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="20%" stop-color="#f39c12" />
            <stop offset="40%" stop-color="#f1c40f" />
            <stop offset="60%" stop-color="#2ecc71" />
            <stop offset="80%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2980b9" />
        </linearGradient>
    </defs>

    <rect x="50" y="30" width="50" height="340" rx="25" fill="url(#temp)" />
    <circle cx="75" cy="380" r="20" fill="#2980b9" />

    <text x="10" y="40" font-size="12">40°</text>
    <text x="10" y="200" font-size="12">20°</text>
    <text x="10" y="360" font-size="12">0°</text>
</svg>
```

### Card Header with Vertical Gradient

```html
<svg width="400" height="500">
    <defs>
        <linearGradient id="cardHeader" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#667eea" />
            <stop offset="100%" stop-color="#764ba2" />
        </linearGradient>
    </defs>

    <!-- Card -->
    <rect width="400" height="500" rx="12" fill="#ffffff" stroke="#ddd" stroke-width="1" />
    <!-- Header -->
    <rect width="400" height="180" rx="12" fill="url(#cardHeader)" />

    <text x="20" y="60" fill="white" font-size="36" font-weight="bold">
        Dashboard
    </text>
    <text x="20" y="100" fill="white" font-size="16" opacity="0.9">
        Analytics Overview
    </text>
</svg>
```

### Vertical Heatmap Legend

```html
<svg width="150" height="400">
    <defs>
        <linearGradient id="heatLegend" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#c0392b" />
            <stop offset="20%" stop-color="#e74c3c" />
            <stop offset="40%" stop-color="#e67e22" />
            <stop offset="60%" stop-color="#f1c40f" />
            <stop offset="80%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>

    <rect x="50" y="50" width="50" height="300" fill="url(#heatLegend)" />
    <text x="110" y="60" font-size="12">High</text>
    <text x="110" y="345" font-size="12">Low</text>
</svg>
```

### Battery Indicator

```html
<svg width="150" height="300">
    <defs>
        <linearGradient id="battery" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>

    <!-- Battery outline -->
    <rect x="40" y="40" width="70" height="220" rx="10" fill="none" stroke="#2c3e50" stroke-width="3" />
    <!-- Battery terminal -->
    <rect x="55" y="25" width="40" height="15" rx="5" fill="#2c3e50" />
    <!-- Battery fill (80%) -->
    <rect x="45" y="84" width="60" height="170" rx="8" fill="url(#battery)" />
</svg>
```

### Data-Driven Fill Level

```html
<!-- Model: { level: 70, maxLevel: 100 } -->
<svg width="200" height="400">
    <defs>
        <linearGradient id="fillLevel"
                        x1="0%"
                        y1="{{100 - model.level}}%"
                        x2="0%"
                        y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2980b9" />
        </linearGradient>
    </defs>

    <rect x="50" y="50" width="100" height="300" rx="50" fill="#ecf0f1" stroke="#bdc3c7" stroke-width="2" />
    <rect x="55"
          y="{{50 + (300 * (1 - model.level / 100))}}"
          width="90"
          height="{{300 * model.level / 100}}"
          rx="45"
          fill="url(#fillLevel)" />

    <text x="100" y="370" text-anchor="middle" font-size="24" font-weight="bold" fill="#333">
        {{model.level}}%
    </text>
</svg>
```

### Vertical Slider

```html
<svg width="100" height="400">
    <defs>
        <linearGradient id="slider" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#1a5490" />
        </linearGradient>
    </defs>

    <rect x="40" y="50" width="20" height="300" rx="10" fill="url(#slider)" />
    <circle cx="50" cy="200" r="18" fill="white" stroke="#3498db" stroke-width="4" />
</svg>
```

### Area Chart with Vertical Gradient Fill

```html
<svg width="600" height="300">
    <defs>
        <linearGradient id="areaFill" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" stop-opacity="0.8" />
            <stop offset="100%" stop-color="#3498db" stop-opacity="0.05" />
        </linearGradient>
    </defs>

    <path d="M 0,200 L 100,180 L 200,150 L 300,170 L 400,140 L 500,160 L 600,130 L 600,300 L 0,300 Z"
          fill="url(#areaFill)" />
    <path d="M 0,200 L 100,180 L 200,150 L 300,170 L 400,140 L 500,160 L 600,130"
          fill="none" stroke="#3498db" stroke-width="3" />
</svg>
```

### Depth Indicator

```html
<svg width="200" height="400">
    <defs>
        <linearGradient id="depth" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#e8f8f5" />
            <stop offset="25%" stop-color="#48c9b0" />
            <stop offset="50%" stop-color="#16a085" />
            <stop offset="75%" stop-color="#0e6655" />
            <stop offset="100%" stop-color="#0a3d3d" />
        </linearGradient>
    </defs>

    <rect width="200" height="400" fill="url(#depth)" />
    <text x="10" y="30" fill="white" font-size="14">0m</text>
    <text x="10" y="130" fill="white" font-size="14">10m</text>
    <text x="10" y="230" fill="white" font-size="14">20m</text>
    <text x="10" y="330" fill="white" font-size="14">30m</text>
    <text x="10" y="390" fill="white" font-size="14">40m</text>
</svg>
```

---

## See Also

- [x1](/reference/svgattributes/x1.html) - Linear gradient start X coordinate
- [y1](/reference/svgattributes/y1.html) - Linear gradient start Y coordinate
- [x2](/reference/svgattributes/x2.html) - Linear gradient end X coordinate
- [linearGradient](/reference/svgtags/linearGradient.html) - Linear gradient definition element
- [gradientUnits](/reference/svgattributes/gradientUnits.html) - Coordinate system mode
- [spreadMethod](/reference/svgattributes/spreadMethod.html) - Gradient spread behavior
- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [Data Binding](/reference/binding/) - Data binding and expressions

---
