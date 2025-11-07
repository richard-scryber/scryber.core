---
layout: default
title: r
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @r : Radius Attribute

The `r` attribute specifies the radius of circular elements within the SVG coordinate system. It defines the distance from the center point to the edge for circles and the outer radius for radial gradients in your PDF documents.

## Usage

The `r` attribute sets the radius:
- For `<circle>`: Distance from center (`cx`, `cy`) to the edge, defining the circle's size
- For `<radialGradient>`: Radius of the gradient's outermost circle

```html
<svg xmlns="http://www.w3.org/2000/svg" width="200" height="200">
    <circle cx="100" cy="100" r="60" fill="#FF9800"/>
</svg>
```

---

## Supported Values

The `r` attribute accepts unit values:

| Unit Type | Example | Description |
|-----------|---------|-------------|
| Points | `r="50"` or `r="50pt"` | Default unit, 1/72 of an inch |
| Pixels | `r="50px"` | Screen pixels |
| Inches | `r="1in"` | Physical inches |
| Centimeters | `r="2cm"` | Metric centimeters |
| Millimeters | `r="20mm"` | Metric millimeters |
| Percentage | `r="25%"` | Percentage of parent viewport (uses smaller of width/height) |

**Default Value:** `0` (no visible circle)

**Constraints:**
- Must be a positive value or zero
- Negative values are invalid
- Zero radius produces no visible output

---

## Supported Elements

The `r` attribute is supported on the following SVG elements:

| Element | Usage |
|---------|-------|
| `<circle>` | Radius of the circle |
| `<radialGradient>` | Radius of the gradient's outermost circle |

---

## Data Binding

The `r` attribute supports dynamic values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Circle Size

```html
<!-- Model: { dot: { cx: 150, cy: 100, radius: 45, color: '#9C27B0' } } -->
<svg width="300" height="200">
    <circle cx="{{dot.cx}}" cy="{{dot.cy}}" r="{{dot.radius}}"
            fill="{{dot.color}}"/>
</svg>
```

### Example 2: Sized by Data Value

```html
<!-- Model: { value: 85, maxValue: 100 } -->
<svg width="200" height="200">
    <!-- Circle radius based on percentage of max value -->
    <circle cx="100" cy="100" r="{{(value / maxValue) * 80}}"
            fill="#4CAF50" opacity="0.7"/>
</svg>
```

### Example 3: Bubble Chart with Variable Sizes

```html
<!-- Model: { bubbles: [
    {cx: 80, cy: 100, value: 50, color: '#e74c3c'},
    {cx: 180, cy: 80, value: 100, color: '#3498db'},
    {cx: 280, cy: 120, value: 35, color: '#2ecc71'}
], scale: 0.5 } -->
<svg width="380" height="200">
    <template data-bind="{{bubbles}}">
        <circle cx="{{.cx}}" cy="{{.cy}}" r="{{.value * scale}}"
                fill="{{.color}}" fill-opacity="0.6"
                stroke="#333" stroke-width="1"/>
    </template>
</svg>
```

---

## Notes

### Radius Definition

- The radius measures from the center point (`cx`, `cy`) to the edge
- The circle's diameter is `2 * r`
- The circle's area is `π * r²`

### Visual Impact

- Larger radius values create larger circles
- Very large radii may extend beyond the SVG viewport boundaries
- Consider viewport size when setting radius values

### Stroke Width Impact

- The stroke (border) is centered on the circle's edge
- Half the stroke-width extends inside the radius, half outside
- Total visual diameter: `2 * r + stroke-width`
- For precise sizing, account for stroke width in calculations

### Percentage Values

- When using percentages, the value is calculated relative to the SVG viewport
- The browser uses the smaller of viewport width or height for the calculation
- For a 400x300 viewport, `r="50%"` = 150 points (50% of 300)

### Zero and Small Radii

- `r="0"` produces no visible output
- Very small radii (< 1pt) may not render clearly in PDF output
- Minimum practical radius depends on stroke-width and output resolution

### Radial Gradients

- For `<radialGradient>`, `r` defines the size of the gradient effect
- Combined with `cx`, `cy`, and focal point attributes for directional gradients
- Larger `r` values create more gradual color transitions

---

## Examples

### Basic Circle

```html
<svg width="200" height="200">
    <circle cx="100" cy="100" r="70" fill="#2196F3"/>
</svg>
```

### Circles of Different Sizes

```html
<svg width="500" height="200">
    <circle cx="60" cy="100" r="40" fill="#f44336" opacity="0.7"/>
    <circle cx="160" cy="100" r="50" fill="#4CAF50" opacity="0.7"/>
    <circle cx="280" cy="100" r="60" fill="#2196F3" opacity="0.7"/>
    <circle cx="420" cy="100" r="70" fill="#FF9800" opacity="0.7"/>
</svg>
```

