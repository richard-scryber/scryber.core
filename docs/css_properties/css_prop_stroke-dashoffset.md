---
layout: default
title: stroke-dashoffset
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# stroke-dashoffset : SVG Dash Pattern Offset Property

The `stroke-dashoffset` property specifies the distance into the dash pattern to start the dash. This property is commonly used in combination with `stroke-dasharray` to create animated drawing effects or to adjust the position of dashed patterns along a path.

## Usage

```css
selector {
    stroke-dashoffset: value;
}
```

The stroke-dashoffset property accepts a numeric value that represents the offset distance. A positive value shifts the pattern forward along the path, while a negative value shifts it backward.

---

## Supported Values

### Numeric Values
- `stroke-dashoffset: 0` - No offset (default)
- `stroke-dashoffset: 10` - Offset pattern by 10 units forward
- `stroke-dashoffset: -5` - Offset pattern by 5 units backward
- `stroke-dashoffset: 50` - Larger offset for longer paths

### Units
Values can be specified in various units:
- Points: `10` or `10pt`
- Pixels: `10px`
- Millimeters: `10mm`
- Inches: `0.2in`
- Percentages: Not typically supported

---

## Supported Elements

The `stroke-dashoffset` property applies to SVG elements including:
- `<rect>` rectangles
- `<circle>` circles
- `<ellipse>` ellipses
- `<polygon>` polygons
- `<polyline>` polylines
- `<line>` lines
- `<path>` paths
- `<text>` SVG text elements (outline)
- All other SVG shape elements with dashed strokes

---

## Notes

- Must be used with `stroke-dasharray` to have a visible effect
- Positive values move the dash pattern start point forward along the path
- Negative values move the dash pattern start point backward along the path
- Commonly used for creating "drawing" animations by animating from path length to 0
- The offset wraps around for values larger than the pattern length
- Default value is `0` (no offset)
- Particularly useful for progress indicators and loading animations
- Can be animated in SVG animations (though PDF is typically static)
- Offset is measured in the same units as the path
- Works with any dash pattern complexity
- Essential for revealing/hiding path animations

---

## Data Binding

The `stroke-dashoffset` property can be dynamically bound to data values, enabling data-driven progress indicators, gauges, and animated transitions. This is essential for creating responsive visualizations that reflect changing data states.

### Example 1: Dynamic progress circles from data

```html
<style>
    .progress-bg { fill: none; stroke: #e5e7eb; stroke-width: 10; }
    .progress-value { fill: none; stroke-width: 10; stroke-linecap: round; }
</style>
<body>
    <svg width="600" height="200" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each metrics}}">
            <g transform="translate({{@index | multiply: 150 | add: 100}}, 100)">
                <circle class="progress-bg" cx="0" cy="0" r="40"/>
                <circle class="progress-value" cx="0" cy="0" r="40"
                        stroke="{{color}}"
                        style="stroke-dasharray: 251.2; stroke-dashoffset: {{offset}}"
                        transform="rotate(-90)"/>
                <text text-anchor="middle" y="5" font-size="18" font-weight="bold">{{value}}%</text>
            </g>
        </template>
    </svg>
</body>
```

### Example 2: Data-driven gauge meter

```html
<style>
    .gauge-track { fill: none; stroke: #e5e7eb; stroke-width: 12; }
    .gauge-indicator { fill: none; stroke-width: 12; stroke-linecap: round; }
</style>
<body>
    <svg width="250" height="180" xmlns="http://www.w3.org/2000/svg">
        <path class="gauge-track" d="M 40 140 A 60 60 0 1 1 210 140"/>
        <path class="gauge-indicator"
              stroke="{{gaugeColor}}"
              style="stroke-dasharray: 188.4; stroke-dashoffset: {{dashOffset}}"
              d="M 40 140 A 60 60 0 1 1 210 140"/>
        <text x="125" y="120" text-anchor="middle" font-size="32" font-weight="bold">{{currentValue}}</text>
        <text x="125" y="145" text-anchor="middle" font-size="14" fill="#6b7280">{{unit}}</text>
    </svg>
</body>
```

### Example 3: Skill level bars with data binding

