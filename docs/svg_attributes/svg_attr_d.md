---
layout: default
title: d (path data)
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# d : The SVG Path Data Attribute

The `d` attribute defines path data for SVG `<path>` elements, specifying a sequence of drawing commands that create complex shapes, curves, lines, and arcs. It uses a mini-language of single-letter commands followed by coordinate parameters.

## Usage

The `d` attribute accepts a string of path commands that define how to draw a shape:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="100" height="100">
    <path d="M 10,10 L 90,90 L 10,90 Z"
          fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

---

## Supported Values

The path data string consists of commands (single letters) followed by numeric parameters. Commands are case-sensitive: **uppercase = absolute coordinates**, **lowercase = relative coordinates**.

### Move Commands

| Command | Parameters | Description | Example |
|---------|------------|-------------|---------|
| `M x,y` | Absolute position | Move pen to absolute position (x,y) without drawing | `M 50,50` |
| `m dx,dy` | Relative offset | Move pen by offset (dx,dy) from current position | `m 10,20` |

### Line Drawing Commands

| Command | Parameters | Description | Example |
|---------|------------|-------------|---------|
| `L x,y` | Absolute position | Draw straight line to absolute position (x,y) | `L 100,100` |
| `l dx,dy` | Relative offset | Draw line by relative offset (dx,dy) | `l 50,50` |
| `H x` | Absolute x-coordinate | Draw horizontal line to absolute x | `H 150` |
| `h dx` | Relative x-offset | Draw horizontal line by relative dx | `h 25` |
| `V y` | Absolute y-coordinate | Draw vertical line to absolute y | `V 200` |
| `v dy` | Relative y-offset | Draw vertical line by relative dy | `v -30` |

### Cubic Bezier Curve Commands

| Command | Parameters | Description | Example |
|---------|------------|-------------|---------|
| `C x1,y1 x2,y2 x,y` | Two control points + end point | Cubic Bezier curve with full control | `C 20,20 40,20 50,10` |
| `c dx1,dy1 dx2,dy2 dx,dy` | Relative coordinates | Relative cubic Bezier curve | `c 10,10 20,10 30,0` |
| `S x2,y2 x,y` | End control point + end point | Smooth cubic curve (mirrors previous control) | `S 80,20 100,50` |
| `s dx2,dy2 dx,dy` | Relative coordinates | Relative smooth cubic curve | `s 20,10 40,0` |

### Quadratic Bezier Curve Commands

| Command | Parameters | Description | Example |
|---------|------------|-------------|---------|
| `Q x1,y1 x,y` | Control point + end point | Quadratic Bezier curve | `Q 50,5 100,50` |
| `q dx1,dy1 dx,dy` | Relative coordinates | Relative quadratic curve | `q 25,5 50,0` |
| `T x,y` | End point only | Smooth quadratic (mirrors previous control) | `T 150,50` |
| `t dx,dy` | Relative coordinates | Relative smooth quadratic | `t 50,0` |

### Arc Commands

| Command | Parameters | Description | Example |
|---------|------------|-------------|---------|
| `A rx,ry angle large-arc sweep x,y` | Ellipse radii, rotation, flags, end point | Elliptical arc segment | `A 30,30 0 0,1 80,80` |
| `a rx,ry angle large-arc sweep dx,dy` | Relative coordinates | Relative elliptical arc | `a 20,20 0 1,0 40,40` |

**Arc Parameters:**
- `rx, ry` - X and Y radii of the ellipse
- `angle` - Rotation angle of the ellipse in degrees
- `large-arc` - 0 for small arc (< 180°), 1 for large arc (≥ 180°)
- `sweep` - 0 for counter-clockwise, 1 for clockwise
- `x,y` - End point coordinates

### Close Path Command

| Command | Parameters | Description | Example |
|---------|------------|-------------|---------|
| `Z` or `z` | None | Close path by drawing line to start point | `Z` |

### Command Chaining

Multiple commands can be chained in a single `d` attribute value:

```html
d="M 10,30 L 90,30 L 90,70 L 10,70 Z"
```

Parameters can be separated by spaces or commas:

```html
d="M10,30 L90 30 L90 70 L10 70Z"  <!-- Valid -->
d="M 10 30 L 90 30 L 90 70 L 10 70 Z"  <!-- Valid -->
```

---

## Supported Elements

| Element | Description |
|---------|-------------|
| `<path>` | SVG path element - the primary element using the `d` attribute |

---

## Data Binding

The `d` attribute supports dynamic path generation using data binding expressions with `{{expression}}` syntax.

### Example 1: Simple Dynamic Path

