---
layout: default
title: refX
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @refX : The Marker Reference X Position Attribute

The `refX` attribute defines the horizontal reference point of a marker - the X coordinate within the marker's coordinate system that aligns with the path vertex. This controls where the marker is positioned relative to the path point.

## Usage

The `refX` attribute is used to:
- Define the horizontal anchor point of the marker
- Control how markers align with path vertices
- Position marker content relative to the path
- Create properly aligned arrowheads
- Support data-driven marker positioning
- Enable precise marker placement control

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
        <!-- Arrow with tip at path point -->
        <marker id="arrow" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
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
| `<length>` | Number in marker units | `refX="5"` |
| Default | `0` if not specified | (default) |

### Common Values

```html
<!-- Left edge (0) -->
refX="0"

<!-- Center (markerWidth / 2) -->
refX="5"    <!-- for markerWidth="10" -->

<!-- Right edge (markerWidth) -->
refX="10"   <!-- for markerWidth="10" -->

<!-- Decimal values -->
refX="7.5"

<!-- Negative values (outside marker) -->
refX="-2"
```

---

## Supported Elements

The `refX` attribute is supported on:

- **[&lt;marker&gt;](/reference/svgtags/marker.html)** - Marker definition element

---

## Data Binding

### Dynamic Reference Point

Adjust reference position based on marker size:

```html
<!-- Model: { markerSize: 12, alignCenter: true } -->
<svg width="400" height="200">
    <defs>
        <marker id="dynamic-ref"
                markerWidth="{{model.markerSize}}"
                markerHeight="{{model.markerSize}}"
                refX="{{model.alignCenter ? model.markerSize / 2 : model.markerSize}}"
                refY="{{model.markerSize / 2}}"
                orient="auto">
            <polygon points="0,0 {{model.markerSize}},{{model.markerSize / 2}} 0,{{model.markerSize}}"
                     fill="#3498db"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#dynamic-ref)"/>
</svg>
```

### Conditional Alignment

Change alignment based on marker type:

```html
<!-- Model: { markerType: 'arrow', arrowTipAlign: true } -->
<svg width="400" height="200">
    <defs>
        <marker id="aligned-marker"
                markerWidth="10" markerHeight="10"
                refX="{{model.arrowTipAlign ? 10 : 5}}"
                refY="5"
                orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#aligned-marker)"/>
</svg>
```

### Data-Driven Marker Positioning

```html
<!-- Model: {
    markers: [
        { size: 10, alignType: 'center', color: '#e74c3c' },
        { size: 12, alignType: 'tip', color: '#3498db' },
        { size: 14, alignType: 'base', color: '#2ecc71' }
    ],
    getRefX: function(m) {
        if (m.alignType === 'center') return m.size / 2;
        if (m.alignType === 'tip') return m.size;
        return 0;
    }
} -->
<svg width="300" height="250">
    <defs>
        <template data-bind="{{model.markers}}">
            <marker id="marker-{{$index}}"
                    markerWidth="{{.size}}" markerHeight="{{.size}}"
                    refX="{{model.getRefX(.)}}"
                    refY="{{.size / 2}}"
                    orient="auto">
                <polygon points="0,0 {{.size}},{{.size / 2}} 0,{{.size}}"
                         fill="{{.color}}"/>
            </marker>
        </template>
    </defs>

    <template data-bind="{{model.markers}}">
        <line x1="50" y1="{{50 + $index * 80}}"
              x2="250" y2="{{50 + $index * 80}}"
              stroke="#2c3e50" stroke-width="2"
              marker-end="url(#marker-{{$index}})"/>
    </template>
</svg>
```

---

## Notes

### Reference Point Concept

The `refX` coordinate within the marker aligns with the path vertex:

```html
<!-- Arrow tip at path point -->
<marker markerWidth="10" markerHeight="10" refX="10" refY="5">
    <!-- Point (10, 5) in marker aligns with path vertex -->
    <polygon points="0,0 10,5 0,10"/>
</marker>

<!-- Arrow center at path point -->
<marker markerWidth="10" markerHeight="10" refX="5" refY="5">
    <!-- Point (5, 5) in marker aligns with path vertex -->
    <polygon points="0,0 10,5 0,10"/>
</marker>
```

### Common Alignment Patterns

**Tip alignment (arrows):**
```html
<!-- Arrow tip exactly at path end -->
<marker markerWidth="10" markerHeight="10"
        refX="10" refY="5" orient="auto">
    <polygon points="0,0 10,5 0,10"/>
</marker>
```

