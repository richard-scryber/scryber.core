---
layout: default
title: dominant-baseline
parent: CSS Properties
parent_url: /reference/cssproperties/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# dominant-baseline : SVG Vertical Text Alignment Property

The `dominant-baseline` property specifies the vertical alignment of SVG text content relative to the baseline. This property controls how text is positioned vertically at its anchor point, allowing precise control over text placement in diagrams, charts, and technical drawings.

## Usage

```css
selector {
    dominant-baseline: value;
}
```

The dominant-baseline property accepts keyword values that determine the vertical alignment of text relative to its y coordinate.

---

## Supported Values

### Keywords

- `auto` - Default value. Uses the baseline appropriate for the script of the text.

- `text-bottom` - Aligns the bottom of the text bounding box with the y coordinate.

- `alphabetic` - Aligns the alphabetic baseline (the line where most letters sit) with the y coordinate. This is typical for Latin scripts.

- `ideographic` - Aligns the ideographic baseline (bottom of ideographic characters) with the y coordinate. Used for East Asian scripts.

- `middle` - Aligns the middle of the text with the y coordinate. Centers text vertically on the given point.

- `central` - Aligns the central baseline with the y coordinate. Similar to middle but uses the font's central baseline.

- `mathematical` - Aligns the mathematical baseline (for mathematical expressions) with the y coordinate.

- `hanging` - Aligns the hanging baseline (top of certain scripts like Devanagari) with the y coordinate.

- `text-top` - Aligns the top of the text bounding box with the y coordinate.

---

## Supported Elements

The `dominant-baseline` property applies to SVG text elements:
- `<text>` SVG text elements
- `<tspan>` text span elements within SVG text
- Any element that can contain SVG text content

---

## Notes

- Default value is `auto` (uses script-appropriate baseline)
- Most commonly used values are `middle`, `hanging`, and `alphabetic`
- Essential for precise vertical positioning of text in technical drawings
- `middle` is particularly useful for centering labels on chart elements
- `hanging` positions text below the y coordinate (text "hangs" from the point)
- Works in combination with `text-anchor` for complete 2D text positioning
- Different from `vertical-align` which applies to HTML inline elements
- Important for aligning text with geometric shapes and data points
- Font metrics influence the exact positioning of baselines
- Vector text maintains alignment precision at any zoom level in PDF viewers

---

## Data Binding

The `dominant-baseline` property can be dynamically controlled through data binding, enabling responsive vertical text positioning based on data requirements. This is crucial for creating adaptive labels, annotations, and dynamic text placement in visualizations.

### Example 1: Dynamic badge labels with centered text

```html
<style>
    .badge-circle { stroke-width: 2; }
    .badge-text { font-size: 12px; font-weight: bold; text-anchor: middle; }
</style>
<body>
    <svg width="500" height="150" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each notifications}}">
            <circle class="badge-circle"
                    cx="{{@index | multiply: 100 | add: 60}}" cy="75" r="{{radius}}"
                    fill="{{badgeColor}}" stroke="{{borderColor}}"/>
            <text class="badge-text"
                  x="{{@index | multiply: 100 | add: 60}}"
                  y="75"
                  fill="{{textColor}}"
                  style="dominant-baseline: middle">{{count}}</text>
        </template>
    </svg>
</body>
```

### Example 2: Data-driven timeline labels with hanging baseline

```html
<style>
    .timeline-line { stroke: #9ca3af; stroke-width: 2; }
    .event-marker { fill: #3b82f6; }
    .event-label { font-size: 12px; fill: #374151; text-anchor: middle; }
</style>
<body>
    <svg width="600" height="200" xmlns="http://www.w3.org/2000/svg">
        <line class="timeline-line" x1="50" y1="100" x2="550" y2="100"/>
        <template data-bind="{{#each events}}">
            <circle class="event-marker" cx="{{position}}" cy="100" r="6"/>
            <text class="event-label"
                  x="{{position}}"
                  y="{{labelY}}"
                  style="dominant-baseline: {{#if above}}text-bottom{{else}}hanging{{/if}}">{{eventName}}</text>
        </template>
    </svg>
</body>
```

