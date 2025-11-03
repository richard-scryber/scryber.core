---
layout: default
title: circle
parent: SVG Elements
parent_url: /reference/svgelements/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;circle&gt; : SVG Circle Element

The `<circle>` element draws a perfect circle shape in SVG content within your PDF documents. It's ideal for creating dots, indicators, icons, pie charts, avatars, and decorative elements.

## Usage

The `<circle>` element creates a circular shape defined by:
- Center position (`cx`, `cy`) - coordinates of the circle's center point
- Radius (`r`) - distance from center to edge
- Styling attributes for fill, stroke, and visual effects
- Transform operations for positioning and scaling

```html
<svg xmlns="http://www.w3.org/2000/svg" width="100" height="100">
    <circle cx="50" cy="50" r="40"
            fill="#ff6347" stroke="#000" stroke-width="2"/>
</svg>
```

---

## Supported Attributes

### Geometry Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `cx` | Unit | X-coordinate of the circle's center. Default: 0 |
| `cy` | Unit | Y-coordinate of the circle's center. Default: 0 |
| `r` | Unit | Radius of the circle. Required for visible output. |

### Styling Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `fill` | Color/URL | Fill color or reference to gradient/pattern (e.g., `#ff0000`, `rgb(255,0,0)`, `url(#gradient1)`). Default: black |
| `fill-opacity` | Number | Opacity of the fill (0.0 to 1.0). Default: 1.0 |
| `stroke` | Color | Stroke (border) color. Default: none |
| `stroke-width` | Unit | Width of the stroke line. Default: 1pt |
| `stroke-opacity` | Number | Opacity of the stroke (0.0 to 1.0). Default: 1.0 |
| `stroke-dasharray` | String | Dash pattern for circular borders (e.g., `5,3`) |

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

The `<circle>` element supports dynamic attribute values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Size and Color

```html
<!-- Model: { dot: { cx: 50, cy: 50, radius: 25, color: '#4CAF50' } } -->
<svg width="100" height="100">
    <circle cx="{{dot.cx}}" cy="{{dot.cy}}" r="{{dot.radius}}"
            fill="{{dot.color}}"/>
</svg>
```

### Example 2: Status Indicators

```html
<!-- Model: { status: { online: true, x: 80, y: 20 } } -->
<svg width="100" height="40">
    <circle cx="{{status.x}}" cy="{{status.y}}" r="8"
            fill="{{status.online ? '#4CAF50' : '#f44336'}}"/>
</svg>
```

### Example 3: Bubble Chart with Template

```html
<!-- Model: { bubbles: [
    {cx: 50, cy: 50, r: 30, color: '#e74c3c'},
    {cx: 120, cy: 60, r: 20, color: '#3498db'},
    {cx: 180, cy: 45, r: 25, color: '#2ecc71'}
]} -->
<svg width="220" height="120">
    <template data-bind="{{bubbles}}">
        <circle cx="{{.cx}}" cy="{{.cy}}" r="{{.r}}"
                fill="{{.color}}" fill-opacity="0.7"
                stroke="#333" stroke-width="1"/>
    </template>
</svg>
```

---

## Notes

### Perfect Circles

The `<circle>` element always produces a perfect circle. For oval shapes, use the `<ellipse>` element with different `rx` and `ry` values.

### Center-Based Positioning

Unlike rectangles which are positioned by their top-left corner, circles are positioned by their center point. This makes circular layouts and radial arrangements more intuitive.

### Radius Constraints

- Radius must be positive
- A radius of 0 produces no visible output
- Very large radius values may exceed the SVG viewport

### Stroke Width

The stroke is drawn centered on the circle's outline, extending both inward and outward from the radius. For precise sizing, account for half the stroke-width extending beyond the radius.

### Transform Scaling

When applying `scale()` transforms to circles, non-uniform scaling (different x and y values) will distort the circle into an ellipse. Use uniform scaling or the `<ellipse>` element for oval shapes.

---

## Examples

### Basic Circle

```html
<svg width="100" height="100">
    <circle cx="50" cy="50" r="40" fill="#336699"/>
</svg>
```

