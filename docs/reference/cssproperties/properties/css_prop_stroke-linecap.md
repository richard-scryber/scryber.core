---
layout: default
title: stroke-linecap
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# stroke-linecap : SVG Line Cap Style Property

The `stroke-linecap` property defines the shape to be used at the end of open subpaths and dashes in SVG strokes. This property controls how line endings appear, whether they are flat, rounded, or extended with a square cap.

## Usage

```css
selector {
    stroke-linecap: value;
}
```

The stroke-linecap property accepts one of three keyword values that determine the appearance of line endings.

---

## Supported Values

### Keywords

- `butt` - Default value. Line ends at the exact endpoint with no extension. Creates a flat edge perpendicular to the direction of the stroke.

- `round` - Adds a semicircular cap with a radius equal to half the stroke width. The cap extends beyond the endpoint by half the stroke width.

- `square` - Adds a square cap with sides equal to half the stroke width. The cap extends beyond the endpoint by half the stroke width, similar to round but with square corners.

---

## Supported Elements

The `stroke-linecap` property applies to SVG elements including:
- `<line>` lines
- `<polyline>` polylines
- `<path>` paths (open subpaths)
- Any SVG element with a stroke and open endpoints
- Dash segments when used with `stroke-dasharray`

Note: This property has no effect on closed shapes like `<rect>`, `<circle>`, `<ellipse>`, or closed `<polygon>` elements.

---

## Notes

- Default value is `butt` (no extension beyond the endpoint)
- The `round` and `square` caps extend beyond the defined endpoint
- The extension distance is half the `stroke-width` in each direction
- `round` is commonly used for smooth, friendly visual styles
- `square` can be used to ensure lines meet precisely at corners
- Essential for creating dotted lines when combined with `stroke-dasharray`
- Affects the total rendered length of lines and paths
- Important for technical drawings where precise dimensions matter
- Does not affect closed shapes or polygon corners (use `stroke-linejoin` instead)
- Vector line caps maintain quality at any zoom level in PDF viewers

---

## Data Binding

The `stroke-linecap` property can be dynamically set through data binding to create responsive line styles based on data states. This is useful for conditional styling in diagrams, visualizations, and UI components.

### Example 1: Data-driven line styles in charts

```html
<style>
    .chart-line {
        stroke-width: 3;
        fill: none;
    }
</style>
<body>
    <svg width="500" height="250" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each series}}">
            <polyline class="chart-line"
                      points="{{dataPoints}}"
                      stroke="{{color}}"
                      style="stroke-linecap: {{lineCapStyle}}; stroke-dasharray: {{dashPattern}}"/>
        </template>
    </svg>
</body>
```

### Example 2: Conditional linecap based on connection status

```html
<style>
    .connector {
        stroke-width: 4;
    }
</style>
<body>
    <svg width="400" height="300" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each connections}}">
            <line class="connector"
                  x1="{{startX}}" y1="{{startY}}"
                  x2="{{endX}}" y2="{{endY}}"
                  stroke="{{statusColor}}"
                  style="stroke-linecap: {{#if active}}round{{else}}butt{{/if}}; stroke-dasharray: {{#if active}}none{{else}}8 4{{/if}}"/>
        </template>
    </svg>
</body>
```

### Example 3: Dynamic progress bars with bound line caps

