---
layout: default
title: markerWidth
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @markerWidth : The Marker Viewport Width Attribute

The `markerWidth` attribute defines the width of the marker viewport - the coordinate system canvas in which the marker content is rendered. This controls the horizontal size of the marker's coordinate space, not necessarily the visual size of the marker.

## Usage

The `markerWidth` attribute is used to:
- Define the horizontal extent of the marker's coordinate system
- Control the aspect ratio of marker content
- Set up the coordinate space for marker graphics
- Support responsive marker sizing
- Enable data-driven marker dimensions
- Coordinate with markerHeight for proper proportions

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
        <marker id="arrow" markerWidth="10" markerHeight="10"
                refX="5" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#arrow)"/>
</svg>
```

---

## Supported Values

| Value Type | Description | Example |
|------------|-------------|---------|
| `<length>` | Positive number (no units) | `markerWidth="10"` |
| Default | `3` if not specified | (default) |

### Valid Values

```html
<!-- Common sizes -->
markerWidth="5"    <!-- Small marker -->
markerWidth="10"   <!-- Medium marker -->
markerWidth="20"   <!-- Large marker -->

<!-- Decimal values -->
markerWidth="12.5"

<!-- Must be positive -->
markerWidth="0"    <!-- Invalid: no viewport -->
markerWidth="-5"   <!-- Invalid: negative not allowed -->
```

---

## Supported Elements

The `markerWidth` attribute is supported on:

- **[&lt;marker&gt;](/reference/svgtags/marker.html)** - Marker definition element

---

## Data Binding

### Dynamic Marker Size

Adjust marker width based on data:

```html
<!-- Model: { markerSize: 12, importance: 'high' } -->
<svg width="400" height="200">
    <defs>
        <marker id="dynamic-marker"
                markerWidth="{{model.markerSize}}"
                markerHeight="{{model.markerSize}}"
                refX="{{model.markerSize / 2}}"
                refY="{{model.markerSize / 2}}">
            <circle cx="{{model.markerSize / 2}}" cy="{{model.markerSize / 2}}"
                    r="{{model.markerSize / 2 - 1}}"
                    fill="{{model.importance === 'high' ? '#e74c3c' : '#3498db'}}"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#dynamic-marker)"/>
</svg>
```

### Responsive Arrow Sizing

Scale markers based on context:

```html
<!-- Model: { arrowScale: 1.5, strokeWidth: 3 } -->
<svg width="400" height="200">
    <defs>
        <marker id="scaled-arrow"
                markerWidth="{{10 * model.arrowScale}}"
                markerHeight="{{10 * model.arrowScale}}"
                refX="{{10 * model.arrowScale}}"
                refY="{{5 * model.arrowScale}}"
                orient="auto">
            <polygon points="0,0 {{10 * model.arrowScale}},{{5 * model.arrowScale}} 0,{{10 * model.arrowScale}}"
                     fill="#3498db"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="{{model.strokeWidth}}"
          marker-end="url(#scaled-arrow)"/>
</svg>
```

### Data-Driven Marker Dimensions

```html
<!-- Model: {
    connections: [
        { points: '50,50 200,50', width: 8, height: 8 },
        { points: '50,100 200,100', width: 12, height: 12 },
        { points: '50,150 200,150', width: 16, height: 16 }
    ]
} -->
<svg width="250" height="200">
    <defs>
        <template data-bind="{{model.connections}}">
            <marker id="marker-{{$index}}"
                    markerWidth="{{.width}}"
                    markerHeight="{{.height}}"
                    refX="{{.width}}"
                    refY="{{.height / 2}}"
                    orient="auto">
                <polygon points="0,0 {{.width}},{{.height / 2}} 0,{{.height}}"
                         fill="#3498db"/>
            </marker>
        </template>
    </defs>

    <template data-bind="{{model.connections}}">
        <polyline points="{{.points}}"
                  fill="none" stroke="#2c3e50" stroke-width="2"
                  marker-end="url(#marker-{{$index}})"/>
    </template>
