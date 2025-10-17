---
layout: default
title: cy
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @cy : Center Y Coordinate Attribute

The `cy` attribute specifies the vertical center position (center Y-coordinate) of circular and elliptical elements within the SVG coordinate system. It defines the vertical center point for circles, ellipses, and radial gradients in your PDF documents.

## Usage

The `cy` attribute sets the vertical center position:
- For `<circle>`: Y-coordinate of the circle's center point
- For `<ellipse>`: Y-coordinate of the ellipse's center point
- For `<radialGradient>`: Y-coordinate of the gradient's focal/center point

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="200">
    <circle cx="100" cy="100" r="50" fill="#4CAF50"/>
</svg>
```

---

## Supported Values

The `cy` attribute accepts unit values:

| Unit Type | Example | Description |
|-----------|---------|-------------|
| Points | `cy="100"` or `cy="100pt"` | Default unit, 1/72 of an inch |
| Pixels | `cy="100px"` | Screen pixels |
| Inches | `cy="2in"` | Physical inches |
| Centimeters | `cy="5cm"` | Metric centimeters |
| Millimeters | `cy="50mm"` | Metric millimeters |
| Percentage | `cy="50%"` | Percentage of parent viewport height |

**Default Value:** `0` (centered at the top edge)

---

## Supported Elements

The `cy` attribute is supported on the following SVG elements:

| Element | Usage |
|---------|-------|
| `<circle>` | Vertical center coordinate of the circle |
| `<ellipse>` | Vertical center coordinate of the ellipse |
| `<radialGradient>` | Vertical position of the gradient's outermost circle |

---

## Data Binding

The `cy` attribute supports dynamic values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Circle Positioning

```html
<!-- Model: { dot: { cx: 150, cy: 100, radius: 40, color: '#9C27B0' } } -->
<svg width="300" height="200">
    <circle cx="{{dot.cx}}" cy="{{dot.cy}}" r="{{dot.radius}}"
            fill="{{dot.color}}"/>
</svg>
```

### Example 2: Vertically Centered Element

```html
<!-- Model: { viewportWidth: 400, viewportHeight: 300 } -->
<svg width="{{viewportWidth}}" height="{{viewportHeight}}">
    <!-- Circle centered vertically and horizontally -->
    <circle cx="{{viewportWidth / 2}}" cy="{{viewportHeight / 2}}" r="60"
            fill="#FF5722"/>
</svg>
```

### Example 3: Vertical Scatter Plot

```html
<!-- Model: { dataPoints: [
    {x: 80, cy: 180, r: 10, value: 45},
    {x: 140, cy: 120, r: 15, value: 75},
    {x: 200, cy: 200, r: 8, value: 30},
    {x: 260, cy: 80, r: 20, value: 95}
]} -->
<svg width="350" height="250">
    <template data-bind="{{dataPoints}}">
        <circle cx="{{.x}}" cy="{{.cy}}" r="{{.r}}"
                fill="#2196F3" opacity="0.7"/>
    </template>
</svg>
```

---

## Notes

### Center-Based Positioning

- Unlike `y` which positions from the top edge, `cy` positions from the center
- This makes circular layouts and vertical alignments more intuitive
- The element is centered vertically at the specified coordinate

### Coordinate System

- SVG uses a coordinate system where Y increases downward
- Positive `cy` values extend down from the top
- Values can be negative but may render outside the visible viewport
- The coordinate is relative to the parent element's coordinate system

### Default Behavior

- If `cy` is not specified, it defaults to `0`
- A circle with `cy="0"` is centered at the top edge of the viewport
- Use `cy` with `cx` to fully position circular elements in 2D space

### Percentage Values

- Percentage values are calculated relative to the parent viewport height
- `cy="50%"` centers the element vertically in the parent
- For nested SVG elements, percentages are relative to the immediate parent

### Transform Operations

- Transform operations are applied after initial positioning
- The `cy` value represents position before any transformations
- Transforms can move the effective center position

### Radial Gradients

- For `<radialGradient>`, `cy` defines where the gradient's outer circle is centered vertically
- Combined with `fy` (focal point Y), creates directional gradient effects
- Default is `50%` for centered gradients

---

## Examples

### Basic Circle Center

```html
<svg width="200" height="200">
    <circle cx="100" cy="100" r="70" fill="#2196F3"/>
