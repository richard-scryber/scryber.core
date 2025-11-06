---
layout: default
title: markerUnits
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @markerUnits : The Marker Coordinate System Attribute

The `markerUnits` attribute defines the coordinate system for the `markerWidth`, `markerHeight`, and marker content. It determines whether the marker scales with the stroke width of the shape it marks or uses absolute document coordinates.

## Usage

The `markerUnits` attribute is used to:
- Scale markers proportionally with stroke width
- Create size-responsive arrowheads and markers
- Maintain consistent marker sizes across different stroke widths
- Support data-driven coordinate system selection
- Build adaptive markers that scale with line thickness
- Control marker sizing behavior

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <!-- Marker scales with stroke width -->
        <marker id="stroke-scaled" markerWidth="3" markerHeight="3"
                refX="1.5" refY="1.5" markerUnits="strokeWidth">
            <circle cx="1.5" cy="1.5" r="1" fill="#3498db"/>
        </marker>

        <!-- Marker uses absolute size -->
        <marker id="absolute" markerWidth="10" markerHeight="10"
                refX="5" refY="5" markerUnits="userSpaceOnUse">
            <circle cx="5" cy="5" r="4" fill="#e74c3c"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#stroke-scaled)"/>
    <line x1="50" y1="200" x2="350" y2="200"
          stroke="#2c3e50" stroke-width="6"
          marker-end="url(#stroke-scaled)"/>
</svg>
```

---

## Supported Values

| Value | Description | Sizing Behavior | Use Case |
|-------|-------------|-----------------|----------|
| `strokeWidth` | Scaled by stroke width (default) | Marker size = markerWidth × strokeWidth | Proportional markers |
| `userSpaceOnUse` | Absolute document units | Marker size = markerWidth in user units | Fixed-size markers |

### Default Behavior

```html
<!-- These are equivalent (strokeWidth is default) -->
<marker id="m1" markerWidth="3" markerHeight="3">
<marker id="m2" markerWidth="3" markerHeight="3" markerUnits="strokeWidth">
```

---

## Supported Elements

The `markerUnits` attribute is supported on:

- **[&lt;marker&gt;](/reference/svgtags/marker.html)** - Marker element coordinate system

---

## Data Binding

### Dynamic Coordinate System Selection

Choose coordinate system based on data:

```html
<!-- Model: { useStrokeScaling: true, strokeWidth: 4 } -->
<svg width="400" height="200">
    <defs>
        <marker id="dynamicUnits" markerWidth="3" markerHeight="3"
                refX="1.5" refY="1.5"
                markerUnits="{{model.useStrokeScaling ? 'strokeWidth' : 'userSpaceOnUse'}}"
                orient="auto">
            <polygon points="0,0 3,1.5 0,3" fill="#3498db"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#2c3e50" stroke-width="{{model.strokeWidth}}"
          marker-end="url(#dynamicUnits)"/>
</svg>
```

### Adaptive Marker Sizing

Switch between proportional and fixed sizing:

```html
<!-- Model: { lines: [{width: 2, adaptive: true}, {width: 6, adaptive: true}, {width: 10, adaptive: false}] } -->
<svg width="400" height="400">
    <defs>
        <marker id="adaptive-marker" markerWidth="3" markerHeight="3"
                refX="1.5" refY="1.5" markerUnits="strokeWidth" orient="auto">
            <polygon points="0,0 3,1.5 0,3" fill="#2ecc71"/>
        </marker>
        <marker id="fixed-marker" markerWidth="12" markerHeight="12"
                refX="6" refY="6" markerUnits="userSpaceOnUse" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="#e74c3c"/>
        </marker>
    </defs>

    <template data-bind="{{model.lines}}">
        <line x1="50" y1="{{$index * 100 + 100}}"
              x2="350" y2="{{$index * 100 + 100}}"
              stroke="#34495e" stroke-width="{{.width}}"
              marker-end="{{.adaptive ? 'url(#adaptive-marker)' : 'url(#fixed-marker)'}}"/>
        <text x="370" y="{{$index * 100 + 105}}" font-size="12">
            {{.adaptive ? 'Adaptive' : 'Fixed'}} (stroke: {{.width}}px)
        </text>
    </template>