</svg>
```

---

## Notes

### Viewport vs Visual Size

The `markerWidth` defines the **coordinate space**, not the rendered size:
- It establishes the horizontal range of user units available
- Marker content is drawn within this coordinate system
- The actual visual size depends on the marker content and `markerUnits`

```html
<!-- markerWidth=10 means coordinates from 0-10 are available -->
<marker id="example" markerWidth="10" markerHeight="10">
    <!-- This circle uses coordinates 0-10 -->
    <circle cx="5" cy="5" r="4"/>
</marker>
```

### Relationship with markerHeight

`markerWidth` and `markerHeight` together define the viewport rectangle:

```html
<!-- Square viewport: 10x10 units -->
<marker markerWidth="10" markerHeight="10">

<!-- Wide viewport: 20x10 units -->
<marker markerWidth="20" markerHeight="10">

<!-- Tall viewport: 10x20 units -->
<marker markerWidth="10" markerHeight="20">
```

### Default Value

If `markerWidth` is not specified, the default is **3 units**:

```html
<!-- These are equivalent -->
<marker id="m1">
<marker id="m2" markerWidth="3">
```

### Scaling with markerUnits

The `markerUnits` attribute affects how markers scale:

**userSpaceOnUse (default):**
```html
<marker markerWidth="10" markerUnits="userSpaceOnUse">
    <!-- Marker is always 10 units wide in document space -->
</marker>
```

**strokeWidth:**
```html
<marker markerWidth="3" markerUnits="strokeWidth">
    <!-- Marker width is 3 × stroke width -->
    <!-- stroke-width="2" → effective width = 6 -->
    <!-- stroke-width="5" → effective width = 15 -->
</marker>
```

### Aspect Ratio Considerations

Maintain consistent proportions by coordinating markerWidth and markerHeight:

```html
<!-- 2:1 aspect ratio -->
<marker markerWidth="20" markerHeight="10">
    <rect width="20" height="10" fill="blue"/>
</marker>

<!-- 1:1 aspect ratio (square) -->
<marker markerWidth="12" markerHeight="12">
    <circle cx="6" cy="6" r="5" fill="red"/>
</marker>
```

### Reference Point Coordination

The `refX` value should coordinate with `markerWidth`:

```html
<!-- Center reference point horizontally -->
<marker markerWidth="10" markerHeight="10"
        refX="5" refY="5">
    <!-- refX = markerWidth / 2 -->
</marker>

<!-- Right-aligned reference point -->
<marker markerWidth="10" markerHeight="10"
        refX="10" refY="5">
    <!-- refX = markerWidth for right edge -->
</marker>
```

### Performance Impact

- Larger viewports don't inherently impact performance
- The complexity of marker content matters more
- Choose appropriate dimensions for your marker design
- Avoid unnecessarily large viewports

### Common Patterns

**Standard arrow marker:**
```html
<marker markerWidth="10" markerHeight="10"
        refX="10" refY="5" orient="auto">
    <polygon points="0,0 10,5 0,10" fill="black"/>
</marker>
```

**Circular marker:**
```html
<marker markerWidth="12" markerHeight="12"
        refX="6" refY="6">
    <circle cx="6" cy="6" r="5" fill="blue"/>
</marker>
```

**Icon marker:**
```html
<marker markerWidth="24" markerHeight="24"
        refX="12" refY="12">
    <!-- More complex icon content -->
</marker>
```

---

## Examples

### Small Arrow Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="small-arrow" markerWidth="6" markerHeight="6"
                refX="6" refY="3" orient="auto">
            <polygon points="0,0 6,3 0,6" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#small-arrow)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">markerWidth="6"</text>
</svg>
```

### Medium Arrow Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="medium-arrow" markerWidth="12" markerHeight="12"
                refX="12" refY="6" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="#3498db"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#medium-arrow)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">markerWidth="12"</text>
</svg>
```

### Large Arrow Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="large-arrow" markerWidth="20" markerHeight="20"
                refX="20" refY="10" orient="auto">
            <polygon points="0,0 20,10 0,20" fill="#2ecc71"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="3"
          marker-end="url(#large-arrow)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">markerWidth="20"</text>
</svg>
```

### Comparison of Different Widths

