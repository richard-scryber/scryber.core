---
layout: default
title: text-anchor
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# text-anchor : SVG Text Alignment Property

The `text-anchor` property defines the horizontal alignment of SVG text content relative to a given point. This property controls whether text is left-aligned (starts at the point), center-aligned (centered on the point), or right-aligned (ends at the point).

## Usage

```css
selector {
    text-anchor: value;
}
```

The text-anchor property accepts keyword values that determine how text is positioned relative to its x coordinate anchor point.

---

## Supported Values

### Keywords

- `start` - Default value. Text is aligned so that the start of the text string (left edge in LTR languages) is at the given x position. Equivalent to left-align in most contexts.

- `middle` - Text is centered on the given x position. The midpoint of the text string is positioned at the x coordinate.

- `end` - Text is aligned so that the end of the text string (right edge in LTR languages) is at the given x position. Equivalent to right-align in most contexts.

Note: For right-to-left (RTL) text, `start` and `end` are reversed to respect the writing direction.

---

## Supported Elements

The `text-anchor` property applies to SVG text elements:
- `<text>` SVG text elements
- `<tspan>` text span elements within SVG text
- Any element that can contain SVG text content

Note: This property does not apply to HTML text elements. Use `text-align` for regular HTML elements.

---

## Notes

- Default value is `start` (left-aligned in LTR languages)
- Alignment is relative to the x coordinate specified in the text element
- Essential for positioning labels, annotations, and data labels in diagrams
- Commonly used with `middle` for centered chart labels and titles
- Use `end` for right-aligned labels, especially in bar charts
- The anchor point does not move; the text position changes relative to it
- Works in combination with `dominant-baseline` for vertical positioning
- Important for SVG data visualizations and technical diagrams
- Text anchor is independent of the bounding box of the text
- Useful for creating responsive text that adapts to different content lengths
- Vector text maintains alignment precision at any zoom level in PDF viewers

---

## Data Binding

The `text-anchor` property can be dynamically controlled through data binding, enabling responsive text alignment based on data position, layout requirements, or conditional states. This is essential for creating adaptive charts, labels, and data visualizations.

### Example 1: Dynamic chart labels with data-driven anchoring

```html
<style>
    .bar { fill: #3b82f6; }
    .data-label { font-size: 14px; font-weight: bold; }
</style>
<body>
    <svg width="500" height="300" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each dataPoints}}">
            <rect class="bar" x="{{x}}" y="{{y}}" width="{{width}}" height="{{height}}"/>
            <text class="data-label"
                  x="{{labelX}}"
                  y="{{labelY}}"
                  fill="{{textColor}}"
                  style="text-anchor: {{anchorPosition}}">{{value}}</text>
        </template>
    </svg>
</body>
```

### Example 2: Conditional text alignment based on position

```html
<style>
    .data-point { fill: #3b82f6; }
    .point-label { font-size: 12px; fill: #1f2937; }
</style>
<body>
    <svg width="600" height="400" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each markers}}">
            <circle class="data-point" cx="{{x}}" cy="{{y}}" r="5"/>
            <text class="point-label"
                  x="{{x}}"
                  y="{{y | subtract: 10}}"
                  style="text-anchor: {{#if x | lessThan: 100}}start{{else if x | greaterThan: 500}}end{{else}}middle{{/if}}">{{label}}</text>
        </template>
    </svg>
</body>
```

### Example 3: Data-driven network diagram with centered labels

