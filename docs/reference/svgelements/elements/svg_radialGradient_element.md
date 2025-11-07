---
layout: default
title: radialGradient
parent: SVG Elements
parent_url: /reference/svgtags/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# &lt;radialGradient&gt; : The Radial Gradient Definition Element

The `<radialGradient>` element defines a radial gradient fill that transitions between colors radiating from a focal point outward in a circular pattern. It must be placed inside an SVG `<defs>` section or directly within an `<svg>` canvas, and is referenced by its `id` attribute from SVG shapes and elements using the `fill` or `stroke` attributes.

## Usage

The `<radialGradient>` element creates reusable gradient definitions that:
- Define smooth color transitions from a central focal point radiating outward
- Support multiple color stops at specified radii
- Allow positioning of both the center and focal point
- Support three spread methods: pad, repeat, and reflect
- Work with objectBoundingBox (relative %) or userSpaceOnUse (absolute units)
- Can be styled via CSS classes and inline styles
- Support data binding for dynamic gradient generation
- Are referenced using `url(#gradientId)` syntax
- Create circular and elliptical gradient effects

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="400">
    <defs>
        <radialGradient id="myRadialGrad" cx="50%" cy="50%" r="50%" fx="50%" fy="50%">
            <stop offset="0%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </radialGradient>
    </defs>
    <circle cx="200" cy="200" r="150" fill="url(#myRadialGrad)" />
