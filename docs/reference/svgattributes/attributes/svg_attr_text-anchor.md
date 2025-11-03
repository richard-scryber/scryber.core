---
layout: default
title: text-anchor
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @text-anchor : The Text Horizontal Alignment Attribute

The `text-anchor` attribute controls the horizontal alignment of text relative to its x position. It determines whether text starts at, centers on, or ends at the specified x coordinate.

## Usage

The `text-anchor` attribute is used to:
- Align text horizontally (left, center, right)
- Center labels on data points
- Position text at the end of lines
- Create aligned text layouts
- Support data-driven text alignment
- Build responsive text positioning

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="300">
    <!-- Reference line at x=200 -->
    <line x1="200" y1="50" x2="200" y2="250" stroke="#bdc3c7" stroke-width="1"/>

    <!-- Start alignment (left) -->
    <text x="200" y="80" text-anchor="start" fill="#3498db">
        Start Aligned
    </text>

    <!-- Middle alignment (center) -->
    <text x="200" y="150" text-anchor="middle" fill="#e74c3c">
        Middle Aligned
    </text>

    <!-- End alignment (right) -->
    <text x="200" y="220" text-anchor="end" fill="#2ecc71">
        End Aligned
    </text>
</svg>
```

---

## Supported Values

| Value | Description | Alignment | Use Case |
|-------|-------------|-----------|----------|
| `start` | Text starts at x position (default) | Left for LTR, right for RTL | Left-aligned labels |
| `middle` | Text centered on x position | Center | Centered labels, chart points |
| `end` | Text ends at x position | Right for LTR, left for RTL | Right-aligned labels |

### Direction Sensitivity

- `start` and `end` respect text direction (dir attribute or CSS)
- For left-to-right text: `start` = left, `end` = right
- For right-to-left text: `start` = right, `end` = left
- `middle` always centers regardless of direction

---

## Supported Elements

The `text-anchor` attribute is supported on:

- **[&lt;text&gt;](/reference/svgtags/text.html)** - Text element
- **[&lt;tspan&gt;](/reference/svgtags/tspan.html)** - Text span element
- **[&lt;textPath&gt;](/reference/svgtags/textPath.html)** - Text on path element

---

## Data Binding

### Dynamic Text Alignment

Change alignment based on data:

```html
<!-- Model: { labels: [{text: 'Left', align: 'start', y: 80}, {text: 'Center', align: 'middle', y: 150}, {text: 'Right', align: 'end', y: 220}] } -->
<svg width="400" height="300">
    <line x1="200" y1="50" x2="200" y2="250" stroke="#bdc3c7" stroke-width="1"/>

    <template data-bind="{{model.labels}}">
        <text x="200" y="{{.y}}" text-anchor="{{.align}}" fill="#3498db" font-size="16">
            {{.text}}
        </text>
    </template>
</svg>
```

### Chart Labels with Centered Text

Center labels on data points:

```html
<!-- Model: { dataPoints: [{x: 100, y: 150, value: 45}, {x: 200, y: 100, value: 78}, {x: 300, y: 130, value: 62}] } -->
<svg width="400" height="250">
    <template data-bind="{{model.dataPoints}}">
        <!-- Data point -->
        <circle cx="{{.x}}" cy="{{.y}}" r="6" fill="#3498db"/>
        <!-- Centered label -->
        <text x="{{.x}}" y="{{.y - 15}}" text-anchor="middle"
              font-size="14" fill="#2c3e50">{{.value}}</text>
    </template>

    <!-- Baseline -->
    <line x1="50" y1="200" x2="350" y2="200" stroke="#95a5a6" stroke-width="2"/>
</svg>
```

### Position-Dependent Alignment

Align text based on position:

```html
<!-- Model: { items: [{x: 50, y: 100, label: 'Left Edge', position: 'start'}, {x: 200, y: 100, label: 'Center', position: 'middle'}, {x: 350, y: 100, label: 'Right Edge', position: 'end'}] } -->
<svg width="400" height="200">
    <template data-bind="{{model.items}}">
        <circle cx="{{.x}}" cy="{{.y}}" r="4" fill="#e74c3c"/>
        <text x="{{.x}}" y="{{.y + 25}}"
              text-anchor="{{.position}}"
              font-size="12" fill="#2c3e50">{{.label}}</text>
    </template>