</svg>
```

### Vertical Stack of Circles

```html
<svg width="200" height="500">
    <circle cx="100" cy="70" r="40" fill="#f44336"/>
    <circle cx="100" cy="170" r="40" fill="#4CAF50"/>
    <circle cx="100" cy="270" r="40" fill="#2196F3"/>
    <circle cx="100" cy="370" r="40" fill="#FF9800"/>
    <circle cx="100" cy="470" r="40" fill="#9C27B0"/>
</svg>
```

### Perfectly Centered Circle

```html
<svg width="300" height="250">
    <!-- Circle centered both horizontally and vertically -->
    <circle cx="50%" cy="50%" r="80" fill="#4CAF50"/>
</svg>
```

### Vertical Line Chart Markers

```html
<svg width="450" height="300">
    <!-- Grid lines -->
    <line x1="50" y1="250" x2="400" y2="250" stroke="#e0e0e0" stroke-width="1"/>
    <line x1="50" y1="200" x2="400" y2="200" stroke="#e0e0e0" stroke-width="1"/>
    <line x1="50" y1="150" x2="400" y2="150" stroke="#e0e0e0" stroke-width="1"/>
    <line x1="50" y1="100" x2="400" y2="100" stroke="#e0e0e0" stroke-width="1"/>
    <line x1="50" y1="50" x2="400" y2="50" stroke="#e0e0e0" stroke-width="1"/>

    <!-- Axes -->
    <line x1="50" y1="30" x2="50" y2="270" stroke="#333" stroke-width="2"/>
    <line x1="50" y1="250" x2="420" y2="250" stroke="#333" stroke-width="2"/>

    <!-- Data points -->
    <polyline points="70,200 140,120 210,180 280,80 350,140"
              fill="none" stroke="#2196F3" stroke-width="3"/>

    <circle cx="70" cy="200" r="6" fill="#2196F3"/>
    <circle cx="140" cy="120" r="6" fill="#2196F3"/>
    <circle cx="210" cy="180" r="6" fill="#2196F3"/>
    <circle cx="280" cy="80" r="6" fill="#2196F3"/>
    <circle cx="350" cy="140" r="6" fill="#2196F3"/>
</svg>
```

### Traffic Light

```html
<svg width="100" height="300">
    <!-- Housing -->
    <rect x="20" y="20" width="60" height="260" rx="10" fill="#333"/>

    <!-- Red light -->
    <circle cx="50" cy="65" r="25" fill="#c0392b"/>

    <!-- Yellow light -->
    <circle cx="50" cy="150" r="25" fill="#34495e"/>

    <!-- Green light (active) -->
    <circle cx="50" cy="235" r="25" fill="#27ae60"/>
</svg>
```

### Ellipse Vertical Positioning

```html
<svg width="300" height="400">
    <ellipse cx="150" cy="100" rx="100" ry="60" fill="#e74c3c" opacity="0.6"/>
    <ellipse cx="150" cy="200" rx="100" ry="60" fill="#3498db" opacity="0.6"/>
    <ellipse cx="150" cy="300" rx="100" ry="60" fill="#2ecc71" opacity="0.6"/>
</svg>
```

### Bubble Chart with Vertical Distribution

```html
<svg width="400" height="350">
    <!-- Small bubbles at top -->
    <circle cx="100" cy="80" r="25" fill="#e74c3c" opacity="0.6"/>
    <circle cx="300" cy="100" r="30" fill="#f39c12" opacity="0.6"/>

    <!-- Medium bubbles in middle -->
    <circle cx="200" cy="175" r="45" fill="#3498db" opacity="0.6"/>
    <circle cx="320" cy="200" r="35" fill="#2ecc71" opacity="0.6"/>

    <!-- Large bubbles at bottom -->
    <circle cx="150" cy="280" r="55" fill="#9b59b6" opacity="0.6"/>
    <circle cx="280" cy="260" r="40" fill="#16a085" opacity="0.6"/>
</svg>
```

### Vertical Progress Indicator

```html
<svg width="100" height="400">
    <!-- Track -->
    <line x1="50" y1="50" x2="50" y2="350" stroke="#e0e0e0" stroke-width="8" stroke-linecap="round"/>

    <!-- Progress line -->
    <line x1="50" y1="50" x2="50" y2="250" stroke="#4CAF50" stroke-width="8" stroke-linecap="round"/>

    <!-- Current position marker -->
    <circle cx="50" cy="250" r="15" fill="#4CAF50" stroke="#fff" stroke-width="3"/>
