---
layout: default
title: rect
parent: SVG Elements
parent_url: /reference/svgelements/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;rect&gt; : SVG Rectangle Element

The `<rect>` element draws a rectangle shape in SVG content within your PDF documents. It supports rounded corners, fills, strokes, and transformations, making it versatile for creating boxes, borders, charts, badges, and layout elements.

## Usage

The `<rect>` element creates a rectangular shape defined by:
- Position (`x`, `y`) - coordinates of the top-left corner
- Dimensions (`width`, `height`) - size of the rectangle
- Optional rounded corners (`rx`, `ry`) - corner radius values
- Styling attributes for fill, stroke, and visual effects
- Transform operations for rotation, scaling, and positioning

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="100">
    <rect x="10" y="10" width="180" height="80"
          fill="#336699" stroke="#000" stroke-width="2"/>
</svg>
```

---

## Supported Attributes

### Geometry Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `x` | Unit | X-coordinate of the rectangle's top-left corner. Default: 0 |
| `y` | Unit | Y-coordinate of the rectangle's top-left corner. Default: 0 |
| `width` | Unit | Width of the rectangle. Required for visible output. |
| `height` | Unit | Height of the rectangle. Required for visible output. |
| `rx` | Unit | Horizontal radius for rounded corners. Creates smooth corners. |
| `ry` | Unit | Vertical radius for rounded corners. Defaults to `rx` if not specified. |

### Styling Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `fill` | Color/URL | Fill color or reference to gradient/pattern (e.g., `#ff0000`, `rgb(255,0,0)`, `url(#gradient1)`). Default: black |
| `fill-opacity` | Number | Opacity of the fill (0.0 to 1.0). Default: 1.0 |
| `stroke` | Color | Stroke (border) color. Default: none |
| `stroke-width` | Unit | Width of the stroke line. Default: 1pt |
| `stroke-opacity` | Number | Opacity of the stroke (0.0 to 1.0). Default: 1.0 |
| `stroke-linecap` | String | Line cap style: `butt`, `round`, `square`. Default: square |
| `stroke-dasharray` | String | Dash pattern (e.g., `5,5` for dashed line) |

### Transform Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `transform` | String | SVG transform operations: `translate(x,y)`, `rotate(angle)`, `scale(x,y)`, `matrix(...)` |
| `transform-origin` | String | Origin point for transformations (e.g., `center`, `50% 50%`) |

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

The `<rect>` element supports dynamic attribute values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Position and Size

```html
<!-- Model: { chart: { x: 50, y: 100, width: 200, height: 150 } } -->
<svg width="300" height="300">
    <rect x="{{chart.x}}" y="{{chart.y}}"
          width="{{chart.width}}" height="{{chart.height}}"
          fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Example 2: Dynamic Colors from Data

```html
<!-- Model: { status: 'success', color: '#4CAF50' } -->
<svg width="100" height="40">
    <rect x="5" y="5" width="90" height="30" rx="5"
          fill="{{color}}" stroke="#333" stroke-width="1"/>
</svg>
```

### Example 3: Repeating Rectangles with Template

```html
<!-- Model: { bars: [
    {x: 10, height: 80, color: '#ff0000'},
    {x: 40, height: 120, color: '#00ff00'},
    {x: 70, height: 60, color: '#0000ff'}
]} -->
<svg width="120" height="150">
    <template data-bind="{{bars}}">
        <rect x="{{.x}}" y="{{150 - .height}}"
              width="20" height="{{.height}}"
              fill="{{.color}}"/>
    </template>
</svg>
```

---

## Notes

### Rounded Corners

When `rx` or `ry` are specified, the rectangle will have rounded corners:
- If only `rx` is specified, `ry` defaults to the same value
- If only `ry` is specified, `rx` defaults to the same value
- The actual corner radius is capped at half the width/height to prevent overlapping

### Fill and Stroke

- Default fill is black if not specified
- Stroke is not rendered unless `stroke` attribute is set
- Both fill and stroke can be applied simultaneously
- Use `fill="none"` to create outlined shapes without fill

### Coordinate System

- SVG uses a coordinate system where (0,0) is the top-left corner
- Positive X extends to the right
- Positive Y extends downward
- Units can be specified (pt, px, mm, cm, in) or default to points

### Transform Operations

Transforms are applied in the order specified and can be combined:
- `translate(x, y)` - moves the rectangle
- `rotate(angle)` or `rotate(angle, cx, cy)` - rotates around a point
- `scale(x, y)` - scales the rectangle
- Multiple transforms: `transform="translate(50,50) rotate(45)"`

---

## Examples

### Basic Rectangle

```html
<svg width="150" height="100">
    <rect x="10" y="10" width="130" height="80"
          fill="#336699"/>
