---
layout: default
title: gradientUnits
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @gradientUnits : The Gradient Coordinate System Attribute

The `gradientUnits` attribute defines the coordinate system used for gradient positioning attributes (x1, y1, x2, y2 for linear gradients; cx, cy, r, fx, fy for radial gradients). It determines whether coordinates are relative to the filled shape or absolute in document space.

## Usage

The `gradientUnits` attribute is used to:
- Define whether gradient coordinates are relative or absolute
- Enable responsive gradients that scale with shapes (objectBoundingBox)
- Create consistent gradients across multiple shapes (userSpaceOnUse)
- Support data-driven coordinate system selection
- Control how gradients adapt to different container sizes
- Position gradients precisely in document space

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
        <!-- Relative coordinates (default) -->
        <linearGradient id="relative" x1="0%" y1="0%" x2="100%" y2="0%"
                        gradientUnits="objectBoundingBox">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>

        <!-- Absolute coordinates -->
        <linearGradient id="absolute" x1="0" y1="0" x2="400" y2="0"
                        gradientUnits="userSpaceOnUse">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>
    <rect x="10" y="10" width="380" height="80" fill="url(#relative)" />
    <rect x="10" y="110" width="380" height="80" fill="url(#absolute)" />
</svg>
```

---

## Supported Values

| Value | Description | Coordinates | Use Case |
|-------|-------------|-------------|----------|
| `objectBoundingBox` | Relative to shape (default) | 0-1 or 0%-100% | Responsive gradients that scale with shapes |
| `userSpaceOnUse` | Absolute document space | pt, px, mm, cm, in | Fixed gradients consistent across shapes |

### Default Behavior

```html
<!-- These are equivalent (objectBoundingBox is default) -->
<linearGradient id="g1" x1="0%" x2="100%">
<linearGradient id="g2" x1="0%" x2="100%" gradientUnits="objectBoundingBox">
```

---

## Supported Elements

The `gradientUnits` attribute is supported on:

- **[&lt;linearGradient&gt;](/reference/svgtags/linearGradient.html)** - Linear gradient coordinate system
- **[&lt;radialGradient&gt;](/reference/svgtags/radialGradient.html)** - Radial gradient coordinate system

---

## Data Binding

### Dynamic Coordinate System Selection

Choose coordinate system based on data:

```html
<!-- Model: { useAbsolute: false } -->
<svg width="400" height="200">
    <defs>
        <linearGradient id="dynamicUnits"
                        x1="{{model.useAbsolute ? '0' : '0%'}}"
                        y1="0%"
                        x2="{{model.useAbsolute ? '400' : '100%'}}"
                        y2="0%"
                        gradientUnits="{{model.useAbsolute ? 'userSpaceOnUse' : 'objectBoundingBox'}}">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>
    <rect width="400" height="200" fill="url(#dynamicUnits)" />
</svg>
```

### Responsive vs Fixed Gradients

Switch between responsive and fixed based on context:

```html
<!-- Model: { isResponsive: true, containerWidth: 500 } -->
<svg width="{{model.containerWidth}}" height="300">
    <defs>
        <linearGradient id="adaptiveGrad"
                        x1="{{model.isResponsive ? '0%' : '0'}}"
                        y1="0%"
                        x2="{{model.isResponsive ? '100%' : model.containerWidth}}"
                        y2="0%"
                        gradientUnits="{{model.isResponsive ? 'objectBoundingBox' : 'userSpaceOnUse'}}">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>
    <rect width="{{model.containerWidth}}" height="300" fill="url(#adaptiveGrad)" />
</svg>
```

### Chart-Specific Gradients

Use absolute coordinates for consistent chart backgrounds:

```html
<!-- Model: { chartX: 50, chartY: 50, chartWidth: 400, chartHeight: 250 } -->
<svg width="500" height="300">
    <defs>
        <linearGradient id="chartBg"
                        x1="{{model.chartX}}"
                        y1="{{model.chartY}}"
                        x2="{{model.chartX}}"
                        y2="{{model.chartY + model.chartHeight}}"
                        gradientUnits="userSpaceOnUse">
            <stop offset="0%" stop-color="#ecf0f1" />
            <stop offset="100%" stop-color="#ffffff" />
        </linearGradient>
    </defs>

    <rect x="{{model.chartX}}" y="{{model.chartY}}"
          width="{{model.chartWidth}}" height="{{model.chartHeight}}"
          fill="url(#chartBg)" />
