---
layout: default
title: orient
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @orient : The Marker Orientation Attribute

The `orient` attribute defines the orientation (rotation angle) of a marker element relative to the shape it marks. It determines whether markers automatically rotate to align with the path direction or use a fixed angle.

## Usage

The `orient` attribute is used to:
- Automatically orient arrowheads along path direction
- Create markers that follow curve tangents
- Set fixed marker angles independent of path direction
- Reverse marker orientation at path start
- Support data-driven marker rotation
- Build directional indicators that adapt to shape geometry

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <defs>
        <!-- Auto-orienting arrow -->
        <marker id="arrow" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
    </defs>

    <path d="M 50,50 Q 200,150 350,50"
          fill="none" stroke="#2c3e50" stroke-width="2"
          marker-end="url(#arrow)"/>
</svg>
```

---

## Supported Values

| Value | Description | Behavior | Example |
|-------|-------------|----------|---------|
| `auto` | Auto-orient along path (default) | Rotates to match path direction at marker position | `orient="auto"` |
| `auto-start-reverse` | Auto-orient, reversed at start | Like auto, but rotated 180° at path start | `orient="auto-start-reverse"` |
| `<angle>` | Fixed angle in degrees | Fixed rotation regardless of path | `orient="45"` or `orient="90deg"` |

### Angle Units

```html
<!-- Degrees (most common) -->
orient="45"
orient="90deg"

<!-- Radians -->
orient="1.57rad"

<!-- Gradians -->
orient="100grad"

<!-- Turns -->
orient="0.25turn"
```

---

## Supported Elements

The `orient` attribute is supported on:

- **[&lt;marker&gt;](/reference/svgtags/marker.html)** - Marker element orientation

---

## Data Binding

### Dynamic Orientation Mode

Switch between auto and fixed orientation based on data:

```html
<!-- Model: { useAutoOrient: true, fixedAngle: 45 } -->
<svg width="400" height="200">
    <defs>
        <marker id="dynamicOrient" markerWidth="10" markerHeight="10"
                refX="10" refY="5"
                orient="{{model.useAutoOrient ? 'auto' : model.fixedAngle}}">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
    </defs>

    <path d="M 50,100 Q 200,50 350,100"
          fill="none" stroke="#2c3e50" stroke-width="2"
          marker-end="url(#dynamicOrient)"/>
</svg>
```

### Data-Driven Arrow Angles

Set marker angles from data for directional indicators:

```html
<!-- Model: { arrows: [{angle: 0, label: 'East'}, {angle: 90, label: 'South'}, {angle: 180, label: 'West'}, {angle: 270, label: 'North'}] } -->
<svg width="400" height="400">
    <defs>
        <template data-bind="{{model.arrows}}">
            <marker id="arrow-{{.angle}}" markerWidth="12" markerHeight="12"
                    refX="6" refY="6" orient="{{.angle}}">
                <path d="M 6,2 L 10,6 L 6,10 L 6,2" fill="#e74c3c"/>
            </marker>
        </template>
    </defs>

    <g transform="translate(200, 200)">
        <template data-bind="{{model.arrows}}">
            <line x1="0" y1="0" x2="80" y2="0"
                  stroke="#95a5a6" stroke-width="2"
                  marker-end="url(#arrow-{{.angle}})"
                  transform="rotate({{.angle}})"/>
            <text x="{{.angle === 0 ? 100 : (.angle === 180 ? -100 : 0)}}"
                  y="{{.angle === 90 ? 110 : (.angle === 270 ? -90 : 0)}}"
                  text-anchor="middle" font-size="12">{{.label}}</text>
        </template>
    </g>
</svg>
```

### Conditional Reversed Orientation

Use reversed orientation for bidirectional flows:

```html
<!-- Model: { connections: [{start: {x: 50, y: 100}, end: {x: 350, y: 100}, bidirectional: true}] } -->
<svg width="400" height="200">
    <defs>
        <marker id="arrow-auto" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
        <marker id="arrow-reverse" markerWidth="10" markerHeight="10"
                refX="0" refY="5" orient="auto-start-reverse">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <template data-bind="{{model.connections}}">
        <line x1="{{.start.x}}" y1="{{.start.y}}"
              x2="{{.end.x}}" y2="{{.end.y}}"
              stroke="#2c3e50" stroke-width="2"
              marker-start="{{.bidirectional ? 'url(#arrow-reverse)' : 'none'}}"
              marker-end="url(#arrow-auto)"/>
    </template>
