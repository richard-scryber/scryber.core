---
layout: default
title: y1
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @y1 : The Linear Gradient Start Y Coordinate Attribute

The `y1` attribute defines the Y-coordinate of the starting point for a linear gradient vector. Together with `x1`, it establishes the origin point from which the gradient begins, determining the vertical component of the gradient's angle and direction.

## Usage

The `y1` attribute is used to:
- Define the vertical starting position of a linear gradient
- Control gradient angle and direction when combined with x1/x2/y2
- Support both relative (percentage) and absolute (unit-based) coordinates
- Enable vertical, diagonal, and custom-angle gradients
- Work with both objectBoundingBox (default) and userSpaceOnUse coordinate systems
- Create data-driven gradient orientations through data binding
- Position gradients for charts, visualizations, and UI elements

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
        <linearGradient id="verticalGradient" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </linearGradient>
    </defs>
    <rect x="10" y="10" width="380" height="180" fill="url(#verticalGradient)" />
</svg>
```

---

## Supported Values

| Value Type | Format | Description | Example |
|------------|--------|-------------|---------|
| Percentage | `0%` to `100%` | Relative to shape bounding box (objectBoundingBox mode) | `y1="0%"` |
| Decimal | `0` to `1.0` | Relative coordinate (0=top, 1=bottom) | `y1="0.5"` |
| Length Units | `pt`, `px`, `mm`, `cm`, `in` | Absolute coordinates (userSpaceOnUse mode) | `y1="50pt"` |
| Default | `0%` | Used when attribute is omitted | - |

### Common Patterns

```html
<!-- Top edge (default) -->
<linearGradient id="g1" y1="0%">

<!-- Center -->
<linearGradient id="g2" y1="50%">

<!-- Bottom edge -->
<linearGradient id="g3" y1="100%">

<!-- Absolute positioning -->
<linearGradient id="g4" y1="100pt" gradientUnits="userSpaceOnUse">
```

---

## Supported Elements

The `y1` attribute is supported on:

- **[&lt;linearGradient&gt;](/reference/svgtags/linearGradient.html)** - Defines linear gradient start Y position

Note: This attribute is NOT used with `<radialGradient>` elements, which use `cx`/`cy` and `fx`/`fy` instead.

---

## Data Binding

### Dynamic Vertical Gradient Positioning

Bind the y1 coordinate to data for dynamic gradient start position:

```html
<!-- Model: { startY: 20 } -->
<svg width="400" height="300">
    <defs>
        <linearGradient id="dynamicY"
                        x1="0%"
                        y1="{{model.startY}}%"
                        x2="0%"
                        y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#dynamicY)" />
</svg>
```

### Data-Driven Gradient Angles

Calculate gradient angles from data:

```html
<!-- Model: { angle: 135 } -->
<!-- Creates gradient at specified angle in degrees -->
<svg width="400" height="300">
    <defs>
        <linearGradient id="angleGrad"
                        x1="{{Math.cos((model.angle + 180) * Math.PI / 180) * 50 + 50}}%"
                        y1="{{Math.sin((model.angle + 180) * Math.PI / 180) * 50 + 50}}%"
                        x2="{{Math.cos(model.angle * Math.PI / 180) * 50 + 50}}%"
                        y2="{{Math.sin(model.angle * Math.PI / 180) * 50 + 50}}%">
            <stop offset="0%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#angleGrad)" />
</svg>
```

### Chart Background with Data Position

Position gradient based on chart data ranges:

```html
<!-- Model: { chartTopMargin: 10, chartHeight: 280 } -->
<svg width="500" height="300">
    <defs>
        <linearGradient id="chartBg"
                        x1="0%"
                        y1="{{(model.chartTopMargin / 300) * 100}}%"
                        x2="0%"
                        y2="{{((model.chartTopMargin + model.chartHeight) / 300) * 100}}%">
            <stop offset="0%" stop-color="#ecf0f1" />
            <stop offset="100%" stop-color="#ffffff" />
        </linearGradient>
    </defs>
    <rect width="500" height="300" fill="url(#chartBg)" />
</svg>
```

### Conditional Gradient Direction

Change gradient direction based on data orientation:

```html
<!-- Model: { orientation: "vertical", color1: "#2ecc71", color2: "#27ae60" } -->
<svg width="400" height="200">
    <defs>
        <linearGradient id="orientGrad"
                        x1="{{model.orientation === 'horizontal' ? '0%' : '0%'}}"
                        y1="{{model.orientation === 'horizontal' ? '0%' : '0%'}}"
                        x2="{{model.orientation === 'horizontal' ? '100%' : '0%'}}"
                        y2="{{model.orientation === 'horizontal' ? '0%' : '100%'}}">
            <stop offset="0%" stop-color="{{model.color1}}" />
            <stop offset="100%" stop-color="{{model.color2}}" />
        </linearGradient>
    </defs>
    <rect width="400" height="200" fill="url(#orientGrad)" />
