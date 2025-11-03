---
layout: default
title: stroke-width
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# stroke-width : SVG Stroke Width Property

The `stroke-width` property controls the thickness of SVG shape strokes in PDF documents. This property is fundamental for defining line weights, border thickness, and the visual prominence of vector graphics outlines.

## Usage

```css
selector {
    stroke-width: value;
}
```

The stroke-width property accepts numeric values with optional units to specify the thickness of strokes.

---

## Supported Values

### Numeric Values
- Unitless numbers (e.g., `2`, `5`, `10`) - Interpreted as user units
- Point values (e.g., `2pt`, `5pt`) - PDF points
- Pixel values (e.g., `2px`, `5px`) - Pixels
- Percent values (e.g., `5%`) - Relative to viewport

### Special Keywords
- `0` - No stroke (same as `stroke: none`)

---

## Supported Elements

The `stroke-width` property applies to SVG elements including:
- `<rect>` rectangles
- `<circle>` circles
- `<ellipse>` ellipses
- `<polygon>` polygons
- `<polyline>` polylines
- `<line>` lines
- `<path>` paths
- `<text>` SVG text elements
- All other SVG shape elements

---

## Notes

- Default stroke-width value is typically `1` (1 user unit)
- Strokes are drawn centered on the path, with half the width on each side
- Very thin strokes (< 0.5) may not render well on some PDF viewers
- Stroke width is affected by transformations and scaling
- Use consistent stroke widths for professional-looking diagrams
- Width of 0 effectively removes the stroke
- Unlike HTML borders, SVG strokes don't affect element layout or dimensions
- Vector strokes maintain quality at any zoom level in PDF viewers
- Larger stroke widths can obscure fine details in complex paths
- For hairline rules, use width of 1 or less

---

## Data Binding

The `stroke-width` property can be dynamically set using data binding expressions, enabling SVG stroke thickness to reflect data magnitudes, importance levels, or hierarchical relationships.

### Example 1: Data-driven line weights in charts

```html
<style>
    .data-line {
        fill: none;
        stroke: #3b82f6;
    }
</style>
<body>
    <svg width="300" height="200">
        {{#each trendLines}}
        <polyline class="data-line"
                  style="stroke-width: {{importance}}"
                  points="{{dataPoints}}"/>
        {{/each}}
    </svg>
</body>
```

With model data:
```json
{
    "trendLines": [
        { "name": "Primary", "importance": 4, "dataPoints": "20,150 80,80 140,120 200,60 260,90" },
        { "name": "Secondary", "importance": 2, "dataPoints": "20,120 80,100 140,140 200,80 260,110" },
        { "name": "Reference", "importance": 1, "dataPoints": "20,100 260,100" }
    ]
}
```

### Example 2: Conditional border thickness based on status

```html
<style>
    .item-box {
        fill: white;
        stroke: #3b82f6;
    }
</style>
<body>
    <svg width="350" height="120">
        {{#each items}}
        <rect class="item-box"
              style="stroke-width: {{isHighlight ? 5 : isNormal ? 2 : 1}}"
              x="{{position}}"
              y="20"
              width="100"
              height="80"
              rx="5"/>
        {{/each}}
    </svg>
</body>
```

### Example 3: Network connections weighted by traffic

```html
<style>
    .node {
        fill: white;
        stroke: #3b82f6;
        stroke-width: 2;
    }
    .connection {
        stroke: #60a5fa;
    }
</style>
<body>
    <svg width="400" height="200">
        {{#each connections}}
        <line class="connection"
              style="stroke-width: {{bandwidth / 10}}"
              x1="{{x1}}"
              y1="{{y1}}"
              x2="{{x2}}"
              y2="{{y2}}"/>
        {{/each}}

        {{#each nodes}}
        <circle class="node" cx="{{x}}" cy="{{y}}" r="20"/>
        {{/each}}
    </svg>
</body>
```

