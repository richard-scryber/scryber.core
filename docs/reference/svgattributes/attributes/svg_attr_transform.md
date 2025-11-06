---
layout: default
title: transform
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @transform : The SVG Transform Attribute

The `transform` attribute applies geometric transformations to SVG elements, allowing you to translate (move), rotate, scale, skew, or apply matrix transformations. Multiple transformations can be combined to create complex effects, and they can be dynamically controlled through data binding.

## Usage

The `transform` attribute modifies the coordinate system for an element and its descendants:
- Translate elements to new positions without changing coordinates
- Rotate elements around a point
- Scale elements to different sizes
- Skew elements along axes
- Apply custom matrix transformations
- Combine multiple transformations in sequence
- Support data binding for dynamic transformations
- Apply to groups to transform multiple elements together

```html
<!-- Single transformation -->
<rect x="0" y="0" width="50" height="50" transform="translate(100,50)" fill="blue"/>

<!-- Multiple transformations -->
<rect x="0" y="0" width="50" height="50"
      transform="translate(100,50) rotate(45) scale(1.5)" fill="red"/>

<!-- Rotation around center -->
<circle cx="100" cy="100" r="30" transform="rotate(45, 100, 100)" fill="green"/>

<!-- Dynamic transformation -->
<g transform="translate({{model.x}}, {{model.y}}) rotate({{model.angle}})">
    <rect width="60" height="40" fill="purple"/>
</g>
```

---

## Supported Values

The `transform` attribute accepts one or more transformation functions separated by spaces:

### translate(x [,y])
Moves the element by x and y offset. If y is omitted, it defaults to 0.

```html
transform="translate(50, 100)"   <!-- Move 50 right, 100 down -->
transform="translate(50)"         <!-- Move 50 right, 0 down -->
```

### rotate(angle [,cx, cy])
Rotates the element by angle (in degrees). Optional cx, cy specify rotation center (default: 0,0).

```html
transform="rotate(45)"            <!-- Rotate 45° around origin -->
transform="rotate(45, 100, 100)"  <!-- Rotate 45° around point (100,100) -->
```

### scale(x [,y])
Scales the element. If y is omitted, uniform scaling is applied.

```html
transform="scale(2)"              <!-- Scale 2x uniformly -->
transform="scale(2, 0.5)"         <!-- Scale 2x horizontally, 0.5x vertically -->
```

### skewX(angle)
Skews the element along the X-axis by the specified angle (in degrees).

```html
transform="skewX(30)"             <!-- Skew 30° along X-axis -->
```

### skewY(angle)
Skews the element along the Y-axis by the specified angle (in degrees).

```html
transform="skewY(30)"             <!-- Skew 30° along Y-axis -->
```

### matrix(a, b, c, d, e, f)
Applies a custom transformation matrix. Provides complete control over transformations.

```html
transform="matrix(1, 0, 0, 1, 50, 100)"  <!-- Equivalent to translate(50,100) -->
```

### Combined Transformations
Multiple transformations are applied from left to right (order matters!).

```html
transform="translate(100,50) rotate(45) scale(1.5)"
```

---

## Supported Elements

The `transform` attribute is supported on most SVG elements:

### Container Elements
- `<g>` - Group (transforms all children)
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

The `transform` attribute supports comprehensive data binding for dynamic transformations:

### Dynamic Translation

```html
<!-- Model: { x: 100, y: 50 } -->
<rect width="50" height="50"
      transform="translate({{model.x}}, {{model.y}})"
      fill="blue"/>
```

### Dynamic Rotation

```html
<!-- Model: { angle: 45, centerX: 100, centerY: 100 } -->
<rect x="75" y="75" width="50" height="50"
      transform="rotate({{model.angle}}, {{model.centerX}}, {{model.centerY}})"
      fill="red"/>
```

### Dynamic Scaling

```html
<!-- Model: { scale: 1.5 } -->
<circle cx="100" cy="100" r="30"
        transform="scale({{model.scale}})"
        fill="green"/>
```

### Combined Dynamic Transformations