</svg>
```

### Chart Visualization with Consistent Markers

Use userSpaceOnUse for consistent marker sizes across varying line widths:

```html
<!-- Model: { dataPoints: [{x: 50, y: 150, weight: 2}, {x: 150, y: 100, weight: 4}, {x: 250, y: 130, weight: 3}, {x: 350, y: 80, weight: 6}] } -->
<svg width="400" height="250">
    <defs>
        <marker id="dataPoint" markerWidth="10" markerHeight="10"
                refX="5" refY="5" markerUnits="userSpaceOnUse">
            <circle cx="5" cy="5" r="4" fill="#3498db" stroke="white" stroke-width="1"/>
        </marker>
    </defs>

    <template data-bind="{{model.dataPoints}}">
        <!-- Lines with varying widths but consistent marker size -->
        <line x1="{{.x}}" y1="200" x2="{{.x}}" y2="{{.y}}"
              stroke="#95a5a6" stroke-width="{{.weight}}"
              marker-end="url(#dataPoint)"/>
    </template>
</svg>
```

### Responsive Arrow Sizing

Create arrows that scale proportionally with line thickness:

```html
<!-- Model: { connections: [{thickness: 1, label: 'Thin'}, {thickness: 3, label: 'Medium'}, {thickness: 5, label: 'Thick'}] } -->
<svg width="500" height="400">
    <defs>
        <marker id="responsive-arrow" markerWidth="4" markerHeight="4"
                refX="4" refY="2" markerUnits="strokeWidth" orient="auto">
            <polygon points="0,0 4,2 0,4" fill="#e74c3c"/>
        </marker>
    </defs>

    <template data-bind="{{model.connections}}">
        <line x1="100" y1="{{$index * 100 + 100}}"
              x2="400" y2="{{$index * 100 + 100}}"
              stroke="#34495e" stroke-width="{{.thickness}}"
              marker-end="url(#responsive-arrow)"/>
        <text x="50" y="{{$index * 100 + 105}}" font-size="12">{{.label}}</text>
    </template>
</svg>
```

---

## Notes

### strokeWidth (Default)

**Characteristics:**
- Marker size scales proportionally with stroke width
- markerWidth and markerHeight are multiplied by stroke width
- Creates consistent visual weight across different line thicknesses
- Default behavior for most use cases
- Maintains proportional appearance

**How it works:**
- Effective marker size = markerWidth × stroke-width
- Example: markerWidth="3" with stroke-width="2" → 6 units effective width
- Marker content also scales proportionally
- refX and refY are also scaled

```html
<!-- Arrow scales with stroke -->
<marker id="scaled" markerWidth="3" markerHeight="3"
        markerUnits="strokeWidth">
    <polygon points="0,0 3,1.5 0,3" fill="blue"/>
</marker>

<!-- Thin line = small arrow -->
<line stroke-width="2" marker-end="url(#scaled)"/>

<!-- Thick line = large arrow -->
<line stroke-width="8" marker-end="url(#scaled)"/>
```

**Advantages:**
- Maintains visual proportion with line thickness
- Single marker definition works for all stroke widths
- Intuitive scaling behavior
- Good for flow diagrams and technical drawings

**Use cases:**
- Arrowheads on lines of varying thickness
- Flow diagrams with weighted connections
- Network diagrams with importance indicators
- Any scenario where marker should scale with line

### userSpaceOnUse

**Characteristics:**
- Marker size is absolute in document coordinates
- markerWidth and markerHeight specify actual pixel/unit sizes
- Independent of stroke width
- Consistent marker size across all shapes
- More predictable sizing

**How it works:**
- Marker size is exactly as specified
- Example: markerWidth="10" → always 10 units wide
- Not affected by stroke-width changes
- refX and refY in absolute coordinates

```html
<!-- Arrow stays same size -->
<marker id="fixed" markerWidth="10" markerHeight="10"
        markerUnits="userSpaceOnUse">
    <polygon points="0,0 10,5 0,10" fill="blue"/>