### Concentric Circles

```html
<svg width="300" height="300">
    <circle cx="150" cy="150" r="120" fill="none" stroke="#e74c3c" stroke-width="2"/>
    <circle cx="150" cy="150" r="90" fill="none" stroke="#e74c3c" stroke-width="2"/>
    <circle cx="150" cy="150" r="60" fill="none" stroke="#e74c3c" stroke-width="2"/>
    <circle cx="150" cy="150" r="30" fill="#e74c3c"/>
</svg>
```

### Target Diagram

```html
<svg width="250" height="250">
    <circle cx="125" cy="125" r="110" fill="#FFF3E0"/>
    <circle cx="125" cy="125" r="85" fill="#FFE0B2"/>
    <circle cx="125" cy="125" r="60" fill="#FFCC80"/>
    <circle cx="125" cy="125" r="35" fill="#FFB74D"/>
    <circle cx="125" cy="125" r="10" fill="#FF9800"/>
</svg>
```

### Bubble Chart

```html
<svg width="500" height="300">
    <!-- Small bubbles -->
    <circle cx="100" cy="150" r="25" fill="#e74c3c" opacity="0.6"/>
    <circle cx="400" cy="200" r="30" fill="#f39c12" opacity="0.6"/>

    <!-- Medium bubbles -->
    <circle cx="250" cy="180" r="45" fill="#3498db" opacity="0.6"/>
    <circle cx="350" cy="120" r="40" fill="#2ecc71" opacity="0.6"/>

    <!-- Large bubbles -->
    <circle cx="180" cy="100" r="60" fill="#9b59b6" opacity="0.6"/>
    <circle cx="320" cy="220" r="55" fill="#16a085" opacity="0.6"/>
</svg>
```

### Ripple Effect

```html
<svg width="300" height="300">
    <circle cx="150" cy="150" r="20" fill="#2196F3" opacity="1.0"/>
    <circle cx="150" cy="150" r="40" fill="none" stroke="#2196F3" stroke-width="3" opacity="0.7"/>
    <circle cx="150" cy="150" r="60" fill="none" stroke="#2196F3" stroke-width="2" opacity="0.4"/>
    <circle cx="150" cy="150" r="80" fill="none" stroke="#2196F3" stroke-width="1" opacity="0.2"/>
</svg>
```

### Venn Diagram with Equal Radii

```html
<svg width="350" height="250">
    <circle cx="140" cy="125" r="90" fill="#e74c3c" fill-opacity="0.5"/>
    <circle cx="210" cy="125" r="90" fill="#3498db" fill-opacity="0.5"/>
</svg>
```

### Loading Spinner

```html
<svg width="200" height="200">
    <circle cx="100" cy="100" r="60" fill="none" stroke="#e0e0e0" stroke-width="8"/>
    <circle cx="100" cy="100" r="60" fill="none" stroke="#2196F3" stroke-width="8"
            stroke-dasharray="188.4 94.2" transform="rotate(-90, 100, 100)"/>
</svg>
```

### Progress Ring

```html
<svg width="180" height="180">
    <!-- Background ring -->
    <circle cx="90" cy="90" r="70" fill="none" stroke="#e0e0e0" stroke-width="12"/>

    <!-- Progress ring (75% complete) -->
    <circle cx="90" cy="90" r="70" fill="none" stroke="#4CAF50" stroke-width="12"
            stroke-dasharray="329.7 109.9" stroke-linecap="round"
            transform="rotate(-90, 90, 90)"/>
</svg>
```

### Radial Gradient with Custom Radius

```html
<svg width="400" height="300">
    <defs>
        <radialGradient id="grad3" cx="50%" cy="50%" r="60%">
            <stop offset="0%" style="stop-color:#FFEB3B;stop-opacity:1"/>
            <stop offset="50%" style="stop-color:#FF9800;stop-opacity:1"/>
            <stop offset="100%" style="stop-color:#F44336;stop-opacity:1"/>
        </radialGradient>
    </defs>
    <rect x="0" y="0" width="400" height="300" fill="url(#grad3)"/>
</svg>
```

### Status Indicators Different Sizes

```html
<svg width="300" height="100">
    <circle cx="50" cy="50" r="8" fill="#f44336"/>
    <text x="70" y="55" font-size="12">Critical</text>

    <circle cx="150" cy="50" r="6" fill="#FF9800"/>
    <text x="165" y="55" font-size="12">Warning</text>

    <circle cx="240" cy="50" r="8" fill="#4CAF50"/>
    <text x="260" y="55" font-size="12">OK</text>
</svg>
```

### Pie Chart Slice Marker