```html
<!-- Model: { x: 150, y: 100, rotation: 30, scale: 1.2 } -->
<g transform="translate({{model.x}}, {{model.y}}) rotate({{model.rotation}}) scale({{model.scale}})">
    <rect x="-25" y="-25" width="50" height="50" fill="purple"/>
</g>
```

### Data-Driven Repeated Transformations

```html
<!-- Model: { items: [{x:50, y:50, angle:0}, {x:150, y:50, angle:45}] } -->
<template data-bind="{{model.items}}">
    <rect width="40" height="40"
          transform="translate({{.x}}, {{.y}}) rotate({{.angle}})"
          fill="orange"/>
</template>
```

### Calculated Transformations

```html
<!-- Model: { index: 3, baseAngle: 30 } -->
<rect width="50" height="50"
      transform="translate({{100 + model.index * 60}}, 100) rotate({{model.baseAngle * model.index}})"
      fill="teal"/>
```

### Conditional Transformations

```html
<!-- Model: { isExpanded: true } -->
<rect width="50" height="50"
      transform="scale({{model.isExpanded ? 2 : 1}})"
      fill="blue"/>
```

---

## Notes

### Transformation Order

The order of transformations matters significantly. Transformations are applied from left to right:

```html
<!-- Translate then rotate: moves, then rotates around origin -->
<rect transform="translate(100,50) rotate(45)" width="50" height="50"/>

<!-- Rotate then translate: rotates around origin, then moves in rotated direction -->
<rect transform="rotate(45) translate(100,50)" width="50" height="50"/>
```

**Common Pattern**: For intuitive rotation around an element's center:
```html
<!-- 1. Translate to center position -->
<!-- 2. Rotate around origin (now at center) -->
<!-- 3. Translate back by half dimensions -->
<rect transform="translate(100,100) rotate(45) translate(-25,-25)"
      width="50" height="50"/>
```

### Coordinate System

- Transformations modify the coordinate system for the element and its children
- Child elements inherit the transformed coordinate system
- Nested transformations accumulate (multiply)

```html
<g transform="translate(100,100)">
    <g transform="rotate(45)">
        <!-- This rect is both translated AND rotated -->
        <rect x="0" y="0" width="50" height="50"/>
    </g>
</g>
```

### Transform Origin

By default, transformations use the origin (0,0) as the reference point. Use `transform-origin` attribute or include the center point in rotation:

```html
<!-- Rotate around element center using center point -->
<rect x="50" y="50" width="100" height="100"
      transform="rotate(45, 100, 100)"/>

<!-- Alternative: using transform-origin (see transform-origin attribute) -->
<rect x="50" y="50" width="100" height="100"
      transform="rotate(45)"
      transform-origin="100 100"/>
```

### Performance Considerations

- Simple transformations (translate, rotate, scale) are optimized
- Complex matrix transformations may be slower
- Transforming groups is more efficient than individual elements
- Cache styles when using repeated transformed elements

### Units

- Translate values can use units (pt, px, etc.) or be unitless (treated as user units)
- Rotation and skew angles are always in degrees
- Scale values are unitless multipliers (1.0 = 100%, 2.0 = 200%)

```html
<rect transform="translate(50pt, 100px)"/>  <!-- With units -->
<rect transform="translate(50, 100)"/>      <!-- Unitless (user units) -->
<rect transform="rotate(45deg)"/>           <!-- Degrees (deg optional) -->
<rect transform="scale(1.5)"/>              <!-- Multiplier -->
```

### Matrix Transformation

The matrix function provides low-level control:
```
matrix(a, b, c, d, e, f)
```

Represents the transformation matrix:
```
| a  c  e |
| b  d  f |
| 0  0  1 |
```

Common matrix equivalents:
```html
<!-- translate(tx, ty) = matrix(1, 0, 0, 1, tx, ty) -->
<rect transform="matrix(1, 0, 0, 1, 50, 100)"/>

<!-- scale(sx, sy) = matrix(sx, 0, 0, sy, 0, 0) -->
<rect transform="matrix(2, 0, 0, 2, 0, 0)"/>

<!-- rotate(θ) = matrix(cos(θ), sin(θ), -sin(θ), cos(θ), 0, 0) -->
<rect transform="matrix(0.707, 0.707, -0.707, 0.707, 0, 0)"/>  <!-- 45° -->
```

