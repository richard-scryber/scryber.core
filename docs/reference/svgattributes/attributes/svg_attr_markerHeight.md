---
layout: default
title: markerHeight
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @markerHeight : The Marker Viewport Height Attribute

The `markerHeight` attribute defines the height of the marker viewport - the vertical extent of the coordinate system in which the marker content is rendered. This works in conjunction with `markerWidth` to establish the complete marker coordinate space.

## Usage

The `markerHeight` attribute is used to:
- Define the vertical extent of the marker's coordinate system
- Control the aspect ratio of marker content
- Set up proportional marker dimensions
- Support responsive marker sizing
- Enable data-driven marker height
- Coordinate with markerWidth for proper marker proportions

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
| `<length>` | Positive number (no units) | `markerHeight="10"` |
| Default | `3` if not specified | (default) |

### Valid Values

```html
<!-- Common sizes -->
markerHeight="5"    <!-- Short marker -->
markerHeight="10"   <!-- Medium marker -->
markerHeight="20"   <!-- Tall marker -->

<!-- Decimal values -->
markerHeight="12.5"

<!-- Must be positive -->
markerHeight="0"    <!-- Invalid: no viewport -->
markerHeight="-5"   <!-- Invalid: negative not allowed -->
```

---

## Supported Elements

The `markerHeight` attribute is supported on:

- **[&lt;marker&gt;](/reference/svgtags/marker.html)** - Marker definition element

---

## Data Binding

### Dynamic Marker Height

Adjust marker height based on data:

```html
<!-- Model: { markerSize: 14, visualScale: 1.2 } -->
<svg width="400" height="200">
    <defs>
        <marker id="dynamic-marker"
                markerWidth="{{model.markerSize}}"
                markerHeight="{{model.markerSize * model.visualScale}}"
                refX="{{model.markerSize / 2}}"
                refY="{{model.markerSize * model.visualScale / 2}}">
            <circle cx="{{model.markerSize / 2}}"
                    cy="{{model.markerSize * model.visualScale / 2}}"
                    r="{{Math.min(model.markerSize, model.markerSize * model.visualScale) / 2 - 1}}"
                    fill="#3498db"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#dynamic-marker)"/>
</svg>
```

### Proportional Sizing

Scale both dimensions together:

```html
<!-- Model: { scale: 1.5 } -->
<svg width="400" height="200">
    <defs>
        <marker id="scaled-marker"
                markerWidth="{{10 * model.scale}}"
                markerHeight="{{10 * model.scale}}"
                refX="{{10 * model.scale}}"
                refY="{{5 * model.scale}}"
                orient="auto">
            <polygon points="0,0 {{10 * model.scale}},{{5 * model.scale}} 0,{{10 * model.scale}}"
                     fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#scaled-marker)"/>
</svg>
```

### Variable Aspect Ratios

```html
<!-- Model: {
    markers: [
        { width: 12, height: 8, color: '#e74c3c' },
        { width: 10, height: 10, color: '#3498db' },
        { width: 8, height: 14, color: '#2ecc71' }
    ]
} -->
<svg width="250" height="250">
    <defs>
        <template data-bind="{{model.markers}}">
            <marker id="marker-{{$index}}"
                    markerWidth="{{.width}}"
                    markerHeight="{{.height}}"
                    refX="{{.width / 2}}"
                    refY="{{.height / 2}}">
                <rect width="{{.width}}" height="{{.height}}"
                      fill="{{.color}}" rx="1"/>
            </marker>
        </template>
    </defs>

    <template data-bind="{{model.markers}}">
        <line x1="50" y1="{{50 + $index * 80}}"
              x2="200" y2="{{50 + $index * 80}}"
              stroke="#2c3e50" stroke-width="2"
              marker-end="url(#marker-{{$index}})"/>
    </template>
</svg>
```

---

## Notes

### Viewport Coordinate System

The `markerHeight` defines the **vertical coordinate space**:
- It establishes the vertical range of user units available
- Marker content is drawn within this coordinate system (0 to markerHeight)
- The actual visual size depends on marker content and `markerUnits`

```html
<!-- markerHeight=10 means y-coordinates from 0-10 are available -->
<marker id="example" markerWidth="10" markerHeight="10">
    <!-- This circle uses y-coordinates 0-10 -->
    <circle cx="5" cy="5" r="4"/>
</marker>
```

### Aspect Ratio Control

`markerWidth` and `markerHeight` together control the aspect ratio:

```html
<!-- Square (1:1) -->
<marker markerWidth="10" markerHeight="10">

<!-- Wide (2:1) -->
<marker markerWidth="20" markerHeight="10">

<!-- Tall (1:2) -->
<marker markerWidth="10" markerHeight="20">
```

