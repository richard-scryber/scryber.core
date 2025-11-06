---
layout: default
title: refY
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @refY : The Marker Reference Y Position Attribute

The `refY` attribute defines the vertical reference point of a marker - the Y coordinate within the marker's coordinate system that aligns with the path vertex. This works with `refX` to control complete marker positioning relative to the path.

## Usage

The `refY` attribute is used to:
- Define the vertical anchor point of the marker
- Control vertical alignment with path vertices
- Position marker content vertically relative to the path
- Create properly aligned markers
- Support data-driven vertical positioning
- Enable precise marker alignment control

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
        <!-- Arrow with vertical center at path point -->
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
| `<length>` | Number in marker units | `refY="5"` |
| Default | `0` if not specified | (default) |

### Common Values

```html
<!-- Top edge (0) -->
refY="0"

<!-- Center (markerHeight / 2) -->
refY="5"    <!-- for markerHeight="10" -->

<!-- Bottom edge (markerHeight) -->
refY="10"   <!-- for markerHeight="10" -->

<!-- Decimal values -->
refY="7.5"

<!-- Negative values (outside marker) -->
refY="-2"
```

---

## Supported Elements

The `refY` attribute is supported on:

- **[&lt;marker&gt;](/reference/svgtags/marker.html)** - Marker definition element

---

## Data Binding

### Dynamic Vertical Positioning

Adjust vertical reference based on marker size:

```html
<!-- Model: { markerSize: 14, verticalAlign: 'center' } -->
<svg width="400" height="200">
    <defs>
        <marker id="dynamic-ref"
                markerWidth="{{model.markerSize}}"
                markerHeight="{{model.markerSize}}"
                refX="{{model.markerSize}}"
                refY="{{model.verticalAlign === 'center' ? model.markerSize / 2 : 0}}"
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

### Conditional Vertical Alignment

```html
<!-- Model: { alignToTop: false } -->
<svg width="400" height="200">
    <defs>
        <marker id="aligned-marker"
                markerWidth="10" markerHeight="10"
                refX="5"
                refY="{{model.alignToTop ? 0 : 5}}"
                orient="auto">
            <circle cx="5" cy="5" r="4" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#aligned-marker)"/>
</svg>
```

### Data-Driven Vertical Positioning

```html
<!-- Model: {
    markers: [
        { size: 10, vAlign: 'top', color: '#e74c3c' },
        { size: 12, vAlign: 'center', color: '#3498db' },
        { size: 14, vAlign: 'bottom', color: '#2ecc71' }
    ],
    getRefY: function(m) {
        if (m.vAlign === 'top') return 0;
        if (m.vAlign === 'center') return m.size / 2;
        return m.size;
    }
} -->
<svg width="300" height="250">
    <defs>
        <template data-bind="{{model.markers}}">
            <marker id="marker-{{$index}}"
                    markerWidth="{{.size}}" markerHeight="{{.size}}"
                    refX="{{.size / 2}}"
                    refY="{{model.getRefY(.)}}">
                <circle cx="{{.size / 2}}" cy="{{.size / 2}}"
                        r="{{.size / 2 - 1}}"
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

The `refY` coordinate within the marker aligns with the path vertex vertically:

```html
<!-- Marker centered vertically -->
<marker markerWidth="10" markerHeight="10" refX="10" refY="5">
    <!-- Point (10, 5) in marker aligns with path vertex -->
    <polygon points="0,0 10,5 0,10"/>
</marker>
```

### Common Vertical Alignment Patterns

**Center alignment (most common):**
```html
<!-- Marker centered on path -->
<marker markerWidth="10" markerHeight="10"
        refX="5" refY="5">
    <circle cx="5" cy="5" r="4"/>
</marker>
```

**Top alignment:**
```html
<!-- Marker's top edge at path -->
<marker markerWidth="10" markerHeight="10"
        refX="5" refY="0">
    <circle cx="5" cy="5" r="4"/>
</marker>
```

**Bottom alignment:**
```html
<!-- Marker's bottom edge at path -->
<marker markerWidth="10" markerHeight="10"
        refX="5" refY="10">
    <circle cx="5" cy="5" r="4"/>
</marker>
```