### Combining with CSS Transforms

Note that SVG transforms are different from CSS transforms:
- SVG: `transform` attribute with SVG syntax
- CSS: `transform` style property with CSS syntax

```html
<!-- SVG transform attribute -->
<rect transform="translate(50, 100) rotate(45)"/>

<!-- CSS transform style (different syntax) -->
<rect style="transform: translate(50px, 100px) rotate(45deg);"/>
```

---

## Examples

### Basic Translation

```html
<svg width="300pt" height="200pt">
    <!-- Original position -->
    <rect x="0" y="0" width="50" height="50" fill="lightblue" opacity="0.5"/>

    <!-- Translated -->
    <rect x="0" y="0" width="50" height="50" fill="blue"
          transform="translate(100, 50)"/>
</svg>
```

### Basic Rotation

```html
<svg width="300pt" height="200pt">
    <!-- Rotate around origin (top-left corner) -->
    <rect x="50" y="50" width="80" height="80" fill="red"
          transform="rotate(45)"/>

    <!-- Rotate around center point -->
    <rect x="50" y="50" width="80" height="80" fill="blue" opacity="0.7"
          transform="rotate(45, 90, 90)"/>
</svg>
```

### Basic Scaling

```html
<svg width="300pt" height="200pt">
    <rect x="50" y="50" width="40" height="40" fill="green" opacity="0.3"/>
    <rect x="50" y="50" width="40" height="40" fill="green"
          transform="scale(1.5)"/>
    <rect x="50" y="50" width="40" height="40" fill="green"
          transform="scale(2.5)"/>
</svg>
```

### Uniform vs Non-Uniform Scaling

```html
<svg width="400pt" height="200pt">
    <!-- Original -->
    <rect x="50" y="75" width="60" height="40" fill="purple" opacity="0.3"/>

    <!-- Uniform scaling -->
    <rect x="50" y="75" width="60" height="40" fill="purple"
          transform="translate(100, 0) scale(1.5)"/>

    <!-- Non-uniform scaling -->
    <rect x="50" y="75" width="60" height="40" fill="purple"
          transform="translate(250, 0) scale(2, 0.8)"/>
</svg>
```

### Skew Transformations

```html
<svg width="400pt" height="200pt">
    <!-- Original -->
    <rect x="50" y="50" width="60" height="60" fill="orange" opacity="0.3"/>

    <!-- Skew X -->
    <rect x="50" y="50" width="60" height="60" fill="orange"
          transform="translate(120, 0) skewX(30)"/>

    <!-- Skew Y -->
    <rect x="50" y="50" width="60" height="60" fill="orange"
          transform="translate(240, 0) skewY(30)"/>
</svg>
```

### Combined Transformations

```html
<svg width="300pt" height="300pt">
    <rect x="-25" y="-25" width="50" height="50" fill="teal"
          transform="translate(150, 150) rotate(45) scale(1.5)"/>
</svg>
```

### Rotation with Different Origins

```html
<svg width="400pt" height="200pt">
    <!-- Rotate around (0,0) -->
    <g>
        <circle cx="50" cy="100" r="3" fill="red"/>
        <rect x="50" y="80" width="60" height="40" fill="blue" opacity="0.5"
              transform="rotate(30)"/>
    </g>

    <!-- Rotate around (80,100) - rectangle center -->
    <g transform="translate(150, 0)">
        <circle cx="80" cy="100" r="3" fill="red"/>
        <rect x="50" y="80" width="60" height="40" fill="green" opacity="0.5"
              transform="rotate(30, 80, 100)"/>
    </g>
</svg>
```

### Transformation on Groups

```html
<svg width="300pt" height="300pt">
    <g transform="translate(150, 150) rotate(30)">
        <rect x="-40" y="-40" width="80" height="80" fill="lightblue" stroke="blue" stroke-width="2"/>
        <circle cx="0" cy="0" r="10" fill="red"/>
        <line x1="-50" y1="0" x2="50" y2="0" stroke="black" stroke-width="2"/>
        <line x1="0" y1="-50" x2="0" y2="50" stroke="black" stroke-width="2"/>
    </g>
</svg>
```

