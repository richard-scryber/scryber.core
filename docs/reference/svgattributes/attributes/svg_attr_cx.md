---
layout: default
title: cx
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @cx : Center X Coordinate Attribute

The `cx` attribute specifies the horizontal center position (center X-coordinate) of circular and elliptical elements within the SVG coordinate system. It defines the horizontal center point for circles, ellipses, and radial gradients in your PDF documents.

## Usage

The `cx` attribute sets the horizontal center position:
- For `<circle>`: X-coordinate of the circle's center point
- For `<ellipse>`: X-coordinate of the ellipse's center point
- For `<radialGradient>`: X-coordinate of the gradient's focal/center point

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="200">
    <circle cx="100" cy="100" r="50" fill="#FF5722"/>
</svg>
```

---

## Supported Values

The `cx` attribute accepts unit values:

| Unit Type | Example | Description |
|-----------|---------|-------------|
| Points | `cx="100"` or `cx="100pt"` | Default unit, 1/72 of an inch |
| Pixels | `cx="100px"` | Screen pixels |
| Inches | `cx="2in"` | Physical inches |
| Centimeters | `cx="5cm"` | Metric centimeters |
| Millimeters | `cx="50mm"` | Metric millimeters |
| Percentage | `cx="50%"` | Percentage of parent viewport width |

**Default Value:** `0` (centered at the left edge)

---

## Supported Elements

The `cx` attribute is supported on the following SVG elements:

| Element | Usage |
|---------|-------|
| `<circle>` | Horizontal center coordinate of the circle |
| `<ellipse>` | Horizontal center coordinate of the ellipse |
| `<radialGradient>` | Horizontal position of the gradient's outermost circle |

---

## Data Binding

The `cx` attribute supports dynamic values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Circle Center

```html
<!-- Model: { dot: { cx: 150, cy: 100, radius: 30, color: '#4CAF50' } } -->
<svg width="300" height="200">
    <circle cx="{{dot.cx}}" cy="{{dot.cy}}" r="{{dot.radius}}"
            fill="{{dot.color}}"/>
</svg>
```

### Example 2: Centered Elements with Calculation

```html
<!-- Model: { viewportWidth: 400, viewportHeight: 300 } -->
<svg width="{{viewportWidth}}" height="{{viewportHeight}}">
    <!-- Circle centered in viewport -->
    <circle cx="{{viewportWidth / 2}}" cy="{{viewportHeight / 2}}" r="50"
            fill="#2196F3"/>
</svg>
```

### Example 3: Bubble Chart with Template

```html
<!-- Model: { dataPoints: [
    {cx: 80, cy: 100, r: 25, value: 100, color: '#e74c3c'},
    {cx: 180, cy: 80, r: 35, value: 150, color: '#3498db'},
    {cx: 280, cy: 120, r: 20, value: 80, color: '#2ecc71'}
]} -->
<svg width="380" height="200">
    <template data-bind="{{dataPoints}}">
        <circle cx="{{.cx}}" cy="{{.cy}}" r="{{.r}}"
                fill="{{.color}}" fill-opacity="0.6"
                stroke="#333" stroke-width="1"/>
    </template>
</svg>
```

---

## Notes

### Center-Based Positioning

- Unlike `x` which positions from the left edge, `cx` positions from the center
- This makes circular layouts and radial arrangements more intuitive
- The element is centered horizontally at the specified coordinate

### Coordinate System

- The coordinate is relative to the parent element's coordinate system
- Positive values extend to the right from the origin
- Values can be negative but may render outside the visible viewport

### Default Behavior

- If `cx` is not specified, it defaults to `0`
- A circle with `cx="0"` is centered at the left edge of the viewport
- Use `cx` with `cy` to fully position circular elements in 2D space

### Percentage Values

- Percentage values are calculated relative to the parent viewport width
- `cx="50%"` centers the element horizontally in the parent
- For nested SVG elements, percentages are relative to the immediate parent

### Transform Operations

- Transform operations are applied after initial positioning
- The `cx` value represents position before any transformations
- Transforms can move the effective center position

### Radial Gradients

- For `<radialGradient>`, `cx` defines where the gradient's outer circle is centered
- Combined with `fx` (focal point X), creates directional gradient effects
- Default is `50%` for centered gradients

---

## Examples

### Basic Circle Center

```html
<svg width="200" height="200">
    <circle cx="100" cy="100" r="60" fill="#9C27B0"/>
</svg>
```

### Multiple Circles in a Row

```html
<svg width="400" height="150">
    <circle cx="70" cy="75" r="40" fill="#f44336"/>
    <circle cx="160" cy="75" r="40" fill="#4CAF50"/>
    <circle cx="250" cy="75" r="40" fill="#2196F3"/>
    <circle cx="340" cy="75" r="40" fill="#FF9800"/>