</svg>
```

### Vertical Gauge

```html
<svg width="150" height="400">
    <!-- Background bar -->
    <rect x="50" y="50" width="50" height="300" fill="#e0e0e0" rx="25"/>

    <!-- Fill (60% full) -->
    <rect x="50" y="170" width="50" height="180" fill="#4CAF50" rx="25"/>

    <!-- Markers -->
    <circle cx="25" cy="75" r="5" fill="#333"/>
    <circle cx="25" cy="200" r="5" fill="#333"/>
    <circle cx="25" cy="325" r="5" fill="#333"/>
</svg>
```

### Radial Menu Items

```html
<svg width="300" height="300">
    <!-- Center button -->
    <circle cx="150" cy="150" r="30" fill="#2196F3"/>

    <!-- Menu items in circle around center -->
    <circle cx="150" cy="80" r="20" fill="#4CAF50"/>
    <circle cx="220" cy="150" r="20" fill="#4CAF50"/>
    <circle cx="150" cy="220" r="20" fill="#4CAF50"/>
    <circle cx="80" cy="150" r="20" fill="#4CAF50"/>

    <!-- Diagonal positions -->
    <circle cx="200" cy="100" r="20" fill="#FF9800"/>
    <circle cx="200" cy="200" r="20" fill="#FF9800"/>
    <circle cx="100" cy="200" r="20" fill="#FF9800"/>
    <circle cx="100" cy="100" r="20" fill="#FF9800"/>
</svg>
```

### Radial Gradient with Offset Center

```html
<svg width="300" height="250">
    <defs>
        <radialGradient id="grad2" cx="50%" cy="30%">
            <stop offset="0%" style="stop-color:#64B5F6;stop-opacity:1"/>
            <stop offset="100%" style="stop-color:#0D47A1;stop-opacity:1"/>
        </radialGradient>
    </defs>
    <rect x="0" y="0" width="300" height="250" fill="url(#grad2)"/>
</svg>
```

### Stacked Data Visualization

```html
<svg width="500" height="300">
    <!-- Category A -->
    <circle cx="100" cy="100" r="15" fill="#e74c3c"/>
    <circle cx="150" cy="100" r="15" fill="#e74c3c"/>
    <circle cx="200" cy="100" r="15" fill="#e74c3c"/>

    <!-- Category B -->
    <circle cx="100" cy="150" r="15" fill="#3498db"/>
    <circle cx="150" cy="150" r="15" fill="#3498db"/>

    <!-- Category C -->
    <circle cx="100" cy="200" r="15" fill="#2ecc71"/>
    <circle cx="150" cy="200" r="15" fill="#2ecc71"/>
    <circle cx="200" cy="200" r="15" fill="#2ecc71"/>
    <circle cx="250" cy="200" r="15" fill="#2ecc71"/>
</svg>
```

### Vertical Timeline

```html
<svg width="250" height="500">
    <!-- Timeline line -->
    <line x1="50" y1="50" x2="50" y2="450" stroke="#2196F3" stroke-width="4"/>

    <!-- Event 1 -->
    <circle cx="50" cy="80" r="15" fill="#4CAF50"/>
    <rect x="80" y="65" width="150" height="30" fill="#E8F5E9" stroke="#4CAF50" rx="4"/>

    <!-- Event 2 -->
    <circle cx="50" cy="170" r="15" fill="#4CAF50"/>
    <rect x="80" y="155" width="150" height="30" fill="#E8F5E9" stroke="#4CAF50" rx="4"/>

    <!-- Event 3 -->
    <circle cx="50" cy="260" r="15" fill="#2196F3"/>
    <rect x="80" y="245" width="150" height="30" fill="#E3F2FD" stroke="#2196F3" rx="4"/>

    <!-- Event 4 -->
    <circle cx="50" cy="350" r="15" fill="#e0e0e0"/>
    <rect x="80" y="335" width="150" height="30" fill="#f5f5f5" stroke="#e0e0e0" rx="4"/>