With model data (bandwidth determines line thickness):
```json
{
    "connections": [
        { "from": 0, "to": 1, "bandwidth": 80, "x1": 75, "y1": 100, "x2": 175, "y2": 100 },
        { "from": 1, "to": 2, "bandwidth": 40, "x1": 175, "y1": 100, "x2": 275, "y2": 100 },
        { "from": 0, "to": 2, "bandwidth": 10, "x1": 75, "y1": 100, "x2": 275, "y2": 100 }
    ],
    "nodes": [
        { "id": 0, "x": 75, "y": 100 },
        { "id": 1, "x": 175, "y": 100 },
        { "id": 2, "x": 275, "y": 100 }
    ]
}
```

---

## Examples

### Example 1: Basic stroke widths

```html
<style>
    .line { stroke: black; }
    .thin { stroke-width: 1; }
    .medium { stroke-width: 3; }
    .thick { stroke-width: 6; }
</style>
<body>
    <svg width="300" height="150">
        <line class="line thin" x1="20" y1="30" x2="280" y2="30"/>
        <line class="line medium" x1="20" y1="70" x2="280" y2="70"/>
        <line class="line thick" x1="20" y1="110" x2="280" y2="110"/>
    </svg>
</body>
```

### Example 2: Circles with varying stroke widths

```html
<style>
    .circle {
        fill: none;
        stroke: #3b82f6;
    }
    .width-1 { stroke-width: 1; }
    .width-3 { stroke-width: 3; }
    .width-5 { stroke-width: 5; }
    .width-8 { stroke-width: 8; }
</style>
<body>
    <svg width="400" height="120">
        <circle class="circle width-1" cx="50" cy="60" r="40"/>
        <circle class="circle width-3" cx="150" cy="60" r="40"/>
        <circle class="circle width-5" cx="250" cy="60" r="40"/>
        <circle class="circle width-8" cx="350" cy="60" r="40"/>
    </svg>
</body>
```

### Example 3: Rectangle borders

```html
<style>
    .box {
        fill: lightblue;
        stroke: darkblue;
    }
    .border-thin { stroke-width: 1; }
    .border-medium { stroke-width: 3; }
    .border-thick { stroke-width: 6; }
</style>
<body>
    <svg width="350" height="100">
        <rect class="box border-thin" x="10" y="10" width="100" height="80"/>
        <rect class="box border-medium" x="125" y="10" width="100" height="80"/>
        <rect class="box border-thick" x="240" y="10" width="100" height="80"/>
    </svg>
</body>
```

### Example 4: Point unit stroke widths

```html
<style>
    .line { stroke: #374151; }
    .hairline { stroke-width: 0.5pt; }
    .standard { stroke-width: 1pt; }
    .bold { stroke-width: 2pt; }
</style>
<body>
    <svg width="300" height="120">
        <line class="line hairline" x1="20" y1="30" x2="280" y2="30"/>
        <line class="line standard" x1="20" y1="60" x2="280" y2="60"/>
        <line class="line bold" x1="20" y1="90" x2="280" y2="90"/>
    </svg>
</body>
```

### Example 5: Chart with weighted lines

```html
<style>
    .line-major {
        stroke: #1e40af;
        stroke-width: 4;
    }
    .line-minor {
        stroke: #60a5fa;
        stroke-width: 2;
    }
    .line-reference {
        stroke: #9ca3af;
        stroke-width: 1;
        stroke-dasharray: 5,5;
    }
</style>
<body>
    <svg width="300" height="200">
        <line class="line-reference" x1="0" y1="100" x2="300" y2="100"/>
        <polyline class="line-minor" points="20,150 80,120 140,140 200,80 260,100" fill="none"/>
        <polyline class="line-major" points="20,180 80,140 140,160 200,60 260,80" fill="none"/>
    </svg>
</body>
```

### Example 6: Icon with detailed strokes

