---
layout: default
title: x
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @x : X Coordinate Attribute

The `x` attribute specifies the horizontal position (X-coordinate) of an element within the SVG coordinate system. It defines the left edge position for rectangles and images, or the starting horizontal position for other elements in your PDF documents.

## Usage

The `x` attribute sets the horizontal position in the coordinate system:
- For `<rect>`: X-coordinate of the top-left corner
- For `<svg>`: X-coordinate of the nested SVG viewport
- For `<image>`: X-coordinate of the top-left corner of the image
- For `<pattern>`: X-coordinate offset for pattern positioning
- For `<text>`: Starting X-coordinate for text positioning

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="200">
    <rect x="50" y="50" width="200" height="100" fill="#4CAF50"/>
</svg>
```

---

## Supported Values

The `x` attribute accepts unit values:

| Unit Type | Example | Description |
|-----------|---------|-------------|
| Points | `x="50"` or `x="50pt"` | Default unit, 1/72 of an inch |
| Pixels | `x="50px"` | Screen pixels |
| Inches | `x="2in"` | Physical inches |
| Centimeters | `x="5cm"` | Metric centimeters |
| Millimeters | `x="50mm"` | Metric millimeters |
| Percentage | `x="25%"` | Percentage of parent viewport width |

**Default Value:** `0` (positioned at the left edge)

---

## Supported Elements

The `x` attribute is supported on the following SVG elements:

| Element | Usage |
|---------|-------|
| `<rect>` | X-coordinate of rectangle's top-left corner |
| `<svg>` | X-coordinate for nested SVG viewport positioning |
| `<image>` | X-coordinate of image's top-left corner |
| `<pattern>` | Horizontal offset for pattern tile positioning |
| `<text>` | Starting X-coordinate for text baseline |
| `<tspan>` | X-coordinate override for text span positioning |
| `<use>` | X-coordinate for referenced element positioning |
| `<foreignObject>` | X-coordinate of foreign content container |

---

## Data Binding

The `x` attribute supports dynamic values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Positioning from Data

```html
<!-- Model: { position: { x: 100, y: 50 }, size: { width: 150, height: 80 } } -->
<svg width="400" height="200">
    <rect x="{{position.x}}" y="{{position.y}}"
          width="{{size.width}}" height="{{size.height}}"
          fill="#2196F3"/>
</svg>
```

### Example 2: Calculated Position

```html
<!-- Model: { margin: 20, index: 3, itemWidth: 60 } -->
<svg width="300" height="100">
    <rect x="{{margin + (index * itemWidth)}}" y="20"
          width="50" height="60"
          fill="#FF5722"/>
</svg>
```

### Example 3: Dynamic Bar Chart Positioning

```html
<!-- Model: { bars: [
    {value: 80, label: 'Q1'},
    {value: 120, label: 'Q2'},
    {value: 95, label: 'Q3'},
    {value: 140, label: 'Q4'}
]} -->
<svg width="400" height="200">
    <template data-bind="{{bars}}">
        <rect x="{{50 + ($index * 80)}}" y="{{180 - .value}}"
              width="60" height="{{.value}}"
              fill="#4CAF50"/>
    </template>
</svg>
```

---

## Notes

### Coordinate System

- SVG uses a coordinate system where (0,0) is the top-left corner
- Positive X values extend to the right
- Negative X values are valid but may render outside the visible viewport
- The X-axis is horizontal, increasing from left to right

### Default Behavior

- If `x` is not specified, it defaults to `0`
- Elements positioned at `x="0"` align with the left edge of the SVG viewport
- The coordinate is relative to the parent element's coordinate system

### Units

- When no unit is specified, points (pt) are assumed
- All unit types are converted to points internally
- Percentage values are calculated relative to the parent viewport width
- For nested SVG elements, percentages are relative to the immediate parent

### Positioning Context

- The `x` attribute works in conjunction with `y` for 2D positioning
- Transform operations (`translate`, `rotate`, etc.) are applied after initial positioning
- The `x` value represents position before any transformations

### Text Elements

- For `<text>` elements, `x` sets the starting position of the text baseline
- Text anchoring (`text-anchor`) affects how text aligns relative to the `x` position
- Multiple `x` values can be specified for `<tspan>` to position individual characters

---

## Examples

### Basic Rectangle Positioning

```html
<svg width="300" height="150">
    <rect x="50" y="30" width="200" height="80" fill="#3F51B5"/>
</svg>
```

### Multiple Elements at Different X Positions

```html
<svg width="400" height="100">
    <rect x="20" y="20" width="60" height="60" fill="#f44336"/>
    <rect x="100" y="20" width="60" height="60" fill="#4CAF50"/>
    <rect x="180" y="20" width="60" height="60" fill="#2196F3"/>
    <rect x="260" y="20" width="60" height="60" fill="#FF9800"/>
