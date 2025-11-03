---
layout: default
title: marker (SVG)
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;marker&gt; : The SVG Marker Element

The `<marker>` element defines graphical markers that can be attached to paths, lines, polygons, and polylines. Markers are automatically positioned and oriented at vertices and endpoints of shapes, commonly used for arrowheads, dots, and other path decorations.

---

## Summary

The `<marker>` element creates reusable graphics that can be placed at the start, middle, or end of path segments. Markers automatically rotate to align with the path direction and scale according to stroke width. They are essential for creating arrows, connection indicators, and decorative path endpoints.

Key features:
- Define reusable marker graphics
- Automatic positioning at path vertices and endpoints
- Auto-rotation to match path direction
- Reference point control with refX and refY
- Size specification with markerWidth and markerHeight
- ViewBox support for scaling
- Apply to paths, lines, polylines, and polygons
- Data binding for dynamic marker content

---

## Usage

Markers are defined in the `<defs>` section and referenced by shapes using marker attributes:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
        <marker id="arrow" markerWidth="10" markerHeight="10" refX="5" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#336699"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#336699" stroke-width="2"
          marker-end="url(#arrow)"/>
</svg>
```

### Basic Syntax

```html
<!-- Simple arrowhead marker -->
<marker id="arrowhead" markerWidth="10" markerHeight="10" refX="10" refY="5" orient="auto">
    <polygon points="0,0 10,5 0,10" fill="black"/>
</marker>

<!-- Circular dot marker -->
<marker id="dot" markerWidth="8" markerHeight="8" refX="4" refY="4">
    <circle cx="4" cy="4" r="3" fill="red"/>
</marker>

<!-- Marker with viewBox -->
<marker id="scalableArrow" markerWidth="6" markerHeight="6"
        refX="3" refY="3" orient="auto"
        viewBox="0 0 10 10">
    <path d="M 0 0 L 10 5 L 0 10 Z" fill="blue"/>
</marker>

<!-- Marker with specific orientation -->
<marker id="diamond" markerWidth="10" markerHeight="10"
        refX="5" refY="5" orient="45">
    <rect x="2" y="2" width="6" height="6" fill="green"/>
</marker>
```

### Applying Markers

```html
<!-- Arrow at end of line -->
<line x1="50" y1="50" x2="200" y2="50" stroke="black" marker-end="url(#arrow)"/>

<!-- Dots at both ends -->
<line x1="50" y1="100" x2="200" y2="100" stroke="black"
      marker-start="url(#dot)" marker-end="url(#dot)"/>

<!-- Markers at all vertices -->
<polyline points="50,150 100,120 150,140 200,150" stroke="black" fill="none"
          marker-start="url(#dot)" marker-mid="url(#dot)" marker-end="url(#arrow)"/>

<!-- Markers on path -->
<path d="M 50 200 Q 100 150 200 200" stroke="black" fill="none"
      marker-start="url(#dot)" marker-end="url(#arrow)"/>
```

---

## Supported Attributes

### Identification Attribute

| Attribute | Type | Description | Required |
|-----------|------|-------------|----------|
| `id` | String | Unique identifier for referencing the marker | Yes |

### Size Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `markerWidth` | Unit | Width of the marker viewport | 3 |
| `markerHeight` | Unit | Height of the marker viewport | 3 |

### Reference Point Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `refX` | Unit | X coordinate of the marker reference point | 0 |
| `refY` | Unit | Y coordinate of the marker reference point | 0 |

The reference point is where the marker is positioned on the path.

### Orientation Attribute

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `orient` | String/Number | Marker orientation: `auto`, `auto-start-reverse`, or angle in degrees | 0 |

- `auto` - Automatically orient to match path direction
- `auto-start-reverse` - Like auto, but reversed at path start
- Numeric value - Fixed angle in degrees

### ViewBox Attribute

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `viewBox` | Rect | Coordinate system for marker content | none |

Format: `minX minY width height`

### Units Attribute

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `markerUnits` | Enum | Coordinate system: `strokeWidth`, `userSpaceOnUse` | strokeWidth |

- `strokeWidth` - Scale with stroke width
- `userSpaceOnUse` - Use SVG coordinate system

### Common Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `class` | String | CSS class name(s) for styling |
| `style` | Style | Inline CSS-style properties |
| `title` | String | Tooltip or title text |
| `desc` | String | Description for accessibility |

---

## Data Binding

Markers support data binding for dynamic appearance and behavior.

### Dynamic Marker Content

```html
<!-- Marker with data-driven color -->
<defs>
    <marker id="statusMarker" markerWidth="10" markerHeight="10" refX="5" refY="5">
        <circle cx="5" cy="5" r="4" fill="{{model.statusColor}}"/>
    </marker>