```html
<style>
    .icon-outline {
        fill: none;
        stroke: #374151;
        stroke-width: 2;
    }
    .icon-detail {
        fill: none;
        stroke: #374151;
        stroke-width: 1;
    }
</style>
<body>
    <svg width="100" height="100" viewBox="0 0 24 24">
        <circle class="icon-outline" cx="12" cy="12" r="10"/>
        <path class="icon-detail" d="M12 6v6l4 2"/>
    </svg>
</body>
```

### Example 7: Wireframe with hierarchy

```html
<style>
    .frame-primary {
        fill: none;
        stroke: black;
        stroke-width: 4;
    }
    .frame-secondary {
        fill: none;
        stroke: black;
        stroke-width: 2;
    }
    .frame-detail {
        fill: none;
        stroke: gray;
        stroke-width: 1;
    }
</style>
<body>
    <svg width="300" height="250">
        <rect class="frame-primary" x="20" y="20" width="260" height="210"/>
        <rect class="frame-secondary" x="40" y="40" width="220" height="80"/>
        <line class="frame-detail" x1="40" y1="140" x2="260" y2="140"/>
        <line class="frame-detail" x1="150" y1="140" x2="150" y2="210"/>
    </svg>
</body>
```

### Example 8: Network diagram connections

```html
<style>
    .node {
        fill: white;
        stroke: #3b82f6;
        stroke-width: 2;
    }
    .connection-strong {
        stroke: #3b82f6;
        stroke-width: 4;
    }
    .connection-medium {
        stroke: #60a5fa;
        stroke-width: 2;
    }
    .connection-weak {
        stroke: #93c5fd;
        stroke-width: 1;
    }
</style>
<body>
    <svg width="300" height="150">
        <line class="connection-strong" x1="75" y1="75" x2="150" y2="75"/>
        <line class="connection-medium" x1="150" y1="75" x2="225" y2="75"/>
        <line class="connection-weak" x1="75" y1="75" x2="225" y2="75"/>
        <circle class="node" cx="75" cy="75" r="20"/>
        <circle class="node" cx="150" cy="75" r="20"/>
        <circle class="node" cx="225" cy="75" r="20"/>
    </svg>
</body>
```

### Example 9: Progress bar with thick stroke

```html
<style>
    .progress-track {
        fill: none;
        stroke: #e5e7eb;
        stroke-width: 20;
    }
    .progress-fill {
        fill: none;
        stroke: #10b981;
        stroke-width: 20;
    }
</style>
<body>
    <svg width="300" height="50">
        <line class="progress-track" x1="10" y1="25" x2="290" y2="25" stroke-linecap="round"/>
        <line class="progress-fill" x1="10" y1="25" x2="210" y2="25" stroke-linecap="round"/>
    </svg>
    <p>70% Complete</p>
</body>
```

### Example 10: Technical drawing line weights

```html
<style>
    .drawing-visible {
        stroke: black;
        stroke-width: 2;
        fill: none;
    }
    .drawing-hidden {
        stroke: gray;
        stroke-width: 1;
        stroke-dasharray: 3,3;
        fill: none;
    }
    .drawing-centerline {
        stroke: gray;
        stroke-width: 0.5;
        stroke-dasharray: 10,5,2,5;
        fill: none;
    }
</style>
<body>
    <svg width="300" height="200">
        <!-- Visible edges -->
        <rect class="drawing-visible" x="50" y="30" width="100" height="80"/>
        <!-- Hidden edges -->
        <line class="drawing-hidden" x1="50" y1="110" x2="150" y2="30"/>
        <!-- Centerlines -->
        <line class="drawing-centerline" x1="100" y1="20" x2="100" y2="120"/>
        <line class="drawing-centerline" x1="40" y1="70" x2="160" y2="70"/>
    </svg>
</body>
```

### Example 11: Map with roads of different sizes

