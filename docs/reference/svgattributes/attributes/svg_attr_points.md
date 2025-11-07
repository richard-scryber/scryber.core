---
layout: default
title: points (coordinate list)
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# points : The SVG Points List Attribute

The `points` attribute defines a list of coordinate pairs for SVG `<polyline>` and `<polygon>` elements, specifying the vertices that form connected line segments. Each point represents an (x,y) coordinate where a line segment begins or ends.

## Usage

The `points` attribute accepts a space or comma-separated list of coordinate pairs:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="100">
    <polyline points="10,50 50,20 90,60 130,30 170,70"
              fill="none" stroke="#2196F3" stroke-width="2"/>
</svg>
```

---

## Supported Values

The `points` attribute value consists of coordinate pairs in one of several flexible formats:

### Format Options

**Comma-separated pairs:**
```html
points="10,20 30,40 50,60 70,80"
```

**Space-separated coordinates:**
```html
points="10 20 30 40 50 60 70 80"
```

**Mixed separators:**
```html
points="10,20, 30,40, 50,60, 70,80"
```

**Minimal spacing:**
```html
points="10,20,30,40,50,60,70,80"
```

### Coordinate Pairs

Each coordinate pair represents:
- **x** - Horizontal position (left to right)
- **y** - Vertical position (top to bottom)

Coordinates can be:
- **Integers:** `50,100`
- **Decimals:** `50.5,100.75`
- **Negative:** `-10,20`
- **Units:** Default is points (pt). Can specify: `10pt,20pt`

### Minimum Requirements

- **Polyline:** At least 2 points required for a visible line
- **Polygon:** At least 3 points required for a closed shape
- Single point or empty list produces no visible output

---

## Supported Elements

| Element | Description | Behavior |
|---------|-------------|----------|
| `<polyline>` | Open path of connected line segments | Lines connect points in order, not automatically closed |
| `<polygon>` | Closed path of connected line segments | Lines connect points in order, automatically closes from last to first point |

---

## Data Binding

The `points` attribute supports dynamic coordinate generation using data binding expressions with `{{expression}}` syntax.

### Example 1: Simple Bound Points

```html
<!-- Model: { pointList: "20,30 60,10 100,40 140,20 180,50" } -->
<svg width="200" height="60">
    <polyline points="{{pointList}}"
              fill="none" stroke="#2196F3" stroke-width="2"/>
</svg>
```

### Example 2: Generated Line Chart

```html
{% raw %}
<!-- Model: {
    data: [30, 45, 35, 60, 50, 70, 55],
    width: 280,
    height: 150,
    padding: 20
} -->
<svg width="300" height="170">
    <polyline points="{{data.map((value, index) => {
                    const x = padding + (index * (width - 2 * padding) / (data.length - 1));
                    const y = height - value;
                    return `${x},${y}`;
                }).join(' ')}}"
              fill="none" stroke="#4CAF50" stroke-width="3"/>
</svg>
{% endraw %}
```

### Example 3: Dynamic Polygon Shape

```html
{% raw %}
<!-- Model: {
    sides: 6,
    radius: 40,
    centerX: 60,
    centerY: 60,
    generatePolygon: function(cx, cy, r, sides) {
        const points = [];
        for (let i = 0; i < sides; i++) {
            const angle = (2 * Math.PI / sides) * i - Math.PI / 2;
            const x = cx + r * Math.cos(angle);
            const y = cy + r * Math.sin(angle);
            points.push(`${x.toFixed(2)},${y.toFixed(2)}`);
        }
        return points.join(' ');
    }
} -->
<svg width="120" height="120">
    <polygon points="{{generatePolygon(centerX, centerY, radius, sides)}}"
             fill="#9C27B0" stroke="#6A1B9A" stroke-width="2"/>
</svg>
{% endraw %}
```

### Example 4: Array-based Scatter Points

```html
{% raw %}
<!-- Model: {
    points: [
        {x: 30, y: 40},
        {x: 70, y: 20},
        {x: 110, y: 60},
        {x: 150, y: 35},
        {x: 190, y: 55}
    ]
} -->
<svg width="220" height="80">
    <polyline points="{{points.map(p => `${p.x},${p.y}`).join(' ')}}"
              fill="none" stroke="#e74c3c" stroke-width="2"/>
