---
layout: default
title: rx
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @rx : Horizontal Radius Attribute

The `rx` attribute specifies the horizontal radius for elliptical shapes and rounded corners within the SVG coordinate system. It defines the horizontal dimension for ellipses and the corner curvature for rectangles in your PDF documents.

## Usage

The `rx` attribute sets the horizontal radius:
- For `<ellipse>`: Horizontal radius (width dimension) of the ellipse
- For `<rect>`: Horizontal radius for rounded corners
- When used with `ry`, creates oval shapes or asymmetrically rounded corners

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="150">
    <ellipse cx="150" cy="75" rx="120" ry="50" fill="#9C27B0"/>
</svg>
```

---

## Supported Values

The `rx` attribute accepts unit values:

| Unit Type | Example | Description |
|-----------|---------|-------------|
| Points | `rx="60"` or `rx="60pt"` | Default unit, 1/72 of an inch |
| Pixels | `rx="60px"` | Screen pixels |
| Inches | `rx="1in"` | Physical inches |
| Centimeters | `rx="3cm"` | Metric centimeters |
| Millimeters | `rx="30mm"` | Metric millimeters |
| Percentage | `rx="30%"` | Percentage of parent viewport width |

**Default Value:**
- For `<ellipse>`: `0` (no visible ellipse)
- For `<rect>`: `0` (sharp corners)

**Constraints:**
- Must be a positive value or zero
- Negative values are invalid
- For `<rect>`, `rx` is capped at half the rectangle's width

---

## Supported Elements

The `rx` attribute is supported on the following SVG elements:

| Element | Usage |
|---------|-------|
| `<ellipse>` | Horizontal radius (distance from center to edge along X-axis) |
| `<rect>` | Horizontal radius for rounded corners |

---

## Data Binding

The `rx` attribute supports dynamic values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Ellipse Size

```html
<!-- Model: { oval: { cx: 150, cy: 80, rx: 100, ry: 50, color: '#FF5722' } } -->
<svg width="300" height="160">
    <ellipse cx="{{oval.cx}}" cy="{{oval.cy}}"
             rx="{{oval.rx}}" ry="{{oval.ry}}"
             fill="{{oval.color}}"/>
</svg>
```

### Example 2: Rounded Corners Based on Size

```html
<!-- Model: { card: { width: 200, height: 120, cornerRadius: 12 } } -->
<svg width="250" height="150">
    <rect x="25" y="15"
          width="{{card.width}}" height="{{card.height}}"
          rx="{{card.cornerRadius}}" ry="{{card.cornerRadius}}"
          fill="#2196F3" stroke="#1565C0" stroke-width="2"/>
</svg>
```

### Example 3: Variable Ellipses in Charts

```html
<!-- Model: { dataPoints: [
    {cx: 80, cy: 100, rx: 40, ry: 25, color: '#e74c3c'},
    {cx: 200, cy: 100, rx: 60, ry: 35, color: '#3498db'},
    {cx: 340, cy: 100, rx: 50, ry: 30, color: '#2ecc71'}
]} -->
<svg width="420" height="200">
    <template data-bind="{{dataPoints}}">
        <ellipse cx="{{.cx}}" cy="{{.cy}}"
                 rx="{{.rx}}" ry="{{.ry}}"
                 fill="{{.color}}" opacity="0.6"/>
    </template>
</svg>
```

---

## Notes

### Ellipse Horizontal Dimension

- `rx` defines half the ellipse's width
- Total width of ellipse: `2 * rx`
- Works with `ry` to create oval shapes
- When `rx` equals `ry`, the ellipse becomes a circle

### Rectangle Rounded Corners

- `rx` creates horizontal curvature of corners
- If only `rx` is specified, `ry` defaults to the same value (symmetrical corners)
- Maximum effective `rx` is half the rectangle's width
- Values larger than half-width are automatically capped

### Asymmetric Rounding

- Use different `rx` and `ry` values for elliptical corner rounding
- Example: `rx="20" ry="10"` creates wider horizontal corner curves
- Useful for custom badge shapes and design elements

### Percentage Values

- For `<ellipse>`: Percentage relative to viewport width
- For `<rect>`: Percentage relative to the rectangle's width
- Helps create responsive designs that scale proportionally

### Zero and Auto Values

- `rx="0"` creates sharp corners (rectangles) or no shape (ellipses)
- If `rx` is specified but `ry` is not, `ry` automatically equals `rx`
- If `ry` is specified but `rx` is not, `rx` automatically equals `ry`

### Visual Considerations

- Larger `rx` values create wider, more horizontal ovals
- Small `rx` with large `ry` creates tall, narrow ovals
- For subtle rounded corners on rectangles, use `rx` values of 4-12pt
- For pill shapes, set `rx` to half the rectangle's height

---

## Examples

### Basic Ellipse

```html
<svg width="300" height="150">
    <ellipse cx="150" cy="75" rx="120" ry="50" fill="#9C27B0"/>