### Example 3: Conditional baseline for chart annotations

```html
<style>
    .data-bar { fill: #3b82f6; }
    .value-label { font-size: 14px; font-weight: bold; text-anchor: middle; }
</style>
<body>
    <svg width="500" height="300" xmlns="http://www.w3.org/2000/svg">
        <template data-bind="{{#each chartData}}">
            <rect class="data-bar"
                  x="{{x}}" y="{{y}}" width="{{barWidth}}" height="{{height}}"/>
            <text class="value-label"
                  x="{{x | add: barWidth | divide: 2}}"
                  y="{{labelY}}"
                  fill="{{labelColor}}"
                  style="dominant-baseline: {{#if insideBar}}middle{{else}}alphabetic{{/if}}">{{value}}</text>
        </template>
    </svg>
</body>
```

---

## Examples

### Example 1: Comparing baseline values

```html
<style>
    .guide-line { stroke: #ef4444; stroke-width: 1; }
    .text-demo { fill: #1f2937; font-size: 16px; }
</style>
<body>
    <svg width="300" height="400">
        <!-- Horizontal guide lines -->
        <line class="guide-line" x1="0" y1="40" x2="300" y2="40"/>
        <line class="guide-line" x1="0" y1="90" x2="300" y2="90"/>
        <line class="guide-line" x1="0" y1="140" x2="300" y2="140"/>
        <line class="guide-line" x1="0" y1="190" x2="300" y2="190"/>
        <line class="guide-line" x1="0" y1="240" x2="300" y2="240"/>
        <line class="guide-line" x1="0" y1="290" x2="300" y2="290"/>
        <line class="guide-line" x1="0" y1="340" x2="300" y2="340"/>

        <text class="text-demo" x="10" y="40" dominant-baseline="auto">auto</text>
        <text class="text-demo" x="10" y="90" dominant-baseline="hanging">hanging</text>
        <text class="text-demo" x="10" y="140" dominant-baseline="middle">middle</text>
        <text class="text-demo" x="10" y="190" dominant-baseline="central">central</text>
        <text class="text-demo" x="10" y="240" dominant-baseline="alphabetic">alphabetic</text>
        <text class="text-demo" x="10" y="290" dominant-baseline="text-top">text-top</text>
        <text class="text-demo" x="10" y="340" dominant-baseline="text-bottom">text-bottom</text>
    </svg>
</body>
```

### Example 2: Centered labels on chart bars

```html
<style>
    .bar { fill: #3b82f6; }
    .label {
        text-anchor: middle;
        dominant-baseline: middle;
        fill: white;
        font-size: 14px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="400" height="250">
        <rect class="bar" x="40" y="80" width="60" height="100"/>
        <text class="label" x="70" y="130">45</text>

        <rect class="bar" x="130" y="50" width="60" height="130"/>
        <text class="label" x="160" y="115">78</text>

        <rect class="bar" x="220" y="100" width="60" height="80"/>
        <text class="label" x="250" y="140">32</text>

        <rect class="bar" x="310" y="30" width="60" height="150"/>
        <text class="label" x="340" y="105">92</text>
    </svg>
</body>
```

### Example 3: Network diagram with centered node labels

```html
<style>
    .node { fill: white; stroke: #3b82f6; stroke-width: 3; }
    .node-label {
        text-anchor: middle;
        dominant-baseline: middle;
        fill: #1f2937;
        font-size: 12px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="400" height="300">
        <circle class="node" cx="200" cy="75" r="35"/>
        <text class="node-label" x="200" y="75">Server</text>

        <circle class="node" cx="100" cy="200" r="30"/>
        <text class="node-label" x="100" y="200">DB</text>

        <circle class="node" cx="200" cy="200" r="30"/>
        <text class="node-label" x="200" y="200">Cache</text>

        <circle class="node" cx="300" cy="200" r="30"/>
        <text class="node-label" x="300" y="200">API</text>
    </svg>
</body>
```

### Example 4: Data point labels hanging below points

