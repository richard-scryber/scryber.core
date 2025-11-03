---
layout: default
title: transform-origin
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @transform-origin : The Transform Origin Point Attribute

The `transform-origin` attribute specifies the origin point for transformations applied to an SVG element. It determines the fixed point around which rotations, scaling, and other transformations are performed, providing precise control over transformation behavior without modifying coordinates.

## Usage

The `transform-origin` attribute controls the transformation reference point:
- Set the origin point for rotations (rotate around specific point)
- Control scaling center point (scale from center, corner, or custom point)
- Define skew transformation anchor point
- Use coordinates (x, y) or keywords (top, center, bottom, left, right)
- Support data binding for dynamic origin points
- Simplify complex transformations without coordinate calculations
- Works with all transform functions (translate, rotate, scale, skew)

```html
<!-- Rotate around element center -->
<rect x="50" y="50" width="100" height="100"
      transform="rotate(45)"
      transform-origin="100 100"
      fill="blue"/>

<!-- Scale from center using keywords -->
<circle cx="150" cy="150" r="50"
        transform="scale(1.5)"
        transform-origin="center center"
        fill="red"/>

<!-- Dynamic origin point -->
<rect x="0" y="0" width="60" height="60"
      transform="rotate({{model.angle}})"
      transform-origin="{{model.originX}} {{model.originY}}"
      fill="green"/>
```

---

## Supported Values

The `transform-origin` attribute accepts coordinates or keywords:

### Coordinate Values

Specify exact coordinates for the origin point:

```
transform-origin="x y"
```

| Format | Example | Description |
|--------|---------|-------------|
| `x y` | `100 150` | Absolute coordinates in user units |
| `x y` | `50pt 75pt` | Coordinates with units |
| `x y` | `50% 50%` | Percentage of element bounding box |

### Keyword Values

Use keywords for common origin points:

#### X-axis Keywords
| Keyword | Position | Equivalent |
|---------|----------|------------|
| `left` | Left edge | `0%` |
| `center` | Horizontal center | `50%` |
| `right` | Right edge | `100%` |

#### Y-axis Keywords
| Keyword | Position | Equivalent |
|---------|----------|------------|
| `top` | Top edge | `0%` |
| `center` | Vertical center | `50%` |
| `bottom` | Bottom edge | `100%` |

### Common Combinations

```html
<!-- Center (default-like behavior) -->
transform-origin="center center"
transform-origin="50% 50%"

<!-- Top-left corner -->
transform-origin="left top"
transform-origin="0% 0%"

<!-- Top-right corner -->
transform-origin="right top"
transform-origin="100% 0%"

<!-- Bottom-center -->
transform-origin="center bottom"
transform-origin="50% 100%"

<!-- Specific coordinates -->
transform-origin="100 150"
transform-origin="50pt 75pt"
```

### Default Value

If omitted, transformations use the origin `(0, 0)` of the SVG coordinate system, **not** the element's center.

---

## Supported Elements

The `transform-origin` attribute is supported on elements that accept `transform`:

### Container Elements
- `<g>` - Group
- `<svg>` - Nested SVG canvas
- `<defs>` - Definitions container
- `<symbol>` - Symbol definition

### Shape Elements
- `<rect>` - Rectangle
- `<circle>` - Circle
- `<ellipse>` - Ellipse
- `<line>` - Line
- `<polyline>` - Polyline
- `<polygon>` - Polygon
- `<path>` - Path

### Text Elements
- `<text>` - Text
- `<tspan>` - Text span

### Other Elements
- `<image>` - Embedded image
- `<use>` - Referenced element
- `<a>` - Link wrapper

---

## Data Binding

The `transform-origin` attribute supports data binding for dynamic origin control:

### Dynamic Origin Coordinates

```html
<!-- Model: { originX: 100, originY: 100 } -->
<rect x="50" y="50" width="100" height="100"
      transform="rotate(45)"
      transform-origin="{{model.originX}} {{model.originY}}"
      fill="blue"/>
```

### Dynamic Keyword-Based Origin

```html
<!-- Model: { horizontalAlign: "center", verticalAlign: "center" } -->
<rect x="0" y="0" width="80" height="80"
      transform="scale(1.5)"
      transform-origin="{{model.horizontalAlign}} {{model.verticalAlign}}"
      fill="red"/>
```

