---
layout: default
title: linearGradient
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;linearGradient&gt; : The Linear Gradient Definition Element

The `<linearGradient>` element defines a linear gradient fill that transitions between colors along a straight line. It must be placed inside an SVG `<defs>` section or directly within an `<svg>` canvas, and is referenced by its `id` attribute from SVG shapes and elements using the `fill` or `stroke` attributes.

## Usage

The `<linearGradient>` element creates reusable gradient definitions that:
- Define smooth color transitions along a linear axis
- Support multiple color stops with precise positioning
- Can be oriented at any angle (horizontal, vertical, diagonal)
- Support three spread methods: pad, repeat, and reflect
- Work with objectBoundingBox (relative %) or userSpaceOnUse (absolute units)
- Can be styled via CSS classes and inline styles
- Support data binding for dynamic gradient generation
- Are referenced using `url(#gradientId)` syntax

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

## Supported Attributes

### Identification Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | **Required**. Unique identifier for the gradient. Used to reference the gradient from other elements. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the gradient definition. |

### Gradient Position Attributes

| Attribute | Type | Default | Description |
|-----------|------|---------|-------------|
| `x1` | Unit | 0% | X-coordinate of the gradient's start point. Supports units: pt, px, mm, cm, in, %. |
| `y1` | Unit | 0% | Y-coordinate of the gradient's start point. Supports units: pt, px, mm, cm, in, %. |
| `x2` | Unit | 100% | X-coordinate of the gradient's end point. Supports units: pt, px, mm, cm, in, %. |
| `y2` | Unit | 0% | Y-coordinate of the gradient's end point. Supports units: pt, px, mm, cm, in, %. |

### Gradient Behavior Attributes

| Attribute | Type | Default | Description |
|-----------|------|---------|-------------|
| `gradientUnits` | GradientUnitType | objectBoundingBox | Coordinate system: `objectBoundingBox` (relative to shape, 0-1 or %) or `userSpaceOnUse` (absolute coordinates). |
| `spreadMethod` | GradientSpreadMode | pad | How gradient extends beyond defined range: `pad` (extend end colors), `repeat` (tile gradient), `reflect` (mirror gradient). |

### Reference Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `href` | string | Reference to another gradient to inherit attributes from. Format: `url(#othergradientId)`. |

### CSS Style Support

The `<linearGradient>` element can be styled through CSS:

**Gradient Properties** (via inline style or CSS):
- `x1`, `x2`, `y1`, `y2` - Gradient vector positioning
- `spreadMethod` - Gradient spread behavior
- `gradientUnits` - Coordinate system type

---

## Notes

### Gradient Coordinate System

Linear gradients use a vector from point (x1, y1) to point (x2, y2):

1. **objectBoundingBox (default)**: Coordinates are relative to the filled shape
   - Values typically 0-1 or percentages (0%-100%)
   - (0%, 0%) = top-left, (100%, 100%) = bottom-right
   - Gradient scales automatically with shape size

2. **userSpaceOnUse**: Coordinates are in absolute document units
   - Values in pt, px, mm, cm, in
   - Gradient position is fixed in document space
   - Useful for consistent gradients across multiple shapes

### Gradient Angles and Directions

Common gradient configurations:

```html
<!-- Horizontal (left to right) - DEFAULT -->
<linearGradient id="horizontal" x1="0%" y1="0%" x2="100%" y2="0%">

<!-- Vertical (top to bottom) -->
<linearGradient id="vertical" x1="0%" y1="0%" x2="0%" y2="100%">

<!-- Diagonal (top-left to bottom-right) -->
<linearGradient id="diagonal" x1="0%" y1="0%" x2="100%" y2="100%">

<!-- 45 degrees -->
<linearGradient id="angle45" x1="0%" y1="50%" x2="100%" y2="50%">
</linearGradient>
```

### Spread Methods

The `spreadMethod` attribute controls gradient behavior beyond the 0%-100% range:

1. **pad (default)**: Extends the first and last stop colors
2. **repeat**: Tiles the gradient pattern
3. **reflect**: Mirrors the gradient alternately

### Gradient Stops

Gradients require child `<stop>` elements to define colors:
- Minimum 2 stops required for a gradient
- Stops define colors at specific positions (0%-100%)
- More stops create complex multi-color transitions
- Stops automatically sorted by offset value

### Class Hierarchy

In the Scryber codebase:
- `SVGLinearGradient` extends `SVGFillBase` extends `Component`
- Implements `IStyledComponent` for CSS styling
- Implements `ICloneable` for gradient duplication
- Uses specialized gradient calculators based on spread mode

### Gradient Reference and Inheritance

Use `href` to reference another gradient's properties:

```html
<linearGradient id="base" x1="0%" y1="0%" x2="100%" y2="0%">
    <stop offset="0%" stop-color="blue" />
    <stop offset="100%" stop-color="red" />
</linearGradient>

<!-- Inherits x1, x2, y1, y2, stops from base -->
<linearGradient id="derived" href="url(#base)">
    <!-- Can override specific stops -->
    <stop offset="50%" stop-color="yellow" />
</linearGradient>
```

---

## Data Binding

### Dynamic Gradient Colors

Bind gradient stop colors to data values:

```html
<!-- Model: { primaryColor: "#3498db", secondaryColor: "#e74c3c" } -->
<svg width="300" height="100">
    <defs>
        <linearGradient id="dataGradient" x1="0%" x2="100%">
            <stop offset="0%" stop-color="{{model.primaryColor}}" />
            <stop offset="100%" stop-color="{{model.secondaryColor}}" />
        </linearGradient>
    </defs>
    <rect width="280" height="80" fill="url(#dataGradient)" />
</svg>
```

### Dynamic Stop Positions

Create gradients with data-driven stop positions:

```html
<!-- Model: { midpoint: 30 } -->
<svg width="300" height="100">
    <linearGradient id="dynamicStops">
        <stop offset="0%" stop-color="#2ecc71" />
        <stop offset="{{model.midpoint}}%" stop-color="#f39c12" />
        <stop offset="100%" stop-color="#e74c3c" />
    </linearGradient>
    <rect width="280" height="80" fill="url(#dynamicStops)" />
</svg>
```

### Conditional Gradients

Generate gradients based on data conditions:

```html
<!-- Model: { status: "warning", warningColor: "#f39c12", successColor: "#2ecc71" } -->
<svg width="300" height="100">
    <linearGradient id="statusGradient">
        <stop offset="0%" stop-color="white" />
        <stop offset="100%"
              stop-color="{{model.status === 'warning' ? model.warningColor : model.successColor}}" />
    </linearGradient>
    <rect width="280" height="80" fill="url(#statusGradient)" />
</svg>
```

### Data-Driven Gradient Angles

Create gradients with dynamic angles:

```html
<!-- Model: { angle: 45, startColor: "#3498db", endColor: "#9b59b6" } -->
<svg width="300" height="100">
    <linearGradient id="angleGradient"
                    x1="{{model.angle === 0 ? '0%' : '0%'}}"
                    y1="{{model.angle === 0 ? '0%' : '0%'}}"
                    x2="{{model.angle === 0 ? '100%' : '100%'}}"
                    y2="{{model.angle === 0 ? '0%' : '100%'}}">
        <stop offset="0%" stop-color="{{model.startColor}}" />
        <stop offset="100%" stop-color="{{model.endColor}}" />
    </linearGradient>
    <rect width="280" height="80" fill="url(#angleGradient)" />
</svg>
```

### Repeating Gradients from Data

Generate multiple gradient stops from arrays:

```html
<!-- Model: { stops: [{offset: 0, color: "#3498db"}, {offset: 50, color: "#f39c12"}, {offset: 100, color: "#e74c3c"}] } -->
<svg width="300" height="100">
    <linearGradient id="multiStop">
        <template data-bind="{{model.stops}}">
            <stop offset="{{.offset}}%" stop-color="{{.color}}" />
        </template>
    </linearGradient>
    <rect width="280" height="80" fill="url(#multiStop)" />
</svg>
```

---

## Examples

### Basic Two-Color Gradient

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="basic" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect x="10" y="10" width="380" height="80" fill="url(#basic)" />
</svg>
```

### Vertical Gradient

```html
<svg width="400" height="200">
    <linearGradient id="vertical" x1="0%" y1="0%" x2="0%" y2="100%">
        <stop offset="0%" stop-color="#2ecc71" />
        <stop offset="100%" stop-color="#27ae60" />
    </linearGradient>
    <rect x="10" y="10" width="380" height="180" fill="url(#vertical)" />
</svg>
```

### Diagonal Gradient

```html
<svg width="400" height="200">
    <linearGradient id="diagonal" x1="0%" y1="0%" x2="100%" y2="100%">
        <stop offset="0%" stop-color="#9b59b6" />
        <stop offset="100%" stop-color="#8e44ad" />
    </linearGradient>
    <rect x="10" y="10" width="380" height="180" fill="url(#diagonal)" />
</svg>
```

### Multi-Color Gradient

```html
<svg width="400" height="100">
    <linearGradient id="rainbow">
        <stop offset="0%" stop-color="#e74c3c" />
        <stop offset="20%" stop-color="#f39c12" />
        <stop offset="40%" stop-color="#f1c40f" />
        <stop offset="60%" stop-color="#2ecc71" />
        <stop offset="80%" stop-color="#3498db" />
        <stop offset="100%" stop-color="#9b59b6" />
    </linearGradient>
    <rect x="10" y="10" width="380" height="80" fill="url(#rainbow)" />