</svg>
```

### Bar Chart with Fixed X Positions

```html
<svg width="400" height="250">
    <!-- Y-axis -->
    <line x1="40" y1="20" x2="40" y2="220" stroke="#333" stroke-width="2"/>
    <!-- X-axis -->
    <line x1="40" y1="220" x2="380" y2="220" stroke="#333" stroke-width="2"/>

    <!-- Bars -->
    <rect x="60" y="120" width="50" height="100" fill="#e74c3c"/>
    <rect x="140" y="80" width="50" height="140" fill="#3498db"/>
    <rect x="220" y="140" width="50" height="80" fill="#2ecc71"/>
    <rect x="300" y="100" width="50" height="120" fill="#f39c12"/>
</svg>
```

### Image Positioning

```html
<svg width="400" height="300">
    <image x="50" y="50" width="300" height="200"
           href="chart-data.png"/>
</svg>
```

### Nested SVG Positioning

```html
<svg width="500" height="300">
    <!-- Nested SVG positioned at x=100 -->
    <svg x="100" y="50" width="300" height="200">
        <rect x="10" y="10" width="280" height="180"
              fill="#E3F2FD" stroke="#2196F3" stroke-width="2"/>
    </svg>
</svg>
```

### Text Positioning

```html
<svg width="300" height="150">
    <text x="150" y="75" font-size="24" fill="#333" text-anchor="middle">
        Centered Text
    </text>
</svg>
```

### Pattern with X Offset

```html
<svg width="300" height="200">
    <defs>
        <pattern id="stripes" x="10" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <rect x="0" y="0" width="10" height="20" fill="#2196F3"/>
            <rect x="10" y="0" width="10" height="20" fill="#fff"/>
        </pattern>
    </defs>
    <rect x="0" y="0" width="300" height="200" fill="url(#stripes)"/>
</svg>
```

### Gantt Chart Timeline

```html
<svg width="500" height="200">
    <!-- Task 1: starts at x=50, duration 150 units -->
    <rect x="50" y="30" width="150" height="30" fill="#4CAF50" opacity="0.8"/>

    <!-- Task 2: starts at x=120, duration 200 units -->
    <rect x="120" y="70" width="200" height="30" fill="#2196F3" opacity="0.8"/>

    <!-- Task 3: starts at x=80, duration 180 units -->
    <rect x="80" y="110" width="180" height="30" fill="#FF9800" opacity="0.8"/>

    <!-- Task 4: starts at x=200, duration 120 units -->
    <rect x="200" y="150" width="120" height="30" fill="#9C27B0" opacity="0.8"/>
</svg>
```

### Percentage-Based Positioning

```html
<svg width="400" height="200">
    <!-- Positioned at 10% from left -->
    <rect x="10%" y="50" width="60" height="100" fill="#f44336"/>

    <!-- Positioned at 30% from left -->
    <rect x="30%" y="50" width="60" height="100" fill="#4CAF50"/>

    <!-- Positioned at 50% from left -->
    <rect x="50%" y="50" width="60" height="100" fill="#2196F3"/>

    <!-- Positioned at 70% from left -->
    <rect x="70%" y="50" width="60" height="100" fill="#FF9800"/>
</svg>
```

### Waterfall Chart

```html
<svg width="450" height="300">
    <rect x="50" y="100" width="60" height="100" fill="#4CAF50"/>
    <rect x="130" y="80" width="60" height="20" fill="#f44336"/>
    <rect x="210" y="60" width="60" height="40" fill="#4CAF50"/>
    <rect x="290" y="90" width="60" height="10" fill="#f44336"/>
    <rect x="370" y="80" width="60" height="120" fill="#2196F3"/>
</svg>
```

### Dynamic Column Layout

```html
<!-- Model: { columns: [
    {x: 20, height: 150, color: '#e74c3c'},
    {x: 90, height: 200, color: '#3498db'},
    {x: 160, height: 120, color: '#2ecc71'},
    {x: 230, height: 180, color: '#f39c12'},
    {x: 300, height: 90, color: '#9b59b6'}
]} -->
<svg width="400" height="250">
    <template data-bind="{{columns}}">
        <rect x="{{.x}}" y="{{230 - .height}}"
              width="50" height="{{.height}}"
              fill="{{.color}}"/>
    </template>