</defs>

<!-- Conditional marker shape -->
<defs>
    <marker id="dynamicMarker" markerWidth="12" markerHeight="12" refX="6" refY="6" orient="auto">
        <polygon points="{{model.useArrow ? '0,0 12,6 0,12' : '3,3 9,3 9,9 3,9'}}"
                 fill="{{model.markerColor}}"/>
    </marker>
</defs>
```

### Size Based on Data

```html
<!-- Variable marker size -->
<defs>
    <marker id="sizedMarker"
            markerWidth="{{model.markerSize}}"
            markerHeight="{{model.markerSize}}"
            refX="{{model.markerSize / 2}}"
            refY="{{model.markerSize / 2}}">
        <circle cx="{{model.markerSize / 2}}"
                cy="{{model.markerSize / 2}}"
                r="{{model.markerSize / 2 - 1}}"
                fill="#336699"/>
    </marker>
</defs>
```

### Template-Generated Markers

```html
<!-- Create multiple marker variations -->
<defs>
    <template data-bind="{{model.markerTypes}}">
        <marker id="marker-{{.id}}"
                markerWidth="10"
                markerHeight="10"
                refX="5"
                refY="5"
                orient="auto">
            <circle cx="5" cy="5" r="4" fill="{{.color}}"/>
        </marker>
    </template>
</defs>

<!-- Use markers dynamically -->
<template data-bind="{{model.connections}}">
    <line x1="{{.x1}}" y1="{{.y1}}" x2="{{.x2}}" y2="{{.y2}}"
          stroke="{{.color}}" stroke-width="2"
          marker-end="url(#marker-{{.markerId}})"/>
</template>
```

---

## Notes

### Marker Positioning

- The reference point (`refX`, `refY`) determines where the marker attaches to the path
- The reference point is positioned at path vertices or endpoints
- For arrowheads, typically set refX to the tip position
- For centered markers, set refX and refY to the center

### Marker Orientation

- `orient="auto"` automatically rotates the marker to align with path direction
- Numeric orient values specify fixed rotation angles
- At path endpoints, orientation matches the path tangent
- At path vertices, orientation is typically the average of adjacent segments

### Marker Units

The `markerUnits` attribute affects marker sizing:
- `strokeWidth` (default) - Marker scales with the stroke width of the shape
- `userSpaceOnUse` - Marker uses the SVG coordinate system directly

### ViewBox Scaling

When a `viewBox` is specified:
- The marker content is scaled to fit the marker viewport
- Useful for creating resolution-independent markers
- Maintains aspect ratio of marker graphics
- Allows reuse of same marker at different sizes

### Marker Application

Markers can be applied using three attributes on shapes:
- `marker-start` - Applied at the first vertex
- `marker-mid` - Applied at all middle vertices
- `marker-end` - Applied at the last vertex

### Performance

- Define markers once in `<defs>` and reuse with URL references
- Markers are rendered efficiently as they are cached
- Complex marker graphics may impact rendering performance
- Consider simplifying marker geometry for better performance

### Limitations

- Markers are only supported on path-based elements (path, line, polyline, polygon)
- Not supported on basic shapes like rect, circle, ellipse
- Marker context attributes have limited support
- Some advanced marker features may not be fully implemented

### Common Use Cases

- Arrow indicators for flow diagrams
- Dots or circles at data points on charts
- Connection endpoints for network diagrams
- Decorative elements on paths
- Direction indicators on maps

---

## Examples

### 1. Simple Arrowhead

Basic arrow marker at line end:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
    <defs>
        <marker id="arrow" markerWidth="10" markerHeight="10" refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#336699"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="250" y2="50"
          stroke="#336699" stroke-width="2"
          marker-end="url(#arrow)"/>
</svg>
```