</svg>
```

### Centered Circle

```html
<svg width="300" height="250">
    <!-- Circle centered in viewport (50% of width) -->
    <circle cx="50%" cy="50%" r="80" fill="#4CAF50"/>
</svg>
```

### Scatter Plot

```html
<svg width="400" height="300">
    <!-- X-axis -->
    <line x1="40" y1="260" x2="380" y2="260" stroke="#333" stroke-width="2"/>
    <!-- Y-axis -->
    <line x1="40" y1="20" x2="40" y2="260" stroke="#333" stroke-width="2"/>

    <!-- Data points -->
    <circle cx="80" cy="200" r="8" fill="#2196F3" opacity="0.7"/>
    <circle cx="140" cy="120" r="8" fill="#2196F3" opacity="0.7"/>
    <circle cx="200" cy="180" r="8" fill="#2196F3" opacity="0.7"/>
    <circle cx="260" cy="100" r="8" fill="#2196F3" opacity="0.7"/>
    <circle cx="320" cy="160" r="8" fill="#2196F3" opacity="0.7"/>
    <circle cx="120" cy="150" r="8" fill="#2196F3" opacity="0.7"/>
    <circle cx="180" cy="90" r="8" fill="#2196F3" opacity="0.7"/>
    <circle cx="240" cy="220" r="8" fill="#2196F3" opacity="0.7"/>
</svg>
```

### Status Indicators

```html
<svg width="200" height="60">
    <circle cx="30" cy="30" r="12" fill="#4CAF50"/>
    <text x="50" y="36" font-size="14">Server 1: Online</text>

    <circle cx="30" cy="30" r="12" fill="#f44336" transform="translate(0, 0)"/>
</svg>
```

### Radial Gauge Markers

```html
<svg width="250" height="250">
    <!-- Center circle -->
    <circle cx="125" cy="125" r="100" fill="none" stroke="#e0e0e0" stroke-width="20"/>

    <!-- Marker dots -->
    <circle cx="125" cy="25" r="8" fill="#2196F3"/>
    <circle cx="205" cy="65" r="8" fill="#2196F3"/>
    <circle cx="225" cy="125" r="8" fill="#2196F3"/>
    <circle cx="205" cy="185" r="8" fill="#2196F3"/>
    <circle cx="125" cy="225" r="8" fill="#2196F3"/>
    <circle cx="45" cy="185" r="8" fill="#2196F3"/>
    <circle cx="25" cy="125" r="8" fill="#2196F3"/>
    <circle cx="45" cy="65" r="8" fill="#2196F3"/>
</svg>
```

### Ellipse Center Positioning

```html
<svg width="300" height="200">
    <ellipse cx="150" cy="100" rx="120" ry="70" fill="#FF5722"/>
</svg>
```

### Bubble Chart Sized by Value

```html
<svg width="500" height="300">
    <circle cx="100" cy="150" r="40" fill="#e74c3c" opacity="0.6"/>
    <circle cx="220" cy="100" r="60" fill="#3498db" opacity="0.6"/>
    <circle cx="350" cy="180" r="35" fill="#2ecc71" opacity="0.6"/>
    <circle cx="180" cy="220" r="50" fill="#f39c12" opacity="0.6"/>
    <circle cx="400" cy="120" r="45" fill="#9b59b6" opacity="0.6"/>
</svg>
```

### Orbital Diagram

```html
<svg width="300" height="300">
    <!-- Sun at center -->
    <circle cx="150" cy="150" r="25" fill="#FFC107"/>

    <!-- Inner planet orbit -->
    <circle cx="150" cy="150" r="60" fill="none" stroke="#ccc" stroke-dasharray="3,2"/>
    <circle cx="210" cy="150" r="8" fill="#4CAF50"/>

    <!-- Middle planet orbit -->
    <circle cx="150" cy="150" r="90" fill="none" stroke="#ccc" stroke-dasharray="3,2"/>
    <circle cx="150" cy="60" r="12" fill="#2196F3"/>

    <!-- Outer planet orbit -->
    <circle cx="150" cy="150" r="120" fill="none" stroke="#ccc" stroke-dasharray="3,2"/>
    <circle cx="270" cy="150" r="15" fill="#FF5722"/>
</svg>
```

### Venn Diagram

```html
<svg width="350" height="250">
    <circle cx="150" cy="125" r="80" fill="#e74c3c" fill-opacity="0.5"/>
    <circle cx="200" cy="125" r="80" fill="#3498db" fill-opacity="0.5"/>
</svg>
```

### Target Diagram

```html
<svg width="250" height="250">
    <circle cx="125" cy="125" r="100" fill="none" stroke="#e74c3c" stroke-width="2"/>
    <circle cx="125" cy="125" r="75" fill="none" stroke="#e74c3c" stroke-width="2"/>
    <circle cx="125" cy="125" r="50" fill="none" stroke="#e74c3c" stroke-width="2"/>
    <circle cx="125" cy="125" r="25" fill="none" stroke="#e74c3c" stroke-width="2"/>
    <circle cx="125" cy="125" r="5" fill="#e74c3c"/>