### Nested Transformations

```html
<svg width="300pt" height="300pt">
    <g transform="translate(150, 150)">
        <rect x="-60" y="-60" width="120" height="120" fill="lightgray" opacity="0.3"/>
        <g transform="rotate(45)">
            <rect x="-40" y="-40" width="80" height="80" fill="blue" opacity="0.5"/>
            <g transform="scale(0.5)">
                <rect x="-20" y="-20" width="40" height="40" fill="red"/>
            </g>
        </g>
    </g>
</svg>
```

### Clock Hands

```html
<svg width="300pt" height="300pt">
    <!-- Clock face -->
    <circle cx="150" cy="150" r="120" fill="white" stroke="black" stroke-width="4"/>

    <!-- Hour markers -->
    <g transform="translate(150, 150)">
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(0)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(30)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(60)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(90)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(120)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(150)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(180)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(210)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(240)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(270)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(300)"/>
        <line x1="0" y1="-110" x2="0" y2="-100" stroke="black" stroke-width="3" transform="rotate(330)"/>
    </g>

    <!-- Hour hand (10:00) -->
    <line x1="150" y1="150" x2="150" y2="90" stroke="black" stroke-width="6"
          transform="rotate(-60, 150, 150)" stroke-linecap="round"/>

    <!-- Minute hand (10:00) -->
    <line x1="150" y1="150" x2="150" y2="60" stroke="black" stroke-width="4"
          transform="rotate(0, 150, 150)" stroke-linecap="round"/>

    <!-- Center dot -->
    <circle cx="150" cy="150" r="8" fill="black"/>
</svg>
```

### Rotating Gear

```html
<svg width="200pt" height="200pt">
    <g transform="translate(100, 100) rotate(15)">
        <!-- Gear center -->
        <circle r="30" fill="#336699"/>

        <!-- Gear teeth (8 teeth) -->
        <rect x="-5" y="-50" width="10" height="20" fill="#336699" transform="rotate(0)"/>
        <rect x="-5" y="-50" width="10" height="20" fill="#336699" transform="rotate(45)"/>
        <rect x="-5" y="-50" width="10" height="20" fill="#336699" transform="rotate(90)"/>
        <rect x="-5" y="-50" width="10" height="20" fill="#336699" transform="rotate(135)"/>
        <rect x="-5" y="-50" width="10" height="20" fill="#336699" transform="rotate(180)"/>
        <rect x="-5" y="-50" width="10" height="20" fill="#336699" transform="rotate(225)"/>
        <rect x="-5" y="-50" width="10" height="20" fill="#336699" transform="rotate(270)"/>
        <rect x="-5" y="-50" width="10" height="20" fill="#336699" transform="rotate(315)"/>

        <!-- Center hole -->
        <circle r="10" fill="white"/>
    </g>
</svg>
```

### Star Pattern with Rotation

```html
<svg width="300pt" height="300pt">
    <g transform="translate(150, 150)">
        <polygon points="0,-40 10,-10 40,-10 15,10 25,40 0,20 -25,40 -15,10 -40,-10 -10,-10"
                 fill="gold" stroke="orange" stroke-width="2" transform="rotate(0)"/>
        <polygon points="0,-40 10,-10 40,-10 15,10 25,40 0,20 -25,40 -15,10 -40,-10 -10,-10"
                 fill="gold" stroke="orange" stroke-width="2" transform="rotate(72)" opacity="0.8"/>
        <polygon points="0,-40 10,-10 40,-10 15,10 25,40 0,20 -25,40 -15,10 -40,-10 -10,-10"
                 fill="gold" stroke="orange" stroke-width="2" transform="rotate(144)" opacity="0.6"/>
        <polygon points="0,-40 10,-10 40,-10 15,10 25,40 0,20 -25,40 -15,10 -40,-10 -10,-10"
                 fill="gold" stroke="orange" stroke-width="2" transform="rotate(216)" opacity="0.4"/>
        <polygon points="0,-40 10,-10 40,-10 15,10 25,40 0,20 -25,40 -15,10 -40,-10 -10,-10"
                 fill="gold" stroke="orange" stroke-width="2" transform="rotate(288)" opacity="0.2"/>
    </g>
</svg>
```