```html
<svg width="400" height="300">
    <defs>
        <marker id="w6" markerWidth="6" markerHeight="6"
                refX="6" refY="3" orient="auto">
            <polygon points="0,0 6,3 0,6" fill="#e74c3c"/>
        </marker>
        <marker id="w10" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
        <marker id="w16" markerWidth="16" markerHeight="16"
                refX="16" refY="8" orient="auto">
            <polygon points="0,0 16,8 0,16" fill="#2ecc71"/>
        </marker>
        <marker id="w24" markerWidth="24" markerHeight="24"
                refX="24" refY="12" orient="auto">
            <polygon points="0,0 24,12 0,24" fill="#f39c12"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="350" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#w6)"/>
    <text x="370" y="55" font-size="10">6</text>

    <line x1="50" y1="110" x2="350" y2="110" stroke="#34495e" stroke-width="2" marker-end="url(#w10)"/>
    <text x="370" y="115" font-size="10">10</text>

    <line x1="50" y1="170" x2="350" y2="170" stroke="#34495e" stroke-width="2" marker-end="url(#w16)"/>
    <text x="370" y="175" font-size="10">16</text>

    <line x1="50" y1="240" x2="350" y2="240" stroke="#34495e" stroke-width="3" marker-end="url(#w24)"/>
    <text x="370" y="245" font-size="10">24</text>
</svg>
```

### Wide Aspect Ratio Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="wide-marker" markerWidth="20" markerHeight="8"
                refX="10" refY="4">
            <rect width="20" height="8" fill="#9b59b6" rx="2"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#wide-marker)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">Wide: 20×8</text>
</svg>
```

### Tall Aspect Ratio Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="tall-marker" markerWidth="8" markerHeight="20"
                refX="4" refY="10">
            <rect width="8" height="20" fill="#e74c3c" rx="2"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#tall-marker)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">Tall: 8×20</text>
</svg>
```

### Circle Markers with Different Widths

```html
<svg width="400" height="250">
    <defs>
        <marker id="circle-8" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#3498db"/>
        </marker>
        <marker id="circle-12" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <circle cx="6" cy="6" r="5" fill="#3498db"/>
        </marker>
        <marker id="circle-16" markerWidth="16" markerHeight="16"
                refX="8" refY="8">
            <circle cx="8" cy="8" r="7" fill="#3498db"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="350" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#circle-8)"/>
    <text x="370" y="55" font-size="10">8</text>

    <line x1="50" y1="125" x2="350" y2="125" stroke="#34495e" stroke-width="2" marker-end="url(#circle-12)"/>
    <text x="370" y="130" font-size="10">12</text>

    <line x1="50" y1="200" x2="350" y2="200" stroke="#34495e" stroke-width="2" marker-end="url(#circle-16)"/>
    <text x="370" y="205" font-size="10">16</text>
</svg>
```

### Diamond Markers with Proportional Sizing

```html
<svg width="400" height="250">
    <defs>
        <marker id="diamond-8" markerWidth="8" markerHeight="8"
                refX="4" refY="4" orient="auto">
            <path d="M 4,0 L 8,4 L 4,8 L 0,4 Z" fill="#f39c12"/>
        </marker>
        <marker id="diamond-12" markerWidth="12" markerHeight="12"
                refX="6" refY="6" orient="auto">
            <path d="M 6,0 L 12,6 L 6,12 L 0,6 Z" fill="#f39c12"/>
        </marker>
        <marker id="diamond-18" markerWidth="18" markerHeight="18"
                refX="9" refY="9" orient="auto">
            <path d="M 9,0 L 18,9 L 9,18 L 0,9 Z" fill="#f39c12"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="350" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-8)"/>
    <line x1="50" y1="125" x2="350" y2="125" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-12)"/>
    <line x1="50" y1="200" x2="350" y2="200" stroke="#34495e" stroke-width="3" marker-end="url(#diamond-18)"/>
</svg>
```

### Complex Icon with Larger Viewport

```html
<svg width="400" height="150">
    <defs>
        <marker id="complex-icon" markerWidth="24" markerHeight="24"
                refX="12" refY="12">
            <circle cx="12" cy="12" r="10" fill="#3498db" stroke="#2980b9" stroke-width="2"/>
            <path d="M 8,12 L 11,15 L 16,10" fill="none" stroke="white" stroke-width="2" stroke-linecap="round"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="3"
          marker-end="url(#complex-icon)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">Complex Icon: markerWidth="24"</text>
</svg>
```