</marker>

<!-- Both lines get same sized arrow -->
<line stroke-width="2" marker-end="url(#fixed)"/>
<line stroke-width="8" marker-end="url(#fixed)"/>
```

**Advantages:**
- Predictable, consistent marker sizes
- Easier to design markers (use actual pixels)
- Better for detailed marker graphics
- Independent of stroke styling

**Use cases:**
- Data visualization with consistent markers
- UI elements requiring exact sizes
- Complex marker graphics
- Markers that shouldn't scale with lines

### Choosing the Right System

**Use strokeWidth when:**
- Markers should scale with line thickness
- Creating proportional visual weight
- Building flow diagrams
- Line weight indicates importance
- Want single marker for all line widths

**Use userSpaceOnUse when:**
- Markers should be consistent size
- Creating data visualizations
- Marker detail is important
- Working with fixed-size icons
- Precise marker sizing is critical

### Design Considerations

**For strokeWidth:**
- Design markers in small units (2-4 units)
- Keep marker content simple
- Test with different stroke widths
- Consider how details scale

**For userSpaceOnUse:**
- Design markers in actual pixel sizes
- Can include more detail
- Consider pixel-perfect alignment
- Test visibility at different scales

### Coordinate System Impact

**strokeWidth:**
```html
<!-- Marker viewport is 3 × stroke-width units -->
<marker markerWidth="3" markerHeight="3" markerUnits="strokeWidth">
    <!-- Content coordinates in 0-3 range -->
    <circle cx="1.5" cy="1.5" r="1"/>
</marker>
```

**userSpaceOnUse:**
```html
<!-- Marker viewport is exactly 10 units -->
<marker markerWidth="10" markerHeight="10" markerUnits="userSpaceOnUse">
    <!-- Content coordinates in 0-10 range -->
    <circle cx="5" cy="5" r="4"/>
</marker>
```

### Performance Considerations

- Both systems have similar performance
- strokeWidth requires recalculation on stroke changes
- userSpaceOnUse is slightly more efficient
- Reuse marker definitions when possible
- Simple shapes perform better than complex paths

### Common Pitfalls

**Marker too small/large with strokeWidth:**
- Adjust markerWidth/markerHeight values
- Consider typical stroke widths in your design
- Test with min/max stroke widths

**Marker content doesn't fit with strokeWidth:**
- Ensure content fits within marker viewport
- Remember viewport scales with stroke
- Adjust refX/refY for proper positioning

**Marker size inconsistent:**
- Check markerUnits setting
- Verify stroke-width values
- Ensure marker definitions are reused correctly

---

## Examples

### Stroke Width Scaling (Default)

```html
<svg width="400" height="300">
    <defs>
        <marker id="scaled-arrow" markerWidth="3" markerHeight="3"
                refX="3" refY="1.5" markerUnits="strokeWidth" orient="auto">
            <polygon points="0,0 3,1.5 0,3" fill="#3498db"/>
        </marker>
    </defs>

    <!-- Arrow scales with stroke width -->
    <line x1="50" y1="80" x2="350" y2="80"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#scaled-arrow)"/>
    <text x="370" y="85" font-size="12">2px</text>

    <line x1="50" y1="150" x2="350" y2="150"
          stroke="#2c3e50" stroke-width="5"
          marker-end="url(#scaled-arrow)"/>
    <text x="370" y="155" font-size="12">5px</text>

    <line x1="50" y1="220" x2="350" y2="220"
          stroke="#2c3e50" stroke-width="8"
          marker-end="url(#scaled-arrow)"/>
    <text x="370" y="225" font-size="12">8px</text>