### Calculated Origin Point

```html
<!-- Model: { elementX: 50, elementY: 50, elementWidth: 100, elementHeight: 100 } -->
<rect x="{{model.elementX}}" y="{{model.elementY}}"
      width="{{model.elementWidth}}" height="{{model.elementHeight}}"
      transform="rotate(30)"
      transform-origin="{{model.elementX + model.elementWidth / 2}} {{model.elementY + model.elementHeight / 2}}"
      fill="green"/>
```

### Interactive Rotation Point

```html
<!-- Model: { rotationAngle: 45, pivotX: 150, pivotY: 100 } -->
<g transform="rotate({{model.rotationAngle}})" transform-origin="{{model.pivotX}} {{model.pivotY}}">
    <rect x="100" y="50" width="100" height="100" fill="purple"/>
    <circle cx="{{model.pivotX}}" cy="{{model.pivotY}}" r="5" fill="red"/>
</g>
```

### Data-Driven Transformations

```html
<!-- Model: { items: [{angle:0, cx:100, cy:100}, {angle:45, cx:200, cy:100}] } -->
<template data-bind="{{model.items}}">
    <rect x="{{.cx - 25}}" y="{{.cy - 25}}" width="50" height="50"
          transform="rotate({{.angle}})"
          transform-origin="{{.cx}} {{.cy}}"
          fill="orange"/>
</template>
```

### Scaling from Different Origins

```html
<!-- Model: { scale: 1.5, scaleOrigin: "center center" } -->
<circle cx="150" cy="150" r="40"
        transform="scale({{model.scale}})"
        transform-origin="{{model.scaleOrigin}}"
        fill="teal"/>
```

---

## Notes

### How Transform Origin Works

The `transform-origin` defines a fixed point that remains stationary during transformations:

**Without transform-origin (default):**
```html
<!-- Rotates around SVG origin (0,0) -->
<rect x="100" y="100" width="50" height="50" transform="rotate(45)" fill="blue"/>
```

**With transform-origin:**
```html
<!-- Rotates around its center (125,125) -->
<rect x="100" y="100" width="50" height="50"
      transform="rotate(45)"
      transform-origin="125 125"
      fill="blue"/>
```

### Rotation Behavior

**Default (origin at 0,0):**
- Element rotates around top-left corner of SVG
- Circular motion around coordinate system origin
- Often not desired behavior

**With transform-origin at center:**
- Element rotates in place
- More intuitive behavior
- Easier to predict visual result

```html
<!-- Rotates around (0,0) - moves away -->
<rect x="100" y="100" width="60" height="60" transform="rotate(45)" fill="blue"/>

<!-- Rotates around its center - spins in place -->
<rect x="100" y="100" width="60" height="60"
      transform="rotate(45)"
      transform-origin="130 130"
      fill="red"/>
```

### Scaling Behavior

The origin point determines which part of the element stays fixed during scaling:

```html
<!-- Scales from top-left corner -->
<rect x="100" y="100" width="60" height="60"
      transform="scale(1.5)"
      transform-origin="100 100"
      fill="blue"/>

<!-- Scales from center (grows evenly in all directions) -->
<rect x="100" y="100" width="60" height="60"
      transform="scale(1.5)"
      transform-origin="130 130"
      fill="red"/>

<!-- Scales from bottom-right corner -->
<rect x="100" y="100" width="60" height="60"
      transform="scale(1.5)"
      transform-origin="160 160"
      fill="green"/>
```

### Percentage Values

Percentages are relative to the element's bounding box:

```html
<!-- 50% 50% = center of element's bounding box -->
<rect x="100" y="100" width="80" height="60"
      transform="rotate(30)"
      transform-origin="50% 50%"
      fill="blue"/>
```

**Bounding box reference:**
- `0% 0%` = top-left corner of element
- `50% 50%` = center of element
- `100% 100%` = bottom-right corner of element

### Units

The `transform-origin` accepts:
- **Unitless values**: Treated as user units in SVG coordinate system
- **Units**: `pt`, `px`, `%`, `em`, etc.
- **Keywords**: `left`, `center`, `right`, `top`, `bottom`

