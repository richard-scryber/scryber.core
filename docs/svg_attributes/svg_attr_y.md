---
layout: default
title: y
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @y : Y Coordinate Attribute

The `y` attribute specifies the vertical position (Y-coordinate) of an element within the SVG coordinate system. It defines the top edge position for rectangles and images, or the starting vertical position for other elements in your PDF documents.

## Usage

The `y` attribute sets the vertical position in the coordinate system:
- For `<rect>`: Y-coordinate of the top-left corner
- For `<svg>`: Y-coordinate of the nested SVG viewport
- For `<image>`: Y-coordinate of the top-left corner of the image
- For `<pattern>`: Y-coordinate offset for pattern positioning
- For `<text>`: Baseline Y-coordinate for text positioning

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="200">
    <rect x="50" y="50" width="200" height="100" fill="#2196F3"/>
</svg>
```

---

## Supported Values

The `y` attribute accepts unit values:

| Unit Type | Example | Description |
|-----------|---------|-------------|
| Points | `y="50"` or `y="50pt"` | Default unit, 1/72 of an inch |
| Pixels | `y="50px"` | Screen pixels |
| Inches | `y="2in"` | Physical inches |
| Centimeters | `y="5cm"` | Metric centimeters |
| Millimeters | `y="50mm"` | Metric millimeters |
| Percentage | `y="25%"` | Percentage of parent viewport height |

**Default Value:** `0` (positioned at the top edge)

---

## Supported Elements

The `y` attribute is supported on the following SVG elements:

| Element | Usage |
|---------|-------|
| `<rect>` | Y-coordinate of rectangle's top-left corner |
| `<svg>` | Y-coordinate for nested SVG viewport positioning |
| `<image>` | Y-coordinate of image's top-left corner |
| `<pattern>` | Vertical offset for pattern tile positioning |
| `<text>` | Baseline Y-coordinate for text positioning |
| `<tspan>` | Y-coordinate override for text span positioning |
| `<use>` | Y-coordinate for referenced element positioning |
| `<foreignObject>` | Y-coordinate of foreign content container |

---

## Data Binding

The `y` attribute supports dynamic values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Vertical Positioning

```html
<!-- Model: { position: { x: 100, y: 75 }, size: { width: 150, height: 100 } } -->
<svg width="400" height="300">
    <rect x="{{position.x}}" y="{{position.y}}"
          width="{{size.width}}" height="{{size.height}}"
          fill="#9C27B0"/>
</svg>
```

### Example 2: Calculated Vertical Position for Charts

```html
<!-- Model: { chartHeight: 200, value: 85, maxValue: 150 } -->
<svg width="300" height="250">
    <!-- Bar positioned from bottom -->
    <rect x="100" y="{{chartHeight - (value / maxValue * chartHeight)}}"
          width="80" height="{{value / maxValue * chartHeight}}"
          fill="#4CAF50"/>
</svg>
```

### Example 3: Stacked Bars with Dynamic Y

```html
<!-- Model: { segments: [
    {height: 60, color: '#e74c3c'},
    {height: 40, color: '#f39c12'},
    {height: 50, color: '#2ecc71'}
], baseY: 200 } -->
<svg width="300" height="250">
    <template data-bind="{{segments}}">
        <rect x="100" y="{{baseY - segments.slice(0, $index + 1).reduce((a, b) => a + b.height, 0)}}"
              width="80" height="{{.height}}"
              fill="{{.color}}"/>
    </template>
</svg>
```

---

## Notes

### Coordinate System

- SVG uses a coordinate system where (0,0) is the top-left corner
- Positive Y values extend downward
- Negative Y values are valid but may render outside the visible viewport
- The Y-axis is vertical, increasing from top to bottom

### Default Behavior

- If `y` is not specified, it defaults to `0`
- Elements positioned at `y="0"` align with the top edge of the SVG viewport
- The coordinate is relative to the parent element's coordinate system

### Units

- When no unit is specified, points (pt) are assumed
- All unit types are converted to points internally
- Percentage values are calculated relative to the parent viewport height
- For nested SVG elements, percentages are relative to the immediate parent

### Positioning Context

- The `y` attribute works in conjunction with `x` for 2D positioning
- Transform operations (`translate`, `rotate`, etc.) are applied after initial positioning
- The `y` value represents position before any transformations

### Text Baseline

- For `<text>` elements, `y` sets the vertical position of the text baseline
- Text with different font sizes aligns along this baseline
- Descenders (like 'g', 'y', 'p') extend below the baseline
- Use `dominant-baseline` or `alignment-baseline` to adjust vertical text alignment

### Chart Positioning

- For bottom-aligned charts, calculate `y` by subtracting height from chart bottom
- Formula: `y = chartBottom - barHeight`
- This creates bars that grow upward from a baseline

---

## Examples

### Basic Vertical Positioning

```html
<svg width="300" height="200">
    <rect x="100" y="50" width="100" height="80" fill="#FF5722"/>
