---
layout: default
title: fx
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @fx : The Radial Gradient Focal Point X Coordinate Attribute

The `fx` attribute defines the X-coordinate of the focal point for a radial gradient. The focal point is where the 0% color stop begins, creating the visual center of the radial color transition. By offsetting the focal point from the gradient circle's center, you can create lighting effects, depth, and directional illumination.

## Usage

The `fx` attribute is used to:
- Define the horizontal position of the radial gradient's focal point (0% stop)
- Create lighting and spotlight effects by offsetting from center
- Support both relative (percentage) and absolute (unit-based) coordinates
- Work with both objectBoundingBox (default) and userSpaceOnUse coordinate systems
- Enable data-driven lighting positions through data binding
- Simulate 3D depth and spherical surfaces
- Position highlights for buttons, badges, and UI elements

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="400">
    <defs>
        <radialGradient id="spotlight" cx="50%" cy="50%" r="50%" fx="30%" fy="30%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="100%" stop-color="#2c3e50" />
        </radialGradient>
    </defs>
    <circle cx="200" cy="200" r="180" fill="url(#spotlight)" />
</svg>
```

---

## Supported Values

| Value Type | Format | Description | Example |
|------------|--------|-------------|---------|
| Percentage | `0%` to `100%` | Relative to shape bounding box (objectBoundingBox mode) | `fx="30%"` |
| Decimal | `0` to `1.0` | Relative coordinate (0=left, 1=right) | `fx="0.3"` |
| Length Units | `pt`, `px`, `mm`, `cm`, `in` | Absolute coordinates (userSpaceOnUse mode) | `fx="100pt"` |
| Default | `cx` value | Inherits from cx if omitted (centered focal point) | - |

### Common Patterns

```html
<!-- Centered focal point (default) -->
<radialGradient id="g1" cx="50%" cy="50%" r="50%">

<!-- Top-left lighting -->
<radialGradient id="g2" cx="50%" cy="50%" r="50%" fx="30%" fy="30%">

<!-- Bottom-right lighting -->
<radialGradient id="g3" cx="50%" cy="50%" r="50%" fx="70%" fy="70%">

<!-- Extreme offset for dramatic effect -->
<radialGradient id="g4" cx="50%" cy="50%" r="50%" fx="10%" fy="10%">
```

---

## Supported Elements

The `fx` attribute is supported on:

- **[&lt;radialGradient&gt;](/reference/svgtags/radialGradient.html)** - Defines radial gradient focal point X position

Note: This attribute is NOT used with `<linearGradient>` elements.

---

## Data Binding

### Dynamic Lighting Position

Bind focal point to data for dynamic lighting effects:

```html
<!-- Model: { lightX: 35, lightY: 35 } -->
<svg width="300" height="300">
    <defs>
        <radialGradient id="dynamicLight"
                        cx="50%" cy="50%" r="50%"
                        fx="{{model.lightX}}%" fy="{{model.lightY}}%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="50%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </radialGradient>
    </defs>
    <circle cx="150" cy="150" r="130" fill="url(#dynamicLight)" />
</svg>
```

### Mouse/Touch Position Lighting

Create interactive lighting following pointer position:

```html
<!-- Model: { mouseX: 120, mouseY: 180, containerWidth: 400, containerHeight: 400 } -->
<svg width="{{model.containerWidth}}" height="{{model.containerHeight}}">
    <defs>
        <radialGradient id="mouseLighting"
                        cx="50%" cy="50%" r="70%"
                        fx="{{(model.mouseX / model.containerWidth) * 100}}%"
                        fy="{{(model.mouseY / model.containerHeight) * 100}}%">
            <stop offset="0%" stop-color="#ffffff" stop-opacity="0.8" />
            <stop offset="50%" stop-color="#95a5a6" stop-opacity="0.4" />
            <stop offset="100%" stop-color="#2c3e50" stop-opacity="0.9" />
        </radialGradient>
    </defs>
    <rect width="{{model.containerWidth}}" height="{{model.containerHeight}}"
          fill="url(#mouseLighting)" />
