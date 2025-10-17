---
layout: default
title: path
parent: SVG Elements
parent_url: /reference/svgelements/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;path&gt; : SVG Path Element

The `<path>` element is the most powerful and flexible SVG shape element, capable of drawing any arbitrary shape using a series of commands in SVG content within your PDF documents. It supports straight lines, curves, arcs, and complex combinations, making it essential for icons, custom shapes, logos, and advanced graphics.

## Usage

The `<path>` element creates complex shapes defined by:
- Path data (`d`) - a string of commands and coordinates defining the shape
- Fill and stroke styling attributes
- Transform operations for rotation and positioning
- Support for both open and closed paths

```html
<svg xmlns="http://www.w3.org/2000/svg" width="100" height="100">
    <path d="M 10,30 A 20,20 0 0,1 50,30 A 20,20 0 0,1 90,30"
          fill="none" stroke="#2196F3" stroke-width="2"/>
</svg>
```

---

## Supported Attributes

### Geometry Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `d` | String | Path data string containing drawing commands. **Required**. See Path Commands section below. |

### Styling Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `fill` | Color/URL | Fill color or reference to gradient/pattern. Default: black |
| `fill-opacity` | Number | Opacity of the fill (0.0 to 1.0). Default: 1.0 |
| `fill-rule` | String | Fill rule for complex paths: `nonzero` or `evenodd`. Default: nonzero |
| `stroke` | Color | Stroke (border) color. Default: none |
| `stroke-width` | Unit | Width of the stroke line. Default: 1pt |
| `stroke-opacity` | Number | Opacity of the stroke (0.0 to 1.0). Default: 1.0 |
| `stroke-linecap` | String | Line cap style: `butt`, `round`, `square`. Default: square |
| `stroke-linejoin` | String | Line join style: `miter`, `round`, `bevel`. Default: bevel |
| `stroke-dasharray` | String | Dash pattern (e.g., `5,3`) |

### Marker Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `marker-start` | URL | Reference to marker for path start (e.g., `url(#arrow)`) |
| `marker-mid` | URL | Reference to marker for path vertices |
| `marker-end` | URL | Reference to marker for path end |

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

## Path Commands

The `d` attribute uses a mini-language of commands and coordinates:

### Move Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `M x,y` | Absolute coordinates | Move to position (x,y) without drawing |
| `m dx,dy` | Relative coordinates | Move by offset (dx,dy) from current position |

### Line Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `L x,y` | Absolute coordinates | Draw line to (x,y) |
| `l dx,dy` | Relative coordinates | Draw line by offset (dx,dy) |
| `H x` | Absolute x | Draw horizontal line to x coordinate |
| `h dx` | Relative x | Draw horizontal line by dx |
| `V y` | Absolute y | Draw vertical line to y coordinate |
| `v dy` | Relative y | Draw vertical line by dy |

### Curve Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `C x1,y1 x2,y2 x,y` | Cubic Bezier | Curve to (x,y) with control points (x1,y1) and (x2,y2) |
| `c dx1,dy1 dx2,dy2 dx,dy` | Relative cubic | Relative cubic Bezier curve |
| `S x2,y2 x,y` | Smooth cubic | Smooth curve to (x,y) with control point (x2,y2) |
| `s dx2,dy2 dx,dy` | Relative smooth cubic | Relative smooth curve |
| `Q x1,y1 x,y` | Quadratic Bezier | Curve to (x,y) with control point (x1,y1) |
| `q dx1,dy1 dx,dy` | Relative quadratic | Relative quadratic curve |
| `T x,y` | Smooth quadratic | Smooth curve to (x,y) |
| `t dx,dy` | Relative smooth quad | Relative smooth quadratic |

### Arc Commands

| Command | Parameters | Description |
|---------|------------|-------------|
| `A rx,ry rotation large-arc sweep x,y` | Elliptical arc | Arc to (x,y) with radii rx,ry |
| `a rx,ry rotation large-arc sweep dx,dy` | Relative arc | Relative elliptical arc |

### Close Path

| Command | Parameters | Description |
|---------|------------|-------------|
| `Z` or `z` | None | Close path by drawing line to first point |

**Note:** Commands are case-sensitive. Uppercase = absolute coordinates, lowercase = relative coordinates.

---

## Data Binding

The `<path>` element supports dynamic path data using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Path Data

```html
<!-- Model: { pathData: 'M 10,50 L 50,20 L 90,60' } -->
<svg width="100" height="80">
    <path d="{{pathData}}"
          fill="none" stroke="#2196F3" stroke-width="2"/>
</svg>
```

### Example 2: Calculated Curve

```html
<!-- Model: { startX: 20, startY: 50, endX: 180, endY: 50, curve: 40 } -->
<svg width="200" height="100">
    <path d="M {{startX}},{{startY}} Q {{(startX + endX) / 2}},{{startY - curve}} {{endX}},{{endY}}"
          fill="none" stroke="#4CAF50" stroke-width="3"/>
</svg>
```