```html
<!-- Model: { pathData: 'M 10,50 L 90,50 L 50,10 Z' } -->
<svg width="100" height="60">
    <path d="{{pathData}}"
          fill="#2196F3" stroke="#1565C0" stroke-width="2"/>
</svg>
```

### Example 2: Calculated Bezier Curve

```html
<!-- Model: { x1: 20, y1: 80, x2: 180, y2: 80, curveHeight: 40 } -->
<svg width="200" height="100">
    <path d="M {{x1}},{{y1}} Q {{(x1 + x2) / 2}},{{y1 - curveHeight}} {{x2}},{{y2}}"
          fill="none" stroke="#4CAF50" stroke-width="3"/>
</svg>
```

### Example 3: Programmatic Path Generation

```html
<!-- Model: {
    radius: 40,
    centerX: 50,
    centerY: 50,
    generateCircle: function(cx, cy, r) {
        return `M ${cx-r},${cy} A ${r},${r} 0 1,0 ${cx+r},${cy} A ${r},${r} 0 1,0 ${cx-r},${cy}`;
    }
} -->
<svg width="100" height="100">
    <path d="{{generateCircle(centerX, centerY, radius)}}"
          fill="#FF9800" stroke="#E65100" stroke-width="2"/>
</svg>
```

### Example 4: Dynamic Chart Path

```html
<!-- Model: {
    data: [20, 45, 35, 60, 50, 75, 65],
    width: 280,
    height: 150,
    padding: 20
} -->
<svg width="300" height="170">
    <path d="M {{padding}},{{height - data[0]}} {{data.map((v, i) => {
                const x = padding + (i * (width - 2 * padding) / (data.length - 1));
                const y = height - v;
                return `L ${x},${y}`;
            }).join(' ')}}"
          fill="none" stroke="#2196F3" stroke-width="3"/>
</svg>
```

### Example 5: Dynamic Star Generator

```html
<!-- Model: {
    points: 5,
    outerRadius: 40,
    innerRadius: 15,
    centerX: 50,
    centerY: 50,
    generateStar: function(cx, cy, points, outer, inner) {
        let path = '';
        for (let i = 0; i < points * 2; i++) {
            const angle = (Math.PI / points) * i - Math.PI / 2;
            const r = i % 2 === 0 ? outer : inner;
            const x = cx + r * Math.cos(angle);
            const y = cy + r * Math.sin(angle);
            path += (i === 0 ? 'M' : 'L') + ` ${x.toFixed(2)},${y.toFixed(2)} `;
        }
        return path + 'Z';
    }
} -->
<svg width="100" height="100">
    <path d="{{generateStar(centerX, centerY, points, outerRadius, innerRadius)}}"
          fill="#FFC107" stroke="#F57F17" stroke-width="2"/>
</svg>
```

### Example 6: Multiple Dynamic Paths

```html
<!-- Model: {
    paths: [
        {d: 'M 10,40 C 40,10 60,10 90,40', color: '#e74c3c', width: 2},
        {d: 'M 10,60 C 40,90 60,90 90,60', color: '#3498db', width: 3}
    ]
} -->
<svg width="100" height="100">
    <template data-bind="{{paths}}">
        <path d="{{.d}}"
              fill="none" stroke="{{.color}}" stroke-width="{{.width}}"/>
    </template>
</svg>
```

### Example 7: Animated Progress Arc

```html
<!-- Model: {
    progress: 0.75,  // 75%
    radius: 45,
    thickness: 10,
    generateArc: function(progress, r) {
        const angle = progress * 2 * Math.PI;
        const largeArc = progress > 0.5 ? 1 : 0;
        const x = 50 + r * Math.sin(angle);
        const y = 50 - r * Math.cos(angle);
        return `M 50,${50-r} A ${r},${r} 0 ${largeArc},1 ${x},${y}`;
    }
} -->
<svg width="100" height="100">
    <!-- Background circle -->
    <circle cx="50" cy="50" r="45" fill="none" stroke="#e0e0e0" stroke-width="10"/>

    <!-- Progress arc -->
    <path d="{{generateArc(progress, radius)}}"
          fill="none" stroke="#4CAF50" stroke-width="10" stroke-linecap="round"/>
</svg>
```

---

## Notes

### Command Case Sensitivity

Commands are strictly case-sensitive:
- **Uppercase (M, L, C, Q, A, etc.)** - Absolute coordinates relative to SVG origin (0,0)
- **Lowercase (m, l, c, q, a, etc.)** - Relative coordinates from current pen position

### Number Format