</svg>
```

---

## Supported Attributes

### Identification Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `id` | string | **Required**. Unique identifier for the gradient. Used to reference the gradient from other elements. |
| `class` | string | CSS class name(s) for styling. Multiple classes separated by spaces. |
| `style` | string | Inline CSS styles applied directly to the gradient definition. |

### Gradient Circle Center Attributes

| Attribute | Type | Default | Description |
|-----------|------|---------|-------------|
| `cx` | Unit | 50% | X-coordinate of the gradient circle's center point. Supports units: pt, px, mm, cm, in, %. |
| `cy` | Unit | 50% | Y-coordinate of the gradient circle's center point. Supports units: pt, px, mm, cm, in, %. |
| `r` | Unit | 50% | Radius of the gradient circle (100% stop position). Supports units: pt, px, mm, cm, in, %. |

### Gradient Focal Point Attributes

| Attribute | Type | Default | Description |
|-----------|------|---------|-------------|
| `fx` | Unit | 50% (cx) | X-coordinate of the gradient's focal point (0% stop position). Supports units: pt, px, mm, cm, in, %. |
| `fy` | Unit | 50% (cy) | Y-coordinate of the gradient's focal point (0% stop position). Supports units: pt, px, mm, cm, in, %. |
| `fr` | Unit | 0% | Radius of the focal circle (inner starting radius). Supports units: pt, px, mm, cm, in, %. |

### Gradient Behavior Attributes

| Attribute | Type | Default | Description |
|-----------|------|---------|-------------|
| `gradientUnits` | GradientUnitType | objectBoundingBox | Coordinate system: `objectBoundingBox` (relative to shape, 0-1 or %) or `userSpaceOnUse` (absolute coordinates). |
| `spreadMethod` | GradientSpreadMode | pad | How gradient extends beyond defined range: `pad` (extend end colors), `repeat` (tile gradient), `reflect` (mirror gradient). |

### Reference Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `href` | string | Reference to another gradient to inherit attributes from. Format: `url(#othergradientId)`. |

### CSS Style Support

The `<radialGradient>` element can be styled through CSS:

**Gradient Properties** (via inline style or CSS):
- `cx`, `cy`, `r` - Gradient circle positioning and size
- `fx`, `fy`, `fr` - Focal point positioning and radius
- `spreadMethod` - Gradient spread behavior
- `gradientUnits` - Coordinate system type

---

## Notes

### Radial Gradient Geometry

Radial gradients define two circles:

1. **Focal Circle** (inner): Defined by (fx, fy, fr)
   - Starting point where 0% color stops appear
   - Default: center of gradient (fx=cx, fy=cy, fr=0)
   - Can be offset from center for lighting effects

2. **Outer Circle**: Defined by (cx, cy, r)
   - End point where 100% color stops appear
   - Gradient radiates from focal point to this circle
   - Defines the extent of the gradient

### Coordinate Systems

1. **objectBoundingBox (default)**: Coordinates relative to the filled shape
   - Values typically 0-1 or percentages (0%-100%)
   - (50%, 50%) = center of shape
   - Gradient scales automatically with shape size
   - Most common for responsive gradients

2. **userSpaceOnUse**: Coordinates in absolute document units
   - Values in pt, px, mm, cm, in
   - Gradient position is fixed in document space
   - Useful for consistent gradients across shapes

### Focal Point Effects

Moving the focal point creates different visual effects:

```html
<!-- Centered focal point (default) - even radial -->
<radialGradient id="centered" cx="50%" cy="50%" fx="50%" fy="50%">

<!-- Offset focal point - lighting from top-left -->
<radialGradient id="topLeft" cx="50%" cy="50%" fx="25%" fy="25%">

<!-- Offset focal point - lighting from bottom-right -->
<radialGradient id="bottomRight" cx="50%" cy="50%" fx="75%" fy="75%">
</radialGradient>
```

### Spread Methods

The `spreadMethod` attribute controls gradient behavior beyond the defined radius:

1. **pad (default)**: Extends the last stop color to fill remaining space
2. **repeat**: Tiles the gradient pattern concentrically
3. **reflect**: Mirrors the gradient alternately in concentric rings

### Gradient Stops

Radial gradients require child `<stop>` elements:
- Minimum 2 stops required for a gradient
- Stops define colors at specific radial distances (0%-100%)
- 0% = focal point (fx, fy, fr)
- 100% = outer circle (cx, cy, r)
- Intermediate stops create smooth transitions

### Class Hierarchy

In the Scryber codebase:
- `SVGRadialGradient` extends `SVGFillBase` extends `Component`
- Implements `IStyledComponent` for CSS styling
- Implements `ICloneable` for gradient duplication
- Uses specialized radial gradient calculators based on spread mode
- Supports focal circle with inner radius (fr attribute)

### Common Use Cases

Radial gradients are ideal for:
- Circular buttons and badges with depth
- Spotlight and lighting effects
- Glowing or luminous elements
- Spherical 3D effects
- Vignette backgrounds
- Radar and gauge charts
- Icons with dimensional appearance
- Sun/star burst effects

---

## Data Binding

### Dynamic Gradient Colors

Bind radial gradient colors to data values:

```html
<!-- Model: { centerColor: "#f39c12", edgeColor: "#e74c3c" } -->
<svg width="300" height="300">
    <defs>
        <radialGradient id="dataRadial" cx="50%" cy="50%" r="50%">
            <stop offset="0%" stop-color="{{model.centerColor}}" />
            <stop offset="100%" stop-color="{{model.edgeColor}}" />
        </radialGradient>
    </defs>
    <circle cx="150" cy="150" r="120" fill="url(#dataRadial)" />
</svg>
```

### Dynamic Focal Point Position

Create lighting effects with data-driven focal points:

```html
<!-- Model: { lightX: 30, lightY: 30 } -->
<svg width="300" height="300">
    <radialGradient id="lighting"
                    cx="50%" cy="50%" r="50%"
                    fx="{{model.lightX}}%" fy="{{model.lightY}}%">
        <stop offset="0%" stop-color="#ffffff" />
        <stop offset="100%" stop-color="#2c3e50" />
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#lighting)" />
</svg>
```

### Conditional Gradient Based on Status

Generate gradients based on data state:

```html
<!-- Model: { status: "active", temperature: 85 } -->
<svg width="200" height="200">
    <radialGradient id="statusRadial">
        <stop offset="0%" stop-color="white" />
        <stop offset="100%"
              stop-color="{{model.temperature > 80 ? '#e74c3c' :
                           model.temperature > 50 ? '#f39c12' : '#2ecc71'}}" />
    </radialGradient>
    <circle cx="100" cy="100" r="80" fill="url(#statusRadial)" />
</svg>
```

### Dynamic Gradient Radius

Control gradient extent with data:

```html
<!-- Model: { intensity: 75 } -->
<svg width="300" height="300">
    <radialGradient id="intensityGrad"
                    cx="50%" cy="50%"
                    r="{{model.intensity}}%">
        <stop offset="0%" stop-color="#3498db" />
        <stop offset="100%" stop-color="#2c3e50" />
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#intensityGrad)" />
</svg>
```

### Data-Driven Multi-Stop Gradients

Generate complex gradients from data arrays:

```html
<!-- Model: { stops: [{offset: 0, color: "#ffffff"}, {offset: 40, color: "#f39c12"}, {offset: 100, color: "#e74c3c"}] } -->
<svg width="300" height="300">
    <radialGradient id="multiRadial">
        <template data-bind="{{model.stops}}">
            <stop offset="{{.offset}}%" stop-color="{{.color}}" />
        </template>
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#multiRadial)" />
</svg>
```

---

## Examples

### Basic Radial Gradient

```html
<svg width="300" height="300">
    <defs>
        <radialGradient id="basic" cx="50%" cy="50%" r="50%">
            <stop offset="0%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </radialGradient>
    </defs>
    <circle cx="150" cy="150" r="120" fill="url(#basic)" />
</svg>
```

### Radial Gradient with Offset Focal Point

```html
<svg width="300" height="300">
    <radialGradient id="lighting" cx="50%" cy="50%" r="50%"
                    fx="30%" fy="30%">
        <stop offset="0%" stop-color="#ffffff" />
        <stop offset="50%" stop-color="#3498db" />
        <stop offset="100%" stop-color="#2c3e50" />
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#lighting)" />
</svg>
```

### Multi-Color Radial Gradient

```html
<svg width="300" height="300">
    <radialGradient id="sunset">
        <stop offset="0%" stop-color="#f1c40f" />
        <stop offset="40%" stop-color="#e67e22" />
        <stop offset="70%" stop-color="#e74c3c" />
        <stop offset="100%" stop-color="#c0392b" />
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#sunset)" />
</svg>
```

### Radial Gradient with Transparency

```html
<svg width="300" height="300">
    <radialGradient id="glow">
        <stop offset="0%" stop-color="#3498db" stop-opacity="1" />
        <stop offset="70%" stop-color="#3498db" stop-opacity="0.5" />
        <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#glow)" />
</svg>
```

### Spotlight Effect

```html
<svg width="400" height="300">
    <radialGradient id="spotlight" cx="50%" cy="40%" r="60%"
                    fx="50%" fy="40%">
        <stop offset="0%" stop-color="#ffffff" stop-opacity="0.9" />
        <stop offset="50%" stop-color="#95a5a6" stop-opacity="0.5" />
        <stop offset="100%" stop-color="#2c3e50" stop-opacity="1" />
    </radialGradient>
    <rect width="400" height="300" fill="url(#spotlight)" />
</svg>
```

### Vignette Effect

```html
<svg width="400" height="300">
    <radialGradient id="vignette" cx="50%" cy="50%" r="70%">
        <stop offset="0%" stop-color="#ecf0f1" stop-opacity="0" />
        <stop offset="70%" stop-color="#2c3e50" stop-opacity="0" />
        <stop offset="100%" stop-color="#2c3e50" stop-opacity="0.8" />
    </radialGradient>
    <rect width="400" height="300" fill="url(#vignette)" />
</svg>
```

### Button with 3D Radial Effect

```html
<svg width="200" height="200">
    <radialGradient id="button3d" cx="50%" cy="40%" r="50%"
                    fx="50%" fy="30%">
        <stop offset="0%" stop-color="#5dade2" />
        <stop offset="50%" stop-color="#3498db" />
        <stop offset="100%" stop-color="#2874a6" />
    </radialGradient>
    <circle cx="100" cy="100" r="80" fill="url(#button3d)" />
    <text x="100" y="110" text-anchor="middle" fill="white"
          font-size="20" font-weight="bold">Click</text>
</svg>
```

### Repeating Radial Gradient

```html
<svg width="400" height="400">
    <radialGradient id="rings" cx="50%" cy="50%" r="20%"
                    spreadMethod="repeat">
        <stop offset="0%" stop-color="#3498db" />
        <stop offset="50%" stop-color="#2980b9" />
        <stop offset="100%" stop-color="#3498db" />
    </radialGradient>
    <rect width="400" height="400" fill="url(#rings)" />
</svg>
```

### Reflecting Radial Gradient

```html
<svg width="400" height="400">
    <radialGradient id="ripples" cx="50%" cy="50%" r="25%"
                    spreadMethod="reflect">
        <stop offset="0%" stop-color="#16a085" />
        <stop offset="100%" stop-color="#1abc9c" />
    </radialGradient>
    <rect width="400" height="400" fill="url(#ripples)" />
</svg>
```

### Badge with Radial Gradient

```html
<svg width="100" height="100">
    <radialGradient id="badge" cx="50%" cy="40%" r="50%">
        <stop offset="0%" stop-color="#f1c40f" />
        <stop offset="100%" stop-color="#f39c12" />
    </radialGradient>
    <circle cx="50" cy="50" r="40" fill="url(#badge)" />
    <text x="50" y="58" text-anchor="middle" fill="white"
          font-size="24" font-weight="bold">5</text>
</svg>
```

### Gauge Indicator with Radial Background

```html
<svg width="300" height="300">
    <radialGradient id="gaugeBg">
        <stop offset="0%" stop-color="#ecf0f1" />
        <stop offset="100%" stop-color="#bdc3c7" />
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#gaugeBg)" />
    <circle cx="150" cy="150" r="100" fill="white" />
</svg>
```

### Status Indicator Light

```html
<svg width="80" height="80">
    <radialGradient id="greenLight" fx="40%" fy="40%">
        <stop offset="0%" stop-color="#a8e6cf" />
        <stop offset="50%" stop-color="#2ecc71" />
        <stop offset="100%" stop-color="#27ae60" />
    </radialGradient>
    <circle cx="40" cy="40" r="30" fill="url(#greenLight)" />
</svg>
```

### Sphere with Lighting

```html
<svg width="200" height="200">
    <radialGradient id="sphere" cx="50%" cy="50%" r="50%"
                    fx="35%" fy="35%">
        <stop offset="0%" stop-color="#ffffff" />
        <stop offset="40%" stop-color="#e74c3c" />
        <stop offset="80%" stop-color="#c0392b" />
        <stop offset="100%" stop-color="#7f2315" />
    </radialGradient>
    <circle cx="100" cy="100" r="80" fill="url(#sphere)" />
</svg>
```

### Pie Chart Segment with Radial Shading

```html
<svg width="300" height="300">
    <radialGradient id="pieSegment">
        <stop offset="0%" stop-color="#5dade2" />
        <stop offset="100%" stop-color="#2874a6" />
    </radialGradient>
    <path d="M 150,150 L 150,30 A 120,120 0 0,1 270,150 Z"
          fill="url(#pieSegment)" />
</svg>
```

### Radar Chart Background

```html
<svg width="400" height="400">
    <radialGradient id="radarBg" cx="50%" cy="50%" r="50%">
        <stop offset="0%" stop-color="#2ecc71" stop-opacity="0.2" />
        <stop offset="50%" stop-color="#3498db" stop-opacity="0.1" />
        <stop offset="100%" stop-color="#9b59b6" stop-opacity="0.05" />
    </radialGradient>
    <circle cx="200" cy="200" r="180" fill="url(#radarBg)" />
</svg>
```

### Icon with Radial Highlight

```html
<svg width="100" height="100" viewBox="0 0 24 24">
    <radialGradient id="iconGrad" fx="30%" fy="30%">
        <stop offset="0%" stop-color="#ffd93d" />
        <stop offset="100%" stop-color="#f39c12" />
    </radialGradient>
    <circle cx="12" cy="12" r="10" fill="url(#iconGrad)" />
</svg>
```

### Glowing Effect

```html
<svg width="300" height="300">
    <radialGradient id="glow">
        <stop offset="0%" stop-color="#3498db" stop-opacity="1" />
        <stop offset="40%" stop-color="#3498db" stop-opacity="0.8" />
        <stop offset="70%" stop-color="#3498db" stop-opacity="0.3" />
        <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
    </radialGradient>
    <circle cx="150" cy="150" r="120" fill="url(#glow)" />
    <circle cx="150" cy="150" r="60" fill="#3498db" />
</svg>
```

### Sun/Starburst Effect

```html
<svg width="300" height="300">
    <radialGradient id="sunburst">
        <stop offset="0%" stop-color="#ffffff" />
        <stop offset="20%" stop-color="#f1c40f" />
        <stop offset="60%" stop-color="#f39c12" />
        <stop offset="100%" stop-color="#e67e22" />
    </radialGradient>
    <circle cx="150" cy="150" r="100" fill="url(#sunburst)" />
</svg>
```

### Data-Driven Temperature Indicator

```html
<!-- Model: { temperature: 85, maxTemp: 100 } -->
<svg width="200" height="200">
    <radialGradient id="tempGrad">
        <stop offset="0%" stop-color="white" />
        <stop offset="{{model.temperature}}%"
              stop-color="{{model.temperature > 80 ? '#e74c3c' :
                           model.temperature > 50 ? '#f39c12' : '#3498db'}}" />
        <stop offset="100%" stop-color="#2c3e50" />
    </radialGradient>
    <circle cx="100" cy="100" r="80" fill="url(#tempGrad)" />
    <text x="100" y="105" text-anchor="middle" fill="white"
          font-size="24" font-weight="bold">{{model.temperature}}°</text>
</svg>
```

### Dynamic Spotlight Position

```html
<!-- Model: { spotX: 60, spotY: 40 } -->
<svg width="400" height="300">
    <radialGradient id="dynamicSpot"
                    cx="{{model.spotX}}%" cy="{{model.spotY}}%" r="50%"
                    fx="{{model.spotX}}%" fy="{{model.spotY}}%">
        <stop offset="0%" stop-color="#ffffff" />
        <stop offset="100%" stop-color="#2c3e50" />
    </radialGradient>
    <rect width="400" height="300" fill="url(#dynamicSpot)" />
</svg>
```

### Progress Circle with Radial Background

```html
<svg width="200" height="200">
    <radialGradient id="progressBg" fx="40%" fy="40%">
        <stop offset="0%" stop-color="#ecf0f1" />
        <stop offset="100%" stop-color="#bdc3c7" />
    </radialGradient>
    <circle cx="100" cy="100" r="80" fill="url(#progressBg)" />
    <circle cx="100" cy="100" r="60" fill="white" />
    <!-- Progress arc here -->
</svg>
```

---

## See Also

- [linearGradient](/reference/svgtags/linearGradient.html) - Linear gradient definition element
- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [svg](/reference/htmltags/svg.html) - SVG canvas element
- [circle](/reference/svgtags/circle.html) - Circle element (commonly used with radial gradients)
- [rect](/reference/svgtags/rect.html) - Rectangle element (supports gradient fills)
- [path](/reference/svgtags/path.html) - Path element (supports gradient fills and strokes)
- [CSS Gradients](/reference/styles/gradients.html) - CSS gradient properties
- [SVG Fills](/reference/svg/fills.html) - SVG fill patterns and gradients
- [Data Binding](/reference/binding/) - Data binding and expressions

---