</svg>
{% endraw %}
```

### Example 5: Temperature Timeline

```html
{% raw %}
<!-- Model: {
    temperatures: [
        {hour: 0, temp: 55},
        {hour: 6, temp: 52},
        {hour: 12, temp: 68},
        {hour: 18, temp: 72},
        {hour: 24, temp: 58}
    ],
    scale: 2.5,
    baseline: 100
} -->
<svg width="280" height="120">
    <polyline points="{{temperatures.map(t => {
                    const x = 20 + t.hour * 10;
                    const y = baseline - t.temp;
                    return `${x},${y}`;
                }).join(' ')}}"
              fill="none" stroke="#FF5722" stroke-width="3" stroke-linecap="round"/>
</svg>
{% endraw %}
```

### Example 6: Multi-Series Chart

```html
{% raw %}
<!-- Model: {
    series: [
        {
            name: 'Series A',
            data: [30, 50, 40, 70, 60],
            color: '#e74c3c'
        },
        {
            name: 'Series B',
            data: [40, 45, 55, 60, 65],
            color: '#3498db'
        },
        {
            name: 'Series C',
            data: [35, 40, 50, 55, 58],
            color: '#2ecc71'
        }
    ],
    toPoints: function(data) {
        return data.map((v, i) => `${30 + i * 60},${130 - v}`).join(' ');
    }
} -->
<svg width="300" height="150">
    <template data-bind="{{series}}">
        <polyline points="{{toPoints(.data)}}"
                  fill="none" stroke="{{.color}}" stroke-width="2"/>
    </template>
</svg>
{% endraw %}
```

### Example 7: Grid of Points

```html
{% raw %}
<!-- Model: {
    rows: 5,
    cols: 5,
    spacing: 20,
    startX: 20,
    startY: 20,
    generateGrid: function(rows, cols, spacing, sx, sy) {
        const points = [];
        for (let r = 0; r < rows; r++) {
            for (let c = 0; c < cols; c++) {
                points.push(`${sx + c * spacing},${sy + r * spacing}`);
            }
        }
        return points.join(' ');
    }
} -->
<svg width="140" height="140">
    <!-- Draw with circles, but points generated programmatically -->
    <template data-bind="{{Array.from({length: rows * cols}, (_, i) => ({
        x: startX + (i % cols) * spacing,
        y: startY + Math.floor(i / cols) * spacing
    }))}}">
        <circle cx="{{.x}}" cy="{{.y}}" r="3" fill="#2196F3"/>
    </template>
</svg>
{% endraw %}
```

### Example 8: Normalized Data Visualization

```html
{% raw %}
<!-- Model: {
    values: [23, 87, 45, 92, 34, 78, 56, 89],
    normalize: function(values, targetHeight) {
        const max = Math.max(...values);
        const min = Math.min(...values);
        const range = max - min;
        return values.map((v, i) => {
            const normalized = ((v - min) / range) * targetHeight;
            const x = 20 + i * 35;
            const y = 130 - normalized;
            return `${x},${y}`;
        }).join(' ');
    }
} -->
<svg width="300" height="150">
    <polyline points="{{normalize(values, 100)}}"
              fill="none" stroke="#9C27B0" stroke-width="2"/>