</svg>
```

### Shared Gradient Across Multiple Shapes

Use userSpaceOnUse for consistent gradient across shapes:

```html
<!-- Model: { shapes: [{x: 50, y: 50, size: 80}, {x: 200, y: 50, size: 120}] } -->
<svg width="600" height="300">
    <defs>
        <linearGradient id="sharedGrad"
                        x1="0" y1="0" x2="600" y2="0"
                        gradientUnits="userSpaceOnUse">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#9b59b6" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <template data-bind="{{model.shapes}}">
        <rect x="{{.x}}" y="{{.y}}" width="{{.size}}" height="{{.size}}"
              fill="url(#sharedGrad)" />
    </template>
</svg>
```

---

## Notes

### objectBoundingBox (Default)

**Characteristics:**
- Coordinates relative to shape's bounding box
- Values: 0-1 (decimal) or 0%-100% (percentage)
- (0, 0) = top-left of shape, (1, 1) = bottom-right of shape
- Gradient scales automatically with shape size
- Most common for UI elements and responsive designs

**Advantages:**
- Automatically adapts to shape size
- Same gradient definition works for different sized shapes
- Percentages are intuitive and readable
- Good for responsive designs

**Limitations:**
- Can't create gradients spanning multiple shapes
- Coordinate calculation more complex for exact positions
- May not align perfectly across different shapes

```html
<!-- Gradient fills entire shape regardless of size -->
<linearGradient id="responsive" x1="0%" y1="0%" x2="100%" y2="0%"
                gradientUnits="objectBoundingBox">
    <stop offset="0%" stop-color="blue" />
    <stop offset="100%" stop-color="red" />
</linearGradient>

<!-- Works for any size rectangle -->
<rect x="10" y="10" width="100" height="50" fill="url(#responsive)" />
<rect x="120" y="10" width="200" height="100" fill="url(#responsive)" />
```

### userSpaceOnUse

**Characteristics:**
- Coordinates in absolute document units
- Values: pt, px, mm, cm, in (no percentages)
- Fixed position in SVG coordinate system
- Gradient doesn't scale with shape
- Useful for consistent effects across multiple shapes

**Advantages:**
- Precise positioning in document space
- Consistent gradient across multiple shapes
- Simpler coordinate calculations
- Good for chart backgrounds and fixed layouts

**Limitations:**
- Doesn't adapt to shape size automatically
- Requires knowing exact document dimensions
- Less flexible for responsive designs
- More complex to maintain with dynamic content

```html
<!-- Gradient fixed in document space -->
<linearGradient id="fixed" x1="0" y1="0" x2="400" y2="0"
                gradientUnits="userSpaceOnUse">
    <stop offset="0%" stop-color="blue" />
    <stop offset="100%" stop-color="red" />
</linearGradient>

<!-- All rectangles share same gradient position -->
<rect x="0" y="10" width="200" height="50" fill="url(#fixed)" />
<rect x="200" y="10" width="200" height="50" fill="url(#fixed)" />
```

### Choosing the Right System

**Use objectBoundingBox when:**
- Creating reusable gradient definitions
- Building responsive UI components
- Gradient should scale with shape
- Working with different sized elements
- Percentages make sense for your use case

**Use userSpaceOnUse when:**
- Creating backgrounds spanning multiple shapes
- Building data visualizations with fixed layouts
- Gradient should be consistent across shapes
- Working with absolute positioning
- Creating chart backgrounds or grid overlays

### Performance Considerations

- objectBoundingBox is generally more efficient
- userSpaceOnUse may require more calculations
- Consider caching for complex gradients
- Test performance with many gradient instances

---

## Examples

### Object Bounding Box (Relative Coordinates)

```html
<svg width="400" height="200">
    <defs>
        <linearGradient id="relative" x1="0%" y1="0%" x2="100%" y2="0%"
                        gradientUnits="objectBoundingBox">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <!-- Same gradient adapts to different sizes -->
    <rect x="10" y="10" width="180" height="80" fill="url(#relative)" />
    <rect x="210" y="10" width="180" height="180" fill="url(#relative)" />
</svg>
```

### User Space on Use (Absolute Coordinates)

```html
<svg width="400" height="200">
    <defs>
        <linearGradient id="absolute" x1="0" y1="0" x2="400" y2="0"
                        gradientUnits="userSpaceOnUse">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </linearGradient>
    </defs>

    <!-- Gradient position is fixed in document space -->
    <rect x="0" y="10" width="200" height="80" fill="url(#absolute)" />
    <rect x="200" y="10" width="200" height="80" fill="url(#absolute)" />