### 2. Dot Markers

Circular dots at line endpoints:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
    <defs>
        <marker id="dot" markerWidth="8" markerHeight="8" refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#cc0000"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="250" y2="50"
          stroke="#333" stroke-width="2"
          marker-start="url(#dot)" marker-end="url(#dot)"/>
</svg>
```

### 3. Bidirectional Arrow

Arrows at both ends of a line:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
    <defs>
        <marker id="arrowStart" markerWidth="10" markerHeight="10" refX="0" refY="5" orient="auto">
            <polygon points="10,0 0,5 10,10" fill="#336699"/>
        </marker>
        <marker id="arrowEnd" markerWidth="10" markerHeight="10" refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#336699"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="250" y2="50"
          stroke="#336699" stroke-width="2"
          marker-start="url(#arrowStart)" marker-end="url(#arrowEnd)"/>
</svg>
```

### 4. Polyline with Mid-Markers

Dots at all vertices:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <defs>
        <marker id="vertex" markerWidth="8" markerHeight="8" refX="4" refY="4">
            <circle cx="4" cy="4" r="3" fill="#ff6600"/>
        </marker>
    </defs>

    <polyline points="50,150 100,50 200,100 300,80 350,150"
              stroke="#336699" stroke-width="2" fill="none"
              marker-start="url(#vertex)"
              marker-mid="url(#vertex)"
              marker-end="url(#vertex)"/>
</svg>
```

### 5. Custom Shape Marker

Diamond-shaped marker:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
    <defs>
        <marker id="diamond" markerWidth="10" markerHeight="10" refX="5" refY="5" orient="auto">
            <path d="M 5 0 L 10 5 L 5 10 L 0 5 Z" fill="#00aa00"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="250" y2="50"
          stroke="#00aa00" stroke-width="2"
          marker-end="url(#diamond)"/>
</svg>
```

### 6. Marker with ViewBox

Scalable marker definition:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="150">
    <defs>
        <marker id="scalable" markerWidth="6" markerHeight="6"
                refX="5" refY="5" orient="auto"
                viewBox="0 0 10 10">
            <path d="M 0 0 L 10 5 L 0 10 Z" fill="#336699"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="250" y2="50"
          stroke="#336699" stroke-width="1"
          marker-end="url(#scalable)"/>

    <line x1="50" y1="100" x2="250" y2="100"
          stroke="#336699" stroke-width="4"
          marker-end="url(#scalable)"/>
</svg>
```

### 7. Flow Diagram Arrows

Process flow with directional arrows:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="200">
    <defs>
        <marker id="flowArrow" markerWidth="12" markerHeight="12" refX="12" refY="6" orient="auto">
            <polygon points="0,0 12,6 0,12 2,6" fill="#336699"/>
        </marker>
    </defs>

    <!-- Process boxes -->
    <rect x="20" y="70" width="100" height="60" rx="5" fill="#f0f0f0" stroke="#336699" stroke-width="2"/>
    <text x="70" y="105" text-anchor="middle" font-size="14">Start</text>

    <rect x="200" y="70" width="100" height="60" rx="5" fill="#f0f0f0" stroke="#336699" stroke-width="2"/>
    <text x="250" y="105" text-anchor="middle" font-size="14">Process</text>

    <rect x="380" y="70" width="100" height="60" rx="5" fill="#f0f0f0" stroke="#336699" stroke-width="2"/>
    <text x="430" y="105" text-anchor="middle" font-size="14">End</text>

    <!-- Connecting arrows -->
    <line x1="120" y1="100" x2="200" y2="100"
          stroke="#336699" stroke-width="2"
          marker-end="url(#flowArrow)"/>

    <line x1="300" y1="100" x2="380" y2="100"
          stroke="#336699" stroke-width="2"
          marker-end="url(#flowArrow)"/>
</svg>
```

