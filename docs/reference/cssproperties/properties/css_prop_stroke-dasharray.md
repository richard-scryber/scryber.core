---
layout: default
title: stroke-dasharray
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# stroke-dasharray : SVG Dashed Line Pattern Property

The `stroke-dasharray` property defines the pattern of dashes and gaps used to stroke paths and shapes in SVG elements. This property is essential for creating dashed lines, dotted borders, and custom dash patterns in PDF documents.

## Usage

```css
selector {
    stroke-dasharray: value;
}
```

The stroke-dasharray property accepts a space-separated or comma-separated list of numbers that define the lengths of alternating dashes and gaps. The pattern repeats along the length of the path.

---

## Supported Values

### Single Value
- `stroke-dasharray: 5` - Creates dashes and gaps of equal length (5pt dash, 5pt gap)

### Two Values
- `stroke-dasharray: 5 2` - Creates 5pt dashes with 2pt gaps
- `stroke-dasharray: 10 3` - Creates 10pt dashes with 3pt gaps

### Multiple Values
- `stroke-dasharray: 5 2 1 2` - Creates a pattern of 5pt dash, 2pt gap, 1pt dash, 2pt gap
- `stroke-dasharray: 10 5 2 5` - Complex repeating pattern

### Special Keywords
- `none` - Solid line (no dashes, default)
- `0` - Equivalent to `none`

### Units
Values can be specified in various units:
- Points: `5` or `5pt`
- Pixels: `5px`
- Millimeters: `5mm`
- Inches: `0.1in`

---

## Supported Elements

The `stroke-dasharray` property applies to SVG elements including:
- `<rect>` rectangles
- `<circle>` circles
- `<ellipse>` ellipses
- `<polygon>` polygons
- `<polyline>` polylines
- `<line>` lines
- `<path>` paths
- `<text>` SVG text elements (outline)
- All other SVG shape elements with strokes

---

## Notes

- The pattern repeats cyclically along the entire length of the path
- If an odd number of values is provided, the list is repeated to yield an even number
  - Example: `5 2 3` becomes `5 2 3 5 2 3` when repeated
- Works in combination with `stroke-dashoffset` for animated dash effects
- Values are measured along the path, not in screen coordinates
- Zero-length dashes and gaps are valid and create specific visual effects
- Use with `stroke-linecap` to control the appearance of dash ends
- Default value is `none` (solid stroke)
- Vector dashes maintain quality at any zoom level in PDF viewers
- Useful for creating separators, borders, measurement lines, and focus indicators
- Pattern spacing is affected by the `stroke-width` when using rounded line caps

---

## Data Binding

The `stroke-dasharray` property can be dynamically controlled through data binding, enabling responsive dash patterns based on data values. This is particularly useful for creating data-driven visualizations, animated progress indicators, and dynamic state representations.

### Example 1: Data-driven dash patterns for chart lines

```html
<style>
    .data-line {
        stroke-width: 3;
        fill: none;
    }
</style>
<body>
    <svg width="400" height="250" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each datasets}}">
            <polyline class="data-line"
                      points="{{points}}"
                      stroke="{{color}}"
                      style="stroke-dasharray: {{dashPattern}}"/>
        </template>
    </svg>
</body>
```

### Example 2: Progress indicator with dynamic dashes

```html
<style>
    .progress-ring {
        fill: none;
        stroke-width: 8;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="200" height="200" xmlns="http://www.w3.org/2000/svg">
        <circle class="progress-ring"
                cx="100" cy="100" r="80"
                stroke="#e5e7eb"/>
        <circle class="progress-ring"
                cx="100" cy="100" r="80"
                stroke="#3b82f6"
                style="stroke-dasharray: {{circumference}}; stroke-dashoffset: {{offset}}"
                transform="rotate(-90 100 100)"/>
    </svg>
</body>
```

### Example 3: Conditional dash styles based on status