</svg>
```

### Comparison: Same Gradient Both Systems

```html
<svg width="600" height="300">
    <defs>
        <!-- Relative: adapts to each shape -->
        <linearGradient id="rel" x1="0%" y1="0%" x2="100%" y2="0%"
                        gradientUnits="objectBoundingBox">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>

        <!-- Absolute: fixed in document -->
        <linearGradient id="abs" x1="0" y1="150" x2="600" y2="150"
                        gradientUnits="userSpaceOnUse">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <text x="10" y="30" font-size="14" font-weight="bold">objectBoundingBox</text>
    <rect x="10" y="40" width="100" height="60" fill="url(#rel)" />
    <rect x="130" y="40" width="200" height="60" fill="url(#rel)" />
    <rect x="350" y="40" width="150" height="60" fill="url(#rel)" />

    <text x="10" y="180" font-size="14" font-weight="bold">userSpaceOnUse</text>
    <rect x="10" y="190" width="100" height="60" fill="url(#abs)" />
    <rect x="130" y="190" width="200" height="60" fill="url(#abs)" />
    <rect x="350" y="190" width="150" height="60" fill="url(#abs)" />
</svg>
```

### Radial Gradient with Object Bounding Box

```html
<svg width="400" height="300">
    <defs>
        <radialGradient id="radialRel" cx="50%" cy="50%" r="50%"
                        gradientUnits="objectBoundingBox">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="100%" stop-color="#3498db" />
        </radialGradient>
    </defs>

    <!-- Gradient centers on each shape -->
    <circle cx="100" cy="150" r="80" fill="url(#radialRel)" />
    <rect x="250" y="50" width="130" height="200" fill="url(#radialRel)" />
</svg>
```

### Radial Gradient with User Space on Use

```html
<svg width="400" height="300">
    <defs>
        <radialGradient id="radialAbs" cx="200" cy="150" r="150"
                        gradientUnits="userSpaceOnUse">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="100%" stop-color="#9b59b6" />
        </radialGradient>
    </defs>

    <!-- All shapes share same gradient center -->
    <circle cx="100" cy="150" r="80" fill="url(#radialAbs)" />
    <circle cx="300" cy="150" r="80" fill="url(#radialAbs)" />
</svg>
```

### Chart Background with Fixed Gradient

```html
<svg width="600" height="400">
    <defs>
        <linearGradient id="chartBg" x1="50" y1="50" x2="50" y2="350"
                        gradientUnits="userSpaceOnUse">
            <stop offset="0%" stop-color="#ecf0f1" />
            <stop offset="100%" stop-color="#ffffff" />
        </linearGradient>
    </defs>

    <!-- Chart area -->
    <rect x="50" y="50" width="500" height="300" fill="url(#chartBg)" />

    <!-- Grid lines -->
    <line x1="50" y1="50" x2="550" y2="50" stroke="#ddd" />
    <line x1="50" y1="125" x2="550" y2="125" stroke="#ddd" />
    <line x1="50" y1="200" x2="550" y2="200" stroke="#ddd" />
    <line x1="50" y1="275" x2="550" y2="275" stroke="#ddd" />
    <line x1="50" y1="350" x2="550" y2="350" stroke="#ddd" />
</svg>
```

### Responsive Button Gradients

```html
<svg width="600" height="200">
    <defs>
        <linearGradient id="buttonGrad" x1="0%" y1="0%" x2="0%" y2="100%"
                        gradientUnits="objectBoundingBox">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="50%" stop-color="#2980b9" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>

    <rect x="20" y="50" width="150" height="50" rx="25" fill="url(#buttonGrad)" />
    <text x="95" y="80" text-anchor="middle" fill="white" font-size="16">Small</text>

    <rect x="220" y="30" width="200" height="70" rx="35" fill="url(#buttonGrad)" />
    <text x="320" y="72" text-anchor="middle" fill="white" font-size="20">Medium</text>

    <rect x="460" y="20" width="130" height="90" rx="45" fill="url(#buttonGrad)" />
    <text x="525" y="72" text-anchor="middle" fill="white" font-size="18">Large</text>