### Dynamic Gauge Needle

```html
<!-- Model: { gaugeValue: 75, minValue: 0, maxValue: 100 } -->
<svg width="300pt" height="200pt">
    <!-- Gauge background -->
    <path d="M 50,180 A 100,100 0 0,1 250,180" fill="none" stroke="#e0e0e0" stroke-width="20" stroke-linecap="round"/>
    <path d="M 50,180 A 100,100 0 0,1 250,180" fill="none" stroke="#4a90e2" stroke-width="20"
          stroke-linecap="round" stroke-dasharray="314" stroke-dashoffset="{{314 - (314 * model.gaugeValue / 100)}}"/>

    <!-- Needle (rotates based on value: -90° to 90°) -->
    <g transform="translate(150, 180)">
        <line x1="0" y1="0" x2="0" y2="-80" stroke="red" stroke-width="3"
              transform="rotate({{-90 + (180 * model.gaugeValue / 100)}})" stroke-linecap="round"/>
        <circle r="8" fill="red"/>
    </g>

    <!-- Value text -->
    <text x="150" y="190" text-anchor="middle" font-size="24" font-weight="bold" fill="#336699">
        {{model.gaugeValue}}%
    </text>
</svg>
```

### Circular Menu Items

```html
<!-- Model: { menuItems: [{label:"Home", angle:0}, {label:"About", angle:60}] } -->
<svg width="400pt" height="400pt">
    <g transform="translate(200, 200)">
        <!-- Center circle -->
        <circle r="40" fill="#336699"/>
        <text y="5" text-anchor="middle" fill="white" font-size="14">Menu</text>

        <!-- Menu items in circle -->
        <template data-bind="{{model.menuItems}}">
            <g transform="rotate({{.angle}})">
                <rect x="60" y="-20" width="80" height="40" rx="5" fill="#4a90e2" stroke="#336699" stroke-width="2"/>
                <text x="100" y="5" text-anchor="middle" fill="white" font-size="12" font-weight="bold">
                    {{.label}}
                </text>
            </g>
        </template>
    </g>
</svg>
```

### Isometric Cube

```html
<svg width="300pt" height="300pt">
    <g transform="translate(150, 150)">
        <!-- Top face -->
        <polygon points="0,-50 50,-25 0,0 -50,-25" fill="#6699cc" stroke="#336699" stroke-width="2"
                 transform="translate(0, -40)"/>

        <!-- Left face -->
        <polygon points="-50,-25 -50,35 0,60 0,0" fill="#4a7ba7" stroke="#336699" stroke-width="2"
                 transform="translate(0, -40)"/>

        <!-- Right face -->
        <polygon points="50,-25 50,35 0,60 0,0" fill="#7fb3d5" stroke="#336699" stroke-width="2"
                 transform="translate(0, -40)"/>
    </g>
</svg>
```

### Rotating Fan Blades

```html
<!-- Model: { fanRotation: 45 } -->
<svg width="300pt" height="300pt">
    <g transform="translate(150, 150) rotate({{model.fanRotation}})">
        <!-- 4 blades -->
        <ellipse cx="40" cy="0" rx="50" ry="15" fill="#336699" opacity="0.8" transform="rotate(0)"/>
        <ellipse cx="40" cy="0" rx="50" ry="15" fill="#336699" opacity="0.8" transform="rotate(90)"/>
        <ellipse cx="40" cy="0" rx="50" ry="15" fill="#336699" opacity="0.8" transform="rotate(180)"/>
        <ellipse cx="40" cy="0" rx="50" ry="15" fill="#336699" opacity="0.8" transform="rotate(270)"/>

        <!-- Center hub -->
        <circle r="15" fill="#254a70"/>
        <circle r="5" fill="#ccc"/>
    </g>
</svg>
```

### Progress Wheel