```html
<style>
    .road-highway {
        stroke: #1f2937;
        stroke-width: 6;
    }
    .road-major {
        stroke: #374151;
        stroke-width: 4;
    }
    .road-minor {
        stroke: #6b7280;
        stroke-width: 2;
    }
    .road-local {
        stroke: #9ca3af;
        stroke-width: 1;
    }
</style>
<body>
    <svg width="300" height="250">
        <line class="road-highway" x1="10" y1="125" x2="290" y2="125"/>
        <line class="road-major" x1="150" y1="10" x2="150" y2="240"/>
        <line class="road-minor" x1="80" y1="10" x2="80" y2="240"/>
        <line class="road-local" x1="220" y1="10" x2="220" y2="240"/>
    </svg>
</body>
```

### Example 12: Border emphasis effect

```html
<style>
    .card {
        fill: white;
    }
    .border-subtle {
        stroke: #e5e7eb;
        stroke-width: 1;
    }
    .border-normal {
        stroke: #d1d5db;
        stroke-width: 2;
    }
    .border-emphasis {
        stroke: #3b82f6;
        stroke-width: 4;
    }
</style>
<body>
    <svg width="350" height="120">
        <rect class="card border-subtle" x="10" y="10" width="100" height="100" rx="5"/>
        <rect class="card border-normal" x="125" y="10" width="100" height="100" rx="5"/>
        <rect class="card border-emphasis" x="240" y="10" width="100" height="100" rx="5"/>
    </svg>
</body>
```

### Example 13: Data visualization with varying weights

```html
<style>
    .bar-outline {
        fill: #3b82f6;
    }
    .emphasis-high {
        stroke: #1e40af;
        stroke-width: 5;
    }
    .emphasis-medium {
        stroke: #1e40af;
        stroke-width: 3;
    }
    .emphasis-low {
        stroke: #1e40af;
        stroke-width: 1;
    }
</style>
<body>
    <svg width="400" height="200">
        <rect class="bar-outline emphasis-low" x="20" y="80" width="60" height="100"/>
        <rect class="bar-outline emphasis-medium" x="110" y="40" width="60" height="140"/>
        <rect class="bar-outline emphasis-high" x="200" y="20" width="60" height="160"/>
        <rect class="bar-outline emphasis-medium" x="290" y="60" width="60" height="120"/>
    </svg>
</body>
```

### Example 14: Flowchart with connection weights

```html
<style>
    .box {
        fill: #dbeafe;
        stroke: #3b82f6;
        stroke-width: 2;
    }
    .arrow-primary {
        stroke: #3b82f6;
        stroke-width: 3;
        marker-end: url(#arrowhead);
    }
    .arrow-secondary {
        stroke: #60a5fa;
        stroke-width: 1.5;
    }
</style>
<body>
    <svg width="300" height="200">
        <defs>
            <marker id="arrowhead" markerWidth="10" markerHeight="7" refX="10" refY="3.5" orient="auto">
                <polygon points="0 0, 10 3.5, 0 7" fill="#3b82f6"/>
            </marker>
        </defs>
        <rect class="box" x="20" y="20" width="80" height="60" rx="5"/>
        <rect class="box" x="200" y="20" width="80" height="60" rx="5"/>
        <rect class="box" x="110" y="120" width="80" height="60" rx="5"/>
        <line class="arrow-primary" x1="100" y1="50" x2="200" y2="50"/>
        <line class="arrow-secondary" x1="60" y1="80" x2="150" y2="120"/>
    </svg>
</body>
```

### Example 15: Typography outline effect

```html
<style>
    .text-outline-thick {
        fill: white;
        stroke: black;
        stroke-width: 4;
        font-size: 48pt;
        font-weight: bold;
        font-family: Arial, sans-serif;
    }
    .text-outline-thin {
        fill: white;
        stroke: black;
        stroke-width: 1;
        font-size: 36pt;
        font-family: Arial, sans-serif;
    }
</style>
<body>
    <svg width="400" height="150">
        <text class="text-outline-thick" x="20" y="60">BOLD</text>
        <text class="text-outline-thin" x="20" y="120">Subtle</text>
    </svg>
</body>
```

---

## See Also

- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [stroke-opacity](/reference/cssproperties/css_prop_stroke-opacity) - SVG stroke transparency
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [border](/reference/htmlattributes/attr_border) - HTML border styling

---