</svg>
```

### Responsive Label Positioning

Adjust alignment for different screen positions:

```html
<!-- Model: { labels: [{text: 'Start', x: 50, isLeftEdge: true}, {text: 'Middle', x: 200, isLeftEdge: false}, {text: 'End', x: 350, isLeftEdge: false}] } -->
<svg width="400" height="150">
    <template data-bind="{{model.labels}}">
        <text x="{{.x}}" y="75"
              text-anchor="{{.isLeftEdge ? 'start' : (.x > 250 ? 'end' : 'middle')}}"
              font-size="14" fill="#3498db">{{.text}}</text>
        <circle cx="{{.x}}" cy="75" r="3" fill="#e74c3c"/>
    </template>
</svg>
```

### Data Table with Aligned Columns

Create aligned text columns:

```html
<!-- Model: { rows: [{name: 'Item A', quantity: 45, price: 29.99}, {name: 'Item B', quantity: 12, price: 149.50}] } -->
<svg width="500" height="200">
    <!-- Headers -->
    <text x="50" y="30" text-anchor="start" font-weight="bold" fill="#2c3e50">Name</text>
    <text x="250" y="30" text-anchor="middle" font-weight="bold" fill="#2c3e50">Qty</text>
    <text x="400" y="30" text-anchor="end" font-weight="bold" fill="#2c3e50">Price</text>

    <!-- Data rows -->
    <template data-bind="{{model.rows}}">
        <text x="50" y="{{$index * 40 + 70}}" text-anchor="start" fill="#34495e">{{.name}}</text>
        <text x="250" y="{{$index * 40 + 70}}" text-anchor="middle" fill="#34495e">{{.quantity}}</text>
        <text x="400" y="{{$index * 40 + 70}}" text-anchor="end" fill="#34495e">${{.price}}</text>
    </template>
</svg>
```

---

## Notes

### Alignment Behavior

**start (default):**
- Text begins at x coordinate
- For LTR languages: left-aligned
- For RTL languages: right-aligned
- Most common for general text

**middle:**
- Text centered on x coordinate
- Text extends equally left and right
- Independent of text direction
- Ideal for centered labels

**end:**
- Text ends at x coordinate
- For LTR languages: right-aligned
- For RTL languages: left-aligned
- Good for right-aligned columns

### Visual Examples

```html
<!-- All text positioned at x=200 -->
<text x="200" y="50" text-anchor="start">Start</text>   <!-- Starts at 200 -->
<text x="200" y="100" text-anchor="middle">Middle</text> <!-- Centered on 200 -->
<text x="200" y="150" text-anchor="end">End</text>       <!-- Ends at 200 -->
```

### Coordinate Reference

The x coordinate always refers to the anchor point:
- **start**: x = left edge of text
- **middle**: x = horizontal center of text
- **end**: x = right edge of text

### Combining with Vertical Alignment

Use with `dominant-baseline` for full control:

```html
<text x="200" y="100"
      text-anchor="middle"          <!-- Horizontal center -->
      dominant-baseline="middle">   <!-- Vertical center -->
    Centered Both Ways
</text>
```

### Multi-line Text

Each line respects text-anchor:

```html
<text x="200" y="100" text-anchor="middle">
    <tspan x="200" dy="0">First Line</tspan>
    <tspan x="200" dy="20">Second Line</tspan>
    <tspan x="200" dy="20">Third Line</tspan>
</text>
```

### Common Use Cases

**Chart labels:**
```html
<!-- Center labels on data points -->
<text x="{{point.x}}" y="{{point.y - 10}}" text-anchor="middle">
    {{point.value}}
</text>
```

**Axis labels:**
```html
<!-- Right-align Y-axis labels -->
<text x="{{axisX - 5}}" y="{{tickY}}" text-anchor="end">
    {{tickValue}}
</text>
```

**Button text:**
```html
<!-- Center text in button -->
<text x="{{buttonX + buttonWidth/2}}" y="{{buttonY + buttonHeight/2}}"
      text-anchor="middle" dominant-baseline="middle">
    Click Me
</text>
```

**Table columns:**
```html
<!-- Right-align numbers -->
<text x="{{columnX}}" y="{{rowY}}" text-anchor="end">
    ${{price}}