</svg>
```

### Rectangle with Stroke

```html
<svg width="150" height="100">
    <rect x="10" y="10" width="130" height="80"
          fill="#fff" stroke="#336699" stroke-width="3"/>
</svg>
```

### Rounded Rectangle (Badge)

```html
<svg width="120" height="40">
    <rect x="5" y="5" width="110" height="30" rx="8" ry="8"
          fill="#ff6347" stroke="#cc2900" stroke-width="1"/>
    <text x="60" y="27" text-anchor="middle"
          font-size="14" fill="#fff" font-weight="bold">NEW</text>
</svg>
```

### Dashed Border Rectangle

```html
<svg width="150" height="100">
    <rect x="10" y="10" width="130" height="80"
          fill="none" stroke="#333" stroke-width="2"
          stroke-dasharray="5,3"/>
</svg>
```

### Semi-Transparent Rectangle

```html
<svg width="150" height="100">
    <rect x="10" y="10" width="60" height="80"
          fill="#ff0000" fill-opacity="0.5"/>
    <rect x="40" y="10" width="60" height="80"
          fill="#0000ff" fill-opacity="0.5"/>
</svg>
```

### Rotated Rectangle

```html
<svg width="150" height="150">
    <rect x="50" y="50" width="50" height="50"
          fill="#4CAF50"
          transform="rotate(45, 75, 75)"/>
</svg>
```

### Bar Chart Using Rectangles

```html
<svg width="300" height="200">
    <!-- Y-axis -->
    <line x1="30" y1="20" x2="30" y2="180" stroke="#333" stroke-width="2"/>
    <!-- X-axis -->
    <line x1="30" y1="180" x2="280" y2="180" stroke="#333" stroke-width="2"/>

    <!-- Bars -->
    <rect x="50" y="80" width="40" height="100" fill="#e74c3c"/>
    <rect x="110" y="50" width="40" height="130" fill="#3498db"/>
    <rect x="170" y="100" width="40" height="80" fill="#2ecc71"/>
    <rect x="230" y="60" width="40" height="120" fill="#f39c12"/>
</svg>
```

### Progress Bar

```html
<svg width="300" height="30">
    <!-- Background -->
    <rect x="0" y="0" width="300" height="30" rx="15"
          fill="#e0e0e0"/>
    <!-- Progress (75%) -->
    <rect x="0" y="0" width="225" height="30" rx="15"
          fill="#4CAF50"/>
</svg>
```

### Checkerboard Pattern

```html
<svg width="200" height="200">
    <rect x="0" y="0" width="50" height="50" fill="#000"/>
    <rect x="50" y="0" width="50" height="50" fill="#fff"/>
    <rect x="100" y="0" width="50" height="50" fill="#000"/>
    <rect x="150" y="0" width="50" height="50" fill="#fff"/>

    <rect x="0" y="50" width="50" height="50" fill="#fff"/>
    <rect x="50" y="50" width="50" height="50" fill="#000"/>
    <rect x="100" y="50" width="50" height="50" fill="#fff"/>
    <rect x="150" y="50" width="50" height="50" fill="#000"/>

    <rect x="0" y="100" width="50" height="50" fill="#000"/>
    <rect x="50" y="100" width="50" height="50" fill="#fff"/>
    <rect x="100" y="100" width="50" height="50" fill="#000"/>
    <rect x="150" y="100" width="50" height="50" fill="#fff"/>

    <rect x="0" y="150" width="50" height="50" fill="#fff"/>
    <rect x="50" y="150" width="50" height="50" fill="#000"/>
    <rect x="100" y="150" width="50" height="50" fill="#fff"/>
    <rect x="150" y="150" width="50" height="50" fill="#000"/>
</svg>
```

### Color Swatch Card

```html
<svg width="100" height="120">
    <!-- Color sample -->
    <rect x="10" y="10" width="80" height="60" rx="4"
          fill="#ff6347"/>
    <!-- Label background -->
    <rect x="10" y="75" width="80" height="35" rx="4"
          fill="#f5f5f5" stroke="#ccc" stroke-width="1"/>