### 8. Data Point Markers

Chart with colored data point markers:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="250">
    <defs>
        <marker id="datapoint" markerWidth="10" markerHeight="10" refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="white" stroke="#336699" stroke-width="2"/>
        </marker>
    </defs>

    <!-- Chart axes -->
    <line x1="50" y1="200" x2="350" y2="200" stroke="#999" stroke-width="1"/>
    <line x1="50" y1="200" x2="50" y2="50" stroke="#999" stroke-width="1"/>

    <!-- Data line with markers -->
    <polyline points="50,180 100,150 150,120 200,140 250,90 300,110 350,80"
              stroke="#336699" stroke-width="2" fill="none"
              marker-mid="url(#datapoint)"
              marker-end="url(#datapoint)"/>
</svg>
```

### 9. Network Diagram Connections

Network nodes with connection arrows:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <marker id="connection" markerWidth="8" markerHeight="8" refX="8" refY="4" orient="auto">
            <polygon points="0,0 8,4 0,8" fill="#666"/>
        </marker>
    </defs>

    <!-- Nodes -->
    <circle cx="100" cy="100" r="30" fill="#336699"/>
    <text x="100" y="105" text-anchor="middle" fill="white" font-weight="700">A</text>

    <circle cx="300" cy="100" r="30" fill="#336699"/>
    <text x="300" y="105" text-anchor="middle" fill="white" font-weight="700">B</text>

    <circle cx="200" cy="220" r="30" fill="#336699"/>
    <text x="200" y="225" text-anchor="middle" fill="white" font-weight="700">C</text>

    <!-- Connections -->
    <line x1="130" y1="100" x2="270" y2="100"
          stroke="#666" stroke-width="2"
          marker-end="url(#connection)"/>

    <line x1="120" y1="120" x2="180" y2="200"
          stroke="#666" stroke-width="2"
          marker-end="url(#connection)"/>

    <line x1="280" y1="120" x2="220" y2="200"
          stroke="#666" stroke-width="2"
          marker-end="url(#connection)"/>
</svg>
```

### 10. Dynamic Status Markers

Data-driven marker colors:

```html
<!-- Model: { connections: [{x1: 50, y1: 50, x2: 200, y2: 50, status: "success"}] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="200">
    <defs>
        <marker id="successMarker" markerWidth="10" markerHeight="10" refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#00aa00"/>
        </marker>
        <marker id="errorMarker" markerWidth="10" markerHeight="10" refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#cc0000"/>
        </marker>
    </defs>

    <template data-bind="{{model.connections}}">
        <line x1="{{.x1}}" y1="{{.y1}}" x2="{{.x2}}" y2="{{.y2}}"
              stroke="{{.status === 'success' ? '#00aa00' : '#cc0000'}}"
              stroke-width="2"
              marker-end="{{.status === 'success' ? 'url(#successMarker)' : 'url(#errorMarker)'}}"/>
    </template>
</svg>
```

### 11. Curved Path with Arrow

Bezier curve with arrow marker:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="200">
    <defs>
        <marker id="curveArrow" markerWidth="10" markerHeight="10" refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#ff6600"/>
        </marker>
    </defs>

    <path d="M 50 150 Q 150 50 250 150"
          stroke="#ff6600" stroke-width="3" fill="none"
          marker-end="url(#curveArrow)"/>
</svg>
```

### 12. Multi-Colored Markers

Different marker colors at start and end:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
    <defs>
        <marker id="greenDot" markerWidth="10" markerHeight="10" refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#00aa00"/>
        </marker>
        <marker id="redDot" markerWidth="10" markerHeight="10" refX="5" refY="5">
            <circle cx="5" cy="5" r="4" fill="#cc0000"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="250" y2="50"
          stroke="#666" stroke-width="2"
          marker-start="url(#greenDot)" marker-end="url(#redDot)"/>

    <text x="50" y="75" text-anchor="middle" font-size="11" fill="#00aa00">Start</text>
    <text x="250" y="75" text-anchor="middle" font-size="11" fill="#cc0000">End</text>
</svg>
```