</svg>
```

### Clock Hand Positions

Create clock-like indicators with precise angles:

```html
<!-- Model: { hourAngle: 90, minuteAngle: 180, secondAngle: 270 } -->
<svg width="300" height="300" viewBox="0 0 300 300">
    <defs>
        <marker id="hour-hand" markerWidth="8" markerHeight="8"
                refX="4" refY="4" orient="{{model.hourAngle}}">
            <circle cx="4" cy="4" r="3" fill="#2c3e50"/>
        </marker>
        <marker id="minute-hand" markerWidth="6" markerHeight="6"
                refX="3" refY="3" orient="{{model.minuteAngle}}">
            <circle cx="3" cy="3" r="2" fill="#3498db"/>
        </marker>
        <marker id="second-hand" markerWidth="4" markerHeight="4"
                refX="2" refY="2" orient="{{model.secondAngle}}">
            <circle cx="2" cy="2" r="1.5" fill="#e74c3c"/>
        </marker>
    </defs>

    <circle cx="150" cy="150" r="100" fill="none" stroke="#ecf0f1" stroke-width="2"/>

    <!-- Clock hands -->
    <line x1="150" y1="150" x2="150" y2="100"
          stroke="#2c3e50" stroke-width="4"
          transform="rotate({{model.hourAngle}} 150 150)"
          marker-end="url(#hour-hand)"/>
    <line x1="150" y1="150" x2="150" y2="80"
          stroke="#3498db" stroke-width="3"
          transform="rotate({{model.minuteAngle}} 150 150)"
          marker-end="url(#minute-hand)"/>
    <line x1="150" y1="150" x2="150" y2="70"
          stroke="#e74c3c" stroke-width="2"
          transform="rotate({{model.secondAngle}} 150 150)"
          marker-end="url(#second-hand)"/>
</svg>
```

---

## Notes

### Auto Orientation

**Characteristics:**
- Marker automatically rotates to align with path direction
- Most common for arrowheads and directional indicators
- Calculates tangent angle at marker position
- Works seamlessly with curves and complex paths
- Default behavior for most use cases

**How it works:**
- At each vertex, calculates the path direction
- Rotates marker to match that direction
- 0° orientation points to the right (east)
- Positive angles rotate clockwise

```html
<!-- Arrow follows path direction -->
<marker id="auto-arrow" orient="auto">
    <polygon points="0,0 10,5 0,10" fill="blue"/>
</marker>
```

**Use cases:**
- Arrowheads on lines and curves
- Flow direction indicators
- Path direction markers
- Navigation arrows

### Auto-Start-Reverse

**Characteristics:**
- Like `auto`, but rotated 180° when used with `marker-start`
- Only affects start markers; behaves like `auto` for mid/end markers
- Useful for bidirectional arrows
- Simplifies creating opposing arrowheads

```html
<!-- Same marker works for both directions -->
<marker id="bi-arrow" orient="auto-start-reverse">
    <polygon points="0,0 10,5 0,10" fill="blue"/>
</marker>

<line x1="50" y1="100" x2="350" y2="100"
      marker-start="url(#bi-arrow)"
      marker-end="url(#bi-arrow)"/>
```

**Use cases:**
- Bidirectional connections
- Two-way data flows
- Symmetric relationships
- Reversible processes

### Fixed Angles

**Characteristics:**
- Marker maintains constant orientation regardless of path
- Specified in degrees (0-360) or other angle units
- 0° points right, 90° points down, 180° points left, 270° points up
- Independent of path direction

```html
<!-- Always points down -->
<marker id="down-arrow" orient="90">
    <polygon points="0,0 10,5 0,10" fill="blue"/>
