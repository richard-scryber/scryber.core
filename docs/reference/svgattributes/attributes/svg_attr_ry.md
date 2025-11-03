---
layout: default
title: ry
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @ry : Vertical Radius Attribute

The `ry` attribute specifies the vertical radius for elliptical shapes and rounded corners within the SVG coordinate system. It defines the vertical dimension for ellipses and the vertical corner curvature for rectangles in your PDF documents.

## Usage

The `ry` attribute sets the vertical radius:
- For `<ellipse>`: Vertical radius (height dimension) of the ellipse
- For `<rect>`: Vertical radius for rounded corners
- When used with `rx`, creates oval shapes or asymmetrically rounded corners

```html
<svg xmlns="http://www.w3.org/2000/svg" width="300" height="200">
    <ellipse cx="150" cy="100" rx="80" ry="70" fill="#4CAF50"/>
</svg>
```

---

## Supported Values

The `ry` attribute accepts unit values:

| Unit Type | Example | Description |
|-----------|---------|-------------|
| Points | `ry="50"` or `ry="50pt"` | Default unit, 1/72 of an inch |
| Pixels | `ry="50px"` | Screen pixels |
| Inches | `ry="1in"` | Physical inches |
| Centimeters | `ry="2cm"` | Metric centimeters |
| Millimeters | `ry="20mm"` | Metric millimeters |
| Percentage | `ry="25%"` | Percentage of parent viewport height |

**Default Value:**
- For `<ellipse>`: `0` (no visible ellipse)
- For `<rect>`: Defaults to `rx` value if `rx` is specified, otherwise `0` (sharp corners)

**Constraints:**
- Must be a positive value or zero
- Negative values are invalid
- For `<rect>`, `ry` is capped at half the rectangle's height

---

## Supported Elements

The `ry` attribute is supported on the following SVG elements:

| Element | Usage |
|---------|-------|
| `<ellipse>` | Vertical radius (distance from center to edge along Y-axis) |
| `<rect>` | Vertical radius for rounded corners |

---

## Data Binding

The `ry` attribute supports dynamic values using data binding expressions with `{{expression}}` syntax.

### Example 1: Dynamic Ellipse Height

```html
<!-- Model: { oval: { cx: 150, cy: 100, rx: 90, ry: 60, color: '#FF5722' } } -->
<svg width="300" height="200">
    <ellipse cx="{{oval.cx}}" cy="{{oval.cy}}"
             rx="{{oval.rx}}" ry="{{oval.ry}}"
             fill="{{oval.color}}"/>
</svg>
```

### Example 2: Adaptive Corner Rounding

```html
<!-- Model: { card: { width: 200, height: 140, radiusX: 10, radiusY: 15 } } -->
<svg width="250" height="180">
    <rect x="25" y="20"
          width="{{card.width}}" height="{{card.height}}"
          rx="{{card.radiusX}}" ry="{{card.radiusY}}"
          fill="#9C27B0" stroke="#6A1B9A" stroke-width="2"/>
</svg>
```

### Example 3: Variable Height Ellipses

```html
<!-- Model: { dataItems: [
    {cx: 100, cy: 100, rx: 50, ry: 30, color: '#e74c3c'},
    {cx: 220, cy: 100, rx: 50, ry: 50, color: '#3498db'},
    {cx: 340, cy: 100, rx: 50, ry: 70, color: '#2ecc71'}
]} -->
<svg width="440" height="200">
    <template data-bind="{{dataItems}}">
        <ellipse cx="{{.cx}}" cy="{{.cy}}"
                 rx="{{.rx}}" ry="{{.ry}}"
                 fill="{{.color}}" opacity="0.6"/>
    </template>
</svg>
```

---

## Notes

### Ellipse Vertical Dimension

- `ry` defines half the ellipse's height
- Total height of ellipse: `2 * ry`
- Works with `rx` to create oval shapes
- When `ry` equals `rx`, the ellipse becomes a circle

### Rectangle Rounded Corners

- `ry` creates vertical curvature of corners
- If only `ry` is specified, `rx` defaults to the same value (symmetrical corners)
- If only `rx` is specified, `ry` defaults to match `rx`
- Maximum effective `ry` is half the rectangle's height
- Values larger than half-height are automatically capped

### Asymmetric Rounding

- Use different `rx` and `ry` values for elliptical corner rounding
- Example: `rx="10" ry="20"` creates taller vertical corner curves
- Useful for specialized button shapes and design elements

### Percentage Values

- For `<ellipse>`: Percentage relative to viewport height
- For `<rect>`: Percentage relative to the rectangle's height
- Helps create responsive designs that scale proportionally

### Auto-Matching Behavior

- If `ry` is omitted but `rx` is specified, `ry` automatically equals `rx`
- If `rx` is omitted but `ry` is specified, `rx` automatically equals `ry`
- This ensures symmetrical rounding when only one value is provided

### Visual Considerations