### 13. Square Marker

Square-shaped marker at path end:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
    <defs>
        <marker id="square" markerWidth="10" markerHeight="10" refX="5" refY="5" orient="auto">
            <rect x="1" y="1" width="8" height="8" fill="#336699"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="250" y2="50"
          stroke="#336699" stroke-width="2"
          marker-end="url(#square)"/>
</svg>
```

### 14. Star Marker

Star-shaped decorative marker:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
    <defs>
        <marker id="star" markerWidth="12" markerHeight="12" refX="6" refY="6">
            <path d="M 6 0 L 7.5 4.5 L 12 4.5 L 8.5 7.5 L 10 12 L 6 9 L 2 12 L 3.5 7.5 L 0 4.5 L 4.5 4.5 Z"
                  fill="#ff6600"/>
        </marker>
    </defs>

    <polyline points="50,50 100,70 150,50 200,65 250,50"
              stroke="#666" stroke-width="2" fill="none"
              marker-mid="url(#star)" marker-end="url(#star)"/>
</svg>
```

### 15. Triangle Marker

Filled triangle marker:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
    <defs>
        <marker id="triangle" markerWidth="10" markerHeight="10" refX="5" refY="5" orient="auto">
            <path d="M 0 5 L 10 0 L 10 10 Z" fill="#00aa00"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="250" y2="50"
          stroke="#00aa00" stroke-width="2"
          marker-end="url(#triangle)"/>
</svg>
```

### 16. Timeline Markers

Timeline with event markers:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="150">
    <defs>
        <marker id="timelineDot" markerWidth="12" markerHeight="12" refX="6" refY="6">
            <circle cx="6" cy="6" r="5" fill="white" stroke="#336699" stroke-width="2"/>
        </marker>
    </defs>

    <!-- Timeline line -->
    <line x1="50" y1="75" x2="450" y2="75"
          stroke="#336699" stroke-width="3"
          marker-start="url(#timelineDot)"
          marker-end="url(#timelineDot)"/>

    <!-- Event markers -->
    <circle cx="150" cy="75" r="6" fill="white" stroke="#336699" stroke-width="2"/>
    <text x="150" y="100" text-anchor="middle" font-size="11">Event 1</text>

    <circle cx="250" cy="75" r="6" fill="white" stroke="#336699" stroke-width="2"/>
    <text x="250" y="100" text-anchor="middle" font-size="11">Event 2</text>

    <circle cx="350" cy="75" r="6" fill="white" stroke="#336699" stroke-width="2"/>
    <text x="350" y="100" text-anchor="middle" font-size="11">Event 3</text>
</svg>
```

### 17. Dashed Line with Arrows

Dashed connection with arrows:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="100">
    <defs>
        <marker id="dashedArrow" markerWidth="10" markerHeight="10" refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#999"/>
        </marker>
    </defs>

    <line x1="50" y1="50" x2="250" y2="50"
          stroke="#999" stroke-width="2" stroke-dasharray="5,5"
          marker-end="url(#dashedArrow)"/>
</svg>
```

### 18. Percentage Indicator

Progress indicator with markers:

```html
<!-- Model: { progress: 0.75 } -->
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="100">
    <defs>
        <marker id="progressDot" markerWidth="14" markerHeight="14" refX="7" refY="7">
            <circle cx="7" cy="7" r="6" fill="#336699"/>
        </marker>
    </defs>

    <!-- Background line -->
    <line x1="50" y1="50" x2="350" y2="50" stroke="#ddd" stroke-width="8"/>

    <!-- Progress line -->
    <line x1="50" y1="50" x2="{{50 + model.progress * 300}}" y2="50"
          stroke="#336699" stroke-width="8"
          marker-end="url(#progressDot)"/>

    <text x="200" y="80" text-anchor="middle" font-size="16">
        {{(model.progress * 100).toFixed(0)}}% Complete
    </text>