Coordinates can be integers or decimals:
- `M 10,20` - integers
- `M 10.5,20.75` - decimals
- `M -10,20` - negative values

### Whitespace and Separators

The parser is flexible with separators:
- Spaces: `M 10 20 L 30 40`
- Commas: `M 10,20 L 30,40`
- Mixed: `M10,20L30,40` (minimal spacing)
- Multiple spaces are ignored

### Path Complexity

Paths can be arbitrarily complex with any combination of commands. However:
- Very complex paths may impact PDF file size
- Consider using simpler shape elements (rect, circle) when possible
- Break extremely complex shapes into multiple paths for maintainability

### Coordinate Precision

While the parser accepts high precision decimals, PDF rendering typically uses:
- Point precision: 1/72 inch
- Values are rounded to reasonable precision
- Excessive decimal places don't improve visual quality

### Fill Rules

For self-intersecting paths, the `fill-rule` attribute determines which areas are filled:
- `nonzero` (default) - Uses winding number algorithm
- `evenodd` - Uses even-odd rule (alternating fill)

### Path Direction

The direction (clockwise vs counter-clockwise) affects:
- Fill rules for complex paths
- Marker orientation
- Text-on-path rendering

---

## Examples

### Example 1: Simple Triangle

```html
<svg width="100" height="100">
    <path d="M 50,10 L 90,80 L 10,80 Z"
          fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Example 2: Smooth Wave Using Curves

```html
<svg width="300" height="100">
    <path d="M 0,50 Q 25,20 50,50 T 100,50 T 150,50 T 200,50 T 250,50 T 300,50"
          fill="none" stroke="#00BCD4" stroke-width="3"/>
</svg>
```

### Example 3: Heart Shape

```html
<svg width="100" height="100">
    <path d="M 50,30 C 50,25 45,20 40,20 C 30,20 25,30 25,35 C 25,45 50,65 50,65 C 50,65 75,45 75,35 C 75,30 70,20 60,20 C 55,20 50,25 50,30 Z"
          fill="#f44336" stroke="#c62828" stroke-width="2"/>
</svg>
```

### Example 4: Cloud Icon

```html
<svg width="120" height="80">
    <path d="M 30,60 Q 10,60 10,45 Q 10,30 25,30 Q 25,15 40,15 Q 55,15 55,25 Q 70,20 80,30 Q 95,30 95,45 Q 95,60 80,60 Z"
          fill="#90CAF9" stroke="#42A5F5" stroke-width="2"/>
</svg>
```

### Example 5: Checkmark Icon

```html
<svg width="100" height="100">
    <path d="M 20,50 L 40,70 L 80,25"
          fill="none" stroke="#4CAF50" stroke-width="6"
          stroke-linecap="round" stroke-linejoin="round"/>
</svg>
```

### Example 6: Complex Star Shape

```html
<svg width="100" height="100">
    <path d="M 50,10 L 61,40 L 93,40 L 68,58 L 79,88 L 50,68 L 21,88 L 32,58 L 7,40 L 39,40 Z"
          fill="#FFC107" stroke="#F57F17" stroke-width="2"/>
</svg>
```

### Example 7: Speech Bubble

```html
<svg width="150" height="100">
    <path d="M 20,20 L 130,20 Q 140,20 140,30 L 140,70 Q 140,80 130,80 L 50,80 L 30,95 L 35,80 L 20,80 Q 10,80 10,70 L 10,30 Q 10,20 20,20 Z"
          fill="#fff" stroke="#333" stroke-width="2"/>
</svg>
```

### Example 8: Arrow with Bezier Curve

```html
<svg width="150" height="100">
    <path d="M 10,50 Q 75,10 140,50 L 130,55 L 140,50 L 130,45"
          fill="none" stroke="#4CAF50" stroke-width="3" stroke-linejoin="round"/>
</svg>
```

### Example 9: Infinity Symbol

```html
<svg width="200" height="100">
    <path d="M 50,50 C 30,20 0,30 0,50 C 0,70 30,80 50,50 C 70,20 130,20 150,50 C 170,80 200,70 200,50 C 200,30 170,20 150,50"
          fill="none" stroke="#673AB7" stroke-width="4"/>
</svg>
```

### Example 10: Location Pin

```html
<svg width="80" height="120">
    <path d="M 40,10 Q 55,10 60,25 Q 65,40 65,50 Q 65,70 40,100 Q 15,70 15,50 Q 15,40 20,25 Q 25,10 40,10 Z"
          fill="#f44336" stroke="#c62828" stroke-width="2"/>
    <circle cx="40" cy="40" r="12" fill="#fff"/>