- Larger `ry` values create taller, more vertical ovals
- Small `ry` with large `rx` creates wide, flat ovals
- For pill shapes on tall rectangles, set `ry` to half the rectangle's width
- Subtle rounded corners typically use `ry` values of 4-12pt

---

## Examples

### Basic Ellipse

```html
<svg width="250" height="200">
    <ellipse cx="125" cy="100" rx="80" ry="70" fill="#2196F3"/>
</svg>
```

### Vertical Oval

```html
<svg width="200" height="300">
    <ellipse cx="100" cy="150" rx="50" ry="120" fill="#4CAF50"/>
</svg>
```

### Rounded Rectangle (Symmetrical)

```html
<svg width="250" height="150">
    <rect x="25" y="25" width="200" height="100" rx="12" ry="12"
          fill="#FF9800" stroke="#E65100" stroke-width="2"/>
</svg>
```

### Rounded Rectangle (Elliptical Corners)

```html
<svg width="250" height="180">
    <rect x="25" y="25" width="200" height="130" rx="15" ry="25"
          fill="#9C27B0" stroke="#6A1B9A" stroke-width="2"/>
</svg>
```

### Tall Pill Button

```html
<svg width="100" height="200">
    <!-- ry set to half the width creates vertical pill shape -->
    <rect x="10" y="10" width="80" height="180" rx="40" ry="40"
          fill="#2196F3"/>
</svg>
```

### Elongated Badge

```html
<svg width="120" height="80">
    <rect x="10" y="10" width="100" height="60" rx="10" ry="20"
          fill="#f44336" stroke="#c62828" stroke-width="2"/>
</svg>
```

### Card with Moderate Rounding

```html
<svg width="300" height="250">
    <rect x="20" y="20" width="260" height="210" rx="8" ry="8"
          fill="#fff" stroke="#ddd" stroke-width="2"/>
</svg>
```

### Ellipses of Different Heights

```html
<svg width="400" height="300">
    <!-- Short -->
    <ellipse cx="100" cy="150" rx="60" ry="30" fill="#e74c3c" opacity="0.6"/>

    <!-- Medium -->
    <ellipse cx="200" cy="150" rx="60" ry="60" fill="#3498db" opacity="0.6"/>

    <!-- Tall -->
    <ellipse cx="300" cy="150" rx="60" ry="90" fill="#2ecc71" opacity="0.6"/>
</svg>
```

### Speech Bubble (Vertical Orientation)

```html
<svg width="180" height="250">
    <ellipse cx="90" cy="125" rx="70" ry="110"
             fill="#fff" stroke="#333" stroke-width="2"/>
    <!-- Tail -->
    <polygon points="90,220 100,260 75,230" fill="#fff" stroke="#333" stroke-width="2"/>
</svg>
```

### Vertical Progress Bar with Rounded Ends

```html
<svg width="60" height="400">
    <!-- Background -->
    <rect x="20" y="10" width="20" height="380" rx="10" ry="10"
          fill="#e0e0e0"/>

    <!-- Progress (70%) -->
    <rect x="20" y="124" width="20" height="266" rx="10" ry="10"
          fill="#4CAF50"/>
</svg>
```

### Orbital Ellipse (Tilted)

```html
<svg width="300" height="400">
    <!-- Sun -->
    <circle cx="150" cy="200" r="25" fill="#FFC107"/>

    <!-- Vertical elliptical orbit -->
    <ellipse cx="150" cy="200" rx="80" ry="150"
             fill="none" stroke="#ccc" stroke-width="2" stroke-dasharray="5,3"/>

    <!-- Planet -->
    <circle cx="150" cy="50" r="12" fill="#2196F3"/>
</svg>
```

### Magnifying Glass Lens

```html
<svg width="150" height="150">
    <!-- Lens (circular) -->
    <ellipse cx="60" cy="60" rx="45" ry="45"
             fill="none" stroke="#333" stroke-width="4"/>

    <!-- Handle -->
    <line x1="90" y1="90" x2="130" y2="130"
          stroke="#333" stroke-width="6" stroke-linecap="round"/>
</svg>
```

### Status Badges Different Shapes

```html
<svg width="400" height="100">
    <!-- Wide badge -->
    <rect x="20" y="30" width="110" height="40" rx="20" ry="10"
          fill="#4CAF50"/>

    <!-- Balanced badge -->
    <rect x="150" y="30" width="110" height="40" rx="15" ry="15"
          fill="#2196F3"/>

    <!-- Tall badge -->
    <rect x="280" y="30" width="110" height="40" rx="10" ry="20"
          fill="#FF9800"/>
</svg>
```

### Balloon Shape

```html
<svg width="200" height="280">
    <!-- Balloon body -->
    <ellipse cx="100" cy="120" rx="70" ry="90"
             fill="#f44336" stroke="#c62828" stroke-width="2"/>

    <!-- String -->
    <line x1="100" y1="210" x2="100" y2="260"
          stroke="#333" stroke-width="2"/>

    <!-- Knot -->
    <circle cx="100" cy="210" r="5" fill="#333"/>
</svg>
```

