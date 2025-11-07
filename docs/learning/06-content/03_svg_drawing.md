---
layout: default
title: SVG Drawing
nav_order: 3
parent: Content Components
parent_url: /learning/06-content/
grand_parent: Learning Guides
grand_parent_url: /learning/
has_toc: false
---

# SVG Drawing

Master SVG shapes, paths, text, styling, and data binding to create dynamic charts, diagrams, and visualizations in PDF documents.

---

## Learning Objectives

By the end of this article, you'll be able to:
- Draw all SVG basic shapes
- Use SVG paths for custom graphics
- Add text to SVG elements
- Style SVG with CSS and attributes
- Bind data to SVG properties
- Create dynamic charts and visualizations
- Build data-driven graphics

---

## SVG Basic Shapes

### Rectangle

```html
<svg width="300" height="200">
    <!-- Basic rectangle -->
    <rect x="10" y="10" width="100" height="80" fill="#3b82f6" />

    <!-- Rounded corners -->
    <rect x="130" y="10" width="100" height="80" rx="10" fill="#10b981" />

    <!-- With stroke -->
    <rect x="10" y="110" width="100" height="80"
          fill="#f59e0b" stroke="#92400e" stroke-width="3" />
</svg>
```

### Circle

```html
<svg width="300" height="200">
    <!-- cx, cy = center coordinates, r = radius -->
    <circle cx="50" cy="50" r="40" fill="#3b82f6" />

    <circle cx="150" cy="50" r="40" fill="none"
            stroke="#10b981" stroke-width="4" />

    <circle cx="250" cy="50" r="40" fill="#f59e0b" opacity="0.6" />
</svg>
```

### Ellipse

```html
<svg width="400" height="200">
    <!-- cx, cy = center, rx = horizontal radius, ry = vertical radius -->
    <ellipse cx="100" cy="100" rx="80" ry="50" fill="#3b82f6" />

    <ellipse cx="250" cy="100" rx="50" ry="80" fill="#10b981" />
</svg>
```

### Line

```html
<svg width="300" height="200">
    <!-- x1, y1 = start point, x2, y2 = end point -->
    <line x1="10" y1="10" x2="290" y2="190"
          stroke="#3b82f6" stroke-width="3" />

    <line x1="10" y1="190" x2="290" y2="10"
          stroke="#ef4444" stroke-width="3" stroke-dasharray="5,5" />
</svg>
```

### Polyline

```html
<svg width="400" height="200">
    <!-- Series of connected points -->
    <polyline points="10,100 50,50 100,80 150,40 200,60 250,30 300,50"
              fill="none" stroke="#3b82f6" stroke-width="3" />

    <!-- Filled polyline -->
    <polyline points="10,150 50,180 100,160 150,190 200,170"
              fill="#10b981" fill-opacity="0.3"
              stroke="#10b981" stroke-width="2" />
</svg>
```

### Polygon

```html
<svg width="400" height="200">
    <!-- Triangle -->
    <polygon points="50,150 100,50 150,150"
             fill="#3b82f6" />

    <!-- Pentagon -->
    <polygon points="250,50 290,90 270,140 230,140 210,90"
             fill="#10b981" stroke="#065f46" stroke-width="2" />
</svg>
```

---

## SVG Paths

The `<path>` element is the most powerful SVG shape.

### Path Commands

```html
<svg width="400" height="300">
    <!-- M = Move to, L = Line to, Z = Close path -->
    <path d="M 50 50 L 150 50 L 100 150 Z"
          fill="#3b82f6" />

    <!-- C = Cubic Bezier curve -->
    <path d="M 200 50 C 200 100, 300 100, 300 50"
          fill="none" stroke="#10b981" stroke-width="3" />

    <!-- Q = Quadratic Bezier curve -->
    <path d="M 50 200 Q 100 150, 150 200"
          fill="none" stroke="#f59e0b" stroke-width="3" />
</svg>
```

### Common Path Patterns

