---
layout: default
title: marker-start
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @marker-start : The Path Start Marker Attribute

The `marker-start` attribute specifies a marker graphic to be drawn at the first vertex of a path, line, polyline, or polygon. Markers are typically used to add arrowheads, dots, or other decorative elements at the start of shapes.

## Usage

The `marker-start` attribute is used to:
- Add arrowheads at the beginning of lines and paths
- Mark the starting point of a shape with dots or icons
- Create decorative terminals at path beginnings
- Support data-driven marker selection
- Build flow diagrams with directional indicators
- Enhance visualization clarity with start markers

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
        <marker id="arrow-start" markerWidth="10" markerHeight="10"
                refX="5" refY="5" orient="auto">
            <circle cx="5" cy="5" r="4" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-start="url(#arrow-start)"/>
</svg>
```

---

## Supported Values

| Value | Description | Example |
|-------|-------------|---------|
| `none` | No marker (default) | `marker-start="none"` |
| `url(#id)` | Reference to marker element | `marker-start="url(#arrow)"` |
| `inherit` | Inherit from parent | `marker-start="inherit"` |

### URL Reference Format

```html
<!-- Reference marker by ID -->
marker-start="url(#markerID)"

<!-- With fallback -->
marker-start="url(#markerID) none"
```

---

## Supported Elements

The `marker-start` attribute is supported on:

- **[&lt;path&gt;](/reference/svgtags/path.html)** - At the first point of path data
- **[&lt;line&gt;](/reference/svgtags/line.html)** - At the (x1, y1) point
- **[&lt;polyline&gt;](/reference/svgtags/polyline.html)** - At the first point
- **[&lt;polygon&gt;](/reference/svgtags/polygon.html)** - At the first point

---

## Data Binding

### Dynamic Marker Selection

Choose marker based on data properties:

```html
<!-- Model: { lineType: 'primary', markers: { primary: 'dot-start', secondary: 'arrow-start' } } -->
<svg width="400" height="200">
    <defs>
        <marker id="dot-start" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#3498db"/>
        </marker>
        <marker id="arrow-start" markerWidth="10" markerHeight="10"
                refX="5" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-start="url(#{{model.markers[model.lineType]}})"/>
</svg>
```

### Conditional Markers

Show markers based on conditions:

```html
<!-- Model: { showStart: true, isActive: true } -->
<svg width="400" height="200">
    <defs>
        <marker id="start-marker" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <circle cx="6" cy="6" r="5" fill="#2ecc71"/>
        </marker>
    </defs>

    <path d="M 50,50 Q 200,150 350,50"
          fill="none" stroke="#34495e" stroke-width="3"
          marker-start="{{model.showStart ? 'url(#start-marker)' : 'none'}}"/>
</svg>
```

### Data-Driven Connection Visualization

```html
<!-- Model: {
    connections: [
        { from: {x: 50, y: 50}, to: {x: 200, y: 50}, type: 'direct' },
        { from: {x: 50, y: 100}, to: {x: 200, y: 100}, type: 'bidirectional' }
    ]
} -->
<svg width="250" height="150">
    <defs>
        <marker id="start-dot" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#9b59b6"/>
        </marker>
        <marker id="start-arrow" markerWidth="10" markerHeight="10"
                refX="0" refY="5" orient="auto">
            <polygon points="10,0 0,5 10,10" fill="#3498db"/>
        </marker>
    </defs>

    <template data-bind="{{model.connections}}">
        <line x1="{{.from.x}}" y1="{{.from.y}}"
              x2="{{.to.x}}" y2="{{.to.y}}"
              stroke="#2c3e50" stroke-width="2"
              marker-start="{{.type === 'bidirectional' ? 'url(#start-arrow)' : 'none'}}"/>
    </template>
</svg>
```

---

## Notes

### Marker Placement

- The marker is positioned at the first vertex of the shape
- For **line**: placed at (x1, y1)
- For **path**: placed at the first M (move) or first point
- For **polyline/polygon**: placed at the first point in the points list

### Marker Orientation

- Use `orient="auto"` on the marker to automatically orient it along the path direction
- Use `orient="auto-start-reverse"` to reverse the marker at the start
- Fixed angles can be specified: `orient="45"` (in degrees)

### Combining with Other Markers

```html
<!-- Use all three marker positions -->
<line x1="50" y1="100" x2="350" y2="100"
      marker-start="url(#start)"
      marker-mid="url(#mid)"
      marker-end="url(#end)"/>
```

### Marker Coordinate System

- Markers have their own coordinate system defined by `markerWidth` and `markerHeight`
- The `refX` and `refY` attributes define the reference point
- Markers can use `markerUnits="strokeWidth"` to scale with stroke width

### Performance Considerations

- Markers are rendered for each applicable vertex
- Complex markers can impact PDF file size
- Consider reusing marker definitions across multiple shapes
- Simple geometric shapes perform better than complex paths

### Styling Inheritance

- Markers do not inherit fill/stroke from the referencing element by default
- Style the marker content explicitly in the marker definition
- Use `context-fill` and `context-stroke` for inheritance (if supported)

### Common Use Cases

- **Start indicators**: Marking the beginning of a process flow
- **Source markers**: Indicating data flow origin
- **Decorative terminals**: Adding visual interest to shape beginnings
- **Bidirectional arrows**: Showing two-way connections

---

## Examples

### Basic Start Dot

```html
<svg width="400" height="150">
    <defs>
        <marker id="dot" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-start="url(#dot)"/>
</svg>
```

### Start Arrow on Path

```html
<svg width="400" height="200">
    <defs>
        <marker id="start-arrow" markerWidth="10" markerHeight="10"
                refX="0" refY="5" orient="auto-start-reverse">
            <polygon points="10,0 0,5 10,10" fill="#3498db"/>
        </marker>
    </defs>

    <path d="M 50,150 Q 200,50 350,150"
          fill="none" stroke="#2c3e50" stroke-width="3"
          marker-start="url(#start-arrow)"/>
</svg>
```

### Multiple Lines with Start Markers

```html
<svg width="400" height="300">
    <defs>
        <marker id="circle-start" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#2ecc71"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="350" y2="50"
          stroke="#34495e" stroke-width="2"
          marker-start="url(#circle-start)"/>
    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#34495e" stroke-width="2"
          marker-start="url(#circle-start)"/>
    <line x1="50" y1="150" x2="350" y2="150"
          stroke="#34495e" stroke-width="2"
          marker-start="url(#circle-start)"/>
</svg>
```

### Process Flow Start

```html
<svg width="500" height="200">
    <defs>
        <marker id="flow-start" markerWidth="20" markerHeight="20"
                refX="10" refY="10">
            <circle cx="10" cy="10" r="8" fill="#2ecc71" stroke="#27ae60" stroke-width="2"/>
            <text x="10" y="14" text-anchor="middle" font-size="10" fill="white" font-weight="bold">S</text>
        </marker>
    </defs>

    <path d="M 50,100 L 200,100 L 200,150 L 450,150"
          fill="none" stroke="#3498db" stroke-width="3"
          marker-start="url(#flow-start)"/>
</svg>
```

### Diamond Start Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="diamond" markerWidth="12" markerHeight="12"
                refX="6" refY="6" orient="auto">
            <path d="M 6,0 L 12,6 L 6,12 L 0,6 Z"
                  fill="#f39c12" stroke="#e67e22" stroke-width="1"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#2c3e50" stroke-width="2"
          marker-start="url(#diamond)"/>
</svg>
```

### Square Start Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="square" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <rect x="1" y="1" width="8" height="8"
                  fill="#9b59b6" stroke="#8e44ad" stroke-width="1"/>
        </marker>
    </defs>

    <polyline points="50,50 200,100 350,50"
              fill="none" stroke="#34495e" stroke-width="2"
              marker-start="url(#square)"/>
</svg>
```

### Custom Icon Start Marker

```html
<svg width="400" height="150">
    <defs>
        <marker id="star-start" markerWidth="20" markerHeight="20"
                refX="10" refY="10">
            <path d="M 10,2 L 12,8 L 18,8 L 13,12 L 15,18 L 10,14 L 5,18 L 7,12 L 2,8 L 8,8 Z"
                  fill="#f1c40f" stroke="#f39c12" stroke-width="1"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-start="url(#star-start)"/>
</svg>
```

### Directional Connection Start

```html
<svg width="400" height="300">
    <defs>
        <marker id="source" markerWidth="16" markerHeight="16"
                refX="8" refY="8">
            <circle cx="8" cy="8" r="7" fill="#3498db" stroke="#2980b9" stroke-width="1"/>
            <circle cx="8" cy="8" r="3" fill="#ecf0f1"/>
        </marker>
    </defs>

    <g stroke="#95a5a6" stroke-width="2" fill="none">
        <line x1="200" y1="150" x2="100" y2="50" marker-start="url(#source)"/>
        <line x1="200" y1="150" x2="300" y2="50" marker-start="url(#source)"/>
        <line x1="200" y1="150" x2="100" y2="250" marker-start="url(#source)"/>
        <line x1="200" y1="150" x2="300" y2="250" marker-start="url(#source)"/>
    </g>

    <circle cx="200" cy="150" r="20" fill="#e74c3c"/>
</svg>
```

### Gradient Fill Start Marker

```html
<svg width="400" height="150">
    <defs>
        <linearGradient id="marker-gradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stop-color="#667eea"/>
            <stop offset="100%" stop-color="#764ba2"/>
        </linearGradient>

        <marker id="gradient-start" markerWidth="14" markerHeight="14"
                refX="7" refY="7">
            <circle cx="7" cy="7" r="6" fill="url(#marker-gradient)"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="3"
          marker-start="url(#gradient-start)"/>
</svg>
```

### Bidirectional Line

```html
<svg width="400" height="150">
    <defs>
        <marker id="arrow-both" markerWidth="10" markerHeight="10"
                refX="5" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-start="url(#arrow-both)"
          marker-end="url(#arrow-both)"/>
</svg>
```

### Different Markers for Start and End

```html
<svg width="400" height="150">
    <defs>
        <marker id="start-circle" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#2ecc71"/>
        </marker>
        <marker id="end-arrow" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="350" y2="75"
          stroke="#34495e" stroke-width="2"
          marker-start="url(#start-circle)"
          marker-end="url(#end-arrow)"/>
</svg>
```

### Network Node Connection

```html
<svg width="400" height="300">
    <defs>
        <marker id="node-start" markerWidth="16" markerHeight="16"
                refX="8" refY="8">
            <rect x="2" y="2" width="12" height="12"
                  fill="#3498db" stroke="#2980b9" stroke-width="1" rx="2"/>
        </marker>
    </defs>

    <!-- Central node -->
    <circle cx="200" cy="150" r="25" fill="#e74c3c"/>

    <!-- Connections with start markers -->
    <line x1="100" y1="80" x2="180" y2="135"
          stroke="#95a5a6" stroke-width="2"
          marker-start="url(#node-start)"/>
    <line x1="300" y1="80" x2="220" y2="135"
          stroke="#95a5a6" stroke-width="2"
          marker-start="url(#node-start)"/>
    <line x1="100" y1="220" x2="180" y2="165"
          stroke="#95a5a6" stroke-width="2"
          marker-start="url(#node-start)"/>
    <line x1="300" y1="220" x2="220" y2="165"
          stroke="#95a5a6" stroke-width="2"
          marker-start="url(#node-start)"/>
</svg>
```

### Timeline Start Point

```html
<svg width="600" height="150">
    <defs>
        <marker id="timeline-start" markerWidth="20" markerHeight="20"
                refX="10" refY="10">
            <circle cx="10" cy="10" r="8" fill="#2ecc71"/>
            <path d="M 7,10 L 10,13 L 15,8"
                  fill="none" stroke="white" stroke-width="2" stroke-linecap="round"/>
        </marker>
    </defs>

    <line x1="50" y1="75" x2="550" y2="75"
          stroke="#3498db" stroke-width="4"
          marker-start="url(#timeline-start)"/>

    <text x="50" y="110" text-anchor="middle" font-size="12">Start</text>
    <text x="550" y="110" text-anchor="middle" font-size="12">End</text>
</svg>
```

### Curved Path with Start Marker

```html
<svg width="400" height="300">
    <defs>
        <marker id="curve-start" markerWidth="12" markerHeight="12"
                refX="6" refY="6" orient="auto">
            <circle cx="6" cy="6" r="5" fill="#9b59b6" stroke="#8e44ad" stroke-width="1"/>
            <circle cx="6" cy="6" r="2" fill="white"/>
        </marker>
    </defs>

    <path d="M 50,250 C 50,100 350,100 350,250"
          fill="none" stroke="#34495e" stroke-width="3"
          marker-start="url(#curve-start)"/>
</svg>
```

### Data Flow Source

```html
<svg width="500" height="200">
    <defs>
        <marker id="data-source" markerWidth="24" markerHeight="24"
                refX="12" refY="12">
            <circle cx="12" cy="12" r="10" fill="#3498db" stroke="#2980b9" stroke-width="2"/>
            <path d="M 8,12 L 12,8 L 16,12 L 12,16 Z" fill="white"/>
        </marker>
    </defs>

    <path d="M 50,100 L 150,100 L 200,50 L 300,50 L 350,100 L 450,100"
          fill="none" stroke="#95a5a6" stroke-width="3"
          marker-start="url(#data-source)"/>
</svg>
```

---

## See Also

- [marker-mid](/reference/svgattributes/marker-mid.html) - Marker at middle vertices
- [marker-end](/reference/svgattributes/marker-end.html) - Marker at path end
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