```html
<rect transform="rotate(45)" transform-origin="100 100"/>     <!-- User units -->
<rect transform="rotate(45)" transform-origin="100pt 100pt"/> <!-- Points -->
<rect transform="rotate(45)" transform-origin="50% 50%"/>     <!-- Percentage -->
<rect transform="rotate(45)" transform-origin="center center"/><!-- Keywords -->
```

### Coordinate System

The origin coordinates are specified in the **current user coordinate system**, not the element's local coordinate system:

```html
<!-- Origin at absolute position 150,150 in SVG coordinate space -->
<rect x="100" y="100" width="100" height="100"
      transform="rotate(45)"
      transform-origin="150 150"
      fill="blue"/>
```

### Multiple Transformations

When multiple transformations are applied, they all use the same origin point:

```html
<!-- All transformations use the same origin -->
<rect x="100" y="100" width="60" height="60"
      transform="rotate(45) scale(1.5)"
      transform-origin="130 130"
      fill="purple"/>
```

### Transform vs Transform-Origin

**Alternative to transform-origin:** Use rotation center parameter:

```html
<!-- Using transform-origin -->
<rect x="100" y="100" width="60" height="60"
      transform="rotate(45)"
      transform-origin="130 130"/>

<!-- Equivalent using rotate center parameters -->
<rect x="100" y="100" width="60" height="60"
      transform="rotate(45, 130, 130)"/>
```

**Note:** `transform-origin` applies to all transformations, while rotate center parameter only affects rotation.

### Common Patterns

**Pattern 1: Rotate Around Element Center**
```html
<!-- For rect at (x, y) with width w and height h -->
<!-- Origin at (x + w/2, y + h/2) -->
<rect x="100" y="50" width="80" height="60"
      transform="rotate(30)"
      transform-origin="140 80"/>
```

**Pattern 2: Scale from Center**
```html
<circle cx="150" cy="150" r="40"
        transform="scale(1.5)"
        transform-origin="150 150"/>
```

**Pattern 3: Rotate Group Around Point**
```html
<g transform="rotate(45)" transform-origin="200 200">
    <!-- Multiple elements rotate together around (200,200) -->
</g>
```

### Browser Compatibility Note

The `transform-origin` attribute is part of the SVG specification. In Scryber, it provides consistent behavior for PDF rendering. CSS `transform-origin` uses different default values (50% 50%) compared to SVG (0 0).

---

## Examples

### Basic Rotation Around Center

```html
<svg width="300pt" height="300pt">
    <!-- Without transform-origin: rotates around (0,0) -->
    <rect x="50" y="50" width="60" height="60" fill="blue" opacity="0.3"/>
    <rect x="50" y="50" width="60" height="60" fill="blue" transform="rotate(45)"/>

    <!-- With transform-origin: rotates around its center -->
    <rect x="150" y="50" width="60" height="60" fill="red" opacity="0.3"/>
    <rect x="150" y="50" width="60" height="60" fill="red"
          transform="rotate(45)" transform-origin="180 80"/>
</svg>
```

### Scaling from Different Origins

```html
<svg width="400pt" height="200pt">
    <!-- Scale from top-left -->
    <rect x="50" y="50" width="40" height="40" fill="blue" opacity="0.3"/>
    <rect x="50" y="50" width="40" height="40" fill="blue"
          transform="scale(1.5)" transform-origin="50 50"/>

    <!-- Scale from center -->
    <rect x="150" y="50" width="40" height="40" fill="green" opacity="0.3"/>
    <rect x="150" y="50" width="40" height="40" fill="green"
          transform="scale(1.5)" transform-origin="170 70"/>

    <!-- Scale from bottom-right -->
    <rect x="250" y="50" width="40" height="40" fill="red" opacity="0.3"/>
    <rect x="250" y="50" width="40" height="40" fill="red"
          transform="scale(1.5)" transform-origin="290 90"/>
</svg>
```

### Clock Hand Rotation

