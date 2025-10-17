---
layout: default
title: paint-order
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# paint-order : SVG Paint Layering Property

The `paint-order` property specifies the order in which fill, stroke, and markers are painted on SVG elements. This property controls the layering of these paint operations, allowing you to create effects like outlined text, bordered shapes, and customized rendering orders.

## Usage

```css
selector {
    paint-order: value;
}
```

The paint-order property accepts keywords that specify the painting sequence of fill, stroke, and markers.

---

## Supported Values

### Keywords

- `normal` - Default value. Paint in the order: fill, then stroke, then markers

- `fill` - Paint fill first, followed by stroke and markers

- `stroke` - Paint stroke first, followed by fill and markers

- `fill stroke` - Paint fill first, then stroke (markers last)

- `stroke fill` - Paint stroke first, then fill (markers last)

- `fill stroke markers` - Explicit order: fill, stroke, then markers

- `stroke fill markers` - Explicit order: stroke, fill, then markers

- `markers fill stroke` - Explicit order: markers, fill, then stroke

- `markers stroke fill` - Explicit order: markers, stroke, then fill

- `stroke markers fill` - Explicit order: stroke, markers, then fill

- `fill markers stroke` - Explicit order: fill, markers, then stroke

Note: Any combination of `fill`, `stroke`, and `markers` can be specified in any order.

---

## Supported Elements

The `paint-order` property applies to SVG elements including:
- `<text>` SVG text elements
- `<tspan>` text spans
- `<rect>` rectangles
- `<circle>` circles
- `<ellipse>` ellipses
- `<line>` lines
- `<polyline>` polylines
- `<polygon>` polygons
- `<path>` paths
- All other SVG shape and text elements

---

## Notes

- Default value is `normal` (fill, stroke, markers order)
- Most commonly used to achieve stroke effects on text
- `paint-order: stroke fill` creates text with visible stroke outlines
- Particularly useful for creating outlined text effects
- Does not affect HTML elements, only SVG elements
- Markers refer to SVG marker elements (arrowheads, bullets, etc.)
- If only one or two values are specified, the remaining are painted in default order
- Essential for creating legible text on complex backgrounds
- Can create neon-like effects with appropriate colors
- Vector paint order maintains quality at any zoom level in PDF viewers

---

## Data Binding

The `paint-order` property can be dynamically controlled through data binding, enabling conditional rendering orders based on data states. This is particularly useful for creating dynamic text effects, state-based styling, and adaptive graphics.

### Example 1: Conditional outlined text based on emphasis level

```html
<style>
    .heading-text {
        font-size: 36px;
        font-weight: bold;
        text-anchor: middle;
    }
</style>
<body>
    <svg width="500" height="300" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each headings}}">
            <text class="heading-text"
                  x="250"
                  y="{{@index | multiply: 70 | add: 60}}"
                  fill="{{textColor}}"
                  stroke="{{outlineColor}}"
                  stroke-width="{{outlineWidth}}"
                  style="paint-order: {{#if emphasized}}stroke fill{{else}}normal{{/if}}">{{title}}</text>
        </template>
    </svg>
</body>
```

### Example 2: Data-driven label styles for status badges

```html
<style>
    .badge-bg { stroke-width: 2; }
    .badge-label { font-size: 14px; font-weight: bold; text-anchor: middle; dominant-baseline: middle; }
</style>
<body>
    <svg width="600" height="200" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each badges}}">
            <g transform="translate({{@index | multiply: 150 | add: 75}}, 100)">
                <rect class="badge-bg" x="-60" y="-30" width="120" height="60" rx="10"
                      fill="{{backgroundColor}}" stroke="{{borderColor}}"/>
                <text class="badge-label"
                      x="0" y="0"
                      fill="{{labelColor}}"
                      stroke="{{#if highlight}}{{strokeColor}}{{else}}none{{/if}}"
                      stroke-width="3"
                      style="paint-order: {{#if highlight}}stroke fill{{else}}normal{{/if}}">{{status}}</text>
            </g>
        </template>
    </svg>
</body>
```

### Example 3: Dynamic signage with variable paint order

