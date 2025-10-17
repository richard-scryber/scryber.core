---
layout: default
title: line
parent: SVG Elements
parent_url: /reference/svgelements/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;line&gt; : SVG Line Element

The `<line>` element draws a straight line between two points in SVG content within your PDF documents. It's essential for creating diagrams, charts, connectors, dividers, borders, and geometric patterns.

## Usage

The `<line>` element creates a straight line defined by:
- Start point (`x1`, `y1`) - coordinates of the line's starting position
- End point (`x2`, `y2`) - coordinates of the line's ending position
- Stroke styling attributes (fill is not applicable to lines)
- Transform operations for rotation and positioning

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="100">
    <line x1="10" y1="50" x2="190" y2="50"
          stroke="#000" stroke-width="2"/>
</svg>
```

---

## Supported Attributes

### Geometry Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `x1` | Unit | X-coordinate of the line's start point. Default: 0 |
| `y1` | Unit | Y-coordinate of the line's start point. Default: 0 |
| `x2` | Unit | X-coordinate of the line's end point. Default: 0 |
| `y2` | Unit | Y-coordinate of the line's end point. Default: 0 |

### Styling Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `stroke` | Color | Stroke color. **Required** for visible output. No default. |
| `stroke-width` | Unit | Width of the line. Default: 1pt |
| `stroke-opacity` | Number | Opacity of the stroke (0.0 to 1.0). Default: 1.0 |
| `stroke-linecap` | String | Line cap style: `butt`, `round`, `square`. Default: square |
| `stroke-dasharray` | String | Dash pattern (e.g., `5,3` for dashed, `1,2` for dotted) |

### Marker Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `marker-start` | URL | Reference to marker element for line start (e.g., `url(#arrow)`) |
| `marker-end` | URL | Reference to marker element for line end (e.g., `url(#arrow)`) |
| `marker-mid` | URL | Reference to marker element for line middle point |

### Transform Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `transform` | String | SVG transform operations: `translate(x,y)`, `rotate(angle)`, `scale(x,y)`, `matrix(...)` |
| `transform-origin` | String | Origin point for transformations |

### Common Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | String | Unique identifier for the element |
| `class` | String | CSS class name(s) for styling |
| `style` | String | Inline CSS styles |
| `title` | String | Title for accessibility and outline/bookmark |
| `hidden` | String | Set to "hidden" to hide the element |

---

## Data Binding

The `<line>` element supports dynamic attribute values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Line Coordinates

```html
<!-- Model: { line: { x1: 10, y1: 20, x2: 190, y2: 80 } } -->
<svg width="200" height="100">
    <line x1="{{line.x1}}" y1="{{line.y1}}"
          x2="{{line.x2}}" y2="{{line.y2}}"
          stroke="#2196F3" stroke-width="3"/>
</svg>
```

### Example 2: Chart Grid Lines

```html
<!-- Model: { gridLines: [20, 40, 60, 80, 100] } -->
<svg width="150" height="120">
    <template data-bind="{{gridLines}}">
        <line x1="10" y1="{{.}}" x2="140" y2="{{.}}"
              stroke="#ccc" stroke-width="1"/>
    </template>
</svg>
```

### Example 3: Connection Lines

```html
<!-- Model: { connections: [
    {x1: 30, y1: 30, x2: 170, y2: 70, color: '#e74c3c'},
    {x1: 30, y1: 70, x2: 170, y2: 30, color: '#3498db'}
]} -->
<svg width="200" height="100">
    <template data-bind="{{connections}}">
        <line x1="{{.x1}}" y1="{{.y1}}"
              x2="{{.x2}}" y2="{{.y2}}"
              stroke="{{.color}}" stroke-width="2"/>
    </template>
</svg>
```

---

## Notes

### Stroke Required

Unlike filled shapes, lines require a `stroke` attribute to be visible. Without a stroke color, the line will not appear in the output.

### Fill Not Applicable

The `fill` attribute has no effect on lines. Lines are rendered using only stroke properties.

### Line Caps

The `stroke-linecap` attribute affects the appearance of line endpoints:
- `butt` - ends flush with endpoints (default)
- `round` - rounded ends extending beyond endpoints by half the stroke width
- `square` - square ends extending beyond endpoints by half the stroke width

### Zero-Length Lines

Lines where start and end points are identical (`x1=x2` and `y1=y2`) will not render unless:
- Using `stroke-linecap="round"` or `stroke-linecap="square"` (creates a dot)

### Performance

Lines are efficient shapes for rendering. Use them for grids, axes, and connectors in charts and diagrams.

---

## Examples

### Basic Horizontal Line

```html
<svg width="200" height="50">
    <line x1="10" y1="25" x2="190" y2="25"
          stroke="#333" stroke-width="2"/>
</svg>
```

### Basic Vertical Line

```html
<svg width="50" height="200">
    <line x1="25" y1="10" x2="25" y2="190"
          stroke="#333" stroke-width="2"/>
</svg>
```

### Diagonal Line

```html
<svg width="150" height="150">
    <line x1="10" y1="10" x2="140" y2="140"
          stroke="#2196F3" stroke-width="3"/>
</svg>
```

### Dashed Line

```html
<svg width="200" height="50">
    <line x1="10" y1="25" x2="190" y2="25"
          stroke="#666" stroke-width="2" stroke-dasharray="5,5"/>
</svg>
```

### Dotted Line

```html
<svg width="200" height="50">
    <line x1="10" y1="25" x2="190" y2="25"
          stroke="#666" stroke-width="2" stroke-dasharray="1,3"/>
</svg>
```

### Line with Rounded Caps

```html
<svg width="200" height="50">
    <line x1="20" y1="25" x2="180" y2="25"
          stroke="#4CAF50" stroke-width="10" stroke-linecap="round"/>
</svg>
```

### Cross (X) Shape

```html
<svg width="100" height="100">
    <line x1="20" y1="20" x2="80" y2="80"
          stroke="#e74c3c" stroke-width="4" stroke-linecap="round"/>
    <line x1="80" y1="20" x2="20" y2="80"
          stroke="#e74c3c" stroke-width="4" stroke-linecap="round"/>
</svg>
```

### Plus (+) Shape

```html
<svg width="100" height="100">
    <line x1="50" y1="20" x2="50" y2="80"
          stroke="#2196F3" stroke-width="4" stroke-linecap="round"/>
    <line x1="20" y1="50" x2="80" y2="50"
          stroke="#2196F3" stroke-width="4" stroke-linecap="round"/>
</svg>
```

### Grid Pattern

```html
<svg width="200" height="200">
    <!-- Vertical lines -->
    <line x1="40" y1="0" x2="40" y2="200" stroke="#ddd" stroke-width="1"/>
    <line x1="80" y1="0" x2="80" y2="200" stroke="#ddd" stroke-width="1"/>
    <line x1="120" y1="0" x2="120" y2="200" stroke="#ddd" stroke-width="1"/>
    <line x1="160" y1="0" x2="160" y2="200" stroke="#ddd" stroke-width="1"/>

    <!-- Horizontal lines -->
    <line x1="0" y1="40" x2="200" y2="40" stroke="#ddd" stroke-width="1"/>
    <line x1="0" y1="80" x2="200" y2="80" stroke="#ddd" stroke-width="1"/>
    <line x1="0" y1="120" x2="200" y2="120" stroke="#ddd" stroke-width="1"/>
    <line x1="0" y1="160" x2="200" y2="160" stroke="#ddd" stroke-width="1"/>
</svg>
```

### Chart Axes

```html
<svg width="300" height="200">
    <!-- Y-axis -->
    <line x1="40" y1="20" x2="40" y2="180"
          stroke="#333" stroke-width="2"/>
    <!-- X-axis -->
    <line x1="40" y1="180" x2="280" y2="180"
          stroke="#333" stroke-width="2"/>
</svg>
```

### Arrow (Using Lines)

```html
<svg width="150" height="100">
    <!-- Shaft -->
    <line x1="20" y1="50" x2="130" y2="50"
          stroke="#333" stroke-width="3" stroke-linecap="round"/>
    <!-- Arrowhead -->
    <line x1="115" y1="35" x2="130" y2="50"
          stroke="#333" stroke-width="3" stroke-linecap="round"/>
    <line x1="115" y1="65" x2="130" y2="50"
          stroke="#333" stroke-width="3" stroke-linecap="round"/>
</svg>
```

### Starburst Pattern

```html
<svg width="150" height="150">
    <line x1="75" y1="75" x2="75" y2="10" stroke="#FFC107" stroke-width="2"/>
    <line x1="75" y1="75" x2="120" y2="30" stroke="#FFC107" stroke-width="2"/>
    <line x1="75" y1="75" x2="140" y2="75" stroke="#FFC107" stroke-width="2"/>
    <line x1="75" y1="75" x2="120" y2="120" stroke="#FFC107" stroke-width="2"/>
    <line x1="75" y1="75" x2="75" y2="140" stroke="#FFC107" stroke-width="2"/>
    <line x1="75" y1="75" x2="30" y2="120" stroke="#FFC107" stroke-width="2"/>
    <line x1="75" y1="75" x2="10" y2="75" stroke="#FFC107" stroke-width="2"/>
    <line x1="75" y1="75" x2="30" y2="30" stroke="#FFC107" stroke-width="2"/>
</svg>
```

### Tick Marks on Axis

```html
<svg width="250" height="60">
    <!-- Main axis -->
    <line x1="20" y1="30" x2="230" y2="30"
          stroke="#333" stroke-width="2"/>
    <!-- Tick marks -->
    <line x1="50" y1="25" x2="50" y2="35" stroke="#333" stroke-width="2"/>
    <line x1="90" y1="25" x2="90" y2="35" stroke="#333" stroke-width="2"/>
    <line x1="130" y1="25" x2="130" y2="35" stroke="#333" stroke-width="2"/>
    <line x1="170" y1="25" x2="170" y2="35" stroke="#333" stroke-width="2"/>
    <line x1="210" y1="25" x2="210" y2="35" stroke="#333" stroke-width="2"/>
</svg>
```

### Divider Line

```html
<svg width="300" height="20">
    <line x1="0" y1="10" x2="300" y2="10"
          stroke="#ccc" stroke-width="1"/>
</svg>
```

### Border Lines

```html
<svg width="200" height="150">
    <!-- Top -->
    <line x1="10" y1="10" x2="190" y2="10" stroke="#333" stroke-width="2"/>
    <!-- Right -->
    <line x1="190" y1="10" x2="190" y2="140" stroke="#333" stroke-width="2"/>
    <!-- Bottom -->
    <line x1="190" y1="140" x2="10" y2="140" stroke="#333" stroke-width="2"/>
    <!-- Left -->
    <line x1="10" y1="140" x2="10" y2="10" stroke="#333" stroke-width="2"/>
</svg>
```

### Connection Between Points

```html
<svg width="200" height="100">
    <!-- Start point -->
    <circle cx="30" cy="50" r="8" fill="#4CAF50"/>
    <!-- Line -->
    <line x1="38" y1="50" x2="162" y2="50"
          stroke="#333" stroke-width="2" stroke-dasharray="5,3"/>
    <!-- End point -->
    <circle cx="170" cy="50" r="8" fill="#f44336"/>
</svg>
```

### Bar Chart Baseline

```html
<svg width="300" height="200">
    <!-- Bars -->
    <rect x="40" y="80" width="40" height="100" fill="#e74c3c"/>
    <rect x="100" y="50" width="40" height="130" fill="#3498db"/>
    <rect x="160" y="100" width="40" height="80" fill="#2ecc71"/>
    <rect x="220" y="60" width="40" height="120" fill="#f39c12"/>

    <!-- Baseline -->
    <line x1="30" y1="180" x2="270" y2="180"
          stroke="#333" stroke-width="3"/>
</svg>
```

### Gauge Needle

```html
<svg width="200" height="120">
    <!-- Arc background (simulated with multiple lines) -->
    <line x1="100" y1="100" x2="50" y2="30"
          stroke="#e74c3c" stroke-width="15" stroke-linecap="round" opacity="0.3"/>

    <!-- Needle -->
    <line x1="100" y1="100" x2="130" y2="40"
          stroke="#333" stroke-width="3" stroke-linecap="round"/>

    <!-- Center pivot -->
    <circle cx="100" cy="100" r="6" fill="#333"/>
</svg>
```

### Timeline Connector

```html
<svg width="50" height="200">
    <!-- Vertical timeline -->
    <line x1="25" y1="0" x2="25" y2="200"
          stroke="#2196F3" stroke-width="3"/>

    <!-- Events -->
    <circle cx="25" cy="30" r="8" fill="#4CAF50"/>
    <circle cx="25" cy="100" r="8" fill="#FF9800"/>
    <circle cx="25" cy="170" r="8" fill="#f44336"/>
</svg>
```

### Slash Through Text

```html
<svg width="100" height="50">
    <line x1="10" y1="40" x2="90" y2="10"
          stroke="#f44336" stroke-width="4"/>
</svg>
```

### Dynamic Grid Lines

```html
<!-- Model: { rows: 5, cols: 5, cellSize: 30 } -->
<svg width="180" height="180">
    <!-- Vertical lines -->
    <template data-bind="{{Array.from({length: cols + 1}, (_, i) => i * cellSize + 15)}}">
        <line x1="{{.}}" y1="15" x2="{{.}}" y2="{{rows * cellSize + 15}}"
              stroke="#ddd" stroke-width="1"/>
    </template>

    <!-- Horizontal lines -->
    <template data-bind="{{Array.from({length: rows + 1}, (_, i) => i * cellSize + 15)}}">
        <line x1="15" y1="{{.}}" x2="{{cols * cellSize + 15}}" y2="{{.}}"
              stroke="#ddd" stroke-width="1"/>
    </template>
</svg>
```

### Progress Line

```html
<!-- Model: { progress: 0.65 } -->
<svg width="300" height="20">
    <!-- Background line -->
    <line x1="10" y1="10" x2="290" y2="10"
          stroke="#e0e0e0" stroke-width="8" stroke-linecap="round"/>
    <!-- Progress line -->
    <line x1="10" y1="10" x2="{{10 + (280 * progress)}}" y2="10"
          stroke="#4CAF50" stroke-width="8" stroke-linecap="round"/>
</svg>
```

---

## See Also

- [polyline](/reference/svgelements/polyline.html) - SVG polyline element for connected lines
- [path](/reference/svgelements/path.html) - SVG path element for complex paths
- [rect](/reference/svgelements/rect.html) - SVG rectangle element
- [polygon](/reference/svgelements/polygon.html) - SVG polygon element
- [svg](/reference/svgelements/svg.html) - SVG container element
- [SVG Markers](/reference/svgelements/markers.html) - Arrowheads and line decorations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
