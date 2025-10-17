---
layout: default
title: fy
parent: SVG Attributes
parent_url: /reference/svgattributes/
grand_parent: Template reference
grand_parent_url: /reference/
has_children: false
has_toc: false
---

# @fy : The Radial Gradient Focal Point Y Coordinate Attribute

The `fy` attribute defines the Y-coordinate of the focal point for a radial gradient. Working together with `fx`, it positions where the 0% color stop begins vertically, enabling creation of directional lighting effects, depth perception, and realistic 3D appearances in SVG graphics.

## Usage

The `fy` attribute is used to:
- Define the vertical position of the radial gradient's focal point (0% stop)
- Create top/bottom lighting effects by offsetting from center vertically
- Support both relative (percentage) and absolute (unit-based) coordinates
- Work with both objectBoundingBox (default) and userSpaceOnUse coordinate systems
- Enable data-driven lighting positions through data binding
- Simulate realistic shadows and highlights
- Position vertical lighting for UI elements and data visualizations

```html
<svg xmlns="http://www.w3.org/2000/svg" width="400" height="400">
    <defs>
        <radialGradient id="topLight" cx="50%" cy="50%" r="50%" fx="50%" fy="25%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="100%" stop-color="#2c3e50" />
        </radialGradient>
    </defs>
    <circle cx="200" cy="200" r="180" fill="url(#topLight)" />
</svg>
```

---

## Supported Values

| Value Type | Format | Description | Example |
|------------|--------|-------------|---------|
| Percentage | `0%` to `100%` | Relative to shape bounding box (objectBoundingBox mode) | `fy="30%"` |
| Decimal | `0` to `1.0` | Relative coordinate (0=top, 1=bottom) | `fy="0.3"` |
| Length Units | `pt`, `px`, `mm`, `cm`, `in` | Absolute coordinates (userSpaceOnUse mode) | `fy="100pt"` |
| Default | `cy` value | Inherits from cy if omitted (centered focal point) | - |

### Common Patterns

```html
<!-- Centered focal point (default) -->
<radialGradient id="g1" cx="50%" cy="50%" r="50%">

<!-- Top lighting -->
<radialGradient id="g2" cx="50%" cy="50%" r="50%" fx="50%" fy="25%">

<!-- Bottom lighting -->
<radialGradient id="g3" cx="50%" cy="50%" r="50%" fx="50%" fy="75%">

<!-- Extreme top offset -->
<radialGradient id="g4" cx="50%" cy="50%" r="50%" fx="50%" fy="10%">
```

---

## Supported Elements

The `fy` attribute is supported on:

- **[&lt;radialGradient&gt;](/reference/svgtags/radialGradient.html)** - Defines radial gradient focal point Y position

Note: This attribute is NOT used with `<linearGradient>` elements.

---

## Data Binding

### Dynamic Vertical Lighting Position

Bind focal point Y coordinate to data:

```html
<!-- Model: { lightY: 30 } -->
<svg width="300" height="300">
    <defs>
        <radialGradient id="dynamicY"
                        cx="50%" cy="50%" r="50%"
                        fx="50%" fy="{{model.lightY}}%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="50%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </radialGradient>
    </defs>
    <circle cx="150" cy="150" r="130" fill="url(#dynamicY)" />
</svg>
```

### Height-Based Lighting Position

Position lighting based on container dimensions:

```html
<!-- Model: { containerHeight: 400, lightPosition: 150 } -->
<svg width="400" height="{{model.containerHeight}}">
    <defs>
        <radialGradient id="heightLight"
                        cx="50%"
                        cy="50%"
                        r="60%"
                        fx="50%"
                        fy="{{(model.lightPosition / model.containerHeight) * 100}}%">
            <stop offset="0%" stop-color="#ffffff" stop-opacity="0.9" />
            <stop offset="100%" stop-color="#2c3e50" />
        </radialGradient>
    </defs>
    <rect width="400" height="{{model.containerHeight}}" fill="url(#heightLight)" />
</svg>
```

### Data-Driven Vertical Direction

Change vertical lighting based on data state:

```html
<!-- Model: { lightDirection: "top", intensity: 0.9 } -->
<svg width="200" height="200">
    <defs>
        <radialGradient id="verticalLight"
                        cx="50%" cy="50%" r="50%"
                        fx="50%"
                        fy="{{model.lightDirection === 'top' ? '25%' :
                              model.lightDirection === 'bottom' ? '75%' : '50%'}}">
            <stop offset="0%" stop-color="#ffffff" stop-opacity="{{model.intensity}}" />
            <stop offset="100%" stop-color="#3498db" />
        </radialGradient>
    </defs>
    <circle cx="100" cy="100" r="80" fill="url(#verticalLight)" />
</svg>
```

### Time-Based Sun Position

Simulate sun position based on time of day:

```html
<!-- Model: { hourOfDay: 14, sunriseHour: 6, sunsetHour: 20 } -->
<svg width="600" height="400">
    <defs>
        <radialGradient id="sunPosition"
                        cx="50%"
                        cy="{{((model.hourOfDay - model.sunriseHour) /
                              (model.sunsetHour - model.sunriseHour)) * 100}}%"
                        r="70%"
                        fx="50%"
                        fy="{{((model.hourOfDay - model.sunriseHour) /
                              (model.sunsetHour - model.sunriseHour)) * 100}}%">
            <stop offset="0%" stop-color="#ffd93d" />
            <stop offset="50%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e67e22" />
        </radialGradient>
    </defs>
    <rect width="600" height="400" fill="url(#sunPosition)" />
</svg>
```

### Scroll-Based Lighting Effect

Change lighting position based on scroll or progress:

```html
<!-- Model: { scrollPercentage: 45 } -->
<svg width="400" height="600">
    <defs>
        <radialGradient id="scrollLight"
                        cx="50%"
                        cy="50%"
                        r="60%"
                        fx="50%"
                        fy="{{model.scrollPercentage}}%">
            <stop offset="0%" stop-color="#ffffff" stop-opacity="0.8" />
            <stop offset="60%" stop-color="#95a5a6" stop-opacity="0.3" />
            <stop offset="100%" stop-color="#2c3e50" stop-opacity="0.9" />
        </radialGradient>
    </defs>
    <rect width="400" height="600" fill="url(#scrollLight)" />
</svg>
```

---

## Notes

### Focal Point Vertical Positioning

The `fy` attribute controls vertical focal point placement:
- Default: matches gradient center vertically (fy=cy)
- Lower values (fy < cy): Light from above, shadow below
- Higher values (fy > cy): Light from below, shadow above
- Extreme values: Dramatic lighting effects

### Visual Effects by Position

Different fy values create specific lighting effects:

1. **fy = 25% (Top Lighting)**: Natural overhead lighting, top highlight
2. **fy = 50% (Centered)**: Symmetrical, even gradient
3. **fy = 75% (Bottom Lighting)**: Under-lighting, dramatic effect
4. **fy < 20% (Extreme Top)**: Strong top highlight, deep bottom shadow
5. **fy > 80% (Extreme Bottom)**: Bottom glow, top shadow

### Coordinate Systems

The `fy` attribute behaves differently based on `gradientUnits`:

1. **objectBoundingBox (default)**: Relative to filled shape
   - Values 0-1 or 0%-100%
   - `0%` = top edge, `50%` = vertical center, `100%` = bottom edge
   - Gradient scales automatically with shape size

2. **userSpaceOnUse**: Absolute document coordinates
   - Values in pt, px, mm, cm, in
   - Fixed position in document space
   - Consistent across multiple shapes

### Common Use Cases

Vertical focal point positioning is ideal for:
- Overhead lighting effects (buttons, badges)
- Under-lighting for dramatic effects
- Sun/moon position simulation
- Depth perception in gauges and meters
- Stage lighting effects
- Data visualization highlights
- Time-based lighting changes
- Scroll-interactive backgrounds

### Combining fx and fy

Using both fx and fy creates diagonal lighting:

```html
<!-- Top-left corner lighting -->
<radialGradient fx="30%" fy="30%">

<!-- Bottom-right corner lighting -->
<radialGradient fx="70%" fy="70%">

<!-- Top-right corner lighting -->
<radialGradient fx="70%" fy="30%">

<!-- Bottom-left corner lighting -->
<radialGradient fx="30%" fy="70%">
```

---

## Examples

### Basic Top Lighting