</svg>
```

### Radial Gradient with Custom Center

```html
<svg width="300" height="200">
    <defs>
        <radialGradient id="grad1" cx="70%" cy="30%">
            <stop offset="0%" style="stop-color:#FFD54F;stop-opacity:1"/>
            <stop offset="100%" style="stop-color:#FF6F00;stop-opacity:1"/>
        </radialGradient>
    </defs>
    <rect x="0" y="0" width="300" height="200" fill="url(#grad1)"/>
</svg>
```

### Connection Nodes

```html
<svg width="400" height="250">
    <!-- Connecting lines -->
    <line x1="100" y1="125" x2="200" y2="75" stroke="#999" stroke-width="2"/>
    <line x1="100" y1="125" x2="200" y2="175" stroke="#999" stroke-width="2"/>
    <line x1="200" y1="75" x2="300" y2="125" stroke="#999" stroke-width="2"/>
    <line x1="200" y1="175" x2="300" y2="125" stroke="#999" stroke-width="2"/>

    <!-- Nodes -->
    <circle cx="100" cy="125" r="15" fill="#4CAF50" stroke="#fff" stroke-width="2"/>
    <circle cx="200" cy="75" r="15" fill="#2196F3" stroke="#fff" stroke-width="2"/>
    <circle cx="200" cy="175" r="15" fill="#2196F3" stroke="#fff" stroke-width="2"/>
    <circle cx="300" cy="125" r="15" fill="#f44336" stroke="#fff" stroke-width="2"/>
</svg>
```

### Progress Dots

```html
<svg width="300" height="60">
    <circle cx="50" cy="30" r="15" fill="#4CAF50"/>
    <circle cx="110" cy="30" r="15" fill="#4CAF50"/>
    <circle cx="170" cy="30" r="15" fill="#2196F3"/>
    <circle cx="230" cy="30" r="15" fill="#e0e0e0"/>
</svg>
```

### Dynamic Scatter Plot

```html
<!-- Model: { points: [
    {cx: 80, cy: 150, size: 8, category: 'A'},
    {cx: 140, cy: 100, size: 12, category: 'B'},
    {cx: 200, cy: 180, size: 6, category: 'A'},
    {cx: 260, cy: 80, size: 10, category: 'C'},
    {cx: 320, cy: 140, size: 14, category: 'B'}
], colors: { A: '#e74c3c', B: '#3498db', C: '#2ecc71' } } -->
<svg width="400" height="250">
    <template data-bind="{{points}}">
        <circle cx="{{.cx}}" cy="{{.cy}}" r="{{.size}}"
                fill="{{colors[.category]}}" opacity="0.7"/>
    </template>
</svg>
```

### Molecule Diagram

```html
<svg width="350" height="300">
    <!-- Central atom -->
    <circle cx="175" cy="150" r="30" fill="#2196F3" stroke="#1565C0" stroke-width="3"/>

    <!-- Surrounding atoms with bonds -->
    <line x1="175" y1="150" x2="100" y2="100" stroke="#999" stroke-width="3"/>
    <circle cx="100" cy="100" r="20" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>

    <line x1="175" y1="150" x2="250" y2="100" stroke="#999" stroke-width="3"/>
    <circle cx="250" cy="100" r="20" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>

    <line x1="175" y1="150" x2="100" y2="200" stroke="#999" stroke-width="3"/>
    <circle cx="100" cy="200" r="20" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>

    <line x1="175" y1="150" x2="250" y2="200" stroke="#999" stroke-width="3"/>
    <circle cx="250" cy="200" r="20" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Animated Loading Dots (Static Positions)

```html
<svg width="200" height="60">
    <circle cx="50" cy="30" r="12" fill="#2196F3" opacity="1.0"/>
    <circle cx="100" cy="30" r="12" fill="#2196F3" opacity="0.6"/>
    <circle cx="150" cy="30" r="12" fill="#2196F3" opacity="0.3"/>
</svg>
```

---

## See Also

- [cy](/reference/svgattributes/cy.html) - Center Y coordinate attribute
- [r](/reference/svgattributes/r.html) - Radius attribute for circles
- [rx](/reference/svgattributes/rx.html) - Horizontal radius for ellipses
- [x](/reference/svgattributes/x.html) - X coordinate attribute
- [circle](/reference/svgelements/circle.html) - SVG circle element
- [ellipse](/reference/svgelements/ellipse.html) - SVG ellipse element
- [radialGradient](/reference/svgelements/radialGradient.html) - SVG radial gradient element
- [SVG Transforms](/reference/svgelements/transforms.html) - Transformation operations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