</svg>
```

### Gradient with Opacity

```html
<svg width="400" height="100">
    <linearGradient id="fadeOut">
        <stop offset="0%" stop-color="#3498db" stop-opacity="1" />
        <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
    </linearGradient>
    <rect x="10" y="10" width="380" height="80" fill="url(#fadeOut)" />
</svg>
```

### Repeating Gradient

```html
<svg width="400" height="100">
    <linearGradient id="stripes" x1="0%" x2="20%" spreadMethod="repeat">
        <stop offset="0%" stop-color="#3498db" />
        <stop offset="50%" stop-color="#2980b9" />
        <stop offset="100%" stop-color="#3498db" />
    </linearGradient>
    <rect x="10" y="10" width="380" height="80" fill="url(#stripes)" />
</svg>
```

### Reflecting Gradient

```html
<svg width="400" height="100">
    <linearGradient id="mirror" x1="0%" x2="25%" spreadMethod="reflect">
        <stop offset="0%" stop-color="#e74c3c" />
        <stop offset="100%" stop-color="#c0392b" />
    </linearGradient>
    <rect x="10" y="10" width="380" height="80" fill="url(#mirror)" />
</svg>
```

### Gradient on Text

```html
<svg width="400" height="100">
    <linearGradient id="textGrad" x1="0%" x2="100%">
        <stop offset="0%" stop-color="#f39c12" />
        <stop offset="100%" stop-color="#e74c3c" />
    </linearGradient>
    <text x="20" y="60" font-size="48" font-weight="bold" fill="url(#textGrad)">
        Gradient Text
    </text>
</svg>
```

### Gradient on Path

```html
<svg width="400" height="200">
    <linearGradient id="pathGrad" x1="0%" y1="0%" x2="0%" y2="100%">
        <stop offset="0%" stop-color="#16a085" />
        <stop offset="100%" stop-color="#1abc9c" />
    </linearGradient>
    <path d="M 50,150 Q 100,50 200,150 T 350,150"
          fill="none" stroke="url(#pathGrad)" stroke-width="10" />
</svg>
```

### Gradient on Circle

```html
<svg width="300" height="300">
    <linearGradient id="circleGrad" x1="0%" y1="0%" x2="100%" y2="100%">
        <stop offset="0%" stop-color="#9b59b6" />
        <stop offset="100%" stop-color="#8e44ad" />
    </linearGradient>
    <circle cx="150" cy="150" r="120" fill="url(#circleGrad)" />
</svg>
```

### Chart Bar with Gradient

```html
<svg width="400" height="300">
    <linearGradient id="barGrad" x1="0%" y1="0%" x2="0%" y2="100%">
        <stop offset="0%" stop-color="#3498db" />
        <stop offset="100%" stop-color="#2c3e50" />
    </linearGradient>

    <rect x="50" y="100" width="60" height="180" fill="url(#barGrad)" />
    <rect x="130" y="50" width="60" height="230" fill="url(#barGrad)" />
    <rect x="210" y="120" width="60" height="160" fill="url(#barGrad)" />
    <rect x="290" y="80" width="60" height="200" fill="url(#barGrad)" />
</svg>
```

### Status Badge with Gradient

```html
<svg width="150" height="50">
    <linearGradient id="successBadge" x1="0%" x2="100%">
        <stop offset="0%" stop-color="#27ae60" />
        <stop offset="100%" stop-color="#2ecc71" />
    </linearGradient>
    <rect width="150" height="50" rx="25" fill="url(#successBadge)" />
    <text x="75" y="32" text-anchor="middle" fill="white" font-size="16" font-weight="bold">
        Success
    </text>
</svg>
```

### Progress Bar with Gradient

```html
<svg width="400" height="40">
    <linearGradient id="progressGrad">
        <stop offset="0%" stop-color="#2ecc71" />
        <stop offset="100%" stop-color="#27ae60" />
    </linearGradient>

    <!-- Background -->
    <rect x="0" y="0" width="400" height="40" rx="20" fill="#ecf0f1" />
    <!-- Progress -->
    <rect x="0" y="0" width="280" height="40" rx="20" fill="url(#progressGrad)" />
</svg>
```

### Button with Gradient Background

```html
<svg width="200" height="60">
    <linearGradient id="buttonGrad" x1="0%" y1="0%" x2="0%" y2="100%">
        <stop offset="0%" stop-color="#3498db" />
        <stop offset="50%" stop-color="#2980b9" />
        <stop offset="100%" stop-color="#3498db" />
    </linearGradient>
    <rect width="200" height="60" rx="8" fill="url(#buttonGrad)" />
    <text x="100" y="38" text-anchor="middle" fill="white" font-size="18" font-weight="bold">
        Click Me
    </text>