```html
<style>
    .progress-track { stroke: #e5e7eb; stroke-width: 12; }
    .progress-bar { stroke-width: 12; }
</style>
<body>
    <svg width="400" height="250" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each tasks}}">
            <g transform="translate(0, {{@index | multiply: 60}})">
                <text x="10" y="30" font-size="14">{{taskName}}</text>
                <line class="progress-track" x1="140" y1="25" x2="380" y2="25" stroke-linecap="round"/>
                <line class="progress-bar" x1="140" y1="25" x2="{{140 | add: progress | multiply: 2.4}}" y2="25"
                      stroke="{{progressColor}}"
                      style="stroke-linecap: {{#if progress | greaterThan: 95}}round{{else}}butt{{/if}}"/>
            </g>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Comparing all linecap styles

```html
<style>
    .line-base { stroke: #3b82f6; stroke-width: 10; }
    .cap-butt { stroke-linecap: butt; }
    .cap-round { stroke-linecap: round; }
    .cap-square { stroke-linecap: square; }
    .guide { stroke: #e5e7eb; stroke-width: 1; }
</style>
<body>
    <svg width="300" height="200">
        <!-- Guide lines showing endpoints -->
        <line class="guide" x1="50" y1="0" x2="50" y2="200"/>
        <line class="guide" x1="250" y1="0" x2="250" y2="200"/>

        <!-- Butt cap -->
        <line class="line-base cap-butt" x1="50" y1="40" x2="250" y2="40"/>

        <!-- Round cap -->
        <line class="line-base cap-round" x1="50" y1="100" x2="250" y2="100"/>

        <!-- Square cap -->
        <line class="line-base cap-square" x1="50" y1="160" x2="250" y2="160"/>
    </svg>
</body>
```

### Example 2: Dotted line with round caps

```html
<style>
    .dotted-round {
        stroke: #ef4444;
        stroke-width: 6;
        stroke-dasharray: 1 10;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="dotted-round" x1="10" y1="50" x2="290" y2="50"/>
    </svg>
</body>
```

### Example 3: Dashed line comparison

```html
<style>
    .dash-base { stroke: black; stroke-width: 4; stroke-dasharray: 20 10; }
    .dash-butt { stroke-linecap: butt; }
    .dash-round { stroke-linecap: round; }
    .dash-square { stroke-linecap: square; }
</style>
<body>
    <svg width="300" height="200">
        <line class="dash-base dash-butt" x1="10" y1="40" x2="290" y2="40"/>
        <line class="dash-base dash-round" x1="10" y1="100" x2="290" y2="100"/>
        <line class="dash-base dash-square" x1="10" y1="160" x2="290" y2="160"/>
    </svg>
</body>
```

### Example 4: Arrow indicators

```html
<style>
    .arrow-line {
        stroke: #6b7280;
        stroke-width: 3;
        stroke-linecap: square;
    }
    .arrow-head {
        fill: #6b7280;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="arrow-line" x1="20" y1="50" x2="250" y2="50"/>
        <polygon class="arrow-head" points="250,50 235,42 235,58"/>
    </svg>
</body>
```

### Example 5: Progress bars with rounded ends

```html
<style>
    .progress-track {
        stroke: #e5e7eb;
        stroke-width: 12;
        stroke-linecap: round;
    }
    .progress-bar {
        stroke: #10b981;
        stroke-width: 12;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="300" height="150">
        <line class="progress-track" x1="25" y1="40" x2="275" y2="40"/>
        <line class="progress-bar" x1="25" y1="40" x2="150" y2="40"/>

        <line class="progress-track" x1="25" y1="100" x2="275" y2="100"/>
        <line class="progress-bar" x1="25" y1="100" x2="225" y2="100"/>
    </svg>
</body>
```

### Example 6: Railroad crossing

```html
<style>
    .rail {
        stroke: #374151;
        stroke-width: 6;
        stroke-linecap: butt;
    }
    .crossing-x {
        stroke: #ef4444;
        stroke-width: 8;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="200" height="200">
        <line class="rail" x1="0" y1="80" x2="200" y2="80"/>
        <line class="rail" x1="0" y1="120" x2="200" y2="120"/>
        <line class="crossing-x" x1="60" y1="60" x2="140" y2="140"/>
        <line class="crossing-x" x1="140" y1="60" x2="60" y2="140"/>
    </svg>
</body>
```

### Example 7: Technical drawing with square caps

```html
<style>
    .dimension-line {
        stroke: #1f2937;
        stroke-width: 1;
        stroke-linecap: square;
    }
    .dimension-mark {
        stroke: #1f2937;
        stroke-width: 2;
        stroke-linecap: butt;
    }
</style>
<body>
    <svg width="300" height="150">
        <line class="dimension-line" x1="50" y1="75" x2="250" y2="75"/>
        <line class="dimension-mark" x1="50" y1="65" x2="50" y2="85"/>
        <line class="dimension-mark" x1="250" y1="65" x2="250" y2="85"/>
    </svg>
</body>
```

### Example 8: Hand-drawn style lines

```html
<style>
    .sketch-line {
        stroke: #374151;
        stroke-width: 3;
        stroke-linecap: round;
        fill: none;
    }
</style>
<body>
    <svg width="300" height="200">
        <path class="sketch-line" d="M 20 50 Q 80 30 140 50 T 280 50"/>
        <path class="sketch-line" d="M 20 100 L 70 90 L 120 110 L 170 95 L 220 105 L 280 100"/>
        <path class="sketch-line" d="M 20 150 Q 100 120 180 150"/>
    </svg>
</body>
```

### Example 9: Music notation stems

```html
<style>
    .note-stem {
        stroke: black;
        stroke-width: 2;
        stroke-linecap: butt;
    }
    .note-head {
        fill: black;
    }
</style>
<body>
    <svg width="300" height="150">
        <ellipse class="note-head" cx="50" cy="100" rx="8" ry="6" transform="rotate(-20 50 100)"/>
        <line class="note-stem" x1="58" y1="100" x2="58" y2="40"/>

        <ellipse class="note-head" cx="120" cy="90" rx="8" ry="6" transform="rotate(-20 120 90)"/>
        <line class="note-stem" x1="128" y1="90" x2="128" y2="30"/>

        <ellipse class="note-head" cx="190" cy="110" rx="8" ry="6" transform="rotate(-20 190 110)"/>
        <line class="note-stem" x1="198" y1="110" x2="198" y2="50"/>
    </svg>
</body>
```

### Example 10: Circuit diagram connections

```html
<style>
    .circuit-wire {
        stroke: #1f2937;
        stroke-width: 2;
        stroke-linecap: round;
        fill: none;
    }
    .junction-dot {
        fill: #1f2937;
    }
</style>
<body>
    <svg width="300" height="200">
        <path class="circuit-wire" d="M 20 100 L 100 100 L 100 50"/>
        <path class="circuit-wire" d="M 100 100 L 200 100"/>
        <path class="circuit-wire" d="M 100 100 L 100 150"/>
        <circle class="junction-dot" cx="100" cy="100" r="4"/>
    </svg>
</body>
```

### Example 11: Loading spinner dots

```html
<style>
    .spinner-dot {
        stroke: #3b82f6;
        stroke-width: 8;
        stroke-dasharray: 1 20;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="150" height="150">
        <circle class="spinner-dot" cx="75" cy="75" r="40" fill="none"/>
    </svg>
</body>
```

### Example 12: Decorative separator lines

```html
<style>
    .separator-simple {
        stroke: #d1d5db;
        stroke-width: 2;
        stroke-linecap: round;
    }
    .separator-fancy {
        stroke: #6b7280;
        stroke-width: 3;
        stroke-dasharray: 50 10;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="300" height="150">
        <line class="separator-simple" x1="30" y1="50" x2="270" y2="50"/>
        <line class="separator-fancy" x1="30" y1="100" x2="270" y2="100"/>
    </svg>
</body>
```

### Example 13: Graph data points connection

```html
<style>
    .data-line {
        stroke: #8b5cf6;
        stroke-width: 3;
        stroke-linecap: round;
        fill: none;
    }
    .data-point {
        fill: white;
        stroke: #8b5cf6;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="300" height="200">
        <polyline class="data-line" points="30,150 80,100 130,120 180,60 230,90 280,40"/>
        <circle class="data-point" cx="30" cy="150" r="5"/>
        <circle class="data-point" cx="80" cy="100" r="5"/>
        <circle class="data-point" cx="130" cy="120" r="5"/>
        <circle class="data-point" cx="180" cy="60" r="5"/>
        <circle class="data-point" cx="230" cy="90" r="5"/>
        <circle class="data-point" cx="280" cy="40" r="5"/>
    </svg>
</body>
```

### Example 14: Brush stroke effect

```html
<style>
    .brush-stroke {
        stroke: #1f2937;
        stroke-width: 12;
        stroke-linecap: round;
        fill: none;
        opacity: 0.8;
    }
</style>
<body>
    <svg width="300" height="150">
        <path class="brush-stroke" d="M 20 75 Q 80 40 150 75 T 280 75"/>
    </svg>
</body>
```

### Example 15: UI slider track with rounded ends

```html
<style>
    .slider-track {
        stroke: #e5e7eb;
        stroke-width: 8;
        stroke-linecap: round;
    }
    .slider-fill {
        stroke: #3b82f6;
        stroke-width: 8;
        stroke-linecap: round;
    }
    .slider-thumb {
        fill: white;
        stroke: #3b82f6;
        stroke-width: 3;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="slider-track" x1="50" y1="50" x2="250" y2="50"/>
        <line class="slider-fill" x1="50" y1="50" x2="150" y2="50"/>
        <circle class="slider-thumb" cx="150" cy="50" r="12"/>
    </svg>
</body>
```

---

## See Also

- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [stroke-width](/reference/cssproperties/css_prop_stroke-width) - Control stroke thickness
- [stroke-dasharray](/reference/cssproperties/css_prop_stroke-dasharray) - Define dash pattern
- [stroke-linejoin](/reference/cssproperties/css_prop_stroke-linejoin) - Control corner appearance
- [stroke-opacity](/reference/cssproperties/css_prop_stroke-opacity) - SVG stroke transparency
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