**Center alignment (dots, circles):**
```html
<!-- Circle center at path point -->
<marker markerWidth="10" markerHeight="10"
        refX="5" refY="5">
    <circle cx="5" cy="5" r="4"/>
</marker>
```

**Base alignment:**
```html
<!-- Base at path point, marker extends outward -->
<marker markerWidth="10" markerHeight="10"
        refX="0" refY="5" orient="auto">
    <polygon points="0,0 10,5 0,10"/>
</marker>
```

### Relationship with markerWidth

`refX` should be within the range `0` to `markerWidth`:

```html
<!-- For markerWidth="10" -->
refX="0"    <!-- Left edge -->
refX="5"    <!-- Center -->
refX="10"   <!-- Right edge -->
```

Values outside this range position the reference point outside the marker viewport (valid but uncommon).

### Default Value

If `refX` is not specified, the default is **0** (left edge):

```html
<!-- These are equivalent -->
<marker id="m1" markerWidth="10" markerHeight="10">
<marker id="m2" markerWidth="10" markerHeight="10" refX="0">
```

### Coordinate System

`refX` uses the marker's own coordinate system:
- Independent of `markerUnits`
- Always relative to marker viewport (0 to markerWidth)
- Not affected by stroke width or scaling

### Visual Impact

Different `refX` values change where the marker appears:

```html
<!-- refX="0": marker starts at path point -->
<marker refX="0" refY="5">
    <!-- Marker extends forward from point -->
</marker>

<!-- refX="5": marker centered on path point -->
<marker refX="5" refY="5">
    <!-- Marker straddles the point -->
</marker>

<!-- refX="10": marker ends at path point -->
<marker refX="10" refY="5">
    <!-- Marker extends backward to point -->
</marker>
```

### Orient Interaction

With `orient="auto"`, the marker rotates, but `refX` always refers to the marker's local coordinate system before rotation.

### Precision

Use decimal values for precise positioning:

```html
<!-- Fine-tuned alignment -->
<marker refX="9.5" refY="5.25">
```

---

## Examples

### Arrow with Tip Alignment (refX = markerWidth)

```html
<svg width="400" height="150">
    <defs>
        <marker id="tip-aligned" markerWidth="12" markerHeight="12"
                refX="12" refY="6" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#tip-aligned)"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">refX="12" (tip at path point)</text>
</svg>
```

### Arrow with Center Alignment (refX = markerWidth / 2)

```html
<svg width="400" height="150">
    <defs>
        <marker id="center-aligned" markerWidth="12" markerHeight="12"
                refX="6" refY="6" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="#3498db"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#center-aligned)"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">refX="6" (center at path point)</text>
</svg>
```

### Arrow with Base Alignment (refX = 0)

```html
<svg width="400" height="150">
    <defs>
        <marker id="base-aligned" markerWidth="12" markerHeight="12"
                refX="0" refY="6" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="#2ecc71"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#base-aligned)"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">refX="0" (base at path point)</text>
</svg>
```

### Comparison of Different refX Values

```html
<svg width="400" height="300">
    <defs>
        <marker id="ref-0" markerWidth="16" markerHeight="10"
                refX="0" refY="5" orient="auto">
            <polygon points="0,0 16,5 0,10" fill="#e74c3c"/>
            <circle cx="0" cy="5" r="1" fill="white"/>
        </marker>
        <marker id="ref-8" markerWidth="16" markerHeight="10"
                refX="8" refY="5" orient="auto">
            <polygon points="0,0 16,5 0,10" fill="#3498db"/>
            <circle cx="8" cy="5" r="1" fill="white"/>
        </marker>
        <marker id="ref-16" markerWidth="16" markerHeight="10"
                refX="16" refY="5" orient="auto">
            <polygon points="0,0 16,5 0,10" fill="#2ecc71"/>
            <circle cx="16" cy="5" r="1" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="60" x2="300" y2="60" stroke="#34495e" stroke-width="2" marker-end="url(#ref-0)"/>
    <circle cx="300" cy="60" r="3" fill="orange"/>
    <text x="320" y="65" font-size="10">refX="0"</text>

    <line x1="50" y1="150" x2="300" y2="150" stroke="#34495e" stroke-width="2" marker-end="url(#ref-8)"/>
    <circle cx="300" cy="150" r="3" fill="orange"/>
    <text x="320" y="155" font-size="10">refX="8"</text>

    <line x1="50" y1="240" x2="300" y2="240" stroke="#34495e" stroke-width="2" marker-end="url(#ref-16)"/>
    <circle cx="300" cy="240" r="3" fill="orange"/>
    <text x="320" y="245" font-size="10">refX="16"</text>
</svg>
```