```html
<style>
    .data-line { stroke: #8b5cf6; stroke-width: 2; fill: none; }
    .data-point { fill: #8b5cf6; }
    .point-label {
        text-anchor: middle;
        dominant-baseline: hanging;
        fill: #6b7280;
        font-size: 11px;
    }
</style>
<body>
    <svg width="400" height="200">
        <polyline class="data-line" points="50,120 120,80 190,100 260,60 330,90"/>

        <circle class="data-point" cx="50" cy="120" r="4"/>
        <text class="point-label" x="50" y="125">45</text>

        <circle class="data-point" cx="120" cy="80" r="4"/>
        <text class="point-label" x="120" y="85">67</text>

        <circle class="data-point" cx="190" cy="100" r="4"/>
        <text class="point-label" x="190" y="105">58</text>

        <circle class="data-point" cx="260" cy="60" r="4"/>
        <text class="point-label" x="260" y="65">82</text>

        <circle class="data-point" cx="330" cy="90" r="4"/>
        <text class="point-label" x="330" y="95">71</text>
    </svg>
</body>
```

### Example 5: Gauge meter with centered text

```html
<style>
    .gauge-bg { fill: none; stroke: #e5e7eb; stroke-width: 12; }
    .gauge-fill { fill: none; stroke: #10b981; stroke-width: 12; }
    .gauge-value {
        text-anchor: middle;
        dominant-baseline: middle;
        fill: #1f2937;
        font-size: 48px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="250" height="200">
        <path class="gauge-bg" d="M 50 150 A 75 75 0 1 1 200 150"/>
        <path class="gauge-fill" d="M 50 150 A 75 75 0 0 1 125 75"/>
        <text class="gauge-value" x="125" y="130">75</text>
    </svg>
</body>
```

### Example 6: Timeline with event labels

```html
<style>
    .timeline { stroke: #9ca3af; stroke-width: 2; }
    .event { fill: #3b82f6; }
    .event-above {
        text-anchor: middle;
        dominant-baseline: text-bottom;
        fill: #374151;
        font-size: 12px;
    }
    .event-below {
        text-anchor: middle;
        dominant-baseline: hanging;
        fill: #6b7280;
        font-size: 10px;
    }
</style>
<body>
    <svg width="400" height="150">
        <line class="timeline" x1="50" y1="75" x2="350" y2="75"/>

        <circle class="event" cx="50" cy="75" r="6"/>
        <text class="event-above" x="50" y="60">Start</text>
        <text class="event-below" x="50" y="85">Jan 1</text>

        <circle class="event" cx="150" cy="75" r="6"/>
        <text class="event-above" x="150" y="60">Milestone</text>
        <text class="event-below" x="150" y="85">Mar 15</text>

        <circle class="event" cx="250" cy="75" r="6"/>
        <text class="event-above" x="250" y="60">Review</text>
        <text class="event-below" x="250" y="85">Jun 30</text>

        <circle class="event" cx="350" cy="75" r="6"/>
        <text class="event-above" x="350" y="60">Launch</text>
        <text class="event-below" x="350" y="85">Dec 31</text>
    </svg>
</body>
```

### Example 7: Pie chart with centered percentage labels

```html
<style>
    .slice-1 { fill: #3b82f6; }
    .slice-2 { fill: #10b981; }
    .slice-3 { fill: #f59e0b; }
    .slice-4 { fill: #ef4444; }
    .percentage {
        text-anchor: middle;
        dominant-baseline: middle;
        fill: white;
        font-size: 18px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="300" height="300">
        <path class="slice-1" d="M 150 150 L 150 50 A 100 100 0 0 1 250 150 Z"/>
        <path class="slice-2" d="M 150 150 L 250 150 A 100 100 0 0 1 150 250 Z"/>
        <path class="slice-3" d="M 150 150 L 150 250 A 100 100 0 0 1 50 150 Z"/>
        <path class="slice-4" d="M 150 150 L 50 150 A 100 100 0 0 1 150 50 Z"/>

        <text class="percentage" x="190" y="110">35%</text>
        <text class="percentage" x="190" y="190">28%</text>
        <text class="percentage" x="110" y="190">22%</text>
        <text class="percentage" x="110" y="110">15%</text>
    </svg>
</body>
```