</svg>
```

### Example 11: Gear Icon

```html
<svg width="100" height="100">
    <path d="M 50,10 L 55,25 L 70,20 L 75,35 L 90,35 L 85,50 L 90,65 L 75,65 L 70,80 L 55,75 L 50,90 L 45,75 L 30,80 L 25,65 L 10,65 L 15,50 L 10,35 L 25,35 L 30,20 L 45,25 Z"
          fill="#607D8B" stroke="#455A64" stroke-width="2"/>
    <circle cx="50" cy="50" r="15" fill="#fff"/>
</svg>
```

### Example 12: Lightning Bolt

```html
<svg width="80" height="120">
    <path d="M 45,10 L 30,50 L 50,50 L 35,110 L 70,60 L 50,60 L 65,10 Z"
          fill="#FFC107" stroke="#F57F17" stroke-width="2"/>
</svg>
```

### Example 13: Music Note

```html
<svg width="60" height="100">
    <path d="M 45,10 L 45,70 Q 45,85 35,85 Q 20,85 20,75 Q 20,65 35,65 Q 45,65 45,70 L 45,25 L 50,22 Z"
          fill="#333"/>
</svg>
```

### Example 14: Leaf Shape

```html
<svg width="100" height="120">
    <path d="M 50,10 Q 70,40 70,80 Q 70,100 50,110 Q 30,100 30,80 Q 30,40 50,10 Z M 50,20 L 50,100"
          fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Example 15: Pie Chart Slice

```html
<svg width="120" height="120">
    <path d="M 60,60 L 60,10 A 50,50 0 0,1 110,60 Z"
          fill="#2196F3" stroke="#1565C0" stroke-width="2"/>
</svg>
```

### Example 16: Hexagon Using Lines

```html
<svg width="120" height="120">
    <path d="M 60,10 L 100,35 L 100,75 L 60,100 L 20,75 L 20,35 Z"
          fill="#9C27B0" stroke="#6A1B9A" stroke-width="2"/>
</svg>
```

### Example 17: Rounded Rectangle Path

```html
<svg width="150" height="100">
    <path d="M 20,10 L 130,10 Q 140,10 140,20 L 140,80 Q 140,90 130,90 L 20,90 Q 10,90 10,80 L 10,20 Q 10,10 20,10 Z"
          fill="#2196F3" stroke="#1565C0" stroke-width="2"/>
</svg>
```

### Example 18: House Icon

```html
<svg width="120" height="120">
    <path d="M 20,60 L 60,20 L 100,60 L 100,110 L 20,110 Z M 45,80 L 45,110 M 75,80 L 75,110 M 45,80 L 75,80"
          fill="#FFF8E1" stroke="#333" stroke-width="2"/>
</svg>
```

### Example 19: Crescent Moon

```html
<svg width="100" height="100">
    <path d="M 70,15 Q 85,20 85,50 Q 85,80 70,85 Q 55,80 50,50 Q 55,20 70,15 M 70,25 Q 60,30 60,50 Q 60,70 70,75 Q 75,70 75,50 Q 75,30 70,25"
          fill="#FFC107" fill-rule="evenodd"/>
</svg>
```

### Example 20: Bar Chart with Paths

```html
<svg width="300" height="200">
    <!-- Bar 1 -->
    <path d="M 30,180 L 30,120 L 70,120 L 70,180 Z"
          fill="#4CAF50"/>

    <!-- Bar 2 -->
    <path d="M 90,180 L 90,80 L 130,80 L 130,180 Z"
          fill="#2196F3"/>

    <!-- Bar 3 -->
    <path d="M 150,180 L 150,100 L 190,100 L 190,180 Z"
          fill="#FF9800"/>

    <!-- Bar 4 -->
    <path d="M 210,180 L 210,60 L 250,60 L 250,180 Z"
          fill="#f44336"/>
</svg>
```

---

## See Also

- [path element](/reference/svgelements/path.html) - SVG path element
- [polyline element](/reference/svgelements/polyline.html) - SVG polyline for straight line segments
- [polygon element](/reference/svgelements/polygon.html) - SVG polygon for closed shapes
- [points attribute](/reference/svgattributes/points.html) - Points list for polyline and polygon
- [stroke attributes](/reference/svgattributes/stroke.html) - Stroke styling attributes
- [fill attributes](/reference/svgattributes/fill.html) - Fill styling attributes
- [SVG Path Specification](https://www.w3.org/TR/SVG/paths.html) - W3C SVG path data specification
- [Data Binding](/reference/binding/) - Data binding and expressions guide

---