### Relationship with markerHeight

`refY` should typically be within `0` to `markerHeight`:

```html
<!-- For markerHeight="10" -->
refY="0"    <!-- Top edge -->
refY="5"    <!-- Center -->
refY="10"   <!-- Bottom edge -->
```

### Default Value

If `refY` is not specified, the default is **0** (top edge):

```html
<!-- These are equivalent -->
<marker id="m1" markerWidth="10" markerHeight="10">
<marker id="m2" markerWidth="10" markerHeight="10" refY="0">
```

### Coordinate with refX

Both `refX` and `refY` work together to define the complete reference point:

```html
<!-- Center-center alignment -->
<marker refX="5" refY="5" markerWidth="10" markerHeight="10">

<!-- Tip-center alignment -->
<marker refX="10" refY="5" markerWidth="10" markerHeight="10">

<!-- Center-bottom alignment -->
<marker refX="5" refY="10" markerWidth="10" markerHeight="10">
```

### Visual Impact

Different `refY` values change vertical positioning:

```html
<!-- refY="0": marker below path point -->
<marker refY="0">
    <!-- Marker extends downward from point -->
</marker>

<!-- refY="5": marker centered on path point -->
<marker refY="5">
    <!-- Marker straddles the point vertically -->
</marker>

<!-- refY="10": marker above path point -->
<marker refY="10">
    <!-- Marker extends upward to point -->
</marker>
```

---

## Examples

### Vertically Centered Marker (refY = markerHeight / 2)

```html
<svg width="400" height="150">
    <defs>
        <marker id="centered" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
            <circle cx="10" cy="5" r="1" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#centered)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">refY="5" (vertically centered)</text>
</svg>
```

### Top-Aligned Marker (refY = 0)

```html
<svg width="400" height="150">
    <defs>
        <marker id="top-aligned" markerWidth="10" markerHeight="10"
                refX="10" refY="0" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
            <circle cx="10" cy="0" r="1" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#top-aligned)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">refY="0" (top edge at path)</text>
</svg>
```

### Bottom-Aligned Marker (refY = markerHeight)

```html
<svg width="400" height="150">
    <defs>
        <marker id="bottom-aligned" markerWidth="10" markerHeight="10"
                refX="10" refY="10" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#2ecc71"/>
            <circle cx="10" cy="10" r="1" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#bottom-aligned)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">refY="10" (bottom edge at path)</text>
</svg>
```

### Comparison of Different refY Values

```html
<svg width="400" height="300">
    <defs>
        <marker id="ref-y0" markerWidth="10" markerHeight="16"
                refX="10" refY="0" orient="auto">
            <polygon points="0,0 10,8 0,16" fill="#e74c3c"/>
            <circle cx="10" cy="0" r="1.5" fill="white"/>
        </marker>
        <marker id="ref-y8" markerWidth="10" markerHeight="16"
                refX="10" refY="8" orient="auto">
            <polygon points="0,0 10,8 0,16" fill="#3498db"/>
            <circle cx="10" cy="8" r="1.5" fill="white"/>
        </marker>
        <marker id="ref-y16" markerWidth="10" markerHeight="16"
                refX="10" refY="16" orient="auto">
            <polygon points="0,0 10,8 0,16" fill="#2ecc71"/>
            <circle cx="10" cy="16" r="1.5" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="60" x2="300" y2="60" stroke="#34495e" stroke-width="2" marker-end="url(#ref-y0)"/>
    <circle cx="300" cy="60" r="3" fill="orange"/>
    <text x="320" y="65" font-size="10">refY="0"</text>

    <line x1="50" y1="150" x2="300" y2="150" stroke="#34495e" stroke-width="2" marker-end="url(#ref-y8)"/>
    <circle cx="300" cy="150" r="3" fill="orange"/>
    <text x="320" y="155" font-size="10">refY="8"</text>

    <line x1="50" y1="240" x2="300" y2="240" stroke="#34495e" stroke-width="2" marker-end="url(#ref-y16)"/>
    <circle cx="300" cy="240" r="3" fill="orange"/>
    <text x="320" y="245" font-size="10">refY="16"</text>
</svg>
```