### Default Value

If `markerHeight` is not specified, the default is **3 units**:

```html
<!-- These are equivalent -->
<marker id="m1">
<marker id="m2" markerHeight="3">
```

### Reference Point Vertical Positioning

The `refY` value should coordinate with `markerHeight`:

```html
<!-- Vertically centered -->
<marker markerWidth="10" markerHeight="10"
        refX="5" refY="5">
    <!-- refY = markerHeight / 2 -->
</marker>

<!-- Top-aligned -->
<marker markerWidth="10" markerHeight="10"
        refX="5" refY="0">
    <!-- refY = 0 for top edge -->
</marker>

<!-- Bottom-aligned -->
<marker markerWidth="10" markerHeight="10"
        refX="5" refY="10">
    <!-- refY = markerHeight for bottom edge -->
</marker>
```

### Scaling Behavior

With `markerUnits="strokeWidth"`, the effective height scales:

```html
<marker markerHeight="3" markerUnits="strokeWidth">
    <!-- stroke-width="2" → effective height = 6 -->
    <!-- stroke-width="4" → effective height = 12 -->
</marker>
```

### Common Patterns

**Square marker (arrows, dots):**
```html
<marker markerWidth="10" markerHeight="10"
        refX="5" refY="5">
    <circle cx="5" cy="5" r="4"/>
</marker>
```

**Wide marker (horizontal emphasis):**
```html
<marker markerWidth="16" markerHeight="8"
        refX="8" refY="4">
    <rect width="16" height="8" rx="2"/>
</marker>
```

**Tall marker (vertical emphasis):**
```html
<marker markerWidth="8" markerHeight="16"
        refX="4" refY="8">
    <rect width="8" height="16" rx="2"/>
</marker>
```

---

## Examples

### Square Markers (Equal Width and Height)

```html
<svg width="400" height="200">
    <defs>
        <marker id="square-8" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <rect width="8" height="8" fill="#e74c3c"/>
        </marker>
        <marker id="square-12" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <rect width="12" height="12" fill="#3498db"/>
        </marker>
        <marker id="square-16" markerWidth="16" markerHeight="16"
                refX="8" refY="8">
            <rect width="16" height="16" fill="#2ecc71"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="350" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#square-8)"/>
    <line x1="50" y1="100" x2="350" y2="100" stroke="#34495e" stroke-width="2" marker-end="url(#square-12)"/>
    <line x1="50" y1="150" x2="350" y2="150" stroke="#34495e" stroke-width="2" marker-end="url(#square-16)"/>
</svg>
```

### Wide Markers (Width > Height)

```html
<svg width="400" height="200">
    <defs>
        <marker id="wide-1" markerWidth="16" markerHeight="8"
                refX="16" refY="4" orient="auto">
            <polygon points="0,0 16,4 0,8" fill="#9b59b6"/>
        </marker>
        <marker id="wide-2" markerWidth="20" markerHeight="10"
                refX="20" refY="5" orient="auto">
            <polygon points="0,0 20,5 0,10" fill="#9b59b6"/>
        </marker>
    </defs>

    <line x1="50" y1="70" x2="350" y2="70" stroke="#34495e" stroke-width="2" marker-end="url(#wide-1)"/>
    <text x="200" y="50" text-anchor="middle" font-size="10">16×8 (2:1 ratio)</text>

    <line x1="50" y1="140" x2="350" y2="140" stroke="#34495e" stroke-width="2" marker-end="url(#wide-2)"/>
    <text x="200" y="120" text-anchor="middle" font-size="10">20×10 (2:1 ratio)</text>
</svg>
```

### Tall Markers (Height > Width)

```html
<svg width="400" height="250">
    <defs>
        <marker id="tall-1" markerWidth="8" markerHeight="16"
                refX="4" refY="8">
            <rect width="8" height="16" fill="#e74c3c" rx="2"/>
        </marker>
        <marker id="tall-2" markerWidth="10" markerHeight="20"
                refX="5" refY="10">
            <rect width="10" height="20" fill="#e74c3c" rx="2"/>
        </marker>
    </defs>

    <line x1="50" y1="80" x2="350" y2="80" stroke="#34495e" stroke-width="2" marker-end="url(#tall-1)"/>
    <text x="200" y="50" text-anchor="middle" font-size="10">8×16 (1:2 ratio)</text>

    <line x1="50" y1="170" x2="350" y2="170" stroke="#34495e" stroke-width="2" marker-end="url(#tall-2)"/>
    <text x="200" y="140" text-anchor="middle" font-size="10">10×20 (1:2 ratio)</text>
</svg>
```

### Circular Markers with Different Heights