### Teardrop Shape

```html
<svg width="180" height="240">
    <ellipse cx="90" cy="140" rx="60" ry="80"
             fill="#2196F3" opacity="0.7"/>
    <circle cx="90" cy="100" r="50"
            fill="#2196F3" opacity="0.7"/>
</svg>
```

### Button Hover States

```html
<svg width="400" height="220">
    <!-- Default (subtle rounding) -->
    <rect x="30" y="30" width="120" height="50" rx="6" ry="6"
          fill="#2196F3"/>

    <!-- Hover (medium rounding) -->
    <rect x="180" y="30" width="120" height="50" rx="8" ry="12"
          fill="#1976D2"/>

    <!-- Active (more pronounced) -->
    <rect x="330" y="35" width="120" height="45" rx="10" ry="15"
          fill="#0D47A1"/>
</svg>
```

### Perspective Coin

```html
<svg width="200" height="80">
    <!-- Coin viewed at angle (flattened ellipse) -->
    <ellipse cx="100" cy="40" rx="60" ry="20"
             fill="#FFD700" stroke="#FFA500" stroke-width="3"/>

    <!-- Inner circle detail -->
    <ellipse cx="100" cy="40" rx="45" ry="15"
             fill="none" stroke="#FFA500" stroke-width="2"/>
</svg>
```

### Toggle Switch Background

```html
<svg width="80" height="50">
    <!-- Track -->
    <rect x="10" y="15" width="60" height="20" rx="10" ry="10"
          fill="#e0e0e0"/>

    <!-- Thumb -->
    <circle cx="25" cy="25" r="13" fill="#fff" stroke="#ccc" stroke-width="2"/>
</svg>
```

### Loading Bar Segments

```html
<svg width="60" height="400">
    <rect x="20" y="20" width="20" height="80" rx="10" ry="10" fill="#4CAF50"/>
    <rect x="20" y="110" width="20" height="80" rx="10" ry="10" fill="#4CAF50"/>
    <rect x="20" y="200" width="20" height="80" rx="10" ry="10" fill="#4CAF50"/>
    <rect x="20" y="290" width="20" height="80" rx="10" ry="10" fill="#e0e0e0"/>
</svg>
```

### Dynamic Vertical Radius

```html
<!-- Model: { badges: [
    {x: 20, y: 30, ry: 8, label: 'Small'},
    {x: 150, y: 30, ry: 15, label: 'Medium'},
    {x: 280, y: 30, ry: 22, label: 'Large'}
], badgeWidth: 110, badgeHeight: 44 } -->
<svg width="410" height="100">
    <template data-bind="{{badges}}">
        <rect x="{{.x}}" y="{{.y}}"
              width="{{badgeWidth}}" height="{{badgeHeight}}"
              rx="12" ry="{{.ry}}"
              fill="#9C27B0"/>
    </template>
</svg>
```

### Aspect Ratio Comparison

```html
<svg width="500" height="350">
    <!-- Flat ellipse -->
    <ellipse cx="120" cy="100" rx="100" ry="40" fill="#e74c3c" opacity="0.6"/>
    <text x="120" y="160" text-anchor="middle" font-size="12">rx=100, ry=40</text>

    <!-- Circular -->
    <ellipse cx="280" cy="100" rx="60" ry="60" fill="#3498db" opacity="0.6"/>
    <text x="280" y="160" text-anchor="middle" font-size="12">rx=60, ry=60</text>

    <!-- Tall ellipse -->
    <ellipse cx="420" cy="150" rx="40" ry="100" fill="#2ecc71" opacity="0.6"/>
    <text x="420" y="270" text-anchor="middle" font-size="12">rx=40, ry=100</text>
</svg>
```

### Vertical Gauge Indicator

```html
<svg width="120" height="300">
    <!-- Gauge background -->
    <rect x="40" y="30" width="40" height="240" rx="20" ry="20"
          fill="#e0e0e0"/>

    <!-- Fill level (60%) -->
    <rect x="40" y="126" width="40" height="144" rx="20" ry="20"
          fill="#4CAF50"/>

    <!-- Current level marker -->
    <circle cx="60" cy="126" r="8" fill="#2E7D32" stroke="#fff" stroke-width="2"/>
</svg>
```

---

## See Also

- [rx](/reference/svgattributes/rx.html) - Horizontal radius attribute
- [r](/reference/svgattributes/r.html) - Radius attribute for circles
- [cy](/reference/svgattributes/cy.html) - Center Y coordinate attribute
- [height](/reference/svgattributes/height.html) - Height attribute
- [ellipse](/reference/svgelements/ellipse.html) - SVG ellipse element
- [rect](/reference/svgelements/rect.html) - SVG rectangle element
- [circle](/reference/svgelements/circle.html) - SVG circle element
- [SVG Transforms](/reference/svgelements/transforms.html) - Transformation operations
- [Data Binding](/reference/binding/) - Data binding and expressions

---