</svg>
```

### Horizontal Oval

```html
<svg width="350" height="120">
    <ellipse cx="175" cy="60" rx="150" ry="40" fill="#2196F3"/>
</svg>
```

### Rounded Rectangle (Symmetrical)

```html
<svg width="250" height="150">
    <rect x="25" y="25" width="200" height="100" rx="15" ry="15"
          fill="#4CAF50" stroke="#2E7D32" stroke-width="2"/>
</svg>
```

### Rounded Rectangle (Elliptical Corners)

```html
<svg width="250" height="150">
    <rect x="25" y="25" width="200" height="100" rx="30" ry="15"
          fill="#FF9800" stroke="#E65100" stroke-width="2"/>
</svg>
```

### Pill Button Shape

```html
<svg width="200" height="60">
    <!-- rx set to half the height creates pill shape -->
    <rect x="10" y="10" width="180" height="40" rx="20" ry="20"
          fill="#2196F3"/>
</svg>
```

### Badge with Rounded Corners

```html
<svg width="150" height="50">
    <rect x="10" y="10" width="130" height="30" rx="8" ry="8"
          fill="#f44336" stroke="#c62828" stroke-width="2"/>
</svg>
```

### Card with Subtle Rounding

```html
<svg width="300" height="200">
    <rect x="20" y="20" width="260" height="160" rx="6" ry="6"
          fill="#fff" stroke="#ddd" stroke-width="1"/>
</svg>
```

### Ellipses of Different Widths

```html
<svg width="400" height="300">
    <!-- Narrow -->
    <ellipse cx="100" cy="100" rx="30" ry="60" fill="#e74c3c" opacity="0.6"/>

    <!-- Medium -->
    <ellipse cx="200" cy="100" rx="60" ry="60" fill="#3498db" opacity="0.6"/>

    <!-- Wide -->
    <ellipse cx="300" cy="100" rx="90" ry="60" fill="#2ecc71" opacity="0.6"/>
</svg>
```

### Speech Bubble

```html
<svg width="250" height="120">
    <ellipse cx="125" cy="60" rx="110" ry="50"
             fill="#fff" stroke="#333" stroke-width="2"/>
    <!-- Tail -->
    <polygon points="100,90 80,120 120,95" fill="#fff" stroke="#333" stroke-width="2"/>
</svg>
```

### Progress Bar with Rounded Ends

```html
<svg width="400" height="40">
    <!-- Background -->
    <rect x="10" y="10" width="380" height="20" rx="10" ry="10"
          fill="#e0e0e0"/>

    <!-- Progress (65%) -->
    <rect x="10" y="10" width="247" height="20" rx="10" ry="10"
          fill="#4CAF50"/>
</svg>
```

### Orbital Ellipse

```html
<svg width="400" height="300">
    <!-- Sun -->
    <circle cx="200" cy="150" r="25" fill="#FFC107"/>

    <!-- Elliptical orbit -->
    <ellipse cx="200" cy="150" rx="150" ry="100"
             fill="none" stroke="#ccc" stroke-width="2" stroke-dasharray="5,3"/>

    <!-- Planet -->
    <circle cx="350" cy="150" r="12" fill="#2196F3"/>
</svg>
```

### Eye Icon

```html
<svg width="80" height="50">
    <!-- Eye outline -->
    <ellipse cx="40" cy="25" rx="35" ry="20"
             fill="none" stroke="#333" stroke-width="2"/>

    <!-- Pupil -->
    <circle cx="40" cy="25" r="8" fill="#333"/>

    <!-- Highlight -->
    <circle cx="37" cy="22" r="3" fill="#fff"/>
</svg>
```

### Status Badge Variations

```html
<svg width="400" height="150">
    <!-- Very rounded (pill) -->
    <rect x="20" y="30" width="100" height="40" rx="20" ry="20"
          fill="#4CAF50"/>

    <!-- Moderately rounded -->
    <rect x="150" y="30" width="100" height="40" rx="8" ry="8"
          fill="#2196F3"/>

    <!-- Slightly rounded -->
    <rect x="280" y="30" width="100" height="40" rx="4" ry="4"
          fill="#FF9800"/>