</svg>
```

### User Space on Use (Absolute Size)

```html
<svg width="400" height="300">
    <defs>
        <marker id="fixed-arrow" markerWidth="12" markerHeight="12"
                refX="12" refY="6" markerUnits="userSpaceOnUse" orient="auto">
            <polygon points="0,0 12,6 0,12" fill="#e74c3c"/>
        </marker>
    </defs>

    <!-- Arrow stays same size regardless of stroke -->
    <line x1="50" y1="80" x2="350" y2="80"
          stroke="#2c3e50" stroke-width="2"
          marker-end="url(#fixed-arrow)"/>
    <text x="370" y="85" font-size="12">2px</text>

    <line x1="50" y1="150" x2="350" y2="150"
          stroke="#2c3e50" stroke-width="5"
          marker-end="url(#fixed-arrow)"/>
    <text x="370" y="155" font-size="12">5px</text>

    <line x1="50" y1="220" x2="350" y2="220"
          stroke="#2c3e50" stroke-width="8"
          marker-end="url(#fixed-arrow)"/>
    <text x="370" y="225" font-size="12">8px</text>
</svg>
```

### Side-by-Side Comparison

```html
<svg width="600" height="300">
    <defs>
        <marker id="stroke-scaled" markerWidth="3" markerHeight="3"
                refX="3" refY="1.5" markerUnits="strokeWidth" orient="auto">
            <polygon points="0,0 3,1.5 0,3" fill="#3498db"/>
        </marker>
        <marker id="absolute" markerWidth="9" markerHeight="9"
                refX="9" refY="4.5" markerUnits="userSpaceOnUse" orient="auto">
            <polygon points="0,0 9,4.5 0,9" fill="#e74c3c"/>
        </marker>
    </defs>

    <text x="150" y="30" text-anchor="middle" font-weight="bold">strokeWidth</text>
    <text x="450" y="30" text-anchor="middle" font-weight="bold">userSpaceOnUse</text>

    <line x1="50" y1="70" x2="250" y2="70" stroke="#2c3e50" stroke-width="2"
          marker-end="url(#stroke-scaled)"/>
    <line x1="350" y1="70" x2="550" y2="70" stroke="#2c3e50" stroke-width="2"
          marker-end="url(#absolute)"/>

    <line x1="50" y1="140" x2="250" y2="140" stroke="#2c3e50" stroke-width="5"
          marker-end="url(#stroke-scaled)"/>
    <line x1="350" y1="140" x2="550" y2="140" stroke="#2c3e50" stroke-width="5"
          marker-end="url(#absolute)"/>

    <line x1="50" y1="210" x2="250" y2="210" stroke="#2c3e50" stroke-width="8"
          marker-end="url(#stroke-scaled)"/>
    <line x1="350" y1="210" x2="550" y2="210" stroke="#2c3e50" stroke-width="8"
          marker-end="url(#absolute)"/>
</svg>
```

### Flow Diagram with Weighted Connections

```html
<svg width="500" height="300">
    <defs>
        <marker id="flow-arrow" markerWidth="4" markerHeight="4"
                refX="4" refY="2" markerUnits="strokeWidth" orient="auto">
            <polygon points="0,0 4,2 0,4" fill="#3498db"/>
        </marker>
    </defs>

    <!-- Process boxes -->
    <rect x="50" y="100" width="100" height="60" rx="5" fill="#ecf0f1" stroke="#34495e"/>
    <rect x="350" y="100" width="100" height="60" rx="5" fill="#ecf0f1" stroke="#34495e"/>

    <!-- Connection weight indicates importance -->
    <path d="M 150,115 L 350,115" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#flow-arrow)"/>
    <path d="M 150,130 L 350,130" stroke="#3498db" stroke-width="5"
          marker-end="url(#flow-arrow)"/>
    <path d="M 150,145 L 350,145" stroke="#2c3e50" stroke-width="8"
          marker-end="url(#flow-arrow)"/>

    <text x="100" y="135" text-anchor="middle">Source</text>
    <text x="400" y="135" text-anchor="middle">Target</text>
    <text x="250" y="95" text-anchor="middle" font-size="10">Low</text>
    <text x="250" y="110" text-anchor="middle" font-size="10">Medium</text>
    <text x="250" y="125" text-anchor="middle" font-size="10">High</text>