### Circle with Stroke

```html
<svg width="100" height="100">
    <circle cx="50" cy="50" r="40"
            fill="#fff" stroke="#336699" stroke-width="4"/>
</svg>
```

### Hollow Circle (Ring)

```html
<svg width="100" height="100">
    <circle cx="50" cy="50" r="40"
            fill="none" stroke="#333" stroke-width="6"/>
</svg>
```

### Traffic Light Dots

```html
<svg width="100" height="300">
    <circle cx="50" cy="50" r="30" fill="#c0392b"/>
    <circle cx="50" cy="150" r="30" fill="#f39c12"/>
    <circle cx="50" cy="250" r="30" fill="#27ae60"/>
</svg>
```

### Status Indicator

```html
<svg width="30" height="30">
    <circle cx="15" cy="15" r="8" fill="#4CAF50"/>
</svg>
```

### Avatar Placeholder

```html
<svg width="80" height="80">
    <circle cx="40" cy="40" r="35"
            fill="#9e9e9e" stroke="#757575" stroke-width="2"/>
    <circle cx="40" cy="32" r="12" fill="#fff"/>
    <circle cx="40" cy="65" r="18" fill="#fff"/>
</svg>
```

### Radio Button

```html
<svg width="24" height="24">
    <!-- Outer circle -->
    <circle cx="12" cy="12" r="10"
            fill="none" stroke="#2196F3" stroke-width="2"/>
    <!-- Inner circle (selected) -->
    <circle cx="12" cy="12" r="5" fill="#2196F3"/>
</svg>
```

### Loading Spinner Dots

```html
<svg width="100" height="30">
    <circle cx="15" cy="15" r="8" fill="#2196F3" opacity="1.0"/>
    <circle cx="40" cy="15" r="8" fill="#2196F3" opacity="0.7"/>
    <circle cx="65" cy="15" r="8" fill="#2196F3" opacity="0.4"/>
    <circle cx="90" cy="15" r="8" fill="#2196F3" opacity="0.2"/>
</svg>
```

### Pie Chart Marker

```html
<svg width="120" height="120">
    <!-- Background circle -->
    <circle cx="60" cy="60" r="50"
            fill="none" stroke="#e0e0e0" stroke-width="20"/>
    <!-- Progress arc (75%) - simulated with stroke -->
    <circle cx="60" cy="60" r="50"
            fill="none" stroke="#4CAF50" stroke-width="20"
            stroke-dasharray="235.5 78.5"
            transform="rotate(-90, 60, 60)"/>
</svg>
```

### Target Icon

```html
<svg width="100" height="100">
    <circle cx="50" cy="50" r="45"
            fill="none" stroke="#e74c3c" stroke-width="2"/>
    <circle cx="50" cy="50" r="32"
            fill="none" stroke="#e74c3c" stroke-width="2"/>
    <circle cx="50" cy="50" r="19"
            fill="none" stroke="#e74c3c" stroke-width="2"/>
    <circle cx="50" cy="50" r="6" fill="#e74c3c"/>
</svg>
```

### Notification Badge

```html
<svg width="30" height="30">
    <circle cx="15" cy="15" r="13"
            fill="#f44336" stroke="#fff" stroke-width="2"/>
</svg>
```

### Rating Dots

```html
<svg width="150" height="30">
    <circle cx="15" cy="15" r="10" fill="#ff9800"/>
    <circle cx="45" cy="15" r="10" fill="#ff9800"/>
    <circle cx="75" cy="15" r="10" fill="#ff9800"/>
    <circle cx="105" cy="15" r="10" fill="#e0e0e0"/>
    <circle cx="135" cy="15" r="10" fill="#e0e0e0"/>
</svg>
```

### Stoplight

```html
<svg width="60" height="180">
    <rect x="5" y="5" width="50" height="170" rx="10" fill="#333"/>
    <circle cx="30" cy="40" r="18" fill="#c0392b"/>
    <circle cx="30" cy="90" r="18" fill="#34495e"/>
    <circle cx="30" cy="140" r="18" fill="#34495e"/>
</svg>
```