```html
<style>
    .skill-track { fill: none; stroke: #f3f4f6; stroke-width: 10; stroke-linecap: round; }
    .skill-level { fill: none; stroke-width: 10; stroke-linecap: round; }
</style>
<body>
    <svg width="400" height="300" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each skills}}">
            <g transform="translate(0, {{@index | multiply: 60}})">
                <text x="10" y="30" font-size="14" fill="#374151">{{name}}</text>
                <line class="skill-track" x1="120" y1="25" x2="370" y2="25"/>
                <line class="skill-level" x1="120" y1="25" x2="370" y2="25"
                      stroke="{{levelColor}}"
                      style="stroke-dasharray: 250; stroke-dashoffset: {{250 | subtract: level | multiply: 2.5}}"/>
            </g>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Basic dash offset

```html
<style>
    .offset-line {
        stroke: #3b82f6;
        stroke-width: 3;
        stroke-dasharray: 10 5;
        stroke-dashoffset: 0;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="offset-line" x1="10" y1="50" x2="290" y2="50"/>
    </svg>
</body>
```

### Example 2: Offset comparison

```html
<style>
    .dash-base { stroke: black; stroke-width: 2; stroke-dasharray: 15 5; }
    .offset-0 { stroke-dashoffset: 0; }
    .offset-5 { stroke-dashoffset: 5; }
    .offset-10 { stroke-dashoffset: 10; }
    .offset-15 { stroke-dashoffset: 15; }
</style>
<body>
    <svg width="300" height="250">
        <line class="dash-base offset-0" x1="10" y1="40" x2="290" y2="40"/>
        <line class="dash-base offset-5" x1="10" y1="90" x2="290" y2="90"/>
        <line class="dash-base offset-10" x1="10" y1="140" x2="290" y2="140"/>
        <line class="dash-base offset-15" x1="10" y1="190" x2="290" y2="190"/>
    </svg>
</body>
```

### Example 3: Progress indicator states

```html
<style>
    .progress-circle {
        fill: none;
        stroke: #e5e7eb;
        stroke-width: 8;
    }
    .progress-fill {
        fill: none;
        stroke: #10b981;
        stroke-width: 8;
        stroke-dasharray: 251.2;  /* 2 * π * 40 */
        stroke-dashoffset: 188.4;  /* 25% progress */
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="200" height="200">
        <circle class="progress-circle" cx="100" cy="100" r="40"/>
        <circle class="progress-fill" cx="100" cy="100" r="40" transform="rotate(-90 100 100)"/>
    </svg>
</body>
```

### Example 4: Marching ants effect simulation

```html
<style>
    .marching-ants {
        fill: none;
        stroke: black;
        stroke-width: 2;
        stroke-dasharray: 5 5;
        stroke-dashoffset: 5;
    }
</style>
<body>
    <svg width="300" height="200">
        <rect class="marching-ants" x="50" y="50" width="200" height="100"/>
    </svg>
</body>
```

### Example 5: Loading spinner segments

```html
<style>
    .spinner-track {
        fill: none;
        stroke: #e5e7eb;
        stroke-width: 6;
    }
    .spinner-segment {
        fill: none;
        stroke: #3b82f6;
        stroke-width: 6;
        stroke-dasharray: 80 200;
        stroke-dashoffset: 0;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="150" height="150">
        <circle class="spinner-track" cx="75" cy="75" r="45"/>
        <circle class="spinner-segment" cx="75" cy="75" r="45" transform="rotate(-90 75 75)"/>
    </svg>
</body>
```

### Example 6: Path reveal effect start state

```html
<style>
    .reveal-path {
        fill: none;
        stroke: #8b5cf6;
        stroke-width: 3;
        stroke-dasharray: 500;
        stroke-dashoffset: 500;  /* Hidden state */
    }
</style>
<body>
    <svg width="300" height="200">
        <path class="reveal-path" d="M 50 100 Q 150 50 250 100 T 450 100"/>
    </svg>
</body>
```

### Example 7: Circular progress variants

```html
<style>
    .progress-bg { fill: none; stroke: #f3f4f6; stroke-width: 10; }
    .progress-0 { fill: none; stroke: #ef4444; stroke-width: 10;
                  stroke-dasharray: 188.4; stroke-dashoffset: 188.4; }
    .progress-50 { fill: none; stroke: #f59e0b; stroke-width: 10;
                   stroke-dasharray: 188.4; stroke-dashoffset: 94.2; }
    .progress-100 { fill: none; stroke: #10b981; stroke-width: 10;
                    stroke-dasharray: 188.4; stroke-dashoffset: 0; }
</style>
<body>
    <svg width="400" height="150">
        <circle class="progress-bg" cx="75" cy="75" r="30"/>
        <circle class="progress-0" cx="75" cy="75" r="30" transform="rotate(-90 75 75)"/>

        <circle class="progress-bg" cx="200" cy="75" r="30"/>
        <circle class="progress-50" cx="200" cy="75" r="30" transform="rotate(-90 200 75)"/>

        <circle class="progress-bg" cx="325" cy="75" r="30"/>
        <circle class="progress-100" cx="325" cy="75" r="30" transform="rotate(-90 325 75)"/>
    </svg>
</body>
```

### Example 8: Gauge meter

```html
<style>
    .gauge-bg {
        fill: none;
        stroke: #e5e7eb;
        stroke-width: 12;
    }
    .gauge-fill {
        fill: none;
        stroke: #3b82f6;
        stroke-width: 12;
        stroke-dasharray: 235.5;  /* 3/4 of circle */
        stroke-dashoffset: 100;   /* Current value */
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="200" height="150">
        <path class="gauge-bg" d="M 40 120 A 60 60 0 1 1 160 120" />
        <path class="gauge-fill" d="M 40 120 A 60 60 0 1 1 160 120" />
    </svg>
</body>
```

### Example 9: Download progress bar

```html
<style>
    .download-track {
        fill: none;
        stroke: #e5e7eb;
        stroke-width: 15;
        stroke-linecap: round;
    }
    .download-progress {
        fill: none;
        stroke: #10b981;
        stroke-width: 15;
        stroke-dasharray: 260;
        stroke-dashoffset: 130;  /* 50% complete */
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="download-track" x1="20" y1="50" x2="280" y2="50"/>
        <line class="download-progress" x1="20" y1="50" x2="280" y2="50"/>
    </svg>
</body>
```

### Example 10: Battery level indicator

```html
<style>
    .battery-shell {
        fill: none;
        stroke: #374151;
        stroke-width: 2;
    }
    .battery-level {
        fill: none;
        stroke: #10b981;
        stroke-width: 8;
        stroke-dasharray: 60;
        stroke-dashoffset: 15;  /* 75% charged */
        stroke-linecap: butt;
    }
</style>
<body>
    <svg width="150" height="100">
        <rect class="battery-shell" x="20" y="30" width="80" height="40" rx="3"/>
        <rect x="100" y="42" width="8" height="16" fill="#374151"/>
        <line class="battery-level" x1="30" y1="50" x2="90" y2="50"/>
    </svg>
</body>
```

### Example 11: Skill level bar

```html
<style>
    .skill-track {
        fill: none;
        stroke: #f3f4f6;
        stroke-width: 10;
        stroke-linecap: round;
    }
    .skill-beginner {
        fill: none;
        stroke: #ef4444;
        stroke-width: 10;
        stroke-dasharray: 200;
        stroke-dashoffset: 150;  /* 25% */
        stroke-linecap: round;
    }
    .skill-intermediate {
        fill: none;
        stroke: #f59e0b;
        stroke-width: 10;
        stroke-dasharray: 200;
        stroke-dashoffset: 80;  /* 60% */
        stroke-linecap: round;
    }
    .skill-expert {
        fill: none;
        stroke: #10b981;
        stroke-width: 10;
        stroke-dasharray: 200;
        stroke-dashoffset: 10;  /* 95% */
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="250" height="200">
        <line class="skill-track" x1="25" y1="40" x2="225" y2="40"/>
        <line class="skill-beginner" x1="25" y1="40" x2="225" y2="40"/>

        <line class="skill-track" x1="25" y1="100" x2="225" y2="100"/>
        <line class="skill-intermediate" x1="25" y1="100" x2="225" y2="100"/>

        <line class="skill-track" x1="25" y1="160" x2="225" y2="160"/>
        <line class="skill-expert" x1="25" y1="160" x2="225" y2="160"/>
    </svg>
</body>
```

### Example 12: Countdown timer

```html
<style>
    .timer-bg {
        fill: none;
        stroke: #f3f4f6;
        stroke-width: 8;
    }
    .timer-remaining {
        fill: none;
        stroke: #ef4444;
        stroke-width: 8;
        stroke-dasharray: 314;
        stroke-dashoffset: 94.2;  /* 70% time elapsed */
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="200" height="200">
        <circle class="timer-bg" cx="100" cy="100" r="50"/>
        <circle class="timer-remaining" cx="100" cy="100" r="50" transform="rotate(-90 100 100)"/>
    </svg>
</body>
```

### Example 13: Multi-step progress

```html
<style>
    .step-track {
        fill: none;
        stroke: #e5e7eb;
        stroke-width: 4;
    }
    .step-complete {
        fill: none;
        stroke: #3b82f6;
        stroke-width: 4;
        stroke-dasharray: 250;
        stroke-dashoffset: 125;  /* 50% complete (2 of 4 steps) */
    }
</style>
<body>
    <svg width="300" height="100">
        <line class="step-track" x1="25" y1="50" x2="275" y2="50"/>
        <line class="step-complete" x1="25" y1="50" x2="275" y2="50"/>
        <circle fill="#3b82f6" cx="25" cy="50" r="8"/>
        <circle fill="#3b82f6" cx="108" cy="50" r="8"/>
        <circle fill="white" stroke="#e5e7eb" stroke-width="3" cx="192" cy="50" r="8"/>
        <circle fill="white" stroke="#e5e7eb" stroke-width="3" cx="275" cy="50" r="8"/>
    </svg>
</body>
```

### Example 14: Radar sweep effect

```html
<style>
    .radar-circle {
        fill: none;
        stroke: #10b981;
        stroke-width: 2;
        opacity: 0.5;
    }
    .radar-sweep {
        fill: none;
        stroke: #10b981;
        stroke-width: 3;
        stroke-dasharray: 31.4 251;
        stroke-dashoffset: -20;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="200" height="200">
        <circle class="radar-circle" cx="100" cy="100" r="80"/>
        <circle class="radar-circle" cx="100" cy="100" r="60"/>
        <circle class="radar-circle" cx="100" cy="100" r="40"/>
        <circle class="radar-sweep" cx="100" cy="100" r="80" transform="rotate(-90 100 100)"/>
    </svg>
</body>
```

### Example 15: Volume level meter

```html
<style>
    .volume-bg {
        fill: none;
        stroke: #f3f4f6;
        stroke-width: 6;
        stroke-linecap: round;
    }
    .volume-low {
        fill: none;
        stroke: #10b981;
        stroke-width: 6;
        stroke-dasharray: 100;
        stroke-dashoffset: 80;
        stroke-linecap: round;
    }
    .volume-medium {
        fill: none;
        stroke: #f59e0b;
        stroke-width: 6;
        stroke-dasharray: 100;
        stroke-dashoffset: 50;
        stroke-linecap: round;
    }
    .volume-high {
        fill: none;
        stroke: #ef4444;
        stroke-width: 6;
        stroke-dasharray: 100;
        stroke-dashoffset: 10;
        stroke-linecap: round;
    }
</style>
<body>
    <svg width="300" height="150">
        <line class="volume-bg" x1="25" y1="30" x2="125" y2="30"/>
        <line class="volume-low" x1="25" y1="30" x2="125" y2="30"/>

        <line class="volume-bg" x1="25" y1="70" x2="125" y2="70"/>
        <line class="volume-medium" x1="25" y1="70" x2="125" y2="70"/>

        <line class="volume-bg" x1="25" y1="110" x2="125" y2="110"/>
        <line class="volume-high" x1="25" y1="110" x2="125" y2="110"/>
    </svg>
</body>
```

---

## See Also

- [stroke-dasharray](/reference/cssproperties/css_prop_stroke-dasharray) - Define dash pattern
- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [stroke-width](/reference/cssproperties/css_prop_stroke-width) - Control stroke thickness
- [stroke-linecap](/reference/cssproperties/css_prop_stroke-linecap) - Control dash end appearance
- [stroke-opacity](/reference/cssproperties/css_prop_stroke-opacity) - SVG stroke transparency
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