</svg>
```

### Multi-Shape Shared Gradient

```html
<svg width="600" height="300">
    <defs>
        <linearGradient id="shared" x1="0" y1="0" x2="600" y2="0"
                        gradientUnits="userSpaceOnUse">
            <stop offset="0%" stop-color="#e74c3c" />
            <stop offset="25%" stop-color="#f39c12" />
            <stop offset="50%" stop-color="#f1c40f" />
            <stop offset="75%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#3498db" />
        </linearGradient>
    </defs>

    <!-- All bars share the same gradient position -->
    <rect x="50" y="100" width="60" height="180" fill="url(#shared)" />
    <rect x="130" y="50" width="60" height="230" fill="url(#shared)" />
    <rect x="210" y="120" width="60" height="160" fill="url(#shared)" />
    <rect x="290" y="80" width="60" height="200" fill="url(#shared)" />
    <rect x="370" y="140" width="60" height="140" fill="url(#shared)" />
    <rect x="450" y="90" width="60" height="190" fill="url(#shared)" />
</svg>
```

### Spotlight Effect Across Canvas

```html
<svg width="600" height="400">
    <defs>
        <radialGradient id="spotlight" cx="300" cy="200" r="300"
                        gradientUnits="userSpaceOnUse">
            <stop offset="0%" stop-color="#ffffff" stop-opacity="0.9" />
            <stop offset="50%" stop-color="#95a5a6" stop-opacity="0.4" />
            <stop offset="100%" stop-color="#2c3e50" stop-opacity="1" />
        </radialGradient>
    </defs>

    <rect width="600" height="400" fill="url(#spotlight)" />
</svg>
```

### Data-Driven Coordinate System

```html
<!-- Model: { mode: "responsive", width: 500, height: 300 } -->
<svg width="{{model.width}}" height="{{model.height}}">
    <defs>
        <linearGradient id="modeGrad"
                        x1="{{model.mode === 'responsive' ? '0%' : '0'}}"
                        y1="0%"
                        x2="{{model.mode === 'responsive' ? '100%' : model.width}}"
                        y2="0%"
                        gradientUnits="{{model.mode === 'responsive' ? 'objectBoundingBox' : 'userSpaceOnUse'}}">
            <stop offset="0%" stop-color="#667eea" />
            <stop offset="100%" stop-color="#764ba2" />
        </linearGradient>
    </defs>

    <rect width="{{model.width}}" height="{{model.height}}" fill="url(#modeGrad)" />
</svg>
```

### Timeline with Absolute Positioning

```html
<svg width="800" height="150">
    <defs>
        <linearGradient id="timelineGrad" x1="100" y1="0" x2="700" y2="0"
                        gradientUnits="userSpaceOnUse">
            <stop offset="0%" stop-color="#3498db" />
            <stop offset="33%" stop-color="#9b59b6" />
            <stop offset="67%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#f39c12" />
        </linearGradient>
    </defs>

    <rect x="100" y="60" width="600" height="30" rx="15" fill="url(#timelineGrad)" />
    <circle cx="100" cy="75" r="12" fill="#3498db" />
    <circle cx="300" cy="75" r="12" fill="#9b59b6" />
    <circle cx="500" cy="75" r="12" fill="#e74c3c" />
    <circle cx="700" cy="75" r="12" fill="#f39c12" />
</svg>
```

### Gauge with Relative Gradient

```html
<svg width="300" height="200" viewBox="0 0 300 200">
    <defs>
        <linearGradient id="gaugeGrad" x1="0%" x2="100%"
                        gradientUnits="objectBoundingBox">
            <stop offset="0%" stop-color="#2ecc71" />
            <stop offset="50%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </linearGradient>
    </defs>

    <path d="M 50,150 A 100,100 0 0,1 250,150"
          fill="none" stroke="url(#gaugeGrad)" stroke-width="30" stroke-linecap="round" />
</svg>
```

---

## See Also

- [linearGradient](/reference/svgtags/linearGradient.html) - Linear gradient definition element
- [radialGradient](/reference/svgtags/radialGradient.html) - Radial gradient definition element
- [x1](/reference/svgattributes/x1.html), [y1](/reference/svgattributes/y1.html), [x2](/reference/svgattributes/x2.html), [y2](/reference/svgattributes/y2.html) - Linear gradient coordinates
- [cx](/reference/svgattributes/cx.html), [cy](/reference/svgattributes/cy.html), [r](/reference/svgattributes/r.html) - Radial gradient coordinates
- [fx](/reference/svgattributes/fx.html), [fy](/reference/svgattributes/fy.html) - Radial gradient focal point
- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [Data Binding](/reference/binding/) - Data binding and expressions

---