```html
<style>
    .status-line {
        stroke-width: 4;
    }
</style>
<body>
    <svg width="400" height="200" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each connections}}">
            <line class="status-line"
                  x1="{{x1}}" y1="{{y1}}"
                  x2="{{x2}}" y2="{{y2}}"
                  stroke="{{statusColor}}"
                  style="stroke-dasharray: {{#if active}}none{{else}}5 3{{/if}}"/>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Basic dashed line

```html
<style>
    .dashed-line {
        stroke: black;
        stroke-width: 2;
        stroke-dasharray: 5 2;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="dashed-line" x1="10" y1="50" x2="290" y2="50"/>
    </svg>
</body>
```

### Example 2: Dotted line pattern

```html
<style>
    .dotted-line {
        stroke: #3b82f6;
        stroke-width: 3;
        stroke-dasharray: 1 3;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="dotted-line" x1="10" y1="50" x2="290" y2="50"/>
    </svg>
</body>
```

### Example 3: Dashed rectangle border

```html
<style>
    .dashed-rect {
        fill: lightblue;
        stroke: darkblue;
        stroke-width: 2;
        stroke-dasharray: 10 5;
    }
</style>
<body>
    <svg width="250" height="150">
        <rect class="dashed-rect" x="25" y="25" width="200" height="100"/>
    </svg>
</body>
```

### Example 4: Multiple dash patterns

```html
<style>
    .pattern-1 { stroke: black; stroke-width: 2; stroke-dasharray: 5 5; }
    .pattern-2 { stroke: black; stroke-width: 2; stroke-dasharray: 10 2; }
    .pattern-3 { stroke: black; stroke-width: 2; stroke-dasharray: 15 3 3 3; }
    .pattern-4 { stroke: black; stroke-width: 2; stroke-dasharray: 20 5 5 5; }
</style>
<body>
    <svg width="300" height="200">
        <line class="pattern-1" x1="10" y1="30" x2="290" y2="30"/>
        <line class="pattern-2" x1="10" y1="70" x2="290" y2="70"/>
        <line class="pattern-3" x1="10" y1="110" x2="290" y2="110"/>
        <line class="pattern-4" x1="10" y1="150" x2="290" y2="150"/>
    </svg>
</body>
```

### Example 5: Dashed circle

```html
<style>
    .dashed-circle {
        fill: none;
        stroke: #ef4444;
        stroke-width: 3;
        stroke-dasharray: 8 4;
    }
</style>
<body>
    <svg width="200" height="200">
        <circle class="dashed-circle" cx="100" cy="100" r="80"/>
    </svg>
</body>
```

### Example 6: Measurement line with long dashes

```html
<style>
    .measure-line {
        stroke: #6b7280;
        stroke-width: 1.5;
        stroke-dasharray: 15 5;
    }
    .endpoint {
        fill: #6b7280;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="measure-line" x1="50" y1="50" x2="250" y2="50"/>
        <circle class="endpoint" cx="50" cy="50" r="4"/>
        <circle class="endpoint" cx="250" cy="50" r="4"/>
    </svg>
</body>
```

### Example 7: Form input with dashed border

```html
<style>
    .input-field {
        fill: white;
        stroke: #9ca3af;
        stroke-width: 2;
        stroke-dasharray: 5 3;
    }
</style>
<body>
    <svg width="300" height="100">
        <rect class="input-field" x="25" y="25" width="250" height="50" rx="5"/>
    </svg>
</body>
```

### Example 8: Railway track pattern

```html
<style>
    .rail {
        stroke: #374151;
        stroke-width: 3;
    }
    .ties {
        stroke: #6b7280;
        stroke-width: 2;
        stroke-dasharray: 2 15;
        stroke-linecap: square;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="rail" x1="10" y1="35" x2="290" y2="35"/>
        <line class="rail" x1="10" y1="65" x2="290" y2="65"/>
        <line class="ties" x1="10" y1="30" x2="290" y2="70" stroke-dasharray="2 12"/>
    </svg>
</body>
```

### Example 9: Focus indicator box

```html
<style>
    .focus-box {
        fill: none;
        stroke: #3b82f6;
        stroke-width: 2;
        stroke-dasharray: 4 2;
    }
</style>
<body>
    <svg width="250" height="150">
        <rect class="focus-box" x="25" y="25" width="200" height="100" rx="8"/>
    </svg>
</body>
```

### Example 10: Coupon cutout border

```html
<style>
    .coupon-border {
        fill: #fef3c7;
        stroke: #f59e0b;
        stroke-width: 2;
        stroke-dasharray: 3 3;
    }
</style>
<body>
    <svg width="300" height="150">
        <rect class="coupon-border" x="25" y="25" width="250" height="100" rx="5"/>
    </svg>
</body>
```

### Example 11: Morse code pattern

```html
<style>
    .morse-code {
        stroke: #1f2937;
        stroke-width: 4;
        stroke-dasharray: 15 5 5 5 15 5 5 5 15 5;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="morse-code" x1="10" y1="50" x2="290" y2="50"/>
    </svg>
</body>
```

### Example 12: Path separator

```html
<style>
    .separator {
        stroke: #d1d5db;
        stroke-width: 2;
        stroke-dasharray: 6 4;
    }
</style>
<body>
    <svg width="300" height="200">
        <line class="separator" x1="10" y1="50" x2="290" y2="50"/>
        <line class="separator" x1="10" y1="100" x2="290" y2="100"/>
        <line class="separator" x1="10" y1="150" x2="290" y2="150"/>
    </svg>
</body>
```

### Example 13: Zigzag pattern using polyline

```html
<style>
    .zigzag {
        fill: none;
        stroke: #8b5cf6;
        stroke-width: 2;
        stroke-dasharray: 10 10;
    }
</style>
<body>
    <svg width="300" height="150">
        <polyline class="zigzag" points="10,75 50,25 90,75 130,25 170,75 210,25 250,75 290,25"/>
    </svg>
</body>
```

### Example 14: Construction zone border

```html
<style>
    .construction {
        fill: #fef3c7;
        stroke: #eab308;
        stroke-width: 4;
        stroke-dasharray: 20 10;
    }
</style>
<body>
    <svg width="300" height="150">
        <rect class="construction" x="25" y="25" width="250" height="100"/>
    </svg>
</body>
```

### Example 15: UI element states

```html
<style>
    .button-default {
        fill: white;
        stroke: #3b82f6;
        stroke-width: 2;
    }
    .button-hover {
        fill: #eff6ff;
        stroke: #3b82f6;
        stroke-width: 2;
        stroke-dasharray: 5 2;
    }
    .button-disabled {
        fill: #f3f4f6;
        stroke: #9ca3af;
        stroke-width: 2;
        stroke-dasharray: 3 3;
    }
</style>
<body>
    <svg width="300" height="250">
        <rect class="button-default" x="50" y="25" width="200" height="50" rx="8"/>
        <rect class="button-hover" x="50" y="100" width="200" height="50" rx="8"/>
        <rect class="button-disabled" x="50" y="175" width="200" height="50" rx="8"/>
    </svg>
</body>
```

---

## See Also

- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [stroke-width](/reference/cssproperties/css_prop_stroke-width) - Control stroke thickness
- [stroke-dashoffset](/reference/cssproperties/css_prop_stroke-dashoffset) - Offset dash pattern for animation
- [stroke-linecap](/reference/cssproperties/css_prop_stroke-linecap) - Control dash end appearance
- [stroke-opacity](/reference/cssproperties/css_prop_stroke-opacity) - SVG stroke transparency
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