```html
<svg width="400" height="250">
    <defs>
        <marker id="circ-6" markerWidth="6" markerHeight="6"
                refX="3" refY="3">
            <circle cx="3" cy="3" r="2.5" fill="#3498db"/>
        </marker>
        <marker id="circ-10" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#3498db"/>
        </marker>
        <marker id="circ-14" markerWidth="14" markerHeight="14"
                refX="7" refY="7">
            <circle cx="7" cy="7" r="6" fill="#3498db"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="350" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#circ-6)"/>
    <text x="370" y="55" font-size="10">H:6</text>

    <line x1="50" y1="125" x2="350" y2="125" stroke="#34495e" stroke-width="2" marker-end="url(#circ-10)"/>
    <text x="370" y="130" font-size="10">H:10</text>

    <line x1="50" y1="200" x2="350" y2="200" stroke="#34495e" stroke-width="2" marker-end="url(#circ-14)"/>
    <text x="370" y="205" font-size="10">H:14</text>
</svg>
```

### Elliptical Markers (Different Aspect Ratios)

```html
<svg width="400" height="250">
    <defs>
        <marker id="ellipse-1" markerWidth="12" markerHeight="8"
                refX="6" refY="4">
            <ellipse cx="6" cy="4" rx="5" ry="3" fill="#f39c12"/>
        </marker>
        <marker id="ellipse-2" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <ellipse cx="5" cy="5" rx="4" ry="4" fill="#f39c12"/>
        </marker>
        <marker id="ellipse-3" markerWidth="8" markerHeight="12"
                refX="4" refY="6">
            <ellipse cx="4" cy="6" rx="3" ry="5" fill="#f39c12"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="350" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#ellipse-1)"/>
    <text x="370" y="55" font-size="9">12×8</text>

    <line x1="50" y1="125" x2="350" y2="125" stroke="#34495e" stroke-width="2" marker-end="url(#ellipse-2)"/>
    <text x="370" y="130" font-size="9">10×10</text>

    <line x1="50" y1="200" x2="350" y2="200" stroke="#34495e" stroke-width="2" marker-end="url(#ellipse-3)"/>
    <text x="370" y="205" font-size="9">8×12</text>
</svg>
```

### Arrow Markers with Varying Heights

```html
<svg width="400" height="300">
    <defs>
        <marker id="arrow-h6" markerWidth="10" markerHeight="6"
                refX="10" refY="3" orient="auto">
            <polygon points="0,0 10,3 0,6" fill="#e74c3c"/>
        </marker>
        <marker id="arrow-h10" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
        <marker id="arrow-h14" markerWidth="10" markerHeight="14"
                refX="10" refY="7" orient="auto">
            <polygon points="0,0 10,7 0,14" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="60" x2="350" y2="60" stroke="#34495e" stroke-width="2" marker-end="url(#arrow-h6)"/>
    <text x="200" y="40" text-anchor="middle" font-size="10">Height: 6 (narrow)</text>

    <line x1="50" y1="150" x2="350" y2="150" stroke="#34495e" stroke-width="2" marker-end="url(#arrow-h10)"/>
    <text x="200" y="130" text-anchor="middle" font-size="10">Height: 10 (balanced)</text>

    <line x1="50" y1="250" x2="350" y2="250" stroke="#34495e" stroke-width="3" marker-end="url(#arrow-h14)"/>
    <text x="200" y="230" text-anchor="middle" font-size="10">Height: 14 (wide)</text>
</svg>
```

### Diamond Markers with Different Heights

```html
<svg width="400" height="250">
    <defs>
        <marker id="diamond-h8" markerWidth="8" markerHeight="8"
                refX="4" refY="4" orient="auto">
            <path d="M 4,0 L 8,4 L 4,8 L 0,4 Z" fill="#2ecc71"/>
        </marker>
        <marker id="diamond-h12" markerWidth="8" markerHeight="12"
                refX="4" refY="6" orient="auto">
            <path d="M 4,0 L 8,6 L 4,12 L 0,6 Z" fill="#2ecc71"/>
        </marker>
        <marker id="diamond-h16" markerWidth="8" markerHeight="16"
                refX="4" refY="8" orient="auto">
            <path d="M 4,0 L 8,8 L 4,16 L 0,8 Z" fill="#2ecc71"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="350" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-h8)"/>
    <line x1="50" y1="125" x2="350" y2="125" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-h12)"/>
    <line x1="50" y1="205" x2="350" y2="205" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-h16)"/>
</svg>
```

### Star Markers with Proportional Heights