</text>
```

### Text Bounding Box

- text-anchor affects horizontal extent only
- Doesn't affect vertical positioning
- Text still uses baseline for vertical positioning
- Combine with dominant-baseline for vertical control

### Direction and Language

**Left-to-Right (LTR):**
```html
<text dir="ltr" text-anchor="start">Starts left</text>
<text dir="ltr" text-anchor="end">Ends right</text>
```

**Right-to-Left (RTL):**
```html
<text dir="rtl" text-anchor="start">Starts right</text>
<text dir="rtl" text-anchor="end">Ends left</text>
```

### Performance Considerations

- All three values have similar performance
- No significant rendering overhead
- Can be applied to many text elements
- Doesn't affect text rendering quality

### Browser Compatibility

- Widely supported in all modern browsers
- Consistent behavior across implementations
- Part of SVG 1.1 specification
- Safe to use in production

---

## Examples

### Basic Alignment Demonstration

```html
<svg width="400" height="250">
    <!-- Reference line -->
    <line x1="200" y1="30" x2="200" y2="230" stroke="#bdc3c7" stroke-width="1" stroke-dasharray="5,5"/>
    <circle cx="200" cy="70" r="3" fill="#e74c3c"/>
    <circle cx="200" cy="140" r="3" fill="#e74c3c"/>
    <circle cx="200" cy="210" r="3" fill="#e74c3c"/>

    <!-- Start aligned -->
    <text x="200" y="75" text-anchor="start" font-size="16" fill="#3498db">
        Start Aligned
    </text>

    <!-- Middle aligned -->
    <text x="200" y="145" text-anchor="middle" font-size="16" fill="#2ecc71">
        Middle Aligned
    </text>

    <!-- End aligned -->
    <text x="200" y="215" text-anchor="end" font-size="16" fill="#e74c3c">
        End Aligned
    </text>
</svg>
```

### Chart Labels Centered on Points

```html
<svg width="500" height="300">
    <defs>
        <linearGradient id="chartGrad" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db"/>
            <stop offset="100%" stop-color="#2980b9"/>
        </linearGradient>
    </defs>

    <!-- Data bars -->
    <rect x="50" y="150" width="60" height="100" fill="url(#chartGrad)"/>
    <rect x="150" y="100" width="60" height="150" fill="url(#chartGrad)"/>
    <rect x="250" y="120" width="60" height="130" fill="url(#chartGrad)"/>
    <rect x="350" y="80" width="60" height="170" fill="url(#chartGrad)"/>

    <!-- Centered value labels -->
    <text x="80" y="140" text-anchor="middle" font-size="14" font-weight="bold" fill="#2c3e50">45</text>
    <text x="180" y="90" text-anchor="middle" font-size="14" font-weight="bold" fill="#2c3e50">78</text>
    <text x="280" y="110" text-anchor="middle" font-size="14" font-weight="bold" fill="#2c3e50">62</text>
    <text x="380" y="70" text-anchor="middle" font-size="14" font-weight="bold" fill="#2c3e50">89</text>

    <!-- Baseline -->
    <line x1="30" y1="250" x2="430" y2="250" stroke="#34495e" stroke-width="2"/>

    <!-- Centered category labels -->
    <text x="80" y="270" text-anchor="middle" font-size="12" fill="#7f8c8d">Q1</text>
    <text x="180" y="270" text-anchor="middle" font-size="12" fill="#7f8c8d">Q2</text>
    <text x="280" y="270" text-anchor="middle" font-size="12" fill="#7f8c8d">Q3</text>
    <text x="380" y="270" text-anchor="middle" font-size="12" fill="#7f8c8d">Q4</text>
