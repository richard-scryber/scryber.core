---
layout: default
title: x2
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @x2 : The Linear Gradient End X Coordinate Attribute

The `x2` attribute defines the X-coordinate of the ending point for a linear gradient vector. Together with `y2`, it establishes the destination point where the gradient ends, working in conjunction with the start point (x1, y1) to determine the gradient's complete direction and angle.

## Usage

The `x2` attribute is used to:
- Define the horizontal ending position of a linear gradient
- Control gradient angle and direction when combined with x1/y1/y2
- Support both relative (percentage) and absolute (unit-based) coordinates
- Enable precise gradient positioning for data visualizations
- Work with both objectBoundingBox (default) and userSpaceOnUse coordinate systems
- Create dynamic gradient effects through data binding
- Determine gradient spread length and extent

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
| Percentage | `0%` to `100%` | Relative to shape bounding box (objectBoundingBox mode) | `x2="100%"` |
| Decimal | `0` to `1.0` | Relative coordinate (0=left, 1=right) | `x2="1.0"` |
| Length Units | `pt`, `px`, `mm`, `cm`, `in` | Absolute coordinates (userSpaceOnUse mode) | `x2="400pt"` |
| Default | `100%` | Used when attribute is omitted | - |

### Common Patterns

```html
<!-- Full width (default) -->
<linearGradient id="g1" x2="100%">

<!-- Half width -->
<linearGradient id="g2" x2="50%">

<!-- No horizontal change (vertical gradient) -->
<linearGradient id="g3" x2="0%">

<!-- Absolute positioning -->
<linearGradient id="g4" x2="400pt" gradientUnits="userSpaceOnUse">
```

---

## Supported Elements

The `x2` attribute is supported on:

- **[&lt;linearGradient&gt;](/reference/svgtags/linearGradient.html)** - Defines linear gradient end X position

Note: This attribute is NOT used with `<radialGradient>` elements.

---

## Data Binding

### Dynamic Gradient End Position

Bind the x2 coordinate to data for dynamic gradient extent:

```html
<!-- Model: { endX: 75 } -->
<svg width="400" height="100">
    <defs>
        <linearGradient id="dynamicEnd"
                        x1="0%" y1="0%"
                        x2="{{model.endX}}%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#dynamicEnd)" />
</svg>
```

### Progress-Based Gradient Width

Create gradients that reflect progress or completion:

```html
<!-- Model: { progress: 65 } -->
<svg width="400" height="100">
    <defs>
        <linearGradient id="progressGrad"
                        x1="0%" y1="0%"
                        x2="{{model.progress}}%" y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>

    <rect width="400" height="100" rx="50" fill="#ecf0f1" />
    <rect width="{{model.progress * 4}}" height="100" rx="50" fill="url(#progressGrad)" />
</svg>
```

### Data-Driven Chart Gradients

Position gradient endpoints based on data ranges:

```html
<!-- Model: { dataStart: 20, dataEnd: 80, chartWidth: 500 } -->
<svg width="{{model.chartWidth}}" height="300">
    <defs>
        <linearGradient id="dataGrad"
                        x1="{{model.dataStart}}%"
                        y1="0%"
                        x2="{{model.dataEnd}}%"
                        y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="{{model.chartWidth}}" height="300" fill="url(#dataGrad)" />
</svg>
```

### Calculated Gradient Vectors

Calculate gradient endpoint from angle and length:

```html
<!-- Model: { angle: 30, length: 80 } -->
<svg width="400" height="300">
    <defs>
        <linearGradient id="calcGrad"
                        x1="50%" y1="50%"
                        x2="{{50 + Math.cos(model.angle * Math.PI / 180) * model.length / 2}}%"
                        y2="{{50 + Math.sin(model.angle * Math.PI / 180) * model.length / 2}}%">
            <stop offset="0%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#calcGrad)" />
</svg>
```

### Heatmap with Data-Driven Range

Create color gradients based on data value ranges:

```html
<!-- Model: { valueRange: { start: 0, end: 100 }, highlightEnd: 70 } -->
<svg width="500" height="100">
    <defs>
        <linearGradient id="heatRange"
                        x1="0%"
                        y1="0%"
                        x2="{{(model.highlightEnd / model.valueRange.end) * 100}}%"
                        y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="50%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="500" height="100" fill="url(#heatRange)" />
</svg>
```

---

## Notes