</svg>
```

### Data Visualization with Consistent Markers

```html
<svg width="500" height="300">
    <defs>
        <marker id="datapoint" markerWidth="10" markerHeight="10"
                refX="5" refY="5" markerUnits="userSpaceOnUse">
            <circle cx="5" cy="5" r="4" fill="#2ecc71" stroke="white" stroke-width="1"/>
        </marker>
    </defs>

    <!-- Data bars with varying widths but consistent markers -->
    <rect x="50" y="200" width="3" height="80" fill="#3498db"/>
    <line x1="51.5" y1="200" x2="51.5" y2="180" stroke="none" marker-end="url(#datapoint)"/>

    <rect x="150" y="150" width="8" height="130" fill="#3498db"/>
    <line x1="154" y1="150" x2="154" y2="130" stroke="none" marker-end="url(#datapoint)"/>

    <rect x="250" y="180" width="5" height="100" fill="#3498db"/>
    <line x1="252.5" y1="180" x2="252.5" y2="160" stroke="none" marker-end="url(#datapoint)"/>

    <rect x="350" y="120" width="10" height="160" fill="#3498db"/>
    <line x1="355" y1="120" x2="355" y2="100" stroke="none" marker-end="url(#datapoint)"/>

    <!-- Baseline -->
    <line x1="30" y1="280" x2="470" y2="280" stroke="#95a5a6" stroke-width="1"/>
</svg>
```

### Network Diagram with Importance Weights

```html
<svg width="500" height="400">
    <defs>
        <marker id="network-arrow" markerWidth="3" markerHeight="3"
                refX="3" refY="1.5" markerUnits="strokeWidth" orient="auto">
            <polygon points="0,0 3,1.5 0,3" fill="#9b59b6"/>
        </marker>
    </defs>

    <!-- Central node -->
    <circle cx="250" cy="200" r="30" fill="#e74c3c" stroke="#c0392b" stroke-width="2"/>
    <text x="250" y="205" text-anchor="middle" fill="white" font-weight="bold">Core</text>

    <!-- Connections with varying importance -->
    <line x1="130" y1="130" x2="225" y2="180" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#network-arrow)"/>
    <line x1="370" y1="130" x2="275" y2="180" stroke="#95a5a6" stroke-width="6"
          marker-end="url(#network-arrow)"/>
    <line x1="130" y1="270" x2="225" y2="220" stroke="#95a5a6" stroke-width="4"
          marker-end="url(#network-arrow)"/>
    <line x1="370" y1="270" x2="275" y2="220" stroke="#95a5a6" stroke-width="3"
          marker-end="url(#network-arrow)"/>

    <!-- Edge nodes -->
    <circle cx="100" cy="100" r="20" fill="#3498db"/>
    <text x="100" y="80" text-anchor="middle" font-size="10">Low</text>
    <circle cx="400" cy="100" r="20" fill="#3498db"/>
    <text x="400" y="80" text-anchor="middle" font-size="10">High</text>
    <circle cx="100" cy="300" r="20" fill="#3498db"/>
    <text x="100" y="325" text-anchor="middle" font-size="10">Med</text>
    <circle cx="400" cy="300" r="20" fill="#3498db"/>
    <text x="400" y="325" text-anchor="middle" font-size="10">Med+</text>
