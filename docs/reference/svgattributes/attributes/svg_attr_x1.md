---
layout: default
title: x1
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @x1 : The Linear Gradient Start X Coordinate Attribute

The `x1` attribute defines the X-coordinate of the starting point for a linear gradient vector. Together with `y1`, it establishes the origin point from which the gradient begins, determining both the angle and direction of the color transition.

## Usage

The `x1` attribute is used to:
- Define the horizontal starting position of a linear gradient
- Control gradient angle when combined with x2/y2
- Support both relative (percentage) and absolute (unit-based) coordinates
- Enable dynamic gradient positioning through data binding
- Work with both objectBoundingBox (default) and userSpaceOnUse coordinate systems
- Create horizontal, vertical, diagonal, or custom-angle gradients
- Position gradients precisely for data visualizations and charts

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
        <linearGradient id="myGradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect x="10" y="10" width="380" height="180" fill="url(#myGradient)" />
</svg>
```

---

## Supported Values

| Value Type | Format | Description | Example |
|------------|--------|-------------|---------|
| Percentage | `0%` to `100%` | Relative to shape bounding box (objectBoundingBox mode) | `x1="0%"` |
| Decimal | `0` to `1.0` | Relative coordinate (0=left, 1=right) | `x1="0.5"` |
| Length Units | `pt`, `px`, `mm`, `cm`, `in` | Absolute coordinates (userSpaceOnUse mode) | `x1="50pt"` |
| Default | `0%` | Used when attribute is omitted | - |

### Common Patterns

```html
<!-- Left edge (default) -->
<linearGradient id="g1" x1="0%">

<!-- Center -->
<linearGradient id="g2" x1="50%">

<!-- Right edge -->
<linearGradient id="g3" x1="100%">

<!-- Absolute positioning -->
<linearGradient id="g4" x1="100pt" gradientUnits="userSpaceOnUse">
```

---

## Supported Elements

The `x1` attribute is supported on:

- **[&lt;linearGradient&gt;](/reference/svgtags/linearGradient.html)** - Defines linear gradient start X position

Note: This attribute is NOT used with `<radialGradient>` elements, which use `cx`/`cy` and `fx`/`fy` instead.

---

## Data Binding

### Dynamic Gradient Start Position

Bind the x1 coordinate to data values for dynamic gradient positioning:

```html
<!-- Model: { startX: 25 } -->
<svg width="400" height="100">
    <defs>
        <linearGradient id="dynamicStart"
                        x1="{{model.startX}}%" y1="0%"
                        x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#dynamicStart)" />
</svg>
```

### Calculated Gradient Angles

Calculate x1/x2 coordinates to create specific gradient angles:

```html
<!-- Model: { angle: 45 } -->
<!-- Calculates gradient vector from angle in degrees -->
<svg width="400" height="200">
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
    <rect width="400" height="200" fill="url(#angleGrad)" />
</svg>
```

### Data-Driven Chart Gradients

Position gradients based on data ranges:

```html
<!-- Model: { dataMin: 0, dataMax: 100, highlightStart: 30 } -->
<svg width="500" height="300">
    <defs>
        <linearGradient id="chartGrad"
                        x1="{{(model.highlightStart / model.dataMax) * 100}}%"
                        y1="0%"
                        x2="100%"
                        y2="0%">
            <stop offset="0%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect x="0" y="0" width="500" height="300" fill="url(#chartGrad)" />
</svg>
```

### Conditional Gradient Direction

Change gradient direction based on data conditions:

```html
<!-- Model: { direction: "rtl", value: 75 } -->
<svg width="400" height="100">
    <defs>
        <linearGradient id="dirGrad"
                        x1="{{model.direction === 'rtl' ? '100%' : '0%'}}"
                        y1="0%"
                        x2="{{model.direction === 'rtl' ? '0%' : '100%'}}"
                        y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>
    <rect width="{{model.value * 4}}" height="100" fill="url(#dirGrad)" />
</svg>
```

### Responsive Gradient Positioning

Adjust gradient based on viewport or container size:

```html
<!-- Model: { containerWidth: 800, offset: 200 } -->
<svg width="{{model.containerWidth}}" height="200">
    <defs>
        <linearGradient id="responsive"
                        x1="{{(model.offset / model.containerWidth) * 100}}%"
                        y1="0%"
                        x2="100%"
                        y2="0%"
                        gradientUnits="objectBoundingBox">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#9b59b6" />
        </linearGradient>
    </defs>
    <rect width="{{model.containerWidth}}" height="200" fill="url(#responsive)" />
</svg>
```

---

## Notes

### Coordinate Systems

The `x1` attribute behaves differently based on `gradientUnits`:

1. **objectBoundingBox (default)**: Relative to filled shape
   - Values 0-1 or 0%-100%
   - `0%` = left edge, `50%` = center, `100%` = right edge
   - Gradient automatically scales with shape size
   - Most common for responsive designs

2. **userSpaceOnUse**: Absolute document coordinates
   - Values in pt, px, mm, cm, in
   - Fixed position in document space
   - Useful for consistent gradients across multiple shapes

### Gradient Vector Direction

The gradient vector is defined by start point (x1, y1) and end point (x2, y2):

```html
<!-- Horizontal (left to right) - DEFAULT -->
<linearGradient x1="0%" y1="0%" x2="100%" y2="0%">

<!-- Horizontal (right to left) -->
<linearGradient x1="100%" y1="0%" x2="0%" y2="0%">

<!-- Diagonal (top-left to bottom-right) -->
<linearGradient x1="0%" y1="0%" x2="100%" y2="100%">

<!-- Custom angle: 45 degrees from center -->
<linearGradient x1="25%" y1="75%" x2="75%" y2="25%">
```

### Default Behavior

- Default value: `0%` (left edge)
- Default gradient: horizontal left-to-right (x1="0%", y1="0%", x2="100%", y2="0%")
- Must be used with `<linearGradient>` only

### Performance Considerations

- Percentage values are more efficient than calculations
- Static gradients perform better than data-bound gradients
- Consider caching calculated gradient definitions
- Use CSS variables for frequently changed values

---

## Examples

### Basic Horizontal Gradient (Default)

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="horizontal" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#horizontal)" />
</svg>
```

### Gradient Starting from Center

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="centerStart" x1="50%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#centerStart)" />
</svg>
```

### Reverse Horizontal Gradient

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="reverse" x1="100%" y1="0%" x2="0%" y2="0%">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#reverse)" />
</svg>
```

### Diagonal Gradient with Custom Start

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="diagonal" x1="20%" y1="20%" x2="80%" y2="80%">
            <stop offset="0%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#diagonal)" />
</svg>
```

### Bar Chart with Gradient Fill

```html
<svg width="500" height="300">
    <defs>
        <linearGradient id="barGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </linearGradient>
    </defs>

    <!-- Chart bars -->
    <rect x="50" y="100" width="60" height="180" fill="url(#barGrad)" />
    <rect x="130" y="50" width="60" height="230" fill="url(#barGrad)" />
    <rect x="210" y="120" width="60" height="160" fill="url(#barGrad)" />
    <rect x="290" y="80" width="60" height="200" fill="url(#barGrad)" />
    <rect x="370" y="140" width="60" height="140" fill="url(#barGrad)" />
</svg>
```

### Progress Bar with Left-to-Right Gradient

```html
<svg width="400" height="50">
    <defs>
        <linearGradient id="progress" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="50%" stop-color="#27ae60" />
            <stop offset="100%" stop-color="#229954" />
        </linearGradient>
    </defs>

    <rect x="0" y="0" width="400" height="50" rx="25" fill="#ecf0f1" />
    <rect x="0" y="0" width="280" height="50" rx="25" fill="url(#progress)" />
</svg>
```

### Status Badge with Gradient

```html
<svg width="150" height="50">
    <defs>
        <linearGradient id="badge" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#27ae60" />
            <stop offset="100%" stop-color="#2ecc71" />
        </linearGradient>
    </defs>

    <rect width="150" height="50" rx="25" fill="url(#badge)" />
    <text x="75" y="32" text-anchor="middle" fill="white" font-size="16" font-weight="bold">
        Active
    </text>
</svg>
```

### Gauge Chart with Angular Gradient

```html
<svg width="300" height="200" viewBox="0 0 300 200">
    <defs>
        <linearGradient id="gauge" x1="0%" y1="100%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="50%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <path d="M 50,150 A 100,100 0 0,1 250,150"
          fill="none" stroke="url(#gauge)" stroke-width="30" stroke-linecap="round" />
</svg>
```

### Heatmap Legend with Horizontal Gradient

```html
<svg width="400" height="80">
    <defs>
        <linearGradient id="heatmap" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="25%" stop-color="#f1c40f" />
            <stop offset="50%" stop-color="#e67e22" />
            <stop offset="75%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#c0392b" />
        </linearGradient>
    </defs>

    <rect x="10" y="30" width="380" height="20" fill="url(#heatmap)" />
    <text x="10" y="20" font-size="12">Low</text>
    <text x="360" y="20" font-size="12">High</text>
</svg>
```

### Card Header with Gradient Background

```html
<svg width="400" height="120">
    <defs>
        <linearGradient id="cardHeader" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#667eea" />
            <stop offset="100%" stop-color="#764ba2" />
        </linearGradient>
    </defs>

    <rect width="400" height="120" rx="8" fill="url(#cardHeader)" />
    <text x="20" y="50" fill="white" font-size="28" font-weight="bold">
        Sales Dashboard
    </text>
    <text x="20" y="85" fill="white" font-size="14" opacity="0.9">
        Q4 2024 Performance
    </text>
</svg>
```

### Data Visualization Area Fill

```html
<svg width="500" height="300">
    <defs>
        <linearGradient id="areaFill" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" stop-opacity="0.8" />
            <stop offset="100%" stop-color="#3498db" stop-opacity="0.1" />
        </linearGradient>
    </defs>

    <path d="M 0,250 L 50,200 L 100,150 L 150,120 L 200,100 L 250,130 L 300,110 L 350,140 L 400,120 L 450,90 L 500,110 L 500,300 L 0,300 Z"
          fill="url(#areaFill)" />
</svg>
```

### Button with Horizontal Gradient

```html
<svg width="200" height="60">
    <defs>
        <linearGradient id="buttonGrad" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#2980b9" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>

    <rect width="200" height="60" rx="8" fill="url(#buttonGrad)" />
    <text x="100" y="38" text-anchor="middle" fill="white" font-size="18" font-weight="bold">
        Submit
    </text>
</svg>
```

### Data-Driven Gradient Start

```html
<!-- Model: { highlightPosition: 30, color1: "#3498db", color2: "#e74c3c" } -->
<svg width="400" height="100">
    <defs>
        <linearGradient id="dataStart"
                        x1="{{model.highlightPosition}}%"
                        y1="0%"
                        x2="100%"
                        y2="0%">
            <stop offset="0%" stop-color="{{model.color1}}" />
            <stop offset="100%" stop-color="{{model.color2}}" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#dataStart)" />
</svg>
```

### Striped Pattern with Offset Start

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="stripes" x1="0%" y1="0%" x2="10%" y2="0%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#2980b9" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#stripes)" />
</svg>
```

### Multi-Bar Chart with Shared Gradient

```html
<svg width="600" height="400">
    <defs>
        <linearGradient id="barGradient" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#1a5490" />
        </linearGradient>
    </defs>

    <!-- Sales by region -->
    <rect x="50" y="150" width="80" height="220" fill="url(#barGradient)" />
    <text x="90" y="390" text-anchor="middle" font-size="12">North</text>

    <rect x="170" y="100" width="80" height="270" fill="url(#barGradient)" />
    <text x="210" y="390" text-anchor="middle" font-size="12">South</text>

    <rect x="290" y="180" width="80" height="190" fill="url(#barGradient)" />
    <text x="330" y="390" text-anchor="middle" font-size="12">East</text>

    <rect x="410" y="130" width="80" height="240" fill="url(#barGradient)" />
    <text x="450" y="390" text-anchor="middle" font-size="12">West</text>
</svg>
```

---

## See Also

- [x2](/reference/svgattributes/x2.html) - Linear gradient end X coordinate
- [y1](/reference/svgattributes/y1.html) - Linear gradient start Y coordinate
- [y2](/reference/svgattributes/y2.html) - Linear gradient end Y coordinate
- [linearGradient](/reference/svgtags/linearGradient.html) - Linear gradient definition element
- [gradientUnits](/reference/svgattributes/gradientUnits.html) - Coordinate system mode
- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [Data Binding](/reference/binding/) - Data binding and expressions
- [CSS Gradients](/reference/styles/gradients.html) - CSS gradient properties

---