</svg>
```

### Data-Driven Highlight Direction

Position highlights based on data orientation:

```html
<!-- Model: { direction: "top-left", intensity: 80 } -->
<svg width="200" height="200">
    <defs>
        <radialGradient id="directionalLight"
                        cx="50%" cy="50%" r="50%"
                        fx="{{model.direction === 'top-left' ? '30%' :
                              model.direction === 'top-right' ? '70%' :
                              model.direction === 'bottom-left' ? '30%' : '70%'}}%"
                        fy="{{model.direction === 'top-left' ? '30%' :
                              model.direction === 'top-right' ? '30%' :
                              model.direction === 'bottom-left' ? '70%' : '70%'}}%">
            <stop offset="0%" stop-color="#ffffff" stop-opacity="{{model.intensity / 100}}" />
            <stop offset="100%" stop-color="#3498db" />
        </radialGradient>
    </defs>
    <circle cx="100" cy="100" r="80" fill="url(#directionalLight)" />
</svg>
```

### Status Indicator with Centered/Offset Glow

Change focal point based on status state:

```html
<!-- Model: { status: "active", isHighlighted: true } -->
<svg width="100" height="100">
    <defs>
        <radialGradient id="statusGlow"
                        cx="50%" cy="50%" r="50%"
                        fx="{{model.isHighlighted ? '40%' : '50%'}}"
                        fy="{{model.isHighlighted ? '40%' : '50%'}}">
            <stop offset="0%" stop-color="#a8e6cf" />
            <stop offset="50%" stop-color="{{model.status === 'active' ? '#2ecc71' : '#95a5a6'}}" />
            <stop offset="100%" stop-color="{{model.status === 'active' ? '#27ae60' : '#7f8c8d'}}" />
        </radialGradient>
    </defs>
    <circle cx="50" cy="50" r="40" fill="url(#statusGlow)" />
</svg>
```

### Calculated Lighting Angle

Calculate focal point from angle data:

```html
<!-- Model: { lightAngle: 315, distance: 20 } -->
<svg width="300" height="300">
    <defs>
        <radialGradient id="angleLighting"
                        cx="50%" cy="50%" r="50%"
                        fx="{{50 + Math.cos(model.lightAngle * Math.PI / 180) * model.distance}}%"
                        fy="{{50 + Math.sin(model.lightAngle * Math.PI / 180) * model.distance}}%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="100%" stop-color="#9b59b6" />
        </radialGradient>
    </defs>
    <circle cx="150" cy="150" r="120" fill="url(#angleLighting)" />
</svg>
```

---

## Notes

### Focal Point Behavior

The focal point defines where the gradient's 0% color stop appears:
- Default: focal point matches gradient center (fx=cx, fy=cy)
- Offset focal points create asymmetric gradients
- Focal point must be within the gradient circle
- Creates perception of light source direction

### Visual Effects

Different focal point positions create various effects:

1. **Centered (fx=cx, fy=cy)**: Even, symmetrical radial gradient
2. **Top-Left (fx<cx, fy<cy)**: Light from top-left, shadow bottom-right
3. **Bottom-Right (fx>cx, fy>cy)**: Light from bottom-right, shadow top-left
4. **Extreme Offset**: Dramatic lighting, strong directional effect

### Coordinate Systems

The `fx` attribute behaves differently based on `gradientUnits`:

1. **objectBoundingBox (default)**: Relative to filled shape
   - Values 0-1 or 0%-100%
   - `50%` = horizontal center of shape
   - Gradient scales automatically with shape size

2. **userSpaceOnUse**: Absolute document coordinates
   - Values in pt, px, mm, cm, in
   - Fixed position in document space
   - Consistent across multiple shapes

### Common Use Cases

Focal point positioning is ideal for:
- 3D button effects with depth
- Spherical objects with realistic lighting
- Glowing indicators with directional light
- Badge and icon highlights
- Spotlight effects on backgrounds
- Gauge and meter depth effects
- Status indicators with shine
- Interactive elements responding to input

### Relationship with Other Attributes

- **cx, cy, r**: Define the outer gradient circle (100% stop position)
- **fx, fy**: Define the focal point (0% stop position)
- **fr**: Inner radius around focal point (advanced)
- Gradient transitions from (fx, fy) to (cx, cy, r)

---

## Examples

### Basic Centered Focal Point (Default)

```html
<svg width="200" height="200">
    <defs>
        <radialGradient id="centered" cx="50%" cy="50%" r="50%">
            <stop offset="0%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </radialGradient>
    </defs>
    <circle cx="100" cy="100" r="80" fill="url(#centered)" />
