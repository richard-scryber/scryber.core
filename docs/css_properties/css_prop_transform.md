---
layout: default
title: transform
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# transform : Element Transformation Property

The `transform` property applies 2D or 3D transformations to elements in PDF documents. This property allows you to rotate, scale, translate (move), skew, or apply matrix transformations to both HTML and SVG elements, enabling complex visual effects and layouts.

## Usage

```css
selector {
    transform: function(values);
}
```

The transform property accepts one or more transformation functions, which are applied in the order specified from left to right.

---

## Supported Values

### translate() - Move elements
- `translate(x, y)` - Move element by x and y distances
- `translateX(x)` - Move element horizontally
- `translateY(y)` - Move element vertically

### rotate() - Rotate elements
- `rotate(angle)` - Rotate element by specified angle
- Angles can be specified in degrees (`deg`), radians (`rad`), or turns (`turn`)

### scale() - Resize elements
- `scale(x, y)` - Scale element by x and y factors
- `scale(n)` - Scale uniformly by factor n
- `scaleX(x)` - Scale horizontally
- `scaleY(y)` - Scale vertically

### skew() - Slant elements
- `skew(x-angle, y-angle)` - Skew element along x and y axes
- `skewX(angle)` - Skew horizontally
- `skewY(angle)` - Skew vertically

### matrix() - Direct transformation matrix
- `matrix(a, b, c, d, e, f)` - Apply transformation matrix directly
- Provides direct control over all transformation parameters

### Multiple Transformations
- `transform: translate(50px, 100px) rotate(45deg) scale(1.5);`
- Transformations are applied right-to-left (in reverse order)

---

## Supported Elements

The `transform` property applies to:
- All HTML block elements (`<div>`, `<p>`, `<section>`, etc.)
- HTML inline elements that are `display: block` or `display: inline-block`
- All SVG elements (`<rect>`, `<circle>`, `<path>`, `<g>`, `<text>`, etc.)
- Images (`<img>`)
- Tables and table cells

---

## Notes

- Transformations do not affect document flow (other elements are not repositioned)
- Multiple transformations are applied from right to left in the declaration
- The transform origin (center point) can be controlled with `transform-origin` property
- Default transform origin is the center of the element (`50% 50%`)
- Angles can use `deg` (degrees), `rad` (radians), `turn` (full rotations), or `grad` (gradians)
- Scale values less than 1 shrink the element; greater than 1 enlarge it
- Negative scale values flip the element
- Translations can use any length unit (px, pt, mm, in, %, etc.)
- Percentage translations are relative to the element's own size
- Transform does not affect the element's box model or layout space
- Transformed elements maintain their interactivity and event boundaries
- PDF transformations are rendered as vector operations (perfect quality at any zoom)

---

## Data Binding

The `transform` property can be dynamically controlled through data binding, enabling data-driven rotations, scaling, translations, and complex transformations. This is essential for creating dynamic visualizations, animated elements, and responsive graphics.

### Example 1: Data-driven rotation for gauge needles

```html
<style>
    .gauge-dial { fill: #e5e7eb; }
    .gauge-needle { fill: #ef4444; }
    .gauge-value { text-anchor: middle; dominant-baseline: middle; font-size: 24px; font-weight: bold; }
</style>
<body>
    <svg width="600" height="250" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each gauges}}">
            <g transform="translate({{@index | multiply: 200 | add: 100}}, 125)">
                <circle class="gauge-dial" cx="0" cy="0" r="80"/>
                <path class="gauge-needle"
                      d="M -3 0 L 0 -60 L 3 0 Z"
                      style="transform: rotate({{angle}}deg); transform-origin: center;"/>
                <text class="gauge-value" x="0" y="35">{{value}}</text>
            </g>
        </template>
    </svg>
</body>
```

### Example 2: Dynamic scaling for data visualization

```html
<style>
    .data-icon { fill: #3b82f6; }
    .icon-label { text-anchor: middle; font-size: 12px; }
</style>
<body>
    <svg width="500" height="300" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each dataPoints}}">
            <g transform="translate({{x}}, {{y}})">
                <circle class="data-icon" cx="0" cy="0" r="20"
                        style="transform: scale({{scale}})"/>
                <text class="icon-label" x="0" y="40">{{label}}</text>
            </g>
        </template>
    </svg>
</body>
```

