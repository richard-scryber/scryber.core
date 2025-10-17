---
layout: default
title: stroke-linejoin
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# stroke-linejoin : SVG Line Join Style Property

The `stroke-linejoin` property defines the shape to be used at the corners where two line segments meet in SVG strokes. This property controls how corners appear, whether they are sharp mitered edges, rounded curves, or beveled (cut off) corners.

## Usage

```css
selector {
    stroke-linejoin: value;
}
```

The stroke-linejoin property accepts one of three keyword values that determine the appearance of stroke corners and joints.

---

## Supported Values

### Keywords

- `miter` - Default value. Creates a sharp corner by extending the outer edges of the stroke until they meet. The `stroke-miterlimit` property controls how far the miter can extend before being converted to a bevel.

- `round` - Creates a rounded corner with a radius equal to half the stroke width. Produces smooth, curved corners.

- `bevel` - Creates a cut-off corner by connecting the outer edges with a straight line. The corner is "clipped" at the point where the strokes would have met.

---

## Supported Elements

The `stroke-linejoin` property applies to SVG elements including:
- `<polyline>` polylines
- `<polygon>` polygons
- `<path>` paths with angular segments
- `<rect>` rectangles (at corners)
- Any SVG element with stroke corners where line segments meet

Note: This property has no effect on smooth curves or elements without corners.

---

## Notes

- Default value is `miter` (sharp corners)
- `miter` can create very long spikes at acute angles (use `stroke-miterlimit` to control)
- `round` is ideal for friendly, approachable designs
- `bevel` is useful for preventing sharp spikes at acute angles
- Affects all corners in a shape uniformly (cannot set per-corner)
- Works with both solid and dashed strokes
- Essential for polygons, rectangles, and angular paths
- Does not affect line endpoints (use `stroke-linecap` instead)
- Important for technical drawings and diagrams
- Vector corners maintain quality at any zoom level in PDF viewers
- Can significantly affect the appearance of shapes with many corners

---

## Data Binding

The `stroke-linejoin` property can be dynamically controlled through data binding, enabling responsive corner styles based on data values or states. This is particularly useful for creating adaptive UI elements and data-driven visualizations.

### Example 1: Data-driven polygon styles

```html
<style>
    .data-polygon {
        fill: none;
        stroke-width: 4;
    }
</style>
<body>
    <svg width="600" height="250" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each shapes}}">
            <polygon class="data-polygon"
                     points="{{pointsArray}}"
                     stroke="{{borderColor}}"
                     style="stroke-linejoin: {{joinStyle}}; fill: {{fillColor}}; fill-opacity: {{opacity}}"/>
        </template>
    </svg>
</body>
```

### Example 2: Conditional border styles for UI states

```html
<style>
    .ui-frame {
        stroke-width: 3;
    }
</style>
<body>
    <svg width="450" height="300" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each panels}}">
            <rect class="ui-frame"
                  x="{{x}}" y="{{y}}" width="{{width}}" height="{{height}}"
                  stroke="{{stateColor}}"
                  fill="{{backgroundColor}}"
                  style="stroke-linejoin: {{#if selected}}round{{else}}miter{{/if}}"/>
            <text x="{{x | add: 10}}" y="{{y | add: 25}}" font-size="14">{{label}}</text>
        </template>
    </svg>
</body>
```

### Example 3: Dynamic chart elements with variable line joins