</svg>
```

### Temperature Gradient Based on Range

Create vertical gradients representing temperature or value ranges:

```html
<!-- Model: { minTemp: -20, maxTemp: 40, currentTemp: 15 } -->
<svg width="100" height="400">
    <defs>
        <linearGradient id="tempGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="50%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>

    <rect width="100" height="400" fill="url(#tempGrad)" />
    <!-- Temperature indicator -->
    <rect x="0"
          y="{{((model.currentTemp - model.minTemp) / (model.maxTemp - model.minTemp)) * 400}}"
          width="100" height="5" fill="white" />
</svg>
```

---

## Notes

### Coordinate Systems

The `y1` attribute behaves differently based on `gradientUnits`:

1. **objectBoundingBox (default)**: Relative to filled shape
   - Values 0-1 or 0%-100%
   - `0%` = top edge, `50%` = center, `100%` = bottom edge
   - Gradient automatically scales with shape size
   - Most common for responsive designs

2. **userSpaceOnUse**: Absolute document coordinates
   - Values in pt, px, mm, cm, in
   - Fixed position in document space
   - Useful for consistent gradients across multiple shapes

### Gradient Direction Patterns

Common gradient orientations using y1/y2:

```html
<!-- Horizontal (no Y change) -->
<linearGradient x1="0%" y1="0%" x2="100%" y2="0%">

<!-- Vertical (top to bottom) -->
<linearGradient x1="0%" y1="0%" x2="0%" y2="100%">

<!-- Vertical (bottom to top) -->
<linearGradient x1="0%" y1="100%" x2="0%" y2="0%">

<!-- Diagonal (top-left to bottom-right) -->
<linearGradient x1="0%" y1="0%" x2="100%" y2="100%">

<!-- Diagonal (bottom-left to top-right) -->
<linearGradient x1="0%" y1="100%" x2="100%" y2="0%">
```

### Default Behavior

- Default value: `0%` (top edge)
- Default gradient: horizontal left-to-right (x1="0%", y1="0%", x2="100%", y2="0%")
- Must be used with `<linearGradient>` only

### Vertical Gradients for UI

Vertical gradients are commonly used for:
- Bar charts (bottom-to-top data visualization)
- Progress indicators
- Temperature/level gauges
- Background fills for cards and panels
- Sky/ground effects
- Depth perception in UI elements

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
        <linearGradient id="bottomUp" x1="0%" y1="100%" x2="0%" y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#bottomUp)" />
</svg>
```

### Gradient Starting from Center

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="centerY" x1="0%" y1="50%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#centerY)" />
</svg>
```

### Diagonal Gradient (Top-Left to Bottom-Right)

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="diagonal1" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#diagonal1)" />
</svg>
```

### Diagonal Gradient (Bottom-Left to Top-Right)

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="diagonal2" x1="0%" y1="100%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#f39c12" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#diagonal2)" />
</svg>
```

### Vertical Bar Chart with Gradient

```html
<svg width="500" height="400">
    <defs>
        <linearGradient id="barVertical" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#1a5490" />
        </linearGradient>
    </defs>

    <!-- Chart bars with vertical gradient -->
    <rect x="50" y="150" width="60" height="220" fill="url(#barVertical)" />
    <rect x="130" y="100" width="60" height="270" fill="url(#barVertical)" />
    <rect x="210" y="180" width="60" height="190" fill="url(#barVertical)" />
    <rect x="290" y="130" width="60" height="240" fill="url(#barVertical)" />
    <rect x="370" y="160" width="60" height="210" fill="url(#barVertical)" />
</svg>
```

### Progress Bar with Vertical Fill

```html
<svg width="100" height="400">
    <defs>
        <linearGradient id="progressVert" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>

    <!-- Background -->
    <rect x="20" y="0" width="60" height="400" rx="30" fill="#ecf0f1" />
    <!-- Progress (70%) -->
    <rect x="20" y="120" width="60" height="280" rx="30" fill="url(#progressVert)" />
</svg>
```

### Temperature Gauge

```html
<svg width="150" height="400">
    <defs>
        <linearGradient id="tempScale" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="25%" stop-color="#f39c12" />
            <stop offset="50%" stop-color="#f1c40f" />
            <stop offset="75%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2980b9" />
        </linearGradient>
    </defs>

    <rect x="50" y="20" width="50" height="360" rx="25" fill="url(#tempScale)" />
    <text x="75" y="15" text-anchor="middle" font-size="12">Hot</text>
    <text x="75" y="395" text-anchor="middle" font-size="12">Cold</text>