</marker>
```

**Use cases:**
- Compass directions
- Fixed drop indicators
- Gravity/force arrows
- Consistent icon orientation

### Angle Reference System

- **0°** - Points right (east, 3 o'clock)
- **90°** - Points down (south, 6 o'clock)
- **180°** - Points left (west, 9 o'clock)
- **270°** or **-90°** - Points up (north, 12 o'clock)

### Combining with Transform

- `orient` affects marker rotation, not position
- Marker transforms are applied after orientation
- Path transforms affect marker position but not auto-orientation
- Use both for complex marker positioning

### Performance Considerations

- `auto` requires tangent calculations at each marker
- Fixed angles are more performant
- For many markers, consider fixed angles if appropriate
- Auto-orientation recalculates on path changes

### Browser Compatibility

- `auto` widely supported in all SVG implementations
- `auto-start-reverse` is SVG 2 feature (check compatibility)
- Fixed angles have universal support
- Fallback: use separate markers for start/end if needed

### Common Pitfalls

**Marker appears rotated incorrectly:**
- Check that marker design assumes 0° = pointing right
- Verify refX/refY are set correctly for rotation center
- Ensure marker viewBox is centered appropriately

**Auto-orientation doesn't work:**
- Verify path has clear direction at marker point
- Check that marker is properly defined in defs
- Ensure orient="auto" is explicitly set

---

## Examples

### Basic Auto-Orienting Arrow

```html
<svg width="400" height="200">
    <defs>
        <marker id="arrow-auto" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
    </defs>

    <path d="M 50,100 L 150,50 L 250,100 L 350,50"
          fill="none" stroke="#2c3e50" stroke-width="2"
          marker-end="url(#arrow-auto)"/>
</svg>
```

### Fixed Angle Arrow (90 degrees)

```html
<svg width="400" height="300">
    <defs>
        <marker id="arrow-down" markerWidth="10" markerHeight="10"
                refX="5" refY="0" orient="90">
            <polygon points="0,0 10,0 5,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <!-- All arrows point down regardless of line direction -->
    <line x1="100" y1="50" x2="100" y2="200" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#arrow-down)"/>
    <line x1="200" y1="50" x2="200" y2="200" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#arrow-down)"/>
    <line x1="300" y1="50" x2="300" y2="200" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#arrow-down)"/>
</svg>
```

### Bidirectional Arrow with Auto-Start-Reverse

```html
<svg width="400" height="200">
    <defs>
        <marker id="arrow-bi" markerWidth="10" markerHeight="10"
                refX="5" refY="5" orient="auto-start-reverse">
            <polygon points="0,2 5,0 10,2 10,8 5,10 0,8" fill="#9b59b6"/>
        </marker>
    </defs>

    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#34495e" stroke-width="3"
          marker-start="url(#arrow-bi)"
          marker-end="url(#arrow-bi)"/>
</svg>
```

### Curved Path with Auto-Orienting Markers

```html
<svg width="400" height="300">
    <defs>
        <marker id="curve-arrow" markerWidth="12" markerHeight="12"
                refX="12" refY="6" orient="auto">
            <path d="M 0,0 L 12,6 L 0,12 Z" fill="#2ecc71"/>
        </marker>
    </defs>

    <path d="M 50,250 C 50,50 350,50 350,250"
          fill="none" stroke="#2c3e50" stroke-width="3"
          marker-start="url(#curve-arrow)"
          marker-mid="url(#curve-arrow)"
          marker-end="url(#curve-arrow)"/>
</svg>
```

### Compass Rose with Fixed Angles

```html
<svg width="400" height="400" viewBox="0 0 400 400">
    <defs>
        <marker id="north" markerWidth="12" markerHeight="12"
                refX="6" refY="12" orient="270">
            <polygon points="6,0 12,12 0,12" fill="#e74c3c"/>
        </marker>
        <marker id="east" markerWidth="12" markerHeight="12"
                refX="0" refY="6" orient="0">
            <polygon points="0,0 12,6 0,12" fill="#3498db"/>
        </marker>
        <marker id="south" markerWidth="12" markerHeight="12"
                refX="6" refY="0" orient="90">
            <polygon points="0,0 12,0 6,12" fill="#2ecc71"/>
        </marker>
        <marker id="west" markerWidth="12" markerHeight="12"
                refX="12" refY="6" orient="180">
            <polygon points="0,6 12,0 12,12" fill="#f39c12"/>
        </marker>
    </defs>

    <circle cx="200" cy="200" r="100" fill="none" stroke="#ecf0f1" stroke-width="2"/>

    <line x1="200" y1="200" x2="200" y2="80" stroke="#e74c3c" stroke-width="3"
          marker-end="url(#north)"/>
    <line x1="200" y1="200" x2="320" y2="200" stroke="#3498db" stroke-width="3"
          marker-end="url(#east)"/>
    <line x1="200" y1="200" x2="200" y2="320" stroke="#2ecc71" stroke-width="3"
          marker-end="url(#south)"/>
    <line x1="200" y1="200" x2="80" y2="200" stroke="#f39c12" stroke-width="3"
          marker-end="url(#west)"/>

    <text x="200" y="65" text-anchor="middle" font-weight="bold" fill="#e74c3c">N</text>
    <text x="340" y="205" text-anchor="middle" font-weight="bold" fill="#3498db">E</text>
    <text x="200" y="345" text-anchor="middle" font-weight="bold" fill="#2ecc71">S</text>
    <text x="60" y="205" text-anchor="middle" font-weight="bold" fill="#f39c12">W</text>