</svg>
```

### Calendar Grid Days

```html
<svg width="420" height="300">
    <!-- Week 1 -->
    <rect x="20" y="50" width="50" height="50" fill="#E3F2FD" stroke="#2196F3"/>
    <rect x="80" y="50" width="50" height="50" fill="#E3F2FD" stroke="#2196F3"/>
    <rect x="140" y="50" width="50" height="50" fill="#E3F2FD" stroke="#2196F3"/>
    <rect x="200" y="50" width="50" height="50" fill="#E3F2FD" stroke="#2196F3"/>
    <rect x="260" y="50" width="50" height="50" fill="#E3F2FD" stroke="#2196F3"/>
    <rect x="320" y="50" width="50" height="50" fill="#FFEBEE" stroke="#f44336"/>
    <rect x="380" y="50" width="50" height="50" fill="#FFEBEE" stroke="#f44336"/>

    <!-- Week 2 -->
    <rect x="20" y="110" width="50" height="50" fill="#E3F2FD" stroke="#2196F3"/>
    <rect x="80" y="110" width="50" height="50" fill="#E3F2FD" stroke="#2196F3"/>
    <rect x="140" y="110" width="50" height="50" fill="#E3F2FD" stroke="#2196F3"/>
</svg>
```

### Dashboard Widget Grid

```html
<!-- Model: { widgets: [
    {x: 10, y: 10, title: 'Sales'},
    {x: 160, y: 10, title: 'Users'},
    {x: 310, y: 10, title: 'Revenue'},
    {x: 10, y: 180, title: 'Traffic'},
    {x: 160, y: 180, title: 'Conversion'},
    {x: 310, y: 180, title: 'Growth'}
]} -->
<svg width="460" height="350">
    <template data-bind="{{widgets}}">
        <rect x="{{.x}}" y="{{.y}}" width="140" height="160"
              fill="#fff" stroke="#ddd" stroke-width="1" rx="4"/>
    </template>
</svg>
```

### Stacked Timeline Events

```html
<svg width="600" height="150">
    <!-- Event 1: 9 AM - 11 AM -->
    <rect x="100" y="20" width="200" height="35" fill="#4CAF50" rx="4"/>

    <!-- Event 2: 11:30 AM - 1 PM -->
    <rect x="250" y="20" width="150" height="35" fill="#2196F3" rx="4"/>

    <!-- Event 3: 2 PM - 4 PM -->
    <rect x="400" y="20" width="180" height="35" fill="#FF9800" rx="4"/>

    <!-- Event 4: 10 AM - 12 PM (parallel track) -->
    <rect x="120" y="65" width="180" height="35" fill="#9C27B0" rx="4"/>
</svg>
```

### Progress Steps

```html
<svg width="500" height="80">
    <!-- Step 1 (Complete) -->
    <circle cx="50" cy="40" r="20" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
    <text x="50" y="70" text-anchor="middle" font-size="12">Step 1</text>

    <!-- Step 2 (Complete) -->
    <circle cx="175" cy="40" r="20" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
    <text x="175" y="70" text-anchor="middle" font-size="12">Step 2</text>

    <!-- Step 3 (Current) -->
    <circle cx="300" cy="40" r="20" fill="#2196F3" stroke="#1976D2" stroke-width="2"/>
    <text x="300" y="70" text-anchor="middle" font-size="12">Step 3</text>

    <!-- Step 4 (Pending) -->
    <circle cx="425" cy="40" r="20" fill="none" stroke="#bbb" stroke-width="2"/>
    <text x="425" y="70" text-anchor="middle" font-size="12">Step 4</text>

    <!-- Connector lines -->
    <line x1="70" y1="40" x2="155" y2="40" stroke="#4CAF50" stroke-width="3"/>
    <line x1="195" y1="40" x2="280" y2="40" stroke="#4CAF50" stroke-width="3"/>
    <line x1="320" y1="40" x2="405" y2="40" stroke="#bbb" stroke-width="3"/>
</svg>
```

### Dynamic Spacing Between Elements

```html
<!-- Model: { items: [
    {label: 'Item 1', color: '#e74c3c'},
    {label: 'Item 2', color: '#f39c12'},
    {label: 'Item 3', color: '#2ecc71'}
], spacing: 80 } -->
<svg width="350" height="100">
    <template data-bind="{{items}}">
        <rect x="{{20 + ($index * spacing)}}" y="30"
              width="60" height="60"
              fill="{{.color}}" rx="4"/>
    </template>
</svg>
```

---

## See Also

- [y](/reference/svgattributes/y.html) - Y coordinate attribute
- [cx](/reference/svgattributes/cx.html) - Center X coordinate attribute
- [width](/reference/svgattributes/width.html) - Width attribute
- [rect](/reference/svgelements/rect.html) - SVG rectangle element
- [image](/reference/svgelements/image.html) - SVG image element
- [text](/reference/svgelements/text.html) - SVG text element
- [svg](/reference/svgelements/svg.html) - SVG container element
- [SVG Transforms](/reference/svgelements/transforms.html) - Transformation operations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