```html
<svg width="300pt" height="300pt">
    <!-- Clock face -->
    <circle cx="150" cy="150" r="120" fill="white" stroke="black" stroke-width="4"/>

    <!-- Center point (rotation origin) -->
    <circle cx="150" cy="150" r="8" fill="black"/>

    <!-- Hour hand (rotates around center) -->
    <rect x="145" y="80" width="10" height="70" fill="black"
          transform="rotate(90)" transform-origin="150 150"/>

    <!-- Minute hand (rotates around center) -->
    <rect x="147" y="50" width="6" height="100" fill="gray"
          transform="rotate(180)" transform-origin="150 150"/>
</svg>
```

### Spinning Wheel

```html
<svg width="300pt" height="300pt">
    <g transform="rotate(30)" transform-origin="150 150">
        <!-- Center hub -->
        <circle cx="150" cy="150" r="30" fill="#336699"/>

        <!-- Spokes (all rotate around center) -->
        <rect x="147" y="60" width="6" height="60" fill="#4a90e2"/>
        <rect x="147" y="60" width="6" height="60" fill="#4a90e2" transform="rotate(45)" transform-origin="150 150"/>
        <rect x="147" y="60" width="6" height="60" fill="#4a90e2" transform="rotate(90)" transform-origin="150 150"/>
        <rect x="147" y="60" width="6" height="60" fill="#4a90e2" transform="rotate(135)" transform-origin="150 150"/>
        <rect x="147" y="60" width="6" height="60" fill="#4a90e2" transform="rotate(180)" transform-origin="150 150"/>
        <rect x="147" y="60" width="6" height="60" fill="#4a90e2" transform="rotate(225)" transform-origin="150 150"/>
        <rect x="147" y="60" width="6" height="60" fill="#4a90e2" transform="rotate(270)" transform-origin="150 150"/>
        <rect x="147" y="60" width="6" height="60" fill="#4a90e2" transform="rotate(315)" transform-origin="150 150"/>
    </g>
</svg>
```

### Door Opening (Rotation on Edge)

```html
<svg width="300pt" height="300pt">
    <!-- Door frame -->
    <rect x="80" y="100" width="120" height="150" fill="none" stroke="#666" stroke-width="4"/>

    <!-- Door (rotates on left edge) -->
    <rect x="85" y="105" width="55" height="140" fill="#8b4513" stroke="#654321" stroke-width="2"
          transform="rotate(-45)" transform-origin="85 175"/>

    <!-- Door handle -->
    <circle cx="130" cy="175" r="5" fill="gold"
            transform="rotate(-45)" transform-origin="85 175"/>
</svg>
```

### Pendulum Swing

```html
<svg width="300pt" height="400pt">
    <!-- Pivot point -->
    <circle cx="150" cy="50" r="5" fill="black"/>

    <!-- Pendulum rod -->
    <line x1="150" y1="50" x2="150" y2="200" stroke="gray" stroke-width="3"
          transform="rotate(30)" transform-origin="150 50"/>

    <!-- Pendulum weight -->
    <circle cx="150" cy="200" r="20" fill="#336699"
            transform="rotate(30)" transform-origin="150 50"/>
</svg>
```

### Gauge Needle

```html
<svg width="300pt" height="200pt">
    <!-- Gauge arc -->
    <path d="M 50,180 A 100,100 0 0,1 250,180" fill="none" stroke="#e0e0e0" stroke-width="20" stroke-linecap="round"/>

    <!-- Gauge center -->
    <circle cx="150" cy="180" r="10" fill="#333"/>

    <!-- Needle (rotates around center point) -->
    <line x1="150" y1="180" x2="150" y2="90" stroke="red" stroke-width="4"
          transform="rotate(-60)" transform-origin="150 180" stroke-linecap="round"/>
</svg>
```

### Compass Needle

```html
<svg width="300pt" height="300pt">
    <!-- Compass circle -->
    <circle cx="150" cy="150" r="120" fill="white" stroke="black" stroke-width="3"/>

    <!-- Cardinal directions -->
    <text x="150" y="45" text-anchor="middle" font-size="24" font-weight="bold">N</text>
    <text x="255" y="155" text-anchor="middle" font-size="24" font-weight="bold">E</text>
    <text x="150" y="265" text-anchor="middle" font-size="24" font-weight="bold">S</text>
    <text x="45" y="155" text-anchor="middle" font-size="24" font-weight="bold">W</text>

    <!-- Needle (rotates around center) -->
    <g transform="rotate(45)" transform-origin="150 150">
        <polygon points="150,70 155,150 150,160 145,150" fill="red"/>
        <polygon points="150,160 155,150 150,230 145,150" fill="white" stroke="black" stroke-width="1"/>
    </g>

    <!-- Center pin -->
    <circle cx="150" cy="150" r="8" fill="black"/>
</svg>
```

