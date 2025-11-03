---
layout: default
title: stroke-opacity
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# stroke-opacity : SVG Stroke Opacity Property

The `stroke-opacity` property controls the transparency of SVG shape strokes independently from the fill. This property enables precise control over outline transparency while maintaining fill opacity, essential for creating layered graphics and sophisticated visual effects in PDF documents.

## Usage

```css
selector {
    stroke-opacity: value;
}
```

The stroke-opacity property accepts a numeric value between 0.0 (fully transparent) and 1.0 (fully opaque).

---

## Supported Values

### Numeric Values
- `0.0` - Fully transparent (invisible stroke)
- `0.5` - 50% transparent (semi-transparent)
- `1.0` - Fully opaque (default, solid stroke)
- Any decimal value between 0.0 and 1.0

### Percentage Values
- `0%` - Fully transparent
- `50%` - 50% transparent
- `100%` - Fully opaque

---

## Supported Elements

The `stroke-opacity` property applies to SVG elements including:
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

- Default stroke-opacity value is `1.0` (fully opaque)
- Values outside the 0.0-1.0 range are clamped to the nearest valid value
- `stroke-opacity` affects only the stroke, not the fill (use `fill-opacity` for fills)
- Can be combined with RGBA stroke colors for additional transparency control
- When both RGBA alpha and stroke-opacity are used, they multiply together
- Stroke-opacity is independent of the overall `opacity` property
- Useful for creating subtle borders and overlay effects
- PDF rendering maintains exact opacity values across viewers
- Semi-transparent strokes blend with underlying content
- Particularly useful for grid lines, guides, and background elements

---

## Data Binding

The `stroke-opacity` property can be dynamically set using data binding expressions, enabling SVG stroke transparency to reflect connection strengths, emphasis levels, or confidence scores.

### Example 1: Connection strength visualization

```html
<style>
    .node {
        fill: white;
        stroke: #3b82f6;
        stroke-width: 2;
    }
    .connection {
        stroke: #3b82f6;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="400" height="200">
        {{#each connections}}
        <line class="connection"
              style="stroke-opacity: {{strength}}"
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

With model data:
```json
{
    "connections": [
        { "from": 0, "to": 1, "strength": 1.0, "x1": 75, "y1": 100, "x2": 175, "y2": 100 },
        { "from": 1, "to": 2, "strength": 0.6, "x1": 175, "y1": 100, "x2": 275, "y2": 100 },
        { "from": 0, "to": 2, "strength": 0.2, "x1": 75, "y1": 100, "x2": 275, "y2": 100 }
    ],
    "nodes": [
        { "id": 0, "x": 75, "y": 100 },
        { "id": 1, "x": 175, "y": 100 },
        { "id": 2, "x": 275, "y": 100 }
    ]
}
```

### Example 2: Conditional emphasis based on importance

```html
<style>
    .boundary {
        fill: none;
        stroke: #1e40af;
        stroke-width: 2;
    }
