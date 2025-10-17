---
layout: default
title: text (SVG)
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;text&gt; : The SVG Text Element

The `<text>` element is used to render text content in SVG graphics. It provides precise positioning and styling control for labels, annotations, and textual information within vector graphics and charts.

---

## Summary

The `<text>` element enables the display of text at specific coordinates within an SVG coordinate system. It supports standard font styling, fill colors, transformations, and text anchoring for flexible positioning. Text can contain plain content or be structured with nested `<tspan>` elements for advanced formatting.

Key features:
- Precise x, y coordinate positioning
- Font family, size, and style control
- Text anchoring (start, middle, end)
- Baseline alignment options
- Fill colors and transformations
- Support for nested tspan elements
- Data binding for dynamic text content

---

## Usage

The `<text>` element is placed within an SVG container and positioned using x and y coordinates:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="100">
    <text x="10" y="30" font-family="Arial" font-size="16" fill="#333">
        Hello, World!
    </text>
</svg>
```

### Basic Syntax

```html
<!-- Simple text at coordinates -->
<text x="50" y="50">Label Text</text>

<!-- With styling attributes -->
<text x="100" y="75" font-family="Helvetica" font-size="14" fill="blue">
    Styled Text
</text>

<!-- With text-anchor alignment -->
<text x="150" y="100" text-anchor="middle" fill="#336699">
    Centered Text
</text>

<!-- With transformation -->
<text x="50" y="50" transform="rotate(45 50 50)" fill="red">
    Rotated Text
</text>
```

---

## Supported Attributes

### Position Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `x` | Unit | Horizontal position of the text baseline start | 0 |
| `y` | Unit | Vertical position of the text baseline | 0 |
| `dx` | Unit | Relative horizontal offset from current position | 0 |
| `dy` | Unit | Relative vertical offset from current position | 0 |

### Text Alignment Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `text-anchor` | Enum | Horizontal alignment: `start`, `middle`, `end` | start |
| `dominant-baseline` | Enum | Vertical baseline alignment: `auto`, `middle`, `hanging`, `central` | auto |

### Font Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `font-family` | FontSelector | Font family name or list | System default |
| `font-size` | Unit | Font size | 12pt |
| `font-weight` | Integer | Font weight (100-900) | 400 |
| `font-style` | Enum | Font style: `normal`, `italic`, `oblique` | normal |

### Appearance Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `fill` | Color/URL | Fill color or paint server reference | black |
| `fill-opacity` | Double | Fill opacity (0.0-1.0) | 1.0 |
| `stroke` | Color | Stroke color for text outline | none |
| `stroke-width` | Unit | Width of text stroke | 0 |

### Text Formatting Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `text-decoration` | String | Text decoration: `none`, `underline`, `overline`, `line-through` | none |
| `letter-spacing` | Unit | Additional spacing between characters | 0 |
| `textLength` | Unit | Target length for text rendering | auto |
| `lengthAdjust` | Enum | Length adjustment method: `spacing`, `spacingAndGlyphs` | spacing |

### Transform Attributes

| Attribute | Type | Description | Default |
|-----------|------|-------------|---------|
| `transform` | Transform | Transformation operations (translate, rotate, scale, etc.) | none |

### Common Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | String | Unique identifier for the element |
| `class` | String | CSS class name(s) for styling |
| `style` | Style | Inline CSS-style properties |
| `title` | String | Tooltip or title text |
| `desc` | String | Description for accessibility |

---

## Data Binding

The `<text>` element supports comprehensive data binding for dynamic text content and styling.

### Basic Text Binding

```html
<!-- Simple value binding -->
<text x="50" y="50" font-size="16">{{model.customerName}}</text>

<!-- Expression binding -->
<text x="50" y="80" fill="blue">Total: ${{model.total.toFixed(2)}}</text>

<!-- Conditional text -->
<text x="50" y="110" fill="{{model.status === 'active' ? 'green' : 'red'}}">
    Status: {{model.status}}
</text>
```

### Dynamic Positioning

```html
<!-- Data-driven coordinates -->
<text x="{{model.labelX}}" y="{{model.labelY}}" font-size="12">
    {{model.labelText}}
</text>

<!-- Calculated positions -->
<text x="{{model.chartWidth / 2}}" y="30" text-anchor="middle" font-weight="700">
    {{model.chartTitle}}