</svg>
```

### Top-Left Lighting Effect

```html
<svg width="200" height="200">
    <defs>
        <radialGradient id="topLeftLight" cx="50%" cy="50%" r="50%" fx="30%" fy="30%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="50%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </radialGradient>
    </defs>
    <circle cx="100" cy="100" r="80" fill="url(#topLeftLight)" />
</svg>
```

### Bottom-Right Lighting Effect

```html
<svg width="200" height="200">
    <defs>
        <radialGradient id="bottomRightLight" cx="50%" cy="50%" r="50%" fx="70%" fy="70%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="50%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#c0392b" />
        </radialGradient>
    </defs>
    <circle cx="100" cy="100" r="80" fill="url(#bottomRightLight)" />
</svg>
```

### 3D Button with Highlight

```html
<svg width="220" height="80">
    <defs>
        <radialGradient id="button3d" cx="50%" cy="50%" r="50%" fx="40%" fy="35%">
            <stop offset="0%" stop-color="#5dade2" />
            <stop offset="40%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2874a6" />
        </radialGradient>
    </defs>

    <rect width="220" height="80" rx="40" fill="url(#button3d)" />
    <text x="110" y="50" text-anchor="middle" fill="white" font-size="24" font-weight="bold">
        Click Me
    </text>
</svg>
```

### Spherical Badge with Lighting

```html
<svg width="120" height="120">
    <defs>
        <radialGradient id="badgeSphere" cx="50%" cy="50%" r="50%" fx="35%" fy="35%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="30%" stop-color="#f1c40f" />
            <stop offset="70%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e67e22" />
        </radialGradient>
    </defs>

    <circle cx="60" cy="60" r="50" fill="url(#badgeSphere)" />
    <text x="60" y="70" text-anchor="middle" fill="white" font-size="32" font-weight="bold">
        5
    </text>
</svg>
```

### Status Indicator with Glow

```html
<svg width="100" height="100">
    <defs>
        <radialGradient id="statusGlow" cx="50%" cy="50%" r="50%" fx="40%" fy="40%">
            <stop offset="0%" stop-color="#a8e6cf" />
            <stop offset="50%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </radialGradient>
    </defs>

    <circle cx="50" cy="50" r="35" fill="url(#statusGlow)" />
</svg>
```

### Gauge with Depth Effect

```html
<svg width="300" height="300">
    <defs>
        <radialGradient id="gaugeDepth" cx="50%" cy="50%" r="50%" fx="45%" fy="45%">
            <stop offset="0%" stop-color="#ecf0f1" />
            <stop offset="70%" stop-color="#bdc3c7" />
            <stop offset="100%" stop-color="#95a5a6" />
        </radialGradient>
    </defs>

    <circle cx="150" cy="150" r="120" fill="url(#gaugeDepth)" />
    <circle cx="150" cy="150" r="90" fill="white" />
    <text x="150" y="160" text-anchor="middle" font-size="48" font-weight="bold" fill="#333">
        75%
    </text>
</svg>
```

### Icon with Top Highlight

```html
<svg width="80" height="80" viewBox="0 0 24 24">
    <defs>
        <radialGradient id="iconHighlight" cx="50%" cy="50%" r="50%" fx="40%" fy="30%">
            <stop offset="0%" stop-color="#ffd93d" />
            <stop offset="100%" stop-color="#f39c12" />
        </radialGradient>
    </defs>

    <circle cx="12" cy="12" r="10" fill="url(#iconHighlight)" />
    <path d="M12 6 L9 10 L12 10 L12 14 L15 10 L12 10 Z" fill="white" />
</svg>
```

### Spotlight Background Effect

```html
<svg width="600" height="400">
    <defs>
        <radialGradient id="spotlight" cx="50%" cy="40%" r="60%" fx="50%" fy="30%">
            <stop offset="0%" stop-color="#ffffff" stop-opacity="0.9" />
            <stop offset="40%" stop-color="#95a5a6" stop-opacity="0.5" />
            <stop offset="100%" stop-color="#2c3e50" stop-opacity="1" />
        </radialGradient>
    </defs>

    <rect width="600" height="400" fill="url(#spotlight)" />