</svg>
```

### Multiple Elements at Different Y Positions

```html
<svg width="300" height="350">
    <rect x="100" y="20" width="100" height="60" fill="#f44336"/>
    <rect x="100" y="100" width="100" height="60" fill="#4CAF50"/>
    <rect x="100" y="180" width="100" height="60" fill="#2196F3"/>
    <rect x="100" y="260" width="100" height="60" fill="#FF9800"/>
</svg>
```

### Vertical Bar Chart (Horizontal Bars)

```html
<svg width="400" height="300">
    <!-- X-axis line -->
    <line x1="80" y1="20" x2="80" y2="280" stroke="#333" stroke-width="2"/>

    <!-- Bars -->
    <rect x="85" y="40" width="200" height="35" fill="#e74c3c"/>
    <rect x="85" y="95" width="280" height="35" fill="#3498db"/>
    <rect x="85" y="150" width="150" height="35" fill="#2ecc71"/>
    <rect x="85" y="205" width="320" height="35" fill="#f39c12"/>
</svg>
```

### Stacked Column Chart

```html
<svg width="300" height="300">
    <!-- Column 1 - stacked segments -->
    <rect x="50" y="200" width="50" height="50" fill="#e74c3c"/>
    <rect x="50" y="150" width="50" height="50" fill="#f39c12"/>
    <rect x="50" y="100" width="50" height="50" fill="#2ecc71"/>

    <!-- Column 2 - stacked segments -->
    <rect x="120" y="170" width="50" height="80" fill="#e74c3c"/>
    <rect x="120" y="110" width="50" height="60" fill="#f39c12"/>
    <rect x="120" y="70" width="50" height="40" fill="#2ecc71"/>
</svg>
```

### Text at Different Vertical Positions

```html
<svg width="300" height="200">
    <text x="150" y="40" font-size="18" fill="#333" text-anchor="middle">
        Title
    </text>
    <text x="150" y="100" font-size="14" fill="#666" text-anchor="middle">
        Subtitle
    </text>
    <text x="150" y="150" font-size="12" fill="#999" text-anchor="middle">
        Description
    </text>
</svg>
```

### Waterfall Chart with Y Positioning

```html
<svg width="450" height="300">
    <!-- Starting value -->
    <rect x="50" y="150" width="60" height="100" fill="#2196F3"/>

    <!-- Increase -->
    <rect x="130" y="120" width="60" height="30" fill="#4CAF50"/>

    <!-- Decrease -->
    <rect x="210" y="150" width="60" height="20" fill="#f44336"/>

    <!-- Increase -->
    <rect x="290" y="100" width="60" height="50" fill="#4CAF50"/>

    <!-- Final value -->
    <rect x="370" y="100" width="60" height="150" fill="#2196F3"/>
</svg>
```

### Nested SVG with Y Offset

```html
<svg width="400" height="400">
    <!-- Nested SVG positioned at y=100 -->
    <svg x="50" y="100" width="300" height="250">
        <rect x="10" y="10" width="280" height="230"
              fill="#F3E5F5" stroke="#9C27B0" stroke-width="2"/>
    </svg>
</svg>
```

### Timeline Events (Vertical Layout)

```html
<svg width="300" height="500">
    <!-- Timeline line -->
    <line x1="50" y1="50" x2="50" y2="450" stroke="#2196F3" stroke-width="4"/>

    <!-- Event 1 -->
    <circle cx="50" cy="80" r="12" fill="#2196F3"/>
    <rect x="75" y="65" width="200" height="30" fill="#E3F2FD" stroke="#2196F3"/>

    <!-- Event 2 -->
    <circle cx="50" cy="160" r="12" fill="#2196F3"/>
    <rect x="75" y="145" width="200" height="30" fill="#E3F2FD" stroke="#2196F3"/>

    <!-- Event 3 -->
    <circle cx="50" cy="240" r="12" fill="#2196F3"/>
    <rect x="75" y="225" width="200" height="30" fill="#E3F2FD" stroke="#2196F3"/>

    <!-- Event 4 -->
    <circle cx="50" cy="320" r="12" fill="#2196F3"/>
    <rect x="75" y="305" width="200" height="30" fill="#E3F2FD" stroke="#2196F3"/>