### Circle with Different Vertical Alignments

```html
<svg width="400" height="300">
    <defs>
        <marker id="circle-top" markerWidth="12" markerHeight="12"
                refX="6" refY="0">
            <circle cx="6" cy="6" r="5" fill="#9b59b6"/>
            <circle cx="6" cy="0" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
        <marker id="circle-mid" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <circle cx="6" cy="6" r="5" fill="#9b59b6"/>
            <circle cx="6" cy="6" r="1.5" fill="white"/>
        </marker>
        <marker id="circle-bot" markerWidth="12" markerHeight="12"
                refX="6" refY="12">
            <circle cx="6" cy="6" r="5" fill="#9b59b6"/>
            <circle cx="6" cy="12" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
    </defs>

    <line x1="50" y1="60" x2="300" y2="60" stroke="#34495e" stroke-width="2" marker-end="url(#circle-top)"/>
    <circle cx="300" cy="60" r="3" fill="orange"/>
    <text x="320" y="65" font-size="9">Top</text>

    <line x1="50" y1="150" x2="300" y2="150" stroke="#34495e" stroke-width="2" marker-end="url(#circle-mid)"/>
    <circle cx="300" cy="150" r="3" fill="orange"/>
    <text x="320" y="155" font-size="9">Center</text>

    <line x1="50" y1="240" x2="300" y2="240" stroke="#34495e" stroke-width="2" marker-end="url(#circle-bot)"/>
    <circle cx="300" cy="240" r="3" fill="orange"/>
    <text x="320" y="245" font-size="9">Bottom</text>
</svg>
```

### Diamond with Vertical Alignment Options

```html
<svg width="400" height="300">
    <defs>
        <marker id="diamond-top" markerWidth="10" markerHeight="12"
                refX="5" refY="0" orient="auto">
            <path d="M 5,0 L 10,6 L 5,12 L 0,6 Z" fill="#f39c12"/>
            <circle cx="5" cy="0" r="1.5" fill="white"/>
        </marker>
        <marker id="diamond-mid" markerWidth="10" markerHeight="12"
                refX="5" refY="6" orient="auto">
            <path d="M 5,0 L 10,6 L 5,12 L 0,6 Z" fill="#f39c12"/>
            <circle cx="5" cy="6" r="1.5" fill="white"/>
        </marker>
        <marker id="diamond-bot" markerWidth="10" markerHeight="12"
                refX="5" refY="12" orient="auto">
            <path d="M 5,0 L 10,6 L 5,12 L 0,6 Z" fill="#f39c12"/>
            <circle cx="5" cy="12" r="1.5" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="60" x2="300" y2="60" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-top)"/>
    <circle cx="300" cy="60" r="3" fill="orange"/>

    <line x1="50" y1="150" x2="300" y2="150" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-mid)"/>
    <circle cx="300" cy="150" r="3" fill="orange"/>

    <line x1="50" y1="240" x2="300" y2="240" stroke="#34495e" stroke-width="2" marker-end="url(#diamond-bot)"/>
    <circle cx="300" cy="240" r="3" fill="orange"/>
</svg>
```

### Rectangle with Corner References

```html
<svg width="400" height="300">
    <defs>
        <marker id="rect-tl" markerWidth="12" markerHeight="10"
                refX="0" refY="0">
            <rect width="12" height="10" fill="#e74c3c"/>
            <circle cx="0" cy="0" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
        <marker id="rect-mc" markerWidth="12" markerHeight="10"
                refX="6" refY="5">
            <rect width="12" height="10" fill="#3498db"/>
            <circle cx="6" cy="5" r="1.5" fill="white"/>
        </marker>
        <marker id="rect-br" markerWidth="12" markerHeight="10"
                refX="12" refY="10">
            <rect width="12" height="10" fill="#2ecc71"/>
            <circle cx="12" cy="10" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
    </defs>

    <line x1="50" y1="60" x2="300" y2="60" stroke="#34495e" stroke-width="2" marker-end="url(#rect-tl)"/>
    <circle cx="300" cy="60" r="3" fill="orange"/>
    <text x="320" y="65" font-size="9">TL (0,0)</text>

    <line x1="50" y1="150" x2="300" y2="150" stroke="#34495e" stroke-width="2" marker-end="url(#rect-mc)"/>
    <circle cx="300" cy="150" r="3" fill="orange"/>
    <text x="320" y="155" font-size="9">MC (6,5)</text>

    <line x1="50" y1="240" x2="300" y2="240" stroke="#34495e" stroke-width="2" marker-end="url(#rect-br)"/>
    <circle cx="300" cy="240" r="3" fill="orange"/>
    <text x="320" y="245" font-size="9">BR (12,10)</text>
</svg>
```