### Example 3: Multiple Dynamic Paths

```html
<!-- Model: { paths: [
    {d: 'M 10,40 C 40,10 60,10 90,40', color: '#e74c3c'},
    {d: 'M 10,60 C 40,90 60,90 90,60', color: '#3498db'}
]} -->
<svg width="100" height="100">
    <template data-bind="{{paths}}">
        <path d="{{.d}}"
              fill="none" stroke="{{.color}}" stroke-width="2"/>
    </template>
</svg>
```

---

## Notes

### Path Complexity

Paths can be arbitrarily complex, combining multiple command types. This makes them the most versatile but also most complex SVG shape element.

### Coordinate Systems

- Uppercase commands use absolute coordinates relative to the SVG origin
- Lowercase commands use relative coordinates from the current position
- Mix both types in a single path for flexible drawing

### Performance

While paths are powerful, simpler shapes (rect, circle, etc.) are more efficient when possible. Use paths for shapes that cannot be created with simpler elements.

### Path Parsing

The path data string is parsed into drawing operations. Invalid syntax will result in no output or partial rendering.

### Closing Paths

- Use `Z` or `z` to close a path (connects last point to first)
- Closing affects fill behavior and stroke joins at the closure point
- Open paths can still have fill (closes implicitly for fill only)

---

## Examples

### Basic Line Path

```html
<svg width="100" height="100">
    <path d="M 10,10 L 90,90"
          stroke="#333" stroke-width="2"/>
</svg>
```

### Triangle (Closed Path)

```html
<svg width="100" height="100">
    <path d="M 50,10 L 90,80 L 10,80 Z"
          fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Quadratic Curve

```html
<svg width="200" height="100">
    <path d="M 10,80 Q 100,10 190,80"
          fill="none" stroke="#2196F3" stroke-width="3"/>
</svg>
```

### Cubic Bezier Curve

```html
<svg width="200" height="100">
    <path d="M 10,50 C 40,10 160,90 190,50"
          fill="none" stroke="#9C27B0" stroke-width="3"/>
</svg>
```

### S-Curve

```html
<svg width="200" height="150">
    <path d="M 20,75 C 40,20 60,20 80,75 S 120,130 140,75 S 180,20 200,75"
          fill="none" stroke="#FF9800" stroke-width="3"/>
</svg>
```

### Heart Shape

```html
<svg width="100" height="100">
    <path d="M 50,80 C 50,70 45,60 35,60 C 25,60 20,65 20,75 C 20,90 50,100 50,100 C 50,100 80,90 80,75 C 80,65 75,60 65,60 C 55,60 50,70 50,80 Z"
          fill="#f44336" stroke="#c62828" stroke-width="2"/>
</svg>
```

### Cloud Icon

```html
<svg width="120" height="80">
    <path d="M 30,60 Q 10,60 10,45 Q 10,30 25,30 Q 25,15 40,15 Q 55,15 55,25 Q 70,20 80,30 Q 95,30 95,45 Q 95,60 80,60 Z"
          fill="#90CAF9" stroke="#42A5F5" stroke-width="2"/>
</svg>
```

### Infinity Symbol

```html
<svg width="200" height="100">
    <path d="M 50,50 C 30,20 0,30 0,50 C 0,70 30,80 50,50 C 70,20 130,20 150,50 C 170,80 200,70 200,50 C 200,30 170,20 150,50"
          fill="none" stroke="#673AB7" stroke-width="4"/>
</svg>
```

### Arrow with Curve

```html
<svg width="150" height="100">
    <path d="M 10,50 Q 75,10 140,50 L 130,55 L 140,50 L 130,45"
          fill="none" stroke="#4CAF50" stroke-width="3" stroke-linejoin="round"/>
</svg>
```

### Star Using Path

```html
<svg width="100" height="100">
    <path d="M 50,10 L 61,40 L 93,40 L 68,58 L 79,88 L 50,68 L 21,88 L 32,58 L 7,40 L 39,40 Z"
          fill="#FFC107" stroke="#F57F17" stroke-width="2"/>
</svg>
```

### Wave Pattern

```html
<svg width="300" height="100">
    <path d="M 0,50 Q 25,20 50,50 T 100,50 T 150,50 T 200,50 T 250,50 T 300,50"
          fill="none" stroke="#00BCD4" stroke-width="3"/>
</svg>
```

### Speech Bubble

```html
<svg width="150" height="100">
    <path d="M 20,20 L 130,20 Q 140,20 140,30 L 140,70 Q 140,80 130,80 L 50,80 L 30,95 L 35,80 L 20,80 Q 10,80 10,70 L 10,30 Q 10,20 20,20 Z"
          fill="#fff" stroke="#333" stroke-width="2"/>
</svg>
```

### Checkmark

```html
<svg width="100" height="100">
    <path d="M 20,50 L 40,70 L 80,25"
          fill="none" stroke="#4CAF50" stroke-width="6" stroke-linecap="round" stroke-linejoin="round"/>