</svg>
```

### 19. Hierarchical Tree Connectors

Tree structure with connection markers:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <marker id="treeArrow" markerWidth="8" markerHeight="8" refX="8" refY="4" orient="auto">
            <polygon points="0,0 8,4 0,8" fill="#336699"/>
        </marker>
    </defs>

    <!-- Root -->
    <rect x="175" y="20" width="50" height="30" rx="3" fill="#336699"/>
    <text x="200" y="40" text-anchor="middle" fill="white" font-size="12">Root</text>

    <!-- Children -->
    <rect x="75" y="120" width="50" height="30" rx="3" fill="#336699"/>
    <text x="100" y="140" text-anchor="middle" fill="white" font-size="12">Child 1</text>

    <rect x="175" y="120" width="50" height="30" rx="3" fill="#336699"/>
    <text x="200" y="140" text-anchor="middle" fill="white" font-size="12">Child 2</text>

    <rect x="275" y="120" width="50" height="30" rx="3" fill="#336699"/>
    <text x="300" y="140" text-anchor="middle" fill="white" font-size="12">Child 3</text>

    <!-- Connections -->
    <line x1="200" y1="50" x2="100" y2="120"
          stroke="#336699" stroke-width="2"
          marker-end="url(#treeArrow)"/>

    <line x1="200" y1="50" x2="200" y2="120"
          stroke="#336699" stroke-width="2"
          marker-end="url(#treeArrow)"/>

    <line x1="200" y1="50" x2="300" y2="120"
          stroke="#336699" stroke-width="2"
          marker-end="url(#treeArrow)"/>
</svg>
```

### 20. Complex Multi-Style Markers

Multiple marker types in one diagram:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="300">
    <defs>
        <!-- Start marker -->
        <marker id="start" markerWidth="12" markerHeight="12" refX="6" refY="6">
            <circle cx="6" cy="6" r="5" fill="#00aa00"/>
        </marker>

        <!-- Process marker -->
        <marker id="process" markerWidth="10" markerHeight="10" refX="5" refY="5">
            <rect x="1" y="1" width="8" height="8" fill="#336699"/>
        </marker>

        <!-- Decision marker -->
        <marker id="decision" markerWidth="12" markerHeight="12" refX="6" refY="6" orient="auto">
            <path d="M 6 0 L 12 6 L 6 12 L 0 6 Z" fill="#ff6600"/>
        </marker>

        <!-- End marker -->
        <marker id="end" markerWidth="10" markerHeight="10" refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#cc0000"/>
        </marker>
    </defs>

    <!-- Workflow paths -->
    <polyline points="50,50 150,50"
              stroke="#00aa00" stroke-width="2" fill="none"
              marker-start="url(#start)" marker-end="url(#process)"/>

    <polyline points="150,50 150,100 200,100"
              stroke="#336699" stroke-width="2" fill="none"
              marker-end="url(#decision)"/>

    <polyline points="200,100 300,100 300,150"
              stroke="#ff6600" stroke-width="2" fill="none"
              marker-end="url(#process)"/>

    <polyline points="300,150 400,150"
              stroke="#cc0000" stroke-width="2" fill="none"
              marker-end="url(#end)"/>

    <!-- Labels -->
    <text x="100" y="40" text-anchor="middle" font-size="11" fill="#666">Start</text>
    <text x="175" y="90" text-anchor="middle" font-size="11" fill="#666">Process</text>
    <text x="250" y="90" text-anchor="middle" font-size="11" fill="#666">Decision</text>
    <text x="350" y="140" text-anchor="middle" font-size="11" fill="#666">Complete</text>
</svg>
```

---

## See Also

- [path element](/reference/svgtags/path.html) - Path element for marker application
- [line element](/reference/svgtags/line.html) - Line element for marker application
- [polyline element](/reference/svgtags/polyline.html) - Polyline element with markers
- [polygon element](/reference/svgtags/polygon.html) - Polygon element with markers
- [defs element](/reference/svgtags/defs.html) - Definitions container for markers
- [Data Binding](/reference/binding/) - Complete data binding guide
- [SVG Styling](/reference/svg/styling/) - SVG style reference

---