### Bubble Levels

```html
<svg width="250" height="60">
    <circle cx="30" cy="30" r="25" fill="#2196F3" opacity="0.3"/>
    <circle cx="90" cy="30" r="22" fill="#2196F3" opacity="0.5"/>
    <circle cx="145" cy="30" r="18" fill="#2196F3" opacity="0.7"/>
    <circle cx="190" cy="30" r="14" fill="#2196F3" opacity="0.9"/>
    <circle cx="225" cy="30" r="10" fill="#2196F3" opacity="1.0"/>
</svg>
```

### Venn Diagram

```html
<svg width="200" height="150">
    <circle cx="70" cy="75" r="50"
            fill="#e74c3c" fill-opacity="0.5"/>
    <circle cx="130" cy="75" r="50"
            fill="#3498db" fill-opacity="0.5"/>
</svg>
```

### Play Button Icon

```html
<svg width="80" height="80">
    <circle cx="40" cy="40" r="35"
            fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
    <!-- Triangle (play symbol) -->
    <polygon points="32,28 32,52 54,40"
             fill="#fff"/>
</svg>
```

### Profile Initial

```html
<svg width="60" height="60">
    <circle cx="30" cy="30" r="28"
            fill="#9C27B0" stroke="#7B1FA2" stroke-width="2"/>
</svg>
```

### Connection Nodes

```html
<svg width="200" height="100">
    <!-- Line connecting nodes -->
    <line x1="30" y1="50" x2="170" y2="50"
          stroke="#999" stroke-width="2"/>
    <!-- Start node -->
    <circle cx="30" cy="50" r="12"
            fill="#4CAF50" stroke="#fff" stroke-width="2"/>
    <!-- Middle node -->
    <circle cx="100" cy="50" r="10"
            fill="#2196F3" stroke="#fff" stroke-width="2"/>
    <!-- End node -->
    <circle cx="170" cy="50" r="12"
            fill="#f44336" stroke="#fff" stroke-width="2"/>
</svg>
```

### Scatter Plot Points

```html
<!-- Model: { points: [
    {x: 30, y: 70, r: 5},
    {x: 50, y: 40, r: 8},
    {x: 90, y: 60, r: 6},
    {x: 120, y: 30, r: 7}
]} -->
<svg width="150" height="100">
    <template data-bind="{{points}}">
        <circle cx="{{.x}}" cy="{{.y}}" r="{{.r}}"
                fill="#2196F3" opacity="0.7"/>
    </template>
</svg>
```

### Dynamic Status Dots

```html
<!-- Model: { servers: [
    {id: 1, online: true},
    {id: 2, online: true},
    {id: 3, online: false}
]} -->
<svg width="100" height="30">
    <template data-bind="{{servers}}">
        <circle cx="{{.id * 30}}" cy="15" r="8"
                fill="{{.online ? '#4CAF50' : '#f44336'}}"/>
    </template>
</svg>
```

### Percentage Indicator

```html
<!-- Model: { completion: 0.75 } -->
<svg width="60" height="60">
    <!-- Background -->
    <circle cx="30" cy="30" r="25"
            fill="none" stroke="#e0e0e0" stroke-width="8"/>
    <!-- Progress -->
    <circle cx="30" cy="30" r="25"
            fill="none" stroke="#4CAF50" stroke-width="8"
            stroke-dasharray="{{157 * completion}} {{157 * (1 - completion)}}"
            transform="rotate(-90, 30, 30)"/>
</svg>
```

---

## See Also

- [ellipse](/reference/svgelements/ellipse.html) - SVG ellipse element for oval shapes
- [rect](/reference/svgelements/rect.html) - SVG rectangle element
- [polygon](/reference/svgelements/polygon.html) - SVG polygon element
- [path](/reference/svgelements/path.html) - SVG path element for complex shapes
- [svg](/reference/svgelements/svg.html) - SVG container element
- [SVG Gradients](/reference/svgelements/gradients.html) - Linear and radial gradients
- [Data Binding](/reference/binding/) - Data binding and expressions

---