### Fan Blades

```html
<svg width="300pt" height="300pt">
    <g transform="rotate(15)" transform-origin="150 150">
        <!-- Blade 1 -->
        <ellipse cx="80" cy="150" rx="60" ry="20" fill="#4a90e2"/>
        <!-- Blade 2 -->
        <ellipse cx="80" cy="150" rx="60" ry="20" fill="#4a90e2" transform="rotate(120)" transform-origin="150 150"/>
        <!-- Blade 3 -->
        <ellipse cx="80" cy="150" rx="60" ry="20" fill="#4a90e2" transform="rotate(240)" transform-origin="150 150"/>

        <!-- Hub -->
        <circle cx="150" cy="150" r="20" fill="#336699"/>
    </g>
</svg>
```

### Windmill

```html
<svg width="300pt" height="400pt">
    <!-- Tower -->
    <polygon points="140,400 160,400 155,200 145,200" fill="#8b4513"/>

    <!-- Blades (rotate around center) -->
    <g transform="rotate(30)" transform-origin="150 200">
        <!-- Blade 1 -->
        <polygon points="150,140 165,200 150,205 135,200" fill="white" stroke="#666" stroke-width="2"/>
        <!-- Blade 2 -->
        <polygon points="150,140 165,200 150,205 135,200" fill="white" stroke="#666" stroke-width="2"
                 transform="rotate(90)" transform-origin="150 200"/>
        <!-- Blade 3 -->
        <polygon points="150,140 165,200 150,205 135,200" fill="white" stroke="#666" stroke-width="2"
                 transform="rotate(180)" transform-origin="150 200"/>
        <!-- Blade 4 -->
        <polygon points="150,140 165,200 150,205 135,200" fill="white" stroke="#666" stroke-width="2"
                 transform="rotate(270)" transform-origin="150 200"/>

        <!-- Center -->
        <circle cx="150" cy="200" r="12" fill="#333"/>
    </g>
</svg>
```

### Steering Wheel

```html
<svg width="300pt" height="300pt">
    <!-- Rotated wheel -->
    <g transform="rotate(-30)" transform-origin="150 150">
        <!-- Outer rim -->
        <circle cx="150" cy="150" r="100" fill="none" stroke="#333" stroke-width="15"/>

        <!-- Spokes -->
        <line x1="150" y1="70" x2="150" y2="230" stroke="#333" stroke-width="8"/>
        <line x1="150" y1="70" x2="150" y2="230" stroke="#333" stroke-width="8" transform="rotate(60)" transform-origin="150 150"/>
        <line x1="150" y1="70" x2="150" y2="230" stroke="#333" stroke-width="8" transform="rotate(120)" transform-origin="150 150"/>

        <!-- Center hub -->
        <circle cx="150" cy="150" r="25" fill="#666"/>
        <circle cx="150" cy="150" r="15" fill="#999"/>
    </g>
</svg>
```

### Loading Spinner

```html
<svg width="200pt" height="200pt">
    <g transform="rotate(45)" transform-origin="100 100">
        <!-- 8 dots in circle -->
        <circle cx="100" cy="40" r="8" fill="#4a90e2" opacity="1.0"/>
        <circle cx="100" cy="40" r="8" fill="#4a90e2" opacity="0.9" transform="rotate(45)" transform-origin="100 100"/>
        <circle cx="100" cy="40" r="8" fill="#4a90e2" opacity="0.8" transform="rotate(90)" transform-origin="100 100"/>
        <circle cx="100" cy="40" r="8" fill="#4a90e2" opacity="0.7" transform="rotate(135)" transform-origin="100 100"/>
        <circle cx="100" cy="40" r="8" fill="#4a90e2" opacity="0.5" transform="rotate(180)" transform-origin="100 100"/>
        <circle cx="100" cy="40" r="8" fill="#4a90e2" opacity="0.3" transform="rotate(225)" transform-origin="100 100"/>
        <circle cx="100" cy="40" r="8" fill="#4a90e2" opacity="0.2" transform="rotate(270)" transform-origin="100 100"/>
        <circle cx="100" cy="40" r="8" fill="#4a90e2" opacity="0.1" transform="rotate(315)" transform-origin="100 100"/>
    </g>
</svg>
```