### Circle Marker with Center refX

```html
<svg width="400" height="150">
    <defs>
        <marker id="centered-circle" markerWidth="14" markerHeight="14"
                refX="7" refY="7">
            <circle cx="7" cy="7" r="6" fill="#9b59b6" stroke="#8e44ad" stroke-width="1"/>
            <circle cx="7" cy="7" r="1" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#centered-circle)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Centered circle: refX="7" (half of 14)</text>
</svg>
```

### Diamond with Different refX Alignments

```html
<svg width="400" height="250">
    <defs>
        <marker id="diamond-left" markerWidth="12" markerHeight="12"
                refX="0" refY="6" orient="auto">
            <path d="M 0,6 L 6,0 L 12,6 L 6,12 Z" fill="#f39c12"/>
            <circle cx="0" cy="6" r="1" fill="white"/>
        </marker>
        <marker id="diamond-center" markerWidth="12" markerHeight="12"
                refX="6" refY="6" orient="auto">
            <path d="M 0,6 L 6,0 L 12,6 L 6,12 Z" fill="#f39c12"/>
            <circle cx="6" cy="6" r="1" fill="white"/>
        </marker>
        <marker id="diamond-right" markerWidth="12" markerHeight="12"
                refX="12" refY="6" orient="auto">
            <path d="M 0,6 L 6,0 L 12,6 L 6,12 Z" fill="#f39c12"/>
            <circle cx="12" cy="6" r="1" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="300" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-left)"/>
    <circle cx="300" cy="50" r="3" fill="orange"/>
    <text x="320" y="55" font-size="9">refX="0"</text>

    <line x1="50" y1="125" x2="300" y2="125" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-center)"/>
    <circle cx="300" cy="125" r="3" fill="orange"/>
    <text x="320" y="130" font-size="9">refX="6"</text>

    <line x1="50" y1="200" x2="300" y2="200" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-right)"/>
    <circle cx="300" cy="200" r="3" fill="orange"/>
    <text x="320" y="205" font-size="9">refX="12"</text>
</svg>
```

### Square Marker with Corner Reference

```html
<svg width="400" height="150">
    <defs>
        <marker id="square-corner" markerWidth="12" markerHeight="12"
                refX="12" refY="12">
            <rect width="12" height="12" fill="#e74c3c" stroke="#c0392b" stroke-width="1"/>
            <circle cx="12" cy="12" r="1.5" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#square-corner)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Square corner at path point: refX="12", refY="12"</text>
</svg>
```

### Star with Center Reference

```html
<svg width="400" height="150">
    <defs>
        <marker id="star-center" markerWidth="20" markerHeight="20"
                refX="10" refY="10">
            <path d="M 10,2 L 12,8 L 18,8 L 13,12 L 15,18 L 10,14 L 5,18 L 7,12 L 2,8 L 8,8 Z"
                  fill="#f1c40f" stroke="#f39c12" stroke-width="1"/>
            <circle cx="10" cy="10" r="1" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#star-center)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Star centered: refX="10", refY="10"</text>
</svg>
```

### Custom Icon with Precise Positioning

```html
<svg width="400" height="150">
    <defs>
        <marker id="custom-icon" markerWidth="24" markerHeight="24"
                refX="12" refY="12">
            <rect x="2" y="2" width="20" height="20" rx="4" fill="#3498db" stroke="#2980b9" stroke-width="2"/>
            <path d="M 8,12 L 11,15 L 16,10" fill="none" stroke="white" stroke-width="2" stroke-linecap="round"/>
            <circle cx="12" cy="12" r="1.5" fill="yellow"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="3"
          marker-end="url(#custom-icon)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Complex icon centered: refX="12"</text>
</svg>
```

### Decimal refX Value

```html
<svg width="400" height="150">
    <defs>
        <marker id="decimal-ref" markerWidth="15" markerHeight="10"
                refX="13.5" refY="5" orient="auto">
            <polygon points="0,0 15,5 0,10" fill="#9b59b6"/>
            <circle cx="13.5" cy="5" r="1" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#decimal-ref)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Precise positioning: refX="13.5"</text>
</svg>
```

### Offset Reference (negative refX)