### Gradient Vector Calculation

The gradient vector is defined by the line from (x1, y1) to (x2, y2):
- The gradient transitions perpendicular to this vector
- Colors are distributed along the vector direction
- Vector length affects gradient spread with `spreadMethod`

### Coordinate Systems

The `x2` attribute behaves differently based on `gradientUnits`:

1. **objectBoundingBox (default)**: Relative to filled shape
   - Values 0-1 or 0%-100%
   - `0%` = left edge, `100%` = right edge
   - Gradient automatically scales with shape size
   - Most common for responsive designs

2. **userSpaceOnUse**: Absolute document coordinates
   - Values in pt, px, mm, cm, in
   - Fixed position in document space
   - Consistent across multiple shapes

### Common Gradient Patterns

```html
<!-- Horizontal left-to-right (DEFAULT) -->
<linearGradient x1="0%" y1="0%" x2="100%" y2="0%">

<!-- Horizontal right-to-left -->
<linearGradient x1="100%" y1="0%" x2="0%" y2="0%">

<!-- Vertical (no X change) -->
<linearGradient x1="0%" y1="0%" x2="0%" y2="100%">

<!-- Diagonal -->
<linearGradient x1="0%" y1="0%" x2="100%" y2="100%">

<!-- Short gradient with repeat -->
<linearGradient x1="0%" y1="0%" x2="20%" y2="0%" spreadMethod="repeat">
```

### Default Behavior

- Default value: `100%` (right edge)
- Default gradient: horizontal left-to-right
- Must be used with `<linearGradient>` only

### Relationship with spreadMethod

The `x2` (and y2) coordinates interact with `spreadMethod`:
- **pad**: Gradient stops at x2, extending final color
- **repeat**: Gradient tiles from x1-x2 repeatedly
- **reflect**: Gradient mirrors from x1-x2 repeatedly

---

## Examples

### Basic Horizontal Gradient (Default)

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="fullWidth" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#fullWidth)" />
</svg>
```

### Half-Width Gradient

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="halfWidth" x1="0%" y1="0%" x2="50%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#halfWidth)" />
</svg>
```

### Reverse Gradient Direction

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="reversed" x1="100%" y1="0%" x2="0%" y2="0%">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#reversed)" />
</svg>
```

### Diagonal Gradient

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="diagonal" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#diagonal)" />
</svg>
```

### Centered Gradient Radiating Outward

```html
<svg width="400" height="300">
    <defs>
        <linearGradient id="centered" x1="50%" y1="50%" x2="100%" y2="50%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="300" fill="url(#centered)" />
</svg>
```

### Short Repeating Gradient

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="repeating" x1="0%" y1="0%" x2="10%" y2="0%" spreadMethod="repeat">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#2980b9" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#repeating)" />
</svg>
```

### Progress Bar with Dynamic End

```html
<!-- Model: { completion: 70 } -->
<svg width="400" height="60">
    <defs>
        <linearGradient id="progress"
                        x1="0%" y1="0%"
                        x2="{{model.completion}}%" y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>

    <rect x="0" y="0" width="400" height="60" rx="30" fill="#ecf0f1" />
    <rect x="0" y="0" width="{{model.completion * 4}}" height="60" rx="30" fill="url(#progress)" />
    <text x="200" y="38" text-anchor="middle" font-size="18" fill="#333">
        {{model.completion}}%
    </text>
</svg>
```

### Bar Chart with Horizontal Gradient Bars

```html
<svg width="500" height="400">
    <defs>
        <linearGradient id="barGrad" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2980b9" />
        </linearGradient>
    </defs>

    <!-- Horizontal bars -->
    <rect x="100" y="50" width="280" height="40" fill="url(#barGrad)" />
    <text x="90" y="75" text-anchor="end" font-size="14">Q1</text>

    <rect x="100" y="110" width="350" height="40" fill="url(#barGrad)" />
    <text x="90" y="135" text-anchor="end" font-size="14">Q2</text>

    <rect x="100" y="170" width="240" height="40" fill="url(#barGrad)" />
    <text x="90" y="195" text-anchor="end" font-size="14">Q3</text>

    <rect x="100" y="230" width="310" height="40" fill="url(#barGrad)" />
    <text x="90" y="255" text-anchor="end" font-size="14">Q4</text>
