---
layout: default
title: marker-end
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @marker-end : The Path End Marker Attribute

The `marker-end` attribute specifies a marker graphic to be drawn at the final vertex of a path, line, polyline, or polygon. Markers are commonly used to add arrowheads, endpoints, or other decorative elements at the end of shapes.

## Usage

The `marker-end` attribute is used to:
- Add arrowheads at the end of lines and paths
- Mark the terminal point of a shape
- Create directional indicators showing flow or movement
- Support data-driven endpoint visualization
- Build flow diagrams with clear direction
- Enhance visualization with terminal markers

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
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

| Value | Description | Example |
|-------|-------------|---------|
| `none` | No marker (default) | `marker-end="none"` |
| `url(#id)` | Reference to marker element | `marker-end="url(#arrow)"` |
| `inherit` | Inherit from parent | `marker-end="inherit"` |

### URL Reference Format

```html
<!-- Reference marker by ID -->
marker-end="url(#markerID)"

<!-- With fallback -->
marker-end="url(#markerID) none"
```

---

## Supported Elements

The `marker-end` attribute is supported on:

- **[&lt;path&gt;](/reference/svgtags/path.html)** - At the final point of path data
- **[&lt;line&gt;](/reference/svgtags/line.html)** - At the (x2, y2) point
- **[&lt;polyline&gt;](/reference/svgtags/polyline.html)** - At the last point
- **[&lt;polygon&gt;](/reference/svgtags/polygon.html)** - At the last point (before closing)

---

## Data Binding

### Dynamic Arrow Selection

Choose arrow type based on data:

```html
<!-- Model: { connectionType: 'forward', arrows: { forward: 'arrow-forward', backward: 'arrow-back' } } -->
<svg width="400" height="200">
    <defs>
        <marker id="arrow-forward" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
        <marker id="arrow-back" markerWidth="10" markerHeight="10"
                refX="0" refY="5" orient="auto">
            <polygon points="10,0 0,5 10,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#{{model.arrows[model.connectionType]}})"/>
</svg>
```

### Conditional End Markers

Show markers based on status:

```html
<!-- Model: { isComplete: true, hasTarget: true } -->
<svg width="400" height="200">
    <defs>
        <marker id="complete-marker" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <circle cx="6" cy="6" r="5" fill="#2ecc71"/>
            <path d="M 3,6 L 5,8 L 9,4" fill="none" stroke="white" stroke-width="2"/>
        </marker>
    </defs>

    <path d="M 50,50 Q 200,150 350,50"
          fill="none" stroke="#34495e" stroke-width="3"
          marker-end="{{model.isComplete && model.hasTarget ? 'url(#complete-marker)' : 'none'}}"/>
</svg>
```

### Data-Driven Flow Visualization

```html
<!-- Model: {
    flows: [
        { from: {x: 50, y: 50}, to: {x: 200, y: 50}, active: true },
        { from: {x: 50, y: 100}, to: {x: 200, y: 100}, active: false }
    ]
} -->
<svg width="250" height="150">
    <defs>
        <marker id="active-arrow" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#2ecc71"/>
        </marker>
        <marker id="inactive-arrow" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#95a5a6"/>
        </marker>
    </defs>

    <template data-bind="{{model.flows}}">
        <line x1="{{.from.x}}" y1="{{.from.y}}"
              x2="{{.to.x}}" y2="{{.to.y}}"
              stroke="{{.active ? '#2ecc71' : '#95a5a6'}}" stroke-width="2"
              marker-end="{{.active ? 'url(#active-arrow)' : 'url(#inactive-arrow)'}}"/>
    </template>
</svg>
```

---

## Notes

### Marker Placement

- The marker is positioned at the last vertex of the shape
- For **line**: placed at (x2, y2)
- For **path**: placed at the final point (last command endpoint)
- For **polyline/polygon**: placed at the last point in the points list

### Automatic Orientation

- Use `orient="auto"` to automatically orient the marker along the path direction
- The marker rotates to match the angle of the incoming path segment
- Fixed angles can be specified: `orient="90"` (in degrees)
- `orient="auto-start-reverse"` is typically not used for end markers

### Arrow Design Best Practices

```html
<!-- Properly aligned arrow -->
<marker id="arrow" markerWidth="10" markerHeight="10"
        refX="10" refY="5" orient="auto">
    <!-- Arrow point at refX for proper alignment -->
    <polygon points="0,0 10,5 0,10" fill="#3498db"/>
</marker>
```

### Combining Markers

```html
<!-- Start, middle, and end markers -->
<polyline points="50,150 150,50 250,100 350,150"
          marker-start="url(#start-dot)"
          marker-mid="url(#mid-dot)"
          marker-end="url(#end-arrow)"/>
```