```html
<svg width="400" height="150">
    <defs>
        <marker id="offset-ref" markerWidth="12" markerHeight="12"
                refX="-3" refY="6" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="#e74c3c"/>
            <circle cx="-3" cy="6" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#offset-ref)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Negative refX="-3" (offset from marker)</text>
</svg>
```

### Multi-Marker Path with Different refX

```html
<svg width="400" height="200">
    <defs>
        <marker id="start-ref" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#2ecc71"/>
            <circle cx="5" cy="5" r="1" fill="white"/>
        </marker>
        <marker id="mid-ref" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#3498db"/>
        </marker>
        <marker id="end-ref" markerWidth="12" markerHeight="12"
                refX="12" refY="6" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="#e74c3c"/>
            <circle cx="12" cy="6" r="1" fill="white"/>
        </marker>
    </defs>

    <polyline points="50,150 120,50 200,120 280,60 350,150"
              fill="none" stroke="#34495e" stroke-width="2"
              marker-start="url(#start-ref)"
              marker-mid="url(#mid-ref)"
              marker-end="url(#end-ref)"/>
</svg>
```

### Wide Marker with Different Alignments

```html
<svg width="400" height="250">
    <defs>
        <marker id="wide-left" markerWidth="24" markerHeight="12"
                refX="0" refY="6" orient="auto">
            <rect width="24" height="12" rx="2" fill="#3498db"/>
            <circle cx="0" cy="6" r="1.5" fill="white"/>
        </marker>
        <marker id="wide-center" markerWidth="24" markerHeight="12"
                refX="12" refY="6" orient="auto">
            <rect width="24" height="12" rx="2" fill="#3498db"/>
            <circle cx="12" cy="6" r="1.5" fill="white"/>
        </marker>
        <marker id="wide-right" markerWidth="24" markerHeight="12"
                refX="24" refY="6" orient="auto">
            <rect width="24" height="12" rx="2" fill="#3498db"/>
            <circle cx="24" cy="6" r="1.5" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="300" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#wide-left)"/>
    <circle cx="300" cy="50" r="3" fill="orange"/>

    <line x1="50" y1="125" x2="300" y2="125" stroke="#34495e" stroke-width="2" marker-end="url(#wide-center)"/>
    <circle cx="300" cy="125" r="3" fill="orange"/>

    <line x1="50" y1="200" x2="300" y2="200" stroke="#34495e" stroke-width="2" marker-end="url(#wide-right)"/>
    <circle cx="300" cy="200" r="3" fill="orange"/>
</svg>
```

### Gradient Marker with Centered refX

```html
<svg width="400" height="150">
    <defs>
        <linearGradient id="grad-h" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#667eea"/>
            <stop offset="100%" stop-color="#764ba2"/>
        </linearGradient>

        <marker id="gradient-centered" markerWidth="18" markerHeight="18"
                refX="9" refY="9">
            <circle cx="9" cy="9" r="8" fill="url(#grad-h)" stroke="#5a67d8" stroke-width="1"/>
            <circle cx="9" cy="9" r="1.5" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="3"
          marker-end="url(#gradient-centered)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Gradient marker centered: refX="9"</text>
</svg>
```

### Complex Shape with Custom refX

```html
<svg width="400" height="150">
    <defs>
        <marker id="complex-ref" markerWidth="20" markerHeight="16"
                refX="20" refY="8" orient="auto">
            <path d="M 0,8 L 10,0 L 20,8 L 10,16 Z" fill="#e74c3c" stroke="#c0392b" stroke-width="1"/>
            <circle cx="20" cy="8" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#complex-ref)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Complex shape: refX="20" (right tip)</text>
</svg>
```

---

## See Also

- [refY](/reference/svgattributes/refY.html) - Marker reference Y position
- [marker element](/reference/svgtags/marker.html) - Marker definition element
- [markerWidth](/reference/svgattributes/markerWidth.html) - Marker viewport width
- [markerHeight](/reference/svgattributes/markerHeight.html) - Marker viewport height
- [orient](/reference/svgattributes/orient.html) - Marker orientation
- [markerUnits](/reference/svgattributes/markerUnits.html) - Marker coordinate system
- [marker-start](/reference/svgattributes/marker-start.html) - Marker at path start
- [marker-mid](/reference/svgattributes/marker-mid.html) - Marker at middle vertices
- [marker-end](/reference/svgattributes/marker-end.html) - Marker at path end
- [Data Binding](/reference/binding/) - Data binding and expressions

---