```html
<style>
    .node { stroke: #3b82f6; stroke-width: 3; }
    .node-label { font-size: 13px; font-weight: bold; dominant-baseline: middle; }
</style>
<body>
    <svg width="500" height="400" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each nodes}}">
            <circle class="node"
                    cx="{{x}}" cy="{{y}}" r="{{radius}}"
                    fill="{{nodeColor}}"/>
            <text class="node-label"
                  x="{{x}}"
                  y="{{y}}"
                  fill="{{labelColor}}"
                  style="text-anchor: middle">{{name}}</text>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Comparing all text-anchor values

```html
<style>
    .anchor-line { stroke: #ef4444; stroke-width: 1; }
    .text-start { text-anchor: start; fill: #1f2937; }
    .text-middle { text-anchor: middle; fill: #1f2937; }
    .text-end { text-anchor: end; fill: #1f2937; }
</style>
<body>
    <svg width="300" height="200">
        <!-- Visual guide line -->
        <line class="anchor-line" x1="150" y1="0" x2="150" y2="200"/>

        <!-- Start anchor -->
        <text class="text-start" x="150" y="40" font-size="16">Start aligned</text>

        <!-- Middle anchor -->
        <text class="text-middle" x="150" y="100" font-size="16">Middle aligned</text>

        <!-- End anchor -->
        <text class="text-end" x="150" y="160" font-size="16">End aligned</text>
    </svg>
</body>
```

### Example 2: Chart labels with center alignment

```html
<style>
    .bar { fill: #3b82f6; }
    .label { text-anchor: middle; fill: #1f2937; font-size: 12px; }
    .value { text-anchor: middle; fill: white; font-size: 14px; font-weight: bold; }
</style>
<body>
    <svg width="400" height="250">
        <!-- Bars -->
        <rect class="bar" x="40" y="80" width="60" height="100"/>
        <rect class="bar" x="130" y="50" width="60" height="130"/>
        <rect class="bar" x="220" y="100" width="60" height="80"/>
        <rect class="bar" x="310" y="30" width="60" height="150"/>

        <!-- Values on bars -->
        <text class="value" x="70" y="135">45</text>
        <text class="value" x="160" y="120">78</text>
        <text class="value" x="250" y="145">32</text>
        <text class="value" x="340" y="110">92</text>

        <!-- Labels -->
        <text class="label" x="70" y="200">Q1</text>
        <text class="label" x="160" y="200">Q2</text>
        <text class="label" x="250" y="200">Q3</text>
        <text class="label" x="340" y="200">Q4</text>
    </svg>
</body>
```

### Example 3: Right-aligned data labels

```html
<style>
    .data-line { stroke: #6b7280; stroke-width: 1; }
    .data-label { text-anchor: end; fill: #374151; font-size: 14px; }
    .data-value { text-anchor: start; fill: #1f2937; font-size: 14px; font-weight: bold; }
</style>
<body>
    <svg width="300" height="200">
        <line class="data-line" x1="100" y1="0" x2="100" y2="200"/>

        <text class="data-label" x="95" y="40">Revenue:</text>
        <text class="data-value" x="105" y="40">$125,000</text>

        <text class="data-label" x="95" y="80">Expenses:</text>
        <text class="data-value" x="105" y="80">$87,500</text>

        <text class="data-label" x="95" y="120">Profit:</text>
        <text class="data-value" x="105" y="120">$37,500</text>

        <text class="data-label" x="95" y="160">Margin:</text>
        <text class="data-value" x="105" y="160">30%</text>
    </svg>
</body>
```

### Example 4: Centered chart title

```html
<style>
    .chart-title {
        text-anchor: middle;
        fill: #1f2937;
        font-size: 20px;
        font-weight: bold;
    }
    .chart-subtitle {
        text-anchor: middle;
        fill: #6b7280;
        font-size: 14px;
    }
</style>
<body>
    <svg width="400" height="100">
        <text class="chart-title" x="200" y="40">Quarterly Sales Report</text>
        <text class="chart-subtitle" x="200" y="70">Fiscal Year 2024</text>
    </svg>
</body>
```

### Example 5: Pie chart labels

```html
<style>
    .slice-1 { fill: #3b82f6; }
    .slice-2 { fill: #10b981; }
    .slice-3 { fill: #f59e0b; }
    .slice-4 { fill: #ef4444; }
    .pie-label {
        text-anchor: middle;
        fill: white;
        font-size: 14px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="300" height="300">
        <circle class="slice-1" cx="150" cy="150" r="100"/>
        <path class="slice-2" d="M 150 150 L 250 150 A 100 100 0 0 1 150 250 Z"/>
        <path class="slice-3" d="M 150 150 L 150 250 A 100 100 0 0 1 50 150 Z"/>
        <path class="slice-4" d="M 150 150 L 50 150 A 100 100 0 0 1 150 50 Z"/>

        <text class="pie-label" x="150" y="100">40%</text>
        <text class="pie-label" x="190" y="190">25%</text>
        <text class="pie-label" x="110" y="190">20%</text>
        <text class="pie-label" x="110" y="110">15%</text>
    </svg>
</body>
```

### Example 6: Timeline with labels

```html
<style>
    .timeline { stroke: #9ca3af; stroke-width: 2; }
    .milestone { fill: #3b82f6; }
    .event-label {
        text-anchor: middle;
        fill: #374151;
        font-size: 12px;
    }
    .date-label {
        text-anchor: middle;
        fill: #6b7280;
        font-size: 10px;
    }
</style>
<body>
    <svg width="400" height="150">
        <line class="timeline" x1="50" y1="75" x2="350" y2="75"/>

        <circle class="milestone" cx="50" cy="75" r="6"/>
        <text class="event-label" x="50" y="50">Start</text>
        <text class="date-label" x="50" y="105">Jan 1</text>

        <circle class="milestone" cx="150" cy="75" r="6"/>
        <text class="event-label" x="150" y="50">Phase 1</text>
        <text class="date-label" x="150" y="105">Mar 15</text>

        <circle class="milestone" cx="250" cy="75" r="6"/>
        <text class="event-label" x="250" y="50">Phase 2</text>
        <text class="date-label" x="250" y="105">Jun 30</text>

        <circle class="milestone" cx="350" cy="75" r="6"/>
        <text class="event-label" x="350" y="50">Complete</text>
        <text class="date-label" x="350" y="105">Dec 31</text>
    </svg>
</body>
```

### Example 7: Gauge meter with centered value

```html
<style>
    .gauge-bg { fill: none; stroke: #e5e7eb; stroke-width: 12; }
    .gauge-fill { fill: none; stroke: #10b981; stroke-width: 12; stroke-linecap: round; }
    .gauge-value {
        text-anchor: middle;
        fill: #1f2937;
        font-size: 36px;
        font-weight: bold;
    }
    .gauge-unit {
        text-anchor: middle;
        fill: #6b7280;
        font-size: 14px;
    }
</style>
<body>
    <svg width="250" height="200">
        <path class="gauge-bg" d="M 50 150 A 75 75 0 1 1 200 150"/>
        <path class="gauge-fill" d="M 50 150 A 75 75 0 0 1 125 75"/>
        <text class="gauge-value" x="125" y="140">75</text>
        <text class="gauge-unit" x="125" y="165">mph</text>
    </svg>
</body>
```

### Example 8: Network diagram with node labels

```html
<style>
    .node { fill: white; stroke: #3b82f6; stroke-width: 3; }
    .connection { stroke: #d1d5db; stroke-width: 2; }
    .node-label {
        text-anchor: middle;
        fill: #1f2937;
        font-size: 12px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="400" height="300">
        <line class="connection" x1="200" y1="75" x2="100" y2="200"/>
        <line class="connection" x1="200" y1="75" x2="200" y2="200"/>
        <line class="connection" x1="200" y1="75" x2="300" y2="200"/>

        <circle class="node" cx="200" cy="75" r="30"/>
        <text class="node-label" x="200" y="80">Server</text>

        <circle class="node" cx="100" cy="200" r="25"/>
        <text class="node-label" x="100" y="205">Client A</text>

        <circle class="node" cx="200" cy="200" r="25"/>
        <text class="node-label" x="200" y="205">Client B</text>

        <circle class="node" cx="300" cy="200" r="25"/>
        <text class="node-label" x="300" y="205">Client C</text>
    </svg>
</body>
```

### Example 9: Axis labels for a graph

```html
<style>
    .axis { stroke: #374151; stroke-width: 2; }
    .axis-label-x {
        text-anchor: middle;
        fill: #6b7280;
        font-size: 12px;
    }
    .axis-label-y {
        text-anchor: end;
        fill: #6b7280;
        font-size: 12px;
    }
</style>
<body>
    <svg width="350" height="300">
        <!-- Axes -->
        <line class="axis" x1="50" y1="250" x2="320" y2="250"/>
        <line class="axis" x1="50" y1="50" x2="50" y2="250"/>

        <!-- X-axis labels -->
        <text class="axis-label-x" x="80" y="270">2020</text>
        <text class="axis-label-x" x="140" y="270">2021</text>
        <text class="axis-label-x" x="200" y="270">2022</text>
        <text class="axis-label-x" x="260" y="270">2023</text>
        <text class="axis-label-x" x="320" y="270">2024</text>

        <!-- Y-axis labels -->
        <text class="axis-label-y" x="45" y="250">0</text>
        <text class="axis-label-y" x="45" y="200">25</text>
        <text class="axis-label-y" x="45" y="150">50</text>
        <text class="axis-label-y" x="45" y="100">75</text>
        <text class="axis-label-y" x="45" y="50">100</text>
    </svg>
</body>
```

### Example 10: Icon with centered caption

```html
<style>
    .icon-bg { fill: #dbeafe; }
    .icon-shape { fill: #3b82f6; }
    .icon-label {
        text-anchor: middle;
        fill: #1f2937;
        font-size: 12px;
    }
</style>
<body>
    <svg width="400" height="200">
        <circle class="icon-bg" cx="75" cy="75" r="35"/>
        <path class="icon-shape" d="M 75 60 L 85 80 L 65 80 Z"/>
        <text class="icon-label" x="75" y="140">Upload</text>

        <circle class="icon-bg" cx="200" cy="75" r="35"/>
        <path class="icon-shape" d="M 200 90 L 190 70 L 210 70 Z"/>
        <text class="icon-label" x="200" y="140">Download</text>

        <circle class="icon-bg" cx="325" cy="75" r="35"/>
        <rect class="icon-shape" x="315" y="65" width="20" height="20"/>
        <text class="icon-label" x="325" y="140">Save</text>
    </svg>
</body>
```

### Example 11: Score display

```html
<style>
    .score-value {
        text-anchor: middle;
        fill: #1f2937;
        font-size: 48px;
        font-weight: bold;
    }
    .score-max {
        text-anchor: middle;
        fill: #6b7280;
        font-size: 24px;
    }
    .score-label {
        text-anchor: middle;
        fill: #9ca3af;
        font-size: 14px;
    }
</style>
<body>
    <svg width="200" height="200">
        <text class="score-value" x="100" y="90">87</text>
        <text class="score-max" x="100" y="120">/100</text>
        <text class="score-label" x="100" y="150">Overall Score</text>
    </svg>
</body>
```

### Example 12: Seating chart labels

```html
<style>
    .seat { fill: #dbeafe; stroke: #3b82f6; stroke-width: 2; }
    .seat-label {
        text-anchor: middle;
        fill: #1e40af;
        font-size: 12px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="350" height="200">
        <rect class="seat" x="30" y="30" width="50" height="50" rx="5"/>
        <text class="seat-label" x="55" y="60">A1</text>

        <rect class="seat" x="100" y="30" width="50" height="50" rx="5"/>
        <text class="seat-label" x="125" y="60">A2</text>

        <rect class="seat" x="170" y="30" width="50" height="50" rx="5"/>
        <text class="seat-label" x="195" y="60">A3</text>

        <rect class="seat" x="240" y="30" width="50" height="50" rx="5"/>
        <text class="seat-label" x="265" y="60">A4</text>

        <rect class="seat" x="30" y="100" width="50" height="50" rx="5"/>
        <text class="seat-label" x="55" y="130">B1</text>

        <rect class="seat" x="100" y="100" width="50" height="50" rx="5"/>
        <text class="seat-label" x="125" y="130">B2</text>

        <rect class="seat" x="170" y="100" width="50" height="50" rx="5"/>
        <text class="seat-label" x="195" y="130">B3</text>

        <rect class="seat" x="240" y="100" width="50" height="50" rx="5"/>
        <text class="seat-label" x="265" y="130">B4</text>
    </svg>
</body>
```

### Example 13: Temperature gauge

```html
<style>
    .temp-bar { fill: #ef4444; }
    .temp-scale { stroke: #9ca3af; stroke-width: 1; }
    .temp-value {
        text-anchor: middle;
        fill: #1f2937;
        font-size: 24px;
        font-weight: bold;
    }
    .temp-mark {
        text-anchor: end;
        fill: #6b7280;
        font-size: 10px;
    }
</style>
<body>
    <svg width="200" height="300">
        <rect x="90" y="80" width="20" height="170" fill="#e5e7eb"/>
        <rect class="temp-bar" x="90" y="150" width="20" height="100"/>

        <line class="temp-scale" x1="85" y1="80" x2="90" y2="80"/>
        <text class="temp-mark" x="80" y="85">100°</text>

        <line class="temp-scale" x1="85" y1="125" x2="90" y2="125"/>
        <text class="temp-mark" x="80" y="130">75°</text>

        <line class="temp-scale" x1="85" y1="170" x2="90" y2="170"/>
        <text class="temp-mark" x="80" y="175">50°</text>

        <line class="temp-scale" x1="85" y1="215" x2="90" y2="215"/>
        <text class="temp-mark" x="80" y="220">25°</text>

        <line class="temp-scale" x1="85" y1="250" x2="90" y2="250"/>
        <text class="temp-mark" x="80" y="255">0°</text>

        <text class="temp-value" x="150" y="175">52°F</text>
    </svg>
</body>
```

### Example 14: Calendar day cells

```html
<style>
    .day-cell { fill: white; stroke: #d1d5db; stroke-width: 1; }
    .day-number {
        text-anchor: middle;
        fill: #374151;
        font-size: 14px;
    }
</style>
<body>
    <svg width="350" height="100">
        <rect class="day-cell" x="10" y="10" width="40" height="40"/>
        <text class="day-number" x="30" y="35">1</text>

        <rect class="day-cell" x="60" y="10" width="40" height="40"/>
        <text class="day-number" x="80" y="35">2</text>

        <rect class="day-cell" x="110" y="10" width="40" height="40"/>
        <text class="day-number" x="130" y="35">3</text>

        <rect class="day-cell" x="160" y="10" width="40" height="40"/>
        <text class="day-number" x="180" y="35">4</text>

        <rect class="day-cell" x="210" y="10" width="40" height="40"/>
        <text class="day-number" x="230" y="35">5</text>

        <rect class="day-cell" x="260" y="10" width="40" height="40"/>
        <text class="day-number" x="280" y="35">6</text>

        <rect class="day-cell" x="310" y="10" width="40" height="40"/>
        <text class="day-number" x="330" y="35">7</text>
    </svg>
</body>
```

### Example 15: Product rating display

```html
<style>
    .rating-star { fill: #fbbf24; }
    .rating-value {
        text-anchor: middle;
        fill: #1f2937;
        font-size: 32px;
        font-weight: bold;
    }
    .rating-count {
        text-anchor: middle;
        fill: #6b7280;
        font-size: 12px;
    }
</style>
<body>
    <svg width="250" height="150">
        <polygon class="rating-star" points="125,20 135,50 165,50 142,67 150,97 125,80 100,97 108,67 85,50 115,50"/>
        <text class="rating-value" x="125" y="125">4.8</text>
        <text class="rating-count" x="125" y="145">(1,234 reviews)</text>
    </svg>
</body>
```

---

## See Also

- [dominant-baseline](/reference/cssproperties/css_prop_dominant-baseline) - SVG vertical text alignment
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [text-align](/reference/cssproperties/css_prop_text-align) - HTML text alignment (for regular elements)
- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
