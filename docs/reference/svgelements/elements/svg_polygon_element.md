---
layout: default
title: polygon
parent: SVG Elements
parent_url: /reference/svgelements/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;polygon&gt; : SVG Polygon Element

The `<polygon>` element draws a closed shape consisting of connected straight lines in SVG content within your PDF documents. Unlike `<polyline>`, polygons automatically connect the last point to the first, creating a closed path. They're perfect for creating stars, triangles, arrows, custom shapes, badges, and geometric diagrams.

## Usage

The `<polygon>` element creates a closed multi-sided shape defined by:
- A series of points (`points`) - coordinates defining each vertex
- Automatic closure between the last and first points
- Fill and stroke styling attributes
- Transform operations for rotation and positioning

```html
<svg xmlns="http://www.w3.org/2000/svg" width="150" height="150">
    <polygon points="75,20 100,80 50,80"
             fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

---

## Supported Attributes

### Geometry Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `points` | String | Space or comma-separated list of x,y coordinate pairs. Format: `"x1,y1 x2,y2 x3,y3"` or `"x1 y1 x2 y2 x3 y3"`. **Required**. Automatically closes path. |

### Styling Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `fill` | Color/URL | Fill color or reference to gradient/pattern. Default: black |
| `fill-opacity` | Number | Opacity of the fill (0.0 to 1.0). Default: 1.0 |
| `fill-rule` | String | Fill rule for complex polygons: `nonzero` or `evenodd`. Default: nonzero |
| `stroke` | Color | Stroke (border) color. Default: none |
| `stroke-width` | Unit | Width of the stroke line. Default: 1pt |
| `stroke-opacity` | Number | Opacity of the stroke (0.0 to 1.0). Default: 1.0 |
| `stroke-linecap` | String | Line cap style: `butt`, `round`, `square`. Default: square |
| `stroke-linejoin` | String | Line join style at vertices: `miter`, `round`, `bevel`. Default: bevel |
| `stroke-dasharray` | String | Dash pattern for the border (e.g., `5,3`) |

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

The `<polygon>` element supports dynamic attribute values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Triangle

```html
<!-- Model: { triangle: { points: "100,50 150,150 50,150", color: '#FF5722' } } -->
<svg width="200" height="200">
    <polygon points="{{triangle.points}}"
             fill="{{triangle.color}}"/>
</svg>
```

### Example 2: Star Rating

```html
{% raw %}
<!-- Model: { stars: 4 } -->
<svg width="150" height="30">
    <template data-bind="{{Array.from({length: stars}, (_, i) => i)}}">
        <polygon points="{{. * 30 + 10}},5 {{. * 30 + 12}},15 {{. * 30 + 20}},15 {{. * 30 + 14}},20 {{. * 30 + 16}},28 {{. * 30 + 10}},23 {{. * 30 + 4}},28 {{. * 30 + 6}},20 {{. * 30 + 0}},15 {{. * 30 + 8}},15"
                 fill="#FFC107"/>
    </template>
</svg>
{% endraw %}
```

### Example 3: Multiple Polygons

```html
<!-- Model: { shapes: [
    {points: '50,10 90,90 10,40 90,40 10,90', color: '#e74c3c'},
    {points: '150,10 190,90 110,40 190,40 110,90', color: '#3498db'}
]} -->
<svg width="220" height="110">
    <template data-bind="{{shapes}}">
        <polygon points="{{.points}}"
                 fill="{{.color}}" stroke="#333" stroke-width="2"/>
    </template>
</svg>
```

---

## Notes

### Automatic Closure

The polygon automatically connects the last point back to the first point. No need to repeat the starting coordinate.

### Points Format

The `points` attribute accepts coordinates in flexible formats:
- Comma-separated: `"10,20 30,40 50,60"`
- Space-separated: `"10 20 30 40 50 60"`
- Mixed: `"10,20, 30,40, 50,60"`

### Minimum Points

At least 3 points are required for a visible polygon. Fewer points produce no output or degenerate shapes.

### Fill Rule

The `fill-rule` attribute determines how complex self-intersecting polygons are filled:
- `nonzero` - Standard winding rule (default)
- `evenodd` - Alternating fill for self-intersecting paths

### Stroke Joins

The `stroke-linejoin` attribute affects how edges meet at vertices:
- `miter` - sharp corners (default)
- `round` - rounded corners
- `bevel` - beveled (cut) corners

### Regular Polygons

For regular polygons (equal sides and angles), calculate points using trigonometry or use predefined patterns.

---

## Examples

### Basic Triangle

```html
<svg width="150" height="150">
    <polygon points="75,20 140,130 10,130"
             fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Square (Diamond Orientation)