```html
<svg width="200" height="200">
    <!-- Background -->
    <circle cx="100" cy="100" r="80" fill="none" stroke="#e0e0e0" stroke-width="40"/>

    <!-- Segment 1 (50%) -->
    <circle cx="100" cy="100" r="80" fill="none" stroke="#4CAF50" stroke-width="40"
            stroke-dasharray="251.2 251.2" transform="rotate(-90, 100, 100)"/>

    <!-- Segment 2 (30%) -->
    <circle cx="100" cy="100" r="80" fill="none" stroke="#2196F3" stroke-width="40"
            stroke-dasharray="150.7 351.7" transform="rotate(90, 100, 100)"/>
</svg>
```

### Molecule Atoms with Variable Sizes

```html
<svg width="400" height="300">
    <!-- Central large atom -->
    <circle cx="200" cy="150" r="40" fill="#2196F3" stroke="#1565C0" stroke-width="3"/>

    <!-- Surrounding medium atoms -->
    <circle cx="100" cy="100" r="25" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
    <circle cx="300" cy="100" r="25" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
    <circle cx="100" cy="200" r="25" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
    <circle cx="300" cy="200" r="25" fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>

    <!-- Small hydrogen atoms -->
    <circle cx="60" cy="80" r="15" fill="#FF9800" stroke="#E65100" stroke-width="1"/>
    <circle cx="340" cy="80" r="15" fill="#FF9800" stroke="#E65100" stroke-width="1"/>
</svg>
```

### Dynamic Radius Based on Data

```html
<!-- Model: { dataPoints: [
    {cx: 80, cy: 150, value: 30, label: 'A'},
    {cx: 180, cy: 150, value: 60, label: 'B'},
    {cx: 280, cy: 150, value: 45, label: 'C'},
    {cx: 380, cy: 150, value: 80, label: 'D'}
], maxRadius: 50 } -->
<svg width="460" height="300">
    <template data-bind="{{dataPoints}}">
        <circle cx="{{.cx}}" cy="{{.cy}}"
                r="{{(.value / 100) * maxRadius}}"
                fill="#2196F3" opacity="0.6"/>
    </template>
</svg>
```

### Size Comparison Chart

```html
<svg width="550" height="300">
    <!-- Earth -->
    <circle cx="100" cy="150" r="40" fill="#2196F3" opacity="0.7"/>
    <text x="100" y="210" text-anchor="middle" font-size="12">Earth</text>

    <!-- Mars -->
    <circle cx="220" cy="170" r="21" fill="#e74c3c" opacity="0.7"/>
    <text x="220" y="210" text-anchor="middle" font-size="12">Mars</text>

    <!-- Jupiter -->
    <circle cx="380" cy="100" r="80" fill="#f39c12" opacity="0.7"/>
    <text x="380" y="210" text-anchor="middle" font-size="12">Jupiter</text>
</svg>
```

### Radar Chart Points

```html
<svg width="300" height="300">
    <!-- Grid circles -->
    <circle cx="150" cy="150" r="120" fill="none" stroke="#e0e0e0" stroke-width="1"/>
    <circle cx="150" cy="150" r="90" fill="none" stroke="#e0e0e0" stroke-width="1"/>
    <circle cx="150" cy="150" r="60" fill="none" stroke="#e0e0e0" stroke-width="1"/>
    <circle cx="150" cy="150" r="30" fill="none" stroke="#e0e0e0" stroke-width="1"/>

    <!-- Data points -->
    <circle cx="150" cy="30" r="6" fill="#2196F3"/>
    <circle cx="254" cy="104" r="6" fill="#2196F3"/>
    <circle cx="221" cy="221" r="6" fill="#2196F3"/>
    <circle cx="79" cy="221" r="6" fill="#2196F3"/>
    <circle cx="46" cy="104" r="6" fill="#2196F3"/>
</svg>
```

### Animated Growth Circles (Static Sizes)

```html
<svg width="400" height="150">
    <circle cx="100" cy="75" r="10" fill="#4CAF50" opacity="0.3"/>
    <circle cx="150" cy="75" r="20" fill="#4CAF50" opacity="0.5"/>
    <circle cx="210" cy="75" r="30" fill="#4CAF50" opacity="0.7"/>
    <circle cx="280" cy="75" r="40" fill="#4CAF50" opacity="0.9"/>
</svg>
```

---

## See Also

- [cx](/reference/svgattributes/cx.html) - Center X coordinate attribute
- [cy](/reference/svgattributes/cy.html) - Center Y coordinate attribute
- [rx](/reference/svgattributes/rx.html) - Horizontal radius for ellipses
- [ry](/reference/svgattributes/ry.html) - Vertical radius for ellipses
- [circle](/reference/svgelements/circle.html) - SVG circle element
- [radialGradient](/reference/svgelements/radialGradient.html) - SVG radial gradient element
- [SVG Transforms](/reference/svgelements/transforms.html) - Transformation operations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