</svg>
```

### Data Visualization Heatmap Color

```html
<svg width="400" height="100">
    <linearGradient id="heatmap">
        <stop offset="0%" stop-color="#2ecc71" />
        <stop offset="33%" stop-color="#f1c40f" />
        <stop offset="66%" stop-color="#e67e22" />
        <stop offset="100%" stop-color="#e74c3c" />
    </linearGradient>

    <rect x="10" y="40" width="380" height="20" fill="url(#heatmap)" />
    <text x="10" y="30" font-size="12">Low</text>
    <text x="370" y="30" font-size="12">High</text>
</svg>
```

### Card Header with Gradient

```html
<svg width="400" height="100">
    <linearGradient id="headerGrad" x1="0%" x2="100%">
        <stop offset="0%" stop-color="#667eea" />
        <stop offset="100%" stop-color="#764ba2" />
    </linearGradient>

    <rect width="400" height="100" fill="url(#headerGrad)" />
    <text x="20" y="40" fill="white" font-size="24" font-weight="bold">
        Dashboard
    </text>
    <text x="20" y="70" fill="white" font-size="14" opacity="0.9">
        Welcome back, user
    </text>
</svg>
```

### Icon with Gradient Fill

```html
<svg width="100" height="100" viewBox="0 0 24 24">
    <linearGradient id="iconGrad" x1="0%" y1="0%" x2="100%" y2="100%">
        <stop offset="0%" stop-color="#f093fb" />
        <stop offset="100%" stop-color="#f5576c" />
    </linearGradient>

    <path d="M12 2L2 7v10c0 5.55 3.84 10.74 9 12 5.16-1.26 9-6.45 9-12V7l-10-5z"
          fill="url(#iconGrad)" />
</svg>
```

### Gauge Chart Segment

```html
<svg width="300" height="200">
    <linearGradient id="gaugeGrad" x1="0%" x2="100%">
        <stop offset="0%" stop-color="#2ecc71" />
        <stop offset="50%" stop-color="#f39c12" />
        <stop offset="100%" stop-color="#e74c3c" />
    </linearGradient>

    <path d="M 50,150 A 100,100 0 0,1 250,150"
          fill="none" stroke="url(#gaugeGrad)" stroke-width="30" stroke-linecap="round" />
</svg>
```

### Gradient Background for Chart Area

```html
<svg width="500" height="300">
    <linearGradient id="chartBg" x1="0%" y1="0%" x2="0%" y2="100%">
        <stop offset="0%" stop-color="#ecf0f1" />
        <stop offset="100%" stop-color="#ffffff" />
    </linearGradient>

    <rect width="500" height="300" fill="url(#chartBg)" />
    <!-- Chart content here -->
</svg>
```

### Data-Driven Status Indicator

```html
<!-- Model: { value: 75, thresholdLow: 50, thresholdHigh: 80 } -->
<svg width="300" height="100">
    <linearGradient id="statusGrad">
        <stop offset="0%" stop-color="{{model.value < model.thresholdLow ? '#e74c3c' :
                                         model.value < model.thresholdHigh ? '#f39c12' : '#2ecc71'}}" />
        <stop offset="100%" stop-color="{{model.value < model.thresholdLow ? '#c0392b' :
                                          model.value < model.thresholdHigh ? '#e67e22' : '#27ae60'}}" />
    </linearGradient>

    <rect width="{{model.value * 3}}" height="80" fill="url(#statusGrad)" />
    <text x="10" y="95" font-size="14">{{model.value}}%</text>
</svg>
```

### Dynamic Theme Gradients

```html
<!-- Model: { theme: { primary: "#3498db", secondary: "#2ecc71" } } -->
<svg width="400" height="100">
    <linearGradient id="themeGrad">
        <stop offset="0%" stop-color="{{model.theme.primary}}" />
        <stop offset="100%" stop-color="{{model.theme.secondary}}" />
    </linearGradient>

    <rect width="400" height="100" fill="url(#themeGrad)" />
</svg>
```

---

## See Also

- [radialGradient](/reference/svgtags/radialGradient.html) - Radial gradient definition element
- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [svg](/reference/htmltags/svg.html) - SVG canvas element
- [rect](/reference/svgtags/rect.html) - Rectangle element (commonly used with gradients)
- [path](/reference/svgtags/path.html) - Path element (supports gradient fills and strokes)
- [circle](/reference/svgtags/circle.html) - Circle element (supports gradient fills)
- [CSS Gradients](/reference/styles/gradients.html) - CSS gradient properties
- [SVG Fills](/reference/svg/fills.html) - SVG fill patterns and gradients
- [Data Binding](/reference/binding/) - Data binding and expressions

---
