---
layout: default
title: offset
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @offset : The Gradient Stop Position Attribute

The `offset` attribute defines the position of a color stop along a gradient vector. It determines where a specific color appears in the gradient transition, with 0% representing the gradient's start and 100% representing its end. Multiple stops at different offsets create smooth multi-color transitions.

## Usage

The `offset` attribute is used to:
- Define the precise position of a color stop along a gradient (0%-100%)
- Create smooth color transitions between stops
- Enable complex multi-color gradients
- Support data-driven color positioning for data visualizations
- Work with both linear and radial gradients
- Create hard color boundaries with duplicate offsets
- Position colors based on data values and thresholds

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="100">
    <defs>
        <linearGradient id="multiStop" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#multiStop)" />
</svg>
```

---

## Supported Values

| Value Type | Format | Description | Example |
|------------|--------|-------------|---------|
| Percentage | `0%` to `100%` | Most common format, represents position along gradient | `offset="50%"` |
| Decimal | `0` to `1.0` | Equivalent to percentage (0 = 0%, 1 = 100%) | `offset="0.5"` |
| Required | - | At least 2 stops required per gradient | - |

### Common Patterns

```html
<!-- Two-color gradient -->
<stop offset="0%" stop-color="blue" />
<stop offset="100%" stop-color="red" />

<!-- Three-color gradient with center stop -->
<stop offset="0%" stop-color="blue" />
<stop offset="50%" stop-color="green" />
<stop offset="100%" stop-color="red" />

<!-- Asymmetric gradient -->
<stop offset="0%" stop-color="blue" />
<stop offset="30%" stop-color="green" />
<stop offset="100%" stop-color="red" />

<!-- Hard transition (duplicate offset) -->
<stop offset="50%" stop-color="blue" />
<stop offset="50%" stop-color="red" />
```

---

## Supported Elements

The `offset` attribute is supported on:

- **[&lt;stop&gt;](/reference/svgtags/stop.html)** - Gradient color stop element

Used within:
- `<linearGradient>` - Linear gradient definitions
- `<radialGradient>` - Radial gradient definitions

---

## Data Binding

### Dynamic Stop Positions

Bind stop positions to data for flexible gradients:

```html
<!-- Model: { midpoint: 35 } -->
<svg width="400" height="100">
    <defs>
        <linearGradient id="dynamicMid">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="{{model.midpoint}}%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#dynamicMid)" />