</svg>
```

### Pie Chart Segment with Depth

```html
<svg width="300" height="300">
    <defs>
        <radialGradient id="pieSegment" cx="50%" cy="50%" r="50%" fx="40%" fy="40%">
            <stop offset="0%" stop-color="#5dade2" />
            <stop offset="100%" stop-color="#2874a6" />
        </radialGradient>
    </defs>

    <path d="M 150,150 L 150,30 A 120,120 0 0,1 270,150 Z" fill="url(#pieSegment)" />
</svg>
```

### Glowing Circle Data Point

```html
<svg width="300" height="300">
    <defs>
        <radialGradient id="dataGlow" cx="50%" cy="50%" r="50%" fx="45%" fy="45%">
            <stop offset="0%" stop-color="#3498db" stop-opacity="1" />
            <stop offset="40%" stop-color="#3498db" stop-opacity="0.8" />
            <stop offset="70%" stop-color="#3498db" stop-opacity="0.3" />
            <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
        </radialGradient>
    </defs>

    <circle cx="150" cy="150" r="80" fill="url(#dataGlow)" />
    <circle cx="150" cy="150" r="30" fill="#3498db" />
</svg>
```

### Progress Circle with Shine

```html
<svg width="200" height="200">
    <defs>
        <radialGradient id="progressShine" cx="50%" cy="50%" r="50%" fx="40%" fy="35%">
            <stop offset="0%" stop-color="#ecf0f1" />
            <stop offset="100%" stop-color="#bdc3c7" />
        </radialGradient>
    </defs>

    <circle cx="100" cy="100" r="80" fill="url(#progressShine)" />
    <circle cx="100" cy="100" r="60" fill="white" />
</svg>
```

### Radar Chart Center Point

```html
<svg width="400" height="400">
    <defs>
        <radialGradient id="radarCenter" cx="50%" cy="50%" r="50%" fx="50%" fy="50%">
            <stop offset="0%" stop-color="#2ecc71" stop-opacity="0.5" />
            <stop offset="50%" stop-color="#3498db" stop-opacity="0.2" />
            <stop offset="100%" stop-color="#9b59b6" stop-opacity="0.1" />
        </radialGradient>
    </defs>

    <circle cx="200" cy="200" r="180" fill="url(#radarCenter)" />
</svg>
```

### Data-Driven Light Position

```html
<!-- Model: { lightX: 35, lightY: 40, brightness: 1.0 } -->
<svg width="250" height="250">
    <defs>
        <radialGradient id="dataLight"
                        cx="50%" cy="50%" r="50%"
                        fx="{{model.lightX}}%" fy="{{model.lightY}}%">
            <stop offset="0%" stop-color="#ffffff" stop-opacity="{{model.brightness}}" />
            <stop offset="50%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </radialGradient>
    </defs>

    <circle cx="125" cy="125" r="100" fill="url(#dataLight)" />
</svg>
```

### Interactive Button State

```html
<!-- Model: { isHovered: true, isPressed: false } -->
<svg width="200" height="80">
    <defs>
        <radialGradient id="buttonState"
                        cx="50%" cy="50%" r="50%"
                        fx="{{model.isPressed ? '50%' : model.isHovered ? '35%' : '40%'}}"
                        fy="{{model.isPressed ? '50%' : model.isHovered ? '30%' : '35%'}}">
            <stop offset="0%" stop-color="#5dade2" />
            <stop offset="50%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2874a6" />
        </radialGradient>
    </defs>

    <rect width="200" height="80" rx="40" fill="url(#buttonState)" />
    <text x="100" y="48" text-anchor="middle" fill="white" font-size="20" font-weight="bold">
        Submit
    </text>
</svg>
```

---

## See Also

- [fy](/reference/svgattributes/fy.html) - Radial gradient focal point Y coordinate
- [cx](/reference/svgattributes/cx.html) - Radial gradient center X coordinate
- [cy](/reference/svgattributes/cy.html) - Radial gradient center Y coordinate
- [r](/reference/svgattributes/r.html) - Radial gradient radius
- [fr](/reference/svgattributes/fr.html) - Radial gradient focal radius
- [radialGradient](/reference/svgtags/radialGradient.html) - Radial gradient definition element
- [gradientUnits](/reference/svgattributes/gradientUnits.html) - Coordinate system mode
- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [Data Binding](/reference/binding/) - Data binding and expressions

---