</svg>
```

### Table with Aligned Columns

```html
<svg width="600" height="250">
    <!-- Table structure -->
    <rect x="30" y="20" width="540" height="200" fill="none" stroke="#bdc3c7" stroke-width="2"/>
    <line x1="30" y1="60" x2="570" y2="60" stroke="#bdc3c7" stroke-width="1"/>

    <!-- Headers -->
    <text x="50" y="45" text-anchor="start" font-weight="bold" font-size="14" fill="#2c3e50">Product</text>
    <text x="300" y="45" text-anchor="middle" font-weight="bold" font-size="14" fill="#2c3e50">Quantity</text>
    <text x="450" y="45" text-anchor="end" font-weight="bold" font-size="14" fill="#2c3e50">Price</text>

    <!-- Row 1 -->
    <text x="50" y="90" text-anchor="start" font-size="12" fill="#34495e">Widget A</text>
    <text x="300" y="90" text-anchor="middle" font-size="12" fill="#34495e">150</text>
    <text x="450" y="90" text-anchor="end" font-size="12" fill="#34495e">$24.99</text>

    <!-- Row 2 -->
    <text x="50" y="120" text-anchor="start" font-size="12" fill="#34495e">Widget B</text>
    <text x="300" y="120" text-anchor="middle" font-size="12" fill="#34495e">87</text>
    <text x="450" y="120" text-anchor="end" font-size="12" fill="#34495e">$149.50</text>

    <!-- Row 3 -->
    <text x="50" y="150" text-anchor="start" font-size="12" fill="#34495e">Widget C</text>
    <text x="300" y="150" text-anchor="middle" font-size="12" fill="#34495e">203</text>
    <text x="450" y="150" text-anchor="end" font-size="12" fill="#34495e">$12.75</text>

    <!-- Row 4 -->
    <text x="50" y="180" text-anchor="start" font-size="12" fill="#34495e">Widget D</text>
    <text x="300" y="180" text-anchor="middle" font-size="12" fill="#34495e">45</text>
    <text x="450" y="180" text-anchor="end" font-size="12" fill="#34495e">$299.99</text>

    <!-- Total -->
    <line x1="30" y1="195" x2="570" y2="195" stroke="#bdc3c7" stroke-width="1"/>
    <text x="450" y="212" text-anchor="end" font-weight="bold" font-size="12" fill="#2c3e50">$487.23</text>
</svg>
```

### Button Labels (Centered)

```html
<svg width="600" height="150">
    <defs>
        <linearGradient id="btnGrad1" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#3498db"/>
            <stop offset="100%" stop-color="#2980b9"/>
        </linearGradient>
        <linearGradient id="btnGrad2" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#2ecc71"/>
            <stop offset="100%" stop-color="#27ae60"/>
        </linearGradient>
        <linearGradient id="btnGrad3" x1="0%" y1="0%" x2="0%" y2="100%">
            <stop offset="0%" stop-color="#e74c3c"/>
            <stop offset="100%" stop-color="#c0392b"/>
        </linearGradient>
    </defs>

    <!-- Button 1 -->
    <rect x="50" y="50" width="140" height="50" rx="8" fill="url(#btnGrad1)"/>
    <text x="120" y="80" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="16" font-weight="bold">Save</text>

    <!-- Button 2 -->
    <rect x="230" y="50" width="140" height="50" rx="8" fill="url(#btnGrad2)"/>
    <text x="300" y="80" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="16" font-weight="bold">Submit</text>

    <!-- Button 3 -->
    <rect x="410" y="50" width="140" height="50" rx="8" fill="url(#btnGrad3)"/>
    <text x="480" y="80" text-anchor="middle" dominant-baseline="middle"
          fill="white" font-size="16" font-weight="bold">Cancel</text>
</svg>
```

### Axis Labels

```html
<svg width="500" height="350">
    <!-- Chart area -->
    <rect x="80" y="50" width="380" height="250" fill="#ecf0f1"/>

    <!-- Y-axis with right-aligned labels -->
    <line x1="80" y1="50" x2="80" y2="300" stroke="#2c3e50" stroke-width="2"/>
    <text x="75" y="300" text-anchor="end" font-size="12" fill="#2c3e50">0</text>
    <text x="75" y="237.5" text-anchor="end" font-size="12" fill="#2c3e50">25</text>
    <text x="75" y="175" text-anchor="end" font-size="12" fill="#2c3e50">50</text>
    <text x="75" y="112.5" text-anchor="end" font-size="12" fill="#2c3e50">75</text>
    <text x="75" y="50" text-anchor="end" font-size="12" fill="#2c3e50">100</text>

    <!-- X-axis with centered labels -->
    <line x1="80" y1="300" x2="460" y2="300" stroke="#2c3e50" stroke-width="2"/>
    <text x="175" y="320" text-anchor="middle" font-size="12" fill="#2c3e50">Jan</text>
    <text x="270" y="320" text-anchor="middle" font-size="12" fill="#2c3e50">Feb</text>
    <text x="365" y="320" text-anchor="middle" font-size="12" fill="#2c3e50">Mar</text>

    <!-- Axis titles (centered) -->
    <text x="270" y="345" text-anchor="middle" font-size="14" font-weight="bold" fill="#2c3e50">Month</text>
    <text x="40" y="175" text-anchor="middle" font-size="14" font-weight="bold" fill="#2c3e50"
          transform="rotate(-90 40 175)">Value</text>