</svg>
```

### Percentage-Based Vertical Positioning

```html
<svg width="300" height="400">
    <!-- Positioned at 10% from top -->
    <rect x="100" y="10%" width="100" height="60" fill="#f44336"/>

    <!-- Positioned at 30% from top -->
    <rect x="100" y="30%" width="100" height="60" fill="#4CAF50"/>

    <!-- Positioned at 50% from top -->
    <rect x="100" y="50%" width="100" height="60" fill="#2196F3"/>

    <!-- Positioned at 70% from top -->
    <rect x="100" y="70%" width="100" height="60" fill="#FF9800"/>
</svg>
```

### Dynamic Height Bar Chart from Bottom

```html
<!-- Model: { bars: [
    {value: 80, label: 'Q1'},
    {value: 120, label: 'Q2'},
    {value: 95, label: 'Q3'},
    {value: 140, label: 'Q4'}
], chartBottom: 250 } -->
<svg width="400" height="280">
    <template data-bind="{{bars}}">
        <rect x="{{50 + ($index * 80)}}"
              y="{{chartBottom - .value}}"
              width="60"
              height="{{.value}}"
              fill="#4CAF50"/>
    </template>
</svg>
```

### Layer Stack

```html
<svg width="300" height="300">
    <!-- Layer 1 (bottom) -->
    <rect x="50" y="200" width="200" height="80" fill="#E3F2FD" stroke="#2196F3"/>

    <!-- Layer 2 -->
    <rect x="50" y="140" width="200" height="50" fill="#F3E5F5" stroke="#9C27B0"/>

    <!-- Layer 3 -->
    <rect x="50" y="100" width="200" height="30" fill="#E8F5E9" stroke="#4CAF50"/>

    <!-- Layer 4 (top) -->
    <rect x="50" y="70" width="200" height="20" fill="#FFF3E0" stroke="#FF9800"/>
</svg>
```

### Vertical Gauge

```html
<svg width="150" height="350">
    <!-- Background -->
    <rect x="50" y="50" width="50" height="250" fill="#e0e0e0" rx="25"/>

    <!-- Fill (70% full) -->
    <rect x="50" y="125" width="50" height="175" fill="#4CAF50" rx="25"/>

    <!-- Tick marks -->
    <line x1="105" y1="75" x2="120" y2="75" stroke="#333" stroke-width="2"/>
    <line x1="105" y1="175" x2="120" y2="175" stroke="#333" stroke-width="2"/>
    <line x1="105" y1="275" x2="120" y2="275" stroke="#333" stroke-width="2"/>
</svg>
```

### Steps Diagram

```html
<svg width="400" height="300">
    <rect x="50" y="200" width="80" height="60" fill="#2196F3"/>
    <rect x="140" y="160" width="80" height="100" fill="#2196F3"/>
    <rect x="230" y="120" width="80" height="140" fill="#2196F3"/>
    <rect x="320" y="80" width="80" height="180" fill="#2196F3"/>
</svg>
```

### Grid Layout with Y Positioning

```html
<svg width="400" height="400">
    <!-- Row 1 -->
    <rect x="20" y="20" width="80" height="80" fill="#E3F2FD" stroke="#2196F3"/>
    <rect x="120" y="20" width="80" height="80" fill="#E3F2FD" stroke="#2196F3"/>
    <rect x="220" y="20" width="80" height="80" fill="#E3F2FD" stroke="#2196F3"/>
    <rect x="320" y="20" width="80" height="80" fill="#E3F2FD" stroke="#2196F3"/>

    <!-- Row 2 -->
    <rect x="20" y="120" width="80" height="80" fill="#F3E5F5" stroke="#9C27B0"/>
    <rect x="120" y="120" width="80" height="80" fill="#F3E5F5" stroke="#9C27B0"/>
    <rect x="220" y="120" width="80" height="80" fill="#F3E5F5" stroke="#9C27B0"/>
    <rect x="320" y="120" width="80" height="80" fill="#F3E5F5" stroke="#9C27B0"/>

    <!-- Row 3 -->
    <rect x="20" y="220" width="80" height="80" fill="#E8F5E9" stroke="#4CAF50"/>
    <rect x="120" y="220" width="80" height="80" fill="#E8F5E9" stroke="#4CAF50"/>
    <rect x="220" y="220" width="80" height="80" fill="#E8F5E9" stroke="#4CAF50"/>
    <rect x="320" y="220" width="80" height="80" fill="#E8F5E9" stroke="#4CAF50"/>