### Gear Rotation

```html
<svg width="300pt" height="300pt">
    <!-- Large gear -->
    <g transform="rotate(15)" transform-origin="150 150">
        <circle cx="150" cy="150" r="60" fill="#336699"/>
        <!-- Teeth -->
        <rect x="145" y="85" width="10" height="15" fill="#336699"/>
        <rect x="145" y="85" width="10" height="15" fill="#336699" transform="rotate(40)" transform-origin="150 150"/>
        <rect x="145" y="85" width="10" height="15" fill="#336699" transform="rotate(80)" transform-origin="150 150"/>
        <rect x="145" y="85" width="10" height="15" fill="#336699" transform="rotate(120)" transform-origin="150 150"/>
        <rect x="145" y="85" width="10" height="15" fill="#336699" transform="rotate(160)" transform-origin="150 150"/>
        <rect x="145" y="85" width="10" height="15" fill="#336699" transform="rotate(200)" transform-origin="150 150"/>
        <rect x="145" y="85" width="10" height="15" fill="#336699" transform="rotate(240)" transform-origin="150 150"/>
        <rect x="145" y="85" width="10" height="15" fill="#336699" transform="rotate(280)" transform-origin="150 150"/>
        <rect x="145" y="85" width="10" height="15" fill="#336699" transform="rotate(320)" transform-origin="150 150"/>
        <circle cx="150" cy="150" r="20" fill="white"/>
    </g>
</svg>
```

### Propeller

```html
<svg width="300pt" height="300pt">
    <g transform="rotate(60)" transform-origin="150 150">
        <!-- Blade 1 -->
        <ellipse cx="150" cy="100" rx="15" ry="50" fill="#666" opacity="0.7"/>
        <!-- Blade 2 -->
        <ellipse cx="150" cy="100" rx="15" ry="50" fill="#666" opacity="0.7" transform="rotate(120)" transform-origin="150 150"/>
        <!-- Blade 3 -->
        <ellipse cx="150" cy="100" rx="15" ry="50" fill="#666" opacity="0.7" transform="rotate(240)" transform-origin="150 150"/>

        <!-- Center -->
        <circle cx="150" cy="150" r="12" fill="#333"/>
    </g>
</svg>
```

### Pivot Point Demonstration

```html
<svg width="500pt" height="300pt">
    <!-- Original position -->
    <rect x="50" y="100" width="80" height="80" fill="blue" opacity="0.2"/>

    <!-- Rotate around top-left corner -->
    <g transform="translate(0, 0)">
        <circle cx="50" cy="100" r="4" fill="red"/>
        <rect x="50" y="100" width="80" height="80" fill="blue" opacity="0.3"
              transform="rotate(30)" transform-origin="50 100"/>
    </g>

    <!-- Rotate around center -->
    <g transform="translate(150, 0)">
        <circle cx="90" cy="140" r="4" fill="red"/>
        <rect x="50" y="100" width="80" height="80" fill="green" opacity="0.3"
              transform="rotate(30)" transform-origin="90 140"/>
    </g>

    <!-- Rotate around bottom-right -->
    <g transform="translate(300, 0)">
        <circle cx="130" cy="180" r="4" fill="red"/>
        <rect x="50" y="100" width="80" height="80" fill="orange" opacity="0.3"
              transform="rotate(30)" transform-origin="130 180"/>
    </g>
</svg>
```

### Dynamic Rotation Point