```html
<style>
    .chart-area {
        fill-opacity: 0.3;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="500" height="300" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each datasets}}">
            <polyline class="chart-area"
                      points="{{dataPath}}"
                      fill="{{areaColor}}"
                      stroke="{{lineColor}}"
                      style="stroke-linejoin: {{smoothCorners | ternary: 'round': 'miter'}}"/>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Comparing all linejoin styles

```html
<style>
    .shape-base { fill: none; stroke: #3b82f6; stroke-width: 10; }
    .join-miter { stroke-linejoin: miter; }
    .join-round { stroke-linejoin: round; }
    .join-bevel { stroke-linejoin: bevel; }
</style>
<body>
    <svg width="400" height="200">
        <polyline class="shape-base join-miter" points="30,150 80,50 130,150"/>
        <polyline class="shape-base join-round" points="160,150 210,50 260,150"/>
        <polyline class="shape-base join-bevel" points="290,150 340,50 390,150"/>
    </svg>
</body>
```

### Example 2: Triangle with different joins

```html
<style>
    .triangle-miter {
        fill: #dbeafe;
        stroke: #1e40af;
        stroke-width: 6;
        stroke-linejoin: miter;
    }
    .triangle-round {
        fill: #fef3c7;
        stroke: #d97706;
        stroke-width: 6;
        stroke-linejoin: round;
    }
    .triangle-bevel {
        fill: #fee2e2;
        stroke: #dc2626;
        stroke-width: 6;
        stroke-linejoin: bevel;
    }
</style>
<body>
    <svg width="400" height="150">
        <polygon class="triangle-miter" points="50,120 100,30 150,120"/>
        <polygon class="triangle-round" points="170,120 220,30 270,120"/>
        <polygon class="triangle-bevel" points="290,120 340,30 390,120"/>
    </svg>
</body>
```

### Example 3: Rectangle corners

```html
<style>
    .rect-miter {
        fill: none;
        stroke: #8b5cf6;
        stroke-width: 8;
        stroke-linejoin: miter;
    }
    .rect-round {
        fill: none;
        stroke: #10b981;
        stroke-width: 8;
        stroke-linejoin: round;
    }
    .rect-bevel {
        fill: none;
        stroke: #f59e0b;
        stroke-width: 8;
        stroke-linejoin: bevel;
    }
</style>
<body>
    <svg width="400" height="250">
        <rect class="rect-miter" x="30" y="20" width="100" height="60"/>
        <rect class="rect-round" x="160" y="20" width="100" height="60"/>
        <rect class="rect-bevel" x="290" y="20" width="100" height="60"/>
    </svg>
</body>
```

### Example 4: Zigzag pattern

```html
<style>
    .zigzag-sharp {
        fill: none;
        stroke: #ef4444;
        stroke-width: 4;
        stroke-linejoin: miter;
    }
    .zigzag-smooth {
        fill: none;
        stroke: #3b82f6;
        stroke-width: 4;
        stroke-linejoin: round;
    }
</style>
<body>
    <svg width="300" height="150">
        <polyline class="zigzag-sharp" points="10,40 40,10 70,40 100,10 130,40 160,10 190,40 220,10 250,40 280,10"/>
        <polyline class="zigzag-smooth" points="10,110 40,80 70,110 100,80 130,110 160,80 190,110 220,80 250,110 280,80"/>
    </svg>
</body>
```

### Example 5: Star shape comparison

```html
<style>
    .star-miter {
        fill: #fef3c7;
        stroke: #d97706;
        stroke-width: 3;
        stroke-linejoin: miter;
    }
    .star-round {
        fill: #dbeafe;
        stroke: #1e40af;
        stroke-width: 3;
        stroke-linejoin: round;
    }
</style>
<body>
    <svg width="300" height="200">
        <polygon class="star-miter" points="75,20 85,60 125,60 95,85 105,125 75,100 45,125 55,85 25,60 65,60"/>
        <polygon class="star-round" points="225,20 235,60 275,60 245,85 255,125 225,100 195,125 205,85 175,60 215,60"/>
    </svg>
</body>
```

### Example 6: Arrow shapes

```html
<style>
    .arrow-sharp {
        fill: #3b82f6;
        stroke: #1e40af;
        stroke-width: 2;
        stroke-linejoin: miter;
    }
    .arrow-soft {
        fill: #10b981;
        stroke: #059669;
        stroke-width: 2;
        stroke-linejoin: round;
    }
</style>
<body>
    <svg width="300" height="150">
        <polygon class="arrow-sharp" points="30,50 80,50 80,30 130,70 80,110 80,90 30,90"/>
        <polygon class="arrow-soft" points="170,50 220,50 220,30 270,70 220,110 220,90 170,90"/>
    </svg>
</body>
```

### Example 7: Lightning bolt icon

```html
<style>
    .lightning-miter {
        fill: #fbbf24;
        stroke: #d97706;
        stroke-width: 2;
        stroke-linejoin: miter;
    }
    .lightning-bevel {
        fill: #fbbf24;
        stroke: #d97706;
        stroke-width: 2;
        stroke-linejoin: bevel;
    }
</style>
<body>
    <svg width="300" height="200">
        <polygon class="lightning-miter" points="80,20 70,90 110,90 60,170 75,100 35,100"/>
        <polygon class="lightning-bevel" points="230,20 220,90 260,90 210,170 225,100 185,100"/>
    </svg>
</body>
```

### Example 8: Hexagon comparison

```html
<style>
    .hex-base { fill: none; stroke-width: 6; }
    .hex-miter { stroke: #8b5cf6; stroke-linejoin: miter; }
    .hex-round { stroke: #10b981; stroke-linejoin: round; }
    .hex-bevel { stroke: #ef4444; stroke-linejoin: bevel; }
</style>
<body>
    <svg width="400" height="200">
        <polygon class="hex-base hex-miter" points="60,40 90,25 120,40 120,70 90,85 60,70"/>
        <polygon class="hex-base hex-round" points="180,40 210,25 240,40 240,70 210,85 180,70"/>
        <polygon class="hex-base hex-bevel" points="300,40 330,25 360,40 360,70 330,85 300,70"/>
    </svg>
</body>
```

### Example 9: Frame corners

```html
<style>
    .frame-outer {
        fill: none;
        stroke: #1f2937;
        stroke-width: 8;
        stroke-linejoin: miter;
    }
    .frame-inner {
        fill: white;
        stroke: #6b7280;
        stroke-width: 2;
        stroke-linejoin: round;
    }
</style>
<body>
    <svg width="300" height="250">
        <rect class="frame-outer" x="30" y="30" width="240" height="190"/>
        <rect class="frame-inner" x="50" y="50" width="200" height="150"/>
    </svg>
</body>
```

### Example 10: Chevron icons

```html
<style>
    .chevron {
        fill: none;
        stroke: #3b82f6;
        stroke-width: 4;
        stroke-linejoin: round;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="300" height="100">
        <polyline class="chevron" points="30,30 60,60 30,90"/>
        <polyline class="chevron" points="100,30 130,60 100,90"/>
        <polyline class="chevron" points="170,30 200,60 170,90"/>
        <polyline class="chevron" points="240,30 270,60 240,90"/>
    </svg>
</body>
```

### Example 11: Diamond grid pattern

```html
<style>
    .diamond {
        fill: none;
        stroke: #6b7280;
        stroke-width: 2;
        stroke-linejoin: miter;
    }
</style>
<body>
    <svg width="300" height="200">
        <polygon class="diamond" points="75,20 130,75 75,130 20,75"/>
        <polygon class="diamond" points="155,20 210,75 155,130 100,75"/>
        <polygon class="diamond" points="235,20 290,75 235,130 180,75"/>
    </svg>
</body>
```

### Example 12: Speech bubble

```html
<style>
    .bubble-sharp {
        fill: white;
        stroke: #3b82f6;
        stroke-width: 3;
        stroke-linejoin: miter;
    }
    .bubble-round {
        fill: white;
        stroke: #10b981;
        stroke-width: 3;
        stroke-linejoin: round;
    }
</style>
<body>
    <svg width="300" height="250">
        <path class="bubble-sharp" d="M 30 30 L 150 30 L 150 80 L 85 80 L 70 110 L 65 80 L 30 80 Z"/>
        <path class="bubble-round" d="M 30 140 L 150 140 L 150 190 L 85 190 L 70 220 L 65 190 L 30 190 Z"/>
    </svg>
</body>
```

### Example 13: Acute angle handling

```html
<style>
    .acute-miter {
        fill: none;
        stroke: #ef4444;
        stroke-width: 10;
        stroke-linejoin: miter;
        stroke-miterlimit: 2;
    }
    .acute-bevel {
        fill: none;
        stroke: #10b981;
        stroke-width: 10;
        stroke-linejoin: bevel;
    }
</style>
<body>
    <svg width="300" height="150">
        <!-- Very acute angle -->
        <polyline class="acute-miter" points="30,100 80,40 130,100"/>
        <polyline class="acute-bevel" points="170,100 220,40 270,100"/>
    </svg>
</body>
```

### Example 14: Bracket symbols

```html
<style>
    .bracket {
        fill: none;
        stroke: #374151;
        stroke-width: 4;
        stroke-linejoin: miter;
        stroke-linecap: square;
    }
</style>
<body>
    <svg width="300" height="150">
        <polyline class="bracket" points="50,30 30,30 30,120 50,120"/>
        <polyline class="bracket" points="110,30 130,30 130,120 110,120"/>
        <polyline class="bracket" points="170,30 190,30 190,75 210,75 190,75 190,120 170,120"/>
        <polyline class="bracket" points="250,30 270,30 270,120 250,120"/>
    </svg>
</body>
```

### Example 15: UI button borders with different styles

```html
<style>
    .btn-sharp {
        fill: #eff6ff;
        stroke: #3b82f6;
        stroke-width: 3;
        stroke-linejoin: miter;
    }
    .btn-soft {
        fill: #f0fdf4;
        stroke: #10b981;
        stroke-width: 3;
        stroke-linejoin: round;
    }
    .btn-flat {
        fill: #fef3c7;
        stroke: #f59e0b;
        stroke-width: 3;
        stroke-linejoin: bevel;
    }
</style>
<body>
    <svg width="300" height="250">
        <rect class="btn-sharp" x="50" y="30" width="200" height="50"/>
        <rect class="btn-soft" x="50" y="100" width="200" height="50"/>
        <rect class="btn-flat" x="50" y="170" width="200" height="50"/>
    </svg>
</body>
```

---

## See Also

- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [stroke-width](/reference/cssproperties/css_prop_stroke-width) - Control stroke thickness
- [stroke-linecap](/reference/cssproperties/css_prop_stroke-linecap) - Control line ending appearance
- [stroke-dasharray](/reference/cssproperties/css_prop_stroke-dasharray) - Define dash pattern
- [stroke-opacity](/reference/cssproperties/css_prop_stroke-opacity) - SVG stroke transparency
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