</text>
```

### Dynamic Styling

```html
<!-- Conditional fill color -->
<text x="100" y="50" fill="{{model.value > 100 ? '#00aa00' : '#aa0000'}}" font-size="20">
    {{model.value}}
</text>

<!-- Data-driven font size -->
<text x="50" y="50" font-size="{{model.importance * 4 + 12}}">
    {{model.message}}
</text>

<!-- Opacity based on data -->
<text x="75" y="75" fill="#336699" fill-opacity="{{model.confidence}}">
    Confidence: {{(model.confidence * 100).toFixed(0)}}%
</text>
```

### Template Iteration

```html
<!-- Generate labels from data array -->
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="200">
    <template data-bind="{{model.labels}}">
        <text x="{{.x}}" y="{{.y}}" text-anchor="middle" font-size="12" fill="#333">
            {{.text}}
        </text>
    </template>
</svg>
```

---

## Notes

### Coordinate System

- The `x` and `y` coordinates position the text baseline start point
- The coordinate system origin (0,0) is at the top-left of the SVG viewport
- Use `text-anchor` to change horizontal alignment relative to the x coordinate
- Use `dominant-baseline` to adjust vertical alignment relative to the y coordinate

### Text Baseline

- By default, `y` positions the alphabetic baseline of the text
- The baseline is the line on which most letters "sit"
- Descenders (like 'g', 'y', 'p') extend below the baseline
- Use `dominant-baseline="middle"` to center text vertically at the y coordinate

### Text Anchoring

The `text-anchor` attribute controls horizontal alignment:
- `start` (default) - Text starts at the x coordinate
- `middle` - Text is centered on the x coordinate
- `end` - Text ends at the x coordinate

### Font Support

- Scryber supports standard font families and system fonts
- Web fonts and custom fonts require proper font registration
- Font fallback works through the font-family stack
- Unsupported characters may render as fallback glyphs

### Text Content

- Text content can be plain text or include nested `<tspan>` elements
- Whitespace handling follows XML rules (collapse by default)
- Use `xml:space="preserve"` to maintain whitespace
- Line breaks must be created explicitly with multiple text/tspan elements

### Performance Considerations

- Text rendering is optimized for PDF output
- Complex transformations may impact rendering performance
- Large amounts of text should use standard HTML elements when possible
- Consider style caching for repeated text elements with templates

### Limitations

- Multi-line text requires multiple `<text>` or nested `<tspan>` elements
- Text wrapping is not automatic - use calculated positioning
- Text-on-path requires special path elements (not standard text)
- Vertical text rendering has limited support

---

## Examples

### 1. Simple Label

Basic text label with standard positioning:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="60">
    <text x="10" y="30" font-family="Arial" font-size="16" fill="#333">
        Product Name
    </text>
</svg>
```

### 2. Centered Title

Text centered using text-anchor:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="60">
    <text x="200" y="35" text-anchor="middle" font-size="24" font-weight="700" fill="#336699">
        Sales Report 2024
    </text>
</svg>
```

### 3. Right-Aligned Value

Right-aligned text for numerical values:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="50">
    <text x="190" y="30" text-anchor="end" font-size="18" font-family="monospace" fill="#000">
        $1,234.56
    </text>
</svg>
```

### 4. Rotated Label

Text with rotation transformation:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="100" height="200">
    <text x="50" y="100" text-anchor="middle" transform="rotate(-90 50 100)"
          font-size="14" fill="#666">
        Y-Axis Label
    </text>
</svg>
```

### 5. Color-Coded Status

Status text with conditional colors:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="80">
    <text x="10" y="25" font-size="14" fill="#00aa00" font-weight="600">
        Active
    </text>
    <text x="10" y="50" font-size="14" fill="#ff6600" font-weight="600">
        Pending
    </text>
    <text x="10" y="75" font-size="14" fill="#cc0000" font-weight="600">
        Inactive
    </text>
</svg>
```

### 6. Chart Axis Labels

