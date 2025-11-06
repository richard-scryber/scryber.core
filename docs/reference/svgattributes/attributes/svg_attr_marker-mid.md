---
layout: default
title: marker-mid
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @marker-mid : The Path Middle Vertices Marker Attribute

The `marker-mid` attribute specifies a marker graphic to be drawn at all interior vertices (middle points) of a path, polyline, or polygon. This is useful for marking path segments, creating dotted visual effects, or highlighting path vertices.

## Usage

The `marker-mid` attribute is used to:
- Mark all intermediate vertices along a path
- Create decorative effects at path bends and corners
- Visualize path control points
- Add directional indicators at path segments
- Build animated or dashed path effects
- Enhance path visualization with vertex markers

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
        <marker id="mid-dot" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#3498db"/>
        </marker>
    </defs>

    <polyline points="50,150 150,50 250,150 350,50"
              fill="none" stroke="#2c3e50" stroke-width="2"
              marker-mid="url(#mid-dot)"/>
</svg>
```

---

## Supported Values

| Value | Description | Example |
|-------|-------------|---------|
| `none` | No marker (default) | `marker-mid="none"` |
| `url(#id)` | Reference to marker element | `marker-mid="url(#dot)"` |
| `inherit` | Inherit from parent | `marker-mid="inherit"` |

### URL Reference Format

```html
<!-- Reference marker by ID -->
marker-mid="url(#markerID)"

<!-- With fallback -->
marker-mid="url(#markerID) none"
```

---

## Supported Elements

The `marker-mid` attribute is supported on:

- **[&lt;path&gt;](/reference/svgtags/path.html)** - At all interior vertices
- **[&lt;polyline&gt;](/reference/svgtags/polyline.html)** - At all points except first and last
- **[&lt;polygon&gt;](/reference/svgtags/polygon.html)** - At all points except first/last
- **[&lt;line&gt;](/reference/svgtags/line.html)** - Not applicable (no middle vertices)

**Note:** Lines have no middle vertices, so `marker-mid` has no effect on line elements.

---

## Data Binding

### Dynamic Marker Selection

Choose middle marker based on data:

```html
<!-- Model: { pathStyle: 'dotted', markers: { dotted: 'dot-mid', segmented: 'arrow-mid' } } -->
<svg width="400" height="200">
    <defs>
        <marker id="dot-mid" markerWidth="6" markerHeight="6"
                refX="3" refY="3">
            <circle cx="3" cy="3" r="2" fill="#3498db"/>
        </marker>
        <marker id="arrow-mid" markerWidth="8" markerHeight="8"
                refX="4" refY="4" orient="auto">
            <polygon points="0,0 8,4 0,8" fill="#e74c3c"/>
        </marker>
    </defs>

    <polyline points="50,150 150,50 250,150 350,50"
              fill="none" stroke="#2c3e50" stroke-width="2"
              marker-mid="url(#{{model.markers[model.pathStyle]}})"/>
</svg>
```

### Conditional Middle Markers

Show markers based on conditions:

```html
<!-- Model: { showVertices: true, highlightPath: false } -->
<svg width="400" height="200">
    <defs>
        <marker id="vertex-marker" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#f39c12"/>
        </marker>
    </defs>

    <path d="M 50,150 L 150,50 L 250,150 L 350,50"
          fill="none" stroke="#34495e" stroke-width="2"
          marker-mid="{{model.showVertices ? 'url(#vertex-marker)' : 'none'}}"/>
</svg>
```

### Data-Driven Path Visualization

```html
<!-- Model: {
    paths: [
        { points: '50,100 150,50 250,100', showNodes: true },
        { points: '50,150 150,100 250,150', showNodes: false }
    ]
} -->
<svg width="300" height="200">
    <defs>
        <marker id="node-mid" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#9b59b6"/>
        </marker>
    </defs>

    <template data-bind="{{model.paths}}">
        <polyline points="{{.points}}"
                  fill="none" stroke="#2c3e50" stroke-width="2"
                  marker-mid="{{.showNodes ? 'url(#node-mid)' : 'none'}}"/>
    </template>
</svg>
```

---

## Notes

### Marker Placement

- Markers are placed at **all interior vertices** (not at the first or last point)
- For **polyline**: placed at all points except the first and last
- For **polygon**: placed at all vertices except where the path closes
- For **path**: placed at all vertices between path commands
- **Lines** have no middle vertices, so marker-mid has no effect

### Path Command Vertices

For path elements, middle markers appear at vertices between these commands:
- Between consecutive `L` (line) commands
- At points in curve commands (`C`, `Q`, `S`, `T`)
- At arc (`A`) endpoints
- Not at the first `M` (move) point or final point

### Combining Markers

```html
<!-- Use all three marker types together -->
<polyline points="50,150 150,50 250,100 350,150"
          marker-start="url(#start)"
          marker-mid="url(#mid)"
          marker-end="url(#end)"/>
```

### Auto-Orientation

- Use `orient="auto"` on markers to align them with path direction
- Each middle marker orients based on the path angle at that vertex
- Useful for directional indicators along the path

### Performance Impact

- Markers are rendered at every middle vertex
- Paths with many vertices will render many markers
- Consider performance with complex paths
- Simple marker shapes perform better than complex ones

### Vertex Detection

```html
<!-- These points create middle vertices -->
<polyline points="50,50 100,100 150,50 200,100">
    <!-- Markers at: (100,100) and (150,50) -->
    <!-- No markers at first (50,50) or last (200,100) -->
</polyline>
```

### Smooth vs Sharp Vertices

- Markers appear at all defined vertices regardless of smoothness
- Smooth curves (`S`, `T` commands) still have vertices at endpoints
- Bezier control points are not vertices and don't get markers

### Common Use Cases

- **Path visualization**: Showing path construction points
- **Waypoints**: Marking route segments in maps
- **Decorative effects**: Creating ornamental path designs
- **Animation guides**: Indicating movement points
- **Process steps**: Marking intermediate stages

---

## Examples

### Simple Vertex Dots

```html
<svg width="400" height="200">
    <defs>
        <marker id="vertex-dot" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#e74c3c"/>
        </marker>
    </defs>

    <polyline points="50,150 120,50 200,120 280,50 350,150"
              fill="none" stroke="#34495e" stroke-width="2"
              marker-mid="url(#vertex-dot)"/>
</svg>
```

### Directional Mid Markers

```html
<svg width="400" height="200">
    <defs>
        <marker id="arrow-mid" markerWidth="10" markerHeight="10"
                refX="5" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
    </defs>

    <path d="M 50,100 L 150,50 L 250,150 L 350,100"
          fill="none" stroke="#2c3e50" stroke-width="2"
          marker-mid="url(#arrow-mid)"/>
</svg>
```

### Highlighted Vertices

```html
<svg width="400" height="200">
    <defs>
        <marker id="highlight" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <circle cx="6" cy="6" r="5" fill="#f39c12" stroke="#e67e22" stroke-width="1"/>
        </marker>
    </defs>

    <polyline points="50,100 100,50 150,120 200,40 250,100 300,60 350,120"
              fill="none" stroke="#95a5a6" stroke-width="2"
              marker-mid="url(#highlight)"/>
</svg>
```

### Diamond Mid Markers

```html
<svg width="400" height="200">
    <defs>
        <marker id="diamond-mid" markerWidth="10" markerHeight="10"
                refX="5" refY="5" orient="auto">
            <path d="M 5,0 L 10,5 L 5,10 L 0,5 Z"
                  fill="#9b59b6" stroke="#8e44ad" stroke-width="1"/>
        </marker>
    </defs>

    <polyline points="50,150 120,80 200,140 280,70 350,150"
              fill="none" stroke="#34495e" stroke-width="2"
              marker-mid="url(#diamond-mid)"/>
</svg>
```

### Numbered Waypoints

```html
<svg width="400" height="200">
    <defs>
        <marker id="waypoint-1" markerWidth="20" markerHeight="20"
                refX="10" refY="10">
            <circle cx="10" cy="10" r="8" fill="#3498db" stroke="#2980b9" stroke-width="2"/>
            <text x="10" y="14" text-anchor="middle" font-size="10" fill="white" font-weight="bold">1</text>
        </marker>
        <marker id="waypoint-2" markerWidth="20" markerHeight="20"
                refX="10" refY="10">
            <circle cx="10" cy="10" r="8" fill="#3498db" stroke="#2980b9" stroke-width="2"/>
            <text x="10" y="14" text-anchor="middle" font-size="10" fill="white" font-weight="bold">2</text>
        </marker>
    </defs>

    <polyline points="50,100 200,50 350,100"
              fill="none" stroke="#95a5a6" stroke-width="2"
              marker-mid="url(#waypoint-1)"/>
</svg>
```

### Curve Control Points Visualization

```html
<svg width="400" height="250">
    <defs>
        <marker id="control-point" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#2ecc71" stroke="#27ae60" stroke-width="1"/>
        </marker>
    </defs>

    <!-- Bezier curve -->
    <path d="M 50,200 C 100,50 300,50 350,200"
          fill="none" stroke="#3498db" stroke-width="3"/>

    <!-- Control point lines -->
    <polyline points="50,200 100,50 300,50 350,200"
              fill="none" stroke="#e74c3c" stroke-width="1" stroke-dasharray="5,5"
              marker-mid="url(#control-point)"/>
</svg>
```

### All Three Marker Types

```html
<svg width="400" height="200">
    <defs>
        <marker id="start-m" markerWidth="10" markerHeight="10"
                refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#2ecc71"/>
        </marker>
        <marker id="mid-m" markerWidth="8" markerHeight="8"
                refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#3498db"/>
        </marker>
        <marker id="end-m" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <polyline points="50,150 120,50 200,120 280,50 350,150"
              fill="none" stroke="#34495e" stroke-width="2"
              marker-start="url(#start-m)"
              marker-mid="url(#mid-m)"
              marker-end="url(#end-m)"/>
</svg>
```

### Route Waypoints

```html
<svg width="500" height="300">
    <defs>
        <marker id="waypoint" markerWidth="16" markerHeight="16"
                refX="8" refY="8">
            <circle cx="8" cy="8" r="6" fill="#3498db" stroke="#2980b9" stroke-width="2"/>
            <circle cx="8" cy="8" r="2" fill="white"/>
        </marker>
    </defs>

    <polyline points="50,250 100,150 180,200 260,100 340,180 420,120 450,220"
              fill="none" stroke="#95a5a6" stroke-width="3"
              marker-mid="url(#waypoint)"/>

    <!-- Labels -->
    <text x="50" y="270" text-anchor="middle" font-size="12">Start</text>
    <text x="450" y="240" text-anchor="middle" font-size="12">End</text>
</svg>
```

### Decorative Path Pattern

```html
<svg width="400" height="200">
    <defs>
        <marker id="decorative" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <path d="M 6,0 L 12,6 L 6,12 L 0,6 Z"
                  fill="#f1c40f" stroke="#f39c12" stroke-width="1"/>
        </marker>
    </defs>

    <path d="M 50,100 Q 100,50 150,100 T 250,100 T 350,100"
          fill="none" stroke="#34495e" stroke-width="2"
          marker-mid="url(#decorative)"/>
</svg>
```

### Network Connection Points

```html
<svg width="400" height="300">
    <defs>
        <marker id="connection" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <rect x="1" y="1" width="10" height="10"
                  fill="#e74c3c" stroke="#c0392b" stroke-width="1" rx="2"/>
        </marker>
    </defs>

    <!-- Network paths -->
    <polyline points="50,50 200,100 350,50"
              fill="none" stroke="#95a5a6" stroke-width="2"
              marker-mid="url(#connection)"/>
    <polyline points="50,150 200,100 350,150"
              fill="none" stroke="#95a5a6" stroke-width="2"
              marker-mid="url(#connection)"/>
    <polyline points="50,250 200,200 350,250"
              fill="none" stroke="#95a5a6" stroke-width="2"
              marker-mid="url(#connection)"/>
</svg>
```

### Gradient Mid Markers

```html
<svg width="400" height="200">
    <defs>
        <radialGradient id="marker-grad">
            <stop offset="0%" stop-color="#667eea"/>
            <stop offset="100%" stop-color="#764ba2"/>
        </radialGradient>

        <marker id="grad-mid" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <circle cx="6" cy="6" r="5" fill="url(#marker-grad)"/>
        </marker>
    </defs>

    <polyline points="50,150 120,70 200,130 280,60 350,140"
              fill="none" stroke="#34495e" stroke-width="3"
              marker-mid="url(#grad-mid)"/>
</svg>
```

### Path Segment Indicators

```html
<svg width="400" height="200">
    <defs>
        <marker id="segment" markerWidth="10" markerHeight="10"
                refX="5" refY="5" orient="auto">
            <path d="M 0,2 L 5,5 L 0,8" fill="none" stroke="#3498db" stroke-width="2" stroke-linecap="round"/>
        </marker>
    </defs>

    <polyline points="50,100 120,50 200,120 280,70 350,130"
              fill="none" stroke="#95a5a6" stroke-width="3"
              marker-mid="url(#segment)"/>
</svg>
```

### Star Vertices

```html
<svg width="400" height="200">
    <defs>
        <marker id="star-vertex" markerWidth="14" markerHeight="14"
                refX="7" refY="7">
            <path d="M 7,1 L 9,6 L 13,6 L 10,9 L 11,13 L 7,10 L 3,13 L 4,9 L 1,6 L 5,6 Z"
                  fill="#f39c12" stroke="#e67e22" stroke-width="0.5"/>
        </marker>
    </defs>

    <polyline points="50,150 120,60 200,140 280,50 350,130"
              fill="none" stroke="#34495e" stroke-width="2"
              marker-mid="url(#star-vertex)"/>
</svg>
```

### Process Flow Steps

```html
<svg width="600" height="200">
    <defs>
        <marker id="step" markerWidth="24" markerHeight="24"
                refX="12" refY="12">
            <circle cx="12" cy="12" r="10" fill="#3498db" stroke="#2980b9" stroke-width="2"/>
            <circle cx="12" cy="12" r="4" fill="#2ecc71"/>
        </marker>
    </defs>

    <path d="M 50,100 L 200,100 L 250,150 L 400,150 L 450,100 L 550,100"
          fill="none" stroke="#95a5a6" stroke-width="3"
          marker-mid="url(#step)"/>

    <text x="300" y="50" text-anchor="middle" font-size="14" font-weight="bold">Process Flow</text>
</svg>
```

### Hollow Circle Mid Markers

```html
<svg width="400" height="200">
    <defs>
        <marker id="hollow" markerWidth="12" markerHeight="12"
                refX="6" refY="6">
            <circle cx="6" cy="6" r="5" fill="white" stroke="#e74c3c" stroke-width="2"/>
        </marker>
    </defs>

    <polyline points="50,100 120,50 200,120 280,60 350,130"
              fill="none" stroke="#34495e" stroke-width="2"
              marker-mid="url(#hollow)"/>
</svg>
```

---

## See Also

- [marker-start](/reference/svgattributes/marker-start.html) - Marker at path start
- [marker-end](/reference/svgattributes/marker-end.html) - Marker at path end
- [marker element](/reference/svgtags/marker.html) - Marker definition element
- [markerWidth](/reference/svgattributes/markerWidth.html) - Marker viewport width
- [markerHeight](/reference/svgattributes/markerHeight.html) - Marker viewport height
- [refX](/reference/svgattributes/refX.html) - Marker reference X position
- [refY](/reference/svgattributes/refY.html) - Marker reference Y position
- [orient](/reference/svgattributes/orient.html) - Marker orientation
- [path element](/reference/svgtags/path.html) - SVG path element
- [polyline element](/reference/svgtags/polyline.html) - SVG polyline element
- [Data Binding](/reference/binding/) - Data binding and expressions

---