### Star with Custom refY

```html
<svg width="400" height="180">
    <defs>
        <marker id="star-custom" markerWidth="18" markerHeight="18"
                refX="9" refY="12">
            <path d="M 9,1 L 11,7 L 17,7 L 12,11 L 14,17 L 9,13 L 4,17 L 6,11 L 1,7 L 7,7 Z"
                  fill="#f1c40f" stroke="#f39c12" stroke-width="1"/>
            <circle cx="9" cy="12" r="1.5" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="90" x2="350" y2="90"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#star-custom)"/>
    <circle cx="350" cy="90" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Star with custom refY="12"</text>
</svg>
```

### Ellipse with Vertical Positioning

```html
<svg width="400" height="300">
    <defs>
        <marker id="ellipse-top" markerWidth="14" markerHeight="20"
                refX="7" refY="0">
            <ellipse cx="7" cy="10" rx="6" ry="9" fill="#3498db"/>
            <circle cx="7" cy="0" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
        <marker id="ellipse-mid" markerWidth="14" markerHeight="20"
                refX="7" refY="10">
            <ellipse cx="7" cy="10" rx="6" ry="9" fill="#3498db"/>
            <circle cx="7" cy="10" r="1.5" fill="white"/>
        </marker>
        <marker id="ellipse-bot" markerWidth="14" markerHeight="20"
                refX="7" refY="20">
            <ellipse cx="7" cy="10" rx="6" ry="9" fill="#3498db"/>
            <circle cx="7" cy="20" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
    </defs>

    <line x1="50" y1="60" x2="300" y2="60" stroke="#34495e" stroke-width="2" marker-end="url(#ellipse-top)"/>
    <circle cx="300" cy="60" r="3" fill="orange"/>

    <line x1="50" y1="150" x2="300" y2="150" stroke="#34495e" stroke-width="2" marker-end="url(#ellipse-mid)"/>
    <circle cx="300" cy="150" r="3" fill="orange"/>

    <line x1="50" y1="245" x2="300" y2="245" stroke="#34495e" stroke-width="2" marker-end="url(#ellipse-bot)"/>
    <circle cx="300" cy="245" r="3" fill="orange"/>
</svg>
```

### Decimal refY Value

```html
<svg width="400" height="150">
    <defs>
        <marker id="decimal-y" markerWidth="10" markerHeight="13"
                refX="10" refY="6.5" orient="auto">
            <polygon points="0,0 10,6.5 0,13" fill="#9b59b6"/>
            <circle cx="10" cy="6.5" r="1.5" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#decimal-y)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Precise: refY="6.5"</text>
</svg>
```

### Negative refY (Outside Marker)

```html
<svg width="400" height="150">
    <defs>
        <marker id="negative-y" markerWidth="10" markerHeight="10"
                refX="10" refY="-3" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
            <circle cx="10" cy="-3" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#negative-y)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Negative refY="-3"</text>
</svg>
```

### Tall Marker with Different refY