</svg>
```

### Vertical Progress Steps

```html
<svg width="200" height="400">
    <!-- Step 1 (Complete) -->
    <circle cx="50" cy="60" r="20" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
    <text x="90" y="67" font-size="14">Complete</text>

    <!-- Connector -->
    <line x1="50" y1="80" x2="50" y2="140" stroke="#4CAF50" stroke-width="3"/>

    <!-- Step 2 (Complete) -->
    <circle cx="50" cy="160" r="20" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
    <text x="90" y="167" font-size="14">Complete</text>

    <!-- Connector -->
    <line x1="50" y1="180" x2="50" y2="240" stroke="#4CAF50" stroke-width="3"/>

    <!-- Step 3 (Current) -->
    <circle cx="50" cy="260" r="20" fill="#2196F3" stroke="#1976D2" stroke-width="2"/>
    <text x="90" y="267" font-size="14">In Progress</text>

    <!-- Connector -->
    <line x1="50" y1="280" x2="50" y2="340" stroke="#bbb" stroke-width="3"/>

    <!-- Step 4 (Pending) -->
    <circle cx="50" cy="360" r="20" fill="none" stroke="#bbb" stroke-width="2"/>
    <text x="90" y="367" font-size="14">Pending</text>
</svg>
```

### Dynamic Row Positioning

```html
<!-- Model: { rows: [
    {label: 'Row 1', color: '#e74c3c'},
    {label: 'Row 2', color: '#f39c12'},
    {label: 'Row 3', color: '#2ecc71'},
    {label: 'Row 4', color: '#3498db'}
], rowHeight: 60, rowSpacing: 10 } -->
<svg width="300" height="300">
    <template data-bind="{{rows}}">
        <rect x="50"
              y="{{20 + ($index * (rowHeight + rowSpacing))}}"
              width="200"
              height="{{rowHeight}}"
              fill="{{.color}}" rx="4"/>
    </template>
</svg>
```

### Hierarchical Tree Layout

```html
<svg width="400" height="350">
    <!-- Level 1 -->
    <rect x="160" y="20" width="80" height="40" fill="#2196F3" rx="4"/>

    <!-- Level 2 -->
    <rect x="60" y="100" width="80" height="40" fill="#4CAF50" rx="4"/>
    <rect x="260" y="100" width="80" height="40" fill="#4CAF50" rx="4"/>

    <!-- Level 3 -->
    <rect x="20" y="180" width="60" height="40" fill="#FF9800" rx="4"/>
    <rect x="100" y="180" width="60" height="40" fill="#FF9800" rx="4"/>
    <rect x="220" y="180" width="60" height="40" fill="#FF9800" rx="4"/>
    <rect x="300" y="180" width="60" height="40" fill="#FF9800" rx="4"/>

    <!-- Connecting lines -->
    <line x1="200" y1="60" x2="100" y2="100" stroke="#999" stroke-width="2"/>
    <line x1="200" y1="60" x2="300" y2="100" stroke="#999" stroke-width="2"/>
</svg>
```

---

## See Also

- [x](/reference/svgattributes/x.html) - X coordinate attribute
- [cy](/reference/svgattributes/cy.html) - Center Y coordinate attribute
- [height](/reference/svgattributes/height.html) - Height attribute
- [rect](/reference/svgelements/rect.html) - SVG rectangle element
- [image](/reference/svgelements/image.html) - SVG image element
- [text](/reference/svgelements/text.html) - SVG text element
- [svg](/reference/svgelements/svg.html) - SVG container element
- [SVG Transforms](/reference/svgelements/transforms.html) - Transformation operations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