</svg>
```

### Gauge Labels

```html
<svg width="400" height="300" viewBox="0 0 400 300">
    <!-- Gauge arc -->
    <path d="M 50,200 A 150,150 0 0,1 350,200"
          fill="none" stroke="#ecf0f1" stroke-width="30" stroke-linecap="round"/>
    <path d="M 50,200 A 150,150 0 0,1 200,65"
          fill="none" stroke="#3498db" stroke-width="30" stroke-linecap="round"/>

    <!-- Gauge value (centered) -->
    <text x="200" y="220" text-anchor="middle" font-size="48" font-weight="bold" fill="#2c3e50">
        65
    </text>
    <text x="200" y="245" text-anchor="middle" font-size="16" fill="#7f8c8d">
        Current Value
    </text>

    <!-- Min/Max labels -->
    <text x="50" y="230" text-anchor="start" font-size="14" fill="#95a5a6">0</text>
    <text x="350" y="230" text-anchor="end" font-size="14" fill="#95a5a6">100</text>
</svg>
```

### Callout Labels

```html
<svg width="500" height="300">
    <!-- Main shape -->
    <circle cx="250" cy="150" r="60" fill="#3498db" opacity="0.5"/>

    <!-- Callout 1 (left - end aligned) -->
    <line x1="195" y1="125" x2="100" y2="50" stroke="#2c3e50" stroke-width="2"/>
    <circle cx="100" cy="50" r="3" fill="#e74c3c"/>
    <text x="95" y="45" text-anchor="end" font-size="12" fill="#2c3e50">Feature A</text>

    <!-- Callout 2 (right - start aligned) -->
    <line x1="305" y1="125" x2="400" y2="50" stroke="#2c3e50" stroke-width="2"/>
    <circle cx="400" cy="50" r="3" fill="#e74c3c"/>
    <text x="405" y="45" text-anchor="start" font-size="12" fill="#2c3e50">Feature B</text>

    <!-- Callout 3 (top - middle aligned) -->
    <line x1="250" y1="90" x2="250" y2="20" stroke="#2c3e50" stroke-width="2"/>
    <circle cx="250" cy="20" r="3" fill="#e74c3c"/>
    <text x="250" y="12" text-anchor="middle" font-size="12" fill="#2c3e50">Feature C</text>

    <!-- Callout 4 (bottom - middle aligned) -->
    <line x1="250" y1="210" x2="250" y2="270" stroke="#2c3e50" stroke-width="2"/>
    <circle cx="250" cy="270" r="3" fill="#e74c3c"/>
    <text x="250" y="290" text-anchor="middle" font-size="12" fill="#2c3e50">Feature D</text>
</svg>
```

### Timeline Events

```html
<svg width="600" height="200">
    <!-- Timeline line -->
    <line x1="50" y1="100" x2="550" y2="100" stroke="#3498db" stroke-width="4"/>

    <!-- Event 1 -->
    <circle cx="100" cy="100" r="8" fill="#e74c3c"/>
    <text x="100" y="80" text-anchor="middle" font-size="12" font-weight="bold" fill="#2c3e50">
        2020
    </text>
    <text x="100" y="130" text-anchor="middle" font-size="10" fill="#7f8c8d">
        Launch
    </text>

    <!-- Event 2 -->
    <circle cx="250" cy="100" r="8" fill="#2ecc71"/>
    <text x="250" y="80" text-anchor="middle" font-size="12" font-weight="bold" fill="#2c3e50">
        2021
    </text>
    <text x="250" y="130" text-anchor="middle" font-size="10" fill="#7f8c8d">
        Growth
    </text>

    <!-- Event 3 -->
    <circle cx="400" cy="100" r="8" fill="#f39c12"/>
    <text x="400" y="80" text-anchor="middle" font-size="12" font-weight="bold" fill="#2c3e50">
        2022
    </text>
    <text x="400" y="130" text-anchor="middle" font-size="10" fill="#7f8c8d">
        Expansion
    </text>

    <!-- Event 4 -->
    <circle cx="500" cy="100" r="8" fill="#9b59b6"/>
    <text x="500" y="80" text-anchor="middle" font-size="12" font-weight="bold" fill="#2c3e50">
        2023
    </text>
    <text x="500" y="130" text-anchor="middle" font-size="10" fill="#7f8c8d">
        Innovation
    </text>

    <!-- Start marker -->
    <circle cx="50" cy="100" r="5" fill="#34495e"/>
    <text x="50" y="150" text-anchor="middle" font-size="10" fill="#95a5a6">Start</text>

    <!-- End marker -->
    <circle cx="550" cy="100" r="5" fill="#34495e"/>
    <text x="550" y="150" text-anchor="middle" font-size="10" fill="#95a5a6">Now</text>