X-axis labels for a bar chart:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="250">
    <rect x="50" y="50" width="40" height="150" fill="#336699"/>
    <rect x="110" y="100" width="40" height="100" fill="#336699"/>
    <rect x="170" y="75" width="40" height="125" fill="#336699"/>
    <rect x="230" y="125" width="40" height="75" fill="#336699"/>

    <text x="70" y="220" text-anchor="middle" font-size="12" fill="#333">Q1</text>
    <text x="130" y="220" text-anchor="middle" font-size="12" fill="#333">Q2</text>
    <text x="190" y="220" text-anchor="middle" font-size="12" fill="#333">Q3</text>
    <text x="250" y="220" text-anchor="middle" font-size="12" fill="#333">Q4</text>
</svg>
```

### 7. Data-Bound Label

Dynamic text from model data:

```html
<!-- Model: { customerName: "Acme Corp", orderCount: 42 } -->
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="60">
    <text x="10" y="30" font-size="16" fill="#333">
        Customer: {{model.customerName}}
    </text>
    <text x="10" y="50" font-size="14" fill="#666">
        Total Orders: {{model.orderCount}}
    </text>
</svg>
```

### 8. Value with Conditional Color

Text color based on threshold:

```html
<!-- Model: { revenue: 125000, target: 100000 } -->
<svg xmlns="http://www.w3.org/2000/svg" width="250" height="80">
    <text x="10" y="30" font-size="14" fill="#333">
        Revenue
    </text>
    <text x="10" y="55" font-size="24" font-weight="700"
          fill="{{model.revenue >= model.target ? '#00aa00' : '#cc0000'}}">
        ${{(model.revenue / 1000).toFixed(1)}}K
    </text>