</svg>
```

### Cloud Shape

```html
<svg width="300" height="150">
    <ellipse cx="70" cy="90" rx="45" ry="35" fill="#90CAF9"/>
    <ellipse cx="120" cy="75" rx="55" ry="40" fill="#90CAF9"/>
    <ellipse cx="180" cy="80" rx="50" ry="38" fill="#90CAF9"/>
    <ellipse cx="230" cy="95" rx="40" ry="30" fill="#90CAF9"/>
</svg>
```

### Button States

```html
<svg width="400" height="200">
    <!-- Default button -->
    <rect x="30" y="30" width="120" height="45" rx="6" ry="6"
          fill="#2196F3"/>

    <!-- Hover button (slightly more rounded) -->
    <rect x="180" y="30" width="120" height="45" rx="8" ry="8"
          fill="#1976D2"/>

    <!-- Pressed button (more rounded) -->
    <rect x="330" y="35" width="120" height="40" rx="10" ry="10"
          fill="#0D47A1"/>
</svg>
```

### Perspective Cylinder Top

```html
<svg width="200" height="250">
    <!-- Cylinder top (ellipse) -->
    <ellipse cx="100" cy="50" rx="70" ry="20"
             fill="#2196F3" stroke="#1565C0" stroke-width="2"/>

    <!-- Cylinder side -->
    <rect x="30" y="50" width="140" height="150"
          fill="#1976D2"/>

    <!-- Cylinder bottom -->
    <ellipse cx="100" cy="200" rx="70" ry="20"
             fill="#0D47A1" stroke="#1565C0" stroke-width="2"/>
</svg>
```

### Tag Shape

```html
<svg width="150" height="50">
    <rect x="10" y="10" width="120" height="30" rx="15" ry="15"
          fill="#E3F2FD" stroke="#2196F3" stroke-width="2"/>
    <circle cx="30" cy="25" r="5" fill="#2196F3"/>
</svg>
```

### Loading Bar Segments

```html
<svg width="400" height="60">
    <rect x="20" y="20" width="80" height="20" rx="10" ry="10" fill="#4CAF50"/>
    <rect x="110" y="20" width="80" height="20" rx="10" ry="10" fill="#4CAF50"/>
    <rect x="200" y="20" width="80" height="20" rx="10" ry="10" fill="#4CAF50"/>
    <rect x="290" y="20" width="80" height="20" rx="10" ry="10" fill="#e0e0e0"/>
</svg>
```

### Dynamic Corner Radius

```html
<!-- Model: { buttons: [
    {x: 20, label: 'Small', rx: 4},
    {x: 140, label: 'Medium', rx: 8},
    {x: 260, label: 'Large', rx: 16}
], buttonWidth: 100, buttonHeight: 40 } -->
<svg width="380" height="80">
    <template data-bind="{{buttons}}">
        <rect x="{{.x}}" y="20"
              width="{{buttonWidth}}" height="{{buttonHeight}}"
              rx="{{.rx}}" ry="{{.rx}}"
              fill="#2196F3"/>
    </template>
</svg>
```

### Aspect Ratio Comparison

```html
<svg width="500" height="200">
    <!-- Wide ellipse -->
    <ellipse cx="120" cy="100" rx="100" ry="50" fill="#e74c3c" opacity="0.6"/>
    <text x="120" y="170" text-anchor="middle" font-size="12">rx=100, ry=50</text>

    <!-- Square (circle-like) -->
    <ellipse cx="300" cy="100" rx="60" ry="60" fill="#3498db" opacity="0.6"/>
    <text x="300" y="170" text-anchor="middle" font-size="12">rx=60, ry=60</text>

    <!-- Tall ellipse -->
    <ellipse cx="440" cy="100" rx="40" ry="80" fill="#2ecc71" opacity="0.6"/>
    <text x="440" y="190" text-anchor="middle" font-size="12">rx=40, ry=80</text>
</svg>
```

---

## See Also

- [ry](/reference/svgattributes/ry.html) - Vertical radius attribute
- [r](/reference/svgattributes/r.html) - Radius attribute for circles
- [cx](/reference/svgattributes/cx.html) - Center X coordinate attribute
- [width](/reference/svgattributes/width.html) - Width attribute
- [ellipse](/reference/svgelements/ellipse.html) - SVG ellipse element
- [rect](/reference/svgelements/rect.html) - SVG rectangle element
- [circle](/reference/svgelements/circle.html) - SVG circle element
- [SVG Transforms](/reference/svgelements/transforms.html) - Transformation operations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