</svg>
```

### 45-Degree Angled Markers

```html
<svg width="400" height="400">
    <defs>
        <marker id="angle-45" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="45">
            <polygon points="0,0 10,5 0,10" fill="#9b59b6"/>
        </marker>
    </defs>

    <g stroke="#95a5a6" stroke-width="2" fill="none">
        <line x1="50" y1="200" x2="150" y2="200" marker-end="url(#angle-45)"/>
        <line x1="200" y1="200" x2="300" y2="200" marker-end="url(#angle-45)"/>
        <line x1="100" y1="100" x2="100" y2="200" marker-end="url(#angle-45)"/>
        <line x1="250" y1="100" x2="250" y2="200" marker-end="url(#angle-45)"/>
    </g>
</svg>
```

### Flow Diagram with Auto-Orient

```html
<svg width="600" height="300">
    <defs>
        <marker id="flow-arrow" markerWidth="8" markerHeight="8"
                refX="8" refY="4" orient="auto">
            <polygon points="0,0 8,4 0,8" fill="#3498db"/>
        </marker>
    </defs>

    <!-- Process boxes -->
    <rect x="50" y="100" width="100" height="60" rx="5" fill="#ecf0f1" stroke="#34495e" stroke-width="2"/>
    <rect x="250" y="50" width="100" height="60" rx="5" fill="#ecf0f1" stroke="#34495e" stroke-width="2"/>
    <rect x="250" y="150" width="100" height="60" rx="5" fill="#ecf0f1" stroke="#34495e" stroke-width="2"/>
    <rect x="450" y="100" width="100" height="60" rx="5" fill="#ecf0f1" stroke="#34495e" stroke-width="2"/>

    <!-- Flow arrows -->
    <path d="M 150,130 L 250,80" fill="none" stroke="#3498db" stroke-width="2"
          marker-end="url(#flow-arrow)"/>
    <path d="M 150,130 L 250,180" fill="none" stroke="#3498db" stroke-width="2"
          marker-end="url(#flow-arrow)"/>
    <path d="M 350,80 L 450,130" fill="none" stroke="#3498db" stroke-width="2"
          marker-end="url(#flow-arrow)"/>
    <path d="M 350,180 L 450,130" fill="none" stroke="#3498db" stroke-width="2"
          marker-end="url(#flow-arrow)"/>

    <!-- Labels -->
    <text x="100" y="135" text-anchor="middle" font-size="14">Start</text>
    <text x="300" y="85" text-anchor="middle" font-size="14">Process A</text>
    <text x="300" y="185" text-anchor="middle" font-size="14">Process B</text>
    <text x="500" y="135" text-anchor="middle" font-size="14">End</text>
</svg>
```

### Mixed Auto and Fixed Orientations

```html
<svg width="400" height="300">
    <defs>
        <!-- Auto-orienting arrow -->
        <marker id="auto-marker" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
        <!-- Fixed downward arrow -->
        <marker id="fixed-marker" markerWidth="10" markerHeight="10"
                refX="5" refY="0" orient="90">
            <polygon points="0,0 10,0 5,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <text x="200" y="30" text-anchor="middle" font-size="14" font-weight="bold">Auto Orient</text>
    <path d="M 50,50 Q 200,120 350,50"
          fill="none" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#auto-marker)"/>

    <text x="200" y="160" text-anchor="middle" font-size="14" font-weight="bold">Fixed Orient (90°)</text>
    <path d="M 50,180 Q 200,250 350,180"
          fill="none" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#fixed-marker)"/>