```html
<svg width="400" height="300">
    <!-- Arrow -->
    <path d="M 50 100 L 150 100 L 150 80 L 180 110 L 150 140 L 150 120 L 50 120 Z"
          fill="#3b82f6" />

    <!-- Heart -->
    <path d="M 250,130 C 250,120 240,110 230,110 C 220,110 210,120 210,130 C 210,120 200,110 190,110 C 180,110 170,120 170,130 C 170,150 210,180 210,180 C 210,180 250,150 250,130 Z"
          fill="#ef4444" />
</svg>
```

---

## SVG Text

### Basic Text

```html
<svg width="400" height="200">
    <!-- Simple text -->
    <text x="50" y="50" font-size="20" fill="#1e40af">
        Hello, SVG!
    </text>

    <!-- Styled text -->
    <text x="50" y="100" font-size="24" font-weight="bold"
          font-family="Arial" fill="#2563eb">
        Bold Text
    </text>

    <!-- Text with stroke -->
    <text x="50" y="150" font-size="30" fill="none"
          stroke="#3b82f6" stroke-width="1">
        Outlined Text
    </text>
</svg>
```

### Text Alignment

```html
<svg width="400" height="200">
    <!-- Left aligned (default) -->
    <text x="200" y="50" text-anchor="start" fill="#666">
        Left aligned
    </text>

    <!-- Center aligned -->
    <text x="200" y="100" text-anchor="middle" fill="#666">
        Center aligned
    </text>

    <!-- Right aligned -->
    <text x="200" y="150" text-anchor="end" fill="#666">
        Right aligned
    </text>

    <!-- Vertical reference line -->
    <line x1="200" y1="20" x2="200" y2="180"
          stroke="#e5e7eb" stroke-width="1" />
</svg>
```

---

## SVG Styling

### Fill and Stroke

```css
.shape {
    fill: #3b82f6;              /* Fill color */
    stroke: #1e40af;            /* Border color */
    stroke-width: 3;            /* Border width */
    stroke-dasharray: 5, 5;     /* Dashed line */
    stroke-linecap: round;      /* Line end style */
    opacity: 0.8;               /* Transparency */
    fill-opacity: 0.5;          /* Fill transparency only */
    stroke-opacity: 0.8;        /* Stroke transparency only */
}
```

```html
<svg width="400" height="200">
    <rect x="50" y="50" width="100" height="100" class="shape" />
</svg>
```

---

## Data Binding in SVG

### Dynamic Values

{% raw %}
```html
<svg width="400" height="300">
    <!-- Bind dimensions -->
    <rect x="50" y="50"
          width="{{barWidth}}"
          height="{{barHeight}}"
          fill="#3b82f6" />

    <!-- Bind colors -->
    <circle cx="200" cy="100" r="50"
            fill="{{circleColor}}" />

    <!-- Bind text -->
    <text x="200" y="200" text-anchor="middle" font-size="16">
        {{displayText}}
    </text>
</svg>
```
{% endraw %}

### Calculated Positions

{% raw %}
```html
<svg width="500" height="300">
    {{#each dataPoints}}
    <!-- Calculate x position based on index -->
    <rect x="{{calc(@index, '*', 50, '+', 10)}}"
          y="{{calc(250, '-', this.value)}}"
          width="40"
          height="{{this.value}}"
          fill="#3b82f6" />

    <!-- Label below each bar -->
    <text x="{{calc(@index, '*', 50, '+', 30)}}"
          y="280"
          text-anchor="middle"
          font-size="12">
        {{this.label}}
    </text>
    {{/each}}
</svg>
```
{% endraw %}

---

## Practical Examples

### Example 1: Bar Chart

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Bar Chart</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            margin: 0;
        }

        h1 {
            font-size: 24pt;
            text-align: center;
            margin: 0 0 30pt 0;
            color: #1e40af;
        }

        .chart-container {
            text-align: center;
            page-break-inside: avoid;
        }

        .chart-title {
            font-size: 16pt;
            font-weight: 600;
            margin-bottom: 15pt;
        }

        /* ==============================================
           SVG CHART STYLES
           ============================================== */
        .bar {
            fill: #3b82f6;
            transition: fill 0.3s;
        }

        .bar-label {
            font-size: 12pt;
            fill: #666;
        }

        .bar-value {
            font-size: 11pt;
            fill: #1e40af;
            font-weight: 600;
        }

        .axis-line {
            stroke: #9ca3af;
            stroke-width: 2;
        }

        .grid-line {
            stroke: #e5e7eb;
            stroke-width: 1;
        }
    </style>