```html
<svg width="400" height="300">
    <defs>
        <marker id="tall-y0" markerWidth="8" markerHeight="20"
                refX="4" refY="0">
            <rect width="8" height="20" rx="2" fill="#2ecc71"/>
            <circle cx="4" cy="0" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
        <marker id="tall-y10" markerWidth="8" markerHeight="20"
                refX="4" refY="10">
            <rect width="8" height="20" rx="2" fill="#2ecc71"/>
            <circle cx="4" cy="10" r="1.5" fill="white"/>
        </marker>
        <marker id="tall-y20" markerWidth="8" markerHeight="20"
                refX="4" refY="20">
            <rect width="8" height="20" rx="2" fill="#2ecc71"/>
            <circle cx="4" cy="20" r="1.5" fill="white" stroke="black" stroke-width="0.5"/>
        </marker>
    </defs>

    <line x1="50" y1="70" x2="300" y2="70" stroke="#34495e" stroke-width="2" marker-end="url(#tall-y0)"/>
    <circle cx="300" cy="70" r="3" fill="orange"/>

    <line x1="50" y1="150" x2="300" y2="150" stroke="#34495e" stroke-width="2" marker-end="url(#tall-y10)"/>
    <circle cx="300" cy="150" r="3" fill="orange"/>

    <line x1="50" y1="230" x2="300" y2="230" stroke="#34495e" stroke-width="2" marker-end="url(#tall-y20)"/>
    <circle cx="300" cy="230" r="3" fill="orange"/>
</svg>
```

### Complex Icon with Precise Vertical Positioning

```html
<svg width="400" height="180">
    <defs>
        <marker id="complex-y" markerWidth="20" markerHeight="24"
                refX="10" refY="12">
            <rect x="2" y="2" width="16" height="20" rx="3" fill="#3498db" stroke="#2980b9" stroke-width="2"/>
            <path d="M 6,12 L 9,15 L 14,10" fill="none" stroke="white" stroke-width="2" stroke-linecap="round"/>
            <circle cx="10" cy="12" r="1.5" fill="yellow"/>
        </marker>
    </defs>

    <line x1="50" y1="90" x2="350" y2="90"
          stroke="#34495e" stroke-width="3"
          marker-end="url(#complex-y)"/>
    <circle cx="350" cy="90" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Complex icon: refY="12" (vertical center)</text>
</svg>
```

### Gradient Marker with Custom refY

```html
<svg width="400" height="150">
    <defs>
        <linearGradient id="grad-v" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#667eea"/>
            <stop offset="100%" stop-color="#764ba2"/>
        </linearGradient>

        <marker id="gradient-y" markerWidth="14" markerHeight="14"
                refX="7" refY="7">
            <circle cx="7" cy="7" r="6" fill="url(#grad-v)" stroke="#5a67d8" stroke-width="1"/>
            <circle cx="7" cy="7" r="1.5" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="3"
          marker-end="url(#gradient-y)"/>
    <circle cx="350" cy="75" r="3" fill="orange"/>
    <text x="200" y="50" text-anchor="middle" font-size="11">Gradient: refY="7"</text>
</svg>
```

### Offset Vertical Positioning

```html
<svg width="400" height="300">
    <defs>
        <marker id="offset-top" markerWidth="10" markerHeight="10"
                refX="10" refY="2" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
            <line x1="0" y1="2" x2="10" y2="2" stroke="white" stroke-width="0.5"/>
        </marker>
        <marker id="offset-mid" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
            <line x1="0" y1="5" x2="10" y2="5" stroke="white" stroke-width="0.5"/>
        </marker>
        <marker id="offset-bot" markerWidth="10" markerHeight="10"
                refX="10" refY="8" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#2ecc71"/>
            <line x1="0" y1="8" x2="10" y2="8" stroke="white" stroke-width="0.5"/>
        </marker>
    </defs>

    <line x1="50" y1="60" x2="300" y2="60" stroke="#34495e" stroke-width="2" marker-end="url(#offset-top)"/>
    <circle cx="300" cy="60" r="3" fill="orange"/>
    <text x="320" y="65" font-size="9">refY="2"</text>

    <line x1="50" y1="150" x2="300" y2="150" stroke="#34495e" stroke-width="2" marker-end="url(#offset-mid)"/>
    <circle cx="300" cy="150" r="3" fill="orange"/>
    <text x="320" y="155" font-size="9">refY="5"</text>

    <line x1="50" y1="240" x2="300" y2="240" stroke="#34495e" stroke-width="2" marker-end="url(#offset-bot)"/>
    <circle cx="300" cy="240" r="3" fill="orange"/>
    <text x="320" y="245" font-size="9">refY="8"</text>
</svg>
```

---

## See Also

- [refX](/reference/svgattributes/refX.html) - Marker reference X position
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