</svg>
```

### Document Page Outline

```html
<svg width="120" height="160">
    <!-- Page shadow -->
    <rect x="8" y="8" width="100" height="140" rx="2"
          fill="#999" opacity="0.3"/>
    <!-- Page -->
    <rect x="5" y="5" width="100" height="140" rx="2"
          fill="#fff" stroke="#333" stroke-width="2"/>
    <!-- Header line -->
    <rect x="15" y="20" width="80" height="3" rx="1"
          fill="#336699"/>
</svg>
```

### Status Indicator Badge

```html
<!-- Model: { status: 'online', color: '#4CAF50' } -->
<svg width="80" height="30">
    <rect x="2" y="2" width="76" height="26" rx="13"
          fill="{{color}}" stroke="#fff" stroke-width="2"/>
</svg>
```

### Card with Border Accent

```html
<svg width="250" height="150">
    <!-- Card background -->
    <rect x="5" y="5" width="240" height="140" rx="8"
          fill="#fff" stroke="#ddd" stroke-width="1"/>
    <!-- Left accent border -->
    <rect x="5" y="5" width="6" height="140" rx="8"
          fill="#2196F3"/>
</svg>
```

### Traffic Light

```html
<svg width="80" height="200">
    <!-- Housing -->
    <rect x="10" y="10" width="60" height="180" rx="10"
          fill="#333" stroke="#000" stroke-width="2"/>
    <!-- Red light -->
    <circle cx="40" cy="45" r="20" fill="#c0392b"/>
    <!-- Yellow light -->
    <circle cx="40" cy="100" r="20" fill="#f39c12"/>
    <!-- Green light -->
    <circle cx="40" cy="155" r="20" fill="#27ae60"/>
</svg>
```

### Button with Shadow

```html
<svg width="150" height="60">
    <!-- Shadow -->
    <rect x="13" y="13" width="130" height="40" rx="5"
          fill="#000" opacity="0.2"/>
    <!-- Button -->
    <rect x="10" y="10" width="130" height="40" rx="5"
          fill="#2196F3" stroke="#1976D2" stroke-width="2"/>
</svg>
```

### Calendar Date Cell

```html
<svg width="60" height="70">
    <!-- Header -->
    <rect x="5" y="5" width="50" height="20"
          fill="#d32f2f"/>
    <!-- Body -->
    <rect x="5" y="25" width="50" height="40"
          fill="#fff" stroke="#ccc" stroke-width="1"/>
</svg>
```

### Dynamic Width Progress Bar

```html
<!-- Model: { progress: 0.65 } -->
<svg width="400" height="40">
    <!-- Background -->
    <rect x="10" y="10" width="380" height="20" rx="10"
          fill="#e0e0e0"/>
    <!-- Progress -->
    <rect x="10" y="10" width="{{380 * progress}}" height="20" rx="10"
          fill="#4CAF50"/>
</svg>
```

### Table Cell Background

```html
<!-- Model: { cells: [{x:0, even:true}, {x:100, even:false}, {x:200, even:true}] } -->
<svg width="300" height="40">
    <template data-bind="{{cells}}">
        <rect x="{{.x}}" y="0" width="100" height="40"
              fill="{{.even ? '#f5f5f5' : '#fff'}}"/>
    </template>
</svg>
```

### Dynamic Status Badges

```html
<!-- Model: { items: [
    {x:10, status:'error', color:'#f44336'},
    {x:90, status:'warning', color:'#ff9800'},
    {x:170, status:'success', color:'#4caf50'}
]} -->
<svg width="250" height="40">
    <template data-bind="{{items}}">
        <rect x="{{.x}}" y="10" width="70" height="25" rx="5"
              fill="{{.color}}"/>
    </template>
</svg>
```

### Heat Map Cell

```html
<!-- Model: { intensity: 0.75 } -->
<svg width="30" height="30">
    <rect x="2" y="2" width="26" height="26" rx="3"
          fill="#ff0000"
          fill-opacity="{{intensity}}"/>
</svg>
```

---

## See Also

- [circle](/reference/svgelements/circle.html) - SVG circle element
- [ellipse](/reference/svgelements/ellipse.html) - SVG ellipse element
- [polygon](/reference/svgelements/polygon.html) - SVG polygon element
- [path](/reference/svgelements/path.html) - SVG path element for complex shapes
- [svg](/reference/svgelements/svg.html) - SVG container element
- [SVG Gradients](/reference/svgelements/gradients.html) - Linear and radial gradients
- [SVG Transforms](/reference/svgelements/transforms.html) - Transformation operations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
