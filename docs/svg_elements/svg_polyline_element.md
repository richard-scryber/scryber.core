---
layout: default
title: polyline
parent: SVG Elements
parent_url: /reference/svgelements/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;polyline&gt; : SVG Polyline Element

The `<polyline>` element draws a series of connected straight lines in SVG content within your PDF documents. Unlike `<polygon>`, polylines are open paths (not automatically closed). They're perfect for line charts, trend lines, signatures, waveforms, and complex multi-segment paths.

## Usage

The `<polyline>` element creates connected line segments defined by:
- A series of points (`points`) - coordinates defining each vertex
- Stroke styling attributes (typically without fill for open paths)
- Transform operations for rotation and positioning
- Optional fill (creates a shape with an open edge)

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="100">
    <polyline points="10,50 50,20 90,60 130,30 170,70"
              fill="none" stroke="#2196F3" stroke-width="2"/>
</svg>
```

---

## Supported Attributes

### Geometry Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `points` | String | Space or comma-separated list of x,y coordinate pairs. Format: `"x1,y1 x2,y2 x3,y3"` or `"x1 y1 x2 y2 x3 y3"`. **Required**. |

### Styling Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `stroke` | Color | Stroke color for the line segments. **Typically required** for visible output. |
| `stroke-width` | Unit | Width of the line segments. Default: 1pt |
| `stroke-opacity` | Number | Opacity of the stroke (0.0 to 1.0). Default: 1.0 |
| `stroke-linecap` | String | Line cap style: `butt`, `round`, `square`. Default: square |
| `stroke-linejoin` | String | Line join style: `miter`, `round`, `bevel`. Default: bevel |
| `stroke-dasharray` | String | Dash pattern (e.g., `5,3` for dashed lines) |
| `fill` | Color/URL | Fill color for the area under the polyline (creates closed shape to baseline). Default: black |
| `fill-opacity` | Number | Opacity of the fill (0.0 to 1.0). Default: 1.0 |

### Marker Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `marker-start` | URL | Reference to marker for the first point (e.g., `url(#dot)`) |
| `marker-mid` | URL | Reference to marker for intermediate points |
| `marker-end` | URL | Reference to marker for the last point |

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

The `<polyline>` element supports dynamic attribute values using data binding expressions with `{{expression}}` syntax.

### Example 1: Line Chart from Data

```html
<!-- Model: { data: [10, 25, 15, 40, 30, 45, 35] } -->
<svg width="300" height="150">
    <polyline points="{{data.map((v, i) => `${30 + i * 40},${120 - v * 2}`).join(' ')}}"
              fill="none" stroke="#2196F3" stroke-width="3"/>
</svg>
```

### Example 2: Dynamic Trend Line

```html
<!-- Model: { points: "50,80 100,60 150,90 200,40 250,70" } -->
<svg width="300" height="120">
    <polyline points="{{points}}"
              fill="none" stroke="#4CAF50" stroke-width="2"
              stroke-linecap="round" stroke-linejoin="round"/>
</svg>
```

### Example 3: Multiple Polylines

```html
<!-- Model: { series: [
    {points: '20,80 60,60 100,70 140,50', color: '#e74c3c'},
    {points: '20,90 60,75 100,85 140,70', color: '#3498db'}
]} -->
<svg width="160" height="120">
    <template data-bind="{{series}}">
        <polyline points="{{.points}}"
                  fill="none" stroke="{{.color}}" stroke-width="2"/>
    </template>
</svg>
```

---

## Notes

### Points Format

The `points` attribute accepts coordinates in flexible formats:
- Comma-separated: `"10,20 30,40 50,60"`
- Space-separated: `"10 20 30 40 50 60"`
- Mixed: `"10,20, 30,40, 50,60"`

Each pair represents an (x,y) coordinate.

### Open vs Closed Paths

- `<polyline>` creates an **open path** (first and last points are not connected)
- Use `<polygon>` for automatically closed shapes
- Can manually close by repeating the first point at the end

### Fill Behavior

When a fill is applied to a polyline:
- An implicit line connects the last point to the first point for filling purposes
- The stroke still follows only the specified points (open path)
- Use `fill="none"` for line-only visualization (common for charts)

### Stroke Joins

The `stroke-linejoin` attribute affects corners:
- `miter` - sharp corners (default)
- `round` - rounded corners
- `bevel` - beveled (cut) corners

### Minimum Points

At least 2 points are required for a visible polyline. A single point produces no output.

---

## Examples

### Basic Polyline

```html
<svg width="200" height="100">
    <polyline points="10,50 50,20 90,60 130,30 170,70"
              fill="none" stroke="#333" stroke-width="2"/>
</svg>
```

### Line Chart

```html
<svg width="300" height="200">
    <!-- Axes -->
    <line x1="40" y1="20" x2="40" y2="180" stroke="#333" stroke-width="2"/>
    <line x1="40" y1="180" x2="280" y2="180" stroke="#333" stroke-width="2"/>

    <!-- Data line -->
    <polyline points="60,150 100,120 140,140 180,90 220,110 260,70"
              fill="none" stroke="#2196F3" stroke-width="3"/>

    <!-- Data points -->
    <circle cx="60" cy="150" r="4" fill="#2196F3"/>
    <circle cx="100" cy="120" r="4" fill="#2196F3"/>
    <circle cx="140" cy="140" r="4" fill="#2196F3"/>
    <circle cx="180" cy="90" r="4" fill="#2196F3"/>
    <circle cx="220" cy="110" r="4" fill="#2196F3"/>
    <circle cx="260" cy="70" r="4" fill="#2196F3"/>
</svg>
```

### Zigzag Pattern

```html
<svg width="200" height="100">
    <polyline points="10,50 30,20 50,50 70,20 90,50 110,20 130,50 150,20 170,50 190,20"
              fill="none" stroke="#FF9800" stroke-width="3"/>
</svg>
```

### Mountain Silhouette

```html
<svg width="300" height="150">
    <polyline points="0,150 30,120 60,80 100,110 140,50 180,90 220,70 260,100 300,150"
              fill="#4A90E2" stroke="none"/>
</svg>
```

### Waveform

```html
<svg width="250" height="100">
    <polyline points="0,50 20,40 40,30 60,40 80,50 100,60 120,70 140,60 160,50 180,40 200,30 220,40 240,50"
              fill="none" stroke="#9C27B0" stroke-width="2"/>
</svg>
```

### Staircase

```html
<svg width="200" height="200">
    <polyline points="20,180 20,160 40,160 40,140 60,140 60,120 80,120 80,100 100,100 100,80 120,80 120,60 140,60 140,40 160,40 160,20"
              fill="none" stroke="#4CAF50" stroke-width="3" stroke-linejoin="miter"/>
</svg>
```

### Lightning Bolt

```html
<svg width="100" height="150">
    <polyline points="50,10 30,60 55,65 40,110 70,70 50,65 65,30"
              fill="#FFC107" stroke="#F57F17" stroke-width="2" stroke-linejoin="miter"/>
</svg>
```

### Rounded Corner Line

```html
<svg width="200" height="150">
    <polyline points="20,130 20,50 100,50 100,130 180,130"
              fill="none" stroke="#e74c3c" stroke-width="4"
              stroke-linecap="round" stroke-linejoin="round"/>
</svg>
```

### Stock Price Chart

```html
<svg width="300" height="150">
    <!-- Background -->
    <rect x="0" y="0" width="300" height="150" fill="#f5f5f5"/>

    <!-- Grid lines -->
    <line x1="0" y1="37.5" x2="300" y2="37.5" stroke="#ddd" stroke-width="1"/>
    <line x1="0" y1="75" x2="300" y2="75" stroke="#ddd" stroke-width="1"/>
    <line x1="0" y1="112.5" x2="300" y2="112.5" stroke="#ddd" stroke-width="1"/>

    <!-- Price line -->
    <polyline points="10,100 50,85 90,95 130,70 170,80 210,60 250,75 290,65"
              fill="none" stroke="#4CAF50" stroke-width="2"/>
</svg>
```

### Multi-Line Chart

```html
<svg width="300" height="200">
    <!-- Series 1 -->
    <polyline points="40,150 80,120 120,140 160,100 200,130 240,90 280,110"
              fill="none" stroke="#e74c3c" stroke-width="2"/>

    <!-- Series 2 -->
    <polyline points="40,160 80,140 120,150 160,130 200,145 240,125 280,135"
              fill="none" stroke="#3498db" stroke-width="2"/>

    <!-- Series 3 -->
    <polyline points="40,170 80,155 120,165 160,150 200,160 240,148 280,152"
              fill="none" stroke="#2ecc71" stroke-width="2"/>
</svg>
```

### Dashed Trend Line

```html
<svg width="250" height="120">
    <polyline points="20,100 70,80 120,90 170,60 220,75"
              fill="none" stroke="#9E9E9E" stroke-width="2"
              stroke-dasharray="5,3"/>
</svg>
```

### Area Chart (Filled Polyline)

```html
<svg width="300" height="150">
    <!-- Filled area -->
    <polyline points="10,140 50,110 90,120 130,80 170,95 210,70 250,90 290,75 290,140 10,140"
              fill="#2196F3" fill-opacity="0.3" stroke="none"/>

    <!-- Line -->
    <polyline points="10,140 50,110 90,120 130,80 170,95 210,70 250,90 290,75"
              fill="none" stroke="#2196F3" stroke-width="2"/>
</svg>
```

### Signature Line

```html
<svg width="250" height="80">
    <polyline points="10,50 30,45 50,35 70,40 90,45 110,35 130,30 150,35 170,45 190,50 210,45 230,40"
              fill="none" stroke="#000" stroke-width="2" stroke-linecap="round"/>
</svg>
```

### Heart Rate Monitor

```html
<svg width="300" height="100">
    <polyline points="10,50 40,50 50,30 60,70 70,50 100,50 110,30 120,70 130,50 160,50 170,30 180,70 190,50 220,50 230,30 240,70 250,50 280,50"
              fill="none" stroke="#4CAF50" stroke-width="2"/>
</svg>
```

### Temperature Graph

```html
<svg width="300" height="180">
    <!-- Background -->
    <rect x="0" y="0" width="300" height="180" fill="#fafafa"/>

    <!-- Grid -->
    <line x1="30" y1="20" x2="30" y2="160" stroke="#ccc" stroke-width="1"/>
    <line x1="30" y1="160" x2="280" y2="160" stroke="#ccc" stroke-width="1"/>

    <!-- Temperature line -->
    <polyline points="50,120 80,110 110,100 140,90 170,85 200,95 230,105 260,100"
              fill="none" stroke="#FF5722" stroke-width="3" stroke-linecap="round"/>
</svg>
```

### Network Path

```html
<svg width="250" height="150">
    <!-- Connection path -->
    <polyline points="30,30 80,50 130,40 180,70 230,60"
              fill="none" stroke="#2196F3" stroke-width="2"
              stroke-dasharray="5,5"/>

    <!-- Nodes -->
    <circle cx="30" cy="30" r="8" fill="#4CAF50"/>
    <circle cx="80" cy="50" r="8" fill="#2196F3"/>
    <circle cx="130" cy="40" r="8" fill="#2196F3"/>
    <circle cx="180" cy="70" r="8" fill="#2196F3"/>
    <circle cx="230" cy="60" r="8" fill="#f44336"/>
</svg>
```

### Roof Shape

```html
<svg width="200" height="150">
    <!-- House -->
    <rect x="50" y="90" width="100" height="60" fill="#FFF8E1" stroke="#333" stroke-width="2"/>

    <!-- Roof -->
    <polyline points="40,90 100,40 160,90"
              fill="#8D6E63" stroke="#5D4037" stroke-width="2"/>
</svg>
```

### Progress Path

```html
<svg width="300" height="100">
    <!-- Background path -->
    <polyline points="30,50 100,50 150,30 200,50 270,50"
              fill="none" stroke="#e0e0e0" stroke-width="8" stroke-linecap="round"/>

    <!-- Progress path (60%) -->
    <polyline points="30,50 100,50 150,30 170,40"
              fill="none" stroke="#4CAF50" stroke-width="8" stroke-linecap="round"/>
</svg>
```

### Dynamic Chart from Array

```html
<!-- Model: { values: [30, 45, 35, 60, 50, 70, 55] } -->
<svg width="280" height="150">
    <polyline points="{{values.map((v, i) => `${20 + i * 40},${130 - v}`).join(' ')}}"
              fill="none" stroke="#3498db" stroke-width="3"
              stroke-linecap="round" stroke-linejoin="round"/>
</svg>
```

### Multi-Series with Data Binding

```html
<!-- Model: { datasets: [
    {name: 'Series A', data: [20,40,35,60,50], color: '#e74c3c'},
    {name: 'Series B', data: [30,35,45,50,55], color: '#3498db'},
    {name: 'Series C', data: [25,30,40,45,48], color: '#2ecc71'}
]} -->
<svg width="300" height="150">
    <template data-bind="{{datasets}}">
        <polyline points="{{.data.map((v, i) => `${30 + i * 60},${130 - v}`).join(' ')}}"
                  fill="none" stroke="{{.color}}" stroke-width="2"/>
    </template>
</svg>
```

---

## See Also

- [polygon](/reference/svgelements/polygon.html) - SVG polygon element (closed polyline)
- [line](/reference/svgelements/line.html) - SVG line element for single segments
- [path](/reference/svgelements/path.html) - SVG path element for curves and complex shapes
- [rect](/reference/svgelements/rect.html) - SVG rectangle element
- [svg](/reference/svgelements/svg.html) - SVG container element
- [SVG Markers](/reference/svgelements/markers.html) - Arrowheads and line decorations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