</svg>
```

### Reverse Arrow Comparison

```html
<svg width="400" height="300">
    <defs>
        <marker id="regular" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
        <marker id="reverse" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto-start-reverse">
            <polygon points="0,0 10,5 0,10" fill="#e74c3c"/>
        </marker>
    </defs>

    <text x="200" y="80" text-anchor="middle" font-size="14" font-weight="bold">
        Regular Auto-Orient
    </text>
    <line x1="50" y1="100" x2="350" y2="100"
          stroke="#95a5a6" stroke-width="2"
          marker-start="url(#regular)" marker-end="url(#regular)"/>

    <text x="200" y="180" text-anchor="middle" font-size="14" font-weight="bold">
        Auto-Start-Reverse
    </text>
    <line x1="50" y1="200" x2="350" y2="200"
          stroke="#95a5a6" stroke-width="2"
          marker-start="url(#reverse)" marker-end="url(#reverse)"/>
</svg>
```

### Spiral with Auto-Orienting Dots

```html
<svg width="400" height="400" viewBox="0 0 400 400">
    <defs>
        <marker id="spiral-dot" markerWidth="8" markerHeight="8"
                refX="4" refY="4" orient="auto">
            <circle cx="4" cy="4" r="3" fill="#9b59b6"/>
        </marker>
    </defs>

    <path d="M 200,200
             Q 220,180 240,200
             T 260,240
             T 240,280
             T 180,280
             T 140,240
             T 140,180
             T 200,140"
          fill="none" stroke="#3498db" stroke-width="2"
          marker-start="url(#spiral-dot)"
          marker-mid="url(#spiral-dot)"
          marker-end="url(#spiral-dot)"/>
</svg>
```

### Data Visualization Arrows

```html
<svg width="500" height="300">
    <defs>
        <marker id="increase" markerWidth="10" markerHeight="10"
                refX="5" refY="0" orient="270">
            <polygon points="0,0 10,0 5,10" fill="#2ecc71"/>
        </marker>
        <marker id="decrease" markerWidth="10" markerHeight="10"
                refX="5" refY="10" orient="90">
            <polygon points="0,10 10,10 5,0" fill="#e74c3c"/>
        </marker>
    </defs>

    <!-- Data points with trend arrows -->
    <polyline points="50,200 100,180 150,150 200,170 250,120 300,140 350,100 400,90 450,80"
              fill="none" stroke="#3498db" stroke-width="2"/>

    <!-- Trend indicators -->
    <line x1="100" y1="180" x2="100" y2="160" stroke="#2ecc71" stroke-width="2"
          marker-end="url(#increase)"/>
    <line x1="200" y1="170" x2="200" y2="190" stroke="#e74c3c" stroke-width="2"
          marker-end="url(#decrease)"/>
    <line x1="250" y1="120" x2="250" y2="100" stroke="#2ecc71" stroke-width="2"
          marker-end="url(#increase)"/>
    <line x1="300" y1="140" x2="300" y2="160" stroke="#e74c3c" stroke-width="2"
          marker-end="url(#decrease)"/>
</svg>
```

### Animated Rotation (with CSS)

```html
<svg width="400" height="300">
    <defs>
        <marker id="rotating" markerWidth="12" markerHeight="12"
                refX="6" refY="6" orient="0">
            <path d="M 6,2 L 10,6 L 6,10 L 2,6 Z" fill="#e74c3c"/>
        </marker>
    </defs>

    <style>
        @keyframes rotate-marker {
            from { orient: 0deg; }
            to { orient: 360deg; }
        }
    </style>

    <line x1="100" y1="150" x2="300" y2="150"
          stroke="#95a5a6" stroke-width="2"
          marker-mid="url(#rotating)"/>

    <text x="200" y="200" text-anchor="middle" font-size="12">
        Fixed orientation (no rotation with path)
    </text>