</svg>
```

### Pie Chart Labels

```html
<svg width="400" height="400" viewBox="0 0 400 400">
    <defs>
        <radialGradient id="slice1" cx="50%" cy="50%" r="50%">
            <stop offset="70%" stop-color="#3498db"/>
            <stop offset="100%" stop-color="#2980b9"/>
        </radialGradient>
        <radialGradient id="slice2" cx="50%" cy="50%" r="50%">
            <stop offset="70%" stop-color="#e74c3c"/>
            <stop offset="100%" stop-color="#c0392b"/>
        </radialGradient>
        <radialGradient id="slice3" cx="50%" cy="50%" r="50%">
            <stop offset="70%" stop-color="#2ecc71"/>
            <stop offset="100%" stop-color="#27ae60"/>
        </radialGradient>
    </defs>

    <!-- Pie slices -->
    <path d="M 200,200 L 200,50 A 150,150 0 0,1 330.4,94.1 Z" fill="url(#slice1)"/>
    <path d="M 200,200 L 330.4,94.1 A 150,150 0 0,1 269.6,305.9 Z" fill="url(#slice2)"/>
    <path d="M 200,200 L 269.6,305.9 A 150,150 0 0,1 200,50 Z" fill="url(#slice3)"/>

    <!-- Centered percentage labels -->
    <text x="240" y="110" text-anchor="middle" font-size="20" font-weight="bold" fill="white">
        25%
    </text>
    <text x="280" y="200" text-anchor="middle" font-size="20" font-weight="bold" fill="white">
        35%
    </text>
    <text x="190" y="280" text-anchor="middle" font-size="20" font-weight="bold" fill="white">
        40%
    </text>
</svg>
```

### Badge Labels

```html
<svg width="500" height="200">
    <!-- Badge 1 -->
    <circle cx="100" cy="100" r="50" fill="#3498db"/>
    <text x="100" y="95" text-anchor="middle" font-size="24" font-weight="bold" fill="white">
        45
    </text>
    <text x="100" y="115" text-anchor="middle" font-size="12" fill="white">
        New
    </text>

    <!-- Badge 2 -->
    <circle cx="250" cy="100" r="50" fill="#2ecc71"/>
    <text x="250" y="95" text-anchor="middle" font-size="24" font-weight="bold" fill="white">
        12
    </text>
    <text x="250" y="115" text-anchor="middle" font-size="12" fill="white">
        Active
    </text>

    <!-- Badge 3 -->
    <circle cx="400" cy="100" r="50" fill="#e74c3c"/>
    <text x="400" y="95" text-anchor="middle" font-size="24" font-weight="bold" fill="white">
        3
    </text>
    <text x="400" y="115" text-anchor="middle" font-size="12" fill="white">
        Alerts
    </text>