</svg>
```

### Sky Background Gradient

```html
<svg width="600" height="400">
    <defs>
        <linearGradient id="sky" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#89cff0" />
            <stop offset="100%" stop-color="#e8f4f8" />
        </linearGradient>
    </defs>

    <rect width="600" height="400" fill="url(#sky)" />
</svg>
```

### Card with Vertical Gradient Header

```html
<svg width="400" height="500">
    <defs>
        <linearGradient id="cardGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#667eea" />
            <stop offset="100%" stop-color="#764ba2" />
        </linearGradient>
    </defs>

    <!-- Card body -->
    <rect width="400" height="500" rx="8" fill="#ffffff" stroke="#ddd" />
    <!-- Header with gradient -->
    <rect width="400" height="150" rx="8" fill="url(#cardGrad)" />

    <text x="20" y="50" fill="white" font-size="32" font-weight="bold">
        Profile
    </text>
    <text x="20" y="85" fill="white" font-size="16" opacity="0.9">
        User Dashboard
    </text>
</svg>
```

### Data Visualization: Stacked Area Chart

```html
<svg width="600" height="300">
    <defs>
        <linearGradient id="area1" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" stop-opacity="0.8" />
            <stop offset="100%" stop-color="#3498db" stop-opacity="0.1" />
        </linearGradient>
    </defs>

    <path d="M 0,200 L 100,180 L 200,150 L 300,170 L 400,140 L 500,160 L 600,150 L 600,300 L 0,300 Z"
          fill="url(#area1)" />
</svg>
```

### Vertical Heatmap Column

```html
<svg width="100" height="400">
    <defs>
        <linearGradient id="heatColumn" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#c0392b" />
            <stop offset="20%" stop-color="#e74c3c" />
            <stop offset="40%" stop-color="#e67e22" />
            <stop offset="60%" stop-color="#f1c40f" />
            <stop offset="80%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>

    <rect x="20" y="20" width="60" height="360" fill="url(#heatColumn)" />
    <text x="50" y="10" text-anchor="middle" font-size="10">High</text>
    <text x="50" y="395" text-anchor="middle" font-size="10">Low</text>
</svg>
```

### Gauge Indicator with Vertical Gradient

```html
<svg width="200" height="300">
    <defs>
        <linearGradient id="gaugeVert" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="50%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <rect x="70" y="50" width="60" height="200" rx="30" fill="url(#gaugeVert)" />
    <rect x="60" y="{{50 + (200 * 0.35)}}" width="80" height="8" fill="white" />
</svg>
```

### Battery Level Indicator

```html
<svg width="150" height="300">
    <defs>
        <linearGradient id="battery" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>

    <!-- Battery outline -->
    <rect x="35" y="30" width="80" height="240" rx="10" fill="none" stroke="#2c3e50" stroke-width="3" />
    <!-- Battery terminal -->
    <rect x="55" y="15" width="40" height="15" rx="5" fill="#2c3e50" />
    <!-- Battery fill (60%) -->
    <rect x="40" y="126" width="70" height="138" rx="8" fill="url(#battery)" />
</svg>
```

### Vertical Slider Track

```html
<svg width="100" height="400">
    <defs>
        <linearGradient id="slider" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#1a5490" />
        </linearGradient>
    </defs>

    <rect x="40" y="20" width="20" height="360" rx="10" fill="url(#slider)" />
    <circle cx="50" cy="200" r="15" fill="white" stroke="#3498db" stroke-width="3" />
</svg>
```

### Data-Driven Vertical Range

```html
<!-- Model: { minValue: 0, maxValue: 100, currentValue: 65 } -->
<svg width="150" height="400">
    <defs>
        <linearGradient id="dataRange"
                        x1="0%"
                        y1="{{(model.currentValue / model.maxValue) * 100}}%"
                        x2="0%"
                        y2="100%">
            <stop offset="0%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <rect x="50" y="20" width="50" height="360" rx="25" fill="url(#dataRange)" />
    <text x="75" y="{{20 + (360 * (1 - model.currentValue / model.maxValue))}}"
          text-anchor="middle" font-size="14" fill="white" font-weight="bold">
        {{model.currentValue}}%
    </text>
</svg>
```

---

## See Also

- [x1](/reference/svgattributes/x1.html) - Linear gradient start X coordinate
- [x2](/reference/svgattributes/x2.html) - Linear gradient end X coordinate
- [y2](/reference/svgattributes/y2.html) - Linear gradient end Y coordinate
- [linearGradient](/reference/svgtags/linearGradient.html) - Linear gradient definition element
- [gradientUnits](/reference/svgattributes/gradientUnits.html) - Coordinate system mode
- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [Data Binding](/reference/binding/) - Data binding and expressions
- [CSS Gradients](/reference/styles/gradients.html) - CSS gradient properties

---