</style>
<body>
    <svg width="300" height="250">
        {{#each boundaries}}
        <rect class="boundary"
              style="stroke-opacity: {{isImportant ? 1.0 : isMedium ? 0.6 : 0.3}}"
              x="{{x}}"
              y="{{y}}"
              width="{{width}}"
              height="{{height}}"/>
        {{/each}}
    </svg>
</body>
```

### Example 3: Data quality indicators

```html
<style>
    .data-point {
        fill: #3b82f6;
        stroke: #1e40af;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="400" height="150">
        {{#each dataPoints}}
        <circle class="data-point"
                style="stroke-opacity: {{quality}}"
                cx="{{xPos}}"
                cy="75"
                r="20"/>
        {{/each}}
    </svg>
    <p>Stroke opacity indicates data quality (0.0-1.0)</p>
</body>
```

With model data:
```json
{
    "dataPoints": [
        { "value": 85, "quality": 0.95, "xPos": 50 },
        { "value": 72, "quality": 0.80, "xPos": 150 },
        { "value": 68, "quality": 0.50, "xPos": 250 },
        { "value": 91, "quality": 0.30, "xPos": 350 }
    ]
}
```

---

## Examples

### Example 1: Basic stroke opacity

```html
<style>
    .opaque-stroke {
        fill: none;
        stroke: blue;
        stroke-width: 4;
        stroke-opacity: 1.0;
    }
    .transparent-stroke {
        fill: none;
        stroke: blue;
        stroke-width: 4;
        stroke-opacity: 0.3;
    }
</style>
<body>
    <svg width="300" height="150">
        <rect class="opaque-stroke" x="20" y="20" width="100" height="100"/>
        <rect class="transparent-stroke" x="160" y="20" width="100" height="100"/>
    </svg>
</body>
```

### Example 2: Overlapping strokes with transparency

```html
<style>
    .circle-outline {
        fill: none;
        stroke-width: 8;
        stroke-opacity: 0.5;
    }
    .circle-red { stroke: red; }
    .circle-blue { stroke: blue; }
    .circle-green { stroke: green; }
</style>
<body>
    <svg width="300" height="150">
        <circle class="circle-outline circle-red" cx="90" cy="75" r="50"/>
        <circle class="circle-outline circle-blue" cx="150" cy="75" r="50"/>
        <circle class="circle-outline circle-green" cx="210" cy="75" r="50"/>
    </svg>
</body>
```

### Example 3: Gradient-like effect with varying opacity

```html
<style>
    .line { stroke: #3b82f6; stroke-width: 3; }
    .opacity-20 { stroke-opacity: 0.2; }
    .opacity-40 { stroke-opacity: 0.4; }
    .opacity-60 { stroke-opacity: 0.6; }
    .opacity-80 { stroke-opacity: 0.8; }
    .opacity-100 { stroke-opacity: 1.0; }
</style>
<body>
    <svg width="300" height="150">
        <line class="line opacity-20" x1="30" y1="20" x2="30" y2="130"/>
        <line class="line opacity-40" x1="80" y1="20" x2="80" y2="130"/>
        <line class="line opacity-60" x1="130" y1="20" x2="130" y2="130"/>
        <line class="line opacity-80" x1="180" y1="20" x2="180" y2="130"/>
        <line class="line opacity-100" x1="230" y1="20" x2="230" y2="130"/>
    </svg>
</body>
```

### Example 4: Stroke opacity with solid fill

```html
<style>
    .shape {
        fill: yellow;
        fill-opacity: 1.0;
        stroke: red;
        stroke-width: 5;
        stroke-opacity: 0.4;
    }
</style>
<body>
    <svg width="200" height="200">
        <rect class="shape" x="30" y="30" width="140" height="140"/>
    </svg>
</body>
```

### Example 5: Percentage-based stroke opacity

```html
<style>
    .rect { fill: lightblue; stroke: darkblue; stroke-width: 4; }
    .stroke-25 { stroke-opacity: 25%; }
    .stroke-50 { stroke-opacity: 50%; }
    .stroke-75 { stroke-opacity: 75%; }
    .stroke-100 { stroke-opacity: 100%; }
</style>
<body>
    <svg width="400" height="100">
        <rect class="rect stroke-25" x="10" y="10" width="80" height="80"/>
        <rect class="rect stroke-50" x="110" y="10" width="80" height="80"/>
        <rect class="rect stroke-75" x="210" y="10" width="80" height="80"/>
        <rect class="rect stroke-100" x="310" y="10" width="80" height="80"/>
    </svg>
</body>
```

### Example 6: Grid lines with subtle strokes

```html
<style>
    .grid-line {
        stroke: #9ca3af;
        stroke-width: 1;
        stroke-opacity: 0.3;
    }
    .grid-major {
        stroke: #6b7280;
        stroke-width: 1;
        stroke-opacity: 0.5;
    }
</style>
<body>
    <svg width="300" height="200">
        <!-- Minor grid lines -->
        <line class="grid-line" x1="0" y1="50" x2="300" y2="50"/>
        <line class="grid-line" x1="0" y1="150" x2="300" y2="150"/>
        <line class="grid-line" x1="75" y1="0" x2="75" y2="200"/>
        <line class="grid-line" x1="225" y1="0" x2="225" y2="200"/>
        <!-- Major grid lines -->
        <line class="grid-major" x1="0" y1="100" x2="300" y2="100"/>
        <line class="grid-major" x1="150" y1="0" x2="150" y2="200"/>
    </svg>
</body>
```

### Example 7: Focus and hover states

```html
<style>
    .button-shape {
        fill: #3b82f6;
        stroke: white;
        stroke-width: 3;
    }
    .state-normal { stroke-opacity: 0.0; }
    .state-hover { stroke-opacity: 0.5; }
    .state-focus { stroke-opacity: 1.0; }
</style>
<body>
    <svg width="300" height="80">
        <rect class="button-shape state-normal" x="10" y="20" width="80" height="40" rx="5"/>
        <rect class="button-shape state-hover" x="110" y="20" width="80" height="40" rx="5"/>
        <rect class="button-shape state-focus" x="210" y="20" width="80" height="40" rx="5"/>
    </svg>
    <p>Normal - Hover - Focus states</p>
</body>
```

### Example 8: Chart with emphasized data points

```html
<style>
    .data-point {
        fill: #3b82f6;
        stroke: #1e40af;
        stroke-width: 3;
    }
    .emphasis-none { stroke-opacity: 0.2; }
    .emphasis-partial { stroke-opacity: 0.5; }
    .emphasis-full { stroke-opacity: 1.0; }
</style>
<body>
    <svg width="400" height="150">
        <circle class="data-point emphasis-none" cx="50" cy="75" r="20"/>
        <circle class="data-point emphasis-none" cx="150" cy="75" r="20"/>
        <circle class="data-point emphasis-partial" cx="250" cy="75" r="20"/>
        <circle class="data-point emphasis-full" cx="350" cy="75" r="20"/>
    </svg>
</body>
```

### Example 9: Layered border effect

```html
<style>
    .base-shape {
        fill: white;
    }
    .border-outer {
        stroke: #3b82f6;
        stroke-width: 8;
        stroke-opacity: 0.2;
    }
    .border-middle {
        stroke: #3b82f6;
        stroke-width: 5;
        stroke-opacity: 0.5;
    }
    .border-inner {
        stroke: #3b82f6;
        stroke-width: 2;
        stroke-opacity: 1.0;
    }
</style>
<body>
    <svg width="200" height="200">
        <rect class="base-shape border-outer" x="50" y="50" width="100" height="100"/>
        <rect class="base-shape border-middle" x="50" y="50" width="100" height="100"/>
        <rect class="base-shape border-inner" x="50" y="50" width="100" height="100"/>
    </svg>
</body>
```

### Example 10: Network connection strengths

```html
<style>
    .node {
        fill: white;
        stroke: #3b82f6;
        stroke-width: 3;
    }
    .connection { stroke: #3b82f6; stroke-width: 2; }
    .connection-weak { stroke-opacity: 0.2; }
    .connection-medium { stroke-opacity: 0.5; }
    .connection-strong { stroke-opacity: 1.0; }
</style>
<body>
    <svg width="300" height="150">
        <line class="connection connection-weak" x1="75" y1="75" x2="150" y2="75"/>
        <line class="connection connection-strong" x1="150" y1="75" x2="225" y2="75"/>
        <circle class="node" cx="75" cy="75" r="20"/>
        <circle class="node" cx="150" cy="75" r="20"/>
        <circle class="node" cx="225" cy="75" r="20"/>
    </svg>
</body>
```

### Example 11: Annotated diagram

```html
<style>
    .shape-main {
        fill: #dbeafe;
        stroke: #3b82f6;
        stroke-width: 2;
        stroke-opacity: 1.0;
    }
    .annotation {
        fill: none;
        stroke: #f59e0b;
        stroke-width: 2;
        stroke-opacity: 0.6;
        stroke-dasharray: 5,5;
    }
</style>
<body>
    <svg width="300" height="200">
        <rect class="shape-main" x="50" y="50" width="200" height="100"/>
        <circle class="annotation" cx="150" cy="100" r="80"/>
    </svg>
</body>
```

### Example 12: Progress indicator rings

```html
<style>
    .ring {
        fill: none;
        stroke-width: 10;
    }
    .ring-background {
        stroke: #e5e7eb;
        stroke-opacity: 0.3;
    }
    .ring-progress {
        stroke: #10b981;
        stroke-opacity: 1.0;
    }
</style>
<body>
    <svg width="150" height="150">
        <circle class="ring ring-background" cx="75" cy="75" r="50"/>
        <circle class="ring ring-progress" cx="75" cy="75" r="50" stroke-dasharray="235 314"/>
    </svg>
    <p>75% Complete</p>
</body>
```

### Example 13: Map boundaries

```html
<style>
    .region {
        fill: #dbeafe;
    }
    .border-international {
        stroke: #1e40af;
        stroke-width: 3;
        stroke-opacity: 1.0;
    }
    .border-provincial {
        stroke: #60a5fa;
        stroke-width: 2;
        stroke-opacity: 0.6;
    }
    .border-municipal {
        stroke: #93c5fd;
        stroke-width: 1;
        stroke-opacity: 0.3;
    }
</style>
<body>
    <svg width="300" height="200">
        <rect class="region border-international" x="20" y="20" width="260" height="160"/>
        <line class="border-provincial" x1="150" y1="20" x2="150" y2="180"/>
        <line class="border-municipal" x1="20" y1="100" x2="280" y2="100"/>
    </svg>
</body>
```

### Example 14: Timeline with varying emphasis

```html
<style>
    .timeline-line {
        stroke: #9ca3af;
        stroke-width: 2;
        stroke-opacity: 0.4;
    }
    .milestone {
        fill: white;
        stroke: #3b82f6;
        stroke-width: 3;
    }
    .milestone-past { stroke-opacity: 0.3; }
    .milestone-current { stroke-opacity: 1.0; }
    .milestone-future { stroke-opacity: 0.5; }
</style>
<body>
    <svg width="400" height="100">
        <line class="timeline-line" x1="50" y1="50" x2="350" y2="50"/>
        <circle class="milestone milestone-past" cx="50" cy="50" r="12"/>
        <circle class="milestone milestone-past" cx="150" cy="50" r="12"/>
        <circle class="milestone milestone-current" cx="250" cy="50" r="12"/>
        <circle class="milestone milestone-future" cx="350" cy="50" r="12"/>
    </svg>
</body>
```

### Example 15: Architectural floor plan

```html
<style>
    .wall-exterior {
        fill: none;
        stroke: black;
        stroke-width: 4;
        stroke-opacity: 1.0;
    }
    .wall-interior {
        fill: none;
        stroke: black;
        stroke-width: 2;
        stroke-opacity: 0.7;
    }
    .wall-proposed {
        fill: none;
        stroke: gray;
        stroke-width: 2;
        stroke-opacity: 0.4;
        stroke-dasharray: 5,5;
    }
</style>
<body>
    <svg width="300" height="250">
        <!-- Exterior walls -->
        <rect class="wall-exterior" x="20" y="20" width="260" height="210"/>
        <!-- Interior walls -->
        <line class="wall-interior" x1="150" y1="20" x2="150" y2="230"/>
        <line class="wall-interior" x1="20" y1="120" x2="280" y2="120"/>
        <!-- Proposed walls -->
        <line class="wall-proposed" x1="20" y1="175" x2="150" y2="175"/>
    </svg>
</body>
```

---

## See Also

- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [stroke-width](/reference/cssproperties/css_prop_stroke-width) - Control stroke thickness
- [fill-opacity](/reference/cssproperties/css_prop_fill-opacity) - SVG fill transparency
- [opacity](/reference/cssproperties/css_prop_opacity) - Overall element transparency

---