```html
<style>
    .sign-text {
        font-size: 42px;
        font-weight: bold;
        text-anchor: middle;
    }
</style>
<body>
    <svg width="500" height="350" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each signs}}">
            <g transform="translate(250, {{@index | multiply: 100 | add: 70}})">
                <rect x="-200" y="-40" width="400" height="80" rx="10"
                      fill="{{signBackground}}" stroke="{{signBorder}}" stroke-width="4"/>
                <text class="sign-text"
                      x="0" y="10"
                      fill="{{textFill}}"
                      stroke="{{textStroke}}"
                      stroke-width="{{strokeWidth}}"
                      style="paint-order: {{paintOrder}}">{{message}}</text>
            </g>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Text with stroke outline (stroke painted first)

```html
<style>
    .outlined-text {
        fill: white;
        stroke: #1f2937;
        stroke-width: 3;
        paint-order: stroke fill;
        font-size: 48px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="400" height="100">
        <text class="outlined-text" x="20" y="60">OUTLINED</text>
    </svg>
</body>
```

### Example 2: Comparing normal vs stroke-first paint order

```html
<style>
    .text-base {
        fill: #3b82f6;
        stroke: #1e40af;
        stroke-width: 4;
        font-size: 36px;
        font-weight: bold;
    }
    .normal-order { paint-order: normal; }
    .stroke-first { paint-order: stroke fill; }
</style>
<body>
    <svg width="400" height="200">
        <text class="text-base normal-order" x="20" y="60">Normal Order</text>
        <text class="text-base stroke-first" x="20" y="140">Stroke First</text>
    </svg>
</body>
```

### Example 3: Neon text effect

```html
<style>
    .neon-text {
        fill: #fbbf24;
        stroke: #f59e0b;
        stroke-width: 2;
        paint-order: stroke fill;
        font-size: 42px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="350" height="100">
        <rect width="350" height="100" fill="#1f2937"/>
        <text class="neon-text" x="20" y="65">NEON GLOW</text>
    </svg>
</body>
```

### Example 4: Bold outlined headline

```html
<style>
    .headline {
        fill: #ef4444;
        stroke: white;
        stroke-width: 6;
        paint-order: stroke fill;
        font-size: 56px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="450" height="120">
        <rect width="450" height="120" fill="#1e40af"/>
        <text class="headline" x="20" y="75">HEADLINE</text>
    </svg>
</body>
```

### Example 5: Shape with border effect using paint-order

```html
<style>
    .shape-normal {
        fill: #10b981;
        stroke: white;
        stroke-width: 6;
        paint-order: normal;
    }
    .shape-stroke-first {
        fill: #10b981;
        stroke: white;
        stroke-width: 6;
        paint-order: stroke fill;
    }
</style>
<body>
    <svg width="350" height="200">
        <rect class="shape-normal" x="30" y="50" width="120" height="100"/>
        <rect class="shape-stroke-first" x="190" y="50" width="120" height="100"/>
    </svg>
</body>
```

### Example 6: Comic book style text

```html
<style>
    .comic-text {
        fill: #fbbf24;
        stroke: #1f2937;
        stroke-width: 5;
        paint-order: stroke fill;
        font-size: 52px;
        font-weight: bold;
        font-family: Impact, sans-serif;
    }
</style>
<body>
    <svg width="400" height="120">
        <text class="comic-text" x="20" y="70">BANG!</text>
    </svg>
</body>
```

### Example 7: Logo text with double outline

```html
<style>
    .logo-outer {
        fill: none;
        stroke: #1f2937;
        stroke-width: 10;
        paint-order: stroke;
        font-size: 48px;
        font-weight: bold;
    }
    .logo-inner {
        fill: white;
        stroke: #3b82f6;
        stroke-width: 4;
        paint-order: stroke fill;
        font-size: 48px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="400" height="100">
        <text class="logo-outer" x="50" y="60">LOGO</text>
        <text class="logo-inner" x="50" y="60">LOGO</text>
    </svg>
</body>
```

### Example 8: Sports jersey numbers

```html
<style>
    .jersey-number {
        fill: white;
        stroke: #1e40af;
        stroke-width: 8;
        paint-order: stroke fill;
        font-size: 72px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="400" height="150">
        <rect fill="#3b82f6" width="400" height="150"/>
        <text class="jersey-number" x="150" y="100">23</text>
    </svg>
</body>
```

### Example 9: Warning sign text

```html
<style>
    .warning-text {
        fill: #1f2937;
        stroke: #fbbf24;
        stroke-width: 3;
        paint-order: stroke fill;
        font-size: 32px;
        font-weight: bold;
        text-anchor: middle;
    }
    .warning-bg {
        fill: #fef3c7;
        stroke: #1f2937;
        stroke-width: 4;
    }
</style>
<body>
    <svg width="300" height="200">
        <polygon class="warning-bg" points="150,20 280,170 20,170"/>
        <text class="warning-text" x="150" y="130">!</text>
        <text class="warning-text" x="150" y="165" font-size="20">CAUTION</text>
    </svg>
</body>
```

### Example 10: Retro gaming style text

```html
<style>
    .retro-text {
        fill: #10b981;
        stroke: #065f46;
        stroke-width: 4;
        paint-order: stroke fill;
        font-size: 40px;
        font-weight: bold;
        font-family: monospace;
    }
</style>
<body>
    <svg width="400" height="100">
        <rect fill="#000000" width="400" height="100"/>
        <text class="retro-text" x="20" y="60">GAME OVER</text>
    </svg>
</body>
```

### Example 11: Embossed text effect

```html
<style>
    .embossed-shadow {
        fill: none;
        stroke: #9ca3af;
        stroke-width: 2;
        paint-order: stroke;
        font-size: 48px;
        font-weight: bold;
    }
    .embossed-main {
        fill: #e5e7eb;
        stroke: white;
        stroke-width: 1;
        paint-order: stroke fill;
        font-size: 48px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="400" height="100">
        <rect fill="#d1d5db" width="400" height="100"/>
        <text class="embossed-shadow" x="21" y="61">EMBOSS</text>
        <text class="embossed-main" x="20" y="60">EMBOSS</text>
    </svg>
</body>
```

### Example 12: High visibility text for safety

```html
<style>
    .safety-text {
        fill: #fbbf24;
        stroke: #1f2937;
        stroke-width: 6;
        paint-order: stroke fill;
        font-size: 36px;
        font-weight: bold;
        text-anchor: middle;
    }
</style>
<body>
    <svg width="400" height="250">
        <rect fill="#ef4444" width="400" height="250"/>
        <text class="safety-text" x="200" y="80">DANGER</text>
        <text class="safety-text" x="200" y="140" font-size="24">HIGH VOLTAGE</text>
        <text class="safety-text" x="200" y="180" font-size="24">KEEP OUT</text>
    </svg>
</body>
```

### Example 13: Movie title style

```html
<style>
    .movie-title {
        fill: #dc2626;
        stroke: #fbbf24;
        stroke-width: 3;
        paint-order: stroke fill;
        font-size: 56px;
        font-weight: bold;
        text-anchor: middle;
    }
    .movie-subtitle {
        fill: white;
        stroke: #1f2937;
        stroke-width: 2;
        paint-order: stroke fill;
        font-size: 20px;
        text-anchor: middle;
    }
</style>
<body>
    <svg width="500" height="150">
        <rect fill="#000000" width="500" height="150"/>
        <text class="movie-title" x="250" y="70">ACTION</text>
        <text class="movie-subtitle" x="250" y="110">THE MOVIE</text>
    </svg>
</body>
```

### Example 14: Label tags with outlined text

```html
<style>
    .tag {
        fill: #3b82f6;
        stroke: #1e40af;
        stroke-width: 2;
    }
    .tag-text {
        fill: white;
        stroke: #1e40af;
        stroke-width: 2;
        paint-order: stroke fill;
        font-size: 14px;
        font-weight: bold;
        text-anchor: middle;
        dominant-baseline: middle;
    }
</style>
<body>
    <svg width="400" height="100">
        <rect class="tag" x="20" y="30" width="100" height="40" rx="8"/>
        <text class="tag-text" x="70" y="50">NEW</text>

        <rect class="tag" x="140" y="30" width="100" height="40" rx="8"/>
        <text class="tag-text" x="190" y="50">SALE</text>

        <rect class="tag" x="260" y="30" width="120" height="40" rx="8"/>
        <text class="tag-text" x="320" y="50">LIMITED</text>
    </svg>
</body>
```

### Example 15: Banner with outlined text

```html
<style>
    .banner-bg {
        fill: #10b981;
        stroke: #065f46;
        stroke-width: 4;
    }
    .banner-text {
        fill: white;
        stroke: #065f46;
        stroke-width: 4;
        paint-order: stroke fill;
        font-size: 32px;
        font-weight: bold;
        text-anchor: middle;
        dominant-baseline: middle;
    }
</style>
<body>
    <svg width="450" height="120">
        <rect class="banner-bg" x="25" y="20" width="400" height="80" rx="10"/>
        <text class="banner-text" x="225" y="60">SPECIAL OFFER</text>
    </svg>
</body>
```

---

## See Also

- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [stroke-width](/reference/cssproperties/css_prop_stroke-width) - Control stroke thickness
- [fill-opacity](/reference/cssproperties/css_prop_fill-opacity) - Fill transparency
- [stroke-opacity](/reference/cssproperties/css_prop_stroke-opacity) - SVG stroke transparency
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