</svg>
{% endraw %}
```

---

## Notes

### Point Order

Points are connected in the order they appear in the list:
- First point → Second point → Third point → etc.
- For `<polygon>`, an automatic line connects the last point back to the first
- For `<polyline>`, the path remains open unless you manually repeat the first point

### Coordinate System

SVG uses a coordinate system where:
- (0,0) is at the **top-left** corner
- X increases **left to right**
- Y increases **top to bottom**
- Units default to points (1pt = 1/72 inch)

### Duplicate Points

Consecutive duplicate points are valid but create zero-length segments:
- `points="10,10 10,10 20,20"` - valid, but first segment has no length
- May affect stroke rendering at that point

### Number Format

Coordinate values support:
- Integers: `50,100`
- Decimals: `50.5,100.75`
- Scientific notation: `1e2,5e1` (100,50)
- Negative values: `-10,20`

### Separator Flexibility

The parser accepts flexible separators:
- Space-separated: `10 20 30 40`
- Comma-separated: `10,20 30,40`
- Mixed: `10,20, 30,40`
- Tight packing: `10,20,30,40`

### Performance Considerations

- Large point lists (thousands of points) may impact:
  - PDF file size
  - Rendering performance
  - Document load time
- Consider simplifying complex paths or using multiple polylines
- Use path simplification algorithms for large datasets

### Fill Behavior

When fill is applied:
- **Polyline:** An implicit line closes the shape for fill purposes only
- **Polygon:** Shape is automatically closed
- The stroke still follows only the explicit points

### Data Resolution

For charts and graphs:
- Match point count to data resolution
- Too many points may create visual noise
- Too few points may lose important details
- Consider data smoothing or sampling for clarity

---

## Examples

### Example 1: Simple Triangle

```html
<svg width="100" height="100">
    <polygon points="50,10 90,80 10,80"
             fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Example 2: Open Zigzag Line

```html
<svg width="200" height="100">
    <polyline points="10,50 30,20 50,50 70,20 90,50 110,20 130,50 150,20 170,50 190,20"
              fill="none" stroke="#FF9800" stroke-width="3"/>
</svg>
```

### Example 3: Pentagon

```html
<svg width="120" height="120">
    <polygon points="60,15 110,50 90,100 30,100 10,50"
             fill="#2196F3" stroke="#1565C0" stroke-width="2"/>
</svg>
```

### Example 4: Line Chart

```html
<svg width="300" height="200">
    <!-- Axes -->
    <line x1="40" y1="20" x2="40" y2="180" stroke="#333" stroke-width="2"/>
    <line x1="40" y1="180" x2="280" y2="180" stroke="#333" stroke-width="2"/>

    <!-- Data line -->
    <polyline points="60,150 100,120 140,140 180,90 220,110 260,70"
              fill="none" stroke="#2196F3" stroke-width="3"/>
</svg>
```

### Example 5: Star Polygon

```html
<svg width="100" height="100">
    <polygon points="50,10 61,40 93,40 68,58 79,88 50,68 21,88 32,58 7,40 39,40"
             fill="#FFC107" stroke="#F57F17" stroke-width="2"/>
</svg>
```

### Example 6: Mountain Silhouette

```html
<svg width="300" height="150">
    <polyline points="0,150 30,120 60,80 100,110 140,50 180,90 220,70 260,100 300,150"
              fill="#4A90E2" stroke="none"/>
</svg>
```

### Example 7: Arrow Pointer

```html
<svg width="150" height="100">
    <polygon points="10,50 80,50 80,30 140,55 80,80 80,60 10,60"
             fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Example 8: Hexagon

```html
<svg width="120" height="120">
    <polygon points="60,10 100,35 100,75 60,100 20,75 20,35"
             fill="#9C27B0" stroke="#6A1B9A" stroke-width="2"/>
</svg>
```

### Example 9: Lightning Bolt

```html
<svg width="100" height="150">
    <polygon points="50,10 30,60 55,65 40,110 70,70 50,65 65,30"
             fill="#FFC107" stroke="#F57F17" stroke-width="2"/>
</svg>
```

### Example 10: Staircase Line

```html
<svg width="200" height="200">
    <polyline points="20,180 20,160 40,160 40,140 60,140 60,120 80,120 80,100 100,100 100,80 120,80 120,60 140,60 140,40 160,40 160,20"
              fill="none" stroke="#4CAF50" stroke-width="3"/>
</svg>
```

### Example 11: Heart Rate Monitor

```html
<svg width="300" height="100">
    <polyline points="10,50 40,50 50,30 60,70 70,50 100,50 110,30 120,70 130,50 160,50 170,30 180,70 190,50 220,50 230,30 240,70 250,50 280,50"
              fill="none" stroke="#4CAF50" stroke-width="2"/>