</svg>
```

### Timeline with Fixed Markers

```html
<svg width="600" height="200">
    <defs>
        <marker id="milestone" markerWidth="16" markerHeight="16"
                refX="8" refY="8" markerUnits="userSpaceOnUse">
            <circle cx="8" cy="8" r="7" fill="#f39c12" stroke="#e67e22" stroke-width="2"/>
        </marker>
    </defs>

    <!-- Timeline with varying emphasis -->
    <line x1="50" y1="100" x2="200" y2="100" stroke="#95a5a6" stroke-width="3"
          marker-end="url(#milestone)"/>
    <line x1="200" y1="100" x2="350" y2="100" stroke="#3498db" stroke-width="6"
          marker-end="url(#milestone)"/>
    <line x1="350" y1="100" x2="500" y2="100" stroke="#2ecc71" stroke-width="4"
          marker-end="url(#milestone)"/>
    <line x1="500" y1="100" x2="550" y2="100" stroke="#e74c3c" stroke-width="2"
          marker-end="url(#milestone)"/>

    <circle cx="50" cy="100" r="8" fill="#95a5a6"/>

    <text x="50" y="140" text-anchor="middle" font-size="10">Start</text>
    <text x="200" y="140" text-anchor="middle" font-size="10">Phase 1</text>
    <text x="350" y="140" text-anchor="middle" font-size="10">Phase 2</text>
    <text x="500" y="140" text-anchor="middle" font-size="10">Phase 3</text>
    <text x="550" y="140" text-anchor="middle" font-size="10">End</text>
</svg>
```

### Proportional Dots on Paths

```html
<svg width="400" height="300">
    <defs>
        <marker id="path-dot" markerWidth="2" markerHeight="2"
                refX="1" refY="1" markerUnits="strokeWidth">
            <circle cx="1" cy="1" r="0.8" fill="#e74c3c"/>
        </marker>
    </defs>

    <path d="M 50,250 Q 200,50 350,250"
          fill="none" stroke="#3498db" stroke-width="3"
          marker-start="url(#path-dot)"
          marker-mid="url(#path-dot)"
          marker-end="url(#path-dot)"/>

    <path d="M 50,270 Q 200,70 350,270"
          fill="none" stroke="#2ecc71" stroke-width="6"
          marker-start="url(#path-dot)"
          marker-mid="url(#path-dot)"
          marker-end="url(#path-dot)"/>
</svg>
```

### Grid Lines with Fixed Endpoints

```html
<svg width="500" height="400">
    <defs>
        <marker id="grid-end" markerWidth="8" markerHeight="8"
                refX="4" refY="4" markerUnits="userSpaceOnUse">
            <circle cx="4" cy="4" r="3" fill="#34495e"/>
        </marker>
    </defs>

    <!-- Vertical grid lines -->
    <line x1="100" y1="50" x2="100" y2="350" stroke="#ecf0f1" stroke-width="1"
          marker-start="url(#grid-end)" marker-end="url(#grid-end)"/>
    <line x1="200" y1="50" x2="200" y2="350" stroke="#ecf0f1" stroke-width="2"
          marker-start="url(#grid-end)" marker-end="url(#grid-end)"/>
    <line x1="300" y1="50" x2="300" y2="350" stroke="#ecf0f1" stroke-width="1"
          marker-start="url(#grid-end)" marker-end="url(#grid-end)"/>
    <line x1="400" y1="50" x2="400" y2="350" stroke="#ecf0f1" stroke-width="2"
          marker-start="url(#grid-end)" marker-end="url(#grid-end)"/>

    <!-- Horizontal grid lines -->
    <line x1="100" y1="100" x2="400" y2="100" stroke="#ecf0f1" stroke-width="1"
          marker-start="url(#grid-end)" marker-end="url(#grid-end)"/>
    <line x1="100" y1="200" x2="400" y2="200" stroke="#ecf0f1" stroke-width="2"
          marker-start="url(#grid-end)" marker-end="url(#grid-end)"/>
    <line x1="100" y1="300" x2="400" y2="300" stroke="#ecf0f1" stroke-width="1"
          marker-start="url(#grid-end)" marker-end="url(#grid-end)"/>