### Decimal Width Value

```html
<svg width="400" height="150">
    <defs>
        <marker id="decimal-width" markerWidth="13.5" markerHeight="13.5"
                refX="13.5" refY="6.75" orient="auto">
            <polygon points="0,0 13.5,6.75 0,13.5" fill="#9b59b6"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#decimal-width)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">markerWidth="13.5"</text>
</svg>
```

### Star Marker with Square Viewport

```html
<svg width="400" height="150">
    <defs>
        <marker id="star" markerWidth="18" markerHeight="18"
                refX="9" refY="9">
            <path d="M 9,2 L 11,7 L 16,7 L 12,10 L 14,16 L 9,12 L 4,16 L 6,10 L 2,7 L 7,7 Z"
                  fill="#f1c40f" stroke="#f39c12" stroke-width="1"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#star)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">Star: markerWidth="18"</text>
</svg>
```

### Multiple Markers with Coordinated Widths

```html
<svg width="400" height="250">
    <defs>
        <marker id="start-dot" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#2ecc71"/>
        </marker>
        <marker id="mid-dot" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#3498db"/>
        </marker>
        <marker id="end-arrow" markerWidth="12" markerHeight="12"
                refX="12" refY="6" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="#e74c3c"/>
        </marker>
    </defs>

    <polyline points="50,125 120,50 200,125 280,50 350,125"
              fill="none" stroke="#34495e" stroke-width="2"
              marker-start="url(#start-dot)"
              marker-mid="url(#mid-dot)"
              marker-end="url(#end-arrow)"/>

    <text x="50" y="150" text-anchor="middle" font-size="9">W:10</text>
    <text x="120" y="40" text-anchor="middle" font-size="9">W:8</text>
    <text x="350" y="150" text-anchor="middle" font-size="9">W:12</text>
</svg>
```

### Gradient Fill with Custom Width

```html
<svg width="400" height="150">
    <defs>
        <linearGradient id="marker-gradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#667eea"/>
            <stop offset="100%" stop-color="#764ba2"/>
        </linearGradient>

        <marker id="gradient-marker" markerWidth="16" markerHeight="16"
                refX="8" refY="8">
            <circle cx="8" cy="8" r="7" fill="url(#marker-gradient)"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="3"
          marker-end="url(#gradient-marker)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">Gradient: markerWidth="16"</text>
</svg>
```

### Target Marker with Concentric Circles

```html
<svg width="400" height="150">
    <defs>
        <marker id="target" markerWidth="20" markerHeight="20"
                refX="10" refY="10">
            <circle cx="10" cy="10" r="9" fill="white" stroke="#e74c3c" stroke-width="2"/>
            <circle cx="10" cy="10" r="6" fill="white" stroke="#e74c3c" stroke-width="2"/>
            <circle cx="10" cy="10" r="3" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#target)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">Target: markerWidth="20"</text>
</svg>
```

### Rectangular Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="rect-marker" markerWidth="18" markerHeight="12"
                refX="18" refY="6" orient="auto">
            <rect width="18" height="12" fill="#e74c3c" rx="2"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#rect-marker)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">Rectangle: markerWidth="18" × markerHeight="12"</text>
</svg>
```

---

## See Also

- [markerHeight](/reference/svgattributes/markerHeight.html) - Marker viewport height
- [marker element](/reference/svgtags/marker.html) - Marker definition element
- [refX](/reference/svgattributes/refX.html) - Marker reference X position
- [refY](/reference/svgattributes/refY.html) - Marker reference Y position
- [markerUnits](/reference/svgattributes/markerUnits.html) - Marker coordinate system
- [orient](/reference/svgattributes/orient.html) - Marker orientation
- [marker-start](/reference/svgattributes/marker-start.html) - Marker at path start
- [marker-mid](/reference/svgattributes/marker-mid.html) - Marker at middle vertices
- [marker-end](/reference/svgattributes/marker-end.html) - Marker at path end
- [viewBox](/reference/svgattributes/viewBox.html) - SVG viewport and coordinate system
- [Data Binding](/reference/binding/) - Data binding and expressions

---