</svg>
```

### Example 12: Diamond Shape

```html
<svg width="100" height="100">
    <polygon points="50,10 90,50 50,90 10,50"
             fill="#E91E63" stroke="#AD1457" stroke-width="2"/>
</svg>
```

### Example 13: Envelope Icon

```html
<svg width="120" height="80">
    <polygon points="10,20 60,50 110,20"
             fill="none" stroke="#2196F3" stroke-width="2"/>
    <polygon points="10,20 10,70 110,70 110,20 60,50"
             fill="none" stroke="#2196F3" stroke-width="2"/>
</svg>
```

### Example 14: Play Button Triangle

```html
<svg width="80" height="80">
    <polygon points="20,15 20,65 65,40"
             fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Example 15: Bar Chart Outline

```html
<svg width="280" height="180">
    <polyline points="30,160 30,100 70,100 70,160"
              fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
    <polyline points="90,160 90,70 130,70 130,160"
              fill="#2196F3" stroke="#1565C0" stroke-width="2"/>
    <polyline points="150,160 150,90 190,90 190,160"
              fill="#FF9800" stroke="#E65100" stroke-width="2"/>
    <polyline points="210,160 210,50 250,50 250,160"
              fill="#f44336" stroke="#c62828" stroke-width="2"/>
</svg>
```

### Example 16: Network Connection Path

```html
<svg width="250" height="150">
    <polyline points="30,30 80,50 130,40 180,70 230,60"
              fill="none" stroke="#2196F3" stroke-width="2" stroke-dasharray="5,5"/>

    <circle cx="30" cy="30" r="8" fill="#4CAF50"/>
    <circle cx="80" cy="50" r="8" fill="#2196F3"/>
    <circle cx="130" cy="40" r="8" fill="#2196F3"/>
    <circle cx="180" cy="70" r="8" fill="#2196F3"/>
    <circle cx="230" cy="60" r="8" fill="#f44336"/>
</svg>
```

### Example 17: Roof Outline

```html
<svg width="200" height="150">
    <rect x="50" y="90" width="100" height="60" fill="#FFF8E1" stroke="#333" stroke-width="2"/>
    <polyline points="40,90 100,40 160,90"
              fill="#8D6E63" stroke="#5D4037" stroke-width="2"/>
</svg>
```

### Example 18: Area Chart (Filled Polyline)

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

### Example 19: Traffic Sign (Octagon)

```html
<svg width="120" height="120">
    <polygon points="40,10 80,10 110,40 110,80 80,110 40,110 10,80 10,40"
             fill="#f44336" stroke="#c62828" stroke-width="3"/>
    <text x="60" y="70" font-size="24" fill="#fff" text-anchor="middle">STOP</text>
</svg>
```

### Example 20: Sparkline Chart

```html
{% raw %}
<!-- Model: { sparkData: [12, 18, 15, 22, 19, 25, 23, 28, 24, 30] } -->
<svg width="150" height="40">
    <polyline points="{{sparkData.map((v, i) => `${5 + i * 15},${35 - v}`).join(' ')}}"
              fill="none" stroke="#4CAF50" stroke-width="2"/>
</svg>
{% endraw %}
```

---

## See Also

- [polyline element](/reference/svgelements/polyline.html) - SVG polyline element for open paths
- [polygon element](/reference/svgelements/polygon.html) - SVG polygon element for closed shapes
- [path element](/reference/svgelements/path.html) - SVG path element for curves and complex shapes
- [d attribute](/reference/svgattributes/d.html) - Path data for complex curves
- [line element](/reference/svgelements/line.html) - SVG line element for single segments
- [stroke attributes](/reference/svgattributes/stroke.html) - Stroke styling attributes
- [fill attributes](/reference/svgattributes/fill.html) - Fill styling attributes
- [Data Binding](/reference/binding/) - Data binding and expressions guide

---