### Example 8: Button labels centered in rectangles

```html
<style>
    .button { fill: #3b82f6; }
    .button-label {
        text-anchor: middle;
        dominant-baseline: middle;
        fill: white;
        font-size: 14px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="400" height="200">
        <rect class="button" x="30" y="30" width="100" height="40" rx="5"/>
        <text class="button-label" x="80" y="50">Save</text>

        <rect class="button" x="150" y="30" width="100" height="40" rx="5"/>
        <text class="button-label" x="200" y="50">Cancel</text>

        <rect class="button" x="270" y="30" width="100" height="40" rx="5"/>
        <text class="button-label" x="320" y="50">Submit</text>
    </svg>
</body>
```

### Example 9: Y-axis labels aligned to tick marks

```html
<style>
    .axis { stroke: #374151; stroke-width: 2; }
    .tick { stroke: #9ca3af; stroke-width: 1; }
    .y-label {
        text-anchor: end;
        dominant-baseline: middle;
        fill: #6b7280;
        font-size: 12px;
    }
</style>
<body>
    <svg width="300" height="300">
        <line class="axis" x1="80" y1="50" x2="80" y2="250"/>

        <line class="tick" x1="75" y1="50" x2="80" y2="50"/>
        <text class="y-label" x="70" y="50">100</text>

        <line class="tick" x1="75" y1="100" x2="80" y2="100"/>
        <text class="y-label" x="70" y="100">75</text>

        <line class="tick" x1="75" y1="150" x2="80" y2="150"/>
        <text class="y-label" x="70" y="150">50</text>

        <line class="tick" x1="75" y1="200" x2="80" y2="200"/>
        <text class="y-label" x="70" y="200">25</text>

        <line class="tick" x1="75" y1="250" x2="80" y2="250"/>
        <text class="y-label" x="70" y="250">0</text>
    </svg>
</body>
```

### Example 10: Badge with centered count

```html
<style>
    .badge { fill: #ef4444; }
    .badge-count {
        text-anchor: middle;
        dominant-baseline: middle;
        fill: white;
        font-size: 12px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="300" height="100">
        <circle class="badge" cx="60" cy="50" r="15"/>
        <text class="badge-count" x="60" y="50">3</text>

        <circle class="badge" cx="140" cy="50" r="15"/>
        <text class="badge-count" x="140" y="50">12</text>

        <circle class="badge" cx="220" cy="50" r="15"/>
        <text class="badge-count" x="220" y="50">99+</text>
    </svg>
</body>
```

### Example 11: Traffic light with centered labels

```html
<style>
    .light-red { fill: #ef4444; }
    .light-yellow { fill: #fbbf24; }
    .light-green { fill: #10b981; }
    .light-label {
        text-anchor: start;
        dominant-baseline: middle;
        fill: #1f2937;
        font-size: 14px;
    }
</style>
<body>
    <svg width="250" height="300">
        <rect fill="#374151" x="50" y="30" width="80" height="220" rx="10"/>

        <circle class="light-red" cx="90" cy="70" r="25"/>
        <text class="light-label" x="140" y="70">Stop</text>

        <circle class="light-yellow" cx="90" cy="140" r="25"/>
        <text class="light-label" x="140" y="140">Caution</text>

        <circle class="light-green" cx="90" cy="210" r="25"/>
        <text class="light-label" x="140" y="210">Go</text>
    </svg>
</body>
```

### Example 12: Progress percentage in circle

```html
<style>
    .progress-bg { fill: none; stroke: #e5e7eb; stroke-width: 10; }
    .progress { fill: none; stroke: #3b82f6; stroke-width: 10; }
    .progress-text {
        text-anchor: middle;
        dominant-baseline: middle;
        fill: #1f2937;
        font-size: 32px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="200" height="200">
        <circle class="progress-bg" cx="100" cy="100" r="60"/>
        <circle class="progress" cx="100" cy="100" r="60"
                stroke-dasharray="377" stroke-dashoffset="94.25"
                transform="rotate(-90 100 100)"/>
        <text class="progress-text" x="100" y="100">75%</text>
    </svg>
</body>
```

### Example 13: Table cell content