```html
<svg width="400" height="250">
    <defs>
        <marker id="star-h12" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <path d="M 6,1 L 7.5,5 L 11,5 L 8.5,7.5 L 9.5,11 L 6,8.5 L 2.5,11 L 3.5,7.5 L 1,5 L 4.5,5 Z"
                  fill="#f1c40f"/>
        </marker>
        <marker id="star-h16" markerWidth="16" markerHeight="16"
                refX="8" refY="8">
            <path d="M 8,1 L 10,6.5 L 15,6.5 L 11,10 L 12.5,15 L 8,11.5 L 3.5,15 L 5,10 L 1,6.5 L 6,6.5 Z"
                  fill="#f1c40f"/>
        </marker>
    </defs>

    <line x1="50" y1="70" x2="350" y2="70" stroke="#34495e" stroke-width="2" marker-end="url(#star-h12)"/>
    <text x="200" y="50" text-anchor="middle" font-size="10">Star: Height 12</text>

    <line x1="50" y1="170" x2="350" y2="170" stroke="#34495e" stroke-width="2" marker-end="url(#star-h16)"/>
    <text x="200" y="150" text-anchor="middle" font-size="10">Star: Height 16</text>
</svg>
```

### Complex Icon with Custom Height

```html
<svg width="400" height="180">
    <defs>
        <marker id="complex" markerWidth="20" markerHeight="24"
                refX="10" refY="12">
            <rect x="2" y="2" width="16" height="20" rx="3" fill="#3498db" stroke="#2980b9" stroke-width="2"/>
            <path d="M 6,12 L 9,15 L 14,10" fill="none" stroke="white" stroke-width="2" stroke-linecap="round"/>
        </marker>
    </defs>

    <line x1="50" y1="90" x2="350" y2="90"
          stroke="#34495e" stroke-width="3"
          marker-end="url(#complex)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">Complex Icon: 20×24</text>
</svg>
```

### Varying Heights with Same Width

```html
<svg width="400" height="300">
    <defs>
        <marker id="rect-h6" markerWidth="12" markerHeight="6"
                refX="12" refY="3" orient="auto">
            <rect width="12" height="6" fill="#9b59b6" rx="1"/>
        </marker>
        <marker id="rect-h10" markerWidth="12" markerHeight="10"
                refX="12" refY="5" orient="auto">
            <rect width="12" height="10" fill="#9b59b6" rx="1"/>
        </marker>
        <marker id="rect-h16" markerWidth="12" markerHeight="16"
                refX="12" refY="8" orient="auto">
            <rect width="12" height="16" fill="#9b59b6" rx="1"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="350" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#rect-h6)"/>
    <text x="370" y="55" font-size="9">H:6</text>

    <line x1="50" y1="125" x2="350" y2="125" stroke="#34495e" stroke-width="2" marker-end="url(#rect-h10)"/>
    <text x="370" y="130" font-size="9">H:10</text>

    <line x1="50" y1="210" x2="350" y2="210" stroke="#34495e" stroke-width="2" marker-end="url(#rect-h16)"/>
    <text x="370" y="215" font-size="9">H:16</text>
</svg>
```

### Decimal Height Values

```html
<svg width="400" height="150">
    <defs>
        <marker id="decimal-h" markerWidth="12" markerHeight="15.5"
                refX="12" refY="7.75" orient="auto">
            <polygon points="0,0 12,7.75 0,15.5" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#decimal-h)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">markerHeight="15.5"</text>
</svg>
```

### Gradient Fill with Custom Height

```html
<svg width="400" height="180">
    <defs>
        <linearGradient id="grad-v" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#667eea"/>
            <stop offset="100%" stop-color="#764ba2"/>
        </linearGradient>

        <marker id="gradient-marker" markerWidth="14" markerHeight="18"
                refX="7" refY="9">
            <rect x="1" y="1" width="12" height="16" rx="2"
                  fill="url(#grad-v)" stroke="#5a67d8" stroke-width="1"/>
        </marker>
    </defs>

    <line x1="50" y1="90" x2="350" y2="90"
          stroke="#34495e" stroke-width="3"
          marker-end="url(#gradient-marker)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">Gradient: 14×18</text>
</svg>
```

### Target with Custom Height Proportions

```html
<svg width="400" height="180">
    <defs>
        <marker id="target-tall" markerWidth="16" markerHeight="20"
                refX="8" refY="10">
            <ellipse cx="8" cy="10" rx="7" ry="9" fill="white" stroke="#e74c3c" stroke-width="2"/>
            <ellipse cx="8" cy="10" rx="4" ry="6" fill="white" stroke="#e74c3c" stroke-width="2"/>
            <ellipse cx="8" cy="10" rx="2" ry="3" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="90" x2="350" y2="90"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#target-tall)"/>
    <text x="200" y="50" text-anchor="middle" font-size="12">Tall Target: 16×20</text>
</svg>
```

---

## See Also

- [markerWidth](/reference/svgattributes/markerWidth.html) - Marker viewport width
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