```html
<svg width="200" height="200">
    <defs>
        <radialGradient id="topLight" cx="50%" cy="50%" r="50%" fx="50%" fy="25%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="50%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2c3e50" />
        </radialGradient>
    </defs>
    <circle cx="100" cy="100" r="80" fill="url(#topLight)" />
</svg>
```

### Bottom Lighting Effect

```html
<svg width="200" height="200">
    <defs>
        <radialGradient id="bottomLight" cx="50%" cy="50%" r="50%" fx="50%" fy="75%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="50%" stop-color="#e74c3c" />
            <stop offset="100%" stop-color="#c0392b" />
        </radialGradient>
    </defs>
    <circle cx="100" cy="100" r="80" fill="url(#bottomLight)" />
</svg>
```

### Centered Focal Point (Default)

```html
<svg width="200" height="200">
    <defs>
        <radialGradient id="centered" cx="50%" cy="50%" r="50%" fx="50%" fy="50%">
            <stop offset="0%" stop-color="#f39c12" />
            <stop offset="100%" stop-color="#e74c3c" />
        </radialGradient>
    </defs>
    <circle cx="100" cy="100" r="80" fill="url(#centered)" />
</svg>
```

### Button with Overhead Highlight

```html
<svg width="220" height="80">
    <defs>
        <radialGradient id="buttonTop" cx="50%" cy="50%" r="50%" fx="50%" fy="30%">
            <stop offset="0%" stop-color="#5dade2" />
            <stop offset="40%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2874a6" />
        </radialGradient>
    </defs>

    <rect width="220" height="80" rx="40" fill="url(#buttonTop)" />
    <text x="110" y="50" text-anchor="middle" fill="white" font-size="24" font-weight="bold">
        Submit
    </text>
</svg>
```

### Badge with Top Shine

```html
<svg width="100" height="100">
    <defs>
        <radialGradient id="badgeShine" cx="50%" cy="50%" r="50%" fx="50%" fy="30%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="30%" stop-color="#f1c40f" />
            <stop offset="100%" stop-color="#e67e22" />
        </radialGradient>
    </defs>

    <circle cx="50" cy="50" r="40" fill="url(#badgeShine)" />
    <text x="50" y="58" text-anchor="middle" fill="white" font-size="24" font-weight="bold">
        9
    </text>
</svg>
```

### Gauge with Top Highlight

```html
<svg width="300" height="300">
    <defs>
        <radialGradient id="gaugeTop" cx="50%" cy="50%" r="50%" fx="50%" fy="35%">
            <stop offset="0%" stop-color="#ecf0f1" />
            <stop offset="70%" stop-color="#bdc3c7" />
            <stop offset="100%" stop-color="#95a5a6" />
        </radialGradient>
    </defs>

    <circle cx="150" cy="150" r="120" fill="url(#gaugeTop)" />
    <circle cx="150" cy="150" r="90" fill="white" />
</svg>
```

### Status Light with Top Glow

```html
<svg width="100" height="100">
    <defs>
        <radialGradient id="statusTop" cx="50%" cy="50%" r="50%" fx="50%" fy="35%">
            <stop offset="0%" stop-color="#a8e6cf" />
            <stop offset="50%" stop-color="#2ecc71" />
            <stop offset="100%" stop-color="#27ae60" />
        </radialGradient>
    </defs>

    <circle cx="50" cy="50" r="35" fill="url(#statusTop)" />
</svg>
```

### Sky Gradient with Sun Position

```html
<svg width="600" height="400">
    <defs>
        <radialGradient id="sky" cx="50%" cy="30%" r="70%" fx="50%" fy="20%">
            <stop offset="0%" stop-color="#ffd93d" />
            <stop offset="30%" stop-color="#89cff0" />
            <stop offset="70%" stop-color="#3498db" />
            <stop offset="100%" stop-color="#2980b9" />
        </radialGradient>
    </defs>

    <rect width="600" height="400" fill="url(#sky)" />
</svg>
```

### Under-lit Dramatic Effect

```html
<svg width="300" height="300">
    <defs>
        <radialGradient id="underlit" cx="50%" cy="50%" r="50%" fx="50%" fy="80%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="40%" stop-color="#e67e22" />
            <stop offset="100%" stop-color="#d35400" />
        </radialGradient>
    </defs>

    <circle cx="150" cy="150" r="120" fill="url(#underlit)" />
</svg>
```