</svg>
```

### Data Visualization Area with Gradient

```html
<svg width="600" height="300">
    <defs>
        <linearGradient id="areaGrad" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" stop-opacity="0.8" />
            <stop offset="100%" stop-color="#9b59b6" stop-opacity="0.8" />
        </linearGradient>
    </defs>

    <path d="M 0,250 L 100,200 L 200,180 L 300,220 L 400,170 L 500,190 L 600,160 L 600,300 L 0,300 Z"
          fill="url(#areaGrad)" />
    <path d="M 0,250 L 100,200 L 200,180 L 300,220 L 400,170 L 500,190 L 600,160"
          fill="none" stroke="#3498db" stroke-width="3" />
</svg>
```

### Status Badge with Gradient

```html
<svg width="180" height="60">
    <defs>
        <linearGradient id="statusBadge" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#27ae60" />
            <stop offset="100%" stop-color="#2ecc71" />
        </linearGradient>
    </defs>

    <rect width="180" height="60" rx="30" fill="url(#statusBadge)" />
    <text x="90" y="38" text-anchor="middle" fill="white" font-size="18" font-weight="bold">
        Completed
    </text>
</svg>
```

### Heatmap Scale

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="heatScale" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="25%" stop-color="#f1c40f" />
            <stop offset="50%" stop-color="#e67e22" />
            <stop offset="75%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#c0392b" />
        </linearGradient>
    </defs>

    <rect x="20" y="40" width="360" height="30" fill="url(#heatScale)" />
    <text x="20" y="30" font-size="12">0</text>
    <text x="370" y="30" font-size="12" text-anchor="end">100</text>
</svg>
```

### Button with Shine Effect

```html
<svg width="220" height="70">
    <defs>
        <linearGradient id="buttonShine" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#5dade2" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>

    <rect width="220" height="70" rx="10" fill="url(#buttonShine)" />
    <text x="110" y="43" text-anchor="middle" fill="white" font-size="20" font-weight="bold">
        Download
    </text>
</svg>
```

### Timeline with Gradient Segments

```html
<svg width="600" height="100">
    <defs>
        <linearGradient id="timeline" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="33%" stop-color="#9b59b6" />
            <stop offset="67%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#f39c12" />
        </linearGradient>
    </defs>

    <rect x="50" y="40" width="500" height="20" rx="10" fill="url(#timeline)" />
    <circle cx="50" cy="50" r="8" fill="#3498db" />
    <circle cx="217" cy="50" r="8" fill="#9b59b6" />
    <circle cx="384" cy="50" r="8" fill="#e74c3c" />
    <circle cx="550" cy="50" r="8" fill="#f39c12" />
</svg>
```

### Gauge Arc with Gradient

```html
<svg width="300" height="200" viewBox="0 0 300 200">
    <defs>
        <linearGradient id="gaugeArc" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="50%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <path d="M 50,150 A 100,100 0 0,1 250,150"
          fill="none" stroke="url(#gaugeArc)" stroke-width="30" stroke-linecap="round" />
    <text x="150" y="140" text-anchor="middle" font-size="48" font-weight="bold" fill="#333">
        75%
    </text>
</svg>
```

### Data-Driven Highlight Region

```html
<!-- Model: { highlightStart: 30, highlightEnd: 70 } -->
<svg width="500" height="100">
    <defs>
        <linearGradient id="highlight"
                        x1="{{model.highlightStart}}%"
                        y1="0%"
                        x2="{{model.highlightEnd}}%"
                        y2="0%">
            <stop offset="0%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <rect width="500" height="100" fill="#ecf0f1" />
    <rect x="{{model.highlightStart * 5}}"
          width="{{(model.highlightEnd - model.highlightStart) * 5}}"
          height="100"
          fill="url(#highlight)" />
</svg>
```

---

## See Also

- [x1](/reference/svgattributes/x1.html) - Linear gradient start X coordinate
- [y1](/reference/svgattributes/y1.html) - Linear gradient start Y coordinate
- [y2](/reference/svgattributes/y2.html) - Linear gradient end Y coordinate
- [linearGradient](/reference/svgtags/linearGradient.html) - Linear gradient definition element
- [gradientUnits](/reference/svgattributes/gradientUnits.html) - Coordinate system mode
- [spreadMethod](/reference/svgattributes/spreadMethod.html) - Gradient spread behavior
- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [Data Binding](/reference/binding/) - Data binding and expressions

---