```html
<!-- Model: { angle: 45, pivotX: 150, pivotY: 150 } -->
<svg width="300pt" height="300pt">
    <!-- Pivot point indicator -->
    <circle cx="{{model.pivotX}}" cy="{{model.pivotY}}" r="5" fill="red"/>
    <text x="{{model.pivotX}}" y="{{model.pivotY - 15}}" text-anchor="middle" font-size="12" fill="red">
        Pivot
    </text>

    <!-- Rotating element -->
    <rect x="100" y="100" width="100" height="100" fill="blue" opacity="0.3"/>
    <rect x="100" y="100" width="100" height="100" fill="blue"
          transform="rotate({{model.angle}})"
          transform-origin="{{model.pivotX}} {{model.pivotY}}"/>
</svg>
```

### Scale Animation Origin

```html
<!-- Model: { scale: 1.5, scaleMode: "center" } -->
<!-- scaleMode: "topleft", "center", "bottomright" -->
<svg width="300pt" height="300pt">
    <rect x="100" y="100" width="100" height="100" fill="purple" opacity="0.2"/>
    <rect x="100" y="100" width="100" height="100" fill="purple"
          transform="scale({{model.scale}})"
          transform-origin="{{model.scaleMode == 'topleft' ? '100 100' : model.scaleMode == 'center' ? '150 150' : '200 200'}}"/>
</svg>
```

### Keywords vs Coordinates

```html
<svg width="500pt" height="200pt">
    <!-- Using keywords -->
    <rect x="50" y="50" width="80" height="80" fill="blue" opacity="0.3"/>
    <rect x="50" y="50" width="80" height="80" fill="blue"
          transform="rotate(30)" transform-origin="center center"/>

    <!-- Using percentage -->
    <rect x="200" y="50" width="80" height="80" fill="green" opacity="0.3"/>
    <rect x="200" y="50" width="80" height="80" fill="green"
          transform="rotate(30)" transform-origin="50% 50%"/>

    <!-- Using coordinates -->
    <rect x="350" y="50" width="80" height="80" fill="red" opacity="0.3"/>
    <rect x="350" y="50" width="80" height="80" fill="red"
          transform="rotate(30)" transform-origin="390 90"/>
</svg>
```

### Text Rotation

```html
<svg width="400pt" height="300pt">
    <!-- Rotate text around its center -->
    <text x="200" y="150" font-size="36" font-weight="bold" fill="blue" opacity="0.3"
          text-anchor="middle">ROTATED</text>

    <text x="200" y="150" font-size="36" font-weight="bold" fill="blue"
          text-anchor="middle"
          transform="rotate(-15)"
          transform-origin="200 150">ROTATED</text>
</svg>
```

### Complex Multi-Origin Transform

```html
<svg width="400pt" height="400pt">
    <!-- Outer rotation around center -->
    <g transform="rotate(30)" transform-origin="200 200">
        <!-- Element at 12 o'clock position -->
        <g transform="translate(200, 100)">
            <!-- Element rotates around its own center -->
            <rect x="-20" y="-20" width="40" height="40" fill="blue"
                  transform="rotate(45)" transform-origin="0 0"/>
        </g>

        <!-- Element at 3 o'clock position -->
        <g transform="translate(300, 200)">
            <rect x="-20" y="-20" width="40" height="40" fill="red"
                  transform="rotate(45)" transform-origin="0 0"/>
        </g>

        <!-- Element at 6 o'clock position -->
        <g transform="translate(200, 300)">
            <rect x="-20" y="-20" width="40" height="40" fill="green"
                  transform="rotate(45)" transform-origin="0 0"/>
        </g>

        <!-- Element at 9 o'clock position -->
        <g transform="translate(100, 200)">
            <rect x="-20" y="-20" width="40" height="40" fill="orange"
                  transform="rotate(45)" transform-origin="0 0"/>
        </g>
    </g>

    <!-- Center point -->
    <circle cx="200" cy="200" r="8" fill="black"/>
</svg>
```

---

## See Also

- [transform](/reference/svgattributes/transform.html) - Transform operations
- [g](/reference/svgtags/g.html) - Group element
- [svg](/reference/svgtags/svg.html) - SVG canvas element
- [Data Binding](/reference/binding/) - Dynamic data binding
- [SVG Shapes](/reference/svgtags/shapes.html) - rect, circle, ellipse, line, etc.
- [SVG Transformations Guide](https://www.w3.org/TR/SVG2/coords.html#TransformAttribute) - W3C specification

---