</svg>
```

### Cross (X)

```html
<svg width="100" height="100">
    <path d="M 20,20 L 80,80 M 80,20 L 20,80"
          fill="none" stroke="#f44336" stroke-width="6" stroke-linecap="round"/>
</svg>
```

### Gear Icon

```html
<svg width="100" height="100">
    <path d="M 50,10 L 55,25 L 70,20 L 75,35 L 90,35 L 85,50 L 90,65 L 75,65 L 70,80 L 55,75 L 50,90 L 45,75 L 30,80 L 25,65 L 10,65 L 15,50 L 10,35 L 25,35 L 30,20 L 45,25 Z"
          fill="#607D8B" stroke="#455A64" stroke-width="2"/>
    <circle cx="50" cy="50" r="15" fill="#fff"/>
</svg>
```

### Music Note

```html
<svg width="60" height="100">
    <path d="M 45,10 L 45,70 Q 45,85 35,85 Q 20,85 20,75 Q 20,65 35,65 Q 45,65 45,70 L 45,25 L 50,22 Z"
          fill="#333"/>
</svg>
```

### Location Pin

```html
<svg width="80" height="120">
    <path d="M 40,10 Q 55,10 60,25 Q 65,40 65,50 Q 65,70 40,100 Q 15,70 15,50 Q 15,40 20,25 Q 25,10 40,10 Z"
          fill="#f44336" stroke="#c62828" stroke-width="2"/>
    <circle cx="40" cy="40" r="12" fill="#fff"/>
</svg>
```

### Pentagon with Curves

```html
<svg width="120" height="120">
    <path d="M 60,10 Q 70,40 90,45 Q 80,70 80,90 Q 60,80 60,100 Q 40,80 40,90 Q 20,70 30,45 Q 50,40 60,10 Z"
          fill="#9C27B0" stroke="#6A1B9A" stroke-width="2"/>
</svg>
```

### Leaf

```html
<svg width="100" height="120">
    <path d="M 50,10 Q 70,40 70,80 Q 70,100 50,110 Q 30,100 30,80 Q 30,40 50,10 Z M 50,20 L 50,100"
          fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Crescent Moon

```html
<svg width="100" height="100">
    <path d="M 70,15 Q 85,20 85,50 Q 85,80 70,85 Q 55,80 50,50 Q 55,20 70,15 M 70,25 Q 60,30 60,50 Q 60,70 70,75 Q 75,70 75,50 Q 75,30 70,25"
          fill="#FFC107" fill-rule="evenodd"/>
</svg>
```

### Yin Yang

```html
<svg width="120" height="120">
    <circle cx="60" cy="60" r="50" fill="#fff" stroke="#000" stroke-width="2"/>
    <path d="M 60,10 A 50,50 0 0,1 60,110 A 25,25 0 0,1 60,60 A 25,25 0 0,0 60,10 Z"
          fill="#000"/>
    <circle cx="60" cy="35" r="8" fill="#fff"/>
    <circle cx="60" cy="85" r="8" fill="#000"/>
</svg>
```

### Rounded Rectangle Path

```html
<svg width="150" height="100">
    <path d="M 20,10 L 130,10 Q 140,10 140,20 L 140,80 Q 140,90 130,90 L 20,90 Q 10,90 10,80 L 10,20 Q 10,10 20,10 Z"
          fill="#2196F3" stroke="#1565C0" stroke-width="2"/>
</svg>
```

### Lightning Bolt Path

```html
<svg width="80" height="120">
    <path d="M 45,10 L 30,50 L 50,50 L 35,110 L 70,60 L 50,60 L 65,10 Z"
          fill="#FFC107" stroke="#F57F17" stroke-width="2"/>
</svg>
```

### Ribbon

```html
<svg width="200" height="80">
    <path d="M 10,40 Q 50,10 100,40 Q 150,70 190,40 L 190,50 Q 150,80 100,50 Q 50,20 10,50 Z"
          fill="#E91E63" stroke="#AD1457" stroke-width="2"/>
</svg>
```

### Dynamic Arc

```html
<!-- Model: { radius: 50, sweep: 1 } -->
<svg width="120" height="120">
    <path d="M 60,10 A {{radius}},{{radius}} 0 0,{{sweep}} 110,60"
          fill="none" stroke="#4CAF50" stroke-width="3"/>
</svg>
```

---

## See Also

- [polyline](/reference/svgelements/polyline.html) - SVG polyline element for straight segments
- [polygon](/reference/svgelements/polygon.html) - SVG polygon element for closed straight shapes
- [rect](/reference/svgelements/rect.html) - SVG rectangle element
- [circle](/reference/svgelements/circle.html) - SVG circle element
- [ellipse](/reference/svgelements/ellipse.html) - SVG ellipse element
- [svg](/reference/svgelements/svg.html) - SVG container element
- [SVG Path Data](https://www.w3.org/TR/SVG/paths.html) - W3C SVG Path specification
- [Data Binding](/reference/binding/) - Data binding and expressions

---