```html
<svg width="150" height="150">
    <polygon points="75,20 140,75 75,130 10,75"
             fill="#2196F3" stroke="#1565C0" stroke-width="2"/>
</svg>
```

### Pentagon

```html
<svg width="150" height="150">
    <polygon points="75,20 140,60 115,125 35,125 10,60"
             fill="#9C27B0" stroke="#6A1B9A" stroke-width="2"/>
</svg>
```

### Hexagon

```html
<svg width="150" height="150">
    <polygon points="75,10 130,40 130,100 75,130 20,100 20,40"
             fill="#FF9800" stroke="#E65100" stroke-width="2"/>
</svg>
```

### 5-Point Star

```html
<svg width="150" height="150">
    <polygon points="75,15 88,60 135,60 95,88 110,135 75,105 40,135 55,88 15,60 62,60"
             fill="#FFC107" stroke="#F57F17" stroke-width="2"/>
</svg>
```

### 6-Point Star (Star of David)

```html
<svg width="150" height="150">
    <!-- Upward triangle -->
    <polygon points="75,20 130,110 20,110"
             fill="#2196F3" opacity="0.7"/>
    <!-- Downward triangle -->
    <polygon points="75,130 20,40 130,40"
             fill="#2196F3" opacity="0.7"/>
</svg>
```

### Arrow Right

```html
<svg width="150" height="80">
    <polygon points="10,20 90,20 90,10 130,40 90,70 90,60 10,60"
             fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Arrow Down

```html
<svg width="80" height="120">
    <polygon points="20,10 60,10 60,70 70,70 40,110 10,70 20,70"
             fill="#f44336" stroke="#c62828" stroke-width="2"/>
</svg>
```

### Chevron

```html
<svg width="100" height="60">
    <polygon points="10,10 50,50 90,10 80,0 50,30 20,0"
             fill="#666"/>
</svg>
```

### House Icon

```html
<svg width="100" height="100">
    <!-- House body -->
    <polygon points="20,50 80,50 80,90 20,90"
             fill="#FFF8E1" stroke="#333" stroke-width="2"/>
    <!-- Roof -->
    <polygon points="10,50 50,15 90,50"
             fill="#8D6E63" stroke="#5D4037" stroke-width="2"/>
</svg>
```

### Shield

```html
<svg width="100" height="120">
    <polygon points="50,10 90,30 90,70 50,110 10,70 10,30"
             fill="#2196F3" stroke="#1565C0" stroke-width="2"/>
</svg>
```

### Badge

```html
<svg width="120" height="140">
    <polygon points="60,10 75,45 110,50 85,80 95,115 60,95 25,115 35,80 10,50 45,45"
             fill="#FFC107" stroke="#F57F17" stroke-width="2"/>
</svg>
```

### Diamond

```html
<svg width="100" height="120">
    <polygon points="50,10 90,60 50,110 10,60"
             fill="#E91E63" stroke="#AD1457" stroke-width="2"/>
</svg>
```

### Parallelogram

```html
<svg width="150" height="80">
    <polygon points="30,10 130,10 120,70 20,70"
             fill="#00BCD4" stroke="#0097A7" stroke-width="2"/>
</svg>
```

### Trapezoid

```html
<svg width="150" height="100">
    <polygon points="30,20 120,20 140,80 10,80"
             fill="#673AB7" stroke="#4527A0" stroke-width="2"/>
</svg>
```

### Play Button Triangle

```html
<svg width="80" height="80">
    <circle cx="40" cy="40" r="35" fill="#4CAF50"/>
    <polygon points="30,25 30,55 55,40"
             fill="#fff"/>
</svg>
```

### Stop Sign

```html
<svg width="120" height="120">
    <polygon points="35,10 85,10 110,35 110,85 85,110 35,110 10,85 10,35"
             fill="#f44336" stroke="#c62828" stroke-width="3"/>