### Spotlight from Above

```html
<svg width="600" height="400">
    <defs>
        <radialGradient id="spotTop" cx="50%" cy="30%" r="60%" fx="50%" fy="20%">
            <stop offset="0%" stop-color="#ffffff" stop-opacity="0.9" />
            <stop offset="50%" stop-color="#95a5a6" stop-opacity="0.4" />
            <stop offset="100%" stop-color="#2c3e50" stop-opacity="1" />
        </radialGradient>
    </defs>

    <rect width="600" height="400" fill="url(#spotTop)" />
</svg>
```

### Icon with Top Highlight

```html
<svg width="80" height="80" viewBox="0 0 24 24">
    <defs>
        <radialGradient id="iconTop" cx="50%" cy="50%" r="50%" fx="50%" fy="30%">
            <stop offset="0%" stop-color="#ffd93d" />
            <stop offset="100%" stop-color="#f39c12" />
        </radialGradient>
    </defs>

    <circle cx="12" cy="12" r="10" fill="url(#iconTop)" />
</svg>
```

### Pie Chart Segment with Depth

```html
<svg width="300" height="300">
    <defs>
        <radialGradient id="pieDepth" cx="50%" cy="50%" r="50%" fx="50%" fy="40%">
            <stop offset="0%" stop-color="#5dade2" />
            <stop offset="100%" stop-color="#2874a6" />
        </radialGradient>
    </defs>

    <path d="M 150,150 L 150,30 A 120,120 0 0,1 270,150 Z" fill="url(#pieDepth)" />
</svg>
```

### Progress Circle with Shine

```html
<svg width="200" height="200">
    <defs>
        <radialGradient id="progressTop" cx="50%" cy="50%" r="50%" fx="50%" fy="35%">
            <stop offset="0%" stop-color="#ecf0f1" />
            <stop offset="100%" stop-color="#bdc3c7" />
        </radialGradient>
    </defs>

    <circle cx="100" cy="100" r="80" fill="url(#progressTop)" />
    <circle cx="100" cy="100" r="60" fill="white" />
</svg>
```

### Data Point with Top Glow

```html
<svg width="300" height="300">
    <defs>
        <radialGradient id="pointGlow" cx="50%" cy="50%" r="50%" fx="50%" fy="40%">
            <stop offset="0%" stop-color="#3498db" stop-opacity="1" />
            <stop offset="60%" stop-color="#3498db" stop-opacity="0.4" />
            <stop offset="100%" stop-color="#3498db" stop-opacity="0" />
        </radialGradient>
    </defs>

    <circle cx="150" cy="150" r="80" fill="url(#pointGlow)" />
    <circle cx="150" cy="150" r="25" fill="#3498db" />
</svg>
```

### Data-Driven Vertical Position

```html
<!-- Model: { verticalPos: 35, color: "#3498db" } -->
<svg width="250" height="250">
    <defs>
        <radialGradient id="dataVertical"
                        cx="50%" cy="50%" r="50%"
                        fx="50%" fy="{{model.verticalPos}}%">
            <stop offset="0%" stop-color="#ffffff" />
            <stop offset="50%" stop-color="{{model.color}}" />
            <stop offset="100%" stop-color="#2c3e50" />
        </radialGradient>
    </defs>

    <circle cx="125" cy="125" r="100" fill="url(#dataVertical)" />
</svg>
```

---

## See Also

- [fx](/reference/svgattributes/fx.html) - Radial gradient focal point X coordinate
- [cx](/reference/svgattributes/cx.html) - Radial gradient center X coordinate
- [cy](/reference/svgattributes/cy.html) - Radial gradient center Y coordinate
- [r](/reference/svgattributes/r.html) - Radial gradient radius
- [fr](/reference/svgattributes/fr.html) - Radial gradient focal radius
- [radialGradient](/reference/svgtags/radialGradient.html) - Radial gradient definition element
- [gradientUnits](/reference/svgattributes/gradientUnits.html) - Coordinate system mode
- [stop](/reference/svgtags/stop.html) - Gradient color stop element
- [Data Binding](/reference/binding/) - Data binding and expressions

---