```html
<style>
    .cell { fill: white; stroke: #d1d5db; stroke-width: 1; }
    .cell-text {
        text-anchor: middle;
        dominant-baseline: middle;
        fill: #374151;
        font-size: 14px;
    }
    .header { fill: #f3f4f6; }
    .header-text {
        text-anchor: middle;
        dominant-baseline: middle;
        fill: #1f2937;
        font-size: 14px;
        font-weight: bold;
    }
</style>
<body>
    <svg width="350" height="200">
        <rect class="cell header" x="25" y="25" width="100" height="40"/>
        <text class="header-text" x="75" y="45">Product</text>

        <rect class="cell header" x="125" y="25" width="100" height="40"/>
        <text class="header-text" x="175" y="45">Price</text>

        <rect class="cell header" x="225" y="25" width="100" height="40"/>
        <text class="header-text" x="275" y="45">Stock</text>

        <rect class="cell" x="25" y="65" width="100" height="35"/>
        <text class="cell-text" x="75" y="82.5">Widget A</text>

        <rect class="cell" x="125" y="65" width="100" height="35"/>
        <text class="cell-text" x="175" y="82.5">$12.99</text>

        <rect class="cell" x="225" y="65" width="100" height="35"/>
        <text class="cell-text" x="275" y="82.5">45</text>
    </svg>
</body>
```

### Example 14: Icon with top-aligned label

```html
<style>
    .icon-circle { fill: #dbeafe; }
    .icon-symbol { fill: #3b82f6; }
    .icon-text {
        text-anchor: middle;
        dominant-baseline: hanging;
        fill: #1f2937;
        font-size: 12px;
    }
</style>
<body>
    <svg width="350" height="150">
        <circle class="icon-circle" cx="75" cy="50" r="30"/>
        <rect class="icon-symbol" x="65" y="40" width="20" height="20" rx="2"/>
        <text class="icon-text" x="75" y="90">Files</text>

        <circle class="icon-circle" cx="175" cy="50" r="30"/>
        <circle class="icon-symbol" cx="175" cy="50" r="10"/>
        <text class="icon-text" x="175" y="90">Settings</text>

        <circle class="icon-circle" cx="275" cy="50" r="30"/>
        <path class="icon-symbol" d="M 265 50 L 275 40 L 285 50"
              fill="none" stroke="#3b82f6" stroke-width="3"/>
        <text class="icon-text" x="275" y="90">Upload</text>
    </svg>
</body>
```

### Example 15: Callout box with centered message

```html
<style>
    .callout-info { fill: #dbeafe; stroke: #3b82f6; stroke-width: 2; }
    .callout-warning { fill: #fef3c7; stroke: #f59e0b; stroke-width: 2; }
    .callout-error { fill: #fee2e2; stroke: #ef4444; stroke-width: 2; }
    .callout-text {
        text-anchor: middle;
        dominant-baseline: middle;
        font-size: 14px;
    }
    .text-info { fill: #1e40af; }
    .text-warning { fill: #92400e; }
    .text-error { fill: #991b1b; }
</style>
<body>
    <svg width="350" height="250">
        <rect class="callout-info" x="25" y="25" width="300" height="50" rx="5"/>
        <text class="callout-text text-info" x="175" y="50">Information: Task completed</text>

        <rect class="callout-warning" x="25" y="100" width="300" height="50" rx="5"/>
        <text class="callout-text text-warning" x="175" y="125">Warning: Low disk space</text>

        <rect class="callout-error" x="25" y="175" width="300" height="50" rx="5"/>
        <text class="callout-text text-error" x="175" y="200">Error: Connection failed</text>
    </svg>
</body>
```

---

## See Also

- [text-anchor](/reference/cssproperties/css_prop_text-anchor) - SVG horizontal text alignment
- [fill](/reference/cssproperties/css_prop_fill) - SVG fill color property
- [vertical-align](/reference/cssproperties/css_prop_vertical-align) - HTML vertical alignment
- [stroke](/reference/cssproperties/css_prop_stroke) - SVG stroke color property
- [style attribute](/reference/htmlattributes/style) - Inline CSS styles

---