### Marker Scaling

- Markers can scale with stroke width using `markerUnits="strokeWidth"`
- Default is `markerUnits="userSpaceOnUse"` (fixed size)
- Choose based on whether markers should resize with the path

### Performance Considerations

- End markers add one marker per shape
- Simple shapes (circle, polygon) perform better than complex paths
- Reuse marker definitions across multiple shapes
- Consider PDF file size with many marker instances

### Styling

- Markers inherit no styles from the referencing element by default
- Define all fill, stroke, and other properties in the marker definition
- Some implementations support `context-fill` and `context-stroke`

### Common Use Cases

- **Directional arrows**: Showing flow or movement direction
- **Target indicators**: Marking endpoints or destinations
- **Process terminals**: Indicating end states in diagrams
- **Decorative endings**: Adding visual flair to line endings

---

## Examples

### Basic Arrow

```html
<svg width="400" height="150">
    <defs>
        <marker id="basic-arrow" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#basic-arrow)"/>
</svg>
```

### Circle End Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="end-circle" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#3498db"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#end-circle)"/>
</svg>
```

### Multiple Arrow Styles

```html
<svg width="400" height="250">
    <defs>
        <marker id="arrow1" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
        <marker id="arrow2" markerWidth="12" markerHeight="12"
                refX="6" refY="6" orient="auto">
            <path d="M 0,0 L 12,6 L 0,12 L 3,6 Z" fill="#3498db"/>
        </marker>
        <marker id="arrow3" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <path d="M 0,0 L 10,5 L 0,10" fill="none" stroke="#2ecc71" stroke-width="2"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="350" y2="50" stroke="#34495e" stroke-width="2" marker-end="url(#arrow1)"/>
    <line x1="50" y1="125" x2="350" y2="125" stroke="#34495e" stroke-width="2" marker-end="url(#arrow2)"/>
    <line x1="50" y1="200" x2="350" y2="200" stroke="#34495e" stroke-width="2" marker-end="url(#arrow3)"/>
</svg>
```

### Flow Diagram Arrow

```html
<svg width="500" height="200">
    <defs>
        <marker id="flow-arrow" markerWidth="12" markerHeight="12"
                refX="12" refY="6" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="#3498db"/>
        </marker>
    </defs>

    <rect x="50" y="75" width="80" height="50" fill="#ecf0f1" stroke="#34495e" stroke-width="2"/>
    <text x="90" y="105" text-anchor="middle" font-size="14">Start</text>

    <line x1="130" y1="100" x2="220" y2="100" stroke="#34495e" stroke-width="2" marker-end="url(#flow-arrow)"/>

    <rect x="220" y="75" width="80" height="50" fill="#ecf0f1" stroke="#34495e" stroke-width="2"/>
    <text x="260" y="105" text-anchor="middle" font-size="14">Process</text>

    <line x1="300" y1="100" x2="390" y2="100" stroke="#34495e" stroke-width="2" marker-end="url(#flow-arrow)"/>

    <rect x="390" y="75" width="80" height="50" fill="#ecf0f1" stroke="#34495e" stroke-width="2"/>
    <text x="430" y="105" text-anchor="middle" font-size="14">End</text>
</svg>
```

### Curved Path with Arrow

```html
<svg width="400" height="300">
    <defs>
        <marker id="curve-arrow" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#9b59b6"/>
        </marker>
    </defs>

    <path d="M 50,50 C 50,200 350,200 350,50"
          fill="none" stroke="#34495e" stroke-width="3"
          marker-end="url(#curve-arrow)"/>