</svg>
```

### Circuit Diagram Connections

```html
<svg width="500" height="300">
    <defs>
        <marker id="junction" markerWidth="2.5" markerHeight="2.5"
                refX="1.25" refY="1.25" markerUnits="strokeWidth">
            <circle cx="1.25" cy="1.25" r="1" fill="#2c3e50"/>
        </marker>
    </defs>

    <!-- Main bus (thick) -->
    <line x1="50" y1="150" x2="450" y2="150" stroke="#2c3e50" stroke-width="8"
          marker-start="url(#junction)" marker-end="url(#junction)"/>

    <!-- Branch connections (medium) -->
    <line x1="150" y1="150" x2="150" y2="80" stroke="#34495e" stroke-width="4"
          marker-end="url(#junction)"/>
    <line x1="250" y1="150" x2="250" y2="80" stroke="#34495e" stroke-width="4"
          marker-end="url(#junction)"/>
    <line x1="350" y1="150" x2="350" y2="220" stroke="#34495e" stroke-width="4"
          marker-end="url(#junction)"/>

    <!-- Sub-branches (thin) -->
    <line x1="150" y1="80" x2="100" y2="50" stroke="#7f8c8d" stroke-width="2"
          marker-end="url(#junction)"/>
    <line x1="150" y1="80" x2="200" y2="50" stroke="#7f8c8d" stroke-width="2"
          marker-end="url(#junction)"/>

    <!-- Components -->
    <rect x="90" y="40" width="20" height="20" fill="#3498db" rx="2"/>
    <rect x="190" y="40" width="20" height="20" fill="#e74c3c" rx="2"/>
    <rect x="240" y="70" width="20" height="20" fill="#2ecc71" rx="2"/>
    <rect x="340" y="210" width="20" height="20" fill="#f39c12" rx="2"/>
</svg>
```

### Tree Structure with Proportional Nodes

```html
<svg width="500" height="400">
    <defs>
        <marker id="tree-node" markerWidth="3" markerHeight="3"
                refX="1.5" refY="1.5" markerUnits="strokeWidth">
            <circle cx="1.5" cy="1.5" r="1.2" fill="#2ecc71" stroke="#27ae60" stroke-width="0.3"/>
        </marker>
    </defs>

    <!-- Root (thickest) -->
    <line x1="250" y1="350" x2="250" y2="250" stroke="#8b4513" stroke-width="12"
          marker-start="url(#tree-node)" marker-end="url(#tree-node)"/>

    <!-- Main branches -->
    <line x1="250" y1="250" x2="150" y2="150" stroke="#a0522d" stroke-width="8"
          marker-end="url(#tree-node)"/>
    <line x1="250" y1="250" x2="350" y2="150" stroke="#a0522d" stroke-width="8"
          marker-end="url(#tree-node)"/>

    <!-- Sub-branches -->
    <line x1="150" y1="150" x2="100" y2="80" stroke="#cd853f" stroke-width="4"
          marker-end="url(#tree-node)"/>
    <line x1="150" y1="150" x2="200" y2="80" stroke="#cd853f" stroke-width="4"
          marker-end="url(#tree-node)"/>
    <line x1="350" y1="150" x2="300" y2="80" stroke="#cd853f" stroke-width="4"
          marker-end="url(#tree-node)"/>
    <line x1="350" y1="150" x2="400" y2="80" stroke="#cd853f" stroke-width="4"
          marker-end="url(#tree-node)"/>

    <!-- Leaves (thinnest) -->
    <line x1="100" y1="80" x2="80" y2="40" stroke="#deb887" stroke-width="2"
          marker-end="url(#tree-node)"/>
    <line x1="100" y1="80" x2="120" y2="40" stroke="#deb887" stroke-width="2"
          marker-end="url(#tree-node)"/>
</svg>
```

### UI Components with Fixed Markers

```html
<svg width="500" height="300">
    <defs>
        <marker id="handle" markerWidth="14" markerHeight="14"
                refX="7" refY="7" markerUnits="userSpaceOnUse">
            <rect x="2" y="2" width="10" height="10" rx="2"
                  fill="white" stroke="#3498db" stroke-width="2"/>
        </marker>
    </defs>

    <!-- Slider tracks with different widths -->
    <line x1="50" y1="80" x2="450" y2="80" stroke="#ecf0f1" stroke-width="4"
          marker-end="url(#handle)"/>
    <text x="470" y="85" font-size="12">Normal</text>

    <line x1="50" y1="150" x2="450" y2="150" stroke="#ecf0f1" stroke-width="8"
          marker-end="url(#handle)"/>
    <text x="470" y="155" font-size="12">Large</text>

    <line x1="50" y1="220" x2="450" y2="220" stroke="#ecf0f1" stroke-width="2"
          marker-end="url(#handle)"/>
    <text x="470" y="225" font-size="12">Small</text>