### Example 3: Conditional transforms for interactive states

```html
<style>
    .card { fill: white; stroke: #3b82f6; stroke-width: 2; }
    .card-title { text-anchor: middle; font-size: 14px; font-weight: bold; }
</style>
<body>
    <svg width="600" height="400" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each cards}}">
            <g transform="translate({{x}}, {{y}})">
                <rect class="card" x="-50" y="-70" width="100" height="140" rx="8"
                      style="transform: {{#if selected}}scale(1.1) rotate(5deg){{else}}scale(1){{/if}}; transform-origin: center;"/>
                <text class="card-title" x="0" y="0">{{title}}</text>
            </g>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Basic translation (movement)

```html
<style>
    .box { width: 50px; height: 50px; fill: #3b82f6; }
    .translated { transform: translate(100px, 50px); }
</style>
<body>
    <svg width="300" height="150">
        <rect class="box" x="20" y="20"/>
        <rect class="box translated" x="20" y="20"/>
    </svg>
</body>
```

### Example 2: Rotation examples

```html
<style>
    .rect-base { fill: #10b981; }
    .rotate-15 { transform: rotate(15deg); }
    .rotate-45 { transform: rotate(45deg); }
    .rotate-90 { transform: rotate(90deg); }
</style>
<body>
    <svg width="400" height="150">
        <rect class="rect-base" x="40" y="50" width="60" height="40"/>
        <rect class="rect-base rotate-15" x="130" y="50" width="60" height="40"/>
        <rect class="rect-base rotate-45" x="220" y="50" width="60" height="40"/>
        <rect class="rect-base rotate-90" x="310" y="50" width="60" height="40"/>
    </svg>
</body>
```

### Example 3: Scaling transformations

```html
<style>
    .circle-base { fill: #8b5cf6; opacity: 0.7; }
    .scale-small { transform: scale(0.5); }
    .scale-normal { transform: scale(1); }
    .scale-large { transform: scale(1.5); }
    .scale-x { transform: scaleX(2); }
</style>
<body>
    <svg width="450" height="150">
        <circle class="circle-base scale-small" cx="60" cy="75" r="30"/>
        <circle class="circle-base scale-normal" cx="150" cy="75" r="30"/>
        <circle class="circle-base scale-large" cx="250" cy="75" r="30"/>
        <circle class="circle-base scale-x" cx="380" cy="75" r="30"/>
    </svg>
</body>
```

### Example 4: Skew transformations

```html
<style>
    .skew-box { fill: #f59e0b; stroke: #92400e; stroke-width: 2; }
    .skew-x { transform: skewX(20deg); }
    .skew-y { transform: skewY(20deg); }
    .skew-both { transform: skew(15deg, 10deg); }
</style>
<body>
    <svg width="400" height="250">
        <rect class="skew-box" x="30" y="30" width="80" height="50"/>
        <rect class="skew-box skew-x" x="150" y="30" width="80" height="50"/>
        <rect class="skew-box skew-y" x="30" y="120" width="80" height="50"/>
        <rect class="skew-box skew-both" x="150" y="120" width="80" height="50"/>
    </svg>
</body>
```

### Example 5: Combined transformations

```html
<style>
    .star { fill: #fbbf24; stroke: #d97706; stroke-width: 2; }
    .combo { transform: translate(150px, 0) rotate(30deg) scale(1.3); }
</style>
<body>
    <svg width="400" height="200">
        <polygon class="star" points="75,20 85,60 125,60 95,85 105,125 75,100 45,125 55,85 25,60 65,60"/>
        <polygon class="star combo" points="75,20 85,60 125,60 95,85 105,125 75,100 45,125 55,85 25,60 65,60"/>
    </svg>
</body>
```

### Example 6: Rotating an arrow icon

```html
<style>
    .arrow-right { fill: #3b82f6; }
    .arrow-down { fill: #10b981; transform: rotate(90deg); }
    .arrow-left { fill: #f59e0b; transform: rotate(180deg); }
    .arrow-up { fill: #ef4444; transform: rotate(270deg); }
</style>
<body>
    <svg width="350" height="200">
        <polygon class="arrow-right" points="30,75 60,60 60,70 80,70 80,80 60,80 60,90"
                 transform-origin="55 75"/>
        <polygon class="arrow-down" points="130,75 160,60 160,70 180,70 180,80 160,80 160,90"
                 transform-origin="155 75"/>
        <polygon class="arrow-left" points="230,75 260,60 260,70 280,70 280,80 260,80 260,90"
                 transform-origin="255 75"/>
        <polygon class="arrow-up" points="30,175 60,160 60,170 80,170 80,180 60,180 60,190"
                 transform-origin="55 175"/>
    </svg>
</body>
```

### Example 7: Creating a loading spinner

```html
<style>
    .spinner-1 { fill: #3b82f6; }
    .spinner-2 { fill: #3b82f6; opacity: 0.8; transform: rotate(45deg); }
    .spinner-3 { fill: #3b82f6; opacity: 0.6; transform: rotate(90deg); }
    .spinner-4 { fill: #3b82f6; opacity: 0.4; transform: rotate(135deg); }
    .spinner-5 { fill: #3b82f6; opacity: 0.2; transform: rotate(180deg); }
</style>
<body>
    <svg width="200" height="200">
        <g transform-origin="100 100">
            <rect class="spinner-1" x="95" y="30" width="10" height="25" rx="5"/>
            <rect class="spinner-2" x="95" y="30" width="10" height="25" rx="5"/>
            <rect class="spinner-3" x="95" y="30" width="10" height="25" rx="5"/>
            <rect class="spinner-4" x="95" y="30" width="10" height="25" rx="5"/>
            <rect class="spinner-5" x="95" y="30" width="10" height="25" rx="5"/>
        </g>
    </svg>
</body>
```

### Example 8: Perspective card flip effect

```html
<style>
    .card { fill: white; stroke: #3b82f6; stroke-width: 3; }
    .card-front { }
    .card-tilted { transform: rotate(5deg) translate(20px, 10px); }
    .card-flipped { transform: scaleX(-1); }
</style>
<body>
    <svg width="450" height="200">
        <rect class="card card-front" x="30" y="50" width="100" height="140" rx="10"/>
        <rect class="card card-tilted" x="170" y="50" width="100" height="140" rx="10"/>
        <rect class="card card-flipped" x="310" y="50" width="100" height="140" rx="10"/>
    </svg>
</body>
```

### Example 9: Isometric box projection

```html
<style>
    .iso-top { fill: #93c5fd; }
    .iso-left { fill: #60a5fa; }
    .iso-right { fill: #3b82f6; }
</style>
<body>
    <svg width="300" height="250">
        <g transform="translate(150, 100)">
            <!-- Top face -->
            <polygon class="iso-top" points="0,-50 50,0 0,50 -50,0"
                     transform="rotateX(60deg)"/>
            <!-- Left face -->
            <polygon class="iso-left" points="-50,0 -50,80 0,130 0,50"/>
            <!-- Right face -->
            <polygon class="iso-right" points="50,0 50,80 0,130 0,50"/>
        </g>
    </svg>
</body>
```

### Example 10: Badge notification with scaling

```html
<style>
    .icon { fill: #3b82f6; }
    .badge {
        fill: #ef4444;
        transform: translate(15px, -15px) scale(1);
    }
    .badge-text {
        fill: white;
        font-size: 12px;
        font-weight: bold;
        text-anchor: middle;
        dominant-baseline: middle;
    }
</style>
<body>
    <svg width="150" height="150">
        <circle class="icon" cx="60" cy="75" r="35"/>
        <circle class="badge" cx="60" cy="75" r="12"/>
        <text class="badge-text" x="75" y="60" transform="translate(0, 0)">5</text>
    </svg>
</body>
```

### Example 11: Text rotation for labels

```html
<style>
    .bar { fill: #3b82f6; }
    .label {
        fill: #1f2937;
        font-size: 12px;
        text-anchor: end;
        transform: rotate(-45deg);
    }
</style>
<body>
    <svg width="400" height="300">
        <rect class="bar" x="50" y="80" width="50" height="150"/>
        <rect class="bar" x="130" y="50" width="50" height="180"/>
        <rect class="bar" x="210" y="100" width="50" height="130"/>
        <rect class="bar" x="290" y="30" width="50" height="200"/>

        <text class="label" x="75" y="245" transform-origin="75 245">January</text>
        <text class="label" x="155" y="245" transform-origin="155 245">February</text>
        <text class="label" x="235" y="245" transform-origin="235 245">March</text>
        <text class="label" x="315" y="245" transform-origin="315 245">April</text>
    </svg>
</body>
```

### Example 12: Mirrored reflection effect

```html
<style>
    .shape-original { fill: #8b5cf6; opacity: 1; }
    .shape-mirror { fill: #8b5cf6; opacity: 0.3; transform: scaleY(-1) translateY(-200px); }
</style>
<body>
    <svg width="300" height="300">
        <polygon class="shape-original" points="150,50 200,150 100,150"/>
        <polygon class="shape-mirror" points="150,50 200,150 100,150"/>
    </svg>
</body>
```

### Example 13: Compass rose with rotated markers

```html
<style>
    .compass-circle { fill: none; stroke: #374151; stroke-width: 2; }
    .compass-marker { fill: #ef4444; }
    .compass-label { fill: #1f2937; font-size: 14px; font-weight: bold; text-anchor: middle; }
</style>
<body>
    <svg width="250" height="250">
        <circle class="compass-circle" cx="125" cy="125" r="80"/>

        <!-- North -->
        <polygon class="compass-marker" points="125,45 130,60 120,60"/>
        <text class="compass-label" x="125" y="35">N</text>

        <!-- East -->
        <polygon class="compass-marker" points="125,45 130,60 120,60"
                 transform="rotate(90 125 125)"/>
        <text class="compass-label" x="220" y="130">E</text>

        <!-- South -->
        <polygon class="compass-marker" points="125,45 130,60 120,60"
                 transform="rotate(180 125 125)"/>
        <text class="compass-label" x="125" y="225">S</text>

        <!-- West -->
        <polygon class="compass-marker" points="125,45 130,60 120,60"
                 transform="rotate(270 125 125)"/>
        <text class="compass-label" x="30" y="130">W</text>
    </svg>
</body>
```

### Example 14: Clock face with rotated hour markers

```html
<style>
    .clock-face { fill: white; stroke: #374151; stroke-width: 3; }
    .hour-marker { fill: #1f2937; }
    .clock-hand { fill: #3b82f6; }
</style>
<body>
    <svg width="250" height="250">
        <circle class="clock-face" cx="125" cy="125" r="100"/>

        <!-- Hour markers (12, 3, 6, 9) -->
        <rect class="hour-marker" x="122" y="30" width="6" height="20" rx="3"/>
        <rect class="hour-marker" x="122" y="30" width="6" height="20" rx="3"
              transform="rotate(90 125 125)"/>
        <rect class="hour-marker" x="122" y="30" width="6" height="20" rx="3"
              transform="rotate(180 125 125)"/>
        <rect class="hour-marker" x="122" y="30" width="6" height="20" rx="3"
              transform="rotate(270 125 125)"/>

        <!-- Clock hands -->
        <rect class="clock-hand" x="122" y="65" width="6" height="60" rx="3"
              transform="rotate(45 125 125)"/>
        <rect class="clock-hand" x="123" y="85" width="4" height="40" rx="2"
              transform="rotate(135 125 125)"/>

        <circle fill="#374151" cx="125" cy="125" r="8"/>
    </svg>
</body>
```

### Example 15: Matrix transformation for complex effects

```html
<style>
    .original { fill: #3b82f6; opacity: 0.5; }
    .matrix-transformed {
        fill: #10b981;
        opacity: 0.7;
        /* matrix(scaleX, skewY, skewX, scaleY, translateX, translateY) */
        transform: matrix(1.5, 0.2, 0.3, 1.2, 50, 20);
    }
</style>
<body>
    <svg width="350" height="250">
        <rect class="original" x="50" y="50" width="80" height="80"/>
        <rect class="matrix-transformed" x="50" y="50" width="80" height="80"/>
    </svg>
</body>
```

---

## See Also

- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [opacity](/reference/cssproperties/css_prop_opacity) - Element transparency
- [position](/reference/cssproperties/css_prop_position) - Element positioning
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