</head>
<body>
    <h1>Quarterly Sales Report</h1>

    <div class="chart-container">
        <div class="chart-title">Revenue by Quarter ($1000s)</div>

        <svg width="600" height="400" viewBox="0 0 600 400">
            <!-- Grid lines -->
            <line x1="50" y1="50" x2="550" y2="50" class="grid-line" />
            <line x1="50" y1="125" x2="550" y2="125" class="grid-line" />
            <line x1="50" y1="200" x2="550" y2="200" class="grid-line" />
            <line x1="50" y1="275" x2="550" y2="275" class="grid-line" />

            <!-- Axes -->
            <line x1="50" y1="50" x2="50" y2="350" class="axis-line" />
            <line x1="50" y1="350" x2="550" y2="350" class="axis-line" />

            <!-- Bars -->
            {{#each quarters}}
            <rect x="{{calc(@index, '*', 120, '+', 80)}}"
                  y="{{calc(350, '-', this.revenue)}}"
                  width="80"
                  height="{{this.revenue}}"
                  class="bar" />

            <!-- Value labels on bars -->
            <text x="{{calc(@index, '*', 120, '+', 120)}}"
                  y="{{calc(350, '-', this.revenue, '-', 10)}}"
                  text-anchor="middle"
                  class="bar-value">
                ${{this.revenue}}
            </text>

            <!-- Quarter labels -->
            <text x="{{calc(@index, '*', 120, '+', 120)}}"
                  y="370"
                  text-anchor="middle"
                  class="bar-label">
                Q{{calc(@index, '+', 1)}}
            </text>
            {{/each}}

            <!-- Y-axis labels -->
            <text x="40" y="355" text-anchor="end" font-size="10" fill="#666">0</text>
            <text x="40" y="280" text-anchor="end" font-size="10" fill="#666">75</text>
            <text x="40" y="205" text-anchor="end" font-size="10" fill="#666">150</text>
            <text x="40" y="130" text-anchor="end" font-size="10" fill="#666">225</text>
            <text x="40" y="55" text-anchor="end" font-size="10" fill="#666">300</text>
        </svg>
    </div>
</body>
</html>
```
{% endraw %}

### Example 2: Line Chart

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Line Chart</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            margin: 0;
        }

        h1 {
            font-size: 24pt;
            text-align: center;
            margin: 0 0 30pt 0;
            color: #1e40af;
        }

        .chart-container {
            text-align: center;
        }

        /* ==============================================
           LINE CHART STYLES
           ============================================== */
        .data-line {
            fill: none;
            stroke: #3b82f6;
            stroke-width: 3;
            stroke-linecap: round;
            stroke-linejoin: round;
        }

        .data-point {
            fill: #3b82f6;
        }

        .data-point-label {
            font-size: 10pt;
            fill: #1e40af;
            font-weight: 600;
        }

        .axis {
            stroke: #9ca3af;
            stroke-width: 2;
        }

        .grid {
            stroke: #e5e7eb;
            stroke-width: 1;
        }
    </style>
</head>
<body>
    <h1>Monthly Growth Trend</h1>

    <div class="chart-container">
        <svg width="700" height="400" viewBox="0 0 700 400">
            <!-- Grid -->
            <line x1="50" y1="50" x2="650" y2="50" class="grid" />
            <line x1="50" y1="125" x2="650" y2="125" class="grid" />
            <line x1="50" y1="200" x2="650" y2="200" class="grid" />
            <line x1="50" y1="275" x2="650" y2="275" class="grid" />
            <line x1="50" y1="350" x2="650" y2="350" class="grid" />

            <!-- Axes -->
            <line x1="50" y1="50" x2="50" y2="350" class="axis" />
            <line x1="50" y1="350" x2="650" y2="350" class="axis" />

            <!-- Build path data for line -->
            <polyline class="data-line">
                <param name="points">
                    {{#each months}}
                    {{calc(@index, '*', 100, '+', 75)}},{{calc(350, '-', this.value, '*', 2.5)}}{{#unless @last}} {{/unless}}
                    {{/each}}
                </param>
            </polyline>

            <!-- Data points and labels -->
            {{#each months}}
            <!-- Point -->
            <circle cx="{{calc(@index, '*', 100, '+', 75)}}"
                    cy="{{calc(350, '-', this.value, '*', 2.5)}}"
                    r="5"
                    class="data-point" />

            <!-- Value label -->
            <text x="{{calc(@index, '*', 100, '+', 75)}}"
                  y="{{calc(350, '-', this.value, '*', 2.5, '-', 15)}}"
                  text-anchor="middle"
                  class="data-point-label">
                {{this.value}}
            </text>

            <!-- Month label -->
            <text x="{{calc(@index, '*', 100, '+', 75)}}"
                  y="370"
                  text-anchor="middle"
                  font-size="11"
                  fill="#666">
                {{this.name}}
            </text>
            {{/each}}

            <!-- Y-axis labels -->
            <text x="40" y="355" text-anchor="end" font-size="10" fill="#666">0</text>
            <text x="40" y="280" text-anchor="end" font-size="10" fill="#666">30</text>
            <text x="40" y="205" text-anchor="end" font-size="10" fill="#666">60</text>
            <text x="40" y="130" text-anchor="end" font-size="10" fill="#666">90</text>
            <text x="40" y="55" text-anchor="end" font-size="10" fill="#666">120</text>
        </svg>
    </div>
</body>
</html>
```
{% endraw %}

### Example 3: Pie Chart

{% raw %}
```html
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Pie Chart</title>
    <style>
        @page {
            size: Letter;
            margin: 1in;
        }

        body {
            font-family: Helvetica, Arial, sans-serif;
            margin: 0;
        }

        h1 {
            font-size: 24pt;
            text-align: center;
            margin: 0 0 30pt 0;
            color: #1e40af;
        }

        .chart-container {
            text-align: center;
        }

        /* ==============================================
           PIE CHART STYLES
           ============================================== */
        .slice {
            stroke: white;
            stroke-width: 2;
        }

        .slice-label {
            font-size: 14pt;
            font-weight: 600;
            fill: white;
        }

        /* ==============================================
           LEGEND
           ============================================== */
        .legend {
            margin-top: 30pt;
        }

        .legend-item {
            display: inline-block;
            margin: 0 15pt;
            font-size: 11pt;
        }

        .legend-color {
            display: inline-block;
            width: 15pt;
            height: 15pt;
            vertical-align: middle;
            margin-right: 5pt;
            border-radius: 2pt;
        }
    </style>
</head>
<body>
    <h1>Market Share Distribution</h1>

    <div class="chart-container">
        <svg width="500" height="500" viewBox="0 0 500 500">
            <!-- Simple pie chart using circles (simplified approach) -->
            <!-- For accurate pie charts, you'd calculate arc paths -->

            <!-- Product A - 40% (top-right quadrant) -->
            <path d="M 250 250 L 250 100 A 150 150 0 0 1 400 250 Z"
                  fill="#3b82f6"
                  class="slice" />
            <text x="320" y="180" class="slice-label">40%</text>

            <!-- Product B - 30% (bottom-right) -->
            <path d="M 250 250 L 400 250 A 150 150 0 0 1 250 400 Z"
                  fill="#10b981"
                  class="slice" />
            <text x="320" y="330" class="slice-label">30%</text>

            <!-- Product C - 20% (bottom-left) -->
            <path d="M 250 250 L 250 400 A 150 150 0 0 1 100 250 Z"
                  fill="#f59e0b"
                  class="slice" />
            <text x="160" y="330" class="slice-label">20%</text>

            <!-- Product D - 10% (top-left) -->
            <path d="M 250 250 L 100 250 A 150 150 0 0 1 250 100 Z"
                  fill="#ef4444"
                  class="slice" />
            <text x="180" y="180" class="slice-label">10%</text>
        </svg>

        <!-- Legend -->
        <div class="legend">
            <div class="legend-item">
                <span class="legend-color" style="background-color: #3b82f6;"></span>
                Product A
            </div>
            <div class="legend-item">
                <span class="legend-color" style="background-color: #10b981;"></span>
                Product B
            </div>
            <div class="legend-item">
                <span class="legend-color" style="background-color: #f59e0b;"></span>
                Product C
            </div>
            <div class="legend-item">
                <span class="legend-color" style="background-color: #ef4444;"></span>
                Product D
            </div>
        </div>
    </div>
</body>
</html>
```
{% endraw %}

---

## Try It Yourself

### Exercise 1: Progress Indicator

Create a circular progress indicator:
- Use SVG circle with stroke-dasharray
- Show percentage in center as text
- Make progress value data-bound
- Add color coding (green >75%, yellow >50%, red <50%)

### Exercise 2: Dashboard Gauges

Build a dashboard with 3-4 gauges:
- Semi-circular gauge design
- Needle or pointer
- Value labels
- Min/max markers

### Exercise 3: Multi-Series Chart

Create a chart comparing 2-3 data series:
- Line or bar chart
- Multiple colors for series
- Legend
- Data binding for all series

---

## Common Pitfalls

### ❌ Incorrect Coordinate Math

```html
<!-- Hard-coded positions don't scale -->
<rect x="10" y="10" width="50" height="100" fill="blue" />
<rect x="70" y="10" width="50" height="150" fill="green" />
```

✅ **Solution:**

{% raw %}
```html
<!-- Calculate positions dynamically -->
{{#each data}}
<rect x="{{calc(@index, '*', 60, '+', 10)}}"
      y="10"
      width="50"
      height="{{this.value}}"
      fill="blue" />
{{/each}}
```
{% endraw %}

### ❌ Text Overflow

```html
<!-- Text may overflow viewBox -->
<svg viewBox="0 0 100 100" width="200" height="200">
    <text x="10" y="50" font-size="20">
        This is very long text that overflows
    </text>
</svg>
```

✅ **Solution:**

```html
<svg viewBox="0 0 300 100" width="200" height="67">
    <text x="10" y="50" font-size="20">
        Properly sized viewBox
    </text>
</svg>
```

### ❌ Missing Stroke Width

```html
<!-- Invisible line (no stroke-width) -->
<line x1="0" y1="0" x2="100" y2="100" stroke="black" />
```

✅ **Solution:**

```html
<line x1="0" y1="0" x2="100" y2="100"
      stroke="black" stroke-width="2" />
```

---

## SVG Drawing Checklist

- [ ] viewBox defined for scalability
- [ ] Coordinate system is consistent
- [ ] Colors use CSS or data binding
- [ ] Text is readable (font-size, anchor)
- [ ] Stroke widths specified
- [ ] Data binding calculations tested
- [ ] Chart scales properly with data
- [ ] Renders correctly in PDF

---

## Best Practices

1. **Plan Coordinate System** - Define viewBox dimensions first
2. **Use Data Binding** - Calculate positions dynamically
3. **Consistent Spacing** - Use formulas for bar/point positions
4. **CSS for Styling** - Easier to maintain colors and styles
5. **Test with Real Data** - Varying data sizes and values
6. **Label Clearly** - Axes, values, legends
7. **Simple Paths** - Complex paths may render slowly
8. **Optimize Performance** - Limit number of SVG elements

---

## Next Steps

1. **[Lists](04_lists.md)** - Structured content formatting
2. **[Tables - Basics](05_tables_basics.md)** - Tabular data
3. **[Content Best Practices](08_content_best_practices.md)** - Performance optimization

---

**Continue learning →** [Lists](04_lists.md)