</svg>
```

### Data Threshold Visualization

Position color stops at data threshold boundaries:

```html
<!-- Model: { warningThreshold: 60, dangerThreshold: 80 } -->
<svg width="500" height="100">
    <defs>
        <linearGradient id="thresholds">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="{{model.warningThreshold}}%" stop-color="#2ecc71" />
            <stop offset="{{model.warningThreshold}}%" stop-color="#f39c12" />
            <stop offset="{{model.dangerThreshold}}%" stop-color="#f39c12" />
            <stop offset="{{model.dangerThreshold}}%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="500" height="100" fill="url(#thresholds)" />
</svg>
```

### Generate Stops from Data Array

Create gradient stops dynamically from data:

```html
<!-- Model: { dataPoints: [{value: 0, color: "#2ecc71"}, {value: 30, color: "#f1c40f"},
              {value: 70, color: "#e67e22"}, {value: 100, color: "#e74c3c"}] } -->
<svg width="400" height="100">
    <defs>
        <linearGradient id="dataStops">
            <template data-bind="{{model.dataPoints}}">
                <stop offset="{{.value}}%" stop-color="{{.color}}" />
            </template>
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#dataStops)" />
</svg>
```

### Progress-Based Color Stops

Position stops based on completion or progress:

```html
<!-- Model: { progress: 65 } -->
<svg width="400" height="100">
    <defs>
        <linearGradient id="progressGrad">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="{{model.progress}}%" stop-color="#2ecc71" />
            <stop offset="{{model.progress}}%" stop-color="#bdc3c7" />
            <stop offset="100%" stop-color="#ecf0f1" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#progressGrad)" />
</svg>
```

### Temperature Scale with Data Offsets

Create color scales based on data ranges:

```html
<!-- Model: { freezing: 0, cool: 25, warm: 50, hot: 75, extreme: 100 } -->
<svg width="400" height="100">
    <defs>
        <linearGradient id="tempScale">
            <stop offset="{{model.freezing}}%" stop-color="#3498db" />
            <stop offset="{{model.cool}}%" stop-color="#2ecc71" />
            <stop offset="{{model.warm}}%" stop-color="#f1c40f" />
            <stop offset="{{model.hot}}%" stop-color="#e67e22" />
            <stop offset="{{model.extreme}}%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#tempScale)" />
</svg>
```

---

## Notes

### Stop Position Behavior

How offset values work:
- **0%**: Gradient start (x1,y1 for linear; focal point for radial)
- **100%**: Gradient end (x2,y2 for linear; outer circle for radial)
- **0-100%**: Linear interpolation between start and end
- **Sorted automatically**: Stops process by offset value regardless of declaration order

### Color Transition Rules

1. **Smooth Transition**: Single color per offset creates blend
   ```html
   <stop offset="0%" stop-color="blue" />
   <stop offset="100%" stop-color="red" />
   ```

2. **Hard Transition**: Duplicate offset creates sharp edge
   ```html
   <stop offset="50%" stop-color="blue" />
   <stop offset="50%" stop-color="red" />
   ```

3. **Multi-Color Blend**: Multiple stops create complex transitions
   ```html
   <stop offset="0%" stop-color="blue" />
   <stop offset="33%" stop-color="green" />
   <stop offset="67%" stop-color="yellow" />
   <stop offset="100%" stop-color="red" />
   ```

### Minimum Requirements

- **Minimum stops**: 2 stops required (start and end)
- **Optimal stops**: 3-5 stops for smooth multi-color gradients
- **No maximum**: Unlimited stops supported (consider performance)

### Out-of-Range Offsets

Stops outside 0%-100% work with spreadMethod:
- **pad (default)**: Extends end colors
- **repeat**: Tiles gradient pattern
- **reflect**: Mirrors gradient

### Best Practices

1. **Use percentages**: More readable than decimal notation
2. **Logical ordering**: Declare stops in order (though not required)
3. **Even distribution**: Consider visual weight of colors
4. **Performance**: Minimize stops for better rendering
5. **Accessibility**: Ensure adequate color contrast

---

## Examples

### Basic Two-Stop Gradient

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="twoStop">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#twoStop)" />
</svg>
```

### Three-Stop Gradient with Center Color

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="threeStop">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#threeStop)" />
</svg>
```

### Asymmetric Color Positioning

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="asymmetric">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="30%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#asymmetric)" />
</svg>
```

### Rainbow Gradient (Seven Colors)

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="rainbow">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="17%" stop-color="#f39c12" />
            <stop offset="33%" stop-color="#f1c40f" />
            <stop offset="50%" stop-color="#2ecc71" />
            <stop offset="67%" stop-color="#3498db" />
            <stop offset="83%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#rainbow)" />
</svg>
```

### Hard Color Transition

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="hard">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#hard)" />
</svg>
```

### Traffic Light Zones

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="traffic">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="33%" stop-color="#2ecc71" />
            <stop offset="33%" stop-color="#f1c40f" />
            <stop offset="67%" stop-color="#f1c40f" />
            <stop offset="67%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#traffic)" />
</svg>
```

### Heatmap Color Scale

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="heatmap">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="25%" stop-color="#f1c40f" />
            <stop offset="50%" stop-color="#e67e22" />
            <stop offset="75%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#c0392b" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#heatmap)" />
    <text x="10" y="90" font-size="12">Low</text>
    <text x="360" y="90" font-size="12">High</text>
</svg>
```

### Progress Bar with Threshold

```html
<!-- Model: { progress: 65 } -->
<svg width="400" height="60">
    <defs>
        <linearGradient id="progressThresh">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="{{model.progress}}%" stop-color="#27ae60" />
            <stop offset="{{model.progress}}%" stop-color="#ecf0f1" />
            <stop offset="100%" stop-color="#ecf0f1" />
        </linearGradient>
    </defs>

    <rect width="400" height="60" rx="30" fill="url(#progressThresh)" />
    <text x="200" y="38" text-anchor="middle" font-size="18" fill="#333">
        {{model.progress}}%
    </text>
</svg>
```

### Chart Color Zones

```html
<svg width="500" height="300">
    <defs>
        <linearGradient id="zones" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#e74c3c" stop-opacity="0.2" />
            <stop offset="25%" stop-color="#e74c3c" stop-opacity="0.2" />
            <stop offset="25%" stop-color="#f39c12" stop-opacity="0.2" />
            <stop offset="75%" stop-color="#f39c12" stop-opacity="0.2" />
            <stop offset="75%" stop-color="#2ecc71" stop-opacity="0.2" />
            <stop offset="100%" stop-color="#2ecc71" stop-opacity="0.2" />
        </linearGradient>
    </defs>

    <rect width="500" height="300" fill="url(#zones)" />
</svg>
```

### Gauge Background Gradient

```html
<svg width="300" height="200" viewBox="0 0 300 200">
    <defs>
        <linearGradient id="gaugeGrad" x1="0%" x2="100%">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="40%" stop-color="#f1c40f" />
            <stop offset="70%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <path d="M 50,150 A 100,100 0 0,1 250,150"
          fill="none" stroke="url(#gaugeGrad)" stroke-width="30" stroke-linecap="round" />
</svg>
```

### Data Visualization Scale

```html
<svg width="400" height="80">
    <defs>
        <linearGradient id="dataScale">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="20%" stop-color="#2ecc71" />
            <stop offset="40%" stop-color="#f1c40f" />
            <stop offset="60%" stop-color="#e67e22" />
            <stop offset="80%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#c0392b" />
        </linearGradient>
    </defs>

    <rect x="20" y="30" width="360" height="20" fill="url(#dataScale)" />
    <!-- Scale markers -->
    <line x1="20" y1="25" x2="20" y2="55" stroke="#333" stroke-width="2" />
    <line x1="92" y1="25" x2="92" y2="55" stroke="#333" stroke-width="2" />
    <line x1="164" y1="25" x2="164" y2="55" stroke="#333" stroke-width="2" />
    <line x1="236" y1="25" x2="236" y2="55" stroke="#333" stroke-width="2" />
    <line x1="308" y1="25" x2="308" y2="55" stroke="#333" stroke-width="2" />
    <line x1="380" y1="25" x2="380" y2="55" stroke="#333" stroke-width="2" />
</svg>
```

### Fade Effect with Multiple Stops

```html
<svg width="400" height="100">
    <defs>
        <linearGradient id="fade">
            <stop offset="0%" stop-color="#3498db" stop-opacity="0" />
            <stop offset="20%" stop-color="#3498db" stop-opacity="0.3" />
            <stop offset="50%" stop-color="#3498db" stop-opacity="1" />
            <stop offset="80%" stop-color="#3498db" stop-opacity="0.3" />
            <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
        </linearGradient>
    </defs>
    <rect width="400" height="100" fill="url(#fade)" />
</svg>
```

### Status Badge with Gradient Zones

```html
<!-- Model: { status: "warning", progress: 55 } -->
<svg width="400" height="80">
    <defs>
        <linearGradient id="statusBadge">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="50%" stop-color="#2ecc71" />
            <stop offset="50%" stop-color="#f39c12" />
            <stop offset="80%" stop-color="#f39c12" />
            <stop offset="80%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <rect width="400" height="80" rx="40" fill="url(#statusBadge)" />
</svg>
```

### Data-Driven Threshold Stops

```html
<!-- Model: { low: 30, medium: 60, high: 80 } -->
<svg width="500" height="100">
    <defs>
        <linearGradient id="dataThresh">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="{{model.low}}%" stop-color="#2ecc71" />
            <stop offset="{{model.low}}%" stop-color="#f1c40f" />
            <stop offset="{{model.medium}}%" stop-color="#f1c40f" />
            <stop offset="{{model.medium}}%" stop-color="#e67e22" />
            <stop offset="{{model.high}}%" stop-color="#e67e22" />
            <stop offset="{{model.high}}%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <rect width="500" height="100" fill="url(#dataThresh)" />
</svg>
```

### Timeline with Color Phases

```html
<svg width="600" height="100">
    <defs>
        <linearGradient id="timeline">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="25%" stop-color="#3498db" />
            <stop offset="25%" stop-color="#9b59b6" />
            <stop offset="50%" stop-color="#9b59b6" />
            <stop offset="50%" stop-color="#e74c3c" />
            <stop offset="75%" stop-color="#e74c3c" />
            <stop offset="75%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#f39c12" />
        </linearGradient>
    </defs>

    <rect x="50" y="40" width="500" height="20" rx="10" fill="url(#timeline)" />
</svg>
```

---

## See Also

- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [stop-color](/reference/svgattributes/stop-color.html) - Color value at stop position
- [stop-opacity](/reference/svgattributes/stop-opacity.html) - Opacity at stop position
- [linearGradient](/reference/svgtags/linearGradient.html) - Linear gradient definition element
- [radialGradient](/reference/svgtags/radialGradient.html) - Radial gradient definition element
- [spreadMethod](/reference/svgattributes/spreadMethod.html) - Gradient spread behavior
- [Data Binding](/reference/binding/) - Data binding and expressions
- [CSS Gradients](/reference/styles/gradients.html) - CSS gradient properties

---