</svg>
```

### Bidirectional Arrows

```html
<svg width="400" height="150">
    <defs>
        <marker id="arrow-end" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
        <marker id="arrow-start" markerWidth="10" markerHeight="10"
                refX="0" refY="5" orient="auto">
            <polygon points="10,0 0,5 10,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-start="url(#arrow-start)"
          marker-end="url(#arrow-end)"/>
</svg>
```

### Target End Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="target" markerWidth="16" markerHeight="16"
                refX="8" refY="8">
            <circle cx="8" cy="8" r="7" fill="white" stroke="#e74c3c" stroke-width="2"/>
            <circle cx="8" cy="8" r="4" fill="#e74c3c"/>
            <circle cx="8" cy="8" r="2" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#target)"/>
</svg>
```

### Success Checkmark End

```html
<svg width="400" height="150">
    <defs>
        <marker id="success" markerWidth="16" markerHeight="16"
                refX="8" refY="8">
            <circle cx="8" cy="8" r="7" fill="#2ecc71"/>
            <path d="M 4,8 L 7,11 L 12,6" fill="none" stroke="white" stroke-width="2" stroke-linecap="round"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#2ecc71" stroke-width="3"
          marker-end="url(#success)"/>
</svg>
```

### Diamond End Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="diamond-end" markerWidth="12" markerHeight="12"
                refX="6" refY="6" orient="auto">
            <path d="M 6,0 L 12,6 L 6,12 L 0,6 Z"
                  fill="#f39c12" stroke="#e67e22" stroke-width="1"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#diamond-end)"/>
</svg>
```

### Gradient Arrow

```html
<svg width="400" height="150">
    <defs>
        <linearGradient id="arrow-gradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#667eea"/>
            <stop offset="100%" stop-color="#764ba2"/>
        </linearGradient>

        <marker id="gradient-arrow" markerWidth="12" markerHeight="12"
                refX="12" refY="6" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="url(#arrow-gradient)"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="3"
          marker-end="url(#gradient-arrow)"/>
</svg>
```

### Network Flow

```html
<svg width="400" height="300">
    <defs>
        <marker id="data-arrow" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
    </defs>

    <!-- Central node -->
    <circle cx="200" cy="150" r="25" fill="#e74c3c"/>

    <!-- Outgoing connections -->
    <line x1="200" y1="150" x2="100" y2="80" stroke="#3498db" stroke-width="2" marker-end="url(#data-arrow)"/>
    <line x1="200" y1="150" x2="300" y2="80" stroke="#3498db" stroke-width="2" marker-end="url(#data-arrow)"/>
    <line x1="200" y1="150" x2="100" y2="220" stroke="#3498db" stroke-width="2" marker-end="url(#data-arrow)"/>
    <line x1="200" y1="150" x2="300" y2="220" stroke="#3498db" stroke-width="2" marker-end="url(#data-arrow)"/>

    <text x="200" y="155" text-anchor="middle" fill="white" font-size="12" font-weight="bold">Hub</text>
</svg>
```

### Stop Sign End Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="stop" markerWidth="18" markerHeight="18"
                refX="9" refY="9">
            <polygon points="9,0 18,6 18,12 9,18 0,12 0,6"
                     fill="#e74c3c" stroke="#c0392b" stroke-width="2"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-end="url(#stop)"/>
</svg>
```

### Polyline with End Arrow

```html
<svg width="400" height="200">
    <defs>
        <marker id="poly-arrow" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#2ecc71"/>
        </marker>
    </defs>

    <polyline points="50,150 120,80 200,140 280,70 350,150"
              fill="none" stroke="#34495e" stroke-width="2"
              marker-end="url(#poly-arrow)"/>
</svg>
```

### Timeline End Point

```html
<svg width="600" height="150">
    <defs>
        <marker id="timeline-end" markerWidth="20" markerHeight="20"
                refX="10" refY="10">
            <circle cx="10" cy="10" r="8" fill="#e74c3c"/>
            <rect x="6" y="6" width="8" height="8" fill="white"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="550" y2="75"
          stroke="#3498db" stroke-width="4"
          marker-end="url(#timeline-end)"/>

    <text x="50" y="110" text-anchor="middle" font-size="12">Start</text>
    <text x="550" y="110" text-anchor="middle" font-size="12">End</text>
</svg>
```

### Destination Pin

```html
<svg width="400" height="200">
    <defs>
        <marker id="destination" markerWidth="24" markerHeight="32"
                refX="12" refY="24">
            <path d="M 12,4 Q 20,4 20,12 Q 20,20 12,28 Q 4,20 4,12 Q 4,4 12,4 Z"
                  fill="#e74c3c" stroke="#c0392b" stroke-width="2"/>
            <circle cx="12" cy="12" r="4" fill="white"/>
        </marker>
    </defs>

    <path d="M 50,50 Q 200,100 350,50"
          fill="none" stroke="#34495e" stroke-width="2"
          marker-end="url(#destination)"/>
</svg>
```

---

## See Also

- [marker-start](/reference/svgattributes/marker-start.html) - Marker at path start
- [marker-mid](/reference/svgattributes/marker-mid.html) - Marker at middle vertices
- [marker element](/reference/svgtags/marker.html) - Marker definition element
- [markerWidth](/reference/svgattributes/markerWidth.html) - Marker viewport width
- [markerHeight](/reference/svgattributes/markerHeight.html) - Marker viewport height
- [refX](/reference/svgattributes/refX.html) - Marker reference X position
- [refY](/reference/svgattributes/refY.html) - Marker reference Y position
- [orient](/reference/svgattributes/orient.html) - Marker orientation
- [path element](/reference/svgtags/path.html) - SVG path element
- [line element](/reference/svgtags/line.html) - SVG line element
- [Data Binding](/reference/binding/) - Data binding and expressions

---