</svg>
```

### 9. Chart with Dynamic Labels

Bar chart with data-driven positioning:

```html
<!-- Model: { data: [{label: "Jan", value: 120}, {label: "Feb", value: 150}, ...] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="250">
    <template data-bind="{{model.data}}">
        <rect x="{{$index * 60 + 50}}"
              y="{{200 - .value}}"
              width="40"
              height="{{.value}}"
              fill="#336699"/>
        <text x="{{$index * 60 + 70}}"
              y="220"
              text-anchor="middle"
              font-size="12"
              fill="#333">
            {{.label}}
        </text>
        <text x="{{$index * 60 + 70}}"
              y="{{195 - .value}}"
              text-anchor="middle"
              font-size="10"
              fill="#fff"
              font-weight="600">
            {{.value}}
        </text>
    </template>
</svg>
```

### 10. Multi-Style Text Header

Combining multiple text elements:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="100">
    <rect width="400" height="100" fill="#336699"/>
    <text x="20" y="40" font-size="28" font-weight="700" fill="white">
        QUARTERLY REPORT
    </text>
    <text x="20" y="70" font-size="16" fill="white" fill-opacity="0.9">
        Financial Performance Summary
    </text>
</svg>
```

### 11. Percentage Display

Formatted percentage with styling:

```html
<!-- Model: { completionRate: 0.75 } -->
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="60">
    <text x="100" y="35" text-anchor="middle" font-size="32" font-weight="700" fill="#336699">
        {{(model.completionRate * 100).toFixed(0)}}%
    </text>
    <text x="100" y="52" text-anchor="middle" font-size="12" fill="#666">
        Complete
    </text>
</svg>
```

### 12. Status Badge

Circular badge with centered text:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="120" height="120">
    <circle cx="60" cy="60" r="50" fill="#00aa00"/>
    <text x="60" y="70" text-anchor="middle" dominant-baseline="middle"
          font-size="24" font-weight="700" fill="white">
        OK
    </text>
</svg>
```

### 13. Temperature Gauge Label

Gauge with temperature reading:

```html
<!-- Model: { temperature: 72.5, unit: "°F" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="200">
    <circle cx="100" cy="100" r="80" fill="none" stroke="#ddd" stroke-width="20"/>
    <circle cx="100" cy="100" r="80" fill="none" stroke="#ff6600" stroke-width="20"
            stroke-dasharray="{{model.temperature * 5}} 502"/>

    <text x="100" y="95" text-anchor="middle" font-size="36" font-weight="700" fill="#333">
        {{model.temperature.toFixed(1)}}
    </text>
    <text x="100" y="115" text-anchor="middle" font-size="16" fill="#666">
        {{model.unit}}
    </text>
</svg>
```

### 14. Data Point Labels

Scatter plot with value labels:

```html
<!-- Model: { points: [{x: 50, y: 100, value: "A"}, {x: 150, y: 50, value: "B"}] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="200">
    <template data-bind="{{model.points}}">
        <circle cx="{{.x}}" cy="{{.y}}" r="8" fill="#336699"/>
        <text x="{{.x}}" y="{{.y + 20}}" text-anchor="middle" font-size="12" fill="#333">
            {{.value}}
        </text>
    </template>
</svg>
```

### 15. Legend with Icons

Chart legend with text labels:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="120">
    <rect x="20" y="20" width="20" height="20" fill="#336699"/>
    <text x="50" y="35" font-size="14" fill="#333">Revenue</text>

    <rect x="20" y="50" width="20" height="20" fill="#ff6600"/>
    <text x="50" y="65" font-size="14" fill="#333">Expenses</text>

    <rect x="20" y="80" width="20" height="20" fill="#00aa00"/>
    <text x="50" y="95" font-size="14" fill="#333">Profit</text>
</svg>
```

### 16. Dynamic Font Sizing

Text size based on importance:

```html
<!-- Model: { items: [{text: "Critical", priority: 3}, {text: "Normal", priority: 1}] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="150">
    <template data-bind="{{model.items}}">
        <text x="20"
              y="{{$index * 40 + 35}}"
              font-size="{{.priority * 6 + 12}}"
              font-weight="{{.priority > 2 ? 700 : 400}}"
              fill="{{.priority > 2 ? '#cc0000' : '#333'}}">
            {{.text}}
        </text>
    </template>
</svg>
```

### 17. Annotation with Leader

Text annotation with visual indicator:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="200">
    <circle cx="150" cy="100" r="50" fill="#336699"/>
    <line x1="150" y1="50" x2="150" y2="20" stroke="#333" stroke-width="1"/>
    <text x="150" y="15" text-anchor="middle" font-size="12" fill="#333">
        Peak: $250K
    </text>
</svg>
```

### 18. Multi-Line Description

Multiple text elements for line breaks:

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="120">
    <text x="20" y="30" font-size="18" font-weight="700" fill="#336699">
        Product Overview
    </text>
    <text x="20" y="55" font-size="12" fill="#333">
        High-performance solution for enterprise customers
    </text>
    <text x="20" y="75" font-size="12" fill="#333">
        Available in multiple configurations
    </text>
    <text x="20" y="95" font-size="12" fill="#333">
        Includes 24/7 support and maintenance
    </text>
</svg>
```

### 19. Time-Series Chart Labels

Date/time labels on chart axis:

```html
<!-- Model: { timePoints: [{date: "Jan 1", value: 100}, {date: "Jan 15", value: 150}] } -->
<svg xmlns="http://www.w3.org/2000/svg" width="500" height="250">
    <template data-bind="{{model.timePoints}}">
        <line x1="{{$index * 100 + 50}}"
              y1="{{200 - .value}}"
              x2="{{($index + 1) * 100 + 50}}"
              y2="{{200 - model.timePoints[$index + 1].value}}"
              stroke="#336699"
              stroke-width="2"/>
        <text x="{{$index * 100 + 50}}"
              y="220"
              text-anchor="middle"
              font-size="10"
              fill="#666">
            {{.date}}
        </text>
    </template>
</svg>
```

### 20. Score Display with Description

Large score with explanatory text:

```html
<!-- Model: { score: 8.7, maxScore: 10, description: "Excellent" } -->
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="150">
    <text x="100" y="60" text-anchor="middle" font-size="56" font-weight="700" fill="#00aa00">
        {{model.score}}
    </text>
    <text x="100" y="85" text-anchor="middle" font-size="18" fill="#666">
        / {{model.maxScore}}
    </text>
    <text x="100" y="110" text-anchor="middle" font-size="16" font-weight="600" fill="#333">
        {{model.description}}
    </text>
</svg>
```

---

## See Also

- [tspan element](/reference/svgtags/svg_tspan_element.html) - Text span for styled inline text
- [svg element](/reference/svgtags/svg.html) - SVG container element
- [transform attribute](/reference/svgattributes/attr_transform.html) - Transformation operations
- [fill attribute](/reference/svgattributes/attr_fill.html) - Fill colors and patterns
- [Data Binding](/reference/binding/) - Complete data binding guide
- [SVG Styling](/reference/svg/styling/) - SVG style reference
- [Font Reference](/reference/fonts/) - Font configuration and usage

---