```html
<!-- Model: { progress: 65, totalSections: 8 } -->
<svg width="300pt" height="300pt">
    <g transform="translate(150, 150)">
        <template data-bind="{{range(0, model.totalSections)}}">
            <rect x="-8" y="-95" width="16" height="35" rx="8"
                  fill="{{. < (model.totalSections * model.progress / 100) ? '#50c878' : '#e0e0e0'}}"
                  transform="rotate({{. * 360 / model.totalSections}})"/>
        </template>

        <!-- Center display -->
        <circle r="60" fill="white" stroke="#e0e0e0" stroke-width="2"/>
        <text y="5" text-anchor="middle" font-size="32" font-weight="bold" fill="#50c878">
            {{model.progress}}%
        </text>
    </g>
</svg>
```

### Arrow Pointer

```html
<!-- Model: { direction: 135 } -->
<svg width="200pt" height="200pt">
    <g transform="translate(100, 100)">
        <!-- Direction indicator -->
        <circle r="80" fill="none" stroke="#e0e0e0" stroke-width="2"/>
        <text y="-90" text-anchor="middle" font-size="12">N</text>
        <text x="90" y="5" text-anchor="middle" font-size="12">E</text>
        <text y="100" text-anchor="middle" font-size="12">S</text>
        <text x="-90" y="5" text-anchor="middle" font-size="12">W</text>

        <!-- Arrow pointing in direction -->
        <g transform="rotate({{model.direction}})">
            <polygon points="0,-60 -15,-30 -5,-30 -5,40 5,40 5,-30 15,-30"
                     fill="#ff6347" stroke="#cc3333" stroke-width="2"/>
        </g>
    </g>
</svg>
```

### Scale Comparison

```html
<!-- Model: { items: [{name:"Small", scale:0.5}, {name:"Medium", scale:1}, {name:"Large", scale:1.8}] } -->
<svg width="500pt" height="200pt">
    <template data-bind="{{model.items}}">
        <g transform="translate({{80 + $index * 150}}, 100)">
            <rect x="-30" y="-30" width="60" height="60" fill="#4a90e2" stroke="#336699" stroke-width="2"
                  transform="scale({{.scale}})"/>
            <text y="70" text-anchor="middle" font-size="14" font-weight="bold">{{.name}}</text>
            <text y="88" text-anchor="middle" font-size="11" fill="#666">{{.scale}}x</text>
        </g>
    </template>
</svg>
```

### Skewed Perspective Box

```html
<svg width="400pt" height="300pt">
    <g transform="translate(200, 150)">
        <!-- Front face -->
        <rect x="-80" y="-60" width="160" height="120" fill="#6699cc" stroke="#336699" stroke-width="2"/>

        <!-- Top face (skewed) -->
        <rect x="-80" y="-60" width="160" height="40" fill="#4a7ba7" stroke="#336699" stroke-width="2"
              transform="skewY(-30) translate(0, -35)"/>

        <!-- Right face (skewed) -->
        <rect x="80" y="-60" width="40" height="120" fill="#7fb3d5" stroke="#336699" stroke-width="2"
              transform="skewX(30) translate(35, 0)"/>
    </g>
</svg>
```

### Matrix Transformation Example

```html
<svg width="300pt" height="200pt">
    <!-- Original -->
    <rect x="50" y="75" width="50" height="50" fill="blue" opacity="0.3"/>

    <!-- Matrix: translate(100, 0) -->
    <rect x="50" y="75" width="50" height="50" fill="blue" opacity="0.5"
          transform="matrix(1, 0, 0, 1, 100, 0)"/>

    <!-- Matrix: translate(200, 0) + rotate(45°) approximation -->
    <rect x="50" y="75" width="50" height="50" fill="blue"
          transform="matrix(0.707, 0.707, -0.707, 0.707, 200, 0)"/>
</svg>
```

---

## See Also

- [transform-origin](/reference/svgattributes/transform-origin.html) - Transform origin point
- [g](/reference/svgtags/g.html) - Group element for transforming multiple elements
- [svg](/reference/svgtags/svg.html) - SVG canvas container
- [matrix](/reference/svgattributes/matrix.html) - Matrix transformation attribute
- [Data Binding](/reference/binding/) - Dynamic data binding
- [SVG Shapes](/reference/svgtags/shapes.html) - rect, circle, ellipse, line, etc.

---