</svg>
```

### Lightning Bolt

```html
<svg width="80" height="120">
    <polygon points="45,10 30,50 50,50 35,110 70,60 50,60"
             fill="#FFC107" stroke="#F57F17" stroke-width="2"/>
</svg>
```

### Speech Bubble Tail

```html
<svg width="150" height="100">
    <!-- Bubble -->
    <rect x="10" y="10" width="130" height="60" rx="10"
          fill="#fff" stroke="#333" stroke-width="2"/>
    <!-- Tail -->
    <polygon points="40,70 30,90 50,70"
             fill="#fff" stroke="#333" stroke-width="2"/>
</svg>
```

### Flag

```html
<svg width="150" height="100">
    <!-- Pole -->
    <rect x="10" y="10" width="5" height="80" fill="#333"/>
    <!-- Flag -->
    <polygon points="15,15 120,15 100,35 120,55 15,55"
             fill="#f44336" stroke="#c62828" stroke-width="2"/>
</svg>
```

### Pie Slice

```html
<svg width="120" height="120">
    <polygon points="60,60 60,10 110,60"
             fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Complex Star

```html
<svg width="150" height="150">
    <polygon points="75,10 85,50 125,50 92,75 105,115 75,90 45,115 58,75 25,50 65,50"
             fill="#FFC107" stroke="none"/>
</svg>
```

### Navigation Arrow

```html
<svg width="100" height="60">
    <polygon points="10,30 70,10 70,25 90,25 90,35 70,35 70,50"
             fill="#2196F3" stroke="#1565C0" stroke-width="2"/>
</svg>
```

### Bookmark

```html
<svg width="60" height="100">
    <polygon points="10,10 50,10 50,90 30,70 10,90"
             fill="#9C27B0" stroke="#6A1B9A" stroke-width="2"/>
</svg>
```

### Check Mark

```html
<svg width="100" height="100">
    <polygon points="20,50 40,70 80,20 90,30 40,90 10,60"
             fill="#4CAF50"/>
</svg>
```

### Location Pin

```html
<svg width="80" height="120">
    <polygon points="40,10 55,30 65,55 40,100 15,55 25,30"
             fill="#f44336" stroke="#c62828" stroke-width="2"/>
    <circle cx="40" cy="35" r="10" fill="#fff"/>
</svg>
```

### Dynamic Star from Rating

```html
{% raw %}
<!-- Model: { rating: 5 } -->
<svg width="200" height="40">
    <template data-bind="{{Array.from({length: rating}, (_, i) => i)}}">
        <polygon points="{{. * 40 + 10}},5 {{. * 40 + 13}},15 {{. * 40 + 23}},15 {{. * 40 + 16}},21 {{. * 40 + 19}},30 {{. * 40 + 10}},25 {{. * 40 + 1}},30 {{. * 40 + 4}},21 {{. * 40 - 3}},15 {{. * 40 + 7}},15"
                 fill="#FFC107"/>
    </template>
</svg>
{% endraw %}
```

### Directional Arrows

```html
<!-- Model: { directions: ['up', 'right', 'down', 'left'] } -->
<svg width="200" height="200">
    <!-- Up arrow -->
    <polygon points="100,20 120,60 105,60 105,100 95,100 95,60 80,60"
             fill="#2196F3"/>
    <!-- Right arrow -->
    <polygon points="180,100 140,80 140,95 100,95 100,105 140,105 140,120"
             fill="#2196F3"/>
    <!-- Down arrow -->
    <polygon points="100,180 80,140 95,140 95,100 105,100 105,140 120,140"
             fill="#2196F3"/>
    <!-- Left arrow -->
    <polygon points="20,100 60,80 60,95 100,95 100,105 60,105 60,120"
             fill="#2196F3"/>
</svg>
```

---

## See Also

- [polyline](/reference/svgelements/polyline.html) - SVG polyline element (open path)
- [path](/reference/svgelements/path.html) - SVG path element for curves and complex shapes
- [rect](/reference/svgelements/rect.html) - SVG rectangle element
- [circle](/reference/svgelements/circle.html) - SVG circle element
- [svg](/reference/svgelements/svg.html) - SVG container element
- [Data Binding](/reference/binding/) - Data binding and expressions

---