</svg>
```

### Molecule Structure

```html
<svg width="300" height="350">
    <!-- Central atom -->
    <circle cx="150" cy="175" r="35" fill="#2196F3" stroke="#1565C0" stroke-width="3"/>

    <!-- Top atom -->
    <line x1="150" y1="175" x2="150" y2="80" stroke="#999" stroke-width="3"/>
    <circle cx="150" cy="80" r="25" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>

    <!-- Bottom atom -->
    <line x1="150" y1="175" x2="150" y2="270" stroke="#999" stroke-width="3"/>
    <circle cx="150" cy="270" r="25" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>

    <!-- Left atom -->
    <line x1="150" y1="175" x2="70" y2="175" stroke="#999" stroke-width="3"/>
    <circle cx="70" cy="175" r="20" fill="#FF9800" stroke="#E65100" stroke-width="2"/>

    <!-- Right atom -->
    <line x1="150" y1="175" x2="230" y2="175" stroke="#999" stroke-width="3"/>
    <circle cx="230" cy="175" r="20" fill="#FF9800" stroke="#E65100" stroke-width="2"/>
</svg>
```

### Dynamic Vertical Positioning

```html
<!-- Model: { markers: [
    {cx: 100, cy: 80, size: 12, label: 'High'},
    {cx: 200, cy: 150, size: 10, label: 'Medium'},
    {cx: 300, cy: 220, size: 8, label: 'Low'}
], color: '#2196F3' } -->
<svg width="400" height="280">
    <template data-bind="{{markers}}">
        <circle cx="{{.cx}}" cy="{{.cy}}" r="{{.size}}"
                fill="{{color}}" opacity="0.7"/>
    </template>
</svg>
```

### Bell Curve Distribution

```html
<svg width="500" height="300">
    <!-- Distribution curve (simulated with circles) -->
    <circle cx="250" cy="100" r="8" fill="#2196F3"/>
    <circle cx="220" cy="130" r="10" fill="#2196F3"/>
    <circle cx="280" cy="130" r="10" fill="#2196F3"/>
    <circle cx="190" cy="165" r="12" fill="#2196F3"/>
    <circle cx="250" cy="160" r="14" fill="#2196F3"/>
    <circle cx="310" cy="165" r="12" fill="#2196F3"/>
    <circle cx="160" cy="200" r="10" fill="#2196F3"/>
    <circle cx="340" cy="200" r="10" fill="#2196F3"/>
    <circle cx="130" cy="230" r="7" fill="#2196F3"/>
    <circle cx="370" cy="230" r="7" fill="#2196F3"/>
</svg>
```

### Status Board

```html
<svg width="350" height="400">
    <!-- Row 1 -->
    <circle cx="50" cy="60" r="20" fill="#4CAF50"/>
    <circle cx="130" cy="60" r="20" fill="#4CAF50"/>
    <circle cx="210" cy="60" r="20" fill="#4CAF50"/>
    <circle cx="290" cy="60" r="20" fill="#f44336"/>

    <!-- Row 2 -->
    <circle cx="50" cy="140" r="20" fill="#4CAF50"/>
    <circle cx="130" cy="140" r="20" fill="#4CAF50"/>
    <circle cx="210" cy="140" r="20" fill="#4CAF50"/>
    <circle cx="290" cy="140" r="20" fill="#4CAF50"/>

    <!-- Row 3 -->
    <circle cx="50" cy="220" r="20" fill="#4CAF50"/>
    <circle cx="130" cy="220" r="20" fill="#FF9800"/>
    <circle cx="210" cy="220" r="20" fill="#4CAF50"/>
    <circle cx="290" cy="220" r="20" fill="#4CAF50"/>
</svg>
```

---

## See Also

- [cx](/reference/svgattributes/cx.html) - Center X coordinate attribute
- [r](/reference/svgattributes/r.html) - Radius attribute for circles
- [ry](/reference/svgattributes/ry.html) - Vertical radius for ellipses
- [y](/reference/svgattributes/y.html) - Y coordinate attribute
- [circle](/reference/svgelements/circle.html) - SVG circle element
- [ellipse](/reference/svgelements/ellipse.html) - SVG ellipse element
- [radialGradient](/reference/svgelements/radialGradient.html) - SVG radial gradient element
- [SVG Transforms](/reference/svgelements/transforms.html) - Transformation operations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