</svg>
```

### Animated Flow Lines

```html
<svg width="600" height="200">
    <defs>
        <marker id="flow-marker" markerWidth="3" markerHeight="3"
                refX="3" refY="1.5" markerUnits="strokeWidth" orient="auto">
            <polygon points="0,0 3,1.5 0,3" fill="#3498db"/>
        </marker>
    </defs>

    <style>
        .flow-line {
            stroke-dasharray: 10 5;
            animation: flow 2s linear infinite;
        }
        @keyframes flow {
            to { stroke-dashoffset: -15; }
        }
    </style>

    <line x1="50" y1="60" x2="550" y2="60" stroke="#3498db" stroke-width="3"
          class="flow-line" marker-end="url(#flow-marker)"/>
    <text x="300" y="90" text-anchor="middle" font-size="12">Light Flow</text>

    <line x1="50" y1="140" x2="550" y2="140" stroke="#2ecc71" stroke-width="6"
          class="flow-line" marker-end="url(#flow-marker)"/>
    <text x="300" y="170" text-anchor="middle" font-size="12">Heavy Flow</text>
</svg>
```

### Measurement Arrows with Consistent Size

```html
<svg width="500" height="400">
    <defs>
        <marker id="measure-arrow" markerWidth="10" markerHeight="10"
                refX="5" refY="5" markerUnits="userSpaceOnUse" orient="auto">
            <path d="M 0,2 L 5,0 L 10,2 L 10,8 L 5,10 L 0,8 Z" fill="#34495e"/>
        </marker>
    </defs>

    <!-- Objects being measured -->
    <rect x="100" y="100" width="150" height="100" fill="#3498db" opacity="0.3"/>
    <rect x="300" y="150" width="100" height="150" fill="#e74c3c" opacity="0.3"/>

    <!-- Dimension lines (varying emphasis but same arrow size) -->
    <line x1="100" y1="220" x2="250" y2="220" stroke="#34495e" stroke-width="1"
          marker-start="url(#measure-arrow)" marker-end="url(#measure-arrow)"/>
    <text x="175" y="240" text-anchor="middle" font-size="12">150</text>

    <line x1="420" y1="150" x2="420" y2="300" stroke="#34495e" stroke-width="1"
          marker-start="url(#measure-arrow)" marker-end="url(#measure-arrow)"/>
    <text x="445" y="230" text-anchor="middle" font-size="12">150</text>

    <line x1="100" y1="320" x2="400" y2="320" stroke="#2ecc71" stroke-width="2"
          marker-start="url(#measure-arrow)" marker-end="url(#measure-arrow)"/>
    <text x="250" y="340" text-anchor="middle" font-size="12">300 (Total)</text>
</svg>
```

---

## See Also

- [marker](/reference/svgtags/marker.html) - Marker definition element
- [markerWidth](/reference/svgattributes/markerWidth.html) - Marker viewport width
- [markerHeight](/reference/svgattributes/markerHeight.html) - Marker viewport height
- [refX](/reference/svgattributes/refX.html) - Marker reference X position
- [refY](/reference/svgattributes/refY.html) - Marker reference Y position
- [orient](/reference/svgattributes/orient.html) - Marker orientation
- [marker-start](/reference/svgattributes/marker-start.html) - Start marker attribute
- [marker-mid](/reference/svgattributes/marker-mid.html) - Mid marker attribute
- [marker-end](/reference/svgattributes/marker-end.html) - End marker attribute
- [Data Binding](/reference/binding/) - Data binding and expressions

---