</svg>
```

### Map Labels

```html
<svg width="600" height="400">
    <!-- Map background -->
    <rect width="600" height="400" fill="#e8f4f8"/>

    <!-- City markers and labels -->
    <circle cx="150" cy="120" r="6" fill="#e74c3c"/>
    <text x="150" y="110" text-anchor="middle" font-size="12" font-weight="bold" fill="#2c3e50">
        New York
    </text>

    <circle cx="350" cy="180" r="6" fill="#3498db"/>
    <text x="350" y="170" text-anchor="middle" font-size="12" font-weight="bold" fill="#2c3e50">
        Chicago
    </text>

    <circle cx="480" cy="250" r="6" fill="#2ecc71"/>
    <text x="480" y="240" text-anchor="middle" font-size="12" font-weight="bold" fill="#2c3e50">
        Miami
    </text>

    <circle cx="100" cy="280" r="6" fill="#f39c12"/>
    <text x="100" y="270" text-anchor="middle" font-size="12" font-weight="bold" fill="#2c3e50">
        Los Angeles
    </text>
</svg>
```

### Progress Indicator

```html
<svg width="400" height="150">
    <!-- Progress bar background -->
    <rect x="50" y="50" width="300" height="30" rx="15" fill="#ecf0f1"/>
    <!-- Progress bar fill -->
    <rect x="50" y="50" width="210" height="30" rx="15" fill="#3498db"/>

    <!-- Centered percentage -->
    <text x="200" y="70" text-anchor="middle" dominant-baseline="middle"
          font-size="16" font-weight="bold" fill="white">
        70%
    </text>

    <!-- Start label -->
    <text x="50" y="100" text-anchor="start" font-size="12" fill="#7f8c8d">
        0%
    </text>

    <!-- End label -->
    <text x="350" y="100" text-anchor="end" font-size="12" fill="#7f8c8d">
        100%
    </text>
</svg>
```

### Social Media Icons with Counts

```html
<svg width="500" height="150">
    <!-- Like -->
    <circle cx="100" cy="75" r="30" fill="#3498db"/>
    <text x="100" y="72" text-anchor="middle" font-size="24" fill="white">♥</text>
    <text x="100" y="120" text-anchor="middle" font-size="14" font-weight="bold" fill="#2c3e50">
        1.2K
    </text>

    <!-- Share -->
    <circle cx="250" cy="75" r="30" fill="#2ecc71"/>
    <text x="250" y="78" text-anchor="middle" font-size="24" fill="white">⇪</text>
    <text x="250" y="120" text-anchor="middle" font-size="14" font-weight="bold" fill="#2c3e50">
        345
    </text>

    <!-- Comment -->
    <circle cx="400" cy="75" r="30" fill="#e74c3c"/>
    <text x="400" y="78" text-anchor="middle" font-size="24" fill="white">💬</text>
    <text x="400" y="120" text-anchor="middle" font-size="14" font-weight="bold" fill="#2c3e50">
        89
    </text>
</svg>
```

### Card Layout with Mixed Alignment

```html
<svg width="300" height="400">
    <!-- Card -->
    <rect x="20" y="20" width="260" height="360" rx="10"
          fill="white" stroke="#bdc3c7" stroke-width="2"/>

    <!-- Image placeholder -->
    <rect x="30" y="30" width="240" height="150" rx="5" fill="#ecf0f1"/>

    <!-- Centered title -->
    <text x="150" y="210" text-anchor="middle" font-size="20" font-weight="bold" fill="#2c3e50">
        Product Name
    </text>

    <!-- Centered description -->
    <text x="150" y="240" text-anchor="middle" font-size="12" fill="#7f8c8d">
        Premium quality item
    </text>

    <!-- Left-aligned details -->
    <text x="40" y="280" text-anchor="start" font-size="12" fill="#34495e">
        SKU: ABC-123
    </text>
    <text x="40" y="300" text-anchor="start" font-size="12" fill="#34495e">
        Stock: 45 units
    </text>

    <!-- Right-aligned price -->
    <text x="260" y="340" text-anchor="end" font-size="24" font-weight="bold" fill="#e74c3c">
        $99.99
    </text>
</svg>
```

---

## See Also

- [text](/reference/svgtags/text.html) - Text element
- [tspan](/reference/svgtags/tspan.html) - Text span element
- [dominant-baseline](/reference/svgattributes/dominant-baseline.html) - Vertical text alignment
- [x](/reference/svgattributes/x.html), [y](/reference/svgattributes/y.html) - Text position coordinates
- [font-size](/reference/svgattributes/font-size.html) - Text size
- [fill](/reference/svgattributes/fill.html) - Text color
- [Data Binding](/reference/binding/) - Data binding and expressions

---