</svg>
```

### Wind Direction Indicators

```html
<svg width="600" height="200">
    <defs>
        <marker id="wind-0" markerWidth="16" markerHeight="16"
                refX="0" refY="8" orient="0">
            <path d="M 0,8 L 12,4 L 12,12 Z" fill="#3498db"/>
        </marker>
        <marker id="wind-45" markerWidth="16" markerHeight="16"
                refX="0" refY="8" orient="45">
            <path d="M 0,8 L 12,4 L 12,12 Z" fill="#3498db"/>
        </marker>
        <marker id="wind-90" markerWidth="16" markerHeight="16"
                refX="8" refY="0" orient="90">
            <path d="M 8,0 L 4,12 L 12,12 Z" fill="#3498db"/>
        </marker>
    </defs>

    <circle cx="100" cy="100" r="40" fill="#ecf0f1" stroke="#95a5a6" stroke-width="2"/>
    <line x1="100" y1="100" x2="160" y2="100" stroke="#3498db" stroke-width="3"
          marker-end="url(#wind-0)"/>
    <text x="100" y="160" text-anchor="middle" font-size="12">0° (East)</text>

    <circle cx="300" cy="100" r="40" fill="#ecf0f1" stroke="#95a5a6" stroke-width="2"/>
    <line x1="300" y1="100" x2="360" y2="60" stroke="#3498db" stroke-width="3"
          marker-end="url(#wind-45)"/>
    <text x="300" y="160" text-anchor="middle" font-size="12">45° (NE)</text>

    <circle cx="500" cy="100" r="40" fill="#ecf0f1" stroke="#95a5a6" stroke-width="2"/>
    <line x1="500" y1="100" x2="500" y2="160" stroke="#3498db" stroke-width="3"
          marker-end="url(#wind-90)"/>
    <text x="500" y="180" text-anchor="middle" font-size="12">90° (South)</text>
</svg>
```

### Network Flow with Mixed Orientations

```html
<svg width="600" height="400">
    <defs>
        <marker id="flow-auto" markerWidth="10" markerHeight="10"
                refX="10" refY="5" orient="auto">
            <polygon points="0,0 10,5 0,10" fill="#3498db"/>
        </marker>
        <marker id="broadcast" markerWidth="12" markerHeight="12"
                refX="6" refY="6" orient="0">
            <circle cx="6" cy="6" r="5" fill="#f39c12"/>
            <circle cx="6" cy="6" r="2" fill="white"/>
        </marker>
    </defs>

    <!-- Central node -->
    <circle cx="300" cy="200" r="30" fill="#e74c3c" stroke="#c0392b" stroke-width="2"/>
    <text x="300" y="205" text-anchor="middle" fill="white" font-weight="bold">Hub</text>

    <!-- Connections with auto-orient -->
    <path d="M 100,100 L 270,180" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#flow-auto)"/>
    <path d="M 500,100 L 330,180" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#flow-auto)"/>
    <path d="M 100,300 L 270,220" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#flow-auto)"/>
    <path d="M 500,300 L 330,220" stroke="#95a5a6" stroke-width="2"
          marker-end="url(#flow-auto)"/>

    <!-- Broadcast indicators (fixed orientation) -->
    <line x1="300" y1="200" x2="380" y2="200" stroke="none"
          marker-end="url(#broadcast)"/>

    <!-- Edge nodes -->
    <circle cx="100" cy="100" r="20" fill="#3498db"/>
    <circle cx="500" cy="100" r="20" fill="#3498db"/>
    <circle cx="100" cy="300" r="20" fill="#3498db"/>
    <circle cx="500" cy="300" r="20" fill="#3498db"/>
</svg>
```

---

## See Also

- [marker](/reference/svgtags/marker.html) - Marker definition element
- [marker-start](/reference/svgattributes/marker-start.html) - Start marker attribute
- [marker-mid](/reference/svgattributes/marker-mid.html) - Mid marker attribute
- [marker-end](/reference/svgattributes/marker-end.html) - End marker attribute
- [markerWidth](/reference/svgattributes/markerWidth.html) - Marker viewport width
- [markerHeight](/reference/svgattributes/markerHeight.html) - Marker viewport height
- [refX](/reference/svgattributes/refX.html) - Marker reference X position
- [refY](/reference/svgattributes/refY.html) - Marker reference Y position
- [markerUnits](/reference/svgattributes/markerUnits.html) - Marker coordinate system
- [Data Binding](/reference/binding/) - Data binding and expressions

---
